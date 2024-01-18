<%--=======================================================Revision History=========================================================================
 1.0     Priti    V2.0.40  06-10-2023     	0026854: Data Freeze Required for Project Sale Invoice & Project Purchase Invoice
 2.0     Priti    V2.0.42  11-01-2024       Mantis : 0027050 A settings is required for the Duplicates Items Allowed or not in the Transaction Module.

=========================================================End Revision History========================================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ProjectPurchaseChallan.aspx.cs" Inherits="ERP.OMS.Management.Activities.ProjectPurchaseChallan" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../../assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.9.0/css/all.css" rel="stylesheet" />
    <link href="../../../assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <%-- <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>--%>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelPurchase.js?var=1.0"></script>
    <script type='text/javascript'>
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
        var SecondUOM = [];
        var SecondUOMProductId = "";
    </script>

    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    <script src="JS/ProductStockIN.js?v00000.008"></script>


    <style type="text/css">
        #grid_DXStatus {
            display: none !important;
        }

        #aspxGridTax_DXStatus {
            display: none !important;
        }


        #gridTax_DXStatus {
            display: none !important;
        }

        #grid_DXMainTable > tbody > tr > td:last-child, #grid_DXMainTable > tbody > tr > td:nth-child(25) {
            display: none !important;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .mbot5 .col-md-8 {
            margin-bottom: 5px;
        }

        .pullleftClass {
            position: absolute;
            right: -3px;
            top: 18px;
        }

        .EWayBillNumber {
            position: absolute;
            right: -3px;
            top: 18px;
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

        .dynamicPopupTbl > thead > tr > th, .dynamicPopupTbl > tbody > tr > td {
            padding: 5px 8px !important;
        }

        .dynamicPopupTbl > tbody > tr > td {
            cursor: pointer;
        }

        .dynamicPopupTbl.back > thead > tr > th {
            background: #4e64a6;
            color: #fff;
        }

        .dynamicPopupTbl.back > tbody > tr > td {
            background: #fff;
        }

        .dynamicPopupTbl > tbody > tr > td input {
            border: none !important;
            cursor: pointer;
            background: transparent !important;
        }

        .focusrow {
            background-color: #0000ff3d;
        }

        .HeaderStyle {
            background-color: #180771d9;
            color: #f5f5f5;
        }

        .gridStatic {
            margin-top: 25px;
        }

            .gridStatic .dynamicPopupTbl {
                width: 100%;
            }

        .bodBot {
            border-bottom: 1px solid #ccc;
            padding-bottom: 10px;
        }

        table.scroll {
            /* width: 100%; */ /* Optional */
            /* border-collapse: collapse; */
            border-spacing: 0;
            width: 100%;
        }

            table.scroll tbody,
            table.scroll thead {
                display: block;
            }

        thead tr th {
            height: 30px;
            line-height: 30px;
            /* text-align: left; */
        }

        table.scroll tbody {
            height: 250px;
            overflow-y: auto;
            overflow-x: hidden;
        }
    </style>
    <script>
        // Rev 1.0
        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= cPLQuoteDate.GetDate()) && (cPLQuoteDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
            }
        }
        // End of Rev 1.0
        $(document).ready(function () {

            if($("#hdnPageStatus").val() !="ADD")
            {
                var dt = new Date();
                if ($("#hdnBackdateddate").val() != "0" && $("#hdnBackdateddate").val() != "") {
                    cPLQuoteDate.SetEnabled(true)
                    var Days = $("#hdnBackdateddate").val();
                    var today = cPLQuoteDate.GetDate();
                    var newdate = cPLQuoteDate.GetDate();
                    newdate.setDate(today.getDate() - Math.round(Days));
                    if ($("#hdnTagDateForbackdated").val() != "")
                    {
                        if (new Date($("#hdnTagDateForbackdated").val()) > newdate) {
                            cPLQuoteDate.SetMinDate(new Date($("#hdnTagDateForbackdated").val()));
                            cPLQuoteDate.SetMaxDate(dt)
                        }
                        else {
                            cPLQuoteDate.SetMinDate(newdate);
                            cPLQuoteDate.SetMaxDate(dt);
                        }
                    }
                    else
                    {
                        cPLQuoteDate.SetMinDate(newdate);
                        cPLQuoteDate.SetMaxDate(dt);
                    }
                }
            }



            var mode = $('#<%=hdnADDEditMode.ClientID %>').val();
            if (mode == 'Edit') {
                if ($("#hdnCustomerId").val() != "")
                {
                    var VendorID = $("#hdnCustomerId").val();
                    SetEntityType(VendorID);
                }
            }
        });

        function ChkDataDigitCount(e) {
            var data = $(e).val();
            $(e).val(parseFloat(data).toFixed(4));
        }

        function ChangePackingByQuantityinjs() {
            if($("#hdnShowUOMConversionInEntry").val()=="1")
            {
                var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                var SpliteDetails = Productdetails.split("||@||");
                var otherdet = {};
                var ProductID = Productdetails.split("||@||")[0];
                otherdet.ProductID = ProductID;
                if (Productdetails != "") {
                    $.ajax({
                        type: "POST",
                        url: "ProjectPurchaseChallan.aspx/GetPackingQuantityWarehouse",
                        data: JSON.stringify(otherdet),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {

                            if (msg.d.length != 0) {
                                var packingQuantity = msg.d[0].packing_quantity;
                                var sProduct_quantity = msg.d[0].sProduct_quantity;
                                var isOverideConvertion = msg.d[0].isOverideConvertion;
                            }
                            else {
                                var packingQuantity = 0;
                                var sProduct_quantity = 0;
                                var isOverideConvertion = 0;
                            }
                            var uomfactor = 0
                            if (sProduct_quantity != 0 && packingQuantity != 0) {
                                uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                                $('#hdnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                            }
                            else {
                                $('#hdnuomFactor').val(0);
                            }

                            $('#hdnpackingqty').val(packingQuantity);
                            $('#hdnisOverideConvertion').val(isOverideConvertion);
                            //var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
                            //var Qty = $("#UOMQuantity").val();
                            //var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

                            ////$("#AltUOMQuantity").val(calcQuantity);

                            //cAltUOMQuantity.SetValue(calcQuantity);

                        }
                    });
                }

                var Quantity = ctxtQty.GetValue();
                var packing = $('#txtAltQty').val();
                if (packing == null || packing == '') {
                    $('#txtAltQty').val(parseFloat(0).toFixed(4));
                    packing = $('#txtAltQty').val();
                }

                if (Quantity == null || Quantity == '') {
                    $(e).val(parseFloat(0).toFixed(4));
                    Quantity = ctxtQty.GetValue();
                }
                var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);

                //Rev Subhra 05-03-2019
                //var calcQuantity = parseFloat(Quantity * packingqty).toFixed(4);
                var uomfac_Qty_to_stock = $('#hdnuomFactor').val();
                //var uomfac_Qty_to_stock = $('#hdnpackingqty').val();
                var calcQuantity = parseFloat(Quantity * uomfac_Qty_to_stock).toFixed(4);
                //End of Rev Subhra 05-03-2019
                //$('#txtAlterQty1').val(calcQuantity);
                ctxtAltQty.SetText(calcQuantity);

                ChkDataDigitCount(Quantity);
            }
        }

        function ChangeQuantityByPacking1() {
            if($("#hdnShowUOMConversionInEntry").val()=="1")
            {
                var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                var SpliteDetails = Productdetails.split("||@||");
                var otherdet = {};
                var ProductID = Productdetails.split("||@||")[0];
                otherdet.ProductID = ProductID;
                if (Productdetails != "") {
                    $.ajax({
                        type: "POST",
                        url: "ProjectPurchaseChallan.aspx/GetPackingQuantityWarehouse",
                        data: JSON.stringify(otherdet),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {

                            if (msg.d.length != 0) {
                                var packingQuantity = msg.d[0].packing_quantity;
                                var sProduct_quantity = msg.d[0].sProduct_quantity;
                                var isOverideConvertion = msg.d[0].isOverideConvertion;
                            }
                            else {
                                var packingQuantity = 0;
                                var sProduct_quantity = 0;
                                var isOverideConvertion = 0;
                            }
                            var uomfactor = 0
                            if (sProduct_quantity != 0 && packingQuantity != 0) {
                                uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                                $('#hdnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                            }
                            else {
                                $('#hdnuomFactor').val(0);
                            }

                            $('#hdnpackingqty').val(packingQuantity);
                            $('#hdnisOverideConvertion').val(isOverideConvertion);
                            //var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
                            //var Qty = $("#UOMQuantity").val();
                            //var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

                            ////$("#AltUOMQuantity").val(calcQuantity);

                            //cAltUOMQuantity.SetValue(calcQuantity);

                        }
                    })

                }

                var isOverideConvertion = $('#hdnisOverideConvertion').val();
                if (isOverideConvertion == "true") {
                    isOverideConvertion = '1';
                }
                if (isOverideConvertion == '1') {
                    var packing = ctxtAltQty.GetValue();
                    var Quantity = ctxtQty.GetValue();
                    if (packing == null || packing == '') {
                        $(e).val(parseFloat(0).toFixed(4));
                        packing = ctxtAltQty.GetValue();
                    }

                    if (Quantity == null || Quantity == '') {
                        ctxtQty.SetValue(parseFloat(0).toFixed(4));

                        Quantity = ctxtQty.GetValue();
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
                    ctxtQty.SetValue(calcQuantity);
                }
                ChkDataDigitCount(Quantity);
            }
        }

    </script>
    <%--Use for set focus on UOM after press ok on UOM--%>
    <script>
        $(function () {
            $('#UOMModal').on('hide.bs.modal', function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
            });
        });

    </script>
    <%--Use for set focus on UOM after press ok on UOM--%>
    <script>
        var ShouldCheck;
        var _ComponentDetails;

        function disp_prompt(name) {
            if (name == "tab0") {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                $("#divcross").show();
                cContactPerson.Focus();
            }
            if (name == "tab1") {
                var custID = GetObjectID('hdnCustomerId').value;
                $("#divcross").hide();
                page.GetTabByName('General').SetEnabled(false);
                if (custID == null && custID == '') {
                    jAlert('Please select a Vendor');
                    page.SetActiveTabIndex(0);
                    return;
                }
                else {
                    page.SetActiveTabIndex(1);
                }
            }
        }
        function CmbScheme_ValueChange() {
            var val = $("#ddl_numberingScheme").val();

            var schemetypeValue = val;
            var schemetype = schemetypeValue.toString().split('~')[1];
            var schemelength = schemetypeValue.toString().split('~')[2];

            var branchID = (schemetypeValue.toString().split('~')[3] != null) ? schemetypeValue.toString().split('~')[3] : "";
            //Rev Debashis
            var fromdate = (schemetypeValue.toString().split('~')[4] != null) ? schemetypeValue.toString().split('~')[4] : "";
            var todate = (schemetypeValue.toString().split('~')[5] != null) ? schemetypeValue.toString().split('~')[5] : "";

            var dt = new Date();

            cPLQuoteDate.SetDate(dt);

            if (dt < new Date(fromdate)) {
                cPLQuoteDate.SetDate(new Date(fromdate));
            }

            if (dt > new Date(todate)) {
                cPLQuoteDate.SetDate(new Date(todate));
            }


            if($("#hdnBackdateddate").val() !="0" && $("#hdnBackdateddate").val()!="")
            {
                var Days = $("#hdnBackdateddate").val();
                var today = cPLQuoteDate.GetDate();
                var newdate = cPLQuoteDate.GetDate();
                newdate.setDate(today.getDate() - Math.round(Days));
                cPLQuoteDate.SetMinDate(newdate);
                cPLQuoteDate.SetMaxDate(dt);

            }
            else
            {
                cPLQuoteDate.SetMinDate(new Date(fromdate));
                cPLQuoteDate.SetMaxDate(new Date(todate));
            }
            //End of Rev Debashis
            $("#hdnTCBranchId").val(branchID);
            if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
            
            $('#<%=hdnBranchID.ClientID %>').val(branchID);
            if(document.getElementById('btn_TermsCondition'))
                BinducTcBank();

            $('#txtVoucherNo').attr('maxLength', schemelength);

            ctxtCustName.SetText("");
            PosGstId = "";
            cddlPosGstChallan.SetValue(PosGstId);
            GetObjectID('hdnCustomerId').value = "";
            page.tabs[1].SetEnabled(false);

            var schemetypeValue = val;
            var schemetype = schemetypeValue.toString().split('~')[1];
            var schemelength = schemetypeValue.toString().split('~')[2];
            $('#txtVoucherNo').attr('maxLength', schemelength);
            BindWarehouse();

            if (schemetype == '0') {
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                $("#txtVoucherNo").focus();
            }
            else if (schemetype == '1') {
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                $("#MandatoryBillNo").hide();
            }
            else if (schemetype == '2') {
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
            }
            else if (schemetype == 'n') {
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
            }
            else {
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
            }
            //Chinmoy added this line

    SetPurchaseBillingShippingAddress( $('#ddl_Branch').val());
    if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.gridView.Refresh();
}

function callback_InlineRemarks_EndCall(s, e) {

    if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
        $("#txtInlineRemarks").focus();
    }
    else  if (ccallback_InlineRemarks.cpRemarksFinalFocus == "RemarksFinalFocus")
    {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
    else {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
          
}

function FinalRemarks() {


    ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + grid.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');
    cPopup_InlineRemarks.Hide();

}
function fn_PopOpen()
{
    var url = '/OMS/management/Store/Master/ProductPopup.html?var=4.75';
    cPosView.SetContentUrl(url);
    cPosView.RefreshContentUrl();

    cPosView.Show();
          
}

       
function  fn_productSave()
{
    cPosView.Hide();
}

function ParentCustomerOnClose(newCustId, customerName, Unique) {
           

    GetObjectID('hdnCustomerId').value = newCustId;

    AspxDirectAddCustPopup.Hide();
    ctxtShipToPartyShippingAdd.SetText('');
    if (newCustId != "") {
        ctxtCustName.SetText(customerName);
        SetCustomer(newCustId, customerName);
    }
           
}

function AddVendorClick() {
    //var isLighterPage = $("#hidIsLigherContactPage").val();
    //// alert(isLighterPage);
    //if (isLighterPage == 1) {
    var url = '/OMS/management/Master/vendorPopup.html?var=1.1.4.4';
    AspxDirectAddCustPopup.SetContentUrl(url);
           
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



function BindWarehouse(){
    var objectToPass = {}
    objectToPass.Branch = $('#ddl_Branch').val();

    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetWarehouse",
        data: JSON.stringify(objectToPass),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var ddlWarehouse = $("[id*=ddlWarehouse]");
            $.each(r.d, function () {
                ddlWarehouse.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        }
    });
}


function txtBillNo_TextChanged() {
    var VoucherNo = document.getElementById("txtVoucherNo").value;

    $.ajax({
        type: "POST",
        url: "PurchaseChallan_Add.aspx/CheckUniqueName",
        data: JSON.stringify({ VoucherNo: VoucherNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d;

            if (data == true) {
                $("#MandatoryBillNo").show();

                document.getElementById("txtVoucherNo").value = '';
                document.getElementById("txtVoucherNo").focus();
            }
            else {
                $("#MandatoryBillNo").hide();
            }
        }
    });
}
//Below function added by chinmoy
function IfVendorGstInIsBlank()
{
    cddl_AmountAre.SetValue("3");
    PopulateGSTCSTVAT();
    cddl_AmountAre.SetEnabled(false);

}

function cmbContactPersonEndCall(s, e) {
    cddl_AmountAre.SetEnabled(true);
    var comboitem = cddl_AmountAre.FindItemByValue('4');
    if (comboitem != undefined && comboitem != null) {
        cddl_AmountAre.RemoveItem(comboitem.index);
    }

    if (cContactPerson.cpGSTN == "No") {
        if (cContactPerson.cpcountry != null && cContactPerson.cpcountry != '') {
            if (cContactPerson.cpcountry != '1') {
                cddl_AmountAre.AddItem("Import", "4");
                cddl_AmountAre.SetValue(4);
                cddl_AmountAre.SetEnabled(false);
            }

                //Added By Chinmoy
            else if($('#hfVendorGSTIN').val()=='')
            {
                IfVendorGstInIsBlank();
            }

            else if($('#hfVendorGSTIN').val()!='') 
            {
                cddl_AmountAre.SetValue("1");
            }
            //end
            //else {
            //    cddl_AmountAre.SetValue(1);
            //}
        }
        //else {
        //    cddl_AmountAre.SetValue(1);
        //}
    }
    //else {
    //    cddl_AmountAre.SetValue(1);
    //}

    cContactPerson.cpGSTN = null;
    cContactPerson.cpcountry = null;
}

function SetFocusonDemand(e) {
    var key = cddl_AmountAre.GetValue();
    //if (key == '1' || key == '3') {
    if (key == '1' || key == '2' || key == '3' || key == '4') {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(0, 3);
        }
    }
    //else if (key == '2') {
    // cddlVatGstCst.Focus();
    //}
    cddlPosGstChallan.Focus();
}

function PopulateGSTCSTVAT(e) {
    var key = cddl_AmountAre.GetValue();

    if (key == 1) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cbtn_SaveRecords.SetVisible(true);

        grid.GetEditor('ProductName').Focus();
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(0, 3);
        }

    }
    else if (key == 2) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cbtn_SaveRecords.SetVisible(true);

        grid.GetEditor('ProductName').Focus();
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(0, 3);
        }
    }
    else if (key == 3) {
        grid.GetEditor('TaxAmount').SetEnabled(false);
        cbtn_SaveRecords.SetVisible(false);

        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(0, 3);
        }
    }
}

