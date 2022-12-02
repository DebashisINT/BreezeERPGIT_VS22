$(function () {
    $('#UOMModal').on('hide.bs.modal', function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    });
});

var canCallBack = true;
function CustomerButnClick(s, e) {
    $('#CustModel').modal('show');
}

function SalesManButnClick(s, e) {
    $('#SalesManModel').modal('show');
}

function SalesManbtnKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#SalesManModel').modal('show');
    }
}

$(document).ready(function () {
    var mode = $('#hdnADDEditMode').val();
    if (mode == 'Edit') {
        if ($("#hdnCustomerId").val() != "") {
            var VendorID = $("#hdnCustomerId").val();
            SetEntityType(VendorID);
        }
    }
});

function GetDocumentAddress(OrderId, TagDocType) {

    var OtherDetail = {};

    OtherDetail.OrderId = OrderId;
    OtherDetail.TagDocType = TagDocType;


    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "ProjectIssueMaterials.aspx/SaveDocumentAddress",
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



var PosGstId = "";
function PopulatePosGst(e) {

    PosGstId = cddl_PosGst.GetValue();
    if (PosGstId == "S") {
        cddl_PosGst.SetValue("S");
    }
    else if (PosGstId == "B") {
        cddl_PosGst.SetValue("B");
    }
  
}

function AfterSaveBillingShipiing(validate) {

    if ($("#hdnmodeId").val() == "Add") {
        GetPosForGstValue();
    }
    if (validate) {
        page.SetActiveTabIndex(0);
        page.tabs[0].SetEnabled(true);
        //$("#divcross").show();

    }
    else {
        page.SetActiveTabIndex(1);
        page.tabs[0].SetEnabled(false);
       // $("#divcross").hide();
    }
}

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


function AllControlInitilize() {

    //debugger;
    if (canCallBack) {

        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbSaleschallan = grid.GetEditor("SrlNo");
        tbSaleschallan.SetValue(noofvisiblerows);
        grid.batchEditApi.EndEdit();
        $('#ddlInventory').focus();
        canCallBack = false;

        if ($("#hdnShowUOMConversionInEntry").val() == "1") {
            $("#btnSecondUOM").removeClass('hide');
        }
        else {
            $("#btnSecondUOM").addClass('hide');
        }
    }
}

function Customerkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtCustSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");
        if ($("#txtCustSearch").val() != null && $("#txtCustSearch").val() != "") {
            callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "GetContactPersonOnJSON");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtCustName.Focus();
    }
}

function SalesMankeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSalesManSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Name");
        if ($("#txtSalesManSearch").val() != null && $("#txtSalesManSearch").val() != "") {
            callonServer("Services/Master.asmx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "OnFocus");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[salesmanIndex=0]"))
            $("input[salesmanIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSalesManAgent.Focus();
    }
}

function OnFocus(Id, Name) {
   
    $("#hdnSalesManAgentId").val(Id);

    ctxtCreditDays.Focus();
    ctxtSalesManAgent.SetText(Name);
    $('#SalesManModel').modal('hide');
}

function ValueSelected(e, indexName) {
    //debugger;
    if (e.code == "Enter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;

        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "ProdIndex") {
                SetProduct(Id, name);
            }
            else if (indexName == "salesmanIndex") {
                OnFocus(Id, name);
            }
            else if (indexName == "customerIndex") {
                $('#CustModel').modal('hide');
                GetContactPersonOnJSON(Id, name);

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
            else
                $('#txtCustSearch').focus();
        }
    }

}
function CustomerKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#CustModel').modal('show');
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

var isCtrl = false;
document.onkeyup = function (e) {
    if (event.keyCode == 17) {
        isCtrl = false;
    }
    else if (event.keyCode == 27) {
        //     btnCancel_Click();
    }
}


document.onkeydown = function (e) {
    if (event.keyCode == 78 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + N -- ie, Save & New  
        StopDefaultAction(e);
        Save_ButtonClick();
    }
    else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + X -- ie, Save & Exit!     
        StopDefaultAction(e);
        SaveExit_ButtonClick();
    }
    else if (event.keyCode == 84 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + T -- ie, Tax & Charges!     
        StopDefaultAction(e);
        Save_TaxesClick();
    }
    else if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Add New
        StopDefaultAction(e);
        if (page.GetActiveTabIndex() == 1) {
            fnSaveBillingShipping();
        }
    }
    else if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        StopDefaultAction(e);
        page.SetActiveTabIndex(0);
        gridLookup.Focus();
    }
    else if (event.keyCode == 77 && event.altKey == true) {
        $('#TermsConditionseModal').modal({
            show: 'true'
        });
    }
    else if (event.keyCode == 69 && event.altKey == true) {
        if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
            StopDefaultAction(e);
            SaveTermsConditionData();
        }
    }
    else if (event.keyCode == 76 && event.altKey == true) {
        StopDefaultAction(e);
        calcelbuttonclick();
    }
    else if (event.keyCode == 83 && event.altKey == true) {
        if (($("#exampleModal").data('bs.modal') || {}).isShown) {
            SaveVehicleControlData();
        }
    }
    else if (event.keyCode == 67 && event.altKey == true) {
        modalShowHide(0);
    }
    else if (event.keyCode == 82 && event.altKey == true) {
        modalShowHide(1);
        $('body').on('shown.bs.modal', '#exampleModal', function () {
            $('input:visible:enabled:first', this).focus();
        });
    }
}

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}

function gridFocusedRowChanged(s, e) {

    globalRowIndex = e.visibleIndex;
}

function ProductKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {

        s.OnButtonClick(0);
    }
    if (e.htmlEvent.key == "Tab") {

        s.OnButtonClick(0);
    }
}

function fn_Edit(keyValue) {
    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}

function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {
        if (!GetObjectID('hdnCustomerId').value) {
            jAlert("Please Select Customer first.", "Alert", function () { $('#txtCustSearch').focus(); })
            return;
        }
        $('#txtProdSearch').val('');
        $('#ProductModel').modal('show');
    }
}

function prodkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    OtherDetails.InventoryType = $("#ddlInventory").val();
    OtherDetails.ProductIds = '';
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetSalesOrderProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        grid.batchEditApi.StartEdit(globalRowIndex, 2);
    }
}

function ProductLostFocused(s, e) {
    grid.GetEditor("Quantity").Focus();
}

function CreditDays_TextChanged(s, e) {

    var CreditDays = ctxtCreditDays.GetValue();
    var today = cPLSalesChallanDate.GetDate();
    var newdate = cPLSalesChallanDate.GetDate();
    newdate.setDate(today.getDate() + Math.round(CreditDays));
    cdt_SaleInvoiceDue.SetDate(newdate);
    cdt_SaleInvoiceDue.SetEnabled(false);
}

function ddlInventory_OnChange() {
    //cproductLookUp.GetGridView().Refresh();
}

function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
}

