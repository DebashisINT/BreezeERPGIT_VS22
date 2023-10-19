<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      22-05-2023          0026195: Add Adjustment of Documents - Journal with Purchase Invoice module design modification & check in small device
2.0   Pallab    V2.0.39      27-07-2023          0026625: Add Adjustment of Documents - Journal with Purchase Invoice module all bootstrap modal outside click event disable
3.0   Priti     V2.0.40      19-10-2023          0026911:Party Invoice No and Party Invoice Date required in the Document Search Window of the Grid for Adjustment of Documents - Journal 
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="JournalCreditorsAdjustPInvoice.aspx.cs" Inherits="ERP.OMS.Management.Activities.JournalCreditorsAdjustPInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js?v=0.02"></script>
    <link href="CSS/CustomerReceiptAdjustment.css" rel="stylesheet" />
    <script src="JS/JournalCreditorsAdjustmentInvoice.js?v=2.4"></script>
    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_partyInvDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_partyInvDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_partyInvDt_B-1 #dt_partyInvDt_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 18px;
                bottom: 6px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 8px !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 34px;
            right: 13px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
    background: #42b39e !important;
}

        #ddlInventory
        {
                -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-2 > label, .col-md-2 > span, .col-md-1 > label, .col-md-1 > span
        {
            margin-top: 8px !important;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
        }

        #gridDEstination
        {
            width:99% !important;
        }

        #txtEntity , #txtCustName
        {
            width: 100%;
        }
        .col-md-6 span
        {
            margin-top: 8px !important;
        }

        .rds
        {
            margin-top: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , select
        {
            height: 30px !important;
            
        }
        select
        {
            background-color: transparent;
                padding: 0 20px 0 5px !important;
        }

        .newLbl
        {
            font-size: 14px;
            margin: 3px 0 !important;
            font-weight: 500 !important;
            margin-bottom: 0 !important;
            line-height: 20px;
        }

        .crossBtn {
            top: 25px !important;
            right: 25px !important;
        }

        .wrapHolder
        {
            height: 60px;
        }
        #rdl_SaleInvoice
        {
            margin-top: 12px;
        }

        .dxeRoot_PlasticBlue
        {
            width: 100% !important;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12
            {
                 padding-right: 10px;
                 padding-left: 10px;
            }
            .simple-select::after
            {
                    top: 34px;
                right: 8px;
            }
                .calendar-icon
            {
                    right: 14px;
                    bottom: 6px;
            }
        }

    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <asp:Label ID="lblHeading" runat="server" Text="Add Adjustment of Documents - Journal with Purchase Invoice"></asp:Label>
        </h3>
    </div>
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href="JournalCreditorsAdjustPInvoiceList.aspx"><i class="fa fa-times"></i></a></div>

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
                                <ClientSideEvents GotFocus="function(s,e){cdtTDate.ShowDropDown();}" DateChanged="cAdjDateChange"></ClientSideEvents>
                            </dxe:ASPxDateEdit>
                        </div>
                        <%--Rev 1.0--%>
                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                        <%--Rev end 1.0--%>
                    </div>

                    <%--Rev 1.0: "simple-select" class add --%>
                    <div class="col-md-2 lblmTop8 simple-select">
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
                                <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                            <span id="MandatoryVendor" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>


                    <div class="col-md-2 lblmTop8">
                        <label>Journal No.</label>
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
                                <dxe:ASPxTextBox runat="server" ID="AdjAmt" ClientInstanceName="cAdjAmt">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents KeyUp="adjAmountLostFocus" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="col-md-2 lblmTop8" id="Divproject" runat="server">
                        <label>Project</label>
                        <div>
                            <div>
                                <dxe:ASPxTextBox runat="server" ID="txtProject" ClientInstanceName="ctxtProject" ClientEnabled="false">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 lblmTop8" id="DivHierarchy" runat="server">
                        <label>Hierarchy</label>
                        <div>
                            <div>
                                <dxe:ASPxTextBox runat="server" ID="txtHierarchy" ClientInstanceName="ctxtHierarchy" ClientEnabled="false">
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
                            KeyFieldName="ActualSL"
                            SettingsBehavior-AllowSort="false"
                            SettingsPager-Mode="ShowAllRecords"
                            Settings-VerticalScrollBarMode="auto"
                            Settings-VerticalScrollableHeight="250">
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
                                <dxe:GridViewDataButtonEditColumn Caption="Document No." FieldName="DocNo" VisibleIndex="2" Width="10%" ReadOnly="true">
                                    <PropertiesButtonEdit>
                                        <Buttons>
                                            <dxe:EditButton Text="..." Width="20px">
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
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                        <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Adjustment Amount" ReadOnly="false" FieldName="AdjAmt" Width="8%">
                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                        <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        <ClientSideEvents LostFocus="gridAdjustAmtLostFocus" KeyUp="gridAdjustAmtLostFocus" />
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Remaining Balance" FieldName="RemainingBalance" Width="8%" ReadOnly="true">
                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
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

                                <dxe:GridViewDataTextColumn FieldName="DocumentId" Caption="DocumentId" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="DocumentType" Caption="DocumentType" Width="0">
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
            </div>
            <div class="clear"></div>
            <div class="row">
                <div class="col-md-12" style="top: 0px; left: 13px; margin-top: 40px;">
                    <table style="float: left;" id="tblBtnSavePanel">
                        <tr>
                            <td style="padding: 5px 0px;">
                                <span id="tdSaveButton" runat="server">
                                    <% if (rights.CanAdd)
                                       { %>
                                    <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" ClientVisible="false"
                                        CssClass="btn btn-success" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <%} %>
                                </span>
                                <span id="Span1" runat="server">
                                    <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" ClientVisible="false"
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
    </div>
    <!--Vendor Modal -->
    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="VendModel" role="dialog">--%>
    <div class="modal fade" id="VendModel" role="dialog" data-backdrop="static" data-keyboard="false">
        <%--Rev end 2.0--%>
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
    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="AdvanceModel" role="dialog">--%>
    <div class="modal fade" id="AdvanceModel" role="dialog" data-backdrop="static" data-keyboard="false">
        <%--Rev end 2.0--%>
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Journal Creditors Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="AdvanceNewkeydown(event)" id="txtAdvanceSearch" autofocus="autofocus" width="100%" placeholder="Search by Document Number" />

                    <div id="AdvRecDocTbl">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Journal Number</th>
                                <th>Journal Date</th>
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
    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="DocumentModel" role="dialog">--%>
    <div class="modal fade" id="DocumentModel" role="dialog" data-backdrop="static" data-keyboard="false">
        <%--Rev end 2.0--%>
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
                                <%--Rev 3.0--%>
                                <th>Party Invoice No</th>
                                <th>Party Invoice Date</th>
                              <%--  Rev 3.0 End--%>

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
    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField ID="hdAdvanceDocNo" runat="server" />
    <asp:HiddenField ID="hdAdjustmentId" runat="server" />
    <asp:HiddenField ID="hdAdjustmentType" runat="server" />
    <asp:HiddenField ID="HiddenSaveButton" runat="server" />
    <asp:HiddenField ID="HiddenRowCount" runat="server" />
    <asp:HiddenField ID="hddnProjectId" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
</asp:Content>
