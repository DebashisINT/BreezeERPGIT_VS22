<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" EnableViewStateMac="false" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="ReturnManual.aspx.cs" Inherits="ERP.OMS.Management.Activities.ReturnManual" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<%-- Date: 31-05-2017    Author: Kallol Samanta  [START] --%>
<%-- Details: Billing/Shipping user control integration --%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%-- Date: 31-05-2017    Author: Kallol Samanta  [END] --%>

<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/PosSalesInvoice.css" rel="stylesheet" />
     <script src="JS/ReturnManual.js?v=2.0"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>
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
        #grid_DXMainTable > tbody > tr > td:last-child > div {
            display: none !important;
        }

        .classout {
            text-transform: none !important;
        }

        .dynamicPopupTbl > tbody > tr > td input {
            border: none !important;
            cursor: pointer;
            background: transparent !important;
            height: 14px;
            margin-bottom: 0;
            font: 12px Tahoma, Geneva, sans-serif;
            width: 400px;
        }
    </style>

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

        .eqTble > tbody > tr > td {
            padding: 0 15px;
            vertical-align: top;
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

        .validReasonclass {
            position: absolute;
            right: 0px;
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
    
<%-- <style>
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

    <%--Batch Product Popup Start--%>

    
    <style>
        .col-md-2 > label, .col-md-2 > span,
        .col-md-1 > label, .col-md-1 > span {
            margin-top: 8px;
            display: inline-block;
        }
    </style>
    <%--Batch Product Popup End--%>

    <style>
       body{
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
        .fileuploader #upload-label{
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
        .fileuploader.active{
          background: #fff;
        }
        .fileuploader.active #upload-label{
          background: #fff;
          color: #e7615c;
        }

        .fileuploader #upload-label i:hover {
            color: #444;
            font-size: 9.4rem;
            -webkit-transition: width 2s;
        }

        .fileuploader #upload-label span.title{
          font-size: 1em;
          font-weight: bold;
          display: block;
        }

        span.tittle {
            position: relative;
            top: 222px;
            color: #bdbdbd;
        }

        .fileuploader #upload-label i{
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
        .preview-container{
          position: relative;
          bottom: 0px;
          width: 35%;
          margin: auto;
          top: 25px;
          visibility: hidden;
        }
        .preview-container #previews{
          max-height: 400px;
          overflow: auto; 
        }
        .preview-container #previews .zdrop-info{
          width: 88%;
          margin-right: 2%;
        }
        .preview-container #previews.collection{
          margin: 0;
          box-shadow: none;
        }

        .preview-container #previews.collection .collection-item {
            background-color: #e0e0e0;
        }

        .preview-container #previews.collection .actions a{
          width: 1.5em;
          height: 1.5em;
          line-height: 1;
        }
        .preview-container #previews.collection .actions a i{
          font-size: 1em;
          line-height: 1.6;
        }
        .preview-container #previews.collection .dz-error-message{
          font-size: 0.8em;
          margin-top: -12px;
          color: #F44336;
        }



        /*media querie*/

        @media only screen and (max-width: 601px){
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
            font-size:34px;
            color:red;
        }
        .dropify-wrapper{
            border: 2px dashed #E5E5E5;
        }
        .ppTabl {
            margin:0 auto;
            
        }
        .ppTabl>tbody>tr>td:first-child{
            text-align:right;
            padding-right:15px;
        }
        .ppTabl>tbody>tr>td {
            padding:4px 0;
            font-size:15px;
            text-align:left;
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
            display:none;
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
            display:none;
        }
        .bcShad.in , .popupSuc.in {
            display:block;
        }
        .bInfoIt{
            text-align: center;
            border-top: 1px solid #ccc;
            border-bottom: 1px solid #ccc;
            padding: 12px;
        }
        .bInfoIt p {
            margin:0;
        }
        .fontSmall>tbody>tr>td {
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
    </style>







    <%-- Date: 31-05-2017    Author: Kallol Samanta  [START] --%>
    <%-- Details: Billing/Shipping user control integration --%>

    <script>

        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= tstartdate.GetDate()) && (tstartdate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
            }
        }


        $(document).ready(function () {

            if ($("#Keyval_internalId").val() != "Add")
            {
                tstartdate.SetEnabled(false);
            }

            var schemaid = $('#ddl_numberingScheme').val();
            if (schemaid != null) {
                if (schemaid == '') {
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
                cdt_SaleInvoiceDue.SetDate(dt);

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
                    $('#<%=txt_PLQuoteNo.ClientID %>').val('Auto');
            document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = true;
            //ctxt_PLQuoteNo.SetText('Auto');
            //ctxt_PLQuoteNo.SetEnabled(false);
            //tstartdate.SetEnabled(false);
            //20-05-2019 Surojit
            if ($('#hdnPostingDateDisable').val() == "1") {
                tstartdate.SetEnabled(false);
            }
            else {
                tstartdate.SetEnabled(true);
            }
            //20-05-2019 Surojit

            if ($("#HdnBackDatedEntryPurchaseGRN").val() == "0") {
                tstartdate.SetEnabled(false);
            }
            else {
                tstartdate.SetEnabled(true);
            }



            tstartdate.Focus();
        }
        else if (NoSchemeType == '0') {
            document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = false;
            tstartdate.SetEnabled(true);
            //  ctxt_PLQuoteNo.GetInputElement().maxLength = quotelength;
            txt_PLQuoteNo.maxLength = quotelength;


            $('#<%=hdnSchemaLength.ClientID %>').val(quotelength);


            $('#<%=txt_PLQuoteNo.ClientID %>').val('');
            $('#<%=txt_PLQuoteNo.ClientID %>').focus();



        }
        else {
            //  ctxt_PLQuoteNo.SetText('');
            // ctxt_PLQuoteNo.SetEnabled(false);
            $('#<%=txt_PLQuoteNo.ClientID %>').val('');
            document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = true;
            //tstartdate.SetEnabled(false);
            //20-05-2019 Surojit
            if ($('#hdnPostingDateDisable').val() == "1") {
                tstartdate.SetEnabled(false);
            }
            else {
                tstartdate.SetEnabled(true);
            }
            //20-05-2019 Surojit

        }
        // clookup_Project.gridView.Refresh();
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
                        ctxt_Rate.SetValue("0.0");
                        ReBindGrid_Currency();
                    }
                }
            }
            else {
                ctxt_Rate.SetValue("0.0");
                ReBindGrid_Currency();
            }
            ctxt_Rate.SetEnabled(true);
        }



    });
        });


        function txtBillNo_TextChanged() {

            var SchemeVal = $('#<%=ddl_numberingScheme.ClientID %> option:selected').val();
            if (SchemeVal == "") {
                alert('Please Select Numbering Scheme');
                $('#<%=txt_PLQuoteNo.ClientID %>').val('');
        $('#<%=txt_PLQuoteNo.ClientID %>').focus();
    }
    else {
        var ReturnNo = $('#<%=txt_PLQuoteNo.ClientID %>').val();
        if (ReturnNo != '') {

            var SchemaLength = GetObjectID('hdnSchemaLength').value;
            var x = parseInt(SchemaLength);
            var y = parseInt(ReturnNo.length);

            if (y > x) {
                alert('Sale Return Manual length cannot be more than ' + x);
                //jAlert('Please enter unique Sales Order No');
                $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                $('#<%=txt_PLQuoteNo.ClientID %>').focus();

            }
            else {
                var CheckUniqueCode = false;
                $.ajax({
                    type: "POST",
                    url: "ReturnManual.aspx/CheckUniqueCode",
                    data: JSON.stringify({ ReturnNo: ReturnNo }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            alert('Please enter unique Sale Return Manual No');
                            //jAlert('Please enter unique Sales Order No');
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


                        //  var startDate = tstartdate.GetValueString();

                        var startDate = new Date();
                        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                        //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                        var key = $('#<%=hdnCustomerId.ClientID %>').val();
                //  var key = ctxtCustName.GetValue();
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
                //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                deleteTax('DeleteAllTax', "", "");
                ctxt_InvoiceDate.SetText('');




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
        var key = $('#<%=hdnCustomerId.ClientID %>').val();
        //  var key = ctxtCustName.GetValue();
        if (key != null && key != '') {
            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

        }
        // grid.PerformCallback('GridBlank');
        ccmbGstCstVat.PerformCallback();
        ccmbGstCstVatcharge.PerformCallback();
        //  ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        deleteTax('DeleteAllTax', "", "");
        page.SetActiveTabIndex(0);

    }
}



        function OnEndCallback(s, e) {
            //  debugger;
            // OnAddNewClick();
            var value = document.getElementById('hdnRefreshType').value;

            //Debjyoti Check grid needs to be refreshed or not
            if ($('#<%=HdUpdateMainGrid.ClientID %>').val() == 'True') {
        $('#<%=HdUpdateMainGrid.ClientID %>').val('False');
        grid.PerformCallback('DateChangeDisplay');
    }

    //   LoadingPanel.Hide();
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

    else if (grid.cpSaveSuccessOrFail == "checkAcurateTaxAmount") {
        LoadingPanel.Hide();
        OnAddNewClick();
        grid.cpSaveSuccessOrFail = null;
        jAlert('Check GST Calculated for Item ' + grid.cpProductName + ' at line ' + grid.cpSerialNo);
        grid.cpSaveSuccessOrFail = '';
        grid.cpSerialNo = '';
        grid.cpProductName = '';
    }

    else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData") {
        LoadingPanel.Hide();
        var SrlNo = grid.cpcheckMultiUOMData;
        var msg = "Please add Alt. Qty for SL No. " + SrlNo;
        grid.cpcheckMultiUOMData = null;
        jAlert(msg);
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "quantityTagged") {
        LoadingPanel.Hide();
        jAlert(' Quantity of selected products cannot be less than Ordered Quantity.');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "EmptyPlaceOfsupply") {
        LoadingPanel.Hide();
        jAlert('Please enter valid Place Of Supply.');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "AddLock") {
        LoadingPanel.Hide();
        jAlert('DATA is Freezed between ' + grid.cpAddLockStatus + '  for Add.');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "errorInsert") {
        LoadingPanel.Hide();
        jAlert('Please try again later.');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
        LoadingPanel.Hide();
        jAlert('Please Select Project.');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "nullAmount") {
        LoadingPanel.Hide();
        jAlert('total amount cant not be zero(0).');
        OnAddNewClick();
    }
        //else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
        //    LoadingPanel.Hide();
        //    jAlert('Please fill Quantity');
        //    OnAddNewClick();
        //}
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
        var SalesReturn_Msg = "Sale Return Manual No. " + SalesReturn_Number + " saved.";
        var EinvSalesReturn_Msg = "Sale Return Manual No. " + SalesReturn_Number + " generated.";
        $("#hdnRDECId").val(grid.cpRecetId);

        var IsEinvoice1 = grid.cpisEinvoice;
        if (IsEinvoice1 == 'true') {
            $.ajax({
                type: "POST",
                url: "SalesReturnList.aspx/Prc_EInvoiceChecking_details",
                data: "{'returnid':'" + $("#hdnRDECId").val() + "','Action':'ExemptedChecked'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == "Yes") {

                    }
                    else {
                        grid.cpisEinvoice = null;
                        SalesReturn_Msg = SalesReturn_Msg + "" + "This Invoice contains an Exempted Item.No Need to generate IRN."

                    }
                }
            });
        }
        var IsEinvoice = grid.cpisEinvoice;
                grid.cpisEinvoice=null;
                if (IsEinvoice == 'true') {
                    jAlert(EinvSalesReturn_Msg, 'Alert Dialog: [Sale Return Manual]', function (r) {
                        if (r == true) {

                            $("#lblInvNUmber").text(SalesReturn_Number);
                            $("#lblInvDate").text(tstartdate.GetText());
                            $("#lblCust").text(ctxtCustName.GetText());
                            $("#lblAmount").text(grid.cpToalAmountDEt);
                            LoadingPanel.Hide();
                            //cUploadConfirmation.Show();
                            $("#exampleModalSRM").modal("show");
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
                        jAlert('Error while generation IRN', 'Alert',function(){
                            window.location.assign("ReturnManualList.aspx");
                        });
                    }
                    else {
                        if (IRNgenerated == "Yes") {
                            $("#IrnNumber").text(grid.cpSucessIRNNumber);
                            $("#IrnlblInvNUmber").text(SalesReturn_Number);
                            $("#IrnlblInvDate").text(tstartdate.GetText());
                            $("#IrnlblCust").text(ctxtCustName.GetText());
                            $("#IrnlblAmount").text(grid.cpToalAmountDEt);
                            $(".bcShad, .popupSuc").addClass("in")
                        }
                        else

                            {
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

                                    jAlert(SalesReturn_Msg, 'Alert Dialog: [Sale Return Manual]', function (r) {
                                        LoadingPanel.Hide();
                                        //jAlert(Order_Msg);
                                        grid.cpQuotationNo = null;
                                        if (r == true) {
                                            window.location.assign("ReturnManualList.aspx");
                                        }
                                    });


                                }
                                else {

                                    window.location.assign("ReturnManualList.aspx");
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

                                    jAlert(SalesReturn_Msg, 'Alert Dialog: [Return Sale Return Manual]', function (r) {
                                        LoadingPanel.Hide();
                                        //jAlert(Order_Msg);
                                        grid.cpQuotationNo = null;
                                        if (r == true) {

                                            window.location.assign("ReturnManual.aspx?key=ADD");
                                        }
                                    });

                                }
                                else {

                                    window.location.assign("ReturnManual.aspx?key=ADD");
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
                                grid.batchEditApi.StartEdit(0, 1);
                                grid.StartEditRow(0);
                                $('#<%=hdnPageStatus.ClientID %>').val('');
                                var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                                var basedCurrency = LocalCurrency.split("~");
                                if ($("#ddl_Currency").val() == basedCurrency[0]) {
                                    ctxt_Rate.SetEnabled(false);
                                }


                                var taxtype = cddl_AmountAre.GetValue();

                                if (taxtype == '3') {
                                    grid.GetEditor('TaxAmount').SetEnabled(false);
                                }
                                else {
                                    grid.GetEditor("TaxAmount").SetEnabled(true);
                                }
                            }
                            else if (pageStatus == "Invoiceupdate") {
                                grid.StartEditRow(0);
                                $('#<%=hdnPageStatus.ClientID %>').val('');
                            }
                            else if (pageStatus == "delete") {
                                grid.StartEditRow(0);
                                OnAddNewClick();
                                $('#<%=hdnPageStatus.ClientID %>').val('');
                            }
                        }
                    }
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
        grid.GetEditor('Product').SetEnabled(true);
    }

    cProductsPopup.Hide();

}

        function DateCheck() {

            if (gridquotationLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {

                        page.SetActiveTabIndex(0);
                        $('.dxeErrorCellSys').addClass('abc');


                        //  var startDate = tstartdate.GetValueString();

                        var startDate = new Date();
                        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                        //  var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                        var key = $('#<%=hdnCustomerId.ClientID %>').val();
                    //  var key = ctxtCustName.GetValue();
                    if (key != null && key != '') {
                        cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                    }
                    grid.PerformCallback('GridBlank');
                    //cQuotationComponentPanel.PerformCallback('RemoveComponentGridOnSelection');

                    if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                        clearTransporter();
                    }
                    ccmbGstCstVat.PerformCallback();
                    ccmbGstCstVatcharge.PerformCallback();
                    // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax('DeleteAllTax', "", "");
                    ctxt_InvoiceDate.SetText('');
                    //  OnAddNewClick();
                }
            });
        }
        else {
            // var startDate = cPLSalesOrderDate.GetValueString();

            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
            page.SetActiveTabIndex(0);
            $('.dxeErrorCellSys').addClass('abc');
            cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

            //   var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
            var key = $('#<%=hdnCustomerId.ClientID %>').val();
            //  var key = ctxtCustName.GetValue();
            if (key != null && key != '') {
                cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

            }
            // grid.PerformCallback('GridBlank');
            ccmbGstCstVat.PerformCallback();
            ccmbGstCstVatcharge.PerformCallback();
            // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            deleteTax('DeleteAllTax', "", "");
            page.SetActiveTabIndex(0);
            //  OnAddNewClick();
        }
    }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="JS/SearchPopup.js"></script>
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
                                    <td>Selected Branch</td>
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
  
        <div id="divcross" runat="server" class="crossBtn"><a href="ReturnManualList.aspx"><i class="fa fa-times"></i></a></div>

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
                                        <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix col-md-12">
                                            <div class="col-md-2" id="divScheme" runat="server">
                                                <%--  <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme"></asp:Label>
                                                <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%"  onchange="CmbScheme_ValueChange()">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lbl_SaleInvoiceNo" runat="server" Text="Document No."></asp:Label>
                                                <%-- <dxe:ASPxLabel ID="lbl_SaleInvoiceNo" runat="server" Text="Return/Credit Note No">
                                                </dxe:ASPxLabel>--%>
                                                <%--<dxe:ASPxTextBox ID="txt_PLQuoteNo" runat="server" ClientInstanceName="ctxt_PLQuoteNo" TabIndex="2" Width="100%">
                                                    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                                </dxe:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_PLQuoteNo" runat="server"  Width="100%" onchange="txtBillNo_TextChanged()"></asp:TextBox>
                                                <span id="MandatorysQuoteno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                </span>
                                                <span id="duplicateQuoteno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Duplicate number">
                                                </span>
                                            </div>
                                            <div class="col-md-2">
                                                <%--  <dxe:ASPxLabel ID="lbl_SaleInvoiceDt" runat="server" Text="Date">
                                                </dxe:ASPxLabel>--%>

                                                <asp:Label ID="lbl_SaleInvoiceDt" runat="server" Text="Posting Date"></asp:Label>
                                                <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate"  Width="100%">
                                                    <ClientSideEvents DateChanged="function(s, e) {DateCheck();}" />
                                                    <ClientSideEvents GotFocus="function(s,e){tstartdate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}"></ClientSideEvents>
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lbl_Branch" runat="server" Text="Unit"></asp:Label>
                                                <%-- <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                                                </dxe:ASPxLabel>--%>
                                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%"  onchange="onBranchItems()">
                                                </asp:DropDownList>
                                            </div>

                                     

                                            <%-- kaushik 13-11-2017--%>
                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_Customer" runat="server" Text="Customer"></asp:Label>

                                                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" >

                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>

                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>




                                                <span id="MandatorysCustomer" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                                            </div>
                                            <%-- kaushik 13-11-2017--%>

                                            <div class="col-md-2">
                                                <%-- <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_ContactPerson" runat="server" Text="Contact Person"></asp:Label>
                                                <%--<dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" ClientSideEvents-EndCallback="cmbContactPersonEndCall" TabIndex="6" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">--%>
                                                <dxe:ASPxComboBox ID="cmbContactPerson" runat="server"  Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">


                                                    <ClientSideEvents GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />

                                                    <%-- <ClientSideEvents TextChanged="function(s, e) { GetContactPersonPhone(e)}" />--%>
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div style="clear: both"></div>
                                            <div class="col-md-2">
                                                <%-- <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="ASPxLabel3" runat="server" Text="Salesman/Agents"></asp:Label>
                                                <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" >
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <%--   <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                                </dxe:ASPxLabel>--%>

                                                <asp:Label ID="lbl_Refference" runat="server" Text="Ref. Invoice No."></asp:Label>
                                                <%--  <dxe:ASPxTextBox ID="txt_Refference" runat="server"  ClientInstanceName="ctxt_Refference"  TabIndex="8" Width="100%">
                                                </dxe:ASPxTextBox>--%>

                                                <asp:TextBox ID="txt_Refference" runat="server"  Width="100%"></asp:TextBox>
                                            </div>

                                            <div class="col-md-2" style="display: none">

                                                <asp:Label ID="lbl_invoice_No" runat="server" Text="Sale Invoice" Width="120px"></asp:Label>
                                                <%--  <dxe:ASPxLabel ID="lbl_invoice_No" runat="server" Text="Sale Invoice" Width="120px">
                                                </dxe:ASPxLabel>--%>
                                                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback" Enabled="false">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server"  ClientInstanceName="gridquotationLookup"
                                                                OnDataBinding="lookup_quotation_DataBinding"
                                                                KeyFieldName="ComponentID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", " Enabled="false">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="ComponentNumber" Visible="true" VisibleIndex="1" Caption="Number" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Date" Width="100" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Customer Name" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="branch" Visible="true" VisibleIndex="4" Caption="Branch" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="Reference" Visible="true" VisibleIndex="5" Caption="Reference" Width="80" Settings-AutoFilterCondition="Contains" />

                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="ASPxButtonmanual" runat="server" UseSubmitBehavior="false"></dxe:ASPxButton>
                                                                                        <%-- <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup"  UseSubmitBehavior="false" ></dxe:ASPxButton>--%>
                                                                                        <%-- <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup"  UseSubmitBehavior="false" />--%>
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



                                            </div>

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
                                                            Settings-ShowFooter="false" AutoGenerateColumns="False" OnHtmlRowCreated="aspxGridProduct_HtmlRowCreated"
                                                            OnRowInserting="Productgrid_RowInserting" OnRowUpdating="Productgrid_RowUpdating" OnRowDeleting="Productgrid_RowDeleting" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">

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
                                            <div class="col-md-2" style="display: none">
                                                <asp:Label ID="lbl_InvoiceNO" runat="server" Text="Sale Invoice Date"></asp:Label>

                                                <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                                        <PanelCollection>
                                                            <dxe:PanelContent runat="server">
                                                                <dxe:ASPxTextBox ID="txt_InvoiceDate" ClientInstanceName="ctxt_InvoiceDate" runat="server" Width="100%" ClientEnabled="false">
                                                                </dxe:ASPxTextBox>
                                                            </dxe:PanelContent>
                                                        </PanelCollection>
                                                    </dxe:ASPxCallbackPanel>
                                                </div>
                                                <%-- <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                                    <dxe:ASPxTextBox ID="txt_InvoiceDate" runat="server" Width="100%" ReadOnly="true">
                                                    </dxe:ASPxTextBox>
                                                </div>--%>
                                            </div>

                                            <div class="col-md-2" style="display:none">
                                                <asp:Label ID="lbl_DueDate" runat="server" Text="Due Date"></asp:Label>
                                                <%-- <dxe:ASPxLabel ID="lbl_DueDate" runat="server" Text="Due Date">
                                                </dxe:ASPxLabel>--%>
                                                <dxe:ASPxDateEdit ID="dt_SaleInvoiceDue" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_SaleInvoiceDue"  Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </div>

                                            <div class="col-md-1">
                                                <asp:Label ID="lbl_Currency" runat="server" Text="Currency"></asp:Label>
                                                <%-- <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                                </dxe:ASPxLabel>--%>
                                                <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" >
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:Label ID="lbl_Rate" runat="server" Text="Exch Rate"></asp:Label>
                                                <%--  <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Exch Rate">
                                                </dxe:ASPxLabel>--%>
                                                <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server"  Width="100%" Height="28px">
                                                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="ReBindGrid_Currency"  />
                                                   
                                                </dxe:ASPxTextBox>
                                            </div>

                                            <div class="col-md-2">
                                                <%--   <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_AmountAre" runat="server" Text="Amounts are"></asp:Label>
                                                <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre"  Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                                    <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" GotFocus="function(s,e){cddl_AmountAre.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="ASPxLabel4" runat="server" Text="Reason For Return"></asp:Label>
                                                <%--<dxe:ASPxMemo ID="txtReasonforChange" runat="server" Width="100%"  MaxLength="500" ClientInstanceName="ctxtReasonforChange" TabIndex="16">  </dxe:ASPxMemo>--%>
                                               <%-- onblur="return blurOut()--%>
                                                <asp:TextBox ID="txtReasonforChange" runat="server"  Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" ></asp:TextBox>
                                                <span id="MandatoryReasonforChange" style="display: none" class="validReasonclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>

                                            <div class="col-md-2">
                                                <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Place Of Supply[GST]">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                                <dxe:ASPxComboBox ID="ddlPosGstReturnManual" runat="server" ClientInstanceName="cddlPosGstReturnManual" Width="100%" ValueField="System.String">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateReturnManualPosGst(e)}"  LostFocus="function(s, e) { SetFocusAfterPlaceOfSupply(e)}"/>
                                                </dxe:ASPxComboBox>
                                            </div>

                                            <div class="col-md-2">
                                                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Transaction Category">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="drdTransCategory" runat="server" Width="100%" Enabled="false">
                                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="B2B" Value="B2B" />
                                                    <asp:ListItem Text="SEZWP" Value="SEZWP" />
                                                    <asp:ListItem Text="SEZWOP" Value="SEZWOP" />
                                                    <asp:ListItem Text="EXPWP" Value="EXPWP" />
                                                    <asp:ListItem Text="EXPWOP" Value="EXPWOP" />
                                                    <asp:ListItem Text="DEXP" Value="DEXP" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <%--<label id="lblProject" runat="server">Project</label>--%>
                                                <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                                                </dxe:ASPxLabel>
                                             <%--   <dxe:ASPxCallbackPanel runat="server" ID="ProjectCallBack" EnableCallbackCompression="true" ClientInstanceName="cProjectCallBack" OnCallback="ProjectCallBackPanel_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">--%>
                                                            <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSalesReturnManual"
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
                                                                <ClientSideEvents GotFocus="clookup_Project_GotFocus" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />


                                                            </dxe:ASPxGridLookup>
                                                      <%--  </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <%--  <ClientSideEvents EndCallback="CustomerCallBackPanelEndCallBack" />--%>
                                              <%--  </dxe:ASPxCallbackPanel>--%>
                                                 <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesReturnManual" runat="server" OnSelecting="EntityServerModeDataSalesReturnManual_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                            </div>
                                            <div class="col-md-4">
                                                <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-2 hide">

                                                <asp:Label ID="lblVatGstCst" runat="server" Text="Select GST"></asp:Label>
                                                <%-- <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                                </dxe:ASPxLabel>--%>
                                                <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" OnCallback="ddl_VatGstCst_Callback"  Width="100%">
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
                                                OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
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
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataButtonEditColumn FieldName="Product" Caption="Product" VisibleIndex="2" Width="13%" ReadOnly="true">
                                                        <PropertiesButtonEdit Width="100%">
                                                            <ClientSideEvents ButtonClick="ProductDisButnClick" KeyDown="ProductDisKeyDown" GotFocus="PsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product Returned" VisibleIndex="3" Width="13%" ReadOnly="true">
                                                        <PropertiesButtonEdit Width="100%">
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>



                                                     <dxe:GridViewCommandColumn VisibleIndex="4" Caption="Addl. Desc." Width="6%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc"  Image-Url="/assests/images/more.png" Image-ToolTip="Addl. Description">
                                                                <Image ToolTip="Addl. Description" Url="/assests/images/more.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        
                                                    </dxe:GridViewCommandColumn>


                                             <%--       <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc" Image-Url="/assests/images/more.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" Text="New" ForeColor="White">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>
                                                    </dxe:GridViewCommandColumn>--%>





                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="5" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityGotFocus" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="6" ReadOnly="true" Width="6%">
                                                        <PropertiesTextEdit>
                                                             <ClientSideEvents LostFocus="UOMLostFocus"/>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewCommandColumn VisibleIndex="7" Caption="Multi UOM" Width="6%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <%--Mantis Issue 24831--%>
                                                    <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="Order_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="8" Width="5%" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                           <%-- <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />--%>
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="Order_AltUOM" Caption="Multi Unit" VisibleIndex="9" ReadOnly="true" Width="6%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--End of Mantis Issue 24831--%>
                                                   
                                                    <%--Mantis Issue 25377 [ ReadOnly="true" added ]--%>
                                                    <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Price" VisibleIndex="10" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents TextChanged="SalePriceTextChange" LostFocus="spLostFocus" GotFocus="SalePriceGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="CloseRate" Caption="Stk In Rate" VisibleIndex="11" Width="6%" HeaderStyle-HorizontalAlign="Right">

                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>

                                                        <PropertiesTextEdit>
                                                           <%--mantis:0024620  <ClientSideEvents LostFocus="CloseRateLostFocus"/>--%>
                                                            <%--<ClientSideEvents LostFocus="CloseRateTextChange"  />--%>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataSpinEditColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="12" Width="5%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" Style-HorizontalAlign="Right">
                                                            <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                            <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountGotChange" />
                                                        </PropertiesSpinEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataSpinEditColumn>

                                                    <%--Mantis Issue 25377 [ ReadOnly="true" added ]--%>
                                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="13" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="AmtTextChange" GotFocus="AmtGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="14" Width="8%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesButtonEdit Width="100%">
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <%-- LostFocus="Taxlostfocus"--%>
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    
                                                    <%--Mantis Issue 25377 [ ReadOnly="true" added]--%>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="15" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <ClientSideEvents GotFocus="TotalAmountgotfocus" />
                                                            <%--Mantis Issue 24831--%>
                                                            <%--<MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>
                                                            <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <%--End of Mantis Issue 24831--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="16" ReadOnly="True" Width="0" CellStyle-CssClass="hide">
                                                        <CellStyle Wrap="True"></CellStyle>

                                                    </dxe:GridViewDataTextColumn>


                                                     <dxe:GridViewDataTextColumn FieldName="StockQuantity" Caption="Stock Qty" VisibleIndex="17" Visible="false">
                                                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="StockUOM" Caption="Stock UOM" VisibleIndex="18" ReadOnly="true" Visible="false">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Invoice ID" VisibleIndex="19" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="20" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="21" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="22" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="23" FieldName="ComponentNumber" ReadOnly="true" Caption="Number" Width="0">
                                                    </dxe:GridViewDataTextColumn>

                                                  <dxe:GridViewDataTextColumn FieldName="CloseRateFlag" Caption="hidden Field Id" VisibleIndex="24" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                        <dxe:GridViewDataTextColumn FieldName="DetailsId" Caption="DetailsId" VisibleIndex="25" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                     <dxe:GridViewDataTextColumn FieldName="ProductDisID" Caption="ProductDisID" VisibleIndex="26" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                
                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="ProductID" VisibleIndex="27" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>



                                                    

                                                      <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="14" Width="9%" ReadOnly="false">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Left">

                                                            <Style HorizontalAlign="Left">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="15" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                   
                                                  
                                                </Columns>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                                <SettingsBehavior ColumnResizeMode="Disabled" />
                                            </dxe:ASPxGridView>
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
                                        <div class="" id="divSubmitButton">
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <span id="tdSaveButtonNew" runat="server">
                                                <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </span>
                                            <span id="tdSaveButton" runat="server">
                                                <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                                </dxe:ASPxButton>
                                                <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                                </dxe:ASPxButton>
                                            </span>

                                            <b><span id="tagged" runat="server" style="display: none; color: red">This Manual Sales Return is tagged in other modules. Cannot Modify data</span></b>
                                            <asp:HiddenField ID="hfControlData" runat="server" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="SRM" />

                                        </div>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                            <%--test generel--%>
                        </dxe:TabPage>

                        <dxe:TabPage Name="Billing/Shipping" Text="Our Billing/Shipping">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">

                                    <%-- Date: 31-05-2017    Author: Kallol Samanta  [START] --%>
                                    <%-- Details: Billing/Shipping user control integration --%>
                                    <ucBS:Purchase_BillingShipping runat="server" ID="Purchase_BillingShipping" />
                                    <%-- Date: 31-05-2017    Author: Kallol Samanta  [END] --%>
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

            <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />

            <%-- <asp:SqlDataSource ID="CountrySelect" runat="server"
                SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>--%>
          <%--  <asp:SqlDataSource ID="StateSelect" runat="server"
                SelectCommand="SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">

                <SelectParameters>
                    <asp:Parameter Name="State" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>--%>
         <%--   <asp:SqlDataSource ID="SelectCity" runat="server"
                SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
                <SelectParameters>
                    <asp:Parameter Name="City" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>--%>

          <%--  <asp:SqlDataSource ID="SelectArea" runat="server"
                SelectCommand="SELECT area_id, area_name from tbl_master_area where (city_id = @Area) ORDER BY area_name">
                <SelectParameters>
                    <asp:Parameter Name="Area" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>--%>
        <%--    <asp:SqlDataSource ID="SelectPin" runat="server"
                SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City order by pin_code">
                <SelectParameters>
                    <asp:Parameter Name="City" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>--%>
          <%--  <asp:SqlDataSource ID="sqltaxDataSource" runat="server"
                SelectCommand="select Taxes_ID,Taxes_Name from dbo.Master_Taxes"></asp:SqlDataSource>--%>

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
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <%-- <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="PercentageTextChange" />
                                                    <ClientSideEvents />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <%--<MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />--%>
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
                                        <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="false">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" UseSubmitBehavior="false">
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
                <asp:HiddenField ID="hdnPOSBillingStateId" runat="server" />
                <asp:HiddenField ID="hdnPOSBillingStateCode" runat="server" />
                <asp:HiddenField ID="hdnPOSShippingStateId" runat="server" />
                <asp:HiddenField ID="hdnPOSShippingStateCode" runat="server" />
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


                <%-- Surojit 14-03-2019 --%>
                <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
                <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
                <%-- Surojit 14-03-2019 --%>

                <%-- Surojit 20-05-2019 --%>
                <asp:HiddenField runat="server" ID="hdnPostingDateDisable" />
                <asp:HiddenField runat="server" ID="hdnProjectSelectInEntryModule" />
                <%-- Surojit 20-05-2019 --%>

                <%--Mantis Issue 24831--%>
                <asp:HiddenField ID="hdVisiableIndex" runat="server" />
                <%--End of Mantis Issue 24831--%>
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
                                                <td>Selected Branch</td>
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
                                            <dxe:ASPxButton ID="ASPxButton8" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary pull-left" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveWarehouse();}" />
                                            </dxe:ASPxButton>

                                            <dxe:ASPxButton ID="ASPxButton9" ClientInstanceName="cbtnrefreshWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Clear Entries" CssClass="btn btn-primary pull-left" UseSubmitBehavior="False">
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
                                    <dxe:ASPxButton ID="ASPxButton10" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="S&#818;ave & Exit" AccessKey="S" CssClass="btn btn-primary" UseSubmitBehavior="False">
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

            <asp:HiddenField ID="hdnCustomerStateCodeId" runat="server" />
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

                            </Columns>
                            <ClientSideEvents ValueChanged="ProductSelected" KeyDown="ProductlookUpKeyDown" GotFocus="function(s,e){cproductLookUp.ShowDropDown();}" />

                        </dxe:ASPxComboBox>
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

                            </Columns>
                            <ClientSideEvents ValueChanged="ProductDisSelected" KeyDown="ProductlookUpdisKeyDown" GotFocus="function(s,e){cproductDisLookUp.ShowDropDown();}" />

                        </dxe:ASPxComboBox>
                        <%-- <dxe:ASPxGridLookup ID="productDisLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductDisLookUp"
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

            <%--<asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="prc_CRMSalesReturn_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                    <asp:SessionParameter Name="campany_Id" SessionField="LastCompanySRM" Type="String" />
                    <asp:SessionParameter Type="String" Name="FinYear" SessionField="LastFinYearSRM" />
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
                                                    <%--<MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />--%>
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
                                        <ClientSideEvents EndCallback="cgridTax_EndCallBack" RowClick="GetTaxVisibleIndex" />

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
                                       <%-- <asp:Button ID="Button1" runat="server" Text="O&#818;K" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" UseSubmitBehavior="false" />--%>
                                        <asp:Button ID="Button1" runat="server" Text="OK&#818;" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" UseSubmitBehavior="false" />
                                        <asp:Button ID="Button2" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" UseSubmitBehavior="false"/>
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
             var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SR&&KeyVal_InternalID=' + keyVal;
             popup.SetContentUrl(url);
             popup.Show();
         }
         return true;
     }

     function acbpCrpUdfEndCall(s, e) {
         //    LoadingPanel.Hide();
         // 
         if (cacbpCrpUdf.cpUDF) {
             if (cacbpCrpUdf.cpUDF == "true" && cacbpCrpUdf.cpTransport == "true") {
                 grid.UpdateEdit();
                 cacbpCrpUdf.cpUDF = null;
                 cacbpCrpUdf.cpTransport = null;
             }
             else if (cacbpCrpUdf.cpUDF == "false") {
                 LoadingPanel.Hide();
                 jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
                 cacbpCrpUdf.cpUDF = null;
                 cacbpCrpUdf.cpTransport = null;
             }
             else {
                 LoadingPanel.Hide();
                 jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                 cacbpCrpUdf.cpUDF = null;
                 cacbpCrpUdf.cpTransport = null;
             }
         }
     }

    </script>


    <script>

        function CustomerButnClick(s, e) {
            $('#CustModel').modal('show');
            setTimeout(function () { $("#txtCustSearch").focus(); }, 500);

        }
        function CustomerKeyDown(s, e) {
            if (e.code == "Enter" || e.code == "NumpadEnter" || e.htmlEvent.key == "Enter") {
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
                //  ctxtCustName.SetValue(Id);
                ctxtCustName.SetText(Name);

                GetObjectID('hdnCustomerId').value = Id;
                GetObjectID('hdfLookupCustomer').value = Id;


                //Written By chinmoy for place of supply 
                var OtherDetail = {};
                OtherDetail.CustomerID = Id;
                $.ajax({
                    type: "POST",
                    url: "ReturnManual.aspx/GetCustomerStateCodeDetails",
                    data: JSON.stringify(OtherDetail),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        StateCodeList = msg.d;
                        if (StateCodeList[0].TransactionType != "") {
                            $("#drdTransCategory").val(StateCodeList[0].TransactionType);
                        }
                        //  $('#hdnPOSStateId').val(StateCodeList[0].id);
                        //$('#hdnPOSStateCode').val(StateCodeList[0].StateCode);

                    },
                    error: function (msg) {
                        jAlert('Please try again later');
                    }
                });






                $('.crossBtn').hide();
                $('#CustModel').modal('hide');
            }
            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
            var branchid = $('#ddl_Branch').val();
            var key = $('#<%=hdnCustomerId.ClientID %>').val();
            //var key = GetObjectID('hdnCustomerId').value;
            //  var key = ctxtCustName.GetValue();
            // alert(key);
            if (key != null && key != '') {

                ctxt_InvoiceDate.SetText('');




                var CheckBillingShippingExist = false;
                $.ajax({
                    type: "POST",
                    url: "ReturnManual.aspx/CheckBillingShippingExist",
                    data: JSON.stringify({ CustomerID: key }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        CheckBillingShippingExist = msg.d;
                        if (CheckBillingShippingExist == false) {
                            jAlert('Please enter Customer default billing /shipping address');

                            //jAlert('Please enter unique Sales Order No');
                            // $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                            //   $('#<%=txt_PLQuoteNo.ClientID %>').focus();

                            //  $('#<%=txtCustName.ClientID %>').val('')
                            ctxtCustName.Focus();

                            return false;
                        }
                        else {




                            $('.dxeErrorCellSys').addClass('abc');
                            GetObjectID('hdnCustomerId').value = key;
                            GetObjectID('hdnAddressDtl').value = '0';

                            //###### Added By : Samrat Roy ##########
                            LoadingPanel.Show();
                            // LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SRM');
                            SetPurchaseBillingShippingAddress($('#ddl_Branch').val());

                            setTimeout(function () {
                                GetPurchaseForGstValue();

                            }, 500);

                            LoadingPanel.Hide();
                            page.tabs[0].SetEnabled(true);
                            page.tabs[1].SetEnabled(true);
                            cContactPerson.Focus();
                            //  $('#MandatorysQuoteno').attr('style', 'display:none');
                        }
                    }

                });


                //  LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SRM');   //For Return Manual => SRM 



                GetObjectID('hdnCustomerId').value = key;

                //  page.tabs[0].SetEnabled(true);
                // page.tabs[1].SetEnabled(true);


            }

            GlobalBillingShipping();
            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                clookup_Project.gridView.Refresh();
            }
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
    <%-- TaxDetails HiddenField Field --%>
    <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
    <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
    <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
    <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
    <%-- TaxDetails HiddenField Field --%>

    <%--   <asp:SqlDataSource ID="CustomerDataSource" runat="server"  />--%>


        <!-- Modal -->
