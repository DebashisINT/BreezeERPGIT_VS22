<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesChallanAdd.aspx.cs"
    MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Activities.SalesChallanAdd" EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Sales_BillingShipping.ascx" TagPrefix="ucBS" TagName="Sales_BillingShipping" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/OtherTermsAndCondition.ascx" TagPrefix="ucOTC" TagName="OtherTermsAndCondition" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/ucVehicleDriverControl.ascx" TagPrefix="uc1" TagName="ucVehicleDriverControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/PosSalesInvoice.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <script src="JS/SearchPopupDatatable.js"></script>
    <link rel="stylesheet" href="http://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <script src="http://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script type='text/javascript'>
        var SecondUOM = [];
        var ModuleName = 'SC';
    </script>
    <script src="../Activities/JS/ProductStockIN.js?v1.00.00.08"></script>
    <!-- brance git feature-->
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <link href="CSS/saleschallanAdd.css" rel="stylesheet" />
   <%-- <script src="JS/SearchPopup.js"></script>--%>

    <%--  <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>--%>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelNew.js?v=1.0.1" type="text/javascript"></script>
    <%--Use for set focus on UOM after press ok on UOM--%>
    <style>
        .wrapHolder#pageheaderContent {
            width: 920px;
        }

        #DivCost {
            min-width: 100px;
        }
    </style>
    <script>
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
        $(function () {
            $('#UOMModal').on('hide.bs.modal', function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
            });
        });

    </script>
    <%--Use for set focus on UOM after press ok on UOM--%>

    <script>




        function callback_InlineRemarks_EndCall(s, e) {

            if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
                $("#txtInlineRemarks").focus();
            }
            else {
                cPopup_InlineRemarks.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 7);
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




        function lookup_Project_Click(s, e) {
            ASPxClientUtils.AttachEventToElement(s.GetMainElement(), "click", function (event) {
                s.ShowDropDown();
            });
        }
        function deleteTax(Action, srl, productid) {
            var OtherDetail = {};
            OtherDetail.Action = Action;
            OtherDetail.srl = srl;
            OtherDetail.prodid = productid;


            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/taxUpdatePanel_Callback",
                data: JSON.stringify(OtherDetail),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var Code = msg.d;

                    if (Code != null) {

                    }
                }
            });
        }


        //chinmoy start

        function closeMultiUOM(s, e) {
            e.cancel = false;
            // cPopup_MultiUOM.Hide();
        }
        $(function () {
            $(".allownumericwithdecimal").on("keypress keyup blur", function (event) {

                //$(this).val($(this).val().replace(/[^0-9\.]/g, ''));
                //if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                //    event.preventDefault();
                //}

                var patt = new RegExp(/[0-9]*[.]{1}[0-9]{4}/i);
                var matchedString = $(this).val().match(patt);
                if (matchedString) {
                    $(this).val(matchedString);
                }
                if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }

            });
        });

        function PopulateMultiUomAltQuantity() {

            var otherdet = {};
            var Quantity = $("#UOMQuantity").val();
            otherdet.Quantity = Quantity;
            var UomId = ccmbUOM.GetValue();
            otherdet.UomId = UomId;
            var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ProductID = Productdetails.split("||@||")[0];
            otherdet.ProductID = ProductID;
            var AltUomId = ccmbSecondUOM.GetValue();
            otherdet.AltUomId = AltUomId;

            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/GetPackingQuantity",
                data: JSON.stringify(otherdet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    if (msg.d.length != 0) {
                        var packingQuantity = msg.d[0].packing_quantity;
                        var sProduct_quantity = msg.d[0].sProduct_quantity;
                    }
                    else {
                        var packingQuantity = 0;
                        var sProduct_quantity = 0;
                    }
                    var uomfactor = 0
                    if (sProduct_quantity != 0 && packingQuantity != 0) {
                        uomfactor = parseFloat(packingQuantity / sProduct_quantity);
                        //.toFixed(4);
                        $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                    }
                    else {
                        $('#hddnuomFactor').val(0);
                    }

                    var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
                    var Qty = $("#UOMQuantity").val();
                    var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock);
                    //.toFixed(4);

                    //$("#AltUOMQuantity").val(calcQuantity);

                }
            });
        }

        function SaveMultiUOM() {
            //grid.GetEditor('ProductID').GetText().split("||@||")[3];

            var qnty = $("#UOMQuantity").val();


            var UomId = ccmbUOM.GetValue();
            //var UomId = ccmbUOM.SetSelectedIndex(grid.GetEditor('ProductID').GetText().split("||@||")[3] - 1);
            var UomName = ccmbUOM.GetText();
            //var AltQnty = parseFloat($("#AltUOMQuantity").val()).toFixed(4);
            var AltQnty = cAltUOMQuantity.GetValue();
            var AltUomId = ccmbSecondUOM.GetValue();
            var AltUomName = ccmbSecondUOM.GetText();
            // rev Sanchita
            grid.batchEditApi.StartEdit(globalRowIndex);
            // End of Rev Sanchita
            var srlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
            var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ProductID = Productdetails.split("||@||")[0];
            var DetailsId = grid.GetEditor('DetailsId').GetText();

            // Mantis Issue 24428
            var BaseRate = ccmbBaseRate.GetValue();
            var AltRate = ccmbAltRate.GetValue();
            if (ProductID == "") {
                ProductID = hdProductID.value;
            }
            if (DetailsId == "") {
                DetailsId = "0";
            }
            var UpdateRow = 'False';
            if ($("#chkUpdateRow").prop("checked")) {
                UpdateRow = 'True';
            }
            // End of Mantis Issue 24428
            // Rev Sanchita
            //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != ""
            //      && BaseRate != "0.0000" && AltRate != "0.0000") {
            if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty != "0.0000") {
                if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
                    // End of Rev Sanchita
                    // Mantis Issue 24428
                    if (cbtnMUltiUOM.GetText() == "Update") {
                        cgrid_MultiUOM.PerformCallback('UpdateRow~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow + '~' + hdMultiUOMID);
                        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
                        cAltUOMQuantity.SetValue("0.0000");
                        // Mantis Issue 24428
                        $("#UOMQuantity").val(0);
                        ccmbBaseRate.SetValue(0);
                        cAltUOMQuantity.SetValue(0);
                        ccmbAltRate.SetValue(0);
                        ccmbSecondUOM.SetValue("");
                        cgrid_MultiUOM.cpAllDetails = "";
                        cbtnMUltiUOM.SetText("Add");
                        // Rev Sanchita
                        $("#chkUpdateRow").prop('checked', false);
                        $("#chkUpdateRow").removeAttr("checked");
                        // End of Rev Sanchita

                    }
                    else {
                        // End of Mantis Issue 24428
                        // Mantis Issue 24428

                        // End of Mantis Issue 24428
                        // Mantis Issue 24428
                        // cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId);
                        cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow);
                        // End of Mantis Issue 24428
                        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
                        cAltUOMQuantity.SetValue("0.0000");
                        // Mantis Issue 24428
                        $("#UOMQuantity").val(0);
                        ccmbBaseRate.SetValue(0)
                        cAltUOMQuantity.SetValue(0)
                        ccmbAltRate.SetValue(0)
                        ccmbSecondUOM.SetValue("")
                        // Rev Sanchita
                        $("#chkUpdateRow").prop('checked', false);
                        $("#chkUpdateRow").removeAttr("checked");
                        // End of Rev Sanchita
                    }
                    // End of Mantis Issue 24428
                    // Rev Sanchita
                }
                else {
                    return;
                }
                // End of Rev Sanchita
            }
            else {
                return;
            }
        }

        function Delete_MultiUom(keyValue, SrlNo, DetailsId) {


            cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo + '~' + DetailsId);
            cgrid_MultiUOM.cpDuplicateAltUOM = "";

        }
        var hdMultiUOMID = "";
        function OnMultiUOMEndCallback(s, e) {
            if (cgrid_MultiUOM.cpDuplicateAltUOM == "DuplicateAltUOM") {
                jAlert("Please Enter Different Alt. Quantity.");
                return;
            }
            // Mantis Issue 24428
            if (cgrid_MultiUOM.cpSetBaseQtyRateInGrid != null && cgrid_MultiUOM.cpSetBaseQtyRateInGrid == "1") {
                grid.batchEditApi.StartEdit(globalRowIndex, 6);

                var BaseQty = cgrid_MultiUOM.cpBaseQty;
                var BaseRate = cgrid_MultiUOM.cpBaseRate;

                grid.GetEditor("Quantity").SetValue(BaseQty);
                grid.GetEditor("SalePrice").SetValue(BaseRate);
                grid.GetEditor("Amount").SetValue(BaseQty * BaseRate)


                grid.GetEditor("Order_AltQuantity").SetValue(cgrid_MultiUOM.cpAltQty);
                grid.GetEditor("Order_AltUOM").SetValue(cgrid_MultiUOM.cpAltUom);
                // Rev Sanchita
                SalePriceTextChange(null, null);
                // End of Rev Sanchita

            }

            if (cgrid_MultiUOM.cpAllDetails == "EditData") {
                var Quan = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
                $('#UOMQuantity').val(Quan);
                //$('#UOMQuantity').val(cgrid_MultiUOM.cpBaseQty);
                ccmbBaseRate.SetValue(cgrid_MultiUOM.cpBaseRate)
                ccmbSecondUOM.SetValue(cgrid_MultiUOM.cpAltUom);
                cAltUOMQuantity.SetValue(cgrid_MultiUOM.cpAltQty);
                ccmbAltRate.SetValue(cgrid_MultiUOM.cpAltRate);
                hdMultiUOMID = cgrid_MultiUOM.cpuomid;
                if (cgrid_MultiUOM.cpUpdatedrow == true) {
                    $("#chkUpdateRow").prop('checked', true);
                    $("#chkUpdateRow").attr('checked', 'checked');



                }
                else {
                    $("#chkUpdateRow").prop('checked', false);
                    $("#chkUpdateRow").removeAttr("checked");
                }

            }

            // End of Mantis Issue 24428
            if (cgrid_MultiUOM.cpOpenFocus == "OpenFocus") {
                ccmbSecondUOM.SetFocus();
            }

        }
        function FinalMultiUOM() {
            UomLenthCalculation();
            if (Uomlength == 0 || Uomlength < 0) {

                // Mantis Issue 24428 
                //jAlert("Please add Alt. Quantity.");
                jAlert("Please add atleast one Alt. Quantity with Update Row as checked.");
                // End of Mantis Issue 24428 
                return;
            }
            else {
                cPopup_MultiUOM.Hide();
                // Mantis Issue 24428 
                var SLNo = grid.GetEditor('SrlNo').GetValue();
                cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~' + SLNo);
                // End of Mantis Issue 24428 
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 10);
                }, 200)
            }
        }
        // Mantis Issue 24428
        function Edit_MultiUom(keyValue, SrlNo) {

            cbtnMUltiUOM.SetText("Update");
            cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);

        }
        // End of Mantis Issue 24428

        function AutoPopulateMultiUOM() {

            var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ProductID = Productdetails.split("||@||")[0];
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/AutoPopulateAltQuantity",
                data: JSON.stringify({ ProductID: ProductID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    if (msg.d.length != 0) {
                        var packingQuantity = msg.d[0].packing_quantity;
                        var sProduct_quantity = msg.d[0].sProduct_quantity;
                        var AltUOMId = msg.d[0].AltUOMId;
                    }
                    else {
                        var packingQuantity = 0;
                        var sProduct_quantity = 0;
                        var AltUOMId = 0;
                    }
                    var uomfactor = 0
                    if (sProduct_quantity != 0 && packingQuantity != 0) {
                        uomfactor = parseFloat(packingQuantity / sProduct_quantity);
                        //.toFixed(4);
                        $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                    }
                    else {
                        $('#hddnuomFactor').val(0);
                    }

                    var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
                    var Qty = QuantityValue;
                    var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock);
                    //.toFixed(4);
                    if ($("#hdnPageStatus").val() == "update") {
                        ccmbSecondUOM.SetValue('');
                        //$("#AltUOMQuantity").val(calcQuantity);

                        //cAltUOMQuantity.SetValue(calcQuantity);
                        cAltUOMQuantity.SetValue("0.0000");
                    }
                    else {
                        // ccmbSecondUOM.SetValue(AltUOMId);
                        if (AltUOMId == 0) {
                            ccmbSecondUOM.SetValue('');
                        }
                        else {
                            ccmbSecondUOM.SetValue(AltUOMId);
                        }
                        //Rev Sanchita
                        //cAltUOMQuantity.SetValue(calcQuantity);
                        //End Rev Sanchita
                    }
                }
            });
        }
        //End
        //Rev Rajdip
        function ChangePackingByQuantityinjs() {

            if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                var Quantity = ctxtQuantity.GetValue();
                var packing = $('#txtPacking').val();
                if (packing == null || packing == '') {
                    $('#txtPacking').val(parseFloat(0).toFixed(4));
                    packing = $('#txtPacking').val();
                }

                if (Quantity == null || Quantity == '') {
                    $(e).val(parseFloat(0).toFixed(4));
                    Quantity = ctxtQuantity.GetValue();
                }
                var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);

                //Rev Subhra 05-03-2019
                //var calcQuantity = parseFloat(Quantity * packingqty).toFixed(4);
                var uomfac_Qty_to_stock = $('#hdnuomFactor').val();
                //var uomfac_Qty_to_stock = $('#hdnpackingqty').val();
                var calcQuantity = parseFloat(Quantity * uomfac_Qty_to_stock).toFixed(4);
                //End of Rev Subhra 05-03-2019
                //$('#txtAlterQty1').val(calcQuantity);
                CtxtPacking.SetText(calcQuantity);

                ChkDataDigitCount(Quantity);
            }
        }
        function ChkDataDigitCount(e) {
            var data = $(e).val();
            $(e).val(parseFloat(data).toFixed(4));
        }
        //---------------------
        ////Surojit 19-03-2019
        function ChangeQuantityByPacking1() {

            var isOverideConvertion = $('#hdnisOverideConvertion').val();
            if (isOverideConvertion == '1') {
                var packing = CtxtPacking.GetValue();
                var Quantity = ctxtQuantity.GetValue();
                if (packing == null || packing == '') {
                    $(e).val(parseFloat(0).toFixed(4));
                    packing = CtxtPacking.GetValue();
                }

                if (Quantity == null || Quantity == '') {
                    ctxtQuantity.SetValue(parseFloat(0).toFixed(4));

                    Quantity = ctxtQuantity.GetValue();
                }
                var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);


                //Rev Subhra 06-03-2019
                // var calcQuantity = parseFloat(packing / packingqty).toFixed(4);
                var uomfac_stock_to_qty = $('#hdnuomFactor').val();
                //var uomfac_stock_to_qty = $('#hdnpackingqty').val();
                //Rev Surojit 21-05-2019
                var calcQuantity = 0;
                if (parseFloat(uomfac_stock_to_qty) != 0) {
                    calcQuantity = parseFloat(packing / uomfac_stock_to_qty).toFixed(4);
                }
                //End of Rev Surojit 21-05-2019

                //End of Rev Subhra 06-03-2019
                ctxtQuantity.SetValue(calcQuantity);
            }
            ChkDataDigitCount(Quantity);
        }

        //End Rev Rajdip
    </script>
    <script type="text/javascript">
        //Subhabrata
        var canCallBack = true;
        function CustomerButnClick(s, e) {
            $('#CustModel').modal('show');
        }

        function SalesManButnClick(s, e) {
            $('#SalesManModel').modal('show');
        }

        function SalesManbtnKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#SalesManModel').modal('show');
            }
        }


        function AllControlInitilize() {

            //debugger;
            if (canCallBack) {

                grid.AddNewRow();
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbSaleschallan = grid.GetEditor("SrlNo");
                tbSaleschallan.SetValue(noofvisiblerows);
                grid.batchEditApi.EndEdit();
                $('#ddlInventory').focus();
                canCallBack = false;

                if ($("#hdnPageStatus").val() == "update") {
                    cddl_PosGst.SetEnabled(false);
                    AllowAddressShipToPartyState = false;
                    SetTotalTaxableAmount(0, 0);
                }

                if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                    $("#btnSecondUOM").removeClass('hide');
                }
                else {
                    $("#btnSecondUOM").addClass('hide');
                }
            }
        }

        function Customerkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtCustSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtCustSearch").val() != null && $("#txtCustSearch").val() != "") {
                    callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "GetContactPersonOnJSON");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
            else if (e.code == "Escape") {
                ctxtCustName.Focus();
            }
        }

        function SalesMankeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSalesManSearch").val();
            OtherDetails.CustomerId = $("#hdnCustomerId").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Name");
                if ($("#txtSalesManSearch").val() != null && $("#txtSalesManSearch").val() != "") {
                    //callonServer("Services/Master.asmx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "OnFocus");
                    callonServer("SalesChallanAdd.aspx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "OnFocus");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[salesmanIndex=0]"))
                    $("input[salesmanIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                ctxtSalesManAgent.Focus();
            }
        }


        var PosGstId = "";
        function PopulatePosGst(e) {
            PosGstId = cddl_PosGst.GetValue();
            if (PosGstId == "S") {
                cddl_PosGst.SetValue("S");
            }
            else if (PosGstId == "B") {
                cddl_PosGst.SetValue("B");
            }
            //if ($("#hdnPlaceShiptoParty").val() == "1") {	
            //    TaxDeleteForShipPartyChange();	
            //}	
        }
        function AfterSaveBillingShipiing(validate) {
            var modeval = '<%=Session["ActionType"] %>'
            if (modeval == "Add") {
                if (gridSalesOrderLookup.GetText() == "" && ($("#hdnProdModeval").val() != "1")) {
                    GetPosForGstValue();
                }
            }
            if (validate) {
                page.SetActiveTabIndex(0);
                page.tabs[0].SetEnabled(true);
                $("#crossBtnId").show();
            }
            else {
                page.SetActiveTabIndex(1);
                page.tabs[0].SetEnabled(false);
                $("#crossBtnId").hide();
            }
        }
        function GetPosForGstValue() {
            cddl_PosGst.ClearItems();
            if (cddl_PosGst.GetItemCount() == 0) {
                cddl_PosGst.AddItem(GetShippingStateName() + '[Shipping]', "S");
                cddl_PosGst.AddItem(GetBillingStateName() + '[Billing]', "B");
            }
            else if (cddl_PosGst.GetItemCount() > 2) {
                cddl_PosGst.ClearItems();
                //cddl_PosGst.RemoveItem(0);	
                //cddl_PosGst.RemoveItem(0);	
            }
            if (PosGstId == "" || PosGstId == null) {
                cddl_PosGst.SetValue("S");
            }
            else {
                cddl_PosGst.SetValue(PosGstId);
            }
        }


        function OnFocus(Id, Name) {
            //debugger;
            $("#<%=hdnSalesManAgentId.ClientID%>").val(Id);

            ctxtCreditDays.Focus();
            //chinmoy 123
            ctxtSalesManAgent.SetText(Name);
            //chinmoy 123
            $('#SalesManModel').modal('hide');
        }

        function ValueSelected(e, indexName) {
            //debugger;
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;

                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex") {
                        SetProduct(Id, name);
                    }
                    else if (indexName == "salesmanIndex") {
                        OnFocus(Id, name);
                    }
                    else if (indexName == "customerIndex") {
                        $('#CustModel').modal('hide');
                        GetContactPersonOnJSON(Id, name);

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
                    if (indexName == "ProdIndex")
                        $('#txtProdSearch').focus();
                    else if (indexName == "salesmanIndex")
                        ctxtCreditDays.Focus();
                    else
                        $('#txtCustSearch').focus();
                }
            }

        }
        function CustomerKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#CustModel').modal('show');
            }
        }

        //function SalesMankeydown(s, e) {
        //    if (e.htmlEvent.key == "Enter") {
        //        $('#SalesManModel').modal('show');
        //    }
        //}


        //END





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

                        //if (type != '' && type != null) {
                        //    cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                        //}
                        //grid.PerformCallback('GridBlank');
                        //cContactPerson.PerformCallback('BindContactPerson~' + key);
                    }

                }
                else {
                    //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                    //var key = gridLookup.GetValue();
                    var key = $("#<%=hdnCustomerId.ClientID%>").val();
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    GetObjectID('hdnCustomerId').value = key;
                    if (key != null && key != '') {
                        //cContactPerson.PerformCallback('BindContactPerson~' + key);
                        //if (type != '' && type != null) {
                        //    cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                        //}
                    }
                }

                $('#CustModel').modal('hide');

            }
        }
    </script>
    <script>
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

        function Onddl_VatGstCstEndCallback(s, e) {
            if (s.GetItemCount() == 1) {
                cddlVatGstCst.SetEnabled(false);
            }
        }
        var isCtrl = false;
        document.onkeyup = function (e) {
            if (event.keyCode == 17) {
                isCtrl = false;
            }
            else if (event.keyCode == 27) {
                //     btnCancel_Click();
            }
        }

        document.onkeydown = function (e) {
            if (event.keyCode == 78 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + N -- ie, Save & New  
                StopDefaultAction(e);
                Save_ButtonClick();
            }
            else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + X -- ie, Save & Exit!     
                StopDefaultAction(e);
                SaveExit_ButtonClick();
            }
            else if (event.keyCode == 84 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + T -- ie, Tax & Charges!     
                StopDefaultAction(e);
                Save_TaxesClick();
            }
            else if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Add New
                StopDefaultAction(e);
                if (page.GetActiveTabIndex() == 1) {
                    fnSaveBillingShipping();
                }
            }
            else if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                page.SetActiveTabIndex(0);
                gridLookup.Focus();
            }
            else if (event.keyCode == 77 && event.altKey == true) {
                $('#TermsConditionseModal').modal({
                    show: 'true'
                });
            }
            else if (event.keyCode == 69 && event.altKey == true) {
                if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
                    StopDefaultAction(e);
                    SaveTermsConditionData();
                }
            }
            else if (event.keyCode == 76 && event.altKey == true) {
                StopDefaultAction(e);
                calcelbuttonclick();
            }
            else if (event.keyCode == 83 && event.altKey == true) {
                if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                    SaveVehicleControlData();
                }
            }
            else if (event.keyCode == 67 && event.altKey == true) {
                modalShowHide(0);
            }
            else if (event.keyCode == 82 && event.altKey == true) {
                modalShowHide(1);
                $('body').on('shown.bs.modal', '#exampleModal', function () {
                    $('input:visible:enabled:first', this).focus();
                });
            }
        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
    </script>

    <%--Batch Product Popup Start on 14-03-2017--%>

    <script>
        function gridFocusedRowChanged(s, e) {

            globalRowIndex = e.visibleIndex;
        }

        function ProductKeyDown(s, e) {
            console.log(e.htmlEvent.key);
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {

                s.OnButtonClick(0);
            }
        }

        function fn_Edit(keyValue) {

            //cGrdWarehouse.PerformCallback('EditWarehouse~' + keyValue);
            SelectedWarehouseID = keyValue;
            cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
        }

        function ProductButnClick(s, e) {

            //cproductLookUp.gridView.Refresh();

            if (e.buttonIndex == 0) {
                //commented On :09-01-2018
                //if (cproductLookUp.Clear()) {



                //    cProductpopUp.Show();
                //    cproductLookUp.Focus();
                //    cproductLookUp.ShowDropDown();


                //}
                //END
                if (!GetObjectID('hdnCustomerId').value) {
                    jAlert("Please Select Customer first.", "Alert", function () { $('#txtCustSearch').focus(); })
                    return;
                }
                $('#txtProdSearch').val('');
                $('#ProductModel').modal('show');
            }
        }

        function prodkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            OtherDetails.InventoryType = $("#<%=ddlInventory.ClientID%>").val();
            OtherDetails.ProductIds = '';
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Description");
                HeaderCaption.push("HSN/SAC");
                //HeaderCaption.push("Brand");
                //HeaderCaption.push("IsInstallation");
                HeaderCaption.push("Class");
                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetSalesOrderProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                //grid.GetEditor("ProductName").Focus();
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }

        }


        function ProductLostFocused(s, e) {

            grid.GetEditor("Quantity").Focus();
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

            //debugger;
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
                    //jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                    //LoadingPanel.Hide();
                    //cacbpCrpUdf.cpUDF = null;
                    //cacbpCrpUdf.cpTransport = null;
                    //cacbpCrpUdf.cpTC = null;

                    grid.UpdateEdit();
                }


            }
        }

        function CreditDays_TextChanged(s, e) {

            //var CreditDays = ctxtCreditDays.GetValue();

            //var today = new Date();
            //var newdate = new Date();
            //newdate.setDate(today.getDate() + Math.round(CreditDays));

            //cdt_SaleInvoiceDue.SetDate(newdate);

            var CreditDays = ctxtCreditDays.GetValue();

            //var today = new Date();
            var today = cPLSalesChallanDate.GetDate();
            //var newdate = new Date();
            var newdate = cPLSalesChallanDate.GetDate();
            newdate.setDate(today.getDate() + Math.round(CreditDays));

            cdt_SaleInvoiceDue.SetDate(newdate);
            cdt_SaleInvoiceDue.SetEnabled(false);
        }


        function ddlInventory_OnChange() {
            //cproductLookUp.GetGridView().Refresh();
        }

        function ProductlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 6);

            }
        }

        var cpstockVal;
        function DeleteProdBySl(sl, strProductID, branch) {


            grid.batchEditApi.StartEdit(globalRowIndex);
            //var otherProddet = {};
            //otherProddet.sl = sl;
            //otherProddet.strProductID = strProductID;

            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/DelProdbySl",
                data: JSON.stringify({ sl: sl, strProductID: strProductID, branch: branch }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    cpstockVal = msg.d;
                    divpopupAvailableStock.style.display = "block";

                    var AvlStk = cpstockVal + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                    //Rev Rajdip
                    document.getElementById('<%=lblAvailableSStk.ClientID %>').innerHTML = cpstockVal;

                    //End Rev rajdip
                    document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cpstockVal;
                    document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;

                    cpstockVal = null;
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                    return false;


                }
            });
        }

        function SetProduct(Id, Name) {
            var LookUpData = Id;//cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            //var focusedRow = cproductLookUp.gridView.GetFocusedRowIndex();
            var ProductCode = Name;//cproductLookUp.gridView.GetRow(focusedRow).children[1].innerText;
            //var ProductCode = cproductLookUp.GetValue();
            $("#hdnProdModeval").val("1");
            $('#ProductModel').modal('hide');

            if (!ProductCode) {
                LookUpData = null;
            }
            cddl_PosGst.SetEnabled(false);
            AllowAddressShipToPartyState = false;
            //cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);


            pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            //var tbStkUOM = grid.GetEditor("StockUOM");
            //var tbStockQuantity = grid.GetEditor("StockQuantity");

            var ProductID = LookUpData;
            if (LookUpData != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strSalePrice = SpliteDetails[6];

                if (grid.GetEditor("ProductID").GetValue() != "" && grid.GetEditor("ProductID").GetValue() != null) {

                    var sl = grid.GetEditor("SrlNo").GetValue();
                    var branch = $("#ddl_Branch").val();
                    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
                    DeleteProdBySl(sl, strProductID, branch)
                }

                tbDescription.SetValue(strDescription);
                tbUOM.SetValue(strUOM);
                tbSalePrice.SetValue(strSalePrice);
                divPacking.style.display = "none";
                grid.GetEditor("Quantity").SetValue("0.00");
                grid.GetEditor("Discount").SetValue("0.00");
                grid.GetEditor("Amount").SetValue("0.00");
                grid.GetEditor("TaxAmount").SetValue("0.00");
                grid.GetEditor("TotalAmount").SetValue("0.00");

                $('#<%= lblStkQty.ClientID %>').text("0.00");
                $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
                //cacpAvailableStock.PerformCallback(strProductID);
                //alert(globalRowIndex);

                //Rev work start 28.06.2022 Mantise no:24949
                //GetSalesRateSchemePrice($("#hdnCustomerId").val(), strProductID, "0");
                if ($('#hdnSettings').val() == "YES") {
                    GetSalesRateSchemePrice($("#hdnCustomerId").val(), strProductID, "0");
                }
                //Rev work close 28.06.2022 Mantise no:24949

                var sProduct_PurPrice = SpliteDetails[13];
                var sProduct_MinSalePrice = SpliteDetails[17];
                var sProduct_MRP = SpliteDetails[18];
                var sProduct_Cost = SpliteDetails[28];
                $('#lblSell').text(strSalePrice);
                $('#lblMRP').text(sProduct_MRP);
                $('#lblPurchase').text(sProduct_PurPrice);
                $('#lblCost').text(sProduct_Cost);

                if ($('#hdnPricingDetail').val() == "1") {
                    $('#DivSell').css({ 'display': 'block' });
                    $('#DivMRP').css({ 'display': 'block' });
                    $('#DivPurchase').css({ 'display': 'block' });
                    $('#DivCost').css({ 'display': 'block' });
                }
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                SetFocusAfterProductSelect();
                //Mantis Issue 24949
                setTimeout(function () {
                    if ($("#ProductMinPrice").val() != "") {
                        grid.GetEditor("SalePrice").SetValue($("#ProductMinPrice").val());
                    }
                }, 200);
                //End of Mantis Issue 24949
            }
        }

        function SetFocusAfterProductSelect() {
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }, 200);
        }

        function ProductSelected(s, e) {
            var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            var focusedRow = cproductLookUp.gridView.GetFocusedRowIndex();
            var ProductCode = cproductLookUp.gridView.GetRow(focusedRow).children[1].innerText;
            //var ProductCode = cproductLookUp.GetValue();



            if (!ProductCode) {
                LookUpData = null;
            }
            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);


            pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            //var tbStkUOM = grid.GetEditor("StockUOM");
            //var tbStockQuantity = grid.GetEditor("StockQuantity");

            var ProductID = LookUpData;
            if (LookUpData != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strSalePrice = SpliteDetails[6];



                tbDescription.SetValue(strDescription);
                tbUOM.SetValue(strUOM);
                tbSalePrice.SetValue(strSalePrice);
                divPacking.style.display = "none";
                grid.GetEditor("Quantity").SetValue("0.00");
                grid.GetEditor("Discount").SetValue("0.00");
                grid.GetEditor("Amount").SetValue("0.00");
                grid.GetEditor("TaxAmount").SetValue("0.00");
                grid.GetEditor("TotalAmount").SetValue("0.00");

                $('#<%= lblStkQty.ClientID %>').text("0.00");
                $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
                cacpAvailableStock.PerformCallback(strProductID);
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }

        }
    </script>

    <%--Batch Product Popup End--%>

    <%--Debu Section--%>
    <script type="text/javascript">
        <%--kaushik Section--%>

        var Pre_TotalAmt = "0";

        function DiscountGotFocus(s, e) {

            var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
            Pre_TotalAmt = _Amount;
        }
        //REv Rajdip
        //function QuantityGotFocus(s, e) {
        //    debugger;
        //    var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
        //    Pre_TotalAmt = _Amount;
        //    //debugger;
        //    //Surojit 25-02-2019
        //    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        //    var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        //    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        //    var SpliteDetails = ProductID.split("||@||");
        //    var strProductID = SpliteDetails[0];
        //    var strDescription = SpliteDetails[1];
        //    var strUOM = SpliteDetails[2];
        //    var strStkUOM = SpliteDetails[4];
        //    var strSalePrice = SpliteDetails[6];
        //    var IsPackingActive = SpliteDetails[10];
        //    var Packing_Factor = SpliteDetails[11];
        //    var Packing_UOM = SpliteDetails[12];
        //    var strProductShortCode = SpliteDetails[14];
        //    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

        //    strProductName = strDescription;

        //    var isOverideConvertion = SpliteDetails[26];
        //    var packing_saleUOM = SpliteDetails[25];
        //    var sProduct_SaleUom = SpliteDetails[24];
        //    var sProduct_quantity = SpliteDetails[22];
        //    var packing_quantity = SpliteDetails[20];

        //    var slno = (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

        //    var ComponentNumber = (grid.GetEditor('Order_Num').GetText() != null) ? grid.GetEditor('Order_Num').GetText() : "0";

        //    var rdl_SaleInvoice = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

        //    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
        //    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
        //    var type = 'add';
        //    var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
        //    var gridPackingQty = '';
        //    var IsInventory = '';

        //    //var gridPackingQty = grid.GetEditor('QuoteDetails_PackingQty').GetText();
        //    if (SpliteDetails.length == 27) {
        //        if (SpliteDetails[26] == "1") {
        //            IsInventory = 'Yes';
        //        }
        //    }

        //    if (SpliteDetails.length > 27) {
        //        if (SpliteDetails[27] == "1") {
        //            IsInventory = 'Yes';

        //            type = 'edit';

        //            if (SpliteDetails[28] != '') {
        //                if (parseFloat(SpliteDetails[28]) > 0) {
        //                    gridPackingQty = SpliteDetails[28];
        //                }
        //                else {
        //                    type = 'add';
        //                }
        //            }
        //            else {
        //                type = 'add';
        //            }

        //            var actionQry = '';


        //            if (ComponentNumber != "0" && ComponentNumber != "") {

        //                if (rdl_SaleInvoice == 'SO') {
        //                    actionQry = 'SalesChallanPackingQtyOrder';
        //                }
        //                if (rdl_SaleInvoice == 'SI') {
        //                    actionQry = 'SalesChallanPackingQtyInvoice';
        //                }

        //                $.ajax({
        //                    type: "POST",
        //                    url: "Services/Master.asmx/GetMultiUOMDetails",
        //                    data: JSON.stringify({ orderid: strProductID, action: actionQry, module: 'SalesChallan', strKey: ComponentNumber }),
        //                    contentType: "application/json; charset=utf-8",
        //                    dataType: "json",
        //                    success: function (msg) {

        //                        gridPackingQty = msg.d;
        //                        type = 'edit';
        //                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
        //                            ShowUOM(type, "SalesChallan", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
        //                        }

        //                    }
        //                });


        //            }
        //            else {

        //                actionQry = 'SalesChallanPackingQtyProductId';
        //                var orderid = grid.GetRowKey(globalRowIndex);
        //                $.ajax({
        //                    type: "POST",
        //                    url: "Services/Master.asmx/GetMultiUOMDetails",
        //                    data: JSON.stringify({ orderid: orderid, action: actionQry, module: 'SalesChallan', strKey: '' }),
        //                    contentType: "application/json; charset=utf-8",
        //                    dataType: "json",
        //                    success: function (msg) {

        //                        gridPackingQty = msg.d;
        //                        type = 'edit';
        //                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
        //                            ShowUOM(type, "SalesChallan", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
        //                        }

        //                    }
        //                });
        //            }

        //        }
        //    }
        //    else {



        //        debugger;
        //        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
        //            ShowUOM(type, "SalesChallan", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
        //        }
        //        else
        //        {
        //            alert('else');
        //        }
        //    }
        //    //Surojit 25-02-2019
        //}

        function QuantityGotFocusForMultoUOM(s, e) {
            if ($("#hddnMultiUOMSelection").val() == "1") {
                // Mantis Issue 24428
                //grid.batchEditApi.StartEdit(globalRowIndex, 5);
                // Mantis Issue End 24428
                // if ((gridquotationLookup.GetValue() != "") && (gridquotationLookup.GetValue() !=null)) {
                if (grid.GetEditor('Quantity').GetValue() != "0.0000") {
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                    $("#UOMQuantity").val(grid.GetEditor('Quantity').GetValue());
                }
                // }
            }
        }



        function QuantityGotFocus(s, e) {

            //Rajdip--------------------------------
            //var value = ctxtvalue.GetValue();
            //var rate = ctxtRate.GetValue();
            //var quantity = value / rate;
            //ctxtQty.SetValue(quantity);
            //--------------------------------------
            var ProductID = $('#hdfProductID').val();
            var Branch = $('#ddl_Branch').val();
            var WarehouseID = cCmbWarehouse.GetValue();

            var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
            var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();

            var type = 'add';
            var actionQry = 'WarehouseOpeningBalanceProduct';
            var GetserviceURL = "../Activities/Services/Master.asmx/GetMultiUOMDetails";

            $.ajax({
                type: "POST",
                url: GetserviceURL,
                data: JSON.stringify({ orderid: ProductID, action: actionQry, module: 'OpeningBalances', strKey: "" }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var SpliteDetails = msg.d.split("||@||");
                    var IsInventory = '';
                    if (SpliteDetails[5] == "1") {
                        IsInventory = 'Yes';
                    }
                    //Rev Subhra 0019966 08-05-2019
                    //var gridprodqty = '';
                    var gridprodqty = parseFloat(ctxtQuantity.GetText()).toFixed(4);
                    //End of Rev
                    var gridPackingQty = '';
                    var slno = WarehouseID;
                    var strProductID = ProductID;

                    var isOverideConvertion = SpliteDetails[4];
                    var packing_saleUOM = SpliteDetails[2];
                    var sProduct_SaleUom = SpliteDetails[3];
                    var sProduct_quantity = SpliteDetails[0];
                    var packing_quantity = SpliteDetails[1];

                    //if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                    //    ShowUOM(type, "Opening Balances", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                    //}
                    var uomfactor = 0
                    var prodquantity = sProduct_quantity;
                    var packingqty = packing_quantity;
                    $('#hdnpackingqty').val(packingqty);
                    if (prodquantity != 0 && packingqty != 0) {
                        uomfactor = parseFloat(packingqty / prodquantity).toFixed(4);
                        $('#hdnuomFactor').val(parseFloat(packingqty / prodquantity));
                    }
                    else {
                        $('#hdnuomFactor').val(0);
                    }
                    $('#hdnisOverideConvertion').val(isOverideConvertion);
                }
            });

        }
        //End Rev Rajdip

        var issavePacking = 0;

        function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
            issavePacking = 1;
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor('Quantity').SetValue(Quantity);
            SetFoucs();

        }

        function SetFoucs() {
            setTimeout(function () {
                // Mantis Issue 24428
                //grid.batchEditApi.StartEdit(globalRowIndex, 6);
                // Mantis Issue End 24428
            }, 600);

        }

        function SalesPriceGotFocus(s, e) {

            var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
            Pre_TotalAmt = _Amount;
        }



        function SalePriceTextChange(s, e) {
            pageheaderContent.style.display = "block";
            IsDiscountVal = $("#<%=IsDiscountPercentage.ClientID%>").val();
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetValue();
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strStkUOM = SpliteDetails[4];
            var strProductID = SpliteDetails[0];
            if (strProductID != "") {
                if ($("#ProductMinPrice").val() <= strSalePrice && $("#ProductMaxPrice").val() >= strSalePrice) {

                }
                else {
                    jAlert("Product Min price :" + $("#ProductMinPrice").val() + " and Max price :" + $("#ProductMaxPrice").val(), "Alert", function () {
                        grid.batchEditApi.StartEdit(globalRowIndex, 11);
                        return;
                    });
                }
            }
            if (strRate == 0) {
                strRate = 1;
            }
            var StockQuantity = strMultiplier * QuantityValue;
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
            var Amount = QuantityValue * strFactor * ((Saleprice.replace(/\,/g, '') * 1) / strRate);

            //Subhabrata on 04-12-2017
            var amountAfterDiscount = "0";
            var ResultamountAfterDiscount = "0";
            if (IsDiscountVal == "Y") {
                if (parseFloat(Discount) > 100) {
                    Discount = "0";

                    var tb_Discount = grid.GetEditor("Discount");
                    tb_Discount.SetValue(Discount);
                }

                ResultamountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
            }
            else {
                ResultamountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
                amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
            }

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);
            var TotaAmountRes = '';
            TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));
            //chinmoy edited below code start	
            var ShippingStateCode = "0";
            if (cddl_PosGst.GetValue == "S") {
                ShippingStateCode = GeteShippingStateID();
            }
            else {
                ShippingStateCode = GetBillingStateID();
            }
            //end
            var TaxType = "";
            if (cddl_AmountAre.GetValue() == "1") {
                TaxType = "E";
            }
            else if (cddl_AmountAre.GetValue() == "2") {
                TaxType = "I";
            }

            //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
            //    SpliteDetails[19], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val());

            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
             SpliteDetails[19], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), cPLSalesChallanDate.GetDate(), QuantityValue);

            DiscountTextChange(s, e);
            //Rev Rajdip           
            var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
            var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
            cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
            SetTotalTaxableAmount(s, e);
            SetInvoiceLebelValue();
            //End Rev rajdip
        }
        //Rev Rajdip
        function Taxlostfocus(s, e) {
            //DiscountTextChange(s, e);
            //Rev Rajdip for Running Balance
            SetTotalTaxableAmount(s, globalRowIndex);
            SetInvoiceLebelValue();
        }
        function TotalAmountgotfocus(s, e) {
            //DiscountTextChange(s, e);
            //Rev Rajdip for Running Balance
            SetTotalTaxableAmount(s, e);
            SetInvoiceLebelValue();
        }
        //End Rev Rajdip
        //'Subhabrata' on 15-03-2017
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
                //checkListBox.SetValue(SelectWarehouse);
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
                //divAvailableStk.style.display = "block";
                divpopupAvailableStock.style.display = "block";
                var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;
                document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                ctaxUpdatePanel.cpstock = null;
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return false;
            }
        }
        //End
        function DiscountTextChange(s, e) {
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
            IsDiscountVal = $("#<%=IsDiscountPercentage.ClientID%>").val();
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetValue();
            var SpliteDetails = ProductID.split("||@||");
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }
            if (strRate == 0) {
                strRate = 1;
            }
            var Amount = QuantityValue * strFactor * ((strSalePrice.replace(/\,/g, '') * 1) / strRate);
            //Subhabrata on 04-12-2017
            var amountAfterDiscount = "0";
            var ResultamountAfterDiscount = "0";
            if (IsDiscountVal == "Y") {
                if (parseFloat(Discount) > 100) {
                    Discount = "0";

                    var tb_Discount = grid.GetEditor("Discount");
                    tb_Discount.SetValue(Discount);
                }
                ResultamountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
            }
            else {
                ResultamountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
                amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
            }
            //End
            //var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);
            var TotaAmountRes = '';
            TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));
            //tbTotalAmount.SetValue(amountAfterDiscount);
            //Debjyoti section GST
            // var ShippingStateCode = $("#bsSCmbStateHF").val();
            //chinmoy edited below code start	
            var ShippingStateCode = "0";
            if (cddl_PosGst.GetValue == "S") {
                ShippingStateCode = GeteShippingStateID();
            }
            else {
                ShippingStateCode = GetBillingStateID();
            }
            //end
            var TaxType = "";
            if (cddl_AmountAre.GetValue() == "1") {
                TaxType = "E";
            }
            else if (cddl_AmountAre.GetValue() == "2") {
                TaxType = "I";
            }

            //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
            //    SpliteDetails[19], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val());

            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
               SpliteDetails[19], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), cPLSalesChallanDate.GetDate(), QuantityValue);

            if (parseFloat(Amount) != parseFloat(Pre_TotalAmt)) {
                deleteTax("DelQtybySl", grid.GetEditor("SrlNo").GetValue(), "")
            }
        }
        <%--kaushik Section--%>

        function SetEntityType(Id) {
            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/GetEntityType",
                data: JSON.stringify({ Id: Id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    $("#hdnEntityType").val(r.d);
                }
            });
        }

        function CmbScheme_ValueChange() {
            var val = $("#ddl_numberingScheme").val();
            $.ajax({
                type: "POST",
                url: 'PurchaseChallan.aspx/getSchemeType',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{sel_scheme_id:\"" + val + "\"}",
                success: function (type) {
                    var schemetypeValue = type.d;
                    var schemetype = schemetypeValue.toString().split('~')[0];
                    var schemelength = schemetypeValue.toString().split('~')[1];
                    $('#txt_SlChallanNo').attr('maxLength', schemelength);
                    if (schemetype == '0') {
                        <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";--%>
                        ctxt_SlChallanNo.SetText('');
                        ctxt_SlChallanNo.SetEnabled(true);
                        ctxt_SlChallanNo.Focus();
                    }
                    else if (schemetype == '1') {
                        <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";--%>
                        ctxt_SlChallanNo.SetText('Auto');
                        ctxt_SlChallanNo.SetEnabled(false);
                        cPLSalesChallanDate.Focus();
                    }
                    else if (schemetype == '2') {
                        <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";--%>
                    }
                    else if (schemetype == 'n') {
                        <%--document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";--%>
                    }
                }
            });
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
                    url: "SalesOrderAdd.aspx/GetRate",
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
        function CmbWarehouse_ValueChange() {
            var isFIFORequired = false;
            //Rev Rajdip For wirehousewise aviable stock
            var strProductID = $("#hdfProductID").val()
            var sl = grid.GetEditor("SrlNo").GetValue();
            var branch = $("#ddl_Branch").val();
            //End Rev Rajdip
            var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();
            var WarehouseID = cCmbWarehouse.GetValue();
            $("#<%=hddnWarehouseId.ClientID%>").val(WarehouseID);
            var type = document.getElementById('hdfProductType').value;
            ctxtMatchQty.SetValue(0);
            if (WarehouseID != null) {
                if (type == "WBS" || type == "WB") {
                    cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
                }
                else if (type == "WS" && FifoExists == "0") {
                    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + 'NoFIFO');
                }
            }
            //Rev Rajdip
            if (WarehouseID != null) {
                getwirehousewiseaviablestock(sl, strProductID, branch, WarehouseID)
            }

            //End Rev Rajdip
        }
        //Rev Rajdip for wirehousewise aviable stock
        function getwirehousewiseaviablestock(sl, strProductID, branch, WarehouseID) {
            grid.batchEditApi.StartEdit(globalRowIndex);
            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/getWarehousewisestock",
                data: JSON.stringify({ sl: sl, strProductID: strProductID, branch: branch, WarehouseID: WarehouseID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    cpstockVal = msg.d;
                    divpopupAvailableStock.style.display = "block";
                    document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cpstockVal;

                    cpstockVal = null;
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                    return false;


                }
            });
        }

        //end Rev Rajdip
        function CmbBatch_ValueChange() {

            var WarehouseID = cCmbWarehouse.GetValue();
            var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();
            var strProductID = $("#hdfProductID").val()
            var sl = grid.GetEditor("SrlNo").GetValue();
            var branch = $("#ddl_Branch").val();
            var BatchID = cCmbBatch.GetValue();
            ctxtMatchQty.SetValue(0);
            $("#<%=hddnBatchId.ClientID%>").val(BatchID);
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS" && FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + 'NoFIFO');
            }
            else if (type == "BS" && FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + 'NoFIFO');
            }
            else if (type == "WS" && FifoExists == "0") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0" + '~' + 'NoFIFO');
            }

            if (BatchID != null) {
                GetWirehouseBatchWiseAviableStock(sl, strProductID, branch, WarehouseID, BatchID);
            }
        }
        function GetWirehouseBatchWiseAviableStock(sl, strProductID, branch, WarehouseID, BatchID) {
            grid.batchEditApi.StartEdit(globalRowIndex);

            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/getWarehouseBatchwisestock",
                data: JSON.stringify({ sl: sl, strProductID: strProductID, branch: branch, WarehouseID: WarehouseID, BatchID: BatchID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    cpstockVal = msg.d;
                    divpopupAvailableStock.style.display = "block";
                    document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cpstockVal;

                    cpstockVal = null;
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                    return false;
                }
            });
        }

        $(document).ready(function () {

            var mode = $('#hdAddOrEdit').val();
            if (mode == 'Edit') {
                if ($("#hdAddOrEdit").val() != "") {
                    var VendorID = $("#hdnCustomerId").val();
                    SetEntityType(VendorID);
                }
            }
            // $("#rdl_SaleInvoice").val("SO").hide();

            if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                page.GetTabByName('Billing/Shipping').SetEnabled(false);
            }
            $('#ApprovalCross').click(function () {

                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.Refresh();
            })

            $('#CustModel').on('shown.bs.modal', function () {
                $('#txtCustSearch').focus();
            })

            $('#SalesManModel').on('shown.bs.modal', function () {
                $('#txtSalesManSearch').focus();
            })

            $('#ProductModel').on('shown.bs.modal', function () {
                $('#txtProdSearch').focus();
            })
            //var IsInventoryValue = ccmbIsInventory.GetValue();
            //cIsInventory.PerformCallback('BindSession' + '~' + IsInventoryValue);

            if ($("#hdnShowUOMConversionInEntry").val() != "1") {
                div_AltQuantity.style.display = 'none';
                _div_Uom.style.display = 'none';
            }

        })

        <%--kaushik 24-2-2017--%>
        function UniqueCodeCheck() {

            var SchemeVal = $('#<%=ddl_numberingScheme.ClientID %> option:selected').val();
            if (SchemeVal == "") {
                alert('Please Select Numbering Scheme');
                ctxt_SlChallanNo.SetValue('');
                ctxt_SlChallanNo.Focus();
            }
            else {
                var OrderNo = ctxt_SlChallanNo.GetText();
                if (OrderNo != '') {

                    var SchemaLength = GetObjectID('hdnSchemaLength').value;
                    var x = parseInt(SchemaLength);
                    var y = parseInt(OrderNo.length);

                    if (y > x) {
                        alert('Sales Order No length cannot be more than ' + x);
                        //jAlert('Please enter unique Sales Order No');
                        ctxt_SlChallanNo.SetValue('');
                        ctxt_SlChallanNo.Focus();

                    }
                    else {
                        var CheckUniqueCode = false;
                        $.ajax({
                            type: "POST",
                            url: "SalesChallanAdd.aspx/CheckUniqueCode",
                            data: JSON.stringify({ OrderNo: OrderNo }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                CheckUniqueCode = msg.d;
                                if (CheckUniqueCode == true) {
                                    alert('Please enter unique Sales Challan No');
                                    //jAlert('Please enter unique Sales Order No');
                                    ctxt_SlChallanNo.SetValue('');
                                    ctxt_SlChallanNo.Focus();
                                }
                                else {
                                    $('#MandatorySlOrderNo').attr('style', 'display:none');
                                }
                            }

                        });
                    }
                }
            }
        }

        //function ValidDateCheck() {
        //    debugger;
        //    if (cPLSalesChallanDate.GetDate() == null) {

        //    }
        //}

        function DateCheck() {
            //debugger;
            var startDate;

            if (cPLSalesChallanDate.GetDate() != null) {


                if (gridSalesOrderLookup.GetValue() != null) {
                    jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            page.SetActiveTabIndex(0);
                            ccmbGstCstVat.PerformCallback();
                            ccmbGstCstVatcharge.PerformCallback();
                            // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                            deleteTax("DeleteAllTax", "", "");
                            startDate = cPLSalesChallanDate.GetValueString();


                            //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                            var key = gridLookup.GetValue();
                            cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                            if (key != null && key != '') {
                                if (type != '' && type != null) {
                                    cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                                }
                                //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                            }
                            grid.PerformCallback('GridBlank');
                        }

                    });
                }
                else {
                    page.SetActiveTabIndex(0);
                    ccmbGstCstVat.PerformCallback();
                    ccmbGstCstVatcharge.PerformCallback();
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax("DeleteAllTax", "", "");
                    startDate = cPLSalesChallanDate.GetValueString();


                    //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                    var key = gridLookup.GetValue();
                    cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    if (key != null && key != '') {
                        if (type != '' && type != null) {
                            cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                        }
                        //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                    }
                    grid.PerformCallback('GridBlank');
                }
                gridSalesOrderLookup.gridView.Refresh();
                cProductsPopup.Hide();
            }
            else {
                cPLSalesChallanDate.SetText('');
                cPLSalesChallanDate.Focus();
                jAlert("Date is not valid.");
                return false;
            }
        }
        var SimilarProjectStatus = "0";
        function CloseGridQuotationLookup() {
            gridSalesOrderLookup.ConfirmCurrentSelection();
            gridSalesOrderLookup.HideDropDown();
            gridSalesOrderLookup.Focus();
            var quotetag_Id = gridSalesOrderLookup.gridView.GetSelectedKeysOnPage();

            if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {

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
                var Doctype = $("#rdl_SaleInvoice").find(":checked").val();
                $.ajax({
                    type: "POST",
                    url: "SalesChallanAdd.aspx/DocWiseSimilarProjectCheck",
                    data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        SimilarProjectStatus = msg.d;

                        if (SimilarProjectStatus != "1") {
                            cPLQADate.SetText("");
                            jAlert("Please select document with same project code to proceed.");

                            return false;

                        }
                    }
                });
            }

        }
        var GlobalCurTaxAmt = 0;
        var rowEditCtrl;
        var globalRowIndex;
        function GetVisibleIndex(s, e) {

            globalRowIndex = e.visibleIndex;
        }

        function cmbtaxCodeindexChange(s, e) {
            if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {

                var taxValue = s.GetValue();

                if (taxValue == null) {
                    taxValue = 0;
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(0);
                    ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
                }


                var isValid = taxValue.indexOf('~');
                if (isValid != -1) {
                    var rate = parseFloat(taxValue.split('~')[1]);
                    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                    ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
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

                    ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                    GlobalCurTaxAmt = 0;
                } else {
                    s.SetText("");
                }
            }

        }

        function CmbtaxClick(s, e) {
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }


        function txtTax_TextChanged(s, i, e) {


            cgridTax.batchEditApi.StartEdit(i, 2);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);




        }
        //Subhabrata Tax
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


            ctxtQuoteTaxTotalAmt.SetValue((totalTaxAmount));
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
        }

        function Save_TaxClick() {
            if (gridTax.GetVisibleRowsOnPage() > 0) {
                gridTax.UpdateEdit();
            }
            else {
                gridTax.PerformCallback('SaveGst');
            }
            //Rev Rajdip
            cbnrOtherChargesvalue.SetText('0.00');
            SetInvoiceLebelValueofothercharges();
            //End Rev Rajdip
            cPopup_Taxes.Hide();
        }
        function SetInvoiceLebelValueofothercharges() {

            //var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());

            //if (document.getElementById('HdPosType').value == 'Crd') {
            //    if (invValue < 0) {
            //        var newAdvAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
            //        cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(newAdvAmount) * 100) / 100).toFixed(2));
            //    }
            //}

            //if (document.getElementById('HdPosType').value == 'Fin') {
            //    if (invValue < 0) {
            //        var newAdvAmountfin = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
            //        cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(ctxtdownPayment.GetValue()) * 100) / 100).toFixed(2));
            //    }
            //}



            //if (document.getElementById('HdPosType').value == 'Crd')
            //    invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
            //else if (document.getElementById('HdPosType').value == 'Fin')
            //    invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue()) + parseFloat(cbnrOtherChargesvalue.GetValue());


            //cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));


            //SetRunningBalance();
            cbnrOtherChargesvalue.SetValue(ctxtQuoteTaxTotalAmt.GetText());
            if (ctxtTotalAmount.GetValue() == 0.0) {
                cbnrLblInvValue.SetValue(parseFloat(cbnrlblAmountWithTaxValue.GetValue()).toFixed(2));
            }
            else {
                cbnrLblInvValue.SetValue(parseFloat(ctxtTotalAmount.GetValue()).toFixed(2));
            }
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

        function recalculateTax() {
            cmbGstCstVatChange(ccmbGstCstVat);
        }

        function recalculateTaxCharge() {
            ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
        }

        function chargeCmbtaxClick(s, e) {
            GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
            ChargegstcstvatGlobalName = s.GetText();
        }
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
            //Rev Rajdip
            cbnrLblInvValue.SetText(ctxtTotalAmount.GetText());
            //End Rev Rajdip
        }

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


        function Save_TaxesClick() {
            //debugger;
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

            ctxtProductAmount.SetValue(Math.round(sumAmount).toFixed(2));
            ctxtProductTaxAmount.SetValue(Math.round(sumTaxAmount).toFixed(2));
            ctxtProductDiscount.SetValue(Math.round(sumDiscount).toFixed(2));
            ctxtProductNetAmount.SetValue(sumNetAmount.toFixed(2));
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

        function BatchUpdate() {

            //cgridTax.batchEditApi.StartEdit(0, 1);

            //if (cgridTax.GetEditor("TaxField").GetText().indexOf('.') == -1) {
            //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '.00');
            //} else {
            //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '0');
            //}

            //if (cgridTax.GetVisibleRowsOnPage() > 0) {
            //    page.tabs[1].SetEnabled(false);
            //    gridLookup.SetEnabled(false);
            //    cgridTax.UpdateEdit();
            //}
            //else {
            //    cgridTax.PerformCallback('SaveGST');
            //}

            cgridTax.UpdateEdit();


            return false;
        }

        function taxAmtButnClick(s, e) {

            if (e.buttonIndex == 0) {

                if (cddl_AmountAre.GetValue() != null) {
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

                    if (ProductID.trim() != "") {

                        document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();
                        caspxTaxpopUp.Show();
                        //Set Product Gross Amount and Net Amount

                        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                        var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
                        var SpliteDetails = ProductID.split("||@||");
                        var strMultiplier = SpliteDetails[7];
                        var strFactor = SpliteDetails[8];
                        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                        //var strRate = "1";
                        var strStkUOM = SpliteDetails[4];
                        // var strSalePrice = SpliteDetails[6];
                        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                        if (strRate == 0) {
                            strRate = 1;
                        }
                        //Subhabrata :Commented and Added on 15-12-2017
                        //var StockQuantity = strMultiplier * QuantityValue;
                        //var Amount = Math.round(QuantityValue * strFactor * ((strSalePrice.replace(/\,/g, '') * 1) / strRate)).toFixed(2);
                        //clblTaxProdGrossAmt.SetText(Amount);
                        //clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue()).toFixed(2));
                        //document.getElementById('HdProdGrossAmt').value = Amount;
                        //document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue()).toFixed(2);

                        document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                        var StockQuantity = strMultiplier * QuantityValue;
                        var Amount = parseFloat((Math.round((QuantityValue * strFactor * (strSalePrice / strRate)) * 100).toFixed(2)) / 100);

                        var IsDiscountPercentage = document.getElementById('IsDiscountPercentage').value;
                        var amountAfterDiscount = "0";
                        var ResultamountAfterDiscount = "0";
                        if (IsDiscountPercentage == "Y") {
                            ResultamountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                            amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
                        }
                        else {
                            ResultamountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
                            amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
                        }
                        //debugger;
                        //var _GrossAmt = parseFloat(amountAfterDiscount);
                        //alert(Amount);
                        var _GrossAmt = (Amount * 1);

                        //var _NetAmt = parseFloat(grid.GetEditor('Amount').GetValue());
                        var _NetAmt = (amountAfterDiscount * 1);

                        clblTaxProdGrossAmt.SetText(_GrossAmt.toFixed(2));
                        clblProdNetAmt.SetText(_NetAmt.toFixed(2));
                        document.getElementById('HdProdGrossAmt').value = _GrossAmt;
                        document.getElementById('HdProdNetAmt').value = _NetAmt;

                        //End Here

                        //Set Discount Here
                        if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                            //var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
                            //clblTaxDiscount.SetText(discount);

                            //var discount = Math.abs(parseFloat(Amount) - parseFloat(_GrossAmt));
                            //clblTaxDiscount.SetText(discount);
                            var DiscountAfter = parseFloat(grid.GetEditor('Discount').GetValue());
                            var discount = DiscountAfter.toFixed(2); //((Math.round((Math.abs(parseFloat(Amount) - parseFloat(_GrossAmt))) * 100).toFixed(2)) / 100);
                            //alert(discount);
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
                            $('.gstNetAmount').show();
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

                            var shippingStCode = '';

                            ////###### Added By : Samrat Roy ##########
                            //Get Customer Shipping StateCode
                            var shippingStCode = '';

                            //chinmoy edited below code start	

                            if (cddl_PosGst.GetValue == "S") {
                                shippingStCode = GeteShippingStateCode();
                            }
                            else {
                                shippingStCode = GetBillingStateCode();
                            }
                            //end	
                            // shippingStCode = cbsSCmbState.GetText();	
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


            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);

            // SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            //Set Running Total
            SetRunningTotal();

            RecalCulateTaxTotalAmountInline();
        }

        //function SetRunningTotal() {
        //    var runningTot = parseFloat(clblProdNetAmt.GetValue());
        //    for (var i = 0; i < taxJson.length; i++) {
        //        cgridTax.batchEditApi.StartEdit(i, 3);
        //        if (taxJson[i].applicableOn == "R") {
        //            cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
        //            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        //            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
        //            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        //            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
        //            var thisRunningAmt = 0;
        //            if (sign == '(+)') {
        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

        //                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
        //                GlobalCurTaxAmt = 0;
        //            }
        //            else {

        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

        //                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
        //                GlobalCurTaxAmt = 0;
        //            }
        //            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
        //        }
        //        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //        cgridTax.batchEditApi.EndEdit();
        //    }
        //}

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

        var taxAmountGlobal;
        function taxAmountGotFocus(s, e) {
            taxAmountGlobal = parseFloat(s.GetValue());
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
                // grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));
                var totalNetAmount = DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2);
                grid.GetEditor("TotalAmount").SetValue(totalNetAmount);

                //if (cddl_AmountAre.GetValue() == "2") {
                //    grid.GetEditor("Amount").SetValue(parseFloat(grid.GetEditor("Amount").GetValue()) + (totalRoundOffAmount - totalNetAmount));
                //    var finalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue()); 
                //    cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
                //}
                // grid.GetEditor("Amount").SetValue(parseFloat(grid.GetEditor("Amount").GetValue()) + (totalRoundOffAmount - totalNetAmount));

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

        function GetTaxVisibleIndex(s, e) {
            globalTaxRowIndex = e.visibleIndex;
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

        var gstcstvatGlobalName;
        function CmbtaxClick(s, e) {
            GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
            gstcstvatGlobalName = s.GetText();
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

                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);

                    //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

                    //Call for Running Total
                    SetRunningTotal();

                } else {
                    s.SetText("");
                }
            }

        }

        function taxAmtButnClick1(s, e) {
            rowEditCtrl = s;
        }


        $(document).ready(function () {

            ctxtRate.SetValue("");
            ctxtRate.SetEnabled(false);
            ctxt_SlChallanNo.SetEnabled(false);
            gridSalesOrderLookup.SetEnabled(false);

            PopulateLoadGSTCSTVAT();
        });
    </script>

    <%--Debu Section End--%>





    <script type="text/javascript">

        //window.onload = function () {
        //    // grid.AddNewRow();
        //    
        //    OnAddNewClick();
        //};
        var QuantityRes = '';
        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
            gridSalesOrderLookup.SetEnabled(true);
        }


        function ContactPersonBind(key) {
            $.ajax({
                type: "POST",
                url: "SalesOrderAdd.aspx/GetContactPersonafterBillingShipping",
                data: JSON.stringify({ Key: key }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (r) {
                    var contactPersonJsonObject = r.d;
                    //cContactPerson.SetValue(contactPerson);
                    IsContactperson = false;
                    SetDataSourceOnComboBox(cContactPerson, contactPersonJsonObject);
                    SetFocusAfterBillingShipping();
                }
            });
        }

        function GetContactPersonOnJSON(id, Name) {

            var IsContactperson = true;
            var startDate = new Date();
            startDate = cPLSalesChallanDate.GetValueString();
            if (gridSalesOrderLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        var key = id;
                        ctxtCustName.SetText(Name);
                        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                        GetObjectID('hdnCustomerId').value = key;

                        if (key != null && key != '') {
                            ContactPersonBind(key);
                            if (type != '' && type != null) {
                            }
                            page.GetTabByName('Billing/Shipping').SetEnabled(true);
                            SetDefaultBillingShippingAddress(key)
                            GetObjectID('hdnCustomerId').value = key;

                            GetObjectID('hdnAddressDtl').value = '0';
                        }
                    }
                });
            }
            else {

                //var key = gridLookup.GetValue();
                var key = id;
                ctxtCustName.SetText(Name);
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                GetObjectID('hdnCustomerId').value = key;
                if (key != null && key != '') {
                    ContactPersonBind(key);
                    page.SetActiveTabIndex(1);
                    $('.dxeErrorCellSys').addClass('abc');
                    page.SetActiveTabIndex(0);
                    $('.dxeErrorCellSys').addClass('abc');
                    page.GetTabByName('Billing/Shipping').SetEnabled(true);
                    if (type != '' && type != null) {
                    }
                    SetDefaultBillingShippingAddress(key)

                    GetObjectID('hdnCustomerId').value = key;
                    GetObjectID('hdnAddressDtl').value = '0';
                }

            }
            $('#CustModel').modal('hide');
            cProductsPopup.Hide();
            SetEntityType(id);
        }
        //Rev Rajdip For Customer Map To SalesMan
        function SalesmanBindWRTCustomer(Id) {

            $("#<%=hdnSalesManAgentId.ClientID%>").val("");
            //chinmoy 123
            ctxtSalesManAgent.SetText("");
            //chinmoy 123
            $.ajax({
                type: "POST",
                url: "SalesQuotation.aspx/MappedSalesManOnetoOne",
                data: JSON.stringify({ Id: Id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    var contactPersonJsonObject = r.d;
                    IsContactperson = false;
                    //SetDataSourceOnComboBox(cddlsalesmanmapped, contactPersonJsonObject);
                    if (r.d.length > 0) {
                        // cPopup_salesman.Show();
                        $("#<%=hdnSalesManAgentId.ClientID%>").val(r.d[0].Id);
                        //chinmoy 123
                        ctxtSalesManAgent.SetText(r.d[0].Name);
                        //chinmoy 123
                    }
                    <%--else if (r.d.length == 1) {
                        $("#<%=hdnSalesManAgentId.ClientID%>").val(r.d[0].Id);
                        ctxtSalesManAgent.SetText(r.d[0].Name);
                    }--%>
                }

            });
        }

        function Set_MappedSalesMan() {

            var Id = cddlsalesmanmapped.GetValue();
            var Name = cddlsalesmanmapped.GetText();
            $("#<%=hdnSalesManAgentId.ClientID%>").val(Id);

            ctxtCreditDays.Focus();
            //chinmoy 123
            ctxtSalesManAgent.SetText(Name);
            //chinmoy 123
            cPopup_salesman.Hide();
            $('#SalesManModel').modal('hide');

        }

        //End Rev Rajdip For Customer Map To SalesMan
        function SetFocusAfterBillingShipping() {
            setTimeout(function () {
                cContactPerson.Focus();
            }, 200);
        }

        function SetDataSourceOnComboBox(ControlObject, Source) {
            ControlObject.ClearItems();
            for (var count = 0; count < Source.length; count++) {
                ControlObject.AddItem(Source[count].Name, Source[count].Id);
            }
            ControlObject.SetSelectedIndex(0);
        }


        function SetDataSourceOnComboBoxandSetVal(ControlObject, Source, id) {
            ControlObject.ClearItems();
            for (var count = 0; count < Source.length; count++) {
                ControlObject.AddItem(Source[count].UOM_Name, Source[count].UOM_Id);
            }
            ControlObject.SetValue(id);
            // ControlObject.SetSelectedIndex(0);
        }

        function GetContactPerson(e) {
            //debugger;
            var CustomerComboBox = gridLookup.GetText();
            if (!gridLookup.FindItemByText(CustomerComboBox)) {
                gridLookup.SetValue("");
                gridLookup.Focus();
                jAlert("Customer not Exists.");
                return;
            }
            var startDate = new Date();
            startDate = cPLSalesChallanDate.GetValueString();
            if (gridSalesOrderLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                        var key = gridLookup.GetValue();

                        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                        GetObjectID('hdnCustomerId').value = key;
                        if (key != null && key != '') {

                            if (type != '' && type != null) {
                                //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                            }
                            //grid.PerformCallback('GridBlank');
                            //var startDate = new Date();
                            //startDate = cPLSalesChallanDate.GetValueString();

                            //cContactPerson.PerformCallback('BindContactPerson~' + key);


                            page.GetTabByName('Billing/Shipping').SetEnabled(true);

                            //###### Added By : Samrat Roy ##########
                            //cchkBilling.SetChecked(false);
                            //cchkShipping.SetChecked(false);
                            //page.SetActiveTabIndex(1);
                            //$('.dxeErrorCellSys').addClass('abc');
                            ////$('.crossBtn').hide();
                            //page.GetTabByName('Billing/Shipping').SetEnabled(true);
                            //page.GetTabByName('General').SetEnabled(false);


                            //chinmoy edited below code start	
                            //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SC');	
                            SetDefaultBillingShippingAddress(key)
                            //end	
                            GetObjectID('hdnCustomerId').value = key;
                            //chinmoy commented below code start	
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
                            //billingLookup.focus();	
                            //end
                            //###### END : Samrat Roy : END ########## 




                            //else {
                            //    page.SetActiveTabIndex(0);
                            //}
                            //});

                            GetObjectID('hdnAddressDtl').value = '0';

                            //document.getElementById('popup_ok').focus();
                        }
                    }
                });
            }
            else {
                //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                var key = gridLookup.GetValue();
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                GetObjectID('hdnCustomerId').value = key;
                if (key != null && key != '') {


                    //var startDate = new Date();
                    //startDate = cPLSalesChallanDate.GetValueString();

                    //cContactPerson.PerformCallback('BindContactPerson~' + key);


                    //page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                    //jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                    //    if (r == true) {
                    page.SetActiveTabIndex(1);
                    $('.dxeErrorCellSys').addClass('abc');
                    //$('.crossBtn').hide();
                    //page.GetTabByName('[B]illing/Shipping').SetEnabled(true);

                    page.SetActiveTabIndex(1);
                    $('.dxeErrorCellSys').addClass('abc');
                    //$('.crossBtn').hide();
                    page.GetTabByName('Billing/Shipping').SetEnabled(true);
                    page.GetTabByName('General').SetEnabled(false);
                    //        billingLookup.focus();
                    //    }
                    //});
                    if (type != '' && type != null) {
                        //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                    }

                    //###### Added By : Samrat Roy ##########
                    //cchkBilling.SetChecked(false);
                    //cchkShipping.SetChecked(false);
                    //page.SetActiveTabIndex(1);
                    //$('.dxeErrorCellSys').addClass('abc');
                    ////$('.crossBtn').hide();
                    //page.GetTabByName('Billing/Shipping').SetEnabled(true);
                    //page.GetTabByName('General').SetEnabled(false);

                    //chinmoy edited below code start	
                    //LoadCustomerAddress(key, $('#ddl_Branch').val());	
                    SetDefaultBillingShippingAddress(key)
                    //end	
                    GetObjectID('hdnCustomerId').value = key;
                    //chinmoy commented below code start	
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
                    //end	
                    //###### END : Samrat Roy : END ########## 	
                    GetObjectID('hdnAddressDtl').value = '0';

                    //document.getElementById('popup_ok').focus();	
                }
            }
            gridSalesOrderLookup.gridView.Refresh();
            cProductsPopup.Hide();
        }

        function SetDifference1() {
            var diff = CheckDifferenceOfFromDateWithTodate();
        }
        function CheckDifferenceOfFromDateWithTodate() {
            var startDate = new Date();
            var endDate = new Date();
            var difference = -1;
            startDate = cPLSalesChallanDate.GetDate();
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
            startDate = cPLSalesChallanDate.GetDate();
            if (startDate != null) {
                endDate = cExpiryDate.GetDate();
                var startTime = startDate.getTime();
                var endTime = endDate.getTime();
                difference = (endTime - startTime) / 86400000;

            }
            return difference;

        }

        $(document).ready(function () {
            //debugger;




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

                //###### Added By : Samrat Roy ##########


                //LoadCustomerAddress($('#hdnCustomerId').val(), $('#ddl_Branch').val(), 'SI');
                //BSDocTagging(BillValue, 'SI');

                //###### END : Samrat Roy : END ########## 

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
                        //ctxt_SlChallanNo.SetText('');
                        //ctxt_SlChallanNo.SetEnabled(true);
                        ctxt_SlChallanNo.Focus();

                    }

                    $("#<%=ddl_Branch.ClientID%>").val(BranchIdPending);
                    $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);
                    //  BSDocTagging(BillValue, 'SI');
                    if ($("#btn_TermsCondition").is(":visible")) {
                        callTCControl(BillValue, 'SI');
                    }
                }
            }

            $("#<%=hddnBranchId.ClientID%>").val($("#<%=ddl_Branch.ClientID%>").val());

            $("#<%=ddl_Branch.ClientID%>").change(function () {
                //debugger;
                var startDate;
                var ddl_BranchId;
                if (gridSalesOrderLookup.GetValue() != null) {
                    jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            page.SetActiveTabIndex(0);
                            ccmbGstCstVat.PerformCallback();
                            ccmbGstCstVatcharge.PerformCallback();
                            ctaxUpdatePanel.PerformCallback('DeleteAllTax');

                            startDate = cPLSalesChallanDate.GetValueString();
                            //ddl_BranchId = $("<%=ddl_Branch.ClientID%>").val();

                            //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                            var key = gridLookup.GetValue();
                            cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                            if (key != null && key != '') {
                                if (type != '' && type != null) {
                                    cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                                }
                                //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                            }
                            grid.PerformCallback('GridBlank');
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
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    deleteTax("DeleteAllTax", "", "");
                    startDate = cPLSalesChallanDate.GetValueString();


                    //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                    var key = gridLookup.GetValue();
                    cSalesOrderComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    if (key != null && key != '') {
                        if (type != '' && type != null) {
                            cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + '@' + '~' + type);
                        }
                        //cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                    }
                    grid.PerformCallback('GridBlank');
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


                //Cut Off  Valid from To Date Sudip
                if (NoSchemeType == '1') {
                    ctxt_SlChallanNo.SetText('Auto');
                    ctxt_SlChallanNo.SetEnabled(false);
                    cPLSalesChallanDate.Focus();
                  <%-- document.getElementById('<%= txt_SlChallanNo.ClientID %>').disabled = true;--%>

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

                //gridLookup.SetText('');
                //clookup_Project.gridView.Refresh();

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

        function PopulateGSTCSTVAT(e) {
            var key = cddl_AmountAre.GetValue();
            if (key == 1) {
                grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                // cddlVatGstCst.PerformCallback('1');
                cddlVatGstCst.SetSelectedIndex(0);
            }
            else if (key == 2) {
                grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');
                cddlVatGstCst.Focus();
            }
            else if (key == 3) {
                grid.GetEditor('TaxAmount').SetEnabled(false);
                cddlVatGstCst.SetEnabled(false);
                // cddlVatGstCst.PerformCallback('3');
                cddlVatGstCst.SetSelectedIndex(0);

            }

        }

        function PopulateLoadGSTCSTVAT() {
            cddlVatGstCst.SetEnabled(false);
        }



        function showQuotationDocument() {
            var URL = "Contact_Document.aspx?requesttype=" + Quotation + "";
            window.location.href = URL;
        }


        // Popup Section

        function ShowCustom() {

            cPopup_wareHouse.Show();


        }

        // Popup Section End

    </script>

    <%--Sudip--%>
    <script>
        var currentEditableVisibleIndex;
        var preventEndEditOnLostFocus = false;
        var lastProductID;
        var setValueFlag;
        var QuantityResultant = '';
        //function ProductsCombo_SelectedIndexChanged(s, e) {
        //    
        //    var tbDescription = grid.GetEditor("Description");
        //    var tbUOM = grid.GetEditor("UOM");
        //    var tbStkUOM = grid.GetEditor("StockUOM");
        //    var tbSalePrice = grid.GetEditor("SalePrice");
        //    var tbStockQuantity = grid.GetEditor("StockQuantity");

        //    var ProductID = s.GetValue();
        //    var SpliteDetails = ProductID.split("||@||");
        //    var strProductID = SpliteDetails[0];
        //    var strDescription = SpliteDetails[1];
        //    var strUOM = SpliteDetails[2];
        //    var strStkUOM = SpliteDetails[4];
        //    var strSalePrice = SpliteDetails[6];

        //    tbDescription.SetValue(strDescription);
        //    tbUOM.SetValue(strUOM);
        //    tbStkUOM.SetValue(strStkUOM);
        //    tbSalePrice.SetValue(strSalePrice);
        //    tbStockQuantity.SetValue("0");
        //}

        function ProductsCombo_SelectedIndexChanged(s, e) {
            pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            //var tbStkUOM = grid.GetEditor("StockUOM");
            //var tbStockQuantity = grid.GetEditor("StockQuantity");

            var ProductID = s.GetValue();
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];



            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tbSalePrice.SetValue(strSalePrice);
            divPacking.style.display = "none";
            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            cacpAvailableStock.PerformCallback(strProductID);
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");
        }

        function cgridProducts_EndCallBack(s, e) {

            page.tabs[1].SetEnabled(false);
        }


        function OnEndCallback(s, e) {
            //debugger;
            //$('#MandatoryEwayBillNo').attr('style', 'display:none');
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
            else if (grid.cpProductZeroStock == "ZeroStock") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Insufficient Avaialble Stock.Cannot proceed');
                grid.cpProductZeroStock = null;
            }
            else if (grid.cpProductZeroStock == "InvoiceTagRequired") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Sale challan with sale invoice tagging is mandatory.');
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

                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please try again later.');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "EmptyProject") {

                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please Select Project.');
                grid.cpSaveSuccessOrFail = null;
            }            
            else if (grid.cpSaveSuccessOrFail == "DocumentNoBlank") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please enter valid document no.');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "PartialInvoice") {

                grid.batchEditApi.StartEdit(0, 2);
                var SaleChallanNo = grid.cpSaleChallanNo;

                jAlert('Tagged Invoice is already used with' + ' ' + SaleChallanNo);
                grid.cpSaveSuccessOrFail = null;
                grid.cpSaleChallanNo = null;
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
            else if (grid.cpSaveSuccessOrFail == "udfNotSaved") {
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('UDF is set as Mandatory. Please enter values.', 'Alert', function () { OpenUdf(); });
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                OnAddNewClick();
                //grid.cpSaveSuccessOrFail = null;

                //grid.AddNewRow();

                //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                //var i;
                //var cnt = 1;
                //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                //    var tbQuotation = grid.GetEditor("SrlNo");
                //    tbQuotation.SetValue(cnt);


                //    cnt++;
                //}

                var SrlNo = grid.cpProductSrlIDCheck;
                var msg = "Qty is entered for product [" + SrlNo + "] but Stock Details not updated.Cannot proceed.";
                jAlert(msg);
                grid.cpSaveSuccessOrFail = null;
                grid.cpProductSrlIDCheck = null;
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouseQty") {
                //debugger;
                //OnAddNewClick();
                grid.AddNewRow();

                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var i;
                var cnt = 1;
                for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                    var tbQuotation = grid.GetEditor("SrlNo");
                    tbQuotation.SetValue(cnt);


                    cnt++;
                }
                //MoreThanStock
                //grid.batchEditApi.StartEdit(0, 1);
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
                //var Quote_Number = grid.cpQuotationNo;
                //var Quote_Msg = "Sales Quotation No. " + Quote_Number + " generated.";
                var SalesOrder_Number = grid.cpSalesOrderNo;
                //$("#hfDocId").val(SalesOrder_Number);
                //$("#hfDocType").val("SC");
                //var DirectMsg = grid.cpSalesOrderExitOnCustomerDelivery;
                var Order_Msg = "Sales Challan No. " + SalesOrder_Number + " saved.";
                if (value == "E") {
                    //window.location.assign("SalesChallan.aspx");
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
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SalesChallan~D&modulename=ODSDChallan&id=" + DocumentNo, '_blank');
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
                            jAlert(Order_Msg, 'Alert Dialog: [SalesChallan]', function (r) {
                                if (r == true) {
                                    grid.cpSalesOrderNo = null;
                                    window.location.assign("SalesChallanEntityList.aspx");
                                }
                            });
                        }

                        //jAlert(Order_Msg);




                    }
                    else {
                        window.location.assign("SalesChallanEntityList.aspx");
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
                    // window.location.assign("SalesOrderAdd.aspx?key=ADD");


                    if (SalesOrder_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [SalesChallan]', function (r) {
                            //jAlert(Order_Msg);
                            grid.cpSalesOrderNo = null;
                            if (r == true) {
                                window.location.assign("SalesChallanAdd.aspx?key=ADD");
                            }
                        });
                        //jConfirm(Order_Msg, 'Confirmation Dialog', function (r) {
                        //    if (r == true) {
                        //        window.location.assign("SalesOrderAdd.aspx?key=ADD");
                        //    }
                        //    else
                        //    { window.location.assign("SalesOrderAdd.aspx?key=ADD"); }
                        //});

                    }
                    else {
                        window.location.assign("SalesChallanAdd.aspx?key=ADD");
                    }
                }
                else {

                    var pageStatus = document.getElementById('hdnPageStatus').value;
                    if (pageStatus == "first") {
                        OnAddNewClick();
                        grid.batchEditApi.EndEdit();
                        ctxtCustName.Focus();

                     <%--   $('#ddl_numberingScheme').focus();	
                        document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();--%>
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
    var hddnPermission = $("#<%=hddnPermissionString.ClientID%>").val();//Subhabrata
            if (gridSalesOrderLookup.GetValue() != null) {
                if (hddnPermission == "0") {
                    grid.GetEditor('ProductName').SetEnabled(false);
                    grid.GetEditor('Description').SetEnabled(false);
                    grid.GetEditor('Order_Num').SetEnabled(false);
                    grid.GetEditor('SalePrice').SetEnabled(false);//Added on 07-06-2017
                    grid.GetEditor('Discount').SetEnabled(false);//Added on 07-06-2017
                    grid.GetEditor('TaxAmount').SetEnabled(false);//Added on 07-06-2017
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
            //Rev Rajdip For Running Total
            if (grid.cpRunningTotal != null || grid.cpRunningTotal != "") {
                if (grid.cpRunningTotal != undefined) {
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

                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                    if (key != null && key != '') {
                        grid.GetEditor("Quantity").SetEnabled(false);
                    }
                }
            }
            //End Rev Rajdip For Running Total
            cProductsPopup.Hide();
            return false;
        }

        function SetArrForUOM() {

            //Rev Subhra 16-09-2019
            issavePacking = 1;
            //End of Rev Subhra 16-09-2019

            if (aarr.length == 0) {
                for (var i = -500; i < 500; i++) {
                    if (grid.GetRow(i) != null) {

                        var ProductID = (grid.batchEditApi.GetCellValue(i, 'ProductID') != null) ? grid.batchEditApi.GetCellValue(i, 'ProductID') : "0";
                        var ComponentNumber = (grid.batchEditApi.GetCellValue(i, 'Order_Num') != null) ? grid.batchEditApi.GetCellValue(i, 'Order_Num') : "0";
                        var rdl_SaleInvoice = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                        if (ProductID != "0") {
                            var gridPackingQty = '';
                            var IsInventory = '';
                            var actionQry = '';
                            if (ComponentNumber != "0" && ComponentNumber != "") {

                                if (rdl_SaleInvoice == 'SO') {
                                    actionQry = 'SalesChallanPackingQtyOrder';
                                }
                                if (rdl_SaleInvoice == 'SI') {
                                    actionQry = 'SalesChallanPackingQtyInvoice';
                                }
                            }
                            else {
                                actionQry = 'SalesChallanPackingQtyProductId';

                            }
                            var QuotationNum = "0";
                            if (grid.GetEditor('Quotation_Num') != null) {
                                QuotationNum = (grid.GetEditor('Quotation_Num').GetText() != null) ? grid.GetEditor('Quotation_Num').GetText() : "0";
                            }
                            //if ($("#hdAddOrEdit").val() == "Edit") {
                            if ($("#hdnPageStatus").val() == "update") {
                                var SpliteDetails = ProductID.split("||@||");
                                var strProductID = SpliteDetails[0];
                                var orderid = grid.GetRowKey(i);
                                var slnoget = grid.batchEditApi.GetCellValue(i, 'SrlNo');
                                var Quantity = grid.batchEditApi.GetCellValue(i, 'Quantity');
                                //Rev Subhra 16-09-2019
                                var challanid = document.getElementById('hfDocId').value;
                                orderid = challanid;
                                if (actionQry = 'SalesChallanPackingQtyProductId') {
                                    ComponentNumber = strProductID;
                                }
                                //End of Rev Subhra 16-09-2019

                                $.ajax({
                                    type: "POST",
                                    url: "Services/Master.asmx/GetMultiUOMDetails",
                                    //Rev Subhra 16-09-2019
                                    //data: JSON.stringify({ orderid: strProductID, action: actionQry, module: 'SalesChallan', strKey: ComponentNumber }),
                                    data: JSON.stringify({ orderid: orderid, action: actionQry, module: 'SalesChallan', strKey: ComponentNumber }),
                                    //End of Rev Subhra 16-09-2019
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    async: false,
                                    success: function (msg) {

                                        gridPackingQty = msg.d;

                                        if (msg.d != "") {
                                            var packing = SpliteDetails[20];
                                            var PackingUom = SpliteDetails[25];
                                            var PackingSelectUom = SpliteDetails[24];
                                            var arrobj = {};
                                            arrobj.productid = strProductID;
                                            arrobj.slno = slnoget;
                                            arrobj.Quantity = Quantity;
                                            arrobj.packing = gridPackingQty;
                                            arrobj.PackingUom = PackingUom;
                                            arrobj.PackingSelectUom = PackingSelectUom;

                                            aarr.push(arrobj);
                                            //alert();
                                        }
                                    }
                                });
                            }
                        }
                    }
                }

            }
        }

        function Save_ButtonClick() {

            var flag = true;
            grid.AddNewRow();
            LoadingPanel.Show();
            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
            var OrderNo = ctxt_SlChallanNo.GetText();
            var slsdate = cPLSalesChallanDate.GetValue();
            var qudate = cPLQADate.GetText();
            var customerid = GetObjectID('hdnCustomerId').value;
            var salesorderDate = new Date(slsdate);
            var quotationDate = "";
            var DoctypeVal = $("[id$='rdl_SaleInvoice']").find(":checked").val(); //$("#hdnTagType").val();
            //$("[id$='rdl_SaleInvoice']").find(":checked").val();

            var ProjectCode = clookup_Project.GetText();
            if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
                LoadingPanel.Hide();
                jAlert("Please Select Project.");
                flag = false;
            }

            if ((gridSalesOrderLookup.GetValue() == "" || gridSalesOrderLookup.GetValue() == undefined || gridSalesOrderLookup.GetValue() == null) && $("#hdnInvoiceTag").val() == "1") {
                LoadingPanel.Hide();
                jAlert("Sale challan with sale invoice tagging is mandatory.");
                flag = false;
            }



            if (qudate != null && qudate != '') {
                var qd = qudate.split('-');
                LoadingPanel.Hide();
                quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

            }
            if (customerid == null || customerid == "") {
                flag = false;
                LoadingPanel.Hide();
                $('#MandatorysCustomer').attr('style', 'display:block');
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }



            if (slsdate == null || slsdate == "") {
                flag = false;
                LoadingPanel.Hide();
                $('#MandatorySlDate').attr('style', 'display:block');
            }
            else {
                $('#MandatorySlDate').attr('style', 'display:none');
                if (qudate != null && qudate != '') {
                    var qd = qudate.split('-');
                    quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

                    if (quotationDate > salesorderDate) {

                        flag = false;
                        $('#MandatoryEgSDate').attr('style', 'display:block');
                    }
                    else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
                }

            }

            if (OrderNo == "") {
                flag = false;
                LoadingPanel.Hide();
                $('#MandatorySlOrderNo').attr('style', 'display:block');
            }
            else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }

            if (cddl_PosGst.GetText() == "") {
                flag = false;
                LoadingPanel.Hide();
                jAlert("Place of Supply Not Empty.");
            }


            if (flag) {

                SetArrForUOM(); //For UOM Conversion Surojit
                SaveSendUOM('SC');

                if (grid.GetVisibleRowsOnPage() > 0) {

                    if (issavePacking == 1) {



                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "SalesChallanAdd.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    //var customerval = gridLookup.GetValue();
                                    var customerval = $("#<%=hdnCustomerId.ClientID%>").val();
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                    grid.batchEditApi.EndEdit();
                                    //grid.UpdateEdit();
                                    cacbpCrpUdf.PerformCallback();
                                    $('#<%=hdnRefreshType.ClientID %>').val('N');
                                }
                            });
                        }
                        else {



                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //var customerval = gridLookup.GetValue();
                            var customerval = $("#<%=hdnCustomerId.ClientID%>").val();
                            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                            $('#<%=hdnRefreshType.ClientID %>').val('N');
                        }
                    }
                    else {

                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "SalesChallanAdd.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    //var customerval = gridLookup.GetValue();
                                    var customerval = $("#<%=hdnCustomerId.ClientID%>").val();
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                    grid.batchEditApi.EndEdit();
                                    //grid.UpdateEdit();
                                    cacbpCrpUdf.PerformCallback();
                                    $('#<%=hdnRefreshType.ClientID %>').val('N');
                                }
                            });
                        }
                        else {

                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //var customerval = gridLookup.GetValue();
                            var customerval = $("#<%=hdnCustomerId.ClientID%>").val();
                            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                            $('#<%=hdnRefreshType.ClientID %>').val('N');
                        }
                    }
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('You must enter proper details before save');
                }
            }
            // return flag;
        }


        function SaveExit_ButtonClick() {
            //debugger;
            var flag = true;
            grid.AddNewRow();
            LoadingPanel.Show();
            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
            $("#<%=hddnSaveOrExitButton.ClientID%>").val('Save_Exit');

            var OrderNo = ctxt_SlChallanNo.GetText();
            var slsdate = cPLSalesChallanDate.GetValue();
            var qudate = cPLQADate.GetText();
            var customerid = GetObjectID('hdnCustomerId').value;
            var salesorderDate = new Date(slsdate);
            var quotationDate = "";

            var DoctypeVal = $("[id$='rdl_SaleInvoice']").find(":checked").val();
            var ProjectCode = clookup_Project.GetText();
            if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
                LoadingPanel.Hide();
                jAlert("Please Select Project.");
                flag = false;
            }

            // if ((DoctypeVal == "" || DoctypeVal == undefined) && $("#hdnInvoiceTag").val() == "1") {
            if ((gridSalesOrderLookup.GetValue() == "" || gridSalesOrderLookup.GetValue() == undefined || gridSalesOrderLookup.GetValue() == null) && $("#hdnInvoiceTag").val() == "1") {
                LoadingPanel.Hide();
                jAlert("Sale challan with sale invoice tagging is mandatory.");
                flag = false;
            }


            if (qudate != null && qudate != '') {
                //LoadingPanel.Hide();
                var qd = qudate.split('-');
                quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

            }
            if (customerid == null || customerid == "") {
                flag = false;
                LoadingPanel.Hide();
                $('#MandatorysCustomer').attr('style', 'display:block');
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }



            if (slsdate == null || slsdate == "") {
                flag = false;
                LoadingPanel.Hide();
                $('#MandatorySlDate').attr('style', 'display:block');
            }
            else {
                $('#MandatorySlDate').attr('style', 'display:none');
                if (qudate != null && qudate != '') {
                    var qd = qudate.split('-');
                    quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

                    if (quotationDate > salesorderDate) {

                        flag = false;
                        LoadingPanel.Hide();
                        $('#MandatoryEgSDate').attr('style', 'display:block');
                    }
                    else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
                }

            }

            if (OrderNo == "") {
                flag = false;
                LoadingPanel.Hide();
                $('#MandatorySlOrderNo').attr('style', 'display:block');
            }
            else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }


            if (cddl_PosGst.GetText() == "") {
                flag = false;
                LoadingPanel.Hide();
                jAlert("Place of Supply Not Empty.");
            }


            if (flag) {

                SetArrForUOM(); //For UOM Conversion Surojit
                SaveSendUOM('SC');
                if (grid.GetVisibleRowsOnPage() > 0) {
                    if (issavePacking == 1) {


                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "SalesChallanAdd.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    //var customerval = gridLookup.GetValue();
                                    var customerval = $("#<%=hdnCustomerId.ClientID%>").val();
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                                    //$('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                    grid.batchEditApi.EndEdit();
                                    //grid.UpdateEdit();
                                    cacbpCrpUdf.PerformCallback();
                                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                                }
                            });
                        }
                        else {




                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //var customerval = gridLookup.GetValue();
                            var customerval = $("#<%=hdnCustomerId.ClientID%>").val();
                            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                            //$('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                            $('#<%=hdnRefreshType.ClientID %>').val('E');
                        }
                    }
                    else {

                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "SalesChallanAdd.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {

                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    //var customerval = gridLookup.GetValue();
                                    var customerval = $("#<%=hdnCustomerId.ClientID%>").val();
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                                    //$('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                    grid.batchEditApi.EndEdit();
                                    //grid.UpdateEdit();
                                    cacbpCrpUdf.PerformCallback();
                                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                                }
                            });
                        }
                        else {


                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //var customerval = gridLookup.GetValue();
                            var customerval = $("#<%=hdnCustomerId.ClientID%>").val();
                            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                            //$('#<%=hdfLookupCustomer.ClientID %>').val(customerval);


                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            cacbpCrpUdf.PerformCallback();
                            $('#<%=hdnRefreshType.ClientID %>').val('E');
                        }
                    }
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('You must enter proper details before save');
                }
            }
            // return flag;
        }


        var Uomlength = 0;
        function UomLenthCalculation() {
            grid.batchEditApi.StartEdit(globalRowIndex);
            var SLNo = "";
            var val = 0;
            var detailsid = grid.GetEditor('DetailsId').GetValue();
            if (detailsid != null && detailsid != "") {
                SLNo = detailsid;
                val = 1;
            }
            else {
                SLNo = grid.GetEditor('SrlNo').GetValue();
            }
            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/GetQuantityfromSL",
                data: JSON.stringify({ SLNo: SLNo, val: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    Uomlength = msg.d;

                }
            });

        }


        function QuantityLostFocusForMultoUOM(s, e) {
            if (($("#hddnMultiUOMSelection").val() == "1")) {

                //setTimeout(function () {
                UomLenthCalculation();
                //  }, 200)

                grid.batchEditApi.StartEdit(globalRowIndex);
                var SLNo = grid.GetEditor('SrlNo').GetValue();

                if (Uomlength > 0) {
                    var qnty = $("#UOMQuantity").val();
                    var QValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0.0000";
                    if (QValue != "0.0000" && QValue != qnty) {
                        jConfirm('Qunatity Change Will Clear Multiple UOM Details , Confirm ?', 'Confirmation Dialog', function (r) {
                            if (r == true) {
                                grid.batchEditApi.StartEdit(globalRowIndex);
                                var tbqty = grid.GetEditor('Quantity');
                                //tbqty.SetValue(Quantity);

                                var detailsid = grid.GetEditor('DetailsId').GetValue();
                                if (detailsid != null && detailsid != "") {
                                    cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo + '~' + detailsid);
                                }
                                else {
                                    cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo + '~' + detailsid);
                                }
                                <%--Use for set focus on UOM after press ok on UOM--%>
                                setTimeout(function () {
                                    grid.batchEditApi.StartEdit(globalRowIndex, 6);
                                }, 600)
                            }
                            else {
                                grid.batchEditApi.StartEdit(globalRowIndex);
                                grid.GetEditor('Quantity').SetValue(qnty);
                                setTimeout(function () {
                                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                                }, 200);
                            }


                        });

                    }
                    else {
                        grid.batchEditApi.StartEdit(globalRowIndex);
                        grid.GetEditor('Quantity').SetValue(qnty);
                        <%--Use for set focus on UOM after press ok on UOM--%>
                        setTimeout(function () {
                            grid.batchEditApi.StartEdit(globalRowIndex, 6);
                        }, 600)
                        <%--Use for set focus on UOM after press ok on UOM--%>

                    }
                }

            }

            //Rev Rajdip           
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            var finalNetAmount = parseFloat(tbTotalAmount.GetValue());
            var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
            cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
            SetTotalTaxableAmount(s, e);
            SetInvoiceLebelValue();
            //End Rev rajdip
        }

        function QuantityTextChange(s, e) {
            //debugger;
            pageheaderContent.style.display = "block";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetText();

            //Subhabrata on 03-03-2017
            var Id = grid.GetEditor('Quotation_No').GetValue();
            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/CheckBalQuantity",
                data: JSON.stringify({ Id: Id, ProductID: ProductID.split('||@||')[0] }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {


                    var ObjData = msg.d;
                    if (ObjData.length > 0) {
                        var balQty = ObjData[0].split('|')[0];
                        if ((QuantityValue * 1) > (balQty * 1)) {
                            var OrdeMsg = 'Balance Quantity of selected Product from tagged document is <' + ObjData + '>.Cannot enter quantity more than balance quantity.';
                            //jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {

                            //});
                            //var tbQuantity = grid.GetEditor("Quantity");
                            //tbQuantity.SetValue(balQty);
                            //return false;

                        }

                    }
                }

            });

            //End

            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            //var strRate = (ctxt_Rate.GetValue() != null) ? ctxt_Rate.GetValue() : "1";
            var strRate = "1";
            var strStkUOM = SpliteDetails[4];
            //var strSalePrice = '';
            //if (gridSalesOrderLookup.GetValue() != null) {
            //    strSalePrice = grid.GetEditor('SalePrice').GetValue();
            //}
            //else {

            //    strSalePrice = SpliteDetails[6];
            //}

            var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }


            if (strRate == 0) {
                strRate = 1;
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Amount = QuantityValue * strFactor * strRate * (strSalePrice * 1);

            $('#<%= lblStkQty.ClientID %>').text(StockQuantity);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);

            //var tbStockQuantity = grid.GetEditor("StockQuantity");
            //tbStockQuantity.SetValue(StockQuantity);

            //Subhabrata added on 14-03-2017
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            //var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
            var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

            if (IsPackingActive == "Y") {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                console.log('jhsdfafa');
                //divPacking.style.display = "block";
                $('#divPacking').css({ 'display': 'block' });
            } else {
                divPacking.style.display = "none";
            }//END

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);

            var TotaAmountRes = '';
            TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount + (TotaAmountRes * 1));
            //tbTotalAmount.SetValue(Amount);

            DiscountTextChange(s, e);

        }
        var globalNetAmount = 0;
        //Rev Rajdip For Running Parameters
        function SetTotalTaxableAmount(inx, vindex) {

            var count = grid.GetVisibleRowsOnPage();
            var totalAmount = 0;
            var netAmount = 0;
            var totaltxAmount = 0;
            var totalQuantity = 0;
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
                        //totalAmount = totalAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "Amount"), 2);
                        //totaltxAmount = totaltxAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "TaxAmount"), 2);


                        //if (globalRowIndex == i) {
                        //    if ($(grid.GetRow(i).children[10].children[1].children[0].children[0].children[0].children[0].children[0]).val().replace('{', '').replace("}", "").split(':')[1].split(',')[0].replace(/&quot;/g, '\\"').replace(/['"]+/g, '').replace('\\', '').split('\\')[0].trim() != "")
                        //        totalAmount = totalAmount + DecimalRoundoff($(grid.GetRow(i).children[10].children[1].children[0].children[0].children[0].children[0].children[0]).val().replace('{', '').replace("}", "").split(':')[1].split(',')[0].replace(/&quot;/g, '\\"').replace(/['"]+/g, '').replace('\\', '').split('\\')[0], 2);
                        //    if ($(grid.GetRow(i).children[11].children[1].children[0].children[0].children[0].children[0].children[0]).val().trim() != "")
                        //        totaltxAmount = totaltxAmount + DecimalRoundoff($(grid.GetRow(i).children[11].children[1].children[0].children[0].children[0].children[0].children[0]).val(), 2);
                        //}
                        //else {
                        //    if (grid.GetRow(i).children[10].children[0].innerText.trim() != "")
                        //        totalAmount = totalAmount + DecimalRoundoff(grid.GetRow(i).children[10].children[0].innerText, 2);
                        //    if (grid.GetRow(i).children[11].children[0].innerText.trim() != "")
                        //        totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetRow(i).children[11].children[0].innerText, 2);

                        //}
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
                            grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("TaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))

                        }
                        else {
                            grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2))
                        }
                        //totalAmount = totalAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "Amount"), 2);
                        //totaltxAmount = totaltxAmount + DecimalRoundoff(grid.batchEditApi.GetCellValue(i, "TaxAmount"), 2);



                        //if (globalRowIndex == i) {
                        //    if ($(grid.GetRow(i).children[10].children[1].children[0].children[0].children[0].children[0].children[0]).val().replace('{', '').replace("}", "").split(':')[1].split(',')[0].replace(/&quot;/g, '\\"').replace(/['"]+/g, '').replace('\\', '').split('\\')[0].trim() != "")
                        //        totalAmount = totalAmount + DecimalRoundoff($(grid.GetRow(i).children[10].children[1].children[0].children[0].children[0].children[0].children[0]).val().replace('{', '').replace("}", "").split(':')[1].split(',')[0].replace(/&quot;/g, '\\"').replace(/['"]+/g, '').replace('\\', '').split('\\')[0], 2);
                        //    if ($(grid.GetRow(i).children[11].children[1].children[0].children[0].children[0].children[0].children[0]).val().trim() != "")
                        //        totaltxAmount = totaltxAmount + DecimalRoundoff($(grid.GetRow(i).children[11].children[1].children[0].children[0].children[0].children[0].children[0]).val(), 2);
                        //}
                        //else {
                        //    if (grid.GetRow(i).children[10].children[0].innerText.trim() != "")
                        //        totalAmount = totalAmount + DecimalRoundoff(grid.GetRow(i).children[10].children[0].innerText, 2);
                        //    if (grid.GetRow(i).children[11].children[0].innerText.trim() != "")
                        //        totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetRow(i).children[11].children[0].innerText, 2);

                        //}
                    }
                }
            }

            //globalRowIndex = inx;


            grid.batchEditApi.EndEdit()
            netAmount = netAmount + parseFloat($("#bnrOtherChargesvalue").text());
            cbnrLblTaxableAmtval.SetText(DecimalRoundoff(totalAmount, 2));
            cbnrLblTaxAmtval.SetText(DecimalRoundoff(totaltxAmount, 2));
            cbnrLblTotalQty.SetText(DecimalRoundoff(totalQuantity, 4));
            var totamt = totalAmount + totaltxAmount;
            cbnrlblAmountWithTaxValue.SetText(totamt);
            cbnrLblInvValue.SetText(totamt);
            globalRowIndex = vindex;
        }
        function SetInvoiceLebelValue() {

            var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
            //if (document.getElementById('HdPosType').value == 'Crd') {
            //    if (invValue < 0) {
            //        var newAdvAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
            //        cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(newAdvAmount) * 100) / 100).toFixed(2));
            //    }
            //}

            //if (document.getElementById('HdPosType').value == 'Fin') {
            //    if (invValue < 0) {
            //        var newAdvAmountfin = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue());
            //        cbnrLblLessAdvanceValue.SetValue(parseFloat(Math.round(Math.abs(ctxtdownPayment.GetValue()) * 100) / 100).toFixed(2));
            //    }
            //}



            //if (document.getElementById('HdPosType').value == 'Crd')
            //    invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());
            //else if (document.getElementById('HdPosType').value == 'Fin')
            //    invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(cbnrLblLessOldMainVal.GetValue()) + parseFloat(cbnrOtherChargesvalue.GetValue());
            //alert(invValue)

            cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));


            // SetRunningBalance();

        }
        //End Rev Rajdip




        //function DiscountTextChange(s, e) {
        //    var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
        //    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

        //    var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        //    var tbAmount = grid.GetEditor("Amount");
        //    tbAmount.SetValue(amountAfterDiscount);

        //    var TotaAmountRes = '';
        //    TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

        //    var tbTotalAmount = grid.GetEditor("TotalAmount");
        //    //tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));
        //    tbTotalAmount.SetValue(amountAfterDiscount);

        //    //Debj
        //}

        function AddBatchNew(s, e) {

            grid.batchEditApi.EndEdit();
            var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
            if (keyCode === 13) {
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var i;
                var cnt = 2;

                grid.AddNewRow();
                if (noofvisiblerows == "0") {
                    grid.AddNewRow();
                }
                grid.SetFocusedRowIndex();

                for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                    cnt++;
                }

                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(cnt);
            }
        }
        function OnAddNewClick() {

            if (gridSalesOrderLookup.GetValue() == null) {
                grid.AddNewRow();

                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var i;
                var cnt = 1;
                for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                    var tbQuotation = grid.GetEditor("SrlNo");
                    tbQuotation.SetValue(cnt);


                    cnt++;

                    /// Mantis Issue 24428 
                    $("#UOMQuantity").val(0);
                    Uomlength = 0;
                    // End of Mantis Issue 24428 
                }
            }
            else {
                OrderNumberChanged();
            }

        }


        var Warehouseindex;
        function OnCustomButtonClick(s, e) {



            function SetDataSourceOnComboBox(ControlObject, Source) {
                ControlObject.ClearItems();
                for (var count = 0; count < Source.length; count++) {
                    ControlObject.AddItem(Source[count].UOM_Name, Source[count].UOM_Id);
                    //ControlObject.AddItem(Source[count].UOM_Id, Source[count].UOM_Name);
                }
                ControlObject.SetSelectedIndex(0);
            }






            //End Rev Rajdip

            if (e.buttonID == 'CustomDelete') {
                grid.batchEditApi.EndEdit();

                //


                var totalNetAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'TotalAmount');
                var oldAmountWithTaxValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue());
                var AfterdeleteoldAmountWithTaxValue = oldAmountWithTaxValue - parseFloat(totalNetAmount);
                cbnrlblAmountWithTaxValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
                //cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));

                var RowQuantity = grid.batchEditApi.GetCellValue(e.visibleIndex, 'Quantity');
                var totalquantity = parseFloat(cbnrLblTotalQty.GetValue());
                var updatedquantity = (parseFloat(totalquantity) - parseFloat(RowQuantity));
                //cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));

                var rowTaxAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'TaxAmount');
                var totaltaxamt = parseFloat(cbnrLblTaxAmtval.GetValue());
                var updatedtaxamt = parseFloat(totaltaxamt) - parseFloat(rowTaxAmount);
                //cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));

                var rowAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'Amount');
                var TotalAmt = parseFloat(cbnrLblTaxableAmtval.GetValue());
                var updatedAmt = parseFloat(TotalAmt) - parseFloat(rowAmount);
                //cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));

                $('#<%=hdnRefreshType.ClientID %>').val('');
                var noofvisiblerows = grid.GetVisibleRowsOnPage();
                if (gridSalesOrderLookup.GetValue() != null) {
                    //jAlert();
                    jAlert('Cannot Delete using this button as the GRN is created from other document.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) {

                    });
                }

                if (noofvisiblerows != "1" && gridSalesOrderLookup.GetValue() == null) {
                    grid.DeleteRow(e.visibleIndex);

                    cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
                    cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));
                    cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));
                    cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));

                    $('#<%=hdfIsDelete.ClientID %>').val('D');
                    grid.UpdateEdit();
                    grid.PerformCallback('Display');
                    grid.batchEditApi.StartEdit(-1, 2);
                    grid.batchEditApi.StartEdit(0, 2);
                    $('#<%=hdnPageStatus.ClientID %>').val('delete');
                }
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
                    ccallback_InlineRemarks.PerformCallback('DisplayRemarks' + '~' + SrlNo + '~' + '0');

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


            else if (e.buttonID == 'CustomMultiUOM') {

                var index = e.visibleIndex;
                grid.batchEditApi.StartEdit(e.visibleIndex);
                var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                var ProductID = Productdetails.split("||@||")[0];
                var UOMName = grid.GetEditor("UOM").GetValue();
                var quantity = grid.GetEditor("Quantity").GetValue();
                var DetailsId = grid.GetEditor('DetailsId').GetValue();
                var StockUOM = Productdetails.split("||@||")[5];
                ///rev bapi
                hdProductID.value = ProductID;
                //End Rev Bapi
                if (StockUOM == "") {
                    StockUOM = "0";
                }

                //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
                cAltUOMQuantity.SetValue("0.0000");
                ///rev bapi
                //  if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
                if ((ProductID != "") && (UOMName != "")) {
                    //End Rev Bapi
                    if (StockUOM == "0") {
                        jAlert("Main Unit Not Defined.");
                    }
                    else {
                        if ($("#hddnMultiUOMSelection").val() == "1") {
                            ccmbUOM.SetEnabled(false);
                            var index = e.visibleIndex;
                            grid.batchEditApi.StartEdit(e.visibleIndex, 8);
                            //grid.batchEditApi.StartEdit(globalRowIndex);
                            var Qnty = grid.GetEditor("Quantity").GetValue();
                            var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                            var UomId = grid.GetEditor('ProductID').GetText().split("||@||")[3];
                            ccmbUOM.SetValue(UomId);
                            // Mantis Issue 24428
                            //$("#UOMQuantity").val(Qnty);
                            $("#UOMQuantity").val(0);
                            ccmbBaseRate.SetValue(0)
                            cAltUOMQuantity.SetValue(0)
                            ccmbAltRate.SetValue(0)
                            ccmbSecondUOM.SetValue("")
                            // End of Mantis Issue 24428
                            cPopup_MultiUOM.Show();
                            cgrid_MultiUOM.cpDuplicateAltUOM = "";
                            // if ($("#hdnPageStatus").val() != "update") {
                            AutoPopulateMultiUOM();
                            //}
                            //chinmoy change start
                            cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo + '~' + DetailsId);
                            //cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + ProductID);
                        }     //End
                    }
                }
                else {
                    return;
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
                var IsUomActivate = $("#<%=hdnShowUOMConversionInEntry.ClientID%>").val();
                // var a=  $("#<%=hdnConvertionOverideVisible.ClientID%>").val();
                //Rev Rajdip
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strSalePrice = SpliteDetails[6];
                var strMultiplier = SpliteDetails[7];
                var IsPackingActive = SpliteDetails[10];
                var Packing_Factor = SpliteDetails[11];
                var Packing_UOM = SpliteDetails[12];
                var strProductShortCode = SpliteDetails[14];
                var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
                IsInventory = '';
                strProductName = strDescription;

                var isOverideConvertion = SpliteDetails[26];
                var packing_saleUOM = SpliteDetails[25];
                var sProduct_SaleUom = SpliteDetails[24];
                var sProduct_quantity = SpliteDetails[22];
                var packing_quantity = SpliteDetails[20];
                //End Rev Rajdip
                if (IsUomActivate == 1) {
                    $("#divuom").show();
                }
                else {
                    $("#divuom").hide();
                }

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
                    //$.ajax({
                    //    type: "POST",
                    //    url: "SalesChallanAdd.aspx/GetAvaiableStockCheckStockOut",
                    //    data: JSON.stringify({ ProductID: ProductID.split("||@||")[0], FinYear: LastFinYear, Company: LastCompany, Branch: Branch, Date: cPLSalesChallanDate.date.format('yyyy-MM-dd') }),
                    //    contentType: "application/json; charset=utf-8",
                    //    dataType: "json",
                    //    async: false,
                    //    success: function (msg) {
                    //        data = msg.d;
                    //        if (data == 'Y') {
                    //            IsExits = false;
                    //        }
                    //    }
                    //});//End
                }
                //Rev Rajdip 
                //if (ProductID != "" && parseFloat(QuantityValue) != 0) {

                var IsInvoiceTagged = gridSalesOrderLookup.GetText();
                //WHEN ALTQTY SETTING ON AND IS NOT TAGGED
                if (ProductID != "" && IsUomActivate == 1 && IsInvoiceTagged == "") {

                    //var SpliteDetails = ProductID.split("||@||");
                    //var strProductID = SpliteDetails[0];
                    //var strDescription = SpliteDetails[1];
                    //var strUOM = SpliteDetails[2];
                    //var strStkUOM = SpliteDetails[4];
                    //var strMultiplier = SpliteDetails[7];
                    //var strProductName = strDescription;

                    //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    var StkQuantityValue = QuantityValue * strMultiplier;
                    var Ptype = SpliteDetails[14];
                    $('#<%=hdfProductType.ClientID %>').val(Ptype);

                    //Subhabrata Check whether it is in FIFo or not on 23-06-2016
                    $.ajax({
                        type: "POST",
                        url: "SalesChallanAdd.aspx/GetConfigSettingRights",
                        data: JSON.stringify({ VariableName: 'IsFIFOExistsInOutModule' }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            //debugger;
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
                    //End



                    document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
                    document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
                    document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
                    document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
                    document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;

                    $('#<%=hdfProductID.ClientID %>').val(strProductID);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);

                    //Rev Rajdip

                    //var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    var objectToPass = {}
                    var product = $("#hdfProductID").val();
                    objectToPass.ProductID = hdfProductID.value;
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
                    // End Rev Rajdip
                    //cacpAvailableStock.PerformCallback(strProductID);

                    SecondUOMProductId = strProductID;
                    //Rev Rajdip
                    var htmlfactor = "";
                    htmlfactor = parseFloat(sProduct_quantity).toFixed(4) + " ";
                    htmlfactor = htmlfactor + " " + strUOM + " = " + parseFloat(packing_quantity).toFixed(4) + " " + Packing_UOM;
                    $('#lbluomfactor1').text(htmlfactor);
                    //End Rev Rajdip

                    if (Ptype == "W") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';

                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "B") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';

                        cCmbBatch.PerformCallback('BindBatch~' + "0");
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "S") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'block';
                        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");

                        // cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");//Subhabrata
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

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cCmbBatch.SetValue("0");
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WS") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';

                        cCmbWarehouse.PerformCallback('BindWarehouse');

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);



                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WBS") {

                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';

                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);
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
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else {
                        //div_Warehouse.style.display = 'none';
                        //div_Batch.style.display = 'none';
                        //div_Serial.style.display = 'none';
                        //div_Quantity.style.display = 'none';

                        //$.confirm({
                        //    title: 'Confirm!',
                        //    type: 'blue',
                        //    content: 'No Warehouse or Batch or Serial is actived !',

                        //    buttons: {
                        //        formSubmit: {
                        //            text: 'Ok',
                        //            btnClass: 'btn-blue',
                        //            keys: ['esc'],
                        //            action: function () {
                        //                grid.batchEditApi.StartEdit(index, 5);
                        //            }
                        //        },
                        //    },
                        //});

                        //var strconfirm = confirm("No Warehouse or Batch or Serial is actived.");
                        //if (strconfirm == true) {
                        //    grid.batchEditApi.StartEdit(index, 5);
                        //}
                        //else {
                        //    grid.batchEditApi.StartEdit(index, 5);
                        //}

                        jAlert("No Warehouse or Batch or Serial is actived.");
                    }
                }




                    //----------------------------------------------------------Rev Rajdip-----------------------------------------------------
                    //WHEN ALTQTY SETTING OFF AND IS NOT TAGGED


                else if (ProductID != "" && IsUomActivate == 0 && parseFloat(QuantityValue) != 0 && IsInvoiceTagged == "") {
                    //var SpliteDetails = ProductID.split("||@||");
                    //var strProductID = SpliteDetails[0];
                    //var strDescription = SpliteDetails[1];
                    //var strUOM = SpliteDetails[2];
                    //var strStkUOM = SpliteDetails[4];
                    //var strMultiplier = SpliteDetails[7];
                    //var strProductName = strDescription;

                    //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    var StkQuantityValue = QuantityValue * strMultiplier;
                    var Ptype = SpliteDetails[14];
                    $('#<%=hdfProductType.ClientID %>').val(Ptype);

                    //Subhabrata Check whether it is in FIFo or not on 23-06-2016
                    $.ajax({
                        type: "POST",
                        url: "SalesChallanAdd.aspx/GetConfigSettingRights",
                        data: JSON.stringify({ VariableName: 'IsFIFOExistsInOutModule' }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            //debugger;
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
                    //End



                    document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
                    document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
                    document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
                    document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
                    document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;

                    $('#<%=hdfProductID.ClientID %>').val(strProductID);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
                    //cacpAvailableStock.PerformCallback(strProductID);
                    var objectToPass = {}
                    var product = $("#hdfProductID").val();
                    objectToPass.ProductID = hdfProductID.value;
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
                    // End Rev Rajdip

                    SecondUOMProductId = strProductID;

                    if (Ptype == "W") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "B") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbBatch.PerformCallback('BindBatch~' + "0");
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "S") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");

                        // cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WB") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WS") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);



                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WBS") {

                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);
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
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbBatch.PerformCallback('BindBatch~' + "0");
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else {
                        //div_Warehouse.style.display = 'none';
                        //div_Batch.style.display = 'none';
                        //div_Serial.style.display = 'none';
                        //div_Quantity.style.display = 'none';

                        //$.confirm({
                        //    title: 'Confirm!',
                        //    type: 'blue',
                        //    content: 'No Warehouse or Batch or Serial is actived !',

                        //    buttons: {
                        //        formSubmit: {
                        //            text: 'Ok',
                        //            btnClass: 'btn-blue',
                        //            keys: ['esc'],
                        //            action: function () {
                        //                grid.batchEditApi.StartEdit(index, 5);
                        //            }
                        //        },
                        //    },
                        //});

                        //var strconfirm = confirm("No Warehouse or Batch or Serial is actived.");
                        //if (strconfirm == true) {
                        //    grid.batchEditApi.StartEdit(index, 5);
                        //}
                        //else {
                        //    grid.batchEditApi.StartEdit(index, 5);
                        //}

                        jAlert("No Warehouse or Batch or Serial is actived.");
                    }
                }

                    //-----------------------------------------------------------------------------------------------
                    //WHEN ALTQTY SETTING OFF AND IS TAGGED

                else if (ProductID != "" && IsUomActivate == 0 && parseFloat(QuantityValue) != 0 && IsInvoiceTagged != "") {
                    //var SpliteDetails = ProductID.split("||@||");
                    //var strProductID = SpliteDetails[0];
                    //var strDescription = SpliteDetails[1];
                    //var strUOM = SpliteDetails[2];
                    //var strStkUOM = SpliteDetails[4];
                    //var strMultiplier = SpliteDetails[7];
                    //var strProductName = strDescription;

                    //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    var StkQuantityValue = QuantityValue * strMultiplier;
                    var Ptype = SpliteDetails[14];
                    $('#<%=hdfProductType.ClientID %>').val(Ptype);

                    //Subhabrata Check whether it is in FIFo or not on 23-06-2016
                    $.ajax({
                        type: "POST",
                        url: "SalesChallanAdd.aspx/GetConfigSettingRights",
                        data: JSON.stringify({ VariableName: 'IsFIFOExistsInOutModule' }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            //debugger;
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
                    //End



                    document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
                    document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
                    document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
                    document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
                    document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;

                    $('#<%=hdfProductID.ClientID %>').val(strProductID);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
                    //cacpAvailableStock.PerformCallback(strProductID);
                    var objectToPass = {}
                    var product = $("#hdfProductID").val();
                    objectToPass.ProductID = hdfProductID.value;
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
                    // End Rev Rajdip
                    SecondUOMProductId = strProductID;

                    if (Ptype == "W") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "B") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbBatch.PerformCallback('BindBatch~' + "0");
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "S") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");

                        // cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WB") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WS") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);



                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WBS") {

                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);
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
                        //Rev Rajdip 
                        div_AltQuantity.style.display = 'none';
                        _div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbBatch.PerformCallback('BindBatch~' + "0");
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else {
                        //div_Warehouse.style.display = 'none';
                        //div_Batch.style.display = 'none';
                        //div_Serial.style.display = 'none';
                        //div_Quantity.style.display = 'none';

                        //$.confirm({
                        //    title: 'Confirm!',
                        //    type: 'blue',
                        //    content: 'No Warehouse or Batch or Serial is actived !',

                        //    buttons: {
                        //        formSubmit: {
                        //            text: 'Ok',
                        //            btnClass: 'btn-blue',
                        //            keys: ['esc'],
                        //            action: function () {
                        //                grid.batchEditApi.StartEdit(index, 5);
                        //            }
                        //        },
                        //    },
                        //});

                        //var strconfirm = confirm("No Warehouse or Batch or Serial is actived.");
                        //if (strconfirm == true) {
                        //    grid.batchEditApi.StartEdit(index, 5);
                        //}
                        //else {
                        //    grid.batchEditApi.StartEdit(index, 5);
                        //}

                        jAlert("No Warehouse or Batch or Serial is actived.");
                    }
                }

                    //----------------------------------------------------------------------------------------------------------------------------------------------
                    //WHEN ALTQTY SETTING ON AND IS TAGGED

                else if (ProductID != "" && IsUomActivate == 1 && parseFloat(QuantityValue) != 0 && IsInvoiceTagged != "") {
                    //var SpliteDetails = ProductID.split("||@||");
                    //var strProductID = SpliteDetails[0];
                    //var strDescription = SpliteDetails[1];
                    //var strUOM = SpliteDetails[2];
                    //var strStkUOM = SpliteDetails[4];
                    //var strMultiplier = SpliteDetails[7];
                    //var strProductName = strDescription;

                    //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    var StkQuantityValue = QuantityValue * strMultiplier;
                    var Ptype = SpliteDetails[14];
                    $('#<%=hdfProductType.ClientID %>').val(Ptype);
                    //Rev Rajdip
                    var htmlfactor = "";
                    htmlfactor = parseFloat(sProduct_quantity).toFixed(4) + " ";
                    htmlfactor = htmlfactor + " " + strUOM + " = " + parseFloat(packing_quantity).toFixed(4) + " " + Packing_UOM;
                    $('#lbluomfactor1').text(htmlfactor);
                    //Rev Rajdip
                    //Subhabrata Check whether it is in FIFo or not on 23-06-2016
                    $.ajax({
                        type: "POST",
                        url: "SalesChallanAdd.aspx/GetConfigSettingRights",
                        data: JSON.stringify({ VariableName: 'IsFIFOExistsInOutModule' }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            //debugger;
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
                    //End



                    document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
                    document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
                    document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
                    document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
                    document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;

                    $('#<%=hdfProductID.ClientID %>').val(strProductID);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
                    //cacpAvailableStock.PerformCallback(strProductID);

                    SecondUOMProductId = strProductID;
                    var objectToPass = {}
                    var product = $("#hdfProductID").val();
                    objectToPass.ProductID = hdfProductID.value;
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
                    // End Rev Rajdip
                    if (Ptype == "W") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        //div_AltQuantity.style.display = 'none';
                        //_div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "B") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        //div_AltQuantity.style.display = 'none';
                        //_div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbBatch.PerformCallback('BindBatch~' + "0");
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "S") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        //div_AltQuantity.style.display = 'none';
                        //_div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");

                        // cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WB") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        //Rev Rajdip 
                        //div_AltQuantity.style.display = 'none';
                        //_div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "block");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WS") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';
                        //Rev Rajdip 
                        //div_AltQuantity.style.display = 'none';
                        //_div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);



                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WBS") {

                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Serial.style.display = 'block';
                        div_Quantity.style.display = 'none';
                        //Rev Rajdip 
                        //div_AltQuantity.style.display = 'none';
                        //_div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);
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
                        //Rev Rajdip 
                        //div_AltQuantity.style.display = 'none';
                        //_div_Uom.style.display = 'none';
                        //End Rev Rajdip
                        cCmbBatch.PerformCallback('BindBatch~' + "0");
                        //cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                        $("#ADelete").css("display", "none");//Subhabrata
                        SelectedWarehouseID = "0";
                        cPopup_Warehouse.Show();
                    }
                    else {
                        //div_Warehouse.style.display = 'none';
                        //div_Batch.style.display = 'none';
                        //div_Serial.style.display = 'none';
                        //div_Quantity.style.display = 'none';

                        //$.confirm({
                        //    title: 'Confirm!',
                        //    type: 'blue',
                        //    content: 'No Warehouse or Batch or Serial is actived !',

                        //    buttons: {
                        //        formSubmit: {
                        //            text: 'Ok',
                        //            btnClass: 'btn-blue',
                        //            keys: ['esc'],
                        //            action: function () {
                        //                grid.batchEditApi.StartEdit(index, 5);
                        //            }
                        //        },
                        //    },
                        //});

                        //var strconfirm = confirm("No Warehouse or Batch or Serial is actived.");
                        //if (strconfirm == true) {
                        //    grid.batchEditApi.StartEdit(index, 5);
                        //}
                        //else {
                        //    grid.batchEditApi.StartEdit(index, 5);
                        //}

                        jAlert("No Warehouse or Batch or Serial is actived.");
                    }
                }


                    //-------------------------------------------------------------------------End Rev Rajdip-------------------------------------------------------


                    //Rev rajdip
                    //else if (ProductID != "" && parseFloat(QuantityValue) == 0 ) {
                else if (ProductID != "" && parseFloat(QuantityValue) == 0) {
                    //End Rev rajdip
                    //$.confirm({
                    //    title: 'Confirm!',
                    //    type: 'blue',
                    //    content: 'Please enter Quantity !',

                    //    buttons: {
                    //        formSubmit: {
                    //            text: 'Ok',
                    //            btnClass: 'btn-blue',
                    //            keys: ['esc'],
                    //            action: function () {
                    //                grid.batchEditApi.StartEdit(index, 5);
                    //            }
                    //        },
                    //    },
                    //});

                    jAlert('Qty is ZERO. Cannot select Stk Details');
                }
                else if (ProductID != "" && parseFloat(QuantityValue) != 0 && IsExits == false) {
                    jAlert("Available stock of the selected product is ZERO(0). Cannot proceed.", "Stock Alert");
                    //['" + ProductID.split("||@||")[1] + "']

                }

}
}
//Rev Rajdip For Delete SalesMan
function Deletesalesman() {

    $("#<%=hdnSalesManAgentId.ClientID%>").val("");
    //chinmoy 123
    ctxtSalesManAgent.SetText("");
    //chinmoy 123
}
//End Rev rajdip
function FinalWarehouse() {
    //if ($('#hdnShowUOMConversionInEntry').val() == "1") {
    //    var FilterSerialMain = $.grep(aarr, function (e) { return e.productid == SecondUOMProductId });
    //    var FilterSerial = $.grep(SecondUOM, function (e) { return e.ProductId == SecondUOMProductId && e.Checked == 1 });
    //    if (FilterSerialMain) {
    //        if (FilterSerial.length > 0) {
    //            //alert(FilterSerialMain[0].packing + FilterSerial.length);
    //            if (parseFloat(FilterSerialMain[0].packing) != parseFloat(FilterSerial.length)) {
    //                jAlert('Qunatity mismatched found . Can not proceed', 'Alert');
    //                return;
    //            }
    //        }
    //    }
    //}


    cGrdWarehouse.PerformCallback('WarehouseFinal');
    //Rev Subhra 15-05-2019
    grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //End of Rev Subhra 15-05-2019

}
// Mantis Issue 24428 
function CalcBaseQty() {

    debugger;

    //var PackingQtyAlt = Productdetails.split("||@||")[20]; // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
    // var PackingQty = Productdetails.split("||@||")[22]; // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
    //var PackingSaleUOM = Productdetails.split("||@||")[25];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)


    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

    var PackingQtyAlt = 0;
    var PackingQty = 0;
    var PackingSaleUOM = 0;

    var ProductID = Productdetails.split("||@||")[0];


    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/AutoPopulateAltQuantity",
        data: JSON.stringify({ ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            if (msg.d.length != 0) {
                PackingQtyAlt = msg.d[0].packing_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                PackingQty = msg.d[0].sProduct_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                PackingSaleUOM = msg.d[0].AltUOMId;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
            }
            else {
                PackingQtyAlt = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                PackingQty = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                PackingSaleUOM = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
            }

            if (PackingQtyAlt == "") {
                PackingQtyAlt = 0
            }
            if (PackingQty == "") {
                PackingQty = 0
            }

            // if Base UOM of product is not same as the Alternate UOM selected from Product Master, then Calculation of Base Quantity will not happen
            if (ccmbSecondUOM.GetValue() != PackingSaleUOM) {
                PackingQtyAlt = 0;
                PackingQty = 0;
            }

            var BaseQty = 0
            if (PackingQtyAlt > 0) {
                var ConvFact = PackingQty / PackingQtyAlt;
                var altQty = cAltUOMQuantity.GetValue();

                if (ConvFact > 0) {
                    var BaseQty = (altQty * ConvFact).toFixed(4);
                    $("#UOMQuantity").val(BaseQty);
                }
            }
            else {
                $("#UOMQuantity").val("0.0000");
            }
        }
    });

}

