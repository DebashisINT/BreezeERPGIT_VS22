<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_master_root_Companies" Codebehind="root_Companies.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Master/Js/root_Companies.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <table class="TableMain100">
        <tr>
            <td style="text-align: center;">
                <strong><span style="color: #000099">Company Master</span></strong></td>
        </tr>
        <tr>
            <td>
                <div class="SearchArea">
                    <div class="FilterSide">
                        <div style="float: left; padding-right: 5px;">
                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                        </div>
                        <div>
                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                        </div>
                    </div>
                    <div class="ExportSide">
                        <div style="margin-left: 90%;">
                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                ValueType="System.Int32" Width="130px">
                                <Items>
                                    <dxe:ListEditItem Text="Select" Value="0" />
                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                </Items>
                                <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                </ButtonStyle>
                                <ItemStyle BackColor="Navy" ForeColor="White">
                                    <HoverStyle BackColor="#8080FF" ForeColor="White">
                                    </HoverStyle>
                                </ItemStyle>
                                <Border BorderColor="White" />
                                <DropDownButton Text="Export">
                                </DropDownButton>
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                </div>

                <%--    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                Show Filter</span></a>
                                        </td>
                                        <td id="Td1">
                                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                                All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                            <td class="gridcellright">
                                <%if (Session["PageAccess"].ToString().Trim() == "All")
                                  { %>
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                    Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                    </ButtonStyle>
                                    <ItemStyle BackColor="Navy" ForeColor="White">
                                        <HoverStyle BackColor="#8080FF" ForeColor="White">
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="White" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                                <%} %>
                            </td>
                        </tr>
                    </table>--%>
            </td>
        </tr>
        <tr>
            <td>
                <dxe:ASPxGridView ID="CompanyGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                    DataSourceID="CompaniesDataSource" KeyFieldName="cmp_id" Width="100%" OnCellEditorInitialize="CompanyGrid_CellEditorInitialize"
                    OnCustomCallback="CompanyGrid_CustomCallback">
                    <Columns>
                        <dxe:GridViewDataTextColumn FieldName="cmp_id" ReadOnly="True" Visible="False"
                            VisibleIndex="0">
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="cmp_internalid" Visible="False" VisibleIndex="0">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataComboBoxColumn FieldName="cmp_parentid" Visible="False" VisibleIndex="0">
                            <PropertiesComboBox DataSourceID="SqlParentComp" ValueField="cmp_internalid" TextField="cmp_Name"
                                EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                            </PropertiesComboBox>
                            <EditFormSettings Caption="Parent Comp" Visible="True" VisibleIndex="0" />
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataTextColumn Caption="Company Name" FieldName="cmp_Name" VisibleIndex="0"
                            Width="30%">
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <PropertiesTextEdit Width="300px">
                                <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                    <RequiredField ErrorText="Please Enter Company Name" IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True" VisibleIndex="0" />
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="cmp_natureOfBusiness" Visible="False" VisibleIndex="1"
                            Caption="Nature Of Business">
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <PropertiesTextEdit Width="300px">
                            </PropertiesTextEdit>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True" Caption="Nature Of Business" VisibleIndex="1" />
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Directors" FieldName="cmp_directors" VisibleIndex="1"
                            Width="20%">
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <PropertiesTextEdit Width="300px">
                            </PropertiesTextEdit>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True" VisibleIndex="2" />
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Authorized Signatories" FieldName="cmp_authorizedSignatories"
                            VisibleIndex="2" Width="15%">
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <PropertiesTextEdit Width="300px">
                            </PropertiesTextEdit>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="cmp_registrationNo" Visible="False" VisibleIndex="3"
                            Caption="Registration No.">
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <PropertiesTextEdit Width="300px">
                            </PropertiesTextEdit>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True" Caption="Registration No." VisibleIndex="3" />
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="PAN No." FieldName="cmp_panNo" VisibleIndex="3">
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <PropertiesTextEdit Width="300px">
                            </PropertiesTextEdit>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True" VisibleIndex="5" />
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="cmp_serviceTaxNo" Visible="False" VisibleIndex="3"
                            Caption="Service Tax No.">
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <PropertiesTextEdit Width="300px">
                            </PropertiesTextEdit>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True" Caption="Service Tax No." VisibleIndex="7" />
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="cmp_salesTaxNo" Visible="False" VisibleIndex="3"
                            Caption="Sales Tax No.">
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <PropertiesTextEdit Width="300px">
                            </PropertiesTextEdit>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True" Caption="Sales Tax No." VisibleIndex="8" />
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataDateColumn Visible="False" VisibleIndex="6" Caption="Date of Incorporation:"
                            FieldName="cmp_DateIncorporation">
                            <EditCellStyle Wrap="False" HorizontalAlign="Left">
                            </EditCellStyle>
                            <PropertiesDateEdit DisplayFormatString="" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                Width="300px" UseMaskBehavior="True">
                            </PropertiesDateEdit>
                            <EditFormSettings Caption="Date of Incorporation:" Visible="True" VisibleIndex="11" />
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataDateColumn>
                        <dxe:GridViewDataTextColumn FieldName="cmp_CIN" Caption="CIN" VisibleIndex="4">
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <PropertiesTextEdit Width="300px">
                            </PropertiesTextEdit>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True" VisibleIndex="9" />
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataDateColumn Caption="CIN Validity Date" FieldName="cmp_CINdt" Visible="False"
                            VisibleIndex="5">
                            <PropertiesDateEdit DisplayFormatString="" Width="300px" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                UseMaskBehavior="True">
                            </PropertiesDateEdit>
                            <EditFormSettings Visible="True" VisibleIndex="10" />
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataDateColumn>
                        <dxe:GridViewDataDateColumn Caption="Registration No Validity Date" FieldName="cmp_VregisNo"
                            Visible="False" VisibleIndex="5">
                            <PropertiesDateEdit DisplayFormatString="" Width="300px" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                UseMaskBehavior="True">
                            </PropertiesDateEdit>
                            <EditFormSettings Visible="True" VisibleIndex="4" />
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataDateColumn>
                        <dxe:GridViewDataDateColumn Caption="PanNo Validity Date" FieldName="cmp_VPanNo"
                            Visible="False" VisibleIndex="5">
                            <PropertiesDateEdit DisplayFormatString="" Width="300px" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                UseMaskBehavior="True">
                            </PropertiesDateEdit>
                            <EditFormSettings Visible="True" VisibleIndex="6" />
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataDateColumn>
                        <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="5">
                            <DataItemTemplate>
                                <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">More Info...</a>
                            </DataItemTemplate>
                            <EditFormSettings Visible="False" />
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <a href="javascript:void(0);" onclick="OnAddButtonClick()"><span style="text-decoration: underline">Add New</span> </a>
                            </HeaderTemplate>
                        </dxe:GridViewDataTextColumn>

                    </Columns>
                    <Templates>
                        <EditForm>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 25%"></td>
                                    <td style="width: 50%">
                                        <controls>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            </controls>
                                        <div style="text-align: right; padding: 2px 2px 2px 2px">
                                            <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                        </div>
                                    </td>
                                    <td style="width: 25%"></td>
                                </tr>
                            </table>
                        </EditForm>
                    </Templates>
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="300px" PopupEditFormHorizontalAlign="Center"
                        PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" />
                    <Styles>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                    </Styles>
                    <SettingsText PopupEditFormCaption="Add/Modify Company Name" ConfirmDelete="Confirm delete?" />
                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    <StylesEditors>
                        <ProgressBar Height="25px">
                        </ProgressBar>
                    </StylesEditors>
                </dxe:ASPxGridView>
            </td>
        </tr>
    </table>
    <%--========================================================== Master Grid DataSource =================================================--%>
    <asp:SqlDataSource ID="CompaniesDataSource" runat="server" 
        InsertCommand="CompanyInsert" InsertCommandType="StoredProcedure" SelectCommand="CompanySelect"
        SelectCommandType="StoredProcedure" DeleteCommand="CompanyDelete" DeleteCommandType="StoredProcedure"
        UpdateCommand="CompanyUpdate" UpdateCommandType="StoredProcedure">
        <deleteparameters>
                <asp:Parameter Name="cmp_internalid" Type="string" />
            </deleteparameters>
        <updateparameters>
                <asp:Parameter Name="cmp_internalid" Type="String" />
                <asp:Parameter Name="cmp_Name" Type="String" />
                <asp:Parameter Name="cmp_natureOfBusiness" Type="String" />
                <asp:Parameter Name="cmp_directors" Type="String" />
                <asp:Parameter Name="cmp_authorizedSignatories" Type="String" />
                <asp:Parameter Name="cmp_registrationNo" Type="String" />
                <asp:Parameter Name="cmp_panNo" Type="String" />
                <asp:Parameter Name="cmp_serviceTaxNo" Type="String" />
                <asp:Parameter Name="cmp_salesTaxNo" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
                <asp:Parameter Name="cmp_CIN" Type="String" />
                <asp:Parameter Name="cmp_DateIncorporation" Type="datetime" />
                <asp:Parameter Name="cmp_CINdt" Type="datetime" />
                <asp:Parameter Name="cmp_VregisNo" Type="datetime" />
                <asp:Parameter Name="cmp_VPanNo" Type="datetime" />
                <asp:Parameter Name="cmp_parentid" Type="string" />
            </updateparameters>
        <insertparameters>
                <asp:Parameter Name="cmp_Name" Type="String" />
                <asp:Parameter Name="cmp_natureOfBusiness" Type="String" />
                <asp:Parameter Name="cmp_directors" Type="String" />
                <asp:Parameter Name="cmp_authorizedSignatories" Type="String" />
                <asp:Parameter Name="cmp_registrationNo" Type="String" />
                <asp:Parameter Name="cmp_panNo" Type="String" />
                <asp:Parameter Name="cmp_serviceTaxNo" Type="String" />
                <asp:Parameter Name="cmp_salesTaxNo" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
                <asp:Parameter Name="cmp_CIN" Type="String" />
                <asp:Parameter Name="cmp_DateIncorporation" Type="datetime" />
                <asp:Parameter Name="cmp_CINdt" Type="datetime" />
                <asp:Parameter Name="cmp_VregisNo" Type="datetime" />
                <asp:Parameter Name="cmp_VPanNo" Type="datetime" />
                <asp:Parameter Name="cmp_parentid" Type="string" />
            </insertparameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlParentComp" runat="server" 
        SelectCommand="SELECT [cmp_internalid], [cmp_Name] FROM [tbl_master_company]"></asp:SqlDataSource>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
</asp:Content>
