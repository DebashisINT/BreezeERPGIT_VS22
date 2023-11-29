<%--/*************************************************************************************************************************************************
 *  Rev 1.0     Sanchita    V2.0.41     28-11-2023      Data Freeze Required for Project Sale Invoice & Project Purchase Invoice. Mantis:26854
 *************************************************************************************************************************************************/--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ProjectPurchaseInvoice.aspx.cs" Inherits="ERP.OMS.Management.Activities.ProjectPurchaseInvoice" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/VendorBillingShipping.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/ProjectTermsConditions.ascx" TagPrefix="ucProject" TagName="ProjectTermsConditionsControl" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/ucImportPurchase_BillOfEntry.ascx" TagPrefix="uc1" TagName="ucImportPurchase_BillOfEntry" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.8.2/css/all.css" integrity="sha384-oS3vJWv+0UjzBfQzYUhtDYW+Pj2yciDJxpsK1OYPAYjqT085Qq/1cq5FLXAZQ7Ay" crossorigin="anonymous">
    <script src="../../../assests/pluggins/choosen/choosen.min.js"></script>
    <%--<script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>--%>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelPurchase.js?var=1.0"></script>
    <style type="text/css">
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

        #grid_DXStatus span > a {
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

        #grid_DXMainTable > tbody > tr > td:last-child {
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
    </style>

    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <%--Use for set focus on UOM after press ok on UOM--%>
    <script>
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
        $(function () {
            $('#UOMModal').on('hide.bs.modal', function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 7);
            });
        });


        function deleteTax(Action, srl, productid) {
            var OtherDetail = {};
            OtherDetail.Action = Action;
            OtherDetail.srl = srl;
            OtherDetail.prodid = productid;


            $.ajax({
                type: "POST",
                url: "ProjectPurchaseInvoice.aspx/taxUpdatePanel_Callback",
                data: JSON.stringify(OtherDetail),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var Code = msg.d;

                    if (Code != null) {

                    }
                }
            });
        }


        function ShowTCS() {

            $("#tcsModal").modal('show');
        }

        // Mantis Issue 24274
        function ShowTDS() {


            var count = grid.GetVisibleRowsOnPage();
            var totalAmount = 0;
            var totaltxAmount = 0;
            var totalQuantity = 0;
            var netAmount = 0;

            for (var i = 0; i < count + 10; i++) {
                if (grid.GetRow(i)) {
                    if (grid.GetRow(i).style.display != "none") {
                        grid.batchEditApi.StartEdit(i, 2);
                        totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2);
                        totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("Quantity").GetValue(), 4);
                        if (grid.GetEditor("TaxAmount").GetValue() != null) {
                            totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2);
                            grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                        }
                        else {
                            grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                        }

                        netAmount = netAmount + DecimalRoundoff(grid.GetEditor("TotalAmount").GetValue(), 2);
                    }
                }
            }

            for (i = -1; i > -count - 10; i--) {
                if (grid.GetRow(i)) {
                    if (grid.GetRow(i).style.display != "none") {
                        grid.batchEditApi.StartEdit(i, 2);
                        totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2);
                        totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("Quantity").GetValue(), 4);
                        if (grid.GetEditor("TaxAmount").GetValue() != null) {
                            totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2);
                            grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2), 2))

                        }
                        else {
                            grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                        }
                        netAmount = netAmount + DecimalRoundoff(grid.GetEditor("TotalAmount").GetValue(), 2);

                    }
                }
            }




            var CustomerId = $("#hdnCustomerId").val();
            var invoice_id = "";
            var date = cPLQuoteDate.GetText();

            var obj = {};
            obj.CustomerId = CustomerId;
            obj.invoice_id = "";
            obj.date = date;
            obj.totalAmount = netAmount;
            obj.taxableAmount = totalAmount;
            obj.branch_id = $("#ddl_Branch").val();
            obj.tds_code = ctxtTDSSection.GetValue();
            if (invoice_id == "" || invoice_id == null) {
                $.ajax({
                    type: "POST",
                    url: 'ProjectPurchaseInvoice.aspx/getTDSDetails',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(obj),
                    success: function (msg) {

                        if (msg) {
                            var response = msg.d;

                            ctxtTDSapplAmount.SetText(response.tds_amount);
                            ctxtTDSpercentage.SetText(response.Rate);
                            ctxtTDSAmount.SetText(response.Amount);
                            cGridTDSdocs.PerformCallback();
                        }


                    }
                });
            }
            else {
                cGridTDSdocs.PerformCallback();
            }

            $("#tdsModal").modal('show');
        }

        function CalcTDSAmount() {
            var applAmt = parseFloat(ctxtTDSapplAmount.GetText())
            var perAmt = parseFloat(ctxtTDSpercentage.GetText())

            var tcsAmount = DecimalRoundoff(parseFloat(applAmt) * parseFloat(perAmt) * 0.01, 2);
            ctxtTDSAmount.SetValue(tcsAmount);

        }

        function TDSsectionchanged(s, e) {
            ShowTDS();
        }

        // End of Mantis Issue 24274

        function CalcTCSAmount() {
            var applAmt = parseFloat(ctxtTCSapplAmount.GetText())
            var perAmt = parseFloat(ctxtTCSpercentage.GetText())

            var tcsAmount = DecimalRoundoff(parseFloat(applAmt) * parseFloat(perAmt) * 0.01, 2);
            ctxtTCSAmount.SetValue(tcsAmount);

        }



        function saveRetention() {

            if (parseFloat(crtxtRetAmount.GetValue()) > 0) {
                if (cbtnGL.GetText() == "") {
                    jAlert('Please select a valid GL to proceed.', 'Alert');
                }
                else {
                    $("#newModal").modal('hide');
                }
            }
            if (cbtnGL.GetText() != "") {
                if (parseFloat(crtxtRetAmount.GetValue()) == 0) {
                    jAlert('Please enter amount to proceed.', 'Alert');
                }
                else {
                    $("#newModal").modal('hide');
                }
            }
            else {
                $("#newModal").modal('hide');
            }
        }
        function MainAccountNewkeydownRO(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSearchRO").val();
            OtherDetails.branchId = $("#ddl_Branch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSearchRO").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                HeaderCaption.push("Subledger Type");
                HeaderCaption.push("Reverse Applicable");
                HeaderCaption.push("HSN/SAC");

                //callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBank", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
                callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountCashBankByProcedure", OtherDetails, "MainAccountTableRO", HeaderCaption, "MainAccountIndexRO", "SetMainAccountRO");


            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountIndexRO=0]"))
                    $("input[MainAccountIndexRO=0]").focus();
            }
            else if (e.code == "Escape") {
                //  
                $('#MainAccountModelRO').modal('hide');

            }
        }

        function SetAmountandPercentage(s, e) {

            if (s.name == "crtxtRetPercentage") {
                if (s.GetValue() > 0) {
                    var per = s.GetValue();
                    var Amount = DecimalRoundoff((parseFloat(ctxtTotAmount.GetValue()) * parseFloat(per)) / 100, 2);
                    crtxtRetAmount.SetValue(Amount);
                }
            }
            else {
                if (s.GetValue() > 0) {
                    var amount = s.GetValue();
                    var per = DecimalRoundoff((parseFloat(amount) / parseFloat(ctxtTotAmount.GetValue())) * 100, 2);
                    crtxtRetPercentage.SetValue(per);
                }
            }

        }


        function MainAccountButnClickRO(s, e) {
            if (e.buttonIndex == 0) {
                var txt = "<table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\" ><th>Main Account Name</th><th>Subledger Type</th><th>Reverse Applicable</th><th>HSN/SAC</th></tr><table>";
                document.getElementById("MainAccountTableRO").innerHTML = txt;
                $('#MainAccountModelRO').modal('show');


            }
        }


    </script>
    <%--Use for set focus on UOM after press ok on UOM--%>
    <script src="JS/SearchPopup.js"></script>

    <%--Batch Product Popup Start--%>

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
    <script type="text/javascript">

        //document.onkeydown = function (e) { 
        //  //  console.log(e);
        //    if (e.ctrlKey && (e.key == 's' || e.key == 'S'))
        //    {
        //        console.log('save not');
        //        if (cPopup_Empcitys.IsVisible()) {
        //            console.log('save');
        //            cbtnSave_citys.DoClick();
        //        }
        //        return false;
        //    }

        //    if (e.ctrlKey && (e.key == 'a' || e.key == 'A')) { 
        //        if (!cPopup_Empcitys.IsVisible()) {
        //            console.log('new');
        //            fn_PopOpen();
        //            return false;
        //        }

        //    }

        //    //if (event.keyCode == 17) isCtrl = true;
        //    //if (event.keyCode == 83 && isCtrl == true) { 
        //    //    console.log(e);
        //    //    return false;

        //    //} 

        //}






        //Added for lite popup



        // Rev 1.0
        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= cPLQuoteDate.GetDate()) && (cPLQuoteDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between  " + $("#hdnDatafrom").val() + " to " + $("#hdnDatato").val());
            }
        }
        // End of Rev 1.0


        function SizeUOMChange() {

            $("#covergaeUOM").val($("#SizeUOM").val());
            var first = ctxtPackingQty.GetValue();
            var second = ctxtpacking.GetValue();

            if (ctxtpackingSaleUom.GetText() == "") {
                first = 0;
            }

            if (ccmbPackingUomPro.GetText() == "") {
                second = 0;
            }



            if ($("#SizeUOM").val() == "1") {
                var cov = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText() * parseFloat(first));
                var vol = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText()) * parseFloat(ctxtThickness.GetText() * parseFloat(first));
            }
            else {
                var cov = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText() * parseFloat(second));
                var vol = parseFloat(ctxtHeight.GetText()) * parseFloat(ctxtWidth.GetText()) * parseFloat(ctxtThickness.GetText() * parseFloat(second));
            }

            if (cov == 0) {
                $("#txtCoverage").attr("disabled", false);
            }
            else {
                $("#txtCoverage").attr("disabled", true);
            }

            $("#txtCoverage").val(cov);
            $("#txtVolumn").val(vol);
            $("#dvCovg").text(cddlSize.GetText() + '²');
            $("#dvvolume").text(cddlSize.GetText() + '³');

        }















        //*************************************************  SI Main Account  *********************************************************************************

        function MainAccountButnClick() {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
            $("#txtMainAccountSearch").val("");
            document.getElementById("MainAccountTable").innerHTML = txt;
            cMainAccountModelSI.Show();
        }

        function MainAccountNewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                callonServer("ProjectPurchaseInvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountSIIndex", "SetMainAccountSI");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountSIIndex=0]"))
                    $("input[MainAccountSIIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cMainAccountModelSI.Hide();
                cMainAccountModelSI.Focus();
            }
        }

        function SetMainAccountSI(Id, name, e) {
            $("#hdnSIMainAccount").val(Id);
            cSIMainAccount.SetText(name);
            cSIMainAccount_active.SetText(name);
            cMainAccountModelSI.Hide();
        }

        //************************************************* End SI Main Account  *********************************************************************************



        //*************************************************  SR Main Account  *********************************************************************************

        function SRMainAccountButnClick() {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
            document.getElementById("MainAccountTableSR").innerHTML = txt;
            $("#txtMainAccountSRSearch").val("");
            cMainAccountModelSR.Show();
        }
        function MainAccountSRNewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSRSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSRSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                callonServer("ProjectPurchaseInvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTableSR", HeaderCaption, "MainAccountSRIndex", "SetMainAccountSR");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountSRIndex=0]"))
                    $("input[MainAccountSRIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cMainAccountModelSR.Hide();
            }
        }
        function SetMainAccountSR(Id, name, e) {
            $("#hdnSRMainAccount").val(Id);
            cSRMainAccount.SetText(name);
            cSRMainAccount_active.SetText(name)
            cMainAccountModelSR.Hide();
        }

        //************************************************* End SR Main Account  *********************************************************************************


        //*************************************************  PI Main Account  *********************************************************************************

        function PIMainAccountButnClick() {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
            document.getElementById("MainAccountTablePI").innerHTML = txt;
            $("#txtMainAccountPISearch").val("");
            cMainAccountModelPI.Show();
        }

        function MainAccountPINewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountPISearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountPISearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                callonServer("ProjectPurchaseInvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTablePI", HeaderCaption, "MainAccountPIIndex", "SetMainAccountPI");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountPIIndex=0]"))
                    $("input[MainAccountPIIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cMainAccountModelPI.Hide();
            }
        }
        function SetMainAccountPI(Id, name, e) {
            $("#hdnPIMainAccount").val(Id);
            cPIMainAccount.SetText(name);
            cPIMainAccount_active.SetText(name);
            cMainAccountModelPI.Hide();
        }

        //************************************************* End PI Main Account  *********************************************************************************



        //*************************************************  PR Main Account  *********************************************************************************

        function PRMainAccountButnClick() {
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Main Account Name</th></tr><table>";
            document.getElementById("MainAccountTablePR").innerHTML = txt;
            $("#txtMainAccountPRSearch").val("");
            cMainAccountModelPR.Show();
        }

        function MainAccountPRNewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountPRSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountPRSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                callonServer("ProjectPurchaseInvoice.aspx/GetMainAccount", OtherDetails, "MainAccountTablePR", HeaderCaption, "MainAccountPRIndex", "SetMainAccountPR");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountPRIndex=0]"))
                    $("input[MainAccountPRIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                cMainAccountModelPR.Hide();
            }
        }
        function SetMainAccountPR(Id, name, e) {
            $("#hdnPRMainAccount").val(Id);
            cPRMainAccount.SetText(name);
            cPRMainAccount_active.SetText(name);
            cMainAccountModelPR.Hide();
        }

        //************************************************* End PR Main Account  *********************************************************************************

        function MainAccountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumEnter") {
                s.OnButtonClick(0);
            }
        }

        function ValueSelected(e, indexName) {
            if (indexName == "MainAccountSIIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnSIMainAccount").val(Code);
                    cSIMainAccount.SetText(name);
                    cSIMainAccount_active.SetText(name);
                    cMainAccountModelSI.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountSearch').focus();
                    }
                }

            }

            else if (indexName == "MainAccountSRIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnSRMainAccount").val(Code);
                    cSRMainAccount.SetText(name);
                    cSRMainAccount_active.SetText(name)
                    cMainAccountModelSR.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountSRSearch').focus();
                    }
                }

            }


            else if (indexName == "MainAccountPIIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnPIMainAccount").val(Code);
                    cPIMainAccount.SetText(name);
                    cPIMainAccount_active.SetText(name);
                    cMainAccountModelPI.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountPISearch').focus();
                    }
                }

            }
            else if (indexName == "MainAccountPRIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnPRMainAccount").val(Code);
                    cPRMainAccount.SetText(name);
                    cPRMainAccount_active.SetText(name);
                    cMainAccountModelPR.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountPRSearch').focus();
                    }
                }

            }
        }




        //End lite Pop up

        var PurReturnOldValue = '';
        function cmbPurReturnGotFocus(s, e) {
            PurReturnOldValue = s.GetValue();
        }

        function mainAccountPurReturn(s, e) {
            for (var i = 0; i < mainAccountInUse.length; i++) {
                if (mainAccountInUse[i] == 'purchaseReturn') {

                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    s.SetValue(PurReturnOldValue);
                }
            }
        }


        var PurInvoiceOldValue = '';
        function cmbPurInvoiceGotFocus(s, e) {
            PurInvoiceOldValue = s.GetValue();
        }

        function mainAccountPurInvoice(s, e) {

            for (var i = 0; i < mainAccountInUse.length; i++) {

                if (mainAccountInUse[i] == 'purchaseInvoice') {
                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    s.SetValue(PurInvoiceOldValue);
                }

            }

        }

        var salesReturnOldValue = '';
        function cmbsalesReturnGotFocus(s, e) {
            salesReturnOldValue = s.GetValue();
        }

        function mainAccountSalesReturn(s, e) {
            for (var i = 0; i < mainAccountInUse.length; i++) {
                if (mainAccountInUse[i] == 'salesReturn') {
                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    s.SetValue(salesReturnOldValue);
                }
            }
        }


        var salesInvoiceOldValue = '';
        function cmbsalesInvoiceGotFocus(s, e) {
            salesInvoiceOldValue = s.GetValue();
        }

        function mainAccountSalesInvoice(s, e) {
            for (var i = 0; i < mainAccountInUse.length; i++) {
                if (mainAccountInUse[i] == 'SalesInvoice') {

                    jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                    s.SetValue(salesInvoiceOldValue);
                }
            }
        }


        $(function () {
            var vAnotherKeyWasPressed = false;
            var ALT_CODE = 18;

            //When some key is pressed
            $(window).keydown(function (event) {
                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                vAnotherKeyWasPressed = vKey != ALT_CODE;
                if (!LoadingPanel.IsVisible()) {
                    if (event.altKey && (event.key == 's' || event.key == 'S')) {
                        console.log('save not');
                        if (cPopup_Empcitys.IsVisible()) {
                            if (cbtnSave_citys.IsVisible())
                                cbtnSave_citys.DoClick();
                        }
                        return false;
                    }

                    if (event.altKey && (event.key == 'a' || event.key == 'A')) {
                        if (!cPopup_Empcitys.IsVisible()) {
                            if (document.getElementById('AddBtn') != null) {
                                console.log('new');
                                fn_PopOpen();
                                return false;
                            }

                        }

                    }

                    if (event.altKey && (event.key == 'c' || event.key == 'C')) {
                        console.log('save not');
                        if (cPopup_Empcitys.IsVisible()) {
                            fn_btnCancel();
                        }
                        return false;
                    }
                }
            });

            //When some key is left
            $(window).keyup(function (event) {

                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            });
        });



        function SetHSnPanelEndCallBack() {
            LoadingPanel.Hide();
            if (cSetHSnPanel.cpHsnCode) {
                if (cSetHSnPanel.cpHsnCode != "") {
                    //cHsnLookUp.gridView.SelectItemsByKey(cSetHSnPanel.cpHsnCode);
                    cSetHSnPanel.cpHsnCode = null;
                } else {
                    cHsnLookUp.Clear();
                }
            }
        }
        function CmbProClassCodeChanged(s, e) {
            if (s.GetValue() != null) {
                cSetHSnPanel.PerformCallback(s.GetValue());
                LoadingPanel.SetText('Please wait searching HSN...');
                LoadingPanel.Show();
            }

        }
        function cCmbTradingLotUnitsLostFocus() {
            var saleUomVal = cCmbTradingLotUnits.GetValue();
            if (saleUomVal == null) {
                $('#btnPackingConfig').attr('disabled', 'disabled');
            }
            else {
                $('#btnPackingConfig').attr('disabled', false);
            }
        }

        function ShowTdsSection() {
            ctdsPopup.Show();
        }

        function ShowPackingDetails() {
            $('#invalidPackingUom').css({ 'display': 'none' });
            ctxtpackingSaleUom.SetText(cCmbTradingLotUnits.GetText());
            ctxtpackingSaleUom.SetEnabled(false);
            cpackingDetails.Show();
        }

        function PackingDetailsOkClick() {

            $('#invalidPackingUom').css({ 'display': 'none' });

            if (ccmbPackingUomPro.GetValue() == null) {
                $('#invalidPackingUom').css({ 'display': 'block' });
            } else {
                cpackingDetails.Hide();
            }
        }

        function ServicetaxOkClick() {
            cServiceTaxPopup.Hide();
        }
        function isInventoryChanged(s, e) {
            //changeControlStateWithInventory(s.GetValue());
            changeControlStateWithInventory();
        }
        function isCapitalChanged(s, e) {

            var Inv = ccmbIsInventory.GetValue();
            var cap = ccmbIsCapitalGoods.GetValue();
            if (Inv != 1) {
                if (cap == 1) {
                    cCmbProType.SetEnabled(true);
                    cCmbStockValuation.SetEnabled(true);
                    //HsnChange
                    //   caspxHsnCode.SetEnabled(true);
                    // cHsnLookUp.SetEnabled(true);
                    ctxtQuoteLot.SetEnabled(true);
                    cCmbQuoteLotUnit.SetEnabled(true);
                    ctxtTradingLot.SetEnabled(true);
                    cCmbTradingLotUnits.SetEnabled(true);
                    ctxtDeliveryLot.SetEnabled(true);
                    cCmbDeliveryLotUnit.SetEnabled(true);
                    ccmbStockUom.SetEnabled(true);
                    ctxtMinLvl.SetEnabled(true);
                    ctxtReorderLvl.SetEnabled(true);
                    ccmbNegativeStk.SetEnabled(true);


                    $('#btnBarCodeConfig').attr('disabled', false);
                    $('#btnProdConfig').attr('disabled', false);

                    $('#btnServiceTaxConfig').attr('disabled', 'disabled');
                    cAspxServiceTax.SetValue('');

                    $('#btnTDS').attr('disabled', 'disabled');
                    cmb_tdstcs.SetValue('');
                }
                else {
                    cCmbProType.SetText('');
                    cCmbProType.SetEnabled(false);

                    cCmbStockValuation.SetValue('A');
                    cCmbStockValuation.SetEnabled(false);

                    //caspxHsnCode.SetText('');
                    //caspxHsnCode.SetEnabled(false);
                    // cHsnLookUp.SetEnabled(false);
                    // cHsnLookUp.Clear();

                    ctxtQuoteLot.SetText('0');
                    ctxtQuoteLot.SetEnabled(false);

                    cCmbQuoteLotUnit.SetText('0');
                    cCmbQuoteLotUnit.SetEnabled(false);

                    ctxtTradingLot.SetText('0');
                    ctxtTradingLot.SetEnabled(false);

                    cCmbTradingLotUnits.SetText('');
                    cCmbTradingLotUnits.SetEnabled(false);

                    ctxtDeliveryLot.SetText('0');
                    ctxtDeliveryLot.SetEnabled(false);

                    cCmbDeliveryLotUnit.SetText('');
                    cCmbDeliveryLotUnit.SetEnabled(false);


                    ccmbStockUom.SetText('');
                    ccmbStockUom.SetEnabled(false);

                    ctxtMinLvl.SetText('0');
                    ctxtMinLvl.SetEnabled(false);

                    ctxtReorderLvl.SetText('0');
                    ctxtReorderLvl.SetEnabled(false);

                    ccmbNegativeStk.SetValue('I');
                    ccmbNegativeStk.SetEnabled(false);

                    //Product Configuration
                    $('#btnProdConfig').attr('disabled', 'disabled');
                    $('#btnBarCodeConfig').attr('disabled', 'disabled');
                    $('#btnServiceTaxConfig').attr('disabled', false);
                    $('#btnTDS').attr('disabled', false);
                    $('#btnPackingConfig').attr('disabled', 'disabled');
                }
            }
        }


        function changeControlStateWithInventory(obj) {
            obj = ccmbIsInventory.GetValue();
            if (obj == 1) {
                cCmbProType.SetEnabled(true);
                cCmbStockValuation.SetEnabled(true);
                //HsnChange
                //   caspxHsnCode.SetEnabled(true);
                // cHsnLookUp.SetEnabled(true);
                ctxtQuoteLot.SetEnabled(true);
                cCmbQuoteLotUnit.SetEnabled(true);
                ctxtTradingLot.SetEnabled(true);
                cCmbTradingLotUnits.SetEnabled(true);
                ctxtDeliveryLot.SetEnabled(true);
                cCmbDeliveryLotUnit.SetEnabled(true);
                ccmbStockUom.SetEnabled(true);
                ctxtMinLvl.SetEnabled(true);
                ctxtReorderLvl.SetEnabled(true);
                ccmbNegativeStk.SetEnabled(true);
                ccmbServiceItem.SetValue('0');
                ccmbServiceItem.SetEnabled(false);

                $('#btnBarCodeConfig').attr('disabled', false);
                $('#btnProdConfig').attr('disabled', false);

                $('#btnServiceTaxConfig').attr('disabled', 'disabled');
                cAspxServiceTax.SetValue('');

                $('#btnTDS').attr('disabled', 'disabled');
                cmb_tdstcs.SetValue('');

            } else {
                cCmbProType.SetText('');
                cCmbProType.SetEnabled(false);

                cCmbStockValuation.SetValue('A');
                cCmbStockValuation.SetEnabled(false);

                //caspxHsnCode.SetText('');
                //caspxHsnCode.SetEnabled(false);
                // cHsnLookUp.SetEnabled(false);
                // cHsnLookUp.Clear();

                ctxtQuoteLot.SetText('1');
                ctxtQuoteLot.SetEnabled(false);

                cCmbQuoteLotUnit.SetText('');
                cCmbQuoteLotUnit.SetEnabled(false);

                ctxtTradingLot.SetText('1');
                ctxtTradingLot.SetEnabled(false);

                cCmbTradingLotUnits.SetText('');
                cCmbTradingLotUnits.SetEnabled(false);

                ctxtDeliveryLot.SetText('1');
                ctxtDeliveryLot.SetEnabled(false);

                cCmbDeliveryLotUnit.SetText('');
                cCmbDeliveryLotUnit.SetEnabled(false);

                ccmbStockUom.SetText('');
                ccmbStockUom.SetEnabled(false);

                ctxtMinLvl.SetText('0');
                ctxtMinLvl.SetEnabled(false);

                ctxtReorderLvl.SetText('0');
                ctxtReorderLvl.SetEnabled(false);


                ccmbServiceItem.SetEnabled(true);

                ccmbNegativeStk.SetValue('I');
                ccmbNegativeStk.SetEnabled(false);

                //Product Configuration
                $('#btnProdConfig').attr('disabled', 'disabled');
                $('#btnBarCodeConfig').attr('disabled', 'disabled');
                $('#btnServiceTaxConfig').attr('disabled', false);
                $('#btnTDS').attr('disabled', false);
                $('#btnPackingConfig').attr('disabled', 'disabled');
            }
        }
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=Prd&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code
        //Declare some global variable for poopUP
        var barCodeType;
        var BarCode, GlobalCode;
        var taxCodeSale, taxCodePur, taxScheme;
        var autoApply;
        var ProdColor, ProdSize, ColApp, SizeApp;
        var tdsValue = '';
        //Declare some global variable for poopUP End Here

        //Close the particular popup on esc
        function OnInitTax(s, e) {
            ASPxClientUtils.AttachEventToElement(window.document, "keydown", function (evt) {
                if (evt.keyCode == ASPxClientUtils.StringToShortcutCode("ESCAPE"))
                    cTaxCodePopup.Hide();
            });
        }
        function OnInitBarCode(s, e) {
            ASPxClientUtils.AttachEventToElement(window.document, "keydown", function (evt) {
                if (evt.keyCode == ASPxClientUtils.StringToShortcutCode("ESCAPE"))
                    cBarCodePopUp.Hide();
            });
        }
        function OnInitProductAttribute(s, e) {
            ASPxClientUtils.AttachEventToElement(window.document, "keydown", function (evt) {
                if (evt.keyCode == ASPxClientUtils.StringToShortcutCode("ESCAPE"))
                    cproductAttributePopUp.Hide();
            });
        }

        //for Image Upload

        function OnUploadComplete(args) {
            console.log(args.callbackData);
            document.getElementById('fileName').value = args.callbackData;
            // cProdImage.SetImageUrl(args.callbackData);
            afterFileUpload();

        }

        function uploadClick() {
            Callback1.PerformCallback('');
        }


        function onFileUploadStart(s, e) {
            uploadInProgress = true;
            uploadErrorOccurred = false;
        }
        //Image upload end here


        //code added by debjyoti 04-01-2017
        function ShowProductAttribute() {

            cproductAttributePopUp.Show();
        }

        function productAttributeOkClik() {
            //Surojit 04-03-2019
            var ismandatory = cchkIsMandatory.GetValue();
            var textvalue = GridLookup_I.value;
            if (ismandatory && (textvalue == "" || textvalue == null)) {
                jAlert("Please select atleast one components!");
                return false;
            }
            else {

                ProdSize = cCmbProductSize.GetValue();
                ProdColor = cCmbProductColor.GetValue();
                ColApp = RrdblappColor.GetSelectedIndex();
                SizeApp = Rrdblapp.GetSelectedIndex();




                cproductAttributePopUp.Hide();
            }
            //Surojit 04-03-2019
        }


        function ShowBarCode() {
            cBarCodePopUp.Show();
        }
        function BarCodeOkClick() {
            barCodeType = cCmbBarCodeType.GetSelectedIndex();
            BarCode = ctxtBarCodeNo.GetText();
            GlobalCode = ctxtGlobalCode.GetText();
            cBarCodePopUp.Hide();
        }

        function ShowTaxCode() {
            cTaxCodePopup.Show();
        }

        function ShowServiceTax() {
            cServiceTaxPopup.Show();
        }


        function taxCodeOkClick() {
            taxCodeSale = cCmbTaxCodeSale.GetValue();
            taxCodePur = cCmbTaxCodePur.GetValue();
            autoApply = cChkAutoApply.GetChecked();
            taxScheme = cCmbTaxScheme.GetValue();
            cTaxCodePopup.Hide();
        }

        function GetCheckBoxValue(value) {
            //var value = s.GetChecked();
            if (value == true) {
                cCmbTaxCodePur.SetValue(0);
                cCmbTaxCodePur.SetEnabled(false);

                cCmbTaxCodeSale.SetValue(0);
                cCmbTaxCodeSale.SetEnabled(false);

                cCmbTaxScheme.SetEnabled(true);

            } else {
                cCmbTaxScheme.SetValue(0);
                cCmbTaxScheme.SetEnabled(false);
                cCmbTaxCodePur.SetEnabled(true);
                cCmbTaxCodeSale.SetEnabled(true);
            }
        }

        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
        }
        //changes end here 04-01-2017


        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;
            var keychar = String.fromCharCode(key);
            if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8 || key == 46) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
                return;
            }
            var regex = /[0-9\b]/;

            if (!regex.test(keychar)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }
        }


        function fn_PopOpen() {
            //Surojit
            if ($('#hdnProductMasterComponentMandatoryVisible').val() == "0") {
                $('#divProductMasterComponentMandatory').hide();
            }
            else {
                $('#divProductMasterComponentMandatory').show();
            }
            //Surojit

            cbtnSave_citys.SetVisible(true);
            mainAccountInUse = [];
            //ccmbsalesInvoice.SetSelectedIndex(0); SI
            //ccmbSalesReturn.SetSelectedIndex(0);
            //ccmbPurInvoice.SetSelectedIndex(0);
            //ccmbPurReturn.SetSelectedIndex(0);

            $("#hdnSIMainAccount").val("");
            $("#hdnSRMainAccount").val("");
            $("#hdnPIMainAccount").val("");
            $("#hdnPRMainAccount").val("");

            cSIMainAccount.SetText("");
            cSRMainAccount.SetText("");
            cPIMainAccount.SetText("");
            cPRMainAccount.SetText("");







            cSIMainAccount.SetEnabled(true);
            cSRMainAccount.SetEnabled(true);
            cPIMainAccount.SetEnabled(true);
            cPRMainAccount.SetEnabled(true);


            document.getElementById('Keyval_internalId').value = 'Add';
            //document.getElementById('btnUdf').disabled =true;

            cPopup_Empcitys.SetHeaderText('Add Products');
            document.getElementById('hiddenedit').value = "";
            ctxtPro_Code.SetText('');
            ctxtPro_Name.SetText('');
            ctxtPro_Description.SetText('');
            cCmbProType.SetSelectedIndex(-1);
            cCmbProClassCode.SetSelectedIndex(-1);
            ctxtGlobalCode.SetText('');
            ctxtTradingLot.SetText('');
            cCmbTradingLotUnits.SetSelectedIndex(-1);
            cCmbQuoteCurrency.SetSelectedIndex(-1);
            ctxtQuoteLot.SetText('');
            cCmbQuoteLotUnit.SetSelectedIndex(-1);
            ctxtDeliveryLot.SetText('');
            cCmbDeliveryLotUnit.SetSelectedIndex(-1);
            cCmbProductColor.SetSelectedIndex(0);
            cCmbProductSize.SetSelectedIndex(0);
            RrdblappColor.SetSelectedIndex(0);
            Rrdblapp.SetSelectedIndex(0);
            gridLookup.Clear();
            //Debjyoti Code Added:30-12-2016
            //Reason: Barcode Type and No
            cCmbBarCodeType.SetSelectedIndex(-1);
            barCodeType = -1;
            BarCode = "";
            GlobalCode = "";
            ctxtBarCodeNo.SetText('');


            //End Debjyoti 30-12-2016
            taxCodeSale = 0;
            taxCodePur = 0;
            taxScheme = 0;
            autoApply = false;

            ProdColor = 0;
            ProdSize = 0;
            ColApp = 0;
            SizeApp = 0;
            //Debjyoti 04-01-2017
            ccmbIsInventory.SetSelectedIndex(0);
            cCmbStockValuation.SetSelectedIndex(1);
            ctxtSalePrice.SetText('');
            ctxtMinSalePrice.SetText('');
            ctxtPurPrice.SetText('');
            ctxtMrp.SetText('');
            ccmbStockUom.SetSelectedIndex(-1);
            ctxtMinLvl.SetText('');
            ctxtReorderLvl.SetText('');
            ctxtReorderQty.SetText('');
            ctxtMaxLvl.SetText('');


            ctxtHeight.SetText('0.00');
            ctxtWidth.SetText('0.00');
            ctxtThickness.SetText('0.00');
            cddlSize.SetSelectedIndex(0);
            $("#SizeUOM").val('1');
            ctxtSeries.SetText('');
            ctxtFinish.SetText('');
            ctxtLeadtime.SetText('0');
            $("#txtCoverage").val('0.00');
            $("#dvCovg").text('');
            $("#txtVolumn").val('0');
            $("#volumeuom").text('');
            ctxtWeight.SetText('0');
            ctxtSubCat.SetText('');
            ctxtPro_Printname.SetText('');

            ccmbNegativeStk.SetSelectedIndex(0);
            cCmbTaxCodeSale.SetSelectedIndex(0);
            cCmbTaxCodePur.SetSelectedIndex(0);
            cCmbTaxScheme.SetSelectedIndex(0);
            cChkAutoApply.SetChecked(false);
            GetCheckBoxValue(false);
            document.getElementById('fileName').value = '';
            cProdImage.SetImageUrl('');
            upload1.ClearText();
            //  gridLookup.SetValue(0);
            cCmbStatus.SetSelectedIndex(0);
            //caspxHsnCode.SetText('');
            cHsnLookUp.Clear();
            //Debjyoti 31-01-2017
            ctxtPro_Code.SetEnabled(true);
            ccmbIsInventory.SetEnabled(true);
            ccmbIsInventory.SetSelectedIndex(0);
            changeControlStateWithInventory();
            $('#reOrderError').css({ 'display': 'None' });
            $('#mrpError').css({ 'display': 'None' });
            cAspxServiceTax.SetValue('');
            $('#btnPackingConfig').attr('disabled', 'disabled');

            //packing details
            ctxtPackingQty.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default
            ctxtpacking.SetValue(1); //0020190: When creating a new Product, in the "Configure UOM conversion" the Quantity and Alt. Quantity will be "1" by default

            cchkOverideConvertion.SetChecked(false); //Surojit 08-02-2018
            cchkIsMandatory.SetChecked(false); //Surojit 11-02-2018

            //ccmbPackingUomPro.SetSelectedIndex(-1);
            //packing details End Here
            caspxInstallation.SetValue('0');
            ccmbBrand.SetValue('');
            cmb_tdstcs.SetValue('');
            cPopup_Empcitys.SetWidth(window.screen.width - 50);
            //cPopup_Empcitys.SetHeight(window.innerHeight.height - 70);
            cPopup_Empcitys.Show();
            cHsnLookUp.SetEnabled(true);
            ctxtPro_Code.Focus();
            //ccmbStatusad.SetSelectedIndex(0);
        }
        function afterFileUpload() {
            if (document.getElementById('hiddenedit').value == '') {
                cgridprod.PerformCallback('savecity~' + GetObjectID('fileName').value);
            }
            else {
                cgridprod.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value + "~" + GetObjectID('fileName').value);
            }

        }


        function SaveActiveDormant() {
            var retval = true;
            var minlvl = (ctxtMinLvl_active.GetValue() != null) ? ctxtMinLvl_active.GetValue() : "0";
            var reordLvl = (ctxtReorderLvl_active.GetValue() != null) ? ctxtReorderLvl_active.GetValue() : "0";

            if ((parseFloat(reordLvl) != 0) || (parseFloat(minlvl) != 0)) {
                if ((parseFloat(reordLvl) <= parseFloat(minlvl)))     //|| (((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0)))
                {
                    $('#reOrderError1').css({ 'display': 'block' });
                    retval = false;
                }
            }
            else if ((((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0))) {
                $('#reOrderError1').css({ 'display': 'None' });
            }
            else {
                $('#reOrderError1').css({ 'display': 'None' });
            }
            if (retval == false) {
                return false
            }

            else {
                if (document.getElementById('hiddenedit').value != '') {
                    cgridprod.PerformCallback('updatecity_active~' + GetObjectID('hiddenedit').value);
                }
            }


        }

        function btnSave_citys() {
            var PackingUom = ccmbPackingUomPro.GetValue();
            var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
            var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
            var ServiceItem = ccmbServiceItem.GetValueString();
            if ((((PackingUom == "0" || PackingUom == "" || PackingUom == null)) && (ShowUOMConversionInEntry == "1")) && ccmbIsInventory.GetValue() == 1) {
                jAlert(' "Show UOM Conversion In Entry" is Activated.You must Select alternate UOM from Product Master - Configure UOM Coinversion');
                return false;
            }

            if (ccmbIsInventory.GetValue() == 0) {
                if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '')) {
                    if (upload1.GetText().trim() != '') {
                        upload1.Upload();
                    } else {
                        afterFileUpload();
                    }
                }
            } else {
                if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '') && (cCmbTradingLotUnits.GetText().trim() != '') && (cCmbDeliveryLotUnit.GetText().trim() != '') && (ccmbStockUom.GetValue() != null)) {

                    if (validReorder() && validMRP()) {
                        if (upload1.GetText().trim() != '') {
                            upload1.Upload();
                        } else {
                            afterFileUpload();
                        }
                    }
                }
            }




        }

        function validReorder() {
            var retval = true;
            var minlvl = (ctxtMinLvl.GetValue() != null) ? ctxtMinLvl.GetValue() : "0";
            var reordLvl = (ctxtReorderLvl.GetValue() != null) ? ctxtReorderLvl.GetValue() : "0";

            if ((parseFloat(reordLvl) != 0) || (parseFloat(minlvl) != 0)) {
                if ((parseFloat(reordLvl) <= parseFloat(minlvl)))     //|| (((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0)))
                {
                    $('#reOrderError').css({ 'display': 'block' });
                    retval = false;
                }
            }
            else if ((((parseFloat(ctxtMinLvl.GetValue())) == 0) && ((parseFloat(ctxtReorderLvl.GetValue())) == 0))) {
                $('#reOrderError').css({ 'display': 'None' });
            }
            else {
                $('#reOrderError').css({ 'display': 'None' });
            }
            return retval;
        }

        function validMRP() {
            var retval = true;
            var txtMinSalePrice = (ctxtMinSalePrice.GetValue() != null) ? ctxtMinSalePrice.GetValue() : "0";
            var txtMrp = (ctxtMrp.GetValue() != null) ? ctxtMrp.GetValue() : "0";

            if (parseFloat(txtMrp) != 0 && parseFloat(txtMrp) < parseFloat(txtMinSalePrice)) {
                $('#mrpError').css({ 'display': 'block' });
                retval = false;
            }
            else {
                $('#mrpError').css({ 'display': 'None' });
            }
            return retval;
        }

        function btnSave_citysOld() {

            var valiEmail = false;

            var validPhNo = false;

            var CheckUniqueCode = false;

            var reg = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
            if (reg.test(ctxtMarkets_Email.GetText())) {
                valiEmail = true;
            }

            if (!isNaN(ctxtMarkets_Phones.GetText()) && ctxtMarkets_Phones.GetText().length == 10) {
                validPhNo = true;
            }


            //for unique code ajax call
            var MarketsCode = ctxtMarkets_Code.GetText();
            $.ajax({
                type: "POST",
                url: "sMarkets.aspx/CheckUniqueCode",
                data: "{'MarketsCode':'" + MarketsCode + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //                    CheckUniqueCode = msg.d;

                    if (document.getElementById('hiddenedit').value == '') {
                        CheckUniqueCode = msg.d;
                    }
                    else {
                        CheckUniqueCode == false
                    }

                    if (CheckUniqueCode == false && ctxtMarkets_Code.GetText() != '' && ctxtMarkets_Name.GetText() != '' && (ctxtMarkets_Email.GetText() == '' || valiEmail == true) && (ctxtMarkets_Phones.GetText() == '' || validPhNo == true)) {
                        if (document.getElementById('hiddenedit').value == '') {
                            //alert("in add");
                            cgridprod.PerformCallback('savecity~');
                        }
                        else {
                            //alert("in update");
                            cgridprod.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value);
                        }
                    }
                    else if (CheckUniqueCode == true) {
                        jAlert('Please enter unique market code');
                        ctxtMarkets_Code.Focus();
                    }
                    else if (ctxtMarkets_Code.GetText() == '') {
                        jAlert('Please Enter Markets Code');
                        ctxtMarkets_Code.Focus();
                    }
                    else if (ctxtMarkets_Name.GetText() == '') {
                        jAlert('Please Enter Markets Name');
                        ctxtMarkets_Name.Focus();
                    }
                    else if (!reg.test(ctxtMarkets_Email.GetText())) {
                        jAlert('Please enter valid email');
                        ctxtMarkets_Email.Focus();
                    }
                    else if (isNaN(ctxtMarkets_Phones.GetText()) || ctxtMarkets_Phones.GetText().length != 10) {
                        jAlert('Please enter valid Phone No');
                        ctxtMarkets_Phones.Focus();
                    }

                }

            });
        }


        function fn_btnCancel() {
            cPopup_Empcitys.Hide();
            $("#txtPro_Code_EC, #txtPro_Name_EC, #txtQuoteLot_EC, #txtTradingLot_EC, #txtDeliveryLot_EC").hide();
        }

        function fn_btnCancel_active() {
            cPopup_Empcitys_active.Hide();
        }
        function fn_ViewProduct(keyValue) {
            /*-------------------------------------------------Arindam-----------------------------------------------------------*/

            var url = '/OMS/management/master/View/ViewProduct.html?v=0.07&&id=' + keyValue;

            CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
            CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
            CAspxDirectCustomerViewPopup.SetContentUrl(url);
            CAspxDirectCustomerViewPopup.RefreshContentUrl();
            CAspxDirectCustomerViewPopup.Show();




            /*-------------------------------------------------Arindam-----------------------------------------------------------*/
            //  fn_Editcity(keyValue);
            //  cbtnSave_citys.SetVisible(false);

        }

        function fn_Editcity(keyValue) {
            cbtnSave_citys.SetVisible(true);
            document.getElementById('btnUdf').disabled = false;
            cPopup_Empcitys.SetHeaderText('Modify Products');

            ctxtHeight.SetText('0.00');
            ctxtWidth.SetText('0.00');
            ctxtThickness.SetText('0.00');
            cddlSize.SetSelectedIndex(0);
            $("#SizeUOM").val('1');
            ctxtSeries.SetText('');
            ctxtFinish.SetText('');
            ctxtLeadtime.SetText('0');
            $("#txtCoverage").val('');
            $("#dvCovg").text('');
            $("#txtVolumn").val('0');
            $("#volumeuom").text('');
            ctxtWeight.SetText('0');
            ctxtSubCat.SetText('');
            ctxtPro_Printname.SetText('');


            cgridprod.PerformCallback('Edit~' + keyValue);
            document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
        }

        function fn_activeEdit(keyValue, status) {
            //if (status == 'A') {
            //    jAlert('Product is already active');
            //    return;
            //}
            // else {
            document.getElementById('HiddenField_status').value = '1';
            // cPopup_Empcitys_active.SetWidth(window.screen.width - 50);

            //cPopup_Empcitys_active.Show();

            cbtnSave_citys.SetVisible(true);
            document.getElementById('btnUdf').disabled = false;
            cPopup_Empcitys_active.SetHeaderText('Modify Products');
            cgridprod.PerformCallback('Active~' + keyValue);
            document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;

            // }


        }

        function fn_Deletecity(keyValue) {
            //if (confirm("Confirm Delete?")) {
            //    cgridprod.PerformCallback('Delete~' + keyValue);
            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cgridprod.PerformCallback('Delete~' + keyValue);
                }
            });
        }


        function componentEndCallBack(s, e) {
            console.log(e);
            // cPopup_Empcitys.Show();
        }

        var mainAccountInUse = [];
        function grid_EndCallBack() {

            if (cgridprod.cpinsert != null) {
                if (cgridprod.cpinsert == 'Success') {
                    jAlert('Saved Successfully');
                    //alert('Saved Successfully');
                    //................CODE  UPDATED BY sAM ON 18102016.................................................
                    ctxtPro_Name.GetInputElement().readOnly = false;
                    //................CODE ABOVE UPDATED BY sAM ON 18102016.................................................
                    cPopup_Empcitys.Hide();

                }
                else if (cgridprod.cpinsert == 'fail') {
                    jAlert("Error On Insertion \n 'Please Try Again!!'")
                }
                else if (cgridprod.cpinsert == 'UDFManddratory') {
                    jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });

                }
                else {
                    jAlert(cgridprod.cpinsert);
                    //cPopup_Empcitys.Hide();
                }
            }

        }
        function OnCmbCountryName_ValueChange() {
            cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
        }
        function CmbState_EndCallback() {
            cCmbState.SetSelectedIndex(0);
            cCmbState.Focus();
        }
        function OnCmbStateName_ValueChange() {
            cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());
        }
        function CmbCity_EndCallback() {
            cCmbCity.SetSelectedIndex(0);
            cCmbCity.Focus();
        }
        $(document).ready(function () {


            if ($("#hdnADDEditMode").val() != "ADD") {
                var dt = new Date();
                if ($("#hdnBackdateddate").val() != "0" && $("#hdnBackdateddate").val() != "") {
                    cPLQuoteDate.SetEnabled(true)
                    var Days = $("#hdnBackdateddate").val();
                    var today = cPLQuoteDate.GetDate();
                    var newdate = cPLQuoteDate.GetDate();
                    newdate.setDate(today.getDate() - Math.round(Days));
                    if ($("#hdnTagDateForbackdated").val() != "") {
                        if (new Date($("#hdnTagDateForbackdated").val()) > newdate) {
                            cPLQuoteDate.SetMinDate(new Date($("#hdnTagDateForbackdated").val()));
                            cPLQuoteDate.SetMaxDate(dt)
                        }
                        else {
                            cPLQuoteDate.SetMinDate(newdate);
                            cPLQuoteDate.SetMaxDate(dt);
                        }
                    }
                    else {
                        cPLQuoteDate.SetMinDate(newdate);
                        cPLQuoteDate.SetMaxDate(dt);
                    }
                }
            }


            $('.dxpc-closeBtn').click(function () {
                fn_btnCancel();
            });

            $('#newModal').on('shown.bs.modal', function () {

                crtxtName.SetText(ctxtVendorName.GetText());
                ShowOnClick();


            })

        });


        function ShowOnClick() {
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

            ctxtTotAmount.SetValue(sumNetAmount);
            crtxtRetPercentage.SetFocus();
        }







    </script>

    <script type="text/javascript">
        function fn_ctxtPro_Name_TextChanged(s, e) {
            var procode = 0;
            if (GetObjectID('hiddenedit').value != '') {
                procode = GetObjectID('hiddenedit').value;
            }

            //var ProductName = ctxtPro_Name.GetText();
            var ProductName = ctxtPro_Code.GetText().trim();
            $.ajax({
                type: "POST",
                url: "ProjectPurchaseInvoice.aspx/CheckUniqueNameProduct",
                //data: "{'ProductName':'" + ProductName + "'}",
                data: JSON.stringify({ ProductName: ProductName, procode: procode }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please enter unique name", "Alert", function () { ctxtPro_Code.SetFocus(); });
                        ctxtPro_Code.SetText("");
                        // ctxtPro_Code.SetFocus();
                        //document.getElementById("Popup_Empcitys_ctxtPro_Code_I").focus();
                        //  document.getElementById("txtPro_Code_I").focus();

                        return false;
                    }
                }

            });
        }
    </script>

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
            display: block;
        }

        .sText {
            font-size: 10px;
        }
    </style>


    <script type="text/javascript">
        function OnAddBusinessClick(keyValue) {
            var url = '../../master/AssignIndustry.aspx?id1=' + keyValue + '&EntType=product';
            window.location.href = url;
        }
        function PopupOpen(obj) {
            var URL = '/OMS/Management/Master/Product_Document.aspx?idbldng=' + obj + '&type=Product';
            //OnMoreInfoClick(URL, "Products Document Details", '1000px', '400px', "Y");
            //document.getElementById("marketscgridprod_DXPEForm_efnew_DXEditor1_I").focus();
            window.location.href = URL;
        }



        var KEYCODE_ENTER = 13;
        var KEYCODE_ESC = 27;



        $(document).keyup(function (e) {
            if (e.keyCode == KEYCODE_ESC) {
                // cPopup_Empcitys.Hide();
                //$('#cPopup_Empcitys').hide();
            }
        });

        function PopupOpentoProductUpload(obj, prod_name) {

            ///  alert(prod_name);
            var URL = '/OMS/Management/Master/Product-Multipleimage.aspx?prodid=' + obj + '&name=' + prod_name;
            window.location.href = URL;

        }

    </script>


    <script type="text/javascript">

        var counter = 0;


        function fetchLebel() {

            $("#generatedForm").html("");
            counter = 0;


            $(".newLbl").each(function () {

                var newField = "<div style='width:500px; margin-left:5px; float:left; margin-bottom:5px;'><label id='LblKey" + counter + "' style='width:110px; float:left;'>" + $(this).text() + "</label>";
                newField += "<input type='text' id='TxtKey" + counter + "' value='" + $(this).text() + "' style='margin-left:41px; width:250px;' />";

                //alert($(this).attr("id").split('_')[4]);

                if (String($(this).attr("id").split('_')[2]) != "undefined") {
                    newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("id").split('_')[2] + "' style='display:none; margin-left:41px; width:250px;' />";
                }
                else {
                    //alert($(this).attr("id"));
                    newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("id") + "' style='display:none; margin-left:41px; width:250px;' />";
                }
                newField += "</div>";

                $("#generatedForm").append(newField);

                counter++;

            });

            AssignValuePopup.Show();

        }


        function tdsOkClick() {
            tdsValue = cmb_tdstcs.GetValue();
            ctdsPopup.Hide();
        }

        function SaveDataToResource() {

            var key = "";
            var value = "";

            for (var i = 0; i < counter; i++) {

                if (key == "") {

                    key = $("#HddnKey" + i).val();
                    value = $("#TxtKey" + i).val();

                }
                else {

                    key += "," + $("#HddnKey" + i).val();
                    value += "," + $("#TxtKey" + i).val();

                }

            }

            $("#AssignValuePopup_KeyField").val(key);
            $("#AssignValuePopup_ValueField").val(value);
            $("#AssignValuePopup_RexPageName").val("ProductValues");


            return true;

        }


    </script>
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
    </style>


    <script>
        var NewExit = '';


        //Chinmoy Added Below Code
        //Start
        var Address = [];
        var ReturnDetails;
        function GetPurchaseAddress(OrderId, TagDocType) {
            var OtherDetail = {};

            OtherDetail.OrderId = OrderId;
            OtherDetail.TagDocType = TagDocType;


            if ((OrderId != null) && (OrderId != "")) {

                $.ajax({
                    type: "POST",
                    url: "ProjectPurchaseInvoice.aspx/SavePurchaseTaggedAddress",
                    data: JSON.stringify(OtherDetail),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        Address = msg.d;
                        PopulateBillShippAddress(Address);

                    }
                });
            }
        }
        function ParentCustomerOnClose(newCustId, customerName, Unique) {
            //clookup_CustomerControlPanelMain1.PerformCallback(newCustId);
            // ctxtCustName.SetText(customerName);

            GetObjectID('hdnCustomerId').value = newCustId;

            AspxDirectAddCustPopup.Hide();
            ctxtShipToPartyShippingAdd.SetText('');
            if (newCustId != "") {
                ctxtVendorName.SetText(customerName);
                SetCustomer(newCustId, customerName);
            }
            //cgridprodLookup.cgridprodView.Refresh();
            //gridLookup.Focus();
        }

        function AddVendorClick() {

            //var isLighterPage = $("#hidIsLigherContactPage").val();
            //// alert(isLighterPage);
            //if (isLighterPage == 1) {
            var url = '/OMS/management/Master/vendorPopup.html?var=1.1.4.7';
            AspxDirectAddCustPopup.SetContentUrl(url);
            //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();
            AspxDirectAddCustPopup.RefreshContentUrl();
            AspxDirectAddCustPopup.Show();
            //}
            //else {
            //    var url = 'HRrecruitmentagent_general.aspx?id=' + 'ADD';
            //    //window.location.href = url;
            //    AspxDirectAddCustPopup.SetContentUrl(url);

            //    AspxDirectAddCustPopup.RefreshContentUrl();
            //    AspxDirectAddCustPopup.Show();

            //}


        }

        function PopulateBillShippAddress(ReturnDetails) {

            var BillingDetails = $.grep(ReturnDetails, function (e) { return e.Type == "Billing" })
            var ShippingDetails = $.grep(ReturnDetails, function (e) { return e.Type == "Shipping" })

            //Billing Address Details
            if (BillingDetails.length > 0) {
                ctxtAddress1.SetText(BillingDetails[0].Address1);
                ctxtAddress2.SetText(BillingDetails[0].Address2);
                ctxtAddress3.SetText(BillingDetails[0].Address3);
                ctxtlandmark.SetText(BillingDetails[0].Landmark);
                ctxtbillingPin.SetText(BillingDetails[0].PinCode);
                $('#hdBillingPin').val(BillingDetails[0].PinId);
                ctxtbillingCountry.SetText(BillingDetails[0].CountryName);
                $('#hdCountryIdBilling').val(BillingDetails[0].CountryId);
                ctxtbillingState.SetText(BillingDetails[0].StateName);
                $('#hdStateCodeBilling').val(BillingDetails[0].StateCode);
                $('#hdStateIdBilling').val(BillingDetails[0].StateId);
                ctxtbillingCity.SetText(BillingDetails[0].CityName);
                $('#hdCityIdBilling').val(BillingDetails[0].CityId);
                ctxtSelectBillingArea.SetText(BillingDetails[0].AreaName);
                $('#hdAreaIdBilling').val(BillingDetails[0].AreaId);


                var GSTIN = BillingDetails[0].GSTIN;
                var GSTIN1 = GSTIN.substring(0, 2);
                var GSTIN2 = GSTIN.substring(2, 12);
                var GSTIN3 = GSTIN.substring(12, 15);

                ctxtBillingGSTIN1.SetText(GSTIN1);
                ctxtBillingGSTIN2.SetText(GSTIN2);
                ctxtBillingGSTIN3.SetText(GSTIN3);
                //cddlPosGstInvoice.SetValue(BillingDetails[0].PosForGst);
                PosGstId = BillingDetails[0].PosForGst;
                // cddlPosGstInvoice.SetValue(posGSTId);

            }
            else {
                ctxtAddress1.SetText('');
                ctxtAddress2.SetText('');
                ctxtAddress3.SetText('');
                ctxtlandmark.SetText('');
                ctxtbillingPin.SetText('');
                $('#hdBillingPin').val('');
                ctxtbillingCountry.SetText('');
                $('#hdCountryIdBilling').val('');
                ctxtbillingState.SetText('');
                $('#hdStateCodeBilling').val('');
                $('#hdStateIdBilling').val('');
                ctxtbillingCity.SetText('');
                $('#hdCityIdBilling').val('');
                ctxtSelectBillingArea.SetText('');
                $('#hdAreaIdBilling').val('');

                ctxtBillingGSTIN1.SetText('');
                ctxtBillingGSTIN2.SetText('');
                ctxtBillingGSTIN3.SetText('');
            }

            //Shipping Address Details
            if (ShippingDetails.length > 0) {
                ctxtsAddress1.SetText(ShippingDetails[0].Address1);
                ctxtsAddress2.SetText(ShippingDetails[0].Address2);
                ctxtsAddress3.SetText(ShippingDetails[0].Address3);
                ctxtslandmark.SetText(ShippingDetails[0].Landmark);
                ctxtShippingPin.SetText(ShippingDetails[0].PinCode);
                $('#hdShippingPin').val(ShippingDetails[0].PinId);
                ctxtshippingCountry.SetText(ShippingDetails[0].CountryName);
                $('#hdCountryIdShipping').val(ShippingDetails[0].CountryId);
                ctxtshippingState.SetText(ShippingDetails[0].StateName);
                $('#hdStateCodeShipping').val(ShippingDetails[0].StateCode);
                $('#hdStateIdShipping').val(ShippingDetails[0].StateId);
                ctxtshippingCity.SetText(ShippingDetails[0].CityName);
                $('#hdCityIdShipping').val(ShippingDetails[0].CityId);
                ctxtSelectShippingArea.SetText(ShippingDetails[0].AreaName);
                $('#hdAreaIdShipping').val(ShippingDetails[0].AreaId);


                var GSTIN = ShippingDetails[0].GSTIN;
                var GSTIN1 = GSTIN.substring(0, 2);
                var GSTIN2 = GSTIN.substring(2, 12);
                var GSTIN3 = GSTIN.substring(12, 15);


                ctxtShippingGSTIN1.SetText(GSTIN1);
                ctxtShippingGSTIN2.SetText(GSTIN2);
                ctxtShippingGSTIN3.SetText(GSTIN3);
                $('#hdShipToParty').val(ShippingDetails[0].ShipToPartyId);
                ctxtShipToPartyShippingAdd.SetText(ShippingDetails[0].ShipToPartyName);
                //cddlPosGstInvoice.SetValue(ShippingDetails[0].PosForGst);
                PosGstId = ShippingDetails[0].PosForGst;
                // cddlPosGstInvoice.SetValue(posGSTId);
            }
            else {
                ctxtsAddress1.SetText('');
                ctxtsAddress2.SetText('');
                ctxtsAddress3.SetText('');
                ctxtslandmark.SetText('');
                ctxtShippingPin.SetText('');
                $('#hdShippingPin').val('');
                ctxtshippingCountry.SetText('');
                $('#hdCountryIdShipping').val('');
                ctxtshippingState.SetText('');
                $('#hdStateCodeShipping').val('');
                $('#hdStateIdShipping').val('');
                ctxtshippingCity.SetText('');
                $('#hdCityIdShipping').val('');
                ctxtSelectShippingArea.SetText('');
                $('#hdAreaIdShipping').val('');

                ctxtShippingGSTIN1.SetText('');
                ctxtShippingGSTIN2.SetText('');
                ctxtShippingGSTIN3.SetText('');
                $('#hdShipToParty').val('');
                ctxtShipToPartyShippingAdd.SetText('');

            }

            GetPurchaseForGstValue();
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

            cddlPosGstInvoice.ClearItems();
            if (cddlPosGstInvoice.GetItemCount() == 0) {
                cddlPosGstInvoice.AddItem(GetShippingStateName() + '[Shipping]', "S");
                cddlPosGstInvoice.AddItem(GetBillingStateName() + '[Billing]', "B");
            }

            else if (cddlPosGstInvoice.GetItemCount() > 2) {
                cddlPosGstInvoice.ClearItems();
                //cddl_PosGstSalesOrder.RemoveItem(0);
                //cddl_PosGstSalesOrder.RemoveItem(0);
            }

            if (PosGstId == "" || PosGstId == null) {
                cddlPosGstInvoice.SetValue("S");
            }
            else {
                cddlPosGstInvoice.SetValue(PosGstId);
            }
        }


        var PosGstId = "";
        function PopulateInvoicePosGst(e) {

            PosGstId = cddlPosGstInvoice.GetValue();
            if (PosGstId == "S") {
                cddlPosGstInvoice.SetValue("S");
            }
            else if (PosGstId == "B") {
                cddlPosGstInvoice.SetValue("B");
            }
        }

        function TDSEditableCheckChanged() {
            var checkval = cchk_TDSEditable.GetChecked();
            if (checkval) {
                cgridinventory.SetEnabled(true);
            }
            else {
                cgridinventory.SetEnabled(false);
            }
        }

        // TDS Section Modification Section Start on 05022017 by Sam
        function TDSAmtLostFocus() {

            var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
            var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
            var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
            var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
            var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
            ctxt_totalnoninventoryproductamt.SetText(TotaNonInvSecAmt);
        }

        function SurchargeAmountLostFocus() {
            var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
            var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
            var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
            var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
            var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
            ctxt_totalnoninventoryproductamt.SetText(TotaNonInvSecAmt);
        }

        function EducationCessAmtLostFocus() {
            var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
            var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
            var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
            var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
            var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
            ctxt_totalnoninventoryproductamt.SetText(TotaNonInvSecAmt);
        }

        function HgrEducationCessAmtLostFocus() {
            var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
            var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
            var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
            var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
            var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
            ctxt_totalnoninventoryproductamt.SetText(TotaNonInvSecAmt);
        }

        var taxtype;
        var isenabled = 0;
        function RCMCheckChanged() {
            var checkval = cchk_reversemechenism.GetChecked();
            if (checkval) {
                taxtype = cddl_AmountAre.GetValue();
                cddl_AmountAre.SetValue(3);
                PopulateGSTCSTVAT();
                cddl_AmountAre.SetEnabled(false);
            }
            else {
                cddl_AmountAre.SetValue(taxtype);
                if (taxtype == '3') {
                    cddl_AmountAre.SetEnabled(false);
                }
                else {
                    cddl_AmountAre.SetEnabled(true);
                }
                //cddl_AmountAre.SetValue(1);
                PopulateGSTCSTVAT();

            }
        }
        // TDS Section Modification Section End on 05022017 by Sam

        //function RCMCheckChanged()
        //{
        //    var checkval = cchk_reversemechenism.GetChecked(); 
        //    if (checkval) {
        //        cddl_AmountAre.SetValue(3);
        //        PopulateGSTCSTVAT();
        //        cddl_AmountAre.SetEnabled(false);
        //    }
        //    else
        //    {
        //        cddl_AmountAre.SetValue(1);
        //        PopulateGSTCSTVAT();
        //        cddl_AmountAre.SetEnabled(true);
        //    }
        //}
        //function ProductSelected(s, e) {
        function SetProduct(Id, Name) {
            $('#ProductModel').modal('hide');

            var LookUpData = Id;
            var ProductCode = Name;

            if (!ProductCode) {
                LookUpData = null;
            }
            //clookup_Project.SetEnabled(false);
            //if (!cproductLookUp.FindItemByValue(cproductLookUp.GetValue())) {
            //    cProductpopUp.Hide();
            //    cgridprod.batchEditApi.StartEdit(globalRowIndex, 4);
            //    jAlert("Product not Exists.", "Alert", function () { cproductLookUp.SetValue(); cproductLookUp.Focus(); });
            //    return;
            //}
            //var LookUpData = cproductLookUp.GetValue();
            //var ProductCode = cproductLookUp.GetText();

            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);
            pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);
            cddl_TdsScheme.SetEnabled(false);
            ctxtVendorName.SetEnabled(false);
            cddlPosGstInvoice.SetEnabled(false);
            $('#ddl_numberingScheme').attr("disabled", true);
            cchk_reversemechenism.SetEnabled(false);
            $('#ddlInventory').prop('disabled', true);
            $('#ddl_Branch').prop('disabled', true);
            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("PurchasePrice");
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
            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();
            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strDescription);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);
            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
            } else {
                divPacking.style.display = "none";
            }
            // Running total Calculation Start
            Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            CalculateAmount();
            // Running total Calculation End

            //Debjyoti
            //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
            deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), strProductID);
            grid.batchEditApi.StartEdit(globalRowIndex, 4);
        }



        function ProductKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {

                s.OnButtonClick(0);
            }

        }

        //function ProductButnClick(s, e) {
        //    if (e.buttonIndex == 0) {

        //        if (!GetObjectID('hdnCustomerId').value) {
        //            jAlert("Please Select Customer first.", "Alert", function () { $('#txtCustSearch').focus(); })
        //            return;
        //        }

        //        $('#txtProdSearch').val('');
        //        $('#ProductModel').modal('show');
        //    }
        //}
        function Purchaseprodkeydown(e) {
            var OtherDetails = {};
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            var invtype = $('#ddlInventory').val();
            var TDSid = cddl_TdsScheme.GetValue();
            OtherDetails.InventoryType = invtype;
            OtherDetails.TDSCode = TDSid;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");
                // HeaderCaption.push("Installation Reqd.");

                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetPurchaseInvoiceProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }

        }

        function ProductButnClick(s, e) {
            if (e.buttonIndex == 0) {
                //var customerval = gridLookup.GetValue();
                var customerval = GetObjectID('hdnCustomerId').value;
                if ($('#txtVoucherNo').val() == '' || $('#txtVoucherNo').val() == null) {
                    jAlert('Select a numbering schema first.');
                    $('#ddl_numberingScheme').focus();
                    return false;
                }
                else if (customerval == '' || customerval == null || customerval == "") {
                    jAlert('Select a Vendor first');
                    //gridLookup.Focus();
                    ctxtVendorName.Focus();
                    return false;
                }
                else if (cproductLookUp.Clear()) {
                    // Running total Calculation Start 
                    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                    // Running total Calculation End

                    //New Modification Section on 05012018
                    $('#txtProdSearch').val('');
                    $('#ProductModel').modal('show');
                    setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
                    //$('input[id=txtProdSearch]').focus();
                    //$('#txtProdSearch').focus();
                    //cProductpopUp.Show();
                    //cproductLookUp.Focus();
                    //cproductLookUp.ShowDropDown();

                    //New Modification Section on 05012018 End
                }
            }
        }



        function SetHsnSac(newHsnSac) {
            newHsnSac = newHsnSac.trim();
            if (newHsnSac != "") {
                var existsHsnSac = $('#hdHsnList').val();
                if (existsHsnSac.indexOf(',' + newHsnSac + ',') == -1) {
                    existsHsnSac = existsHsnSac + newHsnSac + ',';
                    $('#hdHsnList').val(existsHsnSac);
                }
            }
        }

        function RemoveHSnSacFromList(newHsnSac) {
            newHsnSac = newHsnSac.trim();
            if (newHsnSac != "") {
                var existsHsnSac = $('#hdHsnList').val();

                existsHsnSac = existsHsnSac.replace(newHsnSac + ',', '');
                $('#hdHsnList').val(existsHsnSac);

            }
        }

        function ProductSelected(s, e) {
            if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return;
            }

            var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            var ProductCode = cproductLookUp.GetValue();
            if (!ProductCode) {
                LookUpData = null;
            }
            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            //Delete hsn
            if (grid.GetEditor("ProductID").GetText() != "") {
                var previousProductId = grid.GetEditor("ProductID").GetText();
                RemoveHSnSacFromList(previousProductId.split("||@||")[19]);
            }


            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);

            pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");

            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            $('#HDSelectedProduct').val(strProductID);


            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            SetHsnSac(SpliteDetails[19]);
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



            var totalNetAmount = grid.GetEditor("TotalAmount").GetValue();

            var newTotalNetAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(totalNetAmount);
            cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(newTotalNetAmount) * 100) / 100).toFixed(2));
            SetInvoiceLebelValue();


            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strDescription);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);


            // ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
            deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), strProductID);
            cbnrOtherChargesvalue.SetText('0.00');
            SetRunningBalance();
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }



        function ProductlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }
        }
    </script>

    <%--Batch Product Popup End--%>

    <script>
        // Vendor Search Section Start on 03012018

        function VendorButnClick(s, e) {

            document.getElementById("txtCustSearch").value = "";
            var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\"><th>Vendor Name</th><th>Unique Id</th></tr><table>";
            document.getElementById("CustomerTable").innerHTML = txt;

            $('#CustModel').modal('show');
            $('#txtCustSearch').focus();
        }

        function VendorKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                document.getElementById("txtCustSearch").value = "";
                var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\"><th>Vendor Name</th><th>Unique Id</th></tr><table>";
                document.getElementById("CustomerTable").innerHTML = txt;

                $('#CustModel').modal('show');
                $('#txtCustSearch').focus();
            }
        }

        function Customerkeydown(e) {
            var OtherDetails = {};
            OtherDetails.SearchKey = $("#txtCustSearch").val();
            OtherDetails.BranchID = $('#ddl_Branch').val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Vendor Name");
                HeaderCaption.push("Unique Id");
                if (OtherDetails.SearchKey != '') {
                    callonServer("Services/Master.asmx/GetVendorWithBranch", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
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
                                    <%--gridLookup.SetValue($('#<%=hdnTaggedVender.ClientID %>').val());--%>
                        //Pending Section End
                    }

                    //Chinmoy edited below line
                    var invtype = $('#ddlInventory').val();
                    if (key != null && key != '') {
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                    }
                    grid.PerformCallback('GridBlank');
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax('DeleteAllTax', "", "");
                }
                else {
                    //Pending Section Start
                                 <%--gridLookup.SetValue($('#<%=hdnTaggedVender.ClientID %>').val()); --%>
                    var vendorid = $('#<%=hdnTaggedVender.ClientID %>').val();
                    GetObjectID('hdnCustomerId').value = vendorid;
                    ctxtVendorName.SetText($('#<%=hdnTaggedVendorName.ClientID %>').val());
                    //gridLookup.PerformCallback(vendorid) 
                    //Pending Section End
                }
            });
        }

        function SetCustomer(Id, Name) {

            var VendorID = Id;
            if (Id != "") {

                // Newly added code for vendor search by Sam on 03012018 section start
                var noofvisiblerows = grid.GetVisibleRowsOnPage();
                if (noofvisiblerows == '0') {
                    grid.AddNewRow();
                    var tbQuotation = grid.GetEditor("SrlNo");
                    tbQuotation.SetValue('1');
                }
                var invtype = $('#ddlInventory').val();
                var startDate = new Date();
                startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
                var branchid = $('#ddl_Branch').val();
                GetPurchaseForGstValue();
                var key = Id;
                GetObjectID('hdnCustomerId').value = key;
                SetEntityType(VendorID);
                // For Checking Shipping AddressOfVendor End  
                if (key != $('#<%=hdnTaggedVender.ClientID %>').val()) {
                    if (gridquotationLookup.GetValue() != null) {
                        setTimeout(function () {
                            GridClearConfirm();
                        }, 200);
                    }
                    else {

                        var key = GetObjectID('hdnCustomerId').value;
                        if (key != null && key != '') {
                            $('#<%=hdnTaggedVendorName.ClientID %>').val(ctxtVendorName.GetText());
                            if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                                var schemabranchid = $('#ddl_numberingScheme').val();
                                if (schemabranchid != '0') {
                                    var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                                    SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                                    page.tabs[1].SetEnabled(true);
                                }
                            }
                            else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') {
                                var schemabranchid = $('#ddl_Branch').val();
                                if (schemabranchid != '0') {
                                    var schemabranch = schemabranchid;
                                    SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
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
                            $('#ddl_numberingScheme').prop('disabled', false);
                            ctxtVendorName.Focus();
                        }
                    }
                }
                // Newly added code for vendor search by Sam on 03012018 section End
                $('#CustModel').modal('hide');
                ctxtVendorName.SetText(Name);
                SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                page.tabs[0].SetEnabled(true);
                page.tabs[1].SetEnabled(true);
                GetObjectID('hdnCustomerId').value = VendorID;
                //Chinmoy added below line
                GetVendorGSTInFromBillShip(GetObjectID('hdnCustomerId').value);
                if ($('#hfBSAlertFlag').val() == "1") {
                    jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            page.SetActiveTabIndex(1);
                        }
                    });
                }
                ctxt_partyInvNo.Focus();
                //cContactPerson.Focus();
                //------------------------Code added for mantis id 0020338--------
                var VendorId = GetObjectID('hdnCustomerId').value;
                var branchid = $('#ddl_Branch').val();
                if (VendorId != null && VendorId != "") {
                    cContactPerson.PerformCallback('BindContactPerson~' + VendorId + '~' + branchid);
                    if ($("#btn_TermsCondition").is(":visible")) {
                        //callTCControl(quote_Id[0], $("#rdl_PurchaseInvoice").find(":checked").val());
                        callTCspecefiFields_PO(VendorId);
                    }

                }
                //-----------------------------End-----------------------------
            }
        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex")
                        SetProduct(Id, name);
                        //Chinmoy added below line
                    else if (indexName == "BillingAreaIndex") {
                        SetBillingArea(Id, name);
                    }
                    else if (indexName == "ShippingAreaIndex") {
                        SetShippingArea(Id, name);
                    }
                    else if (indexName == "customeraddressIndex") {
                        SetCustomeraddress(Id, name)
                    }
                    else if (indexName == "MainAccountIndexRO") {
                        $('#MainAccountModelRO').modal('hide');
                        var IsSub = e.target.parentElement.parentElement.children[2].innerText;
                        var RevApp = e.target.parentElement.parentElement.children[3].innerText;
                        if (RevApp == 'Yes') {
                            RevApp = '1';
                        }
                        else {
                            RevApp = '0';
                        }
                        var TaxAble = e.target.parentElement.parentElement.children[4].innerText;
                        SetMainAccountRO(Id, name, IsSub, RevApp, TaxAble);
                    }
                        //End
                    else
                        SetCustomer(Id, name);
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
                    if (indexName == "ProdIndex")
                        $('#txtProdSearch').focus();
                        //added by chinmoy
                    else if (indexName == "BillingAreaIndex")
                        $('#txtbillingArea').focus();
                    else if (indexName == "ShippingAreaIndex")
                        $('#txtshippingArea').focus();
                    else if (indexName == "customeraddressIndex")
                        ('#txtshippingShipToParty').focus();
                    else if (indexName == "MainAccountIndexRO")
                        $('#txtMainAccountSearchRO').focus();
                        //End
                    else
                        $('#txtCustSearch').focus();
                }
            }
        }


        function SetMainAccountRO(Id, name, e) {

            $('#MainAccountModelRO').modal('hide');
            cbtnGL.SetText(name);
            $('#hdnROMainAc').val(Id);
            crtxtRemarks.SetFocus();
        }

        // Vendor Search Section End on 03012018
    </script>



    <script type="text/javascript">

        // Final Checking by Sam on 15102017 Start


        function ddlInventory_OnChange() {
            var invtype = $('#ddlInventory').val();
            //Ref Mantis:22993
            //invtype == 'N' || invtype == 'S' || || invtype == 'B'
            if (invtype == 'C') {

                $('#divTdsScheme').removeClass('hide');
                $('#rdlbutton').addClass('hide');
                $('#rdldate').addClass('hide');
                $('#spVehTC').addClass('hide');
            }

            else {
                if (invtype == 'N' || invtype == 'S' || invtype == 'B') {
                    $('#divTdsScheme').removeClass('hide');
                }
                else {
                    $('#divTdsScheme').addClass('hide');
                }
                $('#rdlbutton').removeClass('hide');
                $('#rdldate').removeClass('hide');
                $('#spVehTC').removeClass('hide');
                var key = GetObjectID('hdnCustomerId').value;
                if (key != null && key != '') {
                    selectValue();
                }
            }
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
                        url: "ProjectPurchaseInvoice.aspx/BindBranchByParentID",
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
                    url: 'ProjectPurchaseInvoice.aspx/getSchemeType',
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

                        var dt = new Date();

                        cPLQuoteDate.SetDate(dt);

                        if (dt < new Date(fromdate)) {
                            cPLQuoteDate.SetDate(new Date(fromdate));
                        }

                        if (dt > new Date(todate)) {
                            cPLQuoteDate.SetDate(new Date(todate));
                        }

                        if ($("#hdnBackdateddate").val() != "0" && $("#hdnBackdateddate").val() != "") {
                            var Days = $("#hdnBackdateddate").val();
                            var today = cPLQuoteDate.GetDate();
                            var newdate = cPLQuoteDate.GetDate();
                            newdate.setDate(today.getDate() - Math.round(Days));
                            cPLQuoteDate.SetMinDate(newdate);
                            cPLQuoteDate.SetMaxDate(dt);

                        }
                        else {
                            cPLQuoteDate.SetMinDate(new Date(fromdate));
                            cPLQuoteDate.SetMaxDate(new Date(todate));
                        }
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
                <%--cvendorPanel.PerformCallback('<%=Session["userbranchID"]%>')--%>
            }
            //Chinmoy added below line

            SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
            clookup_Project.gridView.Refresh();
        }

        //............Check Unique   Purchase Order................
        function txtBillNo_TextChanged() {    // function 3
            var mode = ''
            var VoucherNo = document.getElementById("txtVoucherNo").value;
            $.ajax({
                type: "POST",
                url: "ProjectPurchaseInvoice.aspx/CheckUniqueName",
                data: JSON.stringify({ VoucherNo: VoucherNo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        $("#DuplicateBillNo").show();

                        document.getElementById("txtVoucherNo").value = '';
                        document.getElementById("txtVoucherNo").focus();
                    }
                    else {
                        $("#DuplicateBillNo").hide();
                    }
                }
            });
        }

        function OpenRetention()
        {
            $("#hdnSettingsShowRetention").val('ShowRetention');
        }

        function specialedit_ButtonClick() {
            flag = true;

            $("#hdnSettingsShowRetention").val('ShowRetention');

            var invtype = $('#ddlInventory').val();
            if (invtype != 'N') {
                if (ctxt_partyInvNo.GetText() == '' || ctxt_partyInvNo.GetText() == null) {
                    flag = false;
                    $("#MandatorysPartyinvno").show();
                    LoadingPanel.Hide();
                    return false;
                }
            }
            // Invoice Date validation Start
            var sdate = cdt_partyInvDt.GetValue();
            var edate = cPLQuoteDate.GetValue();
            var startDate = new Date(sdate);
            var endDate = new Date(edate);
            if (invtype == 'N') {
                if (sdate == null || sdate == "") {
                }
                else {
                    $('#MandatoryPartyDate').attr('style', 'display:none');

                    if (startDate > endDate) {
                        LoadingPanel.Hide();
                        flag = false;
                        $('#MandatoryEgSDate').attr('style', 'display:block');
                    }
                    else {
                        $('#MandatoryEgSDate').attr('style', 'display:none');
                        flag = true;
                    }
                }
            }
            else if (invtype != 'N') {
                if (sdate == null || sdate == "") {
                    flag = false;
                    $('#MandatoryPartyDate').attr('style', 'display:block');
                    LoadingPanel.Hide();
                    return false;
                }
                else {
                    $('#MandatoryPartyDate').attr('style', 'display:none');

                    if (startDate > endDate) {
                        LoadingPanel.Hide();
                        return false;
                        flag = false;
                        $('#MandatoryEgSDate').attr('style', 'display:block');
                    }
                    else {
                        $('#MandatoryEgSDate').attr('style', 'display:none');
                        flag = true;
                    }
                }
            }
            if (flag == true) {
                cpartyInvoicepanel.PerformCallback('SpecialEdit');
            }
        }
    </script>





    <%--// Running Balance Calculation--%>
    <script>
        function GlobalBillingShippingEndCallBack() {
            if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
                cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
                var invtype = $('#ddlInventory').val();
                var startDate = new Date();
                startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
                var branchid = $('#ddl_Branch').val();
                //var key = gridLookup.GetValue()
                var key = GetObjectID('hdnCustomerId').value;
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                if (type != null && type != '') {
                    //if (gridquotationLookup.GetValue() != null) {
                    cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
                    var startDate = new Date();
                    startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
                    //var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : ""; 
                    if (key != null && key != '') {
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                    }
                    grid.PerformCallback('GridBlank');
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax('DeleteAllTax', "", "");
                    gridquotationLookup.SetText('');
                }
                else {
                    //var key = gridLookup.GetValue()
                    var key = GetObjectID('hdnCustomerId').value;
                    if (key != null && key != '') {
                        cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
                    }
                }
                //if (key != null) {
                //    if ($("#btn_TermsCondition").is(":visible")) {
                //        callTCspecefiFields_PO(key);
                //    }
                //}
            }
        }
        function partyInvDtMandatorycheck() {
            var invtype = $('#ddlInventory').val();
            if (invtype != 'N') {
                var Podt = cdt_partyInvDt.GetValue();
                if (Podt != null) {
                    $('#MandatoryPartyDate').attr('style', 'display:none');
                    var sdate = cdt_partyInvDt.GetValue();
                    var edate = cPLQuoteDate.GetValue();

                    var startDate = new Date(sdate);
                    var endDate = new Date(edate);
                    if (startDate > endDate) {
                        $('#MandatoryEgSDate').attr('style', 'display:block');
                    }
                    else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
                }
                else {
                    $('#MandatoryPartyDate').attr('style', 'display:none');
                }
            }
            else {

            }

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
                            url: "ProjectPurchaseInvoice.aspx/CheckUniquePartyNo",
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
    </script>

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

    <script>
        $(document).ready(function () {
            $('.number').keypress(function (event) {
                if (event.which < 46 || event.which > 59) {
                    event.preventDefault();
                } // prevent if not number/dot

                if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                    event.preventDefault();
                } // prevent if already dot
            });
        });
    </script>

    <script>
        var PreviousCurrency = "1";
        function GetPreviousCurrency() {
            PreviousCurrency = ctxtRate.GetValue();
        }

        function OnAddNewClick_Default() {
            grid.AddNewRow();

            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
        }

        function showhide(obj) {
            if (obj == 'Y') {
                $('#divselectunselect').addClass('hide');
            }
            else {
                $('#divselectunselect').removeClass('hide');
            }
        }
        function GridProductBind(e) {
            var invtype = $('#ddlInventory').val();
            cproductPanel.PerformCallback(invtype);
        }
    </script>
    <script>
        $(document).ready(function () {
            var mode = $('#<%=hdnADDEditMode.ClientID %>').val();
            if (mode == 'Edit') {
                if ($("#hdnCustomerId").val() != "") {
                    var VendorID = $("#hdnCustomerId").val();
                    SetEntityType(VendorID);
                }
                $("#<%=rdl_PurchaseInvoice.ClientID %>").find('input').prop('disabled', true);
                if ($('#<%=hdnTDSShoworNot.ClientID %>').val() == 'S') {
                    $('#divTdsScheme').removeClass('hide');
                }
                else if ($('#<%=hdnTDSShoworNot.ClientID %>').val() == 'H') {
                    $('#divTdsScheme').addClass('hide');
                    
                }
                if ($('#ddlInventory').val() == 'N' || $('#ddlInventory').val() == 'C') {
                    $('#spVehTC').addClass('hide');
                }
                if ($('#ddlInventory').val() == 'Y' || $('#ddlInventory').val() == 'B') {
                    $('#divTCS').show();
                }


            }
            else {
                $("#<%=rdl_PurchaseInvoice.ClientID %>").find('input').prop('disabled', false);
            }


            if (mode == 'ADD') {
                if ($('#ddlInventory').val() == 'Y' || $('#ddlInventory').val() == 'B') {
                    $('#divTCS').show();
                }
            }
        })
    </script>
    <%-- UDF and Transport Section Start--%>
    <script>
        var canCallBack = true;
        function AllControlInitilize() {
            if (canCallBack) {
                if ($('#hdnADDEditMode').val() == 'Edit') {
                    cddlPosGstInvoice.SetEnabled(false);
                    $('#txtVoucherNo').attr("disabled", true);
                    LoadCustomerBillingShippingAddress(GetObjectID('hdnCustomerId').value);
                    LoadBranchAddressInEditMode($('#ddl_Branch').val());
                    cQuotationComponentPanel.SetEnabled(false)
                    PopulateInvoicePosGst();
                    cchk_reversemechenism.SetEnabled(false);
                    if (cchk_reversemechenism.GetValue()) {
                        $('#divreverse').removeClass('hide');
                        grid.GetEditor('TaxAmount').SetEnabled(false);
                    }
                }
                else {
                    ddlInventory_OnChange();
                }
                canCallBack = false;
            }
        }
        function acbpCrpUdfEndCall(s, e) {
            var result = 0;
            if (cacbpCrpUdf.cpUDF == "true") {
                result = 1;
            }
            else {
                jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
                cacbpCrpUdf.cpUDF = null;
                cacbpCrpUdf.cpTransport = null;
                cacbpCrpUdf.cpTC = null;
                LoadingPanel.Hide();
                <%--$('#<%=hdnRefreshType.ClientID %>').val('');--%>
                $('#hdnRefreshType').val('');
                result = 0;
                return;
            }
            if (cacbpCrpUdf.cpTransport == "true") {
                result = 1;
            }
            else {
             <%--   jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                cacbpCrpUdf.cpUDF = null;
                cacbpCrpUdf.cpTransport = null;
                cacbpCrpUdf.cpTC = null;
                LoadingPanel.Hide();
                <%--$('#<%=hdnRefreshType.ClientID %>').val('');--%>
                //  $('#hdnRefreshType').val('');
                // result = 0;
                // return;--%>


            }
            var invtype = $('#ddlInventory').val();
            if (invtype != 'N') {
                if (cacbpCrpUdf.cpTC == "true") {
                    result = 1;
                }
                else {
                    // jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
                    //cacbpCrpUdf.cpUDF = null;
                    // cacbpCrpUdf.cpTransport = null;
                    // cacbpCrpUdf.cpTC = null;
                    // LoadingPanel.Hide();
                    <%--$('#<%=hdnRefreshType.ClientID %>').val('');--%>
                    // $('#hdnRefreshType').val('');
                    // result = 0;
                    // return;
                }
            }
            else {
                result = 1;
            }
            if (cacbpCrpUdf.cpPartyno == "N") {
                result = 1;
            }
            else {
                jAlert("Party Invoice No. already exist for the selected vendor.", "Alert", function () { });
                cacbpCrpUdf.cpUDF = null;
                cacbpCrpUdf.cpTransport = null;
                cacbpCrpUdf.cpTC = null;
                LoadingPanel.Hide();
                <%--$('#<%=hdnRefreshType.ClientID %>').val('');--%>
                $('#hdnRefreshType').val('');
                result = 0;
                return;
            }

            if (cacbpCrpUdf.cpStateId == "Y") {
                result = 1;
                FinalSaveUpdate();
            }
            else {

                LoadingPanel.Hide();
                var messege = 'Vendor' + "'s " + 'shipping address not exist.GST/Reverse charges not to be calculated.Proceed?';
                jConfirm(messege, 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        result = 1;
                        FinalSaveUpdate();
                    }
                    else {
                        result = 0;
                        <%-- $('#<%=hdnRefreshType.ClientID %>').val('');--%>
                        $('#hdnRefreshType').val('');
                    }
                    cacbpCrpUdf.cpStateId = null;
                    return;
                });
            }
        }

        function FinalSaveUpdate() {
            OnAddNewClick_Default();
            grid.UpdateEdit();
            cacbpCrpUdf.cpUDF = null;
            cacbpCrpUdf.cpTransport = null;
            cacbpCrpUdf.cpTC = null;
        }
        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;
        }
    </script>
    <%--UDF and Transport Section End--%>
    <script>

        function ProductsPopup() {
            debugger;
            var selectedrow = gridquotationLookup.gridView.GetSelectedRowCount();
            if (selectedrow > 1) {
                if ($("#hdnPartyInvoiceList").val() != "") {
                    var partyinvoice = $("#hdnPartyInvoiceList").val();
                    ctxt_partyInvNo.SetText(partyinvoice);
                }
            }
        }

        function HideSelectAllSection() {
            debugger;

            // start 02-07-2019

            var selectedrow = gridquotationLookup.gridView.GetSelectedRowCount();
            if (selectedrow > 1) {
                if (cgridproducts.cpPartyInvoiceList != "") {
                    var partyinvoice = cgridproducts.cpPartyInvoiceList;
                    ctxt_partyInvNo.SetText(partyinvoice);
                }
            }
            var selectedrow = gridquotationLookup.gridView.GetSelectedRowCount();
            if (selectedrow == 1) {
                if (cgridproducts.cpPartyInvoiceList != "") {
                    var partyno = cgridproducts.cppartydetail.toString().split('~')[0];

                    ctxt_partyInvNo.SetText(partyno);
                    var partydate = cgridproducts.cppartydetail.toString().split('~')[1];
                    cdt_partyInvDt.SetText(partydate);
                }
            }
            //End


            if (cgridproducts.cpSelectHide != null) {
                if (cgridproducts.cpSelectHide == 'Y') {
                    $('#divselectunselect').addClass('hide')
                }
                else if (cgridproducts.cpSelectHide == 'N') {
                    $('#divselectunselect').removeClass('hide')
                }
                else {
                    $('#divselectunselect').removeClass('hide')
                }
                cgridproducts.cpSelectHide == null;
            }

            if (cgridproducts.cppartydetail != null) {
                var invtype = $('#ddlInventory').val();
                var partyno = cgridproducts.cppartydetail.toString().split('~')[0];
                var partydate = cgridproducts.cppartydetail.toString().split('~')[1];
                var docdate = cgridproducts.cppartydetail.toString().split('~')[2];
                var modetype = $('#<%=hdnADDEditMode.ClientID %>').val();
                if (modetype != 'Edit') {
                    //ctxt_partyInvNo.SetText(partyno);
                    $("#hdn_party_inv_no").val(partyno);
                    if (invtype != 'N') {
                        if (partyno != null && partyno != '') {
                            $("#MandatorysPartyinvno").hide();
                        }
                        else {
                            $("#MandatorysPartyinvno").show();
                        }
                    }
                    else {
                        $("#MandatorysPartyinvno").hide();
                    }
                    if (invtype != 'N') {

                        if (partydate != null && partydate != '') {
                            cdt_partyInvDt.SetText(partydate);
                            $('#MandatoryPartyDate').attr('style', 'display:none');
                        } else {
                            $('#MandatoryPartyDate').attr('style', 'display:block');
                        }
                    }
                    else {
                        $('#MandatoryPartyDate').attr('style', 'display:none');
                    }
                }
                var reff = cgridproducts.cppartydetail.toString().split('~')[3];
                var curr = cgridproducts.cppartydetail.toString().split('~')[4];
                var rate = cgridproducts.cppartydetail.toString().split('~')[5];
                var person = cgridproducts.cppartydetail.toString().split('~')[6];
                var amtare = cgridproducts.cppartydetail.toString().split('~')[7];
                var taxcode = cgridproducts.cppartydetail.toString().split('~')[8];
                var EWayBillNumber = cgridproducts.cppartydetail.toString().split('~')[9];
                cPLQADate.SetText(docdate);
                cddl_AmountAre.SetEnabled(false);
                if (reff != '') {
                    ctxt_Refference.SetText(reff);
                }
                if (person != '') {

                    //cContactPerson.SetValue(person);
                    var key = GetObjectID('hdnCustomerId').value;
                    var branchid = $('#ddl_Branch').val();
                    cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
                }
                if (curr != '') {
                    $("#<%=ddl_Currency.ClientID%>").val(curr);
                }
                if (rate != '') {
                    ctxtRate.SetText(rate);
                }
                if (amtare != '') {
                    cddl_AmountAre.SetValue(amtare);
                }
                if (taxcode != '') {
                    cddlVatGstCst.PerformCallback('SetTaxCode' + '~' + taxcode)
                    var items = $('#cddlVatGstCst option').length;
                    cddlVatGstCst.SetValue(taxcode);
                }
                if (EWayBillNumber != '') {
                    $('#txtEWayBillNumber').val(EWayBillNumber);

                }
                cgridproducts.cppartydetail = null;
            }
        }
        <%--Div Detail for Vendor Section Start--%>
        function acpContactPersonPhoneEndCall(s, e) {
            if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
                pageheaderContent.style.display = "block";
                $("#<%=divContactPhone.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = cacpContactPersonPhone.cpPhone;
                cacpContactPersonPhone.cpPhone = null;

            }
            else {
                $("#<%=divContactPhone.ClientID%>").attr('style', 'display:none');
                document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = '';
                cacpContactPersonPhone.cpPhone = null;
            }
        }
        function GetContactPersonPhone(e) {
            var key = cContactPerson.GetValue();
            cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
        }

        //Added By chinmoy

        function IfVendorGstInIsBlank() {
            //cddl_AmountAre.SetValue("3");

            PopulateGSTCSTVAT();
            //cddl_AmountAre.SetEnabled(false);

        }

        function cmbContactPersonEndCall(s, e) {

            //Chinmoy edited start

            if ($('#hfVendorGSTIN').val() == '') {
                IfVendorGstInIsBlank();
            }

            else if ($('#hfVendorGSTIN').val() != '') {
                cddl_AmountAre.SetValue("1");
            }
            //end

            if (cContactPerson.cpContactdtl != null && cContactPerson.cpContactdtl != undefined) {
                if (cContactPerson.cpContactdtl == 'Y') {
                    $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
                    $("#<%=divContactPhone.ClientID%>").attr('style', 'display:block');
                    <%--document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = cacpContactPersonPhone.cpPhone;--%>
                }
                else {
                    $("#<%=divContactPhone.ClientID%>").attr('style', 'display:none');
                    document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = '';
                }
                cContactPerson.cpContactdtl = null;
            }
            else {
                $("#<%=divContactPhone.ClientID%>").attr('style', 'display:none');
                document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = '';
            }
            var edate = cPLQuoteDate.GetValue();
            var str = $.datepicker.formatDate('yy-mm-dd', edate);
            if ((cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) &&
                (cContactPerson.cpvendortype != null && cContactPerson.cpvendortype != undefined)) {


                $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
                $("#<%=divGSTN.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblGSTIN.ClientID %>').innerHTML = cContactPerson.cpGSTN + ' (' + cContactPerson.cpvendortype + ')';
                if (cContactPerson.cpGSTN == 'Yes' && cContactPerson.cpvendortype == 'Regular') {
                    cddl_vendortype.SetValue('R');
                    // Sandip for Terms & Condition Checking where it is mandatory or not is pending  Section Start
                    $('#pnl_TCspecefiFields_PO').css('display', 'none')
                    $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
                    // Sandip for Terms & Condition Checking where it is mandatory or not Pending Section End
                    var invtype = $('#ddlInventory').val();
                    if (invtype == 'Y' || invtype == 'B' || invtype == 'C') {

                        //$('#rdlbutton').removeClass('hide');
                        //$('#rdldate').removeClass('hide');

                    }
                    else if (invtype == 'N') {


                        //$('#rdlbutton').addClass('hide');
                        //$('#rdldate').addClass('hide');
                    }
                    // Code Commented by Sam on 01022018 Section Start
                    //cchk_reversemechenism.SetValue(false);
                    //$('#divreverse').addClass('hide'); 
                    // Code Commented by Sam on 01022018 Section End


                    cddl_AmountAre.SetValue(1);
                    PopulateGSTCSTVAT();
                    cddl_AmountAre.SetEnabled(true);
                }
                else if (cContactPerson.cpGSTN == 'Yes' && cContactPerson.cpvendortype == 'Composite') {
                    // Sandip for Terms & Condition Checking where it is mandatory or not is pending  Section Start
                    $('#pnl_TCspecefiFields_PO').css('display', 'none')
                    $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
                    // Sandip for Terms & Condition Checking where it is mandatory or not Pending Section Start
                    cddl_vendortype.SetValue('C');
                    var invtype = $('#ddlInventory').val();
                    if (invtype == 'Y' || invtype == 'B' || invtype == 'C') {
                        //$('#rdlbutton').removeClass('hide');
                        //$('#rdldate').removeClass('hide');
                    }
                    else if (invtype == 'N') {
                        //$('#rdlbutton').addClass('hide');
                        //$('#rdldate').addClass('hide');
                    }
                    // Code Commented by Sam on 01022018 Section Start
                    //cchk_reversemechenism.SetValue(false);
                    //$('#divreverse').addClass('hide');
                    // Code Commented by Sam on 01022018 Section End

                    cddl_AmountAre.SetValue(3);
                    PopulateGSTCSTVAT();
                    cddl_AmountAre.SetEnabled(false);
                }
                else {
                    if (cContactPerson.cpcountry != null && cContactPerson.cpcountry != '') {
                        if (cContactPerson.cpcountry != '1') {

                            // Code Commented by Sam on 01022018 Section Start
                            //cchk_reversemechenism.SetValue(false);  //Commented
                            //cchk_reversemechenism.SetEnabled(false) // Commented
                            // Code Commented by Sam on 01022018 Section End

                            //$('#rdlbutton').removeClass('hide');
                            //$('#rdldate').removeClass('hide');
                            cddl_AmountAre.SetValue(4);

                            cContactPerson.cpcountry == null
                            // $('#hfTCspecefiFieldsVisibilityCheck').val('1');
                            //$('#pnl_TCspecefiFields_PO').css('display', 'block')
                            //$('#pnl_TCspecefiFields_Not_PO').css('display', 'none')

                        }
                        else {
                            $('#hfTCspecefiFieldsVisibilityCheck').val('');
                            var RB = document.getElementById("<%=rdl_PurchaseInvoice.ClientID%>");
                            if (RB.rows.length > 0) {
                                for (i = 0; i < RB.rows.length; i++) {
                                    var cell = RB.rows[i].cells;
                                    for (j = 0; j < cell.length; j++) {
                                        if (cell[j].childNodes[0].type == "radio") {
                                            document.getElementById(cell[j].childNodes[0].id).checked = false;
                                        }
                                    }
                                }
                            }
                            //$('#rdlbutton').addClass('hide');
                            //$('#rdldate').addClass('hide');



                            // Waiting for Dirction Start
                            //if (str < '2017-10-13') {
                            // Code Commented by Sam on 01022018 Section Start
                            //cchk_reversemechenism.SetValue(true);
                            //cchk_reversemechenism.SetEnabled(false)
                            //$('#divreverse').removeClass('hide');
                            // Code Commented by Sam on 01022018 Section End
                            <%-- $('#<%=hdnRCMChecked.ClientID %>').val('1');--%>
                            <%--}
                            else
                            {
                                cchk_reversemechenism.SetValue(false);
                                cchk_reversemechenism.SetEnabled(false)
                                $('#divreverse').addClass('hide');
                                $('#<%=hdnRCMChecked.ClientID %>').val('0');
                            }--%>
                            // Waiting for Dirction  End
                            cddl_AmountAre.SetValue(3);

                            cContactPerson.cpcountry == null
                            $('#pnl_TCspecefiFields_PO').css('display', 'none')
                            $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
                        }

                    }
                    else {
                        $('#hfTCspecefiFieldsVisibilityCheck').val('');
                        var RB = document.getElementById("<%=rdl_PurchaseInvoice.ClientID%>");
                        if (RB.rows.length > 0) {
                            for (i = 0; i < RB.rows.length; i++) {
                                var cell = RB.rows[i].cells;
                                for (j = 0; j < cell.length; j++) {
                                    if (cell[j].childNodes[0].type == "radio") {
                                        document.getElementById(cell[j].childNodes[0].id).checked = false;
                                    }
                                }
                            }
                        }
                        //$('#rdlbutton').addClass('hide');
                        //$('#rdldate').addClass('hide');
                        // Waiting for Dirction Start
                        //if (str < '2017-10-13') {
                        // Code Commented by Sam on 01022018 Section Start
                        //cchk_reversemechenism.SetValue(true);
                        //cchk_reversemechenism.SetEnabled(false)
                        //$('#divreverse').removeClass('hide');
                        // Code Commented by Sam on 01022018 Section End

                        <%--$('#<%=hdnRCMChecked.ClientID %>').val('1');
                        }
                        else {
                            cchk_reversemechenism.SetValue(false);
                            cchk_reversemechenism.SetEnabled(false)
                            $('#divreverse').addClass('hide');
                            $('#<%=hdnRCMChecked.ClientID %>').val('0');
                        }--%>
                        // Waiting for Dirction  End
                        cddl_AmountAre.SetValue(3);

                        cContactPerson.cpcountry == null
                        $('#pnl_TCspecefiFields_PO').css('display', 'none')
                        $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
                    }
                    PopulateGSTCSTVAT();
                    cddl_AmountAre.SetEnabled(false);
                }
            cContactPerson.cpGSTN = null;
        }
        else {
            $("#<%=divGSTN.ClientID%>").attr('style', 'display:none');
                document.getElementById('<%=lblGSTIN.ClientID %>').innerHTML = '';
                cContactPerson.cpGSTN = null;
            }
            if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {
                $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
                $("#<%=divOutstanding.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblOutstanding.ClientID %>').innerHTML = cContactPerson.cpOutstanding;
                cContactPerson.cpOutstanding = null;
            }
            else {
                $("#<%=divOutstanding.ClientID%>").attr('style', 'display:none');
                document.getElementById('<%=lblOutstanding.ClientID %>').innerHTML = '';
            }
            ctxt_partyInvNo.Focus();
        }


    </script>
    <script type="text/javascript">
        var globalRowIndex;
        var rowEditCtrl;

        var ProductGetQuantity = "0";
        var ProductGetTotalAmount = "0";
        var ProductPurchaseprice = "0";
        var ProductDiscount = "0";
        var ProductDiscountAmt = "0";
        var ProductGrsAmt = "0";
        // Running Calculation As New Modification
        var globalNetAmount = 0;
        function DiscountGotChange() {
            globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            ProductGetTotalAmount = globalNetAmount;
            ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
            ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            ProductPurchaseprice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            // Running total Calculation Start 
            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            // Running total Calculation End
        }
        function DiscountAmtGotChange() {
            globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            ProductGetTotalAmount = globalNetAmount;
            ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            ProductPurchaseprice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
            ProductDiscountAmt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
            // Running total Calculation Start 
            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            // Running total Calculation End
        }

        function AmtGotFocus() {
            globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            ProductGetTotalAmount = globalNetAmount;
            ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            ProductPurchaseprice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
            ProductDiscountAmt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
            ProductGrsAmt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";

            // Running total Calculation Start

            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

            // Running total Calculation End
        }

        // Running Calculation As New Modification


        // To set Quantity column false if tagging is available by Sam on 10062017 Start   
        function QuantityGotFocus(s, e) {
            var taxqty = grid.GetEditor("Quantity").GetValue();
            <%--$('#<%=hdntaxqty.ClientID %>').val(taxqty);--%>
            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
            if (type == 'PC') {
                grid.GetEditor("Quantity").SetEnabled(false);
            }
            else {
                grid.GetEditor("Quantity").SetEnabled(true);
            }
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            ProductGetQuantity = QuantityValue;
            globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            // Running total Calculation Start 
            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            // Running total Calculation End

            //debugger;
            //Rev 1.0 Subhra 15-03-2019
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strProductName = SpliteDetails[1];
            var strDescription = SpliteDetails[1];

            var isOverideConvertion = SpliteDetails[23];
            var packing_saleUOM = SpliteDetails[22];
            var sProduct_SaleUom = SpliteDetails[21];
            var sProduct_quantity = SpliteDetails[20];
            var packing_quantity = SpliteDetails[19];

            var slno = (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

            var PurchaseInvoiceNum = (grid.GetEditor('ComponentNumber').GetText() != null) ? grid.GetEditor('ComponentNumber').GetText() : "0";

            var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
            var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
            var type = 'add';
            var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
            var gridPackingQty = 0.00;
            var actionQry = '';
            var rdl_PurchaseInvoice = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";

            //if (SpliteDetails.length > 23) {
            if (SpliteDetails.length == 25) {
                if (SpliteDetails[24] == "1") {
                    IsInventory = 'Yes';
                }
                else {
                    IsInventory = '';
                }
            }

            else {
                IsInventory = '';
            }

            if (rdl_PurchaseInvoice == 'PO') {
                actionQry = 'GetPurchaseInvoiceQtyOrder';
            }
            if (rdl_PurchaseInvoice == 'PC') {
                actionQry = 'GetPurchaseInvoiceQtyByGRN';
            }


            type = 'edit';

            if (PurchaseInvoiceNum != "0" && PurchaseInvoiceNum != "" && $('#hdnADDEditMode').val() != "Edit") {

                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({ orderid: strProductID, action: actionQry, module: 'PurchaseINV', strKey: PurchaseInvoiceNum }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        gridPackingQty = msg.d;

                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                            ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }

                    }
                });


            }
            else if ($('#hdnADDEditMode').val() == "Edit") {

                var orderid = grid.GetRowKey(globalRowIndex);
                //var orderid = document.getElementById('Keyval_Id').value;
                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({ orderid: orderid, action: 'GetPurchaseInvoiceQty', module: 'PurchaseINV', strKey: strProductID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        gridPackingQty = msg.d;
                        //if (PurchaseInvoiceNum == "0" && PurchaseInvoiceNum == "") {  // if not blank thats means this data are from tagged  data....you cannot modify it.
                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                            ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }
                        //}
                    }
                });
            }

            else {

                if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                    ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }
            }
            //}


            //End of Rev 1.0 Subhra 15-03-2019

        }
        // To set Quantity column false if tagging is available by Sam on 10062017 End
        //Rev Subhra 18-03-2019
        var issavePacking = 0;
        function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
            issavePacking = 1;
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor('Quantity').SetValue(Quantity);
             <%--Use for set focus on UOM after press ok on UOM--%>
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 7);
            }, 600)
            <%--Use for set focus on UOM after press ok on UOM--%>

            QuantityTextChange();
        }
        //End of Rev Subhra 18-03-2019
        function PurPriceGotFocus() {
            ProductPurchaseprice = grid.GetEditor("PurchasePrice").GetValue();
            globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            // Running total Calculation Start 
            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            // Running total Calculation End 
        }
        //............................Product Pazination..............
        function ChangeState(value) {
            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }
        function ctaxUpdatePanelEndCall(s, e) {
            if (ctaxUpdatePanel.cpstock != null) {
                divAvailableStk.style.display = "block";
                var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;
                ctaxUpdatePanel.cpstock = null;
                grid.batchEditApi.StartEdit(globalRowIndex, 4);
            }
            return false;
        }
        function ProductsGotFocusFromID(s, e) {
            //var customerval = gridLookup.GetValue() 
            var customerval = GetObjectID('hdnCustomerId').value;
            if ($('#txtVoucherNo').val() == '' || $('#txtVoucherNo').val() == null) {
                jAlert('Select a numbering schema first.')
                $('#ddl_numberingScheme').focus();
                return false;
            }
            else if (customerval == '' || customerval == null || customerval == "") {
                jAlert('Select a Vendor first')
                //gridLookup.Focus();
                ctxtVendorName.Focus();
                return false;
            }
            else {
                pageheaderContent.style.display = "block";
                var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
                var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
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
    }
    function ProductlookUpKeyDown(s, e) {
        if (e.htmlEvent.key == "Escape") {
            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 6);
        }
    }
    function ProductSelected(s, e) {
        if (!cproductLookUp.FindItemByValue(cproductLookUp.GetValue())) {
            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 4);
            jAlert("Product not Exists.", "Alert", function () { cproductLookUp.SetValue(); cproductLookUp.Focus(); });
            return;
        }
        var LookUpData = cproductLookUp.GetValue();
        var ProductCode = cproductLookUp.GetText();
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex);
        grid.GetEditor("ProductID").SetText(LookUpData);
        grid.GetEditor("ProductName").SetText(ProductCode);
        pageheaderContent.style.display = "block";
        cddl_AmountAre.SetEnabled(false);
        cddl_TdsScheme.SetEnabled(false);
        $('#ddlInventory').prop('disabled', true);
        var tbDescription = grid.GetEditor("Description");
        var tbUOM = grid.GetEditor("UOM");
        var tbSalePrice = grid.GetEditor("PurchasePrice");
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
        grid.GetEditor("Quantity").SetValue("0.00");
        grid.GetEditor("Discount").SetValue("0.00");
        grid.GetEditor("Amount").SetValue("0.00");
        grid.GetEditor("TaxAmount").SetValue("0.00");
        grid.GetEditor("TotalAmount").SetValue("0.00");
        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();
        $('#<%= lblStkQty.ClientID %>').text("0.00");
        $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
        $('#<%= lblProduct.ClientID %>').text(strDescription);
        $('#<%= lblbranchName.ClientID %>').text(strBranch);
        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
        } else {
            divPacking.style.display = "none";
        }
        // Running total Calculation Start
        Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        CalculateAmount();
        // Running total Calculation End

        //Debjyoti
        // ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
        deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), strProductID);
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
    function ProductKeyDown(s, e) {
        if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
            s.OnButtonClick(0);
        }
        //if (e.htmlEvent.key == "Tab") { 
        //    s.OnButtonClick(0);
        //}
    }
    //function ProductButnClick(s, e) {
    //    if (e.buttonIndex == 0) {
    //        //var customerval = gridLookup.GetValue();
    //        var customerval = GetObjectID('hdnCustomerId').value;
    //        if ($('#txtVoucherNo').val() == '' || $('#txtVoucherNo').val() == null) {
    //            jAlert('Select a numbering schema first.')
    //            $('#ddl_numberingScheme').focus();
    //            return false;
    //        }
    //        else if (customerval == '' || customerval == null || customerval == "") { 
    //            jAlert('Select a Vendor first')
    //            //gridLookup.Focus();
    //            ctxtVendorName.Focus();
    //            return false;
    //        }
    //        else if (cproductLookUp.Clear()) { 
    //            // Running total Calculation Start 
    //            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    //            Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    //            Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0"; 
    //            // Running total Calculation End
    //            cProductpopUp.Show();
    //            cproductLookUp.Focus();
    //            cproductLookUp.ShowDropDown();
    //        }
    //    }
    //}
    //..............End Product........................
    //.............Available Stock Div Show............................
    function ProductsGotFocus(s, e) {
        pageheaderContent.style.display = "block";
        var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
        var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
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
            //  divPacking.style.display = "block";
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
    function acpAvailableStockEndCall(s, e) {
        if (cacpAvailableStock.cpstock != null) {
            divAvailableStk.style.display = "block";
            var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
            document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
            document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = cacpAvailableStock.cpstock;
            cCmbWarehouse.cpstock = null;
        }
    }
    //................Available Stock Div Show....................
    //Code for UDF Control 
    function OpenUdf(s, e) {
        if (document.getElementById('IsUdfpresent').value == '0') {
            jAlert("UDF not define.");
        }
        else {
            var keyVal = document.getElementById('Keyval_internalId').value;
            var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=PB&&KeyVal_InternalID=' + keyVal;
            popup.SetContentUrl(url);
            popup.Show();
        }
        return true;
    }
    // End Udf Code 
    //..................Rate........................
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
            grid.UpdateEdit();
            grid.PerformCallback('CurrencyChangeDisplay~' + PreviousCurrency);
            //grid.PerformCallback('CurrencyChangeDisplay');
        }
    }
    //...............end.........................
    //...............PopulateVAT........................
    function PopulateGSTCSTVAT(e) {
        var key = cddl_AmountAre.GetValue();
        //deleteAllRows(); 
        if (key == 1 || key == 4) {
            grid.GetEditor('TaxAmount').SetEnabled(true);
            cddlVatGstCst.SetEnabled(false);
            cddlVatGstCst.SetSelectedIndex(-1);
            cbtn_SaveRecords.SetVisible(true);
            grid.GetEditor('ProductID').Focus();
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
            cddlVatGstCst.SetSelectedIndex(-1);
            cddlVatGstCst.SetEnabled(false);
            cbtn_SaveRecords.SetVisible(false);
        }
    }
    function Keypressevt() {
        if (event.keyCode == 13) {
            //run code for Ctrl+X -- ie, Save & Exit! 
            SaveWarehouse();
            return false;
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
        if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
            s.OnButtonClick(0);
        }
    }
    function taxAmtButnClick1(s, e) {


        var taxtype = cddl_AmountAre.GetValue();
        var Vendortype = cddl_vendortype.GetValue();
        if (taxtype == '3' || Vendortype == 'C') {
            grid.GetEditor('TaxAmount').SetEnabled(false);
        }
        else {
            grid.GetEditor("TaxAmount").SetEnabled(true);
        }

        rowEditCtrl = s;
    }
    function taxAmtButnClick(s, e) {
        if (e.buttonIndex == 0) {
            if (cddl_AmountAre.GetValue() != null) {
                var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
                if (ProductID.trim() != "") {
                    // Running total Calculation Start
                    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                    // Running total Calculation End
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
                    var strStkUOM = SpliteDetails[4];
                    var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "";
                    if (strRate == 0) {
                        strRate = 1;
                    }
                    var StockQuantity = strMultiplier * QuantityValue;
                    var Amount = parseFloat(Math.round(QuantityValue * strFactor * (strSalePrice / strRate) * 100) / 100).toFixed(2);
                    clblTaxProdGrossAmt.SetText(Amount);
                    clblProdNetAmt.SetText(grid.GetEditor('Amount').GetValue());
                    document.getElementById('HdProdGrossAmt').value = Amount;
                    document.getElementById('HdProdNetAmt').value = parseFloat(Math.round(grid.GetEditor('Amount').GetValue() * 100) / 100).toFixed(2);
                    document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                    //End Here 
                    //Set Discount Here
                    if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                        var discount = (Amount * grid.GetEditor('Discount').GetValue() / 100);
                        //var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
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
                        $('.gstNetAmount').hide();
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
                        //###### Added By : Samrat Roy ##########
                        //Get Customer Shipping StateCode
                        var shippingStCode = '';

                        // Here we are sending Branch StateCode instead of Shipping Statecode after discuss with
                        // Pijush Da and Debjyoti on 14122017
                        //shippingStCode = $("#ucBShfSStateCode").val();
                        if (cddlPosGstInvoice.GetValue() == "S") {
                            shippingStCode = GeteShippingStateCode();
                        }
                        else {
                            shippingStCode = GetBillingStateCode();
                        }
                        shippingStCode = shippingStCode;
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
                                    ccmbGstCstVat.RemoveItem(cmbCount);
                                    cmbCount--;
                                }
                            }
                        }

                    } else {
                        clblTaxableGross.SetText("");
                        clblTaxableNet.SetText("");

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

    function SalePriceTextChange(s, e) {
        pageheaderContent.style.display = "block";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
            var strFactor = SpliteDetails[14]; //Packing_Factor 
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();
            var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
            var strSalePrice = SpliteDetails[6];// purchase Price 
            if (strRate == 0) {
                strRate = 1;
            }
            if (strSalePrice == 0.00000) {
                strSalePrice = 1;
            }
            var StockQuantity = strMultiplier * QuantityValue;
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            var IsPackingActive = SpliteDetails[13];//IsPackingActive
            var Packing_Factor = SpliteDetails[14];//Packing_Factor
            var Packing_UOM = SpliteDetails[15];//Packing_UOM
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                // divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount);
            DiscountTextChange(s, e);
            //.........AvailableStock.............

        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Quantity').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }

    // Non Inventory Section By Sam on 24052017

    function TDSChecking(s, e) {
        var inventoryItem = $('#ddlInventory').val();
        if (inventoryItem == 'N' || inventoryItem == 'S') {
            var schemeid = cddl_TdsScheme.GetValue()
            if (schemeid != '0') {
                var ProductID = grid.GetEditor('ProductID').GetValue();
                if (ProductID != null) {

                        <%--$('#<%=hdntdschecking.ClientID %>').val('1')--%>
                    var slno = grid.GetEditor('SrlNo').GetValue();
                    if ($('#<%=hdntdschecking.ClientID %>').val() == '') {
                        $('#<%=hdntdschecking.ClientID %>').val(slno + ',');
                    }
                    else {
                        var myArray = $('#<%=hdntdschecking.ClientID %>').val().split(',');
                        if ($.inArray(slno, myArray) != -1) {

                        }
                        else {
                            $('#<%=hdntdschecking.ClientID %>').val($('#<%=hdntdschecking.ClientID %>').val() + slno)
                        }


                    }
                }
            }
        }
    }
    function qtyvalidate() {
        var srlno = grid.GetEditor('SrlNo').GetValue();
        var previousqty = grid.GetEditor('Quantity').GetValue();

        return $.ajax({
            type: "POST",
            url: 'ProjectPurchaseInvoice.aspx/ValidQuantity',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ srlno: srlno, previousqty: previousqty }),
            cache: false
        });

    }
    // Non Inventory Section By Sam on 24052017
    function QuantityTextChange(s, e) {
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)) {
            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
            if (type == 'PO') {
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
                    var OrdeMsg = 'Balance Quantity of selected Product from tagged document is (' + ((QuantityValue - TotalQty) + BalanceQty) + '). <br/>Cannot enter quantity more than balance quantity.';
                    grid.batchEditApi.EndEdit();
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
                <%--$('#<%=hdnqtyupdate.ClientID %>').val('Y');--%>
            TDSChecking();
            pageheaderContent.style.display = "block";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetValue();
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
                var strFactor = SpliteDetails[8]; //Packing_Factor 
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strProductID = SpliteDetails[0];
                var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                var ddlbranch = $("[id*=ddl_Branch]");
                var strBranch = ddlbranch.find("option:selected").text();
                var strStkUOM = SpliteDetails[4];//Stk_UOM_Name 
                var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
                   <%-- $('#<%=hdntaxpurprice.ClientID %>').val(strSalePrice);--%>
                if (strFactor == 0) {
                    strFactor = 1;
                }
                if (strRate == 0) {
                    strRate = 1;
                }
                if (strSalePrice == 0.00000) {
                    strSalePrice = 1;
                }
                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue(Amount);

                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue(Amount);
                DiscountTextChange(s, e);
                //.........AvailableStock............ 
            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('Quantity').SetValue('0');
                grid.GetEditor('PurchasePrice').SetValue('0');
                grid.GetEditor('ProductID').Focus();
            }
        }
    }
    function PurchasePriceTextChange(s, e) {
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var PriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
        if (ProductPurchaseprice != parseFloat(PriceValue)) {
            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
            if (type == 'PO') {
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
                    var OrdeMsg = 'Balance Quantity of selected Product from tagged document is (' + ((QuantityValue - TotalQty) + BalanceQty) + '). <br/>Cannot enter quantity more than balance quantity.';
                    grid.batchEditApi.EndEdit();
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
                <%--$('#<%=hdnqtyupdate.ClientID %>').val('Y');--%>
            TDSChecking();
            pageheaderContent.style.display = "block";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetValue();
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
                var strFactor = SpliteDetails[8]; //Packing_Factor 
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strProductID = SpliteDetails[0];
                var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                var ddlbranch = $("[id*=ddl_Branch]");
                var strBranch = ddlbranch.find("option:selected").text();
                var strStkUOM = SpliteDetails[4];//Stk_UOM_Name 
                var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
                if (strFactor == 0) {
                    strFactor = 1;
                }
                if (strRate == 0) {
                    strRate = 1;
                }
                if (strSalePrice == 0.00000) {
                    strSalePrice = 1;
                }
                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue(Amount);
                grid.GetEditor('TaxAmount').SetValue(0);
                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue(Amount);
                DiscountTextChange(s, e);
                //.........AvailableStock............. 
            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('Quantity').SetValue('0');
                grid.GetEditor('PurchasePrice').SetValue('0');
                grid.GetEditor('ProductID').Focus();
            }
        }
    }
    function ProductPriceCalculate() {
        if ((grid.GetEditor('PurchasePrice').GetValue() == null || grid.GetEditor('PurchasePrice').GetValue() == 0) && (grid.GetEditor('Discount').GetValue() == null || grid.GetEditor('Discount').GetValue() == 0)) {
            var _purchaseprice = 0;
            var _Qty = grid.GetEditor('Quantity').GetValue();
            var _Amount = grid.GetEditor('Amount').GetValue();
            _purchaseprice = (_Amount / _Qty);
            grid.GetEditor('PurchasePrice').SetValue(_purchaseprice);
        }
    }
    function AmtTextChange(s, e) {
        ProductPriceCalculate();
        TDSChecking();
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var PurPrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
        var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
        var Discountamt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
        var ProductWiseGrsAmt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            if (parseFloat(Discount) != parseFloat(ProductDiscount)
                || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)
                || parseFloat(PurPrice) != parseFloat(ProductPurchaseprice)
                || parseFloat(Discountamt) != parseFloat(ProductDiscountAmt)
                 || parseFloat(ProductWiseGrsAmt) != parseFloat(ProductGrsAmt)) {
                var SpliteDetails = ProductID.split("||@||");
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
                if (strFactor == 0) {
                    strFactor = 1;
                }
                if (strSalePrice == '0') {
                    strSalePrice = SpliteDetails[6];
                }
                if (strRate == 0) {
                    strRate = 1;
                }
                var Discountamt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
                var Discount = ((parseFloat(Discountamt) * 100) / parseFloat(Amount));
                var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                var grossamt = grid.GetEditor('Amount').GetValue();
                //Chinmoy edited below code
                // grid.GetEditor('TaxAmount').SetValue(0);
                var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue(parseFloat(grossamt) + parseFloat(_TotalTaxAmt));
                var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
                    //Chinmoy edited below code
                    //grid.GetEditor('TaxAmount').SetValue(0);
                    // ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
                    deleteTax('DelQtybySl', grid.GetEditor("SrlNo").GetValue(), "");
                    var incluexclutype = ''
                    var taxtype = cddl_AmountAre.GetValue();
                    if (taxtype == '1') {
                        incluexclutype = 'E'
                    }
                    else if (taxtype == '2') {
                        incluexclutype = 'I'
                    }


                    var CompareStateCode;
                    if (cddlPosGstInvoice.GetValue() == "S") {
                        CompareStateCode = GeteShippingStateCode();
                    }
                    else {
                        CompareStateCode = GetBillingStateCode();
                    }

                    var checkval = cchk_reversemechenism.GetChecked();
                    if (!checkval) {
                        if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                            var schemabranchid = $('#ddl_numberingScheme').val();
                            if (schemabranchid != '0') {
                                var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                                // Here we are sending Branch StateID instead of Shipping StateID after discuss with
                                // Pijush Da and Debjyoti on 14122017
                                //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, CompareStateCode, schemabranch, 'P')


                                caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')


                                //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')

                            }
                        }
                        else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') {
                            var schemabranchid = $('#ddl_Branch').val();
                            if (schemabranchid != '0') {
                                var schemabranch = schemabranchid;
                                // Here we are sending Branch StateID instead of Shipping StateID after discuss with
                                // Pijush Da and Debjyoti on 14122017
                                //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, CompareStateCode, schemabranch, 'P')

                                caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')



                                //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')
                            }
                        }
                    }
                }
                // Running total Calculation Start
                Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                CalculateAmount();
                // Running total Calculation End
            }
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Amount').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }

    function DiscountAmtTextChange(s, e) {
        TDSChecking();
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var PurPrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
        var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
        var Discountamt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(PurPrice) != parseFloat(ProductPurchaseprice) || parseFloat(Discountamt) != parseFloat(ProductDiscountAmt)) {
                var SpliteDetails = ProductID.split("||@||");
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
                if (strFactor == 0) {
                    strFactor = 1;
                }
                if (strSalePrice == '0') {
                    strSalePrice = SpliteDetails[6];
                }
                if (strRate == 0) {
                    strRate = 1;
                }
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
                var Discount = ((parseFloat(Discountamt) * 100) / parseFloat(Amount));
                var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                var DiscountamtField = grid.GetEditor("Discount");
                DiscountamtField.SetValue(Discount);
                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue(amountAfterDiscount);

                var IsPackingActive = SpliteDetails[13];
                var Packing_Factor = SpliteDetails[14];
                var Packing_UOM = SpliteDetails[15];
                var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

                if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                    $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                } else {
                    divPacking.style.display = "none";
                }
                grid.GetEditor('TaxAmount').SetValue(0);
                var tbTotalAmount = grid.GetEditor("TotalAmount");
                var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue(parseFloat(amountAfterDiscount) + parseFloat(_TotalTaxAmt));
            }
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Discountamt').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
        //Debjyoti  
        var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
            grid.GetEditor('TaxAmount').SetValue(0);
            // ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
            deleteTax('DelQtybySl', grid.GetEditor("SrlNo").GetValue(), "");
            var incluexclutype = ''
            var taxtype = cddl_AmountAre.GetValue();
            if (taxtype == '1') {
                incluexclutype = 'E'
            }
            else if (taxtype == '2') {
                incluexclutype = 'I'
            }

            var CompareStateCode;
            if (cddlPosGstInvoice.GetValue() == "S") {
                CompareStateCode = GeteShippingStateCode();
            }
            else {
                CompareStateCode = GetBillingStateCode();
            }


            var checkval = cchk_reversemechenism.GetChecked();
            if (!checkval) {
                if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                    var schemabranchid = $('#ddl_numberingScheme').val();
                    if (schemabranchid != '0') {
                        var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                        // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                        // Pijush Da and Debjyoti on 14122017
                        // caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, 'P')
                        //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')

                        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')


                    }
                }
                else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') {
                    var schemabranchid = $('#ddl_Branch').val();
                    if (schemabranchid != '0') {
                        var schemabranch = schemabranchid;
                        // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                        // Pijush Da and Debjyoti on 14122017
                        // caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')
                        //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')

                        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')

                    }
                }
            }
        }
        // Running total Calculation Start
        Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        CalculateAmount();
        // Running total Calculation End 
    }

    function DiscountTextChange(s, e) {
        TDSChecking();
        var PurPrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
        var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(PurPrice) != parseFloat(ProductPurchaseprice)) {
                var SpliteDetails = ProductID.split("||@||");
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
                if (strFactor == 0) {
                    strFactor = 1;
                }
                if (strSalePrice == '0') {
                    strSalePrice = SpliteDetails[6];
                }
                if (strRate == 0) {
                    strRate = 1;
                }
                var Amount = parseFloat(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                var Discountamt = parseFloat((parseFloat(Discount) * parseFloat(Amount)) / 100).toFixed(2);
                var amountAfterDiscount = parseFloat(parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100)).toFixed(2);

                var DiscountamtField = grid.GetEditor("Discountamt");
                DiscountamtField.SetValue(Discountamt);

                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue(amountAfterDiscount);

                var IsPackingActive = SpliteDetails[13];
                var Packing_Factor = SpliteDetails[14];
                var Packing_UOM = SpliteDetails[15];
                var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
                if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                    $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                } else {
                    divPacking.style.display = "none";
                }
                grid.GetEditor('TaxAmount').SetValue(0);
                var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue(parseFloat(parseFloat(amountAfterDiscount) + parseFloat(_TotalTaxAmt)).toFixed(2));
            }
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Discount').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
        var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
            grid.GetEditor('TaxAmount').SetValue(0);
            // ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
            deleteTax('DelQtybySl', grid.GetEditor("SrlNo").GetValue(), "");
            var incluexclutype = ''
            var taxtype = cddl_AmountAre.GetValue();
            if (taxtype == '1') {
                incluexclutype = 'E'
            }
            else if (taxtype == '2') {
                incluexclutype = 'I'
            }


            var CompareStateCode;
            if (cddlPosGstInvoice.GetValue() == "S") {
                CompareStateCode = GeteShippingStateCode();
            }
            else {
                CompareStateCode = GetBillingStateCode();
            }
            var checkval = cchk_reversemechenism.GetChecked();
            if (!checkval) {
                if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                    var schemabranchid = $('#ddl_numberingScheme').val();
                    if (schemabranchid != '0') {
                        var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                        // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                        // Pijush Da and Debjyoti on 14122017
                        //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, 'P')
                        //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')

                        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')


                    }
                }
                else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') {
                    var schemabranchid = $('#ddl_Branch').val();
                    if (schemabranchid != '0') {
                        var schemabranch = schemabranchid;
                        // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                        // Pijush Da and Debjyoti on 14122017
                        // caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, 'P')
                        //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')

                        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P')


                    }
                }

            }
        }

        // Running total Calculation Start
        Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        CalculateAmount();
        // Running total Calculation End

    }

    //......................Amount Calculation End.......................
    /*........................Tax Start...........................*/
    var taxAmountGlobalCharges;
    var chargejsonTax;
    var taxAmountGlobal;
    var GlobalCurChargeTaxAmt;
    var ChargegstcstvatGlobalName;
    var GlobalCurTaxAmt = 0;
    var rowEditCtrl;
    var globalRowIndex;
    var globalTaxRowIndex;
    var gstcstvatGlobalName;
    var taxJson;
    function Save_TaxClick() {
        if (gridTax.GetVisibleRowsOnPage() > 0) {
            gridTax.UpdateEdit();
        }
        else {
            gridTax.PerformCallback('SaveGst');
        }
        cPopup_Taxes.Hide();
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
                    GlobalTaxAmt = 0;
                }
                else {
                    GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                    gridTax.GetEditor("Amount").SetValue(Sum);
                    ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                    ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
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
        ctxtProductAmount.SetValue(parseFloat(Math.round(sumAmount * 100) / 100).toFixed(2));
        ctxtProductTaxAmount.SetValue(parseFloat(Math.round(sumTaxAmount * 100) / 100).toFixed(2));
        ctxtProductDiscount.SetValue(parseFloat(Math.round(sumDiscount * 100) / 100).toFixed(2));
        ctxtProductNetAmount.SetValue(parseFloat(Math.round(sumNetAmount * 100) / 100).toFixed(2));
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
    function QuotationTaxAmountTextChange(s, e) {
        var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
        var GlobalTaxAmt = 0;
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
            ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges);
            ctxtTotalAmount.SetText(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
            GlobalTaxAmt = 0;
            SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
        }
        else {
            GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
            ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
            ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
            GlobalTaxAmt = 0;
            SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
        }
        RecalCulateTaxTotalAmountCharges();
    }
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
    function QuotationTaxAmountGotFocus(s, e) {
        taxAmountGlobalCharges = parseFloat(s.GetValue());
    }
    function PercentageTextChange(s, e) {
        var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
        var GlobalTaxAmt = 0;
        var Percentage = s.GetText();
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);
        if (sign == '(+)') {
            GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
            gridTax.GetEditor("Amount").SetValue(Sum);
            ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
            ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
            GlobalTaxAmt = 0;
        }
        else {
            GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
            gridTax.GetEditor("Amount").SetValue(Sum);
            ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
            ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
            GlobalTaxAmt = 0;
        }
        SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
        SetChargesRunningTotal();
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
    function TaxAmountKeyDown(s, e) {
        if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
            s.OnButtonClick(0);
        }
    }
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


        // SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
        SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);
        //Set Running Total
        SetRunningTotal();

        RecalCulateTaxTotalAmountInline();
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

    function SetOtherTaxValueOnRespectiveRow(idx, amt, name, runninTot, signCal) {
        for (var i = 0; i < taxJson.length; i++) {
            if (taxJson[i].applicableBy == name) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                var totCal = 0;
                if (signCal == '(+)') {
                    totCal = parseFloat(parseFloat(amt) + parseFloat(runninTot));
                }
                else {
                    totCal = parseFloat(parseFloat(runninTot) - parseFloat(amt));
                }
                cgridTax.GetEditor('calCulatedOn').SetValue(totCal);

                var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                var s = cgridTax.GetEditor("TaxField");
                if (sign == '(+)') {
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                    //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                    ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue()) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
                    GlobalCurTaxAmt = 0;
                }
                else {
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                    //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                    ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (s.GetText() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
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
        gridTax.batchEditApi.EndEdit();
    }
    function txtPercentageLostFocus(s, e) {
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
                //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
                SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);

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
        //

        var runningTot = parseFloat(clblProdNetAmt.GetValue());
        for (var i = 0; i < taxJson.length; i++) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (taxJson[i].applicableOn == "R") {
                cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
                var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();

                var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                var thisRunningAmt = 0;
                if (sign == '(+)') {
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                    //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
                    ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (cgridTax.GetEditor("TaxField").GetValue() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));

                    GlobalCurTaxAmt = 0;
                }
                else {

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                    //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                    ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (cgridTax.GetEditor("TaxField").GetValue() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
                    GlobalCurTaxAmt = 0;
                }
                SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), ProdAmt, sign);
            }
            if (sign == '(+)') {
                runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
            }
            else {
                runningTot = runningTot - parseFloat(cgridTax.GetEditor("Amount").GetValue());
            }

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
    function CmbtaxClick(s, e) {
        GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
        gstcstvatGlobalName = s.GetText();
    }
    function txtTax_TextChanged(s, i, e) {
        cgridTax.batchEditApi.StartEdit(i, 2);
        var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
        cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
    }
    function NonInventoryBatchUpdate() {
        var cnt = cgridinventory.GetVisibleItemsOnPage();
        //cgridinventory.PerformCallback('0' + '~' + '0' + '~' + 'SaveNonInventoryProductChg' + '~' + cnt); 
        cgridinventory.AddNewRow();
        cgridinventory.batchEditApi.StartEdit(0, 3);
        cgridinventory.batchEditApi.EndEdit();
        if (cgridinventory.GetVisibleRowsOnPage() > 0) {
            cgridinventory.UpdateEdit();
        }
        //cgridinventory.UpdateEdit();
        return false;
    }
    function BatchUpdate() {

        $('#ddl_numberingScheme').prop('disabled', true);
        //gridLookup.SetEnabled(false);
        ctxtVendorName.SetEnabled(false);

        if (cgridTax.GetVisibleRowsOnPage() > 0) {
            cgridTax.UpdateEdit();
        }
        else {
            cgridTax.PerformCallback('SaveGST');
        }
        return false;
    }
    function cgridTax_EndCallBack(s, e) {
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
            //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section Start
            //grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
            //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section End
            if (cddl_AmountAre.GetValue() == '2') {

                //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section Start
                var prodqty = (grid.GetEditor("Quantity").GetValue());
                var prodrate = (grid.GetEditor("PurchasePrice").GetValue());
                var prodDiscAmt = (grid.GetEditor("Discountamt").GetValue());
                var prodNetAmt = (parseFloat(prodqty) * parseFloat(prodrate)) - parseFloat(prodDiscAmt)
                grid.GetEditor("TotalAmount").SetValue(parseFloat(prodNetAmt));
                //var prodNetAmt = (grid.GetEditor("TotalAmount").GetValue());
                var prodIncluSiveTax = (grid.GetEditor("TaxAmount").GetValue());
                grid.GetEditor("Amount").SetValue(parseFloat(prodNetAmt) - parseFloat(prodIncluSiveTax));
                //var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue());
                //var totalRoundOffAmount = Math.round(totalNetAmount);
                //grid.GetEditor("Amount").SetValue(parseFloat(grid.GetEditor("Amount").GetValue()) + (totalRoundOffAmount - totalNetAmount));
                //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section End
            }
            else {
                grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
            }
            // Running total Calculation Start
            Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            CalculateAmount();
            // Running total Calculation End 
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
    /*............................End Tax...........................................*/


    function BindOrderProjectdata(OrderId, TagDocType) {
        debugger;
        clookup_Project.SetEnabled(false);
        var OtherDetail = {};

        OtherDetail.OrderId = OrderId;
        OtherDetail.TagDocType = TagDocType;


        if ((OrderId != null) && (OrderId != "")) {

            $.ajax({
                type: "POST",
                url: "ProjectPurchaseInvoice.aspx/SetProjectCode",
                data: JSON.stringify(OtherDetail),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var Code = msg.d;

                    clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                    clookup_Project.SetEnabled(false);
                }
            });

            //Hierarchy Start Tanmoy
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'ProjectPurchaseInvoice.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
            //Hierarchy End Tanmoy
        }
    }


    function PerformCallToGridBind() {

        if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                var loadingmade = $('#<%=hdnADDEditMode.ClientID %>').val();
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
                if (quote_Id.length > 0) {

                    if ($("#hdnProjectSOTC").val() == "1") {
                        CallTaggedOrderTermsCondition(quote_Id[0], "ProjPC");

                    }
                }


            }
            else {
                cProductsPopup.Hide();
            }
            return false;
        }
        function componentEndCallBack(s, e) {
            debugger;


            var loadingmode = $('#<%=hdnADDEditMode.ClientID %>').val();
            if (loadingmode != 'Edit') {
                gridquotationLookup.gridView.Refresh();
                cProductsPopup.Hide();
            }
            if (grid.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick();
            }
            // Tagging Component Section Start
            if (cQuotationComponentPanel.cpDetails != null) {
                var details = cQuotationComponentPanel.cpDetails;
                cQuotationComponentPanel.cpDetails = null;
                var SpliteDetails = details.split("~");
                var Reference = SpliteDetails[0];
                var Currency_Id = SpliteDetails[1];
                var SalesmanId = SpliteDetails[2];
                var ExpiryDate = SpliteDetails[3];
                var CurrencyRate = SpliteDetails[4];
                ctxt_Refference.SetValue(Reference);
                ctxtRate.SetValue(CurrencyRate);
                document.getElementById('ddl_Currency').value = Currency_Id;
                document.getElementById('ddl_SalesAgent').value = SalesmanId;
            }
            gridquotationLookup.Focus();
        }
        var SimilarProjectStatus = "0";


        function SimilarProjetcheck(quote_Id, Doctype) {
            $.ajax({
                type: "POST",
                url: "ProjectPurchaseInvoice.aspx/DocWiseSimilarProjectCheck",
                data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    SimilarProjectStatus = msg.d;
                    debugger;
                    if (SimilarProjectStatus != "1") {
                        cPLQADate.SetText("");
                        jAlert("Please select document with same project code to proceed.");

                        return false;

                    }
                }
            });
        }

        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();

            var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {
                var Doctype = $("#rdl_PurchaseInvoice").find(":checked").val();
                var quote_Id = "";
                // otherDets.quote_Id = quote_Id;
                for (var i = 0; i < quotetag_Id.length; i++) {
                    if (quote_Id == "") {
                        quote_Id = quotetag_Id[i];
                    }
                    else {
                        quote_Id += ',' + quotetag_Id[i];
                    }
                }

                SimilarProjetcheck(quote_Id, Doctype);

            }

        }
        //function QuotationNumberChanged() {
        //    var quote_Id = gridquotationLookup.GetValue();
        //    if (quote_Id != null) {
        //        griducts.PerformCallback('BindProductsDetails' + '~' + '@' + '~' + quote_Id);
        //        cProductsPopup.Show();
        //    }
        //    else {
        //        griducts.PerformCallback('BindProductsDetails' + '~' + '$');
        //        cProductsPopup.Show();
        //    }
        //}


        //Chinmoy added below function
        function validateOrderwithAmountAre() {
            //Check Multiple Row amount are selected or not

            var selectedKeys = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            var ammountsAreOrder = "";
            if (selectedKeys.length > 0) {
                for (var loopcount = 0 ; loopcount < gridquotationLookup.gridView.GetVisibleRowsOnPage() ; loopcount++) {

                    var nowselectedKey = gridquotationLookup.gridView.GetRowKey(loopcount);

                    var found = selectedKeys.find(function (element) {
                        return element == nowselectedKey;
                    });

                    if (found) {
                        if (ammountsAreOrder != "" && ammountsAreOrder != gridquotationLookup.gridView.GetRow(loopcount).children[9].innerText) {

                            jAlert("Unable to procceed. Amount are for the selected order(s) are different");

                            return false;
                        }
                        else
                            ammountsAreOrder = gridquotationLookup.gridView.GetRow(loopcount).children[9].innerText;
                    }

                }

            }

            return true;
        }
        //End



        function QuotationNumberChanged() {
            debugger;
            var quote_Id = gridquotationLookup.GetValue();

            if (SimilarProjectStatus != "-1") {

                if (quote_Id != null) {
                    //Commented by chinmoy
                    //griducts.PerformCallback('BindProductsDetails' + '~' + '@' + '~' + quote_Id);
                    // cProductsPopup.Show();


                    var OrderData = gridquotationLookup.gridView.GetSelectedKeysOnPage();

                    if (validateOrderwithAmountAre() == false) {
                        var startDate = new Date();
                        startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
                        var key = GetObjectID('hdnCustomerId').value;
                        if (key != null && key != '') {
                            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                        }
                        var invtype = $('#ddlInventory').val();
                        if (key != null && key != '') {
                            cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                        }
                        grid.PerformCallback('GridBlank');
                        //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        deleteTax('DeleteAllTax', "", "");
                        gridquotationLookup.SetText('');
                        gridquotationLookup.Clear();

                        cProductsPopup.Hide();

                    }
                    else if (OrderData == 0) {
                        gridquotationLookup.gridView.Hide();
                    }
                    else if (OrderData != 0 && validateOrderwithAmountAre() == true) {

                        if (gridquotationLookup.gridView.GetSelectedRowCount() > 0) {
                            cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@' + '~' + quote_Id);
                            cProductsPopup.Show();
                            //chinmoy start  02-07-2019
                            var selectedrow = gridquotationLookup.gridView.GetSelectedRowCount();
                            //if(selectedrow==1)
                            //{

                            //    var partyinvoice = gridquotationLookup.gridView.GetRow(0).children[7].innerText;
                            //    ctxt_partyInvNo.SetText(partyinvoice);

                            //    var partyinvoicedate = gridquotationLookup.gridView.GetRow(0).children[8].innerText;
                            //    var date = partyinvoicedate;
                            //    var datearray = date.split("/");
                            //    if (datearray[1].length == 1)
                            //    {
                            //        datearray[1] = '0' + datearray[1];
                            //    }
                            //    if (datearray[0].length == 1) {
                            //        datearray[0] = '0' + datearray[0];
                            //    }
                            //    var newdate = datearray[1] + '-' + datearray[0] + '-' + datearray[2];
                            //    cdt_partyInvDt.SetText(newdate);

                            //}
                            if (selectedrow > 1) {
                                //gridquotationLookup.gridView.GetSelectedRowCount();
                                for (var loopcount = 0; loopcount < gridquotationLookup.gridView.GetSelectedRowCount() ; loopcount++) {
                                    //if (gridquotationLookup.gridView.cppartyInvoiceList != null) {
                                    //    var partyinvoice = gridquotationLookup.gridView.cppartyInvoiceList;
                                    //    ctxt_partyInvNo.SetText(partyinvoice);
                                    //}


                                    if (loopcount == 0) {
                                        var partyinvoicedate = gridquotationLookup.gridView.GetRow(loopcount).children[8].innerText;
                                        var date = partyinvoicedate;
                                        var datearray = date.split("/");
                                        if (datearray[1].length == 1) {
                                            datearray[1] = '0' + datearray[1];
                                        }
                                        if (datearray[0].length == 1) {
                                            datearray[0] = '0' + datearray[0];
                                        }
                                        var newdate = datearray[1] + '-' + datearray[0] + '-' + datearray[2];
                                        cdt_partyInvDt.SetText(newdate);
                                    }
                                }
                            }


                            //End
                        }
                    }


                }
                else {
                    if (gridquotationLookup.gridView.GetSelectedRowCount() > 0) {
                        cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
                        cProductsPopup.Show();
                        var selectedrow = gridquotationLookup.gridView.GetSelectedRowCount();
                        if (selectedrow > 0) {
                            var partyinvoice = gridquotationLookup.gridView.GetRow(0).children[7].innerText;
                            ctxt_partyInvNo.SetText(partyinvoice);
                        }
                    }
                }
            }
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

        function OnInventoryEndCallback(s, e) {
            if (cgridinventory.cpPopupReq == 'N') {
                OnAddNewClick();
                jAlert('Applicable amount is greater than entered amount.TDS is not required for this amount.')
                return;
            }
            else if (cgridinventory.cpgrid == "Y") {

                cgridinventory.batchEditApi.StartEdit(0, 2);

                if (cgridinventory.cpEditTDS == "1") {
                    cchk_TDSEditable.SetValue(true);
                }
                else {
                    cchk_TDSEditable.SetValue(false);
                }
                cgridinventory.cpEditTDS = null;
                var checkval = cchk_TDSEditable.GetChecked();
                if (checkval) {
                    cgridinventory.SetEnabled(true);
                }
                else {
                    cgridinventory.SetEnabled(false);
                }
                if (cgridinventory.cpbrMonAmtDtl != null) {
                    var tdsbranch = cgridinventory.cpbrMonAmtDtl.toString().split('~')[0];
                    var tdsmonth = cgridinventory.cpbrMonAmtDtl.toString().split('~')[1];
                    var tdsTotalAmt = cgridinventory.cpbrMonAmtDtl.toString().split('~')[2];
                    var tdsProbAmt = cgridinventory.cpbrMonAmtDtl.toString().split('~')[3];
                    //var TDSEdit = cgridinventory.cpbrMonAmtDtl.toString().split('~')[4];
                    //if (TDSEdit == '1')
                    //{
                    //    cchk_TDSEditable.SetValue(true);
                    //}
                    //else
                    //{
                    //    cchk_TDSEditable.SetValue(false);
                    //}
                    var branchid = $("#ddl_Branch option:selected").text();
                    $('#lbltdsBranch').text(branchid);
                    if (cgridinventory.cpmonth != null) {
                        var monthnm = cgridinventory.cpmonth;
                        if (monthnm == '1') {
                            cddl_month.SetValue('January');
                        }
                        else if (monthnm == '2') {
                            cddl_month.SetValue('February');
                        }
                        else if (monthnm == '3') {
                            cddl_month.SetValue('March');
                        }
                        else if (monthnm == '4') {
                            cddl_month.SetValue('April');
                        }
                        else if (monthnm == '5') {
                            cddl_month.SetValue('May');
                        }
                        else if (monthnm == '6') {
                            cddl_month.SetValue('June');
                        }
                        else if (monthnm == '7') {
                            cddl_month.SetValue('July');
                        }
                        else if (monthnm == '8') {
                            cddl_month.SetValue('August');
                        }
                        else if (monthnm == '9') {
                            cddl_month.SetValue('September');
                        }
                        else if (monthnm == '10') {
                            cddl_month.SetValue('October');
                        }
                        else if (monthnm == '11') {
                            cddl_month.SetValue('November');
                        }
                        else if (monthnm == '12') {
                            cddl_month.SetValue('December');
                        }
                    }
                    cddl_month.SetValue(tdsmonth);
                    if (tdsTotalAmt != null && tdsTotalAmt != '') {
                        ctxt_totalnoninventoryproductamt.SetText(tdsTotalAmt);
                    }
                    ctxt_proamt.SetText(tdsProbAmt);
                    cgridinventory.cpgrid = null;
                }
                else if (cgridinventory.cpbrMonAmtDtl == null) {
                    ctxt_proamt.SetText(parseFloat(cgridinventory.cpproamt));
                    ctxt_totalnoninventoryproductamt.SetText(parseFloat(cgridinventory.cpprochargeamt));
                    var branchid = $("#ddl_Branch option:selected").text();
                    $('#lbltdsBranch').text(branchid);
                    if (cgridinventory.cpmonth != null) {
                        var monthnm = cgridinventory.cpmonth;
                        if (monthnm == '1') {
                            cddl_month.SetValue('January');
                        }
                        else if (monthnm == '2') {
                            cddl_month.SetValue('February');
                        }
                        else if (monthnm == '3') {
                            cddl_month.SetValue('March');
                        }
                        else if (monthnm == '4') {
                            cddl_month.SetValue('April');
                        }
                        else if (monthnm == '5') {
                            cddl_month.SetValue('May');
                        }
                        else if (monthnm == '6') {
                            cddl_month.SetValue('June');
                        }
                        else if (monthnm == '7') {
                            cddl_month.SetValue('July');
                        }
                        else if (monthnm == '8') {
                            cddl_month.SetValue('August');
                        }
                        else if (monthnm == '9') {
                            cddl_month.SetValue('September');

                        }
                        else if (monthnm == '10') {
                            cddl_month.SetValue('October');
                        }
                        else if (monthnm == '11') {
                            cddl_month.SetValue('November');
                        }
                        else if (monthnm == '12') {
                            cddl_month.SetValue('December');
                        }
                    }
                    var checkval = cchk_TDSEditable.GetChecked();
                    if (checkval) {
                        cgridinventory.SetEnabled(true);
                    }
                    else {
                        cgridinventory.SetEnabled(false);
                    }
                }
                cinventorypopup.Show();
                cgridinventory.cpgrid = null;
            }
            else if (cgridinventory.cppopuphide == "Y") {
                var branchid = $("#ddl_Branch option:selected").text();
                $('#lbltdsBranch').text(branchid);
                cddl_month.SetSelectedIndex(-1);
                ctxt_proamt.SetText('');
                ctxt_totalnoninventoryproductamt.SetText('');
                cinventorypopup.Hide();
                OnAddNewClick();
            }
        }
        function OnEndCallback(s, e) {
            var pageStatus = document.getElementById('hdnPageStatus').value;
            var value = document.getElementById('hdnRefreshType').value;
            var pageStatus = document.getElementById('hdnPageStatus').value;
            LoadingPanel.Hide();
            if (grid.cpComponent) {
                if (grid.cpComponent == 'true') {
                    grid.cpComponent = null;
                    $('#<%=hdfIsComp.ClientID %>').val('');
                    OnAddNewClick();
                }
            }
            if (grid.cpSaveSuccessOrFail == "outrange") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');
            }
            // Rev 1.0
            else if (grid.cpSaveSuccessOrFail == "AddLock") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('DATA is Freezed between ' + grid.cpAddLockStatus);
            }
            // End of Rev 1.0
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
    
                //Mantis Issue 24274
            else if (grid.cpSaveSuccessOrFail == "TDSMandatory") {
                grid.cpSaveSuccessOrFail = null;
                ShowTDS();
                grid.cpSaveSuccessOrFail = '';
            }
                // End of Rec Sanchita

            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                OnAddNewClick();
                cbtn_SaveRecords.SetEnabled(true);
                jAlert("Duplicate Product not allowed.Save unsuccessful.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
            }
            // Rev Mantis Issue 24061
            else if (grid.cpSaveSuccessOrFail == "NetAmountExceed") {
                OnAddNewClick();
                cbtn_SaveRecords.SetEnabled(true);
                jAlert('Net Amount of selected Product from tagged document.<br />Cannot enter Net Amount more than Purchase Order Net Amount .');
                grid.cpSaveSuccessOrFail = null;
            }
            // End of Rev Mantis Issue 24061
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
                jAlert('Please Select Project.');
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
                    // window.location.assign("ProjectPurchaseInvoiceList.aspx");
                    if (PurchaseOrder_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [ProjectPurchaseInvoice]', function (r) {
                            if (r == true) {
                                if ($('#<%=hdnPBAutoPrint.ClientID %>').val() == "1") {
                                    NewExit = 'E';
                                    cPopup_NoofCopies.Show();
                                }
                                else {
                                    grid.cpPurchaseOrderNo = null;
                                    grid.cpGeneratedInvoice = null;
                                    window.location.assign("ProjectPurchaseInvoiceList.aspx");
                                }
                            }
                        });
                    }
                    else {
                        window.location.assign("ProjectPurchaseInvoiceList.aspx");
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

                        jAlert(Order_Msg, 'Alert Dialog: [ProjectPurchaseInvoice]', function (r) {
                            if (r == true) {
                                if ($('#<%=hdnPBAutoPrint.ClientID %>').val() == "1") {
                                    NewExit = 'N';
                                    cPopup_NoofCopies.Show();
                                }
                                else {
                                    grid.cpPurchaseOrderNo = null;
                                    grid.cpGeneratedInvoice = null;
                                    window.location.assign("ProjectPurchaseInvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
                                }
                                //grid.cpPurchaseOrderNo = null;
                                //grid.cpGeneratedInvoice = null;
                                //window.location.assign("ProjectPurchaseInvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
                            }
                        });
                    }
                    else {
                        window.location.assign("ProjectPurchaseInvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
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
                                    url: 'ProjectPurchaseInvoice.aspx/getSchemeType',
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
        cTotalAmt.SetValue(SUM_TotalAmount);
        cTotalQty.SetValue(SUM_ProductQuantity);
    }
}

function Call_OK() {
    cPopup_NoofCopies.Hide();
    if (ddlnoofcopies.value == "1") {
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ProjectPurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=1', '_blank')
    }
    else if (ddlnoofcopies.value == "2") {
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ProjectPurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=1', '_blank')
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ProjectPurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=2', '_blank')
    }
    else if (ddlnoofcopies.value == "3") {
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ProjectPurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=1', '_blank')
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ProjectPurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=2', '_blank')
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ProjectPurchaseInvoice-GST~D&modulename=PInvoice&id=" + grid.cpGeneratedInvoice + '&PrintOption=4', '_blank')
    }
    grid.cpPurchaseOrderNo = null;
    grid.cpGeneratedInvoice = null;
    if (NewExit == 'N') {
        window.location.assign("ProjectPurchaseInvoice.aspx?key=ADD&&InvType=" + $('#ddlInventory').val());
    }
    else if (NewExit == 'E') {
        window.location.assign("ProjectPurchaseInvoiceList.aspx");
    }

}

function GridCallBack() {
    grid.PerformCallback('Display');
}
function TDSDetail(s, e) {
    var inventoryItem = $('#ddlInventory').val();
    if (inventoryItem == 'N') {
        document.getElementById('hdnInvWiseSlno').value = grid.GetEditor('SrlNo').GetText();
        var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        var Amt = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
        var Productdt = Productdtl.split("||@||");
        var ProductID = Productdt[0];
        ctxt_proamt.SetText('Amt');
        cgridinventory.PerformCallback(ProductID + '~' + Amt + '~' + 'ShowBindGrid');
    }
}
function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.StartEdit();
        var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
        $('#<%=hdnRefreshType.ClientID %>').val('');
        $('#<%=hdnDeleteSrlNo.ClientID %>').val(SrlNo);
        var inventoryItem = $('#ddlInventory').val();
        if (inventoryItem == 'Y' || inventoryItem == 'B') {
            if (gridquotationLookup.GetValue() != null) {
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                if (type == 'PO') {

                    jAlert('Cannot Delete using this button as the Purchase Order is linked with this Purchase Invoice.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) { });
                }
                else if (type == 'PC') {
                    $('#<%=hdnDeleteSrlNo.ClientID %>').val('');
                    jAlert('Cannot Delete using this button as the Purchase Challan is linked with this Purchase Invoice.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) { });
                }
        }
        else {
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            if (noofvisiblerows != "1" && gridquotationLookup.GetValue() == null) {
                // Running total Calculation Start 
                Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                Cur_Quantity = "0";
                Cur_Amt = "0";
                Cur_TotalAmt = "0";
                CalculateAmount();
                // Running total Calculation End 
                grid.DeleteRow(e.visibleIndex);
                if (inventoryItem == 'N') {
                    $('#<%=hdinvetorttype.ClientID %>').val('N');
                }
                $('#<%=hdfIsDelete.ClientID %>').val('D');
                grid.UpdateEdit();
                $('#<%=hdnPageStatus.ClientID %>').val('delete');
            }
        }
    }
    else {
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        if (noofvisiblerows != "1") {
            // Running total Calculation Start 
            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            Cur_Quantity = "0";
            Cur_Amt = "0";
            Cur_TotalAmt = "0";
            CalculateAmount();

            // Running total Calculation End 
            grid.DeleteRow(e.visibleIndex);
            if (inventoryItem == 'N') {
                $('#<%=hdinvetorttype.ClientID %>').val('N');
            }
            $('#<%=hdfIsDelete.ClientID %>').val('D');
            grid.UpdateEdit();
            $('#<%=hdnPageStatus.ClientID %>').val('delete');
        }
    }
}
    if (e.buttonID == 'CustomAddNewRow') {
        grid.batchEditApi.StartEdit(e.visibleIndex);

        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        var scembranch = '';
        if (ProductID != "") {
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem == 'N' || inventoryItem == 'S' || inventoryItem == 'B') {
                //var customerval = gridLookup.GetValue();
                var customerval = GetObjectID('hdnCustomerId').value;
                var vendorid = GetObjectID('hdnCustomerId').value
                if (customerval == '' || customerval == null) {
                    jAlert('Select a vendor first');
                    return
                }
                else {
                    var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    if (Productdtl != null && Productdtl != '') {
                        var Productdt = Productdtl.split("||@||");
                        var ProductID = Productdt[0];
                        var schemeid = cddl_TdsScheme.GetValue()
                        if (schemeid != '0') {

                            if (inventoryItem == 'B') {
                                var ProductType = Productdt[24];
                                if (ProductType != '1') {
                                    var checkval = cchk_TDSEditable.GetChecked();
                                    if (checkval) {
                                        cgridinventory.SetEnabled(true);
                                    }
                                    else {
                                        cgridinventory.SetEnabled(false);
                                    }
                                    document.getElementById('hdnInvWiseSlno').value = grid.GetEditor('SrlNo').GetText();
                                    var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                                    var Amt = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
                                    var Productdt = Productdtl.split("||@||");
                                    var ProductID = Productdt[0];
                                    ctxt_proamt.SetText('Amt');
                                    cgridinventory.PerformCallback(ProductID + '~' + Amt + '~' + 'ShowBindGrid' + '~' + 'CheckApplicableAmt');
                                    var slno = grid.GetEditor('SrlNo').GetValue();
                                    if ($('#<%=hdntdschecking.ClientID %>').val() != '') {
                                        var myArray = $('#<%=hdntdschecking.ClientID %>').val().split(',');
                                        var index = myArray.indexOf(slno);
                                        if (index > -1) {
                                            myArray.splice(index, 1);
                                            $('#<%=hdntdschecking.ClientID %>').val(myArray);
                                        }
                                    }
                                    cgridinventory.batchEditApi.EndEdit();
                                }
                                else {
                                    OnAddNewClick();
                                }
                            }
                            else {
                                var checkval = cchk_TDSEditable.GetChecked();
                                if (checkval) {
                                    cgridinventory.SetEnabled(true);
                                }
                                else {
                                    cgridinventory.SetEnabled(false);
                                }
                                document.getElementById('hdnInvWiseSlno').value = grid.GetEditor('SrlNo').GetText();
                                var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                                var Amt = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
                                var Productdt = Productdtl.split("||@||");
                                var ProductID = Productdt[0];
                                ctxt_proamt.SetText('Amt');
                                cgridinventory.PerformCallback(ProductID + '~' + Amt + '~' + 'ShowBindGrid' + '~' + 'CheckApplicableAmt');
                                var slno = grid.GetEditor('SrlNo').GetValue();
                                if ($('#<%=hdntdschecking.ClientID %>').val() != '') {
                                    var myArray = $('#<%=hdntdschecking.ClientID %>').val().split(',');
                                    var index = myArray.indexOf(slno);
                                    if (index > -1) {
                                        myArray.splice(index, 1);
                                        $('#<%=hdntdschecking.ClientID %>').val(myArray);
                                }
                            }
                            cgridinventory.batchEditApi.EndEdit();
                        }

                    }
                    else {
                        OnAddNewClick();
                    }
                }
                else {
                    jAlert('Select a Product first.');
                    return;
                }
            }
        }
        else if (gridquotationLookup.GetValue() == null) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
            var SpliteDetails = ProductID.split("||@||");
            var IsComponentProduct = SpliteDetails[16];
            var ComponentProduct = SpliteDetails[17];
            if (IsComponentProduct == "Y") {
                var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
                jConfirm(messege, 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        grid.GetEditor("IsComponentProduct").SetValue("Y");
                        $('#<%=hdfIsComp.ClientID %>').val('C');
                            grid.UpdateEdit();
                            grid.PerformCallback('Display~fromComponent');
                        }
                        else {
                            OnAddNewClick();
                        }
                    });
                    document.getElementById('popup_ok').focus();
                }
                    // Component tagging Section End by Sam
                else if (ProductID != "") {
                    OnAddNewClick();
                }
                else {
                    grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                }
            }
            else {
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                if (type == 'PO') {
                    if ($("#hdnPartialSettings").val() == "No")
                        cgridproducts.SetEnabled(false);
                }
                else if (type == 'PC') {
                    if ($("#hdnPartialSettings").val() == "No")
                        cgridproducts.SetEnabled(false);
                }
                QuotationNumberChanged();
            }
    }
    else {
        jAlert('Select a product first');
        grid.GetEditor("ProductName").Focus();
        return;
    }
}

