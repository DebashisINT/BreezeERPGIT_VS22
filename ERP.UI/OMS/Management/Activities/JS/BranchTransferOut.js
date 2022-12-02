
$(document).ready(function () {
    $('#ddl_transferFrom_Branch').change(function () {
        if ($("#hdnProjectSelectInEntryModule").val() == "1") {
            clookup_Project.gridView.Refresh();
        }
    });

});

function FrombranchChange() {
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        clookup_Project.gridView.Refresh();
    }
}



function clookup_Project_LostFocus() {
    //grid.batchEditApi.StartEdit(-1, 2);

    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}

function ProjectValueChange(s, e) {

    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'BranchTransferOut.aspx/getHierarchyID',
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
var isCtrl = false;

document.onkeyup = function (e) {
    //debugger;
    if (event.keyCode == 17) {
        isCtrl = false;
    }
    else if (event.keyCode == 27) {
        //btnCancel_Click();
    }
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
            case 78:
                StopDefaultAction(e);
                Save_ButtonClick();
                break;
            case 88:
                StopDefaultAction(e);
                SaveExit_ButtonClick();
                break;
            case 120:
                StopDefaultAction(e);
                SaveExit_ButtonClick();
                break;
                //case 84:
                //    StopDefaultAction(e);
                //    Save_TaxesClick();
                //    break;
                //case 85:
                //    OpenUdf();
                //    break;
        }
    }
}

document.onkeydown = function (e) {
    if (event.keyCode == 18) isCtrl = true;
    if (event.keyCode == 78 && isCtrl == true && getUrlVars().req != "V") { //run code for alt+N -- ie, Save & New  
        StopDefaultAction(e);
        Save_ButtonClick();
    }
    else if ((event.keyCode == 120 || event.keyCode == 88) && isCtrl == true && getUrlVars().req != "V") { //run code for Ctrl+X -- ie, Save & Exit!     
        StopDefaultAction(e);
        SaveExit_ButtonClick();
    }
}

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
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

function fn_Edit(keyValue) {
    //debugger;
    //cGrdWarehouse.PerformCallback('EditWarehouse~' + keyValue);
    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}

function ProductButnClick(s, e) {
    //debugger;
    if (e.buttonIndex == 0) {

        if (cproductLookUp.Clear()) {
            cProductpopUp.Show();
            cproductLookUp.Focus();
            cproductLookUp.ShowDropDown();
        }
    }
}

function LostFocusedPurpose(e) {
    //debugger;
    if (grid.GetVisibleRowsOnPage() > 1) {
        grid.batchEditApi.StartEdit(0, 1);
    }
    else {
        grid.batchEditApi.StartEdit(-1, 1);
    }
}


function acbpCrpUdfEndCall(s, e) {
    //debugger;
    if (cacbpCrpUdf.cpUDF) {

        if (cacbpCrpUdf.cpUDF == "false") {
            jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
            cacbpCrpUdf.cpUDF = null;

        }
        else if (cacbpCrpUdf.cpTransport == "false") {
            jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });

            cacbpCrpUdf.cpTransport = null;
        }

        if (cacbpCrpUdf.cpUDF == "true" && cacbpCrpUdf.cpTransport == "true") {
            grid.UpdateEdit();
        }
    }
}

function ProductSelected(s, e) {
    //debugger;
    if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
        return;
    }
    var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
    var ProductCode = cproductLookUp.GetValue();
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
    //var tbStkUOM = grid.GetEditor("StockUOM");
    //var tbStockQuantity = grid.GetEditor("StockQuantity");

    var ProductID = LookUpData;
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
    grid.batchEditApi.StartEdit(globalRowIndex, 5);
    cacpAvailableStock.PerformCallback(strProductID);
}

function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }
}

//<%--kaushik Section--%>

function SalePriceTextChange(s, e) {
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var strMultiplier = SpliteDetails[7];
    var strFactor = SpliteDetails[8];
    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    //var strRate = "1";
    var strStkUOM = SpliteDetails[4];
    //var strSalePrice = SpliteDetails[6];

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
    //var tbAmount = grid.GetEditor("Amount");
    //tbAmount.SetValue(Amount); 
    //var tbTotalAmount = grid.GetEditor("TotalAmount");
    //tbTotalAmount.SetValue(Amount); 
    //DiscountTextChange(s, e);
}

//'Subhabrata' on 15-03-2017
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
    var FifoExists = $("#hddnConfigVariable_Val").val();
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
    else {
        checkComboBox.SetText(0 + " Items");
    }

    if (FifoExists == "1") {
        checkListBox.SelectAll();
        checkListBox.SetEnabled(false);
        UpdateSelectAllItemState();
        UpdateText();
    }
    else {
        checkListBox.SetEnabled(true);
    }
}
function ctaxUpdatePanelEndCall(s, e) {
    if (ctaxUpdatePanel.cpstock != null) {
        divAvailableStk.style.display = "block";
        divpopupAvailableStock.style.display = "block";

        var AvlStk = ctaxUpdatePanel.cpstock + " " + $('#lblStkUOM').val();
        $('#lblAvailableStock').val() = ctaxUpdatePanel.cpstock;
        $('#lblAvailableStockUOM').val() = $('#lblStkUOM').val();

        ctaxUpdatePanel.cpstock = null;
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
        return false;
    }
}
//End
function DiscountTextChange(s, e) {
    //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var Amount = '';
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
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
        //var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        var Branch_Id = $("#ddl_transferFrom_Branch").val();
        $.ajax({
            type: "POST",
            url: "BranchTransferOut.aspx/GetStockValuation",
            data: JSON.stringify({ ProductId: ProductID.split('||@||')[0] }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {


                var ObjData = msg.d;
                if (ObjData.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "BranchTransferOut.aspx/GetStockValuationAmount",
                        data: JSON.stringify({ Pro_Id: ProductID.split('||@||')[0], Qty: QuantityValue, Valuationsign: ObjData, Fromdate: cPLSalesChallanDate.date.format('yyyy-MM-dd'), BranchId: Branch_Id }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg1) {
                            var ObjData1 = msg1.d;
                            if (ObjData1.length > 0) {
                                Amount = (ObjData1 * 1);
                            }
                        }

                    });
                }
            }

        });

        var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(amountAfterDiscount);

        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(amountAfterDiscount);
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
    }

    //Debjyoti 
    //grid.GetEditor('TaxAmount').SetValue(0);
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
}




