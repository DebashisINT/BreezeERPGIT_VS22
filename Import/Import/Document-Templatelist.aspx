<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/Masterpage/ERP.Master" AutoEventWireup="true" CodeBehind="Document-Templatelist.aspx.cs"
    Inherits="Import.Import.Document_Templatelist" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {
            document.onkeydown = function (e) {

                if (event.altKey == true) {
                    switch (event.keyCode) {


                        case 97:

                            StopDefaultAction(e);
                            btnAdd_Click();

                            break;

                        case 65:

                            StopDefaultAction(e);
                            btnAdd_Click();

                            break;

                        case 83:

                            StopDefaultAction(e);
                            grid.PerformCallback();

                            break;

                        case 115:

                            StopDefaultAction(e);
                            grid.PerformCallback();

                            break;


                    }

                }
            }
        });

        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }


        function btnAdd_Click() {

            var url = 'Document-Template.aspx?key=' + 'ADD';
            window.location.href = url;

        }

        function btnDelete_Click(keyValue) {

            jConfirm('Do you want to delete?', 'Confirmation Dialog', function (r) {

                if (r == true) {
                    $.ajax({
                        type: "POST",
                        url: "Document-Templatelist.aspx/DeleteTemplate",
                        data: JSON.stringify({ Id: keyValue }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {

                            var s = data.d;
                            if (s.success == true) {
                                jAlert('Deleted Successfully');

                            }
                            ///val(s.IsDefault);
                            grid.PerformCallback();
                        }
                    });
                }


            });
        }


        function btnOpenlink_Click(keyfield) {

            window.open("../Import/Preview-Template.aspx?T=" + keyfield);

        }

        function BindTemplate() {
            grid.PerformCallback();


        }

        function Callback_BeginCallback() {
            $("#drdExport").val(0);
        }

        function OpenUpdate_Click(keyfield) {
            var url = 'Document-Template.aspx?key=' + keyfield;
            window.location.href = url;
        }


    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Document Template </h3>
        </div>

    </div>

    <div class="form_main">
        <div class="clearfix">
            <div class="row">

                <div class="col-md-12">
                    <% if (rights.CanAdd)
                       { %>
                    <a href="javascript:void(0);" onclick="btnAdd_Click()" class="btn btn-primary" id="btnAdd"><span><u>A</u>dd Template</span> </a>

                    <%} %>


                    <% if (rights.CanExport)
                       { %>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>

                    <%} %>

                    <span style="float: right;">

                        <input type="button" value="Show" class="btn btn-primary" onclick="BindTemplate()" />
                    </span>
                </div>



            </div>
        </div>
    </div>
    <div>

        <dxe:ASPxGridView ID="GrdTemplate" ClientInstanceName="grid" runat="server" KeyFieldName="ID"
            Width="100%" OnCustomCallback="GrdTemplate_CustomCallback" OnDataBinding="GrdTemplate_DataBinding" ClientSideEvents-BeginCallback="Callback_BeginCallback" AutoGenerateColumns="False">
            <styles>
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                    </styles>
            <columns>
                                        <dxe:GridViewDataTextColumn FieldName="ID" Visible="False" VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="Template" VisibleIndex="1" Caption="Template Name">
                                             <Settings AutoFilterCondition="Contains" />
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                               <%--          <dxe:GridViewDataTextColumn FieldName="DocType" VisibleIndex="2" Caption="Type">
                                            <CellStyle CssClass="gridcellleft" Wrap="True">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="IsDefault" VisibleIndex="3" Caption="IsDefault">
                                            <CellStyle CssClass="gridcellleft" Wrap="True">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>--%>


                                        <dxe:GridViewDataTextColumn VisibleIndex="4" Width="100px" Caption="Action">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                            <DataItemTemplate>

                                                <% if (rights.CanEdit)
                                                   { %>
                                             <a href="javascript:void(0);" onclick="OpenUpdate_Click('<%# Container.KeyValue %>')"  title="Edit">
                                                       <img src="../../../assests/images/Edit.png" /></a>
                                                
                                                 <%} %>

                                                 <% if (rights.CanDelete)
                                                    { %>

                                               <a href="javascript:void(0);"  title="Delete"  onclick="btnDelete_Click('<%# Container.KeyValue %>')"><img src="../../../assests/images/Delete.png" /></a>
                                                 
                                                <%} %>

                                                <% if (rights.CanView)
                                                   { %>

                                             <a href="javascript:void(0);"  title="Preview" onclick="btnOpenlink_Click('<%# Container.KeyValue %>')"><img src="../../../assests/images/eye.png" /></a>
                                              <%} %>
                                            </DataItemTemplate>
                                        
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>




                                    </columns>
            <settingssearchpanel visible="True" />
            <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />
            <settingspager numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </settingspager>
            <styleseditors>
                                        <ProgressBar Height="25px">
                                        </ProgressBar>
                                    </styleseditors>
            <settingspager pagesize="10" Position="TopAndBottom">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                        </settingspager>
            <%-- <SettingsBehavior AllowFocusedRow="false" AllowSort="False" />--%>
            <settingssearchpanel visible="True" />
            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />

        </dxe:ASPxGridView>

    </div>

    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdTemplate" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

</asp:Content>
