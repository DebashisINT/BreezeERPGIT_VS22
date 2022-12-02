<%@ Page Title="User Group" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="frmAddUserGroup.aspx.cs" Inherits="ERP.OMS.Management.Master.frmAddUserGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="/assests/pluggins/easyui/easyui.css" rel="stylesheet" />
<link href="/assests/pluggins/easyui/icon.css" rel="stylesheet" />
<link href="/assests/pluggins/jquery.alert/jquery.alerts.css" rel="stylesheet" />
<script src="/assests/pluggins/jquery.alert/jquery.ui.draggable.js"></script>
<script src="/assests/pluggins/jquery.alert/jquery.alerts.js"></script>

    <style type="text/css">
        .tree-title {
            width: 96%;
        }
        .tree-title>div {
            min-width:250px;
        }

            .tree-title span {
                float: left;
               
            }
        .tree-title span input {
            margin-left:13px;
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
           /* left: 3px;*/
            position: relative;
        }

        
     
    </style>

    <script type="text/javascript" src="/assests/pluggins/easyui/jquery.easyui.min.js"></script>
    <script lang="javascript" type="text/javascript">
        var CheckFire = false;
        $(function () {

            $('#ulMenuTree').tree({                
                checkbox: true,
                cascadeCheck: true,
                animate: true,
                icon: false,
                onLoadSuccess: function (node, data) {
                   // alert('2');
                    CheckListgenerator();
                    CheckSelectedValues();
                    CheckFire = true;
                },
                onCheck: function (node, checked) {
                    //alert('3');
                    if (CheckFire) {
                        var nodes = $('#ulMenuTree').tree('getChecked', 'unchecked');

                        var CheckedUncheckedNodeId = node.id;
                        var IsUnchekced = false;

                        //Uncheck all the nodes which is not checked
                        var $chckRights = $('.chckRights');

                        var MenuIdsChecked = [];

                        $.each(nodes, function (index, value) {
                            var menuId = value.id;

                            $.each($chckRights, function (index, value) {
                                var dataMenuId = $(this).attr('data-menuid');

                                var $chck = $(this);

                                if (dataMenuId == menuId) {
                                    MenuIdsChecked.push(menuId);
                                    if (menuId == CheckedUncheckedNodeId) {
                                        IsUnchekced = true;
                                    }
                                    $chck.prop('checked', false);
                                }
                            });
                        });
                        //Uncheck all the nodes which is not checked

                        if (!IsUnchekced) {

                            //If single node checked
                            if (CheckedUncheckedNodeId > 0) {
                                $.each($chckRights, function (index, value) {
                                    var dataMenuId = $(this).attr('data-menuid');

                                    var $chck = $(this);

                                    if (dataMenuId == CheckedUncheckedNodeId) {
                                        $chck.prop('checked', true);
                                    }
                                });
                            }
                                //If single node checked
                            else {
                                var childrenNode = $('#ulMenuTree').tree('getChildren', node.target);
                                $.each(childrenNode, function (index, value) {

                                    var flagValue = true;

                                    for (var i = 0; i < MenuIdsChecked.length; i++) {
                                        if (MenuIdsChecked[i] == value.id) {
                                            flagValue = false;
                                            break;
                                        }
                                    }

                                    if (flagValue) {
                                        $.each($chckRights, function (index, value1) {
                                            var dataMenuId = $(this).attr('data-menuid');

                                            var $chck = $(this);

                                            if (dataMenuId == value.id) {
                                                $chck.prop('checked', true);
                                            }
                                        });
                                    }
                                });
                            }
                        }

                        CheckFire = false;
                        GenerateData();
                        CheckFire = true;
                    }
                }
            });

            if ($('#hdnMessage').val()) {
                jAlert($('#hdnMessage').val());
            }

            $('.chckRights').click(function () {               
                //CheckFire = false;
                //GenerateData();
                //SelectrightData();
                //CheckListgenerator();
               // CheckFire = true;
            });

            $('#btnTagAll').click(function (e) {
                e.preventDefault();
                CheckFire = false;
                $('.chckRights').prop('checked', true);
                GenerateData();
                CheckFire = true;
            });

            $('#btnUnTagAll').click(function (e) {
                e.preventDefault();
                CheckFire = false;
                $('.chckRights').prop('checked', false);
                GenerateData();
                CheckFire = true;
            });
        });

        function OnMemberInfoClick(keyValue) {
            //alert(keyValue);
            $('#dvUserList').load('/Ajax/_PartialGroupUserListForShow', { GroupId: keyValue }, function () {
                $('#dvgrpUserList').modal('show');
            });
        }

        function CheckSelectedValues() {// alert('1')
            var $rightChecked = $('.chckRights');
            var GroupUserRights = $('#GroupUserRights').val();
           // alert(GroupUserRights);
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



        function SelectrightData() {
            //alert('Hi');
            var $rightChecked = $('.chckRights:checked');
           
            var subMenuIds = [];

            $.each($rightChecked, function (index, value) {
                var SubMenuId = $(this).attr('data-menuid');
              //  alert(SubMenuId);
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
                // alert('6');
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

            //CheckListgenerator();
        }

        function GenerateData() {
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
               // alert('6');
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
           // alert('5');

            var GroupUserRights = $('#GroupUserRights').val();
            var nodes = $('#ulMenuTree').tree('getChecked');
            //$.each(nodes, function (index, value) {
            //    var node = $('#ulMenuTree').tree('find', value.id);
            //    $('#ulMenuTree').tree('uncheck', node.target);
            //});
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
    <style>
        #RequiredFieldValidator1 {
            position:absolute;
            right:1px;
            top:5px;
        }
        .scroller1, .scroller2 {
            overflow:scroll;width:70%;
        }
        .scroller1{height: 20px; }
        .scroller2{height: 500px; }
        .scroller1 .div1 {width:2200px; height: 20px; }
    </style>
     <script>
         $(function () {
             $(".scroller1").scroll(function () {
                 $(".scroller2")
                     .scrollLeft($(".scroller1").scrollLeft());
             });
             $(".scroller2").scroll(function () {
                 $(".scroller1")
                     .scrollLeft($(".scroller2").scrollLeft());
             });
         });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" style="padding-bottom:39px !important">
            <h3>Add / Modify User Group</h3>
            <div class="crossBtn" ><a href="root_UserGroups.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">

        <table id="tblCreateModifyForms" runat="server" style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <div class="panel-body">
                        <div class="form-horizontal" style="padding: 10px;">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class=" col-sm-4">
                                            <asp:Label ID="lblGroupName" runat="server" Text="Group Name"></asp:Label><span style="color:red;"> *</span>
                                        </div>
                                        <div class="col-sm-4 divDown" style="position:relative">
                                            <asp:TextBox ID="txtGroupName" runat="server" MaxLength="50"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGroupName" ErrorTextPosition="Right"  CssClass="pullrightClass fa fa-exclamation-circle r591" SetFocusOnError="true"
                                    Display="Dynamic" ErrorMessage="" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-4 divDown">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" OnClientClick="SelectrightData();" Text="Save" />&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" OnClick="btnCancel_Click" Text="Cancel" />
                                        </div>
                                    </div>
                                </div>
                                
                            </div>
                            
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
                            
                            <div class="row">
                                <div class="scroller1">
                                    <div class="div1">
                                    </div>
                                </div>
                                <div class="form-group scroller2" style="">
                                    <div class=" col-sm-12" id="dvTreeMenus" runat="server" style="width:2400px;"></div>
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
</asp:Content>

