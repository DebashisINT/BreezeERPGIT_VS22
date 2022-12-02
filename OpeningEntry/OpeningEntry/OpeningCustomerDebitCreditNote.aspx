<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="OpeningCustomerDebitCreditNote.aspx.cs" Inherits="OpeningEntry.OpeningEntry.OpeningCustomerDebitCreditNote" %>

<%@ Register Src="~/UserControls/OpeningSales_BillingShipping.ascx" TagPrefix="ucbs" TagName="Sales_BillingShipping" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/CustomerNote.js?v1.0.0.0102"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js"></script>

    <style>
        .pdbot30 {
            padding-bottom: 30px;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .lblmBot4 > span, .lblmBot4 > label {
            margin-bottom: 5px !important;
            display: inline-block;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <label id="TxtHeaded">Add Customer Debit/Credit Note</label>
        </h3>
    </div>
    <div id="ApprovalCross" runat="server" class="crossBtn"><a href="OpeningCustomerNoteList.aspx"><i class="fa fa-times"></i></a></div>


    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
        <TabPages>
            <dxe:TabPage Name="General" Text="General">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                            <div class="col-md-3">
                                <label>Note Type</label>
                                <div>
                                    <dxe:ASPxComboBox ID="ddlNoteType" runat="server" TabIndex="1" Width="100%" ClientInstanceName="cddlNoteType">
                                        <Items>
                                            <dxe:ListEditItem Text="Debit Note" Value="Dr" Selected="true" />
                                            <dxe:ListEditItem Text="Credit Note" Value="Cr" />
                                        </Items>
                                        <ClientSideEvents SelectedIndexChanged="Type_Changed" />
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div id="divNumberingScheme">
                                <div class="col-md-3" id="div_Edit">
                                    <label>Select Numbering Scheme</label>
                                    <div>
                                        <dxe:ASPxComboBox ID="CmbScheme" runat="server" TabIndex="2" ClientInstanceName="cCmbScheme"
                                            Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="CmbScheme_ValueChange" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 lblmTop8">
                                <label>Document No.</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" runat="server" Width="95%" meta:resourcekey="txtBillNoResource1" TabIndex="3" MaxLength="30" onchange="txtBillNo_TextChanged()"></dxe:ASPxTextBox>
                                    <span id="MandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    <span id="duplicateMandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Duplicate Journal No."></span>
                                </div>
                            </div>
                            <div class="col-md-2 lblmTop8">
                                <label style="">Posting Date</label>
                                <div>
                                    <dxe:ASPxDateEdit ID="dtTDate" runat="server" EditFormat="Custom" ClientInstanceName="cdtTDate" TabIndex="4" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                        UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                                        <%--  <ClientSideEvents DateChanged="function(s,e){DateChange()}" />--%>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                            <div class="col-md-2 lblmTop8">
                                <label>Branch</label>
                                <div>
                                    <dxe:ASPxComboBox ID="ddlBranch" runat="server" TabIndex="5" ClientInstanceName="cddlBranch"
                                        Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="ddlBranch_Change" />
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>


                            <div style="clear: both;"></div>
                            <div class="col-md-3 lblmTop8 relative">

                                <label>
                                    Customer<span style="color: red">*</span>
                                    <% if (false)
                                       { %>
                                    <i id="openlink" class="fa fa-plus-circle ml5" aria-hidden="true"></i>

                                    <% 
                                       } 
                                    %>
                                </label>






                                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                                </dxe:ASPxButtonEdit>
                                <span id="MandatorysCustomer" class="iconCustomer pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>


                            <div class="col-md-3" id="div_InvoiceNo" style="display: none">
                                <label>
                                    <dxe:ASPxLabel ID="txtPartyInvoiceNo" runat="server" Text="Party Debit Note Number">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtPartyInvoice" runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                            <div class="col-md-3" id="div_InvoiceDate" style="display: none">

                                <label>
                                    <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Party Debit Note Date">
                                    </dxe:ASPxLabel>

                                </label>

                                <dxe:ASPxDateEdit ID="dt_PartyDate" runat="server" EditFormat="Custom" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ClientInstanceName="cPLPartyDate" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </div>
                            <div class="col-md-3">
                                <label>
                                    <dxe:ASPxLabel ID="lblInvoiceNo" ClientInstanceName="clblInvoiceNo" runat="server" Text="Ref. Sale Invoice No.">
                                    </dxe:ASPxLabel>

                                </label>
                                <dxe:ASPxComboBox ID="ddlInvoice" runat="server" ClientInstanceName="cddlInvoice" Width="100%">
                                </dxe:ASPxComboBox>
                            </div>



                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-6 lblmTop8 lblmBot4">
                                        <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddlCurrency" ClientInstanceName="cddlCurrency" runat="server" Width="95%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-6 lblmTop8 lblmBot4">
                                        <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Exch. Rate">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txtRate" ClientInstanceName="ctxt_Rate" runat="server" Width="100%">
                                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                            <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                        </dxe:ASPxTextBox>
                                    </div>

                                </div>
                            </div>


                            <div class="col-md-3 ">
                                <label>Reason For Issuing document</label>
                                <div>
                                    <dxe:ASPxComboBox ID="ddl_Reason" runat="server" ClientInstanceName="cddl_Reason"
                                        Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div class="clear pdbot30"></div>

                            <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid" KeyFieldName="Note_Id"
                                OnBatchUpdate="grid_BatchUpdate"
                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                OnDataBinding="grid_DataBinding"
                                Width="100%" Settings-ShowFooter="true"
                                SettingsBehavior-AllowSort="false"
                                SettingsBehavior-AllowDragDrop="false"
                                SettingsPager-Mode="ShowAllRecords"
                                Settings-VerticalScrollBarMode="auto"
                                Settings-VerticalScrollableHeight="170"
                                OnRowInserting="Grid_RowInserting"
                                OnRowUpdating="Grid_RowUpdating"
                                OnRowDeleting="Grid_RowDeleting">
                                <SettingsPager Visible="false"></SettingsPager>

                                <Settings ShowStatusBar="Hidden" />

                                <Styles>
                                    <Cell Wrap="False"></Cell>
                                </Styles>

                                <Columns>
                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4%" VisibleIndex="0" Caption="">
                                        <CustomButtons>
                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                            </dxe:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dxe:GridViewCommandColumn>
                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="3%">
                                        <PropertiesTextEdit>
                                        </PropertiesTextEdit>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataButtonEditColumn FieldName="MainAccount" Caption="Main Account" VisibleIndex="2">
                                        <PropertiesButtonEdit>
                                            <ClientSideEvents ButtonClick="MainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                            <Buttons>
                                                <dxe:EditButton Text="..." Width="20px">
                                                </dxe:EditButton>
                                            </Buttons>
                                        </PropertiesButtonEdit>
                                    </dxe:GridViewDataButtonEditColumn>


                                    <dxe:GridViewDataButtonEditColumn FieldName="bthSubAccount" Caption="Sub Account" VisibleIndex="3">
                                        <PropertiesButtonEdit>
                                            <ClientSideEvents ButtonClick="SubAccountButnClick" KeyDown="SubAccountKeyDown" />
                                            <Buttons>
                                                <dxe:EditButton Text="..." Width="20px">
                                                </dxe:EditButton>
                                            </Buttons>
                                        </PropertiesButtonEdit>
                                    </dxe:GridViewDataButtonEditColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Amount" FieldName="btnRecieve" Width="130" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                            <ClientSideEvents KeyDown="OnKeyDown" LostFocus="DebitLostFocus"
                                                GotFocus="function(s,e){
                                                                        DebitGotFocus(s,e); 
                                                                        }" />
                                            <ClientSideEvents />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesTextEdit>

                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="5" Width="10%" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesButtonEdit Style-HorizontalAlign="Right">
                                            <ClientSideEvents ButtonClick="taxAmtButnClick" KeyDown="TaxAmountKeyDown" />
                                            <Buttons>
                                                <dxe:EditButton Text="..." Width="20px">
                                                </dxe:EditButton>
                                            </Buttons>
                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesButtonEdit>
                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataButtonEditColumn>

                                    <dxe:GridViewDataTextColumn FieldName="NetAmount" Caption="Net Amount" VisibleIndex="6" Width="12%" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Remarks" FieldName="btnLineNarration" Width="160">
                                        <PropertiesTextEdit>
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30" VisibleIndex="8" Caption=" ">
                                        <CustomButtons>
                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                <Image Url="/assests/images/add.png">
                                                </Image>
                                            </dxe:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dxe:GridViewCommandColumn>
                                    <dxe:GridViewDataTextColumn FieldName="HSNCODE" Caption="HSN" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Note_Id" Caption="Srl No" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="gvColMainAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="gvColSubAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="IsSubledger" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="UpdateEdit" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" BatchEditStartEditing="OnBatchEditStartEditing"
                                    CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" />
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                                </SettingsEditing>
                                <Styles>
                                    <StatusBar CssClass="statusBar">
                                    </StatusBar>
                                </Styles>
                            </dxe:ASPxGridView>
                        </div>




                        <div class="text-center">
                            <table style="margin-left: 193px; margin-top: 5px; margin-bottom: 5px">

                                <tr>
                                    <td style="padding-right: 6px; width: 150px;">Taxable Amount</td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxableAmount" runat="server" Width="105px" ClientInstanceName="c_txtTaxableAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td style="padding-right: 16px; width: 150px;">Tax Amount</td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxAmount" runat="server" Width="105px" ClientInstanceName="c_txtTaxAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td style="padding-right: 16px; width: 150px;">Total Amount</td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txt_Debit" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td style="display: none;">
                                        <dxe:ASPxTextBox ID="txt_Credit" runat="server" Width="105px" ClientInstanceName="c_txt_Credit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="clearfix" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                            <div class="col-md-12">
                                <label>Main Narration</label>
                                <div>
                                    <asp:TextBox ID="txtNarration" Font-Names="Arial" runat="server" TextMode="MultiLine"
                                        Width="100%" meta:resourcekey="txtNarrationResource1" Height="40px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div>
                            <b><span id="tagged" runat="server" style="display: none; color: red">This Customer Debit/Credit Note is tagged with Document : <span id="spanTaggedDocNo"></span>. Cannot Modify data!!</span></b>
                            <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveNew" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClickNew();}" />
                            </dxe:ASPxButton>
                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                            </dxe:ASPxButton>
                            <dxe:ASPxButton ID="btnUDF" ClientInstanceName="cbtnUDF" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                            </dxe:ASPxButton>
                        </div>








                    </dxe:ContentControl>
                </ContentCollection>

            </dxe:TabPage>
            <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <ucbs:Sales_BillingShipping runat="server" ID="BillingShippingControl" />
                        <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="CN" />
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
        </TabPages>
        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
                                                 
                                               if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                           else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }


	                                            }"></ClientSideEvents>

    </dxe:ASPxPageControl>






    <%--Modal Section--%>

    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search by Entity Name or Unique Id" />
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




    <div class="modal fade" id="MainAccountModel" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeModal();">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydown(event)" id="txtMainAccountSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Short Name</th>
                                <th>Subledger Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="closeModal();">Close</button>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="SubAccountModel" role="dialog" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="CloseSubModal();">&times;</button>
                    <h4 class="modal-title">Sub Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SubAccountNewkeydown(event)" id="txtSubAccountSearch" autofocus width="100%" placeholder="Search By Sub Account Name or Code" />

                    <div id="SubAccountTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Sub Account Name [Unique ID]</th>
                                <th>Sub Account Code</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="CloseSubModal();">Close</button>
                </div>
            </div>

        </div>
    </div>


    <%--End Modal Structure--%>




    <%--Tax PopUp Start--%>
    <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
        Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <span style="color: #fff"><strong>Select Tax</strong></span>
            <dxe:ASPxImage ID="ASPxImage31" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            caspxTaxpopUp.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <asp:HiddenField runat="server" ID="setCurrentProdCode" />
                <asp:HiddenField runat="server" ID="HdSerialNo" />
                <asp:HiddenField runat="server" ID="HdSerialNo1" />
                <asp:HiddenField runat="server" ID="hdnDeleteSrlNo" Value="0" />
                <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
                <asp:HiddenField runat="server" ID="HdProdNetAmt" />


                <input type="hidden" id="IsTaxApplicable" value="" />

                <div id="content-6">
                    <div class="col-sm-3">
                        <div class="lblHolder" style="margin-bottom: 8px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Gross Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 28px">
                                            <dxe:ASPxLabel ID="lblTaxProdGrossAmt" runat="server" Text="" ClientInstanceName="clblTaxProdGrossAmt"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-3 gstGrossAmount">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>GST</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 28px">
                                            <dxe:ASPxLabel ID="lblGstForGross" runat="server" Text="" ClientInstanceName="clblGstForGross"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-3" style="display: none">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Discount</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 28px">
                                            <dxe:ASPxLabel ID="lblTaxDiscount" runat="server" Text="" ClientInstanceName="clblTaxDiscount"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>


                    <div class="col-sm-3">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Net Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 28px">
                                            <dxe:ASPxLabel ID="lblProdNetAmt" runat="server" Text="" ClientInstanceName="clblProdNetAmt"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-2 gstNetAmount">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>GST</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 28px">
                                            <dxe:ASPxLabel ID="lblGstForNet" runat="server" Text="" ClientInstanceName="clblGstForNet"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                </div>

                <%--Error Message--%>
                <div id="ContentErrorMsg">
                    <div class="col-sm-8">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Status
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Tax Code/Charges Not defined.
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <table style="width: 100%;">
                    <tr>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="2"></td>
                    </tr>
                    <tr style="display: none">
                        <td><span><strong>Product Basic Amount</strong></span></td>
                        <td>
                            <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" ReadOnly="true"
                                runat="server" Width="50%">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </td>
                    </tr>
                    <tr class="cgridTaxClass">
                        <td colspan="3">

                            <dxe:ASPxGridView runat="server" OnBatchUpdate="taxgrid_BatchUpdate" KeyFieldName="Taxes_ID" ClientInstanceName="cgridTax" ID="aspxGridTax"
                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                OnCustomCallback="cgridTax_CustomCallback"
                                Settings-ShowFooter="false" AutoGenerateColumns="False"
                                OnCellEditorInitialize="aspxGridTax_CellEditorInitialize"
                                OnRowInserting="taxgrid_RowInserting" OnRowUpdating="taxgrid_RowUpdating" OnRowDeleting="taxgrid_RowDeleting">
                                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto" ShowStatusBar="Hidden"></Settings>
                                <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                <SettingsPager Visible="false"></SettingsPager>

                                <Styles>
                                    <Cell Wrap="False"></Cell>
                                </Styles>

                                <Columns>
                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Taxes_Name" ReadOnly="true" Caption="Tax Component ID">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="taxCodeName" ReadOnly="true" Caption="Tax Component Name">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>

                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch">
                                    <BatchEditSettings EditMode="row" ShowConfirmOnLosingChanges="false" />
                                </SettingsEditing>
                                <ClientSideEvents EndCallback="cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />
                            </dxe:ASPxGridView>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="3" style="padding-top: 5px">
                            <div class="pull-left" id="calculateTotalAmountOK">
                                <input type="button" onclick="calculateTotalAmount()" class="btn btn-primary" value="Ok" />
                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-right: 5px"><strong>Total Charges</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </dxe:ASPxTextBox>

                                    </td>
                                </tr>
                            </table>
                            <div class="clear"></div>
                        </td>
                    </tr>

                </table>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <%--Tax popup End--%>





    <div>
        <asp:HiddenField runat="server" ID="hdAddEdit" />
        <div runat="server" id="ReceiptPaymentId" visible="false"></div>
        <asp:HiddenField runat="server" ID="hdnCustomerId" />
        <asp:HiddenField runat="server" ID="hdnRefreshType" />
        <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
        <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
        <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
        <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
        <asp:HiddenField runat="server" ID="TaxAmountOngrid" />
        <asp:HiddenField runat="server" ID="VisibleIndexForTax" />
        <asp:HiddenField runat="server" ID="DoEdit" />
        <asp:HiddenField runat="server" ID="hdnTagCount" />
    </div>

    <dxe:ASPxCallbackPanel runat="server" ID="deleteTax" ClientInstanceName="cdeleteTax" OnCallback="deleteTax_Callback">
        <ClientSideEvents EndCallback="deleteTaxEndCallBack" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxLoadingPanel ID="LoadingPanelCRP" runat="server" ClientInstanceName="cLoadingPanelCRP"
        Modal="True">
    </dxe:ASPxLoadingPanel>

</asp:Content>