function CmbScheme_ValueChange() {

    var val = $("#ddl_numberingScheme").val();

    $.ajax({
        type: "POST",
        url: 'PurchaseChallan.aspx/getSchemeType',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{sel_scheme_id:\"" + val + "\"}",
        success: function (type) {

            var schemetypeValue = type.d;
            var schemetype = schemetypeValue.toString().split('~')[0];
            var schemelength = schemetypeValue.toString().split('~')[1];

            var fromdate = schemetypeValue.toString().split('~')[2];
            var todate = schemetypeValue.toString().split('~')[3];

            var dt = new Date();

            cPLSalesChallanDate.SetDate(dt);

            if (dt < new Date(fromdate)) {
                cPLSalesChallanDate.SetDate(new Date(fromdate));
            }

            if (dt > new Date(todate)) {
                cPLSalesChallanDate.SetDate(new Date(todate));
            }




            cPLSalesChallanDate.SetMinDate(new Date(fromdate));
            cPLSalesChallanDate.SetMaxDate(new Date(todate));


            $('#txt_SlBTOutNo').attr('maxLength', schemelength);
            if (schemetype == '0') {

                //<%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                //document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";--%>
                ctxt_SlChallanNo.SetText('');
                ctxt_SlChallanNo.SetEnabled(false);
                ctxt_SlChallanNo.Focus();

            }
            else if (schemetype == '1') {

                // <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                // document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";--%>
                ctxt_SlChallanNo.SetText('Auto');
                ctxt_SlChallanNo.SetEnabled(false);
                cPLSalesChallanDate.Focus();
            }
            else if (schemetype == '2') {

                // <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                // document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";--%>
            }
            else if (schemetype == 'n') {
                //  <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                // document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";--%>
            }
        }
    });
}


function CmbWarehouse_ValueChange() {
    //debugger;
    var WarehouseID = cCmbWarehouse.GetValue();
    var FifoExists = $("#hddnConfigVariable_Val").val();
    $("#hddnWarehouseId").val(WarehouseID);
    var type = document.getElementById('hdfProductType').value;
    ctxtMatchQty.SetValue(0);
    if (WarehouseID != null) {
        if (type == "WBS" || type == "WB") {
            cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
        }
        else if (type == "WS" && FifoExists == "0") {
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + "NoFIFO");
        }
    }

}
function CmbBatch_ValueChange() {
    var WarehouseID = cCmbWarehouse.GetValue();
    var FifoExists = $("#hddnConfigVariable_Val").val();
    ctxtMatchQty.SetValue(0);
    $("#hddnWarehouseId").val(WarehouseID);
    var BatchID = cCmbBatch.GetValue();
    $("#hddnBatchId").val(BatchID);
    var type = document.getElementById('hdfProductType').value;

    if (type == "WBS" && FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "NoFIFO");
    }
    else if (type == "BS" && FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + "NoFIFO");
    }
    else if (type == "WS" && FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0" + '~' + "NoFIFO");
    }
}
//tab start
function disp_prompt(name) {

    if (name == "tab0") {
        //gridLookup.Focus();
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
//tab end




$(document).ready(function () {
    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
    }
    //$('#ApprovalCross').click(function () {
    //    debugger;
    //    window.parent.popup.Hide();
    //    window.parent.cgridPendingApproval.Refresh()();
    //})
})

function GetBillingAddressDetailByAddressId(e) {
    var addresskey = billingLookup.GetGridView().GetRowKey(billingLookup.GetGridView().GetFocusedRowIndex());
    if (addresskey != null && addresskey != '') {
        //debugger;
        cComponentPanel.PerformCallback('BlookupEdit~' + addresskey);
    }
}

function GetShippingAddressDetailByAddressId(e) {
    //debugger;
    var saddresskey = shippingLookup.GetGridView().GetRowKey(shippingLookup.GetGridView().GetFocusedRowIndex());
    if (saddresskey != null && saddresskey != '') {

        cComponentPanel.PerformCallback('SlookupEdit~' + saddresskey);
    }
}






function DateCheck() {
    debugger;
    var startDate = cPLSalesChallanDate.GetValueString();
    var dateCheck = dateCheckValidation();

    if (dateCheck == true) {

        cPLSalesChallanDate.Focus();
        var today = new Date();
        cPLSalesChallanDate.SetDate(today);
        jAlert("Future date is not allowed");
        return false;
    }
    else {
        cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + startDate + '~' + '@' + '~' + 'DateCheck');
        ccmbGstCstVat.PerformCallback();
        ccmbGstCstVatcharge.PerformCallback();


        //cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + startDate + '~' + '@');


        grid.PerformCallback('GridBlank');
        //cCmbWarehouse.Focus();
        $("#ddl_transferFrom_Branch").focus();
        $("#ddl_transferFrom_Branch").focus();
    }
}

function DateCheckChanged() {
    //debugger;
    var startDate = cPLSalesChallanDate.GetValueString();
    cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + startDate + '~' + '@' + '~' + '');
}

function CloseGridQuotationLookup() {
    gridSalesOrderLookup.ConfirmCurrentSelection();
    gridSalesOrderLookup.HideDropDown();
    gridSalesOrderLookup.Focus();
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

function dateCheckValidation() {
    debugger;

    var isAfterToday = false;
    var dt_BTODate = cPLSalesChallanDate.GetDate();
    //var startDate = cPLSalesChallanDate.date.format('dd/MM/yyyy');
    var startDate = cPLSalesChallanDate.date.format('MM/dd/yyyy');
    var today = new Date();


    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    //today = dd + '/' + mm + '/' + yyyy;
    today = mm + '/' + dd + '/' + yyyy;




    var start_date = new Date(dt_BTODate);
    if (startDate > today) {
        isAfterToday = true;
    }

    return isAfterToday;
}


function txtTax_TextChanged(s, i, e) {
    //debugger;

    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);




}
//Subhabrata Tax
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
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
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

function taxAmtButnClick(s, e) {
    //debugger;
    if (e.buttonIndex == 0) {

        if (cddl_AmountAre.GetValue() != null) {
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

            if (ProductID.trim() != "") {
                //debugger;
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
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                //var strRate = "1";
                var strStkUOM = SpliteDetails[4];
                // var strSalePrice = SpliteDetails[6];
                var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                if (strRate == 0) {
                    strRate = 1;
                }

                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = Math.round(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                clblTaxProdGrossAmt.SetText(Amount);
                clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue()).toFixed(2));
                document.getElementById('HdProdGrossAmt').value = Amount;
                document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue()).toFixed(2);

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
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {
                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I') {
                                    ccmbGstCstVat.RemoveItem(cmbCount);
                                    cmbCount--;
                                }
                            } else {
                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C') {
                                    ccmbGstCstVat.RemoveItem(cmbCount);
                                    cmbCount--;
                                }
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
                if (selectedIndex) {
                    if (ccmbGstCstVat.GetItem(selectedIndex) != null) {
                        ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
                    }
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
        grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));

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

var gstcstvatGlobalName;
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
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

}

function taxAmtButnClick1(s, e) {
    //console.log(grid.GetFocusedRowIndex());
    rowEditCtrl = s;
}




//function cgridTax_EndCallBack(s, e) {
//    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
//        ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);

//        cgridTax.cpUpdated = "";
//    }

//    else {
//        var totAmt = ctxtTaxTotAmt.GetValue();
//        cgridTax.CancelEdit();
//        caspxTaxpopUp.Hide();
//        grid.batchEditApi.StartEdit(globalRowIndex, 13);
//        grid.GetEditor("TaxAmount").SetValue(totAmt);

//    }
//}

$(document).ready(function () {
    //debugger;
    ctxtRate.SetValue("");
    ctxtRate.SetEnabled(false);
    ctxt_SlChallanNo.SetEnabled(false);
    gridSalesOrderLookup.SetEnabled(false);
    //DateCheckChanged(); 
    PopulateLoadGSTCSTVAT();
});



//Subhra-----23-01-2017-------
var Billing_state;
var Billing_city;
var Billing_pin;
var billing_area;

