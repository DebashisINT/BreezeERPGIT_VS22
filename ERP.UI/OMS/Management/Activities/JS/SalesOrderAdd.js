
$(function () {
    $('#UOMModal').on('hide.bs.modal', function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 8);
    });
});

$(document).ready(function () {
    var mode = $('#hdAddOrEdit').val();
    if (mode == 'Edit') {
        if ($("#hdAddOrEdit").val() != "") {
            var VendorID = $("#hdnCustomerId").val();
            SetEntityType(VendorID);

            if (gridquotationLookup.GetValue() == null) {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            }

            if($('#hdnDocumentSegmentSettings').val()=="1")
            {

                $.ajax({
                    type: "POST",
                    url: "SalesOrderAdd.aspx/GetSegmentDetails",
                    data: JSON.stringify({ CustomerId: VendorID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async:false,
                    success: function (msg) {
                        OutStandingAmount = msg.d;
                        if (OutStandingAmount != null)
                        {
                            if (OutStandingAmount.Segment1 != "") {
                                var Segment1 = OutStandingAmount.Segment1;
                                var Segment2 = OutStandingAmount.Segment2;
                                var Segment3 = OutStandingAmount.Segment3;
                                var Segment4 = OutStandingAmount.Segment4;
                                var Segment5 = OutStandingAmount.Segment5;

                                if(Segment1=="0")
                                {
                                    var div = document.getElementById('DivSegment1');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment1').val("0");
                                }
                                else{
                                    $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                    $('#hdnValueSegment1').val("1");
                                }
                                if(Segment2=="0")
                                {
                                    var div = document.getElementById('DivSegment2');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment2').val("0");
                                }
                                else{
                                    $('#ModuleSegment2Header').text(OutStandingAmount.SegmentName2);
                                    $('#hdnValueSegment2').val("1");
                                }

                                if(Segment3=="0")
                                {
                                    var div = document.getElementById('DivSegment3');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment3').val("0");
                                }
                                else{
                                    $('#ModuleSegment3Header').text(OutStandingAmount.SegmentName3);
                                    $('#hdnValueSegment3').val("1");
                                }

                                if(Segment4=="0")
                                {
                                    var div = document.getElementById('DivSegment4');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment4').val("0");
                                }
                                else{
                                    $('#ModuleSegment4Header').text(OutStandingAmount.SegmentName4);
                                    $('#hdnValueSegment4').val("1");
                                }

                                if(Segment5=="0")
                                {
                                    var div = document.getElementById('DivSegment5');
                                    div.style.display = 'none';
                                    $('#hdnValueSegment5').val("0");
                                }
                                else{
                                    $('#ModuleSegment5Header').text(OutStandingAmount.SegmentName5);
                                    $('#hdnValueSegment5').val("1");
                                }                  
                            }

                        }
                        else{
                            
                            document.getElementById('DivSegment1').style.display = 'none';
                            document.getElementById('DivSegment2').style.display = 'none';
                            document.getElementById('DivSegment3').style.display = 'none';
                            document.getElementById('DivSegment4').style.display = 'none';
                            document.getElementById('DivSegment5').style.display = 'none';
                        }
                    }
                });
            }
        }
    }
});

function TaxDeleteForShipPartyChange()
{
    var UniqueVal = $("#hdnGuid").val();
    $.ajax({
        type: "POST",
        url: "SalesOrderAdd.aspx/DeleteTaxForShipPartyChange",
        data: JSON.stringify({ UniqueVal: UniqueVal }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            // RequiredShipToPartyValue = msg.d;
        }
    });
}

function validateOrderwithAmountAre(){
    //Check Multiple Row amount are selectedor not

    var selectedKeys = cgridproducts.GetSelectedKeysOnPage();
    var ammountsAreOrder="";
    if(selectedKeys.length>0){
        for(var loopcount = 0 ; loopcount<cgridproducts.GetVisibleRowsOnPage();loopcount++)
        {
           
            var nowselectedKey = cgridproducts.GetRowKey(loopcount);

            var found = selectedKeys.find(function(element) {
                return element == nowselectedKey;
            });

            if(found){
                if(ammountsAreOrder !="" && ammountsAreOrder !=cgridproducts.GetRow(loopcount).children[9].innerText){
                    jAlert("Unable to procceed. Tax Type are for the selected order(s) are different");
                    return false;
                }
                else
                    ammountsAreOrder= cgridproducts.GetRow(loopcount).children[9].innerText;
            }
           
        }
   
    }

    return true;
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
        url: "SalesOrderAdd.aspx/GetPackingQuantity",
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

function OnMultiUOMEndCallback(s,e)
{
    if(cgrid_MultiUOM.cpDuplicateAltUOM=="DuplicateAltUOM")
    {
        jAlert("Please Enter Different Alt. Quantity.");
        return;
    }
    // Mantis Issue 24397
    if(cgrid_MultiUOM.cpSetBaseQtyRateInGrid != null && cgrid_MultiUOM.cpSetBaseQtyRateInGrid=="1")
    {
        grid.batchEditApi.StartEdit(globalRowIndex, 6);

        var BaseQty = cgrid_MultiUOM.cpBaseQty ;
        var BaseRate = cgrid_MultiUOM.cpBaseRate ;
        // Mantis Issue 24425, 24428
        var AltQuantity = cgrid_MultiUOM.cpAltQuantity ;
        var AltUOM = cgrid_MultiUOM.cpAltUOM ;
        // End of Mantis Issue 24425, 24428

        grid.GetEditor("Quantity").SetValue(BaseQty);
        grid.GetEditor("SalePrice").SetValue(BaseRate);
        grid.GetEditor("Amount").SetValue(BaseQty*BaseRate);
        grid.GetEditor("TotalAmount").SetValue(BaseQty*BaseRate);
        // Mantis Issue 24425, 24428
        grid.GetEditor("Order_AltQuantity").SetValue(AltQuantity);
        grid.GetEditor("Order_AltUOM").SetValue(AltUOM);
        // End of Mantis Issue 24425, 24428
        // Rev Sanchita
        spLostFocus(null,null);
        // End of Rev Sanchita
    }
    // End of Mantis Issue 24397
    // Mantis Issue 24425, 24428
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
            $("#chkUpdateRow").attr('checked', true);
            $("#chkUpdateRow").attr('checked', 'checked');
       
         
        }
        else {
            $("#chkUpdateRow").prop("checked", false);
        }
    }
    // End of Mantis Issue 24425, 24428

    if(cgrid_MultiUOM.cpOpenFocus=="OpenFocus")
    {
        ccmbSecondUOM.SetFocus();
    }

}
function Delete_MultiUom(keyValue, SrlNo) {
   

    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo);
   
}

// Mantis Issue 24425, 24428
function Edit_MultiUom(keyValue, SrlNo) {
    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);
}
// End of Mantis Issue 24425, 24428

function FinalMultiUOM()
{
  
    UomLenthCalculation();
    if (Uomlength == 0 || Uomlength < 0) {

        // Mantis Issue 24397
        //jAlert("Please add Alt. Quantity.");
        jAlert("Please add atleast one Alt. Quantity with Update Row as checked.");
        // End of Mantis Issue 24397
        return;
    }
    else
    {
        cPopup_MultiUOM.Hide();
        // Mantis Issue 24397
        var SLNo = grid.GetEditor('SrlNo').GetValue();
        cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~'+ SLNo);
        // End of Mantis Issue 24397
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 10);
        }, 200)
    }
}
   
// Mantis Issue 24397
function CalcBaseQty()
{
   
   // var PackingQtyAlt = Productdetails.split("||@||")[20];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
    //var PackingQty = Productdetails.split("||@||")[22];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
    //var PackingSaleUOM = Productdetails.split("||@||")[25];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)

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

function CalcBaseRate()
{
    var altQty = cAltUOMQuantity.GetValue();
    var altRate = ccmbAltRate.GetValue();
    var baseQty = $("#UOMQuantity").val();


    if(baseQty>0){
        var BaseRate = (altQty * altRate)/baseQty ;
        ccmbBaseRate.SetValue(BaseRate);
    }
}
// End of Mantis Issue 24397

//function Taxlostfocus(s, e) {
//    
//    //DiscountTextChange(s, e);
//    //Rev Rajdip for Running Balance
//    // SetTotalTaxableAmount(s, e);
//    // SetInvoiceLebelValue();

//}
function TotalAmountgotfocus(s, e) {
            
            
    SetTotalTaxableAmount(s, e);
    //var invValue = parseFloat(cbnrLblTaxableAmtval.GetValue());
    //var invTaxamtval=parseFloat(cbnrLblTaxAmtval.GetValue());   
    //var res=parseFloat(invValue)+parseFloat(invTaxamtval)
    //cbnrLblInvValue.SetText(res.toFixed(2));
    //cbnrlblAmountWithTaxValue.SetText(res.toFixed(2));
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
    // Rev Sanchita
    grid.batchEditApi.StartEdit(globalRowIndex);
    // End of Rev Sanchita
    var srlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    // Mantis Issue 24397
    var BaseRate = ccmbBaseRate.GetValue();
    var AltRate = ccmbAltRate.GetValue();

    var UpdateRow = 'False';
    if($("#chkUpdateRow").prop("checked")){
        UpdateRow='True';
    }
    // End of Mantis Issue 24397

    // Mantis Issue 24397
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "") {
    // Rev Sanchita
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" 
    //        && BaseRate!="0.0000" && AltRate != "0.0000" ) {
    if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty!="0.0000") {
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
            // End of Rev Sanchita
            // End of Mantis Issue 24397
                
            // Mantis Issue 24397
            //cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID);
            if (cbtnMUltiUOM.GetText() == "Update") {
                cgrid_MultiUOM.PerformCallback('UpdateRow~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID+ '~' + BaseRate + '~' + AltRate + '~' + UpdateRow +'~' + hdMultiUOMID);
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
            else{
                cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + BaseRate + '~'+ AltRate + '~' + UpdateRow);
                // End of Mantis Issue 24397

                //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
                cAltUOMQuantity.SetValue("0.0000");
                // Mantis Issue 24397
                $("#UOMQuantity").val(0);
                ccmbBaseRate.SetValue(0) 
                cAltUOMQuantity.SetValue(0)
                ccmbAltRate.SetValue(0)
                ccmbSecondUOM.SetValue("")
                // End of Mantis Issue 24397
                // Rev Sanchita
                $("#chkUpdateRow").prop('checked', false);
                $("#chkUpdateRow").removeAttr("checked");
                // End of Rev Sanchita
            }
            // Rev Sanchita
        }
        else{
            return;
        }
        // End of Rev Sanchita
               
    }
    else {
        return;
    }
}


var canCallBack = true;
var TaggingCall=false;
//Subhabrata
function CustomerButnClick(s, e) {
    $('#CustModel').modal('show');
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

function RateKeydown(s,event){
    if (event.htmlEvent.altKey ==true && event.htmlEvent.keyCode == 82) {
        var Product="0";
        var ProductID = grid.GetEditor('ProductID').GetText();
        if (ProductID != null && ProductID!="") {
            var SpliteDetails = ProductID.split("||@||");
            Product=SpliteDetails[0];
            $.ajax({
                type: "POST",
                url: "SalesOrderAdd.aspx/GetLastRates",
                data: JSON.stringify({ ProductID: Product }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async:false,
                success: function (msg) {
                        
                    var lst=msg.d;
                    var str="";
                    if(lst.length>0)
                    {
                        for(var i=0;i<lst.length;i++){
                            str+="<tr>";
                            str+="<td>"+lst[i].Order_Number+"</td>";
                            str+="<td>"+lst[i].Order_Date+"</td>";
                            str+="<td>"+lst[i].cnt_firstName+"</td>";
                            str+="<td>"+lst[i].OrderDetails_SalePrice+"</td>";
                            str+="</tr>";

                        }
                    }
                    else{
                        str+="<tr>";
                        str+="<td colspan='4' class='text-center'>No Sale Rate Found.</td>";
                        str+="</tr>";
                    }
                            
                    $("#tbodyRate").html('');
                    $("#tbodyRate").html(str);
                    $("#LastRateModal").modal('show');
                }
            });
        }
        else{
            jAlert('Please select a valid product to get sales rate.','Alert');
        }

    }
}
        
function AllControlInitilize() {
           
            
    if (canCallBack) {

        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        grid.batchEditApi.EndEdit();
        $('#ddlInventory').focus();
        canCallBack = false;
        //Edited By Chinmoy 
        //start
        //$('#openlink').hide();
        // PopulatePosGst();
        LoadtBillingShippingCustomerAddress($('#hdnCustomerId').val());
        LoadtBillingShippingShipTopartyAddress();
        //End
        var pageStatus = document.getElementById('hdnPageStatus').value;
        if ($('#hdnPageStatus').val() == "update")
        {
            if($("#hdnPlaceShiptoParty").val()=="1")
            {
                cddl_PosGstSalesOrder.SetEnabled(true);
            }
            else
            {
                cddl_PosGstSalesOrder.SetEnabled(false);
            }
            AllowAddressShipToPartyState = false;
            // BillShipAddressVisible();

            SetTotalTaxableAmount();
                    
        }
        if($("#hdBasketId").val()!="")
        {
            var NoSchemeTypedtl = $("#ddl_numberingScheme").val();
            var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
            var quotelength = NoSchemeTypedtl.toString().split('~')[2];
            var BranchId = NoSchemeTypedtl.toString().split('~')[3];

            var fromdate = NoSchemeTypedtl.toString().split('~')[5];
            var todate = NoSchemeTypedtl.toString().split('~')[6];

            var dt = new Date();

            cPLSalesOrderDate.SetDate(dt);

            if (dt < new Date(fromdate)) {
                cPLSalesOrderDate.SetDate(new Date(fromdate));
            }

            if (dt > new Date(todate)) {
                cPLSalesOrderDate.SetDate(new Date(todate));
            }




            cPLSalesOrderDate.SetMinDate(new Date(fromdate));
            cPLSalesOrderDate.SetMaxDate(new Date(todate));



            if (NoSchemeType == '1') {
                ctxt_SlOrderNo.SetText('Auto');
                ctxt_SlOrderNo.SetEnabled(false);

                var hddnCRmVal = $("#hddnCustIdFromCRM").val();
                if (hddnCRmVal == "1") {
                    page.SetActiveTabIndex(1);
                    page.tabs[0].SetEnabled(false);
                }

                    

                //   document.getElementById('txt_SlOrderNo').disabled = true;
                cPLSalesOrderDate.Focus();
            }
            else if (NoSchemeType == '0') {
                ctxt_SlOrderNo.SetText('');
                ctxt_SlOrderNo.SetEnabled(true);
                ctxt_SlOrderNo.GetInputElement().maxLength = quotelength;
                ctxt_SlOrderNo.Focus();

            }
            else {
                ctxt_SlOrderNo.SetText('');
                ctxt_SlOrderNo.SetEnabled(false);
                document.getElementById("ddl_numberingScheme").focus();

            }

            //Added On 09-01-2018
            if (grid.GetEditor('ProductID').GetText() != "") {
                //ccmbGstCstVat.PerformCallback();
                //ccmbGstCstVatcharge.PerformCallback();
                //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            }
            //End

            $("#ddl_Branch").val(BranchId);
            $("#ddl_Branch").prop("disabled", true);

            //                    grid.AddNewRow();
            PopulateGSTCSTVAT();
        }

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
            callonServer("SalesOrderAdd.aspx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "OnFocus");
            //End Rev Rajdip
            //End Rev Rajdip
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[salesmanIndex=0]"))
            $("input[salesmanIndex=0]").focus();
    } 
    else if (e.code == "Escape") {
        ctxtSalesManAgent.Focus();
    }
}

function OnFocus(Id, Name) {
           
    $("#hdnSalesManAgentId").val(Id);

    ctxtCreditDays.Focus();
    ctxtSalesManAgent.SetText(Name);
    $('#SalesManModel').modal('hide');
}

function Customerkeydown(e) {
            
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    //OtherDetails.CustomerIds=$("#hdnCustomerId").val();
    OtherDetails.CustomerIds=$("#hddnCustomers").val();
    
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");
        if ($("#txtCustSearch").val() != null && $("#txtCustSearch").val() != "") {
            gridquotationLookup.SetEnabled(false);
            $('input[type=radio]').prop('checked', false);
            callonServer("Services/Master.asmx/GetCustomerSaleOrder", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "GetContactPersonOnJSON");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtCustName.Focus();
    }
}

function CustomerKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#CustModel').modal('show');
        $("#txtCustSearch").focus();
    }
}

function ValueSelected(e, indexName) {
            
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;

        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "ProdIndex") {
                SetProduct(Id, name);
            }
            else if (indexName == "salesmanIndex") {
                OnFocus(Id, name);
            }

            else if (indexName == "customerIndex") {
                $('#CustModel').modal('hide');
                GetContactPersonOnJSON(Id, name);
            }


                //Start:Chinmoy 25-05-2018
            else if (indexName == "BillingAreaIndex") {
                SetBillingArea(Id, name);
            }
            else if (indexName == "ShippingAreaIndex") {
                SetShippingArea(Id, name);
            }
            else if (indexName == "customeraddressIndex") {
                SetCustomeraddress(Id, name);
            }
            else if (indexName == "segment1Index") {
                Setsegment1(Id, name);
            }
            else if (indexName == "segment2Index") {
                Setsegment2(Id, name);
            }
            else if (indexName == "segment3Index") {
                Setsegment3(Id, name);
            }
            else if (indexName == "segment4Index") {
                Setsegment4(Id, name);
            }
            else if (indexName == "segment5Index") {
                Setsegment5(Id, name);
            }
            //End

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
            else if (indexName == "salesmanIndex")
                ctxtCreditDays.Focus();
                //Start Chinmoy 25-05-2018
            else if (indexName == "BillingAreaIndex")
                $('#txtbillingArea').focus();
            else if (indexName == "ShippingAreaIndex")
                $('#txtshippingArea').focus();
            else if (indexName == "customeraddressIndex")
                ('#txtshippingShipToParty').focus();
                //End
            else if (indexName == "segment1Index")
                $('#txtSegment1Search').focus();
            else if (indexName == "segment2Index")
                $('#txtSegment2Search').focus();
            else if (indexName == "segment3Index")
                $('#txtSegment3Search').focus();
            else if (indexName == "segment4Index")
                $('#txtSegment4Search').focus();
            else if (indexName == "segment5Index")
                $('#txtSegment5Search').focus();
            else
                $('#txtCustSearch').focus();
        }
    }

}

