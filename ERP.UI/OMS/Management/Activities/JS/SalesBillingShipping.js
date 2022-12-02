var _customerAddress;
var BillingAddress = [];
var BillingOrShipping;
var returnObject;
var shipObject;
var AllowAddressShipToPartyState = true;

var RequiredShipToPartyValue;
var PlaceOfSupplyShipPartyValue;
//Rev work start 06.07.2022 mantise no:25008
var PlaceofSupplyVal;
//Rev work close 06.07.2022 mantise no:25008
function GetCustomerAddress(CustomerId) {

    if ((CustomerId != null) && (CustomerId != "")) {

        $.ajax({
            type: "POST",
            url: "Services/BillingShipping.asmx/FetchCustomAddress",
            data: JSON.stringify({ CustomerId: CustomerId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                _customerAddress = msg.d;
                                    }
              });
                                                     }
                                         }







//Customer Details By CustomerId(add_cntId)

function SetDefaultBillingShippingAddress(CustomerId) {
    var OtherDetails = {}
    OtherDetails.CustomerId = CustomerId;
    $.ajax({
        type: "POST",
        url: "Services/BillingShipping.asmx/FetchCustomAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            BillingAddress = msg.d;
            CustomerAddressLoad(BillingAddress);
            

            var BillingObj = $.grep(BillingAddress, function (e) { return e.Type == "Billing" && e.Isdefault == 1; })
            var ShippingObj = $.grep(BillingAddress, function (e) { return e.Type == "Shipping" && e.Isdefault == 1; })

         
            if (BillingObj.length > 0 && ShippingObj.length > 0) {
                cbtnSave_SalesBillingShiping.Focus();
            }

            if (ShippingObj.length == 0) {
                $('#shiptosame').focus();
            }
            else {
                cbtnSave_SalesBillingShiping.Focus();


            }

            if (BillingObj.length == 0) {
                $('#billtoSame').focus();
            }
            else {
                cbtnSave_SalesBillingShiping.Focus();
            }

 }
    });

}
//Rev work start 06.07.2022 mantise no:25008
function Getshhippingval(PosGstId) {
    PlaceofSupplyVal = PosGstId;
}
//Rev work close 06.07.2022 mantise no:25008
function SetDefaultSegmentBillingShippingAddress(CustomerId, Entity_Segment_ID) {
    var OtherDetails = {}
    OtherDetails.CustomerId = CustomerId;
    OtherDetails.Entity_Segment_ID = Entity_Segment_ID;
    $.ajax({
        type: "POST",
        url: "Services/BillingShipping.asmx/FetchSegmentAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            BillingAddress = msg.d;
            CustomerAddressLoad(BillingAddress);

            var BillingObj = $.grep(BillingAddress, function (e) { return e.Type == "Billing" && e.Isdefault == 1; })
            var ShippingObj = $.grep(BillingAddress, function (e) { return e.Type == "Shipping" && e.Isdefault == 1; })


            if (BillingObj.length > 0 && ShippingObj.length > 0) {
                cbtnSave_SalesBillingShiping.Focus();
            }
            if (ShippingObj.length == 0) {
                $('#shiptosame').focus();
            }
            else {
                cbtnSave_SalesBillingShiping.Focus();
            }
            if (BillingObj.length == 0) {
                $('#billtoSame').focus();
            }
            else {
                cbtnSave_SalesBillingShiping.Focus();
            }
        }
    });

}
function LoadtBillingShippingCustomerAddress(CustomerId) {
    var OtherDetails = {}
    OtherDetails.CustomerId = CustomerId;
    $.ajax({
        type: "POST",
        url: "Services/BillingShipping.asmx/FetchCustomAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            BillingAddress = msg.d;  
        }
    }); 

}

