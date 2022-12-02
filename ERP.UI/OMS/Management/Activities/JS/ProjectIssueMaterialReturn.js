$(function () {
    $('#UOMModal').on('hide.bs.modal', function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    });
});

function FinalRemarks() {
    ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + grid.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');
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
function closeRemarks(s, e) {
    cPopup_InlineRemarks.Hide();
}

function GlobalBillingShipping() {
    var startDate = new Date();
    startDate = tstartdate.GetDate().format('yyyy/MM/dd');
    if (gridquotationLookup.GetValue() != null) {
        var key = $('#hdfLookupCustomer').val();
        if (key != null && key != '') {
            cContactPerson.PerformCallback('BindContactPerson~' + key);
            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
            if (key != null && key != '') {

                // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');
            }
            //grid.PerformCallback('GridBlank');
            deleteAllRows();
            grid.AddNewRow();
            grid.GetEditor('SrlNo').SetValue('1');
            ccmbGstCstVat.PerformCallback();
            ccmbGstCstVatcharge.PerformCallback();
            //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            deleteTax('DeleteAllTax', "", "")
        }
    }
    else {
        // var key = ctxtCustName.GetValue();
        var key = $('#hdfLookupCustomer').val();
        GetObjectID('hdnCustomerId').value = key;
        if (key != null && key != '') {
            cContactPerson.PerformCallback('BindContactPerson~' + key);
            // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%');                      
        }
    }
}

$(document).ready(function () {
    $('#txtReasonforChange').on('change', function () {
        LoadingPanel.Hide();
    });
    $('#txt_PLQuoteNo').on('change', function () {
        UniqueCodeCheck();
    });

});

var ProductGetQuantity = "0";
var ProductGetTotalAmount = "0";
var ProductSaleprice = "0";
var globalNetAmount = 0;
var ProductDiscount = "0";
function AmtGotFocus() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;
}
function AmtTextChange(s, e) {
    var grossamt = grid.GetEditor('Amount').GetValue();
    var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(parseFloat(grossamt) + parseFloat(_TotalTaxAmt));
    var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
        // grid.GetEditor('TaxAmount').SetValue(0);
        //ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
    }

    DiscountTextChange(s, e);

}

function Onddl_VatGstCstEndCallback(s, e) {
    if (s.GetItemCount() == 1) {
        cddlVatGstCst.SetEnabled(false);
    }
}

function CloseGridQuotationLookup() {
    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();
    gridquotationLookup.Focus();
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
            url: "ProjectIssueMaterialReturn.aspx/SetProjectCode",
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
            url: 'ProjectIssueMaterialReturn.aspx/getHierarchyID',
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

function QuotationNumberChanged() {
    debugger;
    var quote_Id = gridquotationLookup.GetValue();
    if (quote_Id != null) {
        var arr = quote_Id.split(',');
        if (arr.length > 1) {
            $('#txt_InvoiceDate').val('Multiple Select Invoice Dates');

        }
        else {
            if (arr.length == 1) {
                cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id);
            }
            else {
                $('#txt_InvoiceDate').val('');
            }
        }
    }
    else {

        $('#txt_InvoiceDate').val('');
    }

    if (quote_Id != null) {
        cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
        cProductsPopup.Show();
    }
    else {
        grid.PerformCallback('RemoveDisplay');

    }

    cddl_AmountAre.SetEnabled(false);
    ctxt_Rate.SetEnabled(false);
    document.getElementById("ddl_Currency").disabled = true;
}

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
    if (event.keyCode == 18) isCtrl = true;
    if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") {
        //  alert('kkk'); //run code for Alt + n -- ie, Save & New
        StopDefaultAction(e);
        Save_ButtonClick();
    }
    else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for alt+X -- ie, Save & Exit!                
        StopDefaultAction(e);
        SaveExit_ButtonClick();
    }

    else if (event.keyCode == 85 && event.altKey == true) { //run code for alt+U -- ie, Save & Exit!                 
        StopDefaultAction(e);
        OpenUdf();
    }
    else if (event.keyCode == 84 && event.altKey == true && getUrlVars().req != "V") { //run code for alt+T -- ie, Save & Exit!                  
        StopDefaultAction(e);
        Save_TaxesClick();
    }
    else if (event.keyCode == 79 && event.altKey == true) { //run code for alt + O -- ie, For billing shipping Samrat!   
        StopDefaultAction(e);
        if (page.GetActiveTabIndex() == 1) {
            ValidationBillingShipping();
        }
    }
}

document.onkeyup = function (e) {

    if (event.altKey == true) {
        switch (event.keyCode) {
            case 83:
                if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                    if (getUrlVars().req != "V") {
                        SaveVehicleControlData();
                    }
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
            case 78:
                StopDefaultAction(e);
                if (getUrlVars().req != "V") {
                    Save_ButtonClick();
                }
                break;
            case 88:
                StopDefaultAction(e);
                if (getUrlVars().req != "V") {
                    SaveExit_ButtonClick();
                }
                break;
            case 120:
                StopDefaultAction(e);
                if (getUrlVars().req != "V") {
                    SaveExit_ButtonClick();
                }
                break;
            case 84:
                StopDefaultAction(e);
                if (getUrlVars().req != "V") {
                    Save_TaxesClick();
                }
                break;
            case 85:
                OpenUdf();
                break;
        }
    }
}

//transporter
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}