var Shipping_state;
var Shipping_city;
var Shipping_pin;
var Shipping_area;
//----------------------------------
function OnChildCall(CmbCity) {
    //debugger;
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

function fn_PopOpen() {
    CmbAddressType.SetSelectedIndex(-1);
    CmbAddressType1.SetSelectedIndex(-1);
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




    Popup_SalesQuote.Show();
    Popup_SalesQuote.PerformCallback('');
}

function cmbstate_endcallback(s, e) {
    s.SetValue(Billing_state);
    CmbCity.PerformCallback(CmbState.GetValue());
    Billing_state = 0;
}
function cmbshipstate_endcallback(s, e) {
    s.SetValue(Shipping_state);
    CmbCity1.PerformCallback(CmbState1.GetValue());
    Shipping_state = 0;
}

function cmbcity_endcallback(s, e) {
    s.SetValue(Billing_city);
    CmbPin.PerformCallback(CmbCity.GetValue());
    CmbArea.PerformCallback(CmbCity.GetValue());
    Billing_city = 0;

}
function cmbshipcity_endcallback(s, e) {
    s.SetValue(Shipping_city);
    CmbPin1.PerformCallback(CmbCity1.GetValue());
    CmbArea1.PerformCallback(CmbCity1.GetValue());
    Shipping_city = 0;

}

function cmbPin_endcallback(s, e) {
    s.SetValue(Billing_pin);
    Billing_pin = 0;
}
function cmbshipPin_endcallback(s, e) {
    s.SetValue(Shipping_pin);
    Shipping_pin = 0;
}

function cmbArea_endcallback(s, e) {
    s.SetValue(billing_area);
    billing_area = 0;
}

function cmbshipArea_endcallback(s, e) {
    s.SetValue(Shipping_area);
    Shipping_area = 0;
}

function Popup_SalesQuote_EndCallBack() {
    if (Popup_SalesQuote.cpshow != null) {


        CmbAddressType.SetText(Popup_SalesQuote.cpshow.split('~')[0]);
        ctxtAddress1.SetText(Popup_SalesQuote.cpshow.split('~')[1]);
        ctxtAddress2.SetText(Popup_SalesQuote.cpshow.split('~')[2]);
        ctxtAddress3.SetText(Popup_SalesQuote.cpshow.split('~')[3]);
        ctxtlandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
        CmbCountry.SetValue(Popup_SalesQuote.cpshow.split('~')[5]);
        Billing_state = Popup_SalesQuote.cpshow.split('~')[6];
        Billing_city = Popup_SalesQuote.cpshow.split('~')[7];
        Billing_pin = Popup_SalesQuote.cpshow.split('~')[8];
        billing_area = Popup_SalesQuote.cpshow.split('~')[9];
        CmbState.PerformCallback(CmbCountry.GetValue());
    }

    if (Popup_SalesQuote.cpshowShip != null) {


        CmbAddressType1.SetText(Popup_SalesQuote.cpshowShip.split('~')[0]);
        ctxtsAddress1.SetText(Popup_SalesQuote.cpshowShip.split('~')[1]);
        ctxtsAddress2.SetText(Popup_SalesQuote.cpshowShip.split('~')[2]);
        ctxtsAddress3.SetText(Popup_SalesQuote.cpshowShip.split('~')[3]);
        ctxtslandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
        CmbCountry1.SetValue(Popup_SalesQuote.cpshowShip.split('~')[5]);
        Shipping_state = Popup_SalesQuote.cpshowShip.split('~')[6];
        Shipping_city = Popup_SalesQuote.cpshowShip.split('~')[7];
        Shipping_pin = Popup_SalesQuote.cpshowShip.split('~')[8];
        Shipping_area = Popup_SalesQuote.cpshowShip.split('~')[9];
        CmbState1.PerformCallback(CmbCountry1.GetValue());
    }

}

function CloseGridLookup() {
    gridSalesOrderLookup.SetEnabled(true);
}
function GetContactPerson(e) {
    //debugger;
    //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());


    //if (key != null && key != '') {


    var startDate = new Date();

    startDate = cPLSalesChallanDate.GetValueString();
    cchkBilling.SetChecked(false);
    cchkShipping.SetChecked(false);
    cContactPerson.PerformCallback('BindContactPerson~' + key);


    page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
    jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            page.SetActiveTabIndex(1);
            $('.dxeErrorCellSys').addClass('abc');
            $('.crossBtn').hide();
        }
    });

    cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + startDate + '~' + '@');
    GetObjectID('hdnCustomerId').value = key;
    GetObjectID('hdnAddressDtl').value = '0';

    //}
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

