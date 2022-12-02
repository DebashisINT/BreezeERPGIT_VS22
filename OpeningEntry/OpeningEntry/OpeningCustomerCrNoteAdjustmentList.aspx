<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="OpeningCustomerCrNoteAdjustmentList.aspx.cs" Inherits="OpeningEntry.OpeningEntry.OpeningCustomerCrNoteAdjustmentList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        var isFirstTime = true;
        document.onkeydown = function (e) {
            if (event.keyCode == 65 && event.altKey == true) {

                if (document.getElementById('AddId'))
                    OnAddClick();
            }
        }
        function AllControlInitilize() {
            if (isFirstTime) {

                //if (localStorage.getItem('AdvanceAdjFromDate')) {
                //    var fromdatearray = localStorage.getItem('AdvanceAdjFromDate').split('-');
                //    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                //    cFormDate.SetDate(fromdate);
                //}
                //if (localStorage.getItem('AdvanceAdjToDate')) {
                //    var todatearray = localStorage.getItem('AdvanceAdjToDate').split('-');
                //    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                //    ctoDate.SetDate(todate);
                //}
                //if (localStorage.getItem('AdvanceAdjListBranch')) {
                //    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('AdvanceAdjListBranch'))) {
                //        ccmbBranchfilter.SetValue(localStorage.getItem('AdvanceAdjListBranch'));
                //    }
                //}
                if ($("#LoadGridData").val() == "ok")
                    updateGridByDate();

                isFirstTime = false;
            }
        }
        function onEditClick(id) {
            window.location.href = 'CustomerCrNoteAdjustment.aspx?Key=' + id;
        }
        function OnClickDelete(id) {
            jConfirm("Confirm Delete?", "Alert", function (ret) {
                if (ret)
                { cgridAdvanceAdj.PerformCallback("Del~" + id); }
            });
        }
        function OnViewClick(keyValue) {
            var url = '/OMS/Management/Activities/CustomerCrNoteAdjustment.aspx?key=' + keyValue + '&req=V';
            window.location.href = url;
        }
        function GridEndCallBack() {
            if (cgridAdvanceAdj.cpReturnMesg) {
                jAlert(cgridAdvanceAdj.cpReturnMesg, "Alert", function () { cgridAdvanceAdj.Refresh(); });
                cgridAdvanceAdj.cpReturnMesg = null;
            }
        }
        function updateGridByDate() {
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

                localStorage.setItem("AdvanceAdjFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("AdvanceAdjToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("AdvanceAdjListBranch", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                cgridAdvanceAdj.Refresh();
            }
        }
        function OnAddClick() {
            window.location.href = 'CustomerCrNoteAdjustment.aspx?Key=Add';
        }

    </script>
    <style>
        .padTab {
            margin-bottom: 4px;
            margin-top: 8px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
                vertical-align: middle;
            }

                .padTab > tbody > tr > td > label, .padTab > tbody > tr > td > input[type="button"] {
                    margin-bottom: 0 !important;
                    font-size: 14px;
                }

            .padTab > tbody > tr > td {
                font-size: 14px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>


    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Adjustment of Documents - Credit Note With Invoice</h3>
        </div>
    </div>
    <table class="padTab">
        <tr>
            <td>
                <label>From Date</label></td>
            <td>
                <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </td>
            <td>
                <label>To Date</label>
            </td>
            <td>
                <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
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

    <div class="form_main">
        <% if (rights.CanAdd)
           { %>
     <%--   <a href="javascript:void(0);" onclick="OnAddClick()" id="AddId" class="btn btn-success btn-xs"><span><u>A</u>dd Adjustment</span> </a>--%>
        <%} %>

        <% if (rights.CanExport)
           { %>
      <%--  <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary pull-right btn-xs" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <asp:ListItem Value="1">PDF</asp:ListItem>
            <asp:ListItem Value="2">XLS</asp:ListItem>
            <asp:ListItem Value="3">RTF</asp:ListItem>
            <asp:ListItem Value="4">CSV</asp:ListItem>
        </asp:DropDownList>--%>
        <% } %>




        <div class="GridViewArea">

            <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridAdvanceAdj" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>


            <dxe:ASPxGridView ID="gridAdvanceAdj" runat="server" KeyFieldName="Adj_id" AutoGenerateColumns="False"
                Width="100%" ClientInstanceName="cgridAdvanceAdj" SettingsBehavior-AllowFocusedRow="true"
                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                 OnSummaryDisplayText="gridAdvanceAdj_SummaryDisplayText"
                SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeDataSource" OnCustomCallback="gridAdvanceAdj_CustomCallback" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >

                <SettingsSearchPanel Visible="True" Delay="5000" />
                <Columns>
                     <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="Adj_id" Caption="Adj_id" SortOrder="Descending">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                         
<Settings AllowAutoFilterTextInputTimer="False" />

                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Adjustment_No" Width="200"
                        VisibleIndex="0" FixedStyle="Left">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        
<Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataDateColumn Caption="Posting Date" FieldName="Adjustment_Date" Width="200"
                        VisibleIndex="0" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        
<Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataDateColumn>
                    <dxe:GridViewDataTextColumn Caption="Customer(s)" FieldName="Customer" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Adjusted Document" FieldName="Adjusted_Doc_no" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        
<Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Adjusted Amount" FieldName="Adjusted_Amount" Width="200" HeaderStyle-HorizontalAlign="Right"
                        VisibleIndex="0">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <CellStyle CssClass="gridcellleft" Wrap="true" HorizontalAlign="Right">
                        </CellStyle>
                        
<Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        
<Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Created By" FieldName="CreatedBy" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        
<Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="UpdatedOn" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                         
<Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Last Updated By" FieldName="LastUpdatedBy" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        
<Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="170">
                        <DataItemTemplate>
                             <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="View">
                                <img src="../../../assests/images/viewIcon.png" /></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                         <%--   <a href="javascript:void(0);" class="pad" title="Edit" onclick="onEditClick('<%# Container.KeyValue %>')">
                                <img src="../../../assests/images/info.png" /></a>--%>
                            
                                              <%} %>

                            <% if (rights.CanDelete)
                               { %>
                        <%--    <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                <img src="../../../assests/images/Delete.png" /></a>--%>
                            <%} %>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        
<Settings AllowAutoFilterTextInputTimer="False" />

                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>

                    </dxe:GridViewDataTextColumn>


                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                  <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Adjusted_Amount" SummaryType="Sum" />                   
                </TotalSummary>

                <SettingsPager NumericButtonCount="10"  PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                </SettingsPager>

                <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsLoadingPanel Text="Please Wait..." />
                <ClientSideEvents EndCallback="GridEndCallBack" />

            </dxe:ASPxGridView>
            <dx:linqservermodedatasource id="EntityServerModeDataSource" runat="server" onselecting="EntityServerModeDataSource_Selecting"
                contexttypename="ERPDataClassesDataContext" tablename="v_CreditNoteAdjustment" />
            <asp:HiddenField ID="hfIsFilter" runat="server" />
            <asp:HiddenField ID="hfFromDate" runat="server" />
            <asp:HiddenField ID="hfToDate" runat="server" />
            <asp:HiddenField ID="hfBranchID" runat="server" />
            <asp:HiddenField ID="hiddenedit" runat="server" />
        </div>
    </div>
</asp:Content>