function SetProduct(Id, Name) {
    var LookUpData = Id;
 
    var ProductCode = Name;
    $('#ProductModel').modal('hide');

    if (!ProductCode) {
        LookUpData = null;
    }
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    cddl_PosGst.SetEnabled(false);

    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");
    AllowAddressShipToPartyState = false;
    var ProductID = LookUpData;
    if (LookUpData != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];

        if (grid.GetEditor("ProductID").GetValue() != "" && grid.GetEditor("ProductID").GetValue() != null) {

            //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
            deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), strProductID)
        }

        tbDescription.SetValue(strDescription);
        tbUOM.SetValue(strUOM);
        tbSalePrice.SetValue(strSalePrice);
        divPacking.style.display = "none";
        grid.GetEditor("Quantity").SetValue("0.00");
        grid.GetEditor("Discount").SetValue("0.00");
        grid.GetEditor("Amount").SetValue("0.00");
        grid.GetEditor("TaxAmount").SetValue("0.00");
        grid.GetEditor("TotalAmount").SetValue("0.00");

        $('#lblStkQty').text("0.00");
        $('#lblStkUOM').text(strStkUOM);
        //cacpAvailableStock.PerformCallback(strProductID);
        //alert(globalRowIndex);
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
        SetFocusAfterProductSelect();
    }
}

function SetFocusAfterProductSelect() {
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }, 200);
}

function ProductSelected(s, e) {
    var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
    var focusedRow = cproductLookUp.gridView.GetFocusedRowIndex();
    var ProductCode = cproductLookUp.gridView.GetRow(focusedRow).children[1].innerText;
    //var ProductCode = cproductLookUp.GetValue();

    if (!ProductCode) {
        LookUpData = null;
    }
    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);


    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");

    var ProductID = LookUpData;
    if (LookUpData != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];



        tbDescription.SetValue(strDescription);
        tbUOM.SetValue(strUOM);
        tbSalePrice.SetValue(strSalePrice);
        divPacking.style.display = "none";
        grid.GetEditor("Quantity").SetValue("0.00");
        grid.GetEditor("Discount").SetValue("0.00");
        grid.GetEditor("Amount").SetValue("0.00");
        grid.GetEditor("TaxAmount").SetValue("0.00");
        grid.GetEditor("TotalAmount").SetValue("0.00");

        $('#lblStkQty').text("0.00");
        $('#lblStkUOM').text(strStkUOM);
        cacpAvailableStock.PerformCallback(strProductID);
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }
}

var Pre_TotalAmt = "0";

function DiscountGotFocus(s, e) {
    var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
    Pre_TotalAmt = _Amount;
}

function QuantityGotFocus(s, e) {
    //debugger;
    var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
    Pre_TotalAmt = _Amount;
    //debugger;
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

    var ComponentNumber = (grid.GetEditor('Order_Num').GetText() != null) ? grid.GetEditor('Order_Num').GetText() : "0";

    var rdl_SaleInvoice = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';
    var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
    var gridPackingQty = '';
    var IsInventory = '';

    //var gridPackingQty = grid.GetEditor('QuoteDetails_PackingQty').GetText();
    if (SpliteDetails.length == 27) {
        if (SpliteDetails[26] == "1") {
            IsInventory = 'Yes';
        }
    }

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


            if (ComponentNumber != "0" && ComponentNumber != "") {

                if (rdl_SaleInvoice == 'MI') {
                    actionQry = 'SalesChallanPackingQtyOrder';
                }

                if (rdl_SaleInvoice == 'SI') {
                    actionQry = 'SalesChallanPackingQtyInvoice';
                }

                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({ orderid: strProductID, action: actionQry, module: 'SalesChallan', strKey: ComponentNumber }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        gridPackingQty = msg.d;
                        type = 'edit';
                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                            ShowUOM(type, "SalesChallan", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }
                    }
                });
            }
            else {

                actionQry = 'SalesChallanPackingQtyProductId';
                var orderid = grid.GetRowKey(globalRowIndex);
                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({ orderid: orderid, action: actionQry, module: 'SalesChallan', strKey: '' }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        gridPackingQty = msg.d;
                        type = 'edit';
                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                            ShowUOM(type, "SalesChallan", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }
                    }
                });
            }
        }
    }
    else {
        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
            ShowUOM(type, "SalesChallan", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
        }
        else {
            // alert('else');
        }
    }
}

var issavePacking = 0;

function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    grid.batchEditApi.EndEdit();
    grid.batchEditApi.StartEdit(globalRowIndex, 8);
    grid.GetEditor('Quantity').SetValue(Quantity);
}

function SetFoucs() {
    grid.batchEditApi.StartEdit(globalRowIndex, 8);
}

function SalesPriceGotFocus(s, e) {
    var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
    Pre_TotalAmt = _Amount;
}

function SalePriceTextChange(s, e) {

    pageheaderContent.style.display = "block";
    IsDiscountVal = $("#IsDiscountPercentage").val();
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var strMultiplier = SpliteDetails[7];
    var strFactor = SpliteDetails[8];
    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    var strStkUOM = SpliteDetails[4];

    if (strRate == 0) {
        strRate = 1;
    }

    var StockQuantity = strMultiplier * QuantityValue;
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

    var Amount = QuantityValue * strFactor * ((Saleprice.replace(/\,/g, '') * 1) / strRate);

    var amountAfterDiscount = "0";
    var ResultamountAfterDiscount = "0";
    if (IsDiscountVal == "Y") {
        if (parseFloat(Discount) > 100) {
            Discount = "0";

            var tb_Discount = grid.GetEditor("Discount");
            tb_Discount.SetValue(Discount);
        }

        ResultamountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
        amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
    }
    else {
        ResultamountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
        amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
    }

    var tbAmount = grid.GetEditor("Amount");
    tbAmount.SetValue(amountAfterDiscount);


    var TotaAmountRes = '';
    TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));

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

   
    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
    //    SpliteDetails[19], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val());

    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), cPLSalesChallanDate.GetDate(), QuantityValue,null);
    
    DiscountTextChange(s, e);
}

function DiscountTextChange(s, e) {
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    IsDiscountVal = $("#IsDiscountPercentage").val();
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var strFactor = SpliteDetails[8];
    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    if (strSalePrice == '0') {
        strSalePrice = SpliteDetails[6];
    }
    if (strRate == 0) {
        strRate = 1;
    }
    var Amount = QuantityValue * strFactor * ((strSalePrice.replace(/\,/g, '') * 1) / strRate);

    var amountAfterDiscount = "0";
    var ResultamountAfterDiscount = "0";
    if (IsDiscountVal == "Y") {
        if (parseFloat(Discount) > 100) {
            Discount = "0";
            var tb_Discount = grid.GetEditor("Discount");
            tb_Discount.SetValue(Discount);
        }
        ResultamountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
        amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
    }
    else {
        ResultamountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
        amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
    }

    var tbAmount = grid.GetEditor("Amount");
    tbAmount.SetValue(amountAfterDiscount);

    var TotaAmountRes = '';
    TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));

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
    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
    //    SpliteDetails[19], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val());

    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
        SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), cPLSalesChallanDate.GetDate(), QuantityValue,null);
    

    if (parseFloat(Amount) != parseFloat(Pre_TotalAmt)) {
        // ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
        deleteTax('DelQtybySl', grid.GetEditor("SrlNo").GetValue(), "")
    }
}

