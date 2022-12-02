<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AnnouncementList.aspx.cs" Inherits="ERP.OMS.Management.Announcement.AnnouncementList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function Addnew() {
            location.href = 'AnnouncementAddEdit.aspx?id=Add';
        }
        function OnMoreInfoClick(id) {
            location.href = 'AnnouncementAddEdit.aspx?id=' + id;
        }
        function OnClickDelete(id) {
            cgrid.PerformCallback(id);
        }

        function gridEndCallback() {
            if (cgrid.cpRetMsg) {
                jAlert(cgrid.cpRetMsg);
                cgrid.cpRetMsg = null;
            }
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cgrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cgrid.SetWidth(cntWidth);
            }
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">

            <h3>Announcement List</h3>

            

        </div>
    </div>


    <div class="form_main">
            <% if(rights.CanAdd){ %>
        <button type="button" class="btn btn-success btn-radius" onclick="Addnew()">
            <span class="btn-icon"><i class="fa fa-plus" ></i></span>
            Add New
        </button>
          <%} %>


        <dxe:ASPxGridView ID="ASPxGridView1" runat="server" ClientInstanceName="cgrid" KeyFieldName="AncId"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" DataSourceID="LinqServerModeDataSource1Count"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true" OnCustomCallback="ASPxGridView1_CustomCallback"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
            <ClientSideEvents EndCallback="gridEndCallback" />
            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true" />
            <Settings ShowFooter="true" />
            <SettingsContextMenu Enabled="true" />

            <Columns>

                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Title" FieldName="title" Width="20%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Annoucement" FieldName="annoucement" Width="40%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataDateColumn Caption="From " Width="20%" FieldName="FromDate"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dxe:GridViewDataDateColumn>


                <dxe:GridViewDataDateColumn Caption="To" Width="20%" FieldName="ToDate"
                    PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                </dxe:GridViewDataDateColumn>


                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="170">
                    <DataItemTemplate>

                        <% if(rights.CanEdit){ %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="pad" title="Edit">
                            <img src="../../../assests/images/info.png" /></a>
                        <%} %>

                        <% if(rights.CanDelete){ %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                            <img src="../../../assests/images/Delete.png" /></a>
                         <%} %>

                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />

                </dxe:GridViewDataTextColumn>
            </Columns>


        </dxe:ASPxGridView>

    </div>

    <dx:LinqServerModeDataSource ID="LinqServerModeDataSource1Count" runat="server" OnSelecting="LinqServerModeDataSource1Count_Selecting" />
</asp:Content>