//Start Chinmoy 25-05-2018
function SetCustomer(Id, Name) {
    if (Id) {
        $('#CustModel').modal('hide');
        ctxtCustName.SetText(Name);

               
        //var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : ""; //Abhisek
               
        //if (type != "") { //Abhisek
        //    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + Id + '~' + startDate + '~' + '%' + '~' + type); //Abhisek
        //} //Abhisek



        GetObjectID('hdnCustomerId').value = Id;
        GetObjectID('hdnAddressDtl').value = '0';

        //REV RAJDIP
        SalesmanBindWRTCustomer(Id);
        //END REV RAJDIP
               

        var CustomerID=Id;

        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/GetCustomerReletedData",
            data: JSON.stringify({ CustomerID: CustomerID }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async:false,
            success: function (msg) {
                var strStatus = data.toString().split('~')[0];
                if (strStatus == "D")
                {
                                
                    jAlert('You have selected a "Dormant" Customer. Please change the Status of this Customer to "Active" to proceed further.');
                    ctxtCustName.SetText("");            
                    GetObjectID('hdnCustomerId').value = "";
                    cddl_AmountAre.SetValue("1");
                    cddl_AmountAre.SetEnabled(true);
                    ctxtCustName.Focus();
                }
                GetContactPerson(Id);
            }
        });
                
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        $('.dxeErrorCellSys').addClass('abc');
        $('.crossBtn').hide();
        page.GetTabByName('General').SetEnabled(false);
        $('#CustModel').modal('hide');                
        var startDate = new Date();
        //startDate = tstartdate.GetValueString();
       

    }
}
//End
       

       
function selectValue() {
    //
    var IsInventory=$("#ddlInventory").val();

    var checked = $('#rdl_Salesquotation').attr('checked', true);
    if (checked) {
        //$(this).attr('checked', false);
        gridquotationLookup.SetEnabled(true);
    }
    else {
        $(this).attr('checked', true);
    }
    var startDate = new Date();
    startDate = cPLSalesOrderDate.GetValueString();
    var key = $("#hdnCustomerId").val();
    var type = ($("[id$='rdl_Salesquotation']").find(":checked").val() != null) ? $("[id$='rdl_Salesquotation']").find(":checked").val() : "";

    //if (type == "QO") {
    //    clbl_Quotation_Date.SetText('PI/Quotation Date');
    //}
    //else if (type == "SINQ") {
    //    clbl_Quotation_Date.SetText('Sales Inquiry Date');
    //}


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
    TaggingCall=true;
    if (key != null && key != '' && type != "") {
        cQuotationComponentPanel.PerformCallback('BindQuotationGrid' + '~' + key + '~' + startDate + '~' + '%'+'~'+ type);
    }
            



    //var componentType = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
    //if (componentType != null && componentType != '') {
    //    grid.PerformCallback('GridBlank');
    //}
}


function GlobalBillingShippingEndCallBack() {
    if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
        cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
        var startDate = new Date();
               
        startDate = cPLSalesOrderDate.GetValueString();
        if (gridquotationLookup.GetValue() != null) {
                   
            var key = $("#hdnCustomerId").val();
            if (key != null && key != '') {
                //cContactPerson.PerformCallback('BindContactPerson~' + key); //ON 08-01-2018 sUBHABRATA
                       

                       
                       

            }
        }
        else {
                    
            var key = $("#hdnCustomerId").val();
            if (key != null && key != '') {
                        
                //cContactPerson.PerformCallback('BindContactPerson~' + key);//sUBHABRATA ON 08-01-2018
                //SetFocusAfterBillingShipping();

                       

                       
                        
            }
        }
        $('#CustModel').modal('hide');
        //ctxtCustName.Focus();
    }
}

function SetDataSourceOnComboBox(ControlObject, Source){
    ControlObject.ClearItems();
    for(var count=0;count<Source.length;count ++){
        ControlObject.AddItem(Source[count].Name , Source[count].Id);
    } 
    ControlObject.SetSelectedIndex(0);
}
    

function SetFocusAfterBillingShipping()
{
    setTimeout(function () {
        cContactPerson.Focus(); 
    }, 200);  
}

function ProductPriceCalculate() {
    if ((grid.GetEditor('SalePrice').GetValue() == null || grid.GetEditor('SalePrice').GetValue() == 0) && (grid.GetEditor('Discount').GetValue() == null || grid.GetEditor('Discount').GetValue() == 0))
    {
        var _saleprice = 0;
        var _Qty = grid.GetEditor('Quantity').GetValue();
        var _Amount = grid.GetEditor('Amount').GetValue();
        _saleprice = (_Amount / _Qty);
        grid.GetEditor('SalePrice').SetValue(_saleprice);
    }
}
function ProductAmountTextChange(s, e) {
    ProductPriceCalculate();
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

function ValidfromCheck(s,e)
{
    cdtProjValidUpto.SetMinDate(cdtProjValidFrom.GetDate());
    if(cdtProjValidUpto.GetDate()<cdtProjValidFrom.GetDate())
    {
        cdtProjValidUpto.Clear();
    }
}

$(document).ready(function () {
    //

         
    if($("#hdnPageStatForApprove").val()=="PO" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {

        document.getElementById("dvReject").style.display="inline-block";
        document.getElementById("dvApprove").style.display="inline-block";
        document.getElementById("dvAppRejRemarks").style.display="block";
    }
    
    if($("#hdnPageStatForApprove").val()=="PO" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {
        var det={};
        det.OrderId=$("#hdnEditOrderId").val();

        if($("#hdnEditOrderId").val()!="")
        {
            $.ajax({
                type: "POST",
                url: "ProjectOrder.aspx/GetApproveRejectStatus",
                data: JSON.stringify(det),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                //async:false,
                success: function (msg) {
                       
                    var   statusValueforApproval = msg.d;
                    if (statusValueforApproval == 1) {
                             
                        document.getElementById("dvRevisionDate").style.display="block";
                        document.getElementById("dvRevision").style.display="block";
                        document.getElementById("dvAppRejRemarks").style.display="block";
                        document.getElementById("dvReject").style.display="none";
                        document.getElementById("dvApprove").style.display="none";
                    }
                    else if(statusValueforApproval == 2)
                    {
                        document.getElementById("dvAppRejRemarks").style.display="block";
                        document.getElementById("dvReject").style.display="none";
                        document.getElementById("dvApprove").style.display="inline-block";
                    }
                    else if(statusValueforApproval == 0)
                    {   document.getElementById("dvAppRejRemarks").style.display="block";
                        document.getElementById("dvReject").style.display="inline-block";
                        document.getElementById("dvApprove").style.display="inline-block";
                    }
                }
            });
        }
    }

    if($("#hdnPageStatus").val()=="update" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {
            
        var detApp={};
        detApp.OrderId=$("#hdnEditOrderId").val();

        $.ajax({
            type: "POST",
            url: "ProjectOrder.aspx/GetApproveRejectStatus",
            data: JSON.stringify(detApp),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            //async:false,
            success: function (msg) {
                       
                statusValueforRejectApproval = msg.d;

                if(statusValueforRejectApproval==1)
                {
                    document.getElementById("dvRevisionDate").style.display="block";
                    document.getElementById("dvRevision").style.display="block";
                   
                }
                 
            }
        });
      
    }

    $('#idOutstanding').on("click",function(){
               
        $("#drdExport").val('0');
        cOutstandingPopup.Show();
        var CustomerId=$("#hdnCustomerId").val();
        var BranchId=$("#ddl_Branch").val();
        $("#hddnBranchId").val(BranchId);
        var AsOnDate=cPLSalesOrderDate.GetDate().format('yyyy-MM-dd');
        $("#hddnAsOnDate").val(AsOnDate);
        $("#hddnOutStandingBlock").val('1');
        //Clear Row
        var rw=$("[id$='CustomerOutstanding_DXMainTable']").find("tr")
        for(var RowClount=0; RowClount <rw.length;RowClount++){
            rw[RowClount].remove();
        }



        //cCustomerOutstanding.Refresh();
                
        //cCustomerOutstanding.PerformCallback('BindOutStanding~' + CustomerId + '~' + BranchId + '~' + AsOnDate);
        var CheckUniqueCode=false;
        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/GetCustomerOutStanding",
            data: JSON.stringify({strAsOnDate: AsOnDate, strCustomerId: CustomerId,BranchId: BranchId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            //async:false,
            success: function (msg) {
                       
                CheckUniqueCode = msg.d;
                if (CheckUniqueCode == true) {
                    cCustomerOutstanding.Refresh();
                            
                }
            }
        });


        //cCustomerOutstanding.Refresh();
        //cOutstandingPopup.Show();

    });

    document.onkeyup = function (e) {
        if (event.keyCode == 27) { //run code for alt+N -- ie, Save & New  
            cOutstandingPopup.Hide();
        }
        else if (event.keyCode == 27) { //run code for alt+N -- ie, Save & New  
            cOutstandingPopup.Hide();
        }
    }

    document.onkeydown = function (e) {
        if (event.altKey == true) {
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
                    if (getUrlVars().req != "V" && $("#hddnDocumentIdTagged").val()!="1") {
                        if (document.getElementById('btn_SaveRecords').style.display != 'none')
                        {
                            Save_ButtonClick();
                        }
                        
                    }
                    break;
                case 88:
                    //
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V" && $("#hddnDocumentIdTagged").val()!="1") {
                        if (document.getElementById('btn_SaveExit').style.display != 'none')
                        {
                            SaveExit_ButtonClick();
                        }
                    }
                    break;
                case 120:
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {
                        if (document.getElementById('btn_SaveExit').style.display != 'none')
                        {
                            SaveExit_ButtonClick();
                        }
                    }
                    break;
                case 84:
                    StopDefaultAction(e);
                    Save_TaxesClick();
                    break;
                case 85:
                    OpenUdf();
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
                case 79:
                    if (page.GetActiveTabIndex() == 1) {
                        fnSaveBillingShipping();
                    }
                    break;
            }
        }
    }
});
function modalShowHide(param) {

    switch (param) {
        case 0:
            $('#exampleModal').modal('toggle');
            break;
        case 1:
            $('#exampleModal').modal({
                show: 'true'
            });
            break;
    }

}

function CustomerCallBackPanelEndCallBack() {
    //GetContactPerson();
}


function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
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
        ctxtQuoteTaxTotalAmt.SetValue( parseFloat(GlobalTaxAmt) - parseFloat(taxAmountGlobalCharges));
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(0 - parseFloat(GlobalTaxAmt) + parseFloat(taxAmountGlobalCharges));
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }

    RecalCulateTaxTotalAmountCharges();

}
var Pre_TotalAmt = "0";
        
function DiscountGotFocus(s, e) {
    if($("#hdnUpperApproveReject").val() == "")
    {
        if($("#hddnCustIdFromCRM").val() == "0")
        {
            cbtn_SaveNewRecords.SetVisible(false);
        }
    }
  
    cbtn_SaveExitRecords.SetVisible(false);

    var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
    Pre_TotalAmt = _Amount;
}

function QuantityGotFocus(s, e) {
             
          
    //LoadingPanelSubmitButton.Show();      
    
    //document.getElementById('btn_SaveExit').style.display = 'none';
    //document.getElementById('btn_SaveRecords').style.display = 'none';

    if($("#hdnUpperApproveReject").val() == "")
    {       
        if($("#hddnCustIdFromCRM").val() == "0")
        {
            cbtn_SaveNewRecords.SetVisible(false);
        }
    }
    
    cbtn_SaveExitRecords.SetVisible(false);

    var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
    Pre_TotalAmt = _Amount;

    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strProductName = SpliteDetails[1];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];

            
    var isOverideConvertion = SpliteDetails[26];
    var packing_saleUOM = SpliteDetails[25];
    var sProduct_SaleUom = SpliteDetails[24];
    var sProduct_quantity = SpliteDetails[22];
    var packing_quantity = SpliteDetails[20];

    var slno= (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

    var QuotationNum= (grid.GetEditor('Quotation_Num').GetText() != null) ? grid.GetEditor('Quotation_Num').GetText() : "0";

    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';
    var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
    var gridPackingQty = '';
    //Rev Subhra 02-05-2019
    if (SpliteDetails.length > 27 ) {
        if (SpliteDetails[27] == "1") {
            IsInventory = 'Yes';
        }
    }
    
    if (QuotationNum!="0" && QuotationNum!="") {
                        
        $.ajax({
            type: "POST",
            url: "Services/Master.asmx/GetMultiUOMDetails",
            data: JSON.stringify({orderid: strProductID,action:'SalesQuotationPackingQty',module:'SalesOrder',strKey : QuotationNum}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                                
                gridPackingQty = msg.d;

                if ($("#hddnMultiUOMSelection").val() == "0") {
                    if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                        ShowUOM(type, "Sales", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                    }
                }
            }
        });


    }
    else if($("#hdAddOrEdit").val() == "Edit"){
                       
        var orderid = grid.GetRowKey(globalRowIndex);
        $.ajax({
            type: "POST",
            url: "Services/Master.asmx/GetMultiUOMDetails",
            data: JSON.stringify({orderid: orderid,action:'SalesOrderPackingQty',module:'SalesOrder',strKey :''}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                                
                gridPackingQty = msg.d;
                if ($("#hddnMultiUOMSelection").val() == "0") {
                    if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                        ShowUOM(type, "Sales", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                    }
                }
            }
        });
    }
      
    else if (SpliteDetails.length > 27 && ( $("#hdBasketId").val()!="" )) {
        if (SpliteDetails[27] == "1") {
            IsInventory = 'Yes';          

            if (SpliteDetails[28] != '') {
                if (parseFloat(SpliteDetails[28]) > 0) {
                    gridPackingQty = SpliteDetails[28];
                    if ($("#hddnMultiUOMSelection").val() == "0") {
                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                            ShowUOM(type, "Sales", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }
                    }
                }
            }
        }
    }



    else{
        if ($("#hddnMultiUOMSelection").val() == "0") {
            if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                ShowUOM(type, "Sales", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
            }
        }
    }
    //End of Rev

    //chinmoy added for  for MultiUOM start
    if($("#hddnMultiUOMSelection").val()=="1")
    {
        grid.batchEditApi.StartEdit(globalRowIndex,7);
        if(gridquotationLookup.GetValue()!="")
        {
            if(grid.GetEditor('Quantity').GetValue() !="0.0000")
            {
                grid.batchEditApi.StartEdit(globalRowIndex,6);
                //Rev 24428
                $("#UOMQuantity").val("0.000");
                //End Rev 24428
            }
        }
    }

    //End
} 

function SalesPriceGotFocus(s, e) {
    //
   // LoadingPanelSubmitButton.Show();
    //document.getElementById('btn_SaveExit').style.display = 'none';
    //document.getElementById('btn_SaveRecords').style.display = 'none';

    if($("#hdnUpperApproveReject").val() == "")
    {
        if($("#hddnCustIdFromCRM").val() == "0")
        {
            cbtn_SaveNewRecords.SetVisible(false);
        }
    }
  
    cbtn_SaveExitRecords.SetVisible(false);

    var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
    Pre_TotalAmt = _Amount;
}

var issavePacking = 0;

function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('Quantity').SetValue(Quantity);
  
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 8);
    }, 600);
}

function SetFoucs() {
    //
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

function ProductsGotFocusFromID(s, e) {

    //pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strProductName = SpliteDetails[1];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    if (IsPackingActive == "Y") {
        $('#lblPackingStk').text(PackingValue);
        divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);

    if (ProductID != "") {

        var SalesOrder_ID = ctxt_SlOrderNo.GetValue();
        var pageStatus = document.getElementById('hddnActionFieldOnStockBlock').value;
        if (pageStatus == 'Add') {
            //Rev work start 25.07.2022 mantise no:0025067 Unable to check warehouse wise stock in Sales Order module
            //cacpAvailableStock.PerformCallback('MainAviableStockBind' + '~' + strProductID);
            cacpAvailableStock.PerformCallback('MainAviableStockBind' + '~' + strProductID);
            //Rev work close 25.07.2022 mantise no:0025067 Unable to check warehouse wise stock in Sales Order module
        }
        else if (pageStatus == 'Edit') {
            //Rev work start 25.07.2022 mantise no:0025067 Unable to check warehouse wise stock in Sales Order module
            //cacpAvailableStock.toback('GetAvailableStockOnOrder' + '~' + strProductID + '~' + SalesOrder_ID + '~' + QuantityValue);
            cacpAvailableStock.toback('GetAvailableStockOnOrder' + '~' + strProductID + '~' + SalesOrder_ID + '~' + QuantityValue);
            //Rev work close 25.07.2022 mantise no:0025067 Unable to check warehouse wise stock in Sales Order module
        }

    }
}

function cmbContactPersonEndCall(s, e) {
    if (cContactPerson.cpDueDate != null) {
        var DeuDate = cContactPerson.cpDueDate;
        var myDate = new Date(DeuDate);

        var invoiceDate = new Date();
        var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));

        ctxtCreditDays.SetValue(0);

        cdt_SaleInvoiceDue.SetDate(myDate);
        cContactPerson.cpDueDate = null;
    }

    if (cContactPerson.cpTotalDue != null) {
        var TotalDue = cContactPerson.cpTotalDue;
        document.getElementById('lblTotalDues').innerHTML = TotalDue;
        //pageheaderContent.style.display = "block";
        divDues.style.display = "block";
        cContactPerson.cpTotalDue = null;
    }
}

function CreditDays_TextChanged(s, e) {
    //
    var CreditDays = ctxtCreditDays.GetValue();

    //var today = new Date();
    var today = cPLSalesOrderDate.GetDate();
    //var newdate = new Date();
    var newdate = cPLSalesOrderDate.GetDate();
    newdate.setDate(today.getDate() + Math.round(CreditDays));

    cdt_SaleInvoiceDue.SetDate(newdate);
    cdt_SaleInvoiceDue.SetEnabled(false);
}

function ProductsGotFocus(s, e) {

    //pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    strProductName = strDescription;
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    if (IsPackingActive == "Y") {
        $('#lblPackingStk').text(PackingValue);
        divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);

    //if (ProductID != "0") {
    //    cacpAvailableStock.PerformCallback(strProductID);
    //}
}

//function isInventoryChanged(s, e) {
//    //
//    var IsInventoryValue = ccmbIsInventory.GetValue();
//    cIsInventory.PerformCallback('BindSession' + '~' + IsInventoryValue);
//    cproductLookUp.gridView.Refresh();
//}

