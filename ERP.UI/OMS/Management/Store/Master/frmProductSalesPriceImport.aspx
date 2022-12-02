<%@ Page Title="Product Sales Price Import" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false" Inherits="ERP.OMS.Management.Store.Master.management_master_frmProductSalesPriceImport" CodeBehind="frmProductSalesPriceImport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style>
        #ghhhtbl td {
            padding-right: 15px;
        }
    </style>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Product Sales Price Import</h3>
        </div>

    </div>
    <div class="form_main" style="align-items: center;">

        <div class="col-md-3">
                    <label>Choose File</label>
                    <div>
                        <asp:FileUpload ID="uploadProdSalesPrice" runat="server" Width="100%"/>
                    </div>
                </div>

       <div class="col-md-3">
                    <label>&nbsp;</label>
                    <div>
                        <asp:Button ID="BtnSave" runat="server" Text="Import File" CssClass="btn btn-primary" OnClick="BtnSave_Click" />
                        <asp:LinkButton ID="lnlDownloader" runat="server" OnClick="lnlDownloader_Click" CssClass="btn btn-info">Download Format</asp:LinkButton>
                    </div>
                </div>

        <div class="clear"></div>
        <div class="col-md-12">
             <dxe:ASPxGridView runat="server" Width="100%" ID="gridprodSalesPrice"  AutoGenerateColumns="False" ClientInstanceName="grid" OnDataBinding="grid_DataBinding"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"   >
                                                    <SettingsPager Mode="ShowAllRecords" Visible="False" ></SettingsPager>
                                                  <SettingsSearchPanel Visible="True" Delay="5000" />
                                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="Product Code" Caption="Product Code">
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                   <%--     <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Product Name" Caption="Product Name">
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                        </dxe:GridViewDataTextColumn>--%>
                                                       <%-- <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Product Class" Caption="Product Class">
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                        </dxe:GridViewDataTextColumn>--%>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="MRP" Caption="MRP">
                                                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle> 
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Markup(-)%" Caption="Markup(-)%">
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Markup(+)%" Caption="Markup(+)%">
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn  VisibleIndex="6" FieldName="Sale Price" Caption="Sale Price">
                                                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle> 
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn   VisibleIndex="7" FieldName="Min Sale Price" Caption="Min Sale Price">
                                                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle> 
                                                             <PropertiesTextEdit DisplayFormatString="f2">
                                                              </PropertiesTextEdit>
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                    </Columns>
                 <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                                </dxe:ASPxGridView>

        </div>
         <div class="clear"></div>
          <div class="col-md-3">
                    <label>&nbsp;</label>
                    <div>
                        <asp:Button ID="btnUploadRecord" runat="server" Text="Save & Update" CssClass="btn btn-primary" OnClick="btnUploadRecord_Click" />
                        
                    </div>
                </div>
    </div>
</asp:Content>



