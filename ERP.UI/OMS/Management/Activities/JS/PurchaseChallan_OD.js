
        function ddlInventory_OnChange() {
            cproductLookUp.GetGridView().Refresh();
        }

function txtBillNo_TextChanged() {
    var VoucherNo = document.getElementById("txtVoucherNo").value;

    $.ajax({
        type: "POST",
        url: "PurchaseChallan_OD.aspx/CheckUniqueName",
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

function GetIndentREquiNo(e) {
    var PODate = new Date();
    PODate = cPLQuoteDate.GetValueString();
    ctaxUpdatePanel.PerformCallback('DeleteAllTax');
}

function GetContactPerson(e) {
    if (gridLookup.GetValue() != null) {
        var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
        if (key != null && key != '') {
            LoadCustomerAddress(key, $('#ddl_Branch').val(), 'PC');
            page.tabs[0].SetEnabled(true);
            page.tabs[1].SetEnabled(true);
            GetObjectID('hdnCustomerId').value = key;
            if ($('#hfBSAlertFlag').val() == "1") {
                jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        page.SetActiveTabIndex(1);
                    }
                });
            }
        }
    }
}

function cmbContactPersonEndCall(s, e) {
    cddl_AmountAre.SetEnabled(true);
    var comboitem = cddl_AmountAre.FindItemByValue('4');
    if (comboitem != undefined && comboitem != null) {
        cddl_AmountAre.RemoveItem(comboitem.index);
    }

    if (cContactPerson.cpGSTN == "No") {
        if (cContactPerson.cpcountry != null && cContactPerson.cpcountry != '') {
            if (cContactPerson.cpcountry != '1') {
                cddl_AmountAre.AddItem("Import", "4");
                cddl_AmountAre.SetValue(4);
                cddl_AmountAre.SetEnabled(false);
            }
            else {
                cddl_AmountAre.SetValue(1);
            }
        }
        else {
            cddl_AmountAre.SetValue(1);
        }
    }
    else {
        cddl_AmountAre.SetValue(1);
    }

    cContactPerson.cpGSTN = null;
    cContactPerson.cpcountry = null;
}

function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
}