//<%--kaushik Section--%>
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

     
var globalNetAmount = 0;
function SalePriceTextChange(s, e) {
    // 
    //pageheaderContent.style.display = "block";
            
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();
    var SpliteDetails = ProductID.split("||@||");
    var strMultiplier = SpliteDetails[7];
    var strFactor = SpliteDetails[8];
    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    //var strRate = "1";
    var strStkUOM = SpliteDetails[4];
    //var strSalePrice = SpliteDetails[6];
    

    IsDiscountVal = $("#IsDiscountPercentage").val();
    if (strRate == 0) {
        strRate = 1;
    }

    var StockQuantity = strMultiplier * QuantityValue;
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var Amount = QuantityValue * strFactor * (Saleprice / strRate);
    var ResultAmountAfterDiscount = "0";
    if (IsDiscountVal == "Y") {
        if (parseFloat(Discount) > 100) {
            Discount = "0";

            var tb_Discount = grid.GetEditor("Discount");
            tb_Discount.SetValue(Discount);
        }
       // var amountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100); 
        amountAfterDiscount =DecimalRoundoff( parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100),2);

    }
    else
    {
        amountAfterDiscount = DecimalRoundoff(parseFloat(Amount) + parseFloat(Discount),2);
        
    }
 
   


    var tbAmount = grid.GetEditor("Amount");
    //tbAmount.SetValue(DecimalRoundoff(amountAfterDiscount));
    tbAmount.SetValue(amountAfterDiscount);
    var TotaAmountRes = '';
    TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

    var tbTotalAmount = grid.GetEditor("TotalAmount");
    //tbTotalAmount.SetValue(amountAfterDiscount);
    tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));




    //Debjyoti section GST
    var ShippingStateCode = $("#bsSCmbStateHF").val();
    var TaxType = "";
    if (cddl_AmountAre.GetValue() == "1") {
        TaxType = "E";
    }
    else if (cddl_AmountAre.GetValue() == "2") {
        TaxType = "I";
    }

    var CompareStateCode;
    if (cddl_PosGstSalesOrder.GetValue()== "S") {
        CompareStateCode = GeteShippingStateID();
    }
    else {
        CompareStateCode = GetBillingStateID();
    }


    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
    //    SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val());

            
    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
       SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), cPLSalesOrderDate.GetDate(), QuantityValue);
    
    if (parseFloat(Amount) != parseFloat(Pre_TotalAmt)) {        
        var SrlNo = grid.GetEditor("SrlNo").GetValue();       
        var uniqueId=$("#uniqueId").val();
        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/DeleteTaxOnSrl",
            data: JSON.stringify({ SrlNo: SrlNo,uniqueId:uniqueId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {               
               // LoadingPanelSubmitButton.Hide();
                
                //document.getElementById('btn_SaveExit').style.display = 'block';
                //document.getElementById('btn_SaveRecords').style.display = 'block';
                
                if($("#hdnUpperApproveReject").val() == "")
                {
                    if($("#hddnCustIdFromCRM").val() == "0")
                    {
                        cbtn_SaveNewRecords.SetVisible(true);
                    }
                }
               
                cbtn_SaveExitRecords.SetVisible(true);

            }
        });
    }
    else
    {
        //LoadingPanelSubmitButton.Hide();
        
        //document.getElementById('btn_SaveExit').style.display = 'block';
        //document.getElementById('btn_SaveRecords').style.display = 'block';
        
        if($("#hdnUpperApproveReject").val() == "")
        {
            if($("#hddnCustIdFromCRM").val() == "0")
            {
                cbtn_SaveNewRecords.SetVisible(true);
            }
        }
        cbtn_SaveExitRecords.SetVisible(true);
    }
    
    
    // SetRunningTotal();
    //Rev Rajdip       
    //cbnrlblAmountWithTaxValue.SetText("0");
    //var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
    //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
    //cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
    //ENd Rev Rajdip
    // DiscountTextChange(s, e);
    //Rev Rajdip
    SetTotalTaxableAmount(globalRowIndex,16); 
    //SetInvoiceLebelValue();           
    //ENd Rev Rajdip
}


     

//Rev Rajdip For Running Parameters
function SetTotalTaxableAmount(inx, vindex) {
    var count = grid.GetVisibleRowsOnPage();
    var totalAmount = 0;
    var totaltxAmount = 0;
    var totalQuantity = 0;
    var netAmount=0;
            
    for (var i = 0; i < count + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("Quantity").GetValue(), 4);
                if (grid.GetEditor("TaxAmount").GetValue() != null) {
                    totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2);
                    //grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2),2))
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
                    //grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2),2))

                }
                else {
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                }           
                netAmount = netAmount + DecimalRoundoff(grid.GetEditor("TotalAmount").GetValue(), 2);
            }
        }
    }

    globalRowIndex = inx;

    grid.batchEditApi.EndEdit()
    netAmount=netAmount+parseFloat($("#bnrOtherChargesvalue").text());
    cbnrLblTaxableAmtval.SetText(DecimalRoundoff(totalAmount, 2));
    cbnrLblTaxAmtval.SetText(DecimalRoundoff(totaltxAmount, 2));
    cbnrLblInvValue.SetText(DecimalRoundoff(netAmount, 2));
    cbnrlblAmountWithTaxValue.SetText(DecimalRoundoff(netAmount, 2));
    cbnrLblTotalQty.SetText(DecimalRoundoff(totalQuantity, 4)); 
    setTimeout(function () { grid.batchEditApi.StartEdit(inx, vindex); }, 300)
}

//End Rev Rajdip
function ctaxUpdatePanelEndCall(s, e) {
    //
    //grid.batchEditApi.StartEdit(globalRowIndex, 6);
    if (ctaxUpdatePanel.cpstock != null) { 
        LoadingPanel.Hide();
        ctaxUpdatePanel.cpstock = null;
    }
}

function ctaxUpdatePanelEndCall2(s, e) {0
    //
    if (ctaxUpdatePanel2.cpstock != null) {
        //grid.batchEditApi.StartEdit(globalRowIndex, 6);
        LoadingPanel.Hide();
        ctaxUpdatePanel2.cpstock = null;
    }
}


function DiscountTextChange(s, e) {
    //
    var IsDiscountVal = '';
    //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var strProductID = '';
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();
    IsDiscountVal = $("#IsDiscountPercentage").val();
    if (ProductID != null) {
       
        var SpliteDetails = ProductID.split("||@||");
        strProductID = SpliteDetails[0];
        var strFactor = SpliteDetails[8];
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
        if (strSalePrice == '0') {
            strSalePrice = SpliteDetails[6];
        }
        if (strRate == 0) {
            strRate = 1;
        }
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        //Subhabrata on 04-12-2017
        var amountAfterDiscount = "0";
        var ResultAmountAfterDiscount = "0";
        if (IsDiscountVal == "Y") {
            if (parseFloat(Discount) > 100) {
                Discount = "0";

                var tb_Discount = grid.GetEditor("Discount");
                tb_Discount.SetValue(Discount);
            }
            //ResultAmountAfterDiscount = Math.round(Amount).toFixed(2) + ((Math.round(Discount).toFixed(2) * Math.round(Amount).toFixed(2)) / 100);
            //parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            ResultAmountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            console.log(ResultAmountAfterDiscount);
            amountAfterDiscount = (ResultAmountAfterDiscount).toFixed(2);
            //alert(amountAfterDiscount);
        }
        else {
            ResultAmountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
            amountAfterDiscount = ResultAmountAfterDiscount.toFixed(2);
        }

        //End

        // var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(amountAfterDiscount);

        var TotaAmountRes = '';
        TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));
        //tbTotalAmount.SetValue(amountAfterDiscount);


        //Debjyoti section GST
        var ShippingStateCode = $("#bsSCmbStateHF").val();
        var TaxType = "";
        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }

        var CompareStateCode;
        if (cddl_PosGstSalesOrder.GetValue()== "S") {
            CompareStateCode = GeteShippingStateID();
        }
        else {
            CompareStateCode = GetBillingStateID();
        }
        //Added on 09-01-2018
        var _SrlNo = grid.GetEditor("SrlNo").GetValue();
        if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
            var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "N" }
            TaxOfProduct.push(ProductTaxes);
        }
        else {
            $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
        }
        //End

        //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
        //    SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val());
        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
      SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), cPLSalesOrderDate.GetDate(), QuantityValue);

        if (parseFloat(Amount) != parseFloat(Pre_TotalAmt)) {
            // ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue()+ '~' + strProductID);
            var SrlNo = grid.GetEditor("SrlNo").GetValue();
            console.log('SrlNo'+SrlNo);
            var uniqueId=$("#uniqueId").val();
            $.ajax({
                type: "POST",
                url: "SalesOrderAdd.aspx/DeleteTaxOnSrl",
                data: JSON.stringify({ SrlNo: SrlNo,uniqueId:uniqueId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    // RequiredShipToPartyValue = msg.d;
                    //LoadingPanelSubmitButton.Hide();
                    
                    //document.getElementById('btn_SaveExit').style.display = 'block';
                    //document.getElementById('btn_SaveRecords').style.display = 'block';
                    if($("#hdnUpperApproveReject").val() == "")
                    {
                        if($("#hddnCustIdFromCRM").val() == "0")
                        {
                            cbtn_SaveNewRecords.SetVisible(true);
                        }
                    }
                    //cbtn_SaveNewRecords.SetVisible(true);
                    cbtn_SaveExitRecords.SetVisible(true);

                }
            });
        }
        else
        {
            //LoadingPanelSubmitButton.Hide();
            
            //document.getElementById('btn_SaveExit').style.display = 'block';
            //document.getElementById('btn_SaveRecords').style.display = 'block';
            if($("#hdnUpperApproveReject").val() == "")
            {
                if($("#hddnCustIdFromCRM").val() == "0")
                {
                    cbtn_SaveNewRecords.SetVisible(true);
                }
            }
           // cbtn_SaveNewRecords.SetVisible(true);
            cbtn_SaveExitRecords.SetVisible(true);

        }
              
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    //Debjyoti 
    //grid.GetEditor('TaxAmount').SetValue(0);
    //ctaxUpdatePanel2.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    // SetRunningTotal();
    //Rev Rajdip       
    //cbnrlblAmountWithTaxValue.SetText("0");
    //var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
    //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
    //cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
    //ENd Rev Rajdip
    // DiscountTextChange(s, e);
    //Rev Rajdip
    SetTotalTaxableAmount(globalRowIndex, 16); 
    //SetInvoiceLebelValue();           
    //ENd Rev Rajdip
        
}





//Ware house KAUSHIK     27-2-2017




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
        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStk').innerHTML = AvlStk;
        document.getElementById('lblAvailableSStk').innerHTML = AvlStk;
        document.getElementById('lblAvailableStock').innerHTML = cacpAvailableStock.cpstock;
        document.getElementById('lblAvailableStockUOM').innerHTML = document.getElementById('lblStkUOM').innerHTML;
        cCmbWarehouse.cpstock = null;

        grid.batchEditApi.StartEdit(globalRowIndex, 6);
        return;
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
//Ware house KAUSHIK     27-2-2017

//TAXES KAUSHIK     27-2-2017


function acbpCrpUdfEndCall(s, e) {
    //
    if (gridquotationLookup.GetValue() != null) {
                        
        grid.AddNewRow();
    }
    if (cacbpCrpUdf.cpUDF) {

        if (cacbpCrpUdf.cpUDF == "false") {
            jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
            LoadingPanel.Hide();
            if (gridquotationLookup.GetValue() != null) {
                grid.AddNewRow();
            }
            cacbpCrpUdf.cpUDF = null;

        }
        else if (cacbpCrpUdf.cpTransport == "false") {
            jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
            LoadingPanel.Hide();
            //grid.AddNewRow();
            if (gridquotationLookup.GetValue() != null) {
                grid.AddNewRow();
            }
            cacbpCrpUdf.cpTransport = null;
        }
        else if (cacbpCrpUdf.cpTC == "false") {

            jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
            LoadingPanel.Hide();
            //grid.AddNewRow();
            if (gridquotationLookup.GetValue() != null) {
                        
                grid.AddNewRow();
            }
            cacbpCrpUdf.cpTC = null;
        }
        else {
            grid.UpdateEdit();
        }
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

    ctxtProductAmount.SetValue(Math.round(sumAmount).toFixed(2));
    ctxtProductTaxAmount.SetValue(Math.round(sumTaxAmount).toFixed(2));
    ctxtProductDiscount.SetValue(Math.round(sumDiscount).toFixed(2));
    ctxtProductNetAmount.SetValue(sumNetAmount.toFixed(2));
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
                    ctxtProductNetAmount.SetText((sumNetAmount / gstDis).toFixed(2));
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





        //// Date: 30-05-2017    Author: Kallol Samanta  [START] 
        //// Details: Billing/Shipping user control integration 

        //Old Code
        //Get Customer Shipping StateCode
        //var shippingStCode = '';
        //if (cchkBilling.GetValue()) {
        //    shippingStCode = CmbState.GetText();
        //}
        //else {
        //    shippingStCode = CmbState1.GetText();
        //}

        //New Copde
        var shippingStCode = '';
        shippingStCode = ctxtshippingState.GetText();
        if (shippingStCode.trim() != "") {
            shippingStCode = shippingStCode;
        }

        //// Date: 30-05-2017    Author: Kallol Samanta  [END] 





        shippingStCode = shippingStCode;

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
    console.log("hear 1");
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
        ctxtQuoteTaxTotalAmt.SetValue(0 + parseFloat(Sum) - GlobalTaxAmt);
        //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(0 - parseFloat(Sum) + GlobalTaxAmt);
        //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }

    SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
    SetChargesRunningTotal();

    RecalCulateTaxTotalAmountCharges();
}

function RecalCulateTaxTotalAmountCharges() {
    var totalTaxAmount = 0;
    for (var i = 0; i < chargejsonTax.length; i++) {
        console.log("length"+chargejsonTax.length);
        gridTax.batchEditApi.EndEdit();
        gridTax.batchEditApi.StartEdit(i, 3);
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        if (sign == '(+)') {
                   
            totalTaxAmount = parseFloat(totalTaxAmount) + parseFloat(gridTax.GetEditor("Amount").GetValue());
        } else {
            totalTaxAmount = parseFloat(totalTaxAmount) - parseFloat(gridTax.GetEditor("Amount").GetValue());
        }
               
        console.log("totalTaxAmount"+totalTaxAmount);
    }

    //  totalTaxAmount = totalTaxAmount + parseFloat(ctxtGstCstVatCharge.GetValue());
    //totalTaxAmount = totalTaxAmount ;
    ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
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


//taxEs KAUSHIK 27-2-2017
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
//tab start
function disp_prompt(name) {
    //
    if (name == "tab0") {
        //gridLookup.Focus();
        ctxtCustName.Focus();
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        $("#divcross").show();
        //alert(name);
        //document.location.href = "SalesQuotation.aspx?";
    }
    if (name == "tab1") {
        $("#divcross").hide();
        var custID = GetObjectID('hdnCustomerId').value;
        page.GetTabByName('General').SetEnabled(false);
        if (custID == null && custID == '') {
            jAlert('Please select a customer');
            page.SetActiveTabIndex(0);

            return;
        }
        else {
            //
            page.SetActiveTabIndex(1);
            //fn_PopOpen();
        }
    }
            
}
//tab end


//<%--kaushik 24-2-2017--%>

$(document).ready(function () {
    //
    //cProductsPopup.Hide();
    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
    }


    $('#CustModel').on('shown.bs.modal', function () {
        $('#txtCustSearch').focus();
    })

            

    $('#LastRateModal').on('shown.bs.modal', function () {
        $('#btnCloseRate').focus();
    })
    $('#LastRateModal').on('hide.bs.modal', function () {

        grid.batchEditApi.StartEdit(globalRowIndex,9);
    })




    $('#ProductModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })

    //region Sandip Section For Approval Section Start
    $('#ApprovalCross').click(function () {
        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.Refresh();
    })
    //endregion Sandip Section For Approval Dtl Section End

    ////
    //var IsInventoryValue = ccmbIsInventory.GetValue();
    //cIsInventory.PerformCallback('BindSession' + '~' + IsInventoryValue);
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

    var SchemeVal = $('#ddl_numberingScheme option:selected').val();
    if (SchemeVal == "") {
        alert('Please Select Numbering Scheme');
        ctxt_SlOrderNo.SetValue('');
        ctxt_SlOrderNo.Focus();
    }
    else {
        var OrderNo = ctxt_SlOrderNo.GetText();
        if (OrderNo != '') {

            var SchemaLength = GetObjectID('hdnSchemaLength').value;
            var x = parseInt(SchemaLength);
            var y = parseInt(OrderNo.length);

            if (y > x) {
                //alert('Sales Order No length cannot be more than ' + x);
                //jAlert('Please enter unique Sales Order No');
                //ctxt_SlOrderNo.SetValue('');
                //ctxt_SlOrderNo.Focus();

            }
            else {
                var CheckUniqueCode = false;
                $.ajax({
                    type: "POST",
                    url: "SalesOrderAdd.aspx/CheckUniqueCode",
                    data: JSON.stringify({ OrderNo: OrderNo }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            alert('Please enter unique Sales Order No');
                            //jAlert('Please enter unique Sales Order No');
                            ctxt_SlOrderNo.SetValue('');
                            ctxt_SlOrderNo.Focus();
                        }
                        else {
                            $('#MandatorySlOrderNo').attr('style', 'display:none');
                            var hddnCRmVal = $("#hddnCustIdFromCRM").val();
                            if (hddnCRmVal == "1") {
                                page.SetActiveTabIndex(1);
                                page.tabs[0].SetEnabled(false);
                            }
                        }
                    }

                });
            }
        }
    }
}

function DateCheck(s,e) {
    var full_url = document.URL;
    var url_array = full_url.split('key=');
    var id = url_array[1];

            

    var proddata=grid.GetRow(-1).children[3].innerText;

    if(id=="ADD" && proddata.trim()!="")
    {
        if (gridquotationLookup.GetValue() != null) {
            jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                if (r == true) {

                    page.SetActiveTabIndex(0);
                    $('.dxeErrorCellSys').addClass('abc');


                    var startDate = cPLSalesOrderDate.GetValueString();
                    cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                    //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                    //var key = gridLookup.GetValue();subhabrata on 02-01-2018
                    var key = $("#hdnCustomerId").val();

                    if (key != null && key != '') {
                        cQuotationComponentPanel.PerformCallback('BindQuotationGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                    }
                    grid.PerformCallback('GridBlank');

                    ccmbGstCstVat.PerformCallback();
                    ccmbGstCstVatcharge.PerformCallback();
                    // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax('DeleteAllTax' , "" , "")

                }
            });
        }
        else {
            jConfirm('Documents in the grid are to be automatically blank. Confirm ?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    var startDate = cPLSalesOrderDate.GetValueString();
                    page.SetActiveTabIndex(0);
                    $('.dxeErrorCellSys').addClass('abc');
                    cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                    //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                    //var key = gridLookup.GetValue();
                    var key = $("#hdnCustomerId").val();

                    if (key != null && key != '') {
                        cQuotationComponentPanel.PerformCallback('BindQuotationGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                    }
                    grid.PerformCallback('GridBlank');
                    ccmbGstCstVat.PerformCallback();
                    ccmbGstCstVatcharge.PerformCallback();
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax('DeleteAllTax','','')
                    page.SetActiveTabIndex(0);
                }
                else
                {
                            
                    cPLSalesOrderDate.SetDate(cPLSalesOrderDate.lastChangedValue);  
                }
            });

        }
    }

    cdtProjValidFrom.SetMinDate(cPLSalesOrderDate.GetDate());
    
    if(cdtProjValidFrom.GetDate()<cPLSalesOrderDate.GetDate())
    {
        cdtProjValidFrom.Clear();
    }
      
    if(ctxtRevisionDate.GetDate()<cPLSalesOrderDate.GetDate())
    {
        ctxtRevisionDate.Clear();
    }

    ctxtRevisionDate.SetMinDate(cPLSalesOrderDate.GetDate());
           
}
var SimilarProjectStatus="0";
function CloseGridQuotationLookup() {
    //
    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();
    //gridquotationLookup.Focus();
    txt_OANumber.focus();
    if(gridquotationLookup.GetValue()==null)
    {
        $('input[type=radio]').prop('checked', false);
        grid.PerformCallback('GridBlank');
        //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        deleteTax('DeleteAllTax','','');

        gridquotationLookup.SetEnabled(false);
    }
    var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
           
    if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val()=="1")
    {
               
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
        var Doctype=$("#rdl_Salesquotation").find(":checked").val();
        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/DocWiseSimilarProjectCheck",
            data: JSON.stringify({quote_Id:quote_Id,Doctype:Doctype}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                SimilarProjectStatus = msg.d;
                if (SimilarProjectStatus != "1")
                {
                    cPLQADate.SetText("");
                    jAlert("Please select document with same project code to proceed.");
                           
                    return false;

                }
            }
        });
    }
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
            //ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
            ctxtTaxTotAmt.SetText((ctxtTaxTotAmt.GetValue() * 1).toFixed(2) - (GlobalCurTaxAmt * 1).toFixed(2));
        }


        var isValid = taxValue.indexOf('~');
        if (isValid != -1) {
            var rate = parseFloat(taxValue.split('~')[1]);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


            //cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
            cgridTax.GetEditor("Amount").SetValue(((ProdAmt * rate) / 100).toFixed(2));
            //ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
            ctxtTaxTotAmt.SetText(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * rate) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
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
            //cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
            cgridTax.GetEditor("Amount").SetValue(((ProdAmt * s.GetText()) / 100).toFixed(2));

            //ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
            ctxtTaxTotAmt.SetText(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
            GlobalCurTaxAmt = 0;
        } else {
            s.SetText("");
        }
    }

}

