var Address;
var CusAdd = [];
var _customerAddress;
var BillingAddress = [];
var BillingOrShipping;
var returnObject;
var shipObject;

function SetPurchaseBillingShippingAddress(BranchId) {
    debugger;
    var OtherDetails = {}
    OtherDetails.BranchId = BranchId;
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/PurchaseBillShip.asmx/FetchBranchAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async:false,
        success: function (msg) {
            debugger;
            Address = msg.d;
            BillingAddress = msg.d;
           // CustomerAddressLoad(BillingAddress);
            var BillShipDet = $.grep(Address, function (e) { return e.Isdefault == 1 })

            var ShipObject = $.grep(BillShipDet, function (e) { return 1 == 1})
            
          //  BillingAddress = $.grep(BillShipDet, function (e) { return e.Isdefault == 1 })

            if (BillShipDet) {
                //Billing
                ctxtAddress1.SetText(BillShipDet[0].Address1);
                ctxtAddress2.SetText(BillShipDet[0].Address2);
                ctxtAddress3.SetText(BillShipDet[0].Address3);
                ctxtbillingPin.SetText(BillShipDet[0].PinCode);
                $('#hdBillingPin').val(BillShipDet[0].PinId);
                ctxtbillingCountry.SetText(BillShipDet[0].CountryName);
                $('#hdCountryIdBilling').val(BillShipDet[0].CountryId);
                ctxtbillingState.SetText(BillShipDet[0].StateName);
                $('#hdStateIdBilling').val(BillShipDet[0].StateId);
                $('#hdStateCodeBilling').val(BillShipDet[0].StateCode);
                ctxtbillingCity.SetText(BillShipDet[0].CityName);
                $('#hdCityIdBilling').val(BillShipDet[0].CityId);
                var GSTIN = BillShipDet[0].GSTIN;
                GSTIN1 = GSTIN.substring(0, 2);
                GSTIN2 = GSTIN.substring(2, 12);
                GSTIN3 = GSTIN.substring(12, 15);
                ctxtBillingGSTIN1.SetText(GSTIN1);
                ctxtBillingGSTIN2.SetText(GSTIN2);
                ctxtBillingGSTIN3.SetText(GSTIN3);
                //extra field addedd start

                ctxtCntperson.SetText(BillShipDet[0].ContactPerson);
                $("#Salesbillingphone").val(BillShipDet[0].Phone);
                ctxtEmail.SetText(BillShipDet[0].Email);
                //end
                //Shipping

                ctxtsAddress1.SetText(BillShipDet[0].Address1);
                ctxtsAddress2.SetText(BillShipDet[0].Address2);
                ctxtsAddress3.SetText(BillShipDet[0].Address3);
                ctxtShippingPin.SetText(BillShipDet[0].PinCode);
                $('#hdShippingPin').val(BillShipDet[0].PinId);
                ctxtshippingCountry.SetText(BillShipDet[0].CountryName);
                $('#hdCountryIdShipping').val(BillShipDet[0].CountryId);
                ctxtshippingState.SetText(BillShipDet[0].StateName);
                $('#hdStateIdShipping').val(BillShipDet[0].StateId);
                $('#hdStateCodeShipping').val(BillShipDet[0].StateCode);
                ctxtshippingCity.SetText(BillShipDet[0].CityName);
                $('#hdCityIdShipping').val(BillShipDet[0].CityId);
                var GSTIN = BillShipDet[0].GSTIN;
                GSTIN1 = GSTIN.substring(0, 2);
                GSTIN2 = GSTIN.substring(2, 12);
                GSTIN3 = GSTIN.substring(12, 15);
                ctxtShippingGSTIN1.SetText(GSTIN1);
                ctxtShippingGSTIN2.SetText(GSTIN2);
                ctxtShippingGSTIN3.SetText(GSTIN3);

                //extra field addedd start

                ctxtshipCntPerson.SetText(BillShipDet[0].ContactPerson);
                ctxtshipPhone.SetText(BillShipDet[0].Phone);
                ctxtshipEmail.SetText(BillShipDet[0].Email);


                //end



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
                ctxtCntperson.SetText('');
                $("#Salesbillingphone").val('');
                ctxtEmail.SetText('');
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
                ctxtshipCntPerson.SetText();
                ctxtshipPhone.SetText();
                ctxtshipEmail.SetText();
            }

            



        }
    });



}


