
 var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
//Rev Subhra 19-03-2019
var issavePacking = 0;


function clookup_Project_LostFocus() {

    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}
function ProjectValueChange(s, e) {
    var projID = clookup_Project.GetValue();
    $.ajax({
        type: "POST",
        url: 'PurchaseReturnManual.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}

function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('Quantity').SetValue(Quantity);
}
//End of Rev Subhra 19-03-2019

function GlobalBillingShippingEndCallBack() {
    var branchid = $('#ddl_Branch').val();
    // var key = cCustomerComboBox.GetValue();

    var key = GetObjectID('hdnCustomerId').value;
    // var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
    if (gridquotationLookup.GetValue() != null) {
        if (key != null && key != '') {
            cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
        }

        var startDate = new Date();
        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

        // var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

        if (key != null && key != '') {
            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);

        }
        grid.PerformCallback('GridBlank');
        ccmbGstCstVat.PerformCallback();
        ccmbGstCstVatcharge.PerformCallback();
        ctaxUpdatePanel.PerformCallback('DeleteAllTax');
    }
    else {
        //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
        if (key != null && key != '') {
            cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
        }
    }
}





 var ProductGetQuantity = "0";
var ProductGetTotalAmount = "0";
var ProductSaleprice = "0";
var globalNetAmount = 0;
var ProductDiscount = "0";
var _TotalAmount = "0";
//var _TotalAmount = 0;
function AmtGotFocus() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;


}
function AmtTextChange(s, e) {


    var grossamt = grid.GetEditor('Amount').GetValue();
    var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(parseFloat(grossamt) + parseFloat(_TotalTaxAmt));

    //var _TotalAmount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
        grid.GetEditor('TaxAmount').SetValue(0);
        ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());



    }

    //TDSDetail();
    //Rev Rajdip           
    var tbTotalAmount = grid.GetEditor("TotalAmount");
    var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
    var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
    cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
    SetTotalTaxableAmount(s, e);
    SetInvoiceLebelValue();
    //End Rev rajdip

}
//contactperson phone
function acpContactPersonPhoneEndCall(s, e) {
    if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
        pageheaderContent.style.display = "block";
        $("#divContactPhone").attr('style', 'display:block');
        document.getElementById('lblContactPhone').innerHTML = cacpContactPersonPhone.cpPhone;
        cacpContactPersonPhone.cpPhone = null;

    }
}

//contactperson phones

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

function componentEndCallBack(s, e) {
    gridquotationLookup.gridView.Refresh();
    if (grid.GetVisibleRowsOnPage() == 0) {
        //  OnAddNewClick();  kaushik 3/7/2017  avoid extra rows
    }

    if (cQuotationComponentPanel.cpstockoutbranchId != null) {
        document.getElementById('ddl_StockOutBranch').value = cQuotationComponentPanel.cpstockoutbranchId;

        cQuotationComponentPanel.cpstockoutbranchId = null;
    }


    if (cQuotationComponentPanel.cpDetails != null) {
        var details = cQuotationComponentPanel.cpDetails;
        cQuotationComponentPanel.cpDetails = null;

        var SpliteDetails = details.split("~");
        var Reference = SpliteDetails[0];
        var Currency_Id = (SpliteDetails[1] == "" || SpliteDetails[1] == null) ? "0" : SpliteDetails[1];
        var SalesmanId = (SpliteDetails[2] == "" || SpliteDetails[2] == null) ? "0" : SpliteDetails[2];
        //var ExpiryDate = SpliteDetails[3];
        var CurrencyRate = SpliteDetails[4];
        var Contact_person_id = SpliteDetails[5];
        var Tax_option = (SpliteDetails[6] == "" || SpliteDetails[6] == null) ? "1" : SpliteDetails[6];


        var Tax_Code = (SpliteDetails[7] == "" || SpliteDetails[7] == null) ? "0" : SpliteDetails[7];
        //  cTextReferrence.SetValue(Reference);
        $('#txt_Refference').val(Reference);
        ctxt_Rate.SetValue(CurrencyRate);
        cddl_AmountAre.SetValue(Tax_option);
        if (Tax_option == 1) {

            grid.GetEditor('TaxAmount').SetEnabled(true);
            cddlVatGstCst.SetEnabled(false);

            cddlVatGstCst.SetSelectedIndex(0);
            cbtn_SaveRecords.SetVisible(true);
            //  grid.GetEditor('ProductID').Focus();
            if (grid.GetVisibleRowsOnPage() == 1) {
                grid.batchEditApi.StartEdit(-1, 2);
            }

        }
        else if (Tax_option == 2) {
            grid.GetEditor('TaxAmount').SetEnabled(true);

            cddlVatGstCst.SetEnabled(true);
            cddlVatGstCst.PerformCallback('2');
            cddlVatGstCst.Focus();
            cbtn_SaveRecords.SetVisible(true);
        }
        else if (Tax_option == 3) {

            grid.GetEditor('TaxAmount').SetEnabled(false);


            cddlVatGstCst.SetSelectedIndex(0);
            cddlVatGstCst.SetEnabled(false);
            cbtn_SaveRecords.SetVisible(false);
            if (grid.GetVisibleRowsOnPage() == 1) {
                grid.batchEditApi.StartEdit(-1, 2);
            }


        }

        cddlVatGstCst.PerformCallback('Tax-code' + '~' + Tax_Code)
        document.getElementById('ddl_Currency').value = Currency_Id;
        document.getElementById('ddl_SalesAgent').value = SalesmanId;
        if (Contact_person_id != "0" && Contact_person_id != "")
        { cContactPerson.SetValue(Contact_person_id); }

    }
}

function ChangeState(value) {

    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}

//function PerformCallToGridBind() {
//    // ;Button13
//    grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
//    cProductsPopup.Hide();
//    return false;
//}


function PerformCallToGridBind() {
    grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
    cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
    $('#hdnPageStatus').val('Invoiceupdate');
    cProductsPopup.Hide();

    return false;
}
function PurchaseChallanNumberChanged() {
    // ;
    var quote_Id = gridquotationLookup.GetValue();

    if (quote_Id != null) {
        var arr = quote_Id.split(',');
        if (arr.length > 1) {
            ctxt_InvoiceDate.SetText('Multiple Select Purchase Chalan Dates');
        }
        else {
            if (arr.length == 1) {
                cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id);
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
    else {
        //cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
        //cProductsPopup.Show();
        grid.PerformCallback('RemoveDisplay');
    }
    //txt_OANumber.Focus();           
}
//.............Available Stock Div Show............................



function ProductsGotFocus(s, e) {
    $("#pageheaderContent").attr('style', 'display:block');
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
        $('#lblPackingStk').val(PackingValue);
        divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').val(QuantityValue);
    $('#lblStkUOM').val(strStkUOM);
    $('#lblProduct').val(strProductName);
    $('#lblbranchName').val(strBranch);



    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = $("[id*=ddl_Branch]").find("option:selected").val();
    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID + "~" + strBranch);
    }
}
//<%--function ProductsGotFocus(s, e) {
//    $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
//    var ProductID = (grid.GetEditor('ProductName').GetValue() != null) ? grid.GetEditor('ProductName').GetValue() : "0";
//    var strProductName = (grid.GetEditor('ProductName').GetText() != null) ? grid.GetEditor('ProductName').GetText() : "0";
//    //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
//    //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

//    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

//    var ddlbranch = $("[id*=ddl_Branch]");
//    var strBranch = ddlbranch.find("option:selected").text();

//    var SpliteDetails = ProductID.split("||@||");
//    var strProductID = SpliteDetails[0];
//    var strDescription = SpliteDetails[1];
//    var strUOM = SpliteDetails[2];
//    var strStkUOM = SpliteDetails[4];
//    var strSalePrice = SpliteDetails[6];
//    var IsPackingActive = SpliteDetails[10];
//    var Packing_Factor = SpliteDetails[11];
//    var Packing_UOM = SpliteDetails[12];
//    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