function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
}


function txtTax_TextChanged(s, i, e) {


    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    //cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
    cgridTax.GetEditor("Amount").SetValue(((ProdAmt * s.GetText()) / 100).toFixed(2));



}


function taxAmtButnClick1(s, e) {
    console.log(grid.GetFocusedRowIndex());
    rowEditCtrl = s;
}

function BatchUpdate() {

    cgridTax.UpdateEdit();

    return false;
}




$(document).ready(function () {
    //
    ctxtRate.SetValue("");
    ctxtRate.SetEnabled(false);
    ctxt_SlOrderNo.SetEnabled(false);
    gridquotationLookup.SetEnabled(false);

    PopulateLoadGSTCSTVAT();
});


//<%--Debu Section End--%>


//window.onload = function () {
//    // grid.AddNewRow();
//     
//    OnAddNewClick();
//};
function ParentCustomerOnClose(newCustId,customerName,Unique) {
    //clookup_CustomerControlPanelMain1.PerformCallback(newCustId);
    // ctxtCustName.SetText(customerName);
           
    GetObjectID('hdnCustomerId').value = newCustId;
           
    AspxDirectAddCustPopup.Hide();
    ctxtShipToPartyShippingAdd.SetText('');
    if (newCustId != "") {
        ctxtCustName.SetText(customerName);
        GetContactPersonOnJSON(newCustId, customerName);
    }
    //gridLookup.gridView.Refresh();
    //gridLookup.Focus();
}
function AddcustomerClick() {
    var isLighterPage = $("#hidIsLigherContactPage").val();
            
    if (isLighterPage == 1) {
        //rev srijeeta mantis issue 0024515
        //var url = '/OMS/management/Master/customerPopup.html?var=1.1.3.6';
        var url = '/OMS/management/Master/customerPopup.html?v=70.5';
        //rev srijeeta mantis issue 0024515
        
        AspxDirectAddCustPopup.SetContentUrl(url);
        //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();
        AspxDirectAddCustPopup.RefreshContentUrl();
        AspxDirectAddCustPopup.Show();
    }
    else
    {
        var url = '/OMS/management/Master/Customer_general.aspx';
        //window.location.href = url;
        AspxDirectAddCustPopup.SetContentUrl(url);
        AspxDirectAddCustPopup.Show();
    }
}


//Start Chinmoy 25-05-2018
function AfterSaveBillingShipiing(validate) {
            
    GetPosForGstValue();
    if (validate) {
        page.SetActiveTabIndex(0);
        page.tabs[0].SetEnabled(true);
        $("#divcross").show();

        var ShippingAddress=ctxtsAddress1.GetValue();

        var IsDistanceCalculate = document.getElementById('hdnIsDistanceCalculate').value;
        //if(IsDistanceCalculate=='Y')
        //GetLocation(ShippingAddress);

    }
    else {
        page.SetActiveTabIndex(1);
        page.tabs[0].SetEnabled(false);
        $("#divcross").hide();
    }
}

//End
function GetPosForGstValue()
{
           
    cddl_PosGstSalesOrder.ClearItems();
    if(cddl_PosGstSalesOrder.GetItemCount()==0)
    {
        cddl_PosGstSalesOrder.AddItem(GetBillingStateName() + '[Billing]', "B");
        cddl_PosGstSalesOrder.AddItem(GetShippingStateName() + '[Shipping]', "S");
               
    }
            
    else  if(cddl_PosGstSalesOrder.GetItemCount()>2)
    {
        cddl_PosGstSalesOrder.ClearItems();
        //cddl_PosGstSalesOrder.RemoveItem(0);
        //cddl_PosGstSalesOrder.RemoveItem(0);
    }

    if(PosGstId=="" || PosGstId==null)
    {
        cddl_PosGstSalesOrder.SetValue("B");
    }
    else
    {
        cddl_PosGstSalesOrder.SetValue(PosGstId);
    }
}




function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
    gridquotationLookup.SetEnabled(true);
}
//function GetSalesManAgent(e) {
//    var SalesManComboBox = cSalesManComboBox.GetText();
//    if (!cSalesManComboBox.FindItemByText(SalesManComboBox)) {
//        cSalesManComboBox.SetValue("");
//        cSalesManComboBox.Focus();
//        jAlert("Entered Salesman/Agent not Exists.");
//        return;
//    }
//} Subhabrata on 03-01-2018


function GetContactPersonOnJSON(id, Name) {

    var CustomerID = id;

    var OtherDetail = {};
    OtherDetail.CustomerID = id;
    $.ajax({
        type: "POST",
        url: "SalesReturn.aspx/GetCustomerStateCode",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            StateCodeList = msg.d;
            if (StateCodeList.length > 0) {
                if (StateCodeList[0].TransactionType != "") {
                    $("#hdnTransCategory").val(StateCodeList[0].TransactionType);
                }
            }
        },
        error: function (msg) {
            jAlert('Please try again later');
        }
    });

    $.ajax({
        type: "POST",
        url: "SalesOrderAdd.aspx/GetCustomerReletedData",
        data: JSON.stringify({ CustomerID: CustomerID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async:false,
        success: function (msg) {
            var data = msg.d;         
            var strStatus = data.toString().split('~')[0];
            if (strStatus == "D")
            {                                
                jAlert('You have selected a "Dormant" Customer. Please change the Status of this Customer to "Active" to proceed further.');
                ctxtCustName.SetText("");            
                GetObjectID('hdnCustomerId').value = "";
                cddl_AmountAre.SetValue("1");
                cddl_AmountAre.SetEnabled(true);
                ctxtCustName.Focus();
                $('#CustModel').modal('hide');
            }
            else
            {
                AllowAddressShipToPartyState=true;
                //LoadingPanel.Show();
                var IsContactperson=true;
                var startDate = new Date();
                startDate = cPLSalesOrderDate.GetValueString();
                var OutStandingAmount='';          

                $('#CustModel').modal('hide');
                // Added  Chinmoy 25-05-2018
                $('#openlink').show();
                cddl_PosGstSalesOrder.ClearItems();
                cddl_PosGstSalesOrder.SetEnabled(true);
                SetDefaultBillingShippingAddress(id);
                //End
                if (gridquotationLookup.GetValue() != null) {
                    jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                            //var key = gridLookup.GetValue();
                            var key = id;
                            //Rev Rajdip
                            SalesmanBindWRTCustomer(key);
                            //End Rev Rajdip
                            ctxtCustName.SetText(Name);
                            GetObjectID('hdnCustomerId').value = key;                                  
                            pageheaderContent.style.display = "block";
                            if (key != null && key != '') {
                                $.ajax({
                                    type: "POST",
                                    url: "SalesOrderAdd.aspx/GetContactPersonafterBillingShipping",
                                    data: JSON.stringify({ Key: key }),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (r) {
                                        var contactPersonJsonObject=r.d;
                                        //cContactPerson.SetValue(contactPerson);
                                        IsContactperson=false;
                                        SetDataSourceOnComboBox(cContactPerson,contactPersonJsonObject);
                                        SetFocusAfterBillingShipping();                            
                                    }
                                });
                                if(IsContactperson)
                                {
                                    SetFocusAfterBillingShipping();
                                }
                                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                                //New Code
                                //Edited Chinmoy Below Line
                                SetDefaultBillingShippingAddress(key);
                                // LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SO');   //For SalesOrder => SO 
                                //GetObjectID('hdnCustomerId').value = key;
                                if ($('#hfBSAlertFlag').val() == "1") {
                                    jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                                        if (r == true) {
                                            page.SetActiveTabIndex(1);
                                            //cbtnSave_SalesBillingShiping.Focus();
                                            page.tabs[0].SetEnabled(false);
                                            $("#divcross").hide();
                                        }
                                    });
                                }
                                else {
                                    page.SetActiveTabIndex(1);
                                    cbtnSave_SalesBillingShiping.Focus();
                                    page.tabs[0].SetEnabled(false);
                                    $("#divcross").hide();
                                }

                                if (grid.GetVisibleRowsOnPage() == 0) {
                                    OnAddNewClick();
                                }
                                //Rev Rajdip
                                SalesmanBindWRTCustomer(key);
                                //End Rev Rajdip
                                GetObjectID('hdnCustomerId').value = key;
                                GetObjectID('hdnAddressDtl').value = '0';
                               

                                if (grid.GetEditor('ProductID').GetText() != "") {
                                    grid.PerformCallback('GridBlank');
                                    ccmbGstCstVat.PerformCallback();
                                    ccmbGstCstVatcharge.PerformCallback();
                                    // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                                    deleteTax('DeleteAllTax','','');

                                }

                            }
                        }
                    });
                }
                else {
                    var key = id;
                    GetObjectID('hdnCustomerId').value = key;
                    pageheaderContent.style.display = "block";
                    ctxtCustName.SetText(Name);
                    if (key != null && key != '') {

                        $.ajax({
                            type: "POST",
                            url: "SalesOrderAdd.aspx/GetContactPersonafterBillingShipping",
                            data: JSON.stringify({ Key: key }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            // async:false,
                            success: function (r) {
                                var contactPersonJsonObject=r.d;
                                //cContactPerson.SetValue(contactPerson);
                                SetDataSourceOnComboBox(cContactPerson,contactPersonJsonObject);
                                IsContactperson=false;
                                //REV RAJDIP
                                $("#hdnSalesManAgentId").val("");
                                ctxtSalesManAgent.SetText("");
                                //var custid = contactPersonJsonObject[0].Id
                                SalesmanBindWRTCustomer(key);
                                //END REV RAJDIP
                                //SetFocusAfterBillingShipping();                            
                            }

                        });

                        if(IsContactperson)
                        {
                            SetFocusAfterBillingShipping();
                        }
                        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                        //New Code
                        PosGstId = "";
                        cddl_PosGstSalesOrder.SetValue(PosGstId);
                        SetDefaultBillingShippingAddress(key);
                        //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SO');   //For SalesOrder => SO 
                        //GetObjectID('hdnCustomerId').value = key;
                        if ($('#hfBSAlertFlag').val() == "1") {
                            jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                                if (r == true) {
                                    page.SetActiveTabIndex(1);
                                    // cbsSave_BillingShipping.Focus();
                                    //page.tabs[0].SetEnabled(false);
                                    $("#divcross").hide();
                                }
                            });
                        }
                        else {
                            page.SetActiveTabIndex(1);
                            // cbsSave_BillingShipping.Focus();
                            //cContactPerson.Focus();
                            page.tabs[0].SetEnabled(false);
                            $("#divcross").hide();
                        }
                        startDate = cPLSalesOrderDate.GetValueString();
                        GetObjectID('hdnCustomerId').value = key;
                        //alert(GetObjectID('hdnCustomerId').value);
                        GetObjectID('hdnAddressDtl').value = '0';                   
                        if (grid.GetEditor('ProductID').GetText() != "") {
                            grid.PerformCallback('GridBlank');
                            ccmbGstCstVat.PerformCallback();
                            ccmbGstCstVatcharge.PerformCallback();
                            // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                            deleteTax('DeleteAllTax','','');

                        }
                    }
                }
                var CustomerId=$("#hdnCustomerId").val();
                var BranchId=$("#ddl_Branch").val();
                var AsOnDate=cPLSalesOrderDate.GetDate().format('yyyy-MM-dd');

                //Ajax Started
                $.ajax({
                    type: "POST",
                    url: "SalesOrderAdd.aspx/GetCustomerOutStandingAmount",
                    data: JSON.stringify({strAsOnDate: AsOnDate, strCustomerId: CustomerId,BranchId: BranchId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    // async:false,
                    success: function (msg) {
                                
                        LoadingPanel.Hide();
                        OutStandingAmount = msg.d;
                        if(OutStandingAmount==="")
                        {
                            $('#lblOutstanding').text('0.00');
                        }
                        else
                        {
                            $('#lblOutstanding').text(OutStandingAmount);
                        }
                    
                    }
                });

                //End
            }
        }
    });
    //Rev Rajdip For Get & set Credit days

    $.ajax({
        type: "POST",
        url: "SalesOrderAdd.aspx/GetCreditdays",
        data: JSON.stringify({ CustomerId: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async:false,
        success: function (msg) {
            OutStandingAmount = msg.d;
            if (OutStandingAmount.CreditDays != "") {
                var cday = OutStandingAmount.CreditDays;
                ctxtCreditDays.SetValue(cday);
                cdt_SaleInvoiceDue.SetEnabled(false);
                var CreditDays = ctxtCreditDays.GetValue();
                var today = cPLSalesOrderDate.GetDate();
                var newdate = cPLSalesOrderDate.GetDate();
                newdate.setDate(today.getDate() + Math.round(CreditDays));
                cdt_SaleInvoiceDue.SetDate(newdate);
                cdt_SaleInvoiceDue.SetEnabled(false);
            }
        }
    });
    //End Rev rajdip
    // clookup_Project.gridView.Refresh();


    SetEntityType(CustomerID);



    if($('#hdnDocumentSegmentSettings').val()=="1")
    {

        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/GetSegmentDetails",
            data: JSON.stringify({ CustomerId: id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async:false,
            success: function (msg) {
                OutStandingAmount = msg.d;
                if (OutStandingAmount != null)
                {
                    if (OutStandingAmount.Segment1 != "") {
                        var Segment1 = OutStandingAmount.Segment1;
                        var Segment2 = OutStandingAmount.Segment2;
                        var Segment3 = OutStandingAmount.Segment3;
                        var Segment4 = OutStandingAmount.Segment4;
                        var Segment5 = OutStandingAmount.Segment5;

                        if(Segment1=="0")
                        {
                            var div = document.getElementById('DivSegment1');
                            div.style.display = 'none';
                            $('#hdnValueSegment1').val("0");
                        }
                        else{
                            $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                            $('#hdnValueSegment1').val("1");
                        }
                        if(Segment2=="0")
                        {
                            var div = document.getElementById('DivSegment2');
                            div.style.display = 'none';
                            $('#hdnValueSegment2').val("0");
                        }
                        else{
                            $('#ModuleSegment2Header').text(OutStandingAmount.SegmentName2);
                            $('#hdnValueSegment2').val("1");
                        }

                        if(Segment3=="0")
                        {
                            var div = document.getElementById('DivSegment3');
                            div.style.display = 'none';
                            $('#hdnValueSegment3').val("0");
                        }
                        else{
                            $('#ModuleSegment3Header').text(OutStandingAmount.SegmentName3);
                            $('#hdnValueSegment3').val("1");
                        }

                        if(Segment4=="0")
                        {
                            var div = document.getElementById('DivSegment4');
                            div.style.display = 'none';
                            $('#hdnValueSegment4').val("0");
                        }
                        else{
                            $('#ModuleSegment4Header').text(OutStandingAmount.SegmentName4);
                            $('#hdnValueSegment4').val("1");
                        }

                        if(Segment5=="0")
                        {
                            var div = document.getElementById('DivSegment5');
                            div.style.display = 'none';
                            $('#hdnValueSegment5').val("0");
                        }
                        else{
                            $('#ModuleSegment5Header').text(OutStandingAmount.SegmentName5);
                            $('#hdnValueSegment5').val("1");
                        }                  

                    }
                }
                else{
                            
                    document.getElementById('DivSegment1').style.display = 'none';
                    document.getElementById('DivSegment2').style.display = 'none';
                    document.getElementById('DivSegment3').style.display = 'none';
                    document.getElementById('DivSegment4').style.display = 'none';
                    document.getElementById('DivSegment5').style.display = 'none';
                }
            }
        });


        $('#hdnCustomerId').val(CustomerID);
        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment1Search").val();
        OtherDetails.CustomerIds=CustomerID;  

        var HeaderCaption = [];
        HeaderCaption.push("Code"); 
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment1", OtherDetails, "Segment1Table", HeaderCaption, "segment1Index", "Setsegment1");
    }
    
}
//Rev Rajdip For Customer Map To SalesMan
function Deletesalesman() {
    $("#hdnSalesManAgentId").val("");
    ctxtSalesManAgent.SetText("");
}
function SalesmanBindWRTCustomer(Id)
{
            
    $.ajax({
        type: "POST",
        url: "SalesOrderAdd.aspx/MappedSalesManOnetoOne",
        data: JSON.stringify({ Id: Id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var contactPersonJsonObject = r.d;
            IsContactperson = false;
            SetDataSourceOnComboBox(cddlsalesmanmapped, contactPersonJsonObject);
            if (r.d.length > 0) {
                // cPopup_salesman.Show();
                $("#hdnSalesManAgentId").val(r.d[0].Id);
                ctxtSalesManAgent.SetText(r.d[0].Name);
            }
           
        }

    });
}

//End Rev Rajdip For Customer Map To SalesMan
function GetContactPerson(e) {
    //
    var CustomerComboBox = gridLookup.GetText();
    if (!gridLookup.FindItemByText(CustomerComboBox)) {
        gridLookup.SetValue("");
        gridLookup.Focus();
        jAlert("Customer not Exists.");
        return;
    }
    var startDate = new Date();
    startDate = cPLSalesOrderDate.GetValueString();
    if (gridquotationLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                var key = gridLookup.GetValue();
                GetObjectID('hdnCustomerId').value = key;
                if (key != null && key != '') {







                    //// Date: 30-05-2017    Author: Kallol Samanta  [START] 
                    //// Details: Billing/Shipping user control integration 

                    //Old Code
                    //cchkBilling.SetChecked(false);
                    //cchkShipping.SetChecked(false);
                    //cContactPerson.PerformCallback('BindContactPerson~' + key);
                    page.GetTabByName('[B]illing/Shipping').SetEnabled(true);

                    //New Code
                    // Edited Chinmoy 25-05-2018
                    SetDefaultBillingShippingAddress(key);
                    // LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SO');   //For SalesOrder => SO 
                    //GetObjectID('hdnCustomerId').value = key;
                    if ($('#hfBSAlertFlag').val() == "1") {
                        jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                            if (r == true) {
                                page.SetActiveTabIndex(1);
                                cbtnSave_SalesBillingShiping.Focus();
                                page.tabs[0].SetEnabled(false);
                                $("#divcross").hide();
                            }
                        });
                    }
                    else {
                        page.SetActiveTabIndex(1);
                        cbtnSave_SalesBillingShiping.Focus();
                        page.tabs[0].SetEnabled(false);
                        $("#divcross").hide();
                    }

                    if (grid.GetVisibleRowsOnPage() == 0) {
                        OnAddNewClick();
                    }

                    GetObjectID('hdnCustomerId').value = key;

                    GetObjectID('hdnAddressDtl').value = '0';
                        


                    //});

                    //document.getElementById('popup_ok').focus();
                }
            }
        });
    }
    else {
        //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
        var key = gridLookup.GetValue();
        GetObjectID('hdnCustomerId').value = key;

        if (key != null && key != '') {

            page.GetTabByName('[B]illing/Shipping').SetEnabled(true);

            //New Code
            //Edited Chinmoy Below Code
            SetDefaultBillingShippingAddress(key);
            //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SO');   //For SalesOrder => SO 
            //GetObjectID('hdnCustomerId').value = key;
            if ($('#hfBSAlertFlag').val() == "1") {
                jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        page.SetActiveTabIndex(1);
                        cbtnSave_SalesBillingShiping.Focus();
                        page.tabs[0].SetEnabled(false);
                        $("#divcross").hide();
                    }
                });
            }
            else {
                page.SetActiveTabIndex(1);
                cbtnSave_SalesBillingShiping.Focus();
                //cContactPerson.Focus();
                page.tabs[0].SetEnabled(false);
                $("#divcross").hide();
            }

            startDate = cPLSalesOrderDate.GetValueString();



            GetObjectID('hdnCustomerId').value = key;
            //alert(GetObjectID('hdnCustomerId').value);
            GetObjectID('hdnAddressDtl').value = '0';


        }
    }
    //grid.GetEditor('Quotation_Num').SetEnabled(true);
    //grid.GetEditor('ProductName').SetEnabled(true);
    //grid.GetEditor('Description').SetEnabled(true);




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


