function BackClick() {
    var keyOpening = document.getElementById('hdnOpening').value;
    if (keyOpening != '') {
        var url = 'PurchaseOrderList.aspx?op=' + 'yes';
    }
    else {
        var url = 'PurchaseOrderList.aspx';
    }
    window.location.href = url;
}

function SettingTabStatus() {
    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
        page.GetTabByName('Billing/Shipping').SetEnabled(true);
    }
}

function CopyBillingAddresstoShipping(obj) {
    var chkbill = obj.GetChecked();
    if (chkbill == true) {
        $('#DivShipping').hide();
    }
    else {
        $('#DivShipping').show();
    }

    //  cComponentPanel.PerformCallback('Edit~BillingAddresstoShipping');
}
function CopyShippingAddresstoBilling(obj) {
    var chkship = obj.GetChecked();
    if (chkship == true) {
        $('#DivBilling').hide();
    }
    else {
        $('#DivBilling').show();
    }
    //cComponentPanel.PerformCallback('Edit~ShippingAddresstoBilling');
}



function btnSave_QuoteAddress() {
           
    checking = true;
    var chkbilling = cchkBilling.GetChecked();
    var chkShipping = cchkShipping.GetChecked();

    if (chkbilling == false && chkShipping == false) {
        // Billing Start
        if (ctxtAddress1.GetText() == '' || ctxtAddress1.GetText() == null) {
            $('#badd1').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#badd1').attr('style', 'display:none'); }

        if (CmbCountry.GetValue() == '' || CmbCountry.GetValue() == null || CmbCountry.GetValue() == 'select') {
            $('#bcountry').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#bcountry').attr('style', 'display:none'); }


        // State

        if (CmbState.GetValue() == '' || CmbState.GetValue() == null || CmbState.GetValue() == 'select') {
            $('#bstate').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#bstate').attr('style', 'display:none'); }

        // city
        if (CmbCity.GetValue() == '' || CmbCity.GetValue() == null || CmbCity.GetValue() == 'select') {
            $('#bcity').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#bcity').attr('style', 'display:none'); }

        // pin

        if (CmbPin.GetValue() == '' || CmbPin.GetValue() == null || CmbPin.GetValue() == 'select') {
            $('#bpin').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#bpin').attr('style', 'display:none'); }
        // Billing End

        // Shipping Start

        if (ctxtsAddress1.GetText() == '' || ctxtsAddress1.GetText() == null) {
            $('#sadd1').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#sadd1').attr('style', 'display:none'); }

        if (CmbCountry1.GetValue() == '' || CmbCountry1.GetValue() == null || CmbCountry1.GetValue() == 'select') {
            $('#scountry').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#scountry').attr('style', 'display:none'); }


        // State

        if (CmbState1.GetValue() == '' || CmbState1.GetValue() == null || CmbState1.GetValue() == 'select') {
            $('#sstate').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#sstate').attr('style', 'display:none'); }

        // city
        if (CmbCity1.GetValue() == '' || CmbCity1.GetValue() == null || CmbCity1.GetValue() == 'select') {
            $('#scity').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#scity').attr('style', 'display:none'); }

        // pin

        if (CmbPin1.GetValue() == '' || CmbPin1.GetValue() == null || CmbPin1.GetValue() == 'select') {
            $('#spin').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#spin').attr('style', 'display:none'); }

        // Shipping End

    }


    else if (chkbilling == true && chkShipping == false) {
        // Billing Start
        if (ctxtAddress1.GetText() == '' || ctxtAddress1.GetText() == null) {
            $('#badd1').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#badd1').attr('style', 'display:none'); }

        if (CmbCountry.GetValue() == '' || CmbCountry.GetValue() == null || CmbCountry.GetValue() == 'select') {
            $('#bcountry').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#bcountry').attr('style', 'display:none'); }


        // State

        if (CmbState.GetValue() == '' || CmbState.GetValue() == null || CmbState.GetValue() == 'select') {
            $('#bstate').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#bstate').attr('style', 'display:none'); }

        // city
        if (CmbCity.GetValue() == '' || CmbCity.GetValue() == null || CmbCity.GetValue() == 'select') {
            $('#bcity').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#bcity').attr('style', 'display:none'); }

        // pin

        if (CmbPin.GetValue() == '' || CmbPin.GetValue() == null || CmbPin.GetValue() == 'select') {
            $('#bpin').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#bpin').attr('style', 'display:none'); }
        // Billing End
    }

    else if (chkbilling == false && chkShipping == true) {
        // Shipping Start

        if (ctxtsAddress1.GetText() == '' || ctxtsAddress1.GetText() == null) {
            $('#sadd1').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#sadd1').attr('style', 'display:none'); }

        if (CmbCountry1.GetValue() == '' || CmbCountry1.GetValue() == null || CmbCountry1.GetValue() == 'select') {
            $('#scountry').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#scountry').attr('style', 'display:none'); }


        // State

        if (CmbState1.GetValue() == '' || CmbState1.GetValue() == null || CmbState1.GetValue() == 'select') {
            $('#sstate').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#sstate').attr('style', 'display:none'); }

        // city
        if (CmbCity1.GetValue() == '' || CmbCity1.GetValue() == null || CmbCity1.GetValue() == 'select') {
            $('#scity').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#scity').attr('style', 'display:none'); }

        // pin

        if (CmbPin1.GetValue() == '' || CmbPin1.GetValue() == null || CmbPin1.GetValue() == 'select') {
            $('#spin').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#spin').attr('style', 'display:none'); }

        // Shipping End

    }
    if (checking == true) {
                
        var custID = GetObjectID('hdnCustomerId').value;
        var chkbilling = cchkBilling.GetChecked();
        //var chkbilling = cchkBilling.GetChecked;
        var chkShipping = cchkShipping.GetChecked();
        //var chkShipping = cchkShipping.GetChecked;
        if ((chkbilling == false) && (chkShipping == false)) {
            cComponentPanel.PerformCallback('save~1');
        }
        else if ((chkbilling == true) && (chkShipping == false)) {
            cComponentPanel.PerformCallback('save~2');
        }
        else if ((chkbilling == false) && (chkShipping == true)) {
            cComponentPanel.PerformCallback('save~3');
        }
        //GetObjectID('hdnAddressDtl').value = '1';
        page.SetActiveTabIndex(0);
        gridLookup.Focus();
    }
    else {
        page.SetActiveTabIndex(1);
    }
}


//function ClosebillingLookup() {
//    billingLookup.ConfirmCurrentSelection();
//    billingLookup.HideDropDown();
//    billingLookup.Focus();
//}
//function CloseshippingLookup() {
//    shippingLookup.ConfirmCurrentSelection();
//    shippingLookup.HideDropDown();
//    shippingLookup.Focus();
//}

//Subhra-----23-01-2017-------
var Billing_state;
var Billing_city;
var Billing_pin;
var billing_area;

var Shipping_state;
var Shipping_city;
var Shipping_pin;
var Shipping_area;


function OnCountryChanged(cmbCountry) {

    CmbState.PerformCallback(cmbCountry.GetValue().toString());
}
function OnCountryChanged1(cmbCountry1) {
    CmbState1.PerformCallback(cmbCountry1.GetValue().toString());
}

function OnStateChanged(cmbState) {

    CmbCity.PerformCallback(cmbState.GetValue().toString());
}
function OnStateChanged1(cmbState1) {
    CmbCity1.PerformCallback(cmbState1.GetValue().toString());
}

function OnCityChanged(abc) {

    CmbPin.PerformCallback(abc.GetValue().toString());
    CmbArea.PerformCallback(abc.GetValue().toString());
}
function OnCityChanged1(abc) {
    CmbPin1.PerformCallback(abc.GetValue().toString());
    CmbArea1.PerformCallback(abc.GetValue().toString());


}
//----------------------------------
function OnChildCall(CmbCity) {

    OnCityChanged(CmbCity.GetValue());
    OnCityChanged(CmbCity1.GetValue());
}
function openAreaPage() {
    var left = (screen.width - 300) / 2;
    var top = (screen.height - 250) / 2;
    var cityid = CmbCity.GetValue();
    var cityname = CmbCity.GetText();
    var URL = '../Master/AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
    popupan.SetContentUrl(URL);
    popupan.Show();
}

function openAreaPageShip() {
    var left = (screen.width - 300) / 2;
    var top = (screen.height - 250) / 2;
    var cityid = CmbCity1.GetValue();
    var cityname = CmbCity1.GetText();
    var URL = '../Master/AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
    popupan.SetContentUrl(URL);
    popupan.Show();
}



function fn_PopOpen() {
    CmbCountry.SetSelectedIndex(-1);
    CmbCountry1.SetSelectedIndex(-1);
    CmbState.SetSelectedIndex(-1);
    CmbState1.SetSelectedIndex(-1);
    CmbCity.SetSelectedIndex(-1);
    CmbCity1.SetSelectedIndex(-1);
    CmbPin.SetSelectedIndex(-1);
    CmbPin1.SetSelectedIndex(-1);
    CmbArea.SetSelectedIndex(-1);
    CmbArea1.SetSelectedIndex(-1);
    cComponentPanel.PerformCallback('Edit~1');
    //cComponentPanel.PerformCallback('Edit~1' + GetObjectID('hdnAddressDtl').value); 
}

function cmbstate_endcallback(s, e) {
    if (Billing_state != 0) {
        s.SetValue(Billing_state);
    }
    CmbCity.PerformCallback(CmbState.GetValue());
    //Billing_state = 0;
}
function cmbshipstate_endcallback(s, e) {
    if (Shipping_state != 0) {
        s.SetValue(Shipping_state);
    }
    CmbCity1.PerformCallback(CmbState1.GetValue());
    Shipping_state = 0;
}

function cmbcity_endcallback(s, e) {
    if (Billing_city != 0) {
        s.SetValue(Billing_city);
    }
    CmbPin.PerformCallback(CmbCity.GetValue());
    CmbArea.PerformCallback(CmbCity.GetValue());
    Billing_city = 0;

}
function cmbshipcity_endcallback(s, e) {
    if (Shipping_city != 0) {
        s.SetValue(Shipping_city);
    }
    CmbPin1.PerformCallback(CmbCity1.GetValue());
    CmbArea1.PerformCallback(CmbCity1.GetValue());
    Shipping_city = 0;

}

function cmbPin_endcallback(s, e) {
    if (Billing_pin != 0) {
        s.SetValue(Billing_pin);
    }
    if (Billing_pin != null || Billing_pin != '' || Billing_pin != '0') {
        cchkBilling.SetEnabled(true);
    }
    Billing_pin = 0;
}
function cmbshipPin_endcallback(s, e) {
    if (Shipping_pin != 0) {
        s.SetValue(Shipping_pin);
    }
    if (Shipping_pin != null || Shipping_pin != '' || Shipping_pin != '0') {
        cchkShipping.SetEnabled(true);
    }
    Shipping_pin = 0;
}

function cmbArea_endcallback(s, e) {
    if (billing_area != 0) {
        s.SetValue(billing_area);
    }
    billing_area = 0;
}

function cmbshipArea_endcallback(s, e) {
    if (Shipping_area != 0) {
        s.SetValue(Shipping_area);
    }
    Shipping_area = 0;
}

//function Popup_SalesQuote_EndCallBack() {
//    if (Popup_SalesQuote.cpshow != null) {


//        CmbAddressType.SetText(Popup_SalesQuote.cpshow.split('~')[0]);
//        ctxtAddress1.SetText(Popup_SalesQuote.cpshow.split('~')[1]);
//        ctxtAddress2.SetText(Popup_SalesQuote.cpshow.split('~')[2]);
//        ctxtAddress3.SetText(Popup_SalesQuote.cpshow.split('~')[3]);
//        ctxtlandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
//        CmbCountry.SetValue(Popup_SalesQuote.cpshow.split('~')[5]);
//        Billing_state = Popup_SalesQuote.cpshow.split('~')[6];
//        Billing_city = Popup_SalesQuote.cpshow.split('~')[7];
//        Billing_pin = Popup_SalesQuote.cpshow.split('~')[8];
//        billing_area = Popup_SalesQuote.cpshow.split('~')[9];
//        CmbState.PerformCallback(CmbCountry.GetValue());
//    }

//    if (Popup_SalesQuote.cpshowShip != null) {


//        CmbAddressType1.SetText(Popup_SalesQuote.cpshowShip.split('~')[0]);
//        ctxtsAddress1.SetText(Popup_SalesQuote.cpshowShip.split('~')[1]);
//        ctxtsAddress2.SetText(Popup_SalesQuote.cpshowShip.split('~')[2]);
//        ctxtsAddress3.SetText(Popup_SalesQuote.cpshowShip.split('~')[3]);
//        ctxtslandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
//        CmbCountry1.SetValue(Popup_SalesQuote.cpshowShip.split('~')[5]);
//        Shipping_state = Popup_SalesQuote.cpshowShip.split('~')[6];
//        Shipping_city = Popup_SalesQuote.cpshowShip.split('~')[7];
//        Shipping_pin = Popup_SalesQuote.cpshowShip.split('~')[8];
//        Shipping_area = Popup_SalesQuote.cpshowShip.split('~')[9];
//        CmbState1.PerformCallback(CmbCountry1.GetValue());
//    }

//}
function Panel_endcallback() {
            
    var billingStatus = null;
    var shippingStatus = null;
    if (cComponentPanel.cpshow != null) {


        //CmbAddressType.SetText(cComponentPanel.cpshow.split('~')[0]);
        ctxtAddress1.SetText(cComponentPanel.cpshow.split('~')[1]);
        ctxtAddress2.SetText(cComponentPanel.cpshow.split('~')[2]);
        ctxtAddress3.SetText(cComponentPanel.cpshow.split('~')[3]);
        ctxtlandmark.SetText(cComponentPanel.cpshow.split('~')[4]);
        var bcon = cComponentPanel.cpshow.split('~')[5];
        if (bcon == '') {
            CmbCountry.SetSelectedIndex(-1);
        }
        else {
            CmbCountry.SetValue(cComponentPanel.cpshow.split('~')[5]);
        }

        var bsta = cComponentPanel.cpshow.split('~')[6];
        if (bsta == '') {
            CmbState.SetSelectedIndex(-1);
            Billing_state = 0;
        }
        else {
            Billing_state = cComponentPanel.cpshow.split('~')[6];
        }
        var bcity = cComponentPanel.cpshow.split('~')[7];
        if (bcity == '') {
            CmbCity.SetSelectedIndex(-1);
            Billing_city = 0;
        }
        else {
            Billing_city = cComponentPanel.cpshow.split('~')[7];
        }

        var bpin = cComponentPanel.cpshow.split('~')[8];
        if (bpin == '') {
            CmbPin.SetSelectedIndex(-1);
            Billing_pin = 0;
        }
        else {
            Billing_pin = cComponentPanel.cpshow.split('~')[8];
        }

        var barea = cComponentPanel.cpshow.split('~')[9];
        if (barea == '') {
            CmbArea.SetSelectedIndex(-1);
            billing_area = 0;
        }
        else {
            billing_area = cComponentPanel.cpshow.split('~')[9];
        }
        //CmbCountry.SetValue(cComponentPanel.cpshow.split('~')[5]);
        //Billing_state = cComponentPanel.cpshow.split('~')[6];
        //Billing_city = cComponentPanel.cpshow.split('~')[7];
        //Billing_pin = cComponentPanel.cpshow.split('~')[8];
        //billing_area = cComponentPanel.cpshow.split('~')[9];
        billingStatus = cComponentPanel.cpshow.split('~')[10];
        var countryid = CmbCountry.GetValue()
        if (countryid != null && countryid != '' && countryid != '0') {
            CmbState.PerformCallback(countryid);
        }
    }

    if (cComponentPanel.cpshowShip != null) {

        //CmbAddressType1.SetText(cComponentPanel.cpshowShip.split('~')[0]);
        ctxtsAddress1.SetText(cComponentPanel.cpshowShip.split('~')[1]);

        ctxtsAddress2.SetText(cComponentPanel.cpshowShip.split('~')[2]);
        ctxtsAddress3.SetText(cComponentPanel.cpshowShip.split('~')[3]);
        ctxtslandmark.SetText(cComponentPanel.cpshowShip.split('~')[4]);
        var scon = cComponentPanel.cpshowShip.split('~')[5];
        if (scon == '') {
            CmbCountry1.SetSelectedIndex(-1);
        }
        else {
            CmbCountry1.SetValue(cComponentPanel.cpshowShip.split('~')[5]);
        }
        var ssta = cComponentPanel.cpshowShip.split('~')[6];
        if (ssta == '') {
            CmbState1.SetSelectedIndex(-1);
        }
        else {
            Shipping_state = cComponentPanel.cpshowShip.split('~')[6];
        }
        var scity = cComponentPanel.cpshowShip.split('~')[7];
        if (scity == '') {
            CmbCity1.SetSelectedIndex(-1);
        }
        else {
            Shipping_city = cComponentPanel.cpshowShip.split('~')[7];
        }

        var spin = cComponentPanel.cpshowShip.split('~')[8];
        if (spin == '') {
            CmbPin1.SetSelectedIndex(-1);
        }
        else {
            Shipping_pin = cComponentPanel.cpshowShip.split('~')[8];
        }

        var sarea = cComponentPanel.cpshowShip.split('~')[9];
        if (sarea == '') {
            CmbArea1.SetSelectedIndex(-1);
        }
        else {
            Shipping_area = cComponentPanel.cpshowShip.split('~')[9];
        }
        //CmbCountry1.SetValue(cComponentPanel.cpshowShip.split('~')[5]);
        //Shipping_state = 
        //Shipping_city = cComponentPanel.cpshowShip.split('~')[7];
        //Shipping_pin = cComponentPanel.cpshowShip.split('~')[8];
        //Shipping_area = cComponentPanel.cpshowShip.split('~')[9];
        shippingStatus = cComponentPanel.cpshowShip.split('~')[10];
        var countryid1 = CmbCountry1.GetValue()
        if (countryid1 != null && countryid1 != '' && countryid1 != '0') {
            CmbState1.PerformCallback(countryid1);
        }
        //CmbState1.PerformCallback(CmbCountry1.GetValue());
    }
    if (billingStatus == 'Y' && shippingStatus == 'N') {
        cchkBilling.SetEnabled(true);
        cchkShipping.SetEnabled(false);
    }
    else if (billingStatus == 'N' && shippingStatus == 'Y') {
        cchkBilling.SetEnabled(false);
        cchkShipping.SetEnabled(true);

    }
    else if (billingStatus == 'Y' && shippingStatus == 'Y') {
        cchkBilling.SetEnabled(false);
        cchkShipping.SetEnabled(false);

    }
    else if (billingStatus == 'N' && shippingStatus == 'N') {
        cchkBilling.SetEnabled(false);
        cchkShipping.SetEnabled(false);
    }
    if (grid.cpView == "1") {
        ctxtAddress1.SetEnabled(false);
        ctxtAddress2.SetEnabled(false);
        ctxtAddress3.SetEnabled(false);
        ctxtlandmark.SetEnabled(false);
        CmbCountry.SetEnabled(false);
        CmbState.SetEnabled(false);
        CmbCity.SetEnabled(false);
        CmbPin.SetEnabled(false);
        CmbArea.SetEnabled(false);
        ctxtsAddress1.SetEnabled(false);
        ctxtsAddress2.SetEnabled(false);
        ctxtsAddress3.SetEnabled(false);
        ctxtslandmark.SetEnabled(false);
        CmbCountry1.SetEnabled(false);
        CmbState1.SetEnabled(false);
        CmbCity1.SetEnabled(false);
        CmbPin1.SetEnabled(false);
        CmbArea1.SetEnabled(false);
        cchkShipping.SetVisible(false);
        cchkBilling.SetVisible(false);
        cButton1.SetVisible(false);
        cbtnSave_citys.SetVisible(false);
    }
}

function disp_prompt(name) {

    if (name == "tab0") {
        gridLookup.Focus();
        //alert(name);
        //document.location.href = "SalesQuotation.aspx?";
    }
    if (name == "tab1") {
        var custID = GetObjectID('hdnCustomerId').value;
        if (custID == null && custID == '') {
            jAlert('Please select a customer');
            page.SetActiveTabIndex(0);
            return;
        }
        else {
            page.SetActiveTabIndex(1);
            fn_PopOpen();
        }
    }
}

//<%-- ------Subhra Address and Billing Section End-----25-01-2017---------%>

    function deleteAllRows() {
        var frontRow = 0;
        var backRow = -1;
        for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
            grid.DeleteRow(frontRow);
            grid.DeleteRow(backRow);
            backRow--;
            frontRow++;
        }

    }
function onBranchItems() {
    GetIndentReqNoOnLoad();

    grid.batchEditApi.StartEdit(-1, 1);
    var accountingDataMin = grid.GetEditor('ProductName').GetValue();
    grid.batchEditApi.EndEdit();
    console.log(accountingDataMin);

    grid.batchEditApi.StartEdit(0, 1);
    var accountingDataplus = grid.GetEditor('ProductName').GetValue();
    console.log(accountingDataplus);
    grid.batchEditApi.EndEdit();

    if (accountingDataMin != null || accountingDataplus != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

            if (r == true) {
                cQuotationComponentPanel.PerformCallback('BindNullGrid');
                //  gridquotationLookup.SetValue(0);

            } else {

            }
        });
    }
}
function disp_prompt(name) {
            
    if (name == "tab0") {
        gridLookup.Focus();
        //alert(name);
        //document.location.href = "SalesQuotation.aspx?";
    }
    if (name == "tab1") {
        var custID = GetObjectID('hdnCustomerId').value;
        if (custID == null && custID == '') {
            jAlert('Please select a customer');
            page.SetActiveTabIndex(0);

            return;
        }
        else {
            page.SetActiveTabIndex(1);
            fn_PopOpen();
        }
    }
}


var globalRowIndex;
var rowEditCtrl;

//............................Product Pazination..............
function ChangeState(value) {

    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}
function ctaxUpdatePanelEndCall(s, e) {
    if (ctaxUpdatePanel.cpstock != null) {
        divAvailableStk.style.display = "block";
        // divpopupAvailableStock.style.display = "block";

        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
        document.getElementById('lblAvailableStk').innerHTML = ctaxUpdatePanel.cpstock;

        ctaxUpdatePanel.cpstock = null;
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }

    return false;
}
function ProductsGotFocusFromID(s, e) {
    pageheaderContent.style.display = "block";
    divAvailableStk.style.display = "block";
    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
    //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[13];
    var Packing_Factor = SpliteDetails[14];
    var Packing_UOM = SpliteDetails[15];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        //divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strProductName);
    $('#lblbranchName').text(strBranch);

    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}
function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
}
function ProductSelected(s, e) {

    if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
        return;
    }
    var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
    var ProductCode = cproductLookUp.GetValue();
    if (!ProductCode) {
        LookUpData = null;
    }
    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("gvColProduct").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    //console.log(LookUpData);
    pageheaderContent.style.display = "block";
    divAvailableStk.style.display = "block";
    cddl_AmountAre.SetEnabled(false);

    var tbDescription = grid.GetEditor("gvColDiscription");
    var tbUOM = grid.GetEditor("gvColUOM");
    var tbSalePrice = grid.GetEditor("gvColStockPurchasePrice");

    //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];

    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];

    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    if (strRate == 0) {
        strSalePrice = strSalePrice;
    }
    else {
        strSalePrice = strSalePrice / strRate;
    }

    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbSalePrice.SetValue(strSalePrice);

    grid.GetEditor("gvColQuantity").SetValue("0.00");
    grid.GetEditor("gvColDiscount").SetValue("0.00");
    grid.GetEditor("gvColAmount").SetValue("0.00");
    grid.GetEditor("gvColTaxAmount").SetValue("0.00");
    grid.GetEditor("gvColTotalAmountINR").SetValue("0.00");

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strDescription);
    $('#lblbranchName').text(strBranch);

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        // divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }
    //divPacking.style.display = "none";

    //lblbranchName lblProduct
    //tbStkUOM.SetValue(strStkUOM);
    //tbStockQuantity.SetValue("0");
    //Debjyoti
    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    grid.batchEditApi.StartEdit(globalRowIndex, 6);
}
function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    if (e.htmlEvent.key == "Tab") {

        s.OnButtonClick(0);
    }
}
function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {

        if (cproductLookUp.Clear()) {
            cProductpopUp.Show();
            cproductLookUp.Focus();
            cproductLookUp.ShowDropDown();
        }
    }
}
//..............End Product........................
//.............Available Stock Div Show............................
function ProductsGotFocus(s, e) {
    pageheaderContent.style.display = "block";
    divAvailableStk.style.display = "block";
    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
    //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[13];
    var Packing_Factor = SpliteDetails[14];
    var Packing_UOM = SpliteDetails[15];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        //  divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strProductName);
    $('#lblbranchName').text(strBranch);

    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}
function acpAvailableStockEndCall(s, e) {
    if (cacpAvailableStock.cpstock != null) {
        divAvailableStk.style.display = "block";
        //divpopupAvailableStock.style.display = "block";

        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
        document.getElementById('lblAvailableStk').innerHTML = cacpAvailableStock.cpstock;


        cCmbWarehouse.cpstock = null;
    }
}
//................Available Stock Div Show....................
//Code for UDF Control 
function OpenUdf(s, e) {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/Opening/ERP/frm_BranchUdfPopUp.aspx?Type=PO&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}
// End Udf Code
//............Check Unique   Purchase Order................
function txtBillNo_TextChanged() {

    var VoucherNo = document.getElementById("txtVoucherNo").value;

    $.ajax({
        type: "POST",
        url: "PurchaseOrder.aspx/CheckUniqueName",
        data: JSON.stringify({ VoucherNo: VoucherNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d;

            if (data == true) {
                $("#MandatoryBillNo").show();

                document.getElementById("txtVoucherNo").value = '';
                document.getElementById("txtVoucherNo").focus();
            }
            else {
                $("#MandatoryBillNo").hide();
            }
        }
    });
}
//..................Rate........................
function ReBindGrid_Currency() {
    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'gvColProduct') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'gvColProduct')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'gvColProduct') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'gvColProduct')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }

    if (IsProduct == "Y") {
        $('#hdfIsDelete').val('D');
        grid.UpdateEdit();
        //   grid.PerformCallback('CurrencyChangeDisplay');
    }
    cddl_AmountAre.Focus();
}
//...............end.........................
//...............PopulateVAT........................
function PopulateGSTCSTVAT(e) {
    var key = cddl_AmountAre.GetValue();
    //deleteAllRows();

    if (key == 1) {

        grid.GetEditor('gvColTaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(false);
        //cddlVatGstCst.PerformCallback('1');
        cddlVatGstCst.SetSelectedIndex(-1);
        cbtn_SaveRecords.SetVisible(true);
        grid.GetEditor('gvColProduct').Focus();
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }

    }
    else if (key == 2) {
        grid.GetEditor('gvColTaxAmount').SetEnabled(true);

        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');
        cddlVatGstCst.Focus();
        cbtn_SaveRecords.SetVisible(true);
    }
    else if (key == 3) {

        grid.GetEditor('gvColTaxAmount').SetEnabled(false);

        //cddlVatGstCst.PerformCallback('3');
        cddlVatGstCst.SetSelectedIndex(-1);
        cddlVatGstCst.SetEnabled(false);
        cbtn_SaveRecords.SetVisible(false);
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }


    }

}
function Keypressevt() {

    if (event.keyCode == 13) {

        //run code for Ctrl+X -- ie, Save & Exit! 
        SaveWarehouse();
        return false;
    }
}

function SetFocusonDemand(e) {
    var key = cddl_AmountAre.GetValue();
    if (key == '1' || key == '3') {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 3);
        }
    }
    else if (key == '2') {
        cddlVatGstCst.Focus();
    }

}

//.................End PopulateVAT...............
//................Amount Calculation.........................
function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}
function taxAmtButnClick1(s, e) {
    // console.log(grid.GetFocusedRowIndex());
    rowEditCtrl = s;
}
function taxAmtButnClick(s, e) {
    if (e.buttonIndex == 0) {

        if (cddl_AmountAre.GetValue() != null) {
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";

            if (ProductID.trim() != "") {

                document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                ctxtTaxTotAmt.SetValue(0);
                ccmbGstCstVat.SetSelectedIndex(0);
                $('.RecalculateInline').hide();
                caspxTaxpopUp.Show();
                //Set Product Gross Amount and Net Amount

                var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                //var strRate = "1";
                var strStkUOM = SpliteDetails[4];
                // var strSalePrice = SpliteDetails[6];
                var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "";
                if (strRate == 0) {
                    strRate = 1;
                }
                //  console.log("1");
                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = Math.round(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                clblTaxProdGrossAmt.SetText(Amount);
                clblProdNetAmt.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue()).toFixed(2));
                document.getElementById('HdProdGrossAmt').value = Amount;
                document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('gvColAmount').GetValue()).toFixed(2);

                //End Here

                //Set Discount Here
                if (parseFloat(grid.GetEditor('gvColDiscount').GetValue()) > 0) {
                    var discount = Math.round((Amount * grid.GetEditor('gvColDiscount').GetValue() / 100)).toFixed(2);
                    clblTaxDiscount.SetText(discount);
                }
                else {
                    clblTaxDiscount.SetText('0.00');
                }
                //End Here 


                //Checking is gstcstvat will be hidden or not
                if (cddl_AmountAre.GetValue() == "2") {
                    $('.GstCstvatClass').hide();
                    $('.gstGrossAmount').show();
                    clblTaxableGross.SetText("(Taxable)");
                    clblTaxableNet.SetText("(Taxable)");
                    $('.gstNetAmount').show();
                    //Set Gross Amount with GstValue
                    //Get The rate of Gst
                    var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
                    if (gstRate) {
                        if (gstRate != 0) {
                            var gstDis = (gstRate / 100) + 1;
                            if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                                $('.gstNetAmount').hide();
                                clblTaxProdGrossAmt.SetText(Math.round(Amount / gstDis).toFixed(2));
                                document.getElementById('HdProdGrossAmt').value = Math.round(Amount / gstDis).toFixed(2);
                                clblGstForGross.SetText(Math.round(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                clblTaxableNet.SetText("");
                            }
                            else {
                                $('.gstGrossAmount').hide();
                                clblProdNetAmt.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2));
                                document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2);
                                clblGstForNet.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
                                clblTaxableGross.SetText("");
                            }
                        }

                    } else {
                        $('.gstGrossAmount').hide();
                        $('.gstNetAmount').hide();
                        clblTaxableGross.SetText("");
                        clblTaxableNet.SetText("");
                    }
                }
                else if (cddl_AmountAre.GetValue() == "1") {
                    $('.GstCstvatClass').show();
                    $('.gstGrossAmount').hide();
                    $('.gstNetAmount').hide();
                    clblTaxableGross.SetText("");
                    clblTaxableNet.SetText("");


                    //Get Customer Shipping StateCode
                    var shippingStCode = '';
                    if (cchkBilling.GetValue()) {
                        shippingStCode = CmbState.GetText();
                    }
                    else {
                        shippingStCode = CmbState1.GetText();
                    }
                    shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                    //Debjyoti 09032017
                    if (shippingStCode.trim() != '') {
                        for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                            //Check if gstin is blank then delete all tax
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {

                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {

                                    //if its state is union territories then only UTGST will apply
                                    if (shippingStCode == "4" || shippingStCode == "35" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "31" || shippingStCode == "34") {
                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S') {
                                            ccmbGstCstVat.RemoveItem(cmbCount);
                                            cmbCount--;
                                        }
                                    }
                                    else {
                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                            ccmbGstCstVat.RemoveItem(cmbCount);
                                            cmbCount--;
                                        }
                                    }
                                } else {
                                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                        ccmbGstCstVat.RemoveItem(cmbCount);
                                        cmbCount--;
                                    }
                                }
                            } else {
                                //remove tax because GSTIN is not define
                                ccmbGstCstVat.RemoveItem(cmbCount);
                                cmbCount--;
                            }
                        }
                    }

                }
                //End here

                if (globalRowIndex > -1) {
                    cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                } else {

                    cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                    //Set default combo
                    cgridTax.cpComboCode = grid.GetEditor('gvColProduct').GetValue().split('||@||')[9];
                }

                ctxtprodBasicAmt.SetValue(grid.GetEditor('gvColAmount').GetValue());
            } else {
                grid.batchEditApi.StartEdit(globalRowIndex, 13);
            }
        }
    }
}

