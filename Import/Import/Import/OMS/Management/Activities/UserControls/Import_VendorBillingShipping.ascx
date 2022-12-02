<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Import_VendorBillingShipping.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.Import_VendorBillingShipping" %>
<script>

    function fn_AllowonlyNumeric(s, e) {
        var theEvent = e.htmlEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        var regex = /[0-9]/;

        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault)
                theEvent.preventDefault();
        }
    }

    var OldBpinid = 0;
    var OldBpinCode = 0;
    var OldSpinid = 0;
    var OldSpinCode = 0;
    //Sandip Section For New Modification Start 
    function StoreBPinNo() {
        if (ctxt_BPin.GetText() != null && ctxt_BPin.GetText() != '') {
            var BPinNo = ctxt_BPin.GetText();
            if (BPinNo.length == 6) {
                if (OldBpinid != BPinNo)
                {
                    OldBpinid = BPinNo;
                    //alert(OldBpinid);
                } 
            }
            
            
                //cShippingPinChangeComponent.PerformCallback("GetShippingDetail~" + ctxt_SPin.GetText + "~Shipping" + "~" + ucbsBranchID + "~" + ucbsCustID);
            }
    }


    function SetBillingDetails() {

        if (ctxt_BPin.GetText() != null && ctxt_BPin.GetText() != '') {
            var BPinNo = ctxt_BPin.GetText();
            if (BPinNo.length == 6) {
                if (OldBpinid != BPinNo) {
                    cBillingPinChangeComponent.PerformCallback("GetBillingDetail~" + BPinNo + "~Billing" + "~" + ucbsBranchID + "~" + ucbsCustID);
                }
            }
            else
            {
                jAlert('Please insert correct Billing Pin Code');
                ctxt_BPin.SetText(OldBpinid);
            }
        }

    }

    function cBillingPinChangeComponent_endcallback() {
        if (cBillingPinChangeComponent.cpBPinChange === "PinChangeEvent") {
            cBillingPinChangeComponent.cpBPinChange = null;
            cbsBTxtAddress1.Focus();
            //cbsBCmbArea.Focus();
        }
        if (cBillingPinChangeComponent.cpInvalidPin == 'Y') {
            jAlert('Entered PIN is invalid for selected State. <br/> Cannot proceed.');
            cBillingPinChangeComponent.PerformCallback("GetBillingDetail~" + OldBpinid + "~Billing" + "~" + ucbsBranchID + "~" + ucbsCustID);
            cBillingPinChangeComponent.cpInvalidPin = null; 
        }
        if (cBillingPinChangeComponent.cpBPinNo != null && cBillingPinChangeComponent.cpBPinNo != undefined) {
            ctxt_BPin.SetText(cBillingPinChangeComponent.cpBPinNo);
            cBillingPinChangeComponent.cpBPinNo = null;
        }
    }

    function StoreSPinNo()
    {
        if (ctxt_SPin.GetText() != null && ctxt_SPin.GetText() != '')
        {
            var SPinNo = ctxt_SPin.GetText();
            if (SPinNo.length == 6) {
                if (OldSpinid != SPinNo)
                {
                    OldSpinid = SPinNo;
                }
                
            }
            //alert(OldSpinid);
            //var PinNo = ctxt_SPin.GetText();
            //if(PinNo.length==6)
            //{
            //    cShippingPinChangeComponent.PerformCallback("GetShippingDetail~" + ctxt_SPin.GetText + "~Shipping" + "~" + ucbsBranchID + "~" + ucbsCustID);
            //}
        }
    }
    function SetShippingDetails()
    {
        if (ctxt_SPin.GetText() != null && ctxt_SPin.GetText() != '') {
            var SPinNo = ctxt_SPin.GetText();
            if (SPinNo.length == 6) {
                if (OldSpinid != SPinNo)
                    {
                    cShippingPinChangeComponent.PerformCallback("GetShippingDetail~" + SPinNo + "~Shipping" + "~" + ucbsBranchID + "~" + ucbsCustID);
                }
            }
            else
            {
                jAlert('Please insert correct Shipping Pin Code');
                ctxt_SPin.SetText(OldSpinid);
            }
        }
    }  
    function cShippingPinChangeComponent_endcallback() {
        if (cShippingPinChangeComponent.cpPinChange === "PinChangeEvent") {
            cShippingPinChangeComponent.cpPinChange = null;
            cbsSTxtAddress1.Focus();
            //cbsSCmbArea.Focus();
        }
        if (cShippingPinChangeComponent.cpInvalidPin == 'Y') {
            jAlert('Entered PIN is invalid for selected State. <br/> Cannot proceed.'); 
            cShippingPinChangeComponent.PerformCallback("GetShippingDetail~" + OldSpinid + "~Shipping" + "~" + ucbsBranchID + "~" + ucbsCustID);
            cShippingPinChangeComponent.cpInvalidPin = null;
            //ctxt_SPin.SetText(OldSpinid);
            //cShippingPinChangeComponent.cpInvalidPin = null;
        }
        if(cShippingPinChangeComponent.cpSPinNo!=null &&  cShippingPinChangeComponent.cpSPinNo!=undefined)
        {
            ctxt_SPin.SetText(cShippingPinChangeComponent.cpSPinNo);
            cShippingPinChangeComponent.cpSPinNo = null;
        }

        // Code has been commented due to use branch statecode instead of Using Shipping StateCode of Branch By Sam on 14122017 
        //if (cShippingPinChangeComponent.cpStateChanged != null && cShippingPinChangeComponent.cpStateChanged != undefined) {
        //    if (cShippingPinChangeComponent.cpStateChanged == 'Y') {
        //        var GridCount = grid.GetVisibleRowsOnPage();
        //        if (GridCount > 0) {
        //            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        //            $('#hdnTaxDeleteByShippingStateMismatch').val('Y');
        //            FinalSaveUpdate();
        //        }

        //    }
        //}
    }


    


    //Sandip Section For New Modification End
    var ucbsCustID, ucbsBranchID, ucbsDocType;

    
    $(document).ready(function () {
        console.log($("#hfBSShipToPartyVisibility").val());
        if ($("#hfBSShipToPartyVisibility").val() == "Yes") {
            $("#shippingcustomer").show();
        }
        var urlKeys = getUrlVars();
        if (urlKeys.type != 'CN' && urlKeys.type != 'VN' && urlKeys.type != 'CBE') {
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
        //if ($("#DivHideShipping-1").is(':visible') || $("#DivHideBilling-1").is(':visible')) {
            var checkFlag = true;
            if (cbsBChkBillingToShipping.GetChecked()) {
                CopyBillingToShipping();
                //checkFlag = validationBillingChecking();
                //if (checkFlag == true) {
                //    CopyBillingToShipping();
                //}
            }
            else if (cbsSChkShiipingToBilling.GetChecked()) {
                CopyShippingToBilling();
                //checkFlag = validationShippingChecking();
                //if (checkFlag == true) {
                //    CopyShippingToBilling();
                //}
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
        //}
        //else
        //{
        //    page.SetActiveTab(page.tabs[0]);
        //    page.tabs[0].SetEnabled(true);
        //    page.tabs[1].SetEnabled(true);
        //}
    }

    function cbsComponentPanel_endcallback() {

        if ($("#hfBSShipToPartyVisibility").val() == "Yes") {
            $("#shippingcustomer").show();
        }

        
        //else
        //{
        //    $('#DivBSSection').attr('disabled', false);
        //}
        GlobalBillingShippingEndCallBack();


        //if (cbsBCmbCountry.GetText().toUpperCase() != "INDIA") {
        //    $("#billingGSTINDiv").hide();
        //    $("#shippingGSTINDiv").hide();
        //}

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



        // -----------------Code Updated by Sam on 13122017 To show Billing Shipping in Edit Mode Section Start------------------
        if (($("#hdnPBTaggedYorN").val() != null) && ($("#hdnPBTaggedYorN").val() != undefined)) {
            if ($("#hdnPBTaggedYorN").val() == 'Y') {
                if ($("#hfBSShipToPartyVisibility").val() == "Yes") {
                    cbsSCustomer.SetEnabled(false);
                }
                billingLookup.SetEnabled(false);
                ctxt_BPin.SetEnabled(false);
                shippingLookup.SetEnabled(false);
                ctxt_SPin.SetEnabled(false);
                cbsBChkBillingToShipping.SetEnabled(false);
                cbsSChkShiipingToBilling.SetEnabled(false);
                //cbsSave_BillingShipping.SetEnabled(false);


            }
            else {
                if ($("#hfBSShipToPartyVisibility").val() == "Yes") {
                    cbsSCustomer.SetEnabled(true);
                }
                billingLookup.SetEnabled(true);
                ctxt_BPin.SetEnabled(true);
                shippingLookup.SetEnabled(true);
                ctxt_SPin.SetEnabled(true);
                cbsBChkBillingToShipping.SetEnabled(true);
                cbsSChkShiipingToBilling.SetEnabled(true);
                //cbsSave_BillingShipping.SetEnabled(true);

            }
        }

        // -----------------Code Updated by Sam on 13122017 To show Billing Shipping in Edit Mode Section End------------------

        //if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit != null && cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit != undefined) {
        //    if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == '1') {
        //        $("#DivBSSection").prop("disabled", true);
        //        //cbsComponentPanel.ClientEnabled(false);
        //    }
        //    else {
        //        $("#DivBSSection").prop("disabled", false);
        //    }
        //}

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
            //$('#DivHideBilling').removeClass('hide');
            //$('#DivHideShipping').removeClass('hide');
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
            //$('#DivHideBilling').removeClass('hide');
            //$('#DivHideShipping').removeClass('hide');
        }
        if (cbsSChkShiipingToBilling.GetChecked() == false) {
            $("#DivBilling").show();
        }
        else {
            $("#DivBilling").hide();
        }

        if (cShippingComponent.cpStateChanged != null && cShippingComponent.cpStateChanged !=undefined) {
            if (cShippingComponent.cpStateChanged == 'Y') {
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                var frontRow = 0;
                var backRow = -1;
                var IsProduct = "";
                for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {

                    var ProductID = grid.GetEditor('Amount').GetValue();
                    var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Amount')) : 0;
                    var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Amount')) : 0;

                    if (frontProduct != "" || backProduct != "") {
                        IsProduct = "Y";
                        break;
                    }
                    backRow--;
                    frontRow++;
                }
            }
        }
    }

   

    // Checked By Sam on 04122017 
    var gloadd1 = '';
    var gloadd2 = '';
    var gloadd3 = '';
    var glolandmark = ''; 
    var gloPincode = 0;
    var gloPinId = 0; 
    var gloCountryname = '';
    var gloCountryId = 0; 
    var gloSatename = '';
    var gloStateId = 0; 
    var gloCityname = '';
    var gloCityId = 0; 
    var gloGST1 = '';
    var gloGST2 = '';
    var gloGST3 = '';


    function CopyBillingToShipping() {

        var checkFlag = false;


       


        switch (cbsBChkBillingToShipping.GetChecked()) {
            case true: 
                checkFlag = validationBillingChecking();
                if (checkFlag == true) { 
                    shippingLookup.SetValue(null);
                    
                    gloadd1 = cbsSTxtAddress1.GetText();
                    cbsSTxtAddress1.SetText(cbsBTxtAddress1.GetText());

                    
                    gloadd2 = cbsSTxtAddress2.GetText();
                    cbsSTxtAddress2.SetText(cbsBTxtAddress2.GetText());

                    
                    gloadd3 = cbsSTxtAddress3.GetText();
                    cbsSTxtAddress3.SetText(cbsBTxtAddress3.GetText());

                   
                    glolandmark = cbsSTxtLandmark.GetText();
                    cbsSTxtLandmark.SetText(cbsBTxtLandmark.GetText());

                    //cbsSCmbPin.SetValue(cbsBCmbPin.GetValue());
                    //cbsSCmbPin.SetText(cbsBCmbPin.GetText());

                    
                    gloPincode = ctxt_SPin.GetText();
                    ctxt_SPin.SetText(ctxt_BPin.GetText())

                    
                    gloPinId = $("#hdnSPinId").val();
                    $("#hdnSPinId").val($("#hdnBPinID").val())

                    
                    gloCountryname = cbsSCmbCountry.GetText();
                    cbsSCmbCountry.SetText(cbsBCmbCountry.GetText());
                    
                    gloCountryId = $("#bsSCmbCountryHF").val();
                    $("#bsSCmbCountryHF").val($("#bsBCmbCountryHF").val());
                    
                    
                    gloSatename = cbsSCmbState.GetText();
                    cbsSCmbState.SetText(cbsBCmbState.GetText());
                    $("#ucBShfSStateCode").val(cbsBCmbState.GetText());
                   
                    gloStateId = $("#bsSCmbStateHF").val();
                    $("#bsSCmbStateHF").val($("#bsBCmbStateHF").val());
                    
                    gloCityname = cbsSCmbCity.GetText();
                    cbsSCmbCity.SetText(cbsBCmbCity.GetText());
                    

                    gloCityId = $("#bsSCmbCityHF").val();
                    $("#bsSCmbCityHF").val($("#bsBCmbCityHF").val());
                    

                    //cbsSCmbArea.SetValue(cbsBCmbArea.GetValue());
                    //cbsSCmbArea.SetText(cbsBCmbArea.GetText());

                    gloGST1 = cbsSTxtGSTIN1.GetText();
                    cbsSTxtGSTIN1.SetText(cbsBTxtGSTIN1.GetText()); 

                    gloGST2 = cbsSTxtGSTIN2.GetText();
                    cbsSTxtGSTIN2.SetText(cbsBTxtGSTIN2.GetText());
                    
                    gloGST3 = cbsSTxtGSTIN3.GetText();
                    cbsSTxtGSTIN3.SetText(cbsBTxtGSTIN3.GetText());
                   

                    cbsSCustomer.SetValue('')

                    $("#DivShipping").css("display", "none");
                    break;
                }
                else
                {
                    jAlert('First fill the mandatory field');
                    cbsBChkBillingToShipping.SetChecked(false);
                    return;
                }
            case false:
                 

                //cbsSTxtAddress1.SetText('');
                cbsSTxtAddress1.SetText(gloadd1);

                //cbsSTxtAddress2.SetText('');
                cbsSTxtAddress2.SetText(gloadd2);

                //cbsSTxtAddress3.SetText('');
                cbsSTxtAddress3.SetText(gloadd3);

                //cbsSTxtLandmark.SetText(''); 
                cbsSTxtLandmark.SetText(glolandmark);

                //cbsSCmbPin.SetValue('0');
                //cbsSCmbPin.SetText('');


                //ctxt_SPin.SetText('');
                ctxt_SPin.SetText(gloPincode);

                //$("#hdnSPinId").val('0');                
                $("#hdnSPinId").val(gloPinId);

                //$("#bsSCmbCountryHF").val('0');
                cbsSCmbCountry.SetText(gloCountryname);

                //cbsSCmbCountry.SetText(''); 
                $("#bsSCmbCountryHF").val(gloCountryId);
                


                //cbsSCmbState.SetText('');
                cbsSCmbState.SetText(gloSatename);

                //$("#bsSCmbStateHF").val('0');
                $("#bsSCmbStateHF").val(gloStateId);
               

                //cbsSCmbCity.SetText('');
                cbsSCmbCity.SetText(gloCityname);

                //$("#bsSCmbCityHF").val('0'); 
                $("#bsSCmbCityHF").val(gloCityId);
               


                //cbsSCmbArea.SetValue('0');
                //cbsSCmbArea.SetText('');

                //cbsSTxtGSTIN1.SetText('');
                cbsSTxtGSTIN1.SetText(gloGST1);

                //cbsSTxtGSTIN2.SetText('');
                cbsSTxtGSTIN2.SetText(gloGST2);

                //cbsSTxtGSTIN3.SetText(''); 
                cbsSTxtGSTIN3.SetText(gloGST3);

                $("#DivShipping").css("display", "block");
                break;
        }
    }

    function CopyShippingToBilling() {
        var checkFlag = false;
        switch (cbsSChkShiipingToBilling.GetChecked()) {
            case true:
                checkFlag = validationShippingChecking();
                if (checkFlag == true) {

                    billingLookup.SetValue(null);

                    gloadd1 = cbsBTxtAddress1.GetText();
                    cbsBTxtAddress1.SetText(cbsSTxtAddress1.GetText());
                    
                    gloadd2 = cbsBTxtAddress2.GetText();
                    cbsBTxtAddress2.SetText(cbsSTxtAddress2.GetText());
                   
                    gloadd3 = cbsBTxtAddress3.GetText();
                    cbsBTxtAddress3.SetText(cbsSTxtAddress3.GetText());
                    
                    glolandmark = cbsBTxtLandmark.GetText();
                    cbsBTxtLandmark.SetText(cbsSTxtLandmark.GetText());
                    

                    //cbsBCmbPin.SetValue(cbsSCmbPin.GetValue());
                    //cbsBCmbPin.SetText(cbsSCmbPin.GetText());

                    gloPincode = ctxt_BPin.GetText();
                    ctxt_BPin.SetText(ctxt_SPin.GetText())
                    
                    gloPinId = $("#hdnBPinID").val();
                    $("#hdnBPinID").val($("#hdnSPinId").val())
                     

                    gloCountryname = cbsBCmbCountry.GetText();
                    cbsBCmbCountry.SetText(cbsSCmbCountry.GetText());
                    
                    gloCountryId = $("#bsBCmbCountryHF").val();
                    $("#bsBCmbCountryHF").val($("#bsSCmbCountryHF").val());
                    

                    gloSatename = cbsBCmbState.GetText();
                    cbsBCmbState.SetText(cbsSCmbState.GetText());
                    
                    gloStateId = $("#bsBCmbStateHF").val();
                    $("#bsBCmbStateHF").val($("#bsSCmbStateHF").val());
                     
                    gloCityname = cbsBCmbCity.GetText();
                    cbsBCmbCity.SetText(cbsSCmbCity.GetText());
                    
                    gloCityId = $("#bsBCmbCityHF").val();
                    $("#bsBCmbCityHF").val($("#bsSCmbCityHF").val());
                    
                    //cbsBCmbArea.SetValue(cbsSCmbArea.GetValue());
                    //cbsBCmbArea.SetText(cbsSCmbArea.GetText());

                    gloGST1 = cbsBTxtGSTIN1.GetText();
                    cbsBTxtGSTIN1.SetText(cbsSTxtGSTIN1.GetText());
                    
                    gloGST2 = cbsBTxtGSTIN2.GetText();
                    cbsBTxtGSTIN2.SetText(cbsSTxtGSTIN2.GetText());
                    
                    gloGST3 = cbsBTxtGSTIN3.GetText();
                    cbsBTxtGSTIN3.SetText(cbsSTxtGSTIN3.GetText());
                   
                     

                    $("#DivBilling").css("display", "none");
                    break;
                }
                else
                {
                    jAlert('First fill the mandatory field');
                    cbsSChkShiipingToBilling.SetChecked(false);
                    return;
                }
            case false:
                 
                //cbsBTxtAddress1.SetText('');
                cbsBTxtAddress1.SetText(gloadd1);

                //cbsBTxtAddress2.SetText('');
                cbsBTxtAddress2.SetText(gloadd2);

                //cbsBTxtAddress3.SetText('');
                cbsBTxtAddress3.SetText(gloadd3);

                //cbsBTxtLandmark.SetText(''); 
                cbsBTxtLandmark.SetText(glolandmark);


                //cbsBCmbPin.SetValue('0');
                //cbsBCmbPin.SetText('');

                //ctxt_BPin.SetText('');
                ctxt_BPin.SetText(gloPincode);

                //$("#hdnBPinID").val('0'); 
                $("#hdnBPinID").val(gloPinId);

               
                //cbsBCmbCountry.SetText('');
                //$("#bsBCmbCountryHF").val('0');
                cbsBCmbCountry.SetText(gloCountryname);
                $("#bsBCmbCountryHF").val(gloCountryId); 
               
                //cbsBCmbState.SetText('');
                cbsBCmbState.SetText(gloSatename);

                //$("#bsBCmbStateHF").val('0');
                $("#bsBCmbStateHF").val(gloStateId);

                //var gloCityname = '';
                //var gloCityId = 0;
                //var gloGST1 = '';
                //var gloGST2 = '';
                //var gloGST3 = '';

               
                //cbsBCmbCity.SetText('');
                cbsBCmbCity.SetText(gloCityname);

                //$("#bsBCmbCityHF").val('0'); 
                $("#bsBCmbCityHF").val(gloCityId);


                //cbsBCmbArea.SetValue('0');
                //cbsBCmbArea.SetText('');

                //cbsBTxtGSTIN1.SetText('');
                cbsBTxtGSTIN1.SetText(gloGST1);

                //cbsBTxtGSTIN2.SetText('');
                cbsBTxtGSTIN2.SetText(gloGST2);

                //cbsBTxtGSTIN3.SetText(''); 
                cbsBTxtGSTIN3.SetText(gloGST3);

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

    // Checked By Sam on 04122017

    

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

        //ctrllVal = validateControl(cbsBCmbPin, 'bpin');
        //checking = checking && ctrllVal;

        //ctrllVal = validateControl(cbsSCmbPin, 'spin');
        //checking = checking && ctrllVal;

        ctrllVal = validateControl(ctxt_BPin, 'bpin');
        checking = checking && ctrllVal;

        ctrllVal = validateControl(ctxt_SPin, 'spin');
        checking = checking && ctrllVal;



        if (ucbsDocType == "TPB" || ucbsDocType == "TSI") {
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

        ctrllVal = validateControl(ctxt_BPin, 'bpin');
        checking = checking && ctrllVal;

        //ctrllVal = validateControl(cbsBCmbPin, 'bpin');
        //checking = checking && ctrllVal;

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

        ctrllVal = validateControl(ctxt_SPin, 'spin');
        checking = checking && ctrllVal;

        //ctrllVal = validateControl(cbsSCmbPin, 'spin');
        //checking = checking && ctrllVal;

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
     
    //var BpreviousBillingAddressID = 0;
    //var BCurrentBillingAddressID = 0;
    //function GetPreviousBillingTypeAddressID(e) {
    //    var BpreviousBillingAddressID = billingLookup.GetGridView().GetRowKey(billingLookup.GetGridView().GetFocusedRowIndex());
    //    alert(BpreviousBillingAddressID);
    //}

    //function GetCurrentBillingTypeAddressID() {
    //    var BCurrentBillingAddressID = billingLookup.GetGridView().GetRowKey(billingLookup.GetGridView().GetFocusedRowIndex());
    //    alert(BCurrentBillingAddressID);
    //}



   
    function GetBillingAddressDetailByAddressId() {
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
                //cbsBCmbPin.SetValue('');
                ctxt_BPin.SetText('');
                $("#hdnBPinID").val('0')

                //cbsBCmbArea.SetValue('');
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

                //cbsSCmbPin.SetValue('');
                ctxt_SPin.SetValue('');
                $("#hdnSPinId").val('0');
                //cbsSCmbArea.SetValue('');
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

                //cbsBCmbPin.SetValue('');
                ctxt_BPin.SetText('');
                $("#hdnBPinID").val('0')

                //cbsBCmbArea.SetValue('');
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

                //cbsSCmbPin.SetValue('');
                ctxt_SPin.SetText('');
                $("#hdnSPinId").val('0');

                //cbsSCmbArea.SetValue('');
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
                <div id="DivBSSection">
                    <asp:HiddenField ID="hdnucSBranchStateCode" runat="server"></asp:HiddenField>
                     <asp:HiddenField ID="hdnucSBranchStateId" runat="server"></asp:HiddenField>
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
                                                                KeyFieldName="add_id" Width="100%" TextFormatString="{0}{1}" AutoGenerateColumns="False" SelectionMode="Single"
                                                                OnDataBinding="BillingAddress_DataBinding">
                                                                <Columns>
                                                                    <%--<dxe:GridViewDataColumn FieldName="add_id" Visible="true" VisibleIndex="0" Caption="Type" Width="0" Settings-AutoFilterCondition="Contains" />--%>
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
                                                                    <%----%>
                                                                </GridViewProperties>
                                                                <ClientSideEvents TextChanged="function(s, e) { GetBillingAddressDetailByAddressId(e)}" />
                                                                <ClearButton DisplayMode="Auto">
                                                                </ClearButton>
                                                            </dxe:ASPxGridLookup>
                                                        </div>
                                                    </div>
                                                    </div>
                                                    <div class="clear"></div>

                                                    <%--<dxe:ASPxCallbackPanel runat="server" ID="BillingPinChangeComponent" ClientInstanceName="cBillingPinChangeComponent" OnCallback="BillingPinChangeComponent_Callback">
                                                                <panelcollection>
	                                                                <dxe:PanelContent runat="server">--%>
                                                    <%--<div class="hide" id="DivHideBilling" >--%>
                                                             <%--start of Pin/Zip.--%>
                                                           <%-- <div class="col-md-4" style="height: auto;">
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
                                                            </div>--%>


                                                            <%-- My new Section Start--%>

                                                         <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                            Pin/Zip(Min 6 digits)<span style="color: red;"> *</span>
                                                        </div>
                                                         <div class="col-md-8">
                                                            <div class="Left_Content relative">
                                                                <dxe:ASPxTextBox ID="txt_BPin" MaxLength="6" ClientInstanceName="ctxt_BPin"
                                                                    runat="server" Width="100%">
                                                                    <ClientSideEvents GotFocus="StoreBPinNo" LostFocus="SetBillingDetails" KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                                </dxe:ASPxTextBox>
                                                                <span id="spinman" style="display: none" class="mandt">
                                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                </span>
                                                            </div>
                                                        </div>
                                                        

                                                        <%--My New Section End--%>

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
                                                            
                                                            

                                                            <dxe:ASPxCallbackPanel runat="server" ID="BillingPinChangeComponent" ClientInstanceName="cBillingPinChangeComponent" OnCallback="BillingPinChangeComponent_Callback">
                                                                <panelcollection>
	                                                                <dxe:PanelContent runat="server">
                                                                        <asp:HiddenField ID="hdnBPinID" runat="server"></asp:HiddenField>
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
                                                                       <%-- <div class="col-md-4" style="height: auto;">
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
                                                                        <div class="clear"></div>--%>

                                                                        <%--Sam Section Modification Start For Area Section Start--%>
                                                                       <%-- <div class="col-md-4" style="height: auto;">
                                                                            <asp:Label ID="Label3" runat="server" Text="Area:" CssClass="newLbl"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-8">
                                                                            <div class="Left_Content relative">
                                                                                 <dxe:ASPxLabel ID="lbl_BArea" runat="server"  ClientInstanceName="clbl_BArea" Width="100%" CssClass="labelClass" 
                                                                                    style="font-weight: bold;font-size: 14px;"></dxe:ASPxLabel>
                                                                                    <asp:HiddenField ID="hdnBArea" runat="server" Value="0" />
                                                                                 
                                                                                <span id="spanBArea" style="display: none" class="mandt">
                                                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                                </span>
                                                                            </div>
                                                                        </div>--%>

                                                                        <%--Sam Section Modification Start For Area Section End--%>
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

                                                    <%--</div>--%>

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
                                                               <%-- <dxe:ASPxComboBox ID="bsSCustomer" ClientInstanceName="cbsSCustomer" runat="server" ValueType="System.String" Width="100%" EnableCallbackMode="true" CallbackPageSize="10" EnableIncrementalFiltering="true">
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { bsSCmbCustomerOnCustomerChanged(s); }"></ClientSideEvents>
                                                                </dxe:ASPxComboBox>--%>
                                                                <dxe:ASPxComboBox ID="bsSCustomer" runat="server" EnableCallbackMode="true" CallbackPageSize="10" Width="98%" 
                                            OnCallback="CustomerComboBox_Callback" ValueType="System.String" ValueField="cnt_internalid" ClientInstanceName="cbsSCustomer"
                                            OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL" OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{1} [{0}]"
                                            DropDownStyle="DropDown" DropDownRows="7">
                                            <Columns>                                              
                                                <dxe:ListBoxColumn FieldName="shortname" Caption="Unique ID" Width="320px" />
                                                <dxe:ListBoxColumn FieldName="Name" Caption="Name" Width="320px" />
                                                <%--<dxe:ListBoxColumn FieldName="Type" Caption="Type" Width="100px" />--%>
                                                <%--<dxe:ListBoxColumn FieldName="cnt_internalid" Caption="Billing Address"  Width="300px" />--%>
                                            </Columns>
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { bsSCmbCustomerOnCustomerChanged(s); }"></ClientSideEvents>
                                            <%--<ClientSideEvents ValueChanged="function(s, e) { $('#DeleteCustomer').val('yes'); GetContactPerson(e)}" GotFocus="function(s,e){gridLookup.ShowDropDown();}" />--%>
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
                                                            KeyFieldName="add_id" Width="100%" TextFormatString="{0}{1}" AutoGenerateColumns="False" SelectionMode="Single"
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

                                                <%--runat="server"--%>

                                               <%-- <div class="hide" id="DivHideShipping"  >--%>

                                                    <%--start of Pin/Zip.--%>
                                                       <%-- <div class="col-md-4" style="height: auto;">
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
                                                        </div>--%>

                                                       <%-- My new Section Start--%>

                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                            Pin/Zip(Min 6 digits)<span style="color: red;"> *</span>
                                                        </div>
                                                        <div class="col-md-8">
                                                            <div class="Left_Content relative">
                                                                <dxe:ASPxTextBox ID="txt_SPin" MaxLength="6" ClientInstanceName="ctxt_SPin"
                                                                    runat="server" Width="100%">
                                                                    <ClientSideEvents GotFocus="StoreSPinNo" LostFocus="SetShippingDetails" KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                                </dxe:ASPxTextBox>
                                                                <span id="spinman" style="display: none" class="mandt">
                                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                </span>
                                                            </div>
                                                        </div>
                                                       

                                                        <%--My New Section End--%>



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
                                                        

                                                        <dxe:ASPxCallbackPanel runat="server" ID="ShippingPinChangeComponent" ClientInstanceName="cShippingPinChangeComponent" OnCallback="ShippingPinChangeComponent_Callback">
	                                                        <panelcollection>
		                                                        <dxe:PanelContent runat="server">
                                                                    
                                                                    <asp:HiddenField ID="hdnSPrevStateId" runat="server"></asp:HiddenField>
                                                                    
                                                                     <asp:HiddenField ID="hdnSPinId" runat="server"></asp:HiddenField>
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
                                                                                    <%--<asp:HiddenField ID="hdShippingState" runat="server" Value="0" />--%>
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
                                                                   <%-- <div class="col-md-4" style="height: auto;">
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
                                                                    </div>--%>
                                                                     <div class="clear"></div>

                                                                     <%--Sam Section Modification Start For Shipping Area Section Start--%>
                                                                        <%-- <div class="col-md-4" style="height: auto;">
                                                                            <asp:Label ID="Label16" runat="server" Text="Area:" CssClass="newLbl"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-8">
                                                                            <div class="Left_Content relative">
                                                                                 <dxe:ASPxLabel ID="lbl_SArea" runat="server"  ClientInstanceName="clbl_SArea" Width="100%" CssClass="labelClass" 
                                                                                    style="font-weight: bold;font-size: 14px;"></dxe:ASPxLabel>
                                                                                    <asp:HiddenField ID="hdnSArea" runat="server" Value="0" />
                                                                                 
                                                                                <span id="spanBArea" style="display: none" class="mandt">
                                                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                                </span>
                                                                            </div>
                                                                        </div>--%>

                                                                        <%--Sam Section Modification Start For Area Section End--%>
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
	                                                        <clientsideevents endcallback="cShippingPinChangeComponent_endcallback" />
                                                        </dxe:ASPxCallbackPanel>

                                               
                                                        <div class="col-md-12" style="height: auto;">
                                                            <dxe:ASPxCheckBox ID="bsSChkShiipingToBilling" runat="server" ClientInstanceName="cbsSChkShiipingToBilling" Text="Billing to be in the same location of Shipping. ">
                                                                <ClientSideEvents CheckedChanged="function(s,e){CopyShippingToBilling(s);}"></ClientSideEvents>
                                                            </dxe:ASPxCheckBox>
                                         
                                                        </div>
                                                <%--</div>--%>

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
                            AutoPostBack="False" Text="O&#818;K" CssClass="btn btn-primary">
                            <ClientSideEvents Click="function (s, e) {fnSaveBillingShipping();}" />
                        </dxe:ASPxButton>

                    </div>
                    <asp:HiddenField ID="hfVendorGSTIN" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hfBSShipToPartyVisibility" runat="server"></asp:HiddenField>
                </div>
            </dxe:PanelContent>
        </panelcollection>
    <clientsideevents endcallback="cbsComponentPanel_endcallback" />
</dxe:ASPxCallbackPanel>
    <asp:SqlDataSource ID="VendorDataSource" runat="server"  />

<%--<asp:HiddenField ID="hfBSShipToPartyVisibility" runat="server"></asp:HiddenField>--%>
<asp:HiddenField ID="hdDefaultAddress" runat="server"  />