<%@ Page Title="Financer" Language="C#" AutoEventWireup="True" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_Financer" CodeBehind="Financer.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



    <script language="javascript" type="text/javascript">
        function OnViewExecutive(keyValue) {
            cPopup_ViewExecutive.Show();
            cAspxExecutiveGrid.PerformCallback(keyValue);
        }

        function OnEditButtonClick(keyValue) {
            var url = 'FinancerAddEdit.aspx?id=' + keyValue;
            window.location.href = url;
        }

        function EndCall(obj) {
            if (grid.cpDelmsg != null) {
                jAlert(grid.cpDelmsg);
                grid.cpDelmsg = null;
            }
        }

        function OnAddButtonClick() {
            var url = 'FinancerAddEdit.aspx?id=ADD';
            window.location.href = url;
        }

        function DeleteRow(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });

        }
        function gridRowclick(s, e) {
            $('#gridFinancer').find('tr').removeClass('rowActive');
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Financer</h3>
        </div>
    </div>
    
    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span>Add New</a>
                                <% } %>
                                <% if (rights.CanExport)
                                   { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>

                                </asp:DropDownList>
                                <% } %>
                            </td>

                        </tr>
                    </table>
                </td>
                <td class="gridcellright" style="float: right; vertical-align: top"></td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2" class="relative">
                    <dxe:ASPxGridView ID="gridFinancer" ClientInstanceName="grid" Width="100%"
                        KeyFieldName="cnt_id" runat="server"
                        AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  ><%--DataSourceID="gridFinancerDataSource" --%>
                       <SettingsSearchPanel Visible="True" Delay="5000"  />
                         <clientsideevents endcallback="function(s, e) {
	                              EndCall(s.cpEND);
                            }" />
                        <columns>
                            <dxe:GridViewDataTextColumn Visible="True" FieldName="cnt_ucc" Caption="Financer ID">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Visible="True" FieldName="cnt_firstName" Caption="Financer Name">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Visible="True" FieldName="branch" Caption="Branch">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn VisibleIndex="10" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">
                                

                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnEditButtonClick('<%# Container.KeyValue %>')" title="" class="">
                                        <span class='ico ColorSix'><i class='fa fa-pencil'></i></span><span class='hidden-xs'>Edit</span>
                                    </a>
                                       <% } %>
                                     <% if (rights.CanDelete)
                                        { %>
                                     <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" title="" class="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                       <% } %>

                                    <a href="javascript:void(0);" onclick="OnViewExecutive('<%# Container.KeyValue %>')" title="" class="pad">
                                        <span class='ico ColorSeven'><i class='fa fa-user'></i></span><span class='hidden-xs'>Executive</span>
                                    </a>
                                        </div>
                                </DataItemTemplate>
                                <HeaderTemplate>Actions</HeaderTemplate> 
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                        </columns>
<SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <settingstext confirmdelete="Confirm delete?" />
                        <styleseditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </styleseditors>
                        <ClientSideEvents RowClick="gridRowclick" />
                        <settingssearchpanel visible="True" />
                        <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />
                        <settingsbehavior columnresizemode="NextColumn" confirmdelete="True" />
                    </dxe:ASPxGridView>
                </td>
                <td>
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </td>
            </tr>
        </table>
        <%--<dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>--%>
        <%--<asp:SqlDataSource ID="gridFinancerDataSource" runat="server" 
            SelectCommand="select h.cnt_id,h.cnt_ucc,h.cnt_firstName,(select branch_description from tbl_master_branch d where d.branch_id=h.cnt_branchId) as branch,* from tbl_master_contact h where cnt_contactType='FI'"></asp:SqlDataSource>--%>
    </div>
</asp:Content>
