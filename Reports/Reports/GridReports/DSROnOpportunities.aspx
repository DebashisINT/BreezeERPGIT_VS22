<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DSROnOpportunities.aspx.cs" Inherits="Reports.Reports.GridReports.DSROnOpportunities" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
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

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>

    <script type="text/javascript">
        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsDSRONOPPORTUNITYDetFilter").val("Y");
            $("#drdExport").val(0);
            cCallbackPanel.PerformCallback(data);
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
            Grid.Refresh();
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
    <div>
        <asp:HiddenField ID="hdnexpid" runat="server" />
    </div>
    <div class="panel-heading">
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
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
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
        <div class="row">          
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;margin-bottom: 4px;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>

                </dxe:ASPxDateEdit>
            </div>

            <div class="col-md-2" style="padding:0;padding-top: 17px;padding-bottom: 5px;">
            <table>
                <tr>
                    <td  style="padding-left:15px;">
                            <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <% if (rights.CanExport)
                    { %> 
                         <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">EXCEL</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                        </asp:DropDownList>
                    <% } %>
                    </td>
                </tr>
            </table>
                
            </div>
            <div class="clear"></div>
        </div>
    </div>

    <div>
    </div>
      <div class="clearfix">
            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                    DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                    OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback"
                    Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Customer/Lead" FieldName="CUSTNAME" Width="220px" VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Product Class" Width="200px" FieldName="MULTIPLEPRODUCTCLASSNAME" VisibleIndex="2">
                            <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ASSIGNED_TO" Width="130px" Caption="Salesman" >
                            <CellStyle HorizontalAlign="Left"></CellStyle>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <EditFormSettings Visible="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="INDUSTRY" Width="130px" Caption="Industry" >
                            <CellStyle HorizontalAlign="Left"></CellStyle>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <EditFormSettings Visible="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="BUDGET" Width="130px" Caption="Product:Budget" >
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right"/>
                            <EditFormSettings Visible="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="BUDGET_MONTHWISE" Width="130px" Caption="Monthly Budget" >
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                            <HeaderStyle HorizontalAlign="Right"/>
                            <EditFormSettings Visible="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Reason for Close" FieldName="CLOSE_REASON" Width="200px" VisibleIndex="7">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataDateColumn Caption="Close Date" Width="100px" FieldName="CLOSED_DATE" VisibleIndex="8" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataDateColumn>

                        <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Quantity" Width="100px" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                                
                        <dxe:GridViewDataTextColumn Caption="Remarks of Close" FieldName="CLOSE_REMARKS" Width="100px" VisibleIndex="10">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataDateColumn Caption="Reopen Date" Width="100px" FieldName="REOPEN_DATE" VisibleIndex="11" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataDateColumn>

                        <dxe:GridViewDataTextColumn Caption="Feedback for Reopen" FieldName="REOPEN_FEEDBACK" Width="120px" VisibleIndex="12">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Status" FieldName="ORDERSTATUS" Width="100px" VisibleIndex="13">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                    <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                    <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                    <SettingsEditing Mode="EditForm" />
                    <SettingsContextMenu Enabled="true" />
                    <SettingsBehavior AutoExpandAllGroups="true" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                    <SettingsSearchPanel Visible="false" />
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                    </SettingsPager>
                    <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                    <TotalSummary>
                        <dxe:ASPxSummaryItem FieldName="CUSTNAME" SummaryType="Custom" Tag="Custname"/>
                        <dxe:ASPxSummaryItem FieldName="BUDGET" SummaryType="Sum" Tag="Budget"/>
                        <dxe:ASPxSummaryItem FieldName="BUDGET_MONTHWISE" SummaryType="Sum" Tag="BudgetMnth"/>
                        <dxe:ASPxSummaryItem FieldName="CLOSE_QTY" SummaryType="Sum" Tag="CloseQty"/>
                    </TotalSummary>
                </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                    ContextTypeName="ReportSourceDataContext" TableName="DSRONOPPORTUNITIES_REPORT"></dx:LinqServerModeDataSource>
      </div>
    

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsDSRONOPPORTUNITYDetFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
          <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
</asp:Content>
