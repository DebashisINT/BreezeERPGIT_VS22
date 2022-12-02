<%@ Page Title="Finance Reconciliation" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="frm_downpaymententry.aspx.cs" Inherits="ERP.OMS.Management.Activities.frm_downpaymententry" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script src="JS/downpaymententry.js"></script>
    <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>

    

    <script>
        function AddnewFinance() {
            LoadingPanel.Show();

            cdtEntryDate.SetEnabled(true);
            cdtDpDate.SetEnabled(true);
            ccmbFinancer.SetEnabled(true);
            ccmbBranch.SetEnabled(true);
            ctxtBillAmount.SetEnabled(true);
            cdtBilldate.SetEnabled(true);
            cCustomerComboBox.SetEnabled(true);
            ctxtFinanceAmount.SetEnabled(true);
            ctxtDownPay1.SetEnabled(true);
            ctxtDownPay2.SetEnabled(true);
            ctxtDivestmentAmt1.SetEnabled(true);
            ctxtDivestmentAmt2.SetEnabled(true);
            ctxtDivestmentAmt3.SetEnabled(true);
            ctxtfinalPayment.SetEnabled(true);
            ctxtDbdPercentage.SetEnabled(true);
            ctxtDbdAmount.SetEnabled(true);
            ctxtMbdPercentage.SetEnabled(true);
            ctxtMbdAmount.SetEnabled(true);
            ctxtProcessingFee.SetEnabled(true);
            ctxtTotalPay.SetEnabled(false);
            ctxtbalance.SetEnabled(false);
            ccmbStatus.SetEnabled(true);

            document.getElementById('<%= txtproduct.ClientID %>').disabled = false;
    document.getElementById('<%= txtdownPayNo.ClientID %>').disabled = false;
            document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
            document.getElementById('<%= txtChallanNo.ClientID %>').disabled = false;
            document.getElementById('<%= txtSfCode.ClientID %>').disabled = false;
            document.getElementById('<%= txtAdjmntNo.ClientID %>').disabled = false;
            document.getElementById('<%= txtAdjmntDt.ClientID %>').disabled = false;
            document.getElementById('<%= txtModeofPayment.ClientID %>').disabled = false;
            document.getElementById('<%= txtfinalMr.ClientID %>').disabled = false;
            document.getElementById('<%= txtNaration.ClientID %>').disabled = false;
            document.getElementById('<%= txtDivestmentNo1.ClientID %>').disabled = false;
            document.getElementById('<%= txtDivestmentDT1.ClientID %>').disabled = false;
            document.getElementById('<%= txtDivestmentNo2.ClientID %>').disabled = false;
            document.getElementById('<%= txtDivestmentDT2.ClientID %>').disabled = false;
            document.getElementById('<%= txtDivestmentNo3.ClientID %>').disabled = false;
            document.getElementById('<%= txtDivestmentDT3.ClientID %>').disabled = false;

            $('#txtdownPayNo').val("");
            $('#txtBillNo').val("");
            $('#txtChallanNo').val("");
            $('#txtSfCode').val("");
            $('#txtModeofPayment').val("");
            $('#txtproduct').val("");
            $('#txtAdjmntNo').val("");
            $('#txtAdjmntDt').val("");
            $('#txtfinalMr').val("");
            $('#txtNaration').val("");
            $('#txtDivestmentNo1').val("");
            $('#txtDivestmentDT1').val("");
            $('#txtDivestmentNo2').val("");
            $('#txtDivestmentDT2').val("");
            $('#txtDivestmentNo3').val("");
            $('#txtDivestmentDT3').val("");

            ctxtDivestmentAmt1.SetValue("0");
            ctxtDivestmentAmt2.SetValue("0");
            ctxtDivestmentAmt3.SetValue("0");
            ctxtFinanceAmount.SetValue("0");
            ctxtDownPay1.SetValue("0");
            ctxtDownPay2.SetValue("0");
            ctxtfinalPayment.SetValue("0");
            ctxtDbdPercentage.SetValue("0");
            ctxtDbdAmount.SetValue("0");
            ctxtMbdPercentage.SetValue("0");
            ctxtMbdAmount.SetValue("0");
            ctxtProcessingFee.SetValue("0");
            ctxtTotalPay.SetValue("0");
            ctxtbalance.SetValue("0");
            ccmbStatus.SetValue("");
            ctxtBillAmount.SetValue("0");

            var strDate = new Date();
            var FinDate = new Date($('#hdfDate').val());

            cdtEntryDate.SetDate(FinDate);
            cdtDpDate.SetDate(FinDate);
            cdtBilldate.SetDate(strDate);

            cdtEntryDate.SetEnabled(false);
            cdtDpDate.SetEnabled(false);

            ccmbFinancer.SetValue("");
            cCustomerComboBox.SetValue("");

            if (ccmbBranchfilter.GetValue() == "0") {
                var branchID = '<%= Session["userbranchID"]%>'
        ccmbBranch.SetValue(branchID);
        ccmbFinancer.PerformCallback();
    }
    else {
        ccmbBranch.SetValue(ccmbBranchfilter.GetValue());
        ccmbFinancer.PerformCallback();
    }

    $('#hdfID').val("");
    $('#hdfInvoiceID').val("");

    cfinancePopup.Show();
    ccmbBranch.Focus();
    LoadingPanel.Hide();
    $("#drdExport").val(0);
}
    </script>
    <script src="JS/frm_downpaymententry.js"></script>
    <link href="CSS/frm_downpaymententry.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3 class="pull-left">Finance Reconciliation</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From Date</label></td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
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
    <div class="form_main clearfix">
        <% if (rights.CanAdd)
           { %>
        <a href="javascript:void(0);" onclick="AddnewFinance()" class="btn btn-success btn-radius"><span><u>A</u>dd New</span> </a>
        <% } %>
        <% if (rights.CanExport)
           { %>
        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <asp:ListItem Value="1">PDF</asp:ListItem>
            <asp:ListItem Value="2">XLS</asp:ListItem>
            <asp:ListItem Value="3">RTF</asp:ListItem>
            <asp:ListItem Value="4">CSV</asp:ListItem>
        </asp:DropDownList>
        <% } %>
    </div>
    <div class="relative">
        <dxe:ASPxGridView ID="downpaygrid" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cdownpaygrid" SettingsBehavior-AllowFocusedRow="true"
            SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
            DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  OnCustomCallback="downpaygrid_CustomCallback" OnSummaryDisplayText="downpaygrid_SummaryDisplayText">
            <SettingsSearchPanel Visible="True" Delay="5000" />
             <Columns>
                <%-- <dxe:GridViewDataTextColumn FieldName="SrlNo"></dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewDataTextColumn FieldName="SrlNo" SortOrder="Ascending" VisibleIndex="0" Visible="false"><Settings AllowAutoFilterTextInputTimer="False" /></dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="DownPayment Number " FieldName="DownPaymentNumber" Width="150px"
                    VisibleIndex="1" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="BillNumber" Width="150px"
                    VisibleIndex="2">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="BillDate" Width="100px"
                    VisibleIndex="3">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <%--<PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>--%>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Type" FieldName="ReconciliatonType" Width="100px"
                    VisibleIndex="4">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch" Width="150px"
                    VisibleIndex="5">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Financer" FieldName="FinancerName" Width="180px"
                    VisibleIndex="6">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="CustomerName" Width="180px"
                    VisibleIndex="7">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Finance Amount" FieldName="FinanceAmount" Width="100px"
                    VisibleIndex="8">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Bill Amount" FieldName="BillAmount" Width="100px"
                    VisibleIndex="9">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Entered by" FieldName="EntryUser" Width="100px"
                    VisibleIndex="10" Visible="False">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated by" FieldName="ModifyUser" Width="100px"
                    VisibleIndex="11">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Last Updated" FieldName="ModifyDateTime" Width="150px"
                    VisibleIndex="12">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="12" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Container.VisibleIndex %>')" class="" title="">
                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                        <% } %>

                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("ID")%>')" class="" title="" style='<%#Eval("Status")%>'>
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                        <% } %>
                            </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Visible="False" FieldName="ID"></dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Visible="False" FieldName="Invoice_Id"></dxe:GridViewDataTextColumn>
            </Columns>
            <ClientSideEvents RowClick="gridRowclick" />
               <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
            </SettingsPager>
            <Settings ShowGroupPanel="True" ShowFooter="true" ShowFilterRow="true" ShowFilterRowMenu="true" ShowHorizontalScrollBar="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
            <TotalSummary>
                <dxe:ASPxSummaryItem FieldName="FinanceAmount" SummaryType="Sum" />
                <dxe:ASPxSummaryItem FieldName="BillAmount" SummaryType="Sum" />
            </TotalSummary>
            <ClientSideEvents EndCallback="function(s,e){ GridEndCallback(s,e);}" />
        </dxe:ASPxGridView>
        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_FinanceReconciliationList" />
    </div>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="function(s,e){AllControlInitilize(s,e);}" />
    </dxe:ASPxGlobalEvents>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="financePopup" runat="server" ClientInstanceName="cfinancePopup"
            Width="1000px" HeaderText="Finance Reconciliation" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad" MinHeight="600px" ScrollBars="Vertical">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div class="row">
                                    <div class="col-md-3">
                                        <label>Unit</label>
                                        <dxe:ASPxComboBox ID="cmbBranch" runat="server" ClientInstanceName="ccmbBranch" Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="BranchChange" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Document No.</label>
                                        <asp:TextBox ID="txtdownPayNo" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Posting Date</label>
                                        <dxe:ASPxDateEdit ID="dtDpDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false" ClientInstanceName="cdtDpDate" Width="100%">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Entry Date</label>
                                        <dxe:ASPxDateEdit ID="dtEntryDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtEntryDate" Width="100%">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-3">
                                        <label>Financer</label>
                                        <dxe:ASPxComboBox ID="cmbFinancer" runat="server" ClientInstanceName="ccmbFinancer" Width="100%" OnCallback="cmbFinancer_Callback">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Bill Amount</label>
                                        <dxe:ASPxTextBox ID="txtBillAmount" ClientInstanceName="ctxtBillAmount" runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="CalculateAmount" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Bill Date</label>
                                        <dxe:ASPxDateEdit ID="dtBilldate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtBilldate" Width="100%">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Bill No</label>
                                        <asp:TextBox ID="txtBillNo" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-3">
                                        <label>Challan No.</label>
                                        <asp:TextBox ID="txtChallanNo" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Customer</label>
                                        <dxe:ASPxComboBox ID="CustomerComboBox" runat="server" EnableCallbackMode="true" CallbackPageSize="15"
                                            ValueType="System.String" ValueField="cnt_internalid" ClientInstanceName="cCustomerComboBox" Width="92%"
                                            OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL"
                                            OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{1} [{0}]"
                                            DropDownStyle="DropDown" FilterMinLength="4">
                                            <Columns>
                                                <dxe:ListBoxColumn FieldName="uniquename" Caption="Unique ID" Width="200px" />
                                                <dxe:ListBoxColumn FieldName="Name" Caption="Name" Width="200px" />
                                                <dxe:ListBoxColumn FieldName="Billing" Caption="Billing Address" Width="300px" />
                                            </Columns>
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Finance Amount</label>
                                        <dxe:ASPxTextBox ID="txtFinanceAmount" ClientInstanceName="ctxtFinanceAmount" runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>SF Code</label>
                                        <asp:TextBox ID="txtSfCode" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <label>Product</label>
                                        <asp:TextBox ID="txtproduct" runat="server" MaxLength="255"></asp:TextBox>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-3">
                                        <label>Down Payment</label>
                                        <dxe:ASPxTextBox ID="txtDownPay1" ClientInstanceName="ctxtDownPay1" runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="CalculateAmount" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Adjustment No</label>
                                        <asp:TextBox ID="txtAdjmntNo" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Adjustment Date</label>
                                        <asp:TextBox ID="txtAdjmntDt" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Adjustment Amount</label>
                                        <dxe:ASPxTextBox ID="txtDownPay2" ClientInstanceName="ctxtDownPay2" runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="CalculateAmount" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-9">
                                        <label>Mode of Payment</label>
                                        <asp:TextBox ID="txtModeofPayment" runat="server" MaxLength="500"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 hide">
                                        <label>Final M.R</label>
                                        <asp:TextBox ID="txtfinalMr" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 hide">
                                        <label>Final Payment</label>
                                        <dxe:ASPxTextBox ID="txtfinalPayment" ClientInstanceName="ctxtfinalPayment" runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-3">
                                        <label>DBD %age</label>
                                        <dxe:ASPxTextBox ID="txtDbdPercentage" ClientInstanceName="ctxtDbdPercentage" runat="server" Width="100%">
                                            <MaskSettings Mask="<0..100>.<0..99>" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="DbdPercentageCalculate" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>DBD Amount</label>
                                        <dxe:ASPxTextBox ID="txtDbdAmount" ClientInstanceName="ctxtDbdAmount" runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="DbdAmountCalculate" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>MBD %age</label>
                                        <dxe:ASPxTextBox ID="txtMbdPercentage" ClientInstanceName="ctxtMbdPercentage" runat="server" Width="100%">
                                            <MaskSettings Mask="<0..100>.<0..99>" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="MbdPercentageCalculate" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>MBD Amount</label>
                                        <dxe:ASPxTextBox ID="txtMbdAmount" ClientInstanceName="ctxtMbdAmount" runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="MbdAmountCalculate" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <%--  Disbursement Row 1--%>
                                    <div class="clear"></div>
                                    <div class="col-md-3">
                                        <label>Disbursement Number 1</label>
                                        <asp:TextBox ID="txtDivestmentNo1" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Disbursement Date 1</label>
                                        <asp:TextBox ID="txtDivestmentDT1" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Disbursement Amount 1</label>
                                        <dxe:ASPxTextBox ID="txtDivestmentAmt1" ClientInstanceName="ctxtDivestmentAmt1" runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="CalculateAmount" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="clear"></div>
                                    <%--  Disbursement Row 1 end--%>
                                    <%--  Disbursement Row 2--%>
                                    <div class="clear"></div>
                                    <div class="col-md-3">
                                        <label>Disbursement Number 2</label>
                                        <asp:TextBox ID="txtDivestmentNo2" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Disbursement Date 2</label>
                                        <asp:TextBox ID="txtDivestmentDT2" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Disbursement Amount 2</label>
                                        <dxe:ASPxTextBox ID="txtDivestmentAmt2" ClientInstanceName="ctxtDivestmentAmt2" runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="CalculateAmount" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="clear"></div>
                                    <%--  Disbursement Row 2 end--%>
                                    <%--  Disbursement Row 3--%>
                                    <div class="clear"></div>
                                    <div class="col-md-3">
                                        <label>Disbursement Number 3</label>
                                        <asp:TextBox ID="txtDivestmentNo3" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Disbursement Date 3</label>
                                        <asp:TextBox ID="txtDivestmentDT3" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Disbursement Amount 3</label>
                                        <dxe:ASPxTextBox ID="txtDivestmentAmt3" ClientInstanceName="ctxtDivestmentAmt3" runat="server" Width="100%">
                                             <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="CalculateAmount" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="clear"></div>
                                    <%--  Disbursement Row 3 end--%>
                                    <div class="clear"></div>
                                    <div class="col-md-3">
                                        <label>Process Fee</label>
                                        <dxe:ASPxTextBox ID="txtProcessingFee" ClientInstanceName="ctxtProcessingFee" runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="CalculateAmount" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Total pay</label>
                                        <dxe:ASPxTextBox ID="txtTotalPay" ClientInstanceName="ctxtTotalPay" runat="server" Width="100%" ClientEnabled="false">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                            <ClientSideEvents LostFocus="CalculateAmount" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Balance</label>
                                        <dxe:ASPxTextBox ID="txtbalance" ClientInstanceName="ctxtbalance" runat="server" Width="100%" ClientEnabled="false">
                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Status</label>
                                        <dxe:ASPxComboBox ID="cmbStatus" runat="server" ClientInstanceName="ccmbStatus" Width="100%">
                                            <Items>
                                                <dxe:ListEditItem Text="Select" Value="" />
                                                <dxe:ListEditItem Text="Excess" Value="E" />
                                                <dxe:ListEditItem Text="Clear" Value="C" />
                                                <dxe:ListEditItem Text="Short" Value="S" />
                                                <dxe:ListEditItem Text="Outstanding " Value="O" />
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <label>Narration</label>
                                        <asp:TextBox ID="txtNaration" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-3">
                                        <input type="button" value="Save" onclick="SaveOppening()" class="btn btn-primary" />
                                        <input type="button" value="Exit" onclick="Exit()" class="btn btn-danger" />
                                    </div>
                                </div>
                                <asp:HiddenField ID="hdfID" runat="server" />
                                <asp:HiddenField ID="hdfInvoiceID" runat="server" />
                                <asp:HiddenField ID="hdfDate" runat="server" />
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
    <asp:SqlDataSource ID="CustomerDataSource" runat="server"  />
    <asp:SqlDataSource runat="server" ID="dsProduct" 
        SelectCommand="select sProducts_ID,sProducts_Code,sProducts_Name from Master_sProducts"></asp:SqlDataSource>

    <dxe:ASPxGlobalEvents ID="ASPxGlobalEvents1" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
    </div>

</asp:Content>
