<%--=======================================================Revision History=====================================================    
    1.0   Pallab    V2.0.38   12-05-2023      26111: Add Stock Adjustment module design modification & check in small device
=========================================================End Revision History===================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="StockAdjustmentAdd.aspx.cs" Inherits="ERP.OMS.Management.Activities.StockAdjustmentAdd" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc1" TagName="UOMConversionControl" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js?v=0.02"></script>
    <script src="JS/StockAdjustment.js?v=1.6"></script>
    <script>
        var module = 'Stock Adjustment ';
        function MSGShow() {
            jAlert($('#hdnMsg').val(), "Alert", function () { window.location.href = 'StockAdjustmentList.aspx'; });
        }

        function lOCKMSGShow() {
            jAlert($('#hDNlOCKMSG').val());
        }

        function adjAmountLostFocus() {
            var totValue = parseFloat(ctxtStockInHand.GetValue()) + parseFloat(ctxtEnterAdjustQty.GetValue());
            ctxtTotalStockInHand.SetValue(parseFloat(ctxtStockInHand.GetValue()) + parseFloat(ctxtEnterAdjustQty.GetValue()));
            $("#hdnTotalStockInHand").val(totValue);
            ctxtEnterRate.Focus();
        }
        //30-04-2019 UOM Conversion In Entry Surojit Chatterjee
        function adjAmountGotFocus() {
            //debugger;
            var adjustqty = parseFloat(ctxtEnterAdjustQty.GetValue());
            var ProductID = GetObjectID('hdnProductId').value.trim();
            var IsInventory = '';
            var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
            var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
            var AllowQty = $('#hdnAllowQty').val();
            var type = 'add';
            //var prodqty = parseFloat(ctxtEnterAdjustQty.GetValue()).toFixed(5);
            var actionQry = '';
            var AdjustmentId = '';
            if ($('#hdAddEdit').val() == 'Add') {
                actionQry = 'StockAdjustmentProduct';
            }
            else {
                actionQry = 'GetStockAdjustmentProduct';
                AdjustmentId = $('#hdAdjustmentId').val();
            }
            var GetserviceURL = "../Activities/Services/Master.asmx/GetMultiUOMDetails";

            $.ajax({
                type: "POST",
                url: GetserviceURL,
                data: JSON.stringify({ orderid: ProductID, action: actionQry, module: 'StockAdjustment', strKey: AdjustmentId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var SpliteDetails = msg.d.split("||@||");
                    var IsInventory = '';
                    if (SpliteDetails[5] == "1") {
                        IsInventory = 'Yes';
                    }
                    var gridprodqty = '';
                    var gridPackingQty = '';
                    var slno = ccmbWarehouse.GetValue();
                    var strProductID = ProductID;
                    var isOverideConvertion = SpliteDetails[4];
                    if (AllowQty == "1")
                    {
                        isOverideConvertion = "0";
                    }
                    else
                    {
                        isOverideConvertion = SpliteDetails[4];
                    }

                   // var isOverideConvertion = SpliteDetails[4];
                   
                    var packing_saleUOM = SpliteDetails[2];
                    var sProduct_SaleUom = SpliteDetails[3];
                    var sProduct_quantity = SpliteDetails[0];
                    var packing_quantity = SpliteDetails[1];

                    if ($('#hdAddEdit').val() == 'Edit') {
                        gridPackingQty = SpliteDetails[6];
                        //gridprodqty = ctxtEnterAdjustQty.GetValue();
                        gridprodqty = SpliteDetails[7];
                    }
                    if ($('#hdAddEdit').val() == 'Add') {
                        if (parseFloat(ctxtEnterAdjustQty.GetValue()) != 0) {
                            gridprodqty = parseFloat(ctxtEnterAdjustQty.GetValue());
                            if ($('#hdnUOMpacking').val() != null && $('#hdnUOMpacking').val() != '') {
                                gridPackingQty = $('#hdnUOMpacking').val();
                            }
                        }
                    }
                    if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {



                       if (AllowQty == "1") {
                            ShowUOMExtraValue(type, "Stock Adjustment", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty, AllowQty);
                       }
                       else
                       {
                           ShowUOM(type, "Stock Adjustment", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);

                       }
                    }

                }
            });

        }

        var aarrUOM = [];

        function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
            ctxtEnterAdjustQty.SetValue(Quantity);
            $('#hdnUOMQuantity').val(Quantity);
            $('#hdnUOMpacking').val(packing);
            $('#hdnUOMPackingUom').val(PackingSelectUom);
            $('#hdnUOMPackingSelectUom').val(PackingUom);
            adjAmountLostFocus();
        }

        //function SetUOMConversionArray(WarehouseID) {
        //    alert();
        //    var Quantity = $('#hdnUOMQuantity').val();
        //    var packing = $('#hdnUOMpacking').val();
        //    var PackingUom = $('#hdnUOMPackingUom').val();
        //    var PackingSelectUom = $('#hdnUOMPackingSelectUom').val();
        //    var slnoget = WarehouseID;

        //    if (StockOfProduct.length > 0) {
        //        var extobj = {};
        //        var PackingUom = $('#hdnUOMPackingUom').val();
        //        var PackingSelectUom = $('#hdnUOMPackingSelectUom').val();

        //        var productidget = $('#hdfProductID').val();


        //        for (i = 0; i < aarrUOM.length; i++) {
        //            extobj = aarr[i];
        //            console.log(extobj);
        //            if (extobj.slno == slnoget && extobj.productid == productidget) {
        //                //aarr.pop(extobj);
        //                aarrUOM.splice(i, 1);
        //            }
        //            extobj = {};
        //        }


        //        var arrobj = {};
        //        arrobj.productid = productidget;
        //        arrobj.slno = slnoget;
        //        arrobj.Quantity = Quantity;
        //        arrobj.packing = packing;
        //        arrobj.PackingUom = PackingUom;
        //        arrobj.PackingSelectUom = PackingSelectUom;

        //        aarrUOM.push(arrobj);
        //    }
        //}

        //30-04-2019 UOM Conversion In Entry Surojit Chatterjee

        $(document).ready(function () {

            if ($('#hdAddEdit').val() != 'Add') {
                ctxtEnterAdjustQty.SetEnabled(false);
            }
            else {
                ctxtEnterAdjustQty.SetEnabled(true);
            }

            document.onkeydown = function (s, e) {
                if (event.keyCode == 88 && event.altKey == true) {
                    cbtn_SaveRecords.DoClick();
                    //s.OnButtonClick(0);
                }
            }
        });
        function ValidateEntry(s, e) {

            var ReturnValue = true;
            if (cCmbScheme.GetText().trim() == "" || cCmbScheme.GetText().trim() == "-Select-") {
                $('#MandatoryNumberingScheme').show();
                ReturnValue = false;
            }
            else {
                $('#MandatoryNumberingScheme').hide();
            }
            if (ctxtVoucherNo.GetText().trim() == "") {
                $('#MandatoryAdjNo').show();
                ReturnValue = false;
            } else {
                $('#MandatoryAdjNo').hide();
            }
            if ($('#ddlBranch').val() == "") {
                $('#MandatoryBranch').show();
                ReturnValue = false;
            }
            else {
                $('#MandatoryBranch').hide();
            }

            if (ccmbWarehouse.GetText().trim() == "") {
                $('#MandatoryWarehouse').show();
                ReturnValue = false;
            }
            else {
                $('#MandatoryWarehouse').hide();
            }
            if (GetObjectID('hdnProductId').value.trim() == "") {
                $('#MandatoryProduct').show();
                ReturnValue = false;
            } else {
                $('#MandatoryProduct').hide();
            }

            if (ctxtEnterAdjustQty.GetText().trim() == "" || ctxtEnterAdjustQty.GetText().trim() == "0.00") {
                $('#MandatoryAdjQty').show()
                ReturnValue = false;
            } else {
                $('#MandatoryAdjQty').hide();
            }
            if ($('#txtReason').val() == "") {
                $('#MandatoryReason').show()
                ReturnValue = false;
            } else {
                $('#MandatoryReason').hide();
            }

            if (GetObjectID('hdnAllowQty').value.trim() == "0") {
                if (parseFloat(ctxtEnterAdjustQty.GetValue()) == 0.00) {

                    jAlert("Adjustment Quantity must be greater than zero.", "Alert", function () { ctxtEnterAdjustQty.Focus(); });
                    ReturnValue = false;
                }
            }
            else if (GetObjectID('hdnAllowQty').value.trim() == "1") {
                if (parseFloat(ctxtEnterAdjustQty.GetValue()) == 0.00 && $('#hdnUOMpacking').val() =="0.0000") {

                    jAlert("Adjustment Quantity must be greater than zero.", "Alert", function () { ctxtEnterAdjustQty.Focus(); });
                    ReturnValue = false;
                }
            }
            //if (parseFloat(ctxtTotalStockInHand.GetValue()) < 0) {
            //    jAlert("Stock In Hand Quantity must be greater than zero.", "Alert", function () { ctxtEnterAdjustQty.Focus(); });
            //    ReturnValue = false;
            //}
            //if (parseFloat(ctxtTotalStockInHand.GetValue()) > parseFloat(ctxtStockInHand.GetValue())) {
            //    jAlert("Adjusted Quantity must be Less than or equal to Stock In Hand .", "Alert", function () { ctxtEnterAdjustQty.Focus(); });
            //    ReturnValue =false;
            //}

            // return ReturnValue;
            e.processOnServer = ReturnValue;

        }


        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
            }
        }

    </script>
    <style>
        .noBorderTbl {
            margin: 1px 0;
            width: 450px;
        }

            .noBorderTbl > tbody > tr > td {
                padding: 8px 0;
            }

        .modal-dialog.modal-sm {
            width: 650px;
        }

        .darkLabel {
            color: #191717;
        }

        .iconRed {
            position: absolute;
            right: -19px;
            top: 3px;
        }

        .dxeErrorFrameSys.dxeErrorCellSys {
            display: none;
        }

        .boxed {
            background: #f9f9f9;
            border: 1px solid #a3a4a5;
            padding: 20px;
            margin-bottom: 10px;
        }

        .rowBox {
            background: #f0f0e0;
            border: 1px solid #c3c3bd;
            padding: 6px 0;
            margin: 5px 0px;
        }

        .mBot11 {
            margin-bottom: 11px;
        }

        .formBox {
            max-width: 560px;
            background: #fff;
            padding: 22px;
            border-radius: 12px;
            box-shadow: 0px -4px 0px rgb(251, 151, 49), 0px 4px 0px #FF5722, 0px 4px 5px rgba(60, 62, 62, 0.22);
            position: relative;
            margin: 0 auto;
            z-index: 55;
        }

        .frmBox-Header {
            font-size: 17px;
            background: #1788bb;
            position: absolute;
            top: -37px;
            padding: 5px 18px;
            border-radius: 8px 18px 0 0;
            color: #fff;
        }

        .pdLeft5 {
            padding-left: 5px;
        }

        .uLabel {
            background: #ff8100;
            padding: 2px 3px;
            border-radius: 4px;
            color: #fff;
        }

        .crossBtn.stkCross {
            top: -40px;
            width: 30px;
            height: 30px;
            line-height: 28px !important;
        }

        .btn-radius {
            border-radius: 15px;
            padding: 5px 15px !important;
        }

        .mtop80 {
            margin-top: 13px;
        }

        .padTbl {
            width: 100%;
            margin-top: 20px;
        }

            .padTbl > tbody > tr > td {
                padding-bottom: 15px;
            }

        .pageOverlay {
            position: fixed;
            width: 100%;
            top: 0;
            left: 0;
            bottom: 0;
            background: rgba(0,0,0,0.74);
            z-index: 44;
        }

        .pagePopLabl {
            font-size: 18px;
            -webkit-transform: translateY(-12px);
            -moz-transform: translateY(-12px);
            transform: translateY(-12px);
        }

        .transMinus {
            -webkit-transform: translateY(-17px);
            -moz-transform: translateY(-17px);
            transform: translateY(-17px);
        }

        .crossBtn.pageTypepop {
            font-size: 14px !important;
            height: 25px;
            line-height: 25px !important;
            top: 14px;
            width: 25px;
        }

        .stLbl + div {
            text-align: center;
            padding: 0 10px;
        }
    </style>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dt_BTOut , #dt_refCreditNoteDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dt_BTOut_B-1 ,
        #dt_refCreditNoteDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dt_BTOut_B-1 #dt_BTOut_B-1Img ,
        #dt_refCreditNoteDt_B-1 #dt_refCreditNoteDt_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 18px;
                bottom: 6px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 0 !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 31px;
            right: 13px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
    background: #42b39e !important;
}

        #ddlInventory
        {
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
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-2 > label, .col-md-2 > span, .col-md-1 > label, .col-md-1 > span
        {
            margin-top: 0px;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
        }

        #gridDEstination
        {
            width:99% !important;
        }

        #txtEntity , #txtCustName
        {
            width: 100%;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12
            {
                 padding-right: 10px;
                 padding-left: 10px;
            }
            .simple-select::after
        {
                top: 31px;
            right: 8px;
        }
            .calendar-icon
        {
                right: 14px;
                bottom: 6px;
        }
        }

    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
    <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />

    <input type="hidden" value="0" id="hdnUOMQuantity" runat="server" />
    <input type="hidden" value="0" id="hdnUOMpacking" runat="server" />
    <input type="hidden" value="0" id="hdnUOMPackingUom" runat="server" />
    <input type="hidden" value="0" id="hdnUOMPackingSelectUom" runat="server" />

    <uc1:UOMConversionControl runat="server" ID="UOMConversionControl" />



    <%--<div id="ApprovalCross" runat="server" class="crossBtn"><a href="StockAdjustmentList.aspx"><i class="fa fa-times"></i></a></div>--%>

    <div class="pageOverlay"></div>
    <div class="">
        <div class=" clearfix formBox mtop80">
            <div id="ApprovalCross" runat="server" class="crossBtn pageTypepop"><a href="StockAdjustmentList.aspx"><i class="fa fa-times"></i></a></div>
            <label class="pagePopLabl">
                <asp:Label ID="lblHeading" runat="server" Text="Add Stock Adjustment"></asp:Label></label>
            <%--<div class="text-center frmBox-Header">Stock Adjustment</div>--%>
            <div class="row ">
                <div class="clearfix" style="padding-bottom: 14px; border-bottom: 1px solid #ccc;">
                    <div class="col-md-6 relative">
                        <label class="darkLabel">Numbering Scheme<span style="color: red">*</span></label>
                        <div class="relative">
                            <dxe:ASPxComboBox ID="CmbScheme" ClientInstanceName="cCmbScheme"
                                SelectedIndex="0" EnableCallbackMode="false"
                                TextField="SchemaName" ValueField="ID"
                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                <ClientSideEvents ValueChanged="CmbScheme_ValueChange"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                            <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                    <div class="col-md-6">
                        <label class="darkLabel">Document No</label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" MaxLength="16" Text="Auto" ClientEnabled="false" Width="100%">
                            </dxe:ASPxTextBox>
                            <span id="MandatoryAdjNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="col-md-6">
                        <label class="darkLabel mTop5">Date<span style="color: red">*</span></label>
                        <div class="relative">
                            <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom" AllowNull="false"
                                Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                <ButtonStyle Width="13px"></ButtonStyle>
                                <ClientSideEvents GotFocus="function(s,e){cdtTDate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}"></ClientSideEvents>
                            </dxe:ASPxDateEdit>
                            <span id="MandatoryDate" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                        <%--Rev 1.0--%>
                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                        <%--Rev end 1.0--%>
                    </div>
                    <%--Rev 1.0: "simple-select" class add --%>
                    <div class="col-md-6 simple-select">
                        <label class="darkLabel mTop5">Unit<span style="color: red">*</span></label>
                        <div class="relative">
                            <asp:DropDownList ID="ddlBranch" runat="server" 
                                DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                            </asp:DropDownList>
                            <span id="MandatoryBranch" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                </div>
            </div>

            <div class="clearfix">


                <table class="padTbl">
                    <tr id="DivTechnician" runat="server">
                        <td>
                            <label>Technician</label></td>
                        <td width="80px"></td>
                        <td>
                            <dxe:ASPxComboBox ID="ccmTechnician" runat="server" ClientInstanceName="cccmTechnician"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Warehouse Name<span style="color: red">*</span></label></td>
                        <td width="80px"></td>
                        <td>
                            <dxe:ASPxComboBox ID="cmbWarehouse" runat="server" ClientInstanceName="ccmbWarehouse"
                                Width="100%">
                                <ClientSideEvents ValueChanged="WH_ValueChange"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                            <span id="MandatoryWarehouse" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Product<span style="color: red">*</span></label></td>
                        <td width="80px"></td>
                        <td>
                            <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                            <span id="MandatoryProduct" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </td>
                    </tr>
                    <tr class="hide">
                        <td>
                            <label>Stock in hand</label></td>
                        <td width="80px"></td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <dxe:ASPxTextBox runat="server" ID="txtStockInHand" ClientInstanceName="ctxtStockInHand" Text="0.00" Width="100%" ClientEnabled="false">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td class="pdLeft5">
                                        <dxe:ASPxLabel runat="server" ID="lblStockHand" ClientInstanceName="clblStockHand" Text=""></dxe:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Adjustment Qty<span style="color: red">*</span></label></td>
                        <td class="pdLeft5" style="width: 80px;">
                            <dxe:ASPxLabel runat="server" ID="ASPxLabel1" ClientInstanceName="clblEnterAdjustQty" Text=""></dxe:ASPxLabel>
                        </td>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <dxe:ASPxTextBox runat="server" ID="txtEnterAdjustQty" ClientInstanceName="ctxtEnterAdjustQty" Text="0.00" Width="100%">
                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                            <%--  <ClientSideEvents KeyUp="adjAmountLostFocus" GotFocus="adjAmountGotFocus" />--%>
                                            <ClientSideEvents LostFocus="adjAmountLostFocus" GotFocus="adjAmountGotFocus" />
                                        </dxe:ASPxTextBox>
                                    </td>

                                </tr>
                            </table>


                            <span id="MandatoryAdjQty" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Rate</label></td>
                        <td></td>
                        <td>
                            <div class="relative">
                                <dxe:ASPxTextBox runat="server" ID="txtEnterRate" ClientInstanceName="ctxtEnterRate" Text="0.00" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </dxe:ASPxTextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                            </dxe:ASPxLabel>
                        </td>
                        <td></td>
                        <td>
                            <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataStock"
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
                                <ClientSideEvents GotFocus="Project_gotFocus" LostFocus="Project_LostFocus" ValueChanged="ProjectValueChange" />

                            </dxe:ASPxGridLookup>
                            <dx:LinqServerModeDataSource ID="EntityServerModeDataStock" runat="server" OnSelecting="ProjectServerModeDataSource_Selecting"
                                ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                            </dxe:ASPxLabel>
                        </td>
                        <td></td>
                        <td>
                            <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                            </asp:DropDownList>
                        </td>
                    </tr>



                </table>
                <div class="row mTop5  mBot11 hide">

                    <div class="col-md-2 hide">
                        <label>Total stock in hand</label>
                        <div class="relative">
                            <dxe:ASPxTextBox runat="server" ID="txtTotalStockInHand" ClientInstanceName="ctxtTotalStockInHand" Text="0.00" Width="100%" ClientEnabled="false">
                                <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" />
                            </dxe:ASPxTextBox>
                            <dxe:ASPxLabel runat="server" ID="ASPxLabel2" ClientInstanceName="clblTotalStockInHand" Text=""></dxe:ASPxLabel>
                        </div>
                    </div>


                </div>

                <div class="row mTop5  mBot11" style="padding-bottom: 14px; border-top: 1px solid #ccc; padding-top: 15px;">
                    <div class="col-md-12">
                        <label class="darkLabel">Reason<span style="color: red">*</span></label>
                        <div class="relative">
                            <textarea id="txtReason" rows="5" cols="10" runat="server" maxlength="500" style="height: 110px;"></textarea>
                            <span id="MandatoryReason" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                </div>

            </div>
            <div class="pull-right">
                <div class="inline-block mTop5 transMinus" id="divAvailableStk" onclick="AvailableStockClick()">
                    <label class="stLbl ">Available Stock</label>
                    <div>
                        <asp:Label ID="lblAvailableStk" runat="server" Text="0.0"></asp:Label>
                    </div>
                </div>
            </div>
            <table id="tblBtnSavePanel" style="margin-left: 3px;">
                <tr>
                    <%-- <td style="width: 100px;" id="tdSaveButton" runat="Server">                       
                        <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                        </dxe:ASPxButton> 
                       ClientSideEvents-Click="function (s,e){e.processOnServer== ValidateEntry();}"                      
                    </td>--%>
                    <td style="width: 100px;" id="td_SaveButton" runat="Server">
                        <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it"
                            CssClass="btn btn-success " OnClick="btn_SaveRecords_Click" UseSubmitBehavior="False">
                            <ClientSideEvents Click="ValidateEntry" />
                        </dxe:ASPxButton>
                    </td>


                </tr>
            </table>

        </div>
    </div>
    <div class="clear"></div>
    <div class="row">
        <div class="col-md-12">
        </div>
    </div>

    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField ID="hdnmultiwarehouse" runat="server" />
    <asp:HiddenField ID="hdnProductId" runat="server" />
    <asp:HiddenField ID="hdAdjustmentId" runat="server" />
    <asp:HiddenField ID="hdnMsg" runat="server" />
    <asp:HiddenField ID="hdnBranch" runat="server" />
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
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name Or Product Code" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">Id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>
                                <%--<th>Installation Reqd.</th>--%>
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
    <asp:HiddenField ID="hdnstockInHand" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTotalStockInHand" runat="server" Value="0" />
     <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
     <asp:HiddenField ID="hdnAllowQty" runat="server" />

      <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
     <asp:HiddenField ID="hDNlOCKMSG" runat="server" />

</asp:Content>
