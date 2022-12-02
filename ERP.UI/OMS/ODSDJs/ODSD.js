var GetHRLVars=function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

var ONDDLVARGST=function Onddl_VatGstCstEndCallback(s, e) {
    if (s.GetItemCount() == 1) {
        cddlVatGstCst.SetEnabled(false);
    }
}
var isCtrl = false;
document.onkeyup = function (e) {
    if (event.keyCode == 17) {
        isCtrl = false;
    }
    else if (event.keyCode == 27) {
        btnCancel_Click();
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

var StopDefaultAction=function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}

var gridFocusedRowChanged= function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}

var ProductKeyDown=function ProductKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {

        s.OnButtonClick(0);
    }
    if (e.htmlEvent.key == "Tab") {

        s.OnButtonClick(0);
    }
}

var fn_Edit=function fn_Edit(keyValue) {

    //cGrdWarehouse.PerformCallback('EditWarehouse~' + keyValue);
    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}

var ProductButnClick= function ProductButnClick(s, e) {

    //cproductLookUp.gridView.Refresh();
    if (e.buttonIndex == 0) {

        if (cproductLookUp.Clear()) {

            //var IsInventoryValue = ccmbIsInventory.GetValue();
            //cIsInventory.PerformCallback('BindSession' + '~' + IsInventoryValue);

            cProductpopUp.Show();
            cproductLookUp.Focus();
            cproductLookUp.ShowDropDown();


        }
    }
}
var PrioductLost=function ProductLostFocused(s, e) {

    grid.GetEditor("Quantity").Focus();
}


var cmbContactPersonEndCall=function cmbContactPersonEndCall(s, e) {

    if (cContactPerson.cpDueDate != null) {
        var DeuDate = cContactPerson.cpDueDate;
        var myDate = new Date(DeuDate);

        var invoiceDate = new Date();
        var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));

        ctxtCreditDays.SetValue(datediff);

        cdt_SaleInvoiceDue.SetDate(myDate);
        cContactPerson.cpDueDate = null;
    }

    if (cContactPerson.cpTotalDue != null) {
        var TotalDue = cContactPerson.cpTotalDue;
        if ((TotalDue * 1) < 0) {
            document.getElementById('<%=lblTotalDues.ClientID %>').innerHTML = (TotalDue * (-1)) + " " + "Cr";
            document.getElementById('<%=lblTotalDues.ClientID %>').style.color = "red";
        }
        else {
            document.getElementById('<%=lblTotalDues.ClientID %>').innerHTML = TotalDue + " " + "Db";
        }

        pageheaderContent.style.display = "block";
        divDues.style.display = "block";
        cContactPerson.cpTotalDue = null;
    }
}

var cmbContactPersonEndCall=function acbpCrpUdfEndCall(s, e) {


    if (cacbpCrpUdf.cpUDF) {


        if (cacbpCrpUdf.cpUDF == "true" && cacbpCrpUdf.cpTransport == "true" && cacbpCrpUdf.cpTC == "true") {
            grid.UpdateEdit();
            cacbpCrpUdf.cpUDF = null;
            cacbpCrpUdf.cpTransport = null;
            cacbpCrpUdf.cpTC = null;
        }
        else if (cacbpCrpUdf.cpUDF == "false") {
            jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
            cacbpCrpUdf.cpUDF = null;
            cacbpCrpUdf.cpTransport = null;
            cacbpCrpUdf.cpTC = null;
        }
        else if (cacbpCrpUdf.cpTC == "false") {
            jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
            cacbpCrpUdf.cpUDF = null;
            cacbpCrpUdf.cpTransport = null;
            cacbpCrpUdf.cpTC = null;
        }
        else {
            jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
            cacbpCrpUdf.cpUDF = null;
            cacbpCrpUdf.cpTransport = null;
            cacbpCrpUdf.cpTC = null;
        }

    }
}

var CreditDays_TextChanged=function CreditDays_TextChanged(s, e) {

    var CreditDays = ctxtCreditDays.GetValue();

    var today = new Date();
    var newdate = new Date();
    newdate.setDate(today.getDate() + Math.round(CreditDays));

    cdt_SaleInvoiceDue.SetDate(newdate);
}


function ddlInventory_OnChange() {
    //cproductLookUp.GetGridView().Refresh();
}

var ProductSelected= function ProductSelected(s, e) {
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

        $('#<%= lblStkQty.ClientID %>').text("0.00");
        $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
        cacpAvailableStock.PerformCallback(strProductID);
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }

}

var SalePriceTextChange=function SalePriceTextChange(s, e) {

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

    var Amount = QuantityValue * strFactor * ((Saleprice.replace(/\,/g, '') * 1) / strRate);
    var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

    var tbAmount = grid.GetEditor("Amount");
    tbAmount.SetValue(amountAfterDiscount);


    var TotaAmountRes = '';
    TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));
    //tbTotalAmount.SetValue(amountAfterDiscount);


    //Debjyoti section GST
    var ShippingStateCode = cbsSCmbState.GetValue();
    var TaxType = "";
    if (cddl_AmountAre.GetValue() == "1") {
        TaxType = "E";
    }
    else if (cddl_AmountAre.GetValue() == "2") {
        TaxType = "I";
    }

    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
        SpliteDetails[17], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val());
    //END


}

//'Subhabrata' on 15-03-2017
var CmbWarehouseEndCallback=function CmbWarehouseEndCallback(s, e) {
    if (SelectWarehouse != "0") {
        cCmbWarehouse.SetValue(SelectWarehouse);
        SelectWarehouse = "0";
    }
    else {
        cCmbWarehouse.SetEnabled(true);
    }
}

var CmbBatchEndCall=function CmbBatchEndCall(s, e) {
    if (SelectBatch != "0") {
        cCmbBatch.SetValue(SelectBatch);
        SelectBatch = "0";
    }
    else {
        cCmbBatch.SetEnabled(true);
    }
}

var listBoxEndCall=function listBoxEndCall(s, e) {
    //debugger;
    var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();
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
    var msg = checkListBox.cpFifoMsg;
    if (msg == 'Check Not Possible') {
        alert('NA');
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

   

var ctaxUpdatePanelEndCall=function ctaxUpdatePanelEndCall(s, e) {
    if (ctaxUpdatePanel.cpstock != null) {
        divAvailableStk.style.display = "block";
        divpopupAvailableStock.style.display = "block";

        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
        document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;
        document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;

        ctaxUpdatePanel.cpstock = null;
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
        return false;
    }
}
//End
var DiscountTextChange=function DiscountTextChange(s, e) {
    //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

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

    var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

    var tbAmount = grid.GetEditor("Amount");
    tbAmount.SetValue(amountAfterDiscount);

    var TotaAmountRes = '';
    TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));
    //tbTotalAmount.SetValue(amountAfterDiscount);

    //Debjyoti section GST
    var ShippingStateCode = cbsSCmbState.GetValue();
    var TaxType = "";
    if (cddl_AmountAre.GetValue() == "1") {
        TaxType = "E";
    }
    else if (cddl_AmountAre.GetValue() == "2") {
        TaxType = "I";
    }

    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
        SpliteDetails[17], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val());
    //END


    //Debjyoti 
    //grid.GetEditor('TaxAmount').SetValue(0);
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
}


