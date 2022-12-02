<%@ Page Title="Companies" Language="C#" AutoEventWireup="True" CodeBehind="root_Companies.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Managemnent.Master.management_master_root_Companies" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<%--<%@ Register Assembly="DevExpress.Web.v9.1.Export, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Cultu7re=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <link type="text/css" href="../../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>

    <script language="javascript" type="text/javascript">
        
        function NewPgae(cnt_id) {
            //alert('cnt_id');
        }
        function OnMoreInfoClick(keyValue) {
            var url = 'rootcompany_general.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Company Details", '940px', '450px', "Y");
            window.location.href = url;
        }

        function OnAddButtonClick() {
            var url = 'rootcompany_general.aspx?id=' + 'ADD';
            //OnMoreInfoClick(url, "Add Company Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback('All');
        }
        //-->
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Companies</h3>

        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                          
                                             <% if (rights.CanAdd)
                                               { %>
                                            <div class="hide">
                                            <a href="javascript:void(0);" onclick="OnAddButtonClick();" class="btn btn-primary"><span>Add New</span></a>
                                                </div>
                                              <%} %>
                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                 <asp:ListItem Value="2">XLS</asp:ListItem>
                                                 <asp:ListItem Value="3">RTF</asp:ListItem>
                                                 <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                              <%} %>
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                                        </td>
                                        <td id="Td1">
                                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <td style="float: right; vertical-align: top">
                                <%--<dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <Border BorderColor="black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2">
                    <dxe:ASPxGridView ID="CompanyGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        DataSourceID="CompaniesDataSource"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" 
 KeyFieldName="cmp_id" Width="100%" OnCellEditorInitialize="CompanyGrid_CellEditorInitialize"
                        OnCustomCallback="CompanyGrid_CustomCallback" SettingsBehavior-AllowFocusedRow="true">
                        <SettingsSearchPanel Visible="True"  Delay="5000"/>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true"/>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="cmp_id" ReadOnly="True" Visible="False"
                                VisibleIndex="0">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="cmp_internalid" Visible="False" VisibleIndex="0">
                           <Settings AllowAutoFilterTextInputTimer="False" />
                                 </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="cmp_parentid" Visible="False" VisibleIndex="0">
                                <PropertiesComboBox DataSourceID="SqlParentComp" ValueField="cmp_internalid" TextField="cmp_Name"
                                    EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                </PropertiesComboBox>
                                <EditFormSettings Caption="Parent Comp" Visible="True" VisibleIndex="0" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="PAN No." FieldName="cmp_panNo" VisibleIndex="3" Width="120px">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" VisibleIndex="5" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
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
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="5" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="100px">
                                <DataItemTemplate>
                                     <% if (rights.CanEdit)
                                               { %>
                                    <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" title="More Info">

                                        <img src="../../../assests/images/info.png" />
                                    </a>  <% } %>
                                </DataItemTemplate>
                                
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>Actions
                                    <%--<a href="javascript:void(0);" onclick="OnAddButtonClick()"><span style="color: #000099; text-decoration: underline">Add New</span> </a>--%>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 25%"></td>
                                        <td style="width: 50%">

                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

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
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu ="true" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="300px" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" />
                        <%--<Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>--%>
                        <SettingsText PopupEditFormCaption="Add/Modify Company Name" ConfirmDelete="Confirm delete?" />
                        <%--<SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
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
        <asp:SqlDataSource ID="CompaniesDataSource" runat="server" InsertCommand="CompanyInsert" InsertCommandType="StoredProcedure" SelectCommand="CompanySelect"
            SelectCommandType="StoredProcedure" DeleteCommand="CompanyDelete" DeleteCommandType="StoredProcedure"
            UpdateCommand="CompanyUpdate" UpdateCommandType="StoredProcedure">
            <DeleteParameters>
                <asp:Parameter Name="cmp_internalid" Type="string" />
            </DeleteParameters>
            <UpdateParameters>
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
            </UpdateParameters>
            <InsertParameters>
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
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlParentComp" runat="server"      SelectCommand="SELECT [cmp_internalid], [cmp_Name] FROM [tbl_master_company]"></asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
