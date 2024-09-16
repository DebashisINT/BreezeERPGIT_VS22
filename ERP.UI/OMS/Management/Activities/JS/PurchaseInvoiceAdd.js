
$(function () {
    $('#UOMModal').on('hide.bs.modal', function () {

        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    });
});
$(document).ready(function () {
    //var text = clookup_GRNOverhead.GetText();
    //$("#divOverheadCost").attr("title", text);
    $("body").tooltip({
        selector: '#divOverheadCost',
        template: '<div class="tooltip CUSTOM-CLASS" role="tooltip"><div class="arrow"></div><div class="tooltip-inner"></div></div>'
    });
});
function OverHeadcomponentEndCallBack() {
    var text = clookup_GRNOverhead.GetText();
    $("#divOverheadCost").attr("title", text);
    $("#divOverheadCost").attr("data-original-title", text);
    $("#divOverheadCost").tooltip({
        customClass: 'tooltip-custom',
        template: '<div class="tooltip CUSTOM-CLASS" role="tooltip"><div class="arrow"></div><div class="tooltip-inner"></div></div>'
    });
}

$(document).ready(function () {
    $('#CustModel').on('shown.bs.modal', function () {
        $('#txtCustSearch').focus();
    })
});
//<%--Batch Product Popup Start--%>
function closeMultiUOM(s, e) {
    e.cancel = false;
    // cPopup_MultiUOM.Hide();
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
        url: "PurchaseInvoice.aspx/GetPackingQuantity",
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
            // Mantis Issue 24429
            //cAltUOMQuantity.SetValue(calcQuantity);
            // End of Mantis Issue 24429
        }
    });
}

// Mantis Issue 24429
function Edit_MultiUom(keyValue, SrlNo) {
    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);
}
// End of Mantis Issue 24429

function AutoPopulateMultiUOM() {

    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    $.ajax({
        type: "POST",
        url: "PurchaseInvoice.aspx/AutoPopulateAltQuantity",
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
            if ($("#hdnPageStatus").val() == "Quoteupdate") {
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
                    // Mantis Issue 24429
                    ccmbSecondUOM.SetValue(AltUOMId);
                    // End of Mantis Issue 24429
                }
                // Mantis Issue 24429
                //cAltUOMQuantity.SetValue(calcQuantity);
                // End of Mantis Issue 24429
            }

        }
    });
}


function Delete_MultiUom(keyValue, SrlNo, DetailsId) {

    if (DetailsId == "0") {
        DetailsId = "";
    }
    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo + '~' + DetailsId);

}


function OnMultiUOMEndCallback(s, e) {
    if (cgrid_MultiUOM.cpDuplicateAltUOM == "DuplicateAltUOM") {
        jAlert("Please Enter Different Alt. Quantity.");
        return;
    }
    if (cgrid_MultiUOM.cpOpenFocus == "OpenFocus") {
        ccmbSecondUOM.SetFocus();
    }
    // Mantis Issue 24429
    if (cgrid_MultiUOM.cpSetBaseQtyRateInGrid != null && cgrid_MultiUOM.cpSetBaseQtyRateInGrid == "1") {
        grid.batchEditApi.StartEdit(globalRowIndex, 6);

        var BaseQty = cgrid_MultiUOM.cpBaseQty;
        var BaseRate = cgrid_MultiUOM.cpBaseRate;
        var AltQuantity = cgrid_MultiUOM.cpAltQuantity;
        var AltUOM = cgrid_MultiUOM.cpAltUOM;

        grid.GetEditor("Quantity").SetValue(BaseQty);
        grid.GetEditor("PurchasePrice").SetValue(BaseRate);
        grid.GetEditor("Amount").SetValue(BaseQty * BaseRate);
        grid.GetEditor("TotalAmount").SetValue(BaseQty * BaseRate);
        grid.GetEditor("InvoiceDetails_AltQuantity").SetValue(AltQuantity);
        grid.GetEditor("InvoiceDetails_AltUOM").SetValue(AltUOM);

        // Rev Mantis Issue 24429
        PurchasePriceTextChange(null, null);
        // End of Rev Mantis Issue 24429
    }

    if (cgrid_MultiUOM.cpAllDetails == "EditData") {
        //$('#UOMQuantity').val(cgrid_MultiUOM.cpBaseQty);
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
    // End of Mantis Issue 24429
}


var Uomlength = 0;
function UomLenthCalculation() {
    grid.batchEditApi.StartEdit(globalRowIndex);
    var SLNo = "";
    var val = 0;
    var detailsid = grid.GetEditor('DetailsId').GetValue();
    if (detailsid != null && detailsid != "" && detailsid != "0") {
        SLNo = detailsid;
        val = 1;
    }
    else {
        SLNo = grid.GetEditor('SrlNo').GetValue();
    }
    $.ajax({
        type: "POST",
        url: "PurchaseInvoice.aspx/GetQuantityfromSL",
        data: JSON.stringify({ SLNo: SLNo, val: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            Uomlength = msg.d;

        }
    });
}
function FinalMultiUOM() {
    UomLenthCalculation();
    if (Uomlength == 0 || Uomlength < 0) {

        // Mantis Issue 24429
        //jAlert("Please add Alt. Quantity.");
        jAlert("Please add atleast one Alt. Quantity with Update Row as checked.");
        // End of Mantis Issue 24429
        return;
    }
    else {
        cPopup_MultiUOM.Hide();
        // Mantis Issue 24429
        var SLNo = grid.GetEditor('SrlNo').GetValue();
        cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~' + SLNo);
        // End of Mantis Issue 24429
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 9);
        }, 400)
    }
}

// Mantis Issue 24429
function CalcBaseQty() {
    //var PackingQtyAlt = Productdetails.split("||@||")[19];
    //var PackingQty = Productdetails.split("||@||")[20];
    //var PackingSaleUOM = Productdetails.split("||@||")[22];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
    

    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var PackingQtyAlt = 0;
    var PackingQty = 0;
    var PackingSaleUOM = 0;

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
    var altQty = cAltUOMQuantity.GetValue();
    var altRate = ccmbAltRate.GetValue();
    var baseQty = $("#UOMQuantity").val();


    if (baseQty > 0) {
        var BaseRate = (altQty * altRate) / baseQty;
        ccmbBaseRate.SetValue(BaseRate);
    }
}
// End of Mantis Issue 24429

function SaveMultiUOM() {
    //debugger;
    //grid.GetEditor('ProductID').GetText().split("||@||")[3];

    var qnty = $("#UOMQuantity").val();


    var UomId = ccmbUOM.GetValue();
    //var UomId = ccmbUOM.SetSelectedIndex(grid.GetEditor('ProductID').GetText().split("||@||")[3] - 1);
    var UomName = ccmbUOM.GetText();
    //var AltQnty = parseFloat($("#AltUOMQuantity").val()).toFixed(4);
    var AltQnty = cAltUOMQuantity.GetValue();
    var AltUomId = ccmbSecondUOM.GetValue();
    var AltUomName = ccmbSecondUOM.GetText();

    // Rev Mantis Issue 24429
    grid.batchEditApi.StartEdit(globalRowIndex);
    // End of Rev Mantis Issue 24429
    var srlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var DetailsId = (grid.GetEditor('DetailsId').GetText() != null) ? grid.GetEditor('DetailsId').GetText() : "0";
    //var DetailsId = grid.GetEditor('DetailsId').GetText();
    // Mantis Issue 24429
    var BaseRate = ccmbBaseRate.GetValue();
    var AltRate = ccmbAltRate.GetValue();

    var UpdateRow = 'False';
    if ($("#chkUpdateRow").prop("checked")) {
        UpdateRow = 'True';
    }
    // End of Mantis Issue 24429

    // Mantis Issue 24429
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "") {
    // Rev Mantis Issue 24429
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != ""
    //    && BaseRate != "0.0000" && AltRate != "0.0000") {
    if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty != "0.0000") {
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
            // End of Rev Mantis Issue 24429
        // End of Mantis Issue 24429

        // Mantis Issue 24429
        //cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId);

        ////$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        //cAltUOMQuantity.SetValue("0.0000");

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
            // Rev Mantis Issue 24429
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
            // End of Rev Mantis Issue 24429

        }
        else {
            cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow);
            cAltUOMQuantity.SetValue("0.0000");
            $("#UOMQuantity").val(0);
            ccmbBaseRate.SetValue(0)
            cAltUOMQuantity.SetValue(0)
            ccmbAltRate.SetValue(0)
            ccmbSecondUOM.SetValue("")
            // Rev Mantis Issue 24429
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
            // End of Rev Mantis Issue 24429
        }
        // End of Mantis Issue 24429
        }
            // Rev Mantis Issue 24429

        else {
            return;
        }
        // End of Rev Mantis Issue 24429
    }
    else {
        return;
    }
}
//Added for lite popup

function SizeUOMChange() {

    $("#covergaeUOM").val($("#SizeUOM").val());
    var first = ctxtPackingQty.GetValue();
    var second = ctxtpacking.GetValue();

    if (ctxtpackingSaleUom.GetText() == "") {
        first = 0;
    }

    if (ccmbPackingUomPro.GetText() == "") {
        second = 0;
    }



    if ($("#SizeUOM").val() == "1") {
        var cov = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText() * parseFloat(first));
        var vol = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText()) * parseFloat(ctxtThickness.GetText() * parseFloat(first));
    }
    else {
        var cov = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText() * parseFloat(second));
        var vol = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText()) * parseFloat(ctxtThickness.GetText() * parseFloat(second));
    }

    if (cov == 0) {
        $("#txtCoverage").attr("disabled", false);
    }
    else {
        $("#txtCoverage").attr("disabled", true);
    }

    $("#txtCoverage").val(cov);
    $("#txtVolumn").val(vol);
    $("#dvCovg").text(cddlSize.GetText() + '²');
    $("#dvvolume").text(cddlSize.GetText() + '³');

}
//*************************************************  SI Main Account  *********************************************************************************

function MainAccountButnClick() {
    var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
    $("#txtMainAccountSearch").val("");
    document.getElementById("MainAccountTable").innerHTML = txt;
    cMainAccountModelSI.Show();
}

function MainAccountNewkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtMainAccountSearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtMainAccountSearch").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Main Account Name");
        callonServer("purchaseinvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountSIIndex", "SetMainAccountSI");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[MainAccountSIIndex=0]"))
            $("input[MainAccountSIIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        cMainAccountModelSI.Hide();
        cMainAccountModelSI.Focus();
    }
}

function SetMainAccountSI(Id, name, e) {
    $("#hdnSIMainAccount").val(Id);
    cSIMainAccount.SetText(name);
    cSIMainAccount_active.SetText(name);
    cMainAccountModelSI.Hide();
}

//************************************************* End SI Main Account  *********************************************************************************



//*************************************************  SR Main Account  *********************************************************************************

function SRMainAccountButnClick() {
    var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
    document.getElementById("MainAccountTableSR").innerHTML = txt;
    $("#txtMainAccountSRSearch").val("");
    cMainAccountModelSR.Show();
}
function MainAccountSRNewkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtMainAccountSRSearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtMainAccountSRSearch").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Main Account Name");
        callonServer("purchaseinvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTableSR", HeaderCaption, "MainAccountSRIndex", "SetMainAccountSR");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[MainAccountSRIndex=0]"))
            $("input[MainAccountSRIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        cMainAccountModelSR.Hide();
    }
}
function SetMainAccountSR(Id, name, e) {
    $("#hdnSRMainAccount").val(Id);
    cSRMainAccount.SetText(name);
    cSRMainAccount_active.SetText(name)
    cMainAccountModelSR.Hide();
}

//************************************************* End SR Main Account  *********************************************************************************

//*************************************************  PI Main Account  *********************************************************************************

function PIMainAccountButnClick() {
    var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
    document.getElementById("MainAccountTablePI").innerHTML = txt;
    $("#txtMainAccountPISearch").val("");
    cMainAccountModelPI.Show();
}

function MainAccountPINewkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtMainAccountPISearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtMainAccountPISearch").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Main Account Name");
        callonServer("purchaseinvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTablePI", HeaderCaption, "MainAccountPIIndex", "SetMainAccountPI");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[MainAccountPIIndex=0]"))
            $("input[MainAccountPIIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        cMainAccountModelPI.Hide();
    }
}
function SetMainAccountPI(Id, name, e) {
    $("#hdnPIMainAccount").val(Id);
    cPIMainAccount.SetText(name);
    cPIMainAccount_active.SetText(name);
    cMainAccountModelPI.Hide();
}

//************************************************* End PI Main Account  *********************************************************************************
//*************************************************  PR Main Account  *********************************************************************************

function PRMainAccountButnClick() {
    var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
    document.getElementById("MainAccountTablePR").innerHTML = txt;
    $("#txtMainAccountPRSearch").val("");
    cMainAccountModelPR.Show();
}

function MainAccountPRNewkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtMainAccountPRSearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtMainAccountPRSearch").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Main Account Name");
        callonServer("purchaseinvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTablePR", HeaderCaption, "MainAccountPRIndex", "SetMainAccountPR");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[MainAccountPRIndex=0]"))
            $("input[MainAccountPRIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        cMainAccountModelPR.Hide();
    }
}
function SetMainAccountPR(Id, name, e) {
    $("#hdnPRMainAccount").val(Id);
    cPRMainAccount.SetText(name);
    cPRMainAccount_active.SetText(name);
    cMainAccountModelPR.Hide();
}

//************************************************* End PR Main Account  *********************************************************************************

function MainAccountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumEnter") {
        s.OnButtonClick(0);
    }
}

function ValueSelected(e, indexName) {
    if (indexName == "MainAccountSIIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#hdnSIMainAccount").val(Code);
            cSIMainAccount.SetText(name);
            cSIMainAccount_active.SetText(name);
            cMainAccountModelSI.Hide();
        } else if (e.code == "ArrowDown") {
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
                $('#txtMainAccountSearch').focus();
            }
        }

    }

    else if (indexName == "MainAccountSRIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#hdnSRMainAccount").val(Code);
            cSRMainAccount.SetText(name);
            cSRMainAccount_active.SetText(name)
            cMainAccountModelSR.Hide();
        } else if (e.code == "ArrowDown") {
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
                $('#txtMainAccountSRSearch').focus();
            }
        }

    }
    else if (indexName == "MainAccountPIIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#hdnPIMainAccount").val(Code);
            cPIMainAccount.SetText(name);
            cPIMainAccount_active.SetText(name);
            cMainAccountModelPI.Hide();
        } else if (e.code == "ArrowDown") {
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
                $('#txtMainAccountPISearch').focus();
            }
        }

    }
    else if (indexName == "MainAccountPRIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#hdnPRMainAccount").val(Code);
            cPRMainAccount.SetText(name);
            cPRMainAccount_active.SetText(name);
            cMainAccountModelPR.Hide();
        } else if (e.code == "ArrowDown") {
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
                $('#txtMainAccountPRSearch').focus();
            }
        }

    }
}
//End lite Pop up

var PurReturnOldValue = '';
function cmbPurReturnGotFocus(s, e) {
    PurReturnOldValue = s.GetValue();
}

function mainAccountPurReturn(s, e) {
    for (var i = 0; i < mainAccountInUse.length; i++) {
        if (mainAccountInUse[i] == 'purchaseReturn') {

            jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
            s.SetValue(PurReturnOldValue);
        }
    }
}


var PurInvoiceOldValue = '';
function cmbPurInvoiceGotFocus(s, e) {
    PurInvoiceOldValue = s.GetValue();
}

function mainAccountPurInvoice(s, e) {

    for (var i = 0; i < mainAccountInUse.length; i++) {

        if (mainAccountInUse[i] == 'purchaseInvoice') {
            jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
            s.SetValue(PurInvoiceOldValue);
        }

    }

}

var salesReturnOldValue = '';
function cmbsalesReturnGotFocus(s, e) {
    salesReturnOldValue = s.GetValue();
}

function mainAccountSalesReturn(s, e) {
    for (var i = 0; i < mainAccountInUse.length; i++) {
        if (mainAccountInUse[i] == 'salesReturn') {
            jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
            s.SetValue(salesReturnOldValue);
        }
    }
}


var salesInvoiceOldValue = '';
function cmbsalesInvoiceGotFocus(s, e) {
    salesInvoiceOldValue = s.GetValue();
}

function mainAccountSalesInvoice(s, e) {
    for (var i = 0; i < mainAccountInUse.length; i++) {
        if (mainAccountInUse[i] == 'SalesInvoice') {

            jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
            s.SetValue(salesInvoiceOldValue);
        }
    }
}


$(function () {
    var vAnotherKeyWasPressed = false;
    var ALT_CODE = 18;

    //When some key is pressed
    $(window).keydown(function (event) {
        var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
        vAnotherKeyWasPressed = vKey != ALT_CODE;
        if (!LoadingPanel.IsVisible()) {
            if (event.altKey && (event.key == 's' || event.key == 'S')) {
                console.log('save not');
                if (cPopup_Empcitys.IsVisible()) {
                    if (cbtnSave_citys.IsVisible())
                        cbtnSave_citys.DoClick();
                }
                return false;
            }

            if (event.altKey && (event.key == 'a' || event.key == 'A')) {
                if (!cPopup_Empcitys.IsVisible()) {
                    if (document.getElementById('AddBtn') != null) {
                        console.log('new');
                        fn_PopOpen();
                        return false;
                    }

                }

            }

            if (event.altKey && (event.key == 'c' || event.key == 'C')) {
                console.log('save not');
                if (cPopup_Empcitys.IsVisible()) {
                    fn_btnCancel();
                }
                return false;
            }
        }
    });

    //When some key is left
    $(window).keyup(function (event) {

        var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

    });
});
function SetHSnPanelEndCallBack() {
    LoadingPanel.Hide();
    if (cSetHSnPanel.cpHsnCode) {
        if (cSetHSnPanel.cpHsnCode != "") {
            //cHsnLookUp.gridView.SelectItemsByKey(cSetHSnPanel.cpHsnCode);
            cSetHSnPanel.cpHsnCode = null;
        } else {
            cHsnLookUp.Clear();
        }
    }
}
function CmbProClassCodeChanged(s, e) {
    if (s.GetValue() != null) {
        cSetHSnPanel.PerformCallback(s.GetValue());
        LoadingPanel.SetText('Please wait searching HSN...');
        LoadingPanel.Show();
    }

}
function cCmbTradingLotUnitsLostFocus() {
    var saleUomVal = cCmbTradingLotUnits.GetValue();
    if (saleUomVal == null) {
        $('#btnPackingConfig').attr('disabled', 'disabled');
    }
    else {
        $('#btnPackingConfig').attr('disabled', false);
    }
}

function ShowTdsSection() {
    ctdsPopup.Show();
}

function ShowPackingDetails() {
    $('#invalidPackingUom').css({ 'display': 'none' });
    ctxtpackingSaleUom.SetText(cCmbTradingLotUnits.GetText());
    ctxtpackingSaleUom.SetEnabled(false);
    cpackingDetails.Show();
}

function PackingDetailsOkClick() {

    $('#invalidPackingUom').css({ 'display': 'none' });

    if (ccmbPackingUomPro.GetValue() == null) {
        $('#invalidPackingUom').css({ 'display': 'block' });
    } else {
        cpackingDetails.Hide();
    }
}

function ServicetaxOkClick() {
    cServiceTaxPopup.Hide();
}
function isInventoryChanged(s, e) {
    //changeControlStateWithInventory(s.GetValue());
    changeControlStateWithInventory();
}
function isCapitalChanged(s, e) {

    var Inv = ccmbIsInventory.GetValue();
    var cap = ccmbIsCapitalGoods.GetValue();
    if (Inv != 1) {
        if (cap == 1) {
            cCmbProType.SetEnabled(true);
            cCmbStockValuation.SetEnabled(true);
            //HsnChange
            //   caspxHsnCode.SetEnabled(true);
            // cHsnLookUp.SetEnabled(true);
            ctxtQuoteLot.SetEnabled(true);
            cCmbQuoteLotUnit.SetEnabled(true);
            ctxtTradingLot.SetEnabled(true);
            cCmbTradingLotUnits.SetEnabled(true);
            ctxtDeliveryLot.SetEnabled(true);
            cCmbDeliveryLotUnit.SetEnabled(true);
            ccmbStockUom.SetEnabled(true);
            ctxtMinLvl.SetEnabled(true);
            ctxtReorderLvl.SetEnabled(true);
            ccmbNegativeStk.SetEnabled(true);


            $('#btnBarCodeConfig').attr('disabled', false);
            $('#btnProdConfig').attr('disabled', false);

            $('#btnServiceTaxConfig').attr('disabled', 'disabled');
            cAspxServiceTax.SetValue('');

            $('#btnTDS').attr('disabled', 'disabled');
            cmb_tdstcs.SetValue('');
        }
        else {
            cCmbProType.SetText('');
            cCmbProType.SetEnabled(false);

            cCmbStockValuation.SetValue('A');
            cCmbStockValuation.SetEnabled(false);
            ctxtQuoteLot.SetText('0');
            ctxtQuoteLot.SetEnabled(false);
            cCmbQuoteLotUnit.SetText('0');
            cCmbQuoteLotUnit.SetEnabled(false);
            ctxtTradingLot.SetText('0');
            ctxtTradingLot.SetEnabled(false);
            cCmbTradingLotUnits.SetText('');
            cCmbTradingLotUnits.SetEnabled(false);
            ctxtDeliveryLot.SetText('0');
            ctxtDeliveryLot.SetEnabled(false);
            cCmbDeliveryLotUnit.SetText('');
            cCmbDeliveryLotUnit.SetEnabled(false);
            ccmbStockUom.SetText('');
            ccmbStockUom.SetEnabled(false);
            ctxtMinLvl.SetText('0');
            ctxtMinLvl.SetEnabled(false);
            ctxtReorderLvl.SetText('0');
            ctxtReorderLvl.SetEnabled(false);
            ccmbNegativeStk.SetValue('I');
            ccmbNegativeStk.SetEnabled(false);
            //Product Configuration
            $('#btnProdConfig').attr('disabled', 'disabled');
            $('#btnBarCodeConfig').attr('disabled', 'disabled');
            $('#btnServiceTaxConfig').attr('disabled', false);
            $('#btnTDS').attr('disabled', false);
            $('#btnPackingConfig').attr('disabled', 'disabled');
        }
    }
}
function changeControlStateWithInventory(obj) {
    obj = ccmbIsInventory.GetValue();
    if (obj == 1) {
        cCmbProType.SetEnabled(true);
        cCmbStockValuation.SetEnabled(true);
        //HsnChange
        //   caspxHsnCode.SetEnabled(true);
        // cHsnLookUp.SetEnabled(true);
        ctxtQuoteLot.SetEnabled(true);
        cCmbQuoteLotUnit.SetEnabled(true);
        ctxtTradingLot.SetEnabled(true);
        cCmbTradingLotUnits.SetEnabled(true);
        ctxtDeliveryLot.SetEnabled(true);
        cCmbDeliveryLotUnit.SetEnabled(true);
        ccmbStockUom.SetEnabled(true);
        ctxtMinLvl.SetEnabled(true);
        ctxtReorderLvl.SetEnabled(true);
        ccmbNegativeStk.SetEnabled(true);
        ccmbServiceItem.SetValue('0');
        ccmbServiceItem.SetEnabled(false);
        $('#btnBarCodeConfig').attr('disabled', false);
        $('#btnProdConfig').attr('disabled', false);

        $('#btnServiceTaxConfig').attr('disabled', 'disabled');
        cAspxServiceTax.SetValue('');

        $('#btnTDS').attr('disabled', 'disabled');
        cmb_tdstcs.SetValue('');

    } else {
        cCmbProType.SetText('');
        cCmbProType.SetEnabled(false);
        cCmbStockValuation.SetValue('A');
        cCmbStockValuation.SetEnabled(false);
        ctxtQuoteLot.SetText('1');
        ctxtQuoteLot.SetEnabled(false);
        cCmbQuoteLotUnit.SetText('');
        cCmbQuoteLotUnit.SetEnabled(false);
        ctxtTradingLot.SetText('1');
        ctxtTradingLot.SetEnabled(false);
        cCmbTradingLotUnits.SetText('');
        cCmbTradingLotUnits.SetEnabled(false);
        ctxtDeliveryLot.SetText('1');
        ctxtDeliveryLot.SetEnabled(false);
        cCmbDeliveryLotUnit.SetText('');
        cCmbDeliveryLotUnit.SetEnabled(false);
        ccmbStockUom.SetText('');
        ccmbStockUom.SetEnabled(false);
        ctxtMinLvl.SetText('0');
        ctxtMinLvl.SetEnabled(false);
        ctxtReorderLvl.SetText('0');
        ctxtReorderLvl.SetEnabled(false);
        ccmbServiceItem.SetEnabled(true);

        ccmbNegativeStk.SetValue('I');
        ccmbNegativeStk.SetEnabled(false);
        //Product Configuration
        $('#btnProdConfig').attr('disabled', 'disabled');
        $('#btnBarCodeConfig').attr('disabled', 'disabled');
        $('#btnServiceTaxConfig').attr('disabled', false);
        $('#btnTDS').attr('disabled', false);
        $('#btnPackingConfig').attr('disabled', 'disabled');
    }
}
//Code for UDF Control 
function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=Prd&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}
// End Udf Code
//Declare some global variable for poopUP
var barCodeType;
var BarCode, GlobalCode;
var taxCodeSale, taxCodePur, taxScheme;
var autoApply;
var ProdColor, ProdSize, ColApp, SizeApp;
var tdsValue = '';
//Declare some global variable for poopUP End Here