function ValueSelected(e, indexName) {
    // debugger;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var Name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {

            if (indexName == "ProdIndex") {
                SetProduct(Id, Name);
            }
            else if (indexName == "ProdDisIndex") {
                SetDisProduct(Id, Name);
            }
            else if (indexName == "customerIndex") {
                SetCustomer(Id, Name);
            }
                //Chinmoy added below line
            else if (indexName == "BillingAreaIndex") {
                SetBillingArea(Id, name);
            }
            else if (indexName == "ShippingAreaIndex") {
                SetShippingArea(Id, name);
            }
            else if (indexName == "customeraddressIndex") {
                SetCustomeraddress(Id, name)
            }
            //End
            //if (indexName == "customerIndex") {
            //    SetCustomer(Id, Name)
            //}

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
            if (indexName == "ProdIndex") {
                $('#txtProdSearch').focus();
            }
            else if (indexName == "ProdDisIndex") {
                $('#txtProdDisSearch').focus();
            }
            else if (indexName == "customerIndex") {
                $('#txtCustSearch').focus();
            }
                //added by chinmoy
            else if (indexName == "BillingAreaIndex") {
                $('#txtbillingArea').focus();
            }
            else if (indexName == "ShippingAreaIndex") {
                $('#txtshippingArea').focus();
            }
            else if (indexName == "customeraddressIndex") {
                ('#txtshippingShipToParty').focus();
            }
            //End
            //if (indexName == "customerIndex")
            //    $('#txtCustSearch').focus();

        }
    }

}


