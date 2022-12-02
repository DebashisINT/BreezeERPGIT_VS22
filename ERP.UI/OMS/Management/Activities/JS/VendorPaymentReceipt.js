

function SetLostFocusonDemand(e) {

    if ($("#ComboVoucherType").val() == "P") {
        if ((new Date($("#hdnLockFromDate").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
            jAlert("DATA is Freezed between   " + $("#hdnAddDataFFrom").val() + " to " + $("#hdnAddDataFTo").val() + " for Add");
        }
    }
    else if ($("#ddlNoteType").val() == "R") {
        if ((new Date($("#hdnLockFromDateCon").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDateCon").val()))) {
            jAlert("DATA is Freezed between   " + $("#hdnAddDataFFromCon").val() + " to " + $("#hdnAddDataFToCon").val() + " for Add");
        }
    }
}

function BindContactPerson(key, val) {

    $.ajax({
        type: "POST",
        url: "VendorPaymentReceipt.aspx/GetContactPersonafterBillingShipping",
        data: JSON.stringify({ Key: key, val: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (r) {
            var contactPersonJsonObject = r.d;
            //cContactPerson.SetValue(contactPerson);
            //IsContactperson = false;
            SetDataSourceOnComboBox(cContactPerson, contactPersonJsonObject);
            //SetFocusAfterBillingShipping();
        }
    });
}

function GlobalBillingShippingEndCallBack() {
    var key = GetObjectID('hdnCustomerId').value;//gridLookup.GetValue();
    if (key != null && key != '') {
        //cContactPerson.PerformCallback('BindContactPerson~' + key + '~');
        $("#hdnClearSession").val('');
        BindContactPerson(key, $("#hdnClearSession").val())
    }
}
var lastCRP = null;
var globalRowIndex;
var ReciptOldValue;
var ReciptNewValue;
var PaymentOldValue;
var PaymentNewValue;
//**********************************************Online  Print ************************************************************
var RecPayId = 0;
var DocType = "";
function onPrintJv(id, Type) {

    RecPayId = id;
    DocType = Type;
    cSelectPanel.PerformCallback('Bindsingledesign');
    $('#btnOK').focus();
}

function PerformCallToGridBind() {
    
    cSelectPanel.PerformCallback('Bindsingledesign');
    cDocumentsPopup.Hide();
    return false;
}


$(document).ready(function () {
    if($("#hdnPageStatus").val()=="update")
    {
        clookup_Project.SetEnabled(true);
    }
});


function cSelectPanelEndCall(s, e) {

    if (cSelectPanel.cpSuccess != "") {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = "";
        if (DocType == "P")
            reportName = "VendorPayment~D";
        else
            reportName = "VendorReceipt~D";
        var module = 'VENDRECPAY';
        if (TotDocument.length > 0) {
            for (var i = 0; i < TotDocument.length; i++) {
                if (TotDocument[i] != "") {
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId + '&PrintOption=' + TotDocument[i], '_blank')
                }
            }
        }
    }
    //if (cSelectPanel.cpSuccess == "") {
    //    if (cSelectPanel.cpChecked != "") {
    //        jAlert('Please check Original For Recipient and proceed.');
    //    }
    //    CselectOriginal.SetCheckState('UnChecked');
    //    CselectDuplicate.SetCheckState('UnChecked');
    //    CselectTriplicate.SetCheckState('UnChecked');
    //    cCmbDesignName.SetSelectedIndex(0);
    //}
}

//*********************************************************************************************************************************************************************




function selectValue() {
    var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Name</th><th>Unique Id</th><th>Type</th></tr><table>";
    document.getElementById("VendorTable").innerHTML = txt;
    ctxtVendorName.Focus();
}

function VendorButnClick(s, e) {
    var valid = true;
    var OrderNo = document.getElementById('txtVoucherNo').value;
    if (OrderNo == "") {
        jAlert('Please Enter Document Number.', 'Alert', function () {
            document.getElementById('txtVoucherNo').focus();
        });


    }

    else {

        var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Name</th><th>Unique Id</th><th>Type</th></tr><table>";
        document.getElementById("VendorTable").innerHTML = txt;
        setTimeout(function () { $("#txtVendSearch").focus(); }, 500);
        $('#VendorModel').modal('show');
        var type = ($("[id$='rdl_Contact']").find(":checked").val() != null) ? $("[id$='rdl_Contact']").find(":checked").val() : "";
        if (type == "DV") {
            $('#VendorModelName').text("Vendor Search");
        }
        else {
            $('#VendorModelName').text("Customer Search");
        }
    }
}
function Vendorkeydown(e) {
    var type = ($("[id$='rdl_Contact']").find(":checked").val() != null) ? $("[id$='rdl_Contact']").find(":checked").val() : "";
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtVendSearch").val();
    OtherDetails.type = type;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Type");

        if ($("#txtVendSearch").val() != "") {
            callonServer("Services/Master.asmx/GetVendorForVendorPayRec", OtherDetails, "VendorTable", HeaderCaption, "VendorIndex", "SetVendor");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[Vendorindex=0]"))
            $("input[Vendorindex=0]").focus();
    }
}
$(document).ready(function () {           
    $('#VendorModel').on('shown.bs.modal', function () {
        $('#txtVendSearch').val("");
        $('#txtVendSearch').focus();
    })
    var VoucherType = $("#ComboVoucherType").val();
    if (VoucherType == "P") {
        $('#tdsSection').show();
    }
    else {
        $('#tdsSection').hide();
    }
});

function VendorKeyDownDV(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "VendorIndex") {
                SetVendor(Id, name);
            }
            else if (indexName == "MainAccountIndex") {
                $('#MainAccountModel').modal('hide');

                var IsSub = e.target.parentElement.parentElement.children[2].innerText;
                var RevApp = e.target.parentElement.parentElement.children[3].innerText;
                if (RevApp == 'Yes') {
                    RevApp = '1';
                }
                else {
                    RevApp = '0';
                }
                var TaxAble = e.target.parentElement.parentElement.children[4].innerText;
                GetMainAcountComboBox(Id, name, IsSub, RevApp, TaxAble);
                grid.batchEditApi.StartEdit(globalRowIndex, 3);
            }
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
            if (indexName == "VendorIndex")
                $('#txtVendSearch').focus();
            else if (indexName == "MainAccountIndex")
                $('#txtMainAccountSearch').focus();
        }
    }
    else if (e.code == "Escape") {
        if (indexName == "MainAccountIndex") {
            $('#MainAccountModel').modal('hide');
            grid.batchEditApi.StartEdit(globalRowIndex, 2);
        }
    }
}


//End Pop Up

function aacpCheckAmountEndCall(s, e) {
         
    if (cacpCheckAmount.cpUnPaidAmount) {
        if (cacpCheckAmount.cpUnPaidAmount != null) {

            var RecieveValue = (parseFloat(grid.GetEditor('Receipt').GetValue()) != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";
            var PaymentValue = (parseFloat(grid.GetEditor('Payment').GetValue()) != null) ? parseFloat(grid.GetEditor('Payment').GetValue()) : "0";
            var UnPaidAmoun = parseFloat(cacpCheckAmount.cpUnPaidAmount);
            if (RecieveValue > UnPaidAmoun) {
                jAlert('Receipt amount cannot be more then the selected Document Amount.', 'Alert', function () {

                    //var vouchertype = cComboVoucherType.GetValue();
                    var vouchertype = document.getElementById("ComboVoucherType").value;
                    if (vouchertype == 'R') {
                        grid.batchEditApi.StartEdit(globalRowIndex, 4);
                        var newValue = grid.GetEditor('Receipt').GetValue();
                        var finalValue = newValue - UnPaidAmoun;
                        grid.GetEditor('Receipt').SetValue(UnPaidAmoun);
                        var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());

                        ctxtVoucherAmount.SetValue(VoucherAmount - finalValue);
                        c_txt_Debit.SetValue(VoucherAmount - finalValue);
                    }
                    else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 5);
                        var newValue = grid.GetEditor('Payment').GetValue();
                        var finalValue = newValue - UnPaidAmoun;
                        grid.GetEditor('Payment').SetValue(UnPaidAmoun);
                        var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());

                        ctxtVoucherAmount.SetValue(VoucherAmount - finalValue);
                        ctxtTotalPayment.SetValue(VoucherAmount - finalValue);
                    }

                });
            }
            else if (PaymentValue > UnPaidAmoun) {
                jAlert('Payment amount cannot be more then the selected Document Amount.', 'Alert', function () {

                    //var vouchertype = cComboVoucherType.GetValue();
                    var vouchertype = document.getElementById("ComboVoucherType").value;
                    if (vouchertype == 'R') {
                        grid.batchEditApi.StartEdit(globalRowIndex, 4);
                        var newValue = grid.GetEditor('Receipt').GetValue();
                        var finalValue = newValue - UnPaidAmoun;
                        grid.GetEditor('Receipt').SetValue(UnPaidAmoun);
                        var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());

                        ctxtVoucherAmount.SetValue(VoucherAmount - finalValue);
                        c_txt_Debit.SetValue(VoucherAmount - finalValue);
                    }
                    else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 5);
                        var newValue = grid.GetEditor('Payment').GetValue();
                        var finalValue = newValue - UnPaidAmoun;
                        grid.GetEditor('Payment').SetValue(UnPaidAmoun);
                        var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());

                        //ctxtVoucherAmount.SetValue(VoucherAmount - finalValue);
                        //ctxtTotalPayment.SetValue(VoucherAmount - finalValue);
                    }

                });
            }
            cacpCheckAmount.cpUnPaidAmount = null;

        }
        else {
            //jAlert('UDF is set as Mandatory. Please enter values.');
            cacpCheckAmount.cpUnPaidAmount = null;
        }
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
function CheckedChange() {

    if ($("#CB_GSTApplicable").is(':checked')) {
        cproductLookUp.SetEnabled(true);
    }
    else {
        cproductLookUp.SetEnabled(false);
        return;
    }

    var proMsg = 'Selected Product (s) to be cleaned if you unchecked this option.';
    var CB_GSTApplicable_Chk = $("#CB_GSTApplicable").prop("checked");

    if (cproductLookUp.GetValue() != null && CB_GSTApplicable_Chk == false) {

        jAlert(proMsg, 'Alert Dialog: [CustomerReceipt/Payment]', function (r) {
            if (r == true) {

                jConfirm('Are You Sure?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        cComponentPanel.PerformCallback("DeSelectAll");
                        $("#CB_GSTApplicable").prop("checked", false);
                    }
                    else {
                        $("#CB_GSTApplicable").prop("checked", true);
                    }
                });
            }
            else {
                $("#CB_GSTApplicable").prop("checked", true);
            }
        });
    }
}