function SalePriceTextChange(s, e) {
    pageheaderContent.style.display = "block";
    divAvailableStk.style.display = "block";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var ProductID = grid.GetEditor('gvColProduct').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
        var strFactor = SpliteDetails[14]; //Packing_Factor 
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        //console.log("Rate"+strRate);
        var strProductID = SpliteDetails[0];
        var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();

        //var strRate = "1";
        var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
        var strSalePrice = SpliteDetails[6];// purchase Price
        //console.log("PurchasePrice"+strSalePrice);

        if (strRate == 0) {
            strRate = 1;
        }
        if (strSalePrice == 0.00000) {
            strSalePrice = 1;
        }

        var StockQuantity = strMultiplier * QuantityValue;
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        // alert("here" + Amount);
        $('#lblbranchName').text(strBranch);

        var IsPackingActive = SpliteDetails[13];//IsPackingActive
        var Packing_Factor = SpliteDetails[14];//Packing_Factor
        var Packing_UOM = SpliteDetails[15];//Packing_UOM
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
            // divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }



        var tbAmount = grid.GetEditor("gvColAmount");
        tbAmount.SetValue(Amount);

        var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
        tbTotalAmount.SetValue(Amount);

        DiscountTextChange(s, e);
        //.........AvailableStock.............
        cacpAvailableStock.PerformCallback(strProductID);
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('gvColQuantity').SetValue('0');
        grid.GetEditor('gvColProduct').Focus();
    }
}