function QuotationNumberChanged() {
    var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage(); //gridquotationLookup.GetValue();
    quote_Id = quote_Id.join();

    if (quote_Id != null) {
        var arr = quote_Id.split(',');
        if (arr.length > 1) {
            cPLQADate.SetText('Multiple Purchase Order Dates');
        }
        else {
            if (arr.length == 1) {
                var selectIndex = gridquotationLookup.gridView.GetFocusedRowIndex()
                var orderDate = gridquotationLookup.gridView.GetRow(selectIndex).children[2].innerText;
                cPLQADate.SetText(orderDate);
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
}

function componentEndCallBack(s, e) {
    gridquotationLookup.gridView.Refresh();
    if (grid.GetVisibleRowsOnPage() == 0) {
        OnAddNewClick();
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



function SetFocusonDemand(e) {
    grid.batchEditApi.StartEdit(-1, 2);
}
function CmbBranch_ValueChange() {
    var strBranch = document.getElementById('ddl_FromBranch').value;
    var _startDate = cPLQuoteDate.GetValue();
    var startDate = GetPCDateFormat(new Date(cPLQuoteDate.GetValue()));

    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + "" + '~' + startDate + '~' + strBranch);
}

function CmbScheme_ValueChange() {
    var val = $("#ddl_numberingScheme").val();

    var schemetypeValue = val;
    var schemetype = schemetypeValue.toString().split('~')[1];
    var schemelength = schemetypeValue.toString().split('~')[2];

    var branchID = (schemetypeValue.toString().split('~')[3] != null) ? schemetypeValue.toString().split('~')[3] : "";
    if (branchID != "") document.getElementById('ddl_Branch').value = branchID;

    $('#txtVoucherNo').attr('maxLength', schemelength);

    var schemetypeValue = val;
    var schemetype = schemetypeValue.toString().split('~')[1];
    var schemelength = schemetypeValue.toString().split('~')[2];

    var fromdate = schemetypeValue.toString().split('~')[4];
    var todate = schemetypeValue.toString().split('~')[5];

    var dt = new Date();

    cPLQuoteDate.SetDate(dt);

    if (dt < new Date(fromdate)) {
        cPLQuoteDate.SetDate(new Date(fromdate));
    }

    if (dt > new Date(todate)) {
        cPLQuoteDate.SetDate(new Date(todate));
    }




    cPLQuoteDate.SetMinDate(new Date(fromdate));
    cPLQuoteDate.SetMaxDate(new Date(todate));


    $('#txtVoucherNo').attr('maxLength', schemelength);
    if (schemetype == '0') {
        document.getElementById('txtVoucherNo').disabled = false;
        document.getElementById('txtVoucherNo').value = "";
        $("#txtVoucherNo").focus();

    }
    else if (schemetype == '1') {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "Auto";
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
    else {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "";
    }
}

//<%--Header Section End--%>

//<%--Billing/Shipping Section Start--%>

    function SettingTabStatus() {
        if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
            page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        }
    }

function disp_prompt(name) {
    if (name == "tab0") {
        gridLookup.Focus();
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

function GlobalBillingShippingEndCallBack() {
    if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
        cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";

        if (gridLookup.GetValue() != null) {
            var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
            if (key != null && key != '') {
                cContactPerson.PerformCallback('BindContactPerson~' + key);
            }
        }
    }
}

//<%--Billing/Shipping Section End--%>

//<%--Product Grid Section Start--%>

    var globalTaxRowIndex;

function GridCallBack() {
    grid.PerformCallback('Display');
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
        function ProductKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {
                s.OnButtonClick(0);
            }
        }

        if (cproductLookUp.Clear()) {
            Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

            cProductpopUp.Show();
            cproductLookUp.Focus();
            cproductLookUp.ShowDropDown();
        }
    }
}

function ProductsGotFocus(s, e) {
    pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

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
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        //divPacking.style.display = "block";
        divPacking.style.display = "none";
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strProductName);

    if (ProductID != "0") {
        // cacpAvailableStock.PerformCallback(strProductID);
    }
}

function ProductsGotFocusFromID(s, e) {
    pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
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
        divPacking.style.display = "none";
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strProductName);
}

function QuantityTextChange(s, e) {
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var ProductID = grid.GetEditor('gvColProduct').GetValue();
    var key = gridquotationLookup.GetValue();// gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

    if (parseFloat(QuantityValue) != parseFloat(Pre_Quantity)) {
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8]; //Packing_Factor
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();
            var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
            var strSalePrice = SpliteDetails[6];// purchase Price

            if (key != null && key != '') {
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
                    grid.GetEditor("gvColQuantity").SetValue(TotalQty);
                    var OrdeMsg = 'Balance Quantity of selected Product from tagged document. <br/>Cannot enter quantity more than balance quantity.';
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

            if (strRate == 0) {
                strRate = 1;
            }
            if (strSalePrice == 0.00000) {
                strSalePrice = 1;
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

            var IsPackingActive = SpliteDetails[13];//IsPackingActive
            var Packing_Factor = SpliteDetails[14];//Packing_Factor
            var Packing_UOM = SpliteDetails[15];//Packing_UOM
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#lblPackingStk').text(PackingValue);
                //divPacking.style.display = "block";
                divPacking.style.display = "none";
            } else {
                divPacking.style.display = "none";
            }



            var tbAmount = grid.GetEditor("gvColAmount");
            tbAmount.SetValue(Amount);

            var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
            tbTotalAmount.SetValue(Amount);

            DiscountTextChange(s, e);
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('gvColQuantity').SetValue('0');
            grid.GetEditor('gvColProduct').Focus();
        }
    }
}

function QuantityProductsGotFocus(s, e) {
    pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

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
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        divPacking.style.display = "none";
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strProductName);

    var editids = getUrlVars()["key"];
    if (ProductID != "0" && editids != "ADD") {
        //  cacpAvailableStock.PerformCallback(strProductID);
    }

    Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
}

function SalePriceTextFocus(s, e) {
    var Saleprice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
    _GetSalesPriceValue = Saleprice;

    Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
}