function CalcBaseRate() {

    var altQty = cAltUOMQuantity.GetValue();
    var altRate = ccmbAltRate.GetValue();
    var baseQty = $("#UOMQuantity").val();


    if (baseQty > 0) {
        var BaseRate = (altQty * altRate) / baseQty;
        ccmbBaseRate.SetValue(BaseRate);
    }
}
// End of Mantis Issue 24428 

function closeWarehouse(s, e) {
    e.cancel = false;
    // cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');
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
        grid.batchEditApi.StartEdit(Warehouseindex, 7);
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        //Rev Rajdip
        var IsUomActivate = $("#<%=hdnShowUOMConversionInEntry.ClientID%>").val();
                    var IsInvoiceTagged = gridSalesOrderLookup.GetText();
                    if (IsUomActivate == 1 && IsInvoiceTagged == "") {

                        var quantity = parseFloat(cGrdWarehouse.cpWarehouseQty);
                        //alert(ctxtQuantity.GetText());	
                        grid.GetEditor('Quantity').SetValue(quantity);
                        cPopup_Warehouse.Hide();
                    }
                    else {
                        jAlert('Entered Quantity for the selected product must be equal to Stock Quantity.');
                    }
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
        grid.batchEditApi.StartEdit(Warehouseindex, 9);
    }
    else {
        grid.batchEditApi.StartEdit(Warehouseindex, 9);
    }

    //grid.GetEditor("SalePrice").Focus();
    //Rev Subhra 15-05-2019
    grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //End of Rev Subhra 15-05-2019
}