function QuantityTextChange(s, e) {

    pageheaderContent.style.display = "block";
    divAvailableStk.style.display = "block";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var ProductID = grid.GetEditor('gvColProduct').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        // console.log(SpliteDetails)
        var strMultiplier = SpliteDetails[7];//Conversion_Multiplier
        //console.log("Multiplier"+strMultiplier);
        var strFactor = SpliteDetails[14]; //Packing_Factor
        //console.log("Factor"+strFactor);
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        //console.log("Rate"+strRate);
        var strProductID = SpliteDetails[0];
        var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();

        //var strRate = "1";
        var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
        //var strSalePrice = SpliteDetails[6];// purchase Price
        var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
        console.log("PurchasePrice" + strSalePrice);

        if (strRate == 0) {
            strRate = 1;
        }
        if (strSalePrice == 0.00000) {
            strSalePrice = 1;
        }

        var StockQuantity = strMultiplier * QuantityValue;
        console.log("StockQuantity" + StockQuantity);
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        // alert("here" + Amount);
        $('#lblbranchName').text(strBranch);

        var IsPackingActive = SpliteDetails[13];//IsPackingActive
        var Packing_Factor = SpliteDetails[14];//Packing_Factor
        var Packing_UOM = SpliteDetails[15];//Packing_UOM
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
            // divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }



        var tbAmount = grid.GetEditor("gvColAmount");
        tbAmount.SetValue(Amount);

        var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
        tbTotalAmount.SetValue(Amount);

        DiscountTextChange(s, e);
        //.........AvailableStock.............
        cacpAvailableStock.PerformCallback(strProductID);
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('gvColQuantity').SetValue('0');
        grid.GetEditor('gvColProduct').Focus();
    }
}
function DiscountTextChange(s, e) {
    //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var Discount = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";

    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var ProductID = grid.GetEditor('gvColProduct').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strFactor = SpliteDetails[14];
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
        if (strSalePrice == '0') {
            strSalePrice = SpliteDetails[6];
        }
        if (strRate == 0) {
            strRate = 1;
        }
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
        //var Amount = grid.GetEditor("gvColAmount");

        var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        var tbAmount = grid.GetEditor("gvColAmount");
        tbAmount.SetValue(amountAfterDiscount);

        var IsPackingActive = SpliteDetails[13];
        var Packing_Factor = SpliteDetails[14];
        var Packing_UOM = SpliteDetails[15];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
            // divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }

        var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
        tbTotalAmount.SetValue(amountAfterDiscount);
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('gvColDiscount').SetValue('0');
        grid.GetEditor('gvColProduct').Focus();
    }
    //Debjyoti 
    grid.GetEditor('gvColTaxAmount').SetValue(0);
    ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());

}

