<%--====================================================== Revision History ============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-02-2023        2.0.36           Pallab              25575 : Report pages modification
====================================================== Revision History ================================================--%>

<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="BankReconciliationStatement.aspx.cs" Inherits="Reports.Reports.GridReports.BankReconciliationStatement" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function selectAll() {
            clookupBank.gridView.SelectRows();
        }

        function unselectAll() {
            clookupBank.gridView.UnselectRows();
        }

        function CloseLookup() {
            clookupBank.ConfirmCurrentSelection();
            clookupBank.HideDropDown();
            clookupBank.Focus();
        }

        function BindOtherDetails(e) {
            clookupBank.gridView.SetFocusedRowIndex(-1);
            cBankPanel.PerformCallback();
        }

        function btn_ShowRecordsClick(e) {
            $("#drdExport").val(0);
            $("#hfIsBRS").val("Y");

            if (clookupBank.GetValue() == null) {
                jAlert('Please select atleast one Bank for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback();
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
            cShowGrid.Refresh();
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

        /*rev Pallab*/
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 80px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        .form_main {
            overflow: hidden;
        }

        label , .mylabel1, .clsTo
        {
            color: #141414 !important;
            font-size: 14px;
                font-weight: 500 !important;
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
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue
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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img
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
        select.btn
        {
            padding-right: 10px !important;
        }

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
            padding: 4px 4px 4px 10px;
            background: #094e8c !important;
        }

        /*table
        {
                max-width: 99%  !important;
        }*/
        /*Rev end 1.0*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cShowGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cShowGrid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cShowGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cShowGrid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <%--Rev 1.0: "outer-div-main" class add: --%>
    <div class="outer-div-main">
        <div class="form_main">
        <div class="row mBot10">
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
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
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>

            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Bank : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxCallbackPanel runat="server" ID="BankPanel" ClientInstanceName="cBankPanel" OnCallback="BankPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupBank" ClientInstanceName="clookupBank" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupBank_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Settings-AutoFilterCondition="Contains" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="2" Caption="ID" Settings-AutoFilterCondition="Contains" Width="0">
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
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />
                                                        <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookup" UseSubmitBehavior="False" />
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
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
                <span id="MandatorClass" style="display: none" class="validclass" />
            </div>
           <%-- <div class="clear"></div>--%>

            <div class="col-md-2">
                <label>&nbsp;</label>
                <div class="">
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                     <% if (rights.CanExport)
                    { %> 
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                            AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">XLSX</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                            <asp:ListItem Value="4">RTF</asp:ListItem>
                        </asp:DropDownList>
                    <% } %>
                </div>
            </div>
        </div>
        <div class="clearfix">
            <%--Rev 1.0: Add grid height--%>
            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="cShowGrid" Width="100%" EnableRowsCache="False" AutoGenerateColumns="False" KeyFieldName="SLNO"
                DataSourceID="GenerateEntityServerModeDataSource" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible"
                Settings-VerticalScrollableHeight="200" Settings-VerticalScrollBarMode="Auto">
                <Columns>
                    <dxe:GridViewDataTextColumn FieldName="BANKNAME" Caption="Bank Name" Width="150" VisibleIndex="0" GroupIndex="0">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DOC_NO" Caption="Document No." Width="150px" VisibleIndex="1">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DOC_DATE" Caption="Document Date" Width="100px" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DOC_TYPE" Caption="Document Type" Width="150px" VisibleIndex="3">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_NO" Caption="Cheque Number" Width="130px" VisibleIndex="4">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_DATE" Caption="Cheque Date" Width="80px" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="RECEIVEDFROM_PAIDTO" Caption="Name of the Payee/Payer" Width="250px" VisibleIndex="6">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="RECONCILE_DATE" Caption="Reconcile Date" Width="100px" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_TYPE" Caption="Instrument Type" Width="120px" VisibleIndex="8">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="RECEIPT_AMOUNT" Caption="Receipt" Width="130px" VisibleIndex="9">
                         <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                        <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="PAYMENT_AMOUNT" Caption="Payment" Width="130px" VisibleIndex="10">
                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                        <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    </dxe:GridViewDataTextColumn>                    
                </Columns>
                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                <SettingsEditing Mode="EditForm" />
                <SettingsContextMenu Enabled="true" />
                <SettingsBehavior AutoExpandAllGroups="true" />
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsSearchPanel Visible="false" />
                <SettingsPager PageSize="50">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                </SettingsPager>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="RECEIVEDFROM_PAIDTO" SummaryType="Custom" />
                    <dxe:ASPxSummaryItem FieldName="RECEIPT_AMOUNT" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="PAYMENT_AMOUNT" SummaryType="Sum" />
                </TotalSummary>
                <GroupSummary>
                    <dxe:ASPxSummaryItem FieldName="RECEIPT_AMOUNT" ShowInGroupFooterColumn="RECEIPT_AMOUNT" SummaryType="SUM" Tag="Item_Receipt"/>
                    <dxe:ASPxSummaryItem FieldName="PAYMENT_AMOUNT" ShowInGroupFooterColumn="PAYMENT_AMOUNT" SummaryType="SUM" Tag="Item_Payment"/>
                </GroupSummary>
            </dxe:ASPxGridView>
             <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
             ContextTypeName="ReportSourceDataContext" TableName="BANKRECONCILIATIONSTATEMENT_REPORT"></dx:LinqServerModeDataSource>
        </div>
    </div>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

  <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsBRS" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
 </dxe:ASPxCallbackPanel>
    </span>

</asp:Content>
