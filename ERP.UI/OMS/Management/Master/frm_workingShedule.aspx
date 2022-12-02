<%@ Page Title="Working Hour Schedule" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_frm_workingShedule" CodeBehind="frm_workingShedule.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dxgvControl_PlasticBlue a {
            color: #5A83D0;
        }
    </style>

    <script language="javascript" type="text/javascript">

        function ClickOnMoreInfo(keyValue) {
            var url = 'WorkinhHourAddEdit.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Working Hour Schedule Details", '940px', '450px', "Y");
            window.location.href = url;

        }
        function OnClickDelete(keyValue) {
            
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        function OnAddButtonClick() {
            var url = 'WorkinhHourAddEdit.aspx?id=Add';
            //OnMoreInfoClick(url, "Add Working Hour Schedule Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function callback() {
            grid.PerformCallback();
        }
        function gridRowclick(s, e) {
            //alert('hi');
            $('#WorkingHourGrid').find('tr').removeClass('rowActive');
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
        function grid_EndCallBack() {
            if (grid.cpDelete != null) {
                jAlert(grid.cpDelete);
                grid.cpDelete = null;
            }
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
    <script>
        function ClickOnAssignModule(RosterId) {
            $("#hdnRosterId").val(RosterId);
            var str
            str = { RosterId: RosterId }
            var html = "";
            // alert();
            $.ajax({
                type: "POST",
                url: "frm_workingShedule.aspx/GetModuleList",
                data: JSON.stringify(str),
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (responseFromServer) {
                    for (i = 0; i < responseFromServer.d.length; i++) {
                        if (responseFromServer.d[i].IsChecked == true) {
                            html += "<li><input type='checkbox' id=" + responseFromServer.d[i].ModuleId + "  class='statecheck' onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].ModuleId + " checked  /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].ModuleId + " >" + responseFromServer.d[i].ModuleName + "</label></a></li>";
                        }
                        else {
                            html += "<li><input type='checkbox' id=" + responseFromServer.d[i].ModuleId + " class='statecheck'  onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].ModuleId + "   /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].ModuleId + ">" + responseFromServer.d[i].ModuleName + "</label></a></li>";
                        }
                    }
                    $("#divModalBody").html(html);
                    $("#myModal").modal('show');
                }
            });
        }


        var Modulelist = []
        function ModulePushPop() {
            var RosterId = $("#hdnRosterId").val();
            let a = [];

            $(".statecheckall:checked").each(function () {
                a.push(this.value);
            });

            $(".statecheck:checked").each(function () {
                a.push(this.value);
            });
            var str1
            //  alert(a);

            str1 = { RosterId: RosterId, Modulelist: a }
            $.ajax({
                type: "POST",
                url: "frm_workingShedule.aspx/GetModuleListSubmit",
                data: JSON.stringify(str1),
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (responseFromServer) {
                    // alert(responseFromServer.d)
                    $("#myModal").modal('hide');
                    jAlert('Module assigned successfully');
                }
            });
        }

        function CheckParticular(v) {
            if (v == false) {
                $(".statecheckall").prop('checked', false);
            }
        }

        function CheckAll(id) {
            var ischecked = $(".statecheckall").is(':checked');
            if (ischecked == true) {
                $('input:checkbox.statecheck').each(function () {
                    $(this).prop('checked', true);
                });

            }
            else {
                $('input:checkbox.statecheck').each(function () {
                    $(this).prop('checked', false);
                });

            }


        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Working Roster</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" cellpadding="0px" cellspacing="0px">
            <%--<tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #000099">Working Schedule</span></strong></td>
            </tr>--%>
            <tr>
                <td class="pull-left"><%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>--%>
                    <% if (rights.CanAdd)
                       { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span>Add New</a><%} %>

                    <asp:DropDownList ID="cmbExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <%--<%} %>--%></td>

            </tr>
            <tr>
                <td class="gridcellcenter relative" colspan="2">
                    <dxe:ASPxGridView ID="WorkingHourGrid" runat="server" KeyFieldName="Id" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        DataSourceID="WorkingHourDataSource" Width="100%" ClientInstanceName="grid" OnCustomCallback="EmployeeGrid_CustomCallback" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="0"
                                Caption="Schedule Name" Width="100%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="" VisibleIndex="8" Width="0" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                        <% if (rights.CanEdit)
                                           { %>
                                        <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title=""><span class='ico ColorSix'><i class='fa fa-info'></i></span><span class='hidden-xs'>Edit</span></a>
                                        <%} %>
                                        <% if (rights.CanDelete)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete">
                                            <img src="../../../assests/images/Delete.png" /></a>
                                        <% } %>
                                        <% if (rights.CanAssignTo)
                                           { %>
                                        <a href="javascript:void(0);" onclick="ClickOnAssignModule('<%# Container.KeyValue %>')" title=""><span class='ico ColorSix'><i class='fa fa-sitemap' aria-hidden='true'></i></span><span class='hidden-xs'>Map Module</span></a>
                                        <%} %>
                                    </div>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />

                                <CellStyle Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderTemplate>
                                    <span></span>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <%--<Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>--%>
                        <ClientSideEvents RowClick="gridRowclick" EndCallback="function (s, e) {grid_EndCallBack();}" />
                        <Settings ShowTitlePanel="false" ShowStatusBar="Hidden" ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                        <SettingsText PopupEditFormCaption="Add/ Modify Working Hour Schedule" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormWidth="900px" PopupEditFormHeight="370px" PopupEditFormHorizontalAlign="Center" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormModal="True" EditFormColumnCount="1"></SettingsEditing>
                        <SettingsBehavior AllowFocusedRow="false" ConfirmDelete="True" />
                        
                        <%--<SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20"
                            PageSize="20">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>

        <asp:SqlDataSource ID="WorkingHourDataSource" runat="server"></asp:SqlDataSource>
        <br />
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>

    <div id="myModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog" style="width: 450px;">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Module List</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <input type="text" id="myInput" onkeyup="myFunction()" placeholder="Search for Module.">
                        <ul id="divModalBody" class="listStyle">
                        </ul>
                    </div>
                    <input type="button" id="btnsatesubmit" title="SUBMIT" value="SUBMIT" class="btn btn-primary" onclick="ModulePushPop()" />
                    <input type="hidden" id="hdnstatelist" class="btn btn-primary" />
                    <input type="hidden" id="hdnRosterId" class="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>


</asp:Content>
