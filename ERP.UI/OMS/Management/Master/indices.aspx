<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_indices" Codebehind="indices.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
    function ShowHideFilter(obj)
    {
        grid.PerformCallback(obj);
    }  
    function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
        if(document.body.scrollHeight>=500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '500px';
        window.frameElement.Width = document.body.scrollWidth;
    }
</script>

 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
<table class="TableMain100">
    <tr>
            <td class="EHEADER" style="text-align:center;">
                <strong><span style="color: #000099">Indices</span></strong></td>
        </tr>
    <tr>
        <td>
            
            <table width="100%">
              <tr>
                 <td style="text-align:left; vertical-align:top">
                        <table>
                            <tr>
                                <td id="ShowFilter">
                                    <a  href="javascript:ShowHideFilter('s');" ><span style="color: #000099; text-decoration: underline">
                                        Show Filter</span></a>
                                </td>
                               <td id="Td1">
                                    <a  href="javascript:ShowHideFilter('All');" ><span style="color: #000099; text-decoration: underline">
                                        All Records</span></a>
                                </td>
                            </tr>
                        </table>
                  </td>
                  <td>
                  </td>
                  <td class="gridcellright">
                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy" Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"  ValueType="System.Int32" Width="130px">
                        <Items>
                            <dxe:ListEditItem Text="Select" Value="0"/>
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
<dxe:aspxgridview id="IndicesGrid" ClientInstanceName="grid" runat="server" autogeneratecolumns="False"
        datasourceid="IndicesMaster" keyfieldname="bench_Id"   Width="100%" OnHtmlEditFormCreated="IndicesGrid_HtmlEditFormCreated" OnHtmlRowCreated="IndicesGrid_HtmlRowCreated" OnCustomCallback="IndicesGrid_CustomCallback"><Columns>
<dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="0" FieldName="bench_Id" Visible="False">
<EditFormSettings Visible="False"></EditFormSettings>
</dxe:GridViewDataTextColumn>
<dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="bench_Index" Caption="Indices" Width="80%">
    <PropertiesTextEdit Width="300px">
        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
            <RequiredField ErrorText="Please Enter Contact Type" IsRequired="True" />
        </ValidationSettings>
    </PropertiesTextEdit>
    <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
    </EditFormCaptionStyle>
    <EditFormSettings Caption="Contact Type" Visible="True" />
    <EditCellStyle>
        <Paddings PaddingTop="15px" />
    </EditCellStyle>
</dxe:GridViewDataTextColumn>
<dxe:GridViewCommandColumn VisibleIndex="1" ShowDeleteButton="True" ShowEditButton="True">
    <HeaderStyle HorizontalAlign="Center"/>
    <HeaderTemplate>
        <a href="javascript:void(0);" onclick="grid.AddNewRow()">
            <span style="color: #000099; text-decoration: underline">Add New</span>
        </a>
    </HeaderTemplate>
</dxe:GridViewCommandColumn>
</Columns>
        <Styles>
            <LoadingPanel ImageSpacing="10px">
            </LoadingPanel>
            <Header ImageSpacing="5px" SortingImageSpacing="5px">
            </Header>
            <Cell CssClass="gridcellleft">
            </Cell>
        </Styles>
        <Settings ShowStatusBar="Visible" ShowGroupPanel="True" />
        <SettingsText PopupEditFormCaption="Add/Modify Indices" />
        <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
            <FirstPageButton Visible="True">
            </FirstPageButton>
            <LastPageButton Visible="True">
            </LastPageButton>
        </SettingsPager>
        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
        <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px" />
     <Templates>
            <EditForm>
                    <table style="width:100%">
                      <tr><td style="width:25%"></td>
                          <td style="width:50%">
                            <controls>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            </controls>
                            <div style="text-align:right; padding:2px 2px 2px 2px">
                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                            </div>
                         </td>
                         <td style="width:25%"></td>
                     </tr>
                   </table>
                </EditForm>
            </Templates>
   </dxe:aspxgridview>
        </td>
    </tr>
  </table>
    <asp:SqlDataSource ID="IndicesMaster" runat="server" ConflictDetection="CompareAllValues"
      
        DeleteCommand="DELETE FROM [tbl_master_indices] WHERE [bench_Id] = @original_bench_Id"
        InsertCommand="INSERT INTO [tbl_master_indices] ([bench_Index],[CreateDate],[CreateUser]) VALUES (@bench_Index,getdate(),@CreateUser)"
        OldValuesParameterFormatString="original_{0}" 
        SelectCommand="SELECT [bench_Id], [bench_Index] FROM [tbl_master_indices]"
        UpdateCommand="UPDATE [tbl_master_indices] SET [bench_Index] = @bench_Index,[LastModifyDate]=getdate(),[LastModifyUser]=@CreateUser WHERE [bench_Id] = @original_bench_Id">
        <DeleteParameters>
            <asp:Parameter Name="original_bench_Id" Type="Decimal" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="bench_Index" Type="String" />
             <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" /> 
        </UpdateParameters>
        <InsertParameters>
             <asp:Parameter Name="bench_Index" Type="String" />
             <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" /> 
        </InsertParameters>
    </asp:SqlDataSource>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
