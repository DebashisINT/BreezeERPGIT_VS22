<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ServiceMaterialIssueAdd.aspx.cs" Inherits="ERP.OMS.Management.Activities.ServiceMaterialIssueAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js?v=0.02"></script>
    <link href="CSS/CustomerReceiptAdjustment.css" rel="stylesheet" />
    <script src="JS/ServiceMaterialIssue.js"></script>
    <style>
        .inputTypediv {
            border: 1px solid #ccc;
            padding: 2px;
            background: #efefef;
        }

        #Popup_Warehouse_PW-1 {
            position: fixed !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <asp:Label ID="lblHeading" runat="server" Text="Material Issue-Add"></asp:Label>
        </h3>
    </div>
    <div id="ApprovalCross" runat="server" class="crossBtn"><a href="ServiceMaterialIssueList.aspx"><i class="fa fa-times"></i></a></div>

    <div class="form_main">
        <div class="row">
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-2" id="divNumberingScheme" runat="server">
                        <label style="margin-top: 8px">Numbering Scheme</label>
                        <div>
                            <dxe:ASPxComboBox ID="CmbScheme" ClientInstanceName="cCmbScheme"
                                SelectedIndex="0" EnableCallbackMode="false"
                                TextField="SchemaName" ValueField="ID"
                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                <ClientSideEvents ValueChanged="CmbScheme_ValueChange"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                            <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label style="margin-top: 8px">Document No.<span style="color: red">*</span></label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" MaxLength="16" Text="Auto" ClientEnabled="false">
                            </dxe:ASPxTextBox>
                            <span id="MandatoryAdjNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label style="margin-top: 8px">Posting Date<span style="color: red">*</span></label>
                        <div>
                            <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom" AllowNull="false"
                                Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                <ButtonStyle Width="13px"></ButtonStyle>
                                <ClientSideEvents GotFocus="function(s,e){cdtTDate.ShowDropDown();}" DateChanged="cAdjDateChange"></ClientSideEvents>
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                    <div class="col-md-2 lblmTop8">
                        <label>Unit <span style="color: red">*</span></label>
                        <div>
                            <asp:DropDownList ID="ddlBranch" runat="server" onchange="ddlBranch_SelectedIndexChanged()"
                                DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                            </asp:DropDownList>


                        </div>
                    </div>
                    <div class="col-md-4 lblmTop8">
                        <label>Reference</label>
                        <div>
                            <div>
                                <dxe:ASPxTextBox runat="server" ID="txtReference" ClientInstanceName="ctxtReference" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">

                    <div class="col-md-2 lblmTop8">
                        <label>Customer<span style="color: red">*</span></label>
                        <div>
                            <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName">

                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>

                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                            <span id="MandatoryCustomer" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                    <div class="col-md-2 lblmTop8">
                        <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                        </dxe:ASPxLabel>
                        <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px" OnCallback="cmbContactPerson_Callback">
                        </dxe:ASPxComboBox>
                    </div>

                    <div class="col-md-2 lblmTop8">
                        <asp:RadioButtonList ID="rdl_SaleInvoice" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();">
                            <asp:ListItem Text="Contract" Value="SO"></asp:ListItem>
                        </asp:RadioButtonList>
                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <asp:HiddenField runat="server" ID="OldSelectedKeyvalue" />
                                    <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridquotationLookup"
                                        OnDataBinding="lookup_quotation_DataBinding"
                                        KeyFieldName="ComponentID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                            <dxe:GridViewDataColumn FieldName="ComponentNumber" Visible="true" VisibleIndex="1" Caption="Number" Width="180" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Date" Width="100" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Customer Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="RevNo" Visible="true" VisibleIndex="4" Caption="Revision No." Width="100" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="RevDate" Visible="true" VisibleIndex="5" Caption="Revision Date" Width="80" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="ReferenceName" Visible="true" VisibleIndex="6" Caption="Reference" Width="150" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="BranchName" Visible="true" VisibleIndex="7" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                        </Columns>
                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                            <Templates>
                                                <StatusBar>
                                                    <table class="OptionsTable" style="float: right">
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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
                                        <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridquotationLookup.ShowDropDown();  }" DropDown="LoadOldSelectedKeyvalue" />
                                    </dxe:ASPxGridLookup>
                                </dxe:PanelContent>
                            </PanelCollection>
                            <%--   <ClientSideEvents EndCallback="componentEndCallBack" BeginCallback="BeginComponentCallback" />--%>
                        </dxe:ASPxCallbackPanel>
                        <dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
                            Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                            <HeaderTemplate>
                                <strong><span style="color: #fff">Select Products</span></strong>
                                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                                    <ClientSideEvents Click="function(s, e){ 
                                                            cProductsPopup.Hide();
                                                        }" />
                                </dxe:ASPxImage>
                            </HeaderTemplate>
                            <ContentCollection>
                                <dxe:PopupControlContentControl runat="server">
                                    <div style="padding: 7px 0;">
                                        <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                                        <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                        <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                                    </div>
                                    <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                        OnCustomCallback="cgridProducts_CustomCallback" OnDataBinding="grid_Products_DataBinding"
                                        Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number" Settings-AllowFilterBySearchPanel="True" Settings-AllowAutoFilter="True">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                                <PropertiesTextEdit>
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                        <SettingsDataSecurity AllowEdit="true" />
                                    </dxe:ASPxGridView>
                                    <div class="text-center">
                                        <asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" UseSubmitBehavior="false" />
                                    </div>
                                </dxe:PopupControlContentControl>
                            </ContentCollection>
                            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                        </dxe:ASPxPopupControl>
                    </div>
                    <div class="col-md-2 lblmTop8">
                        <dxe:ASPxLabel ID="lbl_InvoiceNO" ClientInstanceName="clbl_InvoiceNO" runat="server" Text="Posting Date">
                        </dxe:ASPxLabel>
                        <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                            <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">
                                        <dxe:ASPxTextBox ID="txt_InvoiceDate" ClientInstanceName="ctxt_InvoiceDate" runat="server" Width="100%" ClientEnabled="false">
                                        </dxe:ASPxTextBox>
                                    </dxe:PanelContent>
                                </PanelCollection>
                            </dxe:ASPxCallbackPanel>
                        </div>
                    </div>
                    <div class="col-md-2 lblmTop8" id="DivTechnician" runat="server">
                        <label class="darkLabel mTop5">
                            Technician                           
                        </label>
                        <div>
                            <dxe:ASPxButtonEdit ID="txtTechnician" runat="server" ReadOnly="true" ClientInstanceName="ctxtTechnician">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){TechnicianButnClick();}" KeyDown="function(s,e){TechnicianKeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                            <span id="MandatoryTechnician" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                </div>

                <div class="row">

                    <div class="col-md-2" id="DivSegment1" runat="server">
                        <dxe:ASPxLabel ID="lblSegment1" runat="server" Text="Segment1">
                        </dxe:ASPxLabel>

                        <dxe:ASPxButtonEdit ID="txtSegment1" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment1" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){Segment1ButnClick();}" KeyDown="function(s,e){Segment1_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>

                    </div>
                    <div class="col-md-2  " id="DivSegment2" runat="server">
                        <dxe:ASPxLabel ID="lblSegment2" runat="server" Text="Segment2">
                        </dxe:ASPxLabel>
                        <dxe:ASPxButtonEdit ID="txtSegment2" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment2" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){Segment2ButnClick();}" KeyDown="function(s,e){Segment2_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>
                    </div>
                    <div class="col-md-2  " id="DivSegment3" runat="server">
                        <dxe:ASPxLabel ID="ASPxLabel16" runat="server" Text="Segment3">
                        </dxe:ASPxLabel>

                        <dxe:ASPxButtonEdit ID="txtSegment3" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment3" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){Segment3ButnClick();}" KeyDown="function(s,e){Segment3_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>

                    </div>
                    <div class="col-md-2  " id="DivSegment4" runat="server">
                        <dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="Segment4">
                        </dxe:ASPxLabel>

                        <dxe:ASPxButtonEdit ID="txtSegment4" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment4" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){Segment4ButnClick();}" KeyDown="function(s,e){Segment4_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>

                    </div>
                    <div class="col-md-2  " id="DivSegment5" runat="server">
                        <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Segment5">
                        </dxe:ASPxLabel>

                        <dxe:ASPxButtonEdit ID="txtSegment5" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment5" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){Segment5ButnClick();}" KeyDown="function(s,e){Segment5_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>

                    </div>
                </div>
                <div style="clear: both"></div>
                <div class="clear"></div>



                <div class="row mTop5">
                    <div class="col-md-12 mTop5">
                        <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid"
                            OnDataBinding="grid_DataBinding"
                            OnCustomCallback="grid_CustomCallback"
                            OnRowInserting="Grid_RowInserting"
                            OnRowUpdating="Grid_RowUpdating"
                            OnRowDeleting="Grid_RowDeleting"
                            OnCustomJSProperties="grid_CustomJSProperties"
                            KeyFieldName="QuotationID"
                            SettingsBehavior-AllowSort="false"
                            SettingsPager-Mode="ShowAllRecords"
                            Settings-VerticalScrollBarMode="auto"
                            Settings-VerticalScrollableHeight="150"
                            Width="100%">
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4%" VisibleIndex="0" Caption=" ">
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/Edit.png">
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>
                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="5%">
                                    <PropertiesTextEdit>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ComponentNumber" Caption="Document No." VisibleIndex="2" ReadOnly="True" Width="9%">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ProductName" Caption="Product" VisibleIndex="2" Width="160px" ReadOnly="true">
                                    <CellStyle Wrap="True"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="3" Width="160px" ReadOnly="true">
                                    <CellStyle Wrap="True"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="4" Width="92px" HeaderStyle-HorizontalAlign="Right" ReadOnly="True">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                        <%--<ClientSideEvents LostFocus="QuantityTextChange" />--%>
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="5" Width="100px" ReadOnly="true">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Price" VisibleIndex="6" Width="60px" HeaderStyle-HorizontalAlign="Right" ReadOnly="True">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        <%-- <ClientSideEvents LostFocus="RateTextChange" />--%>
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="7" Width="60px" HeaderStyle-HorizontalAlign="Right" ReadOnly="True">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                        <%-- <ClientSideEvents LostFocus="AmountTextChange" />--%>
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="ProductID" Width="0">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Component ID" VisibleIndex="19" ReadOnly="True" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="DocDetailsID" Caption="Doc Details ID" VisibleIndex="25" ReadOnly="True" Width="0">
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" CustomButtonClick="gridCustomButtonClick"
                                EndCallback="gridGridEndCallBack" />
                            <SettingsDataSecurity AllowEdit="true" />
                            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                            </SettingsEditing>

                            <Settings ShowStatusBar="Hidden" />
                            <Styles>
                                <StatusBar CssClass="statusBar">
                                </StatusBar>
                            </Styles>
                        </dxe:ASPxGridView>
                    </div>
                </div>

                <div style="clear: both"></div>
                <div style="clear: both"></div>
                <div id="DivLbl">
                    <div class="row">
                        <div class="col-md-2">
                            <div>
                                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Document No." ForeColor="Red">
                                </dxe:ASPxLabel>
                            </div>
                            <div class="inputTypediv">
                                <dxe:ASPxLabel ID="lblComponentNumber" runat="server">
                                </dxe:ASPxLabel>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div>
                                <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Product" ForeColor="Red">
                                </dxe:ASPxLabel>
                            </div>
                            <div class="inputTypediv">
                                <dxe:ASPxLabel ID="lblProduct" runat="server">
                                </dxe:ASPxLabel>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div>
                                <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Description" ForeColor="Red">
                                </dxe:ASPxLabel>
                            </div>
                            <div class="inputTypediv">
                                <dxe:ASPxLabel ID="lblDescription" runat="server">
                                </dxe:ASPxLabel>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div>
                                <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Quantity" ForeColor="Red">
                                </dxe:ASPxLabel>
                            </div>
                            <div class="inputTypediv">
                                <dxe:ASPxLabel ID="lblQuantity" runat="server">
                                </dxe:ASPxLabel>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div>
                                <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="UOM" ForeColor="Red">
                                </dxe:ASPxLabel>
                            </div>
                            <div class="inputTypediv">
                                <dxe:ASPxLabel ID="lblUOM" runat="server">
                                </dxe:ASPxLabel>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div>
                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="Price" ForeColor="Red">
                                </dxe:ASPxLabel>
                            </div>
                            <div class="inputTypediv">
                                <dxe:ASPxLabel ID="lblPrice" runat="server">
                                </dxe:ASPxLabel>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            <div>
                                <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="Amount" ForeColor="Red">
                                </dxe:ASPxLabel>
                            </div>
                            <div class="inputTypediv">
                                <dxe:ASPxLabel ID="lblAmount" runat="server">
                                </dxe:ASPxLabel>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Service Template" ForeColor="Red">
                            </dxe:ASPxLabel>

                            <dxe:ASPxButtonEdit ID="txtServiceTemplate" runat="server" ReadOnly="true" ClientInstanceName="ctxtServiceTemplate" Width="100%">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){ServiceTemplateButnClick();}" KeyDown="function(s,e){ServiceTemplateKeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>

                        </div>
                        <div class="col-md-2 hide">
                            <dxe:ASPxLabel ID="lblProductID" runat="server">
                            </dxe:ASPxLabel>
                        </div>
                        <div class="col-md-2 hide">
                            <dxe:ASPxLabel ID="lblComponentID" runat="server">
                            </dxe:ASPxLabel>
                        </div>
                        <div class="col-md-2 hide">
                            <dxe:ASPxLabel ID="lblDocDetailsID" runat="server">
                            </dxe:ASPxLabel>
                        </div>
                        <div class="col-md-2 hide">
                            <dxe:ASPxLabel ID="lblServiceTempID" runat="server">
                            </dxe:ASPxLabel>
                        </div>
                        <div class="col-md-2 hide">
                            <dxe:ASPxLabel ID="lblActualSrlID" runat="server">
                            </dxe:ASPxLabel>
                        </div>
                    </div>
                </div>
                <div style="clear: both"></div>
                <div style="clear: both"></div>
                <div class="row mTop5">
                    <div class="col-md-12 mTop5">
                        <dxe:ASPxGridView runat="server" ClientInstanceName="Editgrid" ID="Editgrid"
                            OnBatchUpdate="Editgrid_BatchUpdate"
                            OnDataBinding="Editgrid_DataBinding"
                            OnCustomCallback="Editgrid_CustomCallback"
                            OnRowInserting="Editgrid_RowInserting"
                            OnRowUpdating="Editgrid_RowUpdating"
                            OnRowDeleting="Editgrid_RowDeleting"
                            OnCustomJSProperties="Editgrid_CustomJSProperties"
                            KeyFieldName="ActualSL"
                            SettingsBehavior-AllowSort="false"
                            SettingsPager-Mode="ShowAllRecords"
                            Settings-VerticalScrollBarMode="auto"
                            Settings-VerticalScrollableHeight="200"
                            OnCellEditorInitialize="Editgrid_CellEditorInitialize"
                            Width="100%">
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4%" VisibleIndex="0" Caption=" ">
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="EditgridCustomDelete" Image-Url="/assests/images/crs.png">
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>
                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="5%">
                                    <PropertiesTextEdit>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="2" Width="180px" ReadOnly="True">
                                    <PropertiesButtonEdit>
                                        <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                        <Buttons>
                                            <dxe:EditButton Text="..." Width="20px">
                                            </dxe:EditButton>
                                        </Buttons>
                                    </PropertiesButtonEdit>
                                </dxe:GridViewDataButtonEditColumn>
                                <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="3" Width="160px" ReadOnly="true">
                                    <CellStyle Wrap="True"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="4" Width="92px" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                        <ClientSideEvents LostFocus="QuantityTextChange" />
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="5" Width="100px" ReadOnly="true">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewCommandColumn VisibleIndex="6" Caption="Stk Details" Width="6%">
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                            <Image ToolTip="Warehouse" Url="/assests/images/warehouse.png">
                                            </Image>
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>

                                <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Price" VisibleIndex="7" Width="60px" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        <ClientSideEvents LostFocus="RateTextChange" />
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="8" Width="60px" HeaderStyle-HorizontalAlign="Right" ReadOnly="True">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                        <%-- <ClientSideEvents LostFocus="AmountTextChange" />--%>
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ServiceTemplate" Caption="Service Template" VisibleIndex="9" Width="160px" ReadOnly="true">
                                    <CellStyle Wrap="True"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="10" Width="200px">
                                    <PropertiesTextEdit>
                                        <ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>
                                    </PropertiesTextEdit>
                                    <CellStyle></CellStyle>
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="ServiceTemplateID" Caption="ProductID" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Component ID" VisibleIndex="19" ReadOnly="True" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="DocDetailsID" Caption="Doc Details ID" VisibleIndex="25" ReadOnly="True" Width="0">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ActualSL" Width="0">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="ProductID" Width="0">
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents RowClick="EditgridGetVisibleIndex" BatchEditStartEditing="EditgridFocusedRowChanged" CustomButtonClick="EditgridCustomButtonClick"
                                EndCallback="EditGridEndCallBack" />
                            <SettingsDataSecurity AllowEdit="true" />
                            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                            </SettingsEditing>

                            <Settings ShowStatusBar="Hidden" />
                            <Styles>
                                <StatusBar CssClass="statusBar">
                                </StatusBar>
                            </Styles>
                        </dxe:ASPxGridView>
                    </div>
                </div>

            </div>

        </div>
        <div class="clear"></div>
        <div class="row">
            <%--  <div class="col-md-12" style="top: 60px; left: 13px;">
                <table style="float: left;" id="tblAddBtnSavePanel">
                    <tr>
                        <td style="padding: 5px 0px;">
                            <span id="Span2" runat="server">                              
                                <dxe:ASPxButton ID="btnAddSaveRecords" ClientInstanceName="cbtnAddSaveRecords" runat="server" AutoPostBack="False" Text="ADD"
                                    CssClass="btn btn-primary" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {AddButtonClick();}" />
                                </dxe:ASPxButton>                               
                            </span>                            
                        </td>
                    </tr>
                </table>
            </div>--%>
            <div class="col-md-12" style="padding-top: 5px">
                <dxe:ASPxButton ID="btnAddSaveRecords" ClientInstanceName="cbtnAddSaveRecords" runat="server" AutoPostBack="False" Text="ADD" ClientVisible="false"
                    CssClass="btn btn-primary" UseSubmitBehavior="False">
                    <ClientSideEvents Click="function(s, e) {AddButtonClick();}" />
                </dxe:ASPxButton>
            </div>
        </div>
        <div class="clear"></div>
        <div class="clear"></div>
        <div class="row mTop5">
            <div class="col-md-12 mTop5">
                <dxe:ASPxGridView runat="server" ClientInstanceName="Finalgrid" ID="Finalgrid"
                    OnBatchUpdate="Finalgrid_BatchUpdate"
                    OnDataBinding="Finalgrid_DataBinding"
                    OnCustomCallback="Finalgrid_CustomCallback"
                    OnRowInserting="Finalgrid_RowInserting"
                    OnRowUpdating="Finalgrid_RowUpdating"
                    OnRowDeleting="Finalgrid_RowDeleting"
                    OnCustomJSProperties="Finalgrid_CustomJSProperties"
                    KeyFieldName="ActualSL"
                    SettingsBehavior-AllowSort="false"
                    SettingsPager-Mode="ShowAllRecords"
                    Settings-VerticalScrollBarMode="auto"
                    Settings-VerticalScrollableHeight="200"
                    OnCellEditorInitialize="Finalgrid_CellEditorInitialize"
                    Width="100%">
                    <SettingsPager Visible="false"></SettingsPager>
                    <Columns>
                        <%--<dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4%" VisibleIndex="0" Caption=" ">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton Text=" " ID="FinalgridgridCustomDelete" Image-Url="/assests/images/viewlcon.png">
                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>--%>
                        <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="5%">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="ComponentNumber" Caption="Document No." VisibleIndex="2" ReadOnly="True" Width="9%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="180px" ReadOnly="True">
                            <PropertiesButtonEdit>
                                <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                <Buttons>
                                    <dxe:EditButton Text="..." Width="20px">
                                    </dxe:EditButton>
                                </Buttons>
                            </PropertiesButtonEdit>
                        </dxe:GridViewDataButtonEditColumn>
                        <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4" Width="160px" ReadOnly="true">
                            <CellStyle Wrap="True"></CellStyle>
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="5" Width="92px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                            <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                <ClientSideEvents LostFocus="FinalgridQuantityTextChange" />
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="6" Width="100px" ReadOnly="true">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Price" VisibleIndex="7" Width="60px" HeaderStyle-HorizontalAlign="Right">
                            <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                <ClientSideEvents LostFocus="FinalgridRateTextChange" />
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="8" Width="60px" HeaderStyle-HorizontalAlign="Right" ReadOnly="True">
                            <PropertiesTextEdit Style-HorizontalAlign="Right">
                                <%-- <ClientSideEvents LostFocus="AmountTextChange" />--%>
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="ServiceTemplate" Caption="Service Template" VisibleIndex="9" Width="160px" ReadOnly="true">
                            <CellStyle Wrap="True"></CellStyle>
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4%" VisibleIndex="10" Caption=" ">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton Text=" " ID="FinalgridgridCustomEdit" Image-Url="/assests/images/viewIcon.png">
                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>

                        <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="ProductID" Width="0">
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="ServiceTemplateID" Caption="ProductID" Width="0">
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Component ID" VisibleIndex="19" ReadOnly="True" Width="0">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="DocDetailsID" Caption="Doc Details ID" VisibleIndex="25" ReadOnly="True" Width="0">
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="ActualSL" Width="0">
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <ClientSideEvents RowClick="FinalgridGetVisibleIndex" BatchEditStartEditing="EditgridFocusedRowChanged" CustomButtonClick="FinalgridCustomButtonClick"
                        EndCallback="GridEndCallBack" />
                    <SettingsDataSecurity AllowEdit="true" />
                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                        <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                    </SettingsEditing>

                    <Settings ShowStatusBar="Hidden" />
                    <Styles>
                        <StatusBar CssClass="statusBar">
                        </StatusBar>
                    </Styles>
                </dxe:ASPxGridView>
            </div>
        </div>
        <div class="clear"></div>
        <div class="row">
            <div class="col-md-12" style="top: 60px; left: 13px;">
                <table style="float: left;" id="tblBtnSavePanel">
                    <tr>
                        <td style="padding: 5px 0px;">
                            <span id="tdSaveButton" runat="server">
                                <%--<% if (rights.CanAdd)
                                       { %>--%>
                                <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New"
                                    CssClass="btn btn-primary" UseSubmitBehavior="False" ClientVisible="false">
                                    <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                </dxe:ASPxButton>
                                <%--   <%} %>--%>
                            </span>
                            <span id="Span1" runat="server">
                                <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it"
                                    CssClass="btn btn-primary" UseSubmitBehavior="False" ClientVisible="false">
                                    <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                                </dxe:ASPxButton>
                            </span>
                        </td>

                    </tr>
                </table>
            </div>
        </div>


    </div>




    <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
        Width="900px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Middle" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	     closeWarehouse(s, e);}" />
        <ContentStyle VerticalAlign="Middle" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                        <ul>
                            <li>
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Selected Product</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblProductName" runat="server"></asp:Label></td>
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
                                                <td>Entered Quantity </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="txt_SalesAmount" runat="server"></asp:Label>
                                                    <asp:Label ID="txt_SalesUOM" runat="server"></asp:Label>

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="lblHolder" id="divpopupAvailableStock" style="display: none;">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Available Stock</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAvailableStock" runat="server"></asp:Label>
                                                    <asp:Label ID="lblAvailableStockUOM" runat="server"></asp:Label>

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li style="display: none;">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Stock Quantity </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="txt_StockAmount" runat="server"></asp:Label>
                                                    <asp:Label ID="txt_StockUOM" runat="server"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>

                        </ul>
                    </div>

                    <div class="clear">
                        <br />
                    </div>
                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                        <div>
                            <div class="col-md-3" id="div_Warehouse">
                                <div style="margin-bottom: 5px;">
                                    Warehouse
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                        TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                        <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange()}" EndCallback="CmbWarehouseEndCallback"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                    <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_Batch">
                                <div style="margin-bottom: 5px;">
                                    Batch/Lot
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxComboBox ID="CmbBatch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBatch"
                                        TextField="BatchName" ValueField="BatchID" runat="server" Width="100%" OnCallback="CmbBatch_Callback">
                                        <ClientSideEvents ValueChanged="function(s,e){CmbBatch_ValueChange()}" EndCallback="CmbBatchEndCall"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                    <span id="spnCmbBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-4" id="div_Serial">
                                <div style="margin-bottom: 5px;">
                                    Serial No &nbsp;&nbsp; (
                                                <input type="checkbox" id="myCheck" name="BarCode" onchange="AutoCalculateMandateOnChange(this)">Barcode )
                                </div>
                                <div class="" id="divMultipleCombo">
                                    <%--<dxe:ASPxComboBox ID="CmbSerial" EnableIncrementalFiltering="True" ClientInstanceName="cCmbSerial"
                                                    TextField="SerialName" ValueField="SerialID" runat="server" Width="100%" OnCallback="CmbSerial_Callback">
                                                </dxe:ASPxComboBox>--%>
                                    <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="85%" CssClass="pull-left" runat="server" AnimationType="None">
                                        <DropDownWindowStyle BackColor="#EDEDED" />
                                        <DropDownWindowTemplate>
                                            <dxe:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" OnCallback="CmbSerial_Callback"
                                                runat="server">
                                                <Border BorderStyle="None" />
                                                <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" EndCallback="listBoxEndCall" />
                                            </dxe:ASPxListBox>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="padding: 4px">
                                                        <dxe:ASPxButton ID="ASPxButton4" AutoPostBack="False" UseSubmitBehavior="false" runat="server" Text="Close" Style="float: right">
                                                            <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                        </dxe:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </DropDownWindowTemplate>
                                        <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" GotFocus="function(s, e){ s.ShowDropDown(); }" />
                                    </dxe:ASPxDropDownEdit>
                                    <span id="spncheckComboBox" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    <div class="pull-left">
                                        <i class="fa fa-commenting" id="abpl" aria-hidden="true" style="font-size: 16px; cursor: pointer; margin: 3px 0 0 5px;" title="Serial No " data-container="body" data-toggle="popover" data-placement="right" data-content=""></i>
                                    </div>
                                </div>
                                <div class="" id="divSingleCombo" style="display: none;">
                                    <dxe:ASPxTextBox ID="txtserial" runat="server" Width="85%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                        <ClientSideEvents TextChanged="txtserialTextChanged" />
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div class="col-md-3" id="div_Quantity">
                                <div style="margin-bottom: 2px;">
                                    Quantity
                                </div>
                                <div class="Left_Content" style="">
                                    <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                        <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />
                                    </dxe:ASPxTextBox>
                                    <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div>
                                </div>
                                <div class="Left_Content" style="padding-top: 14px">
                                    <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" UseSubmitBehavior="False" Text="Add" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {if(!document.getElementById('myCheck').checked) SaveWarehouse();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cGrdWarehouse" OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                    VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Available Quantity" FieldName="AvailableQty" Visible="false"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                    VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Conversion Foctor" FieldName="ConversionMultiplier" Visible="false"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Stock Quantity" FieldName="StkQuantity" Visible="false"
                                    VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Balance Stock" FieldName="BalancrStk" Visible="false"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Batch/Lot Number" FieldName="BatchNo"
                                    VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="MfgDate"
                                    VisibleIndex="7">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ExpiryDate"
                                    VisibleIndex="8">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                    VisibleIndex="9">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete">
                                            <img src="../../../assests/images/Edit.png" /></a>
                                        &nbsp;
                                                        <a href="javascript:void(0);" id="ADelete" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete">
                                                            <img src="/assests/images/crs.png" /></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnWarehouseEndCallback" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="btnWarehouseSave" UseSubmitBehavior="false" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>


    <div class="modal fade" id="ProductModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Code or Name" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>
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
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name or Unique Id" />

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
    <div class="modal fade" id="Segment1Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment1header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment1keydown(event)" id="txtSegment1Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment1Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <div class="modal fade" id="Segment2Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment2Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment2keydown(event)" id="txtSegment2Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment2Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <div class="modal fade" id="Segment3Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment3Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment3keydown(event)" id="txtSegment3Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment3Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <div class="modal fade" id="Segment4Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment4Header">Segment4 Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment4keydown(event)" id="txtSegment4Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment4Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <div class="modal fade" id="Segment5Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment5Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment5keydown(event)" id="txtSegment5Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment5Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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

    <div class="modal fade" id="TechModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Technician Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Techniciankeydown(event)" id="txtTechnicianSearch" autofocus width="100%" placeholder="Search By Customer Name or Unique Id" />

                    <div id="TechnicianTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Technician Name</th>
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

     <div class="modal fade" id="SerTemModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Service Template Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SerTemkeydown(event)" id="txtSerTemSearch" autofocus width="100%" placeholder="Search By Service Template Description" />

                    <div id="SerTemTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Service Description</th>
                                <th>Service</th>
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

    <asp:HiddenField runat="server" ID="hdnSegment1" />
    <asp:HiddenField runat="server" ID="hdnSegment2" />
    <asp:HiddenField runat="server" ID="hdnSegment3" />
    <asp:HiddenField runat="server" ID="hdnSegment4" />
    <asp:HiddenField runat="server" ID="hdnSegment5" />

    <asp:HiddenField runat="server" ID="hdnValueSegment1" />
    <asp:HiddenField runat="server" ID="hdnValueSegment2" />
    <asp:HiddenField runat="server" ID="hdnValueSegment3" />
    <asp:HiddenField runat="server" ID="hdnValueSegment4" />
    <asp:HiddenField runat="server" ID="hdnValueSegment5" />
    <%-- HiddenField Feild  --%>
    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField ID="hdnTechnicianId" runat="server" />
    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField ID="hdAdvanceDocNo" runat="server" />
    <asp:HiddenField ID="hdAdjustmentId" runat="server" />
    <asp:HiddenField ID="hdAdjustmentType" runat="server" />
    <asp:HiddenField ID="HiddenSaveButton" runat="server" />
    <asp:HiddenField ID="HiddenRowCount" runat="server" />
    <asp:HiddenField ID="hdngridAddEdit" runat="server" />
    <asp:HiddenField ID="hdnVisibleIndex" runat="server" />
    <asp:HiddenField ID="hdfProductID" runat="server" />
    <asp:HiddenField ID="hdfProductSerialID" runat="server" />
    <asp:HiddenField ID="hdnProductQuantity" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hdfProductType" runat="server" />
    <asp:HiddenField ID="hdnPagecount" runat="server" />


    <asp:HiddenField ID="hdnActualSrlID" runat="server" />
    <asp:HiddenField ID="hdnlblProduct" runat="server" />
    <asp:HiddenField ID="hdnlblDescription" runat="server" />
    <asp:HiddenField ID="hdnlblQuantity" runat="server" />
    <asp:HiddenField ID="hdnlblUOM" runat="server" />
    <asp:HiddenField ID="hdnlblPrice" runat="server" />
    <asp:HiddenField ID="hdnlblAmount" runat="server" />
    <asp:HiddenField ID="hdnlblProductID" runat="server" />
    <asp:HiddenField ID="hdnlblServiceTempID" runat="server" />
    <asp:HiddenField ID="hdnlblComponentID" runat="server" />
    <asp:HiddenField ID="hdnlblDocDetailsID" runat="server" />
    <asp:HiddenField ID="hdnlblComponentNumber" runat="server" />


    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>




</asp:Content>
