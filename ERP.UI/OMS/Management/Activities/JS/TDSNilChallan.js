 /*****************
Global variable*/

var ReceiptList = [];
var globalRowindex = 0;
var DocumentList = [];
var canCallBack = true;
var PickedDocument = [];
var saveNewOrExit = '';
var alertShow = false;




function RecalculateTotal() {
    var total = DecimalRoundoff(totTDSamount, 2);  
    ctxtTotal.SetValue(parseFloat(total));
}

function adjAmountLostFocus() {
    if (parseFloat(cAdjAmt.GetValue()) > parseFloat(cOsAmt.GetValue())) {
        jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert", function () { cAdjAmt.SetValue(cOsAmt.GetValue()); });
    }
}


function GetVisibleIndex(s, e) {
    globalRowindex = e.visibleIndex;
    //OnAllCheckedChanged();
}
function gridFocusedRowChanged(s, e) {
    globalRowindex = e.visibleIndex;
}

function gridAdjustAmtLostFocus(s, e) {
    console.log('1');
    if (parseFloat(grid.GetEditor("OsAmt").GetValue()) < s.GetValue()) {

        grid.batchEditApi.StartEdit(globalRowindex, 8);
        var NewAmt = grid.GetEditor("OsAmt");
        s.SetValue(NewAmt.GetValue());
        jAlert("Adjusted Amount can not be more than O/S Amount.", "Alert");



    }
    grid.GetEditor("RemainingBalance").SetValue(parseFloat(grid.GetEditor("OsAmt").GetValue()) - s.GetValue());

}

function GridAddnewRow() {
    grid.AddNewRow();
    // grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());

}
function grid_SelectionChanged(s, e) {
    globalRowindex = e.visibleIndex;
    
    s.GetSelectedFieldValues("Tax_Amount", GetSelectedFieldValuesCallback);
    s.GetSelectedFieldValues("Total_Tax", GetSelectedFieldValuesTotal_TaxCallback);
    s.GetSelectedFieldValues("EduCess", GetSelectedFieldValuesEduCessCallback);
    s.GetSelectedFieldValues("Surcharge", GetSelectedFieldValuesSurchargeCallback);

}
function GetSelectedFieldValuesCallback(values) {
    
    document.getElementById("lblcount").innerHTML = grid.GetSelectedRowCount();

    totTDSamount = 0;
    for (var i = 0; i < values.length; i++) {
        totTDSamount = parseFloat(totTDSamount) + parseFloat(values[i]);
    }
    ctxtTax.SetValue(parseFloat(totTDSamount));

    //if (grid.GetVisibleRowsOnPage() >= 1) {
    //    grid.batchEditApi.StartEdit(globalRowindex, 0);
    //    totTDSamount = 0;
    //    totTax = 0;
    //    totEdu = 0;
    //    totSurcharge = 0;
    //    totalcount = 0;

    //    totTDSamount = parseFloat(ctxtTotal.GetText()) + parseFloat(grid.GetEditor("Tax_Amount").GetText());
    //    totTax = totTax + parseFloat(grid.GetEditor("Total_Tax").GetText());
    //    totEdu = totEdu + parseFloat(grid.GetEditor("EduCess").GetText());
    //    totSurcharge = parseFloat(ctxtSurcharge.GetText()) + parseFloat(grid.GetEditor("Surcharge").GetText());

    //    totalcount = totalcount + parseFloat(document.getElementById("lblcount").innerText);


    //    document.getElementById("lblcount").innerHTML = totalcount;


    //    ctxtSurcharge.SetValue(parseFloat(totSurcharge));
    //    ctxteduCess.SetValue(parseFloat(totEdu));
    //    ctxtTotal.SetValue(parseFloat(totTDSamount));
    //    ctxtTax.SetValue(parseFloat(totTax));
    //}
}
function GetSelectedFieldValuesTotal_TaxCallback(values1) {

    //document.getElementById("lblcount").innerHTML = grid.GetSelectedRowCount();

    totTax = 0;
    for (var i = 0; i < values1.length; i++) {
        totTax = parseFloat(totTax) + parseFloat(values1[i]);
    }
    ctxtTotal.SetValue(parseFloat(totTax));
}
function GetSelectedFieldValuesEduCessCallback(values) {

    document.getElementById("lblcount").innerHTML = grid.GetSelectedRowCount();

    toteduCess = 0;
    for (var i = 0; i < values.length; i++) {
        toteduCess = parseFloat(toteduCess) + parseFloat(values[i]);
    }
    ctxteduCess.SetValue(parseFloat(toteduCess));
}
function GetSelectedFieldValuesSurchargeCallback(values) {

    //document.getElementById("lblcount").innerHTML = grid.GetSelectedRowCount();

    totSurcharge = 0;
    for (var i = 0; i < values.length; i++) {
        totSurcharge = parseFloat(totSurcharge) + parseFloat(values[i]);
    }
    ctxtSurcharge.SetValue(parseFloat(totSurcharge));
}