//    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
//        $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
//        divPacking.style.display = "block";
//    } else {
//        divPacking.style.display = "none";
//    }

//    $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
//    $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
//    $('#<%= lblProduct.ClientID %>').text(strProductName);
//    $('#<%= lblbranchName.ClientID %>').text(strBranch);

//    if (ProductID != "0") {
//        cacpAvailableStock.PerformCallback(strProductID);
//    }
//}--%>
//<%--  function acpAvailableStockEndCall(s, e) {
//    if (cacpAvailableStock.cpstock != null) {
//        divAvailableStk.style.display = "block";
              

//        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
//        document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
             


//        cCmbWarehouse.cpstock = null;
//    }
//}--%>


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
        var strALTQuantity = cCallbackPanel.cpEdit.split('~')[4];
        var strAltUOM = cCallbackPanel.cpEdit.split('~')[5];

        SelectWarehouse = strWarehouse;
        SelectBatch = strBatchID;
        SelectSerial = strSrlID;

        cCmbWarehouse.PerformCallback('BindWarehouse');
        cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
        checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);
        ctxtAltQuantity.SetValue(strALTQuantity);
        cCmbWarehouse.SetValue(strWarehouse);
        ctxtQuantity.SetValue(strQuantity);
        //ctxtALTUOM.SetValue(strAltUOM); 
        ccmbPackingUom1.SetValue(strAltUOM);
    }
}

function acpAvailableStockEndCall(s, e) {
    if (cacpAvailableStock.cpstock != null) {
        divAvailableStk.style.display = "block";
        divpopupAvailableStock.style.display = "block";

        var AvlStk = cacpAvailableStock.cpstock + " " + $("#lblStkUOM").val();
       // <%-- document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = AvlStk;--%>
        $("#lblAvailableStock").val(cacpAvailableStock.cpstock);
        $("#lblAvailableStockUOM").val($("#lblStkUOM").val());

        cCmbWarehouse.cpstock = null;
    }
}
//................Available Stock Div Show....................


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

    //   alert(event.keyCode);
    if (event.keyCode == 18) isCtrl = true;
    if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") {
        //  alert('kkk'); //run code for Alt + n -- ie, Save & New  78 //  alert('kkk'); //run code for Alt + x -- ie, Save & New  83
        StopDefaultAction(e);
        Save_ButtonClick();
    }
    else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Ctrl+X -- ie, Save & Exit!   
        // alert('kkk222');
        StopDefaultAction(e);
        SaveExit_ButtonClick();
    }
    else if (event.keyCode == 85 && event.altKey == true) { //run code for alt+U -- ie, Save & Exit!   
        // alert('kkk222');
        StopDefaultAction(e);
        OpenUdf();
    }
    else if (event.keyCode == 84 && event.altKey == true && getUrlVars().req != "V") { //run code for alt+T -- ie, Save & Exit!   
        // alert('kkk222');
        StopDefaultAction(e);
        Save_TaxesClick();
    }
    else if (event.keyCode == 79 && event.altKey == true) { //run code for alt + O -- ie, For billing shipping Samrat!   
        StopDefaultAction(e);
        if (page.GetActiveTabIndex() == 1) {
            fnSaveBillingShipping();
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
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
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


function acbpCrpUdfEndCall(s, e) {

    //   LoadingPanel.Hide();
    if (cacbpCrpUdf.cpUDF) {
        if (cacbpCrpUdf.cpUDF == "true" && cacbpCrpUdf.cpTransport == "true") {
            grid.UpdateEdit();
            cacbpCrpUdf.cpUDF = null;
            cacbpCrpUdf.cpTransport = null;
            cacbpCrpUdf.cprefCreditNoteNo = null;
        }
        else if (cacbpCrpUdf.cpUDF == "false") {
            LoadingPanel.Hide();
            jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });

            cacbpCrpUdf.cpUDF = null;
            cacbpCrpUdf.cpTransport = null;
            cacbpCrpUdf.cprefCreditNoteNo = null;
        }

        else if (cacbpCrpUdf.cprefCreditNoteNo == "false") {
            LoadingPanel.Hide();
            jAlert("Ref. Credit Note No. already exist for the selected vendor.", "Alert", function () { });


            cacbpCrpUdf.cpUDF = null;
            cacbpCrpUdf.cpTransport = null;
            cacbpCrpUdf.cprefCreditNoteNo = null;
        }
        else {
            LoadingPanel.Hide();
            jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
            cacbpCrpUdf.cpUDF = null;
            cacbpCrpUdf.cpTransport = null;
            cacbpCrpUdf.cprefCreditNoteNo = null;
        }
    }



}

//function acbpCrpUdfEndCall(s, e) {

//    
//    if (cacbpCrpUdf.cpUDF) {
//        if (cacbpCrpUdf.cpUDF == "true") {
//            grid.UpdateEdit();
//            cacbpCrpUdf.cpUDF = null;

//        }
//        else {
//            jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
//            cacbpCrpUdf.cpUDF = null;

//        }

//    }
//}
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

    // ctxtTaxTotAmt.SetValue(Math.round(totalInlineTaxAmount));
    ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
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

function TaxCalculateAfterDisCount(s,e)
{
    //if (e.buttonIndex == 0) {
    grid.batchEditApi.StartEdit(globalRowIndex);
        if (cddl_AmountAre.GetValue() != null) {
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

            if (ProductID.trim() != "") {
                //   ;
                document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                ctxtTaxTotAmt.SetValue(0);
                ccmbGstCstVat.SetSelectedIndex(0);
                $('.RecalculateInline').hide();
                //caspxTaxpopUp.Show();
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
                //  var Amount = Math.round(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);  kaushik 29-7-2017
                //var Amount = parseFloat(Math.round(QuantityValue * strFactor * (strSalePrice / strRate) * 100) / 100).toFixed(2);
                //clblTaxProdGrossAmt.SetText(Amount);

                //clblProdNetAmt.SetText(grid.GetEditor('Amount').GetValue());
                //document.getElementById('HdProdGrossAmt').value = Amount;
                //document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue()).toFixed(2);


                var Amount = parseFloat(Math.round(QuantityValue * strFactor * (strSalePrice / strRate) * 100) / 100).toFixed(2);

                //chinmoy edited below code
                var GrossAmount = DecimalRoundoff(strSalePrice, 2) * DecimalRoundoff(QuantityValue, 2);
                //clblTaxProdGrossAmt.SetText(DecimalRoundoff(grid.GetEditor('Amount').GetValue(), 2));
                clblTaxProdGrossAmt.SetText(DecimalRoundoff(GrossAmount, 2));
                var Discount = grid.GetEditor('Discount').GetValue();
                var DiscountValue = DecimalRoundoff(DecimalRoundoff(clblTaxProdGrossAmt.GetValue(), 2) * DecimalRoundoff(Discount, 2), 2) / 100;
                var TotalDiscountedValue = DecimalRoundoff(clblTaxProdGrossAmt.GetValue(), 2) - DecimalRoundoff(DiscountValue, 2);
                //clblProdNetAmt.SetText(parseFloat((Math.round(Amount * 100)) / 100).toFixed(2));
                clblProdNetAmt.SetText(DecimalRoundoff(TotalDiscountedValue, 2));
                document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                document.getElementById('HdProdGrossAmt').value = DecimalRoundoff(clblTaxProdGrossAmt.GetValue(), 2);//grid.GetEditor('Amount').GetValue();
                document.getElementById('HdProdNetAmt').value = DecimalRoundoff(clblProdNetAmt.GetValue(), 2); //Amount;

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
                grid.batchEditApi.StartEdit(globalRowIndex, 11);
            }
        }
    //}
}



