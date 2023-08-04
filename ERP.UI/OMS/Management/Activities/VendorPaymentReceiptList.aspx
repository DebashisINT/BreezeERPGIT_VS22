<%--=======================================================Revision History=======================================    
    1.0   Pallab    V2.0.38   20-04-2023      25867: Vendor Payment/Receipt module design modification
    2.0   Sanchita  V2.0.38   30-05-2023      ERP - Listing Page - Vendor Payment / Receipt. refer: 26660  
=========================================================End Revision History=====================================--%>

<%@ Page Title="Vendor Payment Receipt" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="VendorPaymentReceiptList.aspx.cs" Inherits="ERP.OMS.Management.Activities.VendorPaymentReceiptList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style>
        .padTab {
            margin-top:5px;
        }
        .padTab>tbody>tr>td {
            padding-right:15px;
        }.padTab>tbody>tr>td:last-child {
             padding-right:0px;
         }
    </style>
    <script src="JS/VendorPaymentReceiptList.js"></script>

     <%--Rev 2.0--%>
    <script>
        function CallbackPanelEndCall(s, e) {
            CgvCustomerReceiptPayment.Refresh();
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
                right: 18px;
        }
        
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Vendor Payment/Receipt"></asp:Label>
            </h3>

        </div>
        <table class="padTab pull-right" id="gridFilter">
            <tr>
                <td>From Date</td>
                <%--Rev 1.0: "for-cust-icon" class add--%>
                <td style="width:130px" class="for-cust-icon">
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
                <%--Rev 1.0: "for-cust-icon" class add--%>
                <td style="width:130px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                
                <td>Unit
                </td>
                <td style="width:130px">
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>
    </div>

        <div class="form_main clearfix mb-10" id="btnAddNew">
        <div style="float: left; padding-right: 5px;">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>&nbsp;<u>P</u>ayment/Receipt</span> </a>
            <% } %>
            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
        </div>
    </div>
    
       <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
        <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>
        <div class="relative">
        <div class="makeFullscreen ">
         <span class="fullScreenTitle">Purchase Indent/Requisition</span>
         <span class="makeFullscreen-icon half hovered " data-instance="CgvCustomerReceiptPayment" title="Maximize Grid" id="expandCgvCustomerReceiptPayment">
           <i class="fa fa-expand"></i>
         </span>
    <dxe:ASPxGridView ID="Grid_CustomerReceiptPayment" runat="server" AutoGenerateColumns="False" KeyFieldName="ReceiptPayment_ID"
        ClientInstanceName="CgvCustomerReceiptPayment" Width="100%" SettingsBehavior-AllowFocusedRow="true" OnCustomCallback="Grid_CustomerReceiptPayment_CustomCallback"
        OnSummaryDisplayText="Grid_CustomerReceiptPayment_SummaryDisplayText" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
        OnDataBinding="Grid_CustomerReceiptPayment_DataBinding"
      DataSourceID="EntityServerModeDataSource"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"   Settings-HorizontalScrollBarMode="Auto">
        <SettingsSearchPanel Visible="True" Delay="5000" />
       <%-- <SettingsSearchPanel Visible="True" />--%>
         <%--SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true"--%>
        <ClientSideEvents RowClick="gridRowclick" />
        <Columns>
            <dxe:GridViewDataCheckColumn VisibleIndex="0" Visible="false">
                <EditFormSettings Visible="True" />
                <EditItemTemplate>
                    <dxe:ASPxCheckBox ID="ASPxCheckBox1" Text="" runat="server"></dxe:ASPxCheckBox>
                </EditItemTemplate>
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
               
            </dxe:GridViewDataCheckColumn>
            <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_ID" Visible="false">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="0" Caption="Serial" FieldName="SrlNo" Width="50px">
                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Type" FieldName="ReceiptPayment_TransactionType" Width="80px">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Posting Date" FieldName="ReceiptPayment_TransactionDt" Width="80px" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" >
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Document Number" FieldName="ReceiptPayment_VoucherNumber" Width="130px">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Doc. Type" FieldName="ReceiptDetail_DocumentTypeID" Width="200px" >
                <CellStyle CssClass="gridcellleft" Wrap="True"> </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <%-- <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Currency" FieldName="ReceiptPayment_Currency">
                <CellStyle Wrap="False" CssClass="gridcellleft" HorizontalAlign="left"></CellStyle>
            </dxe:GridViewDataTextColumn>--%>
            <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Vendor(s)" FieldName="Customer" Width="200px">
                <CellStyle CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
             
            <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Unit" FieldName="branch_description" Width="200px">
                <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Instrument Type" FieldName="InstrumentType" Width="150px">
                <CellStyle CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Instrument No" FieldName="InstrumentNumber" Width="200px">
                <CellStyle CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Tax Amount" FieldName="VRPTax_Amount">
                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                <CellStyle CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="CGST Amount" FieldName="Total_CGST">
                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                <CellStyle CssClass="gridcellleft"></CellStyle>
                 <Settings AutoFilterCondition="Contains" />
                <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="11" Caption="SGST Amount" FieldName="Total_SGST">
                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                <CellStyle CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="12" Caption="IGST Amount" FieldName="Total_IGST">
                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                <CellStyle CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="UTGST Amount" FieldName="Total_UTGST">
                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                <CellStyle CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="14" Caption="Amount" FieldName="Amount">
                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                <CellStyle CssClass="gridcellleft" HorizontalAlign="right"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn VisibleIndex="15" Caption="Cash/Bank" FieldName="CashBankID">
                <CellStyle CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="Proj_Name" Caption="Project Name" Settings-AllowAutoFilter="True">
                <CellStyle CssClass="gridcellleft"></CellStyle>
                   <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="17" FieldName="ReceiptPayment_CreateUser"
                Caption="Entered By">
                <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="left">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="18" FieldName="ReceiptPayment_CreateDateTime" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                Caption="Last Update On">
                <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="19" FieldName="ReceiptPayment_ModifyUser"
                Caption="Updated By">
                <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="left">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="20" Width="0">
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
                    </a><%} %>
                   </div>
                </DataItemTemplate>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderTemplate><span></span></HeaderTemplate>
                <EditFormSettings Visible="False"></EditFormSettings>
                <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>

        </Columns>
        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
        <TotalSummary>
            <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
            <dxe:ASPxSummaryItem FieldName="VRPTax_Amount" SummaryType="Sum" />
            <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
            <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
            <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
            <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
        </TotalSummary>

        <Settings ShowGroupPanel="True" HorizontalScrollBarMode="Visible" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" ShowFooter="true"
            ShowGroupFooter="VisibleIfExpanded" />
        <ClientSideEvents EndCallback="function(s, e) {
	                                        ShowMsgLastCall();
                                        }" />
        <SettingsBehavior ConfirmDelete="True" />
        <Styles>
            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
            <Footer CssClass="gridfooter"></Footer>
        </Styles>
        <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
        </SettingsPager>

    </dxe:ASPxGridView>
    </div>
    </div>
    </div>
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext"  TableName="v_VendorPaymentRecieptList" />
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
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
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <asp:HiddenField ID="HdRecPayType" runat="server" />
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
    </div>

       <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
<asp:HiddenField ID="hdnLockToDateedit" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />


      <asp:HiddenField ID="hdnLockFromDateeditCon" runat="server" />
<asp:HiddenField ID="hdnLockToDateeditCon" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDatedeleteCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedeleteCon" runat="server" />



    <asp:HiddenField ID="hdnLockFromDateReceiptEdit" runat="server" />
<asp:HiddenField ID="hdnLockToDateReceiptedit" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDateReceiptdelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDateReceiptdelete" runat="server" />


      <asp:HiddenField ID="hdnLockFromDateReceiptEditCon" runat="server" />
<asp:HiddenField ID="hdnLockToDateReceipteditCon" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDateReceiptdeleteCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateReceiptdeleteCon" runat="server" />

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
