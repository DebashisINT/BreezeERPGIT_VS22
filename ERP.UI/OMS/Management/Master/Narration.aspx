<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_Master_Narration" Codebehind="Narration.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
        <script type="text/javascript">
        function ShowHideFilter(obj)
        {
            grid.PerformCallback(obj);
        }  
    function height()
    {
        if(document.body.scrollHeight>=500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '500px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    function SignOff()
    {
        window.parent.SignOff()
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #000099">Narration List</span></strong></td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
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
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="GrdNarration" runat="server" AutoGenerateColumns="False" DataSourceID="Narration"
                        KeyFieldName="Narration_id" ClientInstanceName="grid" Width="100%" OnCustomCallback="GrdNarration_CustomCallback">
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="Narration_id">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataMemoColumn Caption="Narration" FieldName="Narration_Description" Width="80%"
                                VisibleIndex="0">
                                <PropertiesMemoEdit Width="390px" Height="70px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter Designation" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesMemoEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                    <Paddings PaddingTop="15px" />
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataMemoColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="1" ShowDeleteButton="True" ShowEditButton="True">
                                <HeaderTemplate>
                                    <a href="javascript:void(0);" onclick="grid.AddNewRow();">
                                        <span style="color: #000099;
                                                                                text-decoration: underline">Add New</span>
                                    </a>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>
                        </Columns>
                        <Settings ShowStatusBar="Visible"></Settings>
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <SettingsText PopupEditFormCaption="Add/Modify Narration" />
                        <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" PopupEditFormHeight="200px"
                            PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True" PopupEditFormVerticalAlign="TopSides"
                            PopupEditFormWidth="600px" />
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 25%">
                                        </td>
                                        <td style="width: 50%">
                                            <controls>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            </controls>
                                            <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server">
                                                </dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server">
                                                </dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                        <td style="width: 25%">
                                        </td>
                                    </tr>
                                </table>
                            </EditForm>
                        </Templates>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="Narration" runat="server" ConflictDetection="CompareAllValues"
             DeleteCommand="DELETE FROM [Master_Narration] WHERE [Narration_id] = @original_Narration_id"
            InsertCommand="INSERT INTO [Master_Narration] ([Narration_Description], [Narration_CreateDateTime], [Narration_CreateUser]) VALUES (@Narration_Description, getdate(), @CreateUser)"
            OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Narration_id],[Narration_Description] FROM [Master_Narration]"
            UpdateCommand="UPDATE [Master_Narration] SET [Narration_Description] = @Narration_Description,[Narration_ModifyDateTime]=getdate(),[Narration_ModifyUser]=@CreateUser WHERE [Narration_id] = @original_Narration_id">
            <DeleteParameters>
                <asp:Parameter Name="original_Narration_id" Type="Decimal" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="Narration_Description" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="Narration_Description" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </InsertParameters>
        </asp:SqlDataSource>
</asp:Content>
