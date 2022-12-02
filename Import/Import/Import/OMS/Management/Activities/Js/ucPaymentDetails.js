var JsonMainAccount;
var JsonBranchmainAct;

$(document).ready(function () {


    //JsonMainAccount = JSON.parse(document.getElementById('hdJsonMainAccountString').value);
    //JsonBranchmainAct = JSON.parse(document.getElementById('HdJsonBranchMainAct').value);



    if ($('#HdSelectedBranch').val() != '') {
        var OtherDetails = {}
        OtherDetails.BranchId = $('#HdSelectedBranch').val();

        $.ajax({
            type: "POST",
            url: "/Services/Import_Master.asmx/GetMainAccountForPaymentDet",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                JsonMainAccount = msg.d;
                FetchEnteredPaymentDetails();
            }
        });

    }
});

function loadMainAccountByBranchIdForPayDet(branchId) {

    var OtherDetails = {}
    OtherDetails.BranchId = branchId;

    $.ajax({
        type: "POST",
        url: "/Services/Import_Master.asmx/GetMainAccountForPaymentDet",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            JsonMainAccount = msg.d; 
        }
    });
}


function dateValidationFormat(e) {
    var v = e.target.value; //this.value;
    if (v.match(/^\d{2}$/) !== null) {
        e.target.value = v + '-';
    } else if (v.match(/^\d{2}\-\d{2}$/) !== null) {
        e.target.value = v + '-';
    }
}


function cmbUcpaymentCashLedgerChanged(s, e) {
    if (document.getElementById("B_BankBalance")) {
        PopulateCurrentBankBalance(s.GetValue());
    }
}

function CallRunningBalance() {
    if (document.getElementById('bnrLblInvValue')) {
        SetRunningBalance();
    }
}


function GetPaymentTotalEnteredAmount() {
    var TotalAmountEntered = parseFloat($('#cmbUcpaymentCashLedgerAmt').val());
    var table = document.getElementById("paymentDetails");
    for (var i = 0 ; i < table.rows.length; i++) {
        if (table.rows[i].children[0].children[1].value != "-Select-" && table.rows[i].children[0].children[1].value != "") {
            if (table.rows[i].children[6].children[1].value != '') {
                TotalAmountEntered += parseFloat(table.rows[i].children[6].children[1].value);
            }
        }
    }
    return TotalAmountEntered;
}



function FetchEnteredPaymentDetails() {
    if (cClientSaveData.Get('0')) {
        document.getElementById("paymentDetails").deleteRow(0);
        SetEnteredPaymentDetailsData(0);
        AddPaymentRowOnEdit();
    }
}


function SetEnteredPaymentDetailsData(count) {

    if (cClientSaveData.Get(count + '')) {
        var data = cClientSaveData.Get(count + '');
        var extractData = data.split('|~|');
        if (extractData[0] != "Cash")
            var row = AddPaymentRowOnEdit();
        SetPaymentRowValue(row, extractData);
        SetEnteredPaymentDetailsData(count + 1);
    }
}