function AfterSaveBillingShipiing(validate) {
    if ($("#Keyval_internalId").val() == "Add") {
        GetPurchaseForGstValue();
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


function GetPurchaseForGstValue() {

    var BillingDetails = $.grep(StateCodeList, function (e) { return e.add_addressType == "Billing" })
    var ShippingDetails = $.grep(StateCodeList, function (e) { return e.add_addressType == "Shipping" })


    cddlPosGstSReturn.ClearItems();
    if (cddlPosGstSReturn.GetItemCount() == 0) {

        //---------------Code commented start Mantis Id::0019502----------------------------------------
        //cddlPosGstSReturn.AddItem(GetShippingStateName() + '[Shipping]', "S");
        //cddlPosGstSReturn.AddItem(GetBillingStateName() + '[Billing]', "B");
        //---------------Code commented end Mantis Id::0019502----------------------------------------
        if (BillingDetails.length > 0) {
            cddlPosGstSReturn.AddItem(BillingDetails[0].state + '[Billing]', "B");
        }
        if (ShippingDetails.length > 0) {
            cddlPosGstSReturn.AddItem(ShippingDetails[0].state + '[Shipping]', "S");
        }

    }
    else if (cddlPosGstSReturn.GetItemCount() > 2) {
        cddlPosGstSReturn.ClearItems();
        //cddl_PosGstSalesOrder.RemoveItem(0);
        //cddl_PosGstSalesOrder.RemoveItem(0);
    }

    //if (cddlPosGstSReturn.GetItemCount()>0)
    //{
    //    cddlPosGstSReturn.SetSelectedIndex(1);
    //}

    if (cddlPosGstSReturn.GetItemCount() == 1) {
        cddlPosGstSReturn.SetSelectedIndex(1);
    }
    else if ((cddlPosGstSReturn.GetValue() == "" || cddlPosGstSReturn.GetValue() == null) && (BillingDetails.length > 0)) {
        cddlPosGstSReturn.SetValue("B");
        cddlPosGstSReturn.SetText(BillingDetails[0].state + "[Billing]");
    }

    else if ((cddlPosGstSReturn.GetValue() == "" || cddlPosGstSReturn.GetValue() == null) && (ShippingDetails.length > 0)) {
        cddlPosGstSReturn.SetValue("S");
        cddlPosGstSReturn.SetText(ShippingDetails[0].state + "[Shipping]");
    }

    //---------------Code commented start Mantis Id::0019502----------------------------------------
    //if (PosGstId == "" || PosGstId == null) {
    //    cddlPosGstSReturn.SetValue("S");
    //}
    //else {
    //    cddlPosGstSReturn.SetValue(PosGstId);
    //}
    //---------------Code commented end Mantis Id::0019502----------------------------------------
}


var PosGstId = "";
function PopulateSReturnPosGst(e) {

    PosGstId = cddlPosGstSReturn.GetValue();
    if (PosGstId == "B") {
        cddlPosGstSReturn.SetValue("B");
    }
    else if (PosGstId == "S") {
        cddlPosGstSReturn.SetValue("S");
    }
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
    debugger;
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

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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


    ctxtTaxTotAmt.SetValue(DecimalRoundoff(totalInlineTaxAmount, 2));
}

function txtPercentageLostFocus(s, e) {
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

function SetRunningTotal() {
    debugger;
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
        RecalCulateTaxTotalAmountInline();
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
                globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
                document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                ctxtTaxTotAmt.SetValue(0);
                ccmbGstCstVat.SetSelectedIndex(0);
                $('.RecalculateInline').hide();
                caspxTaxpopUp.Show();
                //Set Product Gross Amount and Net Amount

                var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
                //var strRate = "1";
                document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                var strStkUOM = SpliteDetails[4];
                // var strSalePrice = SpliteDetails[6];
                var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                if (strRate == 0) {
                    strRate = 1;
                }

                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
                var discountAmt = (grid.GetEditor('Discount').GetValue() / 100);
                var netAmt = Amount - (Amount * discountAmt);
                clblTaxProdGrossAmt.SetText(parseFloat((Amount * 100) / 100).toFixed(2));
                clblProdNetAmt.SetText(parseFloat(Math.round(netAmt * 100) / 100).toFixed(2));
                document.getElementById('HdProdGrossAmt').value = Amount;
                document.getElementById('HdProdNetAmt').value = netAmt;
                if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                    var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
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
                    var gstRate = 0;
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
                    //Get Customer Shipping StateCode
                    var shippingStCode = '';
                    shippingStCode = GeteShippingStateCode();// CmbState1.GetText();                  

                    //Debjyoti 09032017
                    if (shippingStCode.trim() != '') {
                        for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                            //Check if gstin is blank then delete all tax
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {

                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {

                                    //if its state is union territories then only UTGST will apply
                                    if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "7" || shippingStCode == "31" || shippingStCode == "34") {
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
    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        cgridTax.UpdateEdit();
    }
    else {
        cgridTax.PerformCallback('SaveGST');
    }
    return false;
}

function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}

$(document).ready(function () {
    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
        page.GetTabByName('Billing/Shipping').SetEnabled(false);
    }
    $('#ApprovalCross').click(function () {
        //  ;
        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.Refresh()();
    })
})

function GetBillingAddressDetailByAddressId(e) {
    var addresskey = billingLookup.GetGridView().GetRowKey(billingLookup.GetGridView().GetFocusedRowIndex());
    if (addresskey != null && addresskey != '') {
        ///   ;
        cComponentPanel.PerformCallback('BlookupEdit~' + addresskey);
    }
}

function GetShippingAddressDetailByAddressId(e) {
    var saddresskey = shippingLookup.GetGridView().GetRowKey(shippingLookup.GetGridView().GetFocusedRowIndex());
    if (saddresskey != null && saddresskey != '') {

        cComponentPanel.PerformCallback('SlookupEdit~' + saddresskey);
    }
}

function UniqueCodeCheck() {
    //debugger;
    var SchemeVal = $('#ddl_numberingScheme option:selected').val();
    if (SchemeVal == "") {
        alert('Please Select Numbering Scheme');

        $('#txt_PLQuoteNo').val('');
        $('#txt_PLQuoteNo').focus();
    }
    else {

        var ReturnNo = $('#txt_PLQuoteNo').val();
        if (ReturnNo != '') {
            var SchemaLength = GetObjectID('hdnSchemaLength').value;
            var x = parseInt(SchemaLength);
            var y = parseInt(ReturnNo.length);

            if (y > x) {
                alert('Material Received No length cannot be more than ' + x);
                $('#txt_PLQuoteNo').val('');
                $('#txt_PLQuoteNo').focus();

            }
            else {
                var CheckUniqueCode = false;
                $.ajax({
                    type: "POST",
                    url: "ProjectIssueMaterialReturn.aspx/CheckUniqueCode",
                    data: JSON.stringify({ ReturnNo: ReturnNo }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            alert('Please enter unique Material Received No');
                            $('#txt_PLQuoteNo').val('');
                            $('#txt_PLQuoteNo').focus();
                        }
                        else {
                            $('#MandatorysQuoteno').attr('style', 'display:none');
                        }
                    }

                });
            }
        }
    }
}


function GetContactPersonPhone(e) {
    var key = cContactPerson.GetValue();
    cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
}

$(document).ready(function () {
    var schemaid = $('#ddl_numberingScheme').val();
    if (schemaid != null) {
        if (schemaid == '') {
            document.getElementById('txt_PLQuoteNo').disabled = true;
        }
    }
    $('#ddl_numberingScheme').change(function () {
        var NoSchemeTypedtl = $(this).val();
        var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
        var quotelength = NoSchemeTypedtl.toString().split('~')[2];
        var branchID = NoSchemeTypedtl.toString().split('~')[3];
        var fromDate = NoSchemeTypedtl.toString().split('~')[5];
        var todate = NoSchemeTypedtl.toString().split('~')[6];
        document.getElementById('ddl_Branch').value = branchID;

        var dt = new Date();

        tstartdate.SetDate(dt);

        if (dt < new Date(fromDate)) {
            tstartdate.SetDate(new Date(fromDate));
        }

        if (dt > new Date(todate)) {
            tstartdate.SetDate(new Date(todate));
        }

        tstartdate.SetMinDate(new Date(fromDate));
        tstartdate.SetMaxDate(new Date(todate));


        if (NoSchemeType == '1') {
            $('#txt_PLQuoteNo').val('Auto');
            //var d = new Date();

            //var strDate = d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getDate();

            //tstartdate.SetDate(strDate);
            document.getElementById('txt_PLQuoteNo').disabled = true;
            //20-05-2019 Surojit
            if ($('#hdnPostingDateDisable').val() == "1") {
                tstartdate.SetEnabled(false);
            }
            else {
                tstartdate.SetEnabled(true);
            }
            //20-05-2019 Surojit
            tstartdate.Focus();
        }
        else if (NoSchemeType == '0') {
            document.getElementById('txt_PLQuoteNo').disabled = false;
            tstartdate.SetEnabled(true);
            $('#txt_PLQuoteNo').maxLength = quotelength;
            $('#txt_PLQuoteNo').val('');
            $('#txt_PLQuoteNo').focus();

        }
        else {
            $('#txt_PLQuoteNo').val('');
            document.getElementById('txt_PLQuoteNo').disabled = true;
            //tstartdate.SetEnabled(false);
            //20-05-2019 Surojit
            if ($('#hdnPostingDateDisable').val() == "1") {
                tstartdate.SetEnabled(false);
            }
            else {
                tstartdate.SetEnabled(true);
            }
            //20-05-2019 Surojit

        }
        clookup_Project.gridView.Refresh();
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

function PopulateGSTCSTVAT(e) {
    var key = cddl_AmountAre.GetValue();

    if (key == 1) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(false);

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
        cddlVatGstCst.SetSelectedIndex(0);
        cddlVatGstCst.SetEnabled(false);
        cbtn_SaveRecords.SetVisible(false);
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    }
}

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
        cacbpCrpUdf.PerformCallback();
        //kaushik
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

function ShowCustom() {
    cPopup_wareHouse.Show();
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
        cacbpCrpUdf.PerformCallback();
        //kaushik
        grid.PerformCallback('CurrencyChangeDisplay');
    }
}

function ProductsCombo_SelectedIndexChanged(s, e) {
    $("#pageheaderContent").attr('style', 'display:block');
    cddl_AmountAre.SetEnabled(false);
    ctxt_Rate.SetEnabled(false);
    document.getElementById("ddl_Currency").disabled = true;
    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");

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

    //Debjyoti
    // ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "")
}


function Save_ButtonClick() {
    grid.AddNewRow();
    LoadingPanel.Show();
    flag = true;
    grid.batchEditApi.EndEdit();
    // Quote no validation Start
    var ReasonforRet = $('#txtReasonforChange').val();
    ReasonforRet = ReasonforRet.trim();
    if (ReasonforRet == '' || ReasonforRet == null) {
        $('#MandatoryReasonforChange').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatoryReasonforChange').attr('style', 'display:none');
    }
    //if ((cddlPosGstSReturn.GetValue() == "") || (cddlPosGstSReturn.GetValue() == null)) {
    //    jAlert('Please enter valid place of supply.')
    //    flag = false;
    //}
    var QuoteNo = $('#txt_PLQuoteNo').val();
    QuoteNo = QuoteNo.trim();
    if (QuoteNo == '' || QuoteNo == null) {
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }
    // Quote no validation End

    //var invoice_Id = gridquotationLookup.GetValue();

    //if (invoice_Id == null) {
    //    $('#MandatorysSCno').attr('style', 'display:block');
    //    flag = false;

    //}
    //else {
    //    $('#MandatorysSCno').attr('style', 'display:none');
    //}

    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);

    // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {

        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
    // Quote Customer validation End
    var amtare = cddl_AmountAre.GetValue();
    if (amtare == '2') {
        //Rev Tanmoy 01-08-2019 For Material Issue Return not save
        //var taxcodeid = cddlVatGstCst.GetValue();
        //if (taxcodeid == '' || taxcodeid == null) {
        //    $('#Mandatorytaxcode').attr('style', 'display:block');
        //    flag = false;
        //}
        //else {
        //    $('#Mandatorytaxcode').attr('style', 'display:none');
        //}
        //End Rev
    }
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
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
                    debugger;
                    $.ajax({
                        type: "POST",
                        url: "ProjectIssueMaterialReturn.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //   var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";

                            $('#hdfLookupCustomer').val(customerval);
                            // Custom Control Data Bind

                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('N');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                            //kaushik
                        }
                    });
                }
                else {


                    //   var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";

                    $('#hdfLookupCustomer').val(customerval);
                    // Custom Control Data Bind

                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('N');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    cacbpCrpUdf.PerformCallback();
                    //kaushik
                }
            }
            else {

                if (aarr.length > 0) {
                    debugger;
                    $.ajax({
                        type: "POST",
                        url: "ProjectIssueMaterialReturn.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //   var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";

                            $('#hdfLookupCustomer').val(customerval);
                            // Custom Control Data Bind

                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('N');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                            //kaushik
                        }
                    });
                }
                else {
                    //   var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";

                    $('#hdfLookupCustomer').val(customerval);
                    // Custom Control Data Bind

                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('N');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    cacbpCrpUdf.PerformCallback();
                    //kaushik
                }
            }
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            LoadingPanel.Hide();
        }
    }
    else { LoadingPanel.Hide(); }

}