function CmbScheme_ValueChange() {

    var val = $("#ddl_numberingScheme").val();

    $.ajax({
        type: "POST",
        url: 'ProjectIssueMaterials.aspx/getSchemeType',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{sel_scheme_id:\"" + val + "\"}",
        success: function (type) {

            var schemetypeValue = type.d;
            var schemetype = schemetypeValue.toString().split('~')[0];
            var schemelength = schemetypeValue.toString().split('~')[1];
            $('#txt_SlChallanNo').attr('maxLength', schemelength);

            if (schemetype == '0') {
                ctxt_SlChallanNo.SetText('');
                ctxt_SlChallanNo.SetEnabled(true);
                ctxt_SlChallanNo.Focus();
            }
            else if (schemetype == '1') {
                ctxt_SlChallanNo.SetText('Auto');
                ctxt_SlChallanNo.SetEnabled(false);
                cPLSalesChallanDate.Focus();
            }
            else if (schemetype == '2') {
                }
            else if (schemetype == 'n') {
                }
        }
    });
}

function CmbWarehouse_ValueChange() {
    var isFIFORequired = false;

    var FifoExists = $("#hddnConfigVariable_Val").val();

    var WarehouseID = cCmbWarehouse.GetValue();
    $("#hddnWarehouseId").val(WarehouseID);
    var type = document.getElementById('hdfProductType').value;
    ctxtMatchQty.SetValue(0);
    if (WarehouseID != null) {
        if (type == "WBS" || type == "WB") {
            cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
        }
        else if (type == "WS" && FifoExists == "0") {
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + 'NoFIFO');
        }
    }
}

function CmbBatch_ValueChange() {
    var WarehouseID = cCmbWarehouse.GetValue();
    var FifoExists = $("#hddnConfigVariable_Val").val();

    var BatchID = cCmbBatch.GetValue();
    ctxtMatchQty.SetValue(0);
    $("#hddnBatchId").val(BatchID);
    var type = document.getElementById('hdfProductType').value;
    if (type == "WBS" && FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + 'NoFIFO');
    }
    else if (type == "BS" && FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + 'NoFIFO');
    }
    else if (type == "WS" && FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0" + '~' + 'NoFIFO');
    }
}

$(document).ready(function () {
    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
       // page.GetTabByName('Billing/Shipping').SetEnabled(false);
    }
    $('#ApprovalCross').click(function () {

        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.Refresh();
    })

    $('#CustModel').on('shown.bs.modal', function () {
        $('#txtCustSearch').focus();
    })

    $('#SalesManModel').on('shown.bs.modal', function () {
        $('#txtSalesManSearch').focus();
    })

    $('#ProductModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })

    if($("#hdnmodeId").val()!="Add")
    {
        cddl_PosGst.SetEnabled(false);
        AllowAddressShipToPartyState = false;
        if (page.GetActiveTabIndex() == 0 ) {
            page.GetTabByName('Billing/Shipping').SetEnabled(true);
        }
        if (page.GetActiveTabIndex() == 1 ) {
            page.GetTabByName('General').SetEnabled(false);
        }

    }




});

function UniqueCodeCheck() {

    var SchemeVal = $('#ddl_numberingScheme option:selected').val();
    if (SchemeVal == "") {
        alert('Please Select Numbering Scheme');
        ctxt_SlChallanNo.SetValue('');
        ctxt_SlChallanNo.Focus();
    }
    else {
        var OrderNo = ctxt_SlChallanNo.GetText();
        if (OrderNo != '') {

            var SchemaLength = GetObjectID('hdnSchemaLength').value;
            var x = parseInt(SchemaLength);
            var y = parseInt(OrderNo.length);

            if (y > x) {
                alert('Sales Order No length cannot be more than ' + x);
                //jAlert('Please enter unique Sales Order No');
                ctxt_SlChallanNo.SetValue('');
                ctxt_SlChallanNo.Focus();

            }
            else {
                var CheckUniqueCode = false;
                $.ajax({
                    type: "POST",
                    url: "ProjectIssueMaterials.aspx/CheckUniqueCode",
                    data: JSON.stringify({ OrderNo: OrderNo }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            alert('Please enter unique Sales Order No');
                            //jAlert('Please enter unique Sales Order No');
                            ctxt_SlChallanNo.SetValue('');
                            ctxt_SlChallanNo.Focus();
                        }
                        else {
                            $('#MandatorySlOrderNo').attr('style', 'display:none');
                        }
                    }
                });
            }
        }
    }
}

function DateCheck() {
    var startDate;
    if (cPLSalesChallanDate.GetDate() != null) {
        if (gridSalesOrderLookup.GetValue() != null) {
            jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    page.SetActiveTabIndex(0);
                    ccmbGstCstVat.PerformCallback();
                    ccmbGstCstVatcharge.PerformCallback();
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax('DeleteAllTax', "", "")

                    startDate = cPLSalesChallanDate.GetValueString();

                    var key = gridLookup.GetValue();
                    cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    if (key != null && key != '') {
                        if (type != '' && type != null) {
                            cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                        }
                    }
                    //grid.PerformCallback('GridBlank');
                    deleteAllRows();
                    grid.AddNewRow();
                    grid.GetEditor('SrlNo').SetValue('1');
                }

            });
        }
        else {
            page.SetActiveTabIndex(0);
            ccmbGstCstVat.PerformCallback();
            ccmbGstCstVatcharge.PerformCallback();
            deleteTax('DeleteAllTax', "", "")

            startDate = cPLSalesChallanDate.GetValueString();
            var key = gridLookup.GetValue();
            cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
            if (key != null && key != '') {
                if (type != '' && type != null) {
                    cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                }
            }
            //grid.PerformCallback('GridBlank');
            deleteAllRows();
            grid.AddNewRow();
            grid.GetEditor('SrlNo').SetValue('1');
        }
        gridSalesOrderLookup.gridView.Refresh();
        cProductsPopup.Hide();
    }
    else {
        cPLSalesChallanDate.SetText('');
        cPLSalesChallanDate.Focus();
        jAlert("Date is not valid.");
        return false;
    }
}

var SimilarProjectStatus = "0";

function CloseGridQuotationLookup() {
    gridSalesOrderLookup.ConfirmCurrentSelection();
    gridSalesOrderLookup.HideDropDown();
    gridSalesOrderLookup.Focus();

    var SalesOrdertetag_Id = gridSalesOrderLookup.gridView.GetSelectedKeysOnPage();

    if (SalesOrdertetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {
        var PSalsInvId = "";
        for (var i = 0; i < SalesOrdertetag_Id.length; i++) {
            if (PSalsInvId == "") {
                PSalsInvId = SalesOrdertetag_Id[i];
            }
            else {
                PSalsInvId += ',' + SalesOrdertetag_Id[i];
            }
        }
        var Doctype = $("#rdl_SaleInvoice").find(":checked").val();
       
        $.ajax({
            type: "POST",
            url: "ProjectIssueMaterials.aspx/DocWiseSimilarProjectCheck",
            data: JSON.stringify({ PSalsInvId: PSalsInvId, Doctype: Doctype }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                SimilarProjectStatus = msg.d;
                debugger;
                if (SimilarProjectStatus != "1") {
                    cPLSalesChallanDate.SetText("");
                    jAlert("Unable to procceed. Project are for the selected Document(s) are different.");
                    return false;
                }
            }
        });
    }
}

var GlobalCurTaxAmt = 0;
var rowEditCtrl;
var globalRowIndex;

function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}

function cmbtaxCodeindexChange(s, e) {
    if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {
        var taxValue = s.GetValue();
        if (taxValue == null) {
            taxValue = 0;
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.GetEditor("Amount").SetValue(0);
            ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
        }

        var isValid = taxValue.indexOf('~');
        if (isValid != -1) {
            var rate = parseFloat(taxValue.split('~')[1]);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
            ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
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

            ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
            GlobalCurTaxAmt = 0;
        } else {
            s.SetText("");
        }
    }
}

function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
}

