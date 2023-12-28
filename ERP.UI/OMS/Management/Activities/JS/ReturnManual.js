//================================================== Revision History =============================================
//Rev Number         DATE              VERSION         DEVELOPER           CHANGES
//1.0                12-06-2023        V2.0.38          Priti            SALES RETURN MNUAL Save & New Button issue.
//2.0                21-07-2023        V2.0.39          Priti            0026601: Manual Sale Return is getting duplicated after Saving using Alt+X
//3.0                15-12-2023        V2.0.41          Priti            0026779: Adding Transporter Details required in Sale Return - Manual module

//====================================================== Revision History =============================================

var FocusValue = '0';

 $(function () {
        $('#UOMModal').on('hide.bs.modal', function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 6);
                 });
});
 
    var StateCodeList = [];
    function GlobalBillingShippingEndCallBack() {
        var startDate = new Date();
        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
        var branchid = $('#ddl_Branch').val();
    //  var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
        var key = $('#hdnCustomerId').val();
    //  var key = ctxtCustName.GetValue();
        if (key != null && key != '') {


    //cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
             }

        else 
        {

        }
   }









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


function deleteTax(Action, srl, productid) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;
    OtherDetail.prodid = productid;


    $.ajax({
        type: "POST",
        url: "ReturnManual.aspx/taxUpdatePanel_Callback",
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

function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}



function GetCustomerStateCode(key) {

    $.ajax({
        type: "POST",
        url: "ReturnManual.aspx/GetCustomerStateCode",
        data: JSON.stringify({ Key: key }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (r) {
            var contactPersonJsonObject = r.d;
            debugger;
            $("#hdnCustomerStateCodeId").val();
        }
    });
}


function BindContactPerson(key, branchid) {

    if (key != null && key != '') {
        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/GetContactPersonafterBillingShipping",
            data: JSON.stringify({ Key: key }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (r) {
                var contactPersonJsonObject = r.d;
                //cContactPerson.SetValue(contactPerson);
                //IsContactperson=false;
                SetDataSourceOnComboBox(cContactPerson, contactPersonJsonObject);
                //SetFocusAfterBillingShipping();                            
            }
        });



        $.ajax({
            type: "POST",
            url: "ReturnManual.aspx/GetContactPersonRelatedData",
            data: JSON.stringify({ Key: key, branchid: branchid }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (r) {

                var contactPersonJsonObject = r.d;
                var SpliteDetails = contactPersonJsonObject.split("@");

                //cContactPerson.SetValue(contactPerson);
                //IsContactperson = false;

                //cContactPerson.value = parseInt(SpliteDetails[0]);
                //cContactPerson.text = SpliteDetails[1];
                debugger;
                $('#lblOutstanding').val(SpliteDetails[0]);
                $('#lblGSTIN').val(SpliteDetails[1]);
               
                //SetDataSourceOnComboBox(cContactPerson, contactPersonJsonObject);
                //SetFocusAfterBillingShipping();
            }
        });
    }

}
function GlobalBillingShipping() {
    var startDate = new Date();
    startDate = tstartdate.GetDate().format('yyyy/MM/dd');
    var branchid = $('#ddl_Branch').val();
    //  var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
    //var key = $('#<%=hdnCustomerId.ClientID %>').val();
    var key = GetObjectID('hdnCustomerId').value;
    //  var key = ctxtCustName.GetValue();
    if (key != null && key != '') {


        //cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
        BindContactPerson(key, branchid);
        //if ($("#hdnProjectSelectInEntryModule").val() == "1")
        //    cProjectCallBack.PerformCallback();
    }

    else {

    }
}


function closeMultiUOM(s, e) {
    e.cancel = false;
    // cPopup_MultiUOM.Hide();
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
        url: "ReturnManual.aspx/GetPackingQuantity",
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
                uomfactor = parseFloat(packingQuantity / sProduct_quantity);
                //.toFixed(4);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = $("#UOMQuantity").val();
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock);
            //.toFixed(4);

            //$("#AltUOMQuantity").val(calcQuantity);
            cAltUOMQuantity.SetValue(calcQuantity);

        }
    });
}


function SaveMultiUOM() {


    //grid.GetEditor('ProductID').GetText().split("||@||")[3];

    var qnty = $("#UOMQuantity").val();


    var UomId = ccmbUOM.GetValue();
    //var UomId = ccmbUOM.SetSelectedIndex(grid.GetEditor('ProductID').GetText().split("||@||")[3] - 1);
    var UomName = ccmbUOM.GetText();
    //var AltQnty = parseFloat($("#AltUOMQuantity").val()).toFixed(4);
    var AltQnty = cAltUOMQuantity.GetValue();
    var AltUomId = ccmbSecondUOM.GetValue();
    var AltUomName = ccmbSecondUOM.GetText();
    // Mantis Issue 24831
    grid.batchEditApi.StartEdit(globalRowIndex);
    // End of Mantis Issue 24831
    var srlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var DetailsId = grid.GetEditor('DetailsId').GetText();

    // Mantis Issue 24831
    var BaseRate = ccmbBaseRate.GetValue();
    var AltRate = ccmbAltRate.GetValue();


    if (ProductID == "") {
        ProductID = hdProductID.value;
    }
    if (DetailsId == "") {
        DetailsId = "0";
    }
    var UpdateRow = 'False';
    if ($("#chkUpdateRow").prop("checked")) {
        UpdateRow = 'True';
    }
    // End of Mantis Issue 24831

    // Mantis Issue 24831
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "") {

    //    cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId);

    //    //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
    //    cAltUOMQuantity.SetValue("0.0000");
    //}
    //else {
    //    return;
    //}

    if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty!="0.0000") {
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
            if (cbtnMUltiUOM.GetText() == "Update") {
                cgrid_MultiUOM.PerformCallback('UpdateRow~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow + '~' + hdMultiUOMID);
                cAltUOMQuantity.SetValue("0.0000");
                $("#UOMQuantity").val(0);
                ccmbBaseRate.SetValue(0);
                cAltUOMQuantity.SetValue(0);
                ccmbAltRate.SetValue(0);
                ccmbSecondUOM.SetValue("");
                cgrid_MultiUOM.cpAllDetails = "";
                cbtnMUltiUOM.SetText("Add");
                $("#chkUpdateRow").prop('checked', false);
                $("#chkUpdateRow").removeAttr("checked");
            }
            else {
                cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow);
                cAltUOMQuantity.SetValue("0.0000");
                $("#UOMQuantity").val(0);
                ccmbBaseRate.SetValue(0)
                cAltUOMQuantity.SetValue(0)
                ccmbAltRate.SetValue(0)
                ccmbSecondUOM.SetValue("")
                $("#chkUpdateRow").prop('checked', false);
                $("#chkUpdateRow").removeAttr("checked");
            }
        }
        else {
            return;
        }
    }
    else {
        return;
    }
    // End of Mantis Issue 24831
}
function Delete_MultiUom(keyValue, SrlNo, DetailsId) {


    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo + '~' + DetailsId);
    cgrid_MultiUOM.cpDuplicateAltUOM = "";

}

    // Mantis Issue 24831
function Edit_MultiUom(keyValue, SrlNo) {

    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);

}
    // End of Mantis Issue 24831

function OnMultiUOMEndCallback(s, e) {
    if (cgrid_MultiUOM.cpDuplicateAltUOM == "DuplicateAltUOM") {
        jAlert("Please Enter Different Alt. Quantity.");
        return;
    }
    // Mantis Issue 24831
    if (cgrid_MultiUOM.cpSetBaseQtyRateInGrid != null && cgrid_MultiUOM.cpSetBaseQtyRateInGrid == "1") {
        grid.batchEditApi.StartEdit(hdVisiableIndex.value, 6);
        
        var BaseQty = cgrid_MultiUOM.cpBaseQty;
        var BaseRate = cgrid_MultiUOM.cpBaseRate;
        var AltUom = cgrid_MultiUOM.cpAltUom;
        var AltQty = cgrid_MultiUOM.cpAltQty;

     
        grid.GetEditor("Quantity").SetValue(BaseQty);
        grid.GetEditor("SalePrice").SetValue(BaseRate);
        grid.GetEditor("Amount").SetValue(BaseQty * BaseRate);

        grid.GetEditor("Order_AltUOM").SetValue(AltUom);
        grid.GetEditor("Order_AltQuantity").SetValue(AltQty);
        
        SalePriceTextChange(null, null);
        
    }
    if (cgrid_MultiUOM.cpAllDetails == "EditData") {
        var Quan = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
        $('#UOMQuantity').val(Quan);
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
    // End of Mantis Issue 24831
    if (cgrid_MultiUOM.cpOpenFocus == "OpenFocus") {
        ccmbSecondUOM.SetFocus();
    }

}


function FinalMultiUOM() {
    UomLenthCalculation();

    if (Uomlength == 0 || Uomlength < 0) {
        // Mantis Issue 24831
        //jAlert("Please add Alt. Quantity.");
        jAlert("Please add atleast one Alt. Quantity with Update Row as checked.");
        // End of Mantis Issue 24831
        return;
    }

    else {
        cPopup_MultiUOM.Hide();
        // Mantis Issue 24831
        grid.batchEditApi.StartEdit(hdVisiableIndex.value);
        var SLNo = grid.GetEditor('SrlNo').GetValue();
        cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~' + SLNo);
        // End of Mantis Issue 24831
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 12);
        }, 200)
    }
}


