<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                10-04-2023        V2.0.37           Pallab              25966: Customer Receipt/Payment module design modification & check in small device
2.0                28/11/2023        V2.0.41          Priti               0027028: Customer code column is required in the listing module of Sales entry
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerReceiptPaymentList.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerReceiptPaymentList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .padTab {
            margin-bottom: 4px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
                vertical-align: middle;
            }

                .padTab > tbody > tr > td > label, .padTab > tbody > tr > td > input[type="button"] {
                    margin-bottom: 0 !important;
                }

        .backBranch {
            font-weight: 600;
            background: #75c1f5;
            padding: 5px;
        }
    </style>
    <script src="JS/CustomerReceiptPaymentList.js?v1.0"></script>


    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    

        <style>
            select
            {
                -webkit-appearance: auto;
            }
            #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate
            {
                position: relative;
                z-index: 1;
                background: transparent;
            }

            #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dtPostingDate_B-1
            {
                background: transparent !important;
                border: none;
                width: 30px;
                padding: 10px !important;
            }

            #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
            #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dtPostingDate_B-1 #dtPostingDate_B-1Img
            {
                display: none;
            }

            .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
            #Grid_CustomerReceiptPayment
            {
                max-width: 99% !important;
            }

        /*.calendar-icon
        {
                right: 5px !important;
        }

        select#ddlInventory
        {
            -webkit-appearance: auto;
        }*/

        .simple-select::after
        {
            top: 26px !important;
            right: 13px !important;
        }

        .col-sm-3 , .col-md-3 , .col-md-2{
            margin-bottom: 5px;
        }

        #rdl_Salesquotation
        {
            margin-top: 10px;
        }
        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        #drdTransCategory.aspNetDisabled {
    background: #f3f3f3 !important;
}

       /* #CustomerTableTbl.dynamicPopupTbl>tbody>tr>td
        {
            width: 33.33%;
        }*/

       .lblmTop8>span, .lblmTop8>label
        {
                margin-top: 0 !important;
        }

            @media only screen and (max-width: 1380px) and (min-width: 1300px)
            {

                /*.col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12 {
                    padding-right: 10px;
                    padding-left: 10px;
                }

                .simple-select::after
                {
                    right: 8px !important;
                }
                .calendar-icon {
                    right: 13px !important;
                }*/

                input[type="radio"], input[type="checkbox"] {
                    margin-right: 0px;
                }
                
            #ASPxPopupControl2_PW-1 , #popupApproval_PW-1 , #ASPXPopupControl1_PW-1
            {
                position:fixed !important;
                left: 10px !important;
                top: 5% !important;
            }
        
            }
        </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Customer Receipt/Payment"></asp:Label>
            </h3>
        </div>
        <table class="padTab pull-right" id="gridFilter" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From Date</label></td>

                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td>&nbsp;</td>
                <td>
                    <label>To Date</label>
                </td>

                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td>&nbsp;</td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>&nbsp;</td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>
    </div>

        <div class="form_main clearfix" id="btnAddNew">
        <div style="float: left; padding-right: 5px;" class="mb-10">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="AddReceiptButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>R</u>eceipt</span> </a>
            <a href="javascript:void(0);" onclick="AddPaymentButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>P</u>ayment</span> </a>
            <%--<a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a>--%>
            <% } %>
            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true">
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
        <dxe:ASPxGridView ID="Grid_CustomerReceiptPayment" runat="server" AutoGenerateColumns="False" KeyFieldName="ReceiptPayment_ID"
            ClientInstanceName="CgvCustomerReceiptPayment" Width="100%" SettingsBehavior-AllowFocusedRow="true"
            OnCustomCallback="Grid_CustomerReceiptPayment_CustomCallback" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
            DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
            OnSummaryDisplayText="Grid_CustomerReceiptPayment_SummaryDisplayText" Settings-HorizontalScrollBarMode="Auto">
            <SettingsSearchPanel Visible="true" Delay="5000" />
            <%-- <SettingsSearchPanel Visible="True" />--%><%-- OnDataBinding="Grid_CustomerReceiptPayment_DataBinding"--%>
            <%--SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true"
        SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true"--%>
            <ClientSideEvents />
            <Columns>
                <%-- Rev Sayantani--%>
                <%--<dxe:GridViewDataTextColumn FieldName="ReceiptPayment_ID" Visible="false" SortOrder="Descending">
                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_ID" Visible="false" ShowInCustomizationForm="false" SortOrder="Descending">
                    <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <%--   Rev Sayantani--%>
                <dxe:GridViewDataTextColumn VisibleIndex="0" Caption="Serial" FieldName="SrlNo" Width="50px">
                    <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Type" FieldName="ReceiptPayment_TransactionType" Width="80px">
                    <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Posting Date" FieldName="ReceiptPayment_TransactionDt" Width="80px" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                    <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Document No." FieldName="ReceiptPayment_VoucherNumber" Width="130px">
                    <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <%-- <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Currency" FieldName="ReceiptPayment_Currency">
                <CellStyle Wrap="False" CssClass="gridcellleft" HorizontalAlign="left"></CellStyle>
            </dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Doc. Type" FieldName="ReceiptDetail_DocumentTypeID" Width="200px">
                    <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Unit" FieldName="BranchName" Width="200px">
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <%--            <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Customer/Vendor" FieldName="Customer" Width="220px">
                <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                 <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>--%>
                 <%--Rev  2.0--%>
                  <dxe:GridViewDataTextColumn Caption="Customer Code" FieldName="CustomerCode" Width="160px"
                  VisibleIndex="6">
                  <CellStyle CssClass="gridcellleft" Wrap="true">
                  </CellStyle>
                  <Settings AutoFilterCondition="Contains" />
                 </dxe:GridViewDataTextColumn>
                <%-- Rev  2.0 End--%>
                <dxe:GridViewDataTextColumn FieldName="Customer" Caption="Customer/Vendor" VisibleIndex="7" Width="220">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="CustomerClick(this,'<%# Eval("ReceiptPayment_CustomerID") %>')">
                            <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("Customer")%>'
                                ToolTip="Customer Outstanding">
                            </dxe:ASPxLabel>
                        </a>

                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False" CssClass="text-center">
                    </CellStyle>
                    <%-- <HeaderTemplate>
                                                    Status
                                                </HeaderTemplate>--%>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AllowAutoFilter="False" />
                    <HeaderStyle Wrap="False" CssClass="text-center" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Instrument Type" FieldName="InstrumentType" Width="150px">
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Instrument No" FieldName="InstrumentNumber" Width="150px">
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Tax Amount" FieldName="CRPTax_Amount">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="11" Caption="CGST Amount" FieldName="Total_CGST">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="12" Caption="SGST Amount" FieldName="Total_SGST">

                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="IGST Amount" FieldName="Total_IGST">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="14" Caption="UTGST Amount" FieldName="Total_UTGST">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="15" Caption="Voucher Amount" FieldName="Amount">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="Proj_Name" Caption="Project Name" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="True" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn VisibleIndex="17" Caption="Cash/Bank" FieldName="CashBankID">
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
               
            <dxe:GridViewDataTextColumn VisibleIndex="18" FieldName="ReceiptPayment_CreateUser"
                Caption="Entered By">
                <CellStyle CssClass="gridcellleft" Wrap="True">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="19" FieldName="ReceiptPayment_CreateDateTime" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                    Caption="Entered On">
                    <CellStyle CssClass="gridcellleft" Wrap="True">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="20" FieldName="ReceiptPayment_ModifyUser"
                    Caption="Updated By">
                    <CellStyle CssClass="gridcellleft" Wrap="True">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="21" FieldName="ReceiptPayment_ModifyDateTime" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                    Caption="Last Update On">
                    <CellStyle CssClass="gridcellleft" Wrap="True">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
        

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="22" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                                <span class='ico ColorFour'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="" title="" style='<%#Eval("Editlock")%>'>
                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                            <% } %>
                            <%--Rev Work Date:-21.03.2022 -Copy Function add--%>
                            <% if (rights.CanAdd)
                               { %>
                            <a href="javascript:void(0);" onclick="OnCopyInfoClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="" title="">
                                    <span class='ico editColor'><i class='fa fa-files-o' aria-hidden='true'></i></span><span class='hidden-xs'>Copy</span></a>
                            <% } %>
                            <%--Close of Rev Work Date:-21.03.2022 -Copy Function add--%>
                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" style='<%#Eval("Deletelock")%>'>
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                            <% } %>
                            <% if (rights.CanPrint)
                               { %>
                            <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="" title="">
                                <span class='ico ColorFive'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                            </a><%} %>
                            <% if (rights.CanViewAdjustment)
                               { %>
                            <a href="javascript:void(0);" onclick="onShowAdjustment('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="" title="">

                                <span class='ico ColorSix'><i class="fa fa-calculator"></i></span><span class='hidden-xs'>Show Adjusted Documents</span>
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
            <%-- --Rev Sayantani--%>
            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
            <SettingsCookies Enabled="true" StorePaging="true" Version="4.0" StoreColumnsVisiblePosition="true" />

            <%-- -- End of Rev Sayantani --%>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <TotalSummary>
                <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                <dxe:ASPxSummaryItem FieldName="CRPTax_Amount" SummaryType="Sum" />
                <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
                <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
                <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
                <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
            </TotalSummary>

            <Settings ShowGroupPanel="True" HorizontalScrollBarMode="Visible" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" ShowFooter="true"
                ShowGroupFooter="VisibleIfExpanded" />
            <ClientSideEvents EndCallback="function(s, e) {
	                                        ShowMsgLastCall();
                                        }"
                RowClick="gridRowclick" />
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
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="CustomerReceiptPaymentList" />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
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


    <div class="PopUpAreaDoc">
        <dxe:ASPxPopupControl ID="popDoc" runat="server" ClientInstanceName="cpopDoc"
            Width="700px" HeaderText="Adjustment Details" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="doc_selectPanel" ClientInstanceName="cdoc_selectPanel" OnCallback="doc_selectPanel_Callback" ClientSideEvents-EndCallback="cdoc_selectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">

                                <dxe:ASPxGridView runat="server" ClientInstanceName="cdocGrid" ID="docGrid" OnDataBinding="docGrid_DataBinding" Width="100%">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="SL" Caption="SL#">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Module" FieldName="AdjustedModule">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Adjusted Doc No." FieldName="AdjustedNo">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Doc Date" FieldName="AdjustedDate">
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>

                                </dxe:ASPxGridView>


                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>



    <dxe:ASPxPopupControl ID="ASPxPopupControl2" runat="server" ClientInstanceName="cOutstandingPopup"
        Width="1300px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <strong><span style="color: #fff">Customer Outstanding</span></strong>

            <dxe:ASPxImage ID="ASPxImage2" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            cOutstandingPopup.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>

            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxGridView runat="server" KeyFieldName="SLNO" ClientInstanceName="cCustomerOutstanding" ID="CustomerOutstanding"
                    DataSourceID="LinqServerModeDataSourceCO" OnSummaryDisplayText="ShowGridCustOut_SummaryDisplayText"
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cCustomerOutstanding_CustomCallback"
                    Settings-ShowFooter="true" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                    OnHtmlFooterCellPrepared="ShowGridCustOut_HtmlFooterCellPrepared" OnHtmlDataCellPrepared="ShowGridCustOut_HtmlDataCellPrepared">

                    <SettingsBehavior AllowDragDrop="true" AllowSort="true"></SettingsBehavior>
                    <SettingsPager Visible="true"></SettingsPager>
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="PARTYNAME" GroupIndex="0"
                            VisibleIndex="0">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="BRANCH_DESCRIPTION" Width="55%" ReadOnly="true" Caption="UNIT">
                        </dxe:GridViewDataTextColumn>
                        <%--<dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PARTYNAME" Width="100" ReadOnly="true" Caption="Customer">
                                </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_TYPE" ReadOnly="true" Caption="Doc. Type" Width="100%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ISOPENING" ReadOnly="true" Caption="Opening?" Width="30%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DOC_NO" ReadOnly="true" Width="95%" Caption="Document No">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOC_DATE" Width="50%" ReadOnly="true" Caption="Document Date">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="DUE_DATE" Width="50%" ReadOnly="true" Caption="Due Date">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="DOC_AMOUNT" ReadOnly="true" Caption="Document Amt." Width="50%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="BAL_AMOUNT" ReadOnly="true" Caption="Balance Amount" Width="50%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="DAYS" Width="20%" ReadOnly="true" Caption="Days">
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" AllowSort="False" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                    <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                    <SettingsEditing Mode="EditForm" />
                    <SettingsContextMenu Enabled="true" />
                    <SettingsBehavior AutoExpandAllGroups="true" />
                    <Settings ShowGroupPanel="true" ShowStatusBar="Visible" ShowFilterRow="true" />
                    <SettingsSearchPanel Visible="false" />
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    </SettingsPager>
                    <TotalSummary>
                        <dxe:ASPxSummaryItem FieldName="DOC_TYPE" SummaryType="Custom" Tag="Item_DocType" />
                        <dxe:ASPxSummaryItem FieldName="BAL_AMOUNT" SummaryType="Custom" Tag="Item_BalAmt"></dxe:ASPxSummaryItem>
                    </TotalSummary>

                    <SettingsDataSecurity AllowEdit="true" />

                </dxe:ASPxGridView>

                <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceCO" runat="server" OnSelecting="EntityServerModeDataSourceCO_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="PARTYOUTSTANDINGDET_REPORT" />
                <div style="display: none">
                    <dxe:ASPxGridViewExporter ID="exporter1" GridViewID="CustomerOutstanding" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hdnCustomerId" runat="server" />
        <asp:HiddenField runat="server" ID="hddnBranchId" />
        <asp:HiddenField runat="server" ID="hddnAsOnDate" />
        <asp:HiddenField runat="server" ID="hddnOutStandingBlock" />
        <asp:HiddenField runat="server" ID="ISAllowBackdatedEntry" />
        <asp:HiddenField runat="server" ID="warehousestrProductID" />
    </div>

    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
<asp:HiddenField ID="hdnLockToDateedit" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateReceiptEdit" runat="server" />
<asp:HiddenField ID="hdnLockToDateReceiptedit" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDateReceiptdelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDateReceiptdelete" runat="server" />
    <asp:HiddenField ID="hFilterType" runat="server" />

<%--      <asp:HiddenField ID="hdnLockFromDateeditDataFreeze" runat="server" />
<asp:HiddenField ID="hdnLockToDateeditDataFreeze" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDatedeleteDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedeleteDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateReceiptEditDataFreeze" runat="server" />
<asp:HiddenField ID="hdnLockToDateReceipteditDataFreeze" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDateReceiptdeleteDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockToDateReceiptdeleteDataFreeze" runat="server" />--%>


     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>


</asp:Content>
