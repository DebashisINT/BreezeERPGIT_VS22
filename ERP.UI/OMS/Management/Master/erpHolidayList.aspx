<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="erpHolidayList.aspx.cs" Inherits="ERP.OMS.Management.Master.erpHolidayList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function OnClickDelete(val) {
            $.ajax({
                type: "POST",
                url: "erpHolidayList.aspx/DeleteHoliday",
                data: JSON.stringify({ HolidayID: val }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Holiday Delete Sucessfully");
                            var url = 'erpHolidayList.aspx';
                            window.location.href = url;
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


        function OnAddCodeButtonClick() {
            var url = "erp_addHoliday.aspx?Key=ADD";
            window.location.href = url;
        }

        function ClickOnEdit(id) {
            location.href = "erp_addHoliday.aspx?id=" + id+"&Key=edit";
        }

        function ClickOnView(id) {
            location.href = "erp_addHoliday.aspx?id=" + id + "&Key=view";
        }

        function gridRowclick(s, e) {
            //alert('hi');
            $('#GrdHolidays').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdHolidays.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdHolidays.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdHolidays.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdHolidays.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Holidays</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" cellpadding="0px" width="100%">
            <tr>
                <td>
                    <% if (rights.CanAdd)
                       { %>
                    <button type="button" class="btn btn-success btn-radius" onclick="OnAddCodeButtonClick()" id="Team">
                        <span class="btn-icon"><i class="fa fa-plus"></i></span> Add Holiday
                    </button>
                    <%} %>

                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" AutoPostBack="true" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>
                </td>
            </tr>
            <tr>
                <td class="gridcellcenter relative" colspan="2">
                    <dxe:ASPxGridView ID="GrdHolidays" runat="server" KeyFieldName="HOLIDAYID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="280" Settings-HorizontalScrollBarMode="Auto" 
                        DataSourceID="EntityServerModeDataSource" 
                        Width="100%" ClientInstanceName="cGrdHolidays">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Holiday Code" FieldName="HOLIDAY_CODE"
                                VisibleIndex="1" FixedStyle="Left" Width="17%">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Holiday Description" FieldName="HOLIDAY_DESC"
                                VisibleIndex="2" Width="17%">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCH" VisibleIndex="3" Width="15%">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Start Date" Width="10%" FieldName="FROMDATE" VisibleIndex="3">
                                <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="End Date" FieldName="TODATE" Width="6%" VisibleIndex="4">
                                <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="ENTRY_NAME" Width="10%" VisibleIndex="5">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataTextColumn Caption="Entered On" FieldName="ENTRY_DATE" Width="6%" VisibleIndex="6">
                               <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UPDATE_NAME" Width="10%" VisibleIndex="7">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="UPDATE_DATE" Width="9%" VisibleIndex="8">
                                <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy hh:mm:ss"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="9" Width="0">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                         <% if (rights.CanView)
                                           { %>
                                        <a href="javascript:void(0);" onclick="ClickOnView('<%# Container.KeyValue %>')" id="a_viewInvoice" class="" title="">
                                            <span class='ico editColor'><i class='fa fa-eye' aria-hidden='true'></i></span><span class='hidden-xs'>View</span></a><%} %>
                                        <% if (rights.CanEdit)
                                           { %>
                                        <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                                        <% if (rights.CanDelete)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="" id="a_delete">
                                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                        <%} %>
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
                       <ClientSideEvents RowClick="gridRowclick" />
                        <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsLoadingPanel Text="Please Wait..." />
                    </dxe:ASPxGridView>
                    <dx:linqservermodedatasource id="EntityServerModeDataSource" runat="server" onselecting="EntityServerModeDataSource_Selecting"
                        contexttypename="ERPDataClassesDataContext" tablename="ERP_TEAMVIEW" />
                     <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                    </dxe:ASPxGridViewExporter>
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
