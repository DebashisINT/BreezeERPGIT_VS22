<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OldUnitReceivedFromServiceCenter.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.OldUnitReceivedFromServiceCenter" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 1.5;
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

        .validclass {
            position: absolute;
            right: -4px;
            top: 24px;
        }

        #content-5 .lblHolder {
            height: auto !important;
        }

        .mandt {
            position: absolute;
            right: -18px;
            top: 4px;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            //debugger;
            //$("#hfDocId").val("POS/00000000015/1718");
            //$("#hfDocType").val("OldUnit");
            grid.GetEditor('sProducts_Name').SetEnabled(false);
            grid.GetEditor('sProducts_Description').SetEnabled(false);
            grid.GetEditor('oldUnit_qty').SetEnabled(false);
            grid.GetEditor('oldUnit_value').SetEnabled(false);
            grid.GetEditor('UOM_Name').SetEnabled(false);
            
        });
    </script>
    <script type="text/javascript">

        $(function () {
            var InvoiceDetails = getUrlVars();


            if (InvoiceDetails["rcv_no"] != '') {
                document.getElementById('ddl_numbering').style.display = "block";
            }
            else {
                document.getElementById('ddl_numbering').style.display = "none";
            }

            //Ctxt_InvoiceNo.SetText(InvoiceDetails["Invoice_Number"]);
            Ctxt_InvoiceNo.SetEnabled(false);
            Ctxt_InvoiceNo.Focus();

            if (InvoiceDetails["ViewMode"] == 'yes') {
                cbtn_SaveRecords.SetVisible(false);
                $("#span_viewmodemsg").show();
                $("#ddl_numbering").hide();
                txt_Refference.SetEnabled(false);
                grid.SetEnabled(false);

                cbtnWarehouseSave.SetVisible(false);
                ctxtserialID.SetEnabled(false);

            }
            else {
                cbtn_SaveRecords.SetVisible(true);
                $("#span_viewmodemsg").hide();
                $("#ddl_numbering").show();
                txt_Refference.SetEnabled(true);
                grid.SetEnabled(true);

                cbtnWarehouseSave.SetVisible(true);
                ctxtserialID.SetEnabled(true);
            }

            $("#ddl_numberingScheme").change(function () {
                grid.batchEditApi.StartEdit(0, 1);
            });
        });
        function txt_InvoiceNoInit() {
            //alert(getUrlVars()["key"]);
            grid.PerformCallback('BindGridStockInvoiceDetails' + '~' + getUrlVars()["key"]);
        }

        function Quantity_Received_TextChanged() {
            var qty = grid.GetEditor('oldUnit_qty');
            var balance_qty = grid.GetEditor('Balance_Quantity');
            var rcv_qty = grid.GetEditor('Quantity_Received');

            var TOT_QTY = qty.GetValue();
            var rcv_qty_val = rcv_qty.GetValue();

            if (parseFloat(TOT_QTY) > parseFloat(rcv_qty_val)) {
                var balance = qty.GetValue() - rcv_qty.GetValue();
                balance_qty.SetValue(balance);
            }
            else {
                var rcv = qty.GetValue();
                rcv_qty.SetValue(rcv);
                balance_qty.SetValue('0');

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
            console.log(vars);
            return vars;
        }

    </script>
    <script>
        var isCtrl = false;
        document.onkeyup = function (e) {
            if (event.keyCode == 17) {
                isCtrl = false;
            }
            else if (event.keyCode == 27) {
                btnCancel_Click();
            }
        }

        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;
            if (event.keyCode == 78 && isCtrl == true) { //run code for alt+N -- ie, Save & New  
                StopDefaultAction(e);
                Save_ButtonClick();
            }
            else if ((event.keyCode == 120 || event.keyCode == 88) && isCtrl == true) { //run code for Ctrl+X -- ie, Save & Exit!     
                StopDefaultAction(e);
                SaveExit_ButtonClick();
            }
        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
    </script>
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

        function fn_Edit(keyValue) {

            SelectedWarehouseID = keyValue;

            ctxtQuantity.SetValue("0");
            ctxtBatchName.SetValue("");
            ctxtStartDate.SetDate(null);
            ctxtEndDate.SetDate(null);
            ctxtserialID.SetValue("");

            cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
        }
        function fn_Delete(keyValue) {
            cGrdWarehouse.PerformCallback('Delete~' + keyValue);
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

        function ProductSelected(s, e) {

            var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            var ProductCode = cproductLookUp.GetValue();
            if (!ProductCode) {
                LookUpData = null;
            }
            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("sProducts_Name").SetText(ProductCode);


            pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("sProducts_Description");
            var tbUOM = grid.GetEditor("oldUnit_Uom");
            var tbSalePrice = grid.GetEditor("oldUnit_value");
            //var tbStkUOM = grid.GetEditor("StockUOM");
            //var tbStockQuantity = grid.GetEditor("StockQuantity");

            var ProductID = LookUpData;
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];



            tbDescription.SetValue(strDescription);
            //tbUOM.SetValue(strUOM);
            //tbSalePrice.SetValue(strSalePrice);
            divPacking.style.display = "none";
            //grid.GetEditor("Quantity").SetValue("0.00");
            //grid.GetEditor("Discount").SetValue("0.00");
            //grid.GetEditor("Amount").SetValue("0.00");
            //grid.GetEditor("TaxAmount").SetValue("0.00");
            //grid.GetEditor("TotalAmount").SetValue("0.00");

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            cacpAvailableStock.PerformCallback(strProductID);
        }
    </script>

    <%--Batch Product Popup End--%>

    <%--Debu Section--%>
    <script type="text/javascript">
        <%--kaushik Section--%>


        function SalePriceTextChange(s, e) {
            pageheaderContent.style.display = "block";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetValue();
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            //var strRate = "1";
            var strStkUOM = SpliteDetails[4];
            //var strSalePrice = SpliteDetails[6];

            if (strRate == 0) {
                strRate = 1;
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

            var Amount = QuantityValue * strFactor * (Saleprice / strRate);
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(amountAfterDiscount);
            //var tbAmount = grid.GetEditor("Amount");
            //tbAmount.SetValue(Amount); 
            //var tbTotalAmount = grid.GetEditor("TotalAmount");
            //tbTotalAmount.SetValue(Amount); 
            //DiscountTextChange(s, e);
        }

        //'Subhabrata' on 15-03-2017
        function CmbWarehouseEndCallback(s, e) {
            if (SelectWarehouse != "0") {
                cCmbWarehouseID.SetValue(SelectWarehouse);
                SelectWarehouse = "0";
            }
            else {
                cCmbWarehouseID.SetEnabled(true);
            }
        }

        function LostFocusedPurpose(e) {

            if (grid.GetVisibleRowsOnPage() > 0) {
                grid.batchEditApi.StartEdit(0, 1);
            }
            else {
                grid.batchEditApi.StartEdit(-1, 1);
            }
        }
        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = dd + '-' + mm + '-' + yyyy;
            }

            return today;
        }
        function GetReverseDateFormat(today) {
            if (today != "") {
                var dd = today.substring(0, 2);
                var mm = today.substring(3, 5);
                var yyyy = today.substring(6, 10);

                today = mm + '-' + dd + '-' + yyyy;
            }

            return today;
        }
        function SubmitWarehouse() {

            var WarehouseID = (cCmbWarehouseID.GetValue() != null) ? cCmbWarehouseID.GetValue() : "0";
            var WarehouseName = (cCmbWarehouseID.GetText() != null) ? cCmbWarehouseID.GetText() : "";
            var BatchName = (ctxtBatchName.GetValue() != null) ? ctxtBatchName.GetValue() : "";
            var MfgDate = (ctxtStartDate.GetValue() != null) ? ctxtStartDate.GetValue() : "";
            var ExpiryDate = (ctxtEndDate.GetValue() != null) ? ctxtEndDate.GetValue() : "";
            var SerialNo = (ctxtserialID.GetValue() != null) ? ctxtserialID.GetValue() : "";
            var Qty = ctxtQuantity.GetValue();

            MfgDate = GetDateFormat(MfgDate);
            ExpiryDate = GetDateFormat(ExpiryDate);

            $("#spnCmbWarehouse").hide();
            $("#spntxtBatch").hide();
            $("#spntxtQuantity").hide();
            $("#spntxtserialID").hide();

            var Ptype = document.getElementById('hdfProductType').value;
            if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
                $("#spnCmbWarehouse").show();
            }
            else if ((Ptype == "B" && BatchName == "") || (Ptype == "WB" && BatchName == "") || (Ptype == "WBS" && BatchName == "") || (Ptype == "BS" && BatchName == "")) {
                $("#spntxtBatch").show();
            }
            else if ((Ptype == "W" && Qty == "0.0") || (Ptype == "B" && Qty == "0.0") || (Ptype == "WB" && Qty == "0.0")) {
                $("#spntxtQuantity").show();
            }
            else if ((Ptype == "S" && SerialNo == "") || (Ptype == "WS" && SerialNo == "") || (Ptype == "WBS" && SerialNo == "") || (Ptype == "BS" && SerialNo == "")) {
                $("#spntxtserialID").show();
            }
            else {
                if ((Ptype == "S" && SelectedWarehouseID == "0") || (Ptype == "WS" && SelectedWarehouseID == "0") || (Ptype == "WBS" && SelectedWarehouseID == "0") || (Ptype == "BS" && SelectedWarehouseID == "0")) {
                    ctxtserialID.SetValue("");
                    ctxtserialID.Focus();
                    IsFocus = "1";
                }
                else {
                    cCmbWarehouseID.PerformCallback('BindWarehouse');
                    ctxtQuantity.SetValue("0");
                    ctxtBatchName.SetValue("");
                    ctxtStartDate.SetDate(null);
                    ctxtEndDate.SetDate(null);
                    ctxtserialID.SetValue("");
                }

                cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + MfgDate + '~' + ExpiryDate + '~' + SerialNo + '~' + Qty + '~' + SelectedWarehouseID);
                SelectedWarehouseID = "0";
                SelectWarehouse = "0";
            }
        }
        function CmbBatchEndCall(s, e) {
            if (SelectBatch != "0") {
                cCmbBatch.SetValue(SelectBatch);
                SelectBatch = "0";
            }
            else {
                cCmbBatch.SetEnabled(true);
            }
        }

        function listBoxEndCall(s, e) {
            if (SelectSerial != "0") {
                var values = [SelectSerial];
                checkListBox.SelectValues(values);
                UpdateSelectAllItemState();
                UpdateText();
                //checkListBox.SetValue(SelectWarehouse);
                SelectSerial = "0";
                cCmbBatch.SetEnabled(false);
                cCmbWarehouseID.SetEnabled(false);
            }
        }
        function ctaxUpdatePanelEndCall(s, e) {
            if (ctaxUpdatePanel.cpstock != null) {
                divAvailableStk.style.display = "block";
                divpopupAvailableStock.style.display = "block";

                var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                <%--document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;--%>
                <%--document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;--%>

                ctaxUpdatePanel.cpstock = null;
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return false;
            }
        }
        //End
        function DiscountTextChange(s, e) {
            //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetValue();
            var SpliteDetails = ProductID.split("||@||");
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }
            if (strRate == 0) {
                strRate = 1;
            }
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(amountAfterDiscount);

            //Debjyoti 
            //grid.GetEditor('TaxAmount').SetValue(0);
            //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
        }
        <%--kaushik Section--%>

        function CmbScheme_ValueChange() {

            var val = $("#ddlDNnumbering").val();

            var branchID = (val.toString().split('~')[3] != null) ? val.toString().split('~')[3] : "";

            document.getElementById('ddlDnBranch').value = branchID;



            var schemetype = val.toString().split('~')[1];
            var schemelength = val.toString().split('~')[0];
            $('#txtDNnumber').attr('maxLength', schemelength);

            if (schemetype == '0') {

                        <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";--%>
                txtDNnumber.SetText('');
                txtDNnumber.SetEnabled(true);
                txtDNnumber.Focus();

            }
            else if (schemetype == '1') {

                        <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";--%>
                        txtDNnumber.SetText('Auto');
                        txtDNnumber.SetEnabled(false);
                    }
                    else if (schemetype == '2') {

                        <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";--%>
                    }
                    else if (schemetype == 'n') {
                        <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";--%>
                    }
            
        }



        function ddl_Currency_Rate_Change() {

            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var Currency_ID = $("#ddl_Currency").val();


            if ($("#ddl_Currency").text().trim() == basedCurrency[1]) {
                ctxtRate.SetValue("");
                ctxtRate.SetEnabled(false);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "SalesOrderAdd.aspx/GetRate",
                    data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var data = msg.d;
                        ctxtRate.SetValue(data);
                    }
                });
                ctxtRate.SetEnabled(true);
            }
        }
        function ProductlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }
        }
        function CmbWarehouse_ValueChange() {
            var WarehouseID = cCmbWarehouseID.GetValue();
            if (WarehouseID != null) {
                var type = document.getElementById('hdfProductType').value;

                if (type == "WBS" || type == "WB") {
                    cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
                }
                else if (type == "WS") {
                    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
                }
            }

        }
        function CmbBatch_ValueChange() {
            var WarehouseID = cCmbWarehouseID.GetValue();
            var BatchID = cCmbBatch.GetValue();
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
            }
            else if (type == "BS") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID);
            }
        }
        //tab start
        function disp_prompt(name) {

            if (name == "tab0") {
                //gridLookup.Focus();
                //alert(name);
                //document.location.href = "SalesQuotation.aspx?";
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
                    fn_PopOpen();
                }
            }
        }
        //tab end


        <%--kaushik 24-2-2017--%>

        $(document).ready(function () {
            if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
            }
            //$('#ApprovalCross').click(function () {
            //    
            //    window.parent.popup.Hide();
            //    window.parent.cgridPendingApproval.Refresh()();
            //})
        })

        function GetBillingAddressDetailByAddressId(e) {
            var addresskey = billingLookup.GetGridView().GetRowKey(billingLookup.GetGridView().GetFocusedRowIndex());
            if (addresskey != null && addresskey != '') {

                cComponentPanel.PerformCallback('BlookupEdit~' + addresskey);
            }
        }

        function GetShippingAddressDetailByAddressId(e) {

            var saddresskey = shippingLookup.GetGridView().GetRowKey(shippingLookup.GetGridView().GetFocusedRowIndex());
            if (saddresskey != null && saddresskey != '') {

                cComponentPanel.PerformCallback('SlookupEdit~' + saddresskey);
            }
        }

        <%--kaushik 24-2-2017--%>
        function UniqueCodeCheck() {

            var SchemeVal = $('#<%=ddl_numberingScheme.ClientID %> option:selected').val();
            if (SchemeVal == "") {
                alert('Please Select Numbering Scheme');
                ctxt_SlChallanNo.SetValue('');
                ctxt_SlChallanNo.Focus();
            }
            else {
                var OrderNo = ctxt_SlChallanNo.GetText();
                if (OrderNo != '') {

                    var SchemaLength = GetObjectID('hdnSchemaLength').value;
                    var x = parseInt(SchemaLength);
                    var y = parseInt(OrderNo.length);

                    if (y > x) {
                        alert('Sales Order No length cannot be more than ' + x);
                        //jAlert('Please enter unique Sales Order No');
                        ctxt_SlChallanNo.SetValue('');
                        ctxt_SlChallanNo.Focus();

                    }
                    else {
                        var CheckUniqueCode = false;
                        $.ajax({
                            type: "POST",
                            url: "SalesChallanAdd.aspx/CheckUniqueCode",
                            data: JSON.stringify({ OrderNo: OrderNo }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                CheckUniqueCode = msg.d;
                                if (CheckUniqueCode == true) {
                                    alert('Please enter unique Sales Order No');
                                    //jAlert('Please enter unique Sales Order No');
                                    ctxt_SlChallanNo.SetValue('');
                                    ctxt_SlChallanNo.Focus();
                                }
                                else {
                                    $('#MandatorySlOrderNo').attr('style', 'display:none');
                                }
                            }

                        });
                    }
                }
            }
        }



        function DateCheck() {

            var startDate = cPLSalesChallanDate.GetValueString();
            //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + startDate + '~' + '@' + '~' + 'DateCheck');
            ccmbGstCstVat.PerformCallback();
            ccmbGstCstVatcharge.PerformCallback();


            //cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + startDate + '~' + '@');


            grid.PerformCallback('GridBlank');
            cCmbWarehouseID.Focus();
            $("#<%=ddl_transferFrom_Branch.ClientID%>").focus();
            $("#ddl_transferFrom_Branch").focus();
        }

        function DateCheckChanged() {

            var startDate = cPLSalesChallanDate.GetValueString();
            // cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + startDate + '~' + '@' + '~' + '');
        }

        function CloseGridQuotationLookup() {
            //gridSalesOrderLookup.ConfirmCurrentSelection();
            //gridSalesOrderLookup.HideDropDown();
            //gridSalesOrderLookup.Focus();
        }
        var GlobalCurTaxAmt = 0;
        var rowEditCtrl;
        var globalRowIndex;
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }

        function cmbtaxCodeindexChange(s, e) {
            if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {

                var taxValue = s.GetValue();

                if (taxValue == null) {
                    taxValue = 0;
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(0);
                    ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
                }


                var isValid = taxValue.indexOf('~');
                if (isValid != -1) {
                    var rate = parseFloat(taxValue.split('~')[1]);
                    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                    ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
                    GlobalCurTaxAmt = 0;
                }
                else {
                    s.SetText("");
                }

            } else {
                var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                if (s.GetValue() == null) {
                    s.SetValue(0);
                }

                if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                    ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                    GlobalCurTaxAmt = 0;
                } else {
                    s.SetText("");
                }
            }

        }

        function CmbtaxClick(s, e) {
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }


        function txtTax_TextChanged(s, i, e) {


            cgridTax.batchEditApi.StartEdit(i, 2);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);




        }
        //Subhabrata Tax
        function Save_TaxClick() {
            if (gridTax.GetVisibleRowsOnPage() > 0) {
                gridTax.UpdateEdit();
            }
            else {
                gridTax.PerformCallback('SaveGst');
            }
            cPopup_Taxes.Hide();
        }
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

        function recalculateTax() {
            cmbGstCstVatChange(ccmbGstCstVat);
        }

        function recalculateTaxCharge() {
            ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
        }

        function chargeCmbtaxClick(s, e) {
            GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
            ChargegstcstvatGlobalName = s.GetText();
        }
        var GlobalCurChargeTaxAmt;
        var ChargegstcstvatGlobalName;
        function ChargecmbGstCstVatChange(s, e) {

            SetOtherChargeTaxValueOnRespectiveRow(0, 0, ChargegstcstvatGlobalName);
            $('.RecalculateCharge').hide();
            var ProdAmt = parseFloat(ctxtProductAmount.GetValue());

            //Set ProductAmount
            if (s.GetValue().split('~')[2] == 'G') {
                ProdAmt = parseFloat(ctxtProductAmount.GetValue());
            }
            else if (s.GetValue().split('~')[2] == 'N') {
                ProdAmt = parseFloat(clblProdNetAmt.GetValue());
            }
            else if (s.GetValue().split('~')[2] == 'O') {
                //Check for Other Dependecy
                $('.RecalculateCharge').show();
                ProdAmt = 0;
                var taxdependentName = s.GetValue().split('~')[3];
                for (var i = 0; i < taxJson.length; i++) {
                    gridTax.batchEditApi.StartEdit(i, 3);
                    var gridTaxName = gridTax.GetEditor("TaxName").GetText();
                    gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
                    if (gridTaxName == taxdependentName) {
                        ProdAmt = gridTax.GetEditor("Amount").GetValue();
                    }
                }
            }
            else if (s.GetValue().split('~')[2] == 'R') {
                $('.RecalculateCharge').show();
                ProdAmt = GetChargesTotalRunningAmount();
            }


            GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());

            var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVatcharge.GetValue().split('~')[1]) / 100;
            ctxtGstCstVatCharge.SetValue(calculatedValue);

            var totAmt = parseFloat(ctxtQuoteTaxTotalAmt.GetText());
            ctxtQuoteTaxTotalAmt.SetValue(totAmt + calculatedValue - GlobalCurChargeTaxAmt);

            //tax others
            SetOtherChargeTaxValueOnRespectiveRow(0, calculatedValue, ctxtGstCstVatCharge.GetText());
            ChargegstcstvatGlobalName = ctxtGstCstVatCharge.GetText();

            //set Total Amount
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
        }

        var chargejsonTax;
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

            SetChargesRunningTotal();
            ShowTaxPopUp("IN");
        }

        var taxAmountGlobalCharges;
        function QuotationTaxAmountGotFocus(s, e) {
            taxAmountGlobalCharges = parseFloat(s.GetValue());
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
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                GlobalTaxAmt = 0;
                SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                //gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                GlobalTaxAmt = 0;
                SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
            }

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
            SetChargesRunningTotal();
        }


        function Save_TaxesClick() {
            grid.batchEditApi.EndEdit();
            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var i, cnt = 1;
            var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

            cnt = 1;
            for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
                var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
                var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
                var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
                var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

                sumAmount = sumAmount + parseFloat(Amount);
                sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
                sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
                sumNetAmount = sumNetAmount + parseFloat(NetAmount);

                cnt++;
            }

            if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
                cnt = 1;
                for (i = 0 ; cnt <= noofvisiblerows ; i++) {
                    var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
                    var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
                    var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
                    var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
                    var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

                    sumAmount = sumAmount + parseFloat(Amount);
                    sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
                    sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
                    sumNetAmount = sumNetAmount + parseFloat(NetAmount);

                    cnt++;
                }
            }

            //Debjyoti 
            document.getElementById('HdChargeProdAmt').value = sumAmount;
            document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
            //End Here

            ctxtProductAmount.SetValue(Math.round(sumAmount).toFixed(2));
            ctxtProductTaxAmount.SetValue(Math.round(sumTaxAmount).toFixed(2));
            ctxtProductDiscount.SetValue(Math.round(sumDiscount).toFixed(2));
            ctxtProductNetAmount.SetValue(Math.round(sumNetAmount).toFixed(2));
            clblChargesTaxableGross.SetText("");
            clblChargesTaxableNet.SetText("");

            //Checking is gstcstvat will be hidden or not
            if (cddl_AmountAre.GetValue() == "2") {

                $('.lblChargesGSTforGross').show();
                $('.lblChargesGSTforNet').show();

                //Set Gross Amount with GstValue
                //Get The rate of Gst
                var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
                if (gstRate) {
                    if (gstRate != 0) {
                        var gstDis = (gstRate / 100) + 1;
                        if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                            $('.lblChargesGSTforNet').hide();
                            ctxtProductAmount.SetText(Math.round(sumAmount / gstDis).toFixed(2));
                            document.getElementById('HdChargeProdAmt').value = Math.round(sumAmount / gstDis).toFixed(2);
                            clblChargesGSTforGross.SetText(Math.round(sumAmount - parseFloat(document.getElementById('HdChargeProdAmt').value)).toFixed(2));
                            clblChargesTaxableGross.SetText("(Taxable)");

                        }
                        else {
                            $('.lblChargesGSTforGross').hide();
                            ctxtProductNetAmount.SetText(Math.round(sumNetAmount / gstDis).toFixed(2));
                            document.getElementById('HdChargeProdNetAmt').value = Math.round(sumNetAmount / gstDis).toFixed(2);
                            clblChargesGSTforNet.SetText(Math.round(sumNetAmount - parseFloat(document.getElementById('HdChargeProdNetAmt').value)).toFixed(2));
                            clblChargesTaxableNet.SetText("(Taxable)");
                        }
                    }

                } else {
                    $('.lblChargesGSTforGross').hide();
                    $('.lblChargesGSTforNet').hide();
                }
            }
            else if (cddl_AmountAre.GetValue() == "1") {
                $('.lblChargesGSTforGross').hide();
                $('.lblChargesGSTforNet').hide();

                //Debjyoti 09032017
                for (var cmbCount = 1; cmbCount < ccmbGstCstVatcharge.GetItemCount() ; cmbCount++) {
                    if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[5] == '19') {
                        if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'I') {
                            ccmbGstCstVatcharge.RemoveItem(cmbCount);
                            cmbCount--;
                        }
                    } else {
                        if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'C') {
                            ccmbGstCstVatcharge.RemoveItem(cmbCount);
                            cmbCount--;
                        }
                    }
                }






            }
            //End here





            //Set Total amount
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));

            gridTax.PerformCallback('Display');
            //Checking is gstcstvat will be hidden or not
            if (cddl_AmountAre.GetValue() == "2") {
                $('.chargeGstCstvatClass').hide();
            }
            else if (cddl_AmountAre.GetValue() == "1") {
                $('.chargeGstCstvatClass').show();
            }
            //End here
            $('.RecalculateCharge').hide();
            cPopup_Taxes.Show();
            gridTax.StartEditRow(0);
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

        function taxAmtButnClick(s, e) {

            if (e.buttonIndex == 0) {

                if (cddl_AmountAre.GetValue() != null) {
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

                    if (ProductID.trim() != "") {

                        document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();
                        caspxTaxpopUp.Show();
                        //Set Product Gross Amount and Net Amount

                        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                        var SpliteDetails = ProductID.split("||@||");
                        var strMultiplier = SpliteDetails[7];
                        var strFactor = SpliteDetails[8];
                        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                        //var strRate = "1";
                        var strStkUOM = SpliteDetails[4];
                        // var strSalePrice = SpliteDetails[6];
                        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                        if (strRate == 0) {
                            strRate = 1;
                        }

                        var StockQuantity = strMultiplier * QuantityValue;
                        var Amount = Math.round(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                        clblTaxProdGrossAmt.SetText(Amount);
                        clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue()).toFixed(2));
                        document.getElementById('HdProdGrossAmt').value = Amount;
                        document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue()).toFixed(2);

                        //End Here

                        //Set Discount Here
                        if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                            var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
                            clblTaxDiscount.SetText(discount);
                        }
                        else {
                            clblTaxDiscount.SetText('0.00');
                        }
                        //End Here 


                        //Checking is gstcstvat will be hidden or not
                        if (cddl_AmountAre.GetValue() == "2") {
                            $('.GstCstvatClass').hide();
                            $('.gstGrossAmount').show();
                            clblTaxableGross.SetText("(Taxable)");
                            clblTaxableNet.SetText("(Taxable)");
                            $('.gstNetAmount').show();
                            //Set Gross Amount with GstValue
                            //Get The rate of Gst
                            var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
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
                        else if (cddl_AmountAre.GetValue() == "1") {
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
                                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {
                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I') {
                                            ccmbGstCstVat.RemoveItem(cmbCount);
                                            cmbCount--;
                                        }
                                    } else {
                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C') {
                                            ccmbGstCstVat.RemoveItem(cmbCount);
                                            cmbCount--;
                                        }
                                    }
                                }
                            }




                        }
                        //End here

                        if (globalRowIndex > -1) {
                            cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                        } else {

                            cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                            //Set default combo
                            cgridTax.cpComboCode = grid.GetEditor('ProductID').GetValue().split('||@||')[9];
                        }

                        ctxtprodBasicAmt.SetValue(grid.GetEditor('Amount').GetValue());
                    } else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 13);
                    }
                }
            }
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

        function GetTotalRunningAmount() {
            var runningTot = parseFloat(clblProdNetAmt.GetValue());
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.batchEditApi.EndEdit();
            }

            return runningTot;
        }

        var taxAmountGlobal;
        function taxAmountGotFocus(s, e) {
            taxAmountGlobal = parseFloat(s.GetValue());
        }

        var taxJson;
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
                    if (cddl_AmountAre.GetValue() == "1") {
                        var selectedIndex;
                        for (var i = 0; i < ccmbGstCstVat.GetItemCount() ; i++) {
                            if (ccmbGstCstVat.GetItem(i).value.split('~')[0] == cgridTax.cpComboCode) {
                                selectedIndex = i;
                            }
                        }
                        if (selectedIndex) {
                            if (ccmbGstCstVat.GetItem(selectedIndex) != null) {
                                ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
                            }
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
                grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));

            }

            if (cgridTax.GetVisibleRowsOnPage() == 0) {
                $('.cgridTaxClass').hide();
                ccmbGstCstVat.Focus();
            }
            //Debjyoti Check where any Gst Present or not
            // If Not then hide the hole section

            SetRunningTotal();
            ShowTaxPopUp("IY");
        }

        function GetTaxVisibleIndex(s, e) {
            globalTaxRowIndex = e.visibleIndex;
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

        var gstcstvatGlobalName;
        function CmbtaxClick(s, e) {
            GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
            gstcstvatGlobalName = s.GetText();
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

        }

        function taxAmtButnClick1(s, e) {
            console.log(grid.GetFocusedRowIndex());
            rowEditCtrl = s;
        }




        //function cgridTax_EndCallBack(s, e) {
        //    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
        //        ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);

        //        cgridTax.cpUpdated = "";
        //    }

        //    else {
        //        var totAmt = ctxtTaxTotAmt.GetValue();
        //        cgridTax.CancelEdit();
        //        caspxTaxpopUp.Hide();
        //        grid.batchEditApi.StartEdit(globalRowIndex, 13);
        //        grid.GetEditor("TaxAmount").SetValue(totAmt);

        //    }
        //}

        $(document).ready(function () {



            ctxtRate.SetValue("");
            ctxtRate.SetEnabled(false);
            ctxt_SlChallanNo.SetEnabled(false);
            // gridSalesOrderLookup.SetEnabled(false);
            DateCheckChanged();
            PopulateLoadGSTCSTVAT();
        });
    </script>

    <%--Debu Section End--%>



    <%-- ------Subhra Address and Billing Sectin Start-----25-01-2017---------%>
    <script type="text/javascript">


        //Subhra-----23-01-2017-------
        var Billing_state;
        var Billing_city;
        var Billing_pin;
        var billing_area;

        var Shipping_state;
        var Shipping_city;
        var Shipping_pin;
        var Shipping_area;
        //----------------------------------
        function OnChildCall(CmbCity) {

            OnCityChanged(CmbCity.GetValue());
            OnCityChanged(CmbCity1.GetValue());
        }
        function openAreaPage() {
            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = CmbCity.GetValue();
            var cityname = CmbCity.GetText();
            var URL = '../Master/AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            popupan.SetContentUrl(URL);
            popupan.Show();
        }

        function openAreaPageShip() {
            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = CmbCity1.GetValue();
            var cityname = CmbCity1.GetText();
            var URL = '../Master/AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            popupan.SetContentUrl(URL);
            popupan.Show();
        }

        function OnCountryChanged(cmbCountry) {
            CmbState.PerformCallback(cmbCountry.GetValue().toString());
        }
        function OnCountryChanged1(cmbCountry1) {
            CmbState1.PerformCallback(cmbCountry1.GetValue().toString());
        }
        function OnStateChanged(cmbState) {
            CmbCity.PerformCallback(cmbState.GetValue().toString());
        }
        function OnStateChanged1(cmbState1) {
            CmbCity1.PerformCallback(cmbState1.GetValue().toString());
        }

        function OnCityChanged(abc) {
            CmbPin.PerformCallback(abc.GetValue().toString());
            CmbArea.PerformCallback(abc.GetValue().toString());
        }
        function OnCityChanged1(abc) {
            CmbPin1.PerformCallback(abc.GetValue().toString());
            CmbArea1.PerformCallback(abc.GetValue().toString());


        }

        function fn_PopOpen() {
            CmbAddressType.SetSelectedIndex(-1);
            CmbAddressType1.SetSelectedIndex(-1);
            CmbCountry.SetSelectedIndex(-1);
            CmbCountry1.SetSelectedIndex(-1);
            CmbState.SetSelectedIndex(-1);
            CmbState1.SetSelectedIndex(-1);
            CmbCity.SetSelectedIndex(-1);
            CmbCity1.SetSelectedIndex(-1);
            CmbPin.SetSelectedIndex(-1);
            CmbPin1.SetSelectedIndex(-1);
            CmbArea.SetSelectedIndex(-1);
            CmbArea1.SetSelectedIndex(-1);




            Popup_SalesQuote.Show();
            Popup_SalesQuote.PerformCallback('');
        }

        function cmbstate_endcallback(s, e) {
            s.SetValue(Billing_state);
            CmbCity.PerformCallback(CmbState.GetValue());
            Billing_state = 0;
        }
        function cmbshipstate_endcallback(s, e) {
            s.SetValue(Shipping_state);
            CmbCity1.PerformCallback(CmbState1.GetValue());
            Shipping_state = 0;
        }

        function cmbcity_endcallback(s, e) {
            s.SetValue(Billing_city);
            CmbPin.PerformCallback(CmbCity.GetValue());
            CmbArea.PerformCallback(CmbCity.GetValue());
            Billing_city = 0;

        }
        function cmbshipcity_endcallback(s, e) {
            s.SetValue(Shipping_city);
            CmbPin1.PerformCallback(CmbCity1.GetValue());
            CmbArea1.PerformCallback(CmbCity1.GetValue());
            Shipping_city = 0;

        }

        function cmbPin_endcallback(s, e) {
            s.SetValue(Billing_pin);
            Billing_pin = 0;
        }
        function cmbshipPin_endcallback(s, e) {
            s.SetValue(Shipping_pin);
            Shipping_pin = 0;
        }

        function cmbArea_endcallback(s, e) {
            s.SetValue(billing_area);
            billing_area = 0;
        }

        function cmbshipArea_endcallback(s, e) {
            s.SetValue(Shipping_area);
            Shipping_area = 0;
        }

        function Popup_SalesQuote_EndCallBack() {
            if (Popup_SalesQuote.cpshow != null) {


                CmbAddressType.SetText(Popup_SalesQuote.cpshow.split('~')[0]);
                ctxtAddress1.SetText(Popup_SalesQuote.cpshow.split('~')[1]);
                ctxtAddress2.SetText(Popup_SalesQuote.cpshow.split('~')[2]);
                ctxtAddress3.SetText(Popup_SalesQuote.cpshow.split('~')[3]);
                ctxtlandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
                CmbCountry.SetValue(Popup_SalesQuote.cpshow.split('~')[5]);
                Billing_state = Popup_SalesQuote.cpshow.split('~')[6];
                Billing_city = Popup_SalesQuote.cpshow.split('~')[7];
                Billing_pin = Popup_SalesQuote.cpshow.split('~')[8];
                billing_area = Popup_SalesQuote.cpshow.split('~')[9];
                CmbState.PerformCallback(CmbCountry.GetValue());
            }

            if (Popup_SalesQuote.cpshowShip != null) {


                CmbAddressType1.SetText(Popup_SalesQuote.cpshowShip.split('~')[0]);
                ctxtsAddress1.SetText(Popup_SalesQuote.cpshowShip.split('~')[1]);
                ctxtsAddress2.SetText(Popup_SalesQuote.cpshowShip.split('~')[2]);
                ctxtsAddress3.SetText(Popup_SalesQuote.cpshowShip.split('~')[3]);
                ctxtslandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
                CmbCountry1.SetValue(Popup_SalesQuote.cpshowShip.split('~')[5]);
                Shipping_state = Popup_SalesQuote.cpshowShip.split('~')[6];
                Shipping_city = Popup_SalesQuote.cpshowShip.split('~')[7];
                Shipping_pin = Popup_SalesQuote.cpshowShip.split('~')[8];
                Shipping_area = Popup_SalesQuote.cpshowShip.split('~')[9];
                CmbState1.PerformCallback(CmbCountry1.GetValue());
            }

        }

    </script>
    <%-- ------Subhra Address and Billing Section End-----25-01-2017---------%>

    <script type="text/javascript">

        //window.onload = function () {
        //    // grid.AddNewRow();
        //    
        //    OnAddNewClick();
        //};
        function CloseGridLookup() {
            //gridLookup.ConfirmCurrentSelection();
            //gridLookup.HideDropDown();
            //gridLookup.Focus();
            //gridSalesOrderLookup.SetEnabled(true);
        }
        function GetContactPerson(e) {

            //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());


            //if (key != null && key != '') {


            var startDate = new Date();

            startDate = cPLSalesChallanDate.GetValueString();
            cchkBilling.SetChecked(false);
            cchkShipping.SetChecked(false);
            cContactPerson.PerformCallback('BindContactPerson~' + key);


            page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    page.SetActiveTabIndex(1);
                    $('.dxeErrorCellSys').addClass('abc');
                    $('.crossBtn').hide();
                }
            });

            //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + startDate + '~' + '@');
            GetObjectID('hdnCustomerId').value = key;
            GetObjectID('hdnAddressDtl').value = '0';

            //}
        }
        function SetDifference1() {
            var diff = CheckDifferenceOfFromDateWithTodate();
        }
        function CheckDifferenceOfFromDateWithTodate() {
            var startDate = new Date();
            var endDate = new Date();
            var difference = -1;
            startDate = cPLSalesChallanDate.GetDate();
            if (startDate != null) {
                endDate = cExpiryDate.GetDate();
                var startTime = startDate.getTime();
                var endTime = endDate.getTime();
                difference = (startTime - endTime) / 86400000;

            }
            return difference;

        }
        function SetDifference() {
            var diff = CheckDifference();
        }
        function CheckDifference() {
            var startDate = new Date();
            var endDate = new Date();
            var difference = -1;
            startDate = cPLSalesChallanDate.GetDate();
            if (startDate != null) {
                endDate = cExpiryDate.GetDate();
                var startTime = startDate.getTime();
                var endTime = endDate.getTime();
                difference = (endTime - startTime) / 86400000;

            }
            return difference;

        }

        $(document).ready(function () {

            $('#txtDNnumber_I').attr('disabled', 'disabled');
            $('#ddlDNnumbering').attr('disabled', 'disabled');
            $('#ddlDnBranch').attr('disabled', 'disabled');
            //$('#ddl_numberingScheme').focus();
            $('#ddl_numberingScheme').change(function () {

                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];


                if (NoSchemeType == '1') {
                    ctxt_SlChallanNo.SetText('Auto');
                    ctxt_SlChallanNo.SetEnabled(false);
                    cPLSalesChallanDate.Focus();

                }
                else if (NoSchemeType == '0') {
                    ctxt_SlChallanNo.SetText('');
                    ctxt_SlChallanNo.SetEnabled(true);
                    ctxt_SlChallanNo.Focus();

                }
                else {
                    ctxt_SlChallanNo.SetText('');
                    ctxt_SlChallanNo.SetEnabled(false);
                    document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();

                }
            });

           <%-- $("#<%=ddl_SalesAgent.ClientID%>").change(function () {
                
                $("#<%=ddl_transferFrom_Branch.ClientID%>").focus();
            });--%>


            $("#ddl_VehicleNo").change(function () {

                var VehicleNo = $("#ddl_VehicleNo").val();
                $.ajax({
                    type: "POST",
                    url: "BranchTransferIn.aspx/GetDriverNamePhNo",
                    data: JSON.stringify({ cnt_Id: VehicleNo }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {

                        var currentMsg = msg.d;
                        var Name = currentMsg.split(',')[0];
                        var PhoneNo = currentMsg.split(',')[1];
                        $('#txtDriverName').find('input').val(Name);
                        $('#txtPhoneNo').find('input').val(PhoneNo);
                        $('#txt_Refference').focus();
                    }
                });

            });


            $('#ddl_Currency').change(function () {
                var CurrencyId = $(this).val();
                var ActiveCurrency = '<%=Session["ActiveCurrency"]%>'
                var Currency = ActiveCurrency.toString().split('~')[0];
                if (ActiveCurrency != null) {
                    if (CurrencyId != '0') {
                        $.ajax({
                            type: "POST",
                            url: "SalesQuotation.aspx/GetCurrentConvertedRate",
                            data: "{'CurrencyId':'" + CurrencyId + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var currentRate = msg.d;
                                $('#txt_Rate').text(currentRate);
                            }
                        });
                    }
                    else {
                        $('#txt_Rate').text('');
                    }
                }
            });
        });

        function PopulateGSTCSTVAT(e) {
            var key = cddl_AmountAre.GetValue();
            if (key == 1) {
                grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                // cddlVatGstCst.PerformCallback('1');
                cddlVatGstCst.SetSelectedIndex(0);
            }
            else if (key == 2) {
                grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');

            }
            else if (key == 3) {
                grid.GetEditor('TaxAmount').SetEnabled(false);
                cddlVatGstCst.SetEnabled(false);
                // cddlVatGstCst.PerformCallback('3');
                cddlVatGstCst.SetSelectedIndex(0);

            }

        }

        function PopulateLoadGSTCSTVAT() {
            cddlVatGstCst.SetEnabled(false);
        }



        function showQuotationDocument() {
            var URL = "Contact_Document.aspx?requesttype=" + Quotation + "";
            window.location.href = URL;
        }


        // Popup Section

        function ShowCustom() {

            cPopup_wareHouse.Show();


        }

        // Popup Section End

    </script>

    <%--Sudip--%>
    <script>
        var currentEditableVisibleIndex;
        var preventEndEditOnLostFocus = false;
        var lastProductID;
        var setValueFlag;

        //function ProductsCombo_SelectedIndexChanged(s, e) {
        //    
        //    var tbDescription = grid.GetEditor("Description");
        //    var tbUOM = grid.GetEditor("UOM");
        //    var tbStkUOM = grid.GetEditor("StockUOM");
        //    var tbSalePrice = grid.GetEditor("SalePrice");
        //    var tbStockQuantity = grid.GetEditor("StockQuantity");

        //    var ProductID = s.GetValue();
        //    var SpliteDetails = ProductID.split("||@||");
        //    var strProductID = SpliteDetails[0];
        //    var strDescription = SpliteDetails[1];
        //    var strUOM = SpliteDetails[2];
        //    var strStkUOM = SpliteDetails[4];
        //    var strSalePrice = SpliteDetails[6];

        //    tbDescription.SetValue(strDescription);
        //    tbUOM.SetValue(strUOM);
        //    tbStkUOM.SetValue(strStkUOM);
        //    tbSalePrice.SetValue(strSalePrice);
        //    tbStockQuantity.SetValue("0");
        //}

        function ProductsCombo_SelectedIndexChanged(s, e) {
            pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            //var tbStkUOM = grid.GetEditor("StockUOM");
            //var tbStockQuantity = grid.GetEditor("StockQuantity");

            var ProductID = s.GetValue();
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];



            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tbSalePrice.SetValue(strSalePrice);
            divPacking.style.display = "none";
            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            cacpAvailableStock.PerformCallback(strProductID);
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");
        }

        function OnEndCallback(s, e) {

            var InvoiceDetails = getUrlVars();


            if (InvoiceDetails["rcv_no"] != '') {
                document.getElementById('ddl_numbering').style.display = "none";
            }
            else {
                document.getElementById('ddl_numbering').style.display = "block";
            }


            var value = document.getElementById('hdnRefreshType').value;

            if (grid.cpSaveSuccessOrFail == "outrange") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                grid.GetEditor('sProducts_Name').SetEnabled(false);
                grid.GetEditor('sProducts_Description').SetEnabled(false);
                grid.GetEditor('oldUnit_qty').SetEnabled(false);
                grid.GetEditor('oldUnit_value').SetEnabled(false);
                grid.GetEditor('UOM_Name').SetEnabled(false);
                grid.batchEditApi.StartEdit(0, 1);
                var SrlNo = grid.cpProductSrlIDCheck;
                var msg = "Mismatch detected in product and Stock Detail Qty. It must be same to proceed.";
                jAlert(msg);
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Save as Duplicate Quotation Numbe No. Found');
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please try again late.');
            }
            else if (grid.cpSaveSuccessOrFail == "HasBalanceQuantity") {
                grid.batchEditApi.StartEdit(0, 2);
                //jAlert('Please try again late.');


                jConfirm("Old unit Quantity is still pending. Do you wish to keep it pending?\r\nClicking on 'Yes' will keep the Quantity as pending and\r\n'No' will adjust balance quantity and Create Debit Note.", 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        $("#hasBalanceQty").val("No");

                        grid.batchEditApi.EndEdit();
                        grid.UpdateEdit();
                    }
                    else {
                        $("#hasBalanceQty").val("Yes");
                        $('#txtDNnumber_I').attr('disabled', false);
                        $('#ddlDNnumbering').attr('disabled', false);
                        jAlert('Select Numbering scheme for Debit Note');
                        //grid.batchEditApi.EndEdit();
                        //grid.UpdateEdit();
                    }
                });
            }
            else {
                //var Quote_Number = grid.cpQuotationNo;
                //var Quote_Msg = "Sales Quotation No. " + Quote_Number + " generated.";
                $("#hasBalanceQty").val("");
                var SalesOrder_Number = grid.cpSalesOrderNo;
                var Order_Msg = "Old Unit Receive No. " + SalesOrder_Number + " saved.";
                var ReceiveFromService_Number = grid.cpIssueToServiceInEdit;
                var IssueToServiceMsg = "Old Unit Receive No. " + ReceiveFromService_Number + " modified.";
                if (value == "E") {
                    //window.location.assign("OldUnitReceived.aspx");


                    if (SalesOrder_Number != "" && SalesOrder_Number !== undefined) {
                        grid.cpSalesOrderNo = null;
                        //jAlert(Order_Msg);
                        jAlert(Order_Msg, 'Alert Dialog: [Receive From Branch]', function (r) {
                            if (r == true || typeof (r) == 'undefined') {
                                window.location.assign("OldUnitReceived.aspx");
                                grid.cpSalesOrderNo = null;
                            }
                        });

                    }
                    else if (ReceiveFromService_Number != "" && ReceiveFromService_Number !== undefined) {
                        grid.cpIssueToServiceInEdit = null;
                        jAlert(IssueToServiceMsg, 'Alert Dialog: [Receive From Branch]', function (r) {
                            if (r == true) {
                                window.location.assign("OldUnitReceived.aspx");
                                grid.cpIssueToServiceInEdit = null;
                            }
                        });
                    }
                    else {
                        window.location.assign("OldUnitReceived.aspx");
                    }
                }
                else if (value == "N") {
                    // window.location.assign("SalesOrderAdd.aspx?key=ADD");


                    if (SalesOrder_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [Receive From Branch]', function (r) {
                            //jAlert(Order_Msg);
                            grid.cpSalesOrderNo = null;
                            if (r == true) {
                                window.location.assign("OldUnitReceived.aspx");
                            }
                        });
                        //jConfirm(Order_Msg, 'Confirmation Dialog', function (r) {
                        //    if (r == true) {
                        //        window.location.assign("SalesOrderAdd.aspx?key=ADD");
                        //    }
                        //    else
                        //    { window.location.assign("SalesOrderAdd.aspx?key=ADD"); }
                        //});

                    }
                    else {
                        window.location.assign("OldUnitReceived.aspx");
                    }
                }
                else {

                    var pageStatus = document.getElementById('hdnPageStatus').value;
                    if (pageStatus == "first") {

                        if (grid.GetVisibleRowsOnPage() == 0) {
                            OnAddNewClick();
                        }
                        grid.batchEditApi.EndEdit();
                        $('#ddl_numberingScheme').focus();
                        document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "update") {
                        //if (grid.GetVisibleRowsOnPage() == 0) {
                        //    OnAddNewClick();
                        //}
                        grid.GetEditor('sProducts_Name').SetEnabled(false);
                        grid.GetEditor('sProducts_Description').SetEnabled(false);
                        grid.GetEditor('oldUnit_qty').SetEnabled(false);
                        grid.GetEditor('oldUnit_value').SetEnabled(false);
                        grid.GetEditor('UOM_Name').SetEnabled(false);
                        grid.batchEditApi.StartEdit(0, 1);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "Quoteupdate") {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
        }

}

            //if (gridSalesOrderLookup.GetValue() != null) {
            //    grid.GetEditor('ProductName').SetEnabled(false);
            //    grid.GetEditor('Description').SetEnabled(false);
            //    grid.GetEditor('Order_Num').SetEnabled(false);
            //    cPLSalesChallanDate.SetEnabled(false);
            //}
            //else {
            //    if (grid.GetVisibleRowsOnPage() == 0) {
            //        OnAddNewClick();
            //    }
            //}

    cProductsPopup.Hide();
            //for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {
            //  grid.batchEditApi.StartEdit(i);
            //}
}
function Save_ButtonClick() {

    var flag = true;
    var OrderNo = ctxt_SlChallanNo.GetText();
    var slsdate = cPLSalesChallanDate.GetValue();
    var qudate = cPLQADate.GetText();
    //dt_BTOut
    //var customerid = GetObjectID('hdnCustomerId').value;
    var salesorderDate = new Date(slsdate);
    var quotationDate = "";
    if (qudate != null && qudate != '') {
        var qd = qudate.split('-');
        quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

    }




    if (slsdate == null || slsdate == "") {
        flag = false;
        $('#MandatorySlDate').attr('style', 'display:block');
    }
    else {
        $('#MandatorySlDate').attr('style', 'display:none');
        if (qudate != null && qudate != '') {
            var qd = qudate.split('-');
            quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

            if (quotationDate > salesorderDate) {

                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }

    }

    if (OrderNo == "") {
        flag = false;
        $('#MandatorySlOrderNo').attr('style', 'display:block');
    }
    else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }


    if (flag) {
        if (grid.GetVisibleRowsOnPage() > 0) {
            <%--var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);--%>


            $('#<%=hdfIsDelete.ClientID %>').val('I');
            grid.batchEditApi.EndEdit();
            grid.UpdateEdit();
            $('#<%=hdnRefreshType.ClientID %>').val('N');
        }
        else {
            jAlert('You must enter proper details before save');
        }
    }
    // return flag;
}