function SaveExit_ButtonClick() {
    //debugger;
    grid.AddNewRow();
    LoadingPanel.Show();
    flag = true;
    grid.batchEditApi.EndEdit();
    // Quote no validation Start
    var ReasonforRet = $('#txtReasonforChange').val();
    ReasonforRet = ReasonforRet.trim();
    if (ReasonforRet == '' || ReasonforRet == null) {
        $('#MandatoryReasonforChange').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatoryReasonforChange').attr('style', 'display:none');
    }
    //if ((cddlPosGstSReturn.GetValue() == "") || (cddlPosGstSReturn.GetValue() == null)) {
    //    jAlert('Please enter valid place of supply.')
    //    flag = false;
    //}

    var QuoteNo = $('#txt_PLQuoteNo').val();
    QuoteNo = QuoteNo.trim();
    if (QuoteNo == '' || QuoteNo == null) {
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }
    // Quote no validation End

    //var invoice_Id = gridquotationLookup.GetValue();

    //if (invoice_Id == null) {
    //    $('#MandatorysSCno').attr('style', 'display:block');
    //    flag = false;

    //}
    //else {
    //    $('#MandatorysSCno').attr('style', 'display:none');
    //}
    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);

    // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
    // Quote Customer validation End

    var amtare = cddl_AmountAre.GetValue();
    if (amtare == '2') {
        //REV Tanmoy 01-08-2019  Material Issue Return no save
        //var taxcodeid = cddlVatGstCst.GetValue();
        //if (taxcodeid == '' || taxcodeid == null) {
        //    $('#Mandatorytaxcode').attr('style', 'display:block');
        //    flag = false;
        //}
        //else {
        //    $('#Mandatorytaxcode').attr('style', 'display:none');
        //}
        // End REV
    }
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
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
                        url: "ProjectIssueMaterialReturn.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {


                            // var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            $('#hdfLookupCustomer').val(customerval);
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                    });
                }
                else {

                    // var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    $('#hdfLookupCustomer').val(customerval);
                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    cacbpCrpUdf.PerformCallback();
                }
            }
            else {

                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ProjectIssueMaterialReturn.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            // var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            $('#hdfLookupCustomer').val(customerval);
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                    });
                }
                else {

                    // var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    $('#hdfLookupCustomer').val(customerval);
                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    cacbpCrpUdf.PerformCallback();
                }
            }

        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            LoadingPanel.Hide();
        }
    }
    else { LoadingPanel.Hide(); }
}

function SalePriceGotFocus() {
    ProductSaleprice = grid.GetEditor("SalePrice").GetValue();
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());

}

