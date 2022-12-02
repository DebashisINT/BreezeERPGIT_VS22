<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SrvMastSMOList.aspx.cs" Inherits="ERP.OMS.Management.Master.SrvMastSMOList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script language="javascript" type="text/javascript">
         function OnViewExecutive(keyValue) {
             var url = 'SrvMastSMO.aspx?id=' + keyValue + '&Type=view';
             window.location.href = url;
         }

         function OnEditButtonClick(keyValue) {
             var url = 'SrvMastSMO.aspx?id=' + keyValue;
             window.location.href = url;
         }

         function EndCall(obj) {
             if (grid.cpDelmsg != null) {
                 jAlert(grid.cpDelmsg);
                 grid.cpDelmsg = null;
             }

             if (grid.cpImportModel != null) {
                 if (grid.cpImportModel == 'Success') {
                     jAlert('Import successfully');
                     cImportPopupModel.Hide();
                 }
                 else {
                     jAlert("No data found!");
                     cImportPopupModel.Hide();
                 }
             }
         }

         function OnAddButtonClick() {
             var url = 'SrvMastSMO.aspx?id=ADD';
             window.location.href = url;
         }

         function DeleteRow(keyValue) {

             jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                 if (r == true) {
                     grid.PerformCallback('Delete~' + keyValue);
                     grid.Refresh();
                     grid.Refresh();
                 }
             });

         }
         function gridRowclick(s, e) {
             $('#gridFinancer').find('tr').removeClass('rowActive');
             $('.floatedBtnArea').removeClass('insideGrid');
             $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
             $(s.GetRow(e.visibleIndex)).addClass('rowActive');
             setTimeout(function () {
                 var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                 $.each(lists, function (index, value) {
                     setTimeout(function () {
                         console.log(value);
                         $(value).css({ 'opacity': '1' });
                     }, 100);
                 });
             }, 200);
         }

         function fn_ImportPopUpOpen() {
             //WorkingRoster();
             //if (rosterstatus) {
                 //$('#valid').attr('style', 'display:none;');
                 cImportPopupModel.Show();
             //}
             //else {
             //    $("#divPopHead").removeClass('hide');
             //}
         }

         function btnImport_Vendor() {
             if ($("#ddlCompany").val() == "") {
                 jAlert('Please select company');
             }
             else {
                 grid.PerformCallback('ImportModel~' + $("#ddlCompany").val());
             }
         }
    </script>
    <style>
        .floatedBtnArea{
            top:2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>OSTB Vendor</h3>
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
                                <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span>OSTB Vendor</a>
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
                                 <% if (IsImport)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_ImportPopUpOpen()" class="btn btn-success btn-radius"><span class="btn-icon"></span>Add/Update Import </a>
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
                        KeyFieldName="cnt_id" runat="server" DataSourceID="MSOServerModeDataSource" 
                        AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback" SettingsDataSecurity-AllowEdit="false" 
                        SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Settings-HorizontalScrollBarMode="Auto">
                        <%--DataSourceID="gridFinancerDataSource" --%>
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <ClientSideEvents EndCallback="function(s, e) {
	                              EndCall(s.cpEND);
                            }" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="True" FieldName="MSOCode" Caption="OSTB Vendor Code" Width="80px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="Network_Name" Caption="Name" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Visible="True" FieldName="ContactPerson" Caption="Contact Person" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Visible="True" FieldName="ContactNo" Caption="Contact Number" Width="80px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Visible="True" FieldName="BRANCH" Caption="Branch" Width="400px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Visible="True" FieldName="EMPLOYEE_NAME" Caption="Employee Name" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Visible="True" FieldName="AddressType" Caption="Address Type" Width="80px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Visible="True" FieldName="Address1" Caption="Address-1" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Visible="True" FieldName="Country" Caption="Country" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Visible="True" FieldName="State" Caption="State" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Visible="True" FieldName="City" Caption="District" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="Pin_Code" Caption="Pin" Width="80px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="Statuss" Caption="Status" Width="80px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Visible="True" FieldName="CREATE_ON" SortOrder="Descending" Caption="Created On" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="100px">
                                <CellStyle CssClass="gridcellleft"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="True" FieldName="CREATE_BY" Caption="Created By" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Visible="True" FieldName="MODIFY_ON" Caption="Updated On" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Visible="True" FieldName="MODIFY_BY" Caption="Updated By" Width="200px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="15" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">


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

                                         <% if (rights.CanView)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnViewExecutive('<%# Container.KeyValue %>')" title="" class="pad">
                                            <span class='ico ColorSeven'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span>
                                        </a>
                                         <% } %>
                                    </div>
                                </DataItemTemplate>
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsText ConfirmDelete="Confirm delete?" />
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <ClientSideEvents RowClick="gridRowclick" />
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    </dxe:ASPxGridView>
                     <dx:linqservermodedatasource id="MSOServerModeDataSource" runat="server" onselecting="MSOServerModeDataSource_Selecting"
                        contexttypename="ERPDataClassesDataContext" tablename="MSO_Report" />
                   
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



    
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ImportPopupModel" runat="server" ClientInstanceName="cImportPopupModel"
            Width="471px" Height="100px" HeaderText="Import Model" PopupHorizontalAlign="Windowcenter"
            PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
            <ContentCollection>
                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <div class="Top clearfix">
                        <div style="padding-top: 5px;" class="col-md-12">
                            <div class="stateDiv" style="padding-top: 5px; width: 100px;">From Company :<span style="color: red;">*</span></div>
                            <div style="padding-top: 5px;">
                                <select id="ddlCompany" class="form-control">
                                    <option value="BRZ_GTPLINV">GTPL_INV</option>
                                    <option value="GTPL">GTPL_SRV</option>
                                    <option value="BRZ_GTPLSTB">GTPL_STB</option>
                                </select>

                            </div>
                        </div>
                    </div>
                    <div class="ContentDiv">
                        <div class="ScrollDiv"></div>
                        <br style="clear: both;" />
                        <div class="Footer" style="padding-left: 84px;">
                            <div style="float: left;">
                                <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtnSave_States" runat="server"
                                    AutoPostBack="False" Text="Import" CssClass="btn btn-primary">
                                    <ClientSideEvents Click="function (s, e) {btnImport_Vendor();}" />
                                </dxe:ASPxButton>
                            </div>
                            <div style="">
                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                    <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                </dxe:ASPxButton>
                            </div>
                            <br style="clear: both;" />
                        </div>
                        <br style="clear: both;" />
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <dxe:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