//Close the particular popup on esc
function OnInitTax(s, e) {
    ASPxClientUtils.AttachEventToElement(window.document, "keydown", function (evt) {
        if (evt.keyCode == ASPxClientUtils.StringToShortcutCode("ESCAPE"))
            cTaxCodePopup.Hide();
    });
}
function OnInitBarCode(s, e) {
    ASPxClientUtils.AttachEventToElement(window.document, "keydown", function (evt) {
        if (evt.keyCode == ASPxClientUtils.StringToShortcutCode("ESCAPE"))
            cBarCodePopUp.Hide();
    });
}
function OnInitProductAttribute(s, e) {
    ASPxClientUtils.AttachEventToElement(window.document, "keydown", function (evt) {
        if (evt.keyCode == ASPxClientUtils.StringToShortcutCode("ESCAPE"))
            cproductAttributePopUp.Hide();
    });
}
//for Image Upload
function OnUploadComplete(args) {
    console.log(args.callbackData);
    document.getElementById('fileName').value = args.callbackData;
    // cProdImage.SetImageUrl(args.callbackData);
    afterFileUpload();
}
function uploadClick() {
    Callback1.PerformCallback('');
}
function onFileUploadStart(s, e) {
    uploadInProgress = true;
    uploadErrorOccurred = false;
}
//Image upload end here
//code added by debjyoti 04-01-2017
function ShowProductAttribute() {

    cproductAttributePopUp.Show();
}
function productAttributeOkClik() {
    //Surojit 04-03-2019
    var ismandatory = cchkIsMandatory.GetValue();
    var textvalue = GridLookup_I.value;
    if (ismandatory && (textvalue == "" || textvalue == null)) {
        jAlert("Please select atleast one components!");
        return false;
    }
    else {
        ProdSize = cCmbProductSize.GetValue();
        ProdColor = cCmbProductColor.GetValue();
        ColApp = RrdblappColor.GetSelectedIndex();
        SizeApp = Rrdblapp.GetSelectedIndex();
        cproductAttributePopUp.Hide();
    }
    //Surojit 04-03-2019
}
function ShowBarCode() {
    cBarCodePopUp.Show();
}
function BarCodeOkClick() {
    barCodeType = cCmbBarCodeType.GetSelectedIndex();
    BarCode = ctxtBarCodeNo.GetText();
    GlobalCode = ctxtGlobalCode.GetText();
    cBarCodePopUp.Hide();
}
function ShowTaxCode() {
    cTaxCodePopup.Show();
}
function ShowServiceTax() {
    cServiceTaxPopup.Show();
}
function taxCodeOkClick() {
    taxCodeSale = cCmbTaxCodeSale.GetValue();
    taxCodePur = cCmbTaxCodePur.GetValue();
    autoApply = cChkAutoApply.GetChecked();
    taxScheme = cCmbTaxScheme.GetValue();
    cTaxCodePopup.Hide();
}
function GetCheckBoxValue(value) {
    //var value = s.GetChecked();
    if (value == true) {
        cCmbTaxCodePur.SetValue(0);
        cCmbTaxCodePur.SetEnabled(false);

        cCmbTaxCodeSale.SetValue(0);
        cCmbTaxCodeSale.SetEnabled(false);

        cCmbTaxScheme.SetEnabled(true);

    } else {
        cCmbTaxScheme.SetValue(0);
        cCmbTaxScheme.SetEnabled(false);
        cCmbTaxCodePur.SetEnabled(true);
        cCmbTaxCodeSale.SetEnabled(true);
    }
}
function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
}
//changes end here 04-01-2017
function fn_AllowonlyNumeric(s, e) {
    var theEvent = e.htmlEvent || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var keychar = String.fromCharCode(key);
    if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8 || key == 46) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
        return;
    }
    var regex = /[0-9\b]/;

    if (!regex.test(keychar)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault)
            theEvent.preventDefault();
    }
}
function fn_PopOpen() {
    //Surojit
    if ($('#hdnProductMasterComponentMandatoryVisible').val() == "0") {
        $('#divProductMasterComponentMandatory').hide();
    }
    else {
        $('#divProductMasterComponentMandatory').show();
    }
    //Surojit
    cbtnSave_citys.SetVisible(true);
    mainAccountInUse = [];
    $("#hdnSIMainAccount").val("");
    $("#hdnSRMainAccount").val("");
    $("#hdnPIMainAccount").val("");
    $("#hdnPRMainAccount").val("");
    cSIMainAccount.SetText("");
    cSRMainAccount.SetText("");
    cPIMainAccount.SetText("");
    cPRMainAccount.SetText("");
    cSIMainAccount.SetEnabled(true);
    cSRMainAccount.SetEnabled(true);
    cPIMainAccount.SetEnabled(true);
    cPRMainAccount.SetEnabled(true);
    document.getElementById('Keyval_internalId').value = 'Add';
    //document.getElementById('btnUdf').disabled =true;

    cPopup_Empcitys.SetHeaderText('Add Products');
    document.getElementById('hiddenedit').value = "";
    ctxtPro_Code.SetText('');
    ctxtPro_Name.SetText('');
    ctxtPro_Description.SetText('');
    cCmbProType.SetSelectedIndex(-1);
    cCmbProClassCode.SetSelectedIndex(-1);
    ctxtGlobalCode.SetText('');
    ctxtTradingLot.SetText('');
    cCmbTradingLotUnits.SetSelectedIndex(-1);
    cCmbQuoteCurrency.SetSelectedIndex(-1);
    ctxtQuoteLot.SetText('');
    cCmbQuoteLotUnit.SetSelectedIndex(-1);
    ctxtDeliveryLot.SetText('');
    cCmbDeliveryLotUnit.SetSelectedIndex(-1);
    cCmbProductColor.SetSelectedIndex(0);
    cCmbProductSize.SetSelectedIndex(0);
    RrdblappColor.SetSelectedIndex(0);
    Rrdblapp.SetSelectedIndex(0);
    gridLookup.Clear();
    //Debjyoti Code Added:30-12-2016
    //Reason: Barcode Type and No
    cCmbBarCodeType.SetSelectedIndex(-1);
    barCodeType = -1;
    BarCode = "";
    GlobalCode = "";
    ctxtBarCodeNo.SetText('');
    //End Debjyoti 30-12-2016
    taxCodeSale = 0;
    taxCodePur = 0;
    taxScheme = 0;
    autoApply = false;

    ProdColor = 0;
    ProdSize = 0;
    ColApp = 0;
    SizeApp = 0;
    //Debjyoti 04-01-2017
    ccmbIsInventory.SetSelectedIndex(0);
    cCmbStockValuation.SetSelectedIndex(1);
    ctxtSalePrice.SetText('');
    ctxtMinSalePrice.SetText('');
    ctxtPurPrice.SetText('');
    ctxtMrp.SetText('');
    ccmbStockUom.SetSelectedIndex(-1);
    ctxtMinLvl.SetText('');
    ctxtReorderLvl.SetText('');
    ctxtReorderQty.SetText('');
    ctxtMaxLvl.SetText('');
    ctxtHeight.SetText('0.00');
    ctxtWidth.SetText('0.00');
    ctxtThickness.SetText('0.00');
    cddlSize.SetSelectedIndex(0);
    $("#SizeUOM").val('1');
    ctxtSeries.SetText('');
    ctxtFinish.SetText('');
    ctxtLeadtime.SetText('0');
    $("#txtCoverage").val('0.00');
    $("#dvCovg").text('');
    $("#txtVolumn").val('0');
    $("#volumeuom").text('');
    ctxtWeight.SetText('0');
    ctxtSubCat.SetText('');
    ctxtPro_Printname.SetText('');

    ccmbNegativeStk.SetSelectedIndex(0);
    cCmbTaxCodeSale.SetSelectedIndex(0);
    cCmbTaxCodePur.SetSelectedIndex(0);
    cCmbTaxScheme.SetSelectedIndex(0);
    cChkAutoApply.SetChecked(false);
    GetCheckBoxValue(false);
    document.getElementById('fileName').value = '';
    cProdImage.SetImageUrl('');
    upload1.ClearText();
    //  gridLookup.SetValue(0);
    cCmbStatus.SetSelectedIndex(0);
    //caspxHsnCode.SetText('');
    cHsnLookUp.Clear();
    //Debjyoti 31-01-2017
    ctxtPro_Code.SetEnabled(true);
    ccmbIsInventory.SetEnabled(true);
    ccmbIsInventory.SetSelectedIndex(0);
    changeControlStateWithInventory();
    $('#reOrderError').css({ 'display': 'None' });
    $('#mrpError').css({ 'display': 'None' });
    cAspxServiceTax.SetValue('');
    $('#btnPackingConfig').attr('disabled', 'disabled');

    //packing details
    ctxtPackingQty.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default
    ctxtpacking.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default

    cchkOverideConvertion.SetChecked(false); //Surojit 08-02-2018
    cchkIsMandatory.SetChecked(false); //Surojit 11-02-2018

    //ccmbPackingUomPro.SetSelectedIndex(-1);
    //packing details End Here
    caspxInstallation.SetValue('0');
    ccmbBrand.SetValue('');
    cmb_tdstcs.SetValue('');
    cPopup_Empcitys.SetWidth(window.screen.width - 50);
    //cPopup_Empcitys.SetHeight(window.innerHeight.height - 70);
    cPopup_Empcitys.Show();
    cHsnLookUp.SetEnabled(true);
    ctxtPro_Code.Focus();
    //ccmbStatusad.SetSelectedIndex(0);
}
function afterFileUpload() {
    if (document.getElementById('hiddenedit').value == '') {
        cgridprod.PerformCallback('savecity~' + GetObjectID('fileName').value);
    }
    else {
        cgridprod.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value + "~" + GetObjectID('fileName').value);
    }
}
function SaveActiveDormant() {
    var retval = true;
    var minlvl = (ctxtMinLvl_active.GetValue() != null) ? ctxtMinLvl_active.GetValue() : "0";
    var reordLvl = (ctxtReorderLvl_active.GetValue() != null) ? ctxtReorderLvl_active.GetValue() : "0";

    if ((parseFloat(reordLvl) != 0) || (parseFloat(minlvl) != 0)) {
        if ((parseFloat(reordLvl) <= parseFloat(minlvl)))     //|| (((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0)))
        {
            $('#reOrderError1').css({ 'display': 'block' });
            retval = false;
        }
    }
    else if ((((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0))) {
        $('#reOrderError1').css({ 'display': 'None' });
    }
    else {
        $('#reOrderError1').css({ 'display': 'None' });
    }
    if (retval == false) {
        return false
    }

    else {
        if (document.getElementById('hiddenedit').value != '') {
            cgridprod.PerformCallback('updatecity_active~' + GetObjectID('hiddenedit').value);
        }
    }
}
function btnSave_citys() {
    var PackingUom = ccmbPackingUomPro.GetValue();
    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var ServiceItem = ccmbServiceItem.GetValueString();
    if ((((PackingUom == "0" || PackingUom == "" || PackingUom == null)) && (ShowUOMConversionInEntry == "1")) && ccmbIsInventory.GetValue() == 1) {
        jAlert(' "Show UOM Conversion In Entry" is Activated.You must Select alternate UOM from Product Master - Configure UOM Coinversion');
        return false;
    }
    if (ccmbIsInventory.GetValue() == 0) {
        if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '')) {
            if (upload1.GetText().trim() != '') {
                upload1.Upload();
            } else {
                afterFileUpload();
            }
        }
    } else {
        if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '') && (cCmbTradingLotUnits.GetText().trim() != '') && (cCmbDeliveryLotUnit.GetText().trim() != '') && (ccmbStockUom.GetValue() != null)) {

            if (validReorder() && validMRP()) {
                if (upload1.GetText().trim() != '') {
                    upload1.Upload();
                } else {
                    afterFileUpload();
                }
            }
        }
    }
}

function validReorder() {
    var retval = true;
    var minlvl = (ctxtMinLvl.GetValue() != null) ? ctxtMinLvl.GetValue() : "0";
    var reordLvl = (ctxtReorderLvl.GetValue() != null) ? ctxtReorderLvl.GetValue() : "0";

    if ((parseFloat(reordLvl) != 0) || (parseFloat(minlvl) != 0)) {
        if ((parseFloat(reordLvl) <= parseFloat(minlvl)))     //|| (((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0)))
        {
            $('#reOrderError').css({ 'display': 'block' });
            retval = false;
        }
    }
    else if ((((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0))) {
        $('#reOrderError').css({ 'display': 'None' });
    }
    else {
        $('#reOrderError').css({ 'display': 'None' });
    }
    return retval;
}

function validMRP() {
    var retval = true;
    var txtMinSalePrice = (ctxtMinSalePrice.GetValue() != null) ? ctxtMinSalePrice.GetValue() : "0";
    var txtMrp = (ctxtMrp.GetValue() != null) ? ctxtMrp.GetValue() : "0";

    if (parseFloat(txtMrp) != 0 && parseFloat(txtMrp) < parseFloat(txtMinSalePrice)) {
        $('#mrpError').css({ 'display': 'block' });
        retval = false;
    }
    else {
        $('#mrpError').css({ 'display': 'None' });
    }
    return retval;
}
function btnSave_citysOld() {
    var valiEmail = false;
    var validPhNo = false;
    var CheckUniqueCode = false;
    var reg = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
    if (reg.test(ctxtMarkets_Email.GetText())) {
        valiEmail = true;
    }

    if (!isNaN(ctxtMarkets_Phones.GetText()) && ctxtMarkets_Phones.GetText().length == 10) {
        validPhNo = true;
    }
    //for unique code ajax call
    var MarketsCode = ctxtMarkets_Code.GetText();
    $.ajax({
        type: "POST",
        url: "sMarkets.aspx/CheckUniqueCode",
        data: "{'MarketsCode':'" + MarketsCode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            //                    CheckUniqueCode = msg.d;

            if (document.getElementById('hiddenedit').value == '') {
                CheckUniqueCode = msg.d;
            }
            else {
                CheckUniqueCode == false
            }

            if (CheckUniqueCode == false && ctxtMarkets_Code.GetText() != '' && ctxtMarkets_Name.GetText() != '' && (ctxtMarkets_Email.GetText() == '' || valiEmail == true) && (ctxtMarkets_Phones.GetText() == '' || validPhNo == true)) {
                if (document.getElementById('hiddenedit').value == '') {
                    //alert("in add");
                    cgridprod.PerformCallback('savecity~');
                }
                else {
                    //alert("in update");
                    cgridprod.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value);
                }
            }
            else if (CheckUniqueCode == true) {
                jAlert('Please enter unique market code');
                ctxtMarkets_Code.Focus();
            }
            else if (ctxtMarkets_Code.GetText() == '') {
                jAlert('Please Enter Markets Code');
                ctxtMarkets_Code.Focus();
            }
            else if (ctxtMarkets_Name.GetText() == '') {
                jAlert('Please Enter Markets Name');
                ctxtMarkets_Name.Focus();
            }
            else if (!reg.test(ctxtMarkets_Email.GetText())) {
                jAlert('Please enter valid email');
                ctxtMarkets_Email.Focus();
            }
            else if (isNaN(ctxtMarkets_Phones.GetText()) || ctxtMarkets_Phones.GetText().length != 10) {
                jAlert('Please enter valid Phone No');
                ctxtMarkets_Phones.Focus();
            }

        }

    });
}
function fn_btnCancel() {
    cPopup_Empcitys.Hide();
    $("#txtPro_Code_EC, #txtPro_Name_EC, #txtQuoteLot_EC, #txtTradingLot_EC, #txtDeliveryLot_EC").hide();
}
function fn_btnCancel_active() {
    cPopup_Empcitys_active.Hide();
}
function fn_ViewProduct(keyValue) {
    /*-------------------------------------------------Arindam-----------------------------------------------------------*/
    var url = '/OMS/management/master/View/ViewProduct.html?v=0.07&&id=' + keyValue;
    CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
    CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
    CAspxDirectCustomerViewPopup.SetContentUrl(url);
    CAspxDirectCustomerViewPopup.RefreshContentUrl();
    CAspxDirectCustomerViewPopup.Show();
    /*-------------------------------------------------Arindam-----------------------------------------------------------*/
}

function fn_Editcity(keyValue) {
    cbtnSave_citys.SetVisible(true);
    document.getElementById('btnUdf').disabled = false;
    cPopup_Empcitys.SetHeaderText('Modify Products');

    ctxtHeight.SetText('0.00');
    ctxtWidth.SetText('0.00');
    ctxtThickness.SetText('0.00');
    cddlSize.SetSelectedIndex(0);
    $("#SizeUOM").val('1');
    ctxtSeries.SetText('');
    ctxtFinish.SetText('');
    ctxtLeadtime.SetText('0');
    $("#txtCoverage").val('');
    $("#dvCovg").text('');
    $("#txtVolumn").val('0');
    $("#volumeuom").text('');
    ctxtWeight.SetText('0');
    ctxtSubCat.SetText('');
    ctxtPro_Printname.SetText('');


    cgridprod.PerformCallback('Edit~' + keyValue);
    document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
}
function fn_activeEdit(keyValue, status) {
    document.getElementById('HiddenField_status').value = '1';
    // cPopup_Empcitys_active.SetWidth(window.screen.width - 50);
    cbtnSave_citys.SetVisible(true);
    document.getElementById('btnUdf').disabled = false;
    cPopup_Empcitys_active.SetHeaderText('Modify Products');
    cgridprod.PerformCallback('Active~' + keyValue);
    document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
}
function fn_Deletecity(keyValue) {
    //if (confirm("Confirm Delete?")) {
    //    cgridprod.PerformCallback('Delete~' + keyValue);
    //}
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            cgridprod.PerformCallback('Delete~' + keyValue);
        }
    });
}
function componentEndCallBack(s, e) {
    console.log(e);
    // cPopup_Empcitys.Show();
}
var mainAccountInUse = [];
function grid_EndCallBack() {
    if (cgridprod.cpinsert != null) {
        if (cgridprod.cpinsert == 'Success') {
            jAlert('Saved Successfully');
            //alert('Saved Successfully');
            //................CODE  UPDATED BY sAM ON 18102016.................................................
            ctxtPro_Name.GetInputElement().readOnly = false;
            //................CODE ABOVE UPDATED BY sAM ON 18102016.................................................
            cPopup_Empcitys.Hide();

        }
        else if (cgridprod.cpinsert == 'fail') {
            jAlert("Error On Insertion \n 'Please Try Again!!'")
        }
        else if (cgridprod.cpinsert == 'UDFManddratory') {
            jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });

        }
        else {
            jAlert(cgridprod.cpinsert);
            //cPopup_Empcitys.Hide();
        }
    }
}
function OnCmbCountryName_ValueChange() {
    cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
}
function CmbState_EndCallback() {
    cCmbState.SetSelectedIndex(0);
    cCmbState.Focus();
}
function OnCmbStateName_ValueChange() {
    cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());
}
function CmbCity_EndCallback() {
    cCmbCity.SetSelectedIndex(0);
    cCmbCity.Focus();
}
$(document).ready(function () {
    $('.dxpc-closeBtn').click(function () {
        fn_btnCancel();
    });


});
function fn_ctxtPro_Name_TextChanged(s, e) {
    var procode = 0;
    if (GetObjectID('hiddenedit').value != '') {
        procode = GetObjectID('hiddenedit').value;
    }
    //var ProductName = ctxtPro_Name.GetText();
    var ProductName = ctxtPro_Code.GetText().trim();
    $.ajax({
        type: "POST",
        url: "purchaseinvoice.aspx/CheckUniqueNameProduct",
        //data: "{'ProductName':'" + ProductName + "'}",
        data: JSON.stringify({ ProductName: ProductName, procode: procode }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            if (data == true) {
                jAlert("Please enter unique name", "Alert", function () { ctxtPro_Code.SetFocus(); });
                ctxtPro_Code.SetText("");
                return false;
            }
        }

    });
}
function OnAddBusinessClick(keyValue) {
    var url = '../../master/AssignIndustry.aspx?id1=' + keyValue + '&EntType=product';
    window.location.href = url;
}
function PopupOpen(obj) {
    var URL = '/OMS/Management/Master/Product_Document.aspx?idbldng=' + obj + '&type=Product';
    //OnMoreInfoClick(URL, "Products Document Details", '1000px', '400px', "Y");
    //document.getElementById("marketscgridprod_DXPEForm_efnew_DXEditor1_I").focus();
    window.location.href = URL;
}
var KEYCODE_ENTER = 13;
var KEYCODE_ESC = 27;
$(document).keyup(function (e) {
    if (e.keyCode == KEYCODE_ESC) {
        // cPopup_Empcitys.Hide();
        //$('#cPopup_Empcitys').hide();
    }
});
function PopupOpentoProductUpload(obj, prod_name) {
    ///  alert(prod_name);
    var URL = '/OMS/Management/Master/Product-Multipleimage.aspx?prodid=' + obj + '&name=' + prod_name;
    window.location.href = URL;
}
var counter = 0;
function fetchLebel() {
    $("#generatedForm").html("");
    counter = 0;
    $(".newLbl").each(function () {

        var newField = "<div style='width:500px; margin-left:5px; float:left; margin-bottom:5px;'><label id='LblKey" + counter + "' style='width:110px; float:left;'>" + $(this).text() + "</label>";
        newField += "<input type='text' id='TxtKey" + counter + "' value='" + $(this).text() + "' style='margin-left:41px; width:250px;' />";
        //alert($(this).attr("id").split('_')[4]);
        if (String($(this).attr("id").split('_')[2]) != "undefined") {
            newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("id").split('_')[2] + "' style='display:none; margin-left:41px; width:250px;' />";
        }
        else {
            //alert($(this).attr("id"));
            newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("id") + "' style='display:none; margin-left:41px; width:250px;' />";
        }
        newField += "</div>";
        $("#generatedForm").append(newField);
        counter++;
    });
    AssignValuePopup.Show();
}
function tdsOkClick() {
    tdsValue = cmb_tdstcs.GetValue();
    ctdsPopup.Hide();
}
function SaveDataToResource() {
    var key = "";
    var value = "";
    for (var i = 0; i < counter; i++) {
        if (key == "") {
            key = $("#HddnKey" + i).val();
            value = $("#TxtKey" + i).val();
        }
        else {
            key += "," + $("#HddnKey" + i).val();
            value += "," + $("#TxtKey" + i).val();
        }
    }
    $("#AssignValuePopup_KeyField").val(key);
    $("#AssignValuePopup_ValueField").val(value);
    $("#AssignValuePopup_RexPageName").val("ProductValues");
    return true;
}
var NewExit = '';
//Chinmoy Added Below Code
//Start
var Address = [];
var ReturnDetails;
function GetPurchaseAddress(OrderId, TagDocType) {
    var OtherDetail = {};

    OtherDetail.OrderId = OrderId;
    OtherDetail.TagDocType = TagDocType;


    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "PurchaseInvoice.aspx/SavePurchaseTaggedAddress",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                Address = msg.d;
                PopulateBillShippAddress(Address);

            }
        });
    }
}
function ParentCustomerOnClose(newCustId, customerName, Unique) {
    GetObjectID('hdnCustomerId').value = newCustId;

    AspxDirectAddCustPopup.Hide();
    ctxtShipToPartyShippingAdd.SetText('');
    if (newCustId != "") {
        ctxtVendorName.SetText(customerName);
        SetCustomer(newCustId, customerName);
    }
}