<div class="modal fade pmsModal w40" id="exampleModalSRM" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">Upload Confirmation</h5>
      
      </div>
      <div class="modal-body poppins">
        <div class="text-center">
            <img src="../../../assests/images/invoiceII.png" style="width: 70px;margin-bottom: 15px;" />
        </div>
        <div>
            <%--<input type="file" class="dropify" data-height="80" />--%>
        </div>
          <div class="text-center pTop10">
              <table class="ppTabl ">
                  <tr>
                      <td>Return Number :</td>
                      <td><b id="lblInvNUmber"></b></td>
                  </tr>
                  <tr>
                      <td>Date : </td>
                      <td><b id="lblInvDate"></b> </td>
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
          <button class="btn btn-danger" type="button"  data-dismiss="modal" onclick="UploadGridbindCancel()">Later</button>
      </div>
    </div>
  </div>
</div>


    
 <div class="bcShad "></div> 
 <div class="popupSuc ">
     <div style="background: #467bbd;
    color: #fff;
    text-align: center;
    padding: 7px;font-size: 14px;">Important Message</div>
     <div class="text-center">
         <span class="cnIcon"><i class="fa fa-check" aria-hidden="true"></i></span>
     </div>
     <div class="bInfoIt">
         <p style="font-size: 15px;color: #e68710;margin-bottom: 10px;">Document has been uploaded successfully to GSTN server</p>
         <p style="font-size: 14px;color: blue;">IRN :<a id="IrnNumber"></a></p>
     </div>
     <table class="ppTabl fontSmall">
        <tr>
            <td>Return Number :</td>
            <td><b id="IrnlblInvNUmber"></b></td>
        </tr>
        <tr>
            <td>Date : </td>
            <td><b id="IrnlblInvDate"></b> </td>
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
     <div style="text-align: center;padding: 14px;background: antiquewhite;">
         <button class="okbtn btn btn-primary" type="button" onclick="IrnGrid()">OK</button>
     </div>
 </div>


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
    <dxe:ASPxCallbackPanel runat="server" ClientSideEvents-EndCallback="CustomerStateEndcall" ID="CustStateCallBackPanel" ClientInstanceName="cCustStateCallBackPanel" OnCallback="CustStateCallBackPanel_Callback">
    </dxe:ASPxCallbackPanel>


    <%--for MultiUOM start--%>

    <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="1000px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeMultiUOM(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">



                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">

                        <table class="eqTble">
                            <tr>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--Mantis Issue 24831--%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" readonly="true" class="allownumericwithdecimal" />--%>
                                                <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onchange="CalcBaseRate()" />
                                                <%--End of Mantis Issue 24831--%>
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
                                <%--Mantis Issue 24831--%>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Mantis Issue 24831--%>
                                <td>
                                    <span style="font-size: 22px; padding-top: 15px; display: inline-block;">=</span>
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
                                            <%-- <input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/>--%>
                                            <dxe:ASPxTextBox ID="AltUOMQuantity" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <%--Mantis Issue 24831--%>
                                                 <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Mantis Issue 24831--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24831--%>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            
                                        </div>
                                        <div>
                                            <label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>

                                    
                                </td>
                                <%--End of Mantis Issue 24831--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveMultiUOM();}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="grid_MultiUOM" runat="server" KeyFieldName="AltUomId;AltQuantity" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgrid_MultiUOM" OnCustomCallback="MultiUOM_CustomCallback" OnDataBinding="MultiUOM_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <%--Mantis Issue 24831--%>
                                <dxe:GridViewDataTextColumn Caption="MultiUOMSR No" 
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" Width="0px">
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24831--%>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <%--Mantis Issue 24831--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24831--%>

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

                                <%--Mantis Issue 24831--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24831--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>','<%#Eval("DetailsId") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>
                                        <%--Mantis Issue 24831--%>
                                        <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                        <%--End of Mantis Issue 24831--%>
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
                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
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
    
    <%--   multiUOM end--%>
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />

    
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

 <asp:HiddenField ID="hdnLockFromDate" runat="server" />
<asp:HiddenField ID="hdnLockToDate" runat="server" />
 <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
    <asp:HiddenField ID="hdnRDECId" runat="server" />
     <asp:HiddenField ID="HiddenField1" runat="server" />
     <asp:HiddenField ID="HdnBackDatedEntryPurchaseGRN" runat="server" />
</asp:Content>
