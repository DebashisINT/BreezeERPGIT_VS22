<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerPendingDelivery.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.CustomerPendingDelivery" EnableEventValidation="false" %>

<%--//chinmoy edited below code start 03-04-2020 start--%>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Sales_BillingShipping.ascx" TagPrefix="ucBS" TagName="Sales_BillingShipping" %>
<%--//End--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/ucVehicleDriverControl.ascx" TagPrefix="uc1" TagName="ucVehicleDriverControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/CustomerPendingDelivery.js?V=1.0.0.9"></script>
    <style type="text/css">
        .gridStatic {
            margin-top: 25px;
        }
            .gridStatic .dynamicPopupTbl {
                width: 100%;
            }

        .dynamicPopupTbl > thead > tr > th, .dynamicPopupTbl > tbody > tr > td {
            padding: 5px 8px !important;
        }

        .dynamicPopupTbl > tbody > tr > td {
            cursor: pointer;
        }

        .dynamicPopupTbl.back > thead > tr > th {
            background: #4e64a6;
            color: #fff;
        }

        .dynamicPopupTbl.back > tbody > tr > td {
            background: #fff;
        }

        .dynamicPopupTbl > tbody > tr > td input {
            border: none !important;
            cursor: pointer;
            background: transparent !important;
        }

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
            right: -3px;
            top: 18px;
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
            top: 15px;
        }

        .mandt {
            position: absolute;
            right: -18px;
            top: 4px;
        }

        .msgStyle {
            font-size: 18px;
            color: red;
        }
    </style>

    <%--    <link href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>--%>
    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>

    <%--    <link href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css"
        rel="stylesheet" type="text/css" />
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js"
        type="text/javascript"></script>--%>
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/json2/20130526/json2.min.js"></script>
    <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>




    <script type="text/javascript">
        //chinmoy added 03-04-2020 start

        $(document).ready(function () {

            if ($("#hdnShowUOMConversionInEntry").val() == "1")
            {
                _div_Uom.style.display = 'block';
                div_AltQuantity.style.display = 'block';
            }
            else
            {
                _div_Uom.style.display = 'none';
                div_AltQuantity.style.display = 'none';
            }

        })

        var PosGstId = "";
        function PopulatePosGst(e) {

            PosGstId = cddl_PosGst.GetValue();
            if (PosGstId == "S") {
                cddl_PosGst.SetValue("S");
            }
            else if (PosGstId == "B") {
                cddl_PosGst.SetValue("B");
            }

        }

        function AfterSaveBillingShipiing(validate) {



            if (validate) {
                page.SetActiveTabIndex(0);
                page.tabs[0].SetEnabled(true);
                $("#divcross").show();

            }
            else {
                page.SetActiveTabIndex(1);
                page.tabs[0].SetEnabled(false);
                $("#divcross").hide();
            }
        }


        //End
        function ShowSerialTextOrNot(element) {
            //$("#spnCmbWarehouse").hide();
            //$("#spnCmbBatch").hide();
            //$("#spncheckComboBox").hide();
            //$("#spntxtQuantity").hide();

            if (document.getElementById("myCheckSerial").checked == true) {
                _div_text.style.display = "block";
                divSerialList.style.display = "none";
                // cbtnWarehouse.SetVisible(false);
                //checkComboBox.Focus();
            }
            else {
                _div_text.style.display = "none";
                divSerialList.style.display = "block";
                //cbtnWarehouse.SetVisible(true);
                // ctxtserial.Focus();
            }
        }
        $(function () {
            $('[id*=lstSerial]').multiselect({
                includeSelectAllOption: true
                // SerialList.push();
            });
        });
    </script>

    <script type="text/javascript">
        function GlobalBillingShippingEndCallBack() { };

    </script>
    <script>

        var SelectedObj = [];
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

        function Onddl_VatGstCstEndCallback(s, e) {
            if (s.GetItemCount() == 1) {
                cddlVatGstCst.SetEnabled(false);
            }
        }
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
            if (event.keyCode == 78 && isCtrl == true && getUrlVars().req != "V") { //run code for alt+N -- ie, Save & New  
                StopDefaultAction(e);
                Save_ButtonClick();
            }
            else if ((event.keyCode == 120 || event.keyCode == 88) && isCtrl == true && getUrlVars().req != "V") { //run code for Ctrl+X -- ie, Save & Exit!     
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

    <%--Batch Product Popup Start on 14-03-2017--%>

    <script>
        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;
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

        function fn_Edit(keyValue) {

            //cGrdWarehouse.PerformCallback('EditWarehouse~' + keyValue);
            SelectedWarehouseID = keyValue;
            cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
        }

        function ProductButnClick(s, e) {

            //cproductLookUp.gridView.Refresh();
            if (e.buttonIndex == 0) {

                if (cproductLookUp.Clear()) {

                    //var IsInventoryValue = ccmbIsInventory.GetValue();
                    //cIsInventory.PerformCallback('BindSession' + '~' + IsInventoryValue);

                    cProductpopUp.Show();
                    cproductLookUp.Focus();
                    cproductLookUp.ShowDropDown();


                }
            }
        }
        function ProductLostFocused(s, e) {

            grid.GetEditor("Quantity").Focus();
        }


        function cmbContactPersonEndCall(s, e) {

            if (cContactPerson.cpDueDate != null) {
                var DeuDate = cContactPerson.cpDueDate;
                var myDate = new Date(DeuDate);

                var invoiceDate = new Date();
                var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));

                ctxtCreditDays.SetValue(datediff);

                cdt_SaleInvoiceDue.SetDate(myDate);
                cContactPerson.cpDueDate = null;
            }

            if (cContactPerson.cpTotalDue != null) {
                var TotalDue = cContactPerson.cpTotalDue;
                if ((TotalDue * 1) < 0) {
                    document.getElementById('<%=lblTotalDues.ClientID %>').innerHTML = (TotalDue * (-1)) + " " + "Cr";
                    document.getElementById('<%=lblTotalDues.ClientID %>').style.color = "red";
                }
                else {
                    document.getElementById('<%=lblTotalDues.ClientID %>').innerHTML = TotalDue + " " + "Db";
                }

                pageheaderContent.style.display = "block";
                divDues.style.display = "block";
                cContactPerson.cpTotalDue = null;
            }
        }

        function acbpCrpUdfEndCall(s, e) {


            if (cacbpCrpUdf.cpUDF) {


                if (cacbpCrpUdf.cpUDF == "true" && cacbpCrpUdf.cpTransport == "true" && cacbpCrpUdf.cpTC == "true") {
                    LoadingPanel.Hide();
                    var JsonProductStock = JSON.stringify(StockOfProduct);
                    GetObjectID('hdnJsonProductStock').value = JsonProductStock;
                    grid.UpdateEdit();
                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                    cacbpCrpUdf.cpTC = null;
                }
                else if (cacbpCrpUdf.cpUDF == "false") {
                    LoadingPanel.Hide();
                    jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                    cacbpCrpUdf.cpTC = null;
                }
                else if (cacbpCrpUdf.cpTC == "false") {
                    LoadingPanel.Hide();
                    jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                    cacbpCrpUdf.cpTC = null;
                }
                else {
                    LoadingPanel.Hide();
                    jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                    cacbpCrpUdf.cpTC = null;
                }

            }
        }

        function CreditDays_TextChanged(s, e) {

            var CreditDays = ctxtCreditDays.GetValue();

            var today = new Date();
            var newdate = new Date();
            newdate.setDate(today.getDate() + Math.round(CreditDays));

            cdt_SaleInvoiceDue.SetDate(newdate);
        }


        function ddlInventory_OnChange() {
            //cproductLookUp.GetGridView().Refresh();
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
            grid.GetEditor("ProductName").SetText(ProductCode);


            pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");

            var ProductID = LookUpData;
            if (LookUpData != null) {
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
                // cacpAvailableStock.PerformCallback(strProductID);
                var PostingDate = new Date();
                PostingDate = cPLSalesChallanDate.GetValueString();
                var branch=$("#ddl_Branch").val();
         
                AvailableStockCheck(PostingDate, branch, strProductID);
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }

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

            var Amount = QuantityValue * strFactor * ((Saleprice.replace(/\,/g, '') * 1) / strRate);
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);


            var TotaAmountRes = '';
            TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));
            //tbTotalAmount.SetValue(amountAfterDiscount);


            //Debjyoti section GST
            //chinmoy edited below code 03-04-2020 start
            var ShippingStateCode = cbsSCmbState.GetValue();
            var CompareStateCode;
            if (cddl_PosGst.GetValue() == "S") {
                CompareStateCode = GeteShippingStateID();
            }
            else {
                CompareStateCode = GetBillingStateID();
            }
            //End
            var TaxType = "";
            if (cddl_AmountAre.GetValue() == "1") {
                TaxType = "E";
            }
            else if (cddl_AmountAre.GetValue() == "2") {
                TaxType = "I";
            }

            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
                SpliteDetails[17], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val());
            //END


            //var tbAmount = grid.GetEditor("Amount");
            //tbAmount.SetValue(Amount); 
            //var tbTotalAmount = grid.GetEditor("TotalAmount");
            //tbTotalAmount.SetValue(Amount); 
            //DiscountTextChange(s, e);
        }

        //'Subhabrata' on 15-03-2017
        function CmbWarehouseEndCallback(s, e) {
            if (SelectWarehouse != "0") {
                cCmbWarehouse.SetValue(SelectWarehouse);
                SelectWarehouse = "0";
            }
            else {
                cCmbWarehouse.SetEnabled(true);
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

            var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();
            if (SelectSerial != "0") {
                var values = [SelectSerial];
                checkListBox.SelectValues(values);
                UpdateSelectAllItemState();
                UpdateText();
                SelectSerial = "0";
                cCmbBatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
            }
            else {
                checkComboBox.SetText(0 + " Items");
            }
            var msg = checkListBox.cpFifoMsg;
            if (msg == 'Check Not Possible') {
                alert('NA');
            }
            if (FifoExists == "1") {
                checkListBox.SelectAll();
                checkListBox.SetEnabled(false);
                UpdateSelectAllItemState();
                UpdateText();
            }
            else {
                checkListBox.SetEnabled(true);
            }

        }

        function ctaxUpdatePanelEndCall(s, e) {
            if (ctaxUpdatePanel.cpstock != null) {
                divAvailableStk.style.display = "block";
                divpopupAvailableStock.style.display = "block";

                var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;
                document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;

                ctaxUpdatePanel.cpstock = null;
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return false;
            }
        }

        function DiscountTextChange(s, e) {

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
            var Amount = QuantityValue * strFactor * ((strSalePrice.replace(/\,/g, '') * 1) / strRate);

            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);

            var TotaAmountRes = '';
            TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));
            //tbTotalAmount.SetValue(amountAfterDiscount);

            //Debjyoti section GST
            //chinmoy edited below code starft 03-04-2020
            //var ShippingStateCode = cbsSCmbState.GetValue();

            var CompareStateCode;
            if (cddl_PosGst.GetValue() == "S") {
                CompareStateCode = GeteShippingStateID();
            }
            else {
                CompareStateCode = GetBillingStateID();
            }
            //end
            var TaxType = "";
            if (cddl_AmountAre.GetValue() == "1") {
                TaxType = "E";
            }
            else if (cddl_AmountAre.GetValue() == "2") {
                TaxType = "I";
            }

            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
                SpliteDetails[17], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val());
            //END


            //Debjyoti 
            //grid.GetEditor('TaxAmount').SetValue(0);
            //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
        }
        <%--kaushik Section--%>

        function CmbScheme_ValueChange() {

            var val = $("#ddl_numberingScheme").val();

            $.ajax({
                type: "POST",
                url: 'PurchaseChallan.aspx/getSchemeType',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{sel_scheme_id:\"" + val + "\"}",
                success: function (type) {

                    var schemetypeValue = type.d;
                    var schemetype = schemetypeValue.toString().split('~')[0];
                    var schemelength = schemetypeValue.toString().split('~')[1];
                    $('#txt_SlChallanNo').attr('maxLength', schemelength);
                    if (schemetype == '0') {

                        <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";--%>
                        ctxt_SlChallanNo.SetText('');
                        ctxt_SlChallanNo.SetEnabled(true);
                        ctxt_SlChallanNo.Focus();

                    }
                    else if (schemetype == '1') {

                        <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";--%>
                        ctxt_SlChallanNo.SetText('Auto');
                        ctxt_SlChallanNo.SetEnabled(false);
                        cPLSalesChallanDate.Focus();
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
            });
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
        function CmbWarehouse_ValueChange() {
            var isFIFORequired = false;

            var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();

            var WarehouseID = cCmbWarehouse.GetValue();
            $("#<%=hddnWarehouseId.ClientID%>").val(WarehouseID);
            var type = document.getElementById('hdfProductType').value;
            ctxtMatchQty.SetValue(0);
            if (WarehouseID != null) {
                if (type == "WBS" || type == "WB") {
                    cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
                }
                else if (type == "WS" && FifoExists == "0") {
                    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + 'NoFIFO');
                }
            }       
        }
        //Rev Rajdip for wirehousewise aviable stock
        function getwirehousewiseaviablestock(sl, strProductID, branch, WarehouseID) {
            

            grid.batchEditApi.StartEdit(globalRowIndex);
            //var otherProddet = {};
            //otherProddet.sl = sl;
            //otherProddet.strProductID = strProductID;

            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/getWarehousewisestock",
                data: JSON.stringify({ sl: sl, strProductID: strProductID, branch: branch, WarehouseID: WarehouseID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    cpstockVal = msg.d;
                    divpopupAvailableStock.style.display = "block";

                    //var AvlStk = cpstockVal + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                    //Rev Rajdip
                    //document.getElementById('<%=lblAvailableSStk.ClientID %>').innerHTML = cpstockVal;
                    //End Rev rajdip
                    document.getElementById('<%=lblavlstock.ClientID %>').innerHTML = cpstockVal;
                    //document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cpstockVal;
                    //document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;

                    cpstockVal = null;
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                    return false;


                }
            });
        }

        //end Rev Rajdip
        function CmbBatch_ValueChange() {
        
            var WarehouseID = cCmbWarehouse.GetValue();
            var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();


            var BatchID = cCmbBatch.GetValue();
            ctxtMatchQty.SetValue(0);
            $("#<%=hddnBatchId.ClientID%>").val(BatchID);
            var type = document.getElementById('hdfProductType').value;
            //var qty = grid.GetEditor('Quantity').GetValue();
            if (type == "WBS" && FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + 'NoFIFO');
            }
            else if (type == "BS" && FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + 'NoFIFO');
            }
            else if (type == "WS" && FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0" + '~' + 'NoFIFO');
            }
        }
        //tab start

        //tab end


        <%--kaushik 24-2-2017--%>

        $(document).ready(function () {

            //chinmoy start 03-04-2020 
            if (url.key != 'ADD') {
                //SetDefaultBillingShippingAddress($("#hdnCustomerId").val());

                cddl_PosGst.SetEnabled(false);

            }
            //end
            if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                page.GetTabByName('Billing/Shipping').SetEnabled(false);
            }
            $('#ApprovalCross').click(function () {

                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.Refresh();
            })

            //var IsInventoryValue = ccmbIsInventory.GetValue();
            //cIsInventory.PerformCallback('BindSession' + '~' + IsInventoryValue);
        })

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
            var startDate;
            if (gridSalesOrderLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        page.SetActiveTabIndex(0);
                        ccmbGstCstVat.PerformCallback();
                        ccmbGstCstVatcharge.PerformCallback();
                        //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                      
                        deleteTax("DeleteAllTax", "", "");
                        startDate = cPLSalesChallanDate.GetValueString();


                        var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                        cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                        if (key != null && key != '') {
                            if (type != '' && type != null) {
                                cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                            }
                            //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                        }
                        grid.PerformCallback('GridBlank');
                    }

                });
            }
            else {
                page.SetActiveTabIndex(0);
                ccmbGstCstVat.PerformCallback();
                ccmbGstCstVatcharge.PerformCallback();
                // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
               
                deleteTax("DeleteAllTax", "", "");
                startDate = cPLSalesChallanDate.GetValueString();


                var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                if (key != null && key != '') {
                    if (type != '' && type != null) {
                        cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                    }
                    //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                }
                grid.PerformCallback('GridBlank');
            }
            gridSalesOrderLookup.gridView.Refresh();
            cProductsPopup.Hide();
        }

        function CloseGridQuotationLookup() {
            gridSalesOrderLookup.ConfirmCurrentSelection();
            gridSalesOrderLookup.HideDropDown();
            gridSalesOrderLookup.Focus();
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
        function RecalCulateTaxTotalAmountCharges() {
            var totalTaxAmount = 0;
            for (var i = 0; i < chargejsonTax.length; i++) {
                gridTax.batchEditApi.StartEdit(i, 3);
                var totLength = gridTax.GetEditor("TaxName").GetText().length;
                var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
                if (sign == '(+)') {
                    totalTaxAmount = totalTaxAmount + parseFloat(gridTax.GetEditor("Amount").GetValue());
                } else {
                    totalTaxAmount = totalTaxAmount - parseFloat(gridTax.GetEditor("Amount").GetValue());
                }

                gridTax.batchEditApi.EndEdit();
            }

            totalTaxAmount = totalTaxAmount + parseFloat(ctxtGstCstVatCharge.GetValue());

            ctxtQuoteTaxTotalAmt.SetValue(Math.round(totalTaxAmount));
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
        }

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
                ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
                SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                //gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
                ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
                SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
            }

            RecalCulateTaxTotalAmountCharges();

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
                //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
                ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
                ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
            }

            SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
            SetChargesRunningTotal();

            RecalCulateTaxTotalAmountCharges();
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
                        var Amount = Math.round(QuantityValue * strFactor * ((strSalePrice.replace(/\,/g, '') * 1) / strRate)).toFixed(2);
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

                            var shippingStCode = '';

                            ////###### Added By : Samrat Roy ##########
                            //Get Customer Shipping StateCode
                            var shippingStCode = '';

                            var shippingStCode = '';

                            //chinmoy edited below code start 03-04-2020


                            if (cddl_PosGst.GetValue() == "S") {
                                shippingStCode = GeteShippingStateCode();
                            }
                            else {
                                shippingStCode = GetBillingStateCode();
                            }

                            //shippingStCode = cbsSCmbState.GetText();
                            //shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();
                            //End
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
                // grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));
                var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue());
                var totalRoundOffAmount = Math.round(totalNetAmount);
                // grid.GetEditor("TaxAmount").SetValue(totalRoundOffAmount);

                grid.GetEditor("Amount").SetValue(parseFloat(grid.GetEditor("Amount").GetValue()) + (totalRoundOffAmount - totalNetAmount));

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
            rowEditCtrl = s;
        }

        $(document).ready(function () {
            ctxtRate.SetValue("");
            ctxtRate.SetEnabled(false);
            ctxt_SlChallanNo.SetEnabled(false);
            gridSalesOrderLookup.SetEnabled(false);
            PopulateLoadGSTCSTVAT();
        });
    </script>

    <%--Debu Section End--%>





    <script type="text/javascript">
        var QuantityRes = '';
        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
            gridSalesOrderLookup.SetEnabled(true);
        }
        function GetContactPerson(e) {


            var startDate = new Date();
            startDate = cPLSalesChallanDate.GetValueString();
            if (gridSalesOrderLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                        GetObjectID('hdnCustomerId').value = key;
                        if (key != null && key != '') {

                            if (type != '' && type != null) {
                                cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                            }
                            grid.PerformCallback('GridBlank');
                            //var startDate = new Date();
                            //startDate = cPLSalesChallanDate.GetValueString();

                            cContactPerson.PerformCallback('BindContactPerson~' + key);


                            page.GetTabByName('Billing/Shipping').SetEnabled(true);

                            //###### Added By : Samrat Roy ##########
                            //cchkBilling.SetChecked(false);
                            //cchkShipping.SetChecked(false);
                            //page.SetActiveTabIndex(1);
                            //$('.dxeErrorCellSys').addClass('abc');
                            ////$('.crossBtn').hide();
                            //page.GetTabByName('Billing/Shipping').SetEnabled(true);
                            //page.GetTabByName('General').SetEnabled(false);

                            //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SC');
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
                            //###### END : Samrat Roy : END ########## 


                            billingLookup.focus();

                            //else {
                            //    page.SetActiveTabIndex(0);
                            //}
                            //});

                            GetObjectID('hdnAddressDtl').value = '0';
                            //$("#<%=ddl_SalesAgent.ClientID%>").focus();
                            //document.getElementById('popup_ok').focus();
                        }
                    }
                });
            }
            else {
                var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                GetObjectID('hdnCustomerId').value = key;
                if (key != null && key != '') {


                    //var startDate = new Date();
                    //startDate = cPLSalesChallanDate.GetValueString();

                    cContactPerson.PerformCallback('BindContactPerson~' + key);


                    //page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                    //jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                    //    if (r == true) {
                    page.SetActiveTabIndex(1);
                    $('.dxeErrorCellSys').addClass('abc');
                    //$('.crossBtn').hide();
                    //page.GetTabByName('[B]illing/Shipping').SetEnabled(true);

                    page.SetActiveTabIndex(1);
                    $('.dxeErrorCellSys').addClass('abc');
                    //$('.crossBtn').hide();
                    page.GetTabByName('Billing/Shipping').SetEnabled(true);
                    page.GetTabByName('General').SetEnabled(false);
                    //        billingLookup.focus();
                    //    }
                    //});
                    if (type != '' && type != null) {
                        cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                    }

                    //###### Added By : Samrat Roy ##########
                    //cchkBilling.SetChecked(false);
                    //cchkShipping.SetChecked(false);
                    //page.SetActiveTabIndex(1);
                    //$('.dxeErrorCellSys').addClass('abc');
                    ////$('.crossBtn').hide();
                    //page.GetTabByName('Billing/Shipping').SetEnabled(true);
                    //page.GetTabByName('General').SetEnabled(false);

                    //LoadCustomerAddress(key, $('#ddl_Branch').val());
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
                    //###### END : Samrat Roy : END ########## 


                    GetObjectID('hdnAddressDtl').value = '0';
                    //$("#<%=ddl_SalesAgent.ClientID%>").focus();
                    //document.getElementById('popup_ok').focus();
                }
            }
            gridSalesOrderLookup.gridView.Refresh();
            cProductsPopup.Hide();
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
          

            var CustomerDelivery = $("#<%=hddnCustomerDelivery.ClientID%>").val();
            var ddl_numbering = $('#<%=ddl_numberingScheme.ClientID%>').val();
            var BillValue = $("#<%=hddnBillId.ClientID%>").val();
            var Branch = $('#<%=ddl_Branch.ClientID %>').val();
            var WarehouseID = cCmbWarehouse.GetValue();
            $("#<%=hdnStockWarehouseId.ClientID%>").val(WarehouseID);
            //callTCControl(BillValue, 'SI');
            if (CustomerDelivery == 'Yes') {

                //LoadingPanel.Show();

                if (ddl_numbering != '' && ddl_numbering != undefined) {
                    var NoSchemeType = ddl_numbering.toString().split('~')[1];
                    var BranchId = ddl_numbering.toString().split('~')[3];

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

                    $("#<%=ddl_Branch.ClientID%>").val(BranchId);
                    $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);
                }

                //###### Added By : Samrat Roy ##########


                //LoadCustomerAddress($('#hdnCustomerId').val(), $('#ddl_Branch').val(), 'SI');


                //###### END : Samrat Roy : END ########## 

            }
            else if (CustomerDelivery == 'No') {
                var ddl_numbering = $('#<%=ddl_numberingScheme.ClientID%>').val();
                if (ddl_numbering != '' && ddl_numbering != undefined) {
                    var NoSchemeType = ddl_numbering.toString().split('~')[1];
                    var BranchIdPending = ddl_numbering.toString().split('~')[3];

                    if (NoSchemeType == '1') {
                        ctxt_SlChallanNo.SetText('Auto');
                        ctxt_SlChallanNo.SetEnabled(false);
                        cPLSalesChallanDate.Focus();

                    }
                    else if (NoSchemeType == '0') {
                        //ctxt_SlChallanNo.SetText('');
                        //ctxt_SlChallanNo.SetEnabled(true);
                        ctxt_SlChallanNo.Focus();

                    }

                    $("#<%=ddl_Branch.ClientID%>").val(BranchIdPending);
                    $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);
                    // alert('2');
                    //BSDocTagging(BillValue, 'SI');
                    if ($("#btn_TermsCondition").is(":visible")) {
                        callTCControl(BillValue, 'SI');
                    }
                }
            }

            $("#<%=hddnBranchId.ClientID%>").val($("#<%=ddl_Branch.ClientID%>").val());

            $("#<%=ddl_Branch.ClientID%>").change(function () {

                var startDate;
                var ddl_BranchId;
                if (gridSalesOrderLookup.GetValue() != null) {
                    jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            page.SetActiveTabIndex(0);
                            ccmbGstCstVat.PerformCallback();
                            ccmbGstCstVatcharge.PerformCallback();
                            //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                          
                            deleteTax("DeleteAllTax", "", "");
                            startDate = cPLSalesChallanDate.GetValueString();
                            //ddl_BranchId = $("<%=ddl_Branch.ClientID%>").val();

                            var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                            cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                            if (key != null && key != '') {
                                if (type != '' && type != null) {
                                    cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                                }
                                //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                            }
                            grid.PerformCallback('GridBlank');
                        }
                        else {
                            $("#<%=ddl_Branch.ClientID%>").val($("#<%=hddnBranchId.ClientID%>").val());
                        }
                    });
                }
                else {
                    page.SetActiveTabIndex(0);
                    ccmbGstCstVat.PerformCallback();
                    ccmbGstCstVatcharge.PerformCallback();
                    // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                 
                    deleteTax("DeleteAllTax", "", "");
                    startDate = cPLSalesChallanDate.GetValueString();


                    var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                    cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    if (key != null && key != '') {
                        if (type != '' && type != null) {
                            cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                        }
                        //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                    }
                    grid.PerformCallback('GridBlank');
                }
            });

            $('#ddl_numberingScheme').change(function () {

                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var BranchId = NoSchemeTypedtl.toString().split('~')[3];

                if (NoSchemeType == '1') {
                    ctxt_SlChallanNo.SetText('Auto');
                    ctxt_SlChallanNo.SetEnabled(false);
                    cPLSalesChallanDate.Focus();
                    //   document.getElementById('<%= txt_SlChallanNo.ClientID %>').disabled = true;

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


                $("#<%=ddl_Branch.ClientID%>").val(BranchId);
                $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);

                //gridLookup.SetText('');
            });

            $("#<%=ddl_SalesAgent.ClientID%>").change(function () {

                $("#<%=ddl_Branch.ClientID%>").focus();
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
                cddlVatGstCst.Focus();
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
        var QuantityResultant = '';
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
            //cacpAvailableStock.PerformCallback(strProductID);
            var PostingDate = new Date();
            PostingDate = cPLSalesChallanDate.GetValueString();
            var branch = $("#ddl_Branch").val();
         
            AvailableStockCheck(PostingDate, branch, strProductID);
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");
        }



        function OnEndCallback(s, e) {

            LoadingPanel.Hide();
            var value = document.getElementById('hdnRefreshType').value;

            if (grid.cpSaveSuccessOrFail == "outrange") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
                OnAddNewClick();
                jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
                OnAddNewClick();
                jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
              
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Cannot select same product in multiple rows.');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpProductZeroStock == "ZeroStock") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Insufficient Avaialble Stock.Cannot proceed');
                grid.cpProductZeroStock = null;
            }
            else if (grid.cpProductNotExists == "Select Product First") {


                if (grid.GetVisibleRowsOnPage() == 0) {
                    OnAddNewClick();


                }
                grid.batchEditApi.StartEdit(0, 1);
                grid.GetEditor('ProductID').Focus();
                jAlert('Select Product First');
                grid.cpProductNotExists = null;
            }
            else if (grid.cpIsQtyNotExists == "QtyNotExists") {

                jAlert('Enter Quantity First');
                grid.GetEditor('Quantity').Focus();
                grid.cpIsQtyNotExists = null;
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please try again later.');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "udfNotSaved") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('UDF is set as Mandatory. Please enter values.', 'Alert', function () { OpenUdf(); });
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                OnAddNewClick();
                //grid.cpSaveSuccessOrFail = null;

                //grid.AddNewRow();

                //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                //var i;
                //var cnt = 1;
                //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                //    var tbQuotation = grid.GetEditor("SrlNo");
                //    tbQuotation.SetValue(cnt);


                //    cnt++;
                //}

                var SrlNo = grid.cpProductSrlIDCheck;
                var msg = "Qty is entered for product [" + SrlNo + "] but Stock Details not updated.Cannot proceed.";
                jAlert(msg);
                grid.cpSaveSuccessOrFail = null;
                grid.cpProductSrlIDCheck = null;
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouseQty") {
                
                //OnAddNewClick();
                grid.AddNewRow();

                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var i;
                var cnt = 1;
                for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                    var tbQuotation = grid.GetEditor("SrlNo");
                    tbQuotation.SetValue(cnt);


                    cnt++;
                }
                //MoreThanStock
                //grid.batchEditApi.StartEdit(0, 1);
                grid.cpSaveSuccessOrFail = null;

                var SrlNo = grid.cpProductSrlIDCheck1;
                var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
                jAlert(msg);
                grid.cpSaveSuccessOrFail = null;
                grid.cpProductSrlIDCheck1 = null;
            }
            else if (grid.cpSaveSuccessOrFail == "MoreThanStock") {
                grid.AddNewRow();

                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var i;
                var cnt = 1;
                for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                    var tbQuotation = grid.GetEditor("SrlNo");
                    tbQuotation.SetValue(cnt);


                    cnt++;
                }
                grid.cpSaveSuccessOrFail = null;
                var msg = "Product entered quantity more than stock quantity.Can not proceed.";
                jAlert(msg);

            }
            else if (grid.cpSaveSuccessOrFail == "BillingShippingBlank") {
                grid.AddNewRow();

                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var i;
                var cnt = 1;
                for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                    var tbQuotation = grid.GetEditor("SrlNo");
                    tbQuotation.SetValue(cnt);


                    cnt++;
                }

                grid.cpSaveSuccessOrFail = null;
                var msg = "No Billing Shipping Entered.";
                jAlert(msg);
            }
            else if (grid.cpProductTotalAmountEway == "ExceedsEway") {
                //grid.batchEditApi.StartEdit(0, 2);
                grid.AddNewRow();

                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var i;
                var cnt = 1;
                for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                    var tbQuotation = grid.GetEditor("SrlNo");
                    tbQuotation.SetValue(cnt);


                    cnt++;
                }
                var msg = "Total Amount Exceeded Rs. 50000.You have to enter Eway Bill.";

                jAlert(msg);
                $('#MandatoryEwayBillNo').attr('style', 'display:block');
                grid.cpProductTotalAmountEway = null;

            }
            else {
                //var Quote_Number = grid.cpQuotationNo;
                //var Quote_Msg = "Sales Quotation No. " + Quote_Number + " generated.";
                var SalesOrder_Number = grid.cpSalesOrderNo;
                //$("#hfDocId").val(SalesOrder_Number);
                //$("#hfDocType").val("SC");
                //var DirectMsg = grid.cpSalesOrderExitOnCustomerDelivery;
                var Order_Msg = "Sales Challan No. " + SalesOrder_Number + " saved.";
                if (value == "E") {
                    //window.location.assign("SalesChallan.aspx");
                    //if (grid.cpApproverStatus == "approve") {
                    //    window.parent.popup.Hide();
                    //    window.parent.cgridPendingApproval.PerformCallback();
                    //}
                    //else if (grid.cpApproverStatus == "rejected") {
                    //    window.parent.popup.Hide();
                    //    window.parent.cgridPendingApproval.PerformCallback();
                    //}

                    if (SalesOrder_Number != "") {
                        var ODSD = $("#<%=hddnCustomerDeliverySDOrOD.ClientID%>").val();
                        if (grid.cpSalesOrderExitOnCustomerDelivery == "CustomerDelivery") {
                            //jAlert(Order_Msg);
                            grid.cpSalesOrderExitOnCustomerDelivery = null;

                            if (ODSD == "0") {

                                jAlert(Order_Msg, 'Alert Dialog: [Customer Delivery OD]', function (r) {
                                    if (r == true) {
                                        grid.cpSalesOrderNo = null;
                                        window.location.assign("CustomerDeliveryPendingList.aspx");
                                    }
                                });
                            }
                            else if (ODSD == "1") {
                                jAlert(Order_Msg, 'Alert Dialog: [Customer Delivery SD]', function (r) {
                                    if (r == true) {
                                        grid.cpSalesOrderNo = null;
                                        window.location.assign("CustomerDeliveryPendingList.aspx?type=SD");
                                    }
                                });
                            }
                            //AutoPrint
                            if ($("#<%=hddnCustomerDelivery.ClientID%>").val() == "No") {
                                if ($("#<%=hddnSaveOrExitButton.ClientID%>").val() == 'Save_Exit') {
                                    var DocumentNo = grid.cpDocumentNo;
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesChallan~D&modulename=ODSDChallan&id=" + DocumentNo, '_blank');
                                }
                            }
                            //End
                        }
                        else if (grid.cpSalesOrderExitOnPendingDelivery == "PendingDelivery") {
                            grid.cpSalesOrderExitOnPendingDelivery = null;
                            jAlert(Order_Msg, 'Alert Dialog: [Pending Delivery]', function (r) {
                                if (r == true) {
                                    grid.cpSalesOrderNo = null;
                                    window.location.assign("PendingDeliveryList.aspx?key=reten");
                                }
                            });
                        }
                        else if (ODSD == "4") {
                            jAlert(Order_Msg, 'Alert Dialog: [Second Hand Sales]', function (r) {
                                if (r == true) {
                                    grid.cpSalesOrderNo = null;
                                    window.location.assign("OldUnit_SalesInvoiceList.aspx");
                                }
                            });
                        }

                        else {
                            jAlert(Order_Msg, 'Alert Dialog: [SalesChallan]', function (r) {
                                if (r == true) {
                                    grid.cpSalesOrderNo = null;
                                    window.location.assign("SalesChallan.aspx");
                                }
                            });
                        }

                        //jAlert(Order_Msg);




                    }
                    else {
                        window.location.assign("SalesChallan.aspx");
                    }
                }
                else if (value == "N") {
                    //if (grid.cpApproverStatus == "approve") {
                    //    window.parent.popup.Hide();
                    //    window.parent.cgridPendingApproval.PerformCallback();
                    //}
                    //else if (grid.cpApproverStatus == "rejected") {
                    //    window.parent.popup.Hide();
                    //    window.parent.cgridPendingApproval.PerformCallback();
                    //}
                    // window.location.assign("SalesOrderAdd.aspx?key=ADD");


                    if (SalesOrder_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [SalesChallan]', function (r) {
                            //jAlert(Order_Msg);
                            grid.cpSalesOrderNo = null;
                            if (r == true) {
                                window.location.assign("SalesChallanAdd.aspx?key=ADD");
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
                        window.location.assign("SalesChallanAdd.aspx?key=ADD");
                    }
                }
                else {

                    var pageStatus = document.getElementById('hdnPageStatus').value;
                    if (pageStatus == "first") {
                        OnAddNewClick();
                        grid.batchEditApi.EndEdit();
                        $('#ddl_numberingScheme').focus();
                        document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "update") {
                        OnAddNewClick();
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "Quoteupdate") {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "EditModeOnDirect") {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
    }

}
    var hddnPermission = $("#<%=hddnPermissionString.ClientID%>").val();//Subhabrata
            if (gridSalesOrderLookup.GetValue() != null) {
                if (hddnPermission == "0") {
                    grid.GetEditor('ProductName').SetEnabled(false);
                    grid.GetEditor('Description').SetEnabled(false);
                    grid.GetEditor('Order_Num').SetEnabled(false);
                    grid.GetEditor('SalePrice').SetEnabled(false);//Added on 07-06-2017
                    grid.GetEditor('Discount').SetEnabled(false);//Added on 07-06-2017
                    grid.GetEditor('TaxAmount').SetEnabled(false);//Added on 07-06-2017
                }
                else {
                    grid.GetEditor('ProductName').SetEnabled(false);
                    grid.GetEditor('Description').SetEnabled(false);
                    grid.GetEditor('Order_Num').SetEnabled(false);
                    //grid.GetEditor('SalePrice').SetEnabled(false);//Added on 07-06-2017
                    grid.GetEditor('Discount').SetEnabled(false);//Added on 07-06-2017
                    //grid.GetEditor('TaxAmount').SetEnabled(false);//Added on 07-06-2017
                }

            }
            else {
                if (grid.GetVisibleRowsOnPage() == 0) {
                    OnAddNewClick();
                }
            }
            var msgOnDeliveryPendingForQtyDisabled = $("#hdnnCustomerOrPendingDelivery").val();
            if (msgOnDeliveryPendingForQtyDisabled == "CustomDeliveryPending" || msgOnDeliveryPendingForQtyDisabled == "PendingDeliveryList") {

                grid.GetEditor('Quantity').SetEnabled(false);

            }



            cProductsPopup.Hide();
            return false;
        }
        function Save_ButtonClick() {

            var flag = true;
            LoadingPanel.Show();
            $('#<%=hfControlData.ClientID %>').val($('#hfTermsConditionData').val());
            var OrderNo = ctxt_SlChallanNo.GetText();
            var slsdate = cPLSalesChallanDate.GetValue();
            var qudate = cPLQADate.GetText();
            var customerid = GetObjectID('hdnCustomerId').value;
            var salesorderDate = new Date(slsdate);
            var quotationDate = "";
            if (qudate != null && qudate != '') {
                var qd = qudate.split('-');
                LoadingPanel.Hide();
                quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

            }
            if (customerid == null || customerid == "") {
                flag = false;
                LoadingPanel.Hide();
                $('#MandatorysCustomer').attr('style', 'display:block');
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }



            if (slsdate == null || slsdate == "") {
                flag = false;
                LoadingPanel.Hide();
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
                LoadingPanel.Hide();
                $('#MandatorySlOrderNo').attr('style', 'display:block');
            }
            else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }


            if (flag) {
                if (grid.GetVisibleRowsOnPage() > 0) {
                    var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                    cacbpCrpUdf.PerformCallback();
                    $('#<%=hdnRefreshType.ClientID %>').val('N');
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('You must enter proper details before save');
                }
            }
            // return flag;
        }


        function SaveExit_ButtonClick() {
           
            var flag = true;
            LoadingPanel.Show();
            //$('#<%=hfControlData.ClientID %>').val($('#hfTermsConditionData').val());
            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
            $("#<%=hddnSaveOrExitButton.ClientID%>").val('Save_Exit');

            var OrderNo = ctxt_SlChallanNo.GetText();
            var slsdate = cPLSalesChallanDate.GetValue();
            var qudate = cPLQADate.GetText();
            var customerid = GetObjectID('hdnCustomerId').value;
            var salesorderDate = new Date(slsdate);
            var quotationDate = "";
            if (qudate != null && qudate != '') {
                //LoadingPanel.Hide();
                var qd = qudate.split('-');
                quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

            }
            if (customerid == null || customerid == "") {
                flag = false;
                LoadingPanel.Hide();
                $('#MandatorysCustomer').attr('style', 'display:block');
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }



            if (slsdate == null || slsdate == "") {
                flag = false;
                LoadingPanel.Hide();
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
                    var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                    cacbpCrpUdf.PerformCallback();
                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('You must enter proper details before save');
                }
            }
            // return flag;
        }



        function QuantityTextChange(s, e) {
         
            pageheaderContent.style.display = "block";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetText();

            //Subhabrata on 03-03-2017
            var Id = grid.GetEditor('Quotation_No').GetValue();
            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/CheckBalQuantity",
                data: JSON.stringify({ Id: Id, ProductID: ProductID.split('||@||')[0] }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {


                    var ObjData = msg.d;
                    if (ObjData.length > 0) {
                        var balQty = ObjData[0].split('|')[0];
                        if ((QuantityValue * 1) > (balQty * 1)) {
                            var OrdeMsg = 'Balance Quantity of selected Product from tagged document is <' + ObjData + '>.Cannot enter quantity more than balance quantity.';
                            //jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {

                            //});
                            //var tbQuantity = grid.GetEditor("Quantity");
                            //tbQuantity.SetValue(balQty);
                            //return false;

                        }

                    }
                }

            });

            //End

            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            //var strRate = (ctxt_Rate.GetValue() != null) ? ctxt_Rate.GetValue() : "1";
            var strRate = "1";
            var strStkUOM = SpliteDetails[4];
            //var strSalePrice = '';
            //if (gridSalesOrderLookup.GetValue() != null) {
            //    strSalePrice = grid.GetEditor('SalePrice').GetValue();
            //}
            //else {

            //    strSalePrice = SpliteDetails[6];
            //}

            var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }


            if (strRate == 0) {
                strRate = 1;
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Amount = QuantityValue * strFactor * strRate * (strSalePrice * 1);

            $('#<%= lblStkQty.ClientID %>').text(StockQuantity);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);

            //var tbStockQuantity = grid.GetEditor("StockQuantity");
            //tbStockQuantity.SetValue(StockQuantity);

            //Subhabrata added on 14-03-2017
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            //var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
            var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

            if (IsPackingActive == "Y") {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                //console.log('jhsdfafa');
                //divPacking.style.display = "block";
                $('#divPacking').css({ 'display': 'block' });
            } else {
                divPacking.style.display = "none";
            }//END

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);

            var TotaAmountRes = '';
            TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount + (TotaAmountRes * 1));
            DiscountTextChange(s, e);
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
            if (gridSalesOrderLookup.GetValue() == null) {
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
            else {
                OrderNumberChanged();
            }
        }


        var Warehouseindex;
        function OnCustomButtonClick(s, e) {
            if (e.buttonID == 'CustomDelete') {
                grid.batchEditApi.EndEdit();
                var noofvisiblerows = grid.GetVisibleRowsOnPage();
                if (gridSalesOrderLookup.GetValue() != null) {
                    jAlert('Cannot Delete using this button as the GRN is created from other document.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) {

                    });
                }
                if (noofvisiblerows != "1" && gridSalesOrderLookup.GetValue() == null) {
                    grid.DeleteRow(e.visibleIndex);
                    $('#<%=hdfIsDelete.ClientID %>').val('D');

                    var JsonProductStock = JSON.stringify(StockOfProduct);
                    GetObjectID('hdnJsonProductStock').value = JsonProductStock;


                    grid.UpdateEdit();
                    grid.PerformCallback('Display');
                    grid.batchEditApi.StartEdit(-1, 2);
                    grid.batchEditApi.StartEdit(0, 2);
                }
            }
            else if (e.buttonID == 'AddNew') {

                if (gridSalesOrderLookup.GetValue() == null) {
                    var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    if (ProductIDValue != "") {
                        OnAddNewClick();
                    }
                    else {
                        grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                    }
                }
                else {
                    OnAddNewClick();
                }
            }

            else if (e.buttonID == 'CustomWarehouse') {
                var index = e.visibleIndex;
                grid.batchEditApi.StartEdit(index, 2)
                var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                if (parseFloat(QuantityValue) == "0") {
                    jAlert("Quantity should not be zero !.");
                }
                else {
                  
                    if (ProductID != "") {
                        var LastFinYear = $('#<%=LastFinancialYear.ClientID %>').val();
                        var LastCompany = $('#<%=LastCompany.ClientID %>').val();
                        var WarehouseID = $('#ddlWarehouse').val();
                        var Date = $('#dt_PLSales').val();
                        var BatchID = $('#ddlBatch').val();
                        // $("#<%=hdnStockWarehouseId.ClientID%>").val(WarehouseID);
                        // var branchId=  $('#ddl_Branch').val();
                        var Branch = $('#<%=ddl_Branch.ClientID %>').val();
                        if (ProductID != "" && parseFloat(QuantityValue) != 0) {
                            var SpliteDetails = ProductID.split("||@||");
                            var strProductID = SpliteDetails[0];
                            var strDescription = SpliteDetails[1];
                            var strUOM = SpliteDetails[2];
                            var strStkUOM = SpliteDetails[4];
                            var strMultiplier = SpliteDetails[7];
                            var strProductName = strDescription;
                            //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                            //  var StkQuantityValue = QuantityValue * strMultiplier;
                            var Ptype = SpliteDetails[14];

                            if (Ptype == "") {
                                jAlert("Please configure inventory.");
                            }
                            else {

                                $('#<%=hdnStockProductType.ClientID %>').val(Ptype);
                                $('#<%=hdnStockProductId.ClientID %>').val(strProductID);
                                $('#<%=hdfProductSrlNo.ClientID %>').val(SrlNo);
                                $('#<%=hdfUOM.ClientID %>').val(strUOM);
                                GetObjectID('hdfIsRateExists').value = "N";
                                document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
                                document.getElementById('<%=lblEnteredAmount.ClientID %>').innerHTML = QuantityValue;
                                document.getElementById('<%=lblEnteredUOM.ClientID %>').innerHTML = strUOM;
                                BindWarehouse();
                                CreateStock();
                                cDelivaryChallanWarehouse.Show();


                                //Rev Rajdip

                             
                                DeleteProdBySl(SrlNo, strProductID, Branch)



                                //End Rev Rajdip


                            }
                            var objectToPass = {}
                            var product = $("#hdnStockProductId").val();
                            objectToPass.ProductID = $("#hdnStockProductId").val();
                            $.ajax({
                                type: "POST",
                                url: "../Activities/Services/Master.asmx/GetUom",
                                data: JSON.stringify(objectToPass),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    var returnObject = msg.d;
                                    var UOMId = returnObject.uom_id;
                                    var UOMName = returnObject.UOM_Name;
                                    if (returnObject) {
                                        SetDataSourceOnComboBoxandSetVal(ccmbPackingUom1, returnObject.uom, UOMId);
                                        //ccmbPackingUom.SetValue(returnObject.uom_id);
                                        //document.getElementById("cmbPackingUom1").disabled = true;
                                        ccmbPackingUom1.SetEnabled(false);
                                        //  document.getElementById("cmbPackingUom1").value = ProductID;
                                    }
                                }
                            });


                        }
                    }
                }
            }
        }


        function SetDataSourceOnComboBoxandSetVal(ControlObject, Source, id) {
            ControlObject.ClearItems();
            for (var count = 0; count < Source.length; count++) {
                ControlObject.AddItem(Source[count].UOM_Name, Source[count].UOM_Id);
            }
            ControlObject.SetValue(id);
            // ControlObject.SetSelectedIndex(0);
        }
        //Rev Rajdip
        function DeleteProdBySl(sl, strProductID, branch) {
          

            grid.batchEditApi.StartEdit(globalRowIndex);
            //var otherProddet = {};
            //otherProddet.sl = sl;
            //otherProddet.strProductID = strProductID;

            $.ajax({
                type: "POST",
                url: "CustomerPendingDelivery.aspx/DelProdbySl",
                data: JSON.stringify({ sl: sl, strProductID: strProductID, branch: branch }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    cpstockVal = msg.d;
                    divpopupAvailableStock.style.display = "block";

                    var AvlStk = cpstockVal + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                    //Rev Rajdip
                    document.getElementById('<%=lblavlstock.ClientID %>').innerHTML = cpstockVal;

                    //End Rev rajdip
                   <%-- document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cpstockVal;
                    document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;--%>

                    cpstockVal = null;
                    //grid.batchEditApi.StartEdit(globalRowIndex, 5);
                    return false;


                }
            });
        }
        //End Rev rajdip
