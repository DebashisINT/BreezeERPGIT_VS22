
            $(function () {
                $('#UOMModal').on('hide.bs.modal', function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 6);
});
});

   var taxSchemeUpdatedDate = 'Convert.ToString(Cache["SchemeMaxDate"])%>';



   function deleteTax(Action, srl, productid) {
       var OtherDetail = {};
       OtherDetail.Action = Action;
       OtherDetail.srl = srl;
       OtherDetail.prodid = productid;


       $.ajax({
    type: "POST",
    url: "ReturnNormal.aspx/taxUpdatePanel_Callback",
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



   function GlobalBillingShippingEndCallBack() {
       if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
           cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";        
       var startDate = new Date();
       startDate = tstartdate.GetDate().format('yyyy/MM/dd');
       var branchid = $('#ddl_Branch').val();
       if (gridquotationLookup.GetValue() != null) {
    // var key = cCustomerComboBox.GetValue();
           var key = $('#hdnCustomerId').val();
           if (key != null && key != '') {
    // cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
               BindContactPerson(key, branchid);
                   var startDate = new Date();
                   startDate = tstartdate.GetDate().format('yyyy/MM/dd');                      
                   if (key != null && key != '') {
    // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');
}
                   grid.PerformCallback('GridBlank');
                   ccmbGstCstVat.PerformCallback();
                   ccmbGstCstVatcharge.PerformCallback();
    // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                   deleteTax("DeleteAllTax", "", "");
}               
}
else {            

    //   var key = cCustomerComboBox.GetValue();
           var key = $('#hdnCustomerId').val();
           if (key != null && key != '') {
    // cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
               BindContactPerson(key, branchid);
    //cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%');                  
}
}
}
}

   function GlobalBillingShippingDetails()
{
       var startDate = new Date();
       startDate = tstartdate.GetDate().format('yyyy/MM/dd');
       var branchid = $('#ddl_Branch').val();
       if (gridquotationLookup.GetValue() != null) {
    // var key = cCustomerComboBox.GetValue();
           var key = $('#hdnCustomerId').val();
           if (key != null && key != '') {
    // cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
               BindContactPerson(key, branchid);
               var startDate = new Date();
               startDate = tstartdate.GetDate().format('yyyy/MM/dd');
               if (key != null && key != '') {
    // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');
}
               grid.PerformCallback('GridBlank');
               ccmbGstCstVat.PerformCallback();
               ccmbGstCstVatcharge.PerformCallback();
    // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
               deleteTax("DeleteAllTax", "", "");
}
}
else {

    //   var key = cCustomerComboBox.GetValue();
           var key = $('#hdnCustomerId').val();
           if (key != null && key != '') {
    // cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
               BindContactPerson(key, branchid);
    //cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%');                  
}
}
}


    function CmbScheme_ValueChange()
    {
        // cddlPosGstReturnNormal.ClearItems();
        PosGstId = "";
        ctxtCustName.SetText("");
    }

function AfterSaveBillingShipiing(validate) {
    //GetPurchaseForGstValue();
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

    cddlPosGstReturnNormal.ClearItems();
    if (cddlPosGstReturnNormal.GetItemCount() == 0) {
        cddlPosGstReturnNormal.AddItem(GetShippingStateName() + '[Shipping]', "S");
        cddlPosGstReturnNormal.AddItem(GetBillingStateName() + '[Billing]', "B");
    }

    else if (cddlPosGstReturnNormal.GetItemCount() > 2) {
        cddlPosGstReturnNormal.ClearItems();
        //cddl_PosGstSalesOrder.RemoveItem(0);
        //cddl_PosGstSalesOrder.RemoveItem(0);
    }
    if (cddlPosGstReturnNormal.GetItemCount() > 0) {
        if (cddlPosGstReturnNormal.GetItemCount() > 1) {
            //cddlPosGstReturnManual.GetSelectedItems(1);
            cddlPosGstReturnNormal.SetSelectedIndex(2);
        }
        else if (cddlPosGstReturnNormal.GetItemCount() == 1) {
            //cddlPosGstReturnManual.SetValue("S");
            cddlPosGstReturnNormal.SetSelectedIndex(1);
        }
    }
    //if (PosGstId == "" || PosGstId == null) {
    //    cddlPosGstReturnNormal.SetValue("S");
    //}
    //else {
    //    cddlPosGstReturnNormal.SetValue(PosGstId);
    //}
}

var PosGstId = "";
function PopulateReturnNormalPosGst(e) {

    PosGstId = cddlPosGstReturnNormal.GetValue();
    if (PosGstId == "S") {
        cddlPosGstReturnNormal.SetValue("S");
    }
    else if (PosGstId == "B") {
        cddlPosGstReturnNormal.SetValue("B");
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
var ProducttaggAmount = "0";
var ProductGetTotalAmount = "0";
var ProductSaleprice = "0";
var globalNetAmount = 0;
var ProductDiscount = "0";
function AmtGotFocus() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;
}
function AmtTextChange(s, e) {

    debugger;
    var grossamt = grid.GetEditor('Amount').GetValue();
    var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(parseFloat(grossamt) + parseFloat(_TotalTaxAmt));          
    var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
        //grid.GetEditor('TaxAmount').SetValue(0);
        // ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
        deleteTax("DelQtybySl", grid.GetEditor("SrlNo").GetValue(), "");
    }

    //TDSDetail();
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
var SimilarProjectStatus = "0";
function CloseGridQuotationLookup() {
    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();
    gridquotationLookup.Focus();

    var quotetag_Id = (gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex()));

    debugger;
    if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {

        var quote_Id = "";
        quote_Id = quotetag_Id;
        // otherDets.quote_Id = quote_Id;
        //for (var i = 0; i < quotetag_Id.length; i++) {
        //    if (quote_Id == "") {
        //        quote_Id = quotetag_Id[i];
        //    }
        //    else {
        //        quote_Id += ',' + quotetag_Id[i];
        //    }
        //}
        var Doctype = $("#rdl_SalesInvoice").find(":checked").val();
        debugger;
        $.ajax({
            type: "POST",
            url: "ReturnNormal.aspx/DocWiseSimilarProjectCheck",
            data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                SimilarProjectStatus = msg.d;
                debugger;
                if (SimilarProjectStatus != "1") {
                    $("#txt_InvoiceDate").val('');
                    clookup_Project.Clear();
                    jAlert("Please select document with same project code to proceed.");

                    return false;

                }
            }
        });
    }

}

function componentEndCallBack(s, e) {         
    gridquotationLookup.gridView.Refresh();
    if (grid.GetVisibleRowsOnPage() == 0) {
        OnAddNewClick();
    }

    if (cQuotationComponentPanel.cpDetails != null) {
        var details = cQuotationComponentPanel.cpDetails;
        cQuotationComponentPanel.cpDetails = null;
        var SpliteDetails = details.split("~");
        var Reference = SpliteDetails[0];
        var Currency_Id = (SpliteDetails[1] == "" || SpliteDetails[1] == null) ? "0" : SpliteDetails[1];
        var SalesmanId = (SpliteDetails[2] == "" || SpliteDetails[2] == null) ? "0" : SpliteDetails[2];               
        var CurrencyRate = SpliteDetails[4];
        var Contact_person_id = SpliteDetails[5];
        var Tax_option = (SpliteDetails[6] == "" || SpliteDetails[6] == null) ? "1" : SpliteDetails[6];
        var Tax_Code = (SpliteDetails[7] == "" || SpliteDetails[7] == null) ? "0" : SpliteDetails[7];
        // var Name = (SpliteDetails[8] == "" || SpliteDetails[8] == null) ? "0" : SpliteDetails[8];
        //  cddlPosGstReturnNormal.SetValue(Name);
        $('#txt_Refference').val(Reference);
        ctxt_Rate.SetValue(CurrencyRate);
        cddl_AmountAre.SetValue(Tax_option); if (Tax_option == 1) {
            grid.GetEditor('TaxAmount').SetEnabled(true);
            cddlVatGstCst.SetEnabled(false);
            cddlVatGstCst.SetSelectedIndex(0);
            cbtn_SaveRecords.SetVisible(true);                   
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

function componentDateEndCallBack()
{
    var PlaceOfSupplyValue = $('#hdnPlaceOfSupply').val();
    var SplitValue = PlaceOfSupplyValue.split("~");
    var PlaceText = SplitValue[0];
    var PlaceValue = SplitValue[1];
    // cddlPosGstReturnNormal.SetValue($('#hdnPlaceOfSupply').val());
    //cddlPosGstReturnNormal.items.AddItem($('#hdnPlaceOfSupply').val());
           
    //// cddlPosGstReturnManual.ClearItems();
    // if (cddlPosGstReturnManual.GetItemCount() == 0)
    // {
    cddlPosGstReturnNormal.AddItem(PlaceText, PlaceValue);
    cddlPosGstReturnNormal.SetText(PlaceText);
    // }
    // else if (cddlPosGstSReturn.GetItemCount() > 1) {
    //     cddlPosGstReturnManual.ClearItems();
    // }
    // //if (cddlPosGstReturnManual.GetItemCount() > 0) {
    // //    cddlPosGstReturnManual.SetSelectedIndex(1);
    // //}
}

function ChangeState(value) {

    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}

function BindOrderProjectdata(OrderId, TagDocType) {
    // debugger;
    var OtherDetail = {};

    OtherDetail.OrderId = OrderId;
    OtherDetail.TagDocType = TagDocType;


    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "ReturnNormal.aspx/SetProjectCode",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async:false,
            success: function (msg) {
                var Code = msg.d;

                clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                clookup_Project.SetEnabled(false);
            }
        });

        var projID = clookup_Project.GetValue();

        $.ajax({
            type: "POST",
            url: 'ReturnNormal.aspx/getHierarchyID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ProjID: projID }),
            async: false,
            success: function (msg) {
                var data = msg.d;
                $("#ddlHierarchy").val(data);
            }
        });

    }
}
      
function PerformCallToGridBind() {
    grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
    // cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
    $('#hdnPageStatus').val('Invoiceupdate');
    cProductsPopup.Hide();
    //cddlPosGstReturnNormal.SetEnabled(false);
    //#### added by Samrat Roy for Transporter Control #############           
    var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

    var Invoice_Id = (gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex()));
    if (Invoice_Id.length > 0) {
        BindOrderProjectdata(Invoice_Id, 'SI');
    }

    return false;
}
function QuotationNumberChanged() {
    if (SimilarProjectStatus != "-1") {
        var quote_Id = gridquotationLookup.GetValue();
        var KeyVal;
        KeyVal = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
        if (quote_Id != null) {
            var arr = quote_Id.split(',');
            if (arr.length > 1) {
                $('#txt_InvoiceDate').val('Multiple Select Invoice Dates');
            }
            else {
                if (arr.length == 1) {

                    cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id);
                    // var PlaceOfSupplyValue = $('#hdnPlaceOfSupply').val();
                    // var SplitValue = PlaceOfSupplyValue.split("~");
                    // var PlaceText = SplitValue[0];
                    // var PlaceValue = SplitValue[1];
                    //// cddlPosGstReturnNormal.SetValue($('#hdnPlaceOfSupply').val());
                    //// cddlPosGstReturnNormal.items.AddItem($('#hdnPlaceOfSupply').val());
                    // cddlPosGstReturnNormal.AddItem(PlaceText, PlaceValue);

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
            cgridproducts.PerformCallback('BindProductsDetails' + '~' + KeyVal);
            cProductsPopup.Show();
        }
        else {
            grid.PerformCallback('RemoveDisplay');

        }
        cddl_AmountAre.SetEnabled(false);
        ctxt_Rate.SetEnabled(false);
        document.getElementById("ddl_Currency").disabled = true;
    }
}
//.............Available Stock Div Show............................


function acpAvailableStockEndCall(s, e) {          
    if (cacpAvailableStock.cpstock != null) {
        divAvailableStk.style.display = "block";             
        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;              
        document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
        document.getElementById('lblAvailableStk').innerHTML = cacpAvailableStock.cpstock;
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
        //run code for Alt + n -- ie, Save & New
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
}

//transporter
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
    debugger;
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
        
    debugger;
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
                var strStkUOM = SpliteDetails[4];                       
                var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                if (strRate == 0) {
                    strRate = 1;
                }

                var StockQuantity = strMultiplier * QuantityValue;                 

                var Amount = parseFloat(Math.round(QuantityValue * strFactor * (strSalePrice / strRate) * 100) / 100).toFixed(2);
                //clblTaxProdGrossAmt.SetText(DecimalRoundoff(grid.GetEditor('Amount').GetValue(), 2));
                //clblProdNetAmt.SetText(parseFloat((Math.round(Amount * 100)) / 100).toFixed(2));
                //document.getElementById('HdProdGrossAmt').value = grid.GetEditor('Amount').GetValue();
                //document.getElementById('HdProdNetAmt').value = Amount;
                //End Here
                //kaushik 24-1-2018

                clblProdNetAmt.SetText(DecimalRoundoff(grid.GetEditor('Amount').GetValue(), 2));
                clblTaxProdGrossAmt.SetText(parseFloat((Math.round(Amount * 100)) / 100).toFixed(2));

                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
                var discountAmt = (grid.GetEditor('Discount').GetValue() / 100);

                var netAmt = Amount - (Amount * discountAmt);



                document.getElementById('HdProdGrossAmt').value = Amount;
                document.getElementById('HdProdNetAmt').value = netAmt;

                //document.getElementById('HdProdNetAmt').value = grid.GetEditor('Amount').GetValue();
                //document.getElementById('HdProdGrossAmt').value = Amount;
                //kaushik 24-1-2018
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
                if (cddl_AmountAre.GetValue() == "1") {
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
                else if (cddl_AmountAre.GetValue() == "2") {
                    $('.GstCstvatClass').show();
                    $('.gstGrossAmount').hide();
                    $('.gstNetAmount').hide();
                    clblTaxableGross.SetText("");
                    clblTaxableNet.SetText("");

                    var shippingStCode = '';

                    ////###### Added By : Samrat Roy ##########
                    //Get Customer Shipping StateCode

                    //if (cddlPosGstReturnNormal.GetValue() == "S") {
                    //    shippingStCode = GeteShippingStateCode();
                    //}
                    //else {
                    //    shippingStCode = GetBillingStateCode();
                    //}

                    ///Chinmoy started for place of Supply

                    var PlaceOfSupplyValue = $('#hdnPlaceOfSupply').val();
                    var SplitValue = PlaceOfSupplyValue.split("~");
                    var PlaceText = SplitValue[0];
                    var PlaceValue = SplitValue[1];
                    var placeStateCode = SplitValue[2];
                    shippingStCode = placeStateCode;

                         
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
                grid.batchEditApi.StartEdit(globalRowIndex, 14);
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


    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
        ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);
        var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
        var ddValue = parseFloat(ctxtGstCstVat.GetValue());
        ctxtTaxTotAmt.SetValue(gridValue + ddValue);
        cgridTax.cpUpdated = "";
    }
    else {
        //var totAmt = ctxtTaxTotAmt.GetValue();
        //cgridTax.CancelEdit();
        //caspxTaxpopUp.Hide();
        //grid.batchEditApi.StartEdit(globalRowIndex, 16);             
        //grid.GetEditor("TaxAmount").SetValue(totAmt);

        //if (cddl_AmountAre.GetValue() == "1") {
        //    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
        //}
        //else if (cddl_AmountAre.GetValue() == "2") {
        //    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
        //}

        var totAmt = ctxtTaxTotAmt.GetValue();
        cgridTax.CancelEdit();
        caspxTaxpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 14);
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
        //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);



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


//function cgridTax_EndCallBack(s, e) {
//    debugger;
//    var totalGst = 0;
//    $('.cgridTaxClass').show();
//    cgridTax.StartEditRow(0);
//    //check Json data
//    if (cgridTax.cpJsonData) {
//        if (cgridTax.cpJsonData != "") {
//            taxJson = JSON.parse(cgridTax.cpJsonData);
//            cgridTax.cpJsonData = null;
//        }
//    }
//    //End Here
//    if (cgridTax.cpComboCode) {
//        if (cgridTax.cpComboCode != "") {
//            if (cddl_AmountAre.GetValue() == "1") {
//                var selectedIndex;
//                for (var i = 0; i < ccmbGstCstVat.GetItemCount() ; i++) {
//                    if (ccmbGstCstVat.GetItem(i).value.split('~')[0] == cgridTax.cpComboCode) {
//                        selectedIndex = i;
//                    }
//                }
//                if (selectedIndex && ccmbGstCstVat.GetItem(selectedIndex) != null) {
//                    ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
//                }
//                cmbGstCstVatChange(ccmbGstCstVat);
//                cgridTax.cpComboCode = null;
//            }
//        }
//    }

         
//    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
//        ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);
//        var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
//        var ddValue = parseFloat(ctxtGstCstVat.GetValue());
//        ctxtTaxTotAmt.SetValue(gridValue + ddValue);
//        cgridTax.cpUpdated = "";
//    }
//    else {
//        var totAmt = ctxtTaxTotAmt.GetValue();
//        cgridTax.CancelEdit();
//        caspxTaxpopUp.Hide();
//        grid.batchEditApi.StartEdit(globalRowIndex, 16);
//        grid.GetEditor("TaxAmount").SetValue(totAmt);              
//        grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));



//        if (cddl_AmountAre.GetValue() == "2") {
//            totalGst = parseFloat(grid.GetEditor("TaxAmount").GetValue());
//            var qty = grid.GetEditor("Quantity").GetValue();
//            var price = grid.GetEditor("SalePrice").GetValue();
//            var Discount = grid.GetEditor("Discount").GetValue();

//            var finalAmt = qty * price;


//            //if (GSTType=="G")
//            //    grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - totalGst), 2));
//            //else if (GSTType == "N") {

//            grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - (finalAmt * (Discount / 100)) - totalGst), 2));
//            grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
//        }
//    }

//    if (cgridTax.GetVisibleRowsOnPage() == 0) {
//        $('.cgridTaxClass').hide();
//        ccmbGstCstVat.Focus();
//    }
           
//    SetRunningTotal();
//    ShowTaxPopUp("IY");
//}

function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}


    $(document).ready(function () {
        if ($('#hdnPageStatus').val() == "update")
        {
            LoadCustomerBillingShippingAddress(GetObjectID('hdnCustomerId').value);
            LoadBranchAddressInEditMode($('#ddl_Branch').val());
               
            cddlPosGstReturnNormal.SetEnabled(false);
            // PopulateReturnNormalPosGst();

        }


        if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
            page.GetTabByName('Billing/Shipping').SetEnabled(false);
        }
        $('#ApprovalCross').click(function () {              
            window.parent.popup.Hide();
            window.parent.cgridPendingApproval.Refresh()();
        })
    })

         //<%--kaushik 24-2-2017--%>
    function UniqueCodeCheck() {          
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
                    alert('Sales Return Normal No length cannot be more than ' + x);                       
                    $('#txt_PLQuoteNo').val('');
                    $('#txt_PLQuoteNo').focus();

                }
                else {
                    var CheckUniqueCode = false;
                    $.ajax({
                        type: "POST",
                        url: "ReturnNormal.aspx/CheckUniqueCode",
                        data: JSON.stringify({ ReturnNo: ReturnNo }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            CheckUniqueCode = msg.d;
                            if (CheckUniqueCode == true) {
                                alert('Please enter unique Sales Return Normal No');                                  
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
    // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
    deleteTax("DeleteAllTax", "", "");
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
function GridCallBack() {
           
    $('#ddl_numberingScheme').focus();
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
    $("#pageheaderContent").attr('style', 'display:block');
    cddl_AmountAre.SetEnabled(false);

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
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue()); 
    deleteTax("DelProdbySl", grid.GetEditor("SrlNo").GetValue(), "");
}
function cmbContactPersonEndCall(s, e) {
    LoadingPanel.Hide();
    if (cContactPerson.cpDueDate != null) {
        var DeuDate = cContactPerson.cpDueDate;
        var myDate = new Date(DeuDate);
        cdt_SaleInvoiceDue.SetDate(myDate);
        cContactPerson.cpDueDate = null;
    }

    if (cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) {

        $("#divGSTN").attr('style', 'display:block');
        document.getElementById('lblGSTIN').innerHTML = cContactPerson.cpGSTN;
        cContactPerson.cpGSTN = null;
    }

    if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {            

        $("#pageheaderContent").attr('style', 'display:block');             

        $("#divOutstanding").attr('style', 'display:block');
        document.getElementById('lblOutstanding').innerHTML = cContactPerson.cpOutstanding;
        cContactPerson.cpOutstanding = null;
    }
    else {
        $("#pageheaderContent").attr('style', 'display:none');             
        $("#divOutstanding").attr('style', 'display:none');
        document.getElementById('lblOutstanding').innerHTML = '';
    }

}


function cgridproductsEndCall(s, e) {
    if (cgridproducts.cpFinancier != null && cgridproducts.cpFinancier != undefined) {
        $('#hdnisFinancier').val(cgridproducts.cpFinancier);

    }
    if (cgridproducts.cpOldUnit != null && cgridproducts.cpOldUnit != undefined) {
        $('#hdnisOldUnit').val(cgridproducts.cpOldUnit);
    }
    cgridproducts.cpFinancier = null;
    cgridproducts.cpOldUnit = null;
}
function cAspxOldUnitGridEndCall(s, e) {
    if (cAspxOldUnitGrid.cpProductInfo != null && cAspxOldUnitGrid.cpProductInfo != undefined) {
        $('#hdnOldUnitProdInfo').val(cAspxOldUnitGrid.cpProductInfo);
        cPopup_OldUnit.Hide();
        grid.UpdateEdit();
    }
    cAspxOldUnitGrid.cpProductInfo = null;
}



function Save_ButtonClick() {
    LoadingPanel.Show();
    flag = true;
    grid.batchEditApi.EndEdit();
    var ReasonforRet = $('#txtReasonforChange').val();
    ReasonforRet = ReasonforRet.trim();
    if (ReasonforRet == '' || ReasonforRet == null) {
        $('#MandatoryReasonforChange').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatoryReasonforChange').attr('style', 'display:none');
    }


    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        flag = false;
    }

    if ((cddlPosGstReturnNormal.GetValue() == "") || (cddlPosGstReturnNormal.GetValue() == null))
    {
        jAlert('Please enter valid place of supply.')
        flag = false;
    }
    // Quote no validation Start
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
    var invoice_Id = gridquotationLookup.GetValue();

    if (invoice_Id == null) {
        $('#MandatorysSCno').attr('style', 'display:block');
        flag = false;

    }
    else {
        $('#MandatorysSCno').attr('style', 'display:none');
    }


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
    if (amtare == '1') {
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
                        url: "ReturnNormal.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            // var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";
                            $('#hdfLookupCustomer').val(customerval);
                            // Custom Control Data Bind

                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('N');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                    });
                }
                else {

                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    // var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";
                    $('#hdfLookupCustomer').val(customerval);
                    // Custom Control Data Bind

                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('N');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    cacbpCrpUdf.PerformCallback();
                }
            }
            else {

                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ReturnNormal.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            // var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";
                            $('#hdfLookupCustomer').val(customerval);
                            // Custom Control Data Bind

                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('N');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                    });
                }
                else {

                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    // var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";
                    $('#hdfLookupCustomer').val(customerval);
                    // Custom Control Data Bind

                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('N');
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
    } else { LoadingPanel.Hide(); }

}

