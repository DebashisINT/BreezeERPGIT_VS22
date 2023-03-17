<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                16-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Building/Warehouses" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_RootBuilding" CodeBehind="RootBuilding.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
        function showhistory(obj) {

            //var URL = 'Contact_Document.aspx?idbldng=' + obj;
            var URL = 'Contact_Document.aspx?idbldng=' + obj;
            //editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Document", "width=1000px,height=400px,center=0,resize=1,top=-1", "recal");
            window.location.href = URL;
            editwin.onclose = function () {
                grid.PerformCallback();
            }

        }
        function Show() {
            var url = "RootBuildingInsertUpdate.aspx?id=ADD";
            // popup.SetContentUrl(url);
            //OnMoreInfoClick(url, "Modify Building Details", '940px', '450px', "Y");
            // popup.Show();
            window.location.href = url;
        }
        function ClickOnMoreInfo(keyValue) {
            var url = 'RootBuildingInsertUpdate.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Building Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function callback() {
            // alert('1');
            buildingGrid.PerformCallback('All');
        }
        function gridcrmCampaignclick(s, e) {
            //alert('hi');
            $('#RootGrid').find('tr').removeClass('rowActive');
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

        // Mantis Issue 24996
        function fn_UserMap(warehouseId) {
            $("#hdnwarehouseId").val(warehouseId);
            var str
            str = { warehouseId: warehouseId }
            var html = "";
            // alert();
            $.ajax({
                type: "POST",
                url: "RootBuilding.aspx/GetUserList",
                data: JSON.stringify(str),
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (responseFromServer) {
                    for (i = 0; i < responseFromServer.d.length; i++) {
                        if (responseFromServer.d[i].IsChecked == true) {
                            html += "<li><input type='checkbox' id=" + responseFromServer.d[i].user_id + "  class='usercheck' onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].user_id + " checked  /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].user_id + " >" + responseFromServer.d[i].user_name + "</label></a></li>";
                        }
                        else {
                            html += "<li><input type='checkbox' id=" + responseFromServer.d[i].user_id + " class='usercheck'  onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].user_id + "   /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].user_id + ">" + responseFromServer.d[i].user_name + "</label></a></li>";
                        }
                    }
                    $("#divModalUserListBody").html(html);
                    $("#myModalUserList").modal('show');
                }
            });
        }

        function myFunction() {
            // Declare variables
            var input, filter, ul, li, a, i, txtValue;
            input = document.getElementById('myInput');
            filter = input.value.toUpperCase();
            ul = document.getElementById("divModalUserListBody");
            li = ul.getElementsByTagName('li');

            // Loop through all list items, and hide those who don't match the search query
            for (i = 0; i < li.length; i++) {
                a = li[i].getElementsByTagName("a")[0];
                txtValue = a.textContent || a.innerText;

                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    li[i].style.display = "";
                } else {
                    li[i].style.display = "none";
                }

            }
        }
        // End of Mantis Issue 24996
    </script>

    <script language="javascript" type="text/javascript">

        function frmOpenNewWindow1(location, v_height, v_weight) {
            var x = (screen.availHeight - v_height) / 2;
            var y = (screen.availWidth - v_weight) / 2
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + x + ",left=" + y + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no'");
        }

        function ShowHideFilter(obj) {
            buildingGrid.PerformCallback(obj);
        } 

        // Mantis Issue 24996
        var Userlist = []
        function UserPushPop() {
            var WarehouseID = $("#hdnwarehouseId").val();
            let a = [];

            $(".usercheckall:checked").each(function () {
                a.push(this.value);
            });

            $(".usercheck:checked").each(function () {
                a.push(this.value);
            });
            var str1
            //  alert(a);

            if(a.length>0)
            {
                str1 = { WarehouseID: WarehouseID, Userlist: a }
                $.ajax({
                    type: "POST",
                    url: "RootBuilding.aspx/UserListSubmit",
                    data: JSON.stringify(str1),
                    contentType: "application/json; charset=utf-8",
                    datatype: "json",
                    success: function (responseFromServer) {
                        // alert(responseFromServer.d)
                        $("#myModalUserList").modal('hide');
                        jAlert('User assigned successfully');
                    }
                });
            }
            else {
                jAlert('No users selected');
            }
            
        }
        // End of Mantis Issue 24996

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                buildingGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                buildingGrid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    buildingGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    buildingGrid.SetWidth(cntWidth);
                }

            });
        });
    </script>

    <%--Mantis Issue 24996--%>
    <style>
        .VerySmall {
            width: 320px;
        }
    #divModalUserListBody{
        padding: 0;
        border: 1px solid #d1d1d1;
        height:350px !important
    }
    #divModalUserListBody > li{
        border-bottom: 1px solid #d1d1d1;
    }
    #divModalUserListBody > li>input{
        margin-right: 8px;
    }
        .listStyle > li {
            list-style-type: none;
            padding: 5px;
        }

        .listStyle {
            height: 450px;
            overflow-y: auto;
        }

            .listStyle > li > input[type="checkbox"] {
                -webkit-transform: translateY(3px);
                -moz-transform: translateY(3px);
                transform: translateY(3px);
            }

        #divModalUserListBody li a:hover:not(.header) {
            background-color: none;
        }

        .modal-backdrop {
            z-index: auto !important;
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
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #RootGrid
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

        #Td1
        {
            vertical-align: baseline;
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
    <%--End of Mantis Issue 24996--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Building/Warehouses</h3>
        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100" width="100%">
            <%--<tr>
                <td class="EHEADER" colspan="2" style="text-align: center;">
                    <strong><span style="color: #000099">Building Details</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                 <% if (rights.CanAdd)
                                        { %>
                                <a href="javascript:void(0);" onclick="Show();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span></a><% } %>
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                            </td>
                            <td id="Td1">
                               <%-- <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                               <% if (rights.CanExport)
                                               { %>
                                 <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                     <asp:ListItem Value="2">XLS</asp:ListItem>
                                     <asp:ListItem Value="3">RTF</asp:ListItem>
                                     <asp:ListItem Value="4">CSV</asp:ListItem>
                        
                                </asp:DropDownList>
                                 <% } %>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="gridcellright" style="float: right; vertical-align: top">&nbsp;
                    <%--<dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                        Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                        ValueType="System.Int32" Width="130px">
                        <Items>
                            <dxe:ListEditItem Text="Select" Value="0" />
                            <dxe:ListEditItem Text="PDF" Value="1" />
                            <dxe:ListEditItem Text="XLS" Value="2" />
                            <dxe:ListEditItem Text="RTF" Value="3" />
                            <dxe:ListEditItem Text="CSV" Value="4" />
                        </Items>
                        <Border BorderColor="black" />
                        <DropDownButton Text="Export">
                        </DropDownButton>
                    </dxe:ASPxComboBox>--%>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2" class="relative">
                    <dxe:ASPxGridView ID="RootGrid" ClientInstanceName="buildingGrid" runat="server"
                        AutoGenerateColumns="False" KeyFieldName="Id" Width="100%"
                        OnHtmlRowCreated="RootGrid_HtmlRowCreated" OnCustomCallback="RootGrid_CustomCallback"  Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        OnRowDeleting="RootGrid_RowDeleting" Settings-HorizontalScrollBarMode="Auto" OnRowCommand="RootGrid_RowCommand" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                       <SettingsSearchPanel Visible="True" Delay="5000" />
                         <Columns>
                             <dxe:GridViewDataTextColumn Caption="Building/Warehouse Code"  FieldName="bui_code" VisibleIndex="0"
                                Width="50%">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Building/Warehouse  Name"  FieldName="Building" VisibleIndex="1"
                                Width="50%">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Caretaker" FieldName="CareTaker"  ReadOnly="True" VisibleIndex="2"
                                Width="30%">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Address" ReadOnly="True"  VisibleIndex="3"
                                Width="40%">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Level" ReadOnly="True"  VisibleIndex="4"
                                Width="300">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="ParentWarehouse"  Caption="Parent Warehouse" VisibleIndex="5" Width="300"  >
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" >
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="6" Width="0"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <%--  <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">Update</a>--%>
                                      <% if (rights.CanEdit)
                                        { %>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title="" class="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><% } %>
                                      <% if (rights.CanDelete)
                                        { %>
                                    <asp:LinkButton ID="btn_delete" runat="server" OnClientClick="return confirm('Confirm delete?');" CommandArgument='<%# Container.KeyValue %>' CommandName="delete" ToolTip="" CssClass="" Font-Underline="false">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span>
                                    </asp:LinkButton><% } %>
                                     <% if (rights.CanView)
                                        { %>
                                    <a href="javascript:void(0);" onclick="showhistory('<%# Container.KeyValue %>')" title="">
                                        <span class='ico ColorThree'><i class='fa fa-file-text'></i></span><span class='hidden-xs'>Add/Update Document(s)</span></a><% } %>
                                    <%--Mantis Issue 24996--%>
                                    <% if (UserSelInWHMast)
                                       { %>
                                        <a href="javascript:void(0);" onclick="fn_UserMap('<%# Container.KeyValue %>')" title="">
                                            <span class='ico deleteColor'><i class='fa fa-sitemap' aria-hidden='true'></i></span><span class='hidden-xs'>Map User</span></a>
                                        <% } %>
                                    <%--End of Mantis Issue 24996--%>
                                        </div>
                                </DataItemTemplate>
                                <HeaderTemplate>
                                    <span>Actions</span>
                                </HeaderTemplate>
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <%-- <dxe:GridViewCommandColumn VisibleIndex="4" ShowDeleteButton="false" Visible="false">
                                <DeleteButton Visible="True">
                                </DeleteButton>
                                <HeaderTemplate>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>--%>
                            <%--<dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="5" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center"
                                Caption="Document">
                            </dxe:GridViewDataTextColumn>--%>
                        </Columns>
                        <ClientSideEvents RowClick="gridcrmCampaignclick" />
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <%--  <SettingsCommandButton>
                            <DeleteButton ButtonType="Image" Image-Url="/assests/images/Delete.png"></DeleteButton>
                        </SettingsCommandButton>--%>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu ="true" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="200px" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px"
                            EditFormColumnCount="1" />
                        <%--<Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>--%>
                        <SettingsText PopupEditFormCaption="Add/Modify CallDisposition" ConfirmDelete="Confirm delete?" />
                        <%--<SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <Templates>
                            <EditForm>
                            </EditForm>
                        </Templates>
                    </dxe:ASPxGridView>
                    <dxe:ASPxPopupControl runat="server" ClientInstanceName="popup" CloseAction="CloseButton"
                        ContentUrl="RootBuilding.aspx.cs" HeaderText="Building Master" Left="150" Top="10"
                        Width="700px" Height="400px" ID="ASPXPopupControl">
                        <ContentCollection>
                            <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                    </dxe:ASPxPopupControl>
                </td>
            </tr>
        </table>
        <%--<asp:SqlDataSource ID="RootSource" ConflictDetection="CompareAllValues" runat="server"
            SelectCommand=""
            DeleteCommand="delete from tbl_master_building where bui_id=@Id">
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="decimal" />
            </DeleteParameters>
        </asp:SqlDataSource>--%>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" FileName="Warehouse">
        </dxe:ASPxGridViewExporter>
    </div>
    </div>
    <%--Mantis Issue 24996--%>
    <div id="myModalUserList" class="modal fade pmsModal" data-backdrop="static" role="dialog">
        <div class="modal-dialog" style="width: 450px;">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">User List</h4>
                </div>
                <div class="modal-body">
                    <div>

                        <input type="text" id="myInput" onkeyup="myFunction()" placeholder="Search for Users.">

                        <ul id="divModalUserListBody" class="listStyle">
                            <%--<input type="checkbox" id="idstate" class="usercheck" /><label id="lblstatename" class="lblstate"></label>--%>
                        </ul>
                    </div>
                    <input type="button" id="btnUsersubmit" title="SUBMIT" value="SUBMIT" class="btn btn-primary" onclick="UserPushPop()" />
                    <input type="hidden" id="hdnUserlist" class="btn btn-primary" />
                    <input type="hidden" id="hdnwarehouseId" class="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>
    <%--End of Mantis Issue 24996--%>
</asp:Content>
