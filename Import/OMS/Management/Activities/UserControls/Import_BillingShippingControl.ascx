<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Import_BillingShippingControl.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.Import_BillingShippingControl" %>

<script>
    var ucbsCustID, ucbsBranchID, ucbsDocType;
    $(document).ready(function () {
        console.log($("#hfBSShipToPartyVisibility").val());
        if ($("#hfBSShipToPartyVisibility").val() == "Yes") {
            $("#shippingcustomer").show();
        }
        var urlKeys = getUrlVars();
        if (urlKeys.type != 'CN' && urlKeys.type != 'VN' ) {
            if (urlKeys.key != 'ADD') {
                var docType = urlKeys.type;
                if (urlKeys.Flag != undefined && urlKeys.Flag != null && urlKeys.Flag != '') {
                    docType = 'SI';
                }
                debugger;
                //var param = "EditDetails~" + urlKeys.key + "~" + urlKeys.type;
                var param = "EditDetails~" + urlKeys.key + "~" + docType;
                ucbsDocType = docType;
                cbsComponentPanel.PerformCallback(param);
            }
        } 

    }); 

    function EditAddressSinglePage(docKey, docType) {
        var param = "EditDetails~" + docKey + "~" + docType;
        ucbsDocType = docType;
        cbsComponentPanel.PerformCallback(param);
    }
    //////### Scripting for New Billing-Shipping Control ######

    //////### Billing/Shipping details get by Doc Tagging ######
    function BSDocTagging(docID, docType) {
        var param = "EditDetails~" + docID + "~" + docType;
        ucbsDocType = docType;
        cbsComponentPanel.PerformCallback(param);
    }

    //////### Billing/Shipping details get by Doc Tagging For Transit Purchase ######
    function BSDocTaggingTSI(docID, docType) {
        var param = "DocTaggingTPB~" + docID + "~" + docType;
        ucbsDocType = docType;
        cbsComponentPanel.PerformCallback(param);
    }

    /// For Billing Section
    function bsBCmbCountryOnCountryChanged(bsBCmbCountry) {
        cbsBCmbState.PerformCallback(cbsBCmbCountry.GetValue().toString());
    }

    /// For Billing Section
    function bsBCmbPinOnChanged(bsBCmbPin) {
        if (bsBCmbPin.GetText().length == 6)
            cBillingPinChangeComponent.PerformCallback("PinChangeEvent~" + bsBCmbPin.GetValue().toString() + "~Billing" + "~" + ucbsBranchID + "~" + ucbsCustID);
    }

    /// For Shipping Section
    function bsSCmbPinOnChanged(bsSCmbPin) {
        if (bsSCmbPin.GetText().length == 6)
            cShippingPinChangeComponent.PerformCallback("PinChangeEvent~" + bsSCmbPin.GetValue().toString() + "~Shipping" + "~" + ucbsBranchID + "~" + ucbsCustID);

    }

    function bsBCmbStateOnStateChanged(bsBCmbState) {
        var stateID = (cbsBCmbState.GetValue() != null) ? cbsBCmbState.GetValue().toString() : "";
        var param = "StateGSTIN~" + ucbsCustID + '~' + ucbsBranchID + "~Billing~" + stateID + '~' + ucbsDocType;
        cbsComponentPanel.PerformCallback(param);
        //ctaxUpdatePanel.PerformCallback('DeleteAllTax'); For Tax Cleaning
    }

    function bsBCmbCityOnCityChanged(bsBCmbCity) {

        cbsBCmbPin.PerformCallback((cbsBCmbCity.GetValue() != null) ? cbsBCmbCity.GetValue().toString() : "");
        cbsBCmbArea.PerformCallback((cbsBCmbCity.GetValue() != null) ? cbsBCmbCity.GetValue().toString() : "");
    }
    /// For Billing Section End

    /// For Shipping Section
    function bsSCmbCountryOnCountryChanged(bsBCmbCountry) {
        cbsSCmbState.PerformCallback(cbsSCmbCountry.GetValue().toString());
    }

    function bsSCmbStateOnStateChanged(bsBCmbState) {
        if (cbsSCmbState.GetValue() != null) {
            var stateID = (cbsSCmbState.GetValue() != null) ? cbsSCmbState.GetValue().toString() : "";
            var param = "StateGSTIN~" + ucbsCustID + '~' + ucbsBranchID + "~Shipping~" + stateID + '~' + ucbsDocType;
            cbsComponentPanel.PerformCallback(param);
            //ctaxUpdatePanel.PerformCallback('DeleteAllTax'); For Tax Cleaning
        }

    }

    function bsSCmbCityOnCityChanged(bsBCmbCity) {

        cbsSCmbPin.PerformCallback((cbsSCmbCity.GetValue() != null) ? cbsSCmbCity.GetValue().toString() : "");
        cbsSCmbArea.PerformCallback((cbsSCmbCity.GetValue() != null) ? cbsSCmbCity.GetValue().toString() : "");
    }
    /// For Shipping Section End

    /// For Shipping Customer Section
    function bsSCmbCustomerOnCustomerChanged(bsSCmbCustomer) {
        var param = "ShippingCustomer~" + bsSCmbCustomer.GetValue();
        cShippingComponent.PerformCallback(param);
    }

    /// Common Function...
    function fnSaveBillingShipping() {
        var checkFlag = false;
        if (cbsBChkBillingToShipping.GetChecked()) {
            checkFlag = validationBillingChecking();
            if (checkFlag == true) {
                CopyBillingToShipping();
            }
        }
        else if (cbsSChkShiipingToBilling.GetChecked()) {
            checkFlag = validationShippingChecking();
            if (checkFlag == true) {
                CopyShippingToBilling();
            }
        }
        else {
            checkFlag = validationChecking();
        }

        if (checkFlag == true) {
            page.SetActiveTab(page.tabs[0]);
            page.tabs[0].SetEnabled(true);
            page.tabs[1].SetEnabled(true);
            //if ($("#hfTermsConditionDocType").val() == "CBE") {
            //    $("#txtNarration").focus();
            //}
            var urlKeys = getUrlVars();
            if (urlKeys.status == "2" || urlKeys.status == "3") {
                $("#ApprovalCross").show();
                $("#divcross").hide();
            }
            else {
                $("#divcross").show();
                $("#ApprovalCross").hide();
            }
        }
        /// ------ Set Reverse Charge Mechanism in page level
        if ($("#hfTermsConditionDocType").val() == "PB" || $("#hfTermsConditionDocType").val() == "TPB" || $("#hfTermsConditionDocType").val() == "CBE") {
            if (cbsSTxtGSTIN1.GetText() == "" && cbsSTxtGSTIN2.GetText() == "" && cbsSTxtGSTIN3.GetText() == "") {
                $("#chk_reversemechenism").attr("checked", true);
            }
            else {
                $("#chk_reversemechenism").attr("checked", true)
            }
        }
    }

    function cbsComponentPanel_endcallback() {

        if ($("#hfBSShipToPartyVisibility").val() == "Yes") {
            $("#shippingcustomer").show();
        }

        GlobalBillingShippingEndCallBack();


        if (cbsBCmbCountry.GetText().toUpperCase() != "INDIA") {
            $("#billingGSTINDiv").hide();
            $("#shippingGSTINDiv").hide();
        }

        if ($("#ucBShfCustID").val() != "") {
            ucbsCustID = $("#ucBShfCustID").val();
        }
        if ($("#ucBShfBranchID").val() != "") {
            ucbsBranchID = $("#ucBShfBranchID").val();
        }

        if ($("#hfucBSBtnSaveFlag").val() == "1") {
            page.SetActiveTab(page.tabs[0]);
            $("#hfucBSBtnSaveFlag").val("0");
        }

        if (cbsComponentPanel.cpDocType == "PC" || cbsComponentPanel.cpDocType == "PB" || cbsComponentPanel.cpDocType == "TPB" || cbsComponentPanel.cpDocType == "PO"
            || cbsComponentPanel.cpDocType == "PRI" || cbsComponentPanel.cpDocType == "SR" || cbsComponentPanel.cpDocType == "VPR"
            || cbsComponentPanel.cpDocType == "VN" || cbsComponentPanel.cpDocType == "CR" || cbsComponentPanel.cpDocType == "SRN" || cbsComponentPanel.cpDocType == "SRM") {

            //$("#billingLookupDiv").hide();
            //$("#shippingLookupDiv").hide();
            //$("#shippingcustomer").hide();
            cbsBHeaderText.SetText("Our Billing Address");
            cbsSHeaderText.SetText("Our Shipping Address");
        }

        if (ucbsDocType == "TPB") {
            $("#DivShipping").show();
            cbsBChkBillingToShipping.SetVisible(false);
            cbsSChkShiipingToBilling.SetVisible(false);
            cbsBChkBillingToShipping.SetChecked(false);
            cbsSChkShiipingToBilling.SetChecked(false);
        }
        if ($("#hfTermsConditionDocType").val() == "CBE") {
            $("#shippingcustomer").css("display", "none");
            $("#billingLookupDiv").css("display", "none");
            $("#shippingLookupDiv").css("display", "none");
            $("#billingGSTINDiv").css("display", "block");
            $("#shippingGSTINDiv").css("display", "block");

            cbsBTxtGSTIN1.GetInputElement().readOnly = false;
            cbsBTxtGSTIN2.GetInputElement().readOnly = false;
            cbsBTxtGSTIN3.GetInputElement().readOnly = false;
            cbsSTxtGSTIN1.GetInputElement().readOnly = false;
            cbsSTxtGSTIN2.GetInputElement().readOnly = false;
            cbsSTxtGSTIN3.GetInputElement().readOnly = false;
        }

    }

    function cBillingComponent_endcallback() {
        if ($("#hfBSShipToPartyVisibility").val() == "Yes") {
            $("#shippingcustomer").show();
        }

        if (cBillingComponent.cpBPinChange === "PinChangeEvent") {
            cBillingComponent.cpBPinChange = null;
            cbsBCmbArea.Focus();
        }

        if (cBillingComponent.cpBSBillingLookup == "1") {
            cBillingComponent.cpBSBillingLookup = null;
            cbsBTxtAddress1.Focus();
        }
        if (cbsBChkBillingToShipping.GetChecked() == false) {
            $("#DivShipping").show();
        }
        else {
            $("#DivShipping").hide();
        }

    }

    function cShippingComponent_endcallback() {
        if ($("#hfBSShipToPartyVisibility").val() == "Yes") {
            $("#shippingcustomer").show();
        }

        if (cShippingComponent.cpPinChange === "PinChangeEvent") {
            cShippingComponent.cpPinChange = null;
            cbsSCmbArea.Focus();
        }
        if (cShippingComponent.cpBSShippingLookup == "1") {
            cShippingComponent.cpBSShippingLookup = null;
            cbsSTxtAddress1.Focus();
        }
        if (cbsSChkShiipingToBilling.GetChecked() == false) {
            $("#DivBilling").show();
        }
        else {
            $("#DivBilling").hide();
        }
    }

    function cBillingPinChangeComponent_endcallback() {
        if (cBillingComponent.cpBPinChange === "PinChangeEvent") {
            cBillingComponent.cpBPinChange = null;
            cbsBCmbArea.Focus();
        }
    }

    function cShippingPinChangeComponent_endcallback() {
        if (cShippingComponent.cpPinChange === "PinChangeEvent") {
            cShippingComponent.cpPinChange = null;
            cbsSCmbArea.Focus();
        }
    }

    function CopyBillingToShipping() {
        debugger;
        switch (cbsBChkBillingToShipping.GetChecked()) {
            case true:
                cbsSTxtAddress1.SetText(cbsBTxtAddress1.GetText());
                cbsSTxtAddress2.SetText(cbsBTxtAddress2.GetText());
                cbsSTxtAddress3.SetText(cbsBTxtAddress3.GetText());
                cbsSTxtLandmark.SetText(cbsBTxtLandmark.GetText());

                cbsSCmbPin.SetValue(cbsBCmbPin.GetValue());
                cbsSCmbPin.SetText(cbsBCmbPin.GetText());

                $("#bsSCmbCountryHF").val($("#bsBCmbCountryHF").val());
                cbsSCmbCountry.SetText(cbsBCmbCountry.GetText());


                $("#ucBShfSStateCode").val(cbsBCmbState.GetText());
                $("#bsSCmbStateHF").val($("#bsBCmbStateHF").val());
                cbsSCmbState.SetText(cbsBCmbState.GetText());

                $("#bsSCmbCityHF").val($("#bsBCmbCityHF").val());
                cbsSCmbCity.SetText(cbsBCmbCity.GetText());

                cbsSCmbArea.SetValue(cbsBCmbArea.GetValue());
                cbsSCmbArea.SetText(cbsBCmbArea.GetText());

                cbsSTxtGSTIN1.SetText(cbsBTxtGSTIN1.GetText());
                cbsSTxtGSTIN2.SetText(cbsBTxtGSTIN2.GetText());
                cbsSTxtGSTIN3.SetText(cbsBTxtGSTIN3.GetText());

                cbsSCustomer.SetValue('')

                $("#DivShipping").css("display", "none");
                break;
            case false:
                cbsSTxtAddress1.SetText('');
                cbsSTxtAddress2.SetText('');
                cbsSTxtAddress3.SetText('');
                cbsSTxtLandmark.SetText('');

                cbsSCmbPin.SetValue('0');
                cbsSCmbPin.SetText('');

                $("#bsSCmbCountryHF").val('0');
                cbsSCmbCountry.SetText('');

                $("#bsSCmbStateHF").val('0');
                cbsSCmbState.SetText('');

                $("#bsSCmbCityHF").val('0');
                cbsSCmbCity.SetText('');

                cbsSCmbArea.SetValue('0');
                cbsSCmbArea.SetText('');

                cbsSTxtGSTIN1.SetText('');
                cbsSTxtGSTIN2.SetText('');
                cbsSTxtGSTIN3.SetText('');

                $("#DivShipping").css("display", "block");
                break;
        }
    }

    function CopyShippingToBilling() {
        switch (cbsSChkShiipingToBilling.GetChecked()) {
            case true:
                cbsBTxtAddress1.SetText(cbsSTxtAddress1.GetText());
                cbsBTxtAddress2.SetText(cbsSTxtAddress2.GetText());
                cbsBTxtAddress3.SetText(cbsSTxtAddress3.GetText());
                cbsBTxtLandmark.SetText(cbsSTxtLandmark.GetText());

                cbsBCmbPin.SetValue(cbsSCmbPin.GetValue());
                cbsBCmbPin.SetText(cbsSCmbPin.GetText());

                $("#bsBCmbCountryHF").val($("#bsSCmbCountryHF").val());
                cbsBCmbCountry.SetText(cbsSCmbCountry.GetText());

                $("#bsBCmbStateHF").val($("#bsSCmbStateHF").val());
                cbsBCmbState.SetText(cbsSCmbState.GetText());

                $("#bsBCmbCityHF").val($("#bsSCmbCityHF").val());
                cbsBCmbCity.SetText(cbsSCmbCity.GetText());

                cbsBCmbArea.SetValue(cbsSCmbArea.GetValue());
                cbsBCmbArea.SetText(cbsSCmbArea.GetText());

                cbsBTxtGSTIN1.SetText(cbsSTxtGSTIN1.GetText());
                cbsBTxtGSTIN2.SetText(cbsSTxtGSTIN2.GetText());
                cbsBTxtGSTIN3.SetText(cbsSTxtGSTIN3.GetText());

                $("#DivBilling").css("display", "none");
                break;
            case false:
                cbsBTxtAddress1.SetText('');
                cbsBTxtAddress2.SetText('');
                cbsBTxtAddress3.SetText('');
                cbsBTxtLandmark.SetText('');

                cbsBCmbPin.SetValue('0');
                cbsBCmbPin.SetText('');

                $("#bsBCmbCountryHF").val('0');
                cbsBCmbCountry.SetText('');

                $("#bsBCmbStateHF").val('0');
                cbsBCmbState.SetText('');

                $("#bsBCmbCityHF").val('0');
                cbsBCmbCity.SetText('');

                cbsBCmbArea.SetValue('0');
                cbsBCmbArea.SetText('');

                cbsBTxtGSTIN1.SetText('');
                cbsBTxtGSTIN2.SetText('');
                cbsBTxtGSTIN3.SetText('');

                $("#DivBilling").css("display", "block");
                break;
        }
    }

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

    function LoadCustomerAddress(custID, branchID, docType) {
        ucbsCustID = custID;
        ucbsBranchID = branchID;
        ucbsDocType = docType;
        var param = "CustAddDetails~" + ucbsCustID + '~' + ucbsBranchID + '~' + docType;
        cbsComponentPanel.PerformCallback(param);

    }

    function LoadCustomerAddressSinglePage(custID, branchID, docType) {

        ucbsCustID = custID;
        ucbsBranchID = branchID;
        ucbsDocType = docType;
        var param = "CustAddDetails~" + ucbsCustID + '~' + ucbsBranchID + '~' + docType;
        cbsComponentPanel.PerformCallback(param);

    }

    function LoadGSTIN(branchID) {
        var param = "GSTIN~" + ucbsCustID + '~' + branchID;
        cbsComponentPanel.PerformCallback(param);
    }
    /// Common Function End ...

    /// Validation Checking ...
    function validationChecking() {
        var checking = true, ctrllVal = true;
        ctrllVal = validateControl(cbsBTxtAddress1, 'badd1');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsSTxtAddress1, 'sadd1');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsBCmbCountry, 'bcountry');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsSCmbCountry, 'scountry');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsBCmbState, 'bstate');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsSCmbState, 'sstate');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsBCmbCity, 'bcity');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsSCmbCity, 'scity');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsBCmbPin, 'bpin');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsSCmbPin, 'spin');
        checking = checking && ctrllVal;

        if (ucbsDocType == "TPB") {
            if ($("#hfBSShipToPartyVisibility").val() == "Yes") {
                ctrllVal = validateControl(cbsSCustomer, 'sshiptoparty');
                checking = checking && ctrllVal;
            }
        }

        return checking;
    }

    function validationBillingChecking() {
        var checking = true, ctrllVal = true;
        ctrllVal = validateControl(cbsBTxtAddress1, 'badd1');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsBCmbCountry, 'bcountry');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsBCmbState, 'bstate');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsBCmbCity, 'bcity');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsBCmbPin, 'bpin');
        checking = checking && ctrllVal;

        return checking;
    }

    function validationShippingChecking() {
        var checking = true, ctrllVal = true;

        ctrllVal = validateControl(cbsSTxtAddress1, 'sadd1');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsSCmbCountry, 'scountry');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsSCmbState, 'sstate');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsSCmbCity, 'scity');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(cbsSCmbPin, 'spin');
        checking = checking && ctrllVal;

        return checking;
    }

    function validateControl(ctrl, mandateID) {
        if (ctrl.GetValue() == '' || ctrl.GetValue() == null || ctrl.GetValue() == "0") {
            $('#' + mandateID).attr('style', 'display:block');
            ctrl.Focus();
            return false;
        }
        else { $('#' + mandateID).attr('style', 'display:none'); return true; }
    }
    /// Validation Checking End...

    /// GridLookup
    function GetBillingAddressDetailByAddressId(e) {
        var addresskey = billingLookup.GetGridView().GetRowKey(billingLookup.GetGridView().GetFocusedRowIndex());
        if (addresskey != null && addresskey != '') {
            cBillingComponent.PerformCallback('BillingLookup~' + ucbsCustID + '~' + ucbsBranchID + '~' + addresskey + '~' + ucbsDocType);
        }
    }

    function GetShippingAddressDetailByAddressId(e) {
        var saddresskey = shippingLookup.GetGridView().GetRowKey(shippingLookup.GetGridView().GetFocusedRowIndex());
        if (saddresskey != null && saddresskey != '') {
            if (cbsSCustomer.GetValue() != "") {
                ucbsCustID = cbsSCustomer.GetValue();
            }

            cShippingComponent.PerformCallback('ShippingLookup~' + ucbsCustID + '~' + ucbsBranchID + '~' + saddresskey + '~' + ucbsDocType);
        }
    }

    function CloseBillingAddressLookup() {
        billingLookup.ConfirmCurrentSelection();
        billingLookup.HideDropDown();
        billingLookup.Focus();
    }

    function CloseShippingAddressLookup() {
        shippingLookup.ConfirmCurrentSelection();
        shippingLookup.HideDropDown();
        shippingLookup.Focus();
    }
    ///
    function StateGSTINLoad(userParam) {
        var param = "StateGSTIN~" + ucbsCustID + '~' + ucbsBranchID + "~" + userParam;
        cbsComponentPanel.PerformCallback(param);
    }

    function ClearBillingShipping(type) {

        switch (type) {
            case "Billing":
                //// --------------  Billing ----------
                cbsBTxtAddress1.SetValue('');
                cbsBTxtAddress2.SetValue('');
                cbsBTxtAddress3.SetValue('');
                cbsBTxtLandmark.SetValue('');
                cbsBCmbCountry.SetValue('');
                cbsBCmbState.SetValue('');
                cbsBCmbCity.SetValue('');
                cbsBCmbPin.SetValue('');
                cbsBCmbArea.SetValue('');
                cbsBTxtGSTIN1.SetValue('');
                cbsBTxtGSTIN2.SetValue('');
                cbsBTxtGSTIN3.SetValue('');
                cbsBChkBillingToShipping.SetChecked(false);
            case "Shipping":
                //// --------------  Shipping ----------
                cbsSCustomer.SetValue('');
                cbsSTxtAddress1.SetValue('');
                cbsSTxtAddress2.SetValue('');
                cbsSTxtAddress3.SetValue('');
                cbsSTxtLandmark.SetValue('');
                cbsSCmbCountry.SetValue('');
                cbsSCmbState.SetValue('');
                cbsSCmbCity.SetValue('');
                cbsSCmbPin.SetValue('');
                cbsSCmbArea.SetValue('');
                cbsSTxtGSTIN1.SetValue('');
                cbsSTxtGSTIN2.SetValue('');
                cbsSTxtGSTIN3.SetValue('');
                cbsSChkShiipingToBilling.SetChecked(false);
            default:
                //// --------------  Billing ----------
                cbsBTxtAddress1.SetValue('');
                cbsBTxtAddress2.SetValue('');
                cbsBTxtAddress3.SetValue('');
                cbsBTxtLandmark.SetValue('');
                cbsBCmbCountry.SetValue('');
                cbsBCmbState.SetValue('');
                cbsBCmbCity.SetValue('');
                cbsBCmbPin.SetValue('');
                cbsBCmbArea.SetValue('');
                cbsBTxtGSTIN1.SetValue('');
                cbsBTxtGSTIN2.SetValue('');
                cbsBTxtGSTIN3.SetValue('');
                cbsBChkBillingToShipping.SetChecked(false);
                //// --------------  Shipping ----------
                cbsSCustomer.SetValue('');
                cbsSTxtAddress1.SetValue('');
                cbsSTxtAddress2.SetValue('');
                cbsSTxtAddress3.SetValue('');
                cbsSTxtLandmark.SetValue('');
                cbsSCmbCountry.SetValue('');
                cbsSCmbState.SetValue('');
                cbsSCmbCity.SetValue('');
                cbsSCmbPin.SetValue('');
                cbsSCmbArea.SetValue('');
                cbsSTxtGSTIN1.SetValue('');
                cbsSTxtGSTIN2.SetValue('');
                cbsSTxtGSTIN3.SetValue('');
                cbsSChkShiipingToBilling.SetChecked(false);

                $("#DivBilling").css("display", "block");
                $("#DivShipping").css("display", "block");
        }
    }


    //////### END -- Scripting for New Billing-Shipping Control -- END ######
