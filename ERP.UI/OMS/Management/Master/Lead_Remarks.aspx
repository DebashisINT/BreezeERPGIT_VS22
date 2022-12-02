<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_Master_Lead_Remarks" CodeBehind="Lead_Remarks.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {

            grid.PerformCallback();
        }

        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Lead_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Lead_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Lead_BankDetails.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                //document.location.href="Lead_DPDetails.aspx"; 
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Lead_Document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Lead_FamilyMembers.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "Lead_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Lead_GroupMember.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                //document.location.href="Lead_Remarks.aspx"; 
            }
        }
    </script>
    <style>
        .dxflCLLSys.dxflItemSys {
            margin: 0 !important;
        }

            .dxflCLLSys.dxflItemSys .btn-primary.dxbTSys {
                margin-left: 55px;
            }
    </style>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Lead Remarks</h3>
        </div>
        <div class="crossBtn"><a href="Lead.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="8" ClientInstanceName="page">
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px"></Border>
                        </ContentStyle>
                        <TabPages>
                            <dxe:TabPage Name="General" Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="CorresPondence" Text="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Text="DP Details" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Family Members" Text="Family Members">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Registration" Text="Registration">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Group Member" Text="Group Member">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Remarks" Text="Remarks">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                      <%-- ..................................... Code added by Sam on 04102016....................................--%>
                                         <div style="float: left; padding-right: 5px;"> 
                                             <a href="javascript:void(0);" onclick="grid.AddNewRow();"  class="btn btn-primary"><span style="text-decoration: underline">Add New</span> </a>
                                           <%-- <a href="javascript:void(0);" onclick="gridAddress.AddNewRow();" class="btn btn-primary">
                                        <span style="text-decoration: underline">Add New</span></a> --%>
                                        </div>
                                        <%-- ..................................... Code added by Sam on 04102016....................................--%>
                                        <dxe:ASPxGridView runat="server" ClientInstanceName="grid" KeyFieldName="id" AutoGenerateColumns="False"
                                            DataSourceID="SqlRemarks" Width="100%" ID="GridRemarks" __designer:wfdid="w1"
                                            OnCellEditorInitialize="GridRemarks_CellEditorInitialize" OnCustomCallback="GridRemarks_CustomCallback">
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="rea_internalId" Visible="False" VisibleIndex="1">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="description" Width="40%" Caption="Remarks Category"
                                                    VisibleIndex="0">
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="cat_id" Visible="False" VisibleIndex="1">
                                                    <PropertiesComboBox DataSourceID="sqlCategory" TextField="cat_description" ValueField="id"
                                                        ValueType="System.String" Width="300px">
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="0" Caption="Category"></EditFormSettings>
                                                    <EditCellStyle Wrap="False">
                                                    </EditCellStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataMemoColumn FieldName="rea_Remarks" Width="40%" Caption="Remarks"
                                                    VisibleIndex="1">
                                                    <PropertiesMemoEdit Width="350px" Height="100px">
                                                    </PropertiesMemoEdit>
                                                    <EditFormSettings Caption="Remarks"></EditFormSettings>
                                                    <EditCellStyle HorizontalAlign="Left">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle HorizontalAlign="Right">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataMemoColumn>
                                                <dxe:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>Actions</HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <%--<HeaderTemplate>
                                                        <a href="javascript:void(0);" onclick="grid.AddNewRow();"><span>Add New</span> </a>
                                                    </HeaderTemplate>--%>
                                                </dxe:GridViewCommandColumn>
                                            </Columns>
                                            <SettingsCommandButton>


                                                <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                                                </EditButton>
                                                <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                </DeleteButton>
                                                <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                            </SettingsCommandButton>
                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True"></SettingsBehavior>
                                            <SettingsPager PageSize="20" NumericButtonCount="20" ShowSeparators="True">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <SettingsEditing Mode="PopupEditForm" PopupEditFormWidth="600px"
                                                PopupEditFormHorizontalAlign="Center" PopupEditFormVerticalAlign="WindowCenter"
                                                PopupEditFormModal="True" EditFormColumnCount="1">
                                            </SettingsEditing>
                                            <Settings ShowTitlePanel="True" ShowGroupPanel="true"></Settings>
                                            <SettingsText ConfirmDelete="Are you sure to Delete this Record!" PopupEditFormCaption="Add/Modify Remarks"></SettingsText>
                                            <Styles>
                                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                            </Styles>
                                            <Templates>
                                                <TitlePanel>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="text-align: left; vertical-align: top">
                                                                <table>
                                                                    <tr>
                                                                        <td id="ShowFilter">
                                                                            <%-- <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>--%>
                                                                        </td>
                                                                        <td id="Td1">
                                                                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <%--      <table style="width:100%">
                                                <tr>
                                                     <td align="right">
                                                        <table width="200">
                                                            <tr>
                                                                
                                                                <td>
                                                                    <dxe:ASPxButton ID="ASPxButton2" runat="server" Text="Search" ToolTip="Search" OnClick="btnSearch"  Height="18px" Width="88px">
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px" AutoPostBack="False">
                                                                        <clientsideevents click="function(s, e) {grid.AddNewRow();}" />
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                                                                    
                                                                 
                                                              </tr>
                                                          </table>
                                                      </td>   
                                                 </tr>
                                            </table>--%>
                                                </TitlePanel>
                                            </Templates>
                                        </dxe:ASPxGridView>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
	                                            var Tab5 = page.GetTab(5);
	                                            var Tab6 = page.GetTab(6);
	                                            var Tab7 = page.GetTab(7);
	                                             var Tab8 = page.GetTab(8);
	                                            
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            else if(activeTab == Tab3)
	                                            {
	                                                disp_prompt('tab3');
	                                            }
	                                            else if(activeTab == Tab4)
	                                            {
	                                                disp_prompt('tab4');
	                                            }
	                                            else if(activeTab == Tab5)
	                                            {
	                                                disp_prompt('tab5');
	                                            }
	                                            else if(activeTab == Tab6)
	                                            {
	                                                disp_prompt('tab6');
	                                            }
	                                            else if(activeTab == Tab7)
	                                            {
	                                                disp_prompt('tab7');
	                                            }
	                                            else if(activeTab == Tab8)
	                                            {
	                                                disp_prompt('tab8');
	                                            }
	                                            
	                                            
	                                            }"></ClientSideEvents>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox></td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlRemarks" runat="server"
            DeleteCommand="DELETE FROM [tbl_master_LeadRemarks] WHERE [id] = @id" InsertCommand="INSERT INTO [tbl_master_LeadRemarks] ([rea_internalId],[cat_id], [rea_Remarks], [CreateDate], [CreateUser]) VALUES (@rea_internalId,@cat_id, @rea_Remarks, getdate(), @CreateUser)"
            SelectCommand="SELECT *,isnull((select cat_description from tbl_master_remarksCategory where id=tbl_master_LeadRemarks.cat_id),'None') as description FROM [tbl_master_LeadRemarks] where rea_internalId=@rea_internalId"
            UpdateCommand="UPDATE [tbl_master_LeadRemarks] SET [rea_internalId] = @rea_internalId,cat_id=@cat_id, [rea_Remarks] = @rea_Remarks,  [LastModifyDate] = getdate(), [LastModifyUser] = @LastModifyUser WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:SessionParameter Name="rea_internalId" SessionField="KeyVal_InternalID" Type="string" />
                <asp:Parameter Name="cat_id" Type="int32" />
                <asp:Parameter Name="rea_Remarks" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="string" />
            </InsertParameters>
            <UpdateParameters>
                <asp:SessionParameter Name="rea_internalId" SessionField="KeyVal_InternalID" Type="string" />
                <asp:Parameter Name="cat_id" Type="int32" />
                <asp:Parameter Name="rea_Remarks" Type="String" />
                <asp:SessionParameter Name="LastModifyUser" SessionField="userid" Type="string" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:SessionParameter Name="rea_internalId" SessionField="KeyVal_InternalID" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlCategory" runat="server" 
            SelectCommand="SELECT * FROM [tbl_master_remarksCategory] where cat_applicablefor='Ld'"></asp:SqlDataSource>
    </div>
</asp:Content>