//cddl_PosGstSalesOrder.AddItem(   GetShippingStateName() + '[Shipping]', "S");
//cddl_PosGstSalesOrder.AddItem( GetBillingStateName() + '[Billing]', "B");
//cddl_PosGstSalesOrder.SetValue("S");
var PosGstId="";
function PopulatePosGst(e)
{
            
    PosGstId=cddl_PosGstSalesOrder.GetValue();
    if(PosGstId=="S")
    {
        cddl_PosGstSalesOrder.SetValue("S");  
        Getshhippingval(PosGstId);
    }
    else if(PosGstId=="B")
    {
        cddl_PosGstSalesOrder.SetValue("B"); 
        Getshhippingval(PosGstId);
    }

    if($("#hdnPlaceShiptoParty").val()=="1")
    {
        TaxDeleteForShipPartyChange();
    }
}



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
        cddlVatGstCst.Focus();
    }
    else if (key == 3) {
        grid.GetEditor('TaxAmount').SetEnabled(false);
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('3');

    }

}

function Onddl_VatGstCstEndCallback(s, e) {
    if (s.GetItemCount() == 1) {
        cddlVatGstCst.SetEnabled(false);
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
//<%--Sudip--%>

var currentEditableVisibleIndex;
var preventEndEditOnLostFocus = false;
var lastProductID;
var setValueFlag;

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

function ProductsCombo_SelectedIndexChanged(s, e) {
    //pageheaderContent.style.display = "block";
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
    divPacking.style.display = "none";
    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);

}
function OnEndCallbackOutstanding(s,e)
{           
}


function OnEndProductCallback(s,e)
{
}
function SetArrForUOM(){
    if (aarr.length == 0) {
        for(var i = -500; i < 500;i++)
        {
            if(grid.GetRow(i) != null){
               
                var ProductID = (grid.batchEditApi.GetCellValue(i,'ProductID') != null) ? grid.batchEditApi.GetCellValue(i,'ProductID') : "0";
                if(ProductID!="0"){
                    var QuotationNum= (grid.GetEditor('Quotation_Num').GetText() != null) ? grid.GetEditor('Quotation_Num').GetText() : "0";
                    if($("#hdAddOrEdit").val() == "Edit"){
                        var SpliteDetails = ProductID.split("||@||");
                        var strProductID = SpliteDetails[0];
                        var orderid = grid.GetRowKey(i);
                        var slnoget = grid.batchEditApi.GetCellValue(i,'SrlNo');
                        var Quantity = grid.batchEditApi.GetCellValue(i,'Quantity');
                        $.ajax({
                            type: "POST",
                            url: "Services/Master.asmx/GetMultiUOMDetails",
                            data: JSON.stringify({orderid: orderid,action:'SalesOrderPackingQty',module:'SalesOrder',strKey :''}),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                               
                                gridPackingQty = msg.d;

                                if(msg.d != ""){
                                    var packing = SpliteDetails[20];
                                    var PackingUom = SpliteDetails[24];
                                    var PackingSelectUom = SpliteDetails[25];
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
    //grid.AddNewRow();
    //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    //var tbQuotation = grid.GetEditor("SrlNo");
    //tbQuotation.SetValue(noofvisiblerows);
    grid.batchEditApi.EndEdit();
    var flag = true;
    LoadingPanel.Show();
    var OrderNo = ctxt_SlOrderNo.GetText();
    var slsdate = cPLSalesOrderDate.GetValue();
    var qudate = cPLQADate.GetText();
    var customerid = GetObjectID('hdnCustomerId').value;
    var salesorderDate = new Date(slsdate);
    var IsTaggingMandatory=$("#hdnConfigValueForTagging").val();
    var quotationDate = "";
    if (qudate != null && qudate != '') {
        var qd = qudate.split('-');
        LoadingPanel.Hide();
        quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

    }
    var revdate=ctxtRevisionDate.GetText();
    var  RevisionDate=new Date(revdate);
    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {
        if(revdate=="01-01-0100"||revdate==null||revdate=="")
        {
            flag = false;
            LoadingPanel.Hide();
            jAlert("Please Enter Revision Details.");
            ctxtRevisionDate.SetFocus();
            return false;
        }
    }
    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {
        if(ctxtRevisionNo.GetText()=="")
        {
            flag = false;
            LoadingPanel.Hide();
            jAlert("Please Enter Revision Details.");
            ctxtRevisionNo.SetFocus();
            return false;
        }
    }

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val()=="1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
    }
    


    if (customerid == null || customerid == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
            
    //var StateCode=GeteShippingStateCode();

    //alert(StateCode);


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

    if(IsTaggingMandatory.trim()=="Y")
    {
        flag = false;
        LoadingPanel.Hide();
        jAlert("Tagging is mandatory.Please select quotationDate");
        gridquotationLookup.Focus();
    }

    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {    
        var detRev={};
        detRev.RevNo=ctxtRevisionNo.GetText();
        detRev.Order=$("#hdnEditOrderId").val();       
        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/Duplicaterevnumbercheck",
            data: JSON.stringify(detRev),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async:false,
            success: function (msg) {
                var duplicateRevCheck=msg.d;
                if (duplicateRevCheck==1)
                {
                    flag = false;
                    LoadingPanel.Hide();
                    jAlert("Please Enter a valid Revision No");                       
                    ctxtRevisionNo.SetFocus();         
                }
            }
        });
      
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

    //var IsQuantityZero=false;
    //for (var i = 1; i <=grid.GetVisibleRowsOnPage() ; i++) {
        
    //    var FrontQuantityValue=(grid.batchEditApi.GetCellValue(backRow, 'Quantity') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Quantity')) : "0";
    //    var QuantityValue=(grid.batchEditApi.GetCellValue(frontRow, 'Quantity') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Quantity')) : "0";
        
    //    //var QuantityValue=(grid.batchEditApi.GetCellValue(i*(-1), 'Quantity') != null) ? (grid.batchEditApi.GetCellValue(i*(-1), 'Quantity')) : "0";
    //   // var Productid=(grid.batchEditApi.GetCellValue(i*(-1), 'ProductID'));
    //    var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
    //    var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

    //    //if(Productid!="" && Productid!=null)
    //    //{
    //    if (frontProduct != "" || backProduct != "") 
    //    {

    //        if((FrontQuantityValue*1)!="0" || (QuantityValue*1)!="0")
    //        {
                
    //        }
    //        else
    //        {
    //            flag = false;
    //            LoadingPanel.Hide();
    //            IsQuantityZero=true;
    //            break;
    //        }            
    //    }
    //    backRow--;
    //    frontRow++;
        
    //}
    //if(IsQuantityZero)
    //{
    //    jAlert("Please fill Quantity" );
    //}

    if (flag) {
        if (IsProduct == "Y") {
       
            SetArrForUOM(); //For UOM Conversion Surojit

            if (gridquotationLookup.GetValue() == null) {
                grid.AddNewRow();
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
            }
            if (grid.GetVisibleRowsOnPage() > 0) {

                if (issavePacking == 1) {
                    if (aarr.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "SalesOrderAdd.aspx/SetSessionPacking",
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {                                
                                var customerval = $("#hdnCustomerId").val() != null ? $("#hdnCustomerId").val() : "";
                                $('#hdfLookupCustomer').val(customerval);
                                // Custom Control Data Bind
                                $('#hfControlData').val($('#hfControlSaveData').val());                              
                                var JsonProductList = JSON.stringify(TaxOfProduct);
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;                          

                                $('#hdfIsDelete').val('I');
                                grid.batchEditApi.EndEdit();                            
                                cacbpCrpUdf.PerformCallback();
                                $('#hdnRefreshType').val('N');
                            }
                        });
                    }
                    else{
                        
                        var customerval = $("#hdnCustomerId").val() != null ? $("#hdnCustomerId").val() : "";
                        $('#hdfLookupCustomer').val(customerval);
                        // Custom Control Data Bind
                        $('#hfControlData').val($('#hfControlSaveData').val());
                        //HiddenTax :On 09-01-2018
                        var JsonProductList = JSON.stringify(TaxOfProduct);
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;
                        //END
                        $('#hdfIsDelete').val('I');
                        grid.batchEditApi.EndEdit();                      
                        cacbpCrpUdf.PerformCallback();
                        $('#hdnRefreshType').val('N');
                    }
                }
                else{

                    if (aarr.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "SalesOrderAdd.aspx/SetSessionPacking",
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                             
                                var customerval = $("#hdnCustomerId").val() != null ? $("#hdnCustomerId").val() : "";
                                $('#hdfLookupCustomer').val(customerval);
                                // Custom Control Data Bind
                                $('#hfControlData').val($('#hfControlSaveData').val());
                                //HiddenTax :On 09-01-2018
                                var JsonProductList = JSON.stringify(TaxOfProduct);
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                //END
                                $('#hdfIsDelete').val('I');
                                grid.batchEditApi.EndEdit();                              
                                cacbpCrpUdf.PerformCallback();
                                $('#hdnRefreshType').val('N');
                            }
                        });
                    }
                    else{
                      
                        var customerval = $("#hdnCustomerId").val() != null ? $("#hdnCustomerId").val() : "";
                        $('#hdfLookupCustomer').val(customerval);
                        // Custom Control Data Bind
                        $('#hfControlData').val($('#hfControlSaveData').val());
                        //HiddenTax :On 09-01-2018
                        var JsonProductList = JSON.stringify(TaxOfProduct);
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;
                        //END
                        $('#hdfIsDelete').val('I');
                        grid.batchEditApi.EndEdit();
                        //grid.UpdateEdit();
                        cacbpCrpUdf.PerformCallback();
                        $('#hdnRefreshType').val('N');
                    }
                }           
            }
            else {
                LoadingPanel.Hide();
                jAlert('You must enter proper details before save');
            }

        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
    // return flag;
}


function Reject_ButtonClick()
{

    if($("#hdnPageStatForApprove").val()=="PO" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {

        if($("#txtAppRejRemarks").val()=="")
        {
                   
            jAlert("Please Enter Reject Remarks.")
            $("#txtAppRejRemarks").focus();
            return false;
        }

    }


    var otherdet={};
    otherdet.ApproveRemarks=$("#txtAppRejRemarks").val();
    otherdet.ApproveRejStatus=2;
    otherdet.OrderId= $("#hdnEditOrderId").val();

    $.ajax({
        type: "POST",
        url: "SalesOrderAdd.aspx/SetApproveReject",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var value=msg.d;
            if (value=="1")
            {
                jAlert("Order Rejected.");
                window.location.href="SalesOrderEntityList.aspx";
            }
        }
               
    });

}

function Approve_ButtonClick() {

    var flag = true;

    if($("#hdnPageStatForApprove").val()=="PO" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {

        if($("#txtAppRejRemarks").val()=="")
        {
            flag = false;
            jAlert("Please Enter Approval Remarks.")
            $("#txtAppRejRemarks").focus();
            return false;
        }

    }

    grid.AddNewRow();
    $("#hdnApproveStatus").val(1);
    var IsProductwithtaggedDoc=false;
    LoadingPanel.Show();
    $("#hddnSaveOrExitButton").val('Save_Exit');
    var OrderNo = ctxt_SlOrderNo.GetText();
    var slsdate = cPLSalesOrderDate.GetValue();
    var qudate = cPLQADate.GetText();
    var customerid = GetObjectID('hdnCustomerId').value;
    var IsTaggingMandatory=$("#hdnConfigValueForTagging").val();
    var salesorderDate = new Date(slsdate);
    var quotationDate = "";

    //var StateCode=GeteShippingStateCode();

    //alert(StateCode);
            
    
    

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

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val()=="1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
    }


    if(IsTaggingMandatory.trim()=="Y")
    {
        if(gridquotationLookup.GetValue() == null)
        {

            flag = false;
            //gridquotationLookup.SetEnabled(true);
            //$('input[type=radio]').prop('checked', true);
            LoadingPanel.Hide();
            jAlert("Tagging is mandatory.Please select quotation number.");
            //gridquotationLookup.Focus();
        }
       
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

        SetArrForUOM(); //For UOM Conversion Surojit

        if (issavePacking == 1) {
            if (aarr.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "SalesOrderAdd.aspx/SetSessionPacking",
                    data: "{'list':'" + JSON.stringify(aarr) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        if (gridquotationLookup.GetValue() == null) {
                            //grid.AddNewRow();
                            //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                            //var tbQuotation = grid.GetEditor("SrlNo");
                            //tbQuotation.SetValue(noofvisiblerows);
                        }
                        if (grid.GetVisibleRowsOnPage() > 0) {
                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                            var customerval = ($("#hdnCustomerId").val()) != null ? $("#hdnCustomerId").val() : "";
                            $('#hdfLookupCustomer').val(customerval);

                            // Custom Control Data Bind

                            $('#hfControlData').val($('#hfControlSaveData').val());

                            //HiddenTax :On 09-01-2018
                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            //END


                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            //alert(2);
                            cacbpCrpUdf.PerformCallback();
                            $('#hdnRefreshType').val('E');
                        }
                        else {
                            LoadingPanel.Hide();
                            jAlert('You must enter proper details before save');
                        }
                    }
                
                
                });
            }
            else{
                if (gridquotationLookup.GetValue() == null) {
                    //grid.AddNewRow();
                    //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                    //var tbQuotation = grid.GetEditor("SrlNo");
                    //tbQuotation.SetValue(noofvisiblerows);
                }
                if (grid.GetVisibleRowsOnPage() > 0) {
                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                    var customerval = ($("#hdnCustomerId").val()) != null ? $("#hdnCustomerId").val() : "";
                    $('#hdfLookupCustomer').val(customerval);

                    // Custom Control Data Bind

                    $('#hfControlData').val($('#hfControlSaveData').val());

                    //HiddenTax :On 09-01-2018
                    var JsonProductList = JSON.stringify(TaxOfProduct);
                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    //END


                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                    //alert(2);
                    cacbpCrpUdf.PerformCallback();
                    $('#hdnRefreshType').val('E');
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('You must enter proper details before save');
                }
            }
        }
        else{


            if (aarr.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "SalesOrderAdd.aspx/SetSessionPacking",
                    data: "{'list':'" + JSON.stringify(aarr) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        if (gridquotationLookup.GetValue() == null) {
                            //grid.AddNewRow();
                            //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                            //var tbQuotation = grid.GetEditor("SrlNo");
                            //tbQuotation.SetValue(noofvisiblerows);
                        }
                        if (grid.GetVisibleRowsOnPage() > 0) {
                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                            var customerval = ($("#hdnCustomerId").val()) != null ? $("#hdnCustomerId").val() : "";
                            $('#hdfLookupCustomer').val(customerval);

                            // Custom Control Data Bind

                            $('#hfControlData').val($('#hfControlSaveData').val());

                            //HiddenTax :On 09-01-2018
                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            //END


                            $('#hdfIsDelete').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            //alert(2);
                            cacbpCrpUdf.PerformCallback();
                            $('#hdnRefreshType').val('E');
                        }
                        else {
                            LoadingPanel.Hide();
                            jAlert('You must enter proper details before save');
                        }
                    }
                
                
                });
            }
            else{


                if (gridquotationLookup.GetValue() == null) {
                    //grid.AddNewRow();
                    //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                    //var tbQuotation = grid.GetEditor("SrlNo");
                    //tbQuotation.SetValue(noofvisiblerows);
                }
                if (grid.GetVisibleRowsOnPage() > 0) {
                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                    var customerval = ($("#hdnCustomerId").val()) != null ? $("#hdnCustomerId").val() : "";
                    $('#hdfLookupCustomer').val(customerval);

                    // Custom Control Data Bind

                    $('#hfControlData').val($('#hfControlSaveData').val());

                    //HiddenTax :On 09-01-2018
                    var JsonProductList = JSON.stringify(TaxOfProduct);
                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    //END


                    $('#hdfIsDelete').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                    //alert(2);
                    cacbpCrpUdf.PerformCallback();
                    $('#hdnRefreshType').val('E');
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('You must enter proper details before save');
                }
            }
        }

       
    }
    // return flag;
}


