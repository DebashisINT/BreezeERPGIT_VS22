<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AuditTrial_Report.aspx.cs" Inherits="Reports.Reports.GridReports.AuditTrial_Report" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">

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
        /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 80px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>


    <script type="text/javascript">

        function fn_OpenDetails(keyValue) {
            cCallbackPanel.PerformCallback('Edit~' + keyValue);
        }

      
    function CallbackPanelEndCall(s, e) {
        Grid.Refresh();
    }
    </script>
    <script type="text/javascript">

        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            var branchid = $('#ddlbranchHO').val();
            $("#hfIsAuditTrialReport").val("Y");

            cCallbackPanel.PerformCallback(data + '~' + $("#ddlDocType").val() + '~' + $("#ddlActionType").val());
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
            $("#drdExport").val(0);
        }

        function OpenBillDetails(branch) {
            $("#drdExport").val(0);
            cgridPendingApproval.PerformCallback('BndPopupgrid~' + branch);
            cpopupApproval.Show();
            return true;
        }

        function popupHide(s, e) {
            cpopupApproval.Hide();
        }

        function OpenPOSDetails(invoice, type) {
            if (type != 'C') {
                url = '/OMS/management/dailytask/CashBankEntry.aspx?key=' + invoice + '&req=V&type=CBE&IsTagged=1';
            }
            popupdetails.SetContentUrl(url);
            popupdetails.Show();
        }

        function DetailsAfterHide(s, e) {
            popupdetails.Hide();
        }

        function Callback2_EndCallback() {
            if (Grid.cpErrorFinancial == 'ErrorFinancial') {
                jAlert('Date Range should be within Financial Year');
            }
            else {
                var Amount = parseFloat(Grid.cpSummary);
                ctxtdiffcalculation.SetText(Amount);
                ctxtdiffcalculationText.SetText('Mismatch Defeated');
                Grid.cpSummary = null;

                if (Amount != 0) {
                    loadCurrencyMassage.style.display = "block";
                }
                else {
                    loadCurrencyMassage.style.display = "none";
                }

                $("#drdExport").val(0);
                Grid.Focus();
                Grid.SetFocusedRowIndex(2);
            }
            Grid.cpErrorFinancial = null;
        }

    </script>
    <script>

        function GethiddenSalesregister() {
            var value1 = '1';

            if (value1 != "0") {
                $("#hdnexpid").val(value1);
                return true;
            }
            else { }
        }

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
        
        #ShowGrid>tbody>tr>td>div.dxgvCSD #ShowGrid_DXMainTable>tbody>tr>td:last-child {
            display:none !important;
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

</asp:content>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">

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
        <div class="row">

            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date  " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date  " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
           
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">Document Type</label>
                <div>
                    <asp:DropDownList ID="ddlDocType" runat="server" Width="100%">
                        <asp:ListItem Value="SI" Text="Sales Invoice"></asp:ListItem>
                        <asp:ListItem Value="SIV" Text="Sales Invoice for Vendor"></asp:ListItem>
                        <asp:ListItem Value="SIT" Text="Sales Invoice for Transporter"></asp:ListItem>
                        <asp:ListItem Value="TSI" Text="Transit Sales Invoice"></asp:ListItem>
                        <%-- Rev Rajdip --%>
                          <asp:ListItem Value="SO" Text="Sales Order"></asp:ListItem>
                        <asp:ListItem Value="PSO" Text="Project Order"></asp:ListItem>
                        <%-- End Rev Rajdip --%>
                        <asp:ListItem Value="DBC" Text="Debit Note Customer"></asp:ListItem>
                        <asp:ListItem Value="PB" Text="Purchase Invoice"></asp:ListItem>
                        <asp:ListItem Value="TPB" Text="Transit Purchase Invoice"></asp:ListItem>
                        <asp:ListItem Value="CNV" Text="Credit Note Vendor"></asp:ListItem>
                        <asp:ListItem Value="CUSTR" Text="Customer Receipt"></asp:ListItem>
                        <asp:ListItem Value="VENDP" Text="Vendor Payment"></asp:ListItem>
                       <%-- REV SAYANTANI--%>
                       <%--<asp:ListItem Value="ESI" Text="Estimate"></asp:ListItem>--%>
                       <%-- END OF REV SAYANTANI--%>
                    </asp:DropDownList>
                </div>
            </div>

             <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">Action Type</label>
                <div>
                    <asp:DropDownList ID="ddlActionType" runat="server" Width="100%">
                        <asp:ListItem Value="All" Text="All"></asp:ListItem>
                        <asp:ListItem Value="I" Text="Add"></asp:ListItem>
                        <asp:ListItem Value="U" Text="Modify"></asp:ListItem>
                        <asp:ListItem Value="D" Text="Delete"></asp:ListItem>
                       <%-- REV SAYANTANI--%>
                       <%-- <asp:ListItem Value="A" Text="Approved"></asp:ListItem>
                        <asp:ListItem Value="C" Text="Cancel"></asp:ListItem>
                        <asp:ListItem Value="O" Text="Re-Open"></asp:ListItem>
                        <asp:ListItem Value="R" Text="Reject"></asp:ListItem>--%>
                        <%--END OF REV SAYANTANI--%>
                    </asp:DropDownList>
                </div>
            </div>



            <div class="col-md-2" style="padding-top: 20px;">
            <table>
                <tr>
                    <td>
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
            
            </div>
            
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
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            Settings-HorizontalScrollBarMode="Visible"  VerticalScrollBarMode="Visible" DataSourceID="GenerateEntityServerModeDataSource" 
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback"
                            Settings-VerticalScrollableHeight="250" >
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="false" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
<ClientSideEvents BeginCallback="Callback_EndCallback"></ClientSideEvents>

                            <SettingsContextMenu Enabled="true" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <SettingsEditing Mode="EditForm" />
                            <Settings ShowFooter="true" ShowGroupPanel="false" ShowGroupFooter="VisibleIfExpanded"/>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" AllowSort ="false" />
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Type" FieldName="DOCTYPE" Width="200px"
                                    VisibleIndex="1" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                              
                                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="DOCNO" Width="200px"
                                    VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Date" Width="130px" FieldName="DOCDATE"
                                    VisibleIndex="3">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <%--Rev Sayantani--%>
                               <%-- <dxe:GridViewDataTextColumn Caption="Revision No." FieldName="REV_NO" Width="200px"
                                    VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Revision Date." Width="130px" FieldName="REV_DATE"
                                    VisibleIndex="5">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>--%>

                                <%--End of Rev Sayantani--%>

                                <dxe:GridViewDataTextColumn Caption="Party" FieldName="CUSTNAME" Width="330px"
                                    VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Value" FieldName="VALUE" Width="100px"
                                    VisibleIndex="5">
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Action" FieldName="ACTION" Width="120px"
                                    VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Done By" FieldName="LOGGEDBY" Width="120px"
                                    VisibleIndex="7">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Done On" FieldName="LOGGEDON" Width="130px"
                                    VisibleIndex="8">
                                     <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy HH:mm:ss"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="AUDIT_TRIAL_REPORT"></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>

    </div>
    <div>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
     <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdetails" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="DetailsAfterHide" />
    </dxe:ASPxPopupControl>
     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsAuditTrialReport" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
 </dxe:ASPxCallbackPanel>
</asp:content>