function SaveExit_ButtonClick() {
    //debugger;
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

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        flag = false;
    }


    if ((cddlPosGstReturnNormal.GetValue() == "") || (cddlPosGstReturnNormal.GetValue() == null)) {
        jAlert('Please enter valid place of supply.')
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
    var invoice_Id = gridquotationLookup.GetValue();

    if (invoice_Id == null) {
        $('#MandatorysSCno').attr('style', 'display:block');
        flag = false;

    }
    else {
        $('#MandatorysSCno').attr('style', 'display:none');
    }

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
    if (amtare == '1') {
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
                        url: "ReturnNormal.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            //  var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";         
                            $('#hdfLookupCustomer').val(customerval);
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                            //kaushik
                        }
                    });
                }
                else {



                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    //  var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";         
                    $('#hdfLookupCustomer').val(customerval);
                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    cacbpCrpUdf.PerformCallback();
                    //kaushik
                }
            }
            else {

                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ReturnNormal.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            //  var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";         
                            $('#hdfLookupCustomer').val(customerval);
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                            //kaushik
                        }
                    });
                }
                else {

                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    //  var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";         
                    $('#hdfLookupCustomer').val(customerval);
                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('E');
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


function SalePriceGotFocus() {
    ProductSaleprice = grid.GetEditor("SalePrice").GetValue();


    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());

}
function QuantityGotFocus(s, e) {
    debugger;
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    $("#hdnTagQnty").val(QuantityValue);
    ProductGetQuantity = QuantityValue;
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());


    //debugger;
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
    ProducttaggAmount = grid.GetEditor('SalePrice').GetValue();
    var isOverideConvertion = SpliteDetails[24];
    var packing_saleUOM = SpliteDetails[23];
    var sProduct_SaleUom = SpliteDetails[22];
    var sProduct_quantity = SpliteDetails[20];
    var packing_quantity = SpliteDetails[18];

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
            var isOverideConvertion = SpliteDetails[23];
            var packing_saleUOM = SpliteDetails[22];
            var sProduct_SaleUom = SpliteDetails[21];
            var sProduct_quantity = SpliteDetails[19];
            var packing_quantity = SpliteDetails[17];

            if (SpliteDetails[16] != '') {
                if (parseFloat(SpliteDetails[16]) > 0) {
                    gridPackingQty = SpliteDetails[16];
                }
            }
            if (SpliteDetails[24] == "1") {
                IsInventory = 'Yes';
            }
        }
        else if (SpliteDetails.length == 25) {
            var isOverideConvertion = SpliteDetails[23];
            var packing_saleUOM = SpliteDetails[22];
            var sProduct_SaleUom = SpliteDetails[21];
            var sProduct_quantity = SpliteDetails[19];
            var packing_quantity = SpliteDetails[17];

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
    //chinmoy added due to empty 2nd uom quantity
    var ComponentID = (grid.GetEditor('ComponentID').GetText() != null) ? grid.GetEditor('ComponentID').GetText() : "";


    if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 1) {
        if (ComponentID != "" && ComponentID != null) {
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMultiUOMDetails",
                data: JSON.stringify({ orderid: strProductID, action: 'SalesReturnPackingQty', module: 'SalesReturn', strKey: ComponentID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    gridPackingQty = msg.d;
                    if ($("#hdnPageStatus").val() == "update")
                    {
                        gridPackingQty = SpliteDetails[16];
                    }

                    ShowUOM(type, "Sale Return Normal", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                   

                }
            });
        }
        else {
           
            ShowUOM(type, "Sale Return Normal", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
        }
    }
    //chinmoy commented due to empty 2nd uom quantity
    //if (ShowUOMConversionInEntry == "1"  && SpliteDetails.length > 1) {
    //    ShowUOM(type, "Sale Return Normal", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
    //}

   
    //Surojit 12-03-2019


       
   
}

