<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                15-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SrvMastEntityList.aspx.cs" Inherits="ERP.OMS.Management.Master.SrvMastEntityList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        function OnViewExecutive(keyValue) {
            WorkingRoster();
            if (rosterstatus) {
                var url = 'SrvMastEntity.aspx?id=' + keyValue + '&Type=view';
                window.location.href = url;
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function OnEditButtonClick(keyValue) {
            WorkingRoster();
            if (rosterstatus) {
                var url = 'SrvMastEntity.aspx?id=' + keyValue;
                window.location.href = url;
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function EndCall(obj) {
            if (grid.cpDelmsg != null) {
                jAlert(grid.cpDelmsg);
                grid.cpDelmsg = null;
            }

            if (grid.cpImportModel != null) {
                if (grid.cpImportModel == 'Success') {
                    jAlert('Import successfully');
                    cImportPopupModel.Hide();
                    //grid.Refresh();
                }
                else {
                    jAlert("No data found!");
                    cImportPopupModel.Hide();
                }
            }
        }

        function OnAddButtonClick() {
            WorkingRoster();
            if (rosterstatus) {
                var url = 'SrvMastEntity.aspx?id=ADD';
                window.location.href = url;
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function DeleteRow(keyValue) {
            WorkingRoster();
            if (rosterstatus) {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        grid.PerformCallback('Delete~' + keyValue);
                        grid.Refresh();
                        grid.Refresh();
                    }
                });
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }
        function gridRowclick(s, e) {
            $('#gridFinancer').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                $.each(lists, function (index, value) {
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

    </script>
    <script>
        function ClickOnAssignModule(EntityId) {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnEntityId").val(EntityId);
                var str
                str = { EntityId: EntityId }
                var html = "";
                // alert();
                $.ajax({
                    type: "POST",
                    url: "SrvMastEntityList.aspx/GetModuleList",
                    data: JSON.stringify(str),
                    contentType: "application/json; charset=utf-8",
                    datatype: "json",
                    success: function (responseFromServer) {
                        for (i = 0; i < responseFromServer.d.length; i++) {
                            if (responseFromServer.d[i].IsChecked == true) {
                                html += "<li><input type='checkbox' id=" + responseFromServer.d[i].ModuleId + "  class='statecheck' onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].ModuleId + " checked  /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].ModuleId + " >" + responseFromServer.d[i].ModuleName + "</label></a></li>";
                            }
                            else {
                                html += "<li><input type='checkbox' id=" + responseFromServer.d[i].ModuleId + " class='statecheck'  onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].ModuleId + "   /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].ModuleId + ">" + responseFromServer.d[i].ModuleName + "</label></a></li>";
                            }
                        }
                        $("#divModalBody").html(html);
                        $("#myModal").modal('show');
                    }
                });
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }


        var Modulelist = []
        function ModulePushPop() {
            var EntityId = $("#hdnEntityId").val();
            let a = [];

            $(".statecheckall:checked").each(function () {
                a.push(this.value);
            });

            $(".statecheck:checked").each(function () {
                a.push(this.value);
            });
            var str1
            //  alert(a);

            str1 = { EntityId: EntityId, Modulelist: a }
            $.ajax({
                type: "POST",
                url: "SrvMastEntityList.aspx/GetModuleListSubmit",
                data: JSON.stringify(str1),
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (responseFromServer) {
                    // alert(responseFromServer.d)
                    $("#myModal").modal('hide');
                    jAlert('Module assigned successfully');
                }
            });
        }

        function CheckParticular(v) {
            if (v == false) {
                $(".statecheckall").prop('checked', false);
            }
        }

        function CheckAll(id) {
            var ischecked = $(".statecheckall").is(':checked');
            if (ischecked == true) {
                $('input:checkbox.statecheck').each(function () {
                    $(this).prop('checked', true);
                });

            }
            else {
                $('input:checkbox.statecheck').each(function () {
                    $(this).prop('checked', false);
                });

            }


        }
    </script>
    <style>
        .floatedBtnArea {
            top: 2px;
        }
    </style>
    <script>

        //$(document).ready(function () {
        //    cCompanyComponentPanel.PerformCallback('BindCompanyGrid');
        //});

        function fn_ImportPopUpOpen() {
            WorkingRoster();
            if (rosterstatus) {
                //$('#valid').attr('style', 'display:none;');
                cImportPopupModel.Show();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function selectAll() {
            gridCompanyLookup.gridView.SelectRows();
        }

        function unselectAll() {
            gridCompanyLookup.gridView.UnselectRows();
        }

        function CloseGridBranchLookup() {
            gridCompanyLookup.ConfirmCurrentSelection();
            gridCompanyLookup.HideDropDown();
            gridCompanyLookup.Focus();
        }

        function btnSave_Model() {
            //if (gridCompanyLookup.GetValue() == null) {
            //    jAlert('Please select company');
            //}
            //else {
            //    grid.PerformCallback('ImportModel~0');
            //}
            if ($("#ddlCompany").val() == "") {
                jAlert('Please select company');
            }
            else {
                grid.PerformCallback('ImportModel~' + $("#ddlCompany").val());
            }
        }

        var rosterstatus = false;
        function WorkingRoster() {
            $.ajax({
                type: "POST",
                url: 'SrvMastEntityList.aspx/CheckWorkingRoster',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ module_ID: '9' }),
                success: function (response) {
                    if (response.d.split('~')[0] == "true") {
                        rosterstatus = true;
                    }
                    else if (response.d.split('~')[0] == "false") {
                        rosterstatus = false;
                        $("#spnbegin").text(response.d.split('~')[1]);
                        $("#spnEnd").text(response.d.split('~')[2]);
                    }
                },
            });
        }

        function WorkingRosterClick() {
            $("#divPopHead").addClass('hide');
        }
    </script>
    <style>
        /* for pop */
        .popupWraper {
            position: fixed;
            top: 0;
            left: 0;
            height: 100vh;
            width: 100%;
            background: rgba(0,0,0,0.85);
            z-index: 10;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .popBox {
            width: 670px;
            background: #fff;
            padding: 35px;
            text-align: center;
            min-height: 350px;
            display: flex;
            align-items: center;
            flex-direction: column;
            justify-content: center;
            background: #fff url("/assests/images/popupBack.png") no-repeat top left;
            box-shadow: 0px 14px 14px rgba(0,0,0,0.56);
        }

            .popBox h1, .popBox p {
                font-family: 'Poppins', sans-serif !important;
                margin-bottom: 20px !important;
            }

            .popBox p {
                font-size: 15px;
            }

        .btn-sign {
            background: #3680fb;
            color: #fff;
            padding: 10px 25px;
            box-shadow: 0px 5px 5px rgba(0,0,0,0.22);
        }

            .btn-sign:hover {
                background: #2e71e1;
                color: #fff;
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

        select
        {
            height: 30px !important;
            border-radius: 4px;
            /*-webkit-appearance: none;*/
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

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
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img
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
            top: 6px;
            right: -2px;
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
        select.btn
        {
            padding-right: 10px !important;
        }

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

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        #B2B , 
        #grid_B2BUR , 
        #grid_IMPS , 
        #grid_IMPG ,
        #grid_CDNR ,
        #grid_CDNUR ,
        #grid_EXEMP ,
        #grid_ITCR ,
        #grid_HSNSUM
        {
            max-width: 98% !important;
        }*/

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

        select.btn
        {
           position: relative;
           z-index: 0;
        }

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
             top: 18px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="popupWraper hide" id="divPopHead" runat="server">
        <div class="popBox">
            <img src="/assests/images/warningAlert.png" class="mBot10" style="width: 70px;" />
            <h1 id="h1heading" class="red">Your Access is Denied</h1>
            <p id="pParagraph" class="red">
                You can access this section starting from <span id="spnbegin"></span>upto <span id="spnEnd"></span>
            </p>
            <button type="button" class="btn btn-sign" onclick="WorkingRosterClick()">OK</button>
        </div>
    </div>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Entity</h3>
        </div>
    </div>

    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span>Entity</a>
                                <% } %>
                                <% if (rights.CanExport)
                                   { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>

                                </asp:DropDownList>
                                <% } %>

                                <% if (IsImport)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_ImportPopUpOpen()" class="btn btn-success btn-radius"><span class="btn-icon"></span>Add/Update Import </a>
                                <% } %>
                            </td>

                        </tr>
                    </table>
                </td>
                <td class="gridcellright" style="float: right; vertical-align: top"></td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2" class="relative">
                    <dxe:ASPxGridView ID="gridFinancer" ClientInstanceName="grid" Width="100%"
                        KeyFieldName="cnt_id" runat="server" DataSourceID="EntityServerModeDataSource"
                        AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback" SettingsDataSecurity-AllowEdit="false"
                        SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Settings-HorizontalScrollBarMode="Auto">
                        <%--DataSourceID="gridFinancerDataSource"  --%>
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <ClientSideEvents EndCallback="function(s, e) {
	                              EndCall(s.cpEND);
                            }" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="True" FieldName="EntityCode" Caption="Entity Code" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="Network_Name" Caption="Name" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="ContactPerson" Caption="Contact Person" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="ContactNo" Caption="Contact Number" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Visible="True" FieldName="BRANCH" Caption="Branch" Width="400px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="EMPLOYEE_NAME" Caption="Employee Name" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="AddressType" Caption="Address Type" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="Address1" Caption="Address-1" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="Country" Caption="Country" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="State" Caption="State" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="City" Caption="District" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="Pin_Code" Caption="Pin" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="Statuss" Caption="Status" Width="80px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <%--Mantis Issue 25174--%>
                            <dxe:GridViewDataTextColumn Visible="True" FieldName="ParentCode" Caption="Parent Code" Width="80px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <%--End of Mantis Issue 25174--%>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="CREATE_ON" SortOrder="Descending" Caption="Created On" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="120px">
                                <CellStyle CssClass="gridcellleft"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="CREATE_BY" Caption="Created By" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="MODIFY_ON" Caption="Updated On" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="MODIFY_BY" Caption="Updated By" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="15" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">


                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                        <% if (rights.CanEdit)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnEditButtonClick('<%# Container.KeyValue %>')" title="" class="">
                                            <span class='ico ColorSix'><i class='fa fa-pencil'></i></span><span class='hidden-xs'>Edit</span>
                                        </a>
                                        <% } %>
                                        <% if (rights.CanDelete)
                                           { %>
                                        <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" title="" class="">
                                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                        <% } %>

                                        <% if (rights.CanView)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnViewExecutive('<%# Container.KeyValue %>')" title="" class="pad">
                                            <span class='ico ColorSeven'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span>
                                        </a>
                                        <% } %>
                                        <% if (rights.CanAssignTo)
                                           { %>
                                        <a href="javascript:void(0);" onclick="ClickOnAssignModule('<%# Container.KeyValue %>')" title=""><span class='ico ColorSix'><i class='fa fa-sitemap' aria-hidden='true'></i></span><span class='hidden-xs'>Map Module</span></a>
                                        <%} %>
                                    </div>
                                </DataItemTemplate>
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsText ConfirmDelete="Confirm delete?" />
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <ClientSideEvents RowClick="gridRowclick" />
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="Technician_Report" />

                </td>
                <td>
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </td>
            </tr>
        </table>
        <%--<dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>--%>
        <%--<asp:SqlDataSource ID="gridFinancerDataSource" runat="server" 
            SelectCommand="select h.cnt_id,h.cnt_ucc,h.cnt_firstName,(select branch_description from tbl_master_branch d where d.branch_id=h.cnt_branchId) as branch,* from tbl_master_contact h where cnt_contactType='FI'"></asp:SqlDataSource>--%>
    </div>
    </div>
    <div id="myModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog" style="width: 450px;">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Module List</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <input type="text" id="myInput" onkeyup="myFunction()" placeholder="Search for Module.">
                        <ul id="divModalBody" class="listStyle">
                        </ul>
                    </div>
                    <input type="button" id="btnsatesubmit" title="SUBMIT" value="SUBMIT" class="btn btn-primary" onclick="ModulePushPop()" />
                    <input type="hidden" id="hdnstatelist" class="btn btn-primary" />
                    <input type="hidden" id="hdnEntityId" class="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>


    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ImportPopupModel" runat="server" ClientInstanceName="cImportPopupModel"
            Width="471px" Height="100px" HeaderText="Import Model" PopupHorizontalAlign="Windowcenter"
            PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
            <ContentCollection>
                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <div class="Top clearfix">
                        <div style="padding-top: 5px;" class="col-md-12">
                            <div class="stateDiv" style="padding-top: 5px; width: 100px;">From Company :<span style="color: red;">*</span></div>
                            <div style="padding-top: 5px;">
                                <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60px" Caption=" " />--%>
                                <%--<dxe:ASPxCallbackPanel runat="server" ID="ComponentCompanyPanel" ClientInstanceName="cCompanyComponentPanel" OnCallback="ComponentCompany_Callback">
                                    <PanelCollection>
                                        <dxe:PanelContent runat="server">
                                            <dxe:ASPxGridLookup ID="lookup_company" SelectionMode="Single" runat="server" ClientInstanceName="gridCompanyLookup"
                                                OnDataBinding="lookup_company_DataBinding"
                                                KeyFieldName="Company_Code" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                <Columns>
                                                   


                                                    <dxe:GridViewDataColumn FieldName="Company_Name" Visible="true" VisibleIndex="1" Width="200px" Caption="Company Name">
                                                    </dxe:GridViewDataColumn>
                                                    <dxe:GridViewDataColumn FieldName="DbName" Visible="true" VisibleIndex="2" Width="0" Caption="Data Base Name">
                                                    </dxe:GridViewDataColumn>
                                                    <dxe:GridViewDataColumn FieldName="Company_Code" Visible="true" VisibleIndex="3" Width="0" Caption="Company Name">
                                                    </dxe:GridViewDataColumn>
                                                </Columns>
                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                    <Templates>
                                                        <StatusBar>
                                                            <table class="OptionsTable" style="float: right">
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="False" />
                                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="False" />
                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridBranchLookup" UseSubmitBehavior="False" />

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </StatusBar>
                                                    </Templates>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                    <SettingsPager Mode="ShowPager">
                                                    </SettingsPager>

                                                    <SettingsPager PageSize="20">
                                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                                    </SettingsPager>

                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                </GridViewProperties>

                                            </dxe:ASPxGridLookup>
                                        </dxe:PanelContent>
                                    </PanelCollection>
                                </dxe:ASPxCallbackPanel>--%>

                                <select id="ddlCompany" class="form-control">
                                    <%--Mantis Issue 25174--%>
                                   <%-- <option value="GTPL_INV">GTPL_INV</option>
                                    <option value="GTPL_SRV">GTPL_SRV</option>
                                    <option value="GTPL_STB">GTPL_STB</option>--%>
                                    <option value="BRZ_GTPLINV">GTPL_INV</option>
                                    <option value="GTPL">GTPL_SRV</option>
                                    <option value="BRZ_GTPLSTB">GTPL_STB</option>
                                    <%--End of Mantis Issue 25174--%>
                                </select>

                                <%--  <div id="valid" style="display: none; position: absolute; right: -4px; top: 30px;">
                                            <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" /></div>--%>
                            </div>
                        </div>
                    </div>
                    <div class="ContentDiv">
                        <div class="ScrollDiv"></div>
                        <br style="clear: both;" />
                        <div class="Footer" style="padding-left: 84px;">
                            <div style="float: left;">
                                <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtnSave_States" runat="server"
                                    AutoPostBack="False" Text="Import" CssClass="btn btn-primary">
                                    <ClientSideEvents Click="function (s, e) {btnSave_Model();}" />
                                </dxe:ASPxButton>
                            </div>
                            <div style="">
                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                    <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                </dxe:ASPxButton>
                            </div>
                            <br style="clear: both;" />
                        </div>
                        <br style="clear: both;" />
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <dxe:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
