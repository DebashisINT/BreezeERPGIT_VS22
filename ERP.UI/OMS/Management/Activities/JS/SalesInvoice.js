/*********************************************************************************************************
 * Rev 1.0      Sanchita      V2.0.37       Tolerance feature required in Sales Order Module 
 *                                          Refer: 25223
 * Rev 2.0      Sanchita      V2.0.38       Base Rate is not recalculated when the Multi UOM is Changed. Mantis : 26320   
 **********************************************************************************************************/

$(document).ready(function () {
    var mode = $('#hdAddOrEdit').val();
    if (mode == 'Edit') {
        if ($("#hdAddOrEdit").val() != "") {
            var VendorID = $("#hdnCustomerId").val();
            SetEntityType(VendorID);
        }
        if ($('#hdnDocumentSegmentSettings').val() == "1") {

            $.ajax({
                type: "POST",
                url: "SalesOrderAdd.aspx/GetSegmentDetails",
                data: JSON.stringify({ CustomerId: VendorID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    OutStandingAmount = msg.d;
                    if (OutStandingAmount != null)
                    {

                    
                        if (OutStandingAmount.Segment1 != "") {
                            var Segment1 = OutStandingAmount.Segment1;
                            var Segment2 = OutStandingAmount.Segment2;
                            var Segment3 = OutStandingAmount.Segment3;
                            var Segment4 = OutStandingAmount.Segment4;
                            var Segment5 = OutStandingAmount.Segment5;

                            if (Segment1 == "0") {
                                var div = document.getElementById('DivSegment1');
                                div.style.display = 'none';
                                $('#hdnValueSegment1').val("0");
                            }
                            else {
                                $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                $('#hdnValueSegment1').val("1");
                            }
                            if (Segment2 == "0") {
                                var div = document.getElementById('DivSegment2');
                                div.style.display = 'none';
                                $('#hdnValueSegment2').val("0");
                            }
                            else {
                                $('#ModuleSegment2Header').text(OutStandingAmount.SegmentName2);
                                $('#hdnValueSegment2').val("1");
                            }

                            if (Segment3 == "0") {
                                var div = document.getElementById('DivSegment3');
                                div.style.display = 'none';
                                $('#hdnValueSegment3').val("0");
                            }
                            else {
                                $('#ModuleSegment3Header').text(OutStandingAmount.SegmentName3);
                                $('#hdnValueSegment3').val("1");
                            }

                            if (Segment4 == "0") {
                                var div = document.getElementById('DivSegment4');
                                div.style.display = 'none';
                                $('#hdnValueSegment4').val("0");
                            }
                            else {
                                $('#ModuleSegment4Header').text(OutStandingAmount.SegmentName4);
                                $('#hdnValueSegment4').val("1");
                            }

                            if (Segment5 == "0") {
                                var div = document.getElementById('DivSegment5');
                                div.style.display = 'none';
                                $('#hdnValueSegment5').val("0");
                            }
                            else {
                                $('#ModuleSegment5Header').text(OutStandingAmount.SegmentName5);
                                $('#hdnValueSegment5').val("1");
                            }

                        }
                    }
                    else{
                            
                        document.getElementById('DivSegment1').style.display = 'none';
                        document.getElementById('DivSegment2').style.display = 'none';
                        document.getElementById('DivSegment3').style.display = 'none';
                        document.getElementById('DivSegment4').style.display = 'none';
                        document.getElementById('DivSegment5').style.display = 'none';
                    }
                }
            });
        }
    }

    if (($("#hddnMultiUOMSelection").val() == "0") && ($("#hdnPageStatus").val() == "update")) {
        aarr = [];
        SelecttedProductEditUomConversion();
    }



    if (!document.getElementById('chkSendMail')) {
        document.getElementById("divMail").style.display = "none";
    }
    else {
        document.getElementById("divMail").style.display = "block";
    }
    
});

//$(function () {
//    $('#UOMModal').on('hide.bs.modal', function () {
//        grid.batchEditApi.StartEdit(globalRowIndex, 6);
//    });
//});

// Rev 2.0
$(function () {
    $(".allownumericwithdecimal").on("keypress keyup blur", function (event) {
        var patt = new RegExp(/[0-9]*[.]{1}[0-9]{4}/i);
        var matchedString = $(this).val().match(patt);
        if (matchedString) {
            $(this).val(matchedString);
        }
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }

    });
});
// End of Rev 2.0

function closeMultiUOM(s, e) {
    e.cancel = false;
    // cPopup_MultiUOM.Hide();
}

function clookup_project_GotFocus() {

    clookup_Project.gridView.Refresh();
    clookup_Project.ShowDropDown();
}

function onViewStockPosition()
{
    cAssignmentPopUp.Show();
}


$(function () {
    $(".allownumericwithdecimal").on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
});

function PopulateMultiUomAltQuantity() {

    var otherdet = {};
    var Quantity = $("#UOMQuantity").val();
    otherdet.Quantity = Quantity;
    var UomId = ccmbUOM.GetValue();
    otherdet.UomId = UomId;
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    otherdet.ProductID = ProductID;
    var AltUomId = ccmbSecondUOM.GetValue();
    otherdet.AltUomId = AltUomId;

    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/GetPackingQuantity",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            if (msg.d.length != 0) {
                var packingQuantity = msg.d[0].packing_quantity;
                var sProduct_quantity = msg.d[0].sProduct_quantity;
            }
            else {
                var packingQuantity = 0;
                var sProduct_quantity = 0;
            }
            var uomfactor = 0
            if (sProduct_quantity != 0 && packingQuantity != 0) {
                uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = $("#UOMQuantity").val();
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

            //$("#AltUOMQuantity").val(calcQuantity);
            // Mantis Issue 24425, 24428
           // cAltUOMQuantity.SetValue(calcQuantity);
            // End of Mantis Issue 24425, 24428
        }
    });
}


function Delete_MultiUom(keyValue, SrlNo, DetailsId) {


    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo + '~' + DetailsId);

}

//Mantis Issue 24425, 24428
function Edit_MultiUom(keyValue, SrlNo) {
    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);
}
// End of Mantis Issue 24425, 24428

function FinalMultiUOM() {
    
    UomLenthCalculation();
    if (Uomlength == 0 || Uomlength < 0) {

        // Mantis Issue 24425, 24428
        //jAlert("Please add Alt. Quantity.");
        jAlert("Please add atleast one Alt. Quantity with Update Row as checked.");
        // End of Mantis Issue 24425, 24428
        return;
    }
    else {
        // Rev 1.0
        var SOQtyCheck = 1;
        var SODoc_ID = grid.GetEditor('ComponentID').GetValue();
        var SODocDetailsID = grid.GetEditor('DetailsId').GetValue();
        var SLNo = grid.GetEditor('SrlNo').GetValue();
        var IsToleranceInSalesOrder = document.getElementById('hdnIsToleranceInSalesOrder').value;

        $.ajax({
            type: "POST",
            url: "SalesInvoice.aspx/CheckSOQty",
            data: JSON.stringify({ SODoc_ID: SODoc_ID, SODocDetailsID: SODocDetailsID, SLNo: SLNo, IsToleranceInSalesOrder: IsToleranceInSalesOrder }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {

                SOQtyCheck = msg.d;

            }
        });

        if (SOQtyCheck == 1) {
        // End of Rev 1.0
            cPopup_MultiUOM.Hide();
            // Mantis Issue 24425, 24428
            var SLNo = grid.GetEditor('SrlNo').GetValue();
            cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~' + SLNo);
            // End of Mantis Issue 24425, 24428
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 11);
            }, 200)
        // Rev 1.0
        }
        else {
            var OrdeMsg = 'Balance Quantity of selected Product from tagged document. <br/>Cannot enter quantity more than balance quantity.';
            jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {
                grid.batchEditApi.StartEdit(globalRowIndex, 7);
            });
            return;
        }
        // End of Rev 1.0
       
    }

}

// Mantis Issue 24425, 24428
function CalcBaseQty() {
   
    //var PackingQtyAlt = Productdetails.split("||@||")[20];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
    //var PackingQty = Productdetails.split("||@||")[22];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
    //var PackingSaleUOM = Productdetails.split("||@||")[25];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)

    // Rev 2.0
    LoadingPanelMultiUOM.Show();
    document.getElementById('lblInfoMsg').innerHTML = "";
    // End of Rev 2.0

    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var PackingQtyAlt = 0;
    var PackingQty = 0;
    var PackingSaleUOM = 0;

    var ProductID = Productdetails.split("||@||")[0];

    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/AutoPopulateAltQuantity",
        data: JSON.stringify({ ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // Rev 2.0
        async: false,
        // End of Rev 2.0
        success: function (msg) {

            if (msg.d.length != 0) {
                PackingQtyAlt = msg.d[0].packing_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                PackingQty = msg.d[0].sProduct_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                PackingSaleUOM = msg.d[0].AltUOMId;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
            }
            else {
                PackingQtyAlt = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                PackingQty = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                PackingSaleUOM = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
            }

            if (PackingQtyAlt == "") {
                PackingQtyAlt = 0
            }
            if (PackingQty == "") {
                PackingQty = 0
            }

            // if Base UOM of product is not same as the Alternate UOM selected from Product Master, then Calculation of Base Quantity will not happen
            if (ccmbSecondUOM.GetValue() != PackingSaleUOM) {
                PackingQtyAlt = 0;
                PackingQty = 0;
            }

            var BaseQty = 0
            if (PackingQtyAlt > 0) {
                var ConvFact = PackingQty / PackingQtyAlt;
                var altQty = cAltUOMQuantity.GetValue();

                if (ConvFact > 0) {
                    var BaseQty = (altQty * ConvFact).toFixed(4);
                    $("#UOMQuantity").val(BaseQty);
                    // Rev 2.0
                    CalcBaseRate();
                    // End of Rev 2.0
                }
            }
            else {
                $("#UOMQuantity").val("0.0000");
                // Rev 2.0
                document.getElementById('lblInfoMsg').innerHTML = "Base Quantity will not get auto calculated since no UOM Conversion details given for the selected Alt. UOM for Product : " + grid.GetEditor('Description').GetText();
                // End of Rev 2.0
            }
        }
    });

    // End of Rev 2.0
    LoadingPanelMultiUOM.Hide();
    // End of Rev 2.0
 }

function CalcBaseRate() {
    var altQty = cAltUOMQuantity.GetValue();
    var altRate = ccmbAltRate.GetValue();
    var baseQty = $("#UOMQuantity").val();


    if (baseQty > 0) {
        var BaseRate = (altQty * altRate) / baseQty;
        ccmbBaseRate.SetValue(BaseRate);
    }
 }
// End of Mantis Issue 24425, 24428


function SaveMultiUOM() {


    //grid.GetEditor('ProductID').GetText().split("||@||")[3];

    // Rev 2.0
    document.getElementById('lblInfoMsg').innerHTML = "";
   
    if ($("#UOMQuantity").val() != 0 || cAltUOMQuantity.GetValue() != 0) {
        LoadingPanelMultiUOM.Show();
        setTimeout(() => {
            LoadingPanelMultiUOM.Hide();

        }, 1000)
    }
    // End of Rev 2.0

    var qnty = $("#UOMQuantity").val();


    var UomId = ccmbUOM.GetValue();
    //var UomId = ccmbUOM.SetSelectedIndex(grid.GetEditor('ProductID').GetText().split("||@||")[3] - 1);
    var UomName = ccmbUOM.GetText();
    //var AltQnty = parseFloat($("#AltUOMQuantity").val()).toFixed(4);
    var AltQnty = cAltUOMQuantity.GetValue();
    var AltUomId = ccmbSecondUOM.GetValue();
    var AltUomName = ccmbSecondUOM.GetText();
    var srlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];

    var DetailsId ="";
     DetailsId = grid.GetEditor('DetailsId').GetText();


    var DeliveryScheduleDetailsID = (grid.GetEditor('DeliveryScheduleDetailsID').GetText() != null) ? grid.GetEditor('DeliveryScheduleDetailsID').GetText() : "0";
    // Mantis Issue 24425, 24428
    if ($("#hdAddOrEdit").val() == "Add") {
        if (DeliveryScheduleDetailsID != null && DeliveryScheduleDetailsID != "0" && DeliveryScheduleDetailsID != "") 
            {
                DetailsId=DeliveryScheduleDetailsID;
            }
        }

    var BaseRate = ccmbBaseRate.GetValue();
    var AltRate = ccmbAltRate.GetValue();

    var UpdateRow = 'False';
    if ($("#chkUpdateRow").prop("checked")) {
        UpdateRow = 'True';
    }
    // End of Mantis Issue 24425, 24428

    // Mantis Issue 24425, 24428
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "") {
    if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty!="0.0000") {
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
        // End of Mantis Issue 24425, 24428

        // Mantis Issue 24425, 24428
        //cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId);
        if (cbtnMUltiUOM.GetText() == "Update") {
            cgrid_MultiUOM.PerformCallback('UpdateRow~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow + '~' + hdMultiUOMID);
            //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
            cAltUOMQuantity.SetValue("0.0000");
            // Mantis Issue 24428
            $("#UOMQuantity").val(0);
            ccmbBaseRate.SetValue(0);
            cAltUOMQuantity.SetValue(0);
            ccmbAltRate.SetValue(0);
            ccmbSecondUOM.SetValue("");
            cgrid_MultiUOM.cpAllDetails = "";
            cbtnMUltiUOM.SetText("Add");
            // Rev Sanchita
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
            // End of Rev Sanchita
        }
        else {
            cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow);
            // End of Mantis Issue 24425, 24428

            //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
            cAltUOMQuantity.SetValue("0.0000");
            // Mantis Issue 24425, 24428
            $("#UOMQuantity").val(0);
            ccmbBaseRate.SetValue(0)
            cAltUOMQuantity.SetValue(0)
            ccmbAltRate.SetValue(0)
            ccmbSecondUOM.SetValue("")
            // Rev Sanchita
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
            // End of Rev Sanchita
        }
            // End of Mantis Issue 24425, 24428
            // Rev Sanchita
        }
        else {
            return;
        }
        // End of Rev Sanchita
    }
    else {
        return;
    }
}


function fn_PopOpen() {
    var url = '/OMS/management/Store/Master/ProductPopup.html?var=21.60';
    cPosView.SetContentUrl(url);
    cPosView.RefreshContentUrl();

    cPosView.Show();

}

function fn_productSave() {
    cPosView.Hide();
}
function LoadOldSelectedKeyvalue() {
    var x = gridquotationLookup.gridView.GetSelectedKeysOnPage();
    var Ids = "";
    for (var i = 0; i < x.length; i++) {
        Ids = Ids + ',' + x[i];
    }
    document.getElementById('OldSelectedKeyvalue').value = Ids;
}


function GlobalBillingShippingEndCallBack() {
    if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
        cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";

        var inventory = '';
        var isinventory = $('#ddlInventory').val()

        if (isinventory == 'Y')
            inventory = 1
        else

            inventory = 0

        if (gridquotationLookup.GetValue() != null) {

            var key = GetObjectID('hdnCustomerId').value;
            if (key != null && key != '') {
                cContactPerson.PerformCallback('BindContactPerson~' + key);
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                var startDate = new Date();
                startDate = tstartdate.GetValueString();




                if (type != "") {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%' + '~' + type + '~' + inventory + '~' + isinventory);
                }

                if (componentType != null && componentType != '') {
                    grid.PerformCallback('GridBlank');
                }
            }
        }
        else {
            var key = GetObjectID('hdnCustomerId').value;
            if (key != null && key != '') {
                cContactPerson.PerformCallback('BindContactPerson~' + key);
                page.GetTabByName('Billing/Shipping').SetEnabled(true);

                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                var startDate = new Date();
                startDate = tstartdate.GetValueString();

                if (type != "") {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%' + '~' + type + '~' + inventory + '~' + isinventory);
                }

                var componentType = gridquotationLookup.GetValue();
                if (componentType != null && componentType != '') {
                    grid.PerformCallback('GridBlank');
                }
            }
        }
    }
}
//Start Chinmoy 
//Rev Rajdip
function TaxDeleteForShipPartyChange() {
    // var UniqueVal = $("#hdnGuid").val();
    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/DeleteTaxForShipPartyChange",
        //data: JSON.stringify({ UniqueVal: UniqueVal }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            // RequiredShipToPartyValue = msg.d;
        }
    });
}
//End Rev Rajdip
function AfterSaveBillingShipiing(validate) {

    if ($("#Keyval_internalId").val() == "Add") {
        GetPosForGstValue();
    }
    if (validate) {
        page.SetActiveTabIndex(0);
        page.tabs[0].SetEnabled(true);
        $("#divcross").show();

    }
    else {
        page.SetActiveTabIndex(1);
        page.tabs[0].SetEnabled(false);
        $("#divcross").hide();
    }
}

//End

function GetPosForGstValue() {
    cddl_PosGst.ClearItems();
    if (cddl_PosGst.GetItemCount() == 0) {
        cddl_PosGst.AddItem(GetShippingStateName() + '[Shipping]', "S");
        cddl_PosGst.AddItem(GetBillingStateName() + '[Billing]', "B");
    }
    else if (cddl_PosGst.GetItemCount() > 2) {
        cddl_PosGst.ClearItems();
        //cddl_PosGst.RemoveItem(0);
        //cddl_PosGst.RemoveItem(0);
    }

    if (PosGstId == "" || PosGstId == null) {
        cddl_PosGst.SetValue("S");
    }
    else {
        cddl_PosGst.SetValue(PosGstId);
    }
}




//Chinmoy Added Below Code
//Start
var Address = [];
var ReturnDetails;
function GetDocumentAddress(OrderId, TagDocType) {

    var OtherDetail = {};

    OtherDetail.OrderId = OrderId;
    OtherDetail.TagDocType = TagDocType;


    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "SalesInvoice.aspx/SaveDocumentAddress",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                Address = msg.d;
                PopulateBillingShippingAddress(Address);

            }
        });
    }
}

function PopulateBillingShippingAddress(ReturnDetails) {

    var BillingDetails = $.grep(ReturnDetails, function (e) { return e.Type == "Billing" })
    var ShippingDetails = $.grep(ReturnDetails, function (e) { return e.Type == "Shipping" })

    //Billing Address Details
    if (BillingDetails.length > 0) {
        ctxtAddress1.SetText(BillingDetails[0].Address1);
        ctxtAddress2.SetText(BillingDetails[0].Address2);
        ctxtAddress3.SetText(BillingDetails[0].Address3);
        ctxtlandmark.SetText(BillingDetails[0].Landmark);
        ctxtbillingPin.SetText(BillingDetails[0].PinCode);
        $('#hdBillingPin').val(BillingDetails[0].PinId);
        ctxtbillingCountry.SetText(BillingDetails[0].CountryName);
        $('#hdCountryIdBilling').val(BillingDetails[0].CountryId);
        ctxtbillingState.SetText(BillingDetails[0].StateName);
        $('#hdStateCodeBilling').val(BillingDetails[0].StateCode);
        $('#hdStateIdBilling').val(BillingDetails[0].StateId);
        ctxtbillingCity.SetText(BillingDetails[0].CityName);
        $('#hdCityIdBilling').val(BillingDetails[0].CityId);
        ctxtSelectBillingArea.SetText(BillingDetails[0].AreaName);
        $('#hdAreaIdBilling').val(BillingDetails[0].AreaId);
        ctxtDistance.SetText(BillingDetails[0].Distance);

        var GSTIN = BillingDetails[0].GSTIN;
        var GSTIN1 = GSTIN.substring(0, 2);
        var GSTIN2 = GSTIN.substring(2, 12);
        var GSTIN3 = GSTIN.substring(12, 15);

        ctxtBillingGSTIN1.SetText(GSTIN1);
        ctxtBillingGSTIN2.SetText(GSTIN2);
        ctxtBillingGSTIN3.SetText(GSTIN3);
        GetPosForGstValue();
        cddl_PosGst.SetValue(BillingDetails[0].PosForGst);
        // clookup_Project.gridView.SelectItemsByKey(BillingDetails[0].ProjectCode);
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
        //chinmoy commeneted
        // GetPosForGstValue();
        // cddl_PosGst.SetText('');
    }

    //Shipping Address Details
    if (ShippingDetails.length > 0) {
        ctxtsAddress1.SetText(ShippingDetails[0].Address1);
        ctxtsAddress2.SetText(ShippingDetails[0].Address2);
        ctxtsAddress3.SetText(ShippingDetails[0].Address3);
        ctxtslandmark.SetText(ShippingDetails[0].Landmark);
        ctxtShippingPin.SetText(ShippingDetails[0].PinCode);
        $('#hdShippingPin').val(ShippingDetails[0].PinId);
        ctxtshippingCountry.SetText(ShippingDetails[0].CountryName);
        $('#hdCountryIdShipping').val(ShippingDetails[0].CountryId);
        ctxtshippingState.SetText(ShippingDetails[0].StateName);
        $('#hdStateCodeShipping').val(ShippingDetails[0].StateCode);
        $('#hdStateIdShipping').val(ShippingDetails[0].StateId);
        ctxtshippingCity.SetText(ShippingDetails[0].CityName);
        $('#hdCityIdShipping').val(ShippingDetails[0].CityId);
        ctxtSelectShippingArea.SetText(ShippingDetails[0].AreaName);
        $('#hdAreaIdShipping').val(ShippingDetails[0].AreaId);
        ctxtDistanceShipping.SetText(ShippingDetails[0].Distance);

        var GSTIN = ShippingDetails[0].GSTIN;
        var GSTIN1 = GSTIN.substring(0, 2);
        var GSTIN2 = GSTIN.substring(2, 12);
        var GSTIN3 = GSTIN.substring(12, 15);


        ctxtShippingGSTIN1.SetText(GSTIN1);
        ctxtShippingGSTIN2.SetText(GSTIN2);
        ctxtShippingGSTIN3.SetText(GSTIN3);
        ctxtShipToPartyShippingAdd.SetText(ShippingDetails[0].ShipToPartyName);
        $('#hdShipToParty').val(ShippingDetails[0].ShipToPartyId);
        GetPosForGstValue();
        cddl_PosGst.SetValue(ShippingDetails[0].PosForGst);
        //clookup_Project.gridView.SelectItemsByKey(ShippingDetails[0].ProjectCode);
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
        ctxtShipToPartyShippingAdd.SetText('');
        $('#hdShipToParty').val('');
        //chinmoy commented
        // GetPosForGstValue();
        // cddl_PosGst.SetText('');

    }


}
//End
(function (global) {

    if (typeof (global) === "undefined") {
        throw new Error("window is undefined");
    }

    var _hash = "!";
    var noBackPlease = function () {
        global.location.href += "#";

        // making sure we have the fruit available for juice (^__^)
        global.setTimeout(function () {
            global.location.href += "!";
        }, 50);
    };

    global.onhashchange = function () {
        if (global.location.hash !== _hash) {
            global.location.hash = _hash;
        }
    };

    global.onload = function () {
        noBackPlease();

        // disables backspace on page except on input fields and textarea..
        document.body.onkeydown = function (e) {
            var elm = e.target.nodeName.toLowerCase();
            if (e.which === 8 && (elm !== 'input' && elm !== 'textarea')) {
                e.preventDefault();
            }
            // stopping event bubbling up the DOM tree..
            e.stopPropagation();
        };
    }

})(window);

