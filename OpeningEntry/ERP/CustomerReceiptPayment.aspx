<%@ Page Title="CustomerReceiptPayment" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" 
    CodeBehind="CustomerReceiptPayment.aspx.cs" Inherits="OpeningEntry.ERP.CustomerReceiptPayment" %>
<%@ Register Src="~/UserControls/ucPaymentDetails.ascx" TagPrefix="uc1" TagName="ucPaymentDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
         #gridBatch_DXStatus {
           display: none;
       }
        .voucherno {
            position: absolute;
            right: -2px;
            top: 36px;
        }
         #openlink {
            font-size: 18px;
        }
        .iconTransDate {
            position: absolute;
            right: -1px;
            top: 36px;
        }

        .iconBranch {
            position: absolute;
            right: -1px;
            top: 36px;
        }

        .iconCustomer {
            position: absolute;
            right: -1px;
            top: 29px;
        }

        .iconCashBank{
            position: absolute;
            right: -1px;
            top: 29px;
        }
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }



        .abs {
            position: absolute;
            right: -20px;
            top: 10px;
        }

        .fa.fa-exclamation-circle:before {
            font-family: FontAwesome;
        }

        .tp2 {
            right: -18px;
            top: 7px;
            position: absolute;
        }

        #txtCreditLimit_EC {
            position: absolute;
        }
        /*.dxgv {
            display: none;
        }*/

        .dxgv.dx-al, .dxgv.dx-ar, .dx-nowrap.dxgv, .gridcellleft.dxgv, .dxgv.dx-ac, .dxgvCommandColumn_PlasticBlue.dxgv.dx-ac {
            display: table-cell !important;
        }

        #gridBatch_DXMainTable tr td:first-child {
            display: table-cell !important;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        #gridBatch_DXStatus span > a {
            display: none;
        }
    </style>
    <script type="text/javascript">
        var globalRowIndex;
        var ReciptOldValue;
        var ReciptNewValue;
        var PaymentOldValue;
        var PaymentNewValue;
        var canCallBack = true;
        var DateChange = "";
        function ddlBranch_SelectedIndexChanged()
        {
            var branch = $("#ddlBranch").val();
            cddlCashBank.PerformCallback(branch);
        }
        $(document).ready(function () {
            var isCtrl = false;
            document.onkeydown = function (e) {
                if (event.keyCode == 78 && event.altKey == true) {
                    SaveButtonClickNew();//........Alt+N
                }
                else if (event.keyCode == 88 && event.altKey == true) {
                    SaveButtonClick();//........Alt+X
                }
                else if (event.keyCode == 85 && event.altKey == true) {
                    OpenUdf();
                }              

            }
        });
        function AllControlInitilize() {
            if (canCallBack) {
                grid.PerformCallback();
                canCallBack = false;
            }
        }
        function selectValue() {
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            if(type=='S')
            {
                $('#Multipletype').hide();
                $('#singletype').show();
                $('#tdCashBankLabel').show();
            }
            else if(type=='M')
            {
                
                $('#tdCashBankLabel').hide();
                $('#Multipletype').show();
                $('#singletype').hide();
            }
        }
        function Type_EndCallback()
        {
            cCmbScheme.Focus();
        }
        function InstrumentDate_GotFocus()
        {
            cInstDate.ShowDropDown();
        }
        function InstrumentType_GotFocus()
        {
            cComboInstrumentTypee.ShowDropDown();
        }
        function CashBank_GotFocus()
        {
            cddlCashBank.ShowDropDown();
        }
        function Customer_GotFocus() {
            gridLookup.ShowDropDown();
        }
        function TransDate_GotFocus() {
            cdtTDate.ShowDropDown();
        }
        function NumberingScheme_GotFocus()
        {
            cCmbScheme.ShowDropDown();
        }
        function CurrencyGotFocus() {
            cCmbCurrency.ShowDropDown();
        }
        $(document).ready(function () {
            $("#openlink").on("click", function () {
                //window.location.href='master/Contact_general.aspx?id=ADD';
                window.open('../master/Contact_general.aspx?id=ADD', '_blank');
            });
        });
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/Opening/ERP/frm_BranchUdfPopUp.aspx?Type=CRP&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        function acbpCrpUdfEndCall(s, e)
        {
            if (cacbpCrpUdf.cpUDF) {
                if (cacbpCrpUdf.cpUDF == "true") {
                    //cacbpCrpUdf.cpUDF = null;
                    SelectAllData();
                    grid.batchEditApi.StartEdit(-1, 2);
                    grid.batchEditApi.StartEdit(0, 2);
                    grid.UpdateEdit();
                   
                }
                else {
                    cacbpCrpUdf.cpUDF = null;
                    jAlert('UDF is set as Mandatory. Please enter values.');
                    
                }
            }
        }
        // End Udf Code
        function PerformCallToGridBind() {
            grid.PerformCallback('BindGridOnSalesInvoice' + '~' + '@');
            //cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
            //$('#hdnPageStatus').val('Quoteupdate');
            cPopup_invoice.Hide();
            OnAddNewClick();
            return false;
        }
        function CashBank_SelectedIndexChanged()
        {
            $('#MandatoryCashBank').hide();
            var CashBankText= cddlCashBank.GetText();
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
        function checkTextAreaMaxLength(textBox, e, length) {

            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }
            }
        }
        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }
        //..................Sales Invoice PopUp...............
        function GetInvoiceMsg(s, e) {
            //if (InsgridBatch.GetVisibleRowsOnPage() == 0) {
            var VoucherAmount = document.getElementById('hdnPageStatus').value;
            if (VoucherAmount == 'update')
            {

            }
            else
            {
                jConfirm('Wish to auto adjust amount with sale Invoice(s)?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        cPopup_invoice.Show();
                        var amount = ctxtVoucherAmount.GetValue();
                        var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                        $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                        cgrid_SalesInvoice.PerformCallback('BindSalesInvoiceDetails' + '~' + amount);
                    }
                    else {
                        ctxtVoucherAmount.SetValue(c_txt_Debit.GetValue());

                        grid.batchEditApi.StartEdit(-1, 1);
                    }
                 });
            }
               
            //}
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
         
            Payment_Lost_Focus(s, e);
            var PaymentValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0";
            var ReceiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";

            if (PaymentValue > 0) {
                recalculateReceipt(grid.GetEditor('Receipt').GetValue());
                grid.GetEditor('Receipt').SetValue("0");
            }

            //..................CheckAmount.......................
            var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            var DocumentNo = (grid.GetEditor('DocumentID').GetValue() != null) ? grid.GetEditor('DocumentID').GetValue() : "0";
            cacpCheckAmount.PerformCallback(Type + '~' + DocumentNo);
            //.................End.........................

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
            ctxtVoucherAmount.SetValue(parseFloat(CurrentSum - newDif));

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

            ctxtTotalPayment.SetValue(parseFloat(CurrentSum - newDif));
            ctxtVoucherAmount.SetValue(parseFloat(CurrentSum - newDif));
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
            ReceiptLostFocus(s, e);
            var RecieveValue = (grid.GetEditor('Receipt').GetValue() != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";
            console.log(RecieveValue);
            var receiptValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0";

            if (RecieveValue > 0) {
                recalculatePayment(grid.GetEditor('Payment').GetValue());
                grid.GetEditor('Payment').SetValue("0");
            }

            //..................CheckAmount.......................
            var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";           
            var DocumentNo = (grid.GetEditor('DocumentID').GetValue() != null) ? grid.GetEditor('DocumentID').GetValue() : "0";          
            cacpCheckAmount.PerformCallback(Type+'~'+DocumentNo);
            //.................End.........................
        }
        function aacpCheckAmountEndCall(s, e) {
            if (cacpCheckAmount.cpUnPaidAmount) {
                if (cacpCheckAmount.cpUnPaidAmount != null) {
                  
                    var RecieveValue = (parseFloat(grid.GetEditor('Receipt').GetValue()) != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";
                    var PaymentValue = (parseFloat(grid.GetEditor('Payment').GetValue()) != null) ? parseFloat(grid.GetEditor('Payment').GetValue()) : "0";
                    var UnPaidAmoun = parseFloat(cacpCheckAmount.cpUnPaidAmount);
                    if (RecieveValue > UnPaidAmoun)
                    {
                        jAlert('Receipt amount cannot be more then the selected Document Amount.', 'Alert', function () {

                            //var vouchertype = cComboVoucherType.GetValue();
                            var vouchertype = document.getElementById("ComboVoucherType").value;
                            if (vouchertype == 'R')
                            {
                                grid.batchEditApi.StartEdit(globalRowIndex, 4);
                                var newValue = grid.GetEditor('Receipt').GetValue();
                                var finalValue = newValue - UnPaidAmoun;
                                grid.GetEditor('Receipt').SetValue(UnPaidAmoun);
                                var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());

                                ctxtVoucherAmount.SetValue(VoucherAmount - finalValue);
                                c_txt_Debit.SetValue(VoucherAmount - finalValue);
                            }
                            else
                            {
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
                    else if (PaymentValue > UnPaidAmoun)
                    {
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

                                ctxtVoucherAmount.SetValue(VoucherAmount - finalValue);
                                ctxtTotalPayment.SetValue(VoucherAmount - finalValue);
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

        //..........Save & New.....
        function SaveButtonClickNew() {
           
            $('#<%=hdnBtnClick.ClientID %>').val("Save_New");

            $('#<%=hdnRefreshType.ClientID %>').val('N');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            if (document.getElementById('<%= txtVoucherNo.ClientID %>').value == "") {
                $("#MandatoryBillNo").show();
                return false;
            }
            var TransDate = cdtTDate.GetDate();
            if (TransDate == null) {
                $("#MandatoryTransDate").show();
                return false;
            }
            var branch = $("#ddlBranch").val();
            if (branch == "") {
                $("#MandatoryBranch").show();
                return false;
            }
            var customerId = GetObjectID('hdnCustomerId').value
            if (customerId == '' || customerId == null) {

                $('#MandatorysCustomer').attr('style', 'display:block');
                return false;
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }
            var CashBank = cddlCashBank.GetValue();           
            if (CashBank == null) {
                $("#MandatoryCashBank").show();
                return false;
            }  

           
            //Code added by Sudip
            grid.batchEditApi.EndEdit();
            var gridCount = grid.GetVisibleRowsOnPage();

            var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
            var txtTotalPayment = ctxtTotalPayment.GetValue() != null ? ctxtTotalPayment.GetValue() : 0;
            //var VoucherType = cComboVoucherType.GetValue();
            var VoucherType = document.getElementById("ComboVoucherType").value;
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
                    var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                    if (VoucherType == "P") {
                        if (txtTotalAmount <= txtTotalPayment) {
                            // grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                        else {
                            jAlert('Payment amount can not be less than receipt amount ');
                        }
                    }
                    if (VoucherType == "R") {
                        if (txtTotalAmount >= txtTotalPayment) {
                           // grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                        else {
                            jAlert('Receipt amount can not be less than payment amount');
                        }
                    }
                }
                else {
                    jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                }
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
            }
        }
        function SaveButtonClick() {
            $('#<%=hdnBtnClick.ClientID %>').val("Save_Exit");

            $('#<%=hdnRefreshType.ClientID %>').val('E');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            if (document.getElementById('<%= txtVoucherNo.ClientID %>').value == "") {
                $("#MandatoryBillNo").show();
                return false;
            }
            var TransDate = cdtTDate.GetDate();
            if (TransDate == null) {
                $("#MandatoryTransDate").show();
                return false;
            }
            var branch = $("#ddlBranch").val();
            if (branch == "") {
                $("#MandatoryBranch").show();
                return false;
            }
            var customerId = GetObjectID('hdnCustomerId').value
            if (customerId == '' || customerId == null) {

                $('#MandatorysCustomer').attr('style', 'display:block');
                return false;
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }
            var CashBank = cddlCashBank.GetValue();
            if (CashBank == null) {
                $("#MandatoryCashBank").show();
                return false;
            }          

            //Code added by Sudip
            grid.batchEditApi.EndEdit();
            var gridCount = grid.GetVisibleRowsOnPage();

            var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
            var txtTotalPayment = ctxtTotalPayment.GetValue() != null ? ctxtTotalPayment.GetValue() : 0;
           // var VoucherType = cComboVoucherType.GetValue();
            var VoucherType = document.getElementById("ComboVoucherType").value;
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
            if (gridCount > 0)
            {
               
                if (IsType == "Y") {
                    var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                    if (VoucherType == "P") {
                        if (txtTotalAmount <= txtTotalPayment) {
                            // grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                        else {
                            jAlert('Payment amount can not be less than receipt amount ');
                        }
                    }
                    if (VoucherType == "R") {
                        if (txtTotalAmount >= txtTotalPayment) {
                            // grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                        }
                        else {
                            jAlert('Receipt amount can not be less than payment amount');
                        }
                    }
                }
                else {
                    jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                }
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
            }
        }


        //..................Batch Grid.....................
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }
        function OnCustomButtonClick(s, e) {
            if (e.buttonID == 'CustomDelete') {
                if (grid.GetVisibleRowsOnPage() > 1) {
                    grid.batchEditApi.StartEdit(e.visibleIndex, 1);
                    var ReceiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0";
                    var PaymentValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0";
                    if (ReceiptValue != "0")
                    {
                        var VoucherAmount = ctxtVoucherAmount.GetValue();
                        ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                        c_txt_Debit.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                    }
                    if (PaymentValue != "0")
                    {
                        var VoucherAmount = ctxtVoucherAmount.GetValue();
                        ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue));
                        ctxtTotalPayment.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue));
                    }

                    grid.DeleteRow(e.visibleIndex);
                }
            }
            if (e.buttonID == 'AddNew') {
                var TypeValue = (grid.GetEditor('Type').GetText() != null) ? grid.GetEditor('Type').GetText() : "0";
                if (TypeValue != "") {
                    OnAddNewClick();
                }
                else {
                    grid.batchEditApi.StartEdit(e.visibleIndex, 1);
                }
            }
        }


        function OnEndCallback(s, e) {
            if (grid.cpTotalAmount != null) {
               // var total_receipt = InsgridBatch.cpTotalAmount.split('~')[0];
                var total_payment = grid.cpTotalAmount;
               
                c_txt_Debit.SetValue(total_payment);
                //ctxtTotalPayment.SetValue(total_payment);
                ctxtVoucherAmount.SetValue(total_payment);
                grid.cpTotalAmount = null;
            }
            var value = document.getElementById('hdnRefreshType').value;
            var pageStatus = document.getElementById('hdnPageStatus').value;
            var IsInvoiceTagged = document.getElementById('IsInvoiceTagged').value;
            //if (grid.cpUDF != null) {
            //    jAlert(grid.cpUDF);
            //    grid.batchEditApi.StartEdit(0, 2);
            //    grid.cpUDF = null;
            //    return;
            //}
            if (grid.cpSaveSuccessOrFail == "outrange") {
                OnAddNewClick();
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More Voucher Number as Voucher Scheme Exausted.<br />Update The Scheme and Try Again');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                OnAddNewClick();
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Save as Duplicate Voucher Number.');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                //OnAddNewClick();
               // grid.batchEditApi.StartEdit(0, 2);
                //OnAddNewClick();
                grid.AddNewRow();
                
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please fill Document');
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                // OnAddNewClick();
                 grid.batchEditApi.StartEdit(0, 2);
              
                jAlert('Please try after sometime.');
                grid.cpSaveSuccessOrFail = null;
            }
            else {
                var Voucher_Number = grid.cpVouvherNo;
                var Order_Msg = "Customer Receipt/Payment No. " + Voucher_Number + " saved.";
               
                if (value == "E") {
                    if (Voucher_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [CustomerReceipt/Payment]', function (r) {
                            if (r == true) {
                                grid.cpVouvherNo = null;
                                window.location.assign("CustomerReceiptPaymentList.aspx");
                            }
                        });

                    }
                    else {
                        window.location.assign("CustomerReceiptPaymentList.aspx");
                    }
                    if (IsInvoiceTagged == "Y") {
                        window.parent.capcReciptPopup.Hide();
                        //window.parent.cgridPendingApproval.PerformCallback();
                        window.location.assign("CustomerReceiptPaymentList.aspx");
                    }
                }
                else if (value == "N") {
                    if (Voucher_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [CustomerReceipt/Payment]', function (r) {

                            grid.cpVouvherNo = null;
                            if (r == true) {
                                ctxtVoucherAmount.SetValue("0.0");
                                c_txt_Debit.SetValue("0.0");
                                window.location.assign("CustomerReceiptPayment.aspx?key=ADD");
                                //window.location.href = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=ADD';
                            }
                        });
                    }
                    else {
                        ctxtVoucherAmount.SetValue("0.0");
                        c_txt_Debit.SetValue("0.0");
                        window.location.assign("CustomerReceiptPayment.aspx?key=ADD");
                        //window.location.href = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=ADD';
                    }
                    if (IsInvoiceTagged == "Y") {
                        window.parent.capcReciptPopup.Hide();
                        //window.parent.cgridPendingApproval.PerformCallback();
                    }
                }
                else {
                    if (pageStatus == "first") {
                        OnAddNewClick();
                        VisibleColumn();
                        grid.batchEditApi.EndEdit();

                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        var VoucherType = document.getElementById("ComboVoucherType").value;
                       
                        if (VoucherType == "P") {
                          
                            grid.GetEditor('Receipt').SetEnabled(false);
                            grid.GetEditor('Payment').SetEnabled(true);
                        }
                        else {
                            
                            grid.GetEditor('Payment').SetEnabled(false);
                            grid.GetEditor('Receipt').SetEnabled(true);
                        }
                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        
                        if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
                            //ctxtRate.SetValue("0.0");
                            ctxtRate.SetEnabled(false);
                        }
                    }
                    else if (pageStatus == "update") {
                       // OnAddNewClick();
                       
                        //Type.PerformCallback(cComboVoucherType.GetValue());
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");

                        if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
                            //ctxtRate.SetValue("0.0");
                            ctxtRate.SetEnabled(false);
                        }
                       
                       var frontRow = 0;
                        //var backRow = -1;

                        for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                           // var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Type')) : "";
                            var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'Type') != null ) ? (grid.batchEditApi.GetCellValue(frontRow, 'Type')) : "";
                            if (backProduct != "Advance" && backProduct !="") {
                                DateChange = "Y";
                                break;
                            }
                            //backRow--;
                            frontRow++;
                        }


                    }
                }
            }
            if (grid.cpView == "1") {
                viewOnly();
            }
        }
        function viewOnly() {
            $('.form_main').find('input, textarea, button, select').attr('disabled', 'disabled');

            grid.SetEnabled(false);
            $('#ComboVoucherType').attr('disabled', 'disabled');
            
           
            cdtTDate.SetEnabled(false);
            $('#ddlBranch').attr('disabled', 'disabled');
            gridLookup.SetEnabled(false);
            cContactPerson.SetEnabled(false);
            cddlCashBank.SetEnabled(false);
            cCmbCurrency.SetEnabled(false);
            ctxtRate.SetEnabled(false);
            cComboInstrumentTypee.SetEnabled(false);
            if (cComboInstrumentTypee.GetValue() != "CH")
            {
                ctxtInstNobth.SetEnabled(false);
                cInstDate.SetEnabled(false);
            }
            

            $('#txtNarration').attr('disabled', 'disabled');
            ctxtVoucherAmount.SetEnabled(false);

            cbtnSaveNew.SetVisible(false);
            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveUdf.SetVisible(false);


            
        }
