<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ApprovalPopup_Transaction.aspx.cs" Inherits="ERP.OMS.Management.Master.ApprovalPopup_Transaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .padCenter {
            width: auto;
            margin: 0 auto;
        }

            .padCenter > tbody > tr > td {
                padding: 10px;
            }
    </style>

    <script>
        //function grid_SelectionChanged(s, e) {
        //    s.GetSelectedFieldValues("EuniqueId", GetSelectedFieldValuesCallback);
        //}

        //var value = "";

        //function GetSelectedFieldValuesCallback(values) {
        //    value = "";
        //    for (var i = 0; i < values.length; i++) {
        //        if (i == 0) {
        //            value = values[i];
        //        }
        //        else {
        //            value = value + ',' + values[i];
        //        }
        //    }
        //    //document.getElementById("selCount").innerHTML = grid.GetSelectedRowCount();
        //}

        function ShowClick() {
            grid.PerformCallback('BindGrid~' + $("#ddlModules").val() + '~' + cddlStatus.GetValue() + '~' + cdtFrom.GetDate().format('yyyy-MM-dd') + '~' + cdtTo.GetDate().format('yyyy-MM-dd'));
        }

        function ConfirmClick() {

            // $("#hdnentityid").val(value);
            grid.PerformCallback('SaveData~' + $("#ddlModules").val());

        }
        function grid_endcallback() {
            if (grid.cpStatus == "1") {
                grid.cpStatus = null;
                jAlert('Data Saved Successfully.', 'Alert.');
            }

            else if (grid.cpStatus == "2")
            {
                grid.cpStatus = null;
                jAlert('You must select atleast one document to save.', 'Alert.');
            }
        }


        function getconfirmtype() {

            cCallbackPanel.PerformCallback(cddlStatus.GetValue());
        }

        function OpenDetails(Uniqueid, type, docno) {
            var url = '';
            //debugger;
            if (type == 'CashBankVoucherPayment') {
                var tp = 'CBV'
                url = '/OMS/Management/dailytask/CashBankEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + tp;
            }

            if (type == 'Journal') {
                var tp = 'JV'
                url = '/OMS/Management/dailytask/JournalEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
            }

            if (type == 'CashBankVoucherReceipt') {
                var tp = 'CBV'
                url = '/OMS/Management/dailytask/CashBankEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + tp;
            }

            if (type == 'SaleOrder') {
                var tp = 'SO'
                url = url = '/OMS/Management/Activities/SalesOrderAdd.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=SO';
            }

            if (type == 'SaleOrder') {
                var tp = 'SO'
                url = url = '/OMS/Management/Activities/SalesOrderAdd.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=SO';
            }

            if (type == 'SaleInvoice') {
                var tp = 'SI'
                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            if (type == 'Posinvoice') {
                var tp = 'POS'
                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&Viemode=1';
            }

            if (type == 'custpayment') {
                var tp = 'CP'
                url = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CRP';
            }

            if (type == 'custreceipt') {
                var tp = 'CR'
                url = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CRP';
            }

            if (type == 'purchaseorder') {
                var tp = 'PO'
                url = '/OMS/Management/Activities/PurchaseOrder.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PO';
            }

            if (type == 'purchaseinvoice') {
                var tp = 'PB'
                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PB';
            }
            if (type == 'VendorPayment') {
                var tp = 'PB'
                url = '/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VPR';
            }
            if (type == 'VendorReceipt') {
                var tp = 'PB'
                url = '/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VPR';
            }
            if (type == 'CustomerDebitNote') {
               // var tp = 'PB'
                url = '/OMS/Management/Activities/CustomerNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
            }

            if (type == 'CustomerCreditNote') {
                // var tp = 'PB'
                url = '/OMS/Management/Activities/CustomerNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
            }

            if (type == 'VendorDebitNote') {
                // var tp = 'PB'
                url = '/OMS/Management/Activities/VendorDrCrNoteAdd.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VDN';
            }

            if (type == 'VendorCreditNote') {
                // var tp = 'PB'
                url = '/OMS/Management/Activities/VendorDrCrNoteAdd.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VCN';
            }

            if (type == 'SalesReturnNormal') {
                var tp = 'SRN'
                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            if (type == 'PurcaseReturn') {
                //var tp = 'SRN'
                url = '/OMS/Management/Activities/PReturn.aspx.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PR';
            }

            popupbudget.RefreshContentUrl();
          
            popupbudget.SetContentUrl(url);
            popupbudget.Show();
        }

        function BudgetAfterHide(s, e) {
            popupbudget.RefreshContentUrl();
            popupbudget.Hide();
        }
    </script>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Document Verify</h3>

        </div>
    </div>
    <div class="form_main">
        <div class="clearfix row">
            <div class="col-md-2">
                <label>Modules</label>
                <div>
                    <select id="ddlModules" class="form-control">
                        <option value="CashBankVoucherPayment">Cash/Bank Vouchers Payment</option>
                        <option value="CashBankVoucherReceipt">Cash/Bank Vouchers Receipt</option>
                        <option value="Journal">Journals</option>
                        <option value="SaleOrder">Sales Order</option>
                        <option value="SaleInvoice">Sales Invoice</option>
                        <option value="Posinvoice">POS Invoice</option>
                        <option value="custpayment">Customer Payment</option>
                        <option value="custreceipt">Customer Receipt</option>
                        <option value="purchaseorder">Purchase Order</option>
                        <option value="purchaseinvoice">Purchase Invoice</option>
                        <option value="VendorPayment">Vendor Payment</option>
                        <option value="VendorReceipt">Vendor Receipt</option>
                        <option value="CustomerDebitNote">Customer Debit Note</option>
                        <option value="CustomerCreditNote">Customer Credit Note</option>
                        <option value="VendorDebitNote">Vendor Debit Note</option>
                        <option value="VendorCreditNote">Vendor Credit Note</option>
                        <option value="SalesReturnNormal">Sales Return Normal</option>
                        <option value="PurcaseReturn">Purchase Return</option>

                    </select>
                </div>
            </div>
            <div id="dvHeader" runat="server">
                <div class="col-md-2">
                    <label>From Date</label>
                    <div>
                        <dxe:ASPxDateEdit runat="server" DisplayFormatString="dd-MM-yyyy" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" ID="dtFrom" Width="100%" ClientInstanceName="cdtFrom"></dxe:ASPxDateEdit>
                    </div>
                </div>

                <div class="col-md-2">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit runat="server" ID="dtTo" Width="100%" DisplayFormatString="dd-MM-yyyy" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="cdtTo"></dxe:ASPxDateEdit>
                    </div>
                </div>
            </div>
            <div class="col-md-2">
                <label>Verification Status</label>
                <div>
                    <dxe:ASPxComboBox runat="server" SelectedIndex="0" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" ID="ddlStatus" Width="100%" ClientInstanceName="cddlStatus" DataSourceID="dsStatus" ValueField="STATUS_ID" TextField="STATUS_NAME">
                        <ClientSideEvents SelectedIndexChanged="getconfirmtype" />
                    </dxe:ASPxComboBox>
                </div>
            </div>

            <div class="col-md-4 pdTop15">
                <label>&nbsp;</label>
                <button class="btn btn-primary mBot10" onclick="ShowClick();" type="button">Show</button>
            </div>
            <br />
        </div>
        <div class="clearfix row">
            <div class="col-md-12">
                <dxe:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding" KeyFieldName="EuniqueId" Width="100%" CssClass="pull-left" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false"
                    SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                   
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />

                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true" Caption="Select"/>
                        <dxe:GridViewDataColumn FieldName="EuniqueId" Visible="false" />

                        <dxe:GridViewDataColumn FieldName="MODULE_TYPE" Visible="false" />

                        <dxe:GridViewDataColumn FieldName="DocumentNo" Caption="Document No">
                            <Settings AutoFilterCondition="Contains" />

                        </dxe:GridViewDataColumn>

                        <dxe:GridViewDataColumn FieldName="DocumentDate" Caption="Document Date">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>

                        <dxe:GridViewDataColumn FieldName="EntityName" Caption="Entity Name">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>

                        <dxe:GridViewDataColumn FieldName="amount" Caption="Amount">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>


                        <dxe:GridViewDataColumn FieldName="EnteredBy" Caption="Created By">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>

                        <dxe:GridViewDataColumn FieldName="EnteredOn" Caption="Created On">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="ModifiedBy" Caption="Modified By">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="ModifiedOn" Caption="Modified On">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>

                        <dxe:GridViewDataTextColumn ReadOnly="True" CellStyle-HorizontalAlign="Center" Width="100px">
                            <HeaderStyle HorizontalAlign="Center" />

                            <CellStyle HorizontalAlign="Center"></CellStyle>
                            <HeaderTemplate>
                                Actions
                            </HeaderTemplate>
                            <DataItemTemplate>
                                <% if (rights.CanView)
                                   { %>
                                <a href="javascript:void(0)" onclick="OpenDetails('<%#Eval("EuniqueId") %>','<%#Eval("MODULE_TYPE") %>','<%#Eval("DocumentNo") %>')" class="pad">
                                <img src="/assests/images/doc.png" />
                                </a>
                                 <%} %>
                            </DataItemTemplate>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>


                    </Columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <ClientSideEvents EndCallback="grid_endcallback" />
                </dxe:ASPxGridView>
            </div>
        </div>
        <div class="clearfix row">
            <div class="col-md-12 text-center">
                <table class="padCenter">
                    <tr>
                        <td>
                            <label>Update verify status as</label>

                        </td>
                        <td>
                            <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">
                                        <dxe:ASPxComboBox runat="server" ID="ddlConfirm" Width="100%" ClientInstanceName="cddlConfirm" ValueType="System.String">
                                        </dxe:ASPxComboBox>


                                    </dxe:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents />
                            </dxe:ASPxCallbackPanel>




                        </td>
                        <td>
                            <% if (rights.CanAdd)
                           {
                            %>
                            <button type="button" id="btnConfirm" onclick="ConfirmClick();" class="btn btn-success btn-xs">Save</button>
                             <%} %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>


        <asp:SqlDataSource ID="dsStatus" runat="server" SelectCommand="select STATUS_ID,STATUS_NAME from MASTER_APPROVAL_STATUS"></asp:SqlDataSource>

        <%--<asp:HiddenField runat="server" ID="hdnentityid" />--%>
    </div>
    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </contentcollection>

        <clientsideevents closeup="BudgetAfterHide" />
    </dxe:ASPxPopupControl>

</asp:Content>
