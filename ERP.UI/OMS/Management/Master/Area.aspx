<%@ Page Title="Area" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_Area" CodeBehind="Area.aspx.cs" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<%--<%@ Register Assembly="DevExpress.Web.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web" tagprefix="dx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- 
    --%>
    <style>
        .dxgvControl_PlasticBlue a {
            color: #5A83D0;
        }

    </style>

    <script type="text/javascript">
        //function is called on changing country
        //function OnCountryChanged(cmbCountry) {
        //    grid.GetEditor("cou_country").PerformCallback(cmbCountry.GetValue().toString());
        //}
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function LastCall(obj) {
            if (grid.cpErrorMsg) {
                if (grid.cpErrorMsg.trim != "") {
                    jAlert(grid.cpErrorMsg);
                    grid.cpErrorMsg = '';
                    return;
                }
            }
            if (grid.cpDelmsg != null) {
                if (grid.cpDelmsg.trim() != '') {
                    jAlert(grid.cpDelmsg);
                    grid.cpDelmsg = '';

                }
            }


        }
        function DeleteRow(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Area</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Area List</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td>

                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                             <%if (rights.CanAdd)
                                              { %>
                                            <a href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-primary">
                                                <span>Add New</span>
                                            </a>
                                              <% } %>
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                                            <% if (rights.CanExport)
                                               { %>
                                             <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                 <asp:ListItem Value="2">XLS</asp:ListItem>
                                                 <asp:ListItem Value="3">RTF</asp:ListItem>
                                                 <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                             <% } %>
                                        </td>
                                        <td id="Td1">
                                           <%-- <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                           
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <%--OnCommandButtonInitialize="AreaGrid_CommandButtonInitialize"--%>
                <td>
                    <dxe:ASPxGridView ID="AreaGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                        DataSourceID="insertupdate" KeyFieldName="areaid" Width="100%"  OnInitNewRow="AreaGrid_InitNewRow"
                        OnHtmlEditFormCreated="AreaGrid_HtmlEditFormCreated" OnStartRowEditing="AreaGrid_StartRowEditing"
                        OnHtmlRowCreated="AreaGrid_HtmlRowCreated" OnRowValidating="AreaGrid_RowValidating" 
                        OnCustomCallback="AreaGrid_CustomCallback"  OnCommandButtonInitialize="AreaGrid_CommandButtonInitialize" SettingsBehavior-AllowFocusedRow="true"  >
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                            <clientsideevents endcallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="areaid" ReadOnly="True" Visible="False"
                                VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Area Name" FieldName="name" VisibleIndex="0" Width="30%">
                                <EditFormSettings Visible="True" Caption="Area Name" />
                                <PropertiesTextEdit MaxLength="50">
                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                        <RequiredField IsRequired="True" ErrorText="Mandatory" />

                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                           
                            <dxe:GridViewDataComboBoxColumn FieldName="SId" Visible="False" VisibleIndex="1">
                                <PropertiesComboBox DataSourceID="SelectState" ValueType="System.String" ValueField="city_id" TextField="city_name" EnableIncrementalFiltering="True" EnableSynchronization="False">
                                    <%--<ClientSideEvents SelectedIndexChanged="function(s,e){OnCountryChanged(s);}" />--%>
                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                        <RequiredField IsRequired="True" ErrorText="Mandatory" />

                                    </ValidationSettings>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="City Name" />
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn Caption="City Name" FieldName="city" VisibleIndex="1" Width="30%">
                                <EditFormSettings Visible="False" Caption="City Name" />
                                <EditFormCaptionStyle Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                             <%--added by debjyoti 30-11-2016--%>
                            <%--<dxe:GridViewDataTextColumn Caption="Pin Code" FieldName="pin" VisibleIndex="2" Width="30%">
                                <EditFormSettings Visible="True" Caption="Pin Code" />
                                <PropertiesTextEdit MaxLength="10">
                                     </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>--%>
                            <%--End by debjyoti 30-11-2016--%>

                            <dxe:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="false" ShowEditButton="true" ShowClearFilterButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="6%">

                                <%--<EditButton Visible="True">
                            </EditButton>
                            <DeleteButton Visible="True">
                            </DeleteButton>
                            <ClearFilterButton Visible="True"></ClearFilterButton>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <span>Actions</span>
                                </HeaderTemplate>
                                 <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Image-Url="../../../assests/images/Delete.png" Image-ToolTip="Delete">
                                       
                                    </dxe:GridViewCommandColumnCustomButton>

                                </CustomButtons>
                            </dxe:GridViewCommandColumn>

                        </Columns>
                        <%--<SettingsContextMenu Enabled="true"></SettingsContextMenu>--%>
                         <ClientSideEvents CustomButtonClick="function(s, e) {
                             var key = s.GetRowKey(e.visibleIndex);
                             DeleteRow(key);
                            
                            }" />
                        <SettingsCommandButton>

                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                            </EditButton>
                            <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                            </DeleteButton>
                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                            <ClearFilterButton Text="Clear Filter"></ClearFilterButton>
                        </SettingsCommandButton>
                        
                       <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="400px" EditFormColumnCount="1" />
                        <%-- <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>--%>
                        <SettingsText PopupEditFormCaption="Add/Modify Area" ConfirmDelete="Confirm delete?" />
                        <%--<SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 90%">

                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                            <div style="padding: 2px 2px 2px 92px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                </table>
                            </EditForm>

                        </Templates>

                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="insertupdate" runat="server" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_area] WHERE [area_id] = @original_areaid"
            InsertCommand="INSERT INTO [tbl_master_area] ([area_name], [city_id], [CreateDate], [CreateUser] ) VALUES (@name, @SId, getdate(), @CreateUser )"
            OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT a.area_id AS areaid, a.area_name AS name, s.city_name AS city, s.city_id AS SId  FROM tbl_master_area AS a INNER JOIN tbl_master_city AS s ON a.city_id = s.city_id"
            UpdateCommand="UPDATE [tbl_master_area] SET [area_name] = @name, [city_id] = @SId, [CreateDate] = getdate(), [CreateUser] = @CreateUser  WHERE [area_id] = @areaid">

            <UpdateParameters>
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="SId" Type="Int32" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="areaid" Type="Int32" /> 

            </UpdateParameters>
            <InsertParameters>
                <%--<asp:Parameter Name="area_name" Type="String" />
                <asp:Parameter Name="city_id" Type="Decimal" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />--%>

                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="SId" Type="Int32" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" /> 
            </InsertParameters>
            <DeleteParameters>
                <asp:Parameter Name="original_areaid" Type="Int32" />

            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectArea" runat="server" 
            SelectCommand="SELECT a.area_id AS areaid, a.area_name AS name, s.city_name AS city, s.city_id AS SId  FROM tbl_master_area AS a INNER JOIN tbl_master_city AS s ON a.city_id = s.city_id"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectState" runat="server" 
            SelectCommand="SELECT [city_id], [city_name] FROM [tbl_master_city] order by city_name"></asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

</asp:Content>
