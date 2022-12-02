<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerAdvanceReceipt.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerAdvanceReceipt" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/ucPaymentDetails.ascx" TagPrefix="uc1" TagName="ucPaymentDetails" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">

    <style>
        #aspxGridTax_DXStatus, #grid_DXStatus {
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
    <style type="text/css">
        #grid_DXMainTable > tbody > tr > td:last-child, #productLookUp_DDD_gv_DXMainTable > tbody > tr > td:nth-child(2) {
            display: none !important;
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

        #grid_DXStatus span > a {
            display: none;
        }

        #aspxGridTax_DXEditingErrorRow0 {
            display: none;
        }

        .horizontal-images.content li {
            float: left;
        }

        #rdl_SaleInvoice {
            margin-top: 3px;
        }

            #rdl_SaleInvoice > tbody > tr > td {
                padding-right: 5px;
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
        var CustomerCurrentDateAmount = 0;

        function ProductlookUpKeyDown(s, e) {

            if (e.htmlEvent.key == "Escape") {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }
        }

        function ProductSelected(s, e) {

            if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return;
            }

            var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            //var ProductCode = cproductLookUp.GetValue();

            var focusedRow = cproductLookUp.gridView.GetFocusedRowIndex();
            var ProductCode = cproductLookUp.gridView.GetRow(focusedRow).children[1].innerText;

            if (!ProductCode) {
                LookUpData = null;
            }
            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);
            //console.log(LookUpData);
            pageheaderContent.style.display = "block";
            //cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");

            //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

            //var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];

            //var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            //if (strRate == 0) {
            //strSalePrice = strSalePrice;
            //}
            //else {
            //strSalePrice = strSalePrice / strRate;
            //}

            //tbDescription.SetValue(strDescription);
            //tbUOM.SetValue(strUOM);
            //tbSalePrice.SetValue(strSalePrice);

            //grid.GetEditor("Quantity").SetValue("0.00");
            //grid.GetEditor("Discount").SetValue("0.00");
            //grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            //grid.GetEditor("TotalAmount").SetValue("0.00");

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            <%--$('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
		    $('#<%= lblProduct.ClientID %>').text(strDescription);
		    $('#<%= lblbranchName.ClientID %>').text(strBranch);--%>

            //var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

            //if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
		        <%--$('#<%= lblPackingStk.ClientID %>').text(PackingValue);--%>
            //    divPacking.style.display = "block";
            //} else {
            //    divPacking.style.display = "none";
            //}

            //divPacking.style.display = "none";
            //lblbranchName lblProduct
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");

            var IsComponentProduct = SpliteDetails[15];
            var ComponentProduct = SpliteDetails[16];

            //document.getElementById("ddlInventory").disabled = true;

            //cacpAvailableStock.PerformCallback(strProductID);
            //Debjyoti
            //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
            cddl_AmountAre.SetEnabled(false);
            grid.batchEditApi.StartEdit(globalRowIndex, 3);
        }

        function ProductKeyDown(s, e) {

            //console.log(e.htmlEvent.key);
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {

                s.OnButtonClick(0);
            }
        }

        function ProductButnClick(s, e) {

            if (e.buttonIndex == 0) {

                if (cproductLookUp.Clear()) {
                    cProductpopUp.Show();
                    cproductLookUp.Focus();
                    cproductLookUp.ShowDropDown();
                }
            }
        }
        function ProductsGotFocusFromID(s, e) {

            //pageheaderContent.style.display = "block";
            //var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

            //var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

            //var ddlbranch = $("[id*=ddl_Branch]");
            //var strBranch = ddlbranch.find("option:selected").text();

            //var SpliteDetails = ProductID.split("||@||");
            //var strProductID = SpliteDetails[0];
            //var strDescription = SpliteDetails[1];
            //var strUOM = SpliteDetails[2];
            //var strStkUOM = SpliteDetails[4];
            //var strSalePrice = SpliteDetails[6];
            //var IsPackingActive = SpliteDetails[10];
            //var Packing_Factor = SpliteDetails[11];
            //var Packing_UOM = SpliteDetails[12];
            //var strProductShortCode = SpliteDetails[14];
            //var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

            //strProductName = strDescription;

            //if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            <%--$('#<%= lblPackingStk.ClientID %>').text(PackingValue);--%>
            //    divPacking.style.display = "block";
            //} else {
            //    divPacking.style.display = "none";
            //}

            <%--$('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);--%>

            //if (ProductID != "0") {
            //    cacpAvailableStock.PerformCallback(strProductID);
            //}
        }

        function DocumentNumberClose(s, e) {
            ctxtVoucherAmount.SetValue(0);
            cDocumentpopUp.Hide();

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
        function CustomerCallBackPanelEndCallBack() {
            if (cCustomerCallBackPanel.cpTotalTransectionAmount) {
                if (cCustomerCallBackPanel.cpTotalTransectionAmount != "") {
                    CustomerCurrentDateAmount = parseFloat(cCustomerCallBackPanel.cpTotalTransectionAmount);
                    cCustomerCallBackPanel.cpTotalTransectionAmount = null;
                }
            }
            //GetContactPerson();
            cContactPerson.Focus();
        }

        //.................Product LookUp.....................
        function CloseProductLookup() {
            cproductLookUp.ConfirmCurrentSelection();
            cproductLookUp.HideDropDown();
            cproductLookUp.Focus();
        }

        function cmbContactPersonEndCall(s, e) {
            if (cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) {
                pageheaderContent.style.display = "block";
                $("#<%=divGSTIN.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblGSTIN.ClientID %>').innerHTML = cContactPerson.cpGSTN;
                cContactPerson.cpGSTN = null;
            }
            cContactPerson.Focus();
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
            cddlCashBank.PerformCallback(branch);
        }
        $(document).ready(function () {
            //alert('111');
            //grid.GetEditor('NetAmount').SetEnabled(false);
            //grid.GetEditor('TaxAmount').SetEnabled(false);
            var isCtrl = false;
            if (getUrlVars().key == "ADD") {
                page.tabs[1].SetEnabled(true);
            }
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
                grid.PerformCallback();
                canCallBack = false;
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
            cCmbScheme.ShowDropDown();
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
                if (salesInvoice == "Yes") {
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

            }


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
        function ReceiptGotFocus(s, e) {
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
            //var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            var Type = "Advance";
            var DocumentNo = (grid.GetEditor('DocumentID').GetValue() != null) ? grid.GetEditor('DocumentID').GetValue() : "0";
            var IsOpening = (grid.GetEditor('IsOpening').GetValue() != null) ? grid.GetEditor('IsOpening').GetValue() : "0";
            cacpCheckAmount.PerformCallback(Type + '~' + DocumentNo + '~' + IsOpening);
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
                var RecieveValue = (grid.GetEditor('Receipt').GetValue() != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";
                var receiptValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0";

                if (RecieveValue > 0) {
                    recalculatePayment(grid.GetEditor('Payment').GetValue());
                    grid.GetEditor('Payment').SetValue("0");
                    calculateNetAmountAndCharges(RecieveValue);
                }
            }
        }
        function calculateNetAmountAndCharges(RecieveValue) {
            if (RecieveValue > 0) {
                grid.GetEditor('NetAmount').SetValue(RecieveValue);
                grid.GetEditor('TaxableAmount').SetValue(RecieveValue);
                grid.GetEditor('TaxAmount').SetValue("0");
            }
        }

        function ReceiptTextChange(s, e) {
            ReceiptLostFocus(s, e);
            var RecieveValue = (grid.GetEditor('Receipt').GetValue() != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";

            var receiptValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0";

            if (RecieveValue > 0) {
                recalculatePayment(grid.GetEditor('Payment').GetValue());
                grid.GetEditor('Payment').SetValue("0");
                calculateNetAmountAndCharges(RecieveValue);
            }
            //var Mode = document.getElementById('hdn_Mode').value;
            //if (Mode != 'Edit')
            //{

            //}
            //..................CheckAmount.......................
            //var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
            //var Type = "Advance";
            //var DocumentNo = (grid.GetEditor('DocumentID').GetValue() != null) ? grid.GetEditor('DocumentID').GetValue() : "0";
            //var IsOpening = (grid.GetEditor('IsOpening').GetValue() != null) ? grid.GetEditor('IsOpening').GetValue() : "0";
            //cacpCheckAmount.PerformCallback(Type + '~' + DocumentNo + '~' + IsOpening);
            //.................End.........................
        }
        function aacpCheckAmountEndCall(s, e) {
            if (cacpCheckAmount.cpUnPaidAmount) {
                if (cacpCheckAmount.cpUnPaidAmount != null) {

                    var RecieveValue = (parseFloat(grid.GetEditor('Receipt').GetValue()) != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";
                    var PaymentValue = (parseFloat(grid.GetEditor('Payment').GetValue()) != null) ? parseFloat(grid.GetEditor('Payment').GetValue()) : "0";
                    var UnPaidAmoun = parseFloat(cacpCheckAmount.cpUnPaidAmount);
                    if (RecieveValue > UnPaidAmoun) {
                        jAlert('Receipt amount cannot be more then the selected Document Amount.', 'Alert', function () {

                            var vouchertype = "R";
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

                            var vouchertype = "R";
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
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            var CashBank = cddlCashBank.GetValue();
            var VoucherType = "R";
            if (type == 'S') {
                if (CashBank == null) {
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
                            return false;
                        }
                    }
                }
            }
            var VoucherAmount = ctxtVoucherAmount.GetValue();
            if (VoucherAmount == "0.0") {
                // $("#MandatoryVoucherAmount").show();
                jAlert("Voucher amount must be greater then ZERO.");
                return false;
            }
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            if (type == 'M') {

                var totalAmount = GetPaymentTotalEnteredAmount();
                var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());
                if (totalAmount != VoucherAmount) {
                    jAlert("Voucher amount And Multiple Payment amount must be Same.");
                    return false;
                }

                if (VoucherType == 'R') {
                    var EnteredCashAmount = parseFloat($('#cmbUcpaymentCashLedgerAmt').val());
                    if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                        jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                        return false;
                    }

                }


            }



            //Code added by Sudip
            grid.batchEditApi.EndEdit();
            var gridCount = grid.GetVisibleRowsOnPage();

            var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
            var txtTotalPayment = ctxtTotalPayment.GetValue() != null ? ctxtTotalPayment.GetValue() : 0;

            var VoucherType = "R";
            var IsType = "Y";
            var frontRow = 0;
            var backRow = -1;

            //for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
            //    var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Type')) : "";
            //    var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Type')) : "";
            //    if (frontProduct != "" || backProduct != "") {
            //        IsType = "Y";
            //        break;
            //    }
            //    backRow--;
            //    frontRow++;
            //}
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
            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            var CashBank = cddlCashBank.GetValue();
            var VoucherType = "R";
            var VoucherAmount = ctxtVoucherAmount.GetValue();
            if (type == 'S') {
                if (CashBank == null) {
                    $("#MandatoryCashBank").show();
                    return false;
                }
                var comboitem = cComboInstrumentTypee.GetValue('CH');
                if (VoucherType == 'R') {
                    if (comboitem == 'CH') {
                        var EnteredCashAmount = parseFloat(VoucherAmount);
                        if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                            jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                            return false;
                        }
                    }
                }
            }

            if (VoucherAmount == "0.0") {
                // $("#MandatoryVoucherAmount").show();
                jAlert("Voucher amount must be greater then ZERO.");
                return false;
            }

            var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
            if (type == 'M') {

                var totalAmount = GetPaymentTotalEnteredAmount();
                var VoucherAmount = parseFloat(ctxtVoucherAmount.GetValue());
                if (totalAmount != VoucherAmount) {
                    jAlert("Voucher amount And Multiple Payment amount must be Same.");
                    return false;
                }
                if (VoucherType == 'R') {
                    var EnteredCashAmount = parseFloat($('#cmbUcpaymentCashLedgerAmt').val());
                    if (CustomerCurrentDateAmount + EnteredCashAmount >= 200000) {
                        jAlert("Cannot Receive more than  1,99,999.00 on a single day.");
                        return false;
                    }
                }


            }

            //Code added by Sudip
            grid.batchEditApi.EndEdit();
            var gridCount = grid.GetVisibleRowsOnPage();

            var txtTotalAmount = c_txt_Debit.GetValue() != null ? c_txt_Debit.GetValue() : 0;
            var txtTotalPayment = ctxtTotalPayment.GetValue() != null ? ctxtTotalPayment.GetValue() : 0;

            var IsType = "Y";
            var frontRow = 0;
            var backRow = -1;

            //for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
            //    var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'Type')) : "";
            //    var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'Type') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'Type')) : "";
            //    if (frontProduct != "" || backProduct != "") {
            //        IsType = "Y";
            //        break;
            //    }
            //    backRow--;
            //    frontRow++;
            //}
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

                var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
                $('#<%=hdnRefreshType.ClientID %>').val('');
                $('#<%=hdnDeleteSrlNo.ClientID %>').val(SrlNo);
                var noofvisiblerows = grid.GetVisibleRowsOnPage();

                if (IsTagged != "Y") {
                    if (grid.GetVisibleRowsOnPage() > 1) {
                        grid.batchEditApi.StartEdit(e.visibleIndex, 1);
                        var ReceiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0.0";
                        var PaymentValue = (grid.GetEditor('Payment').GetValue() != null) ? grid.GetEditor('Payment').GetValue() : "0.0";
                        grid.DeleteRow(e.visibleIndex);
                        var hdnValue = document.getElementById('hdndocumentno').value;
                        var DocumentID = (grid.GetEditor('DocumentID').GetValue() != null) ? grid.GetEditor('DocumentID').GetValue() : "0";
                        //document.getElementById('hdndocumentno').value = (',' + hdnValue).replace(',' + DocumentID + ',' + /,/g, '');
                        document.getElementById('hdndocumentno').value = (',' + hdnValue).replace(',' + DocumentID + ',', '');
                        if (ReceiptValue != "0.0") {
                            var VoucherAmount = ctxtVoucherAmount.GetValue();
                            ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                            c_txt_Debit.SetValue(parseFloat(VoucherAmount) - parseFloat(ReceiptValue));
                        }
                        if (PaymentValue != "0.0") {
                            var VoucherAmount = ctxtVoucherAmount.GetValue();
                            ctxtVoucherAmount.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue));
                            ctxtTotalPayment.SetValue(parseFloat(VoucherAmount) - parseFloat(PaymentValue));
                        }
                        if (noofvisiblerows != "1") {
                            grid.DeleteRow(e.visibleIndex);

                            $('#<%=hdfIsDelete.ClientID %>').val('D');
                            grid.UpdateEdit();
                            grid.PerformCallback('Display');

                            $('#<%=hdnPageStatus.ClientID %>').val('delete');
                        }
                    }
                }
                else {
                    jAlert("Cann't Delete Invoice.");
                }
            }
            if (e.buttonID == 'AddNew') {
                //var TypeValue = (grid.GetEditor('Type').GetText() != null) ? grid.GetEditor('Type').GetText() : "0";
                var TypeValue = "Advance";
                if (TypeValue != "") {
                    if (TypeValue == 'Advance') {
                        //var Receipttype = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
                        //if (Receipttype != 'M') {

                        OnAddNewClick();
                        //}
                    }
                    else {
                        OnAddNewClick();
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
            if (grid.cpBtnVisible != null && grid.cpBtnVisible != "") {
                grid.cpBtnVisible = null;
                BtnVisible();
            }
            if (grid.cpMulType != null && grid.cpMulType != "") {
                grid.cpMulType = null;
                //var type = ($("[id$='rdl_MultipleType']").find(":checked").val() != null) ? $("[id$='rdl_MultipleType']").find(":checked").val() : "";
                //$("[id$='rdl_MultipleType']").prop("checked", true);
                $('#<%=rdl_MultipleType.ClientID %>').find("input[value='M']").prop("checked", true);
                $('#tdCashBankLabel').hide();
                $('#Multipletype').show();
                $('#singletype').hide();

            }
            if (grid.cpTotalAmount != null) {
                var total_receipt = grid.cpTotalAmount.split('~')[0];
                var total_payment = grid.cpTotalAmount.split('~')[1];
                //var total_payment = grid.cpTotalAmount;

                c_txt_Debit.SetValue(total_receipt);
                ctxtTotalPayment.SetValue(total_payment);
                //  ctxtVoucherAmount.SetValue(total_payment);
                grid.cpTotalAmount = null;
            }
            var value = document.getElementById('hdnRefreshType').value;
            var pageStatus = document.getElementById('hdnPageStatus').value;
            var IsInvoiceTagged = document.getElementById('IsInvoiceTagged').value;
            var InvoicepageStatus = document.getElementById('hdnInvoicePageStatus').value;

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
                grid.batchEditApi.StartEdit(0);
                //OnAddNewClick();
                //grid.AddNewRow();

                grid.cpSaveSuccessOrFail = null;
                jAlert('Please fill Document');
            }
            else if (grid.cpSaveSuccessOrFail == "nullReceiptPayment") {
                //OnAddNewClick();
                grid.batchEditApi.StartEdit(0);
                //OnAddNewClick();
                // grid.AddNewRow();

                grid.cpSaveSuccessOrFail = null;
                jAlert('Please enter Amount to save this entry.');
            }
            else if (grid.cpSaveSuccessOrFail == "BSMandatory") {
                grid.AddNewRow();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Billing/Shipping is mandatory', "Alert", function () {
                    page.SetActiveTabIndex(1);
                    cbsSave_BillingShipping.Focus();
                    page.tabs[0].SetEnabled(false);
                    $("#divcross").hide();
                });

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
                                if (IsInvoiceTagged == "Y") {
                                    if (InvoicepageStatus == "E") {
                                        parent.CRP_SaveANDExit_Press();
                                    }
                                    else if (InvoicepageStatus == "N") {
                                        parent.CRP_SaveANDNew_Press();
                                    }
                                    else {
                                        window.parent.capcReciptPopup.Hide();
                                        //window.location.assign("CustomerAdvancedReceiptList.aspx");
                                    }
                                }
                                else {
                                    grid.cpVouvherNo = null;
                                    window.location.assign("CustomerAdvancedReceiptList.aspx");
                                }
                            }
                        });
                    }
                    else {
                        window.location.assign("CustomerAdvancedReceiptList.aspx");
                    }
                }
                else if (value == "N") {
                    if (Voucher_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [Customer Advanced Receipt]', function (r) {
                            grid.cpVouvherNo = null;
                            if (r == true) {
                                ctxtVoucherAmount.SetValue("0.0");
                                c_txt_Debit.SetValue("0.0");
                                window.location.assign("CustomerAdvanceReceipt.aspx?key=ADD");
                                //window.location.href = '/OMS/Management/Activities/CustomerAdvanceReceipt.aspx?key=ADD';
                            }
                        });
                    }
                    else {
                        ctxtVoucherAmount.SetValue("0.0");
                        c_txt_Debit.SetValue("0.0");
                        window.location.assign("CustomerAdvanceReceipt.aspx?key=ADD");
                        //window.location.href = '/OMS/Management/Activities/CustomerAdvanceReceipt.aspx?key=ADD';
                    }
                }
                else {
                    if (pageStatus == "first") {
                        OnAddNewClick();
                        VisibleColumn();
                        grid.batchEditApi.EndEdit();

                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        var VoucherType = "R";

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

                        if (document.getElementById('isFromTab').value == "1") {
                            grid.batchEditApi.StartEdit(-1);
                            grid.GetEditor("Type").SetValue("Advance");
                            var basketAmount = document.getElementById('HdBasketTotalAmount').value;
                            grid.GetEditor("Receipt").SetValue(basketAmount);
                        }



                    }
                    else if (pageStatus == "update") {
                        // OnAddNewClick();

                        // grid.StartEditRow(0);
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
                        var VoucherType = "R";

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
}
}
    if (grid.cpView == "1") {
        viewOnly();
    }
    if (grid.cpUnpaidAmountEqual == "1") {
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
    var VoucherType = "R";
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
    //grid.AddNewRow();
    grid.AddNewRow();

    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var tbQuotation = grid.GetEditor("SrlNo");
    tbQuotation.SetValue(noofvisiblerows);

}
function GridCallBack() {
    //grid.StartEditRow(0);
    grid.PerformCallback('Display');
}
//...............End Batch Grid..................
function changeFocusDate() {

    if (DateChange == "Y") {
        jAlert('Cannot change the date as tagged document date is after the date you have entered now.', 'Alert', function () {

            var TransDate = document.getElementById('hdnDate').value;
            var strDate = new Date(TransDate);

            cdtTDate.SetDate(strDate);
            GetContactPerson();
        });
    }
    //$('#ddlBranch').focus();
}
function CmbScheme_ValueChange() {
    deleteAllRows();
    var val = cCmbScheme.GetValue();
    $.ajax({
        type: "POST",
        url: 'CustomerAdvanceReceipt.aspx/getSchemeType',
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
                document.getElementById('HdSelectedBranch').value = branchID;

                document.getElementById('<%= ddlBranch.ClientID %>').disabled = true;

                var IsTagged = document.getElementById('hdnTaggedFrom').value;
                if (IsTagged != "I") {
                    cddlCashBank.PerformCallback(branchID);
                }
            }
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
            url: "CustomerAdvanceReceipt.aspx/GetRate",
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
function deleteAllRows() {
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
        grid.DeleteRow(frontRow);
        grid.DeleteRow(backRow);
        backRow--;
        frontRow++;
    }
    //grid.AddNewRow();
    OnAddNewClick();

    ctxtTotalPayment.SetValue(0);
    c_txt_Debit.SetValue(0);
    ctxtVoucherAmount.SetValue(0);
}
function GetContactPerson(e) {
    deleteAllRows();
    $('#MandatorysCustomer').hide();
    var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
    var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
    if (key != null && key != '') {
        var startDate = new Date();
        var TransDate = cdtTDate.GetDate();
        var branch = $("#ddlBranch").val();
        cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + cdtTDate.GetDate().format('yyyy-MM-dd') + '~' + branch);
        // cContactPerson.PerformCallback('BindContactPerson~' + key);
        page.GetTabByName('Billing/Shipping').SetEnabled(true);

        //###### Added By : Samrat Roy ##########
        //New Code
        //    LoadCustomerAddress(key, $('#ddlBranch').val(), 'CRP');
        //    if ($('#hfBSAlertFlag').val() == "1") {
        //        jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
        //            if (r == true) {
        //                page.tabs[0].SetEnabled(true);
        //            }
        //        });
        //    }
        //    else {
        //        page.tabs[0].SetEnabled(true);
        //    }

        //    //###### END : Samrat Roy : END ########## 
        //}
        //GetObjectID('hdnCustomerId').value = key;
        LoadCustomerAddress(key, $('#ddl_Branch').val(), 'CRP');
        GetObjectID('hdnCustomerId').value = key;
        if ($('#hfBSAlertFlag').val() == "1") {
            jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    page.SetActiveTabIndex(1);
                    cbsSave_BillingShipping.Focus();
                    page.tabs[0].SetEnabled(false);
                    $("#divcross").hide();
                }
            });
        }
        else {
            page.SetActiveTabIndex(1);
            cbsSave_BillingShipping.Focus();
            page.tabs[0].SetEnabled(false);
            $("#divcross").hide();
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
    </script>
    <%--Batch Product Popup Start--%>
    <script>
        function ProductKeyDown(s, e) {
            // console.log(e.htmlEvent.key);
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {

                s.OnButtonClick(0);
            }
        }
    </script>
    <%--Batch Product Popup End--%>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section--%>
    <script>
        function disp_prompt(name) {

            if (name == "tab0") {
                gridLookup.Focus();
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
    <%--Tax Start--%>
    <script type="text/javascript">
        function taxAmtButnClick(s, e) {
            if (e.buttonIndex == 0) {

                var TaxType = cddl_AmountAre.GetValue();

                if (TaxType != null) {
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

                    if (ProductID.trim() != "") {

                        document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();
                        caspxTaxpopUp.Show();
                        //Set Product Gross Amount and Net Amount

                        //var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                        var SpliteDetails = ProductID.split("||@||");
                        var strMultiplier = SpliteDetails[7];
                        var strFactor = SpliteDetails[8];
                        //var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
                        //var strRate = "1";
                        var strStkUOM = SpliteDetails[4];
                        // var strSalePrice = SpliteDetails[6];
                        //var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                        //if (strRate == 0) {
                        //    strRate = 1;
                        //}

                        //var StockQuantity = strMultiplier * QuantityValue;
                        //var Amount = Math.round(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                        var Amount = (Math.round(grid.GetEditor('Receipt').GetValue() * 100) / 100).toFixed(2);
                        //var Amount = Math.round(grid.GetEditor('Receipt').GetValue()).toFixed(2);
                        clblTaxProdGrossAmt.SetText(Amount);
                        //clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue()).toFixed(2));
                        clblProdNetAmt.SetText(Amount);
                        document.getElementById('HdProdGrossAmt').value = Amount;
                        document.getElementById('HdProdNetAmt').value = Amount;//Math.round(grid.GetEditor('Receipt').GetValue()).toFixed(2);

                        //End Here

                        //Set Discount Here
                        //if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                        //    var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
                        //    clblTaxDiscount.SetText(discount);
                        //}
                        //else {
                        //    clblTaxDiscount.SetText('0.00');
                        //}
                        //End Here 


                        //Checking is gstcstvat will be hidden or not
                        if (TaxType == "2") {
                            $('.GstCstvatClass').hide();
                            //$('.gstGrossAmount').show();
                            clblTaxableGross.SetText("(Taxable)");
                            clblTaxableNet.SetText("(Taxable)");
                            //$('.gstNetAmount').show();
                            //Set Gross Amount with GstValue
                            //Get The rate of Gst
                            //var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
                            //if (gstRate) {
                            //    if (gstRate != 0) {
                            //        var gstDis = (gstRate / 100) + 1;
                            //        if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                            //            $('.gstNetAmount').hide();
                            //            clblTaxProdGrossAmt.SetText(Math.round(Amount / gstDis).toFixed(2));
                            //            document.getElementById('HdProdGrossAmt').value = Math.round(Amount / gstDis).toFixed(2);
                            //            clblGstForGross.SetText(Math.round(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                            //            clblTaxableNet.SetText("");
                            //        }
                            //        else {
                            //            $('.gstGrossAmount').hide();
                            //            clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2));
                            //            document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2);
                            //            clblGstForNet.SetText(Math.round(grid.GetEditor('Amount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
                            //            clblTaxableGross.SetText("");
                            //        }
                            //    }


                            //} else {
                            //    $('.gstGrossAmount').hide();
                            //    $('.gstNetAmount').hide();
                            //    clblTaxableGross.SetText("");
                            //    clblTaxableNet.SetText("");
                            //}
                        }
                        else if (TaxType == "1") {
                            $('.GstCstvatClass').show();
                            $('.gstGrossAmount').hide();
                            $('.gstNetAmount').hide();
                            clblTaxableGross.SetText("");
                            clblTaxableNet.SetText("");

                            var shippingStCode = '';

                            ////###### Added By : Samrat Roy ##########
                            //Get Customer Shipping StateCode
                            var shippingStCode = '';

                            var shippingStCode = '';
                            shippingStCode = cbsSCmbState.GetText();
                            shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                            ////// ###########  Old Code #####################
                            ////if (cchkBilling.GetValue()) {
                            ////    shippingStCode = CmbState.GetText();
                            ////}
                            ////else {
                            ////    shippingStCode = CmbState1.GetText();
                            ////}
                            ////shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                            ////###### END : Samrat Roy : END ########## 

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
                            cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + TaxType);
                        } else {

                            cgridTax.PerformCallback('New~' + TaxType);
                            //Set default combo
                            cgridTax.cpComboCode = grid.GetEditor('ProductID').GetValue().split('||@||')[9];
                        }

                        ctxtprodBasicAmt.SetValue(grid.GetEditor('Receipt').GetValue());
                    } else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 13);
                    }
                }
                else {
                    jAlert("Please provide Amounts are", "Alert", function () {
                        cddl_AmountAre.SetFocus();
                    })
                }
            }
        }
        function taxAmtButnClick1(s, e) {
            //console.log(grid.GetFocusedRowIndex());
            rowEditCtrl = s;
        }
        function TaxAmountKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }
        function PopulateGSTCSTVAT(e) {
            var key = cddl_AmountAre.GetValue();
            //deleteAllRows();

            if (key == 1) {

                grid.GetEditor('TaxAmount').SetEnabled(true);
                //cddlVatGstCst.SetEnabled(false);
                //cddlVatGstCst.PerformCallback('1');
                //cddlVatGstCst.SetSelectedIndex(0);
                //cbtn_SaveRecords.SetVisible(true);
                grid.GetEditor('ProductID').Focus();
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }

            }
            else if (key == 2) {
                grid.GetEditor('TaxAmount').SetEnabled(true);

                //cddlVatGstCst.SetEnabled(true);
                //cddlVatGstCst.PerformCallback('2');
                //cddlVatGstCst.Focus();
                //cbtn_SaveRecords.SetVisible(true);
            }
            else if (key == 3) {

                grid.GetEditor('TaxAmount').SetEnabled(false);

                //cddlVatGstCst.PerformCallback('3');
                //cddlVatGstCst.SetSelectedIndex(0);
                //cddlVatGstCst.SetEnabled(false);
                //cbtn_SaveRecords.SetVisible(false);
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }


            }

        }
        function SetFocusonDemand(e) {
            var key = cddl_AmountAre.GetValue();
            if (key == '1' || key == '3') {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            }
            else if (key == '2') {
                //cddlVatGstCst.Focus();
            }

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
        function GetTotalRunningAmount() {
            var runningTot = parseFloat(clblProdNetAmt.GetValue());
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.batchEditApi.EndEdit();
            }

            return runningTot;
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
        var taxJson;
        function cgridTax_EndCallBack(s, e) {
            var taxType = cddl_AmountAre.GetValue();
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
                    if (taxType == "1") {
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
                grid.GetEditor("TaxAmount").SetValue(totAmt);
                // grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));
                var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("Receipt").GetValue());
                var totalRoundOffAmount = Math.round(totalNetAmount);
                //grid.GetEditor("NetAmount").SetValue(totalRoundOffAmount);
                //grid.GetEditor("Receipt").SetValue(parseFloat(grid.GetEditor("Receipt").GetValue()) + (totalRoundOffAmount - totalNetAmount));

                var totalTax = parseFloat(ctxtTaxTotAmt.GetText());
                var RecieveValue = (grid.GetEditor('Receipt').GetValue() != null) ? parseFloat(grid.GetEditor('Receipt').GetValue()) : "0";

                if (cddl_AmountAre.GetValue() == "1") //Exclusive
                {
                    grid.GetEditor('NetAmount').SetValue(totalNetAmount);
                }
                else if (cddl_AmountAre.GetValue() == "2") //Inclusive
                {
                    grid.GetEditor('NetAmount').SetValue(parseFloat(RecieveValue) - totalTax);
                }
                else //No Tax
                {
                    //do nothing
                }

                //var finalNetAmount = parseFloat(grid.GetEditor("NetAmount").GetValue());
                //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
                //cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));


            }

            if (cgridTax.GetVisibleRowsOnPage() == 0) {
                $('.cgridTaxClass').hide();
                ccmbGstCstVat.Focus();
            }
            //Debjyoti Check where any Gst Present or not
            // If Not then hide the hole section

            SetRunningTotal();
            //ShowTaxPopUp("IY");
            RecalCulateTaxTotalAmountInline();
        }
        function recalculateTax() {
            cmbGstCstVatChange(ccmbGstCstVat);
        }
        function recalculateTaxCharge() {
            ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
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
        function GetTaxVisibleIndex(s, e) {
            globalTaxRowIndex = e.visibleIndex;
        }
        function taxAmountGotFocus(s, e) {
            taxAmountGlobal = parseFloat(s.GetValue());
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

            ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
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
    </script>
    <%--Tax End--%>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeadTitle" Text="Add Customer Advance Receipt" runat="server"></asp:Label>
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
                <div id="divcross" class="crossBtn" style="margin-left: 100px;"><a href="CustomerAdvancedReceiptList.aspx"><i class="fa fa-times"></i></a></div>
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
                                                    <ClientSideEvents GotFocus="TransDate_GotFocus" DateChanged="changeFocusDate"></ClientSideEvents>
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
                                                <i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i>
                                            </label>
                                            <dxe:ASPxCallbackPanel runat="server" ID="CustomerCallBackPanel" ClientInstanceName="cCustomerCallBackPanel"
                                                OnCallback="CustomerCallBackPanel_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" ClientInstanceName="gridLookup"
                                                            KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">
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
                                                    </dxe:PanelContent>
                                                </PanelCollection>
                                                <ClientSideEvents EndCallback="CustomerCallBackPanelEndCallBack" />
                                            </dxe:ASPxCallbackPanel>
                                            <span id="MandatorysCustomer" class="iconCustomer pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                        <div class="col-md-3 lblmTop8">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person" CssClass="inline">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px" ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <div class="col-md-2 lblmTop8" id="tdCashBankLabel">
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
                                                    TextField="Currency_AlphaCode" ValueField="Currency_ID" SelectedIndex="0"
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


                                        <div class="col-md-4 lblmTop8">
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
                                                    <%--<ClientSideEvents LostFocus="function(s, e) { GetInvoiceMsg(e)}" />--%>
                                                    <ClientSideEvents TextChanged="function(s, e) { GetInvoiceMsg(e)}" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                                </dxe:ASPxTextBox>
                                                <span id="MandatoryVoucherAmount" class="iconVoucherAmount pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-4">
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientInstanceName="cddl_AmountAre" Width="100%">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" LostFocus="function(s, e) { SetFocusonDemand(e)}"
                                                        GotFocus="function(s,e){cddl_AmountAre.ShowDropDown();}" />
                                            </dxe:ASPxComboBox>

                                        </div>
                                    </div>

                                    <div class="clear"></div>
                                    <div id="Multipletype" style="display: none" class="bgrnd">
                                        <uc1:ucPaymentDetails runat="server" ID="PaymentDetails" />
                                    </div>

                                </div>
                                
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

                                        <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="2" Width="14%">
                                            <PropertiesButtonEdit>
                                                <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" />
                                                <Buttons>
                                                    <dxe:EditButton Text="..." Width="20px">
                                                    </dxe:EditButton>
                                                </Buttons>
                                            </PropertiesButtonEdit>
                                        </dxe:GridViewDataButtonEditColumn>
                                       
                                        <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="3" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                                            <%--<PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                <ClientSideEvents GotFocus="ProductsGotFocus" LostFocus="QuantityTextChange" />
                                                <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                <Style HorizontalAlign="Right">
                                                </Style>
                                            </PropertiesTextEdit>--%>
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Taxable Amount" FieldName="Receipt" Width="130">
                                            <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                <%--<MaskSettings Mask="<0..999999999999999999>.<0..99>" />--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                <ClientSideEvents KeyDown="OnKeyDown" LostFocus="ReceiptLostFocus"
                                                    GotFocus="function(s,e){
                                                        ReceiptGotFocus(s,e); 
                                                        }" />
                                                <ClientSideEvents />
                                                <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Tax/Charges" VisibleIndex="5" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataButtonEditColumn>

                                        <dxe:GridViewDataTextColumn  Caption="Net Amount" FieldName="NetAmount" Width="130" VisibleIndex="6">
                                            <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Remarks" FieldName="Remarks" Width="200">
                                            <PropertiesTextEdit>
                                                <%-- <ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Taxable Amount" FieldName="TaxableAmount" Width="0">
                                            <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn  Caption="Payment" FieldName="Payment" Width="0">
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

                                        <dxe:GridViewDataTextColumn FieldName="ReceiptDetail_ID" Caption="Srl No" ReadOnly="true" Width="0">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="IsOpening" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                            <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="9" Caption=" ">
                                            <CustomButtons>
                                                <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                </dxe:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                        </dxe:GridViewCommandColumn>
                                         <dxe:GridViewDataButtonEditColumn FieldName="DocumentNo" Caption="Document Number" Width="0">
                                            <PropertiesButtonEdit>
                                                <Buttons>
                                                    <dxe:EditButton Text="..." Width="20px">
                                                    </dxe:EditButton>
                                                </Buttons>
                                            </PropertiesButtonEdit>
                                        </dxe:GridViewDataButtonEditColumn>
                                        <dxe:GridViewDataTextColumn FieldName="DocumentID" Caption="hidden Field Id" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                            <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="ProductID" PropertiesTextEdit-ValidationSettings-ErrorImage-IconID="ghg" Caption="hidden Field Id" VisibleIndex="21" ReadOnly="True" Width="0" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="abcd">
                                                        <PropertiesTextEdit Height="15px">
                                                            <ValidationSettings>
                                                                <ErrorImage IconID="ghg">
                                                                </ErrorImage>
                                                            </ValidationSettings>
                                                            <Style CssClass="abcd">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle Wrap="True" CssClass="abcd"></CellStyle>
                                     </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="GetVisibleIndex" />
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
                                            <td style="padding-right: 50px">
                                                <asp:Label ID="lbltaxAmountHeader" runat="server" Text="Total Taxable Amount" Font-Bold="true"></asp:Label></td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTaxAmount" runat="server" Width="105px" ClientInstanceName="ctxtTaxAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                    <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
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
                                            <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" UseSubmitBehavior="False"
                                                CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                                <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                            </dxe:ASPxButton>
                                            <b><span id="tagged" style="display: none; color: red">This Customer Receipt Payment is tagged in other modules. Cannot Modify data except UDF</span></b>
                                        </td>


                                    </tr>
                                </table>

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
            <asp:HiddenField ID="hdnDeleteSrlNo" runat="server" />
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
        <%--Batch Product Popup Start--%>
            <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <HeaderTemplate>
                    <span>Select Product</span>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By product Name</strong></label>
                        <span style="color: red;">[Press ESC key to Cancel]</span>
                        <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                            KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="Products_Description" Caption="Name" Width="240">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="0">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="80">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="100">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="220">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="140">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120" Visible="false">
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

                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>
            <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="prc_CRPProductDetailsList" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                <%--<asp:Parameter Type="String" Name="InventoryType" DefaultValue="C" />
                <asp:SessionParameter Name="campany_Id" SessionField="LastCompany" Type="String" />
                <asp:SessionParameter Type="String" Name="FinYear" SessionField="LastFinYear" />--%>
            </SelectParameters>
            </asp:SqlDataSource>
            <%--<asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="prc_CRMSalesInvoice_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="AllProductDetails" />
                    <asp:ControlParameter DefaultValue="Y" Name="InventoryType" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddlInventory" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>--%>

            <%--Batch Product Popup End--%>
        <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
                Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                <HeaderTemplate>
                    <span style="color: #fff"><strong>Select Tax</strong></span>
                    <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
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
                                                <td>Gross Amount
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

                            <div class="col-sm-3 gstGrossAmount" style="display:none;">
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

                            <div class="col-sm-3" style="display:none;">
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


                            <div class="col-sm-3">
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

                            <div class="col-sm-2 gstNetAmount" style="display:none;">
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
                        <div id="ContentErrorMsg" style="display:none;">
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
                                    <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" TabIndex="1" ReadOnly="true"
                                        runat="server" Width="50%">
                                        <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                            <tr class="cgridTaxClass">
                                <td colspan="3">
                                    <dxe:ASPxGridView runat="server" OnBatchUpdate="taxgrid_BatchUpdate" KeyFieldName="Taxes_ID" ClientInstanceName="cgridTax" ID="aspxGridTax"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridTax_CustomCallback"
                                        Settings-ShowFooter="false" AutoGenerateColumns="False" OnCellEditorInitialize="aspxGridTax_CellEditorInitialize" OnHtmlRowCreated="aspxGridTax_HtmlRowCreated"
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
                                        <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
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
                                                <dxe:ASPxComboBox ID="cmbGstCstVat" ClientInstanceName="ccmbGstCstVat" runat="server" SelectedIndex="-1" TabIndex="2"
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
                                                <dxe:ASPxTextBox ID="txtGstCstVat" MaxLength="80" ClientInstanceName="ctxtGstCstVat" TabIndex="3" ReadOnly="true" Text="0.00"
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
                                        <asp:Button ID="Button1" runat="server" Text="Ok" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
                                        <asp:Button ID="Button2" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
                                    </div>
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                                    runat="server" Width="100%" CssClass="pull-left mTop">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
        <asp:HiddenField runat="server" ID="HdBasketTotalAmount" />
        <asp:HiddenField runat="server" ID="isFromTab" />
        <asp:HiddenField runat="server" ID="hdnSalesInvoice" />
</asp:content>