function QuantityGotFocus(s, e) {
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductGetQuantity = QuantityValue;
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());


    debugger;
    //Surojit 12-03-2019
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

    var isOverideConvertion = SpliteDetails[25];
    var packing_saleUOM = SpliteDetails[24];
    var sProduct_SaleUom = SpliteDetails[23];
    var sProduct_quantity = SpliteDetails[21];
    var packing_quantity = SpliteDetails[19];

    var slno = (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';
    var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
    var gridPackingQty = '';
    //var gridPackingQty = grid.GetEditor('QuoteDetails_PackingQty').GetText();
    if (SpliteDetails.length > 26) {
        if (SpliteDetails[26] == "1") {
            IsInventory = 'Yes';

            type = 'edit';

            if (SpliteDetails[28] != '') {
                if (parseFloat(SpliteDetails[28]) > 0) {
                    gridPackingQty = SpliteDetails[28];
                }
            }
        }
    }
    else {
        if (SpliteDetails.length == 26) {
            var isOverideConvertion = SpliteDetails[24];
            var packing_saleUOM = SpliteDetails[23];
            var sProduct_SaleUom = SpliteDetails[22];
            var sProduct_quantity = SpliteDetails[20];
            var packing_quantity = SpliteDetails[18];

            if (SpliteDetails[16] != '') {
                if (parseFloat(SpliteDetails[16]) > 0) {
                    gridPackingQty = SpliteDetails[16];
                }
            }
            if (SpliteDetails[24] == "1") {
                IsInventory = 'Yes';
            }
        }

    }

    var ComponentID = (grid.GetEditor('ComponentID').GetText() != null) ? grid.GetEditor('ComponentID').GetText() : "";


    if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 1) {
        if (ComponentID != "" && ComponentID != null && ComponentID != "0") {
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMultiUOMDetails",
                data: JSON.stringify({ orderid: strProductID, action: 'IssueReturnPackingQty', module: 'MaterialIssueReturn', strKey: ComponentID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    gridPackingQty = msg.d;

                    ShowUOM(type, "MaterialIssueReturn", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);

                }
            });
        }
        else {
            ShowUOM(type, "MaterialIssueReturn", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
        }
    }
    //Surojit 12-03-2019

}

var issavePacking = 0;

function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('Quantity').SetValue(Quantity);
    //QuantityTextChange(null, null);

    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }, 600);

}

function SetFoucs() {
    //debugger;
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
var IsInventory = '';

var fromColumn = '';
function QuantityTextChange(s, e) {
    //debugger;
    $("#pageheaderContent").attr('style', 'display:block');
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)) {
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        // var key = ctxtCustName.GetValue();
        var key = $('#hdnCustomerId').val();
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

                //if (CurrQty < 0) {
                //    grid.GetEditor("TotalQty").SetValue(TotalQty);
                //    grid.GetEditor("Quantity").SetValue(TotalQty);
                //    var OrdeMsg = 'Cannot enter quantity more than balance quantity.';
                //    grid.batchEditApi.EndEdit();
                //    jAlert(OrdeMsg, 'Alert Dialog: [Balance Quantity ]', function (r) {
                //        grid.batchEditApi.StartEdit(globalRowIndex, 7);
                //    });
                //    return false;
                //}
                //else {
                //    grid.GetEditor("TotalQty").SetValue(QuantityValue);
                //    grid.GetEditor("BalanceQty").SetValue(CurrQty);
                //}

                grid.GetEditor("TotalQty").SetValue(QuantityValue);
                grid.GetEditor("BalanceQty").SetValue(QuantityValue);
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
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount);
            DiscountTextChange(s, e);

        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Quantity').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}


/// Code Added By Sam on 23022017 after make editable of sale price field Start

function SalePriceTextChange(s, e) {
    $("#pageheaderContent").attr('style', 'display:block');
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";


    if (ProductSaleprice != parseFloat(Saleprice)) {
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            var strStkUOM = SpliteDetails[4];

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
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(amountAfterDiscount);

            $('#lblProduct').text(strProductName);
            $('#lblbranchName').text(strBranch);

            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;




            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#lblPackingStk').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }
            DiscountTextChange(s, e);

        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('SalePrice').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}


/// Code Above Added By Sam on 23022017 after make editable of sale price field End

function DiscountGotChange() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;

    ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductSaleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
}


