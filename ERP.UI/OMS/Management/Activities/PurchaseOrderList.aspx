<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-04-2023        2.0.37           Pallab              25823: Purchase Order page design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Purchase Order" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderList.aspx.cs" Inherits="ERP.OMS.Management.Activities.PurchaseOrderList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <%--Code Added By Sandip For Approval Detail Section Start--%>
    <script src="JS/PurchaseOrderList.js?V=2.2"></script>
    <link href="CSS/PurchaseOrderList.css" rel="stylesheet" />
    <script>
        // Mantis Issue 25394
        function CallbackPanelEndCall(s, e) {
            CgvPurchaseOrder.Refresh();
        }
        // End of Mantis Issue 25394
    </script>

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

        #FormDate , #toDate , #ASPxDateEditFrom , #ASPxDateEditTo , #tDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #ASPxDateEditFrom_B-1 , #ASPxDateEditTo_B-1 , #tDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #ASPxDateEditFrom_B-1 #ASPxDateEditFrom_B-1Img , #ASPxDateEditTo_B-1 #ASPxDateEditTo_B-1Img ,
        #tDate_B-1 #tDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
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

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }
        .crossBtn
        {
            top: 25px;
                right: 25px;
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GrdSalesReturn
        {
            max-width: 98% !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .mb-10{
            margin-bottom: 10px !important;
        }

        .makeFullscreen >table
        {
            z-index: 0;
        }
        .makeFullscreen .makeFullscreen-icon.half
        {
                z-index: 0;
        }

        #CallbackPanel_LPV
        {
                top: 425px !important;
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
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Purchase Order"></asp:Label>
            </h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 8px;">
            <tr>
                <td>
                    <label>From Date</label></td>
                <td>&nbsp;</td>
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
                <td>&nbsp;</td>
                <td>
                    <label>To Date</label>
                </td>
                <td>&nbsp;</td>
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
                <td>&nbsp;</td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>&nbsp;</td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updatePOGridByDate()" />
                </td>

            </tr>

        </table>
    </div>
        <div class="form_main clearfix" id="btnAddNew">
        <div style="float: left; padding-right: 5px;">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="AddInventoryItem()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>I</u>nventory Items</span> </a>
            <a href="javascript:void(0);" onclick="AddNonInventoryItem()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>N</u>on Inventory Items</span> </a>
            <a href="javascript:void(0);" onclick="AddCapitalItem()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>C</u>apital Goods</span> </a>
            <a href="javascript:void(0);" onclick="AddBothItem()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>B</u>oth Items</span> </a>
            <a href="javascript:void(0);" onclick="AddServiceItem()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>S</u>ervice Items</span> </a>
            <% } %>
            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
            <%--Sandip Section for Approval Section in Design Start --%>
            <span id="spanStatus" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary btn-radius">
                    <span>My Purchase Order Status</span>
                    <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
                </a>
            </span>
            <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-primary btn-radius">
                    <span>Approval Waiting</span>

                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                </a>
                <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>

            </span>
            <%--Sandip Section for Approval Section in Design End --%>
            <dxe:ASPxButton ID="btnGenerate" ClientInstanceName="cbtnGenerate" runat="server" AutoPostBack="false" Text="Generate Barcode" CssClass="btn btn-success btn-radius" UseSubmitBehavior="false">
                <ClientSideEvents Click="function(s, e) {GenerateBarcode();}" />
            </dxe:ASPxButton>
            <dxe:ASPxButton ID="btnPrint" ClientInstanceName="cbtnPrint" runat="server" AutoPostBack="false" Text="Print Barcode" CssClass="btn btn-warning btn-radius" UseSubmitBehavior="false">
                <ClientSideEvents Click="function(s, e) {PrintBarcode();}" />
            </dxe:ASPxButton>
        </div>
    </div>
        <div class="GridViewArea relative">
        <%--Settings-HorizontalScrollBarMode="Auto" --%>
        <div class="makeFullscreen ">
            <span class="fullScreenTitle">Purchase Order</span>
            <span class="makeFullscreen-icon half hovered " data-instance="CgvPurchaseIndent" title="Maximize Grid" id="expandCgvPurchaseOrder"><i class="fa fa-expand"></i></span>
            <dxe:ASPxGridView ID="Grid_PurchaseOrder" runat="server" AutoGenerateColumns="False" KeyFieldName="PurchaseOrder_Id" SettingsBehavior-AllowFocusedRow="true"
                ClientInstanceName="CgvPurchaseOrder" Width="100%" OnSummaryDisplayText="Grid_PurchaseOrder_SummaryDisplayText" OnCustomCallback="Grid_PurchaseOrder_CustomCallback" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                OnDataBinding="Grid_PurchaseOrder_DataBinding" OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control">
                <SettingsSearchPanel Visible="True" />
                <%-- SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" --%>

                <ClientSideEvents />
                <Columns>
                    <dxe:GridViewDataCheckColumn VisibleIndex="0" Visible="false">
                        <EditFormSettings Visible="True" />
                        <EditItemTemplate>
                            <dxe:ASPxCheckBox ID="ASPxCheckBox1" Text="" runat="server"></dxe:ASPxCheckBox>
                        </EditItemTemplate>
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataCheckColumn>
                    <dxe:GridViewDataTextColumn FieldName="PurchaseOrder_Id" Visible="false" SortOrder="Descending">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="IsCancel" Visible="false" SortOrder="Descending">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="IsClosed" Visible="false" SortOrder="Descending">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" FixedStyle="Left" Caption="Document No." FieldName="PurchaseOrder_Number" Width="150px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%--Width="120px"--%>
                    <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Posting Date" FieldName="PurchaseOrderDt" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="120px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%-- Width="100px"--%>
                    <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Vendor" FieldName="Customer" Width="250px">
                        <CellStyle Wrap="true" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%--Width="150px"--%>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Amount" FieldName="ValueInBaseCurrency" HeaderStyle-HorizontalAlign="Right" Width="120px">
                        <CellStyle Wrap="False" CssClass="gridcellleft" HorizontalAlign="Right"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <%--  Width="86px"--%>

                    <%-- Add Revision No and Revision Date Tanmoy--%>
                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProjPOrder_RevisionNo"
                        Caption="Revision No." Width="120px">
                        <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="ProjPOrder_RevisionDate" Settings-AllowAutoFilter="False"
                        Caption="Revision Date" Width="180px">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <%-- Add Revision No and Revision Date Tanmoy--%>

                    <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Indent Number" FieldName="Indent_Numbers" Width="150px">
                        <CellStyle Wrap="True" CssClass="gridcellleft" HorizontalAlign="left"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Indent Date" FieldName="Indent_RequisitionDate" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="120px">
                        <CellStyle Wrap="True" CssClass="gridcellleft" HorizontalAlign="left"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%-- Width="100px"--%>
                    <%-- Width="130px"--%>
                    <%--<dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Status" FieldName="PurchaseOrder_Status">
                    <CellStyle  Wrap="True" CssClass="gridcellleft"></CellStyle>
                </dxe:GridViewDataTextColumn>--%>
                    <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Ship to Party" FieldName="ShiftPartyName" Width="250px">
                        <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%--Width="99px"--%>
                    <%--    <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Invoice Number" FieldName="Invoice_Number" Width="150px">
                        <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>--%>

                    <dxe:GridViewDataTextColumn Caption="Invoice Details" VisibleIndex="10" Width="150px">
                        <DataItemTemplate>

                            <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("PurchaseOrder_Id") %>')">
                                <%--<dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("Invoice_Number")%>'
                                    ToolTip="Invoice Number">
                                </dxe:ASPxLabel>--%>
                                Invoice Details
                            </a>
                            <%--<a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("PurchaseOrder_Id") %>'+'~'+'<%# Eval("Invoice_Number") %>')" style='<%#Eval("MultipleStatus")%>'>
                                <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("Invoice_Number")%>'
                                    ToolTip="Invoice Number">
                                </dxe:ASPxLabel>
                            </a>--%>

                            <%--<a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("PurchaseOrder_Id") %>'+'~'+'<%# Eval("Invoice_Number") %>')" style='<%#Eval("SingleStatus")%>'>
                                <dxe:ASPxLabel ID="ASPxTextBox3" runat="server" Text='<%# Eval("Invoice_Number")%>'
                                    ToolTip="Invoice Number">
                                </dxe:ASPxLabel>
                            </a>--%>

                            <%--  <asp:Label ID="lblSN" runat="server" Text='<%# Eval("InvoiceNo") %>' style='<%#Eval("SingleStatus")%>'></asp:Label>--%>
                        </DataItemTemplate>
                        <EditFormSettings Visible="False" />
                        <CellStyle Wrap="False" CssClass="text-center" HorizontalAlign="Center">
                        </CellStyle>
                        <%-- <HeaderTemplate>
                                                                                Status
                                                                            </HeaderTemplate>--%>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AllowAutoFilter="False" />
                        <HeaderStyle Wrap="False" CssClass="text-center" />
                    </dxe:GridViewDataTextColumn>




                    <%--Width="120px"--%>
                    <%-- <dxe:GridViewDataTextColumn VisibleIndex="11" Caption="Invoice Date" FieldName="Invoice_Date" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="120px">
                        <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>--%>


                    <%--   <dxe:GridViewDataTextColumn FieldName="Invoice_Date" Caption="Invoice Date" VisibleIndex="11" Width="80px">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnAddEditDateClick(this,'Show~'+'<%# Eval("PurchaseOrder_Id") %>'+'~'+'<%# Eval("Invoice_Date") %>')" style='<%#Eval("MultipleStatus")%>'>
                            <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("Invoice_Date")%>'
                                ToolTip="Invoice Date">
                            </dxe:ASPxLabel>
                        </a>

                        <a href="javascript:void(0);" onclick="OnAddEditDateClick(this,'Show~'+'<%# Eval("PurchaseOrder_Id") %>'+'~'+'<%# Eval("Invoice_Date") %>')" style='<%#Eval("SingleStatus")%>'>
                            <dxe:ASPxLabel ID="ASPxTextBox3" runat="server" Text='<%# Eval("Invoice_Date")%>'
                                ToolTip="Invoice Date">
                            </dxe:ASPxLabel>
                        </a>
                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False" CssClass="text-center">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AllowAutoFilter="False" />
                    <HeaderStyle Wrap="False" CssClass="text-center" />
                </dxe:GridViewDataTextColumn>--%>

                    <%-- <dxe:GridViewDataTextColumn VisibleIndex="12" Caption="Party Invoice Number" FieldName="PartyInvoiceNo" Width="150px">
                        <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>--%>
                    <%--Width="120px"--%>
                    <%-- <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="Party Invoice Date" FieldName="PartyInvoiceDate" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="120px">
                        <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>--%>


                    <dxe:GridViewDataTextColumn VisibleIndex="11" Caption="Place of Supply[GST]" FieldName="PosState" Width="120px">
                        <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="12" Caption="Project Name" FieldName="Proj_Name" Width="120px">
                        <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="Doc_status" Settings-AllowAutoFilter="False"
                        Caption="Status" Width="120px">
                        <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="ProjectPurchase_ApproveStatus" Settings-AllowAutoFilter="False"
                        Caption="Approval Status" Width="120px">
                        <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="EnteredBy"
                        Caption="Entered By" Width="120px">
                        <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="EnteredOn" Settings-AllowAutoFilter="False"
                        Caption="Entered On" Width="180px">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="17" FieldName="UpdatedBy" Settings-AllowAutoFilter="False"
                        Caption="Updated By" Width="120px">
                        <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="18" FieldName="UpdatedOn" Settings-AllowAutoFilter="False"
                        Caption="Updated On" Width="180px">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn FieldName="IsInventory" Caption="" VisibleIndex="22" ReadOnly="True" Width="0" PropertiesTextEdit-Height="15px">
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="18" Width="0">
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>
                                <% if (rights.CanView)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                                    <span class='ico ColorThree'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                <% } %>
                                <% if (rights.CanEdit)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="" title="" style='<%#Eval("POLastEntry")%>'>
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                <% } %>
                                <%--Mantis Issue 24920--%>
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnCopyClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="" title="" style='<%#Eval("POLastEntry")%>'>
                                    <span class='ico editColor'><i class='fa fa-files-o' aria-hidden='true'></i></span><span class='hidden-xs'>Copy</span></a>
                                <% } %>
                                <%--End of Mantis Issue 24920--%>
                                <% if (rights.CanApproved && IsApprove)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnApproveClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="" title="">
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Approve/Reject</span></a>
                                <% } %>

                                <% if (rights.CanDelete)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="" title="" style='<%#Eval("POLastEntry")%>'>
                                    <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                <% } %>

                                <% if (rights.CanCancel)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnCancelClick('<%# Container.KeyValue %>')" class="" title="">
                                    <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel</span></a></a>
                                <% } %>

                                <% if (rights.CanClose)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnClosedClick('<%# Container.KeyValue %>')" class="" title="">
                                    <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Close</span></a>
                                <% } %>
                                <% if (rights.CanView)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="" title="">
                                    <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                                </a>
                                <% } %>

                                <% if (rights.CanPrint)
                                   { %>
                                <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="" title="">
                                    <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>

                                </a><%} %>
                                <% if (rights.CanClose)
                                   { %>
                                <% if (ShowProductWiseClose.ToUpper() == "1")
                                   {%>
                                <a href="javascript:void(0);" onclick="OnProductWiseClosedClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>','<%#Eval("PurchaseOrder_Number") %>')" class="" title="">
                                    <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Product Wise Closed</span></a>
                                <% } %>
                                <% } %>
                                <%--Mantis Issue 25152--%>
                                <% if (rights.SendSMS)
                                   { %>
                                <a href="javascript:void(0);" onclick="onSmsClickJv('<%# Container.KeyValue %>')" id="onSmsClickJv" class="pad" title="Send Sms">
                                    <span class='ico deleteColor'><i class='fa fa-commenting-o' aria-hidden='true'></i></span><span class='hidden-xs'>Send Sms</span>
                                </a><%} %>
                                <%--End of Mantis Issue 25152--%>
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
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="ValueInBaseCurrency" SummaryType="Sum" />
                </TotalSummary>
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <ClientSideEvents EndCallback="function(s, e) {
	                                        ShowMsgLastCall();
                                        }"
                    RowClick="gridRowclick" />
                <SettingsBehavior ConfirmDelete="True" />
                <Styles>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                    <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                    <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                    <Footer CssClass="gridfooter"></Footer>
                </Styles>
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>
                <Settings ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowStatusBar="Hidden"
                    ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" />


            </dxe:ASPxGridView>
        </div>
    </div>
    </div>
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="PurchaseOrderList" />
    <%--<dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>--%>
    <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:HiddenField ID="hdfOpening" runat="server" />

    <%--DEBASHIS--%>
    <div class="PopUpArea">

        <dxe:ASPxPopupControl ID="Popup_Feedback" runat="server" ClientInstanceName="cPopup_Feedback"
            Width="400px" HeaderText="Reason For Cancel" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width: 94%">

                            <tr>
                                <td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                    <dxe:ASPxMemo ID="txtInstFeedback" runat="server" Width="100%" Height="50px" ClientInstanceName="txtFeedback"></dxe:ASPxMemo>
                                    <span id="MandatoryRemarksFeedback" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnFeedbackSave" class="btn btn-primary" onclick="CallFeedback_save()" type="button" value="Save" />
                                    <input id="btnFeedbackCancel" class="btn btn-danger" onclick="CancelFeedback_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />


        </dxe:ASPxPopupControl>


        <dxe:ASPxPopupControl ID="Popup_Closed" runat="server" ClientInstanceName="cPopup_Closed"
            Width="400px" HeaderText="Reason For Close" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width: 94%">

                            <tr>
                                <td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                    <dxe:ASPxMemo ID="ASPxMemo1" runat="server" Width="100%" Height="50px" ClientInstanceName="txtClosed"></dxe:ASPxMemo>
                                    <span id="MandatoryRemarksFeedback1" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnClosedSave" class="btn btn-primary" onclick="CallClosed_save()" type="button" value="Save" />
                                    <input id="btnClosedCancel" class="btn btn-danger" onclick="CancelClosed_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />


        </dxe:ASPxPopupControl>



        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%-- Sandip Approval Dtl Section Start--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
            Width="900px" HeaderText="Pending Approvals" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback" OnPageIndexChanged="gridPendingApproval_PageIndexChanged">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="CreateDate"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch_description"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="craetedby"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Approved">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkapprove" runat="server" AllowGrayed="false" OnInit="chkapprove_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%--<ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%--<ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <%-- <SettingsSearchPanel Visible="True" />--%>
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                                <ClientSideEvents EndCallback="OnApprovalEndCall" />
                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>


                        <%--<div class="col-md-12" style="padding-top: 10px;">
                            <dxe:ASPxButton ID="ASPxButton1" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                                AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {SaveApprovalStatus();}" />
                            </dxe:ASPxButton>
                        </div>--%>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="1200px" HeaderText="Quotation Approval" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <HeaderTemplate>
                <span>User Approval</span>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="PopupUserWiseQuotation" runat="server" ClientInstanceName="cPopupUserWiseQuotation"
            Width="900px" HeaderText="User Wise Purchase Order Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback" OnPageIndexChanged="gridUserWiseQuotation_PageIndexChanged">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <%-- <SettingsSearchPanel Visible="True" />--%>
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />

                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>

    <%-- Sandip Approval Dtl Section End--%>

    <dxe:ASPxPopupControl ID="InvoiceDatepopup" ClientInstanceName="cInvoiceDatepopup" runat="server"
        AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Invoice Date Detail"
        EnableHotTrack="False" BackColor="#DDECFE" Width="850px" CloseAction="CloseButton">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxCallbackPanel ID="InvoiceDatepanel" runat="server" Width="650px" ClientInstanceName="popInvoiceDatePanel"
                    OnCallback="InvoiceDatepanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <div>
                                <dxe:ASPxGridView ID="grdInvoiceDate" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cpbInvoiceDate" OnDataBinding="InvoiceDatepanel_DataBinding">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Number" FieldName="Invoice_Number" HeaderStyle-CssClass="text-left"
                                            VisibleIndex="0" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-left" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Date" FieldName="Invoice_Date" HeaderStyle-CssClass="text-center"
                                            VisibleIndex="1" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Party Invoice No." FieldName="PartyInvoiceNo" HeaderStyle-CssClass="right"
                                            VisibleIndex="2" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Party Invoice Date" FieldName="PartyInvoiceDate" HeaderStyle-CssClass="right"
                                            VisibleIndex="3" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                    </Columns>
                                </dxe:ASPxGridView>
                            </div>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left">
            <Paddings PaddingRight="6px" />
        </HeaderStyle>
        <SizeGripImage Height="16px" Width="16px" />
        <CloseButtonImage Height="12px" Width="13px" />
        <ClientSideEvents CloseButtonClick="function(s, e) {
        popup.Hide();
        }" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="InvoiceNumberpopup" ClientInstanceName="cInvoiceNumberpopup" runat="server"
        AllowDragging="False" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Invoice Number Detail"
        EnableHotTrack="False" BackColor="#DDECFE" Width="850px" CloseAction="CloseButton">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxCallbackPanel ID="InvoiceNumberpanel" runat="server" Width="100%" ClientInstanceName="popInvoiceNumberPanel"
                    OnCallback="InvoiceNumberpanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <div>
                                <dxe:ASPxGridView ID="grdInvoiceNumber" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cpbInvoiceNumber" OnDataBinding="InvoiceNumberpanel_DataBinding">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Number" FieldName="Invoice_Number" HeaderStyle-CssClass="text-left"
                                            VisibleIndex="0" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-left" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Date" FieldName="Invoice_Date" HeaderStyle-CssClass="text-center"
                                            VisibleIndex="1" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Party Invoice No." FieldName="PartyInvoiceNo" HeaderStyle-CssClass="right"
                                            VisibleIndex="2" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Party Invoice Date" FieldName="PartyInvoiceDate" HeaderStyle-CssClass="right"
                                            VisibleIndex="3" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                    </Columns>
                                </dxe:ASPxGridView>
                            </div>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left">
            <Paddings PaddingRight="6px" />
        </HeaderStyle>
        <SizeGripImage Height="16px" Width="16px" />
        <CloseButtonImage Height="12px" Width="13px" />
        <ClientSideEvents CloseButtonClick="function(s, e) {