function AddVendorClick() {
    //if (isLighterPage == 1) {
    var url = '/OMS/management/Master/vendorPopup.html?var=1.1.4.8';
    AspxDirectAddCustPopup.SetContentUrl(url);
    //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();

    AspxDirectAddCustPopup.RefreshContentUrl();
     AspxDirectAddCustPopup.Show();
}

function PopulateBillShippAddress(ReturnDetails) {
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


        var GSTIN = BillingDetails[0].GSTIN;
        var GSTIN1 = GSTIN.substring(0, 2);
        var GSTIN2 = GSTIN.substring(2, 12);
        var GSTIN3 = GSTIN.substring(12, 15);

        ctxtBillingGSTIN1.SetText(GSTIN1);
        ctxtBillingGSTIN2.SetText(GSTIN2);
        ctxtBillingGSTIN3.SetText(GSTIN3);
        //cddlPosGstInvoice.SetValue(BillingDetails[0].PosForGst);
        PosGstId = BillingDetails[0].PosForGst;
        // cddlPosGstInvoice.SetValue(posGSTId);

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

        ctxtBillingGSTIN1.SetText('');
        ctxtBillingGSTIN2.SetText('');
        ctxtBillingGSTIN3.SetText('');
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


        var GSTIN = ShippingDetails[0].GSTIN;
        var GSTIN1 = GSTIN.substring(0, 2);
        var GSTIN2 = GSTIN.substring(2, 12);
        var GSTIN3 = GSTIN.substring(12, 15);


        ctxtShippingGSTIN1.SetText(GSTIN1);
        ctxtShippingGSTIN2.SetText(GSTIN2);
        ctxtShippingGSTIN3.SetText(GSTIN3);
        $('#hdShipToParty').val(ShippingDetails[0].ShipToPartyId);
        ctxtShipToPartyShippingAdd.SetText(ShippingDetails[0].ShipToPartyName);
        //cddlPosGstInvoice.SetValue(ShippingDetails[0].PosForGst);
        PosGstId = ShippingDetails[0].PosForGst;
        // cddlPosGstInvoice.SetValue(posGSTId);
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

        ctxtShippingGSTIN1.SetText('');
        ctxtShippingGSTIN2.SetText('');
        ctxtShippingGSTIN3.SetText('');
        $('#hdShipToParty').val('');
        ctxtShipToPartyShippingAdd.SetText('');

    }

    GetPurchaseForGstValue();
}
function AfterSaveBillingShipiing(validate) {
    GetPurchaseForGstValue();
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
    cddlPosGstInvoice.ClearItems();
    if (cddlPosGstInvoice.GetItemCount() == 0) {
        cddlPosGstInvoice.AddItem(GetShippingStateName() + '[Shipping]', "S");
        cddlPosGstInvoice.AddItem(GetBillingStateName() + '[Billing]', "B");
    }

    else if (cddlPosGstInvoice.GetItemCount() > 2) {
        cddlPosGstInvoice.ClearItems();
        //cddl_PosGstSalesOrder.RemoveItem(0);
        //cddl_PosGstSalesOrder.RemoveItem(0);
    }
    if (PosGstId == "" || PosGstId == null) {
        cddlPosGstInvoice.SetValue("S");
    }
    else {
        cddlPosGstInvoice.SetValue(PosGstId);
    }
}
var PosGstId = "";
function PopulateInvoicePosGst(e) {

    PosGstId = cddlPosGstInvoice.GetValue();
    if (PosGstId == "S") {
        cddlPosGstInvoice.SetValue("S");
    }
    else if (PosGstId == "B") {
        cddlPosGstInvoice.SetValue("B");
    }
}
function TDSEditableCheckChanged() {
    var checkval = cchk_TDSEditable.GetChecked();
    if (checkval) {
        cgridinventory.SetEnabled(true);
    }
    else {
        cgridinventory.SetEnabled(false);
    }
}
// TDS Section Modification Section Start on 05022017 by Sam
function TDSAmtLostFocus() {

    var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
    var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
    var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
    var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
    var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
    if (!chkNILRateTDS.GetChecked()) {
        ctxt_totalnoninventoryproductamt.SetText(TotaNonInvSecAmt);
    }
    else {
        ctxt_totalnoninventoryproductamt.SetText("0.00");
    }
}
function SurchargeAmountLostFocus() {
    var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
    var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
    var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
    var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
    var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
    ctxt_totalnoninventoryproductamt.SetText(TotaNonInvSecAmt);
}

function EducationCessAmtLostFocus() {
    var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
    var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
    var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
    var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
    var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
    ctxt_totalnoninventoryproductamt.SetText(TotaNonInvSecAmt);
}

function HgrEducationCessAmtLostFocus() {
    var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
    var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
    var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
    var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
    var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
    ctxt_totalnoninventoryproductamt.SetText(TotaNonInvSecAmt);
}

var taxtype;
var isenabled = 0;
function RCMCheckChanged() {
    var checkval = cchk_reversemechenism.GetChecked();
    if (checkval) {
        taxtype = cddl_AmountAre.GetValue();
        cddl_AmountAre.SetValue(3);
        PopulateGSTCSTVAT();
        cddl_AmountAre.SetEnabled(false);
    }
    else {
        cddl_AmountAre.SetValue(taxtype);
        if (taxtype == '3') {
            cddl_AmountAre.SetEnabled(false);
        }
        else {
            cddl_AmountAre.SetEnabled(true);
        }
        //cddl_AmountAre.SetValue(1);
        PopulateGSTCSTVAT();

    }
}
// TDS Section Modification Section End on 05022017 by Sam
function SetProduct(Id, Name) {
    $('#ProductModel').modal('hide');

    var LookUpData = Id;
    var ProductCode = Name;

    if (!ProductCode) {
        LookUpData = null;
    }
    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);
    cddl_TdsScheme.SetEnabled(false);
    ctxtVendorName.SetEnabled(false);
    cddlPosGstInvoice.SetEnabled(false);
    $('#ddl_numberingScheme').attr("disabled", true);
    cchk_reversemechenism.SetEnabled(false);
    $('#ddlInventory').prop('disabled', true);
    $('#ddl_Branch').prop('disabled', true);
    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("PurchasePrice");
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
    } else {
        divPacking.style.display = "none";
    }
    // Running total Calculation Start
    Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    CalculateAmount();
    // Running total Calculation End

    //Debjyoti
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), strProductID, "");
    grid.batchEditApi.StartEdit(globalRowIndex, 6);
}

function deleteTax(Action, srl, productid, hdBasketId) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;
    OtherDetail.prodid = productid;


    $.ajax({
        type: "POST",
        url: "PurchaseInvoice.aspx/taxUpdatePanel_Callback",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var Code = msg.d;
            if (Code != null) {
            }
            if (productid != "") {
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                }, 600)
            }

        }
    });
}
function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {

        s.OnButtonClick(0);
    }

}
function Purchaseprodkeydown(e) {
    var OtherDetails = {};
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    var invtype = $('#ddlInventory').val();
    var TDSid = cddl_TdsScheme.GetValue();
    OtherDetails.InventoryType = invtype;
    OtherDetails.TDSCode = TDSid;

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("GST Rate");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        // HeaderCaption.push("Installation Reqd.");

        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetNormalPurchaseInvoiceProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }

}

function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {
        //var customerval = gridLookup.GetValue();
        var customerval = GetObjectID('hdnCustomerId').value;
        if ($('#txtVoucherNo').val() == '' || $('#txtVoucherNo').val() == null) {
            jAlert('Select a numbering schema first.');
            $('#ddl_numberingScheme').focus();
            return false;
        }
        else if (customerval == '' || customerval == null || customerval == "") {
            jAlert('Select a Vendor first');
            //gridLookup.Focus();
            ctxtVendorName.Focus();
            return false;
        }
        else if (cproductLookUp.Clear()) {
            // Running total Calculation Start 
            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            // Running total Calculation End

            //New Modification Section on 05012018
            $('#txtProdSearch').val('');
            $('#ProductModel').modal('show');
            setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
        }
    }
}
function SetHsnSac(newHsnSac) {
    newHsnSac = newHsnSac.trim();
    if (newHsnSac != "") {
        var existsHsnSac = $('#hdHsnList').val();
        if (existsHsnSac.indexOf(',' + newHsnSac + ',') == -1) {
            existsHsnSac = existsHsnSac + newHsnSac + ',';
            $('#hdHsnList').val(existsHsnSac);
        }
    }
}
function RemoveHSnSacFromList(newHsnSac) {
    newHsnSac = newHsnSac.trim();
    if (newHsnSac != "") {
        var existsHsnSac = $('#hdHsnList').val();

        existsHsnSac = existsHsnSac.replace(newHsnSac + ',', '');
        $('#hdHsnList').val(existsHsnSac);

    }
}
function ProductSelected(s, e) {
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
    //Delete hsn
    if (grid.GetEditor("ProductID").GetText() != "") {
        var previousProductId = grid.GetEditor("ProductID").GetText();
        RemoveHSnSacFromList(previousProductId.split("||@||")[19]);
    }
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    pageheaderContent.style.display = "block";
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
    $('#HDSelectedProduct').val(strProductID);
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    SetHsnSac(SpliteDetails[19]);
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
    var totalNetAmount = grid.GetEditor("TotalAmount").GetValue();

    var newTotalNetAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(totalNetAmount);
    cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(newTotalNetAmount) * 100) / 100).toFixed(2));
    SetInvoiceLebelValue();
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


    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    cbnrOtherChargesvalue.SetText('0.00');
    SetRunningBalance();
    grid.batchEditApi.StartEdit(globalRowIndex, 5);
}
function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }
}
//<%--Batch Product Popup End--%>
// Vendor Search Section Start on 03012018
function VendorButnClick(s, e) {

    document.getElementById("txtCustSearch").value = "";
    var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\"><th>Vendor Name</th><th>Unique Id</th></tr><table>";
    document.getElementById("CustomerTable").innerHTML = txt;

    $('#CustModel').modal('show');
    $('#txtCustSearch').focus();
}
function VendorKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        document.getElementById("txtCustSearch").value = "";
        var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\"><th>Vendor Name</th><th>Unique Id</th></tr><table>";
        document.getElementById("CustomerTable").innerHTML = txt;

        $('#CustModel').modal('show');
        $('#txtCustSearch').focus();
    }
}
function Customerkeydown(e) {
    var OtherDetails = {};
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    OtherDetails.BranchID = $('#ddl_Branch').val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Vendor Name");
        HeaderCaption.push("Unique Id");
        if (OtherDetails.SearchKey != '') {
            callonServer("Services/Master.asmx/GetVendorWithBranch", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}

function SetCustomer(Id, Name) {
    
    var VendorID = Id;
    if (Id != "") {

        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        if (noofvisiblerows == '0') {
            grid.AddNewRow();
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue('1');
        }
        var invtype = $('#ddlInventory').val();
        var startDate = new Date();
        startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
        var branchid = $('#ddl_Branch').val();
        GetPurchaseForGstValue();
        var key = Id;
        GetObjectID('hdnCustomerId').value = key;
        SetEntityType(VendorID);
        // For Checking Shipping AddressOfVendor End  
        if (key != $('#hdnTaggedVender').val()) {
            if (gridquotationLookup.GetValue() != null) {
                setTimeout(function () {
                    GridClearConfirm();
                }, 200);
            }
            else {

                var key = GetObjectID('hdnCustomerId').value;
                if (key != null && key != '') {
                    $('#hdnTaggedVendorName').val(ctxtVendorName.GetText());
                    if ($('#hdnADDEditMode').val() != 'Edit') {
                        var schemabranchid = $('#ddl_numberingScheme').val();
                        if (schemabranchid != '0') {
                            var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                            SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                            page.tabs[1].SetEnabled(true);
                        }
                    }
                    else if ($('#hdnADDEditMode').val() == 'Edit') {
                        var schemabranchid = $('#ddl_Branch').val();
                        if (schemabranchid != '0') {
                            var schemabranch = schemabranchid;
                            SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                            page.tabs[1].SetEnabled(true);
                        }
                    }
                    else {
                        jAlert('Select a numbering schema first');
                        return;
                    }
                }
                else {

                    jAlert('Vendor can not be blank.')
                    $('#ddl_numberingScheme').prop('disabled', false);
                    ctxtVendorName.Focus();
                }
            }
        }
        // Newly added code for vendor search by Sam on 03012018 section End
        $('#CustModel').modal('hide');
        ctxtVendorName.SetText(Name);
        SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
        page.tabs[0].SetEnabled(true);
        page.tabs[1].SetEnabled(true);
        GetObjectID('hdnCustomerId').value = VendorID;
        //Chinmoy added below line
        GetVendorGSTInFromBillShip(GetObjectID('hdnCustomerId').value);
        if ($('#hfBSAlertFlag').val() == "1") {
            jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    page.SetActiveTabIndex(1);
                }
            });
        }
        ctxt_partyInvNo.Focus();
        //cContactPerson.Focus();
        //------------------------Code added for mantis id 0020338--------
        var VendorId = GetObjectID('hdnCustomerId').value;
        var branchid = $('#ddl_Branch').val();
        if (VendorId != null && VendorId != "") {

            //Rev Bapi 
           cContactPerson.PerformCallback('BindContactPerson~' + VendorId + '~' + branchid);
            //End Rev Bapi
            var OtherDetails = {}
            OtherDetails.VendorId = VendorId;
            OtherDetails.vendorbranchid = branchid;
            $.ajax({
                type: "POST",
                url: "PurchaseInvoice.aspx/GetContactPerson",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    if (returnObject) {
                        SetDataSourceOnComboBox(cContactPerson, returnObject);
                    }
                }
            });


            if ($("#btn_TermsCondition").is(":visible")) {
                callTCspecefiFields_PO(VendorId);
            }

            var startDate = new Date();
            startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
            // cPanelGRNOverheadCost.PerformCallback('BindOverheadCostGrid' + '~' + VendorId + '~' + startDate + '~' + '0');
            //clookup_GRNOverhead.gridView.Refresh();

        }
        //-----------------------------End-----------------------------
    }
    if (Id != "" && $('#hdnPInvAmtDetailssettings').val() == "1" && $('#hdnADDEditMode').val() != "Edit")
    {
        var fromDate=$('#hdnnumberingFromdate').val();
        var TodoDate=$('#hdnnumberingTodate').val();
        var bracnvendor = $('#ddl_Branch').val();

        SetVendorWiseTotalAMount(Id, fromDate, TodoDate, bracnvendor, $('#ddlInventory').val());
    }
}

function GetDateFormatForAmt(today) {
    if (today != "" && today != null) {

        today = today.split('-');

        var dd = today[0];
        var mm = today[1];
        var yyyy = today[2];

        today = yyyy + '-' + mm + '-' + dd;
    }
    else {
        today = "";
    }

    return today;
}
function SetVendorWiseTotalAMount(VendorId, fromDate, TodoDate, bracnvendor, InvType)
{
    //var otherdetrails = {};
    //otherdetrails.VendorId = VendorId;
    //otherdetrails.fromDate = fromDate;
    //otherdetrails.TodoDate = TodoDate;
    //otherdetrails.bracnvendor = bracnvendor;
    //otherdetrails.InvType = InvType;
    $.ajax({
        type: "POST",
        url: "purchaseinvoice.aspx/GetVendorWiseTotalTransactedAmt",
        data: JSON.stringify({ VendorId: VendorId, fromDate: fromDate, TodoDate: TodoDate, bracnvendor: bracnvendor, InvType: InvType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d.split('~');

            if (data[0] != "0.00") {
                $('#InvAmount').text(data[0]);
                $('#btnfootTotalAmt').val(data[0]);
                $('#btnfootTotalAmtUnpaid').val(data[1]);
            }
            else {
                $('#InvAmount').text(data[0]);
                $('#btnfootTotalAmt').val(data[0]);
                $('#btnfootTotalAmtUnpaid').val(data[1]);
            }
        }
    });
}


function InvoiceDetails() {

    $('#AmtGridModel').modal('show');
    var OtherDetails = {}
    if ($('#hdnCustomerId').val() == "" || $('#hdnCustomerId').val() == null) {
        return false;
    }
    else
    {
        
        OtherDetails.VendorId = $('#hdnCustomerId').val();
        OtherDetails.FromDate = $('#hdnnumberingFromdate').val();
        OtherDetails.TodoDate = $('#hdnnumberingTodate').val();
        OtherDetails.BranchID = $('#ddl_Branch').val();
        OtherDetails.InventoryType = $('#ddlInventory').val();
        

        var HeaderCaption = [];
        HeaderCaption.push("Invoice Number");
        HeaderCaption.push("Invoice Date");
        HeaderCaption.push("Invoice Amount");
        HeaderCaption.push("Unpaid Amount");
        callonServer("Services/Master.asmx/GetVendorwiseAmountdocument", OtherDetails, "AmtGridTable", HeaderCaption, "", "");

    }
}

function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}
function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "ProdIndex")
                SetProduct(Id, name);
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
            else
                SetCustomer(Id, name);
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
                //added by chinmoy
            else if (indexName == "BillingAreaIndex")
                $('#txtbillingArea').focus();
            else if (indexName == "ShippingAreaIndex")
                $('#txtshippingArea').focus();
            else if (indexName == "customeraddressIndex")
                ('#txtshippingShipToParty').focus();
                //End
            else
                $('#txtCustSearch').focus();
        }
    }
}
// Vendor Search Section End on 03012018
// Final Checking by Sam on 15102017 Start
function ddlInventory_OnChange() {
    var invtype = $('#ddlInventory').val();
    if (invtype == 'N' || invtype == 'S' || invtype == 'C') {
        $('#divTdsScheme').removeClass('hide');
        $('#rdlbutton').addClass('hide');
        $('#rdldate').addClass('hide');
        // Updated by Sam on 13122017 to Hide Terms&Conditions and Vehicle Control Section Start
        $('#spVehTC').addClass('hide');
        // Updated by Sam on 13122017 to Hide Terms&Conditions and Vehicle Control Section End


    }
    else {
        $('#divTdsScheme').addClass('hide');
        $('#rdlbutton').removeClass('hide');
        $('#rdldate').removeClass('hide');
        // Updated by Sam on 13122017 to Show Terms&Conditions and Vehicle Control Section Start
        $('#spVehTC').removeClass('hide');
        // Updated by Sam on 13122017 to Show Terms&Conditions and Vehicle Control Section End
        //var key = gridLookup.GetValue()


        var key = GetObjectID('hdnCustomerId').value;
        if (key != null && key != '') {
          //  var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
            selectValue();
        }
    }
    //Add section by Tanmoy
    if (invtype == 'S') {
        $('#divOverheadCost').removeClass('hide');
    }
    else {
        $('#divOverheadCost').addClass('hide');
    }
    //Add section by Tanmoy
}



