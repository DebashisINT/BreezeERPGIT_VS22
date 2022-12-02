<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="phycicalStoctVerification.aspx.cs" Inherits="ERP.OMS.Management.Activities.phycicalStoctVerification" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/SearchMultiPopup.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <%--  <script src="JS/SearchPopup.js"></script>--%>
    <style>
        .pTop13 {
            padding-top: 13px;
        }
        .py-3 {
            padding: 10px 0;
        }

        .padTab > tbody > tr > td {
            padding-right: 15px;
            vertical-align: middle;
        }

            .padTab > tbody > tr > td > label {
                margin-bottom: 0 !important;
            }

            .padTab > tbody > tr > td > .btn {
                margin-top: 0 !important;
            }

            .padTab > tbody > tr > td > input, .padTab > tbody > tr > td > select {
                margin-bottom: 0 !important;
            }

        .table-primary {
            border: 1px solid #ccc;
            border-top: none;
        }

            .table-primary > thead > tr > th {
                background: #214ca2;
                color: #fff;
                border-top: 1px solid #214ca2;
            }

                .table-primary > thead > tr > th:not(:last-child) {
                    border-right: 1px solid #1a3d84;
                }

            .table-primary > tbody > tr > td:not(:last-child) {
                border-right: 1px solid #ccc;
            }

        .borderTopBox {
            border: 1px solid #ccc;
            border-top: 4px solid #214ca2;
            padding: 15px;
        }

        a.dxbButtonSys > span {
            display: none !important;
        }
        .fakeInput {
            background: #eaeaea;
            border: 1px solid #ccc;
            padding: 3px 9px;
            margin-top: 3px;
            min-height: 26px;
        }
    </style>

    
    <script src="JS/phycicalStoctVerification.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Physical Stock Taking</h3>

            <div class="crossBtn" onclick="CrossBtnClose();"><i class="fa fa-times"></i></div>
        </div>
    </div>
    <div class="form_main">
        <table class="padTab resposive_table" style="margin-top: 7px;">
            <tbody>
                <tr>
                    <td>
                        <label>Select product(s) </label>
                    </td>
                    <td style="width: 300px">
                        <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>
                    </td>
                    <td>
                        <label>Class </label>
                    </td>
                    <td style="width: 150px">
                        <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>
                    </td>
                    <td>Brand</td>
                    <td style="width: 150px">
                        <dxe:ASPxButtonEdit ID="txtBrandName" runat="server" ReadOnly="true" ClientInstanceName="ctxtBrandName" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){BrandButnClick();}" KeyDown="function(s,e){Brand_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <dxe:ASPxCheckBox runat="server" ID="ChkAllProduct" ClientInstanceName="cChkAllProduct">
                                        <ClientSideEvents CheckedChanged="function(s, e) { 
                                                        GetCheckBoxValue(s.GetChecked()); 
                                                    }" />
                                    </dxe:ASPxCheckBox>

                                </td>
                                <td>
                                    <label style="margin:3px 0 0 4px">All products </label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <%--<button type="button" value="" class="btn btn-success " onclick="" >Show</button>--%>
                        <asp:Button ID="Button3" runat="server" Text="Show" CssClass="btn btn-success  mLeft mTop" OnClientClick="return PerformCallToGridBind();" UseSubmitBehavior="false" />

                          <% if (rights.CanExport)
                            { %>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                          <% } %>
                    </td>
                </tr>
                
            </tbody>
        </table>
        <div class="clear"></div>
        <div class="borderTopBox clearfix py-3 mTop5">
            <div class="row">
                <div class="col-md-3" id="div_Warehouse">
                    <div style="margin-bottom: 5px;">
                        Warehouse
                    </div>
                    <div class="Left_Content" style="">
                        <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                            TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%">

                            <ClientSideEvents SelectedIndexChanged="CmbWarehouse_ValueChange" GotFocus="CmbWarehouse_GotFocus" />

                            <%-- TextChanged="function(s, e) {CmbWarehouse_ValueChange();}" --%>
                        </dxe:ASPxComboBox>

                    </div>
                </div>
                <div class="col-md-2" id="div_date">
                    <div style="margin-bottom: 5px;">
                       As On Date
                    </div>
                    <div class="Left_Content" style="">
                           <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate" TabIndex="3" Width="100%" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                     </ButtonStyle>
                        <ClientSideEvents GotFocus="function(s,e){tstartdate.ShowDropDown();}" />
                     </dxe:ASPxDateEdit>

                    </div>
                </div>

                <div class="col-md-2" id="div_Quantity">
                    <div>Product Stock Quantity</div>
                    <div class="fakeInput">
                    <dxe:ASPxLabel runat="server" ID="lblProductQty" ClientInstanceName="clblProductQty"></dxe:ASPxLabel>
                    </div>
                </div>
                <div class="col-md-2" id="div_AltQuantity">
                    <div>Product Alt. Quantity</div>
                    <div class="fakeInput"> <dxe:ASPxLabel runat="server" ID="lblAltQuantity" ClientInstanceName="clblAltQuantity"></dxe:ASPxLabel></div>
                </div>
                <div class="col-md-2 mTop5 pTop13" id="div_Resetbtn" style="display:none">
                 <dxe:ASPxButton ID="Resetbtn_SaveRecords" ClientInstanceName="cResetbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Reset" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                    <ClientSideEvents Click="function(s, e) {ResetProduct_ButtonClick();}" />
                </dxe:ASPxButton>
                 </div>

            </div>
            <div class="clear"></div>
            <div class="py-3">
                <div class="">
                    <%--        OnDataBinding="grid_DataBinding"    --%>

                    
                    <dxe:ASPxGridView runat="server"
                        ClientInstanceName="grid" ID="grid" KeyFieldName="sProducts_ID" AutoGenerateColumns="false"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                        OnBatchUpdate="grid_BatchUpdate" 
                        OnCustomCallback="grid_CustomCallback"
                        OnDataBinding="grid_DataBinding"
                        OnCellEditorInitialize="grid_CellEditorInitialize"
                        OnRowInserting="Grid_RowInserting"
                        OnRowUpdating="Grid_RowUpdating"
                        OnRowDeleting="Grid_RowDeleting"
                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="300"  Settings-HorizontalScrollBarMode="Auto">
                        
                        <SettingsPager Visible="true"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="80px" VisibleIndex="0" Caption="#" Visible="false">
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                        <Image Url="/assests/images/crs.png">
                                        </Image>
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                                <%-- <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" Text="New" ForeColor="White">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>--%>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="50px" Settings-AllowAutoFilter="False">
                                <PropertiesTextEdit>
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataButtonEditColumn FieldName="Product" Caption="Product" VisibleIndex="2" Width="200px" ReadOnly="true">
                            </dxe:GridViewDataButtonEditColumn>
                            <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="3" ReadOnly="True" Width="220px">
                                <CellStyle Wrap="True"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Class" Caption="Class" VisibleIndex="4" ReadOnly="true" Width="180px">
                            </dxe:GridViewDataTextColumn>



                            <dxe:GridViewDataTextColumn FieldName="StockUnit" Caption="Stock Unit" VisibleIndex="5" Width="140px" ReadOnly="true">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="AltUnit" Caption="Alt. Unit" VisibleIndex="6" ReadOnly="true" Width="140px">
                            </dxe:GridViewDataTextColumn>
                       

                            
                              <dxe:GridViewDataTextColumn   FieldName="StockUnitQnty" Caption="Stock Unit Qty" VisibleIndex="7" Width="230px" PropertiesTextEdit-MaxLength="15"
                                    HeaderStyle-HorizontalAlign="Right" >
                                <PropertiesTextEdit Style-HorizontalAlign="Right" ClientSideEvents-KeyPress="CheckDecimal">
                                    <ClientSideEvents LostFocus="PopulateMultiUomAltQuantity" />  <%-- GotFocus="StockQuantityGotFocus" --%>
                                   <%-- <MaskSettings AllowMouseWheel="False"   Mask="&lt;-999999999..999999999&gt;.&lt;00..9999&gt;"/>--%>
                                   
                                  <%--  <ValidationSettings CausesValidation="True">  
                                         <RegularExpression ErrorText="Quantity is not a decimal" ValidationExpression="^\d+(\.\d{1,5})?$" />  
                                                 <RequiredField ErrorText="Quantity is required" IsRequired="true" />  
                                    </ValidationSettings> --%>
                                    <%--ClientSideEvents-KeyPress="CheckDecimal"--%>

                                    <Style HorizontalAlign="Right">
                                     </Style>
                                </PropertiesTextEdit>
                                <HeaderStyle HorizontalAlign="Right" />
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn FieldName="AltUnitQnty" Caption="Alt. Unit Qty" VisibleIndex="8" Width="200px" HeaderStyle-HorizontalAlign="Right" PropertiesTextEdit-MaxLength="15">
                                <PropertiesTextEdit Style-HorizontalAlign="Right" ClientSideEvents-KeyPress="CheckDecimal" DisplayFormatString="">
                                    <ClientSideEvents LostFocus="PopulateMultiUomStockQuantity" />  <%-- GotFocus="StockQuantityGotFocus"--%>
                                 <%--   <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..9999&gt;" />--%>
                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                </PropertiesTextEdit>
                                <HeaderStyle HorizontalAlign="Right" />
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                            </dxe:GridViewDataTextColumn>




                            <dxe:GridViewDataTextColumn FieldName="ProductID" PropertiesTextEdit-ValidationSettings-ErrorImage-IconID="ghg" Caption="hidden Field Id" VisibleIndex="9" ReadOnly="True" Width="0" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="abcd">
                                <PropertiesTextEdit Height="15px">
                                    <ValidationSettings>
                                        <ErrorImage IconID="ghg">
                                        </ErrorImage>
                                    </ValidationSettings>
                                    <Style CssClass="abcd">
                                                            </Style>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="True" CssClass="abcd"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="ClassId" Caption="hidden Field Id" VisibleIndex="10" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="BrandId" Caption="hidden Field Id" VisibleIndex="11" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="StockUnitId" Caption="hidden Field Id" VisibleIndex="12" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="AltUnitId" Caption="hidden Field Id" VisibleIndex="13" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="StockId" Caption="hidden Field Id" VisibleIndex="14" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged"/>

                        <SettingsPager NumericButtonCount="5" PageSize="20" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="20,50,100,200,300,400" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsDataSecurity AllowEdit="true" />
                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                        </SettingsEditing>
                        <Settings  VerticalScrollBarMode="Auto"  ShowFilterRow="true" ShowFilterRowMenu="true"/>
                        <SettingsBehavior ColumnResizeMode="Disabled" />
                    </dxe:ASPxGridView>
                    <%--           <dx:linqservermodedatasource id="EntityServerModeDataSource" runat="server" onselecting="EntityServerModeDataSource_Selecting"
                        contexttypename="ERPDataClassesDataContext" tablename="V_AllProductingridList" />--%>
                </div>

                <div>
                    <br />
                </div>
                     <% if (rights.CanAdd)
                          { %>
                    
                <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                    <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                </dxe:ASPxButton>
                 
                    <%} %>
                <dxe:ASPxButton ID="btn_CommitSave" ClientInstanceName="cbtn_CommitSave" runat="server" AutoPostBack="False" Text="Calculate & Commit" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                     <ClientSideEvents Click="function(s, e) {CalCommit_ButtonClick();}" />
                </dxe:ASPxButton>

            </div>

            <div style="display: none">
                <dxe:ASPxGridViewExporter ID="exporter" GridViewID="grid" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
            </div>

        </div>


    </div>

   <%-- //calculate and commit start --%>

    <dxe:ASPxPopupControl ID="Popup_CalculateCommit" runat="server" ClientInstanceName="cPopup_CalculateCommit"
                    Width="1200px" Height="200px" HeaderText="Commit List" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div class="Top clearfix">
                                 <% if (rights.CanExport)
                                     { %>
                                  <asp:DropDownList ID="calCuCommit" runat="server" CssClass="btn btn-primary btn-radius " OnSelectedIndexChanged="CalculateCommitExport_SelectedIndexChanged" AutoPostBack="true">
                                     <asp:ListItem Value="0">Export to</asp:ListItem>
                                     <asp:ListItem Value="1">PDF</asp:ListItem>
                                     <asp:ListItem Value="2">XLS</asp:ListItem>
                                     <asp:ListItem Value="3">RTF</asp:ListItem>
                                     <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                                   <% } %>
                               <div class="clear">
                                </div>
                                
                          <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="sProducts_ID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
            Width="100%" ClientInstanceName="cGrdQuotation" 
            Settings-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" 
            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="200" Settings-VerticalScrollBarMode="Auto">
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>
                  
                 <dxe:GridViewDataTextColumn FieldName="sProducts_ID" Visible="false" SortOrder="Descending" Width="0" VisibleIndex="0">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Product" FieldName="Product" VisibleIndex="1" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn> 
                    
                <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description" VisibleIndex="2" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>       
                <dxe:GridViewDataTextColumn Caption="As On Date" FieldName="Date"
                    VisibleIndex="2" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                   <dxe:GridViewDataTextColumn Caption="Class" FieldName="Class" VisibleIndex="3" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn> 
                          
                   
                

                 <dxe:GridViewDataTextColumn FieldName="StockUnit" Caption="Stock Unit" VisibleIndex="4" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn> 
                
                   <dxe:GridViewDataTextColumn FieldName="AltUnit" Caption="Alt. Unit" VisibleIndex="5" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>        
                            
                  <dxe:GridViewDataTextColumn FieldName="StockUnitQnty" Caption="Stock Unit Qty" VisibleIndex="6" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                            
                <dxe:GridViewDataTextColumn FieldName="AltUnitQnty" Caption="Alt. Unit Qty" VisibleIndex="7" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                
                 <dxe:GridViewDataTextColumn FieldName="WareHouse" Caption="WareHouse" VisibleIndex="8" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn> 


                <dxe:GridViewDataTextColumn Caption="Created By" FieldName="EnteredByName"
                    VisibleIndex="9" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Created On" FieldName="EnteredByDate"
                    VisibleIndex="10" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="ModifiedByName"
                    VisibleIndex="11" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="ModifiedDate"
                    VisibleIndex="12" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
               
            </Columns>
           <%--   <ClientSideEvents EndCallback="OnCommitEndCallback"/>--%>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
           
            <SettingsPager NumericButtonCount="5" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            <GroupSummary>
                <dxe:ASPxSummaryItem FieldName="GrossAmount" SummaryType="Sum" DisplayFormat="Total Gross Amount : {0}" />
                <dxe:ASPxSummaryItem FieldName="ChargesAmount" SummaryType="Sum" DisplayFormat="Total Tax & Charges : {0}" />
                <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" DisplayFormat="Total Net Amount : {0}" />
                <dxe:ASPxSummaryItem FieldName="AmountReceived" SummaryType="Sum" DisplayFormat="Total Amount Received : {0}" />
                <dxe:ASPxSummaryItem FieldName="BalanceAmount" SummaryType="Sum" DisplayFormat="Total Balance Amount : {0}" />
            </GroupSummary>

            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>

        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="V_CalculateStockList" />
        
    </div>
                                <div class="clear">
                                    <br />
                                </div>



                                <div class="col-sm-12 text-right">
                                     <div class="col-sm-10 text-left">
                                         
                                    <label style="color:maroon;"><b>Note:</b> Cumulative Physical Stock updated for Warehouse(s) is showing here . You can proceed with 
                                                 further physical stock updation by using 'Close' and back to Physical Stock entry screen.
                                                 Otherwise, click on 'Commit' to save the data Finally to post for Reconcilation. Then 
                                                 you will not be able to see the data from here, will be available in Reconciliation module.</label>
                                         </div>
                                    <div class="col-sm-2">
                                            <% if (rights.CanAdd)
                                                  { %>
                                    <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_CommitSaveStockCommit" OnClick="StockCommit" runat="server" AutoPostBack="False" Text="Commit" CssClass="btn btn-primary"  UseSubmitBehavior="False">
                                      <ClientSideEvents Click="function(s, e) {
       
                                               e.processOnServer = true; 
                                             }" />
                                     </dxe:ASPxButton>
                                            <%} %>

                                         <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_CommitClose" runat="server" AutoPostBack="False" Text="Close" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                              <ClientSideEvents Click="function(s, e) {CalCommit_CloseClick();}" />
                                     </dxe:ASPxButton>
                                    </div>
                                </div>

                                  <div style="display: none">
                <dxe:ASPxGridViewExporter ID="Calculateexport" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
            </div>
                               
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>

   <%-- end--%>
    <!--Product Modal -->
    <div class="modal fade" id="ProdModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Productkeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Product Name" />
                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
                                <th>Hsn</th>
                                <th class="hide">StockId</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    <asp:HiddenField ID="hdnCalcommitProductId" runat="server" />

    <!--Class Modal -->
    <div class="modal fade" id="ClassModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Class Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Classkeydown(event)" id="txtClassSearch" width="100%" placeholder="Search By Class Name" />
                    <div id="ClassTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Class Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="DeSelectAll('ClassSource')">Deselect All</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="OKPopup('ClassSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnClassId" runat="server" />

    <!--Brand Modal -->
    <div class="modal fade" id="BrandModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Brand Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Brandkeydown(event)" id="txtBrandSearch" width="100%" placeholder="Search By Brand Name" />
                    <div id="BrandTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="DeSelectAll('BrandSource')">Deselect All</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="OKPopup('BrandSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <asp:HiddenField ID="hdnBranndId" runat="server" />

    <asp:HiddenField ID="hddnuomFactor" runat="server" />
     <asp:HiddenField ID="hdnAltUomFactor" runat="server" />
    <asp:HiddenField ID="hdnDeleteSrlNo" runat="server" />
    <asp:HiddenField ID="hdfIsDelete" runat="server" />
    <asp:HiddenField ID="hdnPageStatus" runat="server" />
     <asp:HiddenField ID="hdnProductEntryCheck" runat="server" />

</asp:Content>
