validateInvoiceNumber = function () {
    var invoiceNumber = $("#SearchInv").val();
    if (invoiceNumber == "") {
        return;
    }
    var jsonData = {};
    jsonData.invoiceNumber = invoiceNumber;
    $.ajax({
        type: "POST",
        url: "PosSalesinvoiceAdminEdit.aspx/GetInvoiceDetails",
        data: JSON.stringify(jsonData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {

                if (obj.invoiceId == "")
                    ShowMsg("This Invoice Number Does not exists");
                else
                    window.location.href = 'PosSalesinvoiceAdminEdit.aspx?id=' + obj.invoiceId;

            } else {
                console.log(obj.Msg);
            }
        },
        Error: function (x, e) {
        }
    });


}

ShowMsg = function (msg) {
    jAlert(msg);
}

UpdateClientClick = function () {
    if ($("#UpdateField").val() == "PaymnetDetails") {
        SelectAllData();
    } 
    if ($("#UpdateField").val() == "changeglobaltax") {
        Save_TaxClick();
    }
}
function gridTax_EndCallback()
{

}


$(document).ready(function () {
    hideAllcontrol();
    $('#UpdateField').change(function () {
        var UpdatedField = $(this).val();
        hideAllcontrol();
        if (UpdatedField == "docNo") {
            $("#divInvoiceNumber").show();
        }
        else if (UpdatedField == "PaymnetDetails") {
            $("#divPaymentDetails").show();
        }
        else if (UpdatedField == "BillingShipping") {
            $("#divBillingShipping").show();
        }
        else if (UpdatedField == "FinanceBlock") {
            $("#divFinanceBlock").show();
        }
        else if (UpdatedField == "Salesman") {
            //var element = document.getElementById("cddl_SalesAgent");
            //element.value = "42600";
            GetAllDetailsByBranch();

            $("#divsalesman").show();
        }
        else if (UpdatedField == "PostingDateofDeliver") {
            $("#divpostingdate").show();
        }
        else if (UpdatedField == "Typecashtocredit") {
            $("#divcashtocredit").show();
        }
        else if (UpdatedField == "Customer") {
            $("#divcustomer").show();
        }
        else if (UpdatedField == "Branch") {
            $("#divbranch").show();
        }
        else if (UpdatedField == "NoCommission") {
            $("#divNoCommission").show();
        }
        else if (UpdatedField == "changeglobaltax") {
            // cPopup_Taxes.Show();
            $("#changeglobaltaxdiv").show();
            //ccmbGstCstVatcharge.PerformCallback(t);
            //clblRunningBalanceCapsul.SetVisible(false);
            GetsalesinvoiceproductsDetails();
            Save_TaxesClick();
            ccmbGstCstVatcharge.PerformCallback();
            gridTax.PerformCallback();
            ctxtQuoteTaxTotalAmt.SetValue(cbnrLblotherchargesVal);
            //alert(ddl_AmountAre.GetValue());
            
        }
        if (cddl_SalesAgent.InCallback())
            lastSalesman = branchID;
        // else
        //  cddl_SalesAgent.PerformCallback(branchID);
        // GetAllDetailsByBranch();
    });
});
function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cbnrOtherChargesvalue.SetText('0.00');
    SetInvoiceLebelValue();
    cPopup_Taxes.Hide();
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
    if (Percentage > 100)
    {
        jAlert("Percentage can't be greater than 100");
        Percentage = 0;

    }
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
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}