var CmbScheme_ValueChange=function CmbScheme_ValueChange() {

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

var ddl_Currency_Rate_Change=function ddl_Currency_Rate_Change() {

    var Campany_ID = '<%=Session["LastCompany"]%>';
    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
    var basedCurrency = LocalCurrency.split("~");
    var Currency_ID = $("#ddl_Currency").val();


    if ($("#ddl_Currency").text().trim() == basedCurrency[1]) {
        ctxtRate.SetValue("");
        ctxtRate.SetEnabled(false);
    }
    else {
        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/GetRate",
            data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var data = msg.d;
                ctxtRate.SetValue(data);
            }
        });
        ctxtRate.SetEnabled(true);
    }
}
var CmbWarehouse_ValueChange=function CmbWarehouse_ValueChange() {
    var isFIFORequired = false;

    var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();

    var WarehouseID = cCmbWarehouse.GetValue();
    $("#<%=hddnWarehouseId.ClientID%>").val(WarehouseID);
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
var CmbBatch_ValueChange=function CmbBatch_ValueChange() {
    //debugger;
    var WarehouseID = cCmbWarehouse.GetValue();
    var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();


    var BatchID = cCmbBatch.GetValue();
    ctxtMatchQty.SetValue(0);
    $("#<%=hddnBatchId.ClientID%>").val(BatchID);
    var type = document.getElementById('hdfProductType').value;
    //var qty = grid.GetEditor('Quantity').GetValue();
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
//tab start

//tab end


$(document).ready(function () {
    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
        page.GetTabByName('Billing/Shipping').SetEnabled(false);
    }
    $('#ApprovalCross').click(function () {

        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.Refresh();
    })

    //var IsInventoryValue = ccmbIsInventory.GetValue();
    //cIsInventory.PerformCallback('BindSession' + '~' + IsInventoryValue);
})

var UniqueCodeCheck=function UniqueCodeCheck() {

    var SchemeVal = $('#<%=ddl_numberingScheme.ClientID %> option:selected').val();
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
                    url: "SalesChallanAdd.aspx/CheckUniqueCode",
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



var DateCheck=function DateCheck() {
    var startDate;
    if (gridSalesOrderLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                page.SetActiveTabIndex(0);
                ccmbGstCstVat.PerformCallback();
                ccmbGstCstVatcharge.PerformCallback();
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');

                startDate = cPLSalesChallanDate.GetValueString();


                var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                if (key != null && key != '') {
                    if (type != '' && type != null) {
                        cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                    }
                    //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                }
                grid.PerformCallback('GridBlank');
            }

        });
    }
    else {
        page.SetActiveTabIndex(0);
        ccmbGstCstVat.PerformCallback();
        ccmbGstCstVatcharge.PerformCallback();
        ctaxUpdatePanel.PerformCallback('DeleteAllTax');

        startDate = cPLSalesChallanDate.GetValueString();


        var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
        cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
        if (key != null && key != '') {
            if (type != '' && type != null) {
                cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
            }
            //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

        }
        grid.PerformCallback('GridBlank');
    }
    gridSalesOrderLookup.gridView.Refresh();
    cProductsPopup.Hide();
}

var CloseGridQuotationLookup=function CloseGridQuotationLookup() {
    gridSalesOrderLookup.ConfirmCurrentSelection();
    gridSalesOrderLookup.HideDropDown();
    gridSalesOrderLookup.Focus();
}
var GlobalCurTaxAmt = 0;
var rowEditCtrl;
var globalRowIndex;
var GetVisibleIndex=function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}

var cmbtaxCodeindexChange=function cmbtaxCodeindexChange(s, e) {
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

var CmbtaxClick=function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
}


var txtTax_TextChanged=function txtTax_TextChanged(s, i, e) {


    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);




}
//Subhabrata Tax
var RecalCulateTaxTotalAmountCharges=function RecalCulateTaxTotalAmountCharges() {
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

var Save_TaxClick=function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cPopup_Taxes.Hide();
}
var SetChargesRunningTotal=function SetChargesRunningTotal() {
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


var GetPercentageData=function GetPercentageData() {
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

var recalculateTax=function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}

var recalculateTaxCharge=function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}

var chargeCmbtaxClick=function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}
var GlobalCurChargeTaxAmt;
var ChargegstcstvatGlobalName;
var ChargecmbGstCstVatChange=function ChargecmbGstCstVatChange(s, e) {

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
var OnTaxEndCallback=function OnTaxEndCallback(s, e) {
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
var QuotationTaxAmountGotFocus= function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}



var QuotationTaxAmountTextChange=function QuotationTaxAmountTextChange(s, e) {
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



var PercentageTextChange= function PercentageTextChange(s, e) {
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


var Save_TaxesClick=function Save_TaxesClick() {
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
var ShowTaxPopUp=function ShowTaxPopUp(type) {
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

var BatchUpdate=function BatchUpdate() {

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

var taxAmtButnClick=function taxAmtButnClick(s, e) {

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
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                //var strRate = "1";
                var strStkUOM = SpliteDetails[4];
                // var strSalePrice = SpliteDetails[6];
                var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                if (strRate == 0) {
                    strRate = 1;
                }

                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = Math.round(QuantityValue * strFactor * ((strSalePrice.replace(/\,/g, '') * 1) / strRate)).toFixed(2);
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

                    var shippingStCode = '';

                    ////###### Added By : Samrat Roy ##########
                    //Get Customer Shipping StateCode
                    var shippingStCode = '';

                    var shippingStCode = '';
                    shippingStCode = cbsSCmbState.GetText();
                    shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

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

var taxAmountLostFocus=function taxAmountLostFocus(s, e) {
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

var SetRunningTotal=function SetRunningTotal() {
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

var GetTotalRunningAmount=function GetTotalRunningAmount() {
    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        cgridTax.batchEditApi.EndEdit();
    }

    return runningTot;
}

var taxAmountGlobal;
var taxAmountGotFocus=function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}

var taxJson;
var cgridTax_EndCallBack=function cgridTax_EndCallBack(s, e) {
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
        // grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));
        var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue());
        var totalRoundOffAmount = Math.round(totalNetAmount);
        // grid.GetEditor("TaxAmount").SetValue(totalRoundOffAmount);

        grid.GetEditor("Amount").SetValue(parseFloat(grid.GetEditor("Amount").GetValue()) + (totalRoundOffAmount - totalNetAmount));

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

var GetTaxVisibleIndex=function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
}

var cmbGstCstVatChange=function cmbGstCstVatChange(s, e) {

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
var CmbtaxClick=function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}


var txtPercentageLostFocus=function txtPercentageLostFocus(s, e) {

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

var taxAmtButnClick1=function taxAmtButnClick1(s, e) {
    rowEditCtrl = s;
}






$(document).ready(function () {

    ctxtRate.SetValue("");
    ctxtRate.SetEnabled(false);
    ctxt_SlChallanNo.SetEnabled(false);
    gridSalesOrderLookup.SetEnabled(false);

    PopulateLoadGSTCSTVAT();
});


       //window.onload = function () {
       //    // grid.AddNewRow();
       //    
       //    OnAddNewClick();
       //};
       var QuantityRes = '';
function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
    gridSalesOrderLookup.SetEnabled(true);
}
function GetContactPerson(e) {


    var startDate = new Date();
    startDate = cPLSalesChallanDate.GetValueString();
    if (gridSalesOrderLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

            if (r == true) {
                var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                GetObjectID('hdnCustomerId').value = key;
                if (key != null && key != '') {

                    if (type != '' && type != null) {
                        cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                    }
                    grid.PerformCallback('GridBlank');
                    //var startDate = new Date();
                    //startDate = cPLSalesChallanDate.GetValueString();

                    cContactPerson.PerformCallback('BindContactPerson~' + key);


                    page.GetTabByName('Billing/Shipping').SetEnabled(true);

                    //###### Added By : Samrat Roy ##########
                    //cchkBilling.SetChecked(false);
                    //cchkShipping.SetChecked(false);
                    //page.SetActiveTabIndex(1);
                    //$('.dxeErrorCellSys').addClass('abc');
                    ////$('.crossBtn').hide();
                    //page.GetTabByName('Billing/Shipping').SetEnabled(true);
                    //page.GetTabByName('General').SetEnabled(false);

                    LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SC');
                    GetObjectID('hdnCustomerId').value = key;
                    if ($('#hfBSAlertFlag').val() == "1") {
                        jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                            if (r == true) {
                                page.SetActiveTabIndex(1);
                                cbsSave_BillingShipping.Focus();
                                page.tabs[0].SetEnabled(false);
                                $("#divcross").hide();
                            }
                        });
                    }
                    else {
                        page.SetActiveTabIndex(1);
                        cbsSave_BillingShipping.Focus();
                        page.tabs[0].SetEnabled(false);
                        $("#divcross").hide();
                    }
                    //###### END : Samrat Roy : END ########## 


                    billingLookup.focus();

                    //else {
                    //    page.SetActiveTabIndex(0);
                    //}
                    //});

                    GetObjectID('hdnAddressDtl').value = '0';
                    //$("#<%=ddl_SalesAgent.ClientID%>").focus();
                    //document.getElementById('popup_ok').focus();
                }
            }
        });
    }
    else {
        var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
        GetObjectID('hdnCustomerId').value = key;
        if (key != null && key != '') {


            //var startDate = new Date();
            //startDate = cPLSalesChallanDate.GetValueString();

            cContactPerson.PerformCallback('BindContactPerson~' + key);


            //page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            //jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
            //    if (r == true) {
            page.SetActiveTabIndex(1);
            $('.dxeErrorCellSys').addClass('abc');
            //$('.crossBtn').hide();
            //page.GetTabByName('[B]illing/Shipping').SetEnabled(true);

            page.SetActiveTabIndex(1);
            $('.dxeErrorCellSys').addClass('abc');
            //$('.crossBtn').hide();
            page.GetTabByName('Billing/Shipping').SetEnabled(true);
            page.GetTabByName('General').SetEnabled(false);
            //        billingLookup.focus();
            //    }
            //});
            if (type != '' && type != null) {
                cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
            }

            //###### Added By : Samrat Roy ##########
            //cchkBilling.SetChecked(false);
            //cchkShipping.SetChecked(false);
            //page.SetActiveTabIndex(1);
            //$('.dxeErrorCellSys').addClass('abc');
            ////$('.crossBtn').hide();
            //page.GetTabByName('Billing/Shipping').SetEnabled(true);
            //page.GetTabByName('General').SetEnabled(false);

            LoadCustomerAddress(key, $('#ddl_Branch').val());
            GetObjectID('hdnCustomerId').value = key;
            if ($('#hfBSAlertFlag').val() == "1") {
                jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        page.SetActiveTabIndex(1);
                        cbsSave_BillingShipping.Focus();
                        page.tabs[0].SetEnabled(false);
                        $("#divcross").hide();
                    }
                });
            }
            else {
                page.SetActiveTabIndex(1);
                cbsSave_BillingShipping.Focus();
                page.tabs[0].SetEnabled(false);
                $("#divcross").hide();
            }
            //###### END : Samrat Roy : END ########## 


            GetObjectID('hdnAddressDtl').value = '0';
            //$("#<%=ddl_SalesAgent.ClientID%>").focus();
            //document.getElementById('popup_ok').focus();
        }
    }
    gridSalesOrderLookup.gridView.Refresh();
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