var isCtrl = false;
//document.onkeyup = function (e) {
//    if (event.keyCode == 17) {
//        isCtrl = false;
//    }
//    else if (event.keyCode == 27) {
//        btnCancel_Click();
//    }
//}

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

document.onkeydown = function (e) {
    if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + n -- ie, Save & New  
        StopDefaultAction(e);
        if (document.getElementById('btn_SaveRecords').style.display != 'none') {
            Save_ButtonClick();
        }
    }
    else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Ctrl+X -- ie, Save & Exit!     
        StopDefaultAction(e);
        if (document.getElementById('ASPxButton1').style.display != 'none') {
            SaveExit_ButtonClick();
        }
    }
    else if (event.keyCode == 79 && event.altKey == true && getUrlVars().req != "V") { //run code for alt + o -- ie, Save & Exit!     
        StopDefaultAction(e);
        if (page.GetActiveTabIndex() == 1) {
            fnSaveBillingShipping();
        }
    }
    else if (event.keyCode == 77 && event.altKey == true && getUrlVars().req != "V") { //run code for alt + m -- ie, TC
        $('#TermsConditionseModal').modal({
            show: 'true'
        });
    }
    else if (event.keyCode == 69 && event.altKey == true && getUrlVars().req != "V") { //run code for alt + e -- ie, TC
        if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
            StopDefaultAction(e);
            SaveTermsConditionData();
        }
    }
    else if (event.keyCode == 76 && event.altKey == true && getUrlVars().req != "V") { //run code for alt + l -- ie, TC
        StopDefaultAction(e);
        calcelbuttonclick();
    }
    else {
        //do nothing
    }
}

//transporter
document.onkeyup = function (e) {
    if (event.altKey == true && getUrlVars().req != "V") {
        switch (event.keyCode) {
            case 83:
                if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                    SaveVehicleControlData();
                }
                break;
            case 67:
                modalShowHide(0);
                break;
            case 82:
                modalShowHide(1);
                $('body').on('shown.bs.modal', '#exampleModal', function () {
                    $('input:visible:enabled:first', this).focus();
                })
                break;
                //case 78:
                //    StopDefaultAction(e);
                //    Save_ButtonClick();
                //    break;
                //case 88:
                //    StopDefaultAction(e);
                //    SaveExit_ButtonClick();
                //    break;
            case 120:
                StopDefaultAction(e);
                SaveExit_ButtonClick();
                break;
            case 84:
                StopDefaultAction(e);
                Save_TaxesClick();
                break;
            case 85:
                OpenUdf();
                break;
        }
    }
}

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}



function RecalCulateTaxTotalAmountInline() {
    var totalInlineTaxAmount = 0;
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            totalInlineTaxAmount = (DecimalRoundoff(totalInlineTaxAmount, 2) + DecimalRoundoff(cgridTax.GetEditor("Amount").GetValue(), 2)).toFixed(2);
        } else {
            totalInlineTaxAmount = (DecimalRoundoff(totalInlineTaxAmount, 2) - DecimalRoundoff(cgridTax.GetEditor("Amount").GetValue(), 2)).toFixed(2);
        }

        cgridTax.batchEditApi.EndEdit();
    }

    totalInlineTaxAmount = DecimalRoundoff(DecimalRoundoff(totalInlineTaxAmount, 2) + DecimalRoundoff(ctxtGstCstVat.GetValue(), 2), 2);

    ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
    // ctxtTaxTotAmt.SetValue(50);
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

    //var ProductIDColumn = s.GetColumnByField("ProductID");
    //if (!e.rowValues.hasOwnProperty(ProductIDColumn.index))
    //    return;
    //var cellInfo = e.rowValues[ProductIDColumn.index];

    //if (cCmbProduct.FindItemByValue(cellInfo.value) != null) {
    //    cCmbProduct.SetValue(cellInfo.value);
    //}
    //else {
    //    cCmbProduct.SetSelectedIndex(-1);
    //}
}

function OnBatchEditEndEditing(s, e) {
    var ProductIDColumn = s.GetColumnByField("ProductID");
    if (!e.rowValues.hasOwnProperty(ProductIDColumn.index))
        return;
    var cellInfo = e.rowValues[ProductIDColumn.index];
    if (cCmbProduct.GetSelectedIndex() > -1 || cellInfo.text != cCmbProduct.GetText()) {
        cellInfo.value = cCmbProduct.GetValue();
        cellInfo.text = cCmbProduct.GetText();
        cCmbProduct.SetValue(null);
    }
}

function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

var taxAmountGlobal;
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


    // SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);

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
function Onddl_VatGstCstEndCallback(s, e) {
    if (s.GetItemCount() == 1) {
        cddlVatGstCst.SetEnabled(false);
    }
}
var GlobalCurChargeTaxAmt;
var ChargegstcstvatGlobalName;
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

var GlobalCurTaxAmt = 0;
var rowEditCtrl;
var globalRowIndex;
var globalTaxRowIndex;
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
            ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt));
        }


        var isValid = taxValue.indexOf('~');
        if (isValid != -1) {
            var rate = parseFloat(taxValue.split('~')[1]);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
            ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt));
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

            ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
            GlobalCurTaxAmt = 0;
        } else {
            s.SetText("");
        }
    }

}
function SetOtherTaxValueOnRespectiveRow(idx, amt, name, runninTot, signCal) {
    for (var i = 0; i < taxJson.length; i++) {
        if (taxJson[i].applicableBy == name) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            var totCal = 0;
            if (signCal == '(+)') {
                totCal = parseFloat(parseFloat(amt) + parseFloat(runninTot));
            }
            else {
                totCal = parseFloat(parseFloat(runninTot) - parseFloat(amt));
            }
            cgridTax.GetEditor('calCulatedOn').SetValue(totCal);

            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var s = cgridTax.GetEditor("TaxField");
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue()) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
                GlobalCurTaxAmt = 0;
            }
            else {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (s.GetText() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
                GlobalCurTaxAmt = 0;
            }
        }
    }
    //return;
    cgridTax.batchEditApi.EndEdit();

}
//function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
//    for (var i = 0; i < taxJson.length; i++) {
//        if (taxJson[i].applicableBy == name) {
//            cgridTax.batchEditApi.StartEdit(i, 3);
//            cgridTax.GetEditor('calCulatedOn').SetValue(amt);

//            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
//            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
//            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
//            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
//            var s = cgridTax.GetEditor("TaxField");
//            if (sign == '(+)') {
//                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
//                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

//                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
//                GlobalCurTaxAmt = 0;
//            }
//            else {

//                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
//                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

//                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
//                GlobalCurTaxAmt = 0;
//            }




//        }
//    }
//    //return;
//    cgridTax.batchEditApi.EndEdit();

//}



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

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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
                cgridTax.GetEditor("Amount").SetValue(DecimalRoundoff(parseFloat(ProdAmt * s.GetText()) / 100, 2));

                //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt), 2);
                ctxtTaxTotAmt.SetValue(DecimalRoundoff(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt), 2);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                //cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                cgridTax.GetEditor("Amount").SetValue(Math.round((parseFloat(ProdAmt * s.GetText()) / 100) * -1), 2);
                //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                // ctxtTaxTotAmt.SetValue(50);
                ctxtTaxTotAmt.SetValue(DecimalRoundoff(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)), 2);
                GlobalCurTaxAmt = 0;
            }
            //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);

            //Call for Running Total
            SetRunningTotal();

        } else {
            s.SetText("");
        }
    }

    RecalCulateTaxTotalAmountInline();
}
function SetRunningTotal() {
    //

    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        if (taxJson[i].applicableOn == "R") {
            cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();

            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var thisRunningAmt = 0;
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (cgridTax.GetEditor("TaxField").GetValue() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));

                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (cgridTax.GetEditor("TaxField").GetValue() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
                GlobalCurTaxAmt = 0;
            }
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), ProdAmt, sign);
        }
        if (sign == '(+)') {
            runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }
        else {
            runningTot = runningTot - parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }

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



var gstcstvatGlobalName;
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}


function txtTax_TextChanged(s, i, e) {
    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
}

function taxAmtButnClick(s, e) {
    if (e.buttonIndex == 0) {

        if (cddl_AmountAre.GetValue() != null) {
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

            if (ProductID.trim() != "") {

                document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                ctxtTaxTotAmt.SetValue(0);
                ccmbGstCstVat.SetSelectedIndex(0);
                $('.RecalculateInline').hide();
                caspxTaxpopUp.Show();
                //Set Product Gross Amount and Net Amount

                var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
                document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                //var strRate = "1";
                var strStkUOM = SpliteDetails[4];
                // var strSalePrice = SpliteDetails[6];
                var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                if (strRate == 0) {
                    strRate = 1;
                }

                //var StockQuantity = strMultiplier * QuantityValue;
                //var Amount = parseFloat((Math.round((QuantityValue * strFactor * (strSalePrice / strRate)) * 100).toFixed(2)) / 100);
                //clblTaxProdGrossAmt.SetText(Amount);
                //clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue() * 100).toFixed(2) / 100);
                //document.getElementById('HdProdGrossAmt').value = Amount;
                //document.getElementById('HdProdNetAmt').value = parseFloat(Math.round(grid.GetEditor('Amount').GetValue() * 100).toFixed(2) / 100);

                var StockQuantity = strMultiplier * QuantityValue;
                //var Amount = parseFloat((Math.round((QuantityValue * strFactor * (strSalePrice / strRate)) * 100).toFixed(2)) / 100);
                var Amount = 0.00;
                var TaxType = "";
                if (cddl_AmountAre.GetValue() == "1") {
                    Amount = parseFloat((Math.round((QuantityValue * strFactor * (strSalePrice / strRate)) * 100).toFixed(2)) / 100);
                }
                else if (cddl_AmountAre.GetValue() == "2") {
                    Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "";
                }

                var IsDiscountPercentage = document.getElementById('IsDiscountPercentage').value;
                var amountAfterDiscount = "0";

                if (IsDiscountPercentage == "Y") {
                    amountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                }
                else {
                    amountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
                }

                var _NetAmt = "0";
                var _GrossAmt = "0";

                var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";
                if (inventoryType == "S" && parseFloat(QuantityValue) == 0) {
                    var _TotalAmount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                    _NetAmt = _TotalAmount;
                    _GrossAmt = _TotalAmount;
                }
                else {
                    _NetAmt = parseFloat(amountAfterDiscount);
                    _GrossAmt = parseFloat(Amount);
                }

                clblTaxProdGrossAmt.SetText(_GrossAmt);
                clblProdNetAmt.SetText(_NetAmt);
                document.getElementById('HdProdGrossAmt').value = _GrossAmt;
                document.getElementById('HdProdNetAmt').value = _NetAmt;


                //End Here

                //Set Discount Here
                if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                    //var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
                    //clblTaxDiscount.SetText(discount);

                    var discount = ((Math.round((Math.abs(parseFloat(Amount) - parseFloat(_GrossAmt))) * 100).toFixed(2)) / 100);
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
                                clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2));
                                document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2);
                                clblGstForNet.SetText(Math.round(grid.GetEditor('Amount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
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

                    var shippingStCode = '';

                    ////###### Added By : Samrat Roy ##########
                    //Get Customer Shipping StateCode
                    //var shippingStCode = '';

                    var shippingStCode = '';
                    //Chinmoy Edited below Line
                    if (cddl_PosGst.GetValue() == "S") {
                        shippingStCode = GeteShippingStateCode();
                    }
                    else {
                        shippingStCode = GetBillingStateCode();
                    }
                    // shippingStCode = ctxtshippingState.GetText();
                    //shippingStCode = cbsSCmbState.GetText();
                    shippingStCode = shippingStCode;

                    ////// ###########  Old Code #####################
                    ////if (cchkBilling.GetValue()) {
                    ////    shippingStCode = CmbState.GetText();
                    ////}
                    ////else {
                    ////    shippingStCode = CmbState1.GetText();
                    ////}
                    ////shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                    ////###### END : Samrat Roy : END ########## 

                    //Debjyoti 09032017
                    if (shippingStCode.trim() != '') {
                        for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                            //Check if gstin is blank then delete all tax
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {

                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {

                                    //if its state is union territories then only UTGST will apply
                                    if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "35" || shippingStCode == "31" || shippingStCode == "34") {
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
                    cgridTax.cpComboCode = grid.GetEditor('ProductID').GetValue().split('||@||')[9];
                }

                ctxtprodBasicAmt.SetValue(grid.GetEditor('Amount').GetValue());
            } else {
                grid.batchEditApi.StartEdit(globalRowIndex, 13);
            }
        }
    }
}
function taxAmtButnClick1(s, e) {
    console.log(grid.GetFocusedRowIndex());
    rowEditCtrl = s;
}

function BatchUpdate() {

    cgridTax.batchEditApi.StartEdit(globalTaxRowIndex);

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

var taxJson;
function cgridTax_EndCallBack(s, e) {

    cgridTax.batchEditApi.StartEdit(globalTaxRowIndex);
    $('.cgridTaxClass').show();

    cgridTax.StartEditRow(0);


    //check Json data
    if (cgridTax.cpJsonData) {
        if (cgridTax.cpJsonData != "") {
            taxJson = JSON.parse(cgridTax.cpJsonData);
            cgridTax.cpJsonData = null;
            SetRunningTotal();
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
        grid.GetEditor("TaxAmount").SetValue(totAmt);
        // grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));
        // var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue());
        var totalNetAmount = (DecimalRoundoff(totAmt, 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2)).toFixed(2);

        var totalRoundOffAmount = totalNetAmount;
        grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(totAmt, 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2));

        if (cddl_AmountAre.GetValue() == "2") {
            grid.GetEditor("Amount").SetValue(parseFloat(grid.GetEditor("Amount").GetValue()) + (totalRoundOffAmount - totalNetAmount));
            var finalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
            cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
        }

    }

    if (cgridTax.GetVisibleRowsOnPage() == 0) {
        $('.cgridTaxClass').hide();
        ccmbGstCstVat.Focus();
    }
    //Debjyoti Check where any Gst Present or not
    // If Not then hide the hole section

    //SetRunningTotal();
    ShowTaxPopUp("IY");
    RecalCulateTaxTotalAmountInline();
    SetTotalTaxableAmount(globalRowIndex, 8);


}

function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}


$(document).ready(function () {
    cProductsPopup.Hide();
    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
        page.GetTabByName('Billing/Shipping').SetEnabled(false);
    }
    $('#ApprovalCross').click(function () {

        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.Refresh()();
    })
})


function UniqueCodeCheck() {

    var QuoteNo = ctxt_PLQuoteNo.GetText();
    if (QuoteNo != '') {
        var CheckUniqueCode = false;
        $.ajax({
            type: "POST",
            url: "SalesInvoice.aspx/CheckUniqueCode",
            data: JSON.stringify({ QuoteNo: QuoteNo }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                CheckUniqueCode = msg.d;
                if (CheckUniqueCode == true) {
                    //jAlert('Please enter unique PI/Quotation number');
                    $('#duplicateQuoteno').attr('style', 'display:block');
                    ctxt_PLQuoteNo.SetValue('');
                    ctxt_PLQuoteNo.Focus();
                }
                else {
                    $('#duplicateQuoteno').attr('style', 'display:none');
                }
            }
        });
    }
}

function GetContactPerson(e) {
    if (gridquotationLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

            if (r == true) {
                var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                if (key != null && key != '') {

                    //cContactPerson.PerformCallback('BindContactPerson~' + key);
                    page.GetTabByName('Billing/Shipping').SetEnabled(true);

                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    var startDate = new Date();
                    startDate = tstartdate.GetValueString();

                    if (type != "") {
                        //cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%' + '~' + type);
                    }

                    var componentType = gridquotationLookup.GetValue();////gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
                    if (componentType != null && componentType != '') {
                        //grid.PerformCallback('GridBlank');
                        $('#hdnPageStatus').val('update');
                    }

                    ////###### Added By : Samrat Roy ##########
                    ////cchkBilling.SetChecked(false);
                    ////cchkShipping.SetChecked(false);
                    ////page.SetActiveTabIndex(1);
                    ////$('.dxeErrorCellSys').addClass('abc');
                    ////$('.crossBtn').hide();
                    ////page.GetTabByName('General').SetEnabled(false);
                    //Edited By Chinmoy Below Line
                    SetDefaultBillingShippingAddress(key);
                    // LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SI');
                    GetObjectID('hdnCustomerId').value = key;
                    if ($('#hfBSAlertFlag').val() == "1") {
                        jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                            if (r == true) {
                                page.SetActiveTabIndex(1);
                                //Chinmoy Edited Below Line
                                cbtnSave_SalesBillingShiping.Focus();
                                //cbsSave_BillingShipping.Focus();
                                page.tabs[0].SetEnabled(false);
                                $("#divcross").hide();
                            }
                        });
                    }
                    else {
                        page.SetActiveTabIndex(1);
                        //Chinmoy Edited Below Line
                        cbtnSave_SalesBillingShiping.Focus();
                        //cbsSave_BillingShipping.Focus();
                        page.tabs[0].SetEnabled(false);
                        $("#divcross").hide();
                    }
                    ////###### END : Samrat Roy : END ########## 


                    GetObjectID('hdnAddressDtl').value = '0';
                }
            }
        });
    }
    else {
        var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
        if (key != null && key != '') {

            // cContactPerson.PerformCallback('BindContactPerson~' + key);
            page.GetTabByName('Billing/Shipping').SetEnabled(true);

            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
            var startDate = new Date();
            startDate = tstartdate.GetValueString();

            if (type != "") {
                //cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%' + '~' + type);
            }

            var componentType = gridquotationLookup.GetValue();////gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
            if (componentType != null && componentType != '') {
                //grid.PerformCallback('GridBlank');
                $('#hdnPageStatus').val('update');
            }

            ////###### Added By : Samrat Roy ##########
            ////cchkBilling.SetChecked(false);
            ////cchkShipping.SetChecked(false);
            ////page.SetActiveTabIndex(1);
            ////$('.dxeErrorCellSys').addClass('abc');
            ////$('.crossBtn').hide();
            ////page.GetTabByName('General').SetEnabled(false);
            // Chinmoy Below line

            SetDefaultBillingShippingAddress(key);
            //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SI');
            GetObjectID('hdnCustomerId').value = key;
            if ($('#hfBSAlertFlag').val() == "1") {
                jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        page.SetActiveTabIndex(1);
                        //Chinmoy Edited Below Line
                        cbtnSave_SalesBillingShiping.Focus();
                        //cbsSave_BillingShipping.Focus();
                        page.tabs[0].SetEnabled(false);
                        $("#divcross").hide();
                    }
                });
            }
            else {
                page.SetActiveTabIndex(1);
                //Chinmoy Edited Below Line
                cbtnSave_SalesBillingShiping.Focus();
                // cbsSave_BillingShipping.Focus();
                page.tabs[0].SetEnabled(false);
                $("#divcross").hide();
            }
            ////###### END : Samrat Roy : END ########## 
        }
    }
}
$(document).ready(function () {
    var schemaid = $('#ddl_numberingScheme').val();
    //LoadingPanel.Show();
    if (schemaid != null) {
        if (schemaid == '') {
            ctxt_PLQuoteNo.SetEnabled(false);
        }
    }
    $('#ddlInventory').change(function () {
        var _txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Product Code</th><th>Product Name</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
        document.getElementById("ProductTable").innerHTML = _txt;
    });
    $('#ddl_numberingScheme').change(function () {
        var NoSchemeTypedtl = $(this).val();
        var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
        var quotelength = NoSchemeTypedtl.toString().split('~')[2];

        var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
        if (branchID != "") document.getElementById('ddl_Branch').value = branchID;


        //Cut Off  Valid from To Date Sudip

        var urlKeys = getUrlVars();

        if (urlKeys.Draft_Invoice == undefined) {
            var fromdate = NoSchemeTypedtl.toString().split('~')[5];
            var todate = NoSchemeTypedtl.toString().split('~')[6];
            //alert(fromdate + '   ' + todate);
            var dt = new Date();
            tstartdate.SetDate(dt);

            if (dt < new Date(fromdate)) {
                tstartdate.SetDate(new Date(fromdate));
            }

            if (dt > new Date(todate)) {
                tstartdate.SetDate(new Date(todate));
            }
            tstartdate.SetMinDate(new Date(fromdate));
            tstartdate.SetMaxDate(new Date(todate));
        }
       
        //Cut Off  Valid from To Date Sudip

        // cddlCashBank.PerformCallback();

        //ctxt_PLQuoteNo.SetMaxLength(quotelength);
        if (NoSchemeType == '1') {
            ctxt_PLQuoteNo.SetText('Auto');
            ctxt_PLQuoteNo.SetEnabled(false);

            // tstartdate.SetEnabled(false);
            tstartdate.Focus();
        }
        else if (NoSchemeType == '0') {
            ctxt_PLQuoteNo.SetEnabled(true);
            ctxt_PLQuoteNo.GetInputElement().maxLength = quotelength;
            //ctxt_PLQuoteNo.SetClientEnabled(true);
            ctxt_PLQuoteNo.SetText('');
            ctxt_PLQuoteNo.Focus();
        }
        else if (NoSchemeType == '2') {
            ctxt_PLQuoteNo.SetText('Datewise');
            ctxt_PLQuoteNo.SetEnabled(false);
            //ctxt_PLQuoteNo.SetClientEnabled(false);

            tstartdate.Focus();
        }
        else {
            ctxt_PLQuoteNo.SetText('');
            ctxt_PLQuoteNo.SetEnabled(false);
            //ctxt_PLQuoteNo.SetClientEnabled(true);
        }
        //Chinmoy 18-04-2019
        //Start
        //Rev Sayantani
        //if (ctxt_PLQuoteNo.GetText() == "Auto") {
        //    tstartdate.SetEnabled(false);
        //}
        //else {
        //    tstartdate.SetEnabled(true);
        //}
        if (ctxt_PLQuoteNo.GetText() == "Auto" && $("#ISAllowBackdatedEntry").val() == "Yes") {
            tstartdate.SetEnabled(true);
        }
        else if (ctxt_PLQuoteNo.GetText() == "Auto" && $("#ISAllowBackdatedEntry").val() == "No") {
            tstartdate.SetEnabled(false);
        }
        else if (ctxt_PLQuoteNo.GetText() != "Auto" && $("#ISAllowBackdatedEntry").val() == "Yes") {
            tstartdate.SetEnabled(true);
        }
        else if (ctxt_PLQuoteNo.GetText() != "Auto" && $("#ISAllowBackdatedEntry").val() == "No") {
            tstartdate.SetEnabled(true);
        }
        //End of Rev Sayantani
        //clookup_Project.gridView.Refresh();
        //End


        $.ajax({
            type: "POST",
            url: "SalesInvoice.aspx/GetEinvoiceBranch",
            data: JSON.stringify({ BranchId: branchID }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                $("#hdnIsBranchEInvoice").val(msg.d);
                if (msg.d == "True") {
                    if (($("#drdTransCategory").val() == "B2B") || ($("#drdTransCategory").val() == "SEZWP") || ($("#drdTransCategory").val() == "SEZWOP") || ($("#drdTransCategory").val() == "EXPWP") || ($("#drdTransCategory").val() == "EXPWOP") || ($("#drdTransCategory").val() == "DEXP")) {
                        $("#chkSendMail").prop("checked", false);
                        $("#chkSendMail").prop("disabled", true);
                    }
                    else {
                        $("#chkSendMail").prop("disabled", false);
                    }
                }
                else {
                    $("#chkSendMail").prop("disabled", false);
                }
            },
            error: function (msg) {
            }
        });
        LoadBillDespatch(document.getElementById('ddl_Branch').value);
    });


});

