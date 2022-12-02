<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ProjectIssueMaterials.aspx.cs" Inherits="ERP.OMS.Management.Activities.ProjectIssueMaterials" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Sales_BillingShipping.ascx" TagPrefix="ucBS" TagName="Sales_BillingShipping" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/ucVehicleDriverControl.ascx" TagPrefix="uc1" TagName="ucVehicleDriverControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="http://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <script src="http://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script type='text/javascript'>
        var SecondUOM = [];
        var ModuleName = 'MI';
    </script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <%--<script src="JS/SearchPopup.js"></script>--%>
    <script src="JS/SearchPopupDatatable.js"></script>
    <script src="../Activities/JS/ProductStockIN.js?v1.00.00.08"></script>
    <%--  <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>--%>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelNew.js?v=1.0.1" type="text/javascript"></script>
    <script src="JS/ProjectIssueMaterials.js?v=2.18"></script>



    <style type="text/css">
        /*#lookup_Project .dxeButtonEditClearButton_PlasticBlue {
            display: table-cell;
        }*/


        #dataTbl .dataTables_empty {
            display: none;
        }

        .padRight15 {
            padding-right: 15px;
        }

        .padTop23 {
            padding-top: 23px;
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
    <script>
        $(document).ready(function () {
            var setting = document.getElementById("hdnShowUOMConversionInEntry").value;           
            if (setting == 1) {
               
                document.getElementById("_div_Uom").style.display = "block";
                document.getElementById("div_AltQuantity").style.display = "block";
            }
            else {
                document.getElementById("_div_Uom").style.display = "none";
                document.getElementById("div_AltQuantity").style.display = "none";
               
            }

        });
        </script>
    <script type="text/javascript">
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
        function GlobalBillingShippingEndCallBack() {
            if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
                cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
                var startDate = new Date();
                startDate = cPLSalesChallanDate.GetValueString();
                if (gridSalesOrderLookup.GetValue() != null) {

                    //var key = gridLookup.GetValue();
                    var key = $("#<%=hdnCustomerId.ClientID%>").val();
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    if (key != null && key != '') {

                    }

                }
                else {

                    var key = $("#<%=hdnCustomerId.ClientID%>").val();
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    GetObjectID('hdnCustomerId').value = key;
                    if (key != null && key != '') {

                    }
                }

                $('#CustModel').modal('hide');

            }
        }
    </script>
    <script>
        function Onddl_VatGstCstEndCallback(s, e) {
            if (s.GetItemCount() == 1) {
                cddlVatGstCst.SetEnabled(false);
            }
        }
    </script>
    <script>
        function gridFocusedRowChanged(s, e) {

            globalRowIndex = e.visibleIndex;
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

            if (gridSalesOrderLookup.GetValue() != null) {
                grid.AddNewRow();

            }

            if (cacbpCrpUdf.cpUDF) {


                if (cacbpCrpUdf.cpUDF == "true" && cacbpCrpUdf.cpTransport == "true" && cacbpCrpUdf.cpTC == "true") {
                    LoadingPanel.Hide();
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
                    jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
                    LoadingPanel.Hide();
                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                    cacbpCrpUdf.cpTC = null;
                }
                else {

                    grid.UpdateEdit();
                }
            }
        }

    </script>
    <script type="text/javascript">

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

                divpopupAvailableStock.style.display = "block";

                var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;
                document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;

                ctaxUpdatePanel.cpstock = null;
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return false;
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
                    url: "ProjectIssueMaterials.aspx/GetRate",
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

            if (gridTax.cpJsonChargeData) {
                if (gridTax.cpJsonChargeData != "") {
                    chargejsonTax = JSON.parse(gridTax.cpJsonChargeData);
                    gridTax.cpJsonChargeData = null;
                }
            }
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

        var taxJson;
        function cgridTax_EndCallBack(s, e) {

            $('.cgridTaxClass').show();
            cgridTax.StartEditRow(0);
            if (cgridTax.cpJsonData) {
                if (cgridTax.cpJsonData != "") {
                    taxJson = JSON.parse(cgridTax.cpJsonData);
                    cgridTax.cpJsonData = null;
                }
            }

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

                var totalNetAmount = DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2);
                grid.GetEditor("TotalAmount").SetValue(totalNetAmount);

            }

            if (cgridTax.GetVisibleRowsOnPage() == 0) {
                $('.cgridTaxClass').hide();
                ccmbGstCstVat.Focus();
            }

            SetRunningTotal();
            ShowTaxPopUp("IY");
            RecalCulateTaxTotalAmountInline();
        }

    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            var IsEditMode = '<%= Session["ActionType"]%>';
            if (IsEditMode.trim() != 'Add') {
                page.SetActiveTabIndex(0);
                page.tabs[1].SetEnabled(false);
            }


            var CustomerDelivery = $("#<%=hddnCustomerDelivery.ClientID%>").val();
            var ddl_numbering = $('#<%=ddl_numberingScheme.ClientID%>').val();
            var BillValue = $("#<%=hddnBillId.ClientID%>").val();

            if (CustomerDelivery == 'Yes') {

                LoadingPanel.Show();

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

                        ctxt_SlChallanNo.Focus();

                    }

                    $("#<%=ddl_Branch.ClientID%>").val(BranchIdPending);
                    $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);
                    BSDocTagging(BillValue, 'SI');
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
                            deleteTax('DeleteAllTax', "", "")

                            startDate = cPLSalesChallanDate.GetValueString();

                            var key = gridLookup.GetValue();
                            cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                            if (key != null && key != '') {
                                if (type != '' && type != null) {
                                    cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                                }

                            }

                            deleteAllRows();
                            grid.AddNewRow();
                            grid.GetEditor('SrlNo').SetValue('1');
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

                    deleteTax('DeleteAllTax', "", "")

                    startDate = cPLSalesChallanDate.GetValueString();

                    var key = gridLookup.GetValue();
                    cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    if (key != null && key != '') {
                        if (type != '' && type != null) {
                            cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                        }
                    }

                    deleteAllRows();
                    grid.AddNewRow();
                    grid.GetEditor('SrlNo').SetValue('1');
                }
            });

            $('#ddl_numberingScheme').change(function () {

                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var BranchId = NoSchemeTypedtl.toString().split('~')[3];
                //Cut Off  Valid from To Date Sudip

                var fromdate = NoSchemeTypedtl.toString().split('~')[5];
                var todate = NoSchemeTypedtl.toString().split('~')[6];
                // alert(fromdate + '   ' + todate);
                var dt = new Date();
                cPLSalesChallanDate.SetDate(dt);


                if (dt < new Date(fromdate)) {
                    cPLSalesChallanDate.SetDate(new Date(fromdate));
                }

                if (dt > new Date(todate)) {
                    cPLSalesChallanDate.SetDate(new Date(todate));
                }
                cPLSalesChallanDate.SetMinDate(new Date(fromdate));
                cPLSalesChallanDate.SetMaxDate(new Date(todate));

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

                $("#<%=ddl_Branch.ClientID%>").val(BranchId);
                $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);

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

    </script>
    <script>

        function cgridProducts_EndCallBack(s, e) {

            page.tabs[1].SetEnabled(false);
        }

        function OnEndCallback(s, e) {

            LoadingPanel.Hide();

            var ActionType = '<%=Session["ActionType"]%>';
            if (ActionType.trim() == "Edit") {
                grid.StartEditRow(0);
            }

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
                //debugger;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Cannot select same product in multiple rows.');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please select project.');
                grid.cpSaveSuccessOrFail = '';
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
                //debugger;
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

                var SrlNo = grid.cpProductSrlIDCheck;
                var msg = "Qty is entered for product [" + SrlNo + "] but Stock Details not updated.Cannot proceed.";
                jAlert(msg);
                grid.cpSaveSuccessOrFail = null;
                grid.cpProductSrlIDCheck = null;
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouseQty") {
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
                var SalesOrder_Number = grid.cpSalesOrderNo;
                var Order_Msg = "Issue Materials No. " + SalesOrder_Number + " saved.";
                if (value == "E") {
                    $('#<%=hdnRefreshType.ClientID %>').val('');
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else if (grid.cpApproverStatus == "rejected") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }

                    if (SalesOrder_Number != "") {
                        var ODSD = $("#<%=hddnCustomerDeliverySDOrOD.ClientID%>").val();
                        if (grid.cpSalesOrderExitOnCustomerDelivery == "CustomerDelivery") {

                            //jAlert(Order_Msg);
                            grid.cpSalesOrderExitOnCustomerDelivery = null;

                            if (ODSD == "0") {

                                jAlert(Order_Msg, 'Alert Dialog: [Customer Delivery OD]', function (r) {
                                    if (r == true) {
                                        grid.cpSalesOrderNo = null;
                                        window.location.assign("CustomerDeliveryPendingListEntity.aspx");
                                    }
                                });
                            }
                            else if (ODSD == "1") {
                                jAlert(Order_Msg, 'Alert Dialog: [Customer Delivery SD]', function (r) {
                                    if (r == true) {
                                        grid.cpSalesOrderNo = null;
                                        window.location.assign("CustomerDeliveryPendingListEntity.aspx?type=SD");
                                    }
                                });
                            }



                            //AutoPrint
                            if ($("#<%=hddnCustomerDelivery.ClientID%>").val() == "Yes") {
                                if ($("#<%=hddnSaveOrExitButton.ClientID%>").val() == 'Save_Exit') {
                                    var DocumentNo = grid.cpDocumentNo;
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ProjectIssueMaterials~D&modulename=ODSDChallan&id=" + DocumentNo, '_blank');
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
                            jAlert(Order_Msg, 'Alert Dialog: [ProjectIssueMaterials]', function (r) {
                                if (r == true) {
                                    grid.cpSalesOrderNo = null;
                                    window.location.assign("ProjectIssueMaterialsList.aspx");
                                }
                            });
                        }
                    }
                    else {
                        window.location.assign("ProjectIssueMaterialsList.aspx");
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

                    if (SalesOrder_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [ProjectIssueMaterials]', function (r) {
                            //jAlert(Order_Msg);
                            grid.cpSalesOrderNo = null;
                            if (r == true) {
                                window.location.assign("ProjectIssueMaterials.aspx?key=ADD");
                            }
                        });
                    }
                    else {
                        window.location.assign("ProjectIssueMaterials.aspx?key=ADD");
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
                    else if (pageStatus == "delete") {
                        $('#<%=hdnPageStatus.ClientID %>').val('');

                        OnAddNewClick();
                    }
}

}
    var hddnPermission = $("#<%=hddnPermissionString.ClientID%>").val();
            if (gridSalesOrderLookup.GetValue() != null) {
                if (hddnPermission == "0") {
                    grid.GetEditor('ProductName').SetEnabled(false);
                    grid.GetEditor('Description').SetEnabled(false);
                    grid.GetEditor('Order_Num').SetEnabled(false);
                    grid.GetEditor('SalePrice').SetEnabled(false);
                    grid.GetEditor('Discount').SetEnabled(false);
                    grid.GetEditor('TaxAmount').SetEnabled(false);
                    grid.GetEditor('Quantity').SetEnabled(false);
                }
                else if (hddnPermission.trim() == "") {
                    grid.GetEditor('ProductName').SetEnabled(true);
                    grid.GetEditor('Description').SetEnabled(true);
                    grid.GetEditor('Order_Num').SetEnabled(true);
                    grid.GetEditor('SalePrice').SetEnabled(true);//Added on 07-06-2017
                    grid.GetEditor('Discount').SetEnabled(true);//Added on 07-06-2017
                    grid.GetEditor('TaxAmount').SetEnabled(true);//Added on 07-06-2017
                    grid.GetEditor('Quantity').SetEnabled(true);
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
                else {
                    grid.GetEditor('ProductName').SetEnabled(false);
                    grid.GetEditor('Description').SetEnabled(false);
                    grid.GetEditor('Order_Num').SetEnabled(false);
                    //grid.GetEditor('SalePrice').SetEnabled(false);//Added on 07-06-2017
                    grid.GetEditor('Discount').SetEnabled(false);//Added on 07-06-2017
                    grid.GetEditor('Quantity').SetEnabled(false);
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

            var key = cddl_AmountAre.GetValue();
            if (key == 3) {
                grid.GetEditor('TaxAmount').SetEnabled(false);
            }

            cProductsPopup.Hide();
            return false;
        }



        var Warehouseindex;
        function OnCustomButtonClick(s, e) {

            if (e.buttonID == 'CustomDelete') {
                grid.batchEditApi.EndEdit();
                $('#<%=hdnRefreshType.ClientID %>').val('');
                var noofvisiblerows = grid.GetVisibleRowsOnPage();

                grid.DeleteRow(e.visibleIndex);

                $('#<%=hdfIsDelete.ClientID %>').val('D');
                grid.UpdateEdit();
                grid.PerformCallback('Display');
                grid.batchEditApi.StartEdit(-1, 2);
                grid.batchEditApi.StartEdit(0, 2);
                $('#<%=hdnPageStatus.ClientID %>').val('delete');

            }
            else if (e.buttonID == "addlDesc") {

                var index = e.visibleIndex;
                grid.batchEditApi.StartEdit(e.visibleIndex, 6);
                cPopup_InlineRemarks.Show();

                $("#txtInlineRemarks").val('');

                var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                if (ProductID != "") {
                    // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
                    // ccallback_InlineRemarks.PerformCallback('DisplayRemarks' + '~' + SrlNo + '~' + '0');
                    InlineRemarksAddEdit("DisplayRemarks", SrlNo, "");
                }
                else {
                    $("#txtInlineRemarks").val('');
                }
                //$("#txtInlineRemarks").focus();
                document.getElementById("txtInlineRemarks").focus();
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
                //debugger;
                $("#<%=hddnIsODSDFirstTime.ClientID%>").val("1");
                    var index = e.visibleIndex;
                    grid.batchEditApi.StartEdit(index, 2)
                    Warehouseindex = index;
                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                    //var StkQuantityValue = (grid.GetEditor('StockQuantity').GetValue() != null) ? grid.GetEditor('StockQuantity').GetValue() : "0";
                    var IsExits = true;
                    $("#spnCmbWarehouse").hide();
                    $("#spnCmbBatch").hide();
                    $("#spncheckComboBox").hide();
                    $("#spntxtQuantity").hide();
                    var LastFinYear = $('#<%=LastFinancialYear.ClientID %>').val();
                    var LastCompany = $('#<%=LastCompany.ClientID %>').val();
                    var Branch = $('#<%=ddl_Branch.ClientID %>').val();
                    //For Avialable stock
                    var data = '';
                    var ActionTypeL = '<%= Session["ActionType"] %>';
                    if (ActionTypeL != 'Edit') {

                    }

                    if (ProductID != "" && parseFloat(QuantityValue) != 0) {
                        var SpliteDetails = ProductID.split("||@||");
                        var strProductID = SpliteDetails[0];
                        var strDescription = SpliteDetails[1];
                        var strUOM = SpliteDetails[2];
                        var strStkUOM = SpliteDetails[4];
                        var strMultiplier = SpliteDetails[7];
                        var strProductName = strDescription;
                        var StkQuantityValue = QuantityValue * strMultiplier;
                        var Ptype = SpliteDetails[14];
                        $('#<%=hdfProductType.ClientID %>').val(Ptype);

                    $.ajax({
                        type: "POST",
                        url: "ProjectIssueMaterials.aspx/GetConfigSettingRights",
                        data: JSON.stringify({ VariableName: 'IsFIFOExistsInOutModule' }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var data = msg.d;
                            $("#<%=hddnConfigVariable_Val.ClientID%>").val(data);
                            if (data == "1") {

                                if (Ptype == "W") {
                                    div_QtyMatch.style.display = 'none';
                                }
                                else if (Ptype == "B") {
                                    div_QtyMatch.style.display = 'none';
                                }
                                else if (Ptype == "S") {
                                    div_QtyMatch.style.display = 'block';
                                }
                                else if (Ptype == "WB") {
                                    div_QtyMatch.style.display = 'none';
                                }
                                else if (Ptype == "WS") {
                                    div_QtyMatch.style.display = 'block';
                                }
                                else if (Ptype == "WBS") {
                                    div_QtyMatch.style.display = 'block';
                                }
                                else if (Ptype == "BS") {
                                    div_QtyMatch.style.display = 'block';
                                }
                            }
                            else {
                                div_QtyMatch.style.display = 'none';
                            }
                        }
                    });

                    document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
                    document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
                    document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
                    document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
                    document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;

                    $('#<%=hdfProductID.ClientID %>').val(strProductID);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);

                    SecondUOMProductId = strProductID;

                    if (Ptype == "W") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';

                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "B") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';

                        cCmbBatch.PerformCallback('BindBatch~' + "0");
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "S") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'block';
                        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WB") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';

                        cCmbWarehouse.PerformCallback('BindWarehouse');

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WS") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';

                        cCmbWarehouse.PerformCallback('BindWarehouse');

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WBS") {

                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';

                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        checkListBox.PerformCallback('BindSerialAfterCross~');
                        checkComboBox.SetText(0 + " Items");

                        SelectedWarehouseID = "0";
                        $("#ADelete").css("display", "none");
                        cPopup_Warehouse.Show();

                    }
                    else if (Ptype == "BS") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';

                        cCmbBatch.PerformCallback('BindBatch~' + "0");
                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else {

                        jAlert("No Warehouse or Batch or Serial is actived.");
                    }
                    if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                        var objectToPass = {}
                        var product = $("#hdfProductID").val();
                        objectToPass.ProductID = $("#hdfProductID").val();
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
                else if (ProductID != "" && parseFloat(QuantityValue) == 0) {

                    jAlert('Qty is ZERO. Cannot select Stk Details');
                }
                else if (ProductID != "" && parseFloat(QuantityValue) != 0 && IsExits == false) {
                    jAlert("Available stock of the selected product is ZERO(0). Cannot proceed.", "Stock Alert");
                    //['" + ProductID.split("||@||")[1] + "']

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

function callback_InlineRemarks_EndCall(s, e) {

    //if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
    if (InlineRemarksmsg == "DisplayRemarksFocus") {
        $("#txtInlineRemarks").focus();
    }
    else {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    }
}


function OnWarehouseEndCallback(s, e) {
    //debugger;
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
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 10);
        }, 200)
        grid.batchEditApi.StartEdit(globalRowIndex, 10);
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
    }
    else {
    }
}






