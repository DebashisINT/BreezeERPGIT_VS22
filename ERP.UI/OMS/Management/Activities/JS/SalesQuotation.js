/*********************************************************************************************************
 * Rev 1.0      Sanchita      V2.0.38                   Base Rate is not recalculated when the Multi UOM is Changed. Mantis : 26320, 26357, 26361   
 * Rev 2.0      Sanchita      V2.0.38                   Tax amount is not calculating automatically while modifying PI/Quotation. Mantis : 26411 
 * Rev 3.0      Priti         V2.0.40                   0026881: The Expiry Date of the Proforma/Quotation is not accepting any date within the Month but taking the Last Date of any Month whiln
 * Rev 4.0      Sanchita      V2.0.40       06-10-2023  New Fields required in Sales Quotation - RFQ Number, RFQ Date, Project/Site
                                                        Mantis : 26871 
 * Rev 5.0      Sanchita      V2.0.40       19-10-2023  Coordinator data not showing in the following screen while linking Quotation / Inquiry Entries
                                                        Mantis: 26924
 * Rev 6.0      Sanchita      V2.0.43       16-05-2024  While making transaction Base rate showing less value of 1paise for this item code - 41B0150HE0181
                                                        Mantis: 27459
 **********************************************************************************************************/
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
var Pre_TotalAmt = "0";
var isCtrl = false;
//document.onkeyup = function (e) {
//    if (event.keyCode == 17) {
//        isCtrl = false;
//    }
//    else if (event.keyCode == 27) {
//        btnCancel_Click();
//    }
//}

function TaxDeleteForShipPartyChange() {
    var UniqueVal = $("#hdnGuid").val();
    $.ajax({
        type: "POST",
        url: "SalesQuotation.aspx/DeleteTaxForShipPartyChange",
        data: JSON.stringify({ UniqueVal: UniqueVal }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            // RequiredShipToPartyValue = msg.d;
        }
    });
}
function clookup_Project_LostFocus() {
    // cdtProjValidFrom.SetFocus();
    grid.batchEditApi.StartEdit(-1, 2);

    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
    //  grid.AddNewRow();
}
function ProjectValueChange(s, e) {
    //debugger;
    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'ProjectQuotation.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
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
var altn = true;
var altx = true;
document.onkeydown = function (e) {
    if (event.keyCode == 18) isCtrl = true;
    if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + s -- ie, Save & New  
        StopDefaultAction(e);
        // Rev 1.0
        //Save_ButtonClick();
        if (document.getElementById('btn_SaveRecords').style.display != 'none') {
            Save_ButtonClick();
        }
        // End of Rev 1.0
        altn = false;
    }
    else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Ctrl+X -- ie, Save & Exit!     
        StopDefaultAction(e);
        // Rev 1.0
        //SaveExit_ButtonClick();
        if (document.getElementById('ASPxButton1').style.display != 'none') {
            SaveExit_ButtonClick();
        }
        // End of Rev 1.0
        altx = false;
    }
}

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
            case 79: //// alt + o 
                if (page.GetActiveTabIndex() == 1) {

                    //Chinmoy edited on 2/05/2018  
                    // fnSaveBillingShipping();
                    ValidationBillingShipping();
                }
                break;
            case 69:
                if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
                    StopDefaultAction(e);
                    SaveTermsConditionData();
                }
                break;
            case 76:
                StopDefaultAction(e);
                calcelbuttonclick();
                break;
            case 77:
                $('#TermsConditionseModal').modal({
                    show: 'true'
                });
                break;
        }
    }
}



function watingQuotegridEndCallback() {
    if (CwatingQuotegrid.cpReturnMsg) {
        if (CwatingQuotegrid.cpReturnMsg != "") {
            jAlert(CwatingQuotegrid.cpReturnMsg);
            document.getElementById('waitingInvoiceCount').value = parseFloat(document.getElementById('waitingInvoiceCount').value) - 1;
            CwatingQuotegrid.cpReturnMsg = null;
        }
    }
}


function RemoveQuote(obj) {
    if (obj) {
        jConfirm("Clicking on Delete will not allow to use this Billing request again. Are you sure?", "Alert", function (ret) {
            if (ret) {
                CwatingQuotegrid.PerformCallback('Remove~' + obj);
            }
        });

    }
}

function OnWaitingGridKeyPress(e) {

    if (e.code == "Enter") {
        var index = CwatingQuotegrid.GetFocusedRowIndex();
        var listKey = CwatingQuotegrid.GetRowKey(index);
        if (listKey) {
            if (CwatingQuotegrid.GetRow(index).children[6].innerText != "Advance") {
                var url = 'PosSalesInvoice.aspx?key=' + 'ADD&&BasketId=' + listKey;
                LoadingPanel.Show();
                window.location.href = url;
            } else {
                ShowbasketReceiptPayment(listKey);
            }
        }
    }

}

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}

//function RecalCulateTaxTotalAmountInline() {
//    var totalInlineTaxAmount = 0;
//    for (var i = 0; i < taxJson.length; i++) {
//        cgridTax.batchEditApi.StartEdit(i, 3);
//        var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
//        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
//        if (sign == '(+)') {
//            totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(cgridTax.GetEditor("Amount").GetValue());
//        } else {
//            totalInlineTaxAmount = totalInlineTaxAmount - parseFloat(cgridTax.GetEditor("Amount").GetValue());
//        }

//        cgridTax.batchEditApi.EndEdit();
//    }

//    totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(ctxtGstCstVat.GetValue());
//    var roundedOfAmount = parseFloat(totalInlineTaxAmount);
//    ctxtTaxTotAmt.SetValue(roundedOfAmount);

//    var diffDisc = roundedOfAmount - totalInlineTaxAmount;
//    if (diffDisc > 0)
//        document.getElementById('taxroundedOf').innerText = 'Total Rounded off Amount (-) ' + Math.abs(diffDisc.toFixed(3));
//    else if (diffDisc < 0)
//        document.getElementById('taxroundedOf').innerText = 'Total Rounded off Amount (+) ' + Math.abs(diffDisc.toFixed(3));
//    else
//        document.getElementById('taxroundedOf').innerText = '';
//}


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


            //if (GSTType=="G")
            //    grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - totalGst), 2));
            //else if (GSTType == "N") {
            if (cddl_AmountAre.GetValue() == "2") {
                grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - (finalAmt * (Discount / 100)) - totalGst), 2));
            }
            //}

            cgridTax.cpTotalGST = null;

        }

        var totalNetAmount = DecimalRoundoff((parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue())), 2);
        grid.GetEditor("TotalAmount").SetValue(totalNetAmount);



        var finalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());

    }

    if (cgridTax.GetVisibleRowsOnPage() == 0) {
        $('.cgridTaxClass').hide();
        ccmbGstCstVat.Focus();
    }
    //Debjyoti Check where any Gst Present or not
    // If Not then hide the hole section

    SetRunningTotal();
    ShowTaxPopUp("IY");
    RecalCulateTaxTotalAmountInline();
}


function spLostFocus(s, e) {
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    // // hdnCustomerId
    var Saleprice = grid.GetEditor('SalePrice').GetValue();
    // GetSalesRateSchemePrice($("#hdnCustomerId").val(), strProductID, SalesPrice);

    if (strProductID != "") {
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
    QuantityTextChange(s, e);

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
    var roundedOfAmount = parseFloat(totalTaxAmount);
    //ctxtQuoteTaxTotalAmt.SetValue(roundedOfAmount);
    ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);

    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));


    //var diffDisc = roundedOfAmount - totalTaxAmount;
    //if (diffDisc > 0)
    //    document.getElementById('chargesRoundOf').innerText = 'Total Rounded off Amount (-) ' + Math.abs(diffDisc.toFixed(2));
    //else if (diffDisc < 0)
    //    document.getElementById('chargesRoundOf').innerText = 'Total Rounded off Amount (+) ' + Math.abs(diffDisc.toFixed(2));
    //else
    //    document.getElementById('chargesRoundOf').innerText = '';

}

//function ParentCustomerOnClose(newCustId, CustomerName, CustUniqueName, BillingStateText, BillingStateCode, ShippingStateText, ShippingStateCode) {
//     
//    AspxDirectAddCustPopup.Hide();
//    if (newCustId.trim() != '') {
//        page.SetActiveTabIndex(0);
//        GetObjectID('hdnCustomerId').value = newCustId;

//        GetObjectID('lblBillingStateText').value = BillingStateText;
//        GetObjectID('lblBillingStateValue').value = BillingStateCode;

//        GetObjectID('lblShippingStateText').value = ShippingStateText;
//        GetObjectID('lblShippingStateValue').value = ShippingStateCode;

//        var FullName = new Array(CustUniqueName, CustomerName);
//        ctxtCustName.SetText(CustomerName);
//       // $('#DeleteCustomer').val("yes");
//        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
//        loadAddressbyCustomerID(newCustId);
//        // cddl_SalesAgent.Focus();
//        ctxtSalesManAgent.Focus();

//    }
//}

function ParentCustomerOnClose(newCustId, customerName, Unique) {
    //clookup_CustomerControlPanelMain1.PerformCallback(newCustId);
    // ctxtCustName.SetText(customerName);

    GetObjectID('hdnCustomerId').value = newCustId;

    AspxDirectAddCustPopup.Hide();
    ctxtShipToPartyShippingAdd.SetText('');
    if (newCustId != "") {
        ctxtCustName.SetText(customerName);
        SetCustomer(newCustId, customerName);
    }
    //gridLookup.gridView.Refresh();
    //gridLookup.Focus();
}

function AddcustomerClick() {
    var isLighterPage = $("#hidIsLigherContactPage").val();

    if (isLighterPage == 1) {
        var url = '/OMS/management/Master/customerPopup.html?var=1.1.8.7';
        AspxDirectAddCustPopup.SetContentUrl(url);
        //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();
        AspxDirectAddCustPopup.RefreshContentUrl();
        AspxDirectAddCustPopup.Show();
    }
    else {
        var url = '/OMS/management/Master/Customer_general.aspx';
        AspxDirectAddCustPopup.SetContentUrl(url);
        AspxDirectAddCustPopup.Show();
        //window.location.href = url;
    }
}

