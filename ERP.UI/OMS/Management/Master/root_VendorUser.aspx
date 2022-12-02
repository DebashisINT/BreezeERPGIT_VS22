<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="root_VendorUser.aspx.cs" Inherits="ERP.OMS.Management.Master.root_VendorUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">

        function AddUserDetails() {
            // document.location.href="RootUserDetails.aspx?id=Add"
            var url = 'RootVendorDetails.aspx?id=Add';
            //OnMoreInfoClick(url, "Add User Details", '940px', '450px', "Y");
            location.href = url;
        }
        function EditUserDetails(keyValue) {

            //document.location.href="RootUserDetails.aspx?id="+keyValue;
            var url = 'RootVendorDetails.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Edit User Details", '940px', '450px', "Y");
            location.href = url;
        }
        function ChangePassword(keyValue) {
            var url = '../ToolsUtilities/frmChangeVendorPassword.aspx?uid=' + keyValue;
            location.href = url;
        }
        function AddCompany(keyValue) {

            //document.location.href="RootUserDetails.aspx?id="+keyValue;
            var url = 'Root_AddUserCompany.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Add Company Details for User", '940px', '450px', "Y");
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
        function gridRowclick(s, e) {
            $('#userGrid').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Vendors</h3>
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
                                           <%-- <%if (rights.CanAdd)
                                              { %>--%>
                                            <a href="javascript:void(0);" onclick="AddUserDetails()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                                           <%-- <% } %>
                                            <% if (rights.CanExport)
                                               { %>--%>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                           <%-- <% } %>--%>
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
                <td class="relative">
                    <dxe:ASPxGridView ID="userGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        DataSourceID="RootUserDataSource" KeyFieldName="user_id" Width="100%" OnCustomCallback="userGrid_CustomCallback" OnCustomJSProperties="userGrid_CustomJSProperties"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="0" FieldName="user_id"
                                Visible="False">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="1" FieldName="user_name"
                                Caption="Name" Width="32%">
                                <PropertiesTextEdit>
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter user Name" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="User Name:" Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                           <%-- <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Designation" FieldName="designation">
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="3" FieldName="user_loginId"
                                Caption="Login ID">
                                <PropertiesTextEdit>
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter Login Id" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="Login Id:" Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="4" FieldName="Status"
                                Caption="User Status">
                                <PropertiesTextEdit>
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter Login Id" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Caption="Login Id:" Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                       <%--     <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="5" FieldName="Status" Settings-AllowAutoFilter="False"
                                Caption="Online Status" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">

                                  <DataItemTemplate>
                                  

                                         <%# Eval("Onlinestatus").ToString()=="1" ? "<img title='logged in' src='../../../assests/images/activeState.png' />" : "<img title='logged off' src='../../../assests/images/inactiveState.png' />" %>


                                </DataItemTemplate>
                                <HeaderTemplate>Online Status</HeaderTemplate>
                                <EditFormSettings Visible="False" />

                            </dxe:GridViewDataTextColumn>--%>


                       <%--     <dxe:GridViewDataTextColumn Caption="Company Add/Edit" VisibleIndex="6" Width="5%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                              
                                   <a href="javascript:void(0);" onclick="AddCompany('<%# Container.KeyValue %>')" title="Add Company">
                                        <img src="../../../assests/images/Add.png" />
                                    </a>
                             
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <HeaderTemplate>
                                    <span>Company Add/Edit</span>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>--%>



                            <dxe:GridViewDataTextColumn VisibleIndex="7" Width="0" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="EditUserDetails('<%# Container.KeyValue %>')" title="" class="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                    <% } %>
                                    <a href="javascript:void(0);" onclick="ChangePassword('<%# Container.KeyValue %>')" title="">
                                       <span class='ico ColorThree'><i class='fa fa-key'></i></span><span class='hidden-xs'>Change Password</span></a>

                                    </div>
                                </DataItemTemplate>
                                <HeaderTemplate></HeaderTemplate>
                                <EditFormSettings Visible="False" />

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
                        <SettingsSearchPanel Visible="True" />
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
}" RowClick="gridRowclick" />
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