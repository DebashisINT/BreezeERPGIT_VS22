<%@ Page Title="Purchase Challan" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="Stock-Transfer.aspx.cs" Inherits="ERP.OMS.Management.Activities.Stock_Transfer" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    <script src="JS/ProductStockIN.js"></script>

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

        #grid_DXMainTable > tbody > tr > td:last-child {
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
        var ShouldCheck;
        var _ComponentDetails;

        function disp_prompt(name) {
            if (name == "tab0") {
                cContactPerson.Focus();
            }
            if (name == "tab1") {
                var custID = GetObjectID('hdnCustomerId').value;
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
            if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
            
            $('#<%=hdnBranchID.ClientID %>').val(branchID);
            $('#txtVoucherNo').attr('maxLength', schemelength);

            ctxtCustName.SetText("");
            GetObjectID('hdnCustomerId').value = "";
            page.tabs[1].SetEnabled(false);

            var schemetypeValue = val;
            var schemetype = schemetypeValue.toString().split('~')[1];
            var schemelength = schemetypeValue.toString().split('~')[2];
            $('#txtVoucherNo').attr('maxLength', schemelength);
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
    //    cddlVatGstCst.Focus();
    //}
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

function QuotationNumberChanged() {
    var OrderData = ctaggingGrid.GetSelectedKeysOnPage();

    if(OrderData==0){
        cpopup_taggingGrid.Hide();
    }
    else{
        cgridproducts.PerformCallback('BindProductsDetails');
        cpopup_taggingGrid.Hide();
        cProductsPopup.Show();
    }
}

function gridProducts_EndCallback(s, e) {
    if (cgridproducts.cpComponentDetails) {
        _ComponentDetails=cgridproducts.cpComponentDetails;
        cgridproducts.cpComponentDetails = null;
    }
}

function ChangeState(value) {
    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}

function Tag_ChangeState(value) {
    ctaggingGrid.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}

function PerformCallToGridBind() {
    var OrderTaggingData = cgridproducts.GetSelectedKeysOnPage();

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
            BSDocTagging(quote_Id[0], 'PO');
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
            cddl_AmountAre.SetEnabled(false);
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

            grid.batchEditApi.StartEdit(globalRowIndex, 5);

            if (grid.GetEditor("ProductID").GetValue() != "" && grid.GetEditor("ProductID").GetValue() != null) {
                var previousProductID = grid.GetEditor("ProductID").GetValue();
                var _previousProductID = previousProductID.split("||@||")[0];

                cDeletePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());  
            }

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
            if (e.htmlEvent.key == "Enter") {
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
            else if (grid.cpSaveSuccessOrFail == "transactionbeingused") {
                LoadingPanel.Hide();
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Transaction exist. cannot be processed.');
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
                var Order_Msg = "Purchase Challan No. " + PurchaseOrder_Number + " saved.";
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


                        jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {
                            if (r == true) {
                                grid.cpSalesOrderNo = null;
                                window.location.assign("PurchaseChallanList.aspx");
                            }
                        });

                    }
                    else {
                        window.location.assign("PurchaseChallanList.aspx");
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
                                window.location.assign("PurchaseChallan.aspx?key=ADD");
                            }
                        });
                    }
                    else {
                        window.location.assign("PurchaseChallan.aspx?key=ADD");
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
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;

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

                    $('#<%=hdfProductID.ClientID %>').val(ProductID);
                    $('#<%=hdfWarehousetype.ClientID %>').val(Warehousetype);
                    $('#<%=hdfProductSrlNo.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
                    $('#<%=hdfProductUOM.ClientID %>').val(Purchase_UOM);

                    document.getElementById('<%=lblProductName.ClientID %>').innerHTML = Product_Name;
                    document.getElementById('<%=lblEnteredAmount.ClientID %>').innerHTML = QuantityValue;
                    document.getElementById('<%=lblEnteredUOM.ClientID %>').innerHTML = Purchase_UOM;

                    Stock_EditID = "0";
                    //cWarehousePanel.PerformCallback('StockDisplay');

                    CreateStock();
                    $('#ProductStock').modal('show');
                }
            }
        }
        else {
            jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
        }
    }
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
    }
    else {
        Pre_Quantity = "0";
        Pre_Amt = "0";
        Pre_TotalAmt = "0";
    }
}