function SetPaymentRowValue(row, paymentType) {


    if (row) {
        row.children[1].innerHTML = GetHtml1(paymentType[0]);
        row.children[2].innerHTML = GetHtml2(paymentType[0]);
        row.children[3].innerHTML = GetHtml3(paymentType[0]);
        row.children[4].innerHTML = GetHtml4(paymentType[0]);
        row.children[5].innerHTML = GetHtml5(paymentType[0]);
        row.children[6].innerHTML = GetHtml6(paymentType[0]);
        row.children[7].innerHTML = GetHtml7(paymentType[0]);


        //SetValue 
        row.children[0].children[1].value = paymentType[0];
    }
    if (paymentType[0] == "Card") {
        row.children[1].children[1].value = paymentType[1];
        row.children[2].children[1].value = paymentType[2];
        row.children[3].children[1].value = paymentType[3];
        row.children[4].children[1].value = paymentType[4];
        row.children[5].children[1].value = paymentType[5];
        row.children[6].children[1].value = paymentType[6];
    }
    else if (paymentType[0] == "Cash") {
        console.log(paymentType[1]);
        ccmbUcpaymentCashLedger.SetValue(paymentType[1].trim());
        $('#cmbUcpaymentCashLedgerAmt').val(paymentType[2]);
    }
    else if (paymentType[0] == "Cheque") {
        row.children[1].children[1].value = paymentType[1];
        row.children[2].children[1].value = paymentType[2];
        row.children[3].children[1].value = paymentType[3];
        row.children[4].children[1].value = paymentType[4];
        row.children[5].children[1].value = paymentType[5];
        row.children[6].children[1].value = paymentType[6];
    }
    else if (paymentType[0] == "Coupon") {
        row.children[1].children[1].value = paymentType[1];
        row.children[5].children[1].value = paymentType[2];
        row.children[6].children[1].value = paymentType[3];
    }
    else if (paymentType[0] == "E Transfer") {
        row.children[1].children[1].value = paymentType[1];
        row.children[2].children[1].value = paymentType[2];
        row.children[5].children[1].value = paymentType[3];
        row.children[6].children[1].value = paymentType[4];
    }


    //Date control initilize
    if (paymentType[0] == 'Cheque' || paymentType[0] == 'E Transfer') {


    }
    if (paymentType[0] == 'Cheque') {

    }

    if (row) {
        row.children[0].width = '10%';
        row.children[1].width = '12%';
        row.children[2].width = '14%';
        row.children[3].width = '14%';
        row.children[4].width = '15%';
        row.children[5].width = '15%';
        row.children[6].width = '10%';
        row.children[7].width = '10%';


        row.children[7].className = 'text-right';
    }
}


function AddPaymentRowOnEdit() {
    var table = document.getElementById("paymentDetails");
    var row = table.insertRow(table.rows.length);
    var selectcell = row.insertCell(0);
    row.insertCell(1);
    row.insertCell(2);
    row.insertCell(3);
    row.insertCell(4);
    row.insertCell(5);
    row.insertCell(6);
    row.insertCell(7);

    var selectHtml = ' <label>Payment Type</label><select class="form-control" onchange="paymentTypeChange(event)">';
    selectHtml += '<option>-Select-</option><option>Card</option> <option>Cheque</option><option>Coupon</option><option>E Transfer</option></select>';
    selectcell.innerHTML = selectHtml;
    selectcell.width = "200px";

    return row;
}

function AddNewPayment(e) {
    if (document.getElementById('lblRunningBalanceCapsul')) {
        if (parseFloat(clblRunningBalanceCapsul.GetValue()) <= 0) {
            jAlert("Cannot proceed. Payment value has been made equal or greater than the Invoice value.");
            return false;
        }
    }
    if (e.target.id == "AddNewPay") {
        if (!validatePaymentDetails(e.target.parentNode.parentNode)) {
            return false;
        }
    } else {
        if (!validatePaymentDetails(e.target.parentNode.parentNode.parentNode)) {
            return false;
        }
    }




    var table = document.getElementById("paymentDetails");
    var row = table.insertRow(table.rows.length);
    var selectcell = row.insertCell(0);
    row.insertCell(1);
    row.insertCell(2);
    row.insertCell(3);
    row.insertCell(4);
    row.insertCell(5);
    row.insertCell(6);
    row.insertCell(7);

    var selectHtml = ' <label>Payment Type</label><select class="form-control" onchange="paymentTypeChange(event)">';
    selectHtml += '<option>-Select-</option><option>Card</option> <option>Cheque</option><option>Coupon</option><option>E Transfer</option></select>';
    selectcell.innerHTML = selectHtml;
    selectcell.width = "200px";
}