//......................Amount Calculation End.......................
/*........................Tax Start...........................*/
var taxAmountGlobalCharges;
var chargejsonTax;
var taxAmountGlobal;
var GlobalCurChargeTaxAmt;
var ChargegstcstvatGlobalName;
var GlobalCurTaxAmt = 0;
var rowEditCtrl;
var globalRowIndex;
var globalTaxRowIndex;
var gstcstvatGlobalName;
var taxJson;
function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cPopup_Taxes.Hide();
}
//Set Running Total for Charges And Tax 
function SetChargesRunningTotal() {
    var runningTot = parseFloat(ctxtProductNetAmount.GetValue());

    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        if (chargejsonTax[i].applicableOn == "R") {
            gridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
            var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
            var GlobalTaxAmt = 0;

            var Percentage = gridTax.GetEditor("Percentage").GetText();
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

            if (sign == '(+)') {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                GlobalTaxAmt = 0;
            }

            SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());


        }
        runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.batchEditApi.EndEdit();
    }
}
function GetPercentageData() {
    var Amount = ctxtProductAmount.GetValue();
    var GlobalTaxAmt = 0;
    var noofvisiblerows = gridTax.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, totalAmount = 0;
    for (i = 0 ; cnt <= noofvisiblerows ; i++) {
        var totLength = gridTax.batchEditApi.GetCellValue(i, 'TaxName').length;
        var sign = gridTax.batchEditApi.GetCellValue(i, 'TaxName').substring(totLength - 3);
        var DisAmount = (gridTax.batchEditApi.GetCellValue(i, 'Amount') != null) ? (gridTax.batchEditApi.GetCellValue(i, 'Amount')) : "0";

        if (sign == '(+)') {
            sumAmount = sumAmount + parseFloat(DisAmount);
        }
        else {
            sumAmount = sumAmount - parseFloat(DisAmount);
        }

        cnt++;
    }

    totalAmount = (parseFloat(Amount)) + (parseFloat(sumAmount));
    // ctxtTotalAmount.SetValue(totalAmount);
}
function Save_TaxesClick() {
    grid.batchEditApi.EndEdit();
    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

    cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var Amount = (grid.batchEditApi.GetCellValue(i, 'gvColAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColAmount')) : "0";
        var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount')) : "0";
        var Discount = (grid.batchEditApi.GetCellValue(i, 'gvColDiscount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColDiscount')) : "0";
        var NetAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR')) : "0";
        var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        sumAmount = sumAmount + parseFloat(Amount);
        sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
        sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
        sumNetAmount = sumNetAmount + parseFloat(NetAmount);

        cnt++;
    }

    if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
        cnt = 1;
        for (i = 0 ; cnt <= noofvisiblerows ; i++) {
            var Amount = (grid.batchEditApi.GetCellValue(i, 'gvColAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColAmount')) : "0";
            var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount')) : "0";
            var Discount = (grid.batchEditApi.GetCellValue(i, 'gvColDiscount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColDiscount')) : "0";
            var NetAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR')) : "0";
            var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            sumAmount = sumAmount + parseFloat(Amount);
            sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
            sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
            sumNetAmount = sumNetAmount + parseFloat(NetAmount);

            cnt++;
        }
    }

    //Debjyoti 
    document.getElementById('HdChargeProdAmt').value = sumAmount;
    document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
    //End Here

    ctxtProductAmount.SetValue(Math.round(sumAmount).toFixed(2));
    ctxtProductTaxAmount.SetValue(Math.round(sumTaxAmount).toFixed(2));
    ctxtProductDiscount.SetValue(Math.round(sumDiscount).toFixed(2));
    ctxtProductNetAmount.SetValue(Math.round(sumNetAmount).toFixed(2));
    clblChargesTaxableGross.SetText("");
    clblChargesTaxableNet.SetText("");

    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {

        $('.lblChargesGSTforGross').show();
        $('.lblChargesGSTforNet').show();

        //Set Gross Amount with GstValue
        //Get The rate of Gst
        var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
        if (gstRate) {
            if (gstRate != 0) {
                var gstDis = (gstRate / 100) + 1;
                if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                    $('.lblChargesGSTforNet').hide();
                    ctxtProductAmount.SetText(Math.round(sumAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdAmt').value = Math.round(sumAmount / gstDis).toFixed(2);
                    clblChargesGSTforGross.SetText(Math.round(sumAmount - parseFloat(document.getElementById('HdChargeProdAmt').value)).toFixed(2));
                    clblChargesTaxableGross.SetText("(Taxable)");

                }
                else {
                    $('.lblChargesGSTforGross').hide();
                    ctxtProductNetAmount.SetText(Math.round(sumNetAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdNetAmt').value = Math.round(sumNetAmount / gstDis).toFixed(2);
                    clblChargesGSTforNet.SetText(Math.round(sumNetAmount - parseFloat(document.getElementById('HdChargeProdNetAmt').value)).toFixed(2));
                    clblChargesTaxableNet.SetText("(Taxable)");
                }
            }

        } else {
            $('.lblChargesGSTforGross').hide();
            $('.lblChargesGSTforNet').hide();
        }
    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.lblChargesGSTforGross').hide();
        $('.lblChargesGSTforNet').hide();
    }
    //End here





    //Set Total amount
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));

    gridTax.PerformCallback('Display');
    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {
        $('.chargeGstCstvatClass').hide();
    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.chargeGstCstvatClass').show();
    }
    //End here
    $('.RecalculateCharge').hide();
    cPopup_Taxes.Show();
    gridTax.StartEditRow(0);
}
//function SetFocusonDemand(e) {
//    var key = cddl_AmountAre.GetValue();
//    if (key == '1' || key == '3') {
//        if (grid.GetVisibleRowsOnPage() == 1) {
//            grid.batchEditApi.StartEdit(-1, 2);
//        }
//    }
//    else if (key == '2') {
//        cddlVatGstCst.Focus();
//    }

//}
function QuotationTaxAmountTextChange(s, e) {
    //var Amount = ctxtProductAmount.GetValue();
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    //var Percentage = (gridTax.GetEditor('Percentage').GetValue() != null) ? gridTax.GetEditor('Percentage').GetValue() : "0";
    //var Percentage = s.GetText();
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    //Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges);
        ctxtTotalAmount.SetText(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }

    RecalCulateTaxTotalAmountCharges();
}
function OnTaxEndCallback(s, e) {
    GetPercentageData();
    $('.gridTaxClass').show();
    if (gridTax.GetVisibleRowsOnPage() == 0) {
        $('.gridTaxClass').hide();
        ccmbGstCstVatcharge.Focus();
    }
    else {
        gridTax.StartEditRow(0);
    }
    //check Json data
    if (gridTax.cpJsonChargeData) {
        if (gridTax.cpJsonChargeData != "") {
            chargejsonTax = JSON.parse(gridTax.cpJsonChargeData);
            gridTax.cpJsonChargeData = null;
        }
    }

    //Set Total Charges And total Amount
    if (gridTax.cpTotalCharges) {
        if (gridTax.cpTotalCharges != "") {
            ctxtQuoteTaxTotalAmt.SetValue(gridTax.cpTotalCharges);
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
            gridTax.cpTotalCharges = null;
        }
    }


    SetChargesRunningTotal();
    ShowTaxPopUp("IN");
}
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}
function PercentageTextChange(s, e) {
    //var Amount = ctxtProductAmount.GetValue();
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    //var Percentage = (gridTax.GetEditor('Percentage').GetValue() != null) ? gridTax.GetEditor('Percentage').GetValue() : "0";
    var Percentage = s.GetText();
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
        GlobalTaxAmt = 0;
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        GlobalTaxAmt = 0;
    }

    SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
    SetChargesRunningTotal();

    RecalCulateTaxTotalAmountCharges();
}
function RecalCulateTaxTotalAmountCharges() {
    var totalTaxAmount = 0;
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            totalTaxAmount = totalTaxAmount + parseFloat(gridTax.GetEditor("Amount").GetValue());
        } else {
            totalTaxAmount = totalTaxAmount - parseFloat(gridTax.GetEditor("Amount").GetValue());
        }

        gridTax.batchEditApi.EndEdit();
    }

    totalTaxAmount = totalTaxAmount + parseFloat(ctxtGstCstVatCharge.GetValue());

    ctxtQuoteTaxTotalAmt.SetValue(Math.round(totalTaxAmount));
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}

function ShowTaxPopUp(type) {
    if (type == "IY") {
        $('#ContentErrorMsg').hide();
        $('#content-6').show();


        if (ccmbGstCstVat.GetItemCount() <= 1) {
            $('.InlineTaxClass').hide();
        } else {
            $('.InlineTaxClass').show();
        }
        if (cgridTax.GetVisibleRowsOnPage() < 1) {
            $('.cgridTaxClass').hide();

        } else {
            $('.cgridTaxClass').show();
        }

        if (ccmbGstCstVat.GetItemCount() <= 1 && cgridTax.GetVisibleRowsOnPage() < 1) {
            $('#ContentErrorMsg').show();
            $('#content-6').hide();
        }
    }
    if (type == "IN") {
        $('#ErrorMsgCharges').hide();
        $('#content-5').show();

        if (ccmbGstCstVatcharge.GetItemCount() <= 1) {
            $('.chargesDDownTaxClass').hide();
        } else {
            $('.chargesDDownTaxClass').show();
        }
        if (gridTax.GetVisibleRowsOnPage() < 1) {
            $('.gridTaxClass').hide();

        } else {
            $('.gridTaxClass').show();
        }

        if (ccmbGstCstVatcharge.GetItemCount() <= 1 && gridTax.GetVisibleRowsOnPage() < 1) {
            $('#ErrorMsgCharges').show();
            $('#content-5').hide();
        }
    }
}
function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}
function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}


function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}

function taxAmountLostFocus(s, e) {
    var finalTaxAmt = parseFloat(s.GetValue());
    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
    if (sign == '(+)') {
        ctxtTaxTotAmt.SetValue(Math.round(totAmt + finalTaxAmt - taxAmountGlobal));
    } else {
        ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
    }


    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
    //Set Running Total
    SetRunningTotal();

    RecalCulateTaxTotalAmountInline();
}

function cmbGstCstVatChange(s, e) {

    SetOtherTaxValueOnRespectiveRow(0, 0, gstcstvatGlobalName);
    $('.RecalculateInline').hide();
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    if (s.GetValue().split('~')[2] == 'G') {
        ProdAmt = parseFloat(clblTaxProdGrossAmt.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'N') {
        ProdAmt = parseFloat(clblProdNetAmt.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'O') {
        //Check for Other Dependecy
        $('.RecalculateInline').show();
        ProdAmt = 0;
        var taxdependentName = s.GetValue().split('~')[3];
        for (var i = 0; i < taxJson.length; i++) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            var gridTaxName = cgridTax.GetEditor("Taxes_Name").GetText();
            gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
            if (gridTaxName == taxdependentName) {
                ProdAmt = cgridTax.GetEditor("Amount").GetValue();
            }
        }
    }
    else if (s.GetValue().split('~')[2] == 'R') {
        ProdAmt = GetTotalRunningAmount();
        $('.RecalculateInline').show();
    }

    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());

    var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVat.GetValue().split('~')[1]) / 100;
    ctxtGstCstVat.SetValue(calculatedValue);

    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    ctxtTaxTotAmt.SetValue(Math.round(totAmt + calculatedValue - GlobalCurTaxAmt));

    //tax others
    SetOtherTaxValueOnRespectiveRow(0, calculatedValue, ccmbGstCstVat.GetText());
    gstcstvatGlobalName = ccmbGstCstVat.GetText();
}


//for tax and charges

function ChargecmbGstCstVatChange(s, e) {

    SetOtherChargeTaxValueOnRespectiveRow(0, 0, ChargegstcstvatGlobalName);
    $('.RecalculateCharge').hide();
    var ProdAmt = parseFloat(ctxtProductAmount.GetValue());

    //Set ProductAmount
    if (s.GetValue().split('~')[2] == 'G') {
        ProdAmt = parseFloat(ctxtProductAmount.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'N') {
        ProdAmt = parseFloat(clblProdNetAmt.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'O') {
        //Check for Other Dependecy
        $('.RecalculateCharge').show();
        ProdAmt = 0;
        var taxdependentName = s.GetValue().split('~')[3];
        for (var i = 0; i < taxJson.length; i++) {
            gridTax.batchEditApi.StartEdit(i, 3);
            var gridTaxName = gridTax.GetEditor("TaxName").GetText();
            gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
            if (gridTaxName == taxdependentName) {
                ProdAmt = gridTax.GetEditor("Amount").GetValue();
            }
        }
    }
    else if (s.GetValue().split('~')[2] == 'R') {
        $('.RecalculateCharge').show();
        ProdAmt = GetChargesTotalRunningAmount();
    }


    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());

    var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVatcharge.GetValue().split('~')[1]) / 100;
    ctxtGstCstVatCharge.SetValue(calculatedValue);

    var totAmt = parseFloat(ctxtQuoteTaxTotalAmt.GetText());
    ctxtQuoteTaxTotalAmt.SetValue(totAmt + calculatedValue - GlobalCurChargeTaxAmt);

    //tax others
    SetOtherChargeTaxValueOnRespectiveRow(0, calculatedValue, ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = ctxtGstCstVatCharge.GetText();

    //set Total Amount
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}




function GetChargesTotalRunningAmount() {
    var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.batchEditApi.EndEdit();
    }

    return runningTot;
}

function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}




function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}
function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
}
function cmbtaxCodeindexChange(s, e) {
    if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {

        var taxValue = s.GetValue();

        if (taxValue == null) {
            taxValue = 0;
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.GetEditor("Amount").SetValue(0);
            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
        }


        var isValid = taxValue.indexOf('~');
        if (isValid != -1) {
            var rate = parseFloat(taxValue.split('~')[1]);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
            GlobalCurTaxAmt = 0;
        }
        else {
            s.SetText("");
        }

    } else {
        var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

        if (s.GetValue() == null) {
            s.SetValue(0);
        }

        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {

            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
            GlobalCurTaxAmt = 0;
        } else {
            s.SetText("");
        }
    }

}

function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
    for (var i = 0; i < taxJson.length; i++) {
        if (taxJson[i].applicableBy == name) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            cgridTax.GetEditor('calCulatedOn').SetValue(amt);

            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var s = cgridTax.GetEditor("TaxField");
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }




        }
    }
    //return;
    cgridTax.batchEditApi.EndEdit();

}



function SetOtherChargeTaxValueOnRespectiveRow(idx, amt, name) {
    name = name.substring(0, name.length - 3).trim();
    for (var i = 0; i < chargejsonTax.length; i++) {
        if (chargejsonTax[i].applicableBy == name) {
            gridTax.batchEditApi.StartEdit(i, 3);
            gridTax.GetEditor('calCulatedOn').SetValue(amt);

            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
            var s = gridTax.GetEditor("Percentage");
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }




        }
    }
    //return;
    gridTax.batchEditApi.EndEdit();
}



function txtPercentageLostFocus(s, e) {

    //var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
    if (s.GetText().trim() != '') {

        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
            //Checking Add or less
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                GlobalCurTaxAmt = 0;
            }
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

            //Call for Running Total
            SetRunningTotal();

        } else {
            s.SetText("");
        }
    }

    RecalCulateTaxTotalAmountInline();
}