function QuantityTextChange(s, e) {
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    if (ProductID != null) {
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
        var Amount = (parseFloat(Quantity)*parseFloat(Purchase_UOM_Factor)*parseFloat(ConversionMultiplier)) * (parseFloat(PurchasePrice) / parseFloat(strRate));
        var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

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

        caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), 'P');
        
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

function AmountTextChange(s, e) {
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

        caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, Amount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), 'P');

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
            if (e.htmlEvent.key == "Enter") {
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

                        //Set Product Gross Amount and Net Amount

                        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                        var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "";
                        var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
                        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                        var Purchase_UOM_Factor = SpliteDetails[24];
                        var ConversionMultiplier = SpliteDetails[25];
                        if (strRate == 0) strRate = 1;

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
                            shippingStCode = cbsSCmbState.GetText();
                            shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();
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
                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

                    //Call for Running Total
                    SetRunningTotal();

                } else {
                    s.SetText("");
                }
            }

            RecalCulateTaxTotalAmountInline();
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


            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            //Set Running Total
            SetRunningTotal();

            RecalCulateTaxTotalAmountInline();
        }
        function GetTaxVisibleIndex(s, e) {
            globalTaxRowIndex = e.visibleIndex;
        }


        function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
            for (var i = 0; i < taxJson.length; i++) {
                if (taxJson[i].applicableBy == name) {
                    cgridTax.batchEditApi.StartEdit(i, 3);
                    cgridTax.GetEditor('calCulatedOn').SetValue(amt);

                    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                    var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                    var s = cgridTax.GetEditor("TaxField");
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




                }
            }
            //return;
            cgridTax.batchEditApi.EndEdit();

        }

        function SetRunningTotal() {
            var runningTot = parseFloat(clblProdNetAmt.GetValue());
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                if (taxJson[i].applicableOn == "R") {
                    cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
                    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                    var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                    var thisRunningAmt = 0;
                    if (sign == '(+)') {
                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        GlobalCurTaxAmt = 0;
                    }
                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
                }
                runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
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

            if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
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
            }

            //Debjyoti 
            document.getElementById('HdChargeProdAmt').value = sumAmount;
            document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
            //End Here

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

                    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
                    gridTax.cpTotalCharges = null;
                }
            }

            SetChargesRunningTotal();
            ShowTaxPopUp("IN");
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

        function SaveNew_Click() {
            flag = true;
            LoadingPanel.Show();

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
                if (IsProduct == "Y") {
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
                else {
                    LoadingPanel.Hide();
                    jAlert('Please add atleast single record first');
                }
            }
        }

        function SaveExit_Click() {
            flag = true;
            LoadingPanel.Show();

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
                if (IsProduct == "Y") {
                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                    var JsonProductList = JSON.stringify(TaxOfProduct);
                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    
                    var JsonProductStock = JSON.stringify(StockOfProduct);
                    GetObjectID('hdnJsonProductStock').value = JsonProductStock;                    

                    grid.AddNewRow();
                    grid.UpdateEdit();
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
        }

        function closeWarehouse(s, e) {
            e.cancel = false;
            cWarehousePanel.PerformCallback('WarehouseDelete');
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

                cWarehousePanel.PerformCallback('StockSave~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + MfgDate + '~' + ExpiryDate + '~' + SerialNo + '~' + Qty + '~' + Stock_EditID);
                Stock_EditID = "0";
            }
        }

        function FinalWarehouse() {
            cWarehousePanel.PerformCallback('WarehouseFinal');
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
            if (e.htmlEvent.key == "Enter") {
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

                LoadCustomerAddress(VendorID, $('#ddl_Branch').val(), 'PC');
                page.tabs[0].SetEnabled(true);
                page.tabs[1].SetEnabled(true);

                GetObjectID('hdnCustomerId').value = VendorID;
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

        function ValueSelected(e, indexName) {
            if (e.code == "Enter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex")
                        SetProduct(Id, name);
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
                HeaderCaption.push("Name");
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

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Stock Transfer"></asp:Label>
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
            <div id="divcross" runat="server" class="crossBtn"><a href="PurchaseChallanList.aspx"><i class="fa fa-times"></i></a></div>
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
                                        <asp:ListItem Text="Non Inventory Item" Value="N" />
                                        <asp:ListItem Text="Capital Goods" Value="C" />
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
                                    <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLQuoteDate" Width="100%" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
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
                                <div class="col-md-2 lblmTop8  hide" style="margin-bottom: 5px">
                                    <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                                <div class="clear"></div>
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
                                        OnBatchUpdate="grid_BatchUpdate" OnCustomCallback="grid_CustomCallback">
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
                                            <dxe:GridViewDataTextColumn FieldName="RowNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="30px">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>


                                            <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" ReadOnly="true" Width="130" VisibleIndex="2">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="150" ReadOnly="true">
                                                <PropertiesButtonEdit>
                                                    <%--<ClientSideEvents LostFocus="ProductsGotFocus" GotFocus="ProductsGotFocus" />--%>
                                                    <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                </PropertiesButtonEdit>
                                            </dxe:GridViewDataButtonEditColumn>

                                            <dxe:GridViewDataTextColumn FieldName="ProductDiscription" Caption="Description" VisibleIndex="4" Width="18%" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="5" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit>
                                                    <ClientSideEvents GotFocus="QuantityProductsGotFocus" LostFocus="QuantityTextChange" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn FieldName="PurchaseUOM" Caption="UOM" VisibleIndex="6" Width="4%" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>



                                            <dxe:GridViewDataTextColumn FieldName="PurchasePrice" Caption="Price" VisibleIndex="8" Width="7%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00">
                                                    <ClientSideEvents GotFocus="PurchasePriceTextFocus" LostFocus="PurchasePriceTextChange" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>



                                            <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Amount" VisibleIndex="10" Width="9%" HeaderStyle-HorizontalAlign="Right">
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
                                            <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="ProductID" VisibleIndex="20" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>



                                        </Columns>
                                        <ClientSideEvents BatchEditStartEditing="gridFocusedRowChanged" EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" />
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
                            <div>
                                <br />
                            </div>
                            <div class="col-md-12" style="padding-top: 15px;">
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
                                <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="PC" />
                                <asp:Label ID="lbl_IsTagged" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                <asp:CheckBox ID="chkmail" runat="server" Text="Send Mail" />
                            </div>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="[B]illing/Shipping" Text="Our Billing/Shipping">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
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
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
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
            Width="900px" Height="300px" HeaderText="GRN Taxes" PopupHorizontalAlign="WindowCenter"
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
                                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                <SettingsPager Visible="false"></SettingsPager>
                                <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="TaxName" Caption="Tax" VisibleIndex="0" Width="40%" ReadOnly="true">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="calCulatedOn" Caption="Calculated On" VisibleIndex="0" Width="20%" ReadOnly="true">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="PercentageTextChange" />
                                            <ClientSideEvents />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
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
                                <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AccessKey="X" AutoPostBack="False" Text="Ok" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                    <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
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
                                            <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                <MaskSettings Mask="<-999999999..99999999999999999999>.<0..99>" AllowMouseWheel="false" />
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
                                                        <ClientSideEvents TextChanged="function(s, e) {SubmitWarehouse();}" />
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                    <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
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
                                                    <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                                        <ClientSideEvents Click="SubmitWarehouse" />
                                                    </dxe:ASPxButton>

                                                    <dxe:ASPxButton ID="btnClear" ClientInstanceName="cbtnClear" Width="50px" runat="server" AutoPostBack="False" Text="Clear" CssClass="btn btn-primary">
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
                                            <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
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
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Height="400px" Width="850px"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <input type="button" value="Select All Products" onclick="Tag_ChangeState('SelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="De-select All Products" onclick="Tag_ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="Revert" onclick="Tag_ChangeState('Revart')" class="btn btn-primary"></input>
                    </div>
                    <div>
                        <dxe:ASPxGridView ID="taggingGrid" ClientInstanceName="ctaggingGrid" runat="server" KeyFieldName="PurchaseOrder_Id"
                            Width="100%" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                            Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                            OnCustomCallback="taggingGrid_CustomCallback" OnDataBinding="taggingGrid_DataBinding">
                            <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                <dxe:GridViewDataTextColumn FieldName="PurchaseOrder_Number" Caption="PurchaseOrder_Number" Width="150" VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ComponentDate" Caption="Purchase Date" Width="100" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Vendor Name" Width="150" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ReferenceName" Caption="Reference" Width="150" VisibleIndex="4">
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
    <asp:SqlDataSource ID="VendorDataSource" runat="server"  />
    <asp:SqlDataSource ID="ProductDataSource" runat="server"  />
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
        <asp:HiddenField runat="server" ID="hdnRefreshType" />
        <asp:HiddenField runat="server" ID="hdnJsonProductTax" />
        <asp:HiddenField runat="server" ID="hdnJsonProductStock" />

        <asp:HiddenField runat="server" ID="hdfProductID" />
        <asp:HiddenField runat="server" ID="hdfWarehousetype" />
        <asp:HiddenField runat="server" ID="hdfProductSrlNo" />
        <asp:HiddenField runat="server" ID="hdnProductQuantity" />
        <asp:HiddenField runat="server" ID="hdfProductUOM" />

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
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
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

    <!--Product Stock Modal -->
    <div class="modal fade" id="ProductStock" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product STock Details</h4>
                </div>
                <div class="modal-body">
                    <div class="clearfix  row">
                        <div class="col-md-3" id="_div_Warehouse">
                            <div>
                                Warehouse
                            </div>
                            <div class="Left_Content" style="">
                                <asp:DropDownList ID="ddlWarehouse" runat="server" Width="100%">
                                    <asp:ListItem Value="1">New Delhi </asp:ListItem>
                                    <asp:ListItem Value="2">Greater Noida</asp:ListItem>
                                    <asp:ListItem Value="3">NewYork</asp:ListItem>
                                    <asp:ListItem Value="4">Paris</asp:ListItem>
                                    <asp:ListItem Value="5">London</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3" id="_div_Batch">
                            <div>
                                Batch/Lot
                            </div>
                            <div class="Left_Content" style="">
                                <input type="text" id="txtBatch" placeholder="Batch" />
                            </div>
                        </div>
                        <div class="col-md-3" id="_div_Manufacture">
                            <div>
                                Manufacture Date
                            </div>
                            <div class="Left_Content" style="">
                                <input type="text" id="txtMfgDate" placeholder="Mfg Date" />
                            </div>
                        </div>
                        <div class="col-md-3" id="_div_Expiry">
                            <div>
                                Expiry Date
                            </div>
                            <div class="Left_Content" style="">
                                <input type="text" id="txtExprieyDate" placeholder="Expiry Date" />
                            </div>
                        </div>
                        <div class="clear" id="_div_Break">
                        </div>
                        <div class="col-md-3" id="_div_Quantity">
                            <div>
                                Quantity
                            </div>
                            <div class="Left_Content" style="">
                                <dxe:ASPxTextBox ID="txtQty" runat="server" ClientInstanceName="ctxtQty" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                    <ClientSideEvents TextChanged="function(s, e) {SubmitWarehouse();}" />
                                    <ValidationSettings Display="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-3" id="_div_Serial">
                            <div>
                                Serial No
                            </div>
                            <div class="Left_Content" style="">
                                <input type="text" id="txtSerial" placeholder="Serial No" onkeydown="Serialkeydown(event)" />
                            </div>
                        </div>
                        <div class="col-md-3" id="_div_Upload">
                            <div class="col-md-3">
                                <div>
                                </div>
                                <%-- <dxe:ASPxButton ID="btnUploadSerial" ClientInstanceName="cbtnUploadSerial" Width="50px" runat="server" AutoPostBack="False" Text="Upload Serial" CssClass="btn btn-primary">
                    <ClientSideEvents Click="UploadSerial" />
                </dxe:ASPxButton>--%>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div>
                            </div>
                            <div class="Left_Content" style="padding-top: 14px">
                                <input type="button" onclick="SaveStock()" value="Add" class="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                    <div id="showData" class="gridStatic">
                    </div>
                    <%-- <div class="clearfix  row">
                        <div class="col-md-3">
                            <div>
                            </div>
                            <div class="Left_Content" style="padding-top: 14px">
                                <input type="button" onclick="FinalSaveStock()" value="Add" class="btn btn-primary" />
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Product Stock Modal -->

    <dxe:ASPxCallback ID="DeletePanel" runat="server" OnCallback="DeletePanel_Callback" ClientInstanceName="cDeletePanel">
    </dxe:ASPxCallback>

</asp:Content>
