var Address;
function SetPurchaseBillingShippingAddress(BranchId) {
    var OtherDetails = {}
    OtherDetails.BranchId = BranchId;
    $.ajax({
        type: "POST",
        url: "Services/PurchaseBillShip.asmx/FetchBranchAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            Address = msg.d;
            var BillShipDet = Address[0];
            var ShipObject = $.grep(Address, function (e) { return e.Type == "Shipping" })


            if (BillShipDet) {
                //Billing
                ctxtAddress1.SetText(BillShipDet.Address1);
                ctxtAddress2.SetText(BillShipDet.Address2);
                ctxtAddress3.SetText(BillShipDet.Address3);
                ctxtbillingPin.SetText(BillShipDet.PinCode);
                $('#hdBillingPin').val(BillShipDet.PinId);
                ctxtbillingCountry.SetText(BillShipDet.CountryName);
                $('#hdCountryIdBilling').val(BillShipDet.CountryId);
                ctxtbillingState.SetText(BillShipDet.StateName);
                $('#hdStateIdBilling').val(BillShipDet.StateId);
                $('#hdStateCodeBilling').val(BillShipDet.StateCode);
                ctxtbillingCity.SetText(BillShipDet.CityName);
                $('#hdCityIdBilling').val(BillShipDet.CityId);
                var GSTIN = BillShipDet.GSTIN;
                GSTIN1 = GSTIN.substring(0, 2);
                GSTIN2 = GSTIN.substring(2, 12);
                GSTIN3 = GSTIN.substring(12, 15);
                ctxtBillingGSTIN1.SetText(GSTIN1);
                ctxtBillingGSTIN2.SetText(GSTIN2);
                ctxtBillingGSTIN3.SetText(GSTIN3);
                //Shipping

                ctxtsAddress1.SetText(BillShipDet.Address1);
                ctxtsAddress2.SetText(BillShipDet.Address2);
                ctxtsAddress3.SetText(BillShipDet.Address3);
                ctxtShippingPin.SetText(BillShipDet.PinCode);
                $('#hdShippingPin').val(BillShipDet.PinId);
                ctxtshippingCountry.SetText(BillShipDet.CountryName);
                $('#hdCountryIdShipping').val(BillShipDet.CountryId);
                ctxtshippingState.SetText(BillShipDet.StateName);
                $('#hdStateIdShipping').val(BillShipDet.StateId);
                $('#hdStateCodeShipping').val(BillShipDet.StateCode);
                ctxtshippingCity.SetText(BillShipDet.CityName);
                $('#hdCityIdShipping').val(BillShipDet.CityId);
                var GSTIN = BillShipDet.GSTIN;
                GSTIN1 = GSTIN.substring(0, 2);
                GSTIN2 = GSTIN.substring(2, 12);
                GSTIN3 = GSTIN.substring(12, 15);
                ctxtShippingGSTIN1.SetText(GSTIN1);
                ctxtShippingGSTIN2.SetText(GSTIN2);
                ctxtShippingGSTIN3.SetText(GSTIN3);



            }
            else {
                //Billing
                ctxtAddress1.SetText('');
                ctxtAddress2.SetText('');
                ctxtAddress3.SetText('');
                ctxtbillingPin.SetText('');
                $('#hdBillingPin').val('');
                ctxtbillingCountry.SetText('');
                $('#hdCountryIdBilling').val('');
                ctxtbillingState.SetText('');
                $('#hdStateIdBilling').val('');
                $('#hdStateCodeBilling').val('');
                ctxtbillingCity.SetText('');
                $('#hdCityIdBilling').val('');
                var GSTIN = BillShipDet.GSTIN;
                GSTIN1 = GSTIN.substring(0, 2);
                GSTIN2 = GSTIN.substring(2, 12);
                GSTIN3 = GSTIN.substring(12, 15);
                ctxtBillingGSTIN1.SetText('');
                ctxtBillingGSTIN2.SetText('');
                ctxtBillingGSTIN3.SetText('');
                //Shipping
                ctxtsAddress1.SetText('');
                ctxtsAddress2.SetText('');
                ctxtsAddress3.SetText('');
                ctxtShippingPin.SetText('');
                $('#hdShippingPin').val('');
                ctxtshippingCountry.SetText('');
                $('#hdCountryIdShipping').val('');
                ctxtshippingState.SetText('');
                $('#hdStateIdShipping').val('');
                $('#hdStateCodeShipping').val('');
                ctxtshippingCity.SetText('');
                $('#hdCityIdShipping').val('');
                var GSTIN = BillShipDet.GSTIN;
                GSTIN1 = GSTIN.substring(0, 2);
                GSTIN2 = GSTIN.substring(2, 12);
                GSTIN3 = GSTIN.substring(12, 15);
                ctxtShippingGSTIN1.SetText('');
                ctxtShippingGSTIN2.SetText('');
                ctxtShippingGSTIN3.SetText('');
            }

        }
    });



}