function AfterSaveBillingShipiing(validate) {
    //Rev work start 06.07.2022 mantise no:25008
    GetPosForGstValue();
    //Rev work close 06.07.2022 mantise no:25008
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
//Rev work start 06.07.2022 mantise no:25008
var PosGstId = "";
function PopulatePosGst(e) {

    PosGstId = cddl_PosGstSalesOrder.GetValue();
    if (PosGstId == "S") {
        cddl_PosGstSalesOrder.SetValue("S");
        Getshhippingval(PosGstId);
    }
    else if (PosGstId == "B") {
        cddl_PosGstSalesOrder.SetValue("B");
        Getshhippingval(PosGstId);
    }

    if ($("#hdnPlaceShiptoParty").val() == "1") {
        TaxDeleteForShipPartyChange();
    }
}
function GetPosForGstValue() {

    cddl_PosGstSalesOrder.ClearItems();
    if (cddl_PosGstSalesOrder.GetItemCount() == 0) {
        cddl_PosGstSalesOrder.AddItem(GetBillingStateName() + '[Billing]', "B");
        cddl_PosGstSalesOrder.AddItem(GetShippingStateName() + '[Shipping]', "S");

    }

    else if (cddl_PosGstSalesOrder.GetItemCount() > 2) {
        cddl_PosGstSalesOrder.ClearItems();
        //cddl_PosGstSalesOrder.RemoveItem(0);
        //cddl_PosGstSalesOrder.RemoveItem(0);
    }

    if (PosGstId == "" || PosGstId == null) {
        cddl_PosGstSalesOrder.SetValue("B");
    }
    else {
        cddl_PosGstSalesOrder.SetValue(PosGstId);
    }
}
//Rev work close 06.07.2022 mantise no:25008


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
        ctxtTaxTotAmt.SetValue(parseFloat(totAmt + finalTaxAmt - taxAmountGlobal));
    } else {
        ctxtTaxTotAmt.SetValue(parseFloat(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
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
    ctxtTaxTotAmt.SetValue(parseFloat(totAmt + calculatedValue - GlobalCurTaxAmt));

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

//Code for UDF Control 
function udfAfterHide() {
    cbtn_SaveRecords.Focus();
}


function OpenUdf(s, e) {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        // var url = '../master/frm_BranchUdfPopUp.aspx?Type=SQO';

        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SQO&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();

    }
    return true;
}
// End Udf Code

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
            ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt));
        }


        var isValid = taxValue.indexOf('~');
        if (isValid != -1) {
            var rate = parseFloat(taxValue.split('~')[1]);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
            ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt));
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

            ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
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

                ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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

                ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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

                ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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

                ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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

                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = parseFloat(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);

                clblTaxProdGrossAmt.SetText(Amount);
                clblProdNetAmt.SetText(parseFloat(grid.GetEditor('Amount').GetValue()).toFixed(2));
                //document.getElementById('HdProdGrossAmt').value = Amount;
                //document.getElementById('HdProdNetAmt').value = parseFloat(grid.GetEditor('Amount').GetValue()).toFixed(2);

                document.getElementById('HdProdGrossAmt').value = parseFloat(grid.GetEditor('Amount').GetValue()).toFixed(2);
                document.getElementById('HdProdNetAmt').value = Amount;
                //End Here

                //Set Discount Here
                if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                    var discount = parseFloat((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
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

                                clblTaxProdGrossAmt.SetText(parseFloat(Amount / gstDis).toFixed(2));
                                document.getElementById('HdProdGrossAmt').value = parseFloat(Amount / gstDis).toFixed(2);
                                clblGstForGross.SetText(parseFloat(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                clblTaxableNet.SetText("");
                            }
                            else {
                                $('.gstGrossAmount').hide();
                                clblProdNetAmt.SetText(parseFloat(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2));
                                document.getElementById('HdProdNetAmt').value = parseFloat(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2);
                                clblGstForNet.SetText(parseFloat(grid.GetEditor('Amount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
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

                    //###### Added By : Samrat Roy ##########
                    //Get Customer Shipping StateCode
                    var shippingStCode = '';
                    //Chinmoy edited
                    //Rev work start 06.07.2022 mantise no:25008
                    //shippingStCode = ctxtshippingState.GetText();
                    //if (shippingStCode.trim() != "") {
                    //    shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();
                    //}
                    if (cddl_PosGstSalesOrder.GetValue() == "S") {
                        shippingStCode = GeteShippingStateCode();
                    }
                    else {
                        shippingStCode = GetBillingStateCode();
                    }
                    shippingStCode = shippingStCode;
                    //Rev work close 06.07.2022 mantise no:25008
                    //Debjyoti 09032017
                    if (shippingStCode.trim() != '') {
                        for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                            //Check if gstin is blank then delete all tax
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {

                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {

                                    //if its state is union territories then only UTGST will apply
                                    if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "35" || shippingStCode == "31" || shippingStCode == "34") {
                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S') {
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
    //console.log(grid.GetFocusedRowIndex());
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

var taxJson;
function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}
//Rev Rajdip For Delete SalesMan
function Deletesalesman() {

    $("#hdnSalesManAgentId").val("");
    ctxtSalesManAgent.SetText("");
}

$(document).ready(function () {
    var mode = $('#hdnPageStatus').val();
    if (mode == 'update') {
        if ($("#hdnPageStatus").val() != "") {
            var VendorID = $("#hdnCustomerId").val();
            SetEntityType(VendorID);
        }
    }


    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
    }
    $('#ApprovalCross').click(function () {

        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.Refresh()();
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

})


function GetBillingAddressDetailByAddressId(e) {
    var addresskey = billingLookup.GetGridView().GetRowKey(billingLookup.GetGridView().GetFocusedRowIndex());
    if (addresskey != null && addresskey != '') {

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

    var QuoteNo = ctxt_PLQuoteNo.GetText();
    if (QuoteNo != '') {
        var CheckUniqueCode = false;
        $.ajax({
            type: "POST",
            url: "SalesQuotation.aspx/CheckUniqueCode",
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
function CloseGridLookup() {
    //gridLookup.ConfirmCurrentSelection();
    //gridLookup.HideDropDown();
    //gridLookup.Focus();
}
function GetContactPerson(id) {

    //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()); // Abhisek
    var key = id;
    if (key != null && key != '') {

        cContactPerson.PerformCallback('BindContactPerson~' + key);
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);

        //###### Added By : Samrat Roy ##########
        SetDefaultBillingShippingAddress(key);

        //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'QO');
        GetObjectID('hdnCustomerId').value = key;
        if ($('#hfBSAlertFlag').val() == "1") {
            jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    page.SetActiveTabIndex(1);
                    //Chinmoy edited 
                    page.tabs[0].SetEnabled(false);
                    $("#divcross").hide();
                }
            });
        }
        else {
            page.SetActiveTabIndex(1);
            page.tabs[0].SetEnabled(false);
            $("#divcross").hide();
        }
        //###### END : Samrat Roy : END ########## 

        GetObjectID('hdnAddressDtl').value = '0';

        //page.SetActiveTabIndex(1);
        //$('.dxeErrorCellSys').addClass('abc');
        //$('.crossBtn').hide();
        //page.GetTabByName('General').SetEnabled(false);

        if (grid.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();
        }

        if (grid.GetEditor('ProductID').GetText() != "") {
            grid.PerformCallback('GridBlank');
            ccmbGstCstVat.PerformCallback();
            ccmbGstCstVatcharge.PerformCallback();
            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        }

    }

}
$(document).ready(function () {

    //Added by :Subhabrata on 03-07-2017
    var hddnCRmVal = $("#hddnCustIdFromCRM").val();
    var CustId = $("#hdnCustomerId").val();
    if (hddnCRmVal == "1") {
        SetDefaultBillingShippingAddress(CustId);
        //LoadCustomerAddress(CustId, $('#ddl_Branch').val(), 'QO');
    }
    //End

    var schemaid = $('#ddl_numberingScheme').val();
    if (schemaid != null) {
        if (schemaid == '') {
            ctxt_PLQuoteNo.SetEnabled(false);
        }
    }
    $('#ddl_numberingScheme').change(function () {
        var NoSchemeTypedtl = $(this).val();
        var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
        var quotelength = NoSchemeTypedtl.toString().split('~')[2];

        var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";

        //Cut Off  Valid from To Date Sudip

        var fromdate = NoSchemeTypedtl.toString().split('~')[5];
        var todate = NoSchemeTypedtl.toString().split('~')[6];
        // alert(fromdate + '   ' + todate);
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


        //Cut Off  Valid from To Date Sudip

        if (branchID != "") document.getElementById('ddl_Branch').value = branchID;

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
        else {
            ctxt_PLQuoteNo.SetText('');
            ctxt_PLQuoteNo.SetEnabled(false);
            //ctxt_PLQuoteNo.SetClientEnabled(true);
        }
        clookup_Project.gridView.Refresh();
    });


});

var PreviousCurrency = "1";
function GetPreviousCurrency() {
    PreviousCurrency = ctxt_Rate.GetValue();
}

function SetFocusonDemand(e) {
    var key = cddl_AmountAre.GetValue();
    if (key == '1' || key == '3') {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    }
    else if (key == '2') {
        // cddlVatGstCst.Focus();
        if ($("#hdnProjectSelectInEntryModule").val() == "1") {
            clookup_Project.SetFocus();

        }
        else {
            grid.batchEditApi.StartEdit(-1, 2);
        }
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
        // Rev 1.0
        cbtn_SaveRecords_N.SetVisible(true);
        cbtn_SaveRecords_p.SetVisible(true);
        // End of Rev 1.0
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
        // Rev 1.0
        cbtn_SaveRecords_N.SetVisible(true);
        cbtn_SaveRecords_p.SetVisible(true);
        // End of Rev 1.0
    }
    else if (key == 3) {

        grid.GetEditor('TaxAmount').SetEnabled(false);

        //cddlVatGstCst.PerformCallback('3');
        cddlVatGstCst.SetSelectedIndex(0);
        cddlVatGstCst.SetEnabled(false);
        cbtn_SaveRecords.SetVisible(false);
        // Rev 1.0
        cbtn_SaveRecords_N.SetVisible(false);
        cbtn_SaveRecords_p.SetVisible(false);
        // End of Rev 1.0
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }


    }

}

function Onddl_VatGstCstEndCallback(s, e) {
    if (s.GetItemCount() == 1) {
        cddlVatGstCst.SetEnabled(false);
    }
}


//Date Function Start

function Startdate(s, e) {
    grid.batchEditApi.EndEdit();
    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    if ($("#hdBasketId").val == "") {
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
            $('#hdfIsDelete').val('D');
            $('#HdUpdateMainGrid').val('True');
            grid.UpdateEdit();
        }

        if (t == "")
        { $('#MandatorysDate').attr('style', 'display:block'); }
        else { $('#MandatorysDate').attr('style', 'display:none'); }
    }
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
        grid.PerformCallback('CurrencyChangeDisplay~' + PreviousCurrency);
    }
}

function ReBindGrid_CurrencyByRate() {
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
        grid.PerformCallback('CurrencyRateChangeDisplay~' + PreviousCurrency);
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
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

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
        $('#cpSaveSuccessOrFaillblPackingStk').text(PackingValue);
        divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }
    //divPacking.style.display = "none";

    //lblbranchName lblProduct
    //tbStkUOM.SetValue(strStkUOM);
    //tbStockQuantity.SetValue("0");
    //Debjyoti
    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
    //cacpAvailableStock.PerformCallback(strProductID);
}

function OnEndCallback(s, e) {

    var value = document.getElementById('hdnRefreshType').value;
    var IsFromActivity = document.getElementById('hdnIsFromActivity').value;
    $('#hdnDeleteSrlNo').val('');
    //Debjyoti Check grid needs to be refreshed or not
    if ($('#HdUpdateMainGrid').val() == 'True') {
        $('#HdUpdateMainGrid').val('False');
        if ($("#hdBasketId").val() == "") {
            grid.PerformCallback('DateChangeDisplay');
        }
    }

    if (grid.cpCRMSavedORNot == "crmQuotationSaved") {
        parent.EnabledSaveBtn();
        grid.cpCRMSavedORNot = null;
    }


    if (grid.cpSaveSuccessOrFail == "outrange") {
        jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
        grid.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });

    }
    else if (grid.cpSaveSuccessOrFail == "BillingShippingNull") {
        grid.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        if (altn == false) {
            jAlert("modified data Loding progress.. press 'OK' to continue");
            Save_ButtonClick();
        }
        else if (altx == false) {
            jAlert("modified data Loding progress.. press 'OK' to continue");
            SaveExit_ButtonClick();
        }
        else {
            jAlert("Billing & Shipping is mandatory, please enter Billing & Shipping address and proceed");
        }
    }
    else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
        grid.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });

    }
    else if (grid.cpSaveSuccessOrFail == "duplicate") {
        jAlert('Can Not Save as Duplicate Quotation Numbe No. Found');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "AddLock") {
        jAlert('Adding Quotation is not allowed as the Data is feeezed from ' + grid.cpAddLockStatus);
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData") {
        var SrlNo = grid.cpcheckMultiUOMData;
        var msg = "Please add Alt. Qty for SL No. " + SrlNo;
        grid.cpcheckMultiUOMData = null;
        grid.cpSaveSuccessOrFail = "";
        grid.cpSaveSuccessOrFail = null;
        jAlert(msg);
        OnAddNewClick();
    }
    // Rev 1.0
    else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData_QtyMismatch") {
        OnAddNewClick();
        grid.cpSaveSuccessOrFail = null;
        var SrlNo = grid.cpcheckMultiUOMData;
        var msg = "Please check Multi UOM details for SL No. not matching with outer grid " + SrlNo;
        grid.cpcheckMultiUOMData = null;
        jAlert(msg);
        grid.cpSaveSuccessOrFail = '';
    }
    else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData_NotFound") {
        OnAddNewClick();
        grid.cpSaveSuccessOrFail = null;
        var SrlNo = grid.cpcheckMultiUOMData;
        var msg = "Multi UOM details not given for SL No. " + SrlNo;
        grid.cpcheckMultiUOMData = null;
        jAlert(msg);
        grid.cpSaveSuccessOrFail = '';
    }
    // End of Rev 1.0
    else if (grid.cpSaveSuccessOrFail == "ExceedQuantity") {
        // grid.batchEditApi.StartEdit(0, 2);
        grid.cpSaveSuccessOrFail = null;
        jAlert('Tagged product quantity exceeded.Update The quantity and Try Again.');
    }
    else if (grid.cpSaveSuccessOrFail == "ReducedQuantity") {
        // grid.batchEditApi.StartEdit(0, 2);
        grid.cpSaveSuccessOrFail = null;
        jAlert('Tagged product quantity can not reduced.Update The quantity and Try Again.');
    }

    else if (grid.cpSaveSuccessOrFail == "quantityTagged") {
        jAlert('Proforma is tagged in Sale Order. So, Quantity of selected products cannot be less than Ordered Quantity.');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "errorInsert") {
        jAlert('Please try again later.');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
        jAlert('Please select project.');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
        jAlert('Can not Duplicate Product in the Quotation List.');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
        // Rev Sanchita
        grid.batchEditApi.StartEdit(0, 2);
        // End of Rev Sanchita
        jAlert('Please fill Quantity');
        // Rev Sanchita
        //OnAddNewClick();
        // End of Rev Sanchita
    }
    else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
        var SrlNo = grid.cpProductSrlIDCheck;
        var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
        jAlert(msg);
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "nullAmount") {
        // Rev Sanchita
        grid.batchEditApi.StartEdit(0, 2);
        // End of Rev Sanchita
        jAlert('total amount cant not be zero(0).');
        // Rev Sanchita
        //OnAddNewClick();
        // End of Rev Sanchita
    }
    else if (grid.cpSaveSuccessOrFail == "errorUdf") {
        jAlert('UDF is set as Mandatory. Please enter values.', 'Alert', function () { OpenUdf(); OnAddNewClick(); });

    }
    else if (grid.cpSaveSuccessOrFail == "nullStateCode") {
        grid.cpSaveSuccessOrFail = null;
        if (altn == false) {
            jAlert("modified data Loding progress.. press 'OK' to continue");
        }
        else if (altx == false) {
            jAlert("modified data Loding progress.. press 'OK' to continue");
            SaveExit_ButtonClick();
        }
        else {
            jAlert("Billing & Shipping is mandatory, please enter Billing & Shipping address and proceed");
        }
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "Addnewrow") {
        grid.cpSaveSuccessOrFail = null;
        OnAddNewClick();
    }

    else if (grid.cpSaveSuccessOrFail == "Addnewrowfromsalesactivty") {
        grid.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        var custname = GetObjectID('hdnCustomerId').value;
        SetDefaultBillingShippingAddress(custname);
        // LoadCustomerAddress(custname, $('#ddl_Branch').val(), 'QO');
    }
    else {
        var Quote_Number = grid.cpQuotationNo;
        var Quote_Msg = "Sales Quotation No. '" + Quote_Number + "' saved.";

        if (IsFromActivity == "Y" && value == "E") {
            $('#hdnRefreshType').val('');
            if (Quote_Number != "") {
                var strconfirm = alert(Quote_Msg);
                if (strconfirm == true) {
                    self.close();
                }
                else {
                    self.close();
                }
            }
            else {
                self.close();
            }
        }
        else if (value == "E") {
            $('#hdnRefreshType').val('');
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
                    //var strconfirm = alert(Quote_Msg);
                    //if (strconfirm == true) {
                    //    window.location.assign("SalesQuotationList.aspx");
                    //}
                    //else {
                    //    window.location.assign("SalesQuotationList.aspx");
                    //}

                    jAlert(Quote_Msg, 'Alert Dialog: [SalesQuotation]', function (r) {
                        if (r == true) {
                            window.location.assign("SalesQuotationList.aspx");
                        }
                    });
                }
                else {
                    window.location.assign("SalesQuotationList.aspx");
                }
            }
        }
        else if (value == "N") {
            $('#hdnRefreshType').val('');
            if (grid.cpApproverStatus == "approve") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else {
                if (Quote_Number != "") {
                    //var strconfirm = confirm(Quote_Msg);
                    //if (strconfirm == true) {
                    //    window.location.assign("SalesQuotation.aspx?key=ADD");
                    //}
                    //else {
                    //    window.location.assign("SalesQuotation.aspx?key=ADD");
                    //}

                    jAlert(Quote_Msg, 'Alert Dialog: [SalesQuotation]', function (r) {
                        if (r == true) {
                            window.location.assign("SalesQuotation.aspx?key=ADD");
                        }
                    });
                }
                else {
                    window.location.assign("SalesQuotation.aspx?key=ADD");
                }
            }
        }
        else {
            var pageStatus = document.getElementById('hdnPageStatus').value;
            if (pageStatus == "first") {
                OnAddNewClick();
                grid.batchEditApi.EndEdit();
                // it has been commented by sam on 04032017 due to set focus from server side start
                //document.getElementById("ddl_numberingScheme").focus();
                // above part has been commented by sam on 04032017 due to set focus from server side start

                $('#hdnPageStatus').val('');
            }
            else if (pageStatus == "update") {
                OnAddNewClick();
                $('#hdnPageStatus').val('');
            }
            else {
                grid.StartEditRow(0);
                $('#hdnPageStatus').val('');
            }
        }
    }
}