function SetFocusonDemand(e) {
    var key = cddl_AmountAre.GetValue();
    if (key == '1' || key == '3') {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    }
    else if (key == '2') {
        cddlVatGstCst.Focus();
    }

}

function SetLostFocusonDemand(e) {
    if ((new Date($("#hdnLockFromDate").val()) <= tstartdate.GetDate()) && (tstartdate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
        jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
    }
}

var PosGstId = "";
function PopulatePosGst(e) {

    PosGstId = cddl_PosGst.GetValue();
    if (PosGstId == "S") {
        cddl_PosGst.SetValue("S");
    }
    else if (PosGstId == "B") {
        cddl_PosGst.SetValue("B");
    }
    if ($("#hdnPlaceShiptoParty").val() == "1") {
        TaxDeleteForShipPartyChange();
    }
}


function PopulateGSTCSTVAT(e) {
    var key = cddl_AmountAre.GetValue();
    //deleteAllRows();

    if (key == 1) {

        grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(false);
        //cddlVatGstCst.PerformCallback('1');
        cddlVatGstCst.SetSelectedIndex(0);
        cbtn_SaveRecords.SetVisible(true);
        grid.GetEditor('ProductID').Focus();
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }

    }
    else if (key == 2) {
        grid.GetEditor('TaxAmount').SetEnabled(true);

        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');
        cddlVatGstCst.Focus();
        cbtn_SaveRecords.SetVisible(true);
    }
    else if (key == 3) {

        grid.GetEditor('TaxAmount').SetEnabled(false);

        //cddlVatGstCst.PerformCallback('3');
        cddlVatGstCst.SetSelectedIndex(0);
        cddlVatGstCst.SetEnabled(false);
        cbtn_SaveRecords.SetVisible(false);
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }


    }

}

//Date Function Start

function Startdate(s, e) {
    grid.batchEditApi.EndEdit();
    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }


    var t = s.GetDate();
    ccmbGstCstVat.PerformCallback(t);
    ccmbGstCstVatcharge.PerformCallback(t);
    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
    deleteTax('DeleteAllTax', "", "")
    if (IsProduct == "Y") {
        $('#hdfIsDelete').val('D');
        $('#HdUpdateMainGrid').val('True');
        grid.UpdateEdit();
    }

    if (t == "")
    { $('#MandatorysDate').attr('style', 'display:block'); }
    else { $('#MandatorysDate').attr('style', 'display:none'); }
}
function Enddate(s, e) {

    var t = s.GetDate();
    if (t == "")
    { $('#MandatoryEDate').attr('style', 'display:block'); }
    else { $('#MandatoryEDate').attr('style', 'display:none'); }



    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);

    if (startDate > endDate) {

        flag = false;
        $('#MandatoryEgSDate').attr('style', 'display:block');
    }
    else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
}

//Date Function End

// Popup Section

function ShowCustom() {

    cPopup_wareHouse.Show();


}

var IsProduct = "";
var currentEditableVisibleIndex;
var preventEndEditOnLostFocus = false;
var lastProductID;
var setValueFlag;
var _GetAmountValue = "0";
var Cur_TotalAmt = "0";
var Pre_TotalAmt = "0";
var Pre_Qty = "0";
var Pre_Price = "0";
var Pre_Discount = "0";

function GridCallBack() {
    $('#ddlInventory').focus();
    //grid.PerformCallback('Display');
}

function ReBindGrid_Currency() {
    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

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
        grid.PerformCallback('CurrencyChangeDisplay');
    }
}

function ProductsCombo_SelectedIndexChanged(s, e) {
    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");
    //var tbStkUOM = grid.GetEditor("StockUOM");
    //var tbStockQuantity = grid.GetEditor("StockQuantity");

    //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
    //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "";
    var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    strProductName = strDescription;

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];

    var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
    if (strRate == 0) {
        strSalePrice = strSalePrice;
    }
    else {
        strSalePrice = strSalePrice / strRate;
    }

    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbSalePrice.SetValue(strSalePrice);

    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strProductName);
    $('#lblbranchName').text(strBranch);

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }
    //divPacking.style.display = "none";

    //lblbranchName lblProduct
    //tbStkUOM.SetValue(strStkUOM);
    //tbStockQuantity.SetValue("0");
    //Debjyoti
    // ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "")
    //cacpAvailableStock.PerformCallback(strProductID);
}
function cmbContactPersonEndCall(s, e) {
    if (cContactPerson.cpDueDate != null) {
        var CreditDays = cContactPerson.cpDueDate;
        var newdate = new Date();
        var today = new Date();

        today = tstartdate.GetDate();
        newdate.setDate(today.getDate() + Math.round(CreditDays));

        cdt_SaleInvoiceDue.SetDate(newdate);
        ctxtCreditDays.SetValue(CreditDays);

        cContactPerson.cpDueDate = null;
    }

    if (cContactPerson.cpTotalDue != null) {
        var TotalDue = cContactPerson.cpTotalDue;
        var TotalCustDue = "";
        if (TotalDue >= 0) {
            TotalCustDue = TotalDue + ' Db';
            document.getElementById('lblTotalDues').style.color = "black";
        }
        else {
            TotalDue = TotalDue * (-1);
            TotalCustDue = TotalDue + ' Cr';
            document.getElementById('lblTotalDues').style.color = "red";
        }

        document.getElementById('lblTotalDues').innerHTML = TotalCustDue;
        pageheaderContent.style.display = "block";
        divDues.style.display = "block";
        cContactPerson.cpTotalDue = null;
    }
}
function CreditDays_TextChanged(s, e) {
    var CreditDays = ctxtCreditDays.GetValue();
    var newdate = new Date();
    var today = new Date();

    today = tstartdate.GetDate();
    today.setDate(today.getDate() + Math.round(CreditDays));

    cdt_SaleInvoiceDue.SetDate(today);
}

function BeginComponentCallback() {

}

function SelecttedProductEditUomConversion() {
    var DetId = "";
    if ($("#hdnPageEditId").val() != "") {
        DetId = $("#hdnPageEditId").val();

        $.ajax({
            type: "POST",
            url: "SalesInvoice.aspx/GetUomEditConversion",
            data: JSON.stringify({ DetId: DetId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                var UomList = msg.d;

                if (UomList.length > 0) {
                    for (var i = 0; i < UomList.length ; i++) {
                        var arrobj = {};
                        arrobj.productid = UomList[i].productid;
                        arrobj.slno = i + 1;
                        arrobj.Quantity = UomList[i].Quantity;
                        arrobj.packing = UomList[i].packing;
                        arrobj.PackingUom = UomList[i].PackingUom;
                        arrobj.PackingSelectUom = UomList[i].PackingSelectUom;
                        aarr.push(arrobj);

                    }


                }





            }
        });

    }
}

function SelecttedProductAddUomConversion() {
    var DetId = "";
    if (grid.cpProductDetailsId != "") {
        DetId = grid.cpProductDetailsId;
        if (DetId != null) {
            $.ajax({
                type: "POST",
                url: "SalesInvoice.aspx/GetUomConversion",
                data: JSON.stringify({ DetId: DetId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var UomList = msg.d;
                    if (UomList.length > 0) {
                        for (var i = 0; i < UomList.length ; i++) {
                            var arrobj = {};
                            arrobj.productid = UomList[i].productid;
                            arrobj.slno = i + 1;
                            arrobj.Quantity = UomList[i].Quantity;
                            arrobj.packing = UomList[i].packing;
                            arrobj.PackingUom = UomList[i].PackingUom;
                            arrobj.PackingSelectUom = UomList[i].PackingSelectUom;
                            aarr.push(arrobj);

                        }


                    }





                }
            });

        }
    }
}



function Save_ButtonClick() {
    LoadingPanel.Show();

    flag = true;
    $('#hfControlData').val($('#hfControlSaveData').val());
    grid.batchEditApi.EndEdit();
    // Quote no validation Start

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
        return;
    }


    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/GetEINvDetails",
        data: JSON.stringify({ Id: $("#ddl_Branch").val(), CustId: $("#hdnCustomerId").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (r) {
            //$("#hdnEntityType").val(r.d);
            var val = r.d;
            if (val != null && val.length > 0) {
                if (val[0].BranchCompany != "") {
                    if (val[0].CustomerId != "") {
                        if (val[0].BranchCompany != "" && val[0].CustomerId != "") {
                            Baddr1 = ctxtAddress1.GetText();
                            Baddr2 = ctxtAddress2.GetText();
                            Baddr3 = ctxtAddress3.GetText();
                            Baddr4 = ctxtlandmark.GetText();
                            saddr1 = ctxtsAddress1.GetText()
                            saddr2 = ctxtsAddress2.GetText();
                            saddr3 = ctxtsAddress3.GetText();
                            saddr4 = ctxtslandmark.GetText();
                            if (ctxtAddress1.GetText() == "" || ctxtAddress2.GetText() == "" || ctxtlandmark.GetText() == "" || ctxtsAddress1.GetText() == "" || ctxtsAddress2.GetText() == "" || ctxtslandmark.GetText() == "") {
                                LoadingPanel.Hide();

                                jAlert("Address1 , Address2 and landmark  are mandatory for billing and shipping.");
                                flag = false;
                                return;

                            }
                            if (Baddr1.length < 3 || Baddr2.length < 3 || Baddr4.length < 3 || saddr1.length < 3 || saddr2.length < 3 || saddr4.length < 3) {
                                LoadingPanel.Hide();

                                jAlert("Please enter Address1 , Address2 and landmark  between 3 to 100 numbers.");
                                flag = false;
                                return;
                            }
                        }
                    }
                }
            }

        }

    });


    var QuoteNo = ctxt_PLQuoteNo.GetText();
    if (QuoteNo == '' || QuoteNo == null) {
        LoadingPanel.Hide();
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }

    // Quote no validation End


    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (sdate == null || sdate == "") {
        LoadingPanel.Hide();
        flag = false;
        $('#MandatorysDate').attr('style', 'display:block');
    }
    else { $('#MandatorysDate').attr('style', 'display:none'); }
    if (sdate == "") {
        LoadingPanel.Hide();
        flag = false;
        $('#MandatoryEDate').attr('style', 'display:block');
    }
    else {
        $('#MandatoryEDate').attr('style', 'display:none');
        //if (startDate > endDate) {

        //    flag = false;
        //    $('#MandatoryEgSDate').attr('style', 'display:block');
        //}
        //else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
    }
    // Quote Date validation End

    // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        LoadingPanel.Hide();

        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }

    //Rev Sayantani
    if ($("#hdnCrDateMandatory").val() == "1") {
        if (ctxtCreditDays.GetValue() == 0) {
            LoadingPanel.Hide();
            jAlert("Credit Days must be greater than Zero(0)");
            flag = false;
        }
    }
    // End of Rev Sayantani
    // Quote Customer validation End
    var amtare = cddl_AmountAre.GetValue();
    if (amtare == '2') {
        var taxcodeid = cddlVatGstCst.GetValue();
        if (taxcodeid == '' || taxcodeid == null) {
            $('#Mandatorytaxcode').attr('style', 'display:block');
            flag = false;
        }
        else {
            $('#Mandatorytaxcode').attr('style', 'display:none');
        }
    }

    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }

    if (flag != false) {
        if (IsProduct == "Y") {

            if (issavePacking == 1) {
                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "SalesInvoice.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //divSubmitButton.style.display = "none";
                            var customerval = GetObjectID('hdnCustomerId').value;
                            $('#hdfLookupCustomer').val(customerval);

                            $('#hdnRefreshType').val('N');
                            $('#hdfIsDelete').val('I');

                            grid.AddNewRow();
                            grid.UpdateEdit();
                        }
                    });
                }
                else {



                    //divSubmitButton.style.display = "none";
                    var customerval = GetObjectID('hdnCustomerId').value;
                    $('#hdfLookupCustomer').val(customerval);

                    $('#hdnRefreshType').val('N');
                    $('#hdfIsDelete').val('I');

                    grid.AddNewRow();
                    grid.UpdateEdit();
                }
            }
            else {

                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "SalesInvoice.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //divSubmitButton.style.display = "none";
                            var customerval = GetObjectID('hdnCustomerId').value;
                            $('#hdfLookupCustomer').val(customerval);

                            $('#hdnRefreshType').val('N');
                            $('#hdfIsDelete').val('I');

                            grid.AddNewRow();
                            grid.UpdateEdit();
                        }
                    });
                }
                else {

                    //divSubmitButton.style.display = "none";
                    var customerval = GetObjectID('hdnCustomerId').value;
                    $('#hdfLookupCustomer').val(customerval);

                    $('#hdnRefreshType').val('N');
                    $('#hdfIsDelete').val('I');

                    grid.AddNewRow();
                    grid.UpdateEdit();
                }
            }
        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}

function SaveExit_ButtonClick() {
    LoadingPanel.Show();

    flag = true;
    $('#hfControlData').val($('#hfControlSaveData').val());
    grid.batchEditApi.EndEdit();

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
        return;
    }
   
    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/GetEINvDetails",
        data: JSON.stringify({ Id: $("#ddl_Branch").val(), CustId: $("#hdnCustomerId").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (r) {
            //$("#hdnEntityType").val(r.d);
            var val = r.d;
            //if (val[0].CustomerId =="")
            //{
            //    LoadingPanel.Hide();
            //    flag = false;
            //    jAlert("Please select registered customer.")
            //}
            //if (val[0].BranchCompany =="")
            //{
            //    LoadingPanel.Hide();
            //    flag = false;
            //    jAlert("This Branch/Company are not map with EInvoice.")
            //}

           

            if (val != null && val.length > 0) {
                if (val[0].BranchCompany != "") {
                    if (val[0].CustomerId != "") {
                        if (val[0].BranchCompany != "" && val[0].CustomerId != "") {
                            Baddr1 = ctxtAddress1.GetText();
                            Baddr2 = ctxtAddress2.GetText();
                            Baddr3 = ctxtAddress3.GetText();
                            Baddr4 = ctxtlandmark.GetText();
                            saddr1 = ctxtsAddress1.GetText()
                            saddr2 = ctxtsAddress2.GetText();
                            saddr3 = ctxtsAddress3.GetText();
                            saddr4 = ctxtslandmark.GetText();
                            if (ctxtAddress1.GetText() == "" || ctxtAddress2.GetText() == "" || ctxtlandmark.GetText() == "" || ctxtsAddress1.GetText() == "" || ctxtsAddress2.GetText() == "" || ctxtslandmark.GetText() == "") {
                                LoadingPanel.Hide();

                                jAlert("Address1 , Address2 and landmark  are mandatory for billing and shipping.");
                                flag = false;
                                return;

                            }
                            if (Baddr1.length < 3 || Baddr2.length < 3 || Baddr4.length < 3 || saddr1.length < 3 || saddr2.length < 3 || saddr4.length < 3) {
                                LoadingPanel.Hide();


                                jAlert("Please enter Address1 , Address2 and landmark  between 3 to 100 numbers.");
                                flag = false;
                                return;
                            }
                        }
                    }
                }
            }
            

        }

    });


    // Quote no validation Start
    var QuoteNo = ctxt_PLQuoteNo.GetText();
    if (QuoteNo == '' || QuoteNo == null) {
        LoadingPanel.Hide();
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }


    // Quote no validation End


    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (sdate == null || sdate == "") {
        LoadingPanel.Hide();
        flag = false;
        $('#MandatorysDate').attr('style', 'display:block');
    }
    else { $('#MandatorysDate').attr('style', 'display:none'); }
    //if (edate == null || sdate == "") {
    if (sdate == "") {
        LoadingPanel.Hide();
        flag = false;
        $('#MandatoryEDate').attr('style', 'display:block');
    }
    else {
        //$('#MandatoryEDate').attr('style', 'display:none');
        //if (startDate > endDate) {

        //    flag = false;
        //    $('#MandatoryEgSDate').attr('style', 'display:block');
        //}
        //else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
    }


    // Quote Date validation End

    // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        LoadingPanel.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
    // Quote Customer validation End

    var amtare = cddl_AmountAre.GetValue();
    if (amtare == '2') {
        var taxcodeid = cddlVatGstCst.GetValue();
        if (taxcodeid == '' || taxcodeid == null) {
            LoadingPanel.Hide();
            $('#Mandatorytaxcode').attr('style', 'display:block');
            flag = false;
        }
        else {
            $('#Mandatorytaxcode').attr('style', 'display:none');
        }
    }

    //if ($('#AltCrDateMandatoryInSI').val() == "1" || ctxtCreditDays.GetValue() == 0) {
    //        LoadingPanel.Hide();
    //        jAlert("Credit Days must be greater than Zero(0)");
    //        flag = false;

    //}

    if ($("#hdnCrDateMandatory").val() == "1") {
        if (ctxtCreditDays.GetValue() == 0) {
            LoadingPanel.Hide();
            jAlert("Credit Days must be greater than Zero(0)");
            flag = false;
        }
    }


    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }
    //Mantis Issue 24881
    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/CheckStateGSTIN",
        data: JSON.stringify({ Id: $("#ddl_Branch").val(), CustId: $("#hdnCustomerId").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (r) {
            var val = r.d;
            //var Status = "";
            var GSTIN = "";
            var GSTINStateCode = "";
            var BillingStateCode = "";
            var BillingState = "";
            var ReturnStatus = "";
            if (val != null && val.length > 0) {
                ReturnStatus = val[0].ReturnStatus;
                GSTIN = val[0].GSTIN;
                GSTINStateCode = val[0].GSTINStateCode;
                BillingStateCode = val[0].BillingStateCode;
                BillingState = val[0].BillingState;
            }
            //Rev work start 21.07.2022 mantise no:0025044 Unable to make Sales Invoice for a Customer whose GSTIN is Blank(Unregistered Party) 
           /* if (ReturnStatus == "InValid") {
                //flag = false;
                LoadingPanel.Hide();
                jAlert("The customer GSTIN is '" + GSTIN + "' where the state code is " + GSTINStateCode + " which is not matched with the billing state code [" + BillingState + ", STCD: " + BillingStateCode + "], Please rectify & proceed.")
                flag = false;
            }*/
            if(GSTIN!="")
            {
                if (ReturnStatus == "InValid") {
                    //flag = false;
                    LoadingPanel.Hide();
                    jAlert("The customer GSTIN is '" + GSTIN + "' where the state code is " + GSTINStateCode + " which is not matched with the billing state code [" + BillingState + ", STCD: " + BillingStateCode + "], Please rectify & proceed.")
                    flag = false;
                }
            }
            //Rev work close 21.07.2022 mantise no:0025044 Unable to make Sales Invoice for a Customer whose GSTIN is Blank(Unregistered Party) 

        }

    });
    //End of Mantis Issue 24881
    if (flag != false) {
        if (IsProduct == "Y") {

            if (issavePacking == 1) {
                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "SalesInvoice.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //divSubmitButton.style.display = "none";
                            var customerval = GetObjectID('hdnCustomerId').value;
                            $('#hdfLookupCustomer').val(customerval);

                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');

                            grid.AddNewRow();
                            grid.UpdateEdit();
                        }
                    });
                }
                else {
                    //divSubmitButton.style.display = "none";
                    var customerval = GetObjectID('hdnCustomerId').value;
                    $('#hdfLookupCustomer').val(customerval);

                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');

                    grid.AddNewRow();
                    grid.UpdateEdit();

                }
            }

            else {

                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "SalesInvoice.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //divSubmitButton.style.display = "none";
                            var customerval = GetObjectID('hdnCustomerId').value;
                            $('#hdfLookupCustomer').val(customerval);

                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');

                            grid.AddNewRow();
                            grid.UpdateEdit();
                        }
                    });
                }
                else {

                    //divSubmitButton.style.display = "none";
                    var customerval = GetObjectID('hdnCustomerId').value;
                    $('#hdfLookupCustomer').val(customerval);

                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');

                    grid.AddNewRow();
                    grid.UpdateEdit();
                }

            }
        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}

function Uom_LostFocus(s, e) {
    if ($("#hddnMultiUOMSelection").val() == "0") {
        setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 11); }, 600);

    }
}

function Quantity_lostFocus(s, e) {

    QuantityTextChange();
}

function OnMultiUOMEndCallback(s, e) {
   
    if (cgrid_MultiUOM.cpDuplicateAltUOM == "DuplicateAltUOM") {
        cgrid_MultiUOM.cpDuplicateAltUOM = null;
        jAlert("Please Enter Different Alt. Quantity.");
        return;
    }
    if (cgrid_MultiUOM.cpOpenFocus == "OpenFocus") {
        ccmbSecondUOM.SetFocus();
        cgrid_MultiUOM.cpOpenFocus = null;
    }
    //Mantis Issue 24425, 24428
    if (cgrid_MultiUOM.cpSetBaseQtyRateInGrid != null && cgrid_MultiUOM.cpSetBaseQtyRateInGrid == "1") {
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
        cgrid_MultiUOM.cpSetBaseQtyRateInGrid = null;
        var BaseQty = cgrid_MultiUOM.cpBaseQty;
        var BaseRate = cgrid_MultiUOM.cpBaseRate;
        var AltQuantity = cgrid_MultiUOM.cpAltQuantity;
        var AltUOM = cgrid_MultiUOM.cpAltUOM;
        
        grid.GetEditor("Quantity").SetValue(BaseQty);
        grid.GetEditor("SalePrice").SetValue(BaseRate);
        grid.GetEditor("Amount").SetValue(BaseQty * BaseRate);
        grid.GetEditor("InvoiceDetails_AltQuantity").SetValue(AltQuantity);
        grid.GetEditor("InvoiceDetails_AltUOM").SetValue(AltUOM);

        SalePriceTextChange(null, null);
        
        
    }

    if (cgrid_MultiUOM.cpAllDetails == "EditData") {
        var Quan = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
        $('#UOMQuantity').val(Quan);
       // $('#UOMQuantity').val(cgrid_MultiUOM.cpBaseQty);
        ccmbBaseRate.SetValue(cgrid_MultiUOM.cpBaseRate)
        ccmbSecondUOM.SetValue(cgrid_MultiUOM.cpAltUom);
        cAltUOMQuantity.SetValue(cgrid_MultiUOM.cpAltQty);
        ccmbAltRate.SetValue(cgrid_MultiUOM.cpAltRate);
        hdMultiUOMID = cgrid_MultiUOM.cpuomid;
        if (cgrid_MultiUOM.cpUpdatedrow == true) {
            $("#chkUpdateRow").prop('checked', true);
            $("#chkUpdateRow").attr('checked', 'checked');


            }
        else {
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");

        }
        cgrid_MultiUOM.cpUpdatedrow = null;
        cgrid_MultiUOM.cpAllDetails = null;
    }
    // End of Mantis Issue 24425, 24428
}   

