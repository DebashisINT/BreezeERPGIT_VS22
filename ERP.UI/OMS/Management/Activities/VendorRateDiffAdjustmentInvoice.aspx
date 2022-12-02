<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="VendorRateDiffAdjustmentInvoice.aspx.cs" Inherits="ERP.OMS.Management.Activities.VendorRateDiffAdjustmentInvoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js?v=0.01"></script>
    <link href="CSS/VendorPaymentAdjustment.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="JS/VendorRateDiffAdjustmentInvoice.js?v=1.0.8"></script>


     <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

      <div class="panel-title clearfix" id="myDiv">
                <h3 class="pull-left">
                     <asp:Label ID="lblHeading" runat="server" Text="Adjustment of Documents - Rate difference With Invoice"></asp:Label>
                   
                 </h3>
        </div>
    <div id="ApprovalCross" runat="server" class="crossBtn"><a href="VendorRateDiffAdjustmentInvoiceList.aspx"><i class="fa fa-times"></i></a></div>

    <div class="form_main">
        <div class="row">
            <div class="col-md-12">
                <div class="row">
                <div class="col-md-2" id="divNumberingScheme" runat="server">
                    <label style="margin-top: 8px">Numbering Scheme</label>
                    <div>
                        <dxe:ASPxComboBox ID="CmbScheme"  ClientInstanceName="cCmbScheme"
                            SelectedIndex="0" EnableCallbackMode="false" 
                            TextField="SchemaName" ValueField="ID"
                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                            <ClientSideEvents ValueChanged="CmbScheme_ValueChange"></ClientSideEvents>
                        </dxe:ASPxComboBox>
                        <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>

                <div class="col-md-2">
                    <label style="margin-top: 8px">Document No.</label>
                    <div>

                          <dxe:ASPxTextBox runat="server" ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" MaxLength="16" Text="Auto" ClientEnabled="false"> 
                            </dxe:ASPxTextBox>

                        <span id="MandatoryAdjNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>

                <div class="col-md-2">
                    <label style="margin-top: 8px">Posting Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom" AllowNull="false"
                            Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                            <ButtonStyle Width="13px"></ButtonStyle>
                            <ClientSideEvents   GotFocus="function(s,e){cdtTDate.ShowDropDown();}" DateChanged="cAdjDateChange" ></ClientSideEvents>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>

                <div class="col-md-2 lblmTop8">
                    <label>For Unit <span style="color: red">*</span></label>
                    <div>
                        <asp:DropDownList ID="ddlBranch" runat="server" onchange="ddlBranch_SelectedIndexChanged()"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                        </asp:DropDownList>
                        

                    </div>
                </div>

                <div class="col-md-2 lblmTop8">
                    <label>Vendor<span style="color: red">*</span></label>
                    <div>
                        <dxe:ASPxButtonEdit ID="txtVendName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendName">

                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>

                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}"  />
                        </dxe:ASPxButtonEdit>
                        <span id="MandatoryVendor" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>


                <div class="col-md-2 lblmTop8">
                    <label>Rate Difference No.</label>
                    <div>
                        <div>
                           <dxe:ASPxButtonEdit ID="btntxtDocNo" runat="server" ReadOnly="true" ClientInstanceName="cbtntxtDocNo">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="DocumentNumberBtnClick" KeyDown="DocumentNumberBtn" />
                         </dxe:ASPxButtonEdit>
                            <span id="MandatoryDocNo" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="col-md-2 lblmTop8">
                    <label>Doc. Amount (Curr.)</label>
                    <div>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="DocAmt" ClientInstanceName="cDocAmt" ClientEnabled="false">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>

                <div class="col-md-2 lblmTop8">
                    <label>Exchange Rate</label>
                    <div>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="ExchRate" ClientInstanceName="cExchRate" ClientEnabled="false">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>

                <div class="col-md-2 lblmTop8">
                    <label>Amount In (Base Curr.)</label>
                    <div>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="BaseAmt" ClientInstanceName="cBaseAmt" ClientEnabled="false">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>

                <div class="col-md-2 lblmTop8">
                    <label>Remarks</label>
                    <div>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="Remarks" Width="100%" ClientInstanceName="cRemarks"></dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label>O/s Amount (Curr.)</label>
                    <div>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="OsAmt" ClientInstanceName="cOsAmt" ClientEnabled="false">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label>Adj. Amount (Curr.)</label>
                    <div>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="AdjAmt" ClientInstanceName="cAdjAmt"  >
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                <ClientSideEvents KeyUp="adjAmountLostFocus" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>

                <div class="clear"></div>
                <div class="clear"></div>
                <div class="clear"></div>



                <div class="col-md-12" style="top: 20px;">
                    <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid" Width="100%"
                        OnCellEditorInitialize="grid_CellEditorInitialize"
                         OnBatchUpdate="grid_BatchUpdate"
                         OnDataBinding="grid_DataBinding"
                        OnCustomCallback="grid_CustomCallback"
                         OnRowInserting="Grid_RowInserting"
                        OnRowUpdating="Grid_RowUpdating"
                        OnRowDeleting="Grid_RowDeleting"
                        OnCustomJSProperties="grid_CustomJSProperties"
                        KeyFieldName="SrlNo"
                        SettingsBehavior-AllowSort="false"
                        SettingsPager-Mode="ShowAllRecords" 
                        Settings-VerticalScrollBarMode="auto"
                    Settings-VerticalScrollableHeight="250" allowMultipleCallbacks="false"
                                                >
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4%" VisibleIndex="0" Caption=" " >
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="5%">
                                <PropertiesTextEdit>
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataButtonEditColumn Caption="Document No." FieldName="DocNo" VisibleIndex="2" Width="10%" ReadOnly="true">
                                <PropertiesButtonEdit>
                                    <Buttons>
                                        <dxe:EditButton Text="..." Width="20px" >
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="gridDocNobuttonClick" KeyDown="gridDocNoKeyDown" />
                                </PropertiesButtonEdit>
                            </dxe:GridViewDataButtonEditColumn>
                            <dxe:GridViewDataTextColumn Caption="Document Amount" HeaderStyle-HorizontalAlign="Right" VisibleIndex="3" FieldName="DocAmt" Width="8%" ReadOnly="true">
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                 <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Currency" FieldName="Currency" Width="8%" HeaderStyle-HorizontalAlign="left" ReadOnly="true">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Exchange Rate" FieldName="ExchangeRate" Width="8%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn FieldName="LocalAmt" Caption="Amount In Local Curr." VisibleIndex="6" Width="10%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="OsAmt" Caption="O/s Amount" VisibleIndex="7" Width="12%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                </PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Adjustment Amount" ReadOnly="false"  FieldName="AdjAmt" Width="8%" >
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    <ClientSideEvents LostFocus="gridAdjustAmtLostFocus" KeyUp="gridAdjustAmtLostFocus"/>
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Remaining Balance" FieldName="RemainingBalance" Width="8%" ReadOnly="true">
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="8%" VisibleIndex="9" Caption=" ">
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                        <Image Url="/assests/images/add.png">
                                        </Image>
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>

                               <dxe:GridViewDataTextColumn FieldName="DocumentId" Caption="DocumentId"  Width="0">
                               </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataTextColumn FieldName="DocumentType" Caption="DocumentType"  Width="0">
                               </dxe:GridViewDataTextColumn>

                        </Columns>
                         <ClientSideEvents  RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" CustomButtonClick="gridCustomButtonClick"
                             EndCallback="GridEndCallBack"/>
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
            <div class="clear"></div>
            <div class="row">
            <div class="col-md-12" style="top: 60px;left: 13px;">
                <table style="float: left;" id="tblBtnSavePanel">
                    <tr>
                        <td style="width: 100px;" id="tdSaveButton" runat="Server">
                            <% if (rights.CanAdd)
                                { %>
                            <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                            </dxe:ASPxButton>
                            <%} %>
                        </td>
                        <td style="width: 100px;" id="td_SaveButton" runat="Server">
                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>
                        

                    </tr>
                </table>
            </div>
            </div> 

            
        </div>


    </div>

    <!--Vendor Modal -->
    <div class="modal fade" id="VendModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Vendor Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Vendorkeydown(event)" id="txtVendSearch" autofocus width="100%" placeholder="Search By Vendor Name or Unique Id" />

                    <div id="VendorTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Vendor/Transporter Name</th>
                                <th>Unique Id</th>
                                <th>Type</th>
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


    <%--Advance Receipt Selection Model--%>
     <div class="modal fade" id="AdvanceModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Rate Difference Search</h4>
                </div>
                <div class="modal-body"> 
                    <input type="text" onkeydown="AdvanceNewkeydown(event)" id="txtAdvanceSearch" autofocus="autofocus" width="100%" placeholder="Search by Document Number" />
                    <div id="AdvPayDocTbl">
                         <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Rate Difference Number</th>
                                <th>Rate Difference Date</th>
                                <th>Amount</th>
                                <th>Balance</th>

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


     <%--Document Selection Model--%>
     <div class="modal fade" id="DocumentModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Document Search</h4>
                </div>
                <div class="modal-body"> 
                     <input type="text" onkeydown="gridDocumentNewkeydown(event)" id="txtGridDocSearch" autofocus="autofocus" width="100%" placeholder="Search by Document Number" />

                    <div id="DocNoDocTbl">
                         <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Document Number</th>
                                <th>Document Date</th>
                                <th>Document Type</th>
                                <th>Document Amount</th>
                                <th>Balance Amount</th>

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


    <%-- HiddenField Feild  --%>
    <asp:HiddenField ID="hdnVendorId" runat="server" />
    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField ID="hdAdvanceDocNo" runat="server" /> 
    <asp:HiddenField ID="hdAdjustmentId" runat="server" />
    <asp:HiddenField ID="hdAdjustmentType" runat="server" />
      <asp:HiddenField ID="HiddenSaveButton" runat="server" />


</asp:Content>
