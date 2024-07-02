<%--=======================================================Revision History=====================================================================================================    
    1.0   Pallab    V2.0.38   20-04-2023      25868: Add Vendor Payment/Receipt module design modification
    2.0   Sanchita  V2.0.41   01-11-2023      26952: Instrument No. field in Cash/Bank Voucher will be mandatory if Bank selected in Cash/Bank
    3.0   Priti     V2.0.43   23-04-2023      0027390: Instrument No. field in Cash/Bank Voucher will be mandatory if Bank selected in Cash/BankAfter selection of "Currency "  if curser keep in Rate filed and scroll down by the mouse then value getting 9999.
    4.0   Priti     V2.0.43   05-06-2024      0027448: Invoice Selection Tab data showing outside of its area in Vendor Payment Screen.
=========================================================End Revision History===================================================================================================--%>

<%@ Page Title="VendorPaymentReceipt" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="VendorPaymentReceipt.aspx.cs" Inherits="ERP.OMS.Management.Activities.VendorPaymentReceipt" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <%-- <script type="text/javascript" src="../Activities/JS/SearchPopup.js"></script>--%>
    <script src="JS/SearchPopupDatatable.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/VendorPaymentReceipt.js?v=2.1"></script>
    <link href="CSS/VendorPaymentReceipt.css" rel="stylesheet" />
    <script>
        function SetVendor(Id, Name) {
            if (Id) {
                $('#VendorModel').modal('hide');
                ctxtVendorName.SetText(Name);
            }
            GetObjectID('hdnCustomerId').value = Id;
            var key = GetObjectID('hdnCustomerId').value;
            if (key != null && key != '') {
                // clookup_Project.gridView.Refresh();
                //cContactPerson.PerformCallback('BindContactPerson~' + key + '~ClearSession');
                $("#hdnClearSession").val('ClearSession');
                BindContactPerson(key, $("#hdnClearSession").val())
            }
            GetContactPerson();
            $('#VendorModel').modal('hide');
            tdsSectionSelectionChange();
            cContactPerson.Focus();
        }
        function OnEndCallback(s, e) {

            if (grid.cpBtnVisible != null && grid.cpBtnVisible != "") {
                grid.cpBtnVisible = null;
                BtnVisible();
            }

            if (grid.cpGridBlank == "GridBlank") {
                //cLoadingPanelCRP.Hide();
                //grid.batchEditApi.StartEdit(0, 2);
                ctxtVoucherAmount.SetValue(0.00);
                ctxtTotalPayment.SetValue(0.00);
                c_txt_Debit.SetValue(0.00);
                grid.cpGridBlank = null;
                //grid.AddNewRow();
            }

            if (grid.cpTotalAmount != null) {
                var total_receipt = grid.cpTotalAmount.split('~')[0];
                var total_payment = grid.cpTotalAmount.split('~')[1];

                c_txt_Debit.SetValue(total_receipt);
                // Arindam
                if (total_receipt != 0 && total_payment != 0) {
                    ctxtTotalPayment.SetValue(parseFloat(total_payment) - parseFloat(total_receipt));
                }
                else if (total_payment != 0) {
                    ctxtTotalPayment.SetValue(total_payment);
                }
                //ctxtTotalPayment.SetValue(total_payment);

                //ctxtVoucherAmount.SetValue(total_payment);
                grid.cpTotalAmount = null;
            }
            var value = document.getElementById('hdnRefreshType').value;
            // $("#hdnRefreshType").val("");


            var pageStatus = document.getElementById('hdnPageStatus').value;
            var IsInvoiceTagged = document.getElementById('IsInvoiceTagged').value;

            if (grid.cpSaveSuccessOrFail == "outrange") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More Voucher Number as Voucher Scheme Exausted.<br />Update The Scheme and Try Again');

            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Save as Duplicate Voucher Number.');

            }
            else if (grid.cpSaveSuccessOrFail == "AddLock") {
                cLoadingPanelCRP.Hide();
                jAlert('DATA is Freezed between ' + grid.cpAddLockStatus + ' for Add');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                // grid.AddNewRow();
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0);
                grid.cpSaveSuccessOrFail = null;
                // jAlert('Please fill Document');

                jAlert('select an outstanding Invoice to Proceed.');/*-------- Change Validatio Message Arindam 05-02-2019*/
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantityLedger") {
                // grid.AddNewRow();
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0);
                grid.cpSaveSuccessOrFail = null;
                // jAlert('Please fill Document');
                jAlert('Select an Account Head to proceed.');/*-------- Change Validatio Message Arindam 05-02-2019*/
            }
            else if (grid.cpSaveSuccessOrFail == "nullReceiptPayment") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please enter Amount to save this entry.');
            }
            else if (grid.cpSaveSuccessOrFail == "NotMatchVoucherAmount") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Mismatch detected in Voucher amount & Total amount.');
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateDocument") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can not Select duplicate document in List.');
            }
            else if (grid.cpSaveSuccessOrFail == "ProductMandatory") {
                cLoadingPanelCRP.Hide();
                grid.AddNewRow();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please Select product to calculate GST for Advance.');
            }
            else if (grid.cpSaveSuccessOrFail == "BSMandatory") {
                cLoadingPanelCRP.Hide();
                grid.AddNewRow();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Billing/Shipping is mandatory', "Alert", function () {
                    page.SetActiveTabIndex(1);
                    cbsSave_BillingShipping.Focus();
                    page.tabs[0].SetEnabled(false);
                    $("#divcross").hide();
                });

            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please try after sometime.');

            }
            else if (grid.cpSaveSuccessOrFail == "InsertProject") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please Select Project.');

            }
            else if (grid.cpSaveSuccessOrFail == "DocumentNoBlank") {
                cLoadingPanelCRP.Hide();
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please provide an unique number to proceed');
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "MandatoryInvoice") {
                cLoadingPanelCRP.Hide();
                grid.AddNewRow();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Ledger Type can only be selected with the Invoice.');

            }
            else if (grid.cpSaveSuccessOrFail == "OtherLedgerInvoice") {
                cLoadingPanelCRP.Hide();
                grid.AddNewRow();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please Select Only Invoice & Ledger .');
            }
            else if (grid.cpSaveSuccessOrFail == "MultipleLedger") {
                cLoadingPanelCRP.Hide();
                grid.AddNewRow();
                grid.cpSaveSuccessOrFail = null;

                //jAlert('More than one Account Head selection is not allowed.');
                jAlert('Only a single Ledger Account is allowed.');
            }
            else {

                var Voucher_Number = grid.cpVouvherNo;
                var Order_Msg = "Vendor Payment/Receipt No. " + Voucher_Number + " saved.";

                if (Voucher_Number != "") {
                    if ($("#HdnPrintOption").val() == "Yes" && grid.cpDocId != null) {
                        var Voucher_id = grid.cpDocId.split(",")[0];
                        var Voucher_Type = grid.cpDocId.split(",")[1];

                        onPrintJv(Voucher_id, Voucher_Type);
                    }
                    grid.cpDocId = null;
                }

                if (value == "E") {

                    if (Voucher_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [VendorPayment/Receipt]', function (r) {
                            if (r == true) {

                                grid.cpVouvherNo = null;
                                window.location.assign("VendorPaymentReceiptList.aspx");
                            }
                        });

                    }
                    else {
                        window.location.assign("VendorPaymentReceiptList.aspx");
                    }
                    if (IsInvoiceTagged == "Y") {
                        window.parent.capcReciptPopup.Hide();
                        //window.parent.cgridPendingApproval.PerformCallback();
                        window.location.assign("VendorPaymentReceiptList.aspx");
                    }

                }
                else if (value == "N") {

                    if (Voucher_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [VendorPayment/Receipt]', function (r) {

                            grid.cpVouvherNo = null;
                            if (r == true) {
                                window.location.assign("VendorPaymentReceipt.aspx?key=ADD");
                            }
                        });
                    }
                    else {
                        window.location.assign("VendorPaymentReceipt.aspx?key=ADD");
                    }
                    if (IsInvoiceTagged == "Y") {
                        window.parent.capcReciptPopup.Hide();
                        //window.parent.cgridPendingApproval.PerformCallback();
                    }
                }
                else {
                    if (pageStatus == "first") {
                        OnAddNewClick();
                        VisibleColumn();
                        grid.batchEditApi.EndEdit();
                        document.getElementById('ComboVoucherType').style.display = 'Block'
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        var VoucherType = document.getElementById("ComboVoucherType").value;

                        if (VoucherType == "R") {

                            grid.GetEditor('Payment').SetEnabled(false);
                            grid.GetEditor('Receipt').SetEnabled(true);
                        }
                        else {


                            //grid.GetEditor('Receipt').SetEnabled(false);
                            grid.GetEditor('Receipt').SetEnabled(true);// receipt field editable 05-02-2019
                            grid.GetEditor('Payment').SetEnabled(true);
                        }
                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
                            ctxtRate.SetEnabled(false);
                        }
                    }
                    else if (pageStatus == "update") {
                        OnAddNewClick();

                        $('#<%=hdnPageStatus.ClientID %>').val('');

                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
                            ctxtRate.SetEnabled(false);
                        }
                        var VoucherType = $("#ComboVoucherType").val();
                        if (VoucherType == "P") {
                            grid.GetEditor('Receipt').SetEnabled(false);
                            grid.GetEditor('Payment').SetEnabled(true);

                        }
                        else {
                            grid.GetEditor('Payment').SetEnabled(false);
                            grid.GetEditor('Receipt').SetEnabled(true);

                        }
                    }

            }

    }
    if (grid.cpView == "1") {
        viewOnly();
    }
}
function Currency_Rate() {

    var Campany_ID = '<%=Session["LastCompany"]%>';
    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
    var basedCurrency = LocalCurrency.split("~");
    var Currency_ID = cCmbCurrency.GetValue();


    if ($("#ddl_Currency").text().trim() == basedCurrency[1]) {
        ctxtRate.SetValue("");
        ctxtRate.SetEnabled(false);
    }
    else {
        //console.log("Campany_ID", Campany_ID);
        //console.log("Currency_ID", Currency_ID);
        //console.log("basedCurrency", basedCurrency[0]);
        $.ajax({
            type: "POST",
            url: "VendorPaymentReceipt.aspx/GetRate",
            data: JSON.stringify({ Campany_ID: Campany_ID, Currency_ID: Currency_ID, basedCurrency: basedCurrency[0] }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                ctxtRate.SetValue(data);
                ctxtRate.Focus();
            }
        });
        ctxtRate.SetEnabled(true);
    }
}

    </script>


    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        #grid {
            max-width: 98% !important;
        }
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate , #dtChallandate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dtPostingDate_B-1 ,
        #dtChallandate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dtPostingDate_B-1 #dtPostingDate_B-1Img ,
        #dtChallandate_B-1 #dtChallandate_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 20px;
                bottom: 8px;
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
    margin-bottom: 10px;
}

        .simple-select::after
        {
                top: 6px;
            right: -2px;
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

        #documentLookUp_DDD_PW-1
        {
            width: 97% !important;
        }

        #documentLookUp_DDD_gv
        {
            width: 100% !important;
        }

    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-title clearfix">
        <h3 class="pull-left">
            <asp:Label ID="lblHeadTitle" Text="Add Vendor Payment/Receipt" runat="server"></asp:Label>
        </h3>
        <div id="pageheaderContent" class="pull-right reverse wrapHolder content horizontal-images" style="display: none;" runat="server">
            <div class="Top clearfix">
                <ul>
                    <li>
                        <div class="lblHolder" id="divContactPhone" style="display: none;" runat="server">
                            <table>
                                <tr>
                                    <td>Contact Person's Phone</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblContactPhone" runat="server" Text="N/A" CssClass="classout"></asp:Label></td>
                                </tr>
                            </table>
                        </div>

                    </li>


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
        <div id="divcross" class="crossBtn" style="margin-left: 100px;"><a href="VendorPaymentReceiptList.aspx"><i class="fa fa-times"></i></a></div>
    </div>
        <div class="form_main  clearfix">

        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
            <TabPages>
                <dxe:TabPage Name="General" Text="General">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <div id="DivEntry">
                                <div id="divChangable" runat="server" style=" padding: 5px 0 5px 0; margin-bottom: 10px; border-radius: 4px;">
                                    <div class="">

                                        <div class="col-md-3">
                                            <label for="exampleInputName2" style="">
                                                Voucher Type <b id="bTypeText" runat="server" style="width: 20%; display: none; font-size: 12px"></b>
                                            </label>
                                            <%--Rev 1.0: "simple-select" class add --%>
                                            <div class="simple-select">
                                                <asp:DropDownList ID="ComboVoucherType" runat="server" Width="100%" onchange="rbtnType_SelectedIndexChanged()">
                                                    <asp:ListItem Text="Payment" Value="P" Selected="True" />
                                                    <asp:ListItem Text="Receipt" Value="R" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="divNumberingScheme" runat="server">
                                            <label style="">Numbering Scheme</label>
                                            <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme"
                                                SelectedIndex="0" EnableCallbackMode="true"
                                                TextField="SchemaName" ValueField="ID"
                                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True" OnCallback="CmbScheme_Callback">
                                                <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}" GotFocus="NumberingScheme_GotFocus"
                                                    EndCallback="CmbScheme_EndCallBack"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                        <div class="col-md-2 lblmTop8" style="display: none" id="divEnterBranch" runat="server">

                                            <label>Unit <span style="color: red">*</span></label>
                                            <div>
                                                <asp:DropDownList ID="ddlEnterBranch" runat="server" DataSourceID="dsBranch"
                                                    DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                                </asp:DropDownList>

                                            </div>
                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <label style="margin-top: 8px">Document No. <span style="color: red">*</span></label>
                                            <div>
                                                <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="30" onchange="UniqueCodeCheck()">                             
                                                </asp:TextBox>
                                                <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <label style="margin-top: 8px">Posting Date <span style="color: red">*</span>  </label>
                                            <div>
                                                <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom"
                                                    Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                                    <ButtonStyle Width="13px"></ButtonStyle>
                                                    <ClientSideEvents GotFocus="TransDate_GotFocus" LostFocus="function(s, e) { SetLostFocusonDemand(e)}"></ClientSideEvents>
                                                </dxe:ASPxDateEdit>
                                                <span id="MandatoryTransDate" class="iconTransDate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <%--Rev 1.0--%>
                                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                <%--Rev end 1.0--%>
                                            </div>
                                        </div>



                                        <div class="col-md-2 lblmTop8">

                                            <label>For Unit <span style="color: red">*</span></label>
                                            <%--Rev 1.0: "simple-select" class add --%>
                                            <div class="simple-select">
                                                <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" onchange="ddlBranch_SelectedIndexChanged()"
                                                    DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                                </asp:DropDownList>
                                                <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-3">
                                            <asp:RadioButtonList ID="rdl_Contact" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();">
                                                <asp:ListItem Text="Vendor" Value="DV" Selected></asp:ListItem>
                                                <asp:ListItem Text="Customer" Value="CL" Enabled="false"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <dxe:ASPxButtonEdit ID="txtVendorName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendorName" Width="100%">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>

                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDownDV(s,e);}" />
                                            </dxe:ASPxButtonEdit>
                                            <span id="MandatorysCustomer" class="iconCustomer pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                        <div class="col-md-3">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person" CssClass="inline">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px" ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <div class="col-md-2 lblmTop8" id="tdCashBankLabel">
                                            <label>Cash/Bank <span style="color: red">*</span></label>
                                            <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientIDMode="Static" ClientInstanceName="cddlCashBank" Width="100%" OnCallback="ddlCashBank_Callback">
                                                <ClientSideEvents SelectedIndexChanged="CashBank_SelectedIndexChanged" GotFocus="CashBank_GotFocus" />
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>



                                        <div class="col-md-1 lblmTop8">
                                            <label>Currency</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="CmbCurrency" EnableIncrementalFiltering="True" ClientInstanceName="cCmbCurrency"
                                                    DataSourceID="SqlCurrencyBind"
                                                    TextField="Currency_AlphaCode" ValueField="Currency_ID" SelectedIndex="0"
                                                    runat="server" ValueType="System.String" EnableSynchronization="True" Width="100%" CssClass="pull-left">
                                                    <ClientSideEvents ValueChanged="function(s,e){Currency_Rate()}" GotFocus="CurrencyGotFocus"></ClientSideEvents>
                                                </dxe:ASPxComboBox>

                                            </div>
                                        </div>
                                        <div class="col-md-1 rate lblmTop8">
                                            <label>Rate  </label>
                                            <div>
                                                <dxe:ASPxTextBox runat="server" ID="txtRate" HorizontalAlign="Right" ClientInstanceName="ctxtRate" Width="100%" CssClass="pull-left">
                                                    <%-- Rev 3.0 --%>
                                                    <%--<MaskSettings Mask="<0..9999>.<0..99999>" IncludeLiterals="DecimalSymbol"  />--%>
                                                    <MaskSettings Mask="<0..9999>.<0..99999>" IncludeLiterals="DecimalSymbol" AllowMouseWheel="false" />
                                                    <%-- Rev 3.0 End--%>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <label style="">Instrument Type</label>
                                            <div style="">
                                                <dxe:ASPxComboBox ID="cmbInstrumentType" runat="server" ClientInstanceName="cComboInstrumentTypee" Font-Size="12px"
                                                    ValueType="System.String" Width="100%" EnableIncrementalFiltering="True">
                                                    <Items>

                                                        <dxe:ListEditItem Text="Cheque" Value="C" Selected />
                                                        <dxe:ListEditItem Text="Draft" Value="D" />
                                                        <dxe:ListEditItem Text="E.Transfer" Value="E" />
                                                        <dxe:ListEditItem Text="Cash" Value="CH" />
                                                        <dxe:ListEditItem Text="Pay Order" Value="PO" />
                                                    </Items>
                                                    <ClientSideEvents SelectedIndexChanged="InstrumentTypeSelectedIndexChanged" GotFocus="InstrumentType_GotFocus" />
                                                </dxe:ASPxComboBox>

                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-3" id="divInstrumentNo" style="" runat="server">
                                           <%--Rev 2.0 [ <span style="color: red">*</span>  added] --%>
                                            <label id="" style="">Instrument No <span style="color: red">*</span></label>
                                            <div id="">
                                                <dxe:ASPxTextBox runat="server" ID="txtInstNobth" ClientInstanceName="ctxtInstNobth" Width="100%" MaxLength="30">
                                                </dxe:ASPxTextBox>
                                                <%--Rev 2.0--%>
                                                <span id="MandatoryInstNo" class="iconInstNo pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <%--End of Rev 2.0--%>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="tdIDateDiv" style="" runat="server">
                                            <label id="tdIDateLable" style="">Instrument Date</label>
                                            <div id="tdIDateValue" style="">
                                                <dxe:ASPxDateEdit ID="InstDate" runat="server" EditFormat="Custom" ClientInstanceName="cInstDate"
                                                    UseMaskBehavior="True" Font-Size="12px" Width="100%" EditFormatString="dd-MM-yyyy">
                                                    <ClientSideEvents GotFocus="InstrumentDate_GotFocus"></ClientSideEvents>
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                        </div>


                                        <div class="col-md-4 lblmTop8">
                                            <label>Narration </label>
                                            <div>
                                                <asp:TextBox ID="txtNarration" Rows="2" cols="20" runat="server" MaxLength="500" onkeydown="checkTextAreaMaxLength(this,event,'500');"
                                                    TextMode="MultiLine"
                                                    Width="100%" onchange="txtNarrationTextChanged()"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="tdsSection">
                                            <div class="col-md-2 lblmTop8">
                                                <label>TDS Section  </label>
                                                <div class="relative">
                                                    <dxe:ASPxComboBox runat="server" ID="ddl_tdsSection" DataSourceID="dsTDSSection" ValueField="tdsCode" TextField="tdsdescription" ClientInstanceName="ctdsSection" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="tdsSectionSelectionChange" />
                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-2">
                                                <div style="margin-top: 29px;">
                                                    <asp:CheckBox ID="chkNILRateTDS" runat="server" Text="NIL rate TDS?" TextAlign="Right"></asp:CheckBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <label>TDS Amount</label>
                                                <dxe:ASPxTextBox ID="txtTdsAmount" runat="server" ClientInstanceName="ctxtTdsAmount" ClientEnabled="false" DisplayFormatString="0.00" Width="100%" CssClass="pull-left">
                                                    <MaskSettings Mask="&lt;-999999999..999999999g&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </dxe:ASPxTextBox>

                                            </div>
                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <label>Voucher Amount <span style="color: red">*</span> </label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtVoucherAmount" runat="server" ClientInstanceName="ctxtVoucherAmount" Width="100%" CssClass="pull-left">
                                                    <ClientSideEvents LostFocus="function(s, e) { GetInvoiceMsg(e)}" TextChanged="SetTDSAmount" />
                                                     <%-- Rev 3.0 --%>
                                                     <%-- <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"  />--%>
                                                     <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                     <%-- Rev 3.0 End--%>
                                                    <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                                </dxe:ASPxTextBox>
                                                <span id="MandatoryVoucherAmount" class="iconVoucherAmount pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>




                                        <div class="col-md-4" style=" margin-bottom: 5px;" id="ProductSection" runat="server">
                                            <div style="height: auto; margin-bottom: 5px;">
                                                Select product to calculate GST for Advance
                                            </div>
                                            <div class="Left_Content">
                                                <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="ComponentPanel_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="productLookUp" runat="server" SelectionMode="Multiple" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                                                                KeyFieldName="Products_ID" Width="100%" TextFormatString="{0}" MultiTextSeparator=", "
                                                                ClientSideEvents-TextChanged="ProductSelected">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Name" Width="220">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60" Visible="false">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100" Visible="false">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <%-- <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataColumn>--%>
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseProductLookup" UseSubmitBehavior="False" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                            </dxe:ASPxGridLookup>
                                                            <asp:HiddenField ID="hfHSN_CODE" runat="server" />
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="ComponentPanel_EndCallBack" />
                                                </dxe:ASPxCallbackPanel>
                                            </div>

                                        </div>
                                        <div class="col-md-2" id="ProductGSTApplicableSection" runat="server" style="margin-top: 29px;">
                                            <asp:CheckBox ID="CB_GSTApplicable" runat="server" Text="GST Applicable" TextAlign="Right" onclick="return CheckedChange();"></asp:CheckBox>
                                        </div>
                                        <div class="clear"></div>
                                        <%-- Rev Tanmoy--%>

                                        <div class="col-md-2 lblmTop8">
                                            <label id="lblProject" runat="server">Project</label>
                                            <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSource"
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
                                                <%--  <ClientSideEvents GotFocus="cProject_GotFocus" CloseUp="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />--%>
                                                <ClientSideEvents GotFocus="Project_gotFocus" CloseUp="clookup_Project_LostFocus" />

                                            </dxe:ASPxGridLookup>
                                            <%--  ValueChanged="ProjectValueChange" --%>
                                            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                                                ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />

                                        </div>
                                        <%--End Rev Tanmoy--%>
                                        <%--Start Rev Hierarchy Tanmoy--%>
                                        <div class="col-md-4 lblmTop8">
                                            <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                            </dxe:ASPxLabel>
                                            <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                            </asp:DropDownList>
                                        </div>
                                        <%--End Rev Hierarchy Tanmoy--%>
                                        <div class="clear"></div>

                                    </div>
                                </div>
                            </div>

                            <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="gridBatch" KeyFieldName="ReceiptDetail_ID"
                                OnBatchUpdate="gridBatch_BatchUpdate" OnCellEditorInitialize="gridBatch_CellEditorInitialize" OnDataBinding="gridBatch_DataBinding"
                                OnCustomCallback="gridBatch_CustomCallback" OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="170">
                                <SettingsPager Visible="false"></SettingsPager>
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption=" ">
                                        <CustomButtons>
                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                            </dxe:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dxe:GridViewCommandColumn>

                                    <dxe:GridViewDataComboBoxColumn Caption="Type" FieldName="Type" VisibleIndex="1" Width="140">
                                        <PropertiesComboBox ClientInstanceName="Type" ValueField="ID" TextField="DocType"
                                            ClearButton-DisplayMode="Always" AllowMouseWheel="false">
                                            <ClientSideEvents SelectedIndexChanged="DocumentType_SelectedIndexChanged" EndCallback="Type_EndCallback" />
                                        </PropertiesComboBox>
                                    </dxe:GridViewDataComboBoxColumn>


                                    <dxe:GridViewDataButtonEditColumn FieldName="DocumentNo" Caption="Document Number" VisibleIndex="1" Width="14%">
                                        <PropertiesButtonEdit>
                                            <ClientSideEvents ButtonClick="DocumentButnClick" KeyDown="ProductKeyDown" />
                                            <Buttons>
                                                <dxe:EditButton Text="..." Width="20px">
                                                </dxe:EditButton>
                                            </Buttons>
                                        </PropertiesButtonEdit>
                                    </dxe:GridViewDataButtonEditColumn>
                                    <dxe:GridViewDataTextColumn FieldName="DocumentID" Caption="DocumentID" VisibleIndex="17" ReadOnly="True" Width="0"
                                        EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                                        PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn FieldName="ProjectId" Caption="ProjectId" VisibleIndex="16" ReadOnly="True" Width="0"
                                        EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                                        PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Receipt" FieldName="Receipt" Width="130">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" MaskSettings-AllowMouseWheel="false">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"  />
                                            <ClientSideEvents KeyDown="OnKeyDown" LostFocus="ReceiptTextChange"
                                                GotFocus="function(s,e){
                                                        DebitGotFocus(s,e); 
                                                        }" />
                                            <ClientSideEvents />
                                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Payment" FieldName="Payment" Width="130">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false"/>
                                            <ClientSideEvents KeyDown="OnKeyDown" LostFocus="PaymentTextChange"
                                                GotFocus="function(s,e){
                                                        CreditGotFocus(s,e);
                                                        }" />
                                            <ClientSideEvents />
                                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                            <%-- <ClientSideEvents LostFocus="PaymentLostFocus"
                                                        GotFocus="PaymentgotFocus" />--%>
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <%--  Chinmoy Added new column Project Code Start 10-12-2019--%>

                                    <dxe:GridViewDataButtonEditColumn FieldName="Project_Code" Caption="Project Code" VisibleIndex="5" Width="14%">
                                        <PropertiesButtonEdit>
                                            <ClientSideEvents ButtonClick="ProjectCodeButnClick" KeyDown="ProjectCodeKeyDown" />
                                            <Buttons>
                                                <dxe:EditButton Text="..." Width="20px">
                                                </dxe:EditButton>
                                            </Buttons>
                                        </PropertiesButtonEdit>
                                    </dxe:GridViewDataButtonEditColumn>


                                    <%-- End--%>

                                    <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Remarks" FieldName="Remarks" Width="200">
                                        <PropertiesTextEdit>
                                            <%-- <ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="ReceiptDetail_ID" Caption="Srl No" ReadOnly="true" VisibleIndex="7" Width="0">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn FieldName="IsOpening" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="6" Caption=" ">
                                        <CustomButtons>
                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                            </dxe:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dxe:GridViewCommandColumn>
                                </Columns>
                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" />
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                </SettingsEditing>
                                <%--<SettingsBehavior ColumnResizeMode="Disabled"   />--%>
                            </dxe:ASPxGridView>
                            <div class="text-center">
                                <table style="margin-left: 40%; margin-top: 10px">
                                    <tr>
                                        <td style="padding-right: 50px"><b>Total Amount</b></td>
                                        <td style="width: 203px;">
                                            <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                            </dxe:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txtTotalPayment" runat="server" Width="105px" ClientInstanceName="ctxtTotalPayment" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td style="padding-right: 50px">
                                            <asp:Label ID="lbltaxAmountHeader" runat="server" Text="Total Taxable Amount" Font-Bold="true"></asp:Label></td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txtTaxAmount" runat="server" Width="105px" ClientInstanceName="ctxtTaxAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <table style="width: 100%;">
                                <tr>
                                    <td style="padding: 5px 0;">
                                        <dxe:ASPxLabel ID="validation" runat="server" Text="" ForeColor="Red" Visible="false"></dxe:ASPxLabel>

                                        <b><%--<span id="Span1" runat="server" style="display: none; color: red">This Vendor Debit/Credit Note is tagged with another module,  <span id="spanTaggedDocNo" runat="server"></span>Cannot Modify data!!</span>--%></b>

                                        <span id="tdSaveButtonNew" runat="server">
                                            <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveNew" runat="server"
                                                AutoPostBack="false" CssClass="btn btn-success" TabIndex="0" Text="Save & N&#818;ew"
                                                UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveButtonClickNew();}" />
                                            </dxe:ASPxButton>

                                        </span>
                                        <span id="tdSaveButton" runat="server">
                                            <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server"
                                                AutoPostBack="false" CssClass="btn btn-success" TabIndex="0" Text="Save & Ex&#818;it"
                                                UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                            </dxe:ASPxButton>

                                        </span>
                                        <span id="tdUdfButton" runat="server">
                                            <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" UseSubmitBehavior="False"
                                                CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                                <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                            </dxe:ASPxButton>
                                        </span>
                                        <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                        </dxe:ASPxButton>
                                        <b><span id="tagged" runat="server" style="display: none; color: red">This Vendor Payment Receipt is tagged in other modules or already reconciled. Cannot Modify data</span></b>
                                    </td>


                                </tr>
                            </table>

                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="Billing/Shipping" Text="Our Billing/Shipping">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <ucBS:BillingShippingControl runat="server" ID="BillingShippingControl" />
                            <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="VPR" />
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


        <%-- Sales Invoice PopUp Start--%>
        <dxe:ASPxPopupControl ID="Popup_invoice" runat="server" ClientInstanceName="cPopup_invoice"
            Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <HeaderTemplate>
                <strong><span style="color: #fff">Select Invoice</span></strong>
                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                    <ClientSideEvents Click="function(s, e){ 
                                                            cPopup_invoice.Hide();
                                                        }" />
                </dxe:ASPxImage>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <input type="button" value="Select All Invoice" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="De-select All Invoice" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                    </div>
                    <dxe:ASPxGridView runat="server" KeyFieldName="DocumentID" ClientInstanceName="cgrid_SalesInvoice" ID="grid_SalesInvoice"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                        OnCustomCallback="grid_SalesInvoice_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Caption=" " VisibleIndex="0" />

                            <dxe:GridViewDataColumn VisibleIndex="1" FieldName="DocDate" Caption="Document Date" Width="100">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" ReadOnly="true" Caption="Invoice No" FieldName="DocumentNo" Width="150">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" ReadOnly="true" Caption="Party Invoice No." FieldName="PartyInvoiceNo" Width="150">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" ReadOnly="true" Caption="Party Inv. Date" FieldName="PartyInvoiceDate" Width="100">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataColumn VisibleIndex="3" FieldName="branch" Caption="Unit" Width="100">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="2" ReadOnly="true" Caption="Customer">
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" ReadOnly="true" Caption="Balance Amount" FieldName="Payment" HeaderStyle-HorizontalAlign="Right">
                            </dxe:GridViewDataTextColumn>

                            <%-- <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                        </dxe:GridViewDataTextColumn>--%>
                        </Columns>
                        <ClientSideEvents EndCallback="grid_SalesInvoiceOnEndCallback" />
                        <SettingsDataSecurity AllowEdit="true" />
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <%--<asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />--%>
                        <dxe:ASPxButton ID="Btn" ClientInstanceName="cbtnOK" runat="server"
                            AutoPostBack="false" CssClass="btn btn-primary  mLeft mTop" Text="OK"
                            UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {PerformCallToGridBind();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <%-- Sales Invoice PopUp End--%>


        <%--Chinmoy added inline Project code start 10-12-2019--%>

        <dxe:ASPxPopupControl ID="ProjectCodePopup" runat="server" ClientInstanceName="cProjectCodePopup"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
            Width="700" HeaderText="Select Document Number" AllowResize="true" ResizingMode="Postponed" Modal="true">
            <%--  <headertemplate>
                <span>Select Document Number</span>
            </headertemplate>--%>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <label><strong>Search By Document Number</strong></label>
                    <%--   <span style="color: red;">[Press ESC key to Cancel]</span>--%>
                    <dxe:ASPxCallbackPanel runat="server" ID="ProjectCodeCallback" ClientInstanceName="cProjectCodeCallback"
                        OnCallback="ProjectCodeCallback_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookupPopup_ProjectCode" runat="server" ClientInstanceName="clookupPopup_ProjectCode" Width="800"
                                    KeyFieldName="ProjectId" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProjectCodeSelected"
                                    ClientSideEvents-KeyDown="lookup_ProjectCodeKeyDown" OnDataBinding="lookup_ProjectCode_DataBinding">
                                    <Columns>

                                        <%--   <dxe:GridViewDataColumn FieldName="Proj_Id" Visible="true" VisibleIndex="0" Caption="Project id" Settings-AutoFilterCondition="Contains" Width="200px">
                                                    </dxe:GridViewDataColumn>--%>
                                        <dxe:GridViewDataColumn FieldName="ProjectCode" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Hierarchy_ID" Visible="true" VisibleIndex="5" Caption="Hierarchy_ID" Settings-AutoFilterCondition="Contains" Width="0">
                                        </dxe:GridViewDataColumn>


                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                </dxe:ASPxGridLookup>
                                <%--   <dx:LinqServerModeDataSource ID="EntityServerModeDataProjectQuotation" runat="server" OnSelecting="EntityServerModeDataProjectQuotation_Selecting"
                                                ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />--%>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="ProjectCodeCallback_endcallback" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
        </dxe:ASPxPopupControl>



        <%--//End--%>



        <%--Batch Product Popup Start--%>
            <%--REV 4.0  add GridViewProperties-Settings-HorizontalScrollBarMode="Auto" for ASPxGridLookup "documentLookUp" & change Height,Width for ASPxPopupControl--%>
        <dxe:ASPxPopupControl ID="DocumentpopUp" runat="server" ClientInstanceName="cDocumentpopUp"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="430px"
            Width="800px" HeaderText="Select Document Number" AllowResize="true" ResizingMode="Postponed" Modal="true">
            <%--  <headertemplate>
                <span>Select Document Number</span>
            </headertemplate>--%>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <label><strong>Search By Document Number</strong></label>
                    <span style="color: red;">[Press ESC key to Cancel]</span>
                    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDocumentNo" ClientInstanceName="cCallbackPanelDocumentNo" 
                        OnCallback="CallbackPanelDocumentNo_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="documentLookUp" runat="server" ClientInstanceName="cdocumentLookUp" Width="600"
                                    KeyFieldName="DocumentID" TextFormatString="{1}" MultiTextSeparator=", " ClientSideEvents-TextChanged="DocumentSelected"
                                    ClientSideEvents-KeyDown="DocumentlookUpKeyDown" OnDataBinding="documentLookUp_DataBinding" GridViewProperties-Settings-HorizontalScrollBarMode="Auto">
                                    <Columns>

                                        <dxe:GridViewDataColumn FieldName="DocDate" Caption="Document Date" Width="17%">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>


                                        <dxe:GridViewDataColumn FieldName="DocumentNumber" Caption="Document Number" Width="17%">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="PartyInvoiceNo" Caption="Party Invoice No" Width="17%">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="PartyInvoiceDate" Caption="Party Inv. Date" Width="17%">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="branch" Caption="Unit" Width="16%">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="UnPaidAmount" Caption="UnPaid Amount" Width="16%">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>


                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="CallbackPanelDocumentNo_endcallback" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
        </dxe:ASPxPopupControl>

        <%--  <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="proc_getDocumentDetails" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="All" />
                </SelectParameters>
            </asp:SqlDataSource>--%>

        <%--Batch Product Popup End--%>

        <%--TDS Section --%>

        <dxe:ASPxPopupControl ID="inventorypopup" runat="server" ClientInstanceName="cinventorypopup"
            Width="1080px" HeaderText="Select TDS" PopupHorizontalAlign="WindowCenter" ShowCloseButton="false"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <HeaderTemplate>
                <strong><span style="color: #fff">Select TDS</span></strong>
                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                    <ClientSideEvents Click="function(s, e){ 
                                                            cinventorypopup.Hide();
                                                        }" />
                </dxe:ASPxImage>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                    <div class="row">
                        <div class="col-md-3">
                            <label><span><strong>Select Unit</strong></span></label>
                            <dxe:ASPxComboBox ID="ddl_noninventoryBranch" ClientInstanceName="cddl_noninventoryBranch" runat="server" SelectedIndex="-1"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                ClearButton-DisplayMode="Always" DataSourceID="SDSBranch" TextField="BANKBRANCH_NAME" ValueField="BANKBRANCH_ID">
                            </dxe:ASPxComboBox>

                        </div>
                        <div class="col-md-3">
                            <label><span><strong>Select Month for TDS</strong></span></label>
                            <dxe:ASPxComboBox ID="ddl_month" ClientInstanceName="cddl_month" runat="server" SelectedIndex="-1"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                ClearButton-DisplayMode="Always">
                                <Items>
                                    <dxe:ListEditItem Text="April" Value="April" />
                                    <dxe:ListEditItem Text="May" Value="May" />
                                    <dxe:ListEditItem Text="June" Value="June" />
                                    <dxe:ListEditItem Text="July" Value="July" />
                                    <dxe:ListEditItem Text="August" Value="August" />
                                    <dxe:ListEditItem Text="September" Value="September" />
                                    <dxe:ListEditItem Text="October" Value="October" />
                                    <dxe:ListEditItem Text="November" Value="November" />
                                    <dxe:ListEditItem Text="December" Value="December" />
                                    <dxe:ListEditItem Text="January" Value="January" />
                                    <dxe:ListEditItem Text="February" Value="February" />
                                    <dxe:ListEditItem Text="March" Value="March" />
                                </Items>
                            </dxe:ASPxComboBox>
                        </div>

                        <div class="col-md-3">
                            <label><span><strong>Product Basic Amount</strong></span></label>
                            <div style="padding-bottom: 5px">
                                <dxe:ASPxTextBox ID="txt_proamt" MaxLength="80" ClientInstanceName="ctxt_proamt" ReadOnly="true"
                                    runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="cityDiv" style="height: auto;">

                                <asp:Label ID="Label15" runat="server" Text="TDS Section" CssClass="newLbl"></asp:Label>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="cmb_tdstcs" ClientInstanceName="cmb_tdstcs" DataSourceID="tdstcs" Width="100%" ItemStyle-Wrap="True"
                                    ClearButton-DisplayMode="Always" runat="server" TextField="tdscode" ValueField="TDSTCS_ID">
                                    <ClientSideEvents SelectedIndexChanged="TDS_SelectedIndexChanged" />
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <table style="width: 100%;">
                        <tr>
                        </tr>
                        <tr class="cgridTaxClass">
                            <td colspan="4">
                                <dxe:ASPxGridView runat="server" KeyFieldName="TDSID" ClientInstanceName="cgridinventory" ID="gridinventory"
                                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                    OnCustomCallback="gridinventory_CustomCallback"
                                    Settings-ShowFooter="false" AutoGenerateColumns="False">
                                    <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <Columns>

                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="TDSRate" ReadOnly="true" Caption="TDS Rate(%)" Width="8%">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="TDS amount" FieldName="TDSAmount" VisibleIndex="3" Width="8%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">

                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="SurchargeRate" ReadOnly="true" Caption="Surcharge Rate(%)" Width="11%">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Surcharge amount" FieldName="SurchargeAmount" VisibleIndex="5" Width="11%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">

                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="EducationCessRate" ReadOnly="true" Caption="Education Cess Rate(%)" Width="14%">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Education Cess Amount" FieldName="EducationCessAmt" VisibleIndex="7" Width="14%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">

                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="HgrEducationCessRate" ReadOnly="true" Caption="Higher Education Cess Rate(%)" Width="17%">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Higher Education Cess Amount" FieldName="HgrEducationCessAmt" VisibleIndex="9" Width="17%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">

                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <%--  <ClientSideEvents EndCallback="OnInventoryEndCallback" />--%>
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="pull-left">

                                    <dxe:ASPxButton ID="btn_noninventoryOk" ClientInstanceName="cbtn_noninventoryOk" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return NonInventoryBatchUpdate();}" />
                                    </dxe:ASPxButton>

                                </div>
                                <table class="pull-right">
                                    <tr>
                                        <td style="padding-top: 10px; padding-right: 5px"><strong>Total TDS</strong></td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txt_totalnoninventoryproductamt" MaxLength="80" ClientInstanceName="ctxt_totalnoninventoryproductamt"
                                                Text="0.00" ReadOnly="true"
                                                runat="server" Width="100%" CssClass="pull-left mTop">
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
        <asp:HiddenField ID="hdntdschecking" runat="server" />
        <%--TDS Section --%>
    </div>
        <div id="DivHiddenField">
        <asp:HiddenField ID="hdnBtnClick" runat="server" />
        <asp:HiddenField ID="hdnInstrumentType" runat="server" />
        <asp:HiddenField ID="hdnPageStatus" runat="server" />
        <asp:HiddenField ID="hdnSchemaType" runat="server" />
        <asp:HiddenField ID="hdfIsDelete" runat="server" />
        <asp:HiddenField ID="hdn_Mode" runat="server" />
        <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="IsInvoiceTagged" runat="server" />
        <asp:HiddenField ID="hdndocumentno" runat="server" Value="0" />
        <asp:HiddenField ID="hdnCustomerId" runat="server" />

        <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
        <asp:HiddenField ID="hdnAllowProjectInDetailsLevel" runat="server" />
        <asp:HiddenField ID="hdnEditProjId" runat="server" />
        <%--for Project  --%>
    </div>

        <div id="DivDataSource">
        <%-- <asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand=""></asp:SqlDataSource>--%>
        <asp:SqlDataSource ID="SqlCurrencyBind" runat="server"></asp:SqlDataSource>
        <asp:SqlDataSource ID="dsBranch" runat="server"
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>
    </div>
        <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="grid"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    </div>
    <%--UDF Popup --%>
    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
        Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%--Tax PopUP Start--%>
    <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
        Width="850px" HeaderText="Vendor Payment/Receipt Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <span style="color: #fff"><strong>Vendor Payment/Receipt Tax</strong></span>
            <dxe:ASPxImage ID="ASPxImage31" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            cgridTax.CancelEdit();
                                                            caspxTaxpopUp.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <asp:HiddenField runat="server" ID="setCurrentProdCode" />
                <asp:HiddenField runat="server" ID="HdSerialNo" />
                <asp:HiddenField runat="server" ID="HdnPrintOption" />
                <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
                <asp:HiddenField runat="server" ID="HdProdNetAmt" />
                <div id="content-6">
                    <div class="col-sm-3">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Entered Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
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
                                        <td>
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
                                        <td>
                                            <dxe:ASPxLabel ID="lblTaxDiscount" runat="server" Text="" ClientInstanceName="clblTaxDiscount"></dxe:ASPxLabel>
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
                                        <td>Net Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
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
                                        <td>
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
                                <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </td>
                    </tr>

                    <tr class="cgridTaxClass">
                        <td colspan="3">
                            <dxe:ASPxGridView runat="server" OnBatchUpdate="taxgrid_BatchUpdate" KeyFieldName="Taxes_ID" ClientInstanceName="cgridTax" ID="aspxGridTax"
                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridTax_CustomCallback"
                                Settings-ShowFooter="false" AutoGenerateColumns="False" OnCellEditorInitialize="aspxGridTax_CellEditorInitialize" OnHtmlRowCreated="aspxGridTax_HtmlRowCreated"
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
                                    <%--<dxe:GridViewDataComboBoxColumn Caption="percentage" FieldName="TaxField" VisibleIndex="2">
                                                <PropertiesComboBox ClientInstanceName="cTaxes_Name" ValueField="Taxes_ID" TextField="Taxes_Name" DropDownStyle="DropDown">
                                                    <ClientSideEvents SelectedIndexChanged="cmbtaxCodeindexChange"
                                                        GotFocus="CmbtaxClick" />
                                                </PropertiesComboBox>
                                            </dxe:GridViewDataComboBoxColumn>--%>
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
                                <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch">
                                    <BatchEditSettings EditMode="row" />
                                </SettingsEditing>
                                <ClientSideEvents EndCallback=" cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />

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
                                            <ClientSideEvents SelectedIndexChanged="cmbGstCstVatChange"
                                                GotFocus="CmbtaxClick" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="padding-left: 15px; padding-top: 10px; padding-bottom: 15px; padding-right: 25px">
                                        <dxe:ASPxTextBox ID="txtGstCstVat" MaxLength="80" ClientInstanceName="ctxtGstCstVat" ReadOnly="true" Text="0.00"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
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
                        <td colspan="3">
                            <div class="pull-left">
                                <asp:Button ID="Button2" runat="server" Text="Close" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
                                <asp:Button ID="Button1" runat="server" Text="Ok" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" Visible="false" />

                                <span id="taxroundedOf"></span>
                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                            <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" /> --%>
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
    <%--Tax PopUP End--%>
    <asp:HiddenField runat="server" ID="IsUdfpresent" />
    <asp:HiddenField runat="server" ID="Keyval_internalId" />
    <%--UDF Popup End--%>
    <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="acpCheckAmount" ClientInstanceName="cacpCheckAmount" OnCallback="acpCheckAmount_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="aacpCheckAmountEndCall" />
    </dxe:ASPxCallbackPanel>


    <asp:SqlDataSource runat="server" ID="ProductDataSource"
        SelectCommand="prc_CustomerReceiptPaymentDetails" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="tdstcs" runat="server"
        SelectCommand="Select  TDSTCS_ID,ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' as tdsdescription ,ltrim(rtrim(tdstcs_code)) tdscode  from master_tdstcs "></asp:SqlDataSource>
    <asp:SqlDataSource ID="SDSBranch" runat="server"
        SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH"></asp:SqlDataSource>

    <asp:SqlDataSource ID="dsTDSSection" runat="server"
        SelectCommand="SELECT * FROM (
		    Select  ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' as tdsdescription ,ltrim(rtrim(tdstcs_code)) tdscode 
		    from master_tdstcs tds inner join (SELECT TDSTCSRates_Code from Config_MULTITDSTCSRates GROUP BY TDSTCSRates_Code) rate on rate.TDSTCSRates_Code=tds.TDSTCS_Code	
		    UNION 
			    SELECT 'Select' as tdsdescription , '0' tdscode
		    ) TBL Order By tdscode
    "></asp:SqlDataSource>


    <%--For Print--%>


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
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>





    <asp:HiddenField runat="server" ID="hdnSalesInvoice" />
    <asp:HiddenField runat="server" ID="hdnEnableUnit" />
    <dxe:ASPxLoadingPanel ID="LoadingPanelCRP" runat="server" ClientInstanceName="cLoadingPanelCRP"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <div class="modal fade" id="VendorModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <%-- <h4 class="modal-title">Vendor Search</h4>--%>
                    <h4 class="modal-title">
                        <label id="VendorModelName"></label>
                    </h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Vendorkeydown(event)" id="txtVendSearch" autofocus width="100%" placeholder="Search By Vendor Name or Unique Id" />
                    <div id="VendorTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Name</th>
                                <th>Unique Id</th>
                                <th>Type</th>
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



    <div class="modal fade" id="MainAccountModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeModal();">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydown(event)" id="txtMainAccountSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Subledger Type</th>
                                <th>Reverse Applicable</th>
                                <th>HSN/SAC</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="closeModal();">Close</button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnTDSRate" />
    <asp:HiddenField runat="server" ID="hdnClearSession" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />

    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />


    <asp:HiddenField ID="hdnAddDataFFrom" runat="server" />
    <asp:HiddenField ID="hdnAddDataFTo" runat="server" />
    <asp:HiddenField ID="hdnAddDataFFromCon" runat="server" />
    <asp:HiddenField ID="hdnAddDataFToCon" runat="server" />

</asp:Content>