else if (e.buttonID == "addlDesc") {

    var index = e.visibleIndex;
    grid.batchEditApi.StartEdit(e.visibleIndex, 5);
    cPopup_InlineRemarks.Show();

    $("#txtInlineRemarks").val('');

    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
    if (ProductID != "") {
        // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
        ccallback_InlineRemarks.PerformCallback('DisplayRemarks' + '~' + SrlNo + '~' + '0');

    }
}

else if (e.buttonID == 'CustomAddNewTDS') {
    grid.batchEditApi.StartEdit(e.visibleIndex);

    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
    var scembranch = '';
    if (ProductID != "") {
        var inventoryItem = $('#ddlInventory').val();
        if (inventoryItem == 'N' || inventoryItem == 'S') {
            //var customerval = gridLookup.GetValue();
            var customerval = GetObjectID('hdnCustomerId').value;
            var vendorid = GetObjectID('hdnCustomerId').value
            if (customerval == '' || customerval == null) {
                jAlert('Select a vendor first');
                return
            }
            else {
                var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                if (Productdtl != null && Productdtl != '') {
                    var Productdt = Productdtl.split("||@||");
                    var ProductID = Productdt[0];
                    var schemeid = cddl_TdsScheme.GetValue()
                    if (schemeid != '0') {
                        var checkval = cchk_TDSEditable.GetChecked();
                        if (checkval) {
                            cgridinventory.SetEnabled(true);
                            //cgridinventory.batchEditApi.StartEdit();
                        }
                        else {
                            cgridinventory.SetEnabled(false);
                        }

                        document.getElementById('hdnInvWiseSlno').value = grid.GetEditor('SrlNo').GetText();
                        var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                        var Amt = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
                        var Productdt = Productdtl.split("||@||");
                        var ProductID = Productdt[0];
                        ctxt_proamt.SetText('Amt');
                        // Code Added by sam to set focus on TDS gridview on 05022018

                        // Code Above Added by sam to set focus on TDS gridview on 05022018
                        cgridinventory.PerformCallback(ProductID + '~' + Amt + '~' + 'ShowBindGrid' + '~' + 'CheckApplicableAmt');
                        var slno = grid.GetEditor('SrlNo').GetValue();
                        if ($('#<%=hdntdschecking.ClientID %>').val() != '') {
                                var myArray = $('#<%=hdntdschecking.ClientID %>').val().split(',');
                                var index = myArray.indexOf(slno);
                                if (index > -1) {
                                    myArray.splice(index, 1);
                                    $('#<%=hdntdschecking.ClientID %>').val(myArray);
                            }

                        }
                        cgridinventory.batchEditApi.EndEdit();
                    }
                    else {
                            //OnAddNewClick();
                    }
                }
                else {
                    jAlert('Select a Product first.');
                    return;
                }
            }
        }
        else if (gridquotationLookup.GetValue() == null) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
            var SpliteDetails = ProductID.split("||@||");
            var IsComponentProduct = SpliteDetails[16];
            var ComponentProduct = SpliteDetails[17];
            if (IsComponentProduct == "Y") {
                var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
                jConfirm(messege, 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        grid.GetEditor("IsComponentProduct").SetValue("Y");
                        $('#<%=hdfIsComp.ClientID %>').val('C');
                        grid.UpdateEdit();
                        grid.PerformCallback('Display~fromComponent');
                    }
                    else {
                        OnAddNewClick();
                    }
                });
                document.getElementById('popup_ok').focus();
            }
                // Component tagging Section End by Sam
            else if (ProductID != "") {
                OnAddNewClick();
            }
            else {
                grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            }
        }
        else {
            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
            if (type == 'PO') {
                cgridproducts.SetEnabled(false);
            }
            else if (type == 'PC') {
                cgridproducts.SetEnabled(false);
            }
            QuotationNumberChanged();
        }
}
else {
    jAlert('Select a product first');
    grid.GetEditor("ProductName").Focus();
    return;
}
}