$(document).ready(function () {
    //debugger;
    var CustomerDelivery = $("#<%=hddnCustomerDelivery.ClientID%>").val();
    var ddl_numbering = $('#<%=ddl_numberingScheme.ClientID%>').val();
    var BillValue = $("#<%=hddnBillId.ClientID%>").val();

    if (CustomerDelivery == 'Yes') {

        LoadingPanel.Show();

        if (ddl_numbering != '' && ddl_numbering != undefined) {
            var NoSchemeType = ddl_numbering.toString().split('~')[1];
            var BranchId = ddl_numbering.toString().split('~')[3];

            if (NoSchemeType == '1') {
                ctxt_SlChallanNo.SetText('Auto');
                ctxt_SlChallanNo.SetEnabled(false);
                cPLSalesChallanDate.Focus();

            }
            else if (NoSchemeType == '0') {
                ctxt_SlChallanNo.SetText('');
                ctxt_SlChallanNo.SetEnabled(true);
                ctxt_SlChallanNo.Focus();

            }

            $("#<%=ddl_Branch.ClientID%>").val(BranchId);
            $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);
        }

        //###### Added By : Samrat Roy ##########


        //LoadCustomerAddress($('#hdnCustomerId').val(), $('#ddl_Branch').val(), 'SI');
        //BSDocTagging(BillValue, 'SI');

        //###### END : Samrat Roy : END ########## 

    }
    else if (CustomerDelivery == 'No') {
        var ddl_numbering = $('#<%=ddl_numberingScheme.ClientID%>').val();
        if (ddl_numbering != '' && ddl_numbering != undefined) {
            var NoSchemeType = ddl_numbering.toString().split('~')[1];
            var BranchIdPending = ddl_numbering.toString().split('~')[3];

            if (NoSchemeType == '1') {
                ctxt_SlChallanNo.SetText('Auto');
                ctxt_SlChallanNo.SetEnabled(false);
                cPLSalesChallanDate.Focus();

            }
            else if (NoSchemeType == '0') {
                //ctxt_SlChallanNo.SetText('');
                //ctxt_SlChallanNo.SetEnabled(true);
                ctxt_SlChallanNo.Focus();

            }

            $("#<%=ddl_Branch.ClientID%>").val(BranchIdPending);
            $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);
            BSDocTagging(BillValue, 'SI');
            if ($("#btn_TermsCondition").is(":visible")) {
                callTCControl(BillValue, 'SI');
            }
        }
    }

    $("#<%=hddnBranchId.ClientID%>").val($("#<%=ddl_Branch.ClientID%>").val());

    $("#<%=ddl_Branch.ClientID%>").change(function () {

        var startDate;
        var ddl_BranchId;
        if (gridSalesOrderLookup.GetValue() != null) {
            jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    page.SetActiveTabIndex(0);
                    ccmbGstCstVat.PerformCallback();
                    ccmbGstCstVatcharge.PerformCallback();
                    ctaxUpdatePanel.PerformCallback('DeleteAllTax');

                    startDate = cPLSalesChallanDate.GetValueString();
                    //ddl_BranchId = $("<%=ddl_Branch.ClientID%>").val();

                    var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                    cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    if (key != null && key != '') {
                        if (type != '' && type != null) {
                            cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                        }
                        //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                    }
                    grid.PerformCallback('GridBlank');
                }
                else {
                    $("#<%=ddl_Branch.ClientID%>").val($("#<%=hddnBranchId.ClientID%>").val());
                }
            });
        }
        else {
            page.SetActiveTabIndex(0);
            ccmbGstCstVat.PerformCallback();
            ccmbGstCstVatcharge.PerformCallback();
            ctaxUpdatePanel.PerformCallback('DeleteAllTax');

            startDate = cPLSalesChallanDate.GetValueString();


            var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
            cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
            if (key != null && key != '') {
                if (type != '' && type != null) {
                    cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                }
                //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

            }
            grid.PerformCallback('GridBlank');
        }
    });

    //$('#ddl_numberingScheme').change(function () {

    //    var NoSchemeTypedtl = $(this).val();
    //    var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
    //    var BranchId = NoSchemeTypedtl.toString().split('~')[3];

    //    if (NoSchemeType == '1') {
    //        ctxt_SlChallanNo.SetText('Auto');
    //        ctxt_SlChallanNo.SetEnabled(false);
    //        cPLSalesChallanDate.Focus();
    //        //   document.getElementById('<%= txt_SlChallanNo.ClientID %>').disabled = true;

    //    }
    //    else if (NoSchemeType == '0') {
    //        ctxt_SlChallanNo.SetText('');
    //        ctxt_SlChallanNo.SetEnabled(true);
    //        ctxt_SlChallanNo.Focus();

    //    }
    //    else {
    //        ctxt_SlChallanNo.SetText('');
    //        ctxt_SlChallanNo.SetEnabled(false);
    //        document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();

    //    }


    //    $("#<%=ddl_Branch.ClientID%>").val(BranchId);
    //    $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);

    //    //gridLookup.SetText('');
    //});

    //$("#<%=ddl_SalesAgent.ClientID%>").change(function () {

    //    $("#<%=ddl_Branch.ClientID%>").focus();
    //});


    //$('#ddl_Currency').change(function () {
    //    var CurrencyId = $(this).val();
    //    var ActiveCurrency = '<%=Session["ActiveCurrency"]%>'
    //    var Currency = ActiveCurrency.toString().split('~')[0];
    //    if (ActiveCurrency != null) {
    //        if (CurrencyId != '0') {
    //            $.ajax({
    //                type: "POST",
    //                url: "SalesQuotation.aspx/GetCurrentConvertedRate",
    //                data: "{'CurrencyId':'" + CurrencyId + "'}",
    //                contentType: "application/json; charset=utf-8",
    //                dataType: "json",
    //                success: function (msg) {
    //                    var currentRate = msg.d;
    //                    $('#txt_Rate').text(currentRate);
    //                }
    //            });
    //        }
    //        else {
    //            $('#txt_Rate').text('');
    //        }
    //    }
    //});
});

