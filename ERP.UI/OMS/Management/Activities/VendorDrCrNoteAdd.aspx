<%@ Page Title="Vendor Credit/Debit Note" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="VendorDrCrNoteAdd.aspx.cs" Inherits="ERP.OMS.Management.Activities.VendorDrCrNoteAdd" %>

<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>

<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
   <%-- <script src="JS/SearchPopup.js"></script>--%>
     <script src="JS/SearchPopupDatatable.js"></script>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>
    <script src="JS/VendorDrCrNoteAdd.js?v=1.0.0120"></script>
    <script>
        function DateChange() {

            var Ctype = $('#hdnMode').val();
            if (Ctype != 1) {
                var SelectedDate = new Date(tDate.GetDate());
                var monthnumber = SelectedDate.getMonth();
                var monthday = SelectedDate.getDate();
                var year = SelectedDate.getYear();
                var SelectedDateValue = new Date(year, monthnumber, monthday);
                var MaxLockDate = new Date('<%=Session["LCKJV"]%>');
                monthnumber = MaxLockDate.getMonth();
                monthday = MaxLockDate.getDate();
                year = MaxLockDate.getYear();
                var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();
                if (SelectedDateValue <= MaxLockDateNumeric) {
                    jAlert('This Entry Date has been Locked.');
                    MaxLockDate.setDate(MaxLockDate.getDate() + 1);
                    tDate.SetDate(MaxLockDate);
                    return;
                }
            }

            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var SelectedDate = new Date(tDate.GetDate());
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();
            var SelectedDateValue = new Date(year, monthnumber, monthday);
            monthnumber = FinYearStartDate.getMonth();
            monthday = FinYearStartDate.getDate();
            year = FinYearStartDate.getYear();
            var FinYearStartDateValue = new Date(year, monthnumber, monthday);
            monthnumber = FinYearEndDate.getMonth();
            monthday = FinYearEndDate.getDate();
            year = FinYearEndDate.getYear();
            var FinYearEndDateValue = new Date(year, monthnumber, monthday);
            var SelectedDateNumericValue = SelectedDateValue.getTime();
            var FinYearStartDateNumericValue = FinYearStartDateValue.getTime();
            var FinYearEndDatNumbericValue = FinYearEndDateValue.getTime();
            if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {
                if (grid.GetVisibleRowsOnPage() == 1) {
                }
            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    tDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    tDate.SetDate(new Date(FinYearEndDate));
                }
            }
        }
    </script>
    <%-- <script src="JS/VendorDrCrNoteAdd.js?1"></script>--%>
    <link href="CSS/VendorDrCrNoteAdd.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="clearfix pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Vendor Credit/Debit Note Add"></asp:Label>
            </h3>
            <div id="pageheaderContent" class="pull-right reverse wrapHolder content horizontal-images">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder" id="divGSTIN" style="display: none;" runat="server">
                                <table>
                                    <tr>
                                        <td>GST Registed?</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblGSTIN" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="btncross" runat="server" class="crossBtn" style="margin-left: 50px;"><a href="javascript:void(0);" onclick="ReloadPage()"><i class="fa fa-times"></i></a></div>

        </div>
    </div>
    <div class="form_main">
        <div id="divAddNew" class="clearfix ">
            <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                <TabPages>
                    <dxe:TabPage Name="General" Text="General">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                                    <div class="col-md-3">
                                        <label>Note Type <span style="color: red">*</span></label>
                                        <div>
                                            <asp:DropDownList ID="ddlNoteType" runat="server" Width="100%" onchange="ddlNoteType_ValueChange()">
                                                <asp:ListItem Text="Credit Note" Value="Cr" />
                                                <asp:ListItem Text="Debit Note" Value="Dr" />
                                            </asp:DropDownList>
                                            <%-- <dxe:ASPxButtonEdit ID="ddlNoteType" runat="server" ReadOnly="true" ClientInstanceName="cddlNoteType" Width="100%">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){NoteTypeClick();}" KeyDown="function(s,e){NoteTypeKeyDown(s,e);}" />
                                            </dxe:ASPxButtonEdit>--%>
                                        </div>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="row">
                                            <div class="col-md-3" id="div_Edit" runat="server">
                                                <label>Select Numbering Scheme <span style="color: red">*</span></label>
                                                <div>
                                                    <%--<asp:DropDownList ID="CmbScheme" runat="server" Width="100%" onchange="CmbScheme_ValueChange()">
                                                    </asp:DropDownList>--%>
                                                    <dxe:ASPxComboBox ID="CmbScheme" runat="server" ClientInstanceName="cCmbScheme"
                                                        Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="CmbScheme_ValueChange" />
                                                    </dxe:ASPxComboBox>



                                                    <%-- <dxe:ASPxButtonEdit ID="CmbScheme" runat="server" ReadOnly="true" ClientInstanceName="cCmbScheme" Width="100%">
                                                        <Buttons>
                                                            <dxe:EditButton>
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                        <ClientSideEvents ButtonClick="function(s,e){NumberingSchemeClick();}" KeyDown="function(s,e){NumberingSchemeKeyDown(s,e);}" />
                                                    </dxe:ASPxButtonEdit>--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Document No.<span style="color: red">*</span></label>
                                                <div>
                                                    <asp:TextBox ID="txtBillNo" runat="server" Width="95%" meta:resourcekey="txtBillNoResource1" MaxLength="30" onchange="txtBillNo_TextChanged()">
                                                    </asp:TextBox>
                                                    <span id="MandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                    <span id="duplicateMandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Duplicate Journal No."></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label style="">Posting Date<span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="tDate" UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                                                        <ClientSideEvents DateChanged="function(s,e){DateChange()}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}" />
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Unit <span style="color: red">*</span></label>
                                                <div>
                                                    <asp:DropDownList ID="ddlBranch" runat="server" DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%"
                                                        meta:resourcekey="ddlBranchResource1" onchange="ddlBranch_ChangeIndex()">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                    <div class="col-md-3">
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                                        </dxe:ASPxButtonEdit>

                                        <span id="MandatorysCustomer" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -3px; top: 24px;"
                                            title="Mandatory"></span>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="row">
                                            <div class="col-md-3" id="div_InvoiceNo" style="display: none" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Document Number">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txtPartyInvoice" ClientInstanceName="ctxtPartyInvoice" runat="server" Width="100%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div class="col-md-3" id="div_InvoiceDate" style="display: none" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Document Date">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxDateEdit ID="dt_PartyDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLPartyDate" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <ClientSideEvents GotFocus="function(s,e){cPLPartyDate.ShowDropDown();}" />
                                                </dxe:ASPxDateEdit>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="row">
                                                    <div class="col-md-6 lblmTop8">
                                                        <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                                        </dxe:ASPxLabel>
                                                        <asp:DropDownList ID="ddl_Currency" runat="server" Width="95%">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-6 lblmTop8">
                                                        <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                                        </dxe:ASPxLabel>
                                                        <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server" Width="90%">
                                                            <ValidationSettings RequiredField-IsRequired="false" Display="None">
                                                            </ValidationSettings>
                                                            <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3" id="dvProject" runat="server">

                                                <div style="margin-top: 5px;">
                                                    <dxe:ASPxLabel ID="dxelblProject" ClientInstanceName="cdxelblProject" runat="server" Text="Project">
                                                    </dxe:ASPxLabel>
                                                    <a href="#" style="left: -12px; top: 20px;"><%--onclick="AddcustomerClick()"--%>

                                                        <i id="I1" runat="server" class="fa fa-trash" aria-hidden="true" onclick="DeleteProjectCode()"></i></a>

                                                </div>
                                                <div>
                                                    <dxe:ASPxButtonEdit ID="txtProject" runat="server" ReadOnly="true" ClientInstanceName="ctxtProject" Width="100%">
                                                        <Buttons>
                                                            <dxe:EditButton>
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                        <ClientSideEvents ButtonClick="function(s,e){ProjectButnClick();}" KeyDown="function(s,e){ProjectKeyDown(s,e);}" />
                                                    </dxe:ASPxButtonEdit>
                                                </div>
                                                <%--<label id="lblProject" runat="server">Project</label>
                                                <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataCustDbCr"
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
                                                    <ClientSideEvents GotFocus="Project_gotFocus" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />

                                                    <ClearButton DisplayMode="Always">
                                                    </ClearButton>
                                                </dxe:ASPxGridLookup>
                                                <dx:LinqServerModeDataSource ID="EntityServerModeDataCustDbCr" runat="server" OnSelecting="EntityServerModeDataCustDbCr_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />--%>
                                            </div>




                                        </div>
                                    </div>
                                    <div style="clear: both;">
                                    </div>

                                    <div class="col-md-9">
                                        <div class="row">

                                            <div class="col-md-4">
                                                <div>
                                                    <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                    </dxe:ASPxLabel>
                                                </div>
                                                <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-md-3">
                                                <div>
                                                    <dxe:ASPxLabel ID="lblInvoiceNo" ClientInstanceName="clblInvoiceNo" runat="server" Text="Ref. Purchase Invoice No.">
                                                    </dxe:ASPxLabel>
                                                </div>
                                                <dxe:ASPxButtonEdit ID="txtPurchaseInvoiceNo" runat="server" ReadOnly="true" ClientInstanceName="ctxtPurchaseInvoiceNo" Width="100%">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){PurInvoiceNoButnClick();}" KeyDown="function(s,e){PurInvoiceNoKeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>


                                                <%--    <dxe:ASPxComboBox ID="ddlInvoice" runat="server" ClientInstanceName="cddlInvoice" OnCallback="ddlInvoice_Callback"
                                                    SelectedIndex="0" DropDownWidth="800" ValueType="System.String"
                                                    Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True"
                                                    ValueField="InvoiceID" IncrementalFilteringMode="Contains" CallbackPageSize="30" TextFormatString="{1}" ItemStyle-Wrap="True">
                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="InvoiceNumber" Caption="Invoice Number" Width="45" />
                                                        <dxe:ListBoxColumn FieldName="PartyInvoiceNo" Caption="Party Invoice No." Width="45" />
                                                        <dxe:ListBoxColumn FieldName="PartyInvoiceDate" Caption="Party Invoice Date" Width="45" />
                                                    </Columns>
                                                    <ClientSideEvents EndCallback="ddlInvoice_EndCallback" />
                                                </dxe:ASPxComboBox>--%>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                    </div>
                                    <div class="col-md-3">
                                    </div>
                                </div>
                                <div class="clearfix">
                                    <br />
                                    <div class="makeFullscreen ">
                                        <span class="fullScreenTitle">Vendor Credit/Debit Note Add</span>
                                        <span class="makeFullscreen-icon half hovered " data-instance="InsgridBatch" title="Maximize Grid" id="expandgrid">
                                            <i class="fa fa-expand"></i>
                                        </span>
                                        <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="CashReportID" ClientInstanceName="grid"
                                            ID="grid" Width="100%" OnCellEditorInitialize="grid_CellEditorInitialize" SettingsBehavior-AllowSort="false"
                                            SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback"
                                            OnDataBinding="grid_DataBinding" OnRowInserting="Grid_RowInserting"
                                            OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting" SettingsPager-Mode="ShowAllRecords"
                                            Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="150">
                                            <SettingsPager Visible="false">
                                            </SettingsPager>
                                            <Columns>
                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="50" VisibleIndex="0" Caption="Action">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>
                                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="5%">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataButtonEditColumn FieldName="MainAccount" Caption="Main Account" VisibleIndex="2" Width="300">
                                                    <PropertiesButtonEdit>
                                                        <ClientSideEvents ButtonClick="MainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>
                                                <dxe:GridViewDataButtonEditColumn FieldName="bthSubAccount" Caption="Sub Account" VisibleIndex="3" Width="300">
                                                    <PropertiesButtonEdit>
                                                        <ClientSideEvents ButtonClick="SubAccountButnClick" KeyDown="SubAccountKeyDown" />
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="WithDrawl" Caption="Amount" Width="180" EditCellStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <ClientSideEvents KeyDown="OnKeyDown" LostFocus="DebitLostFocus" GotFocus="function(s,e){
                        						                                    DebitGotFocus(s,e);
                        						                                    }" />
                                                        <ClientSideEvents />
                                                        <ValidationSettings Display="None">
                                                        </ValidationSettings>
                                                    </PropertiesTextEdit>
                                                    <CellStyle Wrap="False" HorizontalAlign="Right">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="5" Width="10%" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesButtonEdit Style-HorizontalAlign="Right">
                                                        <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                        <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    </PropertiesButtonEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataButtonEditColumn>
                                                <dxe:GridViewDataTextColumn FieldName="NetAmount" Caption="Net Amount" VisibleIndex="6" Width="12%" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </PropertiesTextEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Narration" Caption="Narration" Width="12%">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                    <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4.5%" VisibleIndex="8" Caption=" ">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                            <Image Url="/assests/images/add.png">
                                                            </Image>
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>

                                                <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Srl No" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="gvColMainAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="gvColSubAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="IsSubledger" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="TAXable" Caption="TAXable" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                            <TotalSummary>
                                                <dxe:ASPxSummaryItem SummaryType="Sum" FieldName="C2" Tag="C2_Sum" />
                                            </TotalSummary>
                                            <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" BatchEditStartEditing="OnBatchEditStartEditing"
                                                CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" />
                                            <SettingsDataSecurity AllowEdit="true" />
                                            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                            </SettingsEditing>
                                        </dxe:ASPxGridView>
                                    </div>
                                </div>
                                <div class="text-center">
                                    <table style="float: right; margin-top: 5px; margin-bottom: 5px">
                                        <tr>

                                            <td style="padding-right: 16px">Total Amount</td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txt_Debit" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clearfix" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc; margin-top: 31px;">
                                    <div class="col-md-12">
                                        <label>Main Narration</label>
                                        <div>
                                            <asp:TextBox ID="txtNarration" Font-Names="Arial" runat="server" TextMode="MultiLine" Width="100%" meta:resourcekey="txtNarrationResource1" Height="40px">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div>
                                    <%--<b><span id="tagged" runat="server" style="display: none; color: red">This Vendor Debit/Credit Note is tagged with Document : <span id="spanTaggedDocNo" runat="server"></span>. Cannot Modify data!!</span></b>--%>
                                    <b><span id="tagged" runat="server" style="display: none; color: red">This Vendor Debit/Credit Note is tagged with another module,  <span id="spanTaggedDocNo" runat="server"></span>Cannot Modify data!!</span></b>
                                    <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btnUDF" ClientInstanceName="cbtnUDF" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                    </dxe:ASPxButton>
                                </div>
                                <div id="loadCurrencyMassage" style="display: none;">
                                    <label>
                                        <span style="color: red; font-weight: bold; font-size: medium;">**  Mismatch detected in Total of Debit & Credit Amount.</span>
                                    </label>
                                </div>
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    <dxe:TabPage Name="Billing/Shipping" Text="Our Billing/Shipping">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <ucBS:BillingShippingControl runat="server" ID="BillingShippingControl" />
                                <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="VN" />
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
        </div>
        <div>
            <asp:HiddenField ID="hdnSegmentid" runat="server" />
            <asp:HiddenField ID="hdnMode" runat="server" />
            <asp:HiddenField ID="hdnSchemaType" runat="server" />
            <asp:HiddenField ID="hdnSchemaID" runat="server" />
            <asp:HiddenField ID="hdnRefreshType" runat="server" />
            <asp:HiddenField ID="hdnNotelNo" runat="server" />
            <asp:HiddenField ID="hdfIsDelete" runat="server" />
            <asp:HiddenField ID="hdnPageStatus" runat="server" />
            <%-- <asp:HiddenField ID="hdnNoteType" runat="server" />--%>
            <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
            <asp:HiddenField ID="hdnMainAccountId" runat="server" />

            <asp:HiddenField ID="TaxAmountOngrid" runat="server" />
            <asp:HiddenField ID="VisibleIndexForTax" runat="server" />

            <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
            <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
            <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
            <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />

            <asp:HiddenField runat="server" ID="IsUdfpresent" />
            <asp:HiddenField runat="server" ID="Keyval_internalId" />
            <asp:HiddenField runat="server" ID="hdnPurchaseInvoiceID" />
            <asp:HiddenField runat="server" ID="hdnProjectId" />
            <asp:HiddenField runat="server" ID="hdnProjectSelectInEntryModule" />
            <asp:HiddenField runat="server" ID="hdnCustomerId" />
        </div>

        <!--Vendor Modal -->
        <div class="modal fade" id="CustModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Vendor Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search by Vendor Name or Unique Id" />
                        <div id="CustomerTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Vendor Name</th>
                                    <th>Unique ID</th>
                                    <th>GSTIN</th>
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
        <!--Vendor Modal -->

        <!--Vendor Modal -->
        <div class="modal fade" id="PurchaseInvoiceModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Ref. Purchase Invoice No.</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="PurchaseInvoicekeydown(event)" id="txtPurchaseInvoiceSearch" autofocus width="100%" placeholder="Search by Vendor Name or Unique Id" />
                        <div id="PurchaseInvoiceTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Vendor Name</th>
                                    <th>Unique ID</th>

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
        <!--Vendor Modal -->


        <div class="modal fade" id="ProjectModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Project List</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="ProjectCodekeydown(event)" id="txtProjectSearch" autofocus width="100%" placeholder="Search by Project Name" />
                        <div id="ProjectTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">Proj_Id</th>
                                    <th>Project Code</th>
                                    <th>Project Name</th>
                                    <th>Customer</th>
                                    <th>Hierarchy</th>

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


        <!--Type Modal -->
        <%--<div class="modal fade" id="TypeModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Type Search</h4>
                    </div>
                    <div class="modal-body">
                        <div id="TypeTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <td class="hide">id</td>
                                    <td>Type</td>
                                </tr>
                                <tr onkeydown="crKeyDown(event)" id="crRow" onclick="CrNoteClick(event)" onblur="searchElementlostFocus(event)" onfocus="searchElementGetFocus(event)">
                                    <td class="hide">Cr</td>
                                    <td>
                                        <input type="text" value="Credit Note" id="txtcrKey" readonly onblur="searchElementlostFocus(event)" onfocus="searchElementGetFocus(event)" /></td>
                                </tr>
                                <tr onkeydown="drKeyDown(event)" id="drRow" onclick="DrNoteClick(event)">
                                    <td class="hide">Dr</td>
                                    <td>
                                        <input type="text" value="Debit Note" id="txtdrKey" readonly onblur="searchElementlostFocus(event)" onfocus="searchElementGetFocus(event)" /></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>--%>
        <!--TYpe Modal -->

        <%-- -------------------ModelPOPUPControl   FOR Main & Sub Account-------------------------------------%>
        <div class="modal fade" id="MainAccountModel" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <!-- Modal MainAccount-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
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
                                    <th>HSN/SAC</th>
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
        <div class="modal fade" id="SubAccountModel" role="dialog" data-backdrop="static"
            data-keyboard="false">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Sub Account Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="SubAccountNewkeydown(event)" id="txtSubAccountSearch" autofocus width="100%" placeholder="Search By Sub Account Name or Code" />

                        <div id="SubAccountTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th>Sub Account Name [Unique ID]</th>
                                    <th>Sub Account Type</th>
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
                                    <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                    <SettingsPager Visible="false"></SettingsPager>

                                    <Columns>
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Taxes_Name" ReadOnly="true" Caption="Tax Component ID">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="taxCodeName" ReadOnly="true" Caption="Tax Component Name">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                            <td colspan="4">
                                <table class="InlineTaxClass">
                                    <tr class="GstCstvatClass" style="">
                                        <td style="padding-top: 10px; padding-bottom: 15px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                        <td style="padding-top: 10px; padding-bottom: 15px;">
                                            <dxe:ASPxComboBox ID="cmbGstCstVat" ClientInstanceName="ccmbGstCstVat" runat="server" SelectedIndex="-1"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                                ClearButton-DisplayMode="Always" OnCallback="cmbGstCstVat_Callback">
                                                <Columns>
                                                    <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                    <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />
                                                </Columns>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 10px; padding-bottom: 15px; padding-right: 25px">
                                            <dxe:ASPxTextBox ID="txtGstCstVat" MaxLength="80" ClientInstanceName="ctxtGstCstVat" ReadOnly="true" Text="0.00"
                                                runat="server" Width="100%">
                                                <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </dxe:ASPxTextBox>
                                        </td>
                                        <td>
                                            <input type="button" onclick="recalculateTax()" class="btn btn-info btn-small RecalculateInline" value="Recalculate GST" />
                                        </td>
                                    </tr>
                                </table>
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

        <!--Numbering Schema Modal -->
        <div class="modal fade" id="SchemeModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Numbering Scheme  Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="Schemekeydown(event)" id="txtSchemeSearch" autofocus width="100%" placeholder="Search by Schema Name or Branch" />
                        <div id="SchemeTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Schema Name</th>

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
        <!--TYpe Modal -->





    </div>

    <dxe:ASPxCallbackPanel runat="server" ID="deleteTax" ClientInstanceName="cdeleteTax" OnCallback="deleteTax_Callback">
        <ClientSideEvents EndCallback="deleteTaxEndCallBack" />
    </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />

    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />

    <asp:HiddenField ID="hdnLockFromDateFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockToDateFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateConFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockToDateConFreeze" runat="server" />


</asp:Content>