var issavePacking = 0;

function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('Quantity').SetValue(Quantity);
    QuantityTextChange(null,null);
 
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
    debugger;
    $("#pageheaderContent").attr('style', 'display:block');
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)) {
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";       
        // var key = cCustomerComboBox.GetValue();
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
                        grid.batchEditApi.StartEdit(globalRowIndex, 8);
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

    //if (ProductSaleprice != parseFloat(Saleprice)) {
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
//}


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

        //   ShippingStateCode = cddlPosGstSReturn.GetValue()
        ShippingStateCode = cddlPosGstReturnNormal.GetValue();
        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[25], Amount, amountAfterDiscount, strTax, ShippingStateCode, $('#ddl_Branch').val())


                  
        // }
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

        // ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
        deleteTax('DelQtybySl', grid.GetEditor("SrlNo").GetValue(), "");
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
        $('#hdnIsInvoiceTax').val('2');
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cPopup_Taxes.Hide();
}

var Warehouseindex;


function callback_InlineRemarks_EndCall(s, e) {

    if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
        $("#txtInlineRemarks").focus();
    }
    else {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
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


function OnCustomButtonClick(s, e) {
    //debugger;
    if (e.buttonID == 'CustomDelete') {
        var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
        grid.batchEditApi.EndEdit();

        $('#hdnDeleteSrlNo').val(SrlNo);
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        if (gridquotationLookup.GetValue() != null) {
            var messege = "";
            messege = "Cannot Delete using this button as the Sales Invoice is linked with this Sale Return.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
            jAlert(messege, 'Alert Dialog: [Delete Sales Invoice Products]', function (r) {
            });

        }
        else {
            if (noofvisiblerows != "1") {
                grid.DeleteRow(e.visibleIndex);
                $('#hdfIsDelete').val('D');
                grid.UpdateEdit();                     
                grid.PerformCallback('Display');                      
                $('#hdnPageStatus').val('delete');
                    
            }
        }
    }
    else if (e.buttonID == 'AddNew') {               
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
                grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            }
        }
        else {
            QuotationNumberChanged();
        }
    }


    else if (e.buttonID == "addlDesc") {

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
    }


    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

        var Invoicenumber = (grid.GetEditor('ComponentNumber').GetValue() != null) ? grid.GetEditor('ComponentNumber').GetValue() : "0";

        if (QuantityValue == "0.0") {
            jAlert("Quantity should not be zero !.");
        } else {
            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();                 
            if (ProductID != "") {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strMultiplier = SpliteDetails[7];
                var strProductName = (grid.GetEditor('ProductName').GetText() != null) ? grid.GetEditor('ProductName').GetText() : "";
                var StkQuantityValue = QuantityValue * strMultiplier;
                $('#hdnInnumber').val(Invoicenumber);
                $('#hdfProductIDPC').val(strProductID);
                $('#hdfProductType').val("");
                $('#hdfProductSerialID').val(SrlNo);
                $('#hdfProductSerialID').val(SrlNo);
                $('#hdnProductQuantity').val(QuantityValue);
                var Ptype = "";

                $('#hdnisserial').val("");
                $('#hdnisbatch').val("");
                $('#hdniswarehouse').val("");

                $.ajax({
                    type: "POST",
                    url: 'ReturnNormal.aspx/getProductType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{Products_ID:\"" + strProductID + "\"}",
                    success: function (type) {
                        Ptype = type.d;
                        $('#hdfProductType').val(Ptype);
                        if (Ptype == "") {
                            jAlert("No Warehouse or Batch or Serial is actived !.");
                        } else {                          
                            if (Ptype == "W") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("true");                             

                            }
                            else if (Ptype == "B") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");

                            }
                            else if (Ptype == "S") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");

                            }
                            else if (Ptype == "WB") {

                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("true");                              
                            }
                            else if (Ptype == "WS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("true");                               
                            }
                            else if (Ptype == "WBS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("true");
                             
                            }
                            else if (Ptype == "BS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("false");
                            }
                            else {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");
                            }

                            $("#RequiredFieldValidatortxtbatch").css("display", "none");
                            $("#RequiredFieldValidatortxtserial").css("display", "none");
                            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");

                            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");

                            $(".blockone").css("display", "none");
                            $(".blocktwo").css("display", "none");
                            $(".blockthree").css("display", "none");

                            ctxtqnty.SetText("0.0");
                            ctxtqnty.SetEnabled(true);

                            ctxtbatchqnty.SetText("0.0");
                            ctxtserial.SetText("");
                            ctxtbatchqnty.SetText("");

                            ctxtbatch.SetEnabled(true);
                            cCmbWarehouse.SetEnabled(true);

                            $('#hdnoutstock').val("0");
                            $('#hdnisedited').val("false");
                            $('#hdnisoldupdate').val("false");
                            $('#hdnisnewupdate').val("false");

                            $('#hdnisolddeleted').val("false");

                            $('#hdntotalqntyPC').val(0);
                            $('#hdnoldrowcount').val(0);
                            $('#hdndeleteqnity').val(0);
                            $('#hidencountforserial').val("1");

                            $('#hdfstockidPC').val(0);
                            $('#hdfopeningstockPC').val(0);
                            $('#oldopeningqntity').val(0);
                            $('#hdnnewenterqntity').val(0);

                            $('#hdnenterdopenqnty').val(0);
                            $('#hdbranchIDPC').val(0);

                            $('#hdnisviewqntityhas').val("false");


                            $('#hdndefaultID').val("");
                            $('#hdnbatchchanged').val("0");
                            $('#hdnrate').val("0");
                            $('#hdnvalue').val("0");
                            $('#hdnstrUOM').val(strUOM);                          
                            var branchid = $("#ddl_Branch option:selected").val();
                            $('#hdnisreduing').val("false");
                            var productid = strProductID ? strProductID : "";
                            var StkQuantityValue = QuantityValue ? QuantityValue : "0.0000";
                            var stockids = SpliteDetails[10];
                            var checkstockdecimalornot = StkQuantityValue.toString().split(".")[1]
                            $('#hdnpcslno').val(SrlNo);                          
                            var ProductName = SpliteDetails[12];
                            var ratevalue = "0";
                            var rate = "0";                          
                            var branchid = $('#ddl_Branch').val();                           
                            var BranchNames = $("#ddl_Branch option:selected").text();                          
                            var strProductID = productid;
                            var strDescription = "";
                            var strUOM = (strUOM != null) ? strUOM : "0";
                            var strProductName = ProductName;
                            document.getElementById('lblbranchName').innerHTML = BranchNames;
                            var availablestock = SpliteDetails[11];
                            $('#hdndefaultID').val("0");
                            $('#hdfstockidPC').val(stockids);
                            var calculateopein = Number(StkQuantityValue) - Number(availablestock);
                            var oldopeing = 0;
                            var oldqnt = Number(oldopeing);
                            $('#hdfopeningstockPC').val(QuantityValue);
                            $('#oldopeningqntity').val(0);
                            $('#hdnnewenterqntity').val(QuantityValue);
                            $('#hdnenterdopenqnty').val(0);
                            $('#hdbranchIDPC').val(branchid);
                            $('#hdnselectedbranch').val(branchid);
                            $('#hdnrate').val(rate);
                            $('#hdnvalue').val(ratevalue);
                            var dtd = (Number(StkQuantityValue)).toFixed(4);
                            $("#lblopeningstock").text(dtd + " " + strUOM);
                            ctxtmkgdate.SetDate = null;
                            txtexpirdate.SetDate = null;
                            ctxtserial.SetValue("");
                            ctxtbatch.SetValue("");
                            ctxtqnty.SetValue("0.0");
                            ctxtbatchqnty.SetValue("0.0");

                            var hv = $('#hdnselectedbranch').val();

                            var iswarehousactive = $('#hdniswarehouse').val();
                            var isactivebatch = $('#hdnisbatch').val();
                            var isactiveserial = $('#hdnisserial').val();                        

                            cCmbWarehouse.PerformCallback('BindWarehouse');

                            if (iswarehousactive == "true") {
                                cCmbWarehouse.SetVisible(true);
                                cCmbWarehouse.SetSelectedIndex(1);
                                cCmbWarehouse.Focus();
                                ctxtqnty.SetVisible(true);
                                $('#hdniswarehouse').val("true");

                                $(".blockone").css("display", "block");

                            } else {
                                cCmbWarehouse.SetVisible(false);
                                ctxtqnty.SetVisible(false);
                                $('#hdniswarehouse').val("false");
                                cCmbWarehouse.SetSelectedIndex(-1);
                                $(".blockone").css("display", "none");

                            }

                            if (isactivebatch == "true") {

                                ctxtbatch.SetVisible(true);
                                ctxtmkgdate.SetVisible(true);
                                ctxtexpirdate.SetVisible(true);
                                $('#hdnisbatch').val("true");
                                $(".blocktwo").css("display", "block");

                            } else {
                                ctxtbatch.SetVisible(false);
                                ctxtmkgdate.SetVisible(false);
                                ctxtexpirdate.SetVisible(false);
                                $('#hdnisbatch').val("false");
                                $(".blocktwo").css("display", "none");

                            }
                            if (isactiveserial == "true") {
                                ctxtserial.SetVisible(true);
                                $('#hdnisserial').val("true");
                                $(".blockthree").css("display", "block");
                            } else {
                                ctxtserial.SetVisible(false);
                                $('#hdnisserial').val("false");
                                $(".blockthree").css("display", "none");
                            }

                            if (iswarehousactive == "false" && isactivebatch == "true") {
                                ctxtbatchqnty.SetVisible(true);
                                $(".blocktwoqntity").css("display", "block");
                            } else {
                                ctxtbatchqnty.SetVisible(false);
                                $(".blocktwoqntity").css("display", "none");
                            }

                            if (iswarehousactive == "false" && isactivebatch == "true") {
                                ctxtbatch.Focus();
                            } else {
                                cCmbWarehouse.Focus();
                            }

                            cbtnWarehouse.SetVisible(true);
                            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

                            cPopup_WarehousePCPC.Show();
                        }
                    }
                });
            }


        }
    }
}

