<%@ Page Title="Education" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_Education" CodeBehind="Education.aspx.cs" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
       
    </style>
    <script type="text/javascript">
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function EndCall(obj) {
            if (grid.cpDelmsg != null)
                alert(grid.cpDelmsg);
        }
      
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Education</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--<tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #000099">Educations</span></strong></td>
            </tr>--%>
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
                                            <a href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-primary"><span>Add New</span> </a>
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
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                                        </td>
                                        <td id="Td1">
                                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <%--<td></td>
                            <td class="gridcellright pull-right">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <ButtonStyle>
                                    </ButtonStyle>
                                    <ItemStyle>
                                        <HoverStyle>
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="EducationGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        DataSourceID="education"  KeyFieldName="edu_id" Width="100%" OnHtmlRowCreated="EducationGrid_HtmlRowCreated"
                        OnHtmlEditFormCreated="EducationGrid_HtmlEditFormCreated" OnCustomCallback="EducationGrid_CustomCallback" OnRowDeleting="EducationGrid_RowDeleting" 
                        OnCommandButtonInitialize="EducationGrid_CommandButtonInitialize">
                         <SettingsSearchPanel Visible="True" Delay="5000" />
                        <clientsideevents endcallback="function(s, e) {
                          
	  EndCall(s.cpEND);
}" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="edu_id">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="edu_education" Width="80%"
                                Caption="Education">
                                <PropertiesTextEdit Width="300px" MaxLength="50">
                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right">
                                        <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                    <Paddings PaddingTop="15px" />
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="2" FieldName="CreateDate">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="CreateUser">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="4" FieldName="LastModifyDate">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                              
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="5" FieldName="LastModifyUser">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="6" ShowEditButton="true" ShowDeleteButton="true" Width="6%">
                                <%-- <DeleteButton Visible="True">
                                </DeleteButton>
                               <EditButton Visible="True">
                                </EditButton>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                    <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                    <a href="javascript:void(0);" onclick="grid.AddNewRow()"><span>Add New</span> </a>
                                    <%} %>--%>
                                </HeaderTemplate>
                             
                            </dxe:GridViewCommandColumn>
                        </Columns>
                        <%--<SettingsContextMenu Enabled="true"></SettingsContextMenu>--%>
                        <SettingsCommandButton>
                          
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
<Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

<Styles>
<Style CssClass="pad"></Style>
</Styles>
                            </EditButton>
                              <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image"  Image-AlternateText="Delete">
<Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                            </DeleteButton>
                            
                       <%--      <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" title="Delete">
                                        <img src="/assests/images/Delete.png" /></a>--%>

                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs">
<Styles>
<Style CssClass="btn btn-primary btn-xs"></Style>
</Styles>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs">
<Styles>
<Style CssClass="btn btn-danger btn-xs"></Style>
</Styles>
                            </CancelButton>
                        </SettingsCommandButton>
                        
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true"></Settings>
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <SettingsText PopupEditFormCaption="Add/Modify Education" />
                        <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" 
                            PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter"
                            PopupEditFormWidth="500px" />
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
                                            <div style="text-align: left; padding: 2px 2px 2px 85px">
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
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="education" runat="server" ConflictDetection="CompareAllValues"
         DeleteCommand="DELETE FROM [tbl_master_education] WHERE [edu_id] = @original_edu_id"
            InsertCommand="INSERT INTO [tbl_master_education] ([edu_education], [CreateDate], [CreateUser]) VALUES (@edu_education, getdate(), @CreateUser)"
            OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [edu_id],[edu_education] FROM [tbl_master_education]"
            UpdateCommand="UPDATE [tbl_master_education] SET [edu_education] = @edu_education,[LastModifyDate]=getdate(),[LastModifyUser]=@CreateUser WHERE [edu_id] = @original_edu_id">
           
             <DeleteParameters>
                <asp:Parameter Name="original_edu_id" Type="Decimal" />
            </DeleteParameters>

            <UpdateParameters>
                <asp:Parameter Name="edu_education" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="edu_education" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </InsertParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
        <br />
    </div>
</asp:Content>