//............Check Unique   Purchase Order................
function txtBillNo_TextChanged() {    // function 3
    var mode = ''
    var VoucherNo = document.getElementById("txtVoucherNo").value;
    $.ajax({
        type: "POST",
        url: "purchaseinvoice.aspx/CheckUniqueName",
        data: JSON.stringify({ VoucherNo: VoucherNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d;

            if (data == true) {
                $("#DuplicateBillNo").show();

                document.getElementById("txtVoucherNo").value = '';
                document.getElementById("txtVoucherNo").focus();
            }
            else {
                $("#DuplicateBillNo").hide();
            }
        }
    });
}
function specialedit_ButtonClick() {
    flag = true;
    var invtype = $('#ddlInventory').val();
    if (invtype != 'N') {
        if (ctxt_partyInvNo.GetText() == '' || ctxt_partyInvNo.GetText() == null) {
            flag = false;
            $("#MandatorysPartyinvno").show();
            LoadingPanel.Hide();
            return false;
        }
    }
    // Invoice Date validation Start
    var sdate = cdt_partyInvDt.GetValue();
    var edate = cPLQuoteDate.GetValue();
    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (invtype == 'N') {
        if (sdate == null || sdate == "") {
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');
            $('#MandatoryEgSDate').attr('style', 'display:none');
            flag = true;
        }
    }
    else if (invtype != 'N') {
        if (sdate == null || sdate == "") {
            flag = false;
            $('#MandatoryPartyDate').attr('style', 'display:block');
            LoadingPanel.Hide();
            return false;
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');
            $('#MandatoryEgSDate').attr('style', 'display:none');
            flag = true;
        }
    }
    if (flag == true) {
        cpartyInvoicepanel.PerformCallback('SpecialEdit');
    }
}
//<%--// Running Balance Calculation--%>

function GlobalBillingShippingEndCallBack() {
    
    if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
        cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
        var invtype = $('#ddlInventory').val();
        var startDate = new Date();
        startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
        var branchid = $('#ddl_Branch').val();
        //var key = gridLookup.GetValue()
        var key = GetObjectID('hdnCustomerId').value;
        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
        if (type != null && type != '') {
            //if (gridquotationLookup.GetValue() != null) {
            cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
            var startDate = new Date();
            startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
            //var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : ""; 
            if (key != null && key != '') {
                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
            }
            grid.PerformCallback('GridBlank');
            //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            deleteTax('DeleteAllTax', "", "", "");
            gridquotationLookup.SetText('');
        }
        else {
            //var key = gridLookup.GetValue()
            var key = GetObjectID('hdnCustomerId').value;
            if (key != null && key != '') {
                cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
            }
        }
    }
}
function partyInvDtMandatorycheck() {
    var invtype = $('#ddlInventory').val();
    if (invtype != 'N') {
        var Podt = cdt_partyInvDt.GetValue();
        if (Podt != null) {
            $('#MandatoryPartyDate').attr('style', 'display:none');
            var sdate = cdt_partyInvDt.GetValue();
            var edate = cPLQuoteDate.GetValue();

            var startDate = new Date(sdate);
            var endDate = new Date(edate);
            if (startDate > endDate) {
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');
        }
    }
    else {

    }
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
$(document).ready(function () {

    if ($('#hdnPInvAmtDetailssettings').val() == "1" && $('#hdnADDEditMode').val() != "Edit")
    {
        $('#dvInvoicedetAmount').show();
    }
    else
    {
        $('#dvInvoicedetAmount').hide();
    }

    $('.number').keypress(function (event) {
        if (event.which < 46 || event.which > 59) {
            event.preventDefault();
        } // prevent if not number/dot

        if (event.which == 46 && $(this).val().indexOf('.') != -1) {
            event.preventDefault();
        } // prevent if already dot
    });
});
var PreviousCurrency = "1";
function GetPreviousCurrency() {
    PreviousCurrency = ctxtRate.GetValue();
}

function OnAddNewClick_Default() {
    grid.AddNewRow();

    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var tbQuotation = grid.GetEditor("SrlNo");
    tbQuotation.SetValue(noofvisiblerows);
}

function showhide(obj) {
    if (obj == 'Y') {
        $('#divselectunselect').addClass('hide');
    }
    else {
        $('#divselectunselect').removeClass('hide');
    }
}
function GridProductBind(e) {
    var invtype = $('#ddlInventory').val();
    //cproductPanel.PerformCallback(invtype);
}
$(document).ready(function () {
    var mode = $('#hdnADDEditMode').val();
    if (mode == 'Edit') {
        if ($("#hdnCustomerId").val() != "") {
            var VendorID = $("#hdnCustomerId").val();
            SetEntityType(VendorID);
        }
        $("#rdl_PurchaseInvoice").find('input').prop('disabled', true);
        if ($('#hdnTDSShoworNot').val() == 'S') {
            $('#divTdsScheme').removeClass('hide');

        }
        else if ($('#hdnTDSShoworNot').val() == 'H') {
            $('#divTdsScheme').addClass('hide');

        }

        if ($("#hdnOverHeadCostShoworNot").val() == "S") {
            $('#divOverheadCost').removeClass('hide');
        }
        else if ($("#hdnOverHeadCostShoworNot").val() == "H") {
            $('#divOverheadCost').addClass('hide');
        }

        if ($('#ddlInventory').val() == 'N' || $('#ddlInventory').val() == 'C') {
            $('#spVehTC').addClass('hide');
        }
        else {
            //$('#spVehTC').addClass('hide');
        }
    }
    else {
        $("#rdl_PurchaseInvoice").find('input').prop('disabled', false);
    }
})
//<%-- UDF and Transport Section Start--%>
var canCallBack = true;
function AllControlInitilize() {
    if (canCallBack) {
        if ($('#hdnADDEditMode').val() == 'Edit') {
            cddlPosGstInvoice.SetEnabled(false);
            $('#txtVoucherNo').attr("disabled", true);
            LoadCustomerBillingShippingAddress(GetObjectID('hdnCustomerId').value);
            LoadBranchAddressInEditMode($('#ddl_Branch').val());
            cQuotationComponentPanel.SetEnabled(false)
            PopulateInvoicePosGst();
            cchk_reversemechenism.SetEnabled(false);
            if (cchk_reversemechenism.GetValue()) {
                $('#divreverse').removeClass('hide');
                grid.GetEditor('TaxAmount').SetEnabled(false);
            }
        }
        else {
            ddlInventory_OnChange();
        }
        canCallBack = false;
    }
}
function acbpCrpUdfEndCall(s, e) {

    var result = 0;
    if (cacbpCrpUdf.cpUDF == "true") {
        result = 1;
    }
    else {
        jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
        cacbpCrpUdf.cpUDF = null;
        cacbpCrpUdf.cpTransport = null;
        cacbpCrpUdf.cpTC = null;
        LoadingPanel.Hide();
        $('#hdnRefreshType').val('');
        result = 0;
        return;
    }
    if (cacbpCrpUdf.cpTransport == "true") {
        result = 1;
    }
    else {
        jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
        cacbpCrpUdf.cpUDF = null;
        cacbpCrpUdf.cpTransport = null;
        cacbpCrpUdf.cpTC = null;
        LoadingPanel.Hide();
        $('#hdnRefreshType').val('');
        result = 0;
        return;


    }
    var invtype = $('#ddlInventory').val();

    if (invtype != 'N') {
        if (cacbpCrpUdf.cpTC == "true") {
            result = 1;
        }
        else {
            
            if (cContactPerson.cpvendortype != 'Import') {
                jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
                cacbpCrpUdf.cpUDF = null;
                cacbpCrpUdf.cpTransport = null;
                cacbpCrpUdf.cpTC = null;
                LoadingPanel.Hide();

                $('#hdnRefreshType').val('');
                result = 0;
                return;
            }
            else
            {
                result = 1;
            }
        }
    }
    else {
        result = 1;
    }
    if (cacbpCrpUdf.cpPartyno == "N") {
        result = 1;
    }
    else {
        jAlert("Party Invoice No. already exist for the selected vendor.", "Alert", function () { });
        cacbpCrpUdf.cpUDF = null;
        cacbpCrpUdf.cpTransport = null;
        cacbpCrpUdf.cpTC = null;
        LoadingPanel.Hide();

        $('#hdnRefreshType').val('');
        result = 0;
        return;
    }

    if (cacbpCrpUdf.cpStateId == "Y") {
        result = 1;
        FinalSaveUpdate();
    }
    else {
        LoadingPanel.Hide();
        var messege = 'Vendor' + "'s " + 'shipping address not exist.GST/Reverse charges not to be calculated.Proceed?';
        jConfirm(messege, 'Confirmation Dialog', function (r) {
            if (r == true) {
                result = 1;
                FinalSaveUpdate();
            }
            else {
                result = 0;
                $('#hdnRefreshType').val('');
            }
            cacbpCrpUdf.cpStateId = null;
            return;
        });
    }
}

function FinalSaveUpdate() {
    OnAddNewClick_Default();
    grid.UpdateEdit();
    cacbpCrpUdf.cpUDF = null;
    cacbpCrpUdf.cpTransport = null;
    cacbpCrpUdf.cpTC = null;
}
function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}
//<%--UDF and Transport Section End--%>
function ProductsPopup() {

    var selectedrow = gridquotationLookup.gridView.GetSelectedRowCount();
    if (selectedrow > 1) {
        if ($("#hdnPartyInvoiceList").val() != "") {
            var partyinvoice = $("#hdnPartyInvoiceList").val();
            ctxt_partyInvNo.SetText(partyinvoice);
        }
    }
}
function HideSelectAllSection() {
    // start 02-07-2019
    var selectedrow = gridquotationLookup.gridView.GetSelectedRowCount();
    if (selectedrow > 1) {
        if (cgridproducts.cpPartyInvoiceList != "") {
            var partyinvoice = cgridproducts.cpPartyInvoiceList;
            ctxt_partyInvNo.SetText(partyinvoice);
        }
    }
    var selectedrow = gridquotationLookup.gridView.GetSelectedRowCount();
    if (selectedrow == 1) {
        if (cgridproducts.cpPartyInvoiceList != "") {
            var partyno = cgridproducts.cppartydetail.toString().split('~')[0];

            ctxt_partyInvNo.SetText(partyno);
            var partydate = cgridproducts.cppartydetail.toString().split('~')[1];
            cdt_partyInvDt.SetText(partydate);
            // cdt_partyInvDt.SetDate(new Date(partydate));
        }
    }
    //End
    if (cgridproducts.cpSelectHide != null) {
        if (cgridproducts.cpSelectHide == 'Y') {
            $('#divselectunselect').addClass('hide')
        }
        else if (cgridproducts.cpSelectHide == 'N') {
            $('#divselectunselect').removeClass('hide')
        }
        else {
            $('#divselectunselect').removeClass('hide')
        }
        cgridproducts.cpSelectHide == null;
    }
    if (cgridproducts.cppartydetail != null) {
        var invtype = $('#ddlInventory').val();
        var partyno = cgridproducts.cppartydetail.toString().split('~')[0];
        var partydate = cgridproducts.cppartydetail.toString().split('~')[1];
        var docdate = cgridproducts.cppartydetail.toString().split('~')[2];
        var modetype = $('#hdnADDEditMode').val();
        if (modetype != 'Edit') {
            //ctxt_partyInvNo.SetText(partyno);
            $("#hdn_party_inv_no").val(partyno);
            if (invtype != 'N') {
                if (partyno != null && partyno != '') {
                    $("#MandatorysPartyinvno").hide();
                }
                else {
                    $("#MandatorysPartyinvno").show();
                }
            }
            else {
                $("#MandatorysPartyinvno").hide();
            }
            if (invtype != 'N') {

                if (partydate != null && partydate != '') {
                    cdt_partyInvDt.SetText(partydate);

                    //cdt_partyInvDt.SetDate(new Date(partydate));
                    $('#MandatoryPartyDate').attr('style', 'display:none');
                } else {
                    $('#MandatoryPartyDate').attr('style', 'display:block');
                }
            }
            else {
                $('#MandatoryPartyDate').attr('style', 'display:none');
            }
        }
        var reff = cgridproducts.cppartydetail.toString().split('~')[3];
        var curr = cgridproducts.cppartydetail.toString().split('~')[4];
        var rate = cgridproducts.cppartydetail.toString().split('~')[5];
        var person = cgridproducts.cppartydetail.toString().split('~')[6];
        var amtare = cgridproducts.cppartydetail.toString().split('~')[7];
        var taxcode = cgridproducts.cppartydetail.toString().split('~')[8];
        var EWayBillNumber = cgridproducts.cppartydetail.toString().split('~')[9];
        cPLQADate.SetText(docdate);
        cddl_AmountAre.SetEnabled(false);
        if (reff != '') {
            ctxt_Refference.SetText(reff);
        }
        if (person != '') {

            //cContactPerson.SetValue(person);
            var key = GetObjectID('hdnCustomerId').value;
            var branchid = $('#ddl_Branch').val();
            cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
        }
        if (curr != '') {
            $("#ddl_Currency").val(curr);
        }
        if (rate != '') {
            ctxtRate.SetText(rate);
        }
        if (amtare != '') {
            cddl_AmountAre.SetValue(amtare);
        }
        if (taxcode != '') {
            cddlVatGstCst.PerformCallback('SetTaxCode' + '~' + taxcode)
            var items = $('#cddlVatGstCst option').length;
            cddlVatGstCst.SetValue(taxcode);
        }
        if (EWayBillNumber != '') {
            $('#txtEWayBillNumber').val(EWayBillNumber);

        }
        cgridproducts.cppartydetail = null;
    }
}
//<%--Div Detail for Vendor Section Start--%>
function acpContactPersonPhoneEndCall(s, e) {
    if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
        pageheaderContent.style.display = "block";
        $("#divContactPhone").attr('style', 'display:block');
        document.getElementById('lblContactPhone').innerHTML = cacpContactPersonPhone.cpPhone;
        cacpContactPersonPhone.cpPhone = null;

    }
    else {
        $("#divContactPhone").attr('style', 'display:none');
        document.getElementById('lblContactPhone').innerHTML = '';
        cacpContactPersonPhone.cpPhone = null;
    }
}
function GetContactPersonPhone(e) {
    
    var key = cContactPerson.GetValue();
    cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);

    var branchid = $('#ddl_Branch').val();
    cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
}

//Added By chinmoy
function IfVendorGstInIsBlank() {
    //cddl_AmountAre.SetValue("3");
    PopulateGSTCSTVAT();
    //cddl_AmountAre.SetEnabled(false);
}

function cmbContactPersonEndCall(s, e) {
    

    //Chinmoy edited start

    if ($('#hfVendorGSTIN').val() == '') {
        IfVendorGstInIsBlank();
    }

    else if ($('#hfVendorGSTIN').val() != '') {
        cddl_AmountAre.SetValue("1");
    }
    //end
    if (cContactPerson.cpContactdtl != null && cContactPerson.cpContactdtl != undefined) {
        if (cContactPerson.cpContactdtl == 'Y') {
            $("#pageheaderContent").attr('style', 'display:block');
            $("#divContactPhone").attr('style', 'display:block');

        }
        else {
            $("#divContactPhone").attr('style', 'display:none');
            document.getElementById('lblContactPhone').innerHTML = '';
        }
        cContactPerson.cpContactdtl = null;
    }
    else {
        $("#divContactPhone").attr('style', 'display:none');
        document.getElementById('lblContactPhone').innerHTML = '';
    }
    var edate = cPLQuoteDate.GetValue();
    var str = $.datepicker.formatDate('yy-mm-dd', edate);
    if ((cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) &&
        (cContactPerson.cpvendortype != null && cContactPerson.cpvendortype != undefined)) {
        $("#pageheaderContent").attr('style', 'display:block');
        $("#divGSTN").attr('style', 'display:block');
        document.getElementById('lblGSTIN').innerHTML = cContactPerson.cpGSTN + ' (' + cContactPerson.cpvendortype + ')';
        if (cContactPerson.cpGSTN == 'Yes' && cContactPerson.cpvendortype == 'Regular') {
            cddl_vendortype.SetValue('R');
            // Sandip for Terms & Condition Checking where it is mandatory or not is pending  Section Start
            $('#pnl_TCspecefiFields_PO').css('display', 'none')
            $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
            // Sandip for Terms & Condition Checking where it is mandatory or not Pending Section End
            var invtype = $('#ddlInventory').val();
            if (invtype == 'Y' || invtype == 'B' || invtype == 'C') {
            }
            else if (invtype == 'N') {
            }
            cddl_AmountAre.SetValue(1);
            PopulateGSTCSTVAT();
            cddl_AmountAre.SetEnabled(true);
        }
        else if (cContactPerson.cpGSTN == 'Yes' && cContactPerson.cpvendortype == 'Composite') {
            // Sandip for Terms & Condition Checking where it is mandatory or not is pending  Section Start
            $('#pnl_TCspecefiFields_PO').css('display', 'none')
            $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
            // Sandip for Terms & Condition Checking where it is mandatory or not Pending Section Start
            cddl_vendortype.SetValue('C');
            var invtype = $('#ddlInventory').val();
            if (invtype == 'Y' || invtype == 'B' || invtype == 'C') {
                //$('#rdlbutton').removeClass('hide');
                //$('#rdldate').removeClass('hide');
            }
            else if (invtype == 'N') {
                //$('#rdlbutton').addClass('hide');
                //$('#rdldate').addClass('hide');
            }
            cddl_AmountAre.SetValue(3);
            PopulateGSTCSTVAT();
            cddl_AmountAre.SetEnabled(false);
        }
        else if (cContactPerson.cpvendortype == 'Import') {
            // Sandip for Terms & Condition Checking where it is mandatory or not is pending  Section Start
            $('#pnl_TCspecefiFields_PO').css('display', 'none')
            $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
            // Sandip for Terms & Condition Checking where it is mandatory or not Pending Section Start
            cddl_vendortype.SetValue('I');
            var invtype = $('#ddlInventory').val();
            if (invtype == 'Y' || invtype == 'B' || invtype == 'C') {
                //$('#rdlbutton').removeClass('hide');
                //$('#rdldate').removeClass('hide');
            }
            else if (invtype == 'N') {
                //$('#rdlbutton').addClass('hide');
                //$('#rdldate').addClass('hide');
            }
            cddl_AmountAre.SetValue(1);
            PopulateGSTCSTVAT();
            cddl_AmountAre.SetEnabled(true);
        }




        else {
            if (cContactPerson.cpcountry != null && cContactPerson.cpcountry != '') {
                if (cContactPerson.cpcountry != '1') {

                    // Code Commented by Sam on 01022018 Section Start
                    //cchk_reversemechenism.SetValue(false);  //Commented
                    //cchk_reversemechenism.SetEnabled(false) // Commented
                    // Code Commented by Sam on 01022018 Section End

                    //$('#rdlbutton').removeClass('hide');
                    //$('#rdldate').removeClass('hide');
                    cddl_AmountAre.SetValue(4);

                    cContactPerson.cpcountry == null
                    // $('#hfTCspecefiFieldsVisibilityCheck').val('1');
                    //$('#pnl_TCspecefiFields_PO').css('display', 'block')
                    //$('#pnl_TCspecefiFields_Not_PO').css('display', 'none')

                }
                else {
                    $('#hfTCspecefiFieldsVisibilityCheck').val('');
                    var RB = document.getElementById("rdl_PurchaseInvoice");
                    if (RB.rows.length > 0) {
                        for (i = 0; i < RB.rows.length; i++) {
                            var cell = RB.rows[i].cells;
                            for (j = 0; j < cell.length; j++) {
                                if (cell[j].childNodes[0].type == "radio") {
                                    document.getElementById(cell[j].childNodes[0].id).checked = false;
                                }
                            }
                        }
                    }
                    // Waiting for Dirction  End
                    cddl_AmountAre.SetValue(3);

                    cContactPerson.cpcountry == null
                    $('#pnl_TCspecefiFields_PO').css('display', 'none')
                    $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
                }

            }
            else {
                $('#hfTCspecefiFieldsVisibilityCheck').val('');
                var RB = document.getElementById("rdl_PurchaseInvoice");
                if (RB.rows.length > 0) {
                    for (i = 0; i < RB.rows.length; i++) {
                        var cell = RB.rows[i].cells;
                        for (j = 0; j < cell.length; j++) {
                            if (cell[j].childNodes[0].type == "radio") {
                                document.getElementById(cell[j].childNodes[0].id).checked = false;
                            }
                        }
                    }
                }

                // Waiting for Dirction  End
                cddl_AmountAre.SetValue(3);

                cContactPerson.cpcountry == null
                $('#pnl_TCspecefiFields_PO').css('display', 'none')
                $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
            }
            PopulateGSTCSTVAT();
            cddl_AmountAre.SetEnabled(false);
        }
        cContactPerson.cpGSTN = null;
    }
    else {
        $("#divGSTN").attr('style', 'display:none');
        document.getElementById('lblGSTIN').innerHTML = '';
        cContactPerson.cpGSTN = null;
    }
    if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {
        $("#pageheaderContent").attr('style', 'display:block');
        $("#divOutstanding").attr('style', 'display:block');
        document.getElementById('lblOutstanding').innerHTML = cContactPerson.cpOutstanding;
        cContactPerson.cpOutstanding = null;
    }
    else {
        $("#divOutstanding").attr('style', 'display:none');
        document.getElementById('lblOutstanding').innerHTML = '';
    }
    ctxt_partyInvNo.Focus();
}

var globalRowIndex;
var rowEditCtrl;

var ProductGetQuantity = "0";
var ProductGetTotalAmount = "0";
var ProductPurchaseprice = "0";
var ProductDiscount = "0";
var ProductDiscountAmt = "0";
var ProductGrsAmt = "0";
// Running Calculation As New Modification
var globalNetAmount = 0;
function DiscountGotChange() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;
    ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductPurchaseprice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    // Running total Calculation Start 
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    // Running total Calculation End
}
function DiscountAmtGotChange() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;
    ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductPurchaseprice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    ProductDiscountAmt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
    // Running total Calculation Start 
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    // Running total Calculation End
}

function AmtGotFocus() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;
    ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductPurchaseprice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    ProductDiscountAmt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
    ProductGrsAmt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";

    // Running total Calculation Start

    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

    // Running total Calculation End
}
// Running Calculation As New Modification
// To set Quantity column false if tagging is available by Sam on 10062017 Start   
function QuantityGotFocus(s, e) {
    var taxqty = grid.GetEditor("Quantity").GetValue();

    var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
    if (type == 'PC') {
        grid.GetEditor("Quantity").SetEnabled(false);
    }
    else {
        grid.GetEditor("Quantity").SetEnabled(true);
    }
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductGetQuantity = QuantityValue;
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    // Running total Calculation Start 
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    // Running total Calculation End

    //debugger;
    //Rev 1.0 Subhra 15-03-2019
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strProductName = SpliteDetails[1];
    var strDescription = SpliteDetails[1];

    var isOverideConvertion = SpliteDetails[23];
    var packing_saleUOM = SpliteDetails[22];
    var sProduct_SaleUom = SpliteDetails[21];
    var sProduct_quantity = SpliteDetails[20];
    var packing_quantity = SpliteDetails[19];

    var slno = (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

    var PurchaseInvoiceNum = (grid.GetEditor('ComponentNumber').GetText() != null) ? grid.GetEditor('ComponentNumber').GetText() : "0";

    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';
    var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
    var gridPackingQty = 0.00;
    var actionQry = '';
    var rdl_PurchaseInvoice = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";

    //if (SpliteDetails.length > 23) {
    if (SpliteDetails.length == 25) {
        if (SpliteDetails[24] == "1") {
            IsInventory = 'Yes';
        }
        else {
            IsInventory = '';
        }
    }

    else {
        IsInventory = '';
    }

    if (rdl_PurchaseInvoice == 'PO') {
        actionQry = 'GetPurchaseInvoiceQtyOrder';
    }
    if (rdl_PurchaseInvoice == 'PC') {
        actionQry = 'GetPurchaseInvoiceQtyByGRN';
    }
    type = 'edit';

    if (PurchaseInvoiceNum != "0" && PurchaseInvoiceNum != "" && $('#hdnADDEditMode').val() != "Edit") {

        if ($("#hddnMultiUOMSelection").val() == "0") {
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMultiUOMDetails",
                data: JSON.stringify({ orderid: strProductID, action: actionQry, module: 'PurchaseINV', strKey: PurchaseInvoiceNum }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    gridPackingQty = msg.d;

                    if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                        ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                    }

                }
            });
        }

    }
    else if ($('#hdnADDEditMode').val() == "Edit") {
        if ($("#hddnMultiUOMSelection").val() == "0") {
            var orderid = grid.GetRowKey(globalRowIndex);
            //var orderid = document.getElementById('Keyval_Id').value;
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMultiUOMDetails",
                data: JSON.stringify({ orderid: orderid, action: 'GetPurchaseInvoiceQty', module: 'PurchaseINV', strKey: strProductID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    gridPackingQty = msg.d;
                    //if (PurchaseInvoiceNum == "0" && PurchaseInvoiceNum == "") {  // if not blank thats means this data are from tagged  data....you cannot modify it.
                    if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                        ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                    }
                    //}
                }
            });
        }
    }
    else {
        if ($("#hddnMultiUOMSelection").val() == "0") {
            if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
            }
        }
    }
}
// To set Quantity column false if tagging is available by Sam on 10062017 End
//Rev Subhra 18-03-2019
var issavePacking = 0;
function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('Quantity').SetValue(Quantity);

    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    }, 600)


    QuantityTextChange();
}
//End of Rev Subhra 18-03-2019
function PurPriceGotFocus() {
    ProductPurchaseprice = grid.GetEditor("PurchasePrice").GetValue();
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    // Running total Calculation Start 
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    // Running total Calculation End 
}
//............................Product Pazination..............
function ChangeState(value) {
    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}