function RecalCulateTaxTotalAmountCharges() {
    var totalTaxAmount = 0;
    for (var i = 0; i < chargejsonTax.length; i++) {

        if (chargejsonTax[i].SchemeName != "-Select-") {
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
    }

    totalTaxAmount = totalTaxAmount + parseFloat(ctxtGstCstVatCharge.GetValue());

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

function GetsalesinvoiceproductsDetails()
{
    var OtherDetails = {}
    var urlKeys = getUrlVars();
    OtherDetails.invoiceid = urlKeys.id;//$('#ddl_Branch').val();
    //OtherDetails.EntryType = 0; //$('#HdPosType').val();
    //OtherDetails.Invoiceid = urlKeys.id;
    $.ajax({
        type: "POST",
        url: "PosSalesinvoiceAdminEdit.aspx/GetsalesinvoiceproductsDetails",
        data: JSON.stringify(OtherDetails),
       // data: "{'list':'" + JSON.stringify(OtherDetails) + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d.DetailsofInvoice;
            cbnrLblTotalQty.SetText(returnObject[0].TotalQuantity);
            cbnrLblTaxableAmtval.SetText(returnObject[0].TaxableAmount);
            cbnrLblTaxAmtval.SetText(returnObject[0].TaxAmount);
            cbnrlblAmountWithTaxValue.SetText(returnObject[0].AmountWithTax);
            ctxtProductNetAmount.SetText(returnObject[0].TaxableAmount);
            ctxtProductAmount.SetText(returnObject[0].AmountWithTax);
            ctxtProductTaxAmount.SetText(returnObject[0].TaxAmount);
         //   alert(document.getElementById("hdnposforgst").value);
        },
        error: function (msg) {
            alert(msg);
        }
    });
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

    //Set Total Charges And total Amount
    if (gridTax.cpTotalCharges) {
        if (gridTax.cpTotalCharges != "") {
            ctxtQuoteTaxTotalAmt.SetValue(gridTax.cpTotalCharges);
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
            gridTax.cpTotalCharges = null;
        }
    }
     if (gridTax.cpfinalMsg == "1")
    {
         jAlert("Global Tax Updated Successfully");
    }
    SetOtherChargesLbl();
    SetInvoiceLebelValue();
    SetChargesRunningTotal();
    ShowTaxPopUp("IN");
    RecalCulateTaxTotalAmountCharges();

}
function SetOtherChargesLbl() {
    var finalOtherCharges = parseFloat(Math.round(ctxtQuoteTaxTotalAmt.GetValue() * 100) / 100).toFixed(2);
    if (finalOtherCharges == 0) {
        $('#otherChargesId').hide();
    } else {
        $('#otherChargesId').show();
    }
    cbnrOtherChargesvalue.SetValue(finalOtherCharges);
    SetRunningBalance();
}

function SetRunningBalance() {
    var paymentValue = 0;
    if (document.getElementById('HdPosType').value != 'IST') {
        paymentValue = parseFloat(GetPaymentTotalEnteredAmount());
    }
    //  SetDownPayment();
    if (document.getElementById('HdPosType').value == 'Fin') {
        SetTotalDownPaymentAmount();
    }
    var InvoiceValue = parseFloat(cbnrLblInvValue.GetValue());
    //var FinanceAmount = parseFloat(ctxtFinanceAmt.GetValue());
    var FinanceAmount = parseFloat(0);
    var otherCharges = parseFloat(cbnrOtherChargesvalue.GetValue());
    //var procFee = parseFloat(ctxtprocFee.GetValue());
    var procFee = parseFloat(0);
    //var EmiCardOtCharge = parseFloat(ctxtEmiOtherCharges.GetValue());
    var EmiCardOtCharge = parseFloat(0);

    var runningBalance = 0;
    runningBalance = parseFloat(Math.round((InvoiceValue - paymentValue - FinanceAmount) * 100) / 100).toFixed(2);

    if (document.getElementById('HdPosType').value == 'Fin') {

        if (parseFloat(Math.round((InvoiceValue - paymentValue - FinanceAmount) * 100) / 100) < parseFloat(ctxtAdvnceReceipt.GetValue())) {
            if (parseFloat(ctxtAdvnceReceipt.GetValue()) > 0)
                runningBalance = 0.00;
            else
                runningBalance = runningBalance - parseFloat(ctxtAdvnceReceipt.GetValue());
        } else {
            runningBalance = runningBalance - parseFloat(ctxtAdvnceReceipt.GetValue());
        }
    }

    //Rev Rajdip
    var runbal = ctxtProductTaxAmount.GetText();
    var invoiceval = cbnrLblInvValue.GetText();
    var runval = parseFloat(runbal) + parseFloat(invoiceval);
    // clblRunningBalanceCapsul.SetValue(runningBalance);
    clblRunningBalanceCapsul.SetValue(runval);
    //clblRunningBalanceCapsul.SetValue(runval);
    //End Rev Rajdip
}
function SetInvoiceLebelValue() {

    var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());

    if (document.getElementById('HdPosType').value == 'Crd') {
        if (invValue < 0) {
            var newAdvAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
            cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(newAdvAmount) * 100) / 100).toFixed(2));
        }
    }

    if (document.getElementById('HdPosType').value == 'Fin') {
        if (invValue < 0) {
            var newAdvAmountfin = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
            cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(ctxtdownPayment.GetValue()) * 100) / 100).toFixed(2));
        }
    }



    if (document.getElementById('HdPosType').value == 'Crd')
        invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
    else if (document.getElementById('HdPosType').value == 'Fin')
        invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue()) + parseFloat(cbnrOtherChargesvalue.GetValue());


    cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));


    SetRunningBalance();

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