var PopulateGSTCSTVAT=function PopulateGSTCSTVAT(e) {
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
        cddlVatGstCst.Focus();
    }
    else if (key == 3) {
        grid.GetEditor('TaxAmount').SetEnabled(false);
        cddlVatGstCst.SetEnabled(false);
        // cddlVatGstCst.PerformCallback('3');
        cddlVatGstCst.SetSelectedIndex(0);

    }

}

var PopulateLoadGSTCSTVAT=function PopulateLoadGSTCSTVAT() {
    cddlVatGstCst.SetEnabled(false);
}



var showQuotationDocument=function showQuotationDocument() {
    var URL = "Contact_Document.aspx?requesttype=" + Quotation + "";
    window.location.href = URL;
}


// Popup Section

var ShowCustom=function ShowCustom() {

    cPopup_wareHouse.Show();


}

// Popup Section End



    var currentEditableVisibleIndex;
var preventEndEditOnLostFocus = false;
var lastProductID;
var setValueFlag;
var QuantityResultant = '';
//function ProductsCombo_SelectedIndexChanged(s, e) {
//    
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

var ProductsCombo_SelectedIndexChanged=function ProductsCombo_SelectedIndexChanged(s, e) {
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

    $('#<%= lblStkQty.ClientID %>').text("0.00");
    $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
    cacpAvailableStock.PerformCallback(strProductID);
    //tbStkUOM.SetValue(strStkUOM);
    //tbStockQuantity.SetValue("0");
}