function ctaxUpdatePanelEndCall(s, e) {
    if (ctaxUpdatePanel.cpstock != null) {
        divAvailableStk.style.display = "block";
        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
        document.getElementById('lblAvailableStk').innerHTML = ctaxUpdatePanel.cpstock;
        ctaxUpdatePanel.cpstock = null;
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }
    return false;
}
function ProductsGotFocusFromID(s, e) {
    //var customerval = gridLookup.GetValue() 
    var customerval = GetObjectID('hdnCustomerId').value;
    if ($('#txtVoucherNo').val() == '' || $('#txtVoucherNo').val() == null) {
        jAlert('Select a numbering schema first.')
        $('#ddl_numberingScheme').focus();
        return false;
    }
    else if (customerval == '' || customerval == null || customerval == "") {
        jAlert('Select a Vendor first')
        //gridLookup.Focus();
        ctxtVendorName.Focus();
        return false;
    }
    else {
        pageheaderContent.style.display = "block";
        var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
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
        var IsPackingActive = SpliteDetails[13];
        var Packing_Factor = SpliteDetails[14];
        var Packing_UOM = SpliteDetails[15];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
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
}
function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
}
function ProductSelected(s, e) {
    if (!cproductLookUp.FindItemByValue(cproductLookUp.GetValue())) {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 4);
        jAlert("Product not Exists.", "Alert", function () { cproductLookUp.SetValue(); cproductLookUp.Focus(); });
        return;
    }
    var LookUpData = cproductLookUp.GetValue();
    var ProductCode = cproductLookUp.GetText();
    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);
    cddl_TdsScheme.SetEnabled(false);
    $('#ddlInventory').prop('disabled', true);
    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("PurchasePrice");
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
    } else {
        divPacking.style.display = "none";
    }
    // Running total Calculation Start
    Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    CalculateAmount();
    // Running total Calculation End

    //Debjyoti
    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    grid.batchEditApi.StartEdit(globalRowIndex, 6);
}
function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}
//.............Available Stock Div Show............................
function ProductsGotFocus(s, e) {
    pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
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
    var IsPackingActive = SpliteDetails[13];
    var Packing_Factor = SpliteDetails[14];
    var Packing_UOM = SpliteDetails[15];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        //  divPacking.style.display = "block";
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
//Code for UDF Control 
function OpenUdf(s, e) {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=PB&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}
// End Udf Code 
//..................Rate........................
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
        //grid.PerformCallback('CurrencyChangeDisplay');
    }
}
//...............end.........................
//...............PopulateVAT........................
function PopulateGSTCSTVAT(e) {
    var key = cddl_AmountAre.GetValue();
    //deleteAllRows(); 
    if (key == 1 || key == 4) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.SetSelectedIndex(-1);
        cbtn_SaveRecords.SetVisible(true);
        grid.GetEditor('ProductID').Focus();
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
        cddlVatGstCst.SetSelectedIndex(-1);
        cddlVatGstCst.SetEnabled(false);
        cbtn_SaveRecords.SetVisible(false);
    }
}
function Keypressevt() {
    if (event.keyCode == 13) {
        //run code for Ctrl+X -- ie, Save & Exit! 
        SaveWarehouse();
        return false;
    }
}

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

//.................End PopulateVAT...............
//................Amount Calculation.........................
function TaxAmountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}
function taxAmtButnClick1(s, e) {
    
    var taxtype = cddl_AmountAre.GetValue();
    var Vendortype = cddl_vendortype.GetValue();
    if (taxtype == '3' || Vendortype == 'C') {
        grid.GetEditor('TaxAmount').SetEnabled(false);
    }
    else {
        grid.GetEditor("TaxAmount").SetEnabled(true);
    }

    rowEditCtrl = s;
}
function taxAmtButnClick(s, e) {
    if (e.buttonIndex == 0) {
        if (cddl_AmountAre.GetValue() != null) {
            var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
            if (ProductID.trim() != "") {

                // Running total Calculation Start

                Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

                // Running total Calculation End

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
                var strStkUOM = SpliteDetails[4];
                var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "";
                if (strRate == 0) {
                    strRate = 1;
                }
                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = parseFloat(Math.round(QuantityValue * strFactor * (strSalePrice / strRate) * 100) / 100).toFixed(2);
                document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                clblTaxProdGrossAmt.SetText(Amount);
                clblProdNetAmt.SetText(grid.GetEditor('Amount').GetValue());
                document.getElementById('HdProdGrossAmt').value = Amount;
                document.getElementById('HdProdNetAmt').value = parseFloat(Math.round(grid.GetEditor('Amount').GetValue() * 100) / 100).toFixed(2);
                //End Here 
                //Set Discount Here
                if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                    var discount = (Amount * grid.GetEditor('Discount').GetValue() / 100);
                    //var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
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
                    $('.gstNetAmount').hide();
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
                    //###### Added By : Samrat Roy ##########
                    //Get Customer Shipping StateCode
                    var shippingStCode = '';

                    // Here we are sending Branch StateCode instead of Shipping Statecode after discuss with
                    // Pijush Da and Debjyoti on 14122017
                    //shippingStCode = $("#ucBShfSStateCode").val();
                    if (cddlPosGstInvoice.GetValue() == "S") {
                        shippingStCode = GeteShippingStateCode();
                    }
                    else {
                        shippingStCode = GetBillingStateCode();
                    }


                    shippingStCode = shippingStCode;


                    //###### END : Samrat Roy : END ########## 

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
                                ccmbGstCstVat.RemoveItem(cmbCount);
                                cmbCount--;
                            }
                        }
                    }

                } else {
                    clblTaxableGross.SetText("");
                    clblTaxableNet.SetText("");

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

function SalePriceTextChange(s, e) {
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
        var strFactor = SpliteDetails[14]; //Packing_Factor 
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        var strProductID = SpliteDetails[0];
        var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();
        var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
        var strSalePrice = SpliteDetails[6];// purchase Price 
        if (strRate == 0) {
            strRate = 1;
        }
        if (strSalePrice == 0.00000) {
            strSalePrice = 1;
        }
        var StockQuantity = strMultiplier * QuantityValue;
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        $('#lblbranchName').text(strBranch);

        var IsPackingActive = SpliteDetails[13];//IsPackingActive
        var Packing_Factor = SpliteDetails[14];//Packing_Factor
        var Packing_UOM = SpliteDetails[15];//Packing_UOM
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
            // divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }
        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(Amount);
        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(Amount);
        DiscountTextChange(s, e);
        //.........AvailableStock.............

    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Quantity').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
}
// Non Inventory Section By Sam on 24052017
function TDSChecking(s, e) {
    var inventoryItem = $('#ddlInventory').val();
    if (inventoryItem == 'N' || inventoryItem == 'S') {
        var schemeid = cddl_TdsScheme.GetValue()
        if (schemeid != '0') {
            var ProductID = grid.GetEditor('ProductID').GetValue();
            if (ProductID != null) {


                var slno = grid.GetEditor('SrlNo').GetValue();
                if ($('#hdntdschecking').val() == '') {
                    $('#hdntdschecking').val(slno + ',');
                }
                else {
                    var myArray = $('#hdntdschecking').val().split(',');
                    if ($.inArray(slno, myArray) != -1) {

                    }
                    else {
                        $('#hdntdschecking').val($('#hdntdschecking').val() + slno)
                    }


                }
            }
        }
    }
}
function qtyvalidate() {
    var srlno = grid.GetEditor('SrlNo').GetValue();
    var previousqty = grid.GetEditor('Quantity').GetValue();

    return $.ajax({
        type: "POST",
        url: 'PurchaseInvoice.aspx/ValidQuantity',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ srlno: srlno, previousqty: previousqty }),
        cache: false
    });

}
// Non Inventory Section By Sam on 24052017
function QuantityTextChange(s, e) {
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)) {
        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
        if (type == 'PO') {
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

        TDSChecking();
        pageheaderContent.style.display = "block";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
            var strFactor = SpliteDetails[8]; //Packing_Factor 
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();
            var strStkUOM = SpliteDetails[4];//Stk_UOM_Name 
            var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";

            if (strFactor == 0) {
                strFactor = 1;
            }
            if (strRate == 0) {
                strRate = 1;
            }
            if (strSalePrice == 0.00000) {
                strSalePrice = 1;
            }
            var StockQuantity = strMultiplier * QuantityValue;
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount);
            DiscountTextChange(s, e);
            //.........AvailableStock............ 
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Quantity').SetValue('0');
            grid.GetEditor('PurchasePrice').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}
function PurchasePriceTextChange(s, e) {
    
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var PriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    if (ProductPurchaseprice != parseFloat(PriceValue)) {
        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
        if (type == 'PO') {
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

        TDSChecking();
        pageheaderContent.style.display = "block";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
            var strFactor = SpliteDetails[8]; //Packing_Factor 
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();
            var strStkUOM = SpliteDetails[4];//Stk_UOM_Name 
            var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            if (strFactor == 0) {
                strFactor = 1;
            }
            if (strRate == 0) {
                strRate = 1;
            }
            if (strSalePrice == 0.00000) {
                strSalePrice = 1;
            }
            var StockQuantity = strMultiplier * QuantityValue;
            ///0024414 comment 
            /// var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
            var Amount = QuantityValue * strFactor * (strSalePrice);
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);
            grid.GetEditor('TaxAmount').SetValue(0);
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount);
            DiscountTextChange(s, e);
            //.........AvailableStock............. 
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Quantity').SetValue('0');
            grid.GetEditor('PurchasePrice').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}
function ProductPriceCalculate() {
    
    if ((grid.GetEditor('PurchasePrice').GetValue() == null || grid.GetEditor('PurchasePrice').GetValue() == 0) && (grid.GetEditor('Discount').GetValue() == null || grid.GetEditor('Discount').GetValue() == 0)) {
        var _purchaseprice = 0;
        var _Qty = grid.GetEditor('Quantity').GetValue();
        var _Amount = grid.GetEditor('Amount').GetValue();
        _purchaseprice = (_Amount / _Qty);
        grid.GetEditor('PurchasePrice').SetValue(_purchaseprice);
    }
}
function AmtTextChange(s, e) {
    
    ProductPriceCalculate();
    TDSChecking();
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var PurPrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var Discountamt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
    var ProductWiseGrsAmt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        if (parseFloat(Discount) != parseFloat(ProductDiscount)
            || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)
            || parseFloat(PurPrice) != parseFloat(ProductPurchaseprice)
            || parseFloat(Discountamt) != parseFloat(ProductDiscountAmt)
             || parseFloat(ProductWiseGrsAmt) != parseFloat(ProductGrsAmt)) {
            var SpliteDetails = ProductID.split("||@||");
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            if (strFactor == 0) {
                strFactor = 1;
            }
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }
            if (strRate == 0) {
                strRate = 1;
            }
            var Discountamt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
            var Discount = ((parseFloat(Discountamt) * 100) / parseFloat(Amount));
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            var grossamt = grid.GetEditor('Amount').GetValue();
            //Chinmoy edited below code
            // grid.GetEditor('TaxAmount').SetValue(0);
            var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(parseFloat(grossamt) + parseFloat(_TotalTaxAmt));
            var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
                //Chinmoy edited below code
                //grid.GetEditor('TaxAmount').SetValue(0);
                ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
                var incluexclutype = ''
                var taxtype = cddl_AmountAre.GetValue();
                if (taxtype == '1') {
                    incluexclutype = 'E'
                }
                else if (taxtype == '2') {
                    incluexclutype = 'I'
                }


                var CompareStateCode;
                if (cddlPosGstInvoice.GetValue() == "S") {
                    CompareStateCode = GeteShippingStateID();
                }
                else {
                    CompareStateCode = GetBillingStateID();
                }

                var checkval = cchk_reversemechenism.GetChecked();
                if (!checkval) {
                    if ($('#hdnADDEditMode').val() != 'Edit') {
                        var schemabranchid = $('#ddl_numberingScheme').val();
                        if (schemabranchid != '0') {
                            var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                            // Here we are sending Branch StateID instead of Shipping StateID after discuss with
                            // Pijush Da and Debjyoti on 14122017
                            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')
                            //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')

                        }
                    }
                    else if ($('#hdnADDEditMode').val() == 'Edit') {
                        var schemabranchid = $('#ddl_Branch').val();
                        if (schemabranchid != '0') {
                            var schemabranch = schemabranchid;
                            // Here we are sending Branch StateID instead of Shipping StateID after discuss with
                            // Pijush Da and Debjyoti on 14122017
                            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')
                            //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')
                        }
                    }
                }
            }
            // Running total Calculation Start
            Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            CalculateAmount();
            // Running total Calculation End
        }
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Amount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
}

function DiscountAmtTextChange(s, e) {
    TDSChecking();
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var PurPrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var Discountamt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(PurPrice) != parseFloat(ProductPurchaseprice) || parseFloat(Discountamt) != parseFloat(ProductDiscountAmt)) {
            var SpliteDetails = ProductID.split("||@||");
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            if (strFactor == 0) {
                strFactor = 1;
            }
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }
            if (strRate == 0) {
                strRate = 1;
            }
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
            var Discount = ((parseFloat(Discountamt) * 100) / parseFloat(Amount));
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            var DiscountamtField = grid.GetEditor("Discount");
            DiscountamtField.SetValue(Discount);
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);

            var IsPackingActive = SpliteDetails[13];
            var Packing_Factor = SpliteDetails[14];
            var Packing_UOM = SpliteDetails[15];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#lblPackingStk').text(PackingValue);
            } else {
                divPacking.style.display = "none";
            }
            grid.GetEditor('TaxAmount').SetValue(0);
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(parseFloat(amountAfterDiscount) + parseFloat(_TotalTaxAmt));
        }
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discountamt').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    //Debjyoti  
    var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
        grid.GetEditor('TaxAmount').SetValue(0);
        ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
        var incluexclutype = ''
        var taxtype = cddl_AmountAre.GetValue();
        if (taxtype == '1') {
            incluexclutype = 'E'
        }
        else if (taxtype == '2') {
            incluexclutype = 'I'
        }

        var CompareStateCode;
        if (cddlPosGstInvoice.GetValue() == "S") {
            CompareStateCode = GeteShippingStateID();
        }
        else {
            CompareStateCode = GetBillingStateID();
        }


        var checkval = cchk_reversemechenism.GetChecked();
        if (!checkval) {
            if ($('#hdnADDEditMode').val() != 'Edit') {
                var schemabranchid = $('#ddl_numberingScheme').val();
                if (schemabranchid != '0') {
                    var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                    // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                    // Pijush Da and Debjyoti on 14122017
                    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')
                    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')
                }
            }
            else if ($('#hdnADDEditMode').val() == 'Edit') {
                var schemabranchid = $('#ddl_Branch').val();
                if (schemabranchid != '0') {
                    var schemabranch = schemabranchid;
                    // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                    // Pijush Da and Debjyoti on 14122017
                    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')
                    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')
                }
            }
        }
    }
    // Running total Calculation Start
    Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    CalculateAmount();
    // Running total Calculation End 
}

function DiscountTextChange(s, e) {
    TDSChecking();
    var PurPrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(PurPrice) != parseFloat(ProductPurchaseprice)) {
            var SpliteDetails = ProductID.split("||@||");
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            if (strFactor == 0) {
                strFactor = 1;
            }
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }
            if (strRate == 0) {
                strRate = 1;
            }
            ///0024414  comment division strRate
            //var Amount = parseFloat(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);

            var Amount = parseFloat(QuantityValue * strFactor * (strSalePrice)).toFixed(2);
            var Discountamt = parseFloat((parseFloat(Discount) * parseFloat(Amount)) / 100).toFixed(2);
            var amountAfterDiscount = parseFloat(parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100)).toFixed(2);
            var DiscountamtField = grid.GetEditor("Discountamt");
            DiscountamtField.SetValue(Discountamt);
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);
            var IsPackingActive = SpliteDetails[13];
            var Packing_Factor = SpliteDetails[14];
            var Packing_UOM = SpliteDetails[15];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#lblPackingStk').text(PackingValue);
            } else {
                divPacking.style.display = "none";
            }
            grid.GetEditor('TaxAmount').SetValue(0);
            var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(parseFloat(parseFloat(amountAfterDiscount) + parseFloat(_TotalTaxAmt)).toFixed(2));
        }
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
        grid.GetEditor('TaxAmount').SetValue(0);
        ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());

        var incluexclutype = ''
        var taxtype = cddl_AmountAre.GetValue();
        if (taxtype == '1') {
            incluexclutype = 'E'
        }
        else if (taxtype == '2') {
            incluexclutype = 'I'
        }


        var CompareStateCode;
        if (cddlPosGstInvoice.GetValue() == "S") {
            CompareStateCode = GeteShippingStateID();
        }
        else {
            CompareStateCode = GetBillingStateID();
        }
        var checkval = cchk_reversemechenism.GetChecked();
        if (!checkval) {
            if ($('#hdnADDEditMode').val() != 'Edit') {
                var schemabranchid = $('#ddl_numberingScheme').val();
                if (schemabranchid != '0') {
                    var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                    // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                    // Pijush Da and Debjyoti on 14122017
                    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, 'P')
                    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')

                    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')


                }
            }
            else if ($('#hdnADDEditMode').val() == 'Edit') {
                var schemabranchid = $('#ddl_Branch').val();
                if (schemabranchid != '0') {
                    var schemabranch = schemabranchid;
                    // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                    // Pijush Da and Debjyoti on 14122017
                    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, 'P')
                    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')
                    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')


                }
            }

        }
    }

    // Running total Calculation Start
    Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    CalculateAmount();
    // Running total Calculation End

}

//......................Amount Calculation End.......................
/*........................Tax Start...........................*/
var taxAmountGlobalCharges;
var chargejsonTax;
var taxAmountGlobal;
var GlobalCurChargeTaxAmt;
var ChargegstcstvatGlobalName;
var GlobalCurTaxAmt = 0;
var rowEditCtrl;
var globalRowIndex;
var globalTaxRowIndex;
var gstcstvatGlobalName;
var taxJson;
function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cPopup_Taxes.Hide();
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
                //Rev Rajdip For 	0021885:
                //gridTax.GetEditor("Amount").SetValue(Sum);
                gridTax.GetEditor("Amount").SetValue(GlobalTaxAmt);
                //ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                //End Rev Rajdip

                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
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
function QuotationTaxAmountTextChange(s, e) {
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges);
        ctxtTotalAmount.SetText(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    RecalCulateTaxTotalAmountCharges();
}
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
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
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
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }
    SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
    SetChargesRunningTotal();
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
    ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
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
function TaxAmountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}
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
    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);


    //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
    //Set Running Total
    SetRunningTotal();

    RecalCulateTaxTotalAmountInline();
}
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

//function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
//    for (var i = 0; i < taxJson.length; i++) {
//        if (taxJson[i].applicableBy == name) {
//            cgridTax.batchEditApi.StartEdit(i, 3);
//            cgridTax.GetEditor('calCulatedOn').SetValue(amt);
//            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
//            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
//            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
//            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
//            var s = cgridTax.GetEditor("TaxField");
//            if (sign == '(+)') {
//                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
//                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

//                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
//                GlobalCurTaxAmt = 0;
//            }
//            else {
//                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
//                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
//                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
//                GlobalCurTaxAmt = 0;
//            }
//        }
//    }
//    cgridTax.batchEditApi.EndEdit();
//}

function SetOtherTaxValueOnRespectiveRow(idx, amt, name, runninTot, signCal) {
    for (var i = 0; i < taxJson.length; i++) {
        if (taxJson[i].applicableBy == name) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            var totCal = 0;
            if (signCal == '(+)') {
                totCal = parseFloat(parseFloat(amt) + parseFloat(runninTot));
            }
            else {
                totCal = parseFloat(parseFloat(runninTot) - parseFloat(amt));
            }
            cgridTax.GetEditor('calCulatedOn').SetValue(totCal);

            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var s = cgridTax.GetEditor("TaxField");
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue()) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
                GlobalCurTaxAmt = 0;
            }
            else {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (s.GetText() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
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
    gridTax.batchEditApi.EndEdit();
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
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);

            //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
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
    ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
}
//function SetRunningTotal() {
//    var runningTot = parseFloat(clblProdNetAmt.GetValue());
//    for (var i = 0; i < taxJson.length; i++) {
//        cgridTax.batchEditApi.StartEdit(i, 3);
//        if (taxJson[i].applicableOn == "R") {
//            cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
//            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
//            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
//            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
//            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
//            var thisRunningAmt = 0;
//            if (sign == '(+)') {
//                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
//                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);
//                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
//                GlobalCurTaxAmt = 0;
//            }
//            else {
//                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
//                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);
//                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
//                GlobalCurTaxAmt = 0;
//            }
//            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
//        }
//        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
//        cgridTax.batchEditApi.EndEdit();
//    }
//}