function SalePriceTextChange(s, e) {
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('gvColProduct').GetValue();

    if (ProductID != null) {
        if (parseFloat(Saleprice) == "0") {
            jConfirm('Are you sure to make this Amount as Zero(0) as the charges will also become Zero(0)?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    WorkOn_SalesPrice(s, e);
                    grid.batchEditApi.EndEdit();
                    grid.batchEditApi.StartEdit(globalRowIndex, 11);
                }
                else {
                    grid.batchEditApi.StartEdit(globalRowIndex, 10);

                    var gvColStockPurchasePrice = grid.GetEditor("gvColStockPurchasePrice");
                    gvColStockPurchasePrice.SetValue(_GetSalesPriceValue);
                    // grid.StartEditRow(globalRowIndex,2)
                    WorkOn_SalesPrice(s, e);

                    setTimeout(function () {
                        grid.batchEditApi.EndEdit();
                        //grid.batchEditApi.StartEdit(globalRowIndex, 11);
                    }, 500);
                }
            });
        }
        else {
            WorkOn_SalesPrice(s, e);
        }
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('SalePrice').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
}

function WorkOn_SalesPrice(s, e) {
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('gvColProduct').GetValue();

    var SpliteDetails = ProductID.split("||@||");
    var strMultiplier = SpliteDetails[7];
    var strFactor = SpliteDetails[8];
    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    //var strRate = "1";
    var strStkUOM = SpliteDetails[4];
    //var strSalePrice = SpliteDetails[6];

    var strProductID = SpliteDetails[0];
    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    if (strRate == 0) {
        strRate = 1;
    }

    var StockQuantity = strMultiplier * QuantityValue;
    var Discount = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";

    var Amount = QuantityValue * strFactor * (Saleprice / strRate);
    var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

    var tbAmount = grid.GetEditor("gvColAmount");
    tbAmount.SetValue(amountAfterDiscount);

    var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
    tbTotalAmount.SetValue(amountAfterDiscount);

    $('#lblProduct').text(strProductName);

    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        //divPacking.style.display = "block";
        divPacking.style.display = "none";
    } else {
        divPacking.style.display = "none";
    }

    DiscountTextChange(s, e);
}

function DiscountTextFocus() {
    Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
}

function DiscountTextChange(s, e) {
    var Discount = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var ProductID = grid.GetEditor('gvColProduct').GetValue();

    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strFactor = SpliteDetails[8];
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
        if (strSalePrice == '0') {
            strSalePrice = SpliteDetails[6];
        }
        if (strRate == 0) {
            strRate = 1;
        }
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
        var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        var tbAmount = grid.GetEditor("gvColAmount");
        tbAmount.SetValue(amountAfterDiscount);

        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
            //divPacking.style.display = "block";
            divPacking.style.display = "none";
        } else {
            divPacking.style.display = "none";
        }

        var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
        tbTotalAmount.SetValue(amountAfterDiscount);


        var ShippingStateCode = $("#bsSCmbStateHF").val();
        var TaxType = "";
        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }

        caluculateAndSetGST(grid.GetEditor("gvColAmount"), grid.GetEditor("gvColTaxAmount"), grid.GetEditor("gvColTotalAmountINR"), SpliteDetails[19], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), 'P');

        Cur_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
        CalculateAmount();
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('gvColDiscount').SetValue('0');
        grid.GetEditor('gvColProduct').Focus();
    }

    if (parseFloat(Cur_TotalAmt) != parseFloat(Pre_TotalAmt)) {
        ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
    }
}

function AmountTextFocus(s, e) {
    var Amount = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
    _GetAmountValue = Amount;

    Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
}

function AmountTextChange(s, e) {
    var Amount = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
    var TaxAmount = (grid.GetEditor('gvColTaxAmount').GetValue() != null) ? grid.GetEditor('gvColTaxAmount').GetValue() : "0";
    var ProductID = grid.GetEditor('gvColProduct').GetValue();
    var SpliteDetails = ProductID.split("||@||");

    if (parseFloat(_GetAmountValue) != parseFloat(Amount)) {
        var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
        tbTotalAmount.SetValue(Amount + TaxAmount);

        var ShippingStateCode = $("#bsSCmbStateHF").val();
        var TaxType = "";
        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }

        caluculateAndSetGST(grid.GetEditor("gvColAmount"), grid.GetEditor("gvColTaxAmount"), grid.GetEditor("gvColTotalAmountINR"), SpliteDetails[19], Amount, Amount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), 'P');

        Cur_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
        CalculateAmount();
    }
}

function taxAmtButnClick1(s, e) {
    console.log(grid.GetFocusedRowIndex());
    rowEditCtrl = s;
}