function closeStockPopup(s, e) {
    e.cancel = false;
    grid.batchEditApi.StartEdit(globalRowIndex, 8);
}
function FullnFinalSave() {
    cDelivaryChallanWarehouse.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex, 8);
}

function BindWarehouse() {
    var objectToPass = {}
    objectToPass.Branch = $('#ddl_Branch').val();
    objectToPass.ProductID = $('#hdnStockProductId').val();
    objectToPass.LastFinYear = $('#hdnStockFinYear').val();
    objectToPass.LastCompany = $('#hdnStockLastCompany').val();
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetStockWarehouse",
        data: JSON.stringify(objectToPass),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var ddlWarehouse = $("[id*=ddlWarehouse]");
            $('#ddlWarehouse option').remove();

            ddlWarehouse.append($("<option></option>").val("0").html("Select"));

            $.each(r.d, function () {
                ddlWarehouse.append($("<option></option>").val(this['Value']).html(this['Text']));

            });
        }
    });
}


var JsonData;







//function SearchSerial() {
//    var searchField = $(this).val();
//    //if (searchField === '') {

//    //}

//    var regex = new RegExp(searchField, "i");
//    // var output = '<div class="row">';
//    var count = 1;
//    $.each(SerialObj, function (key, val) {
//        if ((val.Value.search(regex) != "") || (val.Text.search(regex) != "")) {
//            output += '<h5>' + val.Text + '</h5>';
//            if (count % 2 == 0) {
//                output += '</div><div class="row">'
//            }
//            count++;
//        }
//    });
//    output += '</div>';
//    $('#filter_records').html(output);

