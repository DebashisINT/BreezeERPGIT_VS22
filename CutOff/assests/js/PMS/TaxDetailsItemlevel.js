var ItemLevelTaxDetails = [];
var HSNCodewiseTaxSchem = [];
var BranchWiseStateTax = [];
var StateCodeWiseStateIDTax = [];
var isTaxLoaded = false;


$(document).ready(function () {

    CallServerToloadtax();
    //if (!localStorage.getItem("TaxDetailsForSale")) {
    //    CallServerToloadtax();

    //} else {

    //    var storagetaxSchemeUpdatedDate = localStorage.getItem("taxSchemeUpdatedDate");

    //    if (storagetaxSchemeUpdatedDate == taxSchemeUpdatedDate) {
    //        var returnObject = JSON.parse(localStorage.getItem("TaxDetailsForSale"));
    //        ItemLevelTaxDetails = returnObject.ItemLevelTaxDetails;
    //        HSNCodewiseTaxSchem = returnObject.HSNCodewiseTaxSchem;
    //        BranchWiseStateTax = returnObject.BranchWiseStateTax;
    //        StateCodeWiseStateIDTax = returnObject.StateCodeWiseStateIDTax;
    //        isTaxLoaded = true;
    //    } else {
    //        CallServerToloadtax();
    //    }
    //}
});

CallServerToloadtax = function () {
    
    $.ajax({
        type: "POST",
        // url: "../Models/TaxService.asmx/GetTaxDetailsForSale",
        url: "../Estimate/GetTaxDetailsForSale",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //alert(111111);
            var returnObject = response[0];

          
            localStorage.setItem("TaxDetailsForSale", JSON.stringify(returnObject));
            ItemLevelTaxDetails = returnObject.ItemLevelTaxDetails;
            HSNCodewiseTaxSchem = returnObject.HSNCodewiseTaxSchem;
            BranchWiseStateTax = returnObject.BranchWiseStateTax;
            StateCodeWiseStateIDTax = returnObject.StateCodeWiseStateIDTax;

            //localStorage.setItem("taxSchemeUpdatedDate", taxSchemeUpdatedDate);
            isTaxLoaded = true;
        },
        error: function (response) {
            alert(response);
        }
    });
}