function taxAmtButnClick(s, e) {
    if (e.buttonIndex == 0) {

        if (cddl_AmountAre.GetValue() != null) {
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

            if (ProductID.trim() != "") {
                //   ;
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
                //  var Amount = Math.round(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);  kaushik 29-7-2017
                //var Amount = parseFloat(Math.round(QuantityValue * strFactor * (strSalePrice / strRate) * 100) / 100).toFixed(2);
                //clblTaxProdGrossAmt.SetText(Amount);

                //clblProdNetAmt.SetText(grid.GetEditor('Amount').GetValue());
                //document.getElementById('HdProdGrossAmt').value = Amount;
                //document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue()).toFixed(2);


                var Amount = parseFloat(Math.round(QuantityValue * strFactor * (strSalePrice / strRate) * 100) / 100).toFixed(2);

                //chinmoy edited below code
                var GrossAmount = DecimalRoundoff(strSalePrice, 2) * DecimalRoundoff(QuantityValue, 2);
                //clblTaxProdGrossAmt.SetText(DecimalRoundoff(grid.GetEditor('Amount').GetValue(), 2));
                clblTaxProdGrossAmt.SetText(DecimalRoundoff(GrossAmount, 2));
                var Discount = grid.GetEditor('Discount').GetValue();
                var DiscountValue = DecimalRoundoff(DecimalRoundoff(clblTaxProdGrossAmt.GetValue(), 2) * DecimalRoundoff(Discount, 2), 2) / 100;
                var TotalDiscountedValue = DecimalRoundoff(clblTaxProdGrossAmt.GetValue(), 2) - DecimalRoundoff(DiscountValue, 2);
                //clblProdNetAmt.SetText(parseFloat((Math.round(Amount * 100)) / 100).toFixed(2));
                clblProdNetAmt.SetText(DecimalRoundoff(TotalDiscountedValue, 2));
                document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                document.getElementById('HdProdGrossAmt').value = DecimalRoundoff(clblTaxProdGrossAmt.GetValue(), 2);//grid.GetEditor('Amount').GetValue();
                document.getElementById('HdProdNetAmt').value = DecimalRoundoff(clblProdNetAmt.GetValue(), 2); //Amount;

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
                grid.batchEditApi.StartEdit(globalRowIndex, 11);
            }
        }
    }
}
//Rev Rajdip
function Taxlostfocus(s, e) {
    debugger;
    //DiscountTextChange(s, e);
    //Rev Rajdip for Running Balance
    //SetTotalTaxableAmount(s, globalRowIndex);
    //SetInvoiceLebelValue();
}
function TotalAmountgotfocus(s, e) {
    debugger;
    SetTotalTaxableAmount(s, e);
    var invValue = parseFloat(cbnrLblTaxableAmtval.GetValue());
    var invTaxamtval = parseFloat(cbnrLblTaxAmtval.GetValue());
    var res = parseFloat(invValue) + parseFloat(invTaxamtval)
    //alert(res);
    cbnrLblInvValue.SetText(res.toFixed(2));
    cbnrlblAmountWithTaxValue.SetText(res.toFixed(2));
}
//End Rev Rajdip
function taxAmtButnClick1(s, e) {
    console.log(grid.GetFocusedRowIndex());
    rowEditCtrl = s;
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
                cmbGstCstVatChange(ccmbGstCstVat);
                cgridTax.cpComboCode = null;
            }
        }
    }

    //if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
    //    ctxtTaxTotAmt.SetValue(Math.round(cgridTax.cpUpdated.split('~')[1]));
    //    var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
    //    var ddValue = parseFloat(ctxtGstCstVat.GetValue());
    //    ctxtTaxTotAmt.SetValue(Math.round(gridValue + ddValue));
    //    cgridTax.cpUpdated = "";
    //}
    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
        ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);
        var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
        var ddValue = parseFloat(ctxtGstCstVat.GetValue());
        ctxtTaxTotAmt.SetValue(gridValue + ddValue);
        cgridTax.cpUpdated = "";
    }
    else {
        var totAmt = ctxtTaxTotAmt.GetValue();
        cgridTax.CancelEdit();
        caspxTaxpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 14);
        grid.GetEditor("TaxAmount").SetValue(totAmt);
        //  grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));
        grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));

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