var Uomlength = 0;
function UomLenthCalculation() {
    grid.batchEditApi.StartEdit(globalRowIndex);
    var SLNo = "";
    var val = 0;
    var detailsid = grid.GetEditor('DetailsId').GetValue();
    var DeliveryScheduleDetailsID = (grid.GetEditor('DeliveryScheduleDetailsID').GetText() != null) ? grid.GetEditor('DeliveryScheduleDetailsID').GetText() : "0";

    if ($("#hdnPageStatus").val() == "update") {

        if (detailsid != null && detailsid != "") {
            SLNo = detailsid;
            val = 1;
        }
        else {
            SLNo = grid.GetEditor('SrlNo').GetValue();
        }

    }

    else if (DeliveryScheduleDetailsID != null && DeliveryScheduleDetailsID != "0" && DeliveryScheduleDetailsID != "") {
        SLNo = DeliveryScheduleDetailsID;
        val = 1;
    }
    else if (detailsid != null && detailsid != "") {
        SLNo = detailsid;
        val = 1;
    }
    else {
        SLNo = grid.GetEditor('SrlNo').GetValue();
    }


    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/GetQuantityfromSL",
        data: JSON.stringify({ SLNo: SLNo, val: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            Uomlength = msg.d;

        }
    });
}

function spLostFocus(s, e) {
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();
    var SpliteDetails = ProductID.split("||@||");
    var sProduct_PurPrice = SpliteDetails[13];

    if ($('#hdnSalesRateBuyRateChecking').val() == "1") {
        if (parseFloat(sProduct_PurPrice) > parseFloat(Saleprice)) {

            jAlert("Your Buy rate is Rs." + sProduct_PurPrice + " " + "for the product where the selling rate is Rs." + Saleprice + " You are making a Net Loss of Rs." + (parseFloat(sProduct_PurPrice) - parseFloat(Saleprice)) + " (Loss Percentage:" + (((parseFloat(sProduct_PurPrice) - parseFloat(Saleprice)) * 100) / parseFloat(sProduct_PurPrice)).toFixed(2) + "%)", "Alert");
            //return;
        }
    }
    QuantityTextChange(s, e);

}

function QuantityTextChange(s, e) {

    //chinmoy added for multiUom start
    cbtn_SaveRecords_N.SetVisible(false);
    cbtn_SaveRecords_p.SetVisible(false);

    // Rev Sanchita
    //if (($("#hddnMultiUOMSelection").val() == "1")) {


    //    //setTimeout(function () {
    //    UomLenthCalculation();
    //    //  }, 200)

    //    grid.batchEditApi.StartEdit(globalRowIndex);
    //    var SLNo = grid.GetEditor('SrlNo').GetValue();

    //    if (Uomlength > 0) {
    //        var qnty = $("#UOMQuantity").val();
    //        var QValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0.0000";
    //        if (QValue != "0.0000" && QValue != qnty) {
    //            jConfirm('Qunatity Change Will Clear Multiple UOM Details.?', 'Confirmation Dialog', function (r) {
    //                if (r == true) {
    //                    grid.batchEditApi.StartEdit(globalRowIndex);
    //                    var tbqty = grid.GetEditor('Quantity');
    //                    //tbqty.SetValue(Quantity);

    //                    var detailsid = grid.GetEditor('DetailsId').GetValue();
    //                    if (detailsid != null && detailsid != "") {
    //                        cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo + '~' + detailsid);
    //                    }
    //                    else {
    //                        cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo + '~' + detailsid);
    //                    }

    //                    setTimeout(function () {
    //                        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //                    }, 600)
    //                }
    //                else {
    //                    grid.batchEditApi.StartEdit(globalRowIndex);
    //                    grid.GetEditor('Quantity').SetValue(qnty);
    //                    setTimeout(function () {
    //                        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    //                    }, 200);
    //                }


    //            });
    //        }
    //        else {
    //            grid.batchEditApi.StartEdit(globalRowIndex);
    //            grid.GetEditor('Quantity').SetValue(qnty);

    //            setTimeout(function () {
    //                grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //            }, 600)

    //        }
    //    }

    //}
    //End
    // End of Rev Sanchita


    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var key = gridquotationLookup.GetValue();// gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

    // if (parseFloat(Pre_Qty) != parseFloat(QuantityValue)) {
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strMultiplier = SpliteDetails[7];
        var strFactor = SpliteDetails[8];
        var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";

        if (key != null && key != '') {
            var IsComponentProduct = SpliteDetails[15];
            var ComponentProduct = SpliteDetails[16];
            var TotalQty = (grid.GetEditor('TotalQty').GetText() != null) ? grid.GetEditor('TotalQty').GetText() : "0";
            var BalanceQty = (grid.GetEditor('BalanceQty').GetText() != null) ? grid.GetEditor('BalanceQty').GetText() : "0";
            var CurrQty = 0;

            BalanceQty = parseFloat(BalanceQty);
            TotalQty = parseFloat(TotalQty);
            QuantityValue = parseFloat(QuantityValue);

            if (TotalQty > QuantityValue) {
                CurrQty = BalanceQty + (TotalQty - QuantityValue);
            }
            else {
                CurrQty = BalanceQty - (QuantityValue - TotalQty);
            }

            // Rev 1.0
            var IsToleranceInSalesOrder = document.getElementById('hdnIsToleranceInSalesOrder').value;

            if (IsToleranceInSalesOrder == "1") {
                var SOToleranceQty = 0;

                $.ajax({
                    type: "POST",
                    url: "SalesInvoice.aspx/GetSOToleranceQty",
                    data: JSON.stringify({ SODoc_ID: SODoc_ID, SODocDetailsID: SODocDetailsID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {

                        SOToleranceQty = msg.d;

                    }
                });

                if ((CurrQty + SOToleranceQty) < 0) {
                    grid.GetEditor("TotalQty").SetValue(TotalQty);
                    grid.GetEditor("Quantity").SetValue(TotalQty);
                    var OrdeMsg = 'Balance Quantity of selected Product from tagged document. <br/>Cannot enter quantity more than balance quantity.';
                    grid.batchEditApi.EndEdit();
                    jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {
                        grid.batchEditApi.StartEdit(globalRowIndex, 7);
                    });
                    return false;
                }
                else {
                    grid.GetEditor("TotalQty").SetValue(QuantityValue);
                    grid.GetEditor("BalanceQty").SetValue(CurrQty);
                }
            }
            else {
                if (CurrQty < 0) {
                    grid.GetEditor("TotalQty").SetValue(TotalQty);
                    grid.GetEditor("Quantity").SetValue(TotalQty);
                    var OrdeMsg = 'Balance Quantity of selected Product from tagged document. <br/>Cannot enter quantity more than balance quantity.';
                    grid.batchEditApi.EndEdit();
                    jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {
                        grid.batchEditApi.StartEdit(globalRowIndex, 7);
                    });
                    return false;
                }
                else {
                    grid.GetEditor("TotalQty").SetValue(QuantityValue);
                    grid.GetEditor("BalanceQty").SetValue(CurrQty);
                }
            }

            var TotalAmount = (grid.GetEditor('TotalAmount').GetText() != null) ? grid.GetEditor('TotalAmount').GetText() : "0";
            var SONetAmount = (grid.GetEditor('SONetAmount').GetText() != null) ? grid.GetEditor('SONetAmount').GetText() : "0";


        }
        else {
            grid.GetEditor("TotalQty").SetValue(QuantityValue);
            grid.GetEditor("BalanceQty").SetValue(QuantityValue);
        }

        var strProductID = SpliteDetails[0];
        var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();

        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];

        if (strRate == 0) {
            strRate = 1;
        }

        var StockQuantity = strMultiplier * QuantityValue;
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        $('#lblStkQty').text(StockQuantity);
        $('#lblStkUOM').text(strStkUOM);
        $('#lblProduct').text(strProductName);
        $('#lblbranchName').text(strBranch);

        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
            divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }

        //var tbStockQuantity = grid.GetEditor("StockQuantity");
        //tbStockQuantity.SetValue(StockQuantity);

        var IsLinkedProduct = (grid.GetEditor('IsLinkedProduct').GetText() != null) ? grid.GetEditor('IsLinkedProduct').GetText() : "";
        if (IsLinkedProduct != "Y") {
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount);

        }
        DiscountTextChange(s, e);
        //cacpAvailableStock.PerformCallback(strProductID);
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Quantity').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    //}


    //Rev Rajdip       
    //cbnrlblAmountWithTaxValue.SetText("0");
    //var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
    //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
    //cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
    SetTotalTaxableAmount(globalRowIndex, 8);
    //SetInvoiceLebelValue();
    //ENd Rev Rajdip
}

/// Code Added By Sam on 23022017 after make editable of sale price field Start
var globalNetAmount = 0;
function SalePriceTextChange(s, e) {
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();
    var SpliteDetails = ProductID.split("||@||");
    var sProduct_PurPrice = SpliteDetails[13];
    var strSalePrice = SpliteDetails[6];
    if ($('#hdnSalesRateBuyRateChecking').val() == "1") {
        if (parseFloat(sProduct_PurPrice) > parseFloat(Saleprice)) {

            jAlert("Your Buy rate is Rs." + sProduct_PurPrice + " " + "for the product where the selling rate is Rs." + Saleprice + " You are making a Net Loss of Rs." + (parseFloat(sProduct_PurPrice) - parseFloat(Saleprice)) + " (Loss Percentage:" + (((parseFloat(sProduct_PurPrice) - parseFloat(Saleprice)) * 100) / parseFloat(sProduct_PurPrice)).toFixed(2) + "%)", "Alert");
          //  grid.GetEditor('SalePrice').SetValue(strSalePrice);
            return;
        }
    }
    //Mantis 24428 cbtn_SaveRecords_N,cbtn_SaveRecords_p SetVisible false comment because of wrong behaviour of this two button
    //cbtn_SaveRecords_N.SetVisible(false);
    //cbtn_SaveRecords_p.SetVisible(false);
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();

    if (ProductID != null) {
        if ($("#ProductMinPrice").val() <= Saleprice && $("#ProductMaxPrice").val() >= Saleprice) {

        }
        else {
            if ($("#hdnRateType").val() == "2") {
                jAlert("Product Min price :" + $("#ProductMinPrice").val() + " and Max price :" + $("#ProductMaxPrice").val(), "Alert", function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 12);
                    return;
                });
                return;
            }
        }
    }

    if (parseFloat(Pre_Price) != parseFloat(Saleprice)) {
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");

            console.log(SpliteDetails);
            // Rev Sanchita 
            //if (parseFloat(s.GetValue()) < parseFloat(SpliteDetails[17])) {
            if (parseFloat(grid.GetEditor('SalePrice').GetValue()) < parseFloat(SpliteDetails[17])) {
                // End of Rev Sanchita
                jAlert("Sale price cannot be lesser than Min Sale Price locked as: " + parseFloat(Math.round(Math.abs(parseFloat(SpliteDetails[17])) * 100) / 100).toFixed(2), "Alert", function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 10);
                    return;
                });
                s.SetValue(parseFloat(SpliteDetails[6]));
                return;
            }

            // Rev Sanchita 
            //if (parseFloat(SpliteDetails[18]) != 0 && parseFloat(s.GetValue()) > parseFloat(SpliteDetails[18])) {
            if (parseFloat(SpliteDetails[18]) != 0 && parseFloat(grid.GetEditor('SalePrice').GetValue()) > parseFloat(SpliteDetails[18])) {
                // End of Rev Sanchita
                jAlert("Sale price cannot be greater than MRP locked as: " + parseFloat(Math.round(Math.abs(parseFloat(SpliteDetails[18])) * 100) / 100).toFixed(2), "Alert", function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 10);
                    return;
                });
                s.SetValue(parseFloat(SpliteDetails[6]));
                return;
            }



            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            //var strRate = "1";
            var strStkUOM = SpliteDetails[4];
            //var strSalePrice = SpliteDetails[6];

            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            if (strRate == 0) {
                strRate = 1;
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

            var Amount = QuantityValue * strFactor * (Saleprice / strRate);

            var IsDiscountPercentage = document.getElementById('IsDiscountPercentage').value;
            var amountAfterDiscount = "0";

            if (IsDiscountPercentage == "Y") {
                if (parseFloat(Discount) > 100) {
                    Discount = "0";

                    var tb_Discount = grid.GetEditor("Discount");
                    tb_Discount.SetValue(Discount);
                }

                amountAfterDiscount = DecimalRoundoff(parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100), 2);
            }
            else {
                amountAfterDiscount = DecimalRoundoff(parseFloat(Amount) + parseFloat(Discount), 2);
            }

            if (parseFloat(amountAfterDiscount) < 0) {
                var tb_Discount = grid.GetEditor("Discount");
                tb_Discount.SetValue("0");

                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue(Amount);

                jAlert("Amount can not less than zero(0).");
            }
            else {
                var IsLinkedProduct = (grid.GetEditor('IsLinkedProduct').GetText() != null) ? grid.GetEditor('IsLinkedProduct').GetText() : "";
                if (IsLinkedProduct != "Y") {
                    var tbAmount = grid.GetEditor("Amount");
                    //tbAmount.SetValue(DecimalRoundoff(amountAfterDiscount,2));
                    tbAmount.SetValue(amountAfterDiscount);
                    var tbTotalAmount = grid.GetEditor("TotalAmount");
                    //tbTotalAmount.SetValue(DecimalRoundoff(amountAfterDiscount, 2));
                    tbTotalAmount.SetValue(amountAfterDiscount);




                    $('#lblProduct').text(strProductName);
                    $('#lblbranchName').text(strBranch);

                    var IsPackingActive = SpliteDetails[10];
                    var Packing_Factor = SpliteDetails[11];
                    var Packing_UOM = SpliteDetails[12];
                    var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

                    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                        $('#lblPackingStk').text(PackingValue);
                        divPacking.style.display = "block";
                    } else {
                        divPacking.style.display = "none";
                    }
                }
                DiscountTextChange(s, e);
                SetTotalTaxableAmount(globalRowIndex, 13);

            }
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('SalePrice').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}

function TotalAmountgotfocus(s, e) {
}

//Rev Rajdip For Running Parameters
function SetTotalTaxableAmount(inx, vindex) {
    var count = grid.GetVisibleRowsOnPage();
    var totalAmount = 0;
    var totaltxAmount = 0;
    var totalQuantity = 0;
    var netAmount = 0;

    for (var i = 0; i < count + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("Quantity").GetValue(), 4);
                if (grid.GetEditor("TaxAmount").GetValue() != null) {
                    totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2);
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2), 2))
                }
                else {
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                }

                netAmount = netAmount + DecimalRoundoff(grid.GetEditor("TotalAmount").GetValue(), 2);
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("Quantity").GetValue(), 4);
                if (grid.GetEditor("TaxAmount").GetValue() != null) {
                    totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2);
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2), 2))

                }
                else {
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                }
                //totalAmount = totalAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "Amount"), 2);
                //totaltxAmount = totaltxAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "TaxAmount"), 2);
                netAmount = netAmount + DecimalRoundoff(grid.GetEditor("TotalAmount").GetValue(), 2);


                //if (globalRowIndex == i) {
                //    if ($(grid.GetRow(i).children[10].children[1].children[0].children[0].children[0].children[0].children[0]).val().replace('{', '').replace("}", "").split(':')[1].split(',')[0].replace(/&quot;/g, '\\"').replace(/['"]+/g, '').replace('\\', '').split('\\')[0].trim() != "")
                //        totalAmount = totalAmount + DecimalRoundoff($(grid.GetRow(i).children[10].children[1].children[0].children[0].children[0].children[0].children[0]).val().replace('{', '').replace("}", "").split(':')[1].split(',')[0].replace(/&quot;/g, '\\"').replace(/['"]+/g, '').replace('\\', '').split('\\')[0], 2);
                //    if ($(grid.GetRow(i).children[11].children[1].children[0].children[0].children[0].children[0].children[0]).val().trim() != "")
                //        totaltxAmount = totaltxAmount + DecimalRoundoff($(grid.GetRow(i).children[11].children[1].children[0].children[0].children[0].children[0].children[0]).val(), 2);
                //}
                //else {
                //    if (grid.GetRow(i).children[10].children[0].innerText.trim() != "")
                //        totalAmount = totalAmount + DecimalRoundoff(grid.GetRow(i).children[10].children[0].innerText, 2);
                //    if (grid.GetRow(i).children[11].children[0].innerText.trim() != "")
                //        totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetRow(i).children[11].children[0].innerText, 2);

                //}
            }
        }
    }

    globalRowIndex = inx;

    grid.batchEditApi.EndEdit()
    netAmount = netAmount + parseFloat($("#bnrOtherChargesvalue").text());
    cbnrLblTaxableAmtval.SetText(DecimalRoundoff(totalAmount, 2));
    cbnrLblTaxAmtval.SetText(DecimalRoundoff(totaltxAmount, 2));
    cbnrLblInvValue.SetText(DecimalRoundoff(netAmount, 2));
    cbnrlblAmountWithTaxValue.SetText(DecimalRoundoff(netAmount, 2));

    cbnrLblTotalQty.SetText(DecimalRoundoff(totalQuantity, 4));
    //grid.batchEditApi.StartEdit(globalRowIndex, vindex);
    setTimeout(function () { grid.batchEditApi.StartEdit(inx, vindex); }, 600)
}
//Rev Rajdip
function SetInvoiceLebelValue() {

    var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
    //if (document.getElementById('HdPosType').value == 'Crd') {
    //    if (invValue < 0) {
    //        var newAdvAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
    //        cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(newAdvAmount) * 100) / 100).toFixed(2));
    //    }
    //}

    //if (document.getElementById('HdPosType').value == 'Fin') {
    //    if (invValue < 0) {
    //        var newAdvAmountfin = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
    //        cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(ctxtdownPayment.GetValue()) * 100) / 100).toFixed(2));
    //    }
    //}



    //if (document.getElementById('HdPosType').value == 'Crd')
    //    invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
    //else if (document.getElementById('HdPosType').value == 'Fin')
    //    invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue()) + parseFloat(cbnrOtherChargesvalue.GetValue());
    //alert(invValue)

    cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));


    // SetRunningBalance();

}
//End Rev Rajdip
//End Rev Rajdip
function DiscountValueChange(s, e) {
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

    if (ProductID != null) {
        if (parseFloat(Discount) != parseFloat(Pre_Discount)) {
            DiscountTextChange(s, e);
            SetTotalTaxableAmount(globalRowIndex, 17);
        }
    }
    else {
        jAlert('Select a product first.');
    }


}


function DiscountTextChange(s, e) {
    //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var IsLinkedProduct = (grid.GetEditor('IsLinkedProduct').GetText() != null) ? grid.GetEditor('IsLinkedProduct').GetText() : "";
    var _TotalAmount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0.00";
    var ProductID = grid.GetEditor('ProductID').GetValue();

    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strFactor = SpliteDetails[8];
        var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
        if (strSalePrice == '0') {
            strSalePrice = SpliteDetails[6];
        }
        if (strRate == 0) {
            strRate = 1;
        }
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        var IsDiscountPercentage = document.getElementById('IsDiscountPercentage').value;
        var amountAfterDiscount = "0";

        if (IsDiscountPercentage == "Y") {
            if (parseFloat(Discount) > 100) {
                Discount = "0";

                var tb_Discount = grid.GetEditor("Discount");
                tb_Discount.SetValue(Discount);
            }

            amountAfterDiscount = DecimalRoundoff(parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100), 2);
        }
        else {
            amountAfterDiscount = DecimalRoundoff(parseFloat(Amount) + parseFloat(Discount), 2);
        }

        if (parseFloat(amountAfterDiscount) < 0) {
            var tb_Discount = grid.GetEditor("Discount");
            tb_Discount.SetValue("0");

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(DecimalRoundoff(Amount, 2));

            jAlert("Amount can not less than zero(0).");
        }
        else {
            if (IsLinkedProduct != "Y") {
                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue(amountAfterDiscount);
            }

            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#lblPackingStk').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            if (IsLinkedProduct != "Y") {
                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue(amountAfterDiscount);
            }
            else {
                var tbDiscount = grid.GetEditor("Discount");
                tbDiscount.SetValue("0");

                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue("0");

                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue("0");
            }

            var ShippingStateCode = $("#bsSCmbStateHF").val();
            var TaxType = "";
            if (cddl_AmountAre.GetValue() == "1") {
                TaxType = "E";
            }
            else if (cddl_AmountAre.GetValue() == "2") {
                TaxType = "I";
            }

            var CompareStateCode;
            if (cddl_PosGst.GetValue() == "S") {
                CompareStateCode = GeteShippingStateID();
            }
            else {
                CompareStateCode = GetBillingStateID();
            }
            var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";
            if (inventoryType == "S" && parseFloat(QuantityValue) == 0) {
                //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], _TotalAmount, _TotalAmount, TaxType, CompareStateCode, $('#ddl_Branch').val());
                caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), tstartdate.GetDate(), QuantityValue);

            }
            else {
                //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val());
                caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), tstartdate.GetDate(), QuantityValue);

            }
        }
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }


    if (parseFloat(Amount) != parseFloat(Pre_TotalAmt)) {
        // ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());

        var SrlNo = grid.GetEditor("SrlNo").GetValue();
        $.ajax({
            type: "POST",
            url: "SalesInvoice.aspx/DeleteTaxOnSrl",
            data: JSON.stringify({ SrlNo: SrlNo }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                cbtn_SaveRecords_N.SetVisible(true);
                cbtn_SaveRecords_p.SetVisible(true);
            }
        });


    }
    else {
        cbtn_SaveRecords_N.SetVisible(true);
        cbtn_SaveRecords_p.SetVisible(true);
    }

    // SetTotalTaxableAmount(globalRowIndex, 7);
}

function SetEntityType(Id) {

    $.ajax({
        type: "POST",
        url: "SalesQuotation.aspx/GetEntityType",
        data: JSON.stringify({ Id: Id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            $("#hdnEntityType").val(r.d);
        }

    });
}


function AmountTextFocus(s, e) {
    var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    _GetAmountValue = Amount;
    //SetTotalTaxableAmount(globalRowIndex, 17);
}

function ProductAmountTextChange(s, e) {
    var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var TaxAmount = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    if (parseFloat(_GetAmountValue) != parseFloat(Amount)) {
        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(Amount + TaxAmount);

        var ProductID = grid.GetEditor('ProductID').GetValue();
        var SpliteDetails = ProductID.split("||@||");

        var ShippingStateCode = $("#bsSCmbStateHF").val();
        var TaxType = "";
        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }

        var CompareStateCode;
        if (cddl_PosGst.GetValue() == "S") {
            CompareStateCode = GeteShippingStateCode();
        }
        else {
            CompareStateCode = GetBillingStateCode();
        }
        var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";
        if (inventoryType == "S" && parseFloat(QuantityValue) == 0) {
            var _TotalAmount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], _TotalAmount, _TotalAmount, TaxType, CompareStateCode, $('#ddl_Branch').val());

            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], _TotalAmount, _TotalAmount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), tstartdate.GetDate(), QuantityValue);

        }
        else {


            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], _TotalAmount, Amount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), tstartdate.GetDate(), QuantityValue);
        }
        SetTotalTaxableAmount(globalRowIndex, 17);

    }
}