function cmbContactPersonEndCall(s, e) {
    if (cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) {
        pageheaderContent.style.display = "block";
        $("#divGSTIN").attr('style', 'display:block');
        document.getElementById('lblGSTIN').innerHTML = cContactPerson.cpGSTN;
        cContactPerson.cpGSTN = null;
    }
}
function ddlBranch_SelectedIndexChanged() {
    LoadCustomerAddress("", $('#ddlBranch').val(), 'VPR');
   // clookup_Project.gridView.Refresh();
}
$(document).ready(function () {
           
    var strDate = new Date();

    if (getUrlVars().key == "ADD") {
        page.tabs[1].SetEnabled(true);
        //var dt = new Date();
        //cInstDate.SetDate(dt);
        // cdtTDate.SetDate(strDate);
        //Type.AddItem("Ledger", "Ledger");

    }
        /*--------------------- For append ledger in type dropdown Arindam 05-02-2019--------*/
    else {
        //Type.AddItem("Ledger", "Ledger");
    }
    /*--------------------- For append ledger in type dropdown--------*/

    var isCtrl = false;
           
    document.onkeydown = function (e) {
        if (event.keyCode == 78 && event.altKey == true && document.getElementById("tdSaveButtonNew").style.display != "none") {

            SaveButtonClickNew();//........Alt+N
        }
        else if (event.keyCode == 88 && event.altKey == true && document.getElementById("tdSaveButton").style.display != "none") {

            SaveButtonClick();//........Alt+X
        }
        else if (event.keyCode == 85 && event.altKey == true && document.getElementById("tdUdfButton").style.display != "none") {
            OpenUdf();
        }
        else if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+O -- ie, Billing/Shipping Samrat!     
            StopDefaultAction(e);
            if (page.GetActiveTabIndex() == 1) {
                fnSaveBillingShipping();
            }
        }

    }
});
function InstrumentDate_GotFocus() {
    var dt = new Date();
    cInstDate.SetDate(dt);
    cInstDate.ShowDropDown();
}
function InstrumentType_GotFocus() {
    cComboInstrumentTypee.ShowDropDown();
}
function CurrencyGotFocus() {
    cCmbCurrency.ShowDropDown();
}
function CashBank_GotFocus() {
    cddlCashBank.ShowDropDown();
}
function Customer_GotFocus() {
    // gridLookup.ShowDropDown();
}
function TransDate_GotFocus() {
    cdtTDate.ShowDropDown();
}
function VoucherType_GotFocus() {
    cComboVoucherType.ShowDropDown();
}
function NumberingScheme_GotFocus() {
    cCmbScheme.ShowDropDown();
}
//Code for UDF Control 
function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=VPR&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}
function acbpCrpUdfEndCall(s, e) {
    if (cacbpCrpUdf.cpUDF) {
        if (cacbpCrpUdf.cpUDF == "true") {
            grid.batchEditApi.StartEdit(-1, 2);
            grid.batchEditApi.StartEdit(0, 2);
            grid.AddNewRow();
            grid.UpdateEdit();
            cacbpCrpUdf.cpUDF = null;
        }
        else {
            jAlert('UDF is set as Mandatory. Please enter values.');
            cacbpCrpUdf.cpUDF = null;
        }
    }
}
// End Udf Code
function PerformCallToGridBind() {
    if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.Clear();
    document.getElementById('hdnRefreshType').value = "";
    grid.PerformCallback('BindGridOnSalesInvoice' + '~' + '@');
    cPopup_invoice.Hide();
    OnAddNewClick();
    return false;
}
function CashBank_SelectedIndexChanged() {
    $('#MandatoryCashBank').hide();
    var CashBankText = cddlCashBank.GetText();
    var SpliteDetails = CashBankText.split(']');
    var WithDrawType = SpliteDetails[1].trim();
    if (WithDrawType == "Cash") {
        var comboitem = cComboInstrumentTypee.FindItemByValue('C');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('D');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('E');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);

        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
        if (comboitem == undefined || comboitem == null) {
            cComboInstrumentTypee.AddItem("Cash", "CH");
        }
        cComboInstrumentTypee.SetValue("CH");
        InstrumentTypeSelectedIndexChanged();
    }
    else {
        var comboitem = cComboInstrumentTypee.FindItemByValue('C');
        if (comboitem == undefined || comboitem == null) {
            cComboInstrumentTypee.AddItem("Cheque", "C");
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('D');
        if (comboitem == undefined || comboitem == null) {
            cComboInstrumentTypee.AddItem("Draft", "D");
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('E');
        if (comboitem == undefined || comboitem == null) {
            cComboInstrumentTypee.AddItem("E.Transfer", "E");
        }
        var comboitem = cComboInstrumentTypee.FindItemByValue('CH');
        if (comboitem != undefined && comboitem != null) {
            cComboInstrumentTypee.RemoveItem(comboitem.index);
            cComboInstrumentTypee.SetValue("C");
            InstrumentTypeSelectedIndexChanged();
        }
    }
}

function txtNarrationTextChanged() {

}
function checkTextAreaMaxLength(textBox, e, length) {

    var mLen = textBox["MaxLength"];
    if (null == mLen)
        mLen = length;

    var maxLength = parseInt(mLen);
    //if (!checkSpecialKeys(e)) {
    if (textBox.value.length > maxLength - 1) {
        if (window.event)//IE
            e.returnValue = false;
        else//Firefox
            e.preventDefault();
    }
    // }
}
//..................Sales Invoice PopUp...............
function GetInvoiceMsg(s, e) {
         
    var salesInvoice = document.getElementById('hdnSalesInvoice').value;
    if (salesInvoice == "Yes") {
        jConfirm('Wish to auto adjust amount with Purchase Invoice(s)?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                cPopup_invoice.Show();
                var amount = ctxtVoucherAmount.GetValue();
                /*Abhisek
                var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                */
                var customerval = GetObjectID('hdnCustomerId').value;// gridLookup.GetValue();
                $('#hdfLookupCustomer').val(customerval);
                cgrid_SalesInvoice.PerformCallback('BindSalesInvoiceDetails' + '~' + amount);
            }
            else {
                grid.batchEditApi.StartEdit(-1, 1);
            }
        });
    }

    SetTDSAmount();
}
function ChangeState(value) {

    cgrid_SalesInvoice.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}
//..................Voucher Amount  & Total Amount 