function RecalCulateTaxTotalAmountInline() {
    var totalInlineTaxAmount = 0;
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        } else {
            totalInlineTaxAmount = totalInlineTaxAmount - parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }

        cgridTax.batchEditApi.EndEdit();
    }

    totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(ctxtGstCstVat.GetValue());

    ctxtTaxTotAmt.SetValue(Math.round(totalInlineTaxAmount));
}


function SetRunningTotal() {
    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        if (taxJson[i].applicableOn == "R") {
            cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var thisRunningAmt = 0;
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
        }
        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        cgridTax.batchEditApi.EndEdit();
    }
}

function GetTotalRunningAmount() {
    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        cgridTax.batchEditApi.EndEdit();
    }

    return runningTot;
}




function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}


function txtTax_TextChanged(s, i, e) {
    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
}



function BatchUpdate() {

    //cgridTax.batchEditApi.StartEdit(0, 1);

    //if (cgridTax.GetEditor("TaxField").GetText().indexOf('.') == -1) {
    //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '.00');
    //} else {
    //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '0');
    //}
    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        cgridTax.UpdateEdit();
    }
    else {
        cgridTax.PerformCallback('SaveGST');
    }
    return false;
}


function cgridTax_EndCallBack(s, e) {
    //cgridTax.batchEditApi.StartEdit(0, 1);
    $('.cgridTaxClass').show();

    cgridTax.StartEditRow(0);


    //check Json data
    if (cgridTax.cpJsonData) {
        if (cgridTax.cpJsonData != "") {
            taxJson = JSON.parse(cgridTax.cpJsonData);
            cgridTax.cpJsonData = null;
        }
    }
    //End Here

    if (cgridTax.cpComboCode) {
        if (cgridTax.cpComboCode != "") {
            if (cddl_AmountAre.GetValue() == "1") {
                var selectedIndex;
                for (var i = 0; i < ccmbGstCstVat.GetItemCount() ; i++) {
                    if (ccmbGstCstVat.GetItem(i).value.split('~')[0] == cgridTax.cpComboCode) {
                        selectedIndex = i;
                    }
                }
                if (selectedIndex && ccmbGstCstVat.GetItem(selectedIndex) != null) {
                    ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
                }
                cmbGstCstVatChange(ccmbGstCstVat);
                cgridTax.cpComboCode = null;
            }
        }
    }

    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
        ctxtTaxTotAmt.SetValue(Math.round(cgridTax.cpUpdated.split('~')[1]));
        var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
        var ddValue = parseFloat(ctxtGstCstVat.GetValue());
        ctxtTaxTotAmt.SetValue(Math.round(gridValue + ddValue));
        cgridTax.cpUpdated = "";
    }

    else {
        var totAmt = ctxtTaxTotAmt.GetValue();
        cgridTax.CancelEdit();
        caspxTaxpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 13);
        grid.GetEditor("gvColTaxAmount").SetValue(totAmt);
        grid.GetEditor("gvColTotalAmountINR").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue()));

    }

    if (cgridTax.GetVisibleRowsOnPage() == 0) {
        $('.cgridTaxClass').hide();
        ccmbGstCstVat.Focus();
    }
    //Debjyoti Check where any Gst Present or not
    // If Not then hide the hole section

    SetRunningTotal();
    ShowTaxPopUp("IY");
}

function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}




/*............................End Tax...........................................*/




function PerformCallToGridBind() {

    grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
    cQuotationComponentPanel.PerformCallback('BindQuotationGridOnSelection');
    $('#hdnPageStatus').val('Quoteupdate');
    cProductsPopup.Hide();
    return false;
}

function componentEndCallBack(s, e) {
    if (cQuotationComponentPanel.cpNullGrid != null) {
        deleteAllRows();
        if (grid.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();
        }
          
        grid.GetEditor('ProductName').SetEnabled(true);
        cPLQADate.SetText('');
    }
    else {
        gridquotationLookup.gridView.Refresh();
        if (grid.GetVisibleRowsOnPage() == 0) {
               
            OnAddNewClick();
            grid.GetEditor('ProductName').SetEnabled(true);
            cPLQADate.SetText('');
        }
    }

}
function CloseGridQuotationLookup() {
    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();
    gridquotationLookup.Focus();
}

function QuotationNumberChanged() {

    var quote_Id = gridquotationLookup.GetValue();
    if (quote_Id != null) {
        var arr = quote_Id.split(',');
        if (arr.length > 1) {
            cPLQADate.SetText('Multiple Select Indent Dates');
        }
        else {
            if (arr.length == 1) {
                cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);
            }
            else {
                cPLQADate.SetText('');
            }
        }
    }
    else { cPLQADate.SetText(''); }

    if (quote_Id != null) {
        cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
        cProductsPopup.Show();
    }
    else {
        cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
        cProductsPopup.Show();
    }
    //txt_OANumber.Focus();           
}
function SetDifference1() {
    var diff = CheckDifferenceOfFromDateWithTodate();
}
function CheckDifferenceOfFromDateWithTodate() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLSalesOrderDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (startTime - endTime) / 86400000;

    }
    return difference;

}
function SetDifference() {
    var diff = CheckDifference();
}
function CheckDifference() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLSalesOrderDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;

    }
    return difference;

}
//.................WareHouse.......


//...............end..........................
//...............Addeess Part.......

//function ClosebillingLookup() {
//    billingLookup.ConfirmCurrentSelection();
//    billingLookup.HideDropDown();
//    billingLookup.Focus();
//}
//function CloseshippingLookup() {
//    shippingLookup.ConfirmCurrentSelection();
//    shippingLookup.HideDropDown();
//    shippingLookup.Focus();
//}

//.......end........
//function GetVisibleIndex(s, e) {
//    globalRowIndex = e.visibleIndex;
//}
function BtnVisible() {
    document.getElementById('btnSaveExit').style.display = 'none'
    document.getElementById('btnnew').style.display = 'none'
    document.getElementById('tagged').style.display = 'block'

}

function OnEndCallback(s, e) {
    if (grid.cpBtnVisible != null && grid.cpBtnVisible != "") {
        grid.cpBtnVisible = null;
        BtnVisible();
    }
    var value = document.getElementById('hdnRefreshType').value;
    var pageStatus = document.getElementById('hdnPageStatus').value;


    if (grid.cpSaveSuccessOrFail == "outrange") {
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Can Not Add More Purchase Order Number as Purchase Order Scheme Exausted.<br />Update The Scheme and Try Again');
        //OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "duplicate") {
        grid.batchEditApi.StartEdit(0, 2);
        OnAddNewClick();
        jAlert('Can Not Save as Duplicate Quotation Numbe No. Found');
        //if (grid.GetVisibleRowsOnPage() == 0) {
        //    OnAddNewClick();
        //}
    }
    else if (grid.cpSaveSuccessOrFail == "UdfMandetory") {
        grid.batchEditApi.StartEdit(0, 2);
        OnAddNewClick();
        jAlert('UDF is set as Mandatory.Please enter values.', 'Alert', function () { OpenUdf(); });

    }
    else if (grid.cpSaveSuccessOrFail == "errorInsert") {
        grid.batchEditApi.StartEdit(0, 2);
        OnAddNewClick();
        jAlert('Please try after sometime.');
        //if (grid.GetVisibleRowsOnPage() == 0) {
        //    OnAddNewClick();
        //}
    } else if (grid.cpSaveSuccessOrFail == "Ponotexist") {
        grid.batchEditApi.StartEdit(0, 2);
        OnAddNewClick();
        jAlert('Cannot Save. Selected Purchase Indent(s) in this document do not exist.');
    }
    else {

        var PurchaseOrder_Number = grid.cpPurchaseOrderNo;
        var Order_Msg = "Purchase Order No. " + PurchaseOrder_Number + " saved.";
        if (value == "E") {
            if (grid.cpApproverStatus == "approve") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else if (grid.cpApproverStatus == "rejected") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            // window.location.assign("PurchaseOrderList.aspx");
            if (PurchaseOrder_Number != "") {
                jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {
                    if (r == true) {
                        grid.cpSalesOrderNo = null;
                        window.location.assign("PurchaseOrderList.aspx");
                    }
                });

            }
            else {
                window.location.assign("PurchaseOrderList.aspx");
            }
        }
        else if (value == "N") {
            if (grid.cpApproverStatus == "approve") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else if (grid.cpApproverStatus == "rejected") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            if (PurchaseOrder_Number != "") {
                jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {

                    grid.cpSalesOrderNo = null;
                    if (r == true) {
                        window.location.assign("PurchaseOrder.aspx?key=ADD");
                    }
                });


            }
            else {
                window.location.assign("PurchaseOrder.aspx?key=ADD");

            }
        }
        else {
            if (pageStatus == "first") {
                if (grid.GetVisibleRowsOnPage() == 0) {
                    OnAddNewClick();
                }
                grid.batchEditApi.EndEdit();
                // document.getElementByIdfocus();
                //   FinYearCheckOnPageLoad();
                $('#hdnPageStatus').val('');
                GetIndentReqNoOnLoad();
            }
          
            else if (pageStatus == "Quoteupdate") {
                //OnAddGridNewClick();
                grid.StartEditRow(0);
                $('#hdnPageStatus').val('');
                // GetIndentReqNoOnLoad();
            }
        else if (pageStatus == "delete") {
            // grid.StartEditRow(0);
            OnAddNewClick();
            $('#hdnPageStatus').val('');
        }
        }
    }

    if (gridquotationLookup.GetValue() != null) {
        grid.GetEditor('ProductName').SetEnabled(false);
        grid.GetEditor('gvColDiscription').SetEnabled(false);
        grid.StartEditRow(0);
        $('#hdnPageStatus').val('');
    }
    else {
        grid.GetEditor('ProductName').SetEnabled(true);
        grid.GetEditor('gvColDiscription').SetEnabled(true);
        if (grid.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();
        }
    }
    ////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
    ////This method is called when request is for View Only .
    if (grid.cpView == "1") {
        viewOnly();
    }
    cProductsPopup.Hide();
}

////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
////This method is for disable all the attributes.
function viewOnly() {
    $('.form_main').find('input, textarea, button, select').attr('disabled', 'disabled');
    
    grid.SetEnabled(false);
    cgridTax.SetEnabled(false);

    cGrdWarehousePC.SetEnabled(false);
    cgridproducts.SetEnabled(false);
    gridLookup.SetEnabled(false);
    cQuotationComponentPanel.SetEnabled(false);

    ctxt_Refference.SetEnabled(false);
    cComponentDatePanel.SetEnabled(false);
    cContactPerson.SetEnabled(false);
    cPLQuoteDate.SetEnabled(false);

   
    cButton2.SetVisible(false);
    cButton3.SetVisible(false);
    cbtn_SaveTax.SetVisible(false);
    cbtn_tax_cancel.SetVisible(false);
    cbtnWarehouse.SetVisible(false);
    cbtn_SaveRecords.SetVisible(false);
    cbtn_SaveRecordExits.SetVisible(false);
    cbtn_SaveUdf.SetVisible(false);
    cbtn_SaveRecordTaxs.SetVisible(false);
    cbtnSave_citys.SetVisible(false);
}