else if (e.buttonID == 'CustomWarehouse') {
    var inventoryItem = $('#ddlInventory').val();
    if (inventoryItem == 'N') {
        jAlert("NonInventory Item has no warehouse detail.")
        return
    }
    var index = e.visibleIndex;
    grid.batchEditApi.StartEdit(index, 2)
    Warehouseindex = index;
    document.getElementById('hdngridvselectedrowno').value = e.visibleIndex;
    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
    var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
    if (type != 'PC' && type != 'PO') {
        return;
    }
    if (QuantityValue == "0.0") {
        jAlert("Quantity should not be zero !.");
    }
    else {
        if (type == 'PC' || type == 'PO') {
            cbtnaddWarehouse.SetVisible(false); // During Tagging Add button should be Enabled false
            cbtnWarehouse.SetVisible(false);
            cButton1.SetVisible(false);
            cbtnrefreshWarehouse.SetVisible(false);
            cCmbWarehouse.SetEnabled(false);
        }
        else {

            cbtnaddWarehouse.SetVisible(true);
            cbtnWarehouse.SetVisible(true);
            cButton1.SetVisible(true);
            cbtnrefreshWarehouse.SetVisible(true);
            cCmbWarehouse.SetEnabled(true);
        }
        $("#spnCmbWarehouse").hide();  // First Hide during stock detail clicking
        $("#spnCmbBatch").hide();      // First Hide during stock detail clicking
        $("#spncheckComboBox").hide(); // First Hide during stock detail clicking
        $("#spntxtQuantity").hide();   // First Hide during stock detail clicking
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
            $('#<%=hdfProductIDPC.ClientID %>').val(strProductID);  // assign Productid of the selected row
            $('#<%=hdfProductType.ClientID %>').val("");            // assign Producttype of the selected row
            $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);     // assign sl no of the selected row 
            $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue); // assign Product qty of the selected row
            var Ptype = "";
            $('#<%=hdnisserial.ClientID %>').val("");        // serial id is black initialized in first time
            $('#<%=hdnisbatch.ClientID %>').val("");     // Batch id is black initialized in first time
            $('#<%=hdniswarehouse.ClientID %>').val("");    // Warehouse id is black initialized in first time 
            $.ajax({
                type: "POST",
                url: 'ProjectPurchaseInvoice.aspx/getProductType',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{Products_ID:\"" + strProductID + "\"}",
                success: function (type) {
                    Ptype = type.d;
                    $('#<%=hdfProductType.ClientID %>').val(Ptype);
                    if (Ptype == "") {
                        jAlert("No Warehouse or Batch or Serial is actived !.");
                    } else {
                        if (Ptype == "W") {
                            $('#<%=hdnisserial.ClientID %>').val("false");
                            $('#<%=hdnisbatch.ClientID %>').val("false");
                            $('#<%=hdniswarehouse.ClientID %>').val("true");
                        }
                        else if (Ptype == "B") {
                            $('#<%=hdnisserial.ClientID %>').val("false");
                            $('#<%=hdnisbatch.ClientID %>').val("true");
                            $('#<%=hdniswarehouse.ClientID %>').val("false");
                        }
                        else if (Ptype == "S") {
                            $('#<%=hdnisserial.ClientID %>').val("true");
                            $('#<%=hdnisbatch.ClientID %>').val("false");
                            $('#<%=hdniswarehouse.ClientID %>').val("false");
                        }
                        else if (Ptype == "WB") {
                            $('#<%=hdnisserial.ClientID %>').val("false");
                            $('#<%=hdnisbatch.ClientID %>').val("true");
                            $('#<%=hdniswarehouse.ClientID %>').val("true");
                        }
                        else if (Ptype == "WS") {
                            $('#<%=hdnisserial.ClientID %>').val("true");
                            $('#<%=hdnisbatch.ClientID %>').val("false");
                            $('#<%=hdniswarehouse.ClientID %>').val("true");
                        }
                        else if (Ptype == "WBS") {
                            $('#<%=hdnisserial.ClientID %>').val("true");
                                $('#<%=hdnisbatch.ClientID %>').val("true");
                                $('#<%=hdniswarehouse.ClientID %>').val("true");
                            }
                            else if (Ptype == "BS") {
                                $('#<%=hdnisserial.ClientID %>').val("true");
                                $('#<%=hdnisbatch.ClientID %>').val("true");
                                $('#<%=hdniswarehouse.ClientID %>').val("false");
                            }
                            else {
                                $('#<%=hdnisserial.ClientID %>').val("false");
                                $('#<%=hdnisbatch.ClientID %>').val("false");
                                $('#<%=hdniswarehouse.ClientID %>').val("false");
                            }
    $("#RequiredFieldValidatortxtbatch").css("display", "none");         // validation are hidden in starting for batch detail txt box
    $("#RequiredFieldValidatortxtserial").css("display", "none");        // validation are hidden in starting for serial detail txt box
    $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");     // validation are hidden in starting for ware house detail txt box 
    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");   // validation are hidden in starting for batch qty txt box
    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");    // validation are hidden in starting for warehouse qty txt box 
    $(".blockone").css("display", "none");                // this div is hidden for warehouse detail in starting
    $(".blocktwo").css("display", "none");                 // this div is hidden for Batch detail in starting
    $(".blockthree").css("display", "none");             // this div is hidden for Serial detail in starting 
    ctxtqnty.SetText("0.0");            // for warehouse quantity text box
    ctxtqnty.SetEnabled(true);
    ctxtbatchqnty.SetText("0.0");       // for Batch quantity text box not for detail text box
    ctxtserial.SetText("");             // for serial  detail text box
    ctxtbatchqnty.SetText("");
    ctxtbatch.SetEnabled(true);        // for Batch Number text box
    cCmbWarehouse.SetEnabled(true);
                        <%--$('#<%=hdnoutstock.ClientID %>').val("0");--%>              // starting Phase
                        $('#<%=hdnisedited.ClientID %>').val("false");           // starting Phase
                        $('#<%=hdnisoldupdate.ClientID %>').val("false");        // starting Phase
                        $('#<%=hdnisnewupdate.ClientID %>').val("false");        // starting Phase 
                        $('#<%=hdnisolddeleted.ClientID %>').val("false");       // starting Phase 
                        $('#<%=hdntotalqntyPC.ClientID %>').val(0);              // starting Phase
                        $('#<%=hdnoldrowcount.ClientID %>').val(0);              // starting Phase
                        $('#<%=hdndeleteqnity.ClientID %>').val(0);              // starting Phase
                        $('#<%=hidencountforserial.ClientID %>').val("1");       // starting Phase 
                        $('#<%=hdfstockidPC.ClientID %>').val(0);               // starting Phase
                        $('#<%=hdfopeningstockPC.ClientID %>').val(0);          // starting Phase
                        $('#<%=oldopeningqntity.ClientID %>').val(0);           // starting Phase
                        $('#<%=hdnnewenterqntity.ClientID %>').val(0);          // starting Phase 
                        $('#<%=hdnenterdopenqnty.ClientID %>').val(0);         // starting Phase
                        $('#<%=hdbranchIDPC.ClientID %>').val(0);              // starting Phase 
                        $('#<%=hdnisviewqntityhas.ClientID %>').val("false");  // starting Phase 
                        $('#<%=hdndefaultID.ClientID %>').val("");             // starting Phase
                        $('#<%=hdnbatchchanged.ClientID %>').val("0");        // starting Phase
                        $('#<%=hdnrate.ClientID %>').val("0");                // starting Phase
                        $('#<%=hdnvalue.ClientID %>').val("0");               // starting Phase
                        $('#<%=hdnstrUOM.ClientID %>').val(strUOM);          // starting Phase 
                        var branchid = $("#ddl_Branch option:selected").val();
                                <%--$('#<%=hdnisreduing.ClientID %>').val("false");  --%>
                        var productid = SpliteDetails[0] ? SpliteDetails[0] : "";
                        var StkQuantityValue = QuantityValue ? QuantityValue : "0.0000";
                        var stockids = 0;
                        var checkstockdecimalornot = StkQuantityValue.toString().split(".")[1]
                        $('#<%=hdnpcslno.ClientID %>').val(SrlNo);
                        var ProductName = SpliteDetails[1] ? SpliteDetails[1] : "";
                        var ratevalue = "0";
                        var rate = "0";
                        var branchid = $('#<%=ddl_Branch.ClientID %>').val();
                        var BranchNames = $("#ddl_Branch option:selected").text();
                        var strProductID = productid;
                        var strDescription = "";
                        var strUOM = (strUOM != null) ? strUOM : "0";
                        var strProductName = ProductName;
                        document.getElementById('<%=lblbranchName.ClientID %>').innerHTML = BranchNames;
                        var availablestock = SpliteDetails[12];
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
                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        if (iswarehousactive == "true") {    // If Product type Has WH or W Type

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
                            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
                            cPopup_WarehousePC.Show();
                        }
                }
            });
            }
        }
    }
}
function Save_ButtonClick() {

    $("#hdn_party_inv_no").val(ctxt_partyInvNo.GetText());
    flag = true;
    LoadingPanel.Show();
    cbtn_SaveRecords.SetEnabled(false);
    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();
    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        flag = false;
        $("#MandatoryBillNo").show();
        LoadingPanel.Hide();
        return false;
    }

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
        return false;
    }

    if (($("#hdBillingPin").val() == "0") && ($("#hdShippingPin").val() == "0")) {
        LoadingPanel.Hide();
        flag = false;

        jAlert('Please enter pin number.');
        page.SetActiveTabIndex(1);
        return false;
    }


    var Podt = cPLQuoteDate.GetValue();
    if (Podt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDate").show();
        flag = false;
        return false;
    }
    var invtype = $('#ddlInventory').val();
    if (invtype != 'N') {
        if (ctxt_partyInvNo.GetText() == '' || ctxt_partyInvNo.GetText() == null) {
            flag = false;
            $("#MandatorysPartyinvno").show();
            LoadingPanel.Hide();
            return false;
        }
    }
    // Invoice Date validation Start
    var sdate = cdt_partyInvDt.GetValue();
    var edate = cPLQuoteDate.GetValue();
    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (invtype == 'N') {
        if (sdate == null || sdate == "") {
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');

            if (startDate > endDate) {
                LoadingPanel.Hide();
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
    }
    else if (invtype != 'N') {
        if (sdate == null || sdate == "") {
            flag = false;
            $('#MandatoryPartyDate').attr('style', 'display:block');
            LoadingPanel.Hide();
            return false;
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');
            if (startDate > endDate) {
                LoadingPanel.Hide();
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
    }
    //var customerval = gridLookup.GetValue();
    var customerval = GetObjectID('hdnCustomerId').value
    if (customerval == '' || customerval == null) {
        LoadingPanel.Hide();
        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }
    if (Podt == null) {
        $("#MandatoryDate").show();
        LoadingPanel.Hide();
        return false;
    }


    var hdnAlertRetention = $('#hdnAlertRetention').val();
    var RetenValue = $("#hdnSettingsShowRetention").val();
 


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
    if (hdnAlertRetention == '1' && RetenValue != 'ShowRetention') {

        jConfirm('Do you want to Enter Retention?', 'Confirmation Dialog', function (r) {
            if (r == true) {

                $("#newModal").modal('show');
                $("#hdnSettingsShowRetention").val('ShowRetention');

                LoadingPanel.Hide();
                flag = false;
                return false;

            }
            else {

                if (flag != false) {



                    if (IsProduct == "Y") {
                        var inventoryItem = $('#ddlInventory').val();
                        if (inventoryItem == 'N' && cddl_TdsScheme.GetValue() != '0') {
                            if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                                    var tdschk = $('#<%=hdntdschecking.ClientID %>').val()
                                    if (tdschk != '') {
                                        LoadingPanel.Hide();
                                        jAlert('Please enter TDS detail for Sl no.' + tdschk)
                                        return false;
                                    }
                                }
                            }
                            if (inventoryItem == 'S' && cddl_TdsScheme.GetValue() != '0') {
                                if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                                    var tdschk = $('#<%=hdntdschecking.ClientID %>').val()
                                    if (tdschk != '') {
                                        LoadingPanel.Hide();
                                        jAlert('Please enter TDS detail for Sl no.' + tdschk)
                                        return false;
                                    }
                                }
                            }

                            if (grid.GetVisibleRowsOnPage() > 0) {
                                //Subhra 18-03-2019
                                if (issavePacking == 1 && aarr.length > 0) {
                                    $.ajax({
                                        type: "POST",
                                        url: "ProjectPurchaseInvoice.aspx/SetSessionPacking",
                                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (msg) {
                                            var customerval = GetObjectID('hdnCustomerId').value;
                                            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                                $('#<%=hdnRefreshType.ClientID %>').val('E');
                                $('#<%=hdfIsDelete.ClientID %>').val('I');
                                grid.batchEditApi.EndEdit();
                                cacbpCrpUdf.PerformCallback();
                            }
                        });
                    }
                    else {
                        //Subhra 18-03-2019
                        //var customerval = gridLookup.GetValue();
                        var customerval = GetObjectID('hdnCustomerId').value;
                        $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                        $('#<%=hdnRefreshType.ClientID %>').val('E');
                        $('#<%=hdfIsDelete.ClientID %>').val('I');
                        grid.batchEditApi.EndEdit();
                        cacbpCrpUdf.PerformCallback();
                    }
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('Please add atleast single record first');
                }
            }
            else {
                LoadingPanel.Hide();
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }
        }

    }



        });


}

else {


    if (flag != false) {



        if (IsProduct == "Y") {
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem == 'N' && cddl_TdsScheme.GetValue() != '0') {
                if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                    var tdschk = $('#<%=hdntdschecking.ClientID %>').val()
                    if (tdschk != '') {
                        LoadingPanel.Hide();
                        jAlert('Please enter TDS detail for Sl no.' + tdschk)
                        return false;
                    }
                }
            }
            if (inventoryItem == 'S' && cddl_TdsScheme.GetValue() != '0') {
                if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                        var tdschk = $('#<%=hdntdschecking.ClientID %>').val()
                        if (tdschk != '') {
                            LoadingPanel.Hide();
                            jAlert('Please enter TDS detail for Sl no.' + tdschk)
                            return false;
                        }
                    }
                }

                if (grid.GetVisibleRowsOnPage() > 0) {
                    //Subhra 18-03-2019
                    if (issavePacking == 1 && aarr.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "ProjectPurchaseInvoice.aspx/SetSessionPacking",
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var customerval = GetObjectID('hdnCustomerId').value;
                                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                                $('#<%=hdnRefreshType.ClientID %>').val('E');
                                $('#<%=hdfIsDelete.ClientID %>').val('I');
                                grid.batchEditApi.EndEdit();
                                cacbpCrpUdf.PerformCallback();
                            }
                        });
                    }
                    else {
                        //Subhra 18-03-2019
                        //var customerval = gridLookup.GetValue();
                        var customerval = GetObjectID('hdnCustomerId').value;
                        $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                        $('#<%=hdnRefreshType.ClientID %>').val('E');
                        $('#<%=hdfIsDelete.ClientID %>').val('I');
                        grid.batchEditApi.EndEdit();
                        cacbpCrpUdf.PerformCallback();
                    }
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('Please add atleast single record first');
                }
            }
            else {
                LoadingPanel.Hide();
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }
        }
    }
}

