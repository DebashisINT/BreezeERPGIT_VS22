<%@ Page Title="Replacement Stock Out" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ReplacementOutNoteList.aspx.cs" Inherits="ERP.OMS.Management.Activities.ReplacementOutNoteList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
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
    </style>

    <script type="text/javascript">
        var SelectWarehouse = "0";
        var SelectBatch = "0";
        var SelectSerial = "0";
        var SelectedWarehouseID = "0";
        var textSeparator = ";";
        var selectedChkValue = "";
        var IsPostBack = "";
        var PBWarehouseID = "";
        var PBBatchID = "";

        $(document).ready(function () {
            $('#ddl_numberingScheme').change(function () {
                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];

                var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
                if (branchID != "") document.getElementById('ddl_Branch').value = branchID;

                if (NoSchemeType == '1') {
                    ctxt_PLQuoteNo.SetText('Auto');
                }
                else if (NoSchemeType == '0') {
                    ctxt_PLQuoteNo.SetEnabled(true);
                    ctxt_PLQuoteNo.GetInputElement().maxLength = quotelength;
                    ctxt_PLQuoteNo.SetText('');
                    ctxt_PLQuoteNo.Focus();
                }
                else if (NoSchemeType == '2') {
                    ctxt_PLQuoteNo.SetText('Datewise');
                    ctxt_PLQuoteNo.SetEnabled(false);
                    tstartdate.Focus();
                }
                else {
                    ctxt_PLQuoteNo.SetText('');
                    ctxt_PLQuoteNo.SetEnabled(false);
                }
            });
        });
        function CmbBatchEndCall(s, e) {
            if (SelectBatch != "0") {
                cCmbBatch.SetValue(SelectBatch);
                SelectBatch = "0";
            }
            else {
                cCmbBatch.SetEnabled(true);
            }
        }
        function txtserialTextChanged() {
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
            }
        }
        function listBoxEndCall(s, e) {
            if (SelectSerial != "0") {
                var values = [SelectSerial];
                checkListBox.SelectValues(values);
                UpdateSelectAllItemState();
                UpdateText();
                SelectSerial = "0";
                cCmbBatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
            }
        }
        function OnListBoxSelectionChanged(listBox, args) {
            if (args.index == 0)
                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
            UpdateSelectAllItemState();
            UpdateText();

            //Added Subhabrata
            var selectedItems = checkListBox.GetSelectedItems();
            var val = GetSelectedItemsText(selectedItems);
            var strWarehouse = cCmbWarehouse.GetValue();
            var strBatchID = cCmbBatch.GetValue();
            var ProducttId = $("#hdfProductID").val();
        }
        function UpdateSelectAllItemState() {
            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
        }
        function IsAllSelected() {
            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
        }
        function UpdateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            selectedChkValue = GetSelectedItemsText(selectedItems);
            //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
            checkComboBox.SetText(selectedItems.length + " Items");

            var val = GetSelectedItemsText(selectedItems);
            $("#abpl").attr('data-content', val);
        }
        function SynchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            // var texts = dropDown.GetText().split(textSeparator);
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
        function AutoCalculateMandateOnChange(element) {
            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();

            if (document.getElementById("myCheck").checked == true) {
                divSingleCombo.style.display = "block";
                divMultipleCombo.style.display = "none";

                checkComboBox.Focus();
            }
            else {
                divSingleCombo.style.display = "none";
                divMultipleCombo.style.display = "block";

                ctxtserial.Focus();
            }
        }
        function OnMoreInfoClick(ChallanID, keyValue, ReplacementStkID, InvoiceID) {
            $('#<%=hdfReplacementID.ClientID %>').val(keyValue);
            $('#<%=hdfChallanID.ClientID %>').val(ChallanID);
            $('#<%=hdfInvoiceID.ClientID %>').val(InvoiceID);
            $('#<%=hdfReplacementStkID.ClientID %>').val(ReplacementStkID);

            cPopup_Warehouse.Show();
            cGrdProduct.PerformCallback(keyValue);
            cGrdWarehouse.PerformCallback('DisplayWarehouse');
            div_warehouse.style.display = 'none';
            divAvailableStk.style.display = "block";

            var ReplacementStkID = $("#hdfReplacementStkID").val();
            if (ReplacementStkID == "") {
                cbtnWarehouseSave.SetVisible(true);
                ctxt_PLQuoteNo.SetText("");
                ctxt_PLQuoteNo.SetEnabled(true);
                divScheme.style.display = "block";
            }
            else {
                cbtnWarehouseSave.SetVisible(false);
                divScheme.style.display = "none";
                cQuotationComponentPanel.PerformCallback();
            }
        }
        function CmbWarehouse_ValueChange() {
            var WarehouseID = cCmbWarehouse.GetValue();
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS" || type == "WB") {
                cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
            }
            else if (type == "WS") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
            }
        }
        function CmbWarehouseEndCallback(s, e) {
            if (SelectWarehouse != "0") {
                cCmbWarehouse.SetValue(SelectWarehouse);
                SelectWarehouse = "0";
            }
            else {
                cCmbWarehouse.SetEnabled(true);
            }
        }
        function CmbBatch_ValueChange() {
            var WarehouseID = cCmbWarehouse.GetValue();
            var BatchID = cCmbBatch.GetValue();
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
            }
            else if (type == "BS") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID);
            }
        }
        function OnWarehouseEndCallback(s, e) {
            var Ptype = document.getElementById('hdfProductType').value;

            if (cGrdWarehouse.cpIsSave == "Y") {
                cGrdWarehouse.cpIsSave = null;
                cPopup_Warehouse.Hide();
                grid.batchEditApi.StartEdit(Warehouseindex, 5);
            }
            else if (cGrdWarehouse.cpIsSave == "N") {
                cGrdWarehouse.cpIsSave = null;
                jAlert('Sales Quantity must be equal to Warehouse Quantity.');
            }
            else if (cGrdWarehouse.cpIsSave == "SuccessInsert") {
                cGrdWarehouse.cpIsSave = null;
                cGridreplacement.PerformCallback();
                cPopup_Warehouse.Hide();
                jAlert('Replacement Stock Out Successfully.');
            }
            else if (cGrdWarehouse.cpIsSave == "ErrorInsert") {
                cGrdWarehouse.cpIsSave = null;
                cGridreplacement.PerformCallback();
                cPopup_Warehouse.Hide();
                jAlert('Please try again later.');
            }
            else {
                if (document.getElementById("myCheck").checked == true) {
                    if (IsPostBack == "N") {
                        checkListBox.PerformCallback('BindSerial~' + PBWarehouseID + '~' + PBBatchID);

                        IsPostBack = "";
                        PBWarehouseID = "";
                        PBBatchID = "";
                    }

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
        }
        function fn_Deletecity(keyValue) {
            var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
            var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

            cGrdWarehouse.PerformCallback('Delete~' + keyValue);
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
        }
        function fn_Edit(keyValue) {
            SelectedWarehouseID = keyValue;
            cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
        }
        function SaveWarehouse() {
            document.getElementById("ddl_Branch").disabled = true;

            var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
            var WarehouseName = cCmbWarehouse.GetText();
            var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";
            var BatchName = cCmbBatch.GetText();
            var SerialID = "";
            var SerialName = "";
            var Qty = ctxtQuantity.GetValue();

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
            if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
                $("#spnCmbWarehouse").show();
            }
            else if ((Ptype == "B" && BatchID == "0") || (Ptype == "WB" && BatchID == "0") || (Ptype == "WBS" && BatchID == "0") || (Ptype == "BS" && BatchID == "0")) {
                $("#spnCmbBatch").show();
            }
            else if ((Ptype == "W" && Qty == "0.0") || (Ptype == "B" && Qty == "0.0") || (Ptype == "WB" && Qty == "0.0")) {
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
                        checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
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
                    checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
                    ctxtQuantity.SetValue("0");
                }
                UpdateText();
                cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
                SelectedWarehouseID = "0";
            }
        }
        function CallbackPanelEndCall(s, e) {
            if (cCallbackPanel.cpEdit != null) {
                var strWarehouse = cCallbackPanel.cpEdit.split('~')[0];
                var strBatchID = cCallbackPanel.cpEdit.split('~')[1];
                var strSrlID = cCallbackPanel.cpEdit.split('~')[2];
                var strQuantity = cCallbackPanel.cpEdit.split('~')[3];

                SelectWarehouse = strWarehouse;
                SelectBatch = strBatchID;
                SelectSerial = strSrlID;

                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
                checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

                cCmbWarehouse.SetValue(strWarehouse);
                ctxtQuantity.SetValue(strQuantity);
            }
        }
        function OnGridFocusedRowChanged(ProductID) {
            var strProduct = ProductID;
            var Product = strProduct.split("~");
            var ProductId = Product[0];
            var Ptype = Product[1];
            var SrlNo = ProductId;

            cacpAvailableStock.PerformCallback(ProductId);
            $('#<%=hdfProductID.ClientID %>').val(ProductId);
            $('#<%=hdfProductSerialID.ClientID %>').val(ProductId);
            $('#<%=hdnProductQuantity.ClientID %>').val("0");
            $('#<%=hdfProductType.ClientID %>').val(Ptype);

            var ReplacementStkID = $("#hdfReplacementStkID").val();
            if (ReplacementStkID == "") {
                div_warehouse.style.display = 'block';
            }
            else {
                div_warehouse.style.display = 'none';
            }

            if (Ptype == "W") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "B") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbBatch.PerformCallback('BindBatch~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "S") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);

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

                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "WS") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "WBS") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "BS") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                cCmbBatch.PerformCallback('BindBatch~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else {

                jAlert("No Warehouse or Batch or Serial is actived !");
            }
        }
        function closeWarehouse(s, e) {
            e.cancel = false;
            cGrdWarehouse.PerformCallback('WarehouseDelete');
            $('#abpl').popover('hide');
        }
        function FinalWarehouse() {
            cGrdWarehouse.PerformCallback('WarehouseFinal');
            cGridreplacement.PerformCallback();
        }
    </script>

    <script>
        function componentEndCallBack(s, e) {
            if (cQuotationComponentPanel.cpDetails != null) {
                var details = cQuotationComponentPanel.cpDetails;
                cQuotationComponentPanel.cpDetails = null;

                var SpliteDetails = details.split("~");
                var Replacement_Number = SpliteDetails[0];
                var BranchID = SpliteDetails[1];

                ctxt_PLQuoteNo.SetText(Replacement_Number);
                ctxt_PLQuoteNo.SetEnabled(false);
                document.getElementById('ddl_Branch').value = BranchID;
            }
        }
        function acpAvailableStockEndCall(s, e) {
            if (cacpAvailableStock.cpstock != null) {
                cCmbWarehouse.cpstock = null;
                divAvailableStk.style.display = "block";

                document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cacpAvailableStock.cpstock;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="panel-heading">
            <div class="panel-title">
                <h3>Replacement Stock Out</h3>
            </div>
        </div>
        <div class="form_main">
            <div class="clearfix">
                <% if (rights.CanExport)
                   { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <% } %>
            </div>
        </div>
        <div class="GridViewArea">
            <dxe:ASPxGridView ID="GrdReplacement" runat="server" KeyFieldName="ReplacementID" AutoGenerateColumns="False"
                Width="100%" ClientInstanceName="cGridreplacement" OnDataBinding="GrdReplacement_DataBinding" OnCustomCallback="GrdReplacement_CustomCallback">
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="StockOutNumber"
                        VisibleIndex="0" FixedStyle="Left" Width="10%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="ReplacementDate"
                        VisibleIndex="0" FixedStyle="Left" Width="10%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Replacement Note Number" FieldName="Replacement_Number"
                        VisibleIndex="1" FixedStyle="Left" Width="15%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer"
                        VisibleIndex="2" FixedStyle="Left" Width="25%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Sales Invoice Number" FieldName="Invoice_Number"
                        VisibleIndex="3" FixedStyle="Left" Width="30%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Sales Invoice Date" FieldName="InvoiceDate"
                        VisibleIndex="4" FixedStyle="Left" Width="30%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%--  <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="UserCreate"
                        VisibleIndex="7" FixedStyle="Left" Width="10%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Last Updated By" FieldName="UserUpdate"
                        VisibleIndex="9" FixedStyle="Left" Width="10%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Last Updated On" FieldName="ModifiedDate"
                        VisibleIndex="10" FixedStyle="Left" Width="10%">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>--%>
                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="5" Width="150">
                        <DataItemTemplate>
                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("ChallanID")%>','<%# Container.KeyValue %>','<%#Eval("StockOutID")%>','<%#Eval("InvoiceID")%>')" class="pad" title="Edit">
                                <img src="/assests/images/warehouse.png" /></a>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <ClientSideEvents />
                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <SettingsSearchPanel Visible="True" />
                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsLoadingPanel Text="Please Wait..." />
            </dxe:ASPxGridView>
        </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

    <%--Warehouse Popup Start--%>
    <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
        Width="950px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ClientSideEvents Closing="function(s, e) {
	closeWarehouse(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div id="content-5" class="pull-right reverse wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                        <ul>
                            <li>
                                <div class="lblHolder" id="divAvailableStk" style="display: none;">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Available Stock</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAvailableStock" runat="server" Text="0.0"></asp:Label>
                                                    <asp:Label ID="lblAvailableStockUOM" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <%--  <li>
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Replacement Quantity</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblReplacementStock" runat="server" Text="0.0"></asp:Label>
                                                    <asp:Label ID="lblReplacementStockUOM" runat="server"></asp:Label>
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
                                                <td>Entered Quantity</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblEnteredStock" runat="server" Text="0.0"></asp:Label>
                                                    <asp:Label ID="lblEnteredStockUOM" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>--%>
                        </ul>
                    </div>
                    <div class="clear">
                        <br />
                    </div>
                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                        <div class="col-md-3" id="divScheme">
                            <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                            </dxe:ASPxLabel>
                            <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <dxe:ASPxLabel ID="lbl_SaleInvoiceNo" runat="server" Text="Invoice Number">
                            </dxe:ASPxLabel>
                            <dxe:ASPxTextBox ID="txt_PLQuoteNo" runat="server" ClientInstanceName="ctxt_PLQuoteNo" TabIndex="2" Width="100%">
                                <%-- <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />--%>
                            </dxe:ASPxTextBox>
                        </div>
                        <div class="col-md-3">
                            <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                            </dxe:ASPxLabel>
                            <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="4" onchange="ddlBranch_ChangeIndex()" Enabled="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="GrdProduct" runat="server" KeyFieldName="ProductID" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cGrdProduct"
                            OnCustomCallback="GrdProduct_CustomCallback" OnDataBinding="GrdProduct_DataBinding">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Product Code" FieldName="ProductCode"
                                    VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Product Name" FieldName="ProductName"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Product Deception" FieldName="ProductDeception"
                                    VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Return Quantity" FieldName="ReturnQuantity" Width="100px"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="4" Width="40px">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="OnGridFocusedRowChanged('<%# Container.KeyValue %>')" title="Get Warehouse">
                                            <img src="/assests/images/warehouse.png" /></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clear">
                        <br />
                    </div>
                    <div id="div_warehouse" class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc; display: none;">
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
                                    <MaskSettings Mask="<0..999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
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
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cGrdWarehouse"
                            OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto"
                            Settings-VerticalScrollableHeight="150" SettingsBehavior-AllowSort="false">
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
                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" FieldName="Action">
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
                            <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <%--Warehouse Popup End--%>

    <%--Hidden Field--%>
    <asp:HiddenField ID="hdfReplacementStkID" runat="server" />
    <asp:HiddenField ID="hdfReplacementID" runat="server" />
    <asp:HiddenField ID="hdfChallanID" runat="server" />
    <asp:HiddenField ID="hdfInvoiceID" runat="server" />
    <asp:HiddenField ID="hdfProductType" runat="server" />
    <asp:HiddenField ID="hdfProductID" runat="server" />
    <asp:HiddenField ID="hdfProductSerialID" runat="server" />
    <asp:HiddenField ID="hdnProductQuantity" runat="server" />
    <%--Hidden Field--%>

    <%--Callback Panel--%>
    <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotationPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="componentEndCallBack" />
    </dxe:ASPxCallbackPanel>
    <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
    </dxe:ASPxCallbackPanel>
    <%--Callback Panel--%>
</asp:Content>
