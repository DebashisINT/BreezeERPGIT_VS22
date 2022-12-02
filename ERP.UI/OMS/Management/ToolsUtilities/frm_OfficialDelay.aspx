<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frm_OfficialDelay" CodeBehind="frm_OfficialDelay.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    <%--    <script type="text/javascript">
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
   </script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Leave Application</h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <a id="btnAddModify" href="javascript:void(0);" onclick="GridOD.AddNewRow()" title="Add New" class="btn btn-primary"><span>Add New</span></a>
                            </td>
                            <td id="Td1">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="GridOD" ClientInstanceName="GridOD" runat="server" AutoGenerateColumns="False"
                        DataSourceID="DataOD" KeyFieldName="od_id" Width="100%" OnCellEditorInitialize="GridOD_CellEditorInitialize">
                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True"></SettingsBehavior>
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <SettingsPager PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="od_id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn Caption="Employee Name" FieldName="od_cntId" VisibleIndex="0">
                                <PropertiesComboBox DataSourceID="AllEmployee" EnableIncrementalFiltering="True"
                                    TextField="Name" ValueField="ID" ValueType="System.String">
                                </PropertiesComboBox>
                                <EditFormSettings Caption="Employee Name" Visible="True" />
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn Caption="Type" FieldName="od_type" VisibleIndex="1">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	    var value = s.GetValue();
    	
        if(value == &quot;OV&quot;)
        {
            GridOD.GetEditor(&quot;od_reportTime&quot;).SetClientVisible(false);
        }
        else
        {
             GridOD.GetEditor(&quot;od_reportTime&quot;).SetClientVisible(true);
        } 
    }" />
                                    <Items>
                                        <dxe:ListEditItem Text="OD" Value="OD"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="OV" Value="OV"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Caption="Type" Visible="True" />
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn FieldName="od_from1" Caption="From" ReadOnly="True"
                                VisibleIndex="2">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="od_to1" Caption="To" ReadOnly="True" VisibleIndex="3">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn FieldName="od_From" VisibleIndex="2" Caption="Date From"
                                Visible="false">
                                <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy">
                                </PropertiesDateEdit>
                                <EditFormSettings Caption="Date From" Visible="True" />
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataDateColumn FieldName="od_To" VisibleIndex="3" Caption="Date To"
                                Visible="false">
                                <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy">
                                </PropertiesDateEdit>
                                <EditFormSettings Caption="Date to" Visible="True" />
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn FieldName="od_reportTime" VisibleIndex="4" Caption="Reporting Time">
                                <PropertiesTextEdit Width="80px">
                                    <MaskSettings Mask="hh:mm tt" />
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="Reporting Time" Visible="True" />
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataMemoColumn FieldName="od_reason" Visible="False" VisibleIndex="5">
                                <PropertiesMemoEdit Columns="4" Height="100px">
                                </PropertiesMemoEdit>
                                <EditFormSettings Caption="Reason" Visible="True" />
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataMemoColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="5" Width="6%" ShowEditButton="true" ShowClearFilterButton="true" ShowDeleteButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">



                                <HeaderTemplate>
                                    <span>Actions</span>
                                    <%-- <a id="btnAddModify" href="javascript:void(0);" onclick="GridOD.AddNewRow()"><span
                                        style="color: #000099; text-decoration: underline">Add New</span></a>--%>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>
                        </Columns>
                        <SettingsCommandButton>
                            <EditButton Text="Edit" ButtonType="Image" Image-Url="/assests/images/Edit.png" Image-AlternateText="Edit" Styles-Style-CssClass="pad"></EditButton>
                            <DeleteButton Text="Delete" ButtonType="Image" Image-Url="/assests/images/Edit.png" Image-AlternateText="Delete"></DeleteButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                            <ClearFilterButton Text="ClearFilter"></ClearFilterButton>
                        </SettingsCommandButton>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true"></Settings>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px" />
                        <SettingsText ConfirmDelete="Are You Sure want to Delete This OD?" />
                    </dxe:ASPxGridView>
                    <asp:SqlDataSource ID="DataOD" runat="server" ConflictDetection="CompareAllValues"
                        DeleteCommand="DELETE FROM [tbl_trans_OfficialDelay] WHERE [od_id] = @original_od_id "
                        InsertCommand="INSERT INTO [tbl_trans_OfficialDelay] ([od_cntId], [od_type], [od_From], [od_To], [od_reportTime], [od_reason],[CreateUser],[CreateDate]) VALUES (@od_cntId, @od_type, @od_From, @od_To, @od_reportTime, @od_reason,@userid,getdate())"
                        OldValuesParameterFormatString="original_{0}" SelectCommand="" UpdateCommand="UPDATE [tbl_trans_OfficialDelay] SET [od_cntId] = @od_cntId, [od_type] = @od_type, [od_From] = @od_From, [od_To] = @od_To, [od_reportTime] = @od_reportTime, [od_reason] = @od_reason, [lastmodifyUser]=@userid,[lastModifyDate]=getdate() WHERE [od_id] = @original_od_id ">
                        <DeleteParameters>
                            <asp:Parameter Name="original_od_id" Type="Int32" />
                            <asp:Parameter Name="original_od_cntId" Type="String" />
                            <asp:Parameter Name="original_od_type" Type="String" />
                            <asp:Parameter Name="original_od_From" Type="DateTime" />
                            <asp:Parameter Name="original_od_To" Type="DateTime" />
                            <asp:Parameter Name="original_od_reportTime" Type="String" />
                            <asp:Parameter Name="original_od_reason" Type="String" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="od_cntId" Type="String" />
                            <asp:Parameter Name="od_type" Type="String" />
                            <asp:Parameter Name="od_From" Type="DateTime" />
                            <asp:Parameter Name="od_To" Type="DateTime" />
                            <asp:Parameter Name="od_reportTime" Type="String" />
                            <asp:Parameter Name="od_reason" Type="String" />
                            <asp:Parameter Name="original_od_id" Type="Int32" />
                            <asp:Parameter Name="original_od_cntId" Type="String" />
                            <asp:Parameter Name="original_od_type" Type="String" />
                            <asp:Parameter Name="original_od_From" Type="DateTime" />
                            <asp:Parameter Name="original_od_To" Type="DateTime" />
                            <asp:Parameter Name="original_od_reportTime" Type="String" />
                            <asp:Parameter Name="original_od_reason" Type="String" />
                            <asp:SessionParameter Name="userid" SessionField="userid" Type="int32" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="od_cntId" Type="String" />
                            <asp:Parameter Name="od_type" Type="String" />
                            <asp:Parameter Name="od_From" Type="DateTime" />
                            <asp:Parameter Name="od_To" Type="DateTime" />
                            <asp:Parameter Name="od_reportTime" Type="String" />
                            <asp:Parameter Name="od_reason" Type="String" />
                            <asp:SessionParameter Name="userid" SessionField="userid" Type="int32" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="AllEmployee" runat="server" ConflictDetection="CompareAllValues"
                        SelectCommand=""></asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td style="height: 41px">
                    <asp:HiddenField ID="EmployeeIds" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
