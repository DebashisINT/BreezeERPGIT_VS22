var globalOldUnitTotalValue = 0;
var globalNetAmount = 0;
var CustomerCurrentDateAmount = 0;
var isExecutiveHasLedger = 0;
var taxAmountGlobal;
var GlobalCurChargeTaxAmt;
var ChargegstcstvatGlobalName;
var GlobalCurTaxAmt = 0;
var rowEditCtrl;
var globalRowIndex;
var globalTaxRowIndex;
var gstcstvatGlobalName;
var taxJson;

var Billing_state;
var Billing_city;
var Billing_pin;
var billing_area;

var Shipping_state;
var Shipping_city;
var Shipping_pin;
var Shipping_area;

var lastFinancer = null;
var lastSalesman = null;
var lastChallan = null;

var IsProduct = "";
var currentEditableVisibleIndex;
var preventEndEditOnLostFocus = false;
var lastProductID;
var setValueFlag;
var canCallBack = false;
var Warehouseindex;


AvailableStockClick = function () {
    if ($('#HDSelectedProduct').val() == "") {
        jAlert("Please select a Product First.");
    } else {
        cShowAvailableStock.Show();
        cAvailableStockgrid.PerformCallback();
    }
}


InvoiceExists = function () {
    jAlert("Selected Invoice Number Already Exists.", 'Alert', function () { ctxt_PLQuoteNo.SetEnabled(true); });

}


pinBillingChange = function (s, e) {
    if (CmbPin.GetValue() != null)
        cupdateBillingDetailsByPin.PerformCallback(CmbPin.GetValue());
}

pinShippingChange = function (s, e) {
    if (CmbPin1.GetValue() != null)
        cupdateShippingDetailsByPin.PerformCallback(CmbPin1.GetValue());
}

SetTotalDownPaymentAmount = function () {
    var totDownPay = parseFloat(ctxtEmiOtherCharges.GetValue()) + parseFloat(ctxtprocFee.GetValue()) + parseFloat(ctxtdownPayment.GetValue());
    ctxtTotDpAmt.SetValue(totDownPay);

    var InvoiceValue = parseFloat(cbnrLblInvValue.GetValue());
    ctxtFinanceAmt.SetValue(InvoiceValue - totDownPay);

}

SetDownPayment = function() {
    var InvoiceValue = parseFloat(cbnrLblInvValue.GetValue());
    var FinanceAmount = parseFloat(ctxtFinanceAmt.GetValue());

    ctxtdownPayment.SetValue(InvoiceValue - FinanceAmount);
}

SetOtherChargesLbl = function () {
    var finalOtherCharges = parseFloat(Math.round(ctxtQuoteTaxTotalAmt.GetValue() * 100) / 100).toFixed(2);
    if (finalOtherCharges == 0) {
        $('#otherChargesId').hide();
    } else {
        $('#otherChargesId').show();
    }
    cbnrOtherChargesvalue.SetValue(finalOtherCharges);
    SetRunningBalance();
}

ccmbExecNameEndCallBack = function () {
    if (ccmbExecName.cpFinancerHasLedger) {
        if (ccmbExecName.cpFinancerHasLedger != '') {
            isExecutiveHasLedger = parseFloat(ccmbExecName.cpFinancerHasLedger);
            ccmbExecName.cpFinancerHasLedger = null;
            if (isExecutiveHasLedger === 0) {
                jAlert("No ledger is mapped for the selected Financer.", "Alert", function () {
                    ccmbFinancer.Focus();
                });
            }
        }
    }
}

Updated = function () {
    jAlert('Updated Successfully.', 'Alert', function () {
        window.location.assign("PosSalesInvoiceList.aspx");
    });
}

ParentCustomerOnClose = function (newCustId, CustomerName, CustUniqueName) {
    AspxDirectAddCustPopup.Hide();
    if (newCustId.trim() != '') {
        page.SetActiveTabIndex(0);
        // cCustomerCallBackPanel.PerformCallback('SetCustomer~' + newCustId + '~' + CustomerName);
        var FullName = new Array(CustUniqueName, CustomerName);
        cCustomerComboBox.AddItem(FullName, newCustId);
        cCustomerComboBox.SetValue(newCustId);
        $('#DeleteCustomer').val("yes");
        GetContactPerson();
    }
}

SetInvoiceLebelValue = function() {

    var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());

    if (document.getElementById('HdPosType').value == 'Crd') {
        if (invValue < 0) {
            var newAdvAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
            cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(newAdvAmount) * 100) / 100).toFixed(2));
        }
    }

    if (document.getElementById('HdPosType').value == 'Fin') {
        if (invValue < 0) {
            var newAdvAmountfin = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
            cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(ctxtdownPayment.GetValue()) * 100) / 100).toFixed(2));
        }
    }



    if (document.getElementById('HdPosType').value == 'Crd')
        invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
    else if (document.getElementById('HdPosType').value == 'Fin')
        invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue()) + parseFloat(cbnrOtherChargesvalue.GetValue());


    cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));


    SetRunningBalance();

}

DiscountGotChange = function() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;
}


challanNoSchemeSelectedIndexChanged = function() {
    var schemeValue = cchallanNoScheme.GetValue();
    if (schemeValue == null) {
        ctxtChallanNo.SetEnabled(false);
        ctxtChallanNo.SetText('');
    }
    else if (schemeValue.split('~')[1] == '1') {
        ctxtChallanNo.SetEnabled(false);
        ctxtChallanNo.SetText('Auto');
    }
    else if (schemeValue.split('~')[1] == '0') {
        ctxtChallanNo.SetEnabled(true);
        ctxtChallanNo.SetText('');
    }
}

challanNoSchemeEndCallback = function () {
    if (lastChallan) {
        cchallanNoScheme.PerformCallback(lastChallan);
        lastChallan = null;
    }
}


CustomerReceiptEndCallback = function() {
    if (caspxCustomerReceiptGridview.cpCustomerReceiptTotalAmount) {
        if (caspxCustomerReceiptGridview.cpCustomerReceiptTotalAmount != '') {
            ctxtAdvnceReceipt.SetValue(caspxCustomerReceiptGridview.cpCustomerReceiptTotalAmount);
            cbnrLblLessAdvanceValue.SetValue(caspxCustomerReceiptGridview.cpCustomerReceiptTotalAmount);
            SetInvoiceLebelValue();
            caspxCustomerReceiptGridview.cpCustomerReceiptTotalAmount = null;


        }
    }

    if (caspxCustomerReceiptGridview.cpReceiptList != null) {
        $('#hdAddvanceReceiptNo').val(caspxCustomerReceiptGridview.cpReceiptList);
        caspxCustomerReceiptGridview.cpReceiptList = null;
    }

    if (caspxCustomerReceiptGridview.cpTotalTransectionAmount) {
        if (caspxCustomerReceiptGridview.cpTotalTransectionAmount != "") {
            CustomerCurrentDateAmount = parseFloat(caspxCustomerReceiptGridview.cpTotalTransectionAmount);
            caspxCustomerReceiptGridview.cpTotalTransectionAmount = null;
        }
    }

}


CustomerReceiptSaveandExitClick = function() {
    cpopupCustomerRecipt.Hide();
    caspxCustomerReceiptGridview.PerformCallback('SaveCustomerReceiptGridview');
    //    if (document.getElementById('HdPosType').value != 'Crd'  ) {
    ccmbUcpaymentCashLedger.Focus();
    //} else {
    //    cbtn_SaveRecords.Focus();
    //}

}


SelectAllCustomerReceipt = function() {
    caspxCustomerReceiptGridview.PerformCallback('SelectAllRecords');
}

UnSelectAllCustomerReceipt = function() {
    caspxCustomerReceiptGridview.PerformCallback('UnSelectAllRecords');
}

RevertCustomerReceipt = function() {
    caspxCustomerReceiptGridview.PerformCallback('Revert');
}