function AddBatchNew(s, e) {
    var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

    var globalRow_Index = 0;
    if (globalRowIndex > 0) {
        globalRow_Index = globalRowIndex + 1;
    }
    else {
        globalRow_Index = globalRowIndex - 1;
    }


    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode === 13) {
        if (ProductIDValue != "") {
            //var noofvisiblerows = grid.GetVisibleRowsOnPage();
            //var i;
            //var cnt = 2;

            grid.batchEditApi.EndEdit();

            grid.AddNewRow();
            grid.SetFocusedRowIndex();
            var noofvisiblerows = grid.GetVisibleRowsOnPage();

            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);

            grid.batchEditApi.StartEdit(globalRow_Index, 2);
            //grid.batchEditApi.StartEdit(-1, 1);
        }
    }
}
function OnAddNewClick() {
    if (gridquotationLookup.GetValue() == null) {
        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        // Mantis Issue 24425, 24428
        $("#UOMQuantity").val(0);
        Uomlength = 0;
        // End of Mantis Issue 24425, 24428
    }
    else {
        QuotationNumberChanged();
        grid.StartEditRow(0);
    }
}
function OnAddNewClick_AtSaveTime() {
    if (gridquotationLookup.GetValue() == null) {
        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
    }
    else {
        grid.batchEditApi.StartEdit(0, 5);
    }
}




function Save_TaxClick() {

    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    //Rev Rajdip
    cbnrOtherChargesvalue.SetText('0.00');
    SetInvoiceLebelValueofothercharges();
    //End Rev Rajdip
    cPopup_Taxes.Hide();
}
function SetInvoiceLebelValueofothercharges() {
    cbnrOtherChargesvalue.SetValue(ctxtQuoteTaxTotalAmt.GetText());
    if (ctxtTotalAmount.GetValue() == 0.0) {
        cbnrLblInvValue.SetValue(parseFloat(cbnrlblAmountWithTaxValue.GetValue()).toFixed(2));
    }
    else {
        cbnrLblInvValue.SetValue(parseFloat(ctxtTotalAmount.GetValue()).toFixed(2));
    }
}


function callback_InlineRemarks_EndCall(s, e) {

    if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
        $("#txtInlineRemarks").focus();
    }
    else {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
}


function FinalRemarks() {


    ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + grid.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');


}


function closeRemarks(s, e) {

    cPopup_InlineRemarks.Hide();
    //e.cancel = false;
    //ccallback_InlineRemarks.PerformCallback('RemarksDelete'+'~'+grid.GetEditor('SrlNo').GetValue()+'~'+$('#txtInlineRemarks').val());
    //cPopup_InlineRemarks.Hide();
    //e.cancel = false;
    // cPopup_InlineRemarks.Hide();
}


var Warehouseindex;
function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
        grid.batchEditApi.EndEdit();


        var totalNetAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'TotalAmount');
        var oldAmountWithTaxValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue());
        var AfterdeleteoldAmountWithTaxValue = oldAmountWithTaxValue - parseFloat(totalNetAmount);
        cbnrlblAmountWithTaxValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
        //cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));

        var RowQuantity = grid.batchEditApi.GetCellValue(e.visibleIndex, 'Quantity');
        var totalquantity = parseFloat(cbnrLblTotalQty.GetValue());
        var updatedquantity = (parseFloat(totalquantity) - parseFloat(RowQuantity));
        //cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));

        var rowTaxAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'TaxAmount');
        var totaltaxamt = parseFloat(cbnrLblTaxAmtval.GetValue());
        var updatedtaxamt = parseFloat(totaltaxamt) - parseFloat(rowTaxAmount);
        //cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));

        var rowAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'Amount');
        var TotalAmt = parseFloat(cbnrLblTaxableAmtval.GetValue());
        var updatedAmt = parseFloat(TotalAmt) - parseFloat(rowAmount);
        //cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));

        $('#hdnRefreshType').val('');
        $('#hdnDeleteSrlNo').val(SrlNo);
        var noofvisiblerows = grid.GetVisibleRowsOnPage();

        if (gridquotationLookup.GetValue() != null) {
            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
            var messege = "";
            if (type == "QO") {
                messege = "Cannot Delete using this button as the Proforma is linked with this Sale Invoice.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
            }
            else if (type == "SO") {
                messege = "Cannot Delete using this button as the Sales Order is linked with this Sale Invoice.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
            }
            else if (type == "SC") {
                messege = "Cannot Delete using this button as the Sales Challan is linked with this Sale Invoice.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
            }

            jAlert(messege, 'Alert Dialog: [Delete Challan Products]', function (r) {
            });
        }
        else {
            if (noofvisiblerows != "1") {
                grid.DeleteRow(e.visibleIndex);


                cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
                cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));
                cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));
                cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));

                $('#hdfIsDelete').val('D');
                grid.UpdateEdit();
                grid.PerformCallback('Display');

                $('#hdnPageStatus').val('delete');
                //grid.batchEditApi.StartEdit(-1, 2);
                //grid.batchEditApi.StartEdit(0, 2);
            }
        }
    }


    else if (e.buttonID == "addlDesc") {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex, 5);
        cPopup_InlineRemarks.Show();

        $("#txtInlineRemarks").val('');

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        if (ProductID != "") {
            // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
            ccallback_InlineRemarks.PerformCallback('DisplayRemarks' + '~' + SrlNo + '~' + '0');

        }
        else {
            $("#txtInlineRemarks").val('');
        }
        //$("#txtInlineRemarks").focus();
        document.getElementById("txtInlineRemarks").focus();
    }
    else if (e.buttonID == 'CustomMultiUOM') {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex, 9);
        var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ProductID = Productdetails.split("||@||")[0];
        var UOMName = grid.GetEditor("UOM").GetValue();
        var quantity = grid.GetEditor("Quantity").GetValue();
        var DetailsId = grid.GetEditor('DetailsId').GetText();

        var DeliveryScheduleDetailsID = (grid.GetEditor('DeliveryScheduleDetailsID').GetText() != null) ? grid.GetEditor('DeliveryScheduleDetailsID').GetText() : "0";
    

        var StockUOM = Productdetails.split("||@||")[5];
        if (StockUOM == "") {
            StockUOM = "0";
        }
        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        cAltUOMQuantity.SetValue("0.0000");
        // Mantis Issue 24425, 24428
        //if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
        if ((ProductID != "") && (UOMName != "") ) {
            // End of Mantis Issue 24425, 24428
            if (StockUOM == "0") {
                jAlert("Main Unit Not Defined.");
            }
            else {
                if ($("#hddnMultiUOMSelection").val() == "1") {
                    ccmbUOM.SetEnabled(false);
                    var index = e.visibleIndex;
                    grid.batchEditApi.StartEdit(e.visibleIndex, 6);
                    //grid.batchEditApi.StartEdit(globalRowIndex);
                    var Qnty = grid.GetEditor("Quantity").GetValue();
                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var UomId = grid.GetEditor('ProductID').GetText().split("||@||")[3];
                    ccmbUOM.SetValue(UomId);
                    // Mantis Issue 24425, 24428

                    //$("#UOMQuantity").val(Qnty);
                    $("#UOMQuantity").val("0.0000");
                    ccmbBaseRate.SetValue(0)
                    cAltUOMQuantity.SetValue("0.0000")
                    ccmbAltRate.SetValue(0)
                    ccmbSecondUOM.SetValue("")
                    // End of Mantis Issue 24425, 24428
                    // Rev 2.0
                    document.getElementById('lblInfoMsg').innerHTML = "";
                    // End of Rev 2.0
                    cPopup_MultiUOM.Show();
                    cgrid_MultiUOM.cpDuplicateAltUOM = "";
                    AutoPopulateMultiUOM();
                    //chinmoy change start
                    if ($("#hdnPageStatus").val() == "update") {
                        cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo + '~' + DetailsId);
                    }
                    else{
                        if (DeliveryScheduleDetailsID != "0" && DeliveryScheduleDetailsID != "")
                        {
                            cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo + '~' + DeliveryScheduleDetailsID);
                        }
                        else
                        {
                            cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo + '~' + DetailsId);
                        }
                    }
                  
                 
                    //cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + ProductID);
                }
            }//End
        }
        else {
            return;
        }
    }



    else if (e.buttonID == 'AddNew') {

        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var SpliteDetails = ProductID.split("||@||");

        var IsComponentProduct = SpliteDetails[15];
        var ComponentProduct = SpliteDetails[16];

        if (IsComponentProduct == "Y") {
            var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
            jConfirm(messege, 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.GetEditor("IsComponentProduct").SetValue("Y");
                    $('#hdfIsDelete').val('C');

                    grid.UpdateEdit();
                    grid.PerformCallback('Display~fromComponent');
                    //grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }
                else {
                    OnAddNewClick();

                    //setTimeout(function () {
                    //    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                    //}, 500);
                    //return false;
                }
            });
            document.getElementById('popup_ok').focus();
        }
        else {
            if (ProductID != "") {
                OnAddNewClick();

                //setTimeout(function () {
                //    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                //}, 500);
                //return false;
            }
            else {
                grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            }
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

        if (inventoryType == "C" || inventoryType == "Y") {
            Warehouseindex = index;

            var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            //var StkQuantityValue = (grid.GetEditor('StockQuantity').GetValue() != null) ? grid.GetEditor('StockQuantity').GetValue() : "0";

            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();

            if (ProductID != "" && parseFloat(QuantityValue) != 0) {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strMultiplier = SpliteDetails[7];
                var strProductName = strDescription;
                //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                var StkQuantityValue = QuantityValue * strMultiplier;
                var Ptype = SpliteDetails[14];
                $('#hdfProductType').val(Ptype);

                document.getElementById('lblProductName').innerHTML = strProductName;
                document.getElementById('txt_SalesAmount').innerHTML = QuantityValue;
                document.getElementById('txt_SalesUOM').innerHTML = strUOM;
                document.getElementById('txt_StockAmount').innerHTML = StkQuantityValue;
                document.getElementById('txt_StockUOM').innerHTML = strStkUOM;

                $('#hdfProductID').val(strProductID);
                $('#hdfProductSerialID').val(SrlNo);
                $('#hdfProductSerialID').val(SrlNo);
                $('#hdnProductQuantity').val(QuantityValue);
                //cacpAvailableStock.PerformCallback(strProductID);

                if (Ptype == "W") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'none';
                    div_Serial.style.display = 'none';
                    div_Quantity.style.display = 'block';
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "block");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "B") {
                    div_Warehouse.style.display = 'none';
                    div_Batch.style.display = 'block';
                    div_Serial.style.display = 'none';
                    div_Quantity.style.display = 'block';
                    cCmbBatch.PerformCallback('BindBatch~' + "0");
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "block");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "S") {
                    div_Warehouse.style.display = 'none';
                    div_Batch.style.display = 'none';
                    div_Serial.style.display = 'block';
                    div_Quantity.style.display = 'none';
                    checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "none");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "WB") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'block';
                    div_Serial.style.display = 'none';
                    div_Quantity.style.display = 'block';
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "block");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "WS") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'none';
                    div_Serial.style.display = 'block';
                    div_Quantity.style.display = 'none';
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "none");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "WBS") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'block';
                    div_Serial.style.display = 'block';
                    div_Quantity.style.display = 'none';
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "none");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else if (Ptype == "BS") {
                    div_Warehouse.style.display = 'none';
                    div_Batch.style.display = 'block';
                    div_Serial.style.display = 'block';
                    div_Quantity.style.display = 'none';
                    cCmbBatch.PerformCallback('BindBatch~' + "0");
                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                    $("#ADelete").css("display", "none");//Subhabrata
                    SelectedWarehouseID = "0";
                    cPopup_Warehouse.Show();
                }
                else {
                    //jAlert("No Warehouse or Batch or Serial is actived !", 'Alert Dialog: [SalesInvoice]', function (r) {
                    //    if (r == true) {
                    //        grid.batchEditApi.StartEdit(index, 8);
                    //    }
                    //});

                    jAlert("No Warehouse or Batch or Serial is actived !");
                }
            }
            else if (ProductID != "" && parseFloat(QuantityValue) == 0) {
                //jAlert("Please enter Quantity !", 'Alert Dialog: [SalesInvoice]', function (r) {
                //    if (r == true) {
                //        grid.batchEditApi.StartEdit(index, 8);
                //    }
                //});

                jAlert("Please enter Quantity !");
            }
        }
        else {
            //jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.", 'Alert Dialog: [SalesInvoice]', function (r) {
            //    if (r == true) {
            //        grid.batchEditApi.StartEdit(index, 8);
            //    }
            //});

            jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
        }
    }
}



function AutoPopulateMultiUOM() {
  
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/AutoPopulateAltQuantity",
        data: JSON.stringify({ ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            if (msg.d.length != 0) {
                var packingQuantity = msg.d[0].packing_quantity;
                var sProduct_quantity = msg.d[0].sProduct_quantity;
                var AltUOMId = msg.d[0].AltUOMId;
            }
            else {
                var packingQuantity = 0;
                var sProduct_quantity = 0;
                var AltUOMId = 0;
            }
            var uomfactor = 0
            if (sProduct_quantity != 0 && packingQuantity != 0) {
                uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = QuantityValue;
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);
            if ($("#hdnPageStatus").val() == "update") {
                //ccmbSecondUOM.SetValue(AltUOMId);
                //$("#AltUOMQuantity").val(calcQuantity);

                //cAltUOMQuantity.SetValue(calcQuantity);
                cAltUOMQuantity.SetValue("0.0000");
            }
            else {
                 ccmbSecondUOM.SetValue(AltUOMId);
                if (AltUOMId == 0) {
                    ccmbSecondUOM.SetValue('');
                }
                else {
                    // Mantis Issue 24425, 24428
                    //ccmbSecondUOM.SetValue(AltUOMId);
                    // End of Mantis Issue 24425, 24428
                }
                // Mantis Issue 24425, 24428
                //cAltUOMQuantity.SetValue(calcQuantity);
                // End of Mantis Issue 24425, 24428
            }

        }
    });
}





function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');
    //Rev Subhra 15-05-2019
    // grid.batchEditApi.StartEdit(globalRowIndex, 10);
    //End of Rev Subhra 15-05-2019
    // if ($("#hdnProjectSelectInEntryModule").val() == "0") {
    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 11); }, 600);

    //}
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');//Subhabrata
}

function OnWarehouseEndCallback(s, e) {
    var Ptype = document.getElementById('hdfProductType').value;

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 11);
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Sales Quantity must be equal to Warehouse Quantity.');
    }
    else {
        if (document.getElementById("myCheck").checked == true) {
            if (IsPostBack == "N") {
                checkListBox.PerformCallback('BindSerial~' + PBWarehouseID + '~' + PBBatchID);

                IsPostBack = "";
                PBWarehouseID = "";
                PBBatchID = "";
            }

            if (Ptype == "W" || Ptype == "WB") {
                cCmbWarehouse.Focus();
            }
            else if (Ptype == "B") {
                cCmbBatch.Focus();
            }
            else {
                ctxtserial.Focus();
            }
        }
        else {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "WS" || Ptype == "WBS") {
                cCmbWarehouse.Focus();
            }
            else if (Ptype == "B" || Ptype == "BS") {
                cCmbBatch.Focus();
            }
            else if (Ptype == "S") {
                checkComboBox.Focus();
            }
        }
    }
    //Rev Subhra 15-05-2019
    //grid.batchEditApi.StartEdit(globalRowIndex, 11);
    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 11); }, 1000);
    //End of Rev Subhra 15-05-2019
}

var SelectWarehouse = "0";
var SelectBatch = "0";
var SelectSerial = "0";
var SelectedWarehouseID = "0";

function CallbackPanelEndCall(s, e) {
    if (cCallbackPanel.cpEdit != null) {
        var strWarehouse = cCallbackPanel.cpEdit.split('~')[0];
        var strBatchID = cCallbackPanel.cpEdit.split('~')[1];
        var strSrlID = cCallbackPanel.cpEdit.split('~')[2];
        var strQuantity = cCallbackPanel.cpEdit.split('~')[3];

        SelectWarehouse = strWarehouse;
        SelectBatch = strBatchID;
        SelectSerial = strSrlID;

        cCmbWarehouse.PerformCallback('BindWarehouse');
        cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
        checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

        cCmbWarehouse.SetValue(strWarehouse);
        ctxtQuantity.SetValue(strQuantity);
    }
}



function CmbWarehouseEndCallback(s, e) {
    if (SelectWarehouse != "0") {
        cCmbWarehouse.SetValue(SelectWarehouse);
        SelectWarehouse = "0";
    }
    else {
        cCmbWarehouse.SetEnabled(true);
    }
}

function CmbBatchEndCall(s, e) {
    if (SelectBatch != "0") {
        cCmbBatch.SetValue(SelectBatch);
        SelectBatch = "0";
    }
    else {
        cCmbBatch.SetEnabled(true);
    }
}

function listBoxEndCall(s, e) {
    if (SelectSerial != "0") {
        var values = [SelectSerial];
        checkListBox.SelectValues(values);
        UpdateSelectAllItemState();
        UpdateText();
        //checkListBox.SetValue(SelectWarehouse);
        SelectSerial = "0";
        cCmbBatch.SetEnabled(false);
        cCmbWarehouse.SetEnabled(false);
    }
}

function Save_TaxesClick() {
    grid.batchEditApi.EndEdit();
    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

    cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
        var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
        var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
        var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
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
            var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
            var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
            var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
            var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
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
    ctxtProductNetAmount.SetValue(sumNetAmount.toFixed(2));
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
                    ctxtProductNetAmount.SetText((sumNetAmount / gstDis).toFixed(2));
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

        //Debjyoti 09032017
        for (var cmbCount = 1; cmbCount < ccmbGstCstVatcharge.GetItemCount() ; cmbCount++) {
            if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[5] == '19') {
                if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'I') {
                    ccmbGstCstVatcharge.RemoveItem(cmbCount);
                    cmbCount--;
                }
            } else {
                if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'C') {
                    ccmbGstCstVatcharge.RemoveItem(cmbCount);
                    cmbCount--;
                }
            }
        }






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

var chargejsonTax;
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

    //else
    //{
    //     chargejsonTax == "undefined";
    //    //chargejsonTax.length == 0;
    //}


    //Set Total Charges And total Amount
    if (gridTax.cpTotalCharges) {
        if (gridTax.cpTotalCharges != "") {
            chargejsonTax = JSON.parse(gridTax.cpJsonChargeData);
            ctxtQuoteTaxTotalAmt.SetValue(gridTax.cpTotalCharges);
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
            gridTax.cpTotalCharges = null;
        }
    }
    //  chargejsonTax = JSON.parse(gridTax.cpJsonChargeData);
    SetChargesRunningTotal();
    ShowTaxPopUp("IN");
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
        //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
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

    ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}

//Set Running Total for Charges And Tax 
function SetChargesRunningTotal() {
    var runningTot = parseFloat(ctxtProductNetAmount.GetValue());

    if (typeof (chargejsonTax) != "undefined") {
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
}


/////////////////// QuotationTaxAmountTextChange By Sam on 23022017
var taxAmountGlobalCharges;
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}


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
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }

    RecalCulateTaxTotalAmountCharges();

}



////////////

var AmountOldValue;
var AmountNewValue;

function AmountTextChange(s, e) {
    AmountLostFocus(s, e);
    var RecieveValue = (grid.GetEditor('Amount').GetValue() != null) ? parseFloat(grid.GetEditor('Amount').GetValue()) : "0";
}

function AmountLostFocus(s, e) {
    AmountNewValue = s.GetText();
    var indx = AmountNewValue.indexOf(',');

    if (indx != -1) {
        AmountNewValue = AmountNewValue.replace(/,/g, '');
    }
    if (AmountOldValue != AmountNewValue) {
        changeReciptTotalSummary();
    }
}

function AmountGotFocus(s, e) {
    AmountOldValue = s.GetText();
    var indx = AmountOldValue.indexOf(',');
    if (indx != -1) {
        AmountOldValue = AmountOldValue.replace(/,/g, '');
    }
}

function changeReciptTotalSummary() {
    var newDif = AmountOldValue - AmountNewValue;
    var CurrentSum = ctxtSumTotal.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    ctxtSumTotal.SetValue(parseFloat(CurrentSum - newDif));
}

function CmbWarehouse_ValueChange() {
    var WarehouseID = cCmbWarehouse.GetValue();
    var type = document.getElementById('hdfProductType').value;

    if (type == "WBS" || type == "WB") {
        cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
    }
    else if (type == "WS") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
    }
}
function CmbBatch_ValueChange() {
    var WarehouseID = cCmbWarehouse.GetValue();
    var BatchID = cCmbBatch.GetValue();
    var type = document.getElementById('hdfProductType').value;

    if (type == "WBS") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
    }
    else if (type == "BS") {
        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID);
    }
}
function SaveWarehouse() {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var WarehouseName = cCmbWarehouse.GetText();
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";
    var BatchName = cCmbBatch.GetText();
    var SerialID = "";
    var SerialName = "";
    var Qty = ctxtQuantity.GetValue();

    var items = checkListBox.GetSelectedItems();
    var vals = [];
    var texts = [];

    for (var i = 0; i < items.length; i++) {
        if (items[i].index != 0) {
            if (i == 0) {
                SerialID = items[i].value;
                SerialName = items[i].text;
            }
            else {
                if (SerialID == "" && SerialID == "") {
                    SerialID = items[i].value;
                    SerialName = items[i].text;
                }
                else {
                    SerialID = SerialID + '||@||' + items[i].value;
                    SerialName = SerialName + '||@||' + items[i].text;
                }
            }
            //texts.push(items[i].text);
            //vals.push(items[i].value);
        }
    }

    //WarehouseID, BatchID, SerialID, Qty=0.0
    $("#spnCmbWarehouse").hide();
    $("#spnCmbBatch").hide();
    $("#spncheckComboBox").hide();
    $("#spntxtQuantity").hide();

    var Ptype = document.getElementById('hdfProductType').value;
    if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
        $("#spnCmbWarehouse").show();
    }
    else if ((Ptype == "B" && BatchID == "0") || (Ptype == "WB" && BatchID == "0") || (Ptype == "WBS" && BatchID == "0") || (Ptype == "BS" && BatchID == "0")) {
        $("#spnCmbBatch").show();
    }
    else if ((Ptype == "W" && Qty == "0.0") || (Ptype == "B" && Qty == "0.0") || (Ptype == "WB" && Qty == "0.0")) {
        $("#spntxtQuantity").show();
    }
    else if ((Ptype == "S" && SerialID == "") || (Ptype == "WS" && SerialID == "") || (Ptype == "WBS" && SerialID == "") || (Ptype == "BS" && SerialID == "")) {
        $("#spncheckComboBox").show();
    }
    else {
        if (document.getElementById("myCheck").checked == true && SelectedWarehouseID == "0") {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "B") {
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + "");
                checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
                ctxtQuantity.SetValue("0");
            }
            else {
                IsPostBack = "N";
                PBWarehouseID = WarehouseID;
                PBBatchID = BatchID;
            }
        }
        else {
            cCmbWarehouse.PerformCallback('BindWarehouse');
            cCmbBatch.PerformCallback('BindBatch~' + "");
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
        SelectedWarehouseID = "0";
    }
}