function LoadtBillingShippingShipTopartyAddress() {
    $('#ShippingShipToPartyModel').modal('hide');

    $.ajax({
        type: "POST",
        url: "Services/BillingShipping.asmx/FetchCustomAddress",
        data: JSON.stringify({ CustomerId: $("#hdShipToParty").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            shipObject = msg.d; 
}
    });
}

//Customer Billing Details by Pin
function CustomerBillingDetailsByPin() {

    var detailsByPin = ctxtbillingPin.GetText().trim();
    if (detailsByPin != '') {
        //var details = {}

        //details.PinCode = detailsByPin;
        $.ajax({
            type: "POST",
            url: "Services/BillingShipping.asmx/CustomAddressByPin",
            data: JSON.stringify({ PinCode: detailsByPin }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var obj = msg.d;
                var returnObj = obj[0];

                //Billing
                if (returnObj) {

                    if ($('#hdStateCodeBilling').val() != returnObj.StateCode && AllowAddressShipToPartyState == false) {
                        //Rev work start 06.07.2022 mantise no:25008
                        //ctxtbillingPin.SetText($('#hdOldBillingPinCode').val());
                        //jAlert('You may change the Shipping/Billing details except PIN/State.');
                        var currentURL = window.location.href;
                        if ((currentURL.includes("SalesQuotation.aspx") || currentURL.includes("SalesOrderAdd.aspx")))
                        {
                            if(PlaceofSupplyVal=="B")
                            {
                                ctxtbillingPin.SetText($('#hdOldBillingPinCode').val());
                                jAlert('You may change the Shipping/Billing details except PIN/State.');
                            }
                            else
                            {
                                $('#hdBillingPin').val(returnObj.PinId);
                                if ((ctxtbillingPin.SetText(returnObj.PinCode)) != '' || (ctxtbillingPin.SetText(returnObj.PinCode)) != "0") {
                                    ctxtbillingPin.SetText(returnObj.PinCode);
                                    document.getElementById("bpin").style.display = "none";
                                }
                                else {
                                    document.getElementById("bpin").style.display = "block";
                                }

                                $('#hdCountryIdBilling').val(returnObj.CountryId);

                                if (ctxtbillingCountry.SetText(returnObj.CountryName) != '') {
                                    ctxtbillingCountry.SetText(returnObj.CountryName);
                                    document.getElementById("billingcountry").style.display = "none";
                                }
                                else {
                                    document.getElementById("billingcountry").style.display = "block";
                                }
                                $('#hdStateIdBilling').val(returnObj.StateId);

                                if (ctxtbillingState.SetText(returnObj.StateName) != '') {
                                    ctxtbillingState.SetText(returnObj.StateName);
                                    document.getElementById("billingstate").style.display = "none";
                                }
                                else {
                                    document.getElementById("billingstate").style.display = "block";
                                }
                                $('#hdStateCodeBilling').val(returnObj.StateCode);
                                if (ctxtbillingCity.SetText(returnObj.CityName) != '') {
                                    ctxtbillingCity.SetText(returnObj.CityName);
                                    document.getElementById("billingcity").style.display = "none";
                                }
                                else {
                                    document.getElementById("billingcity").style.display = "block";
                                }
                                $('#hdCityIdBilling').val(returnObj.CityId);
                                ctxtSelectBillingArea.SetText('');
                                var htmlScript = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">id</th><th>Area</th> </tr>";

                                document.getElementById('billingAreatable').innerHTML = htmlScript;
                            }
                        }
                        else
                        {
                            ctxtbillingPin.SetText($('#hdOldBillingPinCode').val());
                            jAlert('You may change the Shipping/Billing details except PIN/State.');
                        }
                        //Rev work close 06.07.2022 mantise no:25008
                    }
                    else {
                        $('#hdBillingPin').val(returnObj.PinId);
                        if ((ctxtbillingPin.SetText(returnObj.PinCode)) != '' || (ctxtbillingPin.SetText(returnObj.PinCode))!="0")
                        {
                            ctxtbillingPin.SetText(returnObj.PinCode);
                            document.getElementById("bpin").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("bpin").style.display = "block";
                        }

                        $('#hdCountryIdBilling').val(returnObj.CountryId);

                        if (ctxtbillingCountry.SetText(returnObj.CountryName) != '') {
                            ctxtbillingCountry.SetText(returnObj.CountryName);
                            document.getElementById("billingcountry").style.display = "none";
                        }
                        else {
                            document.getElementById("billingcountry").style.display = "block";
                        }
                        $('#hdStateIdBilling').val(returnObj.StateId);

                        if (ctxtbillingState.SetText(returnObj.StateName) != '') {
                            ctxtbillingState.SetText(returnObj.StateName);
                            document.getElementById("billingstate").style.display = "none";
                        }
                        else {

                            document.getElementById("billingstate").style.display = "block";
                        }
                        $('#hdStateCodeBilling').val(returnObj.StateCode);
                        if (ctxtbillingCity.SetText(returnObj.CityName) != '') {
                            ctxtbillingCity.SetText(returnObj.CityName);
                            document.getElementById("billingcity").style.display = "none";
                        }
                        else {
                            document.getElementById("billingcity").style.display = "block";
                        }
                        $('#hdCityIdBilling').val(returnObj.CityId);
                        ctxtSelectBillingArea.SetText('');
                        var htmlScript = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">id</th><th>Area</th> </tr>";

                        document.getElementById('billingAreatable').innerHTML = htmlScript;
                    }
                    //else if ($('#hdStateCodeBilling').val() != returnObj.StateCode)
                    //{

                    //    ctxtbillingPin.SetText($('#hdOldBillingPinCode').val());
                    //    jAlert('Existing state and Entering state must be same.');
                    //}
                }
                else {

                    // ctxtbillingPin.SetText('');
                    $('#hdCountryIdBilling').val('');
                    ctxtbillingCountry.SetText('');
                    $('#hdStateIdBilling').val('');
                    ctxtbillingState.SetText('');
                    $('#hdStateCodeBilling').val('');
                    ctxtbillingCity.SetText('');
                    $('#hdCityIdBilling').val('');
                    ctxtSelectBillingArea.SetText('');
                }
            }
        });
}

}

//Customer Shipping Details By Pin

function CustomerShippingDetailsByPin() {

    var detailsByPin = ctxtShippingPin.GetText().trim();
    if (detailsByPin != '') {

        $.ajax({
            type: "POST",
            url: "Services/BillingShipping.asmx/CustomAddressByPin",
            data: JSON.stringify({ PinCode: detailsByPin }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var obj = msg.d;
                var returnObj = obj[0];

                //Shipping
                if (returnObj) {

                    if ($('#hdStateCodeShipping').val() != returnObj.StateCode && AllowAddressShipToPartyState == false)
                    {
                        if ($('#hddSOTaggORNot').val() == "")
                        {
                            //Rev work start 06.07.2022 mantise no:25008
                            //ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
                            //jAlert('You may change the Shipping/Billing details except PIN/State.');                            
                            var currentURL = window.location.href;
                            if ((currentURL.includes("SalesQuotation.aspx")) || currentURL.includes("SalesOrderAdd.aspx"))
                              {
                                if (PlaceofSupplyVal=="S")
                                {
                                    ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
                                    jAlert('You may change the Shipping/Billing details except PIN/State.');
                                }
                                else
                                {
                                    $('#hdShippingPin').val(returnObj.PinId);
                                    if ((ctxtShippingPin.SetText(returnObj.PinCode)) != '' || (ctxtShippingPin.SetText(returnObj.PinCode)) != "0") {
                                        ctxtShippingPin.SetText(returnObj.PinCode);
                                        document.getElementById("spin").style.display = "none";
                                    }
                                    else {
                                        document.getElementById("spin").style.display = "block";
                                    }
                                    $('#hdCountryIdShipping').val(returnObj.CountryId);
                                    if (ctxtshippingCountry.SetText(returnObj.CountryName) != '') {
                                        ctxtshippingCountry.SetText(returnObj.CountryName);
                                        document.getElementById("shippingcountry").style.display = "none";
                                    }
                                    else {
                                        document.getElementById("shippingcountry").style.display = "block";
                                    }

                                    $('#hdStateIdShipping').val(returnObj.StateId);
                                    if (ctxtshippingState.SetText(returnObj.StateName) != '') {
                                        ctxtshippingState.SetText(returnObj.StateName);
                                        document.getElementById("shippingstate").style.display = "none";
                                    }
                                    else {
                                        document.getElementById("shippingstate").style.display = "block";
                                    }
                                    $('#hdStateCodeShipping').val(returnObj.StateCode);
                                    if (ctxtshippingCity.SetText(returnObj.CityName) != '') {
                                        ctxtshippingCity.SetText(returnObj.CityName);
                                        document.getElementById("shippingcity").style.display = "none";
                                    }
                                    else {
                                        document.getElementById("shippingcity").style.display = "block";
                                    }
                                    $('#hdCityIdShipping').val(returnObj.CityId);
                                    ctxtSelectShippingArea.SetText('');

                                    var htmlScript = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">id</th><th>Area</th> </tr>";

                                    document.getElementById('shippingAreatable').innerHTML = htmlScript;
                                }
                            }
                            else
                            {
                                ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
                                jAlert('You may change the Shipping/Billing details except PIN/State.');
                            }
                            //Rev work close 06.07.2022 mantise no:25008
                        }
                        else if ($('#hddSOTaggORNot').val() != "0.0000") {
                            //Rev work start 06.07.2022 mantise no:25008
                            //ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
                            //jAlert('You may change the Shipping/Billing details except PIN/State.');                            
                            if ((currentURL.includes("SalesQuotation.aspx")))
                            {
                                var currentURL = window.location.href;
                                if (PlaceofSupplyVal == "S") {
                                    ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
                                    jAlert('You may change the Shipping/Billing details except PIN/State.');
                                }
                                else {
                                    $('#hdShippingPin').val(returnObj.PinId);
                                    if ((ctxtShippingPin.SetText(returnObj.PinCode)) != '' || (ctxtShippingPin.SetText(returnObj.PinCode)) != "0") {
                                        ctxtShippingPin.SetText(returnObj.PinCode);
                                        document.getElementById("spin").style.display = "none";
                                    }
                                    else {
                                        document.getElementById("spin").style.display = "block";
                                    }
                                    $('#hdCountryIdShipping').val(returnObj.CountryId);
                                    if (ctxtshippingCountry.SetText(returnObj.CountryName) != '') {
                                        ctxtshippingCountry.SetText(returnObj.CountryName);
                                        document.getElementById("shippingcountry").style.display = "none";
                                    }
                                    else {
                                        document.getElementById("shippingcountry").style.display = "block";
                                    }

                                    $('#hdStateIdShipping').val(returnObj.StateId);
                                    if (ctxtshippingState.SetText(returnObj.StateName) != '') {
                                        ctxtshippingState.SetText(returnObj.StateName);
                                        document.getElementById("shippingstate").style.display = "none";
                                    }
                                    else {
                                        document.getElementById("shippingstate").style.display = "block";
                                    }
                                    $('#hdStateCodeShipping').val(returnObj.StateCode);
                                    if (ctxtshippingCity.SetText(returnObj.CityName) != '') {
                                        ctxtshippingCity.SetText(returnObj.CityName);
                                        document.getElementById("shippingcity").style.display = "none";
                                    }
                                    else {
                                        document.getElementById("shippingcity").style.display = "block";
                                    }
                                    $('#hdCityIdShipping').val(returnObj.CityId);
                                    ctxtSelectShippingArea.SetText('');

                                    var htmlScript = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">id</th><th>Area</th> </tr>";

                                    document.getElementById('shippingAreatable').innerHTML = htmlScript;
                                }
                            }
                            else {
                                ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
                                jAlert('You may change the Shipping/Billing details except PIN/State.');
                            }
                        }
                        else if ($('#hddSOTaggORNot').val() == "0.0000") {

                            $('#hdShippingPin').val(returnObj.PinId);
                            if ((ctxtShippingPin.SetText(returnObj.PinCode)) != '' || (ctxtShippingPin.SetText(returnObj.PinCode)) != "0") {
                                ctxtShippingPin.SetText(returnObj.PinCode);
                                document.getElementById("spin").style.display = "none";
                            }
                            else {
                                document.getElementById("spin").style.display = "block";
                            }
                            $('#hdCountryIdShipping').val(returnObj.CountryId);
                            if (ctxtshippingCountry.SetText(returnObj.CountryName) != '') {
                                ctxtshippingCountry.SetText(returnObj.CountryName);
                                document.getElementById("shippingcountry").style.display = "none";
                            }
                            else {
                                document.getElementById("shippingcountry").style.display = "block";
                            }

                            $('#hdStateIdShipping').val(returnObj.StateId);
                            if (ctxtshippingState.SetText(returnObj.StateName) != '') {
                                ctxtshippingState.SetText(returnObj.StateName);
                                document.getElementById("shippingstate").style.display = "none";
                            }
                            else {
                                document.getElementById("shippingstate").style.display = "block";
                            }
                            $('#hdStateCodeShipping').val(returnObj.StateCode);
                            if (ctxtshippingCity.SetText(returnObj.CityName) != '') {
                                ctxtshippingCity.SetText(returnObj.CityName);
                                document.getElementById("shippingcity").style.display = "none";
                            }
                            else {
                                document.getElementById("shippingcity").style.display = "block";
                            }
                            $('#hdCityIdShipping').val(returnObj.CityId);
                            ctxtSelectShippingArea.SetText('');

                            var htmlScript = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">id</th><th>Area</th> </tr>";

                            document.getElementById('shippingAreatable').innerHTML = htmlScript;
                        }
                      
                    }
                    else
                    {
                        $('#hdShippingPin').val(returnObj.PinId);
                        if ((ctxtShippingPin.SetText(returnObj.PinCode)) != '' || (ctxtShippingPin.SetText(returnObj.PinCode)) !="0")
                        {
                            ctxtShippingPin.SetText(returnObj.PinCode);
                            document.getElementById("spin").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("spin").style.display = "block";
                        }
                        $('#hdCountryIdShipping').val(returnObj.CountryId);
                        if (ctxtshippingCountry.SetText(returnObj.CountryName) != '') {
                            ctxtshippingCountry.SetText(returnObj.CountryName);
                            document.getElementById("shippingcountry").style.display = "none";
                        }
                        else {
                            document.getElementById("shippingcountry").style.display = "block";
                        }

                        $('#hdStateIdShipping').val(returnObj.StateId);
                        if (ctxtshippingState.SetText(returnObj.StateName) != '') {
                            ctxtshippingState.SetText(returnObj.StateName);
                            document.getElementById("shippingstate").style.display = "none";
                        }
                        else {
                            document.getElementById("shippingstate").style.display = "block";
                        }
                        $('#hdStateCodeShipping').val(returnObj.StateCode);
                        if (ctxtshippingCity.SetText(returnObj.CityName) != '') {
                            ctxtshippingCity.SetText(returnObj.CityName);
                            document.getElementById("shippingcity").style.display = "none";
                        }
                        else {
                            document.getElementById("shippingcity").style.display = "block";
                        }
                        $('#hdCityIdShipping').val(returnObj.CityId);
                        ctxtSelectShippingArea.SetText('');

                        var htmlScript = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">id</th><th>Area</th> </tr>";

                        document.getElementById('shippingAreatable').innerHTML = htmlScript;
                    }
                    //else if ($('#hdStateCodeShipping').val() != returnObj.StateCode)
                    //{

                    //    ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
                    //    jAlert('Existing state and Entering state must be same.');
                    //}
                   }
             else {
                    //  ctxtShippingPin.SetText('');
                    $('#hdCountryIdShipping').val('');
                    ctxtshippingCountry.SetText('');
                    $('#hdStateIdShipping').val('');
                    ctxtshippingState.SetText('');
                    //$('#hdStateCodeShipping').val('');
                    ctxtshippingCity.SetText('');
                    $('#hdCityIdShipping').val('');
                    ctxtSelectShippingArea.SetText('');
 }
            }
        });
    }
}

function BillingPinChange() {
    var BBSPin = ctxtbillingPin.GetText();
    if ((BBSPin.length > 0) && (BBSPin.length<=20)) {
        CustomerBillingDetailsByPin();
    }

    //else if (ctxtbillingPin.GetText() == "0") {
    //    ctxtbillingPin.SetText($('#hdOldBillingPinCode').val());
    //    jAlert('You may change the Shipping/Billing details except PIN/State as the Tax is already calculated based on that.');
    //}


    else  {
        //if (BBSPin.length > 20) 
        ctxtbillingCountry.SetText('');
        $('#hdCountryIdBilling').val('');
        ctxtbillingState.SetText('');
        $('#hdStateCodeBilling').val('');
        $('#hdStateIdBilling').val('');
        ctxtbillingCity.SetText('');
        $('#hdCityIdBilling').val('');
 }
}


function ShippingPinChange() {
    var Spin = ctxtShippingPin.GetText();
    if ((Spin.length > 0) && (Spin.length <=20)) {
        CustomerShippingDetailsByPin();
    }

    //else if (ctxtShippingPin.GetText() == "0") {
    //    ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
    //    jAlert('You may change the Shipping/Billing details except PIN/State as the Tax is already calculated based on that.');
    //}
    else if (Spin.length > 20 || Spin.length==0){
        ctxtshippingCountry.SetText('');
        $('#hdCountryIdShipping').val('');
        ctxtshippingState.SetText('');
        //$('#hdStateCodeShipping').val('');
        $('#hdStateIdShipping').val('');
        ctxtshippingCity.SetText('');
        $('#hdCityIdShipping').val('');
    
 }

}


function ShippingPinGotFocus()
{
     $('#hdOldShippingPinCode').val(ctxtShippingPin.GetText());
}

//Ship to same address
function BillingCheckChange() {
 var BPin = ctxtbillingPin.GetText();
 if ((BPin.length > 0) && (BPin.length<=20)) {
        ShipToSameAddress();
    }

    else if (ctxtbillingPin.GetText() == "0") {
        // ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
        jAlert('You may change the Shipping/Billing details except PIN/State.');
    }
 else if (BPin.length >20) {

        ctxtShippingPin.SetText(ctxtbillingPin.GetText());
        $('#hdShippingPin').val($('#hdBillingPin').val());
        ctxtbillingCountry.SetText('');
        $('#hdCountryIdBilling').val('');
        ctxtbillingState.SetText('');
        $('#hdStateCodeBilling').val('');
        $('#hdStateIdBilling').val('');
        ctxtbillingCity.SetText('');
        $('#hdCityIdBilling').val('');
        ctxtshippingCountry.SetText('');
        $('#hdCountryIdShipping').val('');
        ctxtshippingState.SetText('');
        $('#hdStateCodeShipping').val('');
        $('#hdStateIdShipping').val('');
        ctxtshippingCity.SetText('');
        $('#hdCityIdShipping').val('');
    }
}


function ShipToSameAddress() {

    var BillingAddress1 = ctxtAddress1.GetText();
    var BillingAddress2 = ctxtAddress2.GetText();
    var BillingAddress3 = ctxtAddress3.GetText();
    var BillingLandmark = ctxtlandmark.GetText();
    var BillingPin = ctxtbillingPin.GetText();
    var BillingPinId = $('#hdBillingPin').val();
    var BillingCountry = ctxtbillingCountry.GetText();
    var BillingCountryId = $('#hdCountryIdBilling').val();
    var BillingState = ctxtbillingState.GetText();
    var BillingStateId = $('#hdStateIdBilling').val();
    var BillingStateCode = $('#hdStateCodeBilling').val();
    var BillingCity = ctxtbillingCity.GetText();
    var BillingCityId = $('#hdCityIdBilling').val();
    var BillingAreaName = ctxtSelectBillingArea.GetText();
    var BillingAreaId = $('#hdAreaIdBilling').val();
    var BillingDistance = ctxtDistance.GetText();
    var BillingGstin1 = ctxtBillingGSTIN1.GetText();
    var BillingGstin2 = ctxtBillingGSTIN2.GetText();
    var BillingGstin3 = ctxtBillingGSTIN3.GetText();

    var BillContactPerson = ctxtConPerson.GetText(); 
    //var BillPhone = ctxtSalesbillingPhone.GetText();
    var BillPhone = $("#Salesbillingphone").val();
    var BillEmail = ctxtEmail.GetText();


    if (($('#hdStateCodeShipping').val() != BillingStateCode) && AllowAddressShipToPartyState == false)
    {
        jAlert('You may change the Shipping/Billing details except PIN/State.');
    }
    else
        {
        if (ctxtsAddress1.SetText(BillingAddress1) != '')
        {
            ctxtsAddress1.SetText(BillingAddress1);
            document.getElementById("sadd1").style.display = "none";
        }
        else
        {
            document.getElementById("sadd1").style.display = "block";
        }

        //if (ctxtShipPhone.SetText(BillPhone) != '') {
        //    ctxtShipPhone.SetText(BillPhone);
        //    document.getElementById("shippingPhone").style.display = "none";
        //}
        //else {
        //    document.getElementById("shippingPhone").style.display = "block";
        //}



        ctxtsAddress2.SetText(BillingAddress2);
        ctxtsAddress3.SetText(BillingAddress3);
        ctxtslandmark.SetText(BillingLandmark);
        if (ctxtShippingPin.SetText(BillingPin) != '')
        {
            ctxtShippingPin.SetText(BillingPin);
            document.getElementById("spin").style.display = "none";
        }
        else
        {
            document.getElementById("spin").style.display = "block";
        }
        $('#hdShippingPin').val(BillingPinId);
        if (ctxtshippingCountry.SetText(BillingCountry) != '')
        { 
            ctxtshippingCountry.SetText(BillingCountry);
            document.getElementById("shippingcountry").style.display = "none";
        }
        else
        {
            document.getElementById("shippingcountry").style.display = "block";
        }
        $('#hdCountryIdShipping').val(BillingCountryId);
        if(ctxtshippingState.SetText(BillingState)!='')
        {
            ctxtshippingState.SetText(BillingState);
            document.getElementById("shippingstate").style.display = "none";
        }
        else
        {
            document.getElementById("shippingstate").style.display = "block";
        }
        // CompareState($('#hdStateIdShipping').val(), BillingStateId);
        $('#hdStateCodeShipping').val(BillingStateCode);
        $('#hdStateIdShipping').val(BillingStateId);
        if (ctxtshippingCity.SetText(BillingCity) != '')
        {
            ctxtshippingCity.SetText(BillingCity);
            document.getElementById("shippingcity").style.display = "none";
        }
        else
        {
            document.getElementById("shippingcity").style.display = "block";
        }

        $('#hdCityIdShipping').val(BillingCityId);
        ctxtSelectShippingArea.SetText(BillingAreaName);
        $('#hdAreaIdShipping').val(BillingAreaId);
        ctxtDistanceShipping.SetText(BillingDistance);
        ctxtShippingGSTIN1.SetText(BillingGstin1);
        ctxtShippingGSTIN2.SetText(BillingGstin2);
        ctxtShippingGSTIN3.SetText(BillingGstin3);
        $('#hdShipToParty').val('');
        ctxtShipToPartyShippingAdd.SetText('');

        ctxtShipConPerson.SetText(BillContactPerson);
        ctxtShipPhone.SetText(BillPhone);
        ctxtShipEmail.SetText(BillEmail);



  }
    //else {
    //    jAlert('Shipping state and Billing state must be same.');
    //}
 
}

//Bill to same address
function ShippingCheckChange() {
    var ShippingPin = ctxtShippingPin.GetText();
    if ((ShippingPin.length > 0) && (ShippingPin.length<=20)) {
        BillToSameAddress();
    }

    else if (ctxtShippingPin.GetText() == "0") {
        // ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
        jAlert('You may change the Shipping/Billing details except PIN/State.');
    }
    else if (ShippingPin.length >20) {
        ctxtbillingPin.SetText(ctxtShippingPin.GetText());
        $('#hdBillingPin').val($('#hdShippingPin').val());
        ctxtbillingCountry.SetText('');
        $('#hdCountryIdBilling').val('');
        ctxtbillingState.SetText('');
        $('#hdStateCodeBilling').val('');
        $('#hdStateIdBilling').val('');
        ctxtbillingCity.SetText('');
        $('#hdCityIdBilling').val('');

        ctxtshippingCountry.SetText('');
        $('#hdCountryIdShipping').val('');
        ctxtshippingState.SetText('');
        //$('#hdStateCodeShipping').val('');
        $('#hdStateIdShipping').val('');
        ctxtshippingCity.SetText('');
        $('#hdCityIdShipping').val('');
 }

}


function BillToSameAddress() {

    var ShippingAddress1 = ctxtsAddress1.GetText();
    var ShippingAddress2 = ctxtsAddress2.GetText();
    var ShippingAddress3 = ctxtsAddress3.GetText();
    var ShippingLandmark = ctxtslandmark.GetText();
    var ShippingPin = ctxtShippingPin.GetText();
    var ShippingPinId = $('#hdShippingPin').val();
    var ShippingCountry = ctxtshippingCountry.GetText();
    var ShippingCountryId = $('#hdCountryIdShipping').val();
    var ShippingState = ctxtshippingState.GetText();
    var ShippingStateId = $('#hdStateIdShipping').val();
    var ShippingStateCode = $('#hdStateCodeShipping').val();
    var ShippingCity = ctxtshippingCity.GetText();
    var ShippingCityId = $('#hdCityIdShipping').val();
    var ShippingAreaName = ctxtSelectShippingArea.GetText();
    var ShippingAreaId = $('#hdAreaIdShipping').val();
    var ShippingDistance = ctxtDistanceShipping.GetText();
    var ShippingGstIn1 = ctxtShippingGSTIN1.GetText();
    var ShippingGstIn2 = ctxtShippingGSTIN2.GetText();
    var ShippingGstIn3 = ctxtShippingGSTIN3.GetText();

    var ShippingPhone = ctxtShipPhone.GetText();
    var ShippingContactPerson = ctxtShipConPerson.GetText();
    var ShippingEmail = ctxtShipEmail.GetText();



    if (($('#hdStateCodeBilling').val() != ShippingStateCode) && AllowAddressShipToPartyState == false) {
        jAlert('You may change the Shipping/Billing details except PIN/State.');
    }
    else
        {
        if (ctxtAddress1.SetText(ShippingAddress1) != '') {
            ctxtAddress1.SetText(ShippingAddress1);
            document.getElementById("badd1").style.display = "none";
        }
        else {
            document.getElementById("badd1").style.display = "block";
        }


        //if (ctxtPhone.SetText(ShippingPhone) != '') {
        //    ctxtPhone.SetText(ShippingPhone);
        //    document.getElementById("billingPhone").style.display = "none";
        //}
        //else {
        //    document.getElementById("billingPhone").style.display = "block";
        //}



        ctxtAddress2.SetText(ShippingAddress2);
        ctxtAddress3.SetText(ShippingAddress3);
        ctxtlandmark.SetText(ShippingLandmark);
        if (ctxtbillingPin.SetText(ShippingPin) != '')
        {
            ctxtbillingPin.SetText(ShippingPin);
            document.getElementById("bpin").style.display = "none";
        }
        else
        {
            document.getElementById("bpin").style.display = "block";
        }
        $('#hdBillingPin').val(ShippingPinId);
        if (ctxtbillingCountry.SetText(ShippingCountry) != '') {
            ctxtbillingCountry.SetText(ShippingCountry);
            document.getElementById("billingcountry").style.display = "none";
        }
        else {
            document.getElementById("billingcountry").style.display = "block";
        }
        $('#hdCountryIdBilling').val(ShippingCountryId);
        if (ctxtbillingState.SetText(ShippingState) != '')
        {
            ctxtbillingState.SetText(ShippingState);
            document.getElementById("billingstate").style.display = "none";
        }
        else
        {
            document.getElementById("billingstate").style.display = "block";
        }
        $('#hdStateCodeBilling').val(ShippingStateCode);
        $('#hdStateIdBilling').val(ShippingStateId);
        if (ctxtbillingCity.SetText(ShippingCity) != '')
        {
            ctxtbillingCity.SetText(ShippingCity);
            document.getElementById("billingcity").style.display = "none";
        }
        else
        {
            document.getElementById("billingcity").style.display = "block";
        }
        $('#hdCityIdBilling').val(ShippingCityId);
        ctxtSelectBillingArea.SetText(ShippingAreaName);
        $('#hdAreaIdBilling').val(ShippingAreaId);
        ctxtDistance.SetText(ShippingDistance);
        ctxtBillingGSTIN1.SetText(ShippingGstIn1);
        ctxtBillingGSTIN2.SetText(ShippingGstIn2);
        ctxtBillingGSTIN3.SetText(ShippingGstIn3);

        ctxtConPerson.SetText(ShippingContactPerson);
        $("#Salesbillingphone").val(ShippingPhone);
        ctxtEmail.SetText(ShippingEmail);

    }
    //else
    //{
    //    jAlert('Billing state and Shipping state must be same.');
    //}

}


function SelectBillingAddClick(s, e) {

    BillingOrShipping = 'B'
    showAllAddress();
}

function SelectBillingKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        BillingOrShipping = 'B'
        showAllAddress();
    }
}