AdvanceReceiptOnClick = function() {
    //var custId = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
    var custId = cCustomerComboBox.GetValue();
    caspxCustomerReceiptGridview.PerformCallback('BindCustomerGridByInternalId~' + custId + '~' + tstartdate.GetDate().format('yyyy-MM-dd'));
    cpopupCustomerRecipt.Show();
}

onMainGridKeyPress = function (e) {
    console.log("key pressed", e.code);
    if (e.code == "Tab") {
        ccmbOldUnit.Focus();
    }
}

oldUnitGridClearClick = function() {
    ClearOldUnitData();
    coldUnitProductLookUp.Focus();
    $('#HdDiscountAmount').val('0');

}

oldunitPopupSaveAndEXitClick = function() {
    cOldUnitPopUpControl.Hide();
    ctxtRemarks.Focus();
}

oldUnitButtonShouldVissible = function () {
    if (ccmbOldUnit.GetValue() == "0")
        $('#OldUnitSelectionButton').hide();
    else
        $('#OldUnitSelectionButton').show();
}

ccmbOldUnitTextChanged = function() {
    oldUnitButtonShouldVissible();
    if (ccmbOldUnit.GetValue() == "1") {
        OldUnitButtonOnClick();
        coldUnitProductLookUp.Focus();
    } else {
        if (parseFloat(ctxtunitValue.GetValue()) > 0) {
            jConfirm("Old Unit already entered. Selecting 'No' will clear Old Unit data. Wish to proceed?", "Alert", function (data) {
                if (data == true) {
                    cOldUnitGrid.PerformCallback('DeleteAllRecord');
                } else {
                    ccmbOldUnit.SetValue('1');
                    $('#OldUnitSelectionButton').show();
                }
            });
        }
    }
}


oldUnitGridRowChange = function() {
    if (cOldUnitGrid.GetVisibleRowsOnPage() > 0) {
        if (document.getElementById('hdAddOrEdit').value != "Edit") {
            coldunitPopupSaveAndClickClick.SetVisible(true);
        }
    } else {
        coldunitPopupSaveAndClickClick.SetVisible(false);
    }
}

OldUnitGridEndCallback = function () {
    if (cOldUnitGrid.cpReturnString) {
        if (cOldUnitGrid.cpReturnString != "") {
            if (cOldUnitGrid.cpReturnString == 'AddDataToTable') {
                ClearOldUnitData();
                coldUnitProductLookUp.Focus();
                cOldUnitGrid.cpReturnString = null;
            }
        }
    }

    if (cOldUnitGrid.cpTotalOldUnit) {
        if (cOldUnitGrid.cpTotalOldUnit != "") {
            ctxtunitValue.SetValue(parseFloat(cOldUnitGrid.cpTotalOldUnit));
            cbnrLblLessOldMainVal.SetText(ctxtunitValue.GetText());
            SetInvoiceLebelValue();
            if (parseFloat(ctxtunitValue.GetValue()) == 0) {
                ccmbOldUnit.SetValue('0');
                $('#OldUnitSelectionButton').hide();
            } else {
                ccmbOldUnit.SetValue('1');
                $('#OldUnitSelectionButton').show();
            }
        }
    }
    oldUnitGridRowChange();
}

ClearOldUnitData = function () {
    coldUnitProductLookUp.Clear();
    ctxtOldUnitUom.SetText('');
    ctxtOldUnitqty.SetText('');
    ctxtoldUnitValue.SetText('');
}

OldUnitButtonOnClick = function() {
    cOldUnitPopUpControl.Show();
    coldUnitProductLookUp.Focus();
    cOldUnitGrid.PerformCallback('DisplayOldUnit');

}

oldUnitProductTextChanged = function(s, e) {
    var key = coldUnitProductLookUp.GetGridView().GetRowKey(coldUnitProductLookUp.GetGridView().GetFocusedRowIndex());
    ctxtOldUnitUom.SetText(key.split('|@|')[1]);
}


fn_EditOldUnit = function(keyVal) {
    coldUnitUpdatePanel.PerformCallback(keyVal);
}

fn_removeOldUnit = function (keyVal) {
    cOldUnitGrid.PerformCallback("DeleteFromTable~" + keyVal);
}


oldUnitGridAddClick = function () {
    $('#mandetoryOldUnit').attr('style', 'display:none');
    var focusedRow = coldUnitProductLookUp.gridView.GetFocusedRowIndex();

    var MRP = parseFloat(coldUnitProductLookUp.gridView.GetRow(focusedRow).children[5].innerText);

    if (coldUnitProductLookUp.GetValue() == null) {

        $('#mandetoryOldUnit').attr('style', 'display:block');
    }
    else if (MRP != 0 && ctxtoldUnitValue.GetValue() > MRP) {
        var roundOfValue = parseFloat(Math.round(Math.abs(MRP) * 100) / 100).toFixed(2);
        jAlert("Old Unit Value cannot be Greater then MRP defined.", "Alert", function () { ctxtoldUnitValue.Focus(); });
    }
    else {
        cOldUnitGrid.PerformCallback("AddDataToTable");
    }
}


OnfinancerEndCallback = function (s, e) {
    if (lastFinancer) {
        ccmbFinancer.PerformCallback(lastFinancer);
        lastFinancer = null;
    }
}

OnSalesAgentEndCallback = function (s, e) {
    if (lastSalesman) {
        cddl_SalesAgent.PerformCallback(lastSalesman);
        lastSalesman = null;
    } else {
        cmbUcpaymentCashLedgerChanged(ccmbUcpaymentCashLedger);
    }
}

financerIndexChange = function (s, e) {
    ccmbExecName.PerformCallback(ccmbFinancer.GetValue());
}


isDeliveryTypeChanged = function (s, e) {
    var type = s.GetValue();
    document.getElementById('ddDeliveredFrom').value = $('#sessionBranch').val();
    if (type == 'S') {
        $('#ddDeliveredFrom').attr('disabled', 'disabled');
    }
    else {
        $('#ddDeliveredFrom').attr('disabled', false);
    }

    if (type == "D") {

        cchallanNoScheme.SetEnabled(true);
        tstartdate.SetEnabled(true);

    } else {
        cchallanNoScheme.SetSelectedIndex(0);
        cchallanNoScheme.SetEnabled(false);
        ctxtChallanNo.SetEnabled(false);
        tstartdate.SetEnabled(false);
        tstartdate.SetDate(new Date);
        cdeliveryDate.SetDate(tstartdate.GetDate());
        DateCheck();

        if (isDeliveryTypeChanged != "") {
            if ($('#ddl_numberingScheme').val().split('~')[1] == "0") {
                tstartdate.SetEnabled(true);
            }
        }

    }

}


RecalCulateTaxTotalAmountInline = function () {
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
    //var roundedOfAmount = Math.round(totalInlineTaxAmount);
    var roundedOfAmount = totalInlineTaxAmount;
    ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);


    var diffDisc = roundedOfAmount - totalInlineTaxAmount;
    if (diffDisc > 0)
        document.getElementById('taxroundedOf').innerText = 'Adjustment ' + Math.abs(diffDisc.toFixed(3));
    else if (diffDisc < 0)
        document.getElementById('taxroundedOf').innerText = 'Adjustment ' + Math.abs(diffDisc.toFixed(3));
    else
        document.getElementById('taxroundedOf').innerText = '';
}


