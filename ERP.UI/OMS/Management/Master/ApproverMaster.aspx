<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ApproverMaster.aspx.cs" Inherits="ERP.OMS.Management.Master.ApproverMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function SaveApprover() {
            var module = cmodules.GetValue();
            if (module == "0") {
                jAlert('Please select module name.', 'Alert', function () {
                    return;
                });

            }
            cgrid.PerformCallback('Save~' + module);
        }
        function moduletypechange() {
            var module = cmodules.GetValue();
            cgrid.PerformCallback('ShowSelected~' + module);
        }

        function End_Call(s, e) {
            if (cgrid.cpSave != null) {
                jAlert(cgrid.cpSave, 'Alert');
                cgrid.cpSave = null;
            }
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cgrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cgrid.SetWidth(cntWidth);
                }
            }, 200)

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cgrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cgrid.SetWidth(cntWidth);
                }

            });
        });
        function goback(e) {
            //e.preventDefault();
            window.location.href = '/OMS/management/ProjectMainPage.aspx';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Approval Master</h3>
            <div id="crossdiv" class="crossBtn">
                <a href="#" onclick="goback()"><i class="fa fa-times"></i></a>
            </div>
        </div>
    </div>
    <div class="form_main">
        <div class="row">
        <div class="col-md-4" style="margin-bottom:15px">
            <dxe:ASPxComboBox SelectedIndex="0" runat="server" ID="modules" ClientInstanceName="cmodules" DataSourceID="dsModuleList" ValueField="ID" TextField="MODULE_NAME">
                <ClientSideEvents SelectedIndexChanged="moduletypechange" />
            </dxe:ASPxComboBox>
        </div>
        <div class="clear"></div>
   
    
    <div class="clearfix"></div>
    <div class="col-md-12">
        <dxe:ASPxGridView ID="grid" runat="server" KeyFieldName="id" AutoGenerateColumns="False" OnCustomCallback="grid_CustomCallback"
                                Width="100%" ClientInstanceName="cgrid"  SettingsBehavior-AllowFocusedRow="true"
                                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="220" Settings-VerticalScrollBarMode="Auto"
                                 DataSourceID="dsGvDS" ClientSideEvents-EndCallback="End_Call" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                                <SettingsSearchPanel Visible="True" Delay="5000" />
                                <%--SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" --%>
                                
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0"></dxe:GridViewCommandColumn>
                                    <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="sl" Width="50" Visible="false" SortOrder="Descending"
                                        VisibleIndex="1" FixedStyle="Left" >
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Name" FieldName="name" Width="50%"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn Caption="email" FieldName="email"
                                        VisibleIndex="3" Width="50%">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>




                                                                      
                                </Columns>
                                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                <SettingsPager PageSize="10">
                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                </SettingsPager>
                                <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                                </dxe:ASPxGridView>
    </div>
    <div class="col-md-12 mTop5" >
        <button type="button" class="btn btn-danger">Close</button>
        <button type="button" class="btn btn-success" onclick="SaveApprover();">Save</button>
    </div>
            </div>
</div>
    <asp:SqlDataSource ID="dsModuleList" SelectCommand="SELECT 0 AS ID ,'SELECT' AS MODULE_NAME UNION SELECT * FROM MODULELIST_APPROVER" runat="server">

    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsGvDS" SelectCommand="select * from v_approvers" runat="server">

    </asp:SqlDataSource>
</asp:Content>