function SelectShippingAddClick(s, e) {

    BillingOrShipping = 'S'
    if (ctxtShipToPartyShippingAdd.GetText().trim() != "") {
        showAllAddressForShipToParty();
    }
    else
        showAllAddress();

}
function SelectShippingAddKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        BillingOrShipping = 'S'
        if (ctxtShipToPartyShippingAdd.GetText().trim() != "") {
            showAllAddressForShipToParty();
        }
        else
            showAllAddress();
    }
}

function showAllAddress() {
    $('#billingAddressModel').modal('show');
    //var htmlScript = " <table border='1' width=\"100%\" class=\"dynamicPopupTbl\"> <tr class=\"HeaderStyle\"> <th class=\"hide\">id</th> <th>Address</th> <th>Address Type</th><th>Isdefault</th> </tr> ";
    var htmlScript="<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">Id</th> <th>Address</th> <th style=\"width:150px;\">Address Type</th><th style=\"width:100px;\">Isdefault</th> </tr>";
    for (var len = 0; len < BillingAddress.length; len++) {
        var FullAddress = BillingAddress[len].Address1 + ',' + BillingAddress[len].Address2 + ',' + BillingAddress[len].Address3 + ',' + BillingAddress[len].CityName + ',' + BillingAddress[len].StateName + ',' + BillingAddress[len].CountryName + ',' + BillingAddress[len].PinCode
        FullAddress = FullAddress.replace(',,', ',');
        FullAddress = FullAddress.replace(',,', ',');
        var IsDe = (BillingAddress[len].Isdefault) ? 'Yes' : 'No';
        htmlScript += "<tr > <td><input readonly onclick='AllAddressonClick(" + BillingAddress[len].Id + ")' type='text' style='background-color: #3399520a;'AddressIndex=" + len + " onfocus='AllAddressGetFocus(event)'  onblur='AllAddresslostFocus(event)' onkeydown=AllAddressSelected(event," + BillingAddress[len].Id + ") width='100%' readonly value='" + FullAddress + "'/></td> <td onclick='AllAddressonClick(" + BillingAddress[len].Id + ")'> " + BillingAddress[len].Type + "</td><td>" + IsDe + "</td></tr>";
    }
    htmlScript += ' </table>';
    document.getElementById('AddressTableQuotation').innerHTML = htmlScript;
}