var IsPostBack = "";
var PBWarehouseID = "";
var PBBatchID = "";


$(document).ready(function () {
    

    $('#ddl_VatGstCst_I').blur(function () {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    })
    $('#ddl_AmountAre').blur(function () {
        var id = cddl_AmountAre.GetValue();
        if (id == '1' || id == '3') {
            if (grid.GetVisibleRowsOnPage() == 1) {
                grid.batchEditApi.StartEdit(-1, 2);
            }
        }
    })


});

function deleteAllRows() {
    AllowAddressShipToPartyState = true;
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
        grid.DeleteRow(frontRow);
        grid.DeleteRow(backRow);
        backRow--;
        frontRow++;
    }
    OnAddNewClick();
}
function txtserialTextChanged() {
    checkListBox.UnselectAll();
    var SerialNo = (ctxtserial.GetValue() != null) ? (ctxtserial.GetValue()) : "0";

    if (SerialNo != "0") {
        ctxtserial.SetValue("");
        var texts = [SerialNo];
        var values = GetValuesByTexts(texts);

        if (values.length > 0) {
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
            SaveWarehouse();
        }
        else {
            jAlert("This Serial Number does not exists.");
        }
    }
}

function AutoCalculateMandateOnChange(element) {
    $("#spnCmbWarehouse").hide();
    $("#spnCmbBatch").hide();
    $("#spncheckComboBox").hide();
    $("#spntxtQuantity").hide();

    if (document.getElementById("myCheck").checked == true) {
        divSingleCombo.style.display = "block";
        divMultipleCombo.style.display = "none";

        checkComboBox.Focus();
    }
    else {
        divSingleCombo.style.display = "none";
        divMultipleCombo.style.display = "block";

        ctxtserial.Focus();
    }
}

function fn_Deletecity(keyValue) {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
}
function fn_Edit(keyValue) {
    //cGrdWarehouse.PerformCallback('EditWarehouse~' + keyValue);
    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}

// <![CDATA[
var textSeparator = ";";
var selectedChkValue = "";

function OnListBoxSelectionChanged(listBox, args) {
    if (args.index == 0)
        args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
    UpdateSelectAllItemState();
    UpdateText();

    //Added Subhabrata
    var selectedItems = checkListBox.GetSelectedItems();
    var val = GetSelectedItemsText(selectedItems);
    var strWarehouse = cCmbWarehouse.GetValue();
    var strBatchID = cCmbBatch.GetValue();
    var ProducttId = $("#hdfProductID").val();

    //$.ajax({
    //    type: "POST",
    //    url: "SalesInvoice.aspx/GetSerialId",
    //    data: JSON.stringify({
    //        "id": val,
    //        "wareHouseStr": strWarehouse,
    //        "BatchID": strBatchID,
    //        "ProducttId": ProducttId
    //    }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: false,//Added By:Subhabrata
    //    success: function (msg) {

    //        var type = msg.d;
    //        if (type == "1") {

    //            return true;
    //        }
    //        else if (type == "0") {
    //            alert("Serial No can be Stock out based on FIFO process.Select the Serial No. shown from Oldest to Newest sequence to proceed");
    //            //listBox.UnselectAll();

    //            var indices = [];
    //            //Added By:Subhabrata
    //            if ((selectedItems.length * 1) == 1) {
    //                indices.push(listBox.GetItem(args.index));
    //                listBox.UnselectIndices(indices[0].text);
    //                UpdateSelectAllItemState();
    //                UpdateText();
    //            }
    //            if (((args.index) * 1) <= (selectedItems.length * 1)) {
    //                for (var i = ((args.index) * 1) ; i <= ((selectedItems.length * 1) + 1) ; i++) {
    //                    indices.push(listBox.GetItem(i));

    //                }
    //            }
    //            else {
    //                indices.push(listBox.GetItem(args.index));
    //                listBox.UnselectIndices(indices[0].text);
    //                UpdateSelectAllItemState();
    //                UpdateText();
    //            }

    //            for (var j = 0; j < indices.length   ; j++) {
    //                listBox.UnselectIndices(indices[j].text);
    //                UpdateSelectAllItemState();
    //                UpdateText();
    //            }
    //        }
    //    }
    //});

    //End
}
function UpdateSelectAllItemState() {
    IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
}
function IsAllSelected() {
    var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
    return checkListBox.GetSelectedItems().length == selectedDataItemCount;
}
function UpdateText() {
    var selectedItems = checkListBox.GetSelectedItems();
    selectedChkValue = GetSelectedItemsText(selectedItems);
    //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
    //checkComboBox.SetText(selectedItems.length + " Items");

    var itemsCount = GetSelectedItemsCount(selectedItems);
    checkComboBox.SetText(itemsCount + " Items");

    var val = GetSelectedItemsText(selectedItems);
    $("#abpl").attr('data-content', val);
}
function SynchronizeListBoxValues(dropDown, args) {
    checkListBox.UnselectAll();
    // var texts = dropDown.GetText().split(textSeparator);
    var texts = selectedChkValue.split(textSeparator);

    var values = GetValuesByTexts(texts);
    checkListBox.SelectValues(values);
    UpdateSelectAllItemState();
    UpdateText(); // for remove non-existing texts
}
function GetSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.join(textSeparator);
}
function GetSelectedItemsCount(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.length;
}
function GetValuesByTexts(texts) {
    var actualValues = [];
    var item;
    for (var i = 0; i < texts.length; i++) {
        item = checkListBox.FindItemByText(texts[i]);
        if (item != null)
            actualValues.push(item.value);
    }
    return actualValues;
}
$(function () {
    $('[data-toggle="popover"]').popover();
})
// ]]>

var Pre_TotalAmt = "0";

function DiscountGotFocus(s, e) {
    var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
    var _Quantity = (grid.GetEditor('Quantity').GetText() != null) ? grid.GetEditor('Quantity').GetText() : "0";
    var _Pre_Price = (grid.GetEditor('SalePrice').GetText() != null) ? grid.GetEditor('SalePrice').GetText() : "0";
    var _Discount = (grid.GetEditor('Discount').GetText() != null) ? grid.GetEditor('Discount').GetText() : "0";

    Pre_TotalAmt = _Amount;
    Pre_Qty = _Quantity;
    Pre_Price = _Pre_Price;
    Pre_Discount = _Discount;
}

function ProductsGotFocus(s, e) {

    //Mantis 24428 cbtn_SaveRecords_N,cbtn_SaveRecords_p SetVisible false comment because of wrong behaviour of this two button
    //cbtn_SaveRecords_N.SetVisible(false);
   // cbtn_SaveRecords_p.SetVisible(false);
    //End Mantis 24428
    pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
    //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    var strProductShortCode = SpliteDetails[14];
    var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

    strProductName = strDescription;

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strProductName);
    $('#lblbranchName').text(strBranch);


    var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
    var _Quantity = (grid.GetEditor('Quantity').GetText() != null) ? grid.GetEditor('Quantity').GetText() : "0";
    var _Pre_Price = (grid.GetEditor('SalePrice').GetText() != null) ? grid.GetEditor('SalePrice').GetText() : "0";
    var _Discount = (grid.GetEditor('Discount').GetText() != null) ? grid.GetEditor('Discount').GetText() : "0";

    Pre_TotalAmt = _Amount;
    Pre_Qty = _Quantity;
    Pre_Price = _Pre_Price;
    Pre_Discount = _Discount;

    //if (ProductID != "0") {
    //   cacpAvailableStock.PerformCallback(strProductID);
    //}


}

function QuantityGotFocus(s, e) {

    cbtn_SaveRecords_N.SetVisible(false);
    cbtn_SaveRecords_p.SetVisible(false);
    ProductsGotFocus(s, e);


    //Surojit 25-02-2019
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    var strProductShortCode = SpliteDetails[14];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    strProductName = strDescription;

    var isOverideConvertion = SpliteDetails[26];
    var packing_saleUOM = SpliteDetails[25];
    var sProduct_SaleUom = SpliteDetails[24];
    var sProduct_quantity = SpliteDetails[22];
    var packing_quantity = SpliteDetails[20];

    var slno = (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

    var ComponentNumber = (grid.GetEditor('ComponentNumber').GetText() != null) ? grid.GetEditor('ComponentNumber').GetText() : "0";

    var rdl_SaleInvoice = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';
    var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
    var gridPackingQty = '';
    var IsInventory = '';
    //var gridPackingQty = grid.GetEditor('QuoteDetails_PackingQty').GetText();
    if (SpliteDetails.length > 27) {
        if (SpliteDetails[27] == "1") {
            IsInventory = 'Yes';

            type = 'edit';

            if (SpliteDetails[28] != '') {
                if (parseFloat(SpliteDetails[28]) > 0) {
                    gridPackingQty = SpliteDetails[28];
                }
                else {
                    type = 'add';
                }
            }
            else {
                type = 'add';
            }

            var actionQry = '';


            if (ComponentNumber != "0" && ComponentNumber != "" && $('#hdnPageStatus').val() != "update") {

                if (rdl_SaleInvoice == 'SO') {
                    actionQry = 'SalesInvoicePackingQtyOrder';
                }
                if (rdl_SaleInvoice == 'QO') {
                    actionQry = 'SalesInvoicePackingQtyQuotation';
                }

                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({ orderid: strProductID, action: actionQry, module: 'SalesInvoice', strKey: ComponentNumber }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        gridPackingQty = msg.d;
                        //type = 'edit';
                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                            if ($("#hddnMultiUOMSelection").val() == "0") {

                                ShowUOM(type, "SalesInvoice", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);

                                //chinmoy added for tagging from Sales Order start
                                var productidget = strProductID;
                                //$('#hdnProductID').val();
                                var slnoget = slno;
                                //$('#hdnSLNO').val();
                                var Quantity = $('#txtQuantity').val();
                                var packing = $('#txtPacking').val();
                                if (Quantity == null || Quantity == '') {
                                    Quantity = '0.0000';

                                }
                                if (packing == null || packing == '') {
                                    packing = '0.0000';
                                }

                                var ebj = {};
                                var PackingUom = ccmbPackingUom.GetValue();
                                var PackingSelectUom = ccmbPackingSelectUom.GetValue();

                                for (i = 0; i < aarr.length; i++) {
                                    ebj = aarr[i];
                                    console.log(ebj);
                                    if (ebj.slno == slnoget && ebj.productid == productidget) {
                                        //aarr.pop(extobj);
                                        aarr.splice(i, 1);
                                    }
                                    ebj = {};
                                }


                                var arbj = {};
                                arbj.productid = productidget;
                                arbj.slno = slnoget;
                                arbj.Quantity = Quantity;
                                arbj.packing = packing;
                                arbj.PackingUom = PackingUom;
                                arbj.PackingSelectUom = PackingSelectUom;

                                aarr.push(arbj);
                                //End
                            }
                        }
                    }
                });


            }

            else if (ComponentNumber != "0" && ComponentNumber != "" && $('#hdnPageStatus').val() == "update") {

                //if (rdl_SaleInvoice == 'SO') {
                //    actionQry = 'SalesInvoicePackingQtyOrder';
                //}
                //if (rdl_SaleInvoice == 'QO') {
                //    actionQry = 'SalesInvoicePackingQtyQuotation';
                //}
                actionQry = 'SalesInvoicePackingQtyProductId';
                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({ orderid: strProductID, action: actionQry, module: 'SalesInvoice', strKey: ComponentNumber }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        gridPackingQty = msg.d;
                        //type = 'edit';
                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                            if ($("#hddnMultiUOMSelection").val() == "0") {
                                ShowUOM(type, "SalesInvoice", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);

                                //chinmoy added for tagging from Sales Order start

                                var productidget = strProductID;
                                //$('#hdnProductID').val();
                                var slnoget = slno;
                                //$('#hdnSLNO').val();
                                var Quantity = $('#txtQuantity').val();
                                var packing = $('#txtPacking').val();
                                if (Quantity == null || Quantity == '') {
                                    Quantity = '0.0000';

                                }
                                if (packing == null || packing == '') {
                                    packing = '0.0000';
                                }

                                var ebj = {};
                                var PackingUom = ccmbPackingUom.GetValue();
                                var PackingSelectUom = ccmbPackingSelectUom.GetValue();

                                for (i = 0; i < aarr.length; i++) {
                                    ebj = aarr[i];
                                    console.log(ebj);
                                    if (ebj.slno == slnoget && ebj.productid == productidget) {
                                        //aarr.pop(extobj);
                                        aarr.splice(i, 1);
                                    }
                                    ebj = {};
                                }


                                var arbj = {};
                                arbj.productid = productidget;
                                arbj.slno = slnoget;
                                arbj.Quantity = Quantity;
                                arbj.packing = packing;
                                arbj.PackingUom = PackingUom;
                                arbj.PackingSelectUom = PackingSelectUom;

                                aarr.push(arbj);
                                //End



                            }
                        }
                    }
                });


            }

            else {
                actionQry = 'SalesInvoicePackingQtyProductId';
                var orderid = grid.GetRowKey(globalRowIndex);
                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({ orderid: orderid, action: actionQry, module: 'SalesInvoice', strKey: '' }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        gridPackingQty = msg.d;
                        //type = 'edit';
                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                            if ($("#hddnMultiUOMSelection").val() == "0") {
                                ShowUOM(type, "SalesInvoice", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }
                        }
                    }
                });
            }

        }
    }
    else {

        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
            if ($("#hddnMultiUOMSelection").val() == "0") {
                ShowUOM(type, "SalesInvoice", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
            }
        }
    }
    //Surojit 25-02-2019


    //chinmoy added for  for MultiUOM start
    if ($("#hddnMultiUOMSelection").val() == "1") {
        grid.batchEditApi.StartEdit(globalRowIndex, 8);
        // if ((gridquotationLookup.GetValue() != "") && (gridquotationLookup.GetValue() !=null)) {
        if (grid.GetEditor('Quantity').GetValue() != "0.0000") {
            grid.batchEditApi.StartEdit(globalRowIndex, 8);
            //Rev Mantis 24428 
           // $("#UOMQuantity").val(grid.GetEditor('Quantity').GetValue());
            //End Rev MAntis 24428
        }
        // }
    }

    //End
}

var issavePacking = 0;


$(function () {
    $('#UOMModal').on('hide.bs.modal', function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    });
});


function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {

    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('Quantity').SetValue(Quantity);
    QuantityTextChange();

    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 8);
    }, 600);


}

function ProductsGotFocusFromID(s, e) {
    pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    if (ProductID != "" && ProductID != null) {
        var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
        //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();

        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];
        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var strProductShortCode = SpliteDetails[14];
        var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

        strProductName = strDescription;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
            divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }

        $('#lblStkQty').text(QuantityValue);
        $('#lblStkUOM').text(strStkUOM);
        $('#lblProduct').text(strProductName);
        $('#lblbranchName').text(strBranch);

        if (ProductID != "0") {
            //cacpAvailableStock.PerformCallback(strProductID);
            setStock(strProductID);
        }
    }
}



function ProductKeyDown(s, e) {
    //console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {

        s.OnButtonClick(0);
    }
    if (e.htmlEvent.key == "Tab") {

        s.OnButtonClick(0);
    }
}

function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {
        setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

        if (!GetObjectID('hdnCustomerId').value) {
            jAlert("Please Select Customer first.", "Alert", function () { $('#txtCustSearch').focus(); })
            return;
        }
        $('#txtProdSearch').val('');
        $('#ProductModel').modal('show');


        //if (cproductLookUp.Clear()) {
        //cProductpopUp.Show();
        //cproductLookUp.Focus();                     
        //}
    }
}

function SetProduct(Id, Name) {
    $('#ProductModel').modal('hide');
    var LookUpData = Id;
    var ProductCode = Name;

    if (!ProductCode) {
        LookUpData = null;
    }
    //cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    //  console.log(LookUpData);
    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);
    ctxtCustName.SetEnabled(false);
    // BillShipAddressVisible();
    cContactPerson.PerformCallback('BindContactPerson~' + key);
    //Chinmoy added this variable
    AllowAddressShipToPartyState = false;
    $('#openlink').hide();
    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");

    //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];

    var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
    if (strRate == 0) {
        strSalePrice = strSalePrice;
    }
    else {
        strSalePrice = strSalePrice / strRate;
    }

    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbSalePrice.SetValue(strSalePrice);

    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strDescription);
    $('#lblbranchName').text(strBranch);
    //Rev work start 28.06.2022 Mantise no:24949
    //GetSalesRateSchemePrice($("#hdnCustomerId").val(), strProductID, "0");
    if ($('#hdnSettings').val() == "YES") {
        GetSalesRateSchemePrice($("#hdnCustomerId").val(), strProductID, "0");
    }
    //Rev work close 28.06.2022 Mantise no:24949

    var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;
    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }

    //divPacking.style.display = "none";
    //lblbranchName lblProduct
    //tbStkUOM.SetValue(strStkUOM);
    //tbStockQuantity.SetValue("0");

    var IsComponentProduct = SpliteDetails[15];
    var ComponentProduct = SpliteDetails[16];

    document.getElementById("ddlInventory").disabled = true;
    //Rev Rajdip
    var hdnPlaceShiptoParty = $("#hdnPlaceShiptoParty").val();
    //cddl_PosGst.SetEnabled(false);
    if (hdnPlaceShiptoParty == "1")
    { cddl_PosGst.SetEnabled(true); }
    else
    { cddl_PosGst.SetEnabled(false); }
    //End Rev Rajdip
    setStock(strProductID);

    var strSalePrice = SpliteDetails[6];
    var sProduct_PurPrice = SpliteDetails[13];
    var sProduct_MinSalePrice = SpliteDetails[17];
    var sProduct_MRP = SpliteDetails[18];
    var sProduct_Cost = SpliteDetails[28];

    $('#lblSell').text(strSalePrice);
    $('#lblMRP').text(sProduct_MRP);
    $('#lblPurchase').text(sProduct_PurPrice);
    $('#lblCost').text(sProduct_Cost);

    if ($('#hdnPricingDetail').val() == "1") {
        $('#DivSell').css({ 'display': 'block' });
        $('#DivMRP').css({ 'display': 'block' });
        $('#DivPurchase').css({ 'display': 'block' });
        $('#DivCost').css({ 'display': 'block' });
    }
    if ($('#hdnStockPositionShow').val() == "1") {
        $('#DivStockPosition').css({ 'display': 'block' });        
    }


    //cacpAvailableStock.PerformCallback(strProductID);
    //Debjyoti
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);


    $('#hdnProductID').val(strProductID);


    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), strProductID);
    grid.batchEditApi.StartEdit(globalRowIndex, 4);
    //Rev work start 28.06.2022 Mantise no:24949
    //setTimeout(function () {
    //    grid.batchEditApi.StartEdit(globalRowIndex, 4);
    //    //if ($("#ProductMinPrice").val() != "") {
    //    //    grid.GetEditor("SalePrice").SetValue($("#ProductMinPrice").val());
    //    //}
        
    //    if ($("#hdnRateType").val() == "3") {
    //        if ($("#hdnProductDaynamicRate").val() != "") {
    //            grid.GetEditor("SalePrice").SetValue($("#hdnProductDaynamicRate").val());
    //        }
    //    }
    //}, 200);
    setTimeout(function () {
        if ($("#ProductMinPrice").val() != "") {
            grid.GetEditor("SalePrice").SetValue($("#ProductMinPrice").val());
        }
    }, 200);
    //Rev work close 28.06.2022 Mantise no:24949
    SetTotalTaxableAmount(globalRowIndex, 4);
}
//Rev work start 28.06.2022 Mantise no:24949
function setValueOfSAale() {
    if ($("#ProductMinPrice").val() != "") {
        grid.GetEditor("SalePrice").SetValue($("#ProductMinPrice").val());
    }
}
//Rev work close 28.06.2022 Mantise no:24949
function AssignedBranchSelectedIndexChanged() {
    cAssignedWareHouse.PerformCallback(cAssignedBranch.GetValue());

    cBranchAssignmentBranch.SetValue(cAssignedBranch.GetValue());
    //updateAssignmentGrid();
}
function BranchAssignmentBranchSelectedIndexChanged() {
    cAssignedBranch.SetValue(cBranchAssignmentBranch.GetValue());
    //    AssignedBranchSelectedIndexChanged(cBranchAssignmentBranch);
    cAssignedWareHouse.PerformCallback(cAssignedBranch.GetValue());
}
function updateAssignmentGrid() {
    cAssignmentGrid.PerformCallback(cBranchAssignmentBranch.GetValue() + '~' + $('#hdnProductID').val());
}
function setStock(productid) {
    var OtherDetail = {};
    OtherDetail.prodid = productid;
    OtherDetail.branch = $("#ddl_Branch").val();

    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/acpAvailableStock_Callback",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var Code = msg.d;

            if (Code != null) {
                divAvailableStk.style.display = "block";
                divpopupAvailableStock.style.display = "block";

                var AvlStk = Code + " " + document.getElementById('lblStkUOM').innerHTML;
                document.getElementById('lblAvailableStk').innerHTML = AvlStk;
                document.getElementById('lblAvailableStock').innerHTML = Code;
                document.getElementById('lblAvailableStockUOM').innerHTML = document.getElementById('lblStkUOM').innerHTML;


            }
        }
    });
}

function deleteTax(Action, srl, productid) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;
    OtherDetail.prodid = productid;


    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/taxUpdatePanel_Callback",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var Code = msg.d;

            if (Code != null) {

            }
        }
    });
}



