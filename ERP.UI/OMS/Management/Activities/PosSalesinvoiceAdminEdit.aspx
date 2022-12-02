<%@ Page Language="C#" Title="Special Edit" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="PosSalesinvoiceAdminEdit.aspx.cs" Inherits="ERP.OMS.Management.Activities.PosSalesinvoiceAdminEdit" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/ucPaymentDetails.ascx" TagPrefix="uc1" TagName="ucPaymentDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/PosSalesinvoiceAdminEdit.js?v=1.0"></script>
    <link href="CSS/PosSalesinvoiceAdminEdit.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="UserControls/Js/ucPaymentDetails.js"></script>
    <script src="JS/SearchPopup.js?var=1.0"></script>
    <div class="panel-heading">
        <div class="panel-title clearfix" id="myDiv" runat="server">
            <h3 class="clearfix pull-left">
                <asp:Label ID="lblHeadTitle" runat="server" Text="Special Edit "></asp:Label>
            </h3>
    </div>
    <div class="form_main">
        <div class="clearfix" style="background: #f9f9f9;border: 1px solid #dcdcdc;padding: 30px 20px;border-radius: 3px">
            <div class="row">
                <div class="col-md-6" style="border-right:1px solid #ccc">
                    <div class="mBot10"><h5>Sales Invoice</h5></div>
                    <input type="text" placeholder="Search By Invoice Number and Press Tab Key" id="SearchInv" style="width:80%" onblur="validateInvoiceNumber()"/>
                </div>
                <div class="col-md-6 text-center">
                    <div class="mBot10"><h5>Customer Receipt/Refund</h5></div>
                
                    <input type="button" class="btn btn-success" value="Update Receipt/Refund Number" onclick="ShowManualReceiptPopup()"/>
                </div>
            </div>
            <div class="space-row"></div>
        </div>
        <div class="clearfix" id="divSelectField" style="margin-top:15px;background: #f9f9f9;border: 1px solid #dcdcdc;padding: 15px 20px;border-radius: 3px;display:none;" runat="server">
            <div class="clearfix">

                 <div class="col-md-3">
                    <label><b>Select Field To Update</b></label>
                       <asp:DropDownList ID="UpdateField" runat="server" >
                           <asp:ListItem Text="-Select-" Value="Select"/>
                           <asp:ListItem Text="Document Number" Value="docNo"/>
                           <asp:ListItem Text="Payment Details" Value="PaymnetDetails"/>
                           <asp:ListItem Text="Billing & Shipping" Value="BillingShipping"/>
                            <asp:ListItem Text="Financer Details" Value="FinanceBlock"/>              
                           <%-- Added By Rajdip --%>
                           <asp:ListItem Text="Salesman, Reference, Delivery Date, Vehicles, Remarks" Value="Salesman"/>
                    <%--       <asp:ListItem Text="Reference" Value="Reference"/>
                           <asp:ListItem Text="Delivery Date" Value="Deliverydate"/>
                           <asp:ListItem Text="Vehicles" Value="Vehicles"/>
                           <asp:ListItem Text="Remarks" Value="Remarks"/>--%>
                           <asp:ListItem Text="Posting Date" Value="PostingDateofDeliver"/>
                         <%--  <asp:ListItem Text="Type-Cash To Credit Only" Value="Typecashtocredit"/>--%>
                           <asp:ListItem Text="Branch" Value="Branch"/>
                           <asp:ListItem Text="Customer" Value="Customer"/>
                           <%-- Edit --%>
                           <asp:ListItem Text="Re-Post" Value="repost"/>
                           <asp:ListItem Text="Change Global Tax" Value="changeglobaltax"/>
                           <asp:ListItem Text="No Commission" Value="NoCommission"/>
                       </asp:DropDownList>
                </div>
                <%-- Added by Rajdip --%>
                <div class="clear" >
                </div>
                <div class="clearfix" id="divbranch">
                     <div class="col-md-2">
                                        <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" 
                                            DataTextField="BANKBRANCH_NAME" onchange="onBranchItems()" DataValueField="BANKBRANCH_ID">
                                        </asp:DropDownList>
                                    </div>
                </div>
                  <div class="clearfix" id="divsalesman">
                        <div class="col-md-2">
                            <label>Salesman</label>
                           <dxe:ASPxComboBox ID="ddl_SalesAgent" ClientInstanceName="cddl_SalesAgent" runat="server" ValueType="System.String" Width="92%" EnableSynchronization="True"
                                                                         EnableIncrementalFiltering="True"><%--OnCallback="ddl_SalesAgent_Callback"--%>
                                                                        <ClientSideEvents GotFocus="function(s,e){cddl_SalesAgent.ShowDropDown();}" EndCallback=" OnSalesAgentEndCallback" />
                                                                    </dxe:ASPxComboBox>
                            <%-- <dxe:ASPxComboBox ID="ddl_SalesAgent"  runat="server">--%>
                    
                            <%-- </dxe:ASPxComboBox>--%>
                        </div>
                        <div class="col-md-2">
                            <label>Reference</label>
                             <dxe:ASPxTextBox ID="txt_Refference" ClientInstanceName="ctxt_Refference" runat="server" Width="100%">
                                                                    </dxe:ASPxTextBox>
                        </div>
                        <div class="col-md-2">
                            <label>Delivery Date</label>
                           <dxe:ASPxDateEdit ID="deliveryDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdeliveryDate" Width="92%"></dxe:ASPxDateEdit><%--EditFormatString="dd-MM-yyyy"--%>
                        </div>
                       <div class="col-md-2">
                            <label>Vehicles</label>
                       <dxe:ASPxTextBox ID="txtVehicles" ClientInstanceName="ctxtVehicles" runat="server" MaxLength="1000" Width="100%" aut></dxe:ASPxTextBox>
                        </div>
                           <div class="col-md-2">
                            <label>Remarks</label>
                              <dxe:ASPxTextBox ID="txtRemarks" MaxLength="300" ClientInstanceName="ctxtRemarks" runat="server" Width="100%">
                                                                    </dxe:ASPxTextBox>
                        </div>
                     </div>

            </div>
            <div class="clearfix">
                <div class="col-md-3" id="divpostingdate">
                    <label>Posting Date</label>
                      <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server"  EditFormat="Custom" EditFormatString="dd-MM-yyyy"  ClientInstanceName="tstartdate" Width="100%"></dxe:ASPxDateEdit>
                    <%--<asp:RegularExpressionValidator runat="server" ControlToValidate="dt_PLQuote" ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$"
            ErrorMessage="Invalid date format." ValidationGroup="Group1" />--%>
                </div>
         
            </div>

            <div class="clearfix">
                <div class="col-md-3" id="divNoCommission">
                    <label>No Commission</label>
                     <dxe:ASPxCheckBox ID="chkNocommission"  ClientInstanceName="cchkNocommission" runat="server" Width="100%">
                                                            </dxe:ASPxCheckBox>
                </div>
         
            </div>















                    <%--//Rev Rajdip Pop Up Area--%>
           <div class="clearfix" id="changeglobaltaxdiv">
        <%-- <div class="PopUpArea">--%>
                <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
                <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
                <%--ChargesTax--%>
     <%--           <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
                    Width="900px" Height="300px" HeaderText="Other Charges" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>







                        <dxe:PopupControlContentControl runat="server">--%>
                            <div class="Top clearfix">
                                <div id="content-5" class="col-md-12  wrapHolder content horizontal-images" style="width: 100%; margin-right: 0;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Gross Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel6" runat="server" ClientInstanceName="clblChargesTaxableGross"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductAmount"  Text="0.00"  runat="server"  ClientInstanceName="ctxtProductAmount">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="lblChargesGSTforGross">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>GST</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblChargesGSTforGross"  Text="0.00"  runat="server"  ClientInstanceName="clblChargesGSTforGross">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Total Discount</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductDiscount"  Text="0.00"  runat="server"  ClientInstanceName="ctxtProductDiscount">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Total Charges</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductTaxAmount" runat="server" Text="0.00"   ClientInstanceName="ctxtProductTaxAmount"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Net Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server"   ClientInstanceName="clblChargesTaxableNet"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductNetAmount" runat="server" Text="0.00"  ClientInstanceName="ctxtProductNetAmount"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                <%--        <li class="lblChargesGSTforNet">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>GST</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblChargesGSTforNet" Text="0.00"  runat="server"  ClientInstanceName="clblChargesGSTforNet">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>--%>
                                    </ul>
                                </div>
                                <div class="clear">
                                </div>
                                <%--Error Msg--%>

                                <div class="col-md-8 hide" id="ErrorMsgCharges">
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Status
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Tax Code/Charges Not Defined.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>

                                </div>

                                <div class="clear">
                                </div>
                                <div class="col-md-12 gridTaxClass" style="">
                                    <dxe:ASPxGridView runat="server" KeyFieldName="TaxID" ClientInstanceName="gridTax" ID="gridTax"
                                        Width="100%" SettingsBehavior-AllowSort="false" OnCustomCallback="gridTax_CustomCallback"  SettingsBehavior-AllowDragDrop="false"
                                        Settings-ShowFooter="false"  OnBatchUpdate="gridTax_BatchUpdate" OnRowInserting="gridTax_RowInserting" OnRowUpdating="gridTax_RowUpdating" OnRowDeleting="gridTax_RowDeleting"
                                        OnDataBinding="gridTax_DataBinding" ClientSideEvents-EndCallback="gridTax_EndCallback"><%--  OnBatchUpdate="gridTax_BatchUpdate"
                                        OnRowInserting="gridTax_RowInserting" OnRowUpdating="gridTax_RowUpdating" OnRowDeleting="gridTax_RowDeleting"
                                        OnDataBinding="gridTax_DataBinding">--%>
                                        <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn FieldName="TaxName" Caption="Tax" VisibleIndex="0" Width="40%" ReadOnly="true">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="calCulatedOn" Caption="Calculated On" VisibleIndex="0" Width="20%" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">  <%--DisplayFormatString="0.00"--%>
                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="PercentageTextChange" />
                                                    <ClientSideEvents />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="QuotationTaxAmountTextChange" GotFocus="QuotationTaxAmountGotFocus" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents EndCallback="OnTaxEndCallback" />
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                        </SettingsEditing>
                                    </dxe:ASPxGridView>
                                </div>
                                <div class="col-md-12">
                                    <table style="" class="chargesDDownTaxClass">
                                        <tr class="chargeGstCstvatClass">
                                            <td style="padding-top: 10px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                            <td style="padding-top: 10px; width: 200px;">
                                                <dxe:ASPxComboBox ID="cmbGstCstVatcharge" ClientInstanceName="ccmbGstCstVatcharge" runat="server" SelectedIndex="0"
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}" OnCallback="cmbGstCstVatcharge_Callback">
                                                   <%-- OnCallback="cmbGstCstVatcharge_Callback">--%>
                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                        <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />

                                                    </Columns>
                                                 <%--   <ClientSideEvents SelectedIndexChanged="ChargecmbGstCstVatChange"
                                                        GotFocus="chargeCmbtaxClick" />--%>

                                                </dxe:ASPxComboBox>



                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px; width: 200px;">
                                                <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" ClientInstanceName="ctxtGstCstVatCharge" ReadOnly="true" Text="0.00"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </dxe:ASPxTextBox>

                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px">
                                                <input type="button" onclick="recalculateTaxCharge()" class="btn btn-info btn-small RecalculateCharge" value="Recalculate GST" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clear">
                                    <br />
                                </div>



                                <div class="col-sm-3">
                            
                                </div>

                                <div class="col-sm-9">
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-right: 30px"><strong>Total Charges</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt"
                                                         Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>

                                            </td>
                                            <td style="padding-right: 30px; padding-left: 5px"><strong>Total Amount</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" 
                                                        Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <span></span>
                                </div>
                                <div class="col-sm-4">
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <span></span>
                                </div>
                                <div class="col-sm-4">
                                </div>
                            </div>
                        <%--</dxe:PopupControlContentControl>--%>

               <asp:HiddenField runat="server" ID="HdPosType" />






























            <%--            
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>--%>
                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
               <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                            <ul>
                                                <li class="clsbnrLblTotalQty">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="0.00" ClientInstanceName="cbnrLblTotalQty" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblTaxableAmt">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxableAmt" runat="server" Text="Taxable Amt" ClientInstanceName="cbnrLblTaxableAmt" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxableAmtval" runat="server" Text="0.00" ClientInstanceName="cbnrLblTaxableAmtval" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblTaxAmt">
                                                    <div class="horizontallblHolder" id="">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxAmt" runat="server" Text="Tax Amt" ClientInstanceName="cbnrLblTaxAmt" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTaxAmtval" runat="server" Text="0.00" ClientInstanceName="cbnrLblTaxAmtval" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblAmtWithTax" runat="server" id="oldUnitBanerLbl">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblAmtWithTax" runat="server" Text="Amount With Tax" ClientInstanceName="cbnrLblAmtWithTax" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrlblAmountWithTaxValue" runat="server" Text="0.00" ClientInstanceName="cbnrlblAmountWithTaxValue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblLessOldVal">
                                                    <div class="horizontallblHolder" id="">
                                                      <%--  <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessOldVal" runat="server" Text="Less Old Unit Value" ClientInstanceName="cbnrLblLessOldVal" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessOldMainVal" runat="server" Text=" 0.00" ClientInstanceName="cbnrLblLessOldMainVal"></dxe:ASPxLabel>
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>--%>
                                                    </div>
                                                </li>
                                                  <li class="clsbnrLblotherchrages">
                                                    <div class="horizontallblHolder" id="">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="ASPxLabel0" runat="server" Text="Other Charges" ClientInstanceName="cbnrlblothercharges" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="ASPxLabel01" runat="server" Text=" 0.00" ClientInstanceName="cbnrLblotherchargesVal"></dxe:ASPxLabel>
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblLessAdvance" id="idclsbnrLblLessAdvance" runat="server">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessAdvance" runat="server" Text="Advance Adjusted" ClientInstanceName="cbnrLblLessAdvance" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblLessAdvanceValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblLessAdvanceValue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>

                                                <li class="clsbnrLblInvVal" id="otherChargesId">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="cbnrOtherCharges" runat="server" Text="Other Charges" ClientInstanceName="cbnrOtherCharges" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrOtherChargesvalue" runat="server" Text="0.00" ClientInstanceName="cbnrOtherChargesvalue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>



                                                <li class="clsbnrLblInvVal">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblInvVal" runat="server" Text="Invoice Value" ClientInstanceName="cbnrLblInvVal" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblInvValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblInvValue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>




                                                <li class="clsbnrLblInvVal">
                                                    <div class="horizontallblHolder" style="border-color: #f14327;">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td style="background: #f14327;">
                                                                        <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Running Balance" ClientInstanceName="cbnrLblInvVal" />
                                                                    </td>
                                                                    <td>
                                                                        <strong>
                                                                           <dxe:ASPxLabel ID="lblRunningBalancCapsul" runat="server"  ClientInstanceName="clblRunningBalanceCapsul" />
                                                                        </strong>
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                              <%--  <dxe:ASPxLabel ID="lblRunningBalancCapsul" runat="server" Text="0.00" ClientInstanceName="clblRunningBalanceCapsul" />--%>
                                                <li class="clsbnrLblInvVal">
                                                    <div runat="server" id="divSendSMS">

                                                        <strong>

                                                            <input type="checkbox" name="chksendSMS" id="chksendSMS"  />&nbsp;Send SMS<%--onclick="SendSMSChk()"--%>
                                                             <asp:HiddenField ID="hdnSendSMS" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnCustMobile" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnsendsmsSettings" runat="server" />
                                                            <asp:HiddenField ID="hdnposforgst" runat="server" />
                                                        </strong>

                                                    </div>
                                                </li>

                                            </ul>

                                        </div>
                   <div class="col-md-3 hide">
                                                <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                                    <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" />
                                                </dxe:ASPxComboBox>
                                            </div>

            <%--</div>--%>
        <%-- End Rev Rajdip --%>


               </div>
























            <div class="clearfix">
                <div class="col-md-3" id="divcashtocredit">
                    <label>Type</label>
                   <dxe:ASPxComboBox ID="ASPxComboBox1"  runat="server"></dxe:ASPxComboBox>
                </div>
         
            </div>
            <div class="clear"></div> 
            <div class="col-md-3" id="divcustomer">
        <%--<label>Customer</label>--%>
            <%--  <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName">
                                                                <Buttons>
                                                                <dxe:EditButton>
                                                                </dxe:EditButton>                
                                                            </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>--%>
        <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                                        </dxe:ASPxLabel>
                                                        <%--<% if (rights.CanAdd && hdAddOrEdit.Value != "Edit")
                                                            { %>--%>
                                                        <a href="#" onclick="AddcustomerClick()" style="position: absolute; top: 4px; margin-left: 5px;"><i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i></a>
                                                        <%-- <% } %>--%>

                                                        <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName">

                                                            <Buttons>
                                                                <dxe:EditButton>
                                                                </dxe:EditButton>

                                                            </Buttons>
                                                            <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                                                        </dxe:ASPxButtonEdit>






                                                        <span id="MandatorysCustomer" style="display: none" class="errorField">
                                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>


    </div>
 
            <div class="clear" ></div>

            <div class="col-md-3" id="divInvoiceNumber">
        <label>Enter New Invoice Number</label>
            <asp:TextBox ID="txtInvoiceNumber" runat="server" MaxLength="16"></asp:TextBox>
    </div>

            <div class="clear"></div>
       
            <div class="col-md-12" id="divPaymentDetails">

                <uc1:ucPaymentDetails runat="server" ID="PaymentDetails" />
            </div>
            <div class="clearfix" id="divBillingShipping">

                <div class="col-md-6">
                <label>Billing Details</label>
                    <asp:TextBox ID="txtBillingAddress1" runat="server" placeholder="Address1"></asp:TextBox>
                    <asp:TextBox ID="txtBillingAddress2" runat="server" placeholder="Address2"></asp:TextBox>
                    <asp:TextBox ID="txtBillingAddress3" runat="server" placeholder="Address3"></asp:TextBox>
                    <asp:TextBox ID="txtBillingLandMark" runat="server" placeholder="Land Mark"></asp:TextBox>
                 
                    <asp:TextBox ID="txtBillingPin" runat="server" MaxLength="6" placeholder="Pin (Country, State, City to be automatically updated)"></asp:TextBox>
                    <label style="color: blue;font-size: 12px;">Based on PIN Country, State, City to be automatically updated</label>
                    <br />
                    <a href="#" onclick="CopyToshipping()">Copy To Shipping</a>

                </div>

                <div class="col-md-6">
                <label>Shipping Details</label>
                    <asp:TextBox ID="txtShippingAddress1" runat="server" placeholder="Address1"></asp:TextBox>
                    <asp:TextBox ID="txtShippingAddress2" runat="server" placeholder="Address2"></asp:TextBox>
                    <asp:TextBox ID="txtShippingAddress3" runat="server" placeholder="Address3"></asp:TextBox>
                    <asp:TextBox ID="txtShippingLandmark" runat="server" placeholder="Land mark"></asp:TextBox>
                    <asp:TextBox ID="txtShippingPin" runat="server" MaxLength="6" placeholder="Pin (Country, State, City to be automatically updated)"></asp:TextBox>
                    <label style="color: blue;font-size: 12px;">Based on PIN Country, State, City to be automatically updated</label>
                    <br />
                    <a href="#" onclick="CopyToBilling()">Copy To Billing</a>
                </div>

            </div>
            <div class="clearfix" id="divFinanceBlock">
            <div class="col-md-2">
                <label>Downpayment</label>
                <asp:TextBox ID="txtDownPayment" runat="server" placeholder="Down Payment" MaxLength="10"></asp:TextBox>   
            </div>
            <div class="col-md-2">
                <label>Proc. Fee</label>
                <asp:TextBox ID="txtProcFee" runat="server" placeholder="Proc. Fee" MaxLength="10"></asp:TextBox>   
            </div>
            <div class="col-md-2">
            <label>EMI Card/Other Charges</label>
            <asp:TextBox ID="txtEmiCard" runat="server" placeholder="EMI Card/Other Charges" MaxLength="10"></asp:TextBox>   
        </div>
        </div>



            <div class="col-md-12 mTop5" id="divUpdateButton" runat="server" style="display:none">
                <asp:Button ID="btn_Update" runat="server" Text="Update" OnClick="btn_Update_Click"  CssClass="btn btn-primary" OnClientClick="UpdateClientClick()"  UseSubmitBehavior="False"/>
            </div>
        </div>
        

        

    </div>

    <dxe:ASPxPopupControl ID="ManualReceipt" runat="server" ClientInstanceName="cManualReceipt"
            Width="500px" HeaderText="Update ManualReceipt" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" OnWindowCallback="ManualReceipt_WindowCallback">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                   <div class="row">
                       <div class="col-md-6">
                           <asp:TextBox ID="txtOldReceiptNumber" runat="server" ></asp:TextBox>
                    
                    <asp:Label ID="lblWrongReceipt" runat="server" Text="Invalid Receipt Number" Visible="false"></asp:Label>
                       </div>
                        
                        <div class="col-md-2">
                            <input type="button" onclick="SearchManualReceipt()" value="Search" class="btn btn-primary" />
                        </div>
                       <div class="clear"></div>
                       <div class="col-md-6">
                                 <asp:TextBox ID="txtNewReceiptNumber" runat="server" MaxLength="16"></asp:TextBox>
                            <asp:Button ID="btnmanualReceipt" runat="server" Text="Update"  CssClass="btn btn-primary" OnClientClick="UpdateManualReceipt(); return false;"  UseSubmitBehavior="False"/>
                           </div>
                       </div>

                    <asp:HiddenField ID="hdRecPayType" runat="server"></asp:HiddenField>
                    </dxe:PopupControlContentControl> 
            </ContentCollection>
           
        </dxe:ASPxPopupControl>
       <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name,Unique Id and Phone No." />

                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

        <%-- Rev Rajdip --%>
              <asp:HiddenField ID="hdlblShippingState" runat="server" />
              <asp:HiddenField ID="lblShippingStateText" runat="server"></asp:HiddenField>
              <asp:HiddenField ID="lblShippingStateValue" runat="server"></asp:HiddenField>

          <asp:HiddenField ID="hdlblBillingState" runat="server" />
          <asp:HiddenField ID="lblBillingStateText" runat="server"></asp:HiddenField>
          <asp:HiddenField ID="lblBillingStateValue" runat="server"></asp:HiddenField>









      <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField ID="hdnvehicleid" runat="server" />
            <asp:HiddenField ID="hdnAddressDtl" runat="server" />
    <asp:HiddenField ID="hddnsalesmanId" runat="server" />
</asp:Content>