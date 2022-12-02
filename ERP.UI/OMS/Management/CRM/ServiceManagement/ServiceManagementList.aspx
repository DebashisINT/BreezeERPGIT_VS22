<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ServiceManagementList.aspx.cs" Inherits="ERP.OMS.Management.CRM.ServiceManagement.ServiceManagemrntList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function OnAddButtonClick() {
            window.location.href = "ServiceManagement.aspx";
        }

    </script>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Service Management </h3>
        </div>

    </div>
    <div class="form_main">
        <div class="clearfix">


            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span><u>A</u>dd Services</span> </a>

            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary hide"
                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>




            <span style="float: right;">
                <input type="button" value="Show" class="btn btn-primary hide" onclick="BindTemplate()" />
            </span>


        </div>

    </div>
    <div>


        <dxe:ASPxGridView ID="gridAttachment" ClientInstanceName="gridDocumentAttachment" runat="server" AutoGenerateColumns="False"
            KeyFieldName="DocID" Width="100%" OnCustomCallback="Grdattachment_CustomCallback" OnDataBinding="gridAttachment_DataBinding" ClientSideEvents-BeginCallback="Callback_BeginCallback"
            SettingsBehavior-AllowFocusedRow="true">

            <Columns>



                <dxe:GridViewDataTextColumn FieldName="Docnumber" VisibleIndex="0" Caption="Service Name">
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="Vendor" VisibleIndex="1" Caption="Service Id">
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Docnumber1" VisibleIndex="2" Caption="Service Description">
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="Vendor1" VisibleIndex="3" Caption="From Date">
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Vendor2" VisibleIndex="4" Caption="To Date">
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Vendor55" VisibleIndex="5" Caption="Renewal Date">
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Last_updated_on" VisibleIndex="6" Caption="Last Updated On" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="updated_on" VisibleIndex="7" Caption="Last Updated By">
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn VisibleIndex="8" HeaderStyle-HorizontalAlign="Center" Caption="Action" CellStyle-HorizontalAlign="Center">
                    <DataItemTemplate>

                        <%--  <% if (rights.CanView)
                            { %>--%>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("DocID") %>')" class="pad" title="View">
                            <img src="../../../assests/images/viewIcon.png" /></a>
                        <%-- <% } %>--%>

                        <a href="javascript:void(0);" onclick="DownloadImage('<%#Eval("DocID") %>')" title="Download" class="pad">
                            <img src="/assests/images/download.png" /></a>


                        <% if (rights.CanDelete)
                           { %>

                        <a href="javascript:void(0);" onclick="DeleteAttachment('<%#Eval("DocID") %>')" title="Delete" class="pad">
                            <img src="/assests/images/delete.png" /></a>

                        <%} %>
                    </DataItemTemplate>
                </dxe:GridViewDataTextColumn>



            </Columns>


            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />

            <SettingsEditing Mode="EditForm" />
            <SettingsContextMenu Enabled="true" />
            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />


            <SettingsPager PageSize="10" Position="Bottom">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>


            <Settings ShowFilterRow="True" ShowStatusBar="Visible" UseFixedTableLayout="true" />


        </dxe:ASPxGridView>



    </div>

    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridAttachment" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>


</asp:Content>