function Reject_ButtonClick() {
    if ($("#hdnPageStatForApprove").val() == "QN" && $("#hdnApprovalsettings").val() == "1") {
        if ($("#txtAppRejRemarks").val() == "") {
            jAlert("Please Enter Reject Remarks.")
            $("#txtAppRejRemarks").focus();
            return false;
        }
    }
    var otherdet = {};
    otherdet.ApproveRemarks = $("#txtAppRejRemarks").val();
    otherdet.ApproveRejStatus = 2;
    otherdet.OrderId = $("#hdnEditOrderId").val();
    $.ajax({
        type: "POST",
        url: "SalesQuotation.aspx/SetApproveReject",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var value = msg.d;
            if (value == "1") {
                jAlert("Quotation Rejected.");
                window.location.href = "projectOrderList.aspx";
            }
        }
    });
}



function Approve_ButtonClick() {
    flag = true;
    if ($("#hdnPageStatForApprove").val() == "QN" && $("#hdnApprovalsettings").val() == "1") {
        if ($("#txtAppRejRemarks").val() == "") {
            flag = false;
            jAlert("Please Enter Approval Remarks.")
            $("#txtAppRejRemarks").focus();
            return false;
        }
    }
    $("#hdnApproveStatus").val(1);
    $('#hfControlData').val($('#hfControlSaveData').val());
    grid.batchEditApi.EndEdit();
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        flag = false;
    }
    // Quote no validation Start	
    var QuoteNo = ctxt_PLQuoteNo.GetText();
    if (QuoteNo == '' || QuoteNo == null) {
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
        flag = false;
        $('#MandatorysDate').attr('style', 'display:block');
    }
    else { $('#MandatorysDate').attr('style', 'display:none'); }
    if (edate == null || sdate == "") {
        flag = false;
        $('#MandatoryEDate').attr('style', 'display:block');
    }
    else {
        $('#MandatoryEDate').attr('style', 'display:none');
        //Rev 3.0
       // if (startDate > endDate) {
        /* if (tstartdate.GetText() > tenddate.GetText()) {*/
        if (startDate > endDate) {
        //Rev 3.0 End
            flag = false;
            $('#MandatoryEgSDate').attr('style', 'display:block');
        }
        else {
            $('#MandatoryEgSDate').attr('style', 'display:none');
        }
    }
    // Quote Date validation End	
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
        var taxcodeid = cddlVatGstCst.GetValue();
        //if (taxcodeid == '' || taxcodeid == null) {	
        //    $('#Mandatorytaxcode').attr('style', 'display:block');	
        //    flag = false;	
        //}	
        //else {	
        //    $('#Mandatorytaxcode').attr('style', 'display:block');	
        //}	
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
            if (aarr.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "SalesQuotation.aspx/SetSessionPacking",
                    data: "{'list':'" + JSON.stringify(aarr) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var customerval = GetObjectID('hdnCustomerId').value;
                        $('#hdfLookupCustomer').val(customerval);
                        $('#hdnRefreshType').val('E');
                        $('#hdfIsDelete').val('I');
                        grid.batchEditApi.EndEdit();
                        grid.UpdateEdit();
                    }
                });
            }
            else {
                //divSubmitButton.style.display = "none";	
                //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : ""; // Abhisek	
                var customerval = GetObjectID('hdnCustomerId').value;
                $('#hdfLookupCustomer').val(customerval);
                $('#hdnRefreshType').val('E');
                $('#hdfIsDelete').val('I');
                grid.batchEditApi.EndEdit();
                grid.UpdateEdit();
            }
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}