var OnEndCallback=function OnEndCallback(s, e) {
    //debugger;
    //$('#MandatoryEwayBillNo').attr('style', 'display:none');
    LoadingPanel.Hide();
    var value = document.getElementById('hdnRefreshType').value;

    if (grid.cpSaveSuccessOrFail == "outrange") {
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');
        grid.cpSaveSuccessOrFail = null;
    }
    else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
        OnAddNewClick();
        jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
        grid.cpSaveSuccessOrFail = null;
    }
    else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
        OnAddNewClick();
        jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
        grid.cpSaveSuccessOrFail = null;
    }
    else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
        //debugger;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Cannot select same product in multiple rows.');
        grid.cpSaveSuccessOrFail = null;
    }
    else if (grid.cpProductZeroStock == "ZeroStock") {
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Insufficient Avaialble Stock.Cannot proceed');
        grid.cpProductZeroStock = null;
    }
    else if (grid.cpProductNotExists == "Select Product First") {


        if (grid.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();


        }
        grid.batchEditApi.StartEdit(0, 1);
        grid.GetEditor('ProductID').Focus();
        jAlert('Select Product First');
        grid.cpProductNotExists = null;
    }
    else if (grid.cpIsQtyNotExists == "QtyNotExists") {

        jAlert('Enter Quantity First');
        grid.GetEditor('Quantity').Focus();
        grid.cpIsQtyNotExists = null;
    }
    else if (grid.cpSaveSuccessOrFail == "errorInsert") {
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Please try again later.');
        grid.cpSaveSuccessOrFail = null;
    }
    else if (grid.cpSaveSuccessOrFail == "udfNotSaved") {
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('UDF is set as Mandatory. Please enter values.', 'Alert', function () { OpenUdf(); });
        grid.cpSaveSuccessOrFail = null;
    }
    else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
        OnAddNewClick();
        //grid.cpSaveSuccessOrFail = null;

        //grid.AddNewRow();

        //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        //var i;
        //var cnt = 1;
        //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        //    var tbQuotation = grid.GetEditor("SrlNo");
        //    tbQuotation.SetValue(cnt);


        //    cnt++;
        //}

        var SrlNo = grid.cpProductSrlIDCheck;
        var msg = "Qty is entered for product [" + SrlNo + "] but Stock Details not updated.Cannot proceed.";
        jAlert(msg);
        grid.cpSaveSuccessOrFail = null;
        grid.cpProductSrlIDCheck = null;
    }
    else if (grid.cpSaveSuccessOrFail == "checkWarehouseQty") {
        //debugger;
        //OnAddNewClick();
        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i;
        var cnt = 1;
        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(cnt);


            cnt++;
        }
        //MoreThanStock
        //grid.batchEditApi.StartEdit(0, 1);
        grid.cpSaveSuccessOrFail = null;

        var SrlNo = grid.cpProductSrlIDCheck1;
        var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
        jAlert(msg);
        grid.cpSaveSuccessOrFail = null;
        grid.cpProductSrlIDCheck1 = null;
    }
    else if (grid.cpSaveSuccessOrFail == "MoreThanStock") {
        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i;
        var cnt = 1;
        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(cnt);


            cnt++;
        }
        grid.cpSaveSuccessOrFail = null;
        var msg = "Product entered quantity more than stock quantity.Can not proceed.";
        jAlert(msg);

    }
    else if (grid.cpSaveSuccessOrFail == "BillingShippingBlank") {
        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i;
        var cnt = 1;
        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(cnt);


            cnt++;
        }

        grid.cpSaveSuccessOrFail = null;
        var msg = "No Billing Shipping Entered.";
        jAlert(msg);
    }
    else if (grid.cpProductTotalAmountEway == "ExceedsEway") {
        //grid.batchEditApi.StartEdit(0, 2);
        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i;
        var cnt = 1;
        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(cnt);


            cnt++;
        }
        var msg = "Total Amount Exceeded Rs. 50000.You have to enter Eway Bill.";

        jAlert(msg);
        $('#MandatoryEwayBillNo').attr('style', 'display:block');
        grid.cpProductTotalAmountEway = null;

    }
    else {
        //var Quote_Number = grid.cpQuotationNo;
        //var Quote_Msg = "Sales Quotation No. " + Quote_Number + " generated.";
        var SalesOrder_Number = grid.cpSalesOrderNo;
        //$("#hfDocId").val(SalesOrder_Number);
        //$("#hfDocType").val("SC");
        //var DirectMsg = grid.cpSalesOrderExitOnCustomerDelivery;
        var Order_Msg = "Sales Challan No. " + SalesOrder_Number + " saved.";
        if (value == "E") {
            //window.location.assign("SalesChallan.aspx");
            if (grid.cpApproverStatus == "approve") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else if (grid.cpApproverStatus == "rejected") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }

            if (SalesOrder_Number != "") {
                var ODSD = $("#<%=hddnCustomerDeliverySDOrOD.ClientID%>").val();
                if (grid.cpSalesOrderExitOnCustomerDelivery == "CustomerDelivery") {

                    //jAlert(Order_Msg);
                    grid.cpSalesOrderExitOnCustomerDelivery = null;
                            
                    if (ODSD == "0") {

                        jAlert(Order_Msg, 'Alert Dialog: [Customer Delivery OD]', function (r) {
                            if (r == true) {
                                grid.cpSalesOrderNo = null;
                                window.location.assign("CustomerDeliveryPendingList.aspx");
                            }
                        });
                    }
                    else if (ODSD == "1") {
                        jAlert(Order_Msg, 'Alert Dialog: [Customer Delivery SD]', function (r) {
                            if (r == true) {
                                grid.cpSalesOrderNo = null;
                                window.location.assign("CustomerDeliveryPendingList.aspx?type=SD");
                            }
                        });
                    }
                            


                    //AutoPrint
                    if ($("#<%=hddnCustomerDelivery.ClientID%>").val() == "Yes") {
                        if ($("#<%=hddnSaveOrExitButton.ClientID%>").val() == 'Save_Exit') {
                            var DocumentNo = grid.cpDocumentNo;
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesChallan~D&modulename=ODSDChallan&id=" + DocumentNo, '_blank');
                        }
                    }

                    //End
                }
                else if (grid.cpSalesOrderExitOnPendingDelivery == "PendingDelivery") {
                    grid.cpSalesOrderExitOnPendingDelivery = null;
                    jAlert(Order_Msg, 'Alert Dialog: [Pending Delivery]', function (r) {
                        if (r == true) {
                            grid.cpSalesOrderNo = null;
                            window.location.assign("PendingDeliveryList.aspx");
                        }
                    });
                }
                else if (ODSD == "4") {
                    jAlert(Order_Msg, 'Alert Dialog: [Second Hand Sales]', function (r) {
                        if (r == true) {
                            grid.cpSalesOrderNo = null;
                            window.location.assign("OldUnit_SalesInvoiceList.aspx");
                        }
                    });
                }

                else {
                    jAlert(Order_Msg, 'Alert Dialog: [SalesChallan]', function (r) {
                        if (r == true) {
                            grid.cpSalesOrderNo = null;
                            window.location.assign("SalesChallan.aspx");
                        }
                    });
                }

                //jAlert(Order_Msg);




            }
            else {
                window.location.assign("SalesChallan.aspx");
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
            // window.location.assign("SalesOrderAdd.aspx?key=ADD");


            if (SalesOrder_Number != "") {
                jAlert(Order_Msg, 'Alert Dialog: [SalesChallan]', function (r) {
                    //jAlert(Order_Msg);
                    grid.cpSalesOrderNo = null;
                    if (r == true) {
                        window.location.assign("SalesChallanAdd.aspx?key=ADD");
                    }
                });
                //jConfirm(Order_Msg, 'Confirmation Dialog', function (r) {
                //    if (r == true) {
                //        window.location.assign("SalesOrderAdd.aspx?key=ADD");
                //    }
                //    else
                //    { window.location.assign("SalesOrderAdd.aspx?key=ADD"); }
                //});

            }
            else {
                window.location.assign("SalesChallanAdd.aspx?key=ADD");
            }
        }
        else {

            var pageStatus = document.getElementById('hdnPageStatus').value;
            if (pageStatus == "first") {
                OnAddNewClick();
                grid.batchEditApi.EndEdit();
                $('#ddl_numberingScheme').focus();
                document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();
                $('#<%=hdnPageStatus.ClientID %>').val('');
            }
            else if (pageStatus == "update") {
                OnAddNewClick();
                $('#<%=hdnPageStatus.ClientID %>').val('');
            }
            else if (pageStatus == "Quoteupdate") {
                grid.StartEditRow(0);
                $('#<%=hdnPageStatus.ClientID %>').val('');
            }
            else if (pageStatus == "EditModeOnDirect") {
                grid.StartEditRow(0);
                $('#<%=hdnPageStatus.ClientID %>').val('');
            }
        }

    }
    var hddnPermission = $("#<%=hddnPermissionString.ClientID%>").val();//Subhabrata
    if (gridSalesOrderLookup.GetValue() != null) {
        if (hddnPermission == "0") {
            grid.GetEditor('ProductName').SetEnabled(false);
            grid.GetEditor('Description').SetEnabled(false);
            grid.GetEditor('Order_Num').SetEnabled(false);
            grid.GetEditor('SalePrice').SetEnabled(false);//Added on 07-06-2017
            grid.GetEditor('Discount').SetEnabled(false);//Added on 07-06-2017
            grid.GetEditor('TaxAmount').SetEnabled(false);//Added on 07-06-2017
        }
        else {
            grid.GetEditor('ProductName').SetEnabled(false);
            grid.GetEditor('Description').SetEnabled(false);
            grid.GetEditor('Order_Num').SetEnabled(false);
            //grid.GetEditor('SalePrice').SetEnabled(false);//Added on 07-06-2017
            grid.GetEditor('Discount').SetEnabled(false);//Added on 07-06-2017
            //grid.GetEditor('TaxAmount').SetEnabled(false);//Added on 07-06-2017
        }

    }
    else {
        if (grid.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();
        }
    }
    var msgOnDeliveryPendingForQtyDisabled = $("#hdnnCustomerOrPendingDelivery").val();
    if (msgOnDeliveryPendingForQtyDisabled == "CustomDeliveryPending" || msgOnDeliveryPendingForQtyDisabled == "PendingDeliveryList") {

        grid.GetEditor('Quantity').SetEnabled(false);

    }



    cProductsPopup.Hide();
    return false;
}
var Save_ButtonClick=function Save_ButtonClick() {

    var flag = true;
    LoadingPanel.Show();
    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
    var OrderNo = ctxt_SlChallanNo.GetText();
    var slsdate = cPLSalesChallanDate.GetValue();
    var qudate = cPLQADate.GetText();
    var customerid = GetObjectID('hdnCustomerId').value;
    var salesorderDate = new Date(slsdate);
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
        if (grid.GetVisibleRowsOnPage() > 0) {
            var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


            $('#<%=hdfIsDelete.ClientID %>').val('I');
            grid.batchEditApi.EndEdit();
            //grid.UpdateEdit();
            cacbpCrpUdf.PerformCallback();
            $('#<%=hdnRefreshType.ClientID %>').val('N');
        }
        else {
            LoadingPanel.Hide();
            jAlert('You must enter proper details before save');
        }
    }
    // return flag;
}


var SaveExit_ButtonClick=function SaveExit_ButtonClick() {
    debugger;
    var flag = true;
    LoadingPanel.Show();
    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
    $("#<%=hddnSaveOrExitButton.ClientID%>").val('Save_Exit');

    var OrderNo = ctxt_SlChallanNo.GetText();
    var slsdate = cPLSalesChallanDate.GetValue();
    var qudate = cPLQADate.GetText();
    var customerid = GetObjectID('hdnCustomerId').value;
    var salesorderDate = new Date(slsdate);
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
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }

    }

    if (OrderNo == "") {
        flag = false;
        $('#MandatorySlOrderNo').attr('style', 'display:block');
    }
    else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }


    if (flag) {

        if (grid.GetVisibleRowsOnPage() > 0) {
            var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


            $('#<%=hdfIsDelete.ClientID %>').val('I');
            grid.batchEditApi.EndEdit();
            //grid.UpdateEdit();
            cacbpCrpUdf.PerformCallback();
            $('#<%=hdnRefreshType.ClientID %>').val('E');
        }
        else {
            LoadingPanel.Hide();
            jAlert('You must enter proper details before save');
        }
    }
    // return flag;
}



