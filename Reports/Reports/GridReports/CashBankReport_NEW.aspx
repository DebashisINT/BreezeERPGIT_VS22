<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CashBankReport_NEW.aspx.cs" Inherits="Reports.Reports.GridReports.CashBankReport_NEW" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function _selectAll() {
            clookupBranch.gridView.SelectRows();
        }

        function _unselectAll() {
            clookupBranch.gridView.UnselectRows();
        }

        function _CloseLookup() {
            clookupBranch.ConfirmCurrentSelection();
            clookupBranch.HideDropDown();
            clookupBranch.Focus();
        }

        function selectAll() {
            clookupCashBank.gridView.SelectRows();
        }

        function unselectAll() {
            clookupCashBank.gridView.UnselectRows();
        }

        function CloseLookup() {
            clookupCashBank.ConfirmCurrentSelection();
            clookupCashBank.HideDropDown();
            clookupCashBank.Focus();
        }

        function select_All() {
            clookupUser.gridView.SelectRows();
        }

        function unselect_All() {
            clookupUser.gridView.UnselectRows();
        }

        function Close_Lookup() {
            clookupUser.ConfirmCurrentSelection();
            clookupUser.HideDropDown();
            clookupUser.Focus();
        }

        function _select_All() {
            clookupLedger.gridView.SelectRows();
        }

        function _unselect_All() {
            clookupLedger.gridView.UnselectRows();
        }

        function _Close_Lookup() {
            clookupLedger.ConfirmCurrentSelection();
            clookupLedger.HideDropDown();
            clookupLedger.Focus();
        }

        function BindOtherDetails(e) {
            clookupCashBank.gridView.SetFocusedRowIndex(-1);
            clookupUser.gridView.SetFocusedRowIndex(-1);
            clookupLedger.gridView.SetFocusedRowIndex(-1);

            //cLedgerPanel.PerformCallback();
            cUserPanel.PerformCallback();
            cCashBankPanel.PerformCallback();
        }

        function ddlType_Change() {
            clookupCashBank.gridView.SetFocusedRowIndex(-1);
            cCashBankPanel.PerformCallback();
        }

        function btn_ShowRecordsClick(e) {
            $("#drdExport").val(0);

            //if (clookupBranch.GetValue() == null) {
            //    jAlert('Please select atleast one branch');
            //}
            //else {
            // cShowGrid.PerformCallback();
            cShowGrid.Refresh();
            <%--}--%>
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Cash/Bank Book Report </h3>
        </div>
    </div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxGridLookup ID="lookupBranch" ClientInstanceName="clookupBranch" SelectionMode="Multiple" runat="server"
                    OnDataBinding="lookupBranch_DataBinding" KeyFieldName="Branch_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                        <dxe:GridViewDataColumn FieldName="Branch_Name" Visible="true" VisibleIndex="1" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="Branch_Id" Visible="true" VisibleIndex="2" Caption="Brand ID" Settings-AutoFilterCondition="Contains" Width="0">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            <%--<div class="hide">--%>
                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="_selectAll" />
                                            <%--</div>--%>
                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="_unselectAll" />
                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="_CloseLookup" UseSubmitBehavior="False" />
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                        <SettingsPager Mode="ShowPager">
                        </SettingsPager>
                        <SettingsPager PageSize="20">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                        </SettingsPager>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                    <ClientSideEvents TextChanged="function(s, e) { BindOtherDetails(e)}" />
                </dxe:ASPxGridLookup>
                <span id="MandatoryActivityType" style="display: none" class="validclass" />
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label5" runat="Server" Text="User : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxCallbackPanel runat="server" ID="UserPanel" ClientInstanceName="cUserPanel" OnCallback="UserPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupUser" ClientInstanceName="clookupUser" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupUser_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="User" Visible="true" VisibleIndex="1" Caption="User Name" Width="250px" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="2" Caption="ID" Settings-AutoFilterCondition="Contains" Width="0">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <%--<div class="hide">--%>
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="select_All" />
                                                       <%-- </div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselect_All" />
                                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="Close_Lookup" UseSubmitBehavior="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                    <SettingsPager Mode="ShowPager">
                                    </SettingsPager>
                                    <SettingsPager PageSize="20">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                    </SettingsPager>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                </GridViewProperties>
                            </dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
                <span id="MandatorClass" style="display: none" class="validclass" />
            </div>

            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Cash/Bank Type : " CssClass="mylabel1"></asp:Label>
                </label>
                <asp:DropDownList ID="ddlType" runat="server" Width="100%" onchange="ddlType_Change()">
                    <asp:ListItem Text="Bank" Value="Bank"></asp:ListItem>
                    <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Cash/Bank : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxCallbackPanel runat="server" ID="CashBankPanel" ClientInstanceName="cCashBankPanel" OnCallback="CashBankPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupCashBank" ClientInstanceName="clookupCashBank" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupCashBank_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Settings-AutoFilterCondition="Contains" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="2" Caption="ID" Settings-AutoFilterCondition="Contains" Width="0">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <%--<div class="hide">--%>
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />
                                                        <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookup" UseSubmitBehavior="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                    <SettingsPager Mode="ShowPager">
                                    </SettingsPager>
                                    <SettingsPager PageSize="20">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                    </SettingsPager>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                </GridViewProperties>
                            </dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
                <span id="MandatorClass" style="display: none" class="validclass" />
            </div>
            <div class="clear"></div>
            <div class="col-md-2" style="padding-top: 10px;">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:CheckBox ID="chksubledger" runat="server" Checked="false"></asp:CheckBox>
                    <asp:Label ID="Label2" runat="Server" Text="Show Sub Ledger" CssClass="mylabel1"></asp:Label>
                </div>
            </div>
            <div class="col-md-2" style="display: none;">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label6" runat="Server" Text="Ledger : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxCallbackPanel runat="server" ID="LedgerPanel" ClientInstanceName="cLedgerPanel" OnCallback="LedgerPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupLedger" ClientInstanceName="clookupLedger" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupLedger_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="Doc_Code" Visible="true" VisibleIndex="1" Caption="Doc Code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Description" Visible="true" VisibleIndex="2" Caption="Description" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="3" Caption="ID" Settings-AutoFilterCondition="Contains" Width="0">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <div class="hide">
                                                            <dxe:ASPxButton ID="ASPxButton9" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="_select_All" />
                                                        </div>
                                                        <dxe:ASPxButton ID="ASPxButton10" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="_unselect_All" />
                                                        <dxe:ASPxButton ID="ASPxButton11" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="_Close_Lookup" UseSubmitBehavior="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                    <SettingsPager Mode="ShowPager">
                                    </SettingsPager>
                                    <SettingsPager PageSize="20">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                    </SettingsPager>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                </GridViewProperties>
                            </dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
                <%-- <dxe:ASPxCallbackPanel runat="server" ID="Generate" ClientInstanceName="cGenerate" OnCallback="GeneratePanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <button id="btnGenerate" class="btn btn-primary" type="button" onclick="btn_GenerateRecordsClick(this);">Generate</button>
                            </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>--%>

                <span id="MandatorClass" style="display: none" class="validclass" />
            </div>
            <div class="clear"></div>
            <div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                <div class="">