</script>
<style>
    .mandt {
        position: absolute;
        right: -19px;
        top: 4px;
    }

    .labelClass {
        color: #ffffff;
        background-color: rgb(12, 135, 202);
        font-size: 15px;
    }
</style>
<dxe:ASPxCallbackPanel runat="server" ID="bsComponentPanel" ClientInstanceName="cbsComponentPanel" OnCallback="bsComponentPanel_Callback">
    <panelcollection>
            <dxe:PanelContent runat="server">
                <div>
                    <asp:HiddenField ID="hfBSAlertFlag" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="ucBShfCustID" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="ucBShfBranchID" runat="server"></asp:HiddenField>
                    
                    <asp:HiddenField ID="ucBShfCustomerName" runat="server"></asp:HiddenField>
                    <div class="row">
                        <asp:HiddenField ID="hfucBSBtnSaveFlag" runat="server" />
                          <%--Billing Address--%>
                        <dxe:ASPxCallbackPanel runat="server" ID="BillingComponent" ClientInstanceName="cBillingComponent" OnCallback="BillingComponent_Callback">
                            <panelcollection>
                                    <dxe:PanelContent runat="server">
                                        <div class="col-md-5 mbot5" id="DivBilling">
                                            <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">
                                                <h5 class="headText" style="margin: 0; padding: 10px 15px 10px; background: #ececec; margin-bottom: 10px">
                                                <%--<asp:Label ID="bsBHeaderText" runat="server" Text="Billing Address" CssClass="newLbl"></asp:Label>--%>
                                                <dxe:ASPxLabel ID="bsBHeaderText" runat="server"  ClientInstanceName="cbsBHeaderText" Text="Billing Address" CssClass="newLbl" 
                                                    style="font-weight: bold;font-size: 14px;"></dxe:ASPxLabel>
                                                    <asp:HiddenField ID="bshfBHeaderText" runat="server" />
                                                </h5>
                                                <div style="padding-right: 8px">
                                                    <div id="billingLookupDiv">
                                                        <div class="col-md-4" style="height: auto;">
                                                        <asp:Label ID="LblType" runat="server" Text="Select Address:" CssClass="newLbl"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="Left_Content relative">
                                                            <dxe:ASPxGridLookup ID="billingAddress" runat="server" TabIndex="1" ClientInstanceName="billingLookup"
                                                                KeyFieldName="add_id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" SelectionMode="Single"
                                                                OnDataBinding="BillingAddress_DataBinding">
                                                                <Columns>
                                                                    <dxe:GridViewDataColumn FieldName="add_addressType" Visible="true" VisibleIndex="0" Caption="Type" Width="250" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="address" Visible="true" VisibleIndex="1" Caption="Address" Width="250" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="City_Name" Visible="true" VisibleIndex="2" Caption="City" Settings-AllowAutoFilter="False" Width="100">
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="State" Visible="true" VisibleIndex="3" Caption="State" Width="100" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="pin_code" Visible="true" VisibleIndex="4" Caption="Zip" Width="80" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="Country_Name" Visible="true" VisibleIndex="5" Caption="Country" Width="100" Settings-AutoFilterCondition="Contains" />

                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseBillingAddressLookup" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>

                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                                    <Settings ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                                <ClientSideEvents TextChanged="function(s, e) { GetBillingAddressDetailByAddressId(e)}" />
                                                                <ClearButton DisplayMode="Auto">
                                                                </ClearButton>
                                                            </dxe:ASPxGridLookup>
                                                        </div>
                                                    </div>
                                                    </div>
                                                    <div class="clear"></div>
                                   
                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                        Address1:<span style="color: red;"> *</span>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="Left_Content relative">
                                                            <dxe:ASPxTextBox ID="bsBTxtAddress1" MaxLength="80" ClientInstanceName="cbsBTxtAddress1"
                                                                runat="server" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                            <span id="badd1" style="display: none" class="mandt">
                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                            Address2:
                                                    </div>
                                                    <%--Start of Address2 --%>
                                                    <div class="col-md-8">
                                                        <div class="Left_Content relative">
                                                            <dxe:ASPxTextBox ID="bsBTxtAddress2" MaxLength="80" ClientInstanceName="cbsBTxtAddress2"
                                                                runat="server" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                        Address3:
                                                    </div>
                                                    <%--Start of Address3 --%>
                                                    <div class="col-md-8">
                                                        <div class="Left_Content relative">
                                                            <dxe:ASPxTextBox ID="bsBTxtAddress3" MaxLength="80" ClientInstanceName="cbsBTxtAddress3"
                                                                runat="server" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                    <%--Start of Landmark --%>
                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                            Landmark:
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="Left_Content relative">
                                                            <dxe:ASPxTextBox ID="bsBTxtLandmark" MaxLength="80" ClientInstanceName="cbsBTxtLandmark"
                                                                runat="server" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                     <%--start of Pin/Zip.--%>
                                                    <div class="col-md-4" style="height: auto;">
                                                        <asp:Label ID="Label8" runat="server" Text="Pin/Zip (Min 6 digits):" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="Left_Content relative">
                                                            <dxe:ASPxComboBox ID="bsBCmbPin" ClientInstanceName="cbsBCmbPin" runat="server" EnableCallbackMode="True" CallbackPageSize="10" FilterMinLength="6"
                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"  OnCallback="bsBCmbPin_OnCallback">
                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { bsBCmbPinOnChanged(s); }"></ClientSideEvents>
                                                            </dxe:ASPxComboBox>
                                                            <span id="bpin" style="display: none" class="mandt">
                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="clear"></div>

                                                    <dxe:ASPxCallbackPanel runat="server" ID="BillingPinChangeComponent" ClientInstanceName="cBillingPinChangeComponent" OnCallback="BillingPinChangeComponent_Callback">
                                                        <panelcollection>
	                                                        <dxe:PanelContent runat="server">
                                                                <%--Start of Country--%>
                                                                <div class="col-md-4" style="height: auto;">
                                                                    <asp:Label ID="Label2" runat="server" Text="Country:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                </div>
                                                                <div class="col-md-8">
                                                                    <div class="Left_Content relative">
                                                                        <dxe:ASPxLabel ID="bsBCmbCountry" runat="server"  ClientInstanceName="cbsBCmbCountry" Width="100%" CssClass="labelClass" 
                                                                            style="font-weight: bold;font-size: 14px;"></dxe:ASPxLabel>
                                                                        <asp:HiddenField ID="bsBCmbCountryHF" runat="server" Value="0" />
                                                                        <%--<dxe:ASPxComboBox ID="bsBCmbCountry" ClientInstanceName="cbsBCmbCountry" runat="server" ValueType="System.String" Width="100%">
                                                              
                                                                        </dxe:ASPxComboBox>--%>
                                                                        <span id="bcountry" style="display: none" class="mandt">
                                                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" style="height: auto;">
                                                                    <asp:Label ID="Label4" runat="server" Text="State:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                </div>
                                                                <div class="col-md-8">
                                                                    <div class="Left_Content relative">
                                                                         <dxe:ASPxLabel ID="bsBCmbState" runat="server"  ClientInstanceName="cbsBCmbState" Width="100%" CssClass="labelClass"  
                                                                            style="font-weight: bold;font-size: 14px;"></dxe:ASPxLabel>
                                                                            <asp:HiddenField ID="bsBCmbStateHF" runat="server" Value="0" />
                                                                      <%--  <dxe:ASPxComboBox ID="bsBCmbState" ClientInstanceName="cbsBCmbState" runat="server"
                                                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"
                                                                            OnCallback="bsBCmbState_OnCallback">
                                                                            <ClearButton DisplayMode="Always"></ClearButton>
                                                              
                                                                        </dxe:ASPxComboBox>--%>
                                                                        <span id="bstate" style="display: none" class="mandt">
                                                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                                <%--start of City/district.--%>
                                                                <div class="col-md-4" style="height: auto;">
                                                                    <asp:Label ID="Label6" runat="server" Text="City/District:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                </div>
                                                                <div class="col-md-8">
                                                                    <div class="Left_Content relative">
                                                                         <dxe:ASPxLabel ID="bsBCmbCity" runat="server"  ClientInstanceName="cbsBCmbCity" Width="100%" CssClass="labelClass" 
                                                                            style="font-weight: bold;font-size: 14px;"></dxe:ASPxLabel>
                                                                            <asp:HiddenField ID="bsBCmbCityHF" runat="server" Value="0" />
                                                                        <%--<dxe:ASPxComboBox ID="bsBCmbCity" ClientInstanceName="cbsBCmbCity" runat="server"
                                                                            ValueType="System.String" Width="100%" EnableSynchronization="True" OnCallback="bsBCmbCity_OnCallback"
                                                                            EnableIncrementalFiltering="True" SelectedIndex="0"  >
                                                                            <ClearButton DisplayMode="Always"></ClearButton>
                                                               
                                                                        </dxe:ASPxComboBox>--%>
                                                                        <span id="bcity" style="display: none" class="mandt">
                                                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                                <%--start of Area--%>
                                                                <div class="col-md-4" style="height: auto;">
                                                                    <asp:Label ID="Label10" runat="server" Text="Area:" CssClass="newLbl"></asp:Label>
                                                                </div>
                                                                <div class="col-md-8">
                                                                    <div class="Left_Content">
                                                                        <dxe:ASPxComboBox ID="bsBCmbArea" ClientInstanceName="cbsBCmbArea" runat="server"
                                                                            ValueType="System.String" Width="100%"  SelectedIndex="0"  OnCallback="bsBCmbArea_OnCallback" >
                                                                            <ClearButton DisplayMode="Always"></ClearButton>
                                                                            <ClientSideEvents></ClientSideEvents>
                                                                        </dxe:ASPxComboBox>
                                                                    </div>
                                                                </div>
                                                                <div class="clear"></div>
                                                    <div id="billingGSTINDiv">
                                                        <div class="col-md-4" style="height: auto;">
                                                            <asp:Label ID="Label14" runat="server" Text="GSTIN:" CssClass="newLbl"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8">
                                                        <div class="Left_Content">
                                                             <ul class="nestedinput">
                                                                <li>
                                                                    <dxe:ASPxTextBox ID="bsBTxtGSTIN1" ClientInstanceName="cbsBTxtGSTIN1" MaxLength="2" runat="server" Width="33px" readonly ="true">
                                                                    </dxe:ASPxTextBox>
                                                                </li>
                                                                 <li class="dash">-</li>
                                                                 <li>
                                                                     <dxe:ASPxTextBox ID="bsBTxtGSTIN2" ClientInstanceName="cbsBTxtGSTIN2" MaxLength="10" runat="server" Width="90px" readonly ="true">
                                                                     </dxe:ASPxTextBox>
                                                                 </li>
                                                                 <li class="dash">-</li>
                                                                 <li>
                                                                     <dxe:ASPxTextBox ID="bsBTxtGSTIN3" ClientInstanceName="cbsBTxtGSTIN3" MaxLength="3" runat="server" Width="50px" readonly ="true">
                                                                     </dxe:ASPxTextBox>
                                                                 </li>
                                                             </ul>
                                                        </div>
                                                    </div>
                                                        <div class="clear"></div>
                                                    </div>
	                                                        </dxe:PanelContent>
                                                        </panelcollection>
                                                        <clientsideevents endcallback="cBillingPinChangeComponent_endcallback" />
                                                    </dxe:ASPxCallbackPanel>
                                                    
                                                    <div class="col-md-12" style="height: auto;">
                                                          <dxe:ASPxCheckBox ID="bsBChkBillingToShipping" runat="server" ClientInstanceName="cbsBChkBillingToShipping" Text="Shipping to be in the same location of Billing. ">
                                                            <ClientSideEvents CheckedChanged="function(s,e){CopyBillingToShipping(s);}"></ClientSideEvents>
                                                        </dxe:ASPxCheckBox>
                                                    </div>

                                                    <%-- <div class="col-md-offset-4 col-md-8">
                                                        <a href="#" onclick="javascript:openAreaPage();"><span class="Ecoheadtxt" style="color: Blue">
                                                            <strong>Add New Area</strong></span></a>
                                                    </div>--%>
                                                </div>
                                            </div>
                                        </div>
                                     </dxe:PanelContent>
                                </panelcollection>
                            <clientsideevents endcallback="cBillingComponent_endcallback" />
                        </dxe:ASPxCallbackPanel>

                         <%--Shipping Address--%>
                        <dxe:ASPxCallbackPanel runat="server" ID="ShippingComponent" ClientInstanceName="cShippingComponent" OnCallback="ShippingComponent_Callback">
                            <panelcollection>
                                <dxe:PanelContent runat="server">
                                    <div class="col-md-5 mbot5" id="DivShipping">
                                        <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">
                                            <h5 class="headText" style="margin: 0; padding: 10px 15px 10px; background: #ececec; margin-bottom: 10px">
                                                 <dxe:ASPxLabel ID="bsSHeaderText" runat="server"  ClientInstanceName="cbsSHeaderText" Text="Shipping Address" CssClass="newLbl" 
                                                style="font-weight: bold;font-size: 14px;"></dxe:ASPxLabel>
                                            </h5>
                                            <div style="padding-right: 8px">
                                                <div id="shippingcustomer" style="display:none;">
                                                        <div class="col-md-4" style="height: auto;">
                                                            <asp:Label ID="Label1" runat="server" Text="Ship To Party :" CssClass="newLbl"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8">
                                                            <div class="Left_Content relative">
                                                                <dxe:ASPxComboBox ID="bsSCustomer" ClientInstanceName="cbsSCustomer" runat="server" ValueType="System.String" Width="100%" EnableCallbackMode="true" CallbackPageSize="10" EnableIncrementalFiltering="true">
                                                                   <%--<ClearButton DisplayMode="Always"></ClearButton>--%>
                                                                     <ClientSideEvents SelectedIndexChanged="function(s, e) { bsSCmbCustomerOnCustomerChanged(s); }"></ClientSideEvents>
                                                                </dxe:ASPxComboBox>
                                                                 <span id="sshiptoparty" style="display: none" class="mandt">
                                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                        </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                <div class="clear"></div>
                                                <div id="shippingLookupDiv">
                                                <div class="col-md-4" style="height: auto;">
                                                    <asp:Label ID="Label5" runat="server" Text="Select Address:" CssClass="newLbl"></asp:Label>
                                                </div>
                                                <div class="col-md-8">
                                                    <div class="Left_Content relative">
                                                        <dxe:ASPxGridLookup ID="shippingAddress" runat="server" TabIndex="11" ClientInstanceName="shippingLookup"
                                                            KeyFieldName="add_id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" SelectionMode="Single"
                                                            OnDataBinding="ShippingAddress_DataBinding">
                                                            <Columns>
                                                                 <dxe:GridViewDataColumn FieldName="add_addressType" Visible="true" VisibleIndex="0" Caption="Type" Width="250" Settings-AutoFilterCondition="Contains" />
                                                                <dxe:GridViewDataColumn FieldName="address" Visible="true" VisibleIndex="1" Caption="Address" Width="250" Settings-AutoFilterCondition="Contains" />
                                                                <dxe:GridViewDataColumn FieldName="City_Name" Visible="true" VisibleIndex="2" Caption="City" Settings-AllowAutoFilter="False" Width="100">

                                                                    <%--<Settings AllowAutoFilter="False"></Settings>--%>
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="State" Visible="true" VisibleIndex="3" Caption="State" Width="100" Settings-AutoFilterCondition="Contains" />
                                                                <dxe:GridViewDataColumn FieldName="pin_code" Visible="true" VisibleIndex="4" Caption="Zip" Width="80" Settings-AutoFilterCondition="Contains" />
                                                                <dxe:GridViewDataColumn FieldName="Country_Name" Visible="true" VisibleIndex="5" Caption="Country" Width="100" Settings-AutoFilterCondition="Contains" />

                                                            </Columns>
                                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                <Templates>
                                                                    <StatusBar>
                                                                        <table class="OptionsTable" style="float: right">
                                                                            <tr>
                                                                                <td>
                                                                                    <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseShippingAddressLookup" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </StatusBar>
                                                                </Templates>

                                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                                <Settings ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                            </GridViewProperties>
                                                            <ClientSideEvents TextChanged="function(s, e) { GetShippingAddressDetailByAddressId(e)}" />
                                                            <ClearButton DisplayMode="Auto">
                                                            </ClearButton>
                                                        </dxe:ASPxGridLookup>
                                                    </div>
                                                </div>
                                                </div>
                                                <div class="clear"></div>
                                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                    Address1:<span style="color: red;"> *</span>
                                                </div>
                                                <div class="col-md-8">
                                                    <div class="Left_Content relative">
                                                        <dxe:ASPxTextBox ID="bsSTxtAddress1" MaxLength="80" ClientInstanceName="cbsSTxtAddress1"
                                                            runat="server" Width="100%">
                                                        </dxe:ASPxTextBox>
                                                        <span id="sadd1" style="display: none" class="mandt">
                                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                        Address2:
                                                </div>
                                                <%--Start of Address2 --%>
                                                <div class="col-md-8">
                                                    <div class="Left_Content relative">
                                                        <dxe:ASPxTextBox ID="bsSTxtAddress2" MaxLength="80" ClientInstanceName="cbsSTxtAddress2"
                                                            runat="server" Width="100%">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                    Address3:
                                                </div>
                                                <%--Start of Address3 --%>
                                                <div class="col-md-8">
                                                    <div class="Left_Content relative">
                                                        <dxe:ASPxTextBox ID="bsSTxtAddress3" MaxLength="80" ClientInstanceName="cbsSTxtAddress3"
                                                            runat="server" Width="100%">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>
                                                <%--Start of Landmark --%>
                                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                        Landmark:
                                                </div>
                                                <div class="col-md-8">

                                                    <div class="Left_Content relative">
                                                        <dxe:ASPxTextBox ID="bsSTxtLandmark" MaxLength="80" ClientInstanceName="cbsSTxtLandmark"
                                                            runat="server" Width="100%">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>
                                                <%--start of Pin/Zip.--%>
                                                <div class="col-md-4" style="height: auto;">
                                                    <asp:Label ID="Label12" runat="server" Text="Pin/Zip (Min 6 digits):" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                </div>
                                                <div class="col-md-8">
                                                    <div class="Left_Content relative">
                                                        <dxe:ASPxComboBox ID="bsSCmbPin" ClientInstanceName="cbsSCmbPin" runat="server" EnableCallbackMode="True" CallbackPageSize="10" FilterMinLength="6"
                                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"  OnCallback="bsSCmbPin_OnCallback">
                                                            <ClearButton DisplayMode="Always"></ClearButton>
                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { bsSCmbPinOnChanged(s); }"></ClientSideEvents>
                                                        </dxe:ASPxComboBox>
                                                        <span id="spin" style="display: none" class="mandt">
                                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="clear"></div>

                                                <dxe:ASPxCallbackPanel runat="server" ID="ShippingPinChangeComponent" ClientInstanceName="cShippingPinChangeComponent" OnCallback="ShippingPinChangeComponent_Callback">
	                                                <panelcollection>
		                                                <dxe:PanelContent runat="server">
                                                            <%--Start of Country--%>
                                                            <div class="col-md-4" style="height: auto;">
                                                                <asp:Label ID="Label7" runat="server" Text="Country:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <div class="Left_Content relative">
                                                                    <dxe:ASPxLabel ID="bsSCmbCountry" runat="server"  ClientInstanceName="cbsSCmbCountry" Width="100%" CssClass="labelClass" 
                                                                            style="font-weight: bold;font-size: 14px;"></dxe:ASPxLabel>
                                                                            <asp:HiddenField ID="bsSCmbCountryHF" runat="server" Value="0" />
                                                                    <span id="scountry" style="display: none" class="mandt">
                                                                        <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                    </span>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4" style="height: auto;">
                                                                <asp:Label ID="Label9" runat="server" Text="State:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <div class="Left_Content relative">
                                                                    <dxe:ASPxLabel ID="bsSCmbState" runat="server"  ClientInstanceName="cbsSCmbState" Width="100%" CssClass="labelClass"
                                                                            style="font-weight: bold;font-size: 14px;"></dxe:ASPxLabel>
                                                                            <asp:HiddenField ID="bsSCmbStateHF" runat="server" Value="0" />
                                                                        <asp:HiddenField ID="ucBShfSStateCode" runat="server"></asp:HiddenField>
                                                                        <asp:HiddenField ID="ucBShfSStateCodeText" runat="server"></asp:HiddenField>
                                                                    <span id="sstate" style="display: none" class="mandt">
                                                                        <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                    </span>
                                                                </div>
                                                            </div>
                                                            <%--start of City/district.--%>
                                                            <div class="col-md-4" style="height: auto;">
                                                                <asp:Label ID="Label11" runat="server" Text="City/District:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <div class="Left_Content relative">
                                                                    <dxe:ASPxLabel ID="bsSCmbCity" runat="server"  ClientInstanceName="cbsSCmbCity" Width="100%" CssClass="labelClass" 
                                                                        style="font-weight: bold;font-size: 14px;"></dxe:ASPxLabel>
                                                                    <asp:HiddenField ID="bsSCmbCityHF" runat="server" Value="0" />
                                                                    <span id="scity" style="display: none" class="mandt">
                                                                        <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                    </span>
                                                                </div>
                                                            </div>
                                                            <%--start of Area--%>
                                                            <div class="col-md-4" style="height: auto;">
                                                                <asp:Label ID="Label13" runat="server" Text="Area:" CssClass="newLbl"></asp:Label>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <div class="Left_Content">
                                                                    <dxe:ASPxComboBox ID="bsSCmbArea" ClientInstanceName="cbsSCmbArea" runat="server"
                                                                        ValueType="System.String" Width="100%"  SelectedIndex="0"  OnCallback="bsSCmbArea_OnCallback" >
                                                                        <ClearButton DisplayMode="Always"></ClearButton>
                                                                        <ClientSideEvents></ClientSideEvents>
                                                                    </dxe:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                             <div class="clear"></div>
                                                <div id="shippingGSTINDiv">
                                                    <div class="col-md-4" style="height: auto;">
                                                        <asp:Label ID="Label15" runat="server" Text="GSTIN:" CssClass="newLbl"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="Left_Content">
                                                            <ul class="nestedinput">
                                                                <li>
                                                                    <dxe:ASPxTextBox ID="bsSTxtGSTIN1" ClientInstanceName="cbsSTxtGSTIN1" MaxLength="2" runat="server" Width="33px" readonly ="true">
                                                                    </dxe:ASPxTextBox>
                                                                </li>
                                                                <li class="dash">- </li>
                                                                <li>
                                                                    <dxe:ASPxTextBox ID="bsSTxtGSTIN2" ClientInstanceName="cbsSTxtGSTIN2" MaxLength="10" runat="server" Width="90px" readonly ="true">
                                                                    </dxe:ASPxTextBox>
                                                                </li>
                                                                <li class="dash">- </li>
                                                                <li>
                                                                    <dxe:ASPxTextBox ID="bsSTxtGSTIN3" ClientInstanceName="cbsSTxtGSTIN3" MaxLength="3" runat="server" Width="50px" readonly ="true">
                                                                    </dxe:ASPxTextBox>
                                                                </li>
                                                             </ul>
                                                        </div>
                                                    </div>
                                                    <div class="clear"></div>
                                                </div>
		                                                </dxe:PanelContent>
	                                                </panelcollection>
	                                                <%--<clientsideevents endcallback="cShippingPinChangeComponent_endcallback" />--%>
                                                </dxe:ASPxCallbackPanel>

                                               
                                                <div class="col-md-12" style="height: auto;">
                                                    <dxe:ASPxCheckBox ID="bsSChkShiipingToBilling" runat="server" ClientInstanceName="cbsSChkShiipingToBilling" Text="Billing to be in the same location of Shipping. ">
                                                        <ClientSideEvents CheckedChanged="function(s,e){CopyShippingToBilling(s);}"></ClientSideEvents>
                                                    </dxe:ASPxCheckBox>
                                         
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </dxe:PanelContent>
                            </panelcollection>
                            <clientsideevents endcallback="cShippingComponent_endcallback" />
                        </dxe:ASPxCallbackPanel>
                        <%--<div class="col-md-2" style="padding-top:220px">
                            <dxe:ASPxButton ID="bsShiipingToBBilling" CausesValidation="true" visible="false" ClientInstanceName="cbsBBillingToShiiping" runat="server"
                                            AutoPostBack="False" Text="<=" CssClass="btn btn-primary" TabIndex="26">
                                            <ClientSideEvents Click=" function(s, e) { CopyShippingToBilling(s); }" />
                                        </dxe:ASPxButton>
                            <dxe:ASPxButton ID="bsBBillingToShiiping" CausesValidation="true" visible="false" ClientInstanceName="cbsBBillingToShiiping" runat="server"
                                            AutoPostBack="False" Text="=>" CssClass="btn btn-primary" TabIndex="26">
                                            <ClientSideEvents Click=" function(s, e) { CopyBillingToShipping(s); }" />
                                        </dxe:ASPxButton>
                            
                        </div>--%>
                     
                    </div>
                    <%--End of Address Type--%>

                    <div class="clear"></div>
                    <div class="col-md-12 pdLeft0" style="padding-top: 10px">
                        <dxe:ASPxButton ID="bsSave_BillingShipping" CausesValidation="true" ClientInstanceName="cbsSave_BillingShipping" runat="server" 
                            AutoPostBack="False" Text="O&#818;K" CssClass="btn btn-primary" UseSubmitBehavior="false">
                            <ClientSideEvents Click="function (s, e) {fnSaveBillingShipping();}" />
                        </dxe:ASPxButton>

                    </div>
                    <asp:HiddenField ID="hfVendorGSTIN" runat="server"></asp:HiddenField>
                    
                </div>
            </dxe:PanelContent>
        </panelcollection>
    <clientsideevents endcallback="cbsComponentPanel_endcallback" />
</dxe:ASPxCallbackPanel>

<asp:HiddenField ID="hfBSShipToPartyVisibility" runat="server"></asp:HiddenField>