ShowTaxPopUp = function (type) {
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

gridFocusedRowChanged = function (s, e) {
    globalRowIndex = e.visibleIndex;
}


OnBatchEditEndEditing = function(s, e) {
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

TaxAmountKeyDown = function (s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

taxAmountGotFocus = function (s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}

taxAmountLostFocus = function (s, e) {
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


cmbGstCstVatChange = function(s, e) {

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
Onddl_VatGstCstEndCallback = function(s, e) {
    if (s.GetItemCount() == 1) {
        cddlVatGstCst.SetEnabled(false);
    }
}


ChargecmbGstCstVatChange = function(s, e) {

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


GetChargesTotalRunningAmount = function() {
    var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.batchEditApi.EndEdit();
    }

    return runningTot;
}


chargeCmbtaxClick = function(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}

GetVisibleIndex = function(s, e) {
    globalRowIndex = e.visibleIndex;
}

GetTaxVisibleIndex = function(s, e) {
    globalTaxRowIndex = e.visibleIndex;
}


cmbtaxCodeindexChange = function(s, e) {
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

SetOtherTaxValueOnRespectiveRow = function(idx, amt, name) {
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



SetOtherChargeTaxValueOnRespectiveRow = function (idx, amt, name) {
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


txtPercentageLostFocus = function(s, e) {

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



SetRunningTotal = function() {
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


GetTotalRunningAmount = function() {
    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        cgridTax.batchEditApi.EndEdit();
    }

    return runningTot;
}


CmbtaxClick = function (s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}


txtTax_TextChanged = function(s, i, e) {
    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
}


taxAmtButnClick = function(s, e) {
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

                //End Here

                //Set Discount Here
                if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                    var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
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
                    //   var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
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
                    if (cchkBilling.GetValue()) {
                        shippingStCode = $('#lblBillingState').val();//CmbState.GetText();
                    }
                    else {
                        shippingStCode = $('#lblShippingState').val();// CmbState1.GetText();
                    }
                    shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

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



taxAmtButnClick1 = function (s, e) {
    console.log(grid.GetFocusedRowIndex());
    rowEditCtrl = s;
}




BatchUpdate = function() {


    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        cgridTax.UpdateEdit();
    }
    else {
        cgridTax.PerformCallback('SaveGST');
    }
    return false;
}


cgridTax_EndCallBack = function (s, e) {
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
        grid.GetEditor("TaxAmount").SetValue(totAmt);

        var totalGst = 0;
        var GSTType = 'G';

        if (cgridTax.cpTotalGST != null) {

            if (cgridTax.cpGSTType != null) {
                GSTType = cgridTax.cpGSTType;
                cgridTax.cpGSTType = null;
            }

            totalGst = parseFloat(cgridTax.cpTotalGST);
            var qty = grid.GetEditor("Quantity").GetValue();
            var price = grid.GetEditor("SalePrice").GetValue();
            var Discount = grid.GetEditor("Discount").GetValue();

            var finalAmt = qty * price;

             

            grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - (finalAmt * (Discount / 100)) - totalGst), 2));
            //}
            cgridTax.cpTotalGST = null;

        }

        var totalNetAmount = DecimalRoundoff((parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue())), 2);
        grid.GetEditor("TotalAmount").SetValue(totalNetAmount);



        var finalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
        var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
        cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
        SetInvoiceLebelValue();

    }

    if (cgridTax.GetVisibleRowsOnPage() == 0) {
        $('.cgridTaxClass').hide();
        ccmbGstCstVat.Focus();
    } 

    SetRunningTotal();
    ShowTaxPopUp("IY");
    RecalCulateTaxTotalAmountInline();
}

recalculateTax = function () {
    cmbGstCstVatChange(ccmbGstCstVat);
}

recalculateTaxCharge = function () {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}


SettingTabStatus = function () {
    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
    }
}

CopyBillingAddresstoShipping = function (obj) {
    cchkShipping.SetChecked(false);
    var chkbill = obj.GetChecked();
    if (chkbill == true) {
        $('#DivShipping').hide();
    }
    else {
        $('#DivShipping').show();
    }
    ctaxUpdatePanel.PerformCallback('DeleteAllTax');
}


CopyShippingAddresstoBilling = function(obj) {
    cchkBilling.SetChecked(false);
    var chkship = obj.GetChecked();
    if (chkship == true) {
        $('#DivBilling').hide();
    }
    else {
        $('#DivBilling').show();
    }
}


btnSave_QuoteAddress = function () {
    checking = true;
    var chkbilling = cchkBilling.GetChecked();
    var chkShipping = cchkShipping.GetChecked();

    if (chkbilling == false && chkShipping == false) {
        // Billing Start
        if (ctxtAddress1.GetText().trim() == '' || ctxtAddress1.GetText() == null) {
            $('#badd1').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#badd1').attr('style', 'display:none'); }


        // pin

        if (CmbPin.GetValue() == '' || CmbPin.GetValue() == null || CmbPin.GetValue() == 'select') {
            $('#bpin').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#bpin').attr('style', 'display:none'); }
        // Billing End

        // Shipping Start

        if (ctxtsAddress1.GetText().trim() == '' || ctxtsAddress1.GetText() == null) {
            $('#sadd1').attr('style', 'display:block');
            checking = false;
            //return false;
        }
        else { $('#sadd1').attr('style', 'display:none'); }


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
        GetObjectID('hdnAddressDtl').value = '1';
        page.SetActiveTabIndex(0);
        //   gridLookup.Focus();
        cddl_SalesAgent.Focus();
        gridquotationLookup.HideDropDown();
        $('.crossBtn').show();
    }
    else {
        page.SetActiveTabIndex(1);
        $('.crossBtn').show();
    }
}


ClosebillingLookup = function () {
    billingLookup.ConfirmCurrentSelection();
    billingLookup.HideDropDown();
    billingLookup.Focus();
}
CloseshippingLookup = function () {
    shippingLookup.ConfirmCurrentSelection();
    shippingLookup.HideDropDown();
    shippingLookup.Focus();
}


openAreaPage = function () {
    var left = (screen.width - 300) / 2;
    var top = (screen.height - 250) / 2;
    var cityid = CmbCity.GetValue();
    var cityname = CmbCity.GetText();
    var URL = '../Master/AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
    popupan.SetContentUrl(URL);
    popupan.Show();
}

openAreaPageShip = function () {
    var left = (screen.width - 300) / 2;
    var top = (screen.height - 250) / 2;
    var cityid = CmbCity1.GetValue();
    var cityname = CmbCity1.GetText();
    var URL = '../Master/AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
    popupan.SetContentUrl(URL);
    popupan.Show();
}


cmbPin_endcallback = function (s, e) {
    if (Billing_pin != 0) {
        s.SetValue(Billing_pin);
    }
    if (Billing_pin != null || Billing_pin != '' || Billing_pin != '0') {
        cchkBilling.SetEnabled(true);
    }
    Billing_pin = 0;
}

cmbshipPin_endcallback = function (s, e) {
    if (Shipping_pin != 0) {
        s.SetValue(Shipping_pin);
    }
    if (Shipping_pin != null || Shipping_pin != '' || Shipping_pin != '0') {
        cchkShipping.SetEnabled(true);
    }
    Shipping_pin = 0;
}

cmbArea_endcallback = function (s, e) {
    if (billing_area != 0) {
        s.SetValue(billing_area);
    }
    billing_area = 0;
}

cmbshipArea_endcallback = function (s, e) {
    if (Shipping_area != 0) {
        s.SetValue(Shipping_area);
    }
    Shipping_area = 0;
}

Panel_endcallback = function () {

    if (cComponentPanel.cpEParameter == "Edit") {
        cchkBilling.SetChecked(false);
        cchkShipping.SetChecked(false);

        $('#DeleteCustomer').val("");
        cContactPerson.PerformCallback('BindContactPerson~' + cCustomerComboBox.GetValue());
        cComponentPanel.cpEParameter = null;
    }
}

AddcustomerClick = function () {
    var url = '/OMS/management/Master/Customer_general.aspx';
    AspxDirectAddCustPopup.SetContentUrl(url);
    AspxDirectAddCustPopup.Show();
}

disp_prompt = function (name) {
    if (name == "tab1") {
        var custID = GetObjectID('hdnCustomerId').value;
        if (custID == null && custID == '') {
            jAlert('Please select a customer');
            page.SetActiveTabIndex(0);
            return;
        }
        else {
            page.SetActiveTabIndex(1);
            cComponentPanel.PerformCallback('Edit~1');
        }
    }
}




setViewMode = function () {
    if ($('#HdViewmode').val() == 'Yes') {
        $('#divSubmitButton').hide();
    }
}

setBannerView = function (type) {
    if (type == 'Cash') {
        $('.clsbnrLblLessAdvance').hide();
    }
}

setPosView = function (type) {
    if (type == 'Cash') {
        $('#FinancerTable').hide();
    }
    else if (type == 'Crd') {
        $('#FinancerTable').hide();
    }
    else if (type == 'Fin') {

    }
}

GetBillingAddressDetailByAddressId = function (e) {
    var addresskey = billingLookup.GetGridView().GetRowKey(billingLookup.GetGridView().GetFocusedRowIndex());
    if (addresskey != null && addresskey != '') {

        cComponentPanel.PerformCallback('BlookupEdit~' + addresskey);
    }
}

GetShippingAddressDetailByAddressId = function (e) {

    var saddresskey = shippingLookup.GetGridView().GetRowKey(shippingLookup.GetGridView().GetFocusedRowIndex());
    if (saddresskey != null && saddresskey != '') {

        cComponentPanel.PerformCallback('SlookupEdit~' + saddresskey);
    }
}

UniqueCodeCheck = function () {

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


GetContactPerson = function (e) {

    if (!cCustomerComboBox.FindItemByValue(cCustomerComboBox.GetValue())) {
        jAlert("Customer not Exists.", "Alert", function () { cCustomerComboBox.SetValue(); cCustomerComboBox.Focus(); });
        return;
    }


    if (gridquotationLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

            if (r == true) {
                var key = cCustomerComboBox.GetValue();
                if (key != null && key != '') {
                    cchkBilling.SetChecked(false);
                    cchkShipping.SetChecked(false);
                    //cContactPerson.PerformCallback('BindContactPerson~' + key);
                    page.GetTabByName('[B]illing/Shipping').SetEnabled(true);


                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    var startDate = new Date();
                    startDate = tstartdate.GetValueString();

                    if (type != "") {
                        cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%' + '~' + type);
                    }

                    var componentType = gridquotationLookup.GetValue();//gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
                    if (componentType != null && componentType != '') {
                        grid.PerformCallback('GridBlank');
                        document.getElementById('hdnPageStatus').value = 'update'; 
                    }

                    GetObjectID('hdnCustomerId').value = key;
                    GetObjectID('hdnAddressDtl').value = '0';

                    page.SetActiveTabIndex(1);
                    $('.dxeErrorCellSys').addClass('abc');
                    $('.crossBtn').hide();
                    page.GetTabByName('General').SetEnabled(false);


                }
            }
        });
    }
    else {
        var key = cCustomerComboBox.GetValue();
        if (key != null && key != '') {
            cchkBilling.SetChecked(false);
            cchkShipping.SetChecked(false);
            page.GetTabByName('[B]illing/Shipping').SetEnabled(true);



            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
            var startDate = new Date();
            startDate = tstartdate.GetValueString();

            if (type != "") {
                cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%' + '~' + type);
            }

            var componentType = gridquotationLookup.GetValue();//gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
            if (componentType != null && componentType != '') {
                grid.PerformCallback('GridBlank');
                document.getElementById('hdnPageStatus').value = 'update'; 
            }

            GetObjectID('hdnCustomerId').value = key;
            GetObjectID('hdnAddressDtl').value = '0';

            page.SetActiveTabIndex(1);
            $('.dxeErrorCellSys').addClass('abc');
            $('.crossBtn').hide();
            page.GetTabByName('General').SetEnabled(false);
        }
    }

}














$(document).ready(function () {



    
    cProductsPopup.Hide();
    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
    }
    



    $('#ddl_VatGstCst_I').blur(function () {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    });
    $('#ddl_AmountAre').blur(function () {
        var id = cddl_AmountAre.GetValue();
        if (id == '1' || id == '3') {
            if (grid.GetVisibleRowsOnPage() == 1) {
                grid.batchEditApi.StartEdit(-1, 2);
            }
        }
    });


    setPosView(document.getElementById('HdPosType').value);
    setBannerView($('#HdPosType').val());
    setViewMode();
});




$(document).ready(function () {
    var schemaid = $('#ddl_numberingScheme').val();

    if (schemaid != null) {
        if (schemaid == '') {
            ctxt_PLQuoteNo.SetEnabled(false);
        }
    }
    $('#ddl_numberingScheme').change(function () {

        if ($('#ddl_numberingScheme').val() == "") {
            return;
        }

        var NoSchemeTypedtl = $(this).val();
        var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
        var quotelength = NoSchemeTypedtl.toString().split('~')[2];


        if ($('#ddl_numberingScheme').val().split('~')[1] == "0") {
            tstartdate.SetEnabled(true);
        } else {
            tstartdate.SetEnabled(false);
            tstartdate.SetDate(new Date);
            cdeliveryDate.SetDate(tstartdate.GetDate());
        }

        var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";

        if (document.getElementById('HdPosType').value == "Fin") {
            if (ccmbFinancer.InCallback())
                lastFinancer = branchID;
            else
                ccmbFinancer.PerformCallback(branchID);
        }

        if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
        // if (document.getElementById('HdPosType').value != 'Crd') {
        $('#HdSelectedBranch').val(branchID);
        // }

        if ($('#hdBasketId').val() != "0") {
            document.getElementById('hdnPageStatus').value = 'Rebindbasketgrid';
            grid.PerformCallback('rebindgridFromBasket');
        }

        if (cddl_SalesAgent.InCallback())
            lastSalesman = branchID;
        else
            cddl_SalesAgent.PerformCallback(branchID);



        if (cchallanNoScheme.InCallback())
            lastChallan = 'BindChallanScheme~' + NoSchemeTypedtl.toString().split('~')[3];
        else
            cchallanNoScheme.PerformCallback('BindChallanScheme~' + NoSchemeTypedtl.toString().split('~')[3])


        //ctxt_PLQuoteNo.SetMaxLength(quotelength);
        if (NoSchemeType == '1') {
            ctxt_PLQuoteNo.SetText('Auto');
            ctxt_PLQuoteNo.SetEnabled(false);
            //ctxt_PLQuoteNo.SetClientEnabled(false);

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
    });


});


SetFocusonDemand = function (e) {
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

PopulateGSTCSTVAT = function(e) {
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


Startdate = function (s, e) {
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
    ctaxUpdatePanel.PerformCallback('DeleteAllTax');
    if (IsProduct == "Y") {
        document.getElementById('hdfIsDelete').value = 'D';
        document.getElementById('HdUpdateMainGrid').value = 'True'; 
        grid.UpdateEdit();
    }

    if (t == "")
    { $('#MandatorysDate').attr('style', 'display:block'); }
    else { $('#MandatorysDate').attr('style', 'display:none'); }
}



Enddate = function (s, e) {

    var t = s.GetDate();
    if (t == "")
    { $('#MandatoryEDate').attr('style', 'display:block'); }
    else { $('#MandatoryEDate').attr('style', 'display:none'); }



    var sdate = tstartdate.GetValue();

}


ShowCustom = function () {
    cPopup_wareHouse.Show();
}

GridCallBack = function () {
    grid.PerformCallback('Display');
    canCallBack = true;
}


AllControlInitilize = function () {
    if (canCallBack) {
        grid.PerformCallback();

        if ($('#isBasketContainComponent').val() == 'yes') {
            jAlert("You have selected Products for which Component exist. Components are shown for respective products. Enter Quantity and Values as applicable.", "Alert", function () { $('#ddl_numberingScheme').focus(); });
        }

        //document.getElementById('HdPosType').value != 'Crd' &&
        if (document.getElementById('HdPosType').value != 'IST') {
            cmbUcpaymentCashLedgerChanged(ccmbUcpaymentCashLedger);
            $('#HdSelectedBranch').val(document.getElementById('ddl_Branch').value);
        } else {
            $('#idCashbalanace').hide();
        }



        if ($('#hdBasketId').val() != "0") {
            SetInvoiceLebelValue();
        }



        if (document.getElementById('hdAddOrEdit').value == "Edit") {
            isExecutiveHasLedger = 1;
            $('#customerReceiptButtonSet').hide();
            if (ccmbOldUnit.GetValue() == "1") {
                $('#OldUnitSelectionButton').show();
            } else {
                $('#OldUnitSelectionButton').hide();
            }
            ctxt_PLQuoteNo.SetEnabled(true);
            SetInvoiceLebelValue();
        } else {
            $('#otherChargesId').hide();
            $('#hdHsnList').val(',');
        }

        if (document.getElementById('HdPosType').value == 'Fin') {
            $('#HeaderTextforPaymentDetails')[0].innerText = 'Down Payment Details';
        }
        canCallBack = false;
    }
}


ReBindGrid_Currency = function () {
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
        document.getElementById('hdfIsDelete').value = 'D'; 
        grid.UpdateEdit();
        grid.PerformCallback('CurrencyChangeDisplay');
    }
}


cmbContactPersonEndCall = function (s, e) {
    if (cContactPerson.cpDueDate != null) {
        var DeuDate = cContactPerson.cpDueDate;
        var myDate = new Date(DeuDate);

        cdt_SaleInvoiceDue.SetDate(myDate);
        cContactPerson.cpDueDate = null;
    }

    if (cContactPerson.cpTotalDue != null) {
        var TotalDue = cContactPerson.cpTotalDue;
        var TotalCustDue = "";
        if (TotalDue >= 0) {
            TotalCustDue = TotalDue + ' Cr';
            document.getElementById('lblTotalDues').style.color = "red";
        }
        else {
            TotalDue = TotalDue * (-1);
            TotalCustDue = TotalDue + ' Db';
            document.getElementById('lblTotalDues').style.color = "black";
        }

        document.getElementById('lblTotalDues').innerHTML = TotalCustDue;
        pageheaderContent.style.display = "block";
        divDues.style.display = "block";
        cContactPerson.cpTotalDue = null;
    }

}




OnEndCallback = function (s, e) {
    LoadingPanel.Hide();
    var value = document.getElementById('hdnRefreshType').value;
    //Debjyoti Check grid needs to be refreshed or not

    if (grid.cpTaggingTotalAmount) {
        if (grid.cpTaggingTotalAmount != '') {
            var returnTaggingAmount = parseFloat(grid.cpTaggingTotalAmount);
            cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(returnTaggingAmount) * 100) / 100).toFixed(2));
            SetInvoiceLebelValue();
            grid.cpTaggingTotalAmount = null;
        }
    }


    if (document.getElementById('HdUpdateMainGrid').value == 'True') {
        document.getElementById('HdUpdateMainGrid').value = 'False';
        grid.PerformCallback('DateChangeDisplay');
    }

    if (grid.cpComponent) {
        if (grid.cpComponent == 'true') {
            grid.cpComponent = null;
            OnAddNewClick();
        }
    }

    if (grid.cpSaveSuccessOrFail == "outrange") {
        jAlert('Can Not Add More Invoice (POS) Number as Invoice (POS) Scheme Exausted.<br />Update The Scheme and Try Again');
        // OnAddNewClick();
        grid.StartEditRow(0);
        grid.cpSaveSuccessOrFail = '';
    }
    else if (grid.cpSaveSuccessOrFail == "duplicate") {
        //  OnAddNewClick();
        grid.StartEditRow(0);
        jAlert('Can Not Save as Duplicate Invoice (POS) Numbe No. Found');
        grid.cpSaveSuccessOrFail = '';
    }
    else if (grid.cpSaveSuccessOrFail == "quantityTagged") {
        //OnAddNewClick();
        grid.StartEditRow(0);
        jAlert('Proforma is tagged in Sale Order. So, Quantity of selected products cannot be less than Ordered Quantity.');
        grid.cpSaveSuccessOrFail = '';
    }
    else if (grid.cpSaveSuccessOrFail == "errorInsert") {
        //OnAddNewClick();
        grid.StartEditRow(0);
        jAlert('Please try again later.');
        grid.cpSaveSuccessOrFail = '';
    }
    else if (grid.cpSaveSuccessOrFail == "nullAmount") {
        //  OnAddNewClick();
        grid.StartEditRow(0);
        jAlert('total amount cant not be zero(0).');
        grid.cpSaveSuccessOrFail = '';
    }
    else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
        //OnAddNewClick();
        grid.StartEditRow(0);
        jAlert('Please fill Quantity');
        grid.cpSaveSuccessOrFail = '';
    }
    else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
        //  OnAddNewClick();
        grid.StartEditRow(0);
        jAlert('Can not save Duplicate Product in the Invoice (POS) List.');
        grid.cpSaveSuccessOrFail = '';
    }

    else if (grid.cpSaveSuccessOrFail == "MoreThanStock") {
        grid.StartEditRow(0);
        jAlert('Product entered quantity more than stock quantity.Can not proceed.');
        grid.cpSaveSuccessOrFail = '';
    }

    else if (grid.cpSaveSuccessOrFail == "BillingSHippingRequired") {
        grid.StartEditRow(0);
        jAlert('Please Re-Check the Billing/Shipping Details.', "Alert", function () { page.SetActiveTabIndex(1); });
        grid.cpSaveSuccessOrFail = '';
    }

    else if (grid.cpSaveSuccessOrFail == "minSalePriceMust") {
        //OnAddNewClick();
        grid.StartEditRow(0);
        jAlert('Sale Price Should be equal or higher than Min Sale Price');
        grid.cpSaveSuccessOrFail = '';
    }
    else if (grid.cpSaveSuccessOrFail == "MRPLess") {
        //OnAddNewClick();
        grid.StartEditRow(0);
        jAlert('Sale Price Should be equal or less than MRP');
        grid.cpSaveSuccessOrFail = '';
    }
    else if (grid.cpSaveSuccessOrFail == "InValidReceipt") {
        grid.StartEditRow(0);
        jAlert('Mismatched found of HSN for the selected Product(s) and Advance(s). Correct and proceed.');
        grid.cpSaveSuccessOrFail = '';
    }

    else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
        var SrlNo = grid.cpProductSrlIDCheck;
        //OnAddNewClick();
        grid.StartEditRow(0);
        //   var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
        var msg = 'You must enter Stock details for type "Already Delivered".';
        jAlert(msg);
        grid.cpSaveSuccessOrFail = '';
    }
    else {
        var Quote_Number = grid.cpQuotationNo;
        grid.cpQuotationNo = null;
        var Quote_Msg = "Invoice No. '" + Quote_Number + "' saved.";

        if (value == "E") {
            if (grid.cpApproverStatus == "approve") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else if (grid.cpApproverStatus == "rejected") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else {
                if (Quote_Number != "") {
                    if (grid.cpGeneratedInvoice) {
                        var newInvoiceId = grid.cpGeneratedInvoice;
                        var reportName = "";
                        if (document.getElementById('HdPosType').value == "Cash") {
                            reportName = "POS-Cash~D";
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                        } else if (document.getElementById('HdPosType').value == "Crd") {
                            reportName = "POS-Credit~D";
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                        } else if (document.getElementById('HdPosType').value == "Fin") {
                            reportName = "POS-Finance~D";
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=3', '_blank');

                        } else if (document.getElementById('HdPosType').value == "IST") {
                            reportName = "InterstateStockTransfer-GST~D";
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                        }




                        if (grid.cpIsInstallRequired) {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=IC-Default~D&modulename=Install_Coupon&id=" + newInvoiceId, '_blank');
                        }

                        window.location.assign("PosSalesInvoiceList.aspx");
                    }
                }
                else {
                    window.location.assign("PosSalesInvoiceList.aspx");
                }
            }

        }
        else if (value == "N") {
            if (grid.cpApproverStatus == "approve") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else {
                if (Quote_Number != "") {

                    var newInvoiceId = grid.cpGeneratedInvoice;

                    var reportName = "";
                    if (document.getElementById('HdPosType').value == "Cash") {
                        reportName = "POS-Cash~D";
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                    } else if (document.getElementById('HdPosType').value == "Crd") {
                        reportName = "POS-Credit~D";
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');

                    } else if (document.getElementById('HdPosType').value == "Fin") {
                        reportName = "POS-Finance~D";
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=3', '_blank');

                    } else if (document.getElementById('HdPosType').value == "IST") {
                        reportName = "InterstateStockTransfer-GST~D";
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                    }




                    if (grid.cpIsInstallRequired) {
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=IC-Default~D&modulename=Install_Coupon&id=" + newInvoiceId, '_blank');
                        grid.cpIsInstallRequired = null;
                    }

                    window.location.reload();
                }
                else {
                    window.location.assign("PosSalesInvoice.aspx?key=ADD");
                }
            }
        }
        else {
            var pageStatus = document.getElementById('hdnPageStatus').value;
            if (pageStatus == "first") {
                OnAddNewClick();
                grid.batchEditApi.EndEdit();
                // it has been commented by sam on 04032017 due to set focus from server side start
                //document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();
                // above part has been commented by sam on 04032017 due to set focus from server side start
                document.getElementById('hdnPageStatus').value = ''; 

                var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                var basedCurrency = LocalCurrency.split("~");
                if ($("#ddl_Currency").val() == basedCurrency[0]) {
                    ctxt_Rate.SetEnabled(false);
                }
            }
            else if (pageStatus == "update") {
                OnAddNewClick();
                document.getElementById('hdnPageStatus').value = ''; 
                //document.getElementById("ddlInventory").disabled = true;

                var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                var basedCurrency = LocalCurrency.split("~");
                if ($("#ddl_Currency").val() == basedCurrency[0]) {
                    ctxt_Rate.SetEnabled(false);
                }
            }
            else if (pageStatus == "Quoteupdate") {
                grid.StartEditRow(0);
                document.getElementById('hdnPageStatus').value = ''; 
            }
            else if (pageStatus == "delete") {
                grid.StartEditRow(0);
                document.getElementById('hdnPageStatus').value = ''; 
            }
            else if (pageStatus == "Rebindbasketgrid") {
                grid.StartEditRow(0);
                document.getElementById('hdnPageStatus').value = ''; 
            }
            else {
                grid.StartEditRow(0);
            }
        }


      

    }

    if (gridquotationLookup.GetValue() != null) {
        grid.GetEditor('ComponentNumber').SetEnabled(false);
        grid.GetEditor('ProductName').SetEnabled(false);
        grid.GetEditor('Description').SetEnabled(false);
    }
    cProductsPopup.Hide();
}




