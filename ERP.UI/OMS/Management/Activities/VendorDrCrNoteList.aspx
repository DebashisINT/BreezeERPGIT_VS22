<%--=======================================================Revision History=======================================    
    1.0   Pallab    V2.0.38   20-04-2023      26044: Vendor Credit/Debit Note module design modification & check in small device
    2.0   Sanchita  V2.0.38   30-05-2023      ERP - Listing Views - Vendor Debit/Credit Note. refer: 26589  
=========================================================End Revision History=====================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="VendorDrCrNoteList.aspx.cs" Inherits="ERP.OMS.Management.Activities.VendorDrCrNoteList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }
        .padTabtype2 > tbody > tr > td > label {
            margin-bottom: 0 !important;
            margin-right: 15px;
        }
    </style>
    <script src="JS/VendorDrCrNoteList.js"></script>
     <%--Rev 2.0--%>
    <script>
        function CallbackPanelEndCall(s, e) {
            cGvJvSearch.Refresh();
        }

    </script>
    <%--End of Rev 2.0 --%>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        #GvJvSearch {
            max-width: 98% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: auto;
        }

        .calendar-icon
        {
                right: 10px;
        }
        
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title  pull-left">
            <h3 class="clearfix ">
                <asp:Label ID="lblHeading" runat="server" Text="Vendor Credit/Debit Note"></asp:Label>
            </h3> 
        </div>
        <table class="padTabtype2 pull-right" id="gridFilter">
                    <tr>
                        <td>
                            <label>From Date</label></td>
                        <%--Rev 1.0: "for-cust-icon" class add --%>
                        <td class="for-cust-icon">
                            <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                            <%--Rev 1.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 1.0--%>
                        </td>
                        <td>
                            <label>To Date</label>
                        </td>
                        <%--Rev 1.0: "for-cust-icon" class add --%>
                        <td class="for-cust-icon">
                            <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                            <%--Rev 1.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 1.0--%>
                        </td>
                        <td>Unit</td>
                        <td>
                            <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                        </td>

                    </tr>

                </table>
    </div>
        <div class="form_main rgth pull-left full">

        <div class="clearfix">
            <div style="padding-right: 5px;" class="mb-10">
                <span id="divAddButton">
                     <% if (rights.CanAdd)
                           { %>
                    <a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus" ></i> </span><span><u>A</u>dd New</span> </a>
                     <% } %>
                </span>
                <span id="divExportto">
                    <% if (rights.CanExport)
                           { %>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                       <% } %>
                </span>

                

            </div>
        </div>
         <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
     <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>

        <div class="clearfix relative">
            <div class="makeFullscreen ">
                 <span class="fullScreenTitle">Vendor Credit/Debit Note</span>
                 <span class="makeFullscreen-icon half hovered " data-instance="cGvJvSearch" title="Maximize Grid" id="expandcGvJvSearch">
                   <i class="fa fa-expand"></i>
                 </span>    
            <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                ClientInstanceName="cGvJvSearch" KeyFieldName="DCNote_ID" Width="100%" Settings-HorizontalScrollBarMode="Auto"
                OnCustomCallback="GvJvSearch_CustomCallback" OnCustomButtonInitialize="GvJvSearch_CustomButtonInitialize"
                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <ClientSideEvents EndCallback="function(s, e) {GvJvSearch_EndCallBack();}" />
                <SettingsBehavior ConfirmDelete="True" />
                <Styles>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                    <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                    <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                    <Footer CssClass="gridfooter"></Footer>
                </Styles>

                <Columns>

                    <dxe:GridViewDataComboBoxColumn Caption="Type" FieldName="NoteType" VisibleIndex="0">
                        <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                            ValueType="System.String" DataSourceID="SqlDataSourceapplicable" TextField="TypeName" ValueField="TypeID">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataComboBoxColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="NoteDate" Caption="Document Date" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NoteNumber" Caption="Document Number" Width="150px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Currency" Caption="Currency">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="BranchName" Caption="Unit" Width="200px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="VendorName" Caption="Vendor Name" Width="200px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="Amount" Caption="Net Amount" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Total_CGST" Caption="CGST" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Total_SGST" Caption="SGST" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="Total_UTGST" Caption="UTGST" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Total_IGST" Caption="IGST" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Total_taxable_amount" Caption="Taxable Amount" HeaderStyle-HorizontalAlign="Right">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="Invoice_Number" Caption="Ref. Purchase Invoice No.">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="EnteredBy" Caption="Entered On">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name" Width="150px"  VisibleIndex="17" Settings-AllowAutoFilter="True">
                           <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="true" />
                           <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="UpdateOn" Caption="Last Update On" Width="130px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="UpdatedBy" Caption="Updated By">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="DCNote_ID" SortOrder="Descending" Width="0"></dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="18" Width="0">
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>
                                <% if (rights.CanView)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                                    <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                <% } %>
                                <% if (rights.CanEdit)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="" title="" style='<%#Eval("Editlock")%>'>
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                <% } %>
                                <% if (rights.CanDelete)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="" style='<%#Eval("Deletelock")%>'>
                                   <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                <% } %>
                                <% if (rights.CanPrint)
                                   { %>
                                <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="" title="">
                                   <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                </a>
                                <%} %>
                            </div>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span></span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    
                </Columns>
                <ClientSideEvents RowClick="gridRowclick" />
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" ShowFooter="true" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_taxable_amount" SummaryType="Sum" />
                </TotalSummary>
            </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="V_VendorDrCrNoteDetailsList" />
            <asp:SqlDataSource ID="SqlDataSourceapplicable" runat="server" 
                SelectCommand="(SELECT 'Debit Note' as TypeID,'Debit Note' as TypeName) Union (SELECT 'Credit Note' as TypeID,'Credit Note' as TypeName)"></asp:SqlDataSource>
            <asp:HiddenField ID="hfIsFilter" runat="server" />
            <asp:HiddenField ID="hfFromDate" runat="server" />
            <asp:HiddenField ID="hfToDate" runat="server" />
            <asp:HiddenField ID="hfBranchID" runat="server" />
            </div>
        </div>
    </div>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

     <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <asp:HiddenField ID="HdCrDrNoteType" runat="server" />
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--DEBASHIS--%>

     <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
<asp:HiddenField ID="hdnLockToDateedit" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateReceiptEdit" runat="server" />
<asp:HiddenField ID="hdnLockToDateReceiptedit" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDateReceiptdelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDateReceiptdelete" runat="server" />



    <asp:HiddenField ID="hdnLockFromDateeditDataFreeze" runat="server" />
<asp:HiddenField ID="hdnLockToDateeditDataFreeze" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDatedeleteDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedeleteDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateReceiptEditDataFreeze" runat="server" />
<asp:HiddenField ID="hdnLockToDateReceipteditDataFreeze" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDateReceiptdeleteDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockToDateReceiptdeleteDataFreeze" runat="server" />
    <%--Rev 2.0--%>
     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <%--End of Rev 2.0--%>
</asp:Content>
