<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                21-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Working Hour Schedule" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_frm_workingShedule" CodeBehind="frm_workingShedule.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dxgvControl_PlasticBlue a {
            color: #5A83D0;
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
            -webkit-appearance: none;
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
        .TableMain100 #GrdHolidays , #WorkingHourGrid
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

        #cmbBranch
        {
            margin-bottom: 5px;
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

    <script language="javascript" type="text/javascript">

        function ClickOnMoreInfo(keyValue) {
            var url = 'WorkinhHourAddEdit.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Working Hour Schedule Details", '940px', '450px', "Y");
            window.location.href = url;

        }
        function OnClickDelete(keyValue) {
            
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        function OnAddButtonClick() {
            var url = 'WorkinhHourAddEdit.aspx?id=Add';
            //OnMoreInfoClick(url, "Add Working Hour Schedule Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function callback() {
            grid.PerformCallback();
        }
        function gridRowclick(s, e) {
            //alert('hi');
            $('#WorkingHourGrid').find('tr').removeClass('rowActive');
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
        function grid_EndCallBack() {
            if (grid.cpDelete != null) {
                jAlert(grid.cpDelete);
                grid.cpDelete = null;
            }
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
    <script>
        function ClickOnAssignModule(RosterId) {
            $("#hdnRosterId").val(RosterId);
            var str
            str = { RosterId: RosterId }
            var html = "";
            // alert();
            $.ajax({
                type: "POST",
                url: "frm_workingShedule.aspx/GetModuleList",
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


        var Modulelist = []
        function ModulePushPop() {
            var RosterId = $("#hdnRosterId").val();
            let a = [];

            $(".statecheckall:checked").each(function () {
                a.push(this.value);
            });

            $(".statecheck:checked").each(function () {
                a.push(this.value);
            });
            var str1
            //  alert(a);

            str1 = { RosterId: RosterId, Modulelist: a }
            $.ajax({
                type: "POST",
                url: "frm_workingShedule.aspx/GetModuleListSubmit",
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Working Roster</h3>
        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100" cellpadding="0px" cellspacing="0px">
            <%--<tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #000099">Working Schedule</span></strong></td>
            </tr>--%>
            <tr>
                <%--Rev 1.0: "mb-10" class add --%>
                <td class="pull-left mb-10"><%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>--%>
                    <% if (rights.CanAdd)
                       { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span>Add New</a><%} %>

                    <asp:DropDownList ID="cmbExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <%--<%} %>--%></td>

            </tr>
            <tr>
                <td class="gridcellcenter relative" colspan="2">
                    <dxe:ASPxGridView ID="WorkingHourGrid" runat="server" KeyFieldName="Id" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        DataSourceID="WorkingHourDataSource" Width="100%" ClientInstanceName="grid" OnCustomCallback="EmployeeGrid_CustomCallback" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="0"
                                Caption="Schedule Name" Width="100%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="" VisibleIndex="8" Width="0" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                        <% if (rights.CanEdit)
                                           { %>
                                        <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title=""><span class='ico ColorSix'><i class='fa fa-info'></i></span><span class='hidden-xs'>Edit</span></a>
                                        <%} %>
                                        <% if (rights.CanDelete)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete">
                                            <img src="../../../assests/images/Delete.png" /></a>
                                        <% } %>
                                        <% if (rights.CanAssignTo)
                                           { %>
                                        <a href="javascript:void(0);" onclick="ClickOnAssignModule('<%# Container.KeyValue %>')" title=""><span class='ico ColorSix'><i class='fa fa-sitemap' aria-hidden='true'></i></span><span class='hidden-xs'>Map Module</span></a>
                                        <%} %>
                                    </div>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />

                                <CellStyle Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderTemplate>
                                    <span></span>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <%--<Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>--%>
                        <ClientSideEvents RowClick="gridRowclick" EndCallback="function (s, e) {grid_EndCallBack();}" />
                        <Settings ShowTitlePanel="false" ShowStatusBar="Hidden" ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                        <SettingsText PopupEditFormCaption="Add/ Modify Working Hour Schedule" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormWidth="900px" PopupEditFormHeight="370px" PopupEditFormHorizontalAlign="Center" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormModal="True" EditFormColumnCount="1"></SettingsEditing>
                        <SettingsBehavior AllowFocusedRow="false" ConfirmDelete="True" />
                        
                        <%--<SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20"
                            PageSize="20">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>

        <asp:SqlDataSource ID="WorkingHourDataSource" runat="server"></asp:SqlDataSource>
        <br />
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
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
                    <input type="hidden" id="hdnRosterId" class="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>


</asp:Content>
