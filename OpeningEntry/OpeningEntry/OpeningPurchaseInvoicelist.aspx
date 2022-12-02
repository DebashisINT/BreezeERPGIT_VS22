<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="OpeningPurchaseInvoicelist.aspx.cs" Inherits="OpeningEntry.OpeningEntry.OpeningPurchaseInvoicelist" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--Filteration Section Start By Sam--%>
    <script src="OpeningJS/OpeningPurchaseInvoice.js"></script>
    <script type="text/javascript" src="../assests/js/GenericJScript.js"></script>
    <script>
        function BeginCallback() {
            $("#drdExport").val(0);
        }
        function ClearField() {
            cFormDate.SetDate(null);
            ctoDate.SetDate(null);
            ccmbBranchfilter.SetSelectedIndex(0);
        }
        function OnCancelClick(keyValue, visibleIndex) {
        }
    </script>

    <%-- Filteration Section Start By Sam--%>
    <script>
        function OnAddEditClick(e, obj) {
            var data = obj.split('~');
            if (data.length > 1)
                RowID = data[1];
            cproductpopup.Show();
            popproductPanel.PerformCallback(obj);
        }
    </script>
    <script>
        var PInvoice_id = 0;
        function onPrintJv(id) {
            //debugger;
            PInvoice_id = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            CselectDuplicate.SetEnabled(false);
            CselectTriplicate.SetEnabled(false);
            CselectOriginal.SetCheckState('UnChecked');
            CselectDuplicate.SetCheckState('UnChecked');
            CselectTriplicate.SetCheckState('UnChecked');
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }
        function OnEWayBillClick(id, EWayBillNumber, EWayBillDate, EWayBillValue) {
            //cgrid.SetFocusedRowIndex(VisibleIndex);
            //var EWayBillNumber = cgrid.GetRow(cgrid.GetFocusedRowIndex()).children[16].innerText;
            if (EWayBillNumber.trim() != "") {
                ctxtEWayBillNumber.SetText(EWayBillNumber);
            }
            else {
                ctxtEWayBillNumber.SetText("");
            }

            if (EWayBillDate.trim() != "" && EWayBillDate.trim() != "01-01-1970" && EWayBillDate.trim() != "01-01-1900") {
                cdt_EWayBill.SetText(EWayBillDate);
            }
            else {
                cdt_EWayBill.SetText("");
            }
            if (EWayBillValue.trim() != "0.00" && EWayBillValue.trim() != "") {
                ctxtEWayBillValue.SetText(EWayBillValue);
            }
            else {
                ctxtEWayBillValue.SetText("0.0");
            }


            $('#hddnInvoiceID').val(id);
            cPopup_EWayBill.Show();
            ctxtEWayBillNumber.Focus();
        }
        function GetEWayBillDateFormat(today) {
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
        function CallEWayBill_save() {

            var InvoiceID = $("#<%=hddnInvoiceID.ClientID%>").val();
            //if (ctxtEWayBillNumber.GetValue() == null) {                
            //    jAlert("Please enter E-Way Bill Number.");
            //        return false;               
            //}
            //else
            //{
            var UpdateEWayBill = ctxtEWayBillNumber.GetValue();
            if (UpdateEWayBill == "0") {
                UpdateEWayBill = "";
            }
            if (cdt_EWayBill.GetValue() == "" && cdt_EWayBill.GetValue() == null) {
                var EWayBillDate = "1990-01-01";
            }
            else {
                var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
            }

            var EWayBillValue = ctxtEWayBillValue.GetValue();

            $.ajax({
                type: "POST",
                url: "PurchaseInvoicelist.aspx/UpdateEWayBill",
                data: JSON.stringify({
                    InvoiceID: InvoiceID, UpdateEWayBill: UpdateEWayBill, EWayBillDate: EWayBillDate, EWayBillValue: EWayBillValue
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Saved successfully.");
                        //ctxtEWayBillNumber.SetText("");
                        cPopup_EWayBill.Hide();
                        cgrid.Refresh();
                    }
                    else if (status == "-10") {
                        jAlert("Data not saved.");
                        cPopup_EWayBill.Hide();
                    }
                }
            });
            //}           
        }
        function CancelEWayBill_save() {
            cPopup_EWayBill.Hide();
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback();
            cDocumentsPopup.Hide();
            return false;
        }

        function cSelectPanelEndCall(s, e) {
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'PInvoice';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + PInvoice_id + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    //jAlert('Please check Original For Recipient and proceed.');
                    jAlert('Please check atleast one option and proceed.');
                }
                CselectDuplicate.SetEnabled(false);
                CselectTriplicate.SetEnabled(false);
                CselectNone.SetCheckState('UnChecked');
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        function NoneCheckChange(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CselectOriginal.SetEnabled(false);
            }
            else {
                CselectOriginal.SetCheckState('UnChecked');
                CselectOriginal.SetEnabled(true);
                CselectDuplicate.SetCheckState('UnChecked');
                CselectDuplicate.SetEnabled(false);
                CselectTriplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetEnabled(false);
            }
        }

        function OrginalCheckChange(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CselectDuplicate.SetEnabled(true);
                CselectNone.SetCheckState('UnChecked');
                CselectNone.SetEnabled(false);
            }
            else {
                CselectNone.SetEnabled(true);
                CselectDuplicate.SetCheckState('UnChecked');
                CselectDuplicate.SetEnabled(false);
                CselectTriplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetEnabled(false);
            }

        }
        function DuplicateCheckChange(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CselectTriplicate.SetEnabled(true);
            }
            else {
                CselectTriplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetEnabled(false);
            }
        }
        function OpenPopUPUserWiseQuotaion() {
            cgridUserWiseQuotation.PerformCallback();
            cPopupUserWiseQuotation.Show();
        }
        document.onkeydown = function (e) {

            if (event.keyCode == 73 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddInventoryButtonClick();
            }
            if (event.keyCode == 78 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddNonInventoryButtonClick();
            }
            if (event.keyCode == 67 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddCapitalButtonClick();
            }
            if (event.keyCode == 66 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddBothButtonClick();
            }
            if (event.keyCode == 83 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddServiceButtonClick();
            }
        }
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };
            e.returnValue = false;
            e.stopPropagation();
        }
        function OnAddInventoryButtonClick() {
            var url = 'PurchaseInvoice.aspx?key=' + 'ADD&&InvType=Y';
            window.location.href = url;
        }
        function OnAddNonInventoryButtonClick() {
            var url = 'PurchaseInvoice.aspx?key=' + 'ADD&&InvType=N';
            window.location.href = url;
        }
        function OnAddCapitalButtonClick() {
            var url = 'PurchaseInvoice.aspx?key=' + 'ADD&&InvType=C';
            window.location.href = url;
        }
        function OnAddBothButtonClick() {
            var url = 'purchaseinvoice.aspx?key=' + 'ADD&&InvType=B';
            window.location.href = url;
        }
        function OnAddServiceButtonClick() {
            var url = 'purchaseinvoice.aspx?key=' + 'ADD&&InvType=S';
            window.location.href = url;
        }
        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cgrid.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        function OnClickStatus(keyValue) {
            GetObjectID('hiddenedit').value = keyValue;
            cgrid.PerformCallback('Edit~' + keyValue);
        }
        function OpenPopUPApprovalStatus() {
            cgridPendingApproval.PerformCallback();
            cpopupApproval.Show();
        }

        function grid_EndCallBack() {
            if (cgrid.cpEdit != null) {
                GetObjectID('hiddenedit').value = cgrid.cpEdit.split('~')[0];
                cProforma.SetText(cgrid.cpEdit.split('~')[1]);
                cCustomer.SetText(cgrid.cpEdit.split('~')[4]);
                var pro_status = cgrid.cpEdit.split('~')[2]
                //cgrid.cpEdit = null;
                if (pro_status != null) {
                    var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    //return false;
                    //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
                    cQuotationRemarks.SetText(cgrid.cpEdit.split('~')[3]);

                    cQuotationStatus.Show();
                }
            }
            if (cgrid.cpUpdate != null) {
                GetObjectID('hiddenedit').value = '';
                cProforma.SetText('');
                cCustomer.SetText('');
                cQuotationRemarks.SetText('');
                var pro_status = 2;
                if (pro_status != null) {
                    var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    cQuotationStatus.Hide();
                }
                jAlert(cgrid.cpUpdate);
            }
            if (cgrid.cpDelete != null) {
                jAlert(cgrid.cpDelete);
                updateGridAfterDelete();
                cgrid.cpDelete = null;
            }


        }
        function SavePrpformaStatus() {
            if (document.getElementById('hiddenedit').value == '') {
                cgrid.PerformCallback('save~');
            }
            else {
                var checked_radio = $("[id*=rbl_QuoteStatus] input:checked");
                var status = checked_radio.val();
                var remarks = cQuotationRemarks.GetText();
                cgrid.PerformCallback('update~' + GetObjectID('hiddenedit').value + '~' + status + '~' + remarks);
            }

        }

        function OnMoreInfoClick(keyValue) {
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "purchaseinvoicelist.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'PurchaseInvoice.aspx?key=' + keyValue + '&Permission=' + status + '&type=PB';
                        window.location.href = url;
                    }
                });
            }
        }

        ////##### coded by Samrat Roy - 04/05/2017   
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            // alert(keyValue);
            var url = '/OMS/Management/Activities/View/ViewPnv.html?id=' + keyValue;
            CAspxDirectAddPnvPopup.SetContentUrl(url);
            //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();
            CAspxDirectAddPnvPopup.RefreshContentUrl();
            CAspxDirectAddPnvPopup.Show();
        }


        function OnclickViewAttachment(obj) {
            var URL = '/OMS/Management/Activities/PurchaseInvoice_Document.aspx?idbldng=' + obj + '&type=PurchaseInvoice';
            window.location.href = URL;
        }

        function OnAddButtonClick() {
            var url = 'PurchaseInvoice.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        var keyval;
        //function FocusedRowChanged(s, e) {
        //    keyval=s.GetRowKey(s.GetFocusedRowIndex());
        //}

        //var globalRowIndex;

        //function GetVisibleIndex(s, e) {
        //    globalRowIndex = e.visibleIndex;
        //}
        //RowClick = "GetVisibleIndex"

        // User Approval Status Start

        function GetApprovedQuoteId(s, e, itemIndex) {
            var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
            //var currentRow = cgridPendingApproval.GetRow(0);
            //var col1 = currentRow.find("td:eq(0)").html();

            cgridPendingApproval.PerformCallback('Status~' + rowvalue);
            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

        }
        function OnGetApprovedRowValues(obj) {
            uri = "PurchaseInvoice.aspx?key=" + obj + "&status=2" + '&type=PB';
            popup.SetContentUrl(uri);
            popup.Show();
            //window.location.href = uri;

        }

        function ch_fnApproved() {
        }


        function GetRejectedQuoteId(s, e, itemIndex) {

            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

        }
        function OnGetRejectedRowValues(obj) {
            uri = "PurchaseInvoice.aspx?key=" + obj + "&status=3" + '&type=PB';
            popup.SetContentUrl(uri);
            popup.Show();
        }

        // User Approval Status End

        function OnApprovalEndCall(s, e) {
            $.ajax({
                type: "POST",
                url: "purchaseinvoicelist.aspx/GetPendingCase",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#<%= lblWaiting.ClientID %>').text(data.d);
                }
            });
            }
    </script>
    <style>
        strong label {
            font-weight: bold !important;
        }

        input[type="radio"] {
            webkit-transform: translateY(3px);
            -moz-transform: translateY(3px);
            transform: translateY(3px);
        }

        .blink {
            animation: blink-animation 1s steps(5, start) infinite;
            -webkit-animation: blink-animation 1s steps(5, start) infinite;
            cursor: pointer;
            color: #128AC9;
        }

        @keyframes blink-animation {
            to {
                visibility: hidden;
            }
        }

        @-webkit-keyframes blink-animation {
            to {
                visibility: hidden;
            }
        }

        .padTab > tbody > tr > td {
            padding-right: 15px;
        }
        .dxeErrorFrameWithoutError_PlasticBlue .dxeControlsCell_PlasticBlue, .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue {
             padding:0px !important; 
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Opening Purchase Invoice</h3>
        </div>
        <table class="padTab pull-right">
            <tr>
                <td>From </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" OnInit="FormDate_Init" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>To 
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" OnInit="toDate_Init" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>

                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updatePBGridByDate()" />
                </td>

            </tr>

        </table>
    </div>
    <%--Code Added by Sam For Filteration Section Start--%>

    <%--Code Added by Sam For Filteration Section Start--%>



    <div class="form_main">
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <%--<a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span> <u>A</u>dd New</span> </a>--%>
           <%-- <a href="javascript:void(0);" onclick="OnAddInventoryButtonClick()" class="btn btn-primary"><span>Add <u>I</u>nventory</span> </a>
            <a href="javascript:void(0);" onclick="OnAddNonInventoryButtonClick()" class="btn btn-primary"><span>Add <u>N</u>on Inventory</span> </a>
            <a href="javascript:void(0);" onclick="OnAddCapitalButtonClick()" class="btn btn-primary"><span>Add <u>C</u>apital Goods</span> </a>
            <a href="javascript:void(0);" onclick="OnAddBothButtonClick()" class="btn btn-primary"><span>Add <u>B</u>oth</span> </a>
            <a href="javascript:void(0);" onclick="OnAddServiceButtonClick()" class="btn btn-primary"><span>Add <u>S</u>ervice</span> </a>--%>
            <%} %>
            <% if (rights.CanExport)
               { %>
          <%--  <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">XLS</asp:ListItem>
                <asp:ListItem Value="2">PDF</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>--%>
            <% } %>
            <span id="spanStatus" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary hide">
                    <span>My Purchase Invoice Status</span>
                    <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
                </a>
            </span>
            <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-primary">
                    <span>Approval Waiting</span>
                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                </a>
                <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>

            </span>

        </div>
    </div>
    <div class="GridViewArea">
        <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False" Settings-HorizontalScrollBarMode="Visible"
            Width="100%" ClientInstanceName="cgrid" OnCustomCallback="GrdQuotation_CustomCallback" Settings-VerticalScrollableHeight="300"
            Settings-VerticalScrollBarMode="Visible"
            OnSummaryDisplayText="GrdQuotation_SummaryDisplayText"
            DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
            SettingsBehavior-AllowFocusedRow="true">
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <%--  SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" 
             SettingsCookies-StoreGroupingAndSorting="true" --%>
            <Columns>
                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="Invoice_Id" Caption="Invoice_Id" SortOrder="Descending">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNumber" VisibleIndex="1" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="InvoiceDt" VisibleIndex="2" Width="85px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Vendor" FieldName="VendorName" VisibleIndex="3" Width="180px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Invoice Type" FieldName="Invoice_InventoryItem" VisibleIndex="4" Width="180px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch" VisibleIndex="5" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Party Invoice No" FieldName="PartyInvoiceNo" VisibleIndex="6" Width="140px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Party Invoice Date" FieldName="PartyInvoiceDate" VisibleIndex="7" Width="120px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="GRN No" FieldName="ChallanNumber" VisibleIndex="8" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="GRN Date" FieldName="ChallanDate" VisibleIndex="9" Width="78px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="E-Way Bill No" FieldName="EWayBillNumber" VisibleIndex="10" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Place of Supply[GST]" FieldName="PlaceOfSupply" VisibleIndex="11" Width="100px">
                  
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Entered by" FieldName="CreatedBY" VisibleIndex="12" Width="80px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Entered On" FieldName="CreatedDate" Width="80px" VisibleIndex="13">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Updated by" FieldName="UpdatedBy" VisibleIndex="14" Width="80px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="updatedOn" Width="80px" VisibleIndex="15">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="TotalAmount" VisibleIndex="16" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="Product" Caption="" VisibleIndex="17" Width="80px">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Container.KeyValue %>')">
                            <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("Product")%>'
                                ToolTip="Click to Change Status">
                            </dxe:ASPxLabel>
                        </a>
                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False" CssClass="text-center">
                    </CellStyle>
                    <%-- <HeaderTemplate>
                        Status
                    </HeaderTemplate>--%>
                    <Settings AllowAutoFilter="False" />
                    <HeaderStyle Wrap="False" CssClass="text-center" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="18" Width="160px">
                    <DataItemTemplate>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="View">
                            <img src="../../../assests/images/doc.png" /></a>
                        <% } %>
                        <% if (rights.CanEdit)
                           { %>
                      <%--  <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="pad" title="Edit">
                            <img src="../../../assests/images/info.png" /></a>--%>
                        <%} %>
                        <% if (rights.CanDelete)
                           { %>
                      <%--  <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete">
                            <img src="../../../assests/images/Delete.png" /></a>--%>
                        <%} %>

                        <% if (rights.CanView)
                           { %>
                       <%-- <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="pad" title="View Attachment">
                            <img src="../../../assests/images/attachment.png" />
                        </a>--%>
                        <%} %>
                   <%--     <a href="javascript:void(0);" onclick="OnEWayBillClick('<%# Container.KeyValue %>','<%#Eval("EWayBillNumber") %>','<%#Eval("EWayBillDate") %>','<%#Eval("EWayBillValue") %>')" 
                            class="pad" title="Update E-Way Bill">
                            <img src="../../../assests/images/bill.png" />--%>
                            <% if (rights.CanPrint)
                               { %>
                          <%--  <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="pad" title="print">
                                <img src="../../../assests/images/Print.png" />
                            </a>--%>
                            <%} %>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>


            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <TotalSummary>
                <dxe:ASPxSummaryItem FieldName="TotalAmount" SummaryType="Sum" />
            </TotalSummary>
            <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" BeginCallback="BeginCallback" />


            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
            </SettingsPager>



            <Settings ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_PBList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

    <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%-- <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgriddocuments" ID="grid_Documents"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridDocuments_CustomCallback" ClientSideEvents-EndCallback="cgridDocumentsEndCall"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"
                        Settings-VerticalScrollableHeight="100" Settings-VerticalScrollBarMode="Hidden">
                                                      
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="10" Caption=" " />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="ID" Width="0" ReadOnly="true" Caption="No." CellStyle-CssClass="hide" HeaderStyle-CssClass="hide">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NAME" Width="" ReadOnly="true" Caption="Design(s)">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:ASPxGridView>
                    <div class="text-center pTop10">
                        <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server"  AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                            <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />                            
                        </dxe:ASPxButton>
                    </div>--%>

                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxCheckBox ID="selectNone" Text="None" runat="server" ToolTip="Select None" ClientSideEvents-CheckedChanged="NoneCheckChange"
                                    ClientInstanceName="CselectNone">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" ClientSideEvents-CheckedChanged="OrginalCheckChange"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>

                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>

                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>


    <div class="PopUpArea">

        <%--Client Wise Quotation Status Section Start--%>

        <dxe:ASPxPopupControl ID="Popup_QuotationStatus" runat="server" ClientInstanceName="cQuotationStatus"
            Width="500px" HeaderText="Approvers Configuration" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-12">
                                <table width="">
                                    <tr>
                                        <td style="padding-right: 8px; vertical-align: top">
                                            <strong>
                                                <label style="margin-bottom: 5px">Proforma:-</label></strong>
                                        </td>
                                        <td style="padding-top: 0; vertical-align: top; padding-right: 35px">
                                            <%--<dxe:ASPxTextBox ID="txt_Proforma" MaxLength="80" ClientInstanceName="cProforma" TabIndex="1" 
                                                runat="server" Width="100%"> 
                                            </dxe:ASPxTextBox>--%>
                                            <strong>
                                                <dxe:ASPxLabel ID="lbl_Proforma" runat="server" ClientInstanceName="cProforma" Text="ASPxLabel"></dxe:ASPxLabel>
                                            </strong>
                                        </td>
                                        <td style="padding-right: 8px; padding-left: 8px; vertical-align: top">
                                            <strong>
                                                <label style="margin-bottom: 5px">Customer:-</label></strong>
                                        </td>
                                        <td style="padding-top: 0; vertical-align: top">
                                            <%-- <dxe:ASPxTextBox ID="txt_Customer" ClientInstanceName="cCustomer"  runat="server" MaxLength="100" TabIndex="2"
                                            Width="100%"> 
                                        </dxe:ASPxTextBox>--%>
                                            <strong>
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" ClientInstanceName="cCustomer" Text="ASPxLabel"></dxe:ASPxLabel>
                                            </strong>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-6">
                            </div>
                            <div class="col-md-6">
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <table>
                            <tr>
                                <td style="width: 70px; padding: 13px 0;"><strong>Status </strong></td>
                                <td>
                                    <asp:RadioButtonList ID="rbl_QuoteStatus" runat="server" Width="172px" CssClass="mTop5" RepeatDirection="Horizontal">

                                        <asp:ListItem Text="Accepted" Value="2" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Declined" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                        </table>





                    </div>
                    <div class="clear"></div>
                    <div class="col-md-12">

                        <div class="" style="margin-bottom: 5px;">
                            <strong>Reason </strong>
                        </div>

                        <div>
                            <dxe:ASPxMemo ID="txt_QuotationRemarks" runat="server" ClientInstanceName="cQuotationRemarks" Height="71px" Width="100%"></dxe:ASPxMemo>
                        </div>
                    </div>

                    <div class="col-md-12" style="padding-top: 10px;">
                        <dxe:ASPxButton ID="btn_PrpformaStatus" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                            AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                            <ClientSideEvents Click="function (s, e) {SavePrpformaStatus();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <%--Client Wise Quotation Status Section END--%>

        <%-- Sandip Approval Dtl Section Start--%>


        <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
            Width="900px" HeaderText="Pending Approvals" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="CreateDate"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch_description"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="craetedby"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Approved">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkapprove" runat="server" AllowGrayed="false" OnInit="chkapprove_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%-- <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%-- <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <SettingsSearchPanel Visible="True" />
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                                <ClientSideEvents EndCallback="OnApprovalEndCall" />
                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>


                        <%--<div class="col-md-12" style="padding-top: 10px;">
                            <dxe:ASPxButton ID="ASPxButton1" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                                AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {SaveApprovalStatus();}" />
                            </dxe:ASPxButton>
                        </div>--%>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="1200px" HeaderText="Quotation Approval" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <HeaderTemplate>
                <span>User Approval</span>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="PopupUserWiseQuotation" runat="server" ClientInstanceName="cPopupUserWiseQuotation"
            Width="900px" HeaderText="User Wise Purchase Invoice Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Purchase Invoice No." FieldName="number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <SettingsSearchPanel Visible="True" />
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />

                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <%-- Sandip Approval Dtl Section End--%>
    </div>

    <%--Product Name Detail Invoice Wise--%>
    <dxe:ASPxPopupControl ID="productpopup" ClientInstanceName="cproductpopup" runat="server"
        AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Product Detail"
        EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxCallbackPanel ID="propanel" runat="server" Width="400px" ClientInstanceName="popproductPanel"
                    OnCallback="propanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <div>
                                <dxe:ASPxGridView ID="grdproduct" runat="server" KeyFieldName="id" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cpbproduct">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Product Name" FieldName="product" HeaderStyle-CssClass="text-center"
                                            VisibleIndex="0" FixedStyle="Left" Width="150px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:ASPxGridView>
                            </div>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left">
            <Paddings PaddingRight="6px" />
        </HeaderStyle>
        <SizeGripImage Height="16px" Width="16px" />
        <CloseButtonImage Height="12px" Width="13px" />
        <ClientSideEvents CloseButtonClick="function(s, e) {