function callback_InlineRemarks_EndCall(s, e) {

    if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
        $("#txtInlineRemarks").focus();
    }
    else {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
}


function FinalRemarks() {


    ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + grid.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');


}


function closeRemarks(s, e) {

    cPopup_InlineRemarks.Hide();
    //e.cancel = false;
    //ccallback_InlineRemarks.PerformCallback('RemarksDelete'+'~'+grid.GetEditor('SrlNo').GetValue()+'~'+$('#txtInlineRemarks').val());
    //cPopup_InlineRemarks.Hide();
    //e.cancel = false;
    // cPopup_InlineRemarks.Hide();
}



function SaveExit_ButtonClick() {
    $("#hdn_party_inv_no").val(ctxt_partyInvNo.GetText());
    flag = true;
    LoadingPanel.Show();
    cbtn_SaveRecords.SetEnabled(false);
    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();
    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        LoadingPanel.Hide();
        flag = false;
        $("#MandatoryBillNo").show();
        return false;
    }

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
        return false;
    }

    if (($("#hdBillingPin").val() == "0") && ($("#hdShippingPin").val() == "0")) {
        LoadingPanel.Hide();
        flag = false;

        jAlert('Please enter pin number.');
        page.SetActiveTabIndex(1);
        return false;
    }

    var Podt = cPLQuoteDate.GetValue();
    if (Podt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDate").show();
        flag = false;
        return false;
    }
    var invtype = $('#ddlInventory').val();
    if (invtype != 'N') {
        if (ctxt_partyInvNo.GetText() == '' || ctxt_partyInvNo.GetText() == null) {
            flag = false;
            $("#MandatorysPartyinvno").show();
            LoadingPanel.Hide();
            return false;
        }
    }
    var sdate = cdt_partyInvDt.GetValue();
    var edate = cPLQuoteDate.GetValue();
    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (invtype == 'N') {
        if (sdate == null || sdate == "") {
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');

            if (startDate > endDate) {
                LoadingPanel.Hide();
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else {
                $('#MandatoryEgSDate').attr('style', 'display:none');
                flag = true;
            }
        }
    }
    else if (invtype != 'N') {
        if (sdate == null || sdate == "") {
            flag = false;
            $('#MandatoryPartyDate').attr('style', 'display:block');
            LoadingPanel.Hide();
            return false;
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');

            if (startDate > endDate) {
                LoadingPanel.Hide();
                return false;
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else {
                $('#MandatoryEgSDate').attr('style', 'display:none');
                flag = true;
            }
        }
    }
    //var customerval = gridLookup.GetValue();
    var customerval = GetObjectID('hdnCustomerId').value;
    if (customerval == '' || customerval == null) {
        LoadingPanel.Hide();
        flag = false;
        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }

    var hdnAlertRetention = $('#hdnAlertRetention').val();
    var RetenValue = $("#hdnSettingsShowRetention").val();
    //if (hdnAlertRetention == '1') {
    //    if (RetenValue != 'ShowRetention') {


    //        jConfirm('Do you want to Enter Retention?', 'Confirmation Dialog', function (r) {
    //            if (r == true) {

    //                $("#newModal").modal('show');


    //                LoadingPanel.Hide();
    //                flag = false;
    //                return false;

    //            }
    //        });

    //    }
    //}


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



    if (hdnAlertRetention == '1' && RetenValue != 'ShowRetention') {

        jConfirm('Do you want to Enter Retention?', 'Confirmation Dialog', function (r) {
                if (r == true) {

                    $("#newModal").modal('show');
                    $("#hdnSettingsShowRetention").val('ShowRetention');

                    LoadingPanel.Hide();
                    flag = false;
                    return false;

                }
                else {

                    if (flag != false) {



                        if (IsProduct == "Y") {
                            var inventoryItem = $('#ddlInventory').val();
                            if (inventoryItem == 'N' && cddl_TdsScheme.GetValue() != '0') {
                                if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                                    var tdschk = $('#<%=hdntdschecking.ClientID %>').val()
                                    if (tdschk != '') {
                                        LoadingPanel.Hide();
                                        jAlert('Please enter TDS detail for Sl no.' + tdschk)
                                        return false;
                                    }
                                }
                            }
                            if (inventoryItem == 'S' && cddl_TdsScheme.GetValue() != '0') {
                                if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                        var tdschk = $('#<%=hdntdschecking.ClientID %>').val()
                        if (tdschk != '') {
                            LoadingPanel.Hide();
                            jAlert('Please enter TDS detail for Sl no.' + tdschk)
                            return false;
                        }
                    }
                }

                if (grid.GetVisibleRowsOnPage() > 0) {
                    //Subhra 18-03-2019
                    if (issavePacking == 1 && aarr.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "ProjectPurchaseInvoice.aspx/SetSessionPacking",
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var customerval = GetObjectID('hdnCustomerId').value;
                                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                                $('#<%=hdnRefreshType.ClientID %>').val('E');
                                $('#<%=hdfIsDelete.ClientID %>').val('I');
                                grid.batchEditApi.EndEdit();
                                cacbpCrpUdf.PerformCallback();
                            }
                        });
                    }
                    else {
                        //Subhra 18-03-2019
                        //var customerval = gridLookup.GetValue();
                        var customerval = GetObjectID('hdnCustomerId').value;
                        $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                        $('#<%=hdnRefreshType.ClientID %>').val('E');
                        $('#<%=hdfIsDelete.ClientID %>').val('I');
                        grid.batchEditApi.EndEdit();
                        cacbpCrpUdf.PerformCallback();
                    }
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('Please add atleast single record first');
                }
            }
            else {
                LoadingPanel.Hide();
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }
        }

    }



            });


}