function showAllAddressForShipToParty() {
    $('#ShiptoPartyModel').modal('show');
    //var htmlScript = " <table border='1' width=\"100%\" class=\"dynamicPopupTbl\"> <tr class=\"HeaderStyle\"> <th class=\"hide\">id</th> <th>Address</th> <th>Address Type</th><th>Isdefault</th> </tr> ";
    var htmlScript="<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"> <th class=\"hide\">Id</th><th>Address</th> <th style=\"width:150px;\">Address Type</th> <th style=\"width:100px;\">Isdefault</th> </tr>;"
    for (var len = 0; len < shipObject.length; len++) {
        var FullAddress = shipObject[len].Address1 + ',' + shipObject[len].Address2 + ',' + shipObject[len].Address3 + ',' + shipObject[len].CityName + ',' + shipObject[len].StateName + ',' + shipObject[len].CountryName + ',' + shipObject[len].PinCode;
        FullAddress = FullAddress.replace(',,', ',');
        FullAddress = FullAddress.replace(',,', ',');
        var IsDe=(shipObject[len].Isdefault)?'Yes':'No';
        htmlScript += "<tr > <td><input readonly onclick='AllAddressonClick(" + shipObject[len].Id + ")' type='text' style='background-color: #3399520a;'AddressIndexShip=" + len + " onfocus='AllAddressGetFocus(event)'  onblur='AllAddresslostFocus(event)' onkeydown=AllAddressSelected(event," + shipObject[len].Id + ") width='100%' readonly value='" + FullAddress + "'/></td> <td onclick='AllAddressonClick(" + shipObject[len].Id + ")'> " + shipObject[len].Type + "</td><td>" + IsDe + "</td></tr>";
    }
    htmlScript += ' </table>';
    document.getElementById('ShiptoPartyModelTable').innerHTML = htmlScript;
}

function AllAddressonClick(Id) {
    if (BillingOrShipping == 'B')
        SetBillingAddressFromAllAddress(Id);
    if (BillingOrShipping == 'S')
        SetShippingAddressFromAllAddress(Id);
}

function AllAddressGetFocus(e) {
    e.target.parentElement.parentElement.className = "focusrow";
    e.target.style = "background: #0000ff3d";
}
function AllAddresslostFocus(e) {
    e.target.parentElement.parentElement.className = "";
    e.target.style = "background-color: #3399520a";
}