function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
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

function SaveWarehouse() {

    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var WarehouseName = cCmbWarehouse.GetText();
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";
    var BatchName = cCmbBatch.GetText();
    var SerialID = "";
    var SerialName = "";
    var Qty = ctxtQuantity.GetValue();
    var AltQty;
    var AltUom;
    //Rev Rajdip
    var IsUomActivate = $("#<%=hdnShowUOMConversionInEntry.ClientID%>").val();
    var IsInvoiceTagged = gridSalesOrderLookup.GetText();
    if (IsUomActivate == 0 && IsInvoiceTagged == "") {

        AltQty = 0;
        AltUom = 0;
    }
    else {
        AltQty = (CtxtPacking.GetText() != null) ? CtxtPacking.GetText() : "0";
        AltUom = (ccmbPackingUom1.GetValue() != null) ? ccmbPackingUom1.GetValue() : "0";
    }
    CtxtPacking.SetText("");
    //End Rev Rajdip

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
    if ((Ptype == "B" && BatchID != "0") || (Ptype == "WB" && BatchID != "0") || (Ptype == "WBS" && BatchID != "0") || (Ptype == "BS" && BatchID != "0")) {
        var AVAILsTOCK = $("#lblAvailableStock").text();
        var Qty = ctxtQuantity.GetValue();

        if (parseFloat(Qty,4) > parseFloat(AVAILsTOCK,4))
        {
            jAlert("Entered Quantity is more than Available Stock ");
            return;
        }
       
    }
    if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
        $("#spnCmbWarehouse").show();
    }
    else if ((Ptype == "B" && BatchID == "0") || (Ptype == "WB" && BatchID == "0") || (Ptype == "WBS" && BatchID == "0") || (Ptype == "BS" && BatchID == "0")) {
        $("#spnCmbBatch").show();
    }
    else if ((Ptype == "W" && Qty == "0.0000") || (Ptype == "B" && Qty == "0.0000") || (Ptype == "WB" && Qty == "0.0000")) {
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
                checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
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
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
        }
        UpdateText();

        //Rev Rajdip
        //cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);

        //End Rev Rajdip
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID + '~' + AltQty + '~' + AltUom);
        SelectedWarehouseID = "0";
    }
}