function SaveExit_ButtonClick() {
    //grid.AddNewRow();
    //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    //var tbQuotation = grid.GetEditor("SrlNo");
    //tbQuotation.SetValue(noofvisiblerows);

    var flag = true;
    //alert(1);
    var IsProductwithtaggedDoc=false;
    LoadingPanel.Show();
    grid.batchEditApi.EndEdit();
    var revdate=ctxtRevisionDate.GetText();
    var  RevisionDate=new Date(revdate);
    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {        
        if(revdate=="01-01-0100"||revdate==null||revdate=="")
        {           
            flag = false;
            LoadingPanel.Hide();
            jAlert("Please Enter Revision Details.");
            ctxtRevisionDate.SetFocus();
            return false;           
        }
    }
    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {
        if(ctxtRevisionNo.GetText()=="")
        {          
            flag = false;
            LoadingPanel.Hide();
            jAlert("Please Enter Revision Details.");
            ctxtRevisionNo.SetFocus();
            return false;
        }
    }


    $("#hddnSaveOrExitButton").val('Save_Exit');
    var OrderNo = ctxt_SlOrderNo.GetText();
    var slsdate = cPLSalesOrderDate.GetValue();
    var qudate = cPLQADate.GetText();
    var customerid = GetObjectID('hdnCustomerId').value;
    var IsTaggingMandatory=$("#hdnConfigValueForTagging").val();
    var salesorderDate = new Date(slsdate);
    var quotationDate = "";

    //var StateCode=GeteShippingStateCode();
    //alert(StateCode);
            
    
    

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

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val()=="1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
    }


    if(IsTaggingMandatory.trim()=="Y")
    {
        if(gridquotationLookup.GetValue() == null)
        {

            flag = false;
            //gridquotationLookup.SetEnabled(true);
            //$('input[type=radio]').prop('checked', true);
            LoadingPanel.Hide();
            jAlert("Tagging is mandatory.Please select quotation number.");
            //gridquotationLookup.Focus();
        }
       
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


    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $("#hdnUpperApproveReject").val() == "")
    {        
        //if(ctxtRevisionNo.GetText()=="")
        //{
        var detRev={};
        detRev.RevNo=ctxtRevisionNo.GetText();
        detRev.Order=$("#hdnEditOrderId").val();          
   
        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/Duplicaterevnumbercheck",
            data: JSON.stringify(detRev),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async:false,
            success: function (msg) {

                var duplicateRevCheck=msg.d;
                if (duplicateRevCheck==1)
                {
                    flag = false;
                    LoadingPanel.Hide();
                    jAlert("Please Enter a valid Revision No");
                    //alert("Please Enter a valid Revision No");
                    //  LoadingPanel.Hide();
                    //$("#txtRevisionNo").val("");
                    ctxtRevisionNo.SetFocus();
         
                }

            }
        });
        // }
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

    //var IsQuantityZero=false;

    //for (var i = 1; i <=grid.GetVisibleRowsOnPage() ; i++) {
        
    //    var FrontQuantityValue=(grid.batchEditApi.GetCellValue(backRow, 'Quantity') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Quantity')) : "0";
    //    var QuantityValue=(grid.batchEditApi.GetCellValue(frontRow, 'Quantity') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Quantity')) : "0";
    //    //var QuantityValue=(grid.batchEditApi.GetCellValue(i*(-1), 'Quantity') != null) ? (grid.batchEditApi.GetCellValue(i*(-1), 'Quantity')) : "0";
    //   /// var Productid=(grid.batchEditApi.GetCellValue(i*(-1), 'ProductID'));
    //    var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
    //    var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

    //    if (frontProduct != "" || backProduct != "") 
    //    {
    //        if((FrontQuantityValue*1)!="0" || (QuantityValue*1)!="0")
    //        {
                
    //        }
    //        else
    //        {
    //            flag = false;
    //            LoadingPanel.Hide();
    //            IsQuantityZero=true;
    //            break;
    //        }          

    //    }
    //    backRow--;
    //    frontRow++;
        
    //}
    //if(IsQuantityZero)
    //{
    //    jAlert("Please fill Quantity" );
    //}

    if (flag) {
        if (IsProduct == "Y") {
            SetArrForUOM(); //For UOM Conversion Surojit

            if (issavePacking == 1) {
                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "SalesOrderAdd.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            if (gridquotationLookup.GetValue() == null) {
                                grid.AddNewRow();
                                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                                var tbQuotation = grid.GetEditor("SrlNo");
                                tbQuotation.SetValue(noofvisiblerows);
                            }
                            if (grid.GetVisibleRowsOnPage() > 0) {
                                //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                                var customerval = ($("#hdnCustomerId").val()) != null ? $("#hdnCustomerId").val() : "";
                                $('#hdfLookupCustomer').val(customerval);

                                // Custom Control Data Bind

                                $('#hfControlData').val($('#hfControlSaveData').val());

                                //HiddenTax :On 09-01-2018
                                var JsonProductList = JSON.stringify(TaxOfProduct);
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                //END


                                $('#hdfIsDelete').val('I');
                                grid.batchEditApi.EndEdit();
                                //grid.UpdateEdit();
                                //alert(2);
                                cacbpCrpUdf.PerformCallback();
                                $('#hdnRefreshType').val('E');
                            }
                            else {
                                LoadingPanel.Hide();
                                jAlert('You must enter proper details before save');
                            }
                        }
                
                
                    });
                }
                else{
                    if (gridquotationLookup.GetValue() == null) {
                        grid.AddNewRow();
                        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                        var tbQuotation = grid.GetEditor("SrlNo");
                        tbQuotation.SetValue(noofvisiblerows);
                    }
                    if (grid.GetVisibleRowsOnPage() > 0) {
                        //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                        //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                        var customerval = ($("#hdnCustomerId").val()) != null ? $("#hdnCustomerId").val() : "";
                        $('#hdfLookupCustomer').val(customerval);

                        // Custom Control Data Bind

                        $('#hfControlData').val($('#hfControlSaveData').val());

                        //HiddenTax :On 09-01-2018
                        var JsonProductList = JSON.stringify(TaxOfProduct);
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;
                        //END


                        $('#hdfIsDelete').val('I');
                        grid.batchEditApi.EndEdit();
                        //grid.UpdateEdit();
                        //alert(2);
                        cacbpCrpUdf.PerformCallback();
                        $('#hdnRefreshType').val('E');
                    }
                    else {
                        LoadingPanel.Hide();
                        jAlert('You must enter proper details before save');
                    }
                }
            }
            else{


                if (aarr.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "SalesOrderAdd.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            if (gridquotationLookup.GetValue() == null) {
                                grid.AddNewRow();
                                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                                var tbQuotation = grid.GetEditor("SrlNo");
                                tbQuotation.SetValue(noofvisiblerows);
                            }
                            if (grid.GetVisibleRowsOnPage() > 0) {
                                //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                                var customerval = ($("#hdnCustomerId").val()) != null ? $("#hdnCustomerId").val() : "";
                                $('#hdfLookupCustomer').val(customerval);

                                // Custom Control Data Bind

                                $('#hfControlData').val($('#hfControlSaveData').val());

                                //HiddenTax :On 09-01-2018
                                var JsonProductList = JSON.stringify(TaxOfProduct);
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                //END


                                $('#hdfIsDelete').val('I');
                                grid.batchEditApi.EndEdit();
                                //grid.UpdateEdit();
                                //alert(2);
                                cacbpCrpUdf.PerformCallback();
                                $('#hdnRefreshType').val('E');
                            }
                            else {
                                LoadingPanel.Hide();
                                jAlert('You must enter proper details before save');
                            }
                        }
                
                
                    });
                }
                else{


                    if (gridquotationLookup.GetValue() == null) {
                        grid.AddNewRow();
                        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                        var tbQuotation = grid.GetEditor("SrlNo");
                        tbQuotation.SetValue(noofvisiblerows);
                    }
                    if (grid.GetVisibleRowsOnPage() > 0) {
                        //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                        //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                        var customerval = ($("#hdnCustomerId").val()) != null ? $("#hdnCustomerId").val() : "";
                        $('#hdfLookupCustomer').val(customerval);

                        // Custom Control Data Bind

                        $('#hfControlData').val($('#hfControlSaveData').val());

                        //HiddenTax :On 09-01-2018
                        var JsonProductList = JSON.stringify(TaxOfProduct);
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;
                        //END


                        $('#hdfIsDelete').val('I');
                        grid.batchEditApi.EndEdit();                     
                        cacbpCrpUdf.PerformCallback();
                        $('#hdnRefreshType').val('E');
                    }
                    else {
                        LoadingPanel.Hide();
                        jAlert('You must enter proper details before save');
                    }
                }
            }

       
        }
        else{
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
    // return flag;
}

var Uomlength = 0;
function UomLenthCalculation()
{
    grid.batchEditApi.StartEdit(globalRowIndex,8);
    var SLNo = grid.GetEditor('SrlNo').GetValue();

    $.ajax({
        type: "POST",
        url: "SalesOrderAdd.aspx/GetQuantityfromSL",
        data: JSON.stringify({ SLNo: SLNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            Uomlength = msg.d;

        }
    });
}
//Rev Rajdip
function SetInvoiceLebelValue() {
            
    var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
    cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));

}
function spLostFocus(s,e){
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();
    var SpliteDetails = ProductID.split("||@||");
    var sProduct_PurPrice = SpliteDetails[13];

    if($('#hdnSalesRateBuyRateChecking').val()=="1")
    {
        if (parseFloat(sProduct_PurPrice) > parseFloat(Saleprice)) {     
       
            jAlert("Your Buy rate is Rs."+ sProduct_PurPrice+" "+"for the product where the selling rate is Rs."+Saleprice+" You are making a Net Loss of Rs."+(parseFloat(sProduct_PurPrice)-parseFloat(Saleprice))+" (Loss Percentage:"+(((parseFloat(sProduct_PurPrice)-parseFloat(Saleprice))*100)/parseFloat(sProduct_PurPrice)).toFixed(2)+"%)", "Alert");
            //return;
        }
    }
    
    SalePriceTextChange(s,e);
}


//function spLostFocus(s,e){
//    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
//    var ProductID = grid.GetEditor('ProductID').GetText();
//    var Id = grid.GetEditor('Quotation_No').GetValue();    
//    if (ProductID != null) {
//        var SpliteDetails = ProductID.split("||@||");
//        var strMultiplier = SpliteDetails[7];
//        var strFactor = SpliteDetails[8];
//        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";       
//        var strStkUOM = SpliteDetails[4];        
//        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
//        if (strSalePrice == '0') {
//            strSalePrice = SpliteDetails[6];
//        }
//        if (strRate == 0) {
//            strRate = 1;
//        }

//        if ($("#ProductMinPrice").val() <= strSalePrice && $("#ProductMaxPrice").val() >= strSalePrice) {

//        }
//        else {
//            if ($("#hdnRateType").val() == "2") {
//                jAlert("Product Min price :" + $("#ProductMinPrice").val() + " and Max price :" + $("#ProductMaxPrice").val(), "Alert", function () {
//                    grid.batchEditApi.StartEdit(globalRowIndex, 11);
//                    return;
//                });
//                return;
//            }
//        }

//        var StockQuantity = strMultiplier * QuantityValue;
//        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
//        $('#lblStkQty').text(StockQuantity);
//        $('#lblStkUOM').text(strStkUOM);        
//        var IsPackingActive = SpliteDetails[10];
//        var Packing_Factor = SpliteDetails[11];
//        var Packing_UOM = SpliteDetails[12];
//        var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;
//        if (IsPackingActive == "Y") {
//            $('#lblPackingStk').text(PackingValue);          
//            $('#divPacking').css({ 'display': 'block' });
//        } else {
//            divPacking.style.display = "none";
//        }
//        var tbAmount = grid.GetEditor("Amount");
//        tbAmount.SetValue(DecimalRoundoff(Amount,2));
//        var TotaAmountRes = '';
//        TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();
//        var tbTotalAmount = grid.GetEditor("TotalAmount");
//        tbTotalAmount.SetValue(DecimalRoundoff(Amount + (TotaAmountRes * 1),2));       
//    }
//    else {
//        jAlert('Select a product first.');
//        grid.GetEditor('Quantity').SetValue('0');
//        grid.GetEditor('ProductID').Focus();
//    } 
//    //Rev Rajdip
//    SetTotalTaxableAmount(globalRowIndex,12); 
//    //SetInvoiceLebelValue();           
//}
//End Rev Rajdip
function QuantityTextChange(s, e) {

    //LoadingPanelSubmitButton.Show();
    //document.getElementById('btn_SaveExit').style.display = 'none';
    //document.getElementById('btn_SaveRecords').style.display = 'none';

    


    if($("#hdnUpperApproveReject").val() == "")
    {
        if($("#hddnCustIdFromCRM").val() == "0")
        {
            cbtn_SaveNewRecords.SetVisible(false);
        }
    }
    
    cbtn_SaveExitRecords.SetVisible(false);
    //chinmoy added for multiUom start   
    // Rev Sanchita
    //if (($("#hddnMultiUOMSelection").val() == "1")) {

    //    //setTimeout(function () {
    //    UomLenthCalculation();
    //    //  }, 200)
              
    //    grid.batchEditApi.StartEdit(globalRowIndex,8);
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
    //                    cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo); 
    //                    setTimeout(function () {
    //                        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //                    }, 600)
    //                }
    //                else {
    //                    grid.batchEditApi.StartEdit(globalRowIndex);
    //                    grid.GetEditor('Quantity').SetValue(qnty);
    //                    setTimeout(function () {
    //                        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //                    }, 200);
    //                }
    //            });
    //            //Rev Rajdip
    //            // SetInvoiceLebelValue();
    //            //End Rev Rajdip
    //        }
    //        else {
    //            grid.batchEditApi.StartEdit(globalRowIndex);
    //            grid.GetEditor('Quantity').SetValue(qnty);
    //            //Rev Rajdip
    //            SetInvoiceLebelValue();
    //            //End Rev Rajdip
               
    //            setTimeout(function () {
    //                grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //            }, 600)     
    //        }
    //    }
    //}

    // End of Rev Sanchita
    //End
    //pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();
    var Id = grid.GetEditor('Quotation_No').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strMultiplier = SpliteDetails[7];
        var strFactor = SpliteDetails[8];
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        //var strRate = "1";
        var strStkUOM = SpliteDetails[4];
        //var strSalePrice = SpliteDetails[6];
        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";

        var sProduct_PurPrice= SpliteDetails[13];
        if($('#hdnSalesRateBuyRateChecking').val()=="1")
        {
            if (parseFloat(sProduct_PurPrice) > parseFloat(strSalePrice)) {        
                //grid.batchEditApi.StartEdit(globalRowIndex, 11);
                jAlert("Your Buy rate is Rs."+ sProduct_PurPrice+" "+"for the product where the selling rate is Rs."+strSalePrice+" You are making a Net Loss of Rs."+(parseFloat(sProduct_PurPrice)-parseFloat(strSalePrice))+" (Loss Percentage:"+(((parseFloat(sProduct_PurPrice)-parseFloat(strSalePrice))*100)/parseFloat(sProduct_PurPrice)).toFixed(2)+"%)", "Alert");
                return;
            }
        }

        if (strSalePrice == '0') {
            strSalePrice = SpliteDetails[6];
        }
        if (strRate == 0) {
            strRate = 1;
        }
        var StockQuantity = strMultiplier * QuantityValue;
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
        $('#lblStkQty').text(StockQuantity);
        $('#lblStkUOM').text(strStkUOM);
        //Subhabrata added on 06-03-2017
        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

        if (IsPackingActive == "Y") {
            $('#lblPackingStk').text(PackingValue);
            //console.log('jhsdfafa');
            //divPacking.style.display = "block";
            $('#divPacking').css({ 'display': 'block' });
        } else {
            divPacking.style.display = "none";
        }//END
        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(DecimalRoundoff(Amount,2));
        var TotaAmountRes = '';
        TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();
        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(DecimalRoundoff(Amount + (TotaAmountRes * 1),2));
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Quantity').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    if($("#hddnMultiUOMSelection").val()=="1")
    {
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 8);
        }, 600)
    }

    var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    if (strSalePrice == '0.00') {

        if($("#hdnUpperApproveReject").val() == "")
        {
            if($("#hddnCustIdFromCRM").val() == "0")
            {
                cbtn_SaveNewRecords.SetVisible(true);
            }
        }
        cbtn_SaveExitRecords.SetVisible(true);
        SetTotalTaxableAmount(globalRowIndex, 8); 
    }
    else
    {
        DiscountTextChange(s, e);
    }
    
    //Rev Rajdip
   // SetTotalTaxableAmount(globalRowIndex, 8); 
    //SetInvoiceLebelValue();           
    //ENd Rev Rajdip
}
function AddBatchNew(s, e) {

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

function AddNewRow() {
    grid.AddNewRow();
    var noofvisiblerows = grid.GetVisibleRowsOnPage();
    var tbQuotation = grid.GetEditor("SrlNo");
    tbQuotation.SetValue(noofvisiblerows);
}

function OnAddNewClick() {


    if (gridquotationLookup.GetValue() == null) {
        grid.AddNewRow();
        //grid.StartEditRow(0);
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        //var i;
        //var cnt = 1;
        //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        //    var tbQuotation = grid.GetEditor("SrlNo");
        //    tbQuotation.SetValue(cnt);


        //    cnt++;
        //}
        // Mantis Issue 24397
        $("#UOMQuantity").val(0);
        Uomlength= 0 ;
        // End of Mantis Issue 24397
    }
    else {
        QuotationNumberChanged();
    }
    // $('#ddl_numberingScheme').focus();
}

function OnAddGridNewClick() {


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

function Save_TaxClick() {
    gridTax.UpdateEdit();
    //Rev Rajdip
    cbnrOtherChargesvalue.SetText('0.00');
    SetInvoiceLebelValueofothercharges();
    //End Rev Rajdip
    cPopup_Taxes.Hide();
}
function SetInvoiceLebelValueofothercharges() {

    //var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());

    //if (document.getElementById('HdPosType').value == 'Crd') {
    //    if (invValue < 0) {
    //        var newAdvAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
    //        cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(newAdvAmount) * 100) / 100).toFixed(2));
    //    }
    //}

    //if (document.getElementById('HdPosType').value == 'Fin') {
    //    if (invValue < 0) {
    //        var newAdvAmountfin = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
    //        cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(ctxtdownPayment.GetValue()) * 100) / 100).toFixed(2));
    //    }
    //}



    //if (document.getElementById('HdPosType').value == 'Crd')
    //    invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
    //else if (document.getElementById('HdPosType').value == 'Fin')
    //    invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue()) + parseFloat(cbnrOtherChargesvalue.GetValue());


    //cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));


    //SetRunningBalance();
    cbnrOtherChargesvalue.SetValue(ctxtQuoteTaxTotalAmt.GetText());
    if(ctxtTotalAmount.GetValue()==0.0)
    {
        cbnrLblInvValue.SetValue(parseFloat(cbnrlblAmountWithTaxValue.GetValue()).toFixed(2)); 
    }
    else
    {
        cbnrLblInvValue.SetValue(parseFloat(ctxtTotalAmount.GetValue()).toFixed(2)); 
    }
}

function AutoPopulateMultiUOM()
{
   // debugger;
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/AutoPopulateAltQuantity",
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
                $("#UOMQuantity").val("0.000");
                cAltUOMQuantity.SetValue("0.0000");
            }
            else
            {
                 ccmbSecondUOM.SetValue(AltUOMId);
                if (AltUOMId == 0) {
                    ccmbSecondUOM.SetValue('');
                }
                else
                {
                    ccmbSecondUOM.SetValue(AltUOMId);
                }
                // Rev Sanchita
                //cAltUOMQuantity.SetValue(calcQuantity);
                //End of Rev Sanchita
            }

        }
    });
}







