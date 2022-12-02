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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Imports Sales Invoice</h3>
        </div>
    </div>
    <div class="form_main" style="align-items: center;">
        <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
            <div class="col-md-3">
                <label>Numbering Scheme</label>
                <div>
                    <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                    </asp:DropDownList>
                    <span id="div_numberingScheme" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div class="col-md-3">
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
                    <label>&nbsp;</label>
                    <div>
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
        <div class="clearfix">
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
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridInvoice" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