function LoadBranchAddressInEditMode(BranchId) {
    var OtherDetails = {}
    OtherDetails.BranchId = BranchId;
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/PurchaseBillShip.asmx/FetchBranchAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            Address = msg.d;
            BillingAddress = msg.d;
        }
    });
}




function LoadCustomerBillingShippingAddress(CustomerId) {
    var OtherDetails = {}
    OtherDetails.CustomerId = CustomerId;
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/BillingShipping.asmx/FetchCustomAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            shipObject = msg.d;
            BillingAddress = msg.d;
            // CustomerAddressLoad(BillingAddress);

        }
    });

}








function SetCustomerBillingShippingAddress(CustomerId) {
    var OtherDetails = {}
    OtherDetails.CustomerId = CustomerId;
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/BillingShipping.asmx/FetchCustomAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            BillingAddress = msg.d;
            CustomerAddressLoad(BillingAddress);



        }
    });



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
            callonServer("/OMS/Management/Activities/Services/Master.asmx/GetCustomer", OtherDetails, "ShippingShipToPartytable", HeaderCaption, "customeraddressIndex", "SetCustomeraddress");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customeraddressIndex=0]"))
            $("input[customeraddressIndex=0]").focus();
    }
}


function SetCustomeraddress(Id, Name) {
    $('#hdShipToParty').val(Id);
    ctxtShipToPartyShippingAdd.SetText(Name);
    $('#ShippingShipToPartyModel').modal('hide');

    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/BillingShipping.asmx/FetchCustomAddress",
        data: JSON.stringify({ CustomerId: Id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            shipObject = msg.d;
            AddressShipToParty(shipObject);
           
        }
    });

}

function AddressShipToParty(shipObject) {
    var ShippingObj = $.grep(shipObject, function (e) { return e.Type == "Shipping"})
    //if (ShippingObj.length > 0) {



    //    ctxtsAddress1.SetText(ShippingObj[0].Address1);
    //    ctxtsAddress2.SetText(ShippingObj[0].Address2);
    //    ctxtsAddress3.SetText(ShippingObj[0].Address3);
    //    ctxtslandmark.SetText(ShippingObj[0].Landmark);
    //    ctxtShippingPin.SetText(ShippingObj[0].PinCode);
    //    $('#hdShippingPin').val(ShippingObj[0].PinId);
    //    ctxtshippingCountry.SetText(ShippingObj[0].CountryName);
    //    $('#hdCountryIdShipping').val(ShippingObj[0].CountryId);
    //    ctxtshippingState.SetText(ShippingObj[0].StateName);
    //    // CompareState($('#hdStateIdShipping').val(), ShippingObj[0].StateId);
    //    $('#hdStateCodeShipping').val(ShippingObj[0].StateCode);
    //    $('#hdStateIdShipping').val(ShippingObj[0].StateId);
    //    ctxtshippingCity.SetText(ShippingObj[0].CityName);
    //    $('#hdCityIdShipping').val(ShippingObj[0].CityId);
    //    ctxtSelectShippingArea.SetText(ShippingObj[0].AreaName);
    //    $('#hdAreaIdShipping').val(ShippingObj[0].AreaId);
    //   // ctxtDistanceShipping.SetText(ShippingObj[0].Distance);

    //    var GSTIN = ShippingObj[0].GSTIN;
    //    var GSTIN1 = GSTIN.substring(0, 2);
    //    var GSTIN2 = GSTIN.substring(2, 12);
    //    var GSTIN3 = GSTIN.substring(12, 15);


    //    ctxtShippingGSTIN1.SetText(GSTIN1);
    //    ctxtShippingGSTIN2.SetText(GSTIN2);
    //    ctxtShippingGSTIN3.SetText(GSTIN3);




    //}
    //else {
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
       // ctxtDistanceShipping.SetText('');
        ctxtShippingGSTIN1.SetText('');
        ctxtShippingGSTIN2.SetText('');
        ctxtShippingGSTIN3.SetText('');

        ctxtshipCntPerson.SetText('');
        ctxtshipPhone.SetText('');
        ctxtshipEmail.SetText('');
        ctxtshipCntPerson.SetText('');
        ctxtshipPhone.SetText('');
        ctxtshipEmail.SetText('');
    //}

}