function OnAllCheckedChanged()
{
    
    if (grid.GetVisibleRowsOnPage() > 1) {     
        grid.batchEditApi.StartEdit(globalRowindex, 4);
        totTDSamount = 0;
        totTax = 0;
        totEdu = 0;
        totSurcharge = 0;
        totalcount = 0;

        totTDSamount = parseFloat(ctxtTotal.GetText()) + parseFloat(grid.GetEditor("Tax_Amount").GetText());
        totTax = totTax + parseFloat(grid.GetEditor("Total_Tax").GetText());
        totEdu = totEdu + parseFloat(grid.GetEditor("EduCess").GetText());
        totSurcharge = parseFloat(ctxtSurcharge.GetText()) + parseFloat(grid.GetEditor("Surcharge").GetText());

        totalcount = totalcount + parseFloat(document.getElementById("lblcount").innerText);


        document.getElementById("lblcount").innerHTML = totalcount;


        ctxtSurcharge.SetValue(parseFloat(totSurcharge));
        ctxteduCess.SetValue(parseFloat(totEdu));
        ctxtTotal.SetValue(parseFloat(totTDSamount));
        ctxtTax.SetValue(parseFloat(totTax));
    }
}


function AllControlInitilize() {
    if (canCallBack) {
       
        if ($('#hdAddEdit').val() == "Add") {
            GridAddnewRow();

        } else {
            //showDocumentList();
            //CreateDocumentList();
            //cRemarks.Focus();
        }

        canCallBack = false;
    }
}

function SuffuleSerialNumber() {
    var TotRowNumber = grid.GetVisibleItemsOnPage();
    var SlnoCount = 1;
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            grid.batchEditApi.StartEdit(i, 2);
            grid.GetEditor("SrlNo").SetText(SlnoCount);
            SlnoCount++;
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            grid.batchEditApi.StartEdit(i, 2);
            grid.GetEditor("SrlNo").SetText(SlnoCount);
            SlnoCount++;
        }
    }
}





function ValidateEntry() {
    LoadingPanel.Show();
    var ReturnValue = true;
    if (ctdsSection.GetText().trim() == "Select") {
        $('#MandatorySection').show();
        LoadingPanel.Hide();
        return false;
    } else {
        $('#MandatorySection').hide();
    }
    if (grid.GetSelectedRowCount() == 0) {
        jAlert('At Least one Document should be selected before saving.');
        LoadingPanel.Hide();
        return false;
    }

    //if (ctxtChallanNo.GetText().trim() == "") {
    //    $('#MandatoryChallanNo').show()
    //    return false;
    //} else {
    //    $('#MandatoryChallanNo').hide();
    //}
    

    return ReturnValue;
}
function SaveButtonClick() {
    saveNewOrExit = 'N';
    if (ValidateEntry()) {
        grid.PerformCallback("SaveTDS");
    }
}


function HeaderClear() {
    ctxtCustName.SetText("");
    GetObjectID('hdnCustomerId').value = "";
    GetObjectID('hdAdvanceDocNo').value = "";
    cbtntxtDocNo.SetText("");
    cDocAmt.SetValue(0);
    cExchRate.SetValue(0);
    cBaseAmt.SetValue(0);
    cRemarks.SetText("");
    cOsAmt.SetValue(0);
    cAdjAmt.SetValue(0);
}


function afterSave() {

    if (saveNewOrExit == 'N') {
        window.location.href = 'TDSNilChallan.aspx?Key=Add';

    }
    else {
        window.location.href = 'TDSNilChallanList.aspx';
    }
}



function GetTotalAdjustedAmount() {
    var TotaAdj = 0;
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                TotaAdj = TotaAdj + parseFloat(grid.GetEditor("AdjAmt").GetValue());
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            grid.batchEditApi.StartEdit(i, 2);
            TotaAdj = TotaAdj + parseFloat(grid.GetEditor("AdjAmt").GetValue())

        }
    }

    return TotaAdj;
}

function GridEndCallBack(s, e) {
    LoadingPanel.Hide();
    //console.log(s.cpErrorCode.toString());
    alertShow = true;
    if (grid.cpErrorCode == "1") {
        grid.cpErrorCode = null;
        jAlert(grid.cpChallanNumber, "Alert", function () { afterSave(); alertShow = false; });
    }
    else if (grid.cpBlankGrid == "1") {
        grid.cpBlankGrid = null;
        GridAddnewRow();
    }
    //else if (grid.cpErrorCode == "-9") {
    //    jAlert('DATA is Freezed between ' + grid.cpAddLockStatus + ' for Add.');
    //    grid.cpAddLockStatus = null;
    //    grid.cpErrorCode = null;
    //}
    //else {
    //    jAlert(grid.cpadjustmentNumber, "Alert", function () { grid.batchEditApi.StartEdit(-1, 2); grid.batchEditApi.StartEdit(0, 2); alertShow = false; });
    //}
}

function SaveExitButtonClick() {
    saveNewOrExit = 'E';
    if (ValidateEntry()) {
    //    if (!grid.InCallback()) {
            grid.PerformCallback("SaveTDS");
            //grid.UpdateEdit();
    //    }
    }
}



document.onkeydown = function (e) {
    if (event.keyCode == 83 && event.altKey == true && !alertShow) { //run code for Alt + n -- ie, Save & New  

        SaveButtonClick();
    }
    else if (event.keyCode == 88 && event.altKey == true && !alertShow) { //run code for Ctrl+X -- ie, Save & Exit!     

        SaveExitButtonClick();
    }

}


function GetTwodecimalValue(val) {
    return parseFloat((Math.round(val * 100)) / 100).toFixed(2);
}


