<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="CustomerAdvancedReceiptList.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerAdvancedReceiptList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //function onPrintJv(id) {
        //    debugger;
        //    window.open("../../reports/XtraReports/Viewer/CustomerReceiptPaymentReportViewer.aspx?id=" + id, '_blank');
        //    //   window.location.href = "../../reports/XtraReports/Viewer/CustomerReceiptPaymentReportViewer.aspx?id=" + id;
        //}

        document.onkeydown = function (e) {
            // if (event.keyCode == 18) isCtrl = true;
            if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) { //run code for alt+A -- ie, Save & New  
                StopDefaultAction(e);
                AddButtonClick();
            }
        }
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function AddButtonClick() {
            var url = 'CustomerAdvanceReceipt.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        function OnViewClick(keyValue) {
            var url = 'CustomerAdvanceReceipt.aspx?key=' + keyValue + '&type=CRP' + '&req=V';
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue) {
            var url = 'CustomerAdvanceReceipt.aspx?key=' + keyValue + '&type=CRP';
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

        var RecPayId = 0;
        function onPrintJv(id) {
            debugger;
            RecPayId = id;
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }

        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'CUSTRECPAY';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId, '_blank')
            }
            cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == null) {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Customer Advanced Receipt"></asp:Label>
            </h3>
        </div>

    </div>

    <div class="form_main clearfix" id="btnAddNew">
        <div style="float: left; padding-right: 5px;">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a>
            <% } %>
            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
        </div>
    </div>
    
    <dxe:ASPxGridView ID="Grid_CustomerAdvancedReceipt" runat="server" AutoGenerateColumns="False" KeyFieldName="ReceiptPayment_ID"
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
            <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Tax Amount" FieldName="CRPTax_Amount">
                <CellStyle CssClass="gridcellleft" HorizontalAlign="right"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Cash/Bank" FieldName="CashBankID">
                <CellStyle CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="ReceiptPayment_CreateUser"
                Caption="Entered By">
                <CellStyle Wrap="False" HorizontalAlign="left">
                </CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="ReceiptPayment_CreateDateTime"
                Caption="Last Update On">
                <CellStyle Wrap="False" HorizontalAlign="Right">
                </CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="ReceiptPayment_ModifyUser"
                Caption="Updated By">
                <CellStyle Wrap="False" HorizontalAlign="left">
                </CellStyle>
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="15%">
                <DataItemTemplate>
                     <% if (rights.CanView)
                            { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="View">
                            <img src="../../../assests/images/viewIcon.png" /></a>
                           <% } %>
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
                    <% if (rights.CanPrint)
                                       { %>
                         <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="pad" title="print">
                            <img src="../../../assests/images/Print.png" />
                        </a><%} %>
                    
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
         <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
            </SettingsPager>


    </dxe:ASPxGridView>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
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
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>

</asp:Content>
