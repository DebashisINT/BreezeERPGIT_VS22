<%--=======================================================Revision History=====================================================    
    1.0   Pallab    V2.0.38   10-05-2023      26078: Add Stock Journal (Stock Transfer) module design modification & check in small device
=========================================================End Revision History===================================================--%>

<%@ Page Title="Stock Journal Transfer" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="Stock-JournalTransfer.aspx.cs" Inherits="ERP.OMS.Management.Activities.Stock_JournalTransfer" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>

    <style type="text/css">

    </style>
    <script src="JS/Stock-JournalTransfer.js?V=1.0"></script>
    
    <link href="CSS/Stock-JournalTransfer.css" rel="stylesheet" />
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
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate , #dt_refCreditNoteDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dtPostingDate_B-1 ,
        #dt_refCreditNoteDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dtPostingDate_B-1 #dtPostingDate_B-1Img ,
        #dt_refCreditNoteDt_B-1 #dt_refCreditNoteDt_B-1Img
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
            margin-top: 0 !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 26px;
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
            margin-top: 4px;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
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
                top: 26px;
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
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">

            <h3 class="pull-left">
                <asp:Label ID="lblHeading" runat="server"></asp:Label>
            </h3>

            <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
            <div id="divcross" runat="server" class="crossBtn"><a href="Stock-journalTransferList.aspx"><i class="fa fa-times"></i></a></div>

        </div>
    </div>


        <div class="form_main row">

        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
            <TabPages>
                <dxe:TabPage Name="General" Text="General">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <div style=" padding: 8px 0; margin-bottom: 0px; border-radius: 4px;" class="clearfix col-md-12">
                                <div class="col-md-2 lblmTop8" style="display: none;">
                                    <dxe:ASPxLabel ID="lbl_Inventory" runat="server" Text="Inventory Item?">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddlInventory" CssClass="backSelect" runat="server" Width="100%">
                                        <asp:ListItem Text="Both" Value="B" />
                                        <asp:ListItem Text="Inventory Item" Value="Y" />
                                        <asp:ListItem Text="Non Inventory Item" Value="N" />
                                        <asp:ListItem Text="Capital Goods" Value="C" />
                                    </asp:DropDownList>
                                </div>
                                <%--Rev 1.0: "simple-select" class add --%>
                                <div class="col-md-2 lblmTop8 simple-select" runat="server" id="divNumberingScheme">
                                    <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>

                                    <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" 
                                        DataTextField="SchemaName" DataValueField="ID" onchange="CmbScheme_ValueChange();">
                                    </asp:DropDownList>

                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No.">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" Enabled="false">
                                    </asp:TextBox>
                                    <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>

                                    <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLQuoteDate" Width="100%" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                    <%--Rev 1.0--%>
                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                    <%--Rev end 1.0--%>
                                </div>
                                <%--Rev 1.0: "simple-select" class add --%>
                                <div class="col-md-2 lblmTop8 simple-select">
                                    <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="For Unit ">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddl_Branch"  runat="server" Width="100%" DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Enabled="true">
                                    </asp:DropDownList>
                                </div>

                                  <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                                                </dxe:ASPxLabel>
                                                <%-- <label id="lblProject" runat="server">Project</label>--%>
                                                <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataStock"
                                                    KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                    </Columns>
                                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                        <Templates>
                                                            <StatusBar>
                                                                <table class="OptionsTable" style="float: right">
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </StatusBar>
                                                        </Templates>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                    </GridViewProperties>
                                                    <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />
                                                  
                                                </dxe:ASPxGridLookup>
                                                <dx:LinqServerModeDataSource ID="EntityServerModeDataStock" runat="server" OnSelecting="EntityServerModeDataStock_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                            </div>
                                   <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                </asp:DropDownList>
                                            </div>

                                <div style="clear: both"></div>
                                <div class="col-md-12">
                                    <label>Narration: </label>
                                    <div>
                                        <dxe:ASPxMemo ID="txtnarration" runat="server" Height="60px" MaxLength="500" TabIndex="5" TextMode="MultiLine" Width="100%"></dxe:ASPxMemo>
                                    </div>
                                </div>


                                <%-- <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_bramchto" runat="server" Text="To Branch ">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddl_to_branch" runat="server" Width="100%" DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID">
                                    </asp:DropDownList>
                                </div>--%>
                            </div>
                            <br />
                            <div class="col-md-12 clearfix">
                                <div class="row">

                                    <dxe:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="SrlNo" TabIndex="6"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="grid_CustomCallback"
                                        Settings-ShowFooter="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="50">
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" ReadOnly="true" Width="180" VisibleIndex="2">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="150" ReadOnly="true">
                                                <PropertiesButtonEdit>
                                                    <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                                    <ClientSideEvents />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                </PropertiesButtonEdit>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="ProductDiscription" Caption="Description" VisibleIndex="4" Width="18%" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="ProductClass" Caption="Product Class" VisibleIndex="5" Width="10%" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Brand" Caption="Brand" VisibleIndex="6" Width="10%" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="8" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit>
                                                    <ClientSideEvents LostFocus="PurchasePriceTextFocus" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="PurchaseUOM" Caption="UOM" VisibleIndex="7" Width="4%" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="PurchasePrice" Caption="Price" VisibleIndex="9" Width="7%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00">
                                                    <ClientSideEvents LostFocus="PurchasePriceTextFocus" KeyDown="AddBatchNew" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Amount" VisibleIndex="10" Width="0" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00">
                                                    <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"></MaskSettings>
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <PropertiesTextEdit>
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="NetAmount" Caption="Net Amount" VisibleIndex="12" Width="0" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00">
                                                    <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"></MaskSettings>
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="ProductID" VisibleIndex="20" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents />
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <ClientSideEvents BatchEditStartEditing="gridFocusedRowChanged" EndCallback="cSelectPanelEndCall" />
                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                        </SettingsEditing>
                                    </dxe:ASPxGridView>

                                </div>
                            </div>
                            <div style="clear: both;">
                                <br />
                                <div style="display: none;">
                                    <dxe:ASPxLabel ID="txt_Charges" runat="server" Text="0.00" ClientInstanceName="ctxt_Charges" />
                                    <dxe:ASPxLabel ID="txt_cInvValue" runat="server" Text="0.00" ClientInstanceName="cInvValue" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                    <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-success" UseSubmitBehavior="false" TabIndex="7">
                                        <ClientSideEvents Click="function(s, e) {SaveNew_Click();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btn_SaveRecordsExit" ClientInstanceName="cbtn_SaveRecordsExit" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" UseSubmitBehavior="false" TabIndex="8">
                                        <ClientSideEvents Click="function(s, e) {SaveExit_Click();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btn_stocktransfer" ClientInstanceName="btn_stocktransfer" runat="server" AutoPostBack="false" Text="Stock D&#818;etails" CssClass="btn btn-primary" UseSubmitBehavior="false" TabIndex="9">
                                        <ClientSideEvents Click="function(s, e) {Openwarehousepopup();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btn_stocktransferViemode" ClientInstanceName="btn_stocktransferViemode" runat="server" AutoPostBack="false" Text="V&#818;iew Stock Details" CssClass="btn btn-primary" UseSubmitBehavior="false" TabIndex="10">
                                        <ClientSideEvents Click="function(s, e) {Openwarehousepopup_view();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>

                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>

            </TabPages>

        </dxe:ASPxPageControl>

    </div>
    </div>
    <div>

        <asp:HiddenField runat="server" ID="hdnBranchID" />
        <asp:HiddenField runat="server" ID="hdnrmvetobranch" />
        <asp:HiddenField runat="server" ID="hdnrmvetobranchtext" />

    </div>


    <!--Product Modal -->
    <div class="modal fade" id="ProductModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name" />

                    <div id="ProductTable">
                        <table border='1' style="width: 100%" class="dynamicPopupTbl">

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
    <!--Product Modal -->


    <!--Warehouse Serial Modal -->
    <div class="modal fade" id="stockModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Warehouse </h4>
                </div>
                <div class="modal-body">

                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <table>
                                    <tr>
                                        <td><span>Transfer To Warehouse :&nbsp&nbsp&nbsp&nbsp  </span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlwarehouse" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <div id="gridWarehouseTable">

                                    <div id="dvWHSL" runat="server">
                                        <table>
                                            <tr>
                                                <td><span>Stk-Out Qty : &nbsp&nbsp&nbsp&nbsp </span></td>
                                                <td>
                                                    <input type="text" runat="server" id="txtqtyWHSL" readonly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                        <dxe:ASPxGridView ID="gridwarehouse" ClientInstanceName="gridwarehouse" runat="server" KeyFieldName="SerialID"
                                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Visible"
                                            Settings-ShowFooter="false">


                                            <Columns>

                                                <dxe:GridViewCommandColumn ShowSelectCheckbox="true" Caption=" " Width="3" />

                                                <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName" ReadOnly="true" VisibleIndex="1">
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn Caption="Serial No" FieldName="SerialNo" ReadOnly="true" VisibleIndex="2">
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn FieldName="SerialID" CellStyle-CssClass="hide" HeaderStyle-CssClass="hide" Caption="SerialID" VisibleIndex="4" ReadOnly="True" Width="0">
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="Inventorytype" CellStyle-CssClass="hide" HeaderStyle-CssClass="hide" Caption="Inventorytype" VisibleIndex="5" ReadOnly="True" Width="0">
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="WarehouseID" CellStyle-CssClass="hide" HeaderStyle-CssClass="hide" Caption="WarehouseID" VisibleIndex="7" ReadOnly="True" Width="0">
                                                </dxe:GridViewDataTextColumn>


                                            </Columns>

                                            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                            <ClientSideEvents SelectionChanged="Warehouseselectedcount" />
                                        </dxe:ASPxGridView>
                                    </div>

                                    <div id="dvWH" runat="server">
                                        <div class="col-md-3" style="padding-left: 0; width: 118px;"><span>Stk-Out Qty:  </span></div>
                                        <div class="col-md-3" style="width: 183px">
                                            <input type="text" runat="server" id="txtqty" style="width: 100%;" onkeypress='return event.charCode >= 48 && event.charCode <= 57' oncopy="return false" onpaste="return false" />
                                            <input type="hidden" runat="server" id="hdntotWHquantity" style="width: 100%;" />
                                        </div>

                                        <dxe:ASPxGridView ID="gridwarehousewithoutserial" ClientInstanceName="gridwarehousewithoutserial" runat="server" KeyFieldName="WarehouseID"
                                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                            Settings-ShowFooter="false">


                                            <Columns>


                                                <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName" ReadOnly="true" VisibleIndex="1">
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" ReadOnly="true" VisibleIndex="2">
                                                    <PropertiesTextEdit>
                                                        <ClientSideEvents />
                                                        <MaskSettings Mask="&lt;0..999999999&gt;" AllowMouseWheel="false" />
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <Style HorizontalAlign="Right"></Style>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="Inventorytype" CellStyle-CssClass="hide" HeaderStyle-CssClass="hide" Caption="Inventorytype" VisibleIndex="3" ReadOnly="True" Width="0px">
                                                </dxe:GridViewDataTextColumn>



                                                <dxe:GridViewDataTextColumn FieldName="WarehouseID" CellStyle-CssClass="hide" HeaderStyle-CssClass="hide" Caption="WarehouseID" VisibleIndex="7" ReadOnly="True" Width="0px">
                                                </dxe:GridViewDataTextColumn>

                                            </Columns>


                                        </dxe:ASPxGridView>

                                    </div>
                                </div>



                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="Onpanelendcallback" />
                    </dxe:ASPxCallbackPanel>
                </div>
                <div class="modal-footer">
                    <%--  <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
                    <dxe:ASPxButton ID="btn_ok" ClientInstanceName="btn_ok" runat="server" AutoPostBack="false" Text="OK" UseSubmitBehavior="false" CssClass="btn btn-primary">
                        <ClientSideEvents Click="function(s, e) {Closewarehousepopup();}" />
                    </dxe:ASPxButton>
                </div>
            </div>
        </div>
    </div>
    <!--Warehouse Serial Modal -->
    <style>
        #gridstockjournalout th, #gridstockjournalin th {
            background: #4e64a6;
            color: #fff;
            padding: 5px 8px;
        }
    </style>



    <!--Warehouse Serial View Modal -->
    <div class="modal fade" id="stockModelview" role="dialog">
        <div class="modal-dialog" style="width: 650px;">
            <!-- Modal content-->
            <div class="modal-content">


                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Warehouse </h4>
                </div>

                <div class="modal-body">


                    <div id="div_sockout" class="col-md-6">

                        <h4>Stock-Out</h4>
                        <label>Warehouse</label><input type="text" runat="server" id="txtwarehouseJO" style="width: 180px" readonly />
                        <label>Quantity</label>
                        <input type="text" runat="server" id="txtqtyJO" style="width: 130px" readonly />

                        <asp:GridView ID="gridstockjournalout" runat="server" AutoGenerateColumns="false" Width="100%">

                            <Columns>
                                <asp:BoundField DataField="SerialNo" HeaderText="Serial" />
                                <asp:BoundField DataField="OUT_Quantity" HeaderText="Quantity" DataFormatString="{0:0}" />
                            </Columns>

                        </asp:GridView>


                    </div>



                    <div id="div_sockin" class="col-md-6">

                        <h4>Stock-In</h4>

                        <label>Warehouse</label><input type="text" runat="server" id="txtwarehouseJI" style="width: 180px" readonly />
                        <label>Quantity</label><input type="text" runat="server" id="txtqtyJI" style="width: 130px" readonly />
                        <asp:GridView ID="gridstockjournalin" runat="server" AutoGenerateColumns="false" Width="100%">

                            <Columns>
                                <asp:BoundField DataField="SerialNo" HeaderText="Serial" />
                                <asp:BoundField DataField="IN_Quantity" HeaderText="Quantity" DataFormatString="{0:0}" />
                            </Columns>

                        </asp:GridView>

                    </div>
                    <div style="clear: both"></div>
                </div>

                <div class="modal-footer">

                    <dxe:ASPxButton ID="btn_closeview" ClientInstanceName="btn_closeview" runat="server" AutoPostBack="false" Text="Close" UseSubmitBehavior="false" CssClass="btn btn-primary">
                        <ClientSideEvents Click="function(s, e) {Closewarehousepopup_view();}" />
                    </dxe:ASPxButton>

                </div>


            </div>
        </div>
    </div>
    <!--Warehouse Serial View Modal -->




    <input type="hidden" runat="server" id="hdninventorytype" />
    <input type="hidden" runat="server" id="hdnquantity" />
    <input type="hidden" runat="server" id="hddnwarehouseqty" />
    <input type="hidden" runat="server" id="hddnwarehousetyoe" />
    <input type="hidden" runat="server" id="hdnUOM" />
    <input type="hidden" runat="server" id="hddnproductcode" />

    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
    <asp:HiddenField ID="hdnHierarchySelectInEntryModule" runat="server" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />

</asp:Content>
