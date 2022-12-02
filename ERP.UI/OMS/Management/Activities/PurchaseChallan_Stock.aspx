<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PurchaseChallan_Stock.aspx.cs" Inherits="ERP.OMS.Management.Activities.PurchaseChallan_Stock" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>

    <style type="text/css">
        #grid_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }

        #gridTax_DXStatus {
            display: none !important;
        }

        Warehouse Details
        /*#grid_DXMainTable > tbody > tr > td:last-child {
              display: none !important;
          }*/
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        .voucherno {
            position: absolute;
            right: -3px;
            top: 22px;
        }

        .customerno {
            position: absolute;
            right: -3px;
            top: 22px;
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
            right: 92px;
            top: 18px;
            position: absolute;
        }

        #txtCreditLimit_EC {
            position: absolute;
        }

        #grid_DXStatus span > a {
            display: none;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .absolute, #grid_DXMainTable .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .inline {
            display: inline-block !important;
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

        #grid_DXMainTable > tbody > tr > td:last-child, #productLookUp_DDD_gv_DXMainTable > tbody > tr > td:nth-child(2) {
            display: none !important;
        }

        #aspxGridTax_DXStatus {
            display: none !important;
        }

        .mTop {
            margin-top: 10px;
        }

        .mbot5 .col-md-8 {
            margin-bottom: 5px;
        }

        .pullleftClass {
            position: absolute;
            right: -3px;
            top: 18px;
        }

        a.anchorclass, a.anchorclass:hover {
            color: red;
        }

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
    </style>


    <script type="text/javascript">
        var _GetAmountValue = "0";
        var _GetSalesPriceValue = "0";
        var _GetQuantityValue = "0";
        var _GetDiscountValue = "0";

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

            ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
        }


        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;

            //var ProductIDColumn = s.GetColumnByField("ProductID");
            //if (!e.rowValues.hasOwnProperty(ProductIDColumn.index))
            //    return;
            //var cellInfo = e.rowValues[ProductIDColumn.index];

            //if (cCmbProduct.FindItemByValue(cellInfo.value) != null) {
            //    cCmbProduct.SetValue(cellInfo.value);
            //}
            //else {
            //    cCmbProduct.SetSelectedIndex(-1);
            //}
        }

        function Onddl_VatGstCstEndCallback(s, e) {
            if (s.GetItemCount() == 1) {
                cddlVatGstCst.SetEnabled(false);
            }
        }

        $(document).ready(function () {
            if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
            }
            $('#ApprovalCross').click(function () {

                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.Refresh()();
            })
        })

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
            ctxtTaxTotAmt.SetValue(totAmt + calculatedValue - GlobalCurTaxAmt);

            //tax others
            SetOtherTaxValueOnRespectiveRow(0, calculatedValue, ccmbGstCstVat.GetText());
            gstcstvatGlobalName = ccmbGstCstVat.GetText();
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

            RecalCulateTaxTotalAmountCharges();
        }
        function ctaxUpdatePanelEndCall(s, e) {
            if (ctaxUpdatePanel.cpstock != null) {
                //divAvailableStk.style.display = "block";
                //divpopupAvailableStock.style.display = "block";

                var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;

                ctaxUpdatePanel.cpstock = null;
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
                return false;
            }
        }
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

        //.............Available Stock Div Show............................
        function ProductsGotFocus(s, e) {
            pageheaderContent.style.display = "block";
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
            var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                //divPacking.style.display = "block";
                divPacking.style.display = "none";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID);
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
        function QuantityProductsGotFocus(s, e) {
            pageheaderContent.style.display = "block";
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
            var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                //divPacking.style.display = "block";
                divPacking.style.display = "none";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            var editids = getUrlVars()["key"];
            // alert(strProductID);
            if (ProductID != "0" && editids != "ADD") {
                cacpAvailableStock.PerformCallback(strProductID);
            }

            Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
        }

        function acpAvailableStockEndCall(s, e) {
            if (cacpAvailableStock.cpstock != null) {
                divpopupAvailableStock.style.display = "block";

                //divAvailableStk.style.display = "block";
                //divpopupAvailableStock.style.display = "block";

                var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = cacpAvailableStock.cpstock;

                document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cacpAvailableStock.cpstock;
                document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;

                cCmbWarehouse.cpstock = null;
            }
        }
        //................Available Stock Div Show....................
        //...................Shortcut keys.................
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
            // if (event.keyCode == 18) isCtrl = true;
            if (event.keyCode == 78 && event.altKey == true && getUrlVars().req != "V") { //run code for alt+N -- ie, Save & New  
                StopDefaultAction(e);
                //Save_ButtonClick();
                document.getElementById('btn_SaveRecords').click();
            }
            else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Ctrl+X -- ie, Save & Exit!     
                StopDefaultAction(e);
                // SaveExit_ButtonClick();
                document.getElementById('btn_SaveRecordsExit').click();
            }
            else if (event.keyCode == 116) {

                //run code for Ctrl+X -- ie, Save & Exit! 
                cPopup_WarehousePC.Hide();
                return false;
            }
            else if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+O -- ie, Billing/Shipping Samrat!     
                StopDefaultAction(e);
                if (page.GetActiveTabIndex() == 1) {
                    fnSaveBillingShipping();
                }
            }
            else if (event.keyCode == 77 && event.altKey == true) { //run code for Ctrl+m -- ie, TC Sayan!
                $('#TermsConditionseModal').modal({
                    show: 'true'
                });
            }
            else if (event.keyCode == 69 && event.altKey == true) { //run code for Ctrl+e -- ie, TC Sayan!
                if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
                    StopDefaultAction(e);
                    SaveTermsConditionData();
                }
            }
            else if (event.keyCode == 76 && event.altKey == true) { //run code for Ctrl+l -- ie, TC Sayan!
                StopDefaultAction(e);
                calcelbuttonclick();
            }
            else if (event.keyCode == 83 && event.altKey == true) { //run code for Ctrl+l -- ie, TC Sayan!
                StopDefaultAction(e);
                if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                    SaveVehicleControlData();
                }
            }
            else if (event.keyCode == 82 && event.altKey == true) { //run code for Ctrl+l -- ie, TC Sayan!
                StopDefaultAction(e);
                modalShowHide(1);
                $('body').on('shown.bs.modal', '#exampleModal', function () {
                    $('input:visible:enabled:first', this).focus();
                })
            }
            else if (event.keyCode == 67 && event.altKey == true) { //run code for Ctrl+l -- ie, TC Sayan!
                StopDefaultAction(e);
                modalShowHide(0);
            }
            else {
                //do nothing
            }
        }
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }

        //...................end............................
        //...............PopulateVAT........................
        function PopulateGSTCSTVAT(e) {
            var key = cddl_AmountAre.GetValue();
            //deleteAllRows();

            if (key == 1) {

                grid.GetEditor('gvColTaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                //cddlVatGstCst.PerformCallback('1');
                cddlVatGstCst.SetSelectedIndex(-1);
                cbtn_SaveRecords.SetVisible(true);
                grid.GetEditor('gvColProduct').Focus();
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }

            }
            else if (key == 2) {
                grid.GetEditor('gvColTaxAmount').SetEnabled(true);

                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');
                cddlVatGstCst.Focus();
                cbtn_SaveRecords.SetVisible(true);
            }
            else if (key == 3) {

                grid.GetEditor('gvColTaxAmount').SetEnabled(false);

                //cddlVatGstCst.PerformCallback('3');
                cddlVatGstCst.SetSelectedIndex(-1);
                cddlVatGstCst.SetEnabled(false);
                cbtn_SaveRecords.SetVisible(false);
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
                cddlVatGstCst.Focus();
            }

        }

        //.................End PopulateVAT...............
        //................Amount Calculation.........................
        function TaxAmountKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }
        function taxAmtButnClick1(s, e) {
            console.log(grid.GetFocusedRowIndex());
            rowEditCtrl = s;
        }
        function taxAmtButnClickold(s, e) {
            if (e.buttonIndex == 0) {

                if (cddl_AmountAre.GetValue() != null) {
                    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";

                    if (ProductID.trim() != "") {

                        document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();
                        caspxTaxpopUp.Show();
                        //Set Product Gross Amount and Net Amount

                        var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                        var SpliteDetails = ProductID.split("||@||");
                        var strMultiplier = SpliteDetails[7];
                        var strFactor = SpliteDetails[8];
                        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                        //var strRate = "1";
                        var strStkUOM = SpliteDetails[4];
                        // var strSalePrice = SpliteDetails[6];
                        var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "";
                        if (strRate == 0) {
                            strRate = 1;
                        }
                        console.log("1");
                        var StockQuantity = strMultiplier * QuantityValue;
                        var Amount = Math.round(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                        clblTaxProdGrossAmt.SetText(Amount);
                        clblProdNetAmt.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue()).toFixed(2));
                        document.getElementById('HdProdGrossAmt').value = Amount;
                        document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('gvColAmount').GetValue()).toFixed(2);

                        //End Here

                        //Set Discount Here
                        if (parseFloat(grid.GetEditor('gvColDiscount').GetValue()) > 0) {
                            var discount = Math.round((Amount * grid.GetEditor('gvColDiscount').GetValue() / 100)).toFixed(2);
                            clblTaxDiscount.SetText(discount);
                        }
                        else {
                            clblTaxDiscount.SetText('0.00');
                        }
                        //End Here 


                        //Checking is gstcstvat will be hidden or not
                        if (cddl_AmountAre.GetValue() == "2") {
                            $('.GstCstvatClass').hide();
                            clblTaxableGross.SetText("(Taxable)");
                            clblTaxableNet.SetText("(Taxable)");

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
                                        clblProdNetAmt.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2));
                                        document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2);
                                        clblGstForNet.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
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

                            //###### Added By : Samrat Roy ##########
                            //Get Customer Shipping StateCode
                            var shippingStCode = '';
                            shippingStCode = cbsSCmbState.GetText();
                            shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                            //// ###########  Old Code #####################
                            ////Get Customer Shipping StateCode
                            //var shippingStCode = '';
                            //if (cchkBilling.GetValue()) {
                            //    shippingStCode = CmbState.GetText();
                            //}
                            //else {
                            //    shippingStCode = CmbState1.GetText();
                            //}
                            //shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                            //###### END : Samrat Roy : END ########## 

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
                            cgridTax.cpComboCode = grid.GetEditor('gvColProduct').GetValue().split('||@||')[9];
                        }

                        ctxtprodBasicAmt.SetValue(grid.GetEditor('gvColAmount').GetValue());
                    } else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 14);
                    }
                }
            }
        }

        function SalePriceTextFocus(s, e) {
            var Saleprice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
            _GetSalesPriceValue = Saleprice;

            Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
        }

        function DiscountTextFocus() {
            Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
        }

        function SalePriceTextChange(s, e) {
            pageheaderContent.style.display = "block";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var Saleprice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
            var ProductID = grid.GetEditor('gvColProduct').GetValue();

            //if ((parseFloat(Saleprice) != parseFloat(_GetSalesPriceValue))) {
            if (ProductID != null) {
                if (parseFloat(Saleprice) == "0") {
                    jConfirm('Are you sure to make this Amount as Zero(0) as the charges will also become Zero(0)?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            WorkOn_SalesPrice(s, e);
                            grid.batchEditApi.EndEdit();
                            grid.batchEditApi.StartEdit(globalRowIndex, 11);
                        }
                        else {
                            grid.batchEditApi.StartEdit(globalRowIndex, 10);

                            var gvColStockPurchasePrice = grid.GetEditor("gvColStockPurchasePrice");
                            gvColStockPurchasePrice.SetValue(_GetSalesPriceValue);
                            // grid.StartEditRow(globalRowIndex,2)
                            WorkOn_SalesPrice(s, e);


                            setTimeout(function () {
                                grid.batchEditApi.EndEdit();
                                grid.batchEditApi.StartEdit(globalRowIndex, 11);
                            }, 500);



                        }
                    });
                }
                else {
                    WorkOn_SalesPrice(s, e);
                }
            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('SalePrice').SetValue('0');
                grid.GetEditor('ProductID').Focus();
            }
            //}
        }

        function WorkOn_SalesPrice(s, e) {
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var Saleprice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
            var ProductID = grid.GetEditor('gvColProduct').GetValue();

            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            //var strRate = "1";
            var strStkUOM = SpliteDetails[4];
            //var strSalePrice = SpliteDetails[6];

            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            if (strRate == 0) {
                strRate = 1;
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Discount = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";

            var Amount = QuantityValue * strFactor * (Saleprice / strRate);
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("gvColAmount");
            tbAmount.SetValue(amountAfterDiscount);

            var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
            tbTotalAmount.SetValue(amountAfterDiscount);

            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                //divPacking.style.display = "block";
                divPacking.style.display = "none";
            } else {
                divPacking.style.display = "none";
            }

            DiscountTextChange(s, e);

            //cacpAvailableStock.PerformCallback(strProductID);
        }

        function QuantityTextChange(s, e) {
            //
            pageheaderContent.style.display = "block";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var ProductID = grid.GetEditor('gvColProduct').GetValue();
            var key = gridquotationLookup.GetValue();// gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

            if (parseFloat(QuantityValue) != parseFloat(Pre_Quantity)) {
                if (ProductID != null) {
                    var SpliteDetails = ProductID.split("||@||");
                    //console.log(SpliteDetails)
                    var strMultiplier = SpliteDetails[7];//Conversion_Multiplier
                    // console.log("Multiplier" + strMultiplier);
                    //  var strFactor = SpliteDetails[14]; //Packing_Factor
                    var strFactor = SpliteDetails[8]; //Packing_Factor
                    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                    //console.log("Rate" + strRate);
                    var strProductID = SpliteDetails[0];
                    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
                    var ddlbranch = $("[id*=ddl_Branch]");
                    var strBranch = ddlbranch.find("option:selected").text();

                    //var strRate = "1";
                    var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
                    var strSalePrice = SpliteDetails[6];// purchase Price
                    //console.log("PurchasePrice" + strSalePrice);


                    if (key != null && key != '') {
                        var TotalQty = (grid.GetEditor('TotalQty').GetText() != null) ? grid.GetEditor('TotalQty').GetText() : "0";
                        var BalanceQty = (grid.GetEditor('BalanceQty').GetText() != null) ? grid.GetEditor('BalanceQty').GetText() : "0";
                        var CurrQty = 0;

                        BalanceQty = parseFloat(BalanceQty);
                        TotalQty = parseFloat(TotalQty);
                        QuantityValue = parseFloat(QuantityValue);

                        if (TotalQty > QuantityValue) {
                            CurrQty = BalanceQty + (TotalQty - QuantityValue);
                        }
                        else {
                            CurrQty = BalanceQty - (QuantityValue - TotalQty);
                        }

                        if (CurrQty < 0) {
                            grid.GetEditor("TotalQty").SetValue(TotalQty);
                            grid.GetEditor("gvColQuantity").SetValue(TotalQty);
                            var OrdeMsg = 'Balance Quantity of selected Product from tagged document. <br/>Cannot enter quantity more than balance quantity.';
                            jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {
                                grid.batchEditApi.StartEdit(globalRowIndex, 6);
                            });
                            return false;
                        }
                        else {
                            grid.GetEditor("TotalQty").SetValue(QuantityValue);
                            grid.GetEditor("BalanceQty").SetValue(CurrQty);
                        }
                    }
                    else {
                        grid.GetEditor("TotalQty").SetValue(QuantityValue);
                        grid.GetEditor("BalanceQty").SetValue(QuantityValue);
                    }




                    if (strRate == 0) {
                        strRate = 1;
                    }
                    if (strSalePrice == 0.00000) {
                        strSalePrice = 1;
                    }

                    var StockQuantity = strMultiplier * QuantityValue;
                    var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

                    // alert("here" + Amount);
                    $('#<%= lblbranchName.ClientID %>').text(strBranch);

                    var IsPackingActive = SpliteDetails[13];//IsPackingActive
                    var Packing_Factor = SpliteDetails[14];//Packing_Factor
                    var Packing_UOM = SpliteDetails[15];//Packing_UOM
                    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

                    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                        $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                        //divPacking.style.display = "block";
                        divPacking.style.display = "none";
                    } else {
                        divPacking.style.display = "none";
                    }



                    var tbAmount = grid.GetEditor("gvColAmount");
                    tbAmount.SetValue(Amount);

                    var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                    tbTotalAmount.SetValue(Amount);

                    DiscountTextChange(s, e);
                    //.........AvailableStock.............
                    //cacpAvailableStock.PerformCallback(strProductID);
                }
                else {
                    jAlert('Select a product first.');
                    grid.GetEditor('gvColQuantity').SetValue('0');
                    grid.GetEditor('gvColProduct').Focus();
                }
            }
        }
        function DiscountTextChange(s, e) {
            //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            var Discount = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var ProductID = grid.GetEditor('gvColProduct').GetValue();

            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                //var strFactor = SpliteDetails[14];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
                if (strSalePrice == '0') {
                    strSalePrice = SpliteDetails[6];
                }
                if (strRate == 0) {
                    strRate = 1;
                }
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
                var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

                var tbAmount = grid.GetEditor("gvColAmount");
                tbAmount.SetValue(amountAfterDiscount);

                var IsPackingActive = SpliteDetails[10];
                var Packing_Factor = SpliteDetails[11];
                var Packing_UOM = SpliteDetails[12];
                var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

                if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                    $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                    //divPacking.style.display = "block";
                        divPacking.style.display = "none";
                    } else {
                        divPacking.style.display = "none";
                    }

                    var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                    tbTotalAmount.SetValue(amountAfterDiscount);


                    var ShippingStateCode = cbsSCmbState.GetValue();
                    var TaxType = "";
                    if (cddl_AmountAre.GetValue() == "1") {
                        TaxType = "E";
                    }
                    else if (cddl_AmountAre.GetValue() == "2") {
                        TaxType = "I";
                    }
                // console.log(SpliteDetails);
                    caluculateAndSetGST(grid.GetEditor("gvColAmount"), grid.GetEditor("gvColTaxAmount"), grid.GetEditor("gvColTotalAmountINR"), SpliteDetails[19], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), 'P');

                    Cur_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                    Cur_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
                    Cur_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
                    CalculateAmount();
                }
                else {
                    jAlert('Select a product first.');
                    grid.GetEditor('gvColDiscount').SetValue('0');
                    grid.GetEditor('gvColProduct').Focus();
                }
            //Debjyoti 
            //grid.GetEditor('gvColTaxAmount').SetValue(0);

                if (parseFloat(Cur_TotalAmt) != parseFloat(Pre_TotalAmt)) {
                    ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
                }
            }

            function AmountTextFocus(s, e) {
                var Amount = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
                _GetAmountValue = Amount;

                Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
                Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
            }

            function AmountTextChange(s, e) {
                var Amount = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
                var TaxAmount = (grid.GetEditor('gvColTaxAmount').GetValue() != null) ? grid.GetEditor('gvColTaxAmount').GetValue() : "0";
                var ProductID = grid.GetEditor('gvColProduct').GetValue();
                var SpliteDetails = ProductID.split("||@||");

                if (parseFloat(_GetAmountValue) != parseFloat(Amount)) {
                    var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                    tbTotalAmount.SetValue(Amount + TaxAmount);

                    var ShippingStateCode = cbsSCmbState.GetValue();
                    var TaxType = "";
                    if (cddl_AmountAre.GetValue() == "1") {
                        TaxType = "E";
                    }
                    else if (cddl_AmountAre.GetValue() == "2") {
                        TaxType = "I";
                    }

                    caluculateAndSetGST(grid.GetEditor("gvColAmount"), grid.GetEditor("gvColTaxAmount"), grid.GetEditor("gvColTotalAmountINR"), SpliteDetails[19], Amount, Amount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), 'P');

                    Cur_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                    Cur_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
                    Cur_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
                    CalculateAmount();
                }
            }

            //......................Amount Calculation End.......................


            var globalRowIndex;
            function PerformCallToGridBind() {
                //

                grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
                cProductsPopup.Hide();
                //#### added by : Samrat Roy for Transporter & Billing/Shipping Doc Tagging #############
                var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

                if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                    callTransporterControl(quote_Id[0], 'PO');
                }

                if (quote_Id.length > 0) {
                    BSDocTagging(quote_Id[0], 'PO');
                }
                //#### END : Samrat Roy for Billing/Shipping Doc Tagging : END #############

                //#### added by Sayan Dutta for TC Control #############
                if ($("#btn_TermsCondition").is(":visible")) {
                    callTCControl(quote_Id[0], 'PO');
                }
                //#### End : added by Sayan Dutta for TC Control : End #############

                return false;
            }

            function componentEndCallBack(s, e) {
                gridquotationLookup.gridView.Refresh();
                if (grid.GetVisibleRowsOnPage() == 0) {
                    OnAddNewClick();
                }
            }
            function CloseGridQuotationLookup() {
                gridquotationLookup.ConfirmCurrentSelection();
                gridquotationLookup.HideDropDown();
                gridquotationLookup.Focus();
            }
            function QuotationNumberChanged() {
                //
                var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage(); //gridquotationLookup.GetValue();
                quote_Id = quote_Id.join();

                if (quote_Id != null) {
                    var arr = quote_Id.split(',');
                    if (arr.length > 1) {
                        cPLQADate.SetText('Multiple Purchase Order Dates');
                    }
                    else {
                        if (arr.length == 1) {
                            //cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);
                            var selectIndex = gridquotationLookup.gridView.GetFocusedRowIndex()
                            var orderDate = gridquotationLookup.gridView.GetRow(selectIndex).children[2].innerText;
                            cPLQADate.SetText(orderDate);
                        }
                        else {
                            cPLQADate.SetText('');
                        }
                    }
                }
                else { cPLQADate.SetText(''); }

                if (quote_Id != null) {
                    cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
                    cProductsPopup.Show();
                }
                else {
                    cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
                    cProductsPopup.Show();
                }
                //txt_OANumber.Focus();           
            }
            function SetDifference1() {
                var diff = CheckDifferenceOfFromDateWithTodate();
            }
            function CheckDifferenceOfFromDateWithTodate() {
                var startDate = new Date();
                var endDate = new Date();
                var difference = -1;
                startDate = cPLSalesOrderDate.GetDate();
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
                startDate = cPLSalesOrderDate.GetDate();
                if (startDate != null) {
                    endDate = cExpiryDate.GetDate();
                    var startTime = startDate.getTime();
                    var endTime = endDate.getTime();
                    difference = (endTime - startTime) / 86400000;

                }
                return difference;

            }
            //.................WareHouse.......


            function fn_Deletecity(keyValue) {
                var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
                var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

                cGrdWarehousePC.PerformCallback('Delete~' + keyValue);
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
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
            function OnListBoxSelectionChanged(listBox, args) {
                if (args.index == 0)
                    args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
                UpdateSelectAllItemState();
                UpdateText();
            }
            function SynchronizeListBoxValues(dropDown, args) {
                checkListBox.UnselectAll();
                // var texts = dropDown.GetText().split(textSeparator);
                var texts = selectedChkValue.split(textSeparator);

                var values = GetValuesByTexts(texts);
                checkListBox.SelectValues(values);
                UpdateSelectAllItemState();
                UpdateText(); // for remove non-existing texts
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
            function UpdateSelectAllItemState() {
                IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
            }
            function UpdateText() {
                var selectedItems = checkListBox.GetSelectedItems();
                selectedChkValue = GetSelectedItemsText(selectedItems);
                //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
                checkComboBox.SetText(selectedItems.length + " Items");

                var val = GetSelectedItemsText(selectedItems);
                $("#abpl").attr('data-content', val);
            }

            //...............end..........................

            function GetVisibleIndex(s, e) {
                globalRowIndex = e.visibleIndex;
            }


            function OnEndCallback(s, e) {
                var value = document.getElementById('hdnRefreshType').value;
                var pageStatus = document.getElementById('hdnPageStatus').value;

                if (grid.cpinserterrorwarehouse != null) {
                    LoadingPanel.Hide();
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert(grid.cpinserterrorwarehouse);
                    grid.cpinserterrorwarehouse = null;
                }
                else if (grid.cpSaveSuccessOrFail == "outrange") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert('Can Not Add More Purchase Oder Number as Purchase Order Scheme Exausted.<br />Update The Scheme and Try Again');
                }
                else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert('Please fill Quantity');
                }
                else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert('Can not Duplicate Product in the Challan List.');
                }
                else if (grid.cpSaveSuccessOrFail == "nullPurchasePrice") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert('Purchase Price is Mandatory. Please enter values.');
                }
                else if (grid.cpSaveSuccessOrFail == "duplicateSerial") {
                    LoadingPanel.Hide();
                    var Msg = grid.cpduplicateSerialMsg;

                    grid.cpSaveSuccessOrFail = null;
                    grid.cpduplicateSerialMsg = null;
                    grid.batchEditApi.StartEdit(0, 2);

                    jAlert(Msg);
                }
                else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                    LoadingPanel.Hide();
                    grid.batchEditApi.StartEdit(0, 2);
                    var SrlNo = grid.cpProductSrlIDCheck;

                    grid.cpSaveSuccessOrFail = null;
                    grid.cpProductSrlIDCheck = null;
                    //var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
                    var msg = "Make sure product quantity are equal with Warehouse quantity for SL No. " + SrlNo;
                    jAlert(msg);
                }
                else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                }
                else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
                }
                else if (grid.cpSaveSuccessOrFail == "duplicate") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert('Can Not Save as Duplicate Purchase Order Numbe No. Found');
                }
                else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert('Please try after sometime.');
                }
                else if (grid.cpSaveSuccessOrFail == "transactionbeingused") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert('Transaction exist. cannot be processed.');
                }
                else if (grid.cpSaveSuccessOrFail == "Ponotexist") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 2);
                    jAlert('Cannot Save. Selected Purchase   Order(s) in this document do not exist.');
                }
                else if (grid.cpSaveSuccessOrFail == "stockOut") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;
                    grid.batchEditApi.StartEdit(0, 3);
                    jAlert('Already stock out for this product.');
                }
                else if (grid.cpSaveSuccessOrFail == "allStockOut") {
                    LoadingPanel.Hide();
                    grid.cpSaveSuccessOrFail = null;

                    jAlert("Already stock out for selected products.", 'Alert Dialog: [PurchaseChallan_Stock]', function (r) {
                        if (r == true) {
                            window.location.reload();
                        }
                    });
                }
                else {

                    var PurchaseOrder_Number = grid.cpPurchaseOrderNo;
                    var Order_Msg = "Purchase Challan No. " + PurchaseOrder_Number + " saved.";
                    if (value == "E") {
                        //window.location.assign("PurchaseChallanList_Stock.aspx");
                        if (grid.cpApproverStatus == "approve") {
                            window.parent.popup.Hide();
                            window.parent.cgridPendingApproval.PerformCallback();
                        }
                        else if (grid.cpApproverStatus == "rejected") {
                            window.parent.popup.Hide();
                            window.parent.cgridPendingApproval.PerformCallback();
                        }

                        if (PurchaseOrder_Number != "") {


                            jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {
                                if (r == true) {
                                    grid.cpSalesOrderNo = null;
                                    window.location.assign("PurchaseChallanList_Stock.aspx");
                                }
                            });

                        }
                        else {
                            window.location.assign("PurchaseChallanList_Stock.aspx");
                        }
                    }
                    else if (value == "N") {
                        if (grid.cpApproverStatus == "approve") {
                            window.parent.popup.Hide();
                            window.parent.cgridPendingApproval.PerformCallback();
                        }
                        else if (grid.cpApproverStatus == "rejected") {
                            window.parent.popup.Hide();
                            window.parent.cgridPendingApproval.PerformCallback();
                        }
                        if (PurchaseOrder_Number != "") {
                            jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {

                                grid.cpSalesOrderNo = null;
                                if (r == true) {
                                    window.location.assign("PurchaseChallanList_Stock.aspx?key=ADD");
                                }
                            });


                        }
                        else {
                            window.location.assign("PurchaseChallan_Stock.aspx?key=ADD");
                        }
                    }
                    else {
                        if (pageStatus == "first") {
                            OnAddNewClick();
                            grid.batchEditApi.EndEdit();
                            //document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();
                            $('#<%=hdnPageStatus.ClientID %>').val('');
                        }
                        else if (pageStatus == "update") {

                            OnAddNewClick();
                            $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
            }

        var taxType = cddl_AmountAre.GetValue();
        if (taxType == 3) {
            grid.GetEditor('gvColTaxAmount').SetEnabled(false);
        }

        if (gridquotationLookup.GetValue() != null) {
            grid.GetEditor('ProductName').SetEnabled(false);
            grid.GetEditor('gvColDiscription').SetEnabled(false);
            $('#<%=IsPOTagged.ClientID %>').val('true');
        }
        else {
            <%--if (grid.GetVisibleRowsOnPage() == 0) {
                //OnAddNewClick();

                grid.StartEditRow(0);
                $('#<%=hdnPageStatus.ClientID %>').val('');
            }--%>

            if (grid.cpComponent) {
                if (grid.cpComponent == 'true') {
                    grid.cpComponent = null;
                    OnAddNewClick();
                }
            }
            else {
                grid.StartEditRow(0);
                $('#<%=hdnPageStatus.ClientID %>').val('');
            }
        }
    }

                //alert(pageStatus);
                //if (pageStatus == "update") {
                //    grid.AddNewRow();
                //}
    if (grid.cpPurchaseorderbindnewrow == "yes") {
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
    }

    if (grid.cpOrderRunningBalance) {
        var RunningBalance = grid.cpOrderRunningBalance;
        var RunningSpliteDetails = RunningBalance.split("~");
        grid.cpOrderRunningBalance = null;

        var SUM_ChargesAmount = RunningSpliteDetails[0];
        var SUM_Amount = RunningSpliteDetails[1];
        //var SUM_ChargesAmount = RunningSpliteDetails[2];
        var SUM_TaxAmount = RunningSpliteDetails[3];
        var SUM_TotalAmount = RunningSpliteDetails[4];
        //var SUM_TotalAmount = RunningSpliteDetails[5];
        var SUM_ProductQuantity = parseFloat(RunningSpliteDetails[6]).toFixed(2);

        cTaxableAmtval.SetValue(SUM_Amount);
        cTaxAmtval.SetValue(SUM_TaxAmount);
        ctxt_Charges.SetValue(SUM_ChargesAmount);
        cOtherTaxAmtval.SetValue(SUM_ChargesAmount);
        cInvValue.SetValue(SUM_TotalAmount);
        cTotalAmt.SetValue(SUM_TotalAmount);
        cTotalQty.SetValue(SUM_ProductQuantity);
    }

    cProductsPopup.Hide();

}
function GridCallBack() {

    //page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
    grid.PerformCallback('Display');
}
var Warehouseindex;
function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        $('#<%=hdnRefreshType.ClientID %>').val('');

        if (gridquotationLookup.GetValue() != null) {
            jAlert('Cannot Delete using this button as the Purchase Challan is linked with this Purchase Order.<br /> Click on Plus(+) sign to Add or Delete Product from last column!', 'Alert Dialog: [Delete Challan Products]', function (r) {

            });
        }
        if (noofvisiblerows != "1" && gridquotationLookup.GetValue() == null) {
            //Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
            //Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

            Pre_Quantity = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColQuantity') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColQuantity') : "0";
            Pre_Amt = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColAmount') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColAmount') : "0";
            Pre_TotalAmt = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColTotalAmountINR') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColTotalAmountINR') : "0";

            Cur_Quantity = "0";
            Cur_Amt = "0";
            Cur_TotalAmt = "0";
            CalculateAmount();

            grid.DeleteRow(e.visibleIndex);

            $('#<%=hdfIsDelete.ClientID %>').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display');

            grid.batchEditApi.StartEdit(-1, 2);
            grid.batchEditApi.StartEdit(0, 2);
        }
      <%--  grid.batchEditApi.EndEdit();


        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        if (noofvisiblerows != "1") {

            grid.DeleteRow(e.visibleIndex);

            $('#<%=hdfIsDelete.ClientID %>').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display');
            grid.batchEditApi.StartEdit(-1, 2);
            grid.batchEditApi.StartEdit(0, 2);
        }--%>
    }
    if (e.buttonID == 'CustomAddNewRow') {

        var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "";
        var SpliteDetails = ProductID.split("||@||");

        var IsComponentProduct = SpliteDetails[17];
        var ComponentProduct = SpliteDetails[18];

        if (IsComponentProduct == "Y") {
            var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
            jConfirm(messege, 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.GetEditor("IsComponentProduct").SetValue("Y");
                    $('#<%=hdfIsDelete.ClientID %>').val('C');

                    grid.UpdateEdit();
                    grid.PerformCallback('Display~fromComponent');
                }
                else {
                    OnAddNewClick();
                }
            });
            document.getElementById('popup_ok').focus();
        }
        else {
            if (ProductID != "") {
                OnAddNewClick();
            }
            else {

                grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            }
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;

        var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

        if (inventoryType == "C" || inventoryType == "Y" || inventoryType == "B") {
            var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

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
                    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "";
                    var StkQuantityValue = QuantityValue * strMultiplier;
                    var stockids = SpliteDetails[10];
                    var Ptype = SpliteDetails[16];
                    var StkQuantityValue = QuantityValue * strMultiplier;

                    $('#<%=hdfProductType.ClientID %>').val(Ptype);

                    $('#<%=hdfProductID.ClientID %>').val(strProductID);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);

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
        else {
            jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
        }
    }

    <%--{
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;
        // $('#<%=hdfstockidPC.ClientID %>').val(0);
        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";
        var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
        var POnumber = (grid.GetEditor('PoNumber').GetValue() != null) ? grid.GetEditor('PoNumber').GetValue() : "0";
        if (QuantityValue == "0.0") {
            jAlert("Quantity should not be zero !.");
        } else {
            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();
            //alert(ProductID);
            if (ProductID != "") {

                $('#<%=hdnisoldupdate.ClientID %>').val("false");
                $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                $('#<%=hdnisnewupdate.ClientID %>').val("false");

                $('#<%=hdnPOnumber.ClientID %>').val(POnumber);

                var SpliteDetails = ProductID.split("||@||");
                //alert(SpliteDetails[10]);
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strMultiplier = SpliteDetails[7];
                var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "";
                var StkQuantityValue = QuantityValue * strMultiplier;
                var stockids = SpliteDetails[10];
                //alert(SpliteDetails);
                //alert(SpliteDetails[10]);

                if (stockids == "0") {
                    jAlert("Please Update the Opening Stock!.");
                } else {



                    document.getElementById('<%=lblAvailableStkunit.ClientID %>').innerHTML = strUOM;
                    document.getElementById('<%=lblopeningstockUnit.ClientID %>').innerHTML = strUOM;
                    $('#<%=hdfProductIDPC.ClientID %>').val(strProductID);

                    $('#<%=hdfstockidPC.ClientID %>').val(stockids);

                    $('#<%=hdfProductType.ClientID %>').val("");
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
                    var Ptype = "";

                    $('#<%=hdnisserial.ClientID %>').val("");
                    $('#<%=hdnisbatch.ClientID %>').val("");
                    $('#<%=hdniswarehouse.ClientID %>').val("");

                    $.ajax({
                        type: "POST",
                        url: 'PurchaseChallan_Stock.aspx/getProductType',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{Products_ID:\"" + strProductID + "\"}",
                        success: function (type) {
                            Ptype = type.d;
                            $('#<%=hdfProductType.ClientID %>').val(Ptype);
                            if (Ptype == "") {
                                jAlert("No Warehouse or Batch or Serial is actived !.");
                            } else {
                                grid.batchEditApi.EndEdit();

                                //alert(Ptype);
                                if (Ptype == "W") {
                                    $('#<%=hdnisserial.ClientID %>').val("false");
                                    $('#<%=hdnisbatch.ClientID %>').val("false");
                                    $('#<%=hdniswarehouse.ClientID %>').val("true");
                                    //cCmbWarehouse.PerformCallback('BindWarehouse');
                                    cCmbWarehouse.Focus();
                                }

                                else if (Ptype == "B") {
                                    $('#<%=hdnisserial.ClientID %>').val("false");
                                    $('#<%=hdnisbatch.ClientID %>').val("false");
                                    $('#<%=hdniswarehouse.ClientID %>').val("false");
                                    ctxtbatch.Focus();
                                }
                                else if (Ptype == "S") {
                                    $('#<%=hdnisserial.ClientID %>').val("false");
                                    $('#<%=hdnisbatch.ClientID %>').val("false");
                                    $('#<%=hdniswarehouse.ClientID %>').val("false");

                                }
                                else if (Ptype == "WB") {

                                    $('#<%=hdnisserial.ClientID %>').val("false");
                                    $('#<%=hdnisbatch.ClientID %>').val("true");
                                    $('#<%=hdniswarehouse.ClientID %>').val("true");
                                    //cCmbWarehouse.PerformCallback('BindWarehouse');
                                    //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                                    cCmbWarehouse.Focus();
                                }
                                else if (Ptype == "WS") {
                                    $('#<%=hdnisserial.ClientID %>').val("true");
                                    $('#<%=hdnisbatch.ClientID %>').val("false");
                                    $('#<%=hdniswarehouse.ClientID %>').val("true");
                                    //cCmbWarehouse.PerformCallback('BindWarehouse');
                                    //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                                    cCmbWarehouse.Focus();
                                }
                                else if (Ptype == "WBS") {
                                    $('#<%=hdnisserial.ClientID %>').val("true");
                                    $('#<%=hdnisbatch.ClientID %>').val("true");
                                    $('#<%=hdniswarehouse.ClientID %>').val("true");
                                    //cCmbWarehouse.PerformCallback('BindWarehouse');
                                    //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                                    cCmbWarehouse.Focus();
                                }
                                else if (Ptype == "BS") {
                                    $('#<%=hdnisserial.ClientID %>').val("true");
                                    $('#<%=hdnisbatch.ClientID %>').val("true");
                                    $('#<%=hdniswarehouse.ClientID %>').val("false");

                                    ctxtbatch.Focus();
                                    //cCmbBatch.PerformCallback('BindBatch~' + "0");
                                    //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                                }
                                else {
                                    $('#<%=hdnisserial.ClientID %>').val("false");
                                    $('#<%=hdnisbatch.ClientID %>').val("false");
                                    $('#<%=hdniswarehouse.ClientID %>').val("false");
                                }

        $("#RequiredFieldValidatortxtbatch").css("display", "none");
        $("#RequiredFieldValidatortxtserial").css("display", "none");
        $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");

        $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
        $("#RequiredFieldValidatortxtwareqntity").css("display", "none");

        $(".blockone").css("display", "none");
        $(".blocktwo").css("display", "none");
        $(".blockthree").css("display", "none");

        ctxtqnty.SetText("0.0");
        ctxtqnty.SetEnabled(true);

        ctxtbatchqnty.SetText("0.0");
        ctxtserial.SetText("");
        ctxtbatchqnty.SetText("");

        ctxtbatch.SetEnabled(true);
        cCmbWarehouse.SetEnabled(true);

        $('#<%=hdnoutstock.ClientID %>').val("0");
        $('#<%=hdnisedited.ClientID %>').val("false");
                                $('#<%=hdnisoldupdate.ClientID %>').val("false");
                                $('#<%=hdnisnewupdate.ClientID %>').val("false");

                                $('#<%=hdnisolddeleted.ClientID %>').val("false");

                                $('#<%=hdntotalqntyPC.ClientID %>').val(0);
                                $('#<%=hdnoldrowcount.ClientID %>').val(0);
                                $('#<%=hdndeleteqnity.ClientID %>').val(0);
                                $('#<%=hidencountforserial.ClientID %>').val("1");


                                $('#<%=hdfopeningstockPC.ClientID %>').val(0);
                                $('#<%=oldopeningqntity.ClientID %>').val(0);
                                $('#<%=hdnnewenterqntity.ClientID %>').val(0);

                                $('#<%=hdnenterdopenqnty.ClientID %>').val(0);
                                $('#<%=hdbranchIDPC.ClientID %>').val(0);

                                $('#<%=hdnisviewqntityhas.ClientID %>').val("false");


                                $('#<%=hdndefaultID.ClientID %>').val("");
                                $('#<%=hdnbatchchanged.ClientID %>').val("0");
                                $('#<%=hdnrate.ClientID %>').val("0");
                                $('#<%=hdnvalue.ClientID %>').val("0");
                                $('#<%=hdnstrUOM.ClientID %>').val(strUOM);
                                // var branchid = ccmbbranch.GetValue();
                                var branchid = $("#ddl_Branch option:selected").val();

                                $('#<%=hdnisreduing.ClientID %>').val("false");

                                var productid = strProductID ? strProductID : "";
                                var StkQuantityValue = QuantityValue ? QuantityValue : "0.0000";


                                var checkstockdecimalornot = StkQuantityValue.toString().split(".")[1]

                                $('#<%=hdnpcslno.ClientID %>').val(SrlNo);
                                //var ProductName = (cOpeningGrid.GetEditor('ProductName').GetValue() != null) ? cOpeningGrid.GetEditor('ProductID').GetValue() : "";
                                var ProductName = SpliteDetails[12];
                                var ratevalue = "0";
                                var rate = "0";
                                // var branchid = (cOpeningGrid.GetEditor('branch').GetValue() != null) ? cOpeningGrid.GetEditor('branch').GetValue() : "0";
                                // var branchid = ccmbbranch.GetValue();
                                var branchid = $('#<%=ddl_Branch.ClientID %>').val();
                                //var BranchNames = (cOpeningGrid.GetEditor('branch').GetText() != null) ? cOpeningGrid.GetEditor('branch').GetText() : "0";
                                //var BranchNames = ccmbbranch.GetText();

                                var BranchNames = $("#ddl_Branch option:selected").text();
                                //alert(BranchNames);
                                // ProductName = ProductName.replace('dquote', '"');
                                var strProductID = productid;
                                var strDescription = "";
                                var strUOM = (strUOM != null) ? strUOM : "0";
                                var strProductName = ProductName;

                                document.getElementById('<%=lblbranchName.ClientID %>').innerHTML = BranchNames;
                                var availablestock = SpliteDetails[11];
                                $('#<%=hdndefaultID.ClientID %>').val("0");


                                var calculateopein = Number(StkQuantityValue) - Number(availablestock);
                                var oldopeing = 0;
                                var oldqnt = Number(oldopeing);

                                $('#<%=hdfopeningstockPC.ClientID %>').val(QuantityValue);
                                $('#<%=oldopeningqntity.ClientID %>').val(0);
                                $('#<%=hdnnewenterqntity.ClientID %>').val(QuantityValue);
                                $('#<%=hdnenterdopenqnty.ClientID %>').val(0);
                                $('#<%=hdbranchIDPC.ClientID %>').val(branchid);
                                $('#<%=hdnselectedbranch.ClientID %>').val(branchid);

                                $('#<%=hdnrate.ClientID %>').val(rate);
                                $('#<%=hdnvalue.ClientID %>').val(ratevalue);

                                var dtd = (Number(StkQuantityValue)).toFixed(4);

                                $("#lblopeningstock").text(dtd + " ");

                                ctxtmkgdate.SetDate = null;
                                txtexpirdate.SetDate = null;
                                ctxtserial.SetValue("");
                                ctxtbatch.SetValue("");
                                ctxtqnty.SetValue("0.0");
                                ctxtbatchqnty.SetValue("0.0");

                                var hv = $('#hdnselectedbranch').val();

                                var iswarehousactive = $('#hdniswarehouse').val();
                                var isactivebatch = $('#hdnisbatch').val();
                                var isactiveserial = $('#hdnisserial').val();
                                // alert(iswarehousactive + "/" + isactivebatch + "/" + isactiveserial);

                                cCmbWarehouse.PerformCallback('BindWarehouse');

                                if (iswarehousactive == "true") {

                                    cCmbWarehouse.SetVisible(true);
                                    cCmbWarehouse.SetSelectedIndex(1);
                                    cCmbWarehouse.Focus();
                                    ctxtqnty.SetVisible(true);
                                    $('#<%=hdniswarehouse.ClientID %>').val("true");

                                    $(".blockone").css("display", "block");

                                } else {
                                    cCmbWarehouse.SetVisible(false);
                                    ctxtqnty.SetVisible(false);
                                    $('#<%=hdniswarehouse.ClientID %>').val("false");
                                    cCmbWarehouse.SetSelectedIndex(-1);
                                    $(".blockone").css("display", "none");

                                }

                                if (isactivebatch == "true") {

                                    ctxtbatch.SetVisible(true);
                                    ctxtmkgdate.SetVisible(true);
                                    ctxtexpirdate.SetVisible(true);
                                    $('#<%=hdnisbatch.ClientID %>').val("true");

                                    $(".blocktwo").css("display", "block");

                                } else {
                                    ctxtbatch.SetVisible(false);
                                    ctxtmkgdate.SetVisible(false);
                                    ctxtexpirdate.SetVisible(false);
                                    $('#<%=hdnisbatch.ClientID %>').val("false");

                                    $(".blocktwo").css("display", "none");

                                }
                                if (isactiveserial == "true") {
                                    ctxtserial.SetVisible(true);
                                    $('#<%=hdnisserial.ClientID %>').val("true");


                                    $(".blockthree").css("display", "block");
                                } else {
                                    ctxtserial.SetVisible(false);
                                    $('#<%=hdnisserial.ClientID %>').val("false");


                                    $(".blockthree").css("display", "none");
                                }

                                if (iswarehousactive == "false" && isactivebatch == "true") {
                                    ctxtbatchqnty.SetVisible(true);

                                    $(".blocktwoqntity").css("display", "block");
                                } else {
                                    ctxtbatchqnty.SetVisible(false);
                                    $(".blocktwoqntity").css("display", "none");
                                }

                                if (iswarehousactive == "false" && isactivebatch == "true") {
                                    ctxtbatch.Focus();
                                } else {
                                    cCmbWarehouse.Focus();
                                }

                                cbtnWarehouse.SetVisible(true);
                                cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

                                cPopup_WarehousePCPC.Show();
                            }
                        }
                    });
                }
            }


        }
    }--%>
}
        function Save_ButtonClick() {
            flag = true;
            LoadingPanel.Show();

            var txtPurchaseNo = $("#txtVoucherNo").val().trim();
            var ddl_Vendor = $("#ddl_Vendor").val();

            if (txtPurchaseNo == null || txtPurchaseNo == "") {
                //flag = false;
                LoadingPanel.Hide();
                $("#MandatoryBillNo").show();
                return false;
            }
            if (ddl_Vendor == 0) {
                // flag = false;
                $("#MandatoryBillNo").show();
                return false;
            }

            //var customerId = GetObjectID('hdnCustomerId').value
            var customerId = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

            if (customerId == '' || customerId == null) {
                LoadingPanel.Hide();
                $('#MandatorysCustomer').attr('style', 'display:block');
                flag = false;
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }

            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsProduct = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (flag != false) {
                if (IsProduct == "Y") {
                    var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    $('#<%=hdnRefreshType.ClientID %>').val('N');
                    grid.batchEditApi.EndEdit();
                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                    grid.UpdateEdit();
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('Please add atleast single record first');
                }
            }

            <%--if (flag != false) {
                $('#<%=hdfIsDelete.ClientID %>').val('I');
                grid.batchEditApi.EndEdit();
                grid.UpdateEdit();
            }--%>
        }
        function SaveExit_ButtonClick() {
            flag = true;
            LoadingPanel.Show();

            var txtPurchaseNo = $("#txtVoucherNo").val().trim();
            var ddl_Vendor = $("#ddl_Vendor").val();
            //alert(txtPurchaseNo);
            if (txtPurchaseNo == null || txtPurchaseNo == "") {
                flag = false;
                LoadingPanel.Hide();
                $("#MandatoryBillNo").show();
                return false;
            }
            if (ddl_Vendor == 0) {
                flag = false;
                $("#MandatoryBillNo").show();
                return false;
            }

            //var customerId = GetObjectID('hdnCustomerId').value;
            var customerId = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

            if (customerId == '' || customerId == null) {
                LoadingPanel.Hide();
                $('#MandatorysCustomer').attr('style', 'display:block');
                flag = false;
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }

            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsProduct = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (flag != false) {
                if (IsProduct == "Y") {
                    var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    grid.batchEditApi.EndEdit();
                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                    grid.UpdateEdit();
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('Please add atleast one record.');
                }
            }
        }
        function OnAddNewClick() {
            //grid.AddNewRow();
            //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            //var tbQuotation = grid.GetEditor("SrlNo");
            //tbQuotation.SetValue(noofvisiblerows);
            if (gridquotationLookup.GetValue() == null) {
                grid.AddNewRow();
                //grid.StartEditRow(0);
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                //var i;
                //var cnt = 1;
                //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                //    var tbQuotation = grid.GetEditor("SrlNo");
                //    tbQuotation.SetValue(cnt);


                //    cnt++;
                //}
            }
            else {
                QuotationNumberChanged();
            }

        }
        function ProductsCombo_SelectedIndexChanged(s, e) {
            var tbDescription = grid.GetEditor("gvColDiscription");
            var tbUOM = grid.GetEditor("gvColUOM");
            var tbStockUOM = grid.GetEditor("gvColStockUOM");
            var tbPurchasePrice = grid.GetEditor("gvColStockPurchasePrice");


            //var ProductID = s.GetValue();
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStockUOM = SpliteDetails[4];
            var strPurchasePrice = SpliteDetails[6];

            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tbStockUOM.SetValue(strStockUOM);
            tbPurchasePrice.SetValue(strPurchasePrice);

        }
        function ddl_Currency_Rate_Change() {
            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var Currency_ID = $("#ddl_Currency").val();


            if (Currency_ID == basedCurrency[0]) {
                ctxtRate.SetValue("0");
                ctxtRate.SetEnabled(false);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "PurchaseChallan_Stock.aspx/GetRate",
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
        }
        function ddl_AmountAre_valueChange() {
            var key = $("#ddl_AmountAre").val();
            if (key == 1) {
                // grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                cddlVatGstCst.PerformCallback('1');
            }
            else if (key == 2) {
                // grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');

            }
            else if (key == 3) {
                //  grid.GetEditor('TaxAmount').SetEnabled(false);
                cddlVatGstCst.SetEnabled(false);
                cddlVatGstCst.PerformCallback('3');

            }
        }
        //function ddl_Vendor_ValueChange() {
        //    var venderId = $("#ddl_Vendor").val();
        //    cContactPerson.PerformCallback("BindContact~" + venderId);
        //}
        function GetIndentREquiNo(e) {
            var PODate = new Date();
            PODate = cPLQuoteDate.GetValueString();
            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            //cQuotationComponentPanel.PerformCallback(PODate);
        }
        function GetContactPerson(e) {
            if (gridLookup.GetValue() != null) {
                var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                if (key != null && key != '') {
                    cContactPerson.PerformCallback('BindContactPerson~' + key);

                    //###### Added By : Samrat Roy ##########
                    LoadCustomerAddress(key, $('#ddl_Branch').val(), 'PC');
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

                    ////################# Old Code #####################//
                    //cchkBilling.SetChecked(false);
                    //cchkShipping.SetChecked(false);
                    //page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                    //jConfirm('Wish to View/Enter Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                    //    if (r == true) {
                    //        page.SetActiveTabIndex(1);
                    //        $('.dxeErrorCellSys').addClass('abc');
                    //    }
                    //});
                    //GetObjectID('hdnCustomerId').value = key;

                    //###### END : Samrat Roy : END ########## 



                    //GetObjectID('hdnAddressDtl').value = '0';

                    var ddlbranch = $("[id*=ddl_Branch]");
                    var strBranch = ddlbranch.find("option:selected").val();

                    //startDate = cPLQuoteDate.GetValueString();
                    var _startDate = cPLQuoteDate.GetValue();
                    var startDate = GetPCDateFormat(new Date(cPLQuoteDate.GetValue()));

                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + strBranch);
                    <%--GetObjectID('hdnCustomerId').value = key;
                $("#<%=ddl_SalesAgent.ClientID%>").focus();--%>
                }
            }
        }
        //............Check Unique   Purchase Order................
        function txtBillNo_TextChanged() {

            var VoucherNo = document.getElementById("txtVoucherNo").value;

            $.ajax({
                type: "POST",
                url: "PurchaseChallan_Stock.aspx/CheckUniqueName",
                data: JSON.stringify({ VoucherNo: VoucherNo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        $("#MandatoryBillNo").show();

                        document.getElementById("txtVoucherNo").value = '';
                        document.getElementById("txtVoucherNo").focus();
                    }
                    else {
                        $("#MandatoryBillNo").hide();
                    }
                }
            });
        }

        function CmbScheme_ValueChange() {
            var val = $("#ddl_numberingScheme").val();
            gridLookup.gridView.SetFocusedRowIndex(-1);
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: 'PurchaseChallan_Stock.aspx/getSchemeType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{sel_scheme_id:\"" + val + "\"}",
                    success: function (type) {

                        var schemetypeValue = type.d;
                        var schemetype = schemetypeValue.toString().split('~')[0];
                        var schemelength = schemetypeValue.toString().split('~')[1];

                        var branchID = (schemetypeValue.toString().split('~')[2] != null) ? schemetypeValue.toString().split('~')[2] : "";
                        if (branchID != "") document.getElementById('ddl_Branch').value = branchID;

                        //if (gridLookup.Clear()) {
                        //    gridLookup.Clear();
                        //    gridLookup.GetGridView().Refresh();

                        //    setTimeout(function () {
                        //        gridLookup.Clear();
                        //        gridLookup.GetGridView().SetFocusedRowIndex(-1);
                        //    }, 500);
                        //}





                        $('#txtVoucherNo').attr('maxLength', schemelength);

                        var schemetypeValue = type.d;
                        var schemetype = schemetypeValue.toString().split('~')[0];
                        var schemelength = schemetypeValue.toString().split('~')[1];
                        $('#txtVoucherNo').attr('maxLength', schemelength);
                        if (schemetype == '0') {

                            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                            $("#txtVoucherNo").focus();

                        }
                        else if (schemetype == '1') {

                            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                            $("#MandatoryBillNo").hide();
                        }
                        else if (schemetype == '2') {

                            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
                        }
                        else if (schemetype == 'n') {
                            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                        }
                        else {
                            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                        }
                    }
                });
            }, 500);
}
function IndentRequisitionNo_ValueChange() {

    var val = $("#ddl_IndentRequisitionNo").val();
    if (val != 0) {
        $.ajax({
            type: "POST",
            url: 'PurchaseChallan_Stock.aspx/getIndentRequisitionDate',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{IndentRequisitionNo:\"" + val + "\"}",
            success: function (type) {
                //  console.log(type);
                var Transdt = new Date(type.d);
                cIndentRequisDate.SetDate(Transdt);

            }
        });
    }
    else {
        cIndentRequisDate.SetVal("");
    }

}


function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
}