function txtTax_TextChanged(s, i, e) {
    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
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


    ctxtQuoteTaxTotalAmt.SetValue((totalTaxAmount));
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}

function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cPopup_Taxes.Hide();
}

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

function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}

function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}

function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}
var GlobalCurChargeTaxAmt;
var ChargegstcstvatGlobalName;
function ChargecmbGstCstVatChange(s, e) {

    SetOtherChargeTaxValueOnRespectiveRow(0, 0, ChargegstcstvatGlobalName);
    $('.RecalculateCharge').hide();
    var ProdAmt = parseFloat(ctxtProductAmount.GetValue());

    if (s.GetValue().split('~')[2] == 'G') {
        ProdAmt = parseFloat(ctxtProductAmount.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'N') {
        ProdAmt = parseFloat(clblProdNetAmt.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'O') {
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

    SetOtherChargeTaxValueOnRespectiveRow(0, calculatedValue, ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = ctxtGstCstVatCharge.GetText();

    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}

var taxAmountGlobalCharges;
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}



function QuotationTaxAmountTextChange(s, e) {
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);

    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }

    RecalCulateTaxTotalAmountCharges();
}



function PercentageTextChange(s, e) {
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    var Percentage = s.GetText();
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }

    SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
    SetChargesRunningTotal();

    RecalCulateTaxTotalAmountCharges();
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

    document.getElementById('HdChargeProdAmt').value = sumAmount;
    document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
    
    ctxtProductAmount.SetValue(Math.round(sumAmount).toFixed(2));
    ctxtProductTaxAmount.SetValue(Math.round(sumTaxAmount).toFixed(2));
    ctxtProductDiscount.SetValue(Math.round(sumDiscount).toFixed(2));
    ctxtProductNetAmount.SetValue(Math.round(sumNetAmount).toFixed(2));
    clblChargesTaxableGross.SetText("");
    clblChargesTaxableNet.SetText("");

    if (cddl_AmountAre.GetValue() == "2") {

        $('.lblChargesGSTforGross').show();
        $('.lblChargesGSTforNet').show();

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

function BatchUpdate() {

    cgridTax.UpdateEdit();
    return false;
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
               

                var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                var strStkUOM = SpliteDetails[4];
               
                var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                if (strRate == 0) {
                    strRate = 1;
                }
               
                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = parseFloat((Math.round((QuantityValue * strFactor * (strSalePrice / strRate)) * 100).toFixed(2)) / 100);

                var IsDiscountPercentage = document.getElementById('IsDiscountPercentage').value;
                var amountAfterDiscount = "0";
                var ResultamountAfterDiscount = "0";
                if (IsDiscountPercentage == "Y") {
                    ResultamountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                    amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
                }
                else {
                    ResultamountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
                    amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
                }
                
                var _GrossAmt = (Amount * 1);

                var _NetAmt = (amountAfterDiscount * 1);

                clblTaxProdGrossAmt.SetText(_GrossAmt.toFixed(2));
                clblProdNetAmt.SetText(_NetAmt.toFixed(2));
                document.getElementById('HdProdGrossAmt').value = _GrossAmt;
                document.getElementById('HdProdNetAmt').value = _NetAmt;

                if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                   
                    var DiscountAfter = parseFloat(grid.GetEditor('Discount').GetValue());
                    var discount = DiscountAfter.toFixed(2);
                   
                    clblTaxDiscount.SetText(discount);
                }
                else {
                    clblTaxDiscount.SetText('0.00');
                }
              
                if (cddl_AmountAre.GetValue() == "2") {
                    $('.GstCstvatClass').hide();
                    $('.gstGrossAmount').show();
                    clblTaxableGross.SetText("(Taxable)");
                    clblTaxableNet.SetText("(Taxable)");
                    $('.gstNetAmount').show();
                  
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

                    if (cddl_PosGst.GetValue() != "") {
                        if (cddl_PosGst.GetValue() == "S") {
                            shippingStCode = GeteShippingStateCode();
                        }
                        else {
                            shippingStCode = GetBillingStateCode();
                        }
                    }
                  
                    shippingStCode = shippingStCode;
                    if (shippingStCode.trim() != '') {
                        for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                           
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {

                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {

                                  
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
                              
                                ccmbGstCstVat.RemoveItem(cmbCount);
                                cmbCount--;
                            }
                        }

                    }
                }

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

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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

var taxAmountGlobal;
function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}


function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
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

    SetOtherTaxValueOnRespectiveRow(0, calculatedValue, ccmbGstCstVat.GetText());
    gstcstvatGlobalName = ccmbGstCstVat.GetText();
}

var gstcstvatGlobalName;
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}


function txtPercentageLostFocus(s, e) {

    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
    if (s.GetText().trim() != '') {

        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
          
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

            SetRunningTotal();

        } else {
            s.SetText("");
        }
    }

}

function taxAmtButnClick1(s, e) {
    rowEditCtrl = s;
}

$(document).ready(function () {

    ctxtRate.SetValue("");
    ctxtRate.SetEnabled(false);
    ctxt_SlChallanNo.SetEnabled(false);
    gridSalesOrderLookup.SetEnabled(false);

    PopulateLoadGSTCSTVAT();
});

var QuantityRes = '';
function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
    gridSalesOrderLookup.SetEnabled(true);
}

function GetContactPersonOnJSON(id, Name) {
  
    var IsContactperson = true;
    var startDate = new Date();
    startDate = cPLSalesChallanDate.GetValueString();
    if (gridSalesOrderLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

            if (r == true) {

                var key = id;
                ctxtCustName.SetText(Name);
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                GetObjectID('hdnCustomerId').value = key;
                $('#CustModel').modal('hide');
                if (key != null && key != '') {

                    $.ajax({
                        type: "POST",
                        url: "ProjectIssueMaterials.aspx/GetContactPersonafterBillingShipping",
                        data: JSON.stringify({ Key: key }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (r) {
                            var contactPersonJsonObject = r.d;
                            IsContactperson = false;
                            SetDataSourceOnComboBox(cContactPerson, contactPersonJsonObject);
                        }
                    });

                    if (IsContactperson) {
                    }
                    if (type != '' && type != null) {
                    
                    }

                    cddl_PosGst.ClearItems();
                    cddl_PosGst.SetEnabled(true);
                    SetDefaultBillingShippingAddress(key);
                    GetObjectID('hdnCustomerId').value = key;
                    GetObjectID('hdnAddressDtl').value = '0';
                }
            }
        });
    }
    else {

        var key = id;
        ctxtCustName.SetText(Name);
        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
        GetObjectID('hdnCustomerId').value = key;
        if (key != null && key != '') {
            $('#CustModel').modal('hide');
            $.ajax({
                type: "POST",
                url: "ProjectIssueMaterials.aspx/GetContactPersonafterBillingShipping",
                data: JSON.stringify({ Key: key }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    var contactPersonJsonObject = r.d;
                    
                    IsContactperson = false;
                    SetDataSourceOnComboBox(cContactPerson, contactPersonJsonObject);
                }
            });

            if (IsContactperson) {
               // SetFocusAfterBillingShipping();
            }
            page.SetActiveTabIndex(1);
            $('.dxeErrorCellSys').addClass('abc');
            page.SetActiveTabIndex(1);
            $('.dxeErrorCellSys').addClass('abc');           
           // page.GetTabByName('Billing/Shipping').SetEnabled(true);
            page.GetTabByName('General').SetEnabled(false);          
            if (type != '' && type != null) {              
            }

            PosGstId = "";
            cddl_PosGst.SetValue(PosGstId);
            SetDefaultBillingShippingAddress(key);         
            GetObjectID('hdnCustomerId').value = key;          
            GetObjectID('hdnAddressDtl').value = '0';
        }
    }
    cProductsPopup.Hide();
    SetEntityType(id);
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
function SetFocusAfterBillingShipping() {
    setTimeout(function () {
        cContactPerson.Focus();
    }, 200);
}