function SaveExit_ButtonClick() {

    flag = true;
    $('#hfControlData').val($('#hfControlSaveData').val());
    grid.batchEditApi.EndEdit();
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        flag = false;
    }



    var revdate = ctxtRevisionDate.GetText();
    var RevisionDate = new Date(revdate);
    if ($("#hdnApproveStatus").val() == 1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalsettings").val() == "1") {
        if (revdate == "01-01-0100" || revdate == null || revdate == "") {
            flag = false;
            // LoadingPanel.Hide();	
            jAlert("Please Enter Revision Details.");
            ctxtRevisionDate.SetFocus();
            return false;
        }
    }
    if ($("#hdnApproveStatus").val() == 1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalsettings").val() == "1") {
        if (ctxtRevisionNo.GetText() == "") {
            flag = false;
            //  LoadingPanel.Hide();	
            jAlert("Please Enter Revision Details.");
            ctxtRevisionNo.SetFocus();
            return false;
        }
    }

    // Quote no validation Start
    var QuoteNo = ctxt_PLQuoteNo.GetText();
    if (QuoteNo == '' || QuoteNo == null) {
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
        flag = false;
        $('#MandatorysDate').attr('style', 'display:block');
    }
    else { $('#MandatorysDate').attr('style', 'display:none'); }
    if (edate == null || sdate == "") {
        flag = false;
        $('#MandatoryEDate').attr('style', 'display:block');
    }
    else {
        $('#MandatoryEDate').attr('style', 'display:none');
        if (startDate > endDate) {
        //if (tstartdate.GetText() > tenddate.GetText()) {
            flag = false;
            $('#MandatoryEgSDate').attr('style', 'display:block');
        }
        else {
            $('#MandatoryEgSDate').attr('style', 'display:none');
        }
    }
    // Quote Date validation End

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
        var taxcodeid = cddlVatGstCst.GetValue();
        //if (taxcodeid == '' || taxcodeid == null) {
        //    $('#Mandatorytaxcode').attr('style', 'display:block');
        //    flag = false;
        //}
        //else {
        //    $('#Mandatorytaxcode').attr('style', 'display:block');
        //}
    }


    if ($("#hdnApproveStatus").val() == 1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalsettings").val() == "1") {
        var detRev = {};
        detRev.RevNo = ctxtRevisionNo.GetText();
        detRev.Order = $("#hdnEditOrderId").val();
        $.ajax({
            type: "POST",
            url: "SalesQuotation.aspx/Duplicaterevnumbercheck",
            data: JSON.stringify(detRev),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var duplicateRevCheck = msg.d;
                if (duplicateRevCheck == 1) {
                    flag = false;
                    //  LoadingPanel.Hide();	
                    jAlert("Please Enter a valid Revision No");
                    ctxtRevisionNo.SetFocus();
                }
            }
        });
        //End Rev Rajdip	
        //}	
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

            if (aarr.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "SalesQuotation.aspx/SetSessionPacking",
                    data: "{'list':'" + JSON.stringify(aarr) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var customerval = GetObjectID('hdnCustomerId').value;
                        $('#hdfLookupCustomer').val(customerval);

                        $('#hdnRefreshType').val('E');
                        $('#hdfIsDelete').val('I');
                        grid.batchEditApi.EndEdit();
                        grid.UpdateEdit();
                    }
                });
            }
            else {
                //divSubmitButton.style.display = "none";
                //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : ""; // Abhisek
                var customerval = GetObjectID('hdnCustomerId').value;
                $('#hdfLookupCustomer').val(customerval);

                $('#hdnRefreshType').val('E');
                $('#hdfIsDelete').val('I');
                grid.batchEditApi.EndEdit();
                grid.UpdateEdit();
            }
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}

function QuantityLostFocus(s, e) {
    QuantityTextChange(s, e);
    //DiscountTextChange(s, e);
}
function UomLostFocus(s, e) {
    if ($("#hddnMultiUOMSelection").val() == "0") {
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 11);
        }, 600)
    }
}

var Uomlength = 0;
function UomLenthCalculation() {
    grid.batchEditApi.StartEdit(globalRowIndex);
    var SLNo = "";
    var val = 0;
    var detailsid = grid.GetEditor('DetailsId').GetValue();
    if (detailsid != null && detailsid != "") {
        SLNo = detailsid;
        val = 1;
    }
    else {
        SLNo = grid.GetEditor('SrlNo').GetValue();
    }
    $.ajax({
        type: "POST",
        url: "SalesQuotation.aspx/GetQuantityfromSL",
        data: JSON.stringify({ SLNo: SLNo, val: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            Uomlength = msg.d;

        }
    });
}


function QuantityTextChange(s, e) {

    //chinmoy added for multiUom start

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
    //                        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    //                    }, 600)
    //                }
    //                else {
    //                    grid.batchEditApi.StartEdit(globalRowIndex);
    //                    grid.GetEditor('Quantity').SetValue(qnty);
    //                    setTimeout(function () {
    //                        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    //                    }, 200);
    //                }


    //            });
    //        }
    //        else {
    //            grid.batchEditApi.StartEdit(globalRowIndex);
    //            grid.GetEditor('Quantity').SetValue(qnty);

    //            setTimeout(function () {
    //                grid.batchEditApi.StartEdit(globalRowIndex, 6);
    //            }, 600)

    //        }
    //    }

    //}
    //End
    // End of Rev Sanchita

    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strMultiplier = SpliteDetails[7];
        var strFactor = SpliteDetails[8];
        var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
        var strProductID = SpliteDetails[0];
        var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();
        //var strRate = "1";
        var strStkUOM = SpliteDetails[4];
        //chinmoy edited below code
        var strSalePrice = SpliteDetails[6];
        if (parseInt(strSalePrice) == parseInt(0)) {
            strSalePrice = grid.GetEditor('SalePrice').GetValue();
        }
        //End
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
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
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



    //DiscountTextChange(s, e);

    var inx = globalRowIndex;
    SetTotalTaxableAmount(inx, e);
    SetInvoiceLebelValue();

}
//Rev Rajdip
function Taxlostfocus(s, e) {

    //DiscountTextChange(s, e);
    //Rev Rajdip for Running Balance
    //SetTotalTaxableAmount(s, e);
    //SetInvoiceLebelValue();
}
function TotalAmountgotfocus(s, e) {

    //DiscountTextChange(s, e);
    SetTotalTaxableAmount(s, e);
    //SetInvoiceLebelValue();
}
function SetInvoiceLebelValue() {

    var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
    cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));
    // SetRunningBalance();

}
//End Rev Rajdip
/// Code Added By Sam on 23022017 after make editable of sale price field Start
var globalNetAmount = 0;
function SalePriceTextChange(s, e) {

    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");

        if (parseFloat(s.GetValue()) < parseFloat(SpliteDetails[17])) {
            jAlert("Sale price cannot be lesser than Min Sale Price locked as: " + parseFloat(parseFloat(Math.abs(parseFloat(SpliteDetails[17])) * 100) / 100).toFixed(2), "Alert", function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 10);
                return;
            });
            s.SetValue(parseFloat(SpliteDetails[6]));
            return;
        }


        if (parseFloat(SpliteDetails[18]) != 0 && parseFloat(s.GetValue()) > parseFloat(SpliteDetails[18])) {
            jAlert("Sale price cannot be greater than MRP locked as: " + parseFloat(parseFloat(Math.abs(parseFloat(SpliteDetails[18])) * 100) / 100).toFixed(2), "Alert", function () {
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
        var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(amountAfterDiscount);

        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(amountAfterDiscount);


        //var TaxType = "";
        //if (cddl_AmountAre.GetValue() == "1") {
        //    TaxType = "E";
        //}
        //else if (cddl_AmountAre.GetValue() == "2") {
        //    TaxType = "I";
        //}

        //var CompareStateCode;
        ////if (cddl_PosGstSalesOrder.GetValue() == "S") {
        //    CompareStateCode = GeteShippingStateCode();
        ////}
        ////else {
        ////    CompareStateCode = GetBillingStateCode();
        ////}
        //Rev work start 06.07.2022 mantise no:25008
        var ShippingStateCode = $("#bsSCmbStateHF").val();
        var CompareStateCode;
        if (cddl_PosGstSalesOrder.GetValue() == "S") {
            CompareStateCode = GeteShippingStateID();
        }
        else {
            CompareStateCode = GetBillingStateID();
        }
        //Rev work close 06.07.2022 mantise no:25008
        //     
        //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
        //    SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val());



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

        //ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
        //cacpAvailableStock.PerformCallback(strProductID);


        //Rev Rajdip           
        //var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
        //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);

        //cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
        //End Rev rajdip


        //Rev Rajdip for Running Balance
        //SetTotalTaxableAmount(s, e);
        //SetInvoiceLebelValue();

        //End Rev Rajdip
        //cacpAvailableStock.PerformCallback(strProductID);
        // GetSalesRateSchemePrice($("#hdnCustomerId").val(), ProductID, Saleprice);

    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('SalePrice').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
}
//Rev Rajdip For Running Parameters
function SetTotalTaxableAmount(inx, vindex) {

    var count = grid.GetVisibleRowsOnPage();
    var totalAmount = 0;
    var totaltxAmount = 0;
    var totalQuantity = 0;
    for (var i = 0; i < count + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("Quantity").GetValue(), 4);
                if (grid.GetEditor("TaxAmount").GetValue() != null) {
                    totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2);
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                }
                else {
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                }
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
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))

                }
                else {
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                }
                //totalAmount = totalAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "Amount"), 2);
                //totaltxAmount = totaltxAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "TaxAmount"), 2);



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
    cbnrLblTaxableAmtval.SetText(DecimalRoundoff(totalAmount, 2));
    cbnrLblTaxAmtval.SetText(DecimalRoundoff(totaltxAmount, 2));
    cbnrLblTotalQty.SetText(DecimalRoundoff(totalQuantity, 4));
    var totamt = totalAmount + totaltxAmount;
    cbnrlblAmountWithTaxValue.SetText(DecimalRoundoff(totamt, 2));
    cbnrLblInvValue.SetText(DecimalRoundoff(totamt, 2));
    //grid.batchEditApi.StartEdit(globalRowIndex, vindex);
    //setTimeout(function () { grid.batchEditApi.StartEdit(inx, vindex); }, 200)
}
//Rev Rajdip
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


/// Code Above Added By Sam on 23022017 after make editable of sale price field End