function SetBillingAddressFromAllAddress(SelectedId) {
    $('#billingAddressModel').modal('hide');
    var billingAdd = $.grep(BillingAddress, function (e) { return e.Id == SelectedId; });
    if (($('#hdStateCodeBilling').val() != billingAdd[0].StateCode) && AllowAddressShipToPartyState == false) {
      
        ctxtAddress1.GetText();
        ctxtAddress2.GetText();
        ctxtAddress3.GetText();
        ctxtbillingPin.GetText();
        $('#hdBillingPin').val();
        ctxtbillingCountry.GetText();
        $('#hdCountryIdBilling').val();
        ctxtbillingState.GetText();
        $('#hdStateIdBilling').val();
        $('#hdStateCodeBilling').val();
        ctxtbillingCity.GetText();
        $('#hdCityIdBilling').val();

        //var GSTIN = billingAdd[0].GSTIN;
        //GSTIN1 = GSTIN.substring(0, 2);
        //GSTIN2 = GSTIN.substring(2, 12);
        //GSTIN3 = GSTIN.substring(12, 15);
        ctxtBillingGSTIN1.GetText();
        ctxtBillingGSTIN2.GetText();
        ctxtBillingGSTIN3.GetText();

        ctxtConPerson.GetText();
        $("#Salesbillingphone").val();
        ctxtEmail.GetText();


        jAlert('You may change the Shipping/Billing details except PIN/State.');

    }
    else {
        if (ctxtAddress1.SetText(billingAdd[0].Address1) != '')
        {
            ctxtAddress1.SetText(billingAdd[0].Address1);
            document.getElementById("badd1").style.display = "none";
        }
        else
        {
            document.getElementById("badd1").style.display = "block";
        }


        //if (ctxtPhone.SetText(billingAdd[0].Phone) != '') {
        //    ctxtPhone.SetText(billingAdd[0].Phone);
        //    document.getElementById("billingPhone").style.display = "none";
        //}
        //else {
        //    document.getElementById("billingPhone").style.display = "block";
        //}


        ctxtAddress2.SetText(billingAdd[0].Address2);
        ctxtAddress3.SetText(billingAdd[0].Address3);
        if (ctxtbillingPin.SetText(billingAdd[0].PinCode)!='')
        {
            ctxtbillingPin.SetText(billingAdd[0].PinCode);
            document.getElementById("bpin").style.display = "none";
        }
        else
        {
            document.getElementById("bpin").style.display = "block";
        }
        $('#hdBillingPin').val(billingAdd[0].PinId);
        if (ctxtbillingCountry.SetText(billingAdd[0].CountryName) != '')
        {
            ctxtbillingCountry.SetText(billingAdd[0].CountryName);
            document.getElementById("billingcountry").style.display = "none";
        }
        else
        {
            document.getElementById("billingcountry").style.display = "block";
        }
        $('#hdCountryIdBilling').val(billingAdd[0].CountryId);
        if (ctxtbillingState.SetText(billingAdd[0].StateName) != '')
        {
            ctxtbillingState.SetText(billingAdd[0].StateName);
            document.getElementById("billingstate").style.display = "none";
        }
        else
        {
            document.getElementById("billingstate").style.display = "block";
        }
        $('#hdStateIdBilling').val(billingAdd[0].StateId);
        $('#hdStateCodeBilling').val(billingAdd[0].StateCode);
        if (ctxtbillingCity.SetText(billingAdd[0].CityName) != '')
        {
            ctxtbillingCity.SetText(billingAdd[0].CityName);
            document.getElementById("billingcity").style.display = "none";
        }
        else
        {
            document.getElementById("billingcity").style.display = "block";
        }
        $('#hdCityIdBilling').val(billingAdd[0].CityId);

        var GSTIN = billingAdd[0].GSTIN;
        GSTIN1 = GSTIN.substring(0, 2);
        GSTIN2 = GSTIN.substring(2, 12);
        GSTIN3 = GSTIN.substring(12, 15);
        ctxtBillingGSTIN1.SetText(GSTIN1);
        ctxtBillingGSTIN2.SetText(GSTIN2);
        ctxtBillingGSTIN3.SetText(GSTIN3);


        ctxtConPerson.SetText(billingAdd[0].ContactPerson);
        $("#Salesbillingphone").val(billingAdd[0].Phone);
        ctxtEmail.SetText(billingAdd[0].Email);

        //if (ctxtShippingPin.GetText() != '')
        //    cbtnSave_SalesBillingShiping.Focus();
        //else
        //    $('#shiptosame').focus();
    }
}