function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}

function GetContactPerson(e) {
    //debugger;
    var CustomerComboBox = gridLookup.GetText();
    if (!gridLookup.FindItemByText(CustomerComboBox)) {
        gridLookup.SetValue("");
        gridLookup.Focus();
        jAlert("Customer not Exists.");
        return;
    }
    var startDate = new Date();
    startDate = cPLSalesChallanDate.GetValueString();
    if (gridSalesOrderLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

            if (r == true) {
                //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                var key = gridLookup.GetValue();

                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                GetObjectID('hdnCustomerId').value = key;
                if (key != null && key != '') {

                    if (type != '' && type != null) {
                        //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                    }
                  
                    cddl_PosGst.ClearItems();
                    cddl_PosGst.SetEnabled(true);
                    SetDefaultBillingShippingAddress(key);
                    //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SC');
                    GetObjectID('hdnCustomerId').value = key;
                  
                    billingLookup.focus();

                   
                    GetObjectID('hdnAddressDtl').value = '0';

                }
            }
        });
    }
    else {
        //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
        var key = gridLookup.GetValue();
        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
        GetObjectID('hdnCustomerId').value = key;
        if (key != null && key != '') {

            page.SetActiveTabIndex(1);
            $('.dxeErrorCellSys').addClass('abc');
           
            page.SetActiveTabIndex(1);
            $('.dxeErrorCellSys').addClass('abc');
          
            //page.GetTabByName('Billing/Shipping').SetEnabled(true);
            page.GetTabByName('General').SetEnabled(false);
            
            if (type != '' && type != null) {
                //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
            }
            PosGstId = "";
            cddl_PosGst.SetValue(PosGstId);
            SetDefaultBillingShippingAddress(key);
            //LoadCustomerAddress(key, $('#ddl_Branch').val());
            GetObjectID('hdnCustomerId').value = key;
           
            GetObjectID('hdnAddressDtl').value = '0';
        }
    }
    cProductsPopup.Hide();
}

function SetDifference1() {
    var diff = CheckDifferenceOfFromDateWithTodate();
}
function CheckDifferenceOfFromDateWithTodate() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLSalesChallanDate.GetDate();
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
    startDate = cPLSalesChallanDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;

    }
    return difference;

}

function clookup_project_GotFocus() {
    clookup_Project.gridView.Refresh();
    clookup_Project.ShowDropDown();
}

function deleteTax(Action, srl, productid) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;
    OtherDetail.prodid = productid;
    $.ajax({
        type: "POST",
        url: "ProjectIssueMaterials.aspx/taxUpdatePanel_Callback",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async:false,
        success: function (msg) {
            var Code = msg.d;

            if (Code != null) {
            }
        }
    });
}

function PopulateGSTCSTVAT(e) {
    var key = cddl_AmountAre.GetValue();
    if (key == 1) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.SetSelectedIndex(0);
    }
    else if (key == 2) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');
        cddlVatGstCst.Focus();
    }
    else if (key == 3) {
        grid.GetEditor('TaxAmount').SetEnabled(false);
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.SetSelectedIndex(0);
    }
}

function PopulateLoadGSTCSTVAT() {
    cddlVatGstCst.SetEnabled(false);
}

function showQuotationDocument() {
    var URL = "Contact_Document.aspx?requesttype=" + Quotation + "";
    window.location.href = URL;
}

function ShowCustom() {
    cPopup_wareHouse.Show();
}

var currentEditableVisibleIndex;
var preventEndEditOnLostFocus = false;
var lastProductID;
var setValueFlag;
var QuantityResultant = '';


function ProductsCombo_SelectedIndexChanged(s, e) {
    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");

    var ProductID = s.GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];



    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbSalePrice.SetValue(strSalePrice);
    divPacking.style.display = "none";
    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    cacpAvailableStock.PerformCallback(strProductID);
    //tbStkUOM.SetValue(strStkUOM);
    //tbStockQuantity.SetValue("0");
}

function SetArrForUOM() {

    //Rev Subhra 16-09-2019
    issavePacking = 1;
    //End of Rev Subhra 16-09-2019

    if (aarr.length == 0) {
        for (var i = -500; i < 500; i++) {
            if (grid.GetRow(i) != null) {

                var ProductID = (grid.batchEditApi.GetCellValue(i, 'ProductID') != null) ? grid.batchEditApi.GetCellValue(i, 'ProductID') : "0";
                var ComponentNumber = (grid.batchEditApi.GetCellValue(i, 'Order_Num') != null) ? grid.batchEditApi.GetCellValue(i, 'Order_Num') : "0";
                var rdl_SaleInvoice = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                if (ProductID != "0") {
                    var gridPackingQty = '';
                    var IsInventory = '';
                    var actionQry = '';
                    if (ComponentNumber != "0" && ComponentNumber != "") {

                        if (rdl_SaleInvoice == 'MI') {
                            actionQry = 'SalesChallanPackingQtyOrder';
                        }
                        if (rdl_SaleInvoice == 'SI') {
                            actionQry = 'SalesChallanPackingQtyInvoice';
                        }
                    }
                    else {
                        actionQry = 'SalesChallanPackingQtyProductId';

                    }
                    var QuotationNum = "0";
                    if (grid.GetEditor('Quotation_Num') != null) {
                        QuotationNum = (grid.GetEditor('Quotation_Num').GetText() != null) ? grid.GetEditor('Quotation_Num').GetText() : "0";
                    }
                    //if ($("#hdAddOrEdit").val() == "Edit") {
                    if ($("#hdnPageStatus").val() == "update") {
                        var SpliteDetails = ProductID.split("||@||");
                        var strProductID = SpliteDetails[0];
                        var orderid = grid.GetRowKey(i);
                        var slnoget = grid.batchEditApi.GetCellValue(i, 'SrlNo');
                        var Quantity = grid.batchEditApi.GetCellValue(i, 'Quantity');
                        //Rev Subhra 16-09-2019
                        var challanid = document.getElementById('hfDocId').value;
                        orderid = challanid;
                        if (actionQry = 'SalesChallanPackingQtyProductId') {
                            ComponentNumber = strProductID;
                        }
                        //End of Rev Subhra 16-09-2019

                        $.ajax({
                            type: "POST",
                            url: "Services/Master.asmx/GetMultiUOMDetails",
                            
                            data: JSON.stringify({ orderid: orderid, action: actionQry, module: 'SalesChallan', strKey: ComponentNumber }),
                        
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {

                                gridPackingQty = msg.d;

                                if (msg.d != "") {
                                    var packing = SpliteDetails[20];
                                    var PackingUom = SpliteDetails[25];
                                    var PackingSelectUom = SpliteDetails[24];
                                    var arrobj = {};
                                    arrobj.productid = strProductID;
                                    arrobj.slno = slnoget;
                                    arrobj.Quantity = Quantity;
                                    arrobj.packing = gridPackingQty;
                                    arrobj.PackingUom = PackingUom;
                                    arrobj.PackingSelectUom = PackingSelectUom;

                                    aarr.push(arrobj);
                                    
                                }
                            }
                        });
                    }
                }
            }
        }

    }
}