function DiscountTextChange(s, e) {
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
        tbAmount.SetValue(DecimalRoundoff(amountAfterDiscount,2));

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

        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(DecimalRoundoff(amountAfterDiscount,2));
        //Rev work start 06.07.2022 mantise no:25008
         var ShippingStateCode = $("#bsSCmbStateHF").val();
        //var ShippingStateCode = GeteShippingStateID();
        //Rev work close 06.07.2022 mantise no:25008
        var TaxType = "";
        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }
        //Rev work start 06.07.2022 mantise no:25008
        var CompareStateCode;
        if (cddl_PosGstSalesOrder.GetValue() == "S") {
            CompareStateCode = GeteShippingStateID();
        }
        else {
            CompareStateCode = GetBillingStateID();
        }
        //Rev work close 06.07.2022 mantise no:25008
        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), tstartdate.GetDate(), QuantityValue);
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    
   // Rev 2.0
    var SrlNo = grid.GetEditor("SrlNo").GetValue();
    var UniqueVal = $("#uniqueId").val();
        $.ajax({
            type: "POST",
            url: "SalesQuotation.aspx/DeleteTaxForRateChange",
            data: JSON.stringify({ UniqueVal: UniqueVal, SrlNo: SrlNo }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                // RequiredShipToPartyValue = msg.d;
            }
        });

    //}
    // End of Rev 2.0
}
var _GetAmountValue = "0";
function AmountTextFocus(s, e) {
    _GetAmountValue = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
}

function ProductPriceCalculate() {
    if ((grid.GetEditor('SalePrice').GetValue() == null || grid.GetEditor('SalePrice').GetValue() == 0) && (grid.GetEditor('Discount').GetValue() == null || grid.GetEditor('Discount').GetValue() == 0)) {
        var _saleprice = 0;
        var _Qty = grid.GetEditor('Quantity').GetValue();
        var _Amount = grid.GetEditor('Amount').GetValue();
        _saleprice = (_Amount / _Qty);
        grid.GetEditor('SalePrice').SetValue(_saleprice);
    }
}
function ProductAmountTextChange(s, e) {
    ProductPriceCalculate();
    var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var TaxAmount = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    if (parseFloat(_GetAmountValue) != parseFloat(Amount)) {
        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(Amount + TaxAmount);

        ////////////////// For Tax

        var ProductID = grid.GetEditor('ProductID').GetValue();
        var SpliteDetails = ProductID.split("||@||");

        // var ShippingStateCode = $("#bsSCmbStateHF").val();
        var ShippingStateCode = GeteShippingStateID();


        var TaxType = "";
        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }

        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[19], Amount, Amount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), tstartdate.GetDate(), QuantityValue);
        //  ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
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
    grid.AddNewRow();
    //grid.GetEditor('SrlNo').SetEnabled(false);

    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var tbQuotation = grid.GetEditor("SrlNo");
    tbQuotation.SetValue(noofvisiblerows);
    /// Mantis Issue 24428 
    $("#UOMQuantity").val(0);
    Uomlength = 0;
    // End of Mantis Issue 24428 

    //var i;
    //var cnt = 1;
    //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
    //    var tbQuotation = grid.GetEditor("SrlNo");
    //    tbQuotation.SetValue(cnt);
    //    cnt++;
    //}
}
//function OnAddNewClick_AtSaveTime() {
//    
//    if (gridquotationLookup.GetValue() == null) {
//        grid.AddNewRow();

//        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
//        var tbQuotation = grid.GetEditor("SrlNo");
//        tbQuotation.SetValue(noofvisiblerows);
//    }
//    else {
//        grid.batchEditApi.StartEdit(0, 5);
//    }
//}

function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cbnrOtherChargesvalue.SetText('0.00');
    SetInvoiceLebelValueofothercharges();
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
        cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
        cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));
        cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));
        cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));


        $('#hdnRefreshType').val('');
        $('#hdnDeleteSrlNo').val(SrlNo);
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        if (noofvisiblerows != "1") {
            grid.DeleteRow(e.visibleIndex);

            $('#hdfIsDelete').val('D');


            grid.UpdateEdit();
            grid.PerformCallback('Display');

            $('#hdnPageStatus').val('delete');
            //grid.batchEditApi.StartEdit(-1, 2);
            //grid.batchEditApi.StartEdit(0, 2);
        }
    }

    else if (e.buttonID == "CustomaddDescRemarks") {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex);
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
        setTimeout(function () {
            //grid.batchEditApi.StartEdit(globalRowIndex, 8);
            document.getElementById("txtInlineRemarks").focus();
        }, 600);
        // document.getElementById("txtInlineRemarks").focus();
    }

    else if (e.buttonID == 'AddNew') {
        var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        if (ProductIDValue != "") {
            OnAddNewClick();
        }
        else {
            grid.batchEditApi.StartEdit(e.visibleIndex, 2);
        }
    }


    else if (e.buttonID == 'CustomMultiUOM') {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex, 6);
        var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ProductID = Productdetails.split("||@||")[0];
        var UOMName = grid.GetEditor("UOM").GetValue();
        var quantity = grid.GetEditor("Quantity").GetValue();
        var DetailsId = grid.GetEditor('DetailsId').GetText();
        var StockUOM = Productdetails.split("||@||")[5];
        hdProductID.value = ProductID;
        if (StockUOM == "") {
            StockUOM = "0";
        }
        if (DetailsId = "")
        {
            DetailsId = "0";
        }

        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        cAltUOMQuantity.SetValue("0.0000");
        //Rev Bapi
        //   if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
        if ((ProductID != "") && (UOMName != "")) {
            //End Rev Bapi
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
                    // Mantis Issue 24428
                    //$("#UOMQuantity").val(Qnty);
                    $("#UOMQuantity").val("0.0000");
                    ccmbBaseRate.SetValue(0)
                    cAltUOMQuantity.SetValue(0)
                    ccmbAltRate.SetValue(0)
                    ccmbSecondUOM.SetValue("")
                    // End of Mantis Issue 24428
                    // Rev 1.0
                    document.getElementById('lblInfoMsg').innerHTML = "";
                    cbtn_SaveRecords_N.SetVisible(false);
                    cbtn_SaveRecords_p.SetVisible(false);
                    // End of Rev 1.0
                    cPopup_MultiUOM.Show();
                    cgrid_MultiUOM.cpDuplicateAltUOM = "";
                    AutoPopulateMultiUOM();
                    //chinmoy change start
                    cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo + '~' + DetailsId);
                    //cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + ProductID);
                }
            }//End
        }
        else {
            return;
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;
        var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

        if (inventoryType == "C" || inventoryType == "Y") {
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
                    jAlert("No Warehouse or Batch or Serial is actived !");
                }
            }
            else if (ProductID != "" && parseFloat(QuantityValue) == 0) {

                jAlert("Please enter Quantity !");
            }
        }
        else {
            jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
        }
    }
}

// Mantis Issue 24428 
function CalcBaseQty() {
    //debugger;

    // var PackingQtyAlt = Productdetails.split("||@||")[20]; // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
    //var PackingQty = Productdetails.split("||@||")[22]; // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
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
   // debugger;
    var altQty = cAltUOMQuantity.GetValue();
    var altRate = ccmbAltRate.GetValue();
    var baseQty = $("#UOMQuantity").val();

    // Rev 6.0
    //if (baseQty > 0) {
    //    var BaseRate = (altQty * altRate) / baseQty;
    //    ccmbBaseRate.SetValue(BaseRate);
    //}
    if (baseQty > 0) {
        if (parseFloat(baseQty).toFixed(4) == parseFloat(altQty).toFixed(4)) {
            var BaseRate = altRate;
            ccmbBaseRate.SetValue(BaseRate);
        }
        else {
            var BaseRate = (altQty * altRate) / baseQty;
            ccmbBaseRate.SetValue(BaseRate);
        }
    }
    // End of Rev 6.0
}
// End of Mantis Issue 24428 

function closeRemarks(s, e) {

    cPopup_InlineRemarks.Hide();
    //e.cancel = false;
    //ccallback_InlineRemarks.PerformCallback('RemarksDelete'+'~'+grid.GetEditor('SrlNo').GetValue()+'~'+$('#txtInlineRemarks').val());
    //cPopup_InlineRemarks.Hide();
    //e.cancel = false;
    // cPopup_InlineRemarks.Hide();
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
    cPopup_InlineRemarks.Hide();

}


function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');
    //Rev Subhra 15-05-2019
    grid.batchEditApi.StartEdit(globalRowIndex, 10);
    //End of Rev Subhra 15-05-2019
}

function FinalMultiUOM() {

    UomLenthCalculation();
    if (Uomlength == 0 || Uomlength < 0) {

        // Mantis Issue 24428 
        //jAlert("Please add Alt. Quantity.");
        jAlert("Please add atleast one Alt. Quantity with Update Row as checked.");
        // End of Mantis Issue 24428 
        return;
    }
    else {
        // Rev 1.0
        //cPopup_MultiUOM.Hide();
        // End of Rev 1.0

        // Mantis Issue 24428 
        var SLNo = grid.GetEditor('SrlNo').GetValue();
        cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~' + SLNo);
        // End of Mantis Issue 24428 
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 11);
        }, 200)
    }
}


function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
}

// Rev 1.0
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
// End of Rev 1.0

function closeMultiUOM(s, e) {
    // Rev 1.0
    cbtn_SaveRecords_N.SetVisible(true);
    cbtn_SaveRecords_p.SetVisible(true);
    // End of Rev 1.0
    e.cancel = false;
    // cPopup_MultiUOM.Hide();
}


function OnMultiUOMEndCallback(s, e) {
    if (cgrid_MultiUOM.cpDuplicateAltUOM == "DuplicateAltUOM") {
        jAlert("Please Enter Different Alt. Quantity.");
        return;
    }
    // Mantis Issue 24428
    if (cgrid_MultiUOM.cpSetBaseQtyRateInGrid != null && cgrid_MultiUOM.cpSetBaseQtyRateInGrid == "1") {
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
        var BaseQty = cgrid_MultiUOM.cpBaseQty;
        var BaseRate = cgrid_MultiUOM.cpBaseRate;

        grid.GetEditor("Quantity").SetValue(BaseQty);
        grid.GetEditor("SalePrice").SetValue(BaseRate);
        //grid.GetEditor("Amount").SetValue(BaseQty * BaseRate)

        var Amount = BaseQty * BaseRate;
        grid.GetEditor("Amount").SetValue(DecimalRoundoff(Amount, 2));

        grid.GetEditor("Order_AltQuantity").SetValue(cgrid_MultiUOM.cpAltQty);
        grid.GetEditor("Order_AltUOM").SetValue(cgrid_MultiUOM.cpAltUom);
        // Rev Sanchita
        spLostFocus(null, null);
        // End of Rev Sanchita
        // Rev 1.0
        cPopup_MultiUOM.Hide();  // closeMultiUOM() IS CALLED FROM WHERE SAVE BUTTONS AGAIN BECOMES VISIBLE
        // End of Rev 1.0
        
    }

    if (cgrid_MultiUOM.cpAllDetails == "EditData") {
        var Quan = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
        $('#UOMQuantity').val(Quan);
        //$('#UOMQuantity').val(cgrid_MultiUOM.cpBaseQty);
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

    }

    // End of Mantis Issue 24428
    if (cgrid_MultiUOM.cpOpenFocus == "OpenFocus") {
        ccmbSecondUOM.SetFocus();
    }

}