Save_ButtonClick = function () {
    LoadingPanel.Show();



    flag = true;
    grid.batchEditApi.EndEdit();

    if (document.getElementById('PaymentTable')) {
        var table = document.getElementById('PaymentTable');
        if (table.rows[table.rows.length - 1].children[0].children[1].value != "-Select-") {
            flag = validatePaymentDetails(table.rows[table.rows.length - 1]);
        }

    }

    if (parseFloat(ctxtunitValue.GetValue()) != 0 && cOldUnitGrid.GetVisibleRowsOnPage() == 0) {
        jAlert("Selected data is having Old Unit value as " + parseFloat(Math.round(Math.abs(parseFloat($('#HdDiscountAmount').val())) * 100) / 100).toFixed(2) + ". Please select 'Yes' in Old Unit to enter product details and proceed.", "Alert", function () { ccmbOldUnit.Focus(); });
        flag = false;
        LoadingPanel.Hide();
    }

    if (flag) {
        if ($('#hdBasketId').val() != "0") {
            var receivedDisAmtByTab = parseFloat($('#HdDiscountAmount').val());
            var enteredDiscountAmt = parseFloat(ctxtunitValue.GetValue());
            if (receivedDisAmtByTab != enteredDiscountAmt) {
                flag = false;
                LoadingPanel.Hide();
                jAlert("Selected data is having Old Unit value as " + parseFloat(Math.round(Math.abs(receivedDisAmtByTab) * 100) / 100).toFixed(2) + ". Please select 'Yes' in Old Unit to enter product details and proceed.", "Alert", function () { ccmbOldUnit.Focus(); });
            }
        }
    }
    if (flag) {
        flag = isEnteredAmountValid();
    }

    if (flag) {
        if (document.getElementById('HdPosType').value != 'Crd' && document.getElementById('HdPosType').value != 'Fin') {
            var EnteredCashAmount = parseFloat($('#cmbUcpaymentCashLedgerAmt').val());
            if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                flag = false;
                LoadingPanel.Hide();
            }
        }
    }

    //Delivery Date Checking
    if (cdeliveryDate.GetDate() == null) {
        $('#MandatorysdeliveryDate').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    } else if (cdeliveryDate.GetDate() < tstartdate.GetDate()) {
        $('#MandatorysdeliveryDate').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysdeliveryDate').attr('style', 'display:none');
    }

    // Quote no validation Start
    var QuoteNo = ctxt_PLQuoteNo.GetText();
    if (QuoteNo == '' || QuoteNo == null) {
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }
    // Quote no validation End
    if (ccmbDeliveryType.GetValue() == "0") {
        $('#mandetorydeliveryType').show();
        flag = false;
        LoadingPanel.Hide();
    } else {
        $('#mandetorydeliveryType').hide();
    }

    if (ccmbDeliveryType.GetValue() == 'D') {
        if (cchallanNoScheme.GetValue() == null) {
            $('#mandetorydchallanNoScheme').attr('style', 'display:block');
            flag = false;
            LoadingPanel.Hide();
        } else {
            $('#mandetorydchallanNoScheme').attr('style', 'display:none');
            if (ctxtChallanNo.GetText().trim() == '') {
                $('#mandetorydtxtChallanNo').attr('style', 'display:block');
                flag = false;
                LoadingPanel.Hide();
            } else {
                $('#mandetorydtxtChallanNo').attr('style', 'display:none');
            }
        }
    }

    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (sdate == null || sdate == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorysDate').attr('style', 'display:block');
    }
    else { $('#MandatorysDate').attr('style', 'display:none'); }
    if (sdate == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatoryEDate').attr('style', 'display:block');
    }
    else {
        $('#MandatoryEDate').attr('style', 'display:none');

    }
    // Quote Date validation End

    // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {

        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }

    if (cCustomerComboBox.GetValue() == '' || cCustomerComboBox.GetValue() == null) {

        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }


    // Quote Customer validation End
    //var amtare = cddl_AmountAre.GetValue();
    //if (amtare == '2') {
    //    var taxcodeid = cddlVatGstCst.GetValue();
    //    if (taxcodeid == '' || taxcodeid == null) {
    //        $('#Mandatorytaxcode').attr('style', 'display:block');
    //        flag = false;
    //    }
    //    else {
    //        $('#Mandatorytaxcode').attr('style', 'display:none');
    //    }
    //}

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
            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : ""; 
            document.getElementById('hdfLookupCustomer').value = customerval;
            
            document.getElementById('hdnRefreshType').value = 'N';
            
            document.getElementById('hdfIsDelete').value = 'I';
            grid.batchEditApi.EndEdit();

            //   if (document.getElementById('HdPosType').value != 'Crd' ) {
            SelectAllData(gridUpdateEdit);
            //} else {
            //    gridUpdateEdit();
            //}


        }
        else {
            jAlert('Please add atleast single record first');
            LoadingPanel.Hide();
        }
    } else {
        LoadingPanel.Hide();
    }
}




