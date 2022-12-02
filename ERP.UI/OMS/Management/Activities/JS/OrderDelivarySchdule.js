function MultiUom() {
    //debugger;

    //var StockType = GetObjectID('hdfWarehousetype').value;
    //var ProductSrlNo = GetObjectID('hdfProductSrlNo').value;
    //var UOM = GetObjectID('hdfUOM').value;
    // var ServiceURL = GetObjectID('hdfServiceURL').value;
    // var Branch = GetObjectID('hdfBranch').value;

    //  var WarehouseID = $('#ddlWarehouse').val();
    //  var WarehouseName = $("option:selected", '#ddlWarehouse').text();
    //   var Batch = $("#txtBatch").val().trim();
    // var Qty = ctxtQty.GetValue();
    //  var MfgDate = (ctxtMfgDate.GetValue() != null) ? ctxtMfgDate.GetValue() : "";
    //  var ExprieyDate = (ctxtExprieyDate.GetValue() != null) ? ctxtExprieyDate.GetValue() : "";
    //var Serial = $("#txtSerial").val().trim();
    //var Rate = ctxtRate.GetValue();
    //var Value = ctxtvalue.GetValue();//Rajdip 
    //var AlterQty = cAltertxtQty1.GetValue();
    //var AltUOM = ccmbPackingUom1.GetValue();
    //var AltUOMName = ccmbPackingUom1.GetText();


    var ProductID = GetObjectID('hdnProductId').value;
    var UOMName = ctxtUOM.GetText();
     var quantity = 1;
     var StockUOM = hdnUOMId.value;


    cAltUOMQuantity.SetValue("0.0000");


      if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
     if (StockUOM == "0") {
            jAlert("Main Unit Not Defined.");
      }
    else {
          ccmbUOM.SetEnabled(false);

   cgrid_MultiUOM.cpDuplicateAltUOM = "";
   var Qnty = 1;
      // ctxtQty.GetValue();
   var SrlNo = 1;
       //(GetObjectID('hdfProductSrlNo').value != null) ? GetObjectID('hdfProductSrlNo').value : "";
        var UomId = hdnUOMId.value;
         ccmbUOM.SetValue(UomId);

         $("#UOMQuantity").val("0.0000");
            ccmbBaseRate.SetValue(0)
            cAltUOMQuantity.SetValue(0)
            ccmbAltRate.SetValue(0)
            ccmbSecondUOM.SetValue("")
            // End of Rev Sanchita

            cPopup_MultiUOM.Show();

       AutoPopulateMultiUOM();

      cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo);

      }
     }
    //var SrlNo = 1;
    //cPopup_MultiUOM.Show();
    //AutoPopulateMultiUOM();
    //cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo);

}


var PacQty = 0.00;
var ProQty = 0;
function AutoPopulateMultiUOM() {
    //debugger;
    var ProductID = $("#hdnProductId").val();
    var QuantityValue = (ctxtDeliveryQuantity.GetValue() != null) ? ctxtDeliveryQuantity.GetValue() : "0";

    $.ajax({
        type: "POST",
        url: "OrderDeliveryScheduleAdd.aspx/AutoPopulateAltQuantity",
        data: JSON.stringify({ ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            if (msg.d.length != 0) {
                var packingQuantity = msg.d[0].packing_quantity;
                PacQty = packingQuantity;
                var sProduct_quantity = msg.d[0].sProduct_quantity;
                var AltUOMId = msg.d[0].AltUOMId;
                QuantityValue = sProduct_quantity;
                ProQty = sProduct_quantity;
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
            ccmbSecondUOM.SetValue(AltUOMId);
            if ($("#hdnDeliveryScheduleDetailsId").val() != "") {
               
                cAltUOMQuantity.SetValue("0.0000");
            }
            else {
                if (AltUOMId == 0) {
                    ccmbSecondUOM.SetValue('');
                }
            }
        }
    });
}
//Mantis 24428
function SaveMultiUOM() {
    //debugger;
    var qnty = $("#UOMQuantity").val();
    var UomId = ccmbUOM.GetValue();
    var UomName = ccmbUOM.GetText();
    var AltQnty = cAltUOMQuantity.GetValue();
    var AltUomId = ccmbSecondUOM.GetValue();
    var AltUomName = ccmbSecondUOM.GetText();
    var srlNo = "1";
    var ProductID = $("#hdnProductId").val();
    var BaseRate = ccmbBaseRate.GetValue();
    var AltRate = ccmbAltRate.GetValue();
    var DetailsID = $("#hdnDeliveryScheduleDetailsId").val();
    //Mantis 24428
    if (ProductID == "") {
        ProductID = hdProductID.value;
    }
    //End Mantis 24428
    var UpdateRow = 'False';
    if ($("#chkUpdateRow").prop("checked")) {
        UpdateRow = 'True';
    }
    // Mantis Issue 24428
    // Mantis Issue 24428
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != ""
    //        && BaseRate != "0.0000" && AltRate != "0.0000") {
    if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != ""  && AltUomId != null && AltUomName != "" && AltQnty != "0.0000") {
        //End Mantis 24428
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
        if (cbtnMUltiUOM.GetText() == "Update") {
            cgrid_MultiUOM.PerformCallback('UpdateRow~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~'+DetailsID + '~'  + BaseRate + '~' + AltRate + '~' + UpdateRow + '~' + hdMultiUOMID);

            cAltUOMQuantity.SetValue("0.0000");
            $("#UOMQuantity").val("0.0000");
            ccmbBaseRate.SetValue(0);
            cAltUOMQuantity.SetValue(0);
            ccmbAltRate.SetValue(0);
            ccmbSecondUOM.SetValue("");
            cgrid_MultiUOM.cpAllDetails = "";
            cbtnMUltiUOM.SetText("Add");
            // Mantis Issue 24428
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
            //End Mantis 24428
        }
        else {
            cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsID + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow);
            cAltUOMQuantity.SetValue("0.0000");

            $("#UOMQuantity").val(0);
            ccmbBaseRate.SetValue(0)
            cAltUOMQuantity.SetValue(0)
            ccmbAltRate.SetValue(0)
            ccmbSecondUOM.SetValue("");
            // Mantis Issue 24428
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
            //End Mantis 24428

        }

        }
        else {
            return;
        }
       
    }
    else {
        return;
    }
}