function ddlBranch_ChangeIndex() {

    var inventory = '';
    var isinventory = $('#ddlInventory').val()

    if (isinventory == 'Y')
        inventory = 1
    else

        inventory = 0

    if (gridquotationLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                var startDate = new Date();
                startDate = tstartdate.GetValueString();
                var key = GetObjectID('hdnCustomerId').value;
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                var componentType = gridquotationLookup.GetValue();// gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                if (key != null && key != '' && type != "") {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + inventory + '~' + isinventory);
                }

                if (componentType != null && componentType != '') {
                    grid.PerformCallback('GridBlank');
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax('DeleteAllTax', "", "")
                }
            }
        });
    }
    else {
        var startDate = new Date();
        startDate = tstartdate.GetValueString();
        var key = GetObjectID('hdnCustomerId').value;
        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
        var componentType = gridquotationLookup.GetValue();// gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

        if (key != null && key != '' && type != "") {
            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + inventory + '~' + isinventory);
        }

        if (componentType != null && componentType != '') {
            grid.PerformCallback('GridBlank');
            //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            deleteTax('DeleteAllTax', "", "")
        }
    }

    

}
function DateCheck() {
    var inventory = '';
    var isinventory = $('#ddlInventory').val()

    if (isinventory == 'Y')
        inventory = 1
    else

        inventory = 0

    if (gridquotationLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
            if (r == true) {

               

                var startDate = new Date();
                startDate = tstartdate.GetValueString();
                var key = GetObjectID('hdnCustomerId').value;
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                var componentType = gridquotationLookup.GetValue();// gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                if (key != null && key != '' && type != "") {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + inventory + '~' + isinventory);
                }

                if (componentType != null && componentType != '') {
                    grid.PerformCallback('GridBlank');
                    // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax('DeleteAllTax', "", "")
                }
            }
        });
    }
    else {
        var startDate = new Date();
        startDate = tstartdate.GetValueString();
        var key = GetObjectID('hdnCustomerId').value;
        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
        var componentType = gridquotationLookup.GetValue();// gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

        if (key != null && key != '' && type != "") {
            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + inventory + '~' + isinventory);
        }

        if (componentType != null && componentType != '') {
            grid.PerformCallback('GridBlank');
            //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            deleteTax('DeleteAllTax', "", "")
        }
    }
}
function componentEndCallBack(s, e) {
    gridquotationLookup.gridView.Refresh();
    if (grid.GetVisibleRowsOnPage() == 0) {
        OnAddNewClick();
    }

    if (cQuotationComponentPanel.cpRebindGridQuote && cQuotationComponentPanel.cpRebindGridQuote != "") {
        ctxt_InvoiceDate.SetText(cQuotationComponentPanel.cpRebindGridQuote);
        cQuotationComponentPanel.cpRebindGridQuote = null;
    }


    if (cQuotationComponentPanel.cpDetails != null) {
        var details = cQuotationComponentPanel.cpDetails;
        cQuotationComponentPanel.cpDetails = null;

        var SpliteDetails = details.split("~");
        var Reference = SpliteDetails[0];
        var Currency_Id = SpliteDetails[1];
        var SalesmanId = SpliteDetails[2];
        var ExpiryDate = SpliteDetails[3];
        var CurrencyRate = SpliteDetails[4];
        var Type = SpliteDetails[5];
        var CreditDays = SpliteDetails[6];
        var DueDate = SpliteDetails[7];
        var SalesmanName = SpliteDetails[8];
        if (Type == "SO") {
            if (DueDate != "" && CreditDays != "") {
                ctxtCreditDays.SetValue(CreditDays);

                var Due_Date = new Date(DueDate);
                //cdt_SaleInvoiceDue.SetDate(Due_Date);
                
                var today = new Date();
                today = tstartdate.GetDate();
                today.setDate(today.getDate() + Math.round(CreditDays));

                cdt_SaleInvoiceDue.SetDate(today);

            }
        }
        if (ExpiryDate != "") {
            if (Type != "SO") {
                var myDate = new Date(ExpiryDate);
                var invoiceDate = new Date();
                var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));
                ctxtCreditDays.SetValue(datediff);
                cdt_SaleInvoiceDue.SetDate(myDate);
            }
        }
        ctxt_Refference.SetValue(Reference);
        ctxt_Rate.SetValue(CurrencyRate);
        document.getElementById('ddl_Currency').value = Currency_Id;
        //document.getElementById('ddl_SalesAgent').value = SalesmanId;
        $("#hdnSalesManAgentId").val(SalesmanId);
        ctxtSalesManAgent.SetValue(SalesmanName);
        
    }
}
function selectValue() {
    var startDate = new Date();
    /*--------------Arindam--------------------------*/
    var inventory = '';
    var isinventory = $('#ddlInventory').val()
    if (isinventory == 'Y')
        inventory = 1
    else
        inventory = 0
    /*--------------Arindam------------------*/
    startDate = tstartdate.GetValueString();
    var key = GetObjectID('hdnCustomerId').value;
    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

    if (type == "QO") {
        clbl_InvoiceNO.SetText('PI/Quotation Date');
    }
    else if (type == "SO") {
        clbl_InvoiceNO.SetText('Sales Order Date');
    }
    else if (type == "SC") {
        clbl_InvoiceNO.SetText('Sales Challan Date');
    }

    //cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

    if (key != null && key != '' && type != "") {
        cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + inventory + '~' + isinventory);
    }

    var componentType = gridquotationLookup.GetValue();
    if (componentType != null && componentType != '') {
        //grid.PerformCallback('GridBlank');
        deleteAllRows();
        grid.AddNewRow();
        grid.GetEditor('SrlNo').SetValue('1');
        cddl_PosGst.SetEnabled(true);
    }
}
var SimilarProjectStatus = "0";
function CloseGridQuotationLookup() {

    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();
    gridquotationLookup.Focus();

    var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

    if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {

        var quote_Id = "";
        // otherDets.quote_Id = quote_Id;
        for (var i = 0; i < quotetag_Id.length; i++) {
            if (quote_Id == "") {
                quote_Id = quotetag_Id[i];
            }
            else {
                quote_Id += ',' + quotetag_Id[i];
            }
        }
        var Doctype = $("#rdl_SaleInvoice").find(":checked").val();

        $.ajax({
            type: "POST",
            url: "SalesInvoice.aspx/DocWiseSimilarProjectCheck",
            data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                SimilarProjectStatus = msg.d;

                if (SimilarProjectStatus != "1") {
                    ctxt_InvoiceDate.SetText("");
                    jAlert("Unable to procceed. Project are for the selected Document(s) are different.");

                    return false;

                }
            }
        });
    }

}
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

function QuotationNumberChanged() {
    if (SimilarProjectStatus != "-1") {
        var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();//gridquotationLookup.GetValue();
        quote_Id = quote_Id.join();

        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

        if (quote_Id != null) {
            var arr = quote_Id.split(',');

            if (arr.length > 1) {
                if (type == "QO") {
                    ctxt_InvoiceDate.SetText('Multiple Select Quotation Dates');
                }
                else if (type == "SO") {
                    ctxt_InvoiceDate.SetText('Multiple Select Order Dates');
                }
                else if (type == "SC") {
                    ctxt_InvoiceDate.SetText('Multiple Select Challan Dates');
                }
            }
            else {
                if (arr.length == 1) {
                    cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id + '~' + type);
                }
                else {
                    ctxt_InvoiceDate.SetText('');
                }
            }
        }
        else { ctxt_InvoiceDate.SetText(''); }

        if (quote_Id != null) {
            cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
            cProductsPopup.Show();
        }
    }
}
function ChangeState(value) {

    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}

function BindOrderProjectdata(OrderId, TagDocType) {

    var OtherDetail = {};

    OtherDetail.OrderId = OrderId;
    OtherDetail.TagDocType = TagDocType;


    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "SalesInvoice.aspx/SetProjectCode",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var Code = msg.d;

                clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                clookup_Project.SetEnabled(false);
            }
        });

        //Hierarchy Start Tanmoy
        var projID = clookup_Project.GetValue();

        $.ajax({
            type: "POST",
            url: 'SalesInvoice.aspx/getHierarchyID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ProjID: projID }),
            success: function (msg) {
                var data = msg.d;
                $("#ddlHierarchy").val(data);
            }
        });
        //Hierarchy End Tanmoy
    }
}




function ShowReceiptPayment() {
    //uri = "CustomerReceiptPayment.aspx?key=ADD&IsTagged=Y";
    //capcReciptPopup.SetContentUrl(uri);
    //capcReciptPopup.Show();
}
$(document).ready(function () {
    $("#openlink").on("click", function () {
        //window.open('../master/Contact_general.aspx?id=ADD', '_blank');
        AddcustomerClick();
    });

    $('#CustModel').on('shown.bs.modal', function () {

        $('#txtCustSearch').focus();
    })

    $('#SalesManModel').on('shown.bs.modal', function () {
        $('#txtSalesManSearch').focus();
    })

    $('#ProductModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })
});

function AddcustomerClick() {
    var isLighterPage = $("#hidIsLigherContactPage").val();

    if (isLighterPage == 1) {
        var url = '/OMS/management/Master/customerPopup.html?var=1.1.3.11';
        AspxDirectAddCustPopup.SetContentUrl(url);
        //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();
        AspxDirectAddCustPopup.RefreshContentUrl();
        AspxDirectAddCustPopup.Show();
    }
    else {
        var url = '/OMS/management/Master/Customer_general.aspx';
        AspxDirectAddCustPopup.SetContentUrl(url);
        AspxDirectAddCustPopup.Show();
    }
}

function ParentCustomerOnClose(InternalID, CustomerName, UniqueName) {
    AspxDirectAddCustPopup.Hide();
    //Sudip added below code
    //setproduct ctxtCustName.SetText(CustomerName);
    GetObjectID('hdnCustomerId').value = InternalID;
    ctxtShipToPartyShippingAdd.SetText('');
    if (InternalID != "") {
        ctxtCustName.SetText(CustomerName);
        SetCustomer(InternalID, CustomerName);
    }
    //#KeyVal_InternalID").val(), $("#hdCustomerName").val(), $("#HdCustUniqueName").val());
    //gridLookup.GetGridView().Refresh();
    //cCustomerCallBackPanel.PerformCallback('SetCustomer~' + newCustId);
}
function CustomerCallBackPanelEndCallBack() {
    GetContactPerson();
}


//Code for UDF Control 
function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SI&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}
// End Udf Code

function disp_prompt(name) {

    if (name == "tab0") {
        ctxtCustName.Focus();
        page.GetTabByName('Billing/Shipping').SetEnabled(true);
        //page.GetTabByName('[B]illing/Shipping').Readonly = false;
        $("#divcross").show();
        //alert(name);
        //document.location.href = "SalesQuotation.aspx?";
    }
    if (name == "tab1") {
        $("#divcross").hide();
        var custID = GetObjectID('hdnCustomerId').value;
        page.GetTabByName('General').SetEnabled(false);
        // page.GetTabByName('General').Readonly=true;
        if (custID == null && custID == '') {
            jAlert('Please select a customer');
            page.SetActiveTabIndex(0);
            return;
        }
        else {
            page.SetActiveTabIndex(1);
            //fn_PopOpen();
        }
    }
}


function CRP_SaveANDExit_Press() {
    window.location.assign("SalesInvoiceList.aspx");
}

function CRP_SaveANDNew_Press() {
    window.location.assign("SalesInvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
}

function CustomerButnClick(s, e) {
    $('#CustModel').modal('show');
}

function Customer_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#CustModel').modal('show');
    }
}

function Customerkeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    OtherDetails.BranchID = $('#ddl_Branch').val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");

        if ($("#txtCustSearch").val() != "") {
            callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}

function SetCustomer(Id, Name) {


    if (Id != null && Id != '') {

        cContactPerson.PerformCallback('BindContactPerson~' + Id);


        var OtherDetail = {};
        OtherDetail.CustomerID = Id;
        $.ajax({
            type: "POST",
            url: "SalesReturn.aspx/GetCustomerStateCode",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                StateCodeList = msg.d;
                if (StateCodeList.length > 0) {


                    if (StateCodeList[0].TransactionType != "") {
                        $("#drdTransCategory").val(StateCodeList[0].TransactionType);

                        if (($("#drdTransCategory").val() == "B2B") || ($("#drdTransCategory").val() == "SEZWP") || ($("#drdTransCategory").val() == "SEZWOP")) {
                            var a = document.getElementById("CB_ReverseCharge");
                            a.disabled = false;
                        }
                        else {

                            document.getElementById("CB_ReverseCharge").checked = false
                            var a = document.getElementById("CB_ReverseCharge");
                            a.disabled = true;
                        }

                    }
                }

            },
            error: function (msg) {
                jAlert('Please try again later');
            }
        });

        $.ajax({
            type: "POST",
            url: "SalesInvoice.aspx/GetEinvoiceBranch",
            data: JSON.stringify({ BranchId: $('#ddl_Branch').val() }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                $("#hdnIsBranchEInvoice").val(msg.d);
                if (msg.d == "True") {
                    if (($("#drdTransCategory").val() == "B2B") || ($("#drdTransCategory").val() == "SEZWP") || ($("#drdTransCategory").val() == "SEZWOP") || ($("#drdTransCategory").val() == "EXPWP") || ($("#drdTransCategory").val() == "EXPWOP") || ($("#drdTransCategory").val() == "DEXP")) {
                        $("#chkSendMail").prop("checked", false);
                        $("#chkSendMail").prop("disabled", true);
                    }
                    else {
                        $("#chkSendMail").prop("disabled", false);
                    }
                }
                else {
                    $("#chkSendMail").prop("disabled", false);
                }
            },
            error: function (msg) {
            }
        });

        if ($('#hdnDocumentSegmentSettings').val() == "1") {


            $.ajax({
                type: "POST",
                url: "SalesOrderAdd.aspx/GetSegmentDetails",
                data: JSON.stringify({ CustomerId: Id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    OutStandingAmount = msg.d;
                    if (OutStandingAmount != null)
                    {
                        if (OutStandingAmount.Segment1 != "") {
                            var Segment1 = OutStandingAmount.Segment1;
                            var Segment2 = OutStandingAmount.Segment2;
                            var Segment3 = OutStandingAmount.Segment3;
                            var Segment4 = OutStandingAmount.Segment4;
                            var Segment5 = OutStandingAmount.Segment5;

                            if (Segment1 == "0") {
                                var div = document.getElementById('DivSegment1');
                                div.style.display = 'none';
                                $('#hdnValueSegment1').val("0");
                            }
                            else {
                                $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                $('#hdnValueSegment1').val("1");
                            }
                            if (Segment2 == "0") {
                                var div = document.getElementById('DivSegment2');
                                div.style.display = 'none';
                                $('#hdnValueSegment2').val("0");
                            }
                            else {
                                $('#ModuleSegment2Header').text(OutStandingAmount.SegmentName2);
                                $('#hdnValueSegment2').val("1");
                            }

                            if (Segment3 == "0") {
                                var div = document.getElementById('DivSegment3');
                                div.style.display = 'none';
                                $('#hdnValueSegment3').val("0");
                            }
                            else {
                                $('#ModuleSegment3Header').text(OutStandingAmount.SegmentName3);
                                $('#hdnValueSegment3').val("1");
                            }

                            if (Segment4 == "0") {
                                var div = document.getElementById('DivSegment4');
                                div.style.display = 'none';
                                $('#hdnValueSegment4').val("0");
                            }
                            else {
                                $('#ModuleSegment4Header').text(OutStandingAmount.SegmentName4);
                                $('#hdnValueSegment4').val("1");
                            }

                            if (Segment5 == "0") {
                                var div = document.getElementById('DivSegment5');
                                div.style.display = 'none';
                                $('#hdnValueSegment5').val("0");
                            }
                            else {
                                $('#ModuleSegment5Header').text(OutStandingAmount.SegmentName5);
                                $('#hdnValueSegment5').val("1");
                            }
                        }
                    }
                    else{
                            
                        document.getElementById('DivSegment1').style.display = 'none';
                        document.getElementById('DivSegment2').style.display = 'none';
                        document.getElementById('DivSegment3').style.display = 'none';
                        document.getElementById('DivSegment4').style.display = 'none';
                        document.getElementById('DivSegment5').style.display = 'none';
                    }
                }
                    
            });

            $('#hdnCustomerId').val(Id);
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSegment1Search").val();
            OtherDetails.CustomerIds = Id;

            var HeaderCaption = [];
            HeaderCaption.push("Code");
            HeaderCaption.push("Name");
            callonServer("Services/Master.asmx/GetSegment1", OtherDetails, "Segment1Table", HeaderCaption, "segment1Index", "Setsegment1");
        }
    }







    //Rev Rajdip For Get & set Credit days



    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/GetCreditdays",
        data: JSON.stringify({ CustomerId: Id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // async:false,
        success: function (msg) {
            OutStandingAmount = msg.d;
            if (OutStandingAmount.CreditDays != 0) {
                var cday = OutStandingAmount.CreditDays;
                ctxtCreditDays.SetValue(cday);
                cdt_SaleInvoiceDue.SetEnabled(false);

                var CreditDays = ctxtCreditDays.GetValue();
                var newdate = new Date();
                var today = new Date();

                today = tstartdate.GetDate();
                today.setDate(today.getDate() + Math.round(CreditDays));

                cdt_SaleInvoiceDue.SetDate(today);
            }
            else {
                ctxtCreditDays.SetValue(0);
                cdt_SaleInvoiceDue.SetDate();
            }
        }
    });
    //End Rev rajdip
    if (gridquotationLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                var key = Id;
                if (key != null && key != '') {
                    $('#CustModel').modal('hide');
                    ctxtCustName.SetText(Name);

                    page.GetTabByName('Billing/Shipping').SetEnabled(true);

                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    var startDate = new Date();
                    startDate = tstartdate.GetValueString();

                    //if (type != "") {
                    //cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%' + '~' + type);
                    //}

                    var componentType = gridquotationLookup.GetValue();////gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
                    if (componentType != null && componentType != '') {
                        $('#hdnPageStatus').val('update');
                    }
                    //Edited Chinmoy Below line
                    $('#openlink').show();
                    cddl_PosGst.ClearItems();
                    cddl_PosGst.SetEnabled(true);
                    SetDefaultBillingShippingAddress(key);
                    //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SI');
                    GetObjectID('hdnCustomerId').value = key;

                    //REV RAJDIP
                    SalesmanBindWRTCustomer(key);
                    //END REV RAJDIP




                    if ($('#hfBSAlertFlag').val() == "1") {
                        jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                            if (r == true) {
                                page.SetActiveTabIndex(1);
                                //cbsSave_BillingShipping.Focus();
                                page.tabs[0].SetEnabled(false);
                                $("#divcross").hide();
                            }
                        });
                    }
                    else {
                        page.SetActiveTabIndex(1);
                        //cbsSave_BillingShipping.Focus();
                        page.tabs[0].SetEnabled(false);
                        $("#divcross").hide();
                    }
                    GetObjectID('hdnAddressDtl').value = '0';

                }
            }
        });
    }
    else {
        var key = Id;
        if (key != null && key != '') {
            $('#CustModel').modal('hide');
            ctxtCustName.SetText(Name);
            page.GetTabByName('Billing/Shipping').SetEnabled(true);
            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
            var startDate = new Date();
            startDate = tstartdate.GetValueString();

            //if (type != "") {
            //cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%' + '~' + type);
            //}

            var componentType = gridquotationLookup.GetValue();
            if (componentType != null && componentType != '') {
                $('#hdnPageStatus').val('update');
            }
            //Edited Chinmoy Below line
            PosGstId = "";
            cddl_PosGst.SetValue(PosGstId);
            SetDefaultBillingShippingAddress(key);
            //REV RAJDIP
            SalesmanBindWRTCustomer(key);
            //END REV RAJDIP
            //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SI');
            GetObjectID('hdnCustomerId').value = key;
            if ($('#hfBSAlertFlag').val() == "1") {
                jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        page.SetActiveTabIndex(1);
                        //cbsSave_BillingShipping.Focus();
                        page.tabs[0].SetEnabled(false);
                        $("#divcross").hide();
                    }
                });
            }
            else {
                page.SetActiveTabIndex(1);
                //cbsSave_BillingShipping.Focus();
                page.tabs[0].SetEnabled(false);
                $("#divcross").hide();
            }

            SetEntityType(Id);
        }
    }

}

//Rev Rajdip For Customer Map To SalesMan
//Rev Rajdip For Delete SalesMan
function Deletesalesman() {

    $("#hdnSalesManAgentId.ClientID").val("");
    ctxtSalesManAgent.SetText("");
}
//End Rev rajdip
function SalesmanBindWRTCustomer(Id) {

    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/MappedSalesManOnetoOne",
        data: JSON.stringify({ Id: Id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var contactPersonJsonObject = r.d;
            IsContactperson = false;
            //SetDataSourceOnComboBox(cddlsalesmanmapped, contactPersonJsonObject);
            if (r.d.length > 0) {
                // cPopup_salesman.Show();
                $("#hdnSalesManAgentId").val(r.d[0].Id);
                ctxtSalesManAgent.SetText(r.d[0].Name);
            }

        }

    });
}

function Set_MappedSalesMan() {

    var Id = cddlsalesmanmapped.GetValue();
    var Name = cddlsalesmanmapped.GetText();
    $("#hdnSalesManAgentId").val(Id);

    ctxtSalesManAgent.SetText(Name);
    cPopup_salesman.Hide();
    $('#SalesManModel').modal('hide');

}
function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}
//End Rev Rajdip For Customer Map To SalesMan
function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "ProdIndex") {
                SetProduct(Id, name);
            }
            else if (indexName == "salesmanIndex") {
                OnFocus(Id, name);
            }
                // Added By Chinmoy
                //Start
            else if (indexName == "BillingAreaIndex") {
                SetBillingArea(Id, name);
            }
            else if (indexName == "ShippingAreaIndex") {
                SetShippingArea(Id, name);
            }
            else if (indexName == "customeraddressIndex") {
                SetCustomeraddress(Id, name);
            }
            else if (indexName == "segment1Index") {
                Setsegment1(Id, name);
            }
            else if (indexName == "segment2Index") {
                Setsegment2(Id, name);
            }
            else if (indexName == "segment3Index") {
                Setsegment3(Id, name);
            }
            else if (indexName == "segment4Index") {
                Setsegment4(Id, name);
            }
            else if (indexName == "segment5Index") {
                Setsegment5(Id, name);
            }
            else {
                SetCustomer(Id, name);
            }
        }
    }
    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex++;
        if (thisindex < 10)
            $("input[" + indexName + "=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex--;
        if (thisindex > -1)
            $("input[" + indexName + "=" + thisindex + "]").focus();
        else {
            if (indexName == "ProdIndex")
                $('#txtProdSearch').focus();
            else if (indexName == "salesmanIndex")
                ctxtCreditDays.Focus();
                // Added By Chinmoy
                //Start
            else if (indexName == "BillingAreaIndex")
                $('#txtbillingArea').focus();
            else if (indexName == "ShippingAreaIndex")
                $('#txtshippingArea').focus();
            else if (indexName == "customeraddressIndex")
                ('#txtshippingShipToParty').focus();
                //End
            else
                $('#txtCustSearch').focus();
        }
    }
}


function prodkeydown(e) {
    //Both-->B;Inventory Item-->Y;Capital Goods-->C
    var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";
    var OtherDetails = {}
    if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
        return false;
    }

    OtherDetails.SearchKey = $("#txtProdSearch").val();
    OtherDetails.InventoryType = inventoryType;

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetProductDetailsForSI", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
}

var canCallBack = true;
function AllControlInitilize() {
    if (canCallBack) {

        if ($('#hdnPageStatus').val() == "update") {

            $('#openlink').hide();

        }
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        grid.batchEditApi.EndEdit();
        $('#ddlInventory').focus();
        canCallBack = false;
        //Edited By Chinmoy 
        //start

        //PopulatePosGst();
        LoadtBillingShippingCustomerAddress($('#hdnCustomerId').val());
        LoadtBillingShippingShipTopartyAddress();
        //End

        //if ($('#hdnPageStatus').val() == "update")
        //{
        //    BillShipAddressVisible();
        //}
        //Chinmoy added below condition
        if ($('#hdnPageStatus').val() == "update") {
            //Rev Rajdip
            var hdnPlaceShiptoParty = $("#hdnPlaceShiptoParty").val();
            //cddl_PosGst.SetEnabled(false);
            if (hdnPlaceShiptoParty == "1")
            { cddl_PosGst.SetEnabled(true); }
            else
            { cddl_PosGst.SetEnabled(false); }
            //End Rev Rajdip
            // SetTotalTaxableAmount();
            AllowAddressShipToPartyState = false;
        }

    }
}