function DiscountTextChange(s, e) {


    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var SalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {

        //if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(SalePrice) != parseFloat(ProductSaleprice)) {

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

        var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(amountAfterDiscount);

        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
            divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }


        // grid.GetEditor('TaxAmount').SetValue(0);
        var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
        var tbTotalAmount = grid.GetEditor("TotalAmount");

        tbTotalAmount.SetValue(parseFloat(amountAfterDiscount) + parseFloat(_TotalTaxAmt));

        var strTax = "";

        if (cddl_AmountAre.GetValue() == "1") {
            strTax = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            strTax = "I";

        }

        ShippingStateCode = cddlPosGstSReturn.GetValue()

        //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[25], Amount, amountAfterDiscount, strTax, ShippingStateCode, $('#ddl_Branch').val())
        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[17], Amount, amountAfterDiscount, strTax, ShippingStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), tstartdate.GetDate(), QuantityValue)

    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    //Debjyoti 
    var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";


    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
        //grid.GetEditor('TaxAmount').SetValue(0);

        //ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
        deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "")
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

            grid.batchEditApi.EndEdit();
            grid.AddNewRow();
            grid.SetFocusedRowIndex();
            var noofvisiblerows = grid.GetVisibleRowsOnPage();

            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
            grid.batchEditApi.StartEdit(globalRow_Index, 2);
        }
    }
}
function OnAddNewClick() {

    if (gridquotationLookup.GetValue() == null) {
        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        grid.GetEditor('Product').SetEnabled(true);
    }
    else {

        QuotationNumberChanged();
        grid.AddNewRow();
        grid.StartEditRow(0);
        grid.GetEditor('Product').SetEnabled(false);
    }
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

var Warehouseindex;
function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDelete') {
        var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
        grid.batchEditApi.EndEdit();

        $('#hdnDeleteSrlNo').val(SrlNo);
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        //if (gridquotationLookup.GetValue() != null) {
        //    var messege = "";
        //    messege = "Cannot Delete using this button as the Material Issue is linked with this Material Received.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
        //    jAlert(messege, 'Alert Dialog: [Delete Material Issue Products]', function (r) {
        //    });
        //}
        //else {
        //    if (noofvisiblerows != "1") {
        //        grid.DeleteRow(e.visibleIndex);
        //        $('#hdfIsDelete').val('D');
        //        grid.UpdateEdit();
        //        grid.PerformCallback('Display');
        //        $('#hdnPageStatus').val('delete');
        //    }
        if (noofvisiblerows != "1") {
                    grid.DeleteRow(e.visibleIndex);
                    $('#hdfIsDelete').val('D');
                    grid.UpdateEdit();
                    grid.PerformCallback('Display');
                    $('#hdnPageStatus').val('delete');
        }
    }

    else if (e.buttonID == "addlDesc") {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex, 6);
        cPopup_InlineRemarks.Show();

        $("#txtInlineRemarks").val('');

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        if (ProductID != "") {
            // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
            ccallback_InlineRemarks.PerformCallback('DisplayRemarks' + '~' + SrlNo + '~' + '0');
        }
    }

    else if (e.buttonID == 'AddNew') {
        //debugger;
        //if (gridquotationLookup.GetValue() == null) {
        //    var ProductIDValue = (grid.GetEditor('ProductDisID').GetText() != null) ? grid.GetEditor('ProductDisID').GetText() : "0";
        //    if (ProductIDValue != "") {
        //OnAddNewClick();
        //grid.batchEditApi.StartEdit(globalRowIndex, 2);
        //setTimeout(function () {
        //    grid.batchEditApi.StartEdit(globalRowIndex, 2);
        //}, 500);

        //return false;
        //    }
        //    else {
        //        grid.batchEditApi.StartEdit(e.visibleIndex, 2);
        //    }
        //}
        //else {
        //    QuotationNumberChanged();
        //}

        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        grid.GetEditor('Product').SetEnabled(true);

    } else if (e.buttonID == 'CustomWarehouse') {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)

        Warehouseindex = index;

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var ComponentID = (grid.GetEditor('ComponentID').GetValue() != null) ? grid.GetEditor('ComponentID').GetValue() : "0";

        $("#spnCmbWarehouse").hide();
        $("#spnCmbBatch").hide();
        $("#spncheckComboBox").hide();
        $("#spntxtQuantity").hide();

        if (ProductID != "" && parseFloat(QuantityValue) != 0) {
            var SpliteDetails = ProductID.split("||@||");
            //  alert(SpliteDetails);
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strMultiplier = SpliteDetails[7];
            var strProductName = strDescription;

            var StkQuantityValue = QuantityValue * strMultiplier;
            var Ptype = '';

            document.getElementById('lblProductName').innerHTML = strProductName;
            document.getElementById('txt_SalesAmount').innerHTML = QuantityValue;
            document.getElementById('txt_SalesUOM').innerHTML = strUOM;
            document.getElementById('txt_StockAmount').innerHTML = StkQuantityValue;
            document.getElementById('txt_StockUOM').innerHTML = strStkUOM;

            $('#hdfProductID').val(strProductID);
            $('#hdfProductSerialID').val(SrlNo);
            $('#hdnProductQuantity').val(QuantityValue);
            $('#hdfComponentID').val(ComponentID);

            $.ajax({
                type: "POST",
                url: 'ProjectIssueMaterialReturn.aspx/getProductType',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{Products_ID:\"" + strProductID + "\"}",
                success: function (type) {

                    Ptype = type.d;

                    $('#hdfProductType').val(Ptype);

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

                        jAlert("No Warehouse or Batch or Serial is actived !");
                    }
                }
            });


            if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                var objectToPass = {}
                var product = $("#hdfProductID").val();
                objectToPass.ProductID = $("#hdfProductID").val();
                $.ajax({
                    type: "POST",
                    url: "../Activities/Services/Master.asmx/GetUom",
                    data: JSON.stringify(objectToPass),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var returnObject = msg.d;
                        var UOMId = returnObject.uom_id;
                        var UOMName = returnObject.UOM_Name;
                        if (returnObject) {
                            SetDataSourceOnComboBoxandSetVal(ccmbPackingUom1, returnObject.uom, UOMId);
                            //ccmbPackingUom.SetValue(returnObject.uom_id);
                            //document.getElementById("cmbPackingUom1").disabled = true;
                            ccmbPackingUom1.SetEnabled(false);
                            //  document.getElementById("cmbPackingUom1").value = ProductID;
                        }
                    }
                });
            }
        }
        else if (ProductID != "" && parseFloat(QuantityValue) == 0) {

            jAlert("Please enter Quantity !");
        }
    }
}


function SetDataSourceOnComboBoxandSetVal(ControlObject, Source, id) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].UOM_Name, Source[count].UOM_Id);
    }
    ControlObject.SetValue(id);
    // ControlObject.SetSelectedIndex(0);
}

function fn_Deletecity(keyValue) {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
}
function fn_Edit(keyValue) {

    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}


function ctaxUpdatePanelEndCall(s, e) {


    //if (ctaxUpdatePanel.cpstock != null) {

    //    divAvailableStk.style.display = "block";
    //    var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
    //    document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
    //    ctaxUpdatePanel.cpstock = null;

    //}

    if (fromColumn == 'product') {
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
        fromColumn = '';
    }
    return;
}


function Save_TaxesClick() {
    //debugger;
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

    ctxtProductAmount.SetValue(parseFloat(Math.round(sumAmount * 100) / 100).toFixed(2));
    ctxtProductTaxAmount.SetValue(parseFloat(Math.round(sumTaxAmount * 100) / 100).toFixed(2));
    ctxtProductDiscount.SetValue(parseFloat(Math.round(sumDiscount * 100) / 100).toFixed(2));
    ctxtProductNetAmount.SetValue(parseFloat(Math.round(sumNetAmount * 100) / 100).toFixed(2));
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

}



function PercentageTextChange(s, e) {
    debugger;
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

//Set Running Total for Charges And Tax 
function SetChargesRunningTotal() {
    debugger;
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
                // ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));    
                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                // ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                GlobalTaxAmt = 0;
            }

            SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());


        }
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
        }
        else {
            runningTot = runningTot - parseFloat(gridTax.GetEditor("Amount").GetValue());
        }
        // ctxtTotalAmount.SetValue(runningTot);
        gridTax.batchEditApi.EndEdit();
    }
}

/////////////////// QuotationTaxAmountTextChange By Sam on 23022017
var taxAmountGlobalCharges;
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}


function QuotationTaxAmountTextChange(s, e) {
    debugger;
    //var Amount = ctxtProductAmount.GetValue();
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
    SetChargesRunningTotal();
}


