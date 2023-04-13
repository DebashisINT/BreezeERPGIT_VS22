<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-04-2023        2.0.37           Pallab              25817 :Add Undelivery Return page design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" EnableViewStateMac="false" CodeBehind="UndeliveryReturn.aspx.cs" Inherits="ERP.OMS.Management.Activities.UndeliveryReturn" %>

<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>--%>

<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <style type="text/css">
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

        #grid_DXMainTable > tbody > tr > td:last-child,
        #grid_DXMainTable > tbody > tr > td:last-child > div, #grid_DXMainTable > tbody > tr > td:nth-child(19),
        #grid_DXMainTable > tbody > tr > td:nth-child(3) {
            display: none !important;
        }

        .classout {
            text-transform: none !important;
        }
    </style>

    <script type="text/javascript">
        function GlobalBillingShippingEndCallBack() {
            //if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
            //    cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";

                var startDate = new Date();
                startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                var branchid = $('#ddl_Branch').val();
                if (gridquotationLookup.GetValue() != null) {


                    //  var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                    //  var key = cCustomerComboBox.GetValue();
                    var key = $('#<%=hdnCustomerId.ClientID %>').val();
                     if (key != null && key != '') {

                         cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);

                         var startDate = new Date();
                         startDate = tstartdate.GetDate().format('yyyy/MM/dd');

                         //20-10-2017  kaushik
                         //  cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                         //20-10-2017  kaushik
                         //  var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

                         //   if (key != null && key != '') {
                         // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                         //  }
                         grid.PerformCallback('GridBlank');

                         ccmbGstCstVat.PerformCallback();
                         ccmbGstCstVatcharge.PerformCallback();
                         ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                     }


                 }
                 else {

                     //  var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

                     //var key = cCustomerComboBox.GetValue();
                     var key = $('#<%=hdnCustomerId.ClientID %>').val();
                     if (key != null && key != '') {

                         cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
                         // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%');

                         //20-10-2017  kaushik
                         //   cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                         //20-10-2017  kaushik
                     }

                 }


             }
        



        function AfterSaveBillingShipiing(validate) {
            GetPurchaseForGstValue();
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


        function GetPurchaseForGstValue() {

            cddlPosGstUnReturn.ClearItems();
            if (cddlPosGstUnReturn.GetItemCount() == 0) {
                cddlPosGstUnReturn.AddItem(GetShippingStateName() + '[Shipping]', "S");
                cddlPosGstUnReturn.AddItem(GetBillingStateName() + '[Billing]', "B");
            }

            else if (cddlPosGstUnReturn.GetItemCount() > 2) {
                cddlPosGstUnReturn.ClearItems();
                //cddl_PosGstSalesOrder.RemoveItem(0);
                //cddl_PosGstSalesOrder.RemoveItem(0);
            }

            if (PosGstId == "" || PosGstId == null) {
                cddlPosGstUnReturn.SetValue("S");
            }
            else {
                cddlPosGstUnReturn.SetValue(PosGstId);
            }
        }

        var PosGstId = "";
        function PopulateUndeliveryReturnPosGst(e) {

            PosGstId = cddlPosGstUnReturn.GetValue();
            if (PosGstId == "S") {
                cddlPosGstUnReturn.SetValue("S");
            }
            else if (PosGstId == "B") {
                cddlPosGstUnReturn.SetValue("B");
            }
        }


    </script>

    <script>
        var isudf = '';
        $(document).ready(function () {

            if ($('#hdnPageStatus').val() == "update")
            {
                cddlPosGstUnReturn.SetEnabled(false);
                PopulateUndeliveryReturnPosGst();
                LoadCustomerBillingShippingAddress(GetObjectID('hdnCustomerId').value);
                LoadBranchAddressInEditMode($('#ddl_Branch').val());

            }


            $("#txtReasonforChange").on('change', function () {

                LoadingPanel.Hide();
            });


        });
        function AmtGotFocus() {
            globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            ProductGetTotalAmount = globalNetAmount;


        }
        function AmtTextChange(s, e) {


            var grossamt = grid.GetEditor('Amount').GetValue();
            var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(parseFloat(grossamt) + parseFloat(_TotalTaxAmt));

            //var _TotalAmount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

            if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
                grid.GetEditor('TaxAmount').SetValue(0);
                ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());



            }

            //TDSDetail();
        }

        //contactperson phone
        function acpContactPersonPhoneEndCall(s, e) {
            if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
                pageheaderContent.style.display = "block";
                $("#<%=divContactPhone.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = cacpContactPersonPhone.cpPhone;
                cacpContactPersonPhone.cpPhone = null;

            }
        }

        //contactperson phones
        function Onddl_VatGstCstEndCallback(s, e) {
            if (s.GetItemCount() == 1) {
                cddlVatGstCst.SetEnabled(false);
            }
        }

        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }

        function componentEndCallBack(s, e) {
            //  debugger;
            //  alert('hhhhhh');

            LoadingPanel.Hide();
            gridquotationLookup.gridView.Refresh();
            if (grid.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick();
            }

            if (cQuotationComponentPanel.cpDetails != null) {
                var details = cQuotationComponentPanel.cpDetails;
                cQuotationComponentPanel.cpDetails = null;

                var SpliteDetails = details.split("~");
                var Reference = SpliteDetails[0];
                var Currency_Id = (SpliteDetails[1] == "" || SpliteDetails[1] == null) ? "0" : SpliteDetails[1];
                var SalesmanId = (SpliteDetails[2] == "" || SpliteDetails[2] == null) ? "0" : SpliteDetails[2];
                //var ExpiryDate = SpliteDetails[3];
                var CurrencyRate = SpliteDetails[4];
                var Contact_person_id = SpliteDetails[5];
                var Tax_option = (SpliteDetails[6] == "" || SpliteDetails[6] == null) ? "1" : SpliteDetails[6];

                var Tax_Code = (SpliteDetails[7] == "" || SpliteDetails[7] == null) ? "0" : SpliteDetails[7];
                ctxt_Refference.SetValue(Reference);
                ctxt_Rate.SetValue(CurrencyRate);
                cddl_AmountAre.SetValue(Tax_option); if (Tax_option == 1) {

                    grid.GetEditor('TaxAmount').SetEnabled(true);
                    cddlVatGstCst.SetEnabled(false);

                    cddlVatGstCst.SetSelectedIndex(0);
                    cbtn_SaveRecords.SetVisible(true);
                    // grid.GetEditor('ProductID').Focus();
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 2);
                    }

                }
                else if (Tax_option == 2) {
                    grid.GetEditor('TaxAmount').SetEnabled(true);

                    cddlVatGstCst.SetEnabled(true);
                    cddlVatGstCst.PerformCallback('2');
                    cddlVatGstCst.Focus();
                    cbtn_SaveRecords.SetVisible(true);
                }
                else if (Tax_option == 3) {

                    grid.GetEditor('TaxAmount').SetEnabled(false);


                    cddlVatGstCst.SetSelectedIndex(0);
                    cddlVatGstCst.SetEnabled(false);
                    cbtn_SaveRecords.SetVisible(false);
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 2);
                    }


                }
                cddlVatGstCst.PerformCallback('Tax-code' + '~' + Tax_Code)
                document.getElementById('ddl_Currency').value = Currency_Id;
                document.getElementById('ddl_SalesAgent').value = SalesmanId;
                if (Contact_person_id != "0" && Contact_person_id != "")
                { cContactPerson.SetValue(Contact_person_id); }

            }
        }

        function ChangeState(value) {

            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }

        //function PerformCallToGridBind() {
        //    // ;
        //    grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
        //    cProductsPopup.Hide();
        //    return false;
        //}


        function PerformCallToGridBind() {

            //alert(BindGridOnQuotation1);
            grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
            cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
            $('#hdnPageStatus').val('Invoiceupdate');
            cddlPosGstUnReturn.SetEnabled(false);
            cProductsPopup.Hide();
            //#### added by Samrat Roy for Transporter Control #############
            //debugger;

            var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                callTransporterControl(quote_Id[0], 'SI');
            }
            return false;
        }


        function PerformCallToOldUnitGridBind() {
            cAspxOldUnitGrid.PerformCallback('BindGridOnOldUnit');



            //  SaveExit_ButtonClick();

        }

        function QuotationNumberChanged() {
            // ;
            //  console.log(0);
            //  debugger;
            var quote_Id = gridquotationLookup.GetValue();

            var KeyVal;
            KeyVal = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());


            if (quote_Id != null) {
                var arr = quote_Id.split(',');
                if (arr.length > 1) {
                    $('#<%=txt_InvoiceDate.ClientID %>').val('Multiple Select Invoice Dates');
                }
                else {
                    if (arr.length == 1) {
                        cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id);



                        //var VFinancier = gridquotationLookup.GetEditor('Financier').GetValue();
                        //var VOldUnit = gridquotationLookup.GetEditor('OldUnit').GetValue();
                        //$('#hdnisFinancier').val(VFinancier);

                        //$('#hdnisOldUnit').val(VOldUnit);

                        // alert(('#hdnisFinancier').val());

                        //  alert(('#hdnisOldUnit').val());
                    }
                    else {
                        // ctxt_InvoiceDate.SetText('');
                        $('#<%=txt_InvoiceDate.ClientID %>').val('');
                    }
                }
            }
            else {
                $('#<%=txt_InvoiceDate.ClientID %>').val('');
                //ctxt_InvoiceDate.SetText('');
            }

            if (quote_Id != null) {

                grid.PerformCallback('BindGridOnQuotation' + '~' + KeyVal);
                //cgridproducts.PerformCallback('BindProductsDetails' +'~' + KeyVal);
                //cProductsPopup.Show();
            }
            else {
                grid.PerformCallback('RemoveDisplay');

            }


            cddl_AmountAre.SetEnabled(false);
            ctxt_Rate.SetEnabled(false);
            cddlPosGstUnReturn.SetEnabled(false);
            document.getElementById("ddl_Currency").disabled = true;
        }
        //.............Available Stock Div Show............................


        function acpAvailableStockEndCall(s, e) {
            //   alert('kk');
            //debugger;
            if (cacpAvailableStock.cpstock != null) {
                divAvailableStk.style.display = "block";
                //   divpopupAvailableStock.style.display = "block";

                var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                // alert(AvlStk);
                document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = cacpAvailableStock.cpstock;


                cCmbWarehouse.cpstock = null;
            }
        }

        //................Available Stock Div Show....................


        (function (global) {

            if (typeof (global) === "undefined") {
                throw new Error("window is undefined");
            }

            var _hash = "!";
            var noBackPlease = function () {
                global.location.href += "#";

                // making sure we have the fruit available for juice (^__^)
                global.setTimeout(function () {
                    global.location.href += "!";
                }, 50);
            };

            global.onhashchange = function () {
                if (global.location.hash !== _hash) {
                    global.location.hash = _hash;
                }
            };

            global.onload = function () {
                noBackPlease();

                // disables backspace on page except on input fields and textarea..
                document.body.onkeydown = function (e) {
                    var elm = e.target.nodeName.toLowerCase();
                    if (e.which === 8 && (elm !== 'input' && elm !== 'textarea')) {
                        e.preventDefault();
                    }
                    // stopping event bubbling up the DOM tree..
                    e.stopPropagation();
                };
            }

        })(window);

        var isCtrl = false;
        //document.onkeyup = function (e) {
        //    if (event.keyCode == 17) {
        //        isCtrl = false;
        //    }
        //    else if (event.keyCode == 27) {
        //        btnCancel_Click();
        //    }
        //}

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

        document.onkeydown = function (e) {

            //   alert(event.keyCode);
            if (event.keyCode == 18) isCtrl = true;
            if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") {
                //  alert('kkk'); //run code for Alt + n -- ie, Save & New
                StopDefaultAction(e);
                Save_ButtonClick();
            }
            else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for alt+X -- ie, Save & Exit!   
                // alert('kkk222');
                StopDefaultAction(e);
                SaveExit_ButtonClick();
            }

            else if (event.keyCode == 85 && event.altKey == true) { //run code for alt+U -- ie, Save & Exit!   
                // alert('kkk222');
                StopDefaultAction(e);
                OpenUdf();
            }
            else if (event.keyCode == 84 && event.altKey == true && getUrlVars().req != "V") { //run code for alt+T -- ie, Save & Exit!   
                // alert('kkk222');
                StopDefaultAction(e);
                Save_TaxesClick();
            }
        }

        //transporter
        document.onkeyup = function (e) {
            //debugger;
            if (event.altKey == true) {
                switch (event.keyCode) {
                    case 83:
                        if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                            if (getUrlVars().req != "V") {
                                SaveVehicleControlData();
                            }
                        }
                        break;
                    case 67:
                        modalShowHide(0);
                        break;
                    case 82:
                        modalShowHide(1);
                        $('body').on('shown.bs.modal', '#exampleModal', function () {
                            $('input:visible:enabled:first', this).focus();
                        })
                        break;
                    case 78:
                        StopDefaultAction(e);
                        if (getUrlVars().req != "V") {
                            Save_ButtonClick();
                        }
                        break;
                    case 88:
                        StopDefaultAction(e);
                        if (getUrlVars().req != "V") {
                            SaveExit_ButtonClick();
                        }
                        break;
                    case 120:
                        StopDefaultAction(e);
                        if (getUrlVars().req != "V") {
                            SaveExit_ButtonClick();
                        }
                        break;
                    case 84:
                        StopDefaultAction(e);
                        if (getUrlVars().req != "V") {
                            Save_TaxesClick();
                        }
                        break;
                    case 85:
                        OpenUdf();
                        break;
                }
            }
        }

        //transporter
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }


        function onBranchItems() {
            //  GetIndentReqNoOnLoad();

            grid.batchEditApi.StartEdit(-1, 1);
            var accountingDataMin = grid.GetEditor('ProductName').GetValue();
            grid.batchEditApi.EndEdit();
            //  console.log(accountingDataMin);

            grid.batchEditApi.StartEdit(0, 1);
            var accountingDataplus = grid.GetEditor('ProductName').GetValue();
            // console.log(accountingDataplus);
            grid.batchEditApi.EndEdit();

            if (accountingDataMin != null || accountingDataplus != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

                    if (r == true) {

                        page.SetActiveTabIndex(0);
                        $('.dxeErrorCellSys').addClass('abc');

                        gridquotationLookup.gridView.UnselectRows();
                        gridquotationLookup.gridView.Refresh();
                        //  var startDate = tstartdate.GetValueString();

                        var startDate = new Date();
                        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                        //    var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                        //  var key = cCustomerComboBox.GetValue();
                        var key = $('#<%=hdnCustomerId.ClientID %>').val();
                        if (key != null && key != '') {
                            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                        }
                        grid.PerformCallback('GridBlank');
                        // cQuotationComponentPanel.PerformCallback('RemoveComponentGridOnSelection');

                        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                            clearTransporter();
                        }
                        ccmbGstCstVat.PerformCallback();
                        ccmbGstCstVatcharge.PerformCallback();
                        ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        // ctxt_InvoiceDate.SetText('');

                        $('#<%=txt_InvoiceDate.ClientID %>').val('');


                    } else {

                    }
                });
            }
            else {


                var startDate = new Date();
                startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                page.SetActiveTabIndex(0);
                $('.dxeErrorCellSys').addClass('abc');
                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                // var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                //  var key = cCustomerComboBox.GetValue();
                var key = $('#<%=hdnCustomerId.ClientID %>').val();
                if (key != null && key != '') {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                }
                // grid.PerformCallback('GridBlank');
                ccmbGstCstVat.PerformCallback();
                ccmbGstCstVatcharge.PerformCallback();
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                page.SetActiveTabIndex(0);

            }
        }
    </script>

    <%--Debu Section--%>
    <script type="text/javascript">

        var ProductGetQuantity = "0";
        var ProductGetTotalAmount = "0";
        var ProductSaleprice = "0";
        var globalNetAmount = 0;

        var ProductDiscount = "0";
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


            // debugger;
            if (e.buttonIndex == 0) {

                if (cddl_AmountAre.GetValue() != null) {
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

                    if (ProductID.trim() != "") {
                        //   ;
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
                        var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
                        //var strRate = "1";
                        var strStkUOM = SpliteDetails[4];
                        // var strSalePrice = SpliteDetails[6];
                        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                        if (strRate == 0) {
                            strRate = 1;
                        }

                        var StockQuantity = strMultiplier * QuantityValue;
                        //  var Amount = Math.round(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);  kaushik 29-7-2017
                        var Amount = parseFloat(Math.round(QuantityValue * strFactor * (strSalePrice / strRate) * 100) / 100).toFixed(2);
                        //clblTaxProdGrossAmt.SetText(DecimalRoundoff(grid.GetEditor('Amount').GetValue(),2));
                        //clblProdNetAmt.SetText(parseFloat((Math.round(Amount*100))/100).toFixed(2));

                        //document.getElementById('HdProdGrossAmt').value =grid.GetEditor('Amount').GetValue();
                        //document.getElementById('HdProdNetAmt').value = Amount;


                        //kaushik 24-1-2018

                        clblProdNetAmt.SetText(DecimalRoundoff(grid.GetEditor('Amount').GetValue(), 2));
                        clblTaxProdGrossAmt.SetText(parseFloat((Math.round(Amount * 100)) / 100).toFixed(2));
                        document.getElementById('HdProdNetAmt').value = grid.GetEditor('Amount').GetValue();
                        document.getElementById('HdProdGrossAmt').value = Amount;
                        //kaushik 24-1-2018

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
                        if (cddl_AmountAre.GetValue() == "1") {
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
                                        document.getElementById('HdProdGrossAmt').value = Math.round(Amount / gstDis).toFixed(2);
                                        clblGstForGross.SetText(Math.round(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                        clblTaxableNet.SetText("");
                                    }
                                    else {
                                        $('.gstGrossAmount').hide();

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
                        else if (cddl_AmountAre.GetValue() == "2") {
                            $('.GstCstvatClass').show();
                            $('.gstGrossAmount').hide();
                            $('.gstNetAmount').hide();
                            clblTaxableGross.SetText("");
                            clblTaxableNet.SetText("");

                            var shippingStCode = '';

                            ////###### Added By : Samrat Roy ##########
                            //Get Customer Shipping StateCode
                            if (cddlPosGstUnReturn.GetValue() == "S") {
                                shippingStCode = GeteShippingStateCode();
                            }
                            else {
                                shippingStCode = GetBillingStateCode();
                            }
                            
                            shippingStCode = shippingStCode;

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
        function taxAmtButnClick1(s, e) {
            console.log(grid.GetFocusedRowIndex());
            rowEditCtrl = s;
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

        var taxJson;
        function cgridTax_EndCallBack(s, e) {

            var totalGst = 0;
            //cgridTax.batchEditApi.StartEdit(0, 1);
            $('.cgridTaxClass').show();

            cgridTax.StartEditRow(0);


            //check Json data
            if (cgridTax.cpJsonData) {
                if (cgridTax.cpJsonData != "") {
                    taxJson = JSON.parse(cgridTax.cpJsonData);
                    cgridTax.cpJsonData = null;
                }
            } taxJson
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

            //if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
            //    ctxtTaxTotAmt.SetValue(Math.round(cgridTax.cpUpdated.split('~')[1]));
            //    var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
            //    var ddValue = parseFloat(ctxtGstCstVat.GetValue());
            //    ctxtTaxTotAmt.SetValue(Math.round(gridValue + ddValue));
            //    cgridTax.cpUpdated = "";
            //}
            if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
                ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);
                var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
                var ddValue = parseFloat(ctxtGstCstVat.GetValue());
                ctxtTaxTotAmt.SetValue(gridValue + ddValue);
                cgridTax.cpUpdated = "";
            }
            else {
                var totAmt = ctxtTaxTotAmt.GetValue();
                cgridTax.CancelEdit();
                caspxTaxpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 16);
                grid.GetEditor("TaxAmount").SetValue(totAmt);
                // grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));
                grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));


                if (cddl_AmountAre.GetValue() == "2") {
                    totalGst = parseFloat(grid.GetEditor("TaxAmount").GetValue());
                    var qty = grid.GetEditor("Quantity").GetValue();
                    var price = grid.GetEditor("SalePrice").GetValue();
                    var Discount = grid.GetEditor("Discount").GetValue();

                    var finalAmt = qty * price;


                    //if (GSTType=="G")
                    //    grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - totalGst), 2));
                    //else if (GSTType == "N") {

                    grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - (finalAmt * (Discount / 100)) - totalGst), 2));
                    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
                }

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

        function recalculateTax() {
            cmbGstCstVatChange(ccmbGstCstVat);
        }
        function recalculateTaxCharge() {
            ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
        }

    </script>
    <%--Debu Section End--%>

    <%--Sam Section Start--%>
    <script type="text/javascript">
        $(document).ready(function () {
            if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                page.GetTabByName('Billing/Shipping').SetEnabled(false);
            }
            $('#ApprovalCross').click(function () {
                //  ;
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.Refresh()();
            })
        })

             <%--kaushik 24-2-2017--%>
        function UniqueCodeCheck() {
            //debugger;
            var SchemeVal = $('#<%=ddl_numberingScheme.ClientID %> option:selected').val();
            if (SchemeVal == "") {
                alert('Please Select Numbering Scheme');
                //ctxt_PLQuoteNo.SetValue('');
                //ctxt_PLQuoteNo.Focus();
                $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                $('#<%=txt_PLQuoteNo.ClientID %>').focus();
            }
            else {
                // var ReturnNo = ctxt_PLQuoteNo.GetText();
                var ReturnNo = $('#<%=txt_PLQuoteNo.ClientID %>').val();
                if (ReturnNo != '') {

                    var SchemaLength = GetObjectID('hdnSchemaLength').value;
                    var x = parseInt(SchemaLength);
                    var y = parseInt(ReturnNo.length);

                    if (y > x) {
                        alert('Undelivery Return No length cannot be more than ' + x);
                        //jAlert('Please enter unique Sales Order No');
                        //  ctxt_PLQuoteNo.SetValue('');
                        // ctxt_PLQuoteNo.Focus();
                        $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                        $('#<%=txt_PLQuoteNo.ClientID %>').focus();

                    }
                    else {
                        var CheckUniqueCode = false;
                        $.ajax({
                            type: "POST",
                            url: "UndeliveryReturn.aspx/CheckUniqueCode",
                            data: JSON.stringify({ ReturnNo: ReturnNo }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                CheckUniqueCode = msg.d;
                                if (CheckUniqueCode == true) {
                                    alert('Please enter unique Undelivery Return No');
                                    //jAlert('Please enter unique Sales Order No');
                                    // ctxt_PLQuoteNo.SetValue('');
                                    // ctxt_PLQuoteNo.Focus();

                                    $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                                    $('#<%=txt_PLQuoteNo.ClientID %>').focus();
                                }
                                else {
                                    $('#MandatorysQuoteno').attr('style', 'display:none');
                                }
                            }

                        });
                    }
                }
            }
        }

        //function CloseGridLookup() {
        //    gridLookup.ConfirmCurrentSelection();
        //    gridLookup.HideDropDown();
        //    gridLookup.Focus();
        //}

        function GetContactPersonPhone(e) {
            var key = cContactPerson.GetValue();
            cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
        }
        function GetContactPerson(e) {

            //// var key = gridLookup.GetValue();
            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
            var branchid = $('#ddl_Branch').val();
            var keyC = $('#<%=hdnCustomerId.ClientID %>').val();
            //var keyC = cCustomerComboBox.GetValue();
            if (keyC != $('#<%=hdnCustomerId.ClientID %>').val()) {

                if (gridquotationLookup.GetValue() != null) {
                    jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

                        if (r == true) {
                            //  gridquotationLookup.gridView.UnselectRows();
                            // var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                            // var key = cCustomerComboBox.GetValue();
                            var key = $('#<%=hdnCustomerId.ClientID %>').val();
                            if (key != null && key != '') {



                                gridquotationLookup.gridView.UnselectRows();
                                gridquotationLookup.gridView.Refresh();
                                //  ctxt_InvoiceDate.SetText('');
                                $('#<%=txt_InvoiceDate.ClientID %>').val('');
                                ////  cContactPerson.PerformCallback('BindContactPerson~' + key);
                                // cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
                                // page.SetActiveTabIndex(1);
                                $('.dxeErrorCellSys').addClass('abc');

                                ////###### Added By : Samrat Roy ##########
                                ////cchkBilling.SetChecked(false);
                                ////cchkShipping.SetChecked(false);
                                ////$('.crossBtn').hide();
                                ////page.GetTabByName('Billing/Shipping').SetEnabled(true);
                                ////page.GetTabByName('General').SetEnabled(false);
                                SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                                //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SR');
                                GetObjectID('hdnCustomerId').value = key;
                                page.tabs[0].SetEnabled(true);
                                page.tabs[1].SetEnabled(true);
                                //if ($('#hfBSAlertFlag').val() == "1") {
                                //    jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                                //        if (r == true) {
                                //            page.SetActiveTabIndex(1);
                                //                        cbsSave_BillingShipping.Focus();
                                //            page.tabs[0].SetEnabled(false);
                                //            $("#divcross").hide();
                                //        }
                                //    });
                                //}
                                //else {
                                //    page.SetActiveTabIndex(1);
                                //    cbsSave_BillingShipping.Focus();
                                //    page.tabs[0].SetEnabled(false);
                                //    $("#divcross").hide();
                                //}
                                ////###### END : Samrat Roy : END ########## 


                                var startDate = new Date();
                                startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                                // cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                                //   var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                                // var key = cCustomerComboBox.GetValue();
                                var key = $('#<%=hdnCustomerId.ClientID %>').val();
                                if (key != null && key != '') {
                                    // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                                }
                                //grid.PerformCallback('GridBlank');
                                ////   cQuotationComponentPanel.PerformCallback('RemoveComponentGridOnSelection');

                                if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                                    clearTransporter();
                                }
                                //ccmbGstCstVat.PerformCallback();
                                //ccmbGstCstVatcharge.PerformCallback();
                                //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                                // ctxt_InvoiceDate.SetText('');

                                $('#<%=txt_InvoiceDate.ClientID %>').val('');


                                ////  billingLookup.focus();
                                ////  });

                                ////   document.getElementById('popup_ok').focus();
                            }

                        } else {
                            //  gridLookup.SetValue($('#<%=hdnCustomerId.ClientID %>').val());
                            <%--  cCustomerComboBox.SetValue($('#<%=hdnCustomerId.ClientID %>').val());

                        cCustomerComboBox.PerformCallBack();--%>


                            ctxtCustName.SetValue($('#<%=hdnCustomerId.ClientID %>').val());
                            var customerid = $('#<%=hdnCustomerId.ClientID %>').val();
                            ctxtCustName.PerformCallback(customerid)
                            // cCustomerComboBox.
                        }
                    });
                }
                else {

                    //  var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                    //  var key = cCustomerComboBox.GetValue();
                    var key = $('#<%=hdnCustomerId.ClientID %>').val();
                    if (key != null && key != '') {
                        $('#<%=txt_InvoiceDate.ClientID %>').val('');
                        //  ctxt_InvoiceDate.SetText('');
                        ////  cContactPerson.PerformCallback('BindContactPerson~' + key);
                        //cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
                        ////###### Added By : Samrat Roy ##########
                        ////cchkBilling.SetChecked(false);
                        ////cchkShipping.SetChecked(false);
                        ////$('.crossBtn').hide();
                        ////page.GetTabByName('Billing/Shipping').SetEnabled(true);
                        ////page.GetTabByName('General').SetEnabled(false);
                        ////page.SetActiveTabIndex(1);
                        SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                       // LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SR');
                        GetObjectID('hdnCustomerId').value = key;
                        page.tabs[0].SetEnabled(true);
                        page.tabs[1].SetEnabled(true);
                        //if ($('#hfBSAlertFlag').val() == "1") {
                        //    jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                        //        if (r == true) {
                        //            page.SetActiveTabIndex(1);
                        //            cbsSave_BillingShipping.Focus();
                        //            page.tabs[0].SetEnabled(false);
                        //            $("#divcross").hide();
                        //        }
                        //    });
                        //}
                        //else {
                        //    page.SetActiveTabIndex(1);
                        //    cbsSave_BillingShipping.Focus();
                        //    page.tabs[0].SetEnabled(false);
                        //    $("#divcross").hide();
                        //}
                        ////###### END : Samrat Roy : END ########## 


                        $('.dxeErrorCellSys').addClass('abc');
                        //// document.getElementById('popup_ok').focus();
                        // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%');
                        // cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                        ////   grid.PerformCallback('GridBlank');

                        GetObjectID('hdnAddressDtl').value = '0';
                    }

                }
            }
        }
        $(document).ready(function () {
            var schemaid = $('#ddl_numberingScheme').val();
            if (schemaid != null) {
                if (schemaid == '') {
                    //  ctxt_PLQuoteNo.SetEnabled(false);
                    document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = true;
                }
            }
            $('#ddl_numberingScheme').change(function () {
                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];
                var branchID = NoSchemeTypedtl.toString().split('~')[3];
                var fromDate = NoSchemeTypedtl.toString().split('~')[5];
                var todate = NoSchemeTypedtl.toString().split('~')[6];
                document.getElementById('ddl_Branch').value = branchID;

                var dt = new Date();

                tstartdate.SetDate(dt);

                if (dt < new Date(fromDate)) {
                    tstartdate.SetDate(new Date(fromDate));
                }

                if (dt > new Date(todate)) {
                    tstartdate.SetDate(new Date(todate));
                }




                tstartdate.SetMinDate(new Date(fromDate));
                tstartdate.SetMaxDate(new Date(todate));

                if (NoSchemeType == '1') {
                    //  ctxt_PLQuoteNo.SetText('Auto');
                    $('#<%=txt_PLQuoteNo.ClientID %>').val('Auto');
                    // ctxt_PLQuoteNo.SetEnabled(false);
                    document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = true;
                    tstartdate.SetEnabled(false);
                    tstartdate.Focus();
                }
                else if (NoSchemeType == '0') {
                    // ctxt_PLQuoteNo.SetEnabled(true);
                    document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = false;
                    $('#<%=txt_PLQuoteNo.ClientID %>').maxLength = quotelength;
                    //  ctxt_PLQuoteNo.GetInputElement().maxLength = quotelength;
                    tstartdate.SetEnabled(true);
                    //ctxt_PLQuoteNo.SetText('');
                    //ctxt_PLQuoteNo.Focus();
                    $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                    $('#<%=txt_PLQuoteNo.ClientID %>').focus();
                }
                else {

                    $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                    document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = true;
                    //ctxt_PLQuoteNo.SetText('');
                    //ctxt_PLQuoteNo.SetEnabled(false);
                    tstartdate.SetEnabled(false);

                }
            });

            $('#ddl_Currency').change(function () {

                var CurrencyId = $(this).val();
                var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                var basedCurrency = LocalCurrency.split("~")[0];
                // var Currency_ID = $("#ddl_Currency").val();
                //  alert(basedCurrency);
                if ($("#ddl_Currency").val() == basedCurrency) {
                    ctxt_Rate.SetValue("");
                    ctxt_Rate.SetEnabled(false);
                }
                else {
                    if (basedCurrency != CurrencyId) {
                        if (LocalCurrency != null) {
                            if (CurrencyId != '0') {
                                $.ajax({
                                    type: "POST",
                                    url: "SalesInvoice.aspx/GetCurrentConvertedRate",
                                    data: "{'CurrencyId':'" + CurrencyId + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (msg) {
                                        var currentRate = msg.d;
                                        if (currentRate != null) {

                                            ctxt_Rate.SetValue(currentRate);
                                        }
                                        else {
                                            ctxt_Rate.SetValue('1');
                                        }
                                        ReBindGrid_Currency();
                                    }
                                });
                            }
                            else {
                                ctxt_Rate.SetValue("1");
                                ReBindGrid_Currency();
                            }
                        }
                    }
                    else {
                        ctxt_Rate.SetValue("1");
                        ReBindGrid_Currency();
                    }
                    ctxt_Rate.SetEnabled(true);
                }



            });
        });

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

        function PopulateGSTCSTVAT(e) {
            var key = cddl_AmountAre.GetValue();
            //deleteAllRows();

            if (key == 1) {

                grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);

                cddlVatGstCst.SetSelectedIndex(0);
                cbtn_SaveRecords.SetVisible(true);
                grid.GetEditor('ProductID').Focus();
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }

            }
            else if (key == 2) {
                grid.GetEditor('TaxAmount').SetEnabled(true);

                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');
                cddlVatGstCst.Focus();
                cbtn_SaveRecords.SetVisible(true);
            }
            else if (key == 3) {

                grid.GetEditor('TaxAmount').SetEnabled(false);


                cddlVatGstCst.SetSelectedIndex(0);
                cddlVatGstCst.SetEnabled(false);
                cbtn_SaveRecords.SetVisible(false);
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }


            }

        }

        //Date Function Start

        function Startdate(s, e) {
            grid.batchEditApi.EndEdit();
            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsProduct = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }


            var t = s.GetDate();
            ccmbGstCstVat.PerformCallback(t);
            ccmbGstCstVatcharge.PerformCallback(t);
            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            if (IsProduct == "Y") {
                $('#<%=hdfIsDelete.ClientID %>').val('D');
                $('#<%=HdUpdateMainGrid.ClientID %>').val('True');
                //   grid.UpdateEdit();
                cacbpCrpUdf.PerformCallback();
                //kaushik
            }

            if (t == "")
            { $('#MandatorysDate').attr('style', 'display:block'); }
            else { $('#MandatorysDate').attr('style', 'display:none'); }
        }
        function Enddate(s, e) {

            var t = s.GetDate();
            if (t == "")
            { $('#MandatoryEDate').attr('style', 'display:block'); }
            else { $('#MandatoryEDate').attr('style', 'display:none'); }



            var sdate = tstartdate.GetValue();
            var edate = tenddate.GetValue();

            var startDate = new Date(sdate);
            var endDate = new Date(edate);

            if (startDate > endDate) {

                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }

        //Date Function End

        // Popup Section

        function ShowCustom() {

            cPopup_wareHouse.Show();


        }

        // Popup Section End

    </script>
    <%--Sam Section End--%>

    <%--Sudip--%>
    <script>
        var IsProduct = "";
        var currentEditableVisibleIndex;
        var preventEndEditOnLostFocus = false;
        var lastProductID;
        var setValueFlag;

        function GridCallBack() {
            $('#ddl_numberingScheme').focus();

            //  grid.PerformCallback('Display');
        }

        function ReBindGrid_Currency() {
            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsProduct = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (IsProduct == "Y") {
                $('#<%=hdfIsDelete.ClientID %>').val('D');
                // grid.UpdateEdit();
                cacbpCrpUdf.PerformCallback();
                //kaushik
                grid.PerformCallback('CurrencyChangeDisplay');
            }
        }

        function ProductsCombo_SelectedIndexChanged(s, e) {
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            //var tbStkUOM = grid.GetEditor("StockUOM");
            //var tbStockQuantity = grid.GetEditor("StockQuantity");

            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "";
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            strProductName = strDescription;

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];

            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            if (strRate == 0) {
                strSalePrice = strSalePrice;
            }
            else {
                strSalePrice = strSalePrice / strRate;
            }

            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tbSalePrice.SetValue(strSalePrice);

            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }
            //divPacking.style.display = "none";

            //lblbranchName lblProduct
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");
            //Debjyoti
            ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
            //cacpAvailableStock.PerformCallback(strProductID);
        }
        function cmbContactPersonEndCall(s, e) {
            LoadingPanel.Hide();
            if (cContactPerson.cpDueDate != null) {
                var DeuDate = cContactPerson.cpDueDate;
                var myDate = new Date(DeuDate);

                cdt_SaleInvoiceDue.SetDate(myDate);
                cContactPerson.cpDueDate = null;
            }

            if (cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) {

                $("#<%=divGSTN.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblGSTIN.ClientID %>').innerHTML = cContactPerson.cpGSTN;
                cContactPerson.cpGSTN = null;
            }

            if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {
                //alert(cContactPerson.cpOutstanding);

                $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
                // pageheaderOutContent.style.display = "block";

                $("#<%=divOutstanding.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblOutstanding.ClientID %>').innerHTML = cContactPerson.cpOutstanding;

                cContactPerson.cpOutstanding = null;
            }
            else {
                $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:none');
                //pageheaderOutContent.style.display = "none";
                $("#<%=divOutstanding.ClientID%>").attr('style', 'display:none');
                document.getElementById('<%=lblOutstanding.ClientID %>').innerHTML = '';
            }


        }


        function cgridproductsEndCall(s, e) {



            if (cgridproducts.cpFinancier != null && cgridproducts.cpFinancier != undefined) {

                $('#hdnisFinancier').val(cgridproducts.cpFinancier);



            }
            if (cgridproducts.cpOldUnit != null && cgridproducts.cpOldUnit != undefined) {


                $('#hdnisOldUnit').val(cgridproducts.cpOldUnit);
            }


            cgridproducts.cpFinancier = null;
            cgridproducts.cpOldUnit = null;
        }





        function cAspxOldUnitGridEndCall(s, e) {


            if (cAspxOldUnitGrid.cpProductInfo != null && cAspxOldUnitGrid.cpProductInfo != undefined) {

                $('#hdnOldUnitProdInfo').val(cAspxOldUnitGrid.cpProductInfo);


                cPopup_OldUnit.Hide();
                grid.UpdateEdit();



                //  grid.UpdateEdit();

            }



            cAspxOldUnitGrid.cpProductInfo = null;


        }


        function OnEndCallback(s, e) {
            // debugger;
            // OnAddNewClick();


            if (grid.cpFinancier != null && grid.cpFinancier != undefined) {

                $('#hdnisFinancier').val(grid.cpFinancier);



            }
            if (grid.cpOldUnit != null && grid.cpOldUnit != undefined) {


                $('#hdnisOldUnit').val(grid.cpOldUnit);
            }


            grid.cpFinancier = null;
            grid.cpOldUnit = null;



            var value = document.getElementById('hdnRefreshType').value;

            //Debjyoti Check grid needs to be refreshed or not
            if ($('#<%=HdUpdateMainGrid.ClientID %>').val() == 'True') {
                $('#<%=HdUpdateMainGrid.ClientID %>').val('False');
                grid.PerformCallback('DateChangeDisplay');
            }

            // LoadingPanel.Hide();
            //if (grid.cpinsert == 'UDFMandatory') {
            //    jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
            //    OnAddNewClick();

            //    grid.batchEditApi.StartEdit(-1);
            //    grid.cpinsert = null;

            //}
            //else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
            //    jAlert('Transporter is set as Mandatory. Please enter values.');
            //    //OnAddNewClick();
            //    grid.StartEditRow(0);
            // //   grid.batchEditApi.StartEdit(-1);
            //    grid.cpSaveSuccessOrFail = null;
            //}



            if (grid.cpRemoveProductInvoice) {
                if (grid.cpRemoveProductInvoice == "valid") {
                    OnAddNewClick();
                    grid.cpRemoveProductInvoice = null;
                }
            }
            else { grid.GetEditor('Product').SetEnabled(true); }  //when invoice is not select
            if (grid.cpSaveSuccessOrFail == "outrange") {
                LoadingPanel.Hide();
                jAlert('Can Not Add More Sales Invoice Number as Sales Invoice Scheme Exausted.<br />Update The Scheme and Try Again');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
                LoadingPanel.Hide();
                OnAddNewClick();
                jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                LoadingPanel.Hide();
                jAlert('Can Not Save as Duplicate Sales Return Number No. Found');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "quantityTagged") {
                LoadingPanel.Hide();
                jAlert(' Quantity of selected products cannot be less than Ordered Quantity.');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                LoadingPanel.Hide();
                jAlert('Please try again later.');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "nullAmount") {
                LoadingPanel.Hide();
                jAlert('total amount cant not be zero(0).');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                LoadingPanel.Hide();
                jAlert('Please fill Quantity');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                LoadingPanel.Hide();
                jAlert('Can not Duplicate Product in the Sales Return List.');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                LoadingPanel.Hide();
                var SrlNo = grid.cpProductSrlIDCheck;
                var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
                jAlert(msg);
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "checkAddress") {
                LoadingPanel.Hide();
                jAlert('Enter Billing Shipping address to save this document.');
                //OnAddNewClick();
                grid.AddNewRow();
                grid.cpSaveSuccessOrFail = null;
            }
            else {
                var SalesReturn_Number = grid.cpQuotationNo;
                var SalesReturn_Msg = "Undelivery Return No. " + SalesReturn_Number + " saved.";

                if (value == "E") {
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else if (grid.cpApproverStatus == "rejected") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else {
                        if (SalesReturn_Number != "") {

                            jAlert(SalesReturn_Msg, 'Alert Dialog: [UndeliveryReturn]', function (r) {
                                LoadingPanel.Hide();
                                //jAlert(Order_Msg);
                                grid.cpQuotationNo = null;
                                if (r == true) {
                                    window.location.assign("UndeliveryReturnList.aspx");
                                }
                            });


                        }
                        else {

                            window.location.assign("UndeliveryReturnList.aspx");
                        }
                    }

                }
                else if (value == "N") {
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else {
                        if (SalesReturn_Number != "") {

                            jAlert(SalesReturn_Msg, 'Alert Dialog: [Undeliveryeturn]', function (r) {
                                LoadingPanel.Hide();
                                //jAlert(Order_Msg);
                                grid.cpQuotationNo = null;
                                if (r == true) {

                                    window.location.assign("UndeliveryReturn.aspx?key=ADD");
                                }
                            });

                        }
                        else {

                            window.location.assign("UndeliveryReturn.aspx?key=ADD");
                        }
                    }
                }
                else {
                    var pageStatus = document.getElementById('hdnPageStatus').value;
                    if (pageStatus == "first") {
                        OnAddNewClick();
                        grid.batchEditApi.EndEdit();
                        // it has been commented by sam on 04032017 due to set focus from server side start
                        //document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();
                        // above part has been commented by sam on 04032017 due to set focus from server side start

                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if ($("#ddl_Currency").val() == basedCurrency[0]) {
                            ctxt_Rate.SetEnabled(false);
                        }
                    }
                    else if (pageStatus == "update") {
                        OnAddNewClick();
                        grid.batchEditApi.StartEdit(0, 1)
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if ($("#ddl_Currency").val() == basedCurrency[0]) {
                            ctxt_Rate.SetEnabled(false);
                        }
                    }
                    else if (pageStatus == "Invoiceupdate") {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "delete") {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
    }
}

    if (grid.cpGridBlank == "1") {


        //  grid.AddNewRow();
        //kaushik 14-4-2017
        // grid.StartEditRow(0);

        gridquotationLookup.gridView.Refresh();
        //OnAddNewClick();


        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        grid.GetEditor('Product').SetEnabled(true);
        grid.cpGridBlank = null;
    }
    else {
        if (gridquotationLookup.GetValue() != null) {
            //OnAddNewClick();
            grid.GetEditor('Product').SetEnabled(false);


        }
        else {
            // OnAddNewClick();
            grid.GetEditor('Product').SetEnabled(true);
        }
    }

    cProductsPopup.Hide();

}


<%--        function SaveN_ButtonClick() {


            var financierval = $('#hdnisFinancier').val();
            var OldUnitval = $('#hdnisOldUnit').val();
            var invoiceId = (gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex()) : "";

            var pType = "<%=Convert.ToString(Session["USRN_ActionType"])%>";



            flag = false;
            if ((financierval == 'Yes' || OldUnitval == 'Yes') && (pType != 'Edit')) {

                if (financierval == 'Yes' && OldUnitval == 'Yes') {

                    jConfirm('Wish to reverse adjust Old Unit and Finance Invoice values?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cPopup_OldUnit.Show();
                            cAspxOldUnitGrid.PerformCallback(invoiceId);

                        }
                        else { Save_ButtonClick();}
                    });
                }
                else if (financierval == 'Yes' && OldUnitval == 'No') {
                    jConfirm('Wish to reverse adjust  Finance Invoice values?', 'Confirmation Dialog', function (r) {
                        if (r == true) {

                            Save_ButtonClick();
                        }
                        else { Save_ButtonClick(); }
                    });
                }

                else {
                    jConfirm('Wish to reverse adjust Old Unit  values?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cPopup_OldUnit.Show();
                            cAspxOldUnitGrid.PerformCallback(invoiceId);
                        }
                        else { Save_ButtonClick(); }
                    });
                }

            }

            else { Save_ButtonClick();}

        }--%>




        function Save_ButtonClick() {
            LoadingPanel.Show();
            flag = true;
            grid.batchEditApi.EndEdit();
            // Quote no validation Start

            var ReasonforRet = $('#<%=txtReasonforChange.ClientID %>').val();
    ReasonforRet = ReasonforRet.trim();
    if (ReasonforRet == '' || ReasonforRet == null) {
        $('#MandatoryReasonforChange').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatoryReasonforChange').attr('style', 'display:none');
    }


            // var QuoteNo = ctxt_PLQuoteNo.GetText();
    var QuoteNo = $('#<%=txt_PLQuoteNo.ClientID %>').val();
    QuoteNo = QuoteNo.trim();
    if (QuoteNo == '' || QuoteNo == null) {
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }
            // Quote no validation End
    var invoice_Id = gridquotationLookup.GetValue();

    if (invoice_Id == null) {
        $('#MandatorysSCno').attr('style', 'display:block');
        flag = false;

    }
    else {
        $('#MandatorysSCno').attr('style', 'display:none');
    }


            // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);
            //if (sdate == null || sdate == "") {
            //    flag = false;
            //    $('#MandatorysDate').attr('style', 'display:block');
            //}
            //else { $('#MandatorysDate').attr('style', 'display:none'); }
            //if (edate == null || sdate == "") {
            //    flag = false;
            //    $('#MandatoryEDate').attr('style', 'display:block');
            //}
            //else {
            //    $('#MandatoryEDate').attr('style', 'display:none');
            //    if (startDate > endDate) {

            //        flag = false;
            //        $('#MandatoryEgSDate').attr('style', 'display:block');
            //    }
            //    else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
            //}
            // Quote Date validation End

            // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {

        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
            // Quote Customer validation End
    var amtare = cddl_AmountAre.GetValue();
    if (amtare == '1') {
        var taxcodeid = cddlVatGstCst.GetValue();
        if (taxcodeid == '' || taxcodeid == null) {
            $('#Mandatorytaxcode').attr('style', 'display:block');
            flag = false;
        }
        else {
            $('#Mandatorytaxcode').attr('style', 'display:none');
        }
    }

    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }

    if (flag != false) {
        if (IsProduct == "Y") {
            //divSubmitButton.style.display = "none";
            //  var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
            //  var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";
            var customerval = ($('#<%=hdnCustomerId.ClientID %>').val() != null) ? $('#<%=hdnCustomerId.ClientID %>').val() : "";
            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


            // Custom Control Data Bind

            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
            $('#<%=hdnRefreshType.ClientID %>').val('N');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            grid.batchEditApi.EndEdit();
            // grid.UpdateEdit();
            cacbpCrpUdf.PerformCallback();
            //kaushik
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            LoadingPanel.Hide();
        }
    }
    else { LoadingPanel.Hide(); }
}




      <%--  function SaveExitN_ButtonClick() {

            

            var financierval = $('#hdnisFinancier').val();
            var OldUnitval = $('#hdnisOldUnit').val();
            var invoiceId = (gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex()) : "";
            
            var pType = "<%=Convert.ToString(Session["USRN_ActionType"])%>";


           
            flag = false;


            if ((financierval == 'Yes' || OldUnitval == 'Yes') && (pType != 'Edit')) {
               
                if (financierval == 'Yes' && OldUnitval == 'Yes') {

                    jConfirm('Selected Invoice having old units and financier. Confirm ?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cPopup_OldUnit.Show();
                            cAspxOldUnitGrid.PerformCallback(invoiceId);

                        }
                        else
                        { SaveExit_ButtonClick() }
                    });
                }
                else if (financierval == 'Yes' && OldUnitval == 'No') {
                    jConfirm('Selected Invoice having  financier. Confirm ?', 'Confirmation Dialog', function (r) {
                        if (r == true) {

                            SaveExit_ButtonClick()
                        }
                        else { SaveExit_ButtonClick() }
                    });
                }

                else {
                    jConfirm('Selected Invoice having old units . Confirm ?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cPopup_OldUnit.Show();
                            cAspxOldUnitGrid.PerformCallback(invoiceId);
                        }
                        else { SaveExit_ButtonClick() }
                    });
                }

           }

           else { SaveExit_ButtonClick() }
      }--%>



        function SaveExit_ButtonClick() {
            //  debugger;

            flag = true;


            LoadingPanel.Show();

            grid.batchEditApi.EndEdit();

            var ReasonforRet = $('#<%=txtReasonforChange.ClientID %>').val();
            ReasonforRet = ReasonforRet.trim();
            if (ReasonforRet == '' || ReasonforRet == null) {
                $('#MandatoryReasonforChange').attr('style', 'display:block');
                flag = false;
            }
            else {
                $('#MandatoryReasonforChange').attr('style', 'display:none');
            }
            // Quote no validation Start
            var QuoteNo = $('#<%=txt_PLQuoteNo.ClientID %>').val();
            QuoteNo = QuoteNo.trim();
            if (QuoteNo == '' || QuoteNo == null) {
                $('#MandatorysQuoteno').attr('style', 'display:block');
                flag = false;
            }
            else {
                $('#MandatorysQuoteno').attr('style', 'display:none');
            }
            // Quote no validation End
            var invoice_Id = gridquotationLookup.GetValue();

            if (invoice_Id == null) {
                $('#MandatorysSCno').attr('style', 'display:block');
                flag = false;

            }
            else {
                $('#MandatorysSCno').attr('style', 'display:none');
            }


            // Quote Date validation Start
            var sdate = tstartdate.GetValue();
            var edate = tenddate.GetValue();

            var startDate = new Date(sdate);
            var endDate = new Date(edate);
            //if (sdate == null || sdate == "") {
            //    flag = false;
            //    $('#MandatorysDate').attr('style', 'display:block');
            //}
            //else { $('#MandatorysDate').attr('style', 'display:none'); }
            //if (edate == null || sdate == "") {
            //    flag = false;
            //    $('#MandatoryEDate').attr('style', 'display:block');
            //}
            //else {
            //    $('#MandatoryEDate').attr('style', 'display:none');
            //    if (startDate > endDate) {

            //        flag = false;
            //        $('#MandatoryEgSDate').attr('style', 'display:block');
            //    }
            //    else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
            //}
            // Quote Date validation End

            // Quote Customer validation Start
            var customerId = GetObjectID('hdnCustomerId').value
            if (customerId == '' || customerId == null) {
                $('#MandatorysCustomer').attr('style', 'display:block');
                flag = false;
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }
            // Quote Customer validation End

            var amtare = cddl_AmountAre.GetValue();
            if (amtare == '1') {
                var taxcodeid = cddlVatGstCst.GetValue();
                if (taxcodeid == '' || taxcodeid == null) {
                    $('#Mandatorytaxcode').attr('style', 'display:block');
                    flag = false;
                }
                else {
                    $('#Mandatorytaxcode').attr('style', 'display:none');
                }
            }

            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsProduct = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (flag != false) {
                if (IsProduct == "Y") {
                    //divSubmitButton.style.display = "none";
                    // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    // var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";


                    var customerval = ($('#<%=hdnCustomerId.ClientID %>').val() != null) ? $('#<%=hdnCustomerId.ClientID %>').val() : "";
            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
            $('#<%=hdnRefreshType.ClientID %>').val('E');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            grid.batchEditApi.EndEdit();
            // grid.UpdateEdit();
            cacbpCrpUdf.PerformCallback();
            //kaushik
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            LoadingPanel.Hide();
        }
    }
    else { LoadingPanel.Hide(); }
}



