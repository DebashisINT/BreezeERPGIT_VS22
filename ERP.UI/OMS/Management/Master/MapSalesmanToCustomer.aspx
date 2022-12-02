<%@ Page Title="Industry Map" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="MapSalesmanToCustomer.aspx.cs" Inherits="ERP.OMS.Management.Master.MapSalesmanToCustomer" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script>
        <%--        $(document).ready(function () {
            alert('sss');
            
                var x = document.getElementById("<%=lbAvailable.ClientID %>");
                for (var i = 0; i < x.options.length; i++) {
                    if (x.options[i].selected == true) {
                        alert(x.options[i].selected);
                    }
                }
           
        });--%>
    </script>



    <script type="text/javascript">
        $(document).ready(function () {
            cComponentPanel.PerformCallback();
        });
        function btncancel_clientclick()
        {
            cComponentPanel.PerformCallback();
            gridLookup.gridView.Refresh();
        }
        function LookupGotFocus() {
            gridLookup.ShowDropDown();
        }
            function componentEndCallBack(s, e) {
                debugger;
                gridLookup.gridView.Refresh();

            }
            function CloseGridLookup() {
                gridLookup.ConfirmCurrentSelection();
                gridLookup.HideDropDown();
                // gridLookup.Focus();
            }
            function selectAll() {
                // cComponentPanel.PerformCallback('selectAll~true');
                gridLookup.gridView.SelectRows();
            }
            var deselect=0
            function unselectAll() {
                //cComponentPanel.PerformCallback('selectAll~false');
                deselect=1
                $("#hdndeselectall").val(deselect);
                gridLookup.gridView.UnselectRows();
            }
            function btnclientSave_Click()
            {
                var CashFundRequisition_Msg = "Salesman Successfully mapped to Customer";
                jAlert(CashFundRequisition_Msg, 'Alert Dialog: [Salesman/Agents - Customer Map]', function (r) {
                    if (r == true) {
                        var url = 'frmContactMain.aspx?requesttype=agent';
                        window.location.href = url;
                    }
                });
            }
            function CallConfirmBox() {
                if (confirm("Salesman With Customer Successfully Mapped!,Redirect to Salesman/Agents List?")) {
                    var url = 'frmContactMain.aspx?requesttype=agent';
                    window.location.href = url;
                } else {
                    //var url = 'frmContactMain.aspx?requesttype=agent';
                    //window.location.href = url;
                }
            }
    </script>
    <style>
        #lbAvailable {
            min-width:352px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>
                <asp:Label ID="lblEntityType" runat="server"></asp:Label>
                - Customer Map
                <asp:Label ID="lblEntityUserName" runat="server"></asp:Label>
                <asp:HiddenField ID="hdnid" runat="server" />
                <asp:HiddenField ID="hdndeselectall" runat="server" />

            </h3>

            <div class="crossBtn">
                <asp:LinkButton ID="goBackCrossBtn" runat="server" OnClick="goBackCrossBtn_Click"><i class="fa fa-times"></i></asp:LinkButton>
                <%--<a href="frmContactMain.aspx" id="goBackCrossBtn"><i class="fa fa-times"></i></a>--%>
                <asp:HiddenField ID="hidbackPagerequesttype" runat="server" />
            </div>

        </div>
    </div>
    <div class="form_main" style="border: 1px solid #ccc; padding: 5px 15px">
       
        <div class=" ">
                             
               <dxe:ASPxCallbackPanel runat="server" id="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="Component_Callback">
                <panelcollection>
                        <dxe:PanelContent runat="server">
                            <div class="col-md-3" style="padding-top: 4px">
                                <div class="padBot5" style="display: block; height: auto;">
                                    Customer List
                                </div>
                                <dxe:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Multiple"  ClientInstanceName="gridLookup"
                                    KeyFieldName="cnt_id" Width="100%" TextFormatString="{2}" MultiTextSeparator=", "  OnDataBinding="grid_DataBinding">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " SelectAllCheckboxMode="Page">
                                                                            
                                        </dxe:GridViewCommandColumn> 

                                            <dxe:GridViewDataColumn FieldName="cnt_id" Caption="Id" Width="0">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="cnt_internalId" Caption="Customer Code" Width="150">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="cnt_firstName" Caption="First Name" Width="150">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                    

                                         <%--   <dxe:GridViewDataColumn FieldName="ProductClass_Name" Caption="Class" Width="150">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="sProducts_Code" Caption="Product Code" Width="150">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Product Name" Width="300">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>--%>

                                    </Columns>
                                    <GridViewProperties  Settings-VerticalScrollBarMode="Auto"   >
                                                                             
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true"/>
                                                                            
                                    </GridViewProperties>
                                    <ClientSideEvents GotFocus="LookupGotFocus" />
                                </dxe:ASPxGridLookup>
                              </div>
                            </dxe:PanelContent>
                    </panelcollection>
                <clientsideevents endcallback="componentEndCallBack" />
            </dxe:ASPxCallbackPanel>
                                        </div>

        <div style="clear: both;"></div>
        <br />
        <div class="clearfix">
            <dxe:ASPxButton ID="btnSaveTaxScheme" ClientInstanceName="cbtnSave_citys" runat="server" AutoPostBack="false" OnClick="btnSave_Click" 
                    Text="Save" CssClass="btn btn-primary">
                    <%--<clientsideevents click="function (s, e) {e.processOnServer= btnSave_citys();}" />ClientSideEvents-Click="btnclientSave_Click"--%>
               
                </dxe:ASPxButton>
              <dxe:ASPxButton ID="btncancel" ClientInstanceName="cbtncancel" runat="server" AutoPostBack="false" OnClick="btncancel_Click" ClientSideEvents-Click="btncancel_clientclick"
                    Text="Cancel" CssClass="btn btn-danger">
                    <%--<clientsideevents click="function (s, e) {e.processOnServer= btnSave_citys();}" />--%>

                </dxe:ASPxButton>
            </div>
    </div>
</asp:Content>