function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}





        function refCreditNoteDtMandatorycheck() {
            //var PoRefNote = ctxt_refCreditNoteNo.GetValue();
            //var Podt = cdt_refCreditNoteDt.GetValue();

            var PoRefNote = $('#txt_refCreditNoteNo').val();
            var Podt = cdt_refCreditNoteDt.GetValue();
            if (Podt != null) {


                if (PoRefNote != null && PoRefNote != '') {
                    $('#MandatorysRefCreditNoteno').attr('style', 'display:none');
                    var sdate = cdt_refCreditNoteDt.GetValue();
                    var edate = tstartdate.GetValue();

                    var startDate = new Date(sdate);
                    var endDate = new Date(edate);
                    if (startDate > endDate) {
                        //LoadingPanel.Hide();
                        //flag = false;
                        $('#MandatoryREgSDate').attr('style', 'display:block');
                    }
                    else { $('#MandatoryREgSDate').attr('style', 'display:none'); }
                }
                else {
                    $('#MandatorysRefCreditNoteno').attr('style', 'display:block');


                    var sdate = cdt_refCreditNoteDt.GetValue();
                    var edate = tstartdate.GetValue();

                    var startDate = new Date(sdate);
                    var endDate = new Date(edate);
                    if (startDate > endDate) {
                        //LoadingPanel.Hide();
                        //flag = false;
                        $('#MandatoryREgSDate').attr('style', 'display:block');
                    }
                    else { $('#MandatoryREgSDate').attr('style', 'display:none'); }
                }

            }


        }

        $(document).ready(function () {
            var mode = $('#hdnADDEditMode').val();
            if (mode == 'Edit') {
                if ($("#hdnCustomerId").val() != "") {
                    var VendorID = $("#hdnCustomerId").val();
                    SetEntityType(VendorID);
                }

                if (mode != 'ADD') {
                    tstartdate.SetEnabled(false);
                    }


                if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                    page.GetTabByName('Billing/Shipping').SetEnabled(false);
                }
                $('#ApprovalCross').click(function () {
                    //  ;
                    window.parent.popup.Hide();
                    window.parent.cgridPendingApproval.Refresh()();
                })
            }
        })

   

    //function UniqueCodeCheck() {

    //    var QuoteNo = ctxt_PLQuoteNo.GetText();
    //    if (QuoteNo != '') {
    //        var CheckUniqueCode = false;
    //        $.ajax({
    //            type: "POST",
    //            url: "SalesInvoice.aspx/CheckUniqueCode",
    //            data: JSON.stringify({ QuoteNo: QuoteNo }),
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            success: function (msg) {
    //                CheckUniqueCode = msg.d;
    //                if (CheckUniqueCode == true) {

    //                    $('#duplicateQuoteno').attr('style', 'display:block');
    //                    ctxt_PLQuoteNo.SetValue('');
    //                    ctxt_PLQuoteNo.Focus();
    //                }
    //                else {
    //                    $('#duplicateQuoteno').attr('style', 'display:none');
    //                }
    //            }
    //        });
    //    }
    //}
    //function CloseGridLookup() {
    //    gridLookup.ConfirmCurrentSelection();
    //    gridLookup.HideDropDown();
    //    gridLookup.Focus();
    //}

    function GetContactPersonPhone(e) {
        var key = cContactPerson.GetValue();
        cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
    }
    //function GetContactPerson(e) {

    //    // var key = gridLookup.GetValue();
    //    var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
    //    if (key != null && key != '') {
    //        cchkBilling.SetChecked(false);
    //        cchkShipping.SetChecked(false);
    //        cContactPerson.PerformCallback('BindContactPerson~' + key);
    //        page.GetTabByName('Billing/Shipping').SetEnabled(true);
    //        jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
    //            if (r == true) {
    //                page.SetActiveTabIndex(1);
    //                $('.dxeErrorCellSys').addClass('abc');
    //            }
    //        });
    //        var startDate = new Date();
    //        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
    //        cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%');
    //        GetObjectID('hdnCustomerId').value = key;
    //        GetObjectID('hdnAddressDtl').value = '0';

    //    }

    //}

    function cmbRequestTextChange() {
        var type = "PI";
        var startDate = new Date();
        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

        // var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
        // var key = cCustomerComboBox.GetValue();
        var key = GetObjectID('hdnCustomerId').value;
        if (key != null && key != '') {
            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);

        }

        // grid.PerformCallback('GridBlank');    kaushik 30_6_2017 becaues two rows bond in grid
    }

   
    $(document).ready(function () {
        var schemaid = $('#ddl_numberingScheme').val();
        if (schemaid != null) {
            if (schemaid == '') {
                //  ctxt_PLQuoteNo.SetEnabled(false);
                document.getElementById('txt_PLQuoteNo').disabled = true;
            }
        }
        $('#ddl_numberingScheme').change(function () {
            var NoSchemeTypedtl = $(this).val();
            var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
            var quotelength = NoSchemeTypedtl.toString().split('~')[2];
            var branchID = NoSchemeTypedtl.toString().split('~')[3];
            document.getElementById('ddl_Branch').value = branchID;
            var fromdate = NoSchemeTypedtl.toString().split('~')[5];
            var todate = NoSchemeTypedtl.toString().split('~')[6];
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
            document.getElementById('ddl_StockOutBranch').value = branchID;
            ccmbRequest.PerformCallback(branchID);              
            if (NoSchemeType == '1') {
                $('#txt_PLQuoteNo').val('Auto');
                document.getElementById('txt_PLQuoteNo').disabled = true;
                              

                tstartdate.Focus();
                if ($("#HdnBackDatedEntryPurchaseGRN").val() == "0") {

                    tstartdate.SetEnabled(false);

                }
                else {
                    tstartdate.SetEnabled(true);
                }
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
                tstartdate.SetEnabled(false);                
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
        ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        if (IsProduct == "Y") {
            $('#hdfIsDelete').val('D');
            $('#HdUpdateMainGrid').val('True');
            // grid.UpdateEdit();
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

    //Date Function End

    // Popup Section

    function ShowCustom() {

        cPopup_wareHouse.Show();


    }

    // Popup Section End


   
     var IsProduct = "";
    var currentEditableVisibleIndex;
    var preventEndEditOnLostFocus = false;
    var lastProductID;
    var setValueFlag;

    function GridCallBack() {

        $('#ddl_numberingScheme').focus();
        //  grid.PerformCallback('Display');
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
            // grid.UpdateEdit();
            cacbpCrpUdf.PerformCallback();
            //kaushik
            grid.PerformCallback('CurrencyChangeDisplay');
        }
    }

    function ProductsCombo_SelectedIndexChanged(s, e) {
        $("#pageheaderContent").attr('style', 'display:block');
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

        $('#lblStkQty').val("0.00");
        $('#lblStkUOM').val(strStkUOM);
        $('#lblProduct').val(strProductName);
        $('#lblbranchName').val(strBranch);

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').val(PackingValue);
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
    function cmbContactPersonEndCall(s, e) {
        LoadingPanel.Hide();
        if (cContactPerson.cpDueDate != null) {
            var DeuDate = cContactPerson.cpDueDate;
            var myDate = new Date(DeuDate);

            cdt_SaleInvoiceDue.SetDate(myDate);
            cContactPerson.cpDueDate = null;
        }

        if (cContactPerson.cpReferredBy != null) {
            var ReferredBy = cContactPerson.cpReferredBy;
            $('#txt_Refference').val(ReferredBy);
            //  cTextReferrence.SetValue(ReferredBy);
            cContactPerson.cpReferredBy = null;
        }


        if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {
            //alert(cContactPerson.cpOutstanding);

            $("#pageheaderContent").attr('style', 'display:block');
            // pageheaderOutContent.style.display = "block";

            $("#divOutstanding").attr('style', 'display:block');
            document.getElementById('lblOutstanding').innerHTML = cContactPerson.cpOutstanding;

            cContactPerson.cpOutstanding = null;
        }
        else if (cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) {
            $("#pageheaderContent").attr('style', 'display:block');
            $("#divGSTN").attr('style', 'display:block');
            document.getElementById('lblGSTIN').innerHTML = cContactPerson.cpGSTN;
            cContactPerson.cpGSTN = null;
        }
        else {
            $("#pageheaderContent").attr('style', 'display:none');
            //pageheaderOutContent.style.display = "none";
            $("#divOutstanding").attr('style', 'display:none');
            document.getElementById('lblOutstanding').innerHTML = '';
        }
    }

   


    function SetArrForUOM() {
        if (aarr.length == 0) {
            for (var i = -500; i < 500; i++) {
                if (grid.GetRow(i) != null) {

                    var ProductID = (grid.batchEditApi.GetCellValue(i, 'ProductID') != null) ? grid.batchEditApi.GetCellValue(i, 'ProductID') : "0";
                    if (ProductID != "0") {
                        var actionqty = '';
                        var PurchaseReturnNum = "";
                        if ($("#hdnADDEditMode").val() == "Edit") {

                            var SpliteDetails = ProductID.split("||@||");
                            var strProductID = SpliteDetails[0];
                            var orderid = grid.GetRowKey(i);
                            var slnoget = grid.batchEditApi.GetCellValue(i, 'SrlNo');
                            var Quantity = grid.batchEditApi.GetCellValue(i, 'Quantity');

                            actionqty = 'GetPurchaseReturnQty';

                            $.ajax({
                                type: "POST",
                                url: "Services/Master.asmx/GetMultiUOMDetails",
                                data: JSON.stringify({ orderid: orderid, action: actionqty, module: 'PurchaseRet', strKey: strProductID }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: function (msg) {

                                    gridPackingQty = msg.d;

                                    if (msg.d != "") {
                                        var packing = SpliteDetails[18];
                                        var PackingUom = SpliteDetails[20];
                                        var PackingSelectUom = SpliteDetails[21];
                                        var arrobj = {};
                                        arrobj.productid = strProductID;
                                        arrobj.slno = slnoget;
                                        arrobj.Quantity = Quantity;
                                        arrobj.packing = gridPackingQty;
                                        arrobj.PackingUom = PackingUom;
                                        arrobj.PackingSelectUom = PackingSelectUom;

                                        aarr.push(arrobj);
                                        //alert();
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

        LoadingPanel.Show();
        flag = true;
        grid.batchEditApi.EndEdit();
        // Quote no validation Start
        //var QuoteNo = ctxt_PLQuoteNo.GetText();

        var ProjectCode = clookup_Project.GetText();
        if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
            LoadingPanel.Hide();
            jAlert("Please Select Project.");
            flag = false;
        }


        var QuoteNo = $('#txt_PLQuoteNo').val();
        QuoteNo = QuoteNo.trim();
        if (QuoteNo == '' || QuoteNo == null) {
            LoadingPanel.Hide();
            $('#MandatorysQuoteno').attr('style', 'display:block');
            flag = false;
        }
        else {
            $('#MandatorysQuoteno').attr('style', 'display:none');
        }
        // Quote no validation End

        //var quote_Id = gridquotationLookup.GetValue();

        //if (quote_Id == null) {
        //    LoadingPanel.Hide();
        //    $('#MandatorysPCno').attr('style', 'display:block');
        //    flag = false;

        //}
        //else {
        //    $('#MandatorysPCno').attr('style', 'display:none');
        //}
        // Quote Date validation Start
        var sdate = tstartdate.GetValue();
        var edate = tenddate.GetValue();

        var startDate = new Date(sdate);
        var endDate = new Date(edate);

        var RefCreditNotedate = cdt_refCreditNoteDt.GetValue();
        // var RefCreditNoteNo = ctxt_refCreditNoteNo.GetText();

        var RefCreditNoteNo = $('#txt_refCreditNoteNo').val();

        var rDate = new Date(RefCreditNotedate);



        if (RefCreditNoteNo != null && RefCreditNoteNo != '') {

            if (RefCreditNotedate == null || RefCreditNotedate == '') {
                LoadingPanel.Hide();

                $("#MandatoryRefDate").show();

                flag = false;
            }
            else { $("#MandatoryRefDate").hide(); }

            if (rDate != null && rDate != '') {
                if (rDate > startDate) {
                    LoadingPanel.Hide();
                    flag = false;
                    $('#MandatoryREgSDate').attr('style', 'display:block');
                }
                else {

                    // LoadingPanel.Hide();
                    $('#MandatoryREgSDate').attr('style', 'display:none');
                }
            }
            else {
                $('#MandatoryRefDate').attr('style', 'display:block');
                LoadingPanel.Hide();
                flag = false;

            }


        }
        else {

            if (RefCreditNotedate != null && RefCreditNotedate != '') {
                LoadingPanel.Hide();
                flag = false;
                $('#MandatorysRefCreditNoteno').attr('style', 'display:block');
            }
            else {

                $('#MandatorysRefCreditNoteno').attr('style', 'display:none');


            }

            if (RefCreditNotedate != null && RefCreditNotedate != '') {
                $("#MandatoryRefDate").hide();

            }

        }


        //if (sdate == null || sdate == "") {
        //    flag = false;
        //    $('#MandatorysDate').attr('style', 'display:block');
        //}
        //else { $('#MandatorysDate').attr('style', 'display:none'); }
        //if (edate == null || sdate == "") {
        //    flag = false;
        //    $('#MandatoryEDate').attr('style', 'display:block');
        //}
        //else {
        //    $('#MandatoryEDate').attr('style', 'display:none');
        //    if (startDate > endDate) {

        //        flag = false;
        //        $('#MandatoryEgSDate').attr('style', 'display:block');
        //    }
        //    else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        //}
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

                SetArrForUOM(); //Surojit For UOM EDIT

                //Subhra 20-03-2019
                if (issavePacking == 1 && aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "PurchaseReturnManual.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            $('#hdfLookupCustomer').val(customerval);
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('N');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                    });
                }
                else {
                    //Subhra 20-03-2019
                    //divSubmitButton.style.display = "none";
                    //  var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";
                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    $('#hdfLookupCustomer').val(customerval);
                    // Custom Control Data Bind

                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('N');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    // grid.UpdateEdit();
                    cacbpCrpUdf.PerformCallback();
                    //kaushik
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

        LoadingPanel.Show();
        flag = true;
        grid.batchEditApi.EndEdit();
        // Quote no validation Start
        // var QuoteNo = ctxt_PLQuoteNo.GetText();
        var ProjectCode = clookup_Project.GetText();
        if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
            LoadingPanel.Hide();
            jAlert("Please Select Project.");
            flag = false;
        }
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


        //var quote_Id = gridquotationLookup.GetValue();

        //if (quote_Id == null) {
        //    $('#MandatorysPCno').attr('style', 'display:block');
        //    flag = false;

        //}
        //else {
        //    $('#MandatorysPCno').attr('style', 'display:none');
        //}
        // Quote Date validation Start
        var sdate = tstartdate.GetValue();
        var edate = tenddate.GetValue();

        var startDate = new Date(sdate);
        var endDate = new Date(edate);



        var RefCreditNotedate = cdt_refCreditNoteDt.GetValue();
        // var RefCreditNoteNo = ctxt_refCreditNoteNo.GetText();


        var RefCreditNoteNo = $('#txt_refCreditNoteNo').val();
        var rDate = new Date(RefCreditNotedate);



        if (RefCreditNoteNo != null && RefCreditNoteNo != '') {

            if (RefCreditNotedate == null || RefCreditNotedate == '') {
                LoadingPanel.Hide();

                $("#MandatoryRefDate").show();

                flag = false;
            }
            else { $("#MandatoryRefDate").hide(); }

            if (rDate != null && rDate != '') {
                if (rDate > startDate) {
                    LoadingPanel.Hide();
                    flag = false;
                    $('#MandatoryREgSDate').attr('style', 'display:block');
                }
                else {

                    // LoadingPanel.Hide();
                    $('#MandatoryREgSDate').attr('style', 'display:none');
                }
            }
            else {
                $('#MandatoryRefDate').attr('style', 'display:block');
                LoadingPanel.Hide();
                flag = false;

            }


        }
        else {

            if (RefCreditNotedate != null && RefCreditNotedate != '') {
                LoadingPanel.Hide();
                flag = false;
                $('#MandatorysRefCreditNoteno').attr('style', 'display:block');
            }
            else {

                $('#MandatorysRefCreditNoteno').attr('style', 'display:none');


            }

            if (RefCreditNotedate != null && RefCreditNotedate != '') {
                $("#MandatoryRefDate").hide();

            }

        }



        //if (sdate == null || sdate == "") {
        //    flag = false;
        //    $('#MandatorysDate').attr('style', 'display:block');
        //}
        //else { $('#MandatorysDate').attr('style', 'display:none'); }
        //if (edate == null || sdate == "") {
        //    flag = false;
        //    $('#MandatoryEDate').attr('style', 'display:block');
        //}
        //else {
        //    $('#MandatoryEDate').attr('style', 'display:none');
        //    if (startDate > endDate) {

        //        flag = false;
        //        $('#MandatoryEgSDate').attr('style', 'display:block');
        //    }
        //    else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        //}
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

                SetArrForUOM(); //Surojit For UOM EDIT

                //Subhra 20-03-2019
                if (issavePacking == 1 && aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "PurchaseReturnManual.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
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
                    //Subhra 20-03-2019
                    //divSubmitButton.style.display = "none";
                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    //  var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";
                    //  cCustomerComboBox
                    // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    $('#hdfLookupCustomer').val(customerval);
                    // Custom Control Data Bind

                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    // grid.UpdateEdit();

                    cacbpCrpUdf.PerformCallback();
                    //kaushik
                }
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');

                LoadingPanel.Hide();
            }
        }
        else { LoadingPanel.Hide(); }
    }


    function SalePriceGotFocus(s, e) {
        ProductSaleprice = grid.GetEditor("SalePrice").GetValue();
        globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
        //Rev Rajdip           
        var tbTotalAmount = grid.GetEditor("TotalAmount");
        var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
        var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
        cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
        SetTotalTaxableAmount(s, e);
        SetInvoiceLebelValue();
        //End Rev rajdip

    }
    function QuantityGotFocus(s, e) {

        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        ProductGetQuantity = QuantityValue;
        globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());


        //Rev 1.0 Subhra 20-03-2019
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strProductName = SpliteDetails[1];
        var strDescription = SpliteDetails[1];

        var isOverideConvertion = SpliteDetails[23];
        var packing_saleUOM = SpliteDetails[21];
        var sProduct_SaleUom = SpliteDetails[20];
        var sProduct_quantity = SpliteDetails[19];
        var packing_quantity = SpliteDetails[18];

        var slno = (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

        var PurchaseReturnNum = (grid.GetEditor('ComponentNumber').GetText() != null) ? grid.GetEditor('ComponentNumber').GetText() : "0";

        var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
        var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
        var type = 'add';
        var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
        var gridPackingQty = 0.00;


        if (SpliteDetails[23] == "1") { //IsInventory check
            IsInventory = 'Yes';

            type = 'edit';

            if ($('#hdnADDEditMode').val() == "Edit") {

                var orderid = grid.GetRowKey(globalRowIndex);
                //var orderid = document.getElementById('Keyval_Id').value;
                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({ orderid: orderid, action: 'GetPurchaseReturnQty', module: 'PurchaseRet', strKey: strProductID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        gridPackingQty = msg.d;

                        if (ShowUOMConversionInEntry == "1" && IsInventory == "Yes" && SpliteDetails.length > 1) {
                            ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }

                    }
                });
            }

            else {

                if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                    ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }
            }
        }
        //End of Rev 1.0 Subhra 20-03-2019


    }
    var fromColumn = '';
    function QuantityTextChange(s, e) {
        $("#pageheaderContent").attr('style', 'display:block');
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)) {
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            // var key = gridLookup.GetValue();
            //  var key = cCustomerComboBox.GetValue();
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

                    if (CurrQty < 0) {
                        grid.GetEditor("TotalQty").SetValue(TotalQty);
                        grid.GetEditor("Quantity").SetValue(TotalQty);
                        var OrdeMsg = 'Cannot enter quantity more than balance quantity.';
                        grid.batchEditApi.EndEdit();
                        jAlert(OrdeMsg, 'Alert Dialog: [Balance Quantity ]', function (r) {
                            grid.batchEditApi.StartEdit(globalRowIndex, 5);
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

                $('#lblStkQty').val(StockQuantity);
                $('#lblStkUOM').val(strStkUOM);
                $('#lblProduct').val(strProductName);
                $('#lblbranchName').val(strBranch);



                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue(Amount);

                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue(Amount);

          

                DiscountTextChange(s, e);


                // cacpAvailableStock.PerformCallback(strProductID);
            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('Quantity').SetValue('0');
                grid.GetEditor('ProductID').Focus();
            }
       
        }

    }
    function srlgotfocus(s, e) {
        var inx = globalRowIndex;
        SetTotalTaxableAmount(inx, e);
        SetInvoiceLebelValue();
    }
    function QuantityLostFocus(s, e) {
        QuantityTextChange(s, e);
    }

    var globalNetAmount = 0;
    //Rev Rajdip For Running Parameters
    function SetTotalTaxableAmount(inx, vindex) {
        debugger;
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
                }
            }
        }

        // globalRowIndex = inx;


        grid.batchEditApi.EndEdit()
        cbnrLblTaxableAmtval.SetText(DecimalRoundoff(totalAmount, 2));
        cbnrLblTaxAmtval.SetText(DecimalRoundoff(totaltxAmount, 2));
        cbnrLblTotalQty.SetText(DecimalRoundoff(totalQuantity, 4));
        var totamt = totalAmount + totaltxAmount;
        cbnrlblAmountWithTaxValue.SetText(DecimalRoundoff(totamt,2));
        cbnrLblInvValue.SetText(DecimalRoundoff(totamt,2));
        globalRowIndex = vindex;
    }
    function SetInvoiceLebelValue() {

        var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
        cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));

    }
    //End Rev Rajdip


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

                $('#lblProduct').val(strProductName);
                $('#lblbranchName').val(strBranch);

                var IsPackingActive = SpliteDetails[10];
                var Packing_Factor = SpliteDetails[11];
                var Packing_UOM = SpliteDetails[12];
                var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

                if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                    $('#lblPackingStk').val(PackingValue);
                    divPacking.style.display = "block";
                } else {
                    divPacking.style.display = "none";
                }
                DiscountTextChange(s, e);
                //cacpAvailableStock.PerformCallback(strProductID);
                DiscountTextChange(s, e);
                SetTotalTaxableAmount(s, e);
                SetInvoiceLebelValue();

            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('SalePrice').SetValue('0');
                grid.GetEditor('ProductID').Focus();
            }
        }
        //Rev Rajdip           
        //var tbTotalAmount = grid.GetEditor("TotalAmount");
        //var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
        //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
        //cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
        //SetTotalTaxableAmount(s, e);
        //SetInvoiceLebelValue();
        //End Rev rajdip
    }


    /// Code Above Added By Sam on 23022017 after make editable of sale price field End



    function DiscountGotChange() {
        globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
        ProductGetTotalAmount = globalNetAmount;

        ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
        ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        ProductSaleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    }

    //<%--function DiscountTextChange(s, e) {
    //    //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    //    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

    //    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    //    var SalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    //    var ProductID = grid.GetEditor('ProductID').GetValue();
    //    if (ProductID != null) {

    //        if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(SalePrice) != parseFloat(ProductSaleprice)) {


    //            var SpliteDetails = ProductID.split("||@||");
    //            var strFactor = SpliteDetails[8];
    //            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
    //            var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    //            if (strSalePrice == '0') {
    //                strSalePrice = SpliteDetails[6];
    //            }
    //            if (strRate == 0) {
    //                strRate = 1;
    //            }
    //            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

    //            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

    //            var tbAmount = grid.GetEditor("Amount");
    //            tbAmount.SetValue(amountAfterDiscount);

    //            var IsPackingActive = SpliteDetails[10];
    //            var Packing_Factor = SpliteDetails[11];
    //            var Packing_UOM = SpliteDetails[12];
    //            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    //            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
    //                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
    //                divPacking.style.display = "block";
    //            } else {
    //                divPacking.style.display = "none";
    //            }

    //            grid.GetEditor('TaxAmount').SetValue(0);
    //            var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    //            var tbTotalAmount = grid.GetEditor("TotalAmount");

    //            tbTotalAmount.SetValue(parseFloat(amountAfterDiscount) + parseFloat(_TotalTaxAmt));
    //            //var tbTotalAmount = grid.GetEditor("TotalAmount");
    //            //tbTotalAmount.SetValue(amountAfterDiscount);


    //        }
    //    }
    //    else {
    //        jAlert('Select a product first.');
    //        grid.GetEditor('Discount').SetValue('0');
    //        grid.GetEditor('ProductID').Focus();
    //    }
    //    //Debjyoti 
    //    //if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
    //    if (parseFloat(_TotalTaxAmt) != parseFloat(ProductGetTotalAmount)) {
    //        grid.GetEditor('TaxAmount').SetValue(0);

    //        ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
    //    }


    //}--%>