function taxAmtButnClick(s, e) {
    if (e.buttonIndex == 0) {

        if (cddl_AmountAre.GetValue() != null) {
            var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";

            if (ProductID.trim() != "") {
                Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
                Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

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

                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = parseFloat(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                clblTaxProdGrossAmt.SetText(Amount);
                clblProdNetAmt.SetText(parseFloat(grid.GetEditor('gvColAmount').GetValue()).toFixed(2));
                document.getElementById('HdProdGrossAmt').value = Amount;
                document.getElementById('HdProdNetAmt').value = parseFloat(grid.GetEditor('gvColAmount').GetValue()).toFixed(2);

                //End Here

                //Set Discount Here
                if (parseFloat(grid.GetEditor('gvColDiscount').GetValue()) > 0) {
                    var discount = (parseFloat(Amount * grid.GetEditor('gvColDiscount').GetValue() / 100)).toFixed(2);
                    clblTaxDiscount.SetText(discount);
                }
                else {
                    clblTaxDiscount.SetText('0.00');
                }
                //End Here 


                //Checking is gstcstvat will be hidden or not
                if (cddl_AmountAre.GetValue() == "2") {
                    $('.GstCstvatClass').hide();
                    clblTaxableGross.SetText("(Taxable)");
                    clblTaxableNet.SetText("(Taxable)");

                    $('.gstGrossAmount').hide();
                    $('.gstNetAmount').hide();
                    clblTaxableGross.SetText("");
                    clblTaxableNet.SetText("");

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
                    shippingStCode = cbsSCmbState.GetText();
                    shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();






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
                grid.batchEditApi.StartEdit(globalRowIndex, 14);
            }
        }
    }
}

function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function OnEndCallback(s, e) {
    var value = document.getElementById('hdnRefreshType').value;
    var pageStatus = document.getElementById('hdnPageStatus').value;

    if (grid.cpinserterrorwarehouse != null) {
        LoadingPanel.Hide();
        grid.batchEditApi.StartEdit(0, 2);
        jAlert(grid.cpinserterrorwarehouse);
        grid.cpinserterrorwarehouse = null;
    }
    else if (grid.cpSaveSuccessOrFail == "outrange") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Can Not Add More Purchase Oder Number as Purchase Order Scheme Exausted.<br />Update The Scheme and Try Again');
    }
    else if (grid.cpSaveSuccessOrFail == "checkPartyInvoice") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Party Invoice must be unique for the selected Vendor.');
    }
    else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Please fill Quantity');
    }
    else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Can not Duplicate Product in the Challan List.');
    }
    else if (grid.cpSaveSuccessOrFail == "nullPurchasePrice") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Purchase Price is Mandatory. Please enter values.');
    }
    else if (grid.cpSaveSuccessOrFail == "duplicateSerial") {
        LoadingPanel.Hide();
        var Msg = grid.cpduplicateSerialMsg;

        grid.cpSaveSuccessOrFail = null;
        grid.cpduplicateSerialMsg = null;
        grid.batchEditApi.StartEdit(0, 2);

        jAlert(Msg);
    }
    else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
        LoadingPanel.Hide();
        grid.batchEditApi.StartEdit(0, 2);
        var SrlNo = grid.cpProductSrlIDCheck;

        grid.cpSaveSuccessOrFail = null;
        grid.cpProductSrlIDCheck = null;
        //var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
        var msg = "Make sure product quantity are equal with Warehouse quantity for SL No. " + SrlNo;
        jAlert(msg);
    }
    else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
    }
    else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
    }
    else if (grid.cpSaveSuccessOrFail == "duplicate") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Can Not Save as Duplicate Purchase Order Numbe No. Found');
    }
    else if (grid.cpSaveSuccessOrFail == "errorInsert") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Please try after sometime.');
    }
    else if (grid.cpSaveSuccessOrFail == "transactionbeingused") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Transaction exist. cannot be processed.');
    }
    else if (grid.cpSaveSuccessOrFail == "Ponotexist") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Cannot Save. Selected Purchase   Order(s) in this document do not exist.');
    }
    else if (grid.cpSaveSuccessOrFail == "stockOut") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 3);
        jAlert('Already stock out for this product.');
    }
    else if (grid.cpSaveSuccessOrFail == "allStockOut") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;

        jAlert("Already stock out for selected products.", 'Alert Dialog: [PurchaseChallan]', function (r) {
            if (r == true) {
                window.location.reload();
            }
        });
    }
    else {

        var PurchaseOrder_Number = grid.cpPurchaseOrderNo;
        var Order_Msg = "GRN No. " + PurchaseOrder_Number + " saved.";
        if (value == "E") {
            //window.location.assign("PurchaseChallanList.aspx");
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
                    if (r == true) {
                        grid.cpSalesOrderNo = null;
                        window.location.assign("PurchaseChallanList_OD.aspx");
                    }
                });

            }
            else {
                window.location.assign("PurchaseChallanList_OD.aspx");
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
                        window.location.assign("PurchaseChallanList_OD.aspx?key=ADD");
                    }
                });


            }
            else {
                window.location.assign("PurchaseChallan.aspx?key=ADD");
            }
        }
        else {
            if (pageStatus == "first") {
                OnAddNewClick();
                grid.batchEditApi.EndEdit();
                //document.getElementById("<%=ddl_numberingScheme").focus();
                $('#<%=hdnPageStatus').val('');
            }
            else if (pageStatus == "update") {

                OnAddNewClick();
                $('#<%=hdnPageStatus').val('');
            }
        }

        var taxType = cddl_AmountAre.GetValue();
        if (taxType == 3) {
            grid.GetEditor('gvColTaxAmount').SetEnabled(false);
        }

        if (gridquotationLookup.GetValue() != null) {
            grid.GetEditor('ProductName').SetEnabled(false);
            grid.GetEditor('gvColDiscription').SetEnabled(false);
            grid.GetEditor('gvColQuantity').SetEnabled(false);
            $('#<%=IsPOTagged').val('true');
        }
        else {
            if (grid.cpComponent) {
                if (grid.cpComponent == 'true') {
                    grid.cpComponent = null;
                    OnAddNewClick();
                }
            }
            else {
                grid.StartEditRow(0);
                $('#<%=hdnPageStatus').val('');
            }
        }
    }

    if (grid.cpPurchaseorderbindnewrow == "yes") {
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
    }

    if (grid.cpOrderRunningBalance) {
        var RunningBalance = grid.cpOrderRunningBalance;
        var RunningSpliteDetails = RunningBalance.split("~");
        grid.cpOrderRunningBalance = null;

        var SUM_ChargesAmount = RunningSpliteDetails[0];
        var SUM_Amount = RunningSpliteDetails[1];
        //var SUM_ChargesAmount = RunningSpliteDetails[2];
        var SUM_TaxAmount = RunningSpliteDetails[3];
        var SUM_TotalAmount = RunningSpliteDetails[4];
        //var SUM_TotalAmount = RunningSpliteDetails[5];
        var SUM_ProductQuantity = parseFloat(RunningSpliteDetails[6]).toFixed(2);

        cTaxableAmtval.SetValue(SUM_Amount);
        cTaxAmtval.SetValue(SUM_TaxAmount);
        ctxt_Charges.SetValue(SUM_ChargesAmount);
        cOtherTaxAmtval.SetValue(SUM_ChargesAmount);
        cInvValue.SetValue(SUM_TotalAmount);
        cTotalAmt.SetValue(SUM_TotalAmount);
        cTotalQty.SetValue(SUM_ProductQuantity);
    }

    cProductsPopup.Hide();
}