function AutoPopulateMultiUOM() {

    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    $.ajax({
        type: "POST",
        url: "ReturnManual.aspx/AutoPopulateAltQuantity",
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
                uomfactor = parseFloat(packingQuantity / sProduct_quantity);
                //.toFixed(4);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = QuantityValue;
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock);
            //.toFixed(4);
            if ($("#hdnPageStatus").val() == "update") {
                ccmbSecondUOM.SetValue('');
                //$("#AltUOMQuantity").val(calcQuantity);

                //cAltUOMQuantity.SetValue(calcQuantity);
                cAltUOMQuantity.SetValue("0.0000");
            }
            else {
                //ccmbSecondUOM.SetValue(AltUOMId);
                if (AltUOMId == 0) {
                    ccmbSecondUOM.SetValue('');
                }
                else {
                    ccmbSecondUOM.SetValue(AltUOMId);
                }
                // Mantis Issue 24831
                //cAltUOMQuantity.SetValue(calcQuantity);
                // End of Mantis Issue 24831
            }
        }
    });
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
            else if (indexName == "BillingAreaIndex") {
                SetBillingArea(Id, name);
            }
            else if (indexName == "ShippingAreaIndex") {
                SetShippingArea(Id, name);
            }
            else if (indexName == "customeraddressIndex") {
                SetCustomeraddress(Id, name)
            }
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
            else if (indexName == "BillingAreaIndex") {
                $('#txtbillingArea').focus();
            }
            else if (indexName == "ShippingAreaIndex") {
                $('#txtshippingArea').focus();
            }
            else if (indexName == "customeraddressIndex") {
                ('#txtshippingShipToParty').focus();
            }
            //if (indexName == "customerIndex")
            //    $('#txtCustSearch').focus();

        }
    }

}






    function CmbScheme_ValueChange() {
        // cddlPosGstReturnManual.ClearItems();
        PosGstId = "";
        ctxtCustName.SetText("");
    }


function AfterSaveBillingShipiing(validate) {
    // GetPurchaseForGstValue();
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



//function GetPurchaseForGstValue() {

//    cddlPosGstReturnManual.ClearItems();
//    if (cddlPosGstReturnManual.GetItemCount() == 0) {
//        cddlPosGstReturnManual.AddItem(GetShippingStateName() + '[Shipping]', "S");
//        cddlPosGstReturnManual.AddItem(GetBillingStateName() + '[Billing]', "B");
//    }

//    else if (cddlPosGstReturnManual.GetItemCount() > 2) {
//        cddlPosGstReturnManual.ClearItems();
//        //cddl_PosGstSalesOrder.RemoveItem(0);
//        //cddl_PosGstSalesOrder.RemoveItem(0);
//    }

//    if (PosGstId == "" || PosGstId == null) {
//        cddlPosGstReturnManual.SetValue("S");
//    }
//    else {
//        cddlPosGstReturnManual.SetValue(PosGstId);
//    }
//}


function GetPurchaseForGstValue() {

    var BillingDetails = $.grep(StateCodeList, function (e) { return e.add_addressType == "Billing" })
    var ShippingDetails = $.grep(StateCodeList, function (e) { return e.add_addressType == "Shipping" })

    if (BillingDetails.length > 0) {
        $('#hdnPOSBillingStateId').val(BillingDetails[0].id);
        $('#hdnPOSBillingStateCode').val(BillingDetails[0].StateCode);
    }
    if (ShippingDetails.length > 0) {
        $('#hdnPOSShippingStateId').val(ShippingDetails[0].id);
        $('#hdnPOSShippingStateCode').val(ShippingDetails[0].StateCode);
    }
    cddlPosGstReturnManual.ClearItems();
    if (cddlPosGstReturnManual.GetItemCount() == 0) {

        //---------------Code commented start Mantis Id::0019502----------------------------------------
        //cddlPosGstSReturn.AddItem(GetShippingStateName() + '[Shipping]', "S");
        //cddlPosGstSReturn.AddItem(GetBillingStateName() + '[Billing]', "B");
        //---------------Code commented end Mantis Id::0019502----------------------------------------
        if (ShippingDetails.length > 0) {
            cddlPosGstReturnManual.AddItem(ShippingDetails[0].state + "[Shipping]", "S");
        }
        if (BillingDetails.length > 0) {
            cddlPosGstReturnManual.AddItem(BillingDetails[0].state + "[Billing]", "B");
        }
    }
    else if (cddlPosGstReturnManual.GetItemCount() > 2) {
        cddlPosGstReturnManual.ClearItems();
        //cddl_PosGstSalesOrder.RemoveItem(0);
        //cddl_PosGstSalesOrder.RemoveItem(0);
    }
    //if (cddlPosGstReturnManual.GetItemCount() == 1)
    //{
    //    cddlPosGstReturnManual.SetSelectedIndex(1);
    //}
    // else
    if ((cddlPosGstReturnManual.GetValue() == "" || cddlPosGstReturnManual.GetValue() == null) && (ShippingDetails.length > 0)) {
        cddlPosGstReturnManual.SetValue("S");
        cddlPosGstReturnManual.SetText(ShippingDetails[0].state + "[Shipping]");
    }
    else if ((cddlPosGstReturnManual.GetValue() == "" || cddlPosGstReturnManual.GetValue() == null) && (ShippingDetails.length == 0)) {
        cddlPosGstReturnManual.SetValue("B");
        cddlPosGstReturnManual.SetText(BillingDetails[0].state + "[Billing]");
    }



    //if (cddlPosGstReturnManual.GetItemCount() > 0)
    //{
    //    if (cddlPosGstReturnManual.GetItemCount() > 1)
    //    {
    //        //cddlPosGstReturnManual.GetSelectedItems(1);
    //        cddlPosGstReturnManual.SetValue(ShippingDetails[0].state + "[Shipping]");
    //    }
    //    else if (cddlPosGstReturnManual.GetItemCount() == 1)
    //    {
    //        //cddlPosGstReturnManual.SetValue("S");
    //        cddlPosGstReturnManual.SetSelectedIndex(1);
    //    }
    //}



    //---------------Code commented start Mantis Id::0019502----------------------------------------
    //if (cddlPosGstReturnManual.GetValue() == "" || cddlPosGstReturnManual.GetValue() == null) {
    //    cddlPosGstReturnManual.SetValue("S");
    //}
    //else {
    //    cddlPosGstReturnManual.SetValue(PosGstId);
    //}
    //---------------Code commented end Mantis Id::0019502----------------------------------------
}



var PosGstId = "";
function PopulateReturnManualPosGst(e) {
    PosGstId = cddlPosGstReturnManual.GetValue();
    var BillingDetails = $.grep(StateCodeList, function (e) { return e.add_addressType == "Billing" })
    var ShippingDetails = $.grep(StateCodeList, function (e) { return e.add_addressType == "Shipping" })
    PosGstId = cddlPosGstReturnManual.GetValue();
    debugger;
    if (PosGstId == "S") {
        if (ShippingDetails.length > 0) {
            cddlPosGstReturnManual.SetValue("S");
            cddlPosGstReturnManual.SetText(ShippingDetails[0].state + "[Shipping]");
        }
    }
    else if (PosGstId == "B") {
        if (BillingDetails.length > 0) {
            cddlPosGstReturnManual.SetValue("B");
            cddlPosGstReturnManual.SetText(BillingDetails[0].state + "[Billing]");
        }
    }
}


function StateCodeListLoad() {
    var CustomerID;
    CustomerID = $("#hdnCustomerId").val();



    //Written By chinmoy for place of supply 
    var OtherDetail = {};
    OtherDetail.CustomerID = CustomerID;
    $.ajax({
        type: "POST",
        url: "ReturnManual.aspx/GetCustomerStateCodeDetails",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            StateCodeList = msg.d;
            if (StateCodeList[0].TransactionType != "") {
                $("#drdTransCategory").val(StateCodeList[0].TransactionType);
            }
            //  $('#hdnPOSStateId').val(StateCodeList[0].id);
            //$('#hdnPOSStateCode').val(StateCodeList[0].StateCode);

        },
        error: function (msg) {
            jAlert('Please try again later');
        }
    });
}

$(document).ready(function () {
    if ($('#hdnPageStatus').val() == "update") {
        //StateCodeListLoad();
        cddlPosGstReturnManual.SetEnabled(false);
        PopulateReturnManualPosGst();
        // $("#ddlPosGstReturnManual: selected").text();
        // GetPurchaseForGstValue();
        LoadCustomerBillingShippingAddress(GetObjectID('hdnCustomerId').value);
        LoadBranchAddressInEditMode($('#ddl_Branch').val());
    }


    //$("#txtReasonforChange").on('change', function () {

    //    LoadingPanel.Hide();
    //});


});