$(document).ready(function () {


    $('#ShippingShipToPartyModel').on('shown.bs.modal', function () {
        $('#txtshippingShipToParty').focus();

    })
})

function ShowAddressOfShipToParty() {
    if (ctxtShipToPartyShippingAdd.GetText().trim() != "") {
        //AddressShipToParty(shipObject);
    }
    else
        CustomerAddressLoad(returnObject);

}



function areakeydown(e) {
    var OtherDetails = {}
    OtherDetails.CityId = $("#hdCityIdBilling").val();
    OtherDetails.SearchKey = $("#txtbillingArea").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Area");
        if ($("#txtbillingArea").val() != '') {
            callonServer("/OMS/Management/Activities/Services/PurchaseBillShip.asmx/PurchaseAreaByCity", OtherDetails, "billingAreatable", HeaderCaption, "BillingAreaIndex", "SetBillingArea");
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
            callonServer("/OMS/Management/Activities/Services/PurchaseBillShip.asmx/PurchaseAreaByCity", othersdetails, "shippingAreatable", HeaderCaption, "ShippingAreaIndex", "SetShippingArea");
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
        url: "/OMS/Management/Activities/Services/PurchaseBillShip.asmx/FetchGSTIN",
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
                    //cddl_AmountAre.SetValue("1");
                } 

            }
            else {
                $('#hfVendorGSTIN').val('');
            }

        }

    });

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
    var htmlScript = " <table border='1' width=\"100%\" class=\"dynamicPopupTbl\"> <tr class=\"HeaderStyle\"> <th class=\"hide\">id</th> <th>Address</th> <th>Address Type</th> </tr> ";
    //var htmlScript = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">Id</th> <th>Address</th> <th style=\"width:150px;\">Address Type</th><th style=\"width:100px;\">Isdefault</th> </tr>";
    for (var len = 0; len < BillingAddress.length; len++) {
        var FullAddress = BillingAddress[len].Address1 + ',' + BillingAddress[len].Address2 + ',' + BillingAddress[len].Address3 + ',' + BillingAddress[len].CityName + ',' + BillingAddress[len].StateName + ',' + BillingAddress[len].CountryName + ',' + BillingAddress[len].PinCode;
        FullAddress = FullAddress.replace(',,', ',');
        FullAddress = FullAddress.replace(',,', ',');
        //var IsDe = (BillingAddress[len].Isdefault) ? 'Yes' : 'No';
        htmlScript += "<tr > <td><input readonly onclick='AllAddressonClick(" + BillingAddress[len].Id + ")' type='text' style='background-color: #3399520a;'AddressIndex=" + len + " onfocus='AllAddressGetFocus(event)'  onblur='AllAddresslostFocus(event)' onkeydown=AllAddressSelected(event," + BillingAddress[len].Id + ") width='100%' readonly value='" + FullAddress + "'/></td> <td onclick='AllAddressonClick(" + BillingAddress[len].Id + ")'> " + BillingAddress[len].Type + "</td></tr>";
    }
    htmlScript += ' </table>';
    document.getElementById('AddressTableQuotation').innerHTML = htmlScript;
}