function OnWarehouseEndCallback(s, e) {
    var Ptype = document.getElementById('hdfProductType').value;

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 5);
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
    grid.batchEditApi.StartEdit(globalRowIndex, 11);
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

function acpAvailableStockEndCall(s, e) {
    if (cacpAvailableStock.cpstock != null) {
        divAvailableStk.style.display = "block";
        divpopupAvailableStock.style.display = "block";

        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStk').innerHTML = AvlStk;
        document.getElementById('lblAvailableStock').innerHTML = cacpAvailableStock.cpstock;
        document.getElementById('lblAvailableStockUOM').innerHTML = document.getElementById('lblStkUOM').innerHTML;

        cCmbWarehouse.cpstock = null;
    }
}
function ctaxUpdatePanelEndCall(s, e) {
    if (ctaxUpdatePanel.cpstock != null) {
        divAvailableStk.style.display = "block";
        divpopupAvailableStock.style.display = "block";

        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStk').innerHTML = AvlStk;
        document.getElementById('lblAvailableStock').innerHTML = ctaxUpdatePanel.cpstock;
        document.getElementById('lblAvailableStockUOM').innerHTML = document.getElementById('lblStkUOM').innerHTML;

        ctaxUpdatePanel.cpstock = null;
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
        return false;
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

    ctxtProductAmount.SetValue(parseFloat(sumAmount).toFixed(2));
    ctxtProductTaxAmount.SetValue(parseFloat(sumTaxAmount).toFixed(2));
    ctxtProductDiscount.SetValue(parseFloat(sumDiscount).toFixed(2));
    ctxtProductNetAmount.SetValue(parseFloat(sumNetAmount).toFixed(2));
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
                    ctxtProductAmount.SetText(parseFloat(sumAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdAmt').value = parseFloat(sumAmount / gstDis).toFixed(2);
                    clblChargesGSTforGross.SetText(parseFloat(sumAmount - parseFloat(document.getElementById('HdChargeProdAmt').value)).toFixed(2));
                    clblChargesTaxableGross.SetText("(Taxable)");

                }
                else {
                    $('.lblChargesGSTforGross').hide();
                    ctxtProductNetAmount.SetText(parseFloat(sumNetAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdNetAmt').value = parseFloat(sumNetAmount / gstDis).toFixed(2);
                    clblChargesGSTforNet.SetText(parseFloat(sumNetAmount - parseFloat(document.getElementById('HdChargeProdNetAmt').value)).toFixed(2));
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

        //###### Added By : Samrat Roy ##########
        //Get Customer Shipping StateCode
        var shippingStCode = '';
        //Chinmoy edited
        shippingStCode = ctxtshippingState.GetText();
        if (shippingStCode.trim() != "") {
            shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();
        }

        //// ###########  Old Code #####################
        //Get Customer Shipping StateCode
        //var shippingStCode = '';
        //if (cchkBilling.GetValue()) {
        //    shippingStCode = CmbState.GetText();
        //}
        //else {
        //    shippingStCode = CmbState1.GetText();
        //}
        //shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

        //###### END : Samrat Roy : END ########## 

        //Debjyoti 09032017
        if (shippingStCode.trim() != '') {
            for (var cmbCount = 1; cmbCount < ccmbGstCstVatcharge.GetItemCount() ; cmbCount++) {
                //Check if gstin is blank then delete all tax
                if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[5] != "") {
                    if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {
                        if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "35" || shippingStCode == "31" || shippingStCode == "34") {
                            if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'S') {
                                ccmbGstCstVatcharge.RemoveItem(cmbCount);
                                cmbCount--;
                            }
                        }
                        else {
                            if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                ccmbGstCstVatcharge.RemoveItem(cmbCount);
                                cmbCount--;
                            }
                        }
                    } else {
                        if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'U') {
                            ccmbGstCstVatcharge.RemoveItem(cmbCount);
                            cmbCount--;
                        }
                    }
                } else {
                    //remove tax because GSTIN is not define
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

//Set Running Total for Charges And Tax 
function SetChargesRunningTotal() {

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


function AutoPopulateMultiUOM() {

    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    $.ajax({
        type: "POST",
        url: "SalesQuotation.aspx/AutoPopulateAltQuantity",
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
                ccmbSecondUOM.SetValue('');
                //$("#AltUOMQuantity").val(calcQuantity);

                //cAltUOMQuantity.SetValue(calcQuantity);
                cAltUOMQuantity.SetValue("0.0000");
            }
            else {
                if (AltUOMId == 0) {
                    ccmbSecondUOM.SetValue('');
                }
                else {
                    ccmbSecondUOM.SetValue(AltUOMId);
                }
                // Rev Sanchita
                //cAltUOMQuantity.SetValue(calcQuantity);
                // End of Rev Sanchita
            }

        }
    });
}


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
        url: "SalesQuotation.aspx/GetPackingQuantity",
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
            cAltUOMQuantity.SetValue(calcQuantity);

        }
    });
}


function SaveMultiUOM() {

   // debugger;
    //grid.GetEditor('ProductID').GetText().split("||@||")[3];

    // Rev 1.0
    document.getElementById('lblInfoMsg').innerHTML = "";

    if ($("#UOMQuantity").val() != 0 || cAltUOMQuantity.GetValue() != 0) {
        LoadingPanelMultiUOM.Show();
        setTimeout(() => {
            LoadingPanelMultiUOM.Hide();

        }, 1000)
    }
    // End of Rev 1.0

    var qnty = $("#UOMQuantity").val();


    var UomId = ccmbUOM.GetValue();
    //var UomId = ccmbUOM.SetSelectedIndex(grid.GetEditor('ProductID').GetText().split("||@||")[3] - 1);
    var UomName = ccmbUOM.GetText();
    //var AltQnty = parseFloat($("#AltUOMQuantity").val()).toFixed(4);
    var AltQnty = cAltUOMQuantity.GetValue();
    var AltUomId = ccmbSecondUOM.GetValue();
    var AltUomName = ccmbSecondUOM.GetText();
    // Rev Sanchita
    grid.batchEditApi.StartEdit(globalRowIndex);
    // End of Rev Sanchita
    var srlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var DetailsId = grid.GetEditor('DetailsId').GetText();

    if (ProductID == "") {
        ProductID = hdProductID.value;
    }
    if (DetailsId == "")
    {
        DetailsId = "0";
    }
    // Mantis Issue 24428
    var BaseRate = ccmbBaseRate.GetValue();
    var AltRate = ccmbAltRate.GetValue();

    var UpdateRow = 'False';
    if ($("#chkUpdateRow").prop("checked")) {
        UpdateRow = 'True';
    }
    // End of Mantis Issue 24428
    // Rev Sanchita
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != ""
    //       && BaseRate != "0.0000" && AltRate != "0.0000") {
    if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty != "0.0000") {
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
            // End of Rev Sanchita
        // Mantis Issue 24428
        if (cbtnMUltiUOM.GetText() == "Update") {
            cgrid_MultiUOM.PerformCallback('UpdateRow~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID+ '~' + DetailsId  + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow +'~' + hdMultiUOMID);
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

            // End of Mantis Issue 24428

            // Mantis Issue 24428
            //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "") {
      
            // End of Mantis Issue 24428
            // Mantis Issue 24428
            // cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID);
            cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow);
            // End of Mantis Issue 24428
            //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
            cAltUOMQuantity.SetValue("0.0000");
            // Mantis Issue 24428
            $("#UOMQuantity").val(0);
            ccmbBaseRate.SetValue(0)
            cAltUOMQuantity.SetValue(0)
            ccmbAltRate.SetValue(0)
            ccmbSecondUOM.SetValue("")
            // Rev Sanchita
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
            // End of Rev Sanchita
            // End of Mantis Issue 24428
            }
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



    if ($("#hdnPageStatForApprove").val() == "QN" && $("#hdnApprovalsettings").val() == "1") {
        document.getElementById("dvReject").style.display = "inline-block";
        document.getElementById("dvApprove").style.display = "inline-block";
        document.getElementById("dvAppRejRemarks").style.display = "block";
    }
    if ($("#hdnPageStatForApprove").val() == "QN" && $("#hdnApprovalsettings").val() == "1") {
        var det = {};
        det.OrderId = $("#hdnEditOrderId").val();
        if ($("#hdnEditOrderId").val() != "") {
            $.ajax({
                type: "POST",
                url: "SalesQuotation.aspx/GetApproveRejectStatus",
                data: JSON.stringify(det),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var statusValueforApproval = msg.d;
                    if (statusValueforApproval == 1) {
                        document.getElementById("dvRevisionDate").style.display = "block";
                        document.getElementById("dvRevision").style.display = "block";
                        document.getElementById("dvAppRejRemarks").style.display = "block";
                        document.getElementById("dvReject").style.display = "none";
                        document.getElementById("dvApprove").style.display = "none";
                      
                    }
                    else if (statusValueforApproval == 2) {
                        document.getElementById("dvAppRejRemarks").style.display = "block";
                        document.getElementById("dvReject").style.display = "none";
                        document.getElementById("dvApprove").style.display = "inline-block";
                    }
                    else if (statusValueforApproval == 0) {
                        document.getElementById("dvAppRejRemarks").style.display = "block";
                        document.getElementById("dvReject").style.display = "inline-block";
                        document.getElementById("dvApprove").style.display = "inline-block";
                    }
                }
            });
        }
    }
    if ($("#hdnPageStatus").val() == "update" && $("#hdnApprovalsettings").val() == "1") {
        var detApp = {};
        detApp.OrderId = $("#hdnEditOrderId").val();
        $.ajax({
            type: "POST",
            url: "SalesQuotation.aspx/GetApproveRejectStatus",
            data: JSON.stringify(detApp),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                statusValueforRejectApproval = msg.d;
                if (statusValueforRejectApproval == 1) {
                    document.getElementById("dvRevisionDate").style.display = "block";
                    document.getElementById("dvRevision").style.display = "block";
                    //ctxtRevisionDate.SetMinDate(cPLSalesOrderDate.GetDate());	
                    //ctxtRevisionDate.SetMinDate(new Date(cPLQuoteDate.GetDate().toDateString()))	
                }
            }
        });
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
    var taxtype = cddl_AmountAre.GetValue();
    if (getUrlVars().key != "ADD") {
        if (taxtype == '3') {
            grid.GetEditor('TaxAmount').SetEnabled(false);
        }
    }

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
            jAlert("This Serial Number does not exists.", "Alert", function () { ctxtserial.Focus(); });
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