//function GetContactPerson(e) {
//    var internalid = e.Get
//}

function SetDifference() {
    var diff = CheckDifference();
    if (diff > 0) {
        clientResult.SetText(diff.toString());
    }

}

function CheckDifference() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLQuoteDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;

    }
    return difference;

}

$(document).ready(function () {
    //$('#ddl_Customer').change(function () {
    //    var customerid = $(this).val();
    //    cContactPerson.PerformCallback('BindContactPerson~' + customerid);

    //})
})
function ChangeState(value) {

    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}

function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {

        if (cproductLookUp.Clear()) {
            Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

            cProductpopUp.Show();
            cproductLookUp.Focus();
            cproductLookUp.ShowDropDown();
        }
    }
}
function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    if (e.htmlEvent.key == "Tab") {

        s.OnButtonClick(0);
    }
}


function ProductsGotFocusFromID(s, e) {
    pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
    //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[13];
    var Packing_Factor = SpliteDetails[14];
    var Packing_UOM = SpliteDetails[15];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
        //divPacking.style.display = "block";
        divPacking.style.display = "none";
    } else {
        divPacking.style.display = "none";
    }

    $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
    $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
    $('#<%= lblProduct.ClientID %>').text(strProductName);
    $('#<%= lblbranchName.ClientID %>').text(strBranch);
    //alert(ProductID);
    //if (ProductID != "0") {
    //    cacpAvailableStock.PerformCallback(strProductID);
    //}
}
function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }
}

