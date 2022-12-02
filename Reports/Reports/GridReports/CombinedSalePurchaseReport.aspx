<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CombinedSalePurchaseReport.aspx.cs" Inherits="Reports.Reports.GridReports.CombinedSalePurchaseReport" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
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

        #ListBoxBranches {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>


    <script type="text/javascript">

        function ClearGridLookup() {
            var grid = gridcustomerLookup.GetGridView();
            grid.UnselectRows();
        }

       
        function BindOtherDetails(e) {
            var Branchids = gridbranchLookup.gridView.GetSelectedKeysOnPage();
            gridfinancerLookup.gridView.SetFocusedRowIndex(-1);
            gridcustomerLookup.gridView.SetFocusedRowIndex(-1);

            cCustomerCallbackPanel.PerformCallback(Branchids.join(','));
            cFinancierCallbackPanel.PerformCallback(Branchids.join(','));
        }


    </script>


    <script type="text/javascript">


        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";

            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // Grid.PerformCallback('');
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsGrpWiseSalesPurchaseFilter").val("Y");

            data += '~' + cxdeToDate.GetDate();

            //Grid.PerformCallback(data);
            //LoadingPanel.Show();
            //cCallbackPanel.PerformCallback(data);
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                LoadingPanel.Show();
                cCallbackPanel.PerformCallback(data);
            }

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
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

        function CallbackPanelEndCall(s, e) {
            <%--Rev Subhra 18-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }


        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function Callback2_EndCallback() {
            // alert('');
            $("#drdExport").val(0);
            LoadingPanel.Hide();
        }


        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
        }
        function CloseGridLookupBranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll_customer() {
            gridcustomerLookup.gridView.SelectRows();
        }
        function unselectAll_customer() {
            gridcustomerLookup.gridView.UnselectRows();
        }
        function CloseGridLookupCustomer() {
            gridcustomerLookup.ConfirmCurrentSelection();
            gridcustomerLookup.HideDropDown();
            gridcustomerLookup.Focus();
        }

        function selectAll_financier() {
            gridfinancerLookup.gridView.SelectRows();
        }
        function unselectAll_financier() {
            gridfinancerLookup.gridView.UnselectRows();
        }
        function CloseGridLookupFinancier() {
            gridfinancerLookup.ConfirmCurrentSelection();
            gridfinancerLookup.HideDropDown();
            gridfinancerLookup.Focus();
        }

    </script>

    <style>
        .plhead a {
            font-size:16px;
            padding-left:10px;
            position:relative;
            width:100%;
            display:block;
            padding:9px 10px 5px 10px;
        }
        .plhead a>i {
            position:absolute;
            top:11px;
            right:15px;
        }
        #accordion {
            margin-bottom:10px;
        }
        .companyName {
            font-size:16px;
            font-weight:bold;
            margin-bottom:15px;
        }
        
        .plhead a.collapsed .fa-minus-circle{
            display:none;
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
    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Groupwise Sales/Purchase</h3>
        </div>--%>
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
          <div class="panel panel-info">
            <div class="panel-heading" role="tab" id="headingOne">
              <h4 class="panel-title plhead">
                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                  <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" style="font-weight:bold;"></asp:Label>
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                    <div class="companyName">
                        <asp:Label ID="CompName" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                  <%--Rev Subhra 18-12-2018   0017670--%>
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                  <%--End of Rev--%>
                    <div>
                        <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompPh" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>       
                    <div>
                        <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="DateRange" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>
    </div>
    <div class="form_main">

        <table class="pull-left">
            <tr>
                   <td style="">
                     <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                         <asp:Label ID="Label8" runat="Server" Text="Criteria: " CssClass="mylabel1" Width="110px"></asp:Label>
                     </div>
                  </td>

                  <td style="width: 180px">
                    <asp:DropDownList ID="ddlCriteria" runat="server" Width="100%" >
                        <asp:ListItem Text="Class-Category Wise" Value="Class-Category Wise"></asp:ListItem>
                        <asp:ListItem Text="Category-Class Wise" Value="Category-Class Wise"></asp:ListItem>
                    </asp:DropDownList>
                      <%--OnSelectedIndexChanged="ddlCriteria_SelectedIndexChanged"--%>
                  </td>

                  <td style="padding-left: 15px">
                      <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                         <asp:Label ID="Label4" runat="Server" Text="Transaction Type: " CssClass="mylabel1" Width="110px"></asp:Label>
                      </div>
                  </td>

                 <td style="width: 180px">
                    <asp:DropDownList ID="ddlisdocument" runat="server" Width="100%">
                        <asp:ListItem Text="Sales" Value="Sales"></asp:ListItem>
                        <asp:ListItem Text="Purchases" Value="Purchases"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
              <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>   
                </td>
                <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>

                    </dxe:ASPxDateEdit>
                </td>

                 <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td style="width: 180px">
                    <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                        OnDataBinding="lookup_branch_DataBinding"
                        KeyFieldName="branch_id" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                        <columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                            <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>

                            <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                        </columns>
                        <gridviewproperties settings-verticalscrollbarmode="Auto" settingspager-mode="ShowAllRecords">
                            <Templates>
                                <StatusBar>
                                    <table class="OptionsTable" style="float: right">
                                        <tr>
                                            <td>
                                                <%--<div class="hide">--%>
                                                    <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" UseSubmitBehavior="False"/>
                                               <%--</div>--%>
                                                <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_branch" UseSubmitBehavior="False"/>                                                
                                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookupBranch" UseSubmitBehavior="False" />
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
                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true"   />
                        </gridviewproperties>
                        <clientsideevents textchanged="function(s, e) { BindOtherDetails(e)}" />
                    </dxe:ASPxGridLookup>
                </td>



                <td style="padding-left: 15px">
                    <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div class="pull-right">
        </div>
        <table class="TableMain100">


            <tr>

                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="true"
                            OnDataBound="Showgrid_DataBound" DataSourceID="GenerateEntityServerModeDataSource" OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared"
                            ClientSideEvents-BeginCallback="Callback2_EndCallback" OnSummaryDisplayText="ShowGrid_SummaryDisplayText"
                            Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto">
                            <%--OnCustomCallback="Grid_CustomCallback" OnDataBinding="Showgrid_DataBinding" --%>
                            <columns>
                              <%-- <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Branch and Warehouse Details" Width="50%" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="Over 120 Days" Caption="Over 120 days" Width="25%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Within 120 Days" Caption="Within 120 days" Width="20%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Total" Caption="Total" Width="25%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" />--%>

                             <%--   <dxe:GridViewDataTextColumn FieldName="Sl_No" Caption="Sl." Width="50%" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn FieldName="Class Name" Caption="Class Name" Width="50%" VisibleIndex="2" />
                                <dxe:GridViewDataTextColumn FieldName="Category Name" Caption="Category Name" Width="50%" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" Width="50%" VisibleIndex="4" />
                                <dxe:GridViewDataTextColumn FieldName="Sale Value" Caption="Sale Value" Width="50%" VisibleIndex="5" />
                                <dxe:GridViewDataTextColumn FieldName="Purchase Value" Caption="Purchase Value" Width="50%" VisibleIndex="6" />--%>

                            </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" />
                            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <settingspager pagesize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />

                            </settingspager>
                            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />
                           <%-- <totalsummary>
                                <dxe:ASPxSummaryItem FieldName="Over 120 Days" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Within 120 Days" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total" SummaryType="Sum" />
                            </totalsummary>--%>
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="GROUPWISESALESPURCHASE_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsGrpWiseSalesPurchaseFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </contentcollection>

        <clientsideevents closeup="BudgetAfterHide" />
    </dxe:ASPxPopupControl>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton" Modal="True" >     
    </dxe:ASPxLoadingPanel>
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>