function Delete_MultiUom(keyValue, SrlNo, DetailsId) {


    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo + '~' + DetailsId);

}

// Mantis Issue 24428
function Edit_MultiUom(keyValue, SrlNo) {

    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);

}
// End of Mantis Issue 24428




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

    var ItemCount = GetSelectedItemsCount(selectedItems);
    checkComboBox.SetText(ItemCount + " Items");

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

    //if (ProductID != "0") {
    //   cacpAvailableStock.PerformCallback(strProductID);
    //}
}
function ProductsGotFocusFromID(s, e) {
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

function QuantityGotFocus(s, e) {

    //Surojit 15-02-2019


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
    var QuotationNum = (grid.GetEditor('Quotation_Num').GetText() != null) ? grid.GetEditor('Quotation_Num').GetText() : "0";

    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';
    var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
    var gridPackingQty = '';
    // REV Start 0024380: Alternate UOM is activated : Sales : Quotation - Alternate UOM is activated.
    //var gridPackingQty = grid.GetEditor('QuoteDetails_PackingQty').GetText();
    if (SpliteDetails.length > 27) {
        if (SpliteDetails[27] == "1") {
            IsInventory = 'Yes';

            type = 'edit';

            if (SpliteDetails[28] != '') {
                //if (parseFloat(SpliteDetails[28]) > 0) {
                    
                    gridPackingQty = SpliteDetails[29];
                //}
            }
        }
    }
    // // REV End 0024380: Alternate UOM is activated : Sales : Quotation -Alternate UOM is activated.

    //if ($("#hdBasketId").val() != "")
    //{
    //    gridPackingQty = $("#hdnUomqnty").val();
    //}
    //else
    //{
    //    gridPackingQty = SpliteDetails[28];
    //}
    //for Multiuom chinu
    if ($("#hddnMultiUOMSelection").val() == "0") {
        if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 1) {
            ShowUOM(type, "SalesQuotation", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
        }
    }
    //Surojit 15-02-2019
}




var issavePacking = 0;

function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {

    if ($("#hddnMultiUOMSelection").val() == "0") {
        issavePacking = 1;
        grid.batchEditApi.StartEdit(globalRowIndex);
        grid.GetEditor('Quantity').SetValue(Quantity);


        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 6);
        }, 600)

        QuantityTextChange(null, null);
    }

    else {
        issavePacking = 1;
        grid.batchEditApi.StartEdit(globalRowIndex);


        var qnty = grid.GetEditor('Quantity').GetValue();
        var SLNo = grid.GetEditor('SrlNo').GetValue();

        if (qnty != "0.0000" && Quantity != qnty) {

            jConfirm('Qunatity Change Will Clear Multiple UOM Details.?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.batchEditApi.StartEdit(globalRowIndex);
                    var tbqty = grid.GetEditor('Quantity');
                    tbqty.SetValue(Quantity);
                    cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo);

                    setTimeout(function () {
                        grid.batchEditApi.StartEdit(globalRowIndex, 6);
                    }, 600)
                }
                else {
                    setTimeout(function () {
                        grid.batchEditApi.StartEdit(globalRowIndex, 5);
                    }, 200);
                }


            });
        }
        else {
            grid.GetEditor('Quantity').SetValue(Quantity);
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
            }, 600)
        }
    }
}
function SetFoucs() {
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

document.onkeydown = function (e) {
    if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        StopDefaultAction(e);


        btnSave_QuoteAddress();
        // document.getElementById('Button3').click();

        // return false;
    }

    if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        StopDefaultAction(e);


        page.SetActiveTabIndex(0);
        gridLookup.Focus();
        // document.getElementById('Button3').click();

        // return false;
    }
}
function SettingTabStatus() {
    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
    }
}

function disp_prompt(name) {

    if (name == "tab0") {
        // gridLookup.Focus();
        ctxtCustName.Focus()
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
        }
    }
}

function ProductKeyDown(s, e) {
    //console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {

        s.OnButtonClick(0);
    }
    if (e.htmlEvent.key == "NumpadEnter") {

        s.OnButtonClick(0);
    }
}

function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {
        setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
        $('#txtProdSearch').val('');
        $('#ProductModel').modal('show');
        //if (cproductLookUp.Clear()) {
        //    cProductpopUp.Show();
        //    cproductLookUp.Focus();
        //    cproductLookUp.ShowDropDown();
        //}
    }
}

//function ProductlookUpKeyDown(s, e) {
//    if (e.htmlEvent.key == "Escape") {
//        cProductpopUp.Hide();
//        grid.batchEditApi.StartEdit(globalRowIndex, 5);
//    }
//}

var IsInventory = '';

function SetProduct(Id, Name, e) {
    //Rev Subhra 01-04-2019 (Because it is no where used....for this getting error.)
    //IsInventory = e.parentElement.children[2].innerText;
    //End of Rev Subhra 01-04-2019

    //if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
    //    cProductpopUp.Hide();
    //    grid.batchEditApi.StartEdit(globalRowIndex, 5);
    //    return;
    //}
    //var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
    //var ProductCode = cproductLookUp.GetValue();
    //var focusedRow = cproductLookUp.gridView.GetFocusedRowIndex();
    //var ProductCode = cproductLookUp.gridView.GetRow(focusedRow).children[1].innerText;
    $('#ProductModel').modal('hide');
    var LookUpData = Id;
    var ProductCode = Name;




    if (!ProductCode) {
        LookUpData = null;
    }
    //console.log(LookUpData);
    ///cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);

    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);
    //Rev work start 06.07.2022 mantise no:25008
    if ($("#hdnPlaceShiptoParty").val() == "1") {
        cddl_PosGstSalesOrder.SetEnabled(true);
    }
    else {
        cddl_PosGstSalesOrder.SetEnabled(false);
    }
    PopulatePosGst();
    //Getshhippingval(PosGstId);
    //Rev work close 06.07.2022 mantise no:25008
    AllowAddressShipToPartyState = false;
    //BillShipAddressVisible();
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
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    GetSalesRateSchemePrice($("#hdnCustomerId").val(), strProductID, "0");

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

    document.getElementById("ddlInventory").disabled = true;

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

   

    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    grid.batchEditApi.StartEdit(globalRowIndex, 5);
    setTimeout(function () {
        if ($("#ProductMinPrice").val() != "") {
            grid.GetEditor("SalePrice").SetValue($("#ProductMinPrice").val());
        }
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }, 200);
}
$(document).ready(function () {
    $("#Cross_CloseWindow a").click(function (e) {
        e.preventDefault();
        window.close();
    });
});

function ddlInventory_OnChange() {
    //cproductLookUp.GetGridView().Refresh();
}

function CustomerButnClick(s, e) {
    $('#CustModel').modal('show');
}

function CustomerKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#CustModel').modal('show');
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

        if ($("#txtCustSearch").val() != "") {
            callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}



function ValueSelected(e, indexName) {
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
            else if (indexName == "BillingAreaIndex") {
                SetBillingArea(Id, name);
            }
            else if (indexName == "ShippingAreaIndex") {
                SetShippingArea(Id, name);
            }
            else if (indexName == "customeraddressIndex") {
                SetCustomeraddress(Id, name);
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
            else if (indexName == "BillingAreaIndex")
                $('#txtbillingArea').focus();
            else if (indexName == "ShippingAreaIndex")
                $('#txtshippingArea').focus();
            else if (indexName == "customeraddressIndex")
                ('#txtshippingShipToParty').focus();
            else
                $('#txtCustSearch').focus();
        }
    }

}


function SetCustomer(Id, Name) {
    if (Id) {
        $('#CustModel').modal('hide');
        ctxtCustName.SetText(Name);
        //Rev work start 06.07.2022 mantise no:25008
        cddl_PosGstSalesOrder.ClearItems();
        cddl_PosGstSalesOrder.SetEnabled(true);
        //SetDefaultBillingShippingAddress(Id);
        //Rev work close 06.07.2022 mantise no:25008
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        //Rev work start 06.07.2022 mantise no:25008
        PosGstId = "";
        cddl_PosGstSalesOrder.SetValue(PosGstId);
        //SetDefaultBillingShippingAddress(Id);
        //Rev work close 06.07.2022 mantise no:25008
        //var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : ""; //Abhisek
        var startDate = new Date();
        startDate = tstartdate.GetValueString();

        //if (type != "") { //Abhisek
        //    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + Id + '~' + startDate + '~' + '%' + '~' + type); //Abhisek
        //} //Abhisek



        GetObjectID('hdnCustomerId').value = Id;
        GetObjectID('hdnAddressDtl').value = '0';

        //page.SetActiveTabIndex(1);
        //loadAddressbyCustomerID(Id);


        GetContactPerson(Id);
        $('.dxeErrorCellSys').addClass('abc');
        $('.crossBtn').hide();

        SalesmanBindWRTCustomer(Id);
        SetEntityType(Id);
        //END REV RAJDIP
        page.GetTabByName('General').SetEnabled(false);
        $('#CustModel').modal('hide');

    }
}
//Rev Rajdip For Customer Map To SalesMan
function SalesmanBindWRTCustomer(Id) {

    $.ajax({
        type: "POST",
        url: "SalesQuotation.aspx/MappedSalesManOnetoOne",
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

function loadAddressbyCustomerID(customerId) {

    var OtherDetails = {}
    OtherDetails.CustomerId = customerId;
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetCustomerAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;
            GlobalAllAddress = returnObject;
            var BillingObj = $.grep(returnObject, function (e) { return e.Type == "Billing" && e.deflt == 1; })
            var ShippingObj = $.grep(returnObject, function (e) { return e.Type == "Shipping" && e.deflt == 1; })

            //Set Billing
            if (BillingObj.length > 0) {

                ctxtAddress1.SetText(BillingObj[0].add_address1);
                ctxtAddress2.SetText(BillingObj[0].add_address2);
                ctxtAddress3.SetText(BillingObj[0].add_address3);
                $('#hdBillingPin').val(BillingObj[0].pnId);
                ctxtbillingPin.SetText(BillingObj[0].pnCd);
                $('#lblBillingCountry').text(BillingObj[0].conName);
                $('#lblBillingCountryValue').val(BillingObj[0].couId);
                $('#lblBillingState').text(BillingObj[0].stName);
                $('#lblBillingStateText').text(BillingObj[0].stName);
                $('#lblBillingStateValue').val(BillingObj[0].stId);
                $('#lblBillingCity').text(BillingObj[0].ctyName);
                $('#lblBillingCityValue').val(BillingObj[0].ctyId);
                ctxtlandmark.SetText(BillingObj[0].landMk);
            } else {
                ctxtAddress1.SetText('');
                ctxtAddress2.SetText('');
                ctxtAddress3.SetText('');
                $('#hdBillingPin').val('');
                ctxtbillingPin.SetText('');
                $('#lblBillingCountry').text('');
                $('#lblBillingCountryValue').val('');
                $('#lblBillingState').text('');
                $('#lblBillingStateText').val('');
                $('#lblBillingStateValue').val('');
                $('#lblBillingCity').text('');
                $('#lblBillingCityValue').val('');
                ctxtlandmark.SetText('');
            }

            //Set Shipping
            if (ShippingObj.length > 0) {
                ctxtsAddress1.SetText(ShippingObj[0].add_address1);
                ctxtsAddress2.SetText(ShippingObj[0].add_address2);
                ctxtsAddress3.SetText(ShippingObj[0].add_address3);
                $('#hdShippingPin').val(ShippingObj[0].pnId);
                ctxtShippingPin.SetText(ShippingObj[0].pnCd);
                $('#lblShippingCountry').text(ShippingObj[0].conName);
                $('#lblShippingCountryValue').val(ShippingObj[0].couId);
                $('#lblShippingState').text(ShippingObj[0].stName);
                $('#lblShippingStateText').val(ShippingObj[0].stName);;
                $('#lblShippingStateValue').val(ShippingObj[0].stId);
                $('#lblShippingCity').text(ShippingObj[0].ctyName);
                $('#lblShippingCityValue').val(ShippingObj[0].ctyId);
                ctxtslandmark.SetText(ShippingObj[0].landMk);

            } else {
                ctxtsAddress1.SetText('');
                ctxtsAddress2.SetText('');
                ctxtsAddress3.SetText('');
                $('#hdShippingPin').val('');
                ctxtShippingPin.SetText('');
                $('#lblShippingCountry').text('');
                $('#lblShippingCountryValue').val('');
                $('#lblShippingState').text('');
                $('#lblShippingStateText').val('');
                $('#lblShippingStateValue').val('');
                $('#lblShippingCity').text('');
                $('#lblShippingCityValue').val('');
                ctxtslandmark.SetText('');
            }


            if (BillingObj.length == 0)
                txtSelectBillingAdd.Focus();
            else if (ShippingObj.length == 0)
                ctxtSelectShippingAdd.Focus();

        }
    });
}

