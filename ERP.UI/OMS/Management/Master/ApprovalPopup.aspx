<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ApprovalPopup.aspx.cs" Inherits="ERP.OMS.Management.Master.ApprovalPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .padCenter {
            width: auto;
            margin: 0 auto;
        }

            .padCenter > tbody > tr > td {
                padding: 10px;
            }
    </style>

    <script>
        //function grid_SelectionChanged(s, e) {
        //    s.GetSelectedFieldValues("EuniqueId", GetSelectedFieldValuesCallback);
        //}

        //var value = "";

        //function GetSelectedFieldValuesCallback(values) {
        //    value = "";
        //    for (var i = 0; i < values.length; i++) {
        //        if (i == 0) {
        //            value = values[i];
        //        }
        //        else {
        //            value = value + ',' + values[i];
        //        }
        //    }
        //    //document.getElementById("selCount").innerHTML = grid.GetSelectedRowCount();
        //}

        function ShowClick() {
          

            grid.PerformCallback('BindGrid~' + $("#ddlModules").val() + '~' + cddlStatus.GetValue());
        }

        function ConfirmClick() {

           
            grid.PerformCallback('SaveData~' + $("#ddlModules").val());

        }
        function grid_endcallback() {
            if (grid.cpStatus == "1") {
                grid.cpStatus = null;
                jAlert('Data Saved Successfully.', 'Alert.');
            }

            else if (grid.cpStatus == "2") {
                grid.cpStatus = null;
                jAlert('You must select atleast one document to save.', 'Alert.');
            }
        }


        function getconfirmtype() {

            cCallbackPanel.PerformCallback(cddlStatus.GetValue());
        }


        function OpenDetails(Uniqueid, type, keyValue) {
            var url = '';
            //debugger;
            if (type == 'Customers') {
                CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
                CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
                CAspxDirectCustomerViewPopup.SetHeaderText("View Customer");
                 url = '/OMS/Management/Master/View/ViewCustomer.html?id=' + keyValue;

                CAspxDirectCustomerViewPopup.SetContentUrl(url);

                CAspxDirectCustomerViewPopup.RefreshContentUrl();
                CAspxDirectCustomerViewPopup.Show();
            }

            if (type == 'Vendor') {
                CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
                CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
                CAspxDirectCustomerViewPopup.SetHeaderText("View Vendor");
                 url = '/OMS/management/master/View/ViewVendor.html?id=' + keyValue;

                CAspxDirectCustomerViewPopup.SetContentUrl(url);

                CAspxDirectCustomerViewPopup.RefreshContentUrl();
                CAspxDirectCustomerViewPopup.Show();
            }

           
            if (type == 'Products') {
                CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
                CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
                CAspxDirectCustomerViewPopup.SetHeaderText("View Product");
                url = '/OMS/Management/Master/View/ViewProduct.html?id=' + keyValue;

                CAspxDirectCustomerViewPopup.SetContentUrl(url);

                CAspxDirectCustomerViewPopup.RefreshContentUrl();
                CAspxDirectCustomerViewPopup.Show();
            }
           
           
        }

       

    </script>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Document Verify</h3>

        </div>
    </div>
    <div class="form_main">
        <div class="clearfix row">
            <div class="col-md-2">
                <label>Modules</label>
                <div>
                    <select id="ddlModules" class="form-control">
                        <option value="AccountGroup">Account Group</option>
                        <option value="AccountHead">Account Head</option>
                        <option value="Products">Products</option>
                        <option value="Customers">Customers</option>
                        <option value="Vendor">Vendors</option>
                        <option value="Employees">Employees</option>

                    </select>
                </div>
            </div>
            <div id="dvHeader" runat="server">
                <div class="col-md-2">
                    <label>From Date</label>
                    <div>
                        <dxe:ASPxDateEdit runat="server" ID="dtFrom" Width="100%" ClientInstanceName="cdtFrom"></dxe:ASPxDateEdit>
                    </div>
                </div>

                <div class="col-md-2">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit runat="server" ID="dtTo" Width="100%" ClientInstanceName="cdtTo"></dxe:ASPxDateEdit>
                    </div>
                </div>
            </div>
            <div class="col-md-2">
                <label>Verification Status</label>
                <div>
                    <dxe:ASPxComboBox  runat="server" SelectedIndex="0" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" ID="ddlStatus" Width="100%" ClientInstanceName="cddlStatus" DataSourceID="dsStatus" ValueField="STATUS_ID" TextField="STATUS_NAME">
                        <ClientSideEvents SelectedIndexChanged="getconfirmtype" />
                    </dxe:ASPxComboBox>
                </div>
            </div>

            <div class="col-md-4 pdTop15">
                <label>&nbsp;</label>
                <button class="btn btn-primary mBot10" onclick="ShowClick();" type="button">Show</button>
            </div>
            <br />
        </div>
        <div class="clearfix row">
            <div class="col-md-12">
                <dxe:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding" KeyFieldName="EuniqueId" Width="100%"  CssClass="pull-left" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false"
                    SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                     
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                  
                   
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true"  Caption="Select"/>
                        <dxe:GridViewDataColumn FieldName="EuniqueId" Visible="false" />
                        <dxe:GridViewDataColumn FieldName="MODULE_TYPE" Visible="false" />

                        <dxe:GridViewDataColumn FieldName="hdn_code" Visible="false" />
                        <dxe:GridViewDataColumn FieldName="Code" Caption="Code">
                          <Settings AutoFilterCondition="Contains" />   
                            
                         </dxe:GridViewDataColumn>
                        
                        <dxe:GridViewDataColumn FieldName="Description">
                         <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>   

                        <dxe:GridViewDataColumn FieldName="EnteredBy" Caption="Created By" >
                        <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>   

                        <dxe:GridViewDataColumn FieldName="EnteredOn" Caption="Created On">
                        <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>   
                        <dxe:GridViewDataColumn FieldName="ModifiedBy" Caption="Modified By">
                        <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>   
                        <dxe:GridViewDataColumn FieldName="ModifiedOn" Caption="Modified On">
                        <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn> 
                        
                        
                                <dxe:GridViewDataTextColumn ReadOnly="True" CellStyle-HorizontalAlign="Center" Width="100px">
                            <HeaderStyle HorizontalAlign="Center" />

                            <CellStyle HorizontalAlign="Center"></CellStyle>
                            <HeaderTemplate>
                                Actions
                            </HeaderTemplate>
                            <DataItemTemplate>
                                <% if (rights.CanView)
                                   { %>
                                <a href="javascript:void(0)" onclick="OpenDetails('<%#Eval("EuniqueId") %>','<%#Eval("MODULE_TYPE") %>','<%#Eval("hdn_code") %>')" class="pad">
                                <img src="/assests/images/doc.png" />
                                </a>
                               <%} %>
                            </DataItemTemplate>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <ClientSideEvents EndCallback="grid_endcallback" />
                </dxe:ASPxGridView>
            </div>
        </div>
        <div class="clearfix row">
            <div class="col-md-12 text-center">
                <table class="padCenter">
                    <tr>
                        <td>
                            <label>Update verify status as</label>

                        </td>
                        <td>
                            <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">
                                        <dxe:ASPxComboBox runat="server" ID="ddlConfirm" Width="100%" ClientInstanceName="cddlConfirm" ValueType="System.String">
                                            
                                        </dxe:ASPxComboBox>
                                       

                                    </dxe:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents />
                            </dxe:ASPxCallbackPanel>




                        </td>
                        <td>
                             <% if (rights.CanAdd)
                           { %>
                            <button type="button" id="btnConfirm" onclick="ConfirmClick();" class="btn btn-success btn-xs">Save</button>
                             <%} %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>


        <asp:SqlDataSource ID="dsStatus" runat="server" SelectCommand="select STATUS_ID,STATUS_NAME from MASTER_APPROVAL_STATUS"></asp:SqlDataSource>

        
    </div>

    <dxe:ASPxPopupControl ID="AspxDirectCustomerViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectCustomerViewPopup" Height="650px"
        Width="1020px" HeaderText="Customer View" Modal="true" AllowResize="false">
         
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

</asp:Content>