function SetRunningTotal() {

    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        if (taxJson[i].applicableOn == "R") {
            cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();

            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var thisRunningAmt = 0;
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (cgridTax.GetEditor("TaxField").GetValue() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));

                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (cgridTax.GetEditor("TaxField").GetValue() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
                GlobalCurTaxAmt = 0;
            }
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), ProdAmt, sign);
        }
        if (sign == '(+)') {
            runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }
        else {
            runningTot = runningTot - parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }

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
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}
function txtTax_TextChanged(s, i, e) {
    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
}
function NonInventoryBatchUpdate() {
    var cnt = cgridinventory.GetVisibleItemsOnPage();
    //cgridinventory.PerformCallback('0' + '~' + '0' + '~' + 'SaveNonInventoryProductChg' + '~' + cnt); 
    cgridinventory.AddNewRow();
    cgridinventory.batchEditApi.StartEdit(0, 3);
    cgridinventory.batchEditApi.EndEdit();
    if (cgridinventory.GetVisibleRowsOnPage() > 0) {
        cgridinventory.UpdateEdit();
    }
    //cgridinventory.UpdateEdit();
    return false;
}
function BatchUpdate() {
    $('#ddl_numberingScheme').prop('disabled', true);
    //gridLookup.SetEnabled(false);
    ctxtVendorName.SetEnabled(false);

    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        cgridTax.UpdateEdit();
    }
    else {
        cgridTax.PerformCallback('SaveGST');
    }
    return false;
}
function cgridTax_EndCallBack(s, e) {
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
        grid.batchEditApi.StartEdit(globalRowIndex, 13);
        grid.GetEditor("TaxAmount").SetValue(totAmt);
        //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section Start
        //grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
        //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section End
        if (cddl_AmountAre.GetValue() == '2') {

            //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section Start
            var prodqty = (grid.GetEditor("Quantity").GetValue());
            var prodrate = (grid.GetEditor("PurchasePrice").GetValue());
            var prodDiscAmt = (grid.GetEditor("Discountamt").GetValue());
            var prodNetAmt = (parseFloat(prodqty) * parseFloat(prodrate)) - parseFloat(prodDiscAmt)
            grid.GetEditor("TotalAmount").SetValue(parseFloat(prodNetAmt));
            //var prodNetAmt = (grid.GetEditor("TotalAmount").GetValue());
            var prodIncluSiveTax = (grid.GetEditor("TaxAmount").GetValue());
            grid.GetEditor("Amount").SetValue(parseFloat(prodNetAmt) - parseFloat(prodIncluSiveTax));
        }
        else {
            grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
        }
        // Running total Calculation Start
        Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        CalculateAmount();
        // Running total Calculation End 
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
function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}
/*............................End Tax...........................................*/
function BindOrderProjectdata(OrderId, TagDocType) {

    var OtherDetail = {};

    OtherDetail.OrderId = OrderId;
    OtherDetail.TagDocType = TagDocType;


    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "PurchaseInvoice.aspx/SetProjectCode",
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
            url: 'PurchaseInvoice.aspx/getHierarchyID',
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

function componentEndCallBack(s, e) {
    //  
    var loadingmode = $('#hdnADDEditMode').val();
    if (loadingmode != 'Edit') {
        gridquotationLookup.gridView.Refresh();
        cProductsPopup.Hide();
    }
    if (grid.GetVisibleRowsOnPage() == 0) {
        OnAddNewClick();
    }
    // Tagging Component Section Start
    if (cQuotationComponentPanel.cpDetails != null) {
        var details = cQuotationComponentPanel.cpDetails;
        cQuotationComponentPanel.cpDetails = null;
        var SpliteDetails = details.split("~");
        var Reference = SpliteDetails[0];
        var Currency_Id = SpliteDetails[1];
        var SalesmanId = SpliteDetails[2];
        var ExpiryDate = SpliteDetails[3];
        var CurrencyRate = SpliteDetails[4];
        ctxt_Refference.SetValue(Reference);
        ctxtRate.SetValue(CurrencyRate);
        document.getElementById('ddl_Currency').value = Currency_Id;
        document.getElementById('ddl_SalesAgent').value = SalesmanId;
    }
    gridquotationLookup.Focus();
}



var SimilarProjectStatus = "0";


function SimilarProjetcheck(quote_Id, Doctype) {
    $.ajax({
        type: "POST",
        url: "PurchaseInvoice.aspx/DocWiseSimilarProjectCheck",
        data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            SimilarProjectStatus = msg.d;
            
            if (SimilarProjectStatus != "1") {
                cPLQADate.SetText("");
                jAlert("Please select document with same project code to proceed.");

                return false;

            }
        }
    });
}

function CloseGridQuotationLookup() {
    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();


    var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
    if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {
        var Doctype = $("#rdl_PurchaseInvoice").find(":checked").val();
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

        SimilarProjetcheck(quote_Id, Doctype);

    }

}

//Chinmoy added below function
function validateOrderwithAmountAre() {
    //Check Multiple Row amount are selected or not

    var selectedKeys = gridquotationLookup.gridView.GetSelectedKeysOnPage();
    var ammountsAreOrder = "";
    if (selectedKeys.length > 0) {
        for (var loopcount = 0 ; loopcount < gridquotationLookup.gridView.GetVisibleRowsOnPage() ; loopcount++) {

            var nowselectedKey = gridquotationLookup.gridView.GetRowKey(loopcount);

            var found = selectedKeys.find(function (element) {
                return element == nowselectedKey;
            });

            if (found) {
                if (ammountsAreOrder != "" && ammountsAreOrder != gridquotationLookup.gridView.GetRow(loopcount).children[9].innerText) {

                    jAlert("Unable to procceed. Amount are for the selected order(s) are different");

                    return false;
                }
                else
                    ammountsAreOrder = gridquotationLookup.gridView.GetRow(loopcount).children[9].innerText;
            }

        }

    }

    return true;
}
//End


function QuotationNumberChanged() {
    var quote_Id = gridquotationLookup.GetValue();
    var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
    if (type != "") {


        if (SimilarProjectStatus != "-1") {
            if (quote_Id != null) {
                var OrderData = gridquotationLookup.gridView.GetSelectedKeysOnPage();

                if (validateOrderwithAmountAre() == false) {
                    
                    var startDate = new Date();
                    startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
                    var key = GetObjectID('hdnCustomerId').value;
                    if (key != null && key != '') {
                        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                    }
                    var invtype = $('#ddlInventory').val();
                    if (key != null && key != '') {
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                    }
                    grid.PerformCallback('GridBlank');
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax('DeleteAllTax', "", "", "");
                    gridquotationLookup.SetText('');
                    gridquotationLookup.Clear();

                    cProductsPopup.Hide();

                }
                else if (OrderData == 0) {
                    gridquotationLookup.gridView.Hide();
                }
                else if (OrderData != 0 && validateOrderwithAmountAre() == true) {

                    if (gridquotationLookup.gridView.GetSelectedRowCount() > 0) {
                        cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@' + '~' + quote_Id);
                        cProductsPopup.Show();
                        //chinmoy start  02-07-2019
                        var selectedrow = gridquotationLookup.gridView.GetSelectedRowCount();

                        if (selectedrow > 1) {
                            //gridquotationLookup.gridView.GetSelectedRowCount();
                            for (var loopcount = 0; loopcount < gridquotationLookup.gridView.GetSelectedRowCount() ; loopcount++) {

                                if (loopcount == 0) {
                                    var partyinvoicedate = gridquotationLookup.gridView.GetRow(loopcount).children[8].innerText;
                                    var date = partyinvoicedate;
                                    var datearray = date.split("/");
                                    if (datearray[1].length == 1) {
                                        datearray[1] = '0' + datearray[1];
                                    }
                                    if (datearray[0].length == 1) {
                                        datearray[0] = '0' + datearray[0];
                                    }
                                    var newdate = datearray[1] + '-' + datearray[0] + '-' + datearray[2];
                                    cdt_partyInvDt.SetText(newdate);
                                }
                            }
                        }
                    }
                }


            }
            else {
                if (gridquotationLookup.gridView.GetSelectedRowCount() > 0) {
                    cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
                    cProductsPopup.Show();
                    var selectedrow = gridquotationLookup.gridView.GetSelectedRowCount();
                    if (selectedrow > 0) {
                        var partyinvoice = gridquotationLookup.gridView.GetRow(0).children[7].innerText;
                        ctxt_partyInvNo.SetText(partyinvoice);
                    }
                }
            }
        }
    }
    else
    {
        jAlert('Please Check Radio Button Value.')
        return;
    }
}
function SetDifference1() {
    var diff = CheckDifferenceOfFromDateWithTodate();
}
function CheckDifferenceOfFromDateWithTodate() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLSalesOrderDate.GetDate();
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
    startDate = cPLSalesOrderDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;

    }
    return difference;

}

function OnInventoryEndCallback(s, e) {
    if (cgridinventory.cpPopupReq == 'N') {
        OnAddNewClick();
        jAlert('Applicable amount is greater than entered amount.TDS is not required for this amount.')
        return;
    }
    else if (cgridinventory.cpgrid == "Y") {

        cgridinventory.batchEditApi.StartEdit(0, 2);

        if (cgridinventory.cpEditTDS == "1") {
            cchk_TDSEditable.SetValue(true);
        }
        else {
            cchk_TDSEditable.SetValue(false);
        }
        cgridinventory.cpEditTDS = null;
        var checkval = cchk_TDSEditable.GetChecked();
        if (checkval) {
            cgridinventory.SetEnabled(true);
        }
        else {
            cgridinventory.SetEnabled(false);
        }
        if (cgridinventory.cpbrMonAmtDtl != null) {
            var tdsbranch = cgridinventory.cpbrMonAmtDtl.toString().split('~')[0];
            var tdsmonth = cgridinventory.cpbrMonAmtDtl.toString().split('~')[1];
            var tdsTotalAmt = cgridinventory.cpbrMonAmtDtl.toString().split('~')[2];
            var tdsProbAmt = cgridinventory.cpbrMonAmtDtl.toString().split('~')[3];
            var branchid = $("#ddl_Branch option:selected").text();
            $('#lbltdsBranch').text(branchid);
            if (cgridinventory.cpmonth != null) {
                var monthnm = cgridinventory.cpmonth;
                if (monthnm == '1') {
                    cddl_month.SetValue('January');
                }
                else if (monthnm == '2') {
                    cddl_month.SetValue('February');
                }
                else if (monthnm == '3') {
                    cddl_month.SetValue('March');
                }
                else if (monthnm == '4') {
                    cddl_month.SetValue('April');
                }
                else if (monthnm == '5') {
                    cddl_month.SetValue('May');
                }
                else if (monthnm == '6') {
                    cddl_month.SetValue('June');
                }
                else if (monthnm == '7') {
                    cddl_month.SetValue('July');
                }
                else if (monthnm == '8') {
                    cddl_month.SetValue('August');
                }
                else if (monthnm == '9') {
                    cddl_month.SetValue('September');
                }
                else if (monthnm == '10') {
                    cddl_month.SetValue('October');
                }
                else if (monthnm == '11') {
                    cddl_month.SetValue('November');
                }
                else if (monthnm == '12') {
                    cddl_month.SetValue('December');
                }
            }
            cddl_month.SetValue(tdsmonth);
            if (tdsTotalAmt != null && tdsTotalAmt != '') {
                if (!chkNILRateTDS.GetChecked()) {
                    ctxt_totalnoninventoryproductamt.SetText(tdsTotalAmt);
                }
                else {
                    ctxt_totalnoninventoryproductamt.SetText("0.00");
                }
            }
            ctxt_proamt.SetText(tdsProbAmt);
            cgridinventory.cpgrid = null;
        }
        else if (cgridinventory.cpbrMonAmtDtl == null) {
            ctxt_proamt.SetText(parseFloat(cgridinventory.cpproamt));
            if (!chkNILRateTDS.GetChecked()) {
                ctxt_totalnoninventoryproductamt.SetText(parseFloat(cgridinventory.cpprochargeamt));
            }
            else {
                ctxt_totalnoninventoryproductamt.SetText("0.00");
            }
            var branchid = $("#ddl_Branch option:selected").text();
            $('#lbltdsBranch').text(branchid);
            if (cgridinventory.cpmonth != null) {
                var monthnm = cgridinventory.cpmonth;
                if (monthnm == '1') {
                    cddl_month.SetValue('January');
                }
                else if (monthnm == '2') {
                    cddl_month.SetValue('February');
                }
                else if (monthnm == '3') {
                    cddl_month.SetValue('March');
                }
                else if (monthnm == '4') {
                    cddl_month.SetValue('April');
                }
                else if (monthnm == '5') {
                    cddl_month.SetValue('May');
                }
                else if (monthnm == '6') {
                    cddl_month.SetValue('June');
                }
                else if (monthnm == '7') {
                    cddl_month.SetValue('July');
                }
                else if (monthnm == '8') {
                    cddl_month.SetValue('August');
                }
                else if (monthnm == '9') {
                    cddl_month.SetValue('September');

                }
                else if (monthnm == '10') {
                    cddl_month.SetValue('October');
                }
                else if (monthnm == '11') {
                    cddl_month.SetValue('November');
                }
                else if (monthnm == '12') {
                    cddl_month.SetValue('December');
                }
            }
            var checkval = cchk_TDSEditable.GetChecked();
            if (checkval) {
                cgridinventory.SetEnabled(true);
            }
            else {
                cgridinventory.SetEnabled(false);
            }
        }
        cinventorypopup.Show();
        cgridinventory.cpgrid = null;
    }
    else if (cgridinventory.cppopuphide == "Y") {
        var branchid = $("#ddl_Branch option:selected").text();
        $('#lbltdsBranch').text(branchid);
        cddl_month.SetSelectedIndex(-1);
        ctxt_proamt.SetText('');
        ctxt_totalnoninventoryproductamt.SetText('');
        cinventorypopup.Hide();
        OnAddNewClick();
    }
}