else {


    if (flag != false) {



        if (IsProduct == "Y") {
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem == 'N' && cddl_TdsScheme.GetValue() != '0') {
                if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                        var tdschk = $('#<%=hdntdschecking.ClientID %>').val()
                        if (tdschk != '') {
                            LoadingPanel.Hide();
                            jAlert('Please enter TDS detail for Sl no.' + tdschk)
                            return false;
                        }
                    }
                }
                if (inventoryItem == 'S' && cddl_TdsScheme.GetValue() != '0') {
                    if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                        var tdschk = $('#<%=hdntdschecking.ClientID %>').val()
                        if (tdschk != '') {
                            LoadingPanel.Hide();
                            jAlert('Please enter TDS detail for Sl no.' + tdschk)
                            return false;
                        }
                    }
                }

                if (grid.GetVisibleRowsOnPage() > 0) {
                    //Subhra 18-03-2019
                    if (issavePacking == 1 && aarr.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "ProjectPurchaseInvoice.aspx/SetSessionPacking",
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var customerval = GetObjectID('hdnCustomerId').value;
                                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                                $('#<%=hdnRefreshType.ClientID %>').val('E');
                                $('#<%=hdfIsDelete.ClientID %>').val('I');
                                grid.batchEditApi.EndEdit();
                                cacbpCrpUdf.PerformCallback();
                            }
                        });
                    }
                    else {
                        //Subhra 18-03-2019
                        //var customerval = gridLookup.GetValue();
                        var customerval = GetObjectID('hdnCustomerId').value;
                        $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                        $('#<%=hdnRefreshType.ClientID %>').val('E');
                        $('#<%=hdfIsDelete.ClientID %>').val('I');
                        grid.batchEditApi.EndEdit();
                        cacbpCrpUdf.PerformCallback();
                    }
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('Please add atleast single record first');
                }
            }
            else {
                LoadingPanel.Hide();
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }
        }
    }
}

