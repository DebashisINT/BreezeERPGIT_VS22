

var IsLedgerPresent = 0;
var tt;
function paymentTypeChange(e) {


   

    tableRow.children[1].innerHTML = GetHtml1(e.target.value);
 

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
    }


    else {
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



function GetHtml(type) {
    var inrHtml = '&nbsp;';
    if (type == "Bank") {
        inrHtml = '<label>Bank Name/label> <input type="text" /> <label>Cheque Number/label> <input type="text" /> <label>Cheque Date/label> <input type="text" /> <label>Amount/label> <input type="text" />';
    }
    else if (type == 'GL') {
        inrHtml = ' <label>GL Name/label> <input type="text" />  <label>Amount</label> <input type="text" />';
    }
    else if (type == 'Cash') {
        inrHtml = '  <label>Amount</label> <input type="text" />';
    }
   

    return inrHtml;

}


function GetHtmlAdd(type) {

    var inrHtml = '&nbsp;';
    if (type == "Bank" || type == 'GL' || type == 'Cash') {
        inrHtml = ' <a href="javascript:void(0)" style="margin-top: 18px;margin-left: 10px;display:inline-block;margin-right:5px;" id="AddNewPay" onClick="AddNewPayment(event)"><img src="/assests/images/add.png"></a>  <a href="javascript:void(0)" onclick="removeExecutive(this.parentNode.parentNode)" ><img src="/assests/images/crs.png"></a>';
    }
    return inrHtml;

}


function  paymentTypeChange()
{
 
    var option = $("#ddl_type1").val();

    $("#dvtype1").attr("style", "display:block;");

    if (option == "Cash") {
        $("#divbankgl").attr("style", "display:none;")
        $("#divbank").attr("style", "display:none;")
        $("#LedgerID").attr("style", "display:none;");
    }

    else if (option == "Bank") {
        $("#divbankgl").attr("style", "display:block;")
        $("#divbank").attr("style", "display:block;")
        $("#lblbankledger").html("Bank");
        $("#LedgerID").attr("style", "display:none;");
    }

    else if (option == "Ledger") {
        $("#divbankgl").attr("style", "display:none;")
        $("#divbank").attr("style", "display:none;")
        $("#lblbankledger").attr("style", "display:none;");
        $("#LedgerID").attr("style", "display:block;");
    }


}

function paymentTypeChange1() {
    
    $("#dvtype2").attr("style", "display:block;");
    var option = $("#ddl_type2").val();

    if (option == "Cash") {
        $("#divbankgl1").attr("style", "display:none;")
        $("#divbank1").attr("style", "display:none;")
       // $("#LedgerID").attr("style", "display:none;");
    }

    else if (option == "Bank") {
        $("#divbankgl1").attr("style", "display:block;")
        $("#divbank1").attr("style", "display:block;")
        $("#lblbankledger1").html("Bank");
       // $("#LedgerID").attr("style", "display:none;");
    }

    else if (option == "Ledger") {
        $("#divbankgl1").attr("style", "display:block;")
        $("#divbank1").attr("style", "display:none;")
        $("#lblbankledger1").html("Ledger");
       // $("#LedgerID").attr("style", "display:block;");
        
    }

}

function paymentTypeChange2() {
   
    $("#dvtype3").attr("style", "display:block;");
    var option = $("#ddl_type3").val();

    if (option == "Cash") {
        $("#divbankgl2").attr("style", "display:none;")
        $("#divbank2").attr("style", "display:none;")
    }

    else if (option == "Bank") {
        $("#divbankgl2").attr("style", "display:block;")
        $("#divbank2").attr("style", "display:block;")
        $("#lblbankledger2").html("Bank");
    }

    else if (option == "Ledger") {
        $("#divbankgl2").attr("style", "display:block;")
        $("#divbank2").attr("style", "display:none;")
        $("#lblbankledger2").html("Ledger");
    }


}