<%--                    <button id="btnGenerate" class="btn btn-primary" type="button"  onclick="btn_GenerateRecordsClick(this);">Generate</button>--%>
                    <asp:Button runat="server" ID="btnGenerate" CssClass="btn btn-primary" OnClick="btnGenerate_Click" Text="Generate"/>
                    <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                        AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="clearfix">
            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="cShowGrid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                  KeyFieldName="SrlNo"
                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible" DataSourceID="lsmdsCashBankReport">
                <Columns>
                     <dxe:GridViewDataTextColumn FieldName="CASHBANKNAME" Caption="Cash/Bank Name" Width="150" VisibleIndex="0" GroupIndex="0">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="SLNO" Caption="Sl." Width="50" VisibleIndex="1">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataComboBoxColumn FieldName="BRANCH_DESC" Caption="Branch" Width="100" VisibleIndex="2">
                        <PropertiesComboBox DataSourceID="sqlbranch" TextField="branch_description"
                            ValueField="branch_description" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                        </PropertiesComboBox>
                    </dxe:GridViewDataComboBoxColumn>
                    <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT" Caption="Ledger Desc." Width="350" VisibleIndex="3">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="Voucher No." Width="130" VisibleIndex="4">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="TRAN_DATE" Caption="Voucher Date" Width="80" VisibleIndex="5">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_NO" Caption="Cheque Number" Width="130" VisibleIndex="6">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_DATE" Caption="Cheque Date" Width="80" VisibleIndex="7">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DRAWEE_BANK" Caption="Cheque On Bank" Width="200" VisibleIndex="8">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="TRAN_TYPE" Caption="Doc. Type" Width="200" VisibleIndex="9">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Particulars" Caption="Particulars" Width="300" VisibleIndex="10">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="HEADER_NARRATION" Caption="Header Narration" Width="300" VisibleIndex="11">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="SUBLEDGER" Caption="Subledger" Width="250" VisibleIndex="12">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DEBIT" Caption="Debit" Width="100" VisibleIndex="13">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CREDIT" Caption="Credit" Width="100" VisibleIndex="14">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Closing_Balance" Caption="Balance" Width="100" VisibleIndex="15">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Closebal_DBCR" Caption="DBCR" Width="50" VisibleIndex="16">
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                <SettingsEditing Mode="EditForm" />
                <SettingsContextMenu Enabled="true" />
                <SettingsBehavior AutoExpandAllGroups="true" />
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsSearchPanel Visible="false" />
                <SettingsPager PageSize="50">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Particulars" SummaryType="Custom" />
                    <dxe:ASPxSummaryItem FieldName="DEBIT" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="CREDIT" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Closing_Balance" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Closebal_DBCR" SummaryType="Custom" />
                </TotalSummary>
                <GroupSummary>
                    <%--<dxe:ASPxSummaryItem FieldName="Particulars" ShowInGroupFooterColumn="Particulars" SummaryType="Custom" Tag="Item_Particulars" />--%>
                    <dxe:ASPxSummaryItem FieldName="DEBIT" ShowInGroupFooterColumn="DEBIT" SummaryType="Custom" Tag="Item_Debit" DisplayFormat="C" />
                    <dxe:ASPxSummaryItem FieldName="CREDIT" ShowInGroupFooterColumn="CREDIT" SummaryType="Custom" Tag="Item_Credit" />
                    <dxe:ASPxSummaryItem FieldName="Closing_Balance" ShowInGroupFooterColumn="Closing_Balance" SummaryType="Custom" Tag="Item_Balance" DisplayFormat="C"  />
                    <dxe:ASPxSummaryItem FieldName="Closebal_DBCR" ShowInGroupFooterColumn="Closebal_DBCR" SummaryType="Custom" Tag="Item_DBCR"   />
                </GroupSummary>
            </dxe:ASPxGridView>         

            <dx:LinqServerModeDataSource ID="lsmdsCashBankReport" runat="server" OnSelecting="ReportSourceDataContext_Selecting"
            ContextTypeName="ReportSourceDataContext" TableName="tbl_Report_CashBankBook" />
        </div>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:SqlDataSource ID="sqlbranch" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
        SelectCommand="select branch_id,branch_description from tbl_master_branch"></asp:SqlDataSource>
</asp:Content>