//Rev Rajdip
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






function Save_TaxesClick() {
  
    //commented
    // grid.batchEditApi.EndEdit();
    //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    //var i, cnt = 1;
    var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

    //cnt = 1;
    //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
    //    var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
    //    var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
    //    var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
    //    var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
    //    var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

    //    sumAmount = sumAmount + parseFloat(Amount);
    //    sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
    //    sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
    //    sumNetAmount = sumNetAmount + parseFloat(NetAmount);

    //    cnt++;
    //}

    //if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
    //    cnt = 1;
    //    for (i = 0 ; cnt <= noofvisiblerows ; i++) {
    //        var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
    //        var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
    //        var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
    //        var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
    //        var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

    //        sumAmount = sumAmount + parseFloat(Amount);
    //        sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
    //        sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
    //        sumNetAmount = sumNetAmount + parseFloat(NetAmount);

    //        cnt++;
    //    }
    //}

    ////Debjyoti 
    //document.getElementById('HdChargeProdAmt').value = sumAmount;
    document.getElementById('HdChargeProdNetAmt').value = ctxtProductNetAmount.GetText();
    //End Here

    //ctxtProductAmount.SetValue((Math.round(sumAmount * 100) / 100).toFixed(2));
    //ctxtProductTaxAmount.SetValue((Math.round(sumTaxAmount * 100) / 100).toFixed(2));
    //ctxtProductDiscount.SetValue((Math.round(sumDiscount * 100) / 100).toFixed(2));
    //ctxtProductNetAmount.SetValue((Math.round(sumNetAmount * 100) / 100).toFixed(2));
    //clblChargesTaxableGross.SetText("");
    //clblChargesTaxableNet.SetText("");

    ////Checking is gstcstvat will be hidden or not
    //if (cddl_AmountAre.GetValue() == "2") {

    //    $('.lblChargesGSTforGross').show();
    //    $('.lblChargesGSTforNet').show();

    //    //Set Gross Amount with GstValue
    //    //Get The rate of Gst
    //    var gstRate = 0;
    //    if (gstRate) {
    //        if (gstRate != 0) {
    //            var gstDis = (gstRate / 100) + 1;
    //            if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
    //                $('.lblChargesGSTforNet').hide();
    //                ctxtProductAmount.SetText(Math.round(sumAmount / gstDis).toFixed(2));
    //                document.getElementById('HdChargeProdAmt').value = Math.round(sumAmount / gstDis).toFixed(2);
    //                clblChargesGSTforGross.SetText(Math.round(sumAmount - parseFloat(document.getElementById('HdChargeProdAmt').value)).toFixed(2));
    //                clblChargesTaxableGross.SetText("(Taxable)");

    //            }
    //            else {
    //                $('.lblChargesGSTforGross').hide();
    //                ctxtProductNetAmount.SetText(Math.round(sumNetAmount / gstDis).toFixed(2));
    //                document.getElementById('HdChargeProdNetAmt').value = Math.round(sumNetAmount / gstDis).toFixed(2);
    //                clblChargesGSTforNet.SetText(Math.round(sumNetAmount - parseFloat(document.getElementById('HdChargeProdNetAmt').value)).toFixed(2));
    //                clblChargesTaxableNet.SetText("(Taxable)");
    //            }
    //        }

    //    } else {
    //        $('.lblChargesGSTforGross').hide();
    //        $('.lblChargesGSTforNet').hide();
    //    }
    //}
    //else if (cddl_AmountAre.GetValue() == "1") {
    //    $('.lblChargesGSTforGross').hide();
    //    $('.lblChargesGSTforNet').hide();

    //    //Debjyoti 09032017
    //    for (var cmbCount = 1; cmbCount < ccmbGstCstVatcharge.GetItemCount() ; cmbCount++) {
    //        if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[5] == '19') {
    //            if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'I') {
    //                ccmbGstCstVatcharge.RemoveItem(cmbCount);
    //                cmbCount--;
    //            }
    //        } else {
    //            if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'C') {
    //                ccmbGstCstVatcharge.RemoveItem(cmbCount);
    //                cmbCount--;
    //            }
    //        }
    //    }






    //}
    ////End here





    ////Set Total amount
    //ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));

    //gridTax.PerformCallback('Display');
    ////Checking is gstcstvat will be hidden or not
    //if (cddl_AmountAre.GetValue() == "2") {
    //    $('.chargeGstCstvatClass').hide();
    //}
    //else if (cddl_AmountAre.GetValue() == "1") {
    //    $('.chargeGstCstvatClass').show();
    //}
    ////End here
    //$('.RecalculateCharge').hide();
    //cPopup_Taxes.Show();
    //gridTax.StartEditRow(0);
}