function showAllAddressForShipToParty() {
    $('#ShiptoPartyModel').modal('show');
    var htmlScript = " <table border='1' width=\"100%\" class=\"dynamicPopupTbl\"> <tr class=\"HeaderStyle\"> <th class=\"hide\">id</th> <th>Address</th> <th>Address Type</th> </tr> ";
    //var htmlScript = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"> <th class=\"hide\">Id</th><th>Address</th> <th style=\"width:150px;\">Address Type</th> <th style=\"width:100px;\">Isdefault</th> </tr>;"
    for (var len = 0; len < shipObject.length; len++) {
        var FullAddress = shipObject[len].Address1 + ',' + shipObject[len].Address2 + ',' + shipObject[len].Address3 + ',' + shipObject[len].CityName + ',' + shipObject[len].StateName + ',' + shipObject[len].CountryName + ',' + shipObject[len].PinCode;
        FullAddress = FullAddress.replace(',,', ',');
        FullAddress = FullAddress.replace(',,', ',');
        //var IsDe = (shipObject[len].Isdefault) ? 'Yes' : 'No';
        htmlScript += "<tr > <td><input readonly onclick='AllAddressonClick(" + shipObject[len].Id + ")' type='text' style='background-color: #3399520a;'AddressIndexShip=" + len + " onfocus='AllAddressGetFocus(event)'  onblur='AllAddresslostFocus(event)' onkeydown=AllAddressSelected(event," + shipObject[len].Id + ") width='100%' readonly value='" + FullAddress + "'/></td> <td onclick='AllAddressonClick(" + shipObject[len].Id + ")'> " + shipObject[len].Type +"</td></tr>";
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
        thisindex = parseFloat(e.target.getAttribute('AddressIndexShip'));
        thisindex++;
        if (thisindex < 10)
            $("input[AddressIndexShip=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute('AddressIndexShip'));
        thisindex--;
        if (thisindex > -1)
            $("input[AddressIndexShip=" + thisindex + "]").focus();
    }

}



function SetBillingAddressFromAllAddress(SelectedId) {
    $('#billingAddressModel').modal('hide');
    var billingAdd = $.grep(BillingAddress, function (e) { return e.Id == SelectedId; });
    if (ctxtAddress1.SetText(billingAdd[0].Address1) != '') {
        ctxtAddress1.SetText(billingAdd[0].Address1);
        document.getElementById("badd1").style.display = "none";
    }
    else {
        document.getElementById("badd1").style.display = "block";
    }
    ctxtAddress2.SetText(billingAdd[0].Address2);
    ctxtAddress3.SetText(billingAdd[0].Address3);
    if (ctxtbillingPin.SetText(billingAdd[0].PinCode) != '') {
        ctxtbillingPin.SetText(billingAdd[0].PinCode);
        document.getElementById("bpin").style.display = "none";
    }
    else {
        document.getElementById("bpin").style.display = "block";
    }
    $('#hdBillingPin').val(billingAdd[0].PinId);
    if (ctxtbillingCountry.SetText(billingAdd[0].CountryName) != '') {
        ctxtbillingCountry.SetText(billingAdd[0].CountryName);
        document.getElementById("billingcountry").style.display = "none";
    }
    else {
        document.getElementById("billingcountry").style.display = "block";
    }
    $('#hdCountryIdBilling').val(billingAdd[0].CountryId);
    if (ctxtbillingState.SetText(billingAdd[0].StateName) != '') {
        ctxtbillingState.SetText(billingAdd[0].StateName);
        document.getElementById("billingstate").style.display = "none";
    }
    else {
        document.getElementById("billingstate").style.display = "block";
    }
    $('#hdStateIdBilling').val(billingAdd[0].StateId);
    $('#hdStateCodeBilling').val(billingAdd[0].StateCode);
    if (ctxtbillingCity.SetText(billingAdd[0].CityName) != '') {
        ctxtbillingCity.SetText(billingAdd[0].CityName);
        document.getElementById("billingcity").style.display = "none";
    }
    else {
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

    ctxtCntperson.SetText(billingAdd[0].ContactPerson);
    $("#Salesbillingphone").val(billingAdd[0].Phone);
    ctxtEmail.SetText(billingAdd[0].Email);
    

    //if (ctxtShippingPin.GetText() != '')
    //    cbtnSave_SalesBillingShiping.Focus();
    //else
    //    $('#shiptosame').focus();

}

function SetShippingAddressFromAllAddress(SelectedId) {
    $('#billingAddressModel').modal('hide');
    $('#ShiptoPartyModel').modal('hide');
    var billingAdd;

    if (ctxtShipToPartyShippingAdd.GetText().trim() == "")
        billingAdd = $.grep(BillingAddress, function (e) { return e.Id == SelectedId; });

    else
        billingAdd = $.grep(shipObject, function (e) { return e.Id == SelectedId; });
   
    if (($('#hdStateCodeShipping').val() == billingAdd[0].StateCode)||($('#hdStateCodeShipping').val()=="")) {
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

      
        ctxtshipCntPerson.SetText(billingAdd[0].ContactPerson);
        ctxtshipPhone.SetText(billingAdd[0].Phone);
        ctxtshipEmail.SetText(billingAdd[0].Email);

        $('#billtoSame').focus();
    }
    else
    {
        jAlert('Existing state and entering state must be same');
    }
   
}

function BillingCheckChange() {

    var BPin = ctxtbillingPin.GetText();
    if ((BPin.length > 0) && (BPin.length<=20)) {
        ShipToSameAddress();
    }


    else if (BPin.length > 20) {


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


function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;

    return true;
}

$(document).ready(function () {



    $('#billingAddressModel').on('shown.bs.modal', function () {
        if ($("input[AddressIndex=0]"))
            $("input[AddressIndex=0]").focus();
    })

    $('#ShiptoPartyModel').on('shown.bs.modal', function () {
        if ($("input[AddressIndexShip=0]"))
            $("input[AddressIndexShip=0]").focus();
    })


})


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


    var ContactPerson = ctxtCntperson.GetText();
    var Phone = $("#Salesbillingphone").val();
    var Email = ctxtEmail.GetText();
   
   

    if (ctxtsAddress1.SetText(BillingAddress1) != "")
    {
        ctxtsAddress1.SetText(BillingAddress1);
        document.getElementById("sadd1").style.display = "none";
    }
    else
    {
        document.getElementById("sadd1").style.display = "block";
    }
        ctxtsAddress2.SetText(BillingAddress2);
        ctxtsAddress3.SetText(BillingAddress3);
        ctxtslandmark.SetText(BillingLandmark);
        if (ctxtShippingPin.SetText(BillingPin) != "")
        {
            ctxtShippingPin.SetText(BillingPin);
            document.getElementById("spin").style.display = "none";
        }
        else
        {
            document.getElementById("spin").style.display = "block";
        }
        $('#hdShippingPin').val(BillingPinId);
        if (ctxtshippingCountry.SetText(BillingCountry) != "")
        {
            ctxtshippingCountry.SetText(BillingCountry);
            document.getElementById("shippingcountry").style.display = "none";
        }
        else
        {
            document.getElementById("shippingcountry").style.display = "block";
        }
        $('#hdCountryIdShipping').val(BillingCountryId);
        if (ctxtshippingState.SetText(BillingState) != "")
        {
            ctxtshippingState.SetText(BillingState);
            document.getElementById("shippingstate").style.display = "none";
        }
        else
        {
            document.getElementById("shippingstate").style.display = "block";
        }

        $('#hdStateCodeShipping').val(BillingStateCode);
        $('#hdStateIdShipping').val(BillingStateId);
        if (ctxtshippingCity.SetText(BillingCity) != "")
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
        ctxtShippingGSTIN1.SetText(BillingGstin1);
        ctxtShippingGSTIN2.SetText(BillingGstin2);
        ctxtShippingGSTIN3.SetText(BillingGstin3);
        $('#hdShipToParty').val('');
        ctxtShipToPartyShippingAdd.SetText('');

       
        ctxtshipCntPerson.SetText(ContactPerson);
        ctxtshipPhone.SetText(Phone);
        ctxtshipEmail.SetText(Email);
    



}

//Bill to same address
function ShippingCheckChange() {
    var ShippingPin = ctxtShippingPin.GetText();
    if ((ShippingPin.length > 0) && (ShippingPin.length <= 20)) {
        BillToSameAddress();
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


    
    var ContactPerson = ctxtshipCntPerson.GetText();
    var Phone = ctxtshipPhone.GetText();
    var Email = ctxtshipEmail.GetText();
    
    if (ctxtAddress1.SetText(ShippingAddress1) != "")
    {
        ctxtAddress1.SetText(ShippingAddress1);
        document.getElementById("badd1").style.display = "none";
    }
    else
    {
        document.getElementById("badd1").style.display = "block";
    }
        ctxtAddress2.SetText(ShippingAddress2);
        ctxtAddress3.SetText(ShippingAddress3);
        ctxtlandmark.SetText(ShippingLandmark);
        if (ctxtbillingPin.SetText(ShippingPin) != "")
        {
            ctxtbillingPin.SetText(ShippingPin);
            document.getElementById("bpin").style.display = "none";
        }
        else
        {
            document.getElementById("bpin").style.display = "block";
        }
        $('#hdBillingPin').val(ShippingPinId);
        if (ctxtbillingCountry.SetText(ShippingCountry) != "")
        {
            ctxtbillingCountry.SetText(ShippingCountry);
            document.getElementById("billingcountry").style.display = "none";
        }
        else
        {
            document.getElementById("billingcountry").style.display = "block";
        }
        $('#hdCountryIdBilling').val(ShippingCountryId);

        if (ctxtbillingState.SetText(ShippingState) != "")
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
        if (ctxtbillingCity.SetText(ShippingCity) != "")
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

        ctxtBillingGSTIN1.SetText(ShippingGstIn1);
        ctxtBillingGSTIN2.SetText(ShippingGstIn2);
        ctxtBillingGSTIN3.SetText(ShippingGstIn3);
    
        ctxtCntperson.SetText(ContactPerson);
        $("#Salesbillingphone").val(Phone);
        ctxtEmail.SetText(Email);
       
}

function CustomerBillingDetailsByPin() {

    var detailsByPin = ctxtbillingPin.GetText().trim();
    if (detailsByPin != '') {
        //var details = {}

        //details.PinCode = detailsByPin;
        $.ajax({
            type: "POST",
            url: "/OMS/Management/Activities/Services/PurchaseBillShip.asmx/BranchAddressByPin",
            data: JSON.stringify({ pin_code: detailsByPin }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var obj = msg.d;
                var returnObj = obj[0];

                //Billing
                if (returnObj) {

                  
                        $('#hdBillingPin').val(returnObj.PinId);
                        if(ctxtbillingPin.SetText(returnObj.PinCode)!="")
                        {
                            ctxtbillingPin.SetText(returnObj.PinCode);
                            document.getElementById("bpin").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("bpin").style.display = "block";
                        }
                        $('#hdCountryIdBilling').val(returnObj.CountryId);
                        if (ctxtbillingCountry.SetText(returnObj.CountryName) != "")
                        {
                            ctxtbillingCountry.SetText(returnObj.CountryName);
                            document.getElementById("billingcountry").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("billingcountry").style.display = "block";
                        }
                        $('#hdStateIdBilling').val(returnObj.StateId);
                        if (ctxtbillingState.SetText(returnObj.StateName) != "")
                        {
                            ctxtbillingState.SetText(returnObj.StateName);
                            document.getElementById("billingstate").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("billingstate").style.display = "block";
                        }
                        $('#hdStateCodeBilling').val(returnObj.StateCode);
                        if (ctxtbillingCity.SetText(returnObj.CityName) != "")
                        {
                            ctxtbillingCity.SetText(returnObj.CityName);
                            document.getElementById("billingcity").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("billingcity").style.display = "block";
                        }
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

function BillingPinGotFocus()
{
    $('#hdOldBillingPinCode').val(ctxtbillingPin.GetText());
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
            url: "/OMS/Management/Activities/Services/PurchaseBillShip.asmx/BranchAddressByPin",
            data: JSON.stringify({ pin_code: detailsByPin }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var obj = msg.d;
                var returnObj = obj[0];

                //Shipping
                if (returnObj) {

                   
                        $('#hdShippingPin').val(returnObj.PinId);
                        //ctxtShippingPin.SetText();
                        if (ctxtShippingPin.SetText(returnObj.PinCode) != "")
                        {
                            ctxtShippingPin.SetText(returnObj.PinCode)
                            document.getElementById("spin").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("spin").style.display = "block";
                        }
                        $('#hdCountryIdShipping').val(returnObj.CountryId);
                        if (ctxtshippingCountry.SetText(returnObj.CountryName) != "")
                        {
                            ctxtshippingCountry.SetText(returnObj.CountryName);
                            document.getElementById("shippingcountry").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("shippingcountry").style.display = "block";
                        }
                        $('#hdStateIdShipping').val(returnObj.StateId);
                        if (ctxtshippingState.SetText(returnObj.StateName) != "")
                        {
                            ctxtshippingState.SetText(returnObj.StateName);
                            document.getElementById("shippingstate").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("shippingstate").style.display = "block";
                        }
                        $('#hdStateCodeShipping').val(returnObj.StateCode);
                        if (ctxtshippingCity.SetText(returnObj.CityName) != "")
                        {
                            ctxtshippingCity.SetText(returnObj.CityName);
                            document.getElementById("shippingcity").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("shippingcity").style.display = "block";
                        }
                        $('#hdCityIdShipping').val(returnObj.CityId);
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
    if ((BBSPin.length > 0) && (BBSPin.length<=20)) {
        CustomerBillingDetailsByPin();
    }

    else if (ctxtbillingPin.GetText() == "0") {
        ctxtbillingPin.SetText($('#hdOldBillingPinCode').val());
        jAlert('Pincode are not be Zero.');
    }


    else if (BBSPin.length>20) {

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
    if ((Spin.length > 0) && (Spin.length<=20)) {
        CustomerShippingDetailsByPin();
    }
    else if (ctxtShippingPin.GetText() == "0")
    {
        ctxtShippingPin.SetText($('#hdOldShippingPinCode').val());
        jAlert('Pincode are not be Zero.');
    }

    else if (Spin.length>20) {
        ctxtshippingCountry.SetText('');
        $('#hdCountryIdShipping').val('');
        ctxtshippingState.SetText('');
        $('#hdStateCodeShipping').val('');
        $('#hdStateIdShipping').val('');
        ctxtshippingCity.SetText('');
        $('#hdCityIdShipping').val('');

    }

}

function CustomerAddressLoad(returnObject) {



    var BillingObj = $.grep(returnObject, function (e) { return e.Type == "Billing" })
    var ShippingObj = $.grep(returnObject, function (e) { return e.Type == "Shipping" })
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
       // ctxtDistance.SetText(BillingObj[0].Distance);

        var GSTIN = BillingObj[0].GSTIN;
        var GSTIN1 = GSTIN.substring(0, 2);
        var GSTIN2 = GSTIN.substring(2, 12);
        var GSTIN3 = GSTIN.substring(12, 15);


        ctxtBillingGSTIN1.SetText(GSTIN1);
        ctxtBillingGSTIN2.SetText(GSTIN2);
        ctxtBillingGSTIN3.SetText(GSTIN3);

        ctxtCntperson.SetText(BillingObj[0].ContactPerson);
        $("#Salesbillingphone").val(BillingObj[0].Phone);
        ctxtEmail.SetText(BillingObj[0].Email);
   

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
       // ctxtDistance.SetText('');
        ctxtBillingGSTIN1.SetText('');
        ctxtBillingGSTIN2.SetText('');
        ctxtBillingGSTIN3.SetText('');

        ctxtCntperson.SetText('');
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
       // ctxtDistanceShipping.SetText(ShippingObj[0].Distance);

        var GSTIN = ShippingObj[0].GSTIN;
        var GSTIN1 = GSTIN.substring(0, 2);
        var GSTIN2 = GSTIN.substring(2, 12);
        var GSTIN3 = GSTIN.substring(12, 15);


        ctxtShippingGSTIN1.SetText(GSTIN1);
        ctxtShippingGSTIN2.SetText(GSTIN2);
        ctxtShippingGSTIN3.SetText(GSTIN3);

        ctxtshipCntPerson.SetText(ShippingObj[0].ContactPerson);
        ctxtshipPhone.SetText(ShippingObj[0].Phone);
        ctxtshipEmail.SetText(ShippingObj[0].Email);
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
       // ctxtDistanceShipping.SetText('');
        ctxtShippingGSTIN1.SetText('');
        ctxtShippingGSTIN2.SetText('');
        ctxtShippingGSTIN3.SetText('');

        ctxtshipCntPerson.SetText('');
        ctxtshipPhone.SetText('');
        ctxtshipEmail.SetText('');

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


function GeteShippingStateCode() {
    return $('#hdStateCodeShipping').val();
}

function GetShippingStateName() {
    return ctxtshippingState.GetText();
}


function GetBillingStateCode() {
    return $('#hdStateCodeBilling').val();
}

function GetBillingStateID() {
    return $('#hdStateIdBilling').val();
}
function GeteShippingStateID() {
    return $('#hdStateIdShipping').val();
}


function GetBillingStateName() {
    return ctxtbillingState.GetText();
}