function validatePaymentDetails(row) {
    for (var i = 0; i < row.children.length; i++) {
        if (row.children[0].children[1].value == "Card") {


            if (row.children[1].children[1].value.trim() == "") {
                row.children[1].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[2].children[1].value == "-Select Card-") {
                row.children[2].children[1].className = "NotValid form-control";
                return false;
            }
            else if (row.children[3].children[1].value.trim() == "") {
                row.children[3].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[4].children[1].value.trim() == "") {
                row.children[4].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[5].children[1].value.trim() == "") {
                row.children[5].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[6].children[1].value.trim() == "") {
                row.children[6].children[1].className = "NotValid";
                return false;
            }
            else {
                row.children[1].children[1].className = "";
                row.children[2].children[1].className = "form-control";
                row.children[3].children[1].className = "";
                row.children[4].children[1].className = "";
                row.children[5].children[1].className = "form-control";
                row.children[6].children[1].className = "";
            }
        }

        else if (row.children[0].children[1].value == "Cheque") {

            if (row.children[1].children[1].value.trim() == "") {
                row.children[1].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[2].children[1].value.trim() == "") {
                row.children[2].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[3].children[1].value.trim() == "") {
                row.children[3].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[5].children[1].value.trim() == "") {
                row.children[5].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[6].children[1].value.trim() == "") {
                row.children[6].children[1].className = "NotValid";
                return false;
            }
            else {
                row.children[1].children[1].className = "";
                row.children[2].children[1].className = "";
                row.children[3].children[1].className = "";
                row.children[5].children[1].className = "form-control";
                row.children[6].children[1].className = "";
            }
        }

        else if (row.children[0].children[1].value == "Coupon") {
            if (row.children[1].children[1].value.trim() == "") {
                row.children[1].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[5].children[1].value.trim() == "") {
                row.children[5].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[6].children[1].value.trim() == "") {
                row.children[6].children[1].className = "NotValid";
                return false;
            }
            else {
                row.children[1].children[1].className = "";
                row.children[5].children[1].className = "form-control";
                row.children[6].children[1].className = "";
            }
        }

        else if (row.children[0].children[1].value == "E Transfer") {
            if (row.children[1].children[1].value.trim() == "") {
                row.children[1].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[2].children[1].value.trim() == "") {
                row.children[2].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[5].children[1].value.trim() == "") {
                row.children[5].children[1].className = "NotValid";
                return false;
            }
            else if (row.children[6].children[1].value.trim() == "") {
                row.children[6].children[1].className = "NotValid";
                return false;
            }
            else {
                row.children[1].children[1].className = "";
                row.children[2].children[1].className = "";
                row.children[5].children[1].className = "form-control";
                row.children[6].children[1].className = "";
            }
        }

    }


    row.children[7].children[0].className = 'hide';
    row.children[0].children[1].setAttribute("disabled", "disabled");

    return true;
}



function removeExecutive(obj) {
    var rowIndex = obj.rowIndex;
    var table = document.getElementById("paymentDetails");
    if (table.rows.length > 1) {
        table.rows[table.rows.length - 2].children[7].children[0].className = '';
        table.rows[table.rows.length - 2].children[0].children[1].removeAttribute('disabled');
        table.deleteRow(rowIndex);
        CallRunningBalance();
    } else {
        //jAlert('Cannot delete all Payment Methods.');
        table.rows[0].children[0].children[1].value = '-Select-';
        var evt = document.createEvent("HTMLEvents");
        evt.initEvent("change", false, true);
        table.rows[0].children[0].children[1].dispatchEvent(evt);
    }
}

var IsLedgerPresent = 0;
var tt;
function paymentTypeChange(e) {


    var tableRow = e.target.parentNode.parentNode;
    IsLedgerPresent = 0;

    if (document.getElementById("lblRunningBalanceCapsul")) {
        if (parseFloat(clblRunningBalanceCapsul.GetValue()) <= 0) {
            if (e.target.value != "-Select-") {
                jAlert("Cannot proceed. Payment value has been made equal or greater than the Invoice value.");
            }
            tableRow.children[0].children[1].value = '-Select-';
            CreateSelectControl(e);
            CallRunningBalance();
            return;
        }
    }


    tableRow.children[1].innerHTML = GetHtml1(e.target.value);
    tableRow.children[2].innerHTML = GetHtml2(e.target.value);

    tableRow.children[3].innerHTML = GetHtml3(e.target.value);
    tableRow.children[4].innerHTML = GetHtml4(e.target.value);
    tableRow.children[5].innerHTML = GetHtml5(e.target.value);
    tableRow.children[6].innerHTML = GetHtml6(e.target.value);

    tableRow.children[7].innerHTML = GetHtml7(e.target.value);

    if (e.target.value != '-Select-') {
        if (IsLedgerPresent == 0) {
            jAlert(" You must map ledger for the Selected payment type to post data into respective Ledger. Cannot proceed.")

            tableRow.children[0].children[1].value = '-Select-';
            var evt = document.createEvent("HTMLEvents");
            evt.initEvent("change", false, true);
            tableRow.children[0].children[1].dispatchEvent(evt);
        }
    }


    //Date control initilize
    if (e.target.value == 'Cheque' || e.target.value == 'E Transfer') {

        //tableRow.children[2].children[1].flatpickr({
        //    enableTime: false,
        //    weekNumbers: false,
        //    dateFormat: "d-m-Y"

        //});
    }
    if (e.target.value == 'Cheque') {
        //tableRow.children[3].children[1].flatpickr({
        //    enableTime: false,
        //    weekNumbers: false,
        //    dateFormat: "d-m-Y"

        //});
    }


    tableRow.children[0].width = '10%';
    tableRow.children[1].width = '12%';
    tableRow.children[2].width = '14%';
    tableRow.children[3].width = '14%';
    tableRow.children[4].width = '15%';
    tableRow.children[5].width = '15%';
    tableRow.children[6].width = '10%';
    tableRow.children[7].width = '10%';


    tableRow.children[7].className = 'text-right';

}