var QuantityTextChange=function QuantityTextChange(s, e) {
    //debugger;
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();

    //Subhabrata on 03-03-2017
    var Id = grid.GetEditor('Quotation_No').GetValue();
    $.ajax({
        type: "POST",
        url: "SalesChallanAdd.aspx/CheckBalQuantity",
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
                    //jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {

                    //});
                    //var tbQuantity = grid.GetEditor("Quantity");
                    //tbQuantity.SetValue(balQty);
                    //return false;

                }

            }
        }

    });

    //End

    var SpliteDetails = ProductID.split("||@||");
    var strMultiplier = SpliteDetails[7];
    var strFactor = SpliteDetails[8];
    //var strRate = (ctxt_Rate.GetValue() != null) ? ctxt_Rate.GetValue() : "1";
    var strRate = "1";
    var strStkUOM = SpliteDetails[4];
    //var strSalePrice = '';
    //if (gridSalesOrderLookup.GetValue() != null) {
    //    strSalePrice = grid.GetEditor('SalePrice').GetValue();
    //}
    //else {

    //    strSalePrice = SpliteDetails[6];
    //}

    var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    if (strSalePrice == '0') {
        strSalePrice = SpliteDetails[6];
    }


    if (strRate == 0) {
        strRate = 1;
    }

    var StockQuantity = strMultiplier * QuantityValue;
    var Amount = QuantityValue * strFactor * strRate * (strSalePrice * 1);

    $('#<%= lblStkQty.ClientID %>').text(StockQuantity);
    $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);

    //var tbStockQuantity = grid.GetEditor("StockQuantity");
    //tbStockQuantity.SetValue(StockQuantity);

    //Subhabrata added on 14-03-2017
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    //var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
    var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

    if (IsPackingActive == "Y") {
        $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
        console.log('jhsdfafa');
        //divPacking.style.display = "block";
        $('#divPacking').css({ 'display': 'block' });
    } else {
        divPacking.style.display = "none";
    }//END

    var tbAmount = grid.GetEditor("Amount");
    tbAmount.SetValue(Amount);

    var TotaAmountRes = '';
    TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(Amount + (TotaAmountRes * 1));
    //tbTotalAmount.SetValue(Amount);
            
    DiscountTextChange(s, e);
}

//function DiscountTextChange(s, e) {
//    var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
//    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

//    var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

//    var tbAmount = grid.GetEditor("Amount");
//    tbAmount.SetValue(amountAfterDiscount);

//    var TotaAmountRes = '';
//    TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

//    var tbTotalAmount = grid.GetEditor("TotalAmount");
//    //tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));
//    tbTotalAmount.SetValue(amountAfterDiscount);

//    //Debj
//}

var AddBatchNew=function AddBatchNew(s, e) {

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


var Warehouseindex;
var OnCustomButtonClick=function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();

        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        if (gridSalesOrderLookup.GetValue() != null) {
            //jAlert();
            jAlert('Cannot Delete using this button as the GRN is created from other document.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) {

            });
        }

        if (noofvisiblerows != "1" && gridSalesOrderLookup.GetValue() == null) {
            grid.DeleteRow(e.visibleIndex);

            $('#<%=hdfIsDelete.ClientID %>').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display');
            grid.batchEditApi.StartEdit(-1, 2);
            grid.batchEditApi.StartEdit(0, 2);
        }
    }
    else if (e.buttonID == 'AddNew') {

        if (gridSalesOrderLookup.GetValue() == null) {
            var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
            if (ProductIDValue != "") {
                OnAddNewClick();
            }
            else {
                grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            }
        }
        else {
            OnAddNewClick();
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {
        debugger;
        $("#<%=hddnIsODSDFirstTime.ClientID%>").val("1");
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;
        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        //var StkQuantityValue = (grid.GetEditor('StockQuantity').GetValue() != null) ? grid.GetEditor('StockQuantity').GetValue() : "0";
        var IsExits = true;
        $("#spnCmbWarehouse").hide();
        $("#spnCmbBatch").hide();
        $("#spncheckComboBox").hide();
        $("#spntxtQuantity").hide();
        var LastFinYear = $('#<%=LastFinancialYear.ClientID %>').val();
        var LastCompany = $('#<%=LastCompany.ClientID %>').val();
        var Branch = $('#<%=ddl_Branch.ClientID %>').val();
        //For Avialable stock
        var data = '';
        var ActionTypeL = '<%= Session["ActionType"] %>';
        if (ActionTypeL != 'Edit') {
            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/GetAvaiableStockCheckStockOut",
                data: JSON.stringify({ ProductID: ProductID.split("||@||")[0], FinYear: LastFinYear, Company: LastCompany, Branch: Branch, Date: cPLSalesChallanDate.date.format('yyyy-MM-dd') }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    data = msg.d;
                    if (data == 'Y') {
                        IsExits = false;
                    }
                }
            });//End
        }

        if (ProductID != "" && parseFloat(QuantityValue) != 0 ) {
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
            $('#<%=hdfProductType.ClientID %>').val(Ptype);

            //Subhabrata Check whether it is in FIFo or not on 23-06-2016
            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/GetConfigSettingRights",
                data: JSON.stringify({ VariableName: 'IsFIFOExistsInOutModule' }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    //debugger;
                    var data = msg.d;
                    $("#<%=hddnConfigVariable_Val.ClientID%>").val(data);
                    if (data == "1") {

                        if (Ptype == "W") {
                            div_QtyMatch.style.display = 'none';
                        }
                        else if (Ptype == "B") {
                            div_QtyMatch.style.display = 'none';
                        }
                        else if (Ptype == "S") {
                            div_QtyMatch.style.display = 'block';
                        }
                        else if (Ptype == "WB") {
                            div_QtyMatch.style.display = 'none';
                        }
                        else if (Ptype == "WS") {
                            div_QtyMatch.style.display = 'block';
                        }
                        else if (Ptype == "WBS") {
                            div_QtyMatch.style.display = 'block';
                        }
                        else if (Ptype == "BS") {
                            div_QtyMatch.style.display = 'block';
                        }

                    }
                    else {
                        div_QtyMatch.style.display = 'none';
                    }
                }
            });
            //End



            document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
            document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
            document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
            document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
            document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;

            $('#<%=hdfProductID.ClientID %>').val(strProductID);
            $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
            $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
            $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
            cacpAvailableStock.PerformCallback(strProductID);

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
                div_Quantity.style.display = 'block';
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

                checkListBox.PerformCallback('BindSerialAfterCross~');
                checkComboBox.SetText(0 + " Items");

                SelectedWarehouseID = "0";
                $("#ADelete").css("display", "none");
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
                //div_Warehouse.style.display = 'none';
                //div_Batch.style.display = 'none';
                //div_Serial.style.display = 'none';
                //div_Quantity.style.display = 'none';

                //$.confirm({
                //    title: 'Confirm!',
                //    type: 'blue',
                //    content: 'No Warehouse or Batch or Serial is actived !',

                //    buttons: {
                //        formSubmit: {
                //            text: 'Ok',
                //            btnClass: 'btn-blue',
                //            keys: ['esc'],
                //            action: function () {
                //                grid.batchEditApi.StartEdit(index, 5);
                //            }
                //        },
                //    },
                //});

                //var strconfirm = confirm("No Warehouse or Batch or Serial is actived.");
                //if (strconfirm == true) {
                //    grid.batchEditApi.StartEdit(index, 5);
                //}
                //else {
                //    grid.batchEditApi.StartEdit(index, 5);
                //}

                jAlert("No Warehouse or Batch or Serial is actived.");
            }
        }
        else if (ProductID != "" && parseFloat(QuantityValue) == 0) {
            //$.confirm({
            //    title: 'Confirm!',
            //    type: 'blue',
            //    content: 'Please enter Quantity !',

            //    buttons: {
            //        formSubmit: {
            //            text: 'Ok',
            //            btnClass: 'btn-blue',
            //            keys: ['esc'],
            //            action: function () {
            //                grid.batchEditApi.StartEdit(index, 5);
            //            }
            //        },
            //    },
            //});

            jAlert('Qty is ZERO. Cannot select Stk Details');
        }
        else if (ProductID != "" && parseFloat(QuantityValue) != 0 && IsExits == false) {
            jAlert("Available stock of the selected product is ZERO(0). Cannot proceed.", "Stock Alert");
            //['" + ProductID.split("||@||")[1] + "']

        }
    }
}
var FinalWarehouse=function FinalWarehouse() {

    cGrdWarehouse.PerformCallback('WarehouseFinal');


}

