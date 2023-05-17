<%--=======================================================Revision History=====================================================    
    1.0   Pallab    V2.0.38   15-05-2023      26122: Stock Reconcile module design modification & check in small device
=========================================================End Revision History===================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="stockReconcileList.aspx.cs" Inherits="ERP.OMS.Management.Activities.stockReconcileList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script>
        //function OnAddButtonClick() {
        //    var url = 'stockReconcileAdd.aspx';
        //    window.location.href = url;
        //}
    </script>

    <style>
        .padTab > tbody > tr > td {
            padding-right: 15px;
            vertical-align: middle;
        }

            .padTab > tbody > tr > td > label {
                margin-bottom: 0 !important;
            }

            .padTab > tbody > tr > td > .btn {
                margin-top: 0 !important;
            }

            .padTab > tbody > tr > td > input {
                margin-bottom: 0 !important;
            }

        .table-primary {
            border: 1px solid #ccc;
            border-top: none;
        }

            .table-primary > thead > tr > th {
                background: #214ca2;
                color: #fff;
                border-top: 1px solid #214ca2;
            }

                .table-primary > thead > tr > th:not(:last-child) {
                    border-right: 1px solid #1a3d84;
                }

            .table-primary > tbody > tr > td:not(:last-child) {
                border-right: 1px solid #ccc;
            }
    </style>

    <script>

        var isFirstTime = true;

        function StockImportComplete() {
            var Quote_Msg = '<%= Session["MSG"] %>';
            if (Quote_Msg != "" || Quote_Msg != null) {
                jAlert(Quote_Msg);
            }

            //jAlert(Quote_Msg, 'Alert Dialog: [Import]', function (r) {
            //    if (r == true) {




            //        window.location.assign("stockReconcileList.aspx");

            //    }
            //});

            //jAlert("Stock adjustment successfully done.");
            //var URL = '/OMS/Management/Activities/stockReconcileList.aspx';
            //window.location.href = URL;
        }



        function AllControlInitilize() {
            if (isFirstTime) {
                if (localStorage.getItem('InvoiceList_FromDate')) {
                    var fromdatearray = localStorage.getItem('InvoiceList_FromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('InvoiceList_ToDate')) {
                    var todatearray = localStorage.getItem('InvoiceList_ToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }

                //if (localStorage.getItem('InvoiceList_Branch')) {
                //    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('InvoiceList_Branch'))) {
                //        ccmbBranchfilter.SetValue(localStorage.getItem('InvoiceList_Branch'));
                //    }

                //}

                //updateGridByDate();
                isFirstTime = false;
            }
        }


        $(document).ready(function () {
            // cCmbWarehouse.SetSelectedIndex(0);

            //$("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
            //$("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
            //$("#hfwareHouseID").val(cCmbWarehouse.GetValue());

            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdQuotation.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdQuotation.SetWidth(cntWidth);
                }
            }, 1000);

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdQuotation.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdQuotation.SetWidth(cntWidth);
                }

            });

        });

        function updateGridByDate() {
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (cCmbWarehouse.GetValue() == null) {
                jAlert('Please select warehouse.', 'Alert', function () { cCmbWarehouse.Focus(); });
            }
            else {
                localStorage.setItem("InvoiceList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("InvoiceList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                //localStorage.setItem("InvoiceList_Branch", cCmbWarehouse.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfwareHouseID").val(cCmbWarehouse.GetValue());


                cGrdQuotation.Refresh();

            }
        }


        function StkAdj_CloseClick() {
            cPopup_PostStlAdj.Hide();
        }
        function Adj_CloseClick() {
            cpopupAjustmentList.Hide();
        }

        function grid_EndCallBack(s, e) {
            if (cGrdQuotation.cpDelete) {
                var message = cGrdQuotation.cpDelete;
                if (cGrdQuotation.cpDelete == "Deleted") {
                    jAlert("Deleted successfully.");
                }
                else if (cGrdQuotation.cpDelete == "CannotDelete") {
                    jConfirm("Cannot delete. Stock adjustment entries generated. Please delete those Stock Adjustment and try again.", 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            //cgrdAdjustmnentList.PerformCallback('Delete~' + keyValue);
                            cgrdAdjustmnentList.Refresh();
                            cpopupAjustmentList.Show();
                        }
                    });
                }
                else if (cGrdQuotation.cpDelete == "Inconvenience") {
                    jAlert("Problem in Deleting. Sorry for Inconvenience.");
                }
                cGrdQuotation.cpDelete = null;
                cGrdQuotation.Refresh();
            }
        }


        function gridAdjustment_EndCallBack(s, e) {
            if (cgrdAdjustmnentList.cpgridAdjustmentDelete) {

                jAlert(cgrdAdjustmnentList.cpgridAdjustmentDelete);
                cgrdAdjustmnentList.cpgridAdjustmentDelete = null;
                cgrdAdjustmnentList.Refresh();
                cpopupAjustmentList.Hide();
                cGrdQuotation.Refresh();
            }
        }

        function gridRowclick(s, e) {
            $('#GrdQuotation').find('tr').removeClass('rowActive');
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


        function grdAdjustmnentList(s, e) {
            $('#GrdQuotation').find('tr').removeClass('rowActive');
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
        function OnMoreInfoClick(keyValue) {

            $.ajax({
                type: "POST",
                url: "stockReconcileList.aspx/NonZerodatacheck",
                data: "{'id':'" + keyValue + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status != "2") {
                        var url = 'stockReconcileAdd.aspx?key=' + keyValue;
                        window.location.href = url;

                    }
                    else if (status == "2") {
                        jAlert("The Product/s in this Document is/are already Reconciled. Click on View Reconciled to view the Reconciled Products.");
                    }
                }
            });

            
        }


        function OnViewClick(keyValue) {


            $.ajax({
                type: "POST",
                url: "stockReconcileList.aspx/NonZerodatacheck",
                data: "{'id':'" + keyValue + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status != "2") {
                        var url = 'stockReconcileAdd.aspx?key=' + keyValue + '&mode=V';
                        window.location.href = url;
                    }
                    else if (status == "2") {
                        jAlert("The Product/s in this Document is/are already Reconciled. Click on View Reconciled to view the Reconciled Products.");
                    }
                }
            });



           
        }
        function StockAdjustmentListClick(keyValue) {
            $("#hdnReconcilestkadjId").val(keyValue);
            cStockgrid.Refresh();
            cPopup_PostStlAdj.Show();
        }

        function StockZeroListClick(keyValue) {
            $("#ReconIdForNonStock").val(keyValue);
            cZeroStocckGrid.Refresh();
            cASPxPopupControl1_ZeroStock.Show();

        }

        function OnAddButtonClick() {
            var url = 'stockReconcileAdd.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdQuotation.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        function OnStockClickDelete(keyValue) {
            $("#hdnStockId").val(keyValue);
            cgrdAdjustmnentList.PerformCallback('Delete~' + keyValue);
        }
        function ImportUpdatePopOpenEmployeesTarget(e) {

            $("#modalimport").modal('show');
        }
        function ShowCommitList(e) {
            cGrdComitList.Refresh();
            cPopupComitList.Show();
        }

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
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3 class="pull-left">Stock Reconcile</h3>

        </div>
        <table class="padTab pull-right" style="margin-top: 7px;">
            <tbody>
                <tr>
                    <td>
                        <label>From </label>
                    </td>
                    <%--Rev 1.0: "for-cust-icon" class add --%>
                    <td class="for-cust-icon">
                        <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxDateEdit>
                        <%--Rev 1.0--%>
                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                        <%--Rev end 1.0--%>
                    </td>
                    <td>
                        <label>To </label>
                    </td>
                    <%--Rev 1.0: "for-cust-icon" class add --%>
                    <td class="for-cust-icon">
                        <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxDateEdit>
                        <%--Rev 1.0--%>
                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                        <%--Rev end 1.0--%>
                    </td>
                    <td>Select Warehouse</td>
                    <td style="width: 150px">
                        <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                            TextField="bui_WarehouseName" ValueField="bui_WarehouseID" runat="server" Width="100%">
                        </dxe:ASPxComboBox>
                    </td>
                    <td>
                        <input type="button" value="Show" class="btn btn-primary " onclick="updateGridByDate()" />
                    </td>
                </tr>

            </tbody>
        </table>
    </div>
        <div class="form_main">
        <div>
            <% if (rights.CanAdd)
               { %>

            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus"></i></span><span>New</span> </a>
            <%} %>
            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdStkAdj" runat="server" CssClass="btn btn-primary " OnSelectedIndexChanged="ReconcileStkAdjustmentListExport_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
            <asp:LinkButton ID="lnlDownloaderexcel" runat="server" OnClick="lnlDownloaderexcel_Click" CssClass="btn btn-info mBot0">Download Format</asp:LinkButton>
            <button type="button" onclick="ImportUpdatePopOpenEmployeesTarget();" class="btn btn-warning ">Import(Add)</button>
            <button type="button" onclick="ShowCommitList();" class="btn btn-primary ">Committed Stock List</button>

        </div>
        <div>
            <div class="GridViewArea relative">
                <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Reconcile_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                    Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback"
                    Settings-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false"
                    SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <Columns>
                        <dxe:GridViewDataTextColumn FieldName="Reconcile_Id" SortOrder="Descending" Visible="false" Width="0">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Document No." FieldName="DocNumber"
                            VisibleIndex="1" FixedStyle="Left" Width="200px">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Date" FieldName="ReconcileDate"
                            VisibleIndex="2" Width="120px">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Unit"
                            VisibleIndex="3" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Warehouse" FieldName="WareHouseName"
                            VisibleIndex="4" Width="350px">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Adj. Posted" FieldName="AdjPosted" Width="100px"
                            VisibleIndex="5">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Created By" FieldName="CreatedBy" Width="150px"
                            VisibleIndex="6">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Created On" FieldName="CreatedOn"
                            VisibleIndex="7" Width="160px">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn Caption="Last Updated By" FieldName="LastUpdatedBy" Width="150px"
                            VisibleIndex="8">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Last Updated On" FieldName="LastUpdatedOn"
                            VisibleIndex="9" Width="160px">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="20" Width="0">
                            <DataItemTemplate>
                                <div class='floatedBtnArea'>


                                    <a href="javascript:void(0);" onclick="StockZeroListClick('<%# Container.KeyValue %>')" class="" title="">
                                        <span class='ico ColorFive'><i class='fa fa-file-o'></i></span><span class='hidden-xs'>View Reconciled</span></a>

                                    <a href="javascript:void(0);" onclick="StockAdjustmentListClick('<%# Container.KeyValue %>')" class="" title="">
                                        <span class='ico ColorFive'><i class='fa fa-file-o'></i></span><span class='hidden-xs'>View Stock Adjustment</span></a>

                                    <% if (rights.CanView)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                                        <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                    <% } %>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="" title="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                    <%} %>
                                </div>
                            </DataItemTemplate>
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                            <HeaderTemplate><span></span></HeaderTemplate>
                            <EditFormSettings Visible="False"></EditFormSettings>
                            <Settings AllowAutoFilterTextInputTimer="False" />

                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <ClientSideEvents EndCallback="grid_EndCallBack" RowClick="gridRowclick" />
                    <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <GroupSummary>
                    </GroupSummary>

                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                </dxe:ASPxGridView>

                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="V_ReconCileList" />
                <asp:SqlDataSource runat="server" ID="dsPortCode" SelectCommand="SELECT [Port_id] Port_Id, [Port_Code] Port_Code FROM [tbl_master_PortCode]"></asp:SqlDataSource>
                <asp:HiddenField ID="hiddenedit" runat="server" />
            </div>
            <div style="display: none">
                <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
            </div>
        </div>
    </div>
    </div>
    <dxe:ASPxPopupControl ID="Popup_PostStlAdj" runat="server" ClientInstanceName="cPopup_PostStlAdj"
        Width="150px" Height="50px" HeaderText="Stock Adjustment List" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">



                    <div class="GridViewArea relative">
                        <dxe:ASPxGridView ID="Stockgrid" runat="server" KeyFieldName="Reconcile_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                            Width="100%" ClientInstanceName="cStockgrid"
                            DataSourceID="EntityServerModeDataSourceForStockAdjusted" SettingsDataSecurity-AllowEdit="false"
                            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="200" Settings-VerticalScrollBarMode="Auto">
                            <SettingsSearchPanel Visible="True" Delay="5000" />
                            <Columns>

                                <dxe:GridViewDataTextColumn FieldName="Reconcile_Id" Visible="false" Width="0" VisibleIndex="0">
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>



                                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="DocNumber" SortOrder="Descending" VisibleIndex="1" Width="50%">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>



                                <dxe:GridViewDataTextColumn Caption="Date" FieldName="Stock_Date"
                                    VisibleIndex="2" Width="50%">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>


                            </Columns>
                            <SettingsContextMenu Enabled="true"></SettingsContextMenu>

                            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <GroupSummary>
                            </GroupSummary>

                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>

                        <dx:LinqServerModeDataSource ID="EntityServerModeDataSourceForStockAdjusted" runat="server" OnSelecting="EntityServerModeDataSource_StockAdjustedSelecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="V_ReconCileListDocumentWise" />

                    </div>
                    <div class="clear">
                        <br />
                    </div>

                    <div class="col-sm-12 text-right">




                        <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_close" runat="server" AutoPostBack="False" Text="Close" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {StkAdj_CloseClick();}" />
                        </dxe:ASPxButton>

                    </div>


                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <dxe:ASPxPopupControl ID="popupAjustmentList" runat="server" ClientInstanceName="cpopupAjustmentList"
        Width="150px" Height="50px" HeaderText="Stock Adjustment List" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">



                    <div class="GridViewArea relative">

                        <dxe:ASPxGridView ID="grdAdjustmnentList" runat="server" KeyFieldName="Stock_ID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                            Width="100%" ClientInstanceName="cgrdAdjustmnentList" OnCustomCallback="gridAdjustment_CustomCallback"
                            Settings-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSourcegridAdjustment" SettingsDataSecurity-AllowEdit="false"
                            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="200" Settings-VerticalScrollBarMode="Auto">
                            <SettingsSearchPanel Visible="True" Delay="5000" />
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="Stock_ID" Visible="false" Width="0">
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="DocNumber"
                                    VisibleIndex="1" FixedStyle="Left" Width="240px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Date" FieldName="Stock_Date"
                                    VisibleIndex="2" Width="230px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="3" Width="0">
                                    <DataItemTemplate>
                                        <div class='floatedBtnArea'>



                                            <% if (rights.CanDelete)
                                               { %>
                                            <a href="javascript:void(0);" onclick="OnStockClickDelete('<%# Container.KeyValue %>')" class="" title="">
                                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                            <%} %>
                                        </div>
                                    </DataItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderTemplate><span></span></HeaderTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                    <Settings AllowAutoFilterTextInputTimer="False" />

                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                            <ClientSideEvents EndCallback="gridAdjustment_EndCallBack" RowClick="grdAdjustmnentList" />
                            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <GroupSummary>
                            </GroupSummary>

                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>


                        <dx:LinqServerModeDataSource ID="EntityServerModeDataSourcegridAdjustment" runat="server" OnSelecting="EntityServerModeDataSourcegridAdjustment_StockAdjustedSelecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="V_StockAdjReconcileDocumentWise" />

                    </div>
                    <div class="clear">
                        <br />
                    </div>

                    <div class="col-sm-12 text-right">




                        <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_close" runat="server" AutoPostBack="False" Text="Close" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {Adj_CloseClick();}" />
                        </dxe:ASPxButton>

                    </div>


                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <div class="modal fade" id="modalimport" role="dialog">
        <div class="modal-dialog VerySmall">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Select File to Import (Add/Update)</h4>
                </div>
                <div class="modal-body">

                    <div class="col-md-12">
                        <div id="divproduct">

                            <div>
                                <asp:FileUpload ID="OFDBankSelect" accept=".xls,.xlsx" runat="server" Width="100%" />
                                <div class="pTop10  mTop5">
                                    <asp:Button ID="BtnSaveexcel" runat="server" Text="Import(Add)" OnClick="BtnSaveexcel_Click1" CssClass="btn btn-primary" />

                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <dxe:ASPxPopupControl ID="PopupComitList" runat="server" ClientInstanceName="cPopupComitList"
        Width="1200px" Height="300px" HeaderText="Committed Stock List" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="GridViewArea relative">
                    <dxe:ASPxGridView ID="GrdComitList" runat="server" KeyFieldName="sProducts_ID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                        Width="100%" ClientInstanceName="cGrdComitList"
                        Settings-HorizontalScrollBarMode="Auto" DataSourceID="GrdComitListDataSource" SettingsDataSecurity-AllowEdit="false"
                        SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="350" Settings-VerticalScrollBarMode="Auto">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>

                            <dxe:GridViewDataTextColumn FieldName="sProducts_ID" Visible="false" Width="0" VisibleIndex="0">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="StockSheet_id" FieldName="StockSheet_id" SortOrder="Descending" VisibleIndex="1" Width="0">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Product" FieldName="Product" VisibleIndex="1" Width="250px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description" VisibleIndex="2" Width="250px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="sProduct_Status" Caption="Status" VisibleIndex="3" ReadOnly="True" Width="250px">
                                <CellStyle Wrap="True"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Class" FieldName="Class" VisibleIndex="4" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName" VisibleIndex="5" Width="180px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="StockUnit" Caption="Stock Unit" VisibleIndex="6" Width="100px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="AltUnit" Caption="Alt. Unit" VisibleIndex="7" Width="100px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="StockUnitQnty" Caption="Stock Unit Qty" VisibleIndex="8" Width="100px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="AltUnitQnty" Caption="Alt. Unit Qty" VisibleIndex="9" Width="100px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="As On Date" FieldName="StockSheet_AsOnDate"
                                VisibleIndex="10" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxe:ASPxGridView>

                    <dx:LinqServerModeDataSource ID="GrdComitListDataSource" runat="server" OnSelecting="GrdComitListDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="V_ReconcileComitList" />

                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="ASPxPopupControl1_ZeroStock" runat="server" ClientInstanceName="cASPxPopupControl1_ZeroStock"
        Width="1200px" Height="250px" HeaderText="Zero Stock List" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div class="GridViewArea relative">
                        <dxe:ASPxGridView ID="ZeroStocckGrid" runat="server" KeyFieldName="Reconcile_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                            Width="100%" ClientInstanceName="cZeroStocckGrid"
                            Settings-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSourceZeroStock" SettingsDataSecurity-AllowEdit="false"
                            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="200" Settings-VerticalScrollBarMode="Auto">
                            <SettingsSearchPanel Visible="True" Delay="5000" />
                            <Columns>                           
                                <dxe:GridViewDataTextColumn Caption="Product" FieldName="Product" VisibleIndex="1" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description" VisibleIndex="2" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="sProduct_Status" Caption="Status" VisibleIndex="3" ReadOnly="True" Width="100px">
                                    <CellStyle Wrap="True"></CellStyle>
                                </dxe:GridViewDataTextColumn>                 

                                <dxe:GridViewDataTextColumn FieldName="StockUnit" Caption="Stock Unit" VisibleIndex="4" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="AltUnit" Caption="Alt. Unit" VisibleIndex="5" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Stock Unit Qty" VisibleIndex="6" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ALTCLOSE_QTY" Caption="Alt. Unit Qty" VisibleIndex="7" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="StockUnitQnty" Caption="Physical(Stk Unit)" VisibleIndex="8" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="AltUnitQnty" Caption="Physical(Alt. Unit)" VisibleIndex="9" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="DiffStockUnitQnty" Caption="Diff(Main Unit)" VisibleIndex="10" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="DiffAltUnitQnty" Caption="Diff(Alt. Unit)" VisibleIndex="11" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="WareHouse" Caption="WareHouse" VisibleIndex="12" Width="150px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <%--   <ClientSideEvents EndCallback="OnCommitEndCallback"/>--%>
                            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                            <SettingsPager NumericButtonCount="5" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <GroupSummary>
                                <dxe:ASPxSummaryItem FieldName="GrossAmount" SummaryType="Sum" DisplayFormat="Total Gross Amount : {0}" />
                                <dxe:ASPxSummaryItem FieldName="ChargesAmount" SummaryType="Sum" DisplayFormat="Total Tax & Charges : {0}" />
                                <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" DisplayFormat="Total Net Amount : {0}" />
                                <dxe:ASPxSummaryItem FieldName="AmountReceived" SummaryType="Sum" DisplayFormat="Total Amount Received : {0}" />
                                <dxe:ASPxSummaryItem FieldName="BalanceAmount" SummaryType="Sum" DisplayFormat="Total Balance Amount : {0}" />
                            </GroupSummary>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="EntityServerModeDataSourceZeroStock" runat="server" OnSelecting="EntityServerModeDataSourceZeroStock_StockAdjustedSelecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="V_ZeroStockDEtails" />
                    </div>
                    <div class="clear">
                        <br />
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hfFromDate" runat="server" />
    <asp:HiddenField ID="hfToDate" runat="server" />
    <asp:HiddenField ID="hdnReconcilestkadjId" runat="server" />
    <asp:HiddenField ID="hdnStockAdjustedLIst" runat="server" />
    <asp:HiddenField ID="hfwareHouseID" runat="server" />
    <asp:HiddenField ID="hdnStockId" runat="server" />
    <asp:HiddenField ID="ReconIdForNonStock" runat="server" />
</asp:Content>