function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        $('#<%=hdnRefreshType').val('');

        if (gridquotationLookup.GetValue() != null) {
            jAlert('Cannot Delete using this button as the Challan is linked with this OD.<br /> Click on Plus(+) sign to Add or Delete Product from last column!',
                'Alert Dialog: [Delete Challan Products]', function (r) {
                });
        }
        if (noofvisiblerows != "1" && gridquotationLookup.GetValue() == null) {
            Pre_Quantity = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColQuantity') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColQuantity') : "0";
            Pre_Amt = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColAmount') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColAmount') : "0";
            Pre_TotalAmt = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColTotalAmountINR') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColTotalAmountINR') : "0";

            Cur_Quantity = "0";
            Cur_Amt = "0";
            Cur_TotalAmt = "0";
            CalculateAmount();

            grid.DeleteRow(e.visibleIndex);

            $('#<%=hdfIsDelete').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display');

            grid.batchEditApi.StartEdit(-1, 2);
            grid.batchEditApi.StartEdit(0, 2);
        }
    }
    if (e.buttonID == 'CustomAddNewRow') {

        var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "";
        var SpliteDetails = ProductID.split("||@||");

        var IsComponentProduct = SpliteDetails[17];
        var ComponentProduct = SpliteDetails[18];

        if (IsComponentProduct == "Y") {
            var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
            jConfirm(messege, 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.GetEditor("IsComponentProduct").SetValue("Y");
                    $('#<%=hdfIsDelete').val('C');

                    grid.UpdateEdit();
                    grid.PerformCallback('Display~fromComponent');
                }
                else {
                    OnAddNewClick();
                }
            });
            document.getElementById('popup_ok').focus();
        }
        else {
            if (ProductID != "") {
                OnAddNewClick();
            }
            else {

                grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            }
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;

        var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

        if (inventoryType == "C" || inventoryType == "Y" || inventoryType == "B") {
            var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var ChallanID = (grid.GetEditor('ComponentID').GetValue() != null) ? grid.GetEditor('ComponentID').GetValue() : "0";

            if (QuantityValue == "0.0") {
                jAlert("Quantity should not be zero !.");
            } else {
                if (ProductID != "") {
                    var SpliteDetails = ProductID.split("||@||");
                    var strProductID = SpliteDetails[0];
                    var strDescription = SpliteDetails[1];
                    var strUOM = SpliteDetails[2];
                    var strStkUOM = SpliteDetails[4];
                    var strMultiplier = SpliteDetails[7];
                    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "";
                    var StkQuantityValue = QuantityValue * strMultiplier;
                    var stockids = SpliteDetails[10];
                    var Ptype = SpliteDetails[16];
                    var StkQuantityValue = QuantityValue * strMultiplier;

                    $('#<%=hdfProductType').val(Ptype);

                    $('#<%=hdfProductID').val(strProductID);
                    $('#<%=hdfProductSerialID').val(SrlNo);
                    $('#<%=hdnProductQuantity').val(QuantityValue);
                    $('#<%=hdfChallanID').val(ChallanID);

                    document.getElementById('<%=lblProductName').innerHTML = strDescription;
                    document.getElementById('<%=txt_SalesAmount').innerHTML = QuantityValue;
                    document.getElementById('<%=txt_SalesUOM').innerHTML = strUOM;
                    document.getElementById('<%=txt_StockAmount').innerHTML = StkQuantityValue;
                    document.getElementById('<%=txt_StockUOM').innerHTML = strStkUOM;
                    // cacpAvailableStock.PerformCallback(strProductID);

                    SelectWarehouse = "0";
                    $("#spnCmbWarehouse").hide();
                    $("#spntxtBatch").hide();
                    $("#spntxtQuantity").hide();
                    $("#spntxtserialID").hide();

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
            }
        }
        else {
            jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
        }
    }
}