function ProductSelected(s, e) {

    if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
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
    grid.GetEditor("gvColProduct").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    //console.log(LookUpData);
    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);

    var tbDescription = grid.GetEditor("gvColDiscription");
    var tbUOM = grid.GetEditor("gvColUOM");
    var tbSalePrice = grid.GetEditor("gvColStockPurchasePrice");

    //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];

    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];

    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    if (strRate == 0) {
        strSalePrice = strSalePrice;
    }
    else {
        strSalePrice = strSalePrice / strRate;
    }

    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbSalePrice.SetValue(strSalePrice);

    grid.GetEditor("gvColQuantity").SetValue("0.00");
    grid.GetEditor("gvColDiscount").SetValue("0.00");
    grid.GetEditor("gvColAmount").SetValue("0.00");
    grid.GetEditor("gvColTaxAmount").SetValue("0.00");
    grid.GetEditor("gvColTotalAmountINR").SetValue("0.00");

    document.getElementById("ddlInventory").disabled = true;
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    $('#<%= lblStkQty.ClientID %>').text("0.00");
    $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
    $('#<%= lblProduct.ClientID %>').text(strDescription);
    $('#<%= lblbranchName.ClientID %>').text(strBranch);

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
        //divPacking.style.display = "block";
        divPacking.style.display = "none";
    } else {
        divPacking.style.display = "none";
    }

    Cur_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    Cur_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
    Cur_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
    CalculateAmount();

    //divPacking.style.display = "none";
    //lblbranchName lblProduct
    //tbStkUOM.SetValue(strStkUOM);
    //tbStockQuantity.SetValue("0");
    //Debjyoti

    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    grid.batchEditApi.StartEdit(globalRowIndex, 6);
}


