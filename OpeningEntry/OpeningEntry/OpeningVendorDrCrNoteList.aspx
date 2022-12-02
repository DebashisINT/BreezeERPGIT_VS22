<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="OpeningVendorDrCrNoteList.aspx.cs" Inherits="OpeningEntry.OpeningEntry.OpeningVendorDrCrNoteList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }
        .padTabtype2 > tbody > tr > td > label {
            margin-bottom: 0 !important;
            margin-right: 15px;
        }
    </style>
    <script>
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
        function AddButtonClick() {
            var url = 'VendorDrCrNoteAdd.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        function OnViewClick(keyValue) {
            var url = '/OMS/Management/Activities/VendorDrCrNoteAdd.aspx?key=' + keyValue + '&req=V' + '&type=VN';
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue) {
            var url = 'VendorDrCrNoteAdd.aspx?key=' + keyValue + '&type=VN';
            window.location.href = url;
        }
        function OnClickDelete(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGvJvSearch.PerformCallback("PCB_DeleteBtnOkE~" + keyValue);
                }
            });

        }

        function GvJvSearch_EndCallBack() {
            if (cGvJvSearch.cpJVDelete != undefined && cGvJvSearch.cpJVDelete != null) {
                jAlert(cGvJvSearch.cpJVDelete);
                cGvJvSearch.cpJVDelete = null;
                updateGridByDate();
                //cGvJvSearch.PerformCallback('PCB_BindAfterDelete');
            }
        }
        function onPrintJv(id, RowIndex) {
            DCNoteID = id;
            cDocumentsPopup.Show();
            $('#HdCrDrNoteType').val(cGvJvSearch.GetRow(RowIndex).children[0].innerText);
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
            if (cSelectPanel.cpSuccess != null) {
                var reportName = cCmbDesignName.GetValue();
                var module = 'VENDDRCRNOTE';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + DCNoteID, '_blank')
            }
            cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == null) {
                cCmbDesignName.SetSelectedIndex(0);
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
                localStorage.setItem("FromDateVendorCrDrNote", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateVendorCrDrNote", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BranchVendorCrDrNote", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                cGvJvSearch.Refresh();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="clearfix pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Vendor Credit/Debit Note"></asp:Label>
            </h3>
        </div>
    </div>
    <div class="form_main rgth pull-left full">

        <div class="clearfix">
            <div style="padding-right: 5px;">
                <span id="divAddButton">
                     <% if (rights.CanAdd)
                           { %>
                  <%--  <a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a>--%>
                     <% } %>
                </span>
                <span id="divExportto">
                    <% if (rights.CanExport)
                           { %>

                   <%-- <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>--%>
                       <% } %>
                </span>

                <table class="padTabtype2 pull-right" id="gridFilter">
                    <tr>
                        <td>
                            <label>From Date</label></td>
                        <td>
                            <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td>
                            <label>To Date</label>
                        </td>
                        <td>
                            <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
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
        <div class="clearfix">

            <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                ClientInstanceName="cGvJvSearch" KeyFieldName="DCNote_ID" Width="100%" Settings-HorizontalScrollBarMode="Auto"
                OnCustomCallback="GvJvSearch_CustomCallback" OnCustomButtonInitialize="GvJvSearch_CustomButtonInitialize"
                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <ClientSideEvents EndCallback="function(s, e) {GvJvSearch_EndCallBack();}" />
                <SettingsBehavior ConfirmDelete="True" />
                <Styles>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                    <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                    <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                    <Footer CssClass="gridfooter"></Footer>
                </Styles>

                <Columns>

                    <dxe:GridViewDataComboBoxColumn Caption="Type" FieldName="NoteType" VisibleIndex="0">
                        <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                            ValueType="System.String" DataSourceID="SqlDataSourceapplicable" TextField="TypeName" ValueField="TypeID">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataComboBoxColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="NoteDate" Caption="Document Date">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NoteNumber" Caption="Document Number" Width="150px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Currency" Caption="Currency">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="BranchName" Caption="Unit" Width="200px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="VendorName" Caption="Vendor Name" Width="200px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="Amount" Caption="Net Amount" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Total_CGST" Caption="CGST" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Total_SGST" Caption="SGST" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="Total_UTGST" Caption="UTGST" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Total_IGST" Caption="IGST" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Total_taxable_amount" Caption="Taxable Amount" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="EnteredBy" Caption="Entered On">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="UpdateOn" Caption="Last Update On" Width="130px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="UpdatedBy" Caption="Updated By">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="DCNote_ID" SortOrder="Descending"></dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="18" Width="130px">
                        <DataItemTemplate>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="View">
                                <img src="../../../assests/images/viewIcon.png" /></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                           <%-- <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="pad" title="Edit">
                                <img src="../../../assests/images/info.png" /></a>--%>
                            <% } %>
                            <% if (rights.CanDelete)
                               { %>
                           <%-- <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete">
                                <img src="../../../assests/images/Delete.png" /></a>--%>
                            <% } %>
                            <% if (rights.CanPrint)
                               { %>
                          <%--  <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="pad" title="print">
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
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" ShowFooter="true" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_taxable_amount" SummaryType="Sum" />
                </TotalSummary>
            </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="V_VendorDrCrNoteDetailsList" />
            <asp:SqlDataSource ID="SqlDataSourceapplicable" runat="server" 
                SelectCommand="(SELECT 'Debit Note' as TypeID,'Debit Note' as TypeName) Union (SELECT 'Credit Note' as TypeID,'Credit Note' as TypeName)"></asp:SqlDataSource>
            <asp:HiddenField ID="hfIsFilter" runat="server" />
            <asp:HiddenField ID="hfFromDate" runat="server" />
            <asp:HiddenField ID="hfToDate" runat="server" />
            <asp:HiddenField ID="hfBranchID" runat="server" />
        </div>

    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

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
                                <asp:HiddenField ID="HdCrDrNoteType" runat="server" />
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--DEBASHIS--%>
</asp:Content>