function CustomerButnClick(s, e) {
    $('#CustModel').modal('show');
}

function SalesManButnClick(s, e) {
    $('#SalesManModel').modal('show');
    $("#txtSalesManSearch").focus();
}

function SalesManbtnKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#SalesManModel').modal('show');
        $("#txtSalesManSearch").focus();
    }
}
function SalesMankeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSalesManSearch").val();
    OtherDetails.CustomerId = $("#hdnCustomerId").val();
    if ($.trim($("#txtSalesManSearch").val()) == "" || $.trim($("#txtSalesManSearch").val()) == null) {
        return false;
    }
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Name");
        if ($("#txtSalesManSearch").val() != null && $("#txtSalesManSearch").val() != "") {
            //Rev Rajdip
            //callonServer("Services/Master.asmx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "OnFocus");
            callonServer("SalesInvoice.aspx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "OnFocus");
            //End Rev Rajdip
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[salesmanIndex=0]"))
            $("input[salesmanIndex=0]").focus();
    }
}

function OnFocus(Id, Name) {

    $("#hdnSalesManAgentId").val(Id);

    ctxtCreditDays.Focus();
    ctxtSalesManAgent.SetText(Name);
    $('#SalesManModel').modal('hide');
}



//Hierarchy Start Tanmoy
function ProjectValueChange(s, e) {

    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'SalesInvoice.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}

function clookup_Project_LostFocus() {
    //grid.batchEditApi.StartEdit(-1, 2);

    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}

function ShowTCS() {

    var CustomerId = $("#hdnCustomerId").val();
    var invoice_id = $("#hdnPageEditId").val();
    var date = tstartdate.GetText();
    var totalAmount = $("#bnrLblInvValue").text();
    var taxableAmount = $("#bnrLblTaxableAmtval").text();

    var obj = {};
    obj.CustomerId = CustomerId;
    obj.invoice_id = invoice_id;
    obj.date = date;
    obj.totalAmount = totalAmount;
    obj.taxableAmount = taxableAmount;
    obj.branch_id = $("#ddl_Branch").val();

    if (invoice_id == "" || invoice_id == null) {
        $.ajax({
            type: "POST",
            url: 'SalesInvoice.aspx/getTCSDetails',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(obj),
            success: function (msg) {

                if (msg) {
                    var response = msg.d;
                    ctxtTCSSection.SetText(response.Code);
                    ctxtTCSapplAmount.SetText(response.tds_amount);
                    ctxtTCSpercentage.SetText(response.Rate);
                    ctxtTCSAmount.SetText(response.Amount);
                    cGridTCSdocs.PerformCallback();
                }


            }
        });
    }
    else {
        cGridTCSdocs.PerformCallback();
    }



    $("#tcsModal").modal('show');
}

function GetSalesRateSchemePrice(CustomerID, ProductID, SalesPrice) {

    var date = new Date;
    var seconds = date.getSeconds();
    var minutes = date.getMinutes();
    var hour = date.getHours();

    var times = hour + ':' + minutes;

    var sdate = tstartdate.GetValue();
    var startDate = new Date(sdate);
    var OtherDetails = {}
    OtherDetails.CustomerID = CustomerID;
    OtherDetails.ProductID = ProductID;
    OtherDetails.PostingDate = startDate;//+ ' ' + times;
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetSalesRateSchemePrice",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d;
            // Mantis Issue 24425, 24428
            //console.log(returnObject);
            // End of Mantis Issue 24425, 24428
            if (returnObject.length > 0) {
                $("#ProductMinPrice").val(returnObject[0].MinSalePrice);
                $("#ProductMaxPrice").val(returnObject[0].MaxSalePrice);
                $("#hdnRateType").val(returnObject[0].RateType);
                $("#hdnProductDaynamicRate").val(returnObject[0].MRP);
            }
                //Rev work start 28.06.2022 Mantise no:24949
            else {

                $("#ProductMinPrice").val(0);
                $("#ProductMaxPrice").val(0);
                $("#hdnRateType").val(0);
                $("#hdnProductDaynamicRate").val(0);
            }
            setValueOfSAale();
            //Rev work close 28.06.2022 Mantise no:24949
        }
    });
}


function UploadGridbindCancel() {
    $("#exampleModalSI").modal("hide");
    window.location.assign("SalesInvoiceList.aspx");

}

function UploadGridbind() {

    $("#exampleModalSI").modal("hide");
    grid.PerformCallback('EInvoice~' + $("#hdnRDECId").val());

}

function IrnGrid() {
    $(".bcShad, .popupSuc").removeClass("in");
    var Quote_ID = $("#hdnRDECId").val();
    if (Quote_ID != null && Quote_ID != undefined && Quote_ID != "") {
        //Add Section For Auto Print checking for Einvoice Tanmoy
        if ($("#hdnIsBranchEInvoice").val() == "True") {
            if (($("#drdTransCategory").val() == "B2B") || ($("#drdTransCategory").val() == "SEZWP") || ($("#drdTransCategory").val() == "SEZWOP")||($("#drdTransCategory").val() == "EXPWP") || ($("#drdTransCategory").val() == "EXPWOP") || ($("#drdTransCategory").val() == "DEXP")) {
            }
            else {
                if ($("#hdnAutoPrint").val() == "1") {
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=1', '_blank')
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=2', '_blank')
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=4', '_blank')
                }
            }
        }
        else {
            if ($("#hdnAutoPrint").val() == "1") {
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=1', '_blank')
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=2', '_blank')
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=4', '_blank')
            }
        }
    }
    window.location.assign("SalesInvoiceList.aspx");
}

function Segment1ButnClick(s, e) {
    if ($("#hdnCustomerId").val() != "") {
        $('#Segment1Model').modal('show');
    }
    else {
        jAlert("Please Select Customer");
    }

}
function Segment1keydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment1Search").val();
    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment1", OtherDetails, "Segment1Table", HeaderCaption, "segment1Index", "Setsegment1");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment1Index=0]"))
            $("input[segment1Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment1.Focus();
    }
}

function Segment1_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment1Model').modal('show');
        $("#txtSegment1Search").focus();
    }
}

function Setsegment1(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment1Model').modal('hide');
    ctxtSegment1.SetText(ProductCode);
    $('#hdnSegment1').val(LookUpData);
    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);

    if ($('#hdnValueSegment2').val() == "1") {
        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment2Search").val();
        OtherDetails.CustomerIds = $("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment2", OtherDetails, "Segment2Table", HeaderCaption, "segment2Index", "Setsegment2");
        $('#Segment2Model').modal('show');
    }




}
function Segment2ButnClick(s, e) {
    if ($("#hdnCustomerId").val() != "") {
        $('#Segment2Model').modal('show');
    }
    else {
        jAlert("Please Select Customer");
    }
}
function Segment2keydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment1Search").val();
    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment2", OtherDetails, "Segment2Table", HeaderCaption, "segment2Index", "Setsegment2");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment2Index=0]"))
            $("input[segment2Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment2.Focus();
    }
}
function Segment2_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment2Model').modal('show');
        $("#txtSegment2Search").focus();
    }
}
function Setsegment2(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment2Model').modal('hide');
    ctxtSegment2.SetText(ProductCode);
    $('#hdnSegment2').val(LookUpData);

    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
    if ($('#hdnValueSegment3').val() == "1") {
        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment1Search").val();
        OtherDetails.CustomerIds = $("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment3", OtherDetails, "Segment3Table", HeaderCaption, "segment3Index", "Setsegment3");
        $('#Segment3Model').modal('show');
    }


}
function Segment3ButnClick(s, e) {
    if ($("#hdnCustomerId").val() != "") {
        $('#Segment3Model').modal('show');
    }
    else {
        jAlert("Please Select Customer");
    }

}
function Segment3keydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment3Search").val();
    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment3", OtherDetails, "Segment3Table", HeaderCaption, "segment3Index", "Setsegment3");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment3Index=0]"))
            $("input[segment3Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment3.Focus();
    }
}
function Setsegment3(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment3Model').modal('hide');
    ctxtSegment3.SetText(ProductCode);
    $('#hdnSegment3').val(LookUpData);

    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
    if ($('#hdnValueSegment4').val() == "1") {
        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment4Search").val();
        OtherDetails.CustomerIds = $("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment4", OtherDetails, "Segment4Table", HeaderCaption, "segment4Index", "Setsegment4");
        $('#Segment4Model').modal('show');
    }


}
function Segment3_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment3Model').modal('show');
        $("#txtSegment3Search").focus();
    }
}
function Segment4ButnClick(s, e) {
    if ($("#hdnCustomerId").val() != "") {
        $('#Segment4Model').modal('show');
    }
    else {
        jAlert("Please Select Customer");
    }

}
function Segment4keydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment4Search").val();
    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment4", OtherDetails, "Segment4Table", HeaderCaption, "segment4Index", "Setsegment4");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment4Index=0]"))
            $("input[segment4Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment4.Focus();
    }
}
function Setsegment4(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment4Model').modal('hide');
    ctxtSegment4.SetText(ProductCode);
    $('#hdnSegment4').val(LookUpData);
    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
    if ($('#hdnValueSegment5').val() == "1") {

        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment5Search").val();
        OtherDetails.CustomerIds = $("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment5", OtherDetails, "Segment5Table", HeaderCaption, "segment5Index", "Setsegment5");
        $('#Segment5Model').modal('show');
    }


}
function Segment4_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment4Model').modal('show');
        $("#txtSegment4Search").focus();
    }
}
function Segment5_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment5Model').modal('show');
        $("#txtSegment5Search").focus();
    }
}
function Segment5ButnClick(s, e) {
    if ($("#hdnCustomerId").val() != "") {
        $('#Segment5Model').modal('show');
    }
    else {
        jAlert("Please Select Customer");
    }

}
function Setsegment5(Id, Name, e) {

    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment5Model').modal('hide');
    ctxtSegment5.SetText(ProductCode);
    $('#hdnSegment5').val(LookUpData);

    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
}
function Segment5keydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment5Search").val();
    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment5", OtherDetails, "Segment5Table", HeaderCaption, "segment5Index", "Setsegment5");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment5Index=0]"))
            $("input[segment5Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment5.Focus();
    }
}

function BillBillingPinChange() {

    var detailsByPin = BctxtbillingPin.GetText().trim();
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

                if (returnObj) {

                    $('#BhdBillingPin').val(returnObj.PinId);
                    BctxtbillingPin.SetText(returnObj.PinCode);

                    $('#BhdCountryIdBilling').val(returnObj.CountryId);
                    BctxtbillingCountry.SetText(returnObj.CountryName);
                    $('#BhdStateIdBilling').val(returnObj.StateId);
                    BctxtbillingState.SetText(returnObj.StateName);

                    $('#BhdStateCodeBilling').val(returnObj.StateCode);
                    BctxtbillingCity.SetText(returnObj.CityName);

                    $('#BhdCityIdBilling').val(returnObj.CityId);
                }
                else {

                    $('#BhdCountryIdBilling').val('');
                    BctxtbillingCountry.SetText('');
                    $('#BhdStateIdBilling').val('');
                    BctxtbillingState.SetText('');
                    $('#BhdStateCodeBilling').val('');
                    BctxtbillingCity.SetText('');
                    $('#BhdCityIdBilling').val('');
                }
            }
        })

    }

}

function DespatchShippingPinChange() {

    var detailsByPin = DctxtShippingPin.GetText().trim();
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

                if (returnObj) {


                    $('#DhdShippingPin').val(returnObj.PinId);
                    DctxtShippingPin.SetText(returnObj.PinCode)
                    $('#DhdCountryIdShipping').val(returnObj.CountryId);
                    DctxtshippingCountry.SetText(returnObj.CountryName);

                    $('#DhdStateIdShipping').val(returnObj.StateId);
                    DctxtshippingState.SetText(returnObj.StateName);

                    $('#DhdStateCodeShipping').val(returnObj.StateCode);
                    DctxtshippingCity.SetText(returnObj.CityName);

                    $('#DhdCityIdShipping').val(returnObj.CityId);
                }

                else {

                    $('#DhdCountryIdShipping').val('');
                    DctxtshippingCountry.SetText('');
                    $('#DhdStateIdShipping').val('');
                    DctxtshippingState.SetText('');
                    $('#DhdStateCodeShipping').val('');
                    DctxtshippingCity.SetText('');
                    $('#DhdCityIdShipping').val('');

                }
            }
        });
    }

}

function Save_BillDespatch() {
    cpopupBillDsep.Show();
}

function LoadBillDespatch(BranchId) {

    var OtherDetails = {}
    OtherDetails.BranchId = BranchId;
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/Services/PurchaseBillShip.asmx/FetchBranchAddressBilldespatch",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            var DesspBillingAddress = msg.d;


            var BillBillingObj = $.grep(DesspBillingAddress, function (e) { return e.Type == "Billing" && e.Isdefault == 1; })
            var DespatchShippingObj = $.grep(DesspBillingAddress, function (e) { return e.Type == "Factory/Work/Branch" && e.Isdefault == 1; })

            if (BillBillingObj.length > 0) {
                //Billing
                BctxtAddress1.SetText(BillBillingObj[0].Address1);
                BctxtAddress2.SetText(BillBillingObj[0].Address2);
                BctxtAddress3.SetText(BillBillingObj[0].Address3);
                BctxtbillingPin.SetText(BillBillingObj[0].PinCode);
                $('#BhdBillingPin').val(BillBillingObj[0].PinId);
                BctxtbillingCountry.SetText(BillBillingObj[0].CountryName);
                $('#BhdCountryIdBilling').val(BillBillingObj[0].CountryId);
                BctxtbillingState.SetText(BillBillingObj[0].StateName);
                $('#BhdStateIdBilling').val(BillBillingObj[0].StateId);
                $('#BhdStateCodeBilling').val(BillBillingObj[0].StateCode);
                BctxtbillingCity.SetText(BillBillingObj[0].CityName);
                $('#BhdCityIdBilling').val(BillBillingObj[0].CityId);

                //end
            }
            else {
                //Billing
                BctxtAddress1.SetText('');
                BctxtAddress2.SetText('');
                BctxtAddress3.SetText('');
                BctxtbillingPin.SetText('');
                $('#BhdBillingPin').val('');
                BctxtbillingCountry.SetText('');
                $('#BhdCountryIdBilling').val('');
                BctxtbillingState.SetText('');
                $('#BhdStateIdBilling').val('');
                $('#BhdStateCodeBilling').val('');
                BctxtbillingCity.SetText('');
                $('#BhdCityIdBilling').val('');
            }
            if (DespatchShippingObj.length > 0) {
                //Shipping

                DctxtsAddress1.SetText(DespatchShippingObj[0].Address1);
                DctxtsAddress2.SetText(DespatchShippingObj[0].Address2);
                DctxtsAddress3.SetText(DespatchShippingObj[0].Address3);
                DctxtShippingPin.SetText(DespatchShippingObj[0].PinCode);
                $('#DhdShippingPin').val(DespatchShippingObj[0].PinId);
                DctxtshippingCountry.SetText(DespatchShippingObj[0].CountryName);
                $('#DhdCountryIdShipping').val(DespatchShippingObj[0].CountryId);
                DctxtshippingState.SetText(DespatchShippingObj[0].StateName);
                $('#DhdStateIdShipping').val(DespatchShippingObj[0].StateId);
                $('#DhdStateCodeShipping').val(DespatchShippingObj[0].StateCode);
                DctxtshippingCity.SetText(DespatchShippingObj[0].CityName);
                $('#DhdCityIdShipping').val(DespatchShippingObj[0].CityId);

                //end
            }
            else {


                //Shipping
                DctxtsAddress1.SetText('');
                DctxtsAddress2.SetText('');
                DctxtsAddress3.SetText('');
                DctxtShippingPin.SetText('');
                $('#DhdShippingPin').val('');
                DctxtshippingCountry.SetText('');
                $('#DhdCountryIdShipping').val('');
                DctxtshippingState.SetText('');
                $('#DhdStateIdShipping').val('');
                $('#DhdStateCodeShipping').val('');
                DctxtshippingCity.SetText('');
                $('#DhdCityIdShipping').val('');

            }





        }
    });



}

function Validationbilldespatch() {
    if (BctxtAddress1.GetText() == "") {
        return false;
    }
    else if (BctxtbillingPin.GetText() == "" || BctxtbillingPin.GetText() == "0") {
        return false;
    }
    else if (BctxtbillingCountry.GetText() == "") {
        return false;
    }
    else if (BctxtbillingState.GetText() == "") {
        return false;
    }
    else if (BctxtbillingCity.GetText() == "") {
        return false;
    }
    else if (DctxtsAddress1.GetText() == "") {
        return false;
    }
    else if (DctxtShippingPin.GetText() == "" || DctxtShippingPin.GetText() == "0") {
        return false;
    }
    else if (DctxtshippingCountry.GetText() == "") {
        return false;
    }
    else if (DctxtshippingState.GetText() == "") {
        return false;
    }
    else if (DctxtshippingCity.GetText() == "") {
        return false;
    }
    else {
        cpopupBillDsep.Hide();
    }
}
function ValidationbilldespatchCancel() {
    cpopupBillDsep.Hide();

}



function DeliveryScheduleGotFocusFromID(s, e) {
    pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    if (ProductID != "" && ProductID != null) {
        var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";       
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();

        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];
        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var strProductShortCode = SpliteDetails[14];
        var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

        strProductName = strDescription;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
            divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }

        $('#lblStkQty').text(QuantityValue);
        $('#lblStkUOM').text(strStkUOM);
        $('#lblProduct').text(strProductName);
        $('#lblbranchName').text(strBranch);

        if (ProductID != "0") {
            //cacpAvailableStock.PerformCallback(strProductID);
            setStock(strProductID);
        }
    }
}



function DeliveryScheduleKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    if (e.htmlEvent.key == "Tab") {
        s.OnButtonClick(0);
    }
}

function DeliveryScheduleButnClick(s, e) {
    if (e.buttonIndex == 0) {
        setTimeout(function () { $("#txtScheduleSearch").focus(); }, 500);

        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

        if (type!="SO") {
            jAlert("Please Select Order Tagging  first.", "Alert", function () { $('#txtCustSearch').focus(); })
            return;
        }
        $('#txtScheduleSearch').val('');
        $('#ScheduleModel').modal('show');
        ScheduleList();
    }
}

function Schedulekeydown(e) {
    //Both-->B;Inventory Item-->Y;Capital Goods-->C
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];

    var ComponentID = (grid.GetEditor('ComponentID').GetText() != null) ? grid.GetEditor('ComponentID').GetText() : "0";
    var DetailsId = (grid.GetEditor('DetailsId').GetText() != null) ? grid.GetEditor('DetailsId').GetText() : "0";
    
    

    var OtherDetails = {}
    //if ($.trim($("#txtScheduleSearch").val()) == "" || $.trim($("#txtScheduleSearch").val()) == null) {
    //    return false;
    //}

    OtherDetails.SearchKey = $("#txtScheduleSearch").val();
    OtherDetails.ProductID = strProductID;
    OtherDetails.OrderID = ComponentID;
    OtherDetails.OrderDetailsId = DetailsId;

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Order No.");
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Schedule Quantity");
       
        callonServer("Services/Master.asmx/GetProductDeliverySchedule", OtherDetails, "ScheduleTable", HeaderCaption, "ProdIndex", "SetDeliverySchedule");
        //if ($("#txtScheduleSearch").val() != '') {
        //    callonServer("Services/Master.asmx/GetProductDeliverySchedule", OtherDetails, "ScheduleTable", HeaderCaption, "ProdIndex", "SetProduct");
        //}
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
}

function ScheduleList()
{
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var ComponentID = (grid.GetEditor('ComponentID').GetText() != null) ? grid.GetEditor('ComponentID').GetText() : "0";
    var DetailsId = (grid.GetEditor('DetailsId').GetText() != null) ? grid.GetEditor('DetailsId').GetText() : "0";

    var OtherDetails = {}   
    OtherDetails.SearchKey = $("#txtScheduleSearch").val();
    OtherDetails.ProductID = strProductID;
    OtherDetails.OrderID = ComponentID;
    OtherDetails.OrderDetailsId = DetailsId;

    var HeaderCaption = [];
    HeaderCaption.push("Order No.");
    HeaderCaption.push("Product Name");
    HeaderCaption.push("Schedule balance Quantity");
    HeaderCaption.push("Schedule Serial No");
    HeaderCaption.push("Schedule Delivery Date");
    callonServer("Services/Master.asmx/GetProductDeliverySchedule", OtherDetails, "ScheduleTable", HeaderCaption, "ProdIndex", "SetDeliverySchedule");
}

function SetDeliverySchedule(Id, Name) {
    $('#ScheduleModel').modal('hide');
    var LookUpData = Id;
    var DeliveryScheduleNO = Name;
    var SpliteDetails = Id.split("~");
    var ScheduleID = SpliteDetails[0];
    var ScheduleQty = SpliteDetails[1];

    var OrderDetails_UOMId = SpliteDetails[2];
    var AltQuantity = SpliteDetails[3];
    var OrderDetails_PackingUOM = SpliteDetails[4];
    var ProductId = SpliteDetails[5];
    var ProductUOM_Name = SpliteDetails[6];
    var AltUOM = SpliteDetails[7];
    var DetailsId = SpliteDetails[8];
    var DeliveryQty = SpliteDetails[9];
    if (!DeliveryScheduleNO) {
        LookUpData = null;
    }  
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("Quantity").SetText(ScheduleQty);
    grid.GetEditor("DeliverySchedule").SetText(DeliveryScheduleNO);
    grid.GetEditor("DeliveryScheduleID").SetText(ScheduleID);
    grid.GetEditor("DeliveryScheduleDetailsID").SetText(DetailsId);
    if (parseFloat(ScheduleQty) == parseFloat(DeliveryQty))
    {
        grid.GetEditor("InvoiceDetails_AltQuantity").SetValue(AltQuantity);
    }
    else {
        grid.GetEditor("InvoiceDetails_AltQuantity").SetValue("0.00");
    }
   
    grid.GetEditor("InvoiceDetails_AltUOM").SetValue(AltUOM);
    
    deleteTax('DelMULUOMbySl', grid.GetEditor("SrlNo").GetValue(), ProductId);
    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), ProductId);

    if (parseFloat(ScheduleQty) == parseFloat(DeliveryQty))
    {
        GetMulUOM('FetchMulUOM', grid.GetEditor("SrlNo").GetValue(), ProductId, ScheduleID, DetailsId, DeliveryScheduleNO);
    }
    

    SalePriceTextChange(null, null);
    grid.batchEditApi.StartEdit(globalRowIndex, 4);
   
}
function GetMulUOM(Action, srl, productid, ScheduleID, DetailsId, DeliveryScheduleNO) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;
    OtherDetail.prodid = productid;
    OtherDetail.ScheduleID = ScheduleID;
    OtherDetail.DetailsId = DetailsId;
    OtherDetail.DeliveryScheduleNO = DeliveryScheduleNO;
    $.ajax({
        type: "POST",
        url: "SalesInvoice.aspx/GetMulUOM",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var Code = msg.d;

            if (Code != null) {

            }
        }
    });
}