<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="userformList.aspx.cs" Inherits="ERP.OMS.Management.UserForm.userformList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .editClass, .DeleteClass, .PrintClass {
            cursor: pointer;
        }

        .padTab {
            margin-top: 5px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
            }

                .padTab > tbody > tr > td:last-child {
                    padding-right: 0px;
                }
    
    .dxpcLite_PlasticBlue.dxpclW:before {
    content: unset !important; 
}            
    </style>
    <script>

        function ClickEdit() {

            window.location.href = 'UserForm.aspx?ModName=' + $('#hdModuleName').val() + '&&id=' + Grid.GetRowKey(Grid.GetFocusedRowIndex());
        }

        function PrintClick() {
            window.open('CustomPrint.aspx?ModName=' + $('#hdModuleName').val() + '&&id=' + Grid.GetRowKey(Grid.GetFocusedRowIndex()), '_blank')
        }


        function ClickDelete() {
            id = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            jConfirm("Confirm Delete?", "Alert", function (ret) {
                if (ret) {
                    var OtherDet = {};
                    OtherDet.id = id;
                    OtherDet.modName = $('#hdModuleName').val();
                    $.ajax({
                        type: "POST",
                        url: "userformList.aspx/DeleteDetails",
                        data: JSON.stringify(OtherDet),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            jAlert(msg.d.split('~')[1]);
                            if (msg.d.split('~')[0] == '0') {
                                Grid.Refresh();
                                //  Cancel();
                            }

                        }
                    });
                }
            });
        }

        function updateGridByDate() {
            Grid.PerformCallback();
        }

        $(document).ready(function () {
            $('.editClass').on('click', function () {
                setTimeout(function () { ClickEdit(); }, 200);
            });

            $('.DeleteClass').on('click', function () {
                setTimeout(function () { ClickDelete(); }, 200);
            });

            $('.PrintClass').on('click', function () {
                setTimeout(function () { PrintClick(); }, 200);
            });

        });

        function AddClick() {
            window.location.href = 'UserForm.aspx?ModName=' + $('#hdModuleName').val() + '&&id=Add'
        }

        function clickCustomiztionwindow() {
            Grid.ShowCustomizationWindow();
        }

        function listingGridEndCallBack() {
            $('.editClass').on('click', function () {
                setTimeout(function () { ClickEdit(); }, 200);
            });

            $('.DeleteClass').on('click', function () {
                setTimeout(function () { ClickDelete(); }, 200);
            });

            $('.PrintClass').on('click', function () {
                setTimeout(function () { PrintClick(); }, 200);
            });
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                Grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3 class="pull-left">
                <asp:Label ID="ModuleName" runat="server" Text="Hwkgjsaf " />
            </h3>

        </div>
        <table class="padTab pull-right" runat="server" id="dateTable">
            <tr>
                <td>From Date</td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>To Date
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>

                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>

    </div>
    <div class="form_main">

        <%if (rights.CanAdd)
          { %>
        <a href="javascript:void(0);" onclick="AddClick()" class="btn btn-success "><span>Add New</span> </a>
        <%} %>

        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <asp:ListItem Value="2">XLSX</asp:ListItem>
        </asp:DropDownList>

        <input type="button" value="Column Choser" class="btn btn-primary" onclick="clickCustomiztionwindow()" />

        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="Grid" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>


        <dxe:ASPxGridView ID="Grid" ClientInstanceName="Grid"
            runat="server" AutoGenerateColumns="true" OnDataBinding="ASPxGridView1_DataBinding"
            KeyFieldName="id" Width="100%" SettingsBehavior-AllowFocusedRow="true" 
            SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
            OnCustomCallback="Grid_CustomCallback" SettingsBehavior-EnableCustomizationWindow="true" SettingsBehavior-AllowDragDrop="true">
          
              <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true" />
            <Settings ShowFooter="true" />
            <SettingsContextMenu Enabled="true" />


            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
           
            <ClientSideEvents EndCallback="listingGridEndCallBack" />
            <SettingsPager PageSize="10" NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
        </dxe:ASPxGridView>
    </div>


    <asp:HiddenField ID="hdSqlQuery" runat="server" />
    <asp:HiddenField ID="hdModuleName" runat="server" />
</asp:Content>
