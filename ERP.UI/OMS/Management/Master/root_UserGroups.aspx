<%@ Page Title="Security Roles" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="root_UserGroups.aspx.cs" 
    Inherits="ERP.OMS.Management.Master.management_master_root_UserGroups"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<%--    <script src="/assests/pluggins/jquery.alert/jquery.ui.draggable.js"></script>
    <script src="/assests/pluggins/jquery.alert/jquery.alerts.js"></script>--%>

    <style type="text/css">
        .modal-content {
        width: 500px !important;
        left: 25% !important;
        }

        .tree-title {
            width: 600px;
        }

        .tree-title span {
            float: right;
        }

        .tree-folder {
            display: none !important;
        }

        .tree-folder-open {
            display: none !important;
        }

        .tree-icon {
            display: none !important;
        }

        .tree-file {
            display: none !important;
        }

        .chckRights {
            left: 3px;
            position: relative;
        }

        
    </style>

<%--    <script type="text/javascript" src="/assests/pluggins/easyui/jquery.easyui.min.js"></script>--%>
    <script lang="javascript" type="text/javascript">


        $(document).ready(function () {
            $('#AddNewModel').on('shown.bs.modal', function () {
                $('#txtUserGroup').focus();
            })

            $('#AddCopyModel').on('shown.bs.modal', function () {
                $('#txtUserGrp').focus();
            })
        })


        function AddnewClick() {
            $('#AddNewModel').modal('show');
            $('#txtUserGroup').val('');
        }

        function AddnewCopy(keyValue) {
            $('#AddCopyModel').modal('show');
            $('#txtUserGrp').val('');
            $('#hiddnUser').val(keyValue);
        }

        function AddNewUserGroup() {

            var OtherDetails = {}
            OtherDetails.GroupName = $('#txtUserGroup').val();
            $.ajax({
                type: "POST",
                url: "root_UserGroups.aspx/AddNewUserGroup",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    editId = msg.d;
                    if (msg.d != '-1') {
                        jAlert("Security Role Added Successfully.", "Alert", function () {
                            $('#AddNewModel').modal('hide');
                            window.location.href = 'MenuUserRights.aspx?UserGroup=' + editId;
                        });
                    } else {
                        jAlert("Security Role Already Exists.", "Alert", function () {
                             
                        });
                    }
                   
                }
            });
        }

        function AddCopyUserGroup() {

            var OtherDetails = {}
            OtherDetails.GroupName = $('#txtUserGrp').val();
            OtherDetails.acc_userGroupId = $('#hiddnUser').val();
            if (OtherDetails.GroupName) {
                if (OtherDetails.acc_userGroupId) {
                    try {
                        $.ajax({
                            type: "POST",
                            url: "root_UserGroups.aspx/AddCopyUserGroup",
                            data: JSON.stringify(OtherDetails),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                editId = msg.d;
                                if (msg.d != '-1') {
                                    jAlert("Security Role Added Successfully.", "Alert", function () {
                                        $('#AddCopyModel').modal('hide');
                                        window.location.href = 'root_UserGroups.aspx';
                                    });
                                } else {
                                    jAlert("Security Role Already Exists.", "Alert", function () {

                                    });
                                }

                            }
                        });
                    } catch (e) {
                        alert(e);

                    }
                }
            }
            else {
                jAlert("Enter Security Role", "Alert", function () {

                });
            }
        }


        //$(function () {

        //    if ($('#ulMenuTree')) {
        //        $('#ulMenuTree').tree({
        //            checkbox: true,
        //            cascadeCheck: true,
        //            animate: true,
        //            icon: false,
        //            onLoadSuccess: function (node, data) {
        //                CheckListgenerator();
        //                CheckSelectedValues();
        //            }
        //        });
        //    }

        //    if ($('#hdnMessage').val()) {
        //       // jAlert($('#hdnMessage').val());
        //    }

        //    $('.chckRights').click(function () {
        //        GenerateData();
        //    });

        //    $('#btnTagAll').click(function (e) {
        //        e.preventDefault();
        //        $('.chckRights').prop('checked', true);
        //        GenerateData();
        //    });

        //    $('#btnUnTagAll').click(function (e) {
        //        e.preventDefault();
        //        $('.chckRights').prop('checked', false);
        //        GenerateData();
        //    });
        //});

        function OnMemberInfoClick(keyValue) {
            
            //alert(keyValue);
            // Mantis Issue 24367
            //document.location.href = "root_UserGroupMember.aspx?grp=" + keyValue + "";
            document.location.href = "root_UserGroupMember.aspx?grp=" + keyValue + "&callfrommodule=erp";
            // End of Mantis Issue 24367
            //$('#dvUserList').load('/Ajax/_PartialGroupUserListForShow', { GroupId: keyValue }, function () {
            //    $('#dvgrpUserList').modal('show');
            //});
        }

        function CheckSelectedValues() {
            var $rightChecked = $('.chckRights');
            var GroupUserRights = $('#GroupUserRights').val();
            if (GroupUserRights) {
                var SubMenuWithRights = GroupUserRights.split('_');
                for (var i = 0; i < SubMenuWithRights.length; i++) {
                    var SubMenuId = SubMenuWithRights[i].split('^')[0];
                    var rights = SubMenuWithRights[i].split('^')[1].split('|');

                    if (rights && rights.length > 0) {
                        $.each($rightChecked, function (index, value) {
                            var $chck = $(this);
                            var menuid = $chck.attr('data-menuid');
                            if (menuid == SubMenuId) {
                                var role = $chck.attr('data-id');

                                for (var j = 0; j < rights.length; j++) {
                                    if (role == rights[j]) {
                                        $chck.prop('checked', true);
                                    }
                                }
                            }
                        });
                    }
                }
            }
        }

        function GenerateData() {
            return;
            var $rightChecked = $('.chckRights:checked');

            var subMenuIds = [];

            $.each($rightChecked, function (index, value) {
                var SubMenuId = $(this).attr('data-menuid');

                if (subMenuIds && subMenuIds.length > 0) {
                    var flagVal = true;
                    for (var i = 0; i < subMenuIds.length; i++) {
                        if (subMenuIds[i] == SubMenuId) {
                            flagVal = false;
                            break;
                        }
                    }
                    if (flagVal) {
                        subMenuIds.push(SubMenuId);
                    }
                }
                else {
                    subMenuIds.push(SubMenuId);
                }
            });

            if (subMenuIds && subMenuIds.length > 0) {
                var MenuWithRole = '';
                for (var i = 0; i < subMenuIds.length; i++) {
                    var roleString = '';
                    $.each($rightChecked, function (index, value) {
                        var SubMenuId = $(this).attr('data-menuid');
                        var role = $(this).attr('data-id');
                        if (SubMenuId == subMenuIds[i]) {
                            if (roleString == '') {
                                roleString = role;
                            }
                            else {
                                roleString += '|' + role;
                            }
                        }
                    });
                    if (roleString != '') {
                        if (MenuWithRole != '') {
                            MenuWithRole += '_' + subMenuIds[i] + '^' + roleString;
                        }
                        else {
                            MenuWithRole = subMenuIds[i] + '^' + roleString;
                        }
                    }
                }
                $('#GroupUserRights').val(MenuWithRole);
            }
            else {
                $('#GroupUserRights').val('');
            }

            CheckListgenerator();
        }

        function CheckListgenerator() {
            var GroupUserRights = $('#GroupUserRights').val();
            var nodes = $('#ulMenuTree').tree('getChecked');
            $.each(nodes, function (index, value) {
                var node = $('#ulMenuTree').tree('find', value.id);
                $('#ulMenuTree').tree('uncheck', node.target);
            });
            if (GroupUserRights) {
                var SubMenuWithRights = GroupUserRights.split('_');
                for (var i = 0; i < SubMenuWithRights.length; i++) {
                    var SId = SubMenuWithRights[i].split('^')[0];
                    var node = $('#ulMenuTree').tree('find', SId);
                    if (node) {
                        $('#ulMenuTree').tree('check', node.target);
                    }
                }
            }
        }

        function ConfirmToDelete() {
            var value = confirm('Are you sure you want to delete this group?');
            return value;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div>
            <h3>Security Roles <%--&nbsp;<% ERP.OMS.MVCUtility.RenderAction("Test", "_PartialWebFormToMvcTest", new { }); %>--%></h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <tr>
                <td style="text-align: left">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <% if (rights.CanAdd)
                                               { %>
                                            <button type="button" onclick="AddnewClick()" class="btn btn-primary">Add New</button>
                                            <%--<asp:Button ID="btn_Add_New" runat="server" Text="Add New" CssClass="btn btn-primary"  OnClientClick="AddnewClick()" />--%>
                                             <% } %>
                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}" >
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                 <asp:ListItem Value="2">XLS</asp:ListItem>
                                                 <asp:ListItem Value="3">RTF</asp:ListItem>
                                                 <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                              <% } %>
                                        </td>
                                        <%--<td class="pull-right">
                                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                                ForeColor="Black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                                ValueType="System.Int32" Width="130px">
                                                <Items>
                                                    <dxe:ListEditItem Text="Select" Value="0" />
                                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                                </Items>
                                                <Border BorderColor="Black" />
                                                <DropDownButton Text="Export">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>--%>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; vertical-align: top;" runat="server">
                                <dxe:ASPxGridView ID="GridUserGroup" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ClientInstanceName="GridUserGroup" KeyFieldName="grp_id" OnRowCommand="GridUserGroup_RowCommand"
                                    OnRowDeleting="GridUserGroup_RowDeleting" 
                                    Border-BorderStyle="NotSet" SettingsBehavior-AllowFocusedRow="true"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                                  <SettingsSearchPanel Visible="True" Delay="5000" />
                                    <Columns>

                                        <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="grp_name"
                                            Caption="Role Name">
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn VisibleIndex="2" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Width="160">
                                            <DataItemTemplate>
                                                 <% if (rights.CanEdit)
                                                   { %>
                                                <asp:LinkButton ID="btn_show" runat="server" CommandArgument='<%# Container.KeyValue %>' CommandName="edit" Font-Underline="false" CssClass="pad">
                                                     <img src="../../../assests/images/Edit.png" />
                                                </asp:LinkButton>
                                                 <% } %>
                                                <% if (rights.CanDelete)
                                                   { %>
                                                <asp:LinkButton ID="btn_delete" runat="server" OnClientClick="return confirm('Confirm Delete?');" CommandArgument='<%# Container.KeyValue %>' CommandName="delete"> 

                                                      <img src="../../../assests/images/Delete.png" />
                                                </asp:LinkButton>


                                                 <% } %>
                                                <% if (rights.CanAdd)
                                               { %>
                                               <a href="javascript:void(0);" onclick="AddnewCopy('<%# Container.KeyValue %>')" title="Copy">
                                                    <img src="../../../assests/images/copy.png" />
                                                </a>
                                                <% } %>
                                            </DataItemTemplate>
                                            <HeaderTemplate>Actions</HeaderTemplate>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="1" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Width="6%">
                                            <DataItemTemplate>
                                                <%-- <% if (rights.CanMembers)
                                                   { %>--%>
                                                <a href="javascript:void(0);" onclick="OnMemberInfoClick('<%# Container.KeyValue %>')" title="Members">
                                                    <img src="../../../assests/images/Members.png" />
                                                </a> <%--<% } %>--%>
                                            </DataItemTemplate>
                                            <HeaderTemplate>Members</HeaderTemplate>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                        </dxe:GridViewDataTextColumn>

                                    </Columns>
                                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                    <SettingsSearchPanel Visible="True" />
                                     <Settings ShowTitlePanel="false" ShowStatusBar="Hidden" ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu ="true"/>
                                    <SettingsBehavior AllowFocusedRow="true" ConfirmDelete="True" />
                                    <SettingsText ConfirmDelete="Do you want to delete this?" />

                                 
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table id="tblCreateModifyForms" runat="server" style="width: 100%" visible="false">
            <tr>
                <td style="width: 100%">
                    <div class="panel-body">
                        <div class="form-horizontal" style="padding: 10px;">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class=" col-sm-4">
                                            <asp:Label ID="lblGroupName" runat="server" Text="Role Name"></asp:Label>
                                        </div>
                                        <div class="col-sm-4 divDown">
                                            <asp:TextBox ID="txtGroupName" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4 divDown">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" Text="Save" />&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" OnClick="btnCancel_Click" Text="Cancel" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">&nbsp;</div>
                            </div>
                            <div class="clearfix">&nbsp;</div>
                            <div class="row">
                                <div class="form-group">
                                    <div class=" col-sm-12">
                                        <div class="col-sm-3 divDown">
                                            <input type="button" class="btn btn-primary" id="btnTagAll" value="Tag All" />&nbsp;
                                            <input type="button" class="btn btn-primary " id="btnUnTagAll" value="Untag All" />
                                            <asp:HiddenField ID="GroupUserRights" runat="server" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix">&nbsp;</div>
                            <div class="row">
                                <div class="form-group">
                                    <div class=" col-sm-12" id="dvTreeMenus" runat="server"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnMessage" ClientIDMode="Static" runat="server" />
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="GridUserGroup">
        </dxe:ASPxGridViewExporter>
    </div>
    <div class="modal fade" id="dvgrpUserList">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title">Users</h4>
                </div>
                <div class="modal-body" id="dvUserList"></div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success btnwidth" data-dismiss="modal">Ok</button>
                </div>
            </div>
        </div>
    </div>



    <div class="modal fade" id="AddNewModel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Security Role Add</h4>
                </div>
                <div class="modal-body" style="width:100%">
                    <table style="width:100%">
                        <tr>
                            <td style="width:30%">Security Role Name</td>
                            <td><input type="text" id="txtUserGroup"  maxlength="50"/>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success mTop5" onclick="AddNewUserGroup()" >Save</button>
                    <button type="button" class="btn btn-danger " data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    
    <div class="modal fade" id="AddCopyModel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Security Role Add</h4>
                </div>
                <div class="modal-body" style="width:100%">
                    <table style="width:100%">
                        <tr>
                            <td style="width:30%">Security Role Name</td>
                            <td><input type="text" id="txtUserGrp"  maxlength="50"/>
                                <asp:HiddenField ID="hiddnUser" runat="server" ClientIDMode="Static" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success mTop5" onclick="AddCopyUserGroup()" >Save</button>
                    <button type="button" class="btn btn-danger " data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
