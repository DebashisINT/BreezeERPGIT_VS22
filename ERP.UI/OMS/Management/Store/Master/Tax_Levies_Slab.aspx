<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="management_master_Store_sMarkets" Codebehind="Tax_Levies_Slab.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../../../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript">
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
        <tr>
            <td class="EHEADER" style="text-align: center">
                <strong><span style="color: #000099">marketss</span></strong>
            </td>
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
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <dxe:ASPxGridView ID="marketsGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                    DataSourceID="markets" KeyFieldName="sProducts_ID" Width="100%" OnHtmlRowCreated="marketsGrid_HtmlRowCreated"
                    OnHtmlEditFormCreated="marketsGrid_HtmlEditFormCreated" OnCustomCallback="marketsGrid_CustomCallback">
                    <Columns>
                        <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="Markets_ID">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="sProducts_Code" Caption="Products Code">
                            <PropertiesTextEdit Width="300px">
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Markets Code" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                <Paddings PaddingTop="15px" />
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="sProducts_Name" Caption="Products Name">
                            <PropertiesTextEdit Width="300px">
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Products Name" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                <Paddings PaddingTop="15px" />
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="sProducts_Description"
                            Caption="Products Description">
                            <PropertiesTextEdit Width="300px">
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Products Description" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                <Paddings PaddingTop="15px" />
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                         
                         <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="sProducts_TradingLot"
                            Caption="Products Description">
                            <PropertiesTextEdit Width="300px">
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Products Trading Lot" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                <Paddings PaddingTop="15px" />
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                         
                        <dxe:GridViewDataComboBoxColumn Caption="Country" VisibleIndex="1" FieldName="sProducts_Type">
                             <EditItemTemplate>
                                <dxe:ASPxComboBox ID="GroupType" Value='<%#Bind("sProducts_Type") %>' runat="server" ValueType="System.String"
                                     ClientInstanceName="GroupType" Width="250px">
                                    <Items>
                                        <dxe:ListEditItem Text="Raw Material" Value="A" />
                                        <dxe:ListEditItem Text="Work-In-Process" Value="B" />
                                        <dxe:ListEditItem Text="Finished Goods" Value="C" />  
                                    </Items>
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){OnTypeChanged(s);}" />
                                </dxe:ASPxComboBox>
                            </EditItemTemplate>
                        </dxe:GridViewDataComboBoxColumn>
                        
                        
                       <%-- <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="sProducts_Type" Caption="Products Type">
                            <PropertiesTextEdit Width="300px">
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Products Type" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                <Paddings PaddingTop="15px" />
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                        --%>
                        <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="sProducts_GlobalCode" Caption="Products GlobalCode">
                            <PropertiesTextEdit Width="300px">
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Products GlobalCode" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                <Paddings PaddingTop="15px" />
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                   
                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="CreateDate">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="CreateUser">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="LastModifyDate">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="LastModifyUser">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn VisibleIndex="1" ShowDeleteButton="True" ShowEditButton="True">
                            <HeaderStyle HorizontalAlign="Center"/>
                            <HeaderTemplate>
                                <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                  { %>
                                <a href="javascript:void(0);" onclick="grid.AddNewRow()">
                                    <span style="color: #000099;
                                                                        text-decoration: underline">Add New</span>
                                </a>
                                <%} %>
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
                    <SettingsText PopupEditFormCaption="Add/Modify markets" />
                    <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" PopupEditFormHeight="200px"
                        PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True" PopupEditFormVerticalAlign="Above"
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
    <asp:SqlDataSource ID="markets" runat="server" ConflictDetection="CompareAllValues"
       DeleteCommand="DELETE FROM [Master_Markets] WHERE [Markets_ID] = @Markets_ID"
        InsertCommand="INSERT INTO [Master_Markets] ( [Markets_Code],[Markets_Name],[Markets_Description],[Markets_Address]) VALUES (@Markets_Code, @Markets_Name, @Markets_Description, @Markets_Address)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT  [sProducts_ID],[sProducts_Code],[sProducts_Name],[sProducts_Description],[sProducts_Type]
      ,[sProducts_GlobalCode],[sProducts_TradingLot],[sProducts_TradingLotUnit],[sProducts_QuoteCurrency]
      ,[sProducts_QuoteLot],[sProducts_QuoteLotUnit],[sProducts_DeliveryLot],[sProducts_DeliveryLotUnit]
      ,[sProducts_CreateUser],[sProducts_CreateTime],[sProducts_ModifyUser] ,[sProducts_ModifyTime] FROM [dbo].[Master_sProducts]"
        UpdateCommand="UPDATE [Master_Markets]  SET [Markets_Code] = @Markets_Code, Markets_Country=@Markets_Country WHERE [Markets_ID] = @Markets_ID">
        <DeleteParameters>
            <asp:Parameter Name="Markets_ID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Markets_Code" Type="String" />
            <asp:Parameter Name="Markets_ID" Type="Int32" />
            <asp:Parameter Name="Markets_Country" Type="Int32" />
            <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="edu_markets" Type="String" />
            <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
        </InsertParameters>
    </asp:SqlDataSource>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
    <br />
</asp:Content>