function Call_OK() {
    cPopup_NoofCopies.Hide();
    if (ddlnoofcopies.value == "1") {
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=1', '_blank')
    }
    else if (ddlnoofcopies.value == "2") {
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=1', '_blank')
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=2', '_blank')
    }
    else if (ddlnoofcopies.value == "3") {
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=1', '_blank')
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=2', '_blank')
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=4', '_blank')
    }
    grid.cpPurchaseOrderNo = null;
    grid.cpGeneratedInvoice = null;
    if (NewExit == 'N') {
        window.location.assign("purchaseinvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
    }
    else if (NewExit == 'E') {
        window.location.assign("PurchaseInvoicelist.aspx");
    }

}

function GridCallBack() {
    grid.PerformCallback('Display');
}
function TDSDetail(s, e) {
    var inventoryItem = $('#ddlInventory').val();
    if (inventoryItem == 'N') {
        document.getElementById('hdnInvWiseSlno').value = grid.GetEditor('SrlNo').GetText();
        var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        var Amt = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
        var Productdt = Productdtl.split("||@||");
        var ProductID = Productdt[0];
        ctxt_proamt.SetText('Amt');
        var TDSValue = cddl_TdsScheme.GetValue();
        cgridinventory.PerformCallback(ProductID + '~' + Amt + '~' + 'ShowBindGrid' + '~' + '' + '~' + TDSValue);
    }
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
    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.StartEdit();
        var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
        $('#hdnRefreshType').val('');
        $('#hdnDeleteSrlNo').val(SrlNo);
        var inventoryItem = $('#ddlInventory').val();
        if (inventoryItem == 'Y' || inventoryItem == 'B') {
            if (gridquotationLookup.GetValue() != null) {
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                if (type == 'PO') {

                    jAlert('Cannot Delete using this button as the Purchase Order is linked with this Purchase Invoice.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) { });
                }
                else if (type == 'PC') {
                    $('#hdnDeleteSrlNo').val('');
                    jAlert('Cannot Delete using this button as the Purchase Challan is linked with this Purchase Invoice.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) { });
                }
            }
            else {
                var noofvisiblerows = grid.GetVisibleRowsOnPage();
                if (noofvisiblerows != "1" && gridquotationLookup.GetValue() == null) {
                    // Running total Calculation Start 
                    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                    Cur_Quantity = "0";
                    Cur_Amt = "0";
                    Cur_TotalAmt = "0";
                    CalculateAmount();
                    // Running total Calculation End 
                    grid.DeleteRow(e.visibleIndex);
                    if (inventoryItem == 'N') {
                        $('#hdinvetorttype').val('N');
                    }
                    $('#hdfIsDelete').val('D');
                    grid.UpdateEdit();
                    $('#hdnPageStatus').val('delete');
                }
            }
        }
        else {
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            if (noofvisiblerows != "1") {
                // Running total Calculation Start 
                Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                Cur_Quantity = "0";
                Cur_Amt = "0";
                Cur_TotalAmt = "0";
                CalculateAmount();

                // Running total Calculation End 
                grid.DeleteRow(e.visibleIndex);
                if (inventoryItem == 'N') {
                    $('#hdinvetorttype').val('N');
                }
                $('#hdfIsDelete').val('D');
                grid.UpdateEdit();
                $('#hdnPageStatus').val('delete');
            }
        }
    }
    if (e.buttonID == 'CustomAddNewRow') {
        grid.batchEditApi.StartEdit(e.visibleIndex);

        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        var scembranch = '';
        if (ProductID != "") {
            //chinmoy edited 19-02-2020  mantis 23686 start
            if (chkNILRateTDS.GetChecked()) {
                cchk_TDSEditable.SetEnabled(false);
            }
            else {
                cchk_TDSEditable.SetEnabled(true);
            }
            ///end
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem == 'N' || inventoryItem == 'S') {
                //var customerval = gridLookup.GetValue();
                var customerval = GetObjectID('hdnCustomerId').value;
                var vendorid = GetObjectID('hdnCustomerId').value
                if (customerval == '' || customerval == null) {
                    jAlert('Select a vendor first');
                    return
                }
                else {
                    var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    if (Productdtl != null && Productdtl != '') {
                        var Productdt = Productdtl.split("||@||");
                        var ProductID = Productdt[0];
                        var schemeid = cddl_TdsScheme.GetValue()
                        if (schemeid != '0') {
                            var checkval = cchk_TDSEditable.GetChecked();
                            if (checkval) {
                                cgridinventory.SetEnabled(true);
                                //cgridinventory.batchEditApi.StartEdit();
                            }
                            else {
                                cgridinventory.SetEnabled(false);
                            }

                            document.getElementById('hdnInvWiseSlno').value = grid.GetEditor('SrlNo').GetText();
                            var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                            var Amt = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
                            var Productdt = Productdtl.split("||@||");
                            var ProductID = Productdt[0];
                            ctxt_proamt.SetText('Amt');
                            // Code Added by sam to set focus on TDS gridview on 05022018

                            // Code Above Added by sam to set focus on TDS gridview on 05022018
                            var TDSValue = cddl_TdsScheme.GetValue();
                            //Rev Priti
                            if (TDSValue == null) {
                                TDSValue = 0;
                            }
                            //Rev Priti End 
                            cgridinventory.PerformCallback(ProductID + '~' + Amt + '~' + 'ShowBindGrid' + '~' + 'CheckApplicableAmt'+'~'+ TDSValue);
                            var slno = grid.GetEditor('SrlNo').GetValue();
                            if ($('#hdntdschecking').val() != '') {
                                var myArray = $('#hdntdschecking').val().split(',');
                                var index = myArray.indexOf(slno);
                                if (index > -1) {
                                    myArray.splice(index, 1);
                                    $('#hdntdschecking').val(myArray);
                                }

                            }
                            cgridinventory.batchEditApi.EndEdit();
                        }
                        else {
                            OnAddNewClick();
                        }
                    }
                    else {
                        jAlert('Select a Product first.');
                        return;
                    }
                }
            }
            else if (gridquotationLookup.GetValue() == null) {
                grid.batchEditApi.StartEdit(e.visibleIndex);
                var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                var SpliteDetails = ProductID.split("||@||");
                var IsComponentProduct = SpliteDetails[16];
                var ComponentProduct = SpliteDetails[17];
                if (IsComponentProduct == "Y") {
                    var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
                    jConfirm(messege, 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            grid.GetEditor("IsComponentProduct").SetValue("Y");
                            $('#hdfIsComp').val('C');
                            grid.UpdateEdit();
                            grid.PerformCallback('Display~fromComponent');
                        }
                        else {
                            OnAddNewClick();
                        }
                    });
                    document.getElementById('popup_ok').focus();
                }
                    // Component tagging Section End by Sam
                else if (ProductID != "") {
                    OnAddNewClick();
                }
                else {
                    grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                }
            }
            else {
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                if (type == 'PO') {
                    if ($("#hdnPartialSettings").val() == "No")
                        cgridproducts.SetEnabled(false);
                }
                else if (type == 'PC') {
                    if ($("#hdnPartialSettings").val() == "No")
                        cgridproducts.SetEnabled(false);
                }
                QuotationNumberChanged();
            }
        }
        else {
            jAlert('Select a product first');
            grid.GetEditor("ProductName").Focus();
            return;
        }
    }

    else if (e.buttonID == "addlDesc") {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex, 5);
        cPopup_InlineRemarks.Show();

        $("#txtInlineRemarks").val('');

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        if (ProductID != "") {
            // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
            ccallback_InlineRemarks.PerformCallback('DisplayRemarks' + '~' + SrlNo + '~' + '0');

        }
    }


    else if (e.buttonID == 'CustomMultiUOM') {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex);
        var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ProductID = Productdetails.split("||@||")[0];
        var UOMName = grid.GetEditor("UOM").GetValue();
        var quantity = grid.GetEditor("Quantity").GetValue();
        var DetailsId = grid.GetEditor('DetailsId').GetText();
        var StockUOM = Productdetails.split("||@||")[5];
        if (StockUOM == "") {
            StockUOM = "0";
        }
        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        cAltUOMQuantity.SetValue("0.0000");
        // Mantis Issue 24429
        //if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
        if ((ProductID != "") && (UOMName != "") ) {
            // End of Mantis Issue 24429

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
                    // Mantis Issue 24429
                    //$("#UOMQuantity").val(Qnty);
                    $("#UOMQuantity").val("0.0000");
                    ccmbBaseRate.SetValue(0)
                    cAltUOMQuantity.SetValue("0.0000")
                    ccmbAltRate.SetValue(0)
                    ccmbSecondUOM.SetValue("")
                    // End of Mantis Issue 24429
                    cPopup_MultiUOM.Show();
                    cgrid_MultiUOM.cpDuplicateAltUOM = "";
                    AutoPopulateMultiUOM();
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



    else if (e.buttonID == 'CustomAddNewTDS') {
        grid.batchEditApi.StartEdit(e.visibleIndex);

        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        var scembranch = '';
        if (ProductID != "") {
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem == 'N' || inventoryItem == 'S') {
                //var customerval = gridLookup.GetValue();
                var customerval = GetObjectID('hdnCustomerId').value;
                var vendorid = GetObjectID('hdnCustomerId').value
                if (customerval == '' || customerval == null) {
                    jAlert('Select a vendor first');
                    return
                }
                else {
                    var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    if (Productdtl != null && Productdtl != '') {
                        var Productdt = Productdtl.split("||@||");
                        var ProductID = Productdt[0];
                        var schemeid = cddl_TdsScheme.GetValue()
                        if (schemeid != '0') {
                            var checkval = cchk_TDSEditable.GetChecked();
                            if (checkval) {
                                cgridinventory.SetEnabled(true);
                                //cgridinventory.batchEditApi.StartEdit();
                            }
                            else {
                                cgridinventory.SetEnabled(false);
                            }

                            document.getElementById('hdnInvWiseSlno').value = grid.GetEditor('SrlNo').GetText();
                            var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                            var Amt = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
                            var Productdt = Productdtl.split("||@||");
                            var ProductID = Productdt[0];
                            ctxt_proamt.SetText('Amt');
                            // Code Added by sam to set focus on TDS gridview on 05022018

                            // Code Above Added by sam to set focus on TDS gridview on 05022018
                            var TDSValue = cddl_TdsScheme.GetValue();
                            cgridinventory.PerformCallback(ProductID + '~' + Amt + '~' + 'ShowBindGrid' + '~' + 'CheckApplicableAmt' + '~' + TDSValue);
                            var slno = grid.GetEditor('SrlNo').GetValue();
                            if ($('#hdntdschecking').val() != '') {
                                var myArray = $('#hdntdschecking').val().split(',');
                                var index = myArray.indexOf(slno);
                                if (index > -1) {
                                    myArray.splice(index, 1);
                                    $('#hdntdschecking').val(myArray);
                                }

                            }
                            cgridinventory.batchEditApi.EndEdit();
                        }
                        else {
                            //OnAddNewClick();
                        }
                    }
                    else {
                        jAlert('Select a Product first.');
                        return;
                    }
                }
            }
            else if (gridquotationLookup.GetValue() == null) {
                grid.batchEditApi.StartEdit(e.visibleIndex);
                var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                var SpliteDetails = ProductID.split("||@||");
                var IsComponentProduct = SpliteDetails[16];
                var ComponentProduct = SpliteDetails[17];
                if (IsComponentProduct == "Y") {
                    var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
                    jConfirm(messege, 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            grid.GetEditor("IsComponentProduct").SetValue("Y");
                            $('#hdfIsComp').val('C');
                            grid.UpdateEdit();
                            grid.PerformCallback('Display~fromComponent');
                        }
                        else {
                            OnAddNewClick();
                        }
                    });
                    document.getElementById('popup_ok').focus();
                }
                    // Component tagging Section End by Sam
                else if (ProductID != "") {
                    OnAddNewClick();
                }
                else {
                    grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                }
            }
            else {
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                if (type == 'PO') {
                    cgridproducts.SetEnabled(false);
                }
                else if (type == 'PC') {
                    cgridproducts.SetEnabled(false);
                }
                QuotationNumberChanged();
            }
        }
        else {
            jAlert('Select a product first');
            grid.GetEditor("ProductName").Focus();
            return;
        }
    }

    else if (e.buttonID == 'CustomWarehouse') {
        var inventoryItem = $('#ddlInventory').val();
        if (inventoryItem == 'N') {
            jAlert("NonInventory Item has no warehouse detail.")
            return
        }
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;
        document.getElementById('hdngridvselectedrowno').value = e.visibleIndex;
        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
        if (type != 'PC' && type != 'PO') {
            return;
        }
        if (QuantityValue == "0.0") {
            jAlert("Quantity should not be zero !.");
        }
        else {
            if (type == 'PC' || type == 'PO') {
                cbtnaddWarehouse.SetVisible(false); // During Tagging Add button should be Enabled false
                cbtnWarehouse.SetVisible(false);
                cButton1.SetVisible(false);
                cbtnrefreshWarehouse.SetVisible(false);
                cCmbWarehouse.SetEnabled(false);
            }
            else {

                cbtnaddWarehouse.SetVisible(true);
                cbtnWarehouse.SetVisible(true);
                cButton1.SetVisible(true);
                cbtnrefreshWarehouse.SetVisible(true);
                cCmbWarehouse.SetEnabled(true);
            }
            $("#spnCmbWarehouse").hide();  // First Hide during stock detail clicking
            $("#spnCmbBatch").hide();      // First Hide during stock detail clicking
            $("#spncheckComboBox").hide(); // First Hide during stock detail clicking
            $("#spntxtQuantity").hide();   // First Hide during stock detail clicking
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
                $('#hdfProductIDPC').val(strProductID);  // assign Productid of the selected row
                $('#hdfProductType').val("");            // assign Producttype of the selected row
                $('#hdfProductSerialID').val(SrlNo);     // assign sl no of the selected row 
                $('#hdnProductQuantity').val(QuantityValue); // assign Product qty of the selected row
                var Ptype = "";
                $('#hdnisserial').val("");        // serial id is black initialized in first time
                $('#hdnisbatch').val("");     // Batch id is black initialized in first time
                $('#hdniswarehouse').val("");    // Warehouse id is black initialized in first time 
                $.ajax({
                    type: "POST",
                    url: 'PurchaseInvoice.aspx/getProductType',
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
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("false");
                            }
                            else if (Ptype == "S") {
                                $('#hdnisserial').val("true");
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
                            $("#RequiredFieldValidatortxtbatch").css("display", "none");         // validation are hidden in starting for batch detail txt box
                            $("#RequiredFieldValidatortxtserial").css("display", "none");        // validation are hidden in starting for serial detail txt box
                            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");     // validation are hidden in starting for ware house detail txt box 
                            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");   // validation are hidden in starting for batch qty txt box
                            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");    // validation are hidden in starting for warehouse qty txt box 
                            $(".blockone").css("display", "none");                // this div is hidden for warehouse detail in starting
                            $(".blocktwo").css("display", "none");                 // this div is hidden for Batch detail in starting
                            $(".blockthree").css("display", "none");             // this div is hidden for Serial detail in starting 
                            ctxtqnty.SetText("0.0");            // for warehouse quantity text box
                            ctxtqnty.SetEnabled(true);
                            ctxtbatchqnty.SetText("0.0");       // for Batch quantity text box not for detail text box
                            ctxtserial.SetText("");             // for serial  detail text box
                            ctxtbatchqnty.SetText("");
                            ctxtbatch.SetEnabled(true);        // for Batch Number text box
                            cCmbWarehouse.SetEnabled(true);
                            // starting Phase
                            $('#hdnisedited').val("false");           // starting Phase
                            $('#hdnisoldupdate').val("false");        // starting Phase
                            $('#hdnisnewupdate').val("false");        // starting Phase 
                            $('#hdnisolddeleted').val("false");       // starting Phase 
                            $('#hdntotalqntyPC').val(0);              // starting Phase
                            $('#hdnoldrowcount').val(0);              // starting Phase
                            $('#hdndeleteqnity').val(0);              // starting Phase
                            $('#hidencountforserial').val("1");       // starting Phase 
                            $('#hdfstockidPC').val(0);               // starting Phase
                            $('#hdfopeningstockPC').val(0);          // starting Phase
                            $('#oldopeningqntity').val(0);           // starting Phase
                            $('#hdnnewenterqntity').val(0);          // starting Phase 
                            $('#hdnenterdopenqnty').val(0);         // starting Phase
                            $('#hdbranchIDPC').val(0);              // starting Phase 
                            $('#hdnisviewqntityhas').val("false");  // starting Phase 
                            $('#hdndefaultID').val("");             // starting Phase
                            $('#hdnbatchchanged').val("0");        // starting Phase
                            $('#hdnrate').val("0");                // starting Phase
                            $('#hdnvalue').val("0");               // starting Phase
                            $('#hdnstrUOM').val(strUOM);          // starting Phase 
                            var branchid = $("#ddl_Branch option:selected").val();

                            var productid = SpliteDetails[0] ? SpliteDetails[0] : "";
                            var StkQuantityValue = QuantityValue ? QuantityValue : "0.0000";
                            var stockids = 0;
                            var checkstockdecimalornot = StkQuantityValue.toString().split(".")[1]
                            $('#hdnpcslno').val(SrlNo);
                            var ProductName = SpliteDetails[1] ? SpliteDetails[1] : "";
                            var ratevalue = "0";
                            var rate = "0";
                            var branchid = $('#ddl_Branch').val();
                            var BranchNames = $("#ddl_Branch option:selected").text();
                            var strProductID = productid;
                            var strDescription = "";
                            var strUOM = (strUOM != null) ? strUOM : "0";
                            var strProductName = ProductName;
                            document.getElementById('lblbranchName').innerHTML = BranchNames;
                            var availablestock = SpliteDetails[12];
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
                            if (iswarehousactive == "true") {    // If Product type Has WH or W Type

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
                            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
                            cPopup_WarehousePC.Show();
                        }
                    }
                });
            }
        }
    }
}
function Save_ButtonClick() {

    $("#hdn_party_inv_no").val(ctxt_partyInvNo.GetText());
    flag = true;
    LoadingPanel.Show();
    cbtn_SaveRecords.SetEnabled(false);
    $('#hfControlData').val($('#hfControlSaveData').val());
    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();
    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        flag = false;
        $("#MandatoryBillNo").show();
        LoadingPanel.Hide();
        return false;
    }


    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        flag = false;
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        return false;
    }

    if (($("#hdBillingPin").val() == "0") && ($("#hdShippingPin").val() == "0")) {
        LoadingPanel.Hide();
        flag = false;

        jAlert('Please enter pin number.');
        page.SetActiveTabIndex(1);
        return false;
    }


    var Podt = cPLQuoteDate.GetValue();
    if (Podt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDate").show();
        flag = false;
        return false;
    }
    var invtype = $('#ddlInventory').val();
    if (invtype != 'N') {
        if (ctxt_partyInvNo.GetText() == '' || ctxt_partyInvNo.GetText() == null) {
            flag = false;
            $("#MandatorysPartyinvno").show();
            LoadingPanel.Hide();
            return false;
        }
    }
    // Invoice Date validation Start
    var sdate = cdt_partyInvDt.GetValue();
    var edate = cPLQuoteDate.GetValue();
    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (invtype == 'N') {
        if (sdate == null || sdate == "") {
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');
            $('#MandatoryEgSDate').attr('style', 'display:none');
        }
    }
    else if (invtype != 'N') {
        if (sdate == null || sdate == "") {
            flag = false;
            $('#MandatoryPartyDate').attr('style', 'display:block');
            LoadingPanel.Hide();
            return false;
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');
            $('#MandatoryEgSDate').attr('style', 'display:none');
        }
    }
    //var customerval = gridLookup.GetValue();
    var customerval = GetObjectID('hdnCustomerId').value
    if (customerval == '' || customerval == null) {
        LoadingPanel.Hide();
        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }
    if (Podt == null) {
        $("#MandatoryDate").show();
        LoadingPanel.Hide();
        return false;
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
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem == 'N' && cddl_TdsScheme.GetValue() != '0') {
                if ($('#hdnADDEditMode').val() != 'Edit') {
                    var tdschk = $('#hdntdschecking').val()
                    if (tdschk != '') {
                        LoadingPanel.Hide();
                        jAlert('Please enter TDS detail for Sl no.' + tdschk)
                        return false;
                    }
                }
            }
            if (inventoryItem == 'S' && cddl_TdsScheme.GetValue() != '0') {
                if ($('#hdnADDEditMode').val() != 'Edit') {
                    var tdschk = $('#hdntdschecking').val()
                    if (tdschk != '') {
                        LoadingPanel.Hide();
                        jAlert('Please enter TDS detail for Sl no.' + tdschk)
                        return false;
                    }
                }
            }
            if (grid.GetVisibleRowsOnPage() > 0) {
                //Subhra 18-03-2019
                if (issavePacking == 1 && aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "PurchaseInvoice.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var customerval = GetObjectID('hdnCustomerId').value;
                            $('#hdfLookupCustomer').val(customerval);
                            $('#hdfIsDelete').val('I');
                            $('#hdnRefreshType').val('N');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                    });
                }
                else {
                    //Subhra 18-03-2019
                    //var customerval = gridLookup.GetValue();
                    var customerval = GetObjectID('hdnCustomerId').value;
                    $('#hdfLookupCustomer').val(customerval);
                    $('#hdfIsDelete').val('I');
                    $('#hdnRefreshType').val('N');

                    grid.batchEditApi.EndEdit();
                    cacbpCrpUdf.PerformCallback();
                }
            }
            else {
                LoadingPanel.Hide();
                jAlert('Please add atleast single record first');
            }
        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}
function SaveExit_ButtonClick() {
    
    $("#hdn_party_inv_no").val(ctxt_partyInvNo.GetText());
    flag = true;
    LoadingPanel.Show();
    cbtn_SaveRecords.SetEnabled(false);
    $('#hfControlData').val($('#hfControlSaveData').val());
    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();
    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        LoadingPanel.Hide();
        flag = false;
        $("#MandatoryBillNo").show();
        return false;
    }


    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        flag = false;
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        return false;
    }

    if (($("#hdBillingPin").val() == "0") && ($("#hdShippingPin").val() == "0")) {
        LoadingPanel.Hide();
        flag = false;

        jAlert('Please enter pin number.');
        page.SetActiveTabIndex(1);
        return false;
    }

    var Podt = cPLQuoteDate.GetValue();
    if (Podt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDate").show();
        flag = false;
        return false;
    }
    var invtype = $('#ddlInventory').val();
    if (invtype != 'N') {
        if (ctxt_partyInvNo.GetText() == '' || ctxt_partyInvNo.GetText() == null) {
            flag = false;
            $("#MandatorysPartyinvno").show();
            LoadingPanel.Hide();
            return false;
        }
    }
    var sdate = cdt_partyInvDt.GetValue();
    var edate = cPLQuoteDate.GetValue();
    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (invtype == 'N') {
        if (sdate == null || sdate == "") {
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');

            //if (startDate > endDate) {
            //    LoadingPanel.Hide();
            //    flag = false;
            //    $('#MandatoryEgSDate').attr('style', 'display:block');
            //}
            //else {
            //    $('#MandatoryEgSDate').attr('style', 'display:none');
            //    flag = true;
            //}

            $('#MandatoryEgSDate').attr('style', 'display:none');
            flag = true;
        }
    }
    else if (invtype != 'N') {
        if (sdate == null || sdate == "") {
            flag = false;
            $('#MandatoryPartyDate').attr('style', 'display:block');
            LoadingPanel.Hide();
            return false;
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');

            //if (startDate > endDate) {
            //    LoadingPanel.Hide();
            //    return false;
            //    flag = false;
            //    $('#MandatoryEgSDate').attr('style', 'display:block');
            //}
            //else {
            //    $('#MandatoryEgSDate').attr('style', 'display:none');
            //    flag = true;
            //}
            $('#MandatoryEgSDate').attr('style', 'display:none');
            flag = true;
        }
    }
    //var customerval = gridLookup.GetValue();
    var customerval = GetObjectID('hdnCustomerId').value;
    if (customerval == '' || customerval == null) {
        LoadingPanel.Hide();
        flag = false;
        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
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
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem == 'N' && cddl_TdsScheme.GetValue() != '0') {
                if ($('#hdnADDEditMode').val() != 'Edit') {
                    var tdschk = $('#hdntdschecking').val()
                    if (tdschk != '') {
                        LoadingPanel.Hide();
                        jAlert('Please enter TDS detail for Sl no.' + tdschk)
                        return false;
                    }
                }
            }
            if (inventoryItem == 'S' && cddl_TdsScheme.GetValue() != '0') {
                if ($('#hdnADDEditMode').val() != 'Edit') {
                    var tdschk = $('#hdntdschecking').val()
                    if (tdschk != '') {
                        LoadingPanel.Hide();
                        jAlert('Please enter TDS detail for Sl no.' + tdschk)
                        return false;
                    }
                }
            }

            if (grid.GetVisibleRowsOnPage() > 0) {
                //Subhra 18-03-2019
                if (issavePacking == 1 && aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "PurchaseInvoice.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var customerval = GetObjectID('hdnCustomerId').value;
                            $('#hdfLookupCustomer').val(customerval);
                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                    });
                }
                else {
                    //Subhra 18-03-2019
                    //var customerval = gridLookup.GetValue();
                    var customerval = GetObjectID('hdnCustomerId').value;
                    $('#hdfLookupCustomer').val(customerval);
                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    cacbpCrpUdf.PerformCallback();
                }
            }
            else {
                LoadingPanel.Hide();
                jAlert('Please add atleast single record first');
            }
        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}

function OnAddNewClick() {
    var inventoryItem = $('#ddlInventory').val();
    if (inventoryItem != 'N' && inventoryItem != 'S') {
        if (gridquotationLookup.GetValue() == null) {
            grid.AddNewRow();

            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
            // Mantis Issue 24429
            $("#UOMQuantity").val(0);
            Uomlength = 0;
            // End of Mantis Issue 24429
        }
    }
    else {
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        // Mantis Issue 24429
        $("#UOMQuantity").val(0);
        Uomlength = 0;
        // End of Mantis Issue 24429
    }
}
function ProductsCombo_SelectedIndexChanged(s, e) {

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbStockUOM = grid.GetEditor("gvColStockUOM");
    var tbPurchasePrice = grid.GetEditor("PurchasePrice");
    var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStockUOM = SpliteDetails[4];
    var strPurchasePrice = SpliteDetails[6];
    var strStockId = SpliteDetails[10];
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbStockUOM.SetValue(strStockUOM);
    tbPurchasePrice.SetValue(strPurchasePrice);
    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}

function ddl_AmountAre_valueChange() {
    var key = $("#ddl_AmountAre").val();
    if (key == 1) {
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('1');
    }
    else if (key == 2) {
        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');
    }
    else if (key == 3) {
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('3');
    }
}
function CmbBranch_ValueChange() {
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").val();
    var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
    if (type == 'PO' || type == 'PC') {
        selectValue();
    }
    else {
        ctxtVendorName.SetText('');
        GetObjectID('hdnCustomerId').value = '';
        //var vendorid = gridLookup.GetValue();
        //if(vendorid!=null && vendorid!='')
        //{
        //    gridLookup.PerformCallback('BlankVendor');
        //} 
    }

    var VendorId = $("#hdnCustomerId").val();
    var startDate = new Date();
    //startDate = cPLQuoteDate.GetDate().format('yyyy-MM-dd');
    //cPanelGRNOverheadCost.PerformCallback('BindOverheadCostGrid'+ startDate);
    //clookup_GRNOverhead.gridView.Refresh();

}

