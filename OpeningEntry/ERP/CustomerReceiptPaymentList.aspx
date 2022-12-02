<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master"  AutoEventWireup="true" CodeBehind="CustomerReceiptPaymentList.aspx.cs" 
    Inherits="OpeningEntry.ERP.CustomerReceiptPaymentList" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        document.onkeydown = function (e) {
            // if (event.keyCode == 18) isCtrl = true;
            if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                AddButtonClick();
            }
        }
        function AddButtonClick() {
            var url = 'CustomerReceiptPayment.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue) {
            var url = 'CustomerReceiptPayment.aspx?key=' + keyValue;
            window.location.href = url;
        }
        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    CgvCustomerReceiptPayment.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        function ShowMsgLastCall() {

            if (CgvCustomerReceiptPayment.cpDelete != null) {

                jAlert(CgvCustomerReceiptPayment.cpDelete)
                CgvCustomerReceiptPayment.PerformCallback();
                CgvCustomerReceiptPayment.cpDelete = null
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Customer Receipt/Payment"></asp:Label>
            </h3>
        </div>

    </div>

    <div class="form_main clearfix" id="btnAddNew">
        <div style="float: left; padding-right: 5px;">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>A</u>dd New</span> </a>
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
    <div>
        <br />
    </div>
    <dxe:ASPxGridView ID="Grid_CustomerReceiptPayment" runat="server" AutoGenerateColumns="False" KeyFieldName="ReceiptPayment_ID"
        ClientInstanceName="CgvCustomerReceiptPayment" Width="100%" SettingsBehavior-AllowFocusedRow="true" 
        OnCustomCallback="Grid_CustomerReceiptPayment_CustomCallback"
        SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true"
        SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true">
        <SettingsSearchPanel Visible="True" />
        <ClientSideEvents />
        <Columns>
            <dxe:GridViewDataCheckColumn VisibleIndex="0" Visible="false">
                <EditFormSettings Visible="True" />
                <EditItemTemplate>
                    <dxe:ASPxCheckBox ID="ASPxCheckBox1" Text="" runat="server"></dxe:ASPxCheckBox>
                </EditItemTemplate>
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataCheckColumn>
            <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_ID" Visible="false">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Type" FieldName="ReceiptPayment_TransactionType">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Date" FieldName="ReceiptPayment_TransactionDate">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Voucher Number" FieldName="ReceiptPayment_VoucherNumber">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Currency" FieldName="ReceiptPayment_Currency">
                <CellStyle Wrap="False" CssClass="gridcellleft" HorizontalAlign="left"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Customer(s)" FieldName="Customer" Width="20%">
                <CellStyle CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Amount" FieldName="Amount">
                <CellStyle CssClass="gridcellleft" HorizontalAlign="right"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Cash/Bank" FieldName="CashBankID">
                <CellStyle CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="ReceiptPayment_CreateUser"
                Caption="Entered By">
                <CellStyle Wrap="False" HorizontalAlign="left">
                </CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="ReceiptPayment_CreateDateTime"
                Caption="Last Update On">
                <CellStyle Wrap="False" HorizontalAlign="Right">
                </CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="ReceiptPayment_ModifyUser"
                Caption="Updated By">
                <CellStyle Wrap="False" HorizontalAlign="left">
                </CellStyle>
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="15%">
                <DataItemTemplate>
                    <% if (rights.CanEdit)
                       { %>
                    <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="pad" title="Edit">
                        <img src="../../../assests/images/info.png" /></a>
                    <% } %>
                    <% if (rights.CanDelete)
                       { %>
                    <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete">
                        <img src="../../../assests/images/Delete.png" /></a>
                    <% } %>
                </DataItemTemplate>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>

        </Columns>
        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
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
        <SettingsPager Position="Bottom" NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
        </SettingsPager>


    </dxe:ASPxGridView>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>

</asp:Content>