//function OnGetRowValues(values)
//{
//    if (values > 0) {
//        return false;
//    }
//}

function SaveExit_ButtonClick() {
    var flag = true;
    //grid.StartEditRow(0);
    var dNddl = $('#ddlDNnumbering').val();
    $('#HdnDnNumbering').val(dNddl);
    var OrderNo = ctxt_SlChallanNo.GetText();
    var Adjust = $('#hasBalanceQty').val();
    //var rowcount = grid.GetVisibleRowsOnPage();
    //for (var i = 1 ; i <= rowcount;i++)
    //{
    //    var b = grid.GetRowValues(i, 'Balance_Quantity', OnGetRowValues);
    //}


    if (OrderNo == "") {
        flag = false;
        $('#MandatorySlOrderNo').attr('style', 'display:block');
    }
    else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }


    var DNOrderNo = $('#txtDNnumber_I').val();
    if (Adjust == 'Yes') {
        if (DNOrderNo == "") {
            flag = false;
            $('#spanValidationDN').attr('style', 'display:block');
        }
        else { $('#spanValidationDN').attr('style', 'display:none'); }

    }


    if (flag) {
        if (grid.GetVisibleRowsOnPage() > 0) {

            $('#<%=hdfIsDelete.ClientID %>').val('I');
            grid.batchEditApi.EndEdit();
            grid.UpdateEdit();
            //grid.PerformCallback('OldUnitReceivedsaveChanges');
            $('#<%=hdnRefreshType.ClientID %>').val('E');
        }
        else {
            jAlert('You must enter proper details before save');
        }
    }
}