function prodkeydown(e) {
    //Both-->B;Inventory Item-->Y;Capital Goods-->C
    var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    OtherDetails.InventoryType = inventoryType;

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Name");
        //HeaderCaption.push("Product Description");
        // Mantis Issue 24375
        HeaderCaption.push("Product Description");
        // End of Mantis Issue 24375
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");

        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetSalesProductForQuotation", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
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

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Name");
        if ($("#txtSalesManSearch").val() != null && $("#txtSalesManSearch").val() != "") {


            //Rev Rajdip
            //callonServer("Services/Master.asmx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "OnFocus");
            callonServer("SalesQuotation.aspx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "OnFocus");

            //End Rev rajdip
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[salesmanIndex=0]"))
            $("input[salesmanIndex=0]").focus();
    }
}

function OnFocus(Id, Name) {

    $("#hdnSalesManAgentId").val(Id);


    ctxtSalesManAgent.SetText(Name);
    $('#SalesManModel').modal('hide');
    ctxt_Refference.Focus();
}

var canCallBack = true;
function AllControlInitilize() {
    if (canCallBack) {
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        grid.batchEditApi.EndEdit();
        $('#ddlInventory').focus();
        canCallBack = false;
        //Rev work start 06.07.2022 mantise no:25008
        PopulatePosGst();
        //Rev work close 06.07.2022 mantise no:25008
        LoadtBillingShippingCustomerAddress($('#hdnCustomerId').val());
        LoadtBillingShippingShipTopartyAddress();
        //Rev work start 06.07.2022 mantise no:25008
        //if ($('#hdnPageStatus').val() == "update") {
        if ($('#hdnPageStatus').val() == "update" || $('#hdnQuteMode').val() == "Copy") {
            // BillShipAddressVisible();
            AllowAddressShipToPartyState = false;
            //Rev work start 06.07.2022 mantise no:25008
            if ($("#hdnPlaceShiptoParty").val() == "1") {
                cddl_PosGstSalesOrder.SetEnabled(true);
            }
            else {
                cddl_PosGstSalesOrder.SetEnabled(false);
            }
            //Rev work close 06.07.2022 mantise no:25008
        }       
        PopulateGSTCSTVAT();

    }
}
var SimilarProjectStatus = "0";
function CloseGridQuotationLookup() {
    //
    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();
    //gridquotationLookup.Focus();
    // txt_OANumber.focus();
    if (gridquotationLookup.GetValue() == null) {
        $('input[type=radio]').prop('checked', false);
        grid.PerformCallback('GridBlank');
        ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        gridquotationLookup.SetEnabled(false);
    }



    var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

    if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {
        //debugger;
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


        $.ajax({
            type: "POST",
            url: "SalesQuotation.aspx/DocWiseSimilarProjectCheck",
            data: JSON.stringify({ quote_Id: quote_Id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                SimilarProjectStatus = msg.d;
                if (SimilarProjectStatus != "1") {
                    cPLQADate.SetText("");
                    clookup_Project.Clear();
                    jAlert("Unable to procceed. Project are for the selected Inquiry(s) are different.");
                    return false;

                }
            }
        });
    }
}


function selectValue() {
    //
    var IsInventory = $("#ddlInventory").val();

    var checked = $('#rdl_Salesquotation').attr('checked', true);
    if (checked) {
        //$(this).attr('checked', false);
        gridquotationLookup.SetEnabled(true);
    }
    else {
        $(this).attr('checked', true);
    }
    var startDate = new Date();
    startDate = tstartdate.GetValueString();
    var key = $("#hdnCustomerId").val();
    var type = ($("[id$='rdl_Salesquotation']").find(":checked").val() != null) ? $("[id$='rdl_Salesquotation']").find(":checked").val() : "";

    if (key == null || key == "") {
        jAlert("Customer required !", 'Alert Dialog: [Quoation]', function (r) {
            if (r == true) {
                ctxtCustName.Focus();
                gridquotationLookup.SetEnabled(false);
                $('input[type=radio]').prop('checked', false);
            }
        });

        return;

    }
    TaggingCall = true;
    if (key != null && key != '' && type != "") {
        cQuotationComponentPanel.PerformCallback('BindQuotationGrid' + '~' + key + '~' + startDate + '~' + '%');
    }




    //var componentType = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
    //if (componentType != null && componentType != '') {
    //    grid.PerformCallback('GridBlank');
    //}
}

function QuotationNumberChanged() {
    //
    //gridquotationLookup.GetValue()
    //grid.PerformCallback('BindGridOnQuotation' + '~' + cddl_Quotatione.GetValue() + '~' + ctxt_SlOrderNo.GetValue());
    //var quote_Id = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

    var quote_Id = gridquotationLookup.GetValue();
    if (SimilarProjectStatus != "-1") {
        if (quote_Id != null) {
            var arr = quote_Id.split(',');

            if (arr.length > 1) {
                cPLQADate.SetText('Multiple Select Inquiry Dates');

            }
            else {
                if (arr.length == 1) {
                    cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);

                    // Rev 4.0
                    var type = "SINQ"
                    var Key = quote_Id.split(',')[0];
                    $.ajax({
                        type: "POST",
                        url: "SalesQuotation.aspx/GetRFQHeaderReference",
                        data: JSON.stringify({ KeyVal: Key, type: type }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {

                            var currentString = msg.d;
                            
                            var RFQNumber = currentString.split('~')[0];
                            var RFQDate = currentString.split('~')[1];
                            var ProjectSite = currentString.split('~')[2];

                            ctxtRFQNumber.SetText(RFQNumber);
                            if (RFQDate != "") {
                                cdtRFQDate.SetDate(new Date(RFQDate));
                            }
                            ctxtProjectSite.SetText(ProjectSite);

                            // Rev 5.0
                            var Quote_SalesmanId = currentString.split('~')[3];
                            var Quote_SalesmanName = currentString.split('~')[4];

                            $("#hdnSalesManAgentId").val(Quote_SalesmanId);
                            ctxtSalesManAgent.SetText(Quote_SalesmanName);
                            // End of Rev 5.0
                            
                        }

                    });
                    // End of Rev 4.0

                }
                else {
                    cPLQADate.SetText('');

                }
            }
            //cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
            //cProductsPopup.Show();

        }
        else { cPLQADate.SetText(''); }

        if (quote_Id != null) {
            cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
            cProductsPopup.Show();
        }
    }
}

function SetDifference1() {
    var diff = CheckDifferenceOfFromDateWithTodate();
}
function CheckDifferenceOfFromDateWithTodate() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = tstartdate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (startTime - endTime) / 86400000;

    }
    return difference;
}
function CheckDifference() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = tstartdate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;

    }
    return difference;

}

function validateOrderwithAmountAre() {
    //Check Multiple Row amount are selectedor not

    var selectedKeys = cgridproducts.GetSelectedKeysOnPage();
    var ammountsAreOrder = "";
    if (selectedKeys.length > 0) {
        for (var loopcount = 0 ; loopcount < cgridproducts.GetVisibleRowsOnPage() ; loopcount++) {

            var nowselectedKey = cgridproducts.GetRowKey(loopcount);

            var found = selectedKeys.find(function (element) {
                return element == nowselectedKey;
            });

            if (found) {
                if (ammountsAreOrder != "" && ammountsAreOrder != cgridproducts.GetRow(loopcount).children[9].innerText) {
                    jAlert("Unable to procceed. Tax Type are for the selected order(s) are different");
                    return false;
                }
                else
                    ammountsAreOrder = cgridproducts.GetRow(loopcount).children[9].innerText;
            }
        }
    }
    return true;
}

function ChangeState(value) {

    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}
function componentEndCallBack(s, e) {

    gridquotationLookup.gridView.Refresh();
    if (grid.GetVisibleRowsOnPage() == 0) {
        OnAddNewClick();

    }
    if (TaggingCall) {
        gridquotationLookup.Focus();
        TaggingCall = false;
    }
    else {
        //ctxt_OANumber.Focus();
    }
    if (cQuotationComponentPanel.cpTaggedTaxAmountType != "") {
        var value = cQuotationComponentPanel.cpTaggedTaxAmountType;
        cddl_AmountAre.SetValue(value);
        cddl_AmountAre.SetEnabled(false);
    }
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
        success: function (msg) {
            var returnObject = msg.d;
            console.log(returnObject);
            //if (returnObject[0].MinSalePrice <= SalesPrice && returnObject[0].MaxSalePrice >= SalesPrice) {

            //}
            //else {
            //    jAlert("Product Min price :" + returnObject[0].MinSalePrice + " and Max price :" + returnObject[0].MaxSalePrice,  "Alert", function () {
            //        grid.batchEditApi.StartEdit(globalRowIndex, 15);
            //        return;
            //    });

            //}
            if (returnObject.length > 0) {
                $("#ProductMinPrice").val(returnObject[0].MinSalePrice);
                $("#ProductMaxPrice").val(returnObject[0].MaxSalePrice);
                $("#hdnRateType").val(returnObject[0].RateType);
            }
        }
    });
}