//function txtserialTextChanged() {
//    var SerialNo = (ctxtserial.GetValue != null) ? (ctxtserial.GetValue()) : "0";
//    ctxtserial.SetValue("");
//    var texts = [SerialNo];
//    var values = GetValuesByTexts(texts);
//    checkListBox.SelectValues(values);
//    UpdateSelectAllItemState();
//    UpdateText(); // for remove non-existing texts
//    SaveWarehouse();
//} By Sudip

function txtserialTextChanged() {
    //debugger;
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


        //Subhabrata added: on 19-06-2017
        var WarehouseID = $("#<%=hddnWarehouseId.ClientID%>").val();
                var BatchID = $("#<%=hddnBatchId.ClientID%>").val();
                var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();
                var MatchQty = $("#<%=hddnMatchQty.ClientID%>").val();
                var type = document.getElementById('hdfProductType').value;

                if (type == "WBS" || type == "WB") {
                    //cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
                    if (FifoExists == "0") {
                        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + "NoFIFO");
                    }
                    else if (FifoExists == "1") {
                        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + (MatchQty * 1));
                    }

                }
                else if (type == "BS") {

                    //checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
                    if (FifoExists == "0") {
                        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + "NoFIFO");
                    }
                    else if (FifoExists == "1") {
                        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID + '~' + (MatchQty * 1));
                    }
                }
                else if (type == "WS") {
                    //checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
                    if (FifoExists == "0") {
                        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + "NoFIFO");
                    }
                    else if (FifoExists == "1") {
                        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0" + '~' + (MatchQty * 1));
                    }

                }
                UpdateSelectAllItemState();
                UpdateText(); // for remove non-existing texts
                //END
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
                cbtnWarehouse.SetVisible(false);
                checkComboBox.Focus();
            }
            else {
                divSingleCombo.style.display = "none";
                divMultipleCombo.style.display = "block";
                cbtnWarehouse.SetVisible(true);
                ctxtserial.Focus();
            }
        }


        function fn_Deletecity(keyValue) {
            //debugger;
            var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
            var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

            var FifoExists = $("#<%=hddnConfigVariable_Val.ClientID%>").val();

    var Qty = ctxtMatchQty.GetValue();
    var hddnQty = $("#<%=hddnWarehouseQty.ClientID%>").val();
    <%--if ((hddnQty * 1) > 0)
    {
        hddnQty = (hddnQty * 1) - 1;
    }
    $("#<%=hddnWarehouseQty.ClientID%>").val(hddnQty);--%>
    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    if (FifoExists == "0") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID + '~' + 'NoFIFO');
    }

}