$(document).ready(function () {
    //debugger;
    //$('#ddl_numberingScheme').focus();
    $('#ddl_numberingScheme').change(function () {
        //debugger;
        var NoSchemeTypedtl = $(this).val();
        var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
        var branchIdScheme = NoSchemeTypedtl.toString().split('~')[3];


        var fromdate = NoSchemeTypedtl.toString().split('~')[5];
        var todate = NoSchemeTypedtl.toString().split('~')[6];

        var dt = new Date();

        cPLSalesChallanDate.SetDate(dt);

        if (dt < new Date(fromdate)) {
            cPLSalesChallanDate.SetDate(new Date(fromdate));
        }

        if (dt > new Date(todate)) {
            cPLSalesChallanDate.SetDate(new Date(todate));
        }




        cPLSalesChallanDate.SetMinDate(new Date(fromdate));
        cPLSalesChallanDate.SetMaxDate(new Date(todate));


        if (NoSchemeType == '1') {
            ctxt_SlChallanNo.SetText('Auto');
            ctxt_SlChallanNo.SetEnabled(false);
            cPLSalesChallanDate.SetText('31-10-2017');
            cPLSalesChallanDate.Focus();
            cPLSalesChallanDate.SetEnabled(false);


        }
        else if (NoSchemeType == '0') {
            ctxt_SlChallanNo.SetText('');
            ctxt_SlChallanNo.SetEnabled(true);
            ctxt_SlChallanNo.Focus();
            cPLSalesChallanDate.SetEnabled(true);

        }
        else {
            ctxt_SlChallanNo.SetText('');
            //ctxt_SlChallanNo.SetEnabled(false);
            //document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();
            $("#ddl_numberingScheme").Focus();

        }
        $("#hddnBranchNumberingSchemeWise").val(branchIdScheme);
        $("#ProjectForBranch").val(branchIdScheme);
        if ($("#hdnProjectSelectInEntryModule").val() == "1") {
            clookup_Project.gridView.Refresh();
        }
        //Subhabrata to Bind Vehicle from master accordingly
        $.ajax({
            type: "POST",
            url: "BranchTransferOut.aspx/GetVehicles",
            data: JSON.stringify({ BranchId: branchIdScheme }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (r) {
                var ddl_VehicleNo = $("[id*=ddl_VehicleNo]");
                ddl_VehicleNo.empty().append('<option selected="selected" value="0">Please select</option>');
                $.each(r.d, function () {
                    ddl_VehicleNo.append($("<option></option>").val(this['Id']).html(this['Name']));
                });
            }
        });
        //End
        if ($("#hdnProjectSelectInEntryModule").val() == "1")
            clookup_Project.gridView.Refresh();
        DateCheckChanged();
    });
    $("#ddl_VehicleNo").change(function () {
        //debugger;
        var VehicleNo = $("#ddl_VehicleNo").val();
        $("#hddnddlVehicle").val(VehicleNo);
        $.ajax({
            type: "POST",
            url: "BranchTransferOut.aspx/GetDriverNamePhNo",
            data: JSON.stringify({ cnt_Id: VehicleNo }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                //debugger;
                var currentMsg = msg.d;
                var Name = currentMsg.split(',')[0];
                var PhoneNo = currentMsg.split(',')[1];
                $('#txtDriverName').find('input').val(Name);
                $('#txtPhoneNo').find('input').val(PhoneNo);
                $('#txt_Refference').focus();
            }
        });

    });



});

function PopulateGSTCSTVAT(e) {
    var key = cddl_AmountAre.GetValue();
    if (key == 1) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(false);
        // cddlVatGstCst.PerformCallback('1');
        cddlVatGstCst.SetSelectedIndex(0);
    }
    else if (key == 2) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');

    }
    else if (key == 3) {
        grid.GetEditor('TaxAmount').SetEnabled(false);
        cddlVatGstCst.SetEnabled(false);
        // cddlVatGstCst.PerformCallback('3');
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


// Popup Section

function ShowCustom() {

    cPopup_wareHouse.Show();


}

// Popup Section End

var currentEditableVisibleIndex;
var preventEndEditOnLostFocus = false;
var lastProductID;
var setValueFlag;

//function ProductsCombo_SelectedIndexChanged(s, e) {
//    debugger;
//    var tbDescription = grid.GetEditor("Description");
//    var tbUOM = grid.GetEditor("UOM");
//    var tbStkUOM = grid.GetEditor("StockUOM");
//    var tbSalePrice = grid.GetEditor("SalePrice");
//    var tbStockQuantity = grid.GetEditor("StockQuantity");

//    var ProductID = s.GetValue();
//    var SpliteDetails = ProductID.split("||@||");
//    var strProductID = SpliteDetails[0];
//    var strDescription = SpliteDetails[1];
//    var strUOM = SpliteDetails[2];
//    var strStkUOM = SpliteDetails[4];
//    var strSalePrice = SpliteDetails[6];

//    tbDescription.SetValue(strDescription);
//    tbUOM.SetValue(strUOM);
//    tbStkUOM.SetValue(strStkUOM);
//    tbSalePrice.SetValue(strSalePrice);
//    tbStockQuantity.SetValue("0");
//}

function ProductsCombo_SelectedIndexChanged(s, e) {
    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");
    //var tbStkUOM = grid.GetEditor("StockUOM");
    //var tbStockQuantity = grid.GetEditor("StockQuantity");

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

    $('#lblStkQty').val("0.00");
    $('#lblStkUOM').val(strStkUOM);
    cacpAvailableStock.PerformCallback(strProductID);
    //tbStkUOM.SetValue(strStkUOM);
    //tbStockQuantity.SetValue("0");
}



function OnEndCallback(s, e) {
    //debugger;
    LoadingPanel.Hide();
    var value = document.getElementById('hdnRefreshType').value;
    //Subhabrata
    if (grid.cpSaveSuccessOrFail == "outrange") {
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');
        grid.cpSaveSuccessOrFail = null;
        $("#hddnSaveOrExitButton").val('');
        $('#hdnPageStatus').val('');
        $("#hdnRefreshType").val('');
    }
    else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
        AddNewRowGrid();
        jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });

        grid.cpSaveSuccessOrFail = null;
        $("#hddnSaveOrExitButton").val('');
        $('#hdnPageStatus').val('');
        $("#hdnRefreshType").val('');
    }
    else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Can Not Save as Duplicate Quotation Numbe No. Found');
        grid.cpSaveSuccessOrFail = null;
        $("#hddnSaveOrExitButton").val('');
        $('#hdnPageStatus').val('');
        $("#hdnRefreshType").val('');
        AddNewRowGrid();
    }
    else if (grid.cpSaveSuccessOrFail == "errorInsert") {
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Please try again later.');
        grid.cpSaveSuccessOrFail = null;
        $("#hddnSaveOrExitButton").val('');
        $('#hdnPageStatus').val('');
        AddNewRowGrid();
    }
    else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
        grid.batchEditApi.StartEdit(0, 2);
        grid.cpSaveSuccessOrFail = null;
        jAlert('Please select project.');
    }
    else if (grid.cpSaveSuccessOrFail == "AddLock") {
        grid.batchEditApi.StartEdit(0, 2);
        grid.cpSaveSuccessOrFail = null;
        jAlert('DATA is Freezed between ' + grid.cpAddLockStatus + ' for Add.');
        grid.cpAddLockStatus = null;
    }
    else if (grid.cpSaveSuccessOrFail == "DocumentNumberAlreadyExists") {
        grid.batchEditApi.StartEdit(0, 2);
        grid.cpSaveSuccessOrFail = null;
        jAlert('Document Number Already Exists.');
    }
    else if (grid.cpProductZeroStock == "ZeroStock") {
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Insufficient Available Stock.Cannot proceed');
        grid.cpProductZeroStock = null;
        $("#hddnSaveOrExitButton").val('');
        $('#hdnPageStatus').val('');
        $("#hdnRefreshType").val('');
        AddNewRowGrid();
    }
    else if (grid.cpProductMoreThanStock == "MoreThanStock") {
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Given quantity more than available Stock.Cannot proceed');
        grid.cpProductMoreThanStock = null;
        $("#hddnSaveOrExitButton").val('');
        $('#hdnPageStatus').val('');
        $("#hdnRefreshType").val('');
    }
    else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
        //OnAddNewClick();
        //grid.cpSaveSuccessOrFail = null;

        var SrlNo = grid.cpProductSrlIDCheck;
        var msg = "Product [" + SrlNo + "] has no warehouse selected.Cannot proceed.";
        jAlert(msg);
        grid.cpSaveSuccessOrFail = null;
        grid.cpProductSrlIDCheck = null;
        $("#hddnSaveOrExitButton").val('');
        $('#hdnPageStatus').val('');
        $("#hdnRefreshType").val('');
        AddNewRowGrid();
    }
    else if (grid.cpSaveSuccessOrFail == "checkWarehouseQty") {
        AddNewRowGrid();
        grid.cpSaveSuccessOrFail = null;

        var SrlNo = grid.cpProductSrlIDCheck1;
        var msg = "Product Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
        jAlert(msg);
        grid.cpSaveSuccessOrFail = null;
        grid.cpProductSrlIDCheck1 = null;
        $("#hddnSaveOrExitButton").val('');
        $('#hdnPageStatus').val('');
        $("#hdnRefreshType").val('');
    }
    else {
        var pageStatusPrint = document.getElementById('hdnPageStatus').value;
        var SalesOrder_Number = grid.cpSalesOrderNo;
        var Order_Msg = "Branch Transfer Out  " + SalesOrder_Number + " generated.";
        if (value == "E") {

            if (SalesOrder_Number != "") {
                jAlert(Order_Msg, 'Alert Dialog: [BO]', function (r) {
                    if (r == true) {
                        grid.cpSalesOrderNo = null;
                        window.location.assign("BranchTransferOutLEntityList.aspx");
                    }
                });

            }
            else {
                window.location.assign("BranchTransferOutLEntityList.aspx");
            }
        }
        else if (value == "N") {


            if (SalesOrder_Number != "") {
                jAlert(Order_Msg, 'Alert Dialog', function (r) {
                    grid.cpSalesOrderNo = null;
                    if (r == true) {
                        window.location.assign("BranchTransferOut.aspx?key=ADD");
                    }
                });
            }
            else {
                window.location.assign("BranchTransferOut.aspx?key=ADD");
            }
        }
        else {
            //debugger;
            var pageStatus = document.getElementById('hdnPageStatus').value;
            if (pageStatus == "first") {
                //debugger;
                if (grid.GetVisibleRowsOnPage() == 0) {
                    OnAddNewClick();
                }
                grid.batchEditApi.EndEdit();
                $('#ddl_numberingScheme').focus();
                //document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();
                $("#ddl_numberingScheme").focus();
                $('#hdnPageStatus').val('');
            }
            else if (pageStatus == "update") {
                if (grid.GetVisibleRowsOnPage() == 0) {
                    OnAddNewClick();
                }
                $('#hdnPageStatus').val('');
            }
            else if (pageStatus == "Quoteupdate") {
                grid.StartEditRow(0);
                $('#hdnPageStatus').val('');
            }
        }

        if ($("#hdnPageModeforPrint").val() == "add") {
            if ($("#hddnSaveOrExitButton").val() == 'Save_Exit') {
                var DocumentNo = grid.cpDocumentNo;
                if ($("#hdnPrintingBranchTransferOUT").val() == "1") {
                    if ($("#hdnMultiplePrintingBranchTransferOUT").val() == "Yes") {
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=BTO-Default~D&modulename=BranchTranOut&id=" + DocumentNo + '&PrintOption=1', '_blank');
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=BTO-Default~D&modulename=BranchTranOut&id=" + DocumentNo + '&PrintOption=2', '_blank');
                    }
                    else {
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=BTO-Default~D&modulename=BranchTranOut&id=" + DocumentNo + '&PrintOption=1', '_blank');
                    }
                }

            }
            else if ($("#hddnSaveOrExitButton").val() == 'Save_New') {
                var DocumentNo = grid.cpDocumentNo;
                if ($("#hdnPrintingBranchTransferOUT").val() == "1") {
                    if ($("#hdnMultiplePrintingBranchTransferOUT").val() == "Yes") {
                        //window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=BTO-Default~D&modulename=BranchTranOut&id=" + DocumentNo, '_blank');
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=BTO-Default~D&modulename=BranchTranOut&id=" + DocumentNo + '&PrintOption=1', '_blank');
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=BTO-Default~D&modulename=BranchTranOut&id=" + DocumentNo + '&PrintOption=2', '_blank');
                    }
                    else {
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=BTO-Default~D&modulename=BranchTranOut&id=" + DocumentNo + '&PrintOption=2', '_blank');
                    }
                }
            }
        }

    }
    //debugger;
    if (gridSalesOrderLookup.GetValue() != null) {
        grid.GetEditor('ProductName').SetEnabled(false);
        grid.GetEditor('Description').SetEnabled(false);
        grid.GetEditor('Order_Num').SetEnabled(false);
    }
    else {
        if (grid.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();
        }
    }
    cProductsPopup.Hide();
    for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {
        grid.batchEditApi.StartEdit(i);
    }
}
function Save_ButtonClick() {
    //debugger;
    $("#hddnSaveOrExitButton").val('Save_New');
    var flag = true;
    LoadingPanel.Show();
    $('#hfControlData').val($('#hfControlSaveData').val());
    var OrderNo = ctxt_SlChallanNo.GetText();
    var slsdate = cPLSalesChallanDate.GetValue();
    var qudate = cPLQADate.GetText();
    var lookupOrder = gridSalesOrderLookup.GetValue();
    var VehicleNo = $("#ddl_VehicleNo").val();
    //dt_BTOut
    //var customerid = GetObjectID('hdnCustomerId').value;
    var salesorderDate = new Date(slsdate);
    var quotationDate = "";

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
    }


    if (qudate != null && qudate != '') {
        var qd = qudate.split('-');
        LoadingPanel.Hide();
        quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

    }


    if (lookupOrder == null) {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatoryLookupOrder').attr('style', 'display:block');
    }
    else {
        $('#MandatoryLookupOrder').attr('style', 'display:none');
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

    //if (VehicleNo == "0") {
    //    flag = false;
    //    jAlert('Please select vehicle no');
    //    $('#MandatoryVehicleNo').attr('style', 'display:block');
    //}
    //else {
    //    $('#MandatoryVehicleNo').attr('style', 'display:none');
    //}

    if (flag) {
        if (grid.GetVisibleRowsOnPage() > 0) {
            // <%--var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            // $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);--%>


            $('#hdfIsDelete').val('I');
            grid.batchEditApi.EndEdit();
            //grid.UpdateEdit();
            cacbpCrpUdf.PerformCallback();
            $('#hdnRefreshType').val('N');
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
    LoadingPanel.Show();
    var flag = true;
    $('#hfControlData').val($('#hfControlSaveData').val());
    $("#hddnSaveOrExitButton").val('Save_Exit');
    var OrderNo = ctxt_SlChallanNo.GetText();
    var slsdate = cPLSalesChallanDate.GetValue();
    var lookupOrder = gridSalesOrderLookup.GetValue();
    var qudate = cPLQADate.GetText();
    var VehicleNo = $("#ddl_VehicleNo").val();
    var customerid = GetObjectID('hdnCustomerId').value;
    var salesorderDate = new Date(slsdate);
    var quotationDate = "";

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
    }

    if (qudate != null && qudate != '') {
        var qd = qudate.split('-');
        LoadingPanel.Hide();
        quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

    }

    if (lookupOrder == null) {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatoryLookupOrder').attr('style', 'display:block');
    }
    else {
        $('#MandatoryLookupOrder').attr('style', 'display:none');
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

    //if (VehicleNo == "0") {
    //    flag = false;
    //    LoadingPanel.Hide();
    //    jAlert('Please select vehicle no');
    //    $('#MandatoryVehicleNo').attr('style', 'display:block');
    //}
    //else {
    //    $('#MandatoryVehicleNo').attr('style', 'display:none');
    //}

    if (OrderNo == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlOrderNo').attr('style', 'display:block');
    }
    else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }


    if (flag) {
        if (grid.GetVisibleRowsOnPage() > 0) {
            // <%--var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            // $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);--%>


            $('#hdfIsDelete').val('I');
            grid.batchEditApi.EndEdit();
            //grid.UpdateEdit();
            cacbpCrpUdf.PerformCallback();
            $('#hdnRefreshType').val('E');
        }
        else {
            LoadingPanel.Hide();
            jAlert('You must enter proper details before save');
        }
    }
    // return flag;
}



function QuantityTextChange(s, e) {
    debugger;
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();
    var Amount = '';

    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strMultiplier = SpliteDetails[7];
        var strFactor = SpliteDetails[8];
        //var strRate = (ctxt_Rate.GetValue() != null) ? ctxt_Rate.GetValue() : "1";
        var strRate = "1";
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];

        if (strRate == 0) {
            strRate = 1;
        }

        var StockQuantity = strMultiplier * QuantityValue;

        //Subhabrata:Commented for Fetching FIFO valuation on 20-07-2017
        //var Amount = QuantityValue * strFactor * strRate * strSalePrice;
        //END
        var Branch_Id = $("#ddl_transferFrom_Branch").val();
        $.ajax({
            type: "POST",
            url: "BranchTransferOut.aspx/GetStockValuation",
            data: JSON.stringify({ ProductId: ProductID.split('||@||')[0] }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {


                var ObjData = msg.d;
                if (ObjData.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "BranchTransferOut.aspx/GetStockValuationAmount",
                        data: JSON.stringify({ Pro_Id: ProductID.split('||@||')[0], Qty: QuantityValue, Valuationsign: ObjData, Fromdate: cPLSalesChallanDate.date.format('yyyy-MM-dd'), BranchId: Branch_Id }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg1) {
                            var ObjData1 = msg1.d;
                            if (ObjData1.length > 0) {
                                Amount = (ObjData1 * 1);
                            }
                        }

                    });
                }
            }

        });


        $('#lblStkQty').val(StockQuantity);
        $('#lblStkUOM').val(strStkUOM);

        //var tbStockQuantity = grid.GetEditor("StockQuantity");
        //tbStockQuantity.SetValue(StockQuantity);

        //Subhabrata added on 14-03-2017
        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y") {
            $('#lblPackingStk').val(PackingValue);
            //console.log('jhsdfafa');
            //divPacking.style.display = "block";
            $('#divPacking').css({ 'display': 'block' });
        } else {
            divPacking.style.display = "none";
        }//END

        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(Amount);

        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(Amount);
        var strProductID = ProductID.split('||@||')[0]
        if (ProductID != "0") {
            cacpAvailableStock.PerformCallback(strProductID);
        }
        //DiscountTextChange(s, e);
    }
    else {
        jAlert('Select a product first.');
    }
}