function cmbContactPersonEndCall(s, e) {
    cddl_AmountAre.SetEnabled(true);
    var comboitem = cddl_AmountAre.FindItemByValue('4');
    if (comboitem != undefined && comboitem != null) {
        cddl_AmountAre.RemoveItem(comboitem.index);
    }

    if (cContactPerson.cpGSTN == "No") {
        if (cContactPerson.cpcountry != null && cContactPerson.cpcountry != '') {
            if (cContactPerson.cpcountry != '1') {
                cddl_AmountAre.AddItem("Import", "4");
                cddl_AmountAre.SetValue(4);
                cddl_AmountAre.SetEnabled(false);
            }
            else {
                cddl_AmountAre.SetValue(1);
            }
        }
        else {
            cddl_AmountAre.SetValue(1);
        }
    }
    else {
        cddl_AmountAre.SetValue(1);
    }

    cContactPerson.cpGSTN = null;
    cContactPerson.cpcountry = null;
}

    </script>

    <%-- TAX JQUERY- --%>
    <%--Debu Section--%>
    <script type="text/javascript">
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

            if (gridTax.cpChargesAmt) {
                ctxt_Charges.SetValue(gridTax.cpChargesAmt);
                gridTax.cpChargesAmt = null;

                Pre_Quantity = "0";
                Pre_Amt = "0";
                Pre_TotalAmt = "0";
                Cur_Quantity = "0";
                Cur_Amt = "0";
                Cur_TotalAmt = "0";
                CalculateAmount();
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

        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;

            //var ProductIDColumn = s.GetColumnByField("ProductID");
            //if (!e.rowValues.hasOwnProperty(ProductIDColumn.index))
            //    return;
            //var cellInfo = e.rowValues[ProductIDColumn.index];

            //if (cCmbProduct.FindItemByValue(cellInfo.value) != null) {
            //    cCmbProduct.SetValue(cellInfo.value);
            //}
            //else {
            //    cCmbProduct.SetSelectedIndex(-1);
            //}
        }

        function OnBatchEditEndEditing(s, e) {
            var ProductIDColumn = s.GetColumnByField("ProductID");
            if (!e.rowValues.hasOwnProperty(ProductIDColumn.index))
                return;
            var cellInfo = e.rowValues[ProductIDColumn.index];
            if (cCmbProduct.GetSelectedIndex() > -1 || cellInfo.text != cCmbProduct.GetText()) {
                cellInfo.value = cCmbProduct.GetValue();
                cellInfo.text = cCmbProduct.GetText();
                cCmbProduct.SetValue(null);
            }
        }

        function TaxAmountKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        var taxAmountGlobal;
        function taxAmountGotFocus(s, e) {
            taxAmountGlobal = parseFloat(s.GetValue());
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


        //for tax and charges
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




        function GetChargesTotalRunningAmount() {
            var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
            for (var i = 0; i < chargejsonTax.length; i++) {
                gridTax.batchEditApi.StartEdit(i, 3);
                runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.batchEditApi.EndEdit();
            }

            return runningTot;
        }

        function chargeCmbtaxClick(s, e) {
            GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
            ChargegstcstvatGlobalName = s.GetText();
        }

        //Code for UDF Control 
        function OpenUdf(s, e) {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                // var url = '../master/frm_BranchUdfPopUp.aspx?Type=SQO';

                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SQO&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();

            }
            return true;
        }
        // End Udf Code

        var GlobalCurTaxAmt = 0;
        var rowEditCtrl;
        var globalRowIndex;
        var globalTaxRowIndex;
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }
        function GetTaxVisibleIndex(s, e) {
            globalTaxRowIndex = e.visibleIndex;
        }
        function cmbtaxCodeindexChange(s, e) {
            if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {

                var taxValue = s.GetValue();

                if (taxValue == null) {
                    taxValue = 0;
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(0);
                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt));
                }


                var isValid = taxValue.indexOf('~');
                if (isValid != -1) {
                    var rate = parseFloat(taxValue.split('~')[1]);
                    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt));
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

                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
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

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                        gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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

        function taxAmtButnClick(s, e) {
            if (e.buttonIndex == 0) {

                if (cddl_AmountAre.GetValue() != null) {
                    var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";

                    if (ProductID.trim() != "") {
                        Pre_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                        Pre_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
                        Pre_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

                        document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();
                        caspxTaxpopUp.Show();
                        //Set Product Gross Amount and Net Amount

                        var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                        var SpliteDetails = ProductID.split("||@||");
                        var strMultiplier = SpliteDetails[7];
                        var strFactor = SpliteDetails[8];
                        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                        //var strRate = "1";
                        var strStkUOM = SpliteDetails[4];
                        // var strSalePrice = SpliteDetails[6];
                        var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "";
                        if (strRate == 0) {
                            strRate = 1;
                        }

                        var StockQuantity = strMultiplier * QuantityValue;
                        var Amount = parseFloat(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                        clblTaxProdGrossAmt.SetText(Amount);
                        clblProdNetAmt.SetText(parseFloat(grid.GetEditor('gvColAmount').GetValue()).toFixed(2));
                        document.getElementById('HdProdGrossAmt').value = Amount;
                        document.getElementById('HdProdNetAmt').value = parseFloat(grid.GetEditor('gvColAmount').GetValue()).toFixed(2);

                        //End Here

                        //Set Discount Here
                        if (parseFloat(grid.GetEditor('gvColDiscount').GetValue()) > 0) {
                            var discount = (parseFloat(Amount * grid.GetEditor('gvColDiscount').GetValue() / 100)).toFixed(2);
                            clblTaxDiscount.SetText(discount);
                        }
                        else {
                            clblTaxDiscount.SetText('0.00');
                        }
                        //End Here 


                        //Checking is gstcstvat will be hidden or not
                        if (cddl_AmountAre.GetValue() == "2") {
                            $('.GstCstvatClass').hide();
                            clblTaxableGross.SetText("(Taxable)");
                            clblTaxableNet.SetText("(Taxable)");
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
                                        clblProdNetAmt.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2));
                                        document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2);
                                        clblGstForNet.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
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

                            //###### Added By : Samrat Roy ##########
                            //Get Customer Shipping StateCode
                            var shippingStCode = '';
                            shippingStCode = cbsSCmbState.GetText();
                            shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                            //// ###########  Old Code #####################
                            //Get Customer Shipping StateCode
                            //var shippingStCode = '';
                            //if (cchkBilling.GetValue()) {
                            //    shippingStCode = CmbState.GetText();
                            //}
                            //else {
                            //    shippingStCode = CmbState1.GetText();
                            //}
                            //shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                            //###### END : Samrat Roy : END ########## 

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
                            cgridTax.cpComboCode = grid.GetEditor('gvColProduct').GetValue().split('||@||')[9];
                        }

                        ctxtprodBasicAmt.SetValue(grid.GetEditor('gvColAmount').GetValue());
                    } else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 14);
                    }
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
                grid.batchEditApi.StartEdit(globalRowIndex, 14);
                grid.GetEditor("gvColTaxAmount").SetValue(totAmt);

                if (cddl_AmountAre.GetValue() == "2") {
                    var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue());
                    var totalRoundOffAmount = Math.round(totalNetAmount);

                    grid.GetEditor("gvColTotalAmountINR").SetValue(totalRoundOffAmount);
                    grid.GetEditor("gvColAmount").SetValue(DecimalRoundoff(parseFloat(grid.GetEditor("gvColAmount").GetValue()) + (totalRoundOffAmount - totalNetAmount), 2));
                }
                else {
                    grid.GetEditor("gvColTotalAmountINR").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue()), 2));
                }

                Cur_Quantity = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                Cur_Amt = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
                Cur_TotalAmt = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
                CalculateAmount();
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

        function recalculateTax() {
            cmbGstCstVatChange(ccmbGstCstVat);
        }
        function recalculateTaxCharge() {
            ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
        }
        function Save_TaxesClick() {
            grid.batchEditApi.EndEdit();
            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var i, cnt = 1;
            var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

            cnt = 1;
            for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                var Amount = (grid.batchEditApi.GetCellValue(i, 'gvColAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColAmount')) : "0";
                var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount')) : "0";
                var Discount = (grid.batchEditApi.GetCellValue(i, 'gvColDiscount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColDiscount')) : "0";
                var NetAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR')) : "0";
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
                    var Amount = (grid.batchEditApi.GetCellValue(i, 'gvColAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColAmount')) : "0";
                    var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount')) : "0";
                    var Discount = (grid.batchEditApi.GetCellValue(i, 'gvColDiscount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColDiscount')) : "0";
                    var NetAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR')) : "0";
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

            ctxtProductAmount.SetValue(parseFloat(sumAmount).toFixed(2));
            ctxtProductTaxAmount.SetValue(parseFloat(sumTaxAmount).toFixed(2));
            ctxtProductDiscount.SetValue(parseFloat(sumDiscount).toFixed(2));
            ctxtProductNetAmount.SetValue(parseFloat(sumNetAmount).toFixed(2));
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
        function Save_TaxClick() {
            if (gridTax.GetVisibleRowsOnPage() > 0) {
                gridTax.UpdateEdit();
            }
            else {
                gridTax.PerformCallback('SaveGst');
            }
            cPopup_Taxes.Hide();
        }
    </script>
    <%--Debu Section End--%>

    <script>
        var SelectedWarehouseID = "0";
        var SelectWarehouse = "0";
        var IsFocus = "0";

        function closeWarehouse(s, e) {
            e.cancel = false;
            cGrdWarehouse.PerformCallback('WarehouseDelete');
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
        function GetPCDateFormat(today) {
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
                today = yyyy + '-' + mm + '-' + dd;
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
        function CmbWarehouseIDEndCallback(s, e) {
            if (SelectWarehouse != "0") {
                cCmbWarehouseID.SetValue(SelectWarehouse);
                SelectWarehouse = "0";
            }
            else {
                cCmbWarehouseID.Focus();
            }
        }
        function CallbackPanelEndCall(s, e) {
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

        function ddlInventory_OnChange() {
            cproductLookUp.GetGridView().Refresh();
        }

        function FinalWarehouse() {
            cGrdWarehouse.PerformCallback('WarehouseFinal');
        }

        function OnWarehouseEndCallback(s, e) {
            if (cGrdWarehouse.cperrorMsg == "duplicateSerial") {
                cGrdWarehouse.cperrorMsg = null;
                jAlert("Duplicate Serial. Cannot Proceed.");
            }
            else if (cGrdWarehouse.cpIsSave == "Y") {
                cGrdWarehouse.cpIsSave = null;
                cPopup_Warehouse.Hide();
                grid.batchEditApi.StartEdit(Warehouseindex, 5);
            }
            else if (cGrdWarehouse.cpIsSave == "N") {
                cGrdWarehouse.cpIsSave = null;
                jAlert('Purchase Quantity must be equal to Warehouse Quantity.');
            }

            if (IsFocus == "1") {
                ctxtserialID.Focus();
                IsFocus = "0";
            }
        }
    </script>

    <%--Added By : Samrat Roy -- New Billing/Shipping Section--%>
    <script>
        function SettingTabStatus() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            }
        }

        function disp_prompt(name) {

            if (name == "tab0") {
                gridLookup.Focus();
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
                    //fn_PopOpen();
                }
            }
        }

    </script>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section End--%>

    <script>
        var Pre_Quantity = "0";
        var Pre_Amt = "0";
        var Pre_TotalAmt = "0";
        var Cur_Quantity = "0";
        var Cur_Amt = "0";
        var Cur_TotalAmt = "0";

        function CalculateAmount() {
            var Quantity = (parseFloat((cTotalQty.GetValue()).toString())).toFixed(2);
            var Amount = (parseFloat((cTaxableAmtval.GetValue()).toString())).toFixed(2);
            var TotalAmount = (parseFloat((cInvValue.GetValue()).toString())).toFixed(2);
            var ChargesAmount = (ctxt_Charges.GetValue() != null) ? (parseFloat(ctxt_Charges.GetValue())).toFixed(2) : "0";

            var Calculate_Quantity = (parseFloat(Quantity) + parseFloat(Cur_Quantity) - parseFloat(Pre_Quantity)).toFixed(2);
            var Calculate_Amount = (parseFloat(Amount) + parseFloat(Cur_Amt) - parseFloat(Pre_Amt)).toFixed(2);
            var Calculate_TotalAmount = (parseFloat(TotalAmount) + parseFloat(Cur_TotalAmt) - parseFloat(Pre_TotalAmt)).toFixed(2);
            var Calculate_TaxAmount = (parseFloat(Calculate_TotalAmount) - parseFloat(Calculate_Amount)).toFixed(2);
            var Calculate_SumAmount = (parseFloat(Calculate_TotalAmount) + parseFloat(ChargesAmount)).toFixed(2);

            cTotalQty.SetValue(Calculate_Quantity);
            cTaxableAmtval.SetValue(Calculate_Amount);
            cTaxAmtval.SetValue(Calculate_TaxAmount);
            cOtherTaxAmtval.SetValue(ChargesAmount);
            cInvValue.SetValue(Calculate_TotalAmount);
            cTotalAmt.SetValue(Calculate_SumAmount);
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Add Purchase GRN"></asp:Label>

            </h3>
            <div id="pageheaderContent" class="pull-right reverse wrapHolder content horizontal-images" style="display: none;">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder" id="divAvailableStk">
                                <table>
                                    <tr>
                                        <td>Available Stock</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAvailableStkPro" runat="server" Text="0.0"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder" id="divPacking" style="display: none;">
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
                    </ul>
                    <ul style="display: none;">
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Selected Unit</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label13" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder">
                                <table>

                                    <tr>
                                        <td>Selected Product</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblProduct" runat="server"></asp:Label>
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
                                            <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <%--<div id="btncross" class="crossBtn" style="margin-left: 50px;"><a href="PurchaseChallanList_Stock.aspx"><i class="fa fa-times"></i></a></div>--%>
            <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
            <div id="divcross" runat="server" class="crossBtn"><a href="PurchaseChallanList_Stock.aspx"><i class="fa fa-times"></i></a></div>


        </div>

    </div>
    <div class=" form_main row">
        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
            <TabPages>

                <dxe:TabPage Name="General" Text="General">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <div class="row">
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Inventory" runat="server" Text="Inventory Item?">
                                    </dxe:ASPxLabel>
                                    <asp:DropDownList ID="ddlInventory" CssClass="backSelect" runat="server" Width="100%" onchange="ddlInventory_OnChange()">
                                        <asp:ListItem Text="Inventory Item" Value="Y" />
                                        <%-- <asp:ListItem Text="Non-Inventory Item" Value="N" />--%>
                                        <asp:ListItem Text="Capital Goods" Value="C" />
                                        <asp:ListItem Text="Both" Value="B" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8" runat="server" id="divNumberingScheme">
                                    <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                    </dxe:ASPxLabel>

                                    <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" DataSourceID="SqlSchematype"
                                        DataTextField="SchemaName" DataValueField="ID" onchange="CmbScheme_ValueChange()">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8">

                                    <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document Number">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>

                                    <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()" Enabled="false">
                                    </asp:TextBox>

                                    <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                    <%-- <dxe:ASPxTextBox ID="txt_PLQuoteNo" runat="server" TabIndex="2" Width="100%">
                                            </dxe:ASPxTextBox>--%>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>

                                    <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLQuoteDate" Width="100%">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                        <ClientSideEvents DateChanged="function(s, e) { GetIndentREquiNo(e)}" />
                                    </dxe:ASPxDateEdit>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" DataSourceID="DS_Branch" Enabled="false"
                                        DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" onchange="ddlBranch_ChangeIndex()">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8">

                                    <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                    </dxe:ASPxLabel>
                                    <%--<span style="color: red;">*</span>--%>
                                    <%-- <asp:DropDownList ID="ddl_Vendor" runat="server" Width="100%"  DataSourceID="Sqlvendor"
                                                DataTextField="Name" DataValueField="cnt_internalId" onchange="ddl_Vendor_ValueChange()">
                                            </asp:DropDownList>--%>
                                    <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" ClientInstanceName="gridLookup" DataSourceID="dsCustomer"
                                        KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">
                                        <Columns>
                                            <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Width="150" Settings-AutoFilterCondition="Contains">
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" Width="150" Settings-AutoFilterCondition="Contains" />

                                            <dxe:GridViewDataColumn FieldName="cnt_internalid" Visible="false" VisibleIndex="2" Settings-AllowAutoFilter="False" Width="150">
                                                <Settings AllowAutoFilter="False"></Settings>
                                            </dxe:GridViewDataColumn>
                                        </Columns>
                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                            <Templates>
                                                <StatusBar>
                                                    <table class="OptionsTable" style="float: right">
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </StatusBar>
                                            </Templates>

                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                            <%-- <Settings ShowFilterRow="True" ShowStatusBar="Visible" />--%>

                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                                        </GridViewProperties>
                                        <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" />
                                        <ClearButton DisplayMode="Auto">
                                        </ClearButton>
                                    </dxe:ASPxGridLookup>
                                    <span id="MandatorysCustomer" class="customerno pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-2 lblmTop8">

                                    <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px" ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                    </dxe:ASPxComboBox>
                                    <%--<asp:DropDownList ID="ddl_ContactPerson" runat="server" TabIndex="6" Width="100%"></asp:DropDownList>--%>
                                </div>
                                <div class="col-md-2 lblmTop8" style="display: none">
                                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                    </dxe:ASPxLabel>
                                    <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" DataSourceID="DS_SalesAgent" DataTextField="Name" DataValueField="cnt_id">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Party Invoice No.">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ID="txtPartyInvoice" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Party Invoice Date">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxDateEdit ID="dt_PartyDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLPartyDate" Width="100%">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select Purchase Order">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">
                                                <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" ClientInstanceName="gridquotationLookup"
                                                    OnDataBinding="lookup_quotation_DataBinding"
                                                    KeyFieldName="PurchaseOrder_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">

                                                    <Columns>
                                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                        <dxe:GridViewDataColumn FieldName="PurchaseOrder_Number" Visible="true" VisibleIndex="1" Caption="Document No." Width="150" Settings-AutoFilterCondition="Contains" />
                                                        <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Posting Date" Width="150" Settings-AutoFilterCondition="Contains" />
                                                        <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Vendor Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="ReferenceName" Visible="true" VisibleIndex="4" Caption="Reference" Width="150" Settings-AutoFilterCondition="Contains">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="BranchName" Visible="true" VisibleIndex="5" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataColumn>
                                                    </Columns>
                                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                        <Templates>
                                                            <StatusBar>
                                                                <table class="OptionsTable" style="float: right">
                                                                    <tr>
                                                                        <td>
                                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />
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
                                                    <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" />
                                                </dxe:ASPxGridLookup>

                                            </dxe:PanelContent>
                                        </PanelCollection>
                                        <ClientSideEvents EndCallback="componentEndCallBack" />
                                    </dxe:ASPxCallbackPanel>

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
                                                <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                                                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                                                    Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                                    <SettingsPager Visible="false"></SettingsPager>
                                                    <Columns>
                                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number">
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                                            <PropertiesTextEdit>
                                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                            </PropertiesTextEdit>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="ComponentDetailsID" ReadOnly="true" Caption="ComponentDetailsID" Width="0">
                                                        </dxe:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsDataSecurity AllowEdit="true" />
                                                </dxe:ASPxGridView>
                                                <div class="text-center">
                                                    <asp:Button ID="Button2" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />
                                                </div>
                                            </dxe:PopupControlContentControl>
                                        </ContentCollection>
                                        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                                        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                    </dxe:ASPxPopupControl>
                                    <%--<asp:DropDownList ID="ddl_IndentRequisitionNo" runat="server" Width="100%"  DataSourceID="SqlIndentRequisitionNo" onchange="IndentRequisitionNo_ValueChange()"
                                                DataTextField="Indent_RequisitionNumber" DataValueField="Indent_Id">
                                            </asp:DropDownList>--%>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Purchase Date">
                                    </dxe:ASPxLabel>
                                    <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                        <%--      <dxe:ASPxDateEdit ID="txtDateIndentRequis" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" 
                                              ClientInstanceName="cIndentRequisDate"
                                                    Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>                                                 
                                                </dxe:ASPxDateEdit>--%>
                                        <asp:Label ID="lbl_MultipleDate" runat="server" Text="Multiple Select Quotation Dates" Style="display: none"></asp:Label>

                                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <dxe:ASPxTextBox ID="dt_Quotation" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="cPLQADate">
                                                    </dxe:ASPxTextBox>

                                                    <dxe:ASPxDateEdit ID="txtDateIndentRequis" runat="server" Enabled="false" Visible="false" EditFormat="Custom" ClientInstanceName="cIndentRequisDate" Width="100%">
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
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                    </dxe:ASPxLabel>
                                    <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%"
                                        DataSourceID="SqlCurrency" DataValueField="Currency_ID"
                                        DataTextField="Currency_AlphaCode" onchange="ddl_Currency_Rate_Change()">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate">
                                        <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                        <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="col-md-2 lblmTop8">

                                    <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                    </dxe:ASPxLabel>

                                    <%--<asp:DropDownList ID="ddl_AmountAre" runat="server"  Width="100%" DataSourceID="DS_AmountAre"
                                             DataTextField="taxGrp_Description"  DataValueField="taxGrp_Id"    onchange="ddl_AmountAre_valueChange()">
                                            </asp:DropDownList>--%>
                                    <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" SelectedIndex="0" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                        <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" />
                                    </dxe:ASPxComboBox>
                                </div>
                                <div class="col-md-2 lblmTop8  hide" style="margin-bottom: 5px">

                                    <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" Width="100%" OnCallback="ddl_VatGstCst_Callback">
                                        <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" />
                                    </dxe:ASPxComboBox>

                                </div>
                                <div style="clear: both;"></div>
                                <div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="OrderDetails_Id" ClientInstanceName="grid" ID="grid"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCellEditorInitialize="grid_CellEditorInitialize"
                                        Settings-ShowFooter="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="160"
                                        OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                                        OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating"
                                        OnRowDeleting="Grid_RowDeleting" OnHtmlRowPrepared="grid_HtmlRowPrepared">
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30" VisibleIndex="0" Caption="">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="20px">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <%--<dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="gvColProduct" VisibleIndex="2" Width="15%" >
                                                        <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName" EnableCallbackMode="true">
                                                             <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsGotFocus" />
                                                        </PropertiesComboBox>
                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                            <%--Batch Product Popup Start--%>
                                            <dxe:GridViewDataTextColumn Caption="PO No" FieldName="PoNumber" ReadOnly="true" Width="130" VisibleIndex="2">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="150">
                                                <PropertiesButtonEdit>
                                                    <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" LostFocus="ProductsGotFocus" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                </PropertiesButtonEdit>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColProduct" Caption="hidden Field Id" VisibleIndex="22" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">

                                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <%--Batch Product Popup End--%>
                                            <dxe:GridViewDataTextColumn FieldName="gvColDiscription" Caption="Description" VisibleIndex="4" Width="18%">
                                                <PropertiesTextEdit>
                                                    <%--<ClientSideEvents GotFocus="ProductsGotFocus" />--%>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColQuantity" Caption="Quantity" VisibleIndex="5" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                    <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityProductsGotFocus" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColUOM" Caption="UOM" VisibleIndex="6" Width="4%" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn Width="4%" VisibleIndex="7" Caption="Stock">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>

                                            <dxe:GridViewDataTextColumn FieldName="gvColStockUOM" Caption="Stk UOM" VisibleIndex="14" Width="0">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColStockPurchasePrice" Caption="Price" VisibleIndex="8" Width="7%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="SalePriceTextChange" GotFocus="SalePriceTextFocus" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <%-- <dxe:GridViewDataTextColumn FieldName="gvColDiscount" Caption="Disc.%" VisibleIndex="9" Width="4%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountTextFocus" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>--%>

                                            <dxe:GridViewDataSpinEditColumn FieldName="gvColDiscount" Caption="Disc(%)" VisibleIndex="9" Width="4%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" Style-HorizontalAlign="Right">
                                                    <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                    <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountTextFocus" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesSpinEdit>
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataSpinEditColumn>

                                            <dxe:GridViewDataTextColumn FieldName="gvColAmount" Caption="Amount" VisibleIndex="10" Width="9%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                <PropertiesTextEdit>
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="AmountTextChange" GotFocus="AmountTextFocus" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <%-- <dxe:GridViewDataTextColumn FieldName="gvColTaxAmount" Caption="Tax Amount" VisibleIndex="12" Width="6%">
                                                        <PropertiesTextEdit>
                                                              <MaskSettings Mask="&lt;0..999999999g&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>--%>
                                            <dxe:GridViewDataButtonEditColumn FieldName="gvColTaxAmount" Caption="Charges" VisibleIndex="11" Width="9%" HeaderStyle-HorizontalAlign="Right">

                                                <PropertiesButtonEdit Style-HorizontalAlign="Right">

                                                    <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                </PropertiesButtonEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColTotalAmountINR" Caption="Net Amount" VisibleIndex="12" Width="9%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="5%" VisibleIndex="13" Caption="Add New">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton ID="CustomAddNewRow" Image-Url="/assests/images/add.png" Text=" ">
                                                    </dxe:GridViewCommandColumnCustomButton>

                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColStockQty" Caption="Stk Qty" VisibleIndex="15" Width="0">
                                                <PropertiesTextEdit>
                                                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="16" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="17" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="19" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="IsLinkedProduct" Caption="IsLinkedProduct" VisibleIndex="20" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="ComponentID" VisibleIndex="21" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <%--<dxe:GridViewDataTextColumn Caption="Purchase Order No" CellStyle-CssClass="hide" FilterCellStyle-CssClass="hide" EditCellStyle-CssClass="hide" 
                                                        EditFormCaptionStyle-CssClass="hide" FooterCellStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" 
                                                        FieldName="Quotation_No" Width="0" HeaderStyle-CssClass="hide" VisibleIndex="15">
                                                    <PropertiesTextEdit NullTextStyle-CssClass="hide" ReadOnlyStyle-CssClass="hide" Style-CssClass="hide">

                                                        <NullTextStyle CssClass="hide"></NullTextStyle>

                                                        <ReadOnlyStyle CssClass="hide"></ReadOnlyStyle>

                                                        <Style CssClass="hide"></Style>

                                                    </PropertiesTextEdit>
                                                    <HeaderStyle CssClass="hide" />
                                                    <CellStyle CssClass="hide">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>--%>
                                        </Columns>
                                        <%--      Init="OnInit"BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing"
                                                    CustomButtonClick="OnCustomButtonClick" EndCallback="OnEndCallback" --%>
                                        <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                        </SettingsEditing>

                                    </dxe:ASPxGridView>
                                </div>
                                <div style="clear: both;">
                                    <br />
                                    <div style="display: none;">
                                        <dxe:ASPxLabel ID="txt_Charges" runat="server" Text="0.00" ClientInstanceName="ctxt_Charges" />
                                        <dxe:ASPxLabel ID="txt_cInvValue" runat="server" Text="0.00" ClientInstanceName="cInvValue" />
                                    </div>
                                </div>

                                <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                    <ul>
                                        <li class="clsbnrLblTaxableAmt">
                                            <div class="horizontallblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="bnrLblTotalQty" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txt_TotalQty" runat="server" Text="0.00" ClientInstanceName="cTotalQty" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="clsbnrLblTaxableAmt">
                                            <div class="horizontallblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="bnrLblTaxableAmt" runat="server" Text="Taxable Amount" ClientInstanceName="cbnrLblTaxableAmt" />
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txt_TaxableAmtval" runat="server" Text="0.00" ClientInstanceName="cTaxableAmtval" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="clsbnrLblTaxAmt">
                                            <div class="horizontallblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="bnrLblTaxAmt" runat="server" Text="Tax & Charges" ClientInstanceName="cbnrLblTaxAmt" />
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txt_TaxAmtval" runat="server" Text="0.00" ClientInstanceName="cTaxAmtval" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="clsbnrLblTaxAmt">
                                            <div class="horizontallblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="bnrLblOtherTaxAmt" runat="server" Text="Other Charges" ClientInstanceName="cbnrLblOtherTaxAmt" />
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txt_OtherTaxAmtval" runat="server" Text="0.00" ClientInstanceName="cOtherTaxAmtval" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="clsbnrLblInvVal">
                                            <div class="horizontallblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="bnrLblInvVal" runat="server" Text="Total Amount" ClientInstanceName="cbnrLblInvVal" />
                                                            </td>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txt_TotalAmt" runat="server" Text="0.00" ClientInstanceName="cTotalAmt" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                    </ul>
                                </div>

                                <div class="col-md-12" style="padding-top: 15px;">
                                    <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>

                                    <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                        <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btn_SaveRecordsExit" ClientInstanceName="cbtn_SaveRecordsExit" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                        <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecordsUDF" runat="server" AutoPostBack="False" Text="UDF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                        <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                        <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                    </dxe:ASPxButton>

                                    <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                    <asp:HiddenField ID="hfControlData" runat="server" />

                                    <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                    <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                    <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="PC" />

                                    <asp:Label ID="lbl_IsTagged" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                    <%-- <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[A]ttachment(s)" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                               <%-- <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />--%>
                                    <%--</dxe:ASPxButton>--%>

                                    <asp:CheckBox ID="chkmail" runat="server" Text="Send Mail" />

                                </div>
                            </div>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="[B]illing/Shipping" Text="Our Billing/Shipping">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <%-- <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server" ContentUrl="AddArea_PopUp.aspx"
                                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                                Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                <ContentCollection>
                                    <dxe:PopupControlContentControl runat="server">
                                    </dxe:PopupControlContentControl>
                                </ContentCollection>
                                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                            </dxe:ASPxPopupControl>--%>

                            <ucBS:BillingShippingControl runat="server" ID="BillingShippingControl" />


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

    <%--Batch Product Popup Start--%>

    <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
        Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
        <HeaderTemplate>
            <span>Select Product</span>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By Product Name</strong></label>
                <span style="color: red;">[Press ESC key to Cancel]</span>
                <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                    KeyFieldName="Products_ID" Width="870" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                    <Columns>
                        <dxe:GridViewDataColumn FieldName="Products_Description" Caption="Name" Width="360">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="0">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="230">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="130">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <%--<dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>--%>
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

    <%-- Vendor List --%>

    <asp:SqlDataSource runat="server" ID="dsCustomer" 
        SelectCommand="prc_Purchasechallan_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateVendorsDetailByBranch" />
            <asp:ControlParameter DefaultValue="0" Name="branchid" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddl_Branch" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

    <%-- Vendor List --%>

    <asp:SqlDataSource runat="server" ID="ProductDataSource" 
        SelectCommand="prc_Purchasechallan_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
            <asp:SessionParameter Name="campany_Id" SessionField="LastCompany1" Type="String" />
            <asp:SessionParameter Type="String" Name="FinYear" SessionField="LastFinYear1" />
            <asp:ControlParameter DefaultValue="Y" Name="InventoryType" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddlInventory" PropertyName="SelectedValue" />
            <%--  <asp:Parameter Type="String" name="FinYear" DefaultValue="<%"Session["LastFinYear"]"%>"  />--%>
        </SelectParameters>
    </asp:SqlDataSource>

    <%--Batch Product Popup End--%>
    <dxe:ASPxPopupControl ID="ASPxPopupControl3" runat="server" ClientInstanceName="cPopup_WarehousePCPC"
        Width="900px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div class="">
                        <div id="content-6" class="pull-right wrapHolder reverse content horizontal-images" style="width: 100%; margin-right: 0px; height: auto;">
                            <ul>
                                <li>
                                    <div class="lblHolder">
                                        <table>
                                            <tr>
                                                <td>Selected Unit</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblbranchName" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </div>
                                </li>
                                <li>
                                    <div class="lblHolder">
                                        <table>
                                            <tr>
                                                <td>Selected Product</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblpro" runat="server"></asp:Label></td>
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
                                                    <asp:Label ID="lblAvailableStk" runat="server" Text="0.0"></asp:Label>
                                                    <asp:Label ID="lblAvailableStkunit" runat="server" Text="0.0"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </li>
                                <li>
                                    <div class="lblHolder">
                                        <table>
                                            <tr>
                                                <td>Entered Stock</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblopeningstock" runat="server" Text="0.0000"></asp:Label>
                                                    <asp:Label ID="lblopeningstockUnit" runat="server" Text="0.0000"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </li>


                            </ul>
                        </div>

                    </div>


                    <div class="clear">
                        <br />
                    </div>
                    <div class="clearfix">
                        <div class="row manAb">
                            <div class="blockone">
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatorCmbWarehousetxt">Warehouse</span>
                                    </div>
                                    <div class="Left_Content relative" style="">
                                        <%-- <dxe:ASPxTextBox ID="txtwarehousname" runat="server" Width="80%" ClientInstanceName="ctxtwarehousname" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                            TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                            <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange(s)}" EndCallback="function(s,e){endcallcmware(s)}"></ClientSideEvents>

                                        </dxe:ASPxComboBox>
                                        <span id="RequiredFieldValidatorCmbWarehouse" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatorCmbWarehouseQuantity">Quantity</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtqnty" runat="server" Width="100%" ClientInstanceName="ctxtqnty" HorizontalAlign="Left" Font-Size="12px">
                                            <MaskSettings Mask="<0..999999999>.<0..9999>" IncludeLiterals="DecimalSymbol" />
                                            <ClientSideEvents TextChanged="function(s,e){changedqnty(s)}" LostFocus="function(s,e){Setenterfocuse(s)}" KeyPress="function(s, e) {Keypressevt();}" />
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtwareqntity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="blocktwo">
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtbatchtxt">Batch</span>
                                    </div>
                                    <div class="Left_Content relative" style="">
                                        <dxe:ASPxTextBox ID="txtbatch" runat="server" Width="100%" ClientInstanceName="ctxtbatch" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                            <ClientSideEvents TextChanged="function(s,e){chnagedbtach(s)}" KeyPress="function(s, e) {Keypressevt();}" />
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtbatch" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3 blocktwoqntity">
                                    <div>
                                        <span id="RequiredFieldValidatorbatchQuantity">Quantity</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="batchqnty" runat="server" Width="100%" ClientInstanceName="ctxtbatchqnty" HorizontalAlign="Left" Font-Size="12px">
                                            <MaskSettings Mask="<0..999999999>.<0..9999>" IncludeLiterals="DecimalSymbol" />
                                            <ClientSideEvents TextChanged="function(s,e){changedqntybatch(s)}" />
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtbatchqntity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtbatchtxtmkgdate">Manufacture Date</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <%--<dxe:ASPxTextBox ID="txtmkgdate" runat="server" Width="80%" ClientInstanceName="ctxtmkgdate" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxDateEdit ID="txtmkgdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtmkgdate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>

                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtbatchtxtexpdate">Expiry Date</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <%-- <dxe:ASPxTextBox ID="txtexpirdate" runat="server" Width="80%" ClientInstanceName="ctxtexpirdate" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxDateEdit ID="txtexpirdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtexpirdate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>

                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="blockthree">
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtserialtxt">Serial No</span>
                                    </div>
                                    <div class="Left_Content relative" style="">
                                        <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="100%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                            <ClientSideEvents KeyPress="function(s, e) {Keypressevt();}" />
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtserial" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div>
                                </div>
                                <div class=" clearfix" style="padding-top: 11px;">
                                    <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary pull-left">
                                        <ClientSideEvents Click="function(s, e) {SaveWarehouse();}" />
                                    </dxe:ASPxButton>

                                    <dxe:ASPxButton ID="ASPxButton6" ClientInstanceName="cbtnrefreshWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Clear Entries" CssClass="btn btn-primary pull-left">
                                        <ClientSideEvents Click="function(s, e) {Clraear();}" />
                                    </dxe:ASPxButton>

                                </div>
                            </div>

                        </div>
                        <br />


                        <div class="clearfix">
                            <dxe:ASPxGridView ID="GrdWarehousePC" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False" SettingsBehavior-AllowSort="false"
                                Width="100%" ClientInstanceName="cGrdWarehousePC" OnCustomCallback="GrdWarehousePC_CustomCallback" OnDataBinding="GrdWarehousePC_DataBinding">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="viewWarehouseName"
                                        VisibleIndex="0">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Batch Number" FieldName="viewBatchNo"
                                        VisibleIndex="2">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataDateColumn Caption="Manufacture Date" FieldName="viewMFGDate"
                                        VisibleIndex="2">
                                        <Settings AllowHeaderFilter="False" />
                                        <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy"></PropertiesDateEdit>
                                    </dxe:GridViewDataDateColumn>

                                    <dxe:GridViewDataDateColumn Caption="Expiry Date" FieldName="viewExpiryDate"
                                        VisibleIndex="2">
                                        <Settings AllowHeaderFilter="False" />
                                        <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy"></PropertiesDateEdit>
                                    </dxe:GridViewDataDateColumn>
                                    <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="viewQuantity"
                                        VisibleIndex="3">
                                        <Settings ShowInFilterControl="False" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                        VisibleIndex="5">
                                        <Settings ShowInFilterControl="False" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="viewSerialNo"
                                        VisibleIndex="4">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Action" FieldName="SrlNo" CellStyle-VerticalAlign="Middle" VisibleIndex="6" CellStyle-HorizontalAlign="Center" Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False" Width="100px">
                                        <EditFormSettings Visible="False" />
                                        <DataItemTemplate>
                                            <a href="javascript:void(0);" onclick="UpdateWarehousebatchserial(<%#Eval("SrlNo")%>,'<%#Eval("WarehouseID")%>','<%#Eval("BatchNo")%>','<%#Eval("SerialNo")%>','<%#Eval("isnew")%>','<%#Eval("viewQuantity")%>','<%#Eval("Quantity")%>')" title="update Details" class="pad">
                                                <img src="../../../assests/images/Edit.png" />
                                            </a>
                                            <a href="javascript:void(0);" onclick="DeleteWarehousebatchserial(<%#Eval("SrlNo")%>,'<%#Eval("BatchWarehouseID")%>','<%#Eval("viewQuantity")%>',<%#Eval("Quantity")%>,'<%#Eval("WarehouseID")%>','<%#Eval("BatchNo")%>')" title="delete Details" class="pad">
                                                <img src="../../../assests/images/crs.png" />
                                            </a>
                                        </DataItemTemplate>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <ClientSideEvents EndCallback="function(s,e) { cGrdWarehousePCShowError(s.cpInsertError);}" />
                                <%-- <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>--ShowFilterRow="true" ShowFilterRowMenu="true" --%>
                                <SettingsPager Mode="ShowAllRecords" />
                                <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" VerticalScrollBarMode="Visible" VerticalScrollableHeight="190" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                            </dxe:ASPxGridView>
                        </div>
                        <br />
                        <div class="Center_Content" style="">
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="OK" AccessKey="S" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {SaveWarehouseAll();}" />
                            </dxe:ASPxButton>


                        </div>
                    </div>
                    <%-- <div class="text-center">
                        <table class="pull-right">
                            <tr>
                                <td style="padding-right: 15px"><strong>Total</strong></td>
                                <td>
                                    <dxe:ASPxTextBox ID="ASPxTextBox3" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>


    <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdfProductIDPC" runat="server" />
    <asp:HiddenField ID="hdfstockidPC" runat="server" />
    <asp:HiddenField ID="hdfopeningstockPC" runat="server" />
    <asp:HiddenField ID="hdbranchIDPC" runat="server" />
    <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />
    <asp:HiddenField ID="hiddenOnGridBind" runat="server" Value="0" />
    <asp:HiddenField ID="hdnProductQuantity" runat="server" />

    <asp:HiddenField ID="hdniswarehouse" runat="server" />
    <asp:HiddenField ID="hdnisbatch" runat="server" />
    <asp:HiddenField ID="hdnisserial" runat="server" />
    <asp:HiddenField ID="hdndefaultID" runat="server" />

    <asp:HiddenField ID="hdnoldrowcount" runat="server" Value="0" />

    <asp:HiddenField ID="hdntotalqntyPC" runat="server" Value="0" />

    <asp:HiddenField ID="hdnoldwarehousname" runat="server" />
    <asp:HiddenField ID="hdnoldbatchno" runat="server" />
    <asp:HiddenField ID="hidencountforserial" runat="server" />
    <asp:HiddenField ID="hdnbatchchanged" runat="server" Value="0" />

    <asp:HiddenField ID="hdnrate" runat="server" Value="0" />
    <asp:HiddenField ID="hdnvalue" runat="server" Value="0" />

    <asp:HiddenField ID="oldhdnoldwarehousname" runat="server" Value="0" />

    <asp:HiddenField ID="oldhidencountforserial" runat="server" Value="0" />
    <asp:HiddenField ID="oldhdnbatchchanged" runat="server" Value="0" />
    <asp:HiddenField ID="hdnstrUOM" runat="server" />
    <asp:HiddenField ID="hdnenterdopenqnty" runat="server" />
    <asp:HiddenField ID="hdnnewenterqntity" runat="server" />

    <asp:HiddenField ID="hdnisoldupdate" runat="server" />
    <asp:HiddenField ID="hdncurrentslno" runat="server" />
    <asp:HiddenField ID="oldopeningqntity" runat="server" Value="0" />
    <asp:HiddenField ID="hdnisedited" runat="server" />

    <asp:HiddenField ID="hdnisnewupdate" runat="server" />

    <asp:HiddenField ID="hdnisviewqntityhas" runat="server" />
    <asp:HiddenField ID="hdndeleteqnity" runat="server" Value="0" />
    <asp:HiddenField ID="hdnisolddeleted" runat="server" Value="false" />

    <asp:HiddenField ID="hdnisreduing" runat="server" Value="false" />
    <asp:HiddenField ID="hdnoutstock" runat="server" Value="0" />

    <asp:HiddenField ID="hdnpcslno" runat="server" Value="0" />
    <asp:HiddenField ID="IsPOTagged" runat="server" Value="false" />

    <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
    <asp:HiddenField ID="hdfIsDelete" runat="server" />
    <asp:HiddenField ID="hdnPageStatus" runat="server" />
    <asp:HiddenField ID="hdfProductID" runat="server" />
    <asp:HiddenField ID="hdfProductType" runat="server" />
    <asp:HiddenField ID="hdfProductSerialID" runat="server" />
    <asp:HiddenField ID="hdnRefreshType" runat="server" />
    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField ID="hdnPOnumber" runat="server" Value="0" />


    <asp:SqlDataSource ID="SqlSchematype" runat="server" 
        SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) From tbl_master_Idschema  Where TYPE_ID='18' and financial_year_id IN(select FinYear_ID from Master_FinYear where FinYear_Code=@year) and Branch in (select s FROM dbo.GetSplit(',',@userbranchHierarchy)) and comapanyInt=@company)) as X Order By ID ASC">
        <SelectParameters>
            <asp:SessionParameter Name="userbranchHierarchy" SessionField="userbranchHierarchy" Type="string" />
            <asp:SessionParameter Name="company" SessionField="LastCompany1" Type="string" />
            <asp:SessionParameter Name="year" SessionField="LastFinYear1" Type="string" />

        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlIndentRequisitionNo" runat="server" 
        SelectCommand="(Select '0' as Indent_Id,'Select' as Indent_RequisitionNumber) Union
            (select Indent_Id,Indent_RequisitionNumber from tbl_trans_Indent)"></asp:SqlDataSource>
    <asp:SqlDataSource ID="Sqlvendor" runat="server" 
        SelectCommand="select '0' as cnt_internalId,'Select' as Name 
            union select cnt_internalId,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name 
            from tbl_master_contact  where cnt_contacttype='DV'"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCurrency" runat="server"
        SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_AlphaCode"></asp:SqlDataSource>
    <asp:SqlDataSource ID="DS_Branch" runat="server" 
        SelectCommand=""></asp:SqlDataSource>
    <asp:SqlDataSource ID="DS_SalesAgent" runat="server" 
        SelectCommand="select '0' as cnt_id,'Select' as Name
            union select cnt_id,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name from tbl_master_contact  where Substring(cnt_internalId,1,2)='AG'"></asp:SqlDataSource>
    <asp:SqlDataSource ID="DS_AmountAre" runat="server" 
        SelectCommand="select '0'as taxGrp_Id,'Select'as taxGrp_Description
            union select taxGrp_Id,taxGrp_Description from tbl_master_taxgrouptype order by taxGrp_Id"></asp:SqlDataSource>


    <asp:SqlDataSource ID="CountrySelect" runat="server" 
        SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>
    <asp:SqlDataSource ID="StateSelect" runat="server" 
        SelectCommand="SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">
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

    <div class="PopUpArea">
        <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
        <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
        <%--ChargesTax--%>
        <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
            Width="900px" Height="300px" HeaderText="GRN Taxes" PopupHorizontalAlign="WindowCenter"
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
                                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableGross"></dxe:ASPxLabel>
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
                                                                <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableNet"></dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>
                                                            <dxe:ASPxLabel ID="txtProductNetAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductNetAmount"></dxe:ASPxLabel>
                                                        </strong>
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
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="PercentageTextChange" />
                                            <ClientSideEvents />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
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
                                <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AccessKey="X" AutoPostBack="False" Text="Ok" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                    <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                    <ClientSideEvents Click="function(s, e) {cPopup_Taxes.Hide();}" />
                                </dxe:ASPxButton>
                            </div>
                        </div>

                        <div class="col-sm-9">
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-right: 30px; width: 114px"><strong>Total Charges</strong></td>
                                    <td>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                <MaskSettings Mask="<-999999999..99999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                            </dxe:ASPxTextBox>
                                        </div>

                                    </td>
                                    <td style="padding-right: 30px; padding-left: 5px; width: 114px"><strong>Total Amount</strong></td>
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
    </div>
    <%--debjyoti 22-12-2016--%>
    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
        Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <HeaderTemplate>
            <span>UDF</span>
            <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                                           popup.Hide();
                                                       }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="IsUdfpresent" />
    <asp:HiddenField runat="server" ID="Keyval_internalId" />
    <%--End debjyoti 22-12-2016--%>

    <%--InlineTax--%>

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
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
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

                    <div class="col-sm-3 gstGrossAmount hide">
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
                                            <dxe:ASPxLabel ID="lblProdNetAmt" runat="server" Text="" ClientInstanceName="clblProdNetAmt"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-2 gstNetAmount hide">
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
                                <ClientSideEvents EndCallback="cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />

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
                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                <asp:Button ID="Button3" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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

    <%--debjyoti 22-12-2016--%>
    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
        Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <asp:HiddenField runat="server" ID="HiddenField1" />
    <asp:HiddenField runat="server" ID="HiddenField2" />

    <%--End debjyoti 22-12-2016--%>
    <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <%--Debu Section End--%>

    <script type="text/javascript">


        function Keypressevt() {

            if (event.keyCode == 13) {
                SaveWarehouse();
                return false;
            }
        }


        function DeleteWarehousebatchserial(SrlNo, BatchWarehouseID, viewQuantity, Quantity, WarehouseID, BatchNo) {
            //alert(viewQuantity);
            var IsSerial = $('#hdnisserial').val();
            if (IsSerial == "true" && viewQuantity != "1.0000" && viewQuantity != "1.0" && viewQuantity != "") {
                jAlert("Cannot Proceed. You have to delete subsequent data first before delete this data.");
            } else {
                if (BatchWarehouseID == "" || BatchWarehouseID == "0") {

                    $('#<%=hdnisolddeleted.ClientID %>').val("false");
                    if (SrlNo != "") {


                        cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
                    }

                } else {

                    $('#<%=hdnisolddeleted.ClientID %>').val("true");
                    if (SrlNo != "") {

                        cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
                    }
                }
            }



        }

        function Setenterfocuse(s) {

           <%-- var Isbatch = $('#hdnisbatch').val();
            var IsSerial = $('#hdnisserial').val();
            //alert(Isbatch);
            if (Isbatch == "true") {
                ctxtbatch.Focus();
                document.getElementById("<%=txtbatch.ClientID%>").focus();
            } else if (IsSerial == "true") {
                ctxtserial.Focus();
            }--%>
        }

        function UpdateWarehousebatchserial(SrlNo, WarehouseID, BatchNo, SerialNo, isnew, viewQuantity, Quantity) {

            var Isbatch = $('#hdnisbatch').val();

            if (isnew == "old" || isnew == "Updated") {

                $('#<%=hdnisoldupdate.ClientID %>').val("true");
                $('#<%=hdncurrentslno.ClientID %>').val("");
                cCmbWarehouse.SetValue(WarehouseID);
                if (Quantity != null && Quantity != "" && Isbatch != "true") {
                    ctxtqnty.SetText(Quantity);
                } else {
                    ctxtqnty.SetText(viewQuantity);
                }
                var IsSerial = $('#hdnisserial').val();

                if (IsSerial == "true") {

                    if (viewQuantity == "") {
                        ctxtbatch.SetEnabled(false);
                        cCmbWarehouse.SetEnabled(false);
                        ctxtqnty.SetEnabled(false);
                        ctxtserial.Focus();
                    } else {
                        ctxtbatch.SetEnabled(true);
                        cCmbWarehouse.SetEnabled(true);
                        ctxtqnty.SetEnabled(true);
                        ctxtserial.Focus();
                    }

                }
                else {
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);
                    ctxtbatch.Focus();
                }
                // ctxtqnty.SetEnabled(false);

                ctxtbatchqnty.SetText(Quantity);
                //ctxtbatchqnty.SetEnabled(false);
                ctxtbatch.SetText(BatchNo);
                ctxtserial.SetText(SerialNo);

                if (viewQuantity == "") {
                    ctxtbatch.SetEnabled(false);
                    cCmbWarehouse.SetEnabled(false);
                    $('#<%=hdnisviewqntityhas.ClientID %>').val("true");
                } else {
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                }

                var hdniswarehouse = $('#hdniswarehouse').val();


                if (hdniswarehouse != "true" && Isbatch == "true") {
                    ctxtbatchqnty.SetText(viewQuantity);
                    ctxtbatchqnty.Focus();

                } else {
                    ctxtqnty.Focus();
                }
                $('#<%=hdncurrentslno.ClientID %>').val(SrlNo);

            } else {

                $('#<%=hdnisoldupdate.ClientID %>').val("false");

                ctxtqnty.SetText("0.0");
                ctxtqnty.SetEnabled(true);

                ctxtbatchqnty.SetText("0.0");
                ctxtserial.SetText("");
                ctxtbatchqnty.SetText("");
                $('#<%=hdncurrentslno.ClientID %>').val("");

                $('#<%=hdnisnewupdate.ClientID %>').val("true");
                $('#<%=hdncurrentslno.ClientID %>').val("");
                cCmbWarehouse.SetValue(WarehouseID);
                if (Quantity != null && Quantity != "" && Isbatch != "true") {
                    ctxtqnty.SetText(Quantity);
                } else {
                    ctxtqnty.SetText(viewQuantity);
                }
                var IsSerial = $('#hdnisserial').val();
                if (IsSerial == "true") {

                    if (viewQuantity == "") {
                        ctxtbatch.SetEnabled(false);
                        cCmbWarehouse.SetEnabled(false);
                        ctxtqnty.SetEnabled(false);
                        $('#<%=hdnisviewqntityhas.ClientID %>').val("true");
                        ctxtserial.Focus();
                    } else {
                        ctxtbatch.SetEnabled(true);
                        cCmbWarehouse.SetEnabled(true);
                        ctxtqnty.SetEnabled(true);
                        $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                        ctxtserial.Focus();
                    }

                } else {
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);
                    ctxtbatch.Focus();
                }
                // ctxtqnty.SetEnabled(false);

                ctxtbatchqnty.SetText(Quantity);
                //ctxtbatchqnty.SetEnabled(false);
                ctxtbatch.SetText(BatchNo);
                ctxtserial.SetText(SerialNo);

                if (viewQuantity == "") {
                    ctxtbatch.SetEnabled(false);
                    cCmbWarehouse.SetEnabled(false);
                } else {
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                }

                var hdniswarehouse = $('#hdniswarehouse').val();


                if (hdniswarehouse != "true" && Isbatch == "true") {
                    ctxtbatchqnty.SetText(viewQuantity);
                } else {
                    ctxtqnty.Focus();
                }

                $('#<%=hdncurrentslno.ClientID %>').val(SrlNo);

                //jAlert("Sorry, This is new entry you can not update. please click on 'Clear Entries' and Add again.");
            }
        }

        function changedqnty(s) {

            var qnty = s.GetText();
            var sum = $('#hdntotalqntyPC').val();

            sum = Number(Number(sum) + Number(qnty));
            //alert(sum);
            $('#<%=hdntotalqntyPC.ClientID %>').val(sum);
           <%-- document.getElementById("<%=txtbatch.ClientID%>").focus();
            var Isbatch = $('#hdnisbatch').val();
            var IsSerial = $('#hdnisserial').val();
            //alert(Isbatch);
            if (Isbatch == "true") {
                ctxtbatch.Focus();
                document.getElementById("<%=txtbatch.ClientID%>").focus();
            } else if (IsSerial == "true") {
                ctxtserial.Focus();
            }--%>
        }

        function endcallcmware(s) {

            if (cCmbWarehouse.cpstock != null) {

                //var ddd = cCmbWarehouse.cpstock + " " + $('#hdnstrUOM').val();
                var ddd = cCmbWarehouse.cpstock + " ";
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = ddd;
                cCmbWarehouse.cpstock = null;
            }
        }
        function changedqntybatch(s) {

            var qnty = s.GetText();
            var sum = $('#hdntotalqntyPC').val();
            sum = Number(Number(sum) + Number(qnty));
            //alert(sum);
            $('#<%=hdntotalqntyPC.ClientID %>').val(sum);

            //var Isbatch = $('#hdnisbatch').val();
            //var IsSerial = $('#hdnisserial').val();
            ////alert(Isbatch);
            //if (IsSerial == "true") {
            //    ctxtserial.Focus();
            //}

        }
        function chnagedbtach(s) {

            $('#<%=hdnoldbatchno.ClientID %>').val(s.GetText());
            $('#<%=hidencountforserial.ClientID %>').val(1);

            var sum = $('#hdnbatchchanged').val();
            sum = Number(Number(sum) + Number(1));

            $('#<%=hdnbatchchanged.ClientID %>').val(sum);
            //ctxtqnty.SetValue("0.0");
            //ctxtbatchqnty.SetValue("0.0");
            //ctxtmkgdate.SetDate = null;
            //txtexpirdate.SetDate = null;
            //ASPx.CalClearClick('txtmkgdate_DDD_C');
            //ASPx.CalClearClick('txtexpirdate_DDD_C');
            ctxtexpirdate.SetText("");
            ctxtmkgdate.SetText("");
        }

        function CmbWarehouse_ValueChange(s) {

            var ISupdate = $('#hdnisoldupdate').val();
            var isnewupdate = $('#hdnisnewupdate').val();

            $('#<%=hdnoldwarehousname.ClientID %>').val(s.GetText());

            if (ISupdate == "true" || isnewupdate == "true") {


            } else {
        <%--$('#<%=hidencountforserial.ClientID %>').val(1);
           
            $('#<%=hdnbatchchanged.ClientID %>').val("0");
            $('#<%=hidencountforserial.ClientID %>').val("1");--%>

                ctxtserial.SetValue("");
                //ctxtbatch.SetValue("");
                //ctxtmkgdate.SetDate = null;
                //txtexpirdate.SetDate = null;

                ctxtbatch.SetEnabled(true);
                ctxtexpirdate.SetEnabled(true);
                ctxtmkgdate.SetEnabled(true);

                //ctxtqnty.SetValue("0.0");
                //ctxtbatchqnty.SetValue("0.0");

                //cCmbWarehouse.PerformCallback('Bindstock');
                //ASPx.CalClearClick('txtmkgdate_DDD_C');
                //ASPx.CalClearClick('txtexpirdate_DDD_C');
                //ctxtexpirdate.SetText("");
                //ctxtmkgdate.SetText("");
            }


        }

        function Clraear() {
            ctxtbatch.SetValue("");

            ASPx.CalClearClick('txtmkgdate_DDD_C');
            ASPx.CalClearClick('txtexpirdate_DDD_C');
            $('#<%=hdnisoldupdate.ClientID %>').val("false");
            //ctxtmkgdate.SetDate = null;
            //txtexpirdate.SetDate = null;

            //ctxtexpirdate.SetText("");
            //ctxtmkgdate.SetText("");

            //ctxtmkgdate.CalClearClick('txtmkgdate_DDD_C');
            //ctxtexpirdate.CalClearClick('txtexpirdate_DDD_C');
            ctxtserial.SetValue("");
            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            $('#<%=hdntotalqntyPC.ClientID %>').val(0);
            $('#<%=hidencountforserial.ClientID %>').val(1);
            $('#<%=hdnbatchchanged.ClientID %>').val("0");
            var strProductID = $('#hdfProductIDPC').val();
            var stockids = $('#hdfstockidPC').val();
            var branchid = $('#hdbranchIDPC').val();
            var strProductName = $('#lblProductName').text();
            $('#<%=hdnisnewupdate.ClientID %>').val("false");
            ctxtbatch.SetEnabled(true);
            ctxtexpirdate.SetEnabled(true);
            ctxtmkgdate.SetEnabled(true);
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
            $('#<%=hdnisolddeleted.ClientID %>').val("false");
            ctxtqnty.SetEnabled(true);

            var existingqntity = $('#hdfopeningstockPC').val();
            var totaldeleteqnt = $('#hdndeleteqnity').val();

            var addqntity = Number(existingqntity) + Number(totaldeleteqnt);

            $('#<%=hdndeleteqnity.ClientID %>').val(0);
           <%-- $('#<%=hdfopeningstockPC.ClientID %>').val(addqntity);--%>



            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

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

            var WarehouseID = cCmbWarehouse.GetValue();
            var WarehouseName = cCmbWarehouse.GetText();

            var qnty = ctxtqnty.GetText();
            var IsSerial = $('#hdnisserial').val();
            //alert(qnty);

            if (qnty == "0.0000") {
                qnty = ctxtbatchqnty.GetText();
            }

            if (Number(qnty) % 1 != 0 && IsSerial == "true") {
                jAlert("Serial number is activated, Quantity should not contain decimals. ");
                return;
            }

            //alert(qnty);
            var BatchName = ctxtbatch.GetText();
            var SerialName = ctxtserial.GetText();
            var Isbatch = $('#hdnisbatch').val();

            var enterdqntity = $('#hdfopeningstockPC').val();

            var hdniswarehouse = $('#hdniswarehouse').val();

            var ISupdate = $('#hdnisoldupdate').val();
            var isnewupdate = $('#hdnisnewupdate').val();
            //alert(Isbatch + "/" + IsSerial);
            //alert(hdniswarehouse+"/"+WarehouseID);
            if (Isbatch == "true" && hdniswarehouse == "false") {
                qnty = ctxtbatchqnty.GetText();
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

                    qnty = ctxtbatchqnty.GetText();

                    if (qnty == "0.0000") {
                        //alert("Enter" + ctxtbatchqnty.GetText());

                        ctxtbatchqnty.Focus();
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

                        qnty = ctxtbatchqnty.GetText();

                        if (qnty = "0.0000") {
                            ctxtbatchqnty.Focus();
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

                        cCmbWarehouse.Focus();
                    }
                }
                //}

                //ctxtexpirdate.SetText("");
                //ctxtmkgdate.SetText("");
                return false;
            }
    }
    function SaveWarehouseAll() {
        //var openqnty = Number($('#hdfopeningstockPC').val());
        //var totalqnty = Number($('#hdntotalqntyPC').val());
        // if (totalqnty != openqnty) {

        //jAlert("Please make sure Opening quantity should not be greater and less than total INput quantity.");
        //} else {

        cGrdWarehousePC.PerformCallback('Saveall~');




        //}

    }

    function cGrdWarehousePCShowError(obj) {

        if (cGrdWarehousePC.cpdeletedata != null) {
            var existingqntity = $('#hdfopeningstockPC').val();
            var totaldeleteqnt = $('#hdndeleteqnity').val();

            var addqntity = Number(cGrdWarehousePC.cpdeletedata) + Number(existingqntity);
            var adddeleteqnty = Number(cGrdWarehousePC.cpdeletedata) + Number(totaldeleteqnt);

            $('#<%=hdndeleteqnity.ClientID %>').val(adddeleteqnty);
            <%--$('#<%=hdfopeningstockPC.ClientID %>').val(addqntity);--%>
            cGrdWarehousePC.cpdeletedata = null;
        }

        if (cGrdWarehousePC.cpdeletedatasubsequent != null) {
            jAlert(cGrdWarehousePC.cpdeletedatasubsequent);
            cGrdWarehousePC.cpdeletedatasubsequent = null;
        }
        if (cGrdWarehousePC.cpbatchinsertmssg != null) {
            ctxtbatch.SetText("");

            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            cGrdWarehousePC.cpbatchinsertmssg = null;
        }
        if (cGrdWarehousePC.cpupdateexistingdata != null) {
            $('#<%=hdnisoldupdate.ClientID %>').val("false");
            console.log("cpupdateexistingdata");
            ctxtqnty.SetText("0.0");
            ctxtbatch.SetText("");
            ctxtserial.SetValue("");
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            $('#<%=hdnisedited.ClientID %>').val("true");
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            ctxtserial.SetValue("");

            cGrdWarehousePC.cpupdateexistingdata = null;
        }
        if (cGrdWarehousePC.cpupdatenewdata != null) {

            console.log("cpupdatenewdata");
            $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
            $('#<%=hdnisnewupdate.ClientID %>').val("false");
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtqnty.SetText("0.0");
            ctxtbatch.SetText("");
            ctxtserial.SetValue("");

            $('#<%=hdnisedited.ClientID %>').val("true");

            cGrdWarehousePC.cpupdateexistingdata = null;
        }
        if (cGrdWarehousePC.cpserialuniquemssg != null) {

            jAlert(cGrdWarehousePC.cpserialuniquemssg, 'Alert', function () {
                ctxtserial.Focus();
            });

            cGrdWarehousePC.cpserialuniquemssg = null;

        }

        if (cGrdWarehousePC.cpupdatemssgserialsetdisblebatch != null) {
            ctxtbatch.SetEnabled(false);
            ctxtexpirdate.SetEnabled(false);
            ctxtmkgdate.SetEnabled(false);
            cGrdWarehousePC.cpupdatemssgserialsetdisblebatch = null;
        }
        if (cGrdWarehousePC.cpupdatemssgserialsetenablebatch != null) {
            ctxtbatch.SetEnabled(true);
            ctxtexpirdate.SetEnabled(true);
            ctxtmkgdate.SetEnabled(true);
            $('#<%=hidencountforserial.ClientID %>').val(1);

            $('#<%=hdnbatchchanged.ClientID %>').val("0");
            $('#<%=hidencountforserial.ClientID %>').val("1");
            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            ctxtbatch.SetText("");
            cGrdWarehousePC.cpupdatemssgserialsetenablebatch = null;
        }


        if (cGrdWarehousePC.cpproductname != null) {
            document.getElementById('<%=lblpro.ClientID %>').innerHTML = cGrdWarehousePC.cpproductname;
            cGrdWarehousePC.cpproductname = null;
        }

          <%--  if (cGrdWarehousePC.cpbranchqntity != null) {

                var qnty = cGrdWarehousePC.cpbranchqntity;
                var sum = $('#hdfopeningstockPC').val();
                sum = Number(Number(sum) + Number(qnty));
               
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = "0";
                cGrdWarehousePC.cpbranchqntity = null;
            }--%>

        if (cGrdWarehousePC.cpupdatemssg != null) {
            if (cGrdWarehousePC.cpupdatemssg == "Saved Successfully.") {
                $('#<%=hdntotalqntyPC.ClientID %>').val("0");
                $('#<%=hdnbatchchanged.ClientID %>').val("0");
                $('#<%=hidencountforserial.ClientID %>').val("1");
                ctxtqnty.SetValue("0.0000");
                ctxtbatchqnty.SetValue("0.0000");
                grid.batchEditApi.StartEdit(globalRowIndex, 10);

                parent.cPopup_WarehousePCPC.Hide();



                var hdnselectedbranch = $('#hdnselectedbranch').val();

                //cOpeningGrid.Enable = false;
                // parent.cOpeningGrid.PerformCallback("branchwise~" + hdnselectedbranch);
            } else {
                jAlert(cGrdWarehousePC.cpupdatemssg);
            }

            cGrdWarehousePC.cpupdatemssg = null;


        }
        if (cGrdWarehousePC.cpupdatemssgserial != null) {
            jAlert(cGrdWarehousePC.cpupdatemssgserial);
            cGrdWarehousePC.cpupdatemssgserial = null;
        }

        if (cGrdWarehousePC.cpinsertmssg != null) {
            $('#<%=hidencountforserial.ClientID %>').val(2);
            ctxtserial.SetValue("");
            ctxtserial.Focus();
            cGrdWarehousePC.cpinsertmssg = null;
        }
        if (cGrdWarehousePC.cpinsertmssgserial != null) {

            ctxtserial.SetValue("");
            ctxtserial.Focus();
            cGrdWarehousePC.cpinsertmssgserial = null;
        }


    }

    //Code for UDF Control 
    function OpenUdf() {
        if (document.getElementById('IsUdfpresent').value == '0') {
            jAlert("UDF not define.");
        }
        else {
            var keyVal = document.getElementById('Keyval_internalId').value;
            var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=PC&&KeyVal_InternalID=' + keyVal;
            popup.SetContentUrl(url);
            popup.Show();
        }
        return true;
    }
    </script>


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
                                        <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete" style='<%#Eval("IsOutStatus")%>'>
                                            <img src="../../../assests/images/Edit.png" /></a>
                                        &nbsp;
                                        <a href="javascript:void(0);" onclick="fn_Delete('<%# Container.KeyValue %>')" title="Delete" style='<%#Eval("IsOutStatus")%>'>
                                            <img src="/assests/images/crs.png" /></a>
                                        <a class="anchorclass" style='<%#Eval("IsOutStatusMsg")%>'>Already used</a>
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
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <%--Warehouse Details End--%>

    <div>
        <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
        <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
        <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
        <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>
</asp:Content>

