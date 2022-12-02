<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="InfluencerPaymentList.aspx.cs" Inherits="ERP.OMS.Management.Activities.InfluencerPaymentList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .padTab {
            margin-top: 5px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
            }

                .padTab > tbody > tr > td:last-child {
                    padding-right: 0px;
                }
    </style>
    <script src="JS/InfluencerPaymentList.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Influencer Payment"></asp:Label>
            </h3>

        </div>
        <table class="padTab pull-right" id="gridFilter">
            <tr>
                <td>From Date</td>
                <td>&nbsp;</td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>&nbsp;</td>
                <td>
                    <label>To Date</label>
                </td>
                <td>&nbsp;</td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>&nbsp;</td>
                <td>Unit
                </td>
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
        <div style="float: left; padding-right: 5px;">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>A</u>dd New</span> </a>
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
        <div class="clear"></div>
        <div class="clearfix relative">
            <dxe:ASPxGridView ID="Grid_InfluencerPayment" runat="server" AutoGenerateColumns="False" KeyFieldName="Payment_ID"
                ClientInstanceName="CgvInfluencerPayment" Width="100%" SettingsBehavior-AllowFocusedRow="true"
                DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" OnCustomCallback="Grid_InfluencerPayment_CustomCallback" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="240" Settings-VerticalScrollBarMode="Auto">
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <%-- <SettingsSearchPanel Visible="True" />--%>
                <%--SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true"--%>
                <ClientSideEvents />
                <Columns>
                    <dxe:GridViewDataCheckColumn VisibleIndex="0" Visible="false">
                        <EditFormSettings Visible="True" />
                        <EditItemTemplate>
                            <dxe:ASPxCheckBox ID="ASPxCheckBox1" Text="" runat="server"></dxe:ASPxCheckBox>
                        </EditItemTemplate>
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />

                    </dxe:GridViewDataCheckColumn>
                    <dxe:GridViewDataTextColumn FieldName="Payment_ID" Visible="false">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="0" Caption="Serial" FieldName="SrlNo" Width="50px">
                        <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Type" FieldName="Payment_TransactionType" Width="80px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Posting Date" FieldName="Payment_TransactionDt" Width="80px" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Document Number" FieldName="Payment_VoucherNumber" Width="130px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Doc. Type" FieldName="PaymentDetail_DocumentTypeID" Width="200px">
                        <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%-- <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Currency" FieldName="Payment_Currency">
                <CellStyle Wrap="False" CssClass="gridcellleft" HorizontalAlign="left"></CellStyle>
            </dxe:GridViewDataTextColumn>--%>
                    <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Influencer(s)" FieldName="Customer" Width="200px">
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
                    <dxe:GridViewDataTextColumn VisibleIndex="16" Caption="Project Name" FieldName="Proj_Name" Width="120px">
                        <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="17" FieldName="Payment_CreateUser"
                        Caption="Entered By">
                        <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="left">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="18" FieldName="Payment_CreateDateTime" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                        Caption="Last Update On">
                        <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="19" FieldName="Payment_ModifyUser"
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
                                <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="" title="">
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                <% } %>
                                <% if (rights.CanDelete)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="">
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
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="v_InfluencerList" />
            <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                <ClientSideEvents ControlsInitialized="AllControlInitilize" />
            </dxe:ASPxGlobalEvents>
        </div>
    </div>
    <!-- end of form main -->
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
    </div>
</asp:Content>