//function DiscountTextChange(s, e) {
//    var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
//    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

//    var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

//    var tbAmount = grid.GetEditor("Amount");
//    tbAmount.SetValue(amountAfterDiscount);

//    var tbTotalAmount = grid.GetEditor("TotalAmount");
//    tbTotalAmount.SetValue(amountAfterDiscount);
//}
function AddBatchNew(s, e) {
    //debugger;
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
    //debugger;
    if (gridSalesOrderLookup.GetValue() == null) {
        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i;
        var cnt = 1;
        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(cnt);


            cnt++;
        }
    }
    else {
        OrderNumberChanged();
    }

}

function AddNewRowGrid() {
    grid.AddNewRow();

    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i;
    var cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);


        cnt++;
    }
}


var Warehouseindex;

function FinalWarehouse() {
    //debugger;
    cGrdWarehouse.PerformCallback('WarehouseFinal');
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');
}

function OnWarehouseEndCallback(s, e) {
    //debugger;
    var Ptype = document.getElementById('hdfProductType').value;

    //Added Subhabrata on 22-06-2017
    if (cGrdWarehouse.cpWarehouseDeleticity != "WareHouseDeleticity") {
        var WarehouseBindQty = cGrdWarehouse.cpWarehouseQty;
        $("#hddnWarehouseQty").val(WarehouseBindQty);
    }

    var FifoExists = $("#hddnConfigVariable_Val").val();
    if (cGrdWarehouse.cpWarehouseDeleticity == "WareHouseDeleticity" && FifoExists == "1") {
        cGrdWarehouse.cpWarehouseDeleticity = null;
        var WarehouseID = $("#hddnWarehouseId").val();
        var BatchID = $("#hddnBatchId").val();

        var Qty = ctxtMatchQty.GetValue();
        var WarehouseBindQty1 = cGrdWarehouse.cpWarehouseQty;
        $("#hddnWarehouseQty").val(WarehouseBindQty1);

        var hddnQty = $("#hddnWarehouseQty").val();
        //var ResultantQty = (Qty * 1) - (hddnQty * 1);
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "0");

    }
    if (cGrdWarehouse.cpWarehouseSaveDisplay == "SaveDisplay" && FifoExists == "1") {
        cGrdWarehouse.cpWarehouseSaveDisplay = null;
        //ctxtMatchQty.SetText('');
        var WarehouseID = $("#hddnWarehouseId").val();
        var BatchID = $("#hddnBatchId").val();
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "0");
    }

    //End
    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 7);
    }
    else if (cGrdWarehouse.cperrorMsg == "duplicateSerial") {
        cGrdWarehouse.cperrorMsg = null;
        jAlert("Duplicate Serial. Cannot Proceed.");
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Entered Quantity for the selected product must be equal to Stock Quantity.');
    }
    else {
        if (document.getElementById("myCheck").checked == true) {
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




function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function PopulateSerial() {

    //Serail Bind:Start
    checkComboBox.clientEnabled = true;
    //End
    //debugger;
    var SessionCountSerial = '';
    var indices = [];
    var Qty = ctxtMatchQty.GetValue();
    $("#hddnMatchQty").val(Qty);
    var CountLength = checkListBox.GetItem.length;
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    checkListBox.SetEnabled(true);
    //QuantityResultant = (QuantityResultant * 1) + (Qty * 1);
    SessionCountSerial = $("#hddnWarehouseQty").val();
    if (SessionCountSerial != null) {
        SessionCountSerial = (SessionCountSerial * 1) + (Qty * 1);
        //SessionCountSerial = (Qty * 1);
    }
    else {
        SessionCountSerial = (Qty * 1);
    }

    if ((SessionCountSerial * 1) > QuantityValue) {
        checkListBox.UnselectAll();
        jAlert("Warehouse total Qty must be qual to entered Qty.Cannot proceed!");
        checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
    }
    else {
        checkListBox.UnselectAll();
        //Subhabrata added: on 19-06-2017
        var WarehouseID = $("#hddnWarehouseId").val();
        var BatchID = $("#hddnBatchId").val();
        var type = document.getElementById('hdfProductType').value;

        if (type == "WBS") {
            //cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + (SessionCountSerial * 1));
        }
        else if (type == "BS") {
            checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + (SessionCountSerial * 1));
        }
        else if (type == "WS") {
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + (SessionCountSerial * 1));
        }
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
    var FifoExists = $("#hddnConfigVariable_Val").val();
    var items = checkListBox.GetSelectedItems();
    var vals = [];
    var texts = [];

    var AltQty = (ctxtAltQuantity.GetValue() != null) ? ctxtAltQuantity.GetValue() : "0";    
    var AltUOM = (ccmbPackingUom1.GetValue() != null) ? ccmbPackingUom1.GetValue() : "0";
    //var AltUOM = ccmbPackingUom1.GetValue();

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
                //checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
                checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
                ctxtQuantity.SetValue("0");
                //ccmbPackingUom1.SetValue("0");
                ctxtAltQuantity.SetValue("0");
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
            //checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
           // ccmbPackingUom1.SetValue("0");
            ctxtAltQuantity.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID + '~' + AltQty + '~' + AltUOM);
        SelectedWarehouseID = "0";
    }
}