function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
}

function OnWarehouseEndCallback(s, e) {
    var Ptype = document.getElementById('hdfProductType').value;

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 6);
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
    if (ctaxUpdatePanel.cpstock != null) {            
        divAvailableStk.style.display = "block";              
        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
        document.getElementById('lblAvailableStk').innerHTML = ctaxUpdatePanel.cpstock;
        ctaxUpdatePanel.cpstock = null;

    }

    if (fromColumn == 'product') {
        grid.batchEditApi.StartEdit(globalRowIndex, 8);
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
    ctxtProductAmount.SetValue(parseFloat(Math.round(sumAmount * 100) / 100).toFixed(2));
    ctxtProductTaxAmount.SetValue(parseFloat(Math.round(sumTaxAmount * 100) / 100).toFixed(2));
    ctxtProductDiscount.SetValue(parseFloat(Math.round(sumDiscount * 100) / 100).toFixed(2));
    ctxtProductNetAmount.SetValue(parseFloat(Math.round(sumNetAmount * 100) / 100).toFixed(2));        
    clblChargesTaxableGross.SetText("");
    clblChargesTaxableNet.SetText("");

    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "1") {

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
    else if (cddl_AmountAre.GetValue() == "2") {
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

//Set Running Total for Charges And Tax 
function SetChargesRunningTotal() {

    //debugger;
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
                //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));  
                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));      
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
              
        gridTax.batchEditApi.EndEdit();
    }
}