function RecalCulateTaxTotalAmountCharges() {
    debugger;
    var totalTaxAmount = 0;
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        // var taxnamea = gridTax.GetEditor("TaxName").GetText();
        if (chargejsonTax[i].SchemeName != "") {
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                totalTaxAmount = totalTaxAmount + parseFloat(gridTax.GetEditor("Amount").GetValue());
            } else {
                totalTaxAmount = totalTaxAmount - parseFloat(gridTax.GetEditor("Amount").GetValue());
            }
        }
        gridTax.batchEditApi.EndEdit();
    }

    totalTaxAmount = totalTaxAmount + parseFloat(ctxtGstCstVatCharge.GetValue());

    ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
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






$(document).ready(function () {
    if ($('#hdnPageStatus').val() == "update") {
        PopulateSReturnPosGst();
        cddlPosGstSReturn.SetEnabled(false);
        LoadCustomerBillingShippingAddress(GetObjectID('hdnCustomerId').value);
        LoadBranchAddressInEditMode($('#ddl_Branch').val());
    }



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

    //grid.SetWidth(1350);

});

function deleteAllRows() {
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

function changedqntybatch(s) {

    var qnty = s.GetText();
    var sum = $('#hdntotalqntyPC').val();
    sum = Number(Number(sum) + Number(qnty));
    $('#hdntotalqntyPC').val(sum);
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
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
            CtxtPacking.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID + '~' + AltQty + '~' + AltUOM);
        SelectedWarehouseID = "0";
    }
}

var IsPostBack = "";
var PBWarehouseID = "";
var PBBatchID = "";

function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');
    //Rev Subhra 15-05-2019
    grid.batchEditApi.StartEdit(globalRowIndex, 8);
    //End of Rev Subhra 15-05-2019
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');//Subhabrata
}

var textSeparator = ";";
var selectedChkValue = "";

function OnListBoxSelectionChanged(listBox, args) {
    if (args.index == 0)
        args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
    UpdateSelectAllItemState();
    UpdateText();
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

    var ItemCount = GetSelectedItemsCount(selectedItems);
    checkComboBox.SetText(ItemCount + " Items");

    var val = GetSelectedItemsText(selectedItems);
    $("#abpl").attr('data-content', val);


}
function SynchronizeListBoxValues(dropDown, args) {
    checkListBox.UnselectAll();

    var texts = selectedChkValue.split(textSeparator);

    var values = GetValuesByTexts(texts);
    checkListBox.SelectValues(values);
    UpdateSelectAllItemState();
    UpdateText(); // for remove non-existing texts
}
function GetSelectedItemsCount(items) {

    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.length;
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

function ProductsGotFocus(s, e) {
    $("#pageheaderContent").attr('style', 'display:block');
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
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
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

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
        cacpAvailableStock.PerformCallback(strProductID);
    }
}

function PsGotFocusFromID(s, e) {
    $("#pageheaderContent").attr('style', 'display:block');
    divAvailableStk.style.display = "block";

    var ProductID = (grid.GetEditor('ProductDisID').GetText() != null && grid.GetEditor('ProductDisID').GetText() != "") ? grid.GetEditor('ProductDisID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];

    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}
function ProductsGotFocusFromID(s, e) {

    $("#pageheaderContent").attr('style', 'display:block');
    divAvailableStk.style.display = "block";

    var ProductdisID = (grid.GetEditor('ProductDisID').GetText() != null && grid.GetEditor('ProductDisID').GetText() != "") ? grid.GetEditor('ProductDisID').GetText() : "0";

    var ProductID = (grid.GetEditor('ProductID').GetText() != null && grid.GetEditor('ProductID').GetText() != "") ? grid.GetEditor('ProductID').GetText() : "0";
    var strProductName = (grid.GetEditor('ProductID').GetText() != null && grid.GetEditor('ProductID').GetText() != "") ? grid.GetEditor('ProductID').GetText() : "0";

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
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

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
        cacpAvailableStock.PerformCallback(strProductID);
    }
    else { cacpAvailableStock.PerformCallback(ProductdisID); }
}

function ProductlookUpdisKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUpdis.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 3);
    }
}


function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }
}

function ProductKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function ProductButnClick(s, e) {


    if (e.buttonIndex == 0) {
        var CID = GetObjectID('hdnCustomerId').value;
        if (CID != null && CID != "") {

            setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

            $('#txtProdSearch').val('');
            $('#ProductModel').modal('show');
        }
        else {
            jAlert("Please Select a Customer", "Alert", function () { ctxtCustName.Focus(); });
        }
    }
}

function ProductDisKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {

        s.OnButtonClick(0);
    }
}

function ProductDisButnClick(s, e) {

    if (e.buttonIndex == 0) {
        var CID = GetObjectID('hdnCustomerId').value;
        if (CID != null && CID != "") {

            setTimeout(function () { $("#txtProdDisSearch").focus(); }, 500);

            $('#txtProdDisSearch').val('');
            $('#ProductDisModel').modal('show');
        }
        else {
            jAlert("Please Select a Customer", "Alert", function () { ctxtCustName.Focus(); });
        }
    }
}


function SetProduct(Id, Name) {
    $('#ProductModel').modal('hide');

    var LookUpData = Id;
    var ProductCode = Name;

    if (!ProductCode) {
        LookUpData = null;
    }


    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    // console.log(LookUpData);
    $("#pageheaderContent").attr('style', 'display:block');
    cddl_AmountAre.SetEnabled(false);
    ctxt_Rate.SetEnabled(false);
    cddlPosGstSReturn.SetEnabled(false);
    document.getElementById("ddl_Currency").disabled = true;
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

    //tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    // tbSalePrice.SetValue(strSalePrice);
    //if (quote_Id == null) {
    tbSalePrice.SetValue(strSalePrice);
    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");
    //  }
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strDescription);
    $('#lblbranchName').text(strBranch);

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }
    // setStock(strProductID);

    fromColumn = 'product';
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "")
    grid.batchEditApi.StartEdit(globalRowIndex, 4);
}