function txtserialTextChanged() {
    //debugger;
    var SerialNo = (ctxtserial.GetValue != null) ? (ctxtserial.GetValue()) : "0";
    ctxtserial.SetValue("");
    var texts = [SerialNo];
    var values = GetValuesByTexts(texts);
    checkListBox.UnselectAll();
    checkListBox.SelectValues(values);
    UpdateSelectAllItemState();
    UpdateText(); // for remove non-existing texts
    if (SerialNo != 0) {
        SaveWarehouse();
    }

    //Subhabrata added: on 19-06-2017
    var WarehouseID = $("#hddnWarehouseId").val();
    var BatchID = $("#hddnBatchId").val();
    var FifoExists = $("#hddnConfigVariable_Val").val();
    var MatchQty = $("#hddnMatchQty").val();
    var type = document.getElementById('hdfProductType').value;

    if (type == "WBS" || type == "WB") {
        //cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
        //checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
        if (FifoExists == "0") {
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "NoFIFO");
        }
        else if (FifoExists == "1") {
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + (MatchQty * 1));
        }
    }
    else if (type == "BS") {
        //checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
        if (FifoExists == "0") {
            checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + "NoFIFO");
        }
        else if (FifoExists == "1") {
            checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + (MatchQty * 1));
        }
    }
    else if (type == "WS") {
        //checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
        if (FifoExists == "0") {
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + "NoFIFO");
        }
        else if (FifoExists == "1") {
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + (MatchQty * 1));
        }
    }
    UpdateSelectAllItemState();
    UpdateText(); // for remove non-existing texts
    //END
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

    var FifoExists = $("#hddnConfigVariable_Val").val();

    var Qty = ctxtMatchQty.GetValue();
    var hddnQty = $("#hddnWarehouseQty").val();


    cGrdWarehouse.PerformCallback('Delete~' + keyValue);

    if (FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + 'NoFIFO');
    }
    //checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
}



$(document).ready(function () {
    $('#ddl_VatGstCst_I').blur(function () {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    })
    $('#ddl_AmountAre_I').blur(function () {


        var key = cddl_AmountAre.GetValue();

        if (key == 1 || key == 3) {
            if (grid.GetVisibleRowsOnPage() == 1) {
                grid.batchEditApi.StartEdit(-1, 2);
            }

        }
    })

});