function GlobalBillingShippingEndCallBack() {
    if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
        cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
        var VendorID = document.getElementById('hdnCustomerId').value;

        if (VendorID != null) {
            cContactPerson.PerformCallback('BindContactPerson~' + VendorID);
        }
    }
}

function GetPCDateFormat(today) {
    if (today != "") {
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}

function taggingListKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function taggingListButnClick(s, e) {
    var VendorID = GetObjectID('hdnCustomerId').value;

    if (VendorID != null) {
        ctaggingGrid.PerformCallback('BindComponentGrid');
        cpopup_taggingGrid.Show();
    }
}

//Chinmoy added below function
function validateOrderwithAmountAre(){
    //Check Multiple Row amount are selectedor not

    var selectedKeys = ctaggingGrid.GetSelectedKeysOnPage();
    var ammountsAreOrder="";
    if(selectedKeys.length>0){
        for(var loopcount = 0 ; loopcount<ctaggingGrid.GetVisibleRowsOnPage();loopcount++)
        {
            
            var nowselectedKey = ctaggingGrid.GetRowKey(loopcount);

            var found = selectedKeys.find(function(element) {
                return element == nowselectedKey;
            });

            if(found){
                if(ammountsAreOrder !="" && ammountsAreOrder !=ctaggingGrid.GetRow(loopcount).children[5].innerText){
                    jAlert("Unable to procceed. Amount are for the selected order(s) are different");
                    return false;
                }
                else
                    ammountsAreOrder= ctaggingGrid.GetRow(loopcount).children[5].innerText;
            }
            
        }
    
    }

    return true;
}
//End
//Chinmoy edited below function
var SimilarProjectStatus = "0";

function SimilarProjetcheck(quote_Id,Doctype)
{
    $.ajax({
        type: "POST",
        url: "ProjectPurchaseChallan.aspx/DocWiseSimilarProjectCheck",
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
function QuotationNumberChanged() {

    var OrderData = ctaggingGrid.GetSelectedKeysOnPage();

    var quotetag_Id =ctaggingGrid.GetSelectedKeysOnPage();

    if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {
        var Doctype = "Purchase_Order";
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

        SimilarProjetcheck(quote_Id,Doctype);
    }
    if (SimilarProjectStatus != "-1") {
        if(validateOrderwithAmountAre()==false)
        {
            cpopup_taggingGrid.Hide();
            cProductsPopup.Hide();
        }
        else if(OrderData==0)
        {
            cpopup_taggingGrid.Hide();
        }
        else if(OrderData!=0 && validateOrderwithAmountAre()==true)
        {
            cgridproducts.PerformCallback('BindProductsDetails');
            cpopup_taggingGrid.Hide();
            cProductsPopup.Show();
        }
    }

}
//End

function gridProducts_EndCallback(s, e) {
    if (cgridproducts.cpComponentDetails) {
        _ComponentDetails=cgridproducts.cpComponentDetails;
        cgridproducts.cpComponentDetails = null;

        clookup_Project.gridView.Refresh();
        var  _cpProjectID=_ComponentDetails.split('~')[2];
        clookup_Project.gridView.SelectItemsByKey(_cpProjectID);
        if (_cpProjectID>0) {
            //clookup_Project.gridView.SetEnabled=false;
            clookup_Project.SetEnabled(false);
        }
        else {
            clookup_Project.SetEnabled(true);
        }
    }
}

function ChangeState(value) {
    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}

function Tag_ChangeState(value) {
    ctaggingGrid.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}


//Chinmoy added this function
//Start
var Address = [];
var ReturnDetails;
function  GetPurchaseOrderDocumentAddress(OrderId)
{
    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "ProjectPurchaseChallan.aspx/PurchaseOrderDocumentAddress",
            data: JSON.stringify({OrderId:OrderId}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                Address = msg.d;
                PurchaseOrerBillingShippingAddress(Address);

            }
        });
    }
}      


function PurchaseOrerBillingShippingAddress(ReturnDetails) {

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
       

        //cddlPosGstChallan.SetValue(BillingDetails[0].PosForGst);
        PosGstId = BillingDetails[0].PosForGst;


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
        // cddlPosGstChallan.SetValue(ShippingDetails[0].PosForGst);
        PosGstId = ShippingDetails[0].PosForGst;
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

//End

function BindOrderProjectdata(OrderId) {
    debugger;
    var OtherDetail = {};

    OtherDetail.OrderId = OrderId;
  


    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "ProjectPurchaseChallan.aspx/SetProjectCode",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var Code = msg.d;

                clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                clookup_Project.SetEnabled(false);
            }
        });
        var projID = clookup_Project.GetValue();

        $.ajax({
            type: "POST",
            url: 'ProjectPurchaseChallan.aspx/getHierarchyID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ProjID: projID }),
            success: function (msg) {
                var data = msg.d;
                $("#ddlHierarchy").val(data);
            }
        });


    }
}


function PerformCallToGridBind() {
    var OrderTaggingData = cgridproducts.GetSelectedKeysOnPage();
    GetPurchaseForGstValue();
    cddlPosGstChallan.SetEnabled(false);
    if(OrderTaggingData==0){
        cProductsPopup.Hide();
    }
    else{
        grid.PerformCallback('BindGridOnQuotation' + '~' + '@');

        //#### Transporter & Billing/Shipping Tagging ####
        var quote_Id = ctaggingGrid.GetSelectedKeysOnPage();

        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
            callTransporterControl(quote_Id[0], 'PO');
        }

        if (quote_Id.length > 0) {
            //Chinmoy added below line

            GetPurchaseOrderDocumentAddress(quote_Id[0]);
            //BSDocTagging(quote_Id[0], 'PO');
        }

        if (quote_Id.length > 0) {
            BindOrderProjectdata(quote_Id[0]);
        }
        //#### Transporter & Billing/Shipping Tagging ####

        //#### TC Control Tagging ####
        if ($("#btn_TermsCondition").is(":visible")) {
            callTCControl(quote_Id[0], 'PO');
        }
        //#### TC Control Tagging ####

        if (quote_Id.length > 0) {
            var ComponentDetails = _ComponentDetails.split("~");
            cgridproducts.cpComponentDetails = null;

            var ComponentNumber = ComponentDetails[0];
            var ComponentDate = ComponentDetails[1];
        
            ctaggingList.SetValue(ComponentNumber);
            cPLQADate.SetValue(ComponentDate);
        }

        cProductsPopup.Hide();
    }
}