function Save_ButtonClick() {

    var flag = true;
    LoadingPanel.Show();
    $('#hfControlData').val($('#hfControlSaveData').val());
    var OrderNo = ctxt_SlChallanNo.GetText();
    var slsdate = cPLSalesChallanDate.GetValue();
    var qudate = cPLQADate.GetText();
    var customerid = GetObjectID('hdnCustomerId').value;
    var salesorderDate = new Date(slsdate);

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
    }

    var quotationDate = "";
    if (qudate != null && qudate != '') {
        var qd = qudate.split('-');
        LoadingPanel.Hide();
        quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

    }
    if (customerid == null || customerid == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }

    if (slsdate == null || slsdate == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlDate').attr('style', 'display:block');
    }
    else {
        $('#MandatorySlDate').attr('style', 'display:none');
        if (qudate != null && qudate != '') {
            var qd = qudate.split('-');
            quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

            if (quotationDate > salesorderDate) {

                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
    }

    if (OrderNo == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlOrderNo').attr('style', 'display:block');
    }
    else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }

    if (flag) {

        SetArrForUOM(); //For UOM Conversion Surojit
        SaveSendUOM('SC');

        if (grid.GetVisibleRowsOnPage() > 0) {

            if (issavePacking == 1) {
                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ProjectIssueMaterials.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var customerval = $("#hdnCustomerId").val();
                            $('#hdfLookupCustomer').val(customerval);


                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                            $('#hdnRefreshType').val('N');
                        }
                    });
                }
                else {
                    var customerval = $("#hdnCustomerId").val();
                    $('#hdfLookupCustomer').val(customerval);


                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                    cacbpCrpUdf.PerformCallback();
                    $('#hdnRefreshType').val('N');
                }
            }
            else {

                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ProjectIssueMaterials.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var customerval = $("#hdnCustomerId").val();
                            $('#hdfLookupCustomer').val(customerval);


                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                            $('#hdnRefreshType').val('N');
                        }
                    });
                }
                else {
                    var customerval = $("#hdnCustomerId").val();
                    $('#hdfLookupCustomer').val(customerval);


                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                    cacbpCrpUdf.PerformCallback();
                    $('#hdnRefreshType').val('N');
                }
            }
        }
        else {
            LoadingPanel.Hide();
            jAlert('You must enter proper details before save');
        }
    }
    // return flag;
}


function SaveExit_ButtonClick() {
    //debugger;
    var flag = true;
    LoadingPanel.Show();
    $('#hfControlData').val($('#hfControlSaveData').val());
    $("#hddnSaveOrExitButton").val('Save_Exit');

    var OrderNo = ctxt_SlChallanNo.GetText();
    var slsdate = cPLSalesChallanDate.GetValue();
    var qudate = cPLQADate.GetText();
    var customerid = GetObjectID('hdnCustomerId').value;
    var salesorderDate = new Date(slsdate);

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
    }

    var quotationDate = "";
    if (qudate != null && qudate != '') {
        //LoadingPanel.Hide();
        var qd = qudate.split('-');
        quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

    }
    if (customerid == null || customerid == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }



    if (slsdate == null || slsdate == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlDate').attr('style', 'display:block');
    }
    else {
        $('#MandatorySlDate').attr('style', 'display:none');
        if (qudate != null && qudate != '') {
            var qd = qudate.split('-');
            quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

            if (quotationDate > salesorderDate) {

                flag = false;
                LoadingPanel.Hide();
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
    }

    if (OrderNo == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlOrderNo').attr('style', 'display:block');
    }
    else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }


    if (flag) {

        SetArrForUOM(); //For UOM Conversion Surojit
        SaveSendUOM('SC');
        if (grid.GetVisibleRowsOnPage() > 0) {
            if (issavePacking == 1) {


                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ProjectIssueMaterials.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var customerval = $("#hdnCustomerId").val();
                            $('#hdfLookupCustomer').val(customerval);
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                            $('#hdnRefreshType').val('E');
                        }
                    });
                }
                else {
                    var customerval = $("#hdnCustomerId").val();
                    $('#hdfLookupCustomer').val(customerval);
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    cacbpCrpUdf.PerformCallback();
                    $('#hdnRefreshType').val('E');
                }
            }
            else {

                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ProjectIssueMaterials.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            var customerval = $("#hdnCustomerId").val();
                            $('#hdfLookupCustomer').val(customerval);
                           
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                            $('#hdnRefreshType').val('E');
                        }
                    });
                }
                else {
                    var customerval = $("#hdnCustomerId").val();
                    $('#hdfLookupCustomer').val(customerval);
                   
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                    cacbpCrpUdf.PerformCallback();
                    $('#hdnRefreshType').val('E');
                }
            }
        }
        else {
            LoadingPanel.Hide();
            jAlert('You must enter proper details before save');
        }
    }
}



function QuantityTextChange(s, e) {
   
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();

    var Id = grid.GetEditor('Quotation_No').GetValue();
    $.ajax({
        type: "POST",
        url: "ProjectIssueMaterials.aspx/CheckBalQuantity",
        data: JSON.stringify({ Id: Id, ProductID: ProductID.split('||@||')[0] }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {


            var ObjData = msg.d;
            if (ObjData.length > 0) {
                var balQty = ObjData[0].split('|')[0];
                if ((QuantityValue * 1) > (balQty * 1)) {
                    var OrdeMsg = 'Balance Quantity of selected Product from tagged document is <' + ObjData + '>.Cannot enter quantity more than balance quantity.';
                }
            }
        }
    });

    

    var SpliteDetails = ProductID.split("||@||");
    var strMultiplier = SpliteDetails[7];
    var strFactor = SpliteDetails[8];
   
    var strRate = "1";
    var strStkUOM = SpliteDetails[4];
   
    var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    if (strSalePrice == '0') {
        strSalePrice = SpliteDetails[6];
    }


    if (strRate == 0) {
        strRate = 1;
    }

    var StockQuantity = strMultiplier * QuantityValue;
    var Amount = QuantityValue * strFactor * strRate * (strSalePrice * 1);

    $('#lblStkQty').text(StockQuantity);
    $('#lblStkUOM').text(strStkUOM);

    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

    if (IsPackingActive == "Y") {
        $('#lblPackingStk').text(PackingValue);
        console.log('jhsdfafa');
        $('#divPacking').css({ 'display': 'block' });
    } else {
        divPacking.style.display = "none";
    }

    var tbAmount = grid.GetEditor("Amount");
    tbAmount.SetValue(Amount);

    var TotaAmountRes = '';
    TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(Amount + (TotaAmountRes * 1));

    DiscountTextChange(s, e);
}

function AddBatchNew(s, e) {

    grid.batchEditApi.EndEdit();
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode === 13) {
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i;
        var cnt = 2;

        grid.AddNewRow();
        if (noofvisiblerows == "0") {
            grid.AddNewRow();
        }
        grid.SetFocusedRowIndex();

        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            cnt++;
        }

        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);
    }
}
function OnAddNewClick() {

    grid.AddNewRow();
    var noofvisiblerows = grid.GetVisibleRowsOnPage();
    var i;
    var cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);
        cnt++;
    }   
}


