<%@ Page Title="User IMEI" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" 
    Inherits="ERP.OMS.Management.Master.TabIMEI" CodeBehind="TabIMEI.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   

    <script language="javascript" type="text/javascript">
        FieldName = null;
       
        function EditBranch(bgid) {
            var url = 'TabIMEIAdd.aspx?Imeiid=' + bgid;
            //window.open(url,'aa'); 
            //OnMoreInfoClick(url, "Edit BranchGroup", '940px', '450px', 'Y');
            window.location.href = url;

        }
        function AddBranch() {
            var url = 'TabIMEIAdd.aspx?Imeiid=add'
            //window.open(url,'aa'); 
            //OnMoreInfoClick(url, "Add BranchGroup", '940px', '450px', 'Y');
            window.location.href = url;

        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }


        function DeleteBranch(bgid) {

          //  var QuoteNo = ctxt_replcno.GetText();
           
                var CheckUniqueCode = false;
                $.ajax({
                    type: "POST",
                    url: "TabIMEI.aspx/DeleteImei",
                    data: JSON.stringify({ Imeid: bgid }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {

                            grid.Refresh();
                        }
                        else {
                           
                        }
                    }
                });
            
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>User IMEIs</h3>
        </div>

    </div>
    <div class="form_main">
         <div class="SearchArea clearfix">
                <div class="FilterSide">
                    <div style="float: left; padding-right: 5px;">
                        <% if (rights.CanAdd)
                           { %>
                      <a href="javascript:void(0);" onclick="AddBranch();" class="btn btn-primary"><span>Add New</span> </a>
                        <% } %>
                      

                        <% if (rights.CanExport)
                                               { %>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                         <% } %>
                    </div></div>
        <table class="TableMain100">
           
            <tr>
                <td>
                    <dxe:ASPxGridView ID="gridtabimei" KeyFieldName="Id" runat="Server" ClientInstanceName="grid"  SettingsBehavior-AllowFocusedRow="true">
                         <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu ="true" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch" VisibleIndex="0">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="User" FieldName="UserId" VisibleIndex="1">
                            </dxe:GridViewDataTextColumn>
                               <dxe:GridViewDataTextColumn Caption="IMEI" FieldName="Imei_No" VisibleIndex="2">
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn  VisibleIndex="3" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Width="5%">
                                <Settings AllowAutoFilter="False"></Settings>
                                <DataItemTemplate>
                                      <% if (rights.CanEdit)
                                        { %>
                                    <a href="javascript:void(0);" onclick="EditBranch('<%# Container.KeyValue %>')" title="Status"><img src="../../../assests/images/Edit.png" />                                                              
                                    </a> <% } %>


                                         <% if (rights.CanDelete)
                                        { %>
                                    <a href="javascript:void(0);" onclick="DeleteBranch('<%# Container.KeyValue %>')" title="Status"><img src="../../../assests/images/delete.png" />                                                              
                                    </a> <% } %>



                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderStyle Wrap="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsBehavior AllowFocusedRow="true" ColumnResizeMode="NextColumn" />
                      
                    </dxe:ASPxGridView>
                </td>
            </tr>

        </table>
    </div>
      <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
</asp:Content>

