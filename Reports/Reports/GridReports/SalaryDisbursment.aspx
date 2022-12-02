<%@ Page Title="Salary Disbursment" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true"
    CodeBehind="SalaryDisbursment.aspx.cs" Inherits="Reports.Reports.GridReports.SalaryDisbursment" %>



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


        $(function () {

            function OnWaitingGridKeyPress(e) {
                if (e.code == "Enter") {
                    
                }
            }
        });

        function ListActivityType() {

            $('#ListBoxBranches').chosen();
            $('#ListBoxBranches').fadeIn();

        }

        function Callback_EndCallback() {

            $("#drdExport").val(0);
        }

    </script>


    <script type="text/javascript">


        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // Grid.PerformCallback('');
        }

        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";

            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // alert( data);
            Grid.PerformCallback(data);
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

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function abc() {
            // alert();
            $("#drdExport").val(0);

        }

        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }

        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function selectAll_product() {
            gridquotationLookup.gridView.SelectRows();
        }

        function unselectAll_product() {
            gridquotationLookup.gridView.UnselectRows();
        }

        function CloseGridQuotationLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
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
        //$(document).ready(function () {
        //    gridScreenLoad(Grid);
        //    $('.navbar-minimalize').click(function (e) {
        //        gridScreen(Grid);
        //    });
        //});

        //var wndowWidth = $(window).width();
        //var gridClient;
        
        
        
        //$(function () {
        //    gridClient = Grid;
        //})

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
            <h3>Salary Disbursement</h3>
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
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <table class="pull-left">
            <%--<tr>


                <td style="">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td style="width: 254px">
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
            </tr>--%>
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
                        <buttonstyle width="13px">
                        </buttonstyle>
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
                        <buttonstyle width="13px">
                        </buttonstyle>

                    </dxe:ASPxDateEdit>
                </td>
                <td></td>
                <td style="padding-left: 10px; padding-top: 3px">
                    <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <%-- <% if (rights.CanExport)
                           { %>--%>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <%-- <% } %>--%>
                </td>
            </tr>
            <tr>
            </tr>
        </table>
        <div class="pull-right">
        </div>
        <table class="TableMain100">


            <tr>

                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnDataBinding="grid2_DataBinding"
                            ClientSideEvents-BeginCallback="Callback_EndCallback" OnSummaryDisplayText="ShowGrid_SummaryDisplayText"
                            OnCustomCallback="Grid_CustomCallback" Settings-HorizontalScrollBarMode="Visible"
                            Settings-VerticalScrollableHeight="180" Settings-VerticalScrollBarMode="Auto">
                            <columns>
                                <dxe:GridViewDataTextColumn FieldName="SL" Caption="Sl." Width="5%" VisibleIndex="2" />
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_NAME" Caption="Branch" Width="14%" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="EMP_CODE" Caption="Emp Id" Width="9%" VisibleIndex="4" />
                                <dxe:GridViewDataTextColumn FieldName="EMP_NAME" Caption="Employee's Name" Width="23%" VisibleIndex="5"  />
                                <dxe:GridViewDataTextColumn FieldName="GROSS_AMOUNT" Caption="Gross Salary" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="PF_AMOUNT" Caption="PF Ded." Width="12%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="ESIC_AMOUNT" Caption="ESI Ded." Width="12%" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="PTAX_AMOUNT" Caption="Prof. Tax Ded." Width="12%" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="TDS_AMOUNT" Caption="TDS" Width="12%" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="ADVREC_AMOUNT" Caption="Adv Recovery" Width="12%" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="TOT_DED_AMOUNT" Caption="Net Ded." Width="12%" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="NET_AMOUNT" Caption="Net Salary Paid" Width="15%" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="PAY_TYPE" Caption="Paid By" Width="10%" VisibleIndex="14"/>
                                <dxe:GridViewDataTextColumn FieldName="NET_AMT" Caption="CB Amount" Width="10%" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00"/>
                                <dxe:GridViewDataTextColumn FieldName="BANK_NAME" Caption="Bank Name" Width="20%" VisibleIndex="16"/>
                            </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" />
                            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <settingspager pagesize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />

                            </settingspager>
                            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />
                            <totalsummary>
                                <dxe:ASPxSummaryItem FieldName="GROSS_AMOUNT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="PF_AMOUNT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="ESIC_AMOUNT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="PTAX_AMOUNT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="ADVREC_AMOUNT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="TDS_AMOUNT" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="NET_AMOUNT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="TOT_DED_AMOUNT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="NET_AMT" SummaryType="Sum" />
                            </totalsummary>
                        </dxe:ASPxGridView>

                    </div>
                </td>
            </tr>
        </table>
    </div>



    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>



</asp:Content>