// <![CDATA[
var textSeparator = ";";
var selectedChkValue = "";
var IsSelected = false;
function OnListBoxSelectionChanged(listBox, args) {
    //debugger;

    //if (args.index == 0)
    //    args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();

    if (args.index == 0) {
        args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
        if (args.isSelected) {
            IsSelected = true;
        }
        else {
            IsSelected = false;
        }

    }


    UpdateSelectAllItemState();
    UpdateText();

    var selectedItems = checkListBox.GetSelectedItems();
    var val = GetSelectedItemsText(selectedItems);
    var strWarehouse = cCmbWarehouse.GetValue();
    var strBatchID = cCmbBatch.GetValue();
    var ProducttId = $("#hdfProductID").val();
    //$.ajax({
    //    type: "POST",
    //    url: "BranchTransferOut.aspx/GetSerialId",
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
    //        debugger;
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

}
function UpdateSelectAllItemState() {
    IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
}
function IsAllSelected() {
    var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
    return checkListBox.GetSelectedItems().length == selectedDataItemCount;
}
function UpdateText() {
    //var selectedItems = checkListBox.GetSelectedItems();
    //selectedChkValue = GetSelectedItemsText(selectedItems);
    ////checkComboBox.SetText(GetSelectedItemsText(selectedItems));
    //checkComboBox.SetText(selectedItems.length + " Items");

    //var val = GetSelectedItemsText(selectedItems);
    //$("#abpl").attr('data-content', val);

    var selectedItems = checkListBox.GetSelectedItems();
    selectedChkValue = GetSelectedItemsText(selectedItems);
    //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
    var ActualLength = (checkListBox.GetItemCount() * 1);
    if (IsSelected == true || ActualLength == selectedItems.length) {
        checkComboBox.SetText(((selectedItems.length * 1) - 1) + " Items");
    }
    else {
        checkComboBox.SetText((selectedItems.length) + " Items");
    }

    if (((selectedItems.length * 1) - 1) < 0) {

        checkComboBox.SetText(0 + " Items");
    }


    var val = GetSelectedItemsText(selectedItems);
    $("#abpl").attr('data-content', val);
}
function SynchronizeListBoxValues(dropDown, args) {
    checkListBox.UnselectAll();
    //var texts = dropDown.GetText().split(textSeparator);
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



//Subhra-----23-01-2017-------
var Billing_state;
var Billing_city;
var Billing_pin;
var billing_area;

var Shipping_state;
var Shipping_city;
var Shipping_pin;
var Shipping_area;
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

//kaushik-----24-02-2017-------
function SettingTabStatus() {
    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
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

    //cComponentPanel.PerformCallback('Edit~BillingAddresstoShipping');
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
        GetObjectID('hdnAddressDtl').value = '1';
        page.SetActiveTabIndex(0);
        //gridLookup.Focus();
    }
    else {
        page.SetActiveTabIndex(1);
    }
}
function ClosebillingLookup() {
    billingLookup.ConfirmCurrentSelection();
    billingLookup.HideDropDown();
    billingLookup.Focus();
}
function CloseshippingLookup() {
    shippingLookup.ConfirmCurrentSelection();
    shippingLookup.HideDropDown();
    shippingLookup.Focus();
}


var Billing_state;
var Billing_city;
var Billing_pin;
var billing_area;

var Shipping_state;
var Shipping_city;
var Shipping_pin;
var Shipping_area;


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
        }
        else {
            Billing_state = cComponentPanel.cpshow.split('~')[6];
        }
        var bcity = cComponentPanel.cpshow.split('~')[7];
        if (bcity == '') {
            CmbCity.SetSelectedIndex(-1);
        }
        else {
            Billing_city = cComponentPanel.cpshow.split('~')[7];
        }

        var bpin = cComponentPanel.cpshow.split('~')[8];
        if (bpin == '') {
            CmbPin.SetSelectedIndex(-1);
        }
        else {
            Billing_pin = cComponentPanel.cpshow.split('~')[8];
        }

        var barea = cComponentPanel.cpshow.split('~')[9];
        if (barea == '') {
            CmbArea.SetSelectedIndex(-1);
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
}
//kaushik-----24-02-2017-------
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
    s.SetValue(Billing_state);
    CmbCity.PerformCallback(CmbState.GetValue());
    Billing_state = 0;
}
function cmbshipstate_endcallback(s, e) {
    s.SetValue(Shipping_state);
    CmbCity1.PerformCallback(CmbState1.GetValue());
    Shipping_state = 0;
}

function cmbcity_endcallback(s, e) {
    s.SetValue(Billing_city);
    CmbPin.PerformCallback(CmbCity.GetValue());
    CmbArea.PerformCallback(CmbCity.GetValue());
    Billing_city = 0;

}
function cmbshipcity_endcallback(s, e) {
    s.SetValue(Shipping_city);
    CmbPin1.PerformCallback(CmbCity1.GetValue());
    CmbArea1.PerformCallback(CmbCity1.GetValue());
    Shipping_city = 0;

}

function cmbPin_endcallback(s, e) {
    s.SetValue(Billing_pin);
    Billing_pin = 0;
}
function cmbshipPin_endcallback(s, e) {
    s.SetValue(Shipping_pin);
    Shipping_pin = 0;
}

function cmbArea_endcallback(s, e) {
    s.SetValue(billing_area);
    billing_area = 0;
}

function cmbshipArea_endcallback(s, e) {
    s.SetValue(Shipping_area);
    Shipping_area = 0;
}

function Popup_SalesQuote_EndCallBack() {
    if (Popup_SalesQuote.cpshow != null) {


        CmbAddressType.SetText(Popup_SalesQuote.cpshow.split('~')[0]);
        ctxtAddress1.SetText(Popup_SalesQuote.cpshow.split('~')[1]);
        ctxtAddress2.SetText(Popup_SalesQuote.cpshow.split('~')[2]);
        ctxtAddress3.SetText(Popup_SalesQuote.cpshow.split('~')[3]);
        ctxtlandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
        CmbCountry.SetValue(Popup_SalesQuote.cpshow.split('~')[5]);
        Billing_state = Popup_SalesQuote.cpshow.split('~')[6];
        Billing_city = Popup_SalesQuote.cpshow.split('~')[7];
        Billing_pin = Popup_SalesQuote.cpshow.split('~')[8];
        billing_area = Popup_SalesQuote.cpshow.split('~')[9];
        CmbState.PerformCallback(CmbCountry.GetValue());
    }

    if (Popup_SalesQuote.cpshowShip != null) {


        CmbAddressType1.SetText(Popup_SalesQuote.cpshowShip.split('~')[0]);
        ctxtsAddress1.SetText(Popup_SalesQuote.cpshowShip.split('~')[1]);
        ctxtsAddress2.SetText(Popup_SalesQuote.cpshowShip.split('~')[2]);
        ctxtsAddress3.SetText(Popup_SalesQuote.cpshowShip.split('~')[3]);
        ctxtslandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
        CmbCountry1.SetValue(Popup_SalesQuote.cpshowShip.split('~')[5]);
        Shipping_state = Popup_SalesQuote.cpshowShip.split('~')[6];
        Shipping_city = Popup_SalesQuote.cpshowShip.split('~')[7];
        Shipping_pin = Popup_SalesQuote.cpshowShip.split('~')[8];
        Shipping_area = Popup_SalesQuote.cpshowShip.split('~')[9];
        CmbState1.PerformCallback(CmbCountry1.GetValue());
    }

}

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


//<%--    function Currency_Rate() {