function callback_InlineRemarks_EndCall(s,e)
{
   
    if(ccallback_InlineRemarks.cpDisplayFocus=="DisplayRemarksFocus")
    {
        $("#txtInlineRemarks").focus();
    }
    else
    {
        cPopup_InlineRemarks.Hide();
        // grid.batchEditApi.StartEdit(globalRowIndex, 7);
    }
}


function FinalRemarks() {


    ccallback_InlineRemarks.PerformCallback('RemarksFinal'+'~'+grid.GetEditor('SrlNo').GetValue()+'~'+$('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');
    
   
}


var Warehouseindex;
function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();
        //


        var totalNetAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'TotalAmount');
        var oldAmountWithTaxValue= parseFloat(cbnrlblAmountWithTaxValue.GetValue());
        var AfterdeleteoldAmountWithTaxValue=oldAmountWithTaxValue-parseFloat(totalNetAmount);
        cbnrlblAmountWithTaxValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
        //cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));

        var RowQuantity = grid.batchEditApi.GetCellValue(e.visibleIndex, 'Quantity');
        var totalquantity = parseFloat(cbnrLblTotalQty.GetValue());
        var updatedquantity=(parseFloat(totalquantity)-parseFloat(RowQuantity));
        //cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));

        var rowTaxAmount = grid.batchEditApi.GetCellValue(e.visibleIndex,'TaxAmount');
        var totaltaxamt=parseFloat(cbnrLblTaxAmtval.GetValue());
        var updatedtaxamt=parseFloat(totaltaxamt)-parseFloat(rowTaxAmount);
        //cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));

        var rowAmount = grid.batchEditApi.GetCellValue(e.visibleIndex,'Amount');
        var TotalAmt=parseFloat(cbnrLblTaxableAmtval.GetValue());
        var updatedAmt=parseFloat(TotalAmt)-parseFloat(rowAmount);
        //cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));

        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        $('#hdnRefreshType').val('');
        if (gridquotationLookup.GetValue() != null) {
            //jAlert();
            jAlert('Cannot Delete using this button as the Proforma is linked with this Sale Order.<br /> Click on Plus(+) sign to Add or Delete Product from last column !', 'Alert Dialog: [Delete Challan Products]', function (r) {

            });
        }
        if (noofvisiblerows != "1" && gridquotationLookup.GetValue() == null) {
            grid.DeleteRow(e.visibleIndex);

            cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
            cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));
            cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));
            cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));
            //var finalNetAmount = parseFloat(totalNetAmount.GetValue());
            //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
            //cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));



            $('#hdfIsDelete').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display');

            grid.batchEditApi.StartEdit(-1, 2);
            grid.batchEditApi.StartEdit(0, 2);




            $('#hdnPageStatus').val('delete');
        }
    }
    else if (e.buttonID == 'AddNew') {

        if (gridquotationLookup.GetValue() == null) {
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

    else if(e.buttonID=="CustomInlineRemarks")
    {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex,6);
        cPopup_InlineRemarks.Show();

        $("#txtInlineRemarks").val('');

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        if (ProductID != "") {
            // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
            ccallback_InlineRemarks.PerformCallback('DisplayRemarks'+'~' + SrlNo+'~'+'0');
        
        }
        else
        {
            $("#txtInlineRemarks").val('');
        }
        //$("#txtInlineRemarks").focus();
        document.getElementById("txtInlineRemarks").focus();
    }

    else if (e.buttonID == 'CustomMultiUOM') {
        
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex, 7);
        var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ProductID = Productdetails.split("||@||")[0];
        var UOMName = grid.GetEditor("UOM").GetValue();
        var quantity = grid.GetEditor("Quantity").GetValue();
        var StockUOM = Productdetails.split("||@||")[5];

        if (StockUOM == "") {
            StockUOM="0";
        }

        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        cAltUOMQuantity.SetValue("0.0000");

        // Mantis Issue 24425, 24428
        //if ((ProductID != "") && (UOMName != "") && (quantity !="0.0000")) {
        if ((ProductID != "") && (UOMName != "") ) {
            // End of Mantis Issue 24425, 24428
            if (StockUOM == "0")
            {
                jAlert("Main Unit Not Defined.");
            }
            else
            {
                if ($("#hddnMultiUOMSelection").val() == "1") 
                {
                    ccmbUOM.SetEnabled(false);
                    var index = e.visibleIndex;
                    grid.batchEditApi.StartEdit(e.visibleIndex, 7);
                    //grid.batchEditApi.StartEdit(globalRowIndex);
                    cgrid_MultiUOM.cpDuplicateAltUOM = "";
                    var Qnty = grid.GetEditor("Quantity").GetValue();
                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var UomId = grid.GetEditor('ProductID').GetText().split("||@||")[3];
                    ccmbUOM.SetValue(UomId);
                    // Mantis Issue 24397
                    //$("#UOMQuantity").val(Qnty);
                    $("#UOMQuantity").val("0.0000");
                    ccmbBaseRate.SetValue(0) 
                    cAltUOMQuantity.SetValue(0)
                    ccmbAltRate.SetValue(0)
                    ccmbSecondUOM.SetValue("")
                    // End of Mantis Issue 24397

                    cPopup_MultiUOM.Show();

                    AutoPopulateMultiUOM();

                    cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo);
                }
            }
        }
        else
        {
            return;
        }
    }

    else if (e.buttonID == 'CustomWarehouse') {
        //
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        //var StkQuantityValue = (grid.GetEditor('StockQuantity').GetValue() != null) ? grid.GetEditor('StockQuantity').GetValue() : "0";

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
            //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
            var strProductName = SpliteDetails[1];
            var StkQuantityValue = QuantityValue * strMultiplier;

            document.getElementById('lblProductName').innerHTML = strProductName;
            document.getElementById('txt_SalesAmount').innerHTML = QuantityValue;
            document.getElementById('txt_SalesUOM').innerHTML = strUOM;
            document.getElementById('txt_StockAmount').innerHTML = StkQuantityValue;

            $('#hdfProductID').val(strProductID);
            $('#hdfProductType').val("");
            $('#hdfProductSerialID').val(SrlNo);
            $('#hdfProductSerialID').val(SrlNo);
            $('#hdnProductQuantity').val(QuantityValue);
            var Ptype = "";


            $.ajax({
                type: "POST",
                url: 'SalesQuotationList.aspx/getProductType',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{Products_ID:\"" + strProductID + "\"}",
                async: false,
                success: function (type) {

                    //$.ajax({
                    //    type: "POST",
                    //    url: 'SalesOrderAdd.aspx/GetIsMandatory',
                    //    contentType: "application/json; charset=utf-8",
                    //    dataType: "json",
                    //    data: "{Products_ID:\"" + strProductID + "\"}",
                    //    async:false,
                    //    success: function (type1) {
                    //        var IsMandatory = type1.d;
                    //    }
                    //});

                    Ptype = type.d;
                    $('#hdfProductType').val(Ptype);

                    if (Ptype == "W") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                        $("#ADelete").css("display", "block");//Subhabrata
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
                        cPopup_Warehouse.Show();
                    }
                    else {
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

                        jAlert("No Warehouse or Batch or Serial is actived !");
                    }
                }
            });

        }
    }
}
function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');
    //Rev Subhra 15-05-2019
    grid.batchEditApi.StartEdit(globalRowIndex, 8);
    //End of Rev Subhra 15-05-2019
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
        grid.batchEditApi.StartEdit(Warehouseindex, 7);
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Sales Quantity must be equal to Warehouse.');
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
    grid.batchEditApi.StartEdit(Warehouseindex, 9);
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
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
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



function txtserialTextChanged() {
    checkListBox.UnselectAll();

    var SerialNo = (ctxtserial.GetValue != null) ? (ctxtserial.GetValue()) : "0";
    ctxtserial.SetValue("");
    var texts = [SerialNo];
    var values = GetValuesByTexts(texts);
    checkListBox.SelectValues(values);
    UpdateSelectAllItemState();
    UpdateText(); // for remove non-existing texts
    SaveWarehouse();
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
$(document).ready(function () {
    //
    $('#ddl_VatGstCst_I').blur(function () {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    })
    $('#ddl_AmountAre_I').blur(function () {


        var key = cddl_AmountAre.GetValue();

        if (key == 1 || key == 3) {
            if (grid.GetVisibleRowsOnPage() == 1) {
                if($("#hdnProjectSelectInEntryModule").val()=="1")
                {
                    clookup_Project.SetFocus(); 
                }
                else
                {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
                        
            }

        }
    })

});
//<%-- kaushik 20-2-2017 --%>
function clookup_Project_LostFocus()
{
    grid.batchEditApi.StartEdit(-1, 2);
    //Hierarchy Start Tanmoy
    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
    //Hierarchy End Tanmoy
}

function clookup_project_GotFocus()
{

    clookup_Project.gridView.Refresh();
    clookup_Project.ShowDropDown();
}

//Hierarchy Start Tanmoy
function ProjectValueChange(s, e) {
           
    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'SalesOrderAdd.aspx/getHierarchyID',
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
    if (event.keyCode == 86 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        StopDefaultAction(e);


        page.SetActiveTabIndex(0);
        //gridLookup.Focus();
        ctxtCustName.Focus();
        // document.getElementById('Button3').click();

        // return false;
    }
}
// <![CDATA[
var textSeparator = ";";
var selectedChkValue = "";

function OnListBoxSelectionChanged(listBox, args) {
    if (args.index == 0)
        args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
    UpdateSelectAllItemState();
    UpdateText();

    var selectedItems = checkListBox.GetSelectedItems();
    var val = GetSelectedItemsText(selectedItems);
    var strWarehouse = cCmbWarehouse.GetValue();
    var strBatchID = cCmbBatch.GetValue();
    var ProducttId = $("#hdfProductID").val();
    //$.ajax({
    //    type: "POST",
    //    url: "SalesOrderAdd.aspx/GetSerialId",
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
    var selectedItems = checkListBox.GetSelectedItems();
    selectedChkValue = GetSelectedItemsText(selectedItems);
    //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
    checkComboBox.SetText(selectedItems.length + " Items");

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


   

function QuotationNumberChanged() {
    var quote_Id = gridquotationLookup.GetValue();
    if ( SimilarProjectStatus != "-1" )
    {
        if (quote_Id != null) {
            var arr = quote_Id.split(',');

            if (arr.length > 1) {
                cPLQADate.SetText('Multiple Select Quotation Dates');
            }
            else {
                if (arr.length == 1) {
                    cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);
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
        txt_OANumber.focus();
    }
}
function GridCallBack() {
    $('#ddlInventory').focus();
}
function ChangeState(value) {
    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}
function componentEndCallBack(s, e) {
            
    gridquotationLookup.gridView.Refresh();
    if (grid.GetVisibleRowsOnPage() == 0) {
        OnAddNewClick();
    }
    if(TaggingCall)
    {
        gridquotationLookup.Focus();
        TaggingCall=false;
    }
    else
    {
        ctxt_OANumber.Focus();
    }
    if(cQuotationComponentPanel.cpTaggedTaxAmountType!="")
    {
        var value=cQuotationComponentPanel.cpTaggedTaxAmountType;
        cddl_AmountAre.SetValue(value);
        cddl_AmountAre.SetEnabled(false);
    }
    //  cQuotationBillingShipping.PerformCallback();
}
//Code for UDF Control  kaushik 24-2-2017
function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        // var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '';
        var keyval = $('#hdnmodeId').val();
        //  alert(keyval);
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SO&&KeyVal_InternalID=' + keyval;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}
// End Udf Code  kaushik 24-2-2017
//<%--End Sudip--%>
//<%--Debu Section--%>
function SetRunningTotal() {
    //

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
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''),ProdAmt,sign);
        }
        if (sign == '(+)') {
            runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }
        else
        {
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


function gridFocusedRowChanged(s, e) {
            
    globalRowIndex = e.visibleIndex;
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
    //alert('hi');
    var finalTaxAmt = parseFloat(s.GetValue());
    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
    if (sign == '(+)') {
        //ctxtTaxTotAmt.SetValue(Math.round(totAmt + finalTaxAmt - taxAmountGlobal));
        ctxtTaxTotAmt.SetValue(((totAmt * 1) + (finalTaxAmt * 1) - (taxAmountGlobal * 1)).toFixed(2));
    } else {
        //ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
        ctxtTaxTotAmt.SetValue(((totAmt * 1) + (finalTaxAmt * -1) - (taxAmountGlobal * -1)).toFixed(2));
    }


    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''),parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()),sign);
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
    ctxtTaxTotAmt.SetValue(((totAmt * 1) + (calculatedValue * 1) - (GlobalCurTaxAmt * 1)).toFixed(2));

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
            //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
            ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) - (GlobalCurTaxAmt * 1)).toFixed(2));
        }


        var isValid = taxValue.indexOf('~');
        if (isValid != -1) {
            var rate = parseFloat(taxValue.split('~')[1]);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
            //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
            ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * rate) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
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

            //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
            ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
            GlobalCurTaxAmt = 0;
        } else {
            s.SetText("");
        }
    }

}

function SetOtherTaxValueOnRespectiveRow(idx, amt, name,runninTot,signCal) {
    for (var i = 0; i < taxJson.length; i++) {
        if (taxJson[i].applicableBy == name) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            var totCal=0;
            if (signCal == '(+)') {
                totCal= parseFloat(parseFloat(amt)+parseFloat(runninTot));
            }
            else{
                totCal= parseFloat(parseFloat(runninTot)-parseFloat(amt));
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
                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                ctxtTaxTotAmt.SetValue((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1));
                GlobalCurTaxAmt = 0;
            }
            else {
                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (s.GetText() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
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
                //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
                GlobalCurTaxAmt = 0;
            }
            else {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (s.GetText() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
                GlobalCurTaxAmt = 0;
            }
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''),parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()),sign);

            //Call for Running Total
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

    ctxtTaxTotAmt.SetValue((totalInlineTaxAmount * 1).toFixed(2));
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

function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function taxAmtButnClick(s, e) {   
    if (e.buttonIndex == 0) {

        if (cddl_AmountAre.GetValue() != null) {
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";

            if (ProductID.trim() != "") {

                document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText() == "" ? "1" : grid.GetEditor('SrlNo').GetText();
                ctxtTaxTotAmt.SetValue(0);
                ccmbGstCstVat.SetSelectedIndex(0);
                $('.RecalculateInline').hide();
                caspxTaxpopUp.Show();
                //Set Product Gross Amount and Net Amount

                var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                //var strRate = "1";
                var strStkUOM = SpliteDetails[4];
                // var strSalePrice = SpliteDetails[6];
                var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                if (strRate == 0) {
                    strRate = 1;
                }                
                var StockQuantity = strMultiplier * QuantityValue;

                var Amount =0.00;

                //var Amount = parseFloat((Math.round((QuantityValue * strFactor * (strSalePrice / strRate)) * 100).toFixed(2)) / 100);
                
                var TaxType = "";
                if (cddl_AmountAre.GetValue() == "1") {
                    Amount = parseFloat((Math.round((QuantityValue * strFactor * (strSalePrice / strRate)) * 100).toFixed(2)) / 100);
                }
                else if (cddl_AmountAre.GetValue() == "2") {
                    Amount =(grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "";
                }

                var IsDiscountPercentage = document.getElementById('IsDiscountPercentage').value;
                var amountAfterDiscount = "0";
                var ResultamountAfterDiscount = "0";
                if (IsDiscountPercentage == "Y") {
                    ResultamountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                    amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
                }
                else {
                    ResultamountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
                    amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
                }              
                var _GrossAmt = (Amount * 1);             
                var _NetAmt = (amountAfterDiscount * 1);
                clblTaxProdGrossAmt.SetText(_GrossAmt.toFixed(2));
                clblProdNetAmt.SetText(_NetAmt.toFixed(2));
                document.getElementById('HdProdGrossAmt').value = _GrossAmt;
                document.getElementById('HdProdNetAmt').value = _NetAmt;               
                if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {                  
                    var DiscountAfter = parseFloat(grid.GetEditor('Discount').GetValue());
                    var discount = DiscountAfter.toFixed(2); 
                    clblTaxDiscount.SetText(discount);
                }
                else {
                    clblTaxDiscount.SetText('0.00');
                }               
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
                                //clblTaxProdGrossAmt.SetText(Math.round(Amount / gstDis).toFixed(2));
                                clblTaxProdGrossAmt.SetText(parseFloat(Amount / gstDis));
                                //document.getElementById('HdProdGrossAmt').value = Math.round(Amount / gstDis).toFixed(2);
                                document.getElementById('HdProdGrossAmt').value = parseFloat(Amount / gstDis);
                                //clblGstForGross.SetText(Math.round(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                clblGstForGross.SetText(parseFloat(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)));
                                clblTaxableNet.SetText("");
                            }
                            else {
                                $('.gstGrossAmount').hide();
                                //clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2));
                                clblProdNetAmt.SetText(parseFloat(grid.GetEditor('Amount').GetValue() / gstDis));
                                //document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2);
                                document.getElementById('HdProdNetAmt').value = parseFloat(grid.GetEditor('Amount').GetValue() / gstDis);
                                //clblGstForNet.SetText(Math.round(grid.GetEditor('Amount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
                                clblGstForNet.SetText(parseFloat(grid.GetEditor('Amount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)));
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
                            
                    if (cddl_PosGstSalesOrder.GetValue() == "S") {
                        shippingStCode = GeteShippingStateCode();
                    }
                    else {
                        shippingStCode = GetBillingStateCode();
                    }
                    shippingStCode = shippingStCode;

                    //// ###########  Old Code #####################
                    //////Get Customer Shipping StateCode
                    ////var shippingStCode = '';
                    ////if (cchkBilling.GetValue()) {
                    ////    shippingStCode = CmbState.GetText();
                    ////}
                    ////else {
                    ////    shippingStCode = CmbState1.GetText();
                    ////}
                    ////shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

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
                                //remove tax because GSTIN is not define
                                ccmbGstCstVat.RemoveItem(cmbCount);
                                cmbCount--;
                            }
                        }
                    }


                }
                //End here
                //Subhabrata on 09-01-2018
                var _SrlNo = document.getElementById('HdSerialNo').value;
                var _IsEntry="";
                if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length > 0) {
                    _IsEntry=TaxOfProduct.find(o => o.SrlNo === _SrlNo).IsTaxEntry;
                }

                if(_IsEntry=="N"){
                    cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                    //Set default combo
                    cgridTax.cpComboCode = grid.GetEditor('ProductID').GetText().split('||@||')[9];
                }
                else{
                    cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                }
                //End

                ctxtprodBasicAmt.SetValue(grid.GetEditor('Amount').GetValue());
            } else {
                grid.batchEditApi.StartEdit(globalRowIndex, 13);
            }
        }
    }
}

