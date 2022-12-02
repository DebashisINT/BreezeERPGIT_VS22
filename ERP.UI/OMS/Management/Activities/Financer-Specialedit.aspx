<%@ Page Title=" Special Edit - Financer" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    AutoEventWireup="true" CodeBehind="Financer-Specialedit.aspx.cs" Inherits="ERP.OMS.Management.Activities.Financer_Specialedit" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="JS/Financer-SpecialEdit.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">Special Edit - Financer
        </h3>
        <br />
        <br />
        <div class="col-md-6">
            <label>Sales Invoice</label>
            <input type="text" placeholder="Search By Invoice Number and Press Tab Key" id="SearchInv" style="width: 80%" onchange="validateInvoiceNumber()" />
        </div>
      <%--  <div class="col-md-6">
            <label>Fetch Details</label>
            <br />
            <input type="button" class="btn btn-primary" value="Fetch Details" onclick="ShowManualReceiptPopup()" />
        </div>--%>
    </div>

    <div id="dvfinancer" style="display:none;">


        <div class="row">
            <div class="col-md-6">
                <asp:TextBox ID="txtoldInvoicenumber" runat="server" style="width: 80%" ></asp:TextBox>

                <asp:Label ID="lblWrongReceipt" runat="server" Text="Invalid Receipt Number" Visible="false"></asp:Label>
            </div>

            <div class="col-md-2">
                <%--        <input type="button" onclick="SearchManualReceipt()" value="Search" class="btn btn-primary" />--%>

                <dxe:ASPxCallbackPanel runat="server" ID="cpFinancer" ClientInstanceName="cProductfinancerPanel" OnCallback="Componentfinancer_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="gridfinancerLookup" SelectionMode="Single" runat="server" ClientInstanceName="gridfinancerLookup"
                                OnDataBinding="lookup_financer_DataBinding"
                                KeyFieldName="FinId" AutoGenerateColumns="False" >
                              
                                 <Columns>
                                    
                                    <dxe:GridViewDataColumn FieldName="FinName" Visible="true" VisibleIndex="1" Width="150" Caption="Financer Name" Settings-AutoFilterCondition="Contains">
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



            <div class="col-md-6">
            <%--    <asp:TextBox ID="txtNewReceiptNumber" runat="server" MaxLength="16"></asp:TextBox>--%>
                <asp:Button ID="btnmanualReceipt" runat="server" Text="Update" CssClass="btn btn-primary" OnClientClick="UpdateManualReceipt(); return false;" UseSubmitBehavior="False" />
            </div>
        </div>

    </div>

    <input type="hidden" id="id_invoice"/>
</asp:Content>
