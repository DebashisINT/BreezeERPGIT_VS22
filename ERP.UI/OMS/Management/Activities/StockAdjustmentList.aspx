﻿<%--=======================================================Revision History=====================================================    
    1.0   Pallab    V2.0.38   12-05-2023      26110: Stock Adjustment module design modification & check in small device
=========================================================End Revision History===================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="StockAdjustmentList.aspx.cs" Inherits="ERP.OMS.Management.Activities.StockAdjustmentList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        var isFirstTime = true;
        function OnAddClick() {
            window.location.href = 'StockAdjustmentAdd.aspx?Key=Add';
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 65 && event.altKey == true) {

                if (document.getElementById('AddId'))
                    OnAddClick();
            }
        }
        function AllControlInitilize() {
            if (isFirstTime) {

                if (localStorage.getItem('StockAdjFromDate')) {
                    var fromdatearray = localStorage.getItem('StockAdjFromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('StockAdjToDate')) {
                    var todatearray = localStorage.getItem('StockAdjToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('StockAdjListBranch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('StockAdjListBranch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('StockAdjListBranch'));
                    }

                }
                if ($("#LoadGridData").val() == "ok")
                    updateGridByDate();

                isFirstTime = false;
            }

        }


        function updateGridByDate() {

            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else {

                localStorage.setItem("StockAdjFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("StockAdjToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("StockAdjListBranch", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                cgridAdvanceAdj.Refresh();
            }


        }
        function GridEndCallBack() {
            if (cgridAdvanceAdj.cpReturnMesg) {
                jAlert(cgridAdvanceAdj.cpReturnMesg, "Alert", function () { cgridAdvanceAdj.Refresh(); });
                cgridAdvanceAdj.cpReturnMesg = null;
            }
        }

        function onEditClick(id) {
            window.location.href = 'StockAdjustmentAdd.aspx?Key=' + id;
        }
        function OnViewClick(id) {
            var url = 'StockAdjustmentAdd.aspx?key=' + id + '&req=V';
            window.location.href = url;
        }

      

        function OnClickDelete(id) {
          
            jConfirm("Confirm Delete?", "Alert", function (ret) {
                if (ret)
                { cgridAdvanceAdj.PerformCallback("Del~" + id); }
            });

            //$.ajax({
            //    type: "POST",
            //    url: "StockAdjustmentList.aspx/GetReconValue",
            //    data: "{'id':'" + id + "'}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    async: false,
            //    success: function (msg) {
            //        var status = msg.d;
            //        if (status != "1") {
            //            jConfirm("Confirm Delete?", "Alert", function (ret) {
            //                if (ret)
            //                { cgridAdvanceAdj.PerformCallback("Del~" + id); }
            //            });

            //        }
            //        else if (status == "1") {
            //            jAlert("This Adjustment is created from Stock Reconciliation Module. Cannot Delete from here.");
            //        }
            //    }
            //});

        }

        function gridRowclick(s, e) {
            $('#gridAdvanceAdj').find('tr').removeClass('rowActive');
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
    <style>
        .padTab {
            margin-bottom: 4px;
            margin-top: 8px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
                vertical-align: middle;
            }

                .padTab > tbody > tr > td > label, .padTab > tbody > tr > td > input[type="button"] {
                    margin-bottom: 0 !important;
                    font-size: 14px;
                }

            .padTab > tbody > tr > td {
                font-size: 14px;
            }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cgridAdvanceAdj.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cgridAdvanceAdj.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cgridAdvanceAdj.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cgridAdvanceAdj.SetWidth(cntWidth);
                }

            });
        });
    </script>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        #gridAdvanceAdj {
            max-width: 99% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: auto;
        }

        .calendar-icon
        {
            right: 20px;
        }

        .panel-title h3
        {
            padding-top: 0px !important;
        }
        
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Stock Adjustment </h3>
        </div>
    </div>
        <table class="padTab">
        <tr>
            <td>
                <label>From Date</label></td>
            <%--Rev 1.0: "for-cust-icon" class add --%>
            <td class="for-cust-icon">
                <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </td>
            <td>
                <label>To Date</label>
            </td>
            <%--Rev 1.0: "for-cust-icon" class add --%>
            <td class="for-cust-icon">
                <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </td>
            <td>Unit</td>
            <td>
                <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                </dxe:ASPxComboBox>
            </td>
            <td>
                <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
            </td>

        </tr> 

    </table>
        <div class="form_main">
        <% if (rights.CanAdd)
           { %>
        <a href="javascript:void(0);" onclick="OnAddClick()" id="AddId" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>A</u>dd Adjustment</span> </a>
        <%} %>
        <% if (rights.CanExport)
           { %>
        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <asp:ListItem Value="1">PDF</asp:ListItem>
            <asp:ListItem Value="2">XLS</asp:ListItem>
            <asp:ListItem Value="3">RTF</asp:ListItem>
            <asp:ListItem Value="4">CSV</asp:ListItem>
        </asp:DropDownList>
        <% } %>


         <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
     <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>


        <div class="GridViewArea relative">

            <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridAdvanceAdj" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>
            <dxe:ASPxGridView ID="gridAdvanceAdj" runat="server" KeyFieldName="Stock_ID" AutoGenerateColumns="False"
                Width="100%" ClientInstanceName="cgridAdvanceAdj" SettingsBehavior-AllowFocusedRow="true"
                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" 
                SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false"
                SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" OnSummaryDisplayText="gridAdvanceAdj_SummaryDisplayText"
                OnCustomCallback="gridAdvanceAdj_CustomCallback">

                <SettingsSearchPanel Visible="True" Delay="5000" />
                <Columns>

                    <dxe:GridViewDataTextColumn FieldName="Stock_ID" Visible="false" SortOrder="Descending">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Stock_No" Width="200"
                        VisibleIndex="0" FixedStyle="Left">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataDateColumn Caption="Date" FieldName="Stock_Date" Width="200"
                        VisibleIndex="0" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataDateColumn>

                    <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch_description" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Technician" FieldName="Technician_Name" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Warehouse" FieldName="WarehouseName" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Product" FieldName="Product_Name" Width="200"
                        VisibleIndex="0">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Adjustment Qty" FieldName="EnterAdjustmentQty" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <%--<dxe:GridViewDataTextColumn Caption="Stock In Hand" FieldName="TotalStockInHand" Width="0"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>--%>
                    <dxe:GridViewDataTextColumn Caption="Reason" FieldName="Reason" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn VisibleIndex="0" Caption="Project Name" FieldName="Proj_Name" Width="200">
                        <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Created By" FieldName="CreatedBy" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="LastUpdateOn" Settings-AllowAutoFilter="False"
                        Caption="Last Update On" Width="200">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Last Updated By" FieldName="LastUpdatedBy" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                               <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" class="" title="" onclick="onEditClick('<%# Container.KeyValue %>')" style='<%#Eval("Editlock")%>'>
                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                            </a>
                                              <%} %>

                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="" id="a_delete" style='<%#Eval("Deletelock")%>'>
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                            <%} %>
                                </div>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <HeaderTemplate><span></span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>

                    </dxe:GridViewDataTextColumn>


                </Columns>

                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                </SettingsPager>

                <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="EnterAdjustmentQty" SummaryType="Sum" />
                </TotalSummary>
                <SettingsLoadingPanel Text="Please Wait..." />
                <ClientSideEvents EndCallback="GridEndCallBack" RowClick="gridRowclick" />

            </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="v_StockAdjustmentList" />
            <asp:HiddenField ID="hfIsFilter" runat="server" />
            <asp:HiddenField ID="hfFromDate" runat="server" />
            <asp:HiddenField ID="hfToDate" runat="server" />
            <asp:HiddenField ID="hfBranchID" runat="server" />
            <asp:HiddenField ID="hiddenedit" runat="server" />
        </div>
    </div>
    </div>
    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
<asp:HiddenField ID="hdnLockToDateedit" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />


</asp:Content>
