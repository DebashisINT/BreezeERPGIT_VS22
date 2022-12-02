<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="STBReceivingRegister.aspx.cs" Inherits="Reports.Reports.GridReports.STBReceivingRegister" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        .colDisable {
            cursor: default !important;
        }

        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }

        .btnOkformultiselection {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 13px !important;
            margin-right: 6px;
        }

        #MandatorySupervisorAssign {
            position: absolute;
            right: 1px;
            top: 27px;
        }

        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #ListBoxWarehouse {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
       
        $(function () {
            cEntityCodePanel.PerformCallback('BindEntityCodeGrid' + '~' + "All");
            cLocationPanel.PerformCallback('BindLocationGrid' + '~' + "All");
        });

        function selectAllEntityCode() {
            gridEntityCodeLookup.gridView.SelectRows();
        }

        function unselectAllEntityCode() {
            gridEntityCodeLookup.gridView.UnselectRows();
        }

        function CloseEntityCodeLookup() {
            gridEntityCodeLookup.ConfirmCurrentSelection();
            gridEntityCodeLookup.HideDropDown();
            gridEntityCodeLookup.Focus();
        }

        function selectAllLocation() {
            gridLocationLookup.gridView.SelectRows();
        }

        function unselectAllLocation() {
            gridLocationLookup.gridView.UnselectRows();
        }

        function CloseLoactionLookup() {
            gridLocationLookup.ConfirmCurrentSelection();
            gridLocationLookup.HideDropDown();
            gridLocationLookup.Focus();
        }
    </script>
    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">

        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsSrvMatIORegDetFilter").val("Y");
            cCallbackPanel.PerformCallback(data);
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }

        function CallbackPanelEndCall(s, e) {
            Grid.Refresh();
        }

        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = dd + '-' + mm + '-' + yyyy;
            }

            return today;
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function OpenModuleDetails(Doc_ID, TransType) {
            var url = '';
            if (TransType == 'SAIN' || TransType == 'SAOUT') {
                url = '/OMS/Management/Activities/StockAdjustmentAdd.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'WHSTIN' || TransType == 'WHSTOUT') {
                url = '/OMS/Management/Activities/WarehousewiseStockTransferAdd.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }

            popupdetails.SetContentUrl(url);
            popupdetails.Show();
        }

        function DetailsAfterHide(s, e) {
            popupdetails.Hide();
        }

        $(document).ready(function () {
            $("#ListBoxWarehouse").chosen().change(function () {
                var Ids = $(this).val();

                <%--$('#<%=hdnSelectedWarehouse.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');--%>

            })
        });

        function selectAllWH() {
            gridwarehouseLookup.gridView.SelectRows();
        }
        function unselectAllWH() {
            gridwarehouseLookup.gridView.UnselectRows();
        }

        function CloseLookupwarehouse() {
            gridwarehouseLookup.ConfirmCurrentSelection();
            gridwarehouseLookup.HideDropDown();
            gridwarehouseLookup.Focus();
        }

    </script>
    <script>

        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }

    </script>
    <style>
        
    </style>

    <style>
        .plhead a {
            font-size: 16px;
            padding-left: 10px;
            position: relative;
            width: 100%;
            display: block;
            padding: 9px 10px 5px 10px;
        }

            .plhead a > i {
                position: absolute;
                top: 11px;
                right: 15px;
            }

        #accordion {
            margin-bottom: 10px;
        }

        .companyName {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 15px;
        }

        .plhead a.collapsed .fa-minus-circle {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                Grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>

        <asp:HiddenField ID="hdnexpid" runat="server" />
    </div>
    <div class="panel-heading">
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
            <div class="panel panel-info">
                <div class="panel-heading" role="tab" id="headingOne">
                    <h4 class="panel-title plhead">
                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                            <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" Style="font-weight: bold;"></asp:Label>
                            <i class="fa fa-plus-circle"></i>
                            <i class="fa fa-minus-circle"></i>
                        </a>
                    </h4>
                </div>
                <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                    <div class="panel-body">
                        <div class="companyName">
                            <asp:Label ID="CompName" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompOth" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompPh" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="DateRange" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <label style="color: #b5285f;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <buttonstyle width="13px">
                    </buttonstyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <buttonstyle width="13px">
                    </buttonstyle>

                </dxe:ASPxDateEdit>
            </div>

            <div class="col-md-2">
                <label style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="For Location : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <div>
                    <dxe:ASPxCallbackPanel runat="server" ID="LocationPanel" ClientInstanceName="cLocationPanel" OnCallback="Location_Callback">
                        <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_Location" SelectionMode="Multiple" runat="server" ClientInstanceName="gridLocationLookup"
                                OnDataBinding="lookup_Location_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                          </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <%--<div class="hide">--%>
                                                            <dxe:ASPxButton ID="ASPxButton9" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllLocation" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton10" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllLocation" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton11" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLoactionLookup" UseSubmitBehavior="False" />
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
                    </panelcollection>
                    </dxe:ASPxCallbackPanel>
                </div>
            </div>

            <div class="col-md-2">
                <label style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Entity Code : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <div>
                    <dxe:ASPxCallbackPanel runat="server" ID="EntityCodePanel" ClientInstanceName="cEntityCodePanel" OnCallback="EntityCode_Callback">
                        <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_EntityCode" SelectionMode="Multiple" runat="server" ClientInstanceName="gridEntityCodeLookup"
                                OnDataBinding="lookup_EntityCode_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="40" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="EntityCode" Visible="true" VisibleIndex="1" width="100px" Caption="Entity Code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                 <%--   <dxe:GridViewDataColumn FieldName="EntityCode" Visible="true" VisibleIndex="2" width="200px" Caption="Entity Code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>--%>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <%--<div class="hide">--%>
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllEntityCode" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllEntityCode" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseEntityCodeLookup" UseSubmitBehavior="False" />
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
                    </panelcollection>
                    </dxe:ASPxCallbackPanel>
                </div>
            </div>

            

            <%--<div class="clear"></div>--%>
            <div class="col-md-2" style="padding: 0; padding-top: 17px;">
                <table>
                    <tr>
                        <td style="padding-left: 15px;">
                            <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">XLSX</asp:ListItem>
                                <asp:ListItem Value="2">PDF</asp:ListItem>
                                <asp:ListItem Value="3">CSV</asp:ListItem>
                                <asp:ListItem Value="4">RTF</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="clear"></div>


            <div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                <div class="">
                </div>
            </div>
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div>
                    </div>

                </td>
            </tr>
        </table>
    </div>

    <div>
    </div>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <panelcollection>
            <dxe:PanelContent runat="server">
                
                 <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                    DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                    Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                    <Columns>
                        <dxe:GridViewDataTextColumn FieldName="DOCUMENTNUMBER" Width="250px" Caption="Receipt No" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Receipt Date" Width="100px" FieldName="DOCUMENT_DATE" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn Caption="For Location" Width="200px" FieldName="FOR_LOCATION" VisibleIndex="3" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Challan No" Width="200px" FieldName="CHALLANNUMBER" VisibleIndex="4" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Challan Date" Width="100px" FieldName="CHALLAN_DATE" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="LCO Code" FieldName="LCO_CODE" Width="200px" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="LCO Name" FieldName="LCO_NAME" Width="200px" VisibleIndex="7" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Vendor" FieldName="MSO" Width="100px" VisibleIndex="8" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="STB Model" FieldName="STBMODEL" Width="100px" VisibleIndex="9" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                               
                        <dxe:GridViewDataTextColumn Caption="Type" FieldName="STBTYPE" Width="100px" VisibleIndex="10" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="QUANTITY" Caption="Quantity" Width="100px" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="REMARKS" Width="300px" VisibleIndex="12" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                    </Columns>
                    <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                    <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                    <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                    <SettingsEditing Mode="EditForm" />
                    <SettingsContextMenu Enabled="true" />
                    <SettingsBehavior AutoExpandAllGroups="true" AllowSort="False" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                    <SettingsSearchPanel Visible="false" />
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                    </SettingsPager>
                    <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                    <TotalSummary>
                        <dxe:ASPxSummaryItem FieldName="Quantity" SummaryType="Custom" Tag="Det_BalQty"/>
                    </TotalSummary>
                 </dxe:ASPxGridView>
                 <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                     ContextTypeName="ReportSourceDataContext" TableName="SRVMATERIALIOREGISTER_REPORT"></dx:LinqServerModeDataSource>

            <asp:HiddenField ID="hfIsSrvMatIORegDetFilter" runat="server" />
            </dxe:PanelContent>
        </panelcollection>
        <clientsideevents endcallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>



    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdetails" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </contentcollection>

        <clientsideevents closeup="DetailsAfterHide" />
    </dxe:ASPxPopupControl>
</asp:Content>