function CreditGotFocus(s, e) {

    PaymentOldValue = s.GetValue();
    var indx = PaymentOldValue.indexOf(',');
    if (indx != -1) {
        PaymentOldValue = PaymentOldValue.replace(/,/g, '');
    }
}
function DebitGotFocus(s, e) {
    ReciptOldValue = s.GetText();
    var indx = ReciptOldValue.indexOf(',');
    if (indx != -1) {
        ReciptOldValue = ReciptOldValue.replace(/,/g, '');
    }
}
function PaymentTextChange(s, e) {
           
    var PaymentValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0";
    var ReceiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";

    if (PaymentValue > 0) {
        recalculateReceipt(grid.GetEditor('Receipt').GetValue());
        grid.GetEditor('Receipt').SetValue("0");
    }


    Payment_Lost_Focus(s, e);


    //..................CheckAmount.......................
    var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
    var DocumentNo = (grid.GetEditor('DocumentID').GetValue() != null) ? grid.GetEditor('DocumentID').GetValue() : "0";
    var IsOpening = (grid.GetEditor('IsOpening').GetValue() != null) ? grid.GetEditor('IsOpening').GetValue() : "0";
    cacpCheckAmount.PerformCallback(Type + '~' + DocumentNo + '~' + IsOpening);
    //.................End.........................

    SetTDSAmount();
}
function recalculateReceipt(oldVal) {
    if (oldVal != 0) {
        ReciptNewValue = 0;
        ReciptOldValue = oldVal;
        changeReciptTotalSummary();
    }
}
function changeReciptTotalSummary() {
           
    var newDif = ReciptOldValue - ReciptNewValue;
    var CurrentSum = c_txt_Debit.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    c_txt_Debit.SetValue(parseFloat(CurrentSum - newDif));
    var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
    if (Type == 'Advance') {
        ctxtVoucherAmount.SetValue(parseFloat(CurrentSum - newDif));
    }
    if (Type == 'OnAccount') {
        ctxtVoucherAmount.SetValue(parseFloat(CurrentSum - newDif));
    }
    //ctxtVoucherAmount.SetValue(parseFloat(CurrentSum - newDif));

}
function Payment_Lost_Focus(s, e) {
    PaymentNewValue = s.GetText();
    var indx = PaymentNewValue.indexOf(',');
    if (indx != -1) {
        PaymentNewValue = PaymentNewValue.replace(/,/g, '');
    }

    if (PaymentOldValue != PaymentNewValue) {
        changePaymentTotalSummary();
    }
}
function OnKeyDown(s, e) {

    if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
        return ASPxClientUtils.PreventEvent(e.htmlEvent);
}
function changePaymentTotalSummary() {
          
    var newDif = PaymentOldValue - PaymentNewValue;
    var CurrentSum = ctxtTotalPayment.GetValue();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    //ctxtTotalPayment.SetValue(parseFloat(CurrentSum - newDif));
    //ctxtVoucherAmount.SetValue(parseFloat(CurrentSum - newDif));

    /*----- total sum Arindam 06-02-2019*/
    var totamnt = GetTotalAmount();
    // ctxtTotalPayment.SetValue(totamnt);
    ctxtVoucherAmount.SetValue(totamnt);

}
function recalculatePayment(oldVal) {

    if (oldVal != 0) {
        PaymentNewValue = 0;
        PaymentOldValue = oldVal;
        changePaymentTotalSummary();
    }
}
function ReceiptLostFocus(s, e) {
    ReciptNewValue = s.GetText();
    var indx = ReciptNewValue.indexOf(',');

    if (indx != -1) {
        ReciptNewValue = ReciptNewValue.replace(/,/g, '');
    }
    if (ReciptOldValue != ReciptNewValue) {
        changeReciptTotalSummary();
    }
}
function ReceiptTextChange(s, e) {
           
    var VoucherType = $("#ComboVoucherType").val();
    ReceiptLostFocus(s, e);
    var RecieveValue = (grid.GetEditor('Receipt').GetValue() != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";
    var receiptValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0";

    if (RecieveValue > 0) {
        recalculatePayment(grid.GetEditor('Payment').GetValue());
        grid.GetEditor('Payment').SetValue("0");
    }
    //..................CheckAmount.......................
    var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
    var DocumentNo = (grid.GetEditor('DocumentID').GetValue() != null) ? grid.GetEditor('DocumentID').GetValue() : "0";
    var IsOpening = (grid.GetEditor('IsOpening').GetValue() != null) ? grid.GetEditor('IsOpening').GetValue() : "0";
    cacpCheckAmount.PerformCallback(Type + '~' + DocumentNo + '~' + IsOpening);
    if (Type != 'Ledger' && VoucherType == 'P') {
        grid.GetEditor('Receipt').SetValue("0");
        c_txt_Debit.SetValue("0.00");
    }
    /*-------- Substarct Receipt Amount Arindam 05-02-2019*/
    //if(RecieveValue!="")
    //{ 
    //    var VoucherAmnt = parseFloat(ctxtVoucherAmount.GetValue()) - parseFloat(RecieveValue);
    //    var TotPayment = parseFloat(ctxtTotalPayment.GetValue()) - parseFloat(RecieveValue);
    //    ctxtTotalPayment.SetValue(TotPayment);
    //    ctxtVoucherAmount.SetValue(VoucherAmnt);
    //}

    var totamnt = GetTotalAmount();

    ctxtVoucherAmount.SetValue(totamnt);
    //.................End.........................

    SetTDSAmount();

            

}

function GetServerDateFormat(today) {
    if (today != "" && today != null) {
        var dd = today.getDate();
        var mm = today.getMonth() + 1;
        var yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = yyyy + '-' + mm + '-' + dd;
    }
    else {
        today = "";
    }

    return today;
}


//..........Save & New.....
function SaveButtonClickNew() {
    cLoadingPanelCRP.Show();
    $('#hdnBtnClick').val("Save_New");

    $('#hdnRefreshType').val('N');
    $('#hdfIsDelete').val('I');

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
       
        cLoadingPanelCRP.Hide();
        jAlert("Please Select Project.");
        return false;
    }

    if (document.getElementById('txtVoucherNo').value == "") {
        cLoadingPanelCRP.Hide();
        $("#MandatoryBillNo").show();
        return false;
    }
    var TransDate = cdtTDate.GetDate();
    if (TransDate == null) {
        cLoadingPanelCRP.Hide();
        $("#MandatoryTransDate").show();
        return false;
    }
    var branch = $("#ddlBranch").val();
    if (branch == "") {
        cLoadingPanelCRP.Hide();
        $("#MandatoryBranch").show();
        return false;
    }
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        cLoadingPanelCRP.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
    var CashBank = cddlCashBank.GetValue();
    if (CashBank == null) {
        cLoadingPanelCRP.Hide();
        $("#MandatoryCashBank").show();
        return false;
    }
    var VoucherAmount = ctxtVoucherAmount.GetValue();
    if (parseFloat(VoucherAmount) == 0) {
        jAlert("Voucher amount must be greater then ZERO.");
        cLoadingPanelCRP.Hide();
        return false;
    }

    //Code added by Sudip
    grid.batchEditApi.EndEdit();
    var gridCount = grid.GetVisibleRowsOnPage();

    var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
    var txtTotalPayment = ctxtTotalPayment.GetValue() != null ? ctxtTotalPayment.GetValue() : 0;
    var VoucherType = $("#ComboVoucherType").val(); // cComboVoucherType.GetValue();

    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Type')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Type')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (gridCount > 0) {

        if (IsType == "Y") {
            /*var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";*/
            var customerval = GetObjectID('hdnCustomerId').value;// gridLookup.GetValue();
            $('#hdfLookupCustomer').val(customerval);
            if (VoucherType == "P") {
                if (parseFloat(txtTotalAmount) <= parseFloat(txtTotalPayment)) {
                    // grid.UpdateEdit();

                    var urlKeys = getUrlVars();
                    var VendorPaymentID;
                    if (urlKeys.key != 'ADD') {
                        VendorPaymentID = urlKeys.key;
                    }
                    else {
                        VendorPaymentID = 0;
                    }
                    var PostingDate = GetServerDateFormat(cdtTDate.GetValue());
                          
                    $.ajax({
                        type: "POST",
                        url: "VendorPaymentReceipt.aspx/GetTotalBalanceByCashBankID",
                        data: JSON.stringify({ CashBankID: CashBank, VendorPaymentID: VendorPaymentID, PostingDate: PostingDate }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var data = msg.d;
                            var VoucherAmount = data.toString().split('~')[0];
                            var BalanceLimit = data.toString().split('~')[1];
                            var BalanceExceed = data.toString().split('~')[2];
                            var closingBalnc = data.toString().split('~')[3];
                            if (BalanceLimit != '0.00') {
                                //Rev Tanmoy
                                //var TotalVoucherAmount = parseFloat(txtTotalPayment) + parseFloat(VoucherAmount);
                                var TotalVoucherAmount = parseFloat(txtTotalPayment) - parseFloat(txtTotalAmount);
                                var closingAmount = parseFloat(closingBalnc) - parseFloat(TotalVoucherAmount);
                                // if (parseFloat(TotalVoucherAmount) > parseFloat(BalanceLimit)) {
                                if (parseFloat(closingAmount) < parseFloat(BalanceLimit)) {
                                    //End Rev Tanmoy
                                    if (BalanceExceed.trim() == 'W') {

                                        jConfirm('Cash/Bank Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                            if (r == true) {
                                                //OnAddNewClick();
                                                //cbtnSaveNew.SetVisible(false);
                                                //cbtnSaveRecords.SetVisible(false);


                                                cacbpCrpUdf.PerformCallback();

                                            }
                                            else {
                                                cLoadingPanelCRP.Hide();
                                            }
                                        });
                                    }
                                    else if (BalanceExceed.trim() == 'B') {
                                        jAlert('Cash/Bank Balance - Limit is exceed can not proceed');
                                        cLoadingPanelCRP.Hide();

                                    }
                                    else if (BalanceExceed.trim() == 'I') {
                                        //OnAddNewClick();
                                        //cbtnSaveNew.SetVisible(false);
                                        //cbtnSaveRecords.SetVisible(false);

                                        cacbpCrpUdf.PerformCallback();

                                    }
                                    else if (BalanceExceed.trim() == 'S') {
                                        jAlert('Please Selete Cash/Bank Balance - Limit exceed option');
                                        cLoadingPanelCRP.Hide();

                                    }
                                    //else if (BalanceExceed.trim() == '') {
                                    //    jAlert('Please Selete Cash/Bank Balance - Limit exceed option');

                                    //}
                                }
                                else {
                                    //OnAddNewClick();
                                    //cbtnSaveNew.SetVisible(false);
                                    //cbtnSaveRecords.SetVisible(false);

                                    cacbpCrpUdf.PerformCallback();
                                }
                            }
                            else {
                                //OnAddNewClick();
                                //cbtnSaveNew.SetVisible(false);
                                //cbtnSaveRecords.SetVisible(false);

                                cacbpCrpUdf.PerformCallback();
                            }

                        }
                    });










                    // cacbpCrpUdf.PerformCallback();
                }
                else {
                    cLoadingPanelCRP.Hide();
                    jAlert('Payment amount can not be less than receipt amount ');
                }
            }
            if (VoucherType == "R") {
                if (txtTotalAmount >= txtTotalPayment) {
                    // grid.UpdateEdit();
                    cacbpCrpUdf.PerformCallback();

                }
                else {
                    cLoadingPanelCRP.Hide();
                    jAlert('Receipt amount can not be less than payment amount');
                }
            }
        }
        else {
            cLoadingPanelCRP.Hide();
            jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
        }
    }
    else {
        cLoadingPanelCRP.Hide();
        jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
    }
}
function SaveButtonClick() {
    cLoadingPanelCRP.Show();
    $('#hdnBtnClick').val("Save_Exit");
    $('#hdnRefreshType').val('E');
    $('#hdfIsDelete').val('I');

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {

        cLoadingPanelCRP.Hide();
        jAlert("Please Select Project.");
        return false;
    }

    if (document.getElementById('txtVoucherNo').value == "") {
        cLoadingPanelCRP.Hide();
        $("#MandatoryBillNo").show();
        return false;
    }
    var TransDate = cdtTDate.GetDate();
    if (TransDate == null) {
        cLoadingPanelCRP.Hide();
        $("#MandatoryTransDate").show();
        return false;
    }
    var branch = $("#ddlBranch").val();
    if (branch == "") {
        cLoadingPanelCRP.Hide();
        $("#MandatoryBranch").show();
        return false;
    }
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        cLoadingPanelCRP.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
    var CashBank = cddlCashBank.GetValue();
    if (CashBank == null) {
        cLoadingPanelCRP.Hide();
        $("#MandatoryCashBank").show();
        return false;
    }

    var VoucherAmount = ctxtVoucherAmount.GetValue();
    if (parseFloat(VoucherAmount) == 0) {
        jAlert("Voucher amount must be greater then ZERO.");
        cLoadingPanelCRP.Hide();
        return false;
    }
    //Code added by Sudip
    grid.batchEditApi.EndEdit();
    var gridCount = grid.GetVisibleRowsOnPage();

    var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
    var txtTotalPayment = ctxtTotalPayment.GetValue() != null ? ctxtTotalPayment.GetValue() : 0;
    var VoucherType = $("#ComboVoucherType").val(); //cComboVoucherType.GetValue();
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Type')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Type')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (gridCount > 0) {

        if (IsType == "Y") {
            /* Abhisek
            var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            */
            var customerval = GetObjectID('hdnCustomerId').value;// gridLookup.GetValue();
            $('#hdfLookupCustomer').val(customerval);
            if (VoucherType == "P") {
                if (parseFloat(txtTotalAmount) <= parseFloat(txtTotalPayment)) {

                    var urlKeys = getUrlVars();
                    var VendorPaymentID;
                    if (urlKeys.key != 'ADD') {
                        VendorPaymentID = urlKeys.key;
                    }
                    else {
                        VendorPaymentID = 0;
                    }

                    var PostingDate = GetServerDateFormat(cdtTDate.GetValue());
                    $.ajax({
                        type: "POST",
                        url: "VendorPaymentReceipt.aspx/GetTotalBalanceByCashBankID",
                        data: JSON.stringify({ CashBankID: CashBank, VendorPaymentID: VendorPaymentID, PostingDate: PostingDate }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var data = msg.d;
                            var VoucherAmount = data.toString().split('~')[0];
                            var BalanceLimit = data.toString().split('~')[1];
                            var BalanceExceed = data.toString().split('~')[2];
                            var closingBalnc = data.toString().split('~')[3];
                            if (BalanceLimit != '0.00') {
                                //Rev Tanmoy
                                //var TotalVoucherAmount = parseFloat(txtTotalPayment) + parseFloat(VoucherAmount);
                                var TotalVoucherAmount = parseFloat(txtTotalPayment) - parseFloat(txtTotalAmount);
                                var closingAmount = parseFloat(closingBalnc) - parseFloat(TotalVoucherAmount);
                                if (parseFloat(closingAmount) < parseFloat(BalanceLimit)) {
                                    // if (parseFloat(TotalVoucherAmount) > parseFloat(BalanceLimit)) {
                                    //End Rev Tanmoy
                                    if (BalanceExceed.trim() == 'W') {

                                        jConfirm('Cash/Bank Balance - Limit is exceed do you want to proceed?', 'Confirmation Dialog', function (r) {
                                            if (r == true) {
                                                //OnAddNewClick();
                                                //cbtnSaveNew.SetVisible(false);
                                                //cbtnSaveRecords.SetVisible(false);


                                                cacbpCrpUdf.PerformCallback();

                                            }
                                            else {
                                                cLoadingPanelCRP.Hide();
                                            }
                                        });
                                    }
                                    else if (BalanceExceed.trim() == 'B') {
                                        jAlert('Cash/Bank Balance - Limit is exceed can not proceed');
                                        cLoadingPanelCRP.Hide();

                                    }
                                    else if (BalanceExceed.trim() == 'I') {
                                        //OnAddNewClick();
                                        //cbtnSaveNew.SetVisible(false);
                                        //cbtnSaveRecords.SetVisible(false);

                                        cacbpCrpUdf.PerformCallback();

                                    }
                                    else if (BalanceExceed.trim() == 'S') {
                                        jAlert('Please Selete Cash/Bank Balance - Limit exceed option');
                                        cLoadingPanelCRP.Hide();

                                    }
                                    //else if (BalanceExceed.trim() == '') {
                                    //    jAlert('Please Selete Cash/Bank Balance - Limit exceed option');

                                    //}
                                }
                                else {
                                    //OnAddNewClick();
                                    //cbtnSaveNew.SetVisible(false);
                                    //cbtnSaveRecords.SetVisible(false);

                                    cacbpCrpUdf.PerformCallback();
                                }
                            }
                            else {
                                //OnAddNewClick();
                                //cbtnSaveNew.SetVisible(false);
                                //cbtnSaveRecords.SetVisible(false);

                                cacbpCrpUdf.PerformCallback();
                            }

                        }
                    });


                    //cacbpCrpUdf.PerformCallback();
                }
                else {
                    cLoadingPanelCRP.Hide();
                    jAlert('Payment amount can not be less than receipt amount ');
                }
            }
            if (VoucherType == "R") {
                if (txtTotalAmount >= txtTotalPayment) {
                    // grid.UpdateEdit();
                    cacbpCrpUdf.PerformCallback();
                }
                else {
                    cLoadingPanelCRP.Hide();
                    jAlert('Receipt amount can not be less than payment amount');
                }
            }
        }
        else {
            cLoadingPanelCRP.Hide();
            jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
        }
    }
    else {
        cLoadingPanelCRP.Hide();
        jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
    }
}
function BtnVisible() {
    document.getElementById('btnSaveNew').style.display = 'none'
    document.getElementById('btnSaveRecords').style.display = 'none'
    document.getElementById('tagged').style.display = 'block'
}