function CreateSelectControl(e) {

    var tableRow = e.target.parentNode.parentNode;
    IsLedgerPresent = 0;

    //e.target.value        
    tableRow.children[1].innerHTML = GetHtml1('');
    tableRow.children[2].innerHTML = GetHtml2('');

    tableRow.children[3].innerHTML = GetHtml3('');
    tableRow.children[4].innerHTML = GetHtml4('');
    tableRow.children[5].innerHTML = GetHtml5('');
    tableRow.children[6].innerHTML = GetHtml6('');

    tableRow.children[7].innerHTML = GetHtml7('');

    if (e.target.value != '-Select-') {
        if (IsLedgerPresent == 0) {
            jAlert(" You must map ledger for the Selected payment type to post data into respective Ledger. Cannot proceed.")

            tableRow.children[0].children[1].value = '-Select-';
            var evt = document.createEvent("HTMLEvents");
            evt.initEvent("change", false, true);
            tableRow.children[0].children[1].dispatchEvent(evt);
        }
    }


    //Date control initilize
    if (e.target.value == 'Cheque' || e.target.value == 'E Transfer') {
        //tableRow.children[2].children[1].flatpickr({
        //    enableTime: false,
        //    weekNumbers: false,
        //    dateFormat: "d-m-Y"

        //});
    }
    if (e.target.value == 'Cheque') {
        //tableRow.children[3].children[1].flatpickr({
        //    enableTime: false,
        //    weekNumbers: false,
        //    dateFormat: "d-m-Y"

        //});
    }


    tableRow.children[0].width = '10%';
    tableRow.children[1].width = '12%';
    tableRow.children[2].width = '14%';
    tableRow.children[3].width = '14%';
    tableRow.children[4].width = '15%';
    tableRow.children[5].width = '15%';
    tableRow.children[6].width = '10%';
    tableRow.children[7].width = '10%';


    tableRow.children[7].className = 'text-right';

}

function isDateKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    console.log(charCode);

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    console.log(charCode);
    if (charCode == 46)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}


function GetHtml1(type) {
    var inrHtml = '&nbsp;';
    if (type == "Card") {
        inrHtml = '  <label>Enter Card No</label> <input type="text"  onKeyPress="GetCardType(event)" onKeyUp="GetCardType(event)"/>';
    }
    else if (type == 'Cheque') {
        inrHtml = '  <label>Enter Cheque No</label> <input type="text" />';
    }
    else if (type == 'Coupon') {
        inrHtml = '  <label>Coupon Details</label> <input type="text" />';
    }
    else if (type == 'E Transfer') {
        inrHtml = '  <label>Enter Instument No</label> <input type="text" />';
    }

    return inrHtml;

}

function GetHtml2(type) {
    var inrHtml = '&nbsp;';


    if (type == "Card") {
        inrHtml = '<label>Select Card Type</label>  <select class="form-control"><option>-Select Card-</option><option>Visa</option><option>Master</option><option>American Express</option><option>Maestro</option><option>Rupay</option><option>Diners Club</option></select>';
    }
    else if (type == 'Cheque') {
        inrHtml = '  <label>Enter Date</label> <input type="text" value="' + new Date().format('dd-MM-yyyy') + '" onkeyup="dateValidationFormat(event)" maxlength="10"  onkeypress="return isDateKey(event)" class="flatpickr"/>';
    }
    else if (type == 'E Transfer') {
        inrHtml = '  <label>Enter Date</label> <input type="text"  value="' + new Date().format('dd-MM-yyyy') + '" onkeyup="dateValidationFormat(event)" maxlength="10" onkeypress="return isDateKey(event)"  class="flatpickr"/>';
    }
    return inrHtml;
}