function DiscountTextChange(s, e) {
          

    //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var SalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(SalePrice) != parseFloat(ProductSaleprice)) {

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
                $('#lblPackingStk').val(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(amountAfterDiscount);

           
            //  var ShippingStateCode = cbsSCmbState.GetValue();

            var ShippingStateCode = $("#bsSCmbStateHF").val();


            var TaxType = "";
            if (cddl_AmountAre.GetValue() == "1") {
                TaxType = "E";
            }
            else if (cddl_AmountAre.GetValue() == "2") {
                TaxType = "I";
            }
            //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[17], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val());
            // caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[17], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), 'P');
            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[17], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), tstartdate.GetDate(), QuantityValue, 'P');

        }
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }

    //Debjyoti 
    //grid.GetEditor('TaxAmount').SetValue(0);
    //Rev Rajdip           
    SetTotalTaxableAmount(s, e);
    //End Rev rajdip

   // TaxCalculateAfterDisCount(s, e);
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
        }
        else {

            PurchaseChallanNumberChanged();
            //kaushik 14-4-2017
            grid.StartEditRow(0);
        }
    }

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



    function callback_InlineRemarks_EndCall(s, e) {

        if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
            $("#txtInlineRemarks").focus();
        }
        else if (ccallback_InlineRemarks.cpRemarksFinalFocus == "RemarksFinalFocus") {
            cPopup_InlineRemarks.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }
        else {
            cPopup_InlineRemarks.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }

    }


    function FinalRemarks() {


        ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + grid.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
        $("#txtInlineRemarks").val('');
        cPopup_InlineRemarks.Hide();

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
                url: "StockinReturnManual.aspx/GetPackingQuantity",
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
        ctxtAltQuantity.SetText(calcQuantity);

        ChkDataDigitCount(Quantity);
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
                url: "StockinReturnManual.aspx/GetPackingQuantity",
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


    function SetDataSourceOnComboBoxandSetVal(ControlObject, Source, id) {
        ControlObject.ClearItems();
        for (var count = 0; count < Source.length; count++) {
            ControlObject.AddItem(Source[count].UOM_Name, Source[count].UOM_Id);
        }
        ControlObject.SetValue(id);
        // ControlObject.SetSelectedIndex(0);
    }

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

            $('#hdnDeleteSrlNo').val(SrlNo);
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            if (gridquotationLookup.GetValue() != null) {
                var messege = "";
                messege = "Cannot Delete using this button as the Purchase Invoice is linked with this Return With Stock & Account.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
                jAlert(messege, 'Alert Dialog: [Delete Purchase Challan Products]', function (r) {
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
                    //  cacbpCrpUdf.PerformCallback();
                    //kaushik
                    grid.PerformCallback('Display');

                    //  $('#<%=hdnPageStatus.ClientID %>').val('update');
                    $('#hdnPageStatus').val('delete');
                    //grid.batchEditApi.StartEdit(-1, 2);
                    //grid.batchEditApi.StartEdit(0, 2);
                }
            }
        }

        else if (e.buttonID == "CustomaddDescRemarks") {

            var index = e.visibleIndex;
            grid.batchEditApi.StartEdit(e.visibleIndex, 4);
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


        else if (e.buttonID == 'AddNew') {
            //
            if (gridquotationLookup.GetValue() == null) {



                var ProductIDValue = (grid.GetEditor('ProductDisID').GetText() != null) ? grid.GetEditor('ProductDisID').GetText() : "0";
                if (ProductIDValue != "") {
                    OnAddNewClick();

                    grid.batchEditApi.StartEdit(globalRowIndex, 2);
                    setTimeout(function () {
                        grid.batchEditApi.StartEdit(globalRowIndex, 2);
                    }, 500);

                    return false;
                }
                else {
                    //setTimeout(function () {
                    //    grid.batchEditApi.StartEdit(globalRowIndex, 2);
                    //}, 500);
                    //return false;
                    ////
                    grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                }
            }
            else {
                PurchaseChallanNumberChanged();
            }
        }
        else if (e.buttonID == 'CustomWarehouse') {


            var index = e.visibleIndex;
            grid.batchEditApi.StartEdit(index, 2)
            //   var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";


            Warehouseindex = index;

            var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var ComponentID = (grid.GetEditor('ComponentID').GetValue() != null) ? grid.GetEditor('ComponentID').GetValue() : "0";
            //var StkQuantityValue = (grid.GetEditor('StockQuantity').GetValue() != null) ? grid.GetEditor('StockQuantity').GetValue() : "0";

            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();
            var AltUOMID = "";
            if (ProductID != "" && parseFloat(QuantityValue) != 0) {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strMultiplier = SpliteDetails[7];
                var strProductName = strDescription;
                debugger;

                 AltUOMID = SpliteDetails[21];
                //ctxtALTUOM.SetValue(AltUOMID);
                 if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                     ccmbPackingUom1.SetValue(AltUOMID);
                 }
                 else
                 {
                     ccmbPackingUom1.SetValue(0);
                 }
                //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                var StkQuantityValue = QuantityValue * strMultiplier;
                var Ptype = SpliteDetails[16];
                $('#hdfProductType').val(Ptype);

                document.getElementById('lblProductName').innerHTML = strProductName;
                document.getElementById('txt_SalesAmount').innerHTML = QuantityValue;
                document.getElementById('txt_SalesUOM').innerHTML = strUOM;
                document.getElementById('txt_StockAmount').innerHTML = StkQuantityValue;
                document.getElementById('txt_StockUOM').innerHTML = strStkUOM;

                $('#hdfProductID').val(strProductID);
                $('#hdfProductSerialID').val(SrlNo);
                $('#hdnProductQuantity').val(QuantityValue);
                $('#hdfComponentID').val(ComponentID);
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


                var objectToPass = {}
                var product = $("#hdfProductID").val();
                objectToPass.ProductID = $("#hdfProductID").val();//hdfProductID.value;
                if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                    $.ajax({
                        type: "POST",
                        url: "../Activities/Services/Master.asmx/GetUom",
                        data: JSON.stringify(objectToPass),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var returnObject = msg.d;
                            var UOMId = returnObject.uom_id;
                            var UOMName = returnObject.UOM_Name;
                            if (returnObject) {
                                SetDataSourceOnComboBoxandSetVal(ccmbPackingUom1, returnObject.uom, UOMId);
                                ccmbPackingUom1.SetEnabled(false);

                            }
                        }
                    });
                    ccmbPackingUom1.SetValue(AltUOMID);
                    ctxtQuantity.SetValue(QuantityValue);
                }
                ChangePackingByQuantityinjs();

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

    }


  
    function FinalWarehouse() {
        cGrdWarehouse.PerformCallback('WarehouseFinal');
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
            grid.batchEditApi.StartEdit(Warehouseindex, 8);
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

    var SelectWarehouse = "0";
    var SelectBatch = "0";
    var SelectSerial = "0";
    var SelectedWarehouseID = "0";




    function ctaxUpdatePanelEndCall(s, e) {
        console.log(ctaxUpdatePanel.cpstock);
        if (ctaxUpdatePanel.cpstock != null) {
            //divAvailableStk.style.display = "block";
            //divpopupAvailableStock.style.display = "block";

            var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
            document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
      

            ctaxUpdatePanel.cpstock = null;

        }

        if (fromColumn == 'product') {
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
            fromColumn = '';
        }
        return;
    }


    function CmbWarehouseEndCallback(s, e) {
        if (SelectWarehouse != "0") {
            cCmbWarehouse.SetValue(SelectWarehouse);
            SelectWarehouse = "0";
        }
        else {
            cCmbWarehouse.SetEnabled(true);
        }

        // Changes
        if (cCmbWarehouse.cpwarehouseid == "Y") {
            cCmbWarehouse.cpwarehouseid = null;

            var WarehouseID = cCmbWarehouse.GetValue();
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS" || type == "WB") {
                cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
            }
            else if (type == "WS") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
            }
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

        //kaushik 29-7-2017
        ctxtProductAmount.SetValue(parseFloat(Math.round(sumAmount * 100) / 100).toFixed(2));
        ctxtProductTaxAmount.SetValue(parseFloat(Math.round(sumTaxAmount * 100) / 100).toFixed(2));
        ctxtProductDiscount.SetValue(parseFloat(Math.round(sumDiscount * 100) / 100).toFixed(2));
        ctxtProductNetAmount.SetValue(parseFloat(Math.round(sumNetAmount * 100) / 100).toFixed(2));
        //ctxtProductAmount.SetValue(Math.round(sumAmount).toFixed(2));
        //ctxtProductTaxAmount.SetValue(Math.round(sumTaxAmount).toFixed(2));
        //ctxtProductDiscount.SetValue(Math.round(sumDiscount).toFixed(2));
        //ctxtProductNetAmount.SetValue(Math.round(sumNetAmount).toFixed(2));
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

        ctxtQuoteTaxTotalAmt.SetValue(Math.round(totalTaxAmount));
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
        //var altUOM = ctxtALTUOM.GetValue(); 
        var altUOM = (ccmbPackingUom1.GetValue() != null) ? ccmbPackingUom1.GetValue() : "0";
        var AltQty = (ctxtAltQuantity.GetValue() != null) ? ctxtAltQuantity.GetValue() : "0";


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
            cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID + '~' + AltQty + '~' + altUOM);
            SelectedWarehouseID = "0";
        }
    }

    var IsPostBack = "";
    var PBWarehouseID = "";
    var PBBatchID = "";


    $(document).ready(function () {

        if ($("#hdnShowUOMConversionInEntry").val() == "1")
        {
            div_AltQuantity.style.display = 'block';
            dv_AltUOM.style.display = 'block';
        }
        else
        {
            div_AltQuantity.style.display = 'none';
            dv_AltUOM.style.display = 'none';
         
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
        //var selectedItems = checkListBox.GetSelectedItems();
        //selectedChkValue = GetSelectedItemsText(selectedItems);
        ////checkComboBox.SetText(GetSelectedItemsText(selectedItems));
        //checkComboBox.SetText(selectedItems.length + " Items");

        //var val = GetSelectedItemsText(selectedItems);
        //$("#abpl").attr('data-content', val);
    }
    function SynchronizeListBoxValues(dropDown, args) {
        checkListBox.UnselectAll();
        // var texts = dropDown.GetText().split(textSeparator);
        var texts = selectedChkValue.split(textSeparator);

        var values = GetValuesByTexts(texts);
        checkListBox.SelectValues(values);
        UpdateSelectAllItemState();
        UpdateText(); // for remove non-existing texts
    } function GetSelectedItemsCount(items) {

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
                $('#lblPackingStk').val(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#lblStkQty').val(QuantityValue);
            $('#lblStkUOM').val(strStkUOM);
            $('#lblProduct').val(strProductName);
            $('#lblbranchName').val(strBranch);

            //if (ProductID != "0") {
            //   cacpAvailableStock.PerformCallback(strProductID);
            //}
        }




        function PsGotFocusFromID(s, e) {

            $("#pageheaderContent").attr('style', 'display:block');
            divAvailableStk.style.display = "block";

            var ProductID = (grid.GetEditor('ProductDisID').GetText() != null && grid.GetEditor('ProductDisID').GetText() != "") ? grid.GetEditor('ProductDisID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            //  grid.batchEditApi.StartEdit(globalRowIndex);
            //  grid.GetEditor("ProductID").SetText(LookUpData);
            //  grid.GetEditor("Product").Focus(ProductCode);


            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = $("[id*=ddl_Branch]").find("option:selected").val();
            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID + "~" + strBranch);
            }
        }
        function ProductsGotFocusFromID(s, e) {

            //grid.batchEditApi.StartEdit(globalRowIndex);
            //grid.GetEditor("ProductID").SetText(LookUpData);
            //grid.GetEditor("ProductName").Focus(ProductCode);

            $("#pageheaderContent").attr('style', 'display:block');
            divAvailableStk.style.display = "block";
            var ProductID = (grid.GetEditor('ProductID').GetText() != null && grid.GetEditor('ProductID').GetText() != "") ? grid.GetEditor('ProductID').GetText() : "0";
            var strProductName = (grid.GetEditor('ProductID').GetText() != null && grid.GetEditor('ProductID').GetText() != "") ? grid.GetEditor('ProductID').GetText() : "0";
            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";
            var ProductdisID = (grid.GetEditor('ProductDisID').GetText() != null && grid.GetEditor('ProductDisID').GetText() != "") ? grid.GetEditor('ProductDisID').GetText() : "0";

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
                $('#lblPackingStk').val(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#lblStkQty').val(QuantityValue);
            $('#lblStkUOM').val(strStkUOM);
            $('#lblProduct').val(strProductName);
            $('#lblbranchName').val(strBranch);

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = $("[id*=ddl_Branch]").find("option:selected").val();
            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID + "~" + strBranch);
            }
            else { cacpAvailableStock.PerformCallback(ProductdisID + "~" + strBranch); }
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

        function ProductButnClick(s, e) {
            if (e.buttonIndex == 0) {
                var CID = GetObjectID('hdnCustomerId').value;
                if (CID != null && CID != "") {

                    setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

                    $('#txtProdSearch').val('');
                    $('#ProductModel').modal('show');
                }
                else {
                    jAlert("Please Select a Vendor", "Alert", function () { ctxtCustName.Focus(); });
                }
            }
        }



        function ProductDisKeyDown(s, e) {
            console.log(e.htmlEvent.key);
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {

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
                    jAlert("Please Select a Vendor", "Alert", function () { ctxtCustName.Focus(); });
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

            $('#lblStkQty').val("0.00");
            $('#lblStkUOM').val(strStkUOM);
            $('#lblProduct').val(strDescription);
            $('#lblbranchName').val(strBranch);

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#lblPackingStk').val(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }
            //divPacking.style.display = "none";

            //lblbranchName lblProduct
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");
            //Debjyoti
            fromColumn = 'product';
            ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
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
            var productdsc = productall[0];
            grid.GetEditor("ProductDisID").SetText(productdsc);
            grid.GetEditor("Product").SetText(ProductCode);


            grid.batchEditApi.StartEdit(globalRowIndex, 3);

        }

        function ProductDisSelected(s, e) {
            var LookUpData = cproductDisLookUp.GetValue();
            //  var LookUpData = cproductDisLookUp.GetGridView().GetRowKey(cproductDisLookUp.GetGridView().GetFocusedRowIndex());
            if (LookUpData == null)
                return;
            var ProductCode = cproductDisLookUp.GetText();
            // var ProductCode = cproductDisLookUp.GetValue();
            if (!ProductCode) {
                LookUpData = null;
            }
            cProductpopUpdis.Hide();
            //grid.batchEditApi.StartEdit(globalRowIndex);
            grid.batchEditApi.StartEdit(globalRowIndex, 3);
            var productall = LookUpData.split('||')

            var productdsc = productall[0];
            grid.GetEditor("ProductDisID").SetText(productdsc);
            grid.GetEditor("Product").SetText(ProductCode);

            grid.batchEditApi.StartEdit(globalRowIndex, 3);
        }

        function ProductSelected(s, e) {
            //if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
            //    cProductpopUp.Hide();
            //    grid.batchEditApi.StartEdit(globalRowIndex, 7);
            //    return;
            //}

            //var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            //if (LookUpData == null)
            //    return;

            //var ProductCode = cproductLookUp.GetValue();
            //if (!ProductCode) {
            //    LookUpData = null;
            //}

            if (!cproductLookUp.FindItemByValue(cproductLookUp.GetValue())) {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                jAlert("Product not Exists.", "Alert", function () { cproductLookUp.SetValue(); cproductLookUp.Focus(); });
                return;
            }

            var LookUpData = cproductLookUp.GetValue();
            var ProductCode = cproductLookUp.GetText();
            //  var quote_Id = gridquotationLookup.GetValue();





            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);
            //  console.log(LookUpData);
            $("#pageheaderContent").attr('style', 'display:block');
            cddl_AmountAre.SetEnabled(false);

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
            // tbSalePrice.SetValue(strSalePrice);
            // if (quote_Id == null) {
            tbSalePrice.SetValue(strSalePrice);
            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");
            //  }
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            $('#lblStkQty').val("0.00");
            $('#lblStkUOM').val(strStkUOM);
            $('#lblProduct').val(strDescription);
            $('#lblbranchName').val(strBranch);

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#lblPackingStk').val(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }
            //divPacking.style.display = "none";

            //lblbranchName lblProduct
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");
            //Debjyoti
            fromColumn = 'product';
            ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
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


        

     
                function SettingTabStatus() {
                    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                    }
                }

        function disp_prompt(name) {

            if (name == "tab0") {
                //  gridLookup.Focus();
                ctxtCustName.Focus();
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
                    page.tabs[0].SetEnabled(false);
                    //fn_PopOpen();
                }
            }
        }
        var canCallBack = true;

        function AllControlInitilize() {

            // debugger;
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
    


     