function areakeydown(e) {
    var OtherDetails = {}
    OtherDetails.CityId = $("#hdCityIdBilling").val();
    OtherDetails.SearchKey = $("#txtbillingArea").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Area");
        if ($("#txtbillingArea").val() != '') {
            callonServer("Services/PurchaseBillShip.asmx/PurchaseAreaByCity", OtherDetails, "billingAreatable", HeaderCaption, "BillingAreaIndex", "SetBillingArea");
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
function SelectBillingAreaKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#billingAreaModel').modal('show');
    }
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

function GetVendorGSTInFromBillShip(internalId) {
    $.ajax({
        type: "POST",
        url: "Services/PurchaseBillShip.asmx/FetchGSTIN",
        data: JSON.stringify({ internalId: internalId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var Gst = msg.d;

            if (Gst) {
                $('#hfVendorGSTIN').val(Gst[0].GSTIN);
                if ($('#hfVendorGSTIN').val() == '') {
                    IfVendorGstInIsBlank();
                }
                else if ($('#hfVendorGSTIN').val() != '') {
                    cddl_AmountAre.SetValue("1");
                }

            }
            else {
                $('#hfVendorGSTIN').val('');
            }

        }

    });

}






function BillingCheckChange() {

    var BPin = ctxtbillingPin.GetText();
    if (BPin.length == 6) {
        ShipToSameAddress();
    }

    else if (BPin.length != 6) {


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

    var BillingGstin1 = ctxtBillingGSTIN1.GetText();
    var BillingGstin2 = ctxtBillingGSTIN2.GetText();
    var BillingGstin3 = ctxtBillingGSTIN3.GetText();


    if ($('#hdStateCodeShipping').val() == BillingStateCode) {

        ctxtsAddress1.SetText(BillingAddress1);
        ctxtsAddress2.SetText(BillingAddress2);
        ctxtsAddress3.SetText(BillingAddress3);
        ctxtslandmark.SetText(BillingLandmark);
        ctxtShippingPin.SetText(BillingPin);
        $('#hdShippingPin').val(BillingPinId);
        ctxtshippingCountry.SetText(BillingCountry);
        $('#hdCountryIdShipping').val(BillingCountryId);
        ctxtshippingState.SetText(BillingState);

        $('#hdStateCodeShipping').val(BillingStateCode);
        $('#hdStateIdShipping').val(BillingStateId);
        ctxtshippingCity.SetText(BillingCity);
        $('#hdCityIdShipping').val(BillingCityId);
        ctxtSelectShippingArea.SetText(BillingAreaName);
        $('#hdAreaIdShipping').val(BillingAreaId);
        ctxtShippingGSTIN1.SetText(BillingGstin1);
        ctxtShippingGSTIN2.SetText(BillingGstin2);
        ctxtShippingGSTIN3.SetText(BillingGstin3);


    }
    else
        jAlert('Shipping state and branch state must be same.');



}

//Bill to same address
function ShippingCheckChange() {
    var ShippingPin = ctxtShippingPin.GetText();
    if (ShippingPin.length == 6) {
        BillToSameAddress();
    }
    else if (ShippingPin.length != 6) {
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
        $('#hdStateCodeShipping').val('');
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

    var ShippingGstIn1 = ctxtShippingGSTIN1.GetText();
    var ShippingGstIn2 = ctxtShippingGSTIN2.GetText();
    var ShippingGstIn3 = ctxtShippingGSTIN3.GetText();

    ctxtAddress1.SetText(ShippingAddress1);
    ctxtAddress2.SetText(ShippingAddress2);
    ctxtAddress3.SetText(ShippingAddress3);
    ctxtlandmark.SetText(ShippingLandmark);
    ctxtbillingPin.SetText(ShippingPin);
    $('#hdBillingPin').val(ShippingPinId);
    ctxtbillingCountry.SetText(ShippingCountry);
    $('#hdCountryIdBilling').val(ShippingCountryId);
    ctxtbillingState.SetText(ShippingState);
    $('#hdStateCodeBilling').val(ShippingStateCode);
    $('#hdStateIdBilling').val(ShippingStateId);
    ctxtbillingCity.SetText(ShippingCity);
    $('#hdCityIdBilling').val(ShippingCityId);
    ctxtSelectBillingArea.SetText(ShippingAreaName);
    $('#hdAreaIdBilling').val(ShippingAreaId);

    ctxtBillingGSTIN1.SetText(ShippingGstIn1);
    ctxtBillingGSTIN2.SetText(ShippingGstIn2);
    ctxtBillingGSTIN3.SetText(ShippingGstIn3);

}

function CustomerBillingDetailsByPin() {

    var detailsByPin = ctxtbillingPin.GetText().trim();
    if (detailsByPin != '') {
        //var details = {}

        //details.PinCode = detailsByPin;
        $.ajax({
            type: "POST",
            url: "Services/PurchaseBillShip.asmx/BranchAddressByPin",
            data: JSON.stringify({ pin_code: detailsByPin }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var obj = msg.d;
                var returnObj = obj[0];

                //Billing
                if (returnObj) {
                    $('#hdBillingPin').val(returnObj.PinId);
                    ctxtbillingPin.SetText(returnObj.PinCode);
                    $('#hdCountryIdBilling').val(returnObj.CountryId);
                    ctxtbillingCountry.SetText(returnObj.CountryName);
                    $('#hdStateIdBilling').val(returnObj.StateId);
                    ctxtbillingState.SetText(returnObj.StateName);
                    $('#hdStateCodeBilling').val(returnObj.StateCode);
                    ctxtbillingCity.SetText(returnObj.CityName);
                    $('#hdCityIdBilling').val(returnObj.CityId);
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
                }

            }
        })

    }




}


function ShippingPinGotFocus() {
    $('#hdOldShippingPinCode').val(ctxtShippingPin.GetText());
}

//Customer Shipping Details By Pin


function CustomerShippingDetailsByPin() {

    var detailsByPin = ctxtShippingPin.GetText().trim();
    if (detailsByPin != '') {

        $.ajax({
            type: "POST",
            url: "Services/PurchaseBillShip.asmx/BranchAddressByPin",
            data: JSON.stringify({ pin_code: detailsByPin }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var obj = msg.d;
                var returnObj = obj[0];

                //Shipping
                if (returnObj) {

                    if ($('#hdStateCodeShipping').val() == returnObj.StateCode) {
                        $('#hdShippingPin').val(returnObj.PinId);
                        //ctxtShippingPin.SetText();
                        ctxtShippingPin.SetText(returnObj.PinCode);
                        $('#hdCountryIdShipping').val(returnObj.CountryId);
                        ctxtshippingCountry.SetText(returnObj.CountryName);
                        $('#hdStateIdShipping').val(returnObj.StateId);
                        ctxtshippingState.SetText(returnObj.StateName);
                        $('#hdStateCodeShipping').val(returnObj.StateCode);
                        ctxtshippingCity.SetText(returnObj.CityName);
                        $('#hdCityIdShipping').val(returnObj.CityId);
                    }
                    else if ($('#hdStateCodeShipping').val() != returnObj.StateCode) {
                        // ctxtShippingPin.SetText();
                        ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
                        jAlert('Shipping state and branch state must be same.');
                    }


                }
                else {
                    //  ctxtShippingPin.SetText('');
                    $('#hdCountryIdShipping').val('');
                    ctxtshippingCountry.SetText('');
                    $('#hdStateIdShipping').val('');
                    ctxtshippingState.SetText('');
                    $('#hdStateCodeShipping').val('');
                    ctxtshippingCity.SetText('');
                    $('#hdCityIdShipping').val('');


                }
            }
        });
    }

}




function BillingPinChange() {
    var BBSPin = ctxtbillingPin.GetText();
    if (BBSPin.length == 6) {
        CustomerBillingDetailsByPin();
    }
    else if (BBSPin.length != 6) {

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
    if (Spin.length == 6) {
        CustomerShippingDetailsByPin();
    }

    else if (Spin.length != 6) {
        ctxtshippingCountry.SetText('');
        $('#hdCountryIdShipping').val('');
        ctxtshippingState.SetText('');
        $('#hdStateCodeShipping').val('');
        $('#hdStateIdShipping').val('');
        ctxtshippingCity.SetText('');
        $('#hdCityIdShipping').val('');

    }

}



function ValidationBillingShipping() {
    var _returnStatus = true;
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

    if (CheckAddress == '') {
        document.getElementById("badd1").style.display = "block";
        _returnStatus = false;
        ctxtAddress1.Focus();
    }
    else {
        document.getElementById("badd1").style.display = "none";
    }


    if (CheckPin == '') {
        document.getElementById("bpin").style.display = "block";
        _returnStatus = false;
        ctxtbillingPin.Focus();
    }
    else {
        document.getElementById("bpin").style.display = "none";
    }


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


    if (CheckAddressShip == '') {
        document.getElementById("sadd1").style.display = "block";
        _returnStatus = false;
        ctxtsAddress1.Focus();


    }
    else {
        document.getElementById("sadd1").style.display = "none";
    }


    if (CheckPinShip == '') {
        document.getElementById("spin").style.display = "block";
        _returnStatus = false;
        ctxtShippingPin.Focus();

    }
    else {
        document.getElementById("spin").style.display = "none";
    }


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