var closeWarehouse=function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');
}

var OnWarehouseEndCallback=function OnWarehouseEndCallback(s, e) {
    //debugger;
    var Ptype = document.getElementById('hdfProductType').value;
    var ResultantQty = '';

    //Added Subhabrata on 22-06-2017
    if (cGrdWarehouse.cpWarehouseDeleticity != "WareHouseDeleticity") {
        var WarehouseBindQty = cGrdWarehouse.cpWarehouseQty;
        $("#<%=hddnWarehouseQty.ClientID%>").val(WarehouseBindQty);
    }

    var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();
    if (cGrdWarehouse.cpWarehouseDeleticity == "WareHouseDeleticity" && FifoExists == "1") {
        cGrdWarehouse.cpWarehouseDeleticity = null;
        var WarehouseID = $("#<%=hddnWarehouseId.ClientID%>").val();
        var BatchID = $("#<%=hddnBatchId.ClientID%>").val();

        var Qty = ctxtMatchQty.GetValue();

        var hddnQty = $("#<%=hddnWarehouseQty.ClientID%>").val();
        ResultantQty = (Qty * 1) - (hddnQty * 1);
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "0");

    }
    if (cGrdWarehouse.cpWarehouseSaveDisplay == "SaveDisplay" && FifoExists == "1") {
        cGrdWarehouse.cpWarehouseSaveDisplay = null;
        //ctxtMatchQty.SetText('');
        var WarehouseID = $("#<%=hddnWarehouseId.ClientID%>").val();
        var BatchID = $("#<%=hddnBatchId.ClientID%>").val();
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "0");
    }

    //End

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 7);
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
    var CustomerDelivery = $("#<%=hddnCustomerDelivery.ClientID%>").val();
    if (CustomerDelivery == "Yes" || CustomerDelivery == "No") {
        grid.batchEditApi.StartEdit(Warehouseindex, 9);
    }
    else {
        grid.batchEditApi.StartEdit(Warehouseindex, 9);
    }

    //grid.GetEditor("SalePrice").Focus();
}




var TaxAmountKeyDown=function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}


var PopulateSerial=function PopulateSerial() {

    //Serail Bind:Start

    //End
    //debugger;
    var SessionCountSerial = '';
    var indices = [];
    var Qty = ctxtMatchQty.GetValue();
    $("#<%=hddnMatchQty.ClientID%>").val(Qty);
    var CountLength = checkListBox.GetItem.length;
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    checkListBox.SetEnabled(true);
    QuantityResultant = (QuantityResultant * 1) + (Qty * 1);
    //SessionCountSerial = '<%= Session["WarehouseBindQty"] %>'; 
    SessionCountSerial = $("#<%=hddnWarehouseQty.ClientID%>").val();
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
        var WarehouseID = $("#<%=hddnWarehouseId.ClientID%>").val();
        var BatchID = $("#<%=hddnBatchId.ClientID%>").val();
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

var SaveWarehouse=function SaveWarehouse() {

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
                checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
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
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
        SelectedWarehouseID = "0";
    }
}



//function txtserialTextChanged() {
//    var SerialNo = (ctxtserial.GetValue != null) ? (ctxtserial.GetValue()) : "0";
//    ctxtserial.SetValue("");
//    var texts = [SerialNo];
//    var values = GetValuesByTexts(texts);
//    checkListBox.SelectValues(values);
//    UpdateSelectAllItemState();
//    UpdateText(); // for remove non-existing texts
//    SaveWarehouse();
//} By Sudip