function GridCallBack() {
    //page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
    grid.PerformCallback('Display');
}
function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();
        if (gridquotationLookup.GetValue() != null) {
            //jAlert();
            jAlert('Cannot Delete using this button as the Purchase Indent is linked with this Purchase Order.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) {

            });
        }
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        //if (noofvisiblerows != "1" && lookup_quotation.GetValue() == null) {
        if (noofvisiblerows != "1" && gridquotationLookup.GetValue() == null) {
            grid.DeleteRow(e.visibleIndex);

            $('#hdfIsDelete').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display');
            $('#hdnPageStatus').val('delete');
            // grid.batchEditApi.StartEdit(-1, 2);
            // grid.batchEditApi.StartEdit(0, 2);
        }
    }
    if (e.buttonID == 'CustomAddNewRow') {
      
        if (gridquotationLookup.GetValue() == null) {
            grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            var Product = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";

            if (Product != "") {
               
                OnAddNewClick();


                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }, 500);
                return false;
            }
            else {
                // grid.batchEditApi.StartEdit(e.visibleIndex, 3);
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }, 50);
                return false;
                //grid.batchEditApi.StartEdit(globalRowIndex, 3);
            }
        }
        else {
            QuotationNumberChanged();
        }
        //var key = s.GetRowKey(e.visibleIndex);
        //if (key == 9) {
        //    cbtn_SaveRecords.Focus();
        //}



    }
    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";
        var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
        if (QuantityValue == "0.0") {
            jAlert("Quantity should not be zero !.");
        } else {

            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();
        
            if (ProductID != "") {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strMultiplier = SpliteDetails[7];
                var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "";
                var StkQuantityValue = QuantityValue * strMultiplier;
              
                $('#hdfProductIDPC').val(strProductID);
                $('#hdfProductType').val("");
                $('#hdfProductSerialID').val(SrlNo);
                $('#hdfProductSerialID').val(SrlNo);
                $('#hdnProductQuantity').val(QuantityValue);
                var Ptype = "";

                $('#hdnisserial').val("");
                $('#hdnisbatch').val("");
                $('#hdniswarehouse').val("");
                document.getElementById('lblAvailableStkunit').innerHTML = strUOM;
                document.getElementById('lblopeningstockUnit').innerHTML = strUOM;
                $.ajax({
                    type: "POST",
                    url: 'PurchaseOrder.aspx/getProductType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{Products_ID:\"" + strProductID + "\"}",
                    success: function (type) {
                        Ptype = type.d;
                        $('#hdfProductType').val(Ptype);
                        //alert(Ptype);
                        if (Ptype == "") {
                            jAlert("No Warehouse or Batch or Serial is actived !.");
                        } else {
                            if (Ptype == "W") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("true");
                                //cCmbWarehouse.PerformCallback('BindWarehouse');

                            }

                            else if (Ptype == "B") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");

                            }
                            else if (Ptype == "S") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");

                            }
                            else if (Ptype == "WB") {

                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("true");
                                //cCmbWarehouse.PerformCallback('BindWarehouse');
                                //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                            }
                            else if (Ptype == "WS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("true");
                                //cCmbWarehouse.PerformCallback('BindWarehouse');
                                //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                            }
                            else if (Ptype == "WBS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("true");
                                //cCmbWarehouse.PerformCallback('BindWarehouse');
                                //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                            }
                            else if (Ptype == "BS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("false");

                         
                            }
                            else {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");
                            }

                            $("#RequiredFieldValidatortxtbatch").css("display", "none");
                            $("#RequiredFieldValidatortxtserial").css("display", "none");
                            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");

                            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");

                            $(".blockone").css("display", "none");
                            $(".blocktwo").css("display", "none");
                            $(".blockthree").css("display", "none");

                            ctxtqnty.SetText("0.0");
                            ctxtqnty.SetEnabled(true);

                            ctxtbatchqnty.SetText("0.0");
                            ctxtserial.SetText("");
                            ctxtbatchqnty.SetText("");

                            ctxtbatch.SetEnabled(true);
                            cCmbWarehouse.SetEnabled(true);

                            $('#hdnoutstock').val("0");
                            $('#hdnisedited').val("false");
                            $('#hdnisoldupdate').val("false");
                            $('#hdnisnewupdate').val("false");

                            $('#hdnisolddeleted').val("false");

                            $('#hdntotalqntyPC').val(0);
                            $('#hdnoldrowcount').val(0);
                            $('#hdndeleteqnity').val(0);
                            $('#hidencountforserial').val("1");

                            $('#hdfstockidPC').val(0);
                            $('#hdfopeningstockPC').val(0);
                            $('#oldopeningqntity').val(0);
                            $('#hdnnewenterqntity').val(0);

                            $('#hdnenterdopenqnty').val(0);
                            $('#hdbranchIDPC').val(0);

                            $('#hdnisviewqntityhas').val("false");


                            $('#hdndefaultID').val("");
                            $('#hdnbatchchanged').val("0");
                            $('#hdnrate').val("0");
                            $('#hdnvalue').val("0");
                            $('#hdnstrUOM').val(strUOM);
                        
                            var branchid = $("#ddl_Branch option:selected").val();

                            $('#hdnisreduing').val("false");

                            var productid = strProductID ? strProductID : "";
                            var StkQuantityValue = QuantityValue ? QuantityValue : "0.0000";

                            var stockids = SpliteDetails[10];
                            var checkstockdecimalornot = StkQuantityValue.toString().split(".")[1]

                            $('#hdnpcslno').val(SrlNo);
                          
                            // var ProductName = SpliteDetails[12];
                            var ProductName = SpliteDetails[1];
                            var ratevalue = "0";
                            var rate = "0";
                            
                            var branchid = $('#ddl_Branch').val();
                            

                            var BranchNames = $("#ddl_Branch option:selected").text();
                          
                            var strProductID = productid;
                            var strDescription = "";
                            var strUOM = (strUOM != null) ? strUOM : "0";
                            var strProductName = ProductName;

                            document.getElementById('lblbranchName').innerHTML = BranchNames;
                            var availablestock = SpliteDetails[12];
                            $('#hdndefaultID').val("0");

                            $('#hdfstockidPC').val(stockids);
                            var calculateopein = Number(StkQuantityValue) - Number(availablestock);
                            var oldopeing = 0;
                            var oldqnt = Number(oldopeing);

                            $('#hdfopeningstockPC').val(QuantityValue);
                            $('#oldopeningqntity').val(0);
                            $('#hdnnewenterqntity').val(QuantityValue);
                            $('#hdnenterdopenqnty').val(0);
                            $('#hdbranchIDPC').val(branchid);
                            $('#hdnselectedbranch').val(branchid);

                            $('#hdnrate').val(rate);
                            $('#hdnvalue').val(ratevalue);

                            var dtd = (Number(StkQuantityValue)).toFixed(4);

                         
                            $("#lblopeningstock").text(dtd);
                            ctxtmkgdate.SetDate = null;
                            txtexpirdate.SetDate = null;
                            ctxtserial.SetValue("");
                            ctxtbatch.SetValue("");
                            ctxtqnty.SetValue("0.0");
                            ctxtbatchqnty.SetValue("0.0");

                            var hv = $('#hdnselectedbranch').val();

                            var iswarehousactive = $('#hdniswarehouse').val();
                            var isactivebatch = $('#hdnisbatch').val();
                            var isactiveserial = $('#hdnisserial').val();
                        

                            cCmbWarehouse.PerformCallback('BindWarehouse');

                            if (iswarehousactive == "true") {

                                cCmbWarehouse.SetVisible(true);
                                cCmbWarehouse.SetSelectedIndex(1);
                                cCmbWarehouse.Focus();
                                ctxtqnty.SetVisible(true);
                                $('#hdniswarehouse').val("true");

                                $(".blockone").css("display", "block");

                            } else {
                                cCmbWarehouse.SetVisible(false);
                                ctxtqnty.SetVisible(false);
                                $('#hdniswarehouse').val("false");
                                cCmbWarehouse.SetSelectedIndex(-1);
                                $(".blockone").css("display", "none");

                            }

                            if (isactivebatch == "true") {

                                ctxtbatch.SetVisible(true);
                                ctxtmkgdate.SetVisible(true);
                                ctxtexpirdate.SetVisible(true);
                                $('#hdnisbatch').val("true");

                                $(".blocktwo").css("display", "block");

                            } else {
                                ctxtbatch.SetVisible(false);
                                ctxtmkgdate.SetVisible(false);
                                ctxtexpirdate.SetVisible(false);
                                $('#hdnisbatch').val("false");

                                $(".blocktwo").css("display", "none");

                            }
                            if (isactiveserial == "true") {
                                ctxtserial.SetVisible(true);
                                $('#hdnisserial').val("true");


                                $(".blockthree").css("display", "block");
                            } else {
                                ctxtserial.SetVisible(false);
                                $('#hdnisserial').val("false");


                                $(".blockthree").css("display", "none");
                            }

                            if (iswarehousactive == "false" && isactivebatch == "true") {
                                ctxtbatchqnty.SetVisible(true);

                                $(".blocktwoqntity").css("display", "block");
                            } else {
                                ctxtbatchqnty.SetVisible(false);
                                $(".blocktwoqntity").css("display", "none");
                            }

                            if (iswarehousactive == "false" && isactivebatch == "true") {
                                ctxtbatch.Focus();
                            } else {
                                cCmbWarehouse.Focus();
                            }

                            cbtnWarehouse.SetVisible(true);
                            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

                            cPopup_WarehousePC.Show();
                        }
                    }
                });
            }





        }
    }


}
function Save_ButtonClick() {
    flag = true;
    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();
    var Podt = cPLQuoteDate.GetValue();

    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        //flag = false;
        $("#MandatoryBillNo").show();
        return false;
    }
    if (Podt == null) {
        $("#MandatoryDate").show();
        return false;
    }
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {

        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }

    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }

    if (flag != false) {
        if (grid.GetVisibleRowsOnPage() > 0) {
            if (IsType == "Y") {
                var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                $('#hdfLookupCustomer').val(customerval);
                $('#hdfIsDelete').val('I');
                $('#hdnRefreshType').val('N');
                grid.batchEditApi.EndEdit();
                $('#hfControlData').val($('#hfControlSaveData').val());
                grid.UpdateEdit();
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }

        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
   
    }
function SaveExit_ButtonClick() {
    flag = true;
    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();
    // alert(txtPurchaseNo);
    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        flag = false;
        $("#MandatoryBillNo").show();
        return false;
    }
    var Podt = cPLQuoteDate.GetValue();
    if (Podt == null) {
        $("#MandatoryDate").show();
        return false;
    }
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {

        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }

    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (flag != false) {
        if (grid.GetVisibleRowsOnPage() > 0) {
            if (IsType == "Y") {
                var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                $('#hdfLookupCustomer').val(customerval);
                $('#hdnRefreshType').val('E');
                $('#hdfIsDelete').val('I');
                $('#hfControlData').val($('#hfControlSaveData').val());
                grid.batchEditApi.EndEdit();
                grid.UpdateEdit();
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }

        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}
function OnAddNewClick() {

    grid.AddNewRow();
    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i;
    var cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);


        cnt++;
    }
    //var tbQuotation = grid.GetEditor("SrlNo");
    //tbQuotation.SetValue(noofvisiblerows);

}
function ProductsCombo_SelectedIndexChanged(s, e) {

    var tbDescription = grid.GetEditor("gvColDiscription");
    var tbUOM = grid.GetEditor("gvColUOM");
    var tbStockUOM = grid.GetEditor("gvColStockUOM");
    var tbPurchasePrice = grid.GetEditor("gvColStockPurchasePrice");

    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
    // var ProductID = s.GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStockUOM = SpliteDetails[4];
    var strPurchasePrice = SpliteDetails[6];
    var strStockId = SpliteDetails[10];
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbStockUOM.SetValue(strStockUOM);
    tbPurchasePrice.SetValue(strPurchasePrice);
    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }

}

function ddl_AmountAre_valueChange() {
    var key = $("#ddl_AmountAre").val();
    if (key == 1) {
        // grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('1');
    }
    else if (key == 2) {
        // grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');

    }
    else if (key == 3) {
        //  grid.GetEditor('TaxAmount').SetEnabled(false);
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('3');

    }
}

function GetIndentREquiNo(e) {

    var PODate = new Date();
    PODate = cPLQuoteDate.GetValueString();
    cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate);

    grid.batchEditApi.StartEdit(-1, 1);
    var accountingDataMin = grid.GetEditor('ProductName').GetValue();
    grid.batchEditApi.EndEdit();
    console.log(accountingDataMin);

    grid.batchEditApi.StartEdit(0, 1);
    var accountingDataplus = grid.GetEditor('ProductName').GetValue();
    console.log(accountingDataplus);
    grid.batchEditApi.EndEdit();

    if (accountingDataMin != null || accountingDataplus != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

            if (r == true) {
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                grid.PerformCallback('GridBlank');
            }
        });
        //onBranchItems();
    }
}

function GetIndentReqNoOnLoad() {

    var PODate = new Date();
    PODate = cPLQuoteDate.GetValueString();
    cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate);

}
function GetContactPersonPhone(e) {
    var key = cContactPerson.GetValue();
    cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
}
function GetContactPerson(e) {
    //  GetIndentReqNoOnLoad();
    var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
    if (key != null && key != '') {
        cchkBilling.SetChecked(false);
        cchkShipping.SetChecked(false);
        cContactPerson.PerformCallback('BindContactPerson~' + key);
        page.GetTabByName('Billing/Shipping').SetEnabled(true);
        jConfirm('Wish to View/Enter Billing and Shipping details?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                page.SetActiveTabIndex(1);
                $('.dxeErrorCellSys').addClass('abc');
            }
            else {
                cContactPerson.Focus();
            }
        });
        GetObjectID('hdnCustomerId').value = key;

        //grid.batchEditApi.StartEdit(-1, 1);
        //var accountingDataMin = grid.GetEditor('ProductName').GetValue();
        //grid.batchEditApi.EndEdit();
               

        //grid.batchEditApi.StartEdit(0, 1);
        //var accountingDataplus = grid.GetEditor('ProductName').GetValue();               
        //grid.batchEditApi.EndEdit();

        //if (accountingDataMin != null || accountingDataplus != null) {
        //    jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

        //        if (r == true) {
        //            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        //            grid.PerformCallback('GridBlank');
        //        }
        //    });

        //}

    }

}
function ShowIndntRequisition()
{

}
function cmbContactPersonEndCall(s, e) {

    //if (cContactPerson.cpDueDate != null) {
    //    var DeuDate = cContactPerson.cpDueDate;
    //    var myDate = new Date(DeuDate);

    //    cdt_SaleInvoiceDue.SetDate(myDate);
    //    cContactPerson.cpDueDate = null;
    //}

    if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {

        pageheaderContent.style.display = "block";

        $("#divOutstanding").attr('style', 'display:block');
        document.getElementById('lblTotalPayable').innerHTML = cContactPerson.cpOutstanding;
        cContactPerson.cpOutstanding = null;
    }
    else {
        pageheaderContent.style.display = "none";

        $("#divOutstanding").attr('style', 'display:none');
        document.getElementById('lblTotalPayable').innerHTML = '';
    }

}
function acpContactPersonPhoneEndCall(s, e) {
    if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
        pageheaderContent.style.display = "block";
        $("#divContactPhone").attr('style', 'display:block');
        document.getElementById('lblContactPhone').innerHTML = cacpContactPersonPhone.cpPhone;
        cacpContactPersonPhone.cpPhone = null;

    }
}
$(document).ready(function () {
    var schemaid = $('#ddl_numberingScheme').val();
    // alert(schemaid);
    if (schemaid != null) {
        if (schemaid == '0') {
            document.getElementById('txtVoucherNo').disabled = true;
        }
    }
   
   $('#ApprovalCross').click(function () {

       window.parent.popup.Hide();
       window.parent.cgridPendingApproval.Refresh()();
   })
  
   });
