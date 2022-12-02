<%@ Page Title="Defective Product Marking" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DefectiveProductMarking.aspx.cs" Inherits="ERP.OMS.Management.Activities.DefectiveProductMarking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function SearchStock() {
            cOpeningGrid.PerformCallback('BindGrid');
        }

        function SaveStock() {
            cOpeningGrid.PerformCallback('SaveBindGrid');
        }

        function OnDentSelectedChanged(s, e, itemIndex) {
            var obj = cOpeningGrid.GetRowKey(itemIndex);
            var globalChecked = s.GetChecked();
            var productList = globalChecked + '~' + obj;

            if (document.getElementById("hdnDentProduct").value == '') {
                document.getElementById("hdnDentProduct").value = productList;
            }
            else {
                document.getElementById("hdnDentProduct").value = document.getElementById("hdnDentProduct").value + ',' + productList;
            }
        }

        function OnDisplaySelectedChanged(s, e, itemIndex) {
            var obj = cOpeningGrid.GetRowKey(itemIndex);
            var globalChecked = s.GetChecked();
            var productList = globalChecked + '~' + obj;

            if (document.getElementById("hdnDisplayProduct").value == '') {
                document.getElementById("hdnDisplayProduct").value = productList;
            }
            else {
                document.getElementById("hdnDisplayProduct").value = document.getElementById("hdnDisplayProduct").value + ',' + productList;
            }
        }

        function OnStolenSelectedChanged(s, e, itemIndex) {
            var obj = cOpeningGrid.GetRowKey(itemIndex);
            var globalChecked = s.GetChecked();
            var productList = globalChecked + '~' + obj;

            if (document.getElementById("hdnStolenProduct").value == '') {
                document.getElementById("hdnStolenProduct").value = productList;
            }
            else {
                document.getElementById("hdnStolenProduct").value = document.getElementById("hdnStolenProduct").value + ',' + productList;
            }
        }

        function OnEndCallback(s, e) {
            if (cOpeningGrid.cpSaveSuccessOrFail == "successInsert") {
                document.getElementById("hdnDentProduct").value = "";
                document.getElementById("hdnDisplayProduct").value = "";
                document.getElementById("hdnStolenProduct").value = "";
                cOpeningGrid.cpSaveSuccessOrFail = null;

                jAlert('Product Marking successfully.');
            }
            else if (cOpeningGrid.cpSaveSuccessOrFail == "errorInsert") {
                document.getElementById("hdnDentProduct").value = "";
                document.getElementById("hdnDisplayProduct").value = "";
                document.getElementById("hdnStolenProduct").value = "";
                cOpeningGrid.cpSaveSuccessOrFail = null;

                jAlert('try again later.');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Product Marking</h3>
        </div>
    </div>
    <div class="form_main">
        <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix col-md-12">
            <div class="col-md-3">
                <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                </dxe:ASPxLabel>
                <dxe:ASPxComboBox ID="cmbbranch" runat="server" ClientInstanceName="ccmbbranch" TextField="branch_description" ValueField="branch_id" Width="100%">
                </dxe:ASPxComboBox>
            </div>
            <div class="col-md-3">
                <dxe:ASPxLabel ID="lbl_Product" runat="server" Text="Product">
                </dxe:ASPxLabel>
                <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                    KeyFieldName="sProducts_ID" Width="100%" TextFormatString="{0}" MultiTextSeparator=", ">
                    <Columns>
                        <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Code" Width="180">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="Products_Description" Caption="Name" Width="240">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="220">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="140">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                </dxe:ASPxGridLookup>
            </div>
            <div class="col-md-3">
                <div>
                    <br />
                </div>
                <dxe:ASPxButton ID="btnSearch" ClientInstanceName="cbtnSearch" runat="server" Text="Search" AutoPostBack="False" CssClass="btn btn-primary" UseSubmitBehavior="False">
                    <ClientSideEvents Click="function(s, e) {SearchStock();}" />
                </dxe:ASPxButton>
                &nbsp;&nbsp;
                 <% if (rights.CanExport)
                    { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <% } %>
            </div>
        </div>
        <div style="clear: both">
            <br />
        </div>
        <div class="clearfix">
            <dxe:ASPxGridView ID="OpeningGrid" runat="server" KeyFieldName="SerialID" AutoGenerateColumns="False" EnableRowsCache="true"
                Width="100%" ClientInstanceName="cOpeningGrid" OnDataBinding="OpeningGrid_DataBinding" OnCustomCallback="OpeningGrid_CustomCallback"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
                <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowTitlePanel="false" ShowFilterRow="true" ShowFilterRowMenu="true"   />
                <SettingsDataSecurity AllowDelete="False" />
                 <SettingsSearchPanel Visible="True" Delay="5000" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Warehouse" VisibleIndex="0" Width="20%" FieldName="WarehouseName" CellStyle-VerticalAlign="Top">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <%--<dxe:GridViewDataTextColumn Caption="Rate" VisibleIndex="1" Width="5%" FieldName="Rate" CellStyle-Wrap="True">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Stock Qty" VisibleIndex="2" Width="5%" FieldName="IN_Quantity" CellStyle-VerticalAlign="Top">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>--%>

                    <dxe:GridViewDataTextColumn Caption="Doc Type" VisibleIndex="1" Width="10%" FieldName="DocType" CellStyle-Wrap="True">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                  <%--  <dxe:GridViewDataTextColumn Caption="Stock IN Time" VisibleIndex="2" Width="15%" FieldName="Stock_IN_OUT_Date" CellStyle-VerticalAlign="Top">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>--%>

                    <dxe:GridViewDataTextColumn Caption="Batch" VisibleIndex="4" Width="20%" FieldName="BatchNo" CellStyle-VerticalAlign="Top" CellStyle-Wrap="True">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Serial" VisibleIndex="5" Width="20%" FieldName="SerialNo" CellStyle-Wrap="True">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Dent Marking" FieldName="IsDent" VisibleIndex="6" Settings-AllowAutoFilter="False">
                        <DataItemTemplate>
                            <div style="text-align: center">
                                <dxe:ASPxCheckBox ID="chkDent" ClientInstanceName="cchkDent" runat="server" Checked='<%# GetChecked(Eval("IsDent").ToString()) %>' OnInit="chkDent_Init">
                                </dxe:ASPxCheckBox>
                            </div>
                        </DataItemTemplate>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Display Marking" FieldName="IsDisplay" VisibleIndex="6" Settings-AllowAutoFilter="False">
                        <DataItemTemplate>
                            <div style="text-align: center">
                                <dxe:ASPxCheckBox ID="chkDisplay" ClientInstanceName="cchkDisplay" runat="server" Checked='<%# GetChecked(Eval("IsDisplay").ToString()) %>' OnInit="chkDisplay_Init">
                                </dxe:ASPxCheckBox>
                            </div>
                        </DataItemTemplate>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Stolen Marking" FieldName="IsStolen" VisibleIndex="6" Visible="false" Settings-AllowAutoFilter="False">
                        <DataItemTemplate>
                            <div style="text-align: center">
                                <dxe:ASPxCheckBox ID="chkStolen" ClientInstanceName="cchkStolen" runat="server" Checked='<%# GetChecked(Eval("IsStolen").ToString()) %>' OnInit="chkStolen_Init">
                                </dxe:ASPxCheckBox>
                            </div>
                        </DataItemTemplate>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <ClientSideEvents EndCallback="OnEndCallback" />
               
            </dxe:ASPxGridView>
        </div>
        <div style="clear: both">
            <br />
        </div>
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <dxe:ASPxButton ID="btnSave" ClientInstanceName="cbtnSave" runat="server" Text="Save & Exit" AutoPostBack="False" CssClass="btn btn-primary" UseSubmitBehavior="False">
                <ClientSideEvents Click="function(s, e) {SaveStock();}" />
            </dxe:ASPxButton>
            <% } %>
        </div>
    </div>
    <div>
        <asp:SqlDataSource runat="server" ID="ProductDataSource" 
            SelectCommand="prc_CRMSalesInvoice_Details" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Type="String" Name="Action" DefaultValue="AllProductDetails" />
                <asp:Parameter DefaultValue="Y" Name="InventoryType" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <div>
        <asp:HiddenField runat="server" ID="hdnDentProduct" />
        <asp:HiddenField runat="server" ID="hdnDisplayProduct" />
        <asp:HiddenField runat="server" ID="hdnStolenProduct" />
    </div>
    <div style="display: none">
        <dxe:ASPxGridView ID="openingGridExport" runat="server" KeyFieldName="ProductID" AutoGenerateColumns="True"
            Width="100%" EnableRowsCache="true" SettingsBehavior-AllowFocusedRow="true"
            OnDataBinding="openingGridExport_DataBinding">
        </dxe:ASPxGridView>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