var txtserialTextChanged=function txtserialTextChanged() {
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


        //Subhabrata added: on 19-06-2017
        var WarehouseID = $("#<%=hddnWarehouseId.ClientID%>").val();
        var BatchID = $("#<%=hddnBatchId.ClientID%>").val();
        var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();
        var MatchQty = $("#<%=hddnMatchQty.ClientID%>").val();
        var type = document.getElementById('hdfProductType').value;

        if (type == "WBS" || type == "WB") {
            //cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
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
}

var AutoCalculateMandateOnChange=function AutoCalculateMandateOnChange(element) {
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


var fn_Deletecity=function fn_Deletecity(keyValue) {
    //debugger;
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();

    var Qty = ctxtMatchQty.GetValue();
    var hddnQty = $("#<%=hddnWarehouseQty.ClientID%>").val();
    <%--if ((hddnQty * 1) > 0)
    {
        hddnQty = (hddnQty * 1) - 1;
    }
    $("#<%=hddnWarehouseQty.ClientID%>").val(hddnQty);--%>
    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    if (FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + 'NoFIFO');
    }

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
var OnListBoxSelectionChanged=function OnListBoxSelectionChanged(listBox, args) {
    //debugger;
    var selectedItems = checkListBox.GetSelectedItems();
    //if ((args.index * 1) > ((selectedItems.length * 1))) {
    //    if (args.isSelected) {

    //        var indices = [];
    //        //checkListBox.SetCheckBoxEnabled(args.index, true);
    //        indices.push(listBox.GetItem(args.index));
    //        listBox.UnselectIndices(indices[0].text);
    //        //UpdateSelectAllItemState();
    //        UpdateText();
    //        jAlert("Canonot proceed!");
    //        return false;
    //    }

    //}
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



    //if (selectedItems.length != 0) {
    //    checkListBox.SetCheckBoxEnabled(((selectedItems.length * 1) + 1), true);
    //    //listBox.SetEnabled(((selectedItems.length * 1) + 1), false);
    //}

    //for (var i = (selectedItems.length * 1) + 1 ; i < (checkListBox.GetItemCount() * 1) ; i++) {

    //    checkListBox.SetCheckBoxEnabled(i, false);
    //}



    var qty = grid.GetEditor('Quantity').GetValue();
    var selectedItems = checkListBox.GetSelectedItems();
    var val = GetSelectedItemsText(selectedItems);

    UpdateSelectAllItemState();
    UpdateText();



    //checkboxEnabled/Disabled
    //var indices = [];
    //indices.push(listBox.GetItem(args.index));
    //listBox.SetEnabled(indices, false);

    //checkListBox.items[indices[0].text].enable = false;
    //listBox.SetEnabled(indices[0].text) = false;
    //checkListBox["SetEnabled"][indices[0].text] = false;
    //checkListBox.SetCheckBoxEnabled[
    //End

    var strWarehouse = cCmbWarehouse.GetValue();
    var strBatchID = cCmbBatch.GetValue();
    var ProducttId = $("#hdfProductID").val();

    // FIFO Checking 
    //$.ajax({
    //    type: "POST",
    //    url: "SalesChallanAdd.aspx/GetSerialId",
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
    //        
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

    //Subhabrata

    //checkListBox.PerformCallback('CheckSerialOnFIFO~' + strWarehouse + '~' + strBatchID + '~' + val); var val = GetSelectedItemsText(selectedItems);

    //checkListBox.PerformCallback('CheckSerialOnFIFO~' + strWarehouse + '~' + strBatchID + '~' + val);
    //End
}
var UpdateSelectAllItemState=function UpdateSelectAllItemState() {
    IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
}
//function IsAllSelected() {
//    debugger;
//    var selectedDataItemCount;
//    if (checkListBox.GetValue() != null) {
//        selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
//        checkListBox.GetSelectedItems().length = selectedDataItemCount;
//    }
//    else {
//        checkListBox.GetSelectedItems().length = 0;
//    }

//    return checkListBox.GetSelectedItems().length;
//    //var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
//    //return checkListBox.GetSelectedItems().length == selectedDataItemCount;

//}

var IsAllSelected=function IsAllSelected() {
    var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
    return checkListBox.GetSelectedItems().length == selectedDataItemCount;
}
var UpdateText=function UpdateText() {
    //debugger;
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


    var val = GetSelectedItemsText(selectedItems);
    $("#abpl").attr('data-content', val);
}

//function UpdateText() {
//    var selectedItems = checkListBox.GetSelectedItems();
//    selectedChkValue = GetSelectedItemsText(selectedItems);
//    //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
//    checkComboBox.SetText(selectedItems.length + " Items");

//    var val = GetSelectedItemsText(selectedItems);
//    $("#abpl").attr('data-content', val);
//}by Subhabrata

var SynchronizeListBoxValues=function SynchronizeListBoxValues(dropDown, args) {
    checkListBox.UnselectAll();
    //var texts = dropDown.GetText().split(textSeparator);
    var texts = selectedChkValue.split(textSeparator);
    var values = GetValuesByTexts(texts);
    checkListBox.SelectValues(values);
    UpdateSelectAllItemState();
    UpdateText(); // for remove non-existing texts
}
var GetSelectedItemsText=function GetSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.join(textSeparator);
}
var GetValuesByTexts=function GetValuesByTexts(texts) {
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

var GetDynamicSerial=function GetDynamicSerial(value) {
    return '<input name = "SerialContainer" type="text" value = "' + value + '" />'
}

var GetDynamicBatch=function GetDynamicBatch(value) {
    return '<input name = "BatchContainer" type="text" value = "' + value + '" />'
}


var selectValue=function selectValue() {

    var startDate = new Date();
    startDate = cPLSalesChallanDate.GetValueString();
    var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";



    if (key != null && key != '' && type != "") {
        cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);
    }



    var componentType = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
    if (componentType != null && componentType != '') {
        grid.PerformCallback('GridBlank');
    }
}

var OrderNumberChanged=function OrderNumberChanged() {

    //gridquotationLookup.GetValue()
    //grid.PerformCallback('BindGridOnQuotation' + '~' + cddl_Quotatione.GetValue() + '~' + ctxt_SlOrderNo.GetValue());
    //var quote_Id = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

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
        //cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
        //cProductsPopup.Show();

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

var GridCallBack=function GridCallBack() {
    grid.PerformCallback('Display');
}

var ChangeState=function ChangeState(value) {

    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}
var PerformCallToGridBind=function PerformCallToGridBind() {

    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

    if (type != '' && type != null) {
        grid.PerformCallback('BindGridOnQuotation' + '~' + '@' + '~' + type);
    }
    var OrderIds = gridSalesOrderLookup.GetValue();
    var Key = OrderIds.split(',')[0];
    $.ajax({
        type: "POST",
        url: "SalesChallanAdd.aspx/GetContactSalesManReference",
        data: "{'KeyVal':'" + Key + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            var currentString = msg.d;
            //var ContactPersonId = currentString.split('~')[0];
            var Reference = currentString.split('~')[0];
            var SalesManId = currentString.split('~')[1];
            var CurrencyId = currentString.split('~')[2];
            txt_Refference.SetText(Reference)
            $("#<%=ddl_SalesAgent.ClientID%>").val(SalesManId);
            $("#<%=ddl_Currency.ClientID%>").val(CurrencyId);

        }
    });

    cSalesOrderComponentPanel.PerformCallback('BindOrderLookupOnSelection');
    cProductsPopup.Hide();
    $('#<%=hdnPageStatus.ClientID %>').val('Quoteupdate');

    //#### added by Samrat Roy for Transporter Control #############
    var quote_Id = gridSalesOrderLookup.gridView.GetSelectedKeysOnPage();
    if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
        callTransporterControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
    }
    if (quote_Id.length > 0) {
        BSDocTagging(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
    }
    //#### End : Samrat Roy for Transporter Control : End #############

    //#### added by Sayan Dutta for TC Control #############
    if ($("#btn_TermsCondition").is(":visible")) {
        callTCControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
    }
    //#### End : added by Sayan Dutta for TC Control : End #############

    return false;
}


var componentEndCallBack=function componentEndCallBack(s, e) {
    //gridSalesOrderLookup.gridView.Refresh();
    if (grid.GetVisibleRowsOnPage() == 0) {
        OnAddNewClick();
    }
}
//Code for UDF Control 
var OpenUdf=function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        // var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '';
        var keyval = $('#<%=hdnmodeId.ClientID %>').val();
        //  alert(keyval);
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
var GetVisibleIndex=function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}

var cmbtaxCodeindexChange=function cmbtaxCodeindexChange(s, e) {
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

var SetOtherTaxValueOnRespectiveRow=function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
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



var SetOtherChargeTaxValueOnRespectiveRow=function SetOtherChargeTaxValueOnRespectiveRow(idx, amt, name) {
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



var txtPercentageLostFocus=function txtPercentageLostFocus(s, e) {

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

var RecalCulateTaxTotalAmountInline=function RecalCulateTaxTotalAmountInline() {
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
var gstcstvatGlobalName;
var CmbtaxClick=function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}


var txtTax_TextChanged=function txtTax_TextChanged(s, i, e) {
    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
}

var acpAvailableStockEndCall=function acpAvailableStockEndCall(s, e) {


    if (cacpAvailableStock.cpstock != null) {
        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
  
       document.getElementById('<%=lblAvailableSStk.ClientID %>').innerHTML = AvlStk;
        document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cacpAvailableStock.cpstock;
        document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
        cCmbWarehouse.cpstock = null;

        grid.batchEditApi.StartEdit(globalRowIndex, 5);
        return;
    }



}
var SelectWarehouse = "0";
var SelectBatch = "0";
var SelectSerial = "0";
var SelectedWarehouseID = "0";
var CallbackPanelEndCall=function CallbackPanelEndCall(s, e) {

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

document.onkeydown = function (e) {
    if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        StopDefaultAction(e);
        if (page.GetActiveTabIndex() == 1) {
            fnSaveBillingShipping();
        }
        // document.getElementById('Button3').click();

        // return false;
    }

    else if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        StopDefaultAction(e);
        page.SetActiveTabIndex(0);
        gridLookup.Focus();
        // document.getElementById('Button3').click();

        // return false;
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
    else {
        //do nothing
    }
}
// ]]>