isEnteredAmountValid = function () {
    var returnValue = true;
    var enteredAmount = 0;
    var otherCharges = parseFloat(cbnrOtherChargesvalue.GetValue());
    if (document.getElementById('HdPosType').value != 'IST') {
        enteredAmount = parseFloat(GetPaymentTotalEnteredAmount());
    }

    //- parseFloat(ctxtprocFee.GetValue()) - parseFloat(ctxtEmiOtherCharges.GetValue())
    var unPaidAmount = parseFloat(ctxtunitValue.GetValue()) + parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(ctxtFinanceAmt.GetValue()) + enteredAmount - otherCharges;
    // if (document.getElementById('HdPosType').value != 'Fin') {



    if (document.getElementById('HdPosType').value != 'Crd' && document.getElementById('HdPosType').value != 'Fin' && document.getElementById('HdPosType').value != 'IST') {

        if (parseFloat(cbnrlblAmountWithTaxValue.GetValue()) != unPaidAmount) {
            jAlert("Mismatch detected in between Invoice Amount and Payment Amount. Cannot proceed.", "Alert", function () {
                $('#cmbUcpaymentCashLedgerAmt').focus();
            });
            returnValue = false;
        }
    }
    else if (document.getElementById('HdPosType').value == 'Fin') {
        var runningBal = parseFloat(clblRunningBalanceCapsul.GetValue());
        if (runningBal != 0) {
            jAlert("Mismatch detected in between Invoice Amount and Payment Amount. Cannot proceed.", "Alert", function () {

            });
            returnValue = false;
        }
    }
    

    return returnValue;
}







