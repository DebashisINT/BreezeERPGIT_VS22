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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>

                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
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
    <div>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:Content>