function GetHtml3(type) {
    var inrHtml = '&nbsp;';
    if (type == "Card") {
        inrHtml = '  <label>Approval Code</label> <input type="text" />';
    }
    else if (type == 'Cheque') {
        inrHtml = '  <label>Drawee Date</label> <input type="text"  value="' + new Date().format('dd-MM-yyyy') + '" onkeyup="dateValidationFormat(event)" onkeypress="return isDateKey(event)" maxlength="10" class="flatpickr"/>';
    }
    return inrHtml;
}

function GetHtml4(type) {
    var inrHtml = '&nbsp;';
    if (type == "Card") {
        inrHtml = '  <label>Bank</label> <input type="text" />';
    } else if (type == 'Cheque') {
        inrHtml = '  <label>Bank and Branch</label> <input type="text" />';
    }
    return inrHtml;
}

function GetHtml5(type) {
    var inrHtml = '&nbsp;';
    var ledgerCount = 0;
    var BranchLedgerObj;
    var MainAccountCount;

    if (type == "Card") {
        inrHtml = '  <label>Select Ledger</label>';
        inrHtml += '<select class="form-control">';

        var optionsHtml = ''
        for (var i = 0; i < JsonMainAccount.length; i++) {
            if (JsonMainAccount[i].MainAccount_BankCashType == "Card") { 
                    optionsHtml += '<option value="' + JsonMainAccount[i].MainAccount_AccountCode + '">' + JsonMainAccount[i].MainAccount_Name + '</option>'
                    ledgerCount = ledgerCount + 1;
            }
        }

        inrHtml += optionsHtml;
        inrHtml += '</select>';
    } else if (type == "Cheque") {
        inrHtml = '  <label>Select Ledger</label>';
        inrHtml += '<select class="form-control">';

        var optionsHtml = ''
        for (var i = 0; i < JsonMainAccount.length; i++) {
            if (JsonMainAccount[i].MainAccount_BankCashType == "None" ) {
              
                optionsHtml += '<option value="' + JsonMainAccount[i].MainAccount_AccountCode + '">' + JsonMainAccount[i].MainAccount_Name + '</option>'
                ledgerCount = ledgerCount + 1;
            }
        }

        inrHtml += optionsHtml;
        inrHtml += '</select>';

    }
    else if (type == "E Transfer") {
        inrHtml = '  <label>Select Ledger</label>';
        inrHtml += '<select class="form-control">';

        var optionsHtml = ''
        for (var i = 0; i < JsonMainAccount.length; i++) {
            if (JsonMainAccount[i].MainAccount_BankCashType == "Etransfer" ) { 
                    optionsHtml += '<option value="' + JsonMainAccount[i].MainAccount_AccountCode + '">' + JsonMainAccount[i].MainAccount_Name + '</option>'
                ledgerCount = ledgerCount + 1;
            }
        }

        inrHtml += optionsHtml;
        inrHtml += '</select>';

    }
    else if (type == "Coupon") {
        inrHtml = '  <label>Select Ledger</label>';
        inrHtml += '<select class="form-control">';

        var optionsHtml = ''
        for (var i = 0; i < JsonMainAccount.length; i++) {
            if (JsonMainAccount[i].MainAccount_BankCashType == "Coupon" ) { 
                    optionsHtml += '<option value="' + JsonMainAccount[i].MainAccount_AccountCode + '">' + JsonMainAccount[i].MainAccount_Name + '</option>'
                ledgerCount = ledgerCount + 1;
            }
        }

        inrHtml += optionsHtml;
        inrHtml += '</select>';

    }

    if (ledgerCount > 0) {
        IsLedgerPresent = 1;
    }

    return inrHtml;
}


function GetHtml6(type) {
    var inrHtml = '&nbsp;';

    if (type == "Card" || type == 'Cheque' || type == 'Coupon' || type == 'E Transfer') {
        inrHtml = '  <label>Enter Amount</label> <input type="text" onkeypress="return isNumberKey(event)" onblur="CallRunningBalance()"/>';
    }

    return inrHtml;

}