function viewOnly() {
    $('.form_main').find('input, textarea, button, select').attr('disabled', 'disabled');

    grid.SetEnabled(false);
    //cComboVoucherType.SetEnabled(false);
    $("#ComboVoucherType").attr('disabled', 'disabled');

    cdtTDate.SetEnabled(false);
    $('#ddlBranch').attr('disabled', 'disabled');
    gridLookup.SetEnabled(false);
    cContactPerson.SetEnabled(false);
    cddlCashBank.SetEnabled(false);
    cCmbCurrency.SetEnabled(false);
    ctxtRate.SetEnabled(false);
    cComboInstrumentTypee.SetEnabled(false);
    if (cComboInstrumentTypee.GetValue() != "CH") {
        ctxtInstNobth.SetEnabled(false);
        cInstDate.SetEnabled(false);
    }


    $('#txtNarration').attr('disabled', 'disabled');
    ctxtVoucherAmount.SetEnabled(false);

    cbtnSaveNew.SetVisible(false);
    cbtnSaveRecords.SetVisible(false);
    cbtn_SaveUdf.SetVisible(false);
}
//..................Batch Grid.....................
function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}
function grid_SalesInvoiceOnEndCallback(s, e) {
    //if (cgrid_SalesInvoice.cpOKVisible != null) {
    //    if (cgrid_SalesInvoice.cpOKVisible == "False") {
    //        cbtnOK.SetVisible(false);
    //        cgrid_SalesInvoice.cpOKVisible = null;
    //    }
    //}

    if (cgrid_SalesInvoice.cpOKVisible != null) {
        if (cgrid_SalesInvoice.cpOKVisible == "False") {
            cbtnOK.SetVisible(false);
            cgrid_SalesInvoice.cpOKVisible = null;
        }
        else {
            cbtnOK.SetVisible(true);
            cgrid_SalesInvoice.cpOKVisible = null;
        }
    }
    else {
        cbtnOK.SetVisible(true);

    }


}
function OnCustomButtonClick(s, e) {
  
    if (e.buttonID == 'CustomDelete') {
        //if (grid.GetVisibleRowsOnPage() > 1) {
        //    grid.DeleteRow(e.visibleIndex);
        //}
        if (grid.GetVisibleRowsOnPage() > 1) {
            grid.batchEditApi.StartEdit(e.visibleIndex, 1);
            var ReceiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0.0";
            var PaymentValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0.0";
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            if (noofvisiblerows != "1") {
                grid.DeleteRow(e.visibleIndex);
                $('#hdfIsDelete').val('D');
                grid.UpdateEdit();
                grid.PerformCallback('Display');
                $('#hdnPageStatus').val('delete');
            }

            if (ReceiptValue != "0.00") {
                var VoucherAmount = ctxtVoucherAmount.GetValue();
                ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                c_txt_Debit.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
            }
            if (PaymentValue != "0.00") {
                var VoucherAmount = ctxtVoucherAmount.GetValue();
                ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue));
                ctxtTotalPayment.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue));
            }
        }
        SetTDSAmount();
    }
    if (e.buttonID == 'AddNew') {
        var TypeValue = (grid.GetEditor('Type').GetText() != null) ? grid.GetEditor('Type').GetText() : "0";
        if (TypeValue != "") {
            //if (TypeValue == "Advance")
            //{
            //    var PaymentValue = (grid.GetEditor('Payment').GetText() != null) ? grid.GetEditor('Payment').GetText() : "0";
            //    ctxt_proamt.SetValue(PaymentValue);
            //    cinventorypopup.Show();
            //}
            //else
            //{
            //    OnAddNewClick();
            //}
            OnAddNewClick();

        }
        else {
            grid.batchEditApi.StartEdit(e.visibleIndex, 1);
        }
    }
}