//End Rev Rajdip
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

function GetAllDetailsByBranch() {

    var urlKeys = getUrlVars();
    var OtherDetails = {}
    OtherDetails.BranchId = 1;//$('#ddl_Branch').val();
    OtherDetails.EntryType = 0; //$('#HdPosType').val();
    OtherDetails.Invoiceid = urlKeys.id;
    $.ajax({
        type: "POST",
        url: "PosSalesinvoiceAdminEdit.aspx/GetAllDetailsByBranch",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;

            if (returnObject.SalesMan) {
                SetDataSourceOnComboBox(cddl_SalesAgent, returnObject.SalesMan);
                document.getElementById("ddl_SalesAgent").selectedIndex = "0";
            }
            var hiddenvalue = document.getElementById("hddnsalesmanId").value;
            if (hiddenvalue == "") {
                cddl_SalesAgent.SetValue(0);
            }
            else { 
                cddl_SalesAgent.SetValue(hiddenvalue);
            }
            //if (returnObject.ChallanNumberScheme) {
            //    SetDataSourceOnComboBox(cchallanNoScheme, returnObject.ChallanNumberScheme);
            //}

            //if (returnObject.Financer) {
            //    SetDataSourceOnComboBox(ccmbFinancer, returnObject.Financer);
            //    ccmbFinancer.SetSelectedIndex(-1);
            //    financerDetails = returnObject.Financer;

            //}

            //if (returnObject.Executive) {
            //    executiveList = returnObject.Executive;
            //}
            //financerIndexChange();
            //ccmbExecName.SetValue('');
        }
    });

}
function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    // ControlObject.SetSelectedIndex(0);
}
hideAllcontrol = function () {
    $("#divInvoiceNumber").hide();
    $("#divPaymentDetails").hide();
    $("#divBillingShipping").hide();
    $("#divFinanceBlock").hide();
    $("#divsalesman").hide();
    $("#divpostingdate").hide();
    $("#divcashtocredit").hide();
    $("#divcustomer").hide();
    $("#divbranch").hide();
    $("#divNoCommission").hide();
    $("#changeglobaltaxdiv").hide();
}

SearchManualReceipt = function () {

    cManualReceipt.PerformCallback('validateReceiptNumber');

}

ShowManualReceiptPopup = function () {
    cManualReceipt.Show();
}

UpdateManualReceipt = function () {
    cManualReceipt.PerformCallback('UpdateManualReceipt');
    return false;
}


CopyToshipping = function () {
    $("#txtShippingAddress1").val($("#txtBillingAddress1").val());
    $("#txtShippingAddress2").val($("#txtBillingAddress2").val());
    $("#txtShippingAddress3").val($("#txtBillingAddress3").val());
    $("#txtShippingLandmark").val($("#txtBillingLandMark").val());
    $("#txtShippingPin").val($("#txtBillingPin").val());
}

