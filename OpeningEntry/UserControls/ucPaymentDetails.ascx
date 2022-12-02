<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPaymentDetails.ascx.cs" Inherits="OpeningEntry.UserControls.ucPaymentDetails" %>
  <link rel="stylesheet" href="https://unpkg.com/flatpickr/dist/flatpickr.min.css">
      <link href="../../../assests/bootstrap/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <script src="../../../assests/bootstrap/js/bootstrap-datetimepicker.min.js"></script>

    <script src="https://unpkg.com/flatpickr"></script>  
<style>
      .table-duplicate {
            width:100%;
        }
        .table-duplicate td>label {
            background:#215b61;
            color:#fff;
            
            padding:2px 5px;
        }
         .NotValid {
             border-color: #a43a3a !important;
         }
         #paymentDetails>tr>td {
             padding-right:15px !important;
         }
</style>
<script>

    var JsonMainAccount;
    $(document).ready(function () {


        JsonMainAccount = JSON.parse(document.getElementById('hdJsonMainAccountString').value);
        FetchEnteredPaymentDetails();
        $(".flatpickr").flatpickr({
            enableTime: true,
            weekNumbers: true,
            dateFormat: "d-m-Y"

        });
        $('body').addClass('mini-navbar');
        $('html, body').animate({ scrollTop: $('#paymentDetails').offset().top }, 1000);
    });


    function FetchEnteredPaymentDetails() {
        if (cClientSaveData.Get('0')) {
            document.getElementById("paymentDetails").deleteRow(0);
            SetEnteredPaymentDetailsData(0);
        }
    }

    function SetEnteredPaymentDetailsData(count) {

        if (cClientSaveData.Get(count + '')) {
            var data = cClientSaveData.Get(count + '');
            var extractData = data.split('|~|');
            var row = AddPaymentRowOnEdit();
            SetPaymentRowValue(row, extractData);
            SetEnteredPaymentDetailsData(count + 1);
        }
    }

    function SetPaymentRowValue(row, paymentType) {



        row.children[1].innerHTML = GetHtml1(paymentType[0]);
        row.children[2].innerHTML = GetHtml2(paymentType[0]);
        row.children[3].innerHTML = GetHtml3(paymentType[0]);
        row.children[4].innerHTML = GetHtml4(paymentType[0]);
        row.children[5].innerHTML = GetHtml5(paymentType[0]);
        row.children[6].innerHTML = GetHtml6(paymentType[0]);
        row.children[7].innerHTML = GetHtml7(paymentType[0]);


        //SetValue 
        row.children[0].children[1].value = paymentType[0];
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
            row.children[2].children[1].flatpickr({
                enableTime: false,
                weekNumbers: false,
                dateFormat: "d-m-Y"

            });
        }
        if (paymentType[0] == 'Cheque') {
            row.children[3].children[1].flatpickr({
                enableTime: false,
                weekNumbers: false,
                dateFormat: "d-m-Y"

            });
        }


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


    function validatePaymentDetails(row) {
        for (var i = 0; i < row.children.length; i++) {
            if (row.children[0].children[1].value == "Card") {

                if (row.children[1].children[1].value.trim() == "") {
                    row.children[1].children[1].className = "NotValid";
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
                    row.children[3].children[1].className = "";
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

    function AddNewPayment(e) {

        if (!validatePaymentDetails(e.target.parentNode.parentNode.parentNode)) {
            return false;
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

    function removeExecutive(obj) {
        var rowIndex = obj.rowIndex;
        var table = document.getElementById("paymentDetails");
        if (table.rows.length > 1) {
            table.rows[table.rows.length - 2].children[7].children[0].className = '';
            table.rows[table.rows.length - 2].children[0].children[1].removeAttribute('disabled');
            table.deleteRow(rowIndex);
        } else {
            jAlert('Cannot delete all Payment Methods.');
        }
    }

    function paymentTypeChange(e) {
        var tableRow = e.target.parentNode.parentNode;

        tableRow.children[1].innerHTML = GetHtml1(e.srcElement.value);
        tableRow.children[2].innerHTML = GetHtml2(e.srcElement.value);

        tableRow.children[3].innerHTML = GetHtml3(e.srcElement.value);
        tableRow.children[4].innerHTML = GetHtml4(e.srcElement.value);
        tableRow.children[5].innerHTML = GetHtml5(e.srcElement.value);
        tableRow.children[6].innerHTML = GetHtml6(e.srcElement.value);
        tableRow.children[7].innerHTML = GetHtml7(e.srcElement.value);


        //Date control initilize
        if (e.srcElement.value == 'Cheque' || e.srcElement.value == 'E Transfer') {
            tableRow.children[2].children[1].flatpickr({
                enableTime: false,
                weekNumbers: false,
                dateFormat: "d-m-Y"

            });
        }
        if (e.srcElement.value == 'Cheque') {
            tableRow.children[3].children[1].flatpickr({
                enableTime: false,
                weekNumbers: false,
                dateFormat: "d-m-Y"

            });
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

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode

        if (charCode == 46)
            return true;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }


    function GetHtml1(type) {
        var inrHtml = '&nbsp;';
        if (type == "Card") {
            inrHtml = '  <label>Enter Card No</label> <input type="text" />';
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
            inrHtml = '<label>Select Card Type</label>  <select class="form-control"><option>Visa</option><option>Credit</option><option>Debit</option></select>';
        }
        else if (type == 'Cheque') {
            inrHtml = '  <label>Enter Date</label> <input type="text" />';
        }
        else if (type == 'E Transfer') {
            inrHtml = '  <label>Enter Date</label> <input type="text" class="flatpickr"/>';
        }
        return inrHtml;
    }

    function GetHtml3(type) {
        var inrHtml = '&nbsp;';
        if (type == "Card") {
            inrHtml = '  <label>Authorization No</label> <input type="text" />';
        }
        else if (type == 'Cheque') {
            inrHtml = '  <label>Drawee Date</label> <input type="text" />';
        }
        return inrHtml;
    }

    function GetHtml4(type) {
        var inrHtml = '&nbsp;';
        if (type == "Card" || type == 'Cheque') {
            inrHtml = '  <label>Remarks ( if any )</label> <input type="text" />';
        }
        return inrHtml;
    }

    function GetHtml5(type) {
        var inrHtml = '&nbsp;';
        if (type == "Card" || type == 'Cheque' || type == 'Coupon' || type == 'E Transfer') {
            inrHtml = '  <label>Select Ledger</label>';

            //Create The ledger DropDown
            inrHtml += '<select class="form-control">';

            //Load ledger options
            var optionsHtml = ''
            for (var i = 0; i < JsonMainAccount.length; i++) {
                optionsHtml += '<option value="' + JsonMainAccount[i].MainAccount_AccountCode + '">' + JsonMainAccount[i].MainAccount_Name + '</option>'
            }

            inrHtml += optionsHtml;
            inrHtml += '</select>'



        }
        return inrHtml;
    }


    function GetHtml6(type) {
        var inrHtml = '&nbsp;';

        if (type == "Card" || type == 'Cheque' || type == 'Coupon' || type == 'E Transfer') {
            inrHtml = '  <label>Enter Amount</label> <input type="text" onkeypress="return isNumberKey(event)"/>';
        }

        return inrHtml;

    }

    function GetHtml7(type) {
        var inrHtml = '&nbsp;';
        if (type == "Card" || type == 'Cheque' || type == 'Coupon' || type == 'E Transfer') {
            inrHtml = ' <a href="javascript:void(0)" style="margin-top: 18px;margin-left: 10px;display:inline-block;margin-right:5px;" onClick="AddNewPayment(event)"><img src="/assests/images/add.png"></a>  <a href="javascript:void(0)" onclick="removeExecutive(this.parentNode.parentNode)" ><img src="/assests/images/crs.png"></a>';
        }
        return inrHtml;

    }

    function SelectAllData(callback) {
        cClientSaveData.Clear();
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
            }
            else if (select.value == "Coupon") {
                var data = "Coupon";
                data += '|~|' + row.children[1].children[1].value;//coupon 
                data += '|~|' + row.children[5].children[1].value; // Amount
                data += '|~|' + row.children[6].children[1].value; // Amount


                cClientSaveData.Set(i, data);
            }
            else if (select.value == "E Transfer") {
                var data = "E Transfer";
                data += '|~|' + row.children[1].children[1].value;//instrument no
                data += '|~|' + row.children[2].children[1].value;//enter date 
                data += '|~|' + row.children[5].children[1].value; // Main Account
                data += '|~|' + row.children[6].children[1].value; // Amount
                cClientSaveData.Set(i, data);
            }

        }

        //Add Cash Invoice
        var data = "Cash";
        data += '|~|' + ccmbUcpaymentCashLedger.GetValue();
        data += '|~|' + $('#cmbUcpaymentCashLedgerAmt').val();
        cClientSaveData.Set(i, data);

        if (callback) {
            callback();
        }
    }

</script>




            <dxe:ASPxHiddenField runat="server" Id="ClientSaveData" ClientInstanceName="cClientSaveData"></dxe:ASPxHiddenField>
            <asp:HiddenField ID="hdJsonMainAccountString" runat="server" />
       <section class="rds col-md-12">
                     <div class="clearfix">
                         <span class="fieldsettype">Cash Invoice Payment Details</span>
                         <table class="pull-right pad">
                             <tbody><tr>
                                 <td><span style="background: #215b61;color: #fff;padding: 2px 5px;">Select Ledger(Cash)</span></td>
                                 <td>
                                      <dxe:ASPxComboBox ID="cmbUcpaymentCashLedger" runat="server" ClientIDMode="Static" ClientInstanceName="ccmbUcpaymentCashLedger" SelectedIndex="0"  Width="100%">
                                      </dxe:ASPxComboBox>
                                 </td>
                                 <td class="hd"><span style="background: #215b61;color: #fff;padding: 2px 5px;">Amount</span></td>
                                 <td><input type="text" id="cmbUcpaymentCashLedgerAmt"></td>
                             </tr>
                         </tbody></table>
                     </div>


                     <table class="table-duplicate" id="PaymentTable">
                         <tbody id="paymentDetails">
                             <tr>
                             <td width="10%">
                                 <label>Payment Type</label>
                                 <select class="form-control" onchange="paymentTypeChange(event)">
                                      <option>-Select-</option>
                                     <option>Card</option>
                                     <option>Cheque</option>
                                     <option>Coupon</option>
                                     <option>E Transfer</option>
                                 </select>
                             </td>
                             
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                         </tr>

                         
                         
                     </tbody></table>
                 </section>