caluculateAndSetGST = function (AmountEditor, chargesEditor, TotalAmountEditor, HSNCode, Amount, AmountAfterDiscount, inclsOrExclsv, ShippingState, branchId, Module) {
    debugger;
    var GrosstotalAmount = Amount;
    var NettotalAmount = AmountAfterDiscount;

    var shippingStateCode = FindStateCodeByStateID(ShippingState);
    //    var compStateCode = FindStateCodeByStateID(FindStateIDByBranchID(branchId));

    if (Module && Module == "P") {
        shippingStateCode = "";
        if ($('#hfVendorGSTIN').val() != "") {

            shippingStateCode = $('#hfVendorGSTIN').val().substr(0, 2);
        }
    }

    if (shippingStateCode == "") {
        return;
    }



    var compObject = $.grep(BranchWiseStateTax, function (e) { return e.branch_id == branchId; })
    var compGSTIN = "19";
    if (compGSTIN == "") {
        return;
    }

    var compStateCode = compGSTIN.substr(0, 2);

    var TaxTypeMode = '';
    if (shippingStateCode != compStateCode) {
        TaxTypeMode = "IGST";
    } else {
        TaxTypeMode = "SGST";
        //Remove shippingStateCode == "35" as now Delhi is not under UTGST
        if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "7" || shippingStateCode == "31" || shippingStateCode == "34") {
            TaxTypeMode = "UTGST";
        }
    }


    var HSNObject = $.grep(HSNCodewiseTaxSchem, function (e) { return e.HSNCODE == HSNCode; })



    if (HSNObject.length > 0) {
        var taxes = HSNObject[0].config_TaxRatesIDs;
        var TaxChargesAmount = 0;
        var totalTaxchargesRate = 0;
        var TaxApplicableOn = '';
        var arrayTaxchargesRate = [];

        for (var taxCount = 0; taxCount < taxes.length; taxCount++) {

            //  var backProcessRate = (1 + (taxes[taxCount].Rate / 100));
            if (TaxTypeMode == "IGST") {
                if (taxes[taxCount].TaxTypeCode == "IGST") {

                    if (taxes[taxCount].Taxes_ApplicableOn == "G") {
                        TaxApplicableOn = "G";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                        arrayTaxchargesRate.push(taxes[taxCount].Rate);
                        // TaxChargesAmount = TaxChargesAmount + (GrosstotalAmount - (GrosstotalAmount / backProcessRate));
                    }
                    else if (taxes[taxCount].Taxes_ApplicableOn == "N") {
                        //TaxChargesAmount = TaxChargesAmount + (NettotalAmount - (NettotalAmount / backProcessRate));
                        TaxApplicableOn = "N";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                        arrayTaxchargesRate.push(taxes[taxCount].Rate);
                    }
                }
            }


            else if (TaxTypeMode == "UTGST") {

                if (taxes[taxCount].TaxTypeCode == "UTGST" || taxes[taxCount].TaxTypeCode == "CGST") {

                    if (taxes[taxCount].Taxes_ApplicableOn == "G") {
                        TaxApplicableOn = "G";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                        arrayTaxchargesRate.push(taxes[taxCount].Rate);
                    }
                    else if (taxes[taxCount].Taxes_ApplicableOn == "N") {
                        TaxApplicableOn = "N";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                        arrayTaxchargesRate.push(taxes[taxCount].Rate);
                    }
                }
            }

            else if (TaxTypeMode == "SGST") {

                if (taxes[taxCount].TaxTypeCode == "SGST" || taxes[taxCount].TaxTypeCode == "CGST") {

                    if (taxes[taxCount].Taxes_ApplicableOn == "G") {
                        TaxApplicableOn = "G";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                        arrayTaxchargesRate.push(taxes[taxCount].Rate);
                    }
                    else if (taxes[taxCount].Taxes_ApplicableOn == "N") {
                        TaxApplicableOn = "N";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                        arrayTaxchargesRate.push(taxes[taxCount].Rate);
                    }
                }
            }

        }

        if (inclsOrExclsv == "I") {

            var backProcessRate = (1 + (totalTaxchargesRate / 100));
            if (TaxApplicableOn == "G") {
                TaxChargesAmount = (GrosstotalAmount - (GrosstotalAmount / backProcessRate));
            } else if (TaxApplicableOn == "N") {
                TaxChargesAmount = (NettotalAmount - (NettotalAmount / backProcessRate));
            }

            // TaxChargesAmount = parseFloat(Math.round(Math.abs(Math.round(TaxChargesAmount)) * 100) / 100).toFixed(2);
            TaxChargesAmount = parseFloat((Math.abs(TaxChargesAmount) * 100) / 100).toFixed(2);
            var InClusiveAmount = parseFloat(Math.round(Math.abs(NettotalAmount - TaxChargesAmount) * 100) / 100).toFixed(2)

            //AmountEditor.SetValue(NettotalAmount - TaxChargesAmount);
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex)
            //AmountEditor.SetValue(InClusiveAmount);
            //chargesEditor.SetText(TaxChargesAmount);
            //TotalAmountEditor.SetValue(NettotalAmount);
            return InClusiveAmount + '~' + TaxChargesAmount + '~' + NettotalAmount;
        }
        else if (inclsOrExclsv == "E") {

            var backProcessRate = (totalTaxchargesRate / 100);


            // Code Added By Sam on 20122017 Due to Miscalculation in case of Exclusive Section Start


            var chargesAmount = 0;
            if (TaxApplicableOn == "G") {
                //TaxChargesAmount = (GrosstotalAmount * backProcessRate);
                for (var i = 0; i < arrayTaxchargesRate.length; i++) {
                    TaxChargesAmount = TaxChargesAmount + RoundUp((GrosstotalAmount * (arrayTaxchargesRate[i] / 100)), 2);
                }
            }
            else if (TaxApplicableOn == "N") {
                // TaxChargesAmount = (NettotalAmount * backProcessRate);
                for (var i = 0; i < arrayTaxchargesRate.length; i++) {
                    TaxChargesAmount = TaxChargesAmount + RoundUp((NettotalAmount * (arrayTaxchargesRate[i] / 100)), 2);
                }
            }
            TaxChargesAmount = parseFloat((Math.abs(TaxChargesAmount) * 100) / 100).toFixed(2);

            //var chargesAmount = NettotalAmount * backProcessRate;
            //TaxChargesAmount = parseFloat((Math.abs(chargesAmount) * 100) / 100).toFixed(2);
            // Code Added By Sam on 20122017 Due to Miscalculation in case of Exclusive Section End

            //TaxChargesAmount = parseFloat(Math.round(Math.abs(Math.round(chargesAmount)) * 100) / 100).toFixed(2);



            //AmountEditor.SetValue(NettotalAmount);
            //chargesEditor.SetValue(TaxChargesAmount);
            //TotalAmountEditor.SetValue(DecimalRoundoff(parseFloat(NettotalAmount) + parseFloat(TaxChargesAmount), 2));
            return NettotalAmount + '~' + TaxChargesAmount + '~' + DecimalRoundoff(parseFloat(NettotalAmount) + parseFloat(TaxChargesAmount), 2);
        }



        //Set Actual tax for tax charges from Calculated Amount

        //var chargesAmount = 0;

        //var FinalAmount = AmountEditor.GetValue();
        //for (var taxCount = 0; taxCount < taxes.length; taxCount++) {


        //    if (TaxTypeMode == "IGST") { 
        //        if (taxes[taxCount].TaxTypeCode == "IGST") {

        //            chargesAmount = chargesAmount +parseFloat( parseFloat(Math.floor(Math.abs((FinalAmount * (taxes[taxCount].Rate / 100))) * 100) / 100).toFixed(2));
        //        }
        //    }


        //    else if (TaxTypeMode == "UTGST") {

        //        if (taxes[taxCount].TaxTypeCode == "UTGST" || taxes[taxCount].TaxTypeCode == "CGST") {

        //            chargesAmount = chargesAmount + parseFloat(parseFloat(Math.floor(Math.abs((FinalAmount * (taxes[taxCount].Rate / 100))) * 100) / 100).toFixed(2));
        //        }
        //    }

        //    else if (TaxTypeMode == "SGST") {

        //        if (taxes[taxCount].TaxTypeCode == "SGST" || taxes[taxCount].TaxTypeCode == "CGST") {

        //            chargesAmount = chargesAmount + parseFloat(parseFloat(Math.floor(Math.abs((FinalAmount * (taxes[taxCount].Rate / 100))) * 100) / 100).toFixed(2));
        //        }
        //    }

        //}




        //if (inclsOrExclsv == "I") {


        //    TaxChargesAmount = parseFloat(Math.floor(Math.abs(chargesAmount) * 100) / 100).toFixed(2);
        //    InClusiveAmount = parseFloat(Math.round(Math.abs(NettotalAmount - TaxChargesAmount) * 100) / 100).toFixed(2);

        //    AmountEditor.SetValue(InClusiveAmount);
        //    chargesEditor.SetValue(TaxChargesAmount);
        //    TotalAmountEditor.SetValue(NettotalAmount);
        //} 



        return InClusiveAmount + '~' + TaxChargesAmount;//+ '~' + NettotalAmount;

    }
    else {
        return AmountAfterDiscount + '~' + "0.00" + '~' + AmountAfterDiscount;
    }

}