//}
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
    var ResultantQty = '';

    //Added Subhabrata on 22-06-2017
    if (cGrdWarehouse.cpWarehouseDeleticity != "WareHouseDeleticity") {
        var WarehouseBindQty = cGrdWarehouse.cpWarehouseQty;
        $("#<%=hddnWarehouseQty.ClientID%>").val(WarehouseBindQty);
    }

    var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();
    if (cGrdWarehouse.cpWarehouseDeleticity == "WareHouseDeleticity" && FifoExists == "1") {
        cGrdWarehouse.cpWarehouseDeleticity = null;
        var WarehouseID = $("#<%=hddnWarehouseId.ClientID%>").val();
        var BatchID = $("#<%=hddnBatchId.ClientID%>").val();

        var Qty = ctxtMatchQty.GetValue();

        var hddnQty = $("#<%=hddnWarehouseQty.ClientID%>").val();
        ResultantQty = (Qty * 1) - (hddnQty * 1);
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "0");

    }
    if (cGrdWarehouse.cpWarehouseSaveDisplay == "SaveDisplay" && FifoExists == "1") {
        cGrdWarehouse.cpWarehouseSaveDisplay = null;
        //ctxtMatchQty.SetText('');
        var WarehouseID = $("#<%=hddnWarehouseId.ClientID%>").val();
        var BatchID = $("#<%=hddnBatchId.ClientID%>").val();
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "0");
    }

    //End

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 7);
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Entered Quantity for the selected product must be equal to Stock Quantity.');
    }
    else {
        if (document.getElementById("myCheck").checked == true) {
            if (Ptype == "W" || Ptype == "WB") {
                cCmbWarehouse.Focus();
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
                cCmbWarehouse.Focus();
            }
            else if (Ptype == "B" || Ptype == "BS") {
                cCmbBatch.Focus();
            }
            else if (Ptype == "S") {
                checkComboBox.Focus();
            }
        }
    }
    var CustomerDelivery = $("#<%=hddnCustomerDelivery.ClientID%>").val();
    if (CustomerDelivery == "Yes" || CustomerDelivery == "No") {
        grid.batchEditApi.StartEdit(Warehouseindex, 9);
    }
    else {
        grid.batchEditApi.StartEdit(Warehouseindex, 9);
    }

    //grid.GetEditor("SalePrice").Focus();
}