function GetHtml7(type) {
    var inrHtml = '&nbsp;';
    if (type == "Card" || type == 'Cheque' || type == 'Coupon' || type == 'E Transfer') {
        inrHtml = ' <a href="javascript:void(0)" style="margin-top: 18px;margin-left: 10px;display:inline-block;margin-right:5px;" id="AddNewPay" onClick="AddNewPayment(event)"><img src="/assests/images/add.png"></a>  <a href="javascript:void(0)" onclick="removeExecutive(this.parentNode.parentNode)" ><img src="/assests/images/crs.png"></a>';
    }
    return inrHtml;

}


function GetCardType(event) {


    var number = event.target.value;
    var cardtype = "";// visa

    var visa_re = new RegExp("^4");
    var mastercard_re = new RegExp("^5[1-5]");
    var amex_re = new RegExp("^3[47]");
    //var mastro_re = new RegExp("^(5018|5020|5038|6304|6759|6761|6763|6220)"); 
    var mastro_re = new RegExp("^(6|5(0|[6-9]))");
    var rupay_re = new RegExp("^508[5-9]|6(069(8[5-9]|9)|07([0-8]|9([0-7]|8[0-4]))|08([0-4]|500)|52([2-9]|1[5-9])|53(0|1[0-4]))");
    var diners_re = new RegExp("^30");


    if (number.match(visa_re) != null) {
        cardtype = "Visa";
    }


    if (number.match(mastercard_re) != null) {
        cardtype = "Master";
    }


    if (number.match(amex_re) != null) {
        cardtype = "American Express";
    }


    if (number.match(mastro_re) != null) {
        cardtype = "Maestro";

    }

    if (number.match(rupay_re) != null) {
        cardtype = "Rupay";
    }


    if (number.match(diners_re) != null) {
        cardtype = "Diners Club";

    }


    event.target.parentElement.parentElement.children[2].children[1].value = cardtype;

    if (cardtype == "") {
        event.target.parentElement.parentElement.children[2].children[1].selectedIndex = 0;
    }

}



function SelectAllData(callback) {
    cClientSaveData.Clear();
    var cashId = 0;
    var tbl = document.getElementById('PaymentTable');
    for (var i = 0; i < tbl.rows.length; i++) {
        var row = tbl.rows[i];
        var select = row.children[0].children[1];
        if (select.value == "Card") {
            var data = "Card";
            data += '|~|' + row.children[1].children[1].value;//card No
            data += '|~|' + row.children[2].children[1].value;//card type
            data += '|~|' + row.children[3].children[1].value; //Auth no
            data += '|~|' + row.children[4].children[1].value; // Remarks
            data += '|~|' + row.children[5].children[1].value; // MainAccount
            data += '|~|' + row.children[6].children[1].value; // MainAccount

            cClientSaveData.Set(i, data);
            cashId++;
        }
        else if (select.value == "Cheque") {
            var data = "Cheque";
            data += '|~|' + row.children[1].children[1].value;//cheque no
            data += '|~|' + row.children[2].children[1].value;//enter date
            data += '|~|' + row.children[3].children[1].value; //draweble date
            data += '|~|' + row.children[4].children[1].value; // Remarks
            data += '|~|' + row.children[5].children[1].value; // Main Account
            data += '|~|' + row.children[6].children[1].value; // Amount

            cClientSaveData.Set(i, data);
            cashId++;
        }
        else if (select.value == "Coupon") {
            var data = "Coupon";
            data += '|~|' + row.children[1].children[1].value;//coupon 
            data += '|~|' + row.children[5].children[1].value; // Amount
            data += '|~|' + row.children[6].children[1].value; // Amount


            cClientSaveData.Set(i, data);
            cashId++;
        }
        else if (select.value == "E Transfer") {
            var data = "E Transfer";
            data += '|~|' + row.children[1].children[1].value;//instrument no
            data += '|~|' + row.children[2].children[1].value;//enter date 
            data += '|~|' + row.children[5].children[1].value; // Main Account
            data += '|~|' + row.children[6].children[1].value; // Amount
            cClientSaveData.Set(i, data);
            cashId++;
        }

    }

    //Add Cash Invoice
    var data = "Cash";
    data += '|~|' + ccmbUcpaymentCashLedger.GetValue();
    data += '|~|' + $('#cmbUcpaymentCashLedgerAmt').val();
    cClientSaveData.Set(cashId, data);

    if (callback) {
        callback();
    }
}