function SetShippingAddressFromAllAddress(SelectedId) {

  


    $('#billingAddressModel').modal('hide');
    $('#ShiptoPartyModel').modal('hide');
    var billingAdd;

    if (ctxtShipToPartyShippingAdd.GetText().trim() == "")
        billingAdd = $.grep(BillingAddress, function (e) { return e.Id == SelectedId; });

    else
        billingAdd = $.grep(shipObject, function (e) { return e.Id == SelectedId; });

    if (billingAdd) {
        if (($('#hdStateCodeShipping').val() != billingAdd[0].StateCode) && AllowAddressShipToPartyState == false  && PlaceOfSupplyShipPartyValue !=1) {
           
            // ctxtShipToPartyShippingAdd.SetText('');
            ctxtsAddress1.GetText();
            ctxtsAddress2.GetText();
            ctxtsAddress3.GetText();
            ctxtShippingPin.GetText();
            $('#hdShippingPin').val();
            ctxtshippingCountry.GetText();
            $('#hdCountryIdShipping').val();
            ctxtshippingState.GetText();

            $('#hdStateIdShipping').val();
            //$('#hdStateCodeShipping').val();
            ctxtshippingCity.GetText();
            $('#hdCityIdShipping').val(billingAdd[0].CityId);
            ctxtShippingGSTIN1.GetText();
            ctxtShippingGSTIN2.GetText();
            ctxtShippingGSTIN3.GetText();
            //Rev work start 06.07.2022 mantise no:25008
            //   ctxtShipToPartyShippingAdd.SetText('');
            var currentURL = window.location.href;
            if ((currentURL.includes("SalesQuotation.aspx")))
            {
                if (PlaceofSupplyVal == "S") 
                {
                    ctxtShipToPartyShippingAdd.SetText('');
                }
            }
            else
            {
                ctxtShipToPartyShippingAdd.SetText('');
            }
            //Rev work close 06.07.2022 mantise no:25008
            ctxtShipConPerson.SetText('');
            ctxtShipPhone.SetText('');
            ctxtShipEmail.SetText('');
            //if (ctxtShipToPartyShippingAdd.GetText() != "")
            //{
            //    $('#hdShipToParty').val();
            // ctxtShipToPartyShippingAdd.GetText();

            //}
            //else
            //{
            //    $('#hdShipToParty').val('');
            //    ctxtShipToPartyShippingAdd.SetText('');
            //}
            var htmlScript = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"> <th class=\"hide\">Id</th><th>Address</th> <th style=\"width:150px;\">Address Type</th> <th style=\"width:100px;\">Isdefault</th> </tr>;"

            document.getElementById('ShiptoPartyModelTable').innerHTML = htmlScript;
            //Rev work start 06.07.2022 mantise no:25008
            //jAlert('You may change the Shipping/Billing details except PIN/State.');
            var currentURL = window.location.href;            
            if ((currentURL.includes("SalesQuotation.aspx")||currentURL.includes("SalesOrderAdd.aspx")))
            {
                if (PlaceofSupplyVal == "S") {
                    jAlert('You may change the Shipping/Billing details except PIN/State.');
                }
                else {
                    if ((ctxtsAddress1.SetText(billingAdd[0].Address1)) != '') {
                        ctxtsAddress1.SetText(billingAdd[0].Address1);
                        document.getElementById("sadd1").style.display = "none";
                    }
                    else {
                        document.getElementById("sadd1").style.display = "block";
                    }
                    ctxtsAddress2.SetText(billingAdd[0].Address2);
                    ctxtsAddress3.SetText(billingAdd[0].Address3);
                    if (ctxtShippingPin.SetText(billingAdd[0].PinCode) != '') {
                        ctxtShippingPin.SetText(billingAdd[0].PinCode);
                        document.getElementById("spin").style.display = "none";
                    }
                    else {
                        document.getElementById("spin").style.display = "block";
                    }
                    $('#hdShippingPin').val(billingAdd[0].PinId);
                    if (ctxtshippingCountry.SetText(billingAdd[0].CountryName) != '') {
                        ctxtshippingCountry.SetText(billingAdd[0].CountryName);
                        document.getElementById("shippingcountry").style.display = "none";
                    }
                    else {
                        document.getElementById("shippingcountry").style.display = "block";
                    }
                    $('#hdCountryIdShipping').val(billingAdd[0].CountryId);
                    if (ctxtshippingState.SetText(billingAdd[0].StateName) != '') {
                        ctxtshippingState.SetText(billingAdd[0].StateName);
                        document.getElementById("shippingstate").style.display = "none";
                    }
                    else {
                        document.getElementById("shippingstate").style.display = "block";
                    }
                    $('#hdStateIdShipping').val(billingAdd[0].StateId);
                    $('#hdStateCodeShipping').val(billingAdd[0].StateCode);
                    if (ctxtshippingCity.SetText(billingAdd[0].CityName) != '') {
                        ctxtshippingCity.SetText(billingAdd[0].CityName);
                        document.getElementById("shippingcity").style.display = "none";
                    }
                    else {
                        document.getElementById("shippingcity").style.display = "block";
                    }

                    $('#hdCityIdShipping').val(billingAdd[0].CityId);

                    var GSTIN = billingAdd[0].GSTIN;
                    GSTIN1 = GSTIN.substring(0, 2);
                    GSTIN2 = GSTIN.substring(2, 12);
                    GSTIN3 = GSTIN.substring(12, 15);
                    ctxtShippingGSTIN1.SetText(GSTIN1);
                    ctxtShippingGSTIN2.SetText(GSTIN2);
                    ctxtShippingGSTIN3.SetText(GSTIN3);

                    ctxtShipConPerson.SetText(billingAdd[0].ContactPerson);
                    ctxtShipPhone.SetText(billingAdd[0].Phone);
                    ctxtShipEmail.SetText(billingAdd[0].Email);

                    //ctxtShipToPartyShippingAdd.SetText();
                    $('#billtoSame').focus();
                }
            }
            else
            {
                jAlert('You may change the Shipping/Billing details except PIN/State.');
            }
            //Rev work close 06.07.2022 mantise no:25008
        }

        else if (AllowAddressShipToPartyState == false && ctxtShipToPartyShippingAdd.GetValue() != "" && RequiredShipToPartyValue != 0 && PlaceOfSupplyShipPartyValue !=0)
        {


            if ((ctxtsAddress1.SetText(billingAdd[0].Address1)) != '') {
                ctxtsAddress1.SetText(billingAdd[0].Address1);
                document.getElementById("sadd1").style.display = "none";
            }
            else {
                document.getElementById("sadd1").style.display = "block";
            }
            ctxtsAddress2.SetText(billingAdd[0].Address2);
            ctxtsAddress3.SetText(billingAdd[0].Address3);
            if (ctxtShippingPin.SetText(billingAdd[0].PinCode) != '') {
                ctxtShippingPin.SetText(billingAdd[0].PinCode);
                document.getElementById("spin").style.display = "none";
            }
            else {
                document.getElementById("spin").style.display = "block";
            }
            $('#hdShippingPin').val(billingAdd[0].PinId);
            if (ctxtshippingCountry.SetText(billingAdd[0].CountryName) != '') {
                ctxtshippingCountry.SetText(billingAdd[0].CountryName);
                document.getElementById("shippingcountry").style.display = "none";
            }
            else {
                document.getElementById("shippingcountry").style.display = "block";
            }
            $('#hdCountryIdShipping').val(billingAdd[0].CountryId);
            if (ctxtshippingState.SetText(billingAdd[0].StateName) != '') {
                ctxtshippingState.SetText(billingAdd[0].StateName);
                document.getElementById("shippingstate").style.display = "none";
            }
            else {
                document.getElementById("shippingstate").style.display = "block";
            }
            $('#hdStateIdShipping').val(billingAdd[0].StateId);
            $('#hdStateCodeShipping').val(billingAdd[0].StateCode);
            if (ctxtshippingCity.SetText(billingAdd[0].CityName) != '') {
                ctxtshippingCity.SetText(billingAdd[0].CityName);
                document.getElementById("shippingcity").style.display = "none";
            }
            else {
                document.getElementById("shippingcity").style.display = "block";
            }

            $('#hdCityIdShipping').val(billingAdd[0].CityId);

            var GSTIN = billingAdd[0].GSTIN;
            GSTIN1 = GSTIN.substring(0, 2);
            GSTIN2 = GSTIN.substring(2, 12);
            GSTIN3 = GSTIN.substring(12, 15);
            ctxtShippingGSTIN1.SetText(GSTIN1);
            ctxtShippingGSTIN2.SetText(GSTIN2);
            ctxtShippingGSTIN3.SetText(GSTIN3);

            ctxtShipConPerson.SetText(billingAdd[0].ContactPerson);
            ctxtShipPhone.SetText(billingAdd[0].Phone);
            ctxtShipEmail.SetText(billingAdd[0].Email);

            if (PlaceOfSupplyShipPartyValue == 1) {
                TaxDeleteForShipPartyChange();
            }
            //ctxtShipToPartyShippingAdd.SetText();
            $('#billtoSame').focus();

        }

        else if ($('#hdStateCodeShipping').val() == billingAdd[0].StateCode) {

            if ((ctxtsAddress1.SetText(billingAdd[0].Address1))!='')
            {
                ctxtsAddress1.SetText(billingAdd[0].Address1);
                document.getElementById("sadd1").style.display = "none";
            }
            else
            {
               
                document.getElementById("sadd1").style.display = "block";
            }
            ctxtsAddress2.SetText(billingAdd[0].Address2);
            ctxtsAddress3.SetText(billingAdd[0].Address3);
            if (ctxtShippingPin.SetText(billingAdd[0].PinCode) != '') {
                ctxtShippingPin.SetText(billingAdd[0].PinCode);
                document.getElementById("spin").style.display = "none";
            }
            else {
                document.getElementById("spin").style.display = "block";
            }
            $('#hdShippingPin').val(billingAdd[0].PinId);
            if (ctxtshippingCountry.SetText(billingAdd[0].CountryName) != '')
            {
                ctxtshippingCountry.SetText(billingAdd[0].CountryName);
                document.getElementById("shippingcountry").style.display = "none";
            }
            else
            {
                document.getElementById("shippingcountry").style.display = "block";
            }
            $('#hdCountryIdShipping').val(billingAdd[0].CountryId);
            if (ctxtshippingState.SetText(billingAdd[0].StateName) != '')
            {
                ctxtshippingState.SetText(billingAdd[0].StateName);
                document.getElementById("shippingstate").style.display = "none";
            }
            else
            {
                document.getElementById("shippingstate").style.display = "block";
            }
            $('#hdStateIdShipping').val(billingAdd[0].StateId);
            $('#hdStateCodeShipping').val(billingAdd[0].StateCode);
            if (ctxtshippingCity.SetText(billingAdd[0].CityName) != '')
            {
                ctxtshippingCity.SetText(billingAdd[0].CityName);
                document.getElementById("shippingcity").style.display = "none";
            }
            else
            {
                document.getElementById("shippingcity").style.display = "block";
            }

            $('#hdCityIdShipping').val(billingAdd[0].CityId);

            var GSTIN = billingAdd[0].GSTIN;
            GSTIN1 = GSTIN.substring(0, 2);
            GSTIN2 = GSTIN.substring(2, 12);
            GSTIN3 = GSTIN.substring(12, 15);
            ctxtShippingGSTIN1.SetText(GSTIN1);
            ctxtShippingGSTIN2.SetText(GSTIN2);
            ctxtShippingGSTIN3.SetText(GSTIN3);

            ctxtShipConPerson.SetText(billingAdd[0].ContactPerson);
            ctxtShipPhone.SetText(billingAdd[0].Phone);
            ctxtShipEmail.SetText(billingAdd[0].Email);

            //ctxtShipToPartyShippingAdd.SetText();
            $('#billtoSame').focus();
        }
        else if (AllowAddressShipToPartyState == true) {


            if ((ctxtsAddress1.SetText(billingAdd[0].Address1)) != '') {
                ctxtsAddress1.SetText(billingAdd[0].Address1);
                document.getElementById("sadd1").style.display = "none";
            }
            else {
                document.getElementById("sadd1").style.display = "block";
            }
            ctxtsAddress2.SetText(billingAdd[0].Address2);
            ctxtsAddress3.SetText(billingAdd[0].Address3);
            if (ctxtShippingPin.SetText(billingAdd[0].PinCode) != '') {
                ctxtShippingPin.SetText(billingAdd[0].PinCode);
                document.getElementById("spin").style.display = "none";
            }
            else {
                document.getElementById("spin").style.display = "block";
            }
            $('#hdShippingPin').val(billingAdd[0].PinId);
            if (ctxtshippingCountry.SetText(billingAdd[0].CountryName) != '') {
                ctxtshippingCountry.SetText(billingAdd[0].CountryName);
                document.getElementById("shippingcountry").style.display = "none";
            }
            else {
                document.getElementById("shippingcountry").style.display = "block";
            }
            $('#hdCountryIdShipping').val(billingAdd[0].CountryId);
            if (ctxtshippingState.SetText(billingAdd[0].StateName) != '') {
                ctxtshippingState.SetText(billingAdd[0].StateName);
                document.getElementById("shippingstate").style.display = "none";
            }
            else {
                document.getElementById("shippingstate").style.display = "block";
            }
            $('#hdStateIdShipping').val(billingAdd[0].StateId);
            $('#hdStateCodeShipping').val(billingAdd[0].StateCode);
            if (ctxtshippingCity.SetText(billingAdd[0].CityName) != '') {
                ctxtshippingCity.SetText(billingAdd[0].CityName);
                document.getElementById("shippingcity").style.display = "none";
            }
            else {
                document.getElementById("shippingcity").style.display = "block";
            }

            $('#hdCityIdShipping').val(billingAdd[0].CityId);

            var GSTIN = billingAdd[0].GSTIN;
            GSTIN1 = GSTIN.substring(0, 2);
            GSTIN2 = GSTIN.substring(2, 12);
            GSTIN3 = GSTIN.substring(12, 15);
            ctxtShippingGSTIN1.SetText(GSTIN1);
            ctxtShippingGSTIN2.SetText(GSTIN2);
            ctxtShippingGSTIN3.SetText(GSTIN3);

            ctxtShipConPerson.SetText(billingAdd[0].ContactPerson);
            ctxtShipPhone.SetText(billingAdd[0].Phone);
            ctxtShipEmail.SetText(billingAdd[0].Email);

            //ctxtShipToPartyShippingAdd.SetText();
            $('#billtoSame').focus();

        }
    }
    cbtnSave_SalesBillingShiping.SetEnabled(true);
}

function AllAddressSelected(e, Id) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if (Id) {
            if (BillingOrShipping == 'B')
                SetBillingAddressFromAllAddress(Id);
            if (BillingOrShipping == 'S') {
                SetShippingAddressFromAllAddress(Id);

            }
        }
    }

    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute('AddressIndex'));
        thisindex++;
        if (thisindex < 10)
            $("input[AddressIndex=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute('AddressIndex'));
        thisindex--;
        if (thisindex > -1)
            $("input[AddressIndex=" + thisindex + "]").focus();
    }

}


$(document).ready(function () {

   
    



        $.ajax({
            type: "POST",
            url: "Services/BillingShipping.asmx/GetRequiredShipToParty",
            //data: JSON.stringify(),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                RequiredShipToPartyValue = msg.d;
            }
        });

   


  



        $.ajax({
            type: "POST",
            url: "Services/BillingShipping.asmx/GetPlaceOfSupplyShipParty",
            //data: JSON.stringify(),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                PlaceOfSupplyShipPartyValue = msg.d;
                //thats only for Quotation start 
                //Chinmoy
                var currentURL = window.location.href;
                if (!((currentURL.includes("SalesQuotation.aspx")) || (currentURL.includes("SalesOrderAdd.aspx")) || (currentURL.includes("SalesInvoice.aspx")) || (currentURL.includes("TSalesInvoice.aspx")))) {
                    PlaceOfSupplyShipPartyValue = 0;
                }

                //End


            }
        });

    

    $('#billingAddressModel').on('shown.bs.modal', function () {
        if ($("input[AddressIndex=0]"))
            $("input[AddressIndex=0]").focus();
    })
    
    $('#ShiptoPartyModel').on('shown.bs.modal', function () {
        if ($("input[AddressIndexShip=0]"))
            $("input[AddressIndexShip=0]").focus();
    })
})