function ddl_Currency_Rate_Change() {
    var Campany_ID = '<%=Session["LastCompany"]%>';
    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
    var basedCurrency = LocalCurrency.split("~");
    var Currency_ID = $("#ddl_Currency").val();


    if (Currency_ID == basedCurrency[0]) {
        ctxtRate.SetValue("0");
        ctxtRate.SetEnabled(false);
    }
    else {
        $.ajax({
            type: "POST",
            url: "PurchaseChallan_Add.aspx/GetRate",
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


    </script>
    <script>
        $(document).ready(function(){
            // Change the selector if needed

            if($("#hdnShowUOMConversionInEntry").val()=="1")
            {
                $('#_Altdiv_Quantity').attr('style', 'display:block');
                $('#dv_AltUOM').attr('style', 'display:block');
                
            }
            else
            {
                $('#_Altdiv_Quantity').attr('style', 'display:none');
                $('#dv_AltUOM').attr('style', 'display:none');
            }


            if( $('#hdnPageStatus').val() == "EDIT")
            {
                clookup_Project.SetEnabled(false);
                PopulateChallanPosGst();
                cddlPosGstChallan.SetEnabled(false);
                LoadCustomerBillingShippingAddress(GetObjectID('hdnCustomerId').value);
                LoadBranchAddressInEditMode($('#ddl_Branch').val());
            }

            if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                $("#btnSecondUOM").removeClass('hide');
            }
            else {
                $("#btnSecondUOM").addClass('hide');
            }

            var $table = $('table.scroll'),
                $bodyCells = $table.find('tbody tr:first').children(),
                colWidth;

            // Adjust the width of thead cells when window resizes
            $(window).resize(function() {
                // Get the tbody columns width array
                colWidth = $bodyCells.map(function() {
                    return $(this).width();
                }).get();
    
                // Set the width of thead columns
                $table.find('thead tr').children().each(function(i, v) {
                    $(v).width(colWidth[i]);
                });    
            }).resize(); // Trigger resize handler
        });
    </script>
    <script>
        var globalRowIndex;
        var rowEditCtrl;
        var Stock_EditID = "0";
        var TaxOfProduct = [];

        var _GetQuantityValue = "0";
        var _GetPurchasePriceValue = "0";
        var _GetDiscountValue = "0";
        var _GetAmountValue = "0";

        function GridCallBack() {
            grid.PerformCallback('Display');
        }

        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;
        }

        function ProductButnClick(s, e) {
            if (e.buttonIndex == 0) {
                var VendorID = GetObjectID('hdnCustomerId').value;
                if (VendorID != null && VendorID != "") {
                    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                    Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                    Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";

                    setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

                    $('#txtProdSearch').val('');
                    $('#ProductModel').modal('show');
                }
                else {
                    jAlert("Please Select a Vendor", "Alert", function () { ctxtCustName.Focus(); });
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

            var SpliteDetails = Id.split("||@||");

            var Product_ID = Id;
            var Product_Code = SpliteDetails[1];
            var Product_Name = SpliteDetails[2];
            var HSNCode = SpliteDetails[14];
            var Purchase_UOMID = SpliteDetails[6];
            var Purchase_UOM = SpliteDetails[7];
            var Purchase_Price = SpliteDetails[10];
            var IsPackingActive = SpliteDetails[18];
            var Packing_Factor = SpliteDetails[19];
            var Packing_UOM = SpliteDetails[20];
            var Warehousetype = SpliteDetails[21];
            var IsComponent = SpliteDetails[22];
            var ComponentProduct = SpliteDetails[23];

            ctxtCustName.SetEnabled(false);
            cPLQuoteDate.SetEnabled(false);
            cddl_AmountAre.SetEnabled(false);
            cddlPosGstChallan.SetEnabled(false);
            document.getElementById("ddl_numberingScheme").disabled = true;
            document.getElementById("ddlInventory").disabled = true;
            grid.batchEditApi.StartEdit(globalRowIndex);

            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            if (parseFloat(strRate) == 0) {
                Purchase_Price = Purchase_Price;
            }
            else {
                Purchase_Price = Purchase_Price / strRate;
            }

            grid.batchEditApi.StartEdit(globalRowIndex, 4);

            if (grid.GetEditor("ProductID").GetValue() != "" && grid.GetEditor("ProductID").GetValue() != null) {
                var previousProductID = grid.GetEditor("ProductID").GetValue();
                var _previousProductID = previousProductID.split("||@||")[0];

                cDeletePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());  
            }


            Pre_Quantity = (grid.GetEditor("Quantity").GetText() != null) ? grid.GetEditor("Quantity").GetText() : "0";
            Pre_Amt = (grid.GetEditor("TotalAmount").GetText() != null) ? grid.GetEditor("TotalAmount").GetText() : "0";
            Pre_TotalAmt = (grid.GetEditor("NetAmount").GetText() != null) ? grid.GetEditor("NetAmount").GetText() : "0";

            grid.GetEditor("ProductID").SetText(Product_ID);
            grid.GetEditor("ProductName").SetText(Product_Code);
            grid.GetEditor("ProductDiscription").SetText(Product_Name);
            grid.GetEditor("Quantity").SetText("0");
            grid.GetEditor("PurchaseUOM").SetText(Purchase_UOM);
            grid.GetEditor("PurchasePrice").SetText(Purchase_Price);
            grid.GetEditor("Discount").SetText("0");
            grid.GetEditor("TotalAmount").SetText("0");
            grid.GetEditor("TaxAmount").SetText("0");
            grid.GetEditor("NetAmount").SetText("0");
            grid.GetEditor("TotalQty").SetText("0");
            grid.GetEditor("BalanceQty").SetText("0");
            grid.GetEditor("IsComponentProduct").SetText("");
            grid.GetEditor("DocID").SetText("");


            

            Cur_Quantity = "0";
            Cur_Amt = "0";
            Cur_TotalAmt = "0";
            CalculateAmount();



            var _SrlNo = grid.GetEditor("SrlNo").GetValue();
            if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
                var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry:"N" }
                TaxOfProduct.push(ProductTaxes);
                SetFocusAfterProductSelect();
            }
            else {
                $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
                SetFocusAfterProductSelect();
            }
        }

        function SetFocusAfterProductSelect(){
            setTimeout(function () {              
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return;
            }, 200);
        }

        function ProductKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
                s.OnButtonClick(0);
            }
            //if (e.htmlEvent.key == "Tab") {
            //    s.OnButtonClick(0);
            //}
        }

        function OnEndCallback(s, e) {
            var refreshType = document.getElementById('hdnRefreshType').value;
            var pageStatus = document.getElementById('hdnPageStatus').value;

            $('#<%=hdnRefreshType.ClientID %>').val('');

            if (grid.cpinserterrorwarehouse != null) {
                LoadingPanel.Hide();
                grid.batchEditApi.StartEdit(0, 2);
                jAlert(grid.cpinserterrorwarehouse);
                grid.cpinserterrorwarehouse = null;
            }
            else if (grid.cpSaveSuccessOrFail == "outrange") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More Purchase Oder Number as Purchase Order Scheme Exausted.<br />Update The Scheme and Try Again');
            }
            else if (grid.cpSaveSuccessOrFail == "checkPartyInvoice") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Party Invoice must be unique for the selected Vendor.');
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please fill Quantity');
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can not Duplicate Product in the Challan List.');
            }
            else if (grid.cpSaveSuccessOrFail == "nullPurchasePrice") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Purchase Price is Mandatory. Please enter values.');
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateSerial") {
                LoadingPanel.Hide();
                var Msg = grid.cpduplicateSerialMsg;

                grid.cpSaveSuccessOrFail = null;
                grid.cpduplicateSerialMsg = null;
                grid.batchEditApi.StartEdit(0, 2);

                jAlert(Msg);
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                LoadingPanel.Hide();
                grid.batchEditApi.StartEdit(0, 2);
                var SrlNo = grid.cpProductSrlIDCheck;

                grid.cpSaveSuccessOrFail = null;
                grid.cpProductSrlIDCheck = null;
                var msg = "Make sure product quantity are equal with Warehouse quantity for SL No. " + SrlNo;
                jAlert(msg);
            }
                // Rev Mantis Issue 24061
            else if (grid.cpSaveSuccessOrFail == "NetAmountExceed") {
                LoadingPanel.Hide();
                grid.batchEditApi.StartEdit(0, 2);
                var SrlNo = grid.cpProductSrlIDCheck;

                grid.cpSaveSuccessOrFail = null;
                grid.cpProductSrlIDCheck = null;
                jAlert('Net Amount of selected Product from tagged document.<br />Cannot enter Net Amount more than Purchase Order Net Amount .');
            }
                // End of Rev Mantis Issue 24061
            else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
            }
            else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Save as Duplicate Purchase Order Numbe No. Found');
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please try after sometime.');
            }
            else if (grid.cpSaveSuccessOrFail == "ProjectError") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please select project.');
            }
            else if (grid.cpSaveSuccessOrFail == "transactionbeingused") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Transaction exist. cannot be processed.');
            }
            else if (grid.cpSaveSuccessOrFail == "ExceedQuantity") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Tagged product quantity exceeded.Update The quantity and Try Again.');
            }

            else if (grid.cpSaveSuccessOrFail == "Ponotexist") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Cannot Save. Selected Purchase   Order(s) in this document do not exist.');
            }
            else if (grid.cpSaveSuccessOrFail == "stockOut") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 3);
                jAlert('Already stock out for this product.');
            }
            else if (grid.cpSaveSuccessOrFail == "allStockOut") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;

                jAlert("Already stock out for selected products.", 'Alert Dialog: [PurchaseChallan]', function (r) {
                    if (r == true) {
                        window.location.reload();
                    }
                });
            }
            else if (grid.cpSaveSuccessOrFail == "PurchaseOrderMandatory") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Purchase Order is mandatory while save the data.');
            }
            else {
                var PurchaseOrder_Number = grid.cpPurchaseOrderNo;
                var Order_Msg = "Project Purchase Challan No. " + PurchaseOrder_Number + " saved.";
                if (refreshType == "E") {
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else if (grid.cpApproverStatus == "rejected") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }

                    if (PurchaseOrder_Number != "") {


                        jAlert(Order_Msg, 'Alert Dialog: [Purchase Challan]', function (r) {
                            if (r == true) {
                                grid.cpSalesOrderNo = null;
                                window.location.assign("ProjectPurchaseChallanList.aspx");
                            }
                        });

                    }
                    else {
                        window.location.assign("ProjectPurchaseChallanList.aspx");
                    }
                }
                else if (refreshType == "N") {
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else if (grid.cpApproverStatus == "rejected") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    if (PurchaseOrder_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {

                            grid.cpSalesOrderNo = null;
                            if (r == true) {
                                window.location.assign("ProjectPurchaseChallan.aspx?key=ADD&InvType="+$("#hdnChallanType").val());
                            }
                        });
                    }
                    else {
                        window.location.assign("ProjectPurchaseChallan.aspx?key=ADD&InvType="+$("#hdnChallanType").val());
                    }
                }
                else {
                    if (pageStatus == "first") {
                        OnAddNewClick();
                        grid.batchEditApi.EndEdit();
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "update") {
                        OnAddNewClick();
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                   }
           }
   }

    if (grid.cpComponent) {
        if (grid.cpComponent == 'true') {
            grid.cpComponent = null;
            OnAddNewClick();
        }
    }
    
    if (grid.cpTaggingStockData) {
        if (grid.cpTaggingStockData!="") {
            var myObj=grid.cpTaggingStockData;            
            var JObject=JSON.parse(myObj);    
            
            if (JObject.length > 0) {
                for (x in JObject) {
                    JObject[x]["SrlNo"]=parseInt(JObject[x]["SrlNo"]);
                    JObject[x]["LoopID"]=parseInt(JObject[x]["LoopID"]);
                }
            }

            StockOfProduct=JObject;
            grid.cpTaggingStockData=null;
        }
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
        var Tax_Option = RunningSpliteDetails[7];
        var Currency = RunningSpliteDetails[8];
        var Rate = RunningSpliteDetails[9];

        cTaxableAmtval.SetValue(SUM_Amount);
        cTaxAmtval.SetValue(SUM_TaxAmount);
        ctxt_Charges.SetValue(SUM_ChargesAmount);
        cOtherTaxAmtval.SetValue(SUM_ChargesAmount);
        cInvValue.SetValue(SUM_TotalAmount);
        cTotalAmt.SetValue(SUM_TotalAmount);
        cTotalQty.SetValue(SUM_ProductQuantity);

        if(Tax_Option!="") cddl_AmountAre.SetValue(Tax_Option);
        if(Currency!="") document.getElementById('ddl_Currency').value = Currency;
        ctxtRate.SetValue(Rate);
    }

    if (ctaggingList.GetValue() != null && ctaggingList.GetValue()!="") {
        grid.GetEditor('ProductName').SetEnabled(false);

        ctxtCustName.SetEnabled(false);
        cPLQuoteDate.SetEnabled(false);
        cddl_AmountAre.SetEnabled(false);
        document.getElementById("ddl_numberingScheme").disabled = true;
        document.getElementById("ddlInventory").disabled = true;
    }

    cProductsPopup.Hide();
}

function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomAddNewRow') {
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";

        var SpliteDetails = ProductID.split("||@||");
        var IsComponentProduct = SpliteDetails[22];

        if (IsComponentProduct == "Y") {
            var ComponentProduct = SpliteDetails[23];

            var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
            jConfirm(messege, 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.GetEditor("IsComponentProduct").SetValue("Y");
                    $('#<%=hdfIsDelete.ClientID %>').val('C');

                    grid.AddNewRow();
                    grid.UpdateEdit();

                    grid.PerformCallback('Display~fromComponent');
                }
                else {
                    OnAddNewClick();
                }
            });
        }
        else {
            if (ProductID != "") {
                OnAddNewClick();
            }
            else {
                grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            }
        }
    }

    else if (e.buttonID == "CustomaddDescRemarks") {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex,4);
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

    else if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        $('#<%=hdnRefreshType.ClientID %>').val('');

        if (ctaggingList.GetValue() != null) {
            jAlert('Cannot Delete using this button as the Purchase Challan is linked with this Purchase Order.<br /> Click on Plus(+) sign to Add or Delete Product from last column!', 'Alert Dialog: [Delete Challan Products]', function (r) {
            });
        }

        if (noofvisiblerows != "1" && ctaggingList.GetValue() == null) {
            var ProductID = grid.batchEditApi.GetCellValue(e.visibleIndex, 'ProductID');

            if (ProductID != null) {
                Pre_Quantity = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'Quantity') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'Quantity') : "0";
                Pre_Amt = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'TotalAmount') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'TotalAmount') : "0";
                Pre_TotalAmt = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'NetAmount') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'NetAmount') : "0";

                Cur_Quantity = "0";
                Cur_Amt = "0";
                Cur_TotalAmt = "0";
                CalculateAmount();

                grid.DeleteRow(e.visibleIndex);
                $('#<%=hdfIsDelete.ClientID %>').val('D');

                grid.AddNewRow();
                grid.UpdateEdit();

                grid.PerformCallback('Display');
            }
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 8)
        Warehouseindex = index;
        var AltUOMID = "0";
        var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

        if (inventoryType == "C" || inventoryType == "Y" || inventoryType == "B") {
            var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
            var IDs = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var SpliteDetails = IDs.split("||@||");
            var ProductID = SpliteDetails[0];

            if (parseFloat(QuantityValue) == "0") {
                jAlert("Quantity should not be zero !.");
            }
            else {
                if (ProductID != "") {
                    var Product_Name = SpliteDetails[2];
                    var Purchase_UOM = SpliteDetails[7];
                    var Warehousetype = SpliteDetails[21];
                    var serviceURL = "Services/Master.asmx/CheckDuplicateSerial";

                    $('#<%=hdfProductID.ClientID %>').val(ProductID);
                    $('#<%=hdfWarehousetype.ClientID %>').val(Warehousetype);
                    $('#<%=hdfProductSrlNo.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
                    $('#<%=hdfUOM.ClientID %>').val(Purchase_UOM);
                    GetObjectID('hdfServiceURL').value = serviceURL;
                    GetObjectID('hdfBranch').value = $('#ddl_Branch').val();
                    GetObjectID('hdfIsRateExists').value = "N";
                    SecondUOMProductId = ProductID;
                    document.getElementById('<%=lblProductName.ClientID %>').innerHTML = Product_Name;
                    document.getElementById('<%=lblEnteredAmount.ClientID %>').innerHTML = QuantityValue;
                    document.getElementById('<%=lblEnteredUOM.ClientID %>').innerHTML = Purchase_UOM;
                    AltUOMID = SpliteDetails[29];
                    Stock_EditID = "0";
                    //cWarehousePanel.PerformCallback('StockDisplay');

                    if(GetObjectID('IsBarcodeActive').value=="Y"){
                        if (ctaggingList.GetValue()) {
                            StockHeader.style.display = 'none';
                        }
                        else{
                            StockHeader.style.display =  'block';
                        }
                    }
                    else{
                        StockHeader.style.display =  'block';
                    }

                    CreateStock();
                    cPopupWarehouse.Show();
                    if($("#hdnShowUOMConversionInEntry").val()=="1")
                    {
                        var objectToPass = {}
                        var product = $("#hdfProductID").val();
                        objectToPass.ProductID = $("#hdfProductID").val();//hdfProductID.value;

                        $.ajax({
                            type: "POST",
                            url: "../Activities/Services/Master.asmx/GetUom",
                            data: JSON.stringify(objectToPass),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                                var returnObject = msg.d;
                                var UOMId = returnObject.uom_id;
                                var UOMName = returnObject.UOM_Name;
                                if (returnObject) {
                                    SetDataSourceOnComboBoxandSetVal(ccmbAltUOM, returnObject.uom, UOMId);
                                    ccmbAltUOM.SetEnabled(false);

                                }
                            }
                        });
           
                        ccmbAltUOM.SetValue(AltUOMID);
                        ctxtQty.SetValue(QuantityValue);
                        ChangePackingByQuantityinjs();
                    }
                }
            }
        }
        else {
            jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
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

function QuantityProductsGotFocus(s, e) {
    

    var ProductID = grid.GetEditor('ProductID').GetValue();

    if (ProductID != null) {
       
        Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
        _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
        _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
        _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

       
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strProductName = SpliteDetails[1];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];
        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];

        //debugger;

        //Rev 2.0 Subhra 11-03-2019     
        var isOverideConvertion = SpliteDetails[30];
        var packing_saleUOM = SpliteDetails[29];
        var sProduct_SaleUom = SpliteDetails[28];
        var sProduct_quantity = SpliteDetails[27];
        var packing_quantity = SpliteDetails[26];

        var slno= (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

        var PurchaseOrderNum= (grid.GetEditor('DocNumber').GetText() != null) ? grid.GetEditor('DocNumber').GetText() : "0";

        var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
        var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
        var type = 'add';
        var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
        var gridPackingQty = '';

        //Rev Surojit 21-06-2019

        if (SpliteDetails.length == 32) {
            if (SpliteDetails[31] == "1") {
                IsInventory = 'Yes';
            }
            else {
                IsInventory = '';
            }
        }
        else {
            IsInventory = '';
        }

        //End of rev 21-06-2019


        if (SpliteDetails.length > 26 ) {
            //IsInventory = 'Yes';
                    
            type = 'edit';

            if (PurchaseOrderNum!="0" && PurchaseOrderNum!="" && $('#hdnPageStatus').val()!="EDIT") {
                        
                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({orderid: strProductID,action:'GetPurchaseGRNQtyByOrder',module:'PurchaseGRN',strKey : PurchaseOrderNum}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                                
                        gridPackingQty = msg.d;
                            

                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes'  && SpliteDetails.length > 1) {
                            ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }

                    }
                });


            }
            else{
                       
                //var orderid = grid.GetRowKey(globalRowIndex);
                var orderid =document.getElementById('Keyval_Id').value;
                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({orderid: orderid,action:'GetPurchaseGRNQty',module:'PurchaseGRN',strKey :strProductID}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                                
                        gridPackingQty = msg.d;

                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes'  && SpliteDetails.length > 1) {
                            ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }

                    }
                });
            }
        }
        else{

            if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes'  && SpliteDetails.length > 1) {
                ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
            }
        }

        //End of Rev 2.0 Subhra 11-03-2019
    }
    else {
        Pre_Quantity = "0";
        Pre_Amt = "0";
        Pre_TotalAmt = "0";
    }

}
//Rev Subhra 13-03-2019
var issavePacking = 0;
function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('Quantity').SetValue(Quantity);
    SetFoucs();
    QuantityTextChange();
}
//End of Rev Subhra 13-03-2019
function SetFoucs() {
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    }, 2000);
           
}


