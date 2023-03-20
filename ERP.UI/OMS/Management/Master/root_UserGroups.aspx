<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                20-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

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

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 5px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #StateGrid
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        .chosen-container-single .chosen-single span
        {
            min-height: 30px;
            line-height: 30px;
        }

        .chosen-container-single .chosen-single div {
        background: #094e8c;
        color: #fff;
        border-radius: 4px;
        height: 26px;
        top: 1px;
        right: 1px;
        /*position:relative;*/
    }

        .chosen-container-single .chosen-single div b {
            display: none;
        }

        .chosen-container-single .chosen-single div::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 2px;
            right: 5px;
            font-size: 18px;
            transform: rotate(269deg);
            font-weight: 500;
        }

    .chosen-container-active.chosen-with-drop .chosen-single div {
        background: #094e8c;
        color: #fff;
    }

        .chosen-container-active.chosen-with-drop .chosen-single div::after {
            transform: rotate(90deg);
            right: 5px;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
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
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
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
                            <%--Rev 1.0: "pb-10" class add --%>
                            <td class="pb-10">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <% if (rights.CanAdd)
                                               { %>
                                            <button type="button" onclick="AddnewClick()" class="btn btn-success">Add New</button>
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
                    <button type="button" class="btn btn-primary" onclick="AddNewUserGroup()" >Save</button>
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
