<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                12-04-2023        2.0.37           Pallab              Transactions pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerReturn.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" Inherits="ERP.OMS.Management.Activities.CustomerReturn" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>

<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    
     <%--Use for set focus on UOM after press ok on UOM--%> 
    <script>
        function OnEndCallback(s, e) {
            //debugger;
            // OnAddNewClick();
            var value = document.getElementById('hdnRefreshType').value;

            //Debjyoti Check grid needs to be refreshed or not
            if ($('#<%=HdUpdateMainGrid.ClientID %>').val() == 'True') {
        $('#<%=HdUpdateMainGrid.ClientID %>').val('False');
        grid.PerformCallback('DateChangeDisplay');
    }
            //   LoadingPanel.Hide();


    if (grid.cpRemoveProductInvoice) {
        if (grid.cpRemoveProductInvoice == "valid") {
            OnAddNewClick();
            grid.cpRemoveProductInvoice = null;
        }
    }
    else { grid.GetEditor('Product').SetEnabled(true); }  //when invoice is not select
    if (grid.cpSaveSuccessOrFail == "outrange") {
        LoadingPanel.Hide();
        jAlert('Can Not Add More Sales Challan Number as Sales Invoice Scheme Exausted.<br />Update The Scheme and Try Again');
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
        jAlert('Can not Duplicate Product in the Customer Return List.');
        OnAddNewClick();
    }
    else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
        LoadingPanel.Hide();
        var SrlNo = grid.cpProductSrlIDCheck;
        var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
        jAlert(msg);
        OnAddNewClick();
    }
    else {
        var SalesReturn_Number = grid.cpQuotationNo;
        var SalesReturn_Msg = "Customer Return No. " + SalesReturn_Number + " saved.";

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

                    jAlert(SalesReturn_Msg, 'Alert Dialog: [CustomerReturn]', function (r) {
                        LoadingPanel.Hide();
                        //jAlert(Order_Msg);
                        grid.cpQuotationNo = null;
                        if (r == true) {
                            window.location.assign("CustomerReturnList.aspx");
                        }
                    });


                }
                else {

                    window.location.assign("CustomerReturnList.aspx");
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

                    jAlert(SalesReturn_Msg, 'Alert Dialog: [CustomerReturn]', function (r) {
                        LoadingPanel.Hide();
                        //jAlert(Order_Msg);
                        grid.cpQuotationNo = null;
                        if (r == true) {

                            window.location.assign("CustomerReturn.aspx?key=ADD");
                        }
                    });

                }
                else {

                    window.location.assign("CustomerReturn.aspx?key=ADD");
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
            else {
                grid.StartEditRow(0);

            }
}
}

    if (grid.cpGridBlank == "1") {
        grid.GetEditor('Product').SetEnabled(true);
        OnAddNewClick();
        gridquotationLookup.gridView.Refresh();
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
                        //  var key = cCustomerComboBox.GetValue();
                        var key = $('#<%=hdnCustomerId.ClientID %>').val();
                // var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

                if (key != null && key != '') {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                }
                grid.PerformCallback('GridBlank');
                // cQuotationComponentPanel.PerformCallback('RemoveComponentGridOnSelection');

                if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                    clearTransporter();
                }
                // ctxt_InvoiceDate.SetText('');
                $('#<%=txt_InvoiceDate.ClientID %>').val('');
                ccmbGstCstVat.PerformCallback();
                ccmbGstCstVatcharge.PerformCallback();
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                // grid.AddNewRow();
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
        // var key = cCustomerComboBox.GetValue();
        var key = $('#<%=hdnCustomerId.ClientID %>').val();
        //   var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

        if (key != null && key != '') {
            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

        }
        // grid.PerformCallback('GridBlank');
        ccmbGstCstVat.PerformCallback();
        ccmbGstCstVatcharge.PerformCallback();
        ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        page.SetActiveTabIndex(0);
        // OnAddNewClick();
        //  grid.AddNewRow();
    }
        }

        function SetCustomer(Id, Name) {



            if (Id) {
                $('#CustModel').modal('hide');
                ctxtCustName.SetText(Name);
                //ctxtCustName.SetValue(Id);

                GetObjectID('hdnCustomerId').value = Id;
                GetObjectID('hdfLookupCustomer').value = Id;

                //  GetObjectID('hdnAddressDtl').value = '0';

                // page.SetActiveTabIndex(1);
                //  loadAddressbyCustomerID(Id);
                //  $('.dxeErrorCellSys').addClass('abc');
                $('.crossBtn').hide();
                //   page.GetTabByName('General').SetEnabled(false);
                $('#CustModel').modal('hide');
            }


            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');

            if (gridquotationLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        gridquotationLookup.gridView.UnselectRows();
                        //   var key = ctxtCustName.GetValue();

                        var key = Id;

                        if (key != null && key != '') {
                            $('#<%=txt_InvoiceDate.ClientID %>').val('');
                    $('.dxeErrorCellSys').addClass('abc');

                    var startDate = new Date();
                    startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                    if (key != null && key != '') {
                        cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                    }

                    if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                            clearTransporter();
                        }
                    }

                    $('#<%=txt_InvoiceDate.ClientID %>').val('');

                    LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SR');
                    GetObjectID('hdnCustomerId').value = key;
                    page.tabs[0].SetEnabled(true);
                    page.tabs[1].SetEnabled(true);

                    //###### END : Samrat Roy : END ########## 

                }

            } else {

                ctxtCustName.SetValue($('#<%=hdnCustomerId.ClientID %>').val());
                var Customerid = $('#<%=hdnCustomerId.ClientID %>').val();
                ctxtCustName.PerformCallback(Customerid);
                // gridLookup.SetValue($('#<%=hdnCustomerId.ClientID %>').val());
            }

        });
    }
    else {
        //  var key = ctxtCustName.GetValue();
        var key = $('#<%=hdnCustomerId.ClientID %>').val();
        if (key != null && key != '') {
            $('#<%=txt_InvoiceDate.ClientID %>').val('');

            //###### Added By : Samrat Roy ##########

            LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SR');
            GetObjectID('hdnCustomerId').value = key;
            page.tabs[0].SetEnabled(true);
            page.tabs[1].SetEnabled(true);

            //###### END : Samrat Roy : END ########## 
            $('.dxeErrorCellSys').addClass('abc');
            GetObjectID('hdnCustomerId').value = key;
            GetObjectID('hdnAddressDtl').value = '0';
        }

    }
    cContactPerson.Focus();

        }



        function PerformCallToGridBind() {
            grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
            //  cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
            $('#hdnPageStatus').val('Invoiceupdate');
            cProductsPopup.Hide();
            //#### added by Samrat Roy for Transporter Control #############
            //debugger;
            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
        var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
        callTransporterControl(quote_Id[0], 'SC');
    }

    return false;
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

            var key = $('#<%=hdnCustomerId.ClientID %>').val();
            // var key = cCustomerComboBox.GetValue();
    if (accountingDataMin != null || accountingDataplus != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

            if (r == true) {

                page.SetActiveTabIndex(0);
                $('.dxeErrorCellSys').addClass('abc');


                //  var startDate = tstartdate.GetValueString();

                var startDate = new Date();
                startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                // var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

                if (key != null && key != '') {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                }
                grid.PerformCallback('GridBlank');
                //   cQuotationComponentPanel.PerformCallback('RemoveComponentGridOnSelection');

                if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                    clearTransporter();
                }

                ccmbGstCstVat.PerformCallback();
                ccmbGstCstVatcharge.PerformCallback();
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                //  ctxt_InvoiceDate.SetText('');
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

        //   var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());

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
        function GetContactPerson(e) {

            // var key = gridLookup.GetValue();
            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
            var branchid = $('#ddl_Branch').val();
            if (gridquotationLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        gridquotationLookup.gridView.UnselectRows();
                        //   var key = cCustomerComboBox.GetValue();
                        var key = $('#<%=hdnCustomerId.ClientID %>').val();
                if (key != null && key != '') {
                    LoadCustomerAddress(key, $('#ddl_Branch').val(), 'CR');
                    GetObjectID('hdnCustomerId').value = key;
                    page.tabs[0].SetEnabled(true);
                    page.tabs[1].SetEnabled(true);
                    $('.dxeErrorCellSys').addClass('abc');
                    var startDate = new Date();
                    startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                    if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                        clearTransporter();
                    }
                    $('#<%=txt_InvoiceDate.ClientID %>').val('');
                    //  });

                    //   document.getElementById('popup_ok').focus();
                }

            } else {
                // gridLookup.SetValue($('#<%=hdnCustomerId.ClientID %>').val());

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
 
            LoadCustomerAddress(key, $('#ddl_Branch').val(), 'CR');
            GetObjectID('hdnCustomerId').value = key;
            page.tabs[0].SetEnabled(true);
            page.tabs[1].SetEnabled(true);
        
            $('.dxeErrorCellSys').addClass('abc');
          
            GetObjectID('hdnAddressDtl').value = '0';
        }

    }

        }

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
    </script>    
    <script src="JS/CustomerReturn.js"></script>
    <link href="CSS/CustomerReturn.css" rel="stylesheet" />
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
        <div id="divcross" runat="server" class="crossBtn"><a href="CustomerReturnList.aspx"><i class="fa fa-times"></i></a></div>

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
                                        <div style=" padding: 8px 0; margin-bottom: 0px; border-radius: 4px; " class="clearfix col-md-12">
                                            <%--Rev 1.0 : "simple-select" class add--%>
                                            <div class="col-md-2 simple-select" id="divScheme" runat="server">
                                                <%--<dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
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
                                                <asp:Label ID="lbl_SaleInvoiceDt" runat="server" Text="Posting Date"></asp:Label>
                                                <%--<dxe:ASPxLabel ID="lbl_SaleInvoiceDt" runat="server" Text="Date">
                                                </dxe:ASPxLabel>--%>
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
                                                <asp:Label ID="lbl_Branch" runat="server" Text="Unit"></asp:Label>
                                                <%--  <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                                                </dxe:ASPxLabel>--%>
                                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="4" onchange="onBranchItems()">
                                                </asp:DropDownList>
                                            </div>

                                            <%--  <div class="col-md-2">
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" TabIndex="5" ClientInstanceName="gridLookup"
                                                    KeyFieldName="cnt_internalid" TextFormatString="{1}" AutoGenerateColumns="False" Width="100%" DataSourceID="dsCustomer">
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
                                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior="False" />
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
                                                <span id="MandatorysCustomer" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>--%>

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
                                            <div class="col-md-2">
                                                <%-- <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_ContactPerson" runat="server" Text="Contact Person"></asp:Label>
                                                <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" ClientSideEvents-EndCallback="cmbContactPersonEndCall" TabIndex="6" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">

                                                    <ClientSideEvents GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />
                                                    <ClientSideEvents TextChanged="function(s, e) { GetContactPersonPhone(e)}" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div style="clear: both"></div>
                                            <%--Rev 1.0 : "simple-select" class add--%>
                                            <div class="col-md-2 simple-select">
                                                <asp:Label ID="ASPxLabel3" runat="server" Text="Salesman/Agents"></asp:Label>
                                                <%--<dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                                </dxe:ASPxLabel>--%>
                                                <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" TabIndex="7">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <%--  <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                                </dxe:ASPxLabel>--%>

                                                <asp:Label ID="lbl_Refference" runat="server" Text="Reference"></asp:Label>
                                                <%-- <dxe:ASPxTextBox ID="txt_Refference" runat="server" ClientInstanceName="ctxt_Refference" TabIndex="8" Width="100%">
                                                </dxe:ASPxTextBox>--%>


                                                <asp:TextBox ID="txt_Refference" runat="server" TabIndex="8" Width="100%"></asp:TextBox>
                                            </div>

                                            <div class="col-md-2">

                                                <asp:RadioButtonList ID="rdl_SalesInvoice" runat="server" RepeatDirection="Horizontal" TabIndex="9" onchange="return selectValue();" Width="120px">
                                                    <asp:ListItem Text="Sale Challan" Value="SIN"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <%-- <asp:Label ID="lbl_invoice_No" runat="server" Text="Sale Challan" Width="120px"></asp:Label>--%>
                                                <%--  <dxe:ASPxLabel ID="lbl_invoice_No" runat="server" Text="Sale Challan" Width="120px">
                                                </dxe:ASPxLabel>--%>
                                                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" TabIndex="10" ClientInstanceName="gridquotationLookup"
                                                                OnDataBinding="lookup_quotation_DataBinding"
                                                                KeyFieldName="ComponentID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
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

                                                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="false" />
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
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quotation_Num" Width="90" ReadOnly="true" Caption="Sales Challan No">
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
                                                <%--  <dxe:ASPxLabel ID="lbl_InvoiceNO" runat="server" Text="Sale Challan Date">
                                                </dxe:ASPxLabel>--%>
                                                <asp:Label ID="lbl_InvoiceNO" runat="server" Text="Sale Challan Date"></asp:Label>
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
                                                <asp:Label ID="lbl_DueDate" runat="server" Text="Due Date"></asp:Label>
                                                <%-- <dxe:ASPxLabel ID="lbl_DueDate" runat="server" Text="Due Date">
                                                </dxe:ASPxLabel>--%>
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
                                                <%-- <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Exch Rate">
                                                </dxe:ASPxLabel>--%>
                                                <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server" TabIndex="14" Width="100%" Height="28px">
                                                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="ReBindGrid_Currency" GotFocus="function(s,e){ctxt_Rate.ShowDropDown();}" />
                                                </dxe:ASPxTextBox>
                                            </div>

                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_AmountAre" runat="server" Text="Amounts are"></asp:Label>
                                                <%-- <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>--%>
                                                <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" TabIndex="15" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                                    <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" GotFocus="function(s,e){cddl_AmountAre.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                            </div>

                                            <div class="clear"></div>
                                            <div class="col-md-6">
                                                <%-- <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Reason For Return">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxMemo ID="txtReasonforChange" runat="server" Width="100%" MaxLength="500" ClientInstanceName="ctxtReasonforChange" TabIndex="16"></dxe:ASPxMemo>--%>
                                                <asp:Label ID="ASPxLabel4" runat="server" Text="Reason For Return"></asp:Label>
                                                <asp:TextBox ID="txtReasonforChange" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" onblur="return blurOut()"></asp:TextBox>


                                                <span id="MandatoryReasonforChange" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
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
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
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

                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="4" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityGotFocus" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--  <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="4" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                           <MaskSettings Mask="<0..999999999>" AllowMouseWheel="false" /> 
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="ProductsGotFocusFromID" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM(Sale)" VisibleIndex="5" ReadOnly="true" Width="6%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Caption="Warehouse"--%>
                                                    <dxe:GridViewCommandColumn VisibleIndex="6" Caption="Stk Details" Width="6%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="StockQuantity" Caption="Stock Qty" VisibleIndex="7" Visible="false">
                                                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="StockUOM" Caption="Stock UOM" VisibleIndex="8" ReadOnly="true" Visible="false">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Sale Price" VisibleIndex="9" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <%--   <asp:Button ID="ASPxButton2" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />--%>
                                            <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                            </dxe:ASPxButton>
                                            <%--  Text="T&#818;axes"--%>
                                            <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>
                                            <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                            <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                                            <asp:HiddenField ID="hfControlData" runat="server" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="CR" />
                                            <%--   <uc1:VehicleDetailsControl runat="server" id="VehicleDetailsControl" />--%>
                                            <%-- onclick=""--%>
                                            <%--<a href="javascript:void(0);" id="btnAddNew" runat="server" class="btn btn-primary"><span>[A]ttachment(s)</span></a>--%>
                                            <%--<dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[A]ttachment(s)" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>--%>
                                            <%--<a class="btn btn-primary" href="javascript:void(0);" onclick="fn_PopOpen()"><span>[B]illing/Shipping</span> </a>--%>
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
                        <dxe:TabPage Name="Billing/Shipping" Text="Our Billing/Shipping">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">



                                    <%--   <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server" ContentUrl="AddArea_PopUp.aspx"
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
                                                    <%--<MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="PercentageTextChange" />
                                                    <ClientSideEvents />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <%-- <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />--%>
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
                                        <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
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
                                                                    <dxe:ASPxButton ID="ASPxButton4" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
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
                                                    <ClientSideEvents LostFocus="txtserialTextChanged" />
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
                                                    <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Edit">
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

                <asp:HiddenField ID="hdfProductIDPC" runat="server" />
                <asp:HiddenField ID="hdfstockidPC" runat="server" />
                <asp:HiddenField ID="hdfopeningstockPC" runat="server" />
                <asp:HiddenField ID="hdbranchIDPC" runat="server" />
                <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />


                <asp:HiddenField ID="hdfComponentID" runat="server" />
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

                <%-- Surojit 12-03-2019 --%>
                <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
                <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
                <%-- Surojit 12-03-2019 --%>

                 <%-- Surojit 20-05-2019 --%>
                 <asp:HiddenField runat="server" ID="hdnPostingDateDisable" />
                 <%-- Surojit 20-05-2019 --%>
            </div>

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
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <dxe:ASPxPopupControl ID="ProductpopUpdis" runat="server" ClientInstanceName="cProductpopUpdis"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--  <label><strong>Search By product Name</strong></label>
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
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <%--  <asp:SqlDataSource runat="server" ID="ProductDataSource" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
                SelectCommand="prc_CRMSalesReturn_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                    <asp:SessionParameter Name="campany_Id" SessionField="LastCompanySR" Type="String" />
                    <asp:SessionParameter Type="String" Name="FinYear" SessionField="LastFinYearSR" />
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
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
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



        function changedqnty(s) {

            var qnty = s.GetText();
            var sum = $('#hdntotalqntyPC').val();

            sum = Number(Number(sum) + Number(qnty));
            //alert(sum);
            $('#<%=hdntotalqntyPC.ClientID %>').val(sum);

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






        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=CR&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }

        function acbpCrpUdfEndCall(s, e) {
            //  LoadingPanel.Hide();
            //  debugger;
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
        function blurOut() {
            if (grid.GetVisibleRowsOnPage() == 1) {

                grid.batchEditApi.StartEdit(-1, 2);
            }


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
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");
                HeaderCaption.push("Installation Reqd.");

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
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");
                HeaderCaption.push("Installation Reqd.");

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

    <asp:SqlDataSource runat="server" ID="dsCustomer"
        SelectCommand="prc_CRMSalesReturn_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateCustomerDetail" />
        </SelectParameters>
    </asp:SqlDataSource>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmit1Button"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <%-- <asp:SqlDataSource ID="CustomerDataSource" runat="server"  ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"/>--%>

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
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>
                                <th>Installation Reqd.</th>
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
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>
                                <th>Installation Reqd.</th>
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