function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}

function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}

function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }
}

function OnAddNewClick() {
    if (gridquotationLookup.GetValue() == null) {
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
    }
    else {
        QuotationNumberChanged();
        grid.StartEditRow(0);
    }

}

function ProductSelected(s, e) {
    if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
        return;
    }

    var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
    var focusedRow = cproductLookUp.gridView.GetFocusedRowIndex();
    var ProductCode = cproductLookUp.gridView.GetRow(focusedRow).children[1].innerText;

    if (!ProductCode) {
        LookUpData = null;
    }

    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("gvColProduct").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    pageheaderContent.style.display = "block";
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

    document.getElementById("ddlInventory").disabled = true;
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strDescription);

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        //divPacking.style.display = "block";
        divPacking.style.display = "none";
    } else {
        divPacking.style.display = "none";
    }

    Cur_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    Cur_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
    Cur_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
    CalculateAmount();

    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    grid.batchEditApi.StartEdit(globalRowIndex, 6);
}

//<%--Product Grid Section End--%>

//<%--Tax Section Start--%>

    function ctaxUpdatePanelEndCall(s, e) {
        if (ctaxUpdatePanel.cpstock != null) {
            ctaxUpdatePanel.cpstock = null;
            grid.batchEditApi.StartEdit(globalRowIndex, 6);
            return false;
        }
    }