function clookup_Project_GotFocus() {
    clookup_Project.gridView.Refresh();
    clookup_Project.ShowDropDown();
}


function clookup_Project_LostFocus() {
    grid.batchEditApi.StartEdit(-1, 2);

    //Hierarchy Start Tanmoy
    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
    //Hierarchy End Tanmoy
}

//Hierarchy Start Tanmoy
function ProjectValueChange(s, e) {
    //debugger;
    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'SalesChallanAdd.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}
//Hierarchy End Tanmoy

  <%-- kaushik 20-2-2017 --%>

        $(document).ready(function () {

            if (document.getElementById('hdnPageStatus').value == "update") {
                clookup_Project.SetEnabled(false);
            }



            $('#ddl_VatGstCst_I').blur(function () {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            })


            $('#ddl_AmountAre_I').blur(function () {


                var key = cddl_AmountAre.GetValue();

                if (key == 1 || key == 3) {
                    if (grid.GetVisibleRowsOnPage() == 1) {

                        if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                            clookup_Project.SetFocus();
                        }
                        else {
                            grid.batchEditApi.StartEdit(-1, 2);
                        }
                    }

                }
            })

        });
        <%-- kaushik 20-2-2017 --%>
    </script>
    <script type="text/javascript">
        // <![CDATA[
        var textSeparator = ";";
        var selectedChkValue = "";
        var IsSelected = false;
        function OnListBoxSelectionChanged(listBox, args) {
            //debugger;
            var selectedItems = checkListBox.GetSelectedItems();
            //if ((args.index * 1) > ((selectedItems.length * 1))) {
            //    if (args.isSelected) {

            //        var indices = [];
            //        //checkListBox.SetCheckBoxEnabled(args.index, true);
            //        indices.push(listBox.GetItem(args.index));
            //        listBox.UnselectIndices(indices[0].text);
            //        //UpdateSelectAllItemState();
            //        UpdateText();
            //        jAlert("Canonot proceed!");
            //        return false;
            //    }

            //}
            if (args.index == 0) {
                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
                if (args.isSelected) {
                    IsSelected = true;
                }
                else {
                    IsSelected = false;
                }

            }
            var k = 0;



            //if (selectedItems.length != 0) {
            //    checkListBox.SetCheckBoxEnabled(((selectedItems.length * 1) + 1), true);
            //    //listBox.SetEnabled(((selectedItems.length * 1) + 1), false);
            //}

            //for (var i = (selectedItems.length * 1) + 1 ; i < (checkListBox.GetItemCount() * 1) ; i++) {

            //    checkListBox.SetCheckBoxEnabled(i, false);
            //}



            var qty = grid.GetEditor('Quantity').GetValue();
            var selectedItems = checkListBox.GetSelectedItems();
            var val = GetSelectedItemsText(selectedItems);

            UpdateSelectAllItemState();
            UpdateText();



            //checkboxEnabled/Disabled
            //var indices = [];
            //indices.push(listBox.GetItem(args.index));
            //listBox.SetEnabled(indices, false);

            //checkListBox.items[indices[0].text].enable = false;
            //listBox.SetEnabled(indices[0].text) = false;
            //checkListBox["SetEnabled"][indices[0].text] = false;
            //checkListBox.SetCheckBoxEnabled[
            //End

            var strWarehouse = cCmbWarehouse.GetValue();
            var strBatchID = cCmbBatch.GetValue();
            var ProducttId = $("#hdfProductID").val();

            // FIFO Checking 
            //$.ajax({
            //    type: "POST",
            //    url: "SalesChallanAdd.aspx/GetSerialId",
            //    data: JSON.stringify({
            //        "id": val,
            //        "wareHouseStr": strWarehouse,
            //        "BatchID": strBatchID,
            //        "ProducttId": ProducttId
            //    }),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    async: false,//Added By:Subhabrata
            //    success: function (msg) {
            //        
            //        var type = msg.d;
            //        if (type == "1") {

            //            return true;
            //        }
            //        else if (type == "0") {
            //            alert("Serial No can be Stock out based on FIFO process.Select the Serial No. shown from Oldest to Newest sequence to proceed");
            //            //listBox.UnselectAll();

            //            var indices = [];
            //            //Added By:Subhabrata
            //            if ((selectedItems.length * 1) == 1) {
            //                indices.push(listBox.GetItem(args.index));
            //                listBox.UnselectIndices(indices[0].text);
            //                UpdateSelectAllItemState();
            //                UpdateText();
            //            }
            //            if (((args.index) * 1) <= (selectedItems.length * 1)) {
            //                for (var i = ((args.index) * 1) ; i <= ((selectedItems.length * 1) + 1) ; i++) {
            //                    indices.push(listBox.GetItem(i));

            //                }
            //            }
            //            else {
            //                indices.push(listBox.GetItem(args.index));
            //                listBox.UnselectIndices(indices[0].text);
            //                UpdateSelectAllItemState();
            //                UpdateText();
            //            }

            //            for (var j = 0; j < indices.length   ; j++) {
            //                listBox.UnselectIndices(indices[j].text);
            //                UpdateSelectAllItemState();
            //                UpdateText();
            //            }



            //        }
            //    }
            //});

            //Subhabrata

            //checkListBox.PerformCallback('CheckSerialOnFIFO~' + strWarehouse + '~' + strBatchID + '~' + val); var val = GetSelectedItemsText(selectedItems);

            //checkListBox.PerformCallback('CheckSerialOnFIFO~' + strWarehouse + '~' + strBatchID + '~' + val);
            //End
        }
        function UpdateSelectAllItemState() {
            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
        }
        //function IsAllSelected() {
        //    debugger;
        //    var selectedDataItemCount;
        //    if (checkListBox.GetValue() != null) {
        //        selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
        //        checkListBox.GetSelectedItems().length = selectedDataItemCount;
        //    }
        //    else {
        //        checkListBox.GetSelectedItems().length = 0;
        //    }

        //    return checkListBox.GetSelectedItems().length;
        //    //var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
        //    //return checkListBox.GetSelectedItems().length == selectedDataItemCount;

        //}

        function IsAllSelected() {
            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
        }
        function UpdateText() {
            //debugger;
            var selectedItems = checkListBox.GetSelectedItems();
            selectedChkValue = GetSelectedItemsText(selectedItems);
            //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
            var ActualLength = (checkListBox.GetItemCount() * 1);
            if (IsSelected == true || ActualLength == selectedItems.length) {
                checkComboBox.SetText(((selectedItems.length * 1) - 1) + " Items");
            }
            else {
                checkComboBox.SetText((selectedItems.length) + " Items");
            }


            var val = GetSelectedItemsText(selectedItems);
            $("#abpl").attr('data-content', val);
        }

        //function UpdateText() {
        //    var selectedItems = checkListBox.GetSelectedItems();
        //    selectedChkValue = GetSelectedItemsText(selectedItems);
        //    //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
        //    checkComboBox.SetText(selectedItems.length + " Items");

        //    var val = GetSelectedItemsText(selectedItems);
        //    $("#abpl").attr('data-content', val);
        //}by Subhabrata

        function SynchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            //var texts = dropDown.GetText().split(textSeparator);
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





    <script type="text/javascript">
        $(function () {
            $("#btnAdd").bind("click", function () {
                $("#SerialContainer").empty();
                $("#BatchContainer").empty();
                $("#SerialContainer").append("<div><span>Serial Number</span><div />");
                $("#BatchContainer").append("<div><span>Batch Number</span><div />");

                var count = ctxtQuantity_Warehouse.GetValue();


                for (var i = 1; i <= count; i++) {
                    var div = $("<div />");
                    div.html(GetDynamicSerial(""));
                    $("#SerialContainer").append(div);

                    var div1 = $("<div />");
                    div1.html(GetDynamicBatch(""));
                    $("#BatchContainer").append(div1);
                }
            });
        });

        function GetDynamicSerial(value) {
            return '<input name = "SerialContainer" type="text" value = "' + value + '" />'
        }

        function GetDynamicBatch(value) {
            return '<input name = "BatchContainer" type="text" value = "' + value + '" />'
        }


        function selectValue() {

            var startDate = new Date();
            startDate = cPLSalesChallanDate.GetValueString();
            //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
            //var key = gridLookup.GetValue();
            var key = $("#<%=hdnCustomerId.ClientID%>").val();
            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

            // gridSalesOrderLookup.gridView.Refresh();

            if (key != null && key != '' && type != "") {
                cSalesOrderComponentPanel.PerformCallback('BindSalesOrderGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);
            }



            var componentType = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
            if (componentType != null && componentType != '') {
                grid.PerformCallback('GridBlank');
            }
        }

        function OrderNumberChanged() {

            //gridquotationLookup.GetValue()
            //grid.PerformCallback('BindGridOnQuotation' + '~' + cddl_Quotatione.GetValue() + '~' + ctxt_SlOrderNo.GetValue());
            //var quote_Id = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());

            var quote_Id = gridSalesOrderLookup.GetValue();
            if (SimilarProjectStatus != "-1") {
                if (quote_Id != null) {
                    var arr = quote_Id.split(',');

                    if (arr.length > 1) {
                        cPLQADate.SetText('Multiple Select Order Dates');

                    }
                    else {
                        if (arr.length == 1) {
                            cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);


                        }
                        else {
                            cPLQADate.SetText('');

                        }
                    }
                    //cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
                    //cProductsPopup.Show();

                }
                else { cPLQADate.SetText(''); }

                if (quote_Id != null) {
                    var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

                    if (type != '' && type != null) {
                        cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@' + '~' + type);
                        cProductsPopup.Show();
                    }
                }
                else {
                    cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
                    cProductsPopup.Show();
                }

                txt_OANumber.Focus();
            }
        }

        <%--kaushik--%>
        function GridCallBack() {
            //grid.PerformCallback('Display');
            $('#ddlInventory').focus();
        }
        <%--kaushik--%>

        function ChangeState(value) {

            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }


        function GetDocumentAddress(OrderId, TagDocType) {
            var OtherDetail = {};
            OtherDetail.OrderId = OrderId;
            OtherDetail.TagDocType = TagDocType;
            if ((OrderId != null) && (OrderId != "")) {
                $.ajax({
                    type: "POST",
                    url: "SalesChallanAdd.aspx/SaveDocumentAddress",
                    data: JSON.stringify(OtherDetail),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        Address = msg.d;
                        PopulateBillingShippingAddress(Address);
                        cddl_PosGst.SetEnabled(false);
                    }
                });
            }
        }
        function PopulateBillingShippingAddress(ReturnDetails) {
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
                ctxtDistance.SetText(BillingDetails[0].Distance);
                var GSTIN = BillingDetails[0].GSTIN;
                var GSTIN1 = GSTIN.substring(0, 2);
                var GSTIN2 = GSTIN.substring(2, 12);
                var GSTIN3 = GSTIN.substring(12, 15);
                ctxtBillingGSTIN1.SetText(GSTIN1);
                ctxtBillingGSTIN2.SetText(GSTIN2);
                ctxtBillingGSTIN3.SetText(GSTIN3);
                GetPosForGstValue();
                cddl_PosGst.SetValue(BillingDetails[0].PosForGst);
                // clookup_Project.gridView.SelectItemsByKey(BillingDetails[0].ProjectCode);	
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
                ctxtDistance.SetText('');
                ctxtBillingGSTIN1.SetText('');
                ctxtBillingGSTIN2.SetText('');
                ctxtBillingGSTIN3.SetText('');
                //chinmoy commeneted	
                // GetPosForGstValue();	
                // cddl_PosGst.SetText('');	
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
                ctxtDistanceShipping.SetText(ShippingDetails[0].Distance);
                var GSTIN = ShippingDetails[0].GSTIN;
                var GSTIN1 = GSTIN.substring(0, 2);
                var GSTIN2 = GSTIN.substring(2, 12);
                var GSTIN3 = GSTIN.substring(12, 15);
                ctxtShippingGSTIN1.SetText(GSTIN1);
                ctxtShippingGSTIN2.SetText(GSTIN2);
                ctxtShippingGSTIN3.SetText(GSTIN3);
                ctxtShipToPartyShippingAdd.SetText(ShippingDetails[0].ShipToPartyName);
                $('#hdShipToParty').val(ShippingDetails[0].ShipToPartyId);
                GetPosForGstValue();
                cddl_PosGst.SetValue(ShippingDetails[0].PosForGst);
                //clookup_Project.gridView.SelectItemsByKey(ShippingDetails[0].ProjectCode);	
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
                ctxtDistanceShipping.SetText('');
                ctxtShippingGSTIN1.SetText('');
                ctxtShippingGSTIN2.SetText('');
                ctxtShippingGSTIN3.SetText('');
                ctxtShipToPartyShippingAdd.SetText('');
                $('#hdShipToParty').val('');
                //chinmoy commented	
                // GetPosForGstValue();	
                // cddl_PosGst.SetText('');	
            }
        }

        function PerformCallToGridBind() {
            //debugger;
            if (cgridproducts.GetSelectedRowCount() != 0) {
                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
                AllowAddressShipToPartyState = false;
                $("#hdnTagType").val(type);
                if (type != '' && type != null) {
                    grid.PerformCallback('BindGridOnQuotation' + '~' + '@' + '~' + type);
                }
                var OrderIds = gridSalesOrderLookup.GetValue();
                var Key = OrderIds.split(',')[0];
                $.ajax({
                    type: "POST",
                    url: "SalesChallanAdd.aspx/GetContactSalesManReference",
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
                        //chinmoy 123
                        ctxtSalesManAgent.SetText(SalesManAgent.trim());
                        //chinmoy 123
                        ctxtCreditDays.SetText(creditDays);
                        cdt_SaleInvoiceDue.SetText(DueDate);
                        //GetContactSalesManReference
                        $("#<%=ddl_Currency.ClientID%>").val(CurrencyId);
                        ctxtEWayBillNO.SetValue(EwayBillNumber);
                        //ctxtEwayBillNO.SetText(EwayBillNumber);
                        //ctxtEWayBillNO.SetEnabled(false);
                        //ctxtEWayBillNO.Focus();

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
                    //BSDocTagging(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());	
                    //Chinmoy added Below line	
                    GetDocumentAddress(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
                }

                if (quote_Id.length > 0) {
                    BindOrderProjectdata(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
                }

                ctxtCustName.SetEnabled(false);


                //#### End : Samrat Roy for Transporter Control : End #############

                //#### added by Sayan Dutta for TC Control #############
                if ($("#btn_TermsCondition").is(":visible")) {
                    callTCControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
                }
                //#### End : added by Sayan Dutta for TC Control : End #############
                if ($("#btn_OtherTermsCondition").is(":visible")) {
                    if (quote_Id.length > 0) {
                        callOTCControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
                    }
                }
                return false;
            }
            else {
                //grid.PerformCallback('GridBlank');
                //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                deleteTax("DeleteAllTax", "", "");
                $('input[type=radio]').prop('checked', false);
                gridSalesOrderLookup.SetEnabled(false);
                return false;
            }
        }



        function BindOrderProjectdata(OrderId, TagDocType) {

            var OtherDetail = {};

            OtherDetail.OrderId = OrderId;
            OtherDetail.TagDocType = TagDocType;


            if ((OrderId != null) && (OrderId != "")) {

                $.ajax({
                    type: "POST",
                    url: "SalesChallanAdd.aspx/SetProjectCode",
                    data: JSON.stringify(OtherDetail),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var Code = msg.d;

                        clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                        clookup_Project.SetEnabled(false);
                    }
                });
            }
        }



        function componentEndCallBack(s, e) {
            //gridSalesOrderLookup.gridView.Refresh();
            if (grid.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick();
            }
        }
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                // var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '';
                var keyval = $('#<%=hdnmodeId.ClientID %>').val();
                //  alert(keyval);
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SC&&KeyVal_InternalID=' + keyval;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code
    </script>
    <%--End Sudip--%>




    <%--Debu Section--%>
    <script type="text/javascript">


        //function taxAmountLostFocus(s, e) {
        //    var finalTaxAmt = parseFloat(s.GetValue());
        //    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
        //    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        //    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        //    if (sign == '(+)') {
        //        ctxtTaxTotAmt.SetValue(totAmt + finalTaxAmt - taxAmountGlobal);
        //    } else {
        //        ctxtTaxTotAmt.SetValue(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1));
        //    }


        //    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

        //}



        var GlobalCurTaxAmt = 0;
        var rowEditCtrl;
        var globalRowIndex;
        var globalTaxRowIndex;
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
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

        //function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
        //    for (var i = 0; i < taxJson.length; i++) {
        //        if (taxJson[i].applicableBy == name) {
        //            cgridTax.batchEditApi.StartEdit(i, 3);
        //            cgridTax.GetEditor('calCulatedOn').SetValue(amt);

        //            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        //            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
        //            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        //            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
        //            var s = cgridTax.GetEditor("TaxField");
        //            if (sign == '(+)') {
        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                //cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
        //                cgridTax.GetEditor("Amount").SetValue(((ProdAmt * s.GetText()) / 100).toFixed(2));

        //                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
        //                GlobalCurTaxAmt = 0;
        //            }
        //            else {

        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                //cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
        //                cgridTax.GetEditor("Amount").SetValue((((ProdAmt * s.GetText()) / 100) * -1).toFixed(2));

        //                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
        //                GlobalCurTaxAmt = 0;
        //            }




        //        }
        //    }
        //    //return;
        //    cgridTax.batchEditApi.EndEdit();
        //}

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
            //return;
            gridTax.batchEditApi.EndEdit();
        }



        //function txtPercentageLostFocus(s, e) {

        //    //var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
        //    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
        //    if (s.GetText().trim() != '') {

        //        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
        //            //Checking Add or less
        //            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        //            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
        //            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        //            if (sign == '(+)') {
        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

        //                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
        //                GlobalCurTaxAmt = 0;
        //            }
        //            else {

        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

        //                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
        //                GlobalCurTaxAmt = 0;
        //            }
        //            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

        //            //Call for Running Total
        //            SetRunningTotal();

        //        } else {
        //            s.SetText("");
        //        }
        //    }

        //    RecalCulateTaxTotalAmountInline();
        //}

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

            ctxtTaxTotAmt.SetValue((totalInlineTaxAmount * 1).toFixed(2));
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







    </script>
    <%--Debu Section End--%>

    <%--Warehouse Section Start--%>
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
                var strAltQty = cCallbackPanel.cpEdit.split('~')[4];
                var strAltUOM = cCallbackPanel.cpEdit.split('~')[5];
                SelectWarehouse = strWarehouse;
                SelectBatch = strBatchID;
                SelectSerial = strSrlID;

                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
                //Rev Rajdip
                //checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);
                checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID + '~' + strAltQty + '~' + strAltUOM);
                //End Rev Rajdip
                cCmbWarehouse.SetValue(strWarehouse);
                ctxtQuantity.SetValue(strQuantity);
                CtxtPacking.SetValue(strAltQty);
                ccmbPackingUom1.SetValue(strAltUOM);
            }
        }
        $(document).ready(function () {

            $('.generalTab').click(function () {
                $('.crossBtn').show();
            });
            $('.bilingTab').click(function () {
                if (!$(this).hasClass('dxtcLiteDisabled_PlasticBlue')) {
                    $('.crossBtn').hide();
                }
            });
        });
    </script>
    <%--Warehouse Section End--%>




    <%--Added By : Samrat Roy -- New Billing/Shipping Section--%>
    <script>
        function disp_prompt(name) {

            if (name == "tab0") {
                //gridLookup.Focus();	
                ctxtCustName.Focus();
                page.GetTabByName('Billing/Shipping').SetEnabled(true);
                $("#crossBtnId").show();
            }
            if (name == "tab1") {
                $("#crossBtnId").hide();
                var custID = GetObjectID('hdnCustomerId').value;
                page.GetTabByName('General').SetEnabled(false);
                if (custID == null && custID == '') {
                    jAlert('Please select a customer');
                    page.SetActiveTabIndex(0);
                    return;
                }
                else {
                    page.SetActiveTabIndex(1);
                }
            }
        }

    </script>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section End--%>
    <script>
        function GetSalesRateSchemePrice(CustomerID, ProductID, SalesPrice) {

            var date = new Date;
            var seconds = date.getSeconds();
            var minutes = date.getMinutes();
            var hour = date.getHours();

            var times = hour + ':' + minutes;

            var sdate = cPLSalesChallanDate.GetValue();
            var startDate = new Date(sdate);
            var OtherDetails = {}
            OtherDetails.CustomerID = CustomerID;
            OtherDetails.ProductID = ProductID;
            OtherDetails.PostingDate = startDate;//+ ' ' + times;
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetSalesRateSchemePrice",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    console.log(returnObject);
                    //Rev work start 28.06.2022 Mantise no:24949
                    //$("#ProductMinPrice").val(returnObject[0].MinSalePrice);
                    //$("#ProductMaxPrice").val(returnObject[0].MaxSalePrice);
                    if (returnObject.length > 0) {
                        $("#ProductMinPrice").val(returnObject[0].MinSalePrice);
                        $("#ProductMaxPrice").val(returnObject[0].MaxSalePrice);
                    }
                    else {
                        $("#ProductMinPrice").val(0);
                        $("#ProductMaxPrice").val(0);
                    }
                    setValueOfSAale();
                    //Rev work close 28.06.2022 Mantise no:24949
                }
            });
        }
        //Rev work start 28.06.2022 Mantise no:24949
        function setValueOfSAale() {
            if ($("#ProductMinPrice").val() != "") {
                grid.GetEditor("SalePrice").SetValue($("#ProductMinPrice").val());
            }
        }
        //Rev work close 28.06.2022 Mantise no:24949
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Subhra Section Start--%>



    <%--Subhra Section End--%>


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
                    <div class="lblHolder">
                        <table>
                            <tr>
                                <td>Available Stock</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblAvailableSStk" runat="server" Text="0.0000"></asp:Label>
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

        <div id="crossBtnId" class="crossBtn" runat="server"><a href="SalesChallanEntityList.aspx"><i class="fa fa-times"></i></a></div>
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
                                                <%-- <asp:ListItem Value="N">Non-Inventory Items</asp:ListItem>
                                                  <asp:ListItem Value="C">Capital Goods</asp:ListItem>--%>
                                            </asp:DropDownList>

                                            <%--<dxe:ASPxCallbackPanel runat="server" ID="IsInventotry" ClientInstanceName="cIsInventory" OnCallback="ComponentIsInventory_Callback">
                                                        <PanelCollection>
                                                            <dxe:PanelContent runat="server">
                                                <dxe:ASPxComboBox ID="cmbIsInventory" ClientInstanceName="ccmbIsInventory" runat="server" TabIndex="14"  
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                     <Items>
                                                        <dxe:ListEditItem Text="Yes" Value="1" />
                                                        <dxe:ListEditItem Text="No" Value="0" />
                                                    </Items>
                               
                                                    <ClientSideEvents SelectedIndexChanged="isInventoryChanged" />
                                                </dxe:ASPxComboBox>
                                             </dxe:PanelContent>
                                                        </PanelCollection>

                                                    </dxe:ASPxCallbackPanel>--%>
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
                                            <ClientSideEvents LostFocus="function(s, e) {UniqueCodeCheck();}" />

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

                                        <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" TabIndex="6" ClientInstanceName="ctxtCustName" Width="100%">

                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>

                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />

                                        </dxe:ASPxButtonEdit>

                                        <%--<dxe:ASPxComboBox ID="CustomerComboBox" runat="server" TabIndex="6" EnableCallbackMode="true" CallbackPageSize="15"
                                            ValueType="System.String" ValueField="cnt_internalid" ClientInstanceName="gridLookup" Width="92%"
                                            OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL"
                                            OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{1} [{0}]"
                                            DropDownStyle="DropDown">
                                            <Columns>
                                                <dxe:ListBoxColumn FieldName="uniquename" Caption="Unique ID" Width="200px" />
                                                <dxe:ListBoxColumn FieldName="Name" Caption="Name" Width="200px" />
                                                <dxe:ListBoxColumn FieldName="Billing" Caption="Billing Address" Width="300px" />
                                            </Columns>
                                            <ClientSideEvents ValueChanged="function(s, e) { GetContactPerson(e)}" />
                                            <ClientSideEvents GotFocus="function(s,e){gridLookup.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>--%>


                                        <%--<dxe:ASPxGridLookup ID="lookup_Customer" runat="server" TabIndex="6" ClientInstanceName="gridLookup"
                                                KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">

                                                <Columns>


                                                    <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" Width="150" Settings-AutoFilterCondition="Contains" />
                                                    <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                    </dxe:GridViewDataColumn>
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
                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" UseSubmitBehavior="False" ClientSideEvents-Click="CloseGridLookup" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </StatusBar>
                                                    </Templates>

                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                   

                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                                                </GridViewProperties>
                                                <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" />
                                                <ClientSideEvents GotFocus="function(s,e){gridLookup.ShowDropDown();}" />
                                                <ClearButton DisplayMode="Auto">
                                                </ClearButton>
                                            </dxe:ASPxGridLookup>--%>

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
                                        <%--<asp:DropDownList ID="ddl_ContactPerson" runat="server" TabIndex="6" Width="100%"></asp:DropDownList>--%>
                                    </div>
                                    <div class="col-md-2">
                                        <%-- <label>
                                            <dxe:ASPxLabel ID="lbl_quotation_No" runat="server" Text="Sale Order No" Width="120px">
                                            </dxe:ASPxLabel>
                                        </label>--%>
                                        <asp:RadioButtonList ID="rdl_SaleInvoice" runat="server" RepeatDirection="Horizontal" TabIndex="8" onchange="return selectValue();" Width="120px">
                                            <asp:ListItem Text="Order" Value="SO"></asp:ListItem>
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
                                        <%--  <asp:DropDownList ID="ddl_Quotation_No" runat="server" Width="100%" TabIndex="1" >
                    </asp:DropDownList>--%>
                                    </div>

                                    <div class="col-md-2">
                                        <label>
                                            <dxe:ASPxLabel ID="lbl_SaleOrder_Date" runat="server" Text="Document Date" Width="120px">
                                            </dxe:ASPxLabel>
                                        </label>

                                        <asp:Label ID="lbl_MultipleDate" runat="server" Text="Multiple Select Sale Invoice Dates" Style="display: none"></asp:Label>

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
                                            <a href="#" style="left: -12px; top: 20px;"><%--onclick="AddcustomerClick()"--%>

                                                <i id="I1" runat="server" class="fa fa-trash" aria-hidden="true" onclick="Deletesalesman()"></i>


                                            </a>
                                        </label>
                                        <%--<asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" TabIndex="11">
                                        </asp:DropDownList>--%>

                                        <dxe:ASPxButtonEdit ID="txtSalesManAgent" runat="server" ReadOnly="true" TabIndex="11" ClientInstanceName="ctxtSalesManAgent" Width="100%">

                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>

                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){SalesManButnClick();}" KeyDown="SalesManbtnKeyDown" />

                                        </dxe:ASPxButtonEdit>


                                        <%--  <dxe:ASPxComboBox ID="SalesManComboBox" runat="server" TabIndex="11" EnableCallbackMode="true" CallbackPageSize="15"
                                            ValueType="System.String" ValueField="cnt_internalid" ClientInstanceName="cSalesManComboBox" Width="92%"
                                            OnItemsRequestedByFilterCondition="SalesManComboBox_OnItemsRequestedByFilterCondition_SQL"
                                            OnItemRequestedByValue="SalesManComboBox_OnItemRequestedByValue_SQL" TextFormatString="{1} [{0}]"
                                                DropDownStyle="DropDown">
                                            <Columns>
                                                <dxe:ListBoxColumn FieldName="uniquename"  Caption="Unique ID"  Width="200px"/>
                                                <dxe:ListBoxColumn FieldName="Name" Caption="Name"  Width="200px"/>
                                            </Columns> 
                                               
                                        </dxe:ASPxComboBox>--%>
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
                                            <%-- <dxe:ASPxComboBox ID="CmbCurrency" EnableIncrementalFiltering="True" ClientInstanceName="cCmbCurrency"
                            DataSourceID="SqlCurrencyBind"
                            TextField="Currency_AlphaCode" ValueField="Currency_ID" SelectedIndex="0"
                            runat="server" ValueType="System.String" EnableSynchronization="True" Width="100%" CssClass="pull-left">
                            <clientsideevents valuechanged="function(s,e){Currency_Rate()}"></clientsideevents>
                        </dxe:ASPxComboBox>--%>
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

                                    <div class="col-md-2 lblmTop8" id="divposGst">
                                        <dxe:ASPxLabel ID="lbl_PosForGst" runat="server" Text="Place Of Supply [GST]">
                                        </dxe:ASPxLabel>
                                        <span style="color: red">*</span>
                                        <dxe:ASPxComboBox ID="ddl_PosGst" runat="server" ValueType="System.String" Width="100%" ClientInstanceName="cddl_PosGst" TabIndex="18">
                                            <%-- <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulatePosGst(e)}" />--%>
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
                                        <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                                        </dxe:ASPxLabel>
                                        <%--<label id="lblProject" runat="server">Project</label>--%>
                                        <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSalesChallan"
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
                                        <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesChallan" runat="server" OnSelecting="EntityServerModeDataSalesChallan_Selecting"
                                            ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                    </div>
                                    <div class="col-md-4">
                                        <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
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

                                        <%-- Rev sanchita [  Settings-HorizontalScrollBarMode added ]--%>
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
                                            Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsPager-Mode="ShowAllRecords" Settings-HorizontalScrollBarMode="Visible">
                                            <SettingsPager Visible="false"></SettingsPager>
                                            <Columns>
                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30px" VisibleIndex="0" Caption=" ">
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
                                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="20px">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Order_Num" ReadOnly="True" Width="120px" VisibleIndex="2">
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="120">


                                                    <PropertiesButtonEdit>
                                                        <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>
                                                <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" VisibleIndex="23" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <%--Batch Product Popup End--%>

                                                <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4" ReadOnly="True" Width="210px">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewCommandColumn VisibleIndex="5" Caption="Addl. Desc." Width="210px">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc" Image-Url="/assests/images/more.png" Image-ToolTip="Warehouse">
                                                            <Image ToolTip="Warehouse" Url="/assests/images/more.png">
                                                            </Image>
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>


                                                <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="6" Width="70px" PropertiesTextEdit-MaxLength="14" ReadOnly="true">
                                                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                        <%-- Rev Rajdip  <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityGotFocus" /> ENd Rev Rajdip--%>
                                                        <ClientSideEvents TextChanged="QuantityLostFocusForMultoUOM" GotFocus="QuantityGotFocusForMultoUOM" />
                                                        <ClientSideEvents />
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="7" ReadOnly="true" Width="80px">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewCommandColumn VisibleIndex="8" Caption="Multi UOM" Width="70px">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>

                                                <%--  Manis 24428--%>
                                                <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="Order_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="9" Width="80px" ReadOnly="true">
                                                    <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                        <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />
                                                        <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                        <Style HorizontalAlign="Right">
                                                            </Style>
                                                    </PropertiesTextEdit>
                                                    <CellStyle HorizontalAlign="Right">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="Order_AltUOM" ReadOnly="true" VisibleIndex="10" Width="80px">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <%--  Manis End 24428--%>


                                                <%--Caption="Warehouse"--%>
                                                <dxe:GridViewCommandColumn Width="80px" VisibleIndex="11" Caption="Stk Details">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>

                                                <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Price" VisibleIndex="12" Width="60px">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                        <ClientSideEvents LostFocus="SalePriceTextChange" GotFocus="SalesPriceGotFocus" />
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="13" Width="80px" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                        <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" />
                                                        <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountGotFocus" />
                                                        <Style HorizontalAlign="Right">
                                                            </Style>
                                                    </PropertiesTextEdit>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="14" Width="70px" ReadOnly="true">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <%--   <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="10" Width="75">
                                                    <PropertiesButtonEdit>
                                                        <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" />
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>--%>

                                                <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="15" Width="75px" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesButtonEdit>
                                                        <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" LostFocus="Taxlostfocus" />
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                        <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </PropertiesButtonEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataButtonEditColumn>

                                                <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="16" Width="80px" ReadOnly="true">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                        <%-- Rev Rajdip For Running Total --%>
                                                        <ClientSideEvents GotFocus="TotalAmountgotfocus" />
                                                        <%-- End Rev Rajdip --%>
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <%--<ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="17" Width="100px" ReadOnly="false">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Left">

                                                        <Style HorizontalAlign="Left">
                                                            </Style>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="40px" VisibleIndex="18" Caption=" ">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>
                                                <dxe:GridViewDataTextColumn FieldName="StockQuantity" Caption="Stock Qty" VisibleIndex="19" Width="0">
                                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="StockUOM" Caption="Stock UOM" VisibleIndex="20" ReadOnly="true" Width="0">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Quotation No" CellStyle-CssClass="hide" FilterCellStyle-CssClass="hide" EditCellStyle-CssClass="hide" EditFormCaptionStyle-CssClass="hide" FooterCellStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" FieldName="Quotation_No" Width="0" HeaderStyle-CssClass="hide" VisibleIndex="21">
                                                    <PropertiesTextEdit NullTextStyle-CssClass="hide" ReadOnlyStyle-CssClass="hide" Style-CssClass="hide">

                                                        <NullTextStyle CssClass="hide"></NullTextStyle>

                                                        <ReadOnlyStyle CssClass="hide"></ReadOnlyStyle>

                                                        <Style CssClass="hide"></Style>

                                                    </PropertiesTextEdit>
                                                    <HeaderStyle CssClass="hide" />
                                                    <CellStyle CssClass="hide">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <%--          <dxe:GridViewDataTextColumn FieldName="DetailsId" Caption="Details ID" VisibleIndex="16" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                <dxe:GridViewDataTextColumn FieldName="DetailsId" Caption="hidden Field Id" VisibleIndex="22" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
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
                                    <%-- Rev Rajdip --%>
                                    <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                        <ul>
                                            <li class="clsbnrLblTotalQty">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
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
                                                                    <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Running Balance" ClientInstanceName="cbnrLblInvVal" />
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
                                    <div class="col-md-12">
                                        <asp:Label ID="ClientShowMsg" runat="server" Text="Already Delivered." CssClass="msgStyle" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                        <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & N&#818;ew" UseSubmitBehavior="false" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <%--  <asp:Button ID="ASPxButton2" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />--%>

                                        <dxe:ASPxButton ID="ASPxButton12" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" UseSubmitBehavior="false" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                            <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <asp:Button ID="Button1" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" UseSubmitBehavior="false" />

                                        <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" UseSubmitBehavior="false" meta:resourcekey="btnSaveRecordsResource1">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                        </dxe:ASPxButton>
                                        <%-- onclick=""--%>
                                        <a href="javascript:void(0);" id="btnAddNew" runat="server" class="btn btn-primary" style="display: none"><span><u>A</u>ttachment(s)</span></a>
                                        <%--<dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[A]ttachment(s)" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                        <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                        </dxe:ASPxButton>--%>
                                        <%--<a class="btn btn-primary" href="javascript:void(0);" onclick="fn_PopOpen()"><span><u>B</u>illing/Shipping</span> </a>--%>
                                        <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                        <asp:HiddenField ID="hfControlData" runat="server" />
                                        <%--<uc1:ucVehicleDriverControl runat="server" ID="ucVehicleDriverControl" />--%>
                                        <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                        <ucOTC:OtherTermsAndCondition runat="server" ID="OtherTermsAndCondition" />
                                        <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                                        <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                        <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="SC" />


                                        <asp:HiddenField runat="server" ID="hfOtherTermsConditionData" />
                                        <asp:HiddenField runat="server" ID="hfOtherTermsConditionDocType" Value="SC" />
                                    </div>
                                </div>
                            </dxe:ContentControl>
                        </ContentCollection>
                        <%--test generel--%>
                    </dxe:TabPage>
                    <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping" TabStyle-CssClass="bilingTab">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <%--        <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server" ContentUrl="AddArea_PopUp.aspx"
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


        <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
        </dxe:ASPxCallbackPanel>

        <%--<asp:SqlDataSource ID="CountrySelect" runat="server"
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
                    <dxe:ASPxGridView runat="server" KeyFieldName="Key_UniqueId" ClientInstanceName="cgridproducts" ID="grid_Products" OnDataBinding="grid_Products_DataBinding"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False" OnHtmlRowCreated="aspxGridProduct_HtmlRowCreated"
                        OnRowInserting="Productgrid_RowInserting" OnRowUpdating="Productgrid_RowUpdating" OnRowDeleting="Productgrid_RowDeleting" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                        <%-- <Settings VerticalScrollableHeight="450" VerticalScrollBarMode="Auto"></Settings>--%>
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " VisibleIndex="0" />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="45" ReadOnly="true" Caption="Sl. No.">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Product_Shortname" ReadOnly="true" Width="100" Caption="Product Name">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Description" Width="200" ReadOnly="true" Caption="Product Description">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Quotation_No" ReadOnly="true" Caption="Quotation Id" Width="0">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="OrderDetails_Id" ReadOnly="true" Caption="Quotation_U" Width="0">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quotation_Num" Width="90" ReadOnly="true" Caption="Order No">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                <Settings AutoFilterCondition="Contains" />
                                <PropertiesTextEdit>
                                    <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsDataSecurity AllowEdit="true" />
                        <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>

                        <%--<SettingsEditing Mode="Batch">
                                            <BatchEditSettings EditMode="row" />
                                        </SettingsEditing>--%>
                        <ClientSideEvents EndCallback="cgridProducts_EndCallBack " />
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <asp:Button ID="Button2" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <%-- End--%>
        <%--Sudip--%>

        <div class="PopUpArea">
            <asp:HiddenField runat="server" ID="hdnuomFactor" />
            <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
            <asp:HiddenField runat="server" ID="hdnisOverideConvertion" />
            <asp:HiddenField runat="server" ID="hdnIsOverride" />
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
            <asp:HiddenField runat="server" ID="hdnpackingqty" />
            <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
            <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
            <%--Rev Bapi--%>
            <asp:HiddenField ID="hdProductID" runat="server" />
            <%--End Rev Bapi--%>
            <%-- Rev work start 28.06.2022 Mantise no:24949--%>
            <asp:HiddenField ID="hdnSettings" runat="server"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="ProductMinPrice" />
            <asp:HiddenField runat="server" ID="ProductMaxPrice" />
            <%-- Rev work close 28.06.2022 Mantise no:24949--%>
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
                                            <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" DisplayFormatString="0.00" ClientInstanceName="ctxtGstCstVatCharge" TabIndex="3" ReadOnly="true" Text="0.00"
                                                runat="server" Width="100%">
                                                <MaskSettings Mask="&lt;-999999999..999999999g&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                    <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" UseSubmitBehavior="false">
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
                                    <%--Rev Rajdip--%>
                                    <li>
                                        <div class="lblHolder" id="divuom">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td><span><b>UOM Conversion: </b></span></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%--<span id="lbluomfactor"></span>--%>
                                                            <asp:Label ID="lbluomfactor1" runat="server"></asp:Label>
                                                            <%--    <div id="lbluomfactor1"></div>--%>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                    <%--End Rev rajdip--%>
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
                                <%-- <div class="clear:both"></div>--%>
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
                                            <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" ClientSideEvents-GotFocus="QuantityGotFocus" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                <%--Rev Rajdip <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />End Rev Rajdip--%>
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
                                        <%-- Rev Rajdip --%>
                                        <dxe:GridViewDataTextColumn Caption="Alt. Qty" FieldName="AltQty"
                                            VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>
                                        <%-- Rev Rajdip --%>
                                        <%--  <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="AltUOM"
                                            VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>--%>
                                        <%--End Rev Rajdip --%>
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
                                    <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
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

            <%-- kaushik 20-2-2017--%>
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
            <%--kaushik 24-2-2017 --%>
            <asp:HiddenField runat="server" ID="IsUdfpresent" />
            <%--kaushik 24-2-2017--%>
        </div>
        <%--    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
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
            </dxe:ASPxCallbackPanel>--%>
        <%--End Sudip--%>

        <%--Debu Section--%>


        <%--Debu Section End--%>
        <%-- </asp:Panel>--%>

        <!--Customer Modal For sales Challan -->
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
                        <%--Rev work start 24.06.2022 mantise no:0024987 --%>
                        <%--<h4 class="modal-title">SalesMan/Agent Search</h4>--%>
                        <h4 class="modal-title" runat="server" id="hs1">Salesman/Agent Search</h4>
                        <%--Rev work close 24.06.2022 mantise no:0024987--%>
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
        <%-- <asp:SqlDataSource ID="SqlCurrencyBind" runat="server" ></asp:SqlDataSource>--%>
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
                                <asp:Button ID="Button4" runat="server" Text="Ok" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
                                <asp:Button ID="Button5" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
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

    <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <%--<asp:SqlDataSource ID="CustomerDataSource" runat="server"  />
    <asp:SqlDataSource runat="server" ID="ProductDataSource" 
        SelectCommand="prc_SalesCRM_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetailsChallan" />

            
            <asp:ControlParameter Name="IsInventory" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddlInventory" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>--%>
    <%--   <asp:SqlDataSource ID="SqlSchematype" runat="server" 
           SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='10' and financial_year_id IN(select FinYear_ID from Master_FinYear where FinYear_Code=@year) and Branch=@userbranch and comapanyInt=@company)) as X Order By ID ASC">
       <SelectParameters>
           <asp:sessionparameter name="userbranch" sessionfield="userbranch" type="string" />
              <asp:sessionparameter name="company" sessionfield="LastCompany1" type="string" />
              <asp:sessionparameter name="year" sessionfield="LastFinYear1" type="string" />
           
       </SelectParameters>
   </asp:SqlDataSource>--%>
    <%--Batch Product Popup End--%>
    <asp:HiddenField ID="hdnmodeId" runat="server" />

    <%--  <asp:SqlDataSource ID="sqlQuationList" runat="server"  
        SelectCommand="prc_GetQuotationOnSalesOrder" 
        SelectCommandType="StoredProcedure" 
       >     
      <SelectParameters>
           <asp:Parameter Name="Status" Type="String"   />
          </SelectParameters>
    </asp:SqlDataSource>--%>

    <%--  <asp:SqlDataSource ID="sqlQuationList" runat="server" 
        SelectCommand="select ttq.Quote_Id,ttq.Quote_Number,IsNull(CONVERT(VARCHAR(10), ttq.Quote_Date, 103),'') as Quote_Date	 ,case when( tmc.cnt_middleName is null  or tmc.cnt_middleName='') then isnull(tmc.cnt_firstName,'')+' ' +isnull(tmc.cnt_LastName,'')+' ' else   isnull(tmc.cnt_firstName,'')+' '+ isnull(tmc.cnt_middleName,'')+' ' +isnull(tmc.cnt_LastName,'')+' ' end as name from tbl_trans_Quotation  ttq left join tbl_master_contact tmc on ttq.Customer_Id=tmc.cnt_internalId where ttq.Quote_Number is not null and ttq.Quote_Number <>' '"></asp:SqlDataSource>--%>
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


    <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="900px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
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
                                                <%--Rev Sanchita--%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" />--%>
                                                <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onchange="CalcBaseRate()" />
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
                                <%--Mantis Issue 24428--%>
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
                                <%--End of Mantis Issue 24428--%>
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
                                                <%--Mantis Issue 24428--%>
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Mantis Issue 24428--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24428--%>
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
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>


                                </td>
                                <%--End of Mantis Issue 24428--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="function(s, e) { if(!document.getElementById('myCheck').checked)  {SaveMultiUOM();}}" />
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
                                <%--Mantis Issue 24428--%>
                                <dxe:GridViewDataTextColumn Caption="MultiUOMSR No"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" Width="0px">
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24428--%>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>


                                <%--Mantis Issue 24428 --%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24428 --%>



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

                                <%--Mantis Issue 24428--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24428 --%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>','<%#Eval("DetailsId") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>
                                        <%--Mantis Issue 24428 --%>

                                        <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                        <%--End of Mantis Issue 24428 --%>
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
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="false">
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
    <asp:HiddenField ID="hdnProdModeval" runat="server" />

    <%-- Rev Rajdip For Customer Mapping With Sales Man --%>
    <dxe:ASPxPopupControl ID="ASPxPopupSalesman" runat="server" ClientInstanceName="cPopup_salesman"
        Width="200px" HeaderText="SalesMan List" PopupHorizontalAlign="WindowCenter"
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
                                                <label>Salesman</label>
                                            </div>
                                            <div>
                                                <dxe:ASPxComboBox ID="ddlsalesmanmapped" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cddlsalesmanmapped" Font-Size="12px">
                                                    <%-- <ClientSideEvents EndCallback="cmbContactPersonEndCall" GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />--%>
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <dxe:ASPxButton ID="btnselectsalesman" ClientInstanceName="cbtn_btnselectsalesman" runat="server" AutoPostBack="False" Text="Map Salesman" UseSubmitBehavior="false" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {Set_MappedSalesMan();}" />

                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%-- End Rev Rajdip --%>

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


    <asp:HiddenField ID="hdnInvoiceTag" runat="server" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnTagType" runat="server" />
    <asp:HiddenField runat="server" ID="hdnQty" />
    <asp:HiddenField runat="server" ID="hdnEntityType" />
    <asp:HiddenField runat="server" ID="hdAddOrEdit" />
    <asp:HiddenField runat="server" ID="hdnPricingDetail" />
</asp:Content>
