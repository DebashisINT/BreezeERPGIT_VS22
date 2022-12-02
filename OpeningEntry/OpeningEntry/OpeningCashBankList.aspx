<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="OpeningCashBankList.aspx.cs" Inherits="OpeningEntry.OpeningEntry.OpeningCashBankList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        var isFirstTime = true;
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };
            e.returnValue = false;
            e.stopPropagation();
        }
        document.onkeydown = function (e) {
            if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) { //run code for alt+A -- ie, Add New  
                StopDefaultAction(e);
                AddButtonClick();
            }
        }
        function onPrintJv(id) {

            RecPayId = id;
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }
        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }
        function cSelectPanelEndCall(s, e) {

            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'CBVUCHR';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId, '_blank')
            }
            if (cSelectPanel.cpSuccess == "") {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        function AllControlInitilizeList() {
            if (isFirstTime) {

                //if (localStorage.getItem('FromDateCashBank')) {
                //    var fromdatearray = localStorage.getItem('FromDateCashBank').split('-');
                //    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                //    cFormDate.SetDate(fromdate);
                //}

                //if (localStorage.getItem('ToDateCashBank')) {
                //    var todatearray = localStorage.getItem('ToDateCashBank').split('-');
                //    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                //    ctoDate.SetDate(todate);
                //}
                //if (localStorage.getItem('BranchCashBank')) {
                //    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchCashBank'))) {
                //        ccmbBranchfilter.SetValue(localStorage.getItem('BranchCashBank'));
                //    }

                }
                //updateGridByDate();
                isFirstTime = false;
            }
        
        function AddButtonClick() {
            var url = 'CashBankEntry.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue, ValueDate) {
            if (ValueDate != "") {
                jAlert("Voucher is Reconciled.Cannot Edit");
            }
            else {
                var url = 'CashBankEntry.aspx?key=' + keyValue + '&type=CBE';
                window.location.href = url;
            }

        }
        //function OnViewClick(keyValue) {
        //    var url = 'CashBankEntry.aspx?key=' + keyValue + '&req=V' + '&type=CBE';
        //    window.location.href = url;
        //}
        function OnViewClick(keyValue) {
            var url = '/OMS/management/dailytask/CashBankEntry.aspx?key=' + keyValue + '&req=V' + '&type=CBE';
            window.location.href = url;
      
        }
        function GvCBSearch_EndCallBack() {
            if (cGvCBSearch.cpDelete != null) {
                // cGvCBSearch.PerformCallback();
                jAlert(cGvCBSearch.cpDelete);
                cGvCBSearch.cpDelete = null;
                cGvCBSearch.Refresh();
            }
        }
        function OnClickDelete(keyValue, ValueDate) {
            if (ValueDate != "") {
                jAlert("Voucher is Reconciled.Cannot Delete");
            }
            else {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        cGvCBSearch.PerformCallback("Delete~" + keyValue);
                    }
                });
            }
        }

        function updateGridByDate() {

            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else {
                localStorage.setItem("FromDateCashBank", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateCashBank", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BranchCashBank", ccmbBranchfilter.GetValue());
                //cGvCBSearch.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                cGvCBSearch.Refresh();
                $("#drdExport").val(0);

            }
        }
    </script>
    <style>
        .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }

            .padTabtype2 > tbody > tr > td > label {
                margin-bottom: 0 !important;
                margin-right: 15px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Opening Cash/Bank Voucher List"></asp:Label>

            </h3>
        </div>
    </div>
    <div class="form_main rgth pull-left full">
        <div class="clearfix" id="btnAddNew">
            <div style="padding-right: 5px;">
                <span id="divAddButton">
                    <% if (rights.CanAdd)
                       { %>
                    <%--<a id="AddButton" href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a>--%>
                    <% } %>
                </span>
                <span id="divExportto">
                    <% if (rights.CanExport)
                       { %>
                   <%-- <asp:DropDownList ID="drdCashBank" runat="server" CssClass="btn btn-sm btn-primary expad" OnSelectedIndexChanged="drdCashBankExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                        <asp:ListItem Value="4">XLSX</asp:ListItem>
                    </asp:DropDownList>--%>
                    <% } %>

                    <%-- <% if (rights.CanAdd)
                           { %>

                        <a id="AddButton" style="display: none;" href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a>


                        <% } %>
                    </span>
                    <span id="divExportto">
                        <% if (rights.CanExport)
                           { %>

                        <asp:DropDownList ID="drdCashBank" runat="server" CssClass="btn btn-sm btn-primary expad" OnSelectedIndexChanged="drdCashBankExport_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>

                        <% } %>--%>
                </span>
                <table class="padTabtype2 pull-right" id="gridFilter">
                    <tr>
                        <td>
                            <label>From Date</label></td>
                        <td>
                            <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                 DisplayFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td>
                            <label>To Date</label>
                        </td>
                        <td>
                            <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" 
                                ClientInstanceName="ctoDate" Width="100%" UseMaskBehavior="True">
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
                            <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                        </td>

                    </tr>

                </table>
            </div>

        </div>
        <div id="DivEdit">
            <dxe:ASPxGridView ID="GvCBSearch" ClientInstanceName="cGvCBSearch" runat="server" AutoGenerateColumns="False"
                KeyFieldName="CBID" Width="100%" SettingsBehavior-AllowFocusedRow="true"
                OnCustomCallback="GvCBSearch_CustomCallback"
                OnSummaryDisplayText="GvCBSearch_SummaryDisplayText"
                Settings-HorizontalScrollBarMode="Auto"
                DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <SettingsBehavior ColumnResizeMode="NextColumn" />
                <Columns>
                    <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="CBID" Caption="CBID" SortOrder="Descending">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CashBank_TransactionType" Caption="Type" FixedStyle="Left">
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="TransactionDate" FixedStyle="Left"
                        Caption="Posting Date">
                        <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>--%>
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="VoucherNumber" FixedStyle="Left"
                        Caption="Document No." Width="140px">
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="CashBank_Currency" Settings-AllowAutoFilter="False"
                        Caption="Currency" Width="6%">
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="CashBankID" Settings-AllowAutoFilter="True"
                        Caption="Bank/Cash">
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Amount" HeaderStyle-HorizontalAlign="Right"
                        Caption="Amount">
                        <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Total_taxable_amount" HeaderStyle-HorizontalAlign="Right" Settings-AllowAutoFilter="False"
                        Caption="Taxable Amount">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="Total_CGST" Settings-AllowAutoFilter="False" HeaderStyle-HorizontalAlign="Right"
                        Caption="CGST">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Total_SGST" Settings-AllowAutoFilter="False" HeaderStyle-HorizontalAlign="Right"
                        Caption="SGST">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="Total_UTGST" Settings-AllowAutoFilter="False" HeaderStyle-HorizontalAlign="Right"
                        Caption="UTGST">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="Total_IGST" Settings-AllowAutoFilter="False" HeaderStyle-HorizontalAlign="Right"
                        Caption="IGST">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="EnteredBranchName" 
                        Caption="Entered Unit">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="ForBranchName" 
                        Caption="For Unit">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="CashBank_PaidTo" 
                        Caption="Paid To">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="CashBank_CreateUser" 
                        Caption="Entered By">
                        <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="CashBank_CreateDateTime" Settings-AllowAutoFilter="False"
                        Caption="Last Update On">
                        <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="17" FieldName="CashBank_ModifyUser" Settings-AllowAutoFilter="False"
                        Caption="Updated By">
                        <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="18" Width="130px">
                        <DataItemTemplate>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="View">
                                <img src="../../../assests/images/viewIcon.png" /></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                            <%--<a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>','<%#Eval("ValueDate")%>')" class="pad" title="Edit">
                                <img src="../../../assests/images/info.png" /></a>--%>
                            <% } %>
                            <% if (rights.CanDelete)
                               { %>
                            <%--<a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("IBRef")%>','<%#Eval("ValueDate")%>')" class="pad" title="Delete">
                                <img src="../../../assests/images/Delete.png" /></a>--%>
                            <% } %>
                            <% if (rights.CanPrint)
                               { %>
                           <%-- <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="pad" title="print">
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
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="CBID">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="IBRef">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="ExchangeSegmentID">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="MaxLockDate">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="ValueDate">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="BankStatementDate">
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_taxable_amount" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
                </TotalSummary>
                <Settings ShowGroupPanel="True" HorizontalScrollBarMode="Visible" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" ShowFooter="true"
                    ShowGroupFooter="VisibleIfExpanded" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>

                <%-- <SettingsSearchPanel Visible="True" />--%>
                <ClientSideEvents EndCallback="function(s, e) {GvCBSearch_EndCallBack();}" />
            </dxe:ASPxGridView>

        </div>
    </div>
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="v_CashBankList" />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilizeList" />
    </dxe:ASPxGlobalEvents>
    <asp:HiddenField ID="hfIsFilter" runat="server" />
    <asp:HiddenField ID="hfFromDate" runat="server" />
    <asp:HiddenField ID="hfToDate" runat="server" />
    <asp:HiddenField ID="hfBranchID" runat="server" />

     <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
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
    <%--DEBASHIS--%>
</asp:Content>