var ProductGetQuantity = "0";
var ProductAfterSalesPrice = "0";
var ProductGetTotalAmount = "0";
var ProductSaleprice = "0";
var globalNetAmount = 0;
var ProductDiscount = "0";
var _TotalAmount = 0;
function AmtGotFocus() {
    // globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    globalNetAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    ProductGetTotalAmount = globalNetAmount;

    //SetTotalTaxableAmount(s, e);
}
function AmtTextChange(s, e) {

    var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var TaxAmount = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    //var grossamt = grid.GetEditor('Amount').GetValue();
    //var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(parseFloat(Amount) + parseFloat(TaxAmount));

    //var _TotalAmount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {


        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(Amount + TaxAmount);

        ////////////////// For Tax

        var ProductID = grid.GetEditor('ProductID').GetValue();
        var SpliteDetails = ProductID.split("||@||");
        // var ShippingStateCode = $("#bsSCmbStateHF").val();

        var CompareStateCode;
        var CompareStateId;
        if (cddlPosGstReturnManual.GetValue() == "S") {
            // CompareStateCode = GeteShippingStateCode();
            CompareStateCode = $('#hdnPOSShippingStateCode').val();
            CompareStateId = $('#hdnPOSShippingStateId').val();
        }
        else {
            // CompareStateCode = GetBillingStateCode();
            CompareStateCode = $('#hdnPOSBillingStateCode').val();
            CompareStateId = $('#hdnPOSBillingStateId').val();
        }

              
        // var ShippingStateCode = cbsSCmbState.GetValue();
        var TaxType = "";
        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }



        if (CompareStateCode != null && CompareStateCode != '' && CompareStateCode != '0') {
            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[17], Amount, Amount, TaxType, CompareStateId, $('#ddl_Branch').val(), '');
        }

        // grid.GetEditor('TaxAmount').SetValue(0);
        //  ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
        //alert('hi');
        SetTotalTaxableAmount(s, e);
        SetInvoiceLebelValue();


    }

    //TDSDetail();
}
//contactperson phone
function acpContactPersonPhoneEndCall(s, e) {
    if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
        pageheaderContent.style.display = "block";
        $("#divContactPhone").attr('style', 'display:block');
        document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = cacpContactPersonPhone.cpPhone;
        cacpContactPersonPhone.cpPhone = null;

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
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
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

    //  alert('hhhhhh');

    LoadingPanel.Hide();
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
        //var ExpiryDate = SpliteDetails[3];
        var CurrencyRate = SpliteDetails[4];
        var Contact_person_id = SpliteDetails[5];
        var Tax_option = (SpliteDetails[6] == "" || SpliteDetails[6] == null) ? "1" : SpliteDetails[6];

        var Tax_Code = (SpliteDetails[7] == "" || SpliteDetails[7] == null) ? "0" : SpliteDetails[7];
        $('#<%=txt_Refference.ClientID %>').val(Reference);
        ctxt_Rate.SetValue(CurrencyRate);
        cddl_AmountAre.SetValue(Tax_option); if (Tax_option == 1) {

            grid.GetEditor('TaxAmount').SetEnabled(true);
            cddlVatGstCst.SetEnabled(false);

            cddlVatGstCst.SetSelectedIndex(0);
            cbtn_SaveRecords.SetVisible(true);
            // grid.GetEditor('ProductID').Focus();
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
function ShowMSG(s,e)
{
    alert('');
}
function ChangeState(value) {

    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}

//function PerformCallToGridBind() {
//    // ;
//    grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
//    cProductsPopup.Hide();
//    return false;
//}




function PerformCallToGridBind() {
    grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
    cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
    $('#hdnPageStatus').val('Invoiceupdate');
    cProductsPopup.Hide();
    cddlPosGstReturnManual.SetEnabled(false);
    //#### added by Samrat Roy for Transporter Control #############

    var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
    callTransporterControl(quote_Id[0], 'SI');

    return false;
}
function QuotationNumberChanged() {
    // ;
    //  console.log(0);
    //  
    var quote_Id = gridquotationLookup.GetValue();
    if (quote_Id != null) {
        var arr = quote_Id.split(',');
        if (arr.length > 1) {
            ctxt_InvoiceDate.SetText('Multiple Select Invoice Dates');
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
        grid.PerformCallback('RemoveDisplay');

    }

}
//.............Available Stock Div Show............................


function acpAvailableStockEndCall(s, e) {
    //   alert('kk');

    if (cacpAvailableStock.cpstock != null) {
        divAvailableStk.style.display = "block";
        //   divpopupAvailableStock.style.display = "block";

        var AvlStk = cacpAvailableStock.cpstock + " " + $('#lblStkUOM').val();
        // alert(AvlStk);
        $('#lblAvailableStkPro').val(AvlStk);
        $('#lblAvailableStk').val(cacpAvailableStock.cpstock);
      


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

    //alert(event.keyCode);
    if (event.keyCode == 18) isCtrl = true;
    if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") {
        //  alert('kkk'); //run code for Alt + n -- ie, Save & New  78 //  alert('kkk'); //run code for Alt + x -- ie, Save & New  83

        //  alert('kkk');
        StopDefaultAction(e);
        //Rev 2.0
        //Save_ButtonClick();
        if (document.getElementById('btn_SaveRecords').style.display != 'none') {
            Save_ButtonClick();
        }
        //Rev 2.0 End
    }
    else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for alt+X -- ie, Save & Exit!   
        // alert('kkk222');
        StopDefaultAction(e);
        //Rev 2.0
        //SaveExit_ButtonClick();
        if (document.getElementById('ASPxButton2').style.display != 'none') {
            SaveExit_ButtonClick();
        }
         //Rev 2.0 End
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
    else if (event.keyCode == 79 && event.altKey == true) { //run code for alt+O -- ie, Billing Shipping!   
        StopDefaultAction(e);
        if (page.GetActiveTabIndex() == 1) {
            fnSaveBillingShipping();
        }
    }
    else if (event.keyCode == 75 && event.altKey == true) { //run code for Alt+K -- ie, Press OK for tax charges!   
        //StopDefaultAction(e);
        BatchUpdate();

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
                    if (document.getElementById('btn_SaveRecords').style.display != 'none') {
                        Save_ButtonClick();
                    } 
                }
                break;
            case 88:
                StopDefaultAction(e);
                if (getUrlVars().req != "V") {
                    //Rev 2.0
                    //SaveExit_ButtonClick();
                    if (document.getElementById('ASPxButton2').style.display != 'none') {
                        SaveExit_ButtonClick();
                    }
                    //Rev 2.0 End
                }
                break;
            case 120:
                StopDefaultAction(e);
                if (getUrlVars().req != "V") {
                    //Rev 2.0
                    //SaveExit_ButtonClick();
                    if (document.getElementById('ASPxButton2').style.display != 'none') {
                        SaveExit_ButtonClick();
                    }
                    //Rev 2.0 End
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

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    else if (e.htmlEvent.key == "Tab") {
        grid.batchEditApi.StartEdit(globalRowIndex, 13);
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
                var TaxAmount = parseFloat(ProdAmt * s.GetText()) / 100;
                cgridTax.GetEditor("Amount").SetValue(parseFloat((TaxAmount * 100) / 100).toFixed(2));

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {
                var TaxAmount = (parseFloat(ProdAmt * s.GetText()) / 100) * -1;
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat((TaxAmount * 100) / 100).toFixed(2));

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

function taxAmtButnClick(s, e) {
    //debugger;
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
                // clblTaxProdGrossAmt.SetText(DecimalRoundoff(grid.GetEditor('Amount').GetValue(), 2));

                //clblTaxProdGrossAmt.SetText(parseFloat(grid.GetEditor('Amount').GetValue()).toFixed(2));
                //clblProdNetAmt.SetText(parseFloat((Math.round(Amount * 100)) / 100).toFixed(2));



                //document.getElementById('HdProdGrossAmt').value = Amount;
                //document.getElementById('HdProdNetAmt').value = grid.GetEditor('Amount').GetValue();

                //kaushik 24-1-2018

                var Amountfor = QuantityValue * strFactor * (strSalePrice / strRate);
                var discountAmtfor = (grid.GetEditor('Discount').GetValue() / 100);
                var netAmt = Amountfor - (Amountfor * discountAmtfor);
                clblProdNetAmt.SetText(DecimalRoundoff(grid.GetEditor('Amount').GetValue(), 2));
                clblTaxProdGrossAmt.SetText(parseFloat((Math.round(Amount * 100)) / 100).toFixed(2));
                //document.getElementById('HdProdNetAmt').value = grid.GetEditor('Amount').GetValue();
                document.getElementById('HdProdNetAmt').value = netAmt;
                document.getElementById('HdProdGrossAmt').value = Amount;
                //kaushik 24-1-2018

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
                    //var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
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







                    // Date: 31-05-2017    Author: Kallol Samanta  [START]
                    // Details: Billing/Shipping user control integration

                    //Get Customer Shipping StateCode
                    //Old Code
                    //var shippingStCode = '';
                    //if (cchkBilling.GetValue()) {
                    //    shippingStCode = CmbState.GetText();
                    //}
                    //else {
                    //    shippingStCode = CmbState1.GetText();
                    //}

                    //New Code
                    var shippingStCode = '';


                    // Date: 31-05-2017    Author: Kallol Samanta  [END]
                    if (cddlPosGstReturnManual.GetValue() == "S") {
                        //shippingStCode = GeteShippingStateCode();
                        shippingStCode = $('#hdnPOSShippingStateCode').val();

                        // shippingStCode = $('#hdnPOSShippingStateId').val();
                    }
                    else {
                        // shippingStCode = GetBillingStateCode();
                        shippingStCode = $('#hdnPOSBillingStateCode').val();
                        // shippingStCode = $('#hdnPOSBillingStateId').val();
                    }


                    shippingStCode = shippingStCode;

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
                //grid.batchEditApi.StartEdit(globalRowIndex, 13);
            }
        }
    }
    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 13); }, 500);
    
}
function taxAmtButnClick1(s, e) {
    // console.log(grid.GetFocusedRowIndex());

    var taxtype = cddl_AmountAre.GetValue();

    if (taxtype == '3') {
        grid.GetEditor('TaxAmount').SetEnabled(false);
    }
    else {
        grid.GetEditor("TaxAmount").SetEnabled(true);
    }
    rowEditCtrl = s;
}