function QuantityTextChange(s, e) {

    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetText();

    //Subhabrata on 03-03-2017
    //var Id = grid.GetEditor('Quotation_No').GetValue();
    //$.ajax({
    //    type: "POST",
    //    url: "SalesChallanAdd.aspx/CheckBalQuantity",
    //    data: JSON.stringify({ Id: Id, ProductID: ProductID.split('||@||')[0] }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: false,
    //    success: function (msg) {
    //        

    //        var ObjData = msg.d;
    //        if (ObjData.length > 0) {
    //            var balQty = ObjData[0].split('|')[0];
    //            if ((QuantityValue * 1) > (balQty * 1)) {
    //                var OrdeMsg = 'Balance Quantity of selected Product from tagged document is <' + ObjData + '>.Cannot enter quantity more than balance quantity.';
    //                jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {

    //                });
    //                //var tbQuantity = grid.GetEditor("Quantity");
    //                //tbQuantity.SetValue(balQty);
    //                //return false;

    //            }

    //        }
    //    }

    //});

    //End

    var SpliteDetails = ProductID.split("||@||");
    var strMultiplier = SpliteDetails[7];
    var strFactor = SpliteDetails[8];
    //var strRate = (ctxt_Rate.GetValue() != null) ? ctxt_Rate.GetValue() : "1";
    var strRate = "1";
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];

    if (strRate == 0) {
        strRate = 1;
    }

    var StockQuantity = strMultiplier * QuantityValue;
    var Amount = QuantityValue * strFactor * strRate * strSalePrice;

    $('#<%= lblStkQty.ClientID %>').text(StockQuantity);
    $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);

    //var tbStockQuantity = grid.GetEditor("StockQuantity");
    //tbStockQuantity.SetValue(StockQuantity);

    //Subhabrata added on 14-03-2017
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    if (IsPackingActive == "Y") {
        $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
        console.log('jhsdfafa');
        //divPacking.style.display = "block";
        $('#divPacking').css({ 'display': 'block' });
    } else {
        divPacking.style.display = "none";
    }//END

    var tbAmount = grid.GetEditor("Amount");
    tbAmount.SetValue(Amount);

    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(Amount);

    DiscountTextChange(s, e);
}

function DiscountTextChange(s, e) {

    var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

    var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

    var tbAmount = grid.GetEditor("Amount");
    tbAmount.SetValue(amountAfterDiscount);

    var tbTotalAmount = grid.GetEditor("TotalAmount");
    tbTotalAmount.SetValue(amountAfterDiscount);
}
function AddBatchNew(s, e) {

    grid.batchEditApi.EndEdit();
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode === 13) {
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i;
        var cnt = 2;

        grid.AddNewRow();
        if (noofvisiblerows == "0") {
            grid.AddNewRow();
        }
        grid.SetFocusedRowIndex();

        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            cnt++;
        }

        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);
    }
}
function OnAddNewClick() {


    //if (Ctxt_InvoiceNo.GetText() == null && Ctxt_InvoiceNo.GetText() == '' && grid.GetVisibleRowsOnPage() == 0) {
    if (grid.GetVisibleRowsOnPage() == 0) {

        grid.AddNewRow();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i;
        var cnt = 1;
        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(cnt);


            cnt++;
        }
    }
}