function PopulateSerial() {

    //Serail Bind:Start

    //End
    //debugger;
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

        #productLookUp_DDD_gv_DXMainTable > tbody > tr > td:nth-child(2) {
            display: none;
        }
    </style>
    <script type="text/javascript">

        function PerformCallToGridBind() {
            //debugger;
            if (cgridproducts.GetSelectedRowCount() != 0) {
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                AllowAddressShipToPartyState = false;
                cddl_PosGst.SetEnabled(false);
                if (type != '' && type != null) {
                    grid.PerformCallback('BindGridOnQuotation' + '~' + '@' + '~' + type);
                }
                var OrderIds = gridSalesOrderLookup.GetValue();
                var Key = OrderIds.split(',')[0];
                $.ajax({
                    type: "POST",
                    url: "ProjectIssueMaterials.aspx/GetContactSalesManReference",
                    //data: "{'KeyVal':'" + Key + "'}",
                    data: JSON.stringify({ KeyVal: Key, type: type }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {

                        var currentString = msg.d;
                        //var ContactPersonId = currentString.split('~')[0];
                        var Reference = currentString.split('~')[0];
                        var SalesManId = currentString.split('~')[1];
                        var CurrencyId = currentString.split('~')[2];
                        var SalesManAgent = currentString.split('~')[3];
                        var creditDays = currentString.split('~')[4];
                        var DueDate = currentString.split('~')[5];
                        var EwayBillNumber = currentString.split('~')[6];
                        txt_Refference.SetText(Reference);
                        //<%--$("#<%=ddl_SalesAgent.ClientID%>").val(SalesManId);--%> on 28-12-2017
                        $("#<%=hdnSalesManAgentId.ClientID%>").val(SalesManId);
                        ctxtSalesManAgent.SetText(SalesManAgent.trim());
                        ctxtCreditDays.SetText(creditDays);
                        cdt_SaleInvoiceDue.SetText(DueDate);
                        //GetContactSalesManReference
                        $("#<%=ddl_Currency.ClientID%>").val(CurrencyId);
                        ctxtEWayBillNO.SetValue(EwayBillNumber);

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
                    //Chinmoy added Below line
                    GetDocumentAddress(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
                    // BSDocTagging(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
                }
                if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                    //clookup_Project.gridView.Refresh();
                }
                if (quote_Id.length > 0) {
                    BindOrderProjectdata(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
                }


                if ($("#btn_TermsCondition").is(":visible")) {
                    callTCControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
                }

                return false;
            }
            else {

                deleteAllRows();
                grid.AddNewRow();
                grid.GetEditor('SrlNo').SetValue('1');

                //  deleteTax('DeleteAllTax', "", "")
                $('input[type=radio]').prop('checked', false);
                gridSalesOrderLookup.SetEnabled(false);
                return false;
            }
        }

        function componentEndCallBack(s, e) {
            //gridSalesOrderLookup.gridView.Refresh();
            if (grid.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick();
            }
        }

    </script>
    <script>
        function acpAvailableStockEndCall(s, e) {
            if (cacpAvailableStock.cpstock != null) {
                var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                 <%--document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = AvlStk;--%>
                document.getElementById('<%=lblAvailableSStk.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cacpAvailableStock.cpstock;
                document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                cCmbWarehouse.cpstock = null;

                //grid.batchEditApi.StartEdit(globalRowIndex, 5);
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
                var AltQty = cCallbackPanel.cpEdit.split('~')[4];
                var AltUOM = cCallbackPanel.cpEdit.split('~')[5];

                SelectWarehouse = strWarehouse;
                SelectBatch = strBatchID;
                SelectSerial = strSrlID;

                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
                checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

                cCmbWarehouse.SetValue(strWarehouse);
                ctxtQuantity.SetValue(strQuantity);
                if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                    CtxtPacking.SetValue(AltQty);
                    ccmbPackingUom1.SetValue(AltUOM);
                }
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
                                <td>2nd Quantity</td>
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

        <div id="crossBtnId" class="crossBtn" runat="server"><a href="ProjectIssueMaterialsList.aspx"><i class="fa fa-times"></i></a></div>
        <div id="crossBtnCustDeliveryListId" class="crossBtn" runat="server"><a href="CustomerDeliveryPendingListEntity.aspx"><i class="fa fa-times"></i></a></div>
        <div id="crossBtnCustDeliveryListForSD" class="crossBtn" runat="server"><a href="CustomerDeliveryPendingListEntity.aspx?type=SD"><i class="fa fa-times"></i></a></div>
        <div id="crossBtnPendingDeliveryListId" class="crossBtn" runat="server"><a href="PendingDeliveryList.aspx?key=reten"><i class="fa fa-times"></i></a></div>
        <div id="crossBtnPendingSecondHand" class="crossBtn" runat="server"><a href="OldUnit_SalesInvoiceList.aspx"><i class="fa fa-times"></i></a></div>
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <%--<asp:Panel ID="pnl_quotation" runat="server">--%>
        <div class="row">

            <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="98%">
                <TabPages>
                    <dxe:TabPage Name="General" Text="General" TabStyle-CssClass="generalTab">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <div class="row">
                                    <div class="col-md-2 hide" runat="server" id="ddlInventoryId">
                                        <label>
                                            <%--Inventory Item--%>
                                            <asp:Label ID="Label12" runat="server" Text="Inventory Item?" CssClass="newLbl"></asp:Label>
                                        </label>
                                        <div class="Left_Content">
                                            <asp:DropDownList ID="ddlInventory" CssClass="backSelect" runat="server" Width="100%" onchange="ddlInventory_OnChange()">
                                                <asp:ListItem Value="Y">Inventory Items</asp:ListItem>

                                            </asp:DropDownList>

                                        </div>
                                    </div>

                                    <div class="col-md-3" id="ddl_numberingDiv" runat="server">

                                        <label>
                                            <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                            </dxe:ASPxLabel>
                                        </label>
                                        <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                                        </asp:DropDownList>


                                    </div>

                                    <div class="col-md-3">
                                        <label>
                                            <dxe:ASPxLabel ID="lbl_SlOrderNo" runat="server" Text="Document No." Width="">
                                            </dxe:ASPxLabel>
                                            <span style="color: red">*</span>
                                        </label>

                                        <dxe:ASPxTextBox ID="txt_SlChallanNo" runat="server" ClientInstanceName="ctxt_SlChallanNo" TabIndex="2" Width="100%" MaxLength="30">
                                            <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                        </dxe:ASPxTextBox>
                                        <span id="MandatorySlOrderNo" style="display: none" class="validclass">
                                            <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                                    </div>

                                    <div class="col-md-2">
                                        <label>
                                            <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date" Width="120px" CssClass="inline">
                                            </dxe:ASPxLabel>
                                            <span style="color: red">*</span>
                                        </label>
                                        <dxe:ASPxDateEdit ID="dt_PLSales" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLSalesChallanDate" TabIndex="3" Width="100%">
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
                                            <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                            </dxe:ASPxLabel>
                                        </label>
                                        <dxe:ASPxCallbackPanel runat="server" ID="BranchCallBackPanel" ClientInstanceName="cBranchComponentPanel" OnCallback="ComponentBranch_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="4">
                                                    </asp:DropDownList>
                                                </dxe:PanelContent>
                                            </PanelCollection>
                                            <%--<ClientSideEvents EndCallback="componentEndCallBack" />--%>
                                        </dxe:ASPxCallbackPanel>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-2">
                                        <span style="margin: 3px 0; display: block">
                                            <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                            </dxe:ASPxLabel>
                                        </span>
                                        <dxe:ASPxTextBox ID="txt_Refference" runat="server" TabIndex="5" Width="100%" MaxLength="50">
                                        </dxe:ASPxTextBox>
                                    </div>

                                    <div class="col-md-3">
                                        <label>
                                            <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                            </dxe:ASPxLabel>
                                            <span style="color: red">*</span>
                                        </label>

                                        <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" TabIndex="6" UseSubmitBehavior="false" ClientInstanceName="ctxtCustName" Width="100%">

                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>

                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />

                                        </dxe:ASPxButtonEdit>

                                        <span id="MandatorysCustomer" style="display: none" class="validclass">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>


                                    </div>
                                    <div class="col-md-3">
                                        <label>
                                            <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person" CssClass="inline">
                                            </dxe:ASPxLabel>
                                        </label>
                                        <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" TabIndex="7" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">
                                            <ClientSideEvents EndCallback="cmbContactPersonEndCall" GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>

                                    </div>
                                    <div class="col-md-2">

                                        <asp:RadioButtonList ID="rdl_SaleInvoice" runat="server" RepeatDirection="Horizontal" TabIndex="8" onchange="return selectValue();" Width="120px">
                                            <%-- <asp:ListItem Text="Order" Value="MI" class="hide"></asp:ListItem>--%>
                                            <asp:ListItem Text="Invoice" Value="SI"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cSalesOrderComponentPanel" OnCallback="ComponentSalesOrder_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <dxe:ASPxGridLookup ID="lookup_order" SelectionMode="Multiple" runat="server" TabIndex="9" ClientInstanceName="gridSalesOrderLookup" OnDataBinding="lookup_order_DataBinding"
                                                        KeyFieldName="Order_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">

                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />


                                                            <dxe:GridViewDataColumn FieldName="Order_Number" Visible="true" VisibleIndex="1" Caption="Doc Number" Width="150" Settings-AutoFilterCondition="Contains" />
                                                            <dxe:GridViewDataColumn FieldName="Order_Date" Visible="true" VisibleIndex="2" Caption="Doc Date" Width="150" Settings-AutoFilterCondition="Contains" />
                                                            <dxe:GridViewDataColumn FieldName="name" Visible="true" VisibleIndex="3" Caption="Customer" Width="150" Settings-AutoFilterCondition="Contains" />
                                                            <dxe:GridViewDataColumn FieldName="Branch" Visible="true" VisibleIndex="4" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains" />
                                                            <dxe:GridViewDataColumn FieldName="Reference" Visible="true" VisibleIndex="5" Caption="Reference" Width="150" Settings-AutoFilterCondition="Contains" />

                                                        </Columns>
                                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                            <Templates>
                                                                <StatusBar>
                                                                    <table class="OptionsTable" style="float: right">
                                                                        <tr>
                                                                            <td>
                                                                                <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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

                                    <div class="col-md-2">
                                        <label>
                                            <dxe:ASPxLabel ID="lbl_SaleOrder_Date" runat="server" Text="Document Date" Width="120px">
                                            </dxe:ASPxLabel>
                                        </label>

                                        <asp:Label ID="lbl_MultipleDate" runat="server" Text="Multiple Select Sale Order Dates" Style="display: none"></asp:Label>

                                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <dxe:ASPxTextBox ID="dt_Quotation" runat="server" TabIndex="10" Width="100%" ClientEnabled="false" ClientInstanceName="cPLQADate">
                                                    </dxe:ASPxTextBox>

                                                    <dxe:ASPxDateEdit ID="dt_PLQuotation" runat="server" Enabled="false" Visible="false" EditFormat="Custom" ClientInstanceName="cPLOADate" TabIndex="13" Width="100%">
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
                                    <div class="clear"></div>
                                    <div class="col-md-2">
                                        <label>
                                            <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                            </dxe:ASPxLabel>
                                        </label>
                                        <%--<asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" TabIndex="11">
                                        </asp:DropDownList>--%>

                                        <dxe:ASPxButtonEdit ID="txtSalesManAgent" runat="server" ReadOnly="true" TabIndex="11" ClientInstanceName="ctxtSalesManAgent" Width="100%" UseSubmitBehavior="False">

                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>

                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){SalesManButnClick();}" KeyDown="SalesManbtnKeyDown" />

                                        </dxe:ASPxButtonEdit>

                                    </div>

                                    <div class="col-md-1 lblmTop8">
                                        <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Credit Days">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txtCreditDays" ClientInstanceName="ctxtCreditDays" runat="server" TabIndex="12" Width="100%">
                                            <MaskSettings Mask="<0..999999999>" AllowMouseWheel="false" />
                                            <ClientSideEvents TextChanged="CreditDays_TextChanged" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_DueDate" runat="server" Text="Due Date">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxDateEdit ID="dt_SaleInvoiceDue" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_SaleInvoiceDue" TabIndex="13" Width="100%">
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

                                        <dxe:ASPxTextBox ID="txt_OANumber" runat="server" TabIndex="21" Width="100%" MaxLength="50">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-3" style="display: none;">
                                        <label>
                                            <dxe:ASPxLabel ID="lbl_OADate" runat="server" Text="OA Date" Width="120px">
                                            </dxe:ASPxLabel>
                                        </label>

                                        <dxe:ASPxDateEdit ID="dt_OADate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLOADate" TabIndex="11" Width="100%">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div class="col-md-3" style="display: none;">
                                        <label>
                                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Expiry" CssClass="inline"
                                                Width="61px">
                                            </dxe:ASPxLabel>
                                        </label>
                                        <dxe:ASPxDateEdit ID="dt_PlOrderExpiry" runat="server" Style="display: none;" ClientInstanceName="cExpiryDate" EditFormat="Custom" EditFormatString="dd-MM-yyyy" TabIndex="4444" Width="100%">

                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" ErrorText="Expiry date can not be shorter than Pl/Quote date.">
                                                <RequiredField IsRequired="true" />
                                            </ValidationSettings>

                                            <ClientSideEvents DateChanged="function(s,e){SetDifference();}"
                                                Validation="function(s,e){e.isValid = (CheckDifference()>=0)}" />
                                        </dxe:ASPxDateEdit>

                                    </div>


                                    <div class="col-md-1">
                                        <label style="margin: 3px 0; display: block">Currency:  </label>
                                        <div>
                                            <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%"
                                                DataSourceID="SqlCurrency" DataValueField="Currency_ID" TabIndex="14"
                                                DataTextField="Currency_AlphaCode" onchange="ddl_Currency_Rate_Change()">
                                            </asp:DropDownList>

                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <label style="margin: 3px 0; display: block">Exch. Rate: </label>
                                        <div>
                                            <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate" TabIndex="15">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-2 hide">
                                        <label style="margin: 3px 0; display: block">E-Way Bill No.</label>
                                        <div>
                                            <dxe:ASPxTextBox ID="txt_EWayBillNO" runat="server" Width="100%" ClientInstanceName="ctxtEWayBillNO" TabIndex="16">
                                                <MaskSettings Mask="&lt;0..999999999999&gt;" AllowMouseWheel="false" />
                                            </dxe:ASPxTextBox>
                                            <span id="MandatoryEwayBillNo" style="display: none" class="validclass">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                        </div>
                                    </div>



                                    <div class="col-md-2">
                                        <span style="margin: 3px 0; display: block">
                                            <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                            </dxe:ASPxLabel>
                                        </span>
                                        <%--<asp:DropDownList ID="ddl_AmountAre" runat="server" TabIndex="12" Width="100%">
            </asp:DropDownList>--%>
                                        <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientInstanceName="cddl_AmountAre" TabIndex="17" Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                        </dxe:ASPxComboBox>

                                    </div>

                                    <div class="col-md-2" id="divposGst">
                                        <dxe:ASPxLabel ID="lbl_PosForGst" runat="server" Text="Place Of Supply [GST]">
                                        </dxe:ASPxLabel>
                                        <span style="color: red">*</span>
                                        <dxe:ASPxComboBox ID="ddl_PosGst" runat="server" ValueType="System.String" Width="100%" ClientInstanceName="cddl_PosGst" TabIndex="18">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulatePosGst(e)}" />
                                        </dxe:ASPxComboBox>
                                    </div>

                                    <div class="clear"></div>
                                    <div class="col-md-2 hide">
                                        <span style="margin: 3px 0; display: block">
                                            <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                            </dxe:ASPxLabel>
                                        </span>
                                        <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" OnCallback="ddl_VatGstCst_Callback" TabIndex="18" Width="100%">
                                            <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" />
                                        </dxe:ASPxComboBox>

                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="col-md-2">
                                        <label id="lblProject" runat="server">Project</label>
                                        <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataProjectChallan"
                                            KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                                            <Columns>
                                                <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
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
                                            <ClientSideEvents GotFocus="clookup_project_GotFocus" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />

                                            <%-- <ClearButton DisplayMode="Always">
                                            </ClearButton>--%>
                                        </dxe:ASPxGridLookup>
                                        <dx:LinqServerModeDataSource ID="EntityServerModeDataProjectChallan" runat="server" OnSelecting="EntityServerModeDataProjectChallan_Selecting"
                                            ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                    </div>
                                    <div class="col-md-4">
                                        <%-- <label id="Label1" runat="server">Hierarchy</label>--%>
                                        <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="col-md-12">

                                        <div style="display: none;">
                                            <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                                        </div>
                                        <div>
                                            <br />
                                        </div>


                                        <dxe:ASPxGridView runat="server" KeyFieldName="OrderID"
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
                                            Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsPager-Mode="ShowAllRecords">
                                            <SettingsPager Visible="false"></SettingsPager>
                                            <Columns>
                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2%" VisibleIndex="0" Caption=" ">
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
                                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Order_Num" ReadOnly="True" Width="0" VisibleIndex="2">
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="10%">
                                                    <PropertiesButtonEdit>
                                                        <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>


                                                <%--Batch Product Popup End--%>

                                                <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4" ReadOnly="True" Width="10%">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewCommandColumn VisibleIndex="5" Caption="Addl. Desc." Width="6%">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc" Image-Url="/assests/images/more.png" Image-ToolTip="Warehouse">
                                                            <Image ToolTip="Warehouse" Url="/assests/images/more.png">
                                                            </Image>
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>


                                                <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="6" Width="6%" PropertiesTextEdit-MaxLength="14">
                                                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                        <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityGotFocus" />
                                                        <ClientSideEvents />
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="7" ReadOnly="true" Width="6%">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <%--Caption="Warehouse"--%>
                                                <dxe:GridViewCommandColumn Width="6%" VisibleIndex="8" Caption="Stk Details">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>

                                                <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Price" VisibleIndex="9" Width="6%">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                        <ClientSideEvents LostFocus="SalePriceTextChange" GotFocus="SalesPriceGotFocus" />
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="10" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                        <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" />
                                                        <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountGotFocus" />
                                                        <Style HorizontalAlign="Right">
                                                            </Style>
                                                    </PropertiesTextEdit>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="11" Width="6%" ReadOnly="true">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="12" Width="6%" HeaderStyle-HorizontalAlign="Right">
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

                                                <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="13" Width="8%" ReadOnly="true">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <%--<ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="14" Width="10%" ReadOnly="false">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Left">

                                                        <Style HorizontalAlign="Left">
                                                            </Style>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4%" VisibleIndex="15" Caption=" ">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>
                                                <dxe:GridViewDataTextColumn FieldName="StockQuantity" Caption="Stock Qty" Width="0">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="StockUOM" Caption="Stock UOM" ReadOnly="true" Width="0">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Quotation No" CellStyle-CssClass="hide" FilterCellStyle-CssClass="hide" EditCellStyle-CssClass="hide" EditFormCaptionStyle-CssClass="hide" FooterCellStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" FieldName="Quotation_No" Width="0" HeaderStyle-CssClass="hide">
                                                    <PropertiesTextEdit NullTextStyle-CssClass="hide" ReadOnlyStyle-CssClass="hide" Style-CssClass="hide">

                                                        <NullTextStyle CssClass="hide"></NullTextStyle>

                                                        <ReadOnlyStyle CssClass="hide"></ReadOnlyStyle>

                                                        <Style CssClass="hide"></Style>

                                                    </PropertiesTextEdit>
                                                    <HeaderStyle CssClass="hide" />
                                                    <CellStyle CssClass="hide">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" VisibleIndex="20" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn VisibleIndex="19" FieldName="OrderDetails_Id" ReadOnly="true" Caption="Quotation_U" Width="0">
                                                </dxe:GridViewDataTextColumn>
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
                                    <div style="clear: both;"></div>
                                    <br />
                                    <div class="col-md-12">
                                        <asp:Label ID="ClientShowMsg" runat="server" Text="Already Delivered." CssClass="msgStyle" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                        <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & N&#818;ew" UseSubmitBehavior="false" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                        </dxe:ASPxButton>

                                        <dxe:ASPxButton ID="ASPxButton12" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" UseSubmitBehavior="false" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                            <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <asp:Button ID="Button1" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" UseSubmitBehavior="false" />

                                        <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" UseSubmitBehavior="false" meta:resourcekey="btnSaveRecordsResource1">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                        </dxe:ASPxButton>

                                        <a href="javascript:void(0);" id="btnAddNew" runat="server" class="btn btn-primary" style="display: none"><span><u>A</u>ttachment(s)</span></a>

                                        <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                        <asp:HiddenField ID="hfControlData" runat="server" />
                                        <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                        <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                                        <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                        <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="MI" />
                                    </div>
                                </div>
                            </dxe:ContentControl>
                        </ContentCollection>
                        <%--test generel--%>
                    </dxe:TabPage>
                    <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping" TabStyle-CssClass="bilingTab">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">

                                <ucBS:Sales_BillingShipping runat="server" ID="Sales_BillingShipping" />

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
                        <ClientSideEvents EndCallback="cgridProducts_EndCallBack " />
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <asp:Button ID="Button2" UseSubmitBehavior="false" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <%-- End--%>
        <%--Sudip--%>

        <div class="PopUpArea">

            <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
            <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
            <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
            <asp:HiddenField ID="hdfProductIDPC" runat="server" />
            <asp:HiddenField ID="hdfstockidPC" runat="server" />
            <asp:HiddenField ID="hdfopeningstockPC" runat="server" />
            <asp:HiddenField ID="hdbranchIDPC" runat="server" />
            <asp:HiddenField ID="hddnTextTaggedDoc" runat="server" />
            <asp:HiddenField ID="hddnBranchId" runat="server" />
            <asp:HiddenField ID="LastCompany" runat="server" />
            <asp:HiddenField ID="LastFinancialYear" runat="server" />
            <asp:HiddenField ID="hdnnCustomerOrPendingDelivery" runat="server" />
            <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />
            <asp:HiddenField ID="hddnCustomerDelivery" runat="server" />
            <asp:HiddenField ID="hddnCustomerDeliverySDOrOD" runat="server" />
            <asp:HiddenField ID="hddnWarehouseId" runat="server" />
            <asp:HiddenField ID="hddnBatchId" runat="server" />
            <asp:HiddenField ID="hddnWarehouseQty" runat="server" />
            <asp:HiddenField ID="hddnMatchQty" runat="server" />
            <asp:HiddenField ID="hddnConfigVariable_Val" runat="server" />
            <asp:HiddenField ID="hddnBillId" runat="server" />
            <asp:HiddenField ID="hddnPermissionString" runat="server" />
            <asp:HiddenField ID="hddnTypeString" runat="server" />
            <asp:HiddenField ID="hddnIsODSDFirstTime" runat="server" />

            <%--Debjyoti GST on 30-06-2017--%>
            <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
            <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
            <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
            <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
            <asp:HiddenField runat="server" ID="hddnSaveOrExitButton" />
            <asp:HiddenField runat="server" ID="IsDiscountPercentage" />

            <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
            <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
            <%--END--%>
            <%--ChargesTax--%>
            <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
                Width="900px" Height="300px" HeaderText="Tax & Charges" PopupHorizontalAlign="WindowCenter"
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
                                    <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" UseSubmitBehavior="false" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                        <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_tax_cancel" runat="server" UseSubmitBehavior="false" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
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
                                            Batch
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
                                            <%--<dxe:ASPxComboBox ID="CmbSerial" EnableIncrementalFiltering="True" ClientInstanceName="cCmbSerial"
                                                    TextField="SerialName" ValueField="SerialID" runat="server" Width="100%" OnCallback="CmbSerial_Callback">
                                                </dxe:ASPxComboBox>--%>
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
                                                                <dxe:ASPxButton ID="ASPxButton6" AutoPostBack="False" runat="server" Text="Close" Style="float: right" UseSubmitBehavior="false">
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
                                                <%--<ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />--%>
                                                  <ClientSideEvents TextChanged="function(s,e) { ChangePackingByQuantityinjs();}" />
                                            </dxe:ASPxTextBox>
                                            <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                    </div>

                                      <div class="col-md-3" id="div_AltQuantity">
                                        <div style="margin-bottom: 5px;">
                                            Alt. Qty
                                        </div>
                                        <div class="Left_Content" style="">
                                            <%--                  Rev Rajdip                         <dxe:ASPxTextBox ID="txtPacking"  runat="server" ClientSideEvents-GotFocus="QuantityGotFocus" ClientInstanceName="CtxtPacking"   HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">End Rev Rajdip--%>
                                            <dxe:ASPxTextBox ID="txtPacking" runat="server" ClientInstanceName="CtxtPacking" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                <%--ClientSideEvents-GotFocus="QuantityGotFocus"--%>
                                                <ValidationSettings Display="None"></ValidationSettings>
                                                <ClientSideEvents TextChanged="function(s,e) { ChangeQuantityByPacking1();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3" id="_div_Uom">
                                        <div style="margin-bottom: 5px;">
                                            Alt. UOM
                                        </div>
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
                                        <div class="Left_Content" style="padding-top: 14px;">
                                            <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="function(s, e) {if(!document.getElementById('myCheck').checked) SaveWarehouse();}" />
                                            </dxe:ASPxButton>
                                            <input id="btnSecondUOM" type="button" onclick="AlternateUOMDetails('SC')" value="2nd UOM" class="btn btn-success hide" />
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
                                        <dxe:GridViewDataTextColumn Caption="Batch Number" FieldName="BatchNo"
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
                                    <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" UseSubmitBehavior="false" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>

            <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                <ClientSideEvents ControlsInitialized="AllControlInitilize" />
            </dxe:ASPxGlobalEvents>


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
            <asp:HiddenField ID="hdnSalesManAgentId" runat="server" />
            <asp:HiddenField ID="hdnAddressDtl" runat="server" />
            <asp:HiddenField ID="hdnPageStatus" runat="server" />
            <asp:HiddenField ID="hdnSchemaLength" runat="server" />
            <asp:HiddenField runat="server" ID="IsUdfpresent" />
            <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
            <asp:HiddenField runat="server" ID="hdnEntityType" />
            <asp:HiddenField runat="server" ID="hdnQty" />
            <asp:HiddenField ID="hdnADDEditMode" runat="server" />
        </div>

        <!--Customer Modal For Issue Materials -->
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
                            <table border='1' width="100%">
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

        <%--SalesMan/Agent--%>
        <div class="modal fade" id="SalesManModel" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">SalesMan/Agent Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="SalesMankeydown(event)" id="txtSalesManSearch" autofocus width="100%" placeholder="Search By SalesMan/Agent Name" />

                        <div id="SalesManTable">
                            <table border='1' width="100%">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Name</th>
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
                            <table border='1' width="100%">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Product Code</th>
                                    <th>Product Description</th>
                                    <th>HSN/SAC</th>
                                    <%--<th>Brand</th>--%>
                                    <th>Class</th>
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


        <asp:SqlDataSource ID="SqlCurrency" runat="server"
            SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrencyBind" runat="server"></asp:SqlDataSource>
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
        <HeaderTemplate>
            <span>Select Product(s)</span>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By Product Name</strong></label>
                <dxe:ASPxGridLookup ID="productLookUp" runat="server" ClientInstanceName="cproductLookUp"
                    KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected"
                    ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                    <Columns>
                        <dxe:GridViewDataColumn FieldName="Products_Description" Caption="Name" Width="220">
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

    <%--InlineTax--%>

    <dxe:ASPxPopupControl ID="ASPxPopupControl3" runat="server" ClientInstanceName="caspxTaxpopUp"
        Width="850px" HeaderText="Tax & Charges" PopupHorizontalAlign="WindowCenter"
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
                                        <td>Add/Less</td>
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
                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
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
                                <asp:Button ID="Button4" runat="server" Text="Ok" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" UseSubmitBehavior="false" />
                                <asp:Button ID="Button5" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" UseSubmitBehavior="false" />
                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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


    <asp:HiddenField ID="hdnmodeId" runat="server" />

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>


    <dxe:ASPxPopupControl ID="SecondUOMpopup" runat="server" ClientInstanceName="cSecondUOM" ShowCloseButton="false"
        Width="850px" HeaderText="Second UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="clearfix boxStyle hide">
                    <div class="col-md-3">
                        <label>Length</label>
                        <dxe:ASPxTextBox runat="server" ID="txtLength" ClientInstanceName="ctxtLength">
                            <ClientSideEvents LostFocus="SizeLostFocus" />

                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />

                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Width</label>
                        <dxe:ASPxTextBox runat="server" ID="txtWidth" ClientInstanceName="ctxtWidth">
                            <ClientSideEvents LostFocus="SizeLostFocus" />
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />

                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Total</label>
                        <dxe:ASPxTextBox runat="server" ID="txtTotal" ClientEnabled="false" ClientInstanceName="ctxtTotal">
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3 padTop23 pdLeft0">
                        <label></label>
                        <button type="button" onclick="AddSecondUOMDetails();" class="btn btn-primary">Add</button>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="col-md-12">
                    <table id="dataTbl" class="display nowrap" style="width: 100%">
                        <thead>
                            <tr>
                                <th class="hide">GUID</th>
                                <th class="hide">WarehouseID</th>
                                <th class="hide">ProductId</th>
                                <th>Selection</th>
                                <th>SL</th>
                                <th>Branch</th>
                                <th>Warehouse</th>
                                <th>Size</th>
                                <th>Total</th>

                            </tr>
                        </thead>
                        <tbody id="tbodySecondUOM">
                        </tbody>
                    </table>
                </div>
                <div class="col-md-12 text-right pdTop15">
                    <button class="btn btn-success" type="button" onclick="SaveSecondUOMDetails();">OK</button>
                    <button class="btn btn-danger hide" type="button" onclick="return cSecondUOM.Hide();">Cancel</button>
                </div>
                <asp:HiddenField ID="hfDocId" runat="server" />
            </dxe:PopupControlContentControl>
        </ContentCollection>

    </dxe:ASPxPopupControl>
    <style>
        .boxStyle {
            padding: 5px;
            background: #f7f7f7;
            margin: 0 15px 8px 15px;
            border: 1px solid #ccc;
        }

        .link {
            cursor: pointer;
        }

        .pdLeft0 {
            padding-left: 0 !important;
        }

        #dataTbl_wrapper .dataTables_scrollHeadInner, #dataTbl_wrapper .dataTables_scrollHeadInner table {
            width: 100% !important;
        }

            #dataTbl_wrapper .dataTables_scrollHeadInner table > thead > tr > th {
                background: #337ab7;
                color: #fff;
                padding: 2px 15px;
            }

        #tbodySecondUOM > td {
            padding: 2px 25px;
        }

        #dataTbl_wrapper .dataTables_scrollHeadInner table > thead > tr > th:not(:last-child) {
            border-right: #333;
        }
    </style>

    <%--  OnCallback="callback_InlineRemarks_Callback"--%>
    <dxe:ASPxCallbackPanel runat="server" ID="callback_InlineRemarks" ClientInstanceName="ccallback_InlineRemarks">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <dxe:ASPxPopupControl ID="Popup_InlineRemarks" runat="server" ClientInstanceName="cPopup_InlineRemarks"
                    Width="900px" HeaderText="Remarks" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <%--<ClientSideEvents Closing="function(s, e) {
	                   closeRemarks(s, e);}" />--%>
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>


                        <dxe:PopupControlContentControl runat="server">
                            <div>
                                <asp:Label ID="lblInlineRemarks" runat="server" Text="Additional Remarks"></asp:Label>

                                <asp:TextBox ID="txtInlineRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" MaxLength="5000"></asp:TextBox>
                            </div>


                            <div class="clearfix">
                                <br />
                                <div style="align-content: center">
                                    <dxe:ASPxButton ID="btnSaveInlineRemarks" ClientInstanceName="cbtnSaveInlineRemarks" Width="50px" runat="server" AutoPostBack="false" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">

                                        <ClientSideEvents Click="FinalRemarks" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>



                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="callback_InlineRemarks_EndCall" />
    </dxe:ASPxCallbackPanel>


       <asp:HiddenField runat="server" ID="hdnpackingqty" />  
     <asp:HiddenField runat="server" ID="hdnuomFactor" /> 
    <asp:HiddenField runat="server" ID="hdnisOverideConvertion" /> 

</asp:Content>