SaveExit_ButtonClick = function (s, e) {
    LoadingPanel.Show();

    flag = true;
    grid.batchEditApi.EndEdit();

    if (document.getElementById('PaymentTable')) {
        if (document.getElementById('hdAddOrEdit').value == "Add") {
            var table = document.getElementById('PaymentTable');
            if (table.rows[table.rows.length - 1].children[0].children[1].value != "-Select-") {
                flag = validatePaymentDetails(table.rows[table.rows.length - 1]);
            }
        }
    }

    if (parseFloat(ctxtunitValue.GetValue()) != 0 && cOldUnitGrid.GetVisibleRowsOnPage() == 0) {
        jAlert("Selected data is having Old Unit value as " + parseFloat(Math.round(Math.abs(parseFloat($('#HdDiscountAmount').val())) * 100) / 100).toFixed(2) + ". Please select 'Yes' in Old Unit to enter product details and proceed.", "Alert", function () { ccmbOldUnit.Focus(); });
        flag = false;
        LoadingPanel.Hide();
    }


    if (flag) {
        if ($('#hdBasketId').val() != "0") {
            var receivedDisAmtByTab = parseFloat($('#HdDiscountAmount').val());
            var enteredDiscountAmt = parseFloat(ctxtunitValue.GetValue());
            if (receivedDisAmtByTab != enteredDiscountAmt) {
                flag = false;
                jAlert("Selected data is having Old Unit value as " + parseFloat(Math.round(Math.abs(receivedDisAmtByTab) * 100) / 100).toFixed(2) + ". Please select 'Yes' in Old Unit to enter product details and proceed.", "Alert", function () { ccmbOldUnit.Focus(); });
                LoadingPanel.Hide();
            }
        }
    }
    if (flag) {
        flag = isEnteredAmountValid();
    }

    if (flag) {
        if (document.getElementById('HdPosType').value != 'Crd' && document.getElementById('HdPosType').value != 'Fin') {
            var EnteredCashAmount = parseFloat($('#cmbUcpaymentCashLedgerAmt').val());
            if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                flag = false;
                LoadingPanel.Hide();
            }
        }
    }
    //Delivery Date Checking
    if (cdeliveryDate.GetDate() == null) {
        $('#MandatorysdeliveryDate').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    } else if (cdeliveryDate.GetDate() < tstartdate.GetDate()) {
        $('#MandatorysdeliveryDate').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysdeliveryDate').attr('style', 'display:none');
    }


    // Quote no validation Start
    var QuoteNo = ctxt_PLQuoteNo.GetText();
    if (QuoteNo == '' || QuoteNo == null) {
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }
    // Quote no validation End
    if (ccmbDeliveryType.GetValue() == "0") {
        $('#mandetorydeliveryType').show();
        flag = false;
        LoadingPanel.Hide();
    } else {
        $('#mandetorydeliveryType').hide();
    }

    if (ccmbDeliveryType.GetValue() == 'D') {
        if (cchallanNoScheme.GetValue() == null) {
            $('#mandetorydchallanNoScheme').attr('style', 'display:block');
            flag = false;
            LoadingPanel.Hide();
        } else {
            $('#mandetorydchallanNoScheme').attr('style', 'display:none');
            if (ctxtChallanNo.GetText().trim() == '') {
                $('#mandetorydtxtChallanNo').attr('style', 'display:block');
                flag = false;
                LoadingPanel.Hide();
            } else {
                $('#mandetorydtxtChallanNo').attr('style', 'display:none');
            }
        }
    }


    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (sdate == null || sdate == "") {
        flag = false;
        $('#MandatorysDate').attr('style', 'display:block');
    }
    else { $('#MandatorysDate').attr('style', 'display:none'); }
    if (sdate == "") {
        flag = false;
        $('#MandatoryEDate').attr('style', 'display:block');
    }
    else {
        $('#MandatoryEDate').attr('style', 'display:none');

    }

    if (flag) {
        if (document.getElementById('HdPosType').value == 'Fin') {
            if (isExecutiveHasLedger == 0) {
                jAlert("No ledger is mapped for the selected Financer.", "Alert", function () {
                    ccmbFinancer.Focus();
                });
                flag = false;
                LoadingPanel.Hide();
            }


        }
    }
    // Quote Date validation End

    // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }

    if (cCustomerComboBox.GetValue() == '' || cCustomerComboBox.GetValue() == null) {

        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
        LoadingPanel.Hide();
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
    // Quote Customer validation End

    //var amtare = cddl_AmountAre.GetValue();
    //if (amtare == '2') {
    //    var taxcodeid = cddlVatGstCst.GetValue();
    //    if (taxcodeid == '' || taxcodeid == null) {
    //        $('#Mandatorytaxcode').attr('style', 'display:block');
    //        flag = false;
    //    }
    //    else {
    //        $('#Mandatorytaxcode').attr('style', 'display:none');
    //    }
    //}

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
            //divSubmitButton.style.display = "none";
            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";
            document.getElementById('hdfLookupCustomer').value = customerval;
            document.getElementById('hdnRefreshType').value = 'E';
            document.getElementById('hdfIsDelete').value = 'I';
            grid.batchEditApi.EndEdit();



            if (document.getElementById('HdPosType').value != 'IST') {

                SelectAllData(gridUpdateEdit);

            } else {
                gridUpdateEdit();
            }

            // grid.UpdateEdit();
        }
        else {
            jAlert('Please add atleast single record first');
            LoadingPanel.Hide();
        }
    } else {
        LoadingPanel.Hide();
        if (document.getElementById('hdAddOrEdit').value != "Add") {
            e.processOnServer = false;
        }
    }

}