function ValidationBillingShipping() {
    var _returnStatus = true;
   // var CheckaddressShip = ctxtsAddress1.GetText();
    var CheckAddress = ctxtAddress1.GetText();
    var CheckPin = ctxtbillingPin.GetText();
    var CheckCountry = ctxtbillingCountry.GetText();
    var CheckState = ctxtbillingState.GetText();
    var CheckCity = ctxtbillingCity.GetText();
    var CheckAddressShip = ctxtsAddress1.GetText();
    var CheckPinShip = ctxtShippingPin.GetText();
    var CheckCountryShip = ctxtshippingCountry.GetText();
    var CheckStateShip = ctxtshippingState.GetText();
    var CheckCityShip = ctxtshippingCity.GetText();

    var BillPhone = $("#Salesbillingphone").val();
    var shipPhone = ctxtShipPhone.GetText();


    if (CheckPin == '') {
        document.getElementById("bpin").style.display = "block";
        _returnStatus = false;
        ctxtbillingPin.Focus();
    }
    else {
        document.getElementById("bpin").style.display = "none";
    }



    if (CheckAddress == '') {
        document.getElementById("badd1").style.display = "block";
        _returnStatus = false;
        ctxtAddress1.Focus();
    }
    else {
        document.getElementById("badd1").style.display = "none";
    }


    //if (BillPhone == '') {
    //    document.getElementById("billingPhone").style.display = "block";
    //    _returnStatus = false;
    //    ctxtPhone.Focus();
    //}
    //else {
    //    document.getElementById("billingPhone").style.display = "none";
    //}


    if (CheckCountry == '') {
        document.getElementById("billingcountry").style.display = "block";
        _returnStatus = false;
        ctxtbillingCountry.Focus();
    }
    else {
        document.getElementById("billingcountry").style.display = "none";
    }


    if (CheckState == '') {
        document.getElementById("billingstate").style.display = "block";
        _returnStatus = false;
        ctxtbillingState.Focus();
    }
    else {
        document.getElementById("billingstate").style.display = "none";
    }


    if (CheckCity == '') {
        document.getElementById("billingcity").style.display = "block";
        _returnStatus = false;
        ctxtbillingCity.Focus();
    }
    else {
        document.getElementById("billingcity").style.display = "none";
    }


    if (CheckPinShip == '') {
        document.getElementById("spin").style.display = "block";
        _returnStatus = false;
        ctxtShippingPin.Focus();

    }
    else {
        document.getElementById("spin").style.display = "none";
    }

    if (CheckAddressShip == '') {
        document.getElementById("sadd1").style.display = "block";
        ctxtsAddress1.Focus();

        _returnStatus = false;
        
 }
    else {
        document.getElementById("sadd1").style.display = "none";
    }


    //if (shipPhone == '') {
    //    document.getElementById("shippingPhone").style.display = "block";
    //    ctxtShipPhone.Focus();

    //    _returnStatus = false;

    //}
    //else {
    //    document.getElementById("shippingPhone").style.display = "none";
    //}



    if (CheckCountryShip == '') {
        document.getElementById("shippingcountry").style.display = "block";
        _returnStatus = false;
        ctxtshippingCountry.Focus();
    }
    else {
        document.getElementById("shippingcountry").style.display = "none";
    }


    if (CheckStateShip == '') {
        document.getElementById("shippingstate").style.display = "block";
        _returnStatus = false;
        ctxtshippingState.Focus();
    }
    else {
        document.getElementById("shippingstate").style.display = "none";
    }


    if (CheckCityShip == '') {
        document.getElementById("shippingcity").style.display = "block";
        _returnStatus = false;
        ctxtshippingCity.Focus();
    }
    else {
        document.getElementById("shippingcity").style.display = "none";
    }

    AfterSaveBillingShipiing(_returnStatus);
}


function AreaAddressByCity(SearchKey, CityId) {

    $.ajax({
        type: "POST",
        url: "Services/BillingShipping.asmx/AreaByCity",
        data: JSON.stringify(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

        }
    })
}




function areakeydown(e) {
    var OtherDetails = {}
    OtherDetails.CityId = $("#hdCityIdBilling").val();
    OtherDetails.SearchKey = $("#txtbillingArea").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Area");
        if ($("#txtbillingArea").val() != '') {
            callonServer("Services/BillingShipping.asmx/AreaByCity", OtherDetails, "billingAreatable", HeaderCaption, "BillingAreaIndex", "SetBillingArea");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[BillingAreaIndex=0]"))
            $("input[BillingAreaIndex=0]").focus();
    }

}


function SetBillingArea(Id, Name) {
    //console.log(Id);
    //console.log(Name);
    $("#hdAreaIdBilling").val(Id);
    ctxtSelectBillingArea.SetText(Name);
    $('#billingAreaModel').modal('hide');
}

function SelectBillingAreaClick() {

    $('#billingAreaModel').modal('show');
}


function areakeydownshipping(e) {
    var othersdetails = {}
    othersdetails.CityId = $("#hdCityIdShipping").val();
    othersdetails.SearchKey = $("#txtshippingArea").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Area");
        if ($("#txtshippingArea").val() != '') {
            callonServer("Services/BillingShipping.asmx/AreaByCity", othersdetails, "shippingAreatable", HeaderCaption, "ShippingAreaIndex", "SetShippingArea");
        }

    }
    else if (e.code == "ArrowDown") {
        if ($("input[ShippingAreaIndex=0]"))
            $("input[ShippingAreaIndex=0]").focus();
    }

}

function SetShippingArea(Id, Name) {
    // console.log(Id);
    // console.log(Name);
    $("#hdAreaIdShipping").val(Id);
    ctxtSelectShippingArea.SetText(Name);
    $('#shippingAreaModel').modal('hide');
}

function SelectShippingAreaClick() {
    $('#shippingAreaModel').modal('show');
}

function SelectShippingAreaKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#shippingAreaModel').modal('show');
    }
}


function SelectBillingAreaKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#billingAreaModel').modal('show');
    }
}



function ShipToPartyAddClick() {
    $('#ShippingShipToPartyModel').modal('show');

    ShowAddressOfShipToParty();

}

function ShipToPartyAddkeydown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#ShippingShipToPartyModel').modal('show');

    }

}

function ShipToPartykeydown(e) {



    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtshippingShipToParty").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");
        if ($("#txtshippingShipToParty").val() != '') {
            callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "ShippingShipToPartytable", HeaderCaption, "customeraddressIndex", "SetCustomeraddress");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customeraddressIndex=0]"))
            $("input[customeraddressIndex=0]").focus();
    }
}





function SetCustomeraddress(Id, Name) {

    cbtnSave_SalesBillingShiping.SetEnabled(false);

    $('#hdShipToParty').val(Id);
    ctxtShipToPartyShippingAdd.SetText(Name);
    $('#ShippingShipToPartyModel').modal('hide');

    $.ajax({
        type: "POST",
        url: "Services/BillingShipping.asmx/FetchCustomAddress",
        data: JSON.stringify({ CustomerId: Id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            shipObject = msg.d;
           AddressShipToParty(shipObject);

        }
    }); 

}

function PhonenumberCheck()
{
    var inputphoneval = $("#Salesbillingphone").val();
    var phoneno = /^\d{10}$/;
    if (inputphoneval.match(phoneno)) {
        return true;
    }
    else {

        return false;
    }
}

function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;

    return true;
}

$(document).ready(function () {


    $('#ShippingShipToPartyModel').on('shown.bs.modal', function () {
        $('#txtshippingShipToParty').focus();

    });
   


})


