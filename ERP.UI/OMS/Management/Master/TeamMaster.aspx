<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TeamMaster.aspx.cs" Inherits="ERP.OMS.Management.Master.TeamMaster" %>


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
        <table class="TableMain100">
            <tr>
                <td>
                    <div style="float: left; padding-right: 5px;">
                <% if (rights.CanAdd)
                      { %>
                        <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span>Add New</span> </a>
                   <% } %>
                    </div>
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
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" FieldName="TEAM_NAME" Caption="Team Name" Width="20%">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" FieldName="TEAM_DESCRIPTION" Caption="Team Description" Width="20%">
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

                           
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center" Width="21%">

                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                     <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Eval("id") %>')" title="Edit" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                    <a href="javascript:void(0);" onclick="OnClickDelete('<%# Eval("id") %>')" title="Delete" class="pad">
                                        <img src="/assests/images/Delete.png" />

                                    </a>
                                    
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