var Warehouseindex;
function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();

        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        //if (gridSalesOrderLookup.GetValue() != null) {
        //    //jAlert();
        //    jAlert('Cannot Delete using this button as the Indent is linked with the current document .<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) {

        //    });
        //}

        <%--if (noofvisiblerows != "1" && gridSalesOrderLookup.GetValue() == null) {
            grid.DeleteRow(e.visibleIndex);

            $('#<%=hdfIsDelete.ClientID %>').val('D');
                    grid.UpdateEdit();
                    grid.PerformCallback('Display');
                    grid.batchEditApi.StartEdit(-1, 2);
                    grid.batchEditApi.StartEdit(0, 2);
                }--%>
    }
        //else if (e.buttonID == 'AddNew') {
        //    
        //    if (gridSalesOrderLookup.GetValue() == null) {
        //        var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        //        if (ProductIDValue != "") {

        //            OnAddNewClick();
        //        }
        //        else {
        //            grid.batchEditApi.StartEdit(e.visibleIndex, 2);
        //        }
        //    }
        //    else {
        //        OnAddNewClick();
        //    }
        //}
    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
        var QuantityValue = (grid.GetEditor('oldUnit_qty').GetValue() != null) ? grid.GetEditor('oldUnit_qty').GetValue() : "0";
        var RcvQuantityValue = (grid.GetEditor('Quantity_Received').GetValue() != null) ? grid.GetEditor('Quantity_Received').GetValue() : "0";
        if (QuantityValue == "0.0") {
            jAlert("Quantity should not be zero !.");
        } else {
            if (ProductID != "") {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strMultiplier = SpliteDetails[7];
                var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                var StkQuantityValue = QuantityValue * strMultiplier;
                var stockids = SpliteDetails[10];
                //var Ptype = SpliteDetails[16];
                var Ptype = SpliteDetails[14];
                var StkQuantityValue = QuantityValue * strMultiplier;

                if (stockids == "0") {

                    jAlert("Please Update the Opening Stock!.");
                } else {
                    $('#<%=hdfProductType.ClientID %>').val(Ptype);

                    $('#<%=hdfProductID.ClientID %>').val(strProductID);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(RcvQuantityValue);

                    document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strDescription;
                    document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
                    document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
                    document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
                    document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;
                    cacpAvailableStock.PerformCallback(strProductID);

                    SelectWarehouse = "0";
                    $("#spnCmbWarehouse").hide();
                    $("#spntxtBatch").hide();
                    $("#spntxtQuantity").hide();
                    $("#spntxtserialID").hide();

                    if (Ptype == "W") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Manufacture.style.display = 'none';
                        div_Expiry.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                        cCmbWarehouseID.PerformCallback('BindWarehouse');
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "B") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'block';
                        div_Manufacture.style.display = 'block';
                        div_Expiry.style.display = 'block';
                        div_Quantity.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "S") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'none';
                        div_Manufacture.style.display = 'none';
                        div_Expiry.style.display = 'none';
                        div_Quantity.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WB") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Manufacture.style.display = 'block';
                        div_Expiry.style.display = 'block';
                        div_Quantity.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                        cCmbWarehouseID.PerformCallback('BindWarehouse');
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WS") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Manufacture.style.display = 'none';
                        div_Expiry.style.display = 'none';
                        div_Quantity.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                        cCmbWarehouseID.PerformCallback('BindWarehouse');
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WBS") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Manufacture.style.display = 'block';
                        div_Expiry.style.display = 'block';
                        div_Quantity.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Break.style.display = 'block';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                        cCmbWarehouseID.PerformCallback('BindWarehouse');
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "BS") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'block';
                        div_Manufacture.style.display = 'block';
                        div_Expiry.style.display = 'block';
                        div_Quantity.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                        cPopup_Warehouse.Show();
                    }
                    else {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'none';
                        div_Manufacture.style.display = 'none';
                        div_Expiry.style.display = 'none';
                        div_Quantity.style.display = 'none';
                        div_Serial.style.display = 'none';
                    }
                }
            }
        }
    }

}
function FinalWarehouse() {

    cGrdWarehouse.PerformCallback('WarehouseFinal');
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');
}

function OnWarehouseEndCallback(s, e) {
    var Ptype = document.getElementById('hdfProductType').value;

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 7);
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Sales Quantity must be equal to Warehouse.');
    }
    else {
        if (document.getElementById("myCheck").checked == true) {
            if (Ptype == "W" || Ptype == "WB") {
                cCmbWarehouseID.Focus();
            }
            else if (Ptype == "B") {
                cCmbBatch.Focus();
            }
            else {
                ctxtserial.Focus();
            }
        }
        else {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "WS" || Ptype == "WBS") {
                cCmbWarehouseID.Focus();
            }
            else if (Ptype == "B" || Ptype == "BS") {
                cCmbBatch.Focus();
            }
            else if (Ptype == "S") {
                checkComboBox.Focus();
            }
        }
    }
}




function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}