function CustomerAddressLoad(returnObject) {



    var BillingObj = $.grep(returnObject, function (e) { return e.Type == "Billing" && e.Isdefault == 1; })
    var ShippingObj = $.grep(returnObject, function (e) { return e.Type == "Shipping" && e.Isdefault == 1; })
    //Billing
    if (BillingObj.length > 0) {

        ctxtAddress1.SetText(BillingObj[0].Address1);
        ctxtAddress2.SetText(BillingObj[0].Address2);
        ctxtAddress3.SetText(BillingObj[0].Address3);
        ctxtlandmark.SetText(BillingObj[0].Landmark);
        ctxtbillingPin.SetText(BillingObj[0].PinCode);
        $('#hdBillingPin').val(BillingObj[0].PinId);
        ctxtbillingCountry.SetText(BillingObj[0].CountryName);
        $('#hdCountryIdBilling').val(BillingObj[0].CountryId);
        ctxtbillingState.SetText(BillingObj[0].StateName);
        $('#hdStateCodeBilling').val(BillingObj[0].StateCode);
        $('#hdStateIdBilling').val(BillingObj[0].StateId);
        ctxtbillingCity.SetText(BillingObj[0].CityName);
        $('#hdCityIdBilling').val(BillingObj[0].CityId);
        ctxtSelectBillingArea.SetText(BillingObj[0].AreaName);
        $('#hdAreaIdBilling').val(BillingObj[0].AreaId);
        ctxtDistance.SetText(BillingObj[0].Distance);

        var GSTIN = BillingObj[0].GSTIN;
        var GSTIN1 = GSTIN.substring(0, 2);
        var GSTIN2 = GSTIN.substring(2, 12);
        var GSTIN3 = GSTIN.substring(12, 15);


        ctxtBillingGSTIN1.SetText(GSTIN1);
        ctxtBillingGSTIN2.SetText(GSTIN2);
        ctxtBillingGSTIN3.SetText(GSTIN3);

        ctxtConPerson.SetText(BillingObj[0].ContactPerson);
        //ctxtPhone.SetText(BillingObj[0].Phone);
        $("#Salesbillingphone").val(BillingObj[0].Phone);
        ctxtEmail.SetText(BillingObj[0].Email);
        //$("#abb").val(BillingObj[0].Phone);
    }
    else {
        ctxtAddress1.SetText('');
        ctxtAddress2.SetText('');
        ctxtAddress3.SetText('');
        ctxtlandmark.SetText('');
        ctxtbillingPin.SetText('');
        $('#hdBillingPin').val('');
        ctxtbillingCountry.SetText('');
        $('#hdCountryIdBilling').val('');
        ctxtbillingState.SetText('');
        $('#hdStateCodeBilling').val('');
        $('#hdStateIdBilling').val('');
        ctxtbillingCity.SetText('');
        $('#hdCityIdBilling').val('');
        ctxtSelectBillingArea.SetText('');
        $('#hdAreaIdBilling').val('');
        ctxtDistance.SetText('');
        ctxtBillingGSTIN1.SetText('');
        ctxtBillingGSTIN2.SetText('');
        ctxtBillingGSTIN3.SetText('');
        ctxtConPerson.SetText('');
        $("#Salesbillingphone").val('');
        ctxtEmail.SetText('');
    }

    //Shipping
    if (ShippingObj.length > 0) {
       
        ctxtsAddress1.SetText(ShippingObj[0].Address1);
        ctxtsAddress2.SetText(ShippingObj[0].Address2);
        ctxtsAddress3.SetText(ShippingObj[0].Address3);
        ctxtslandmark.SetText(ShippingObj[0].Landmark);
        ctxtShippingPin.SetText(ShippingObj[0].PinCode);
        $('#hdShippingPin').val(ShippingObj[0].PinId);
        ctxtshippingCountry.SetText(ShippingObj[0].CountryName);
        $('#hdCountryIdShipping').val(ShippingObj[0].CountryId);
        ctxtshippingState.SetText(ShippingObj[0].StateName);
        //CompareState($('#hdStateIdShipping').val(), ShippingObj[0].StateId);
        $('#hdStateCodeShipping').val(ShippingObj[0].StateCode);
        $('#hdStateIdShipping').val(ShippingObj[0].StateId);
        ctxtshippingCity.SetText(ShippingObj[0].CityName);
        $('#hdCityIdShipping').val(ShippingObj[0].CityId);
        ctxtSelectShippingArea.SetText(ShippingObj[0].AreaName);
        $('#hdAreaIdShipping').val(ShippingObj[0].AreaId);
        ctxtDistanceShipping.SetText(ShippingObj[0].Distance);

        var GSTIN = ShippingObj[0].GSTIN;
        var GSTIN1 = GSTIN.substring(0, 2);
        var GSTIN2 = GSTIN.substring(2, 12);
        var GSTIN3 = GSTIN.substring(12, 15);


        ctxtShippingGSTIN1.SetText(GSTIN1);
        ctxtShippingGSTIN2.SetText(GSTIN2);
        ctxtShippingGSTIN3.SetText(GSTIN3);
        ctxtShipToPartyShippingAdd.SetText('');


        ctxtShipConPerson.SetText(ShippingObj[0].ContactPerson);
        ctxtShipPhone.SetText(ShippingObj[0].Phone);
        ctxtShipEmail.SetText(ShippingObj[0].Email);

    }



    else {
        ctxtsAddress1.SetText('');
        ctxtsAddress2.SetText('');
        ctxtsAddress3.SetText('');
        ctxtslandmark.SetText('');
        ctxtShippingPin.SetText('');
        $('#hdShippingPin').val('');
        ctxtshippingCountry.SetText('');
        $('#hdCountryIdShipping').val('');
        ctxtshippingState.SetText('');
        $('#hdStateCodeShipping').val('');
        $('#hdStateIdShipping').val('');
        ctxtshippingCity.SetText('');
        $('#hdCityIdShipping').val('');
        ctxtSelectShippingArea.SetText('');
        $('#hdAreaIdShipping').val('');
        ctxtDistanceShipping.SetText('');
        ctxtShippingGSTIN1.SetText('');
        ctxtShippingGSTIN2.SetText('');
        ctxtShippingGSTIN3.SetText('');


        ctxtShipConPerson.SetText('');
        ctxtShipPhone.SetText('');
        ctxtShipEmail.SetText('');

    }

}

function AddressShipToParty(shipObject) {
    var ShippingObj = $.grep(shipObject, function (e) { return e.Type == "Shipping" && e.Isdefault == 1; })
   
    //if (ShippingObj.length > 0) {
    //    if (($('#hdStateCodeShipping').val() == ShippingObj[0].StateCode)) {
    //        ctxtsAddress1.SetText(ShippingObj[0].Address1);
    //        ctxtsAddress2.SetText(ShippingObj[0].Address2);
    //        ctxtsAddress3.SetText(ShippingObj[0].Address3);
    //        ctxtslandmark.SetText(ShippingObj[0].Landmark);
    //        ctxtShippingPin.SetText(ShippingObj[0].PinCode);
    //        $('#hdShippingPin').val(ShippingObj[0].PinId);
    //        ctxtshippingCountry.SetText(ShippingObj[0].CountryName);
    //        $('#hdCountryIdShipping').val(ShippingObj[0].CountryId);
    //        ctxtshippingState.SetText(ShippingObj[0].StateName);
    //        // CompareState($('#hdStateIdShipping').val(), ShippingObj[0].StateId);
    //        $('#hdStateCodeShipping').val(ShippingObj[0].StateCode);
    //        $('#hdStateIdShipping').val(ShippingObj[0].StateId);
    //        ctxtshippingCity.SetText(ShippingObj[0].CityName);
    //        $('#hdCityIdShipping').val(ShippingObj[0].CityId);
    //        ctxtSelectShippingArea.SetText(ShippingObj[0].AreaName);
    //        $('#hdAreaIdShipping').val(ShippingObj[0].AreaId);
    //        ctxtDistanceShipping.SetText(ShippingObj[0].Distance);

    //        var GSTIN = ShippingObj[0].GSTIN;
    //        var GSTIN1 = GSTIN.substring(0, 2);
    //        var GSTIN2 = GSTIN.substring(2, 12);
    //        var GSTIN3 = GSTIN.substring(12, 15);


    //        ctxtShippingGSTIN1.SetText(GSTIN1);
    //        ctxtShippingGSTIN2.SetText(GSTIN2);
    //        ctxtShippingGSTIN3.SetText(GSTIN3);
    //    }
    //    else {


            ctxtsAddress1.SetText('');
            ctxtsAddress2.SetText('');
            ctxtsAddress3.SetText('');
            ctxtslandmark.SetText('');
            ctxtShippingPin.SetText('');
            $('#hdShippingPin').val('');
            ctxtshippingCountry.SetText('');
            $('#hdCountryIdShipping').val('');
            ctxtshippingState.SetText('');
           
            ctxtshippingCity.SetText('');
            $('#hdCityIdShipping').val('');
            ctxtSelectShippingArea.SetText('');
            $('#hdAreaIdShipping').val('');
            ctxtDistanceShipping.SetText('');
            ctxtShippingGSTIN1.SetText('');
            ctxtShippingGSTIN2.SetText('');
            ctxtShippingGSTIN3.SetText('');
            ctxtShipConPerson.SetText('');
            ctxtShipPhone.SetText('');
            ctxtShipEmail.SetText('');
    //        jAlert('Existing State and Entering state must be same');
    //    }
                
        

    //}
    //else {
    //    ctxtsAddress1.SetText('');
    //    ctxtsAddress2.SetText('');
    //    ctxtsAddress3.SetText('');
    //    ctxtslandmark.SetText('');
    //    ctxtShippingPin.SetText('');
    //    $('#hdShippingPin').val('');
    //    ctxtshippingCountry.SetText('');
    //    $('#hdCountryIdShipping').val('');
    //    ctxtshippingState.SetText('');
    //    $('#hdStateCodeShipping').val('');
    //    $('#hdStateIdShipping').val('');
    //    ctxtshippingCity.SetText('');
    //    $('#hdCityIdShipping').val('');
    //    ctxtSelectShippingArea.SetText('');
    //    $('#hdAreaIdShipping').val('');
    //    ctxtDistanceShipping.SetText('');
    //    ctxtShippingGSTIN1.SetText('');
    //    ctxtShippingGSTIN2.SetText('');
    //    ctxtShippingGSTIN3.SetText('');
    //}

}


function ShowAddressOfShipToParty() {
    if (ctxtShipToPartyShippingAdd.GetText().trim() != "") {
       // AddressShipToParty(shipObject);
    }
    else
        CustomerAddressLoad(returnObject);

}

function GeteShippingStateID() {
    return $('#hdStateIdShipping').val();
}
function GeteShippingStateCode()
{
    return $('#hdStateCodeShipping').val();
}
function GetShippingStateName()
{
    return ctxtshippingState.GetText();
}


function GetBillingStateID() {
    return $('#hdStateIdBilling').val();
}
function GetBillingStateCode()
{
  return  $('#hdStateCodeBilling').val();
}
function GetBillingStateName()
{
   return  ctxtbillingState.GetText();
}

function Panel_endcallback() {

}

function BillShipAddressVisible()
{
    txtSelectBillingAdd.SetEnabled(false);
    ctxtbillingPin.SetEnabled(false);
    ctxtShipToPartyShippingAdd.SetEnabled(false);
    ctxtSelectShippingAdd.SetEnabled(false);
    ctxtShippingPin.SetEnabled(false);
    $('#lblShipToSame').hide();
    $('#lblBillToSame').hide();
   
    document.getElementById("dvBillShipMessage").style.display = "block";

}
function BillingPinGotFocus() {
    $('#hdOldBillingPinCode').val(ctxtbillingPin.GetText());
}