function QuantityTextChange(s, e) {
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";



    if (ProductID != null) {

        //Tag qty check Tanmoy

        var DocNumber = (grid.GetEditor('DocNumber').GetText() != null) ? grid.GetEditor('DocNumber').GetText() : "0";
       

        grid.batchEditApi.StartEdit(globalRowIndex);
        var Bal_Qty= (grid.GetEditor('BalanceQty').GetValue() != null) ? grid.GetEditor('BalanceQty').GetValue() : "0";       
        if(DocNumber!="")
        {    
            if(parseFloat(Quantity)>parseFloat(Bal_Qty))
            {
                jAlert('Quantity can not be less than tagged quantity.','Alert',function(){                           
                    grid.batchEditApi.StartEdit(globalRowIndex, 6);
                    grid.GetEditor('Quantity').SetValue(Bal_Qty);
                    // return
                });
                Quantity=Bal_Qty;
            }
            else {                        
                grid.GetEditor('Quantity').SetValue(Quantity);
                grid.batchEditApi.StartEdit(globalRowIndex, 8);
            }
        }
        else {
            //Rev qty checking only edit 24253
            if($("#Keyval_internalId").val()!="Add")
            {
                //End Of Rev qty checking only edit 24253     
                if(parseFloat(Quantity)<parseFloat(Bal_Qty))
                {
                    jAlert('Quantity can not be less than tagged quantity.','Alert',function(){                           
                        grid.batchEditApi.StartEdit(globalRowIndex, 6);
                        grid.GetEditor('Quantity').SetValue(Bal_Qty);
                        // return
                    });
                    Quantity=Bal_Qty;
                }
                else {                        
                    grid.GetEditor('Quantity').SetValue(Quantity);
                    grid.batchEditApi.StartEdit(globalRowIndex, 8);
                }               
            }
            else {
                grid.GetEditor('Quantity').SetValue(Quantity);
                grid.batchEditApi.StartEdit(globalRowIndex, 8);
            }
        }
        //End of Tag qty check Tanmoy

        if (parseFloat(Quantity) != parseFloat(Pre_Quantity)) {
            DiscountTextChange(s, e);
        }
    }
    else {
        jAlert('Select a product first.');
    }
}

function PurchasePriceTextFocus(s, e) {
    var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetPurchasePriceValue = PurchasePrice;

    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
    _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
}

function PurchasePriceTextChange(s, e) {
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";    

    if (ProductID != null) {
        if (parseFloat(PurchasePrice) == "0") {
            jConfirm('Are you sure to make this Amount as Zero(0) as the charges will also become Zero(0)?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    DiscountTextChange(s, e);
                    grid.batchEditApi.EndEdit();
                    grid.batchEditApi.StartEdit(globalRowIndex, 9);
                }
                else {                    
                    if (parseFloat(_GetPurchasePriceValue) != parseFloat(PurchasePrice)) {
                        grid.batchEditApi.StartEdit(globalRowIndex, 8);

                        var tbPurchasePrice = grid.GetEditor("PurchasePrice");
                        tbPurchasePrice.SetValue(_GetPurchasePriceValue);
                        DiscountTextChange(s, e);
                    }
                }
            });
        }
        else {
            if (parseFloat(_GetPurchasePriceValue) != parseFloat(PurchasePrice)) {
                DiscountTextChange(s, e);
            }
        }
    }
    else {
        jAlert('Select a product first.');
    }
}

function DiscountTextFocus() {
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
    _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
}
     
function DiscountValueChange(s, e) {
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

    if (ProductID != null) {
        if (parseFloat(Discount) != parseFloat(_GetDiscountValue)) {
            DiscountTextChange(s, e);
        }
    }
    else {
        jAlert('Select a product first.');
    }
}

function DiscountTextChange(s, e) {
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    var Purchase_UOM_Factor = SpliteDetails[24];    
    var ConversionMultiplier = SpliteDetails[25];
    if (parseFloat(strRate) == 0) strRate = "1";

    if (ProductID != null) {
        var Amount = parseFloat((parseFloat(Quantity)*parseFloat(Purchase_UOM_Factor)*parseFloat(ConversionMultiplier)) * (parseFloat(PurchasePrice) / parseFloat(strRate))).toFixed(2);
        var amountAfterDiscount = parseFloat(parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100)).toFixed(2);

        var tbAmount = grid.GetEditor("TotalAmount");
        var tbNetAmount = grid.GetEditor("NetAmount");

        tbAmount.SetValue(amountAfterDiscount);
        tbNetAmount.SetValue(amountAfterDiscount);

        var ShippingStateCode = $("#bsSCmbStateHF").val();
        var HSNCode = SpliteDetails[14];
        var TaxType = "";

        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }

        var _SrlNo = grid.GetEditor("SrlNo").GetValue();
        if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
            var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "N" }
            TaxOfProduct.push(ProductTaxes);
        }
        else {
            $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
        }

        
        var CompareStateCode;
        if (cddlPosGstChallan.GetValue()== "S") {
            CompareStateCode = GeteShippingStateCode();
        }
        else {
            CompareStateCode = GetBillingStateCode();
        }
  
        //caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), 'P');
        
        caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), Quantity, 'P');

        Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        CalculateAmount();
    }
    else {
        jAlert('Select a product first.');
    }
}
   
function AmountTextFocus(s, e) {
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
    _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
}
function ProductPriceCalculate() {
    if ((grid.GetEditor('PurchasePrice').GetValue() == null || grid.GetEditor('PurchasePrice').GetValue() == 0) && (grid.GetEditor('Discount').GetValue() == null || grid.GetEditor('Discount').GetValue() == 0)) {
        var _purchaseprice = 0;
        var _Qty = grid.GetEditor('Quantity').GetValue();
        var _Amount = grid.GetEditor('TotalAmount').GetValue();
        _purchaseprice = (_Amount / _Qty);
        grid.GetEditor('PurchasePrice').SetValue(_purchaseprice);
    }
}
function AmountTextChange(s, e) {
    ProductPriceCalculate();
    var Amount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    var TaxAmount = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var SpliteDetails = ProductID.split("||@||");

    if (parseFloat(_GetAmountValue) != parseFloat(Amount)) {
        var tbTotalAmount = grid.GetEditor("NetAmount");
        tbTotalAmount.SetValue(parseFloat(Amount) + parseFloat(TaxAmount));
        var ShippingStateCode = $("#bsSCmbStateHF").val();
        var HSNCode = SpliteDetails[14];
        var TaxType = "";
        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }
        var _SrlNo = grid.GetEditor("SrlNo").GetValue();
        if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
            var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "N" }
            TaxOfProduct.push(ProductTaxes);
        }
        else {
            $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
        }                
        var CompareStateCode;
        if (cddlPosGstChallan.GetValue()== "S") {
            CompareStateCode = GeteShippingStateCode();
        }
        else {
            CompareStateCode = GetBillingStateCode();
        }
        //caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, Amount, TaxType, CompareStateCode, $('#ddl_Branch').val(), 'P');
        caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, Amount, TaxType, CompareStateCode, $('#ddl_Branch').val(),$("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P');

        Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        CalculateAmount();
    }
}