function taxAmtButnClick1(s, e) {
    console.log(grid.GetFocusedRowIndex());
    rowEditCtrl = s;
}
        
function BatchUpdate() {
    //on 09-01-2018
    var _SrlNo = document.getElementById('HdSerialNo').value;
    if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
        var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "Y" }
        TaxOfProduct.push(ProductTaxes)
    }
    else {
        $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "Y"; });
    }
    //END
    cgridTax.UpdateEdit();
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
            SetRunningTotal();
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
        //ctxtTaxTotAmt.SetValue(gridValue + ddValue);
        ctxtTaxTotAmt.SetValue(((gridValue * 1) + (ddValue * 1)).toFixed(2));
        cgridTax.cpUpdated = "";
    }

    else {
        var totAmt = ctxtTaxTotAmt.GetValue();
        cgridTax.CancelEdit();
        caspxTaxpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 13);
        grid.GetEditor("TaxAmount").SetValue(totAmt);
        grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()),2));

    }

    if (cgridTax.GetVisibleRowsOnPage() == 0) {
        $('.cgridTaxClass').hide();
        ccmbGstCstVat.Focus();
    }
    //Rev Rajdip
    SetTotalTaxableAmount(globalRowIndex, 5); 
    //SetInvoiceLebelValue();           
    //ENd Rev Rajdip
    ShowTaxPopUp("IY");
    RecalCulateTaxTotalAmountInline();
}

//kaushik 27-2-2017 grid bind with respect to currency changes
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
//kaushik 27-2-2017
//<%--Debu Section End--%>
//<%--Batch Product Popup Start--%>

function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    if (e.htmlEvent.key == "Tab") {

        s.OnButtonClick(0);
    }
}
var strProductName;
var TaxOfProduct = [];
function ProductButnClick(s, e) {
            
    if (e.buttonIndex == 0) {
        //var ProductID = grid.GetEditor('ProductID').GetText();
        //var SpliteDetails = ProductID.split("||@||");
        //strProductName = grid.GetEditor('ProductName').GetText();
        //if (cproductLookUp.Clear()) {

        //    cProductpopUp.Show();
        //    cproductLookUp.Focus();
        //    cproductLookUp.ShowDropDown();
        //}

        $('#txtProdSearch').val('');
        $('#ProductModel').modal('show');
    }
}

function prodkeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    OtherDetails.InventoryType = $("#ddlInventory").val();
    OtherDetails.ProductIds= $("#hdnnproductIds").val();
            
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("IsInventory");
        //HeaderCaption.push("IsInstallation");
        HeaderCaption.push("Class");
        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetSalesOrderProductList", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //grid.GetEditor("ProductName").Focus();
        grid.batchEditApi.StartEdit(globalRowIndex, 3);
    }
}
function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        //cProductpopUp.Hide();//Subhabrata on 08-01-2018
        grid.batchEditApi.StartEdit(globalRowIndex, 6);

    }
}
function ddlInventory_OnChange() {
    //cproductLookUp.GetGridView().Refresh();
    $('input[type=radio]').prop('checked', false);
    gridquotationLookup.SetEnabled(false);      
}
var IsInventory = '';
function deleteTax(Action,srl,productid) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;
    OtherDetail.prodid = productid;
    OtherDetail.uniqueId = $("#uniqueId").val();

    $.ajax({
        type: "POST",
        url: "SalesOrderAdd.aspx/taxUpdatePanel_Callback",
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
function SetProduct(Id, Name,e) {
    //Subhra 28-03-2019 (Commented Because it is not used anywhere and for this,page getting error)
    //IsInventory = e.parentElement.children[4].innerText;
    //Subhra 28-03-2019
    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#ProductModel').modal('hide');

    //cProductpopUp.Hide();Subhabrata on 08-01-2018
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    //console.log(LookUpData);

    pageheaderContent.style.display = "block";
    // $('#openlink').hide();
    cddl_AmountAre.SetEnabled(false);
    if($("#hdnPlaceShiptoParty").val()=="1")
    {
        cddl_PosGstSalesOrder.SetEnabled(true);
    }
    else
    {
        cddl_PosGstSalesOrder.SetEnabled(false);
    }
    PopulatePosGst();
    // BillShipAddressVisible();
    //Chinmoy added this variable
    AllowAddressShipToPartyState = false;

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");
    //var tbStkUOM = grid.GetEditor("StockUOM");
    //var tbStockQuantity = grid.GetEditor("StockQuantity");

    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];

    var sProduct_PurPrice = SpliteDetails[13];
    var sProduct_MinSalePrice = SpliteDetails[17];
    var sProduct_MRP = SpliteDetails[18];
    var sProduct_Cost= SpliteDetails[28];

    $('#lblSell').text(strSalePrice);
    $('#lblMRP').text(sProduct_MRP);
    $('#lblPurchase').text(sProduct_PurPrice);
    $('#lblCost').text(sProduct_Cost);

    if($('#hdnPricingDetail').val()=="1")
    {
        $('#DivSell').css({ 'display': 'block' });
        $('#DivMRP').css({ 'display': 'block' });
        $('#DivPurchase').css({ 'display': 'block' });
        $('#DivCost').css({ 'display': 'block' });
    }
    





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
    divPacking.style.display = "none";
    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");

            
    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    //grid.batchEditApi.StartEdit(globalRowIndex, 6);
            
    //Debjyoti
    if (grid.GetEditor("ProductID").GetValue() != "" && grid.GetEditor("ProductID").GetValue() != null) {

        // ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);

        deleteTax('DelProdbySl' , grid.GetEditor("SrlNo").GetValue() , strProductID)
    }

    var _SrlNo = grid.GetEditor("SrlNo").GetValue();
    if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
        var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "N" }
        TaxOfProduct.push(ProductTaxes);
        grid.batchEditApi.StartEdit(globalRowIndex, 4); 
        SetFocusAfterProductSelect();
    }
    else {
        $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
        grid.batchEditApi.StartEdit(globalRowIndex, 4); 
        SetFocusAfterProductSelect();
    }


    //cacpAvailableStock.PerformCallback('MainAviableStockBind' + '~' + strProductID);
    //Rev work start 25.07.2022 mantise no:0025067 Unable to check warehouse wise stock in Sales Order module
    cacpAvailableStock.PerformCallback('MainAviableStockBind' + '~' + strProductID);
    //Rev work close 25.07.2022 mantise no:0025067 Unable to check warehouse wise stock in Sales Order module
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();
    var Id = grid.GetEditor('Quotation_No').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strMultiplier = SpliteDetails[7];
        var strFactor = SpliteDetails[8];
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        //var strRate = "1";
        var strStkUOM = SpliteDetails[4];
        //var strSalePrice = SpliteDetails[6];
        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
        if (strSalePrice == '0') {
            strSalePrice = SpliteDetails[6];
        }
        if (strRate == 0) {
            strRate = 1;
        }
        var StockQuantity = strMultiplier * QuantityValue;
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        $('#lblStkQty').text(StockQuantity);
        $('#lblStkUOM').text(strStkUOM);
        //Rev work start 28.06.2022 Mantise no:24949
        if($('#hdnSettings').val()=="YES")
        {
            GetSalesRateSchemePrice($("#hdnCustomerId").val(), strProductID, "0");            
        }        
        //Rev work close 28.06.2022 Mantise no:24949
        //var tbStockQuantity = grid.GetEditor("StockQuantity");
        //tbStockQuantity.SetValue(StockQuantity);

        //Subhabrata added on 06-03-2017
        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

        if (IsPackingActive == "Y") {
            $('#lblPackingStk').text(PackingValue);
            //console.log('jhsdfafa');
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

        //DiscountTextChange(s, e);
        
        setTimeout(function () {
            if ($("#ProductMinPrice").val() != "") {
                grid.GetEditor("SalePrice").SetValue($("#ProductMinPrice").val());
            }
        }, 200);
        
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Quantity').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    //Rev Rajdip
    SetTotalTaxableAmount(globalRowIndex, 4); 
    //SetInvoiceLebelValue();     
    



}
//Rev work start 28.06.2022 Mantise no:24949
function setValueOfSAale(){
        if ($("#ProductMinPrice").val() != "") {
            grid.GetEditor("SalePrice").SetValue($("#ProductMinPrice").val());
        }
}
//Rev work close 28.06.2022 Mantise no:24949
function SetFocusAfterProductSelect()
{
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 4); 
    }, 200);  
}
function ProductSelected(s, e) {
    //LoadingPanel.Show();
    if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
        //cProductpopUp.Hide();//Subhabrata on 08-01-2018
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
        return;
    }
    var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
    var ProductCode = cproductLookUp.GetValue();
    if (!ProductCode) {
        LookUpData = null;
    }
    //cProductpopUp.Hide();//Subhabrata on 08-01-2018
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    //console.log(LookUpData);
    //pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);
    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");
    //var tbStkUOM = grid.GetEditor("StockUOM");
    //var tbStockQuantity = grid.GetEditor("StockQuantity");
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];

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
    divPacking.style.display = "none";
    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    //Debjyoti
    // ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);

    deleteTax('DelProdbySl' , grid.GetEditor("SrlNo").GetValue() , strProductID)


    //cacpAvailableStock.PerformCallback('MainAviableStockBind' + '~' + strProductID);

    grid.batchEditApi.StartEdit(globalRowIndex, 6);
}
$(document).ready(function () {
    $("#Cross_CloseWindow a").click(function (e) {
        e.preventDefault();
        window.close();
    });
});

//<%--Batch Product Popup End--%>
//<%-- Date: 30-05-2017    Author: Kallol Samanta  [START] --%>
//<%-- Details: Billing/Shipping user control integration --%>
function SettingTabStatus() {
    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
    }
}


function GetSalesRateSchemePrice(CustomerID, ProductID, SalesPrice) {

    var date = new Date;
    var seconds = date.getSeconds();
    var minutes = date.getMinutes();
    var hour = date.getHours();

    var times = hour + ':' + minutes;

    var sdate = cPLSalesOrderDate.GetValue();
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
            // Mantis Issue 24397
            //console.log(returnObject);
            // End of Mantis Issue 24397
            if (returnObject.length > 0) {
               
                $("#ProductMinPrice").val(returnObject[0].MinSalePrice);
                $("#ProductMaxPrice").val(returnObject[0].MaxSalePrice);
                $("#hdnRateType").val(returnObject[0].RateType);
            }
                //Rev work start 28.06.2022 Mantise no:24949
            else
            {
                
                $("#ProductMinPrice").val(0);
                $("#ProductMaxPrice").val(0);
                $("#hdnRateType").val(0);
            }
            setValueOfSAale();            
            //Rev work close 28.06.2022 Mantise no:24949
        }
    });
}

function Segment1ButnClick(s, e) {
    if($("#hdnCustomerId").val()!="")
    {
        $('#Segment1Model').modal('show');
    }
    else{
        jAlert("Please Select Customer");
    }
        
}
function Segment1keydown(e) {
            
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment1Search").val();
    OtherDetails.CustomerIds=$("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment1", OtherDetails, "Segment1Table", HeaderCaption, "segment1Index", "Setsegment1");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment1Index=0]"))
            $("input[segment1Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment1.Focus();
    }
}

function Segment1_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment1Model').modal('show');
        $("#txtSegment1Search").focus();
    }
}

function Setsegment1(Id, Name,e) {           
   
    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment1Model').modal('hide');
    ctxtSegment1.SetText(ProductCode);
    $('#hdnSegment1').val(LookUpData);
    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(),Id);

    if($('#hdnValueSegment2').val()=="1")
    {
        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment2Search").val();
        OtherDetails.CustomerIds=$("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");   
        HeaderCaption.push("Name");   
        callonServer("Services/Master.asmx/GetSegment2", OtherDetails, "Segment2Table", HeaderCaption, "segment2Index", "Setsegment2");
        $('#Segment2Model').modal('show');
    }

    
    
    
}
function Segment2ButnClick(s, e) {
    if($("#hdnCustomerId").val()!="")
    {
        $('#Segment2Model').modal('show');    
    }    
    else{
        jAlert("Please Select Customer");
    }
}
function Segment2keydown(e) {
            
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment1Search").val();
    OtherDetails.CustomerIds=$("#hdnCustomerId").val();

    

    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code"); 
        HeaderCaption.push("Name");   
        callonServer("Services/Master.asmx/GetSegment2", OtherDetails, "Segment2Table", HeaderCaption, "segment2Index", "Setsegment2");
    }
else 
    if (e.code == "ArrowDown") {
        if ($("input[segment2Index=0]"))
            $("input[segment2Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment2.Focus();
    }
}
function Segment2_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment2Model').modal('show');
        $("#txtSegment2Search").focus();
    }
}
function Setsegment2(Id, Name,e) {           
   
    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment2Model').modal('hide');
    ctxtSegment2.SetText(ProductCode);
    $('#hdnSegment2').val(LookUpData);

    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(),Id);
    if($('#hdnValueSegment3').val()=="1")
    {
        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment1Search").val();
        OtherDetails.CustomerIds=$("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment3", OtherDetails, "Segment3Table", HeaderCaption, "segment3Index", "Setsegment3");
        $('#Segment3Model').modal('show');
    }
    
    
}
function Segment3ButnClick(s, e) {
    if($("#hdnCustomerId").val()!="")
    {
        $('#Segment3Model').modal('show'); 
    }    
    else{
        jAlert("Please Select Customer");
    }
        
}
function Segment3keydown(e) {
            
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment3Search").val();
    OtherDetails.CustomerIds=$("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment3", OtherDetails, "Segment3Table", HeaderCaption, "segment3Index", "Setsegment3");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment3Index=0]"))
            $("input[segment3Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment3.Focus();
    }
}
function Setsegment3(Id, Name,e) {           
   
    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment3Model').modal('hide');
    ctxtSegment3.SetText(ProductCode);
    $('#hdnSegment3').val(LookUpData);

    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(),Id);
    if($('#hdnValueSegment4').val()=="1")
    {
        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment4Search").val();
        OtherDetails.CustomerIds=$("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");  
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment4", OtherDetails, "Segment4Table", HeaderCaption, "segment4Index", "Setsegment4");
        $('#Segment4Model').modal('show');
    }
    
    
}
function Segment3_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment3Model').modal('show');
        $("#txtSegment3Search").focus();
    }
}
function Segment4ButnClick(s, e) {
    if($("#hdnCustomerId").val()!="")
    {
        $('#Segment4Model').modal('show');    
    }    
    else{
        jAlert("Please Select Customer");
    }
    
}
function Segment4keydown(e) {
            
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment4Search").val();
    OtherDetails.CustomerIds=$("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code"); 
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment4", OtherDetails, "Segment4Table", HeaderCaption, "segment4Index", "Setsegment4");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment4Index=0]"))
            $("input[segment4Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment4.Focus();
    }
}
function Setsegment4(Id, Name,e) {           
   
    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment4Model').modal('hide');
    ctxtSegment4.SetText(ProductCode);
    $('#hdnSegment4').val(LookUpData);
    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(),Id);
    if($('#hdnValueSegment5').val()=="1")
    {

        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtSegment5Search").val();
        OtherDetails.CustomerIds=$("#hdnCustomerId").val();
        var HeaderCaption = [];
        HeaderCaption.push("Code");   
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment5", OtherDetails, "Segment5Table", HeaderCaption, "segment5Index", "Setsegment5");
        $('#Segment5Model').modal('show');
    }
    
    
}
function Segment4_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment4Model').modal('show');
        $("#txtSegment4Search").focus();
    }
}
function Segment5_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#Segment5Model').modal('show');
        $("#txtSegment5Search").focus();
    }
}
function Segment5ButnClick(s, e) {
    if($("#hdnCustomerId").val()!="")
    {
        $('#Segment5Model').modal('show'); 
    }    
    else{
        jAlert("Please Select Customer");
    }
       
}
function Setsegment5(Id, Name,e) {           
   
    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    $('#Segment5Model').modal('hide');
    ctxtSegment5.SetText(ProductCode);
    $('#hdnSegment5').val(LookUpData);
    
    SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(),Id);
}
function Segment5keydown(e) {
            
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSegment5Search").val();
    OtherDetails.CustomerIds=$("#hdnCustomerId").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code"); 
        HeaderCaption.push("Name");
        callonServer("Services/Master.asmx/GetSegment5", OtherDetails, "Segment5Table", HeaderCaption, "segment5Index", "Setsegment5");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[segment5Index=0]"))
            $("input[segment5Index=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtSegment5.Focus();
    }
}

$(document).ready(function () {

    var mode = $('#hdAddOrEdit').val();
    if (mode == 'Edit') {
        if ($("#hddnDocumentIdTagged").val()!="1") {
            page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        }
        else
        {
            page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
        }
	
        if (getUrlVars().req != "V" && $("#hddnDocumentIdTagged").val()=="1") 
        {
            page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
        }
        else
        {
            page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        }
    
    }
});