function OnAddNewClick() {
    var inventoryItem = $('#ddlInventory').val();
    if (inventoryItem != 'N' && inventoryItem != 'S') {
        if (gridquotationLookup.GetValue() == null) {
            grid.AddNewRow();

            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
        }
    }
    else {
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
    }
}
function ProductsCombo_SelectedIndexChanged(s, e) {

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbStockUOM = grid.GetEditor("gvColStockUOM");
    var tbPurchasePrice = grid.GetEditor("PurchasePrice");
    var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStockUOM = SpliteDetails[4];
    var strPurchasePrice = SpliteDetails[6];
    var strStockId = SpliteDetails[10];
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbStockUOM.SetValue(strStockUOM);
    tbPurchasePrice.SetValue(strPurchasePrice);
    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
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
            url: "ProjectPurchaseInvoice.aspx/GetRate",
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
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('1');
    }
    else if (key == 2) {
        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');
    }
    else if (key == 3) {
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('3');
    }
}
//function GetIndentReqNoOnLoad() {

//    var PODate = new Date();
//    PODate = cPLQuoteDate.GetValueString();
//    cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate); 
//} 
        <%--function GetContactPerson(e) {
            // Code Added by Sam to Set By Default 1 row in grid during Save&New 
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            if (noofvisiblerows == '0') {
                grid.AddNewRow();
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue('1');
            }



            if (!gridLookup.FindItemByValue(gridLookup.GetValue())) {
                $('#ddl_numberingScheme').prop('disabled', false);
                jAlert("Vendor not Exists.", "Alert", function () { gridLookup.SetValue(); gridLookup.Focus(); }); 
                return;
            }
            var invtype = $('#ddlInventory').val();
            var startDate = new Date();
            startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
            var branchid = $('#ddl_Branch').val(); 
            var key = gridLookup.GetValue()
            GetObjectID('hdnCustomerId').value = key; 
            // For Checking Shipping AddressOfVendor End  
            if (key != $('#<%=hdnTaggedVender.ClientID %>').val()) {
                if (gridquotationLookup.GetValue() != null) { 
                    jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) { 
                        if (r == true) { 
                            var key = gridLookup.GetValue()
                            if (key != null && key != '') {
                                if ($('#<%=hdnTaggedVender.ClientID %>').val() != null && $('#<%=hdnTaggedVender.ClientID %>').val() != '') {
                                    $('#<%=hdnTaggedVender.ClientID %>').val(key);
                                    $('#<%=hdnTaggedVendorName.ClientID %>').val(gridLookup.GetText()); 
                                }
                                gridquotationLookup.SetValue(''); 
                                $('.dxeErrorCellSys').addClass('abc'); 
                                var startDate = new Date();
                                startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd'); 
                                var key = gridLookup.GetValue()
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
                                        LoadCustomerAddress(key, schemabranch, 'PB'); 
                                        //page.tabs[0].SetEnabled(true);
                                        page.tabs[1].SetEnabled(true);
                                    }
                                }
                                else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') { 
                                    var schemabranchid = $('#ddl_Branch').val();
                                    if (schemabranchid != '0') {
                                        var schemabranch = schemabranchid;
                                        // Geet on 15102017 Start
                                        LoadCustomerAddress(key, schemabranch, 'PB');
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
                                gridLookup.SetValue($('#<%=hdnTaggedVender.ClientID %>').val());
                            } 
                        }
                        else {
                            gridLookup.SetValue($('#<%=hdnTaggedVender.ClientID %>').val());
                            var vendorid = $('#<%=hdnTaggedVender.ClientID %>').val();
                            gridLookup.PerformCallback(vendorid) 
                        }
                    });
                }
                else { 
                    var key = gridLookup.GetValue()
                    if (key != null && key != '') { 
                        //###### Added By : Samrat Roy ########## 
                        if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                            var schemabranchid = $('#ddl_numberingScheme').val();
                            if (schemabranchid != '0') {
                                var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                                // geet 15102017 Start
                                LoadCustomerAddress(key, schemabranch, 'PB');
                                // geet 15102017 End 
                                //page.tabs[0].SetEnabled(true);
                                //$('#ddl_numberingScheme').prop('disabled', true);
                                page.tabs[1].SetEnabled(true);
                            }
                        }
                        else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') {
                            var schemabranchid = $('#ddl_Branch').val();
                            if (schemabranchid != '0') {
                                var schemabranch = schemabranchid;
                                // geet 15102017 Start
                                LoadCustomerAddress(key, schemabranch, 'PB');
                                // geet 15102017 End
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
                        $('#ddl_numberingScheme').prop('disabled', false);
                        gridLookup.Focus();
                    }
                } 
            }
        } --%>
        function CmbBranch_ValueChange() {
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").val();
            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
            if (type == 'PO' || type == 'PC') {
                selectValue();
            }
            else {
                ctxtVendorName.SetText('');
                GetObjectID('hdnCustomerId').value = '';
                //var vendorid = gridLookup.GetValue();
                //if(vendorid!=null && vendorid!='')
                //{
                //    gridLookup.PerformCallback('BlankVendor');
                //} 
            }
        }
        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
        }
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
    </script>
    <%--   Warehouse  Script   --%>
    <script type="text/javascript">
        function Keypressevt() {
            if (event.keyCode == 13) {
                //run code for Ctrl+X -- ie, Save & Exit! 
                SaveWarehouse();
                return false;
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
        $(document).ready(function () {

            //var key = GetObjectID('hdnCustomerId').value;
            //if (key != null) {
            //    if ($("#btn_TermsCondition").is(":visible")) {
            //        callTCspecefiFields_PO(key);
            //    }
            //}

            var val = $("#ddl_numberingScheme").val();
            //var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
            if (val == '0') {
                document.getElementById('txtVoucherNo').disabled = true;
            }






            if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                page.GetTabByName('Billing/Shipping').SetEnabled(false);
            }
            var isCtrl = false;
            document.onkeydown = function (e) {
                //if (event.keyCode == 78 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + n -- ie, Save & New  
                if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V" && $("#lbl_quotestatusmsg")[0].innerHTML == "") { //run code for Alt + s -- ie, Save & New  
                    StopDefaultAction(e);
                    Save_ButtonClick();
                }
                else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V" && $("#lbl_quotestatusmsg")[0].innerHTML == "") { //run code for Alt+X -- ie, Save & Exit!  
                    //else if (event.keyCode == 69 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt+e -- ie, Save & Exit!     
                    StopDefaultAction(e);
                    SaveExit_ButtonClick();
                }
                else if (event.keyCode == 79 && event.altKey == true) { //run code for Alt+O -- ie, Billing/Shipping Samrat!     
                    StopDefaultAction(e);
                    if (page.GetActiveTabIndex() == 1) {
                        fnSaveBillingShipping();
                    }
                }
                else if (event.keyCode == 77 && event.altKey == true) { //run code for Alt+m -- ie, TC Sayan!
                    $('#TermsConditionseModal').modal({
                        show: 'true'
                    });
                }
                else if (event.keyCode == 69 && event.altKey == true) { //run code for Alt+e -- ie, TC Sayan!
                    if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
                        StopDefaultAction(e);
                        SaveTermsConditionData();
                    }
                }
                else if (event.keyCode == 76 && event.altKey == true) { //run code for Alt+l -- ie, TC Sayan!
                    StopDefaultAction(e);
                    calcelbuttonclick();
                }
                else if (event.keyCode == 85 && event.altKey == true) {//run code for Alt+U -- ie,
                    OpenUdf();
                }
                else if (event.keyCode == 84 && event.altKey == true) {//run code for Alt+T
                    Save_TaxesClick();
                }
                else if (event.keyCode == 83 && event.altKey == true) { //run code for Alt+X -- ie, Save & Exit!     
                    StopDefaultAction(e);
                    if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                        SaveVehicleControlData();
                    }
                }
                else if (event.keyCode == 67 && event.altKey == true) { //run code for Alt+X -- ie, Save & Exit!     
                    StopDefaultAction(e);
                    modalShowHide(0);
                }
                else if (event.keyCode == 82 && event.altKey == true) { //run code for Alt+X -- ie, Save & Exit!     
                    StopDefaultAction(e);
                    $('body').on('shown.bs.modal', '#exampleModal', function () {
                        $('input:visible:enabled:first', this).focus();
                    })
                }
            }
        });
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function DeleteWarehousebatchserial(SrlNo, BatchWarehouseID, viewQuantity, Quantity, WarehouseID, BatchNo) {
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
                ctxtbatchqnty.SetText(Quantity);
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
                ctxtbatchqnty.SetText(Quantity);
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
            }
        }
        function changedqnty(s) {
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
            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
        }
        function SaveWarehouse() {
            if ($('#wbsqtychecking').val() == '1') {
                var qnty = ctxtqnty.GetText();
                var sum = $('#hdntotalqntyPC').val();
                sum = Number(Number(sum) + Number(qnty));
                $('#<%=hdntotalqntyPC.ClientID %>').val(sum);
                ctxtqnty.SetEnabled(false);
                $('#wbsqtychecking').val('0');
            }
            var prosrlno = $('#hdfProductSerialID').val();
            var WarehouseID = cCmbWarehouse.GetValue();
            var WarehouseName = cCmbWarehouse.GetText();
            var qnty = ctxtqnty.GetText();
            var IsSerial = $('#hdnisserial').val();
            if (qnty == "0.0000") {
                qnty = ctxtbatchqnty.GetText();
            }
            if (Number(qnty) % 1 != 0 && IsSerial == "true") {
                jAlert("Serial number is activated, Quantity should not contain decimals. ");
                return;
            }
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
                    cGrdWarehousePC.PerformCallback('Updatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty + '~' + prosrlno);
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
                    cGrdWarehousePC.PerformCallback('NewUpdatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty + '~' + prosrlno);
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
                        ctxtbatchqnty.Focus();
                    }
                }
                if (qnty == "0.0") {
                    if (Isbatch != "false" || hdniswarehouse != "false") {
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
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
                        cGrdWarehousePC.PerformCallback('Display~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + qnty + '~' + prosrlno);
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
        if (cGrdWarehousePC.cperrorMsg != null && cGrdWarehousePC.cperrorMsg != undefined) {
            jAlert(cGrdWarehousePC.cperrorMsg);
            ctxtserial.Focus();
            return;
        }
        if (cGrdWarehousePC.cpdeletedata != null) {
            var existingqntity = $('#hdfopeningstockPC').val();
            var totaldeleteqnt = $('#hdndeleteqnity').val();
            var addqntity = Number(cGrdWarehousePC.cpdeletedata) + Number(existingqntity);
            var adddeleteqnty = Number(cGrdWarehousePC.cpdeletedata) + Number(totaldeleteqnt);
            $('#<%=hdndeleteqnity.ClientID %>').val(adddeleteqnty);
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
            //New Code Added by Sam
            ctxtqnty.SetEnabled(true)
            $('#wbsqtychecking').val('1')
            //New Code Added by Sam
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
        if (cGrdWarehousePC.cpupdatemssg != null) {
            if (cGrdWarehousePC.cpupdatemssg == "Saved Successfully.") {
                $('#<%=hdntotalqntyPC.ClientID %>').val("0");
                $('#<%=hdnbatchchanged.ClientID %>').val("0");
                $('#<%=hidencountforserial.ClientID %>').val("1");
                ctxtqnty.SetValue("0.0000");
                ctxtbatchqnty.SetValue("0.0000");
                parent.cPopup_WarehousePC.Hide();
                var hdnselectedbranch = $('#hdnselectedbranch').val();
                var selectedrow = $('#hdngridvselectedrowno').val();
                grid.batchEditApi.StartEdit(selectedrow, 8);
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
    function Onddl_VatGstCstEndCallback(s, e) {
        if (s.GetItemCount() == 1) {
            cddlVatGstCst.SetEnabled(false);
        }
    }
    </script>
    <%--   Warehouse Script End    --%>
    <%--Sam Section For extra Modification and tagging Section Start--%>
    <script>
        $(document).ready(function () {
            $('#ApprovalCross').click(function () {

                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.Refresh()();
            })
        })
        function DateCheck() {
            var invtype = $('#ddlInventory').val();
            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
            var endDate = cPLQuoteDate.GetValue();
            var str = $.datepicker.formatDate('yy-mm-dd', endDate);
            var checkval = cchk_reversemechenism.GetChecked();
            //var key = gridLookup.GetValue()
            var key = GetObjectID('hdnCustomerId').value;
            // Waiting for Dirction Start
            <%--if (key != null && key != '') {
                if (str < '2017-10-13') {
                    if($('#<%=hdnRCMChecked.ClientID %>').val()=='1')
                    {
                        cchk_reversemechenism.SetValue(true);
                        cchk_reversemechenism.SetEnabled(false)
                        $('#divreverse').removeClass('hide');
                    } 
                    else
                    {
                        cchk_reversemechenism.SetValue((false));
                        cchk_reversemechenism.SetEnabled(false) 
                        $('#divreverse').addClass('hide');
                    }
                }
                else {
                    cchk_reversemechenism.SetValue((false));
                    cchk_reversemechenism.SetEnabled(false)
                    $('#divreverse').addClass('hide');
                } 
            }
            else
            {
                cchk_reversemechenism.SetValue((false));
                cchk_reversemechenism.SetEnabled(false) 
                $('#divreverse').addClass('hide');
            }--%>
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
                            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                        }
                        grid.PerformCallback('GridBlank');
                        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                            clearTransporter(); disc
                        }
                        ccmbGstCstVat.PerformCallback();
                        ccmbGstCstVatcharge.PerformCallback();
                        // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        deleteTax('DeleteAllTax', "", "");
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
        function selectValue() {
            var startDate = new Date();
            startDate = cPLQuoteDate.GetValueString();
            //var key = gridLookup.GetValue()
            var key = GetObjectID('hdnCustomerId').value;
            if (key != null && key != '') {
                $('#<%=hdnTaggedVender.ClientID %>').val(key);
                <%--$('#<%=hdnTaggedVendorName.ClientID %>').val(gridLookup.GetText());--%>
                $('#<%=hdnTaggedVendorName.ClientID %>').val(ctxtVendorName.GetText());
            }
            else {
                $("table[id$=rdl_PurchaseInvoice] input:radio:checked").removeAttr("checked");
                jAlert('Select a Vendor first');
                return;
            }
            var invtype = $('#ddlInventory').val();
            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
            var schemaid = $('#ddl_numberingScheme').val();
            if (schemaid != '0') {
                page.tabs[1].SetEnabled(false);
                if (type == "PO") {
                    clbl_InvoiceNO.SetText('Purchase Order Date');
                    $("#ddlInventory").attr("disabled", true);
                    if (invtype == 'Y') {
                        grid.GetEditor('ProductName').SetEnabled(false);
                    }
                }
                else if (type == "PC") {

                    clbl_InvoiceNO.SetText('GRN Date');
                    $("#ddlInventory").attr("disabled", true);
                    $("#ddl_numberingScheme").attr("disabled", true);
                    if (invtype == 'Y' || invtype == 'B' || invtype == 'C') {
                        grid.GetEditor('ProductName').SetEnabled(false);
                    }
                }
                else {
                    $("#ddlInventory").attr("disabled", true);
                    $("#ddl_numberingScheme").attr("disabled", false);
                    grid.GetEditor('ProductName').SetEnabled(true);
                }
                if (key != null && key != '' && type != "") {
                    cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                }
                var componentType = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
                if (componentType != null && componentType != '') {
                    var noofvisiblerows = grid.GetVisibleRowsOnPage();
                    if (noofvisiblerows != '1') {

                        grid.PerformCallback('GridBlank');
                    }
                }
            }
        }
    </script>
    <%--Sam Section For extra Modification Section End--%>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section--%>
    <script>
        function SettingTabStatus() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            }
        }

        function disp_prompt(name) {
            if (name == "tab0") {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                page.GetTabByName('General').SetEnabled(false);
                ctxt_partyInvNo.Focus();
                $("#crossdiv").show();
            }
            if (name == "tab1") {

                $("#crossdiv").hide();
                var custID = GetObjectID('hdnCustomerId').value;
                if (custID == null && custID == '') {
                    jAlert('Please select a customer');
                    page.SetActiveTabIndex(0);

                    return;
                }
                else {
                    page.SetActiveTabIndex(1);
                    page.tabs[0].SetEnabled(false);
                }
            }
        }

        $(document).ready(function () {
            //Toggle fullscreen expandEntryGrid
            $("#expandgrid").click(function (e) {
                e.preventDefault();
                var $this = $(this);
                if ($this.children('i').hasClass('fa-expand')) {
                    $this.removeClass('hovered half').addClass('full');
                    $this.attr('title', 'Minimize Grid');
                    $this.children('i').removeClass('fa-expand');
                    $this.children('i').addClass('fa-arrows-alt');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
                    var cntWidth = $(this).parent('.makeFullscreen').width();
                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;
                    grid.SetHeight(browserHeight - 150);
                    grid.SetWidth(cntWidth);
                }
                else if ($this.children('i').hasClass('fa-arrows-alt')) {
                    $this.children('i').removeClass('fa-arrows-alt');
                    $this.removeClass('full').addClass('hovered half');
                    $this.attr('title', 'Maximize Grid');
                    $this.children('i').addClass('fa-expand');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');
                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;
                    grid.SetHeight(300);
                    var cntWidth = $this.parent('.makeFullscreen').width();
                    grid.SetWidth(cntWidth);
                }
            });
        });


        //Hierarchy Start Tanmoy
        function ProjectValueChange(s, e) {
            //debugger;
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'ProjectPurchaseInvoice.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
        }

        function clookup_Project_LostFocus() {
            //grid.batchEditApi.StartEdit(-1, 2);

            var projID = clookup_Project.GetValue();
            if (projID == null || projID == "") {
                $("#ddlHierarchy").val(0);
            }
        }
        //Hierarchy End Tanmoy

    </script>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section End--%>
    <script>
        function SetEntityType(Id) {
            $.ajax({
                type: "POST",
                url: "SalesQuotation.aspx/GetEntityType",
                data: JSON.stringify({ Id: Id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    $("#hdnEntityType").val(r.d);
                }

            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <span class="">
                    <asp:Label ID="lblHeading" runat="server" Text="Add Project Purchase Invoice"></asp:Label>

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
            <div id="ApprovalCross" runat="server" class="crossBtn">
                <a href=""><i class="fa fa-times"></i></a>

            </div>
            <%--<div id="divcross1" runat="server" class="crossBtn" margin-left: 50px;">--%>
            <div id="crossdiv" runat="server" class="crossBtn">
                <a href="ProjectPurchaseInvoiceList.aspx"><i class="fa fa-times"></i></a>
            </div>
            <%--</div>--%>
        </div>

    </div>

    <div class="form_main">


        <div class="row">
            <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                <TabPages>
                    <dxe:TabPage Name="General" Text="General">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <div class="row">
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
                                    <div class="col-md-2 lblmTop8" runat="server" id="divNumberingScheme">
                                        <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
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

                                        <%--Rev 1.0 [ LostFocus="function(s, e) { SetLostFocusonDemand(e)}" added ] --%>
                                        <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLQuoteDate" Width="100%">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents DateChanged="function(s, e) { DateCheck()}" GotFocus="function(s,e){cPLQuoteDate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}" />
                                        </dxe:ASPxDateEdit>
                                        <span id="MandatoryDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="For Unit">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" onchange="CmbBranch_ValueChange()" CssClass="lst-clear">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%" ClientInstanceName="ctxt_Refference">
                                        </dxe:ASPxTextBox>
                                    </div>

                                    <div style="clear: both"></div>
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

                                        <dxe:ASPxCallbackPanel runat="server" ID="vendorPanel" ClientInstanceName="cvendorPanel" OnCallback="vendorPanel_Callback">
                                            <PanelCollection>

                                                <dxe:PanelContent runat="server">
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
                                                </dxe:PanelContent>
                                            </PanelCollection>
                                        </dxe:ASPxCallbackPanel>
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
                                                        <ClientSideEvents LostFocus="partyInvDtMandatorycheck" GotFocus="function(s,e){cdt_partyInvDt.ShowDropDown();}" />
                                                    </dxe:ASPxDateEdit>
                                                    <span id="MandatoryPartyDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                    <span id="MandatoryEgSDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Party Invoice Date can not be greater than Invoice Date"></span>
                                                    <%--                                            <span id="MandatoryDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>--%>
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

                                    <div style="clear: both"></div>

                                    <div class="col-md-2  hide">

                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Cash">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_cashBank" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_cashBank" Width="100%">
                                            <ClientSideEvents GotFocus="function(s,e){cddl_cashBank.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-2 ">
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
                                                <asp:CheckBox ID="chkNILRateTDS" runat="server" Text="NIL rate TDS?" TextAlign="Right"></asp:CheckBox>
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
                                                <dxe:ListEditItem Text="None" Value="R" Selected="true" />
                                                <dxe:ListEditItem Text="Composite" Value="C" />
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
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Place Of Supply[GST]">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <dxe:ASPxComboBox ID="ddlPosGstInvoice" runat="server" ClientInstanceName="cddlPosGstInvoice" Width="100%" ValueField="System.String">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateInvoicePosGst(e)}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div style="clear: both;"></div>
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
                                            <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />

                                        </dxe:ASPxGridLookup>
                                        <dx:LinqServerModeDataSource ID="EntityServerModeDataPInvoice" runat="server" OnSelecting="EntityServerModeDataPInvoice_Selecting"
                                            ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                    </div>
                                    <div class="col-md-4">
                                        <%-- <label id="Label27" runat="server">Hierarchy</label>--%>
                                        <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                        </asp:DropDownList>
                                    </div>

                                    <div style="clear: both;"></div>
                                    <div class="col-md-12 relative">
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
                                                OnCustomColumnDisplayText="grid_CustomColumnDisplayText">

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
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentNumber" Caption="Document No." VisibleIndex="2" ReadOnly="True" Width="7%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="11%" ReadOnly="True">
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
                                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4" ReadOnly="True" Width="15%">
                                                        <CellStyle Wrap="True"></CellStyle>

                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewCommandColumn VisibleIndex="5" Caption="Addl. Desc." Width="6%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc" Image-Url="/assests/images/more.png" Image-ToolTip="Warehouse">
                                                                <Image ToolTip="Warehouse" Url="/assests/images/more.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="6" Width="7%"
                                                        HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityGotFocus" />
                                                            <%-- LostFocus="QuantityTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM(Purc.)" VisibleIndex="7" Width="6%" ReadOnly="true">
                                                        <PropertiesTextEdit>
                                                            <%-- <ClientSideEvents LostFocus="QuantityTextChange" />--%>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewCommandColumn Width="5.5%" VisibleIndex="6" Caption="Stk Details">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="PurchasePrice" Caption="Purc. Price" VisibleIndex="8" Width="7%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <%-- <MaskSettings Mask="&lt;0..999999999g&gt;.&lt;00..99&gt;" AllowMouseWheel="false"/>--%>
                                                            <ClientSideEvents LostFocus="PurchasePriceTextChange" GotFocus="PurPriceGotFocus" />
                                                            <%--LostFocus="QuantityTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="9" Width="4%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountGotChange" />
                                                            <%--LostFocus="DiscountTextChange" GotFocus="DiscountGotChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%-- Discount in Amt Section Start By Sam on 17052017--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Discountamt" Caption="Disc Amt" VisibleIndex="10" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="DiscountAmtTextChange" GotFocus="DiscountAmtGotChange" />
                                                            <%--LostFocus="DiscountAmtTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- Discount in Amt Section Start By Sam on 17052017--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="11" Width="8%" HeaderStyle-HorizontalAlign="Right">
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
                                                    <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="13" Width="6.5%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <%--<MaskSettings Mask="&lt;0..9999999999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="5%" VisibleIndex="12" Caption="Add New">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewTDS" Image-Url="/assests/images/add.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="14" Width="6%" ReadOnly="false">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Left">

                                                            <Style HorizontalAlign="Left">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                     

                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="5%" VisibleIndex="15" Caption="Add New">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Component ID" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentDetailID" Caption="ComponentDetailID" VisibleIndex="20" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="18" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="17" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="19" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" PropertiesTextEdit-ValidationSettings-ErrorImage-IconID="ghg" Caption="hidden Field Id" VisibleIndex="21" ReadOnly="True" Width="0" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="abcd">
                                                        <CellStyle Wrap="True" CssClass="abcd"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- Rev Mantis Issue 24061 --%>
                                                    <dxe:GridViewDataTextColumn FieldName="Balance_Amount" Caption="Balance Amount" VisibleIndex="22" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- End of Rev Mantis Issue 24061 --%>
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
                                    <div class="col-md-12">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                                    <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveNewRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                                    </dxe:ASPxButton>
                                                    <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                                    </dxe:ASPxButton>
                                                    <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                                    </dxe:ASPxButton>
                                                    <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="T&#818;axes" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                                    </dxe:ASPxButton>
                                                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#newModal" onclick="OpenRetention();">Retention</button>

                                                    <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_specialedit" runat="server" AutoPostBack="False" Text="Special Edit" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False" Visible="false">
                                                        <ClientSideEvents Click="function(s, e) {specialedit_ButtonClick();}" />
                                                    </dxe:ASPxButton>
                                                    <span id="divTCS" runat="server">
                                                        <dxe:ASPxButton ID="ASPxButton10" ClientInstanceName="cbtn_TCS" runat="server" AutoPostBack="False" Text="Add TC&#818;S" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {ShowTCS();}" />
                                                        </dxe:ASPxButton>
                                                        <%--Mantis Issue 24274--%>
                                                        <dxe:ASPxButton ID="ASPxButton11" ClientInstanceName="cbtn_TDS" runat="server" AutoPostBack="False" Text="Add TD&#818;S" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {ShowTDS();}" />
                                                        </dxe:ASPxButton>
                                                        <%--End of Mantis Issue 24274--%>
                                                    </span>

                                                    <asp:HiddenField ID="hfControlData" runat="server" />
                                                    <asp:HiddenField ID="hdnPBTaggedYorN" runat="server" />
                                                    <asp:HiddenField ID="hdnTaxDeleteByShippingStateMismatch" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hdnRCMChecked" runat="server" />
                                                    <span id="spVehTC" style="display: inline-block">
                                                        <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />

                                                    </span>
                                                    <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                                    <div id="dvSOTC" runat="server" style="display: inline-block">
                                                        <ucProject:ProjectTermsConditionsControl runat="server" ID="ProjectTermsConditionsControl" />
                                                    </div>


                                                    <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                                                    <span id="spVendorImport">
                                                        <%-- <uc1:ucImportPurchase_BillOfEntry runat="server" ID="ucImportPurchase_BillOfEntry" />--%>
                                                    </span>
                                                    <asp:HiddenField runat="server" ID="hdnProjectSOTC" />
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
            <asp:HiddenField ID="hdnEntityType" runat="server" />
            <asp:HiddenField runat="server" ID="hdnQty" />


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
            SelectCommand="Select * From ((Select '0' as ID,' Select' as SchemaName) Union (Select  convert(nvarchar(10),ID)+'~'+convert(nvarchar(10),b.branch_id) as ID,SchemaName+'('+b.branch_description +')'as SchemaName  From tbl_master_Idschema  join tbl_master_branch b on tbl_master_Idschema.Branch=b.branch_id  Where TYPE_ID='119' and IsActive=1
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
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..00&gt;" AllowMouseWheel="false" />
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
    <%--Rev 1.0--%>
    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
    <asp:HiddenField ID="hdnDatafrom" runat="server" />
    <asp:HiddenField ID="hdnDatato" runat="server" />
    <%--End of Rev 1.0--%>

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

    <div class="modal fade pmsModal w40" id="newModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Retention Details</h4>
                </div>
                <div class="modal-body">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Customer  Name</label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox runat="server" ClientInstanceName="crtxtName" ID="crtxtName" ClientEnabled="false" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Total Amount </label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox runat="server" ClientInstanceName="ctxtTotAmount" ID="ctxtTotAmount" ClientEnabled="false" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Retention Percentage</label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox ID="crtxtRetPercentage" ClientInstanceName="crtxtRetPercentage" runat="server" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    <ClientSideEvents LostFocus="SetAmountandPercentage" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Retention Amt </label>
                            <div class="col-sm-8">
                                <dxe:ASPxTextBox ID="crtxtRetAmount" ClientInstanceName="crtxtRetAmount" runat="server" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    <ClientSideEvents LostFocus="SetAmountandPercentage" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Due Date</label>
                            <div class="col-sm-8">
                                <dxe:ASPxDateEdit ID="dtDueDate" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" runat="server" ClientInstanceName="cdtDueDate" Width="100%">
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>



                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Retention GL</label>
                            <div class="col-sm-8">
                                <dxe:ASPxButtonEdit ID="btnGL" runat="server" ReadOnly="true" ClientInstanceName="cbtnGL" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="MainAccountButnClickRO"
                                        KeyDown="MainAccountNewkeydownRO" />
                                </dxe:ASPxButtonEdit>
                            </div>
                        </div>




                        <div class="form-group row">
                            <label for="" class="col-sm-4  col-form-label">Remarks </label>
                            <div class="col-sm-8">
                                <dxe:ASPxMemo runat="server" ID="crtxtRemarks" ClientInstanceName="crtxtRemarks" Width="100%"></dxe:ASPxMemo>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" onclick="saveRetention();">OK</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="MainAccountModelRO" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydownRO(event)" id="txtMainAccountSearchRO" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTableRO">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Subledger Type</th>
                                <th>Reverse Applicable</th>
                                <th>HSN/SAC</th>
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

    <%--Mantis Issue 24274--%>
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
    <%--End of Mantis Issue 24274--%>

    <asp:SqlDataSource runat="server" ID="tcsDatasource" SelectCommand="select TDSTCS_Code,LTRIM(RTRIM(TDSTCS_Code))+' ('+TDSTCS_Description+')' TCS_SECTION from Master_TDSTCS inner join tbl_master_TDS_Section on Section_Code=TDSTCS_Code where TYPE='TCS'"></asp:SqlDataSource>
    <%--Mantis Issue 24274--%>
    <asp:SqlDataSource runat="server" ID="tdsDatasource" SelectCommand=" select TDSTCS_Code,LTRIM(RTRIM(TDSTCS_Code))+' ('+TDSTCS_Description+')' TDS_SECTION from Master_TDSTCS inner join tbl_master_TDS_Section on Section_Code=TDSTCS_Code where TYPE='TDS' and TDSTCS_ID not in (select DISTINCT TDSTCS_ID from tbl_master_productTdsMap where TDSTCS_ID<>0) and TDSTCS_Code='194Q'"></asp:SqlDataSource>
    <%--End of Mantis Issue 24274--%>


    <asp:HiddenField runat="server" ID="hdnROMainAc" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnAllowDuplicatePartyInvoiceNo" runat="server" />
    <asp:HiddenField ID="hdnBackdateddate" runat="server" />
    <asp:HiddenField ID="hdnTagDateForbackdated" runat="server" />
    <asp:HiddenField ID="hdnSettingsShowRetention" runat="server" />
    <asp:HiddenField ID="hdnAlertRetention" runat="server" />
    <%--Rev Mantis Issue 24061--%>
    <asp:HiddenField runat="server" ID="hdnPurchaseOrderItemNegative" />
    <%--End of Rev Mantis Issue 24061--%>
</asp:Content>