/////////////////// QuotationTaxAmountTextChange By Sam on 23022017
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
    SetChargesRunningTotal();
}


function RecalCulateTaxTotalAmountCharges() {
    var totalTaxAmount = 0;
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        if (chargejsonTax[i].SchemeName != "") {
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
    checkComboBox.SetText(selectedItems.length + " Items");
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


function ProductlookUpdisKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUpdis.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 3);
    }
}


function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 4);
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







    function ProductKeyDown(s, e) {
        console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Enter") {

            s.OnButtonClick(0);
        }
        //if (e.htmlEvent.key == "Tab") {

        //    s.OnButtonClick(0);
        //}
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
    //if (e.htmlEvent.key == "Tab") {

    //    s.OnButtonClick(0);
    //}
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
function ProductDisSelected(s, e) {        
    // var LookUpData = cproductDisLookUp.GetValue();
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
    cddlPosGstReturnNormal.SetEnabled(false);
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
    //divPacking.style.display = "none";

    //lblbranchName lblProduct
    //tbStkUOM.SetValue(strStkUOM);
    //tbStockQuantity.SetValue("0");
    //Debjyoti
    fromColumn = 'product';
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());\
    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "")
    grid.batchEditApi.StartEdit(globalRowIndex, 8);
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

function ProductSelected(s, e) {
    var quote_Id = gridquotationLookup.GetValue();

    if (!cproductLookUp.FindItemByValue(cproductLookUp.GetValue())) {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 8);
        jAlert("Product not Exists.", "Alert", function () { cproductLookUp.SetValue(); cproductLookUp.Focus(); });
        return;
    }

    var LookUpData = cproductLookUp.GetValue();
    var ProductCode = cproductLookUp.GetText();
    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);         
    $("#pageheaderContent").attr('style', 'display:block');
    cddl_AmountAre.SetEnabled(false);

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
          
    //Debjyoti
    fromColumn = 'product';
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
    deleteTax("DelProdbySl", grid.GetEditor("SrlNo").GetValue(), "");
    grid.batchEditApi.StartEdit(globalRowIndex, 8);
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

     function selectValue() {
         //  debugger;
         var checked = $('#rdl_SalesInvoice').attr('checked', true);
         if (checked) {
             $(this).attr('checked', false);
         }
         else {
             $(this).attr('checked', true);
         }
         var startDate = new Date();
         startDate = tstartdate.GetDate().format('yyyy/MM/dd');
          
         var key = $('#hdnCustomerId').val();
           
         var type = ($("[id$='rdl_SalesInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SalesInvoice']").find(":checked").val() : "";

         if (key == null || key == "") {
             jAlert("Customer required !", 'Alert Dialog: [Sales Invoice]', function (r) {
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

    function grid_ProductsEndCallback(s, e) {

        cddl_AmountAre.SetValue(cgridproducts.cpTaxTypeid);
        cddlVatGstCst.PerformCallback('Tax-code' + '~' + '');//Tax_Code
        if (cgridproducts.cpTaxType == "refreshGrid") {
             
        }
    }


    function clookup_Project_LostFocus() {
        grid.batchEditApi.StartEdit(-1, 2);
        //Hierarchy Start Tanmoy
        var projID = clookup_Project.GetValue();
        if (projID == null || projID == "") {
            $("#ddlHierarchy").val(0);
        }
        //Hierarchy End Tanmoy
    }

    function clookup_project_GotFocus() {

        //clookup_Project.gridView.Refresh();
        clookup_Project.ShowDropDown();
    }

    //Hierarchy Start Tanmoy
    function ProjectValueChange(s, e) {

        var projID = clookup_Project.GetValue();

        $.ajax({
            type: "POST",
            url: 'ReturnNormal.aspx/getHierarchyID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ProjID: projID }),
            success: function (msg) {
                var data = msg.d;
                $("#ddlHierarchy").val(data);
            }
        });
    }