caluculateAndSetGSTforOldUnit = function (AmountEditor, chargesEditor, TotalAmountEditor, HSNCode, Amount, AmountAfterDiscount, inclsOrExclsv, ShippingState, branchId, AvgValue) {


    var GrosstotalAmount = Amount;
    var NettotalAmount = AmountAfterDiscount;

    var shippingStateCode = FindStateCodeByStateID(ShippingState);


    if (Amount < AvgValue) {
        chargesEditor.SetValue("0.00");
        TotalAmountEditor.SetValue(AmountEditor.GetValue());
        return;
    }

    var compObject = $.grep(BranchWiseStateTax, function (e) { return e.branch_id == branchId; })
    var compGSTIN = "";
    if (compObject[0].BranchGSTIN) {
        compGSTIN = compObject[0].BranchGSTIN;
    } else {
        compGSTIN = compObject[0].CompanyGSTIN
    }

    if (compGSTIN == "") {
        return;
    }

    var compStateCode = compGSTIN.substr(0, 2);

    var TaxTypeMode = '';
    if (shippingStateCode != compStateCode) {
        TaxTypeMode = "IGST";
    } else {
        TaxTypeMode = "SGST";
        //Remove shippingStateCode == "35" as now Delhi is not under UTGST
        if (shippingStateCode == "4" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "7" || shippingStateCode == "31" || shippingStateCode == "34") {
            TaxTypeMode = "UTGST";
        }
    }


    var HSNObject = $.grep(HSNCodewiseTaxSchem, function (e) { return e.HSNCODE == HSNCode; })



    if (HSNObject.length > 0) {
        var taxes = HSNObject[0].config_TaxRatesIDs;
        var TaxChargesAmount = 0;
        var totalTaxchargesRate = 0;
        var TaxApplicableOn = '';

        for (var taxCount = 0; taxCount < taxes.length; taxCount++) {

            //  var backProcessRate = (1 + (taxes[taxCount].Rate / 100));
            if (TaxTypeMode == "IGST") {
                if (taxes[taxCount].TaxTypeCode == "IGST") {

                    if (taxes[taxCount].Taxes_ApplicableOn == "G") {
                        TaxApplicableOn = "G";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                        // TaxChargesAmount = TaxChargesAmount + (GrosstotalAmount - (GrosstotalAmount / backProcessRate));
                    }
                    else if (taxes[taxCount].Taxes_ApplicableOn == "N") {
                        //TaxChargesAmount = TaxChargesAmount + (NettotalAmount - (NettotalAmount / backProcessRate));
                        TaxApplicableOn = "N";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                    }
                }
            }


            else if (TaxTypeMode == "UTGST") {

                if (taxes[taxCount].TaxTypeCode == "UTGST" || taxes[taxCount].TaxTypeCode == "CGST") {

                    if (taxes[taxCount].Taxes_ApplicableOn == "G") {
                        TaxApplicableOn = "G";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                    }
                    else if (taxes[taxCount].Taxes_ApplicableOn == "N") {
                        TaxApplicableOn = "N";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                    }
                }
            }

            else if (TaxTypeMode == "SGST") {

                if (taxes[taxCount].TaxTypeCode == "SGST" || taxes[taxCount].TaxTypeCode == "CGST") {

                    if (taxes[taxCount].Taxes_ApplicableOn == "G") {
                        TaxApplicableOn = "G";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                    }
                    else if (taxes[taxCount].Taxes_ApplicableOn == "N") {
                        TaxApplicableOn = "N";
                        totalTaxchargesRate = totalTaxchargesRate + taxes[taxCount].Rate;
                    }
                }
            }

        }

        if (inclsOrExclsv == "I") {

            var backProcessRate = (1 + (totalTaxchargesRate / 100));
            if (TaxApplicableOn == "G") {
                TaxChargesAmount = ((GrosstotalAmount - AvgValue) - ((GrosstotalAmount - AvgValue) / backProcessRate));
            } else if (TaxApplicableOn == "N") {
                TaxChargesAmount = ((NettotalAmount - AvgValue) - ((NettotalAmount - AvgValue) / backProcessRate));
            }

            // TaxChargesAmount = parseFloat(Math.round(Math.abs(Math.round(TaxChargesAmount)) * 100) / 100).toFixed(2);
            TaxChargesAmount = parseFloat((Math.abs(TaxChargesAmount) * 100) / 100).toFixed(2);
            var InClusiveAmount = parseFloat(Math.round(Math.abs(NettotalAmount - TaxChargesAmount) * 100) / 100).toFixed(2)

            //AmountEditor.SetValue(NettotalAmount - TaxChargesAmount);
            AmountEditor.SetValue(InClusiveAmount);
            chargesEditor.SetValue(TaxChargesAmount);
            TotalAmountEditor.SetValue(NettotalAmount);
        }
        else if (inclsOrExclsv == "E") {

            var backProcessRate = (totalTaxchargesRate / 100);
            var chargesAmount = (NettotalAmount - AvgValue) * backProcessRate;
            //TaxChargesAmount = parseFloat(Math.round(Math.abs(Math.round(chargesAmount)) * 100) / 100).toFixed(2);
            TaxChargesAmount = parseFloat((Math.abs(chargesAmount) * 100) / 100).toFixed(2);


            AmountEditor.SetValue(NettotalAmount);
            chargesEditor.SetValue(TaxChargesAmount);
            TotalAmountEditor.SetValue(parseFloat(NettotalAmount) + parseFloat(TaxChargesAmount));
        }






    }


}




