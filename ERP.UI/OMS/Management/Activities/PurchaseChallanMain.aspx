<%@ Page Title="Purchase Challan" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PurchaseChallanMain.aspx.cs" Inherits="ERP.OMS.Management.Activities.PurchaseChallanMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Add Purchase GRN"></asp:Label>
            </h3>
        </div>
    </div>
    <div class=" form_main row">
        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
            <TabPages>
                <dxe:TabPage Name="General" Text="General">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <dxe:ASPxCallbackPanel runat="server" ID="cbpGeneralCallbackPanel" ClientInstanceName="cbpGeneralCallbackPanel">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">
                                        <div class="row">
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_Inventory" runat="server" Text="Inventory Item?">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddlInventory" runat="server" ClientInstanceName="ddlInventory">
                                                    <Items>
                                                        <dxe:ListEditItem Text="Both" Value="B" />
                                                        <dxe:ListEditItem Text="Inventory Item" Value="Y" />
                                                        <dxe:ListEditItem Text="Capital Goods" Value="C" />
                                                    </Items>
                                                    <ClientSideEvents SelectedIndexChanged="ddlInventory_OnChange" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-2 lblmTop8" runat="server" id="divNumberingScheme">
                                                <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddl_numberingScheme" runat="server" ClientInstanceName="ddl_numberingScheme" DataTextField="SchemaName" DataValueField="ID">
                                                    <ClientSideEvents SelectedIndexChanged="CmbScheme_ValueChange" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Purchase Number">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                                <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()" Enabled="false">
                                                </asp:TextBox>
                                                <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                                <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLQuoteDate" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <ClientSideEvents DateChanged="function(s, e) { GetIndentREquiNo(e)}" />
                                                </dxe:ASPxDateEdit>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                                <dxe:ASPxComboBox ID="ddl_Branch" runat="server" ClientInstanceName="cddl_Branch" DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID">
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="VendorComboBox" ClientInstanceName="cVendorComboBox" runat="server" EnableCallbackMode="true" Width="92%"
                                                    CallbackPageSize="15" ValueType="System.String" ValueField="cnt_internalid" TextFormatString="{1} [{0}]" DropDownStyle="DropDown"
                                                    OnItemsRequestedByFilterCondition="VendorComboBox_ItemsRequestedByFilterCondition" FilterMinLength="4"
                                                    OnItemRequestedByValue="VendorComboBox_ItemRequestedByValue">
                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="Name" Caption="Name" Width="200px" />
                                                        <dxe:ListBoxColumn FieldName="shortname" Caption="Short Name" Width="200px" />
                                                    </Columns>
                                                    <ClientSideEvents ValueChanged="function(s, e) {GetContactPerson(e)}" />
                                                </dxe:ASPxComboBox>
                                                <span id="MandatorysCustomer" class="customerno pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-2 lblmTop8">

                                                <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" Width="100%" ClientInstanceName="cContactPerson"
                                                    Font-Size="12px">
                                                    <ClientSideEvents EndCallback="cmbContactPersonEndCall" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-2 lblmTop8" style="display: none">
                                                <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Party Invoice No.">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txtPartyInvoice" ClientInstanceName="ctxtPartyInvoice" runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                                <span id="MandatorysPartyinvno" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Party Invoice Date">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxDateEdit ID="dt_PartyDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLPartyDate" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select Purchase Order">
                                                </dxe:ASPxLabel>
                                                <%--<dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" ClientInstanceName="gridquotationLookup"
                                                                OnDataBinding="lookup_quotation_DataBinding"
                                                                KeyFieldName="PurchaseOrder_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="PurchaseOrder_Number" Visible="true" VisibleIndex="1" Caption="Purchase Order" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Purchase Date" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Vendor Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="ReferenceName" Visible="true" VisibleIndex="4" Caption="Reference" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="BranchName" Visible="true" VisibleIndex="5" Caption="Branch" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager Mode="ShowAllRecords">
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                                <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" />
                                                            </dxe:ASPxGridLookup>

                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="componentEndCallBack" />
                                                </dxe:ASPxCallbackPanel>--%>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Purchase Date">
                                                </dxe:ASPxLabel>
                                                <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                                    <asp:Label ID="lbl_MultipleDate" runat="server" Text="Multiple Select Quotation Dates" Style="display: none"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddl_Currency" ClientInstanceName="cddl_Currency" runat="server" Width="100%" DataValueField="Currency_ID" DataTextField="Currency_AlphaCode">
                                                    <ClientSideEvents SelectedIndexChanged="ddl_Currency_Rate_Change" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate">
                                                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" SelectedIndex="0" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                                    <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-2 lblmTop8  hide" style="margin-bottom: 5px">
                                                <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" Width="100%">
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>
                                    </dxe:PanelContent>
                                </PanelCollection>
                            </dxe:ASPxCallbackPanel>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="[B]illing/Shipping" Text="Our Billing/Shipping">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
            </TabPages>
        </dxe:ASPxPageControl>
    </div>
    <asp:SqlDataSource ID="VendorDataSource" runat="server"  />
</asp:Content>