function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}
var gstcstvatGlobalName;
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}
var taxAmountGlobal;
function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}
var taxJson;
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
        grid.batchEditApi.StartEdit(globalRowIndex, 14);
        grid.GetEditor("gvColTaxAmount").SetValue(totAmt);

        if (cddl_AmountAre.GetValue() == "2") {
            var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue());
            var totalRoundOffAmount = Math.round(totalNetAmount);

            grid.GetEditor("gvColTotalAmountINR").SetValue(totalRoundOffAmount);
            grid.GetEditor("gvColAmount").SetValue(DecimalRoundoff(parseFloat(grid.GetEditor("gvColAmount").GetValue()) + (totalRoundOffAmount - totalNetAmount), 2));
        }
        else {
            grid.GetEditor("gvColTotalAmountINR").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue()), 2));
        }

        Cur_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
        CalculateAmount();
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
function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
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

    ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
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


        $('.lblChargesGSTforGross').hide();
        $('.lblChargesGSTforNet').hide();

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

var taxAmountGlobalCharges;
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
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

    if (gridTax.cpChargesAmt) {
        ctxt_Charges.SetValue(gridTax.cpChargesAmt);
        gridTax.cpChargesAmt = null;

        Pre_Quantity = "0";
        Pre_Amt = "0";
        Pre_TotalAmt = "0";
        Cur_Quantity = "0";
        Cur_Amt = "0";
        Cur_TotalAmt = "0";
        CalculateAmount();
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

function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cPopup_Taxes.Hide();
}



//<%--Tax Section End--%>

//<%--Warehouse Section Start--%>

    var textSeparator = ";";
var selectedChkValue = "";
var SelectWarehouse = "0";
var SelectBatch = "0";
var SelectSerial = "0";
var SelectedWarehouseID = "0";
var IsPostBack = "";
var PBWarehouseID = "";
var PBBatchID = "";

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
}

function UpdateSelectAllItemState() {
    IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
}

function IsAllSelected() {
    if (checkListBox.GetItem(0)) {
        var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
        return checkListBox.GetSelectedItems().length == selectedDataItemCount;
    }
}

function UpdateText() {
    var selectedItems = checkListBox.GetSelectedItems();
    selectedChkValue = GetSelectedItemsText(selectedItems);

    var itemsCount = GetSelectedItemsCount(selectedItems);
    checkComboBox.SetText(itemsCount + " Items");

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

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');//Subhabrata
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
}

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

function fn_Edit(keyValue) {
    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}

function fn_DeleteWarehouse(keyValue) {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
}

function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');
}

 //<%--Warehouse Section End--%>


    function GetDateFormat(today) {
        if (today != "") {
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!

            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            today = dd + '-' + mm + '-' + yyyy;
        }

        return today;
    }