function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}


function PopulateSerial() {

    //Serail Bind:Start

    //End
    
    var SessionCountSerial = '';
    var indices = [];
    var Qty = ctxtMatchQty.GetValue();
    $("#<%=hddnMatchQty.ClientID%>").val(Qty);
    var CountLength = checkListBox.GetItem.length;
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    checkListBox.SetEnabled(true);
    QuantityResultant = (QuantityResultant * 1) + (Qty * 1);
    //SessionCountSerial = '<%= Session["WarehouseBindQty"] %>'; 
    SessionCountSerial = $("#<%=hddnWarehouseQty.ClientID%>").val();
    if (SessionCountSerial != null) {
        SessionCountSerial = (SessionCountSerial * 1) + (Qty * 1);
        //SessionCountSerial = (Qty * 1);
    }
    else {
        SessionCountSerial = (Qty * 1);
    }

    if ((SessionCountSerial * 1) > QuantityValue) {
        checkListBox.UnselectAll();
        jAlert("Warehouse total Qty must be qual to entered Qty.Cannot proceed!");
        checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
    }
    else {
        checkListBox.UnselectAll();
        //Subhabrata added: on 19-06-2017
        var WarehouseID = $("#<%=hddnWarehouseId.ClientID%>").val();
        var BatchID = $("#<%=hddnBatchId.ClientID%>").val();
        var type = document.getElementById('hdfProductType').value;

        if (type == "WBS") {
            //cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + (SessionCountSerial * 1));
        }
        else if (type == "BS") {
            checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + (SessionCountSerial * 1));
        }
        else if (type == "WS") {
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + (SessionCountSerial * 1));
        }
    }


}

function SaveWarehouse() {

    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var WarehouseName = cCmbWarehouse.GetText();
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";
    var BatchName = cCmbBatch.GetText();
    var SerialID = "";
    var SerialName = "";
    var Qty = ctxtQuantity.GetValue();

    var items = checkListBox.GetSelectedItems();
    var vals = [];
    var texts = [];

    for (var i = 0; i < items.length; i++) {
        if (items[i].index != 0) {
            if (i == 0) {
                SerialID = items[i].value;
                SerialName = items[i].text;
            }
            else {
                if (SerialID == "" && SerialID == "") {
                    SerialID = items[i].value;
                    SerialName = items[i].text;
                }
                else {
                    SerialID = SerialID + '||@||' + items[i].value;
                    SerialName = SerialName + '||@||' + items[i].text;
                }
            }
            //texts.push(items[i].text);
            //vals.push(items[i].value);
        }
    }

    //WarehouseID, BatchID, SerialID, Qty=0.0
    $("#spnCmbWarehouse").hide();
    $("#spnCmbBatch").hide();
    $("#spncheckComboBox").hide();
    $("#spntxtQuantity").hide();

    var Ptype = document.getElementById('hdfProductType').value;
    if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
        $("#spnCmbWarehouse").show();
    }
    else if ((Ptype == "B" && BatchID == "0") || (Ptype == "WB" && BatchID == "0") || (Ptype == "WBS" && BatchID == "0") || (Ptype == "BS" && BatchID == "0")) {
        $("#spnCmbBatch").show();
    }
    else if ((Ptype == "W" && Qty == "0.0") || (Ptype == "B" && Qty == "0.0") || (Ptype == "WB" && Qty == "0.0")) {
        $("#spntxtQuantity").show();
    }
    else if ((Ptype == "S" && SerialID == "") || (Ptype == "WS" && SerialID == "") || (Ptype == "WBS" && SerialID == "") || (Ptype == "BS" && SerialID == "")) {
        $("#spncheckComboBox").show();
    }
    else {
        if (document.getElementById("myCheck").checked == true && SelectedWarehouseID == "0") {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "B") {
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + "");
                checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
                ctxtQuantity.SetValue("0");
            }
            else {
                IsPostBack = "N";
                PBWarehouseID = WarehouseID;
                PBBatchID = BatchID;
            }
        }
        else {
            cCmbWarehouse.PerformCallback('BindWarehouse');
            cCmbBatch.PerformCallback('BindBatch~' + "");
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
        SelectedWarehouseID = "0";
    }
}



//function txtserialTextChanged() {
//    var SerialNo = (ctxtserial.GetValue != null) ? (ctxtserial.GetValue()) : "0";
//    ctxtserial.SetValue("");
//    var texts = [SerialNo];
//    var values = GetValuesByTexts(texts);
//    checkListBox.SelectValues(values);
//    UpdateSelectAllItemState();
//    UpdateText(); // for remove non-existing texts
//    SaveWarehouse();
//} By Sudip

function txtserialTextChanged() {
   
    checkListBox.UnselectAll();
    var SerialNo = (ctxtserial.GetValue() != null) ? (ctxtserial.GetValue()) : "0";

    if (SerialNo != "0") {
        ctxtserial.SetValue("");
        var texts = [SerialNo];
        var values = GetValuesByTexts(texts);

        if (values.length > 0) {
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
            SaveWarehouse();
        }
        else {
            jAlert("This Serial Number does not exists.");
        }


        //Subhabrata added: on 19-06-2017
        var WarehouseID = $("#<%=hddnWarehouseId.ClientID%>").val();
        var BatchID = $("#<%=hddnBatchId.ClientID%>").val();
        var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();
        var MatchQty = $("#<%=hddnMatchQty.ClientID%>").val();
        var type = document.getElementById('hdfProductType').value;

        if (type == "WBS" || type == "WB") {
            //cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
            if (FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "NoFIFO");
            }
            else if (FifoExists == "1") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + (MatchQty * 1));
            }

        }
        else if (type == "BS") {

            //checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
            if (FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + "NoFIFO");
            }
            else if (FifoExists == "1") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + (MatchQty * 1));
            }
        }
        else if (type == "WS") {
            //checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
            if (FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + "NoFIFO");
            }
            else if (FifoExists == "1") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + (MatchQty * 1));
            }

        }
        UpdateSelectAllItemState();
        UpdateText(); // for remove non-existing texts
        //END
    }
}

function AutoCalculateMandateOnChange(element) {
    $("#spnCmbWarehouse").hide();
    $("#spnCmbBatch").hide();
    $("#spncheckComboBox").hide();
    $("#spntxtQuantity").hide();

    if (document.getElementById("myCheck").checked == true) {
        divSingleCombo.style.display = "block";
        divMultipleCombo.style.display = "none";
        cbtnWarehouse.SetVisible(false);
        checkComboBox.Focus();
    }
    else {
        divSingleCombo.style.display = "none";
        divMultipleCombo.style.display = "block";
        cbtnWarehouse.SetVisible(true);
        ctxtserial.Focus();
    }
}