function VisibleColumn() {
    //var VoucherType = cComboVoucherType.GetValue();
    var VoucherType = document.getElementById("ComboVoucherType").value;
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
    //Type.PerformCallback(cComboVoucherType.GetValue());
    //grid.StartEditRow(0);
   grid.PerformCallback('Display');
}
        //...............End Batch Grid..................
function changeFocusDate() {
    
   if(DateChange == "Y")
    {
       jAlert('Cannot change the date as tagged document date is after the date you have entered now.', 'Alert', function () {
           
           var TransDate = document.getElementById('hdnDate').value;
           var strDate = new Date(TransDate);
           console.log(TransDate);
           cdtTDate.SetDate(strDate);
       });
    }
    //$('#ddlBranch').focus();
}
function CmbScheme_ValueChange() {

    var val = cCmbScheme.GetValue();
    $.ajax({
        type: "POST",
        url: 'CustomerReceiptPayment.aspx/getSchemeType',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{sel_scheme_id:\"" + val + "\"}",
        success: function (type) {
            var schemetypeValue = type.d;
            var schemetype = schemetypeValue.toString().split('~')[0];
            var schemelength = schemetypeValue.toString().split('~')[1];
            $('#txtVoucherNo').attr('maxLength', schemelength);

            if (schemetype == '0') {
                $('#<%=hdnSchemaType.ClientID %>').val('0');
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                // document.getElementById("txtVoucherNo").focus();
                $('#txtVoucherNo').focus();
            }
            else if (schemetype == '1') {
                $('#<%=hdnSchemaType.ClientID %>').val('1');
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                $("#MandatoryBillNo").hide();
                cdtTDate.Focus();
            }
            else if (schemetype == '2') {
                $('#<%=hdnSchemaType.ClientID %>').val('2');
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
            }
            else if (schemetype == 'n') {
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
            }
        }
    });
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
        function rbtnType_SelectedIndexChanged()
        {
            deleteAllRows()
            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
            ctxtVoucherAmount.SetValue("0");
            //var VoucherType = cComboVoucherType.GetValue();
            var VoucherType = document.getElementById("ComboVoucherType").value;
            cCmbScheme.PerformCallback(VoucherType);
            Type.PerformCallback(VoucherType);
            if (VoucherType == "P") {
                $('#multipleredio').hide();
                grid.GetEditor('Receipt').SetEnabled(false);
                grid.GetEditor('Payment').SetEnabled(true);
            }
            else {
                $('#multipleredio').show();
                grid.GetEditor('Payment').SetEnabled(false);        
                grid.GetEditor('Receipt').SetEnabled(true);
            }
        }
function Currency_Rate() {
    var Campany_ID = '<%=Session["LastCompany"]%>';
    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
    var basedCurrency = LocalCurrency.split("~");
    var Currency_ID = cCmbCurrency.GetValue();


    if ($("#ddl_Currency").text().trim() == basedCurrency[1]) {
        ctxtRate.SetValue("");
        ctxtRate.SetEnabled(false);
    }
    else {
        $.ajax({
            type: "POST",
            url: "CustomerReceiptPayment.aspx/GetRate",
            data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                ctxtRate.SetValue(data);
                ctxtRate.SetEnabled(true);
            }
        });
        //ctxtRate.SetEnabled(true);
    }
}
function InstrumentTypeSelectedIndexChanged() {
    $("#MandatoryInstrumentType").hide();
    var InstType = cComboInstrumentTypee.GetValue();

    if (InstType == "CH") {
        $('#<%=hdnInstrumentType.ClientID %>').val(0);
        document.getElementById("divInstrumentNo").style.display = 'none';
        document.getElementById("tdIDateDiv").style.display = 'none';
    }
    else {
        $('#<%=hdnInstrumentType.ClientID %>').val(InstType);
        document.getElementById("divInstrumentNo").style.display = 'block';
        document.getElementById("tdIDateDiv").style.display = 'block';
    }
}
//...............Customer LookUp.....
function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
    gridquotationLookup.SetEnabled(true);
}
function GetContactPerson(e) {
    $('#MandatorysCustomer').hide();
    var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
    if (key != null && key != '') {
        var startDate = new Date();
        // startDate = cPLSalesOrderDate.GetValueString();
        // cchkBilling.SetChecked(false);
        //  cchkShipping.SetChecked(false);
        // cContactPerson.PerformCallback('BindContactPerson~' + key);

    }
    GetObjectID('hdnCustomerId').value = key;
}
//...............End............
    </script>
    <script>
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
        function DocumentType_SelectedIndexChanged(s, e) {
            var currentValue = s.GetValue();//grid.GetEditor('cType').GetValue();//s.GetValue();//
            if(currentValue="Advance")
            {

            }
            //if (lastDocumentType == currentValue) {
            //    if (DocumentID.GetSelectedIndex() < 0)
            //        DocumentID.SetSelectedIndex(0);
            //    return;
            //}

            //lastDocumentType = currentValue;
            //DocumentID.PerformCallback(currentValue);
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
    </script>
     <%--Batch Product Popup Start--%>

    <script>
        function ProductKeyDown(s, e) {
            console.log(e.htmlEvent.key);
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {

                s.OnButtonClick(0);
            }
        }

        function DocumentButnClick(s, e) {
            if (e.buttonIndex == 0) {

                if (cdocumentLookUp.Clear()) {
                  
                    cDocumentpopUp.Show();
                    cdocumentLookUp.Focus();
                   // cdocumentLookUp.ShowDropDown();
                }
                var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
                cCallbackPanelDocumentNo.PerformCallback('Type~' + Type);
            }
        }

        function DocumentlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cDocumentpopUp.Hide();
                //var vouchertype = cComboVoucherType.GetValue();
                var vouchertype = document.getElementById("ComboVoucherType").value;
                if (vouchertype == 'P') {
                    grid.batchEditApi.StartEdit(globalRowIndex, 4);
                }
                else {
                    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }
            }
        }
        
        function DocumentSelected(s, e) {

            if (cdocumentLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                cDocumentpopUp.Hide();
                //var vouchertype = cComboVoucherType.GetValue();
                var vouchertype = document.getElementById("ComboVoucherType").value;
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
            var ProductCode = cdocumentLookUp.GetValue();
            if (!ProductCode) {
                LookUpData = null;
            }
            cDocumentpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            var ReceiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0";
            var PaymentValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0";
            grid.GetEditor("DocumentID").SetText(DocumentID);
            //var vouchertype = cComboVoucherType.GetValue();
            var vouchertype = document.getElementById("ComboVoucherType").value;
            if (vouchertype == 'P') {
                grid.GetEditor("Payment").SetText(unpaidamount);

                var VoucherAmount = ctxtVoucherAmount.GetValue();
                ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue) + parseFloat(unpaidamount));
                var payment = ctxtTotalPayment.GetValue();
                ctxtTotalPayment.SetValue(parseFloat(payment) + parseFloat(unpaidamount) - parseFloat(PaymentValue));
            }
            else
            {
                grid.GetEditor("Receipt").SetText(unpaidamount);
                var VoucherAmount = ctxtVoucherAmount.GetValue();
                ctxtVoucherAmount.SetValue(parseFloat(unpaidamount) + parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                var Receipt = c_txt_Debit.GetValue();
                c_txt_Debit.SetValue(parseFloat(Receipt) + parseFloat(unpaidamount) - parseFloat(ReceiptValue));

            }
          
           
            if (LookUpData != null)
            {
                $('#<%=hdndocumentno.ClientID %>').val(DocumentID + ',');
            }
           
           
            grid.GetEditor("DocumentNo").SetText(ProductCode);
           // var vouchertype = cComboVoucherType.GetValue();
            var vouchertype = document.getElementById("ComboVoucherType").value;
            if(vouchertype=='P')
            {
                // grid.GetEditor("Payment").Focus();
                grid.batchEditApi.EndEdit();
               // grid.batchEditApi.StartEdit(globalRowIndex)
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                
            }
            else
            {
                //grid.GetEditor("Payment").Focus();
                grid.batchEditApi.StartEdit(globalRowIndex,4);
            }
            
        }
        function CallbackPanelDocumentNo_endcallback()
        {
            cdocumentLookUp.ShowDropDown();
            cdocumentLookUp.Focus();
        }
    </script>

    <%--Batch Product Popup End--%>
    <style>
        #txtRate_EC.dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }
        .bgrnd {
            background: #f5f4f3; 
            padding: 5px 0 5px 0; 
            margin-bottom: 10px; 
            border-radius: 4px; 
            border: 1px solid #ccc;
        }
        table.pad>tbody>tr>td {
            padding:2px 8px;
        }
        table#gridBatch, table#gridBatch>tbody>tr>td>.dxgvHSDC>div, table#gridBatch>tbody>tr>td>.dxgvCSD {
            width:100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-title">
        <h3>
            <asp:Label ID="lblHeadTitle" Text="Add Customer Receipt/Payment" runat="server"></asp:Label>
        </h3>
        <div id="btncross" class="crossBtn" style="margin-left: 100px;"><a href="CustomerReceiptPaymentList.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main  clearfix">

        <div id="DivEntry">
            <div id="divChangable" runat="server" style="background: #f5f4f3; padding: 5px 0 5px 0; margin-bottom: 10px; border-radius: 4px; border: 1px solid #ccc;">
                <div class="">

                    <div class="col-md-3">
                        <label for="exampleInputName2" style="margin-top: 8px">
                            Voucher Type <b id="bTypeText" runat="server" style="width: 20%; display: none; font-size: 12px"></b>
                        </label>
                        <div>
                            <%--<dxe:ASPxComboBox ID="ComboVoucherType" runat="server" ClientInstanceName="cComboVoucherType" Font-Size="12px"
                                ValueType="System.String" Width="100%" EnableIncrementalFiltering="True">
                                <Items>
                                    <dxe:ListEditItem Value="R" Text="Receipt"></dxe:ListEditItem>
                                    <dxe:ListEditItem Value="P" Text="Payment"></dxe:ListEditItem>
                                </Items>

                                <ClientSideEvents SelectedIndexChanged="rbtnType_SelectedIndexChanged" />
                            </dxe:ASPxComboBox>--%>
                            <asp:DropDownList ID="ComboVoucherType" runat="server"  Width="100%" onchange="rbtnType_SelectedIndexChanged()">
                                <asp:ListItem Text="Receipt" Value="R" />
                                <asp:ListItem Text="Payment" Value="P" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3" id="divNumberingScheme" runat="server">
                        <label style="margin-top: 8px">Select Numbering Scheme</label>
                        <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme"
                            DataSourceID="SqlSchematype" SelectedIndex="0" EnableCallbackMode="true"
                            TextField="SchemaName" ValueField="ID"
                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True" OnCallback="CmbScheme_Callback">
                            <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}" GotFocus="NumberingScheme_GotFocus"></ClientSideEvents>
                        </dxe:ASPxComboBox>

                        <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                    <div class="col-md-2">
                        <label style="margin-top: 8px">Voucher No. <span style="color: red">*</span></label>
                        <div>
                            <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="30">                             
                            </asp:TextBox>
                            <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label style="margin-top: 8px">Trans.Date <span style="color: red">*</span>  </label>
                        <div>
                            <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom"
                                Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                <ButtonStyle Width="13px"></ButtonStyle>
                                <%--  <ClientSideEvents GotFocus="TransDate_GotFocus" DateChanged="changeFocusDate" ></ClientSideEvents>--%>
                            </dxe:ASPxDateEdit>
                            <span id="MandatoryTransDate" class="iconTransDate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>

                    

                    <div class="col-md-2 lblmTop8">

                        <label>Branch <span style="color: red">*</span></label>
                        <div>
                            <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" onchange="ddlBranch_SelectedIndexChanged()"
                                DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                            </asp:DropDownList>
                            <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="col-md-3 lblmTop8">
                        <label>
                            <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                            </dxe:ASPxLabel>
                            <span style="color: red">*</span>
                             <%-- <i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i>--%>
                        </label>
                        <dxe:ASPxGridLookup ID="lookup_Customer" runat="server"  ClientInstanceName="gridLookup"
                            KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" Width="150" Settings-AutoFilterCondition="Contains" />
                                <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Width="150" Settings-AutoFilterCondition="Contains">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="2" Caption="Type" Settings-AutoFilterCondition="Contains" Width="150">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="cnt_internalid" Visible="false" VisibleIndex="3" Settings-AllowAutoFilter="False" Width="150">
                                    <Settings AllowAutoFilter="False"></Settings>
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>

                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                <%-- <Settings ShowFilterRow="True" ShowStatusBar="Visible" />--%>

                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                            </GridViewProperties>
                            <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" GotFocus="Customer_GotFocus" />
                            <ClearButton DisplayMode="Auto">
                            </ClearButton>
                        </dxe:ASPxGridLookup>
                        <span id="MandatorysCustomer" class="iconCustomer pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                    <div class="col-md-3 lblmTop8">
                        <label>
                            <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person" CssClass="inline">
                            </dxe:ASPxLabel>
                        </label>
                        <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback"  Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">
                        </dxe:ASPxComboBox>
                    </div>
                    <div class="col-md-2 lblmTop8" id="tdCashBankLabel">
                        <label>Cash/Bank <span style="color: red">*</span></label>
                        <dxe:ASPxComboBox ID="ddlCashBank" runat="server"  ClientInstanceName="cddlCashBank"  Width="100%" OnCallback="ddlCashBank_Callback">
                             <ClientSideEvents SelectedIndexChanged="CashBank_SelectedIndexChanged" GotFocus="CashBank_GotFocus" />
                        </dxe:ASPxComboBox>
                        <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>

                    

                    <div class="col-md-1 lblmTop8">
                        <label>Currency</label>
                        <div>
                            <dxe:ASPxComboBox ID="CmbCurrency" EnableIncrementalFiltering="True" ClientInstanceName="cCmbCurrency"
                                DataSourceID="SqlCurrencyBind"
                                TextField="Currency_AlphaCode" ValueField="Currency_ID" SelectedIndex="0"
                                runat="server" ValueType="System.String" EnableSynchronization="True" Width="100%" CssClass="pull-left">
                                <ClientSideEvents ValueChanged="function(s,e){Currency_Rate()}" GotFocus="CurrencyGotFocus"></ClientSideEvents>
                            </dxe:ASPxComboBox>

                        </div>
                    </div>
                    <div class="col-md-1 rate lblmTop8">
                        <label>Rate  </label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtRate" HorizontalAlign="Right" ClientInstanceName="ctxtRate" Width="100%" CssClass="pull-left">
                                <MaskSettings Mask="<0..9999>.<0..99999>" IncludeLiterals="DecimalSymbol" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                     <div id="multipleredio" class="col-md-2" runat="server">
                          <div style="padding-top:20px;">
                            <asp:RadioButtonList ID="rdl_MultipleType" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();">
                                <asp:ListItem Text="Single" Value="S" Selected></asp:ListItem>
                                <asp:ListItem Text="Multiple" Value="M"></asp:ListItem>                            
                            </asp:RadioButtonList>
                          </div>
                     </div>
                     <div class="clear"></div>
                    <panel id="singletype">
                        <div class="col-md-2 lblmTop8">
                            <label style="">Instrument Type</label>
                            <div style="">
                                <dxe:ASPxComboBox ID="cmbInstrumentType" runat="server" ClientInstanceName="cComboInstrumentTypee" Font-Size="12px"
                                    ValueType="System.String" Width="100%" EnableIncrementalFiltering="True">
                                    <Items>

                                        <dxe:ListEditItem Text="Cheque" Value="C" Selected />
                                        <dxe:ListEditItem Text="Draft" Value="D" />
                                        <dxe:ListEditItem Text="E.Transfer" Value="E" />
                                        <dxe:ListEditItem Text="Cash" Value="CH" />
                                    </Items>
                                    <ClientSideEvents SelectedIndexChanged="InstrumentTypeSelectedIndexChanged" GotFocus="InstrumentType_GotFocus" />
                                </dxe:ASPxComboBox>

                            </div>
                        </div>
                   
                        <div class="col-md-2 lblmTop8" id="divInstrumentNo" style="" runat="server">
                            <label id="" style="">Instrument No</label>
                            <div id="">
                                <dxe:ASPxTextBox runat="server" ID="txtInstNobth" ClientInstanceName="ctxtInstNobth" Width="100%" MaxLength="30">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-2 lblmTop8" id="tdIDateDiv" style=""  runat="server">
                            <label id="tdIDateLable" style="">Instrument Date</label>
                            <div id="tdIDateValue" style="">
                                <dxe:ASPxDateEdit ID="InstDate" runat="server" EditFormat="Custom" ClientInstanceName="cInstDate"
                                    UseMaskBehavior="True" Font-Size="12px" Width="100%" EditFormatString="dd-MM-yyyy">
                                    <ClientSideEvents   GotFocus="InstrumentDate_GotFocus"></ClientSideEvents>
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>

                    </panel>
                   
                    <div class="clear"></div>
                     <div class="col-md-4 lblmTop8">
                        <label>Narration </label>
                        <div>
                            <asp:TextBox ID="txtNarration" runat="server" MaxLength="500" onkeydown="checkTextAreaMaxLength(this,event,'500');"
                                TextMode="MultiLine"
                                Width="100%" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 lblmTop8">
                        <label>Voucher Amount <span style="color: red">*</span> </label>
                        <div>
                            <dxe:ASPxTextBox ID="txtVoucherAmount" runat="server" ClientInstanceName="ctxtVoucherAmount" Width="100%" CssClass="pull-left">
                                <%--<ClientSideEvents LostFocus="function(s, e) { GetInvoiceMsg(e)}" />--%>
                                <ClientSideEvents TextChanged="function(s, e) { GetInvoiceMsg(e)}" />
                                 <MaskSettings Mask="<0..999999999999999999>.<0..99>" />
                                 <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                            </dxe:ASPxTextBox>
                            <span id="MandatoryVoucherAmount" class="iconVoucherAmount pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                   

                    <div class="clear"></div>
                     
                </div>
            </div>

            <div class="clear"></div>
            <div id="Multipletype" style="display:none" class="bgrnd">
                <uc1:ucPaymentDetails runat="server" ID="PaymentDetails" />
            </div>
            
        </div>
        <%-- Sales Invoice PopUp Start--%>
        <dxe:ASPxPopupControl ID="Popup_invoice" runat="server" ClientInstanceName="cPopup_invoice"
            Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <HeaderTemplate>
                <strong><span style="color: #fff">Select Invoice</span></strong>
                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                    <ClientSideEvents Click="function(s, e){ 
                                                            cPopup_invoice.Hide();
                                                        }" />
                </dxe:ASPxImage>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <input type="button" value="Select All Invoice" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="De-select All Invoice" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                    </div>
                    <dxe:ASPxGridView runat="server" KeyFieldName="DocumentID" ClientInstanceName="cgrid_SalesInvoice" ID="grid_SalesInvoice"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                        OnCustomCallback="grid_SalesInvoice_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Caption=" " VisibleIndex="0" />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" ReadOnly="true" Caption="Invoice No" FieldName="DocumentNo">
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="2" ReadOnly="true" Caption="Customer">
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" ReadOnly="true" Caption="Balance Amount" FieldName="Receipt">
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DocumentID" ReadOnly="true" Caption="DocumentID" Width="0">
                        </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsDataSecurity AllowEdit="true" />
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <%--<asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />--%>
                         <dxe:ASPxButton ID="Btn" ClientInstanceName="cbtnOK" runat="server"
                            AutoPostBack="false" CssClass="btn btn-primary  mLeft mTop"  Text="OK"
                            UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {PerformCallToGridBind();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <%-- Sales Invoice PopUp End--%>

       
        <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="gridBatch" KeyFieldName="ReceiptDetail_ID"
            OnBatchUpdate="gridBatch_BatchUpdate" OnCellEditorInitialize="gridBatch_CellEditorInitialize" OnDataBinding="gridBatch_DataBinding"
            OnCustomCallback="gridBatch_CustomCallback" OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="170">
            <SettingsPager Visible="false"></SettingsPager>
            <Columns>
                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption=" ">
                    <CustomButtons>
                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                        </dxe:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxe:GridViewCommandColumn>

                <dxe:GridViewDataComboBoxColumn Caption="Type" FieldName="Type" VisibleIndex="1" Width="140">
                    <PropertiesComboBox ClientInstanceName="Type" ValueField="ID" TextField="Type"
                        ClearButton-DisplayMode="Always" AllowMouseWheel="false">
                        <ClientSideEvents SelectedIndexChanged="DocumentType_SelectedIndexChanged" EndCallback="Type_EndCallback" />
                    </PropertiesComboBox>
                </dxe:GridViewDataComboBoxColumn>

               
                <dxe:GridViewDataButtonEditColumn FieldName="DocumentNo" Caption="Document Number" VisibleIndex="1" Width="14%">
                    <PropertiesButtonEdit>
                        <ClientSideEvents ButtonClick="DocumentButnClick" KeyDown="ProductKeyDown"  />
                        <Buttons>
                            <dxe:EditButton Text="..." Width="20px">
                            </dxe:EditButton>
                        </Buttons>
                    </PropertiesButtonEdit>
                </dxe:GridViewDataButtonEditColumn>
                <dxe:GridViewDataTextColumn FieldName="DocumentID" Caption="hidden Field Id" VisibleIndex="15" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Receipt" FieldName="Receipt" Width="130">
                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                        <MaskSettings Mask="<0..999999999999999999>.<0..99>" />
                        <ClientSideEvents KeyDown="OnKeyDown" LostFocus="ReceiptTextChange"
                            GotFocus="function(s,e){
                                    DebitGotFocus(s,e); 
                                    }" />
                        <ClientSideEvents />
                          <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                    </PropertiesTextEdit>
                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Payment" FieldName="Payment" Width="130">
                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                        <MaskSettings Mask="<0..999999999999999999>.<0..99>" />
                        <ClientSideEvents KeyDown="OnKeyDown" LostFocus="PaymentTextChange"
                            GotFocus="function(s,e){
                                    CreditGotFocus(s,e);
                                    }" />
                        <ClientSideEvents />
                          <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                        <%-- <ClientSideEvents LostFocus="PaymentLostFocus"
                                    GotFocus="PaymentgotFocus" />--%>
                    </PropertiesTextEdit>
                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Remarks" FieldName="Remarks" Width="200">
                    <PropertiesTextEdit>
                        <%-- <ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                    </PropertiesTextEdit>
                    <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="ReceiptDetail_ID" Caption="Srl No" ReadOnly="true" VisibleIndex="7" Width="0">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="6" Caption=" ">
                    <CustomButtons>
                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                            
                        </dxe:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxe:GridViewCommandColumn>
            </Columns>
            <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick"   RowClick="GetVisibleIndex" BatchEditStartEditing="GetVisibleIndex" 
               />
            <SettingsDataSecurity AllowEdit="true" />
            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
            </SettingsEditing>
            <%--<SettingsBehavior ColumnResizeMode="Disabled"   />--%>
        </dxe:ASPxGridView>
        
        <div class="text-center">
            <table style="margin-left: 40%; margin-top: 10px">
                <tr>
                    <td style="padding-right: 50px"><b>Total Amount</b></td>
                    <td style="width: 203px;">
                        <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                        </dxe:ASPxTextBox>
                    </td>
                    <td>
                        <dxe:ASPxTextBox ID="txtTotalPayment" runat="server" Width="105px" ClientInstanceName="ctxtTotalPayment" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                            <MaskSettings Mask="<0..999999999999>.<0..9999>" IncludeLiterals="DecimalSymbol" />
                        </dxe:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </div>

        <table style="width: 100%;">
            <tr>
                <td style="padding: 5px 0;">



                    <span>
                        <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveNew" runat="server"
                            AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save & N&#818;ew"
                            UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {SaveButtonClickNew();}" />
                        </dxe:ASPxButton>

                    </span>
                    <span id="tdSaveButton">
                        <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server"
                            AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save & Ex&#818;it"
                            UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                        </dxe:ASPxButton>

                    </span>
                    <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF"  UseSubmitBehavior="False"
                        CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                         <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                    </dxe:ASPxButton>

                </td>


            </tr>
        </table>

         <%--Batch Product Popup Start--%>

            <dxe:ASPxPopupControl ID="DocumentpopUp" runat="server" ClientInstanceName="cDocumentpopUp"  
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Document Number" AllowResize="true" ResizingMode="Postponed" Modal="true">
                
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By Invoice Number</strong></label>
                           <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDocumentNo" ClientInstanceName="cCallbackPanelDocumentNo" OnCallback="CallbackPanelDocumentNo_Callback">
                                <PanelCollection>
                                   <dxe:PanelContent runat="server">
                                         <dxe:ASPxGridLookup ID="documentLookUp" runat="server"  ClientInstanceName="cdocumentLookUp" Width="800"
                                        KeyFieldName="DocumentID"  TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="DocumentSelected" 
                                        ClientSideEvents-KeyDown="DocumentlookUpKeyDown">
                                        <Columns>
                                            <dxe:GridViewDataColumn FieldName="DocumentNumber" Caption="Document Number" Width="400">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="UnPaidAmount" Caption="UnPaid Amount" Width="350">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                
                                        </Columns>
                                         <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                            <Templates>
                                                <StatusBar>
                                                    <table class="OptionsTable" style="float: right">
                                                        <tr>
                                                            <td>
                                                                <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </StatusBar>
                                            </Templates>
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                        </GridViewProperties>
                                        </dxe:ASPxGridLookup>
                                   </dxe:PanelContent>
                                 </PanelCollection>
                                 <ClientSideEvents EndCallback="CallbackPanelDocumentNo_endcallback" />
                           </dxe:ASPxCallbackPanel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

          <%--  <asp:SqlDataSource runat="server" ID="ProductDataSource" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
                SelectCommand="proc_getDocumentDetails" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="All" />
                </SelectParameters>
            </asp:SqlDataSource>--%>

            <%--Batch Product Popup End--%>
             <%--UDF Popup --%>
            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" 
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
                   
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <asp:HiddenField runat="server" ID="IsUdfpresent" />
            <asp:HiddenField runat="server" ID="Keyval_internalId" />
         <%--UDF Popup End--%>
    </div>
    <div id="DivHiddenField">
        <asp:HiddenField ID="hdnBtnClick" runat="server" />
        <asp:HiddenField ID="hdnInstrumentType" runat="server" />
        <asp:HiddenField ID="hdnPageStatus" runat="server" />
        <asp:HiddenField ID="hdnSchemaType" runat="server" />
        <asp:HiddenField ID="hdfIsDelete" runat="server" />
        <asp:HiddenField ID="hdn_Mode" runat="server" />
        <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="IsInvoiceTagged" runat="server" />
        <asp:HiddenField ID="hdndocumentno" runat="server" Value="0" />
        <asp:HiddenField ID="hdnCustomerId" runat="server" />
        <asp:HiddenField ID="hdnDate" runat="server" />
    </div>

    <div id="DivDataSource">
        <asp:SqlDataSource ID="SqlSchematype" runat="server" 
            SelectCommand=""></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrencyBind" runat="server" ></asp:SqlDataSource>
        <asp:SqlDataSource ID="dsBranch" runat="server" 
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="grid"
        Modal="True">
    </dxe:ASPxLoadingPanel>
     <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
     </dxe:ASPxCallbackPanel>
     <dxe:ASPxCallbackPanel runat="server" ID="acpCheckAmount" ClientInstanceName="cacpCheckAmount" OnCallback="acpCheckAmount_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="aacpCheckAmountEndCall" />
     </dxe:ASPxCallbackPanel>
     
        <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
           <ClientSideEvents ControlsInitialized="AllControlInitilize" />
        </dxe:ASPxGlobalEvents>
</asp:Content>