function OnAddNewClick() {
    if (ctaggingList.GetValue() == null) {
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage();

        var tbSrl = grid.GetEditor("SrlNo");
        var tbRow = grid.GetEditor("RowNo");

        tbSrl.SetValue(noofvisiblerows);
        tbRow.SetValue(noofvisiblerows);
    }
    else {
        QuotationNumberChanged();
    }
}
    </script>
    <script>
        function TaxAmountFocus(s, e) {
            rowEditCtrl = s;
        }

        function TaxAmountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key== "NumpadEnter") {
                s.OnButtonClick(0);
            }
        }

        function TaxAmountClick(s, e) {
            if (e.buttonIndex == 0) {
                if (cddl_AmountAre.GetValue() != null) {
                    var IDs = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                    var SpliteDetails = IDs.split("||@||");
                    var ProductID = SpliteDetails[0];

                    if (ProductID.trim() != "") {
                        Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                        Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                        Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";

                        document.getElementById('setCurrentProdCode').value = ProductID;
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();

                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();
                        caspxTaxpopUp.Show();
                        //caspxTaxpopUp.SetWidth(window.screen.width - 200);
                        //caspxTaxpopUp.popupHorizontalAlign = "WindowCenter";

                        //Set Product Gross Amount and Net Amount

                        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                        var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "";
                        var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
                        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                        var Purchase_UOM_Factor = SpliteDetails[24];
                        var ConversionMultiplier = SpliteDetails[25];
                        if (strRate == 0) strRate = 1;
                        document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                        var StockQuantity = parseFloat(QuantityValue)*parseFloat(Purchase_UOM_Factor);
                        var Amount = parseFloat((parseFloat(QuantityValue)*parseFloat(Purchase_UOM_Factor)*parseFloat(ConversionMultiplier)) * (strSalePrice / strRate)).toFixed(2);
                        var amountAfterDiscount =(grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                        //var amountAfterDiscount = (parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100)).toFixed(2);

                        clblTaxProdGrossAmt.SetText(Amount);
                        clblProdNetAmt.SetText(amountAfterDiscount);

                        document.getElementById('HdProdGrossAmt').value = Amount;
                        document.getElementById('HdProdNetAmt').value = amountAfterDiscount;

                        //End Here


                        //Set Discount Here

                        if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                            var discount = (parseFloat(Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
                            clblTaxDiscount.SetText(discount);
                        }
                        else {
                            clblTaxDiscount.SetText('0.00');
                        }

                        //End Here 


                        //Checking is gstcstvat will be hidden or not

                        if (cddl_AmountAre.GetValue() == "2") {
                            $('.GstCstvatClass').hide();
                            clblTaxableGross.SetText("(Taxable)");
                            clblTaxableNet.SetText("(Taxable)");

                            $('.gstGrossAmount').hide();
                            $('.gstNetAmount').hide();
                            clblTaxableGross.SetText("");
                            clblTaxableNet.SetText("");
                        }
                        else if (cddl_AmountAre.GetValue() == "1") {
                            $('.GstCstvatClass').show();
                            $('.gstGrossAmount').hide();
                            $('.gstNetAmount').hide();
                            clblTaxableGross.SetText("");
                            clblTaxableNet.SetText("");

                            //Get Customer Shipping StateCode
                            var shippingStCode = '';
                            //shippingStCode = ctxtshippingState.GetText();
                            // shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();
                            
                            if (cddlPosGstChallan.GetValue() == "S") {
                                shippingStCode = GeteShippingStateCode();
                            }
                            else {
                                shippingStCode = GetBillingStateCode();
                            }
                        }

                        //End here
                        
                        var _SrlNo = document.getElementById('HdSerialNo').value;
                        var _IsEntry="";
                        if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length > 0) {
                            _IsEntry=TaxOfProduct.find(o => o.SrlNo === _SrlNo).IsTaxEntry;
                        }

                        if(_IsEntry=="N"){
                            cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                        }
                        else{
                            cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                        }

                        ctxtprodBasicAmt.SetValue(grid.GetEditor('TotalAmount').GetValue());
                    } else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 14);
                    }
                }
            }
        }
    </script>
    <script>
        function ctaxUpdatePanelEndCall(s, e) {
            if (ctaxUpdatePanel.cpstock != null) {
                ctaxUpdatePanel.cpstock = null;
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
                return false;
            }
        }
        function chargeCmbtaxClick(s, e) {
            GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
            ChargegstcstvatGlobalName = s.GetText();
        }
        var gstcstvatGlobalName;
        function CmbtaxClick(s, e) {
            GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
            gstcstvatGlobalName = s.GetText();
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
                        if (selectedIndex && ccmbGstCstVat.GetItem(selectedIndex) != null) {
                            ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
                        }

                        cgridTax.cpComboCode = null;
                    }
                }
            }
            if(cgridTax.cpUpdated!=null && typeof(cgridTax.cpUpdated)!='undefined')
            {
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
                    grid.batchEditApi.StartEdit(globalRowIndex, 14);
                    grid.GetEditor("TaxAmount").SetValue(totAmt);

                    if (cddl_AmountAre.GetValue() == "2") {
                        var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("TotalAmount").GetValue());
                        var totalRoundOffAmount = Math.round(totalNetAmount);

                        grid.GetEditor("NetAmount").SetValue(totalRoundOffAmount);
                        grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(grid.GetEditor("TotalAmount").GetValue()) + (totalRoundOffAmount - totalNetAmount), 2));
                    }
                    else {
                        grid.GetEditor("NetAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("TotalAmount").GetValue()), 2));
                    }

                    Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                    Cur_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                    Cur_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
                    CalculateAmount();
                }
            }
            if (cgridTax.GetVisibleRowsOnPage() == 0) {
                $('.cgridTaxClass').hide();
                ccmbGstCstVat.Focus();
            }
            //Debjyoti Check where any Gst Present or not
            // If Not then hide the hole section
            if(cgridTax.cpJsonData!=undefined)
                SetRunningTotal();
            ShowTaxPopUp("IY");
            if(cgridTax.cpJsonData!=undefined)
                RecalCulateTaxTotalAmountInline();
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
                    //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);
                    //Call for Running Total
                    if(cgridTax.cpJsonData!=undefined)
                        SetRunningTotal();

                } else {
                    s.SetText("");
                }
            }
            if(cgridTax.cpJsonData!=undefined)
                RecalCulateTaxTotalAmountInline();
        }

        function taxAmountLostFocus(s, e) {
            debugger;
            var finalTaxAmt = parseFloat(s.GetValue());
            var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                ctxtTaxTotAmt.SetValue(Math.round(totAmt + finalTaxAmt - taxAmountGlobal));
            } else {
                ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
            }


            //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);
            //Set Running Total
            if(cgridTax.cpJsonData!=undefined)
                SetRunningTotal();
            if(cgridTax.cpJsonData!=undefined)
                RecalCulateTaxTotalAmountInline();
        }
        function GetTaxVisibleIndex(s, e) {
            globalTaxRowIndex = e.visibleIndex;
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

        function BatchUpdate() {
            var _SrlNo = document.getElementById('HdSerialNo').value;
            if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
                var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "Y" }
                TaxOfProduct.push(ProductTaxes)
            }
            else {
                $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "Y"; });
            }

            if (cgridTax.GetVisibleRowsOnPage() > 0) {
                cgridTax.UpdateEdit();
            }
            else {
                cgridTax.PerformCallback('SaveGST');
            }
            return false;
        }

        function Save_TaxesClick() {
            debugger;
            grid.batchEditApi.EndEdit();
            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var i, cnt = 1;
            var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

            cnt = 1;
            for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                var Amount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
                var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
                var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
                var NetAmount = (grid.batchEditApi.GetCellValue(i, 'NetAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'NetAmount')) : "0";
                var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

                sumAmount = sumAmount + parseFloat(Amount);
                sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
                sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
                sumNetAmount = sumNetAmount + parseFloat(NetAmount);

                cnt++;
            }

            //if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
            cnt = 1;
            for (i = 0 ; cnt <= noofvisiblerows ; i++) {
                var Amount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
                var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
                var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
                var NetAmount = (grid.batchEditApi.GetCellValue(i, 'NetAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'NetAmount')) : "0";
                var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

                sumAmount = sumAmount + parseFloat(Amount);
                sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
                sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
                sumNetAmount = sumNetAmount + parseFloat(NetAmount);

                cnt++;
            }
            //}

            ////Debjyoti 
            //document.getElementById('HdChargeProdAmt').value = sumAmount;
            //document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
            ////End Here


            document.getElementById('HdChargeProdAmt').value = sumAmount;
            document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;

            ctxtProductAmount.SetValue(parseFloat(sumAmount).toFixed(2));
            ctxtProductTaxAmount.SetValue(parseFloat(sumTaxAmount).toFixed(2));
            ctxtProductDiscount.SetValue(parseFloat(sumDiscount).toFixed(2));
            ctxtProductNetAmount.SetValue(parseFloat(sumNetAmount).toFixed(2));
            clblChargesTaxableGross.SetText("");
            clblChargesTaxableNet.SetText("");

            //Checking is gstcstvat will be hidden or not
            if (cddl_AmountAre.GetValue() == "2") {

                $('.lblChargesGSTforGross').show();
                $('.lblChargesGSTforNet').show();


                $('.lblChargesGSTforGross').hide();
                $('.lblChargesGSTforNet').hide();

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
            //ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));

            ctxtTotalAmount.SetValue(parseFloat($("#txt_TotalAmt").text()));


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

        function PercentageTextChange(s, e) {
            debugger;
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
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));  //////////This line should be change
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                GlobalTaxAmt = 0;
            }

            SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
            SetChargesRunningTotal();

            RecalCulateTaxTotalAmountCharges();
        }

        var taxAmountGlobalCharges;
        function QuotationTaxAmountGotFocus(s, e) {
            taxAmountGlobalCharges = parseFloat(s.GetValue());
        }
        var chargejsonTax=[];
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

                    //ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
                    gridTax.cpTotalCharges = null;
                }
            }

            SetChargesRunningTotal();
            ShowTaxPopUp("IN");
        }
        function QuotationTaxAmountTextChange(s, e) {
            debugger;
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
                ctxtQuoteTaxTotalAmt.SetValue((DecimalRoundoff(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges,2)));
                ctxtTotalAmount.SetValue(DecimalRoundoff(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()),2));
                GlobalTaxAmt = 0;
                SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                //gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(DecimalRoundoff(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges,2));
                ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
                SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
            }

            RecalCulateTaxTotalAmountCharges();
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

        function Save_TaxClick() {
            if (gridTax.GetVisibleRowsOnPage() > 0) {
                gridTax.UpdateEdit();
            }
            else {
                gridTax.PerformCallback('SaveGst');
            }
            cPopup_Taxes.Hide();
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
        function OpenUdf(s, e) {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SQO&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();

            }
            return true;
        }


        function SetArrForUOM(){
            //Rev Subhra 16-09-2019
            issavePacking = 1;
            //End of Rev Subhra 16-09-2019
            if (aarr.length == 0) {
                for(var i = -500; i < 500;i++)
                {
                    if(grid.GetRow(i) != null){
               
                        var ProductID = (grid.batchEditApi.GetCellValue(i,'ProductID') != null) ? grid.batchEditApi.GetCellValue(i,'ProductID') : "0";
                        if(ProductID!="0") {
                            var actionqty = '';
                            var PurchaseOrderNum = (grid.GetEditor('DocNumber').GetText() != null) ? grid.GetEditor('DocNumber').GetText() : "";
                            if($("#hdnPageStatus").val() == "EDIT"){

                                


                                var SpliteDetails = ProductID.split("||@||");
                                var strProductID = SpliteDetails[0];
                                var orderid = grid.GetRowKey(i);
                                var slnoget = grid.batchEditApi.GetCellValue(i,'SrlNo');
                                var Quantity = grid.batchEditApi.GetCellValue(i,'Quantity');
                                //Rev Subhra 16-09-2019
                                var challanid =document.getElementById('Keyval_Id').value;
                                orderid=challanid;
                                //End of Rev Subhra 16-09-2019
                                if(PurchaseOrderNum!=""){
                                    actionqty = 'GetPurchaseGRNQtyByOrder';
                                    orderid =strProductID;
                                }
                                else{
                                    actionqty = 'GetPurchaseGRNQty';
                                    PurchaseOrderNum = strProductID;
                                }

                                $.ajax({
                                    type: "POST",
                                    url: "Services/Master.asmx/GetMultiUOMDetails",
                                    data: JSON.stringify({orderid: orderid,action:actionqty,module:'PurchaseGRN',strKey :PurchaseOrderNum}),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    async: false,
                                    success: function (msg) {
                               
                                        gridPackingQty = msg.d;

                                        if(msg.d != ""){
                                            var packing = SpliteDetails[26];
                                            var PackingUom = SpliteDetails[29];
                                            var PackingSelectUom = SpliteDetails[28];
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
        // Rev 1.0
        function AddContraLockStatus(LockDate) {
            $.ajax({
                type: "POST",
                url: "ProjectPurchaseChallan.aspx/GetAddLock",
                data: JSON.stringify({ LockDate: LockDate }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var currentRate = msg.d;
                    if (currentRate != null && currentRate == "-9") {
                        $("#hdnValAfterLock").val("-9");
                    }
                    else {
                        $("#hdnValAfterLock").val("1");
                    }

                }
            });
        }
        // End of Rev 1.0


        function SaveNew_Click() {
            flag = true;
            LoadingPanel.Show();
            // Rev 1.0
            AddContraLockStatus(cPLQuoteDate.GetDate());
            // End of Rev 1.0

            // Rev 1.0
            if ($("#hdnValAfterLock").val() == "-9") {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
                LoadingPanel.hide();
                flag = false;
                return false;
            }
            // End of Rev 1.0
            var txtPurchaseNo = $("#txtVoucherNo").val().trim();
            if (txtPurchaseNo == null || txtPurchaseNo == "") {
                LoadingPanel.Hide();
                $("#MandatoryBillNo").show();
                flag = false;
                return false;
            }
            else {
                $('#MandatoryBillNo').attr('style', 'display:none');
            }

            var ProjectCode = clookup_Project.GetText();
            if ( $("#hdnProjectMandatory").val()=="1" && ProjectCode == "") {
                LoadingPanel.Hide();
                jAlert("Please Select Project.");
                flag = false;
                return false;
            }

            var customerId = GetObjectID('hdnCustomerId').value;
            if (customerId == '' || customerId == null) {
                LoadingPanel.Hide();
                $('#MandatorysCustomer').attr('style', 'display:block');
                flag = false;
                return false;
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }

            var PartyInvoiceNo = ctxtPartyInvoice.GetValue();
            if (PartyInvoiceNo == '' || PartyInvoiceNo == null) {
                LoadingPanel.Hide();
                $('#MandatorysPartyinvno').attr('style', 'display:block');
                flag = false;
                return false;
            }
            else {
                $('#MandatorysPartyinvno').attr('style', 'display:none');
            }

            var EWayBilMendatory = document.getElementById('hdfEWayBillMendatory').value;// $('#hdfTagMendatory').val();
            var strEWayBillNumber=$("#txtEWayBillNumber").val();
            if(strEWayBillNumber=="")
            {
                if (EWayBilMendatory == 'Yes') {
                    LoadingPanel.Hide();
                    $("#MandatoryEWayBillNumber").show();
                    return false;
                }
         
            }
           


            var RowCount = 0;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var ProductID = (grid.batchEditApi.GetCellValue(RowCount, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(RowCount, 'ProductName')) : "";

                if (ProductID != "") {
                    IsProduct = "Y";
                    break;
                }
                RowCount++;
            }

            if (flag != false) {
                SetArrForUOM(); //Surojit For UOM EDIT

                if (IsProduct == "Y") {
                    //Subhra 13-03-2019
                    if (issavePacking == 1 && aarr.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "ProjectPurchaseChallan.aspx/SetSessionPacking",
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                $('#<%=hdfIsDelete.ClientID %>').val('I');
                                $('#<%=hdnRefreshType.ClientID %>').val('N');
                                $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                                var JsonProductList = JSON.stringify(TaxOfProduct);
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;

                                var JsonProductStock = JSON.stringify(StockOfProduct);
                                GetObjectID('hdnJsonProductStock').value = JsonProductStock;                    

                                grid.AddNewRow();
                                grid.UpdateEdit();
                            }
                        });
                    }
                    else
                    {
                        //Subhra 13-03-2019
                        $('#<%=hdfIsDelete.ClientID %>').val('I');
                        $('#<%=hdnRefreshType.ClientID %>').val('N');
                        $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                        var JsonProductList = JSON.stringify(TaxOfProduct);
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;

                        var JsonProductStock = JSON.stringify(StockOfProduct);
                        GetObjectID('hdnJsonProductStock').value = JsonProductStock;                    

                        grid.AddNewRow();
                        grid.UpdateEdit();
                    }

                    
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('Please add atleast single record first');
                }
            }
        }

        function SaveExit_Click() {
            flag = true;
            LoadingPanel.Show();
            // Rev 1.0
            AddContraLockStatus(cPLQuoteDate.GetDate());
            // End of Rev 1.0
            // Rev 1.0
            if ($("#hdnValAfterLock").val() == "-9") {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
                LoadingPanel.Hide();
                flag = false;
                return false;
            }
            // End of Rev 1.0
            var txtPurchaseNo = $("#txtVoucherNo").val().trim();
            if (txtPurchaseNo == null || txtPurchaseNo == "") {
                LoadingPanel.Hide();
                $("#MandatoryBillNo").show();
                flag = false;
                return false;
            }
            else {
                $('#MandatoryBillNo').attr('style', 'display:none');
            }



            var customerId = GetObjectID('hdnCustomerId').value;
            if (customerId == '' || customerId == null) {
                LoadingPanel.Hide();
                $('#MandatorysCustomer').show();
                flag = false;
                return false;
            }
            else {
                $('#MandatorysCustomer').attr('style', 'display:none');
            }

            var ProjectCode = clookup_Project.GetText();
            if ( $("#hdnProjectMandatory").val()=="1" && ProjectCode == "") {
                LoadingPanel.Hide();
                jAlert("Please Select Project.");
                flag = false;
                return false;
            }


            var PartyInvoiceNo = ctxtPartyInvoice.GetValue();
            if (PartyInvoiceNo == '' || PartyInvoiceNo == null) {
                LoadingPanel.Hide();
                $('#MandatorysPartyinvno').show();
                flag = false;
                return false;
            }
            else {
                $('#MandatorysPartyinvno').attr('style', 'display:none');
            }
            var EWayBilMendatory = document.getElementById('hdfEWayBillMendatory').value;// $('#hdfTagMendatory').val();
            var strEWayBillNumber=$("#txtEWayBillNumber").val();
            if(strEWayBillNumber=="")
            {
                if (EWayBilMendatory == 'Yes') {
                    LoadingPanel.Hide();
                    $("#MandatoryEWayBillNumber").show();
                    return false;
                }
            }

            var RowCount = 0;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var ProductID = (grid.batchEditApi.GetCellValue(RowCount, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(RowCount, 'ProductName')) : "";

                if (ProductID != "") {
                    IsProduct = "Y";
                    break;
                }
                RowCount++;
            }

            if (flag != false) {
                SetArrForUOM(); //Surojit For UOM EDIT
                if (IsProduct == "Y") {

                    if (issavePacking == 1 && aarr.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "ProjectPurchaseChallan.aspx/SetSessionPacking",
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                $('#<%=hdfIsDelete.ClientID %>').val('I');
                                $('#<%=hdnRefreshType.ClientID %>').val('E');
                                $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                                var JsonProductList = JSON.stringify(TaxOfProduct);
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    
                                var JsonProductStock = JSON.stringify(StockOfProduct);
                                GetObjectID('hdnJsonProductStock').value = JsonProductStock;                    
                                SaveSendUOM('PC');
                                grid.AddNewRow();
                                grid.UpdateEdit();
                            }
                        });
                    }
                    else
                    {
                        $('#<%=hdfIsDelete.ClientID %>').val('I');
                        $('#<%=hdnRefreshType.ClientID %>').val('E');
                        $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                        var JsonProductList = JSON.stringify(TaxOfProduct);
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    
                        var JsonProductStock = JSON.stringify(StockOfProduct);
                        GetObjectID('hdnJsonProductStock').value = JsonProductStock;                    
                        SaveSendUOM('PC');
                        grid.AddNewRow();
                        grid.UpdateEdit();
                    }
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('Please add atleast single record first');
                }
            }
        }
    </script>
    <script>
        var IsFocus = "0";

        function WarehousePanelEndCall(s, e) {
            if (cWarehousePanel.cperrorMsg == "duplicateSerial") {
                cWarehousePanel.cperrorMsg = null;
                jAlert("Duplicate Serial. Cannot Proceed.");
            }
            else if (cWarehousePanel.cpIsSave == "Y") {
                cWarehousePanel.cpIsSave = null;
                cWarehousePanel.cpIsShow = null;
                cPopup_Warehouse.Hide();
                grid.batchEditApi.StartEdit(Warehouseindex, 8);
            }
            else if (cWarehousePanel.cpIsSave == "N") {
                cWarehousePanel.cpIsSave = null;
                jAlert('Purchase Quantity must be equal to Warehouse Quantity.');
            }
            else if (cWarehousePanel.cpduplicateMsg == "_duplicateSerial") {
                var list = cWarehousePanel.cpduplicateSerial;

                cWarehousePanel.cpduplicateMsg = null;
                cWarehousePanel.cpduplicateSerial = null;

                jAlert("Duplicate Serial are : " + list);
            }


            if (cWarehousePanel.cpIsShow == "Y") {
                cWarehousePanel.cpIsShow = null;
                var Warehousetype = document.getElementById("hdfWarehousetype").value;

                if (Warehousetype == "W") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'none';
                    div_Manufacture.style.display = 'none';
                    div_Expiry.style.display = 'none';
                    div_Quantity.style.display = 'block';
                    div_Serial.style.display = 'none';
                    div_Upload.style.display = 'none';
                    div_Break.style.display = 'none';
                    cPopup_Warehouse.Show();
                }
                else if (Warehousetype == "B") {
                    div_Warehouse.style.display = 'none';
                    div_Batch.style.display = 'block';
                    div_Manufacture.style.display = 'block';
                    div_Expiry.style.display = 'block';
                    div_Quantity.style.display = 'block';
                    div_Serial.style.display = 'none';
                    div_Upload.style.display = 'none';
                    div_Break.style.display = 'none';
                    cPopup_Warehouse.Show();
                }
                else if (Warehousetype == "S") {
                    div_Warehouse.style.display = 'none';
                    div_Batch.style.display = 'none';
                    div_Manufacture.style.display = 'none';
                    div_Expiry.style.display = 'none';
                    div_Quantity.style.display = 'none';
                    div_Serial.style.display = 'block';
                    div_Upload.style.display = 'block';
                    div_Break.style.display = 'none';
                    cPopup_Warehouse.Show();
                }
                else if (Warehousetype == "WB") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'block';
                    div_Manufacture.style.display = 'block';
                    div_Expiry.style.display = 'block';
                    div_Quantity.style.display = 'block';
                    div_Serial.style.display = 'none';
                    div_Upload.style.display = 'none';
                    div_Break.style.display = 'none';
                    cPopup_Warehouse.Show();
                }
                else if (Warehousetype == "WS") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'none';
                    div_Manufacture.style.display = 'none';
                    div_Expiry.style.display = 'none';
                    div_Quantity.style.display = 'none';
                    div_Serial.style.display = 'block';
                    div_Upload.style.display = 'block';
                    div_Break.style.display = 'none';
                    cPopup_Warehouse.Show();
                }
                else if (Warehousetype == "WBS") {
                    div_Warehouse.style.display = 'block';
                    div_Batch.style.display = 'block';
                    div_Manufacture.style.display = 'block';
                    div_Expiry.style.display = 'block';
                    div_Quantity.style.display = 'none';
                    div_Serial.style.display = 'block';
                    div_Upload.style.display = 'block';
                    div_Break.style.display = 'block';
                    cPopup_Warehouse.Show();
                }
                else if (Warehousetype == "BS") {
                    div_Warehouse.style.display = 'none';
                    div_Batch.style.display = 'block';
                    div_Manufacture.style.display = 'block';
                    div_Expiry.style.display = 'block';
                    div_Quantity.style.display = 'none';
                    div_Serial.style.display = 'block';
                    div_Upload.style.display = 'block';
                    div_Break.style.display = 'none';
                    cPopup_Warehouse.Show();
                }
                else {
                    div_Warehouse.style.display = 'none';
                    div_Batch.style.display = 'none';
                    div_Manufacture.style.display = 'none';
                    div_Expiry.style.display = 'none';
                    div_Quantity.style.display = 'none';
                    div_Serial.style.display = 'none';
                    div_Upload.style.display = 'none';
                }
            }

            if (IsFocus == "1") {
                ctxtserialID.Focus();
                IsFocus = "0";
            }
            //Rev Subhra 15-05-2019
            grid.batchEditApi.StartEdit(globalRowIndex, 8);
            //End of Rev Subhra 15-05-2019
        }

        function closeWarehouse(s, e) {
            e.cancel = false;
            cWarehousePanel.PerformCallback('WarehouseDelete');
        }

        function closeStockPopup(s, e) {
            e.cancel = false;
            grid.batchEditApi.StartEdit(globalRowIndex, 8);
        }

        function FullnFinalSave(){
            cPopupWarehouse.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 8);
        }

        function ClearWarehouse() {
            Stock_EditID = "0";

            ctxtQuantity.SetValue("0");
            ctxtBatchName.SetValue("");
            ctxtStartDate.SetDate(null);
            ctxtEndDate.SetDate(null);
            ctxtserialID.SetValue("");
        }

        function SubmitWarehouse() {
            var WarehouseID = (cCmbWarehouseID.GetValue() != null) ? cCmbWarehouseID.GetValue() : "0";
            var WarehouseName = (cCmbWarehouseID.GetText() != null) ? cCmbWarehouseID.GetText() : "";
            var BatchName = (ctxtBatchName.GetValue() != null) ? ctxtBatchName.GetValue() : "";
            var MfgDate = (ctxtStartDate.GetValue() != null) ? ctxtStartDate.GetValue() : "";
            var ExpiryDate = (ctxtEndDate.GetValue() != null) ? ctxtEndDate.GetValue() : "";
            var SerialNo = (ctxtserialID.GetValue() != null) ? ctxtserialID.GetValue() : "";
            var Qty = parseFloat(ctxtQuantity.GetValue());
            var altQty=ctxtAltQuantity.GetValue();

            MfgDate = GetDateFormat(MfgDate);
            ExpiryDate = GetDateFormat(ExpiryDate);

            $("#spnCmbWarehouse").hide();
            $("#spntxtBatch").hide();
            $("#spntxtQuantity").hide();
            $("#spntxtserialID").hide();

            var Ptype = document.getElementById('hdfWarehousetype').value;
            if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
                $("#spnCmbWarehouse").show();
            }
            else if ((Ptype == "B" && BatchName == "") || (Ptype == "WB" && BatchName == "") || (Ptype == "WBS" && BatchName == "") || (Ptype == "BS" && BatchName == "")) {
                $("#spntxtBatch").show();
            }
            else if ((Ptype == "W" && Qty == "0") || (Ptype == "B" && Qty == "0") || (Ptype == "WB" && Qty == "0")) {
                $("#spntxtQuantity").show();
            }
            else if ((Ptype == "S" && SerialNo == "") || (Ptype == "WS" && SerialNo == "") || (Ptype == "WBS" && SerialNo == "") || (Ptype == "BS" && SerialNo == "")) {
                $("#spntxtserialID").show();
            }
            else {
                if ((Ptype == "S" && Stock_EditID == "0") || (Ptype == "WS" && Stock_EditID == "0") || (Ptype == "WBS" && Stock_EditID == "0") || (Ptype == "BS" && Stock_EditID == "0")) {
                    ctxtserialID.SetValue("");
                    ctxtserialID.Focus();
                    IsFocus = "1";
                }
                else {
                    ctxtQuantity.SetValue("0");
                    ctxtBatchName.SetValue("");
                    ctxtStartDate.SetDate(null);
                    ctxtEndDate.SetDate(null);
                    ctxtserialID.SetValue("");
                }

                cWarehousePanel.PerformCallback('StockSave~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + MfgDate + '~' + ExpiryDate + '~' + SerialNo + '~' + Qty + '~' + Stock_EditID+ '~' + altQty);
                Stock_EditID = "0";
            }
        }

        function FinalWarehouse() {
            cWarehousePanel.PerformCallback('WarehouseFinal');
            //Rev Subhra 15-05-2019
            grid.batchEditApi.StartEdit(globalRowIndex, 8);
            //End of Rev Subhra 15-05-2019
        }

        function fn_Delete(keyValue) {
            cWarehousePanel.PerformCallback('Delete~' + keyValue);
        }

        function fn_Edit(keyValue) {
            SelectedWarehouseID = keyValue;

            ctxtQuantity.SetValue("0");
            ctxtBatchName.SetValue("");
            ctxtStartDate.SetDate(null);
            ctxtEndDate.SetDate(null);
            ctxtserialID.SetValue("");

            cWarehousePanel.PerformCallback('EditWarehouse~' + keyValue);
        }

        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = dd + '-' + mm + '-' + yyyy;
            }

            return today;
        }

        function GetPCDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = yyyy + '-' + mm + '-' + dd;
            }

            return today;
        }

        function GetReverseDateFormat(today) {
            if (today != "") {
                var dd = today.substring(0, 2);
                var mm = today.substring(3, 5);
                var yyyy = today.substring(6, 10);

                today = mm + '-' + dd + '-' + yyyy;
            }

            return today;
        }
    </script>
    <script>
        document.onkeydown = function (e) {
            if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt+S -- ie, Save & New  
                StopDefaultAction(e);
                document.getElementById('btn_SaveRecords').click();
            }
            else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt+X -- ie, Save & Exit!     
                StopDefaultAction(e);
                document.getElementById('btn_SaveRecordsExit').click();
            }
        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
    </script>
    <script>
        function VendorButnClick(s, e) {
            document.getElementById("txtCustSearch").value = "";
            var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Unique Id</th><th>Vendor Name</th></tr><table>";
            document.getElementById("CustomerTable").innerHTML = txt;

            setTimeout(function () { $("#txtCustSearch").focus(); }, 500);

            $('#txtCustSearch').val('');
            $('#CustModel').modal('show');
        }

        function VendorKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key== "NumpadEnter") {
                s.OnButtonClick(0);
            }
        }

        function Customerkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtCustSearch").val();
            OtherDetails.BranchID = $('#ddl_Branch').val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Vendor Name");

                if ($("#txtCustSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetVendorWithBranch", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
        }

        function SetCustomer(Id, Name) {
            var VendorID = Id;
            if (VendorID != "") {
                $('#CustModel').modal('hide');
                ctxtCustName.SetText(Name);
                //Chinmoy comment below line
                //LoadCustomerAddress(VendorID, $('#ddl_Branch').val(), 'PC');
          

                GetObjectID('hdnCustomerId').value = VendorID;
                SetEntityType(VendorID);
                //chinmoy added this line
                cddl_AmountAre.SetEnabled(true);
                PopulateGSTCSTVAT();
                GetVendorGSTInFromBillShip(GetObjectID('hdnCustomerId').value);
                GetPurchaseForGstValue();
                page.tabs[0].SetEnabled(true);
                page.tabs[1].SetEnabled(true);
                if ($('#hfBSAlertFlag').val() == "1") {
                    jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            page.SetActiveTabIndex(1);
                        }
                    });
                }
                cContactPerson.Focus();
            }
        }


        //Chinmoy added below function

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

        
        function GetPurchaseForGstValue()
        {
           
            cddlPosGstChallan.ClearItems();
            if(cddlPosGstChallan.GetItemCount()==0)
            {
                cddlPosGstChallan.AddItem(GetShippingStateName() + '[Shipping]', "S");
                cddlPosGstChallan.AddItem(GetBillingStateName() + '[Billing]', "B");
            }
            
            else  if(cddlPosGstChallan.GetItemCount()>2)
            {
                cddlPosGstChallan.ClearItems();
                //cddl_PosGstSalesOrder.RemoveItem(0);
                //cddl_PosGstSalesOrder.RemoveItem(0);
            }

            if(PosGstId=="" || PosGstId==null)
            {
                cddlPosGstChallan.SetValue("S");
            }
            else
            {
                cddlPosGstChallan.SetValue(PosGstId);
            }
        }


        var PosGstId="";
        function PopulateChallanPosGst(e)
        {
            
            PosGstId=cddlPosGstChallan.GetValue();
            if(PosGstId=="S")
            {
                cddlPosGstChallan.SetValue("S");  
            }
            else if(PosGstId=="B")
            {
                cddlPosGstChallan.SetValue("B"); 
            }
        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code== "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex")
                        SetProduct(Id, name);

                        //Start:Chinmoy 
                    else if (indexName == "BillingAreaIndex") {
                        SetBillingArea(Id, name);
                    }
                    else if (indexName == "ShippingAreaIndex") {
                        SetShippingArea(Id, name);
                    }
                    else if(indexName=="customeraddressIndex")
                    {
                        SetCustomeraddress(Id,name)
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
                        //End
                    else
                        $('#txtCustSearch').focus();
                }
            }

        }
    </script>
    <script>
        function prodkeydown(e) {
            //Both-->B;Inventory Item-->Y;Capital Goods-->C
            var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            OtherDetails.InventoryType = inventoryType;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");
                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetPurchaseProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
        }
    </script>
    <script>
        $(document).ready(function() {
            var DocNo=getParameterByName("key");
            var DocType=getParameterByName("type");
            var BranchId=$("#ddl_Branch").val();
            $("#hdnTCBranchId").val(BranchId);
            if(DocNo){
                if(DocNo.toLowerCase()!="add"){
                    if(GetObjectID('hdnJsonTempStock').value){
                        var myObj=GetObjectID('hdnJsonTempStock').value;
                        var JObject=JSON.parse(myObj);    
            
                        if (JObject.length > 0) {
                            for (x in JObject) {
                                JObject[x]["SrlNo"]=parseInt(JObject[x]["SrlNo"]);
                                JObject[x]["LoopID"]=parseInt(JObject[x]["LoopID"]);
                            }
                        }

                        StockOfProduct=JObject;
                        GetObjectID('hdnJsonTempStock').value="";

                        if (ctaggingList.GetValue() != null && ctaggingList.GetValue()!="") {
                            grid.GetEditor('ProductName').SetEnabled(false);
                        }
                    }        
                } 
            }
        });
    </script>

    <script>
        function onBranchItems() {
            if ($("#hdnProjectSelectInEntryModule").val() == "1")
                clookup_Project.gridView.Refresh();

            function ProjectListKeyDown(s, e) {
                if (e.htmlEvent.key == "Enter") {
                    s.OnButtonClick(0);
                }
            }

            function ProjectListButnClick(s, e) {
                clookup_Project.ShowDropDown();
            }

        }   
        

        function ProjectValueChange(s, e) {
            //debugger;
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'ProjectPurchaseChallan.aspx/getHierarchyID',
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
    </script>
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
                <asp:Label ID="lblHeading" runat="server" Text="Add Project Purchase GRN"></asp:Label>
            </h3>
            <div id="pageheaderContent" class="pull-right reverse wrapHolder content horizontal-images" style="display: none;">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder" id="divAvailableStk">
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
                    </ul>
                </div>
            </div>
            <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
            <div id="divcross" runat="server" class="crossBtn"><a href="ProjectPurchaseChallanList.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class=" form_main row">
        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
            <TabPages>
                <dxe:TabPage Name="General" Text="General">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <div class="row">
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Inventory" runat="server" Text="Inventory Item?">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddlInventory" CssClass="backSelect" runat="server" Width="100%">
                                        <asp:ListItem Text="Both" Value="B" />
                                        <asp:ListItem Text="Inventory Item" Value="Y" />
                                        <%--  <asp:ListItem Text="Non Inventory Item" Value="N" />
                                        <asp:ListItem Text="Capital Goods" Value="C" />
                                        <asp:ListItem Text="Service Item" Value="S" />--%>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8" runat="server" id="divNumberingScheme">
                                    <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%"
                                        DataTextField="SchemaName" DataValueField="ID" onchange="CmbScheme_ValueChange()">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No.">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()" Enabled="false">
                                    </asp:TextBox>
                                    <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <%--Rev 1.0 [ LostFocus="function(s, e) { SetLostFocusonDemand(e)}" added ]--%>
                                    <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" 
                                        ClientInstanceName="cPLQuoteDate" Width="100%" UseMaskBehavior="True" >
                                        
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                        <ClientSideEvents  LostFocus="function(s, e) { SetLostFocusonDemand(e)}"  />
                                    </dxe:ASPxDateEdit>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Enabled="false" onchange="onBranchItems()">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <% if (rightsVendor.CanAdd)
                                       { %>
                                    <a href="#" onclick="AddVendorClick()" style="left: -12px; top: 20px; font-size: 16px;"><i id="openlink" runat="server" class="fa fa-plus-circle" aria-hidden="true"></i></a>
                                    <% } %>
                                    <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                                    </dxe:ASPxButtonEdit>
                                    <span id="MandatorysCustomer" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson"
                                        Font-Size="12px" ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                        <ClientSideEvents EndCallback="cmbContactPersonEndCall"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Party Invoice No.">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <dxe:ASPxTextBox ID="txtPartyInvoice" ClientInstanceName="ctxtPartyInvoice" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                    <span id="MandatorysPartyinvno" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Party Invoice Date">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxDateEdit ID="dt_PartyDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLPartyDate" Width="100%">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                        <ClientSideEvents LostFocus="function(s, e) {s.HideDropDown();}" GotFocus="function(s, e) {s.ShowDropDown();}" />
                                    </dxe:ASPxDateEdit>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select Purchase Order">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxButtonEdit ID="taggingList" ClientInstanceName="ctaggingList" runat="server" ReadOnly="true" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="taggingListButnClick" KeyDown="taggingListKeyDown" />
                                    </dxe:ASPxButtonEdit>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Purchase Date">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ID="dt_Quotation" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="cPLQADate">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                    </dxe:ASPxLabel>
                                    <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%"
                                        DataValueField="Currency_ID" DataTextField="Currency_AlphaCode" onchange="ddl_Currency_Rate_Change()">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate">
                                        <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                        <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" SelectedIndex="0" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" LostFocus="function(s, e) { SetFocusonDemand(e)}" />
                                    </dxe:ASPxComboBox>
                                </div>

                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Place Of Supply[GST]">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <dxe:ASPxComboBox ID="ddlPosGstChallan" runat="server" ClientInstanceName="cddlPosGstChallan" Width="100%" ValueField="System.String">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateChallanPosGst(e)}" />
                                    </dxe:ASPxComboBox>
                                </div>
                                <div class="col-md-2 lblmTop8  hide" style="margin-bottom: 5px">
                                    <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                                    </dxe:ASPxLabel>
                                    <%--<span style="color: red;">*</span>--%>
                                    <asp:TextBox ID="txtEWayBillNumber" runat="server" Width="100%" MaxLength="20">                             
                                    </asp:TextBox>
                                    <span id="MandatoryEWayBillNumber" class="EWayBillNumber  pullleftClass fa fa-exclamation-circle iconRed "
                                        style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-4 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Narration">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ClientInstanceName="ctxtNarration" ID="txtNarration" runat="server" Width="100%" MaxLength="500">
                                    </dxe:ASPxTextBox>

                                </div>

                                <div class="col-md-2 lblmTop8">
                                    <%--<label id="lblProject" runat="server">Project</label>--%>
                                    <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="ProjectServerModeDataSource"
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
                                        <ClearButton DisplayMode="Always">
                                        </ClearButton>
                                    </dxe:ASPxGridLookup>
                                    <dx:LinqServerModeDataSource ID="ProjectServerModeDataSource" runat="server" OnSelecting="ProjectServerModeDataSource_Selecting"
                                        ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />

                                </div>

                                <div class="col-md-4 lblmTop8">
                                    <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                    </dxe:ASPxLabel>
                                    <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div>
                                <br />
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <dxe:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="SrlNo"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                        Settings-ShowFooter="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="160"
                                        OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
                                        OnCellEditorInitialize="grid_CellEditorInitialize" OnHtmlRowPrepared="grid_HtmlRowPrepared" OnDataBinding="grid_DataBinding"
                                        OnBatchUpdate="grid_BatchUpdate" OnCustomCallback="grid_CustomCallback" Settings-HorizontalScrollBarMode="Visible">
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Settings VerticalScrollableHeight="160" VerticalScrollBarMode="Auto"></Settings>
                                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30" VisibleIndex="0" Caption="">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                        <Image Url="/assests/images/crs.png"></Image>
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="RowNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="PO No" FieldName="DocNumber" ReadOnly="true" Width="12%" VisibleIndex="2">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="12%" ReadOnly="true">
                                                <PropertiesButtonEdit>
                                                    <%--<ClientSideEvents LostFocus="ProductsGotFocus" GotFocus="ProductsGotFocus" />--%>
                                                    <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                </PropertiesButtonEdit>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="ProductDiscription" Caption="Description" VisibleIndex="4" Width="20%" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn Caption="Addl Desc." Width="70" VisibleIndex="5">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton ID="CustomaddDescRemarks" Image-ToolTip="Remarks" Image-Url="/assests/images/warehouse.png" Text=" ">
                                                        <Image ToolTip="Addl Desc." Url="/assests/images/more.png">
                                                        </Image>
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="6" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit>
                                                    <ClientSideEvents GotFocus="QuantityProductsGotFocus" LostFocus="QuantityTextChange" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="PurchaseUOM" Caption="UOM" VisibleIndex="7" Width="4%" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn Width="4%" VisibleIndex="8" Caption="Stock">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                                        <Image Url="/assests/images/warehouse.png"></Image>
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="PurchasePrice" Caption="Price" VisibleIndex="9" Width="7%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.000">
                                                    <ClientSideEvents GotFocus="PurchasePriceTextFocus" LostFocus="PurchasePriceTextChange" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataSpinEditColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="10" Width="4%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" DecimalPlaces="2">
                                                    <ClientSideEvents GotFocus="DiscountTextFocus" LostFocus="DiscountValueChange" />
                                                    <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesSpinEdit>
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataSpinEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Amount" VisibleIndex="11" Width="9%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00">
                                                    <ClientSideEvents GotFocus="AmountTextFocus" LostFocus="AmountTextChange"></ClientSideEvents>
                                                    <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"></MaskSettings>
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <PropertiesTextEdit>
                                                    <ClientSideEvents GotFocus="AmountTextFocus" LostFocus="AmountTextChange" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="12" Width="9%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                                <PropertiesButtonEdit Style-HorizontalAlign="Right">
                                                    <ClientSideEvents ButtonClick="TaxAmountClick" GotFocus="TaxAmountFocus" KeyDown="TaxAmountKeyDown" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesButtonEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="NetAmount" Caption="Net Amount" VisibleIndex="13" Width="9%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00">
                                                    <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"></MaskSettings>
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="ChallanDetails_InlineRemarks" Width="150" VisibleIndex="14" PropertiesTextEdit-MaxLength="5000">
                                                <PropertiesTextEdit Style-HorizontalAlign="Left">
                                                    <Style HorizontalAlign="Left">
                                                            </Style>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="5%" VisibleIndex="15" Caption="Add New">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton ID="CustomAddNewRow" Image-Url="/assests/images/add.png" Text=" ">
                                                        <Image Url="/assests/images/add.png"></Image>
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="14" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="15" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="16" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="IsLinkedProduct" Caption="IsLinkedProduct" VisibleIndex="17" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="DocID" Caption="DocID" VisibleIndex="18" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="SrlNo" VisibleIndex="19" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="ProductID" VisibleIndex="27" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Doc_DetailsID" FieldName="Doc_DetailsID" Width="0" VisibleIndex="20" ReadOnly="true">
                                            </dxe:GridViewDataTextColumn>
                                            <%--Rev Mantis Issue 24061--%>
                                            <dxe:GridViewDataTextColumn FieldName="Balance_Amount" Caption="Balance Amount" VisibleIndex="25" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <%-- End of Rev Mantis Issue 24061--%>
                                        </Columns>
                                        <ClientSideEvents BatchEditStartEditing="gridFocusedRowChanged" EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" />
                                        <%--<ClientSideEvents  CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />--%>
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
                            <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px 0; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
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

                            <div class="clearfix" style="padding-top: 3px;">
                                <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {SaveNew_Click();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btn_SaveRecordsExit" ClientInstanceName="cbtn_SaveRecordsExit" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {SaveExit_Click();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecordsUDF" runat="server" AutoPostBack="False" Text="UDF" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                </dxe:ASPxButton>
                                <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                <asp:HiddenField ID="hfControlData" runat="server" />
                                <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                                <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="PC" />
                                <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
                                <asp:Label ID="lbl_IsTagged" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                <asp:CheckBox ID="chkmail" runat="server" Text="Send Mail" />
                            </div>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="[B]illing/Shipping" Text="Our Billing/Shipping">
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
    <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
        Width="1000px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter" Height="500px"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" AllowDragging="true">
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
                <asp:HiddenField runat="server" ID="HiddenField1" />
                <asp:HiddenField runat="server" ID="HiddenField2" />
                <asp:HiddenField runat="server" ID="HiddenField3" />
                <asp:HiddenField runat="server" ID="HiddenField4" />
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
                                        <td>Net Amount
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
                                Settings-ShowFooter="false" AutoGenerateColumns="False" OnCellEditorInitialize="aspxGridTax_CellEditorInitialize"
                                OnRowInserting="taxgrid_RowInserting" OnRowUpdating="taxgrid_RowUpdating" OnRowDeleting="taxgrid_RowDeleting">
                                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                <SettingsPager Visible="false"></SettingsPager>
                                <Columns>
                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Taxes_Name" ReadOnly="true" Caption="Tax Component ID">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="taxCodeName" ReadOnly="true" Caption="Tax Component Name">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                            <MaskSettings Mask="<0..999999999999>.<00..999>" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch">
                                    <BatchEditSettings EditMode="row" ShowConfirmOnLosingChanges="false" />
                                </SettingsEditing>
                                <ClientSideEvents EndCallback="cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />
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
                                            ClearButton-DisplayMode="Always">
                                            <Columns>
                                                <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />
                                            </Columns>
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
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="pull-left">
                                <asp:Button ID="Button1" runat="server" Text="Ok" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
                                <asp:Button ID="Button3" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
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
    <div class="PopUpArea">
        <%--ChargesTax--%>
        <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
            Width="1000px" Height="400px" HeaderText="GRN Taxes" PopupHorizontalAlign="WindowCenter"
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
                                                        <strong>
                                                            <dxe:ASPxLabel ID="txtProductNetAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductNetAmount"></dxe:ASPxLabel>
                                                        </strong>
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
                                <Settings VerticalScrollableHeight="300" VerticalScrollBarMode="Auto"></Settings>
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
                            </dxe:ASPxGridView>
                        </div>
                        <div class="col-md-12">
                            <table style="" class="chargesDDownTaxClass">
                                <tr class="chargeGstCstvatClass">
                                    <td style="padding-top: 10px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                    <td style="padding-top: 10px; width: 200px;">
                                        <dxe:ASPxComboBox ID="cmbGstCstVatcharge" ClientInstanceName="ccmbGstCstVatcharge" runat="server" SelectedIndex="0" TabIndex="2"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}">
                                            <Columns>
                                                <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />
                                            </Columns>
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="padding-left: 15px; padding-top: 10px; width: 200px;">
                                        <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" ClientInstanceName="ctxtGstCstVatCharge" TabIndex="3" ReadOnly="true" Text="0.00"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;-999999999..999999999g&gt;.&lt;00..99&gt;" />
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
                                <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AccessKey="X" AutoPostBack="False" Text="Ok" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {cPopup_Taxes.Hide();}" />
                                </dxe:ASPxButton>
                            </div>
                        </div>
                        <div class="col-sm-9">
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-right: 30px; width: 114px"><strong>Total Charges</strong></td>
                                    <td>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.0000" HorizontalAlign="Right" Font-Size="12px" ClientEnabled="false">
                                                <MaskSettings Mask="<-9999999..9999999g>.<00..99>" AllowMouseWheel="false" />
                                                <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                            </dxe:ASPxTextBox>
                                        </div>

                                    </td>
                                    <td style="padding-right: 30px; padding-left: 5px; width: 114px"><strong>Total Amount</strong></td>
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
    </div>
    <div>
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
                    <div>
                        <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                            Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
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
                                <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                    <PropertiesTextEdit>
                                        <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="ComponentDetailsID" ReadOnly="true" Caption="ComponentDetailsID" Width="0">
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsDataSecurity AllowEdit="true" />
                            <ClientSideEvents EndCallback="gridProducts_EndCallback" />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="text-center">
                        <dxe:ASPxButton ID="btn_gridproducts" ClientInstanceName="cbtn_gridproducts" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                            <ClientSideEvents Click="function(s, e) {PerformCallToGridBind();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
    <div>
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
                        <div class="clear">
                            <br />
                        </div>
                        <dxe:ASPxCallbackPanel runat="server" ID="WarehousePanel" ClientInstanceName="cWarehousePanel" OnCallback="WarehousePanel_Callback">
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                                        <div>
                                            <div class="col-md-3" id="div_Warehouse">
                                                <div>
                                                    Warehouse
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxComboBox ID="CmbWarehouseID" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouseID" SelectedIndex="0"
                                                        TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                    <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="div_Batch">
                                                <div>
                                                    Batch/Lot
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtBatchName" runat="server" Width="100%" ClientInstanceName="ctxtBatchName" HorizontalAlign="Left" Font-Size="12px">
                                                    </dxe:ASPxTextBox>
                                                    <span id="spntxtBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="div_Manufacture">
                                                <div>
                                                    Manufacture Date
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxDateEdit ID="txtStartDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtStartDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="div_Expiry">
                                                <div>
                                                    Expiry Date
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxDateEdit ID="txtEndDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtEndDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="clear" id="div_Break"></div>
                                            <div class="col-md-3" id="div_Quantity">
                                                <div>
                                                    Quantity
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                        <%--<ClientSideEvents TextChanged="function(s, e) {SubmitWarehouse();}" />--%>
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                    <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="Altdiv_Quantity">
                                                <div>
                                                    Alt. Quantity
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtAltQuantity" runat="server" ClientInstanceName="ctxtAltQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />

                                                    </dxe:ASPxTextBox>

                                                </div>
                                            </div>
                                            <div class="col-md-3" id="div_Serial">
                                                <div>
                                                    Serial No
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtserialID" runat="server" Width="100%" ClientInstanceName="ctxtserialID" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                                        <ClientSideEvents LostFocus="SubmitWarehouse" />
                                                    </dxe:ASPxTextBox>
                                                    <span id="spntxtserialID" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="div_Upload">
                                                <div class="col-md-3">
                                                    <div>
                                                    </div>
                                                    <%-- <dxe:ASPxButton ID="btnUploadSerial" ClientInstanceName="cbtnUploadSerial" Width="50px" runat="server" AutoPostBack="False" Text="Upload Serial" CssClass="btn btn-primary">
                                                        <ClientSideEvents Click="UploadSerial" />
                                                    </dxe:ASPxButton>--%>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <div>
                                                </div>
                                                <div class="Left_Content" style="padding-top: 14px">
                                                    <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                        <ClientSideEvents Click="SubmitWarehouse" />
                                                    </dxe:ASPxButton>

                                                    <dxe:ASPxButton ID="btnClear" ClientInstanceName="cbtnClear" Width="50px" runat="server" AutoPostBack="False" Text="Clear" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                        <ClientSideEvents Click="ClearWarehouse" />
                                                    </dxe:ASPxButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                        <dxe:ASPxGridView ID="GrdWarehouse" ClientInstanceName="cGrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                                            Width="100%" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200"
                                            SettingsBehavior-AllowSort="false" OnDataBinding="GrdWarehouse_DataBinding">
                                            <%--OnCustomCallback="GrdWarehouse_CustomCallback" --%>
                                            <Columns>
                                                <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                                    VisibleIndex="0">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Batch/Lot Number" FieldName="BatchNo"
                                                    VisibleIndex="1">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="ViewMfgDate"
                                                    VisibleIndex="2">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ViewExpiryDate"
                                                    VisibleIndex="3">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                                    VisibleIndex="4">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                                    VisibleIndex="5">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="6" Width="120px">
                                                    <DataItemTemplate>
                                                        <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete" style='<%#Eval("IsOutStatus")%>'>
                                                            <img src="../../../assests/images/Edit.png" /></a>
                                                        &nbsp;
                                        <a href="javascript:void(0);" onclick="fn_Delete('<%# Container.KeyValue %>')" title="Delete" style='<%#Eval("IsOutStatus")%>'>
                                            <img src="/assests/images/crs.png" /></a>
                                                        <a class="anchorclass" style='<%#Eval("IsOutStatusMsg")%>'>Already used</a>
                                                    </DataItemTemplate>
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
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
                                </dxe:PanelContent>
                            </PanelCollection>
                            <ClientSideEvents EndCallback="WarehousePanelEndCall" />
                        </dxe:ASPxCallbackPanel>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
    <div>
        <dxe:ASPxPopupControl ID="popup_taggingGrid" runat="server" ClientInstanceName="cpopup_taggingGrid"
            HeaderText="Select Purchase Order" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Height="300px" Width="750px"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <input type="button" value="Select All" onclick="Tag_ChangeState('SelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="De-select All" onclick="Tag_ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="Revert" onclick="Tag_ChangeState('Revart')" class="btn btn-primary"></input>
                    </div>
                    <div>
                        <dxe:ASPxGridView ID="taggingGrid" ClientInstanceName="ctaggingGrid" runat="server" KeyFieldName="PurchaseOrder_Id"
                            Width="100%" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                            Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible" Settings-HorizontalScrollBarMode="Visible"
                            OnCustomCallback="taggingGrid_CustomCallback" OnDataBinding="taggingGrid_DataBinding">
                            <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" FixedStyle="Left" />
                                <dxe:GridViewDataTextColumn FieldName="PurchaseOrder_Number" Caption="Purchase Order Number" Width="150" VisibleIndex="1" FixedStyle="Left">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ComponentDate" Caption="Purchase Order Date" Width="100" VisibleIndex="2" FixedStyle="Left">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Vendor Name" Width="180" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ReferenceName" Caption="Reference" Width="150" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Tax_Option" Caption="Tax" Width="1" VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="RevNo" Caption="Revision No" Width="150" VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Proj_Code" Caption="Project Code" Width="150" VisibleIndex="7">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Proj_Name" Caption="Project Name" Width="150" VisibleIndex="8">
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsDataSecurity AllowEdit="true" />
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="text-center">
                        <dxe:ASPxButton ID="btnTaggingSave" ClientInstanceName="cbtnTaggingSave" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                            <ClientSideEvents Click="function(s, e) {QuotationNumberChanged();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
    <asp:SqlDataSource ID="VendorDataSource" runat="server" />
    <asp:SqlDataSource ID="ProductDataSource" runat="server" />
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <div>
        <asp:HiddenField runat="server" ID="hdnBranchID" />
        <asp:HiddenField runat="server" ID="hdnCustomerId" />
        <asp:HiddenField runat="server" ID="hdfIsDelete" />
        <asp:HiddenField runat="server" ID="hfProduct_Json" />
        <asp:HiddenField runat="server" ID="hdnPageStatus" />
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <%--Subhra 14-03-2019--%>
        <asp:HiddenField runat="server" ID="Keyval_Id" />
        <%--End Subhra 14-03-2019--%>
        <asp:HiddenField runat="server" ID="hdnRefreshType" />
        <asp:HiddenField runat="server" ID="hdnJsonProductTax" />
        <asp:HiddenField runat="server" ID="hdnJsonProductStock" />
        <asp:HiddenField runat="server" ID="hdnJsonTempStock" />
        <asp:HiddenField runat="server" ID="hdndefaultWarehouse" />
        <asp:HiddenField runat="server" ID="IsBarcodeActive" />

        <asp:HiddenField runat="server" ID="hdfProductID" />
        <asp:HiddenField runat="server" ID="hdfWarehousetype" />
        <asp:HiddenField runat="server" ID="hdfProductSrlNo" />
        <asp:HiddenField runat="server" ID="hdnProductQuantity" />
        <asp:HiddenField runat="server" ID="hdfUOM" />
        <asp:HiddenField runat="server" ID="hdfServiceURL" />
        <asp:HiddenField runat="server" ID="hdfBranch" />
        <asp:HiddenField runat="server" ID="hdfIsRateExists" />
        <asp:HiddenField runat="server" ID="hdfIsBarcodeGenerator" />

        <asp:HiddenField runat="server" ID="setCurrentProdCode" />
        <asp:HiddenField runat="server" ID="HdSerialNo" />
        <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
        <asp:HiddenField runat="server" ID="HdProdNetAmt" />
        <asp:HiddenField runat="server" ID="HdChargeProdAmt" />
        <asp:HiddenField runat="server" ID="HdChargeProdNetAmt" />

        <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
        <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
        <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
        <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
        <asp:HiddenField ID="hdfEWayBillMendatory" runat="server" />
        <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
        <asp:HiddenField ID="hdnEntityType" runat="server" />
        <asp:HiddenField runat="server" ID="hdnQty" />
        <asp:HiddenField ID="hdnADDEditMode" runat="server" />
    </div>
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
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Unique Id</th>
                                <th>Vendor Name</th>
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
                <div class="modal-footer">
                    <% if (rightsProd.CanAdd)
                       { %>
                    <button type="button" class="btn btn-success btn-radius" onclick="fn_PopOpen();">
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

    <!--Product Stock Modal -->
    <dxe:ASPxPopupControl ID="PopupWarehouse" runat="server" ClientInstanceName="cPopupWarehouse"
        Width="850px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeStockPopup(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div id="content-1" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
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
                                                <asp:Label ID="lblProductName" runat="server"></asp:Label>
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
                                            <td>Entered Quantity </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEnteredAmount" runat="server"></asp:Label>
                                                <asp:Label ID="lblEnteredUOM" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="clear"></div>
                <div id="StockHeader">
                    <div class="clearfix  modal-body" style="padding: 8px 0 8px 0; margin-bottom: 15px; margin-top: 15px; border-radius: 4px; border: 1px solid #ccc;">
                        <div class="col-md-12">
                            <div class="clearfix  row">
                                <div class="col-md-3" id="_div_Warehouse">
                                    <div>
                                        Warehouse
                                    </div>
                                    <div class="Left_Content" style="">
                                        <asp:DropDownList ID="ddlWarehouse" runat="server" Width="100%">
                                        </asp:DropDownList>
                                        <span id="rfvWarehouse" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_div_Batch">
                                    <div>
                                        Batch
                                    </div>
                                    <div class="Left_Content" style="">
                                        <input type="text" id="txtBatch" placeholder="Batch" />
                                        <span id="rfvBatch" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_div_Manufacture">
                                    <div>
                                        Manufacture Date
                                    </div>
                                    <div class="Left_Content" style="">
                                        <%--<input type="text" id="txtMfgDate" placeholder="Mfg Date" />--%>
                                        <dxe:ASPxDateEdit ID="txtMfgDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True"
                                            ClientInstanceName="ctxtMfgDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_div_Expiry">
                                    <div>
                                        Expiry Date
                                    </div>
                                    <div class="Left_Content" style="">
                                        <%-- <input type="text" id="txtExprieyDate" placeholder="Expiry Date" />--%>
                                        <dxe:ASPxDateEdit ID="txtExprieyDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True"
                                            ClientInstanceName="ctxtExprieyDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                                <div class="clear" id="_div_Break"></div>
                                <div class="col-md-3" id="_div_Rate">
                                    <div>
                                        Rate
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtRate" runat="server" ClientInstanceName="ctxtRate" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_div_Quantity">
                                    <div>
                                        Quantity
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtQty" runat="server" ClientInstanceName="ctxtQty" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" AllowMouseWheel="false" />
                                            <%-- <ClientSideEvents TextChanged="function(s, e) {SubmitWarehouse();}" />--%>
                                            <ClientSideEvents TextChanged="function(s,e) { ChangePackingByQuantityinjs();}" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                        </dxe:ASPxTextBox>
                                        <span id="rfvQuantity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_Altdiv_Quantity" runat="server">
                                    <div>
                                        Alt. Quantity
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtAltQty" runat="server" ClientInstanceName="ctxtAltQty" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                            <ClientSideEvents TextChanged="function(s,e) { ChangeQuantityByPacking1();}" />
                                        </dxe:ASPxTextBox>

                                    </div>
                                </div>

                                <div class="col-md-3" id="dv_AltUOM" runat="server">
                                    <div style="margin-bottom: 2px;">
                                        Alt. UOM
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxComboBox ID="cmbPackingUom1" ClientInstanceName="ccmbAltUOM" runat="server" SelectedIndex="0"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>

                                <div class="col-md-3" id="_div_Serial">
                                    <div>
                                        Serial No
                                    </div>
                                    <div class="Left_Content" style="">
                                        <input type="text" id="txtSerial" placeholder="Serial No" onkeyup="Serialkeydown(event)" />
                                        <span id="rfvSerial" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>

                                <div class="col-md-3" id="_div_Upload">
                                    <div class="col-md-3">
                                        <div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div>
                                    </div>
                                    <div class="Left_Content">
                                        <input type="button" onclick="SaveStockPC()" value="Add" class="btn btn-primary" />
                                        <input id="btnSecondUOM" type="button" onclick="AlternateUOMDetails('PC')" value="Alt Unit Details" class="btn btn-success" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="showData" class="gridStatic">
                </div>
                <div class="clearfix  row">
                    <div class="col-md-3">
                        <div>
                        </div>
                        <div class="Left_Content" style="padding-top: 14px">
                            <input type="button" onclick="FullnFinalSave()" value="Ok" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <!--Product Stock Modal -->

    <%--Rev 2.0 Subhra 11-03-2019--%>
    <div>
        <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
        <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
    </div>
    <%--End of Rev 2.0 Subhra 11-03-2019--%>

    <dxe:ASPxCallback ID="DeletePanel" runat="server" OnCallback="DeletePanel_Callback" ClientInstanceName="cDeletePanel">
    </dxe:ASPxCallback>



    <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="650px"
        Width="1020px" HeaderText="Add New Vendor" Modal="true" AllowResize="false" ResizingMode="Postponed">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>



    <dxe:ASPxPopupControl ID="SecondUOMpopup" runat="server" ClientInstanceName="cSecondUOM" ShowCloseButton="false"
        Width="850px" HeaderText="Second UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="clearfix boxStyle">
                    <div class="col-md-3">
                        <label>Length (inch)</label>
                        <dxe:ASPxTextBox runat="server" ID="txtLength" ClientInstanceName="ctxtLength">
                            <ClientSideEvents LostFocus="SizeLostFocus" />

                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />

                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Width (inch)</label>
                        <dxe:ASPxTextBox runat="server" ID="txtWidth" ClientInstanceName="ctxtWidth">
                            <ClientSideEvents LostFocus="SizeLostFocus" />
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />

                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Total (Sq. Feet)</label>
                        <dxe:ASPxTextBox runat="server" ID="txtTotal" ClientEnabled="false" ClientInstanceName="ctxtTotal">
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3 padTop23 pdLeft0">
                        <label></label>
                        <button type="button" onclick="AddSecondUOMDetails('PC');" class="btn btn-primary">Add</button>
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
                                <th>SL</th>
                                <th>Branch</th>
                                <th>Warehouse</th>
                                <th>Size</th>
                                <th>Total</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody id="tbodySecondUOM">
                        </tbody>
                    </table>
                </div>
                <div class="col-md-12 text-right pdTop15">
                    <button class="btn btn-success" type="button" onclick="SavePOESecondUOMDetails();">OK</button>
                    <button class="btn btn-danger hide" type="button" onclick="return cSecondUOM.Hide();">Cancel</button>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>

    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdnAlternateProdId" runat="server" />


    <asp:HiddenField runat="server" ID="hdnChallanType" />
    <dxe:ASPxPopupControl ID="PosView" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cPosView" Height="650px"
        Width="1200px" HeaderText="Product" Modal="true">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <dxe:ASPxCallbackPanel runat="server" ID="callback_InlineRemarks" ClientInstanceName="ccallback_InlineRemarks" OnCallback="callback_InlineRemarks_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <dxe:ASPxPopupControl ID="Popup_InlineRemarks" runat="server" ClientInstanceName="cPopup_InlineRemarks"
                    Width="900px" HeaderText="Additional Description" PopupHorizontalAlign="WindowCenter"
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
                                <asp:Label ID="lblInlineRemarks" runat="server"></asp:Label>

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
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField runat="server" ID="hdnpackingqty" />
    <asp:HiddenField runat="server" ID="hdnuomFactor" />
    <asp:HiddenField runat="server" ID="hdnisOverideConvertion" />
    <asp:HiddenField ID="hdnBackdateddate" runat="server" />

    <asp:HiddenField ID="hdnTagDateForbackdated" runat="server" />
    <%--Rev Mantis Issue 24061--%>
    <asp:HiddenField runat="server" ID="hdnPurchaseOrderItemNegative" />
    <%--End of Rev Mantis Issue 24061--%>
     <%--Rev 1.0--%>
    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
    <asp:HiddenField ID="hdnValAfterLock" runat="server" />
    <asp:HiddenField ID="hdnValAfterLockMSG" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
    <asp:HiddenField ID="hdnLockToDateedit" runat="server" /> 
    <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />
    <%--End of Rev 1.0--%>
 <%-- Rev 2.0--%>
 <asp:HiddenField runat="server" ID="hdnIsDuplicateItemAllowedOrNot" />
 <%-- Rev 2.0 End--%>
</asp:Content>

