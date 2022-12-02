<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="erpTeamList.aspx.cs" Inherits="ERP.OMS.Management.Master.erpTeamList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function OnAddTeamButtonClick() {
            var url = 'Team.aspx?id=ADD';
            window.location.href = url;
        }

        function OnClickDelete(val) {
            $.ajax({
                type: "POST",
                url: "erpTeamList.aspx/DeleteTeam",
                data: JSON.stringify({ TEAM_ID: val }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            // jAlert("Team Deleted Successfully.");
                            jAlert("Team Deleted Successfully.", "Alert", function () {
                                setTimeout(function () {
                                    var url = 'erpTeamList.aspx';
                                    window.location.href = url;
                                }, 200);
                            });
                            
                            cGrdTeam.Refresh();
                        }
                        else {
                            alert(response.d);
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }

        function grid_EndCallBack(s, e) {
            //var url = 'erpTeamList.aspx';
            //window.location.href = url;
            cGrdTeam.Refresh();
        }

        function ClickOnEdit(id) {
            location.href = "Team.aspx?id=" + id;
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdTeam.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdTeam.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdTeam.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdTeam.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Team</h3>
        </div>
    </div>
    <div class="form_main" >
        <div class="row">
            <div class="">
                <div class="col-md-3">
                    <% if (rights.CanAdd)
                       { %>
                    <button type="button" class="btn btn-success btn-radius" onclick="OnAddTeamButtonClick()" id="Team" >
                        <span class="btn-icon"><i class="fa fa-plus" ></i></span>
                        Add Team
                    </button>
                    <%} %>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="">
                <div class="col-md-12">
                    <dxe:ASPxGridView ID="GrdTeam" runat="server" KeyFieldName="TEAM_ID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto"
                        DataSourceID="EntityServerModeDataSource"
                        Width="100%" ClientInstanceName="cGrdTeam">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Team Name" FieldName="TEAM_NAME"
                                VisibleIndex="1" FixedStyle="Left" Width="200px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Description" FieldName="DESCRIPTIONS"
                                VisibleIndex="2" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCH_NAME" VisibleIndex="3" Width="150px">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Entry By" FieldName="ENTRY_BY" Width="150px" VisibleIndex="5">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="7" Width="240px">
                                <DataItemTemplate>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                                        <img src="../../../assests/images/info.png" /></a><%} %>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                        <img src="../../../assests/images/Delete.png" /></a>
                                    <%} %>
                                    <%-- <% if (rights.CanView)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnclickView('<%# Container.KeyValue %>')" class="pad" title="View Attachment">
                                        <img src="../../../assests/images/viewIcon.png" />
                                    </a><%} %>--%>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <ClientSideEvents EndCallback="grid_EndCallBack" />
                        <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <%--  <GroupSummary>
                            <dxe:ASPxSummaryItem FieldName="GrossAmount" SummaryType="Sum" DisplayFormat="Total Gross Amount : {0}" />
                            <dxe:ASPxSummaryItem FieldName="ChargesAmount" SummaryType="Sum" DisplayFormat="Total Tax & Charges : {0}" />
                            <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" DisplayFormat="Total Net Amount : {0}" />
                            <dxe:ASPxSummaryItem FieldName="AmountReceived" SummaryType="Sum" DisplayFormat="Total Amount Received : {0}" />
                            <dxe:ASPxSummaryItem FieldName="BalanceAmount" SummaryType="Sum" DisplayFormat="Total Balance Amount : {0}" />
                        </GroupSummary>--%>

                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsLoadingPanel Text="Please Wait..." />
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="ERP_TEAMVIEW" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
