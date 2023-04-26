<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                06-04-2023        2.0.37           Pallab              25919: Imports Sales Invoice module design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Imports Sales Invoice" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SalesInvoice_Import.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesInvoice_Import" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .pullleftClass {
            position: absolute;
            right: -4px;
            top: 32px;
        }
    </style>

    <script>
        $(document).ready(function () {
            $('#ddl_numberingScheme').change(function () {
                clookup_MainAccount.gridView.SetFocusedRowIndex(-1);
                clookup_SubAccount.gridView.SetFocusedRowIndex(-1);

                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];

                var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
                if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
                GetObjectID('hdnBranchID').value = branchID;

                cMainAccountPanel.PerformCallback();
            });
        });

        function btnImportClick() {
            cgridInvoice.PerformCallback("Save");
        }

        function CloseGridLookup() {
            clookup_MainAccount.ConfirmCurrentSelection();
            clookup_MainAccount.HideDropDown();
            clookup_MainAccount.Focus();
        }

        function GetMainAccount(e) {
            clookup_SubAccount.gridView.SetFocusedRowIndex(-1);
            var MainAccount = clookup_MainAccount.GetGridView().GetRowKey(clookup_MainAccount.GetGridView().GetFocusedRowIndex());
            cSubAccountPanel.PerformCallback(MainAccount);
        }

        function checkValidate() {
            if ($('#ddl_numberingScheme').val() == "") {
                $("#div_numberingScheme").show();
                return false;
            }
            else {
                $("#div_numberingScheme").hide();
            }

            var NoSchemeTypedtl = $("#ddl_numberingScheme").val();
            var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
            if (branchID != "") document.getElementById('ddl_Branch').value = branchID;

            var key = clookup_MainAccount.GetGridView().GetRowKey(clookup_MainAccount.GetGridView().GetFocusedRowIndex());
            if (key == null || key == '') {
                $("#div_mainAccount").show();
                return false;
            }
            else {
                $("#div_mainAccount").hide();
            }
        }
    </script>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dt_OADate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dt_OADate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dt_OADate_B-1 #dt_OADate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 34px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
                z-index: 0;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
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
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        #txtProdSearch
        {
            margin-bottom: 10px;
        }

        select.btn
        {
           position: relative;
           z-index: 0;
        }

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        #massrecdt
        {
            width: 100%;
        }

        .col-sm-3 , .col-md-3{
            margin-bottom: 10px;
        }

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        input[type="text"], input[type="password"], textarea
        {
                margin-bottom: 0;
        }

        .typeNotification span
        {
             color: #ffffff !important;
        }

        #rdl_Salesquotation
        {
            margin-top: 8px;
    line-height: 20px;
        }

        #ASPxLabel8
        {
            line-height: 16px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
                margin-top: 0 !important;
        }

        #OFDBankSelect
        {
            height: 30px;
            border-radius: 4px;
        }

        .mt-28{
            margin-top: 28px;
        }

        .mb-10{
            margin-bottom: 10px;
        }

        /*Rev end 1.0*/
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Imports Sales Invoice</h3>
        </div>
    </div>
        <div class="form_main" style="align-items: center;">
        <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
            <%--Rev 1.0: "simple-select" class add --%>
            <div class="col-md-3 simple-select">
                <label>Numbering Scheme</label>
                <div>
                    <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                    </asp:DropDownList>
                    <span id="div_numberingScheme" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <%--Rev 1.0: "simple-select" class add --%>
            <div class="col-md-3 simple-select">
                <label>Unit</label>
                <div>
                    <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" Enabled="false">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
                <label>Main Account</label>
                <div>
                    <dxe:ASPxCallbackPanel runat="server" ID="MainAccountPanel" ClientInstanceName="cMainAccountPanel" OnCallback="MainAccountPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_MainAccount" runat="server" TabIndex="5" ClientInstanceName="clookup_MainAccount"
                                    KeyFieldName="MainAccount_ReferenceID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False"
                                    OnDataBinding="lookup_MainAccount_DataBinding" callback>
                                    <Columns>
                                        <dxe:GridViewDataColumn FieldName="IntegrateMainAccount" Visible="true" VisibleIndex="0" Caption="Main Account" Width="200px" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
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
                                    <ClientSideEvents TextChanged="function(s, e) { GetMainAccount(e)}" GotFocus="function(s,e){gridLookup.ShowDropDown();}" />
                                    <ClearButton DisplayMode="Auto">
                                    </ClearButton>
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                    <span id="div_mainAccount" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div class="col-md-3">
                <label>Sub Account</label>
                <div>
                    <dxe:ASPxCallbackPanel runat="server" ID="SubAccountPanel" ClientInstanceName="cSubAccountPanel" OnCallback="SubAccountPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_SubAccount" runat="server" TabIndex="5" ClientInstanceName="clookup_SubAccount"
                                    KeyFieldName="SubAccount_ReferenceID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" OnDataBinding="lookup_SubAccount_DataBinding">
                                    <Columns>
                                        <dxe:GridViewDataColumn FieldName="Contact_Name" Visible="true" VisibleIndex="0" Caption="Sub Account" Width="350px" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
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
                                    <ClearButton DisplayMode="Auto">
                                    </ClearButton>
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                    <span id="div_subAccount" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div style="clear: both">
                <div class="col-md-3">
                    <label>Choose File</label>&nbsp;&nbsp;<span style="color: red">(Only .CSV File)</span>
                    <div>
                        <asp:FileUpload ID="OFDBankSelect" runat="server" Width="100%" />
                    </div>
                </div>
                <div class="col-md-9">
                    <%--<label>&nbsp;</label>--%>
                    <div class="mt-28">
                        <%-- <dxe:ASPxButton ID="btnImport" ClientInstanceName="cbtnImport" runat="server" AutoPostBack="False" Text="Import File" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                        <ClientSideEvents Click="function(s, e) {btnImportClick();}" />
                    </dxe:ASPxButton>--%>
                        <asp:Button ID="btnImport" runat="server" Text="Import File" CssClass="btn btn-primary" OnClick="btnImport_Click" OnClientClick="javascript:return checkValidate()" />
                        <asp:LinkButton ID="lnlDownloader" runat="server" OnClick="lnlDownloader_Click" CssClass="btn btn-info">Download Format</asp:LinkButton>
                         <label style="color:red""><b><font size="2">** Document Date format must be 'DD-MM-YYYY' (English-India)</font></b></label>
                    </div>
                </div>                
            </div>
        </div>
        <div class="clearfix" style="align-items: center;">
            <div class="col-md-3">
                <asp:Label ID="txtErrorMessege" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                <div>
                    <asp:Label ID="txtErrorList" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
        <div class="clearfix mb-10">
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div>
            <dxe:ASPxGridView ID="gridInvoice" runat="server" KeyFieldName="DCNoteProductl_ID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                Width="100%" ClientInstanceName="cgridInvoice" OnDataBinding="gridInvoice_DataBinding" OnSummaryDisplayText="gridInvoice_SummaryDisplayText"
                SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="DocumentNumber"
                        VisibleIndex="0" Width="10%">
                        <CellStyle Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="DCNote_DocumentDate"
                        VisibleIndex="1" Width="10%">
                        <CellStyle Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="ShipToParty"
                        VisibleIndex="2" Width="25%">
                        <CellStyle Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Product Name" FieldName="ProductName"
                        VisibleIndex="3" Width="25%">
                        <CellStyle Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Product Class" FieldName="ProductClass"
                        VisibleIndex="4" Width="5%">
                        <CellStyle Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                        VisibleIndex="5" Width="10%">
                        <CellStyle Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        <PropertiesTextEdit DisplayFormatString="0.00">
                            <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" />
                        </PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                        VisibleIndex="6" Width="10%">
                        <CellStyle Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        <PropertiesTextEdit DisplayFormatString="0.00">
                            <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" />
                        </PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Total Amount" FieldName="DCNote_TotalAmount"
                        VisibleIndex="7" Width="10%">
                        <CellStyle Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        <PropertiesTextEdit DisplayFormatString="0.00">
                            <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" />
                        </PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Quantity" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                </TotalSummary>
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <SettingsSearchPanel Visible="True" />
                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <Settings ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsLoadingPanel Text="Please Wait..." />
            </dxe:ASPxGridView>

            <%-- <dxe:ASPxGridView ID="gridInvoice" ClientInstanceName="cgridInvoice" runat="server" KeyFieldName="ProductID" AutoGenerateColumns="True"
                Width="100%" EnableRowsCache="true" SettingsBehavior-AllowFocusedRow="true"
                OnCustomCallback="gridInvoice_CustomCallback" OnDataBinding="gridInvoice_DataBinding">
            </dxe:ASPxGridView>--%>
        </div>
        <asp:HiddenField ID="hdnBranchID" runat="server" />
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridInvoice" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