//End Mantis 24428






function CalcBaseQty() {
    //debugger;
    //Mantis 24428
    var PackingQtyAlt = PacQty;
    var PackingQty = ProQty;
    //debugger;
    //if (PackingQtyAlt == "") {
    //    PackingQtyAlt = 0
    //}
    //if (PackingQty == "") {
    //    PackingQty = 0
    //}
    //var BaseQty = 0
    //if (PackingQtyAlt > 0) {
    //    var ConvFact = PackingQty / PackingQtyAlt;
    //    var altQty = cAltUOMQuantity.GetValue();
    //    if (ConvFact > 0) {
    //        var BaseQty = altQty * ConvFact;
    //        $("#UOMQuantity").val(BaseQty);
    //    }
    //}
    //else {
    //    $("#UOMQuantity").val(PackingQty);
    //}

    //  var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var PackingQtyAlt = 0;
    var PackingQty = 0;
    var PackingSaleUOM = 0;


    var ProductID = $("#hdnProductId").val();
    //Productdetails.split("||@||")[0];


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





    //End Mantis 24428
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

var hdmultiuser = "";
// Mantis Issue 24428
function Edit_MultiUom(keyValue, SrlNo) {

    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);



}
// End of Mantis Issue 24428

function Delete_MultiUom(keyValue, SrlNo) {
    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo);
}
function OnMultiUOMEndCallback(s, e) {
    //debugger;
    if (cgrid_MultiUOM.cpDuplicateAltUOM == "DuplicateAltUOM") {
        jAlert("Please Enter Different Alt. Quantity.");
        return;
    }
    if (cgrid_MultiUOM.cpSetBaseQtyRateInGrid != null && cgrid_MultiUOM.cpSetBaseQtyRateInGrid == "1") {
        var BaseQty = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
        var BaseRate = cgrid_MultiUOM.cpBaseRate;
        var AltQuantity = (cgrid_MultiUOM.cpAltQuantity).toFixed(4);
       
        ctxtDeliveryQuantity.SetValue(BaseQty);
      
        cAltertxtQty1.SetValue(AltQuantity);
        ccmbPackingUom2.SetValue(cgrid_MultiUOM.cpAltUOM);
        // ctxtRate.SetValue(BaseRate);
        // ctxtvalue.SetValue(BaseQty * BaseRate * 1.00)

        $('#hdnUOMpacking').val(cgrid_MultiUOM.cpAltQuantity);
        $('#hdnUOMPackingSelectUom').val(cgrid_MultiUOM.cpAltUomId);
        //dueLostFocus();

    }

    // Mantis Issue 24428
    if (cgrid_MultiUOM.cpAllDetails == "EditData") {
        var Quan = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
        $('#UOMQuantity').val(Quan);
       // $('#UOMQuantity').val(cgrid_MultiUOM.cpBaseQty);
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