function BatchUpdate() {


    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        cgridTax.UpdateEdit();
    }
    else {
        cgridTax.PerformCallback('SaveGST');
    }
    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 13); }, 500)
    return false;
}

var taxJson;
function cgridTax_EndCallBack(s, e) {



    var totalGst = 0;
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

        var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue());
        var totalRoundOffAmount = totalNetAmount;

        grid.GetEditor("TotalAmount").SetValue(totalRoundOffAmount);
        //  grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));



        //if (cddl_AmountAre.GetValue() == "2") {
        //   grid.GetEditor("Amount").SetValue(parseFloat(grid.GetEditor("Amount").GetValue()) + (totalRoundOffAmount - totalNetAmount));
        //    //var finalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
        //    //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
        //    //cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
        //}



        //totalGst = parseFloat(cgridTax.cpTotalGST);
        if (cddl_AmountAre.GetValue() == "2") {
            totalGst = parseFloat(grid.GetEditor("TaxAmount").GetValue());
            var qty = grid.GetEditor("Quantity").GetValue();
            var price = grid.GetEditor("SalePrice").GetValue();
            var Discount = grid.GetEditor("Discount").GetValue();

            var finalAmt = qty * price;


            //if (GSTType=="G")
            //    grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - totalGst), 2));
            //else if (GSTType == "N") {

            grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - (finalAmt * (Discount / 100)) - totalGst), 2));
            grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
        }
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
    //grid.batchEditApi.StartEdit(globalRowIndex, 16);
    //grid.GetEditor("TaxAmount").SetValue(ctxtTaxTotAmt.GetValue());
    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex,13); }, 500);
}

function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}








    //window.onload = function () {

    //    grid.AddNewRow();
    //};
    //$(document).ready(function () {
    //    page.SetActiveTabIndex(parseInt(document.getElementById("hdntab2").value));
    //})
    function SettingTabStatus() {
        if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
            page.GetTabByName('Billing/Shipping').SetEnabled(true);
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
    ctaxUpdatePanel.PerformCallback('DeleteAllTax');
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
        //   ;
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
        $('.crossBtn').show();
        page.GetTabByName('General').SetEnabled(true);
        gridLookup.Focus();
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

//Subhra-----23-01-2017-------
var Billing_state;
var Billing_city;
var Billing_pin;
var billing_area;

var Shipping_state;
var Shipping_city;
var Shipping_pin;
var Shipping_area;


function OnCountryChanged(cmbCountry) {

    CmbState.PerformCallback(cmbCountry.GetValue().toString());
}
function OnCountryChanged1(cmbCountry1) {
    CmbState1.PerformCallback(cmbCountry1.GetValue().toString());
}

function OnStateChanged(cmbState) {

    CmbCity.PerformCallback(cmbState.GetValue().toString());
    ctaxUpdatePanel.PerformCallback('DeleteAllTax');
}
function OnStateChanged1(cmbState1) {
    CmbCity1.PerformCallback(cmbState1.GetValue().toString());
    ctaxUpdatePanel.PerformCallback('DeleteAllTax');
}

function OnCityChanged(abc) {

    CmbPin.PerformCallback(abc.GetValue().toString());
    CmbArea.PerformCallback(abc.GetValue().toString());
}
function OnCityChanged1(abc) {
    CmbPin1.PerformCallback(abc.GetValue().toString());
    CmbArea1.PerformCallback(abc.GetValue().toString());


}
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
    if (Billing_state != 0) {
        s.SetValue(Billing_state);
    }
    CmbCity.PerformCallback(CmbState.GetValue());
    //Billing_state = 0;
}
function cmbshipstate_endcallback(s, e) {
    if (Shipping_state != 0) {
        s.SetValue(Shipping_state);
    }
    CmbCity1.PerformCallback(CmbState1.GetValue());
    Shipping_state = 0;
}

function cmbcity_endcallback(s, e) {
    if (Billing_city != 0) {
        s.SetValue(Billing_city);
    }
    CmbPin.PerformCallback(CmbCity.GetValue());
    CmbArea.PerformCallback(CmbCity.GetValue());
    Billing_city = 0;

}
function cmbshipcity_endcallback(s, e) {
    if (Shipping_city != 0) {
        s.SetValue(Shipping_city);
    }
    CmbPin1.PerformCallback(CmbCity1.GetValue());
    CmbArea1.PerformCallback(CmbCity1.GetValue());
    Shipping_city = 0;

}

function cmbPin_endcallback(s, e) {
    if (Billing_pin != 0) {
        s.SetValue(Billing_pin);
    }
    if (Billing_pin != null || Billing_pin != '' || Billing_pin != '0') {
        cchkBilling.SetEnabled(true);
    }
    Billing_pin = 0;
}
function cmbshipPin_endcallback(s, e) {
    if (Shipping_pin != 0) {
        s.SetValue(Shipping_pin);
    }
    if (Shipping_pin != null || Shipping_pin != '' || Shipping_pin != '0') {
        cchkShipping.SetEnabled(true);
    }
    Shipping_pin = 0;
}

function cmbArea_endcallback(s, e) {
    if (billing_area != 0) {
        s.SetValue(billing_area);
    }
    billing_area = 0;
}

function cmbshipArea_endcallback(s, e) {
    if (Shipping_area != 0) {
        s.SetValue(Shipping_area);
    }
    Shipping_area = 0;
}


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
            Billing_state = 0;
        }
        else {
            Billing_state = cComponentPanel.cpshow.split('~')[6];
        }
        var bcity = cComponentPanel.cpshow.split('~')[7];
        if (bcity == '') {
            CmbCity.SetSelectedIndex(-1);
            Billing_city = 0;
        }
        else {
            Billing_city = cComponentPanel.cpshow.split('~')[7];
        }

        var bpin = cComponentPanel.cpshow.split('~')[8];
        if (bpin == '') {
            CmbPin.SetSelectedIndex(-1);
            Billing_pin = 0;
        }
        else {
            Billing_pin = cComponentPanel.cpshow.split('~')[8];
        }

        var barea = cComponentPanel.cpshow.split('~')[9];
        if (barea == '') {
            CmbArea.SetSelectedIndex(-1);
            billing_area = 0;
        }
        else {
            billing_area = cComponentPanel.cpshow.split('~')[9];
        }

        billingStatus = cComponentPanel.cpshow.split('~')[10];
        var countryid = CmbCountry.GetValue()
        if (countryid != null && countryid != '' && countryid != '0') {
            CmbState.PerformCallback(countryid);
        }
    }

    if (cComponentPanel.cpshowShip != null) {


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

        shippingStatus = cComponentPanel.cpshowShip.split('~')[10];
        var countryid1 = CmbCountry1.GetValue()
        if (countryid1 != null && countryid1 != '' && countryid1 != '0') {
            CmbState1.PerformCallback(countryid1);
        }

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
            fn_PopOpen();
        }
    }
}