popup.Hide();
}" />
    </dxe:ASPxPopupControl>
    <%--Product Name Detail Invoice Wise--%>
    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hddnInvoiceID" runat="server" />

    </div>


    <dxe:ASPxPopupControl ID="Popup_EWayBill" runat="server" ClientInstanceName="cPopup_EWayBill"
        Width="400px" HeaderText="Update E-Way Bill" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div class="Top clearfix">

                    <table style="width: 100%; margin: 0 auto; margin-top: 5px;">

                        <tr>
                            <td>
                            <label><dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                            </dxe:ASPxLabel></label>

                            <dxe:ASPxTextBox ID="txtEWayBillNumber" MaxLength="12" ClientInstanceName="ctxtEWayBillNumber"
                                runat="server" Width="100%">
                                <MaskSettings Mask="<0..999999999999>" AllowMouseWheel="false" />
                                <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                            </dxe:ASPxTextBox>
                           </td>
                        </tr>

                        <tr>
                            <td>
                                <label style="margin-top:6px;"><dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="E-Way Bill Date">
                                </dxe:ASPxLabel></label>
                                <dxe:ASPxDateEdit ID="dt_EWayBill" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EWayBill" Width="100%" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            

                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top:6px;"><dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="E-Way Bill Value">
                                </dxe:ASPxLabel></label>
                                <dxe:ASPxTextBox ID="txtEWayBillValue" ClientInstanceName="ctxtEWayBillValue"
                                    runat="server" Width="100%">
                                    <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="margin-top: 10px;">
                        <input id="btnEWayBillSave" class="btn btn-primary" onclick="CallEWayBill_save()" type="button" value="Save" />
                        <input id="btnEWayBillCancel" class="btn btn-danger" onclick="CancelEWayBill_save()" type="button" value="Cancel" />
                    </div>

                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>
      <dxe:ASPxPopupControl ID="AspxDirectAddPnvPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectAddPnvPopup" Height="650px"
        Width="1020px" HeaderText="View Purchase Invoice " Modal="true" AllowResize="true">
         
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

</asp:Content>

