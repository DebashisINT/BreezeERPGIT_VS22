
<%--**********************************************************************************************************************************
 1.0    v2.0.22     Sanchita	21/04/2021		New check box "Dont Consider in Profitability Report" introduced module Warehouse wise Stock Journal .
												checkbox "NotInProfReport" added.
												Refer:  23986
 2.0    v2.0.38     Pallab	    18/05/2023		Add Warehouse Wise Stock Journal module design modification & check in small device
												Refer:  26115
 ***********************************************************************************************************************************--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="WarehousewiseStockJournalAdd.aspx.cs" Inherits="ERP.OMS.Management.Activities.WarehousewiseStockJournalAdd" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc1" TagName="UOMConversionControl" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
     <script src="JS/SearchPopupDatatable.js"></script>
    <%--<script src="JS/SearchPopup.js?v=0.03"></script>--%>
     <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <script src="JS/WarehousewiseStockJournalJS.js?v=12.02"></script>
   
    <style type="text/css">
        /*#grid_DXMainTable > tbody > tr > td:last-child, #productLookUp_DDD_gv_DXMainTable > tbody > tr > td:nth-child(2) {
            display: none !important;
        }*/
        /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
         .eqTble > tbody > tr > td {
            padding: 0 7px;
            vertical-align: top;
        }
         /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        #grid_DXStatus span > a, #gridDEstination_DXStatus a, #grid_DXStatus a, #gridDEstination_DXStatus span > a {
            display: none;
        }

        .HeaderStyle {
            background-color: #180771d9;
            color: #f5f5f5;
        }

        .lblWarhouse {
            font-size: 10px !important;
            font-weight: 600;
            width: 100%;
            display: block;
            margin-bottom: 5px;
            color: #e29437;
            padding: 6px 2px;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff;
        }

        .iconBranchTo {
            position: absolute;
            right: -17px;
            top: 5px;
        }

        .iconBranch {
            position: absolute;
            right: -17px;
            top: 5px;
        }

        .iconNumberScheme {
            position: absolute;
            right: -17px;
            top: 5px;
        }

        .bac {
            background: #c2d8e6;
            margin: 10px 0;
            padding: 2px 15px;
            border-radius: 5px;
        }

        .greyd {
            background: #ececec;
            margin: 10px 0;
            padding: 0px 15px;
            border-radius: 5px;
        }

        .newLbl .lblHolder table tr:first-child td {
            background: #2bb1bf;
        }

        table.pad > tbody > tr > td {
            padding: 0px 10px;
        }

        section.rds {
            margin-top: 25px;
            border: 1px solid #ccc;
            padding: 3px 15px;
        }

        span.fieldsettype {
            background: #1671b7;
            padding: 8px 10px;
            color: #fff;
            position: relative;
            top: -10px;
            z-index: 5;
        }

            span.fieldsettype::before {
                content: "";
                border-left: 9px solid transparent;
                border-right: 9px solid transparent;
                border-bottom: 13px solid #184d75;
                position: absolute;
                right: -9px;
                z-index: -1;
            }

        .horizontallblHolder {
            height: auto;
            border: 1px solid #12a79b;
            border-radius: 3px;
            overflow: hidden;
        }

            .horizontallblHolder > table > tbody > tr > td {
                padding: 8px 10px;
                background: #ffffff;
                background: -moz-linear-gradient(top, #ffffff 0%, #f3f3f3 50%, #ededed 51%, #ffffff 100%);
                background: -webkit-linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
                background: linear-gradient(to bottom, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ffffff',GradientType=0 );
            }

                .horizontallblHolder > table > tbody > tr > td:first-child {
                    background: #12a79b;
                    color: #fff;
                }

                .horizontallblHolder > table > tbody > tr > td:last-child {
                    font-weight: 500;
                    text-transform: uppercase;
                    color: #121212;
                }


        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .popover {
            z-index: 999999;
            max-width: 350px;
        }

            .popover .popover-title {
                margin-top: 0 !important;
                background: #465b9d;
                color: #fff;
            }

        .pdLeft15 {
            padding-left: 15px;
        }

        .mTop {
            margin-top: 10px;
        }

        .mLeft {
            margin-left: 15px;
        }

        #gridDEstination_DXMainTable > tbody > tr > td:nth-child(18) {
            display: none !important;
        }

        #grid_DXMainTable > tbody > tr > td:nth-child(18) {
            display: none !important;
        }

        #pageheaderContent {
            margin-right: 50px;
        }
    </style>
    <script>


        function clookup_Project_LostFocus() {
            //grid.batchEditApi.StartEdit(-1, 2);

            var projID = clookup_Project.GetValue();
            if (projID == null || projID == "") {
                $("#ddlHierarchy").val(0);
            }
        }

        function ProjectValueChange(s, e) {

            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'WarehousewiseStockJournalAdd.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
        }


        $(document).ready(function () {
            $("#expandgridDEstination").click(function (e) {
                e.preventDefault();

                var $this = $(this);

                if ($this.children('i').hasClass('fa-expand')) {
                    $this.removeClass('hovered half').addClass('full');
                    $this.attr('title', 'Minimize Grid');
                    $this.children('i').removeClass('fa-expand');
                    $this.children('i').addClass('fa-arrows-alt');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
                    var cntWidth = $(this).parent('.makeFullscreen').width();
                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;


                    gridDEstination.SetHeight(browserHeight - 150);
                    gridDEstination.SetWidth(cntWidth);
                }
                else if ($this.children('i').hasClass('fa-arrows-alt')) {
                    $this.children('i').removeClass('fa-arrows-alt');
                    $this.removeClass('full').addClass('hovered half');
                    $this.attr('title', 'Maximize Grid');
                    $this.children('i').addClass('fa-expand');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');

                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;
                    gridDEstination.SetHeight(180);

                    var cntWidth = $this.parent('.makeFullscreen').width();
                    gridDEstination.SetWidth(cntWidth);
                }
            });
            $("#expandgrid").click(function (e) {
                e.preventDefault();

                var $this = $(this);

                if ($this.children('i').hasClass('fa-expand')) {
                    $this.removeClass('hovered half').addClass('full');
                    $this.attr('title', 'Minimize Grid');
                    $this.children('i').removeClass('fa-expand');
                    $this.children('i').addClass('fa-arrows-alt');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
                    var cntWidth = $(this).parent('.makeFullscreen').width();
                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;


                    grid.SetHeight(browserHeight - 150);
                    grid.SetWidth(cntWidth);
                }
                else if ($this.children('i').hasClass('fa-arrows-alt')) {
                    $this.children('i').removeClass('fa-arrows-alt');
                    $this.removeClass('full').addClass('hovered half');
                    $this.attr('title', 'Maximize Grid');
                    $this.children('i').addClass('fa-expand');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');

                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;
                    grid.SetHeight(200);

                    var cntWidth = $this.parent('.makeFullscreen').width();
                    grid.SetWidth(cntWidth);
                }
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                gridDEstination.SetWidth(cntWidth);
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                gridDEstination.SetWidth(cntWidth);
                grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    gridDEstination.SetWidth(cntWidth);
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    gridDEstination.SetWidth(cntWidth);
                    grid.SetWidth(cntWidth);
                }

            });
        });
        
    </script>

    <%--Rev 2.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dt_BTOut , #dt_refCreditNoteDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dt_BTOut_B-1 ,
        #dt_refCreditNoteDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dt_BTOut_B-1 #dt_BTOut_B-1Img ,
        #dt_refCreditNoteDt_B-1 #dt_refCreditNoteDt_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 18px;
                bottom: 6px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 0 !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 26px;
            right: 13px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
    background: #42b39e !important;
}

        #ddlInventory
        {
                -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-2 > label, .col-md-2 > span, .col-md-1 > label, .col-md-1 > span
        {
            margin-top: 0px;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
        }

        #gridDEstination
        {
            width:99% !important;
        }

        #txtEntity , #txtCustName
        {
            width: 100%;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12
            {
                 padding-right: 10px;
                 padding-left: 10px;
            }
            .simple-select::after
        {
                top: 26px;
            right: 8px;
        }
            .calendar-icon
        {
                right: 14px;
                bottom: 6px;
        }
        }

    </style>
    <%--Rev end 2.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
    <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
    <uc1:UOMConversionControl runat="server" ID="UOMConversionControl" />
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <%--Rev 2.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <asp:Label ID="lblHeading" runat="server" Text="Add Warehouse Wise Stock Journal"></asp:Label>
        </h3>
        <div id="pageheaderContent" class=" pull-right  content horizontal-images">
            <div class="Top clearfix">
                <ul>

                    <li style="cursor: pointer">
                        <div class="lblHolder" id="" onclick="">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Daily Alt Stock Quantity</td>
                                    </tr>
                                    <tr>
                                        <td style="color: blue">
                                            <asp:Label ID="lblDailyAltStkQty" runat="server" Text="0.0"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>
                    <li style="cursor: pointer">
                        <div class="lblHolder" id="" onclick="">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Daily Stock Quantity</td>
                                    </tr>
                                    <tr>
                                        <td style="color: blue">
                                            <asp:Label ID="lblDailyStkQty" runat="server" Text="0.0"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href="WarehousewiseStockJournalList.aspx"><i class="fa fa-times"></i></a></div>
        <div class="form_main">

        <div class="boxBorder">
            <div class="styledBox mTop5">
                <div class="row">
                    <div class="col-md-2 relative">
                        <label class="darkLabel mTop5">Numbering Scheme<span style="color: red">*</span></label>
                        <div class="relative">
                            <dxe:ASPxComboBox ID="CmbScheme" ClientInstanceName="cCmbScheme"
                                SelectedIndex="0" EnableCallbackMode="false"
                                TextField="SchemaName" ValueField="ID"
                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                <ClientSideEvents ValueChanged="CmbScheme_ValueChange"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                            <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                    <div class="col-md-2">
                        <label class="darkLabel mTop5">Document No</label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" MaxLength="16" Text="Auto" ClientEnabled="false" Width="100%">
                            </dxe:ASPxTextBox>
                            <span id="MandatoryAdjNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label class="darkLabel mTop5">Date<span style="color: red">*</span></label>
                        <div class="relative">
                            <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom" AllowNull="false"
                                Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                <ButtonStyle Width="13px"></ButtonStyle>
                                <ClientSideEvents GotFocus="function(s,e){cdtTDate.ShowDropDown();}"  LostFocus="function(s, e) { SetLostFocusonDemand(e)}"></ClientSideEvents>
                            </dxe:ASPxDateEdit>
                            <span id="MandatoryDate" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                        <%--Rev 2.0--%>
                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                        <%--Rev end 2.0--%>
                    </div>
                    <%--Rev 2.0: "simple-select" class add --%>
                    <div class="col-md-2 simple-select">
                        <label class="darkLabel mTop5">From Unit<span style="color: red">*</span></label>
                        <div class="relative">
                            <asp:DropDownList ID="ddlBranch" runat="server" onchange="ddlBranch_SelectedIndexChanged()"
                                DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                            </asp:DropDownList>
                            <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                    <%--Rev 2.0: "simple-select" class add --%>
                    <div class="col-md-2 simple-select">
                        <label class="darkLabel mTop5">To Unit<span style="color: red">*</span></label>
                        <div class="relative">
                            <asp:DropDownList ID="ddlBranchTo" runat="server"
                                DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                            </asp:DropDownList>
                            <span id="MandatoryBranchTo" class="iconBranchTo pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                    <div style="clear: both;"></div>
                    <div class="col-md-2">
                        <label class="darkLabel mTop5">Transportation Mode</label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtTransportationMode" ClientInstanceName="ctxtTransportationMode" Width="100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label class="darkLabel mTop5">Vehicle No</label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtVehicleNo" ClientInstanceName="ctxtVehicleNo" Width="100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <label class="darkLabel">Remarks</label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txRemarks" ClientInstanceName="ctxRemarks" MaxLength="500" Width="100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>


                    <div class="col-md-2 lblmTop8">
                        <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                        </dxe:ASPxLabel>
                        <%-- <label id="lblProject" runat="server">Project</label>--%>
                        <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataStock"
                            KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                            <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />

                        </dxe:ASPxGridLookup>
                        <dx:LinqServerModeDataSource ID="EntityServerModeDataStock" runat="server" OnSelecting="EntityServerModeDataStock_Selecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                    </div>
                    <div class="col-md-2 lblmTop8">
                        <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                        </dxe:ASPxLabel>
                        <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                        </asp:DropDownList>
                    </div>
                    <div style="clear: both;"></div>
                    <div class="col-md-2" id="DivTechnician" runat="server">
                        <label class="darkLabel mTop5">Technician</label>
                        <div>
                            <dxe:ASPxComboBox ID="ccmTechnician" runat="server" ClientInstanceName="cccmTechnician"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                     <%--Rev 1.0 --%> 
                    <div class="col-md-4">
                        <label class="darkLabel mTop5"> &nbsp </label>
                        <div>
                            <label class="checkbox-inline">
                                <asp:CheckBox ID="chkNotInProfReport" runat="server" ></asp:CheckBox>
                                <span style="margin: -2px 0; display: block">
                                    <dxe:ASPxLabel ID="lblNotInProfReport" runat="server" Text="Don't Consider in Profitability Report">
                                    </dxe:ASPxLabel>
                                </span>
                            </label>
                        </div>
                    </div>
                    <%--End of Rev 1.0--%>
                    <div style="clear: both;"></div>
                    <div class="col-md-2 hide">
                        <label class="mTop5">Source Warehouse<span style="color: red">*</span></label>
                        <div class="relative">
                            <dxe:ASPxComboBox ID="cmbSourceWarehouse" runat="server" ClientInstanceName="ccmbSourceWarehouse"
                                Width="100%">
                                <ClientSideEvents ValueChanged="SourceWH_ValueChange"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                            <span id="MandatoryWarehouse" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            <dxe:ASPxLabel runat="server" ID="lblSourceWHAddress" ClientInstanceName="clblSourceWHAddress" Text="" CssClass="text-muted lblWarhouse"></dxe:ASPxLabel>
                        </div>
                    </div>

                    <div class="col-md-2 hide">
                        <label class="mTop5">Destination Warehouse<span style="color: red">*</span></label>
                        <div class="relative">
                            <dxe:ASPxComboBox ID="cmbDestWarehouse" runat="server" ClientInstanceName="ccmbDestWarehouse"
                                Width="100%">
                                <ClientSideEvents ValueChanged="DestWH_ValueChange"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                            <span id="MandatorycmbDestWarehouse" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            <dxe:ASPxLabel runat="server" ID="lblDestWHAddress" ClientInstanceName="clblDestWHAddress" Text="" CssClass="text-muted lblWarhouse"></dxe:ASPxLabel>
                        </div>
                    </div>
                </div>
            </div>

            <div style="clear: both;"></div>

            <div style="clear: both;"></div>
            <h4 style="font-size: 14px;">Stock In</h4>
            <div class="row mTop5">
                <div class="col-md-12 mTop5">
                    <div class="makeFullscreen ">
                        <span class="fullScreenTitle">Stock In</span>
                        <span class="makeFullscreen-icon half hovered " data-instance="gridDEstination" title="Maximize Grid" id="expandgridDEstination"><i class="fa fa-expand"></i></span>
                        <dxe:ASPxGridView runat="server" OnBatchUpdate="gridDEstination_BatchUpdate" KeyFieldName="ActualDestSL" ClientInstanceName="gridDEstination" ID="gridDEstination"
                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                            OnCellEditorInitialize="gridDEstination_CellEditorInitialize" OnCustomJSProperties="gridDEstination_CustomJSProperties"
                            Settings-ShowFooter="false" OnDataBinding="gridDEstination_DataBinding"
                            OnRowInserting="gridDEstination_RowInserting" OnRowUpdating="gridDEstination_RowUpdating" OnRowDeleting="gridDEstination_RowDeleting"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollableHeight="130" Settings-VerticalScrollBarMode="Visible" Settings-HorizontalScrollBarMode="Visible">                           
                            <SettingsPager Visible="false"></SettingsPager>
                             <%--Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                            <SettingsBehavior AllowDragDrop="False" AllowSort="False" />
                            <%--Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30px" VisibleIndex="0"
                                    Caption=" ">
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDeleteDest" Image-Url="/assests/images/crs.png">
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>
                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="30px">
                                    <PropertiesTextEdit>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataButtonEditColumn FieldName="ProductNameDest" Caption="Product" VisibleIndex="2" Width="150px" ReadOnly="True">
                                    <PropertiesButtonEdit>
                                        <ClientSideEvents ButtonClick="DestProductButnClick" KeyDown="DestProductKeyDown" />
                                        <Buttons>
                                            <dxe:EditButton Text="..." Width="20px">
                                            </dxe:EditButton>
                                        </Buttons>
                                    </PropertiesButtonEdit>
                                </dxe:GridViewDataButtonEditColumn>
                                <dxe:GridViewDataTextColumn FieldName="DestProductID" Caption="" VisibleIndex="22" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <%--Batch Product Popup End--%>
                                <dxe:GridViewDataTextColumn FieldName="DestDiscription" Caption="Description" VisibleIndex="3" Width="150px" ReadOnly="true">
                                    <CellStyle Wrap="True"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataButtonEditColumn FieldName="DestinationWarehouse" Caption="Destination Warehouse" VisibleIndex="4" Width="150px" ReadOnly="True">
                                    <PropertiesButtonEdit>
                                        <ClientSideEvents ButtonClick="DestinationWarehouseButnClick" KeyDown="DestinationWarehouseKeyDown" />
                                        <Buttons>
                                            <dxe:EditButton Text="..." Width="20px">
                                            </dxe:EditButton>
                                        </Buttons>
                                    </PropertiesButtonEdit>
                                </dxe:GridViewDataButtonEditColumn>
                                <dxe:GridViewDataTextColumn FieldName="DestinationWarehouseID" Caption="" VisibleIndex="23" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="AvlStkDestWH" Caption="Avl. Stock" VisibleIndex="5" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                        <%--<MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />  --%>
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                 <%--Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <dxe:GridViewDataTextColumn FieldName="DestQuantity" Caption="Quantity" VisibleIndex="6" Width="80px" HeaderStyle-HorizontalAlign="Right">
                                <%--<dxe:GridViewDataTextColumn FieldName="DestQuantity" Caption="DestQuantity" VisibleIndex="6" Width="80px" HeaderStyle-HorizontalAlign="Right">--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                        <ClientSideEvents LostFocus="DestQuantityTextChange" GotFocus="DestQuantityTextChangeGotFocus" />
                                        <%--<ClientSideEvents  GotFocus="QuantityTextChangeGotFocus" />--%>
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="DestUOM" Caption="UOM" VisibleIndex="7" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                    <PropertiesTextEdit>
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <%--Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <dxe:GridViewCommandColumn VisibleIndex="8" Caption="Multi UOM" Width="60px">
                                   <CustomButtons>
                                       <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                       </dxe:GridViewCommandColumnCustomButton>
                                   </CustomButtons>
                                 </dxe:GridViewCommandColumn>
                                <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="Dest_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="9" Width="5%" ReadOnly="true">
                                    <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                        <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="DestQuantityTextChange" />
                                        <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                        <Style HorizontalAlign="Right">
                                        </Style>
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                 </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="Dest_AltUOM" ReadOnly="true" VisibleIndex="10" Width="5%" >
                                  <PropertiesTextEdit>
                                  </PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn>
                                <%--<dxe:GridViewCommandColumn Width="70px" VisibleIndex="8" Caption="Stk Details">--%>
                                    <dxe:GridViewCommandColumn Width="70px" VisibleIndex="11" Caption="Stk Details">
                                    <%--Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouseDest" Image-Url="/assests/images/warehouse.png">
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>
                                <%--Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="DestRate" Caption="Rate" VisibleIndex="9" Width="80px" HeaderStyle-HorizontalAlign="Right">--%>
                                    <dxe:GridViewDataTextColumn FieldName="DestRate" Caption="Rate" VisibleIndex="12" Width="80px" HeaderStyle-HorizontalAlign="Right">
                                        <%--Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.000">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />                                        
                                        <ClientSideEvents LostFocus="DestRateTextChange" />
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                 <%--Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="DestValue" Caption="Value" VisibleIndex="10" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">--%>
                                <dxe:GridViewDataTextColumn FieldName="DestValue" Caption="Value" VisibleIndex="13" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                     <%--Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right">                                        
                                        <ClientSideEvents LostFocus="DestAmountTextChange" />
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <%--Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="DestRemarks" Caption="Remarks" VisibleIndex="11" Width="150px">--%>
                                <dxe:GridViewDataTextColumn FieldName="DestRemarks" Caption="Remarks" VisibleIndex="14" Width="150px">
                                    <%--Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <PropertiesTextEdit>
                                    </PropertiesTextEdit>
                                    <CellStyle></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ActualDestSL" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <%--Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <%--<dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="60px" VisibleIndex="12" Caption="Add New">--%>
                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="60px" VisibleIndex="15" Caption="Add New">
                                    <%--Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRowDest" Image-Url="/assests/images/add.png">
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>
                                 <%--Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="DestPackingQty" Caption="hidden DestPackingQty" VisibleIndex="20" ReadOnly="True" Width="0"
                                    EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">--%>
                                <dxe:GridViewDataTextColumn FieldName="DestPackingQty" Caption="hidden DestPackingQty" VisibleIndex="24" ReadOnly="True" Width="0"
                                    EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                    <%--Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.000">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                            </Columns>

                            <ClientSideEvents RowClick="GetVisibleIndexDest" BatchEditStartEditing="GetVisibleIndexDest" CustomButtonClick="gridDEstinationCustomButtonClick" EndCallback="GridEndCallBackgridDEstination"/>
                            <SettingsDataSecurity AllowEdit="true" />
                            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                            </SettingsEditing>
                        </dxe:ASPxGridView>
                    </div>
                </div>
            </div>

            <div style="clear: both;"></div>
            <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px 0; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                <ul>
                    <li class="clsbnrLblTotalQty">
                        <div class="horizontallblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblDesttotQty" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblDestTotalQty" />
                                        </td>
                                        <td>
                                            <dxe:ASPxLabel ID="LblDestTotalQty" runat="server" Text="0.00" ClientInstanceName="cbnrLblDestTotalQty" />
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>
                    </li>
                    <%--Rev work start 17.05.2022  Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                    <li class="clsbnrLblTotalQty">
                    <li class="clsbnrLblTotalQty" id="lbl1" runat="server">
                    <%--Rev work close 17.05.2022  Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                        <div class="horizontallblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Total Alt. Quantity" ClientInstanceName="cbnrLblDestAltTotalQty" />
                                        </td>
                                        <td>
                                            <dxe:ASPxLabel ID="LblDestAltTotalQty" runat="server" Text="0.00" ClientInstanceName="cbnrLblDestAltTotalQty" />
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>
                    </li>
                    <li class="clsbnrLblTotalQty">
                        <div class="horizontallblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="Total Value" ClientInstanceName="cLblDestTotalQty" />
                                        </td>
                                        <td>
                                            <dxe:ASPxLabel ID="bnrLblDestTotalValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblDestTotalValue" />
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>
                    </li>
                </ul>
            </div>
            <div class="clear"></div>
            <h4 style="font-size: 14px;">Stock Out</h4>
            <div class="row mTop5">
                <div class="col-md-12 mTop5">
                    <%--<dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="ActualSL" ClientInstanceName="grid" ID="grid"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                        OnCellEditorInitialize="grid_CellEditorInitialize" OnCustomJSProperties="grid_CustomJSProperties"
                        Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                        OnRowInserting="grid_RowInserting" OnRowUpdating="grid_RowUpdating" OnRowDeleting="grid_RowDeleting" 
                        OnHtmlRowPrepared="grid_HtmlRowPrepared" 
                         SettingsPager-Mode="ShowAllRecords" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="120" Settings-VerticalScrollBarMode="Visible" >
                        <SettingsPager Visible="false"></SettingsPager>--%>
                    <div class="makeFullscreen ">
                        <span class="fullScreenTitle">Stock Out</span>
                        <span class="makeFullscreen-icon half hovered " data-instance="grid" title="Maximize Grid" id="expandgrid"><i class="fa fa-expand"></i></span>
                        <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="ActualSL" ClientInstanceName="grid" ID="grid"
                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                            OnCellEditorInitialize="grid_CellEditorInitialize" OnCustomJSProperties="grid_CustomJSProperties"
                            Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                            OnRowInserting="grid_RowInserting" OnRowUpdating="grid_RowUpdating" OnRowDeleting="grid_RowDeleting" OnHtmlRowPrepared="grid_HtmlRowPrepared"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollableHeight="130" Settings-VerticalScrollBarMode="Visible" Settings-HorizontalScrollBarMode="Auto">
                            <Columns>
                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30px" VisibleIndex="0"
                                    Caption=" ">
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>
                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="30px">
                                    <PropertiesTextEdit>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--Batch Product Popup Start--%>

                                <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="2" Width="150px" ReadOnly="True">
                                    <PropertiesButtonEdit>
                                        <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                        <Buttons>
                                            <dxe:EditButton Text="..." Width="20px">
                                            </dxe:EditButton>
                                        </Buttons>
                                    </PropertiesButtonEdit>
                                </dxe:GridViewDataButtonEditColumn>
                                <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" CellStyle-CssClass="hideContent" VisibleIndex="19" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <%--Batch Product Popup End--%>
                                <dxe:GridViewDataTextColumn FieldName="Discription" Caption="Description" VisibleIndex="3" Width="150px" ReadOnly="true">
                                    <CellStyle Wrap="True"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataButtonEditColumn FieldName="SourceWarehouse" Caption="Source Warehouse" VisibleIndex="4" Width="150px" ReadOnly="True">
                                    <PropertiesButtonEdit>
                                        <ClientSideEvents ButtonClick="SourceWarehouseButnClick" KeyDown="SourceWarehouseKeyDown" />
                                        <Buttons>
                                            <dxe:EditButton Text="..." Width="20px">
                                            </dxe:EditButton>
                                        </Buttons>
                                    </PropertiesButtonEdit>
                                </dxe:GridViewDataButtonEditColumn>
                                <dxe:GridViewDataTextColumn FieldName="SourceWarehouseID" Caption="hidden Field Id" VisibleIndex="20" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="AvlStkSourceWH" Caption="Avl. Stock" VisibleIndex="5" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                        <%--  <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />--%>
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TransferQuantity" Caption="Quantity" VisibleIndex="6" Width="80px" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                        <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityTextChangeGotFocus" />
                                        <%--<ClientSideEvents  GotFocus="QuantityTextChangeGotFocus" />--%>
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="SaleUOM" Caption="UOM" VisibleIndex="7" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                    <PropertiesTextEdit>
                                    </PropertiesTextEdit>
                                    <CellStyle Wrap="True"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <%--Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <dxe:GridViewCommandColumn VisibleIndex="8" Caption="Multi UOM" Width="65px">
                                   <CustomButtons>
                                       <dxe:GridViewCommandColumnCustomButton Text=" " ID="SourceCustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                       </dxe:GridViewCommandColumnCustomButton>
                                   </CustomButtons>
                                 </dxe:GridViewCommandColumn>
                                <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="Source_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="9" Width="5%" ReadOnly="true">
                                    <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                        <ClientSideEvents GotFocus="QuantityGotFocus_Source" LostFocus="QuantityTextChange" />
                                        <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                        <Style HorizontalAlign="Right">
                                        </Style>
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                 </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="Source_AltUOM" ReadOnly="true" VisibleIndex="10" Width="5%" >
                                  <PropertiesTextEdit>
                                  </PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn>
                               
                                <%--<dxe:GridViewCommandColumn Width="80px" VisibleIndex="8" Caption="Stk Details">--%>
                                <dxe:GridViewCommandColumn Width="80px" VisibleIndex="11" Caption="Stk Details">
                                    <%-- Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>
                                <%-- Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="9" Width="80px" HeaderStyle-HorizontalAlign="Right">--%>
                                <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="12" Width="80px" HeaderStyle-HorizontalAlign="Right">
                                    <%-- Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.000">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                        <ClientSideEvents LostFocus="RateTextChange" />
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <%-- Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                               <%-- <dxe:GridViewDataTextColumn FieldName="Value" Caption="Value" VisibleIndex="10" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">--%>
                                 <dxe:GridViewDataTextColumn FieldName="Value" Caption="Value" VisibleIndex="13" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                     <%-- Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                        <%--Rev Rajdip make three decimal to two decimal--%>
                                        <ClientSideEvents LostFocus="AmountTextChange" />
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <%-- Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="11" Width="150px">--%>
                                <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="14" Width="150px">
                                    <%-- Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <PropertiesTextEdit>
                                    </PropertiesTextEdit>
                                    <CellStyle></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ActualSL" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <%-- Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <%--<dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="60px" VisibleIndex="12" Caption="Add New">--%>
                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="60px" VisibleIndex="15" Caption="Add New">
                                    <%-- Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png">
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>
                                <%-- Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="PackingQty" Caption="hidden Packingqty" VisibleIndex="20" ReadOnly="True" Width="0"
                                    EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">--%>
                                <dxe:GridViewDataTextColumn FieldName="PackingQty" Caption="hidden Packingqty" VisibleIndex="21" ReadOnly="True" Width="0"
                                    EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                    <%-- Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.000">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                            </Columns>

                            <ClientSideEvents RowClick="GetVisibleIndex" BatchEditStartEditing="GetVisibleIndex" CustomButtonClick="gridCustomButtonClick" EndCallback="GridEndCallBack" />
                            <SettingsDataSecurity AllowEdit="true" />
                            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                            </SettingsEditing>
                        </dxe:ASPxGridView>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
            <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px 0; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                <ul>
                    <li class="clsbnrLblTotalQty">
                        <div class="horizontallblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Total Quantity" ClientInstanceName="cLblTotalQty" />
                                        </td>
                                        <td>
                                            <dxe:ASPxLabel ID="LblTotalQty" runat="server" Text="0.00" ClientInstanceName="cbnrLblTotalQty" />
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>
                    </li>
                    <%--Rev work start 17.05.2022  Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                    <%--<li class="clsbnrLblTotalQty">--%>
                    <li class="clsbnrLblTotalQty" runat="server" id="lbl2">
<%--Rev work close 17.05.2022  Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
                        <div class="horizontallblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Total Alt. Quantity" ClientInstanceName="cbnrLblAltTotalQty" />
                                        </td>
                                        <td>
                                            <dxe:ASPxLabel ID="LblAltTotalQty" runat="server" Text="0.00" ClientInstanceName="cbnrLblAltTotalQty" />
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>
                    </li>
                    <li class="clsbnrLblTotalQty">
                        <div class="horizontallblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Total Value" ClientInstanceName="cLblTotalValue" />
                                        </td>
                                        <td>
                                            <dxe:ASPxLabel ID="bnrLblTotalValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblTotalValue" />
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>
                    </li>

                </ul>
            </div>
            <div class="clear"></div>
            <div class="row">
                <div class="col-md-12 mTop5">
                    <table style="float: left;" id="tblBtnSavePanel">
                        <tr>
                            <td style="padding: 5px 0px;">
                                <span id="tdSaveButton" runat="server">
                                    <%--<% if (rights.CanAdd)
                                       { %>--%>
                                    <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" ClientVisible="false"
                                        CssClass="btn btn-success" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <%--  <%} %>--%>
                                </span>
                                <span id="Span1" runat="server">
                                    <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" ClientVisible="false"
                                        CssClass="btn btn-primary" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                                    </dxe:ASPxButton>
                                </span>
                            </td>

                        </tr>
                    </table>
                </div>
            </div>
        </div>







    </div>
    </div>
    <div class="clear"></div>
    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField ID="hdAdjustmentId" runat="server" />
    <asp:HiddenField ID="HiddenSaveButton" runat="server" />
    <asp:HiddenField ID="HiddenRowCount" runat="server" />
    <asp:HiddenField ID="DestHiddenRowCount" runat="server" />
    <asp:HiddenField ID="hdfProductID" runat="server" />
    <asp:HiddenField ID="hdfProductType" runat="server" />
    <asp:HiddenField ID="hdfProductSerialID" runat="server" />
    <asp:HiddenField ID="hdnProductQuantity" runat="server" />
    <asp:HiddenField ID="hdnProductTypeAssign" runat="server" />

    <asp:HiddenField ID="hdfProductTypeDest" runat="server" />
    <asp:HiddenField ID="hdfProductIDdest" runat="server" />
    <asp:HiddenField ID="hdfProductSerialIDDest" runat="server" />
    <asp:HiddenField ID="hdnProductQuantityDest" runat="server" />

    <asp:HiddenField ID="hdnStockOutTotalQty" runat="server" />
    <asp:HiddenField ID="hdnStockOutAltTotalQty" runat="server" />

    <asp:HiddenField ID="hdnStockInTotalQty" runat="server" />
    <asp:HiddenField ID="hdnStockInAltTotalQty" runat="server" />

    <asp:HiddenField ID="hdnWHtype" runat="server" />
    <asp:HiddenField ID="hdnStockNeg" runat="server" />
    <asp:HiddenField ID="hdnDestStockNeg" runat="server" />

    <asp:HiddenField runat="server" ID="hdnWSTAutoPrint" Value="" />
   <%-- Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
     <asp:HiddenField runat="server" ID="HiddenField1" />
        <asp:HiddenField runat="server" ID="HiddenField2" />
  <%--  Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>

    <!--Product Modal -->
    <div class="modal fade" id="ProductModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Code or Name" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Product Modal -->

    <!--Product Modal -->
    <div class="modal fade" id="DestProductModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Destprodkeydown(event)" id="txtDestProdSearch" autofocus width="100%" placeholder="Search By Product Code or Name" />

                    <div id="DestProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Product Modal -->

    <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
        Width="900px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeWarehouse(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                        <ul>
                            <li>
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Selected Product</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblProductName" runat="server"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Entered Quantity </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="txt_SalesAmount" runat="server"></asp:Label>
                                                    <asp:Label ID="txt_SalesUOM" runat="server"></asp:Label>

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="lblHolder" id="divpopupAvailableStock" style="display: none;">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Available Stock</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAvailableStock" runat="server"></asp:Label>
                                                    <asp:Label ID="lblAvailableStockUOM" runat="server"></asp:Label>

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li style="display: none;">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Stock Quantity </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="txt_StockAmount" runat="server"></asp:Label>
                                                    <asp:Label ID="txt_StockUOM" runat="server"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>

                        </ul>
                    </div>

                    <div class="clear">
                        <br />
                    </div>
                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                        <div>
                            <div class="col-md-3" id="div_Warehouse">
                                <div style="margin-bottom: 5px;">
                                    Warehouse
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                        TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                        <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange()}" EndCallback="CmbWarehouseEndCallback"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                    <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_Batch">
                                <div style="margin-bottom: 5px;">
                                    Batch
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxComboBox ID="CmbBatch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBatch"
                                        TextField="BatchName" ValueField="BatchID" runat="server" Width="100%" OnCallback="CmbBatch_Callback">
                                        <ClientSideEvents ValueChanged="function(s,e){CmbBatch_ValueChange()}" EndCallback="CmbBatchEndCall"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                    <%--<dxe:ASPxTextBox ID="CmbBatch" runat="server" ClientInstanceName="cCmbBatch" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                       <%-- <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />
                                    </dxe:ASPxTextBox>--%>
                                    <span id="spnCmbBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-4" id="div_Serial">
                                <div style="margin-bottom: 5px;">
                                    Serial No &nbsp;&nbsp; (
                                                <input type="checkbox" id="myCheck" name="BarCode" onchange="AutoCalculateMandateOnChange(this)">Barcode )
                                </div>
                                <div class="" id="divMultipleCombo">

                                    <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="85%" CssClass="pull-left" runat="server" AnimationType="None">
                                        <DropDownWindowStyle BackColor="#EDEDED" />
                                        <DropDownWindowTemplate>
                                            <dxe:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" OnCallback="CmbSerial_Callback"
                                                runat="server">
                                                <Border BorderStyle="None" />
                                                <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" EndCallback="listBoxEndCall" />
                                            </dxe:ASPxListBox>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="padding: 4px">
                                                        <dxe:ASPxButton ID="ASPxButton4" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                            <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                        </dxe:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </DropDownWindowTemplate>
                                        <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" GotFocus="function(s, e){ s.ShowDropDown(); }" />
                                    </dxe:ASPxDropDownEdit>
                                    <span id="spncheckComboBox" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    <div class="pull-left">
                                        <i class="fa fa-commenting" id="abpl" aria-hidden="true" style="font-size: 16px; cursor: pointer; margin: 3px 0 0 5px;" title="Serial No " data-container="body" data-toggle="popover" data-placement="right" data-content=""></i>
                                    </div>
                                </div>
                                <div class="" id="divSingleCombo" style="display: none;">
                                    <dxe:ASPxTextBox ID="txtserial" runat="server" Width="85%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                        <ClientSideEvents TextChanged="txtserialTextChanged" />
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_Quantity">
                                <div style="margin-bottom: 2px;">
                                    Quantity
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                        <%--<ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />--%>
                                    </dxe:ASPxTextBox>
                                    <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div>
                                </div>
                                <div class="Left_Content" style="padding-top: 14px">
                                    <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" UseSubmitBehavior="True" Text="Add" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {if(!document.getElementById('myCheck').checked) SaveWarehouse();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cGrdWarehouse" OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                    VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Available Quantity" FieldName="AvailableQty" Visible="false"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                    VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Conversion Foctor" FieldName="ConversionMultiplier" Visible="false"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Stock Quantity" FieldName="StkQuantity" Visible="false"
                                    VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Balance Stock" FieldName="BalancrStk" Visible="false"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Batch Number" FieldName="BatchNo"
                                    VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="MfgDate"
                                    VisibleIndex="7">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ExpiryDate"
                                    VisibleIndex="8">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                    VisibleIndex="9">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete">
                                            <img src="../../../assests/images/Edit.png" /></a>
                                        &nbsp;
                                                        <a href="javascript:void(0);" id="ADelete" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete">
                                                            <img src="/assests/images/crs.png" /></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnWarehouseEndCallback" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="Popup_WarehouseDestination" runat="server" ClientInstanceName="cPopup_WarehouseDestination"
        Width="900px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	    closeWarehouseDest(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                        <ul>
                            <li>
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Selected Product</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblProductNameDest" runat="server"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Entered Quantity </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="txt_SalesAmountDest" runat="server"></asp:Label>
                                                    <asp:Label ID="txt_SalesUOMdest" runat="server"></asp:Label>

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="lblHolder" id="divpopupAvailableStock" style="display: none;">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Available Stock</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server"></asp:Label>
                                                    <asp:Label ID="Label5" runat="server"></asp:Label>

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li style="display: none;">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Stock Quantity </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="txt_StockAmountDest" runat="server"></asp:Label>
                                                    <asp:Label ID="txt_StockUOMDest" runat="server"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li style="display: none;">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Mfg.Date </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="txtStartDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtStartDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                        </ul>
                    </div>

                    <div class="clear">
                        <br />
                    </div>
                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                        <div>
                            <div class="col-md-3" id="div_WarehouseDest">
                                <div style="margin-bottom: 5px;">
                                    Warehouse
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxComboBox ID="CmbWarehouseDest" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouseDest" SelectedIndex="0"
                                        TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouseDest_Callback">
                                        <ClientSideEvents ValueChanged="function(s,e){CmbWarehouseDest_ValueChange()}" EndCallback="CmbWarehouseDestEndCallback"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                    <span id="spnCmbWarehouseDest" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_BatchDist">
                                <div style="margin-bottom: 5px;">
                                    Batch
                                </div>
                                <div class="Left_Content" style="">
                                    <%--  <dxe:ASPxComboBox ID="CmbBatchDest" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBatchDest"
                                        TextField="BatchName" ValueField="BatchID" runat="server" Width="100%" OnCallback="CmbBatchDest_Callback">
                                        <ClientSideEvents ValueChanged="function(s,e){CmbBatchDest_ValueChange()}" EndCallback="CmbBatchDestEndCall"></ClientSideEvents>
                                    </dxe:ASPxComboBox>--%>
                                    <dxe:ASPxTextBox ID="CmbBatchDest" runat="server" ClientInstanceName="cCmbBatchDest" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                        <%-- <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />--%>
                                        <%-- <ClientSideEvents TextChanged="function(s, e) {SaveWarehouseDest();}" />--%>
                                    </dxe:ASPxTextBox>
                                    <span id="spnCmbBatchDest" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_MFGDATE">
                                <div style="margin-bottom: 5px;">
                                    Mfg.Date
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxDateEdit ID="txtmfgdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtStartDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                    <span id="spnMfgDate" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_EXPDATE">
                                <div style="margin-bottom: 5px;">
                                    Exp.Date
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxDateEdit ID="txtexpdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtexpDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                    <span id="spnExpDate" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-4" id="div_SerialDest">
                                <div style="margin-bottom: 5px;">
                                    Serial No &nbsp;&nbsp; (
                                                <input type="checkbox" id="myCheckDest" name="BarCode" onchange="AutoCalculateMandateOnChangeDest(this)" />Barcode )
                                </div>
                                <div class="" id="divMultipleCombodest">

                                    <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBoxDest" ID="ASPxDropDownEdit2" Width="85%" CssClass="pull-left" runat="server" AnimationType="None">
                                        <DropDownWindowStyle BackColor="#EDEDED" />
                                        <DropDownWindowTemplate>
                                            <dxe:ASPxListBox Width="100%" ID="listBoxDest" ClientInstanceName="checkListBoxDest" SelectionMode="CheckColumn" OnCallback="CmbSerialDest_Callback"
                                                runat="server">
                                                <Border BorderStyle="None" />
                                                <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChangedDest" EndCallback="listBoxEndCallDest" />
                                            </dxe:ASPxListBox>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="padding: 4px">
                                                        <dxe:ASPxButton ID="ASPxButton4" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                            <ClientSideEvents Click="function(s, e){ checkComboBoxDest.HideDropDown(); }" />
                                                        </dxe:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </DropDownWindowTemplate>
                                        <ClientSideEvents TextChanged="SynchronizeListBoxValuesDest" DropDown="SynchronizeListBoxValuesDest" GotFocus="function(s, e){ s.ShowDropDown(); }" />
                                    </dxe:ASPxDropDownEdit>
                                    <span id="spncheckComboBoxDest" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    <div class="pull-left">
                                        <i class="fa fa-commenting" id="abplDest" aria-hidden="true" style="font-size: 16px; cursor: pointer; margin: 3px 0 0 5px;" title="Serial No " data-container="body" data-toggle="popover" data-placement="right" data-content=""></i>
                                    </div>
                                </div>
                                <div class="" id="divSingleComboDest" style="display: none;">
                                    <dxe:ASPxTextBox ID="txtserialDest" runat="server" Width="85%" ClientInstanceName="ctxtserialDest" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                        <ClientSideEvents TextChanged="txtserialDestTextChanged" />
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_QuantityDest">
                                <div style="margin-bottom: 2px;">
                                    Quantity
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxTextBox ID="txtQuantityDest" runat="server" ClientInstanceName="ctxtQuantityDest" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                        <ClientSideEvents TextChanged="function(s, e) {SaveWarehouseDest();}" />
                                    </dxe:ASPxTextBox>
                                    <span id="spntxtQuantityDest" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div>
                                </div>
                                <div class="Left_Content" style="padding-top: 14px">
                                    <dxe:ASPxButton ID="btnWarehouseDest" ClientInstanceName="cbtnWarehouseDest" Width="50px" runat="server" AutoPostBack="False" UseSubmitBehavior="True" Text="Add" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {if(!document.getElementById('myCheckDest').checked) SaveWarehouseDest();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="GrdWarehouseDest" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cGrdWarehouseDest" OnCustomCallback="GrdWarehouseDest_CustomCallback" OnDataBinding="GrdWarehouseDest_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                    VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Available Quantity" FieldName="AvailableQty" Visible="false"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                    VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Conversion Foctor" FieldName="ConversionMultiplier" Visible="false"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Stock Quantity" FieldName="StkQuantity" Visible="false"
                                    VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Balance Stock" FieldName="BalancrStk" Visible="false"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Batch Number" FieldName="BatchNo"
                                    VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="MfgDate"
                                    VisibleIndex="7">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ExpiryDate"
                                    VisibleIndex="8">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                    VisibleIndex="9">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="fn_EditDist('<%# Container.KeyValue %>')" title="Delete">
                                            <img src="../../../assests/images/Edit.png" /></a>
                                        &nbsp;
                                                        <a href="javascript:void(0);" id="ADeleteDest" onclick="fn_DeletecityDest('<%# Container.KeyValue %>')" title="Delete">
                                                            <img src="/assests/images/crs.png" /></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnWarehouseEndCallbackDest" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="btnWarehouseSaveDest" ClientInstanceName="cbtnWarehouseSaveDest" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {FinalWarehouseDest();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxCallbackPanel runat="server" ID="NCallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="NCallbackPanelDest" ClientInstanceName="cCallbackPanelDest" OnCallback="CallbackPanelDest_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <%-- Rev Rajdip
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
         End Rev Rajdip--%>
        <ClientSideEvents EndCallback="CallbackPanelEnddestCall" />
    </dxe:ASPxCallbackPanel>

    <!--Source Wahehouse Modal -->
    <div class="modal fade" id="SourceWarehouseModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Source Warehouse Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SourceWarehousekeydown(event)" id="txtSourceWarehouseSearch" autofocus width="100%" placeholder="Search By Warehouse Name" />

                    <div id="SourceWarehouseTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Source Warehouse</th>

                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Product Modal -->

    <!--Destination Wahehouse Modal -->
    <div class="modal fade" id="DestinationWarehouseModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Destination Warehouse Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="DestinationWarehousekeydown(event)" id="txtDestinationWarehouseSearch" autofocus width="100%" placeholder="Search By Warehouse Name" />

                    <div id="DestinationWarehouseTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Destination Warehouse</th>

                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Product Modal -->
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnHierarchySelectInEntryModule" runat="server" />

     <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />

    <%--Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
    <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="1100px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeMultiUOM(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">



                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">

                        <table class="eqTble">
                            <tr>
                                <%--Mantis Issue 24425, 24428--%>
                                <dxe:GridViewDataTextColumn Caption="MultiUOMSR No" 
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" width="0">
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24425, 24428--%>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--Rev Sanchita--%>
                                                <%--<input type="text" style="width:80px" id="UOMQuantity" style="text-align: right;" maxlength="18" readonly="true" class="allownumericwithdecimal" />--%>
                                                <input type="text" style="width:80px" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onchange="CalcBaseRate()" />
                                                <%--End of Rev Sanchita--%>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="Left_Content" style="">
                                        <div>
                                            <label style="text-align: right;">Base UOM</label>
                                        </div>
                                        <div>
                                            <dxe:ASPxComboBox ID="cmbUOM" ClientInstanceName="ccmbUOM" runat="server" SelectedIndex="0" DataSourceID="UomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24397--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Mantis Issue 24397--%>
                                <td>
                                    <span style="font-size: 22px; padding-top: 15px; display: inline-block;">=</span>
                                </td>
                                <td>
                                    <div>
                                        <div>
                                            <label style="text-align: right;">Alt. UOM</label>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="cmbSecondUOM" ClientInstanceName="ccmbSecondUOM" runat="server" SelectedIndex="0" DataSourceID="AltUomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                                <ClientSideEvents TextChanged="function(s,e) { PopulateMultiUomAltQuantity();}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label style="white-space:nowrap">Alt. Quantity </label>
                                        </div>
                                        <div>
                                            <%--<input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/> --%>
                                            <dxe:ASPxTextBox Width="80px" ID="AltUOMQuantity" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <%--Mantis Issue 24397--%>
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Mantis Issue 24397--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24397--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;padding-top: 20px;">
                                        <div>
                                            
                                        </div>
                                        <div>
                                            <%--<label class="checkbox-inline mlableWh">
                                                <asp:CheckBox ID="chkUpdateRow" Checked="false" runat="server" ></asp:CheckBox>
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>--%>

                                            <label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>

                                    
                                </td>
                                <%--End of Mantis Issue 24397--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="function(s, e) { if(!document.getElementById('myCheck').checked)  {SaveMultiUOM();}}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="grid_MultiUOM" runat="server" KeyFieldName="AltUomId;AltQuantity" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgrid_MultiUOM" OnCustomCallback="MultiUOM_CustomCallback" OnDataBinding="MultiUOM_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <%--Mantis Issue 24397--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24397--%>

                                <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="AltUOM"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Alt. Quantity" FieldName="AltQuantity"
                                    VisibleIndex="4" HeaderStyle-HorizontalAlign="Right">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="" FieldName="UomId" Width="0px"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="" FieldName="AltUomId" Width="0px"
                                    VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>

                                <%--Mantis Issue 24397--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24397--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>

                                        <%--Mantis Issue 24425, 24428--%>
                                        <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                        <%--End of Mantis Issue 24425, 24428--%>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnMultiUOMEndCallback" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <asp:SqlDataSource ID="UomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:HiddenField ID="hddnuomFactor" runat="server" />
    <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />
    <asp:HiddenField ID="hdnPageStatus" runat="server" />
    <asp:HiddenField runat="server" ID="hdnUpperApproveReject" />
    <asp:HiddenField runat="server" ID="hddnCustIdFromCRM" />

    <%----Stock out multiuom------------%>
    <dxe:ASPxPopupControl ID="Popup_MultiUOM_Source" runat="server" ClientInstanceName="cPopup_MultiUOM_Source"
        Width="1100px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeMultiUOM_Source(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                        <table class="eqTble">
                            <tr>
                                <%--Mantis Issue 24425, 24428--%>
                                <dxe:GridViewDataTextColumn Caption="MultiUOMSR No" 
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" width="0">
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24425, 24428--%>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--Rev Sanchita--%>
                                                <%--<input type="text" style="width:80px" id="UOMQuantity" style="text-align: right;" maxlength="18" readonly="true" class="allownumericwithdecimal" />--%>
                                                <input type="text" style="width:80px" id="UOMQuantity_Source" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onchange="CalcBaseRate_Source()" />
                                                <%--End of Rev Sanchita--%>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="Left_Content" style="">
                                        <div>
                                            <label style="text-align: right;">Base UOM</label>
                                        </div>
                                        <div>
                                            <dxe:ASPxComboBox ID="cmbUOM_Source" ClientInstanceName="ccmbUOM_Source" runat="server" SelectedIndex="0" DataSourceID="UomSelect_Source"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24397--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbBaseRate_Source" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate_Source" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Mantis Issue 24397--%>
                                <td>
                                    <span style="font-size: 22px; padding-top: 15px; display: inline-block;">=</span>
                                </td>
                                <td>
                                    <div>
                                        <div>
                                            <label style="text-align: right;">Alt. UOM</label>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="cmbSecondUOM_Source" ClientInstanceName="ccmbSecondUOM_Source" runat="server" SelectedIndex="0" DataSourceID="AltUomSelect_Source"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                                <ClientSideEvents TextChanged="function(s,e) { PopulateMultiUomAltQuantity_Source();}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label style="white-space:nowrap">Alt. Quantity </label>
                                        </div>
                                        <div>
                                            <%--<input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/> --%>
                                            <dxe:ASPxTextBox Width="80px" ID="AltUOMQuantity_Source" runat="server" ClientInstanceName="cAltUOMQuantity_Source" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <%--Mantis Issue 24397--%>
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty_Source();}" />
                                                <%--End of Mantis Issue 24397--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24397--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbAltRate_Source" Width="80px" runat="server" ClientInstanceName="ccmbAltRate_Source" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseRate_Source();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;padding-top: 20px;">
                                        <div>
                                            
                                        </div>
                                        <div>
                                            <%--<label class="checkbox-inline mlableWh">
                                                <asp:CheckBox ID="chkUpdateRow" Checked="false" runat="server" ></asp:CheckBox>
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>--%>

                                            <label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow_Source"  />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>

                                    
                                </td>
                                <%--End of Mantis Issue 24397--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM_Source" ClientInstanceName="cbtnMUltiUOM_Source" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="function(s, e) { if(!document.getElementById('myCheck').checked)  {SaveMultiUOM_Source();}}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="grid_MultiUOM_Source" runat="server" KeyFieldName="AltUomId;AltQuantity" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgrid_MultiUOM_Source" OnCustomCallback="MultiUOM_CustomCallback_Source" OnDataBinding="MultiUOM_DataBinding_Source"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <%--Mantis Issue 24397--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24397--%>

                                <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="AltUOM"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Alt. Quantity" FieldName="AltQuantity"
                                    VisibleIndex="4" HeaderStyle-HorizontalAlign="Right">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="" FieldName="UomId" Width="0px"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="" FieldName="AltUomId" Width="0px"
                                    VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>

                                <%--Mantis Issue 24397--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24397--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom_Source('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>

                                        <%--Mantis Issue 24425, 24428--%>
                                        <a href="javascript:void(0);" onclick="Edit_MultiUom_Source('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                        <%--End of Mantis Issue 24425, 24428--%>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnMultiUOMEndCallback_Source" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtnfinalUomSave_Source" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM_Source();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <asp:SqlDataSource ID="UomSelect_Source" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:SqlDataSource ID="AltUomSelect_Source" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:HiddenField ID="hddnuomFactor_Source" runat="server" />
    <asp:HiddenField ID="hddnMultiUOMSelection_Source" runat="server" />
    <asp:HiddenField ID="hdnPageStatus_Source" runat="server" />
    <asp:HiddenField runat="server" ID="hdnUpperApproveReject_Source" />
    <asp:HiddenField runat="server" ID="hddnCustIdFromCRM_Source" />
    <asp:HiddenField runat="server" ID="hdnPageMode" />
    <%--Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal--%>
</asp:Content>