$(document).ready(function () {

    if (caspxTaxpopUp.Hide()) {
        grid.batchEditApi.StartEdit(globalRowIndex, 13);
    }

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
    ///   ;
    var saddresskey = shippingLookup.GetGridView().GetRowKey(shippingLookup.GetGridView().GetFocusedRowIndex());
    if (saddresskey != null && saddresskey != '') {

        cComponentPanel.PerformCallback('SlookupEdit~' + saddresskey);
    }
}








//function CloseGridLookup() {
//    gridLookup.ConfirmCurrentSelection();
//    gridLookup.HideDropDown();
//    gridLookup.Focus();
//}

function GetContactPersonPhone(e) {
    var key = cContactPerson.GetValue();
    cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
}






function GetContactPerson(e) {

    var startDate = new Date();
    startDate = tstartdate.GetDate().format('yyyy/MM/dd');
    var branchid = $('#ddl_Branch').val();
    //  var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
    var key = $('#hdnCustomerId').val();
    //  var key = ctxtCustName.GetValue();
    // alert(key);
    if (key != null && key != '') {

        ctxt_InvoiceDate.SetText('');

        //cContactPerson.PerformCallback('GetCustomerStateCode~' + key);
        GetCustomerStateCode(key);
        // cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
        //page.GetTabByName('Billing/Shipping').SetEnabled(true);
        //page.GetTabByName('General').SetEnabled(false);
        //$('.crossBtn').hide();                   
        //page.SetActiveTabIndex(1);
        $('.dxeErrorCellSys').addClass('abc');
        GetObjectID('hdnCustomerId').value = key;
        GetObjectID('hdnAddressDtl').value = '0';

        //###### Added By : Samrat Roy ##########
        LoadingPanel.Show();


        SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
        //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SRM');   //For Return Manual => SRM 
        LoadingPanel.Hide();
        GetObjectID('hdnCustomerId').value = key;
        page.tabs[0].SetEnabled(true);
        page.tabs[1].SetEnabled(true);
        //if ($('#hfBSAlertFlag').val() == "1") {
        //    jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
        //        if (r == true) {
        //            page.SetActiveTabIndex(1);
        //            cbsSave_BillingShipping.Focus();
        //            page.tabs[0].SetEnabled(false);
        //            $("#divcross").hide();
        //        }
        //    });
        //}
        //else {
        //    page.SetActiveTabIndex(1);
        //    cbsSave_BillingShipping.Focus();
        //    page.tabs[0].SetEnabled(false);
        //    $("#divcross").hide();
        //}
        //###### END : Samrat Roy : END ########## 







    }
}











function SetFocusonDemand(e) {
    var key = cddl_AmountAre.GetValue();
    if (key == '1' || key == '3') {
        //if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        //    clookup_Project.SetFocus();
        //}
        //else {
        //    if (grid.GetVisibleRowsOnPage() == 1) {
        //        grid.batchEditApi.StartEdit(-1, 2);
        //    }
        //}
    }
    else if (key == '2') {
       // cddlVatGstCst.Focus();
    }

}


function SetFocusAfterPlaceOfSupply(e)
{
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
            clookup_Project.SetFocus();
    }
     else {
            if (grid.GetVisibleRowsOnPage() == 1) {
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
    deleteTax('DeleteAllTax', "", "");
    if (IsProduct == "Y") {
        $('#hdfIsDelete').val('D');
        $('#HdUpdateMainGrid').val('True');
        grid.UpdateEdit();
        // cacbpCrpUdf.PerformCallback();
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
    // grid.PerformCallback('Display');
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
        var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
        if (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0.0")
        {
            grid.UpdateEdit();
            grid.PerformCallback('CurrencyChangeDisplay');
        }
              
    }
}

function ProductsCombo_SelectedIndexChanged(s, e) {
    $("#pageheaderContent").attr('style', 'display:block');
    //  cddl_AmountAre.SetEnabled(false);

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
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "");
    //cacpAvailableStock.PerformCallback(strProductID);
}

function CustomerStateEndcall(s, e) {
    if (cCustStateCallBackPanel.cpCustomerStateCode != null && cCustStateCallBackPanel.cpCustomerStateCode != 'undefined' && cCustStateCallBackPanel.cpCustomerStateCode != "") {

        GetObjectID('hdnCustomerStateCodeId').value = cCustStateCallBackPanel.cpCustomerStateCode;
        cCustStateCallBackPanel.cpCustomerStateCode = null;
    }
}

function cmbContactPersonEndCall(s, e) {
    LoadingPanel.Hide();

    if (cContactPerson.cpDueDate != null) {
        var DeuDate = cContactPerson.cpDueDate;
        var myDate = new Date(DeuDate);


        var key = GetObjectID('hdnCustomerId').value;
        // cCustStateCallBackPanel.PerformCallback('GetCustomerStateCode~' + key);
        GetCustomerStateCode(key);

        cdt_SaleInvoiceDue.SetDate(myDate);
        cContactPerson.cpDueDate = null;

    }


    //if (cContactPerson.cpCustomerStateCode != null && cContactPerson.cpCustomerStateCode != 'undefined' && cContactPerson.cpCustomerStateCode != "") {

    //    GetObjectID('hdnCustomerStateCodeId').value = cContactPerson.cpCustomerStateCode;




    //    // hdnCustomerStateCodeId.val(cContactPerson.cpCustomerStateCode);
    //    cContactPerson.cpCustomerStateCode = null;
    //}
    //else
    //{
    //    jAlert('Customer default Billing Shipping address not exist.');
    //    return false;

    //}


    if (cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) {

        $("#divGSTN").attr('style', 'display:block');
        document.getElementById('lblGSTIN').innerHTML = cContactPerson.cpGSTN;
        cContactPerson.cpGSTN = null;
    }

    if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {
        //alert(cContactPerson.cpOutstanding);

        $("#pageheaderContent").attr('style', 'display:block');
        // pageheaderOutContent.style.display = "block";

        $("#divOutstanding").attr('style', 'display:block');
        document.getElementById('<%=lblOutstanding.ClientID %>').innerHTML = cContactPerson.cpOutstanding;

        cContactPerson.cpOutstanding = null;
    }
    else {
        $("#pageheaderContent").attr('style', 'display:none');
        //pageheaderOutContent.style.display = "none";
        $("#divOutstanding").attr('style', 'display:none');
        document.getElementById('lblOutstanding').innerHTML = '';
    }


}


function Save_ButtonClick() {
    grid.AddNewRow();
    LoadingPanel.Show();
    //REV 2.0
    cbtn_SaveRecords.SetVisible(false);
    //REV 2.0 END
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

    if ((cddlPosGstReturnManual.GetValue() == "") || (cddlPosGstReturnManual.GetValue() == null)) {
        jAlert('Please enter valid place of supply.')
        flag = false;
    }
    //Rev 1.0
    var QuoteNo = $('#txt_PLQuoteNo').val();
    //Rev 1.0 End
    QuoteNo = QuoteNo.trim();
    if (QuoteNo == '' || QuoteNo == null) {
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }
    // Quote no validation End

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        // LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
    }
   

    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);
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


            if (issavePacking == 1) {
                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ReturnManual.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //divSubmitButton.style.display = "none";
                            // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //  var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            $('#hdfLookupCustomer').val(customerval);


                            // Custom Control Data Bind

                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('N');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                              cacbpCrpUdf.PerformCallback();
                            //kaushik
                        }
                    });
                }
                else {


                    //divSubmitButton.style.display = "none";
                    // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    //  var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    $('#hdfLookupCustomer').val(customerval);


                    // Custom Control Data Bind

                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('N');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                      cacbpCrpUdf.PerformCallback();
                    //kaushik
                }
            }
            else {

                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ReturnManual.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //divSubmitButton.style.display = "none";
                            // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //  var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            $('#hdfLookupCustomer').val(customerval);


                            // Custom Control Data Bind

                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('N');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                              cacbpCrpUdf.PerformCallback();
                            //kaushik
                        }
                    });
                }
                else {

                    //divSubmitButton.style.display = "none";
                    // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    //  var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";
                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    $('#hdfLookupCustomer').val(customerval);


                    // Custom Control Data Bind

                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('N');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
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
    

    grid.AddNewRow();
    //REV 2.0
    cbtn_SaveRecords_p.SetVisible(false);
    //REV 2.0 END
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

    if ((cddlPosGstReturnManual.GetValue() == "") || (cddlPosGstReturnManual.GetValue() == null)) {
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
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        //  LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
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
                        url: "ReturnManual.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            //divSubmitButton.style.display = "none";
                            //  var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";

                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            $('#hdfLookupCustomer').val(customerval);
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                              cacbpCrpUdf.PerformCallback();
                            //kaushik
                        }
                    });
                }
                else {


                    //divSubmitButton.style.display = "none";
                    //  var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";

                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    $('#hdfLookupCustomer').val(customerval);
                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                      cacbpCrpUdf.PerformCallback();
                    //kaushik
                }
            }
            else {

                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ReturnManual.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            //divSubmitButton.style.display = "none";
                            //  var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";

                            var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                            // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            $('#hdfLookupCustomer').val(customerval);
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                              cacbpCrpUdf.PerformCallback();
                            //kaushik
                        }
                    });
                }
                else {
                    //divSubmitButton.style.display = "none";
                    //  var customerval = (ctxtCustName.GetValue() != null) ? ctxtCustName.GetValue() : "";

                    var customerval = ($('#hdnCustomerId').val() != null) ? $('#hdnCustomerId').val() : "";
                    // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    $('#hdfLookupCustomer').val(customerval);
                    $('#hfControlData').val($('#hfControlSaveData').val());
                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                   // grid.UpdateEdit();
                      cacbpCrpUdf.PerformCallback();
                    //kaushik
                }
            }

        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            LoadingPanel.Hide();
            cbtn_SaveRecords_p.SetVisible(true);
        }
    }
    else
    {
        LoadingPanel.Hide();
        cbtn_SaveRecords_p.SetVisible(true);
    }
}


function SalePriceGotFocus() {
    ProductSaleprice = grid.GetEditor("SalePrice").GetValue();
    ProductAfterSalesPrice = ProductSaleprice;


    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());

}

function QuantityGotFocus(s, e) {

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductGetQuantity = QuantityValue;
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());


    //Surojit 14-03-2019
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
    if (SpliteDetails.length > 25) {
        if (SpliteDetails[25] == "1") {
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
        if (SpliteDetails.length == 25) {
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

    if (SpliteDetails.length == 27) {
        var isOverideConvertion = SpliteDetails[25];
        var packing_saleUOM = SpliteDetails[24];
        var sProduct_SaleUom = SpliteDetails[23];
        var sProduct_quantity = SpliteDetails[21];
        var packing_quantity = SpliteDetails[19];

        if (SpliteDetails[26] == "1") {
            IsInventory = 'Yes';
        }
        else {
            IsInventory = 'No';
        }

        if (SpliteDetails[18] != '') {
            if (parseFloat(SpliteDetails[18]) > 0) {
                gridPackingQty = SpliteDetails[18];
            }
        }
    }
    if ($("#hddnMultiUOMSelection").val() == "0") {
        if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 1) {
            ShowUOM(type, "Sale Return Manual", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
        }
    }
    //Surojit 14-03-2019

    //chinmoy added for  for MultiUOM start
    if ($("#hddnMultiUOMSelection").val() == "1") {
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
        // if ((gridquotationLookup.GetValue() != "") && (gridquotationLookup.GetValue() !=null)) {
        if (grid.GetEditor('Quantity').GetValue() != "0.0000") {
            grid.batchEditApi.StartEdit(globalRowIndex, 7);
            $("#UOMQuantity").val(grid.GetEditor('Quantity').GetValue());
        }
        // }
    }

    //End


}

var issavePacking = 0;

function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('Quantity').SetValue(Quantity);

    QuantityTextChange(globalRowIndex, 6);
  
    // setTimeout(function () {
    //     grid.batchEditApi.StartEdit(globalRowIndex, 6);
    //}, 300);
   

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
        url: "ReturnManual.aspx/GetQuantityfromSL",
        data: JSON.stringify({ SLNo: SLNo, val: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            Uomlength = msg.d;

        }
    });
}