function ComponentPanel_EndCallBack(s, e) {

    var hfValue = $("#hfHSN_CODE").val();
    if (cproductLookUp.cpScheme != null) {

        jAlert("Please Select Numbering Scheme.", 'Alert Dialog: [VendorPayment/Receipt]', function (r) {
            if (r == true) {
                cproductLookUp.cpScheme = null
                cCmbScheme.ShowDropDown();
            }
        });

    }
    else {
        if (hfValue == '') {
            cproductLookUp.ShowDropDown();
            jAlert("Please make sure all Product(s) HSN/SAC are same.");
        }
        else {
            cproductLookUp.ConfirmCurrentSelection();
            cproductLookUp.HideDropDown();
            cproductLookUp.Focus();
        }
    }


}
//.................Product LookUp.....................
function CloseProductLookup() {
    cproductLookUp.ConfirmCurrentSelection();
    cproductLookUp.HideDropDown();
    cproductLookUp.Focus();
}
function ProductSelected() {
   
    var VoucherType = document.getElementById("ComboVoucherType").value;
    var Products = cproductLookUp.GetValue();
    if (Products != null) {

        // Type.PerformCallback(VoucherType + "~" + "Yes");
        cComponentPanel.PerformCallback();
        RemoveTypeByReceiptPayment();
        // PropolateTypeByReceiptProduct();
    }
    else {
        Type.PerformCallback(VoucherType + "~" + "");
    }

}
function PropolateTypeByReceipt() {
    
    Type.AddItem("Debit Note", "VenDbNote");
    Type.AddItem("Advance Payment", "AdvanceReceipt");
    Type.AddItem("On Account", "OnAccountRec");
}
function RemoveTypeByReceipt() {
   
    var comboitem = Type.FindItemByValue('Advance');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('Invoice');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('VenCrNote');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('OnAccount');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('Ledger');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
}
function RemoveTypeByPayment() {
  
    var comboitem = Type.FindItemByValue('VenDbNote');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('AdvanceReceipt');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('OnAccountRec');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }

}
function PropolateTypeByPayment() {
   
    Type.AddItem("Advance", "Advance");
    Type.AddItem("Invoice", "Invoice");
    Type.AddItem("Credit Note", "VenCrNote");
    Type.AddItem("On Account", "OnAccount");
    Type.AddItem("Ledger", "Ledger");
}
function PropolateTypeByPaymentProduct() {
    Type.AddItem("Advance", "Advance");

}
function RemoveTypeByReceiptPayment() {

    var comboitem = Type.FindItemByValue('Invoice');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('VenDbNote');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('VenCrNote');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('AdvanceReceipt');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('Ledger');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }

}
//.................Product LookUp  End.....................
function VisibleColumn() {
    var VoucherType = $("#ComboVoucherType").val();

    if (VoucherType == "P") {
        grid.GetEditor('Receipt').SetEnabled(false);
        grid.GetEditor('Payment').SetEnabled(true);
    }
    else {
        grid.GetEditor('Payment').SetEnabled(false);
        grid.GetEditor('Receipt').SetEnabled(true);
    }
}
function OnAddNewClick() {
    grid.AddNewRow();

}
function GridCallBack() {
    var urlKeys = getUrlVars();
    if (urlKeys.key != 'ADD') {
        if ($("#CB_GSTApplicable").is(':checked')) {
            cproductLookUp.SetEnabled(true);
        }
        else {
            cproductLookUp.SetEnabled(false);
            return;
        }
    }
    else {
        cproductLookUp.SetEnabled(false);
    }
    grid.PerformCallback('Display');
}
//...............End Batch Grid..................
function RemoveTypeAll() {
    var comboitem = Type.FindItemByValue('Advance');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('Invoice');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('VenCrNote');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('VenDbNote');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('AdvanceReceipt');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('OnAccount');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
    var comboitem = Type.FindItemByValue('Ledger');
    if (comboitem != undefined && comboitem != null) {
        Type.RemoveItem(comboitem.index);
    }
}
function PropolateTypeByNonAdvance() {

    Type.AddItem("Invoice", "Invoice");
    Type.AddItem("Credit Note", "VenCrNote");
    Type.AddItem("On Account", "OnAccount");
    Type.AddItem("Ledger", "Ledger");
}
function PropolateTypeByOnAccount() {
    Type.AddItem("On Account", "OnAccount");
}
function CmbScheme_ValueChange() {

    var IsType = "";
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Type')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Type')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (IsType == "Y") {
        deleteAllRows();
    } 

    var schemetypeValue = cCmbScheme.GetValue();
    var schemeID;
    var schemetype;
    var schemelength;
    var branchID;
    var Type;
    if (schemetypeValue != "" && schemetypeValue != null) {
        schemeID = schemetypeValue.toString().split('~')[0];
        schemetype = schemetypeValue.toString().split('~')[1];
        schemelength = schemetypeValue.toString().split('~')[2];
        branchID = schemetypeValue.toString().split('~')[3];
        Type = schemetypeValue.toString().split('~')[4];
        var fromdate = schemetypeValue.toString().split('~')[5];
        var todate = schemetypeValue.toString().split('~')[6];

        var dt = new Date();
        cdtTDate.SetDate(dt);
        if (dt < new Date(fromdate)) {
            cdtTDate.SetDate(new Date(fromdate));
        }
        if (dt > new Date(todate)) {
            cdtTDate.SetDate(new Date(todate));
        }
        cdtTDate.SetMinDate(new Date(fromdate));
        cdtTDate.SetMaxDate(new Date(todate));

        var enableUnit = document.getElementById('hdnEnableUnit').value;
        if (enableUnit == "No") {
            document.getElementById('ddlBranch').disabled = true;
        }
        else {
            document.getElementById('ddlBranch').disabled = false;
        }
        document.getElementById('ddlBranch').value = branchID;
        document.getElementById('ddlEnterBranch').value = branchID;
        $('#txtVoucherNo').attr('maxLength', schemelength);
        LoadCustomerAddress("", $('#ddlBranch').val(), 'VPR');
        //cddlCashBank.PerformCallback(branchID);
        $('#ddlBranch').attr('disabled', 'disabled');
        var OtherDetails = {}
        OtherDetails.branchID = branchID;
        $.ajax({
            type: "POST",
            url: "VendorPaymentReceipt.aspx/GetCashBank",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var returnObject = msg.d;
                if (returnObject) {
                    SetDataSourceOnComboBox(cddlCashBank, returnObject);
                }
            }
        });
    }
    var VoucherType = $("#ComboVoucherType").val();
    if (VoucherType == "P") {
        if (Type == "Advance") {
            RemoveTypeAll();
            PropolateTypeByPaymentProduct();
        }
        else if (Type == "None" || Type == "") {
            RemoveTypeAll();
            PropolateTypeByPayment();
        }
        else if (Type == "NonAdvance") {
            RemoveTypeAll();
            PropolateTypeByNonAdvance();
        }
        else if (Type == "OnAccount") {
            RemoveTypeAll();
            PropolateTypeByOnAccount();
        }

    }
    if (schemetype == '0') {
        $('#hdnSchemaType').val('0');
        document.getElementById('txtVoucherNo').disabled = false;
        document.getElementById('txtVoucherNo').value = "";
        document.getElementById('txtVoucherNo').focus();
    }
    else if (schemetype == '1') {
        $('#hdnSchemaType').val('1');
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "Auto";
        $("#MandatoryBillNo").hide();
        cdtTDate.Focus();
    }
    else if (schemetype == '2') {
        $('#hdnSchemaType').val('2');
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "Datewise";
    }
    else if (schemetype == 'n') {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "";
    }
    if ($("#hdnProjectSelectInEntryModule").val()=="1")
     clookup_Project.gridView.Refresh();
    
}
function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}
function deleteAllRows() {
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
        grid.DeleteRow(frontRow);
        grid.DeleteRow(backRow);
        backRow--;
        frontRow++;
    }
    grid.AddNewRow();

    ctxtTotalPayment.SetValue(0);
    c_txt_Debit.SetValue(0);

}
function CmbScheme_EndCallBack() {
    if (lastCRP) {
        cCmbScheme.PerformCallback(lastCRP);
        lastCRP = null;
    }
}
function rbtnType_SelectedIndexChanged() {
  
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Type')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Type')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (IsType == "Y") {
        deleteAllRows();
    }
    document.getElementById('txtVoucherNo').value = "";
    var VoucherType = $("#ComboVoucherType").val();
    if (cCmbScheme.InCallback()) {
        lastCRP = VoucherType;
    }
    else {
        cCmbScheme.PerformCallback(VoucherType);
    }


    if (VoucherType == "P") {
        grid.GetEditor('Receipt').SetEnabled(false);
        grid.GetEditor('Payment').SetEnabled(true);
        $('#ProductSection').show();
        $("#CB_GSTApplicable").prop("checked", true);
        $('#ProductGSTApplicableSection').show();
        $('#tdsSection').show();
        PropolateTypeByPayment();
        RemoveTypeByPayment();

    }
    else {
        PropolateTypeByReceipt();
        RemoveTypeByReceipt();
        grid.GetEditor('Payment').SetEnabled(false);
        grid.GetEditor('Receipt').SetEnabled(true);
        $('#ProductSection').hide();
        $('#tdsSection').hide();
        $('#ProductGSTApplicableSection').hide();
        $("#CB_GSTApplicable").prop("checked", false);
    }
}
//............Rate.........................
function InstrumentTypeSelectedIndexChanged() {
    $("#MandatoryInstrumentType").hide();
    var InstType = cComboInstrumentTypee.GetValue();

    if (InstType == "CH") {
        $('#hdnInstrumentType').val(0);
        document.getElementById("divInstrumentNo").style.display = 'none';
        document.getElementById("tdIDateDiv").style.display = 'none';
    }
    else {
        $('#hdnInstrumentType').val(InstType);
        document.getElementById("divInstrumentNo").style.display = 'block';
        document.getElementById("tdIDateDiv").style.display = 'block';
    }
}
//...............Customer LookUp.....
function CloseGridLookup() {
    //gridLookup.ConfirmCurrentSelection();
    //gridLookup.HideDropDown();
    //gridLookup.Focus();
    gridquotationLookup.SetEnabled(true);
}
function deleteAllRows() {
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
        grid.DeleteRow(frontRow);
        grid.DeleteRow(backRow);
        backRow--;
        frontRow++;
    }
    grid.AddNewRow();

    ctxtTotalPayment.SetValue(0);
    c_txt_Debit.SetValue(0);
    ctxtVoucherAmount.SetValue(0);
}
function GetContactPerson(e) {
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Type')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Type')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (IsType == "Y") {
        deleteAllRows();
    }
    $('#MandatorysCustomer').hide();
    var customerval = GetObjectID('hdnCustomerId').value;// gridLookup.GetValue();
    $('#hdfLookupCustomer').val(customerval);
    var key = GetObjectID('hdnCustomerId').value;// gridLookup.GetValue();
    if (key != null && key != '') {
        var startDate = new Date();
    }
    GetObjectID('hdnCustomerId').value = key;
    cContactPerson.Focus();

}
//...............End............

    var setValueFlag = "";