function CmbScheme_ValueChange() {

    // GetIndentReqNoOnLoad();
    var val = $("#ddl_numberingScheme").val();

    $.ajax({
        type: "POST",
        url: 'PurchaseOrder.aspx/getSchemeType',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{sel_scheme_id:\"" + val + "\"}",
        success: function (type) {

            var schemetypeValue = type.d;
            var schemetype = schemetypeValue.toString().split('~')[0];
            var schemelength = schemetypeValue.toString().split('~')[1];
            $('#txtVoucherNo').attr('maxLength', schemelength);
            if (schemetype == '0') {

                document.getElementById('txtVoucherNo').disabled = false;
                document.getElementById('txtVoucherNo').value = "";
                $("#txtVoucherNo").focus();

            }
            else if (schemetype == '1') {

                document.getElementById('txtVoucherNo').disabled = true;
                document.getElementById('txtVoucherNo').value = "Auto";
                cPLQuoteDate.Focus();
                $("#MandatoryBillNo").hide();

            }
            else if (schemetype == '2') {

                document.getElementById('txtVoucherNo').disabled = true;
                document.getElementById('txtVoucherNo').value = "Datewise";
            }
            else if (schemetype == 'n') {
                document.getElementById('txtVoucherNo').disabled = true;
                document.getElementById('txtVoucherNo').value = "";
            }
        }
    });
}
function IndentRequisitionNo_ValueChange() {

    var val = $("#ddl_IndentRequisitionNo").val();
    if (val != 0) {
        $.ajax({
            type: "POST",
            url: 'PurchaseOrder.aspx/getIndentRequisitionDate',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{IndentRequisitionNo:\"" + val + "\"}",
            success: function (type) {

                var Transdt = new Date(type.d);
                cIndentRequisDate.SetDate(Transdt);

            }
        });
    }
    else {
        cIndentRequisDate.SetVal("");
    }

}


function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
}



function SetDifference() {
    var diff = CheckDifference();
    if (diff > 0) {
        clientResult.SetText(diff.toString());
    }

}

function CheckDifference() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLQuoteDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;

    }
    return difference;

}

 //<%--   Warehouse  Script   --%>
 

function Keypressevt() {

    if (event.keyCode == 13) {

        //run code for Ctrl+X -- ie, Save & Exit! 
        SaveWarehouse();
        return false;
    }
}
$(document).ready(function () {
             
    var isCtrl = false;
    document.onkeydown = function (e) {
        if (event.keyCode == 83 && event.altKey == true) {
            if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                SaveVehicleControlData();
            }
        }
        if (event.keyCode == 67 && event.altKey == true) {
            modalShowHide(0);
        }
        if (event.keyCode == 82 && event.altKey == true) {
            modalShowHide(1);
            $('body').on('shown.bs.modal', '#exampleModal', function () {
                $('input:visible:enabled:first', this).focus();
            })
        }
                 
        if (event.keyCode == 78 && event.altKey == true) {                     
            Save_ButtonClick();
        }
        else if (event.keyCode == 88 && event.altKey == true) {                     
            SaveExit_ButtonClick();                    
        }
        else if (event.keyCode == 85 && event.altKey == true) {
            OpenUdf();
        }
        else if (event.keyCode == 84 && event.altKey == true) {
            Save_TaxesClick();
        }
        if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
            //  StopDefaultAction(e);
            alert();

            btnSave_QuoteAddress();
            // document.getElementById('Button3').click();

            // return false;
        }

    }
});

function DeleteWarehousebatchserial(SrlNo, BatchWarehouseID, viewQuantity, Quantity, WarehouseID, BatchNo) {
    //alert(viewQuantity);
    var IsSerial = $('#hdnisserial').val();
    if (IsSerial == "true" && viewQuantity != "1.0000" && viewQuantity != "1.0" && viewQuantity != "") {
        jAlert("Cannot Proceed. You have to delete subsequent data first before delete this data.");
    } else {
        if (BatchWarehouseID == "" || BatchWarehouseID == "0") {

            $('#hdnisolddeleted').val("false");
            if (SrlNo != "") {


                cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
            }

        } else {

            $('#hdnisolddeleted').val("true");
            if (SrlNo != "") {

                cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
            }
        }
    }



}

function Setenterfocuse(s) {

}

function UpdateWarehousebatchserial(SrlNo, WarehouseID, BatchNo, SerialNo, isnew, viewQuantity, Quantity) {

    var Isbatch = $('#hdnisbatch').val();

    if (isnew == "old" || isnew == "Updated") {

        $('#hdnisoldupdate').val("true");
        $('#hdncurrentslno').val("");
        cCmbWarehouse.SetValue(WarehouseID);
        if (Quantity != null && Quantity != "" && Isbatch != "true") {
            ctxtqnty.SetText(Quantity);
        } else {
            ctxtqnty.SetText(viewQuantity);
        }
        var IsSerial = $('#hdnisserial').val();

        if (IsSerial == "true") {

            if (viewQuantity == "") {
                ctxtbatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
                ctxtqnty.SetEnabled(false);
                ctxtserial.Focus();
            } else {
                ctxtbatch.SetEnabled(true);
                cCmbWarehouse.SetEnabled(true);
                ctxtqnty.SetEnabled(true);
                ctxtserial.Focus();
            }

        }
        else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtbatch.Focus();
        }
        // ctxtqnty.SetEnabled(false);

        ctxtbatchqnty.SetText(Quantity);
        //ctxtbatchqnty.SetEnabled(false);
        ctxtbatch.SetText(BatchNo);
        ctxtserial.SetText(SerialNo);

        if (viewQuantity == "") {
            ctxtbatch.SetEnabled(false);
            cCmbWarehouse.SetEnabled(false);
            $('#hdnisviewqntityhas').val("true");
        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            $('#hdnisviewqntityhas').val("false");
        }

        var hdniswarehouse = $('#hdniswarehouse').val();


        if (hdniswarehouse != "true" && Isbatch == "true") {
            ctxtbatchqnty.SetText(viewQuantity);
            ctxtbatchqnty.Focus();

        } else {
            ctxtqnty.Focus();
        }
        $('#hdncurrentslno').val(SrlNo);

    } else {

        $('#hdnisoldupdate').val("false");

        ctxtqnty.SetText("0.0");
        ctxtqnty.SetEnabled(true);

        ctxtbatchqnty.SetText("0.0");
        ctxtserial.SetText("");
        ctxtbatchqnty.SetText("");
        $('#hdncurrentslno').val("");

        $('#hdnisnewupdate').val("true");
        $('#hdncurrentslno').val("");
        cCmbWarehouse.SetValue(WarehouseID);
        if (Quantity != null && Quantity != "" && Isbatch != "true") {
            ctxtqnty.SetText(Quantity);
        } else {
            ctxtqnty.SetText(viewQuantity);
        }
        var IsSerial = $('#hdnisserial').val();
        if (IsSerial == "true") {

            if (viewQuantity == "") {
                ctxtbatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
                ctxtqnty.SetEnabled(false);
                $('#hdnisviewqntityhas').val("true");
                ctxtserial.Focus();
            } else {
                ctxtbatch.SetEnabled(true);
                cCmbWarehouse.SetEnabled(true);
                ctxtqnty.SetEnabled(true);
                $('#hdnisviewqntityhas').val("false");
                ctxtserial.Focus();
            }

        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtbatch.Focus();
        }
        // ctxtqnty.SetEnabled(false);

        ctxtbatchqnty.SetText(Quantity);
        //ctxtbatchqnty.SetEnabled(false);
        ctxtbatch.SetText(BatchNo);
        ctxtserial.SetText(SerialNo);

        if (viewQuantity == "") {
            ctxtbatch.SetEnabled(false);
            cCmbWarehouse.SetEnabled(false);
        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
        }

        var hdniswarehouse = $('#hdniswarehouse').val();


        if (hdniswarehouse != "true" && Isbatch == "true") {
            ctxtbatchqnty.SetText(viewQuantity);
        } else {
            ctxtqnty.Focus();
        }

        $('#hdncurrentslno').val(SrlNo);

        //jAlert("Sorry, This is new entry you can not update. please click on 'Clear Entries' and Add again.");
    }
}

function changedqnty(s) {

    var qnty = s.GetText();
    var sum = $('#hdntotalqntyPC').val();

    sum = Number(Number(sum) + Number(qnty));
    //alert(sum);
    $('#hdntotalqntyPC').val(sum);
   
    }

function endcallcmware(s) {

    if (cCmbWarehouse.cpstock != null) {

        var ddd = cCmbWarehouse.cpstock + " " + $('#hdnstrUOM').val();
        document.getElementById('lblAvailableStk').innerHTML = ddd;
        cCmbWarehouse.cpstock = null;
    }
}
function changedqntybatch(s) {

    var qnty = s.GetText();
    var sum = $('#hdntotalqntyPC').val();
    sum = Number(Number(sum) + Number(qnty));
    //alert(sum);
    $('#hdntotalqntyPC').val(sum);

    //var Isbatch = $('#hdnisbatch').val();
    //var IsSerial = $('#hdnisserial').val();
    ////alert(Isbatch);
    //if (IsSerial == "true") {
    //    ctxtserial.Focus();
    //}

}
function chnagedbtach(s) {

    $('#hdnoldbatchno').val(s.GetText());
    $('#hidencountforserial').val(1);

    var sum = $('#hdnbatchchanged').val();
    sum = Number(Number(sum) + Number(1));

    $('#hdnbatchchanged').val(sum);
    //ctxtqnty.SetValue("0.0");
    //ctxtbatchqnty.SetValue("0.0");
    //ctxtmkgdate.SetDate = null;
    //txtexpirdate.SetDate = null;
    //ASPx.CalClearClick('txtmkgdate_DDD_C');
    //ASPx.CalClearClick('txtexpirdate_DDD_C');
    ctxtexpirdate.SetText("");
    ctxtmkgdate.SetText("");
}

function CmbWarehouse_ValueChange(s) {

    var ISupdate = $('#hdnisoldupdate').val();
    var isnewupdate = $('#hdnisnewupdate').val();

    $('#hdnoldwarehousname').val(s.GetText());

    if (ISupdate == "true" || isnewupdate == "true") {


    } else {
        

             ctxtserial.SetValue("");
        //ctxtbatch.SetValue("");
        //ctxtmkgdate.SetDate = null;
        //txtexpirdate.SetDate = null;

        ctxtbatch.SetEnabled(true);
        ctxtexpirdate.SetEnabled(true);
        ctxtmkgdate.SetEnabled(true);

        //ctxtqnty.SetValue("0.0");
        //ctxtbatchqnty.SetValue("0.0");

        //cCmbWarehouse.PerformCallback('Bindstock');
        //ASPx.CalClearClick('txtmkgdate_DDD_C');
        //ASPx.CalClearClick('txtexpirdate_DDD_C');
        //ctxtexpirdate.SetText("");
        //ctxtmkgdate.SetText("");
    }


}

function Clraear() {
    ctxtbatch.SetValue("");

    ASPx.CalClearClick('txtmkgdate_DDD_C');
    ASPx.CalClearClick('txtexpirdate_DDD_C');
    $('#hdnisoldupdate').val("false");
    //ctxtmkgdate.SetDate = null;
    //txtexpirdate.SetDate = null;

    //ctxtexpirdate.SetText("");
    //ctxtmkgdate.SetText("");

    //ctxtmkgdate.CalClearClick('txtmkgdate_DDD_C');
    //ctxtexpirdate.CalClearClick('txtexpirdate_DDD_C');
    ctxtserial.SetValue("");
    ctxtqnty.SetValue("0.0000");
    ctxtbatchqnty.SetValue("0.0000");
    $('#hdntotalqntyPC').val(0);
    $('#hidencountforserial').val(1);
    $('#hdnbatchchanged').val("0");
    var strProductID = $('#hdfProductIDPC').val();
    var stockids = $('#hdfstockidPC').val();
    var branchid = $('#hdbranchIDPC').val();
    var strProductName = $('#lblProductName').text();
    $('#hdnisnewupdate').val("false");
    ctxtbatch.SetEnabled(true);
    ctxtexpirdate.SetEnabled(true);
    ctxtmkgdate.SetEnabled(true);
    ctxtbatch.SetEnabled(true);
    cCmbWarehouse.SetEnabled(true);
    $('#hdnisviewqntityhas').val("false");
    $('#hdnisolddeleted').val("false");
    ctxtqnty.SetEnabled(true);

    var existingqntity = $('#hdfopeningstockPC').val();
    var totaldeleteqnt = $('#hdndeleteqnity').val();

    var addqntity = Number(existingqntity) + Number(totaldeleteqnt);

    $('#hdndeleteqnity').val(0);
   



      cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

}