function FinalRemarks() {

    //ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + grid.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
    InlineRemarksAddEdit("RemarksFinal", grid.GetEditor('SrlNo').GetValue(), $('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');
}


function closeRemarks(s, e) {
    cPopup_InlineRemarks.Hide();
}

function FinalWarehouse() {
  
    cGrdWarehouse.PerformCallback('WarehouseFinal');
    grid.batchEditApi.StartEdit(globalRowIndex, 10);
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');
}

function TaxAmountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
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
    var AltQty = "0";
    var AltUOM = "0";

    if ($("#hdnShowUOMConversionInEntry").val() == "1") {
         AltQty = (CtxtPacking.GetText() != null) ? CtxtPacking.GetText() : "0";
         AltUOM = (ccmbPackingUom1.GetValue() != null) ? ccmbPackingUom1.GetValue() : "0";
    }
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
                checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
                ctxtQuantity.SetValue("0");
                CtxtPacking.SetValue("0");
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
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
            CtxtPacking.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID + '~' + AltQty + '~' + AltUOM);
        SelectedWarehouseID = "0";
    }
}


function txtserialTextChanged() {
    //debugger;
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

        var WarehouseID = $("#hddnWarehouseId").val();
        var BatchID = $("#hddnBatchId").val();
        var FifoExists = $("#hddnConfigVariable_Val").val();
        var MatchQty = $("#hddnMatchQty").val();
        var type = document.getElementById('hdfProductType').value;

        if (type == "WBS" || type == "WB") {
           
            if (FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "NoFIFO");
            }
            else if (FifoExists == "1") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + (MatchQty * 1));
            }

        }
        else if (type == "BS") {

            if (FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + "NoFIFO");
            }
            else if (FifoExists == "1") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + (MatchQty * 1));
            }
        }
        else if (type == "WS") {
            
            if (FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + "NoFIFO");
            }
            else if (FifoExists == "1") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + (MatchQty * 1));
            }

        }
        UpdateSelectAllItemState();
        UpdateText();
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
        cbtnWarehouse.SetVisible(false);
        checkComboBox.Focus();
    }
    else {
        divSingleCombo.style.display = "none";
        divMultipleCombo.style.display = "block";
        cbtnWarehouse.SetVisible(true);
        ctxtserial.Focus();
    }
}


function fn_Deletecity(keyValue) {
    //debugger;
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    var FifoExists = $("#hddnConfigVariable_Val").val();

    var Qty = ctxtMatchQty.GetValue();
    var hddnQty = $("#hddnWarehouseQty").val();
  
    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    if (FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + 'NoFIFO');
    }

}

function clookup_Project_LostFocus() {
    grid.batchEditApi.StartEdit(-1, 2);

    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}

$(document).ready(function () {

    if (document.getElementById('hdnPageStatus').value == "update") {
        clookup_Project.SetEnabled(false);
    }

    $('#ddl_VatGstCst_I').blur(function () {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    })

    $('#ddl_AmountAre_I').blur(function () {
        var key = cddl_AmountAre.GetValue();
        if (key == 1 || key == 3) {
            if (grid.GetVisibleRowsOnPage() == 1) {
                if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                    clookup_Project.SetFocus();
                }
                else {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            }
        }
    })
});

var textSeparator = ";";
var selectedChkValue = "";
var IsSelected = false;
function OnListBoxSelectionChanged(listBox, args) {
    
    var selectedItems = checkListBox.GetSelectedItems();
    if (args.index == 0) {
        args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
        if (args.isSelected) {
            IsSelected = true;
        }
        else {
            IsSelected = false;
        }
    }
    var k = 0;

    var qty = grid.GetEditor('Quantity').GetValue();
    var selectedItems = checkListBox.GetSelectedItems();
    var val = GetSelectedItemsText(selectedItems);

    UpdateSelectAllItemState();
    UpdateText();

    var strWarehouse = cCmbWarehouse.GetValue();
    var strBatchID = cCmbBatch.GetValue();
    var ProducttId = $("#hdfProductID").val();
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
    var ActualLength = (checkListBox.GetItemCount() * 1);
    if (IsSelected == true || ActualLength == selectedItems.length) {
        checkComboBox.SetText(((selectedItems.length * 1) - 1) + " Items");
    }
    else {
        checkComboBox.SetText((selectedItems.length) + " Items");
    }

    var val = GetSelectedItemsText(selectedItems);
    $("#abpl").attr('data-content', val);
}

function SynchronizeListBoxValues(dropDown, args) {
    checkListBox.UnselectAll();
    
    var texts = selectedChkValue.split(textSeparator);
    var values = GetValuesByTexts(texts);
    checkListBox.SelectValues(values);
    UpdateSelectAllItemState();
    UpdateText(); 
}
function GetSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.join(textSeparator);
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

$(function () {
    $("#btnAdd").bind("click", function () {
        $("#SerialContainer").empty();
        $("#BatchContainer").empty();
        $("#SerialContainer").append("<div><span>Serial Number</span><div />");
        $("#BatchContainer").append("<div><span>Batch Number</span><div />");

        var count = ctxtQuantity_Warehouse.GetValue();


        for (var i = 1; i <= count; i++) {
            var div = $("<div />");
            div.html(GetDynamicSerial(""));
            $("#SerialContainer").append(div);

            var div1 = $("<div />");
            div1.html(GetDynamicBatch(""));
            $("#BatchContainer").append(div1);
        }
    });
});

function GetDynamicSerial(value) {
    return '<input name = "SerialContainer" type="text" value = "' + value + '" />'
}

function GetDynamicBatch(value) {
    return '<input name = "BatchContainer" type="text" value = "' + value + '" />'
}


function selectValue() {

    var startDate = new Date();
    startDate = cPLSalesChallanDate.GetValueString();
    var key = $("#hdnCustomerId").val();
    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";



    if (key != null && key != '' && type != "") {
        cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);
    }



    var componentType = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
    if (componentType != null && componentType != '') {
        //grid.PerformCallback('GridBlank');
        deleteAllRows();
        grid.AddNewRow();
        grid.GetEditor('SrlNo').SetValue('1');
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


function OrderNumberChanged() {
    if (SimilarProjectStatus != "-1") {
        var quote_Id = gridSalesOrderLookup.GetValue();

        if (quote_Id != null) {
            var arr = quote_Id.split(',');

            if (arr.length > 1) {
                cPLQADate.SetText('Multiple Select Order Dates');
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
            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

            if (type != '' && type != null) {
                cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@' + '~' + type);
                cProductsPopup.Show();
            }
        }
        else {
            cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
            cProductsPopup.Show();
        }

        txt_OANumber.Focus();
    }
}

function GridCallBack() {
    $('#ddlInventory').focus();
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
            url: "ProjectIssueMaterials.aspx/SetProjectCode",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var Code = msg.d;

                clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                clookup_Project.SetEnabled(false);
            }
        });

        var projID = clookup_Project.GetValue();

        $.ajax({
            type: "POST",
            url: 'ProjectIssueMaterials.aspx/getHierarchyID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ProjID: projID }),
            success: function (msg) {
                var data = msg.d;
                $("#ddlHierarchy").val(data);
            }
        });
    }
}

function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var url = '';
        var keyval = $('hdnmodeId').val();
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SC&&KeyVal_InternalID=' + keyval;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}

