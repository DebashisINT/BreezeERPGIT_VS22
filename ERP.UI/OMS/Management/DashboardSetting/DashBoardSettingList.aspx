<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DashBoardSettingList.aspx.cs" Inherits="ERP.OMS.Management.DashboardSetting.DashBoardSettingList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #EmployeeGrid_DXPagerBottom {
            min-width: 100% !important;
        }
    </style>
    <script>
        function OnAddButtonClick() {
            location.href = "DashboardSetting.aspx?id=ADD";
        }
        function ClickOnEdit(id) {
            location.href = "DashboardSetting.aspx?id=" + id;
        }
        function OnClickDelete(id) {

            jConfirm("Confirm Delete?", "Title", function (ret) {

                if (ret == true) {
                    $.ajax({
                        type: "POST",
                        url: "DashBoardSettingList.aspx/DeleteDashBoard",
                        data: JSON.stringify({ 'id': id }),
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            console.log(response);
                            if (response.d) {
                                if (response.d == "true") {
                                    jAlert("Deleted Successfully");
                                    grid.Refresh();
                                }
                                else {
                                    jAlert(response.d);
                                }

                            }

                        },
                        error: function (response) {

                            console.log(response);
                        }
                    });

                }

            });


        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" id="td_dashboard" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Dashboard Settings"></asp:Label>
            </h3>
        </div>
    </div>

    <div class="form_main">
        <div class="clearfix mBot10">
                <% if (rights.CanAdd)
                      { %>
                        <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span>Add New</span> </a>
                   <% } %>
                    </div>
        <table class="TableMain100">
            <tr>
                <td>
                    
                    <dxe:ASPxGridView ID="EmployeeGrid" OnDataBinding="EmployeeGrid_DataBinding" runat="server" KeyFieldName="id" AutoGenerateColumns="False"
                        Width="100%" ClientInstanceName="grid"
                        SettingsBehavior-AllowFocusedRow="true" Settings-HorizontalScrollBarMode="Visible" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />

                        <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" FilterRowMode="Auto" />

                        <%--<SettingsText PopupEditFormCaption="Add/ Modify Employee" ConfirmDelete="Confirm delete?" />--%>
                        <%--<StylesPager>
                            <Summary Width="100%">
                            </Summary>
                        </StylesPager>--%>
                        <Columns>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" FieldName="group_name" Caption="Setting Name" Width="20%">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="cr_by" Caption="Created By" Width="15%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>

                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataDateColumn VisibleIndex="3" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy" FieldName="created_on" Caption="Created On" Width="15%" >
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>

                            </dxe:GridViewDataDateColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="mod_by" Caption="Modified By" Width="15%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>

                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataDateColumn VisibleIndex="5" FieldName="mod_on" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy" Caption="Modified On" Width="15%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>

                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center" Width="21%">

                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                  <% if (rights.CanEdit)
                                     { %>
                                    <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Eval("id") %>')" title="Edit" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                   <%}%>

                                    <% if (rights.CanDelete)
                                               { %>
                                    <a href="javascript:void(0);" onclick="OnClickDelete('<%# Eval("id") %>')" title="Delete" class="pad">
                                        <img src="/assests/images/Delete.png" />

                                    </a>
                                     <%} %>
                                    <%--<%  if (rights.CanCreateActivity)
                                        {%>
                                    <a href="javascript:void(0);" onclick="OnCreateActivityClick('<%# Eval("Id") %>','<%# Eval("cnt_id") %>','<%# Eval("Status") %>')" title="Create Activity" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/activity.png" />
                                    </a>
                                    <% }
                                        if (rights.CanEdit)
                                        {%>
                                   

                                    <% }
                                           if (rights.CanView)
                                           {%>
                                    <a href="javascript:void(0);" onclick="ClickVIewInfo('<%# Eval("Id") %>')" title="View" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/doc.png" />
                                    </a>


                                    <% }
                                           if (rights.CanContactPerson)
                                           {%>

                                    <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>')" title="Add Contact Person" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/show.png" />
                                    </a>
                                    <% }
                                           if (rights.CanIndustry)
                                           { %>
                                    <a href="javascript:void(0);" onclick="OnAddBusinessClick('<%#Eval("Id") %>','<%#Eval("Name") %>')" title="Map Industry" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/icoaccts.gif" />
                                    </a>

                                    <%     }
                                           if (rights.CanDelete)
                                           { %>
                                    <a href="javascript:void(0);" onclick="OnDelete('<%# Eval("Id") %>')" title="Delete" class="pad">
                                        <img src="/assests/images/Delete.png" /></a>
                                    <%   }%>

                                    <%  if (rights.CanBudget)
                                        { %>

                                    <a href="javascript:void(0);" onclick="OnBudgetopen('<%# Eval("cnt_Id") %>')" title="Budget" class="pad">
                                        <img src="/assests/images/cashbudget.png" width="16" height="16" /></a>

                                    <%   }%>--%>
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>



                        </Columns>

                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>

                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>

    </div>

</asp:Content>
