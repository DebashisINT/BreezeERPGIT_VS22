<%@ Page title="TDS/TCS" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_IframeTdsTcs" CodeBehind="IframeTdsTcs.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Js/IframeTdsTcs.js"></script>   
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>TDS/TCS</h3>
        </div>

    </div>
    <div class="form_main">
        <div class="FilterSide">
                <div style="float: left; padding-right: 5px;">
                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                    { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick( )" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                    <%} %>
                </div>
                <div class="pull-left">
              <% if (rights.CanExport)
                                               { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
               <%} %>
             
                </div>
        </div>
        <table class="TableMain100">
            <%--    <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">TDS/TCS</span></strong>
                    </td>
                </tr>--%>
            <tr>
                <td class="relative">
                    <dxe:ASPxGridView ID="gridTdsTcs" runat="server" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu ="true" 
                        ClientInstanceName="gridTdsTcs" DataSourceID="SqlTdsCts" KeyFieldName="TDSTCS_ID" Settings-ShowFilterBar="Visible" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        OnCustomCallback="gridTdsTcs_CustomCallback" OnCustomJSProperties="gridTdsTcs_CustomJSProperties"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                        <SettingsSearchPanel Visible="true" Delay="7000" />
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                        </Styles>
                        <Settings ShowStatusBar="Visible" ShowGroupPanel="True" ShowFilterBar="Hidden" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" AllowFocusedRow="true" ConfirmDelete="True" />
                        <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20"
                            PageSize="20">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_ID" Visible="False" ReadOnly="True"
                                VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_Type" Caption="Type" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_Code" Caption="Short Name" ReadOnly="True"
                                VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_Description" Caption="Description"
                                VisibleIndex="3">
                                <EditFormCaptionStyle Wrap="True">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_MainAccountCode" Caption="Main Account"
                                VisibleIndex="4">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_SubAccountCode" Caption="Sub Account"
                                VisibleIndex="5">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" Width="0">
                                <HeaderStyle HorizontalAlign="Center" />
                                 <HeaderTemplate>
                                     <span ></span>
                                    <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                    <a href="javascript:void(0);" onclick="OnAddButtonClick( )"><span>Add New</span> </a>
                                    <%} %>--%>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <%-- <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">
                                        <img src="/assests/images/Edit.png" /></a>
                                    <%} %>--%>
                                    <div class='floatedBtnArea'>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" class="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                    <a href="javascript:void(0);" onclick="fn_DeleteTDSTCS('<%# Container.KeyValue %>')" title=""  class="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                     </div>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="true" HorizontalAlign="Center">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                          <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
                       <%-- <ClientSideEvents EndCallback="function(s, e) {EndCall(s);}" />--%>
                     <%--   <ClientSideEvents EndCallback="function(s, e) {EndCall(s.cpEND);}" />--%>
                    </dxe:ASPxGridView>
                    <%--                        SelectCommand="select TDSTCS_ID,TDSTCS_Type,TDSTCS_Code,TDSTCS_Description,(select mainaccount_name from master_mainaccount where mainaccount_accountcode=Master_TDSTCS.TDSTCS_MainAccountCode) as TDSTCS_MainAccountCode,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode)))))) as TDSTCS_SubAccountCode from Master_TDSTCS order by TDSTCS_CreateDateTime desc"--%>

                    <%--SelectCommand ="select TDSTCS_ID,TDSTCS_Type,TDSTCS_Code,TDSTCS_Description,(select mainaccount_name from master_mainaccount where mainaccount_accountcode=Master_TDSTCS.TDSTCS_MainAccountCode) as TDSTCS_MainAccountCode,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),'' ))))  as TDSTCS_SubAccountCode from Master_TDSTCS order by TDSTCS_CreateDateTime desc"--%>
                    <asp:SqlDataSource ID="SqlTdsCts" runat="server"
                        DeleteCommand="DELETE FROM [Master_TDSTCS] WHERE [TDSTCS_ID] = @TDSTCS_ID AND [TDSTCS_Code] = @TDSTCS_Code"
                        InsertCommand="INSERT INTO [Master_TDSTCS] ([TDSTCS_Type], [TDSTCS_Code], [TDSTCS_Description], [TDSTCS_MainAccountCode], [TDSTCS_SubAccountCode], [TDSTCS_CreateUser], [TDSTCS_CreateDateTime], [TDSTCS_ModifyUser], [TDSTCS_ModifyDateTime]) VALUES (@TDSTCS_Type, @TDSTCS_Code, @TDSTCS_Description, @TDSTCS_MainAccountCode, @TDSTCS_SubAccountCode, @TDSTCS_CreateUser, @TDSTCS_CreateDateTime, @TDSTCS_ModifyUser, @TDSTCS_ModifyDateTime)"
                         
                        SelectCommand ="select TDSTCS_ID,TDSTCS_Type,TDSTCS_Code,TDSTCS_Description,(select mainaccount_name from master_mainaccount where mainaccount_accountcode=Master_TDSTCS.TDSTCS_MainAccountCode) as TDSTCS_MainAccountCode,(select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=Master_TDSTCS.TDSTCS_SubAccountCode) as TDSTCS_SubAccountCode from Master_TDSTCS order by TDSTCS_CreateDateTime desc"

                        UpdateCommand="UPDATE [Master_TDSTCS] SET [TDSTCS_Type] = @TDSTCS_Type, [TDSTCS_Description] = @TDSTCS_Description, [TDSTCS_MainAccountCode] = @TDSTCS_MainAccountCode, [TDSTCS_SubAccountCode] = @TDSTCS_SubAccountCode, [TDSTCS_CreateUser] = @TDSTCS_CreateUser, [TDSTCS_CreateDateTime] = @TDSTCS_CreateDateTime, [TDSTCS_ModifyUser] = @TDSTCS_ModifyUser, [TDSTCS_ModifyDateTime] = @TDSTCS_ModifyDateTime WHERE [TDSTCS_ID] = @TDSTCS_ID AND [TDSTCS_Code] = @TDSTCS_Code">
                        <DeleteParameters>
                            <asp:Parameter Name="TDSTCS_ID" Type="Int32" />
                            <asp:Parameter Name="TDSTCS_Code" Type="String" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="TDSTCS_Type" Type="String" />
                            <asp:Parameter Name="TDSTCS_Description" Type="String" />
                            <asp:Parameter Name="TDSTCS_MainAccountCode" Type="String" />
                            <asp:Parameter Name="TDSTCS_SubAccountCode" Type="String" />
                            <asp:Parameter Name="TDSTCS_ModifyUser" Type="Int32" />
                            <asp:Parameter Name="TDSTCS_ModifyDateTime" Type="DateTime" />
                            <asp:Parameter Name="TDSTCS_ID" Type="Int32" />
                            <asp:Parameter Name="TDSTCS_Code" Type="String" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="TDSTCS_Type" Type="String" />
                            <asp:Parameter Name="TDSTCS_Code" Type="String" />
                            <asp:Parameter Name="TDSTCS_Description" Type="String" />
                            <asp:Parameter Name="TDSTCS_MainAccountCode" Type="String" />
                            <asp:Parameter Name="TDSTCS_SubAccountCode" Type="String" />
                            <asp:Parameter Name="TDSTCS_CreateUser" Type="Int32" />
                            <asp:Parameter Name="TDSTCS_CreateDateTime" Type="DateTime" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
     <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
</asp:Content>
