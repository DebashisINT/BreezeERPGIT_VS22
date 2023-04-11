<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                11-04-2023        2.0.37           Pallab              Transactions pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerPendingDeliveryList.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerPendingDeliveryList"
    MasterPageFile="~/OMS/MasterPage/ERP.Master" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">

        var isFirstTime = true;
        function AllControlInitilize() {
            if (isFirstTime) {
                //PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());

                if (localStorage.getItem('CustDvListFromDate')) {
                    var fromdatearray = localStorage.getItem('CustDvListFromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('CustDvListToDate')) {
                    var todatearray = localStorage.getItem('CustDvListToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('CustDvListBranch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('CustDvListBranch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('CustDvListBranch'));
                    }

                }
                //updateGridByDate();
                isFirstTime = false;
            }
        }
        function updateGridByDate() {
            $("#drdExport").val(0);
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

                localStorage.setItem("CustDvListFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("CustDvListToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("CustDvListBranch", ccmbBranchfilter.GetValue());

                $('#branchName').text(ccmbBranchfilter.GetText());
                //PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());
                cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                //$("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                //$("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                //$("#hfBranchID").val(ccmbBranchfilter.GetValue());
                //$("#hfIsFilter").val("Y");
                //cGrdOrder.Refresh();
            }
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdOrder.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdOrder.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdOrder.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdOrder.SetWidth(cntWidth);
                }

            });
        });
    </script>
    <link href="CSS/CustomerPendingDeliveryList.css" rel="stylesheet" />

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            /*-webkit-appearance: none;
            position: relative;
            z-index: 1;*/
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

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
            bottom: 7px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dtPostingDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dtPostingDate_B-1 #dtPostingDate_B-1Img
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
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
                z-index: 0;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
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

        /*input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }*/
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GrdOrder
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

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        #txtProdSearch
        {
            margin-bottom: 10px;
        }

        select.btn
        {
           position: relative;
           z-index: 0;
        }

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        .mb-10{
            margin-bottom: 10px !important;
        }

        /*.btn
        {
            padding: 5px 10px;
        }*/


        /*Rev end 1.0*/

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Customer Pending Delivery List</h3>
            <div id="pageheaderContent" class="scrollHorizontal pull-right wrapHolder content horizontal-images"  style="display:none;">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder" id="idCashbalanace">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Cash Balance </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <b style="text-align: center" id="B_BankBalance" runat="server">0.00</b>
                                                </div>

                                            </td>
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
                                            <td>For Unit </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <asp:Label runat="server" ID="branchName" Text=""></asp:Label>
                                                </div>

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
        <table class="padTab">
            <tr>
                <td>
                    <label>From Date</label></td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
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
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
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
                    <input type="button" value="Show" class="btn btn-success" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>
    </div>
        <div class="form_main">
        <div class="clearfix">
            <%-- <% if (rights.CanAdd)
               { %>--%>
            <%--<a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary" style="display: none;"><span><u>A</u>dd New</span> </a>--%>
            <%-- <% } %>--%>

            <%--<% if (rights.CanExport)
               { %>--%>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" >
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <%--<% } %>--%>
        </div>
    </div>
    
        <div class="GridViewArea">
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="slno" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
            OnDataBinding="GrdOrder_DataBinding"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
            <SettingsSearchPanel Visible="True" Delay="5000" />
       <%-- <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="slno" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdOrder" SettingsBehavior-AllowFocusedRow="true" OnCustomCallback="GrdOrder_CustomCallback">--%>
       <%-- <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="slno" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdOrder" SettingsBehavior-AllowFocusedRow="true" OnCustomCallback="GrdOrder_CustomCallback" 
            DataSourceID="EntityServerModeDataSource">--%>
            <Columns>
                <dxe:GridViewDataTextColumn Caption="SL. NO." FieldName="slno" Width="50" Visible="false" SortOrder="Descending" 
                    VisibleIndex="0" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                   
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="BRANCH NAME" FieldName="BranchName" Width="100"
                    VisibleIndex="1" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False"  AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="DOCUMENT NO." FieldName="InvoiceNo" Width="150"
                    VisibleIndex="2" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="POSTING DATE" FieldName="InvoiceDate" Width="100"
                    VisibleIndex="3" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="CUSTOMER NAME" FieldName="CustomerName" Width="150"
                    VisibleIndex="4" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="CUSTOMER CONTACT NO." FieldName="CustomerPhone" Width="150"
                    VisibleIndex="5" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="SHIPPING ADDRESS" FieldName="CustomerAddress" Width="400"
                    VisibleIndex="6" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains"/>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="PIN CODE" FieldName="CustomerPin" Width="100"
                    VisibleIndex="7" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="PRODUCT" FieldName="product" Width="300"
                    VisibleIndex="8" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="QUANTITY" VisibleIndex="9" Width="100" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                    <PropertiesTextEdit DisplayFormatString="0" Style-HorizontalAlign="Right">
                        <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;" />
                        <Style HorizontalAlign="Right">
                         </Style>
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Right" />
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Available Stock" FieldName="AvailableStock" Width="100"
                    VisibleIndex="10" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Mode of Payment" FieldName="PaymentType" Width="200"
                    VisibleIndex="11" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False"  AutoFilterCondition="Contains"/>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Cheque Cleared" FieldName="ChkCleared" Width="100"
                    VisibleIndex="12" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False"  AutoFilterCondition="Contains"/>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Date of Chq. Clearance" FieldName="ChqDate" Width="150"
                    VisibleIndex="13" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains"/>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="SERIAL NO OF PRODUCT" FieldName="prdSL" Width="200"
                    VisibleIndex="14" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <%--<ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" />--%>
            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
         <%--   <SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
        <%--<dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="v_CustomerPendingDeliveryList" />--%>
                <asp:HiddenField ID="hfIsFilter" runat="server" />
                <asp:HiddenField ID="hfFromDate" runat="server" />
                <asp:HiddenField ID="hfToDate" runat="server" />
                <asp:HiddenField ID="hfBranchID" runat="server" />
    </div>
    </div>
    <div>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:Content>