function SetFoucs() {
    //debugger;
}

function UOMLostFocus(s,e)
{
    if($("#hddnMultiUOMSelection").val()=="1")
    {

    }
    else
    {
        grid.batchEditApi.StartEdit(globalRowIndex, 8);
    }
}
function CloseRateLostFocus(s,e)
{
    grid.batchEditApi.StartEdit(globalRowIndex, 10);
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

    $("#pageheaderContent").attr('style', 'display:block');
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var SalePrice12 = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
   // ProductAfterSalesPrice = SalePrice12;
    var ddlbranch = $("[id*=ddl_Branch]");
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var splitData = ProductID.split('||@||');

    // console.log(ProductID);

    if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(ProductAfterSalesPrice) != parseFloat(SalePrice12)) {
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
              
                if (BalanceQty == "NaN") {
                    BalanceQty = 0;
                }
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
                if (CurrQty < 0 && $("#Keyval_internalId").val() != "Add") {
                    grid.GetEditor("TotalQty").SetValue(TotalQty);
                    grid.GetEditor("Quantity").SetValue(TotalQty);
                    var OrdeMsg = 'Cannot enter quantity more than balance quantity.';
                    grid.batchEditApi.EndEdit();
                    jAlert(OrdeMsg, 'Alert Dialog: [Balance Quantity ]', function (r) {
                        grid.batchEditApi.StartEdit(globalRowIndex, 7);
                    });
                    return false;
                }                    //chinmoy added for multiuom start
                else if (CurrQty > 0) {
                    if (($("#hddnMultiUOMSelection").val() == "1")) {
                        UomLenthCalculation();

                        grid.batchEditApi.StartEdit(globalRowIndex);
                        var SLNo = grid.GetEditor('SrlNo').GetValue();
                        if (Uomlength > 0) {
                            var qnty = $("#UOMQuantity").val();
                            var QValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0.0000";
                            if (QValue != "0.0000" && QValue != qnty) {
                                jConfirm('Qunatity Change Will Clear Multiple UOM Details, Confirm?', 'Confirmation Dialog', function (r) {
                                    if (r == true) {
                                        grid.batchEditApi.StartEdit(globalRowIndex);
                                        var tbqty = grid.GetEditor('Quantity');

                                        var detailsid = grid.GetEditor('DetailsId').GetValue();
                                        if (detailsid != null && detailsid != "") {
                                            cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo + '~' + detailsid);
                                        }
                                        else {
                                            cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo + '~' + detailsid);
                                        }
                                        grid.GetEditor("TotalQty").SetValue(QuantityValue);
                                        grid.GetEditor("BalanceQty").SetValue(CurrQty);
                                    
                                                setTimeout(function () {
                                                    grid.batchEditApi.StartEdit(globalRowIndex, 6);
                                        }, 600)
                                        }
                                    else {
                                        grid.batchEditApi.StartEdit(globalRowIndex);
                                        grid.GetEditor('Quantity').SetValue(qnty);
                                        setTimeout(function () {
                                            grid.batchEditApi.StartEdit(globalRowIndex, 5);
                                        }, 200);
                                    }
                                });
                            }
                            else {
                                grid.batchEditApi.StartEdit(globalRowIndex);
                                grid.GetEditor('Quantity').SetValue(qnty);
                               
                                        setTimeout(function () {
                                            grid.batchEditApi.StartEdit(globalRowIndex, 6);
                                }, 600)
                                    
                                    }
                        }
                    }
                    if (($("#hddnMultiUOMSelection").val() == "0")) {
                        grid.GetEditor("TotalQty").SetValue(QuantityValue);
                        grid.GetEditor("BalanceQty").SetValue(CurrQty);
                    }
                } //chinmoy added for multiuom end
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
            // SetTotalTaxableAmount(globalRowIndex, 7);
            //var inx = globalRowIndex;
            //SetTotalTaxableAmount(inx, e);
            //SetInvoiceLebelValue();
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Quantity').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }

        if (QuantityValue != null && QuantityValue != "" && QuantityValue != "0.0000") {
            $.ajax({
                type: "POST",
                url: "ReturnManual.aspx/GetStockValuation",
                data: JSON.stringify({ ProductId: splitData[0] }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    //console.log('QuantityValue' + QuantityValue);
                    //console.log('Pro_Id' + splitData[0]);
                    //console.log('branch' + $('#ddl_Branch').val());
                    //console.log('date' + tstartdate.date.format('yyyy-MM-dd'));

                    var ObjData = msg.d;
                    if (ObjData.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "ReturnManual.aspx/GetStockValuationAmount",
                            data: JSON.stringify({ Pro_Id: splitData[0], Qty: QuantityValue, Valuationsign: ObjData, Fromdate: tstartdate.date.format('yyyy-MM-dd'), BranchId: $('#ddl_Branch').val() }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg1) {
                                var ObjData1 = msg1.d;
                                console.log("Hear" + ObjData1.length);
                                if (ObjData1.length > 0) {
                                    Amount = (ObjData1 * 1);
                                    Rate = Amount / QuantityValue;
                                    console.log("Rate" + Rate);
                                    grid.batchEditApi.StartEdit(globalRowIndex);

                                    var tbRate = grid.GetEditor("CloseRate");
                                    tbRate.SetValue(Rate);

                                    var tbRateflag = grid.GetEditor("CloseRateFlag");
                                    tbRateflag.SetValue(Rate);

                                }
                                else {
                                    console.log("one");
                                    var tbRateflag = grid.GetEditor("CloseRateFlag");
                                    tbRateflag.SetValue("0.0");
                                }
                            }
                        });
                    }
                }
            });
        }


    }

    if (ProductID != null && ProductID != "") {

    }
    deleteTax('DeleteAllTax', "", "");
    if (FocusValue == '1' && $('#hdnShowUOMConversionInEntry').val()=="1") {
        FocusValue = '0';
        setTimeout(function () { grid.batchEditApi.StartEdit(s, e); }, 600)
    }
    else if (FocusValue == '0' && $('#hdnShowUOMConversionInEntry').val() == "1") {
        FocusValue = '0';
        setTimeout(function () { grid.batchEditApi.StartEdit(s, e); }, 600)
    }
    else if (FocusValue == '1' && $('#hdnShowUOMConversionInEntry').val() == "0") {
        FocusValue = '0';
        setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 9); }, 600)
    }
    else
    {
        FocusValue = '0';
        setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex,6); }, 600)
    }
}
function spLostFocus(s, e) {
    FocusValue = '1';
    QuantityTextChange(globalRowIndex, 9);

}

function GridTaxamountLostFocus(s,e)
{
   
    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 13); }, 500)
}

/// Code Added By Sam on 23022017 after make editable of sale price field Start

function SalePriceTextChange(s, e) {
    // debugger;
    $("#pageheaderContent").attr('style', 'display:block');
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    //ProductAfterSalesPrice = Saleprice;
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
            SetTotalTaxableAmount(s, e);
            SetInvoiceLebelValue();
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('SalePrice').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}
//Rev Rajdip For Running Parameters
function Taxlostfocus(s, e) {
    debugger;
    //DiscountTextChange(s, e);
    //Rev Rajdip for Running Balance
    //SetTotalTaxableAmount(s, globalRowIndex);
    //SetInvoiceLebelValue();
}
function TotalAmountgotfocus(s, e) {
   
    //DiscountTextChange(s, e);
    //Rev Rajdip for Running Balance
    //SetTotalTaxableAmount(s, e);
    //SetInvoiceLebelValue();
   // SetTotalTaxableAmount(globalRowIndex, 13);
    //var invValue = parseFloat(cbnrLblTaxableAmtval.GetValue());
    //var invTaxamtval = parseFloat(cbnrLblTaxAmtval.GetValue());
    //var res = parseFloat(invValue) + parseFloat(invTaxamtval)
    ////alert(res);
    //cbnrLblInvValue.SetText(res.toFixed(2));
    //cbnrlblAmountWithTaxValue.SetText(res.toFixed(2));

}
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
                // Mantis Issue 24831
                //grid.batchEditApi.StartEdit(i, 2);
                grid.batchEditApi.StartEdit(i, 5);
                // End of Mantis Issue 24831
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
    cbnrlblAmountWithTaxValue.SetText(totamt.toFixed(2));
    cbnrLblInvValue.SetText(totamt.toFixed(2));
    // globalRowIndex = vindex;

    grid.batchEditApi.StartEdit(inx, vindex);

}
//Rev Rajdip
function SetInvoiceLebelValue() {
    var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
    cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));
}