function fn_Deletecity(keyValue) {
  
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();

    var Qty = ctxtMatchQty.GetValue();
    var hddnQty = $("#<%=hddnWarehouseQty.ClientID%>").val();
    <%--if ((hddnQty * 1) > 0)
    {
        hddnQty = (hddnQty * 1) - 1;
    }
    $("#<%=hddnWarehouseQty.ClientID%>").val(hddnQty);--%>
    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    if (FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + 'NoFIFO');
    }

}

  <%-- kaushik 20-2-2017 --%>

        $(document).ready(function () {
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
        var IsSelected = false;
        function OnListBoxSelectionChanged(listBox, args) {
            //debugger;
            var selectedItems = checkListBox.GetSelectedItems();
            //if ((args.index * 1) > ((selectedItems.length * 1))) {
            //    if (args.isSelected) {

            //        var indices = [];
            //        //checkListBox.SetCheckBoxEnabled(args.index, true);
            //        indices.push(listBox.GetItem(args.index));
            //        listBox.UnselectIndices(indices[0].text);
            //        //UpdateSelectAllItemState();
            //        UpdateText();
            //        jAlert("Canonot proceed!");
            //        return false;
            //    }

            //}
            if (args.index == 0) {
                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
                if (args.isSelected) {
                    IsSelected = true;
                }
                else {
                    IsSelected = false;
                }

            }
            var k = 0;



            //if (selectedItems.length != 0) {
            //    checkListBox.SetCheckBoxEnabled(((selectedItems.length * 1) + 1), true);
            //    //listBox.SetEnabled(((selectedItems.length * 1) + 1), false);
            //}

            //for (var i = (selectedItems.length * 1) + 1 ; i < (checkListBox.GetItemCount() * 1) ; i++) {

            //    checkListBox.SetCheckBoxEnabled(i, false);
            //}



            var qty = grid.GetEditor('Quantity').GetValue();
            var selectedItems = checkListBox.GetSelectedItems();
            var val = GetSelectedItemsText(selectedItems);

            UpdateSelectAllItemState();
            UpdateText();



            //checkboxEnabled/Disabled
            //var indices = [];
            //indices.push(listBox.GetItem(args.index));
            //listBox.SetEnabled(indices, false);

            //checkListBox.items[indices[0].text].enable = false;
            //listBox.SetEnabled(indices[0].text) = false;
            //checkListBox["SetEnabled"][indices[0].text] = false;
            //checkListBox.SetCheckBoxEnabled[
            //End

            var strWarehouse = cCmbWarehouse.GetValue();
            var strBatchID = cCmbBatch.GetValue();
            var ProducttId = $("#hdfProductID").val();

            // FIFO Checking 
            //$.ajax({
            //    type: "POST",
            //    url: "SalesChallanAdd.aspx/GetSerialId",
            //    data: JSON.stringify({
            //        "id": val,
            //        "wareHouseStr": strWarehouse,
            //        "BatchID": strBatchID,
            //        "ProducttId": ProducttId
            //    }),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    async: false,//Added By:Subhabrata
            //    success: function (msg) {
            //        
            //        var type = msg.d;
            //        if (type == "1") {

            //            return true;
            //        }
            //        else if (type == "0") {
            //            alert("Serial No can be Stock out based on FIFO process.Select the Serial No. shown from Oldest to Newest sequence to proceed");
            //            //listBox.UnselectAll();

            //            var indices = [];
            //            //Added By:Subhabrata
            //            if ((selectedItems.length * 1) == 1) {
            //                indices.push(listBox.GetItem(args.index));
            //                listBox.UnselectIndices(indices[0].text);
            //                UpdateSelectAllItemState();
            //                UpdateText();
            //            }
            //            if (((args.index) * 1) <= (selectedItems.length * 1)) {
            //                for (var i = ((args.index) * 1) ; i <= ((selectedItems.length * 1) + 1) ; i++) {
            //                    indices.push(listBox.GetItem(i));

            //                }
            //            }
            //            else {
            //                indices.push(listBox.GetItem(args.index));
            //                listBox.UnselectIndices(indices[0].text);
            //                UpdateSelectAllItemState();
            //                UpdateText();
            //            }

            //            for (var j = 0; j < indices.length   ; j++) {
            //                listBox.UnselectIndices(indices[j].text);
            //                UpdateSelectAllItemState();
            //                UpdateText();
            //            }



            //        }
            //    }
            //});

            //Subhabrata

            //checkListBox.PerformCallback('CheckSerialOnFIFO~' + strWarehouse + '~' + strBatchID + '~' + val); var val = GetSelectedItemsText(selectedItems);

            //checkListBox.PerformCallback('CheckSerialOnFIFO~' + strWarehouse + '~' + strBatchID + '~' + val);
            //End
        }
        function UpdateSelectAllItemState() {
            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
        }
        //function IsAllSelected() {
        //    debugger;
        //    var selectedDataItemCount;
        //    if (checkListBox.GetValue() != null) {
        //        selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
        //        checkListBox.GetSelectedItems().length = selectedDataItemCount;
        //    }
        //    else {
        //        checkListBox.GetSelectedItems().length = 0;
        //    }

        //    return checkListBox.GetSelectedItems().length;
        //    //var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
        //    //return checkListBox.GetSelectedItems().length == selectedDataItemCount;

        //}

        function IsAllSelected() {
            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
        }
        function UpdateText() {
            //debugger;
            var selectedItems = checkListBox.GetSelectedItems();
            selectedChkValue = GetSelectedItemsText(selectedItems);
            //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
            var ActualLength = (checkListBox.GetItemCount() * 1);
            if (IsSelected == true || ActualLength == selectedItems.length) {
                checkComboBox.SetText(((selectedItems.length * 1) - 1) + " Items");
            }
            else {
                checkComboBox.SetText((selectedItems.length) + " Items");
            }


            var val = GetSelectedItemsText(selectedItems);
            $("#abpl").attr('data-content', val);
        }

        //function UpdateText() {
        //    var selectedItems = checkListBox.GetSelectedItems();
        //    selectedChkValue = GetSelectedItemsText(selectedItems);
        //    //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
        //    checkComboBox.SetText(selectedItems.length + " Items");

        //    var val = GetSelectedItemsText(selectedItems);
        //    $("#abpl").attr('data-content', val);
        //}by Subhabrata

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


        function selectValue() {

            var startDate = new Date();
            startDate = cPLSalesChallanDate.GetValueString();
            var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";



            if (key != null && key != '' && type != "") {
                cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);
            }



            var componentType = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
            if (componentType != null && componentType != '') {
                grid.PerformCallback('GridBlank');
            }
        }

        function OrderNumberChanged() {

            //gridquotationLookup.GetValue()
            //grid.PerformCallback('BindGridOnQuotation' + '~' + cddl_Quotatione.GetValue() + '~' + ctxt_SlOrderNo.GetValue());
            //var quote_Id = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

            var quote_Id = gridSalesOrderLookup.GetValue();

            if (quote_Id != null) {
                var arr = quote_Id.split(',');

                if (arr.length > 1) {
                    cPLQADate.SetText('Multiple Select Order Dates');

                }
                else {
                    if (arr.length == 1) {
                        cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);


                    }
                    else {
                        cPLQADate.SetText('');

                    }
                }
                //cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
                //cProductsPopup.Show();

            }
            else { cPLQADate.SetText(''); }

            if (quote_Id != null) {
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

                if (type != '' && type != null) {
                    cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@' + '~' + type);
                    cProductsPopup.Show();
                }
            }
            else {
                cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
                cProductsPopup.Show();
            }

            txt_OANumber.Focus();
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

            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

            if (type != '' && type != null) {
                grid.PerformCallback('BindGridOnQuotation' + '~' + '@' + '~' + type);
            }
            var OrderIds = gridSalesOrderLookup.GetValue();
            var Key = OrderIds.split(',')[0];
            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/GetContactSalesManReference",
                data: "{'KeyVal':'" + Key + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    var currentString = msg.d;
                    //var ContactPersonId = currentString.split('~')[0];
                    var Reference = currentString.split('~')[0];
                    var SalesManId = currentString.split('~')[1];
                    var CurrencyId = currentString.split('~')[2];
                    txt_Refference.SetText(Reference)
                    $("#<%=ddl_SalesAgent.ClientID%>").val(SalesManId);
                    $("#<%=ddl_Currency.ClientID%>").val(CurrencyId);

                }
            });

            cSalesOrderComponentPanel.PerformCallback('BindOrderLookupOnSelection');
            cProductsPopup.Hide();
            $('#<%=hdnPageStatus.ClientID %>').val('Quoteupdate');

            //#### added by Samrat Roy for Transporter Control #############
            var quote_Id = gridSalesOrderLookup.gridView.GetSelectedKeysOnPage();
            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                callTransporterControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
            }
            if (quote_Id.length > 0) {
                //alert('1');
                //BSDocTagging(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
            }
            //#### End : Samrat Roy for Transporter Control : End #############

            //#### added by Sayan Dutta for TC Control #############
            if ($("#btn_TermsCondition").is(":visible")) {
                callTCControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
            }
            //#### End : added by Sayan Dutta for TC Control : End #############

            return false;
        }


        function componentEndCallBack(s, e) {
            //gridSalesOrderLookup.gridView.Refresh();
            if (grid.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick();
            }
        }
        //Code for UDF Control 
        <%--   function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                // var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '';
                var keyval = $('#<%=hdnmodeId.ClientID %>').val();
                //  alert(keyval);
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SC&&KeyVal_InternalID=' + keyval;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }--%>
        // End Udf Code
    </script>
    <%--End Sudip--%>




    <%--Debu Section--%>
    <script type="text/javascript">


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
                document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cacpAvailableStock.cpstock;
                document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                cCmbWarehouse.cpstock = null;

                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return;
            }



        }
        var SelectWarehouse = "0";
        var SelectBatch = "0";
        var SelectSerial = "0";
        var SelectedWarehouseID = "0";
        function CallbackPanelEndCall(s, e) {

            if (cCallbackPanel.cpEdit != null) {
                var strWarehouse = cCallbackPanel.cpEdit.split('~')[0];
                var strBatchID = cCallbackPanel.cpEdit.split('~')[1];
                var strSrlID = cCallbackPanel.cpEdit.split('~')[2];
                var strQuantity = cCallbackPanel.cpEdit.split('~')[3];

                SelectWarehouse = strWarehouse;
                SelectBatch = strBatchID;
                SelectSerial = strSrlID;

                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
                checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

                cCmbWarehouse.SetValue(strWarehouse);
                ctxtQuantity.SetValue(strQuantity);
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
    <%--Warehouse Section End--%>

    <script>
        document.onkeydown = function (e) {
            if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                if (page.GetActiveTabIndex() == 1) {
                    fnSaveBillingShipping();
                }
                // document.getElementById('Button3').click();

                // return false;
            }

            else if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                page.SetActiveTabIndex(0);
                gridLookup.Focus();
                // document.getElementById('Button3').click();

                // return false;
            }
            else if (event.keyCode == 77 && event.altKey == true) {
                $('#TermsConditionseModal').modal({
                    show: 'true'
                });
            }
            else if (event.keyCode == 69 && event.altKey == true) {
                if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
                    StopDefaultAction(e);
                    SaveTermsConditionData();
                }
            }
            else if (event.keyCode == 76 && event.altKey == true) {
                StopDefaultAction(e);
                calcelbuttonclick();
            }
            else if (event.keyCode == 83 && event.altKey == true) {
                if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                    SaveVehicleControlData();
                }
            }
            else if (event.keyCode == 67 && event.altKey == true) {
                modalShowHide(0);
            }
            else if (event.keyCode == 82 && event.altKey == true) {
                modalShowHide(1);
                $('body').on('shown.bs.modal', '#exampleModal', function () {
                    $('input:visible:enabled:first', this).focus();
                });
            }
            else {
                //do nothing
            }
        }

    </script>


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



    <%--  <script>
        function SaveStock() {
            var Obj = {};
            var StockType = GetObjectID('hdnStockProductType').value;
            // var ProductSrlNo = GetObjectID('hdfProductSrlNo').value;
            //  var ProductID = GetObjectID('hdfProductID').value;
            //  var UOM = GetObjectID('hdfUOM').value;
            //  var ServiceURL = GetObjectID('hdfServiceURL').value;
            //  var Branch = GetObjectID('hdfBranch').value;

            var WarehouseID = $('#ddlWarehouse').val();
            var WarehouseName = $("option:selected", '#ddlWarehouse').text();
            var BatchId = $("#ddlBatch").val().trim();
            var BatchName = $("option:selected", '#ddlBatch').text();
            var SerialId = $('#lstSerial').val();
            var SerialName = $("option:selected", '#lstSerial').text();
            var Quantity = $('#txtQuty').val();
            // SelectedObj , SerialObj

            var LoopId = 1;

            Obj.WarehouseID = WarehouseID;
            Obj.WarehouseName = WarehouseName;
            Obj.BatchId = BatchId;
            Obj.BatchName = BatchName;
            Obj.SerialId = SerialId;
            Obj.SerialName = SerialName;
            Obj.Quantity = Quantity;

            SelectedObj.push(Obj);
            if ((StockType == "W") && ($("#ddlBatch").val() == "0") && ($('#lstSerial').val() == null)) {

                var matchData = $.grep(SelectedObj, function (element, index) { return element.WarehouseID == $('#ddlWarehouse').val(); });
                if (matchData.length > 1) {
                    if (SelectedObj.length > 1) {

                        matchData[0].Quantity = parseInt(matchData[0].Quantity) + parseInt($('#txtQuty').val());






                    }
                }

                else {

                }
            }
            ShowWarehouse();
        }

        function ShowWarehouse() {
            var SelectedObj = SerialObj;
            if (SelectedObj.length > 0) {
                var txt = '';

                txt += "<table width='100%' class='dvxtbl'><thead><tr>";
                txt += "<th style='width:10%'>WareHouse Id</th>";
                txt += "<th style='width:20%'>WareHouse Name</th>";
                txt += "<th style='width:10%'>Batch Id</th>";
                txt += "<th style='width:20%'>Batch Name</th>";
                txt += "<th style='width:10%'>Serial Id</th>";
                txt += "<th style='width:20%'>Serial Name</th>";
                txt += "<th style='width:10%'>Quantity</th>";
                txt += "</tr></thead><tbody>";

                for (x in SelectedObj) {
                    txt += "<tr>";
                    PropertyCount = 0;

                    for (key in SelectedObj[0]) {
                        if (PropertyCount == "0") txt += " <td style='width:10%'>" + SelectedObj[x][key] + "</td>";
                        else if (PropertyCount == "2") txt += " <td style='width:10%'>" + SelectedObj[x][key] + "</td>";
                        else txt += " <td style='width:10%'>" + SelectedObj[x][key] + "</td>";

                        PropertyCount++;
                    }
                    txt += "</tr>";
                }
                txt += "</tbody></table>";
                document.getElementById("showData").innerHTML = txt;

            }
        }
    </script>--%>
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
                    <div class="lblHolder" id="divDues" style="display: none;">
                        <table>
                            <tr>
                                <td>Receivable(Dues)</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTotalDues" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </li>
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

        <%-- <div id="crossBtnId" class="crossBtn" runat="server"><a href="SalesChallan.aspx"><i class="fa fa-times"></i></a></div>--%>
        <div id="crossBtnCustDeliveryListId" class="crossBtn" runat="server"><a href="CustomerDeliveryPendingList.aspx"><i class="fa fa-times"></i></a></div>
        <%-- <div id="crossBtnCustDeliveryListForSD" class="crossBtn" runat="server"><a href="CustomerDeliveryPendingList.aspx?type=SD"><i class="fa fa-times"></i></a></div>--%>
        <div id="crossBtnPendingDeliveryListId" class="crossBtn" runat="server"><a href="PendingDeliveryList.aspx?key=reten"><i class="fa fa-times"></i></a></div>
        <%-- <div id="crossBtnPendingSecondHand" class="crossBtn" runat="server"><a href="OldUnit_SalesInvoiceList.aspx"><i class="fa fa-times"></i></a></div>--%>
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <asp:Panel ID="pnl_quotation" runat="server">
            <div class="row">

                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="98%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General" TabStyle-CssClass="generalTab">
                            <TabStyle CssClass="generalTab">
                            </TabStyle>
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="row">
                                        <div id="ddlInventoryId" runat="server" class="col-md-2">
                                            <label>
                                                <%--Inventory Item--%>
                                                <asp:Label ID="Label12" runat="server" CssClass="newLbl" Text="Inventory Item?"></asp:Label>
                                            </label>
                                            <div class="Left_Content">
                                                <asp:DropDownList ID="ddlInventory" runat="server" CssClass="backSelect" onchange="ddlInventory_OnChange()" Width="100%">
                                                    <asp:ListItem Value="Y">Inventory Items</asp:ListItem>
                                                    <%--<asp:ListItem Value="N">Non-Inventory Items</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <%--<dxe:ASPxCallbackPanel runat="server" ID="IsInventotry" ClientInstanceName="cIsInventory" OnCallback="ComponentIsInventory_Callback">
                                                        <PanelCollection>
                                                            <dxe:PanelContent runat="server">
                                                <dxe:ASPxComboBox ID="cmbIsInventory" ClientInstanceName="ccmbIsInventory" runat="server" TabIndex="14"  
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                     <Items>
                                                        <dxe:ListEditItem Text="Yes" Value="1" />
                                                        <dxe:ListEditItem Text="No" Value="0" />
                                                    </Items>
                               
                                                    <ClientSideEvents SelectedIndexChanged="isInventoryChanged" />
                                                </dxe:ASPxComboBox>
                                             </dxe:PanelContent>
                                                        </PanelCollection>

                                                    </dxe:ASPxCallbackPanel>--%>
                                            </div>
                                        </div>
                                        <div id="ddl_numberingDiv" runat="server" class="col-md-3">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <asp:DropDownList ID="ddl_numberingScheme" runat="server" TabIndex="1" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_SlOrderNo" runat="server" Text="Document No" Width="">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                            </label>
                                            <dxe:ASPxTextBox ID="txt_SlChallanNo" runat="server" ClientInstanceName="ctxt_SlChallanNo" MaxLength="30" TabIndex="2" Width="100%">
                                                <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                            </dxe:ASPxTextBox>
                                            <span id="MandatorySlOrderNo" class="validclass" style="display: none">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></img></span>
                                        </div>
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="inline" Text="Posting Date" Width="120px">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                            </label>
                                            <dxe:ASPxDateEdit ID="dt_PLSales" runat="server" ClientInstanceName="cPLSalesChallanDate" EditFormat="Custom" EditFormatString="dd-MM-yyyy" TabIndex="3" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <ClientSideEvents DateChanged="function(s, e) {DateCheck();}" />
                                                <ClientSideEvents GotFocus="function(s,e){cPLSalesChallanDate.ShowDropDown();}" />
                                            </dxe:ASPxDateEdit>
                                            <span id="MandatorySlDate" class="validclass" style="display: none">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor211_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></img></span> <span id="MandatoryEgSDate" class="validclass" style="display: none">
                                                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2114_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Sales Order date must not be prior date than quotation date"></img></span>
                                        </div>
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxCallbackPanel ID="BranchCallBackPanel" runat="server" ClientInstanceName="cBranchComponentPanel" OnCallback="ComponentBranch_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <asp:DropDownList ID="ddl_Branch" runat="server" TabIndex="4" Width="100%">
                                                        </asp:DropDownList>
                                                    </dxe:PanelContent>
                                                </PanelCollection>
                                                <%--<ClientSideEvents EndCallback="componentEndCallBack" />--%>
                                            </dxe:ASPxCallbackPanel>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <div class="col-md-2">
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxTextBox ID="txt_Refference" runat="server" MaxLength="50" TabIndex="5" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                            </label>
                                            <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridLookup" KeyFieldName="cnt_internalid" TabIndex="6" TextFormatString="{1}" Width="100%">
                                                <Columns>
                                                    <dxe:GridViewDataColumn Caption="Short Name" FieldName="shortname" Settings-AutoFilterCondition="Contains" Visible="true" VisibleIndex="0" Width="150">
                                                        <Settings AutoFilterCondition="Contains" />
                                                    </dxe:GridViewDataColumn>
                                                    <dxe:GridViewDataColumn Caption="Name" FieldName="Name" Settings-AutoFilterCondition="Contains" Visible="true" VisibleIndex="1" Width="150">
                                                        <Settings AutoFilterCondition="Contains" />
                                                    </dxe:GridViewDataColumn>
                                                    <dxe:GridViewDataColumn FieldName="cnt_internalid" Settings-AllowAutoFilter="False" Visible="false" VisibleIndex="2" Width="150">
                                                        <Settings AllowAutoFilter="False" />
                                                    </dxe:GridViewDataColumn>
                                                </Columns>
                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                    <Templates>
                                                        <StatusBar>
                                                            <table class="OptionsTable" style="float: right">
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" ClientSideEvents-Click="CloseGridLookup" Text="Close" UseSubmitBehavior="False" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </StatusBar>
                                                    </Templates>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" />
                                                    <%-- <Settings ShowFilterRow="True" ShowStatusBar="Visible" />--%>
                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                </GridViewProperties>
                                                <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" />
                                                <ClientSideEvents GotFocus="function(s,e){gridLookup.ShowDropDown();}" />
                                                <ClearButton DisplayMode="Auto">
                                                </ClearButton>
                                            </dxe:ASPxGridLookup>
                                            <span id="MandatorysCustomer" class="validclass" style="display: none">
                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></img></span>
                                        </div>
                                        <div class="col-md-3">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" CssClass="inline" Text="Contact Person">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" ClientInstanceName="cContactPerson" Font-Size="12px" OnCallback="cmbContactPerson_Callback" TabIndex="7" Width="100%">
                                                <ClientSideEvents EndCallback="cmbContactPersonEndCall" GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />
                                            </dxe:ASPxComboBox>
                                            <%--<asp:DropDownList ID="ddl_ContactPerson" runat="server" TabIndex="6" Width="100%"></asp:DropDownList>--%>
                                        </div>
                                        <div class="col-md-2">
                                            <%-- <label>
                                            <dxe:ASPxLabel ID="lbl_quotation_No" runat="server" Text="Sale Order No" Width="120px">
                                            </dxe:ASPxLabel>
                                        </label>--%>
                                            <asp:RadioButtonList ID="rdl_SaleInvoice" runat="server" onchange="return selectValue();" RepeatDirection="Horizontal" TabIndex="8" Width="120px">
                                                <asp:ListItem Text="Order" Value="SO"></asp:ListItem>
                                                <asp:ListItem Text="Invoice" Value="SI"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <dxe:ASPxCallbackPanel ID="ComponentQuotationPanel" runat="server" ClientInstanceName="cSalesOrderComponentPanel" OnCallback="ComponentSalesOrder_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxGridLookup ID="lookup_order" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridSalesOrderLookup" KeyFieldName="Order_Id" MultiTextSeparator=", " OnDataBinding="lookup_order_DataBinding" SelectionMode="Multiple" TabIndex="9" TextFormatString="{0}" Width="100%">
                                                            <Columns>
                                                                <dxe:GridViewCommandColumn Caption=" " ShowSelectCheckbox="True" VisibleIndex="0" Width="60" />
                                                                <dxe:GridViewDataColumn Caption="Doc Number" FieldName="Order_Number" Settings-AutoFilterCondition="Contains" Visible="true" VisibleIndex="1" Width="150">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn Caption="Doc Date" FieldName="Order_Date" Settings-AutoFilterCondition="Contains" Visible="true" VisibleIndex="2" Width="150">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn Caption="Customer" FieldName="name" Settings-AutoFilterCondition="Contains" Visible="true" VisibleIndex="3" Width="150">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn Caption="Unit" FieldName="Branch" Settings-AutoFilterCondition="Contains" Visible="true" VisibleIndex="4" Width="150">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn Caption="Reference" FieldName="Reference" Settings-AutoFilterCondition="Contains" Visible="true" VisibleIndex="5" Width="150">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                            </Columns>
                                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                <Templates>
                                                                    <StatusBar>
                                                                        <table class="OptionsTable" style="float: right">
                                                                            <tr>
                                                                                <td>
                                                                                    <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" ClientSideEvents-Click="CloseGridQuotationLookup" Text="Close" UseSubmitBehavior="False" />
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
                                            <%--  <asp:DropDownList ID="ddl_Quotation_No" runat="server" Width="100%" TabIndex="1" >
                    </asp:DropDownList>--%>
                                        </div>
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_SaleOrder_Date" runat="server" Text="Document Date" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <asp:Label ID="lbl_MultipleDate" runat="server" Style="display: none" Text="Multiple Select Sale Order Dates"></asp:Label>
                                            <dxe:ASPxCallbackPanel ID="ComponentDatePanel" runat="server" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxTextBox ID="dt_Quotation" runat="server" ClientEnabled="false" ClientInstanceName="cPLQADate" TabIndex="10" Width="100%">
                                                        </dxe:ASPxTextBox>
                                                        <dxe:ASPxDateEdit ID="dt_PLQuotation" runat="server" ClientInstanceName="cPLOADate" EditFormat="Custom" Enabled="false" TabIndex="13" Visible="false" Width="100%">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Expiry date can not be shorter than Pl/Quote date." ErrorTextPosition="Right">
                                                                <RequiredField IsRequired="true" />
                                                            </ValidationSettings>
                                                            <ClientSideEvents DateChanged="function(s,e){SetDifference1();}" Validation="function(s,e){e.isValid = (CheckDifference()&gt;=0)}" />
                                                        </dxe:ASPxDateEdit>
                                                    </dxe:PanelContent>
                                                </PanelCollection>
                                            </dxe:ASPxCallbackPanel>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <asp:DropDownList ID="ddl_SalesAgent" runat="server" TabIndex="11" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-1 lblmTop8">
                                            <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Credit Days">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxTextBox ID="txtCreditDays" runat="server" ClientInstanceName="ctxtCreditDays" TabIndex="12" Width="100%">
                                                <MaskSettings AllowMouseWheel="false" Mask="&lt;0..999999999&gt;" />
                                                <ClientSideEvents TextChanged="CreditDays_TextChanged" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <dxe:ASPxLabel ID="lbl_DueDate" runat="server" Text="Due Date">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxDateEdit ID="dt_SaleInvoiceDue" runat="server" ClientInstanceName="cdt_SaleInvoiceDue" EditFormat="Custom" EditFormatString="dd-MM-yyyy" TabIndex="13" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <ClientSideEvents GotFocus="function(s,e){cdt_SaleInvoiceDue.ShowDropDown();}" />
                                            </dxe:ASPxDateEdit>
                                        </div>
                                        <div class="col-md-3" style="display: none;">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_OANumber" runat="server" Text="OA Number" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxTextBox ID="txt_OANumber" runat="server" MaxLength="50" TabIndex="21" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                        <div class="col-md-3" style="display: none;">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_OADate" runat="server" Text="OA Date" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxDateEdit ID="dt_OADate" runat="server" ClientInstanceName="cPLOADate" EditFormat="Custom" EditFormatString="dd-MM-yyyy" TabIndex="11" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <%-- <validationsettings causesvalidation="True" errordisplaymode="ImageWithTooltip" errortextposition="Right" errortext="Expiry date can not be shorter than Pl/Quote date.">
                            <RequiredField IsRequired="true" />
                        </validationsettings>--%><%-- <clientsideevents datechanged="function(s,e){SetDifference1();}"
                            validation="function(s,e){e.isValid = (CheckDifference()>=0)}" />--%>
                                            </dxe:ASPxDateEdit>
                                        </div>
                                        <div class="col-md-3" style="display: none;">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="inline" Text="Expiry" Width="61px">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxDateEdit ID="dt_PlOrderExpiry" runat="server" ClientInstanceName="cExpiryDate" EditFormat="Custom" EditFormatString="dd-MM-yyyy" Style="display: none;" TabIndex="4444" Width="100%">
                                                <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Expiry date can not be shorter than Pl/Quote date." ErrorTextPosition="Right">
                                                    <RequiredField IsRequired="true" />
                                                </ValidationSettings>
                                                <ClientSideEvents DateChanged="function(s,e){SetDifference();}" Validation="function(s,e){e.isValid = (CheckDifference()&gt;=0)}" />
                                            </dxe:ASPxDateEdit>
                                        </div>
                                        <div class="col-md-1">
                                            <label style="margin: 3px 0; display: block">
                                                Currency:
                                            </label>
                                            <div>
                                                <asp:DropDownList ID="ddl_Currency" runat="server" DataSourceID="SqlCurrency" DataTextField="Currency_AlphaCode" DataValueField="Currency_ID" onchange="ddl_Currency_Rate_Change()" TabIndex="14" Width="100%">
                                                </asp:DropDownList>
                                                <%-- <dxe:ASPxComboBox ID="CmbCurrency" EnableIncrementalFiltering="True" ClientInstanceName="cCmbCurrency"
                            DataSourceID="SqlCurrencyBind"
                            TextField="Currency_AlphaCode" ValueField="Currency_ID" SelectedIndex="0"
                            runat="server" ValueType="System.String" EnableSynchronization="True" Width="100%" CssClass="pull-left">
                            <clientsideevents valuechanged="function(s,e){Currency_Rate()}"></clientsideevents>
                        </dxe:ASPxComboBox>--%>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <label style="margin: 3px 0; display: block">
                                                Exch. Rate:
                                            </label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txt_Rate" runat="server" ClientInstanceName="ctxtRate" TabIndex="15" Width="100%">
                                                </dxe:ASPxTextBox>
                                                <%-- <dxe:ASPxTextBox runat="server" ID="txt_Rate" ClientInstanceName="ctxtRate" Width="100%" CssClass="pull-left">
                            <masksettings mask="<0..9999>.<0..99999>" includeliterals="DecimalSymbol" />
                        </dxe:ASPxTextBox>--%>
                                            </div>
                                        </div>
                                        <div class="col-md-2 hide">
                                            <label style="margin: 3px 0; display: block">
                                                E-Way Bill No.
                                            </label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txt_EWayBillNO" runat="server" ClientInstanceName="ctxtEWayBillNO" TabIndex="16" Width="100%">
                                                </dxe:ASPxTextBox>
                                                <span id="MandatoryEwayBillNo" class="validclass" style="display: none">
                                                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></img></span>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>
                                            </span><%--<asp:DropDownList ID="ddl_AmountAre" runat="server" TabIndex="12" Width="100%">
            </asp:DropDownList>--%>
                                            <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientInstanceName="cddl_AmountAre" TabIndex="17" Width="100%">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                            </dxe:ASPxComboBox>
                                        </div>

                                        <%--   //Chinmoy added below code 03-04-2020 start--%>
                                        <div class="col-md-2 lblmTop8" id="divposGst">
                                            <dxe:ASPxLabel ID="lbl_PosForGst" runat="server" Text="Place Of Supply [GST]">
                                            </dxe:ASPxLabel>
                                            <span style="color: red">*</span>
                                            <dxe:ASPxComboBox ID="ddl_PosGst" runat="server" ValueType="System.String" Width="100%" ClientInstanceName="cddl_PosGst" TabIndex="18">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulatePosGst(e)}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <%--  //End--%>
                                        <div class="clear">
                                        </div>
                                        <div class="col-md-2 hide">
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" OnCallback="ddl_VatGstCst_Callback" TabIndex="18" Width="100%">
                                                <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <div style="clear: both;">
                                        </div>
                                        <div class="col-md-12">
                                            <div style="display: none;">
                                                <a class="btn btn-primary" href="javascript:void(0);" onclick="OnAddNewClick()"><span>Add New</span> </a>
                                            </div>
                                            <div>
                                                <br />
                                            </div>
                                            <dxe:ASPxGridView ID="grid" runat="server" ClientInstanceName="grid" KeyFieldName="OrderID" OnBatchUpdate="grid_BatchUpdate" OnCellEditorInitialize="grid_CellEditorInitialize" OnCustomCallback="grid_CustomCallback" OnCustomUnboundColumnData="grid_CustomUnboundColumnData" OnDataBinding="grid_DataBinding" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared" OnHtmlRowCreated="grid_HtmlRowCreated" OnRowDeleting="Grid_RowDeleting" OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" Settings-ShowFooter="false" Settings-VerticalScrollableHeight="200" Settings-VerticalScrollBarMode="auto" SettingsBehavior-AllowDragDrop="false" SettingsBehavior-AllowSort="false" SettingsPager-Mode="ShowAllRecords" Width="100%">
                                                <SettingsPager Visible="false">
                                                </SettingsPager>
                                                <Settings VerticalScrollBarMode="Auto" />
                                                <SettingsBehavior AllowDragDrop="False" AllowSort="False" />
                                                <Columns>
                                                    <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" VisibleIndex="0" Width="30">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="CustomDelete" Image-Url="/assests/images/crs.png" Text=" ">
                                                                <Image Url="/assests/images/crs.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" ForeColor="White" Text=" ">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Sl" FieldName="SrlNo" ReadOnly="true" VisibleIndex="1" Width="20">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- <dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="ProductID" VisibleIndex="3" Width="150">
                                                    <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName">
                                                        <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" />
                                                    </PropertiesComboBox>
                                                </dxe:GridViewDataComboBoxColumn>--%><%--Batch Product Popup Start--%>
                                                    <dxe:GridViewDataButtonEditColumn Caption="Product" FieldName="ProductName" VisibleIndex="3" Width="150">
                                                        <PropertiesButtonEdit>
                                                            <%--<ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />--%>
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn Caption="hidden Field Id" EditCellStyle-CssClass="hide" FieldName="ProductID" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="hide" ReadOnly="True" VisibleIndex="16" Width="0">
                                                        <PropertiesTextEdit Height="15px">
                                                            <FocusedStyle CssClass="hide">
                                                            </FocusedStyle>
                                                            <Style CssClass="hide">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <EditCellStyle CssClass="hide">
                                                        </EditCellStyle>
                                                        <CellStyle CssClass="hide" Wrap="True">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Batch Product Popup End--%>
                                                    <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description" ReadOnly="True" VisibleIndex="4" Width="250">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="5" Width="70">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                        </PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings AllowMouseWheel="false" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <%--<ClientSideEvents LostFocus="QuantityTextChange" />--%>
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Doc No." FieldName="Order_Num" ReadOnly="True" VisibleIndex="2" Width="80">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="UOM(Sale)" FieldName="UOM" ReadOnly="true" VisibleIndex="5" Width="80">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Caption="Warehouse"--%>
                                                    <dxe:GridViewCommandColumn Caption="Stk Details" VisibleIndex="6" Width="80">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="CustomWarehouse" Image-ToolTip="Warehouse" Image-Url="/assests/images/warehouse.png" Text=" ">
                                                                <Image ToolTip="Warehouse" Url="/assests/images/warehouse.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Sale Price" FieldName="SalePrice" VisibleIndex="7" Width="60">
                                                        <PropertiesTextEdit DisplayFormatString="0.00">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        </PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <%--<ClientSideEvents LostFocus="SalePriceTextChange" />--%>
                                                            <MaskSettings AllowMouseWheel="false" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataSpinEditColumn Caption="Disc(%)" FieldName="Discount" VisibleIndex="8" Width="50">
                                                        <PropertiesSpinEdit AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" MaxValue="100" MinValue="0">
                                                            <SpinButtons ShowIncrementButtons="false">
                                                            </SpinButtons>
                                                            <%--<ClientSideEvents LostFocus="DiscountTextChange" />--%>
                                                        </PropertiesSpinEdit>
                                                    </dxe:GridViewDataSpinEditColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount" ReadOnly="true" VisibleIndex="9" Width="70">
                                                        <PropertiesTextEdit DisplayFormatString="0.00">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        </PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings AllowMouseWheel="false" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--   <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="10" Width="75">
                                                    <PropertiesButtonEdit>
                                                        <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" />
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>--%>
                                                    <dxe:GridViewDataButtonEditColumn Caption="Charges" FieldName="TaxAmount" HeaderStyle-HorizontalAlign="Right" VisibleIndex="10" Width="75">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                            <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                        </PropertiesButtonEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="TotalAmount" ReadOnly="true" VisibleIndex="11" Width="80">
                                                        <PropertiesTextEdit DisplayFormatString="0.00">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        </PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings AllowMouseWheel="false" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                            <%--<ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" VisibleIndex="12" Width="40">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="AddNew" Image-Url="/assests/images/add.png" Text=" ">
                                                                <Image Url="/assests/images/add.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Stock Qty" FieldName="StockQuantity" VisibleIndex="13" Width="0">
                                                        <PropertiesTextEdit DisplayFormatString="0.00">
                                                        </PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Stock UOM" FieldName="StockUOM" ReadOnly="true" VisibleIndex="14" Width="0">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Quotation No" CellStyle-CssClass="hide" EditCellStyle-CssClass="hide" EditFormCaptionStyle-CssClass="hide" FieldName="Quotation_No" FilterCellStyle-CssClass="hide" FooterCellStyle-CssClass="hide" HeaderStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" VisibleIndex="15" Width="0">
                                                        <PropertiesTextEdit NullTextStyle-CssClass="hide" ReadOnlyStyle-CssClass="hide" Style-CssClass="hide">
                                                            <NullTextStyle CssClass="hide">
                                                            </NullTextStyle>
                                                            <ReadOnlyStyle CssClass="hide">
                                                            </ReadOnlyStyle>
                                                            <Style CssClass="hide">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <EditCellStyle CssClass="hide">
                                                        </EditCellStyle>
                                                        <FilterCellStyle CssClass="hide">
                                                        </FilterCellStyle>
                                                        <EditFormCaptionStyle CssClass="hide">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="hide" />
                                                        <CellStyle CssClass="hide">
                                                        </CellStyle>
                                                        <FooterCellStyle CssClass="hide">
                                                        </FooterCellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DetailsId" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <%--      Init="OnInit"BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing"
                        CustomButtonClick="OnCustomButtonClick" EndCallback="OnEndCallback" --%>
                                                <ClientSideEvents BatchEditStartEditing="gridFocusedRowChanged" CustomButtonClick="OnCustomButtonClick" EndCallback="OnEndCallback" RowClick="GetVisibleIndex" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings EditMode="row" ShowConfirmOnLosingChanges="false" />
                                                </SettingsEditing>
                                            </dxe:ASPxGridView>
                                            <%--<HeaderTemplate>
                                <img src="../../../assests/images/Add.png" />
                            </HeaderTemplate>--%>
                                            <%--<dxe:ASPxGridView runat="server" KeyFieldName="OrderID"
                        OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" ID="grid"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                        OnBatchUpdate="grid_BatchUpdate"
                        OnHtmlRowCreated="grid_HtmlRowCreated"
                        OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                        OnCustomCallback="grid_CustomCallback"
                        OnDataBinding="grid_DataBinding"
                        OnCellEditorInitialize="grid_CellEditorInitialize"
                        OnRowInserting="Grid_RowInserting"
                        OnRowUpdating="Grid_RowUpdating"
                        OnRowDeleting="Grid_RowDeleting" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200"
                        >
                        <settingspager visible="false"></settingspager>
                        <settingsbehavior allowdragdrop="False" allowsort="False" />
                        <columns>
                            <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" VisibleIndex="0" Width="1%">
                                <custombuttons>
                                    <dxe:GridViewCommandColumnCustomButton ID="CustomDelete" Image-Url="/assests/images/crs.png" Text=" ">
                                        <image url="/assests/images/crs.png">
                                        </image>
                                    </dxe:GridViewCommandColumnCustomButton>
                                </custombuttons>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn Caption="Sl" FieldName="SrlNo" ReadOnly="true" VisibleIndex="1" Width="2%">
                                <propertiestextedit>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="ProductID" VisibleIndex="1" Width="10%">
                                <propertiescombobox clientinstancename="ProductID" textfield="ProductName" valuefield="ProductID">
                                    
                                    <clientsideevents selectedindexchanged="ProductsCombo_SelectedIndexChanged" />

                                </propertiescombobox>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description" ReadOnly="True" VisibleIndex="3">
                                <propertiestextedit>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" VisibleIndex="4" Width="6%">
                                <propertiestextedit>
                                    <masksettings allowmousewheel="false" mask="&lt;0..999999999999&gt;.&lt;0..99&gt;" />
                                    <clientsideevents lostfocus="QuantityTextChange" />
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="UOM(Sale)" FieldName="UOM" ReadOnly="true" VisibleIndex="5" Width="8%">
                                <propertiestextedit>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewCommandColumn Caption="Warehouse" VisibleIndex="6" Width="1%">
                                <custombuttons>
                                    <dxe:GridViewCommandColumnCustomButton ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Text=" ">
                                        <image url="/assests/images/warehouse.png">
                                        </image>
                                    </dxe:GridViewCommandColumnCustomButton>
                                </custombuttons>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn Caption="Stock Qty" FieldName="StockQuantity" VisibleIndex="7" Width="6%">
                                <propertiestextedit>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Stock UOM" FieldName="StockUOM" ReadOnly="true" VisibleIndex="8" Width="8%">
                                <propertiestextedit>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Sale Price" FieldName="SalePrice" ReadOnly="true" VisibleIndex="9" Width="6%">
                                <propertiestextedit>
                                <masksettings allowmousewheel="false" mask="&lt;0..999999999999&gt;.&lt;0..99&gt;" />
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Discount" FieldName="Discount" VisibleIndex="10" Width="6%">
                                <propertiestextedit>
                                <masksettings allowmousewheel="false" mask="&lt;0..999999999999&gt;.&lt;0..99&gt;" />
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount" VisibleIndex="11" Width="6%">
                                <propertiestextedit>
                                <masksettings allowmousewheel="false" mask="&lt;0..999999999999&gt;.&lt;0..99&gt;" />
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                           
                            <dxe:GridViewDataButtonEditColumn Caption="TaxAmount" FieldName="TaxAmount" VisibleIndex="12" Width="6%">
                                <propertiesbuttonedit>
                                <clientsideevents buttonclick="taxAmtButnClick" gotfocus="taxAmtButnClick1" />
                                <buttons>
                                <dxe:EditButton Text="..." Width="20px">
                                        </dxe:EditButton>
                                </buttons>
                                </propertiesbuttonedit>
                            </dxe:GridViewDataButtonEditColumn>
                            <dxe:GridViewDataTextColumn Caption="Total Amount" FieldName="TotalAmount" VisibleIndex="13" Width="6%">
                                <propertiestextedit>
                                <masksettings allowmousewheel="false" mask="&lt;0..999999999999&gt;.&lt;0..99&gt;" />
                                <clientsideevents keydown="AddBatchNew"></clientsideevents>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Quotation No" CellStyle-CssClass="hide" FieldName="Quotation_No" HeaderStyle-CssClass="hide" VisibleIndex="14">
                                <propertiestextedit nulltextstyle-cssclass="hide" readonlystyle-cssclass="hide" style-cssclass="hide">
                                
                                <nulltextstyle cssclass="hide"></nulltextstyle>

                                <readonlystyle cssclass="hide"></readonlystyle>

                                    <style cssclass="hide"></style>

                                    </propertiestextedit>
                                <HeaderStyle CssClass="hide" />
                                <cellstyle cssclass="hide">
                                </cellstyle>
                            </dxe:GridViewDataTextColumn>
                        
                        </columns>
                     
                        <clientsideevents endcallback="OnEndCallback" custombuttonclick="OnCustomButtonClick" rowclick="GetVisibleIndex" />
                        <settingsdatasecurity allowedit="true" />
                        <settingsediting mode="Batch" newitemrowposition="Bottom">
                            <BatchEditSettings ShowConfirmOnLosingChanges="false"  EditMode="row" />
                        </settingsediting>
                    </dxe:ASPxGridView>--%>
                                        </div>
                                        <div style="clear: both;">
                                        </div>
                                        <br />
                                        <div class="col-md-12">
                                            <asp:Label ID="ClientShowMsg" runat="server" CssClass="msgStyle" Text="Already Delivered." Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red" Text=""></asp:Label>
                                            <dxe:ASPxButton ID="btn_SaveRecords" runat="server" AutoPostBack="False" ClientInstanceName="cbtn_SaveRecords" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" Text="Save &amp; N̲ew" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <%--  <asp:Button ID="ASPxButton2" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />--%>
                                            <dxe:ASPxButton ID="ASPxButton12" runat="server" AutoPostBack="False" ClientInstanceName="cbtn_SaveRecords" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" Text="Save &amp; Ex̲it" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" Text="UDF" UseSubmitBehavior="false" />
                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False" ClientInstanceName="cbtn_SaveRecords" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" Text="T̲ax &amp; Charges" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>
                                            <%-- onclick=""--%><a id="btnAddNew" runat="server" class="btn btn-primary" href="javascript:void(0);" style="display: none"><span><u>A</u>ttachment(s)</span></a> <%--<dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[A]ttachment(s)" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>--%><%--<a class="btn btn-primary" href="javascript:void(0);" onclick="fn_PopOpen()"><span><u>B</u>illing/Shipping</span> </a>--%>
                                            <uc1:VehicleDetailsControl ID="VehicleDetailsControl" runat="server" />
                                            <asp:HiddenField ID="hfControlData" runat="server" />
                                            <%--<uc1:ucVehicleDriverControl runat="server" ID="ucVehicleDriverControl" />--%>
                                            <uc2:TermsConditionsControl ID="TermsConditionsControl" runat="server" />
                                            <asp:HiddenField ID="hfTermsConditionData" runat="server" />
                                            <asp:HiddenField ID="hfTermsConditionDocType" runat="server" Value="SC" />
                                        </div>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                            <%--test generel--%>
                        </dxe:TabPage>
                        <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping" TabStyle-CssClass="bilingTab">
                            <TabStyle CssClass="bilingTab">
                            </TabStyle>
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <%--        <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server" ContentUrl="AddArea_PopUp.aspx"
                                        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                                        Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                        <ContentCollection>
                                            <dxe:PopupControlContentControl runat="server">
                                            </dxe:PopupControlContentControl>
                                        </ContentCollection>
                                        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                    </dxe:ASPxPopupControl>--%>
                                    <ucBS:Sales_BillingShipping ID="Sales_BillingShipping" runat="server" />
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


            <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
            </dxe:ASPxCallbackPanel>



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
                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Quotation_No" ReadOnly="true" Caption="Quotation Id" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="OrderDetails_Id" ReadOnly="true" Caption="Quotation_U" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quotation_Num" Width="90" ReadOnly="true" Caption="Order No">
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
                            <%--<ClientSideEvents EndCallback=" cgridProducts_EndCallBack " />--%>
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
                <asp:HiddenField ID="hddnTextTaggedDoc" runat="server" />
                <asp:HiddenField ID="hddnBranchId" runat="server" />
                <asp:HiddenField ID="LastCompany" runat="server" />
                <asp:HiddenField ID="hdnStockLastCompany" runat="server" />
                <asp:HiddenField ID="LastFinancialYear" runat="server" />
                <asp:HiddenField ID="hdnStockFinYear" runat="server" />
                <asp:HiddenField ID="hdnnCustomerOrPendingDelivery" runat="server" />
                <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />
                <asp:HiddenField ID="hddnCustomerDelivery" runat="server" />
                <asp:HiddenField ID="hddnCustomerDeliverySDOrOD" runat="server" />
                <asp:HiddenField ID="hddnWarehouseId" runat="server" />
                <asp:HiddenField ID="hdnStockWarehouseId" runat="server" />
                <asp:HiddenField ID="hddnBatchId" runat="server" />
                <asp:HiddenField ID="hddnWarehouseQty" runat="server" />
                <asp:HiddenField ID="hddnMatchQty" runat="server" />
                <asp:HiddenField ID="hddnConfigVariable_Val" runat="server" />
                <asp:HiddenField ID="hddnBillId" runat="server" />
                <asp:HiddenField ID="hddnPermissionString" runat="server" />
                <asp:HiddenField ID="hddnTypeString" runat="server" />
                <asp:HiddenField ID="hddnIsODSDFirstTime" runat="server" />
                <asp:HiddenField runat="server" ID="hdfProductSrlNo" />
                <asp:HiddenField runat="server" ID="hdfUOM" />
                <%--Debjyoti GST on 30-06-2017--%>
                <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
                <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
                <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
                <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
                <asp:HiddenField runat="server" ID="hddnSaveOrExitButton" />
                <%--END--%>
                <asp:HiddenField runat="server" ID="hdnJsonProductStock" />

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
                                        <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
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
                                <div id="content-5" class="reverse wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
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
                                                                <asp:Label ID="lblProductName1" runat="server"></asp:Label></td>
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
                                            <div style="margin-bottom: 5px;">
                                                Warehouse
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                                    TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                                    <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange()}" EndCallback="CmbWarehouseEndCallback"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                                <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; top: 24px; right: -2px; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_Batch">
                                            <div style="margin-bottom: 5px;">
                                                Batch/Lot
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxComboBox ID="CmbBatch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBatch"
                                                    TextField="BatchName" ValueField="BatchID" runat="server" Width="100%" OnCallback="CmbBatch_Callback">
                                                    <ClientSideEvents ValueChanged="function(s,e){CmbBatch_ValueChange()}" EndCallback="CmbBatchEndCall"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                                <span id="spnCmbBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; top: 24px; right: -2px; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-2" id="div_QtyMatch">
                                            <div style="margin-bottom: 5px;">
                                                Match Quantity
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxTextBox ID="txtMatchQty" runat="server" ClientInstanceName="ctxtMatchQty" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                                    <ClientSideEvents TextChanged="function(s, e) {PopulateSerial();}" />
                                                </dxe:ASPxTextBox>
                                                <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_Serial">
                                            <div style="margin-bottom: 5px;">
                                                Serial No &nbsp;&nbsp; (
                                                <input type="checkbox" id="myCheck" name="BarCode" onchange="AutoCalculateMandateOnChange(this)">Barcode )
                                            </div>
                                            <div class="" id="divMultipleCombo">

                                                <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="89%" CssClass="pull-left" runat="server" AnimationType="None">
                                                    <DropDownWindowStyle BackColor="#EDEDED" />
                                                    <DropDownWindowTemplate>
                                                        <dxe:ASPxListBox Width="100%" Height="150" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" OnCallback="CmbSerial_Callback"
                                                            runat="server">
                                                            <Border BorderStyle="None" />
                                                            <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                            <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" EndCallback="listBoxEndCall" />
                                                        </dxe:ASPxListBox>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="padding: 4px">
                                                                    <dxe:ASPxButton ID="ASPxButton6" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                                        <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DropDownWindowTemplate>
                                                    <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" GotFocus="function(s, e){ s.ShowDropDown(); }" />
                                                </dxe:ASPxDropDownEdit>
                                                <span id="spncheckComboBox" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <div class="pull-right">
                                                    <i class="fa fa-commenting" id="abpl" aria-hidden="true" style="font-size: 16px; cursor: pointer; margin: 3px 0 0 5px;" title="Serial No " data-container="body" data-toggle="popover" data-placement="right" data-content=""></i>
                                                </div>
                                            </div>
                                            <div class="" id="divSingleCombo" style="display: none;">
                                                <dxe:ASPxTextBox ID="txtserial" runat="server" Width="85%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                                    <ClientSideEvents TextChanged="txtserialTextChanged" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_Quantity">
                                            <div style="margin-bottom: 5px;">
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
                                        <div class="col-md-3">
                                            <div>
                                            </div>
                                            <div class="Left_Content" style="padding-top: 14px;">
                                                <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                                    <ClientSideEvents Click="function(s, e) {if(!document.getElementById('myCheck').checked) SaveWarehouse();}" />
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
                                            <dxe:GridViewDataTextColumn Caption="Available Quantity" FieldName="AvailableQty" Visible="false"
                                                VisibleIndex="1">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                                VisibleIndex="2">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Conversion Foctor" FieldName="ConversionMultiplier" Visible="false"
                                                VisibleIndex="3">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Stock Quantity" FieldName="StkQuantity" Visible="false"
                                                VisibleIndex="4">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Balance Stock" FieldName="BalancrStk" Visible="false"
                                                VisibleIndex="5">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Batch/Lot Number" FieldName="BatchNo"
                                                VisibleIndex="6">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="MfgDate"
                                                VisibleIndex="7">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ExpiryDate"
                                                VisibleIndex="8">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                                VisibleIndex="9">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px">
                                                <DataItemTemplate>
                                                    &nbsp; <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete">
                                                        <img src="../../../assests/images/Edit.png" /></a>
                                                    &nbsp;
                                                        <a href="javascript:void(0);" id="ADelete" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete">
                                                            <img src="/assests/images/crs.png" /></a>
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


                <dxe:ASPxPopupControl ID="DelivaryChallanWarehouse" runat="server" ClientInstanceName="cDelivaryChallanWarehouse"
                    Height="500px" Width="850px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ClientSideEvents Closing="function(s, e) {closeStockPopup(s, e);}" />
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div id="content-1" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
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
                                                            <asp:Label ID="lblProductName" runat="server"></asp:Label>
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
                                                        <td>Entered Quantity </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblEnteredAmount" runat="server"></asp:Label>
                                                            <asp:Label ID="lblEnteredUOM" runat="server"></asp:Label>
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
                                                        <td>Available Stock </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblavlstock" runat="server"></asp:Label>
                                                          <%--  <asp:Label ID="Label2" runat="server"></asp:Label>--%>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div class="clear"></div>
                            <div id="StockHeader">
                                <div class="clearfix  modal-body" style="padding: 8px 0 8px 0; margin-bottom: 15px; margin-top: 15px; border-radius: 4px; border: 1px solid #ccc;">
                                    <div class="col-md-12">
                                        <div class="clearfix  row">
                                            <div class="col-md-3" id="_div_Warehouse">
                                                <div>
                                                    Warehouse
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <asp:DropDownList ID="ddlWarehouse" runat="server" Width="100%" onchange="WarehouseChange()">
                                                    </asp:DropDownList>

                                                    <span id="DelChWarehouse" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>

                                            <div class="col-md-3" id="_div_Batch">
                                                <div>
                                                    Batch/Lot
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <asp:DropDownList ID="ddlBatch" runat="server" Width="100%" onchange="BindSerial()">
                                                    </asp:DropDownList>
                                                    <span id="DelChBatch" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="_div_Serial">
                                                <div>
                                                    Serial No &nbsp;&nbsp; (
                                                <input type="checkbox" id="myCheckSerial" name="BarCode" onchange="ShowSerialTextOrNot(this)" />Barcode )
                                                </div>
                                                <div class="Left_Content" style="" id="divSerialList">
                                                    <asp:ListBox ID="lstSerial" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                    <span id="DelChSerial" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                                <div id="_div_text" style="display: none;">
                                                    <input type="text" id="txtSerialForStock" onkeyup="Serialkeydown(event)" />
                                                </div>
                                            </div>

                                            <div class="col-md-3" id="_div_Quantity">
                                                <div>
                                                    Quantity
                                                </div>
                                                <dxe:ASPxTextBox ID="txtQuty" runat="server" ClientInstanceName="ctxtQuty" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                    <%-- <ClientSideEvents TextChanged="function(s, e) {SubmitWarehouse();}" />--%>
                                                    <ClientSideEvents TextChanged="function(s,e) { ChangePackingByQuantityinjs();}" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                </dxe:ASPxTextBox>
                                                <%--<div class="Left_Content" style="">
                                                    <asp:TextBox ID="txtQuty" runat="server" Width="100%" MaxLength="50" Text="0">                             
                                                    </asp:TextBox>
                                                    <span id="spnQuantity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>--%>
                                            </div>
                                 <div class="col-md-3" id="div_AltQuantity" runat="server">
                                      <%--  <div style="margin-bottom: 5px;">--%>
                                            Alt. Qty
                                     <%--   </div>--%>
                                        <div class="Left_Content" style="">
                                   
                                            <dxe:ASPxTextBox ID="txtPacking" runat="server" ClientInstanceName="CtxtPacking" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                 <ValidationSettings Display="None"></ValidationSettings>
                                                <ClientSideEvents TextChanged="function(s,e) { ChangeQuantityByPacking1();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>

                               <div class="col-md-3" id="_div_Uom" runat="server">
                                        <%--<div style="margin-bottom: 5px;">--%>
                                            Alt. UOM
                                       <%-- </div>--%>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="cmbPackingUom1" ClientInstanceName="ccmbPackingUom1" runat="server" SelectedIndex="0"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                <%--EnableIncrementalFiltering="False"--%>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>

                                            <div class="col-md-3">
                                                <div>
                                                </div>
                                                <div class="Left_Content" style="padding-top: 14px">
                                                    <input type="button" onclick="SaveStock()" value="Add" class="btn btn-primary" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="showData" class="gridStatic">
                            </div>
                            <div class="clearfix  row">
                                <div class="col-md-3">
                                    <div>
                                    </div>
                                    <div class="Left_Content" style="padding-top: 14px">
                                        <input type="button" onclick="FullnFinalSave()" value="Ok" class="btn btn-primary" />
                                    </div>
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>



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
                <asp:HiddenField ID="hdnStockProductId" runat="server" />
                <asp:HiddenField ID="hdfProductSerialID" runat="server" />
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdfProductType" runat="server" />
                <asp:HiddenField ID="hdnStockProductType" runat="server" />
                <asp:HiddenField ID="hdnProductQuantity" runat="server" />
                <asp:HiddenField ID="hdntab2" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnCustomerId" runat="server" />
                <asp:HiddenField ID="hdnAddressDtl" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hdnSchemaLength" runat="server" />
                <asp:HiddenField runat="server" ID="hdfIsRateExists" />
                <%--kaushik 24-2-2017 --%>
                <asp:HiddenField runat="server" ID="IsUdfpresent" />
                <%--kaushik 24-2-2017--%>
            </div>

        </asp:Panel>

        <asp:SqlDataSource ID="SqlCurrency" runat="server"
            SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>
        <%-- <asp:SqlDataSource ID="SqlCurrencyBind" runat="server" ></asp:SqlDataSource>--%>
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
                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
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
                                <asp:Button ID="Button4" runat="server" Text="Ok" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
                                <asp:Button ID="Button5" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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

    <asp:HiddenField ID="hdnmodeId" runat="server" />

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>
       <asp:HiddenField runat="server" ID="hdnpackingqty" />  
     <asp:HiddenField runat="server" ID="hdnuomFactor" /> 
    <asp:HiddenField runat="server" ID="hdnisOverideConvertion" /> 

    <asp:HiddenField ID="hdnShowUOMConversionInEntry" runat="server" />
</asp:Content>
