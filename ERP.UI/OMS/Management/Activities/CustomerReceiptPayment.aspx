<%@ Page Title="CustomerReceiptPayment" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerReceiptPayment.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerReceiptPayment" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/ucPaymentDetails.ascx" TagPrefix="uc1" TagName="ucPaymentDetails" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>

    <style>
        .horizontallblHolder {
            height: auto;
            border: 1px solid #12a79b;
            border-radius: 3px;
            overflow: hidden;
        }

            .horizontallblHolder > table > tbody > tr > td {
                padding: 8px 10px;
                background: #ffffff;
                background: -moz-linear-gradient(top, #ffffff 0%, #f3f3f3 50%, #ededed 51%, #ffffff 100%);
                background: -webkit-linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
                background: linear-gradient(to bottom, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ffffff',GradientType=0 );
            }

                .horizontallblHolder > table > tbody > tr > td:first-child {
                    background: #12a79b;
                    color: #fff;
                }

                .horizontallblHolder > table > tbody > tr > td:last-child {
                    font-weight: 500;
                    text-transform: uppercase;
                    color: #121212;
                }

        #aspxGridTax_DXStatus, #grid_DXStatus, #gridBatch_DXEmptyRow {
            display: none;
        }

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

        table.pad > tbody > tr > td {
            padding: 2px 8px;
        }

        table#gridBatch, table#gridBatch > tbody > tr > td > .dxgvHSDC > div, table#gridBatch > tbody > tr > td > .dxgvCSD {
            width: 100% !important;
        }

        #DivBilling [class^="col-md"], #DivShipping [class^="col-md"] {
            padding-top: 5px;
            padding-bottom: 5px;
        }
    </style>
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
            font-size: 15px;
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

        .iconCashBank {
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
        function CustomerButnClick(s, e) {
            $('#CustModel').modal('show');
        }

        function Customer_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#CustModel').modal('show');
                // setTimeout(function () { $("#txtCustSearch").focus(); }, 500);
            }

        }
        function Customerkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtCustSearch").val();
            // OtherDetails.BranchID = $('#ddl_Branch').val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtCustSearch").val() != "") {
                    callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
        }
        function SetCustomer(Id, Name) {
            var key = Id;
            if (key != null && key != '') {

                ctxtCustName.SetText(Name);
                GetObjectID('hdnCustomerId').value = key;
                GetContactPerson();
                $('#CustModel').modal('hide');
                cContactPerson.Focus();
            }
        }
        function ValueSelected(e, indexName) {
            if (e.code == "Enter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex") {
                        SetProduct(Id, name);
                    }
                    else if (indexName == "salesmanIndex") {
                        OnFocus(Id, name);
                    }
                    else {
                        SetCustomer(Id, name);
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
                if (thisindex > -1) {
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                }
                else {
                    //if (indexName == "ProdIndex")
                    //    $('#txtProdSearch').focus();
                    //else if (indexName == "salesmanIndex")
                    //    ctxtCreditDays.Focus();
                    //else
                    //    $('#txtCustSearch').focus();
                    $('#txtCustSearch').focus();
                }
            }
        }
    </script>
    <script type="text/javascript">
        function GlobalBillingShippingEndCallBack() {
            if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
                cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
                //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                //var key = cCustomerComboBox.GetValue();
                var key = GetObjectID('hdnCustomerId').value;
                if (key != null && key != '') {
                    var startDate = new Date();
                    var TransDate = cdtTDate.GetDate();
                    var branch = $("#ddlBranch").val();
                    cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + cdtTDate.GetDate().format('yyyy-MM-dd') + '~' + branch);
                }
            }
        }
    </script>

    <script type="text/javascript">
        var lastCRP = null;
        var IsPageLoad = false;
        var globalRowIndex;
        var ReciptOldValue;
        var ReciptNewValue;
        var PaymentOldValue;
        var PaymentNewValue;
        var canCallBack = true;
        var DateChange = "";
        var CustomerCurrentDateAmount = 0;
        function TypeGotFocus(s, e) {
            var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            if (Type == "OnAccount") {
                RemoveTypeAll();
                PropolateTypeByOnAccount();
            }
            if (Type == "Advance") {
                if (!$("#CB_GSTApplicable").is(':checked')) {
                    RemoveTypeAll();
                    PropolateTypeByReceipt();
                }
                else {
                    RemoveTypeAll();
                    PropolateTypeByReceiptProduct();
                }

            }
            if (Type == "CustDbNote" || Type == "Invoice") {

                if (!$("#CB_GSTApplicable").is(':checked')) {
                    RemoveTypeAll();
                    PropolateTypeByReceipt();
                }
                else {
                    RemoveTypeAll();
                    PropolateTypeByReceiptNonAdvance();
                }
            }
        }
        function DocumentNumberClose(s, e) {
            ctxtVoucherAmount.SetValue(0);
            clblRunningBalanceCapsul.SetValue(0.00);
            cDocumentpopUp.Hide();

        }
        function CheckedChange() {

            if ($("#CB_GSTApplicable").is(':checked')) {
                cproductLookUp.SetEnabled(true);
            }
            else {
                cproductLookUp.SetEnabled(false);
                //return;
            }
            var proMsg = 'Selected Product (s) to be cleaned if you unchecked this option.';
            if (cproductLookUp.GetValue() != null) {
                if ($("#CB_GSTApplicable").is(':checked')) {
                   
                }
                else
                {
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


            if (!$("#CB_GSTApplicable").is(':checked')) {
                RemoveTypeAll();
                PropolateTypeByReceipt();
            }

        }
        function ComponentPanel_EndCallBack(s, e) {

            var hfValue = $("#hfHSN_CODE").val();

            if (cproductLookUp.cpScheme != null) {

                jAlert("Please Select Numbering Scheme.", 'Alert Dialog: [CustomerReceipt/Payment]', function (r) {
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
                    //cproductLookUp.Focus();
                }
            }
            //cproductLookUp.ConfirmCurrentSelection();
            //cproductLookUp.HideDropDown();
            //cproductLookUp.Focus();
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

        //CustomerNew Add
        function AddcustomerClick() {
            var url = '/OMS/management/Master/Customer_general.aspx';
            AspxDirectAddCustPopup.SetContentUrl(url);
            AspxDirectAddCustPopup.Show();
        }
        function ParentCustomerOnClose(newCustId) {
            AspxDirectAddCustPopup.Hide();
            cCustomerCallBackPanel.PerformCallback('SetCustomer~' + newCustId);
            LoadCustomerAddress(newCustId, $('#ddlBranch').val(), 'CRP');
        }
        function CustomerSelected() {
            var internalID = gridLookup.GetValue();
            var TransDate = cdtTDate.GetDate();
            var branch = $("#ddlBranch").val();
            cCustomerCallBackPanel.PerformCallback('ChkTotalAmountInCash~' + internalID + '~' + TransDate + '~' + branch);
        }
        //function CustomerCallBackPanelEndCallBack() {
        //    if (cCustomerCallBackPanel.cpTotalTransectionAmount) {
        //        if (cCustomerCallBackPanel.cpTotalTransectionAmount != "") {
        //            CustomerCurrentDateAmount = parseFloat(cCustomerCallBackPanel.cpTotalTransectionAmount);
        //            cCustomerCallBackPanel.cpTotalTransectionAmount = null;
        //        }
        //    }
        //    GetContactPerson();
        //    cContactPerson.Focus();
        //}

        //.................Product LookUp.....................
        function CloseProductLookup() {
            cproductLookUp.ConfirmCurrentSelection();
            cproductLookUp.HideDropDown();
            cproductLookUp.Focus();
        }
        function ProductSelected() {

            document.getElementById("hdnTaxMode").value = "N";
            var VoucherType = document.getElementById("ComboVoucherType").value;
            var Products = cproductLookUp.GetValue();
            if (Products != null) {


                cComponentPanel.PerformCallback();
                RemoveTypeByReceiptPayment();
                // PropolateTypeByReceiptProduct();
            }
            else {
                Type.PerformCallback(VoucherType + "~" + "");
            }

        }
        //.................Product LookUp  End.....................


        function cmbContactPersonEndCall(s, e) {
            if (cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) {
                pageheaderContent.style.display = "block";
                $("#<%=divGSTIN.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblGSTIN.ClientID %>').innerHTML = cContactPerson.cpGSTN;
                cContactPerson.cpGSTN = null;
            }
            if (cContactPerson.cpTotalTransectionAmount) {
                if (cContactPerson.cpTotalTransectionAmount != "") {
                    cContactPerson = parseFloat(cContactPerson.cpTotalTransectionAmount);
                    cContactPerson.cpTotalTransectionAmount = null;
                }
            }
            // GetContactPerson();
            // cContactPerson.Focus();
            //cContactPerson.Focus();
        }

        function btncrossPopupOnClick() {
            var IsInvoiceTagged = document.getElementById('IsInvoiceTagged').value;
            var InvoicepageStatus = document.getElementById('hdnInvoicePageStatus').value;

            if (InvoicepageStatus == "E") {
                parent.CRP_SaveANDExit_Press();
            }
            else if (InvoicepageStatus == "N") {
                parent.CRP_SaveANDNew_Press();
            }
            else {
                parent.capcReciptPopup.Hide();
            }
        }

        function ddlBranch_SelectedIndexChanged() {
            var branch = $("#ddlBranch").val();
            // cddlCashBank.PerformCallback(branch);
        }
        $(document).ready(function () {
            var isCtrl = false;
            if (getUrlVars().key == "ADD") {
                page.tabs[1].SetEnabled(true);
            }
            $('#CustModel').on('shown.bs.modal', function () {
                $('#txtCustSearch').focus();
            })
            $('#CustModel').on('hide.bs.modal', function () {
                ctxtCustName.Focus();
            })

            document.onkeydown = function (e) {
                if (event.keyCode == 83 && event.altKey == true) {
                    // StopDefaultAction(e);
                    SaveButtonClickNew();//........Alt+N
                }
                else if (event.keyCode == 88 && event.altKey == true) {
                    SaveButtonClick();//........Alt+X
                }
                else if (event.keyCode == 85 && event.altKey == true) {
                    OpenUdf();
                }
                else if (event.keyCode == 84 && event.altKey == true) { //Tax Charges (T)
                    Save_TaxesClick();
                }
                else if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+O -- ie, Billing/Shipping Samrat!     
                    StopDefaultAction(e);
                    if (page.GetActiveTabIndex() == 1) {
                        fnSaveBillingShipping();
                    }
                }
            }
            $("#openlink").on("click", function () {
                //window.open('../master/Contact_general.aspx?id=ADD', '_blank');
                AddcustomerClick();
            });
        });
        function AllControlInitilize() {

            if (canCallBack) {
                IsPageLoad = true;
                canCallBack = false;
            }
            var urlKeys = getUrlVars();
            if (urlKeys.key != 'ADD') {
                // grid.Refresh();
            }

        }
        function selectValue() {
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            if (type == 'S') {
                $('#Multipletype').hide();
                $('#singletype').show();
                $('#tdCashBankLabel').show();
            }
            else if (type == 'M') {

                $('#tdCashBankLabel').hide();

                $('#Multipletype').show();
                $('#singletype').hide();
            }
        }
        function Type_EndCallback() {
            //cCmbScheme.Focus();

        }
        function InstrumentDate_GotFocus() {
            cInstDate.ShowDropDown();
        }
        function InstrumentType_GotFocus() {
            cComboInstrumentTypee.ShowDropDown();
        }
        function CashBank_GotFocus() {
            cddlCashBank.ShowDropDown();
        }
        function Customer_GotFocus() {
            gridLookup.ShowDropDown();
        }
        function TransDate_GotFocus() {
            cdtTDate.ShowDropDown();
        }
        function NumberingScheme_GotFocus() {
            // cCmbScheme.ShowDropDown();
        }
        function CurrencyGotFocus() {
            cCmbCurrency.ShowDropDown();
        }

        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=CRP&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        function acbpCrpUdfEndCall(s, e) {
            if (cacbpCrpUdf.cpUDF) {
                if (cacbpCrpUdf.cpUDF == "true") {
                    cacbpCrpUdf.cpUDF = null;
                    SelectAllData();
                    grid.batchEditApi.StartEdit(-1, 5);
                    grid.batchEditApi.StartEdit(0, 5);

                    grid.AddNewRow();
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
                var comboitem = cComboInstrumentTypee.FindItemByValue('CRD');
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
                var comboitem = cComboInstrumentTypee.FindItemByValue('CRD');
                if (comboitem == undefined || comboitem == null) {
                    cComboInstrumentTypee.AddItem("Card", "CRD");
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

            var VoucherAmount = document.getElementById('hdnPageStatus').value;
            if (VoucherAmount == 'update') {

            }
            else {
                var salesInvoice = document.getElementById('hdnSalesInvoice').value;
                var vouchAmt = parseFloat(ctxtVoucherAmount.GetValue());
                var totPay = parseFloat(ctxtTotalPayment.GetValue());
                var totDebit = c_txt_Debit.GetValue();
                var totalRunningValue = "0.00";
                if (totDebit == "0.0" && totDebit == "0" && totDebit == "0.00") {
                    var totalRunningValue = vouchAmt - totPay;
                }

                if (salesInvoice == "Yes") {
                    clblRunningBalanceCapsul.SetValue(totalRunningValue);
                    jConfirm('Wish to auto adjust amount with sale Invoice(s)?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cPopup_invoice.Show();
                            var amount = ctxtVoucherAmount.GetValue();
                            var customerval = GetObjectID('hdnCustomerId').value;
                            //var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";
                           <%-- $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);--%>
                            cgrid_SalesInvoice.PerformCallback('BindSalesInvoiceDetails' + '~' + amount);
                        }
                        else {
                            grid.batchEditApi.StartEdit(-1, 1);
                        }
                    });
                }
            }
            var amount = ctxtVoucherAmount.GetValue();
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
            //var Mode = document.getElementById('hdn_Mode').value;
            //if (Mode != 'Edit')
            //{

            //}

            //..................CheckAmount.......................
            var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            if (Type != 'Advance') {
                var DocumentNo = (grid.GetEditor('DocumentID').GetValue() != null) ? grid.GetEditor('DocumentID').GetValue() : "0";
                var IsOpening = (grid.GetEditor('IsOpening').GetValue() != null) ? grid.GetEditor('IsOpening').GetValue() : "0";
                if (PaymentOldValue != PaymentValue) {
                    cacpCheckAmount.PerformCallback(Type + '~' + DocumentNo + '~' + IsOpening);
                }


            }

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
            var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            if (Type == 'Advance') {
                ctxtVoucherAmount.SetValue(parseFloat(CurrentSum - newDif));
            }
            if (Type == 'OnAccount') {
                ctxtVoucherAmount.SetValue(parseFloat(CurrentSum - newDif));
            }
            var VoucherAmount = ctxtVoucherAmount.GetValue();
            // console.log('VoucherAmount' + VoucherAmount);
            var Debit = c_txt_Debit.GetValue();
            if (VoucherAmount != '0.00') {
                clblRunningBalanceCapsul.SetValue(parseFloat(VoucherAmount - Debit));
            }


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

            var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            if (Type == 'Advance') {
                ctxtVoucherAmount.SetValue(parseFloat(CurrentSum - newDif));
            }
            if (Type == 'OnAccount') {
                ctxtVoucherAmount.SetValue(parseFloat(CurrentSum - newDif));
            }
            var VoucherAmount = ctxtVoucherAmount.GetValue();
            var PaymentAmt = ctxtTotalPayment.GetValue();
            if (VoucherAmount != '0.00') {
                clblRunningBalanceCapsul.SetValue(parseFloat(VoucherAmount - PaymentAmt));
            }

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
        function lostFocusVoucherAmount(s, e) {
            var totPayment = ctxtTotalPayment.GetValue();
            var totDebit = c_txt_Debit.GetValue();
            var RunningBalance = clblRunningBalanceCapsul.GetValue();
            var VoucherAmount = ctxtVoucherAmount.GetValue();
            if (totPayment != "0.0" && totPayment != "0" && totPayment != "0.00") {
                clblRunningBalanceCapsul.SetValue(parseFloat(VoucherAmount) - parseFloat(totPayment));
            }
            else {
                clblRunningBalanceCapsul.SetValue(parseFloat(VoucherAmount) - parseFloat(totDebit));
            }

        }
        function ReceiptTextChange(s, e) {
            ReceiptLostFocus(s, e);
            var RecieveValue = (grid.GetEditor('Receipt').GetValue() != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";

            var receiptValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0";

            if (RecieveValue > 0) {
                recalculatePayment(grid.GetEditor('Payment').GetValue());
                grid.GetEditor('Payment').SetValue("0");
            }

            //..................CheckAmount.......................
            var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            if (Type != 'Advance' && Type != '0') {
                var DocumentNo = (grid.GetEditor('DocumentID').GetValue() != null) ? grid.GetEditor('DocumentID').GetValue() : "0";
                var IsOpening = (grid.GetEditor('IsOpening').GetValue() != null) ? grid.GetEditor('IsOpening').GetValue() : "0";
                if (ReciptOldValue != RecieveValue) {
                    cacpCheckAmount.PerformCallback(Type + '~' + DocumentNo + '~' + IsOpening);
                }
            }

            //.................End.........................
        }
        function aacpCheckAmountEndCall(s, e) {
            if (cacpCheckAmount.cpUnPaidAmount) {
                //if (cacpCheckAmount.cpUnPaidAmount != null && cacpCheckAmount.cpUnPaidAmount !="0.0000") {
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
                                //grid.GetEditor('Receipt').SetValue(UnPaidAmoun);
                                var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());

                                //ctxtVoucherAmount.SetValue(VoucherAmount - finalValue);
                                // c_txt_Debit.SetValue(VoucherAmount - finalValue);
                            }
                            else {
                                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                                var newValue = grid.GetEditor('Payment').GetValue();
                                var finalValue = newValue - UnPaidAmoun;
                                // grid.GetEditor('Payment').SetValue(UnPaidAmoun);
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

        //..........Save & New.....
        function SaveButtonClickNew() {
            cLoadingPanelCRP.Show();
            $('#<%=hdnBtnClick.ClientID %>').val("Save_New");
            $('#<%=hdnRefreshType.ClientID %>').val('N');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            if (document.getElementById('<%= txtVoucherNo.ClientID %>').value.trim() == "") {
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
            // var customerId = cCustomerComboBox.GetValue();
            if (customerId == '' || customerId == null) {
                cLoadingPanelCRP.Hide();
                $('#MandatorysCustomer').attr('style', 'display:block');
                return false;
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            var CashBank = cddlCashBank.GetValue();
            var VoucherType = document.getElementById("ComboVoucherType").value;
            if (type == 'S') {
                if (CashBank == null) {
                    cLoadingPanelCRP.Hide();
                    $("#MandatoryCashBank").show();
                    return false;
                }
                var comboitem = cComboInstrumentTypee.GetValue('CH');
                var VoucherAmount = ctxtVoucherAmount.GetValue();
                if (VoucherType == 'R') {
                    if (comboitem == 'CH') {
                        var EnteredCashAmount = parseFloat(VoucherAmount);
                        if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                            jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                            cLoadingPanelCRP.Hide();
                            return false;
                        }
                    }
                }
            }
            var VoucherAmount = ctxtVoucherAmount.GetValue();
            if (VoucherAmount == "0.0") {
                cLoadingPanelCRP.Hide();
                jAlert("Voucher amount must be greater then ZERO.");
                return false;
            }
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            if (type == 'M') {
                var totalAmount = GetPaymentTotalEnteredAmount();
                var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());
                if (totalAmount != VoucherAmount) {
                    cLoadingPanelCRP.Hide();
                    jAlert("Voucher amount And Multiple Payment amount must be Same.");
                    return false;
                }
                if (VoucherType == 'R') {
                    var EnteredCashAmount = parseFloat($('#cmbUcpaymentCashLedgerAmt').val());
                    if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                        cLoadingPanelCRP.Hide();
                        jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                        return false;
                    }
                }
            }
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            if (type == 'M') {
                var retValueonpayDet;
                if (document.getElementById('PaymentTable')) {
                    var table = document.getElementById('PaymentTable');
                    if (table.rows[table.rows.length - 1]) {
                        if (table.rows[table.rows.length - 1].children[0].children[1].value != "-Select-") {
                            retValueonpayDet = validatePaymentDetails(table.rows[table.rows.length - 1]);
                            if (!retValueonpayDet) {
                                cLoadingPanelCRP.Hide();
                                return false;
                            }
                        }
                    }
                }
            }
            //Code added by Sudip
            grid.batchEditApi.EndEdit();
            var gridCount = grid.GetVisibleRowsOnPage();
            var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
            var txtTotalPayment = ctxtTotalPayment.GetValue() != null ? ctxtTotalPayment.GetValue() : 0;
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
            grid.batchEditApi.StartEdit(-1, 5);
            grid.batchEditApi.StartEdit(0, 5);
            if (VoucherType == "R") {
                var TypeValue = (grid.GetEditor('Type').GetText() != null) ? grid.GetEditor('Type').GetText() : "0";
                if (TypeValue == "Advance") {
                    var ShippingStateCode = $("#bsSCmbStateHF").val();
                    var BranchStateID = "";

                    if ($("#CB_GSTApplicable").is(':checked')) {                   
                        $.ajax({
                            type: "POST",
                            url: 'CustomerReceiptPayment.aspx/getBranchStateByID',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: "{BranchID:\"" + branch + "\"}",
                            success: function (type) {
                                BranchStateID = type.d;
                                if (ShippingStateCode != BranchStateID) {

                                    var Order_Msg = "Branch address and Customer shipping address is different. Do you want proceed with IGST calculation?";
                                    var msg = confirm(Order_Msg);
                                    if (msg == true) {

                                        if (gridCount > 0) {
                                            if (IsType == "Y") {
                                                var customerval = GetObjectID('hdnCustomerId').value

                                                if (VoucherType == "R") {
                                                    if (txtTotalAmount >= txtTotalPayment) {
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
                                    else {
                                        $("#CB_GSTApplicable").prop("checked", false);
                                        cLoadingPanelCRP.Hide();
                                        return;
                                    }
                                }
                                else {
                                    if (gridCount > 0) {
                                        if (IsType == "Y") {
                                            var customerval = GetObjectID('hdnCustomerId').value;

                                            if (VoucherType == "R") {
                                                if (txtTotalAmount >= txtTotalPayment) {
                                                    cacbpCrpUdf.PerformCallback();
                                                }
                                                else {
                                                    jAlert('Receipt amount can not be less than payment amount');
                                                    cLoadingPanelCRP.Hide();
                                                }
                                            }
                                        }
                                        else {
                                            jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                                            cLoadingPanelCRP.Hide();
                                        }
                                    }
                                    else {
                                        jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                                        cLoadingPanelCRP.Hide();
                                    }
                                }
                            }
                        });
                    }
                    else {
                        if (gridCount > 0) {
                            if (IsType == "Y") {
                                var customerval = GetObjectID('hdnCustomerId').value

                                if (VoucherType == "R") {
                                    if (txtTotalAmount >= txtTotalPayment) {
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


                }
                else {
                    if (gridCount > 0) {
                        if (IsType == "Y") {
                            var customerval = GetObjectID('hdnCustomerId').value

                            if (VoucherType == "R") {
                                if (txtTotalAmount >= txtTotalPayment) {
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
            }
            else {
                if (gridCount > 0) {
                    if (IsType == "Y") {
                        var customerval = GetObjectID('hdnCustomerId').value
                        if (VoucherType == "P") {
                            if (txtTotalAmount <= txtTotalPayment) {
                                cacbpCrpUdf.PerformCallback();
                            }
                            else {
                                cLoadingPanelCRP.Hide();
                                jAlert('Payment amount can not be less than receipt amount ');
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
        }
        function SaveButtonClick() {
            cLoadingPanelCRP.Show();
            $('#<%=hdnBtnClick.ClientID %>').val("Save_Exit");
            $('#<%=hdnRefreshType.ClientID %>').val('E');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            if (document.getElementById('<%= txtVoucherNo.ClientID %>').value.trim() == "") {
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
            var customerId = GetObjectID('hdnCustomerId').value;
            // var customerId = cCustomerComboBox.GetValue();
            if (customerId == '' || customerId == null) {
                cLoadingPanelCRP.Hide();
                $('#MandatorysCustomer').attr('style', 'display:block');
                return false;
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            var CashBank = cddlCashBank.GetValue();
            var VoucherType = document.getElementById("ComboVoucherType").value;
            var VoucherAmount = ctxtVoucherAmount.GetValue();
            if (type == 'S') {
                if (CashBank == null) {
                    cLoadingPanelCRP.Hide();
                    $("#MandatoryCashBank").show();
                    return false;
                }
                var comboitem = cComboInstrumentTypee.GetValue('CH');
                if (VoucherType == 'R') {
                    if (comboitem == 'CH') {
                        var EnteredCashAmount = parseFloat(VoucherAmount);
                        if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                            jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                            cLoadingPanelCRP.Hide();
                            return false;
                        }
                    }
                }
            }

            if (VoucherAmount == "0.0") {
                // $("#MandatoryVoucherAmount").show();
                jAlert("Voucher amount must be greater then ZERO.");
                cLoadingPanelCRP.Hide();
                return false;
            }
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            if (type == 'M') {
                var totalAmount = GetPaymentTotalEnteredAmount();
                var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());
                if (totalAmount != VoucherAmount) {
                    jAlert("Voucher amount And Multiple Payment amount must be Same.");
                    cLoadingPanelCRP.Hide();
                    return false;
                }
                if (VoucherType == 'R') {
                    var EnteredCashAmount = parseFloat($('#cmbUcpaymentCashLedgerAmt').val());
                    if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                        jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                        cLoadingPanelCRP.Hide();
                        return false;
                    }
                }
            }
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            if (type == 'M') {
                var retValueonpayDet;
                if (document.getElementById('PaymentTable')) {
                    var table = document.getElementById('PaymentTable');
                    if (table.rows[table.rows.length - 1]) {
                        if (table.rows[table.rows.length - 1].children[0].children[1].value != "-Select-") {
                            retValueonpayDet = validatePaymentDetails(table.rows[table.rows.length - 1]);
                            if (!retValueonpayDet) {
                                cLoadingPanelCRP.Hide();
                                return false;
                            }
                        }
                    }
                }
            }
            //Code added by Sudip
            grid.batchEditApi.EndEdit();
            var gridCount = grid.GetVisibleRowsOnPage();
            var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
            var txtTotalPayment = ctxtTotalPayment.GetValue() != null ? ctxtTotalPayment.GetValue() : 0;

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
            grid.batchEditApi.StartEdit(-1, 5);
            grid.batchEditApi.StartEdit(0, 5);

      
            if (VoucherType == "R") {
                var TypeValue = (grid.GetEditor('Type').GetText() != null) ? grid.GetEditor('Type').GetText() : "0";
                if (TypeValue == "Advance") {
                    var ShippingStateCode = $("#bsSCmbStateHF").val();
                    var BranchStateID = "";

                    if ($("#CB_GSTApplicable").is(':checked')) {
                        $.ajax({
                            type: "POST",
                            url: 'CustomerReceiptPayment.aspx/getBranchStateByID',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: "{BranchID:\"" + branch + "\"}",
                            success: function (type) {
                                BranchStateID = type.d;
                                if (ShippingStateCode != BranchStateID) {

                                    var Order_Msg = "Branch address and Customer shipping address is different. Do you want proceed with IGST calculation?";
                                    var msg = confirm(Order_Msg);
                                    if (msg == true) {
                                        if (gridCount > 0) {
                                            if (IsType == "Y") {
                                                var customerval = GetObjectID('hdnCustomerId').value;

                                                if (VoucherType == "R") {
                                                    if (txtTotalAmount >= txtTotalPayment) {
                                                        cacbpCrpUdf.PerformCallback();
                                                    }
                                                    else {
                                                        jAlert('Receipt amount can not be less than payment amount');
                                                        cLoadingPanelCRP.Hide();
                                                    }
                                                }
                                            }
                                            else {
                                                jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                                                cLoadingPanelCRP.Hide();
                                            }
                                        }
                                        else {
                                            jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                                            cLoadingPanelCRP.Hide();
                                        }
                                    }
                                    else {
                                        $("#CB_GSTApplicable").prop("checked", false);
                                        cLoadingPanelCRP.Hide();
                                        return;
                                    }
                                }
                                else
                                {
                                    if (gridCount > 0) {
                                        if (IsType == "Y") {
                                            var customerval = GetObjectID('hdnCustomerId').value;

                                            if (VoucherType == "R") {
                                                if (txtTotalAmount >= txtTotalPayment) {
                                                    cacbpCrpUdf.PerformCallback();
                                                }
                                                else {
                                                    jAlert('Receipt amount can not be less than payment amount');
                                                    cLoadingPanelCRP.Hide();
                                                }
                                            }
                                        }
                                        else {
                                            jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                                            cLoadingPanelCRP.Hide();
                                        }
                                    }
                                    else {
                                        jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                                        cLoadingPanelCRP.Hide();
                                    }
                                }
                            }
                        });
                    }
                    else {
                        if (gridCount > 0) {
                            if (IsType == "Y") {
                                var customerval = GetObjectID('hdnCustomerId').value;

                                if (VoucherType == "R") {
                                    if (txtTotalAmount >= txtTotalPayment) {
                                        cacbpCrpUdf.PerformCallback();
                                    }
                                    else {
                                        jAlert('Receipt amount can not be less than payment amount');
                                        cLoadingPanelCRP.Hide();
                                    }
                                }
                            }
                            else {
                                jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                                cLoadingPanelCRP.Hide();
                            }
                        }
                        else {
                            jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                            cLoadingPanelCRP.Hide();
                        }
                    }

                }
                else {
                    if (gridCount > 0) {
                        if (IsType == "Y") {
                            var customerval = GetObjectID('hdnCustomerId').value;

                            if (VoucherType == "R") {
                                if (txtTotalAmount >= txtTotalPayment) {
                                    cacbpCrpUdf.PerformCallback();
                                }
                                else {
                                    jAlert('Receipt amount can not be less than payment amount');
                                    cLoadingPanelCRP.Hide();
                                }
                            }
                        }
                        else {
                            jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                            cLoadingPanelCRP.Hide();
                        }
                    }
                    else {
                        jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                        cLoadingPanelCRP.Hide();
                    }
                }
            }
            else {
                if (gridCount > 0) {
                    if (IsType == "Y") {
                        var customerval = GetObjectID('hdnCustomerId').value;
                        if (VoucherType == "P") {
                            if (txtTotalAmount <= txtTotalPayment) {

                                cacbpCrpUdf.PerformCallback();
                            }
                            else {
                                jAlert('Payment amount can not be less than receipt amount ');
                                cLoadingPanelCRP.Hide();
                            }
                        }
                    }
                    else {
                        jAlert('Cannot Save. You must enter atleast one Record to save this entry.');
                        cLoadingPanelCRP.Hide();
                    }
                }

            }
        }


        //..................Batch Grid.....................
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }
        function grid_SalesInvoiceOnEndCallback(s, e) {
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
                var IsTagged = document.getElementById('IsInvoiceTagged').value;

                if (IsTagged != "Y") {
                    if (grid.GetVisibleRowsOnPage() > 1) {
                        grid.batchEditApi.StartEdit(e.visibleIndex, 1);
                        var ReceiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0.0";
                        var PaymentValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0.0";
                        /// grid.DeleteRow(e.visibleIndex);
                        var noofvisiblerows = grid.GetVisibleRowsOnPage();
                        if (noofvisiblerows != "1") {
                            var VoucherType = document.getElementById("ComboVoucherType").value;
                            if (VoucherType == 'P') {
                                RemoveTypeAll();
                                PropolateTypeByPayment();
                            }
                            grid.DeleteRow(e.visibleIndex);
                            $('#<%=hdfIsDelete.ClientID %>').val('D');
                            grid.UpdateEdit();
                            grid.PerformCallback('Display');
                            $('#<%=hdnPageStatus.ClientID %>').val('delete');
                        }
                        var hdnValue = document.getElementById('hdndocumentno').value;
                        var DocumentID = (grid.GetEditor('DocumentID').GetValue() != null) ? grid.GetEditor('DocumentID').GetValue() : "0";
                        //document.getElementById('hdndocumentno').value = (',' + hdnValue).replace(',' + DocumentID + ',' + /,/g, '');
                        document.getElementById('hdndocumentno').value = (',' + hdnValue).replace(',' + DocumentID + ',', '');
                        var RunningBalanceCapsul = clblRunningBalanceCapsul.GetValue();
                        if (ReceiptValue != "0.00") {
                            var VoucherAmount = ctxtVoucherAmount.GetValue();
                            if (VoucherAmount != "0.0" && VoucherAmount != "0") {
                                clblRunningBalanceCapsul.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                            }
                            else {
                                //ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                                c_txt_Debit.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                                //clblRunningBalanceCapsul.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                                clblRunningBalanceCapsul.SetValue(parseFloat(RunningBalanceCapsul) + parseFloat(ReceiptValue));
                            }

                        }
                        if (PaymentValue != "0.00") {
                            var VoucherAmount = ctxtVoucherAmount.GetValue();
                            ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue));
                            ctxtTotalPayment.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue));
                            clblRunningBalanceCapsul.SetValue(parseFloat(RunningBalanceCapsul) + parseFloat(PaymentValue));
                        }
                    }
                }
                else {
                    jAlert("Can not Delete Invoice.");
                }

            }
            if (e.buttonID == 'AddNew') {
                var TypeValue = (grid.GetEditor('Type').GetText() != null) ? grid.GetEditor('Type').GetText() : "0";
                if (TypeValue != "") {
                    if (!$("#CB_GSTApplicable").is(':checked')) {
                        OnAddNewClick();
                    }
                    else {
                        if (TypeValue == 'Advance') {
                            var Receipttype = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
                            if (Receipttype != 'M') {

                                // OnAddNewClick();
                            }
                        }
                        else {
                            OnAddNewClick();
                        }
                    }


                }
                else {
                    grid.batchEditApi.StartEdit(e.visibleIndex, 1);
                }
            }
        }

        function BtnVisible() {
            document.getElementById('btnSaveNew').style.display = 'none'
            document.getElementById('btnSaveRecords').style.display = 'none'
            document.getElementById('tagged').style.display = 'block'

        }

        function OnEndCallback(s, e) {
            //if (IsPageLoad) {
            //    IsPageLoad = false;
            //    document.getElementById('ComboVoucherType').style.display = 'inline-block';
            //    cLoadingPanelCRP.Hide();
            //}
            if (grid.cpBtnVisible != null && grid.cpBtnVisible != "") {
                grid.cpBtnVisible = null;
                BtnVisible();
            }
            if (grid.cpMulType != null && grid.cpMulType != "") {
                grid.cpMulType = null;
                $('#<%=rdl_MultipleType.ClientID %>').find("input[value='M']").prop("checked", true);
                $('#tdCashBankLabel').hide();
                $('#Multipletype').show();
                $('#singletype').hide();

            }
            if (grid.cpTotalAmount != null) {
                var total_receipt = grid.cpTotalAmount.split('~')[0];
                var total_payment = grid.cpTotalAmount.split('~')[1];
                c_txt_Debit.SetValue(total_receipt);
                ctxtTotalPayment.SetValue(total_payment);
                grid.cpTotalAmount = null;
            }
            var value = document.getElementById('hdnRefreshType').value;
            var pageStatus = document.getElementById('hdnPageStatus').value;
            var IsInvoiceTagged = document.getElementById('IsInvoiceTagged').value;
            var InvoicepageStatus = document.getElementById('hdnInvoicePageStatus').value;

            if (grid.cpSaveSuccessOrFail == "outrange") {
                OnAddNewClick();
                grid.batchEditApi.StartEdit(0, 2);
                cLoadingPanelCRP.Hide();
                jAlert('Can Not Add More Voucher Number as Voucher Scheme Exausted.<br />Update The Scheme and Try Again');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                OnAddNewClick();
                grid.batchEditApi.StartEdit(0, 2);
                cLoadingPanelCRP.Hide();
                jAlert('Can Not Save as Duplicate Voucher Number.');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                grid.batchEditApi.StartEdit(0);
                cLoadingPanelCRP.Hide();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please fill Document');
            }
            else if (grid.cpSaveSuccessOrFail == "nullType") {
                grid.batchEditApi.StartEdit(0);
                cLoadingPanelCRP.Hide();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please Select Type');
            }
            else if (grid.cpSaveSuccessOrFail == "nullReceiptPayment") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0);

                grid.cpSaveSuccessOrFail = null;
                jAlert('Please enter Amount to save this entry.');
            }
            else if (grid.cpSaveSuccessOrFail == "NOGSTApplicable") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0);

                grid.cpSaveSuccessOrFail = null;
                jAlert('Cannot save by selecting GST Applicable. Please uncheck to proceed.');
            }
            else if (grid.cpSaveSuccessOrFail == "NotMatchVoucherAmount") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Mismatch detected in Voucher amount & Total amount.');
            }

            else if (grid.cpSaveSuccessOrFail == "duplicateDocument") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can not Select duplicate document in List.');
            }
            else if (grid.cpSaveSuccessOrFail == "ProductMandatory") {
                grid.AddNewRow();
                grid.cpSaveSuccessOrFail = null;
                cLoadingPanelCRP.Hide();
                jAlert('Please Select product to calculate GST for Advance.');
            }
            else if (grid.cpSaveSuccessOrFail == "BSMandatory") {
                grid.batchEditApi.StartEdit(0);
                grid.cpSaveSuccessOrFail = null;
                cLoadingPanelCRP.Hide();
                jAlert('Billing/Shipping is mandatory', "Alert", function () {
                    page.SetActiveTabIndex(1);
                    cbsSave_BillingShipping.Focus();
                    page.tabs[0].SetEnabled(false);
                    $("#divcross").hide();
                });

            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                grid.batchEditApi.StartEdit(0, 2);
                cLoadingPanelCRP.Hide();
                jAlert('Please try after sometime.');
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
            }
            else {
                var Voucher_Number = grid.cpVouvherNo;
                var Order_Msg = "Customer Receipt/Payment No. " + Voucher_Number + " saved.";

                if (value == "E") {
                    if (Voucher_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [CustomerReceipt/Payment]', function (r) {
                            if (r == true) {
                                if (IsInvoiceTagged == "Y") {
                                    if (InvoicepageStatus == "E") {
                                        parent.CRP_SaveANDExit_Press();
                                    }
                                    else if (InvoicepageStatus == "N") {
                                        parent.CRP_SaveANDNew_Press();
                                    }
                                    else {
                                        window.parent.capcReciptPopup.Hide();
                                        //window.location.assign("CustomerReceiptPaymentList.aspx");
                                    }
                                }
                                else {
                                    grid.cpVouvherNo = null;
                                    window.location.assign("CustomerReceiptPaymentList.aspx");
                                }
                            }
                        });
                    }
                    else {
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
                            }
                        });
                    }
                    else {
                        ctxtVoucherAmount.SetValue("0.0");
                        c_txt_Debit.SetValue("0.0");
                        window.location.assign("CustomerReceiptPayment.aspx?key=ADD");
                    }
                }
                else {
                    if (pageStatus == "first") {
                        OnAddNewClick();
                        VisibleColumn();
                        grid.batchEditApi.EndEdit();
                        c_txt_Debit.SetValue("0.0");
                        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('ComboVoucherType').style.display = 'Block'
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
                            ctxtRate.SetEnabled(false);
                        }
                        if (document.getElementById('isFromTab').value == "1") {
                            grid.batchEditApi.StartEdit(-1);
                            grid.GetEditor("Type").SetValue("Advance");
                            var basketAmount = document.getElementById('HdBasketTotalAmount').value;
                            grid.GetEditor("Receipt").SetValue(basketAmount);
                            Type.ClearItems();
                            Type.AddItem("Advance", "Advance");
                        }
                    }
                    else if (pageStatus == "update") {
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
                            ctxtRate.SetEnabled(false);
                        }

                        var frontRow = 0;
                        //var backRow = -1;

                        for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                            // var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Type')) : "";
                            var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Type')) : "";
                            if (backProduct != "Advance" && backProduct != "") {
                                DateChange = "Y";
                                break;
                            }
                            //backRow--;
                            frontRow++;
                        }
                    }
                    else if (pageStatus == "tagfirst") {
                        OnAddNewClick();
                        VisibleColumn();
                        grid.batchEditApi.EndEdit();

                        grid.GetEditor('DocumentNo').SetEnabled(false);
                        grid.GetEditor('Type').SetEnabled(false);

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
                                ctxtRate.SetEnabled(false);
                            }
                        }
                        else if (pageStatus == "delete") {
                           // OnAddNewClick();
                            $('#<%=hdnPageStatus.ClientID %>').val('');
                            var VoucherType = document.getElementById("ComboVoucherType").value;
                            if (VoucherType == 'P') {
                                RemoveTypeAll();
                                PropolateTypeByPayment();
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
    //cCustomerComboBox.SetEnabled(false);
    ctxtCustName.SetEnabled(false);
    //gridLookup.SetEnabled(false);
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

    var urlKeys = getUrlVars();
    if (urlKeys.key != 'ADD') {

        var VoucherType = document.getElementById("ComboVoucherType").value;
        if (VoucherType == "P") {
           // var customerval = GetObjectID('hdnCustomerId').value;
           // LoadCustomerAddress(customerval, $('#ddlBranch').val(), 'CRP');
            //page.GetTabByName('Billing/Shipping').SetEnabled(false);
        }
        if (document.getElementById('Multipletype').style.display == "block") {
            $('#<%=rdl_MultipleType.ClientID %>').find("input[value='M']").prop("checked", true);
            $('#tdCashBankLabel').hide();
            $('#Multipletype').show();
            $('#singletype').hide();
        }
        if (VoucherType == "P") {
            RemoveTypeAll();
            PropolateTypeByPayment();
            $('#ProductSection').hide();
            $('#ProductGSTApplicableSection').hide();
            // $("#CB_GSTApplicable").prop("checked", false);
            grid.GetEditor('Receipt').SetEnabled(false);
            grid.GetEditor('Payment').SetEnabled(true);
            page.GetTabByName('Billing/Shipping').SetEnabled(true);
        }
        else {
            RemoveTypeAll();
            PropolateTypeByReceipt();
            $('#ProductSection').show();
            // $("#CB_GSTApplicable").prop("checked", true);
            $('#ProductGSTApplicableSection').show();
            $('#multipleredio').show();
            grid.GetEditor('Payment').SetEnabled(false);
            grid.GetEditor('Receipt').SetEnabled(true);
            //$('#<%=rdl_MultipleType.ClientID %>').find("input[value='S']").prop("checked", true);
            page.GetTabByName('Billing/Shipping').SetEnabled(true);
        }
        if ($("#CB_GSTApplicable").is(':checked')) {
            cproductLookUp.SetEnabled(true);
        }
        else {
            cproductLookUp.SetEnabled(false);
            return;
        }
    }
    else {
        var VoucherType = document.getElementById("ComboVoucherType").value;
        if (VoucherType == "P") {
            RemoveTypeAll();
            PropolateTypeByPayment();
            $('#ProductSection').hide();
            $('#ProductGSTApplicableSection').hide();
            $("#CB_GSTApplicable").prop("checked", false);
            grid.GetEditor('Receipt').SetEnabled(false);
            grid.GetEditor('Payment').SetEnabled(true);
            page.GetTabByName('Billing/Shipping').SetEnabled(true);
        }
        else {
            RemoveTypeAll();
            PropolateTypeByReceipt();
            $('#ProductSection').show();
            // $("#CB_GSTApplicable").prop("checked", true);
            $('#ProductGSTApplicableSection').show();
            $('#multipleredio').show();
            grid.GetEditor('Payment').SetEnabled(false);
            grid.GetEditor('Receipt').SetEnabled(true);
            $('#<%=rdl_MultipleType.ClientID %>').find("input[value='S']").prop("checked", true);
            page.GetTabByName('Billing/Shipping').SetEnabled(true);
        }
        cproductLookUp.SetEnabled(false);

    }
    cLoadingPanelCRP.Hide();
    grid.batchEditApi.EndEdit();
    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;


    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
    var basedCurrency = LocalCurrency.split("~");
    if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
        ctxtRate.SetEnabled(false);
    }
    if (document.getElementById('isFromTab').value == "1") {
        grid.batchEditApi.StartEdit(-1);
        grid.GetEditor("Type").SetValue("Advance");
        var basketAmount = document.getElementById('HdBasketTotalAmount').value;
        grid.GetEditor("Receipt").SetValue(basketAmount);
        Type.ClearItems();
        Type.AddItem("Advance", "Advance");
    }


}
//...............End Batch Grid..................
function changeFocusDate() {

    if (DateChange == "Y") {
        jAlert('Cannot change the date as tagged document date is after the date you have entered now.', 'Alert', function () {

            var TransDate = document.getElementById('hdnDate').value;
            var strDate = new Date(TransDate);

            cdtTDate.SetDate(strDate);
            //GetContactPerson();
        });
    }

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

        document.getElementById('ddlBranch').value = branchID;
        document.getElementById('ddlEnterBranch').value = branchID;
        document.getElementById('HdSelectedBranch').value = branchID;
        loadMainAccountByBranchIdForPayDet(branchID);
        var IsTagged = document.getElementById('hdnTaggedFrom').value;
        if (IsTagged != "I") {
            cddlCashBank.PerformCallback(branchID);
        }
    }
    var VoucherType = document.getElementById("ComboVoucherType").value;
    if (VoucherType == "R") {
        if (Type == "Advance") {
            RemoveTypeAll();
            PropolateTypeByReceiptProduct();
        }
        else if (Type == "None" || Type == "NA") {
            RemoveTypeAll();
            PropolateTypeByReceipt();

        }
        else if (Type == "NonAdvance") {
            RemoveTypeAll();

            PropolateTypeByReceiptNonAdvance();

        }
        else if (Type == "OnAccount") {
            RemoveTypeAll();
            PropolateTypeByOnAccount();
        }

    }

    $('#txtVoucherNo').attr('maxLength', schemelength);

    if (schemetype == '0') {
        $('#<%=hdnSchemaType.ClientID %>').val('0');
                            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
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




   <%-- $.ajax({
        type: "POST",
        url: 'CustomerReceiptPayment.aspx/getSchemeType',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{sel_scheme_id:\"" + val + "\"}",
        success: function (type) {
            var schemetypeValue = type.d;
            var schemetype = schemetypeValue.toString().split('~')[0];
            var schemelength = schemetypeValue.toString().split('~')[1];
            var branchID = schemetypeValue.toString().split('~')[2];
            var Type = schemetypeValue.toString().split('~')[3];
            if (schemetypeValue != "") {
                document.getElementById('ddlBranch').value = branchID;
                document.getElementById('ddlEnterBranch').value = branchID;
                document.getElementById('HdSelectedBranch').value = branchID;             

                var IsTagged = document.getElementById('hdnTaggedFrom').value;
                if (IsTagged != "I") {
                    cddlCashBank.PerformCallback(branchID);
                }
            }
            var VoucherType = document.getElementById("ComboVoucherType").value;
            if (VoucherType == "R") {
                if (Type == "Advance") {
                    RemoveTypeAll();
                    PropolateTypeByReceiptProduct();
                }
                else if (Type == "None" || Type == "") {
                    RemoveTypeAll();
                    PropolateTypeByReceipt();

                }
                else if (Type == "NonAdvance") {
                    RemoveTypeAll();

                    PropolateTypeByReceiptNonAdvance();

                }
                else if (Type == "OnAccount")
                {
                     RemoveTypeAll();
                    PropolateTypeByOnAccount();
                }

            }

            $('#txtVoucherNo').attr('maxLength', schemelength);

            if (schemetype == '0') {
                $('#<%=hdnSchemaType.ClientID %>').val('0');
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";               
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
    });--%>
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
        function PropolateTypeByPayment() {
            Type.AddItem("Credit Note", "CustCrNote");
            Type.AddItem("Advance Receipt", "AdvancePayment");
            Type.AddItem("On Account", "OnAccountPay");
        }
        function RemoveTypeByPayment() {
            var comboitem = Type.FindItemByValue('Advance');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('Invoice');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('CustDbNote');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('OnAccount');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
        }
        function RemoveTypeByRemove() {
            var comboitem = Type.FindItemByValue('CustCrNote');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('AdvancePayment');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('OnAccountPay');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }

        }
        function PropolateTypeByReceipt() {
            Type.AddItem("Advance", "Advance");
            Type.AddItem("Invoice", "Invoice");
            Type.AddItem("Debit Note", "CustDbNote");
            Type.AddItem("On Account", "OnAccount");
        }
        function RemoveTypeAll() {
            var comboitem = Type.FindItemByValue('Advance');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('Invoice');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('CustDbNote');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('CustCrNote');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('AdvancePayment');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('OnAccount');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('OnAccountPay');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
        }
        function PropolateTypeByReceiptNonAdvance() {

            Type.AddItem("Invoice", "Invoice");
            Type.AddItem("Debit Note", "CustDbNote");

        }
        function PropolateTypeByReceiptProduct() {
            Type.AddItem("Advance", "Advance");

        }
        function PropolateTypeByOnAccount() {
            Type.AddItem("On Account", "OnAccount");
        }
        function RemoveTypeByReceiptPayment() {

            var comboitem = Type.FindItemByValue('Invoice');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('CustDbNote');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('CustCrNote');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }
            var comboitem = Type.FindItemByValue('AdvancePayment');
            if (comboitem != undefined && comboitem != null) {
                Type.RemoveItem(comboitem.index);
            }

        }
        function CmbSchemeEndCallback() {
            if (lastCRP) {
                cCmbScheme.PerformCallback(lastCRP);
                lastCRP = null;
            }
            After_NumberingSchema();
        }
        function After_NumberingSchema() {
            var VoucherType = document.getElementById("ComboVoucherType").value;
            if (VoucherType == "P") {
                RemoveTypeAll();
                PropolateTypeByPayment();
                //RemoveTypeByPayment();               
                $('#ProductSection').hide();
                $('#ProductGSTApplicableSection').hide();
                $("#CB_GSTApplicable").prop("checked", false);
                // $('#multipleredio').hide();
                // $('#Multipletype').hide();
                grid.GetEditor('Receipt').SetEnabled(false);
                grid.GetEditor('Payment').SetEnabled(true);

            }
            else {
                RemoveTypeAll();
                PropolateTypeByReceipt();
                // RemoveTypeByRemove();               
                $('#ProductSection').show();
                $("#CB_GSTApplicable").prop("checked", true);
                $('#ProductGSTApplicableSection').show();
                $('#multipleredio').show();
                grid.GetEditor('Payment').SetEnabled(false);
                grid.GetEditor('Receipt').SetEnabled(true);
                $('#<%=rdl_MultipleType.ClientID %>').find("input[value='S']").prop("checked", true);
            }
        }
        function rbtnType_SelectedIndexChanged() {
            var IsType = "";
            var frontRow = 0;
            var backRow = -1;

            var VoucherType = document.getElementById("ComboVoucherType").value;
            if (VoucherType == "P") {
                RemoveTypeAll();
                PropolateTypeByPayment();
                //RemoveTypeByPayment();               
                $('#ProductSection').hide();
                $('#ProductGSTApplicableSection').hide();
                $("#CB_GSTApplicable").prop("checked", false);
                // $('#multipleredio').hide();
                // $('#Multipletype').hide();
                grid.GetEditor('Receipt').SetEnabled(false);
                grid.GetEditor('Payment').SetEnabled(true);
                page.GetTabByName('Billing/Shipping').SetEnabled(true);
            }
            else {
                RemoveTypeAll();
                PropolateTypeByReceipt();
                // RemoveTypeByRemove();               
                $('#ProductSection').show();
                $("#CB_GSTApplicable").prop("checked", true);
                $('#ProductGSTApplicableSection').show();
                $('#multipleredio').show();
                grid.GetEditor('Payment').SetEnabled(false);
                grid.GetEditor('Receipt').SetEnabled(true);
                $('#<%=rdl_MultipleType.ClientID %>').find("input[value='S']").prop("checked", true);
                page.GetTabByName('Billing/Shipping').SetEnabled(true);
            }
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
            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
            ctxtVoucherAmount.SetValue("0");
            clblRunningBalanceCapsul.SetValue("0.0");

            //if (cCmbScheme.InCallback()) {
            //    lastCRP = VoucherType;
            //}
            //else {
            //    cCmbScheme.PerformCallback(VoucherType);
            //}
            var OtherDetails = {}
            OtherDetails.VoucherType = document.getElementById("ComboVoucherType").value;

            $.ajax({
                type: "POST",
                url: "CustomerReceiptPayment.aspx/GetNumberingSchemeByType",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;

                    if (returnObject.numberingscheme) {
                        SetDataSourceOnComboBox(cCmbScheme, returnObject.numberingscheme);
                    }

                }
            });
        }
        function SetDataSourceOnComboBox(ControlObject, Source) {
            ControlObject.ClearItems();
            for (var count = 0; count < Source.length; count++) {
                ControlObject.AddItem(Source[count].SchemaName, Source[count].Id);
            }
            ControlObject.SetSelectedIndex(0);
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
                document.getElementById("divDrawnOn").style.display = 'none';
            }
            else if (InstType == "C") {
                $('#<%=hdnInstrumentType.ClientID %>').val(InstType);
                document.getElementById("divInstrumentNo").style.display = 'block';
                document.getElementById("tdIDateDiv").style.display = 'block';
                document.getElementById("divDrawnOn").style.display = 'block';
            }
            else {
                $('#<%=hdnInstrumentType.ClientID %>').val(InstType);
                document.getElementById("divInstrumentNo").style.display = 'block';
                document.getElementById("tdIDateDiv").style.display = 'block';
                document.getElementById("divDrawnOn").style.display = 'none';
            }
        }
        //...............Customer LookUp.....
        //function CloseGridLookup() {
        //    gridLookup.ConfirmCurrentSelection();
        //    gridLookup.HideDropDown();
        //    gridLookup.Focus();
        //    gridquotationLookup.SetEnabled(true);
        //}
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
        function GetContactPerson() {
            //var CustomerComboBox = cCustomerComboBox.GetText();
            //if (!cCustomerComboBox.FindItemByText(CustomerComboBox)) {
            //    cCustomerComboBox.SetValue("");
            //    cCustomerComboBox.Focus();
            //    jAlert("Customer not Exists.");
            //    return;
            //}
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
                clblRunningBalanceCapsul.SetValue("0");
            }
            $('#MandatorysCustomer').hide();
            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            //var customerval = cCustomerComboBox.GetValue();
            var customerval = GetObjectID('hdnCustomerId').value;
           <%-- $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);--%>
            //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
            if (customerval != null && customerval != '') {
                var startDate = new Date();
                var TransDate = cdtTDate.GetDate();
                var branch = $("#ddlBranch").val();
                var VoucherType = document.getElementById("ComboVoucherType").value;
                //if (VoucherType != "P") {
                    page.GetTabByName('Billing/Shipping').SetEnabled(true);
                    LoadCustomerAddress(customerval, $('#ddlBranch').val(), 'CRP');
                //}
                if ($('#hfBSAlertFlag').val() == "1") {
                    jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            page.tabs[0].SetEnabled(true);
                        }
                    });
                }
                else {
                    page.tabs[0].SetEnabled(true);
                }
            }

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
            if (currentValue == "Advance") {
                if (document.getElementById('isFromTab').value == "1") {
                    var basketAmount = parseFloat(document.getElementById('HdBasketTotalAmount').value);
                    grid.GetEditor("Receipt").SetValue(basketAmount);
                    ReciptOldValue = 0;
                    ReceiptTextChange(grid.GetEditor("Receipt"));
                    return;
                }
                var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
                if (type == 'M') {
                    var totalAmount = GetPaymentTotalEnteredAmount();
                    grid.GetEditor("Receipt").SetValue(totalAmount);
                    ctxtVoucherAmount.SetValue(totalAmount);
                    c_txt_Debit.SetValue(totalAmount);
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                }
            }
            var type = grid.GetEditor("Type").GetValue();
            if (type != "Advance") {
                $("#CB_GSTApplicable").prop("checked", false);
                $('#ProductGSTApplicableSection').attr('style', 'display:none');
                cproductLookUp.gridView.UnselectRows();

                document.getElementById('hfHSN_CODE').value = "";
                $('#ProductSection').hide();
                //if (type == "OnAccount")
                //{
                //    $("#CB_GSTApplicable").prop("checked", true);
                //    $('#ProductGSTApplicableSection').attr('style', 'visibility:visible;margin-top: 29px;');
                //}
                //else
                //{
                //    $("#CB_GSTApplicable").prop("checked", false);
                //    $('#ProductGSTApplicableSection').attr('style', 'visibility:hidden');
                //}

            }
            else {
                $('#ProductSection').show();
                // $("#CB_GSTApplicable").prop("checked", true);
                $('#ProductGSTApplicableSection').attr('style', 'visibility:visible;margin-top: 29px;');
            }
            if (grid.GetEditor("Receipt").GetValue() != "0.00" && grid.GetEditor("Receipt").GetValue() != "0.0") {
                var VoucherAmount = ctxtVoucherAmount.GetValue();
                var ReceiptAmt = grid.GetEditor("Receipt").GetValue();
                var RunningBalance = clblRunningBalanceCapsul.GetValue();

                var noofvisiblerows = grid.GetVisibleRowsOnPage();
                if (noofvisiblerows == "1") {
                    c_txt_Debit.SetValue("0.00");
                    clblRunningBalanceCapsul.SetValue(parseFloat(RunningBalance) + parseFloat(ReceiptAmt));
                }
                else {
                    c_txt_Debit.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptAmt));
                    clblRunningBalanceCapsul.SetValue(parseFloat(RunningBalance) + parseFloat(ReceiptAmt));
                }

                grid.GetEditor("DocumentNo").SetValue("");
                grid.GetEditor("Receipt").SetValue("0.0");
                grid.GetEditor("DocumentID").SetValue("");
                
            }
            if (grid.GetEditor("Payment").GetValue() != "0.0" && grid.GetEditor("Payment").GetValue() != "0.00") {
                var VoucherAmount = ctxtVoucherAmount.GetValue();
                var PaymentAmt = grid.GetEditor("Payment").GetValue();
                var RunningBalance = clblRunningBalanceCapsul.GetValue();

                var noofvisiblerows = grid.GetVisibleRowsOnPage();
                if (noofvisiblerows == "1") {
                    ctxtTotalPayment.SetValue("0.00");
                    clblRunningBalanceCapsul.SetValue(parseFloat(RunningBalance) + parseFloat(PaymentAmt));
                }
                else {
                    ctxtTotalPayment.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentAmt));
                    clblRunningBalanceCapsul.SetValue(parseFloat(RunningBalance) + parseFloat(PaymentAmt));
                }
                //ctxtTotalPayment.SetValue(parseFloat(VoucherAmount - PaymentAmt));
                //clblRunningBalanceCapsul.SetValue(parseFloat(RunningBalance + PaymentAmt));
                grid.GetEditor("DocumentNo").SetValue("");
                grid.GetEditor("Payment").SetValue("0.0");
                grid.GetEditor("DocumentID").SetValue("");
            }

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

            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            //   if (e.htmlEvent.key == "Tab") {

            //   s.OnButtonClick(0);
            // }
        }

        function DocumentButnClick(s, e) {
            if (e.buttonIndex == 0) {
                var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
                if (Type == 'OnAccount' || Type == 'OnAccountPay') {
                    cDocumentpopUp.Hide();
                }
                else if (Type != 'Advance' && Type != '0') {
                    if (cdocumentLookUp.Clear()) {
                        cDocumentpopUp.Show();
                        cdocumentLookUp.Focus();
                    }
                    cCallbackPanelDocumentNo.PerformCallback('Type~' + Type);
                }

                else {
                    var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
                    if (type == 'M') {
                        var totalAmount = GetPaymentTotalEnteredAmount();
                        grid.GetEditor("Receipt").SetValue(totalAmount);
                        ctxtVoucherAmount.SetValue(totalAmount);
                        c_txt_Debit.SetValue(totalAmount);
                    }
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                }
            }
        }

        function DocumentlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cDocumentpopUp.Hide();
                //var vouchertype = cComboVoucherType.GetValue();
                var vouchertype = document.getElementById("ComboVoucherType").value;
                if (vouchertype == 'P') {
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                }
                else {
                    grid.batchEditApi.StartEdit(globalRowIndex, 4);
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
            //var vouchertype = cComboVoucherType.GetValue();
            var vouchertype = document.getElementById("ComboVoucherType").value;
            if (vouchertype == 'P') {
                grid.GetEditor("Payment").SetText(unpaidamount);

                var VoucherAmount = ctxtVoucherAmount.GetValue();

                // ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue) + parseFloat(unpaidamount));

                var payment = ctxtTotalPayment.GetValue();
                //ctxtTotalPayment.SetValue(parseFloat(payment) - parseFloat(PaymentValue) + parseFloat(unpaidamount));
                //ctxtTotalPayment.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue) + parseFloat(unpaidamount));//  9-10-17
                ctxtTotalPayment.SetValue(parseFloat(ctxtTotalPayment.GetValue()) - parseFloat(PaymentValue) + parseFloat(unpaidamount));

                var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
                if (Type != 'AdvancePayment') {
                    clblRunningBalanceCapsul.SetValue(parseFloat(VoucherAmount - ctxtTotalPayment.GetValue()));
                }

            }
            else {
                grid.GetEditor("Receipt").SetText(unpaidamount);
                var VoucherAmount = ctxtVoucherAmount.GetValue();
                // ctxtVoucherAmount.SetValue(parseFloat(unpaidamount) + parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                var Receipt = c_txt_Debit.GetValue();
                c_txt_Debit.SetValue(parseFloat(Receipt) + parseFloat(unpaidamount) - parseFloat(ReceiptValue));
                var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
                if (Type != 'AdvancePayment') {
                    clblRunningBalanceCapsul.SetValue(parseFloat(VoucherAmount - c_txt_Debit.GetValue()));
                }
            }


            if (LookUpData != null) {
                $('#<%=hdndocumentno.ClientID %>').val(DocumentID + ',');
            }


            grid.GetEditor("DocumentNo").SetText(ProductCode);
            // var vouchertype = cComboVoucherType.GetValue();
            var vouchertype = document.getElementById("ComboVoucherType").value;
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

        }
        function CallbackPanelDocumentNo_endcallback() {
            cdocumentLookUp.ShowDropDown();
            cdocumentLookUp.Focus();
        }
    </script>
    <%--Batch Product Popup End--%>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section--%>
    <script>
        function disp_prompt(name) {

            if (name == "tab0") {
                // gridLookup.Focus();
                ctxtCustName.Focus();

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

    </script>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section End--%>
    <%--Tax Script Start--%>
    <script type="text/javascript">
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
            taxAmtButnClick();
            taxAmtButnClick1();
        }
        function TaxAmountKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }
        function taxAmtButnClick(s, e) {
            //if (e.buttonIndex == 0) {
            var amountAre = "2"; // To set value Inclusive by default

            if (amountAre != null) {
                var ProductID = $("#hfHSN_CODE").val();

                if (ProductID.trim() != "") {

                    globalNetAmount = parseFloat(c_txt_Debit.GetValue());
                    document.getElementById('HdSerialNo').value = 0;
                    ctxtTaxTotAmt.SetValue(0);
                    ccmbGstCstVat.SetSelectedIndex(0);
                    $('.RecalculateInline').hide();
                    caspxTaxpopUp.Show();
                    //var Amount = Math.round(globalNetAmount).toFixed(2);
                    var Amount = globalNetAmount.toFixed(2);
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
                    //if (globalRowIndex > -1) {
                    //    cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + amountAre);
                    //} else {
                    //    cgridTax.PerformCallback('New~' + amountAre);

                    //}

                    if (document.getElementById("hdnTaxMode").value == "N") {
                        // document.getElementById("hdnTaxMode").value = "";
                        cgridTax.PerformCallback('New~' + amountAre);


                    } else {
                        cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + amountAre);

                    }
                    ctxtprodBasicAmt.SetValue(Amount);
                } else {
                    grid.batchEditApi.StartEdit(globalRowIndex, 13);
                }
            }
        }
        function taxAmtButnClick1(s, e) {
            //console.log(grid.GetFocusedRowIndex());
            rowEditCtrl = s;
        }
        function txtPercentageLostFocus(s, e) {

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
                ctxtTaxTotAmt.SetValue(gridValue + ddValue);
                cgridTax.cpUpdated = "";
            }

            else {
                var totAmt = ctxtTaxTotAmt.GetValue();
                cgridTax.CancelEdit();
                caspxTaxpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 13);

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
            var roundedOfAmount = totalInlineTaxAmount;
            ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);


            var diffDisc = roundedOfAmount - totalInlineTaxAmount;
            if (diffDisc > 0)
                document.getElementById('taxroundedOf').innerText = 'Adjustment ' + Math.abs(diffDisc.toFixed(3));
            else if (diffDisc < 0)
                document.getElementById('taxroundedOf').innerText = 'Adjustment ' + Math.abs(diffDisc.toFixed(3));
            else
                document.getElementById('taxroundedOf').innerText = '';
        }
    </script>
    <%--Tax Script End--%>

    
     <%-- Project Script start --%>
    <script>
        function ProjectListKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function ProjectListButnClick(s, e) {
            //ctaggingGrid.PerformCallback('BindComponentGrid');
            clookup_Project.ShowDropDown();
        }
    </script>
    <%-- Project Script End --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="UserControls/Js/ucPaymentDetails.js?var=1.0"></script>
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeadTitle" Text="Add Customer Receipt/Payment" runat="server"></asp:Label>
            </h3>
            <div id="pageheaderContent" class="pull-right reverse wrapHolder content horizontal-images" style="display: none;" runat="server">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder" id="divContactPhone" style="display: none;" runat="server">
                                <table>
                                    <tr>
                                        <td>Contact Person's Phone</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblContactPhone" runat="server" Text="N/A" CssClass="classout"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder" id="divGSTIN" style="display: none;" runat="server">
                                <table>
                                    <tr>
                                        <td>GST Registed?</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblGSTIN" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="customerReceiptCross" runat="server">
                <div id="divcross" class="crossBtn" style="margin-left: 100px;"><a href="CustomerReceiptPaymentList.aspx"><i class="fa fa-times"></i></a></div>
            </div>
            <div id="customerReceiptPopupCross" runat="server" style="display: none">
                <div id="btncrossPopup" class="crossBtn" style="margin-left: 100px;"><a href="#" onclick="btncrossPopupOnClick()"><i class="fa fa-times"></i></a></div>
            </div>
        </div>
    </div>
    <div class="form_main  clearfix">

        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
            <TabPages>
                <dxe:TabPage Name="General" Text="General">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <div id="DivEntry">
                                <div id="divChangable" runat="server" style="background: #f5f4f3; padding: 5px 0 5px 0; margin-bottom: 10px; border-radius: 4px; border: 1px solid #ccc;">
                                    <div class="">

                                        <div class="col-md-3">
                                            <label for="exampleInputName2" style="margin-top: 8px">
                                                Voucher Type <b id="bTypeText" runat="server" style="width: 20%; font-size: 12px"></b>
                                            </label>
                                            <div>
                                                <asp:DropDownList ID="ComboVoucherType" runat="server" Width="100%" onchange="rbtnType_SelectedIndexChanged()">
                                                    <asp:ListItem Text="Receipt" Value="R" />
                                                    <asp:ListItem Text="Payment" Value="P" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="divNumberingScheme" runat="server">
                                            <label style="margin-top: 8px">Numbering Scheme</label>
                                            <%-- <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme"
                                                DataSourceID="SqlSchematype" SelectedIndex="0" EnableCallbackMode="true"
                                                TextField="SchemaName" ValueField="ID" 
                                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True" OnCallback="CmbScheme_Callback">
                                                <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}" GotFocus="NumberingScheme_GotFocus"></ClientSideEvents>
                                            </dxe:ASPxComboBox>--%>
                                            <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme"
                                                SelectedIndex="0"
                                                TextField="SchemaName" ValueField="ID"
                                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True" OnCallback="CmbScheme_Callback">
                                                <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}" GotFocus="NumberingScheme_GotFocus" EndCallback="CmbSchemeEndCallback"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                        <div class="col-md-2 lblmTop8" style="display: none" runat="server">
                                            <div>
                                                <asp:DropDownList ID="ddlEnterBranch" runat="server" DataSourceID="dsBranch"
                                                    DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                                </asp:DropDownList>

                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <label style="margin-top: 8px">Document No. <span style="color: red">*</span></label>
                                            <div>
                                                <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="30">                             
                                                </asp:TextBox>
                                                <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <label style="margin-top: 8px">Posting Date <span style="color: red">*</span>  </label>
                                            <div>
                                                <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom"
                                                    Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                                    <ButtonStyle Width="13px"></ButtonStyle>
                                                    <ClientSideEvents GotFocus="TransDate_GotFocus" DateChanged="changeFocusDate"></ClientSideEvents>
                                                </dxe:ASPxDateEdit>
                                                <span id="MandatoryTransDate" class="iconTransDate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <label>Unit <span style="color: red">*</span></label>
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
                                                <% if (CustomerRights.CanAdd )
                                                            { %>
                                                <i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i>

                                                <% 
                                                } 
                                                %>

                                            </label>
                                            <%--<dxe:ASPxCallbackPanel runat="server" ID="CustomerCallBackPanel" ClientInstanceName="cCustomerCallBackPanel"
                                                OnCallback="CustomerCallBackPanel_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                         <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" ClientInstanceName="gridLookup"
                                                            KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">
                                                            <Columns>
                                                                <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Unique ID" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="2" Caption="Type" Settings-AutoFilterCondition="Contains" Width="0">
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

                                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                            </GridViewProperties>
                                                            <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" GotFocus="Customer_GotFocus" />
                                                            <ClearButton DisplayMode="Auto">
                                                            </ClearButton>
                                                        </dxe:ASPxGridLookup>
                                                    </dxe:PanelContent>
                                                </PanelCollection>
                                                <ClientSideEvents EndCallback="CustomerCallBackPanelEndCallBack" />
                                            </dxe:ASPxCallbackPanel>--%>
                                            <%--<dxe:ASPxComboBox ID="CustomerComboBox" runat="server" EnableCallbackMode="true" CallbackPageSize="15"
                                                ValueType="System.String" ValueField="cnt_internalid" ClientInstanceName="cCustomerComboBox" Width="92%"
                                                OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL"  FilterMinLength="4" 
                                                OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{1} [{0}]"
                                                DropDownStyle="DropDown">
                                                <Columns>
                                                    <dxe:ListBoxColumn FieldName="uniquename" Caption="Unique ID" Width="200px" />
                                                    <dxe:ListBoxColumn FieldName="Name" Caption="Name" Width="200px" />
                                                </Columns>
                                                <ClientSideEvents ValueChanged="function(s, e) {GetContactPerson()}" GotFocus="function(s,e){cCustomerComboBox.ShowDropDown();}" />
                                            </dxe:ASPxComboBox>--%>
                                            <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                                            </dxe:ASPxButtonEdit>
                                            <span id="MandatorysCustomer" class="iconCustomer pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                        <div class="col-md-3 lblmTop8">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person" CssClass="inline">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" Native="true"
                                                ClientInstanceName="cContactPerson" Font-Size="12px" ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <div class="col-md-2 lblmTop8" id="tdCashBankLabel" runat="server">
                                            <label>Cash/Bank <span style="color: red">*</span></label>
                                            <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientInstanceName="cddlCashBank" Width="100%" OnCallback="ddlCashBank_Callback">
                                                <ClientSideEvents SelectedIndexChanged="CashBank_SelectedIndexChanged" GotFocus="CashBank_GotFocus" />
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                        <div class="col-md-1 lblmTop8">
                                            <label>Currency</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="CmbCurrency" EnableIncrementalFiltering="True" ClientInstanceName="cCmbCurrency"
                                                    DataSourceID="SqlCurrencyBind"
                                                    TextField="Currency_AlphaCode" ValueField="Currency_ID" SelectedIndex="0" Native="true"
                                                    runat="server" ValueType="System.String" EnableSynchronization="True" Width="100%" CssClass="pull-left">
                                                    <ClientSideEvents ValueChanged="function(s,e){Currency_Rate()}" GotFocus="CurrencyGotFocus"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1 rate lblmTop8">
                                            <label>Rate  </label>
                                            <div>
                                                <dxe:ASPxTextBox runat="server" ID="txtRate" HorizontalAlign="Right" ClientInstanceName="ctxtRate" Width="100%" Height="24px" CssClass="pull-left">
                                                    <MaskSettings Mask="&lt;0..999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div id="multipleredio" class="col-md-2" runat="server">
                                            <div style="padding-top: 20px; margin-top: 10px">
                                                <asp:RadioButtonList ID="rdl_MultipleType" runat="server" Width="160px" RepeatDirection="Horizontal" onchange="return selectValue();">
                                                    <asp:ListItem Text="Single" Value="S" Selected></asp:ListItem>
                                                    <asp:ListItem Text="Multiple" Value="M"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div id="singletype" runat="server">

                                            <div class="col-md-2 lblmTop8">
                                                <label style="">Instrument Type</label>
                                                <div style="">
                                                    <dxe:ASPxComboBox ID="cmbInstrumentType" runat="server" ClientInstanceName="cComboInstrumentTypee" Font-Size="12px"
                                                        ValueType="System.String" Width="100%" EnableIncrementalFiltering="True" Native="true">
                                                        <Items>

                                                            <dxe:ListEditItem Text="Cheque" Value="C" Selected />
                                                            <dxe:ListEditItem Text="Draft" Value="D" />
                                                            <dxe:ListEditItem Text="E.Transfer" Value="E" />
                                                            <dxe:ListEditItem Text="Cash" Value="CH" />
                                                            <dxe:ListEditItem Text="Card" Value="CRD" />
                                                        </Items>
                                                        <ClientSideEvents SelectedIndexChanged="InstrumentTypeSelectedIndexChanged" GotFocus="InstrumentType_GotFocus" />
                                                    </dxe:ASPxComboBox>

                                                </div>
                                            </div>

                                            <div class="col-md-3 lblmTop8" id="divInstrumentNo" style="" runat="server">
                                                <label id="" style="">Instrument No</label>
                                                <div id="">
                                                    <dxe:ASPxTextBox runat="server" ID="txtInstNobth" ClientInstanceName="ctxtInstNobth" Width="100%" MaxLength="30">
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2 lblmTop8" id="tdIDateDiv" style="" runat="server">
                                                <label id="tdIDateLable" style="">Instrument Date</label>
                                                <div id="tdIDateValue" style="">
                                                    <dxe:ASPxDateEdit ID="InstDate" runat="server" EditFormat="Custom" ClientInstanceName="cInstDate"
                                                        UseMaskBehavior="True" Font-Size="12px" Width="100%" EditFormatString="dd-MM-yyyy">
                                                        <ClientSideEvents GotFocus="InstrumentDate_GotFocus"></ClientSideEvents>
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3 lblmTop8" id="divDrawnOn" style="" runat="server">
                                                <label id="" style="">Drawn on ( Party banker’s name )</label>
                                                <div id="">
                                                    <dxe:ASPxTextBox runat="server" ID="txtDrawnOn" ClientInstanceName="ctxtDrawnOn" Width="100%" MaxLength="30">
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </div>

                                        </div>
                                         <div class="clear"></div>
                                        <div class="col-md-5 lblmTop8">
                                            <label>Narration </label>
                                            <div>
                                                <asp:TextBox ID="txtNarration" runat="server" MaxLength="500" onkeydown="checkTextAreaMaxLength(this,event,'500');"
                                                    TextMode="MultiLine"
                                                    Width="100%"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <label>Voucher Amount <span style="color: red">*</span> </label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtVoucherAmount" runat="server" ClientInstanceName="ctxtVoucherAmount" Width="100%" CssClass="pull-left">
                                                    <ClientSideEvents TextChanged="function(s, e) { GetInvoiceMsg(e)}" LostFocus="lostFocusVoucherAmount" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                    <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                                </dxe:ASPxTextBox>
                                                <span id="MandatoryVoucherAmount" class="iconVoucherAmount pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-6" style="margin-top: 7px; margin-bottom: 5px;" id="ProductSection" runat="server">
                                            <div style="height: auto; margin-bottom: 5px;">
                                                Select product to calculate GST for Advance
                                            </div>
                                            <div class="Left_Content">
                                                <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="ComponentPanel_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="productLookUp" runat="server" SelectionMode="Multiple" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                                                                KeyFieldName="Products_ID" Width="100%" TextFormatString="{0}" MultiTextSeparator=", "
                                                                ClientSideEvents-TextChanged="ProductSelected">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Name" Width="220">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60" Visible="false">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100" Visible="false">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseProductLookup" UseSubmitBehavior="False" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                            </dxe:ASPxGridLookup>
                                                            <asp:HiddenField ID="hfHSN_CODE" runat="server" />
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="ComponentPanel_EndCallBack" />
                                                </dxe:ASPxCallbackPanel>
                                                <%-- <dxe:ASPxComboBox ID="ProductComboBox" runat="server" EnableCallbackMode="true" CallbackPageSize="15"
                                                    ValueType="System.String" ValueField="cnt_internalid" ClientInstanceName="cProductComboBox" Width="92%"
                                                    OnItemsRequestedByFilterCondition="ProductComboBox_OnItemsRequestedByFilterCondition_SQL"
                                                    OnItemRequestedByValue="ProductComboBox_OnItemRequestedByValue_SQL" TextFormatString="{1} [{0}]"
                                                    DropDownStyle="DropDown">
                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="uniquename" Caption="Unique ID" Width="200px" />
                                                        <dxe:ListBoxColumn FieldName="Name" Caption="Name" Width="200px" />
                                                    </Columns>
                                                    <ClientSideEvents  GotFocus="function(s,e){cProductComboBox.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>--%>
                                            </div>

                                        </div>
                                        <div class="col-md-2" id="ProductGSTApplicableSection" runat="server" style="margin-top: 29px;">
                                            <asp:CheckBox ID="CB_GSTApplicable" runat="server" Text="GST Applicable" TextAlign="Right" Checked="false" onclick="return CheckedChange();"></asp:CheckBox>
                                        </div>

                                        <div class="col-md-2 lblmTop8">
                                                <label id="lblProject" runat="server">Project</label>
                                                <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="ProjectServerModeDataSource"
                                                    KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                    </Columns>
                                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                        <Templates>
                                                            <StatusBar>
                                                                <table class="OptionsTable" style="float: right">
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </StatusBar>
                                                        </Templates>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                    </GridViewProperties>
                                                    <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" />
                                                    <ClearButton DisplayMode="Always">
                                                    </ClearButton>
                                                </dxe:ASPxGridLookup>
                                                <dx:LinqServerModeDataSource ID="ProjectServerModeDataSource" runat="server" OnSelecting="ProjectServerModeDataSource_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />

                                            </div>
                                        <div class="clear"></div>
                                    </div>

                                    <div class="clear"></div>
                                    <div id="Multipletype" style="display: none" class="bgrnd" runat="server">
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
                                                DocumentNumberClose();
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
                                                        <EditFormSettings />
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DocumentID" ReadOnly="true" Caption="DocumentID" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <ClientSideEvents EndCallback="grid_SalesInvoiceOnEndCallback" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                            </dxe:ASPxGridView>
                                            <div class="text-center">

                                                <dxe:ASPxButton ID="Btn" ClientInstanceName="cbtnOK" runat="server"
                                                    AutoPostBack="false" CssClass="btn btn-primary  mLeft mTop" Text="OK"
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
                                                <ClientSideEvents SelectedIndexChanged="DocumentType_SelectedIndexChanged" EndCallback="Type_EndCallback"
                                                    GotFocus="TypeGotFocus" />
                                            </PropertiesComboBox>
                                        </dxe:GridViewDataComboBoxColumn>


                                        <dxe:GridViewDataButtonEditColumn FieldName="DocumentNo" Caption="Document Number" VisibleIndex="1" Width="14%" ReadOnly="true">
                                            <PropertiesButtonEdit>
                                                <ClientSideEvents ButtonClick="DocumentButnClick" KeyDown="ProductKeyDown" />
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
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
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
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
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
                                        <dxe:GridViewDataTextColumn FieldName="IsOpening" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                            <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="6" Caption=" ">
                                            <CustomButtons>
                                                <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                </dxe:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                        </dxe:GridViewCommandColumn>
                                    </Columns>
                                    <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="GetVisibleIndex" />
                                    <SettingsDataSecurity AllowEdit="true" />
                                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                        <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                    </SettingsEditing>
                                    <%--<SettingsBehavior ColumnResizeMode="Disabled"   />--%>
                                </dxe:ASPxGridView>

                                <div class="text-center">
                                    <table style="margin-left: 30%; margin-top: 10px">
                                        <tr>
                                            <td style="padding-right: 50px"><b>Total Amount</b></td>
                                            <td style="width: 203px;">
                                                <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                    <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTotalPayment" runat="server" Width="105px" ClientInstanceName="ctxtTotalPayment" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                    <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td style="padding-right: 30px">
                                                <asp:Label ID="lbltaxAmountHeader" runat="server" Text="Total Taxable Amount" Font-Bold="true"></asp:Label></td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTaxAmount" runat="server" Width="105px" ClientInstanceName="ctxtTaxAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                    <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="content reverse horizontal-images clearfix" style="margin-top: 8px; width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                        <ul>
                                            <li class="clsbnrLblTaxableAmt" style="float: right;">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Running Balance" ClientInstanceName="cbnrLblInvVal" />
                                                                </td>
                                                                <td>
                                                                    <strong>
                                                                        <dxe:ASPxLabel ID="lblRunningBalanceCapsulCrp" runat="server" Text="0.00" ClientInstanceName="clblRunningBalanceCapsul" />
                                                                    </strong>
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div id="divSubmitButton" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="padding: 5px 0;">
                                                <span id="tdSaveButtonNew" runat="server" style="display:none">
                                                    <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveNew" runat="server"
                                                        AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="S&#818;ave & New"
                                                        UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {SaveButtonClickNew();}" />
                                                    </dxe:ASPxButton>
                                                </span>
                                                <span id="tdSaveButton" runat="server" style="display:none">
                                                    <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server"
                                                        AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save & Ex&#818;it"
                                                        UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                                    </dxe:ASPxButton>
                                                </span>
                                                <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" UseSubmitBehavior="False"
                                                    CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                                    <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                                </dxe:ASPxButton>
                                                <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                                </dxe:ASPxButton>
                                                <b><span id="tagged" runat="server" style="display: none; color: red">This Customer Receipt Payment is tagged in other modules. Cannot Modify data except UDF</span></b>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%--Batch Product Popup Start--%>

                                <dxe:ASPxPopupControl ID="DocumentpopUp" runat="server" ClientInstanceName="cDocumentpopUp"
                                    CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                                    Width="700" HeaderText="Select Document Number" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                    <%--   <HeaderTemplate>
                                        <span>Select Document Number</span>
                                    </HeaderTemplate>
                                    --%>
                                    <ContentCollection>
                                        <dxe:PopupControlContentControl runat="server">
                                            <label><strong>Search By Document Number</strong></label>
                                            <%-- <span style="color: red;">[Press ESC key to Cancel]</span>--%>
                                            <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDocumentNo" ClientInstanceName="cCallbackPanelDocumentNo" OnCallback="CallbackPanelDocumentNo_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxGridLookup ID="documentLookUp" runat="server" ClientInstanceName="cdocumentLookUp" Width="800"
                                                            KeyFieldName="DocumentID" TextFormatString="{1}" MultiTextSeparator=", " ClientSideEvents-TextChanged="DocumentSelected"
                                                            ClientSideEvents-KeyDown="DocumentlookUpKeyDown" OnDataBinding="documentLookUp_DataBinding">
                                                            <Columns>

                                                                <dxe:GridViewDataColumn FieldName="DocDate" Caption="Document Date" Width="200">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>

                                                                <dxe:GridViewDataColumn FieldName="DocumentNumber" Caption="Document Number" Width="250">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>

                                                                <dxe:GridViewDataColumn FieldName="branch" Caption="Unit" Width="150">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>

                                                                <dxe:GridViewDataColumn FieldName="UnPaidAmount" Caption="UnPaid Amount" Width="200">
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
                                <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                                    SelectCommand="prc_CustomerReceiptPaymentDetails" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
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
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <ucBS:BillingShippingControl runat="server" ID="BillingShippingControl" />
                            <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="CRP" />
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
            </TabPages>
            <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
                                                 
                                               if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                           else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }


	                                            }"></ClientSideEvents>

        </dxe:ASPxPageControl>


        <div id="DivHiddenField">
            <asp:HiddenField ID="hdnBtnClick" runat="server" />
            <asp:HiddenField ID="hdnInstrumentType" runat="server" />
            <asp:HiddenField ID="hdnPageStatus" runat="server" />
            <asp:HiddenField ID="hdnTaggedFrom" runat="server" />
            <asp:HiddenField ID="hdnSchemaType" runat="server" />
            <asp:HiddenField ID="hdfIsDelete" runat="server" />
            <asp:HiddenField ID="hdn_Mode" runat="server" />
            <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
            <asp:HiddenField ID="hdnRefreshType" runat="server" />
            <asp:HiddenField ID="IsInvoiceTagged" runat="server" />
            <asp:HiddenField ID="hdndocumentno" runat="server" Value="0" />
            <asp:HiddenField ID="hdnCustomerId" runat="server" />
            <asp:HiddenField ID="hdnDate" runat="server" />
            <asp:HiddenField ID="hdnInvoicePageStatus" runat="server" />
            <asp:HiddenField ID="HdSelectedBranch" runat="server" />
            <asp:HiddenField ID="hdnTaxMode" runat="server" />
            <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" /><%--for Project  --%>
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
        <!--Customer Modal -->
        <%--Rev 1.0--%>
        <%--<div class="modal fade" id="CustModel" role="dialog">--%>
        <div class="modal fade" id="CustModel" role="dialog" data-backdrop="static" data-keyboard="false">
        <%--Rev end 1.0--%>
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Customer Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search by Customer Name or Unique Id" />
                        <div id="CustomerTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Customer Name</th>
                                    <th>Unique Id</th>
                                    <th>Address</th>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <!--Customer Modal -->

        <%--Customer Popup--%>
        <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="750px"
            Width="1020px" HeaderText="Add New Customer" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <HeaderTemplate>
                <span>Add New Customer</span>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <%--Tax PopUP Start--%>
        <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
            Width="850px" HeaderText="Customer Receipt/Payment Tax" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <HeaderTemplate>
                <span style="color: #fff"><strong>Customer Receipt/Payment Tax</strong></span>
                <dxe:ASPxImage ID="ASPxImage31" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                    <ClientSideEvents Click="function(s, e){ 
                                                            cgridTax.CancelEdit();
                                                            caspxTaxpopUp.Hide();
                                                        }" />
                </dxe:ASPxImage>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <asp:HiddenField runat="server" ID="setCurrentProdCode" />
                    <asp:HiddenField runat="server" ID="HdSerialNo" />
                    <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
                    <asp:HiddenField runat="server" ID="HdProdNetAmt" />
                    <div id="content-6">
                        <div class="col-sm-3">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Entered Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxLabel ID="lblTaxProdGrossAmt" runat="server" Text="" ClientInstanceName="clblTaxProdGrossAmt"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div class="col-sm-3 gstGrossAmount">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>GST</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxLabel ID="lblGstForGross" runat="server" Text="" ClientInstanceName="clblGstForGross"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div class="col-sm-3" style="display: none">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Discount</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxLabel ID="lblTaxDiscount" runat="server" Text="" ClientInstanceName="clblTaxDiscount"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>


                        <div class="col-sm-3" style="display: none">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Net Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxLabel ID="lblProdNetAmt" runat="server" Text="" ClientInstanceName="clblProdNetAmt"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div class="col-sm-2 gstNetAmount">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>GST</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxLabel ID="lblGstForNet" runat="server" Text="" ClientInstanceName="clblGstForNet"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                    </div>

                    <%--Error Message--%>
                    <div id="ContentErrorMsg">
                        <div class="col-sm-8">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Status
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tax Code/Charges Not defined.
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <table style="width: 100%;">
                        <tr>
                            <td colspan="2"></td>
                        </tr>

                        <tr>
                            <td colspan="2"></td>
                        </tr>

                        <tr style="display: none">
                            <td><span><strong>Product Basic Amount</strong></span></td>
                            <td>
                                <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" ReadOnly="true"
                                    runat="server" Width="50%">
                                    <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>

                        <tr class="cgridTaxClass">
                            <td colspan="3">
                                <dxe:ASPxGridView runat="server" OnBatchUpdate="taxgrid_BatchUpdate" KeyFieldName="Taxes_ID" ClientInstanceName="cgridTax" ID="aspxGridTax"
                                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                    OnCustomCallback="cgridTax_CustomCallback"
                                    Settings-ShowFooter="false" AutoGenerateColumns="False" OnCellEditorInitialize="aspxGridTax_CellEditorInitialize"
                                    OnHtmlRowCreated="aspxGridTax_HtmlRowCreated"
                                    OnRowInserting="taxgrid_RowInserting" OnRowUpdating="taxgrid_RowUpdating" OnRowDeleting="taxgrid_RowDeleting">
                                    <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Taxes_Name" ReadOnly="true" Caption="Tax Component ID">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="taxCodeName" ReadOnly="true" Caption="Tax Component Name">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <%--<dxe:GridViewDataComboBoxColumn Caption="percentage" FieldName="TaxField" VisibleIndex="2">
                                                <PropertiesComboBox ClientInstanceName="cTaxes_Name" ValueField="Taxes_ID" TextField="Taxes_Name" DropDownStyle="DropDown">
                                                    <ClientSideEvents SelectedIndexChanged="cmbtaxCodeindexChange"
                                                        GotFocus="CmbtaxClick" />
                                                </PropertiesComboBox>
                                            </dxe:GridViewDataComboBoxColumn>--%>
                                        <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <%--<SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
                                    <SettingsDataSecurity AllowEdit="true" />
                                    <SettingsEditing Mode="Batch">
                                        <BatchEditSettings EditMode="row" />
                                    </SettingsEditing>
                                    <ClientSideEvents EndCallback=" cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />

                                </dxe:ASPxGridView>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table class="InlineTaxClass">
                                    <tr class="GstCstvatClass" style="">
                                        <td style="padding-top: 10px; padding-bottom: 15px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                        <td style="padding-top: 10px; padding-bottom: 15px;">
                                            <dxe:ASPxComboBox ID="cmbGstCstVat" ClientInstanceName="ccmbGstCstVat" runat="server" SelectedIndex="-1"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                                ClearButton-DisplayMode="Always" OnCallback="cmbGstCstVat_Callback">
                                                <Columns>
                                                    <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                    <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />

                                                </Columns>
                                                <ClientSideEvents SelectedIndexChanged="cmbGstCstVatChange"
                                                    GotFocus="CmbtaxClick" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 10px; padding-bottom: 15px; padding-right: 25px">
                                            <dxe:ASPxTextBox ID="txtGstCstVat" MaxLength="80" ClientInstanceName="ctxtGstCstVat" ReadOnly="true" Text="0.00"
                                                runat="server" Width="100%">
                                                <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                            </dxe:ASPxTextBox>


                                        </td>
                                        <td>
                                            <input type="button" onclick="recalculateTax()" class="btn btn-info btn-small RecalculateInline" value="Recalculate GST" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="pull-left">
                                    <asp:Button ID="Button2" runat="server" Text="Close" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
                                    <asp:Button ID="Button1" runat="server" Text="Ok" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" Visible="false" />
                                    <span id="taxroundedOf"></span>
                                </div>
                                <table class="pull-right">
                                    <tr>
                                        <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                                runat="server" Width="100%" CssClass="pull-left mTop">
                                                <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                            </dxe:ASPxTextBox>

                                        </td>
                                    </tr>
                                </table>


                                <div class="clear"></div>
                            </td>
                        </tr>

                    </table>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <%--Tax PopUP End--%>
        <asp:HiddenField runat="server" ID="HdBasketTotalAmount" />
        <asp:HiddenField runat="server" ID="isFromTab" />
        <asp:HiddenField runat="server" ID="hdnSalesInvoice" />

        <dxe:ASPxLoadingPanel ID="LoadingPanelCRP" runat="server" ClientInstanceName="cLoadingPanelCRP"
            Modal="True">
        </dxe:ASPxLoadingPanel>
        <asp:SqlDataSource ID="CustomerDataSource" runat="server"  />
</asp:Content>