FindStateIDByBranchID = function (branchID) {
    var branch_state = -1;
    $.each(BranchWiseStateTax, function (index, _Obj) {
        if (_Obj.branch_id == branchID) {
            branch_state = _Obj.branch_state;
            return false;
        }
    });
    return branch_state;
}

FindStateCodeByStateID = function (StateID) {
    var StateCode = -1;
    $.each(StateCodeWiseStateIDTax, function (index, _Obj) {
        if (_Obj.id == StateID) {
            StateCode = _Obj.StateCode;
            return false;
        }
    });
    return StateCode;
}



FindIsAddressinIGST = function (branchId, shippingStCode) {
    console.log(shippingStCode);
    var compObject = $.grep(BranchWiseStateTax, function (e) { return e.branch_id == branchId; })
    var compGSTIN = "";
    if (compObject[0].BranchGSTIN) {
        compGSTIN = compObject[0].BranchGSTIN;
    } else {
        compGSTIN = compObject[0].CompanyGSTIN
    }

    compGSTIN = compGSTIN.substr(0, 2);


    var returnvalue = false;
    if (compGSTIN != shippingStCode) {
        returnvalue = true;
    }
    return returnvalue;
}

function RoundUp(x, n) {
    return Math.round(x * Math.pow(10, n)) / Math.pow(10, n)
}