function SetDisProduct(Id, Name) {
    $('#ProductDisModel').modal('hide');

    var LookUpData = Id;
    var ProductCode = Name;

    if (!ProductCode) {
        LookUpData = null;
    }

    grid.batchEditApi.StartEdit(globalRowIndex, 3);
    var productall = LookUpData.split('||')
    cddl_AmountAre.SetEnabled(false);
    ctxt_Rate.SetEnabled(false);
    document.getElementById("ddl_Currency").disabled = true;
    var productdsc = productall[0];
    grid.GetEditor("ProductDisID").SetText(productdsc);
    grid.GetEditor("Product").SetText(ProductCode);

    SetProduct(Id, Name);

    grid.batchEditApi.StartEdit(globalRowIndex, 3);

}
function ProductDisSelected(s, e) {

    var LookUpData = cproductDisLookUp.GetValue();
    if (LookUpData == null)
        return;

    var ProductCode = cproductDisLookUp.GetText();
    if (!ProductCode) {
        LookUpData = null;
    }
    cProductpopUpdis.Hide();

    grid.batchEditApi.StartEdit(globalRowIndex, 3);
    var productall = LookUpData.split('||')

    var productdsc = productall[0];
    grid.GetEditor("ProductDisID").SetText(productdsc);
    grid.GetEditor("Product").SetText(ProductCode);


    grid.batchEditApi.StartEdit(globalRowIndex, 3);
}

function ProductSelected(s, e) {


    if (!cproductLookUp.FindItemByValue(cproductLookUp.GetValue())) {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
        jAlert("Product not Exists.", "Alert", function () { cproductLookUp.SetValue(); cproductLookUp.Focus(); });
        return;
    }

    var LookUpData = cproductLookUp.GetValue();
    var ProductCode = cproductLookUp.GetText();
    var quote_Id = gridquotationLookup.GetValue();
    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    //  console.log(LookUpData);
    $("#pageheaderContent").attr('style', 'display:block');
    cddl_AmountAre.SetEnabled(false);
    ctxt_Rate.SetEnabled(false);
    document.getElementById("ddl_Currency").disabled = true;
    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");
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


    tbUOM.SetValue(strUOM);
    if (quote_Id == null) {
        tbSalePrice.SetValue(strSalePrice);
        grid.GetEditor("Quantity").SetValue("0.00");
        grid.GetEditor("Discount").SetValue("0.00");
        grid.GetEditor("Amount").SetValue("0.00");
        grid.GetEditor("TaxAmount").SetValue("0.00");
        grid.GetEditor("TotalAmount").SetValue("0.00");
    }
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strDescription);
    $('#lblbranchName').text(strBranch);

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }

    fromColumn = 'product';
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "")
    grid.batchEditApi.StartEdit(globalRowIndex, 7);
}

var oldadate;

function dateInit() {
}

function SettingTabStatus() {
    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
    }
}

function disp_prompt(name) {
    if (name == "tab0") {
        ctxtCustName.Focus();
    }
    if (name == "tab1") {
        page.tabs[0].SetEnabled(false);
        $('#divcross').hide();
        var custID = GetObjectID('hdnCustomerId').value;
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


var canCallBack = true;

function AllControlInitilize() {
    if (canCallBack) {

        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        grid.batchEditApi.EndEdit();
        $('#ddl_numberingScheme').focus();
        canCallBack = false;
    }
}

function CmbScheme_ValueChange() {
    PosGstId = "";
    ctxtCustName.SetText("");
}

function selectValue() {

    var checked = $('#rdl_SalesInvoice').attr('checked', true);
    if (checked) {
        $(this).attr('checked', false);
    }
    else {
        $(this).attr('checked', true);
    }
    var startDate = new Date();
    startDate = tstartdate.GetDate().format('yyyy/MM/dd');
    // var key = $("#hdnCustomerId").val();
    var key = $('#hdfLookupCustomer').val();
    GetObjectID('hdnCustomerId').value = key;
    var type = ($("[id$='rdl_SalesInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SalesInvoice']").find(":checked").val() : "";

    if (key == null || key == "") {
        jAlert("Customer required !", 'Alert Dialog: [Material Issue]', function (r) {
            if (r == true) {
                ctxtCustName.Focus();
            }
        });
        return;
    }
    if (key != null && key != '' && type != "") {
        cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%');
    }
}

function ProjectValueChange(s, e) {
    var projID = clookup_Project.GetValue();
    $.ajax({
        type: "POST",
        url: 'ProjectIssueMaterialReturn.aspx/getHierarchyID',
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

    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}

function deleteTax(Action, srl, productid) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;
    OtherDetail.prodid = productid;
    $.ajax({
        type: "POST",
        url: "ProjectIssueMaterialReturn.aspx/taxUpdatePanel_Callback",
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

function clookup_project_GotFocus() {
    clookup_Project.gridView.Refresh();
    clookup_Project.ShowDropDown();
}


function ChkDataDigitCount(e) {
    var data = $(e).val();
    $(e).val(parseFloat(data).toFixed(4));
}

function ChangePackingByQuantityinjs() {
    if ($("#hdnShowUOMConversionInEntry").val() == "1") {
        var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var SpliteDetails = Productdetails.split("||@||");
        var otherdet = {};
        var ProductID = Productdetails.split("||@||")[0];
        otherdet.ProductID = ProductID;
        if (Productdetails != "") {
            $.ajax({
                type: "POST",
                url: "ProjectIssueMaterialReturn.aspx/GetPackingQuantityWarehouse",
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


        var uomfac_Qty_to_stock = $('#hdnuomFactor').val();
        var calcQuantity = parseFloat(Quantity * uomfac_Qty_to_stock).toFixed(4);

        var setting = document.getElementById("hdnShowUOMConversionInEntry").value;
        if (setting == 1) {
            CtxtPacking.SetText(calcQuantity);
            ChkDataDigitCount(Quantity);
        }
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
                url: "ProjectIssueMaterialReturn.aspx/GetPackingQuantityWarehouse",
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


