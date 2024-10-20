﻿<%--========================================================== Revision History ============================================================================================
1.0   Priti     V2.0.36   31-01-2023    0025507:An error message is appearing while modifying Sales rate scheme
2.0   Pallab    V2.0.36   23-03-2023    0025733 : Master pages design modification
3.0   Priti     V2.0.39   09-08-2023    0026685 : Sales Rate scheme is showing wrong entity after modifying the same.
========================================== End Revision History =======================================================================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SalesRateScheme.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesRateScheme" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../Activities/JS/SearchPopup.js"></script>
    <style>
        .w40 .modal-dialog {
            width: 40%;
        }

        .pmsModal.w70 .modal-dialog {
            width: 70%;
        }

        .pmsModal.w60 .modal-dialog {
            width: 60%;
        }

        .pmsModal.w80 .modal-dialog {
            width: 80%;
        }

        .pmsModal .modal-header h4 {
            font-size: 16px;
        }

        .iminentSpan div#CmbState_DDD_PW-1.dxpcDropDown_PlasticBlue {
            left: 15px !important;
        }

        /*Rev 2.0*/

        select {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue, .dxeTextBox_PlasticBlue {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        #ASPxFromDate, #ASPxToDate, #ASPxASondate, #ASPxAsOnDate, #txtDOB, #txtAnniversary, #txtcstVdate, #txtLocalVdate,
        #txtCINVdate, #txtincorporateDate, #txtErpValidFrom, #txtErpValidUpto, #txtESICValidFrom, #txtESICValidUpto, #FormDate, #toDate {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1, #ASPxToDate_B-1, #ASPxASondate_B-1, #ASPxAsOnDate_B-1, #txtDOB_B-1, #txtAnniversary_B-1, #txtcstVdate_B-1,
        #txtLocalVdate_B-1, #txtCINVdate_B-1, #txtincorporateDate_B-1, #txtErpValidFrom_B-1, #txtErpValidUpto_B-1, #txtESICValidFrom_B-1,
        #txtESICValidUpto_B-1, #FormDate_B-1, #toDate_B-1 {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

            #ASPxFromDate_B-1 #ASPxFromDate_B-1Img, #ASPxToDate_B-1 #ASPxToDate_B-1Img, #ASPxASondate_B-1 #ASPxASondate_B-1Img, #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img,
            #txtDOB_B-1 #txtDOB_B-1Img,
            #txtAnniversary_B-1 #txtAnniversary_B-1Img,
            #txtcstVdate_B-1 #txtcstVdate_B-1Img,
            #txtLocalVdate_B-1 #txtLocalVdate_B-1Img, #txtCINVdate_B-1 #txtCINVdate_B-1Img, #txtincorporateDate_B-1 #txtincorporateDate_B-1Img,
            #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img, #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img, #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img,
            #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img, #FormDate_B-1 #FormDate_B-1Img, #toDate_B-1 #toDate_B-1Img {
                display: none;
            }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue {
            background: #1b5ea4 !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current {
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

        #ShowGrid {
            margin-top: 10px;
        }

        .pt-25 {
            padding-top: 25px !important;
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1 {
            left: -182px !important;
        }

        .plhead a > i {
            top: 9px;
        }

        .clsTo {
            display: flex;
            align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"] {
            margin-right: 5px;
        }

        .dxeCalendarDay_PlasticBlue {
            padding: 6px 6px;
        }

        .modal-dialog {
            width: 50%;
        }

        .modal-header {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid, .TableMain100 #ShowGridList, .TableMain100 #ShowGridRet, .TableMain100 #EmployeeGrid, .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays, #cityGrid {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info {
            background-color: #1da8d1 !important;
            background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex {
            display: flex;
            align-items: baseline;
        }

        input + label {
            line-height: 1;
            margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys {
            padding-top: 2px !important;
        }

        .pBackDiv {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }

        .HeaderStyle th {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer {
            padding-top: 15px;
        }

        .pt-2 {
            padding-top: 5px;
        }

        .pt-10 {
            padding-top: 10px;
        }

        .pt-15 {
            padding-top: 15px;
        }

        .pb-10 {
            padding-bottom: 10px;
        }

        .pTop10 {
            padding-top: 20px;
        }

        .custom-padd {
            padding-top: 4px;
            padding-bottom: 10px;
        }

        input + label {
            margin-right: 10px;
        }

        .btn {
            margin-bottom: 0;
        }

        .pl-10 {
            padding-left: 10px;
        }

        .col-md-3 > label, .col-md-3 > span {
            margin-top: 0 !important;
        }

        .devCheck {
            margin-top: 5px;
        }

        .mtc-5 {
            margin-top: 5px;
        }

        .mtc-10 {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select {
            margin-bottom: 0;
        }

        .form-control {
            background-color: transparent;
            border-radius: 4px;
        }

        select.btn-radius {
            padding: 4px 8px 6px 11px !important;
        }

        .mt-30 {
            margin-top: 30px;
        }

        .panel-title h3 {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn {
            right: 30px;
            top: 25px;
        }

        .mb-10 {
            margin-bottom: 10px;
        }

        .btn-cust {
            background-color: #108b47 !important;
            color: #fff;
        }

            .btn-cust:hover {
                background-color: #097439 !important;
                color: #fff;
            }

        .gHesder {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close {
            color: #fff;
            opacity: .5;
            font-weight: 400;
        }

        .mt-27 {
            margin-top: 27px !important;
        }

        .col-md-3 {
            margin-top: 8px;
        }

        #upldBigLogo, #upldSmallLogo {
            width: 100%;
        }

        #DivSetAsDefault {
            margin-top: 25px;
        }

        .dxeBase_PlasticBlue .dxichTextCellSys label {
            color: #fff !important;
        }

        #actv-warh label {
            color: #111 !important;
        }

        .btn.btn-xs {
            font-size: 14px !important;
        }

        .dxeTextBoxSys, .dxeButtonEditSys {
            width: 100%;
        }

        .simple-select::after {
            top: 34px;
            right: 13px;
        }

        /*#ShowFilter
        {
            padding-bottom: 3px !important;
        }*/

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
        /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 2.0*/
    </style>
    <script>
       /* Rev 3.0*/
        function EntityValueChange(s, e) {
            clookup_Entity.gridView.GetSelectedFieldValues("CNT_INTERNALID", GetSelectedFieldValuesCallback);
        }
        function GetSelectedFieldValuesCallback(values) {
            try {
                var _EntityIds = 0;
                for (var i = 0; i < values.length; i++) {
                    if (_EntityIds == 0) {
                        _EntityIds = values[i];
                    }
                    else {
                        _EntityIds = values[i] + ',' + _EntityIds;
                    }
                    $("#hdnCustId").val(_EntityIds);
                }
            } finally {

            }
        }
        function ProductValueChange(s, e) {
            clookup_Product.gridView.GetSelectedFieldValues("SPRODUCTSID", GetSelectedProductFieldValuesCallback);
        } 
        function GetSelectedProductFieldValuesCallback(values) {
            try {
                var _ProductIds = 0;
                for (var i = 0; i < values.length; i++) {
                    if (_ProductIds == 0) {
                        _ProductIds = values[i];
                    }
                    else {
                        _ProductIds = values[i] + ',' + _ProductIds;
                    }
                    $("#hdnProdId").val(_ProductIds);
                }
            } finally {

            }
        }


        /* Rev 3.0 End*/
        //REV 1.0
        $(function () {
            cQuotationComponentPanel.PerformCallback('BINDEntity');
        });
        $(function () {
            //cQuotationComponentPanel.PerformCallback('BINDPRODUCT');
            cProductComponentPanel.PerformCallback('BINDPRODUCT');
        });
        //REV 1.0 END
        /***Short cut key handling***/
        document.onkeydown = function (e) {
            if (event.keyCode == 83 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                document.getElementById('btnSaveRecords').click();
                return false;
            }
            else if (event.keyCode == 65 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddButtonClick();
            }
            else if (event.keyCode == 67 && event.altKey == true) {
                StopDefaultAction(e);
                cancel()
            }
        }
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function clookup_entity_GotFocus() {
            //clookup_Entity.gridView.Refresh();
            clookup_Entity.ShowDropDown();


        }
    </script>
    <script>
        //***Customer***//
        function CustomerButnClick(s, e) {
            $("#CustomerTable").empty();
            var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th style='display:none'>id</th><th>Customer Name</th><th>Unique Id</th><th>Address</th><th>Type</th></tr></table>";
            $("#CustomerTable").html(html);
            setTimeout(function () { $("#txtCustSearch").focus(); }, 500);
            $('#txtCustSearch').val('');
            //shouldCheck = 1;
            //$('#mainActMsg').hide();
            $('#CustModel').modal('show');

        }
        function CustomerKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
                //shouldCheck = 0;
                s.OnButtonClick(0);
            }
        }
        function Customerkeydown(e) {
            var OtherDetails = {}
            if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
                return false;
            }

            OtherDetails.SearchKey = $("#txtCustSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                HeaderCaption.push("Type");
                if ($("#txtCustSearch").val() != '') {
                    callonServer("/OMS/Management/Activities/SalesRateScheme.aspx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }

        }
        function SetCustomer(Id, Name) {
            if (Id) {
                $('#CustModel').modal('hide');
                SetCustNametxt(Id, Name)
            }
        }
        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "customerIndex") {
                        $('#CustModel').modal('hide');
                        SetCustomer(Id, name);
                    }
                    else if (indexName == "ProdIndex") {
                        $('#ProductModel').modal('hide');
                        SetProduct(Id, name);
                    }
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
                    if (indexName == "customerIndex") {
                        $('#txtCustSearch').focus();
                    }
                    else if (indexName == "ProdIndex") {
                        $('#txtProdSearch').focus();
                    }
                }
            }
            else if (e.code == "Escape") {
                if (indexName == "customerIndex") {
                    $('#CustModel').modal('hide');
                    ctxtCustName.Focus();
                }
                else if (indexName == "ProdIndex") {
                    $('#ProductModel').modal('hide');
                    ctxtProductName.Focus();
                }

            }
        }
        function SetCustNametxt(id, name) {

            ctxtCustName.SetText(name);
            document.getElementById('hdnCustId').value = id;

            ctxtProductName.Focus();
            $("#MandatorysCustName").hide();


        }
    </script>
    <script>
        //***Product***//
        function ProductButnClick(s, e) {
            $("#ProductTable").empty();
            var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th style='display:none'>id</th><th>Product Name</th><th>Product Description</th><th>Min Sale Price</th></tr></table>";
            $("#ProductTable").html(html);
            setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
            $('#txtProdSearch').val('');
            //shouldCheck = 1;
            //$('#mainActMsg').hide();
            $('#ProductModel').modal('show');

        }
        function ProductKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
                //shouldCheck = 0;
                s.OnButtonClick(0);
            }
        }
        function prodkeydown(e) {
            var OtherDetails = {}
            if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
                return false;
            }

            OtherDetails.SearchKey = $("#txtProdSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Product Description");
                HeaderCaption.push("Min Sale Price");
                if ($("#txtProdSearch").val() != '') {
                    callonServer("/OMS/Management/Activities/SalesRateScheme.aspx/GetProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }

        }
        function SetProduct(Id, Name) {
            if (Id) {
                $('#ProductModel').modal('hide');
                SetProdNametxt(Id, Name);
            }
        }
        function SetProdNametxt(id, name) {
            ctxtMinSalePrice.SetValue("0.00");
            ctxtDiscount.SetValue("0.00");
            ctxtAmount.SetValue("0.00");
            ctxtProductName.SetText(name);
            var varproductid = id.split("||@||")[0];
            var minsaleprice = id.split("||@||")[1];
            document.getElementById('hdnProdId').value = varproductid;
            ctxtDiscount.Focus();
            //ctxtMinSalePrice.SetText = "90";
            ctxtMinSalePrice.SetValue(minsaleprice);
            $("#MandatorysProductName").hide();
        }
    </script>
    <script>
        /***************Calculation************/
        function AmountCalculate() {
            var minsaleprice = ctxtMinSalePrice.GetValue();
            var percentage = ctxtDiscount.GetValue();
            if (percentage == "0.00") {
                ctxtAmount.SetValue(minsaleprice);
            }
            else {
                var calcPrice = (minsaleprice - (minsaleprice * percentage / 100)).toFixed(2);
                ctxtAmount.SetValue(calcPrice);
            }
        }
        function PercentageCalculate() {
            var Amount = ctxtAmount.GetValue();
            var minsaleprice = ctxtMinSalePrice.GetValue();
            if (minsaleprice == "0.00") {

            }
            else {
                var calcDis = (((minsaleprice - Amount) * 100) / minsaleprice).toFixed(2);
                if (calcDis == "NaN" || calcDis == "") {
                }
                else {
                    ctxtDiscount.SetValue(calcDis);
                }
            }
        }

    </script>
    <script type="text/javascript">
        /****************Save, get, update, Other ************/
        $(document).ready(function () {
            cbtnSaveRecords.Focus();
        });

        var product = [];
        var Entity = [];

        function SaveButtonClick(flag) {
            $("#MandatorysCustName").hide();
            $("#MandatorysProductName").hide();
            $("#MandatorysDiscount").hide();
            $("#MandatorysFromdt").hide();
            $("#MandatorysTodt").hide();
            $("#MandatorysAmount").hide();

            if (ctxtSchemeCode.GetValue() == "" || ctxtSchemeCode.GetValue() == null) {
                jAlert("Please enter scheme code.");
                return false;
            }

            if (ctxtScheme.GetValue() == "" || ctxtScheme.GetValue() == null) {
                jAlert("Please enter scheme name.");
                return false;
            }

            if ($("#ddlType").val() == "0") {
                jAlert("Please select Basis.");
                return false;
            }


            var IsActive = 0;
            if (document.getElementById("chkIsActive").checked == true) {
                IsActive = 1;
            }

            if (document.getElementById("chkAll").checked == false) {
                //if (ctxtCustName.GetText() == "") {
                //    $("#MandatorysCustName").show();
                //    ctxtCustName.Focus();
                //    return false;
                //}
                var EntityLookUp = clookup_Entity.GetText();
                //if (clookup_Entity.gridView.GetSelectedKeysOnPage().length <= 0) {
                if (EntityLookUp == "") {
                    jAlert("Select Entity.");
                    clookup_Entity.gridView.Focus();
                    return false;
                }
                else {
                    /* Rev 3.0*/
                   // $("#hdnCustId").val(clookup_Entity.gridView.GetSelectedKeysOnPage());
                    /* Rev 3.0 End*/
                    // Mantis Issue 25020
                    //Entity = clookup_Entity.gridView.GetSelectedKeysOnPage();
                    /* Rev 3.0*/
                    //Entity = clookup_Entity.GetText().split(",");
                    Entity = $("#hdnCustId").val().split(","); 
                    /* Rev 3.0 End*/
                    // End of Mantis Issue 25020
                }
            }
            else {
                $("#hdnCustId").val(0);
                Entity.push(0);
            }

            if (document.getElementById("chkAllProduct").checked == false) {
                //if (ctxtProductName.GetText() == "") {
                //    $("#MandatorysProductName").show();
                //    ctxtProductName.Focus();
                //    return false;
                //}
                var ProductLookup = clookup_Product.GetText();
                //if (clookup_Entity.gridView.GetSelectedKeysOnPage().length <= 0) {

                //if (clookup_Product.gridView.GetSelectedKeysOnPage().length <= 0) {
                if (ProductLookup == "") {
                    jAlert("Select Product.");
                    clookup_Product.gridView.Focus();
                    return false;
                }
                else {
                    // Mantis Issue 25020
                    //product = clookup_Product.gridView.GetSelectedKeysOnPage();
                   /* Rev 3.0*/
                    //product = clookup_Product.GetText().split(",");
                    product = $("#hdnProdId").val().split(",");
                    // End of Mantis Issue 25020
                    //$("#hdnProdId").val(clookup_Product.gridView.GetSelectedKeysOnPage());
                    /* Rev 3.0 End*/
                }
            }
            else {
                product.push(0);
                $("#hdnProdId").val(0);
            }
            //if (ctxtDiscount.GetValue() == "") {
            //    $("#MandatorysDiscount").show();
            //    ctxtDiscount.Focus();
            //    return false;
            //}
            if ($("#ddlType").val() != "3") {
                if (ctxtAmount.GetValue() == "0.00") {
                    $("#MandatorysAmount").show();
                    ctxtAmount.Focus();
                    return false;
                }
            }


            if (cFormDate.GetValue() == "") {
                $("#MandatorysFromdt").show();
                cFormDate.Focus();
                return false;
            }
            if (cToDate.GetValue() == "") {
                $("#MandatorysTodt").show();
                cToDate.Focus();
                return false;
            }
            if (cbtnSaveRecords.GetText() == "Update") {
                flag = "update";
            }

            var maxrt = 0;
            if ($("#ddlType").val() == 1) {
                maxrt = ctxtAmount.GetValue();
            }
            else {
                maxrt = ctxtMaxSalesRate.GetValue();
            }
            //"CustID": $.trim($("#hdnCustId").val()), "ProductID": $.trim($("#hdnProdId").val())

            $.ajax({
                type: "POST",
                url: "/OMS/Management/Activities/SalesRateScheme.aspx/addSaleRateLock",
                data: JSON.stringify({
                    "SaleRateLockID": $.trim($("#HiddenSaleRateLockID").val()), "CustID": Entity, "ProductID": product, "DiscSalesPrice": ctxtAmount.GetValue(),
                    "MinSalePrice": ctxtAmount.GetValue(), "discount": ctxtDiscount.GetValue(), "fromdt": cFormDate.GetDate(), "todate": cToDate.GetDate(),
                    "action": flag, "FixRate": ctxtFixRate.GetValue(), "SCHEME": ctxtScheme.GetValue(), "SchemeCode": ctxtSchemeCode.GetValue(), "IsActive": IsActive,
                    "RateType": $("#ddlType").val(), "MaxSalePrice": maxrt,
                    "Method": $("#ddlBasis").val(), "CalculatedOn": $("#ddlCalculatedOn").val(), "CalculationBasis": $("#ddlCalculationBasis").val(), "Margin": ctxtPercentage.GetValue()
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                global: false,
                async: false,
                success: function (msg) {
                    if (msg.d) {
                        if (flag == "Insert") {
                            if (msg.d == "-12") {
                                jAlert("From date cannot be greater than to date");
                                $("#MandatorysFromdt").show();
                                return false;
                            } else if (msg.d == "-11") {
                                jAlert(" Scheme already exists.");
                                return false;
                            } else if (msg.d == "-33") {
                                jAlert(" Scheme already exists.");
                                return false;
                            } else if (msg.d == "-13") {
                                jAlert("From date and to date is same");
                                return false;
                            }
                            else {
                                jAlert("Added Successfully");
                                //$("#entry").hide();
                                //$("#view").show();
                                $("#lblheading").html("Sales Rate Scheme");
                                $("#divAddButton").show();
                                $("#divcross").hide();
                                clear();
                                cgridProductRate.Refresh();
                                return false;
                            }
                        }
                        if (flag == "update") {
                            if (msg.d == "-12") {
                                jAlert("From date cannot be greater than to date");
                                $("#MandatorysFromdt").show();
                                return false;
                            }
                            if (msg.d == "-11") {
                                jAlert("Product already is in sale");
                                return false;
                            }
                            else {
                                //$("#entry").hide();
                                //$("#view").show();
                                $("#lblheading").html("Sales Rate Scheme");
                                $("#divAddButton").show();
                                $("#divcross").hide();
                                cgridProductRate.Refresh();
                                $("#txtCustName_B0").show();
                                clear();
                                return false;
                            }
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }

        function SaleRateCustomButtonClick(s, e) {
            var id = s.GetRowKey(e.visibleIndex);
            if (e.buttonID == 'CustomBtnEdit') {
                if (id != "") {
                    document.getElementById('HiddenSaleRateLockID').value = id;
                    $.ajax({
                        type: "POST",
                        url: "/OMS/Management/Activities/SalesRateScheme.aspx/GetSaleRateLock",
                        data: JSON.stringify({ "SaleRateLockID": id }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        global: false,
                        async: false,
                        success: OnSuccess
                    });
                    //Rev 3.0
                    $.ajax({
                        type: "POST",
                        url: "/OMS/Management/Activities/SalesRateScheme.aspx/GetSaleRateLockProductList",
                        data: JSON.stringify({ "SaleRateLockID": id }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        global: false,
                        async: false,
                        success: OnSuccessProduct
                    });


                    $.ajax({
                        type: "POST",
                        url: "/OMS/Management/Activities/SalesRateScheme.aspx/GetSaleRateLockCustomersList",
                        data: JSON.stringify({ "SaleRateLockID": id }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        global: false,
                        async: false,
                        success: OnSuccessCustomers
                    });
                    //Rev 3.0 End
                }
            }
            if (e.buttonID == 'CustomBtnDelete') {
                if (id != "") {
                    if (confirm("Are you sure to delete")) {
                        $.ajax({
                            type: "POST",
                            url: "/OMS/Management/Activities/SalesRateScheme.aspx/DeleteSaleRateLock",
                            data: JSON.stringify({ "SaleRateLockID": id }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            global: false,
                            async: false,
                            success: function (msg) {
                                if (msg.d) {
                                    if (msg.d == "-998") {
                                        jAlert("Already is in used.Unable to delete");
                                        return false;
                                    }
                                    if (msg.d == "-999") {
                                        jAlert("Deleted Successfuly");
                                        cgridProductRate.Refresh();
                                        return false;
                                    }
                                }
                            }
                        });
                    }
                }

            }
        }

        function OnSuccess(data) {
            //alert(data);
            for (var i = 0; i < data.d.length; i++) {
                if (data.d[i].IsInUse == "1") {
                    jAlert("Already is in used.Unable to delete");
                    return false;
                }

                if (data.d[i].RateType == 1) {
                    $("#DivMaxSalesRate").addClass('hide');
                }
                else {
                    $("#DivMaxSalesRate").removeClass('hide');
                }

                if (data.d[i].CustomerID == "0") {
                    clookup_Entity.SetEnabled(false);
                    document.getElementById("chkAll").checked = true;
                }
                else {
                    //for (var j = 0; j < data.d[i].CustomersList.length; j++) {

                    //    clookup_Entity.SetValue(data.d[i].CustomersList[j].CustomerID);
                    //}

                    cQuotationComponentPanel.PerformCallback('Customers');

                    document.getElementById("chkAll").checked = false;
                }

                if (data.d[i].ProductID == "0") {
                    clookup_Product.SetEnabled(false);
                   
                    document.getElementById("chkAllProduct").checked = true;
                }
                else {
                    //for (var j = 0; j < data.d[i].ProductsList.length; j++) {

                    //    clookup_Product.SetValue(data.d[i].ProductsList[j].ProductID);
                    //}
                    //cgridProductRate.PerformCallback('Products');
                    cProductComponentPanel.PerformCallback('Customers');
                    document.getElementById("chkAllProduct").checked = false;
                }

                // ctxtCustName.SetText(data.d[i].CustomerName);
                document.getElementById('hdnCustId').value = data.d[i].CustomerID;
                //ctxtProductName.SetText(data.d[i].Products_Name);
                document.getElementById('hdnProdId').value = data.d[i].ProductID;
                ctxtMinSalePrice.SetValue(data.d[i].MinSalePrice);
                ctxtDiscount.SetValue(data.d[i].Disc);
                ctxtAmount.SetValue(data.d[i].DiscSalesPrice);
                ctxtFixRate.SetValue(data.d[i].FixRate);
                var frmdt = new Date(data.d[i].ValidFrom);
                cFormDate.SetDate(frmdt);
                var todt = new Date(data.d[i].ValidUpto);
                cToDate.SetDate(todt);
                // ctxtCustName.Focus();
                //ctxtCustName.SetButtonVisible(0);
                ctxtScheme.SetText(data.d[i].Scheme);
                $("#txtCustName_B0").hide();

                ctxtSchemeCode.SetValue(data.d[i].SchemeCode);
                $("#ddlType").val(data.d[i].RateType);
                ctxtMaxSalesRate.SetValue(data.d[i].MaxSalePrice);
                if (data.d[i].IsActive == "True") {
                    document.getElementById("chkIsActive").checked = true;
                }
                else {
                    document.getElementById("chkIsActive").checked = false;
                }

                if (data.d[i].RateType == "3") {
                    $("#DivMaxSalesRate").addClass('hide');
                    $("#DivSalesRate").addClass('hide');
                    $("#DivCalculatedOn").removeClass('hide');
                }

                $("#ddlBasis").val(data.d[i].Method);

                if (data.d[i].Method == "1") {
                    $("#DivNumericValue").removeClass('hide');
                    $("#DivCalculatedBasis").removeClass('hide');
                }
                else {
                    $("#DivNumericValue").addClass('hide');
                    $("#DivCalculatedBasis").addClass('hide');
                    $("#ddlCalculationBasis").val(0);
                    ctxtPercentage.SetValue();
                }

                ctxtPercentage.SetValue(data.d[i].MarginPercentage);

                $("#ddlCalculatedOn").val(data.d[i].CalculatedOn);
                $("#ddlCalculationBasis").val(data.d[i].CalculationBasis);
            }
            //$("#entry").show();
            //$("#view").hide();
            $("#divAddButton").hide();
            //$("#divcross").show();
            $("#lblheading").html("Modify Sales Rate Scheme");
            cbtnSaveRecords.SetText("Update");
        }
       /* Rev 3.0*/
        function OnSuccessCustomers(data) {           
            for (var i = 0; i < data.d.length; i++) {   

                if (document.getElementById('hdnCustId').value == "")
                { 
                    document.getElementById('hdnCustId').value = data.d[i].CustomerID;
                }                
                else {
                    document.getElementById('hdnCustId').value = document.getElementById('hdnCustId').value+','+data.d[i].CustomerID;
                }                       
            }           
        }

        function OnSuccessProduct(data) {           
            for (var i = 0; i < data.d.length; i++) {
                if (document.getElementById('hdnProdId').value == "")
                { 
                    document.getElementById('hdnProdId').value = data.d[i].ProductID;
                }
                else {
                    document.getElementById('hdnProdId').value = document.getElementById('hdnProdId').value + ',' + data.d[i].ProductID;
                }
            }            
        }
        /* Rev 3.0 End*/
        function OnAddButtonClick() {
            $("#divAddButton").hide();
            $("#entry").show();
            $("#view").hide();
            $("#lblheading").html("Add Sales Rate Scheme");
            $("#divcross").show();
            //  ctxtCustName.Focus();
            //ctxtCustName.SetButtonVisible(1);
            $("#txtCustName_B0").show();
            clear();
        }

        function cancel() {
            clear();
            //$("#entry").hide();
            //$("#view").show();
            $("#lblheading").html("Sales Rate Scheme");
            $("#divAddButton").show();
            cbtnSaveRecords.Focus()
            $("#divcross").hide();
            $("#txtCustName_B0").show();
        }

        function clear() {
            //ctxtCustName.SetText("");
            document.getElementById('hdnCustId').value = "";
            // ctxtProductName.SetText("");
            document.getElementById('hdnProdId').value = "";
            ctxtMinSalePrice.SetValue("0.00");
            ctxtDiscount.SetValue("0.00");
            ctxtAmount.SetValue("0.00");
            ctxtFixRate.SetValue("0.00");
            var frmdt = new Date($.trim($("#Hiddenvalidfrom").val()));
            cFormDate.SetDate(frmdt);
            var todt = new Date($.trim($("#Hiddenvalidupto").val()));
            cToDate.SetDate(todt);
            cbtnSaveRecords.SetText("S&#818;ave");
            $("#MandatorysCustName").hide();
            $("#MandatorysProductName").hide();
            $("#MandatorysDiscount").hide();
            $("#MandatorysFromdt").hide();
            $("#MandatorysTodt").hide();
            $("#MandatorysAmount").hide();
            clookup_Entity.SetEnabled(true);
            clookup_Product.SetEnabled(true);
            document.getElementById("chkAll").checked = false;
            document.getElementById("chkAllProduct").checked = false;
            ctxtScheme.SetText("");
            clookup_Entity.gridView.UnselectRows();
            clookup_Product.gridView.UnselectRows()
            ctxtSchemeCode.SetValue("");
            ctxtScheme.SetValue("");
            $("#ddlType").val(0);
            ctxtMaxSalesRate.SetValue("0.00");
            ctxtAmount.SetValue("0.00");
            document.getElementById("chkIsActive").checked = false;
        }

        $(document).ready(function () {
            console.log('ready');
            cgridProductRate.Refresh();
            $('.navbar-minimalize').click(function () {
                console.log('clicked');
                // cGridSaleRate.Refresh();
                cgridProductRate.Refresh();
            });
        });

        function gridRowclick(s, e) {
            $('#GridSaleRate').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

        function AllCustCheck() {
            if (document.getElementById("chkAll").checked == true) {
                clookup_Entity.SetEnabled(false);
            }
            else {
                clookup_Entity.SetEnabled(true);
            }
        }

        function AllProdCheck() {
            if (document.getElementById("chkAllProduct").checked == true) {
                clookup_Product.SetEnabled(false);
            }
            else {
                clookup_Product.SetEnabled(true);
            }
        }


        function ImportUpdatePopOpenProductStock() {
            $("#myImportModal").modal('show');
        }

        function getDownloadTemplateSettings() {
            $("#myModal").modal('show');

        }

        function getTemplateByStaten() {
            var StateId = cCmbState.GetValue();

            if (StateId != "") {


                var StateName = cCmbState.GetText();

                // alert($("#ddlYear").val());
                $.ajax({
                    type: "POST",
                    url: "/OMS/Management/Activities/SalesRateScheme.aspx/DeleteSaleRateLock",
                    data: JSON.stringify({ "MonthID": StateId, "MonthName": StateName }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    global: false,
                    async: false,
                    success: function (msg) {
                        if (msg.d) {
                            if (msg.d == "-998") {
                                jAlert("Already is in used.Unable to delete");
                                return false;
                            }
                            if (msg.d == "-999") {
                                jAlert("Deleted Successfuly");
                                cgridProductRate.Refresh();
                                return false;
                            }
                        }
                    }
                });

            }
            else {
                jAlert("Please select State");

            }
        }

        function ChekprodUpload() {
            var filename = $("#fileprod").val();
            if ($('#fileprod').get(0).files.length === 0) {
                jAlert("No files selected.");
                return false;
            }
            else {
                var extension = filename.replace(/^.*\./, '');
                switch (extension) {
                    case 'xls':
                    case 'xlsx':
                        return true;
                    default:
                        // Cancel the form submission
                        jAlert('Only excel file require.');
                        return false;
                }
            }
        }

        function CloseGridQuotationLookup() {
            //$("#hdnCustId").val(clookup_Entity.gridView.GetSelectedKeysOnPage());
        }

        function DownLoadFormat() {
            //debugger;
            window.location.href = "/Ajax/getExportProductRate?State=" + cCmbState.GetValue();
            //$.ajax({
            //    type: "post",
            //    url: "/OMS/Management/Activities/CustSaleRateLock.aspx/download",
            //    data: JSON.stringify({ "State": cCmbState.GetValue() }),
            //    dataType: "json",
            //    contentType: "application/json; charset=utf-8",
            //    global: false,
            //    async: false,
            //    success: function (msg) {
            //        if (msg.d) {
            //            //var url = "@Url.Action("getExportPJP", "PJPDetailsList")";
            //            //window.location.href = url;
            //        }
            //    },
            //    error:function (er) {

            //    }
            //});
        }

    </script>

    <script>
        function OpenProductRateLogModal() {
            cgridproducts.PerformCallback('');
            $("#RateImportLogModal").modal('show');
        }

        function PopupproductHide() {

        }

        function ddlType_Change() {
            $("#DivCalculatedOn").addClass('hide');
            $("#DivCalculatedBasis").addClass('hide');

            if ($("#ddlType").val() == "1") {
                $("#DivMaxSalesRate").addClass('hide');
            }
            else if ($("#ddlType").val() == "3") {
                $("#DivMaxSalesRate").addClass('hide');
                $("#DivSalesRate").addClass('hide');

                $("#DivCalculatedOn").removeClass('hide');
                // $("#DivCalculatedBasis").removeClass('hide');

            }
            else {
                $("#DivMaxSalesRate").removeClass('hide');
            }
        }

        function ddlBasis_Change() {
            if ($("#ddlBasis").val() == "1") {
                $("#DivNumericValue").removeClass('hide');
                $("#DivCalculatedBasis").removeClass('hide');
            }
            else {
                $("#DivNumericValue").addClass('hide');
                $("#DivCalculatedBasis").addClass('hide');
                $("#ddlCalculationBasis").val(0);
                ctxtPercentage.SetValue();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 2.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
            <div class="panel-title clearfix">
                <h3 class="pull-left">Sales Rate Scheme
                <%--<label id="lblheading">Sales Rate Scheme</label>--%>
                </h3>
            </div>
            <div id="div1" runat="server" class="crossBtn" style="display: none; margin-left: 50px;"><a href="#" onclick="cancel()"><i class="fa fa-times"></i></a></div>
        </div>
        <div class="form_main">
            <div id="TblSearch" class="rgth  full hide">
                <div class="clearfix">
                    <div style="padding-right: 5px;">
                        <span id="divAddButton">
                            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span><u>A</u>dd New</span> </a>
                        </span>
                    </div>

                </div>
            </div>
            <div class="clear"></div>
            <div id="entry">
                <div class="col-md-2 lblmTop8 mb-10">
                    <label>Scheme Code <span class="red">*</span></label>
                    <dxe:ASPxTextBox ID="txtSchemeCode" ClientInstanceName="ctxtSchemeCode" runat="server" TabIndex="1" MaxLength="50">
                    </dxe:ASPxTextBox>
                    <span id="MandatorysSchemeCode" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>
                <div class="col-md-2 lblmTop8 mb-10">
                    <label>Scheme Name <span class="red">*</span></label>
                    <dxe:ASPxTextBox ID="txtScheme" ClientInstanceName="ctxtScheme" runat="server" TabIndex="2" MaxLength="250">
                    </dxe:ASPxTextBox>
                    <span id="MandatorysScheme" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>
                <div class="col-md-2 lblmTop8 mb-10 simple-select">
                    <label>Basis <span class="red">*</span></label>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" TabIndex="3" OnChange="ddlType_Change();">
                        <asp:ListItem Value="0">Select</asp:ListItem>
                        <asp:ListItem Value="1">On Fixed Rate</asp:ListItem>
                        <asp:ListItem Value="2">On Min/Max Rate</asp:ListItem>
                        <asp:ListItem Value="3">Dynamic Rate</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <%--Rev 2.0: "simple-select" class add --%>
                <div class="col-md-2 lblmTop8 simple-select">
                    <label>Method </label>
                    <asp:DropDownList ID="ddlBasis" runat="server" CssClass="form-control" TabIndex="3" OnChange="ddlBasis_Change();">
                        <asp:ListItem Value="0">Select</asp:ListItem>
                        <asp:ListItem Value="1">On MRP</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-2 lblmTop8">
                    <label>From Date <span class="red">*</span></label>
                    <dxe:ASPxDateEdit ID="Fromdt" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy HH:mm" ClientInstanceName="cFormDate"
                        Width="100%" TabIndex="4" DisplayFormatString="dd-MM-yyyy HH:mm" UseMaskBehavior="True">
                        <TimeSectionProperties Visible="true"></TimeSectionProperties>
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <ClientSideEvents GotFocus="function(s,e){cFormDate.ShowDropDown();}" />
                    </dxe:ASPxDateEdit>
                    <span id="MandatorysFromdt" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label>To Date <span class="red">*</span></label>
                    <dxe:ASPxDateEdit ID="ToDate" runat="server" EditFormat="DateTime" EditFormatString="dd-MM-yyyy HH:mm" ClientInstanceName="cToDate"
                        Width="100%" TabIndex="5" DisplayFormatString="dd-MM-yyyy HH:mm" UseMaskBehavior="True">
                        <TimeSectionProperties Visible="true"></TimeSectionProperties>
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <ClientSideEvents GotFocus="function(s,e){cToDate.ShowDropDown();}" />
                    </dxe:ASPxDateEdit>
                    <span id="MandatorysTodt" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>
                <div class="clear"></div>
                <%-- <div style="background: #f5f4f3; padding: 17px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                 DataSourceID="EntityServerModeData" DataSourceID="EntityServerModeDataProduct"--%>
                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotationPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <div class="col-md-2">
                                <label>Entity  <span class="red">*</span></label>
                                &nbsp;
                    <input type="checkbox" id="chkAll" onchange="AllCustCheck()" />
                                All
                    <dxe:ASPxGridLookup ID="lookup_Entity" runat="server" ClientInstanceName="clookup_Entity"
                        OnDataBinding="lookup_Entity_DataBinding"
                        SelectionMode="Multiple"
                        KeyFieldName="CNT_INTERNALID" Width="100%" CheckBoxRowSelect="true" TextFormatString="{0}"
                        AutoGenerateColumns="False" MultiTextSeparator=", ">
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                            <dxe:GridViewDataColumn FieldName="NAME" Visible="true" VisibleIndex="1" Caption="Entity Name" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="UNIQUENAME" Visible="true" VisibleIndex="2" Caption="Unique Id" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="BILLING" Visible="true" VisibleIndex="3" Caption="Address" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="TYPE" Visible="true" VisibleIndex="3" Caption="Type" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                        </Columns>
                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">
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

                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="false"></SettingsBehavior>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                        </GridViewProperties>
                        <ClientSideEvents GotFocus="function(s,e){clookup_Entity.ShowDropDown();}" />
                        <%-- <ClientSideEvents GotFocus="clookup_entity_GotFocus" />--%>
                        <%--LostFocus="Project_LostFocus" ValueChanged="ProjectValueChange"--%>
                        <ClientSideEvents ValueChanged="EntityValueChange" />
                        <ClearButton DisplayMode="Always">
                        </ClearButton>
                    </dxe:ASPxGridLookup>
                                <%--  REV 1.0--%>
                                <%--<dx:LinqServerModeDataSource ID="EntityServerModeData" runat="server" OnSelecting="EntityServerModeData_Selecting"
                                ContextTypeName="ERPDataClassesDataContext" TableName="v_SaleRateLock_customerDetails" />--%>
                                <%--  REV 1.0 END--%>
                            </div>
                        </dxe:PanelContent>
                    </PanelCollection>
                    <%--<ClientSideEvents EndCallback="componentEndCallBack" BeginCallback="BeginComponentCallback" />--%>
                </dxe:ASPxCallbackPanel>
                <dxe:ASPxCallbackPanel runat="server" ID="ProductComponentPanel" ClientInstanceName="cProductComponentPanel" OnCallback="ProductComponentPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <div class="col-md-2">
                                <label>Product <span class="red">*</span></label>
                                &nbsp;
                    <input type="checkbox" id="chkAllProduct" onchange="AllProdCheck()" />
                                All
                  
                    <dxe:ASPxGridLookup ID="lookup_Product" runat="server" ClientInstanceName="clookup_Product" SelectionMode="Multiple" OnDataBinding="lookup_Product_DataBinding"
                        KeyFieldName="SPRODUCTSID" Width="100%" CheckBoxRowSelect="true" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                            <dxe:GridViewDataColumn FieldName="PRODUCTS_NAME" Visible="true" VisibleIndex="1" Caption="Product Name" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="PRODUCTS_DESCRIPTION" Visible="true" VisibleIndex="2" Caption="Product Description" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="SPRODUCT_MINSALEPRICE" Visible="true" VisibleIndex="3" Caption="Min Sale Price" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                        </Columns>
                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                            <Templates>
                                <StatusBar>
                                    <table class="OptionsTable" style="float: right">
                                        <tr>
                                            <td>
                                                <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" />--%>
                                                <%--ClientSideEvents-Click="CloseGridQuotationLookup"--%>
                                            </td>
                                        </tr>
                                    </table>
                                </StatusBar>
                            </Templates>

                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="false"></SettingsBehavior>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                        </GridViewProperties>
                        <ClientSideEvents GotFocus="function(s,e){clookup_Product.ShowDropDown();}" ValueChanged="ProductValueChange" />
                        <%--LostFocus="Project_LostFocus" ValueChanged="ProjectValueChange"--%>

                        <ClearButton DisplayMode="Always">
                        </ClearButton>
                    </dxe:ASPxGridLookup>
                                <%--  REV 1.0--%>
                                <%--  <dx:LinqServerModeDataSource ID="EntityServerModeDataProduct" runat="server" OnSelecting="EntityServerModeDataProduct_Selecting"
                                ContextTypeName="ERPDataClassesDataContext" TableName="v_Product_SaleRateLock" />--%>
                                <%--  REV 1.0 END--%>
                            </div>
                        </dxe:PanelContent>
                    </PanelCollection> 
                     <ClientSideEvents EndCallback="ProductComponentEndCallBack"  />
                </dxe:ASPxCallbackPanel>

                <div class="col-md-2" id="DivSalesRate">
                    <label>Rate</label>
                    <dxe:ASPxTextBox ID="txtAmount" ClientInstanceName="ctxtAmount" runat="server" TabIndex="8">
                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                        <ClientSideEvents LostFocus="PercentageCalculate" />
                    </dxe:ASPxTextBox>
                    <span id="MandatorysAmount" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>

                <div class="col-md-2" id="DivMaxSalesRate">
                    <label>Max Rate</label>
                    <dxe:ASPxTextBox ID="txtMaxSalesRate" ClientInstanceName="ctxtMaxSalesRate" runat="server" TabIndex="9">
                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                        <ClientSideEvents LostFocus="PercentageCalculate" />
                    </dxe:ASPxTextBox>
                    <span id="MandatorysMaxSalesRate" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;" title="Mandatory"></span>
                </div>
                <div class="col-md-2 hide" id="DivCalculatedOn">
                    <label>Calculated On </label>
                    <asp:DropDownList ID="ddlCalculatedOn" runat="server" CssClass="form-control" TabIndex="10">
                        <asp:ListItem Value="0">Select</asp:ListItem>
                        <asp:ListItem Value="1">Fixed Rate (All existing scheme)</asp:ListItem>
                        <asp:ListItem Value="2">Min/Max Rate (All existing scheme)</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2 hide" id="DivCalculatedBasis">
                    <label>Calculation Basis </label>
                    <asp:DropDownList ID="ddlCalculationBasis" runat="server" CssClass="form-control" TabIndex="11">
                        <asp:ListItem Value="0">Select</asp:ListItem>
                        <asp:ListItem Value="1">Markup % (+)</asp:ListItem>
                        <asp:ListItem Value="2">Markup % (-)</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2 hide" id="DivNumericValue">
                    <label>Margin %</label>
                    <dxe:ASPxTextBox ID="txtPercentage" ClientInstanceName="ctxtPercentage" runat="server">
                        <MaskSettings Mask="&lt;0..99&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </div>
                <div class="col-md-2">
                    <label>&nbsp;</label>
                    <div>
                        <input type="checkbox" id="chkIsActive" />
                        Is Active
                    </div>
                </div>
                <div class="col-md-2 hide">
                    <label>Min Sale Price</label>
                    <dxe:ASPxTextBox ID="txtMinSalePrice" ClientInstanceName="ctxtMinSalePrice" runat="server" ReadOnly="true">
                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </div>
                <div class="col-md-2 hide">
                    <label>Discount (%)</label>
                    <dxe:ASPxSpinEdit ID="txtDiscount" ClientInstanceName="ctxtDiscount" runat="server" ShowOutOfRangeWarning="false" SpinButtons-ClientVisible="false"
                        TabIndex="3" MaxValue="100" AllowMouseWheel="false" EditFormatString="0.00" DisplayFormatString="0.00" DecimalPlaces="2" NumberType="Float" MinValue="0.00">

                        <%-- <MaskSettings Mask="&lt;0..999&gt;.&lt;00..99&gt;" AllowMouseWheel="false"  />--%>
                        <ValidationSettings Display="None"></ValidationSettings>
                        <ClientSideEvents LostFocus="AmountCalculate" />
                    </dxe:ASPxSpinEdit>
                    <span id="MandatorysDiscount" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -13px; top: 24px;"
                        title="Mandatory"></span>
                </div>

                <div class="col-md-2 hide">
                    <label>Fix Rate</label>
                    <dxe:ASPxTextBox ID="txtFixRate" ClientInstanceName="ctxtFixRate" runat="server" TabIndex="5">
                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                        <%--<ClientSideEvents LostFocus="PercentageCalculate" />--%>
                    </dxe:ASPxTextBox>
                    <span id="MandatorysFixRate" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>
                <div class="clearfix"></div>

                <%--</div>--%>
                <%--<div class="clearfix"></div>--%>
                <div style="padding: 15px 10px 10px 0px;">
                    <dxe:ASPxButton ID="btnSaveRecords" TabIndex="10" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave" CssClass="btn btn-success" UseSubmitBehavior="False">
                        <ClientSideEvents Click="function(s, e) {SaveButtonClick('Insert');}" />
                    </dxe:ASPxButton>
                    <dxe:ASPxButton ID="btncancel" TabIndex="8" ClientInstanceName="cbtncancel" runat="server" AutoPostBack="False" Text="C&#818;ancel" CssClass="btn btn-primary" UseSubmitBehavior="False">
                        <ClientSideEvents Click="function(s, e) {cancel();}" />
                    </dxe:ASPxButton>

                </div>
            </div>

            <div id="view">

                <dxe:ASPxGridView ID="gridProductRate" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                    ClientInstanceName="cgridProductRate" KeyFieldName="SaleRateLockID" Width="100%" Settings-HorizontalScrollBarMode="Auto"
                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false"
                    SettingsDataSecurity-AllowDelete="false" OnCustomCallback="gridProductRate_CustomCallback" DataSourceID="ProductRateServerModeDataSource">

                    <%----%>
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <ClientSideEvents CustomButtonClick="SaleRateCustomButtonClick" RowClick="gridRowclick" />
                    <SettingsBehavior ConfirmDelete="True" />
                    <Columns>

                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SchemeCode" Caption="Scheme Code" Width="200px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="SchemeName" Caption="Scheme Name" Width="200px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="RateType" Caption="Type" Width="150px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ValidFrom" Caption="From Date" Width="150px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ValidUpto" Caption="To Date" Width="150px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Entity" Caption="Entity" Width="200px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="sProducts_Description" Caption="Product" Width="200px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="DiscSalesPrice" Caption="Sales Rate" Width="100px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="ActiveStatus" Caption="Is Active" Width="100px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="STATUS" Caption="Status" Width="100px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewCommandColumn VisibleIndex="10" Width="130px" ButtonType="Image" Caption="Actions" HeaderStyle-HorizontalAlign="Center">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" meta:resourcekey="GridViewCommandColumnCustomButtonResource1" Image-ToolTip="Edit" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Edit.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" meta:resourcekey="GridViewCommandColumnCustomButtonResource1" Image-ToolTip="Delete" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Delete.png" ToolTip="Delete"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>

                    </Columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>

                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" ShowFooter="true" />
                    <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                </dxe:ASPxGridView>
                <dx:LinqServerModeDataSource ID="ProductRateServerModeDataSource" runat="server" OnSelecting="ProductRateServerModeDataSource_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="V_SalesRateScheme" />
            </div>

        </div>
    </div>


    <asp:HiddenField ID="hdnCustId" runat="server" />
    <asp:HiddenField ID="hdnProdId" runat="server" />
    <asp:HiddenField ID="HiddenSaleRateLockID" runat="server" />
    <asp:HiddenField ID="Hiddenvalidfrom" runat="server" />
    <asp:HiddenField ID="Hiddenvalidupto" runat="server" />

    <div id="RateImportLogModal" class="modal fade pmsModal w70" data-backdrop="static" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Rate Import Log</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <div class="">
                            <div class="row">
                                <div>
                                    <div class="clearfix ">
                                        <div class="col-lg-12">
                                            <dxe:ASPxGridView runat="server" KeyFieldName="SEQ" ClientInstanceName="cgridproducts" ID="grid_RateLog"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                                OnCustomCallback="ProductRateLog_Callback" OnDataBinding="grid_RateLog_DataBinding"
                                                Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                                <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="STATE" Width="100" ReadOnly="true" Caption="State">
                                                        <Settings AutoFilterCondition="Contains" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Code" ReadOnly="true" Caption="Product Code" Width="100">
                                                        <Settings AutoFilterCondition="Contains" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Description" ReadOnly="true" Width="100" Caption="Product Description">
                                                        <Settings AutoFilterCondition="Contains" />

                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Price to Distributor" FieldName="DistributorPrice" Width="70" VisibleIndex="4">
                                                        <PropertiesTextEdit DisplayFormatString="0.00">
                                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Price to Retailer" FieldName="RetailerPrice" Width="70" VisibleIndex="5">
                                                        <PropertiesTextEdit DisplayFormatString="0.00">
                                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Status" ReadOnly="true" Width="100" Caption="Status">
                                                        <Settings AutoFilterCondition="Contains" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Reason" ReadOnly="true" Width="100" Caption="Reason">
                                                        <Settings AutoFilterCondition="Contains" />
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                            </dxe:ASPxGridView>

                                            <dxe:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="grid_RateLog" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                                            </dxe:ASPxGridViewExporter>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