var GlobalCurTaxAmt = 0;
var rowEditCtrl;
var globalRowIndex;
var globalTaxRowIndex;
function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
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
                //cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
                cgridTax.GetEditor("Amount").SetValue(((ProdAmt * s.GetText()) / 100).toFixed(2));

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                //cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                cgridTax.GetEditor("Amount").SetValue((((ProdAmt * s.GetText()) / 100) * -1).toFixed(2));

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }
        }
    }
   
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

    ctxtTaxTotAmt.SetValue((totalInlineTaxAmount * 1).toFixed(2));
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

$(document).ready(function () {

    $('.generalTab').click(function () {
        $('.crossBtn').show();
    });
    $('.bilingTab').click(function () {
        if (!$(this).hasClass('dxtcLiteDisabled_PlasticBlue')) {
            $('.crossBtn').hide();
        }
    });
});

function disp_prompt(name) {

    if (name == "tab0") {
        ctxtCustName.Focus();
        page.GetTabByName('Billing/Shipping').SetEnabled(true);
    }
    if (name == "tab1") {
        var custID = GetObjectID('hdnCustomerId').value;
        page.GetTabByName('General').SetEnabled(false);
        if (custID == null && custID == '') {
            jAlert('Please select a customer');
            page.SetActiveTabIndex(0);
            return;
        }
        else {
            page.SetActiveTabIndex(1);
        }
    }
}


function ProjectValueChange(s, e) {
    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'ProjectIssueMaterials.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}

var InlineRemarksmsg = "";

function InlineRemarksAddEdit(Action, srl, AddlRemarks) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.Slno = srl;
    OtherDetail.AddlRemarks = AddlRemarks; 
    $.ajax({
        type: "POST",
        url: "ProjectIssueMaterials.aspx/InlineRemarks",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var Code = msg.d;

            if (Code != null) {
                if (Code.split('~')[0] == "200") {
                    InlineRemarksmsg = Code.split('~')[1];
                    if (Code.split('~')[1] == "DisplayRemarksFocus") {
                        $("#txtInlineRemarks").val(Code.split('~')[2]);
                        $("#txtInlineRemarks").focus();
                    }
                    else {
                        cPopup_InlineRemarks.Hide();
                    }
                }
            }
        }
    });
}


function ChkDataDigitCount(e) {
    var data = $(e).val();
    $(e).val(parseFloat(data).toFixed(4));
}

function ChangePackingByQuantityinjs() {
    if ($("#hdnShowUOMConversionInEntry").val() == "1")
    {
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = Productdetails.split("||@||");
    var otherdet = {};
    var ProductID = Productdetails.split("||@||")[0];
    otherdet.ProductID = ProductID;
    if (Productdetails != "") {
        $.ajax({
            type: "POST",
            url: "ProjectIssueMaterials.aspx/GetPackingQuantityWarehouse",
            data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {

                if (msg.d.length != 0) {
                    var packingQuantity = msg.d[0].packing_quantity;
                    var sProduct_quantity = msg.d[0].sProduct_quantity;
                    var isOverideConvertion = msg.d[0].isOverideConvertion;
                }
                else {
                    var packingQuantity = 0;
                    var sProduct_quantity = 0;
                    var isOverideConvertion = 0;
                }
                var uomfactor = 0
                if (sProduct_quantity != 0 && packingQuantity != 0) {
                    uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                    $('#hdnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                }
                else {
                    $('#hdnuomFactor').val(0);
                }

                $('#hdnpackingqty').val(packingQuantity);
                $('#hdnisOverideConvertion').val(isOverideConvertion);
                //var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
                //var Qty = $("#UOMQuantity").val();
                //var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

                ////$("#AltUOMQuantity").val(calcQuantity);

                //cAltUOMQuantity.SetValue(calcQuantity);

            }
        });
    }

    var Quantity = ctxtQuantity.GetValue();
    var packing = $('#txtAltQuantity').val();
    if (packing == null || packing == '') {
        $('#txtAltQuantity').val(parseFloat(0).toFixed(4));
        packing = $('#txtAltQuantity').val();
    }

    if (Quantity == null || Quantity == '') {
        $(e).val(parseFloat(0).toFixed(4));
        Quantity = ctxtQuantity.GetValue();
    }
    var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);

    //Rev Subhra 05-03-2019
    //var calcQuantity = parseFloat(Quantity * packingqty).toFixed(4);
    var uomfac_Qty_to_stock = $('#hdnuomFactor').val();
    //var uomfac_Qty_to_stock = $('#hdnpackingqty').val();
    var calcQuantity = parseFloat(Quantity * uomfac_Qty_to_stock).toFixed(4);
    //End of Rev Subhra 05-03-2019
    //$('#txtAlterQty1').val(calcQuantity);
    CtxtPacking.SetText(calcQuantity);

    ChkDataDigitCount(Quantity);
  }
}
function ChangeQuantityByPacking1() {
    if ($("#hdnShowUOMConversionInEntry").val() == "1") {
        var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var SpliteDetails = Productdetails.split("||@||");
        var otherdet = {};
        var ProductID = Productdetails.split("||@||")[0];
        otherdet.ProductID = ProductID;
        if (Productdetails != "") {
            $.ajax({
                type: "POST",
                url: "ProjectIssueMaterials.aspx/GetPackingQuantityWarehouse",
                data: JSON.stringify(otherdet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    if (msg.d.length != 0) {
                        var packingQuantity = msg.d[0].packing_quantity;
                        var sProduct_quantity = msg.d[0].sProduct_quantity;
                        var isOverideConvertion = msg.d[0].isOverideConvertion;
                    }
                    else {
                        var packingQuantity = 0;
                        var sProduct_quantity = 0;
                        var isOverideConvertion = 0;
                    }
                    var uomfactor = 0
                    if (sProduct_quantity != 0 && packingQuantity != 0) {
                        uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                        $('#hdnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                    }
                    else {
                        $('#hdnuomFactor').val(0);
                    }

                    $('#hdnpackingqty').val(packingQuantity);
                    $('#hdnisOverideConvertion').val(isOverideConvertion);
                    //var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
                    //var Qty = $("#UOMQuantity").val();
                    //var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

                    ////$("#AltUOMQuantity").val(calcQuantity);

                    //cAltUOMQuantity.SetValue(calcQuantity);

                }
            })

        }

        var isOverideConvertion = $('#hdnisOverideConvertion').val();
        if (isOverideConvertion == "true") {
            isOverideConvertion = '1';
        }
        if (isOverideConvertion == '1') {
            var packing = CtxtPacking.GetValue();
            var Quantity = ctxtQuantity.GetValue();
            if (packing == null || packing == '') {
                $(e).val(parseFloat(0).toFixed(4));
                packing = CtxtPacking.GetValue();
            }

            if (Quantity == null || Quantity == '') {
                ctxtQuantity.SetValue(parseFloat(0).toFixed(4));

                Quantity = ctxtQuantity.GetValue();
            }
            var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);


            //Rev Subhra 06-03-2019
            // var calcQuantity = parseFloat(packing / packingqty).toFixed(4);
            var uomfac_stock_to_qty = $('#hdnuomFactor').val();
            //var uomfac_stock_to_qty = $('#hdnpackingqty').val();
            //Rev Surojit 21-05-2019
            var calcQuantity = 0;
            if (parseFloat(uomfac_stock_to_qty) != 0) {
                calcQuantity = parseFloat(packing / uomfac_stock_to_qty).toFixed(4);
            }
            //End of Rev Surojit 21-05-2019

            //End of Rev Subhra 06-03-2019
            ctxtQuantity.SetValue(calcQuantity);
        }
        ChkDataDigitCount(Quantity);
    }
}