function SaveWarehouse() {


    //alert(ISupdate);
           <%-- var openqnty = Number($('#hdfopeningstockPC').val());
            var totalqnty = Number($('#hdntotalqntyPC').val());
            if (totalqnty > openqnty) {

                var qnty = Number(ctxtqnty.GetText());
                var againcal = Number(totalqnty - qnty);

                $('#<%=hdntotalqntyPC.ClientID %>').val(againcal);

                var totalqntys = Number($('#hdntotalqntyPC').val());
                ctxtqnty.SetText("0.0");
                ctxtbatchqnty.SetText("0.0");
                //alert(totalqntys);
                jAlert("Please make sure Opening quantity should not be greater and less than total INput quantity.");--%>

    //}
    //else {

    var WarehouseID = cCmbWarehouseID.GetValue();
    var WarehouseName = cCmbWarehouseID.GetText();

    var qnty = ctxtQuantity.GetText();
    var IsSerial = $('#hdnisserial').val();
    //alert(qnty);

    if (qnty == "0.0000") {
        qnty = ctxtQuantity.GetText();
    }

    if (Number(qnty) % 1 != 0 && IsSerial == "true") {
        jAlert("Serial number is activated, Quantity should not contain decimals. ");
        return;
    }

    //alert(qnty);
    var BatchName = ctxtBatchName.GetText();
    var SerialName = ctxtserialID.GetText();
    var Isbatch = $('#hdnisbatch').val();

    var enterdqntity = $('#hdfopeningstockPC').val();

    var hdniswarehouse = $('#hdniswarehouse').val();

    var ISupdate = $('#hdnisoldupdate').val();
    var isnewupdate = $('#hdnisnewupdate').val();
    //alert(Isbatch + "/" + IsSerial);
    //alert(hdniswarehouse+"/"+WarehouseID);
    if (Isbatch == "true" && hdniswarehouse == "false") {
        qnty = ctxtQuantity.GetText();
    }

    if (ISupdate == "true") {

        if (hdniswarehouse == "true" && WarehouseID == null) {

            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
        }
        else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {

            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                //jAlert("Quantity should not be 0.0");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {

            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }

        if (Isbatch == "true" && BatchName == "") {

            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
        } else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {
            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();

        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        var slno = $('#hdncurrentslno').val();



        if (slno != "") {

            cGrdWarehousePC.PerformCallback('Updatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty);

                   <%-- $('#<%=hdnisoldupdate.ClientID %>').val("false");
                    ctxtqnty.SetText("0.0");
                    ctxtbatch.SetText("");
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);--%>
            return false;
        }


    } else if (isnewupdate == "true") {
        if (hdniswarehouse == "true" && WarehouseID == null) {

            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
        }
        else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {

            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                //jAlert("Quantity should not be 0.0");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {

            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }

        if (Isbatch == "true" && BatchName == "") {

            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
        }
        else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {


            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();

        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        var slno = $('#hdncurrentslno').val();

        if (slno != "") {

            cGrdWarehousePC.PerformCallback('NewUpdatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty);

                    <%--$('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                    $('#<%=hdnisnewupdate.ClientID %>').val("false");
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);
                    ctxtqnty.SetText("0.0");
                    ctxtbatch.SetText("");--%>
            return false;
        }

    }
    else {

        var hdnisediteds = $('#hdnisedited').val();

        if (hdniswarehouse == "true" && WarehouseID == null) {

            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");

            return;
        } else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {

            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                //jAlert("Quantity should not be 0.0");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {

            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }
        if (Isbatch == "true" && BatchName == "") {

            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
            return;

        } else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {


            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();
            return;

        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        if (Isbatch == "true" && hdniswarehouse == "false") {

            qnty = ctxtQuantity.GetText();

            if (qnty == "0.0000") {
                //alert("Enter" + ctxtbatchqnty.GetText());

                ctxtQuantity.Focus();
            }
        }

        if (qnty == "0.0") {

            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                //jAlert("Quantity should not be 0.0");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        }
        else if (((hdniswarehouse == "true" && WarehouseID != null) || hdniswarehouse == "false") && ((Isbatch == "true" && BatchName != "") || Isbatch == "false") && ((IsSerial == "true" && SerialName != "") || IsSerial == "false") && qnty != "0.0") {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
            $("#RequiredFieldValidatortxtserial").css("display", "none");

            $("#RequiredFieldValidatortxtwareqntity").removeAttr("style");
            $("#RequiredFieldValidatortxtbatchqntity").removeAttr("style");
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");


            if (Isbatch == "true" && hdniswarehouse == "false") {

                qnty = ctxtQuantity.GetText();

                if (qnty = "0.0000") {
                    ctxtQuantity.Focus();
                }
            }


            var oldenterqntity = $('#hdnenterdopenqnty').val();
            var enterdqntityss = $('#hdnnewenterqntity').val();
            var deletedquantity = $('#hdndeleteqnity').val();
            //alert(deletedquantity);
            // alert(enterdqntityss + "|" + oldenterqntity);
            //if (Number(enterdqntityss) < Number(oldenterqntity)) {
            //    qnty = "0.00";
            //    jConfirm('You have entered Quantity less than Opening Quantity. Do you want to clear all existing entries.?', 'Confirmation Dialog', function (r) {
            //        if (r == true) {

            //            cGrdWarehousePC.PerformCallback('Delete~' + WarehouseID);
            //            var strProductID = $('#hdfProductIDPC').val();
            //            var stockids = $('#hdfstockidPC').val();
            //            var branchid = $('#hdbranchIDPC').val();
            //            var strProductName = $('#lblProductName').text();
            //            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
            //        }
            //    });


            //}




            if (Number(qnty) > (Number(enterdqntity) + Number(deletedquantity)) && hdnisediteds == "false") {
                qnty = "0.00";
                jAlert("You have entered Quantity greater than Quantity. Cannot Proceed.");


            }
            else {

                //alert();
                cGrdWarehousePC.PerformCallback('Display~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + qnty);

                //ctxtserial.SetValue("");

                //ASPx.CalClearClick('txtmkgdate_DDD_C');
                //ASPx.CalClearClick('txtexpirdate_DDD_C');

                cCmbWarehouseID.Focus();
            }
        }
        //}

        //ctxtexpirdate.SetText("");
        //ctxtmkgdate.SetText("");
        return false;
    }
}



function txtserialTextChanged() {
    var SerialNo = (ctxtserial.GetValue != null) ? (ctxtserial.GetValue()) : "0";
    ctxtserial.SetValue("");
    var texts = [SerialNo];
    var values = GetValuesByTexts(texts);
    checkListBox.SelectValues(values);
    UpdateSelectAllItemState();
    UpdateText(); // for remove non-existing texts
    SaveWarehouse();
}

function AutoCalculateMandateOnChange(element) {
    $("#spnCmbWarehouse").hide();
    $("#spnCmbBatch").hide();
    $("#spncheckComboBox").hide();
    $("#spntxtQuantity").hide();

    if (document.getElementById("myCheck").checked == true) {
        divSingleCombo.style.display = "block";
        divMultipleCombo.style.display = "none";

        checkComboBox.Focus();
    }
    else {
        divSingleCombo.style.display = "none";
        divMultipleCombo.style.display = "block";

        ctxtserial.Focus();
    }
}


function fn_Deletecity(keyValue) {
    var WarehouseID = (cCmbWarehouseID.GetValue() != null) ? cCmbWarehouseID.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
}

  <%-- kaushik 20-2-2017 --%>

        $(document).ready(function () {
            // $('#CNNumberingScheme').hide();
            $('#txtDNnumber_I').attr('disabled', true);
            $('#ddlDNnumbering').attr('disabled', true);
            $('#ddl_VatGstCst_I').blur(function () {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            })
            $('#ddl_AmountAre_I').blur(function () {


                var key = cddl_AmountAre.GetValue();

                if (key == 1 || key == 3) {
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 2);
                    }

                }
            })

        });
        <%-- kaushik 20-2-2017 --%>
    </script>
    <script type="text/javascript">
        // <![CDATA[
        var textSeparator = ";";
        var selectedChkValue = "";

        function OnListBoxSelectionChanged(listBox, args) {

            if (args.index == 0)
                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
            UpdateSelectAllItemState();
            UpdateText();

            var selectedItems = checkListBox.GetSelectedItems();
            var val = GetSelectedItemsText(selectedItems);
            var strWarehouse = cCmbWarehouseID.GetValue();
            var strBatchID = cCmbBatch.GetValue();
            var ProducttId = $("#hdfProductID").val();
            $.ajax({
                type: "POST",
                url: "BranchTransferOut.aspx/GetSerialId",
                data: JSON.stringify({
                    "id": val,
                    "wareHouseStr": strWarehouse,
                    "BatchID": strBatchID,
                    "ProducttId": ProducttId
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {

                    var type = msg.d;
                    if (type == "1") {

                        return true;
                    }
                    else if (type == "0") {
                        alert("Serial No can be Stock out based on FIFO process.Select the Serial No. shown from Oldest to Newest sequence to proceed");
                        //listBox.UnselectAll();

                        var indices = [];
                        //Added By:Subhabrata
                        if ((selectedItems.length * 1) == 1) {
                            indices.push(listBox.GetItem(args.index));
                            listBox.UnselectIndices(indices[0].text);
                            UpdateSelectAllItemState();
                            UpdateText();
                        }
                        if (((args.index) * 1) <= (selectedItems.length * 1)) {
                            for (var i = ((args.index) * 1) ; i <= ((selectedItems.length * 1) + 1) ; i++) {
                                indices.push(listBox.GetItem(i));

                            }
                        }
                        else {
                            indices.push(listBox.GetItem(args.index));
                            listBox.UnselectIndices(indices[0].text);
                            UpdateSelectAllItemState();
                            UpdateText();
                        }

                        for (var j = 0; j < indices.length   ; j++) {
                            listBox.UnselectIndices(indices[j].text);
                            UpdateSelectAllItemState();
                            UpdateText();
                        }



                    }
                }
            });

        }
        function UpdateSelectAllItemState() {
            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
        }
        function IsAllSelected() {
            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
        }
        function UpdateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            selectedChkValue = GetSelectedItemsText(selectedItems);
            //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
            checkComboBox.SetText(selectedItems.length + " Items");

            var val = GetSelectedItemsText(selectedItems);
            $("#abpl").attr('data-content', val);
        }
        function SynchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            //var texts = dropDown.GetText().split(textSeparator);
            var texts = selectedChkValue.split(textSeparator);
            var values = GetValuesByTexts(texts);
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
        }
        function GetSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                if (items[i].index != 0)
                    texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function GetValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
        $(function () {
            $('[data-toggle="popover"]').popover();
        })
        // ]]>
    </script>



    <%-- ------Subhra Address and Billing Sectin Start-----25-01-2017---------%>
    <script type="text/javascript">


        //Subhra-----23-01-2017-------
        var Billing_state;
        var Billing_city;
        var Billing_pin;
        var billing_area;

        var Shipping_state;
        var Shipping_city;
        var Shipping_pin;
        var Shipping_area;
        //----------------------------------
        function OnChildCall(CmbCity) {

            OnCityChanged(CmbCity.GetValue());
            OnCityChanged(CmbCity1.GetValue());
        }
        function openAreaPage() {
            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = CmbCity.GetValue();
            var cityname = CmbCity.GetText();
            var URL = '../Master/AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            popupan.SetContentUrl(URL);
            popupan.Show();
        }

        function openAreaPageShip() {
            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = CmbCity1.GetValue();
            var cityname = CmbCity1.GetText();
            var URL = '../Master/AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            popupan.SetContentUrl(URL);
            popupan.Show();
        }

        //kaushik-----24-02-2017-------
        function SettingTabStatus() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            }
        }

        function CopyBillingAddresstoShipping(obj) {
            var chkbill = obj.GetChecked();
            if (chkbill == true) {
                $('#DivShipping').hide();
            }
            else {
                $('#DivShipping').show();
            }

            //cComponentPanel.PerformCallback('Edit~BillingAddresstoShipping');
        }
        function CopyShippingAddresstoBilling(obj) {
            var chkship = obj.GetChecked();
            if (chkship == true) {
                $('#DivBilling').hide();
            }
            else {
                $('#DivBilling').show();
            }
            //cComponentPanel.PerformCallback('Edit~ShippingAddresstoBilling');
        }
        function btnSave_QuoteAddress() {
            checking = true;
            var chkbilling = cchkBilling.GetChecked();
            var chkShipping = cchkShipping.GetChecked();

            if (chkbilling == false && chkShipping == false) {
                // Billing Start
                if (ctxtAddress1.GetText() == '' || ctxtAddress1.GetText() == null) {
                    $('#badd1').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#badd1').attr('style', 'display:none'); }

                if (CmbCountry.GetValue() == '' || CmbCountry.GetValue() == null || CmbCountry.GetValue() == 'select') {
                    $('#bcountry').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bcountry').attr('style', 'display:none'); }


                // State

                if (CmbState.GetValue() == '' || CmbState.GetValue() == null || CmbState.GetValue() == 'select') {
                    $('#bstate').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bstate').attr('style', 'display:none'); }

                // city
                if (CmbCity.GetValue() == '' || CmbCity.GetValue() == null || CmbCity.GetValue() == 'select') {
                    $('#bcity').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bcity').attr('style', 'display:none'); }

                // pin

                if (CmbPin.GetValue() == '' || CmbPin.GetValue() == null || CmbPin.GetValue() == 'select') {
                    $('#bpin').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bpin').attr('style', 'display:none'); }
                // Billing End

                // Shipping Start

                if (ctxtsAddress1.GetText() == '' || ctxtsAddress1.GetText() == null) {
                    $('#sadd1').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#sadd1').attr('style', 'display:none'); }

                if (CmbCountry1.GetValue() == '' || CmbCountry1.GetValue() == null || CmbCountry1.GetValue() == 'select') {
                    $('#scountry').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#scountry').attr('style', 'display:none'); }


                // State

                if (CmbState1.GetValue() == '' || CmbState1.GetValue() == null || CmbState1.GetValue() == 'select') {
                    $('#sstate').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#sstate').attr('style', 'display:none'); }

                // city
                if (CmbCity1.GetValue() == '' || CmbCity1.GetValue() == null || CmbCity1.GetValue() == 'select') {
                    $('#scity').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#scity').attr('style', 'display:none'); }

                // pin

                if (CmbPin1.GetValue() == '' || CmbPin1.GetValue() == null || CmbPin1.GetValue() == 'select') {
                    $('#spin').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#spin').attr('style', 'display:none'); }

                // Shipping End

            }


            else if (chkbilling == true && chkShipping == false) {
                // Billing Start
                if (ctxtAddress1.GetText() == '' || ctxtAddress1.GetText() == null) {
                    $('#badd1').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#badd1').attr('style', 'display:none'); }

                if (CmbCountry.GetValue() == '' || CmbCountry.GetValue() == null || CmbCountry.GetValue() == 'select') {
                    $('#bcountry').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bcountry').attr('style', 'display:none'); }


                // State

                if (CmbState.GetValue() == '' || CmbState.GetValue() == null || CmbState.GetValue() == 'select') {
                    $('#bstate').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bstate').attr('style', 'display:none'); }

                // city
                if (CmbCity.GetValue() == '' || CmbCity.GetValue() == null || CmbCity.GetValue() == 'select') {
                    $('#bcity').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bcity').attr('style', 'display:none'); }

                // pin

                if (CmbPin.GetValue() == '' || CmbPin.GetValue() == null || CmbPin.GetValue() == 'select') {
                    $('#bpin').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bpin').attr('style', 'display:none'); }
                // Billing End
            }

            else if (chkbilling == false && chkShipping == true) {
                // Shipping Start

                if (ctxtsAddress1.GetText() == '' || ctxtsAddress1.GetText() == null) {
                    $('#sadd1').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#sadd1').attr('style', 'display:none'); }

                if (CmbCountry1.GetValue() == '' || CmbCountry1.GetValue() == null || CmbCountry1.GetValue() == 'select') {
                    $('#scountry').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#scountry').attr('style', 'display:none'); }


                // State

                if (CmbState1.GetValue() == '' || CmbState1.GetValue() == null || CmbState1.GetValue() == 'select') {
                    $('#sstate').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#sstate').attr('style', 'display:none'); }

                // city
                if (CmbCity1.GetValue() == '' || CmbCity1.GetValue() == null || CmbCity1.GetValue() == 'select') {
                    $('#scity').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#scity').attr('style', 'display:none'); }

                // pin

                if (CmbPin1.GetValue() == '' || CmbPin1.GetValue() == null || CmbPin1.GetValue() == 'select') {
                    $('#spin').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#spin').attr('style', 'display:none'); }

                // Shipping End

            }
            if (checking == true) {

                var custID = GetObjectID('hdnCustomerId').value;
                var chkbilling = cchkBilling.GetChecked();
                //var chkbilling = cchkBilling.GetChecked;
                var chkShipping = cchkShipping.GetChecked();
                //var chkShipping = cchkShipping.GetChecked;
                if ((chkbilling == false) && (chkShipping == false)) {
                    cComponentPanel.PerformCallback('save~1');
                }
                else if ((chkbilling == true) && (chkShipping == false)) {
                    cComponentPanel.PerformCallback('save~2');
                }
                else if ((chkbilling == false) && (chkShipping == true)) {
                    cComponentPanel.PerformCallback('save~3');
                }
                GetObjectID('hdnAddressDtl').value = '1';
                page.SetActiveTabIndex(0);
                //gridLookup.Focus();
            }
            else {
                page.SetActiveTabIndex(1);
            }
        }
        function ClosebillingLookup() {
            billingLookup.ConfirmCurrentSelection();
            billingLookup.HideDropDown();
            billingLookup.Focus();
        }
        function CloseshippingLookup() {
            shippingLookup.ConfirmCurrentSelection();
            shippingLookup.HideDropDown();
            shippingLookup.Focus();
        }


        var Billing_state;
        var Billing_city;
        var Billing_pin;
        var billing_area;

        var Shipping_state;
        var Shipping_city;
        var Shipping_pin;
        var Shipping_area;


        function Panel_endcallback() {

            var billingStatus = null;
            var shippingStatus = null;
            if (cComponentPanel.cpshow != null) {


                //CmbAddressType.SetText(cComponentPanel.cpshow.split('~')[0]);
                ctxtAddress1.SetText(cComponentPanel.cpshow.split('~')[1]);
                ctxtAddress2.SetText(cComponentPanel.cpshow.split('~')[2]);
                ctxtAddress3.SetText(cComponentPanel.cpshow.split('~')[3]);
                ctxtlandmark.SetText(cComponentPanel.cpshow.split('~')[4]);
                var bcon = cComponentPanel.cpshow.split('~')[5];
                if (bcon == '') {
                    CmbCountry.SetSelectedIndex(-1);
                }
                else {
                    CmbCountry.SetValue(cComponentPanel.cpshow.split('~')[5]);
                }

                var bsta = cComponentPanel.cpshow.split('~')[6];
                if (bsta == '') {
                    CmbState.SetSelectedIndex(-1);
                }
                else {
                    Billing_state = cComponentPanel.cpshow.split('~')[6];
                }
                var bcity = cComponentPanel.cpshow.split('~')[7];
                if (bcity == '') {
                    CmbCity.SetSelectedIndex(-1);
                }
                else {
                    Billing_city = cComponentPanel.cpshow.split('~')[7];
                }

                var bpin = cComponentPanel.cpshow.split('~')[8];
                if (bpin == '') {
                    CmbPin.SetSelectedIndex(-1);
                }
                else {
                    Billing_pin = cComponentPanel.cpshow.split('~')[8];
                }

                var barea = cComponentPanel.cpshow.split('~')[9];
                if (barea == '') {
                    CmbArea.SetSelectedIndex(-1);
                }
                else {
                    billing_area = cComponentPanel.cpshow.split('~')[9];
                }
                //CmbCountry.SetValue(cComponentPanel.cpshow.split('~')[5]);
                //Billing_state = cComponentPanel.cpshow.split('~')[6];
                //Billing_city = cComponentPanel.cpshow.split('~')[7];
                //Billing_pin = cComponentPanel.cpshow.split('~')[8];
                //billing_area = cComponentPanel.cpshow.split('~')[9];
                billingStatus = cComponentPanel.cpshow.split('~')[10];
                var countryid = CmbCountry.GetValue()
                if (countryid != null && countryid != '' && countryid != '0') {
                    CmbState.PerformCallback(countryid);
                }
            }

            if (cComponentPanel.cpshowShip != null) {

                //CmbAddressType1.SetText(cComponentPanel.cpshowShip.split('~')[0]);
                ctxtsAddress1.SetText(cComponentPanel.cpshowShip.split('~')[1]);

                ctxtsAddress2.SetText(cComponentPanel.cpshowShip.split('~')[2]);
                ctxtsAddress3.SetText(cComponentPanel.cpshowShip.split('~')[3]);
                ctxtslandmark.SetText(cComponentPanel.cpshowShip.split('~')[4]);
                var scon = cComponentPanel.cpshowShip.split('~')[5];
                if (scon == '') {
                    CmbCountry1.SetSelectedIndex(-1);
                }
                else {
                    CmbCountry1.SetValue(cComponentPanel.cpshowShip.split('~')[5]);
                }
                var ssta = cComponentPanel.cpshowShip.split('~')[6];
                if (ssta == '') {
                    CmbState1.SetSelectedIndex(-1);
                }
                else {
                    Shipping_state = cComponentPanel.cpshowShip.split('~')[6];
                }
                var scity = cComponentPanel.cpshowShip.split('~')[7];
                if (scity == '') {
                    CmbCity1.SetSelectedIndex(-1);
                }
                else {
                    Shipping_city = cComponentPanel.cpshowShip.split('~')[7];
                }

                var spin = cComponentPanel.cpshowShip.split('~')[8];
                if (spin == '') {
                    CmbPin1.SetSelectedIndex(-1);
                }
                else {
                    Shipping_pin = cComponentPanel.cpshowShip.split('~')[8];
                }

                var sarea = cComponentPanel.cpshowShip.split('~')[9];
                if (sarea == '') {
                    CmbArea1.SetSelectedIndex(-1);
                }
                else {
                    Shipping_area = cComponentPanel.cpshowShip.split('~')[9];
                }
                //CmbCountry1.SetValue(cComponentPanel.cpshowShip.split('~')[5]);
                //Shipping_state = 
                //Shipping_city = cComponentPanel.cpshowShip.split('~')[7];
                //Shipping_pin = cComponentPanel.cpshowShip.split('~')[8];
                //Shipping_area = cComponentPanel.cpshowShip.split('~')[9];
                shippingStatus = cComponentPanel.cpshowShip.split('~')[10];
                var countryid1 = CmbCountry1.GetValue()
                if (countryid1 != null && countryid1 != '' && countryid1 != '0') {
                    CmbState1.PerformCallback(countryid1);
                }
                //CmbState1.PerformCallback(CmbCountry1.GetValue());
            }
            if (billingStatus == 'Y' && shippingStatus == 'N') {
                cchkBilling.SetEnabled(true);
                cchkShipping.SetEnabled(false);
            }
            else if (billingStatus == 'N' && shippingStatus == 'Y') {
                cchkBilling.SetEnabled(false);
                cchkShipping.SetEnabled(true);

            }
            else if (billingStatus == 'Y' && shippingStatus == 'Y') {
                cchkBilling.SetEnabled(false);
                cchkShipping.SetEnabled(false);

            }
            else if (billingStatus == 'N' && shippingStatus == 'N') {
                cchkBilling.SetEnabled(false);
                cchkShipping.SetEnabled(false);
            }
        }
        //kaushik-----24-02-2017-------
        function OnCountryChanged(cmbCountry) {
            CmbState.PerformCallback(cmbCountry.GetValue().toString());
        }
        function OnCountryChanged1(cmbCountry1) {
            CmbState1.PerformCallback(cmbCountry1.GetValue().toString());
        }
        function OnStateChanged(cmbState) {
            CmbCity.PerformCallback(cmbState.GetValue().toString());
        }
        function OnStateChanged1(cmbState1) {
            CmbCity1.PerformCallback(cmbState1.GetValue().toString());
        }

        function OnCityChanged(abc) {
            CmbPin.PerformCallback(abc.GetValue().toString());
            CmbArea.PerformCallback(abc.GetValue().toString());
        }
        function OnCityChanged1(abc) {
            CmbPin1.PerformCallback(abc.GetValue().toString());
            CmbArea1.PerformCallback(abc.GetValue().toString());


        }

        function fn_PopOpen() {
            CmbCountry.SetSelectedIndex(-1);
            CmbCountry1.SetSelectedIndex(-1);
            CmbState.SetSelectedIndex(-1);
            CmbState1.SetSelectedIndex(-1);
            CmbCity.SetSelectedIndex(-1);
            CmbCity1.SetSelectedIndex(-1);
            CmbPin.SetSelectedIndex(-1);
            CmbPin1.SetSelectedIndex(-1);
            CmbArea.SetSelectedIndex(-1);
            CmbArea1.SetSelectedIndex(-1);
            cComponentPanel.PerformCallback('Edit~1');
            //cComponentPanel.PerformCallback('Edit~1' + GetObjectID('hdnAddressDtl').value); 
        }
        function cmbstate_endcallback(s, e) {
            s.SetValue(Billing_state);
            CmbCity.PerformCallback(CmbState.GetValue());
            Billing_state = 0;
        }
        function cmbshipstate_endcallback(s, e) {
            s.SetValue(Shipping_state);
            CmbCity1.PerformCallback(CmbState1.GetValue());
            Shipping_state = 0;
        }

        function cmbcity_endcallback(s, e) {
            s.SetValue(Billing_city);
            CmbPin.PerformCallback(CmbCity.GetValue());
            CmbArea.PerformCallback(CmbCity.GetValue());
            Billing_city = 0;

        }
        function cmbshipcity_endcallback(s, e) {
            s.SetValue(Shipping_city);
            CmbPin1.PerformCallback(CmbCity1.GetValue());
            CmbArea1.PerformCallback(CmbCity1.GetValue());
            Shipping_city = 0;

        }

        function cmbPin_endcallback(s, e) {
            s.SetValue(Billing_pin);
            Billing_pin = 0;
        }
        function cmbshipPin_endcallback(s, e) {
            s.SetValue(Shipping_pin);
            Shipping_pin = 0;
        }

        function cmbArea_endcallback(s, e) {
            s.SetValue(billing_area);
            billing_area = 0;
        }

        function cmbshipArea_endcallback(s, e) {
            s.SetValue(Shipping_area);
            Shipping_area = 0;
        }

        function Popup_SalesQuote_EndCallBack() {
            if (Popup_SalesQuote.cpshow != null) {


                CmbAddressType.SetText(Popup_SalesQuote.cpshow.split('~')[0]);
                ctxtAddress1.SetText(Popup_SalesQuote.cpshow.split('~')[1]);
                ctxtAddress2.SetText(Popup_SalesQuote.cpshow.split('~')[2]);
                ctxtAddress3.SetText(Popup_SalesQuote.cpshow.split('~')[3]);
                ctxtlandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
                CmbCountry.SetValue(Popup_SalesQuote.cpshow.split('~')[5]);
                Billing_state = Popup_SalesQuote.cpshow.split('~')[6];
                Billing_city = Popup_SalesQuote.cpshow.split('~')[7];
                Billing_pin = Popup_SalesQuote.cpshow.split('~')[8];
                billing_area = Popup_SalesQuote.cpshow.split('~')[9];
                CmbState.PerformCallback(CmbCountry.GetValue());
            }

            if (Popup_SalesQuote.cpshowShip != null) {


                CmbAddressType1.SetText(Popup_SalesQuote.cpshowShip.split('~')[0]);
                ctxtsAddress1.SetText(Popup_SalesQuote.cpshowShip.split('~')[1]);
                ctxtsAddress2.SetText(Popup_SalesQuote.cpshowShip.split('~')[2]);
                ctxtsAddress3.SetText(Popup_SalesQuote.cpshowShip.split('~')[3]);
                ctxtslandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
                CmbCountry1.SetValue(Popup_SalesQuote.cpshowShip.split('~')[5]);
                Shipping_state = Popup_SalesQuote.cpshowShip.split('~')[6];
                Shipping_city = Popup_SalesQuote.cpshowShip.split('~')[7];
                Shipping_pin = Popup_SalesQuote.cpshowShip.split('~')[8];
                Shipping_area = Popup_SalesQuote.cpshowShip.split('~')[9];
                CmbState1.PerformCallback(CmbCountry1.GetValue());
            }

        }

    </script>
    <%-- ------Subhra Address and Billing Section End-----25-01-2017---------%>
    <style>
        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        /*#grid_DXEditingErrorRow-1 {
            display: none;
        }*/

        /*#grid_DXStatus span > a {
            display: none;
        }

        #gridTax_DXStatus span > a {
            display: none;
        }*/

        #grid_DXStatus {
            display: none;
        }

        #aspxGridTax_DXStatus {
            display: none;
        }

        #gridTax_DXStatus {
            display: none;
        }

        .hideCell {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#btnAdd").bind("click", function () {
                $("#SerialContainer").empty();
                $("#BatchContainer").empty();
                $("#SerialContainer").append("<div><span>Serial Number</span><div />");
                $("#BatchContainer").append("<div><span>Batch Number</span><div />");

                var count = ctxtQuantity_Warehouse.GetValue();


                for (var i = 1; i <= count; i++) {
                    var div = $("<div />");
                    div.html(GetDynamicSerial(""));
                    $("#SerialContainer").append(div);

                    var div1 = $("<div />");
                    div1.html(GetDynamicBatch(""));
                    $("#BatchContainer").append(div1);
                }
            });
        });

        function GetDynamicSerial(value) {
            return '<input name = "SerialContainer" type="text" value = "' + value + '" />'
        }

        function GetDynamicBatch(value) {
            return '<input name = "BatchContainer" type="text" value = "' + value + '" />'
        }


    <%--    function Currency_Rate() {

            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var Currency_ID = cCmbCurrency.GetValue();
            $('#<%=hdnCurrenctId.ClientID %>').val(Currency_ID);


            if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
                ctxtRate.SetValue("");
                ctxtRate.SetEnabled(false);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "ContraVoucher.aspx/GetRate",
                    data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;
                        ctxtRate.SetValue(data);


                    }
                });

                ctxtRate.SetEnabled(true);
            }
        }--%>

        function OrderNumberChanged() {

            var quote_Id = '';  //gridSalesOrderLookup.GetValue();
            if (quote_Id != null) {
                //var keyVal = gridSalesOrderLookup.gridView.GetSelectedKeysOnPage();
                var KeyVal = '';  //gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());

                cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + KeyVal);

                $.ajax({
                    type: "POST",
                    url: "ReceiveFromServiceCenter.aspx/GetIssueToServiceTaggedDetails",
                    data: JSON.stringify({ KeyVal: KeyVal }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {

                        var currentMsg = msg.d;

                        var Branch = currentMsg.split(',')[0];
                        var ServiceCentre = currentMsg.split(',')[1];
                        var CallNo = currentMsg.split(',')[2];
                        var Narration = currentMsg.split(',')[3];

                        $("#<%=ddl_transferFrom_Branch.ClientID%>").val(Branch);
                        $("#<%=ddl_ServiceCenter.ClientID%>").val(ServiceCentre);
                        txtCallNo.SetText(CallNo);
                        txt_Refference.SetText(Narration);




                    }
                });


                //cComponentTransferFrom.PerformCallback('TransferBranchFrom' + '~' + KeyVal);
                cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
                //cComponentServiceCentre.PerformCallback('ServiceCentre' + '~' + KeyVal);
                //cComponentCallNo.PerformCallback('CallNo' + '~' + KeyVal);
                //cComponentNarration.PerformCallback('Narration' + '~' + KeyVal);
                cProductsPopup.Show();
            }
            else {
                cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
                cProductsPopup.Show();
            }

        }

        <%--kaushik--%>
        function GridCallBack() {
            grid.PerformCallback('Display');
        }
        <%--kaushik--%>

        function ChangeState(value) {

            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }
        function PerformCallToGridBind() {

            grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
            //cSalesOrderComponentPanel.PerformCallback('BindOrderLookupOnSelection');
            cProductsPopup.Hide();
            $('#<%=hdnPageStatus.ClientID %>').val('Quoteupdate');

            return false;
        }


        function componentEndCallBack(s, e) {
            //gridSalesOrderLookup.gridView.Refresh();
            //if (grid.GetVisibleRowsOnPage() == 0) {
            //    OnAddNewClick();
            //}
        }
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                // var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '';
                var keyval = $('#<%=hdnmodeId.ClientID %>').val();
                //  alert(keyval);
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=BSO&&KeyVal_InternalID=' + keyval;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code
    </script>
    <%--End Sudip--%>




    <%--Debu Section--%>
    <script type="text/javascript">

        var taxAmountGlobal;
        function taxAmountGotFocus(s, e) {
            taxAmountGlobal = parseFloat(s.GetValue());
        }
        //function taxAmountLostFocus(s, e) {
        //    var finalTaxAmt = parseFloat(s.GetValue());
        //    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
        //    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        //    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        //    if (sign == '(+)') {
        //        ctxtTaxTotAmt.SetValue(totAmt + finalTaxAmt - taxAmountGlobal);
        //    } else {
        //        ctxtTaxTotAmt.SetValue(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1));
        //    }


        //    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

        //}

        function cmbGstCstVatChange(s, e) {

            SetOtherTaxValueOnRespectiveRow(0, 0, gstcstvatGlobalName);

            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());

            var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVat.GetValue().split('~')[1]) / 100;
            ctxtGstCstVat.SetValue(calculatedValue);

            var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
            ctxtTaxTotAmt.SetValue(totAmt + calculatedValue - GlobalCurTaxAmt);

            //tax others
            SetOtherTaxValueOnRespectiveRow(0, calculatedValue, ccmbGstCstVat.GetText());
            gstcstvatGlobalName = ccmbGstCstVat.GetText();
        }


        var GlobalCurTaxAmt = 0;
        var rowEditCtrl;
        var globalRowIndex;
        var globalTaxRowIndex;
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }

        function cmbtaxCodeindexChange(s, e) {
            if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {

                var taxValue = s.GetValue();

                if (taxValue == null) {
                    taxValue = 0;
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(0);
                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
                }


                var isValid = taxValue.indexOf('~');
                if (isValid != -1) {
                    var rate = parseFloat(taxValue.split('~')[1]);
                    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
                    GlobalCurTaxAmt = 0;
                }
                else {
                    s.SetText("");
                }

            } else {
                var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                if (s.GetValue() == null) {
                    s.SetValue(0);
                }

                if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                    GlobalCurTaxAmt = 0;
                } else {
                    s.SetText("");
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

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        GlobalCurTaxAmt = 0;
                    }




                }
            }
            //return;
            cgridTax.batchEditApi.EndEdit();
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

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                        gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        GlobalCurTaxAmt = 0;
                    }




                }
            }
            //return;
            gridTax.batchEditApi.EndEdit();
        }



        function txtPercentageLostFocus(s, e) {
            console.log(s);
            console.log(e);
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

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        GlobalCurTaxAmt = 0;
                    }
                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
                } else {
                    s.SetText("");
                }
            }

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







    </script>
    <%--Debu Section End--%>

    <%--Warehouse Section Start--%>
    <script>
        function acpAvailableStockEndCall(s, e) {
            if (cacpAvailableStock.cpstock != null) {
                var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                 <%--document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = AvlStk;--%>
                document.getElementById('<%=lblAvailableSStk.ClientID %>').innerHTML = AvlStk;
                <%--document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cacpAvailableStock.cpstock;
                document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;--%>
                //cCmbWarehouse.cpstock = null;
            }



        }
        var SelectWarehouse = "0";
        var SelectBatch = "0";
        var SelectSerial = "0";
        var SelectedWarehouseID = "0";
        var IsFocus = "0";


        //function CallbackPanelEndCall(s, e) {

        //    if (cCallbackPanel.cpEdit != null) {
        //        var strWarehouse = cCallbackPanel.cpEdit.split('~')[0];
        //        var strBatchID = cCallbackPanel.cpEdit.split('~')[1];
        //        var strSrlID = cCallbackPanel.cpEdit.split('~')[2];
        //        var strQuantity = cCallbackPanel.cpEdit.split('~')[3];

        //        SelectWarehouse = strWarehouse;
        //        SelectBatch = strBatchID;
        //        SelectSerial = strSrlID;

        //        cCmbWarehouseID.PerformCallback('BindWarehouse');
        //        cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
        //        checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

        //        cCmbWarehouseID.SetValue(strWarehouse);
        //        ctxtQuantity.SetValue(strQuantity);
        //    }
        //}
        function CallbackPanelEndCall(s, e) {

            var aa = cCallbackPanel.cpEdit;
            if (cCallbackPanel.cpEdit != null) {
                var strWarehouse = cCallbackPanel.cpEdit.split('~')[0];
                var strBatchID = cCallbackPanel.cpEdit.split('~')[1];
                var MfgDate = cCallbackPanel.cpEdit.split('~')[2];
                var ExpiryDate = cCallbackPanel.cpEdit.split('~')[3];
                var strSrlID = cCallbackPanel.cpEdit.split('~')[4];
                var strQuantity = cCallbackPanel.cpEdit.split('~')[5];

                SelectWarehouse = strWarehouse;

                //SelectWarehouse = strWarehouse;
                //SelectBatch = strBatchID;
                //SelectSerial = strSrlID;

                cCmbWarehouseID.PerformCallback('BindWarehouse');
                ctxtQuantity.SetValue(strQuantity);
                ctxtBatchName.SetValue(strBatchID);
                ctxtserialID.SetValue(strSrlID);

                if (MfgDate != "") {
                    var strMfgDate = new Date(GetReverseDateFormat(MfgDate));
                    ctxtStartDate.SetDate(strMfgDate);
                }

                if (ExpiryDate != "") {
                    var strExpiryDate = new Date(GetReverseDateFormat(ExpiryDate));
                    ctxtEndDate.SetDate(strExpiryDate);
                }
            }
        }
        $(document).ready(function () {
            $('.generalTab').click(function () {
                $('.crossBtn').show();
            });
            $('.bilingTab').click(function () {
                if (!$(this).hasClass('dxtcLiteDisabled_PlasticBlue')) {
                    $('.crossBtn').hide();
                }
            });
        });
    </script>
    <script>
        function CmbWarehouseIDEndCallback(s, e) {
            if (SelectWarehouse != "0") {
                cCmbWarehouseID.SetValue(SelectWarehouse);
                SelectWarehouse = "0";
            }
            else {
                cCmbWarehouseID.Focus();
            }
        }
    </script>
    <%--Warehouse Section End--%>

    <style>
        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .dxeButtonEditClearButton_PlasticBlue {
            display: none;
        }

        #txt_Rate {
            min-height: 24px;
        }

        .col-md-3 > label {
            margin-bottom: 3px;
            margin-top: 0;
            display: block;
        }

        .mTop {
            margin-top: 10px;
            padding: 5px 20px;
        }

        #DivBilling [class^="col-md"], #DivShipping [class^="col-md"] {
            padding-top: 5px;
            padding-bottom: 5px;
        }

        #grid_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Subhra Section Start--%>



    <%--Subhra Section End--%>


    <div class="panel-title clearfix">

        <h3 class="pull-left">
            <%--<asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>--%>
            <label>
                <asp:Literal ID="ltrTitle" Text="" runat="server"></asp:Literal>
            </label>
        </h3>

        <div id="pageheaderContent" class="pull-right wrapHolder reverse content horizontal-images" style="display: none;">
            <ul>
                <li>
                    <div class="lblHolder">
                        <table>
                            <tr>
                                <td>Available Stock</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblAvailableSStk" runat="server" Text="0.0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </li>
                <li>
                    <div class="lblHolder">
                        <table>
                            <tr>
                                <td>Stock Quantity</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </li>
                <li>
                    <div class="lblHolder" id="divPacking" style="display: none">
                        <table>
                            <tr>
                                <td>Packing Quantity</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPackingStk" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </li>
                <li>
                    <div class="lblHolder" style="display: none;">
                        <table>
                            <tr>
                                <td>Stock UOM</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </li>

            </ul>
        </div>

        <div class="crossBtn"><a href="OldUnitReceived.aspx"><i class="fa fa-times"></i></a></div>

    </div>
    <div class="form_main">
        <asp:Panel ID="pnl_quotation" runat="server">
            <div class="row">

                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="98%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General" TabStyle-CssClass="generalTab">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="row">
                                        <div id="ddl_numbering" class="col-md-2" runat="server">

                                            <label>
                                                <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" TabIndex="1" OnSelectedIndexChanged="ddl_numberingScheme_SelectedIndexChanged">
                                                
                                                <%-- DataSourceID="SqlSchematype"
                                                DataTextField="SchemaName" DataValueField="ID" onchange="CmbScheme_ValueChange()">--%>
                                            </asp:DropDownList>


                                        </div>
                                        <%--Numbering Scheme--%>
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_SlOrderNo" runat="server" Text="Document No." Width="">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                            </label>

                                            <dxe:ASPxTextBox ID="txt_SlBTOutNo" runat="server" ClientInstanceName="ctxt_SlChallanNo" TabIndex="2" Width="100%" MaxLength="30">
                                                <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                            </dxe:ASPxTextBox>
                                            <span id="MandatorySlOrderNo" style="display: none" class="validclass">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                                        </div>
                                        <%--Received No--%>
                                        <div class="col-md-2" style="display: none;">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date" Width="120px" CssClass="inline">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                            </label>
                                            <dxe:ASPxDateEdit ID="dt_BTOut" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLSalesChallanDate" TabIndex="3" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <ClientSideEvents DateChanged="function(s, e) {DateCheck();}" />
                                                <ClientSideEvents GotFocus="function(s,e){cPLSalesChallanDate.ShowDropDown();}" />
                                            </dxe:ASPxDateEdit>

                                            <span id="MandatorySlDate" style="display: none" class="validclass">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor211_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            <span id="MandatoryEgSDate" style="display: none" class="validclass">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2114_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Sales Order date must not be prior date than quotation date"></span>


                                        </div>
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_BR_No" runat="server" Text="Invoice No." Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxTextBox runat="server" ID="txt_InvoiceNo" ClientInstanceName="Ctxt_InvoiceNo">
                                                <%--<ClientSideEvents Init="txt_InvoiceNoInit" />--%>
                                            </dxe:ASPxTextBox>
                                            <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cSalesOrderComponentPanel"
                                                OnCallback="ComponentSalesOrder_Callback" Visible="false">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxGridLookup ID="lookup_order" runat="server" TabIndex="4" ClientInstanceName="gridSalesOrderLookup" OnDataBinding="lookup_order_DataBinding"
                                                            KeyFieldName="Service_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">

                                                            <Columns>
                                                                <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />--%>


                                                                <dxe:GridViewDataColumn FieldName="Service_IssueNumber" Visible="true" VisibleIndex="1" Caption="Service Out Number" Width="250" Settings-AutoFilterCondition="Contains" />
                                                                <dxe:GridViewDataColumn FieldName="ServiceOut_Date" Visible="true" VisibleIndex="2" Caption="Service Out Date" Width="150" Settings-AutoFilterCondition="Contains" />
                                                            </Columns>
                                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                <Templates>
                                                                    <StatusBar>
                                                                        <table class="OptionsTable" style="float: right">
                                                                            <tr>
                                                                                <td>
                                                                                    <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </StatusBar>
                                                                </Templates>
                                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                <SettingsPager Mode="ShowAllRecords">
                                                                </SettingsPager>
                                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                            </GridViewProperties>
                                                            <ClientSideEvents ValueChanged="function(s, e) { OrderNumberChanged();}" />
                                                            <ClientSideEvents GotFocus="function(s,e){gridSalesOrderLookup.ShowDropDown();}" />
                                                        </dxe:ASPxGridLookup>

                                                    </dxe:PanelContent>
                                                </PanelCollection>
                                                <ClientSideEvents EndCallback="componentEndCallBack" />
                                            </dxe:ASPxCallbackPanel>
                                        </div>
                                        <%--Invoice No--%>
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_OADate" runat="server" Text="Posting Date" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>

                                            <dxe:ASPxDateEdit ID="dt_OADate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLOADate1" TabIndex="12" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </div>
                                        <div class="col-md-2" style="display: none">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_BROut_Date" runat="server" Text=" Date" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>

                                            <asp:Label ID="lbl_MultipleDate" runat="server" Text="Multiple Select Unit Transfer Out Dates" Style="display: none"></asp:Label>

                                            <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxTextBox ID="dt_Quotation" runat="server" TabIndex="5" Width="100%" ClientEnabled="false" ClientInstanceName="cPLQADate" Visible="false">
                                                        </dxe:ASPxTextBox>

                                                        <dxe:ASPxDateEdit ID="dt_PLQuotation" runat="server" EditFormat="Custom" ClientInstanceName="cPLOADate" TabIndex="13" Width="100%">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" ErrorText="Expiry date can not be shorter than Pl/Quote date.">
                                                                <RequiredField IsRequired="true" />
                                                            </ValidationSettings>

                                                            <ClientSideEvents DateChanged="function(s,e){SetDifference1();}"
                                                                Validation="function(s,e){e.isValid = (CheckDifference()>=0)}" />
                                                        </dxe:ASPxDateEdit>
                                                    </dxe:PanelContent>
                                                </PanelCollection>

                                            </dxe:ASPxCallbackPanel>
                                        </div>
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_transferFrom_Branch" runat="server" Text="Unit">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxCallbackPanel runat="server" ID="ComponentTransferFromBranch" ClientInstanceName="cComponentTransferFrom"
                                                OnCallback="ComponentTransferFromBranch_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <asp:DropDownList ID="ddl_transferFrom_Branch" runat="server" Width="100%" TabIndex="6">
                                                        </asp:DropDownList>
                                                    </dxe:PanelContent>
                                                </PanelCollection>

                                            </dxe:ASPxCallbackPanel>
                                        </div>
                                        <%--Branch--%>
                                        <div class="col-md-2" style="display: none;">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_Servicecenter" runat="server" Text="Service Center">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxCallbackPanel runat="server" ID="ServiceCentreCallback" ClientInstanceName="cComponentServiceCentre"
                                                OnCallback="ComponentServiceCentre_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <asp:DropDownList ID="ddl_ServiceCenter" runat="server" Width="100%" TabIndex="5">
                                                        </asp:DropDownList>
                                                    </dxe:PanelContent>
                                                </PanelCollection>

                                            </dxe:ASPxCallbackPanel>
                                        </div>
                                        <div class="col-md-3" style="display: none;">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_transferTo_Branch" runat="server" Text="Transfer To Unit">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <asp:DropDownList ID="ddl_transferTo_Branch" runat="server" Width="100%" TabIndex="6">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2" style="display: none;">
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Call No">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxCallbackPanel runat="server" ID="CallNo" ClientInstanceName="cComponentCallNo"
                                                OnCallback="ComponentCallNo_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxTextBox ID="txtCallNo" runat="server" TabIndex="7" Width="100%" MaxLength="30">
                                                        </dxe:ASPxTextBox>
                                                    </dxe:PanelContent>
                                                </PanelCollection>

                                            </dxe:ASPxCallbackPanel>
                                        </div>
                                        <div class="col-md-2" style="display: none;">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_OANumber" runat="server" Text="OA Number" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>

                                            <dxe:ASPxTextBox ID="txt_OANumber" runat="server" TabIndex="11" Width="100%" MaxLength="50">
                                            </dxe:ASPxTextBox>
                                        </div>

                                        <div class="col-md-2" style="display: none;">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Expiry" CssClass="inline"
                                                    Width="61px">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxDateEdit ID="dt_PlOrderExpiry" runat="server" Style="display: none;" ClientInstanceName="cExpiryDate" EditFormat="Custom" EditFormatString="dd-MM-yyyy" TabIndex="4" Width="100%">

                                                <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" ErrorText="Expiry date can not be shorter than Pl/Quote date.">
                                                    <RequiredField IsRequired="true" />
                                                </ValidationSettings>

                                                <ClientSideEvents DateChanged="function(s,e){SetDifference();}"
                                                    Validation="function(s,e){e.isValid = (CheckDifference()>=0)}" />
                                            </dxe:ASPxDateEdit>

                                        </div>
                                        <div class="col-md-4">
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Remarks">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxCallbackPanel runat="server" ID="Narration" ClientInstanceName="cComponentNarration"
                                                OnCallback="ComponentNarration_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxTextBox ID="txt_Refference" runat="server" TabIndex="8" Width="100%" MaxLength="50" Height="25px">
                                                            <ClientSideEvents LostFocus="function(s, e) { LostFocusedPurpose(e)}" />
                                                        </dxe:ASPxTextBox>
                                                    </dxe:PanelContent>
                                                </PanelCollection>

                                            </dxe:ASPxCallbackPanel>
                                        </div>
                                        <%--Remarks--%>

                                        <div class="col-md-1" style="display: none;">
                                            <label style="margin: 3px 0; display: none;">Currency:  </label>
                                            <div>
                                                <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%"
                                                    DataSourceID="SqlCurrency" DataValueField="Currency_ID" TabIndex="13"
                                                    DataTextField="Currency_AlphaCode" onchange="ddl_Currency_Rate_Change()">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-2" style="display: none;">
                                            <label style="margin: 3px 0; display: none;">Exch. Rate:  </label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate" TabIndex="14">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" style="display: none;">
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientInstanceName="cddl_AmountAre" TabIndex="15" Width="100%">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                            </dxe:ASPxComboBox>

                                        </div>
                                        <div class="col-md-3" style="display: none;">
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" OnCallback="ddl_VatGstCst_Callback" TabIndex="16" Width="100%"></dxe:ASPxComboBox>

                                        </div>
                                        <div class="clear"></div>
                                        <div id="CNNumberingScheme">
                                            <div class="col-md-2" runat="server" id="DivSelectDN">
                                                <label>
                                                    <dxe:ASPxLabel ID="lblDrNumbering" Width="250px" runat="server" Text="Debit Note Numbering Scheme">
                                                    </dxe:ASPxLabel>
                                                </label>
                                               
                                                <asp:DropDownList ID="ddlDNnumbering" runat="server" Width="100%" onchange="CmbScheme_ValueChange()">
                                                </asp:DropDownList>
                                                    
                                            </div>

                                            <div class="col-md-2">
                                                <label>
                                                    <dxe:ASPxLabel ID="lblDNNumber" Width="250px" runat="server" Text="Dr. Note Number">
                                                    </dxe:ASPxLabel>
                                                </label>

                                                <dxe:ASPxTextBox ID="txtDNnumber" runat="server" ClientInstanceName="txtDNnumber" TabIndex="2" Width="100%" MaxLength="30">
                                                </dxe:ASPxTextBox>
                                                <span id="spanValidationDN" style="display: none" class="validclass">
                                                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor3_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                            <div class="col-md-2" aria-disabled="true">
                                                <label>
                                                    <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Unit">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                
                                                        <dxe:PanelContent runat="server">
                                                            <asp:DropDownList ID="ddlDnBranch" DataSourceID="dsBranch" runat="server"  TabIndex="6" DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID"  Width="100%">
                                                            </asp:DropDownList>
                                                        </dxe:PanelContent>
                                                
                                            </div>

                                        </div>













                                        <div style="clear: both;"></div>
                                        <div class="col-md-12">

                                            <div style="display: none;">
                                                <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                                            </div>
                                            <div>
                                                <br />
                                            </div>


                                            <dxe:ASPxGridView runat="server" KeyFieldName="oldUnit_id"
                                                OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                                OnBatchUpdate="grid_BatchUpdate"
                                                OnCustomCallback="grid_CustomCallback"
                                                OnDataBinding="grid_DataBinding"
                                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                                OnRowInserting="Grid_RowInserting"
                                                OnRowUpdating="Grid_RowUpdating"
                                                OnRowDeleting="Grid_RowDeleting"
                                                OnHtmlRowCreated="grid_HtmlRowCreated"
                                                OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                                                Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200">
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Columns>

                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30" VisibleIndex="0" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" Text=" " ForeColor="White">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>
                                                    </dxe:GridViewCommandColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="20">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataButtonEditColumn FieldName="sProducts_Name" ReadOnly="true" Caption="Product" VisibleIndex="3" Width="150">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="oldUnit_id" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--Batch Product Popup End--%>

                                                    <dxe:GridViewDataTextColumn FieldName="sProducts_Description" Caption="Description" VisibleIndex="4" ReadOnly="True" Width="250">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="oldUnit_qty" ReadOnly="true" Caption="Quantity" VisibleIndex="5" Width="70" PropertiesTextEdit-MaxLength="14">
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;" />
                                                        </PropertiesTextEdit>

                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="UOM_Name" Caption="UOM(Sale)" VisibleIndex="6" ReadOnly="true" Width="80">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--<dxe:GridViewDataTextColumn FieldName="oldUnit_value" Caption="Amount" ReadOnly="true" VisibleIndex="8" Width="70">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="oldUnit_value" Caption="Amount" VisibleIndex="8" Width="70">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Quantity_Received" Caption="Received Qty" VisibleIndex="7" Width="6%" HeaderStyle-HorizontalAlign="Right">

                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                            <ClientSideEvents LostFocus="Quantity_Received_TextChanged" />
                                                            <MaskSettings Mask="&lt;0..999999999&gt;" />
                                                        </PropertiesTextEdit>

                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Balance_Quantity" Caption="Balance" VisibleIndex="8" Width="6%" HeaderStyle-HorizontalAlign="Right" PropertiesTextEdit-EnableClientSideAPI="false">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>




                                                    <dxe:GridViewDataTextColumn FieldName="oldUnit_value" Caption="Amount" VisibleIndex="9" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewCommandColumn Width="80" VisibleIndex="10" Caption="Stk Details">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="0" VisibleIndex="12" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <dxe:GridViewDataTextColumn Caption="Quotation No" CellStyle-CssClass="hide" FilterCellStyle-CssClass="hide" EditCellStyle-CssClass="hide" EditFormCaptionStyle-CssClass="hide" FooterCellStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" FieldName="oldUnit_id" Width="0" HeaderStyle-CssClass="hide" VisibleIndex="15">
                                                        <PropertiesTextEdit NullTextStyle-CssClass="hide" ReadOnlyStyle-CssClass="hide" Style-CssClass="hide">

                                                            <NullTextStyle CssClass="hide"></NullTextStyle>

                                                            <ReadOnlyStyle CssClass="hide"></ReadOnlyStyle>

                                                            <Style CssClass="hide"></Style>

                                                        </PropertiesTextEdit>
                                                        <HeaderStyle CssClass="hide" />
                                                        <CellStyle CssClass="hide">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                            </dxe:ASPxGridView>

                                        </div>
                                        <div style="clear: both;"></div>
                                        <br />
                                        <div class="col-md-12">
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" Visible="false" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <%--  <asp:Button ID="ASPxButton2" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />--%>

                                            <dxe:ASPxButton ID="ASPxButton12" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <%--<uc1:ucVehicleDriverControl runat="server" ID="ucVehicleDriverControl" />
                                            <uc4:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                            <asp:HiddenField ID="hfControlData" runat="server" />--%>
                                            <span id="span_viewmodemsg" style="color: Red; font-size: Medium; font-weight: bold;">*** View Mode Only</span>
                                            <asp:Button ID="Button1" runat="server" Text="UDF" Style="display: none;" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />

                                            <dxe:ASPxButton ID="ASPxButton3" Visible="false" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>
                                            <%-- onclick=""--%>
                                            <a href="javascript:void(0);" id="btnAddNew" runat="server" class="btn btn-primary" style="display: none"><span><u>A</u>ttachment(s)</span></a>
                                        </div>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                            <%--test generel--%>
                        </dxe:TabPage>
                        <dxe:TabPage Name="[B]illing/Shipping" Text="[B]illing/Shipping" Visible="false" TabStyle-CssClass="bilingTab">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">



                                    <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server" ContentUrl="AddArea_PopUp.aspx"
                                        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                                        Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                        <ContentCollection>
                                            <dxe:PopupControlContentControl runat="server">
                                            </dxe:PopupControlContentControl>
                                        </ContentCollection>
                                        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                    </dxe:ASPxPopupControl>



                                    <%--Subhra Changes-----------01-02-2017--%>
                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="ComponentPanel_Callback">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                    <div class="row">
                                                        <div class="col-md-5 mbot5" id="DivBilling">
                                                            <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">
                                                                <h5 class="headText" style="margin: 0; padding: 10px 15px 10px; background: #ececec; margin-bottom: 10px">Billing Address</h5>
                                                                <div style="padding-right: 8px">
                                                                    <div class="col-md-4" style="height: auto;">

                                                                        <%--// Sandip Latest  Addres Section Start--%>
                                                                        <%--Type--%>
                                                                        <asp:Label ID="LblType" runat="server" Text="Select Address:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <%--/////////////////--%>
                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxGridLookup ID="billingAddress" runat="server" TabIndex="5" ClientInstanceName="billingLookup"
                                                                                KeyFieldName="add_id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" SelectionMode="Single">
                                                                                <Columns>
                                                                                    <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " VisibleIndex="0"/>--%>
                                                                                    <dxe:GridViewDataColumn FieldName="address" Visible="true" VisibleIndex="1" Caption="Address" Width="250" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="City_Name" Visible="true" VisibleIndex="2" Caption="City" Settings-AllowAutoFilter="False" Width="100">

                                                                                        <%--<Settings AllowAutoFilter="False"></Settings>--%>
                                                                                    </dxe:GridViewDataColumn>
                                                                                    <dxe:GridViewDataColumn FieldName="State" Visible="true" VisibleIndex="3" Caption="State" Width="100" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="pin_code" Visible="true" VisibleIndex="4" Caption="Zip" Width="80" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="Country_Name" Visible="true" VisibleIndex="5" Caption="Country" Width="100" Settings-AutoFilterCondition="Contains" />

                                                                                </Columns>
                                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                                    <Templates>
                                                                                        <StatusBar>
                                                                                            <table class="OptionsTable" style="float: right">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="ClosebillingLookup" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </StatusBar>
                                                                                    </Templates>

                                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                                                    <Settings ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                                </GridViewProperties>
                                                                                <ClientSideEvents TextChanged="function(s, e) { GetBillingAddressDetailByAddressId(e)}" />
                                                                                <ClearButton DisplayMode="Auto">
                                                                                </ClearButton>
                                                                            </dxe:ASPxGridLookup>
                                                                            <%--// Sandip Latest  Addres Section End--%>
                                                                            <%-- <dxe:ASPxComboBox ID="CmbAddressType" ClientInstanceName="CmbAddressType" runat="server" TabIndex="1"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                                                 
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Billing" Value="Billing" Selected="true"></dxe:ListEditItem>
                                                                                     
                                                                                </Items>
                                                                            </dxe:ASPxComboBox>--%>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        <%--Code--%>
                                                                        Address1:
                                                                         <%-- <asp:Label ID="LblCode" runat="server" Text="Short Name (Unique)" CssClass="newLbl"></asp:Label>--%><span style="color: red;"> *</span>

                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtAddress1" MaxLength="80" ClientInstanceName="ctxtAddress1" TabIndex="2"
                                                                                runat="server" Width="100%">
                                                                                <%-- <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                                <%-- <ClientSideEvents TextChanged="function(s,e){fn_ctxtPro_Name_TextChanged()}" />--%>
                                                                            </dxe:ASPxTextBox>
                                                                            <span id="badd1" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        <%--Code--%>
                                                                            Address2:
                                                                           

                                                                    </div>
                                                                    <%--Start of Address2 --%>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtAddress2" MaxLength="80" ClientInstanceName="ctxtAddress2" TabIndex="3"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        <%--Code--%>
                                                                        Address3:
                                                                       <%-- <asp:Label ID="LblCode" runat="server" Text="Short Name (Unique)" CssClass="newLbl"></asp:Label>--%>
                                                                    </div>
                                                                    <%--Start of Address3 --%>

                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtAddress3" MaxLength="80" ClientInstanceName="ctxtAddress3" TabIndex="4"
                                                                                runat="server" Width="100%">
                                                                                <%--<ValidationSettings    ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                            </ValidationSettings>--%>
                                                                                <%-- <ClientSideEvents TextChanged="function(s,e){fn_ctxtPro_Name_TextChanged()}" />--%>
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <%--Start of Landmark --%>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        <%--Code--%>
                                                                            Landmark:
                                                                             

                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtlandmark" MaxLength="80" ClientInstanceName="ctxtlandmark" TabIndex="5"
                                                                                runat="server" Width="100%">
                                                                                <%--<ValidationSettings    ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                            </ValidationSettings>--%>
                                                                                <%-- <ClientSideEvents TextChanged="function(s,e){fn_ctxtPro_Name_TextChanged()}" />--%>
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of Country--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label2" runat="server" Text="Country:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbCountry" ClientInstanceName="CmbCountry" runat="server" TabIndex="6" ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"
                                                                                DataSourceID="CountrySelect" TextField="Country" ValueField="cou_id">
                                                                                <%--<ClearButton DisplayMode="Always"></ClearButton>--%>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>
                                                                                <%--<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="bcountry" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of State--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label4" runat="server" Text="State:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbState" ClientInstanceName="CmbState" runat="server" TabIndex="7"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"
                                                                                DataSourceID="StateSelect" TextField="State" ValueField="ID" OnCallback="cmbState_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }" EndCallback="cmbstate_endcallback"></ClientSideEvents>
                                                                                <%-- <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="bstate" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--start of City/district.--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label6" runat="server" Text="City/District:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbCity" ClientInstanceName="CmbCity" runat="server" TabIndex="8"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True"
                                                                                EnableIncrementalFiltering="True" SelectedIndex="0" DataSourceID="SelectCity" TextField="City" ValueField="CityId" OnCallback="cmbCity_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged(s); }" EndCallback="cmbcity_endcallback"></ClientSideEvents>
                                                                                <%--<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="bcity" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--start of Pin/Zip.--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label8" runat="server" Text="Pin/Zip:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbPin" ClientInstanceName="CmbPin" runat="server" TabIndex="9"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0" DataSourceID="SelectPin" TextField="pin_code" ValueField="pin_id" OnCallback="cmbPin_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents EndCallback="cmbPin_endcallback"></ClientSideEvents>
                                                                                <%--<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="bpin" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>

                                                                        </div>
                                                                    </div>
                                                                    <%--start of Area--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label10" runat="server" Text="Area:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxComboBox ID="CmbArea" ClientInstanceName="CmbArea" runat="server" TabIndex="10"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0" DataSourceID="SelectArea" ValueField="area_id" TextField="area_name" OnCallback="cmbArea_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents EndCallback="cmbArea_endcallback"></ClientSideEvents>
                                                                            </dxe:ASPxComboBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <div class="col-md-12" style="height: auto;">
                                                                        <dxe:ASPxCheckBox ID="chkBilling" runat="server" ClientInstanceName="cchkBilling" Text="Shipping to be in the same location of Billing. ">
                                                                            <ClientSideEvents CheckedChanged="function(s,e){CopyBillingAddresstoShipping(s);}"></ClientSideEvents>
                                                                        </dxe:ASPxCheckBox>
                                                                    </div>

                                                                    <%-- <div class="col-md-offset-4 col-md-8">
                                                                        <a href="#" onclick="javascript:openAreaPage();"><span class="Ecoheadtxt" style="color: Blue">
                                                                            <strong>Add New Area</strong></span></a>
                                                                    </div>--%>
                                                                </div>
                                                            </div>
                                                        </div>


                                                        <div class="col-md-5 mbot5" id="DivShipping">
                                                            <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">

                                                                <h5 class="headText" style="margin: 0; padding: 10px 15px 10px; background: #ececec; margin-bottom: 10px">Shipping Address</h5>
                                                                <div style="padding-right: 8px">
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>

                                                                        <%--// Sandip Latest  Addres Section Start--%>
                                                                        <asp:Label ID="Label1" runat="server" Text="Select Address:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">


                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxGridLookup ID="shippingAddress" runat="server" TabIndex="5" ClientInstanceName="shippingLookup"
                                                                                KeyFieldName="add_id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" SelectionMode="Single">
                                                                                <Columns>
                                                                                    <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " VisibleIndex="0"/>--%>
                                                                                    <dxe:GridViewDataColumn FieldName="address" Visible="true" VisibleIndex="1" Caption="Address" Width="250" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="City_Name" Visible="true" VisibleIndex="2" Caption="City" Settings-AllowAutoFilter="False" Width="100">

                                                                                        <%--<Settings AllowAutoFilter="False"></Settings>--%>
                                                                                    </dxe:GridViewDataColumn>
                                                                                    <dxe:GridViewDataColumn FieldName="State" Visible="true" VisibleIndex="3" Caption="State" Width="100" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="pin_code" Visible="true" VisibleIndex="4" Caption="Zip" Width="80" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="Country_Name" Visible="true" VisibleIndex="5" Caption="Country" Width="100" Settings-AutoFilterCondition="Contains" />

                                                                                </Columns>
                                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                                    <Templates>
                                                                                        <StatusBar>
                                                                                            <table class="OptionsTable" style="float: right">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseshippingLookup" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </StatusBar>
                                                                                    </Templates>

                                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                                                    <Settings ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                                </GridViewProperties>
                                                                                <ClientSideEvents TextChanged="function(s, e) { GetShippingAddressDetailByAddressId(e)}" />
                                                                                <ClearButton DisplayMode="Auto">
                                                                                </ClearButton>
                                                                            </dxe:ASPxGridLookup>
                                                                            <%--// Sandip Latest  Addres Section End--%>
                                                                            <%--<dxe:ASPxComboBox ID="CmbAddressType1" ClientInstanceName="CmbAddressType1" runat="server" TabIndex="11"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Shipping" Value="Shipping"  Selected="true"></dxe:ListEditItem>
                                                                                </Items>
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                            </dxe:ASPxComboBox>--%>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address1: <span style="color: red;">*</span>

                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtsAddress1" MaxLength="80" ClientInstanceName="ctxtsAddress1" TabIndex="12"
                                                                                runat="server" Width="100%">
                                                                                <%--<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxTextBox>
                                                                            <span id="sadd1" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address2:
                                                                           
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxTextBox ID="txtsAddress2" MaxLength="80" ClientInstanceName="ctxtsAddress2" TabIndex="13"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address3: 
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxTextBox ID="txtsAddress3" MaxLength="80" ClientInstanceName="ctxtsAddress3" TabIndex="14"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Landmark: 
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxTextBox ID="txtslandmark" MaxLength="80" ClientInstanceName="ctxtslandmark" TabIndex="15"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto;">

                                                                        <asp:Label ID="Label3" runat="server" Text="Country:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbCountry1" ClientInstanceName="CmbCountry1" runat="server" TabIndex="16"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True"
                                                                                SelectedIndex="0" DataSourceID="CountrySelect" TextField="Country" ValueField="cou_id">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged1(s); }"></ClientSideEvents>
                                                                                <%-- <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="scountry" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of Country--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label5" runat="server" Text="State:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbState1" ClientInstanceName="CmbState1" runat="server" TabIndex="17"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True"
                                                                                SelectedIndex="0" DataSourceID="StateSelect" TextField="State" ValueField="ID" OnCallback="cmbState1_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged1(s); }" EndCallback="cmbshipstate_endcallback"></ClientSideEvents>
                                                                                <%--<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="sstate" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of State--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label7" runat="server" Text="City/District:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbCity1" ClientInstanceName="CmbCity1" runat="server" TabIndex="18"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0" DataSourceID="SelectCity" TextField="City" ValueField="CityId" OnCallback="cmbCity1_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged1(s); }" EndCallback="cmbshipcity_endcallback"></ClientSideEvents>
                                                                                <%-- <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="scity" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of City/District--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label9" runat="server" Text="Pin/Zip:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbPin1" ClientInstanceName="CmbPin1" runat="server" TabIndex="19"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0" DataSourceID="SelectPin" TextField="pin_code" ValueField="pin_id" OnCallback="cmbPin_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents EndCallback="cmbshipPin_endcallback"></ClientSideEvents>
                                                                                <%--<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="spin" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI1" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of Pin/Zip.--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label11" runat="server" Text="Area:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxComboBox ID="CmbArea1" ClientInstanceName="CmbArea1" runat="server" TabIndex="20"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0" DataSourceID="SelectArea" ValueField="area_id" TextField="area_name" OnCallback="cmbArea1_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents EndCallback="cmbshipArea_endcallback"></ClientSideEvents>
                                                                            </dxe:ASPxComboBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <div class="col-md-12" style="height: auto;">
                                                                        <dxe:ASPxCheckBox ID="chkShipping" runat="server" ClientInstanceName="cchkShipping" Text="Billing to be in the same location of Shipping">
                                                                            <ClientSideEvents CheckedChanged="function(s,e){CopyShippingAddresstoBilling(s);}"></ClientSideEvents>
                                                                        </dxe:ASPxCheckBox>
                                                                    </div>
                                                                    <%--<div class="col-md-offset-4 col-md-8">
                                                                        <a href="#" onclick="javascript:openAreaPageShip();"><span class="Ecoheadtxt" style="color: Blue">
                                                                            <strong>Add New Area</strong></span></a>
                                                                    </div>--%>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%--End of Address Type--%>




                                                    <%--End of Area--%>


                                                    <div class="clear"></div>
                                                    <div class="col-md-12 pdLeft0" style="padding-top: 10px">
                                                        <%--   <button class="btn btn-primary">OK</button> ValidationGroup="Address"--%>

                                                        <dxe:ASPxButton ID="btnSave_citys" CausesValidation="true" ClientInstanceName="cbtnSave_citys" runat="server"
                                                            AutoPostBack="False" Text="OK" CssClass="btn btn-primary" TabIndex="26">
                                                            <ClientSideEvents Click="function (s, e) {btnSave_QuoteAddress();}" />
                                                        </dxe:ASPxButton>

                                                    </div>
                                                </div>
                                            </dxe:PanelContent>
                                        </PanelCollection>
                                        <ClientSideEvents EndCallback="Panel_endcallback" />
                                    </dxe:ASPxCallbackPanel>



                                    <%--         <dxe:ASPxPopupControl ID="Popup_SalesQuote" runat="server" ClientInstanceName="Popup_SalesQuote"
                Width="550px" HeaderText="Add/Modify Address" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" OnWindowCallback="Popup_SalesQuote_WindowCallback"
                Modal="True" EnableHierarchyRecreation="True">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                      
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>

                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                <ClientSideEvents EndCallback="function (s, e) {Popup_SalesQuote_EndCallBack();}" />
            </dxe:ASPxPopupControl>--%>
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                    </TabPages>
                    <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
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
            </div>
            <asp:SqlDataSource ID="CountrySelect" runat="server" 
                SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>
            <asp:SqlDataSource ID="StateSelect" runat="server" 
                SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">
                <SelectParameters>
                    <asp:Parameter Name="State" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SelectCity" runat="server" 
                SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
                <SelectParameters>
                    <asp:Parameter Name="City" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SelectArea" runat="server" 
                SelectCommand="SELECT area_id, area_name from tbl_master_area where (city_id = @Area) ORDER BY area_name">
                <SelectParameters>
                    <asp:Parameter Name="Area" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SelectPin" runat="server" 
                SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City order by pin_code">
                <SelectParameters>
                    <asp:Parameter Name="City" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="sqltaxDataSource" runat="server" 
                SelectCommand="select Taxes_ID,Taxes_Name from dbo.Master_Taxes"></asp:SqlDataSource>

            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="AddArea_PopUp.aspx"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>




            <%--Subhabrata Start Popup--%>
            <dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
                Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                <HeaderTemplate>
                    <strong><span style="color: #fff">Select Products</span></strong>
                    <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                        <ClientSideEvents Click="function(s, e){ 
                                                            cProductsPopup.Hide();
                                                        }" />
                    </dxe:ASPxImage>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <div style="padding: 7px 0;">
                            <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                            <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                            <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                        </div>
                        <dxe:ASPxGridView runat="server" KeyFieldName="Key_UniqueId" ClientInstanceName="cgridproducts" ID="grid_Products"
                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                            Settings-ShowFooter="false" AutoGenerateColumns="False" OnHtmlRowCreated="aspxGridProduct_HtmlRowCreated"
                            OnRowInserting="Productgrid_RowInserting" OnRowUpdating="Productgrid_RowUpdating" OnRowDeleting="Productgrid_RowDeleting" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                            <%-- <Settings VerticalScrollableHeight="450" VerticalScrollBarMode="Auto"></Settings>--%>
                            <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />
                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="45" ReadOnly="true" Caption="Sl. No.">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Product_Shortname" ReadOnly="true" Width="100" Caption="Product Name">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Description" Width="200" ReadOnly="true" Caption="Product Description">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Service_Id" ReadOnly="true" Caption="Stock Transfer Id" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="StkDetails_Id" ReadOnly="true" Caption="StkDetailsId" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Indent" Width="90" ReadOnly="true" Caption="Service Out No.">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                    <PropertiesTextEdit>
                                        <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                        <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
                            <SettingsDataSecurity AllowEdit="true" />
                            <%--<SettingsEditing Mode="Batch">
                                            <BatchEditSettings EditMode="row" />
                                        </SettingsEditing>--%>
                            <%--<ClientSideEvents EndCallback=" cgridTax_EndCallBack " />--%>
                        </dxe:ASPxGridView>
                        <div class="text-center">
                            <asp:Button ID="Button2" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            <%-- End--%>
            <%--Sudip--%>

            <div class="PopUpArea">


                <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
                <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
                <asp:HiddenField ID="hdfProductIDPC" runat="server" />
                <asp:HiddenField ID="hdfstockidPC" runat="server" />
                <asp:HiddenField ID="hdfopeningstockPC" runat="server" />
                <asp:HiddenField ID="hdbranchIDPC" runat="server" />
                <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />
                <%--ChargesTax--%>
                <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
                    Width="900px" Height="300px" HeaderText="Quotation Taxes" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div class="Top clearfix">
                                <div id="content-5" class="col-md-12  wrapHolder content horizontal-images" style="width: 100%; margin-right: 0;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Gross Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableGross"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductAmount">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="lblChargesGSTforGross">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>GST</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblChargesGSTforGross" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforGross">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Total Discount</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductDiscount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductDiscount">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Total Charges</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductTaxAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductTaxAmount"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Net Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableNet"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductNetAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductNetAmount"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="lblChargesGSTforNet">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>GST</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblChargesGSTforNet" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforNet">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <div class="clear">
                                </div>
                                <%--Error Msg--%>

                                <div class="col-md-8" id="ErrorMsgCharges">
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Status
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Tax Code/Charges Not Defined.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>

                                </div>

                                <div class="clear">
                                </div>
                                <div class="col-md-12 gridTaxClass" style="">
                                    <dxe:ASPxGridView runat="server" KeyFieldName="TaxID" ClientInstanceName="gridTax" ID="gridTax"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                        Settings-ShowFooter="false" OnCustomCallback="gridTax_CustomCallback" OnBatchUpdate="gridTax_BatchUpdate"
                                        OnRowInserting="gridTax_RowInserting" OnRowUpdating="gridTax_RowUpdating" OnRowDeleting="gridTax_RowDeleting"
                                        OnDataBinding="gridTax_DataBinding">
                                        <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn FieldName="TaxName" Caption="Tax" VisibleIndex="0" Width="40%" ReadOnly="true">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="calCulatedOn" Caption="Calculated On" VisibleIndex="0" Width="20%" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="PercentageTextChange" />
                                                    <ClientSideEvents />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="QuotationTaxAmountTextChange" GotFocus="QuotationTaxAmountGotFocus" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents EndCallback="OnTaxEndCallback" />
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                        </SettingsEditing>
                                    </dxe:ASPxGridView>
                                </div>
                                <div class="col-md-12">
                                    <table style="" class="chargesDDownTaxClass">
                                        <tr class="chargeGstCstvatClass">
                                            <td style="padding-top: 10px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                            <td style="padding-top: 10px; width: 200px;">
                                                <dxe:ASPxComboBox ID="cmbGstCstVatcharge" ClientInstanceName="ccmbGstCstVatcharge" runat="server" SelectedIndex="0" TabIndex="2"
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                                    OnCallback="cmbGstCstVatcharge_Callback">
                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                        <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />

                                                    </Columns>
                                                    <ClientSideEvents SelectedIndexChanged="ChargecmbGstCstVatChange"
                                                        GotFocus="chargeCmbtaxClick" />

                                                </dxe:ASPxComboBox>



                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px; width: 200px;">
                                                <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" ClientInstanceName="ctxtGstCstVatCharge" TabIndex="3" ReadOnly="true" Text="0.00"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                                </dxe:ASPxTextBox>

                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px">
                                                <input type="button" onclick="recalculateTaxCharge()" class="btn btn-info btn-small RecalculateCharge" value="Recalculate GST" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clear">
                                    <br />
                                </div>



                                <div class="col-sm-3">
                                    <div>
                                        <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AccessKey="X" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                            <ClientSideEvents Click="function(s, e) {cPopup_Taxes.Hide();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </div>

                                <div class="col-sm-9">
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-right: 30px"><strong>Total Charges</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="<-999999999..99999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                        <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>

                                            </td>
                                            <td style="padding-right: 30px; padding-left: 5px"><strong>Total Amount</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="<-999999999..99999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                        <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <span></span>
                                </div>
                                <div class="col-sm-4">
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <span></span>
                                </div>
                                <div class="col-sm-4">
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>





                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
                <%-- kaushik 20-2-2017 --%>

                <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                        </dxe:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
                </dxe:ASPxCallbackPanel>
                <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                        </dxe:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
                </dxe:ASPxCallbackPanel>

                <%-- kaushik 20-2-2017--%>
            </div>
            <div>
                <asp:HiddenField ID="hdfIsDelete" runat="server" />
                <asp:HiddenField ID="hdfSerialDetails" runat="server" />
                <asp:HiddenField ID="hdfBatchDetails" runat="server" />
                <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
                <asp:HiddenField ID="hddnOrderNumber" runat="server" />
                <asp:HiddenField ID="hdfProductID" runat="server" />
                <asp:HiddenField ID="hdfProductSerialID" runat="server" />
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdfProductType" runat="server" />
                <asp:HiddenField ID="hdnProductQuantity" runat="server" />
                <asp:HiddenField ID="hdntab2" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnCustomerId" runat="server" />
                <asp:HiddenField ID="hdnAddressDtl" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hdnSchemaLength" runat="server" />
                <asp:HiddenField ID="hdndefaultID" runat="server" />
                <%--kaushik 24-2-2017 --%>
                <asp:HiddenField runat="server" ID="IsUdfpresent" />
                <%--kaushik 24-2-2017--%>
            </div>
            <%--    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="CallbackPanelEndCall" />
            </dxe:ASPxCallbackPanel>

            <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
            </dxe:ASPxCallbackPanel>--%>
            <%--End Sudip--%>

            <%--Debu Section--%>


            <%--Debu Section End--%>
        </asp:Panel>

        <asp:SqlDataSource ID="SqlCurrency" runat="server" 
            SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrencyBind" runat="server" ></asp:SqlDataSource>
        <asp:HiddenField ID="hdnCurrenctId" runat="server" />
        <%--kaushik 24-2-2017--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>


    <%--Batch Product Popup Start--%>

    <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
        Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By Product Name</strong></label>
                <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                    KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                    <Columns>
                        <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="220">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120">
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
        SelectCommand="prc_SalesCRM_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
        </SelectParameters>
    </asp:SqlDataSource>

    <%--Batch Product Popup End--%>

    <%--Warehouse Details Start--%>

    <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
        Width="900px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeWarehouse(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                        <ul>
                            <li>
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Selected Product</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblProductName" runat="server"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Entered Quantity </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="txt_SalesAmount" runat="server"></asp:Label>
                                                    <asp:Label ID="txt_SalesUOM" runat="server"></asp:Label>

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="lblHolder" id="divpopupAvailableStock" style="display: none;">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Available Stock</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAvailableStock" runat="server"></asp:Label>
                                                    <asp:Label ID="lblAvailableStockUOM" runat="server"></asp:Label>

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li style="display: none;">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Stock Quantity </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="txt_StockAmount" runat="server"></asp:Label>
                                                    <asp:Label ID="txt_StockUOM" runat="server"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>

                        </ul>
                    </div>
                    <div class="clear">
                        <br />
                    </div>
                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                        <div>
                            <div class="col-md-3" id="div_Warehouse">
                                <div>
                                    Warehouse
                                </div>
                                <div class="Left_Content" style="">
                                    <%-- --%>
                                    <dxe:ASPxComboBox ID="CmbWarehouseID" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouseID" SelectedIndex="0"
                                        TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouseID_Callback">
                                        <ClientSideEvents EndCallback="CmbWarehouseIDEndCallback"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                    <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_Batch">
                                <div>
                                    Batch/Lot
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxTextBox ID="txtBatchName" runat="server" Width="100%" ClientInstanceName="ctxtBatchName" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>
                                    <span id="spntxtBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_Manufacture">
                                <div>
                                    Manufacture Date
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxDateEdit ID="txtStartDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtStartDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_Expiry">
                                <div>
                                    Expiry Date
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxDateEdit ID="txtEndDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtEndDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                            <div class="clear" id="div_Break"></div>
                            <div class="col-md-3" id="div_Quantity">
                                <div>
                                    Quantity
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                        <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />
                                    </dxe:ASPxTextBox>
                                    <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_Serial">
                                <div>
                                    Serial No
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxTextBox ID="txtserialID" runat="server" Width="100%" ClientInstanceName="ctxtserialID" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                        <ClientSideEvents LostFocus="SubmitWarehouse" />
                                    </dxe:ASPxTextBox>
                                    <span id="spntxtserialID" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="col-md-3">
                                <div>
                                </div>
                                <div class="Left_Content" style="padding-top: 14px">
                                    <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="SubmitWarehouse" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cGrdWarehouse" OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                    VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Batch/Lot Number" FieldName="BatchNo"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="ViewMfgDate"
                                    VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ViewExpiryDate"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                    VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="6" Width="120px">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete">
                                            <img src="../../../assests/images/Edit.png" /></a>
                                        &nbsp;
                                        <a href="javascript:void(0);" onclick="fn_Delete('<%# Container.KeyValue %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>
                                        <a class="anchorclass" style='<%#Eval("IsOutStatusMsg")%>'></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnWarehouseEndCallback" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <%--Warehouse Details End--%>

    <%--InlineTax--%>

    <dxe:ASPxPopupControl ID="ASPxPopupControl3" runat="server" ClientInstanceName="caspxTaxpopUp"
        Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <span style="color: #fff"><strong>Select Tax</strong></span>
            <dxe:ASPxImage ID="ASPxImage2" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
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
                                            <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="" ClientInstanceName="clblTaxProdGrossAmt"></dxe:ASPxLabel>
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

                    <div class="col-sm-3">
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
                                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="" ClientInstanceName="clblProdNetAmt"></dxe:ASPxLabel>
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
                            <dxe:ASPxTextBox ID="ASPxTextBox1" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" TabIndex="1" ReadOnly="true"
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
                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
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
                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
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
                                <asp:Button ID="Button4" runat="server" Text="Ok" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
                                <asp:Button ID="Button5" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                            <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" /> --%>
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
    <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:SqlDataSource ID="SqlSchematype" runat="server" 
        SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='31' and financial_year_id IN(select FinYear_ID from Master_FinYear where FinYear_Code=@year) and Branch=@userbranch and comapanyInt=@company)) as X Order By ID ASC">
        <SelectParameters>
            <asp:SessionParameter Name="userbranch" SessionField="userbranch" Type="string" />
            <asp:SessionParameter Name="company" SessionField="LastCompany1" Type="string" />
            <asp:SessionParameter Name="year" SessionField="LastFinYear1" Type="string" />

        </SelectParameters>
    </asp:SqlDataSource>
    <asp:HiddenField ID="hdnmodeId" runat="server" />
    <asp:HiddenField ID="hasBalanceQty" runat="server" />
    <asp:HiddenField ID="HdnDnNumbering" runat="server" />
    <asp:HiddenField ID="PosBranchId" runat="server" />
        <asp:SqlDataSource ID="dsBranch" runat="server" 
        ConflictDetection="CompareAllValues"
        SelectCommand=""></asp:SqlDataSource>
</asp:Content>