function SalePriceGotFocus() {
    ProductSaleprice = grid.GetEditor("SalePrice").GetValue();


    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());

}



function QuantityGotFocus(s, e) {

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductGetQuantity = QuantityValue;
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());

}


var fromColumn = '';
function QuantityTextChange(s, e) {
    $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');



    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)) {
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        //   var key = gridLookup.GetValue();
        // var key = cCustomerComboBox.GetValue();
        var key = $('#<%=hdnCustomerId.ClientID %>').val();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";

            if (key != null && key != '') {
                var IsComponentProduct = SpliteDetails[15];
                var ComponentProduct = SpliteDetails[16];
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
                    grid.GetEditor("Quantity").SetValue(TotalQty);
                    var OrdeMsg = 'Cannot enter quantity more than balance quantity.';
                    grid.batchEditApi.EndEdit();
                    jAlert(OrdeMsg, 'Alert Dialog: [Balance Quantity ]', function (r) {
                        grid.batchEditApi.StartEdit(globalRowIndex, 7);
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
            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

            if (strRate == 0) {
                strRate = 1;
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

            $('#<%= lblStkQty.ClientID %>').text(StockQuantity);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            <%--  var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
            divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }--%>

            //var tbStockQuantity = grid.GetEditor("StockQuantity");
            //tbStockQuantity.SetValue(StockQuantity);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount);


            DiscountTextChange(s, e);
            //  cacpAvailableStock.PerformCallback(strProductID);
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Quantity').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}
<%--function QuantityTextChange(s, e) {
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strMultiplier = SpliteDetails[7];
        var strFactor = SpliteDetails[8];
        var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";

        var strProductID = SpliteDetails[0];
        var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();

     
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];

        if (strRate == 0) {
            strRate = 1;
        }

        var StockQuantity = strMultiplier * QuantityValue;
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        $('#<%= lblStkQty.ClientID %>').text(StockQuantity);
        $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
        $('#<%= lblProduct.ClientID %>').text(strProductName);
        $('#<%= lblbranchName.ClientID %>').text(strBranch);

        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
            divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }

     
        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(Amount);

        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(Amount);

        DiscountTextChange(s, e);
        cacpAvailableStock.PerformCallback(strProductID);
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Quantity').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
}--%>

        /// Code Added By Sam on 23022017 after make editable of sale price field Start

        function SalePriceTextChange(s, e) {
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";


            if (ProductSaleprice != parseFloat(Saleprice)) {
                var ProductID = grid.GetEditor('ProductID').GetValue();
                if (ProductID != null) {
                    var SpliteDetails = ProductID.split("||@||");
                    var strMultiplier = SpliteDetails[7];
                    var strFactor = SpliteDetails[8];
                    var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
                    //var strRate = "1";
                    var strStkUOM = SpliteDetails[4];
                    //var strSalePrice = SpliteDetails[6];

                    var strProductID = SpliteDetails[0];
                    var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                    var ddlbranch = $("[id*=ddl_Branch]");
                    var strBranch = ddlbranch.find("option:selected").text();

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

                    $('#<%= lblProduct.ClientID %>').text(strProductName);
                    $('#<%= lblbranchName.ClientID %>').text(strBranch);

                    var IsPackingActive = SpliteDetails[10];
                    var Packing_Factor = SpliteDetails[11];
                    var Packing_UOM = SpliteDetails[12];
                    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

                    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                        $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                        divPacking.style.display = "block";
                    } else {
                        divPacking.style.display = "none";
                    }

                    // cacpAvailableStock.PerformCallback(strProductID);
                }
                else {
                    jAlert('Select a product first.');
                    grid.GetEditor('SalePrice').SetValue('0');
                    grid.GetEditor('ProductID').Focus();
                }
            }
        }


        /// Code Above Added By Sam on 23022017 after make editable of sale price field End


        function DiscountGotChange() {
            globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            ProductGetTotalAmount = globalNetAmount;

            ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
            ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            ProductSaleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";

        }

        function DiscountTextChange(s, e) {
            //debugger;
            //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var SalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetValue();
            if (ProductID != null) {

                if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(SalePrice) != parseFloat(ProductSaleprice)) {


                    var SpliteDetails = ProductID.split("||@||");
                    var strFactor = SpliteDetails[8];
                    var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
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

                    var IsPackingActive = SpliteDetails[10];
                    var Packing_Factor = SpliteDetails[11];
                    var Packing_UOM = SpliteDetails[12];
                    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

                    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                        $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                    divPacking.style.display = "block";
                } else {
                    divPacking.style.display = "none";
                }
                grid.GetEditor('TaxAmount').SetValue(0);
                var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
                var tbTotalAmount = grid.GetEditor("TotalAmount");

                tbTotalAmount.SetValue(parseFloat(amountAfterDiscount) + parseFloat(_TotalTaxAmt));

                  //var tbTotalAmount = grid.GetEditor("TotalAmount");
                  //tbTotalAmount.SetValue(amountAfterDiscount);
            }
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Discount').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
            //Debjyoti 


        var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";


        if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
            grid.GetEditor('TaxAmount').SetValue(0);

            ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
        }

    }
    function AddBatchNew(s, e) {
        var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

        var globalRow_Index = 0;
        if (globalRowIndex > 0) {
            globalRow_Index = globalRowIndex + 1;
        }
        else {
            globalRow_Index = globalRowIndex - 1;
        }


        var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
        if (keyCode === 13) {
            if (ProductIDValue != "") {
                //var noofvisiblerows = grid.GetVisibleRowsOnPage();
                //var i;
                //var cnt = 2;

                grid.batchEditApi.EndEdit();

                grid.AddNewRow();
                grid.SetFocusedRowIndex();
                var noofvisiblerows = grid.GetVisibleRowsOnPage();

                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);

                grid.batchEditApi.StartEdit(globalRow_Index, 2);
                //grid.batchEditApi.StartEdit(-1, 1);
            }
        }
    }
    function OnAddNewClick() {


        //kaushik 21-10-2017

        //kaushik 21-10-2017
        if (gridquotationLookup.GetValue() == null) {
            grid.AddNewRow();

            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
            grid.GetEditor('Product').SetEnabled(true);
        }
        else {

            //   QuotationNumberChanged();

            grid.AddNewRow();
            //kaushik 14-4-2017
            grid.StartEditRow(0);
            grid.GetEditor('Product').SetEnabled(false);
        }
    }

    function Save_TaxClick() {
        // debugger;
        if (gridTax.GetVisibleRowsOnPage() > 0) {
            gridTax.UpdateEdit();
            $('#hdnIsInvoiceTax').val('2');
        }
        else {
            gridTax.PerformCallback('SaveGst');
        }
        cPopup_Taxes.Hide();
    }

    var Warehouseindex;
    function OnCustomButtonClick(s, e) {
        //debugger;
        <%--    if (e.buttonID == 'CustomDelete') {
                var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
                grid.batchEditApi.EndEdit();

                $('#<%=hdnDeleteSrlNo.ClientID %>').val(SrlNo);
                var noofvisiblerows = grid.GetVisibleRowsOnPage();
                if (gridquotationLookup.GetValue() != null) {
                    var messege = "";
                    messege = "Cannot Delete using this button as the Sales Invoice is linked with this Sale Return.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
                    jAlert(messege, 'Alert Dialog: [Delete Sales Invoice Products]', function (r) {
                    });

                }
                else {
                    if (noofvisiblerows != "1") {
                        grid.DeleteRow(e.visibleIndex);

                        $('#<%=hdfIsDelete.ClientID %>').val('D');
                grid.UpdateEdit();
                // cacbpCrpUdf.PerformCallback();
                //kaushik
                grid.PerformCallback('Display');



                //  $('#<%=hdnPageStatus.ClientID %>').val('update');
                $('#<%=hdnPageStatus.ClientID %>').val('delete');
                //grid.batchEditApi.StartEdit(-1, 2);
                //grid.batchEditApi.StartEdit(0, 2);
            }
        }
    }
    else if (e.buttonID == 'AddNew') {
        //debugger;
        if (gridquotationLookup.GetValue() == null) {



            var ProductIDValue = (grid.GetEditor('ProductDisID').GetText() != null) ? grid.GetEditor('ProductDisID').GetText() : "0";
            if (ProductIDValue != "") {
                OnAddNewClick();

                grid.batchEditApi.StartEdit(globalRowIndex, 2);
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 2);
                }, 500);

                return false;
            }
            else {
                //setTimeout(function () {
                //    grid.batchEditApi.StartEdit(globalRowIndex, 2);
                //}, 500);
                //return false;
                ////
                grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            }
        }
        else {
           
            QuotationNumberChanged();
        }
    }--%>
            if (e.buttonID == 'CustomWarehouse') {


                //alert(grid.GetEditor('ProductDisID').GetValue());
                //alert(grid.GetEditor('ProductID').GetValue());
                var index = e.visibleIndex;
                grid.batchEditApi.StartEdit(index, 2)
                Warehouseindex = index;

                var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
                var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

                var Invoicenumber = (grid.GetEditor('ComponentNumber').GetValue() != null) ? grid.GetEditor('ComponentNumber').GetValue() : "0";

                if (QuantityValue == "0.0") {
                    jAlert("Quantity should not be zero !.");
                } else {
                    $("#spnCmbWarehouse").hide();
                    $("#spnCmbBatch").hide();
                    $("#spncheckComboBox").hide();
                    $("#spntxtQuantity").hide();
                    //alert(ProductID);
                    if (ProductID != "") {
                        var SpliteDetails = ProductID.split("||@||");
                        var strProductID = SpliteDetails[0];
                        var strDescription = SpliteDetails[1];
                        var strUOM = SpliteDetails[2];
                        var strStkUOM = SpliteDetails[4];
                        var strMultiplier = SpliteDetails[7];
                        var strProductName = (grid.GetEditor('ProductName').GetText() != null) ? grid.GetEditor('ProductName').GetText() : "";
                        var StkQuantityValue = QuantityValue * strMultiplier;
                        $('#<%=hdnInnumber.ClientID %>').val(Invoicenumber);
                $('#<%=hdfProductIDPC.ClientID %>').val(strProductID);
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
                    url: 'UndeliveryReturn.aspx/getProductType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{Products_ID:\"" + strProductID + "\"}",
                    success: function (type) {
                        Ptype = type.d;
                        $('#<%=hdfProductType.ClientID %>').val(Ptype);
                        if (Ptype == "") {
                            jAlert("No Warehouse or Batch or Serial is actived !.");
                        } else {
                            //alert(Ptype);
                            if (Ptype == "W") {
                                $('#<%=hdnisserial.ClientID %>').val("false");
                                $('#<%=hdnisbatch.ClientID %>').val("false");
                                $('#<%=hdniswarehouse.ClientID %>').val("true");
                                //cGrdWarehousePC.PerformCallback('BindWarehouse');

                            }

                            else if (Ptype == "B") {
                                $('#<%=hdnisserial.ClientID %>').val("false");
                                $('#<%=hdnisbatch.ClientID %>').val("false");
                                $('#<%=hdniswarehouse.ClientID %>').val("false");

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
                            }
                            else if (Ptype == "WS") {
                                $('#<%=hdnisserial.ClientID %>').val("true");
                                $('#<%=hdnisbatch.ClientID %>').val("false");
                                $('#<%=hdniswarehouse.ClientID %>').val("true");
                                //cCmbWarehouse.PerformCallback('BindWarehouse');
                                //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                            }
                            else if (Ptype == "WBS") {
                                $('#<%=hdnisserial.ClientID %>').val("true");
                                $('#<%=hdnisbatch.ClientID %>').val("true");
                                $('#<%=hdniswarehouse.ClientID %>').val("true");
                                //cCmbWarehouse.PerformCallback('BindWarehouse');
                                //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                            }
                            else if (Ptype == "BS") {
                                $('#<%=hdnisserial.ClientID %>').val("true");
                                $('#<%=hdnisbatch.ClientID %>').val("true");
                                $('#<%=hdniswarehouse.ClientID %>').val("false");

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

                            $('#<%=hdfstockidPC.ClientID %>').val(0);
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

                            var stockids = SpliteDetails[10];
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

                            $('#<%=hdfstockidPC.ClientID %>').val(stockids);
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

                            $("#lblopeningstock").text(dtd + " " + strUOM);

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
}

function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
}

function OnWarehouseEndCallback(s, e) {
    var Ptype = document.getElementById('hdfProductType').value;

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 5);
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Sales Quantity must be equal to Warehouse Quantity.');
    }
    else {
        if (document.getElementById("myCheck").checked == true) {
            if (IsPostBack == "N") {
                checkListBox.PerformCallback('BindSerial~' + PBWarehouseID + '~' + PBBatchID);

                IsPostBack = "";
                PBWarehouseID = "";
                PBBatchID = "";
            }

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
}

var SelectWarehouse = "0";
var SelectBatch = "0";
var SelectSerial = "0";
var SelectedWarehouseID = "0";



<%--function acpAvailableStockEndCall(s, e) {
    if (cacpAvailableStock.cpstock != null) {
        divAvailableStk.style.display = "block";
      //  divpopupAvailableStock.style.display = "block";

        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
        document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = AvlStk;
 
        cCmbWarehouse.cpstock = null;
    }
}--%>


        function ctaxUpdatePanelEndCall(s, e) {

            //  alert('jjj');
            //debugger;
            //console.log(ctaxUpdatePanel.cpstock);
            if (ctaxUpdatePanel.cpstock != null) {

                //kaushik 21-4-2017
                divAvailableStk.style.display = "block";
                //  divpopupAvailableStock.style.display = "block";
                //kaushik 21-4-2017
                var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;

                ctaxUpdatePanel.cpstock = null;

            }

            if (fromColumn == 'product') {
                grid.batchEditApi.StartEdit(globalRowIndex, 7);
                fromColumn = '';
            }
            return;
        }
<%--function ctaxUpdatePanelEndCall(s, e) {
    if (ctaxUpdatePanel.cpstock != null) {
        divAvailableStk.style.display = "block";
       // divpopupAvailableStock.style.display = "block";

        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
        document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = AvlStk;
     
        ctaxUpdatePanel.cpstock = null;
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
        return false;
    }
}--%>

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
            if (SelectSerial != "0") {
                var values = [SelectSerial];
                checkListBox.SelectValues(values);
                UpdateSelectAllItemState();
                UpdateText();
                //checkListBox.SetValue(SelectWarehouse);
                SelectSerial = "0";
                cCmbBatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
            }
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

            //kaushik 29-7-2017
            ctxtProductAmount.SetValue(parseFloat(Math.round(sumAmount * 100) / 100).toFixed(2));
            ctxtProductTaxAmount.SetValue(parseFloat(Math.round(sumTaxAmount * 100) / 100).toFixed(2));
            ctxtProductDiscount.SetValue(parseFloat(Math.round(sumDiscount * 100) / 100).toFixed(2));
            ctxtProductNetAmount.SetValue(parseFloat(Math.round(sumNetAmount * 100) / 100).toFixed(2));
            //ctxtProductAmount.SetValue(Math.round(sumAmount).toFixed(2));
            //ctxtProductTaxAmount.SetValue(Math.round(sumTaxAmount).toFixed(2));
            //ctxtProductDiscount.SetValue(Math.round(sumDiscount).toFixed(2));
            //ctxtProductNetAmount.SetValue(Math.round(sumNetAmount).toFixed(2));
            clblChargesTaxableGross.SetText("");
            clblChargesTaxableNet.SetText("");

            //Checking is gstcstvat will be hidden or not
            if (cddl_AmountAre.GetValue() == "1") {

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
            else if (cddl_AmountAre.GetValue() == "2") {
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

        //Set Running Total for Charges And Tax 
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

        /////////////////// QuotationTaxAmountTextChange By Sam on 23022017
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

        ////////////

        var AmountOldValue;
        var AmountNewValue;

        function AmountTextChange(s, e) {
            AmountLostFocus(s, e);
            var RecieveValue = (grid.GetEditor('Amount').GetValue() != null) ? parseFloat(grid.GetEditor('Amount').GetValue()) : "0";
        }

        function AmountLostFocus(s, e) {
            AmountNewValue = s.GetText();
            var indx = AmountNewValue.indexOf(',');

            if (indx != -1) {
                AmountNewValue = AmountNewValue.replace(/,/g, '');
            }
            if (AmountOldValue != AmountNewValue) {
                changeReciptTotalSummary();
            }
        }

        function AmountGotFocus(s, e) {
            AmountOldValue = s.GetText();
            var indx = AmountOldValue.indexOf(',');
            if (indx != -1) {
                AmountOldValue = AmountOldValue.replace(/,/g, '');
            }
        }

        function changeReciptTotalSummary() {
            var newDif = AmountOldValue - AmountNewValue;
            var CurrentSum = ctxtSumTotal.GetText();
            var indx = CurrentSum.indexOf(',');
            if (indx != -1) {
                CurrentSum = CurrentSum.replace(/,/g, '');
            }

            ctxtSumTotal.SetValue(parseFloat(CurrentSum - newDif));
        }

        function CmbWarehouse_ValueChange() {
            var WarehouseID = cCmbWarehouse.GetValue();
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS" || type == "WB") {
                cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
            }
            else if (type == "WS") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
            }
        }
        function CmbBatch_ValueChange() {
            var WarehouseID = cCmbWarehouse.GetValue();
            var BatchID = cCmbBatch.GetValue();
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
            }
            else if (type == "BS") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID);
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
                        checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
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
                    checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
                    ctxtQuantity.SetValue("0");
                }
                UpdateText();
                cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
                SelectedWarehouseID = "0";
            }
        }

        var IsPostBack = "";
        var PBWarehouseID = "";
        var PBBatchID = "";


        $(document).ready(function () {
            $('#ddl_VatGstCst_I').blur(function () {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            })
            $('#ddl_AmountAre').blur(function () {
                var id = cddl_AmountAre.GetValue();
                if (id == '1' || id == '3') {
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 2);
                    }
                }
            })


        });

        function deleteAllRows() {
            var frontRow = 0;
            var backRow = -1;
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
                grid.DeleteRow(frontRow);
                grid.DeleteRow(backRow);
                backRow--;
                frontRow++;
            }
            OnAddNewClick();
        }
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

                checkComboBox.Focus();
            }
            else {
                divSingleCombo.style.display = "none";
                divMultipleCombo.style.display = "block";

                ctxtserial.Focus();
            }
        }

        function fn_Deletecity(keyValue) {
            var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
            var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

            cGrdWarehouse.PerformCallback('Delete~' + keyValue);
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
        }
        function fn_Edit(keyValue) {
            //cGrdWarehouse.PerformCallback('EditWarehouse~' + keyValue);
            SelectedWarehouseID = keyValue;
            cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
        }
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
            // var texts = dropDown.GetText().split(textSeparator);
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
    <script>
        function ProductsGotFocus(s, e) {
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

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
            var strProductShortCode = SpliteDetails[14];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            strProductName = strDescription;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
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




        function PsGotFocusFromID(s, e) {

            //debugger;
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            divAvailableStk.style.display = "block";

            var ProductID = (grid.GetEditor('ProductDisID').GetText() != null && grid.GetEditor('ProductDisID').GetText() != "") ? grid.GetEditor('ProductDisID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            //  grid.batchEditApi.StartEdit(globalRowIndex);
            //  grid.GetEditor("ProductID").SetText(LookUpData);
            //  grid.GetEditor("Product").Focus(ProductCode);


            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID);
            }
        }
        function ProductsGotFocusFromID(s, e) {
            //debugger;
            //grid.batchEditApi.StartEdit(globalRowIndex);
            //grid.GetEditor("ProductID").SetText(LookUpData);
            //grid.GetEditor("ProductName").Focus(ProductCode);

            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            divAvailableStk.style.display = "block";

            var ProductdisID = (grid.GetEditor('ProductDisID').GetText() != null && grid.GetEditor('ProductDisID').GetText() != "") ? grid.GetEditor('ProductDisID').GetText() : "0";

            var ProductID = (grid.GetEditor('ProductID').GetText() != null && grid.GetEditor('ProductID').GetText() != "") ? grid.GetEditor('ProductID').GetText() : "0";
            var strProductName = (grid.GetEditor('ProductID').GetText() != null && grid.GetEditor('ProductID').GetText() != "") ? grid.GetEditor('ProductID').GetText() : "0";
            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

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
            var strProductShortCode = SpliteDetails[14];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            strProductName = strDescription;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (ProductID != "0") {

                //console.log('ProductID', ProductID);
                cacpAvailableStock.PerformCallback(strProductID);
            }
            else { cacpAvailableStock.PerformCallback(ProductdisID); }
        }


        function ProductlookUpdisKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProductpopUpdis.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 3);
            }
        }


        function ProductlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }
        }
    </script>

    <script>

        <%-- Unused Error Code-- Please Check Page Fun. fist then Add new Function--%>
        <%-- Already Alt+X Code Exists-> Again someone add Alt+X Code -> Please Check --%>
        //document.onkeydown = function (e) {
        //    if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        //        StopDefaultAction(e);


        //        btnSave_QuoteAddress();
        //        // document.getElementById('Button3').click();

        //        // return false;
        //    }

        //    if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        //        StopDefaultAction(e);


        //        page.SetActiveTabIndex(0);
        //        gridLookup.Focus();
        //        // document.getElementById('Button3').click();

        //        // return false;
        //    }
        //}

    </script>
    <style>
        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .popover {
            z-index: 999999;
            max-width: 350px;
        }

            .popover .popover-title {
                margin-top: 0 !important;
                background: #465b9d;
                color: #fff;
            }

        .pdLeft15 {
            padding-left: 15px;
        }

        .mTop {
            margin-top: 10px;
        }

        .mLeft {
            margin-left: 15px;
        }

        .popover .popover-content {
            min-height: 60px;
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

        .pullleftClass {
            position: absolute;
            right: -3px;
            top: 24px;
        }

        #myCheck {
            transform: translateY(2px);
            -webkit-transform: translateY(2px);
            -moz-transform: translateY(2px);
            margin-right: 5px;
        }
        /*#grid_DXMainTable>tbody>tr> td:last-child {
    display: none !important;
}*/
    </style>
    <%--End Sudip--%>

    <style>
        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .dxeButtonEditClearButton_PlasticBlue {
            display: none;
        }

        .mbot5 .col-md-8 {
            margin-bottom: 5px;
        }

        .validclass {
            position: absolute;
            right: -4px;
            top: 20px;
        }

        .mandt {
            position: absolute;
            right: -18px;
            top: 4px;
        }

        #txtProductAmount, #txtProductTaxAmount, #txtProductDiscount {
            font-weight: bold;
        }

        /*#grid, #grid div {
            width: 100% !important;
        }*/
        .crossBtn {
            cursor: pointer;
        }

        #txtTaxTotAmt input, #txtprodBasicAmt input, #txtGstCstVat input {
            text-align: right;
        }

        #grid .dxgvHSDC > div, #grid .dxgvCSD {
            width: 100% !important;
        }
    </style>


    <%--Batch Product Popup Start--%>

    <script>
        function ProductKeyDown(s, e) {
            console.log(e.htmlEvent.key);
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            //if (e.htmlEvent.key == "Tab") {

            //    s.OnButtonClick(0);
            //}
        }

        function ProductButnClick(s, e) {


            if (e.buttonIndex == 0) {
                var CID = GetObjectID('hdnCustomerId').value;
                if (CID != null && CID != "") {

                    setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

                    $('#txtProdSearch').val('');
                    $('#ProductModel').modal('show');
                }
                else {
                    jAlert("Please Select a Customer", "Alert", function () { ctxtCustName.Focus(); });
                }
            }

        }



        function ProductDisKeyDown(s, e) {
            console.log(e.htmlEvent.key);
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            //if (e.htmlEvent.key == "Tab") {

            //    s.OnButtonClick(0);
            //}
        }

        function ProductDisButnClick(s, e) {

            if (e.buttonIndex == 0) {
                var CID = GetObjectID('hdnCustomerId').value;
                if (CID != null && CID != "") {

                    setTimeout(function () { $("#txtProdDisSearch").focus(); }, 500);

                    $('#txtProdDisSearch').val('');
                    $('#ProductDisModel').modal('show');
                }
                else {
                    jAlert("Please Select a Customer", "Alert", function () { ctxtCustName.Focus(); });
                }
            }

        }


        function SetProduct(Id, Name) {
            $('#ProductModel').modal('hide');

            var LookUpData = Id;
            var ProductCode = Name;

            if (!ProductCode) {
                LookUpData = null;
            }


            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);
            // console.log(LookUpData);
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            cddl_AmountAre.SetEnabled(false);
            cddlPosGstUnReturn.SetEnabled(false);

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

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];

            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            if (strRate == 0) {
                strSalePrice = strSalePrice;
            }
            else {
                strSalePrice = strSalePrice / strRate;
            }

            //tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            // tbSalePrice.SetValue(strSalePrice);
            //if (quote_Id == null) {
            tbSalePrice.SetValue(strSalePrice);
            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");
            //  }
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strDescription);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                      divPacking.style.display = "block";
                  } else {
                      divPacking.style.display = "none";
                  }
            //divPacking.style.display = "none";

            //lblbranchName lblProduct
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");
            //Debjyoti
                  fromColumn = 'product';
                  ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
                  grid.batchEditApi.StartEdit(globalRowIndex, 7);
              }



              function SetDisProduct(Id, Name) {
                  $('#ProductDisModel').modal('hide');

                  var LookUpData = Id;
                  var ProductCode = Name;

                  if (!ProductCode) {
                      LookUpData = null;
                  }

                  grid.batchEditApi.StartEdit(globalRowIndex, 3);
                  var productall = LookUpData.split('||')
                  cddl_AmountAre.SetEnabled(false);
                  var productdsc = productall[0];
                  grid.GetEditor("ProductDisID").SetText(productdsc);
                  grid.GetEditor("Product").SetText(ProductCode);


                  grid.batchEditApi.StartEdit(globalRowIndex, 3);

              }
              function ProductDisSelected(s, e) {
                  //debugger;

                  // var LookUpData = cproductDisLookUp.GetGridView().GetRowKey(cproductDisLookUp.GetGridView().GetFocusedRowIndex());

                  var LookUpData = cproductDisLookUp.GetValue();
                  if (LookUpData == null)
                      return;
                  // var ProductCode = cproductDisLookUp.GetValue();
                  var ProductCode = cproductDisLookUp.GetText();
                  if (!ProductCode) {
                      LookUpData = null;
                  }
                  cProductpopUpdis.Hide();
                  //   grid.batchEditApi.StartEdit(globalRowIndex);
                  grid.batchEditApi.StartEdit(globalRowIndex, 3);
                  var productall = LookUpData.split('||')

                  var productdsc = productall[0];
                  grid.GetEditor("ProductDisID").SetText(productdsc);
                  grid.GetEditor("Product").SetText(ProductCode);

                  // grid.batchEditApi.StartEdit(-1, 3);
                  //grid.batchEditApi.EndEdit();
                  //grid.batchEditApi.StartEdit(globalRowIndex, 3);
                  //return;

                  //  fromColumn = 'productdis';


                  // if (fromColumn == 'productdis') {
                  //grid.GetEditor("ProductName").Focus();
                  //grid.batchEditApi.StartEdit(globalRowIndex, 2);
                  // fromColumn = '';
                  //return;
                  //}
                  //  grid.batchEditApi.StartEdit(globalRowIndex, 7);

                  grid.batchEditApi.StartEdit(globalRowIndex, 3);
              }

              function ProductSelected(s, e) {

                  //debugger;
                  //if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                  //    cProductpopUp.Hide();
                  //    grid.batchEditApi.StartEdit(globalRowIndex, 7);
                  //    return;
                  //}

                  //var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());

                  //if (LookUpData == null)
                  //    return;
                  //var ProductCode = cproductLookUp.GetValue();
                  //if (!ProductCode) {
                  //    LookUpData = null;
                  //}

                  if (!cproductLookUp.FindItemByValue(cproductLookUp.GetValue())) {
                      cProductpopUp.Hide();
                      grid.batchEditApi.StartEdit(globalRowIndex, 7);
                      jAlert("Product not Exists.", "Alert", function () { cproductLookUp.SetValue(); cproductLookUp.Focus(); });
                      return;
                  }

                  var LookUpData = cproductLookUp.GetValue();
                  var ProductCode = cproductLookUp.GetText();
                  var quote_Id = gridquotationLookup.GetValue();





                  cProductpopUp.Hide();
                  grid.batchEditApi.StartEdit(globalRowIndex);
                  grid.GetEditor("ProductID").SetText(LookUpData);
                  grid.GetEditor("ProductName").SetText(ProductCode);
                  console.log(LookUpData);
                  $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            cddl_AmountAre.SetEnabled(false);

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

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];

            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            if (strRate == 0) {
                strSalePrice = strSalePrice;
            }
            else {
                strSalePrice = strSalePrice / strRate;
            }

            //tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            // tbSalePrice.SetValue(strSalePrice);
            if (quote_Id == null) {
                tbSalePrice.SetValue(strSalePrice);
                grid.GetEditor("Quantity").SetValue("0.00");
                grid.GetEditor("Discount").SetValue("0.00");
                grid.GetEditor("Amount").SetValue("0.00");
                grid.GetEditor("TaxAmount").SetValue("0.00");
                grid.GetEditor("TotalAmount").SetValue("0.00");
            }
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strDescription);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }
            //divPacking.style.display = "none";

            //lblbranchName lblProduct
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");
            //Debjyoti
            fromColumn = 'product';
            ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
            grid.batchEditApi.StartEdit(globalRowIndex, 7);
        }



        function DateCheck() {

            if (gridquotationLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        // var key = cCustomerComboBox.GetValue();
                        var key = $('#<%=hdnCustomerId.ClientID %>').val();
                        page.SetActiveTabIndex(0);
                        $('.dxeErrorCellSys').addClass('abc');


                        //  var startDate = tstartdate.GetValueString();

                        var startDate = new Date();
                        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                        // cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                        //  var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());


                        //kaushik 20-10-2017
                        //  var key = cCustomerComboBox.GetValue();
                        if (key != null && key != '') {
                            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                        }

                        //kaushik 20-10-2017
                        grid.PerformCallback('GridBlank');
                        gridquotationLookup.gridView.UnselectRows();
                        gridquotationLookup.gridView.Refresh();
                        //cQuotationComponentPanel.PerformCallback('RemoveComponentGridOnSelection');

                        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                     clearTransporter();
                 }
                 ccmbGstCstVat.PerformCallback();
                 ccmbGstCstVatcharge.PerformCallback();
                 ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        // ctxt_InvoiceDate.SetText('');
                 $('#<%=txt_InvoiceDate.ClientID %>').val('');
                        //  OnAddNewClick();
                    }
                });
            }
            else {
                // var startDate = cPLSalesOrderDate.GetValueString();
                // var key = cCustomerComboBox.GetValue();
                var key = $('#<%=hdnCustomerId.ClientID %>').val();
                var startDate = new Date();
                startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                page.SetActiveTabIndex(0);
                $('.dxeErrorCellSys').addClass('abc');
                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                // var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

                //kaushik 20-10-2017
                //  var key = cCustomerComboBox.GetValue();
                if (key != null && key != '') {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                }
                //kaushik 20-10-2017
                // grid.PerformCallback('GridBlank');
                ccmbGstCstVat.PerformCallback();
                ccmbGstCstVatcharge.PerformCallback();
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                page.SetActiveTabIndex(0);
                //  OnAddNewClick();
            }
        }

        var canCallBack = true;

        function AllControlInitilize() {

            // debugger;
            if (canCallBack) {

                grid.AddNewRow();
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                grid.batchEditApi.EndEdit();
                $('#ddl_numberingScheme').focus();
                canCallBack = false;
            }
        }
    </script>
    <style>
        .col-md-2 > label, .col-md-2 > span,
        .col-md-1 > label, .col-md-1 > span {
            margin-top: 8px;
            display: inline-block;
        }
    </style>

    <%--<style>
        .dynamicPopupTbl > tbody > tr > td {
           padding: 0px 3px !important;
           font-size: 14px;
       }

       .dynamicPopupTbl > tr > th {
           height: 28px;
       }

       .dynamicPopupTbl > tbody > tr > td {
           cursor: pointer;
       }

           .dynamicPopupTbl > tbody > tr > td input {
               border: none !important;
               cursor: pointer;
               background: transparent !important;
           }

       .focusrow {
           background-color: #3CA5DF;
           color: #ffffff;
       }

           .focusrow > td input {
               color: white;
           }

       .HeaderStyle {
           background-color: #180771d9;
           color: #f5f5f5;
       }


    </style>--%>

    <script>
        function ValueSelected(e, indexName) {
            // debugger;
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var Name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {

                    if (indexName == "ProdIndex") {
                        SetProduct(Id, Name);
                    }
                    else if (indexName == "ProdDisIndex") {
                        SetDisProduct(Id, Name);
                    }
                    else if (indexName == "customerIndex") {
                        SetCustomer(Id, Name);
                    }
                    else if (indexName == "BillingAreaIndex") {
                        SetBillingArea(Id, name);
                    }
                    else if (indexName == "ShippingAreaIndex") {
                        SetShippingArea(Id, name);
                    }
                    else if (indexName == "customeraddressIndex") {
                        SetCustomeraddress(Id, name)
                    }
                    //if (indexName == "customerIndex") {
                    //    SetCustomer(Id, Name)
                    //}

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
                    if (indexName == "ProdIndex") {
                        $('#txtProdSearch').focus();
                    }
                    else if (indexName == "ProdDisIndex") {
                        $('#txtProdDisSearch').focus();
                    }
                    else if (indexName == "customerIndex") {
                        $('#txtCustSearch').focus();
                    }
                    else if (indexName == "BillingAreaIndex") {
                        $('#txtbillingArea').focus();
                    }
                    else if (indexName == "ShippingAreaIndex") {
                        $('#txtshippingArea').focus();
                    }
                    else if (indexName == "customeraddressIndex") {
                        ('#txtshippingShipToParty').focus();
                    }
                    //if (indexName == "customerIndex")
                    //    $('#txtCustSearch').focus();

                }
            }

        }


    </script>
    <%--Batch Product Popup End--%>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section--%>
    <script>
        function disp_prompt(name) {

            if (name == "tab0") {
                //  gridLookup.Focus();
                ctxtCustName.Focus();
               
                //alert(name);
                //document.location.href = "SalesQuotation.aspx?";
            }
            if (name == "tab1") {
                page.tabs[0].SetEnabled(false);
                $('#divcross').hide();
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

        function CustomerButnClick(s, e) {
            $('#CustModel').modal('show');
            setTimeout(function () { $("#txtCustSearch").focus(); }, 500);

        }
        function CustomerKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#CustModel').modal('show');
                setTimeout(function () { $("#txtCustSearch").focus(); }, 500);

            }
        }

        function Customerkeydown(e) {
            var OtherDetail = {}
            OtherDetail.SearchKey = $("#txtCustSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");

                callonServer("Services/Master.asmx/GetCustomer", OtherDetail, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
        }

        function SetCustomer(Id, Name) {
            if (Id) {
                $('#CustModel').modal('hide');
                ctxtCustName.SetText(Name);

                GetObjectID('hdnCustomerId').value = Id;
                GetObjectID('hdfLookupCustomer').value = Id;

                $('.crossBtn').hide();
                //   page.GetTabByName('General').SetEnabled(false);
                $('#CustModel').modal('hide');
            }


            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
            var branchid = $('#ddl_Branch').val();

            var keyC = $('#<%=hdnCustomerId.ClientID %>').val();
             //  if (keyC != $('#<%=hdnCustomerId.ClientID %>').val()) {

             if (gridquotationLookup.GetValue() != null) {
                 jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

                     if (r == true) {

                         var key = $('#<%=hdnCustomerId.ClientID %>').val();
                            if (key != null && key != '') {
                                gridquotationLookup.gridView.UnselectRows();
                                gridquotationLookup.gridView.Refresh();

                                $('#<%=txt_InvoiceDate.ClientID %>').val('');

                                $('.dxeErrorCellSys').addClass('abc');
                                SetPurchaseBillingShippingAddress($('#ddl_Branch').val());

                                setTimeout(function () {
                                    GetPurchaseForGstValue();

                                }, 500);
                               // LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SR');
                                GetObjectID('hdnCustomerId').value = key;
                                page.tabs[0].SetEnabled(true);
                                page.tabs[1].SetEnabled(true);


                                var startDate = new Date();
                                startDate = tstartdate.GetDate().format('yyyy/MM/dd');

                                var key = $('#<%=hdnCustomerId.ClientID %>').val();
                                if (key != null && key != '') {


                                }


                                if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                                    clearTransporter();
                                }

                                $('#<%=txt_InvoiceDate.ClientID %>').val('');


                            }

                        } else {

                            ctxtCustName.SetValue($('#<%=hdnCustomerId.ClientID %>').val());
                            var customerid = $('#<%=hdnCustomerId.ClientID %>').val();
                            ctxtCustName.PerformCallback(customerid)

                        }
                    });
                }
                else {


                    var key = $('#<%=hdnCustomerId.ClientID %>').val();
                    if (key != null && key != '') {
                        $('#<%=txt_InvoiceDate.ClientID %>').val('');
                        SetPurchaseBillingShippingAddress($('#ddl_Branch').val());

                        setTimeout(function () {
                            GetPurchaseForGstValue();

                        }, 500);
                        //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SR');
                        GetObjectID('hdnCustomerId').value = key;
                        page.tabs[0].SetEnabled(true);
                        page.tabs[1].SetEnabled(true);


                        $('.dxeErrorCellSys').addClass('abc');

                        GetObjectID('hdnAddressDtl').value = '0';
                    }

                }
             // }
            GlobalBillingShippingEndCallBack();
                cContactPerson.Focus();
            }

    </script>
    <script>


        function selectValue() {
            //  debugger;
            var checked = $('#rdl_SalesInvoice').attr('checked', true);
            if (checked) {
                $(this).attr('checked', false);
            }
            else {
                $(this).attr('checked', true);
            }
            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');

            var key = $('#<%=hdnCustomerId.ClientID %>').val();

             var type = ($("[id$='rdl_SalesInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SalesInvoice']").find(":checked").val() : "";

             if (key == null || key == "") {
                 jAlert("Customer required !", 'Alert Dialog: [Sales Invoice]', function (r) {
                     if (r == true) {
                         ctxtCustName.Focus();
                     }
                 });

                 return;

             }

             if (key != null && key != '' && type != "") {
                 cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%');
             }




         }
    </script>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 7px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #ASPxDateEditFrom , #cdDeliveryDate , #dt_PLQuote
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #ASPxDateEditFrom_B-1 , #cdDeliveryDate_B-1 , #dt_PLQuote_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #ASPxDateEditFrom_B-1 #ASPxDateEditFrom_B-1Img , #cdDeliveryDate_B-1 #cdDeliveryDate_B-1Img ,
        #dt_PLQuote_B-1 #dt_PLQuote_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 28px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
                z-index: 0;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        /*input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }*/
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GrdOrder
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        #txtProdSearch
        {
            margin-bottom: 10px;
        }

        select.btn
        {
           position: relative;
           z-index: 0;
        }

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        .mb-10{
            margin-bottom: 10px !important;
        }

        /*.btn
        {
            padding: 5px 10px;
        }*/


        /*Rev end 1.0*/

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="JS/SearchPopup.js"></script>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix">
        <h3 class="pull-left">
            <asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>
            <%--<label>Add Proforma Invoice/ Quotation</label>--%>
        </h3>
        <%-- <div id="div1" runat="server" class="crossBtn"><a href="SalesReturnList.aspx" ><i class="fa fa-times"></i></a></div>--%>


        <div id="pageheaderContent" class="pull-right wrapHolder content horizontal-images" style="display: none;" runat="server">
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
                        <div class="lblHolder" id="divOutstanding" style="display: none;" runat="server">
                            <table>
                                <tr>
                                    <td>Receivable</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOutstanding" runat="server" Text="0.0" CssClass="classout"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>

                    <li>
                        <div class="lblHolder" id="divAvailableStk" style="display: none;">
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

                    <li>
                        <div class="lblHolder" id="divGSTN" style="display: none;" runat="server">
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
                <ul style="display: none;">
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Selected Unit</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbranchName" runat="server"></asp:Label>
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
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
        <div id="divcross" runat="server" class="crossBtn"><a href="UndeliveryReturnList.aspx"><i class="fa fa-times"></i></a></div>

    </div>
        <div class="form_main">
        <asp:Panel ID="pnl_quotation" runat="server">
            <div class="">
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="">
                                        <div style=" padding: 8px 0; margin-bottom: 0px; border-radius: 4px;" class="clearfix col-md-12">
                                            <%--Rev 1.0 : "simple-select" class add--%>
                                            <div class="col-md-2 simple-select" id="divScheme" runat="server">
                                                <%--  <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme"></asp:Label>
                                                <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" TabIndex="1">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lbl_SaleInvoiceNo" runat="server" Text="Document No."></asp:Label>
                                                <%--  <dxe:ASPxLabel ID="lbl_SaleInvoiceNo" runat="server" Text="Return/Credit Note No">
                                                </dxe:ASPxLabel>--%>
                                                <%--  <dxe:ASPxTextBox ID="txt_PLQuoteNo" runat="server" ClientInstanceName="ctxt_PLQuoteNo" TabIndex="2" Width="100%">
                                                    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                                </dxe:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_PLQuoteNo" runat="server" TabIndex="2" Width="100%"></asp:TextBox>

                                                <span id="MandatorysQuoteno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                </span>
                                                <span id="duplicateQuoteno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Duplicate number">
                                                </span>
                                            </div>
                                            <div class="col-md-2">
                                                <%-- <dxe:ASPxLabel ID="lbl_SaleInvoiceDt" runat="server" Text="Date">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_SaleInvoiceDt" runat="server" Text="Posting Date"></asp:Label>
                                                <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate" TabIndex="3" Width="100%">
                                                    <ClientSideEvents DateChanged="function(s, e) {DateCheck();}" />
                                                    <ClientSideEvents GotFocus="function(s,e){tstartdate.ShowDropDown();}"></ClientSideEvents>
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                                <%--Rev 1.0--%>
                                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                <%--Rev end 1.0--%>
                                            </div>
                                            <%--Rev 1.0 : "simple-select" class add--%>
                                            <div class="col-md-2 simple-select">
                                                <%-- <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_Branch" runat="server" Text="Unit"></asp:Label>
                                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="4" onchange="onBranchItems()">
                                                </asp:DropDownList>
                                            </div>

                                            <%--<div class="col-md-2">
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" TabIndex="5" ClientInstanceName="gridLookup"
                                                    KeyFieldName="cnt_internalid" TextFormatString="{1}" AutoGenerateColumns="False" Width="100%"  DataSourceID="dsCustomer">
                                                    <Columns>
                                                        <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="1" Caption="Short Name" Width="150" Settings-AutoFilterCondition="Contains" />
                                                        <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="0" Caption="Name" Settings-AutoFilterCondition="Contains" Width="200">
                                                         
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="2" Caption="Type" Settings-AutoFilterCondition="Contains" Width="150">
                                                          
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="cnt_internalid" Visible="false" VisibleIndex="3" Settings-AllowAutoFilter="False" Width="0">
                                                            <Settings AllowAutoFilter="False"></Settings>
                                                        </dxe:GridViewDataColumn>
                                                    </Columns>
                                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                        <Templates>
                                                            <StatusBar>
                                                                <table class="OptionsTable" style="float: right">
                                                                    <tr>
                                                                        <td>
                                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior ="False"/>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </StatusBar>
                                                        </Templates>
                                                        <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                                                            <FirstPageButton Visible="True">
                                                            </FirstPageButton>
                                                            <LastPageButton Visible="True">
                                                            </LastPageButton>
                                                        </SettingsPager>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                                        <SettingsLoadingPanel Text="Please Wait..." />
                                                        <Settings ShowFilterRow="True" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                    </GridViewProperties>
                                                    <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" GotFocus="function(s,e){gridLookup.ShowDropDown();}" />
                                                    <ClearButton DisplayMode="Auto">
                                                    </ClearButton>
                                                </dxe:ASPxGridLookup>
                                                <span id="MandatorysCustomer1" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>--%>

                                            <%-- kaushik 20-10-2017--%>
                                            <div class="col-md-2">
                                                <%--   <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer (4 Char)">
                                                         </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_Customer" runat="server" Text="Customer"></asp:Label>

                                                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" TabIndex="5">

                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>

                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>




                                                <span id="MandatorysCustomer" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                                            </div>
                                            <%-- kaushik 20-10-2017--%>
                                            <div class="col-md-2">
                                                <%-- <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_ContactPerson" runat="server" Text="Contact Person"></asp:Label>
                                                <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" ClientSideEvents-EndCallback="cmbContactPersonEndCall" TabIndex="6" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">
                                                    <%--  <ClientSideEvents  GotFocus="function(s,e){cQuotationComponentPanel.ShowDropDown();}" ></ClientSideEvents>--%>

                                                    <ClientSideEvents GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />

                                                    <ClientSideEvents TextChanged="function(s, e) { GetContactPersonPhone(e)}" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div style="clear: both"></div>
                                            <%--Rev 1.0 : "simple-select" class add--%>
                                            <div class="col-md-2 simple-select">
                                                <%-- <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="ASPxLabel3" runat="server" Text="Salesman/Agents"></asp:Label>
                                                <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" TabIndex="7">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <%--  <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                                </dxe:ASPxLabel>--%>

                                                <asp:Label ID="lbl_Refference" runat="server" Text="Reference"></asp:Label>
                                                <%--   <dxe:ASPxTextBox ID="txt_Refference" runat="server"  ClientInstanceName="ctxt_Refference"  TabIndex="8" Width="100%">
                                                </dxe:ASPxTextBox>--%>

                                                <asp:TextBox ID="txt_Refference" runat="server" TabIndex="8" Width="100%"></asp:TextBox>
                                            </div>

                                            <div class="col-md-2">

                                                <asp:RadioButtonList ID="rdl_SalesInvoice" runat="server" RepeatDirection="Horizontal" TabIndex="9" onchange="return selectValue();" Width="120px">
                                                    <asp:ListItem Text="Sale Invoice" Value="SIN"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <%-- <asp:Label ID="lbl_invoice_No" runat="server" Text="Sale Invoice" Width="120px"></asp:Label>--%>
                                                <%--  <dxe:ASPxLabel ID="lbl_invoice_No" runat="server" Text="Sale Invoice" Width="120px">
                                                </dxe:ASPxLabel>--%>
                                                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_quotation" runat="server" TabIndex="10" ClientInstanceName="gridquotationLookup"
                                                                OnDataBinding="lookup_quotation_DataBinding"
                                                                KeyFieldName="ComponentID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                <Columns>
                                                                    <%-- <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />--%>
                                                                    <dxe:GridViewDataColumn FieldName="ComponentNumber" Visible="true" VisibleIndex="1" Caption="Document Number" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Posting Date" Width="100" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Customer Name" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="branch" Visible="true" VisibleIndex="4" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="Reference" Visible="true" VisibleIndex="5" Caption="Reference" Width="80" Settings-AutoFilterCondition="Contains" />


                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>

                                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="false" />
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
                                                                <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}" />
                                                            </dxe:ASPxGridLookup>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="componentEndCallBack" />
                                                </dxe:ASPxCallbackPanel>
                                                <span id="MandatorysSCno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor21_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                </span>

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
                                                                <input type="button" disabled="disabled" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                                                                <input type="button" disabled="disabled" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                                                <input type="button" disabled="disabled" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                                                            </div>
                                                            <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                                                                Settings-ShowFooter="false" AutoGenerateColumns="False" OnHtmlRowCreated="aspxGridProduct_HtmlRowCreated"
                                                                ClientSideEvents-EndCallback="cgridproductsEndCall" OnRowInserting="Productgrid_RowInserting" OnRowUpdating="Productgrid_RowUpdating" OnRowDeleting="Productgrid_RowDeleting" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">

                                                                <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                                                <SettingsPager Visible="false"></SettingsPager>
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="45" ReadOnly="true" Caption="Sl. No.">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="gvColProduct" ReadOnly="true" Caption="Product" Width="0">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <%--  <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                                                            </dxe:GridViewDataTextColumn>--%>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Product_Shortname" ReadOnly="true" Width="100" Caption="Product Name">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="gvColDiscription" Width="200" ReadOnly="true" Caption="Product Description">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Quotation_No" ReadOnly="true" Caption="Quotation Id" Width="0">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quotation_Num" Width="90" ReadOnly="true" Caption="Sales Invoice No">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="gvColQuantity" Width="70" VisibleIndex="6">
                                                                        <PropertiesTextEdit>
                                                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                                        </PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                                                    </dxe:GridViewDataTextColumn>
                                                                </Columns>

                                                                <SettingsDataSecurity AllowEdit="true" />

                                                            </dxe:ASPxGridView>
                                                            <div class="text-center" style="padding-top: 8px;">


                                                                <dxe:ASPxButton ID="Button13" ClientInstanceName="cbtn_Button13" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                                    <ClientSideEvents Click="function(s, e) {PerformCallToGridBind();}" />
                                                                </dxe:ASPxButton>

                                                                <%--   <asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />--%>
                                                            </div>
                                                        </dxe:PopupControlContentControl>
                                                    </ContentCollection>
                                                    <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                                                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                                </dxe:ASPxPopupControl>
                                            </div>
                                            <div class="col-md-2">
                                                <%--  <dxe:ASPxLabel ID="lbl_InvoiceNO" runat="server" Text="Sale Invoice Date">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_InvoiceNO" runat="server" Text="Sale Invoice Date"></asp:Label>
                                                <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                                        <PanelCollection>
                                                            <dxe:PanelContent runat="server">
                                                                <asp:TextBox ID="txt_InvoiceDate" runat="server" TabIndex="8" Width="100%" Enabled="false"></asp:TextBox>
                                                                <%-- <dxe:ASPxTextBox ID="txt_InvoiceDate" ClientInstanceName="ctxt_InvoiceDate" runat="server" Width="100%" ClientEnabled="false">
                                                                </dxe:ASPxTextBox>--%>
                                                            </dxe:PanelContent>
                                                        </PanelCollection>
                                                    </dxe:ASPxCallbackPanel>
                                                </div>
                                                <%-- <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                                    <dxe:ASPxTextBox ID="txt_InvoiceDate" runat="server" Width="100%" ReadOnly="true">
                                                    </dxe:ASPxTextBox>
                                                </div>--%>
                                            </div>

                                            <div class="col-md-2" style="display: none">
                                                <%--  <dxe:ASPxLabel ID="lbl_DueDate" runat="server" Text="Due Date">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_DueDate" runat="server" Text="Due Date"></asp:Label>
                                                <dxe:ASPxDateEdit ID="dt_SaleInvoiceDue" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_SaleInvoiceDue" TabIndex="12" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                            <%--Rev 1.0 : "simple-select" class add--%>
                                            <div class="col-md-1 simple-select">
                                                <asp:Label ID="lbl_Currency" runat="server" Text="Currency"></asp:Label>
                                                <%-- <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                                </dxe:ASPxLabel>--%>
                                                <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" TabIndex="13">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:Label ID="lbl_Rate" runat="server" Text="Exch Rate"></asp:Label>
                                                <%--  <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Exch Rate">
                                                </dxe:ASPxLabel>--%>
                                                <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server" TabIndex="14" Width="100%" Height="28px">
                                                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="ReBindGrid_Currency" GotFocus="function(s,e){ctxt_Rate.ShowDropDown();}" />
                                                </dxe:ASPxTextBox>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:Label ID="lbl_AmountAre" runat="server" Text="Amounts are"></asp:Label>
                                                <%--   <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>--%>
                                                <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" TabIndex="15" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                                    <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" GotFocus="function(s,e){cddl_AmountAre.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                            </div>



                                            <div class="clear"></div>
                                            <div class="col-md-6">
                                                <%--  <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Reason For Return" >
                                                </dxe:ASPxLabel>--%>

                                                <asp:Label ID="ASPxLabel4" runat="server" Text="Reason For Return"></asp:Label>
                                                <asp:TextBox ID="txtReasonforChange" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" onblur="return blurOut()"></asp:TextBox>

                                                <%-- <dxe:ASPxMemo ID="txtReasonforChange" runat="server" Width="100%"  MaxLength="500" ClientInstanceName="ctxtReasonforChange" TabIndex="16">  </dxe:ASPxMemo>--%>
                                                <span id="MandatoryReasonforChange" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>

                                             <div class="col-md-2">
                                                <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Place Of Supply[GST]">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                                <dxe:ASPxComboBox ID="ddlPosGstUnReturn" runat="server" ClientInstanceName="cddlPosGstUnReturn" Width="100%" ValueField="System.String">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateUndeliveryReturnPosGst(e)}" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-2 hide">
                                                <%-- <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                                </dxe:ASPxLabel>--%>

                                                <asp:Label ID="lblVatGstCst" runat="server" Text="Select GST"></asp:Label>
                                                <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" OnCallback="ddl_VatGstCst_Callback" TabIndex="16" Width="100%">
                                                    <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" GotFocus="function(s,e){cddlVatGstCst.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                                <span id="Mandatorytaxcode" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <div class="">
                                            <div style="display: none;">
                                                <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                                            </div>
                                            <div>
                                                <br />
                                            </div>
                                            <dxe:ASPxGridView runat="server" KeyFieldName="QuotationID" OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                                OnBatchUpdate="grid_BatchUpdate"
                                                OnCustomCallback="grid_CustomCallback"
                                                OnDataBinding="grid_DataBinding"
                                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                                OnRowInserting="Grid_RowInserting"
                                                OnRowUpdating="Grid_RowUpdating"
                                                OnRowDeleting="Grid_RowDeleting"
                                                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="170">
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" Text="New" ForeColor="White">
                                                                <%--<ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />--%>
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--<dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="ProductID" VisibleIndex="1" Width="15%">
                                                        <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName" DropDownWidth="300">
                                                            <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsGotFocus" />
                                                        </PropertiesComboBox>
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                    <%--CallbackPageSize="10" OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL"
                                                            OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" EnableCallbackMode="true"--%>
                                                    <%-- <dxe:GridViewDataComboBoxColumn FieldName="ProductID" Caption="Product" VisibleIndex="1" Width="15%">
                                                        <PropertiesComboBox TextField="ProductName" ValueField="ProductID">
                                                        </PropertiesComboBox>
                                                        <EditItemTemplate>
                                                            <dxe:ASPxComboBox runat="server" Width="100%" EnableIncrementalFiltering="true" TextField="ProductName" ValueField="ProductID"
                                                                ClearButton-DisplayMode="Always" ID="CmbProduct" ClientInstanceName="cCmbProduct" EnableCallbackMode="true" 
                                                                AllowMouseWheel="false" OnInit="CmbProduct_Init">
                                                                <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsGotFocus" />
                                                            </dxe:ASPxComboBox>
                                                        </EditItemTemplate>
                                                    </dxe:GridViewDataComboBoxColumn>--%>

                                                    <%--Batch Product Popup Start--%>

                                                    <dxe:GridViewDataButtonEditColumn FieldName="Product" Caption="Product" VisibleIndex="2" Width="14%" ReadOnly="true">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductDisButnClick" KeyDown="ProductDisKeyDown" GotFocus="PsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>

                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product Returned" VisibleIndex="3" Width="14%" ReadOnly="true">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" VisibleIndex="23" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ProductDisID" Caption="hidden Field Id" VisibleIndex="21" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Batch Product Popup End--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="1" ReadOnly="True" Width="0" CellStyle-CssClass="hide">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                        <%--<PropertiesTextEdit>
                                                            <ClientSideEvents GotFocus="ProductsGotFocus" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>--%>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="4" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="ProductsGotFocusFromID" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="4" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                            <%--  <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="ProductsGotFocusFromID" />
                                                            <ClientSideEvents />--%>
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityGotFocus" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="5" ReadOnly="true" Width="6%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Caption="Warehouse"--%>
                                                    <%--  <dxe:GridViewCommandColumn VisibleIndex="6" Caption="Stk Details" Width="6%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>--%>
                                                    <dxe:GridViewDataTextColumn FieldName="StockQuantity" Caption="Stock Qty" VisibleIndex="7" Visible="false">
                                                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="StockUOM" Caption="Stock UOM" VisibleIndex="8" ReadOnly="true" Visible="false">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Price" VisibleIndex="9" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <%-- <ClientSideEvents LostFocus="SalePriceTextChange" GotFocus="ProductsGotFocus" />--%>
                                                            <ClientSideEvents LostFocus="SalePriceTextChange" GotFocus="SalePriceGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataSpinEditColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="10" Width="5%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" Style-HorizontalAlign="Right">
                                                            <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                            <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountGotChange" />
                                                        </PropertiesSpinEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataSpinEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="11" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="AmtTextChange" GotFocus="AmtGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="12" Width="6%" HeaderStyle-HorizontalAlign="Right" ReadOnly="True">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                            <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                        </PropertiesButtonEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="13" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <%--<ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Invoice ID" VisibleIndex="16" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="17" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="18" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="19" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="ComponentNumber" ReadOnly="true" Caption="Number" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="14" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                </Columns>
                                                <%--BatchEditEndEditing="OnBatchEditEndEditing"--%>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                                <SettingsBehavior ColumnResizeMode="Disabled" />
                                            </dxe:ASPxGridView>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <br />
                                        <div class="" id="divSubmitButton" runat="server">
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <span id="tdSaveButtonNew" runat="server">
                                                <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </span>
                                             <span id="tdSaveButton" runat="server">
                                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                             </span>

                                          
                                            <dxe:ASPxButton ID="ASPxButton12" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                            </dxe:ASPxButton>
                                            <span id="tdTaxButton" runat="server">      
                                            <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>
                                             </span>
                                            <b><span id="tagged" runat="server" style="display: none; color: red">This Undelivery Return is tagged in other modules. Cannot Modify data</span></b>
                                            <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" Visible="false" />
                                            <asp:HiddenField ID="hfControlData" runat="server" />
                                          
                                        </div>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                         
                        </dxe:TabPage>
                       
                        <dxe:TabPage Name="Billing/Shipping" Text="Our Billing/Shipping" ClientVisible="true">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <ucBS:Purchase_BillingShipping runat="server" ID="Purchase_BillingShipping" />
                                    <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="USR" />
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
            <%--SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">--%>

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

            <%--Sudip--%>
            <div class="PopUpArea">
                <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
                <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
                <%--ChargesTax--%>
                <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
                    Width="900px" Height="300px" HeaderText="Taxes" PopupHorizontalAlign="WindowCenter"
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
                                                                <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableGross"></dxe:ASPxLabel>
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
                                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableNet"></dxe:ASPxLabel>
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

                                <div class="col-md-8" id="ErrorMsgCharges" style="display: none;">
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
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="PercentageTextChange" />
                                                    <ClientSideEvents />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                        <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
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
                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>

                                            </td>
                                            <td style="padding-right: 30px; padding-left: 5px"><strong>Total Amount</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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

            </div>
            <div>
                <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                        </dxe:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
                </dxe:ASPxCallbackPanel>


                <dxe:ASPxCallbackPanel runat="server" ID="acpContactPersonPhone" ClientInstanceName="cacpContactPersonPhone" OnCallback="acpContactPersonPhone_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                        </dxe:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="acpContactPersonPhoneEndCall" />
                </dxe:ASPxCallbackPanel>
                <%----hidden --%>

                <asp:HiddenField ID="hdnisFinancier" runat="server" />
                <asp:HiddenField ID="hdnisOldUnit" runat="server" />
                <asp:HiddenField ID="hdnOldUnitProdInfo" runat="server" />

                <asp:HiddenField ID="hdfProductIDPC" runat="server" />
                <asp:HiddenField ID="hdfstockidPC" runat="server" />
                <asp:HiddenField ID="hdfopeningstockPC" runat="server" />
                <asp:HiddenField ID="hdbranchIDPC" runat="server" />
                <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />



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


                <%---- hidden--%>

                <asp:HiddenField ID="HdUpdateMainGrid" runat="server" />
                <asp:HiddenField ID="hdfIsDelete" runat="server" />
                <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
                <asp:HiddenField ID="hdfProductID" runat="server" />
                <asp:HiddenField ID="hdfProductType" runat="server" />
                <asp:HiddenField ID="hdfProductSerialID" runat="server" />
                <asp:HiddenField ID="hdnProductQuantity" runat="server" />
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hdnDeleteSrlNo" runat="server" />
                <%--Subhra--%>
                <asp:HiddenField ID="hdnInnumber" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnSchemaLength" runat="server" />


                <asp:HiddenField ID="hdnIsInvoiceTax" runat="server" />
            </div>

            <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                <ClientSideEvents ControlsInitialized="AllControlInitilize" />
            </dxe:ASPxGlobalEvents>
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
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="lblHolder">
                                        <table>
                                            <tr>
                                                <td>Selected Unit</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label12" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="col-md-6">
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
                                </div>
                                <div class="col-md-3">
                                    <div class="lblHolder">
                                        <table>
                                            <tr>
                                                <td>Available Stock</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAvailableStk" runat="server" Text="0.0"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="lblHolder">
                                        <table>
                                            <tr>
                                                <td>Entered Stock</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblopeningstock" runat="server" Text="0.0000"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </div>
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
                                            <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary pull-left">
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

                                        <SettingsPager Mode="ShowAllRecords" />
                                        <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" VerticalScrollBarMode="Visible" VerticalScrollableHeight="190" />
                                        <SettingsLoadingPanel Text="Please Wait..." />
                                    </dxe:ASPxGridView>
                                </div>
                                <br />
                                <div class="Center_Content" style="">
                                    <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="S&#818;ave & Exit" AccessKey="S" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {SaveWarehouseAll();}" />
                                    </dxe:ASPxButton>


                                </div>
                            </div>

                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>

            <%--End Sudip--%>

            <asp:HiddenField ID="hdnCustomerId" runat="server" />
            <asp:HiddenField ID="hdnAddressDtl" runat="server" />
            <%--Debu Section--%>

            <%--Batch Product Popup Start--%>

            <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">

                        <label><strong>Search By product Name (4 Char)</strong></label>


                        <dxe:ASPxComboBox ID="productLookUp" runat="server" EnableCallbackMode="true" CallbackPageSize="5"
                            ValueType="System.String" ValueField="Products_ID" ClientInstanceName="cproductLookUp" Width="92%"
                            OnItemsRequestedByFilterCondition="productLookUp_ItemsRequestedByFilterCondition"
                            OnItemRequestedByValue="productLookUp_ItemRequestedByValue" TextFormatString="{0}"
                            DropDownStyle="DropDown" DropDownRows="5" ItemStyle-Wrap="True" FilterMinLength="4">
                            <Columns>
                                <dxe:ListBoxColumn FieldName="Products_Name" Caption="Name" Width="400px" />
                                <dxe:ListBoxColumn FieldName="IsInventory" Caption="Inventory" Width="90px" />
                                <dxe:ListBoxColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="100px" />
                                <dxe:ListBoxColumn FieldName="ClassCode" Caption="Class" Width="100px" />
                                <dxe:ListBoxColumn FieldName="BrandName" Caption="Brand" Width="100px" />
                                <dxe:ListBoxColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="100px" />
                            </Columns>
                            <ClientSideEvents ValueChanged="ProductSelected" KeyDown="ProductlookUpKeyDown" GotFocus="function(s,e){cproductLookUp.ShowDropDown();}" />

                        </dxe:ASPxComboBox>
                        <%--   <label><strong>Search By product Name</strong></label>
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
                                                  
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                        </dxe:ASPxGridLookup>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <dxe:ASPxPopupControl ID="ProductpopUpdis" runat="server" ClientInstanceName="cProductpopUpdis"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By product Name (4 Char)</strong></label>


                        <dxe:ASPxComboBox ID="productDisLookUp" runat="server" EnableCallbackMode="true" CallbackPageSize="5"
                            ValueType="System.String" ValueField="Products_ID" ClientInstanceName="cproductDisLookUp" Width="92%"
                            OnItemsRequestedByFilterCondition="productDisLookUp_ItemsRequestedByFilterCondition"
                            OnItemRequestedByValue="productDisLookUp_ItemRequestedByValue" TextFormatString="{0}"
                            DropDownStyle="DropDown" DropDownRows="5" ItemStyle-Wrap="True" FilterMinLength="4">
                            <Columns>
                                <dxe:ListBoxColumn FieldName="Products_Name" Caption="Name" Width="400px" />
                                <dxe:ListBoxColumn FieldName="IsInventory" Caption="Inventory" Width="90px" />
                                <dxe:ListBoxColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="100px" />
                                <dxe:ListBoxColumn FieldName="ClassCode" Caption="Class" Width="100px" />
                                <dxe:ListBoxColumn FieldName="BrandName" Caption="Brand" Width="100px" />
                                <dxe:ListBoxColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="100px" />
                            </Columns>
                            <ClientSideEvents ValueChanged="ProductDisSelected" KeyDown="ProductlookUpdisKeyDown" GotFocus="function(s,e){cproductDisLookUp.ShowDropDown();}" />

                        </dxe:ASPxComboBox>
                        <%-- <label><strong>Search By product Name</strong></label>
                        <dxe:ASPxGridLookup ID="productDisLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductDisLookUp"
                            KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductDisSelected" ClientSideEvents-KeyDown="ProductlookUpdisKeyDown">
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
                                                   
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                        </dxe:ASPxGridLookup>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <%-- <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="prc_CRMSalesReturn_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                    <asp:SessionParameter Name="campany_Id" SessionField="LastCompanyUSRN" Type="String" />
                    <asp:SessionParameter Type="String" Name="FinYear" SessionField="LastFinYearUSRN" />
                </SelectParameters>
            </asp:SqlDataSource>--%>

            <%--Batch Product Popup End--%>


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
                        <div id="ContentErrorMsg" style="display: none;">
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
                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <%-- <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>
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

            <%--debjyoti 22-12-2016--%>
            <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>

            <asp:HiddenField runat="server" ID="IsUdfpresent" />
            <asp:HiddenField runat="server" ID="Keyval_internalId" />
            <%--End debjyoti 22-12-2016--%>
            <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
            </dxe:ASPxCallbackPanel>

            <%--Debu Section End--%>
        </asp:Panel>
    </div>
    </div>

    <script type="text/javascript">

        function Keypressevt() {

            if (event.keyCode == 13) {

                //run code for Ctrl+X -- ie, Save & Exit! 
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

        }

        function endcallcmware(s) {

            if (cCmbWarehouse.cpstock != null) {

                var ddd = cCmbWarehouse.cpstock + " " + $('#hdnstrUOM').val();
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


        }
        function chnagedbtach(s) {

            $('#<%=hdnoldbatchno.ClientID %>').val(s.GetText());
             $('#<%=hidencountforserial.ClientID %>').val(1);

             var sum = $('#hdnbatchchanged').val();
             sum = Number(Number(sum) + Number(1));

             $('#<%=hdnbatchchanged.ClientID %>').val(sum);

             ctxtexpirdate.SetText("");
             ctxtmkgdate.SetText("");
         }

         function CmbWarehouse_ValueChange(s) {

             var ISupdate = $('#hdnisoldupdate').val();
             var isnewupdate = $('#hdnisnewupdate').val();

             $('#<%=hdnoldwarehousname.ClientID %>').val(s.GetText());

            if (ISupdate == "true" || isnewupdate == "true") {


            } else {

                ctxtserial.SetValue("");

                ctxtbatch.SetEnabled(true);
                ctxtexpirdate.SetEnabled(true);
                ctxtmkgdate.SetEnabled(true);

            }


        }

        function Clraear() {
            ctxtbatch.SetValue("");

            ASPx.CalClearClick('txtmkgdate_DDD_C');
            ASPx.CalClearClick('txtexpirdate_DDD_C');
            $('#<%=hdnisoldupdate.ClientID %>').val("false");

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

                    $('#<%=hdnisoldupdate.ClientID %>').val("false");
                    ctxtqnty.SetText("0.0");
                    ctxtbatch.SetText("");
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);
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

                    $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                     $('#<%=hdnisnewupdate.ClientID %>').val("false");
                     ctxtbatch.SetEnabled(true);
                     cCmbWarehouse.SetEnabled(true);
                     ctxtqnty.SetEnabled(true);
                     ctxtqnty.SetText("0.0");
                     ctxtbatch.SetText("");
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

                     if (Number(qnty) > (Number(enterdqntity) + Number(deletedquantity)) && hdnisediteds == "false") {
                         qnty = "0.00";
                         jAlert("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");


                     }
                     else {


                         cGrdWarehousePC.PerformCallback('Display~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + qnty);

                         cCmbWarehouse.Focus();
                     }
                 }

                 return false;
             }
     }
     function SaveWarehouseAll() {

         cGrdWarehousePC.PerformCallback('Saveall~');

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

             $('#<%=hdnisedited.ClientID %>').val("true");
            cGrdWarehousePC.cpupdateexistingdata = null;
        }
        if (cGrdWarehousePC.cpupdatenewdata != null) {

            $('#<%=hdnisedited.ClientID %>').val("true");

            cGrdWarehousePC.cpupdateexistingdata = null;
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

        grid.batchEditApi.StartEdit(globalRowIndex, 12);
    }

    //Code for UDF Control 
    function OpenUdf() {
        if (document.getElementById('IsUdfpresent').value == '0') {
            jAlert("UDF not define.");
        }
        else {
            var keyVal = document.getElementById('Keyval_internalId').value;
            var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=USR&&KeyVal_InternalID=' + keyVal;
            popup.SetContentUrl(url);
            popup.Show();
        }
        return true;
    }

    function acbpCrpUdfEndCall(s, e) {
        //    LoadingPanel.Hide();
        // debugger;

        // OnAddNewClick();
        if (cacbpCrpUdf.cpUDF) {
            if (cacbpCrpUdf.cpUDF == "true") {

                flag = false;

                var financierval = $('#hdnisFinancier').val();
                var OldUnitval = $('#hdnisOldUnit').val();
                var invoiceId = (gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex()) : "";

                var pType = "<%=Convert.ToString(Session["USRN_ActionType"])%>";

                var IsTaxInvoice = $('#hdnIsInvoiceTax').val();

                var pTaxdetails = '';

                var TaxFlag = '0';

                if ($('#hdnIsInvoiceTax').val() != '2') {

                    $('#hdnIsInvoiceTax').val(cacbpCrpUdf.cpInvoiceTax);
                    pTaxdetails = $('#hdnIsInvoiceTax').val();

                }
                else {
                    pTaxdetails = '';

                }


                if (pTaxdetails != null && pTaxdetails != "" && (pType != 'Edit')) {
                    TaxFlag = '1';

                    jConfirm('Please check Sales Promotion value from Tax and charges and proceed.?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            LoadingPanel.Hide();
                            TaxFlag = '1';
                            Save_TaxesClick();
                        }
                        else {

                            if ((financierval == 'Yes' || OldUnitval == 'Yes') && (pType != 'Edit')) {
                                LoadingPanel.Hide();
                                if (financierval == 'Yes' && OldUnitval == 'Yes') {

                                    jConfirm('Finance file sent?', 'Confirmation Dialog', function (r) {
                                        if (r == true) {
                                            $('#hdnisFinancier').val('No');
                                            grid.UpdateEdit();


                                        }
                                        else {
                                            $('#hdnisFinancier').val('Yes');
                                            grid.UpdateEdit();

                                        }
                                    });
                                }
                                else if (financierval == 'Yes' && OldUnitval == 'No') {
                                    jConfirm('Finance file sent?', 'Confirmation Dialog', function (r) {
                                        if (r == true) {

                                            $('#hdnisFinancier').val('No');
                                            $('#hdnisOldUnit').val('No');
                                            grid.UpdateEdit();
                                        }
                                        else {


                                            grid.UpdateEdit();

                                        }
                                    });
                                }

                                else {

                                    grid.UpdateEdit();

                                }

                            }

                            else {
                                grid.UpdateEdit();
                            }
                        }
                    });

                }


                if (TaxFlag == '0') {

                    if ((financierval == 'Yes' || OldUnitval == 'Yes') && (pType != 'Edit')) {
                        LoadingPanel.Hide();
                        if (financierval == 'Yes' && OldUnitval == 'Yes') {

                            jConfirm('Finance file sent?', 'Confirmation Dialog', function (r) {
                                if (r == true) {
                                    $('#hdnisFinancier').val('No');
                                    grid.UpdateEdit();

                                }
                                else {
                                    $('#hdnisFinancier').val('Yes');
                                    grid.UpdateEdit();

                                }
                            });
                        }
                        else if (financierval == 'Yes' && OldUnitval == 'No') {
                            jConfirm('Finance file sent?', 'Confirmation Dialog', function (r) {
                                if (r == true) {

                                    $('#hdnisFinancier').val('No');
                                    $('#hdnisOldUnit').val('No');
                                    grid.UpdateEdit();
                                }
                                else {


                                    grid.UpdateEdit();

                                }
                            });
                        }

                        else {
                            grid.UpdateEdit();

                        }

                    }

                    else {
                        grid.UpdateEdit();
                    }


                }

                cacbpCrpUdf.cpUDF = null;
                cacbpCrpUdf.cacbpCrpUdf = null;

            }
            else {
                LoadingPanel.Hide();
                jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
                cacbpCrpUdf.cpUDF = null;
                cacbpCrpUdf.cacbpCrpUdf = null;
                isudf = '1';
            }

        }
    }

    function blurOut() {
        //if (grid.GetVisibleRowsOnPage() == 1) {

        //    grid.batchEditApi.StartEdit(-1, 2);
        //}
        grid.batchEditApi.StartEdit(0, 2);

        //alert("uuu");
    }
    </script>

    <script>
        function prodkeydown(e) {


            //Both-->B;Inventory Item-->Y;Capital Goods-->C
            // var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            //  OtherDetails.InventoryType = inventoryType;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");


                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetSalesReturnProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
        }


        function prodDiskeydown(e) {


            //Both-->B;Inventory Item-->Y;Capital Goods-->C
            // var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdDisSearch").val();
            //  OtherDetails.InventoryType = inventoryType;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");


                if ($("#txtProdDisSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetSalesReturnProduct", OtherDetails, "ProductDisTable", HeaderCaption, "ProdDisIndex", "SetDisProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdDisIndex=0]"))
                    $("input[ProdDisIndex=0]").focus();
            }
        }
    </script>
    <style>
        .ui-dialog-buttonset #popup_ok {
            background-image: -webkit-linear-gradient(top,#337ab7 0,#265a88 100%);
            background-image: -o-linear-gradient(top,#337ab7 0,#265a88 100%);
            background-image: -webkit-gradient(linear,left top,left bottom,from(#337ab7),to(#265a88));
            background-image: linear-gradient(to bottom,#337ab7 0,#265a88 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ff337ab7', endColorstr='#ff265a88', GradientType=0);
            filter: progid:DXImageTransform.Microsoft.gradient(enabled=false);
            background-repeat: repeat-x;
            border-color: #245580;
            background-color: #138dcc !important;
            border-color: #167db2 !important;
            background-image: none !important;
            border-width: 1px;
            padding: 4px 10px;
            font-size: 14px !important;
            margin-bottom: 5px;
            margin-right: 6px;
            color: #fff;
        }

        .ui-dialog-buttonset #popup_no {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 14px !important;
            margin-bottom: 5px;
            margin-right: 6px;
            background-image: -webkit-linear-gradient(top,#d9534f 0,#c12e2a 100%);
            background-image: -o-linear-gradient(top,#d9534f 0,#c12e2a 100%);
            background-image: -webkit-gradient(linear,left top,left bottom,from(#d9534f),to(#c12e2a));
            background-image: linear-gradient(to bottom,#d9534f 0,#c12e2a 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffd9534f', endColorstr='#ffc12e2a', GradientType=0);
            filter: progid:DXImageTransform.Microsoft.gradient(enabled=false);
            background-repeat: repeat-x;
            border-color: #b92c28;
            color: #fff;
        }
    </style>
    <div style="display: none">
        <dxe:ASPxDateEdit ID="dt_PlQuoteExpiry" runat="server" Date="" Width="100%" EditFormatString="dd-MM-yyyy" ClientInstanceName="tenddate" TabIndex="4">
            <ClientSideEvents DateChanged="Enddate" />
        </dxe:ASPxDateEdit>
    </div>

    <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
    </dxe:ASPxCallbackPanel>

    <%--  <asp:SqlDataSource runat="server" ID="dsCustomer"
        SelectCommand="prc_CRMSalesReturn_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateCustomerDetail" />
        </SelectParameters>
    </asp:SqlDataSource>--%>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmit1Button"
        Modal="True">
    </dxe:ASPxLoadingPanel>



    <dxe:ASPxPopupControl ID="Popup_OldUnit" runat="server" ClientInstanceName="cPopup_OldUnit"
        Width="400px" HeaderText="Old Unit Details List" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">


                <dxe:PanelContent runat="server">

                    <dxe:ASPxGridView ID="AspxOldUnitGrid" ClientInstanceName="cAspxOldUnitGrid" Width="100%" runat="server" OnCustomCallback="AspxOldUnit_CustomCallback"
                        AutoGenerateColumns="False" KeyFieldName="oldUnit_Id" ClientSideEvents-EndCallback="cAspxOldUnitGridEndCall">

                        <Columns>

                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />
                            <dxe:GridViewDataTextColumn Visible="True" FieldName="sProducts_Name" Caption="Product Name">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="oldUnit_qty" Caption="Quantity">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="oldUnit_value" Caption="Value">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="oldUnit_Id" Caption="PID" Width="0px" CellStyle-CssClass="hide" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide" FooterCellStyle-CssClass="hide" EditCellStyle-CssClass="hide">
                                <CellStyle CssClass="hide">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>



                        </Columns>
                        <SettingsBehavior AllowSort="false" AllowGroup="false" />
                    </dxe:ASPxGridView>

                    <div style="padding-top: 10px; text-align: center">
                        <dxe:ASPxButton ID="Button113" ClientInstanceName="cbtn_Button113" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {PerformCallToOldUnitGridBind();}" />
                        </dxe:ASPxButton>
                    </div>



                </dxe:PanelContent>

            </dxe:PopupControlContentControl>

        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>

    <%-- <asp:SqlDataSource ID="CustomerDataSource" runat="server"  />--%>


    <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name or Unique Id" />

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
    <!--Product Modal -->
    <div class="modal fade" id="ProductModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name or Description" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>

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
    <!--Product Modal -->



    <!--Product Modal Dis-->
    <div class="modal fade" id="ProductDisModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodDiskeydown(event)" id="txtProdDisSearch" autofocus width="100%" placeholder="Search By Product Name or Description" />

                    <div id="ProductDisTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>

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
    <!--Product Modal Dis-->
</asp:Content>