//End Rev Rajdip

/// Code Above Added By Sam on 23022017 after make editable of sale price field End

function DiscountGotChange() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;
    ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductSaleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
}


var productgettotalamount = 0;
function DiscountTextChange(s, e) {
    //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    //alert(GetObjectID('hdnCustomerStateCodeId').value);
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

            //var ShippingStateCode = cbsSCmbState.GetValue();
            // var ShippingStateCode = $("#bsSCmbStateHF").val()
            var CompareStateCode;
            var CompareStateId;
            if (cddlPosGstReturnManual.GetValue() == "S") {
                // CompareStateCode = GeteShippingStateCode();
                CompareStateCode = $('#hdnPOSShippingStateCode').val();
                CompareStateId = $('#hdnPOSShippingStateId').val();
            }
            else {
                //CompareStateCode = GetBillingStateCode();
                CompareStateCode = $('#hdnPOSBillingStateCode').val();
                CompareStateId = $('#hdnPOSBillingStateId').val();
            }

                      

            var TaxType = "";
            if (cddl_AmountAre.GetValue() == "1") {
                TaxType = "E";
            }
            else if (cddl_AmountAre.GetValue() == "2") {
                TaxType = "I";
            }

            if (CompareStateCode != null && CompareStateCode != '' && CompareStateCode != '0') {
                caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[17], Amount, amountAfterDiscount, TaxType, CompareStateId, $('#ddl_Branch').val(), '');
            }
        }
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }

    SetTotalTaxableAmount(s,e);
    //Debjyoti 
    //grid.GetEditor('TaxAmount').SetValue(0);


    // if (parsefloat(Amount) != parsefloat(productgettotalamount)) {
    //if (parsefloat(_totalamount) != parsefloat(productgettotalamount)) {
    //grid.geteditor('taxamount').setvalue(0);

    //    ctaxupdatepanel.performcallback('delqtybysl~' + grid.geteditor("srlno").getvalue());
    // }

    // ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 11);
    }, 500);
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

    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var tbQuotation = grid.GetEditor("SrlNo");
    tbQuotation.SetValue(noofvisiblerows);
    grid.GetEditor('Product').SetEnabled(true);
    // Mantis Issue 24831
    $("#UOMQuantity").val(0);
    Uomlength = 0;
    // End of Mantis Issue 24831

    //if (gridquotationLookup.GetValue() == null) {
    //    grid.AddNewRow();

    //    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    //    var tbQuotation = grid.GetEditor("SrlNo");
    //    tbQuotation.SetValue(noofvisiblerows);
    //    grid.GetEditor('Product').SetEnabled(true);
    //}
    //else {

    //    QuotationNumberChanged();
    //    grid.AddNewRow();
    //    //kaushik 14-4-2017
    //    grid.StartEditRow(0);
    //    grid.GetEditor('Product').SetEnabled(false);
    //}
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
function OnCustomButtonClick(s, e) {
    debugger;
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
        //if (gridquotationLookup.GetValue() != null) {
        //    var messege = "";
        //    messege = "Cannot Delete using this button as the Sales Invoice is linked with this Sale Return.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
        //    jAlert(messege, 'Alert Dialog: [Delete Sales Invoice Products]', function (r) {
        //    });

        //}
        //else {
        if (noofvisiblerows != "1") {
            grid.DeleteRow(e.visibleIndex);


            cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
            cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));
            cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));
            cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));

            $('#hdfIsDelete').val('D');
            grid.UpdateEdit();
            // cacbpCrpUdf.PerformCallback();
            //kaushik
            grid.PerformCallback('Display');



            //  $('#<%=hdnPageStatus.ClientID %>').val('update');
            $('#hdnPageStatus').val('delete');
            //grid.batchEditApi.StartEdit(-1, 2);
            //grid.batchEditApi.StartEdit(0, 2);
        }
        //  }
    }


    else if (e.buttonID == "addlDesc") {
        debugger;
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 4);
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
        grid.batchEditApi.StartEdit(e.visibleIndex);
        // Mantis Issue 24831
        hdVisiableIndex.value = e.visibleIndex;
        // End of Mantis Issue 24831
        var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ProductID = Productdetails.split("||@||")[0];
        var UOMName = grid.GetEditor("UOM").GetValue();
        var quantity = grid.GetEditor("Quantity").GetValue();
        var DetailsId = grid.GetEditor('DetailsId').GetValue();
        var StockUOM = Productdetails.split("||@||")[5];

        if (StockUOM == "") {
            StockUOM = "0";
        }

        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        cAltUOMQuantity.SetValue("0.0000");
        // Mantis Issue 24831
        //if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
        if ((ProductID != "") && (UOMName != "") ) {
            // End of Mantis Issue 24831

            if (StockUOM == "0") {
                jAlert("Main Unit Not Defined.");
            }
            else {
                if ($("#hddnMultiUOMSelection").val() == "1") {
                    ccmbUOM.SetEnabled(false);
                    var index = e.visibleIndex;
                    grid.batchEditApi.StartEdit(e.visibleIndex);
                    //grid.batchEditApi.StartEdit(globalRowIndex);
                    var Qnty = grid.GetEditor("Quantity").GetValue();
                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var UomId = grid.GetEditor('ProductID').GetText().split("||@||")[3];
                    ccmbUOM.SetValue(UomId);
                    // Mantis Issue 24831
                    //$("#UOMQuantity").val(Qnty);
                    $("#UOMQuantity").val("0.0000");
                    ccmbBaseRate.SetValue(0);
                    cAltUOMQuantity.SetValue(0);
                    ccmbAltRate.SetValue(0);
                    ccmbSecondUOM.SetValue("");
                    // End of Mantis Issue 24831
                    cPopup_MultiUOM.Show();
                    cgrid_MultiUOM.cpDuplicateAltUOM = "";
                    // if ($("#hdnPageStatus").val() != "update") {
                    AutoPopulateMultiUOM();
                    //}
                    //chinmoy change start
                    cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo + '~' + DetailsId);
                    //cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + ProductID);
                }     //End
            }
        }
        else {
            return;
        }
    }


    else if (e.buttonID == 'AddNew') {
        //
        //if (gridquotationLookup.GetValue() == null) {



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
        //}
        //else {
        //    QuotationNumberChanged();
        //}
    }
    else if (e.buttonID == 'CustomWarehouse') {


        //alert(grid.GetEditor('ProductDisID').GetValue());
        //alert(grid.GetEditor('ProductID').GetValue());
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
            //alert(ProductID);
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
                    url: 'ReturnManual.aspx/getProductType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{Products_ID:\"" + strProductID + "\"}",
                    success: function (type) {
                        Ptype = type.d;
                        $('#hdfProductType').val(Ptype);
                        if (Ptype == "") {
                            jAlert("No Warehouse or Batch or Serial is actived !.");
                        } else {
                            //alert(Ptype);
                            if (Ptype == "W") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("true");
                                //cGrdWarehousePC.PerformCallback('BindWarehouse');

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
                                //cCmbWarehouse.PerformCallback('BindWarehouse');
                                //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                            }
                            else if (Ptype == "WS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("true");
                                //cCmbWarehouse.PerformCallback('BindWarehouse');
                                //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                            }
                            else if (Ptype == "WBS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("true");
                                //cCmbWarehouse.PerformCallback('BindWarehouse');
                                //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                            }
                            else if (Ptype == "BS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("false");

                                //cCmbBatch.PerformCallback('BindBatch~' + "0");
                                //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
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
                            // var branchid = ccmbbranch.GetValue();
                            var branchid = $("#ddl_Branch option:selected").val();

                            $('#hdnisreduing').val("false");

                            var productid = strProductID ? strProductID : "";
                            var StkQuantityValue = QuantityValue ? QuantityValue : "0.0000";

                            var stockids = SpliteDetails[10];
                            var checkstockdecimalornot = StkQuantityValue.toString().split(".")[1]

                            $('#hdnpcslno').val(SrlNo);
                            //var ProductName = (cOpeningGrid.GetEditor('ProductName').GetValue() != null) ? cOpeningGrid.GetEditor('ProductID').GetValue() : "";
                            var ProductName = SpliteDetails[12];
                            var ratevalue = "0";
                            var rate = "0";
                            // var branchid = (cOpeningGrid.GetEditor('branch').GetValue() != null) ? cOpeningGrid.GetEditor('branch').GetValue() : "0";
                            // var branchid = ccmbbranch.GetValue();
                            var branchid = $('#ddl_Branch').val();
                            //var BranchNames = (cOpeningGrid.GetEditor('branch').GetText() != null) ? cOpeningGrid.GetEditor('branch').GetText() : "0";
                            //var BranchNames = ccmbbranch.GetText();

                            var BranchNames = $("#ddl_Branch option:selected").text();
                            //alert(BranchNames);
                            // ProductName = ProductName.replace('dquote', '"');
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
                            // alert(iswarehousactive + "/" + isactivebatch + "/" + isactiveserial);

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
    //Rev Subhra 15-05-2019
    grid.batchEditApi.StartEdit(globalRowIndex, 9);
    //End of Rev Subhra 15-05-2019
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
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

var SelectWarehouse = "0";
var SelectBatch = "0";
var SelectSerial = "0";
var SelectedWarehouseID = "0";



function acpAvailableStockEndCall(s, e) {
    if (cacpAvailableStock.cpstock != null) {
        divAvailableStk.style.display = "block";
        //  divpopupAvailableStock.style.display = "block";

        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStk').innerHTML = AvlStk;
 
        cCmbWarehouse.cpstock = null;
    }
}


        function ctaxUpdatePanelEndCall(s, e) {

            //  alert('jjj');

            //console.log(ctaxUpdatePanel.cpstock);
            if (ctaxUpdatePanel.cpstock != null) {

                //kaushik 21-4-2017
                divAvailableStk.style.display = "block";
                //  divpopupAvailableStock.style.display = "block";
                //kaushik 21-4-2017
                var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
                document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
                document.getElementById('lblAvailableStk').innerHTML = ctaxUpdatePanel.cpstock;

                ctaxUpdatePanel.cpstock = null;

            }

            if (fromColumn == 'product') {
                grid.batchEditApi.StartEdit(globalRowIndex, 7);
                fromColumn = '';
            }
            return;
        }
function ctaxUpdatePanelEndCall(s, e) {
    if (ctaxUpdatePanel.cpstock != null) {
        divAvailableStk.style.display = "block";
        // divpopupAvailableStock.style.display = "block";

        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStk').innerHTML = AvlStk;
     
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
                //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                // ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
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
        // runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
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

    //ctxtQuoteTaxTotalAmt.SetValue(Math.round(totalTaxAmount));

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
    debugger;
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

    // Mantis Issue 24831
function CalcBaseQty() {
    //debugger;
   
    //var PackingQtyAlt = Productdetails.split("||@||")[18]; // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
    // var PackingQty = Productdetails.split("||@||")[20]; // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
    // var PackingSaleUOM = Productdetails.split("||@||")[25];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)


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
                }
            }
            else {
                $("#UOMQuantity").val("0.0000");
            }
        }
    });

  
}

function CalcBaseRate() {
    //debugger;
    var altQty = cAltUOMQuantity.GetValue();
    var altRate = ccmbAltRate.GetValue();
    var baseQty = $("#UOMQuantity").val();


    if (baseQty > 0) {
        var BaseRate = (altQty * altRate) / baseQty;
        ccmbBaseRate.SetValue(BaseRate);
    }
}
    // End of Mantis Issue 24831

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
    checkComboBox.SetText(selectedItems.length + " Items");

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
    //  grid.batchEditApi.StartEdit(globalRowIndex);
    //  grid.GetEditor("ProductID").SetText(LookUpData);
    //  grid.GetEditor("Product").Focus(ProductCode);


    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}
function ProductsGotFocusFromID(s, e) {

    //grid.batchEditApi.StartEdit(globalRowIndex);
    //grid.GetEditor("ProductID").SetText(LookUpData);
    //grid.GetEditor("ProductName").Focus(ProductCode);

    $("#pageheaderContent").attr('style', 'display:block');
    divAvailableStk.style.display = "block";

    var ProductdisID = (grid.GetEditor('ProductDisID').GetText() != null && grid.GetEditor('ProductDisID').GetText() != "") ? grid.GetEditor('ProductDisID').GetText() : "0";

    var ProductID = (grid.GetEditor('ProductID').GetText() != null && grid.GetEditor('ProductID').GetText() != "") ? grid.GetEditor('ProductID').GetText() : "0";
    var strProductName = (grid.GetEditor('ProductID').GetText() != null && grid.GetEditor('ProductID').GetText() != "") ? grid.GetEditor('ProductID').GetText() : "0";
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

    if (ProductID != "0") {

        //console.log('ProductID', ProductID);
        cacpAvailableStock.PerformCallback(strProductID);
    }
    else { cacpAvailableStock.PerformCallback(ProductdisID); }
}




   
    //document.onkeydown = function (e) {
    //    if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
    //        StopDefaultAction(e);


    //        btnSave_QuoteAddress();
    //        // document.getElementById('Button3').click();

    //        // return false;
    //    }

    //    if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
    //        StopDefaultAction(e);


    //        page.SetActiveTabIndex(0);
    //        gridLookup.Focus();
    //        // document.getElementById('Button3').click();

    //        // return false;
    //    }
    //}





        function ProductKeyDown(s, e) {
            // console.log(e.htmlEvent.key);
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
                //Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                //Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                //Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";

                setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

                $('#txtProdSearch').val('');
                $('#ProductModel').modal('show');
            }
            else {
                jAlert("Please Select a Customer", "Alert", function () { ctxtCustName.Focus(); });
            }
        }



        //if (e.buttonIndex == 0) {

        //    if (cproductLookUp.Clear()) {
        //        cProductpopUp.Show();
        //        cproductLookUp.Focus();
        //        cproductLookUp.ShowDropDown();
        //    }
        //}
    }



    function ProductDisKeyDown(s, e) {
        //console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Enter") {

            s.OnButtonClick(0);
        }
        //if (e.htmlEvent.key == "Tab") {

        //    s.OnButtonClick(0);
        //}
    }

    function ProductDisButnClick(s, e) {
        //if (e.buttonIndex == 0) {

        //    if (cproductDisLookUp.Clear()) {
        //        cProductpopUpdis.Show();
        //        cproductDisLookUp.Focus();
        //        cproductDisLookUp.ShowDropDown();
        //    }
        //}



        if (e.buttonIndex == 0) {
            var CID = GetObjectID('hdnCustomerId').value;
            if (CID != null && CID != "") {
                //Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                //Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                //Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";

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


        // var LookUpData = cproductDisLookUp.GetGridView().GetRowKey(cproductDisLookUp.GetGridView().GetFocusedRowIndex());
        var LookUpData = cproductDisLookUp.GetValue();

        if (LookUpData == null)
            return;
        //  var ProductCode = cproductDisLookUp.GetValue();
        var ProductCode = cproductDisLookUp.GetText();
        if (!ProductCode) {
            LookUpData = null;
        }
        cProductpopUpdis.Hide();
        //   grid.batchEditApi.StartEdit(globalRowIndex);
        grid.batchEditApi.StartEdit(globalRowIndex, 3);
        var productall = LookUpData.split('||')
        cddl_AmountAre.SetEnabled(false);
        var productdsc = productall[0];
        grid.GetEditor("ProductDisID").SetText(productdsc);
        grid.GetEditor("Product").SetText(ProductCode);

        // grid.batchEditApi.StartEdit(-1, 3);
        //grid.batchEditApi.EndEdit();
        //grid.batchEditApi.StartEdit(globalRowIndex, 3);
        //return;

        //  fromColumn = 'productdis';


        // if (fromColumn == 'productdis') {
        //grid.GetEditor("ProductName").Focus();
        //grid.batchEditApi.StartEdit(globalRowIndex, 2);
        // fromColumn = '';
        //return;
        //}
        //  grid.batchEditApi.StartEdit(globalRowIndex, 7);

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
        cddlPosGstReturnManual.SetEnabled(false);
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
        //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
        deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "");

        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }, 1000);


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


        //var quote_Id = gridquotationLookup.GetValue();

        if (!cproductLookUp.FindItemByValue(cproductLookUp.GetValue())) {
            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 7);
            jAlert("Product not Exists.", "Alert", function () { cproductLookUp.SetValue(); cproductLookUp.Focus(); });
            return;
        }

        var LookUpData = cproductLookUp.GetValue();
        var ProductCode = cproductLookUp.GetText();

        cProductpopUp.Hide();
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
        //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
        deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "");
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
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
   



        function SettingTabStatus() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            }
        }

        function disp_prompt(name) {
            if (name == "tab0") {
                //page.tabs[1].SetEnabled(false);
                $('#divcross').hide();
                // gridLookup.Focus();
                ctxtCustName.Focus();
            }
            if (name == "tab1") {
                $('#divcross').hide();
                page.tabs[0].SetEnabled(false);
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


        function blurOut() {
            debugger;
            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                clookup_Project.SetFocus();
            }
            else {
                if (grid.GetVisibleRowsOnPage() > 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }

                //if (grid.GetVisibleRowsOnPage() == 1) {

                //    grid.batchEditApi.StartEdit(-1, 2);
                //}


                //alert("uuu");
            }

        }
      

            function clookup_Project_GotFocus() {

                // clookup_Project.gridView.Refresh();
                clookup_Project.ShowDropDown();
            }


        //Hierarchy Start Tanmoy
        function clookup_Project_LostFocus() {
            var projID = clookup_Project.GetValue();
            if (projID == null || projID == "") {
                $("#ddlHierarchy").val(0);
            }
            if (grid.GetVisibleRowsOnPage() == 1) {
                grid.batchEditApi.StartEdit(-1, 2);
            }

        }

        function ProjectValueChange(s, e) {
            //debugger;
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'ReturnManual.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
        }
        //Hierarchy End Tanmoy
        function UploadGridbindCancel() {
            $("#exampleModalSRM").modal("hide");
            window.location.assign("ReturnManualList.aspx");

        }

        function UploadGridbind() {

            $("#exampleModalSRM").modal("hide");
            grid.PerformCallback('EInvoice~' + $("#hdnRDECId").val());

        }

        function IrnGrid() {

            $(".bcShad, .popupSuc").removeClass("in");
            var RateDiffVendID = $("#hdnRDECId").val();
           window.location.assign("ReturnManualList.aspx");

          }