gridUpdateEdit = function () {

    if (document.getElementById('hdAddOrEdit').value != "Edit") {
        OnAddNewClick();
        grid.UpdateEdit();
    }

    // grid.PerformCallback('UpdateExistingData');
}


QuantityTextChange = function (s, e) {
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var key = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

    if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)) {
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

                if (CurrQty < 0) {
                    grid.GetEditor("TotalQty").SetValue(TotalQty);
                    grid.GetEditor("Quantity").SetValue(TotalQty);
                    var OrdeMsg = 'Balance Quantity of selected Product from tagged document is (' + ((QuantityValue - TotalQty) + BalanceQty) + '). <br/>Cannot enter quantity more than balance quantity.';
                    grid.batchEditApi.EndEdit();
                    jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {
                        grid.batchEditApi.StartEdit(globalRowIndex, 6);
                    });
                    return false;
                }
                else {
                    grid.GetEditor("TotalQty").SetValue(QuantityValue);
                    grid.GetEditor("BalanceQty").SetValue(CurrQty);
                }
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

            $('#<%= lblStkQty.ClientID %>').text(StockQuantity);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            //var tbStockQuantity = grid.GetEditor("StockQuantity");
            //tbStockQuantity.SetValue(StockQuantity);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount);

            DiscountTextChange(s, e);



            //  cacpAvailableStock.PerformCallback(strProductID);
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Quantity').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}