var lastDocumentType = "";
var currentEditableVisibleIndex = "";

function OnInit(s, e) {
    IntializeGlobalVariables(s);
}
function OnBatchEditStartEditing(s, e) {
    currentEditableVisibleIndex = e.visibleIndex;
    var currentDocumentType = grid.batchEditApi.GetCellValue(currentEditableVisibleIndex, "Type");
    var DocumentIDColumn = s.GetColumnByField("DocumentID");
    if (!e.rowValues.hasOwnProperty(DocumentIDColumn.index))
        return;
    var cellInfo = e.rowValues[DocumentIDColumn.index];

    if (lastDocumentType == currentDocumentType) {
        if (DocumentID.FindItemByValue(cellInfo.value) != null) {
            DocumentID.SetValue(cellInfo.value);
        }
        else {
            RefreshData(cellInfo, lastDocumentType);
            LoadingPanel.Show();
        }
    }
    else {
        if (currentDocumentType == null) {
            DocumentID.SetSelectedIndex(-1);
            return;
        }
        lastDocumentType = currentDocumentType;
        RefreshData(cellInfo, lastDocumentType);
        LoadingPanel.Show();
    }
}
function OnBatchEditEndEditing(s, e) {
    currentEditableVisibleIndex = -1;
    var DocumentIDColumn = s.GetColumnByField("DocumentID");
    var DocumentTypeColumn = s.GetColumnByField("Type");

    if (!e.rowValues.hasOwnProperty(DocumentIDColumn.index))
        return;

    var cellInfo = e.rowValues[DocumentIDColumn.index];
    if (DocumentID.GetSelectedIndex() > -1 || cellInfo.text != DocumentID.GetText()) {
        cellInfo.value = DocumentID.GetValue();
        cellInfo.text = DocumentID.GetText();
        DocumentID.SetValue(null);
    }

    var cellTypeInfo = e.rowValues[DocumentTypeColumn.index];
}
function RefreshData(cellInfo, DocumentType) {
    setValueFlag = cellInfo.value;
    DocumentID.PerformCallback(DocumentType);
}
function IntializeGlobalVariables(grid) {
    lastDocumentType = grid.cplastDocumentType;
    currentEditableVisibleIndex = -1;
    setValueFlag = -1;
}
function Type_EndCallback() {
          
    cCmbScheme.Focus();
}
function DocumentType_SelectedIndexChanged(s, e) {
         
    var currentValue = s.GetValue();
    var DocumentID = (grid.GetEditor('DocumentID').GetValue() != null) ? parseFloat(grid.GetEditor('DocumentID').GetValue()) : "0";
    var type = grid.GetEditor("Type").GetValue();

    if (ctdsSection.GetValue() != "0" && ctdsSection.GetValue() != "Select" && ctdsSection.GetValue() != null && $("#ComboVoucherType").val() == "P") {
        if (type != "Advance" && type != "OnAccount") {
            jAlert('You can not select ' + type + ' for TDS entry', 'Alert', function () {
                grid.GetEditor("Type").SetValue("Advance");
            })
            grid.GetEditor("Type").SetValue("Advance");

        }
    }


    lastDocumentType = currentValue;

    if (type != "Advance") {
        $("#CB_GSTApplicable").prop("checked", false);
        $('#ProductGSTApplicableSection').attr('style', 'display:none');
        $('#ProductSection').hide();
    }
    else {

       // $("#CB_GSTApplicable").prop("checked", true);
        $('#ProductGSTApplicableSection').attr('style', 'display:block;margin-top: 29px;');
        $('#ProductSection').show();
    }
    if (grid.GetEditor("Receipt").GetValue() != "0.00" && grid.GetEditor("Receipt").GetValue() != "0.0") {

        var VoucherAmount = ctxtVoucherAmount.GetValue();
        var ReceiptAmt = grid.GetEditor("Receipt").GetValue();

        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        if (noofvisiblerows == "1") {
            c_txt_Debit.SetValue("0.00");

        }
        else {
            c_txt_Debit.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptAmt));
        }

        grid.GetEditor("Receipt").SetValue("0.0");
        grid.GetEditor("DocumentNo").SetValue("");
        grid.GetEditor("DocumentID").SetValue("");
        grid.GetEditor("IsOpening").SetValue("");

    }
    if (grid.GetEditor("Payment").GetValue() != "0.0" && grid.GetEditor("Payment").GetValue() != "0.00") {
        var VoucherAmount = ctxtVoucherAmount.GetValue();
        var PaymentAmt = grid.GetEditor("Payment").GetValue();

        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        if (noofvisiblerows == "1") {
            ctxtTotalPayment.SetValue("0.00");

        }
        else {
            ctxtTotalPayment.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentAmt));

        }

        grid.GetEditor("IsOpening").SetValue("");
        grid.GetEditor("Payment").SetValue("0.0");
        grid.GetEditor("DocumentNo").SetValue("");
        grid.GetEditor("DocumentID").SetValue("");
    }

            
    var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
    if ((Type == "Advance") || (Type == "OnAccount") || (Type == "Ledger") || (Type == "OnAccountRec"))
    {
        if ($("#hdnProjectSelectInEntryModule").val() == "1") {
            if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {
                //cCallbackPanelDocumentNo.PerformCallback('Type~' + Type + "~" + clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
                var otherdet = {};
                var InProjId = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
                otherdet.InProjId = InProjId;
                otherdet.Type = Type;

                $.ajax({
                    type: "POST",
                    url: "VendorPaymentReceipt.aspx/SetNonDocProjectCode",
                    data: JSON.stringify(otherdet),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        var Code = msg.d;
                          
                        clookupPopup_ProjectCode.gridView.SelectItemsByKey(Code.split('~')[0]);
                        grid.GetEditor("ProjectId").SetText(Code.split('~')[0]);
                        grid.GetEditor("Project_Code").SetText(Code.split('~')[1]);

                    }
                });

            }
              
        }
          
    }
    //clookup_Project.SetEnabled(false);
}
function DocumentsCombo_EndCallback(s, e) {
    if (setValueFlag == null || setValueFlag == "0" || setValueFlag == "") {
        s.SetSelectedIndex(-1);
    }
    else {
        if (DocumentID.FindItemByValue(setValueFlag) != null) {
            DocumentID.SetValue(setValueFlag);
            setValueFlag = null;
        }
    }

    LoadingPanel.Hide();
}

function tdsSectionSelectionChange() {

    var TDSsection = ctdsSection.GetValue();

    var OtherDetails = {};
    OtherDetails.InfluencerId = GetObjectID('hdnCustomerId').value;
    OtherDetails.TDSSection = TDSsection;
    OtherDetails.TDSDate = cdtTDate.GetText();


    if (TDSsection != "0" && TDSsection != null) {
        $.ajax({
            type: "POST",
            url: "../Activities/Services/InfluencerPayment.asmx/GetInflencerTDSRate",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                $("#hdnTDSRate").val(msg.d);

            },
            error: function (msg) {
                alert(msg);
            }
        });
    }
    else {
        $("#hdnTDSRate").val("0.00");
    }
    SetTDSAmount();
}

function SetTDSAmount() {

    var totVouAmount = ctxtVoucherAmount.GetValue();
    var TDSTotAmt = (totVouAmount * parseFloat($("#hdnTDSRate").val().split('~')[0])) / 100;


    var ro = $("#hdnTDSRate").val().split('~')[1];

    if (ro == "1") {
        TDSTotAmt = round1(TDSTotAmt);
    }
    else if (ro == "2") {
        TDSTotAmt = round5(TDSTotAmt);
    }
    else if (ro == "3") {
        TDSTotAmt = round10(TDSTotAmt);
    }

    if ($("#chkNILRateTDS").is(':checked')) {
        
    }
    else {
        ctxtTdsAmount.SetValue(TDSTotAmt);
    }
}

function round5(x) {
    return x % 5 < 2.5 ? (x % 5 === 0 ? x : Math.floor(x / 5) * 5) : Math.ceil(x / 5) * 5
}

function round10(x) {
    return x % 10 < 5 ? (x % 10 === 0 ? x : Math.floor(x / 10) * 10) : Math.ceil(x / 10) * 10
}

function round1(x) {
    return x % 1 < .5 ? (x % 1 === 0 ? x : Math.floor(x / 1) * 1) : Math.ceil(x / 1) * 1
}


//<%--Batch Product Popup Start--%>

    function ProductKeyDown(s, e) {

        console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Enter") {

            s.OnButtonClick(0);
        }
        if (e.htmlEvent.key == "Tab") {

            s.OnButtonClick(0);
        }
        cdtTDate.SetEnabled(false);
    }



function ProjectCodeKeyDown(s,e)
{

}

function MainAccountNewkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtMainAccountSearch").val();
    OtherDetails.branchId = $("#ddlBranch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtMainAccountSearch").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Main Account Name");
        HeaderCaption.push("Subledger Type");
        HeaderCaption.push("Reverse Applicable");
        HeaderCaption.push("HSN/SAC");

        //callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBank", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
        callonServer("/OMS/Management/Activities/Services/Master.asmx/GetledgerAccountVendorPaymentReceipt", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");


    }
    else if (e.code == "ArrowDown") {
        if ($("input[MainAccountIndex=0]"))
            $("input[MainAccountIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //  
        $('#MainAccountModel').modal('hide');
        grid.batchEditApi.StartEdit(globalRowIndex, 1);
        var totamnt = GetTotalAmount();

        ctxtVoucherAmount.SetValue(totamnt);

    }
}
function SetMainAccount(Id, name, e) {

    $('#MainAccountModel').modal('hide');
    var IsSub = e.parentElement.cells[2].innerText;
    var RevApp = e.parentElement.cells[3].innerText;
    if (RevApp == 'Yes') {
        RevApp = '1';
    }
    else {
        RevApp = '0';
    }
    var TaxAble = e.parentElement.cells[4].innerText;
    GetMainAcountComboBox(Id, name, IsSub, RevApp, TaxAble);
    grid.batchEditApi.StartEdit(globalRowIndex, 2);

    var totamnt = GetTotalAmount();

    ctxtVoucherAmount.SetValue(totamnt);

}
function GetMainAcountComboBox(Id, name, IsSub, RevApp, TaxAble) {
    var MainAccountText = name;
    // IsSubAccount = IsSub;          
    var MainAccountID = Id;
    var ReverseApplicable = RevApp;
    var TaxApplicable = TaxAble;

    grid.batchEditApi.StartEdit(globalRowIndex, 3);
    grid.GetEditor("DocumentNo").SetValue(MainAccountText);
    grid.GetEditor("DocumentID").SetValue(MainAccountID);
    grid.GetEditor("Receipt").SetValue("0.0");
    grid.GetEditor("Payment").SetValue("0.0");
    grid.GetEditor("IsOpening").SetValue("Ledger");

}

function DocumentButnClick(s, e) {
          
    if (e.buttonIndex == 0) {
        var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";

        if (Type == 'Ledger') {
            var txt = "<table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\" ><th>Main Account Name</th><th>Subledger Type</th><th>Reverse Applicable</th><th>HSN/SAC</th></tr><table>";
            document.getElementById("MainAccountTable").innerHTML = txt;
            $('#MainAccountModel').modal('show');

        }
        else if (Type == 'OnAccount' || Type == 'OnAccountRec') {
            cDocumentpopUp.Hide();
        }
        else if (Type != 'Advance' && Type != '0') {
            if (cdocumentLookUp.Clear()) {
                cDocumentpopUp.Show();
                cdocumentLookUp.Focus();

            }
                    
            //cCallbackPanelDocumentNo.PerformCallback('Type~' + Type);
            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {
                    cCallbackPanelDocumentNo.PerformCallback('Type~' + Type + "~" + clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
                }
                else
                {
                    cCallbackPanelDocumentNo.PerformCallback('Type~' + Type);
                }
                       
            }
            else
            {
                cCallbackPanelDocumentNo.PerformCallback('Type~' + Type);
            }
        }
        else {
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }
    }
}
        
//chinmoy added for inline project code 10-12-2019
function ProjectCodeButnClick(s,e)
{
    if (e.buttonIndex == 0) {
        if ($("#hdnAllowProjectInDetailsLevel").val() != "0") {
            clookupPopup_ProjectCode.Clear();
            var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            var InvoiceNo = (grid.GetEditor('DocumentNo').GetValue() != null) ? grid.GetEditor('DocumentNo').GetValue() : "0";
            if (clookupPopup_ProjectCode.Clear()) {
                cProjectCodePopup.Show();
                clookupPopup_ProjectCode.Focus();
            }

            var id = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
            if (((Type == "Advance") || (Type == "OnAccount") || (Type == "Ledger") || (Type == "OnAccountRec")) && (id != null)) {


                cProjectCodeCallback.PerformCallback('Type~' + Type + "~" + id);
            }
            else {
                cProjectCodeCallback.PerformCallback('Type~' + Type + "~" + InvoiceNo);
            }
        }
    }
}
//End


function closeModal() {
    $('#MainAccountModel').modal('hide');
    grid.batchEditApi.StartEdit(globalRowIndex, 2);

    /*  Arindam*/
    var totamnt = GetTotalAmount();

    ctxtVoucherAmount.SetValue(totamnt);
}

function DocumentlookUpKeyDown(s, e) {
           
    if (e.htmlEvent.key == "Escape") {
        cDocumentpopUp.Hide();
        var vouchertype = $("#ComboVoucherType").val(); //cComboVoucherType.GetValue();
        if (vouchertype == 'P') {
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }
        else {
            grid.batchEditApi.StartEdit(globalRowIndex, 4);
        }
    }
}


function lookup_ProjectCodeKeyDown(s,e)
{
    if (e.htmlEvent.key == "Escape") {
        cProjectCodePopup.Hide();
        var vouchertype = $("#ComboVoucherType").val(); //cComboVoucherType.GetValue();
        if (vouchertype == 'P') {
            //grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }
        else {
            // grid.batchEditApi.StartEdit(globalRowIndex, 4);
        }
    }
}


function DocumentSelected(s, e) {
           
    if (cdocumentLookUp.GetGridView().GetFocusedRowIndex() == -1) {
        cDocumentpopUp.Hide();
        var vouchertype = $("#ComboVoucherType").val(); //cComboVoucherType.GetValue();
        if (vouchertype == 'P') {
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }
        else {
            grid.batchEditApi.StartEdit(globalRowIndex, 4);
        }
        return;
    }
    var LookUpData = cdocumentLookUp.GetGridView().GetRowKey(cdocumentLookUp.GetGridView().GetFocusedRowIndex());
    var DocumentID = LookUpData.split('~')[0];
    var unpaidamount = LookUpData.split('~')[1];
    var IsOpening = LookUpData.split('~')[2];
    var ProductCode = cdocumentLookUp.GetValue();
    if (!ProductCode) {
        LookUpData = null;
    }
    cDocumentpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);

    var ReceiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0";
    var PaymentValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0";
    grid.GetEditor("DocumentID").SetText(DocumentID);

    grid.GetEditor("IsOpening").SetText(IsOpening);
    // var vouchertype = cComboVoucherType.GetValue();
    var vouchertype = $("#ComboVoucherType").val(); //cComboVoucherType.GetValue();
    if (vouchertype == 'P') {
        grid.GetEditor("Payment").SetText(unpaidamount);
        var VoucherAmount = ctxtVoucherAmount.GetValue();
        //ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue) + parseFloat(unpaidamount));
        var Payment = ctxtTotalPayment.GetValue();
        //ctxtTotalPayment.SetValue(parseFloat(Payment) + parseFloat(unpaidamount));
        //ctxtVoucherAmount.SetValue(parseFloat(Payment) + parseFloat(unpaidamount));
        /*----------------- If the Ledger is changed the value against it becomes zero but the field "Voucher Amount"
at the header level does not change. If I again provide the amount this field gets updated - as a result the amount becomes wrong  Arindam 05-02-2019 */
        ctxtTotalPayment.SetValue((parseFloat(Payment) - parseFloat(PaymentValue)) + parseFloat(unpaidamount));
        ctxtVoucherAmount.SetValue((parseFloat(Payment) - parseFloat(PaymentValue)) + parseFloat(unpaidamount));
    }
    else {
        grid.GetEditor("Receipt").SetText(unpaidamount);
        var VoucherAmount = ctxtVoucherAmount.GetValue();
        // ctxtVoucherAmount.SetValue(parseFloat(unpaidamount) + parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
        var Receipt = c_txt_Debit.GetValue();
        c_txt_Debit.SetValue(parseFloat(Receipt) + parseFloat(unpaidamount));
    }

    //var VoucherAmount = ctxtVoucherAmount.GetValue();

    //ctxtVoucherAmount.SetValue(parseFloat(unpaidamount) + parseFloat(VoucherAmount));


    if (LookUpData != null) {
        $('#hdndocumentno').val(DocumentID + ',');
    }
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        if (DocumentID != "" || DocumentID != null) {
            var otherdet = {};
            var type = "";
            if (grid.GetEditor('Type').GetValue() == "Invoice") {
                type = "Purchase_Invoice";
            }
            else if (grid.GetEditor('Type').GetValue() == "VenCrNote") {
                type = "CN Vendor";
            }
            else if (grid.GetEditor('Type').GetValue() == "VenDbNote") {
                type = "'DN Vendor','Purchase_Return','Purchase_Return_Manual'";
            }
            else if (grid.GetEditor('Type').GetValue() == "AdvanceReceipt") {
                type = "VendorPayRec";
            }

            var Id = DocumentID
            otherdet.Id = DocumentID;
            otherdet.type = type;

            $.ajax({
                type: "POST",
                url: "VendorPaymentReceipt.aspx/SetProjectCode",
                data: JSON.stringify(otherdet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var Code = msg.d;
                           
                    clookupPopup_ProjectCode.gridView.SelectItemsByKey(Code.split('~')[0]);
                    grid.GetEditor("ProjectId").SetText(Code.split('~')[0]);
                    grid.GetEditor("Project_Code").SetText(Code.split('~')[1]);

                }
            });
        }

    }
    grid.GetEditor("DocumentNo").SetText(ProductCode);
    var vouchertype = $("#ComboVoucherType").val(); //cComboVoucherType.GetValue();
    if (vouchertype == 'P') {
        // grid.GetEditor("Payment").Focus();
        grid.batchEditApi.EndEdit();
        // grid.batchEditApi.StartEdit(globalRowIndex)
        grid.batchEditApi.StartEdit(globalRowIndex, 5);

    }
    else {
        //grid.GetEditor("Payment").Focus();
        grid.batchEditApi.StartEdit(globalRowIndex, 4);
    }
    cdtTDate.SetEnabled(false);
    // return;
}



