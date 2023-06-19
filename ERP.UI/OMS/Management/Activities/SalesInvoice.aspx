<%--==========================================================Revision History ============================================================================================   
   1.0   Priti      V2.0.36     10-02-2023     0025664:Transaction Category is not updated if the customer is B2C Type
   2.0   Sanchita   V2.0.38     10-04-2023     Tolerance feature required in Sales Order Module. Refer: 25223     
   3.0   Pallab     V2.0.38     27-04-2023     Add Sales Invoice module design modification. Refer: 25921
   4.0   Sanchita   V2.0.38     13-06-2023     Base Rate is not recalculated when the Multi UOM is Changed. Mantis : 26320, 26357, 26361   
   5.0   Pallab     V2.0.38     16-06-2023     "Multi UOM Details" popup parameter alignment issue fix . Mantis : 26331
========================================== End Revision History =======================================================================================================--%>

<%@ Page Title="Sales Invoice" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SalesInvoice.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesInvoice" %>

<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Sales_BillingShipping.ascx" TagPrefix="ucBS" TagName="Sales_BillingShipping" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/OtherTermsAndCondition.ascx" TagPrefix="ucOTC" TagName="OtherTermsAndCondition" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <script src="JS/SearchPopupDatatable.js"></script>
    <link href="CSS/PosSalesInvoice.css" rel="stylesheet" />
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <%-- <script src="JS/SearchPopup.js?v=2.0"></script>--%>
    <link href="CSS/SalesInvoice.css" rel="stylesheet" />
    <script src="JS/SalesInvoice.js?v=3.0"></script>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelNew.js?v=3.0" type="text/javascript"></script>
    <style>
        .wrapHolder#pageheaderContent {
            width: 840px;
        }

        #DivCost {
            min-width: 100px;
        }
    </style>
    <style>
        body {
            background-color: #f5f5f5;
        }

        .fileuploader {
            position: relative;
            width: 60%;
            margin: auto;
            height: 400px;
            border: 4px dashed #ddd;
            background: #f6f6f6;
            margin-top: 85px;
        }

            .fileuploader #upload-label {
                background: rgba(231, 97, 92, 0);
                color: #fff;
                position: absolute;
                height: 115px;
                top: 20%;
                left: 0;
                right: 0;
                margin-right: auto;
                margin-left: auto;
                min-width: 20%;
                text-align: center;
                cursor: pointer;
            }

            .fileuploader.active {
                background: #fff;
            }

                .fileuploader.active #upload-label {
                    background: #fff;
                    color: #e7615c;
                }

            .fileuploader #upload-label i:hover {
                color: #444;
                font-size: 9.4rem;
                -webkit-transition: width 2s;
            }

            .fileuploader #upload-label span.title {
                font-size: 1em;
                font-weight: bold;
                display: block;
            }

        span.tittle {
            position: relative;
            top: 222px;
            color: #bdbdbd;
        }

        .fileuploader #upload-label i {
            text-align: center;
            display: block;
            color: #e7615c;
            height: 115px;
            font-size: 9.5rem;
            position: absolute;
            top: -12px;
            left: 0;
            right: 0;
            margin-right: auto;
            margin-left: auto;
        }
        /** Preview of collections of uploaded documents **/
        .preview-container {
            position: relative;
            bottom: 0px;
            width: 35%;
            margin: auto;
            top: 25px;
            visibility: hidden;
        }

            .preview-container #previews {
                max-height: 400px;
                overflow: auto;
            }

                .preview-container #previews .zdrop-info {
                    width: 88%;
                    margin-right: 2%;
                }

                .preview-container #previews.collection {
                    margin: 0;
                    box-shadow: none;
                }

                    .preview-container #previews.collection .collection-item {
                        background-color: #e0e0e0;
                    }

                    .preview-container #previews.collection .actions a {
                        width: 1.5em;
                        height: 1.5em;
                        line-height: 1;
                    }

                        .preview-container #previews.collection .actions a i {
                            font-size: 1em;
                            line-height: 1.6;
                        }

                    .preview-container #previews.collection .dz-error-message {
                        font-size: 0.8em;
                        margin-top: -12px;
                        color: #F44336;
                    }



        /*media querie*/

        @media only screen and (max-width: 601px) {
            .fileuploader {
                width: 100%;
            }

            .preview-container {
                width: 100%;
            }
        }
    </style>
    <style>
        .cap {
            font-size: 34px;
            color: red;
        }

        .dropify-wrapper {
            border: 2px dashed #E5E5E5;
        }

        .ppTabl {
            margin: 0 auto;
        }

            .ppTabl > tbody > tr > td:first-child {
                text-align: right;
                padding-right: 15px;
            }

            .ppTabl > tbody > tr > td {
                padding: 4px 0;
                font-size: 15px;
                text-align: left;
            }

        .empht {
            font-size: 18px;
            color: #d68f0d;
            margin: 6px;
        }

        .poppins {
            font-family: 'Poppins', sans-serif;
        }

        .bcShad {
            position: fixed;
            width: 100%;
            background: rgba(0,0,0,0.75);
            height: 100%;
            left: 0;
            z-index: 120;
            top: 0;
            display: none;
        }

        .popupSuc {
            position: absolute;
            z-index: 123;
            background: #fff;
            padding: 3px;
            min-width: 650px;
            left: 50%;
            top: 50%;
            transform: translate(-50%, -50%);
            display: none;
        }

            .bcShad.in, .popupSuc.in {
                display: block;
            }

        .bInfoIt {
            text-align: center;
            border-top: 1px solid #ccc;
            border-bottom: 1px solid #ccc;
            padding: 12px;
        }

            .bInfoIt p {
                margin: 0;
            }

        .fontSmall > tbody > tr > td {
            font-size: 13px !important;
        }

        .cnIcon {
            display: flex;
            background: #4ec34e;
            border-radius: 50%;
            width: 80px;
            height: 80px;
            margin: 15px auto;
            justify-content: center;
            align-items: center;
            font-size: 32px;
            color: #fff;
        }

        #grid_DXMainTable > tbody > tr > td:nth-child(25) {
            display: none !important;
        }

        .mlableWh {
            padding-top: 22px;
            display: inline-block;
        }
            /*Mantis Issue 24428*/
            .mlableWh > input + span {
                white-space: nowrap;
            }

        .eqTble > tbody > tr > td {
            padding: 0 7px;
            vertical-align: top;
        }
        /*End of Mantis Issue 24428*/

    </style>

    <script>
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
        $(document).ready(function () {
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
        })

        function OnEndCallback(s, e) {
            if (($("#hddnMultiUOMSelection").val() == "0") && ($("#Keyval_internalId").val() == "Add")) {
                aarr = [];
                SelecttedProductAddUomConversion();
            }
            cbtn_SaveRecords.SetEnabled = true;
            var value = document.getElementById('hdnRefreshType').value;
            //Rev work start 24.06.2022 mantise no:0024987 
            var seting = document.getElementById('hdnCoordinate').value;
            //Rev work close 24.06.2022 mantise no:0024987 
            if ($('#HdUpdateMainGrid').val() == 'True') {
                $('#HdUpdateMainGrid').val('False');
                grid.PerformCallback('DateChangeDisplay');
            }
            if (grid.cpComponent) {
                if (grid.cpComponent == 'true') {
                    grid.cpComponent = null;
                    OnAddNewClick();
                }
            }
            LoadingPanel.Hide();
            if (grid.cpinsert == 'UDFMandatory') {
                OnAddNewClick();
                grid.cpinsert = null;
                jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
            }
            else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
                OnAddNewClick_AtSaveTime();
                grid.cpSaveSuccessOrFail = null;
                jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
                OnAddNewClick_AtSaveTime();
                grid.cpSaveSuccessOrFail = null;
                jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "OrderTaggingMandatory") {
                OnAddNewClick_AtSaveTime();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Order Tagging is set as Mandatory. Please enter values.');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "outrange") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can Not Add More Invoice Number as Invoice Scheme Exausted.<br />Update The Scheme and Try Again');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "AddLock") {
                LoadingPanel.Hide();
                jAlert('DATA is Freezed between ' + grid.cpAddLockStatus + ' for Add.');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "SalesmanMandatory") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                //Rev work start 24.06.2022 mantise no:0024987   
                //jAlert("Salesman is set as Mandatory. Please select a salesman.");               
                if (seting == "NO") {
                    jAlert("Salesman is set as Mandatory. Please select a salesman.");
                }
                else {
                    jAlert("Coordinator is set as Mandatory. Please select a Coordinator.");
                }
                //Rev work close 24.06.2022 mantise no:0024987   
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "BillingShippingNotLoaded") {
                grid.AddNewRow();
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Billing Shipping is not yet loaded.Please wait.');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "DueDateLess") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Due date must be greater than Today');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "TCSMandatory") {
                grid.cpSaveSuccessOrFail = null;
                // Mantis Issue 24789
                //ShowTCS();
                // End of Mantis Issue 24789
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "ZeroTaxSalesInvoice") {
                //OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jConfirm('Do you want to proceed with zero tax calculation?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        $("#hdnShowAlertZeroTaxSI").val("1");
                        if (value == "E") {
                            SaveExit_ButtonClick();
                        }
                        else if (value == "N") {
                            Save_ButtonClick();
                        }
                    }
                    else {

                    }

                });

                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can Not Save as Duplicate Invoice Numbe No. Found');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "checkAcurateTaxAmount") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Check GST Calculated for Item ' + grid.cpProductName + ' at line ' + grid.cpSerialNo);
                grid.cpSaveSuccessOrFail = '';
                grid.cpSerialNo = '';
                grid.cpProductName = '';
            }
            else if (grid.cpSaveSuccessOrFail == "MultiTax") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('All the selected documents must have same type of taxes. Documents with different tax and type cannot be selected. Please re-check.');

                cQuotationComponentPanel.PerformCallback('RebindGridQuote');
                grid.cpSaveSuccessOrFail = '';
            }

            else if (grid.cpSaveSuccessOrFail == "TCSLock") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('TCS already posted after selected date for this customer,Can not proceed. Please re-check.');


            }
            else if (grid.cpSaveSuccessOrFail == "quantityTagged") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Proforma is tagged in Sale Order. So, Quantity of selected products cannot be less than Ordered Quantity.');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please try after sometime.');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please select project.');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "nullAmount") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('total amount cant not be zero(0).');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "NetAmountExceed") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Net Amount of selected Product from tagged document.<br />Cannot enter Net Amount more than Sales Order Net Amount .');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                // Rev Sanchita
                //OnAddNewClick();
                grid.batchEditApi.StartEdit(0, 2);
                // End of Rev Sanchita
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please fill Quantity');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "nullCredit") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Credit Days must be greater than Zero(0)');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can not Duplicate Product in the Invoice List.');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "minSalePriceMust") {
                OnAddNewClick();
                jAlert('Sale Price Should be equal or higher than Min Sale Price');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "MRPLess") {
                OnAddNewClick();
                jAlert('Sale Price Should be equal or less than MRP');
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;

                var SrlNo = grid.cpProductSrlIDCheck;
                var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
                jAlert(msg);
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                var SrlNo = grid.cpcheckMultiUOMData;
                var msg = "Please add Alt. Qty for SL No. " + SrlNo;
                grid.cpcheckMultiUOMData = null;
                jAlert(msg);
                grid.cpSaveSuccessOrFail = '';
            }
            // Rev 4.0
            else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData_QtyMismatch") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                var SrlNo = grid.cpcheckMultiUOMData;
                var msg = "Please check Multi UOM details for SL No. not matching with outer grid " + SrlNo;
                grid.cpcheckMultiUOMData = null;
                jAlert(msg);
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData_NotFound") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                var SrlNo = grid.cpcheckMultiUOMData;
                var msg = "Multi UOM details not given for SL No. " + SrlNo;
                grid.cpcheckMultiUOMData = null;
                jAlert(msg);
                grid.cpSaveSuccessOrFail = '';
            }
            // End of Rev 4.0
            else {
                var Quote_Number = grid.cpQuotationNo;
                var Quote_ID = grid.cpQuotationID;
                $("#hdnRDECId").val(Quote_ID);
                var Quote_Msg = "Sales Invoice No. '" + Quote_Number + "' saved.";
                var EInvQuote_Msg = "Sales Invoice No. '" + Quote_Number + "' generated.";

                var IsEinvoice1 = grid.cpisEinvoice;
                if (IsEinvoice1 == 'true') {
                    $.ajax({
                        type: "POST",
                        url: "SalesInvoiceList.aspx/GetEditablePermissionFromEInvoice",
                        data: "{'SalesInvoiceID':'" + $("#hdnRDECId").val() + "','Action':'ExemptedChecked'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var status = msg.d;
                            if (status == "Yes") {

                            }
                            else {
                                grid.cpisEinvoice = null;
                                Quote_Msg = Quote_Msg + "<br>" + "This Invoice contains an Exempted Item.No Need to generate IRN."

                            }
                        }
                    });
                }
                var IsEinvoice = grid.cpisEinvoice;
               
                grid.cpisEinvoice = null;
                if (IsEinvoice == 'true') {
                    jAlert(EInvQuote_Msg, 'Alert Dialog: [Sales Invoice]', function (r) {
                        if (r == true) {

                            $("#lblInvNUmber").text(Quote_Number);
                            $("#lblInvDate").text(tstartdate.GetText());
                            $("#lblCust").text(ctxtCustName.GetText());
                            $("#lblAmount").text(grid.cpToalAmountDEt);
                            LoadingPanel.Hide();
                            //cUploadConfirmation.Show();
                            $("#exampleModalSI").modal("show");
                           
                        }

                    });
                }
                else {
                    grid.cpQuotationNo = null;
                    grid.cpQuotationID = null;
                    document.getElementById('hdnRefreshType').value = "";

                    var IRNgenerated = grid.cpSucessIRN;
                    grid.cpSucessIRN = null;

                    if (IRNgenerated == "No") {
                        jAlert('Error while generation IRN', 'Alert', function () {
                            window.location.assign("SalesInvoiceList.aspx");
                        });
                    }
                    else {
                        if (IRNgenerated == "Yes") {
                            $("#IrnNumber").text(grid.cpSucessIRNNumber);
                            $("#IrnlblInvNUmber").text(Quote_Number);
                            $("#IrnlblInvDate").text(tstartdate.GetText());
                            $("#IrnlblCust").text(ctxtCustName.GetText());
                            $("#IrnlblAmount").text(grid.cpToalAmountDEt);
                            $(".bcShad, .popupSuc").addClass("in")
                        }
                        else {
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
                                    if (Quote_Number != "") {
                                        //Add Section For Auto Print checking for Einvoice Tanmoy
                                        if ($("#hdnIsBranchEInvoice").val() == "True") {
                                            if (($("#drdTransCategory").val() == "B2B") || ($("#drdTransCategory").val() == "SEZWP") || ($("#drdTransCategory").val() == "SEZWOP") || ($("#drdTransCategory").val() == "EXPWP") || ($("#drdTransCategory").val() == "EXPWOP") || ($("#drdTransCategory").val() == "DEXP")) {

                                            }
                                            else {
                                                if ($("#hdnAutoPrint").val() == "1") {
                                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=1', '_blank')
                                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=2', '_blank')
                                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=4', '_blank')
                                                }
                                            }
                                        }
                                        else {
                                            if ($("#hdnAutoPrint").val() == "1") {
                                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=1', '_blank')
                                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=2', '_blank')
                                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=4', '_blank')
                                            }
                                        }
                                        //Add Section For Auto Print checking for Einvoice Tanmoy
                                        jAlert(Quote_Msg, 'Alert Dialog: [SalesInvoice]', function (r) {
                                            if (r == true) {
                                                var CashBank = (cddlCashBank.GetValue() != null) ? cddlCashBank.GetValue() : "";

                                                if (CashBank != "") {
                                                    var URL = "CustomerReceiptPayment.aspx?key=ADD&IsTagged=Y&PageStatus=E&ComponentType=I&ComponentID=" + Quote_ID; // ComponentType => I : Invoice ; B : POS Bucket
                                                    capcReciptPopup.SetContentUrl(URL);
                                                    capcReciptPopup.Show();
                                                }
                                                else {
                                                    window.location.assign("SalesInvoiceList.aspx");
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        window.location.assign("SalesInvoiceList.aspx");
                                    }
                                }

                            }
                            else if (value == "N") {
                                if (grid.cpApproverStatus == "approve") {
                                    window.parent.popup.Hide();
                                    window.parent.cgridPendingApproval.PerformCallback();
                                }
                                else {
                                    if (Quote_Number != "") {
                                        //Add Section For Auto Print checking for Einvoice Tanmoy
                                        if ($("#hdnIsBranchEInvoice").val() == "True") {
                                            if (($("#drdTransCategory").val() == "B2B") || ($("#drdTransCategory").val() == "SEZWP") || ($("#drdTransCategory").val() == "SEZWOP") || ($("#drdTransCategory").val() == "EXPWP") || ($("#drdTransCategory").val() == "EXPWOP") || ($("#drdTransCategory").val() == "DEXP")) {

                                            }
                                            else {
                                                if ($("#hdnAutoPrint").val() == "1") {
                                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=1', '_blank')
                                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=2', '_blank')
                                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=4', '_blank')
                                                }
                                            }
                                        }
                                        else {
                                            if ($("#hdnAutoPrint").val() == "1") {
                                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=1', '_blank')
                                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=2', '_blank')
                                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesInvoice_PK&modulename=Invoice&id=" + Quote_ID + '&PrintOption=4', '_blank')
                                            }
                                        }
                                        //Add Section For Auto Print checking for Einvoice Tanmoy

                                        jAlert(Quote_Msg, 'Alert Dialog: [SalesInvoice]', function (r) {
                                            if (r == true) {
                                                var CashBank = (cddlCashBank.GetValue() != null) ? cddlCashBank.GetValue() : "";

                                                if (CashBank != "") {
                                                    var URL = "CustomerReceiptPayment.aspx?key=ADD&IsTagged=Y&PageStatus=E&ComponentType=I&ComponentID=" + Quote_ID; // ComponentType => I : Invoice ; B : POS Bucket
                                                    capcReciptPopup.SetContentUrl(URL);
                                                    capcReciptPopup.Show();
                                                }
                                                else {
                                                    window.location.assign("SalesInvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        window.location.assign("SalesInvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
                                    }
                                }
                            }
                            else {
                                var pageStatus = document.getElementById('hdnPageStatus').value;
                                if (pageStatus == "first") {
                                    OnAddNewClick();
                                    grid.batchEditApi.EndEdit();
                                    $('#hdnPageStatus').val('');
                                    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                                    var basedCurrency = LocalCurrency.split("~");
                                    if ($("#ddl_Currency").val() == basedCurrency[0]) {
                                        ctxt_Rate.SetEnabled(false);
                                    }
                                }
                                else if (pageStatus == "update") {
                                    OnAddNewClick();
                                    $('#hdnPageStatus').val('');

                                    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                                    var basedCurrency = LocalCurrency.split("~");
                                    if ($("#ddl_Currency").val() == basedCurrency[0]) {
                                        ctxt_Rate.SetEnabled(false);
                                    }
                                }
                                else if (pageStatus == "Quoteupdate") {
                                    grid.StartEditRow(0);
                                    $('#hdnPageStatus').val('');
                                }
                                else if (pageStatus == "delete") {
                                    grid.StartEditRow(0);
                                    $('#hdnPageStatus').val('');
                                }
                        }
                }
            }
        }
    }

    if (gridquotationLookup.GetValue() != null) {
        grid.GetEditor('ComponentNumber').SetEnabled(false);
        grid.GetEditor('ProductName').SetEnabled(false);
        grid.GetEditor('Description').SetEnabled(false);
        ctxtCustName.SetEnabled(false);
        $("#ddl_numberingScheme").prop("disabled", true);
    }
            //Rev Rajdip For Running Total
    if (grid.cpRunningTotal != null && grid.cpRunningTotal != "" && grid.cpRunningTotal != "undefined") {
        var strRunnging = grid.cpRunningTotal;
        var TotalQty = strRunnging.split("~")[0].toString();
        var Amount = strRunnging.split("~")[1].toString();
        var TaxAmount = strRunnging.split("~")[2].toString();
        var AmountWithTaxValue = strRunnging.split("~")[3].toString();
        var TotalAmt = strRunnging.split("~")[4].toString();
        cbnrLblTotalQty.SetText(TotalQty);
        cbnrLblTaxableAmtval.SetText(Amount);
        cbnrLblTaxAmtval.SetText(TaxAmount);
        cbnrlblAmountWithTaxValue.SetText(AmountWithTaxValue);
        cbnrLblInvValue.SetText(TotalAmt);
    }
            //End Rev Rajdip For Running Total
    if (grid.cpDetails != null) {
        var details = grid.cpDetails;
        grid.cpDetails = null;
        var SpliteDetails = details.split("~");
        var Reference = SpliteDetails[0];
        var Currency_Id = SpliteDetails[1];
        var SalesmanId = SpliteDetails[2];
        var ExpiryDate = SpliteDetails[3];
        var CurrencyRate = SpliteDetails[4];
        var Type = SpliteDetails[5];
        var CreditDays = SpliteDetails[6];
        var DueDate = SpliteDetails[7];
        var SalesmanName = SpliteDetails[8];

        var TaxOption = SpliteDetails[9];
        if (Type == "SO") {
            if (DueDate != "" && CreditDays != "") {
                ctxtCreditDays.SetValue(CreditDays);
                var Due_Date = new Date(DueDate);

                var CreditDays = ctxtCreditDays.GetValue();
                var today = new Date();
                today = tstartdate.GetDate();
                today.setDate(today.getDate() + Math.round(CreditDays));
                cdt_SaleInvoiceDue.SetDate(today);
                //cdt_SaleInvoiceDue.SetDate(Due_Date);


            }
        }
        ctxt_Refference.SetValue(Reference);
        ctxt_Rate.SetValue(CurrencyRate);
        document.getElementById('ddl_Currency').value = Currency_Id;
        $("#hdnSalesManAgentId").val(SalesmanId);
        ctxtSalesManAgent.SetValue(SalesmanName);
        cddl_AmountAre.SetValue(TaxOption);

        if (ExpiryDate != "") {
            if (Type == "SO") {
                var myDate = new Date(ExpiryDate);
                var invoiceDate = new Date();
                var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));
                ctxtCreditDays.SetValue(datediff);
                cdt_SaleInvoiceDue.SetDate(myDate);
            }
        }
    }
    cProductsPopup.Hide();
}
function PerformCallToGridBind() {
    var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

    var CreditDays = 1;
    var newdate = new Date();
    var today = new Date();

    today = tstartdate.GetDate();
    today.setDate(today.getDate() + Math.round(CreditDays));

    cdt_SaleInvoiceDue.SetDate(today);


    grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
    if (quote_Id.length == 0 || quote_Id.length < 0) {
        grid.AddNewRow();
        grid.GetEditor('SrlNo').SetValue('1');
    }
    //cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
    $('#hdnPageStatus').val('Quoteupdate');
    cProductsPopup.Hide();

    // Mantis Issue 24861
    if (cddl_AmountAre.GetValue() == 1) {
        cddlVatGstCst.PerformCallback('2');
    }
    // End of Mantis Issue 24861

    AllowAddressShipToPartyState = false;
    //#### added by Samrat Roy for Transporter Control #############

    // var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
    if (quote_Id.length > 0) {
        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
            callTransporterControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
        }
    }

    if (quote_Id.length > 0) {
        //Chinmoy added Below line
        GetDocumentAddress(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
        //BSDocTagging(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
    }
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        if (quote_Id.length > 0) {
            BindOrderProjectdata(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
        }
    }
    //#### end : Samrat Roy for Transporter Control :end #############

    //#### added by Sayan Dutta for TC Control #############
    if ($("#btn_TermsCondition").is(":visible")) {
        if (quote_Id.length > 0) {
            callTCControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
        }
    }
    if ($("#btn_OtherTermsCondition").is(":visible")) {
        if (quote_Id.length > 0) {
            callOTCControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
        }
    }



    //Chinmoy added below line
    //cddl_PosGst.SetEnabled(false);
    //Rev Rajdip
    var hdnPlaceShiptoParty = $("#hdnPlaceShiptoParty").val();
    //cddl_PosGst.SetEnabled(false);
    if (hdnPlaceShiptoParty == "1")
    { cddl_PosGst.SetEnabled(true); }
    else
    { cddl_PosGst.SetEnabled(false); }
    //End Rev Rajdip
    //#### End : added by Sayan Dutta for TC Control : End #############





    return false;
}


//Rev Bapi
$(document).ready(function () {

    $("#UOMQuantity").on('blur', function () {
        var currentObj = $(this);
        var currentVal = currentObj.val();
        if (!isNaN(currentVal)) {
            var updatedVal = parseFloat(currentVal).toFixed(4);
            currentObj.val(updatedVal);
        }
        else {
            currentObj.val("");
        }
    })


})
//End Rev Bapi
    </script>

    <%--Rev 3.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    

        <style>
            #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dt_OADate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

            #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dt_OADate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dt_OADate_B-1 #dt_OADate_B-1Img
        {
            display: none;
        }

        .calendar-icon
        {
                right: 18px !important;
        }

        select#ddlInventory
        {
            -webkit-appearance: auto;
                background: #42b39e !important;
        }

        .simple-select::after
        {
            top: 26px !important;
            right: 13px !important;
        }

        .col-sm-3 , .col-md-3 , .col-md-2{
            margin-bottom: 5px;
        }

        #rdl_Salesquotation
        {
            margin-top: 10px;
        }
        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        /*#CustomerTableTbl.dynamicPopupTbl>tbody>tr>td
        {
            width: 33.33%;
        }*/

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 0 !important;
        }

            @media only screen and (max-width: 1380px) and (min-width: 1300px)
            {

                .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12 {
                    padding-right: 10px;
                    padding-left: 10px;
                }

                .simple-select::after
                {
                    right: 8px !important;
                }
                .calendar-icon {
                    right: 13px !important;
                }

                input[type="radio"], input[type="checkbox"] {
                    margin-right: 0px;
                }
            }
            /*Rev 5.0: for parameter alignment issue fix*/
            .mlableWh
            {
                width: 120px !important;
                padding-top: 25px !important;
            }

            .mlableWh .dxeBase_PlasticBlue , .mlableWh label
            {
                line-height: 13px !important;
            }
            /*Rev end 5.0*/
        </style>
    <%--Rev end 3.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 3.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix">
        <h3 class="pull-left">
            <asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>
            <%--<label>Add Proforma Invoice/ Quotation</label>--%>
        </h3>
        <div id="pageheaderContent" class="scrollHorizontal pull-right reverse wrapHolder content horizontal-images" style="display: none;">
            <div class="Top clearfix">
                <ul>

                    <li>
                        <div class="lblHolder" id="divPacking" style="display: none;">
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
                        <div class="lblHolder" id="DivSell" style="display: none;">
                            <table>
                                <tr>
                                    <td>Sell @</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSell" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="DivMRP" style="display: none;">
                            <table>
                                <tr>
                                    <td>MRP @</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMRP" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="DivPurchase" style="display: none;">
                            <table>
                                <tr>
                                    <td>Purchase @</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPurchase" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="DivCost" style="display: none;">
                            <table>
                                <tr>
                                    <td>Cost @</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCost" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="divDues" style="display: none;">
                            <table>
                                <tr>
                                    <td>Receivable(Dues)</td>
                                </tr>
                                <tr>
                                    <td class="lower">
                                        <asp:Label ID="lblTotalDues" runat="server"></asp:Label>
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
                                        <asp:Label ID="lblAvailableStk" runat="server" Text="0.0"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="DivStockPosition" style="display: none;">
                            <table>
                                <tr>
                                    <td>Stock Position</td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#" onclick="onViewStockPosition()"><i class="fa fa-eye"></i></a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <%--<li>
                        <div class="lblStockPosition" id="" style="display: none;">                            
                            <input type="button" value="" class="btn btn-primary btn-radius"  />
                        </div>
                    </li>--%>
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
        <div id="divcross" runat="server" class="crossBtn"><a href="SalesInvoiceList.aspx"><i class="fa fa-times"></i></a></div>

    </div>
        <%--Rev 4.0 [id="divPageGeneral"] --%>
        <div class="form_main" id="divPageGeneral">
        <asp:Panel ID="pnl_quotation" runat="server">
            <div class="">
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="">
                                        <div style=" padding: 8px 0; margin-bottom: 0px; border-radius: 4px; " class="clearfix col-md-12">
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_Inventory" runat="server" Text="Type">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddlInventory" CssClass="backSelect" runat="server" Width="100%" TabIndex="1">
                                                    <asp:ListItem Text="Inventory Item" Value="Y" />
                                                    <asp:ListItem Text="Non-Inventory Item" Value="N" />
                                                    <asp:ListItem Text="Capital Goods" Value="C" />
                                                    <asp:ListItem Text="Service Item" Value="S" />
                                                    <asp:ListItem Text="All Item" Value="B" />
                                                </asp:DropDownList>
                                            </div>
                                            <%--Rev 3.0: "simple-select" class add --%>
                                            <div class="col-md-2 lblmTop8 simple-select" id="divScheme" runat="server">
                                                <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" TabIndex="2">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_SaleInvoiceNo" runat="server" Text="Document No.">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txt_PLQuoteNo" runat="server" ClientInstanceName="ctxt_PLQuoteNo" TabIndex="2" Width="100%">
                                                    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                                </dxe:ASPxTextBox>
                                                <span id="MandatorysQuoteno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                </span>
                                                <span id="duplicateQuoteno" style="display: none" class="validclass"><img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Duplicate number">
                                                </span>
                                                
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_SaleInvoiceDt" runat="server" Text="Posting Date">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate" TabIndex="3" Width="100%" UseMaskBehavior="True">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <ClientSideEvents DateChanged="function(s, e) {DateCheck();}" GotFocus="function(s,e){tstartdate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}" />
                                                </dxe:ASPxDateEdit>
                                                <%--Rev 3.0--%>
                                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                <%--Rev end 3.0--%>
                                            </div>
                                            <%--Rev 3.0: "simple-select" class add --%>
                                            <div class="col-md-2 lblmTop8 simple-select">
                                                <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="4" onchange="ddlBranch_ChangeIndex()" Enabled="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2  lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                                </dxe:ASPxLabel>
                                                <i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i>
                                                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>
                                                <span id="MandatorysCustomer" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-2  " id="DivSegment1" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="Segment1">
                                                </dxe:ASPxLabel>

                                                <dxe:ASPxButtonEdit ID="txtSegment1" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment1" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){Segment1ButnClick();}" KeyDown="function(s,e){Segment1_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>

                                            </div>
                                            <div class="col-md-2  " id="DivSegment2" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel15" runat="server" Text="Segment2">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxButtonEdit ID="txtSegment2" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment2" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){Segment2ButnClick();}" KeyDown="function(s,e){Segment2_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>
                                            </div>
                                            <div class="col-md-2  " id="DivSegment3" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel16" runat="server" Text="Segment3">
                                                </dxe:ASPxLabel>

                                                <dxe:ASPxButtonEdit ID="txtSegment3" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment3" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){Segment3ButnClick();}" KeyDown="function(s,e){Segment3_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>

                                            </div>
                                            <div class="col-md-2  " id="DivSegment4" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="Segment4">
                                                </dxe:ASPxLabel>

                                                <dxe:ASPxButtonEdit ID="txtSegment4" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment4" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){Segment4ButnClick();}" KeyDown="function(s,e){Segment4_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>

                                            </div>
                                            <div class="col-md-2  " id="DivSegment5" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Segment5">
                                                </dxe:ASPxLabel>

                                                <dxe:ASPxButtonEdit ID="txtSegment5" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment5" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){Segment5ButnClick();}" KeyDown="function(s,e){Segment5_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>

                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" ClientSideEvents-EndCallback="cmbContactPersonEndCall" TabIndex="6" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">
                                                    <ClientSideEvents EndCallback="cmbContactPersonEndCall" GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-4 lblmTop8">
                                                <asp:RadioButtonList ID="rdl_SaleInvoice" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();">
                                                    <asp:ListItem Text="PI/Quotation" Value="QO"></asp:ListItem>
                                                    <asp:ListItem Text="Order" Value="SO"></asp:ListItem>
                                                    <%--<asp:ListItem Text="Challan" Value="SC"></asp:ListItem>--%>
                                                </asp:RadioButtonList>
                                                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <asp:HiddenField runat="server" ID="OldSelectedKeyvalue" />
                                                            <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridquotationLookup"
                                                                OnDataBinding="lookup_quotation_DataBinding"
                                                                KeyFieldName="ComponentID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="ComponentNumber" Visible="true" VisibleIndex="1" Caption="Number" Width="180" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Date" Width="100" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Customer Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>

                                                                    <dxe:GridViewDataColumn FieldName="RevNo" Visible="true" VisibleIndex="4" Caption="Revision No." Width="100" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="RevDate" Visible="true" VisibleIndex="5" Caption="Revision Date" Width="80" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="ReferenceName" Visible="true" VisibleIndex="6" Caption="Reference" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="BranchName" Visible="true" VisibleIndex="7" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="Order_OANumber" Visible="true" VisibleIndex="8" Caption="Party Order No." Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="Order_OADate" Visible="true" VisibleIndex="9" Caption="Party Order Date" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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
                                                                <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridquotationLookup.ShowDropDown();  }" DropDown="LoadOldSelectedKeyvalue" />
                                                            </dxe:ASPxGridLookup>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="componentEndCallBack" BeginCallback="BeginComponentCallback" />
                                                </dxe:ASPxCallbackPanel>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_InvoiceNO" ClientInstanceName="clbl_InvoiceNO" runat="server" Text="Posting Date">
                                                </dxe:ASPxLabel>
                                                <div style="width: 100%; border: 1px solid #e6e6e6;">
                                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                                        <PanelCollection>
                                                            <dxe:PanelContent runat="server">
                                                                <dxe:ASPxTextBox ID="txt_InvoiceDate" ClientInstanceName="ctxt_InvoiceDate" runat="server" Width="100%" ClientEnabled="false">
                                                                </dxe:ASPxTextBox>
                                                            </dxe:PanelContent>
                                                        </PanelCollection>
                                                    </dxe:ASPxCallbackPanel>
                                                    <%--Rev 3.0--%>
                                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                    <%--Rev end 3.0--%>
                                                </div>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txt_Refference" ClientInstanceName="ctxt_Refference" runat="server" TabIndex="7" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                                </dxe:ASPxLabel>
                                                <a href="javascript:void(0);" style="left: -12px; top: 20px;"><%--onclick="AddcustomerClick()"--%>

                                                    <i id="I1" runat="server" class="fa fa-trash" aria-hidden="true" onclick="Deletesalesman()"></i>


                                                </a>
                                                <%--<asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" TabIndex="8">
                                                </asp:DropDownList>--%>
                                                <dxe:ASPxButtonEdit ID="txtSalesManAgent" runat="server" ReadOnly="true" ClientInstanceName="ctxtSalesManAgent" TabIndex="8" Width="100%">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){SalesManButnClick();}" KeyDown="SalesManbtnKeyDown" />
                                                </dxe:ASPxButtonEdit>
                                            </div>
                                            <div style="clear: both"></div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Cash/Bank">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientIDMode="Static" ClientInstanceName="cddlCashBank" TabIndex="11" Width="100%" OnCallback="ddlCashBank_Callback" ClientEnabled="false">
                                                    <ClientSideEvents GotFocus="function(s,e){cddlCashBank.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                                <%--TextChanged="ShowReceiptPayment" --%>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Credit Days">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txtCreditDays" ClientInstanceName="ctxtCreditDays" runat="server" TabIndex="12" Width="100%">
                                                    <MaskSettings Mask="<0..999999999>" AllowMouseWheel="false" />
                                                    <ClientSideEvents TextChanged="CreditDays_TextChanged" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_DueDate" runat="server" Text="Due Date">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxDateEdit ID="dt_SaleInvoiceDue" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_SaleInvoiceDue" TabIndex="12" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <ClientSideEvents GotFocus="function(s,e){cdt_SaleInvoiceDue.ShowDropDown();}" />
                                                </dxe:ASPxDateEdit>
                                                <%--Rev 3.0--%>
                                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                <%--Rev end 3.0--%>
                                            </div>
                                            <%--Rev 3.0: "simple-select" class add --%>
                                            <div class="col-md-2 lblmTop8 simple-select">
                                                <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" TabIndex="13">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server" TabIndex="14" Width="100%" Height="28px">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="ReBindGrid_Currency" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" TabIndex="15" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" LostFocus="function(s, e) { SetFocusonDemand(e)}"
                                                        GotFocus="function(s,e){cddl_AmountAre.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-3 hide">
                                                <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select VAT/GST/CST">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" OnCallback="ddl_VatGstCst_Callback" TabIndex="16" Width="100%">
                                                    <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" GotFocus="function(s,e){cddlVatGstCst.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                                <span id="Mandatorytaxcode" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                            <div style="clear: both;"></div>
                                            <div class="col-md-4">
                                                <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="Remarks">
                                                </dxe:ASPxLabel>
                                                <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2" id="divposGst">
                                                <dxe:ASPxLabel ID="lbl_PosForGst" runat="server" Text="Place Of Supply [GST]">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                                <dxe:ASPxComboBox ID="ddl_PosGst" runat="server" ValueType="System.String" Width="100%" ClientInstanceName="cddl_PosGst" TabIndex="18">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulatePosGst(e)}" />
                                                </dxe:ASPxComboBox>
                                            </div>

                                            <div class="col-md-2">
                                                <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Transaction Category">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="drdTransCategory" runat="server" Width="100%" Enabled="false">
                                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="B2B" Value="B2B" />
                                                    <asp:ListItem Text="B2C" Value="B2C" /><%--Rev 1.0--%>
                                                    <asp:ListItem Text="SEZWP" Value="SEZWP" />
                                                    <asp:ListItem Text="SEZWOP" Value="SEZWOP" />
                                                    <asp:ListItem Text="EXPWP" Value="EXPWP" />
                                                    <asp:ListItem Text="EXPWOP" Value="EXPWOP" />
                                                    <asp:ListItem Text="DEXP" Value="DEXP" />
                                                </asp:DropDownList>
                                            </div>

                                            <div id="divMail" class="col-md-2" style="padding-top: 19px; display: none">
                                                <label class="checkbox-inline">
                                                    <asp:CheckBox ID="chkSendMail" runat="server"></asp:CheckBox>
                                                    <span style="margin: 0px 0; display: block">
                                                        <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Send Email">
                                                        </dxe:ASPxLabel>
                                                    </span>
                                                </label>
                                            </div>
                                            <div class="col-md-2">
                                                <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                                                </dxe:ASPxLabel>
                                                <%-- <label id="lblProject" runat="server">Project</label>--%>
                                                <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSalesInvoice"
                                                    KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                                            <Settings AutoFilterCondition="Contains" />
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
                                                    <%--     <ClearButton DisplayMode="Always">
                                                    </ClearButton>--%>
                                                </dxe:ASPxGridLookup>
                                                <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesInvoice" runat="server" OnSelecting="EntityServerModeDataSalesInvoice_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                            </div>
                                            <div class="col-md-4">
                                                <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div style="clear: both;"></div>
                                            <div class="col-md-2">
                                                <%-- <dxe:ASPxLabel ID="lblReverseCharge" runat="server" Text="Reverse Charge">
                                                </dxe:ASPxLabel>--%>

                                                <asp:CheckBox ID="CB_ReverseCharge" runat="server" Text="Reverse Charge" TextAlign="Right" Checked="false"></asp:CheckBox>
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
                                            <%--rev Pallab(id:001)
                                            id:001-- change all grid column width % to px--%>
                                            <dxe:ASPxGridView runat="server" KeyFieldName="QuotationID" EnableRowsCache="false" OnCustomUnboundColumnData="grid_CustomUnboundColumnData"
                                                ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                                OnBatchUpdate="grid_BatchUpdate"
                                                OnCustomCallback="grid_CustomCallback"
                                                OnDataBinding="grid_DataBinding"
                                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                                OnRowInserting="Grid_RowInserting"
                                                OnRowUpdating="Grid_RowUpdating"
                                                OnRowDeleting="Grid_RowDeleting"
                                                OnHtmlRowPrepared="grid_HtmlRowPrepared"
                                                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="150"
                                               
                                                Settings-HorizontalScrollBarMode="Visible"
                                                >
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Columns>
                                                    <%--<dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption="#">--%>
                                                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="40px" VisibleIndex="0" Caption="#">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                                <Image Url="/assests/images/crs.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <%-- <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" Text="New" ForeColor="White">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>--%>
                                                    </dxe:GridViewCommandColumn>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">--%>
                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="40px">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="ComponentNumber" Caption="Document No." VisibleIndex="2" ReadOnly="True" Width="9%">--%>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentNumber" Caption="Document No." VisibleIndex="2" ReadOnly="True" Width="140px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--<dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="14%" ReadOnly="true">--%>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="120px" ReadOnly="true">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4" ReadOnly="True" Width="15%">--%>
                                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4" ReadOnly="True" Width="140px">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <%--<dxe:GridViewCommandColumn VisibleIndex="5" Caption="Addl. Desc." Width="6%">--%>
                                                    <dxe:GridViewCommandColumn VisibleIndex="5" Caption="Addl. Desc." Width="90px">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc" Image-Url="/assests/images/more.png" Image-ToolTip="Warehouse">
                                                                <Image ToolTip="Warehouse" Url="/assests/images/more.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                        <%--<dxe:GridViewDataButtonEditColumn FieldName="DeliverySchedule" Caption="Delivery Schedule" VisibleIndex="6" Width="14%" ReadOnly="true">--%>
                                                        <dxe:GridViewDataButtonEditColumn FieldName="DeliverySchedule" Caption="Delivery Schedule" VisibleIndex="6" Width="140px" ReadOnly="true">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="DeliveryScheduleButnClick" KeyDown="DeliveryScheduleKeyDown" GotFocus="DeliveryScheduleGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="7" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">--%>
                                                        <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="7" Width="100px" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />
                                                            <%--LostFocus="Quantity_lostFocus"--%>
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM(Sale)" VisibleIndex="8" ReadOnly="true" Width="6%">--%>
                                                        <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM(Sale)" VisibleIndex="8" ReadOnly="true" Width="120px">
                                                        <PropertiesTextEdit>
                                                            <%--<ClientSideEvents LostFocus="Uom_LostFocus" />--%>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                    <%--<dxe:GridViewCommandColumn VisibleIndex="9" Caption="Multi UOM" Width="4%">--%>
                                                        <dxe:GridViewCommandColumn VisibleIndex="9" Caption="Multi UOM" Width="140px">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                                                <Image ToolTip="Multi UOM" Url="/assests/images/MultiUomIcon.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <%--Mantis Issue 24425, 24428--%>
                                                    <%--<dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="InvoiceDetails_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="10" Width="6%" ReadOnly="true">--%>
                                                         <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="InvoiceDetails_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="10" Width="160px" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--<dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="InvoiceDetails_AltUOM" ReadOnly="true" VisibleIndex="11" Width="6%">--%>
                                                        <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="InvoiceDetails_AltUOM" ReadOnly="true" VisibleIndex="11" Width="160px">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--VisibleIndex changed for below columns--%>
                                                    <%--End of Mantis Issue 24425, 24428--%>


                                                    <%--Caption="Warehouse"--%>
                                                    <%--<dxe:GridViewCommandColumn VisibleIndex="12" Caption="Stk Details" Width="10%">--%>
                                                        <dxe:GridViewCommandColumn VisibleIndex="12" Caption="Stk Details" Width="100px">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                                <Image ToolTip="Warehouse" Url="/assests/images/warehouse.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                   
                                                   
                                                    <%--Mantis Issue 25377 [ReadOnly="true" added ]--%>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Sale Price" VisibleIndex="13" Width="6%" HeaderStyle-HorizontalAlign="Right">--%>
                                                        <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Sale Price" VisibleIndex="13" Width="120px" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents GotFocus="ProductsGotFocus" LostFocus="SalePriceTextChange" />
                                                            <%--TextChanged="SalePriceTextChange" LostFocus="spLostFocus--%>
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--<dxe:GridViewDataTextColumn FieldName="Discount" Caption="Add/Less Amt" VisibleIndex="14" Width="6%" HeaderStyle-HorizontalAlign="Right">--%>
                                                        <dxe:GridViewDataTextColumn FieldName="Discount" Caption="Add/Less Amt" VisibleIndex="14" Width="120px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" />
                                                            <ClientSideEvents LostFocus="DiscountValueChange" GotFocus="DiscountGotFocus" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                     
                                                   

                                                    <%--<dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="15" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">--%>
                                                        <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="15" Width="120px" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" />
                                                            <ClientSideEvents LostFocus="ProductAmountTextChange" GotFocus="AmountTextFocus" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--<dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="16" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">--%>
                                                        <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="16" Width="100px" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <%--LostFocus="Taxlostfocus"--%>
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="17" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">--%>
                                                        <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="17" Width="120px" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents GotFocus="TotalAmountgotfocus" />
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                   <%-- <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="18" Width="9%" ReadOnly="false">--%>
                                                        <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="18" Width="120px" ReadOnly="false">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Left">

                                                            <Style HorizontalAlign="Left">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                    <%--<dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="18" Caption=" ">--%>
                                                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="50px" VisibleIndex="18" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                                <Image Url="/assests/images/add.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                     <dxe:GridViewDataTextColumn FieldName="StockQuantity" Caption="Stock Qty" VisibleIndex="19" Visible="false">
                                                        <PropertiesTextEdit DisplayFormatString="0.00">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="StockUOM" Caption="Stock UOM" VisibleIndex="20" ReadOnly="true" Visible="false">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Component ID" VisibleIndex="21" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="22" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="23" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="24" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsLinkedProduct" Caption="IsLinkedProduct" VisibleIndex="25" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DetailsId" Caption="Details ID" VisibleIndex="26" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" PropertiesTextEdit-ValidationSettings-ErrorImage-IconID="ghg" Caption="hidden Field Id" VisibleIndex="25" ReadOnly="True" Width="0" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="abcd">
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
                                                    <dxe:GridViewDataTextColumn FieldName="DocDetailsID" Caption="Doc Details ID" VisibleIndex="27" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SONetAmount" Caption="SONetAmount" VisibleIndex="28" ReadOnly="True" Width="0">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn FieldName="DeliveryScheduleID" Caption="Details ID" VisibleIndex="29" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn FieldName="DeliveryScheduleDetailsID" Caption="Details ID" VisibleIndex="30" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto" />
                                                <SettingsBehavior ColumnResizeMode="Disabled" />
                                            </dxe:ASPxGridView>
                                            <%--Rev end Pallab--%>
                                        </div>
                                        <%-- Rev Rajdip --%>
                                        <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                            <ul>
                                                <li class="clsbnrLblTotalQty">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="0.00" ClientInstanceName="cbnrLblTotalQty" />
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
                                                                        <dxe:ASPxLabel ID="bnrLblTaxableAmt" runat="server" Text="Taxable Amt" ClientInstanceName="cbnrLblTaxableAmt" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxableAmtval" runat="server" Text="0.00" ClientInstanceName="cbnrLblTaxableAmtval" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblTaxAmt">
                                                    <div class="horizontallblHolder" id="">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxAmt" runat="server" Text="Tax & Others Amt" ClientInstanceName="cbnrLblTaxAmt" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxAmtval" runat="server" Text="0.00" ClientInstanceName="cbnrLblTaxAmtval" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblAmtWithTax" runat="server" id="oldUnitBanerLbl">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblAmtWithTax" runat="server" Text="Amount" ClientInstanceName="cbnrLblAmtWithTax" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrlblAmountWithTaxValue" runat="server" Text="0.00" ClientInstanceName="cbnrlblAmountWithTaxValue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblInvVal" id="otherChargesId">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="cbnrOtherCharges" runat="server" Text="Additional Amt" ClientInstanceName="cbnrOtherCharges" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrOtherChargesvalue" runat="server" Text="0.00" ClientInstanceName="cbnrOtherChargesvalue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>

                                                <li class="clsbnrLblLessOldVal" style="display: none;">
                                                    <div class="horizontallblHolder" id="">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessOldVal" runat="server" Text="Less Old Unit Value" ClientInstanceName="cbnrLblLessOldVal" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessOldMainVal" runat="server" Text=" 0.00" ClientInstanceName="cbnrLblLessOldMainVal"></dxe:ASPxLabel>
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblLessAdvance" id="idclsbnrLblLessAdvance" runat="server" style="display: none;">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessAdvance" runat="server" Text="Advance Adjusted" ClientInstanceName="cbnrLblLessAdvance" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessAdvanceValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblLessAdvanceValue" />
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
                                                                        <dxe:ASPxLabel ID="bnrLblInvVal" runat="server" Text="Net Amt" ClientInstanceName="cbnrLblInvVal" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblInvValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblInvValue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>




                                                <li class="clsbnrLblInvVal" style="display: none;">
                                                    <div class="horizontallblHolder" style="border-color: #f14327;">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td style="background: #f14327;">
                                                                        <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Running Balance" ClientInstanceName="cbnrLblInvVal" />
                                                                    </td>
                                                                    <td>
                                                                        <strong>
                                                                            <dxe:ASPxLabel ID="lblRunningBalanceCapsul" runat="server" Text="0.00" ClientInstanceName="clblRunningBalanceCapsul" />
                                                                        </strong>
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblInvVal">
                                                    <div runat="server" id="divSendSMS">

                                                        <strong>

                                                            <%-- <input type="checkbox" name="chksendSMS" id="chksendSMS" onclick="SendSMSChk()" />&nbsp;Send SMS--%>
                                                            <asp:HiddenField ID="hdnSendSMS" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnCustMobile" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnsendsmsSettings" runat="server" />
                                                        </strong>

                                                    </div>
                                                </li>

                                            </ul>
                                        </div>
                                        <%-- End Rev Rajdip --%>
                                        <div style="clear: both;"></div>
                                        <br />
                                        <div class="col-md-12" id="divSubmitButton" runat="server">
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords_N" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords_p" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <%--   <asp:Button ID="ASPxButton2" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />--%>
                                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords2" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                            </dxe:ASPxButton>
                                            <%--  Text="T&#818;axes"--%>
                                            <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>
                                            <span id="divTCS" runat="server">
                                                <%--Mantis Issue 24789  [ ClientVisible="false"  added]--%>
                                                <dxe:ASPxButton ID="ASPxButton6" ClientInstanceName="cbtn_TCS" ClientVisible="false" runat="server" AutoPostBack="False" Text="Add TC&#818;S" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {ShowTCS();}" />
                                                </dxe:ASPxButton>
                                            </span>

                                            <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />



                                            <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />

                                            <ucOTC:OtherTermsAndCondition runat="server" ID="OtherTermsAndCondition" />
                                            <span id="spnBillDespatch" runat="server">
                                                <dxe:ASPxButton ID="btn_BillDespatch" ClientInstanceName="cbtn_BillDespatch" runat="server" AutoPostBack="False" Text="Bill from/Despatch from" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Save_BillDespatch();}" />
                                                </dxe:ASPxButton>
                                            </span>

                                            <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                                            <asp:HiddenField ID="hfControlData" runat="server" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="SI" />


                                            <asp:HiddenField runat="server" ID="hfOtherTermsConditionData" />
                                            <asp:HiddenField runat="server" ID="hfOtherTermsConditionDocType" Value="SI" />
                                            <%-- onclick=""--%>
                                            <%--<a href="javascript:void(0);" id="btnAddNew" runat="server" class="btn btn-primary"><span>[A]ttachment(s)</span></a>--%>
                                            <%--<dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[A]ttachment(s)" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>--%>
                                            <%--<a class="btn btn-primary" href="javascript:void(0);" onclick="
                                                
                                                
                                                
                                                ()"><span>[B]illing/Shipping</span> </a>--%>
                                        </div>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                            <%--test generel--%>
                        </dxe:TabPage>
                        <%--<dxe:TabPage Name="[A]ttachment(s)" Visible="false" Text="[A]ttachment(s)">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>--%>
                        <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">



                                    <%--<dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server" ContentUrl="AddArea_PopUp.aspx"
                                        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                                        Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                        <ContentCollection>
                                            <dxe:PopupControlContentControl runat="server">
                                            </dxe:PopupControlContentControl>
                                        </ContentCollection>
                                        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                    </dxe:ASPxPopupControl>--%>

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
            <%--SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">--%>


            <dxe:ASPxPopupControl ID="popupBillDsep" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cpopupBillDsep" Height="350px"
                Width="750px" HeaderText="Bill from/Despatch from" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <div class="row">
                            <div class="col-md-6 mbot5" id="BillDivBilling">
                                <h5 class="headText">Bill From</h5>
                                <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">

                                    <div style="padding-right: 8px; padding-top: 5px">


                                        <div class="col-md-4" style="height: auto; margin-bottom: 5px;">

                                            <asp:Label ID="LabelAddress1" runat="server" Text="Address1:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>


                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="BtxtAddress1" MaxLength="80" ClientInstanceName="BctxtAddress1"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>

                                            </div>
                                        </div>
                                        <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                            Address2:
                                                                           

                                        </div>

                                        <div class="col-md-8">

                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="BtxtAddress2" MaxLength="80" ClientInstanceName="BctxtAddress2"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                            Address3: 
                                        </div>


                                        <div class="col-md-8">

                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="BtxtAddress3" MaxLength="80" ClientInstanceName="BctxtAddress3"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                            Landmark (Location ):
                                                                             

                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="Btxtlandmark" MaxLength="80" ClientInstanceName="Bctxtlandmark"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>

                                        <div class="clear"></div>

                                        <div class="col-md-4" style="height: auto;">

                                            <asp:Label ID="Label8" runat="server" Text="Pin/Zip (6 Characters):" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content relative">

                                                <dxe:ASPxTextBox ID="BtxtbillingPin" ClientInstanceName="BctxtbillingPin"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;-0..999999&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="BillBillingPinChange" />
                                                </dxe:ASPxTextBox>
                                                <asp:HiddenField ID="BhdBillingPin" runat="server"></asp:HiddenField>


                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-4" style="height: auto;">

                                            <asp:Label ID="Label2" runat="server" Text="Country:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="BtxtbillingCountry" ClientEnabled="false" MaxLength="80" ClientInstanceName="BctxtbillingCountry"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>

                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-4" style="height: auto;">

                                            <asp:Label ID="Label4" runat="server" Text="State:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="BtxtbillingState" ClientEnabled="false" MaxLength="80" ClientInstanceName="BctxtbillingState"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>

                                            </div>

                                        </div>
                                        <div class="clear"></div>
                                        <%--start of City/district.--%>
                                        <div class="col-md-4" style="height: auto;">
                                            <asp:Label ID="Label6" runat="server" Text="City/District:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="BtxtbillingCity" ClientEnabled="false" MaxLength="80" ClientInstanceName="BctxtbillingCity"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>

                                            </div>

                                        </div>
                                        <div class="clear"></div>


                                        <div class="Left_Content relative">
                                            <asp:HiddenField ID="BhdStateIdBilling" runat="server" />

                                        </div>




                                        <div class="Left_Content relative">
                                            <asp:HiddenField ID="BhdStateCodeBilling" runat="server" />

                                        </div>




                                        <div class="Left_Content relative">
                                            <asp:HiddenField ID="BhdCountryIdBilling" runat="server" />

                                        </div>

                                        <div class="Left_Content relative">
                                            <asp:HiddenField ID="BhdCityIdBilling" runat="server" />

                                        </div>



                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 mbot5" id="DDivShipping">
                                <h5 class="headText">Despatch From</h5>
                                <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">


                                    <div style="padding-right: 8px; padding-top: 5px">



                                        <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                            <asp:Label ID="lblSAddress1" runat="server" Text="Address1:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>


                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="DtxtsAddress1" MaxLength="80" ClientInstanceName="DctxtsAddress1"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>

                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                            Address2:
                                                                           
                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content">
                                                <dxe:ASPxTextBox ID="DtxtsAddress2" MaxLength="80" ClientInstanceName="DctxtsAddress2"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                            Address3: 
                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content">
                                                <dxe:ASPxTextBox ID="DtxtsAddress3" MaxLength="80" ClientInstanceName="DctxtsAddress3"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                            Landmark (Location ): 
                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content">
                                                <dxe:ASPxTextBox ID="Dtxtslandmark" MaxLength="80" ClientInstanceName="Dctxtslandmark"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="clear"></div>


                                        <div class="col-md-4" style="height: auto;">

                                            <asp:Label ID="Label9" runat="server" Text="Pin/Zip (6 Characters):" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content relative">

                                                <dxe:ASPxTextBox ID="DtxtShippingPin" MaxLength="6" ClientInstanceName="DctxtShippingPin"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;-0..999999&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="DespatchShippingPinChange" />
                                                </dxe:ASPxTextBox>
                                                <asp:HiddenField ID="DhdShippingPin" runat="server"></asp:HiddenField>




                                            </div>
                                        </div>
                                        <div class="clear"></div>





                                        <div class="col-md-4" style="height: auto;">

                                            <asp:Label ID="Label3" runat="server" Text="Country:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="DtxtshippingCountry" ClientEnabled="false" MaxLength="80" ClientInstanceName="DctxtshippingCountry"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>

                                            </div>
                                        </div>
                                        <div class="clear"></div>

                                        <div class="col-md-4" style="height: auto;">

                                            <asp:Label ID="Label5" runat="server" Text="State:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="DtxtshippingState" ClientEnabled="false" MaxLength="80" ClientInstanceName="DctxtshippingState"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>

                                            </div>
                                        </div>
                                        <div class="clear"></div>



                                        <div class="col-md-4" style="height: auto;">

                                            <asp:Label ID="Label7" runat="server" Text="City/District:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                        </div>
                                        <div class="col-md-8">

                                            <div class="Left_Content relative">
                                                <dxe:ASPxTextBox ID="DtxtshippingCity" ClientEnabled="false" MaxLength="80" ClientInstanceName="DctxtshippingCity"
                                                    runat="server" Width="100%">
                                                </dxe:ASPxTextBox>

                                            </div>
                                        </div>
                                        <div class="clear"></div>




                                        <div class="Left_Content relative">
                                            <asp:HiddenField ID="DhdStateCodeShipping" runat="server" />
                                        </div>



                                        <div class="Left_Content relative">
                                            <asp:HiddenField ID="DhdCountryIdShipping" runat="server" />

                                        </div>


                                        <div class="Left_Content relative">
                                            <asp:HiddenField ID="DhdStateIdShipping" runat="server" />

                                        </div>
                                        <div class="Left_Content relative">
                                            <asp:HiddenField ID="DhdCityIdShipping" runat="server" />

                                        </div>






                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="clear" style="margin-bottom: 5px"></div>
                        <dxe:ASPxButton ID="btnSave_billdespatch" ClientInstanceName="cbtnSave_billdespatch" runat="server"
                            AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                            <ClientSideEvents Click="function (s, e) {Validationbilldespatch();}" />
                        </dxe:ASPxButton>
                        <dxe:ASPxButton ID="btnCancel_billdespatch" ClientInstanceName="cbtnCancel_billdespatch" runat="server"
                            AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" UseSubmitBehavior="false">
                            <ClientSideEvents Click="function (s, e) {ValidationbilldespatchCancel();}" />
                        </dxe:ASPxButton>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

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
                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                        <dxe:ASPxButton ID="btn_SaveTax" UseSubmitBehavior="false" ClientInstanceName="cbtn_SaveTax" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton5" UseSubmitBehavior="false" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
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
                                            <div style="margin-bottom: 5px;">
                                                Warehouse
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                                    TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                                    <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange()}" EndCallback="CmbWarehouseEndCallback"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                                <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
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
                                                <span id="spnCmbBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-4" id="div_Serial">
                                            <div style="margin-bottom: 5px;">
                                                Serial No &nbsp;&nbsp; (
                                                <input type="checkbox" id="myCheck" name="BarCode" onchange="AutoCalculateMandateOnChange(this)">Barcode )
                                            </div>
                                            <div class="" id="divMultipleCombo">
                                                <%--<dxe:ASPxComboBox ID="CmbSerial" EnableIncrementalFiltering="True" ClientInstanceName="cCmbSerial"
                                                    TextField="SerialName" ValueField="SerialID" runat="server" Width="100%" OnCallback="CmbSerial_Callback">
                                                </dxe:ASPxComboBox>--%>
                                                <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="85%" CssClass="pull-left" runat="server" AnimationType="None">
                                                    <DropDownWindowStyle BackColor="#EDEDED" />
                                                    <DropDownWindowTemplate>
                                                        <dxe:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" OnCallback="CmbSerial_Callback"
                                                            runat="server">
                                                            <Border BorderStyle="None" />
                                                            <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                            <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" EndCallback="listBoxEndCall" />
                                                        </dxe:ASPxListBox>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="padding: 4px">
                                                                    <dxe:ASPxButton ID="ASPxButton4" AutoPostBack="False" UseSubmitBehavior="false" runat="server" Text="Close" Style="float: right">
                                                                        <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DropDownWindowTemplate>
                                                    <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" GotFocus="function(s, e){ s.ShowDropDown(); }" />
                                                </dxe:ASPxDropDownEdit>
                                                <span id="spncheckComboBox" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <div class="pull-left">
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
                                            <div style="margin-bottom: 2px;">
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
                                            <div class="Left_Content" style="padding-top: 14px">
                                                <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" UseSubmitBehavior="False" Text="Add" CssClass="btn btn-primary">
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
                                                    <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete">
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
                                        <dxe:ASPxButton ID="btnWarehouseSave" UseSubmitBehavior="false" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
            </div>
            <div>
                <asp:HiddenField ID="HdUpdateMainGrid" runat="server" />
                <asp:HiddenField ID="hdfIsDelete" runat="server" />
                <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
                <asp:HiddenField ID="hdfProductID" runat="server" />
                <asp:HiddenField ID="hdfProductType" runat="server" />
                <asp:HiddenField ID="hdfProductSerialID" runat="server" />
                <asp:HiddenField ID="hdnProductQuantity" runat="server" />
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hdnPageEditId" runat="server" />
                <asp:HiddenField ID="hdnDeleteSrlNo" runat="server" />
                <asp:HiddenField ID="IsDiscountPercentage" runat="server" />
                <asp:HiddenField ID="ProductType" runat="server" />
                <%--Subhra--%>
                <asp:HiddenField ID="hdntab2" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnSalesManAgentId" runat="server" />
                <%--Rajdip--%>
                <asp:HiddenField runat="server" ID="hdnPlaceShiptoParty" />
                <%--End Rajdip--%>

                <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
                <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
                <asp:HiddenField runat="server" ID="hdnProjectSelectInEntryModule" />
                <asp:HiddenField runat="server" ID="hdnHierarchySelectInEntryModule" />
                <%-- Rev work start 28.06.2022 Mantise no:24949--%>
                <asp:HiddenField ID="hdnSettings" runat="server"></asp:HiddenField>
                <%-- Rev work close 28.06.2022 Mantise no:24949--%>
            </div>

            <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="CallbackPanelEndCall" />
            </dxe:ASPxCallbackPanel>



            <%--End Sudip--%>

            <asp:HiddenField ID="hdnCustomerId" runat="server" />
            <asp:HiddenField ID="hdnAddressDtl" runat="server" />
            <%--Debu Section--%>

            <%--Batch Product Popup Start--%>

            <%--<dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <HeaderTemplate>
                    <span>Select Product</span>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By product Name</strong></label>
                        <span style="color: red;">[Press ESC key to Cancel]</span>
                        <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="lsmdsProduct" ClientInstanceName="cproductLookUp"
                            KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="Products_Description" Caption="Name" Width="240">
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
                                                  //<dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
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
            </dxe:ASPxPopupControl>--%>

            <%--  <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="prc_CRMSalesInvoice_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="AllProductDetails" />
                    <asp:ControlParameter DefaultValue="Y" Name="InventoryType" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddlInventory" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>--%>

            <%-- <asp:SqlDataSource runat="server" ID="ProductDataSource"
                SelectCommand="prc_SalesCRM_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
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
                                        <asp:Button ID="Button1" UseSubmitBehavior="false" runat="server" Text="Ok" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
                                        <asp:Button ID="Button2" UseSubmitBehavior="false" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
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

        </asp:Panel>
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxDateEdit ID="dt_PlQuoteExpiry" runat="server" Date="" Width="100%" EditFormatString="dd-MM-yyyy" ClientInstanceName="tenddate" TabIndex="4">
            <ClientSideEvents DateChanged="Enddate" />
        </dxe:ASPxDateEdit>
    </div>
    <%--Compnent Tag Start--%>

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
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                    OnCustomCallback="cgridProducts_CustomCallback" OnDataBinding="grid_Products_DataBinding"
                    Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                    <SettingsPager Visible="false"></SettingsPager>
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number" Settings-AllowFilterBySearchPanel="True" Settings-AllowAutoFilter="True">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsDataSecurity AllowEdit="true" />
                </dxe:ASPxGridView>
                <div class="text-center">
                    <asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" UseSubmitBehavior="false" />
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <%--Compnent Tag End--%>

    <asp:HiddenField runat="server" ID="hdnSegment1" />
    <asp:HiddenField runat="server" ID="hdnSegment2" />
    <asp:HiddenField runat="server" ID="hdnSegment3" />
    <asp:HiddenField runat="server" ID="hdnSegment4" />
    <asp:HiddenField runat="server" ID="hdnSegment5" />

    <asp:HiddenField runat="server" ID="hdnValueSegment1" />
    <asp:HiddenField runat="server" ID="hdnValueSegment2" />
    <asp:HiddenField runat="server" ID="hdnValueSegment3" />
    <asp:HiddenField runat="server" ID="hdnValueSegment4" />
    <asp:HiddenField runat="server" ID="hdnValueSegment5" />

    <div class="modal fade" id="Segment1Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment1header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment1keydown(event)" id="txtSegment1Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment1Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
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
    <div class="modal fade" id="Segment2Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment2Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment2keydown(event)" id="txtSegment2Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment2Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
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
    <div class="modal fade" id="Segment3Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment3Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment3keydown(event)" id="txtSegment3Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment3Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
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
    <div class="modal fade" id="Segment4Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment4Header">Segment4 Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment4keydown(event)" id="txtSegment4Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment4Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
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
    <div class="modal fade" id="Segment5Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment5Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment5keydown(event)" id="txtSegment5Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment5Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
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

    <%-- Customer Payment & Recipt --%>

    <dxe:ASPxPopupControl ID="apcReciptPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="capcReciptPopup" Height="630px"
        Width="1200px" HeaderText="Customer Receipt/Payment" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <HeaderTemplate>
            <span>Customer Receipt/Payment</span>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <%-- Customer Payment & Recipt --%>

    <%-- <asp:SqlDataSource runat="server" ID="dsCustomer" 
        SelectCommand="prc_CRMSalesInvoice_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateCustomerDetail" />
        </SelectParameters>
    </asp:SqlDataSource>--%>

    <%--  <dx:LinqServerModeDataSource ID="lsmdCustomer" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="v_CustomerList" />--%>

    <%-- Customer Payment & Recipt --%>

    <%-- UDF Module Start --%>
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
    <%-- UDF Module End--%>

    <%--Customer Popup--%>
    <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="750px"
        Width="1020px" HeaderText="Add New Customer" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <HeaderTemplate>
            <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            AspxDirectAddCustPopup.Hide();
                                                        }" />
            </dxe:ASPxImage>
            <span>Add New Customer</span>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <%-- TaxDetails HiddenField Field --%>
    <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
    <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
    <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
    <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
    <%-- TaxDetails HiddenField Field --%>
    <%-- Rev Sayantani--%>
    <asp:HiddenField runat="server" ID="ISAllowBackdatedEntry" />
    <asp:HiddenField runat="server" ID="hdnCrDateMandatory" />
    <asp:HiddenField runat="server" ID="hdnEntityType" />
    <asp:HiddenField runat="server" ID="hdAddOrEdit" />
    <%--  End of Rev Sayantani--%>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    
    <%--Rev 4.0--%>
    <dxe:ASPxLoadingPanel ID="LoadingPanelMultiUOM" runat="server" ClientInstanceName="LoadingPanelMultiUOM" ContainerElementID="divMultiUOM"
        Modal="True">
    </dxe:ASPxLoadingPanel>
     <dxe:ASPxLoadingPanel ID="LoadingPanelMultiUOMSave" runat="server" ClientInstanceName="LoadingPanelMultiUOMSave" ContainerElementID="divPageGeneral"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <%--End of Rev 4.0--%>

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
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name,Unique Id and Phone No." />
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
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Code or Name" />

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
                <%-- <div class="modal-footer">
                      <% if (rightsProd.CanAdd)
                       { %>
                    <button type="button" class="btn btn-success btn-radius" onclick="fn_PopOpen();">
                        <span class="btn-icon"><i class="fa fa-plus"></i></span>
                        Add New
                    </button>
                    <% } %>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>--%>
                <div class="modal-footer">
                    <% if (rightsProd.CanAdd)
                       { %>
                    <button type="button" class="btn btn-success" onclick="fn_PopOpen();">
                        <span class="btn-icon"><i class="fa fa-plus"></i></span>
                        Add New
                    </button>
                    <% } %>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Product Modal -->
    <%--SalesMan/Agent--%>
    <div class="modal fade" id="SalesManModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <%--Rev work start 24.06.2022 mantise no:0024987 --%>
                    <%--<h4 class="modal-title">SalesMan/Agent Search</h4>--%>
                    <h4 class="modal-title" runat="server" id="hs1">Salesman/Agent Search</h4>
                    <%--Rev work close 24.06.2022 mantise no:0024987--%>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SalesMankeydown(event)" id="txtSalesManSearch" autofocus width="100%" placeholder="Search By SalesMan/Agent Name" />

                    <div id="SalesManTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
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
    <%--SalesMan/Agent--%>

    <dxe:ASPxPopupControl ID="PosView" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cPosView" Height="650px"
        Width="1200px" HeaderText="Product" Modal="true">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="1150px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeMultiUOM(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <%--Rev 4.0 [ id="divMultiUOM" added ] --%>
                <div class="Top clearfix" id="divMultiUOM">



                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">

                        <table class="eqTble" >
                            <tr>
                                <%--Mantis Issue 24425, 24428--%>
                                <dxe:GridViewDataTextColumn Caption="MultiUOMSR No"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" width="0">
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24425, 24428--%>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--Rev Sanchita--%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal" />--%>
                                               <%--Rev 4.0 --%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onchange="CalcBaseRate()" />--%>
                                                <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onfocusout="CalcBaseRate()" />
                                                <%--End of Rev 4.0--%>
                                                <%--End of Rev Sanchita--%>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="Left_Content" style="">
                                        <div>
                                            <label style="text-align: right;">Base UOM</label>
                                        </div>
                                        <div>
                                            <dxe:ASPxComboBox ID="cmbUOM" ClientInstanceName="ccmbUOM" runat="server" SelectedIndex="0" DataSourceID="UomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24425, 24428--%>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <%--Rev 4.0--%>
                                            <%--<dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true"></dxe:ASPxTextBox>--%>
                                            <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.00" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..99&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true"></dxe:ASPxTextBox>
                                            <%--End of Rev 4.0--%>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Mantis Issue 24425, 24428--%>
                                <td>
                                    <span style="font-size: 22px; padding-top: 20px; display: inline-block;">=</span>
                                </td>
                                <td>
                                    <div>
                                        <div>
                                            <label style="text-align: right;">Alt. UOM</label>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="cmbSecondUOM" ClientInstanceName="ccmbSecondUOM" runat="server" SelectedIndex="0" DataSourceID="AltUomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                                <ClientSideEvents TextChanged="function(s,e) { PopulateMultiUomAltQuantity();}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt. Quantity </label>
                                        </div>
                                        <div>
                                            <%--  <input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/> --%>
                                            <dxe:ASPxTextBox ID="AltUOMQuantity" Width="80px" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <%--Mantis Issue 24425, 24428--%>
                                                <%--Rev 4.0--%>
                                                <%--<ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />--%>
                                                <ClientSideEvents LostFocus="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Rev 4.0--%>
                                                <%--End of Mantis Issue 24425, 24428--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24425, 24428--%>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <%--Rev 4.0--%>
                                            <%--<dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">--%>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.00" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..99&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <%--<ClientSideEvents TextChanged="function(s,e) { CalcBaseRate();}" />--%>
                                                <ClientSideEvents LostFocus="function(s,e) { CalcBaseRate();}" />
                                                 <%--End of Rev 4.0--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                        </div>
                                        <%--Rev 4.0 [ class="mlableWh" added --%>
                                        <div class="mlableWh" >
                                            <%--<label class="checkbox-inline mlableWh">
                                                <asp:CheckBox ID="chkUpdateRow" Checked="false" runat="server" ></asp:CheckBox>
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>--%>
                                            <%--Rev 4.0 [ class="mlableWh" removed --%>
                                            <label class="checkbox-inline ">
                                                <input type="checkbox" id="chkUpdateRow" />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>


                                </td>
                                <%--End of Mantis Issue 24425, 24428--%>
                                <%--Rev 5.0: For "Add" button in separate table row(tr)--%>
                                </tr>
                                <tr>
                                    <td style="padding-top: 10px;">
                                        <dxe:ASPxButton ID="btnMUltiUOM" UseSubmitBehavior="false" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function(s, e) { if(!document.getElementById('myCheck').checked)  {SaveMultiUOM();}}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                                <%--Rev end 5.0--%>
                        </table>

                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="grid_MultiUOM" runat="server" KeyFieldName="AltUomId;AltQuantity" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgrid_MultiUOM" OnCustomCallback="MultiUOM_CustomCallback" OnDataBinding="MultiUOM_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>

                                <%--Mantis Issue 24425, 24428--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24425, 24428--%>

                                <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="AltUOM"
                                    VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Alt. Quantity" FieldName="AltQuantity"
                                    VisibleIndex="3" HeaderStyle-HorizontalAlign="Right">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="" FieldName="UomId" Width="0px"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="" FieldName="AltUomId" Width="0px"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>

                                <%--Mantis Issue 24425, 24428--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24425, 24428--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>','<%#Eval("DetailsId") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>

                                        <%--Mantis Issue 24425, 24428--%>
                                        <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                        <%--End of Mantis Issue 24425, 24428--%>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnMultiUOMEndCallback" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="ASPxButton7" UseSubmitBehavior="false" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>

                             <%--Rev 4.0--%>
                            <label id="lblInfoMsg" style="font-weight:bold; color:red; " > </label>
                            <%--End of Rev 4.0--%>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <asp:SqlDataSource ID="UomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:HiddenField ID="hddnuomFactor" runat="server" />
    <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />
    <asp:HiddenField ID="hdnQuantitySL" runat="server" />
    <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
    <asp:HiddenField runat="server" ID="hdnQty" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
      <asp:HiddenField ID="hddnDeliveryScheduleRequired" runat="server" />
    <asp:HiddenField ID="hdnShowAlertZeroTaxSI" runat="server" Value="0" />


    <dxe:ASPxCallbackPanel runat="server" ID="callback_InlineRemarks" ClientInstanceName="ccallback_InlineRemarks" OnCallback="callback_InlineRemarks_Callback">
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
                                <asp:Label ID="lblInlineRemarks" runat="server" Text="Addl. Desc."></asp:Label>

                                <asp:TextBox ID="txtInlineRemarks" runat="server" ValidateRequestMode="Disabled" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" MaxLength="5000"></asp:TextBox>
                            </div>


                            <div class="clearfix">
                                <br />
                                <div style="align-content: center">
                                    <dxe:ASPxButton ID="btnSaveInlineRemarks" ClientInstanceName="cbtnSaveInlineRemarks" Width="50px" runat="server" AutoPostBack="false" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">

                                        <ClientSideEvents Click="function(s, e) {FinalRemarks();}" />
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
    <div id="tcsModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">TCS Calculation</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-3">
                            <label>
                                TCS Section
                            </label>
                            <dxe:ASPxTextBox runat="server" ClientEnabled="false" ID="txtTCSSection" ClientInstanceName="ctxtTCSSection">
                            </dxe:ASPxTextBox>
                        </div>
                        <div class="col-md-3">
                            <label>
                                TCS Applicable Amount
                            </label>
                            <dxe:ASPxTextBox runat="server" ClientEnabled="true" ID="txtTCSapplAmount" ClientInstanceName="ctxtTCSapplAmount">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </div>

                        <div class="col-md-3">
                            <label>
                                TCS Percentage
                            </label>
                            <dxe:ASPxTextBox runat="server" ClientEnabled="true" ID="txtTCSpercentage" ClientInstanceName="ctxtTCSpercentage">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </div>

                        <div class="col-md-3">
                            <label>
                                TCS Amount
                            </label>
                            <dxe:ASPxTextBox runat="server" ClientEnabled="true" ID="txtTCSAmount" ClientInstanceName="ctxtTCSAmount">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="GridTCSdocs" runat="server" ClientInstanceName="cGridTCSdocs" Width="100%"
                                KeyFieldName="SLNO" OnDataBinding="GridTCSdocs_DataBinding" OnCustomCallback="GridTCSdocs_CustomCallback">
                                <Columns>

                                    <dxe:GridViewDataColumn FieldName="SLNO" Visible="true" VisibleIndex="1" Caption="SL#" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Invoice_Number" Visible="true" VisibleIndex="2" Caption="Doc. No." Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="3" Caption="Unit" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Doc_Type" Visible="true" VisibleIndex="4" Caption="Doc. Type" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Invoice_Date" Visible="true" VisibleIndex="5" Caption="Doc. Date" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataTextColumn FieldName="TaxableAmount" Visible="true" VisibleIndex="6" Caption="Taxable Amount" Settings-AutoFilterCondition="Contains" Width="200px">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="NetAmount" Visible="true" VisibleIndex="7" Caption="Net. Amount." Settings-AutoFilterCondition="Contains" Width="200px">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="TaxableRunning" Visible="true" VisibleIndex="8" Caption="Taxable Aggr." Settings-AutoFilterCondition="Contains" Width="200px">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>

                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="NetRunning" Visible="true" VisibleIndex="9" Caption="Net. Aggr." Settings-AutoFilterCondition="Contains" Width="200px">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                    </dxe:GridViewDataTextColumn>

                                </Columns>

                            </dxe:ASPxGridView>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" data-dismiss="modal">Save</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade pmsModal w40" id="exampleModalSI" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Upload Confirmation</h5>

                </div>
                <div class="modal-body poppins">
                    <div class="text-center">
                        <img src="../../../assests/images/invoiceII.png" style="width: 70px; margin-bottom: 15px;" />
                    </div>
                    <div>
                        <%--<input type="file" class="dropify" data-height="80" />--%>
                    </div>
                    <div class="text-center pTop10">
                        <table class="ppTabl ">
                            <tr>
                                <td>Invoice Number :</td>
                                <td><b id="lblInvNUmber"></b></td>
                            </tr>
                            <tr>
                                <td>Date : </td>
                                <td><b id="lblInvDate"></b></td>
                            </tr>
                            <tr>
                                <td>Customer : </td>
                                <td><b id="lblCust"></b></td>
                            </tr>
                            <tr>
                                <td>Amount : </td>
                                <td><b id="lblAmount"></b></td>
                            </tr>
                        </table>
                        <div class="empht">Do you want to procced with upload ?</div>

                    </div>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-info" type="button" onclick="UploadGridbind()">Upload</button>
                    <button class="btn btn-danger" type="button" data-dismiss="modal" onclick="UploadGridbindCancel()">Later</button>
                </div>
            </div>
        </div>
    </div>



    <div class="bcShad "></div>
    <div class="popupSuc ">
        <div style="background: #467bbd; color: #fff; text-align: center; padding: 7px; font-size: 14px;">
            Important Message
        </div>
        <div class="text-center">
            <span class="cnIcon"><i class="fa fa-check" aria-hidden="true"></i></span>
        </div>
        <div class="bInfoIt">
            <p style="font-size: 15px; color: #e68710; margin-bottom: 10px;">Document has been uploaded successfully to GSTN server</p>
            <p style="font-size: 14px; color: blue;">IRN :<a id="IrnNumber"></a></p>
        </div>
        <table class="ppTabl fontSmall">
            <tr>
                <td>Invoice Number :</td>
                <td><b id="IrnlblInvNUmber"></b></td>
            </tr>
            <tr>
                <td>Date : </td>
                <td><b id="IrnlblInvDate"></b></td>
            </tr>
            <tr>
                <td>Customer : </td>
                <td><b id="IrnlblCust"></b></td>
            </tr>
            <tr>
                <td>Amount : </td>
                <td><b id="IrnlblAmount"></b></td>
            </tr>
        </table>
        <div style="text-align: center; padding: 14px; background: antiquewhite;">
            <button class="okbtn btn btn-primary" type="button" onclick="IrnGrid()">OK</button>
        </div>
    </div>

    <dxe:ASPxPopupControl ID="AssignmentPopUp" runat="server" Width="800"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cAssignmentPopUp" Height="500"
        HeaderText="Stock Position" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div id="BranchAssignmentHeader">
                    <dxe:ASPxCallbackPanel runat="server" ID="BranchRequUpdatePanel" ClientInstanceName="cBranchRequUpdatePanel">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">

                                <table class="smllpad">
                                    <tr>
                                        <td style="width: 110px"></td>
                                        <td>
                                            <label>Unit</label></td>
                                        <td>
                                            <label>Warehouse</label></td>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td style="padding-right: 10px">
                                            <label>Assigned To</label></td>
                                        <td class="relative">
                                            <dxe:ASPxComboBox ID="AssignedBranch" runat="server" ClientInstanceName="cAssignedBranch" Width="100%">
                                                <%-- <ClientSideEvents SelectedIndexChanged="AssignedBranchSelectedIndexChanged"></ClientSideEvents>--%>
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryBranchAssign" style="display: none" class="errorField">
                                                <img id="MandatoryBranchAssignid" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                            </span>
                                        </td>
                                        <td class="relative">
                                            <dxe:ASPxComboBox ID="AssignedWareHouse" runat="server" OnCallback="AssignedWareHouse_Callback" ClientInstanceName="cAssignedWareHouse" SelectedIndex="1" Width="100%">
                                            </dxe:ASPxComboBox>
                                            <span id="mandetoryAssignedWareHouse" style="display: none" class="errorField">
                                                <img id="idmandetoryAssignedWareHouse" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                            </span>
                                        </td>
                                        <%--<td>
                                            <input type="button" value="Assign" class="btn btn-primary" onclick="AssignBranchToThisInvoice()" />
                                            <input type="button" value="Cancel" class="btn btn-danger" onclick="CancelBranchToThisInvoice()" />
                                        </td>--%>
                                    </tr>
                                </table>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </div>
                <table class="smllpad" style="margin-top: 15px;">
                    <tr>

                        <td style="width: 110px">Select Unit To View Stock </td>
                        <td>
                            <dxe:ASPxComboBox ID="BranchAssignmentBranch" runat="server" ClientInstanceName="cBranchAssignmentBranch" Width="100%">
                                <%--<ClientSideEvents SelectedIndexChanged="BranchAssignmentBranchSelectedIndexChanged"></ClientSideEvents>--%>
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <a href="#" onclick="updateAssignmentGrid()">
                                <button type="button" class="btn btn-primary "><i class="fa fa-search" style="" aria-hidden="true"></i>View Stock</button></a>
                            <%--   <input type="button" value="Show Stock" class="btn btn-primary" onclick="updateAssignmentGrid()" />--%>
                        </td>

                    </tr>

                </table>


                <dxe:ASPxGridView ID="AssignmentGrid" runat="server" KeyFieldName="InvoiceDetails_Id" AutoGenerateColumns="False"
                    Width="100%" ClientInstanceName="cAssignmentGrid" OnCustomCallback="AssignmentGrid_CustomCallback" KeyboardSupport="true"
                    SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="AssignmentGrid_DataBinding" SettingsBehavior-AllowFocusedRow="true">
                    <Columns>


                        <dxe:GridViewDataTextColumn Caption="Code" FieldName="sProducts_Code"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Name" FieldName="sProducts_Description"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Available Stock" FieldName="availableQty"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Curently Invoiced" FieldName="InvoicedBalance"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Actual Balance" FieldName="Actual_Balance"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                    </Columns>

                    <%--<ClientSideEvents EndCallback="AssignmentGridEndCallback" />--%>


                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>


                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                </dxe:ASPxGridView>

            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
    <asp:HiddenField runat="server" ID="ProductMinPrice" />
    <asp:HiddenField runat="server" ID="ProductMaxPrice" />
    <asp:HiddenField runat="server" ID="hdnRateType" />
    <asp:HiddenField ID="hdnRDECId" runat="server" />
    <asp:HiddenField ID="hdnAutoPrint" runat="server" />
    <asp:HiddenField ID="hdnIsBranchEInvoice" runat="server" />
    <asp:HiddenField runat="server" ID="hdnPricingDetail" />
    <asp:HiddenField runat="server" ID="hdnSalesRateBuyRateChecking" />
    <asp:HiddenField runat="server" ID="hdnDocumentSegmentSettings" />
    <asp:HiddenField runat="server" ID="hdnProductID" />
    <asp:HiddenField runat="server" ID="hdnStockPositionShow" />
    <asp:HiddenField runat="server" ID="hdnBillDepatchsetting" />
    <asp:HiddenField runat="server" ID="hdnSalesOrderItemNegative" />
    <asp:HiddenField runat="server" ID="hdnProductDaynamicRate" />
    <%--Rev work start 24.06.2022 mantise no:0024987--%>
    <asp:HiddenField runat="server" ID="hdnCoordinate" />
    <%--Rev work start 24.06.2022 mantise no:0024987--%>
    <%--Rev 2.0--%>
    <asp:HiddenField runat="server" ID="hdnIsToleranceInSalesOrder" />
    <%--End of Rev 2.0--%>

     <!--Schedule Modal -->
    <div class="modal fade" id="ScheduleModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Schedule Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Schedulekeydown(event)" id="txtScheduleSearch" autofocus width="100%" placeholder="Search By Product Code or Name" />

                    <div id="ScheduleTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Delivery Schedule Number</th>
                                <th>Product Name</th>
                                <th>Schedule Balance Quantity</th> 
                                 <th>Schedule Serial No</th>
                                <th>Schedule Delivery Date</th>
                            </tr>
                        </table>
                    </div>
                </div>
               
            </div>
        </div>
    </div>
    <!--Schedule Modal -->
</asp:Content>