function GetPCDateFormat(today) {
    if (today != "") {
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}

function GetReverseDateFormat(today) {
    if (today != "") {
        var dd = today.substring(0, 2);
        var mm = today.substring(3, 5);
        var yyyy = today.substring(6, 10);

        today = mm + '-' + dd + '-' + yyyy;
    }

    return today;
}


    var Pre_Quantity = "0";
var Pre_Amt = "0";
var Pre_TotalAmt = "0";
var Cur_Quantity = "0";
var Cur_Amt = "0";
var Cur_TotalAmt = "0";

function CalculateAmount() {
    var Quantity = (parseFloat((cTotalQty.GetValue()).toString())).toFixed(2);
    var Amount = (parseFloat((cTaxableAmtval.GetValue()).toString())).toFixed(2);
    var TotalAmount = (parseFloat((cInvValue.GetValue()).toString())).toFixed(2);
    var ChargesAmount = (ctxt_Charges.GetValue() != null) ? (parseFloat(ctxt_Charges.GetValue())).toFixed(2) : "0";

    var Calculate_Quantity = (parseFloat(Quantity) + parseFloat(Cur_Quantity) - parseFloat(Pre_Quantity)).toFixed(2);
    var Calculate_Amount = (parseFloat(Amount) + parseFloat(Cur_Amt) - parseFloat(Pre_Amt)).toFixed(2);
    var Calculate_TotalAmount = (parseFloat(TotalAmount) + parseFloat(Cur_TotalAmt) - parseFloat(Pre_TotalAmt)).toFixed(2);
    var Calculate_TaxAmount = (parseFloat(Calculate_TotalAmount) - parseFloat(Calculate_Amount)).toFixed(2);
    var Calculate_SumAmount = (parseFloat(Calculate_TotalAmount) + parseFloat(ChargesAmount)).toFixed(2);

    cTotalQty.SetValue(Calculate_Quantity);
    cTaxableAmtval.SetValue(Calculate_Amount);
    cTaxAmtval.SetValue(Calculate_TaxAmount);
    cOtherTaxAmtval.SetValue(ChargesAmount);
    cInvValue.SetValue(Calculate_TotalAmount);
    cTotalAmt.SetValue(Calculate_SumAmount);
}


    function Save_ButtonClick() {
        flag = true;
        LoadingPanel.Show();

        var txtPurchaseNo = $("#txtVoucherNo").val().trim();
        var ddl_Vendor = $("#ddl_Vendor").val();

        if (txtPurchaseNo == null || txtPurchaseNo == "") {
            //flag = false;
            LoadingPanel.Hide();
            $("#MandatoryBillNo").show();
            return false;
        }
        if (ddl_Vendor == 0) {
            // flag = false;
            $("#MandatoryBillNo").show();
            return false;
        }

        //var customerId = GetObjectID('hdnCustomerId').value
        var customerId = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

        if (customerId == '' || customerId == null) {
            LoadingPanel.Hide();
            $('#MandatorysCustomer').attr('style', 'display:block');
            flag = false;
        }
        else {
            $('#MandatorysCustomer').attr('style', 'display:none');
        }

        var PartyInvoiceNo = ctxtPartyInvoice.GetValue;
        if (PartyInvoiceNo == '' || PartyInvoiceNo == null) {
            LoadingPanel.Hide();
            $('#MandatorysPartyinvno').attr('style', 'display:block');
            flag = false;
        }
        else {
            $('#MandatorysPartyinvno').attr('style', 'display:none');
        }

        var frontRow = 0;
        var backRow = -1;
        var IsProduct = "";
        for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
            var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
            var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";

            if (frontProduct != "" || backProduct != "") {
                IsProduct = "Y";
                break;
            }

            backRow--;
            frontRow++;
        }

        if (flag != false) {
            if (IsProduct == "Y") {
                var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                $('#<%=hdfLookupCustomer').val(customerval);
                $('#<%=hdfIsDelete').val('I');
                $('#<%=hdnRefreshType').val('N');
                grid.batchEditApi.EndEdit();
                $('#<%=hfControlData').val($('#hfControlSaveData').val());
                grid.UpdateEdit();
            }
            else {
                LoadingPanel.Hide();
                jAlert('Please add atleast single record first');
            }
        }
    }

function SaveExit_ButtonClick() {
    flag = true;
    LoadingPanel.Show();

    var txtPurchaseNo = $("#txtVoucherNo").val().trim();
    var ddl_Vendor = $("#ddl_Vendor").val();
    //alert(txtPurchaseNo);
    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        flag = false;
        LoadingPanel.Hide();
        $("#MandatoryBillNo").show();
        return false;
    }
    if (ddl_Vendor == 0) {
        flag = false;
        $("#MandatoryBillNo").show();
        return false;
    }

    //var customerId = GetObjectID('hdnCustomerId').value;
    var customerId = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

    if (customerId == '' || customerId == null) {
        LoadingPanel.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }

    var PartyInvoiceNo = ctxtPartyInvoice.GetValue;
    if (PartyInvoiceNo == '' || PartyInvoiceNo == null) {
        LoadingPanel.Hide();
        $('#MandatorysPartyinvno').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysPartyinvno').attr('style', 'display:none');
    }

    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }

    if (flag != false) {
        if (IsProduct == "Y") {
            var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            $('#<%=hdfLookupCustomer').val(customerval);
            $('#<%=hdnRefreshType').val('E');
            $('#<%=hdfIsDelete').val('I');
            grid.batchEditApi.EndEdit();
            $('#<%=hfControlData').val($('#hfControlSaveData').val());
            grid.UpdateEdit();
        }
        else {
            LoadingPanel.Hide();
            jAlert('Please add atleast one record.');
        }
    }
}

//Code for UDF Control 
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