SalePriceTextChange = function (s, e) {
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        console.log(SpliteDetails);

        if (parseFloat(s.GetValue()) < parseFloat(SpliteDetails[17])) {
            jAlert("Sale price cannot be lesser than Min Sale Price locked as: " + parseFloat(Math.round(Math.abs(parseFloat(SpliteDetails[17])) * 100) / 100).toFixed(2), "Alert", function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 10);
                return;
            });
            s.SetValue(parseFloat(SpliteDetails[6]));
            return;
        }



        if ($('#hdBasketId').val() == "0") {
            if (parseFloat(SpliteDetails[18]) != 0 && parseFloat(s.GetValue()) > parseFloat(SpliteDetails[18])) {
                jAlert("Sale price cannot be greater than MRP locked as: " + parseFloat(Math.round(Math.abs(parseFloat(SpliteDetails[18])) * 100) / 100).toFixed(2), "Alert", function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 10);
                    return;
                });
                s.SetValue(parseFloat(SpliteDetails[6]));
                return;
            }
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
        var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(amountAfterDiscount);

        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(amountAfterDiscount);

        //GetShipping State Value
        var ShippingStateCode = '';
        if (cchkBilling.GetValue()) {
            ShippingStateCode = $('#lblBillingStateValue').val();//CmbState.GetValue();
        } else {
            ShippingStateCode = $('#lblShippingStateValue').val();//CmbState1.GetValue();
        }

        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], Amount, amountAfterDiscount, 'I', ShippingStateCode, $('#ddl_Branch').val())

        var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
        var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
        cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
        cbnrLblTaxableAmtval.SetText(grid.GetEditor("Amount").GetText());
        cbnrLblTaxAmtval.SetText(grid.GetEditor("TaxAmount").GetText());

        SetInvoiceLebelValue();

        $('#<%= lblProduct.ClientID %>').text(strProductName);
        $('#<%= lblbranchName.ClientID %>').text(strBranch);

        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
            divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }




        cacpAvailableStock.PerformCallback(strProductID);
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('SalePrice').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
}




DiscountTextChange = function (s, e) {
    //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
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

        var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(amountAfterDiscount);

        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
            divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }

        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(amountAfterDiscount);


        var ShippingStateCode = '';
        if (cchkBilling.GetValue()) {
            ShippingStateCode = $('#lblBillingStateValue').val();// CmbState.GetValue();
        } else {
            ShippingStateCode = $('#lblShippingStateValue').val();//CmbState1.GetValue();
        }
        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], Amount, amountAfterDiscount, 'I', ShippingStateCode, $('#ddl_Branch').val());


        var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
        var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
        cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
        cbnrLblTaxableAmtval.SetText(grid.GetEditor("Amount").GetText());
        cbnrLblTaxAmtval.SetText(grid.GetEditor("TaxAmount").GetText());
        SetInvoiceLebelValue();
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    //Debjyoti 
    //  grid.GetEditor('TaxAmount').SetValue(0);

    var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
        ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
    }
}




AddBatchNew = function (s, e) {
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



OnAddNewClick = function (callback) {
    if (gridquotationLookup.GetValue() == null) {
        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
    }
    else {
        QuotationNumberChanged();
    }
     

}

Save_TaxClick = function () {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cPopup_Taxes.Hide();
}





OnCustomButtonClick = function (s, e) {

    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.StartEdit(e.visibleIndex);
        var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
        var totalNetAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'TotalAmount');

        grid.batchEditApi.EndEdit();

        $('#<%=hdnRefreshType.ClientID %>').val('');
        $('#<%=hdnDeleteSrlNo.ClientID %>').val(SrlNo);
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

                var newTotalNetAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(totalNetAmount);
                cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(newTotalNetAmount) * 100) / 100).toFixed(2));
                SetInvoiceLebelValue();

                var prodIDForHsn = grid.batchEditApi.GetCellValue(e.visibleIndex, 'ProductID').split("||@||");
                if (prodIDForHsn.length > 19) {
                    var HSNSac = prodIDForHsn[19];
                    RemoveHSnSacFromList(HSNSac);
                }


                grid.DeleteRow(e.visibleIndex);

                $('#<%=hdfIsDelete.ClientID %>').val('D');
                grid.UpdateEdit();
                grid.PerformCallback('Display');

                $('#<%=hdnPageStatus.ClientID %>').val('delete');
                //grid.batchEditApi.StartEdit(-1, 2);
                //grid.batchEditApi.StartEdit(0, 2);
            }
        }
    }
    else if (e.buttonID == 'AddNew') {
        var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        if (ProductIDValue != "") {
            var SpliteDetails = ProductIDValue.split("||@||");
            var IsComponentProduct = SpliteDetails[15];
            var ComponentProduct = SpliteDetails[16];

            if (IsComponentProduct == "Y") {
                var messege = "Selected product is defined with components.<br/> Would you like to proceed with components?";
                jConfirm(messege, 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                        grid.GetEditor("IsComponentProduct").SetValue("Y");
                        $('#<%=hdfIsDelete.ClientID %>').val('C');

                        grid.UpdateEdit();
                        grid.PerformCallback('Display~fromComponent');
                        //grid.batchEditApi.StartEdit(globalRowIndex, 3);
                    }
                    else {
                        OnAddNewClick();
                    }
                });
                document.getElementById('popup_ok').focus();
            }
            else {
                OnAddNewClick();
            }
        }
        else {
            grid.batchEditApi.StartEdit(e.visibleIndex, 2);
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {

        var index = e.visibleIndex;

        if (ccmbDeliveryType.GetValue() != "D") {
            jAlert("Only Applicable for delivery type 'Already Delivered'.");
            return;
        }

        grid.batchEditApi.StartEdit(index, 2)
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
            document.getElementById('hdfProductType').value = Ptype;
            

            document.getElementById('lblProductName').innerHTML = strProductName;
            document.getElementById('txt_SalesAmount').innerHTML = QuantityValue;
            document.getElementById('txt_SalesUOM').innerHTML = strUOM;
            document.getElementById('txt_StockAmount').innerHTML = StkQuantityValue;
            document.getElementById('txt_StockUOM').innerHTML = strStkUOM;


            document.getElementById('hdfProductID').value = strProductID; 
            document.getElementById('hdfProductSerialID').value = SrlNo; 
            document.getElementById('hdfProductSerialID').value = SrlNo;
            document.getElementById('hdnProductQuantity').value = QuantityValue;
          
            cacpAvailableStock.PerformCallback(strProductID);

            if (Ptype == "W") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);

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

                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else {
               
                jAlert("Please enter Quantity !");
                
            }
        }
        else if (ProductID != "" && parseFloat(QuantityValue) == 0) {
            jAlert('Please enter Quantity.');
        }
    }
}