CopyToBilling = function () {
    $("#txtBillingAddress1").val($("#txtShippingAddress1").val());
    $("#txtBillingAddress2").val($("#txtShippingAddress2").val());
    $("#txtBillingAddress3").val($("#txtShippingAddress3").val());
    $("#txtBillingLandMark").val($("#txtShippingLandmark").val());
    $("#txtBillingPin").val($("#txtShippingPin").val());
}
//Rev Rajdip
function OnSalesAgentEndCallback(s, e) {
    if (lastSalesman) {
        cddl_SalesAgent.PerformCallback(lastSalesman);
        cddl_SalesAgent.SetValue(42600);
        //lastSalesman = null;
    } else {
        cddl_SalesAgent.SetValue(42600);
        cmbUcpaymentCashLedgerChanged(ccmbUcpaymentCashLedger);
    }
}
function AddcustomerClick() {
    var isLighterPage = $("#hidIsLigherContactPage").val();
    debugger;
    if (isLighterPage == 1) {
        var url = '/OMS/management/Master/customerPopup.html?var=1.1.3.8';
        AspxDirectAddCustPopup.SetContentUrl(url);
        //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();
        AspxDirectAddCustPopup.RefreshContentUrl();
        AspxDirectAddCustPopup.Show();
    }
    else {
        var url = '';
        //var url = '/OMS/management/Master/Contact_general.aspx?id=' + 'ADD';
        var url = '/OMS/management/Master/Customer_general.aspx';
        // window.location.href = url;
        AspxDirectAddCustPopup.SetContentUrl(url);
        AspxDirectAddCustPopup.Show();
    }
}
function ParentCustomerOnClose(newCustId, CustomerName, CustUniqueName, BillingStateText, BillingStateCode, ShippingStateText, ShippingStateCode) {
    AspxDirectAddCustPopup.Hide();
    if (newCustId.trim() != '') {
        page.SetActiveTabIndex(0);
        GetObjectID('hdnCustomerId').value = newCustId;

        GetObjectID('lblBillingStateText').value = BillingStateText;
        GetObjectID('lblBillingStateValue').value = BillingStateCode;

        GetObjectID('lblShippingStateText').value = ShippingStateText;
        GetObjectID('lblShippingStateValue').value = ShippingStateCode;

        var FullName = new Array(CustUniqueName, CustomerName);
        ctxtCustName.SetText(CustomerName);
        $('#DeleteCustomer').val("yes");
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        loadAddressbyCustomerID(newCustId);
        cddl_SalesAgent.Focus();

    }
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
        if ($("#txtCustSearch").val() != '') {
            callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }

}
function SetCustomer(Id, Name) {
    if (Id) {
        $('#CustModel').modal('hide');
        ctxtCustName.SetText(Name);
        GetObjectID('hdnCustomerId').value = Id;
        GetObjectID('hdnAddressDtl').value = '0';
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
        var startDate = new Date();
        startDate = tstartdate.GetValueString();

        if (type != "") {
            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + Id + '~' + startDate + '~' + '%' + '~' + type);
        }

        var componentType = gridquotationLookup.GetValue();
        if (componentType != null && componentType != '') {
            grid.PerformCallback('GridBlank');
            $('#<%=hdnPageStatus.ClientID %>').val('update');
        }

        //GetObjectID('hdnCustomerId').value = Id;
        //GetObjectID('hdnAddressDtl').value = '0';

        page.SetActiveTabIndex(1);
        cbtnSave_citys.Focus();
        loadAddressbyCustomerID(Id);
        $('.dxeErrorCellSys').addClass('abc');
        $('.crossBtn').hide();
        page.GetTabByName('General').SetEnabled(false);
        $('#CustModel').modal('hide');
        ShowCustomerBalance();
    }
}
function validatepostingdate() {
    var Postingdate = document.getElementById("dt_PLQuote").value;
    if (Postingdate == "") {
        alert('Please Enter Posting Date');
        return;
    }
}
//End Rev Rajdip