function SaveWarehouse() {


    //alert(ISupdate);
   

    //}
    //else {

    var WarehouseID = cCmbWarehouse.GetValue();
        var WarehouseName = cCmbWarehouse.GetText();

        var qnty = ctxtqnty.GetText();
        var IsSerial = $('#hdnisserial').val();
        //alert(qnty);

        if (qnty == "0.0000") {
            qnty = ctxtbatchqnty.GetText();
        }

        if (Number(qnty) % 1 != 0 && IsSerial == "true") {
            jAlert("Serial number is activated, Quantity should not contain decimals. ");
            return;
        }

        //alert(qnty);
        var BatchName = ctxtbatch.GetText();
        var SerialName = ctxtserial.GetText();
        var Isbatch = $('#hdnisbatch').val();

        var enterdqntity = $('#hdfopeningstockPC').val();

        var hdniswarehouse = $('#hdniswarehouse').val();

        var ISupdate = $('#hdnisoldupdate').val();
        var isnewupdate = $('#hdnisnewupdate').val();
        //alert(Isbatch + "/" + IsSerial);
        //alert(hdniswarehouse+"/"+WarehouseID);
        if (Isbatch == "true" && hdniswarehouse == "false") {
            qnty = ctxtbatchqnty.GetText();
        }

        if (ISupdate == "true") {

            if (hdniswarehouse == "true" && WarehouseID == null) {

                $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
            }
            else {
                $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
            }
            if (qnty == "0.0") {

                if (Isbatch != "false" || hdniswarehouse != "false") {
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                    //jAlert("Quantity should not be 0.0");
                } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                    qnty = "0.00"
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                }
            } else {

                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }

            if (Isbatch == "true" && BatchName == "") {

                $("#RequiredFieldValidatortxtbatch").css("display", "block");
                ctxtbatch.Focus();
            } else {
                $("#RequiredFieldValidatortxtbatch").css("display", "none");
            }
            if (IsSerial == "true" && SerialName == "") {
                $("#RequiredFieldValidatortxtserial").css("display", "block");
                ctxtserial.Focus();

            } else {
                $("#RequiredFieldValidatortxtserial").css("display", "none");
            }
            var slno = $('#hdncurrentslno').val();



            if (slno != "") {

                cGrdWarehousePC.PerformCallback('Updatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty);

              
                return false;
            }


        } else if (isnewupdate == "true") {
            if (hdniswarehouse == "true" && WarehouseID == null) {

                $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
            }
            else {
                $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
            }
            if (qnty == "0.0") {

                if (Isbatch != "false" || hdniswarehouse != "false") {
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                    //jAlert("Quantity should not be 0.0");
                } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                    qnty = "0.00"
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                }
            } else {

                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }

            if (Isbatch == "true" && BatchName == "") {

                $("#RequiredFieldValidatortxtbatch").css("display", "block");
                ctxtbatch.Focus();
            }
            else {
                $("#RequiredFieldValidatortxtbatch").css("display", "none");
            }
            if (IsSerial == "true" && SerialName == "") {


                $("#RequiredFieldValidatortxtserial").css("display", "block");
                ctxtserial.Focus();

            } else {
                $("#RequiredFieldValidatortxtserial").css("display", "none");
            }
            var slno = $('#hdncurrentslno').val();

            if (slno != "") {

                cGrdWarehousePC.PerformCallback('NewUpdatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty);

                $('#hdnisviewqntityhas').val("false");
                $('#hdnisnewupdate').val("false");
                ctxtbatch.SetEnabled(true);
                cCmbWarehouse.SetEnabled(true);
                ctxtqnty.SetEnabled(true);
                ctxtqnty.SetText("0.0");
                ctxtbatch.SetText("");
                return false;
            }

        }
        else {

            var hdnisediteds = $('#hdnisedited').val();

            if (hdniswarehouse == "true" && WarehouseID == null) {

                $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");

                return;
            } else {
                $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
            }
            if (qnty == "0.0") {

                if (Isbatch != "false" || hdniswarehouse != "false") {
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                    //jAlert("Quantity should not be 0.0");
                } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                    qnty = "0.00"
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                }
            } else {

                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
            if (Isbatch == "true" && BatchName == "") {

                $("#RequiredFieldValidatortxtbatch").css("display", "block");
                ctxtbatch.Focus();
                return;

            } else {
                $("#RequiredFieldValidatortxtbatch").css("display", "none");
            }
            if (IsSerial == "true" && SerialName == "") {


                $("#RequiredFieldValidatortxtserial").css("display", "block");
                ctxtserial.Focus();
                return;

            } else {
                $("#RequiredFieldValidatortxtserial").css("display", "none");
            }
            if (Isbatch == "true" && hdniswarehouse == "false") {

                qnty = ctxtbatchqnty.GetText();

                if (qnty == "0.0000") {
                    //alert("Enter" + ctxtbatchqnty.GetText());

                    ctxtbatchqnty.Focus();
                }
            }

            if (qnty == "0.0") {

                if (Isbatch != "false" || hdniswarehouse != "false") {
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                    //jAlert("Quantity should not be 0.0");
                } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                    qnty = "0.00"
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                }
            }
            else if (((hdniswarehouse == "true" && WarehouseID != null) || hdniswarehouse == "false") && ((Isbatch == "true" && BatchName != "") || Isbatch == "false") && ((IsSerial == "true" && SerialName != "") || IsSerial == "false") && qnty != "0.0") {
                $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
                $("#RequiredFieldValidatortxtbatch").css("display", "none");
                $("#RequiredFieldValidatortxtserial").css("display", "none");

                $("#RequiredFieldValidatortxtwareqntity").removeAttr("style");
                $("#RequiredFieldValidatortxtbatchqntity").removeAttr("style");
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");


                if (Isbatch == "true" && hdniswarehouse == "false") {

                    qnty = ctxtbatchqnty.GetText();

                    if (qnty = "0.0000") {
                        ctxtbatchqnty.Focus();
                    }
                }


                var oldenterqntity = $('#hdnenterdopenqnty').val();
                var enterdqntityss = $('#hdnnewenterqntity').val();
                var deletedquantity = $('#hdndeleteqnity').val();
                //alert(deletedquantity);
                // alert(enterdqntityss + "|" + oldenterqntity);
                //if (Number(enterdqntityss) < Number(oldenterqntity)) {
                //    qnty = "0.00";
                //    jConfirm('You have entered Quantity less than Opening Quantity. Do you want to clear all existing entries.?', 'Confirmation Dialog', function (r) {
                //        if (r == true) {

                //            cGrdWarehousePC.PerformCallback('Delete~' + WarehouseID);
                //            var strProductID = $('#hdfProductIDPC').val();
                //            var stockids = $('#hdfstockidPC').val();
                //            var branchid = $('#hdbranchIDPC').val();
                //            var strProductName = $('#lblProductName').text();
                //            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
                //        }
                //    });


                //}




                if (Number(qnty) > (Number(enterdqntity) + Number(deletedquantity)) && hdnisediteds == "false") {
                    qnty = "0.00";
                    jAlert("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");


                }
                else {


                    cGrdWarehousePC.PerformCallback('Display~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + qnty);

                    //ctxtserial.SetValue("");

                    //ASPx.CalClearClick('txtmkgdate_DDD_C');
                    //ASPx.CalClearClick('txtexpirdate_DDD_C');

                    cCmbWarehouse.Focus();
                }
            }
            //}

            //ctxtexpirdate.SetText("");
            //ctxtmkgdate.SetText("");
            return false;
        }
    }
    function SaveWarehouseAll() {
        //var openqnty = Number($('#hdfopeningstockPC').val());
        //var totalqnty = Number($('#hdntotalqntyPC').val());
        // if (totalqnty != openqnty) {

        //jAlert("Please make sure Opening quantity should not be greater and less than total INput quantity.");
        //} else {

        cGrdWarehousePC.PerformCallback('Saveall~');


        //}

    }

    function cGrdWarehousePCShowError(obj) {

        if (cGrdWarehousePC.cpdeletedata != null) {
            var existingqntity = $('#hdfopeningstockPC').val();
            var totaldeleteqnt = $('#hdndeleteqnity').val();

            var addqntity = Number(cGrdWarehousePC.cpdeletedata) + Number(existingqntity);
            var adddeleteqnty = Number(cGrdWarehousePC.cpdeletedata) + Number(totaldeleteqnt);

            $('#hdndeleteqnity').val(adddeleteqnty);
          
            cGrdWarehousePC.cpdeletedata = null;
        }

        if (cGrdWarehousePC.cpdeletedatasubsequent != null) {
            jAlert(cGrdWarehousePC.cpdeletedatasubsequent);
            cGrdWarehousePC.cpdeletedatasubsequent = null;
        }
        if (cGrdWarehousePC.cpbatchinsertmssg != null) {
            ctxtbatch.SetText("");

            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            cGrdWarehousePC.cpbatchinsertmssg = null;
        }
        if (cGrdWarehousePC.cpupdateexistingdata != null) {

            $('#hdnisedited').val("true");
            cGrdWarehousePC.cpupdateexistingdata = null;
        }
        if (cGrdWarehousePC.cpupdatenewdata != null) {

            $('#hdnisedited').val("true");

            cGrdWarehousePC.cpupdateexistingdata = null;
        }

        if (cGrdWarehousePC.cpupdatemssgserialsetdisblebatch != null) {
            ctxtbatch.SetEnabled(false);
            ctxtexpirdate.SetEnabled(false);
            ctxtmkgdate.SetEnabled(false);
            cGrdWarehousePC.cpupdatemssgserialsetdisblebatch = null;
        }
        if (cGrdWarehousePC.cpupdatemssgserialsetenablebatch != null) {
            ctxtbatch.SetEnabled(true);
            ctxtexpirdate.SetEnabled(true);
            ctxtmkgdate.SetEnabled(true);
            $('#hidencountforserial').val(1);

            $('#hdnbatchchanged').val("0");
            $('#hidencountforserial').val("1");
            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            ctxtbatch.SetText("");
            cGrdWarehousePC.cpupdatemssgserialsetenablebatch = null;
        }


        if (cGrdWarehousePC.cpproductname != null) {
            document.getElementById('lblpro').innerHTML = cGrdWarehousePC.cpproductname;
            cGrdWarehousePC.cpproductname = null;
        }

       

    if (cGrdWarehousePC.cpupdatemssg != null) {
        if (cGrdWarehousePC.cpupdatemssg == "Saved Successfully.") {
            $('#hdntotalqntyPC').val("0");
            $('#hdnbatchchanged').val("0");
            $('#hidencountforserial').val("1");
            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");

            parent.cPopup_WarehousePC.Hide();
            var hdnselectedbranch = $('#hdnselectedbranch').val();
            grid.batchEditApi.StartEdit(globalRowIndex, 9);
            //cOpeningGrid.Enable = false;
            // parent.cOpeningGrid.PerformCallback("branchwise~" + hdnselectedbranch);
        } else {
            jAlert(cGrdWarehousePC.cpupdatemssg);
        }

        cGrdWarehousePC.cpupdatemssg = null;


    }
        if (cGrdWarehousePC.cpupdatemssgserial != null) {
            jAlert(cGrdWarehousePC.cpupdatemssgserial);
            cGrdWarehousePC.cpupdatemssgserial = null;
        }

        if (cGrdWarehousePC.cpinsertmssg != null) {
            $('#hidencountforserial').val(2);
            ctxtserial.SetValue("");
            ctxtserial.Focus();
            cGrdWarehousePC.cpinsertmssg = null;
        }
        if (cGrdWarehousePC.cpinsertmssgserial != null) {

            ctxtserial.SetValue("");
            ctxtserial.Focus();
            cGrdWarehousePC.cpinsertmssgserial = null;
        }


    }
    function Onddl_VatGstCstEndCallback(s, e) {
        if (s.GetItemCount() == 1) {
            cddlVatGstCst.SetEnabled(false);
        }
    }
 

document.onkeydown = function (e) {
    if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        //  StopDefaultAction(e);
        alert();

        btnSave_QuoteAddress();
        // document.getElementById('Button3').click();

        // return false;
    }

    if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        StopDefaultAction(e);
        alert('');

        page.SetActiveTabIndex(0);
        gridLookup.Focus();
        // document.getElementById('Button3').click();

        // return false;
    }
}