function ProjectCodeSelected(s,e)
{
          
    if (clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex() == -1) {
        cProjectCodePopup.Hide();
        var vouchertype = $("#ComboVoucherType").val(); //cComboVoucherType.GetValue();
        if (vouchertype == 'P') {
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }
        else {
            grid.batchEditApi.StartEdit(globalRowIndex, 4);
        }
        return;
    }
    var ProjectInlineLookUpData = clookupPopup_ProjectCode.GetGridView().GetRowKey(clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex());
    var ProjectInlinedata = ProjectInlineLookUpData.split('~')[0];
    grid.batchEditApi.StartEdit(globalRowIndex);
    var ProjectCode = clookupPopup_ProjectCode.GetValue();
    cProjectCodePopup.Hide();
           
    grid.GetEditor("Project_Code").SetText(ProjectCode);
    grid.GetEditor("ProjectId").SetText(ProjectInlinedata);

}



function CallbackPanelDocumentNo_endcallback() {
    cdocumentLookUp.ShowDropDown();
    cdocumentLookUp.Focus();
}

function ProjectCodeCallback_endcallback()
{
     
    clookupPopup_ProjectCode.ShowDropDown();;
    clookupPopup_ProjectCode.Focus();
    clookupPopup_ProjectCode.Clear()

}


function TDS_SelectedIndexChanged(s, e) {

}

//<%--Batch Product Popup End--%>
//<%--Added By : Samrat Roy -- New Billing/Shipping Section--%>

    function disp_prompt(name) {

        if (name == "tab0") {
            //gridLookup.Focus();
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
            }
        }
    }
//<%--Added By : Samrat Roy -- New Billing/Shipping Section End--%>
//<%--Tax Script Start--%>
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
function Save_TaxesClick() {
    //TaxAmountKeyDown();
    taxAmtButnClick();
    taxAmtButnClick1();
}
function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}
function taxAmtButnClick(s, e) {

    var amountAre = "2"; // To set value Inclusive by default

    if (amountAre != null) {
        var ProductID = $("#hfHSN_CODE").val();

        if (ProductID.trim() != "") {
            globalNetAmount = parseFloat(ctxtTotalPayment.GetValue());
            document.getElementById('HdSerialNo').value = 0;
            ctxtTaxTotAmt.SetValue(0);
            ccmbGstCstVat.SetSelectedIndex(0);
            $('.RecalculateInline').hide();
            if ($("#CB_GSTApplicable").is(':checked')) {
                caspxTaxpopUp.Show();
            }

            var Amount = Math.round(globalNetAmount).toFixed(2);
            clblTaxProdGrossAmt.SetText(Amount);
            clblProdNetAmt.SetText(Amount);
            document.getElementById('HdProdGrossAmt').value = Amount;
            document.getElementById('HdProdNetAmt').value = Amount;
            clblTaxDiscount.SetText('0.00');


            //Checking is gstcstvat will be hidden or not
            if (amountAre == "2") {
                $('.GstCstvatClass').hide();
                $('.gstGrossAmount').show();
                clblTaxableGross.SetText("(Taxable)");
                clblTaxableNet.SetText("(Taxable)");
                $('.gstNetAmount').show();
                //   var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
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
            else if (amountAre == "1") {
                $('.GstCstvatClass').show();
                $('.gstGrossAmount').hide();
                $('.gstNetAmount').hide();
                clblTaxableGross.SetText("");
                clblTaxableNet.SetText("");

                //Get Customer Shipping StateCode
                var shippingStCode = '';
                if (cchkBilling.GetValue()) {
                    shippingStCode = CmbState.GetText();
                }
                else {
                    shippingStCode = CmbState1.GetText();
                }
                shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

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
                cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + amountAre);
            } else {

                cgridTax.PerformCallback('New~' + amountAre);
                //Set default combo
                //cgridTax.cpComboCode = grid.GetEditor('ProductID').GetValue().split('||@||')[9];

            }

            //ctxtprodBasicAmt.SetValue(grid.GetEditor('Amount').GetValue());
            ctxtprodBasicAmt.SetValue(Amount);
        } else {
            grid.batchEditApi.StartEdit(globalRowIndex, 13);
        }
    }
    // }
}
function taxAmtButnClick1(s, e) {
    console.log(grid.GetFocusedRowIndex());
    rowEditCtrl = s;
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

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

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
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
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
function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}
function cgridTax_EndCallBack(s, e) {
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
            if (amountAre == "1") {
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
        //grid.GetEditor("TaxAmount").SetValue(totAmt);
        //grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));

        //var finalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
        //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
        //cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
        //SetInvoiceLebelValue();

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
function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
}
function BatchUpdate() {

    //cgridTax.batchEditApi.StartEdit(0, 1);

    //if (cgridTax.GetEditor("TaxField").GetText().indexOf('.') == -1) {
    //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '.00');
    //} else {
    //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '0');
    //}
    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        cgridTax.UpdateEdit();
    }
    else {
        cgridTax.PerformCallback('SaveGST');
    }
    return false;
}
function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
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
    var roundedOfAmount = Math.round(totalInlineTaxAmount);
    ctxtTaxTotAmt.SetValue(Math.round(totalInlineTaxAmount));


    var diffDisc = roundedOfAmount - totalInlineTaxAmount;
    if (diffDisc > 0)
        document.getElementById('taxroundedOf').innerText = 'Adjustment ' + Math.abs(diffDisc.toFixed(3));
    else if (diffDisc < 0)
        document.getElementById('taxroundedOf').innerText = 'Adjustment ' + Math.abs(diffDisc.toFixed(3));
    else
        document.getElementById('taxroundedOf').innerText = '';
}


function GetTotalAmount() {
           
    var totalAmount = 0;
    var receiptamnt = 0;
    var paymentamnt = 0;

    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = DecimalRoundoff(totalAmount, 2) + DecimalRoundoff(grid.GetEditor("Payment").GetText(), 2);
                totalAmount = DecimalRoundoff(totalAmount, 2) - DecimalRoundoff(grid.GetEditor("Receipt").GetText(), 2);
                paymentamnt = DecimalRoundoff(paymentamnt, 2) + DecimalRoundoff(grid.GetEditor("Payment").GetText(), 2);
                receiptamnt = DecimalRoundoff(receiptamnt, 2) + DecimalRoundoff(grid.GetEditor("Receipt").GetText(), 2);
            }
        }
    }


    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = DecimalRoundoff(totalAmount, 2) + DecimalRoundoff(grid.GetEditor("Payment").GetText(), 2);
                totalAmount = DecimalRoundoff(totalAmount, 2) - DecimalRoundoff(grid.GetEditor("Receipt").GetText(), 2);
                paymentamnt = DecimalRoundoff(paymentamnt, 2) + DecimalRoundoff(grid.GetEditor("Payment").GetText(), 2);
                receiptamnt = DecimalRoundoff(receiptamnt, 2) + DecimalRoundoff(grid.GetEditor("Receipt").GetText(), 2);
            }
        }
    }

    ctxtTotalPayment.SetValue(DecimalRoundoff(paymentamnt, 2));
    c_txt_Debit.SetValue(DecimalRoundoff(receiptamnt, 2));
    return totalAmount;
}
//<%--Tax Script End--%>
//<%-- Project Script start --%>
function ProjectListKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}
function ProjectListButnClick(s, e) {
    //ctaggingGrid.PerformCallback('BindComponentGrid');
    clookup_Project.ShowDropDown();
}
//<%-- Project Script End --%>
    function UniqueCodeCheck() {
        var OrderNo = document.getElementById('txtVoucherNo').value
        var SchemeVal = cCmbScheme.GetValue();
        if (SchemeVal == null) {
            alert('Please Select Numbering Scheme');
            document.getElementById('txtVoucherNo').value = '';
            cCmbScheme.Focus();
        }
           
        else {
                
            var CheckUniqueCode = false;
            $.ajax({
                type: "POST",
                url: "VendorPaymentReceipt.aspx/CheckUniqueCode",
                data: JSON.stringify({ OrderNo: OrderNo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCode = msg.d;
                    if (CheckUniqueCode == true) {
                        alert('Please provide an unique number to proceed');

                        document.getElementById('txtVoucherNo').value = '';
                        $('#txtVoucherNo').focus();
                    }
                    else {
                        $('#MandatoryBillNo').attr('style', 'display:none');
                    }
                }

            });
        }

    }
//Hierarchy Start Tanmoy
function clookup_Project_LostFocus() {
  
    var gridVal = "";
    if (grid.GetVisibleRowsOnPage() > 0) {
        if ($("#Keyval_internalId").val() == "Add") {
            grid.batchEditApi.StartEdit(0);
            gridVal = grid.GetEditor("Type").GetValue();
            grid.batchEditApi.EndEdit();
        }
        else
        {
            grid.batchEditApi.StartEdit(0);
            gridVal = grid.GetEditor("Type").GetValue();
            grid.batchEditApi.EndEdit();
        }
    }
    ///&& Gotprojid != ""
    var lostProjectId = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
    if (lostProjectId == null)
    {
        lostProjectId = "";
    }
    var Gotprojid = $("#hdnEditProjId").val();
    if (grid.GetVisibleRowsOnPage() > 0 && gridVal != "" && gridVal != null && Gotprojid != lostProjectId) {
               
        jConfirm('Project Change will  blank  the grid. Confirm ?', 'Confirmation Dialog', function (r)
        {
            cLoadingPanelCRP.Show();
            if (r == true) {
                // LoadDocument();
                grid.PerformCallback("GridBlank");

            }
            else {
               
                clookup_Project.gridView.SelectItemsByKey($("#hdnEditProjId").val());
                
            }
            setTimeout(function () {
                cLoadingPanelCRP.Hide();
            }, 800);
           
        });
    }
    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
    //var projID = clookup_Project.GetValue();
    $.ajax({
        type: "POST",
        url: 'VendorPaymentReceipt.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}
function Project_gotFocus() {
   
    //clookup_Project.ShowDropDown();
    var ProjectId = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
    $("#hdnEditProjId").val(ProjectId);

}
function ProjectValueChange(s, e) {  
    var projID = clookup_Project.GetValue();
    $.ajax({
        type: "POST",
        url: 'VendorPaymentReceipt.aspx/getHierarchyID',
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