//    var Campany_ID = '<%=Session["LastCompany"]%>';
//    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
//    var basedCurrency = LocalCurrency.split("~");
//    var Currency_ID = cCmbCurrency.GetValue();
//    $('#<%=hdnCurrenctId.ClientID %>').val(Currency_ID);


//    if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
//        ctxtRate.SetValue("");
//        ctxtRate.SetEnabled(false);
//    }
//    else {
//        $.ajax({
//            type: "POST",
//            url: "ContraVoucher.aspx/GetRate",
//            data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (msg) {
//                var data = msg.d;
//                ctxtRate.SetValue(data);


//            }
//        });

//        ctxtRate.SetEnabled(true);
//    }
//}--%>

//function lookup_Click() {
//    debugger;

//}



function OrderNumberChanged() {

    var IsBranchIdSame = true;
    var RKeys = '';
    //var ReqKeys = gridSalesOrderLookup.gridView.GetSelectedKeysOnPage();
    var ReqKeys = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
    //for (var i = 0; i < ReqKeys.length  ; i++) {
    //    RKeys = RKeys + "," + ReqKeys[i];
    //}
    $.ajax({
        type: "POST",
        url: "BranchTransferOut.aspx/GetSameBranchId",
        data: JSON.stringify({ BranchReqNo: ReqKeys }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            //debugger;
            var data = msg.d;
            if (data === "") {
                IsBranchIdSame = false;

            }
        }
    });//End

    if (IsBranchIdSame == true) {
        var KeyVal;
        KeyVal = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
        //KeyVal = ReqKeys[0];

        //For TransferFrom and Transfer To Branch
        $.ajax({
            type: "POST",
            url: "BranchTransferOut.aspx/GetReqNoBranchDetails",
            data: JSON.stringify({ BranchReqNo: KeyVal }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var data = msg.d;
                var TransferTo = data.split('~')[1];
                var TransferFrom = data.split('~')[0];

                $('#ddl_transferFrom_Branch').val(TransferTo);
                $('#ddl_transferTo_Branch').val(TransferFrom);

                $("#ddl_transferFrom_Branch").prop("disabled", true);
                $("#ddl_transferTo_Branch").prop("disabled", true);
            }
        });//End
        if ($("#hdnProjectSelectInEntryModule").val() == "1")
            clookup_Project.gridView.Refresh();

        //var quote_Id = gridSalesOrderLookup.GetValue();
        var quote_Id = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
        //var quote_Id = ReqKeys[0];

        if (quote_Id != null) {
            cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);
            cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@' + '~' + KeyVal);
            cProductsPopup.Show();
        }
        else {
            cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$' + '~' + KeyVal);
            cProductsPopup.Show();
        }

    }
    else {
        jAlert("You can select Branch Requisition of Same Branch only.", "Branch Selection");
        //gridSalesOrderLookup.gridView.UnselectRows();
        //gridSalesOrderLookup.gridView.Refresh;
    }


}


function GridCallBack() {
    grid.PerformCallback('Display');
}


function ChangeState(value) {
    //debugger;
    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}

function BindOrderProjectdata(OrderId) {

    var OtherDetail = {};

    OtherDetail.OrderId = OrderId;
    // OtherDetail.TagDocType = "BR_Trans_OUT";


    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "BranchTransferOut.aspx/SetProjectCode",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var Code = msg.d;
                if (Code.length > 0) {
                    clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                    clookup_Project.SetEnabled(false);
                }
            }
        });
    }
}

function PerformCallToGridBind() {
    //debugger;
    $("#hddnSaveOrExitButton").val('');
    $("#hdnRefreshType").val('');
    grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
    //cSalesOrderComponentPanel.PerformCallback('BindOrderLookupOnSelection');
    cProductsPopup.Hide();
    $('#hdnPageStatus').val('Quoteupdate');
    clookup_Project.SetEnabled(false);
    // if ($("#hdnProjectSelectInEntryModule").val() == "1")
    // clookup_Project.gridView.Refresh();
    var quote_Id = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
    if (quote_Id.length > 0) {
        BindOrderProjectdata(quote_Id);
    }

    return false;
}


function componentEndCallBack(s, e) {
    //clookup_VehicleNo.PerformCallback('BindVehicleNo');
    gridSalesOrderLookup.gridView.Refresh();
    if (grid.GetVisibleRowsOnPage() == 0) {
        OnAddNewClick();
    }
}


//Code for UDF Control 
function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        // var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '';
        var keyval = $('#hdnmodeId').val();
        //  alert(keyval);
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=BSO&&KeyVal_InternalID=' + keyval;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}
// End Udf Code


var taxAmountGlobal;
function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}
//function taxAmountLostFocus(s, e) {
//    var finalTaxAmt = parseFloat(s.GetValue());
//    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
//    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
//    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
//    if (sign == '(+)') {
//        ctxtTaxTotAmt.SetValue(totAmt + finalTaxAmt - taxAmountGlobal);
//    } else {
//        ctxtTaxTotAmt.SetValue(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1));
//    }


//    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

//}

function cmbGstCstVatChange(s, e) {

    SetOtherTaxValueOnRespectiveRow(0, 0, gstcstvatGlobalName);

    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());

    var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVat.GetValue().split('~')[1]) / 100;
    ctxtGstCstVat.SetValue(calculatedValue);

    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    ctxtTaxTotAmt.SetValue(totAmt + calculatedValue - GlobalCurTaxAmt);

    //tax others
    SetOtherTaxValueOnRespectiveRow(0, calculatedValue, ccmbGstCstVat.GetText());
    gstcstvatGlobalName = ccmbGstCstVat.GetText();
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
    //console.log(s);
    //console.log(e);
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

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
        } else {
            s.SetText("");
        }
    }

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
function acpAvailableStockEndCall(s, e) {
    if (cacpAvailableStock.cpstock != null) {
        var AvlStk = cacpAvailableStock.cpstock + " " + $('#lblStkUOM').val();
        // <%--document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = AvlStk;--%>
        $('#lblAvailableSStk').val(AvlStk);
        $('#lblAvailableStock').val(cacpAvailableStock.cpstock);
        $('#lblAvailableStockUOM').val($('#lblStkUOM').val());
        cCmbWarehouse.cpstock = null;
        // grid.batchEditApi.StartEdit(globalRowIndex, 5);
        return;
    }



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
        var AltQty = cCallbackPanel.cpEdit.split('~')[4];
        var AltUOM = cCallbackPanel.cpEdit.split('~')[5];

        SelectWarehouse = strWarehouse;
        SelectBatch = strBatchID;
        SelectSerial = strSrlID;

        cCmbWarehouse.PerformCallback('BindWarehouse');
        cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
        checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

        cCmbWarehouse.SetValue(strWarehouse);
        ctxtQuantity.SetValue(strQuantity);
        ctxtAltQuantity.SetValue(AltQty);
        ccmbPackingUom1.SetValue(AltUOM);
    }
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
            url: "BranchTransferOut.aspx/GetPackingQuantityWarehouse",
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

    var setting = document.getElementById("hdnShowUOMConversionInEntry").value;        
    if (setting == 1) {
        ctxtAltQuantity.SetText(calcQuantity);
        ChkDataDigitCount(Quantity);
    }
  }
}
function ChangeQuantityByPacking1() {
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
            url: "BranchTransferOut.aspx/GetPackingQuantityWarehouse",
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
        var packing = ctxtAltQuantity.GetValue();
        var Quantity = ctxtQuantity.GetValue();
        if (packing == null || packing == '') {
            $(e).val(parseFloat(0).toFixed(4));
            packing = ctxtAltQuantity.GetValue();
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