function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
}
function SetDifference() {
    var diff = CheckDifference();
    if (diff > 0) {
        clientResult.SetText(diff.toString());
    }
}
function CheckDifference() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLQuoteDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;

    }
    return difference;

}
//<%--   Warehouse  Script   --%>
function Keypressevt() {
    if (event.keyCode == 13) {
        //run code for Ctrl+X -- ie, Save & Exit! 
        SaveWarehouse();
        return false;
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
$(document).ready(function () {
    var val = $("#ddl_numberingScheme").val();
    //var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
    if (val == '0') {
        document.getElementById('txtVoucherNo').disabled = true;
    }
    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
        page.GetTabByName('Billing/Shipping').SetEnabled(false);
    }
    var isCtrl = false;
    document.onkeydown = function (e) {
        //if (event.keyCode == 78 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + n -- ie, Save & New  
        if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V" && $("#lbl_quotestatusmsg")[0].innerHTML == "") { //run code for Alt + s -- ie, Save & New  
            StopDefaultAction(e);
            Save_ButtonClick();
        }
        else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V" && $("#lbl_quotestatusmsg")[0].innerHTML == "") { //run code for Alt+X -- ie, Save & Exit!  
            //else if (event.keyCode == 69 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt+e -- ie, Save & Exit!     
            StopDefaultAction(e);
            SaveExit_ButtonClick();
        }
        else if (event.keyCode == 79 && event.altKey == true) { //run code for Alt+O -- ie, Billing/Shipping Samrat!     
            StopDefaultAction(e);
            if (page.GetActiveTabIndex() == 1) {
                fnSaveBillingShipping();
            }
        }
        else if (event.keyCode == 77 && event.altKey == true) { //run code for Alt+m -- ie, TC Sayan!
            $('#TermsConditionseModal').modal({
                show: 'true'
            });
        }
        else if (event.keyCode == 69 && event.altKey == true) { //run code for Alt+e -- ie, TC Sayan!
            if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
                StopDefaultAction(e);
                SaveTermsConditionData();
            }
        }
        else if (event.keyCode == 76 && event.altKey == true) { //run code for Alt+l -- ie, TC Sayan!
            StopDefaultAction(e);
            calcelbuttonclick();
        }
        else if (event.keyCode == 85 && event.altKey == true) {//run code for Alt+U -- ie,
            OpenUdf();
        }
        else if (event.keyCode == 84 && event.altKey == true) {//run code for Alt+T
            Save_TaxesClick();
        }
        else if (event.keyCode == 83 && event.altKey == true) { //run code for Alt+X -- ie, Save & Exit!     
            StopDefaultAction(e);
            if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                SaveVehicleControlData();
            }
        }
        else if (event.keyCode == 67 && event.altKey == true) { //run code for Alt+X -- ie, Save & Exit!     
            StopDefaultAction(e);
            modalShowHide(0);
        }
        else if (event.keyCode == 82 && event.altKey == true) { //run code for Alt+X -- ie, Save & Exit!     
            StopDefaultAction(e);
            $('body').on('shown.bs.modal', '#exampleModal', function () {
                $('input:visible:enabled:first', this).focus();
            })
        }
    }
});
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function DeleteWarehousebatchserial(SrlNo, BatchWarehouseID, viewQuantity, Quantity, WarehouseID, BatchNo) {
    var IsSerial = $('#hdnisserial').val();
    if (IsSerial == "true" && viewQuantity != "1.0000" && viewQuantity != "1.0" && viewQuantity != "") {
        jAlert("Cannot Proceed. You have to delete subsequent data first before delete this data.");
    } else {
        if (BatchWarehouseID == "" || BatchWarehouseID == "0") {
            $('#hdnisolddeleted').val("false");
            if (SrlNo != "") {
                cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
            }
        } else {
            $('#hdnisolddeleted').val("true");
            if (SrlNo != "") {
                cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
            }
        }
    }
}
function UpdateWarehousebatchserial(SrlNo, WarehouseID, BatchNo, SerialNo, isnew, viewQuantity, Quantity) {
    var Isbatch = $('#hdnisbatch').val();
    if (isnew == "old" || isnew == "Updated") {
        $('#hdnisoldupdate').val("true");
        $('#hdncurrentslno').val("");
        cCmbWarehouse.SetValue(WarehouseID);
        if (Quantity != null && Quantity != "" && Isbatch != "true") {
            ctxtqnty.SetText(Quantity);
        } else {
            ctxtqnty.SetText(viewQuantity);
        }
        var IsSerial = $('#hdnisserial').val();
        if (IsSerial == "true") {
            if (viewQuantity == "") {
                ctxtbatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
                ctxtqnty.SetEnabled(false);
                ctxtserial.Focus();
            } else {
                ctxtbatch.SetEnabled(true);
                cCmbWarehouse.SetEnabled(true);
                ctxtqnty.SetEnabled(true);
                ctxtserial.Focus();
            }
        }
        else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtbatch.Focus();
        }
        ctxtbatchqnty.SetText(Quantity);
        ctxtbatch.SetText(BatchNo);
        ctxtserial.SetText(SerialNo);
        if (viewQuantity == "") {
            ctxtbatch.SetEnabled(false);
            cCmbWarehouse.SetEnabled(false);
            $('#hdnisviewqntityhas').val("true");
        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            $('#hdnisviewqntityhas').val("false");
        }
        var hdniswarehouse = $('#hdniswarehouse').val();
        if (hdniswarehouse != "true" && Isbatch == "true") {
            ctxtbatchqnty.SetText(viewQuantity);
            ctxtbatchqnty.Focus();
        } else {
            ctxtqnty.Focus();
        }
        $('#hdncurrentslno').val(SrlNo);
    } else {
        $('#hdnisoldupdate').val("false");
        ctxtqnty.SetText("0.0");
        ctxtqnty.SetEnabled(true);
        ctxtbatchqnty.SetText("0.0");
        ctxtserial.SetText("");
        ctxtbatchqnty.SetText("");
        $('#hdncurrentslno').val("");
        $('#hdnisnewupdate').val("true");
        $('#hdncurrentslno').val("");
        cCmbWarehouse.SetValue(WarehouseID);
        if (Quantity != null && Quantity != "" && Isbatch != "true") {
            ctxtqnty.SetText(Quantity);
        } else {
            ctxtqnty.SetText(viewQuantity);
        }
        var IsSerial = $('#hdnisserial').val();
        if (IsSerial == "true") {
            if (viewQuantity == "") {
                ctxtbatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
                ctxtqnty.SetEnabled(false);
                $('#hdnisviewqntityhas').val("true");
                ctxtserial.Focus();
            } else {
                ctxtbatch.SetEnabled(true);
                cCmbWarehouse.SetEnabled(true);
                ctxtqnty.SetEnabled(true);
                $('#hdnisviewqntityhas').val("false");
                ctxtserial.Focus();
            }
        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtbatch.Focus();
        }
        ctxtbatchqnty.SetText(Quantity);
        ctxtbatch.SetText(BatchNo);
        ctxtserial.SetText(SerialNo);
        if (viewQuantity == "") {
            ctxtbatch.SetEnabled(false);
            cCmbWarehouse.SetEnabled(false);
        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
        }
        var hdniswarehouse = $('#hdniswarehouse').val();
        if (hdniswarehouse != "true" && Isbatch == "true") {
            ctxtbatchqnty.SetText(viewQuantity);
        } else {
            ctxtqnty.Focus();
        }
        $('#hdncurrentslno').val(SrlNo);
    }
}
function changedqnty(s) {
}
function endcallcmware(s) {
    if (cCmbWarehouse.cpstock != null) {
        var ddd = cCmbWarehouse.cpstock + " " + $('#hdnstrUOM').val();
        document.getElementById('lblAvailableStk').innerHTML = ddd;
        cCmbWarehouse.cpstock = null;
    }
}
function changedqntybatch(s) {
    var qnty = s.GetText();
    var sum = $('#hdntotalqntyPC').val();
    sum = Number(Number(sum) + Number(qnty));
    $('#hdntotalqntyPC').val(sum);
}
function chnagedbtach(s) {
    $('#hdnoldbatchno').val(s.GetText());
    $('#hidencountforserial').val(1);
    var sum = $('#hdnbatchchanged').val();
    sum = Number(Number(sum) + Number(1));
    $('#hdnbatchchanged').val(sum);
    ctxtexpirdate.SetText("");
    ctxtmkgdate.SetText("");
}
function CmbWarehouse_ValueChange(s) {
    var ISupdate = $('#hdnisoldupdate').val();
    var isnewupdate = $('#hdnisnewupdate').val();
    $('#hdnoldwarehousname').val(s.GetText());
    if (ISupdate == "true" || isnewupdate == "true") {
    } else {
        ctxtserial.SetValue("");
        ctxtbatch.SetEnabled(true);
        ctxtexpirdate.SetEnabled(true);
        ctxtmkgdate.SetEnabled(true);
    }
}
function Clraear() {
    ctxtbatch.SetValue("");
    ASPx.CalClearClick('txtmkgdate_DDD_C');
    ASPx.CalClearClick('txtexpirdate_DDD_C');
    $('#hdnisoldupdate').val("false");
    ctxtserial.SetValue("");
    ctxtqnty.SetValue("0.0000");
    ctxtbatchqnty.SetValue("0.0000");
    $('#hdntotalqntyPC').val(0);
    $('#hidencountforserial').val(1);
    $('#hdnbatchchanged').val("0");
    var strProductID = $('#hdfProductIDPC').val();
    var stockids = $('#hdfstockidPC').val();
    var branchid = $('#hdbranchIDPC').val();
    var strProductName = $('#lblProductName').text();
    $('#hdnisnewupdate').val("false");
    ctxtbatch.SetEnabled(true);
    ctxtexpirdate.SetEnabled(true);
    ctxtmkgdate.SetEnabled(true);
    ctxtbatch.SetEnabled(true);
    cCmbWarehouse.SetEnabled(true);
    $('#hdnisviewqntityhas').val("false");
    $('#hdnisolddeleted').val("false");
    ctxtqnty.SetEnabled(true);
    var existingqntity = $('#hdfopeningstockPC').val();
    var totaldeleteqnt = $('#hdndeleteqnity').val();
    var addqntity = Number(existingqntity) + Number(totaldeleteqnt);
    $('#hdndeleteqnity').val(0);
    cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
}
function SaveWarehouse() {
    if ($('#wbsqtychecking').val() == '1') {
        var qnty = ctxtqnty.GetText();
        var sum = $('#hdntotalqntyPC').val();
        sum = Number(Number(sum) + Number(qnty));
        $('#hdntotalqntyPC').val(sum);
        ctxtqnty.SetEnabled(false);
        $('#wbsqtychecking').val('0');
    }
    var prosrlno = $('#hdfProductSerialID').val();
    var WarehouseID = cCmbWarehouse.GetValue();
    var WarehouseName = cCmbWarehouse.GetText();
    var qnty = ctxtqnty.GetText();
    var IsSerial = $('#hdnisserial').val();
    if (qnty == "0.0000") {
        qnty = ctxtbatchqnty.GetText();
    }
    if (Number(qnty) % 1 != 0 && IsSerial == "true") {
        jAlert("Serial number is activated, Quantity should not contain decimals. ");
        return;
    }
    var BatchName = ctxtbatch.GetText();
    var SerialName = ctxtserial.GetText();
    var Isbatch = $('#hdnisbatch').val();
    var enterdqntity = $('#hdfopeningstockPC').val();
    var hdniswarehouse = $('#hdniswarehouse').val();
    var ISupdate = $('#hdnisoldupdate').val();
    var isnewupdate = $('#hdnisnewupdate').val();
    if (Isbatch == "true" && hdniswarehouse == "false") {
        qnty = ctxtbatchqnty.GetText();
    }
    if (ISupdate == "true") {
        if (hdniswarehouse == "true" && WarehouseID == null) {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
        }
        else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {
            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {
            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }
        if (Isbatch == "true" && BatchName == "") {
            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
        } else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {
            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();
        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        var slno = $('#hdncurrentslno').val();
        if (slno != "") {
            cGrdWarehousePC.PerformCallback('Updatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty + '~' + prosrlno);
            return false;
        }
    } else if (isnewupdate == "true") {
        if (hdniswarehouse == "true" && WarehouseID == null) {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
        }
        else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {
            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {
            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }
        if (Isbatch == "true" && BatchName == "") {
            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
        }
        else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {
            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();
        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        var slno = $('#hdncurrentslno').val();
        if (slno != "") {
            cGrdWarehousePC.PerformCallback('NewUpdatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty + '~' + prosrlno);
            $('#hdnisviewqntityhas').val("false");
            $('#hdnisnewupdate').val("false");
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtqnty.SetText("0.0");
            ctxtbatch.SetText("");
            return false;
        }
    }
    else {
        var hdnisediteds = $('#hdnisedited').val();
        if (hdniswarehouse == "true" && WarehouseID == null) {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");

            return;
        } else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {

            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {
            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }
        if (Isbatch == "true" && BatchName == "") {

            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
            return;

        } else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {
            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();
            return;
        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        if (Isbatch == "true" && hdniswarehouse == "false") {
            qnty = ctxtbatchqnty.GetText();
            if (qnty == "0.0000") {
                ctxtbatchqnty.Focus();
            }
        }
        if (qnty == "0.0") {
            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        }
        else if (((hdniswarehouse == "true" && WarehouseID != null) || hdniswarehouse == "false") && ((Isbatch == "true" && BatchName != "") || Isbatch == "false") && ((IsSerial == "true" && SerialName != "") || IsSerial == "false") && qnty != "0.0") {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
            $("#RequiredFieldValidatortxtserial").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").removeAttr("style");
            $("#RequiredFieldValidatortxtbatchqntity").removeAttr("style");
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            if (Isbatch == "true" && hdniswarehouse == "false") {
                qnty = ctxtbatchqnty.GetText();
                if (qnty = "0.0000") {
                    ctxtbatchqnty.Focus();
                }
            }
            var oldenterqntity = $('#hdnenterdopenqnty').val();
            var enterdqntityss = $('#hdnnewenterqntity').val();
            var deletedquantity = $('#hdndeleteqnity').val();
            if (Number(qnty) > (Number(enterdqntity) + Number(deletedquantity)) && hdnisediteds == "false") {
                qnty = "0.00";
                jAlert("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
            }
            else {
                cGrdWarehousePC.PerformCallback('Display~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + qnty + '~' + prosrlno);
                cCmbWarehouse.Focus();
            }
        }
        return false;
    }
}
function SaveWarehouseAll() {
    cGrdWarehousePC.PerformCallback('Saveall~');
}
function cGrdWarehousePCShowError(obj) {
    if (cGrdWarehousePC.cperrorMsg != null && cGrdWarehousePC.cperrorMsg != undefined) {
        jAlert(cGrdWarehousePC.cperrorMsg);
        ctxtserial.Focus();
        return;
    }
    if (cGrdWarehousePC.cpdeletedata != null) {
        var existingqntity = $('#hdfopeningstockPC').val();
        var totaldeleteqnt = $('#hdndeleteqnity').val();
        var addqntity = Number(cGrdWarehousePC.cpdeletedata) + Number(existingqntity);
        var adddeleteqnty = Number(cGrdWarehousePC.cpdeletedata) + Number(totaldeleteqnt);
        $('#hdndeleteqnity').val(adddeleteqnty);
        cGrdWarehousePC.cpdeletedata = null;
    }
    if (cGrdWarehousePC.cpdeletedatasubsequent != null) {
        jAlert(cGrdWarehousePC.cpdeletedatasubsequent);
        cGrdWarehousePC.cpdeletedatasubsequent = null;
    }
    if (cGrdWarehousePC.cpbatchinsertmssg != null) {
        ctxtbatch.SetText("");

        ctxtqnty.SetValue("0.0000");
        ctxtbatchqnty.SetValue("0.0000");
        cGrdWarehousePC.cpbatchinsertmssg = null;
    }
    if (cGrdWarehousePC.cpupdateexistingdata != null) {

        $('#hdnisedited').val("true");
        cGrdWarehousePC.cpupdateexistingdata = null;
    }
    if (cGrdWarehousePC.cpupdatenewdata != null) {

        $('#hdnisedited').val("true");

        cGrdWarehousePC.cpupdateexistingdata = null;
    }
    if (cGrdWarehousePC.cpupdatemssgserialsetdisblebatch != null) {
        ctxtbatch.SetEnabled(false);
        ctxtexpirdate.SetEnabled(false);
        ctxtmkgdate.SetEnabled(false);
        cGrdWarehousePC.cpupdatemssgserialsetdisblebatch = null;
    }
    if (cGrdWarehousePC.cpupdatemssgserialsetenablebatch != null) {
        ctxtbatch.SetEnabled(true);
        ctxtexpirdate.SetEnabled(true);
        ctxtmkgdate.SetEnabled(true);
        $('#hidencountforserial').val(1);
        //New Code Added by Sam
        ctxtqnty.SetEnabled(true)
        $('#wbsqtychecking').val('1')
        //New Code Added by Sam
        $('#hdnbatchchanged').val("0");
        $('#hidencountforserial').val("1");
        ctxtqnty.SetValue("0.0000");
        ctxtbatchqnty.SetValue("0.0000");
        ctxtbatch.SetText("");
        cGrdWarehousePC.cpupdatemssgserialsetenablebatch = null;
    }
    if (cGrdWarehousePC.cpproductname != null) {
        document.getElementById('lblpro').innerHTML = cGrdWarehousePC.cpproductname;
        cGrdWarehousePC.cpproductname = null;
    }
    if (cGrdWarehousePC.cpupdatemssg != null) {
        if (cGrdWarehousePC.cpupdatemssg == "Saved Successfully.") {
            $('#hdntotalqntyPC').val("0");
            $('#hdnbatchchanged').val("0");
            $('#hidencountforserial').val("1");
            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            parent.cPopup_WarehousePC.Hide();
            var hdnselectedbranch = $('#hdnselectedbranch').val();
            var selectedrow = $('#hdngridvselectedrowno').val();
            grid.batchEditApi.StartEdit(selectedrow, 8);
        } else {
            jAlert(cGrdWarehousePC.cpupdatemssg);
        }
        cGrdWarehousePC.cpupdatemssg = null;
    }
    if (cGrdWarehousePC.cpupdatemssgserial != null) {
        jAlert(cGrdWarehousePC.cpupdatemssgserial);
        cGrdWarehousePC.cpupdatemssgserial = null;
    }
    if (cGrdWarehousePC.cpinsertmssg != null) {
        $('#hidencountforserial').val(2);
        ctxtserial.SetValue("");
        ctxtserial.Focus();
        cGrdWarehousePC.cpinsertmssg = null;
    }
    if (cGrdWarehousePC.cpinsertmssgserial != null) {
        ctxtserial.SetValue("");
        ctxtserial.Focus();
        cGrdWarehousePC.cpinsertmssgserial = null;
    }
}
function Onddl_VatGstCstEndCallback(s, e) {
    if (s.GetItemCount() == 1) {
        cddlVatGstCst.SetEnabled(false);
    }
}

//<%--   Warehouse Script End    --%>

//<%--Sam Section For extra Modification and tagging Section Start--%>

$(document).ready(function () {
    $('#ApprovalCross').click(function () {

        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.Refresh()();
    })
})

function selectValue(  ) {
    var startDate = new Date();
    startDate = cPLQuoteDate.GetValueString();
    //var key = gridLookup.GetValue()
    var key = GetObjectID('hdnCustomerId').value;
    if (key != null && key != '') {
        $('#hdnTaggedVender').val(key);

        $('#hdnTaggedVendorName').val(ctxtVendorName.GetText());
    }
    else {
        $("table[id$=rdl_PurchaseInvoice] input:radio:checked").removeAttr("checked");
        jAlert('Select a Vendor first');
        return;
    }
    var invtype = $('#ddlInventory').val();
    var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
    var schemaid = $('#ddl_numberingScheme').val();
    if (schemaid != '0') {
        page.tabs[1].SetEnabled(false);
        if (type == "PO") {
            clbl_InvoiceNO.SetText('Purchase Order Date');
            $("#ddlInventory").attr("disabled", true);
            if (invtype == 'Y') {
                grid.GetEditor('ProductName').SetEnabled(false);
            }
        }
        else if (type == "PC") {

            clbl_InvoiceNO.SetText('GRN Date');
            $("#ddlInventory").attr("disabled", true);
            $("#ddl_numberingScheme").attr("disabled", true);
            if (invtype == 'Y' || invtype == 'B' || invtype == 'C') {
                grid.GetEditor('ProductName').SetEnabled(false);
            }
        }
        else {
            $("#ddlInventory").attr("disabled", true);
            $("#ddl_numberingScheme").attr("disabled", false);
            grid.GetEditor('ProductName').SetEnabled(true);
        }
        if (key != null && key != '' && type != "") {
            cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
        }
        var componentType = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
        if (componentType != null && componentType != '') {
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            if (noofvisiblerows != '1') {

                grid.PerformCallback('GridBlank');
            }
        }
    }
}

//<%--Sam Section For extra Modification Section End--%>

//<%--Added By : Samrat Roy -- New Billing/Shipping Section--%>

function SettingTabStatus() {
    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
    }
}

function disp_prompt(name) {
    if (name == "tab0") {
        page.GetTabByName('Billing/Shipping').SetEnabled(true);
        page.GetTabByName('General').SetEnabled(false);
        ctxt_partyInvNo.Focus();
        $("#crossdiv").show();
    }
    if (name == "tab1") {

        $("#crossdiv").hide();
        var custID = GetObjectID('hdnCustomerId').value;
        if (custID == null && custID == '') {
            jAlert('Please select a customer');
            page.SetActiveTabIndex(0);

            return;
        }
        else {
            page.SetActiveTabIndex(1);
            page.tabs[0].SetEnabled(false);
        }
    }
}

$(document).ready(function () {



    if ($("#hdnADDEditMode").val() != "ADD") {
        var dt = new Date();
        if ($("#hdnBackdateddate").val() != "0" && $("#hdnBackdateddate").val() != "") {
            cPLQuoteDate.SetEnabled(true)
            var Days = $("#hdnBackdateddate").val();
            var today = cPLQuoteDate.GetDate();
            var newdate = cPLQuoteDate.GetDate();
            newdate.setDate(today.getDate() - Math.round(Days));
            if ($("#hdnTagDateForbackdated").val() != "") {
                if (new Date($("#hdnTagDateForbackdated").val()) > newdate) {
                    cPLQuoteDate.SetMinDate(new Date($("#hdnTagDateForbackdated").val()));
                    cPLQuoteDate.SetMaxDate(dt)
                }
                else {
                    cPLQuoteDate.SetMinDate(newdate);
                    cPLQuoteDate.SetMaxDate(dt);
                }
            }
            else {
                cPLQuoteDate.SetMinDate(newdate);
                cPLQuoteDate.SetMaxDate(dt);
            }
        }
    }

    //Toggle fullscreen expandEntryGrid
    $("#expandgrid").click(function (e) {
        e.preventDefault();
        var $this = $(this);
        if ($this.children('i').hasClass('fa-expand')) {
            $this.removeClass('hovered half').addClass('full');
            $this.attr('title', 'Minimize Grid');
            $this.children('i').removeClass('fa-expand');
            $this.children('i').addClass('fa-arrows-alt');
            var gridId = $(this).attr('data-instance');
            $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
            var cntWidth = $(this).parent('.makeFullscreen').width();
            var browserHeight = document.documentElement.clientHeight;
            var browserWidth = document.documentElement.clientWidth;
            grid.SetHeight(browserHeight - 150);
            grid.SetWidth(cntWidth);
        }
        else if ($this.children('i').hasClass('fa-arrows-alt')) {
            $this.children('i').removeClass('fa-arrows-alt');
            $this.removeClass('full').addClass('hovered half');
            $this.attr('title', 'Maximize Grid');
            $this.children('i').addClass('fa-expand');
            var gridId = $(this).attr('data-instance');
            $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');
            var browserHeight = document.documentElement.clientHeight;
            var browserWidth = document.documentElement.clientWidth;
            grid.SetHeight(300);
            var cntWidth = $this.parent('.makeFullscreen').width();
            grid.SetWidth(cntWidth);
        }
    });
});


//Hierarchy Start Tanmoy
function clookup_Project_LostFocus() {
    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}

function ProjectValueChange(s, e) {
    //
    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'PurchaseInvoice.aspx/getHierarchyID',
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

$(document).ready(function () {
    if ($('body').hasClass('mini-navbar')) {
        var windowWidth = $(window).width();
        var cntWidth = windowWidth - 90;
        grid.SetWidth(cntWidth);
    } else {
        var windowWidth = $(window).width();
        var cntWidth = windowWidth - 220;
        grid.SetWidth(cntWidth);
    }

    $('.navbar-minimalize').click(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            grid.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            grid.SetWidth(cntWidth);
        }

    });
});

function Project_gotFocus() {
    if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.gridView.Refresh();
    clookup_Project.ShowDropDown();
}

function OverheadCost_gotFocus() {
    //clookup_GRNOverhead.gridView.Refresh();
    //var startDate = new Date();
    //startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
    //cPanelGRNOverheadCost.PerformCallback('BindOverheadCostGrid' + '~' + startDate );
    // clookup_GRNOverhead.gridView.Refresh();

    //clookup_GRNOverhead.ShowDropDown();
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
function ShowTCS() {

    $("#tcsModal").modal('show');
}

function CalcTCSAmount() {
    var applAmt = parseFloat(ctxtTCSapplAmount.GetText())
    var perAmt = parseFloat(ctxtTCSpercentage.GetText())

    var tcsAmount = DecimalRoundoff(parseFloat(applAmt) * parseFloat(perAmt) * 0.01, 2);
    ctxtTCSAmount.SetValue(tcsAmount);
    // Mantis Issue 24641
    if (tcsAmount != null) {
        var TotalAmount = (parseFloat((cInvValue.GetValue()).toString())).toFixed(2);
        var ChargesAmount = (ctxt_Charges.GetValue() != null) ? ((parseFloat(ctxt_Charges.GetValue()) + tcsAmount)).toFixed(2) : "0";
        var Calculate_TotalAmount = (parseFloat(TotalAmount) + parseFloat(Cur_TotalAmt) - parseFloat(Pre_TotalAmt)).toFixed(2);
        var Calculate_SumAmount = (parseFloat(Calculate_TotalAmount) + parseFloat(ChargesAmount) ).toFixed(2);
        
        cTotalAmt.SetValue(Calculate_SumAmount);
        cOtherTaxAmtval.SetValue(ChargesAmount);
    }
    /// End of Mantis Issue 24641
}


function ShowTDS() {
    

    var count = grid.GetVisibleRowsOnPage();
    var totalAmount = 0;
    var totaltxAmount = 0;
    var totalQuantity = 0;
    var netAmount = 0;
    //dev Bapi
    var vendortype = cddl_vendortype.GetValue();
    //end dev Bapi

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

                netAmount = netAmount + DecimalRoundoff(grid.GetEditor("TotalAmount").GetValue(), 2);
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
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2), 2))

                }
                else {
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                }
                netAmount = netAmount + DecimalRoundoff(grid.GetEditor("TotalAmount").GetValue(), 2);

            }
        }
    }




    var CustomerId = $("#hdnCustomerId").val();
    var invoice_id = "";
    // Mantis Issue 24432
    if ($("#hdnADDEditMode").val() == 'Edit') {
        invoice_id = $("#hdnPageEditId").val();
    }
    // End of Mantis Issue 24432
   
    var date = cPLQuoteDate.GetText();
    
    var obj = {};
    obj.CustomerId = CustomerId;
    // Mantis Issue 24432
    //obj.invoice_id = "";
    obj.invoice_id = invoice_id;
    // End of Mantis Issue 24432
    obj.date = date;
    obj.totalAmount = netAmount;
    obj.taxableAmount = totalAmount;
    obj.branch_id = $("#ddl_Branch").val();
    obj.tds_code = ctxtTDSSection.GetValue();
    obj.vendortype = vendortype;
    if (invoice_id == "" || invoice_id == null) {
        $.ajax({
            type: "POST",
            url: 'PurchaseInvoice.aspx/getTDSDetails',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(obj),
            success: function (msg) {

                if (msg) {
                    var response = msg.d;
                    
                    ctxtTDSapplAmount.SetText(response.tds_amount);
                    ctxtTDSpercentage.SetText(response.Rate);
                    ctxtTDSAmount.SetText(response.Amount);
                    cGridTDSdocs.PerformCallback();
                }


            }
        });
    }
    else {
        cGridTDSdocs.PerformCallback();
    }



    $("#tdsModal").modal('show');
}

function CalcTDSAmount() {
    var applAmt = parseFloat(ctxtTDSapplAmount.GetText())
    var perAmt = parseFloat(ctxtTDSpercentage.GetText())

    var tcsAmount = DecimalRoundoff(parseFloat(applAmt) * parseFloat(perAmt) * 0.01, 2);
    ctxtTDSAmount.SetValue(tcsAmount);

}

function TDSsectionchanged(s,e)
{
    ShowTDS();
}