popup.Hide();
}" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hfIsBarcode" runat="server" />
    </div>

    <div>
        <dxe:ASPxPopupControl ID="PopupWarehouse" runat="server" ClientInstanceName="cPopupWarehouse"
            Width="900px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="DocumentPanel" ClientInstanceName="cDocumentPanel" OnCallback="DocumentPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div>
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <table width="100%" class="tblPad">
                                                    <tr>
                                                        <td>Document No.</td>
                                                        <td>
                                                            <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" ClientInstanceName="ctxtDocNumber" ReadOnly="true" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Unit</td>
                                                        <td>
                                                            <dxe:ASPxTextBox ID="ASPxTextBox2" runat="server" ClientInstanceName="ctxtBatch" ReadOnly="true" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Vendor</td>
                                                        <td>
                                                            <dxe:ASPxTextBox ID="ASPxTextBox3" runat="server" ClientInstanceName="ctxtVendor" ReadOnly="true" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="col-md-6" style="border-left: 1px solid #ccc; height: 98px;">
                                                <table width="100%" class="tblPad">
                                                    <tr>
                                                        <td>Product Name</td>
                                                        <td>
                                                            <dxe:ASPxTextBox ID="ASPxTextBox4" runat="server" ClientInstanceName="ctxtProduct" ReadOnly="true" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Generate Qty</td>
                                                        <td>
                                                            <dxe:ASPxSpinEdit ID="ctxtQty" ClientInstanceName="ctxtQty" runat="server" NumberType="Integer"
                                                                Width="100%" AllowMouseWheel="false">
                                                                <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                            </dxe:ASPxSpinEdit>
                                                        </td>
                                                        <td class="pdLeft">
                                                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_Generate" runat="server" AutoPostBack="False"
                                                                Text="G&#818;enerate" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                                <ClientSideEvents Click="function(s, e) {GenerateProductBarcode();}" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">

                                                <div class="clear"></div>
                                                <dxe:ASPxGridView ID="DocumentGrid" ClientInstanceName="cDocumentGrid" runat="server" AutoGenerateColumns="False"
                                                    KeyFieldName="OrderDetails_Id" Width="100%" Settings-VerticalScrollBarMode="Visible" SettingsBehavior-AllowFocusedRow="true"
                                                    SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollableHeight="300">
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn FieldName="Products_Name" Caption="Product Name" VisibleIndex="0">
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" Width="60" VisibleIndex="1">
                                                        </dxe:GridViewDataTextColumn>
                                                    </Columns>
                                                    <ClientSideEvents RowClick="OnRowClick" />
                                                    <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                                    <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                                                    <SettingsPager Visible="false"></SettingsPager>
                                                </dxe:ASPxGridView>
                                            </div>
                                            <div class="col-md-6" style="border-left: 1px solid #ccc;">

                                                <div class="clear"></div>
                                                <dxe:ASPxGridView ID="StockGrid" ClientInstanceName="cStockGrid" runat="server" AutoGenerateColumns="False"
                                                    KeyFieldName="PurchaseOrder_MapId" Width="100%" Settings-VerticalScrollBarMode="Visible" SettingsBehavior-AllowFocusedRow="true"
                                                    SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollableHeight="300">
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn FieldName="Barcode" Caption="Barcode" VisibleIndex="0">
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="Serial_Number" Caption="Serial No" VisibleIndex="1">
                                                        </dxe:GridViewDataTextColumn>
                                                    </Columns>
                                                    <ClientSideEvents RowClick="OnDetailsRowClick" />
                                                    <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                                    <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                                                    <SettingsPager Visible="false"></SettingsPager>
                                                </dxe:ASPxGridView>
                                                <div class="clear"></div>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="pdTop">Put Serial No</td>
                                                        <td class="pdTop">
                                                            <dxe:ASPxTextBox ID="cSerialNo" runat="server" ClientInstanceName="cSerialNo" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td class="pdTop pdLeft">
                                                            <dxe:ASPxButton ID="cbtn_Save" ClientInstanceName="cbtn_Save" runat="server" AutoPostBack="False"
                                                                Text="Update" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                                <ClientSideEvents Click="function(s, e) {SaveSerial();}" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div>
                                    <asp:HiddenField runat="server" ID="hdfDocNumber" />
                                    <asp:HiddenField runat="server" ID="hdfDocDetailsNumber" />
                                    <asp:HiddenField runat="server" ID="hdfBranch" />
                                    <asp:HiddenField runat="server" ID="hdfMapID" />
                                    <asp:HiddenField runat="server" ID="hdfIsBarcodeActive" />
                                    <asp:HiddenField runat="server" ID="hdfIsBarcodeGenerator" />
                                    <asp:HiddenField ID="hddnKeyValue" runat="server" />

                                    <asp:HiddenField ID="hdnIsMultiuserApprovalRequired" runat="server" />
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="DocumentPanelEndCall" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>

    <dxe:ASPxPopupControl ID="PopupProductwiseClose" runat="server" ClientInstanceName="cPopupProductwiseClose"
        Width="900px" HeaderText="Product wise - Close" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="row">
                    <div class="col-md-4">
                        <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No.">
                        </dxe:ASPxLabel>
                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" ReadOnly="true">                             
                        </asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <dxe:ASPxGridView ID="gridProductwiseClose" runat="server" KeyFieldName="OrderDetails_Id" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgridProductwiseClose" OnCustomCallback="gridProductwiseClose_CustomCallback"
                            OnRowInserting="gridProductwiseClose_RowInserting" OnRowUpdating="gridProductwiseClose_RowUpdating" OnRowDeleting="gridProductwiseClose_RowDeleting" Settings-VerticalScrollableHeight="400" Settings-VerticalScrollBarMode="Visible">
                            <Columns>
                                <dxe:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="60" Caption=" " />
                                <dxe:GridViewDataTextColumn Caption="Product" FieldName="ProductName"
                                    VisibleIndex="1">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description"
                                    VisibleIndex="2">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="3">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Balance Quantity" FieldName="BalQuantity"
                                    VisibleIndex="4">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="OrderDetails_OrderId" ReadOnly="true" Caption="OrderDetails_OrderId" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="OrderDetails_ProductId" ReadOnly="true" Caption="OrderDetails_ProductId" Width="0">
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior AllowFocusedRow="true" />
                            <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <SettingsEditing Mode="Inline">
                            </SettingsEditing>

                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                            <ClientSideEvents EndCallback="OnProductCloseEndCall" />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clear"></div>
                    <div class="text-center pTop10">
                        <dxe:ASPxButton ID="btnCloseProduct" ClientInstanceName="cbtnCloseProduct" runat="server" AutoPostBack="False" Text="Close Product" CssClass="btn btn-primary" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {return CloseProduct();}" />
                        </dxe:ASPxButton>
                    </div>



                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%--Mantis Issue 25152--%>
        <div class="modal fade pmsModal w30" id="assignEmployee" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    
                    <h5 class="modal-title">Select Employee</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    
                </div>
                <div class="modal-body">
                    <div class="row ">
                        
                        <div class="col-md-12 mTop5">
                            <label class="deep">Employee </label>
                            <div class="fullWidth">
                                 <select class="form-control" id="ddl_DirEmployee" style="width:200px" >
                                    <option value="0">--Select--</option>
                                </select>
                                <%--<input type="hidden" id="hdDbName" runat="server" />--%>
                                
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer" id="divsave">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                   
                    <button type="button" class="btn btn-success" onclick="PhoneNoSend();">Confirm</button>
                    
                </div>
            </div>
        </div>
    </div>

        <asp:HiddenField ID="hdnEmployee" runat="server" />
        <asp:HiddenField ID="hdnSettings" runat="server" />
        <asp:HiddenField ID="hdnOrderId" runat="server" />
        <asp:HiddenField  ID="hdDbName" runat="server" />
       <%-- End of Mantis Issue 25152--%>

        <%--Mantis Issue 25394--%>
         <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">           
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="CallbackPanelEndCall" />
        </dxe:ASPxCallbackPanel>
        <%--End of Mantis Issue 25394--%>

</asp:Content>
