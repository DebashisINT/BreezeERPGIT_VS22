<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Replacementnotestockout.aspx.cs" Inherits="ERP.OMS.Management.Activities.Replacementnotestockout" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="Replacementnotestockout.aspx.cs" Inherits="ERP.OMS.Management.Activities.Replacementnotestockout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

        #gridReplacement_DXStatus span > a {
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
    </style>



    <script>

        $(document).ready(function () {

            //$('body').on('change', 'ddl_numberingScheme', function () {
            //    alert();
            //});


            $('#ddl_numberingScheme').change(function () {

                $('#duplicateQuoteno').attr('style', 'display:none');
                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];


                if (NoSchemeType == '1') {
                    ctxt_replcno.SetText('Auto');
                    ctxt_replcno.SetEnabled(false);

                    tstartdate.Focus();
                }
                else if (NoSchemeType == '0') {
                    ctxt_replcno.SetEnabled(true);
                    ctxt_replcno.GetInputElement().maxLength = quotelength;
                    ctxt_replcno.SetText('');
                    ctxt_replcno.Focus();
                }
                else if (NoSchemeType == '2') {
                    ctxt_replcno.SetText('Datewise');
                    ctxt_replcno.SetEnabled(false);
                    tstartdate.Focus();
                }
                else {
                    ctxt_replcno.SetText('');
                    ctxt_replcno.SetEnabled(false);
                }
            });


            $('#ddl_Branch').change(function () {
                debugger;
                GetContactPerson();
            });



        });



        function closeWarehouse(s, e) {
            e.cancel = false;
            cGrdWarehouse.PerformCallback('WarehouseDelete');
            $('#abpl').popover('hide');
        }


        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/Management/Master/frm_BranchUdfPopUp.aspx?Type=REP&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
        }


        function Save_ButtonClick() {

            if (ctxt_replcno.GetText() == '') {

                jAlert('Replace Number is mandatory');
            }

            else if (tstartdate.GetDate() == null) {

                jAlert('Date Field is mandatory');
            }

            else if (gridLookup.GetValue() == null) {

                jAlert('Customer Field is mandatory');
            }

            else {
                $("#hdnsaveorsaveexists").val(0);
                gridReplacement.batchEditApi.EndEdit();
                gridReplacement.UpdateEdit();
            }
        }


        function SaveExit_ButtonClick() {

            if (ctxt_replcno.GetText() == '') {

                jAlert('Replace Number is mandatory');
            }

            else if (tstartdate.GetDate() == null) {

                jAlert('Date Field is mandatory');
            }

            else if (gridLookup.GetValue() == null) {

                jAlert('Customer Field is mandatory');
            }


            else {
                $("#hdnsaveorsaveexists").val(1);
                gridReplacement.batchEditApi.EndEdit();
                gridReplacement.UpdateEdit();
            }

        }


        function Warehouseclickevent(valueget) {

            //  debugger;
            //alert(valueget);
            // var index = e.visibleIndex;
            // grid.batchEditApi.StartEdit(index, 2)
            //    Warehouseindex = index;

            gridReplacement.batchEditApi.StartEdit(0, 2);

            var ProductID = (gridReplacement.GetEditor('ProductID').GetValue() != null) ? gridReplacement.GetEditor('ProductID').GetValue() : "";
            var QuantityValue = (gridReplacement.GetEditor('QuantityInput').GetValue() != null) ? gridReplacement.GetEditor('QuantityInput').GetValue() : "0";
            var UOM = (gridReplacement.GetEditor('UOM').GetValue() != null) ? gridReplacement.GetEditor('UOM').GetValue() : "0";
            var SalesAmount = (gridReplacement.GetEditor('Rate').GetValue() != null) ? gridReplacement.GetEditor('Rate').GetValue() : "0";
            var Productserials = (gridReplacement.GetEditor('ProductSerials').GetValue() != null) ? gridReplacement.GetEditor('ProductSerials').GetValue() : "0";
            var ProductDescription = (gridReplacement.GetEditor('ProductDescription').GetValue() != null) ? gridReplacement.GetEditor('ProductDescription').GetValue() : "0";
            var ComponentNumber = (gridReplacement.GetEditor('ComponentNumber').GetValue() != null) ? gridReplacement.GetEditor('ComponentNumber').GetValue() : "0";

            //for storing recent warehouse
            var componentdetailsid = (gridReplacement.GetEditor('ComponentDetailsID').GetValue() != null) ? gridReplacement.GetEditor('ComponentDetailsID').GetValue() : "0";

            //  alert(ProductID + ' ' + QuantityValue + ' ' + UOM + ' ' + SalesAmount + ' ' + Productserials + ' ' + ProductDescription + ' ' + ComponentNumber);


            var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";

            if (QuantityValue == "0.0") {
                jAlert("Quantity should not be zero !.");
            } else {
                if (ProductID != "") {

                    var strProductID = ProductID;
                    //var strDescription = SpliteDetails[1];
                    //var strUOM = SpliteDetails[2];
                    //var strStkUOM = SpliteDetails[4];
                    //var strMultiplier = SpliteDetails[7];
                    //var strProductName = (gridReplacement.GetEditor('ProductID').GetText() != null) ? gridReplacement.GetEditor('ProductID').GetText() : "";
                    //var StkQuantityValue = QuantityValue * strMultiplier;
                    //var stockids = SpliteDetails[10];

                    // var Ptype = SpliteDetails[14];
                    var Ptype = (gridReplacement.GetEditor('ProdType').GetValue() != null) ? gridReplacement.GetEditor('ProdType').GetValue() : "0";

                    //var StkQuantityValue = QuantityValue * strMultiplier;

                    //if (stockids == "0") {
                    //    jAlert("Please Update the Opening Stock!.");
                    //} else {
                    $('#<%=hdfProductType.ClientID %>').val(Ptype);

                    $('#<%=hdfProductID.ClientID %>').val(strProductID);
                    $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                    $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);

                    document.getElementById('<%=lblProductName.ClientID %>').innerHTML = ProductDescription;
                    document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
                    document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = UOM;
                    <%--        document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
                            document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;--%>
                    cacpAvailableStock.PerformCallback(strProductID);

                    SelectWarehouse = "0";
                    $("#spnCmbWarehouse").hide();
                    $("#spntxtBatch").hide();
                    $("#spntxtQuantity").hide();
                    $("#spntxtserialID").hide();

                    if (Ptype == "W") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Manufacture.style.display = 'none';
                        div_Expiry.style.display = 'none';
                        div_Quantity.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + Replacementid);
                        cCmbWarehouseID.PerformCallback('BindWarehouse~' + ProductID);
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "B") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'block';
                        div_Manufacture.style.display = 'block';
                        div_Expiry.style.display = 'block';
                        div_Quantity.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + Replacementid);
                        cCmbWarehouseID.PerformCallback('BindWarehouse~' + ProductID);
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "S") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'none';
                        div_Manufacture.style.display = 'none';
                        div_Expiry.style.display = 'none';
                        div_Quantity.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cCmbWarehouseID.PerformCallback('BindWarehouse~' + ProductID);
                        cGrdWarehouse.PerformCallback('Display~' + ProductID);
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WB") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Manufacture.style.display = 'block';
                        div_Expiry.style.display = 'block';
                        div_Quantity.style.display = 'block';
                        div_Serial.style.display = 'none';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + Replacementid);
                        cCmbWarehouseID.PerformCallback('BindWarehouse~' + ProductID);
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WS") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'none';
                        div_Manufacture.style.display = 'none';
                        div_Expiry.style.display = 'none';
                        div_Quantity.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + Replacementid);
                        cCmbWarehouseID.PerformCallback('BindWarehouse~' + ProductID);
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "WBS") {
                        div_Warehouse.style.display = 'block';
                        div_Batch.style.display = 'block';
                        div_Manufacture.style.display = 'block';
                        div_Expiry.style.display = 'block';
                        div_Quantity.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Break.style.display = 'block';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + Replacementid);
                        cCmbWarehouseID.PerformCallback('BindWarehouse~' + ProductID);
                        cPopup_Warehouse.Show();
                    }
                    else if (Ptype == "BS") {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'block';
                        div_Manufacture.style.display = 'block';
                        div_Expiry.style.display = 'block';
                        div_Quantity.style.display = 'none';
                        div_Serial.style.display = 'block';
                        div_Break.style.display = 'none';

                        SelectedWarehouseID = "0";
                        cGrdWarehouse.PerformCallback('Display~' + Replacementid);
                        cPopup_Warehouse.Show();
                    }
                    else {
                        div_Warehouse.style.display = 'none';
                        div_Batch.style.display = 'none';
                        div_Manufacture.style.display = 'none';
                        div_Expiry.style.display = 'none';
                        div_Quantity.style.display = 'none';
                        div_Serial.style.display = 'none';
                    }

                }
            }

        }


        function OnCustomButtonClick(s, e) {
            if (e.buttonID == 'CustomDelete') {
                gridReplacement.batchEditApi.EndEdit();
                gridReplacement.DeleteRow(e.visibleIndex);

                //   gridReplacement.PerformCallback('Delete~' + SrlNo);

                // gridReplacement.PerformCallback('Delete' + '~' + '@');

            }


            if (e.buttonID == 'CustomWarehouse') {
            }
        }




        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
        }

        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }

        function MultipleSerial() {


        }

        function componentEndCallBack(s, e) {


        }
        function OnEndCallback(s, e) {


            if (gridReplacement.cpSaveSuccessOrFail == "Success") {

                var Row_Number = gridReplacement.cpReplacementNo;
                var Quote_Msg = "Replacement No. '" + Row_Number + "' saved.";


                if ($("#hdnsaveorsaveexists").val() == "0") {

                    var strconfirm = confirm(Quote_Msg);
                    if (strconfirm == true) {

                        window.location.assign("ReplacementNote.aspx?key=ADD");
                    }
                }
                else if ($("#hdnsaveorsaveexists").val() == "1") {
                    var strconfirm = confirm(Quote_Msg);
                    if (strconfirm == true) {
                        window.location.assign("ReplacementNoteList.aspx?key=ADD");
                    }
                }

            }
        }

        function CmbWarehouseEndCallback(s, e) {

        }
        function CmbBatchEndCall(s, e) {

        }
        function OnListBoxSelectionChanged(listBox, args) {

        }
        function SynchronizeListBoxValues() {


        }
        function txtserialTextChanged() {


        }
        function OnWarehouseEndCallback() {


        }
        function CallbackPanelEndCall() {


        }


        function acpAvailableStockEndCall() {


        }

        function ProductlookUpKeyDown() {


        }

        function listBoxEndCall(s, e) {
        }

        var globalTaxRowIndex;
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }


        function ProductSelected(s, e) {
            if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return;
            }

        }
        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;


        }

        function ChangeState(value) {

            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }


        function GetContactPerson(e) {

            //debugger;
            if (gridLookup != null) {



                var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                //alert(key);
                if (key != null && key != '') {




                    var startDate = new Date();
                    startDate = tstartdate.GetValueString();

                    var BranchId = $('#ddl_Branch').val();


                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + BranchId);


                    GetObjectID('hdnCustomerId').value = key;
                    GetObjectID('hdnAddressDtl').value = '0';


                }
            }
        }


        function InvoiceNumberChanged() {

            var quote_Id = gridquotationLookup.GetValue();

            //  alert(quote_Id);
            if (quote_Id != null) {
                var arr = quote_Id.split(',');

            }
            else { ctxt_InvoiceDate.SetText(''); }

            if (quote_Id != null) {
                cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
                cProductsPopup.Show();
            }
        }

        function PerformCallToGridBind() {

            gridReplacement.PerformCallback('BindGridOnQuotation' + '~' + '@');
            //    cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
            $('#hdnPageStatus').val('Quoteupdate');

            var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

            cProductsPopup.Hide();
            return false;
        }


        function UniqueCodeCheck() {

            var QuoteNo = ctxt_replcno.GetText();
            if (QuoteNo != '') {
                var CheckUniqueCode = false;
                $.ajax({
                    type: "POST",
                    url: "ReplacementNote.aspx/CheckUniqueCode",
                    data: JSON.stringify({ QuoteNo: QuoteNo }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {

                            $('#duplicateQuoteno').attr('style', 'display:block');
                            ctxt_replcno.SetValue('');
                            ctxt_replcno.Focus();
                        }
                        else {
                            $('#duplicateQuoteno').attr('style', 'display:none');
                        }
                    }
                });
            }
        }

        function QuantityCap() {
            var InputQty = gridReplacement.GetEditor('QuantityInput').GetValue();
            var MainQty = gridReplacement.GetEditor('Quantity').GetValue();

            if (parseInt(InputQty) > parseInt(MainQty) || parseInt(InputQty) == 0) {
                gridReplacement.GetEditor("QuantityInput").SetText('');
                jAlert('Replacement Quantity must be greter than 0 and equal or less than original Invoice Quantity');

            }
        }


        function SubmitWarehouse() {


            //      debugger;

            var ProductID = (gridReplacement.GetEditor('ProductID').GetValue() != null) ? gridReplacement.GetEditor('ProductID').GetValue() : "";

            var ComponentNumber = (gridReplacement.GetEditor('ComponentNumber').GetValue() != null) ? gridReplacement.GetEditor('ComponentNumber').GetValue() : "0";

            var Quantityinput = (gridReplacement.GetEditor('QuantityInput').GetValue() != null) ? gridReplacement.GetEditor('QuantityInput').GetValue() : "0";

            var ProductType = (gridReplacement.GetEditor('ProdType').GetValue() != null) ? gridReplacement.GetEditor('ProdType').GetValue() : "0";

            var componentdetailsid = (gridReplacement.GetEditor('ComponentDetailsID').GetValue() != null) ? gridReplacement.GetEditor('ComponentDetailsID').GetValue() : "0";

            var WarehouseID = (cCmbWarehouseID.GetValue() != null) ? cCmbWarehouseID.GetValue() : "0";
            var WarehouseName = (cCmbWarehouseID.GetText() != null) ? cCmbWarehouseID.GetText() : "";
            var BatchName = (ctxtBatchName.GetValue() != null) ? ctxtBatchName.GetValue() : "";


            var MfgDate = (ctxtStartDate.GetValue() != null) ? ctxtStartDate.GetDate() : "";
            var ExpiryDate = (ctxtEndDate.GetValue() != null) ? ctxtEndDate.GetDate() : "";


            var SerialNo = (ctxtserialID.GetValue() != null) ? ctxtserialID.GetValue() : "";
            var Qty = ctxtQuantity.GetValue();

            if (ProductType == 'WBS' || ProductType == 'S' || ProductType == 'WS' || ProductType == 'BS')
                //  MfgDate = GetDateFormat(MfgDate);
                ///  ExpiryDate = GetDateFormat(ExpiryDate);

                var SelectedWarehouseID = 0;

            $("#spnCmbWarehouse").hide();
            $("#spntxtBatch").hide();
            $("#spntxtQuantity").hide();
            $("#spntxtserialID").hide();


            if (parseInt(Quantityinput) >= parseInt(Qty)) {
                var Ptype = document.getElementById('hdfProductType').value;
                if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
                    $("#spnCmbWarehouse").show();
                }

                else if ((Ptype == "B" && BatchName == "") || (Ptype == "WB" && BatchName == "") || (Ptype == "WBS" && BatchName == "") || (Ptype == "BS" && BatchName == "")) {
                    $("#spntxtBatch").show();
                }

                else if ((Ptype == "W" && Qty == "0.0") || (Ptype == "B" && Qty == "0.0") || (Ptype == "WB" && Qty == "0.0")) {
                    $("#spntxtQuantity").show();
                }

                else if ((Ptype == "S" && SerialNo == "") || (Ptype == "WS" && SerialNo == "") || (Ptype == "WBS" && SerialNo == "") || (Ptype == "BS" && SerialNo == "")) {
                    $("#spntxtserialID").show();
                }

                else {

                    if ((Ptype == "S" && SelectedWarehouseID == "0") || (Ptype == "WS" && SelectedWarehouseID == "0") || (Ptype == "WBS" && SelectedWarehouseID == "0") || (Ptype == "BS" && SelectedWarehouseID == "0")) {
                        ctxtserialID.SetValue("");
                        ctxtserialID.Focus();
                    }
                    else {
                        cCmbWarehouseID.PerformCallback('BindWarehouse~' + ProductID);
                        ctxtQuantity.SetValue("0");
                        ctxtBatchName.SetValue("");
                        ctxtStartDate.SetDate(null);
                        ctxtEndDate.SetDate(null);
                        ctxtserialID.SetValue("");
                    }

                    cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + MfgDate + '~' + ExpiryDate + '~' + SerialNo + '~' + Qty + '~' + SelectedWarehouseID + '~' + ProductID + '~' + Ptype + '~' + componentdetailsid + '~' + ComponentNumber);
                    SelectedWarehouseID = "0";
                    SelectWarehouse = "0";
                }
            }
            else {

                jAlert('Quantity should be match with entered quantity');
            }
        }

        function fn_Edit(keyValue) {

            SelectedWarehouseID = keyValue;
            ctxtQuantity.SetValue("0");
            ctxtBatchName.SetValue("");
            ctxtStartDate.SetDate(null);
            ctxtEndDate.SetDate(null);
            ctxtserialID.SetValue("");

            // cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
        }

        function fn_Delete(keyValue) {

            var ProductID = (gridReplacement.GetEditor('ProductID').GetValue() != null) ? gridReplacement.GetEditor('ProductID').GetValue() : "";
            var ComponentNumber = (gridReplacement.GetEditor('ComponentNumber').GetValue() != null) ? gridReplacement.GetEditor('ComponentNumber').GetValue() : "0";
            var Quantityinput = (gridReplacement.GetEditor('QuantityInput').GetValue() != null) ? gridReplacement.GetEditor('QuantityInput').GetValue() : "0";
            cGrdWarehouse.PerformCallback('Delete~' + ProductID + '~' + ComponentNumber + '~' + Quantityinput);

            //     var index = cGrdWarehouse.GetFocusedRowIndex();
            //    gvEditing.DeleteRow(index);
        }



    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-title">
        <h3>Replacement Note</h3>
    </div>
    <div id="ApprovalCross" runat="server" class="crossBtn"><a href="ReplacementNoteList.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">


        <asp:Panel ID="pnl_Replacement" runat="server">
            <div class="">
                <%--    General  Controls--%>
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="">
                                        <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix col-md-12">
                                            <div class="col-md-3" id="divScheme" runat="server">
                                                <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" TabIndex="1">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <dxe:ASPxLabel ID="lbl_Replacementnumber" runat="server" Text="Replacement Number">
                                                </dxe:ASPxLabel>
                                                &nbsp;<span style="color: red">*</span>
                                                <dxe:ASPxTextBox ID="txt_replcno" runat="server" ClientInstanceName="ctxt_replcno" TabIndex="2" Width="100%">
                                                    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                                </dxe:ASPxTextBox>

                                                <span id="MandatorysQuoteno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                </span>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span id="duplicateQuoteno" style="display: none" class="validclass"><img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Duplicate number">
                                                </span>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </div>
                                            <div class="col-md-3">
                                                <dxe:ASPxLabel ID="lbl_SaleInvoiceusrinputDt" runat="server" Text="Date">
                                                </dxe:ASPxLabel>
                                                &nbsp;<span style="color: red">*</span>
                                                <dxe:ASPxDateEdit ID="dt_date" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate" TabIndex="3" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <ClientSideEvents DateChanged="GetContactPerson" />
                                                </dxe:ASPxDateEdit>
                                            </div>
                                            <div class="col-md-3">
                                                <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="4">
                                                </asp:DropDownList>
                                            </div>
                                            <div style="clear: both">
                                            </div>

                                            <%--  Customer DataBind--%>

                                            <div class="col-md-3">
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                                </dxe:ASPxLabel>
                                                &nbsp;<span style="color: red">*</span>
                                                <%--   <i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i>--%>
                                                <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" TabIndex="5" ClientInstanceName="gridLookup"
                                                    KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" DataSourceID="dsCustomer">
                                                    <Columns>
                                                        <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" Width="200px" Settings-AutoFilterCondition="Contains">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Settings-AutoFilterCondition="Contains" Width="200px">

                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="2" Caption="Type" Settings-AutoFilterCondition="Contains" Width="200px">

                                                            <Settings AutoFilterCondition="Contains" />
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
                                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior="False" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </StatusBar>
                                                        </Templates>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                                        <SettingsLoadingPanel Text="Please Wait..." />
                                                        <Settings ShowFilterRow="True" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                    </GridViewProperties>
                                                    <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" />
                                                    <ClearButton DisplayMode="Auto">
                                                    </ClearButton>
                                                </dxe:ASPxGridLookup>
                                                <span id="MandatorysCustomer" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>


                                            <%--  Customer DataBind--%>


                                            <%--     Invoice  Number Popup--%>

                                            <div class="col-md-3">
                                                <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Invoice Number">
                                                </dxe:ASPxLabel>

                                                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
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
                                                                <ClientSideEvents ValueChanged="function(s, e) { InvoiceNumberChanged();}" />
                                                            </dxe:ASPxGridLookup>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="componentEndCallBack" />
                                                </dxe:ASPxCallbackPanel>

                                            </div>
                                            <%--     Invoice  Number Popup--%>


                                            <div class="col-md-3">
                                                <dxe:ASPxLabel ID="lbl_InvoiceNO" ClientInstanceName="clbl_InvoiceNO" runat="server" Text="Date">
                                                </dxe:ASPxLabel>
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



                                            </div>

                                            <div style="clear: both">
                                            </div>


                                        </div>
                                        <div style="clear: both;"></div>
                                        <div class="">
                                            <div style="display: none;">
                                            </div>
                                            <div>
                                                <br />
                                            </div>

                                            <%-- Gridview Product Details--%>

                                            <dxe:ASPxGridView runat="server" KeyFieldName="slno"
                                                ClientInstanceName="gridReplacement" ID="gridReplacement" DisplayFormatInEditMode="true"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                                SettingsPager-Mode="ShowAllRecords"
                                                Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200"
                                                OnCustomCallback="gridReplacement_CustomCallback"
                                                OnDataBinding="gridReplacement_DataBinding"
                                                OnBatchUpdate="gridReplacement_BatchUpdate"
                                                OnRowInserting="gridReplacement_RowInserting"
                                                OnRowUpdating="gridReplacement_RowUpdating"
                                                OnRowDeleting="gridReplacement_RowDeleting"
                                                OnCustomColumnDisplayText="gridReplacement_CustomColumnDisplayText"
                                                ViewStateMode="Disabled">
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Columns>


                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30" VisibleIndex="0" Caption=" ">
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




                                                    <dxe:GridViewDataTextColumn Caption="Sl" ReadOnly="True" UnboundType="String"
                                                        VisibleIndex="1" Width="4%">
                                                        <Settings AllowAutoFilter="False" AllowAutoFilterTextInputTimer="False"
                                                            AllowDragDrop="False" AllowGroup="False" AllowHeaderFilter="False"
                                                            AllowSort="False" />
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="ComponentNumber" Caption="Invoice" VisibleIndex="2" ReadOnly="True" Width="15%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="ProductsName" Caption="Product(Receivable)" VisibleIndex="3" ReadOnly="True" Width="15%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="ProductDescription" Caption="Description" VisibleIndex="4" ReadOnly="True" Width="30%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="QuantityInput" Caption="Quantity" VisibleIndex="5" Width="10%">
                                                        <PropertiesTextEdit>
                                                            <ClientSideEvents TextChanged="QuantityCap" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="Stock UOM" VisibleIndex="6" ReadOnly="True" Width="10%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Invoice Qty" VisibleIndex="7" ReadOnly="True" Width="10%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="8" ReadOnly="True" Width="7%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="ProductSerials" Caption="Product Serials" VisibleIndex="9" ReadOnly="True" Width="20%">
                                                        <CellStyle Wrap="True"></CellStyle>

                                                    </dxe:GridViewDataTextColumn>





                                                    <dxe:GridViewDataTextColumn FieldName="ProductsName2" Caption="Product(Issuable)" VisibleIndex="10" ReadOnly="True" Width="20%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="ProductSerials" Visible="false" Caption="Product Serials" VisibleIndex="11" ReadOnly="True" Width="7%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                        <PropertiesTextEdit>

                                                            <ClientSideEvents TextChanged="MultipleSerial()" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>



                                                    <%--                <dxe:GridViewCommandColumn VisibleIndex="12" Caption="Stk Details" Width="6%" >
                                                        <CustomButtons>

                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                                <Image ToolTip="Warehouse" Url="/assests/images/warehouse.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>--%>


                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" VisibleIndex="13" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide"
                                                        PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ComponentDetailsID" Caption="hidden Field Id" VisibleIndex="19" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide"
                                                        PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ProdType" Caption="hidden Field Id" VisibleIndex="20" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide"
                                                        PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn Width="6%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="12">
                                                        <DataItemTemplate>

                                                            <% if (rights.CanEdit)
                                                               { %>


                                                            <a href="javascript:void(0);" title="Stk Details" onclick="Warehouseclickevent(0)" class="pad">
                                                                <img src="../../../assests/images/warehouse.png" /></a>
                                                            <%} %>
                                                        </DataItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <HeaderTemplate><span>Stk Details</span></HeaderTemplate>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>

                                                </Columns>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />

                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                                <SettingsBehavior ColumnResizeMode="Disabled" />
                                            </dxe:ASPxGridView>


                                            <%-- Gridview Product Details--%>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <br />
                                        <div class="" id="divSubmitButton">


                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>

                                            <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                            </dxe:ASPxButton>


                                        </div>
                                    </div>
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

                <%--    General  Controls--%>
            </div>



            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="AddArea_PopUp.aspx"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>







            <div class="PopUpArea">
                <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
                <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />


                <asp:HiddenField ID="hdnsaveorsaveexists" runat="server" />
                <asp:HiddenField runat="server" ID="IsUdfpresent" />



                    <%--  Pop Up Warehouse--%>

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
                                            <div>
                                                Warehouse
                                            </div>
                                            <div class="Left_Content" style="">
                                                <%-- --%>
                                                <dxe:ASPxComboBox ID="CmbWarehouseID" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouseID" SelectedIndex="0"
                                                    TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouseID_Callback">
                                                    <ClientSideEvents EndCallback="CmbWarehouseEndCallback"></ClientSideEvents>
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
                                                <dxe:ASPxTextBox ID="ASPxTextBox2" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                                    <ClientSideEvents />
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
                                                    <ClientSideEvents TextChanged="SubmitWarehouse" />
                                                </dxe:ASPxTextBox>
                                                <span id="spntxtserialID" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-3">
                                            <div>
                                            </div>
                                            <div class="Left_Content" style="padding-top: 14px">
                                                <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                                    <ClientSideEvents Click="SubmitWarehouse" />
                                                </dxe:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                        <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="Slno" AutoGenerateColumns="False"
                                            Width="100%" ClientInstanceName="cGrdWarehouse" OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding"
                                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
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
                                                <dxe:GridViewDataTextColumn VisibleIndex="6" Width="80px">
                                                    <DataItemTemplate>
                                                    
                                                        <a href="javascript:void(0);" onclick="fn_Delete('<%# Container.KeyValue %>')" title="Delete">
                                                            <img src="/assests/images/crs.png" /></a>
                                                    </DataItemTemplate>
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                            <%--<ClientSideEvents EndCallback="OnWarehouseEndCallback" />--%>
                                            <SettingsPager Visible="false"></SettingsPager>
                                            <SettingsLoadingPanel Text="Please Wait..." />
                                        </dxe:ASPxGridView>
                                    </div>
                                </div>
                                <%--     <div class="clearfix">
                                    <br />
                                    <div style="align-content: center">
                                        <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </div>--%>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>


                    <%--  Pop Up Warehouse--%>

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
                <asp:HiddenField ID="hdnDeleteSrlNo" runat="server" />

                <asp:HiddenField ID="hdntab2" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnCustomerId" runat="server" />
                <asp:HiddenField ID="hdnAddressDtl" runat="server" />
            </div>


            <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel">
                <%--OnCallback="CallbackPanel_Callback"--%>
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="CallbackPanelEndCall" />
            </dxe:ASPxCallbackPanel>

            <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock">
                <PanelCollection>
                    <%--   OnCallback="acpAvailableStock_Callback"--%>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
            </dxe:ASPxCallbackPanel>


            <%--  Sql Data Source--%>

            <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="prc_CRMSalesInvoice_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="AllProductDetails" />
                </SelectParameters>
            </asp:SqlDataSource>



            <asp:SqlDataSource runat="server" ID="dsCustomer" 
                SelectCommand="prc_CRMSalesInvoice_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateCustomerDetail" />
                </SelectParameters>
            </asp:SqlDataSource>



            <asp:SqlDataSource ID="CountrySelect" runat="server" 
                SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country">

            </asp:SqlDataSource>

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


            <%--  Sql Data Source--%>
        </asp:Panel>


    </div>


    <div></div>



    <%-- Invoice Number wise products check boxes Popup--%>
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

                    <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                            <%--     <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <%--  <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                            </dxe:GridViewDataTextColumn>--%>
                        </Columns>
                        <SettingsDataSecurity AllowEdit="true" />
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>


    </div>


    <%-- Invoice Number wise products check boxes Popup--%>
</asp:Content>

