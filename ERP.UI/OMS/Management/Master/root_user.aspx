<%@ Page Title="Users" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_root_user" CodeBehind="root_user.aspx.cs" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">

        function AddUserDetails() {
            // document.location.href="RootUserDetails.aspx?id=Add"
            var url = 'RootUserDetails.aspx?id=Add';
            //OnMoreInfoClick(url, "Add User Details", '940px', '450px', "Y");
            location.href = url;
        }
        function EditUserDetails(keyValue) {

            //document.location.href="RootUserDetails.aspx?id="+keyValue;
            var url = 'RootUserDetails.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Edit User Details", '940px', '450px', "Y");
            location.href = url;
        }
        //Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master
        function CopyUserDetails(keyValue) {
            var url = 'RootUserDetails.aspx?key=Copy&id=' + keyValue;
            location.href = url;
        }
        //Rev work close 13.05.2022 Mantise No:0024892: Copy feature add in User master
        function ChangePassword(keyValue) {
            var url = '../ToolsUtilities/frmchangeuserspassword.aspx?uid=' + keyValue;
            location.href = url;
        }

        function RefreshLoggoff(keyValue) {

            //document.location.href="RootUserDetails.aspx?id="+keyValue;
          



            $.ajax({
                type: "POST",
                url: "root_user.aspx/Resetloggedin",
                data: JSON.stringify({ Userid: keyValue }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                
                    if (msg.d == true) {

                        grid.Refresh();

                    }
                    else {
                      
                    }
                }
            });
        }


        function AddCompany(keyValue, user_loginId) {

            //document.location.href="RootUserDetails.aspx?id="+keyValue;
            var url = 'Root_AddUserCompany.aspx?id=' + keyValue + '&LoginID=' + user_loginId;
            //OnMoreInfoClick(url, "Add Company Details for User", '940px', '450px', "Y");
            location.href = url;
        }
        function AddCashBank(keyValue) {
            var url = 'Root_AddCashBank.aspx?id=' + keyValue;
            location.href = url;
        }

        FieldName = 'Headermain1_cmbSegment';
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback('All');
            // grid.PerformCallback();
        }
        function EndCall(obj) {
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Users</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--<tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">All User List</span></strong>
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
                                            <a href="javascript:void(0);" onclick="AddUserDetails()" class="btn btn-primary"><span>Add New</span> </a>
                                            <% } %>
                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                            <% } %>
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                                        </td>
                                        <%--<td id="Td1">
                                            <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>
                                        </td>--%>
                                    </tr>
                                </table>
                            </td>
                            <td></td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="userGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        DataSourceID="RootUserDataSource" KeyFieldName="user_id" Width="100%" OnCustomCallback="userGrid_CustomCallback" OnCustomJSProperties="userGrid_CustomJSProperties" SettingsBehavior-AllowFocusedRow="true"
                        SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
                       <SettingsSearchPanel Visible="True" Delay="5000" />
                         <Columns>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="0" FieldName="user_id"
                                Visible="False">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="1" FieldName="user_name"
                                Caption="Name">
                                <PropertiesTextEdit>
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter user Name" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="User Name:" Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Designation" FieldName="designation">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="3" FieldName="user_loginId"
                                Caption="Login ID" Width="8%">
                                <PropertiesTextEdit>
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter Login Id" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="Login Id:" Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="4" FieldName="AssignedUser"
                                Caption="Associated User">
                                <PropertiesTextEdit>
                                   
                                </PropertiesTextEdit>
                                <EditFormSettings  Visible="false" />
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="5" FieldName="BranchName"
                                Caption="Branch" Width="15%">
                                <PropertiesTextEdit>
                                   
                                </PropertiesTextEdit>
                                <EditFormSettings  Visible="false" />
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="6" FieldName="grp_name"
                                Caption="Group" Width="15%">
                                <PropertiesTextEdit>
                                   
                                </PropertiesTextEdit>
                                <EditFormSettings  Visible="false" />
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="7" FieldName="Status"
                                Caption="User Status">
                                <PropertiesTextEdit>
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter Login Id" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="Login Id:" Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                                  <%--<dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="8" FieldName="StatusMac"
                                Caption="Mac Lock">
<Settings AllowAutoFilterTextInputTimer="False" />
                  
                            </dxe:GridViewDataTextColumn>--%>



                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="9" FieldName="Status" Settings-AllowAutoFilter="False"
                                Caption="Online Status" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">

                                  <DataItemTemplate>
                                  

                                         <%# Eval("Onlinestatus").ToString()=="1" ? "<img title='logged in' src='../../../assests/images/activeState.png' />" : "<img title='logged off' src='../../../assests/images/inactiveState.png' />" %>


                                </DataItemTemplate>
                                <HeaderTemplate>Online Status</HeaderTemplate>
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />

                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Company Rights" VisibleIndex="10" Width="5%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                          
                                     <% if (CmpanyCount >1)
                                       { %>
                                   <a href="javascript:void(0);" onclick="AddCompany('<%#Container.KeyValue %>','<%#Eval("user_loginId") %>')" title="Add Company">
                                        <img src="../../../assests/images/Add.png" />
                                    </a>
                             <% } %>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <HeaderTemplate>
                                    <span>Company Rights</span>
                                </HeaderTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Cash/Bank Rights" VisibleIndex="11" Width="5%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                              
                                   <a href="javascript:void(0);" onclick="AddCashBank('<%# Container.KeyValue %>')" title="Add Cash Bank">
                                        <img src="../../../assests/images/Add.png" />
                                    </a>
                             
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <HeaderTemplate>
                                    <span>Cash/Bank Rights</span>
                                </HeaderTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="12" Width="6%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                                       <a href="javascript:void(0);" onclick="RefreshLoggoff('<%# Container.KeyValue %>')" title="Reset Online Status" class="pad">
                                        <span class="fa fa-refresh"></span></a>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="EditUserDetails('<%# Container.KeyValue %>')" title="Edit" class="pad">
                                        <img src="../../../assests/images/Edit.png" /></a>
                                    <% } %>
                                    <%--Rev work start 13.05.2022 Mantise No:0024892: Copy feature add in User master--%>
                                    <% if (rights.CanAdd)
                                       { %>
                                    <a href="javascript:void(0);" onclick="CopyUserDetails('<%# Container.KeyValue %>')" title="Copy" class="pad">
                                        <img src="../../../assests/images/copy.png" /></a>
                                    <% } %>
                                    <%--Rev work close 13.05.2022 Mantise No:0024892: Copy feature add in User master--%>
                                    <a href="javascript:void(0);" onclick="ChangePassword('<%# Container.KeyValue %>')" title="Change Password">
                                        <img src="../../../assests/images/change-dark.png" /></a>
                                </DataItemTemplate>
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <%--<Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                        </Styles>--%>
                   
                        <Settings ShowStatusBar="Hidden" ShowFilterRow="true" ShowGroupPanel="True" ShowFilterRowMenu="true" />
                        <%--<SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                        <SettingsBehavior ConfirmDelete="True" />
                        <ClientSideEvents EndCallback="function(s, e) {
	EndCall(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="RootUserDataSource" runat="server" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_user] WHERE [user_id] = @original_user_id"
            OldValuesParameterFormatString="original_{0}" SelectCommand="">
            <DeleteParameters>
                <asp:Parameter Name="original_user_id" Type="Decimal" />
            </DeleteParameters>
            <SelectParameters>
                <asp:SessionParameter Name="branch" SessionField="userbranchHierarchy" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
