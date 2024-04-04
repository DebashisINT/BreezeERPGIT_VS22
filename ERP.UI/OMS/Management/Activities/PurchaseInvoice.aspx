<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                05-05-2023        V2.0.37           Pallab              26040: Add Purchase Invoice module design modification & check in small device
2.0                25-10-2023        V2.0.40           Priti               0026898:Global level round off is not coming while tagging GRN into the Invoice. 
3.0                11-01-2024        V2.0.42           Priti               0027050: A settings is required for the Duplicates Items Allowed or not in the Transaction Module.
                                                 
====================================================== Revision History =============================================--%>

<%@ Page Title="Purchase Invoice" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="PurchaseInvoice.aspx.cs" Inherits="ERP.OMS.Management.Activities.PurchaseInvoice" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/VendorBillingShipping.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/ucImportPurchase_BillOfEntry.ascx" TagPrefix="uc1" TagName="ucImportPurchase_BillOfEntry" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.8.2/css/all.css" integrity="sha384-oS3vJWv+0UjzBfQzYUhtDYW+Pj2yciDJxpsK1OYPAYjqT085Qq/1cq5FLXAZQ7Ay" crossorigin="anonymous" />
    <script src="../../../assests/pluggins/choosen/choosen.min.js"></script>
    <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <%-- <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>--%>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelPurchase.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <%-- <script src="JS/SearchPopup.js"></script>--%>
    <script src="JS/SearchPopupDatatable.js"></script>
    <script src="JS/PurchaseInvoiceAdd.js?v=4.3"></script>
    <style type="text/css">
        .CUSTOM-CLASS .tooltip-inner {
            background: #268c7e
        }

        .statusBar a:first-child {
            display: none;
        }

        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }


        #DivBilling [class^="col-md"], #DivShipping [class^="col-md"] {
            padding-top: 5px;
            padding-bottom: 5px;
        }


        .voucherno {
            position: absolute;
            right: -3px;
            top: 22px;
        }

        .POVendor {
            position: absolute;
            right: -3px;
            top: 22px;
        }

        #Popup_NoofCopies_HCB-1 {
            display: none;
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

        #grid_DXStatus span > a, #grid_DXStatus a {
            display: none;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .absolute, #grid_DXMainTable .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .col-md-3 > label, .col-md-3 > span {
            margin-top: 0px;
            display: inline-block;
        }

        #grid_DXMainTable > tbody > tr > td.abcd, #grid_DXMainTable > tbody > tr > td:nth-child(26) {
            display: none !important;
        }

        #aspxGridTax_DXStatus {
            display: none !important;
        }

        .mTop {
            margin-top: 10px;
        }

        .pullleftClass {
            position: absolute;
            right: -3px;
            top: 20px;
        }

        .inline {
            display: inline !important;
        }

        .statusBar {
            display: none;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        strong .dxeBase_PlasticBlue {
            font-weight: 700 !important;
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

        .bod-table {
            width: 100%;
            border-radius: 5px;
        }

            .bod-table > tbody > tr > td {
                padding: 5px;
                background: #c2d8e6;
                font-weight: 500;
            }

            .bod-table.none > tbody > tr > td {
                background: none;
            }

        .bac {
            background: #c2d8e6;
            margin: 10px 0;
            padding: 2px 15px;
            border-radius: 5px;
        }

        .greyd {
            background: #ececec;
            margin: 10px 0;
            padding: 0px 15px;
            border-radius: 5px;
        }

        .newLbl .lblHolder table tr:first-child td {
            background: #2bb1bf;
        }

        table.pad > tbody > tr > td {
            padding: 0px 10px;
        }

        section.rds {
            margin-top: 25px;
            border: 1px solid #ccc;
            padding: 3px 15px;
        }

        span.fieldsettype {
            background: #1671b7;
            padding: 8px 10px;
            color: #fff;
            position: relative;
            top: -10px;
            z-index: 5;
        }

            span.fieldsettype::before {
                content: "";
                border-left: 9px solid transparent;
                border-right: 9px solid transparent;
                border-bottom: 13px solid #184d75;
                position: absolute;
                right: -9px;
                z-index: -1;
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

        .sendMailCheckbox {
            padding-top: 16px;
            padding-left: 15px;
        }

            .sendMailCheckbox > label {
                -webkit-transform: translateY(-3px);
                -moz-transform: translateY(-3px);
                transform: translateY(-3px);
            }


        ul.ks-cboxtags {
            list-style: none;
            padding: 0;
        }

            ul.ks-cboxtags li {
                display: inline;
            }

                ul.ks-cboxtags li label {
                    display: inline-block;
                    background-color: rgba(255, 255, 255, .9);
                    border: 2px solid rgba(139, 139, 139, .3);
                    color: #adadad;
                    border-radius: 25px;
                    white-space: nowrap;
                    margin: 3px 0px;
                    -webkit-touch-callout: none;
                    -webkit-user-select: none;
                    -moz-user-select: none;
                    -ms-user-select: none;
                    user-select: none;
                    -webkit-tap-highlight-color: transparent;
                    transition: all .2s;
                }

                ul.ks-cboxtags li label {
                    padding: 8px 12px;
                    cursor: pointer;
                }

                    ul.ks-cboxtags li label::before {
                        display: inline-block;
                        font-style: normal;
                        font-variant: normal;
                        text-rendering: auto;
                        -webkit-font-smoothing: antialiased;
                        font-family: "Font Awesome 5 Free";
                        font-weight: 900;
                        font-size: 12px;
                        padding: 2px 6px 2px 2px;
                        content: "\f058";
                        transition: transform .3s ease-in-out;
                    }

                ul.ks-cboxtags li input[type="checkbox"]:checked + label::before {
                    content: "\f057";
                    transform: rotate(-360deg);
                    transition: transform .3s ease-in-out;
                }

                ul.ks-cboxtags li input[type="checkbox"]:checked + label {
                    border: 2px solid #1bdbf8;
                    background-color: #12bbd4;
                    color: #fff;
                    transition: all .2s;
                }

                ul.ks-cboxtags li input[type="checkbox"] {
                    display: absolute;
                }

                ul.ks-cboxtags li input[type="checkbox"] {
                    position: absolute;
                    opacity: 0;
                }

                    ul.ks-cboxtags li input[type="checkbox"]:focus + label {
                        border: 2px solid #e9a1ff;
                    }

        .eqTble > tbody > tr > td {
            padding: 0 7px;
            vertical-align: top;
        }
    </style>
    <style>
        .myImage {
            max-height: 100px;
            max-width: 100px;
        }

        .boxarea {
            border: 1px solid #a7a6a64a;
            position: relative;
            margin: 15px;
            padding-top: 8px;
            padding-bottom: 7px;
        }

        .boxareaH {
            position: absolute;
            font-size: 14px;
            font-weight: bold;
            top: -13px;
            left: 9px;
            /* border: 1px solid #ccc; */
            background: #edf3f4;
            padding: 3px 5px;
            color: #b11212;
        }
    </style>
    <style>
        .imageArea {
            width: 150px;
            height: 100px !important;
            overflow: hidden;
        }

        .popUpHeader {
            float: right;
        }

        .blll {
            margin: 0;
            padding: 0 !important;
            margin-top: 6px;
        }

        .dxeErrorCellSys.dxeNoBorderLeft {
            position: absolute;
        }

        .mkSht {
            width: 100%;
        }

            .mkSht > tbody > tr > td {
                padding: 2px 5px;
            }

        .multiply {
            padding-top: 18px !important;
            font-size: 14px;
            font-weight: 600;
            color: #b11212;
        }

        .mlableWh {
            padding-top: 22px;
            display: inline-block
        }

            .mlableWh > input + span {
                white-space: nowrap;
            }
    </style>
    <style type="text/css">
        #Popup_Empcitys_active_DXPWMB-1 {
            display: none;
        }

        .cityDiv {
            height: 25px;
        }

        .cityTextbox {
            height: 25px;
            width: 50px;
        }

        .mtop8 {
            margin-top: 8px;
        }

        .Top {
            height: 90px;
            width: 100%;
            padding-top: 5px;
            valign: top;
        }

        .Footer {
            height: 30px;
            width: 400px;
            padding-top: 10px;
        }

        .ScrollDiv {
            height: 250px;
            width: 400px;
            overflow-x: hidden;
            overflow-y: scroll;
        }

        .ContentDiv {
            width: 100%;
            height: 300px;
            border: 2px;
        }



        .TitleArea {
            height: 20px;
            padding-left: 10px;
            padding-right: 3px;
            background-image: url( '../images/EHeaderBack.gif' );
            background-repeat: repeat-x;
            background-position: bottom;
            text-align: center;
        }

        .FilterSide {
            float: left;
            width: 50%;
        }

        .SearchArea {
            width: 100%;
            height: 30px;
            padding-top: 5px;
        }

        .newLbl {
            margin: 5px 0 !important;
            display: inline-block;
        }

        .sText {
            font-size: 10px;
        }
    </style>



    <script>
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';



        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= cPLQuoteDate.GetDate()) && (cPLQuoteDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between  " + $("#hdnDatafrom").val() + " to " + $("#hdnDatato").val());
            }
            var VendorId = $("#hdnCustomerId").val();
            var startDate = new Date();
            startDate = cPLQuoteDate.GetDate().format('yyyy-MM-dd');
            cPanelGRNOverheadCost.PerformCallback('BindOverheadCostGrid' + '~' + startDate);
            clookup_GRNOverhead.gridView.Refresh();

        }




        function GridClearConfirm() {
            jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    var key = GetObjectID('hdnCustomerId').value;
                    if (key != null && key != '') {
                        if ($('#<%=hdnTaggedVender.ClientID %>').val() != null && $('#<%=hdnTaggedVender.ClientID %>').val() != '') {

                            $('#<%=hdnTaggedVender.ClientID %>').val(key);
                            $('#<%=hdnTaggedVendorName.ClientID %>').val(ctxtVendorName.GetText());
                            //gridquotationLookup.SetText('');
                        }
                        $('.dxeErrorCellSys').addClass('abc');
                        var startDate = new Date();
                        startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
                        var key = GetObjectID('hdnCustomerId').value;

                        if (key != null && key != '') {
                            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                        }

                        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                            clearTransporter();
                        }
                        //###### Added By : Samrat Roy ########## 
                        if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                            var schemabranchid = $('#ddl_numberingScheme').val();
                            if (schemabranchid != '0') {
                                var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                                //LoadCustomerAddress(key, schemabranch, 'PB');
                                SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                                //page.tabs[0].SetEnabled(true);
                                page.tabs[1].SetEnabled(true);
                                //selectValue();
                            }
                        }
                        else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') {
                            var schemabranchid = $('#ddl_Branch').val();
                            if (schemabranchid != '0') {
                                var schemabranch = schemabranchid;
                                // Geet on 15102017 Start
                                SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                                // LoadCustomerAddress(key, schemabranch, 'PB');
                                // Geet on 15102017 End
                                //page.tabs[0].SetEnabled(true);
                                page.tabs[1].SetEnabled(true);
                            }
                        }
                        else {
                            jAlert('Select a numbering schema first');
                            return;
                        }
                    }
                    else {
                        jAlert('Vendor can not be blank.')
                        //Pending Section Start              
                        //Pending Section End
                    }
                    //Chinmoy edited below line
                    var invtype = $('#ddlInventory').val();
                    if (key != null && key != '') {
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                    }
                    grid.PerformCallback('GridBlank');
                    ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                }
                else {
                    //Pending Section Start
                    var vendorid = $('#<%=hdnTaggedVender.ClientID %>').val();
                    GetObjectID('hdnCustomerId').value = vendorid;
                    ctxtVendorName.SetText($('#<%=hdnTaggedVendorName.ClientID %>').val());
                    //gridLookup.PerformCallback(vendorid) 
                    //Pending Section End
                }
            });
        }



        function CmbScheme_ValueChange() {
            cddlPosGstInvoice.ClearItems();
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            if (noofvisiblerows == '0') {
                grid.AddNewRow();
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue('1');
            }
            //gridLookup.SetValue('');
            ctxtVendorName.SetText('');
            GetObjectID('hdnCustomerId').value = '';
            var val = $("#ddl_numberingScheme").val();
            var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
            $("#hdnTCBranchId").val(schemabranch);
            if (document.getElementById('btn_TermsCondition')) {
                BinducTcBank();
            }
            if (val != '0') {
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                if (type == 'PO' || type == 'PC') {
                    selectValue();
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: "purchaseinvoice.aspx/BindBranchByParentID",
                        data: JSON.stringify({ schemabranch: schemabranch }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var ddl_Branch = $("[id*=ddl_Branch]");
                            var list = msg.d;

                            if (list.length > 0) {
                                $(".lst-clear").empty();
                                var option = document.createElement('option');
                                for (var i = 0; i < list.length; i++) {

                                    var id = '';
                                    var name = '';
                                    id = list[i].split('~')[0];
                                    name = list[i].split('~')[1];
                                    ddl_Branch.append($("<option></option>").val(id).html(name));
                                }
                            }
                        }
                    });
                }
                var schemabranchid = val.toString().split('~')[1];
                if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                    var schemabranchid = $('#ddl_numberingScheme').val();
                    if (schemabranchid != '0') {
                        var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                        //page.tabs[1].SetEnabled(true);
                        document.getElementById('ddl_Branch').value = schemabranch;
                    }
                }
                else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') {
                    var schemabranchid = $('#ddl_Branch').val();
                    if (schemabranchid != '0') {
                        var schemabranch = schemabranchid;
                        //page.tabs[1].SetEnabled(true);
                        document.getElementById('ddl_Branch').value = schemabranch;
                    }
                }
                $.ajax({
                    type: "POST",
                    url: 'PurchaseInvoice.aspx/getSchemeType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{sel_scheme_id:\"" + val + "\"}",
                    success: function (type) {

                        var schemetypeValue = type.d;

                        var schemetype = schemetypeValue.toString().split('~')[0];
                        var schemelength = schemetypeValue.toString().split('~')[1];
                        //Rev Debashis
                        var fromdate = (schemetypeValue.toString().split('~')[2] != null) ? schemetypeValue.toString().split('~')[2] : "";
                        var todate = (schemetypeValue.toString().split('~')[3] != null) ? schemetypeValue.toString().split('~')[3] : "";
                        debugger;
                        var ff = GetDateFormatForAmt(fromdate);
                        var TT = GetDateFormatForAmt(todate)
                        $('#hdnnumberingFromdate').val(ff);
                        $('#hdnnumberingTodate').val(TT);
                        var dt = new Date();

                        cPLQuoteDate.SetDate(dt);

                        if (dt < new Date(fromdate)) {
                            cPLQuoteDate.SetDate(new Date(fromdate));
                        }

                        if (dt > new Date(todate)) {
                            cPLQuoteDate.SetDate(new Date(todate));
                        }

                        cPLQuoteDate.SetMinDate(new Date(fromdate));
                        cPLQuoteDate.SetMaxDate(new Date(todate));



                        //cPLQuoteDate.SetMinDate(new Date(fromdate));
                        //cPLQuoteDate.SetMaxDate(new Date(todate));
                        //End of Rev Debashis
                        $('#txtVoucherNo').attr('maxLength', schemelength);
                        if (schemetype == '0') {

                            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";


                            if ($('#<%=hdnManual.ClientID %>').val() == 'N') {
                                cPLQuoteDate.SetEnabled(false);
                            }
                            else if ($('#<%=hdnManual.ClientID %>').val() == 'Y') {
                                cPLQuoteDate.SetEnabled(true);
                            }
                            $("#txtVoucherNo").focus();
                        }
                        else if (schemetype == '1') {

                            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                            if (cPLQuoteDate.clientEnabled == false) {
                                ctxt_Refference.Focus();
                            }
                            else {
                                cPLQuoteDate.Focus();
                            }
                            $("#MandatoryBillNo").hide();
                            if ($('#<%=hdnAuto.ClientID %>').val() == 'N') {
                                cPLQuoteDate.SetEnabled(false);
                            }
                            else if ($('#<%=hdnAuto.ClientID %>').val() == 'Y') {
                                cPLQuoteDate.SetEnabled(true);
                            }
                            if ($("#HdnBackDatedEntryPurchaseGRN").val() == "1") {
                                cPLQuoteDate.SetEnabled(true);
                            }
                            else {
                                if ($("#hdnBackdateddate").val() != "0" && $("#hdnBackdateddate").val() != "") {
                                    var Days = $("#hdnBackdateddate").val();
                                    var today = cPLQuoteDate.GetDate();
                                    var newdate = cPLQuoteDate.GetDate();
                                    newdate.setDate(today.getDate() - Math.round(Days));
                                    cPLQuoteDate.SetMinDate(newdate);
                                    cPLQuoteDate.SetMaxDate(new Date(todate));
                                    cPLQuoteDate.SetEnabled(true);
                                }
                                else {
                                    cPLQuoteDate.SetEnabled(false);
                                }
                            }

                        }
                        else if (schemetype == '2') {

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
            else {
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                document.getElementById('ddl_Branch').value = '<%=Session["userbranchID"]%>';
            }
            //Chinmoy added below line
            //SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
            //clookup_Project.gridView.Refresh();

            var startDate = new Date();
            startDate = cPLQuoteDate.GetDate().format('yyyy-MM-dd');
            cPanelGRNOverheadCost.PerformCallback('BindOverheadCostGrid' + '~' + startDate);
            clookup_GRNOverhead.gridView.Refresh();
        }

        function DuplicatePartyNo() {

            var invtype = $('#ddlInventory').val();
            var partyno = '';
            if (invtype != 'N') {
                var PBid = ''
                var partyno = ctxt_partyInvNo.GetText();

                if (partyno != null && partyno != '') {
                    $("#MandatorysPartyinvno").hide();
                }
                else {
                    $("#MandatorysPartyinvno").show();
                    return;
                }
                //var vendorid = gridLookup.GetValue()
                var vendorid = GetObjectID('hdnCustomerId').value;
                if (vendorid != null && vendorid != '') {

                    if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                        mode = 'A'
                    }
                    else {
                        mode = 'E'
                        PBid = "<%=Convert.ToString(Session["PurchaseInvoice_Id"])%>"
                    }
                    if (document.getElementById('hdnAllowDuplicatePartyInvoiceNo').value == 0) {
                        $.ajax({
                            type: "POST",
                            url: "purchaseinvoice.aspx/CheckUniquePartyNo",
                            data: JSON.stringify({ vendorid: vendorid, partyno: partyno, mode: mode, PBid: PBid }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                                var data = msg.d;

                                if (data == true) {
                                    $("#DuplicatePartyinvno").show();
                                    ctxt_partyInvNo.SetText('');
                                    ctxt_partyInvNo.Focus();
                                }
                                else {
                                    $("#DuplicatePartyinvno").hide();
                                }
                            }
                        });
                    }
                }
            }
            else {
                $("#MandatorysPartyinvno").hide();
            }
        }
        function PerformCallToGridBind() {
            //debugger;
            if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
        var loadingmade = $('#<%=hdnADDEditMode.ClientID %>').val();
        $("#rdl_PurchaseInvoice_0").prop('checked', true);
        grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
        $('#hdnPageStatus').val('Quoteupdate');
        cProductsPopup.Hide();
        cddlPosGstInvoice.SetEnabled(false);
        ctxtVendorName.SetEnabled(false);

        //#### added by Samrat Roy for Transporter Control #############
        var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                    callTransporterControl(quote_Id[0], $("#rdl_PurchaseInvoice").find(":checked").val());
                }
                if (quote_Id.length > 0) {
                    //Chinmoy edited below line
                    GetPurchaseAddress(quote_Id[0], $("#rdl_PurchaseInvoice").find(":checked").val());
                    // BSDocTagging(quote_Id[0], $("#rdl_PurchaseInvoice").find(":checked").val());
                }


                if (quote_Id.length > 0) {
                    BindOrderProjectdata(quote_Id[0], $("#rdl_PurchaseInvoice").find(":checked").val());
                }


                if ($("#btn_TermsCondition").is(":visible")) {
                    callTCControl(quote_Id[0], $("#rdl_PurchaseInvoice").find(":checked").val());
                }
            }
            else {
                cProductsPopup.Hide();
            }
            return false;
        }
        function OnEndCallback(s, e) {
           // debugger;
            var pageStatus = document.getElementById('hdnPageStatus').value;
            var value = document.getElementById('hdnRefreshType').value;
            var pageStatus = document.getElementById('hdnPageStatus').value;
            LoadingPanel.Hide();
            if (grid.cpComponent) {
                if (grid.cpComponent == 'true') {
                    grid.cpComponent = null;
                    $('#<%=hdfIsComp.ClientID %>').val('');
                    OnAddNewClick();
                   // debugger;
                }
            }
            if (grid.cpSaveSuccessOrFail == "outrange") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');
            }
            else if (grid.cpSaveSuccessOrFail == "AddressProblem") {
                cbtn_SaveRecords.SetEnabled(true);
                page.tabs[1].SetEnabled(true);
                jAlert("Billing and Shipping Address can not be blank.Save unsuccessful.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
            }


            else if (grid.cpSaveSuccessOrFail == "BillingShippingNull") {
                cbtn_SaveRecords.SetEnabled(true);
                page.tabs[1].SetEnabled(true);
                jAlert("Billing Shipping Address can not be blank.Save unsuccessful.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
            }
            //Rev Bapi add Condition ddl_VendorType.value!="I"
            else if (grid.cpSaveSuccessOrFail == "TDSMandatory") {
                grid.cpSaveSuccessOrFail = null;
                ShowTDS();
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
            else if (grid.cpSaveSuccessOrFail == "AddLock") {
                LoadingPanel.Hide();
                jAlert('DATA is Freezed between ' + grid.cpAddLockStatus);
                OnAddNewClick();
            }
            // Rev Mantis Issue 24061
            else if (grid.cpSaveSuccessOrFail == "NetAmountExceed") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Net Amount of selected Product from tagged document.<br />Cannot enter Net Amount more than Purchase Order Net Amount .');
                grid.cpSaveSuccessOrFail = '';
            }
            // End of Rev Mantis Issue 24061
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                OnAddNewClick();
                cbtn_SaveRecords.SetEnabled(true);
                jAlert("Duplicate Product not allowed.Save unsuccessful.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "checkAcurateTaxAmount") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Check GST Calculated for Item ' + grid.cpProductName + ' at line ' + grid.cpSerialNo);
                grid.cpSaveSuccessOrFail = '';
                grid.cpSerialNo = '';
                grid.cpProductName = '';
            }
            else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {

                cbtn_SaveRecords.SetEnabled(true);
                jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;

            }
            else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
                cbtn_SaveRecords.SetEnabled(true);
                jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Save as Duplicate Quotation Numbe No. Found');
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please try after sometime.');
            }
            else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please select project.');
            }
            else if (grid.cpSaveSuccessOrFail == "ChallanTaggingMandatory") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('GRN tagging is mandatory, Cannot proceed.');
            }

            else if (grid.cpSaveSuccessOrFail == "Ponotexist") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Cannot Save. Selected Purchase Indent(s) in this document do not exist.');
            }
            //Registered Vendor Address Checking 
            else if (grid.cpSaveSuccessOrFail == "VendorAddressProblem") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('You must enter Vendor Billing and Shipping in Vendor Master and set as default to proceed further.');
            }
            else if (grid.cpRVMechMainAc == '-20') {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Reverse Charge is applicable here. No ledger is found mapped for posting within Masters->Accounts->Tax Component Scheme->"Reverse Charge Posting Ledger". Cannot Proceed.');
                OnAddNewClick();
            }
            else if (grid.cpReturnLedgerAmt == '-3') {
                var dramt = 0;
                var cramt = 0;
                if (grid.cpDRAmt != null) {
                    dramt = grid.cpDRAmt
                }
                if (grid.cpCRAmt != null) {
                    cramt = grid.cpCRAmt
                }
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                //jAlert('Db toatl= ' + dramt + '.......Cr total= ' + cramt + ' Mismatch Detected.<br/>Cannot Save.');
                jAlert('Mismatch detected in total of Debit & Credit Values.<br/>Cannot Save.');
                grid.cpReturnLedgerAmt = null;
                grid.cpDRAmt = null;
                grid.cpCRAmt = null;
                OnAddNewClick();
            }
            else {
                debugger;
                var PurchaseOrder_Number = grid.cpPurchaseOrderNo;
                var Quote_ID = grid.cpGeneratedInvoice;
                var Order_Msg = "Purchase Invoice No. " + PurchaseOrder_Number + " saved.";
                if (value == "E") {
                    cbtn_SaveRecords.SetEnabled(true);
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else if (grid.cpApproverStatus == "rejected") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    // window.location.assign("PurchaseInvoicelist.aspx");
                    if (PurchaseOrder_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [PurchaseInvoice]', function (r) {
                            if (r == true) {
                                if ($('#<%=hdnPBAutoPrint.ClientID %>').val() == "1") {
                            NewExit = 'E';
                            cPopup_NoofCopies.Show();
                        }
                        else {
                            grid.cpPurchaseOrderNo = null;
                            grid.cpGeneratedInvoice = null;
                            window.location.assign("PurchaseInvoicelist.aspx");
                        }
                    }
                });
            }
            else {
                window.location.assign("PurchaseInvoicelist.aspx");
            }
        }
        else if (value == "N") {
            cbtn_SaveRecords.SetEnabled(true);
            if (grid.cpApproverStatus == "approve") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else if (grid.cpApproverStatus == "rejected") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }


            if (PurchaseOrder_Number != "") {

                jAlert(Order_Msg, 'Alert Dialog: [PurchaseInvoice]', function (r) {
                    if (r == true) {
                        if ($('#<%=hdnPBAutoPrint.ClientID %>').val() == "1") {
                                     NewExit = 'N';
                                     cPopup_NoofCopies.Show();
                                 }
                                 else {
                                     grid.cpPurchaseOrderNo = null;
                                     grid.cpGeneratedInvoice = null;
                                     window.location.assign("purchaseinvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
                                 }
                                 //grid.cpPurchaseOrderNo = null;
                                 //grid.cpGeneratedInvoice = null;
                                 //window.location.assign("purchaseinvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
                             }
                         });
            }
            else {
                window.location.assign("purchaseinvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
            }
        }
        else {
            if (pageStatus == "first") {
                if (grid.GetVisibleRowsOnPage() == 0) {
                    OnAddNewClick();
                    grid.batchEditApi.EndEdit();
                    $('#<%=hdnPageStatus.ClientID %>').val('');
                    var val = '<%= Session["schemavaluePB"] %>';
                    if (val != '') {
                        $.ajax({
                            type: "POST",
                            url: 'PurchaseInvoice.aspx/getSchemeType',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: "{sel_scheme_id:\"" + val + "\"}",
                            success: function (type) {
                                var schemetypeValue = type.d;
                                var schemetype = schemetypeValue.toString().split('~')[0];
                                var schemelength = schemetypeValue.toString().split('~')[1];
                                $('#txtVoucherNo').attr('maxLength', schemelength);
                                if (schemetype == '0') {
                                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                                             document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                                             $("#txtVoucherNo").focus();
                                             if ($('#<%=hdnManual.ClientID %>').val() == 'N') {
                                                 cPLQuoteDate.SetEnabled(false);
                                             }
                                             else if ($('#<%=hdnManual.ClientID %>').val() == 'Y') {
                                                 cPLQuoteDate.SetEnabled(true);
                                             }
                                         }
                                         else if (schemetype == '1') {
                                             document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                                             document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                                             cPLQuoteDate.Focus();
                                             $("#MandatoryBillNo").hide();
                                             if ($('#<%=hdnAuto.ClientID %>').val() == 'N') {
                                                 cPLQuoteDate.SetEnabled(false);
                                             }
                                             else if ($('#<%=hdnAuto.ClientID %>').val() == 'Y') {
                                                 cPLQuoteDate.SetEnabled(true);
                                             }
                                         }
                                         else if (schemetype == '2') {
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
                }
            }
            else if (pageStatus == "update") {
                OnAddNewClick();
                $('#<%=hdnPageStatus.ClientID %>').val('');
            }
            else if (pageStatus == "Quoteupdate") {
                cProductsPopup.Hide();
                grid.StartEditRow(0);
                $('#<%=hdnPageStatus.ClientID %>').val('');
            }
            else if (pageStatus == "delete") {
                var inventoryItem = $('#ddlInventory').val();
                if (inventoryItem == 'N') {
                    var schemeid = cddl_TdsScheme.GetValue()
                }
                OnAddNewClick();
                $('#<%=hdnPageStatus.ClientID %>').val('');
                    }

                }
            }
            if (grid.cpdelete != null && grid.cpdelete != '' && grid.cpdelete != undefined) {
                if (grid.cpdelete == 'Y') {
                    $('#<%=hdnDeleteSrlNo.ClientID %>').val('');
                }
            }
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem != 'N') {
                if (gridquotationLookup.GetValue() != null) {
                    grid.GetEditor('ProductName').SetEnabled(false);
                    grid.GetEditor('Description').SetEnabled(false);
                    grid.StartEditRow(0);
                    $('#<%=hdnPageStatus.ClientID %>').val('');
                }
            }
            if (cchk_reversemechenism.GetValue()) {
                grid.GetEditor('TaxAmount').SetEnabled(false);
            }
            if (grid.cpPurchaseorderbindnewrow == "yes") {
                grid.AddNewRow();
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                grid.cpPurchaseorderbindnewrow = null;
            }
            if (grid.cpOrderRunningBalance) {
                var RunningBalance = grid.cpOrderRunningBalance;
                var RunningSpliteDetails = RunningBalance.split("~");
                grid.cpOrderRunningBalance = null;
                var SUM_ChargesAmount = RunningSpliteDetails[0];
                var SUM_Amount = RunningSpliteDetails[1];
                var SUM_TaxAmount = RunningSpliteDetails[3];
                var SUM_TotalAmount = RunningSpliteDetails[4];
                var SUM_ProductQuantity = parseFloat(RunningSpliteDetails[6]).toFixed(2);
                cTaxableAmtval.SetValue(SUM_Amount);
                cTaxAmtval.SetValue(SUM_TaxAmount);
                ctxt_Charges.SetValue(SUM_ChargesAmount);
                cOtherTaxAmtval.SetValue(SUM_ChargesAmount);
                cInvValue.SetValue(SUM_TotalAmount);

                cTotalQty.SetValue(SUM_ProductQuantity);

                //Rev 2.0
                // cTotalAmt.SetValue(SUM_TotalAmount);
                var SUM_Total_Amount = RunningSpliteDetails[7];
                cTotalAmt.SetValue(SUM_Total_Amount);
                //Rev 2.0 End
            }
        }
        function ddl_Currency_Rate_Change() {
            var Campany_ID = '<%=Session["LastCompany"]%>';
    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var Currency_ID = $("#ddl_Currency").val();
            if (Currency_ID == basedCurrency[0]) {
                ctxtRate.SetValue("0.00");
                ctxtRate.SetEnabled(false);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "purchaseinvoice.aspx/GetRate",
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

        function DateCheck() {
            var invtype = $('#ddlInventory').val();
            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";

            var endDate = cPLQuoteDate.GetValue();
            var str = $.datepicker.formatDate('yy-mm-dd', endDate);
            var checkval = cchk_reversemechenism.GetChecked();
            //var key = gridLookup.GetValue()
            var key = GetObjectID('hdnCustomerId').value;
            // Waiting for Dirction Start

            // Waiting for Dirction  End 
            if (gridquotationLookup.GetValue() != null) {

                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        page.SetActiveTabIndex(0);
                        $('.dxeErrorCellSys').addClass('abc');
                        var startDate = new Date();
                        startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                        //var key = gridLookup.GetValue()
                        var key = GetObjectID('hdnCustomerId').value;

                        if (key != null && key != '') {
                            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                            if (type != "") {
                                cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                            }
                            else {
                                jAlert('Please Check Radio Button Value.')
                                return;
                            }

                        }
                        grid.PerformCallback('GridBlank');
                        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                    clearTransporter(); disc
                }
                ccmbGstCstVat.PerformCallback();
                ccmbGstCstVatcharge.PerformCallback();
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            }
        });
            }
            else {
                var startDate = new Date();
                startDate = cPLQuoteDate.GetValueString();
                //var key = gridLookup.GetValue()
                var key = GetObjectID('hdnCustomerId').value;
                if (key != "" && key != null) {
                    var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                    var componentType = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
                    cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                    if (key != null && key != '' && type != "") {
                        cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                    }
                    if (componentType != null && componentType != '') {
                        grid.PerformCallback('GridBlank');
                    }
                }
            }
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
    <style>
        #dvInvoicedetAmount {
            display: inline-block;
            background: #fff;
            border: 1px solid #2597d3;
            border-radius: 4px;
            float: right;
            font-size: 14px !important;
            margin-right: 36px;
        }

            #dvInvoicedetAmount > span {
                padding: 2px 10px;
            }

                #dvInvoicedetAmount > span:first-child {
                    display: block;
                    font-weight: 500;
                    background: #2296d1;
                    color: #fff;
                    font-size: 12px;
                }
    </style>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />

    <style>
        select {
            z-index: 1;
        }

        #grid {
            max-width: 98% !important;
        }

        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PartyDate, #dt_partyInvDt, #dt_EntryDate {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1, #toDate_B-1, #dtTDate_B-1, #dt_PLQuote_B-1, #dt_PartyDate_B-1, #dt_partyInvDt_B-1, #dt_EntryDate_B-1 {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

            #FormDate_B-1 #FormDate_B-1Img, #toDate_B-1 #toDate_B-1Img, #dtTDate_B-1 #dtTDate_B-1Img, #dt_PLQuote_B-1 #dt_PLQuote_B-1Img,
            #dt_PartyDate_B-1 #dt_PartyDate_B-1Img, #dt_partyInvDt_B-1 #dt_partyInvDt_B-1Img, #dt_EntryDate_B-1 #dt_EntryDate_B-1Img {
                display: none;
            }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon {
            right: 20px;
            bottom: 8px;
        }

        .padTabtype2 > tbody > tr > td {
            vertical-align: bottom;
        }

        #rdl_Salesquotation {
            margin-top: 0px;
        }

        .lblmTop8 > span, .lblmTop8 > label {
            margin-top: 0 !important;
        }

        .col-md-2, .col-md-4 {
            margin-bottom: 10px;
        }

        .simple-select::after {
            top: 26px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue {
            padding: 0;
        }

        .aspNetDisabled {
            background: #f3f3f3 !important;
        }

        .backSelect {
            background: #42b39e !important;
        }

        #ddlInventory {
            -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl {
            margin: 3px 0 !important;
        }

        .ui-widget.ui-widget-content {
            position: fixed;
            top: 40%;
            left: 40%;
        }

        #Popup_NoofCopies_PW-1 {
            position: fixed !important;
            top: 25% !important;
        }
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
            <div class="panel-title clearfix">
                <h3 class="pull-left">
                    <span class="">
                        <asp:Label ID="lblHeading" runat="server" Text="Add Purchase Invoice"></asp:Label>

                    </span>
                </h3>

                <div id="pageheaderContent" class="scrollHorizontal pull-right reverse wrapHolder content horizontal-images" style="display: none;" runat="server">
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
                                                <asp:Label ID="Label1" runat="server"></asp:Label>
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
                <div runat="server" id="dvInvoicedetAmount">
                    <span onclick="InvoiceDetails()" title="InvoiceDetails">Invoice Details</span>
                    <span id="InvAmount" runat="server"></span>
                </div>
                <div id="ApprovalCross" runat="server" class="crossBtn">
                    <a href=""><i class="fa fa-times"></i></a>

                </div>
                <%--<div id="divcross1" runat="server" class="crossBtn" margin-left: 50px;">--%>
                <div id="crossdiv" runat="server" class="crossBtn">
                    <a href="purchaseinvoicelist.aspx"><i class="fa fa-times"></i></a>
                </div>
                <%--</div>--%>
            </div>

        </div>

        <div class="form_main row clearfix">


            <%--<div class="row">--%>
            <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                <TabPages>
                    <dxe:TabPage Name="General" Text="General">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <div class="row wid-90">
                                    <div class="col-md-2 ">
                                        <div class="cityDiv " style="height: auto;">
                                            <asp:Label ID="Label12" runat="server" Text="Type" CssClass="newLbl"></asp:Label>
                                        </div>
                                        <div class="Left_Content">
                                            <asp:DropDownList ID="ddlInventory" runat="server" Width="100%" CssClass="backSelect" onchange="ddlInventory_OnChange()">
                                                <asp:ListItem Value="B">Both</asp:ListItem>
                                                <asp:ListItem Value="Y">Inventory Items</asp:ListItem>
                                                <asp:ListItem Value="N">Non-Inventory Items</asp:ListItem>
                                                <asp:ListItem Value="C">Capital Goods</asp:ListItem>
                                                <asp:ListItem Value="S">Service</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-2 lblmTop8 simple-select" runat="server" id="divNumberingScheme">
                                        <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="160px" runat="server" Text="Numbering Scheme">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" DataSourceID="SqlSchematype" DataTextField="SchemaName" DataValueField="ID"
                                            onchange="CmbScheme_ValueChange()">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No.">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()">                             
                                        </asp:TextBox>
                                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        <span id="DuplicateBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Duplicate Bill Number not allowed"></span>


                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>

                                        <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLQuoteDate" Width="100%">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents DateChanged="function(s, e) { DateCheck()}" GotFocus="function(s,e){cPLQuoteDate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}" />
                                        </dxe:ASPxDateEdit>
                                        <span id="MandatoryDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        <%--Rev 1.0--%>
                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon" />
                                        <%--Rev end 1.0--%>
                                    </div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-2 lblmTop8 simple-select">
                                        <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="For Unit">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" onchange="CmbBranch_ValueChange()" CssClass="lst-clear">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 lblmTop8" id="DivForUnit" runat="server">
                                        <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="For Unit">
                                        </dxe:ASPxLabel>

                                        <asp:DropDownList ID="ddlForBranch" runat="server" Width="100%">
                                        </asp:DropDownList>

                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%" ClientInstanceName="ctxt_Refference">
                                        </dxe:ASPxTextBox>
                                    </div>


                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <% if (rightsVendor.CanAdd)
                                            { %>
                                        <a href="#" onclick="AddVendorClick()" style="left: -12px; top: 20px; font-size: 16px;"><i id="openlink" runat="server" class="fa fa-plus-circle" aria-hidden="true"></i></a>
                                        <% } %>
                                        <dxe:ASPxButtonEdit ID="txtVendorName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendorName" Width="100%">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                                        </dxe:ASPxButtonEdit>
                                        <span id="MandatorysCustomer" class="customerno pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                        <%--  <dxe:ASPxCallbackPanel runat="server" ID="vendorPanel" ClientInstanceName="cvendorPanel" OnCallback="vendorPanel_Callback">
                                            <PanelCollection>

                                                <dxe:PanelContent runat="server">--%>
                                        <%-- <dxe:ASPxGridLookup ID="lookup_Customer"  runat="server" ClientInstanceName="gridLookup"  OnDataBinding="lookup_Customer_DataBinding" 
                                            KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">

                                            <Columns>
                                                <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" Width="200px" Settings-AutoFilterCondition="Contains" />
                                                <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="2" Caption="Type" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="cnt_internalid" Visible="false" VisibleIndex="3" Settings-AllowAutoFilter="False" Width="200px">
                                                    <Settings AllowAutoFilter="False"></Settings>
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

                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                

                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                             </GridViewProperties>
                                            <ClientSideEvents  TextChanged="function(s, e) { GetContactPerson(e)}" GotFocus="function(s,e){gridLookup.ShowDropDown();}"  />
                                            
                                            <ClearButton DisplayMode="Auto">
                                            </ClearButton>
                                        </dxe:ASPxGridLookup>
                                        <span id="MandatorysVendor" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>--%>
                                        <%-- </dxe:PanelContent>
                                            </PanelCollection>
                                        </dxe:ASPxCallbackPanel>--%>
                                    </div>
                                    <%-- Code Added by Sam on 25052017--%>
                                    <dxe:ASPxCallbackPanel runat="server" ID="partyInvoicepanel" ClientInstanceName="cpartyInvoicepanel" OnCallback="partyInvoicepanel_Callback">
                                        <PanelCollection>

                                            <dxe:PanelContent runat="server">
                                                <div class="col-md-2 lblmTop8">
                                                    <dxe:ASPxLabel ID="lbl_partyInvNo" runat="server" Text="Party Invoice No">
                                                    </dxe:ASPxLabel>
                                                    <dxe:ASPxTextBox ID="txt_partyInvNo" runat="server" Width="100%" ClientInstanceName="ctxt_partyInvNo" MaxLength="16">
                                                        <ClientSideEvents LostFocus="DuplicatePartyNo" />
                                                        <%--<ClientSideEvents  GotFocus="function(s,e){ctxt_partyInvNo.ShowDropDown();}" />--%>
                                                    </dxe:ASPxTextBox>
                                                    <span id="MandatorysPartyinvno" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                    <span id="DuplicatePartyinvno" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Party Invoice No. already exist for the selected vendor."></span>

                                                </div>

                                                <div class="col-md-2 lblmTop8">
                                                    <dxe:ASPxLabel ID="lbl_partyInvDt" runat="server" Text="Party Invoice Date">
                                                    </dxe:ASPxLabel>

                                                    <dxe:ASPxDateEdit ID="dt_partyInvDt" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_partyInvDt"
                                                        Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                        <ClientSideEvents GotFocus="function(s,e){cdt_partyInvDt.ShowDropDown();}" />
                                                        <%--<ClientSideEvents LostFocus="partyInvDtMandatorycheck" GotFocus="function(s,e){cdt_partyInvDt.ShowDropDown();}" />--%>
                                                    </dxe:ASPxDateEdit>
                                                    <span id="MandatoryPartyDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                    <span id="MandatoryEgSDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Party Invoice Date can not be greater than Invoice Date"></span>
                                                    <%--                                            <span id="MandatoryDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>--%>
                                                    <%--Rev 1.0--%>
                                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon" />
                                                    <%--Rev end 1.0--%>
                                                </div>

                                            </dxe:PanelContent>
                                        </PanelCollection>
                                    </dxe:ASPxCallbackPanel>
                                    <%--Code added by Sam on 25052017  --%>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px"
                                            ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                            <ClientSideEvents TextChanged="function(s, e) { GetContactPersonPhone(e)}" GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-2 relative" id="rdlbutton">
                                        <%-- <i class="fa fa-close" style="position:absolute; right:18px;top:0;color:red" aria-hidden="true" title="clear"></i>--%>
                                        <asp:RadioButtonList ID="rdl_PurchaseInvoice" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();" Width="150px">
                                            <asp:ListItem Text="Order" Value="PO"></asp:ListItem>
                                            <asp:ListItem Text="GRN" Value="PC"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel"
                                            OnCallback="ComponentQuotation_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" AllowUserInput="false" runat="server" ClientInstanceName="gridquotationLookup"
                                                        OnDataBinding="lookup_quotation_DataBinding"
                                                        KeyFieldName="ComponentID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">

                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                            <dxe:GridViewDataColumn FieldName="ComponentNumber" Visible="true" VisibleIndex="1" Caption="Document No." Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Posting Date" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Vendor Name" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="branch" Visible="true" VisibleIndex="4" Caption="Unit" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="reference" Visible="true" VisibleIndex="5" Caption="Reference" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="Invtype" Visible="true" VisibleIndex="6" Caption="Type" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="PartyInvoiceNo" Visible="true" VisibleIndex="7" Caption="Party Invoice No" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="PartyInvoiceDate" Visible="true" VisibleIndex="8" Caption="Party Invoice Date" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="Tax_Option" Visible="true" VisibleIndex="9" Caption="Tax" Width="1" />
                                                            <dxe:GridViewDataColumn FieldName="RevNo" Visible="true" VisibleIndex="10" Caption="Revision No" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="RevDate" Visible="true" VisibleIndex="11" Caption="Rev. date" Settings-AutoFilterCondition="Contains" Width="120" />
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

                                                        <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}" />
                                                        <%-- GotFocus="DisableDeleteOption"--%>
                                                    </dxe:ASPxGridLookup>
                                                    <%--GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}"--%>
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
                                                    <div style="padding: 7px 0;" id="divselectunselect" runat="server">
                                                        <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                                                        <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                                        <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                                                    </div>
                                                    <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                                                        Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible" OnCommandButtonInitialize="grid_Products_CommandButtonInitialize">
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
                                                            <dxe:GridViewDataTextColumn Caption="Balance Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                                                <PropertiesTextEdit>
                                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                                </PropertiesTextEdit>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                                            </dxe:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsDataSecurity AllowEdit="true" />

                                                        <ClientSideEvents EndCallback="HideSelectAllSection" />
                                                    </dxe:ASPxGridView>

                                                    <div class="text-center">

                                                        <asp:HiddenField ID="hdnPartyInvoiceList" runat="server" />
                                                        <dxe:ASPxButton ID="Button2" ClientInstanceName="cButton2" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                                        </dxe:ASPxButton>
                                                    </div>
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                                            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

                                        </dxe:ASPxPopupControl>



                                    </div>
                                    <div class="col-md-2 lblmTop8" id="rdldate" runat="server">
                                        <dxe:ASPxLabel ID="lbl_InvoiceNO" runat="server" Text="Order/GRN Date" ClientInstanceName="clbl_InvoiceNO">
                                        </dxe:ASPxLabel>
                                        <div style="width: 100%; height: 23px">
                                            <dxe:ASPxTextBox ID="dt_Quotation" runat="server" Width="100%" DisplayFormatString="dd-MM-yyyy" ClientEnabled="false" ClientInstanceName="cPLQADate">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>



                                    <div class="col-md-2  hide">

                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Cash">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_cashBank" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_cashBank" Width="100%">
                                            <ClientSideEvents GotFocus="function(s,e){cddl_cashBank.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-2 simple-select">
                                        <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" onchange="ddl_Currency_Rate_Change()">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 ">
                                        <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate" CssClass="number" DisplayFormatString="0.00">
                                            <ClientSideEvents LostFocus="ReBindGrid_Currency" GotFocus="GetPreviousCurrency" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div id="divreverse" class="col-md-2" style="padding-top: 18px;">
                                        <dxe:ASPxCheckBox ID="chk_reversemechenism" ClientInstanceName="cchk_reversemechenism" runat="server">
                                            <ClientSideEvents CheckedChanged="RCMCheckChanged" />
                                        </dxe:ASPxCheckBox>
                                        <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Reverse Mechanism">
                                        </dxe:ASPxLabel>
                                    </div>
                                    <div id="divTdsScheme" class="hide" runat="server">
                                        <div class="col-md-2">
                                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Select TDS Section">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxComboBox ID="ddl_TdsScheme" runat="server" OnCallback="ddl_TdsScheme_Callback" Width="100%" ClientInstanceName="cddl_TdsScheme" Font-Size="12px">
                                                <ClientSideEvents TextChanged="function(s, e) { GridProductBind(e)}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <div class="col-md-2">
                                            <div style="margin-top: 17px;">
                                                <%--<asp:CheckBox ID="chkNILRateTDS" runat="server" Text="NIL rate TDS?" TextAlign="Right"></asp:CheckBox>--%>
                                                <dxe:ASPxCheckBox ID="chkNILRateTDS" ClientInstanceName="chkNILRateTDS" Checked="false" Text="NIL TDS ?" TextAlign="Right" runat="server">
                                                </dxe:ASPxCheckBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2 ">
                                        <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                            <ClientSideEvents GotFocus="function(s,e){cddl_AmountAre.ShowDropDown();}" />
                                            <%-- LostFocus="function(s, e) { SetFocusonDemand(e)}"--%>
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-2  hide" style="margin-bottom: 15px">
                                        <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" Width="100%" OnCallback="ddl_VatGstCst_Callback">
                                            <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" GotFocus="function(s,e){cddlVatGstCst.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                                        </dxe:ASPxLabel>
                                        <%--<span style="color: red;">*</span>--%>
                                        <asp:TextBox ID="txtEWayBillNumber" runat="server" Width="100%" MaxLength="20">                             
                                        </asp:TextBox>
                                        <%-- <span id="MandatoryEWayBillNumber" class="EWayBillNumber  pullleftClass fa fa-exclamation-circle iconRed " 
                                            style="color: red; position: absolute; display: none" title="Mandatory"></span> --%>
                                    </div>

                                    <div class="col-md-6 lblmTop8">
                                        <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Remarks">
                                        </dxe:ASPxLabel>
                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="500"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2 lblmTop8" style="margin-bottom: 15px">

                                        <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Vendor Type">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_vendortype" runat="server" ClientInstanceName="cddl_vendortype" Width="100%" ClientEnabled="false">
                                            <Items>
                                                <dxe:ListEditItem Text="None" Value="R" />
                                                <dxe:ListEditItem Text="Composite" Value="C" />
                                                <dxe:ListEditItem Text="Import" Value="I" />
                                            </Items>
                                        </dxe:ASPxComboBox>

                                    </div>
                                    <div class="col-md-2 lblmTop8" style="margin-bottom: 15px">
                                        <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Entry Date">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxDateEdit ID="dt_EntryDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EntryDate" Width="100%" ClientEnabled="false">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                        <%--Rev 1.0--%>
                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon" />
                                        <%--Rev end 1.0--%>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Place Of Supply[GST]">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <dxe:ASPxComboBox ID="ddlPosGstInvoice" runat="server" ClientInstanceName="cddlPosGstInvoice" Width="100%" ValueField="System.String">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateInvoicePosGst(e)}" />
                                        </dxe:ASPxComboBox>
                                    </div>

                                    <div class="col-md-3 hide" id="divOverheadCost" runat="server" data-toggle="tooltip" data-placement="top" title="">
                                        <label id="Label27" runat="server">Overhead Cost</label>
                                        <dxe:ASPxCallbackPanel runat="server" ID="PanelGRNOverheadCost" ClientInstanceName="cPanelGRNOverheadCost" OnCallback="PanelGRNOverheadCost_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <dxe:ASPxGridLookup ID="lookup_GRNOverhead" SelectionMode="Multiple" AllowUserInput="false" runat="server" ClientInstanceName="clookup_GRNOverhead"
                                                        KeyFieldName="PurchaseChallan_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", "
                                                        OnDataBinding="lookup_GRNOverhead_DataBinding">
                                                        <%--DataSourceID="EntityServerModeDataOverheadCost"--%>
                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                            <dxe:GridViewDataColumn FieldName="PurchaseChallan_Number" Visible="true" VisibleIndex="1" Caption="GRN" Settings-AutoFilterCondition="Contains" Width="150" />
                                                            <%--<dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Posting Date" Settings-AutoFilterCondition="Contains" Width="120" />--%>
                                                            <dxe:GridViewDataColumn FieldName="cnt_firstName" Visible="true" VisibleIndex="3" Caption="Vendor Name" Settings-AutoFilterCondition="Contains" Width="150" />
                                                            <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="4" Caption="Unit" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataTextColumn Caption="Total Amount" FieldName="Challan_TotalAmount" Width="150" VisibleIndex="6">
                                                                <PropertiesTextEdit>
                                                                    <MaskSettings Mask="<0..999999999999>.<0..9999>" AllowMouseWheel="false" />
                                                                </PropertiesTextEdit>
                                                            </dxe:GridViewDataTextColumn>
                                                        </Columns>
                                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                            <Templates>
                                                                <StatusBar>
                                                                    <table class="OptionsTable" style="float: right">
                                                                        <tr>
                                                                            <td>

                                                                                <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />--%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </StatusBar>
                                                            </Templates>
                                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                            <SettingsPager Mode="ShowPager">
                                                                <%-- <PageSizeItemSettings Items="10,20,30"></PageSizeItemSettings>--%>
                                                            </SettingsPager>
                                                            <%--<SettingsPager Mode="ShowAllRecords">
                                                </SettingsPager>--%>
                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                        </GridViewProperties>
                                                        <%--<ClientSideEvents GotFocus="OverheadCost_gotFocus"  />--%>

                                                        <%--<ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}" />--%>
                                                        <%-- GotFocus="DisableDeleteOption"--%>
                                                        <ClientSideEvents LostFocus="function(s, e) { OverHeadcomponentEndCallBack();}" />
                                                    </dxe:ASPxGridLookup>
                                                    <%--GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}"--%>
                                                </dxe:PanelContent>
                                            </PanelCollection>

                                        </dxe:ASPxCallbackPanel>
                                        <%--  <dx:LinqServerModeDataSource ID="EntityServerModeDataOverheadCost" runat="server" OnSelecting="EntityServerModeDataOverheadCost_Selecting"
                                            ContextTypeName="ERPDataClassesDataContext" TableName="v_OverHeadCostPurchaseServiceInvoice" />--%>
                                    </div>

                                    <div class="col-md-2">
                                        <label id="lblProject" runat="server">Project</label>
                                        <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataPInvoice"
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
                                            <ClientSideEvents GotFocus="Project_gotFocus" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />

                                            <ClearButton DisplayMode="Always">
                                            </ClearButton>
                                        </dxe:ASPxGridLookup>
                                        <dx:LinqServerModeDataSource ID="EntityServerModeDataPInvoice" runat="server" OnSelecting="EntityServerModeDataPInvoice_Selecting"
                                            ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                    </div>

                                    <div class="col-md-4">
                                        <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                        </asp:DropDownList>
                                    </div>


                                    <div style="clear: both;"></div>
                                    <div class="col-md-12 relative" style="margin-top: 5px">
                                        <div class="makeFullscreen ">
                                            <span class="fullScreenTitle">Add Purchase Invoice</span>
                                            <span class="makeFullscreen-icon half hovered " data-instance="grid" title="Maximize Grid" id="expandgrid">
                                                <i class="fa fa-expand"></i>
                                            </span>
                                            <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="PurchaseInvoiceDetailID"
                                                ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                                OnCellEditorInitialize="grid_CellEditorInitialize" OnHtmlRowPrepared="grid_HtmlRowPrepared"
                                                Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                                                OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
                                                Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="150" RowHeight="2" OnDataBound="grid_DataBound"
                                                OnCustomColumnDisplayText="grid_CustomColumnDisplayText" Settings-HorizontalScrollBarMode="Auto">

                                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>

                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="0"
                                                        Caption="">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Batch Product Popup Start--%>
                                                    <%--<dxe:GridViewDataTextColumn Caption="Indent" FieldName="Indent_Num" ReadOnly="True" Width="80" VisibleIndex="2">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentNumber" Caption="Document No." VisibleIndex="2" ReadOnly="True" Width="130px">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="250px" ReadOnly="True">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" LostFocus="ProductsGotFocus" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width=".5%">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <%--Batch Product Popup End--%>
                                                    <%-- <dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="ProductID" VisibleIndex="2" Width="15%">
                                                        <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName"  EnableCallbackMode="true" CallbackPageSize="100">
                                                             <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsGotFocus" />
                                                        </PropertiesComboBox>
                                                          <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4"  Width="200"  >
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                         <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4" ReadOnly="True" Width="240px">
                                                        <CellStyle Wrap="True"></CellStyle>

                                                    </dxe:GridViewDataTextColumn>




                                                    <dxe:GridViewCommandColumn VisibleIndex="5" Caption="Addl. Desc." Width="150px">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc" Image-Url="/assests/images/more.png" Image-ToolTip="Warehouse">
                                                                <Image ToolTip="Warehouse" Url="/assests/images/more.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="6" Width="80px"
                                                        HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityGotFocus" />
                                                            <%-- LostFocus="QuantityTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM(Purc.)" VisibleIndex="7" Width="80px" ReadOnly="true">
                                                        <PropertiesTextEdit>
                                                            <%-- <ClientSideEvents LostFocus="QuantityTextChange" />--%>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewCommandColumn VisibleIndex="8" Caption="Multi UOM" Width="80px">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <%--Mantis Issue 24429--%>
                                                    <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="InvoiceDetails_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="9" Width="100px" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="InvoiceDetails_AltUOM" ReadOnly="true" VisibleIndex="10" Width="100px">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--VisibleIndex changed for below columns--%>
                                                    <%--End of Mantis Issue 24429--%>

                                                    <dxe:GridViewCommandColumn Width="110px" VisibleIndex="11" Caption="Stk Details">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="PurchasePrice" Caption="Purc. Price" VisibleIndex="12" Width="120px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <%-- <MaskSettings Mask="&lt;0..999999999g&gt;.&lt;00..99&gt;" AllowMouseWheel="false"/>--%>
                                                            <ClientSideEvents LostFocus="PurchasePriceTextChange" GotFocus="PurPriceGotFocus" />
                                                            <%--LostFocus="QuantityTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="13" Width="80px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountGotChange" />
                                                            <%--LostFocus="DiscountTextChange" GotFocus="DiscountGotChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%-- Discount in Amt Section Start By Sam on 17052017--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Discountamt" Caption="Disc Amt" VisibleIndex="14" Width="100px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="DiscountAmtTextChange" GotFocus="DiscountAmtGotChange" />
                                                            <%--LostFocus="DiscountAmtTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- Discount in Amt Section Start By Sam on 17052017--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="15" Width="100px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <%-- <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>
                                                            <ClientSideEvents LostFocus="AmtTextChange" GotFocus="AmtGotFocus" />
                                                            <%-- LostFocus="AmtTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>

                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- <dxe:GridViewDataTextColumn FieldName="TaxAmount" Caption="Tax Amount" VisibleIndex="12" Width="6%">
                                                        <PropertiesTextEdit>
                                                              <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="16" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="True">

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
                                                    <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="17" Width="100px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <%--<MaskSettings Mask="&lt;0..9999999999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="18" Width="150px" ReadOnly="false">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Left">

                                                            <Style HorizontalAlign="Left">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>



                                                    <%--                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="5%" VisibleIndex="12" Caption="Add New">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewTDS" Image-Url="/assests/images/add.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>--%>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="150px" VisibleIndex="19" Caption="Add New">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Component ID" VisibleIndex="20" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentDetailID" Caption="ComponentDetailID" VisibleIndex="21" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="22" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="24" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="25" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="DetailsId" Caption="Details ID" VisibleIndex="26" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="UOMNAME" PropertiesTextEdit-ValidationSettings-ErrorImage-IconID="ghg" Caption="hidden Field Id" VisibleIndex="27" ReadOnly="True" Width="0" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="abcd">
                                                        <CellStyle Wrap="True" CssClass="abcd"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <%-- Rev Mantis Issue 24061 --%>
                                                    <dxe:GridViewDataTextColumn FieldName="Balance_Amount" Caption="Balance Amount" VisibleIndex="29" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- End of Rev Mantis Issue 24061 --%>

                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" PropertiesTextEdit-ValidationSettings-ErrorImage-IconID="ghg" 
                                                        Caption="hidden Field Id" 
                                                        VisibleIndex="30" ReadOnly="True" Width="0" 
                                                         PropertiesTextEdit-Style-CssClass="abcd">
                                                       <%-- <CellStyle Wrap="True" CssClass="abcd"></CellStyle>--%>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%-- <dxe:GridViewDataTextColumn Caption="Quotation No" FieldName="Indent" Width="0"  VisibleIndex="13">
                                                <PropertiesTextEdit >
                                                    <NullTextStyle ></NullTextStyle>
                                                    <ReadOnlyStyle ></ReadOnlyStyle>
                                                    <Style></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle  />
                                                <CellStyle >
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn> 
                                                     <dxe:GridViewDataTextColumn FieldName="gvColStockQty" Caption="Stock Qty"   Width="0" >
                                                        <PropertiesTextEdit >
                                                            <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                            <NullTextStyle ></NullTextStyle>
                                                        <ReadOnlyStyle ></ReadOnlyStyle>
                                                        <Style ></Style>
                                                        </PropertiesTextEdit>
                                                         <HeaderStyle  />
                                                    <CellStyle >
                                                    </CellStyle>
                                                          <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                   <dxe:GridViewDataTextColumn FieldName="gvColStockUOM" Caption="Stk UOM"  
                                                       width="0">
                                                        <PropertiesTextEdit >
                                                             <NullTextStyle ></NullTextStyle>
                                                             <ReadOnlyStyle ></ReadOnlyStyle>
                                                             <Style ></Style>
                                                        </PropertiesTextEdit>
                                                       <HeaderStyle  />
                                                        <CellStyle > </CellStyle>
                                                   
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    <%--BatchEditStartEditing="OnBatchStartEdit"--%>
                                                </Columns>
                                                <%--      Init="OnInit"BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing"
                                                    CustomButtonClick="OnCustomButtonClick" EndCallback="OnEndCallback" --%>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex"
                                                    BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                            </dxe:ASPxGridView>
                                        </div>
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
                                    <div class="col-md-12 pt-10">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                                    <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveNewRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                                    </dxe:ASPxButton>
                                                    <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                                    </dxe:ASPxButton>
                                                    <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                                    </dxe:ASPxButton>
                                                    <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="T&#818;axes" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                                    </dxe:ASPxButton>

                                                    <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_specialedit" runat="server" AutoPostBack="False" Text="Special Edit" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False" Visible="false">
                                                        <ClientSideEvents Click="function(s, e) {specialedit_ButtonClick();}" />
                                                    </dxe:ASPxButton>
                                                    <span id="divTCS" runat="server">
                                                        <dxe:ASPxButton ID="ASPxButton10" ClientInstanceName="cbtn_TCS" runat="server" AutoPostBack="False" Text="Add TC&#818;S" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {ShowTCS();}" />
                                                        </dxe:ASPxButton>
                                                        <dxe:ASPxButton ID="ASPxButton11" ClientInstanceName="cbtn_TDS" runat="server" AutoPostBack="False" Text="Add TD&#818;S" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {ShowTDS();}" />
                                                        </dxe:ASPxButton>
                                                    </span>

                                                    <asp:HiddenField ID="hfControlData" runat="server" />
                                                    <asp:HiddenField ID="hdnPBTaggedYorN" runat="server" />
                                                    <asp:HiddenField ID="hdnTaxDeleteByShippingStateMismatch" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hdnRCMChecked" runat="server" />
                                                    <span id="spVehTC">
                                                        <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                                        <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />

                                                    </span>
                                                    <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                                                    <span id="spVendorImport">
                                                        <%-- <uc1:ucImportPurchase_BillOfEntry runat="server" ID="ucImportPurchase_BillOfEntry" />--%>
                                                    </span>

                                                    <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                                    <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="PB" />

                                                    <asp:HiddenField runat="server" ID="hdnVendorImport" Value="PB" />
                                                    <asp:HiddenField runat="server" ID="hdnPBAutoPrint" Value="" />
                                                    <asp:HiddenField runat="server" ID="hdnNoofCopies" Value="" />

                                                    <%--<asp:HiddenField ID="hdnqtyupdate" runat="server" Value="N" />--%>

                                                    <%-- <asp:HiddenField ID="hdndelcnt" runat="server" Value="N" />--%>

                                                    <asp:HiddenField ID="hdnTaggedVender" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnTaggedVendorName" runat="server" Value="" />

                                                    <%--Tax Related Hiddenfield--%>
                                                    <%--<asp:HiddenField ID="hdntaxqty" runat="server" Value="0" />--%>
                                                    <%--<asp:HiddenField ID="hdntaxpurprice" runat="server" Value="0" />--%>
                                                    <%--<asp:HiddenField ID="hdntaxdisc" runat="server" Value="0" />--%>
                                                    <%--<asp:HiddenField ID="hdntaxdiscamt" runat="server" Value="0" />--%>
                                                    <%-- <asp:HiddenField ID="hdntaxamt" runat="server" Value="0" />--%>

                                                    <asp:HiddenField ID="hdnTDSShoworNot" runat="server" Value="N" />

                                                    <asp:HiddenField ID="hdnOverHeadCostShoworNot" runat="server" Value="N" />

                                                    <%--<asp:HiddenField ID="hdnrunningtaxbleAmt" runat="server" Value="" />--%>
                                                    <%--<asp:HiddenField ID="hdnRunningTaxAmt" runat="server" Value="" />--%>
                                                    <%--Tax Related Hiddenfield--%>

                                                </td>
                                                <td class="sendMailCheckbox ">
                                                    <ul class="ks-cboxtags">
                                                        <li>
                                                            <asp:CheckBox ID="chkmail" runat="server" Text="Send Mail" />
                                                        </li>
                                                    </ul>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                </div>
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    <dxe:TabPage Name="Billing/Shipping" Text="Our Billing/Shipping">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">

                                <ucBS:Purchase_BillingShipping runat="server" ID="Purchase_BillingShipping" />
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
        <%--InlineTax--%>
    </div>
    <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
        Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <span style="color: #fff"><strong>Select Tax</strong></span>
            <dxe:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
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
                <asp:HiddenField runat="server" ID="hdnInvWiseSlno" />
                <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
                <asp:HiddenField runat="server" ID="HdProdNetAmt" />
                <%-- <asp:HiddenField ID="hdnPageStatus1" runat="server" />--%>
                <%-- Added by Sam to show default cursor after save--%>
                <%-- <asp:HiddenField ID="hdnschemeid" runat="server" />--%>
                <asp:HiddenField ID="hdnDeleteSrlNo" runat="server" />
                <asp:HiddenField ID="hdnADDEditMode" runat="server" />
                <%--<asp:HiddenField ID="hdnprevqty" runat="server" />--%>
                <%-- Added by Sam to show default cursor after save--%>
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
                                        <td>Amount
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
                            <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" ReadOnly="true"
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
                                        <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
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
                            <table class="InlineTaxClass hide">
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

                                            <%--<ClientSideEvents SelectedIndexChanged="cmbGstCstVatChange"
                                                    GotFocus="CmbtaxClick" />--%>
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

                                <dxe:ASPxButton ID="Button1" ClientInstanceName="cButton1" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {return BatchUpdate();}" />
                                </dxe:ASPxButton>

                                <dxe:ASPxButton ID="Button3" ClientInstanceName="cButton3" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;}" />
                                </dxe:ASPxButton>
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

    <%--ChargesTax--%>
    <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
        Width="900px" Height="300px" HeaderText="Purchase Invoice Taxes" PopupHorizontalAlign="WindowCenter"
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
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                    <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.000">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
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
                            <Styles>
                                <StatusBar CssClass="statusBar">
                                </StatusBar>
                            </Styles>
                        </dxe:ASPxGridView>
                    </div>
                    <div class="col-md-12">
                        <table style="" class="chargesDDownTaxClass">
                            <tr class="chargeGstCstvatClass">
                                <td style="padding-top: 10px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                <td style="padding-top: 10px; width: 200px;">
                                    <dxe:ASPxComboBox ID="cmbGstCstVatcharge" ClientInstanceName="ccmbGstCstVatcharge" runat="server" SelectedIndex="0"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                        OnCallback="cmbGstCstVatcharge_Callback">
                                        <Columns>
                                            <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                            <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />

                                        </Columns>
                                        <%-- <ClientSideEvents SelectedIndexChanged="ChargecmbGstCstVatChange"
                                                GotFocus="chargeCmbtaxClick" />--%>
                                    </dxe:ASPxComboBox>



                                </td>
                                <td style="padding-left: 15px; padding-top: 10px; width: 200px;">
                                    <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" ClientInstanceName="ctxtGstCstVatCharge" ReadOnly="true" Text="0.00"
                                        runat="server" Width="100%">

                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        <%--<MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />--%>
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
                            <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AccessKey="X" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                            </dxe:ASPxButton>
                            <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" UseSubmitBehavior="False">
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
    <%--   Inline Tax End    --%>

    <%--   Warehouse     --%>
    <dxe:ASPxPopupControl ID="ASPxPopupControl3" runat="server" ClientInstanceName="cPopup_WarehousePC"
        Width="900px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter" Height="500px"
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
                                            <asp:Label ID="lblbranchName" runat="server"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="col-md-3">
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
                                    <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtnaddWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary pull-left" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveWarehouse();}" />
                                    </dxe:ASPxButton>

                                    <dxe:ASPxButton ID="ASPxButton6" ClientInstanceName="cbtnrefreshWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Clear Entries" CssClass="btn btn-primary pull-left" UseSubmitBehavior="False">
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
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="S&#818;ave & Exit" AccessKey="S" CssClass="btn btn-primary" UseSubmitBehavior="False">
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
    <div id="hdnFieldWareHouse">
        <asp:HiddenField ID="hdfProductIDPC" runat="server" />
        <asp:HiddenField ID="hdfstockidPC" runat="server" />
        <asp:HiddenField ID="hdfopeningstockPC" runat="server" />
        <asp:HiddenField ID="hdbranchIDPC" runat="server" />
        <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />
        <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
        <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
        <asp:HiddenField ID="hdnProductQuantity" runat="server" />

        <asp:HiddenField ID="hdniswarehouse" runat="server" />
        <asp:HiddenField ID="hdnisbatch" runat="server" />
        <asp:HiddenField ID="hdnisserial" runat="server" />
        <asp:HiddenField ID="hdndefaultID" runat="server" />

        <asp:HiddenField ID="hdnoldrowcount" runat="server" Value="0" />

        <asp:HiddenField ID="hdntotalqntyPC" runat="server" Value="0" />

        <%-- Sam New Modification For Qty Checking--%>
        <asp:HiddenField ID="wbsqtychecking" runat="server" Value="1" />
        <%--<asp:HiddenField ID="producttype" runat="server" Value="" />--%>
        <%--Sam New Modification For Qty Checking--%>
        <asp:HiddenField ID="hdnoldwarehousname" runat="server" />
        <asp:HiddenField ID="hdnoldbatchno" runat="server" />
        <asp:HiddenField ID="hidencountforserial" runat="server" />
        <asp:HiddenField ID="hdnbatchchanged" runat="server" Value="0" />

        <asp:HiddenField ID="hdnrate" runat="server" Value="0" />
        <asp:HiddenField ID="hdnvalue" runat="server" Value="0" />

        <%--<asp:HiddenField ID="oldhdnoldwarehousname" runat="server" Value="0" />--%>

        <%--<asp:HiddenField ID="oldhidencountforserial" runat="server" Value="0" />--%>
        <%--<asp:HiddenField ID="oldhdnbatchchanged" runat="server" Value="0" />--%>
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

        <%--<asp:HiddenField ID="hdnisreduing" runat="server" Value="false" />--%>
        <%-- <asp:HiddenField ID="hdnoutstock" runat="server" Value="0" />--%>

        <asp:HiddenField ID="hdnpcslno" runat="server" Value="0" />
    </div>

    <%--   Warehouse End    --%>

    <%-- HiddenField --%>
    <div>
        <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
        <asp:HiddenField ID="hdfIsDelete" runat="server" />
        <asp:HiddenField ID="hdfIsComp" runat="server" />
        <asp:HiddenField ID="hdnPageStatus" runat="server" />
        <asp:HiddenField ID="hdfProductID" runat="server" />
        <asp:HiddenField ID="hdfProductType" runat="server" />
        <asp:HiddenField ID="hdfProductSerialID" runat="server" />
        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="hdnCustomerId" runat="server" />
        <%--Mantis Issue 24432--%>
        <asp:HiddenField ID="hdnPageEditId" runat="server" />
        <%--End of Mantis Issue 24432--%>

        <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
        <%--added by sam to delete the noninventory item and its session detail from grid--%>

        <asp:HiddenField ID="hdinvetorttype" runat="server" />
        <%-- added by sam to delete the noninventory item and its session detail from grid--%>
    </div>
    <%-- HiddenField End--%>
    <%--UDF--%>
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
    <%--End UDF--%>
    <%--Batch Product Popup Start--%>

    <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
        Width="1000" HeaderText="Select Product " AllowResize="true" ResizingMode="Postponed" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By Product Name (4 Char)</strong></label>
                <%--  DataSourceID="ProductDataSource"--%>
                <dxe:ASPxCallbackPanel runat="server" ID="productPanel" ClientInstanceName="cproductPanel" OnCallback="productPanel_Callback">
                    <PanelCollection>

                        <dxe:PanelContent runat="server">

                            <dxe:ASPxComboBox ID="productLookUp" runat="server" EnableCallbackMode="true" CallbackPageSize="5"
                                ValueType="System.String" ValueField="Products_ID" ClientInstanceName="cproductLookUp" Width="92%"
                                DropDownStyle="DropDown" DropDownRows="5" ItemStyle-Wrap="True">
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

                            <%--<dxe:ASPxGridLookup ID="productLookUp" runat="server" ClientInstanceName="cproductLookUp" OnDataBinding="productLookUp_DataBinding"
                        KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", "
                         ClientSideEvents-TextChanged="ProductSelected" 
                        ClientSideEvents-KeyDown="ProductlookUpKeyDown">
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
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>



    <%--Batch Product Popup End--%>
    <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
    </dxe:ASPxCallbackPanel>

    <%--Div Detail for Vendor Section Start--%>

    <dxe:ASPxCallbackPanel runat="server" ID="acpContactPersonPhone" ClientInstanceName="cacpContactPersonPhone" OnCallback="acpContactPersonPhone_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acpContactPersonPhoneEndCall" />
    </dxe:ASPxCallbackPanel>
    <%--Div Detail for Vendor Section Start--%>



    <asp:HiddenField runat="server" ID="hdngridvselectedrowno" />
    <asp:SqlDataSource ID="SqlSchematype" runat="server"
        SelectCommand="Select * From ((Select '0' as ID,' Select' as SchemaName) Union (Select  convert(nvarchar(10),ID)+'~'+convert(nvarchar(10),b.branch_id) as ID,SchemaName+'('+b.branch_description +')'as SchemaName  From tbl_master_Idschema  join tbl_master_branch b on tbl_master_Idschema.Branch=b.branch_id  Where TYPE_ID='19' and IsActive=1
                    and financial_year_id IN(select FinYear_ID from Master_FinYear where FinYear_Code=@year) 
                    and Isnull(Branch,'') in (select s FROM dbo.GetSplit(',',@userbranchHierarchy)) and comapanyInt=@company)) as X Order By SchemaName ASC">


        <SelectParameters>

            <asp:SessionParameter Name="userbranchHierarchy" SessionField="userbranchHierarchy" Type="string" />
            <asp:SessionParameter Name="company" SessionField="LastCompany" Type="string" />
            <asp:SessionParameter Name="year" SessionField="LastFinYear" Type="string" />
            <%-- <asp:SessionParameter Name="year" SessionField="LastFinYear1" Type="string" />--%>
            <%-- <asp:SessionParameter Name="company" SessionField="LastCompany1" Type="string" />--%>
        </SelectParameters>
    </asp:SqlDataSource>


    <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
    </dxe:ASPxCallbackPanel>



    <dxe:ASPxPopupControl ID="inventorypopup" runat="server" ClientInstanceName="cinventorypopup"
        Width="1080px" HeaderText="Select TDS" PopupHorizontalAlign="WindowCenter" ShowCloseButton="false"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <asp:HiddenField runat="server" ID="hdn_tdsedit" Value="0" />





                <div class="row">
                    <div class="col-md-3">
                        <label><span><strong>Select Unit</strong></span></label>
                        <div>
                            <asp:Label ID="lbltdsBranch" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label><span><strong>Edit TDS</strong></span></label>
                        <div>
                            <dxe:ASPxCheckBox ID="chk_TDSEditable" ClientInstanceName="cchk_TDSEditable" runat="server">
                                <ClientSideEvents CheckedChanged="TDSEditableCheckChanged" />
                            </dxe:ASPxCheckBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label><span><strong>Select Month for TDS</strong></span></label>
                        <dxe:ASPxComboBox ID="ddl_month" ClientInstanceName="cddl_month" runat="server" SelectedIndex="-1"
                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                            ClearButton-DisplayMode="Always">
                            <Items>
                                <dxe:ListEditItem Text="April" Value="April" />
                                <dxe:ListEditItem Text="May" Value="May" />
                                <dxe:ListEditItem Text="June" Value="June" />
                                <dxe:ListEditItem Text="July" Value="July" />
                                <dxe:ListEditItem Text="August" Value="August" />
                                <dxe:ListEditItem Text="September" Value="September" />
                                <dxe:ListEditItem Text="October" Value="October" />
                                <dxe:ListEditItem Text="November" Value="November" />
                                <dxe:ListEditItem Text="December" Value="December" />
                                <dxe:ListEditItem Text="January" Value="January" />
                                <dxe:ListEditItem Text="February" Value="February" />
                                <dxe:ListEditItem Text="March" Value="March" />
                            </Items>
                        </dxe:ASPxComboBox>
                    </div>

                    <div class="col-md-3 ">
                        <label><span><strong>Product Basic Amount</strong></span></label>
                        <div style="padding-bottom: 5px">
                            <dxe:ASPxTextBox ID="txt_proamt" MaxLength="80" ClientInstanceName="ctxt_proamt" ReadOnly="true" DisplayFormatString="0.00"
                                runat="server" Width="50%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>
                <table style="width: 100%;">

                    <tr>
                        <td colspan="4">
                            <dxe:ASPxGridView runat="server" KeyFieldName="TDSID" ClientInstanceName="cgridinventory" ID="gridinventory"
                                Width="100%" SettingsBehavior-AllowSort="false" OnBatchUpdate="gridinventory_BatchUpdate" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                OnCustomCallback="gridinventory_CustomCallback"
                                Settings-ShowFooter="false" AutoGenerateColumns="False" OnRowUpdating="gridinventory_RowUpdating" OnRowInserting="gridinventory_RowInserting">
                                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                <SettingsPager Visible="false"></SettingsPager>
                                <Columns>

                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="TDSRate" Caption="TDS Rate(%)" Width="8%">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="TDS amount" FieldName="TDSAmount" VisibleIndex="3" Width="8%">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="TDSAmtLostFocus" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="SurchargeRate" Caption="Surcharge Rate(%)" Width="11%">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Surcharge amount" FieldName="SurchargeAmount" VisibleIndex="5" Width="11%">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="SurchargeAmountLostFocus" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="EducationCessRate" Caption="Education Cess Rate(%)" Width="14%">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Education Cess Amount" FieldName="EducationCessAmt" VisibleIndex="7" Width="14%">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="EducationCessAmtLostFocus" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="HgrEducationCessRate" Caption="Higher Education Cess Rate(%)" Width="17%">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Higher Education Cess Amount" FieldName="HgrEducationCessAmt" VisibleIndex="9" Width="17%">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="HgrEducationCessAmtLostFocus" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>




                                </Columns>
                                <Styles>
                                    <StatusBar CssClass="statusBar"></StatusBar>
                                </Styles>
                                <ClientSideEvents EndCallback="OnInventoryEndCallback" />
                                <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
                                <%-- <SettingsDataSecurity AllowEdit="true" />--%>
                                <%--<SettingsEditing Mode="Batch">
                                            <BatchEditSettings EditMode="row" />
                                        </SettingsEditing>--%>
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                </SettingsEditing>
                            </dxe:ASPxGridView>

                        </td>
                    </tr>

                    <tr>
                        <td colspan="4">
                            <div class="pull-left">

                                <dxe:ASPxButton ID="btn_noninventoryOk" ClientInstanceName="cbtn_noninventoryOk" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {return NonInventoryBatchUpdate();}" />
                                </dxe:ASPxButton>

                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-top: 10px; padding-right: 5px"><strong>Total TDS</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txt_totalnoninventoryproductamt" MaxLength="80" ClientInstanceName="ctxt_totalnoninventoryproductamt" DisplayFormatString="0.00"
                                            ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <%-- <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..00&gt;" AllowMouseWheel="false" />--%>
                                            <%--<MaskSettings Mask="<-999999999..999999999>.<0..00>" AllowMouseWheel="false" />--%>
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
    <asp:HiddenField ID="hdntdschecking" runat="server" />
    <%--Inventory Section By Sam End on 15052017 --%>

    <dxe:ASPxCallbackPanel runat="server" ID="ApplicableAmtPopup" ClientInstanceName="CApplicableAmtPopup" OnCallback="ApplicableAmtPopup_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <%-- <clientsideevents endcallback="panelEndCallBack" />--%>
    </dxe:ASPxCallbackPanel>
    </div>
    </div>
    <%-- new Modified Hidden Tax Field--%>
    <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
    <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
    <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
    <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
    <%--  new Modified Hidden Tax Field--%>
    <%--Rev 1.0 Subhra 15-03-2019--%>
    <div>
        <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
        <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
    </div>
    <%--End of Rev 1.0 Subhra 15-03-2019--%>

    <asp:HiddenField ID="hdnManual" runat="server" Value="" />
    <asp:HiddenField ID="hdnAuto" runat="server" Value="" />
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <asp:SqlDataSource ID="VendorDataSource" runat="server" />


    <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="650px"
        Width="1020px" HeaderText="Add New Vendor" Modal="true" AllowResize="false" ResizingMode="Postponed">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <!--Vendor Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Vendor Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Vendor Name or Unique Id" />
                    <div id="CustomerTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Vendor Name</th>
                                <th>Unique Id</th>
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
    <!--Vendor Modal -->

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
                    <input type="text" onkeydown="Purchaseprodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Product Description</th>
                                <th>HSN/SAC</th>
                                <th>GST Rate</th>
                                <th>Class</th>
                                <th>Brand</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <% if (rightsProd.CanAdd)
                        { %>
                    <button type="button" class="btn btn-success btn-radius" onclick="fn_PopOpen();">
                        <span class="btn-icon"><i class="fa fa-plus"></i></span>
                        Add New
                    </button>
                    <% } %>
                    <button type="button" class="btn btn-danger btn-radius" data-dismiss="modal">
                        <%--<span class="btn-icon"><i class="fa fa-file" ></i></span>--%>
                        Close 
                    </button>
                    <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>

        </div>
    </div>
    <%---------------------------------------------------------------------%>

    <dxe:ASPxPopupControl ID="Popup_NoofCopies" runat="server" ClientInstanceName="cPopup_NoofCopies"
        Width="200px" HeaderText="Print Copies" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div class="Top clearfix">
                    <div class="row">
                        <div class="col-md-10 col-md-offset-1 relative">
                            <label>No. of Copies:</label>
                            <div>
                                <asp:DropDownList ID="ddlnoofcopies" runat="server" Width="80px">
                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                </asp:DropDownList>
                                <input id="btnSave" class="btn btn-primary" onclick="Call_OK()" type="button" style="margin-top: -3px" value="Ok" />
                            </div>
                        </div>
                        <div class="col-md-10 col-md-offset-1 relative" style="padding-top: 8px">
                        </div>
                    </div>
                    <table>
                        <tr>
                            <td colspan="3" style="padding-left: 121px;"></td>
                        </tr>
                    </table>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top"></ContentStyle>

        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="hdn_party_inv_no" />
    <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />








    <%--Product Pop up Start--%>

    <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
        Width="1000px" HeaderText="Add/Modify products" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                <div class="ProductMainContaint">

                    <div class="row ">
                        <div class="col-md-12 ">
                            <div class="col-md-6 " style="padding: 0">
                                <div class="col-md-6">
                                    <div class="cityDiv" style="height: auto; margin-bottom: 5px;">
                                        <%--Code--%>
                                            Short Name (Unique)
                                           <%-- <asp:Label ID="LblCode" runat="server" Text="Short Name (Unique)" CssClass="newLbl"></asp:Label>--%><span style="color: red;"> *</span>

                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtPro_Code" MaxLength="80" ClientInstanceName="ctxtPro_Code"
                                            runat="server" Width="100%" CssClass="upper">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                            </ValidationSettings>
                                            <ClientSideEvents TextChanged="function(s,e){fn_ctxtPro_Name_TextChanged()}" />

                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="cityDiv" style="height: auto; margin-bottom: 5px;">
                                        Name<span style="color: red;">*</span>
                                        <%--<asp:Label ID="LblName" runat="server" Text="Name" CssClass="newLbl"></asp:Label>--%>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtPro_Name" ClientInstanceName="ctxtPro_Name" runat="server" MaxLength="100"
                                            Width="100%" CssClass="upper">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                            </ValidationSettings>
                                            <%--<ClientSideEvents TextChanged="function(s,e){fn_ctxtPro_Name_TextChanged()}" />--%>
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                                <%--place here--%>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Inventory Item--%>
                                        <asp:Label ID="Label2" runat="server" Text="Inventory Item?" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbIsInventory" ClientInstanceName="ccmbIsInventory" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <Items>
                                                <dxe:ListEditItem Text="Yes" Value="1" />
                                                <dxe:ListEditItem Text="No" Value="0" />
                                            </Items>

                                            <ClientSideEvents SelectedIndexChanged="isInventoryChanged" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label22" runat="server" Text="Service Item?" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbServiceItem" ClientInstanceName="ccmbServiceItem" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <Items>
                                                <dxe:ListEditItem Text="No" Value="0" />
                                                <dxe:ListEditItem Text="Yes" Value="1" />

                                            </Items>


                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Inventory Item--%>
                                        <asp:Label ID="Label14" runat="server" Text="Capital Goods?" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbIsCapitalGoods" ClientInstanceName="ccmbIsCapitalGoods" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                            <Items>
                                                <dxe:ListEditItem Text="Yes" Value="1" />
                                                <dxe:ListEditItem Text="No" Value="0" />
                                            </Items>

                                            <ClientSideEvents SelectedIndexChanged="isCapitalChanged" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="" style="height: auto;">
                                        <%--Description--%>
                                        <asp:Label ID="Label24" runat="server" Text="Alternate Name" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtPro_Printname" ClientInstanceName="ctxtPro_Printname" MaxLength="100"
                                            runat="server" Width="100%" CssClass="upper">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                            </div>




                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto; margin-top: -5px;">
                                    <%--Description--%>
                                    <asp:Label ID="LblDecs" runat="server" Text="Description" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxMemo ID="txtPro_Description" ClientInstanceName="ctxtPro_Description" MaxLength="300"
                                        runat="server" Width="100%" Height="60px" Text='<%# Bind("txtMarkets_Description") %>' CssClass="upper">
                                    </dxe:ASPxMemo>
                                </div>
                            </div>




                            <div class="clear"></div>
                        </div>
                        <div class="col-md-4" style="display: none">
                            <div class="">

                                <div class="imageArea" style="height: auto; margin-bottom: 5px;">
                                    <dxe:ASPxImage ID="ProdImage" runat="server" ClientInstanceName="cProdImage" CssClass="myImage">
                                    </dxe:ASPxImage>
                                </div>


                                <div class="Left_Content">
                                    <%--<dxe:ASPxCallbackPanel  ID="ASPxCallback1" runat="server" ClientInstanceName="Callback1" OnCallback="ASPxCallback1_Callback">
                                            <PanelCollection>
                                                  <dxe:PanelContent ID="PanelContent3" runat="server">
                                                         <button type="button" onclick="uploadClick()">Upload</button>
                                                       <asp:FileUpload ID="FileUpload1" runat="server"></asp:FileUpload>
                                                   
                                                      </dxe:PanelContent>
                                                </PanelCollection>
                                            </dxe:ASPxCallbackPanel>--%>

                                    <dxe:ASPxUploadControl ID="ASPxUploadControl1" runat="server" ClientInstanceName="upload1" OnFileUploadComplete="ASPxUploadControl1_FileUploadComplete"
                                        ShowProgressPanel="True" CssClass="pull-left">
                                        <ValidationSettings MaxFileSize="2194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" ErrorStyle-CssClass="validationMessage" />
                                        <ClientSideEvents FileUploadComplete="function(s, e) { OnUploadComplete(e); }" />
                                    </dxe:ASPxUploadControl>
                                    <dxe:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Text="Upload" ClientInstanceName="btnUpload" CssClass="pull-right btn btn-primary btn-small blll hide">
                                        <ClientSideEvents Click="function(s, e) {
                                                     upload1.Upload(); 
                                                    }"></ClientSideEvents>
                                    </dxe:ASPxButton>

                                    <asp:HiddenField runat="server" ID="fileName" />

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="Top">


                        <%--Product Image--%>


                        <%--Product Image--%>

                        <div class="boxarea clearfix">
                            <span class="boxareaH">Miscellaneous</span>


                            <%--<div class="clear"></div>--%>
                            <%--End of Inventory Type--%>

                            <div class="col-md-3">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Product Class Code--%>
                                    <asp:Label ID="LblPCcode" runat="server" Text="Class Name" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbProClassCode" ClientInstanceName="cCmbProClassCode" runat="server" SelectedIndex="0" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                        <ClearButton DisplayMode="Always"></ClearButton>
                                        <ClientSideEvents SelectedIndexChanged="CmbProClassCodeChanged" />
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Product Class Code--%>
                                    <asp:Label ID="Label3" runat="server" Text="Status" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbStatus" ClientInstanceName="cCmbStatus" runat="server" SelectedIndex="0"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                        <Items>
                                            <dxe:ListEditItem Text="Active" Value="A" />
                                            <dxe:ListEditItem Text="Dormant" Value="D" />
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Product Class Code--%>
                                    <asp:Label ID="Label7" runat="server" Text="HSN Code" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <div style="display: none">
                                        <dxe:ASPxTextBox ID="txtHsnCode" ClientInstanceName="ctxtHsnCode" MaxLength="10"
                                            runat="server" Width="100%" Text='<%# Bind("txtMarkets_Description") %>'>
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <%--<dxe:ASPxComboBox ID="aspxHsnCode" ClientInstanceName="caspxHsnCode" runat="server" Width="100%"  ValueType="System.String" AutoPostBack="false" 
                                          ItemStyle-Wrap="True" ClearButton-DisplayMode="Always"  EnableCallbackMode="true"  >                         
                                        </dxe:ASPxComboBox>--%>

                                    <dxe:ASPxCallbackPanel runat="server" ID="SetHSnPanel" ClientInstanceName="cSetHSnPanel" OnCallback="SetHSnPanel_Callback">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">






                                                <dxe:ASPxGridLookup ID="HsnLookUp" runat="server" DataSourceID="HsnDataSource" ClientInstanceName="cHsnLookUp"
                                                    KeyFieldName="Code" Width="100%" TextFormatString="{0}" MultiTextSeparator=", ">
                                                    <Columns>
                                                        <dxe:GridViewDataColumn FieldName="Code" Caption="Code" Width="50" />
                                                        <dxe:GridViewDataColumn FieldName="Description" Caption="Description" Width="350" />
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
                                        <ClientSideEvents EndCallback="SetHSnPanelEndCallBack" />
                                    </dxe:ASPxCallbackPanel>



                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="cityDiv" style="height: auto;">
                                    <asp:Label ID="Label21" runat="server" Text="Furtherance to Business" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxCheckBox ID="chkFurtherance" ClientInstanceName="cchkFurtherance" runat="server">
                                    </dxe:ASPxCheckBox>
                                </div>
                            </div>


                        </div>



                        <%--Bar Code type and bar code added by Debjyoti 30-12-2016--%>
                        <%--<div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    
                                    <asp:Label ID="lblBarCodeType" runat="server" Text="Barcode Type" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbBarCodeType" ClientInstanceName="cCmbBarCodeType" runat="server" SelectedIndex="0" TabIndex="6" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>

                             <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    
                                    <asp:Label ID="lblMpc" runat="server" Text="MPC No." CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxTextBox ID="txtMpcNo" ClientInstanceName="ctxtMpcNo" MaxLength="50" TabIndex="7"
                                        runat="server" Width="226px">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div style="clear: both"></div>--%>
                        <%--Bar Code type and bar code added by Debjyoti 30-12-2016--%>


                        <%--                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    
                                    <asp:Label ID="LblGlobalCode" runat="server" Text="Global Code(UPC)" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxTextBox ID="txtGlobalCode" ClientInstanceName="ctxtGlobalCode" MaxLength="30" TabIndex="8"
                                        runat="server" Width="226px" Text='<%# Bind("txtMarkets_Description") %>'>
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>--%>
                        <div class="clear"></div>
                        <div class="col-md-3" style="display: none">
                            <div class="cityDiv" style="height: auto;">
                                <%--Quote Currency--%>
                                <asp:Label ID="LblQCurrency" runat="server" Text="Quote Currency" CssClass="newLbl"></asp:Label>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbQuoteCurrency" ClientInstanceName="cCmbQuoteCurrency" runat="server" SelectedIndex="0" ClearButton-DisplayMode="Always"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                    <ClearButton DisplayMode="Always"></ClearButton>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>



                        <div class="col-md-3" style="display: none">
                            <div class="cityDiv lblmTop8" style="height: auto; margin-bottom: 5px;">
                                <span>UOM Factor <span style="color: red;">*</span></span>
                                <%--<asp:Label ID="LblQLot" runat="server" Text="Quote Lot" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="txtQuoteLot" ClientInstanceName="ctxtQuoteLot" MaxLength="8"
                                    runat="server" Width="100%" Text='<%# Bind("txtMarkets_Description") %>'>

                                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>

                        <div class="col-md-3" style="display: none">
                            <div class="cityDiv" style="height: auto;">
                                <%--Quote Lot Unit<span style="color:red;"> *</span>--%>
                                <span class="newLbl">Quote UOM<span style="color: red;"> *</span></span>
                                <%--<asp:Label ID="LblQLotUnit" runat="server" Text="Quote Lot unit" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbQuoteLotUnit" ClientInstanceName="cCmbQuoteLotUnit" runat="server" ClearButton-DisplayMode="Always"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    <ClearButton DisplayMode="Always"></ClearButton>

                                </dxe:ASPxComboBox>
                            </div>
                        </div>







                        <div class="col-md-3" style="display: none">
                            <div class="cityDiv" style="height: auto;">
                                <span class="newLbl">Sale UOM Factor<span style="color: red;"> *</span></span>
                                <%--<asp:Label ID="LblTradingLot" runat="server" Text="Trading Lot" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="txtTradingLot" ClientInstanceName="ctxtTradingLot" MaxLength="8"
                                    runat="server" Width="100%" Text='<%# Bind("txtMarkets_Description") %>'>

                                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>


                        <div class="col-md-6" style="padding: 0px !important;">

                            <div class="boxarea clearfix">
                                <span class="boxareaH">Sales</span>



                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Trading Lot Units--%>
                                        <%--<asp:Label ID="LblTLotUnit" runat="server" Text="Trading Lot Units" CssClass="newLbl"></asp:Label>--%>
                                        <span class="newLbl">Unit<span style="color: red;"> *</span></span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="CmbTradingLotUnits" ClientInstanceName="cCmbTradingLotUnits" runat="server" ClearButton-DisplayMode="Always"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                            </ValidationSettings>
                                            <ClearButton DisplayMode="Always"></ClearButton>
                                            <ClientSideEvents LostFocus="cCmbTradingLotUnitsLostFocus" SelectedIndexChanged="cCmbTradingLotUnitsLostFocus" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>

                                <%--Debjyoti Sale price & min sale price--%>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">Sell @</span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtSalePrice" ClientInstanceName="ctxtSalePrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">

                                            <MaskSettings Mask="<0..99999999>.<0..99>" />

                                            <%-- <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />--%>
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">Minimum Sell @</span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtMinSalePrice" ClientInstanceName="ctxtMinSalePrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />
                                            <%--<ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />--%>
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">MRP </span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtMrp" ClientInstanceName="ctxtMrp" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />
                                            <%-- <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />--%>
                                        </dxe:ASPxTextBox>
                                        <span id="mrpError" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -2px; top: 27px; display: none" title="Must be greater than Min Sale Price"></span>
                                    </div>
                                </div>

                            </div>

                        </div>

                        <%--End here--%>
                        <%--<div class="clear"></div>--%>
                        <div class="col-md-6" style="padding: 0px !important;">
                            <div class="boxarea clearfix">
                                <span class="boxareaH">Purchases</span>
                                <div class="col-md-3" style="display: none">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Purchase UOM <span style="color:red;"> *</span>--%>
                                        <span class="newLbl">Purchase UOM Factor<span style="color: red;"> *</span></span>
                                        <%--<asp:Label ID="LblDeliveryLot" runat="server" Text="Delivery Lot" CssClass="newLbl"></asp:Label>--%>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtDeliveryLot" ClientInstanceName="ctxtDeliveryLot" MaxLength="8"
                                            runat="server" Width="100%" Text='<%# Bind("txtMarkets_Description") %>'>

                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Delivery Lot Unit--%>
                                        <%--<asp:Label ID="LblDeliveryLotUnit" runat="server" Text="Delivery Lot Unit" CssClass="newLbl"></asp:Label>--%>

                                        <span class="newLbl">Unit<span style="color: red;"> *</span></span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="CmbDeliveryLotUnit" ClientInstanceName="cCmbDeliveryLotUnit" runat="server" ClearButton-DisplayMode="Always"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                            </ValidationSettings>
                                            <ClearButton DisplayMode="Always"></ClearButton>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>



                                <%--Debjyoti Purchase price & MRP--%>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">Buy @</span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtPurPrice" ClientInstanceName="ctxtPurPrice" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="clear"></div>
                        <div class="col-md-6" style="padding: 0px !important;">
                            <div class="boxarea clearfix">
                                <span class="boxareaH">Inventory</span>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">Min Level        </span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtMinLvl" ClientInstanceName="ctxtMinLvl" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>




                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <span class="newLbl pull-right">Max Level        </span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtMaxLvl" ClientInstanceName="ctxtMaxLvl" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>





                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label5" runat="server" Text="Reorder Level" CssClass="newLbl pull-right"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtReorderLvl" ClientInstanceName="ctxtReorderLvl" HorizontalAlign="Right" MaxLength="18" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                            <%--<MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />--%>
                                            <%--<MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />--%>
                                            <MaskSettings Mask="<0..99999999>.<0..99>" />

                                        </dxe:ASPxTextBox>
                                        <span id="reOrderError" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -6px; top: 29px; display: none" title="Must be greater than Min level"></span>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label23" runat="server" Text="Reorder Quantity" CssClass="newLbl pull-right"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtReorderQty" ClientInstanceName="ctxtReorderQty" MaxLength="18" HorizontalAlign="Right" DisplayFormatString="{0:0.00}"
                                            runat="server" Width="100%">
                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                            <%--<MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />--%>
                                            <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" />
                                        </dxe:ASPxTextBox>
                                        <span id="reOrderQuantityError" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -6px; top: 29px; display: none" title="Must be greater than Min level"></span>
                                    </div>
                                </div>




                                <div class="clear"></div>




                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label6" runat="server" Text="Negative Stock" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbNegativeStk" ClientInstanceName="ccmbNegativeStk" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <Items>
                                                <dxe:ListEditItem Text="Warn" Value="W" />
                                                <dxe:ListEditItem Text="Ignore" Value="I" />
                                                <dxe:ListEditItem Text="Block" Value="B" />
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>




                                <%--Debjyoti Add Inventory Type--%>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Type--%>
                                        <asp:Label ID="LblType" runat="server" Text="Type" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="CmbProType" ClientInstanceName="cCmbProType" runat="server" ClearButton-DisplayMode="Always"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <ClearButton DisplayMode="Always"></ClearButton>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>

                                <%--End of Inventory Type--%>
                                <%--Debjyoti Stock Valuation Tech.--%>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--Inventory Item--%>
                                        <asp:Label ID="Label4" runat="server" Text="Stock Valuation Tech." CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="CmbStockValuation" ClientInstanceName="cCmbStockValuation" runat="server"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                            <Items>
                                                <dxe:ListEditItem Text="LIFO" Value="L" />
                                                <dxe:ListEditItem Text="FIFO" Value="F" />
                                                <dxe:ListEditItem Text="Average" Value="A" />
                                                <%--<dxe:ListEditItem Text="RATED" Value="R" />--%>
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <%--  <asp:Label ID="Label4" runat="server" Text="Stock UOM" CssClass="newLbl"></asp:Label>--%>
                                        <span class="newLbl">Stock Unit<span style="color: red;"> *</span></span>
                                    </div>
                                    <div class="Left_Content">
                                        <dxe:ASPxComboBox ID="cmbStockUom" ClientInstanceName="ccmbStockUom" runat="server" ClearButton-DisplayMode="Always"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                            </ValidationSettings>
                                            <ClearButton DisplayMode="Always"></ClearButton>
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>


                            </div>
                            <%--End here--%>
                        </div>
                        <div class="col-md-6" style="padding: 0px !important;">

                            <%-- <div style="clear: both"></div>--%>
                            <div class="boxarea clearfix">
                                <span class="boxareaH">Ledger Mapping</span>
                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label17" runat="server" Text="Sales" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <div class="Left_Content">

                                        <dxe:ASPxButtonEdit ID="SIMainAccount" runat="server" ReadOnly="true" ClientInstanceName="cSIMainAccount" Width="100%">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="MainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                        </dxe:ASPxButtonEdit>
                                        <%--<dxe:ASPxComboBox ID="cmbsalesInvoice" ClientInstanceName="ccmbsalesInvoice" runat="server" TabIndex="25"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                            <ClientSideEvents SelectedIndexChanged="mainAccountSalesInvoice" GotFocus="cmbsalesInvoiceGotFocus" />
                                        </dxe:ASPxComboBox>--%>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label18" runat="server" Text="Sales Return" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <dxe:ASPxButtonEdit ID="SRMainAccount" runat="server" ReadOnly="true" ClientInstanceName="cSRMainAccount" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="SRMainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                    </dxe:ASPxButtonEdit>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label19" runat="server" Text="Purchase" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <dxe:ASPxButtonEdit ID="PIMainAccount" runat="server" ReadOnly="true" ClientInstanceName="cPIMainAccount" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="PIMainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                    </dxe:ASPxButtonEdit>
                                </div>

                                <div class="col-md-3">
                                    <div class="cityDiv" style="height: auto;">
                                        <asp:Label ID="Label20" runat="server" Text="Purchase Return" CssClass="newLbl"></asp:Label>
                                    </div>
                                    <dxe:ASPxButtonEdit ID="PRMainAccount" runat="server" ReadOnly="true" ClientInstanceName="cPRMainAccount" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="PRMainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                    </dxe:ASPxButtonEdit>
                                </div>

                            </div>
                        </div>
                        <div style="clear: both"></div>

                        <%--Code commented and added by debjyoti--%>
                        <%--Reason: Product attribute now showing on popup--%>
                        <div class="col-md-12">
                            <div class="cityDiv" style="height: auto;">

                                <%--<asp:Label ID="Label1" runat="server" Text="(s)" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content" style="padding-top: 10px">
                                <button type="button" class="btn btn-info btn-small" onclick="ShowProductAttribute()" id="btnProdConfig">Configure Product Attribute</button>
                                <button type="button" class="btn btn-info btn-small" onclick="ShowBarCode()" id="btnBarCodeConfig" style="display: none">Configure Barcode</button>
                                <button type="button" class="btn btn-info btn-small" onclick="ShowTaxCode()" style="display: none">Configure Tax</button>
                                <button type="button" class="btn btn-info btn-small" onclick="ShowServiceTax()" id="btnServiceTaxConfig">Configure Service Category</button>
                                <button type="button" class="btn btn-info btn-small" onclick="ShowPackingDetails()" id="btnPackingConfig">Configure UOM Conversion</button>
                                <button type="button" class="btn btn-info btn-small" onclick="ShowTdsSection()" id="btnTDS">Configure TDS Section</button>
                            </div>
                        </div>


                        <div class="col-md-2">
                            <div class="cityDiv" style="height: auto;">

                                <%--<asp:Label ID="Label7" runat="server" Text="Bar Code" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content" style="padding-top: 10px">
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="cityDiv" style="height: auto;">

                                <%--<asp:Label ID="Label8" runat="server" Text="Tax Codes" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content" style="padding-top: 10px">
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="cityDiv" style="height: auto;">
                            </div>
                            <div class="Left_Content" style="padding-top: 10px">
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="cityDiv" style="height: auto;">
                            </div>
                            <div class="Left_Content" style="padding-top: 10px">
                            </div>
                        </div>


                        <%-- //......................... Code Commented and Updated  by Sam on 04-10-2014............................--%>


                        <%-- <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                     
                                    <asp:Label ID="LblProductColor" runat="server" Text="Product Color" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbProductColor" ClientInstanceName="cCmbProductColor" ClearButton-DisplayMode="Always" runat="server" TabIndex="16"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>


                            <div class="col-md-6" style="margin-top: 25px">
                                <div class="cityDiv" style="height: auto;">
                                    
                                </div>
                                <div class="Left_Content">
                                    
                                    <dxe:ASPxRadioButtonList ID="rdblappColor" ClientInstanceName="RrdblappColor"  runat="server" RepeatDirection="Horizontal" Width="226px" TabIndex="17">
                                        <Items>
                                             <dxe:ListEditItem Text="Applicable" Value="1" Selected="true" />
                                            <dxe:ListEditItem Text="Not Applicable" Value="0" />
                                           
                                        </Items>
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>--%>

                        <%-- //......................... Code Commented and Updated  by Sam on 04-10-2014............................--%>
                        <%--   <div style="clear: both"></div>

                            <div class="col-md-6" >
                                <div class="cityDiv" style="height: auto;">
                                     
                                    <asp:Label ID="LblProductSize" runat="server" Text="Product Size" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbProductSize" ClientInstanceName="cCmbProductSize" ClearButton-DisplayMode="Always" runat="server" TabIndex="18"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div class="col-md-6" style="margin-top: 25px">
                                <div class="cityDiv" style="height: auto;">
                                </div>
                                <div class="Left_Content">

                                    <dxe:ASPxRadioButtonList ID="rdblapp" ClientInstanceName="Rrdblapp" runat="server" RepeatDirection="Horizontal" Width="226px" TabIndex="19">
                                        <Items>
                                            <dxe:ListEditItem Text="Applicable" Value="1" Selected="true" />
                                            <dxe:ListEditItem Text="Not Applicable" Value="0" />
                                            
                                        </Items>
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>--%>
                    </div>
                    <div class="ContentDiv" style="height: auto">
                        <div style="display: none">
                            <div style="height: 20px; width: 280px; background-color: Gray; padding-left: 120px;">
                                <h5>Static Code</h5>
                            </div>
                            <div style="height: 20px; width: 130px; padding-left: 70px; background-color: Gray; float: left;">
                                Exchange
                            </div>
                            <div style="height: 20px; width: 200px; background-color: Gray; text-align: left;">
                                Value
                            </div>
                            <div class="ScrollDiv">
                                <div class="cityDiv" style="padding-top: 5px;">
                                    NSE Code
                                </div>
                                <div style="padding-top: 5px;">
                                    <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    BSE Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    MCX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    MCXSX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NCDEX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    CDSL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" CssClass="cityTextbox"
                                        runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NSDL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" CssClass="cityTextbox"
                                        runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NDML Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    CVL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtCvlCode" ClientInstanceName="ctxtCvlCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    DOTEX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtDotexCode" ClientInstanceName="ctxtDotexCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                        </div>

                        <br style="clear: both;" />
                        <div class="col-md-12"></div>
                        <div class="Footer clearfix" style="padding-left: 16px">


                            <dxe:ASPxButton ID="btnSave_citys" CausesValidation="true" ClientInstanceName="cbtnSave_citys" runat="server" ValidationGroup="product" EncodeHtml="false"
                                AutoPostBack="False" Text="<u>S</u>ave" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {btnSave_citys();}" />
                            </dxe:ASPxButton>


                            <dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="<u>C</u>ancel" CssClass="btn btn-danger" EncodeHtml="false">
                                <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                            </dxe:ASPxButton>
                            <asp:Button ID="btnUdf" runat="server" Text="UDF" CssClass="btn btn-primary dxbButton" OnClientClick="if(OpenUdf()){ return false;}" />
                            <input type="button" value="Assing Values" style="display: none;" onclick="fetchLebel()" class="btn btn-primary" />

                            <br style="clear: both;" />
                        </div>
                        <br style="clear: both;" />
                    </div>
                    <%-- </div>--%>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>

        <HeaderStyle BackColor="LightGray" ForeColor="Black" />

    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl runat="server" ID="MainAccountModelSI" ClientInstanceName="cMainAccountModelSI"
        Width="500px" Height="300px" HeaderText="Search Main Account" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search Main Account</span>
            <dxe:ASPxImage ID="ASPxImage2" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cMainAccountModelSI.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydown(event)" id="txtMainAccountSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="function(){ $('#txtMainAccountSearch').focus();}" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="hdnSIMainAccount" />

    <dxe:ASPxPopupControl runat="server" ID="MainAccountModelSR" ClientInstanceName="cMainAccountModelSR"
        Width="500px" Height="300px" HeaderText="Search Main Account" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search Main Account</span>
            <dxe:ASPxImage ID="ASPxImage4" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cMainAccountModelSR.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountSRNewkeydown(event)" id="txtMainAccountSRSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTableSR">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="function(){ $('#txtMainAccountSRSearch').focus();}" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="hdnSRMainAccount" />


    <dxe:ASPxPopupControl runat="server" ID="MainAccountModelPI" ClientInstanceName="cMainAccountModelPI"
        Width="500px" Height="300px" HeaderText="Search Main Account" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search Main Account</span>
            <dxe:ASPxImage ID="ASPxImage5" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cMainAccountModelPI.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountPINewkeydown(event)" id="txtMainAccountPISearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTablePI">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="function(){ $('#txtMainAccountPISearch').focus();}" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="hdnPIMainAccount" />

    <dxe:ASPxPopupControl runat="server" ID="MainAccountModelPR" ClientInstanceName="cMainAccountModelPR"
        Width="500px" Height="300px" HeaderText="Search Main Account" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search Main Account</span>
            <dxe:ASPxImage ID="ASPxImage6" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cMainAccountModelPR.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountPRNewkeydown(event)" id="txtMainAccountPRSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTablePR">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="function(){ $('#txtMainAccountPRSearch').focus();}" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="hdnPRMainAccount" />
    <asp:HiddenField runat="server" ID="HiddenField_status" />

    <dxe:ASPxPopupControl ID="AspxDirectCustomerViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectCustomerViewPopup" Height="650px"
        Width="1020px" HeaderText="View Product" Modal="true" AllowResize="False">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <asp:SqlDataSource ID="ProductDataSource" runat="server"
        SelectCommand="select sProducts_ID,sProducts_Code,sProducts_Name from Master_sProducts"></asp:SqlDataSource>
    <asp:SqlDataSource ID="HsnDataSource" runat="server"
        SelectCommand="select * from tbl_HSN_Master"></asp:SqlDataSource>
    <asp:SqlDataSource ID="tdstcs" runat="server"
        SelectCommand="Select  TDSTCS_ID,ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' as tdsdescription ,ltrim(rtrim(tdstcs_code)) tdscode  from master_tdstcs "></asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlClassSource" runat="server"
        SelectCommand="select ProductClass_Name from Master_ProductClass order by ProductClass_Name"></asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlHSNDataSource" runat="server"
        SelectCommand="select distinct sProducts_HsnCode Code  from master_sproducts where sProducts_HsnCode<>''  union all select  distinct SERVICE_CATEGORY_CODE   from Master_sProducts MP inner join TBL_MASTER_SERVICE_TAX sac on MP.sProducts_serviceTax=sac.TAX_ID "></asp:SqlDataSource>

    <dxe:ASPxPopupControl ID="productAttributePopUp" runat="server" ClientInstanceName="cproductAttributePopUp"
        Width="550px" HeaderText="Set Product Attribute(s)" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Set Product Attribute(s)</span>
            <dxe:ASPxImage ID="img" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){
                                cCmbProductSize.SetValue(ProdSize);
                                cCmbProductColor.SetValue(ProdColor);
                                 RrdblappColor.SetSelectedIndex(ColApp);
                                Rrdblapp.SetSelectedIndex(SizeApp);
                                cproductAttributePopUp.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="row">
                    <div class="col-md-6">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="LblProductColor" runat="server" Text="Product Color" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxComboBox ID="CmbProductColor" ClientInstanceName="cCmbProductColor" ClearButton-DisplayMode="Always" runat="server" TabIndex="16"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>


                    <div class="col-md-6" style="margin-top: 25px">
                        <div class="cityDiv" style="height: auto;">
                        </div>
                        <div class="Left_Content">

                            <dxe:ASPxRadioButtonList ID="rdblappColor" ClientInstanceName="RrdblappColor" runat="server" RepeatDirection="Horizontal" Width="100%" TabIndex="17">
                                <Items>
                                    <dxe:ListEditItem Text="Applicable" Value="1" Selected="true" />
                                    <dxe:ListEditItem Text="Not Applicable" Value="0" />

                                </Items>
                            </dxe:ASPxRadioButtonList>
                        </div>
                    </div>

                    <div style="clear: both"></div>

                    <div class="col-md-6">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="LblProductSize" runat="server" Text="Product Size" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxComboBox ID="CmbProductSize" ClientInstanceName="cCmbProductSize" ClearButton-DisplayMode="Always" runat="server" TabIndex="18"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-md-6" style="margin-top: 25px">
                        <div class="cityDiv" style="height: auto;">
                        </div>
                        <div class="Left_Content">

                            <dxe:ASPxRadioButtonList ID="rdblapp" ClientInstanceName="Rrdblapp" runat="server" RepeatDirection="Horizontal" Width="100%" TabIndex="19">
                                <Items>
                                    <dxe:ListEditItem Text="Applicable" Value="1" Selected="true" />
                                    <dxe:ListEditItem Text="Not Applicable" Value="0" />

                                </Items>
                            </dxe:ASPxRadioButtonList>
                        </div>
                    </div>

                    <%--Product Component--%>
                    <div class="clear"></div>
                    <div class="col-md-12" style="margin-top: 7px">Components</div>
                    <div class="clear"></div>
                    <div class="col-md-6" style="margin-top: 7px">
                        <div class="cityDiv" style="height: auto; margin-bottom: 5px;">

                            <div id="divProductMasterComponentMandatory" runat="server">
                                (Mandatory tick/untick)
                                        <dxe:ASPxCheckBox runat="server" ID="chkIsMandatory" ClientInstanceName="cchkIsMandatory"></dxe:ASPxCheckBox>
                            </div>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="Component_Callback">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">


                                        <dxe:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Multiple" DataSourceID="ProductDataSource" ClientInstanceName="gridLookup"
                                            KeyFieldName="sProducts_ID" Width="100%" TextFormatString="{0}" MultiTextSeparator=", ">
                                            <Columns>
                                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />
                                                <dxe:GridViewDataColumn FieldName="sProducts_Code" Caption="Product Code" Width="150" />
                                                <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Product Name" Width="300" />
                                            </Columns>
                                            <%--<GridViewProperties  Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords"   >--%>
                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                                <Templates>
                                                    <StatusBar>
                                                        <table class="OptionsTable" style="float: right">
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
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
                                <ClientSideEvents EndCallback="componentEndCallBack" />
                            </dxe:ASPxCallbackPanel>
                        </div>
                    </div>



                    <div id="divPosInstallation" runat="server">


                        <%--Installation Required--%>
                        <div class="col-md-6">
                            <div class="cityDiv" style="height: auto;">

                                <asp:Label ID="Label8" runat="server" Text="Installation Required" CssClass="newLbl"></asp:Label>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="aspxInstallation" ClientInstanceName="caspxInstallation" runat="server" TabIndex="16"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                    <Items>
                                        <dxe:ListEditItem Text="Yes" Value="1" />
                                        <dxe:ListEditItem Text="No" Value="0" />
                                    </Items>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div style="clear: both"></div>





                    <%--Brand --%>
                    <div class="col-md-6">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="Label13" runat="server" Text="Brand" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxComboBox ID="cmbBrand" ClientInstanceName="ccmbBrand" ClearButton-DisplayMode="Always" runat="server" TabIndex="16"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>


                    <div class="col-md-6" id="divPosOldUnit" runat="server">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="Label16" runat="server" Text="Old Unit?" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxComboBox ID="cmbOldUnit" ClientInstanceName="ccmbOldUnit" runat="server" TabIndex="16"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="1">
                                <Items>
                                    <dxe:ListEditItem Text="Yes" Value="1" />
                                    <dxe:ListEditItem Text="No" Value="0" />
                                </Items>
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                </div>
                <div style="clear: both"></div>

                <div class="boxarea clearfix">
                    <%--<span class="boxareaH">Product Size</span>--%>
                    <table class="mkSht">

                        <tr>
                            <td>
                                <label>Product Series</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtSeries" ClientInstanceName="ctxtSeries" runat="server"></dxe:ASPxTextBox>

                                </div>
                            </td>
                            <td>
                                <label>Surface</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtFinish" ClientInstanceName="ctxtFinish" runat="server"></dxe:ASPxTextBox>
                                </div>
                            </td>
                            <td>
                                <label>Lead Time</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtLeadtime" ClientInstanceName="ctxtLeadtime" runat="server"></dxe:ASPxTextBox>
                                </div>
                            </td>
                            <td>
                                <label class="pull-right">Weight</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtWeight" ClientInstanceName="ctxtWeight" DisplayFormatString="0.00" HorizontalAlign="Right" runat="server">
                                        <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                    </dxe:ASPxTextBox>
                                </div>
                            </td>
                            <td>
                                <label>Sub-Category</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtSubCat" MaxLength="100" ClientInstanceName="ctxtSubCat" DisplayFormatString="0.00" HorizontalAlign="Right" runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                            </td>

                        </tr>

                    </table>

                </div>
                <div style="clear: both"></div>
                <div class="boxarea clearfix">
                    <span class="boxareaH">Product Coverage Area</span>
                    <table class="mkSht">
                        <tr>
                            <td>
                                <label class="pull-right">Length</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtHeight" ClientInstanceName="ctxtHeight" HorizontalAlign="Right" Text="0" DisplayFormatString="0.00" runat="server" Width="100%">
                                        <ClientSideEvents LostFocus="SizeUOMChange" />
                                        <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                    </dxe:ASPxTextBox>
                                </div>
                            </td>
                            <td class="multiply" style="color: blue; font-size: 20px;">x</td>
                            <td>
                                <label class="pull-right">Width</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtWidth" ClientInstanceName="ctxtWidth" HorizontalAlign="Right" runat="server" Width="100%">
                                        <ClientSideEvents LostFocus="SizeUOMChange" />
                                        <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                    </dxe:ASPxTextBox>

                                </div>
                            </td>

                            <td>
                                <label class="pull-right">Thickness</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtThickness" ClientInstanceName="ctxtThickness" HorizontalAlign="Right" runat="server" Width="100%">
                                        <ClientSideEvents LostFocus="SizeUOMChange" />
                                        <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                    </dxe:ASPxTextBox>

                                </div>
                            </td>
                            <td>
                                <label>UOM</label>
                                <div>
                                    <dxe:ASPxComboBox ID="ddlSize" ClientInstanceName="cddlSize" runat="server" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                        <ClientSideEvents ValueChanged="SizeUOMChange" />
                                    </dxe:ASPxComboBox>
                                </div>
                            </td>
                            <td style="width: 102px;">
                                <label>For</label>
                                <div>
                                    <select id="SizeUOM" runat="server" class="form-control" onchange="SizeUOMChange();">
                                        <option selected value="1">First UOM</option>
                                        <option value="2">Second UOM</option>
                                    </select>
                                </div>
                            </td>
                        </tr>

                    </table>
                    <table class="mkSht">
                        <tr>

                            <td>
                                <label class="pull-right">Coverage Area</label>
                                <input value="0.00" runat="server" id="txtCoverage" style="text-align: right;" disabled="disabled" type="text" />

                            </td>
                            <td>
                                <label>&nbsp;</label>
                                <div runat="server" id="dvCovg"></div>
                            </td>

                            <td>
                                <label class="pull-right">Volume</label><input value="0.00" style="text-align: right;" runat="server" id="txtVolumn" disabled="disabled" type="text" /></td>
                            <td>
                                <label>&nbsp;</label>
                                <div runat="server" id="dvvolume">Unit</div>
                            </td>
                        </tr>
                        <tr>

                            <td>
                                <div class="red sText" style="color: blue; font-size: 15px;">Length*Width</div>
                            </td>
                            <td></td>
                            <td>
                                <div class="red sText" style="color: blue; font-size: 15px;">Length*Width*Thickness</div>
                            </td>
                            <td></td>
                        </tr>
                    </table>

                </div>
                <div style="clear: both"></div>

                <div class="boxarea clearfix">
                    <table class="mkSht hide">
                        <tr>
                            <td>
                                <label>Coverage Per</label></td>
                            <td>


                                <div>
                                    <select id="covergaeUOM" class="form-control" disabled="disabled">
                                        <option value="1">First UOM</option>
                                        <option value="2">Second UOM</option>
                                    </select>

                                </div>
                            </td>
                            <td>
                                <div>
                                </div>
                            </td>
                            <td></td>

                        </tr>
                    </table>
                </div>

                <div style="clear: both"></div>


                <div class="col-md-6" style="margin-top: 25px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button class="btn btn-primary" type="button" onclick="productAttributeOkClik()">Ok</button>

                    </div>
                </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitProductAttribute" />
    </dxe:ASPxPopupControl>

    <%--Packing Details popup--%>
    <dxe:ASPxPopupControl ID="ASPxPopupControl2" runat="server" ClientInstanceName="cpackingDetails"
        Width="500px" HeaderText="Packing Details" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <HeaderTemplate>
            <span>UOM Conversion</span>
            <dxe:ASPxImage ID="ASPxImage7" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                $('#invalidPackingUom').css({ 'display': 'none' });
                                cpackingDetails.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>

                <table>
                    <tr>
                        <td class="pull-left">Quantity</td>
                        <%--Rev 1.0 11-03-2019--%>
                        <%--<td>Sale UOM</td>--%>
                        <%--<td>UOM</td>--%>
                        <%--mantis issue number 19833--%>
                        <td>Main Unit</td>
                        <%--End of Rev 1.0 11-03-2019--%>
                        <td></td>
                        <%--Rev 1.0 07-03-2019--%>
                        <%-- <td>Packing</td>
                                    <td>Select UOM</td>--%>
                        <%--mantis issue number 19833--%>
                        <td class="pull-left">Alt. Quantity</td>
                        <td>Alt. Unit</td>
                        <%--End of Rev 1.0 07-03-2019--%>
                    </tr>
                    <tr>
                        <td style="padding-right: 7px">
                            <dxe:ASPxTextBox ID="txtPackingQty" HorizontalAlign="Right" ClientInstanceName="ctxtPackingQty" MaxLength="50" runat="server" Width="100%">
                                <MaskSettings Mask="<0..99999999>.<0..99>" />
                                <ClientSideEvents LostFocus="SizeUOMChange" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="padding-right: 7px">
                            <dxe:ASPxTextBox ID="txtpackingSaleUom" ClientInstanceName="ctxtpackingSaleUom" MaxLength="50" runat="server" Width="100%">
                                <ClientSideEvents LostFocus="SizeUOMChange" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="padding-right: 7px">
                            <span>=</span>

                        </td>
                        <td style="padding-right: 7px">
                            <dxe:ASPxTextBox ID="txtpacking" ClientInstanceName="ctxtpacking" HorizontalAlign="Right" MaxLength="50" runat="server" Width="100%">
                                <MaskSettings Mask="<0..99999999>.<0..99>" />
                                <ClientSideEvents LostFocus="SizeUOMChange" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="padding-right: 7px">
                            <dxe:ASPxComboBox ID="cmbPackingUomPro" ClientInstanceName="ccmbPackingUomPro" runat="server" SelectedIndex="0"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                <ClientSideEvents SelectedIndexChanged="SizeUOMChange" />
                            </dxe:ASPxComboBox>
                            <%--Rev Subhra 02-04-2019--%>
                            <%--<span id="invalidPackingUom" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;" title="Invalid GSTIN"></span>--%>
                            <span id="invalidPackingUom" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;" title="Invalid 2nd UOM"></span>
                            <%--End of Rev Subhra 02-04-2019--%>
                        </td>
                    </tr>

                </table>

                <table id="tblOverideConvertion">
                    <tr>
                        <td>
                            <dxe:ASPxCheckBox runat="server" ID="chkOverideConvertion" ClientInstanceName="cchkOverideConvertion"></dxe:ASPxCheckBox>
                        </td>
                        <td>Do not override UOM conversion in transaction.   
                        </td>
                    </tr>
                </table>



                <div style="clear: both"></div>
                <div class="" style="margin-top: 12px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-primary" onclick="PackingDetailsOkClick()">Ok</button>

                    </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitTax" />
    </dxe:ASPxPopupControl>
    <%--Packing Details popup end Here--%>

    <dxe:ASPxPopupControl ID="BarCodePopUp" runat="server" ClientInstanceName="cBarCodePopUp"
        Width="360px" HeaderText="Set Barcode" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <HeaderTemplate>
            <span>Set Barcode</span>
            <dxe:ASPxImage ID="ASPxImage8" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){
                                cCmbBarCodeType.SetSelectedIndex(barCodeType);
                                ctxtBarCodeNo.SetText(BarCode);
                                ctxtGlobalCode.SetText(GlobalCode);
                                cBarCodePopUp.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>
                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">

                        <asp:Label ID="lblBarCodeType" runat="server" Text="Barcode Type" CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxComboBox ID="CmbBarCodeType" ClientInstanceName="cCmbBarCodeType" runat="server" SelectedIndex="0" TabIndex="6" ClearButton-DisplayMode="Always"
                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        </dxe:ASPxComboBox>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">

                        <asp:Label ID="lblBarcodeNo" runat="server" Text="Barcode No." CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxTextBox ID="txtBarCodeNo" ClientInstanceName="ctxtBarCodeNo" MaxLength="50" TabIndex="7"
                            runat="server" Width="100%">
                        </dxe:ASPxTextBox>
                    </div>
                </div>

                <div style="clear: both"></div>

                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">
                        <%--Global Code--%>
                        <asp:Label ID="LblGlobalCode" runat="server" Text="Global Code(UPC)" CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxTextBox ID="txtGlobalCode" ClientInstanceName="ctxtGlobalCode" MaxLength="30" TabIndex="8"
                            runat="server" Width="100%" Text='<%# Bind("txtMarkets_Description") %>'>
                        </dxe:ASPxTextBox>
                    </div>
                </div>

                <div style="clear: both"></div>
                <div class="col-md-6" style="margin-top: 25px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-primary" onclick="BarCodeOkClick()">Ok</button>

                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitBarCode" />
    </dxe:ASPxPopupControl>
    <dxe:ASPxCallbackPanel ClientInstanceName="cgridprod" ID="gridPro" runat="server" OnCallback="cityGrid_CustomCallback" ClientSideEvents-EndCallback="grid_EndCallBack">
    </dxe:ASPxCallbackPanel>
    <%--taxCode popup--%>
    <dxe:ASPxPopupControl ID="TaxCodePopup" runat="server" ClientInstanceName="cTaxCodePopup"
        Width="360px" HeaderText="Set Tax Codes" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <HeaderTemplate>
            <span>Set Tax Codes</span>
            <dxe:ASPxImage ID="ASPxImage9" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){
                                cCmbTaxCodeSale.SetValue(taxCodeSale);
                                cCmbTaxCodePur.SetValue(taxCodePur);
                                 cChkAutoApply.SetChecked(autoApply);
                                cCmbTaxScheme.SetValue(taxScheme);
                                GetCheckBoxValue(autoApply);
                                cTaxCodePopup.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>
                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">

                        <asp:Label ID="Label9" runat="server" Text="Select Tax Code Scheme -Sales" CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxComboBox ID="CmbTaxCodeSale" ClientInstanceName="cCmbTaxCodeSale" runat="server" SelectedIndex="0"
                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">

                        <asp:Label ID="Label10" runat="server" Text="Select Tax Code Scheme -Purchases" CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxComboBox ID="CmbTaxCodePur" ClientInstanceName="cCmbTaxCodePur" runat="server" SelectedIndex="0"
                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div style="clear: both"></div>
                <div class="hide">
                    <div class="col-md-12">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="Label11" runat="server" Text="Apply Auto Selection in Entries" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxCheckBox runat="server" ID="ChkAutoApply" ClientInstanceName="cChkAutoApply">
                                <ClientSideEvents CheckedChanged="function(s, e) { 
                                            GetCheckBoxValue(s.GetChecked()); 
                                        }" />
                            </dxe:ASPxCheckBox>
                        </div>
                    </div>

                    <div style="clear: both"></div>
                    <div class="col-md-12">
                        <div class="cityDiv" style="height: auto;">

                            <asp:Label ID="Label15" runat="server" Text="Select Tax Scheme" CssClass="newLbl"></asp:Label>
                        </div>
                        <div class="Left_Content">
                            <dxe:ASPxComboBox ID="CmbTaxScheme" ClientInstanceName="cCmbTaxScheme" runat="server" SelectedIndex="0"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                </div>


                <div style="clear: both"></div>
                <div class="col-md-6" style="margin-top: 25px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-primary" onclick="taxCodeOkClick()">Ok</button>

                    </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitTax" />
    </dxe:ASPxPopupControl>

    <%--TaxCode popup End Here--%>

    <%--Service Tax popup--%>
    <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cServiceTaxPopup"
        Width="360px" HeaderText="Service Category" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <HeaderTemplate>
            <span>Set Service Category</span>
            <dxe:ASPxImage ID="ASPxImage10" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cServiceTaxPopup.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>
                <div class="cityDiv" style="height: auto;">

                    <asp:Label ID="Label25" runat="server" Text="Service Category" CssClass="newLbl"></asp:Label>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="AspxServiceTax" ClientInstanceName="cAspxServiceTax" runat="server" SelectedIndex="0" DropDownWidth="800"
                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True"
                        ValueField="TAX_ID" IncrementalFilteringMode="Contains" CallbackPageSize="30" TextFormatString="{0} {1}" ItemStyle-Wrap="True">
                        <Columns>
                            <dxe:ListBoxColumn FieldName="SERVICE_CATEGORY_CODE" Caption="Code" Width="45" />
                            <dxe:ListBoxColumn FieldName="SERVICE_TAX_NAME" Caption="Name" Width="250" />
                            <%-- <dxe:ListBoxColumn FieldName="ACCOUNT_HEAD_TAX_RECEIPTS" Caption="Receipts" Width="65" />
                                        <dxe:ListBoxColumn FieldName="ACCOUNT_HEAD_OTHERS_RECEIPTS" Caption="Oth Receipts" Width="65" />
                                        <dxe:ListBoxColumn FieldName="ACCOUNT_HEAD_PENALTIES" Caption="Penalties" Width="65" />
                                        <dxe:ListBoxColumn FieldName="ACCOUNT_HEAD_DeductRefund" Caption="A/C Head (Deduct Refund)" Width="120" />--%>
                        </Columns>

                    </dxe:ASPxComboBox>
                </div>



                <div style="clear: both"></div>
                <div class="col-md-6" style="margin-top: 25px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-primary" onclick="ServicetaxOkClick()">Ok</button>

                    </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitTax" />
    </dxe:ASPxPopupControl>
    <%--TaxCode popup End Here--%>

    <%--Tds Section start Here--%>
    <dxe:ASPxPopupControl ID="tdsPopup" runat="server" ClientInstanceName="ctdsPopup"
        Width="450" HeaderText="Set TDS" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <HeaderTemplate>
            <span>Set TDS Codes</span>
            <dxe:ASPxImage ID="ASPxImage11" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cmb_tdstcs.SetValue(tdsValue);
                                ctdsPopup.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>
                <div class="col-md-12">
                    <div class="cityDiv" style="height: auto;">

                        <asp:Label ID="Label26" runat="server" Text="TDS Section" CssClass="newLbl"></asp:Label>
                    </div>
                    <div class="Left_Content">
                        <dxe:ASPxComboBox ID="cmb_tdstcs" ClientInstanceName="cmb_tdstcs" DataSourceID="tdstcs" Width="100%" ItemStyle-Wrap="True"
                            ClearButton-DisplayMode="Always" runat="server" TextField="tdscode" ValueField="TDSTCS_ID">
                        </dxe:ASPxComboBox>
                    </div>
                </div>

                <div style="clear: both"></div>
                <div class="col-md-6" style="margin-top: 25px">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-primary" onclick="tdsOkClick()">Ok</button>

                    </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        <ClientSideEvents Init="OnInitTax" />
    </dxe:ASPxPopupControl>
    <%--TDS popup End Here--%>

    <div class="HiddenFieldArea" style="display: none;">
        <asp:HiddenField runat="server" ID="hiddenedit" />
    </div>
    <asp:HiddenField runat="server" ID="hdnPartialSettings" />
    <%--Product Pop up End--%>

    <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="1100px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeMultiUOM(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class=" clearfix">



                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">

                        <table class="eqTble">
                            <tr>
                                <%--Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="MultiUOMSR No"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" width="0">
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24429--%>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--Rev Mantis Issue 24429--%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal" />--%>
                                                <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onchange="CalcBaseRate()" />
                                                <%--End of Rev Mantis Issue 24429--%>
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
                                <%--Mantis Issue 24429--%>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true"></dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Mantis Issue 24429--%>
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
                                            <%--  <input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/> --%>
                                            <dxe:ASPxTextBox ID="AltUOMQuantity" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <%--Mantis Issue 24429--%>
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Mantis Issue 24429--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24429--%>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
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
                                                <input type="checkbox" id="chkUpdateRow" />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>


                                </td>
                                <%--End of Mantis Issue 24429--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
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
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>

                                <%--Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24429--%>


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

                                <%--Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24429--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>','<%#Eval("DetailsId") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>

                                        <%--Mantis Issue 24429--%>
                                        <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                        <%--End of Mantis Issue 24429--%>
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
                            <dxe:ASPxButton ID="ASPxButton9" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

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
                                <asp:Label ID="lblInlineRemarks" runat="server" Text="Remarks"></asp:Label>

                                <asp:TextBox ID="txtInlineRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" MaxLength="5000"></asp:TextBox>
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



    <asp:SqlDataSource ID="UomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:HiddenField ID="hddnuomFactor" runat="server" />
    <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />
    <asp:HiddenField ID="hdnQuantitySL" runat="server" />
    <asp:HiddenField ID="hdnEntityType" runat="server" />
    <asp:HiddenField runat="server" ID="hdnQty" />

    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnAllowDuplicatePartyInvoiceNo" runat="server" />
    <%--Rev Mantis Issue 24061--%>
    <asp:HiddenField runat="server" ID="hdnPurchaseOrderItemNegative" />
    <%--End of Rev Mantis Issue 24061--%>

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
                            <dxe:ASPxComboBox DataSourceID="tcsDatasource" TextField="TCS_SECTION" SelectedIndex="0" ValueField="TDSTCS_Code" ValueType="System.String" runat="server" ID="txtTCSSection" ClientInstanceName="ctxtTCSSection">
                            </dxe:ASPxComboBox>
                        </div>
                        <div class="col-md-3">
                            <label>
                                TCS Applicable Amount
                            </label>
                            <dxe:ASPxTextBox runat="server" ClientSideEvents-LostFocus="CalcTCSAmount" ClientEnabled="true" ID="txtTCSapplAmount" ClientInstanceName="ctxtTCSapplAmount">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </div>

                        <div class="col-md-3">
                            <label>
                                TCS Percentage
                            </label>
                            <dxe:ASPxTextBox ClientSideEvents-LostFocus="CalcTCSAmount" runat="server" ClientEnabled="true" ID="txtTCSpercentage" ClientInstanceName="ctxtTCSpercentage">
                                <MaskSettings Mask="&lt;0..99&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />

                            </dxe:ASPxTextBox>
                        </div>

                        <div class="col-md-3">
                            <label>
                                TCS Amount
                            </label>
                            <dxe:ASPxTextBox runat="server" ClientEnabled="false" ID="txtTCSAmount" ClientInstanceName="ctxtTCSAmount">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
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


    <div id="tdsModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">TDS Calculation</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-3">
                            <label>
                                TDS Section
                            </label>
                            <dxe:ASPxComboBox DataSourceID="tdsDatasource" ClientSideEvents-SelectedIndexChanged="TDSsectionchanged" TextField="TDS_SECTION" SelectedIndex="0" ValueField="TDSTCS_Code" ValueType="System.String" runat="server" ID="txtTDSSection" ClientInstanceName="ctxtTDSSection">
                            </dxe:ASPxComboBox>
                        </div>
                        <div class="col-md-3">
                            <label>
                                TDS Applicable Amount
                            </label>
                            <dxe:ASPxTextBox runat="server" ClientSideEvents-LostFocus="CalcTDSAmount" ClientEnabled="true" ID="txtTDSapplAmount" ClientInstanceName="ctxtTDSapplAmount">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </div>

                        <div class="col-md-3">
                            <label>
                                TDS Percentage
                            </label>
                            <dxe:ASPxTextBox ClientSideEvents-LostFocus="CalcTDSAmount" runat="server" ClientEnabled="true" ID="txtTDSpercentage" ClientInstanceName="ctxtTDSpercentage">
                                <MaskSettings Mask="&lt;0..99&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />

                            </dxe:ASPxTextBox>
                        </div>

                        <div class="col-md-3">
                            <label>
                                TDS Amount
                            </label>
                            <dxe:ASPxTextBox runat="server" ClientEnabled="false" ID="txtTDSAmount" ClientInstanceName="ctxtTDSAmount">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="GridTDSdocs" runat="server" ClientInstanceName="cGridTDSdocs" Width="100%"
                                KeyFieldName="SLNO" OnDataBinding="GridTDSdocs_DataBinding" OnCustomCallback="GridTDSdocs_CustomCallback">
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


    <asp:SqlDataSource runat="server" ID="tcsDatasource" SelectCommand="select TDSTCS_Code,LTRIM(RTRIM(TDSTCS_Code))+' ('+TDSTCS_Description+')' TCS_SECTION from Master_TDSTCS inner join tbl_master_TDS_Section on Section_Code=TDSTCS_Code where TYPE='TCS'"></asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="tdsDatasource" SelectCommand=" select TDSTCS_Code,LTRIM(RTRIM(TDSTCS_Code))+' ('+TDSTCS_Description+')' TDS_SECTION from Master_TDSTCS inner join tbl_master_TDS_Section on Section_Code=TDSTCS_Code where TYPE='TDS' and TDSTCS_ID not in (select DISTINCT TDSTCS_ID from tbl_master_productTdsMap where TDSTCS_ID<>0) and TDSTCS_Code='194Q'"></asp:SqlDataSource>

    <asp:HiddenField ID="hdnBackdateddate" runat="server" />
    <asp:HiddenField ID="hdnTagDateForbackdated" runat="server" />

    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
    <asp:HiddenField ID="hdnDatafrom" runat="server" />
    <asp:HiddenField ID="hdnDatato" runat="server" />
    <asp:HiddenField ID="HdnBackDatedEntryPurchaseGRN" runat="server" />
    <asp:HiddenField ID="hdnnumberingFromdate" runat="server" />
    <asp:HiddenField ID="hdnnumberingTodate" runat="server" />


    <div class="modal fade" id="AmtGridModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Invoice Details</h4>
                </div>
                <div class="modal-body">

                    <div id="AmtGridTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Invoice Number</th>
                                <th>Invoice Date</th>
                                <th>Invoice Amount</th>
                                <th>Unpaid Amount</th>
                            </tr>
                        </table>
                    </div>
                    <div class="clearfix row" style="padding-top: 5px">
                        <div class="col-sm-4">
                            <label>Total Amount</label>
                            <div>
                                <input type="text" id="btnfootTotalAmt" title="Total Amount" disabled="disabled" /></div>
                        </div>
                        <div class="col-sm-4">
                            <label>Unpaid Amount</label>
                            <div>
                                <input type="text" id="btnfootTotalAmtUnpaid" title="Unpaid Amount" disabled="disabled" /></div>
                        </div>
                    </div>

                    <%--<input type="text" id="btnfootTotalAmtUnpaid"  title="Unpaid Amount"/>--%>
                </div>
                <div class="modal-footer">

                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnPInvAmtDetailssettings" runat="server" />
    <asp:HiddenField ID="hdnvendortotalamt" runat="server" />
    <asp:HiddenField ID="hdnvendortotalamtUnpaid" runat="server" />
    <asp:HiddenField ID="hdnForBranchTaggingPurchase" runat="server" />

    <%-- Rev 3.0--%>
    <asp:HiddenField runat="server" ID="hdnIsDuplicateItemAllowedOrNot" />
    <%-- Rev 3.0 End--%>
</asp:Content>
