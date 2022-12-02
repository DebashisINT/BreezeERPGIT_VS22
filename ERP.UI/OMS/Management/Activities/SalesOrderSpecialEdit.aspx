<%@ Page Title=" Special Edit - Sale Order" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    AutoEventWireup="true" CodeBehind="SalesOrderSpecialEdit.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesOrderSpecialEdit" EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/Saleorderspecialedit.js"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-title clearfix" id="myDiv">
      
         <h3 class="pull-left">
             Special Edit - Sales Order
         </h3>
        <br />
    </div>

    <div>

        Sales Order Number

            <input type="text" id="inp_saleorder" runat="server" style="width:30%" onchange="validateSaleorderNumber()"  />
      
          <br />
           <div id="dvusersd" style="display:none;">
        Change 'Entered by' To
     
       <dxe:ASPxCallbackPanel runat="server" ID="cpuserid" ClientInstanceName="cuseridpanel" OnCallback="ComponentUserLogin_Callback">
                <PanelCollection>

                    <dxe:PanelContent runat="server">
                       
                         <dxe:ASPxGridLookup ID="griduserloginIdLookup" SelectionMode="Single" runat="server" ClientInstanceName="griduserloginLookup"
                            OnDataBinding="lookup_LoginID_DataBinding" TextFormatString="{1}"
                            KeyFieldName="user_id" AutoGenerateColumns="False">

                            <Columns>

                                <dxe:GridViewDataColumn FieldName="ContactName" Visible="true" VisibleIndex="1" Width="150" Caption="Contact Name" Settings-AutoFilterCondition="Contains">
                                </dxe:GridViewDataColumn>

                                <dxe:GridViewDataColumn FieldName="Username" Visible="true" VisibleIndex="2" Width="150" Caption="User Name" Settings-AutoFilterCondition="Contains">
                                </dxe:GridViewDataColumn>

                            </Columns>

                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">

                                 <Templates>
                                  
                                     <StatusBar>

                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <div class="hide">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>

                                    </StatusBar>

                                </Templates>

                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                <SettingsPager Mode="ShowPager">
                                </SettingsPager>

                                <SettingsPager PageSize="20">
                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                </SettingsPager>

                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                            </GridViewProperties>

                        </dxe:ASPxGridLookup>

                    </dxe:PanelContent>

                </PanelCollection>

            </dxe:ASPxCallbackPanel>

        </div>


            <div class="clear"></div>

    <br />

            <div class="col-md-6">
                <asp:Button ID="btnmanualReceipt" runat="server" Text="Update" CssClass="btn btn-primary" OnClientClick="UpdateSaleorder(); return false;" UseSubmitBehavior="False" />
            </div>

    <input type="hidden" id="id_invoice" />


</asp:Content>
