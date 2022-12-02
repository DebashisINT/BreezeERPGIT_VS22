<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ServiceTempleateAdd.aspx.cs" Inherits="ERP.OMS.Management.Activities.ServiceTempleateAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js?v=0.02"></script>
    <link href="CSS/CustomerReceiptAdjustment.css" rel="stylesheet" />
    <script src="JS/ServiceTempleate.js?V=1.0"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <asp:Label ID="lblHeading" runat="server" Text="Service Template-Add"></asp:Label>
        </h3>
    </div>
    <div id="ApprovalCross" runat="server" class="crossBtn"><a href="ServiceTempleateList.aspx"><i class="fa fa-times"></i></a></div>

    <div class="form_main">
        <div class="row">
            <div class="col-md-12">
                <div class="row">

                    <div class="col-md-4 lblmTop8">
                        <label>Service Description</label>
                        <div>
                            <div>
                                <dxe:ASPxTextBox runat="server" ID="txtServiceDescription" ClientInstanceName="ctxtServiceDescription" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 lblmTop8">
                        <label>Service<span style="color: red">*</span></label>
                        <div>
                            <dxe:ASPxButtonEdit ID="txtServiceItemName" runat="server" ReadOnly="true" ClientInstanceName="ctxServiceItemName">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){ServiceItemButnClick();}" KeyDown="function(s,e){ServiceItemKeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                            <span id="MandatoryVendor" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>



                    <div class="col-md-2 lblmTop8">
                        <label>Quantity</label>
                        <div>
                            <div>
                                <dxe:ASPxTextBox runat="server" ID="txtQuantity" ClientInstanceName="ctxtQuantity">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-2 lblmTop8">
                        <label>Unit </label>
                        <div>
                            <asp:DropDownList ID="ddlBranch" runat="server" onchange="ddlBranch_SelectedIndexChanged()"
                                DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                            </asp:DropDownList>


                        </div>
                    </div>
                    <div style="clear: both"></div>
                    <div class="clear"></div>                   

                    <div class="row mTop5">
                        <div class="col-md-12 mTop5">
                            <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid"
                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                OnBatchUpdate="grid_BatchUpdate"
                                OnDataBinding="grid_DataBinding"
                                OnCustomCallback="grid_CustomCallback"
                                OnRowInserting="Grid_RowInserting"
                                OnRowUpdating="Grid_RowUpdating"
                                OnRowDeleting="Grid_RowDeleting"
                                OnCustomJSProperties="grid_CustomJSProperties"
                                KeyFieldName="ActualSL"
                                SettingsBehavior-AllowSort="false"
                                SettingsPager-Mode="ShowAllRecords"
                                Settings-VerticalScrollBarMode="auto"
                                Settings-VerticalScrollableHeight="250"
                                Width="100%">
                                <SettingsPager Visible="false"></SettingsPager>
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4%" VisibleIndex="0" Caption=" ">
                                        <CustomButtons>
                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
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
                                    <dxe:GridViewDataTextColumn FieldName="Discription" Caption="Description" VisibleIndex="3" Width="160px" ReadOnly="true">
                                        <CellStyle Wrap="True"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Qty" VisibleIndex="4" Width="92px" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="QuantityTextChange" />
                                        </PropertiesTextEdit>
                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="5" Width="100px"  ReadOnly="true">                                       
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="6" Width="60px" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="RateTextChange" />
                                        </PropertiesTextEdit>
                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn FieldName="Value" Caption="Amount" VisibleIndex="7" Width="60px" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                           <%-- <ClientSideEvents LostFocus="AmountTextChange" />--%>
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="8" Width="200px">
                                        <PropertiesTextEdit>
                                            <ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>
                                        </PropertiesTextEdit>
                                        <CellStyle></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="ProductID" Width="0">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn FieldName="ActualSL" Width="0">
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <ClientSideEvents RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" CustomButtonClick="gridCustomButtonClick"
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

                    <div style="clear: both"></div>
                    <div class="col-md-8">
                        <label style="margin-bottom: 5px; display: inline-block">Notes</label>
                        <div>
                            <dxe:ASPxMemo ID="txtNotes" ClientInstanceName="ctxtNotes" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>
                        </div>
                    </div>
                    <div class="clear"></div>


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
                                        CssClass="btn btn-primary" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <%--   <%} %>--%>
                                </span>
                                <span id="Span1" runat="server">
                                    <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it"
                                        CssClass="btn btn-primary" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                                    </dxe:ASPxButton>
                                </span>
                            </td>

                        </tr>
                    </table>
                </div>
            </div>


        </div>


    </div>

    <!--Vendor Modal -->
    <div class="modal fade" id="ServiceItemModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Service Item Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="ServiceItemkeydown(event)" id="txtServiceItemSearch" autofocus width="100%" placeholder="Search By Vendor Name or Unique Id" />

                    <div id="ServiceItemTable">
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


    <%-- HiddenField Feild  --%>
    <asp:HiddenField ID="hdnServiceProductId" runat="server" />
    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField ID="hdAdvanceDocNo" runat="server" />
    <asp:HiddenField ID="hdAdjustmentId" runat="server" />
    <asp:HiddenField ID="hdAdjustmentType" runat="server" />
    <asp:HiddenField ID="HiddenSaveButton" runat="server" />
    <asp:HiddenField ID="HiddenRowCount" runat="server" />
    <asp:HiddenField ID="hddnProjectId" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
</asp:Content>
