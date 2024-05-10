<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                04-04-2023        2.0.37           Pallab              25882: Cash/Bank Voucher Add module design modification
2.0                11-05-2023        2.0.38           Sanchita            In the TDS Challan module(Cash/Bank) the FY 23-24 is missing. Refer: 26091     
3.0                01-11-2023        V2.0.41          Sanchita            26952: Instrument No. field in Cash/Bank Voucher will be mandatory if Bank selected in Cash/Bank
4.0                10-05-2024        V2.0.43          Priti               0027390: Instrument No. field in Cash/Bank Voucher will be mandatory if Bank selected in Cash/BankAfter selection of "Currency "  if curser keep in Rate filed and scroll down by the mouse then value getting 9999.

====================================================== Revision History =============================================--%>

<%@ Page Title="Cash/Bank Voucher" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_CashBankEntry" CodeBehind="CashBankEntry.aspx.cs" %>

<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>--%>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>

    <link rel="stylesheet" href="http://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <script src="http://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="JS/CashBankEntry.js?v=4.2"></script>

    <script>
        var currentEditableVisibleIndex;
        var lastMainAccountID;
        var setValueFlag;



        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
            }
        }


        function InstrumentDateChange() {
            var SelectedDate = new Date(cInstDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);
            ///Checking of Transaction Date For MaxLockDate
            var MaxLockDate = new Date('<%=Session["LCKBNK"]%>');
    monthnumber = MaxLockDate.getMonth();
    monthday = MaxLockDate.getDate();
    year = MaxLockDate.getYear();
    var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();

    if (SelectedDateValue <= MaxLockDateNumeric) {
        jAlert('This Entry Date has been Locked.');
        MaxLockDate.setDate(MaxLockDate.getDate() + 1);
        cInstDate.SetDate(MaxLockDate);
        return;
    }
    ///End Checking of Transaction Date For MaxLockDate


    ///Date Should Between Current Fin Year StartDate and EndDate
    var FYS = "<%=Session["FinYearStart"]%>";
    var FYE = "<%=Session["FinYearEnd"]%>";
    var LFY = "<%=Session["LastFinYear"]%>";
    var FinYearStartDate = new Date(FYS);
    var FinYearEndDate = new Date(FYE);
    var LastFinYearDate = new Date(LFY);

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
        //                   alert('Between');
    }
    else {
        // jAlert('Enter Date Is Outside Of Financial Year !!');
        jAlert('Enter Date Is Outside Of Financial Year !!', 'Alert Dialog: [CashBank]', function (r) {
            if (r == true) {
                //cdtTDate.Focus();
            }
        });
        if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
            cInstDate.SetDate(new Date(FinYearStartDate));
        }
        if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
            cInstDate.SetDate(new Date(FinYearEndDate));
        }
    }
    ///End OF Date Should Between Current Fin Year StartDate and EndDate
        }
        function Currency_Rate() {

            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var Currency_ID = cCmbCurrency.GetValue();
            $('#<%=hdnCurrenctId.ClientID %>').val(Currency_ID);


    if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
        ctxtRate.SetValue("");
        ctxtRate.SetEnabled(false);
    }
    else {
        $.ajax({
            type: "POST",
            url: "ContraVoucher.aspx/GetRate",
            data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                ctxtRate.SetValue(data);


            }
        });

        ctxtRate.SetEnabled(true);
    }
        }
        function OnEndCallback(s, e) {
            IntializeGlobalVariables(s);
            LoadingPanel.Hide();
            $('#<%=hdnDeleteSrlNo.ClientID %>').val('0');
    var pageStatus = document.getElementById('hdnPageStatus').value;

    var ViewStatus = document.getElementById('hdnView').value;

    if ($('#<%=hdnPayment.ClientID %>').val() == "YES") {
        $('#<%=hdnPayment.ClientID %>').val("NO");
        OnAddNewClick();
    }
    if (InsgridBatch.cpRefcahbankClear == "RefcahbankClear") {
        InsgridBatch.cpRefcahbankClear = null;
        OnAddNewClick();
    }

    if (InsgridBatch.cpTotalAmount != null) {
        var total_receipt = InsgridBatch.cpTotalAmount.split('~')[0];
        var total_payment = InsgridBatch.cpTotalAmount.split('~')[1];
        c_txt_Debit.SetValue(total_receipt);
        ctxtTotalPayment.SetValue(total_payment);
        InsgridBatch.cpTotalAmount = null;
    }
    if (InsgridBatch.cpSaveSuccessOrFail == "outrange") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('The Numbering Scheme has exhausted, please update the Scheme and try adding the Voucher');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
    }                //Rev Rajdip
    if (InsgridBatch.cpSaveSuccessOrFail == "DuplicateMainOrSubAcnt") {
        InsgridBatch.cpvalidateduplicate = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Same Main Account and Sub Account already entered. Cannot Duplicate.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
    }                //End Rev Rajdip
    else if (InsgridBatch.cpSaveSuccessOrFail == "duplicate") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Can not save as duplicate Voucher No.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
        //document.getElementById('btnSaveNew').style.display = 'none';
        //document.getElementById('btnSaveRecords').style.display = 'none';
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "DuplicateMainAccount") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Same Main Account already entered. Cannot Duplicate.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "DuplicateSubAccount") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Same Sub Account already entered. Cannot Duplicate.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "zeroAmount") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Cannot save with ZERO Amount.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "SubAccountMandatory") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Sub-account ledger selection is set as mandatory in System Settings.\n Please select Sub-account ledger to proceed.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "errorInsert") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Try again later.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
        //document.getElementById('btnSaveNew').style.display = 'none';
        //document.getElementById('btnSaveRecords').style.display = 'none';
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "EmptyProject") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Please select project.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);

    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "AddLock") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('DATA is Freezed between ' + InsgridBatch.cpAddLockStatus + ' for Add.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);

    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "mixedvalue") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('You must select all the ledgers either with Reverse Charge Applicable or all the ledgers </br>without Reverse charge applicable in a single entry to Save this entry.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
        //document.getElementById('btnSaveNew').style.display = 'none';
        //document.getElementById('btnSaveRecords').style.display = 'none';

    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "reverserequired") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Selected Ledger(s) are mapped with Reverse Charge, please check Reverse Charge option.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
        //document.getElementById('btnSaveNew').style.display = 'none';
        //document.getElementById('btnSaveRecords').style.display = 'none';
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "reversenotrequired") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Selected Ledger(s) are not mapped with Reverse Charge, please un-check Reverse Charge option.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
        //document.getElementById('btnSaveNew').style.display = 'none';
        //document.getElementById('btnSaveRecords').style.display = 'none';
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "reversetaxledgermissing") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('reversetaxledgermissing');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
        //document.getElementById('btnSaveNew').style.display = 'none';
        //document.getElementById('btnSaveRecords').style.display = 'none';
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "addressrequired") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;

        jAlert("Please Enter Billing/Shipping and GSTIN Details to Calculate GST.", "Alert !!", function () {
            page.SetActiveTabIndex(1);
            cbsSave_BillingShipping.Focus();
            page.tabs[0].SetEnabled(false);
            $("#divcross").hide();
        });
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
        //document.getElementById('btnSaveNew').style.display = 'none';
        //document.getElementById('btnSaveRecords').style.display = 'none';

    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "taxREquired") {
        InsgridBatch.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        chkAccount = 1;
        jAlert('Selected Ledger is tagged for GST calculation. Click on Charges to calculate GST and Proceed.');
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);
        //document.getElementById('btnSaveNew').style.display = 'none';
        //document.getElementById('btnSaveRecords').style.display = 'none';
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "successInsert") {

        if (InsgridBatch.cpVouvherNo != null) {
            var JV_Number = InsgridBatch.cpVouvherNo;
            var value = document.getElementById('hdnRefreshType').value;
            var JV_Msg = "Cash/Bank Voucher No. " + JV_Number + " generated.";
            var strSchemaType = document.getElementById('hdnSchemaType').value;

            if (value == "E") {
                if (JV_Number != "") {
                    var Type = InsgridBatch.cpType;
                    var newInvoiceId = InsgridBatch.cpAutoID;

                    //if (newInvoiceId == "-20")
                    //{
                    //    var mismatch_Msg = 'Mismatch in Debit & Credit Posting.';
                    //    jAlert(mismatch_Msg, 'Alert Dialog: [CashBank]', function (r) {
                    //        if (r == true) {                                       
                    //        }
                    //    });
                    //}
                    //else
                    //{
                    var AutoPrint = document.getElementById('hdnAutoPrint').value;
                    if (AutoPrint == "Yes") {
                        if (Type == "P") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PaymentVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                        }
                        else {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ReceiptVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                        }
                    }
                    if (strSchemaType == '1') {
                        jAlert(JV_Msg, 'Alert Dialog: [CashBank]', function (r) {
                            if (r == true) {
                                window.location.assign("CashBankEntryList.aspx");
                            }
                        });
                    }
                    else {
                        window.location.assign("CashBankEntryList.aspx");
                    }
                    //}                           

                }
                else {
                    // window.location.reload();
                    window.location.assign("CashBankEntryList.aspx");
                }
            }
            else if (value == "S") {
                if (JV_Number != "") {
                    var Type = InsgridBatch.cpType;
                    var newInvoiceId = InsgridBatch.cpAutoID;
                    var AutoPrint = document.getElementById('hdnAutoPrint').value;
                    if (AutoPrint == "Yes") {
                        if (Type == "P") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PaymentVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                        }
                        else {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ReceiptVoucher~D&modulename=CBVUCHR&id=" + newInvoiceId, '_blank');
                        }
                    }

                    // if (strSchemaType == '1') {
                    jAlert(JV_Msg, 'Alert Dialog: [CashBank]', function (r) {
                        if (r == true) {
                            window.location.assign("CashBankEntry.aspx?key=ADD");
                        }
                    });
                    //jAlert(JV_Msg, "Alert!!", function () {
                    //});
                    // }
                }
                else {
                    // window.location.reload();
                    window.location.assign("CashBankEntry.aspx?key=ADD");
                }
            }
            // InsgridBatch.cpVouvherNo = null;
        }
        if ($('#<%=hdnBtnClick.ClientID %>').val() == "Save_Exit") {
            if (InsgridBatch.cpExitNew == "YES") {
                //window.location.reload();
                //window.location.assign("CashBankEntryList.aspx");
            }
            else {
                OnAddNewClick();
            }
            InsgridBatch.cpType = null;
            InsgridBatch.cpAutoID = null;
        }
        if ($('#<%=hdnBtnClick.ClientID %>').val() == "Save_New") {

            //'''''''''''''''''''''''''''Commented for New Page
            $("#divNumberingScheme").show();
            $("#divEnterBranch").hide();
            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            cCmbCurrency.SetValue(basedCurrency[0]);
            c_txt_Debit.SetValue("0.0");
            ctxtTotalPayment.SetValue("0.0");
            ctxtRate.SetValue("");
            cddl_AmountAre.SetEnabled(true);
           // cddl_AmountAre.SetValue(3);
            //window.location.assign("CashBankEntry.aspx?key=ADD");

            //'''''''''''''''''''''''end''''''''''''''''''''''''''''''''''''''''''''''

            //if (cComboInstrumentTypee.GetValue() == "CH") {
            //    document.getElementById("divInstrumentNo").style.display = 'none';
            //    document.getElementById("tdIDateDiv").style.display = 'none';
            //}
            //else {
            //    document.getElementById("divInstrumentNo").style.display = 'block';
            //    document.getElementById("tdIDateDiv").style.display = 'block';
            //}

            <%--$('#<%=lblHeading.ClientID %>').text("");
            $('#<%=lblHeading.ClientID %>').text("Add Cash/Bank Voucher");

            deleteAllRows();
            OnAddNewClick();
            if (document.getElementById('txtVoucherNo').value == "Auto") {
                document.getElementById('txtVoucherNo').value = "Auto";
                cComboInstrumentTypee.Focus();
            }
            else {
                document.getElementById('txtVoucherNo').value = "";
                InsgridBatch.batchEditApi.EndEdit();
                $('#txtVoucherNo').focus();
            }

            var CashBankId = cddlCashBank.GetValue();
            var CashBankText = cddlCashBank.GetText();
            var strbranch = $('#<%=hdnBranchId.ClientID %>').val();
            var arr = CashBankText.split('|');
            PopulateCurrentBankBalance(arr[0], strbranch);
            // cComboType.Focus();


            InsgridBatch.cpType = null;
            InsgridBatch.cpAutoID = null;--%>
        }
    }
            ////##### coded by Samrat Roy - 14/04/2017 - ref IssueLog(Voucher - 110) 
            ////This method is called when request is for View Only .
            // if (InsgridBatch.cpView == "1") {
            if (ViewStatus == "1") {
                viewOnly();
            }
            else if (pageStatus == "delete") {
                $('#<%=hdnPageStatus.ClientID %>').val('');
        CustomDeleteID = "";
        cbtnSaveNew.SetVisible(true);
        cbtnSaveRecords.SetVisible(true);

        //document.getElementById('btnSaveNew').style.display = 'none';
        //document.getElementById('btnSaveRecords').style.display = 'none';

        OnAddNewClick();
    }
    else if (pageStatus == "update") {
        $('#<%=hdnPageStatus.ClientID %>').val('');
            AddButtonClick();
            var VoucherType = $('#rbtnType').val();
        //if (VoucherType == "P") {
        //    InsgridBatch.GetEditor('btnRecieve').SetEnabled(false);
        //}
        //else {
        //    InsgridBatch.GetEditor('btnPayment').SetEnabled(false);
        //}
        }
            //else if (pageStatus == "first") {
            //    AddButtonClick();
            //}

        }
        function  Gridupdate() {
            var VoucherType = $("#rbtnType").val();

            var CashBank_ID = getParameterByName('key');
            //var CashBank_ID = document.getElementById('hdnEditRfid').Value;
            var Currency = cCmbCurrency.GetValue();
            var InstrumentType = cComboInstrumentTypee.GetValue();
            // EditAddressSinglePage(CashBank_ID, 'CBE');
            if (VoucherType == "P") {
                document.getElementById('divPaidTo').style.display = 'block';
                document.getElementById('divReceivedfrom').style.display = 'none';
            }
            else {
                document.getElementById('divReceivedfrom').style.display = 'block';
                document.getElementById('divPaidTo').style.display = 'none';
            }
            $("#txtVoucherNo").attr("disabled", "disabled");
            $("#ddlBranch").attr("disabled", "disabled");
            $("#ddlEnterBranch").attr("disabled", "disabled");
            $("#ddl_AmountAre").attr("disabled", "disabled");
            var setCurr = '<%=Session["LocalCurrency"]%>';
    var localCurrency = setCurr.split('~')[0];
    if (Currency != localCurrency) {
        $('#<%=hdnCurrenctId.ClientID %>').val(Currency);
        ctxtRate.SetEnabled(true);
    }
    else {
        ctxtRate.SetEnabled(false);
        $('#<%=hdnCurrenctId.ClientID %>').val("");
    }

    var WithdrawalType = "";
    if (InstrumentType == "C") {
        WithdrawalType = "Cheque";
    }
    else if (InstrumentType == "E") {
        WithdrawalType = "E.Transfer";
    }
    else if (InstrumentType == "D") {
        WithdrawalType = "Draft";
    }
    else if (InstrumentType == "CH") {
        WithdrawalType = "Cash";
        $('#<%=hdnInstrumentType.ClientID %>').val(0);
        document.getElementById("divInstrumentNo").style.display = 'none';
        document.getElementById("tdIDateDiv").style.display = 'none';
    }
    else {
        $('#<%=hdnInstrumentType.ClientID %>').val(InstrumentType);
    }

    WithdrawalChangedNew(WithdrawalType);
    cComboInstrumentTypee.SetValue(InstrumentType);

    InsgridBatch.batchEditApi.StartEdit(-1, 1);
    if ($('#<%=hdnEditClick.ClientID %>').val() == 'T') {
        OnAddNewClick();
        $('#<%=hdnEditClick.ClientID %>').val("");
    }
    $("#rbtnType").attr("disabled", "disabled");
    cddl_AmountAre.SetEnabled(false);
    var CashBankText = cddlCashBank.GetText();
    var arr = CashBankText.split('|');
    var strbranch = $("#ddlBranch").val();
    PopulateCurrentBankBalance(arr[0], strbranch);
    document.getElementById('hdnEditCBID').value = CashBank_ID;
    document.getElementById('divNumberingScheme').style.display = 'none';
    document.getElementById('divEnterBranch').style.display = 'Block';
    InsgridBatch.PerformCallback('Display');


    $("#tdSaveNewButton").hide();


        }
        function AddButtonClick() {
            debugger;
            cCmbScheme.SetEnabled(true);
            ctxtRate.SetEnabled(false);
            // document.getElementById('rbtnType').value = 'P';
            var VoucherType = document.getElementById('rbtnType').value;
            $('#<%=hdnPayment.ClientID %>').val('NO');
    $('#<%=hdnTaxGridBind.ClientID %>').val('NO');
    <%-- document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;--%>
            OnAddNewClick();
            var defaultbranch = '<%=Session["userbranchID"]%>';
$('#<%=hdnBranchId.ClientID %>').val(defaultbranch);
            var SaveModeCB = '<%=Session["SaveModeCB"]%>';
            if (SaveModeCB == "") {
                cCmbScheme.Focus();
                if ($('#hdn_Mode').val() != "Edit") {
                    //cddl_AmountAre.SetEnabled(true);
                    cddl_AmountAre.SetValue(3);
                }
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
    }
    else {
        if (SaveModeCB == 'A') {
            cdtTDate.Focus();
        }
        if (SaveModeCB == 'M') {
            document.getElementById("txtVoucherNo").focus();
        }
        <%--var InstrumentType = cComboInstrumentTypee.GetValue();
        var WithdrawalType = "";
        if (InstrumentType == "C") {
            WithdrawalType = "Cheque";
        }
        else if (InstrumentType == "E") {
            WithdrawalType = "E.Transfer";
        }
        else if (InstrumentType == "D") {
            WithdrawalType = "Draft";
        }
        else if (InstrumentType == "CH") {
            WithdrawalType = "Cash";
            $('#<%=hdnInstrumentType.ClientID %>').val(0);
            document.getElementById("divInstrumentNo").style.display = 'none';
            document.getElementById("tdIDateDiv").style.display = 'none';

        }
        else {
            $('#<%=hdnInstrumentType.ClientID %>').val(InstrumentType);

        }
        WithdrawalChangedNew(WithdrawalType);
        cComboInstrumentTypee.SetValue(InstrumentType);--%>

    }

            //LoadCustomerAddress('', $('#ddlBranch').val(), 'PO');
        }

        function FinYearCheckOnPageLoad() {
            var SelectedDate = new Date(cdtTDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();
            var SelectedDateValue = new Date(year, monthnumber, monthday);
            var FYS = "<%=Session["FinYearStart"]%>";
    var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);
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

            }
            else {
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    cdtTDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    cdtTDate.SetDate(new Date(FinYearEndDate));
                }
            }
        }
        function TDateChange() {
            var SelectedDate = new Date(cdtTDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);
            ///Checking of Transaction Date For MaxLockDate
            var MaxLockDate = new Date('<%=Session["LCKBNK"]%>');
    monthnumber = MaxLockDate.getMonth();
    monthday = MaxLockDate.getDate();
    year = MaxLockDate.getYear();
    var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();
    if (SelectedDateValue <= MaxLockDateNumeric) {
        jAlert('This Entry Date has been Locked.');
        MaxLockDate.setDate(MaxLockDate.getDate() + 1);
        cdtTDate.SetDate(MaxLockDate);
        return;
    }
    ///End Checking of Transaction Date For MaxLockDate
    ///Date Should Between Current Fin Year StartDate and EndDate
    var FYS = "<%=Session["FinYearStart"]%>";
    var FYE = "<%=Session["FinYearEnd"]%>";
    var LFY = "<%=Session["LastFinYear"]%>";
    var FinYearStartDate = new Date(FYS);
    var FinYearEndDate = new Date(FYE);
    var LastFinYearDate = new Date(LFY);

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
        //                   alert('Between');
    }
    else {
        // jAlert('Enter Date Is Outside Of Financial Year !!');
        jAlert('Enter Date Is Outside Of Financial Year !!', 'Alert Dialog: [CashBank]', function (r) {
            if (r == true) {
                cdtTDate.Focus();
            }
        });
        if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
            cdtTDate.SetDate(new Date(FinYearStartDate));
        }
        if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
            cdtTDate.SetDate(new Date(FinYearEndDate));
        }
    }
    cInstDate.SetDate(SelectedDate);
    ///End OF Date Should Between Current Fin Year StartDate and EndDate
    }
    </script>
    
    <link href="CSS/CashBankEntry.css" rel="stylesheet" />

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 7px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        .calendar-icon-2 {
            position: absolute;
            bottom: 7px;
            right: 4px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #FormDate , #toDate , #dtTDate , #InstDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #InstDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #InstDate_B-1 #InstDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 6px;
            right: -2px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
                z-index: 0;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GvCBSearch
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        select.btn
        {
           position: relative;
           z-index: 0;
        }

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .makeFullscreen
        {
                z-index: 0;
        }

        .panel-fullscreen
        {
                z-index: 99 !important;
        }
        #massrecdt
        {
            width: 100%;
        }

        .mb-10{
            margin-bottom: 10px;
        }

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfVSFileName" runat="server" />

    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Cash/Bank Voucher Add"></asp:Label>

            </h3>
            <div id="pageheaderContent" class="pull-right wrapHolder reverse content horizontal-images" style="display: none;">
                <ul>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Current Balance </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="width: 100%;">
                                                <b style="text-align: left" id="B_ImgSymbolBankBal" runat="server"></b>
                                                <b style="text-align: center" id="B_BankBalance" runat="server"></b>
                                            </div>

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>


                </ul>
            </div>
            <div id="btncross" runat="server" class="crossBtn" style="margin-left: 50px;">
                <a href="javascript:void(0);" onclick="ReloadPage()">
                    <i class="fa fa-times"></i>
                </a>
            </div>
        </div>
    </div>
        <div class="form_main  clearfix">

        <div class="rgth pull-left full">
          
            <div id="DivEntry">
                <dxe:PanelContent runat="server">
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                        <TabPages>
                            <dxe:TabPage Name="General" Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackGeneral" ClientInstanceName="cASPxCallbackGeneral">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <div id="divChangable" runat="server" style=" padding: 5px 0 5px 0; margin-bottom: 0px; border-radius: 4px; margin-bottom: 15px;">
                                                        <div class="row">
                                                            <div class="col-md-12" style="padding: 0 5px;">
                                                                <div class="col-md-2">
                                                                    <label for="exampleInputName2" style="margin-top: 8px">
                                                                        Voucher Type <b id="bTypeText" runat="server" style="width: 20%; display: none; font-size: 12px"></b>
                                                                    </label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <asp:DropDownList ID="rbtnType" runat="server" Width="100%" onchange="rbtnType_SelectedIndexChanged()">
                                                                            <asp:ListItem Text="Receipt" Value="R" />
                                                                            <asp:ListItem Text="Payment" Value="P" />
                                                                        </asp:DropDownList>
                                                                       
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2" id="divNumberingScheme" runat="server">
                                                                    <label style="margin-top: 8px">Numbering Scheme</label>
                                                                    <div>
                                                                        <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme"
                                                                            SelectedIndex="0" EnableCallbackMode="false"
                                                                            TextField="SchemaName" ValueField="ID"
                                                                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True" OnCallback="CmbScheme_Callback">
                                                                            <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}" EndCallback="CmbSchemeEndCallback"></ClientSideEvents>
                                                                        </dxe:ASPxComboBox>
                                                                        <%--  GotFocus="NumberingScheme_GotFocus"--%>
                                                                        <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2 lblmTop8" style="display: none" id="divEnterBranch" runat="server">

                                                                    <label>Unit <span style="color: red">*</span></label>
                                                                    <div>
                                                                        <asp:DropDownList ID="ddlEnterBranch" runat="server" DataSourceID="dsBranch"
                                                                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                                                        </asp:DropDownList>

                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <label style="margin-top: 8px">Document No.</label>
                                                                    <div>
                                                                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="30" onchange="txtBillNo_TextChanged()">                             
                                                                        </asp:TextBox>
                                                                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-2">
                                                                    <label style="margin-top: 8px">Posting Date  </label>
                                                                    <div>
                                                                        <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom"
                                                                            Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                                                            <ButtonStyle Width="13px"></ButtonStyle>
                                                                            <ClientSideEvents DateChanged="function(s,e){TDateChange();}" GotFocus="function(s,e){cdtTDate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}"></ClientSideEvents>
                                                                        </dxe:ASPxDateEdit>
                                                                        <%--Rev 1.0--%>
                                                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                                        <%--Rev end 1.0--%>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-2 lblmTop8">
                                                                    <label>For Unit <span style="color: red">*</span></label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <asp:DropDownList ID="ddlBranch" runat="server" onchange="ddlBranch_SelectedIndexChanged()"
                                                                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                                                        </asp:DropDownList>
                                                                        <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                                                    </div>
                                                                </div>
                                                                <div class="clear"></div>
                                                                <div style="border-top: 1px solid #ccc; margin-top: 8px; height: 10px;">
                                                                </div>
                                                                <div class="clear"></div>
                                                                <div class="col-md-2" id="tdCashBankLabel">
                                                                    <label>Cash/Bank</label>
                                                                    <div>
                                                                        <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientInstanceName="cddlCashBank" Width="100%" OnCallback="ddlCashBank_Callback">
                                                                            <ClientSideEvents EndCallback="CashBank_EndCallback" SelectedIndexChanged="CashBank_SelectedIndexChanged" GotFocus="CashBank_GotFocus" KeyDown="CashBank_GotFocus" />
                                                                        </dxe:ASPxComboBox>
                                                                        <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                                                    </div>
                                                                </div>
                                                                <div class="col-md-1">
                                                                    <label>Currency  </label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <dxe:ASPxComboBox ID="CmbCurrency" EnableIncrementalFiltering="True" ClientInstanceName="cCmbCurrency"
                                                                            TextField="Currency_AlphaCode" ValueField="Currency_ID" SelectedIndex="0" Native="true"
                                                                            runat="server" ValueType="System.String" EnableSynchronization="True" Width="100%" CssClass="pull-left">
                                                                            <ClientSideEvents ValueChanged="function(s,e){Currency_Rate()}" GotFocus="function(s,e){cCmbCurrency.ShowDropDown();}"></ClientSideEvents>
                                                                        </dxe:ASPxComboBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-1">
                                                                    <label>Rate  </label>
                                                                    <div>

                                                                        <dxe:ASPxTextBox runat="server" ID="txtRate" HorizontalAlign="Right" ClientInstanceName="ctxtRate" Width="100%" CssClass="pull-left">
                                                                            <%-- Rev 4.0 --%>
                                                                            <%--<MaskSettings Mask="<0..9999>.<0..99999>" IncludeLiterals="DecimalSymbol" />--%>
                                                                            <MaskSettings Mask="<0..9999>.<0..99999>" IncludeLiterals="DecimalSymbol" AllowMouseWheel="false"/>
                                                                            <%-- Rev 4.0 End--%>
                                                                        </dxe:ASPxTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <label style="">Instrument Type</label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <dxe:ASPxComboBox ID="cmbInstrumentType" runat="server" ClientInstanceName="cComboInstrumentTypee" Font-Size="12px"
                                                                            ValueType="System.String" Width="100%" EnableIncrementalFiltering="True" Native="true">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Cheque" Value="C" Selected />
                                                                                <dxe:ListEditItem Text="Draft" Value="D" />
                                                                                <dxe:ListEditItem Text="E.Transfer" Value="E" />
                                                                                <dxe:ListEditItem Text="Cash" Value="CH" />
                                                                            </Items>
                                                                            <ClientSideEvents SelectedIndexChanged="InstrumentTypeSelectedIndexChanged" GotFocus="function(s,e){cComboInstrumentTypee.ShowDropDown();}" />
                                                                        </dxe:ASPxComboBox>
                                                                        <span id="MandatoryInstrumentType" class="iconInstrumentType pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2" id="divInstrumentNo" style="">
                                                                    <%--Rev 3.0 [ <span style="color: red">*</span>  added] --%>
                                                                    <label id="" style="">Instrument No <span style="color: red">*</span></label>
                                                                    <div id="">
                                                                        <dxe:ASPxTextBox runat="server" ID="txtInstNobth" ClientInstanceName="ctxtInstNobth" Width="100%" MaxLength="30" CssClass="pull-left">
                                                                        </dxe:ASPxTextBox>
                                                                        <span id="MandatoryInstNo" class="iconInstNo pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2" id="tdIDateDiv" style="">
                                                                    <label id="tdIDateLable" style="">Instrument Date</label>
                                                                    <div id="tdIDateValue" style="">
                                                                        <dxe:ASPxDateEdit ID="InstDate" runat="server" EditFormat="Custom" ClientInstanceName="cInstDate"
                                                                            UseMaskBehavior="True" Font-Size="12px" Width="100%" EditFormatString="dd-MM-yyyy">
                                                                            <%--<ClientSideEvents DateChanged="function(s,e){InstrumentDateChange();}" GotFocus="function(s,e){cInstDate.ShowDropDown();}"></ClientSideEvents>--%>
                                                                            <ClientSideEvents DateChanged="function(s,e){InstrumentDateChange();}"></ClientSideEvents>
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                        </dxe:ASPxDateEdit>
                                                                        <%--Rev 1.0--%>
                                                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                                        <%--Rev end 1.0--%>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2" id="divDraweeBank">
                                                                    <label>Drawee Bank</label>
                                                                    <div>
                                                                        <asp:TextBox ID="txtDraweeBank" runat="server" Width="100%">                             
                                                                        </asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="clear"></div>
                                                                <div class="col-md-3 lblmTop8" id="divReceivedfrom" style="display: none;">
                                                                    <label id="" style="">Received From</label>
                                                                    <div id="">
                                                                        <dxe:ASPxTextBox runat="server" ID="txtReceivedFrom" ClientInstanceName="ctxtReceivedFrom" Width="100%" MaxLength="500">
                                                                        </dxe:ASPxTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3 lblmTop8" id="divPaidTo" style="">
                                                                    <label id="" style="">Paid To</label>
                                                                    <div id="">
                                                                        <dxe:ASPxTextBox runat="server" ID="txtPaidTo" ClientInstanceName="ctxtPaidTo" Width="100%" MaxLength="500">
                                                                        </dxe:ASPxTextBox>
                                                                        <span id="MandatoryPaidTo" class="iconInstNo pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2 lblmTop8">
                                                                    <label>Contact </label>
                                                                    <div>
                                                                        <asp:TextBox ID="txtContact" runat="server" MaxLength="20"
                                                                            Width="100%"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3 lblmTop8">
                                                                    <label>Narration </label>
                                                                    <div>
                                                                        <asp:TextBox ID="txtNarration" runat="server" MaxLength="500" onkeydown="checkTextAreaMaxLength(this,event,'500');"
                                                                            TextMode="MultiLine"
                                                                            Width="100%"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2 lblmTop8" style="display: none;">
                                                                    <label>Reverse Charge </label>
                                                                    <div>
                                                                        <asp:CheckBox ID="chk_reversemechenism" runat="server"></asp:CheckBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2 lblmTop8">
                                                                    <label>
                                                                        <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                                        </dxe:ASPxLabel>
                                                                    </label>
                                                                    <%--Rev 1.0: "simple-select" class add --%>
                                                                    <div class="simple-select">
                                                                        <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre"
                                                                            Width="100%" Native="true">

                                                                            <ClientSideEvents LostFocus="function(s, e) { cddl_AmountAre_LostFocus()}" />
                                                                        </dxe:ASPxComboBox>
                                                                    </div>
                                                                </div>
                                                              <div class="col-md-2" style="padding-top: 25px;" runat="server" id="dvTDSpop">
                                                                    <label class="mTop5">&nbsp;</label>
                                                                    <button class="btn btn-success btn-xs" type="button" data-toggle="modal" onclick="ShowTDSPopup();">Add TDS Details</button>
                                                                </div>
                                                                <div class="clear"></div>
                                                                <div class="col-md-2 lblmTop8" id="dvProjectCode" runat="server">
                                                                    <label id="lblProject" runat="server">Project</label>
                                                                    <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSource"
                                                                        KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" ClientSideEvents-TextChanged="ProjectCodeinlineSelectedPayment">
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
                                                                        <ClientSideEvents GotFocus="Project_gotFocus" LostFocus="Project_LostFocus" ValueChanged="ProjectValueChange"/>
                                                                    
                                                                    </dxe:ASPxGridLookup>
                                                                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                                                                        ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                                                    <%--<dxe:ASPxLabel ID="lbl_ProjectCode" runat="server" Text="Project Code">
                                                                      </dxe:ASPxLabel>
                           <dxe:ASPxButtonEdit ID="ProjectCodeLIst" ClientInstanceName="cProjectCodeLIst" runat="server" ReadOnly="true" Width="100%">
                                                               <Buttons>
                                                              <dxe:EditButton>
                                                             </dxe:EditButton>
                                                                </Buttons>
                                                             <ClientSideEvents ButtonClick="ProjectListButnClick" KeyDown="ProjectListKeyDown" />
                                        </dxe:ASPxButtonEdit>--%>

                                                                    <%--  <dxe:ASPxPopupControl ID="popup_Projectgrid" runat="server" ClientInstanceName="cpopup_Projectgrid"
                                            HeaderText="Select Project" PopupHorizontalAlign="WindowCenter"
                                            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Height="400px" Width="850px"
                                            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                                            ContentStyle-CssClass="pad">
                                            <ContentStyle VerticalAlign="Top" CssClass="pad">
                                            </ContentStyle>
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                    <div>
                                                        <dxe:ASPxGridView ID="ProjectGrid" ClientInstanceName="cProjectGrid" runat="server" KeyFieldName="Proj_Id"
                                                            Width="100%" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" DataSourceID="EntityServerModeDataSource"
                                                            Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                                            <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                                                            <SettingsPager Visible="false"></SettingsPager>
                                                            <Columns>
                                                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                                                <dxe:GridViewDataTextColumn FieldName="Proj_Code" Caption="Project Code" Width="150" VisibleIndex="1">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Proj_Name" Caption="Project Name" Width="100" VisibleIndex="2">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Customer" Caption="Customer" Width="150" VisibleIndex="3">
                                                                </dxe:GridViewDataTextColumn>

                                                            </Columns>
                                                            <SettingsDataSecurity AllowEdit="true" />
                                                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />

                                                        </dxe:ASPxGridView>
                                                         <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                                                          ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                                    </div>
                                                    <div class="text-center">
                                                        <dxe:ASPxButton ID="btnProjectSave" ClientInstanceName="cbtnProjectSave" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                            <ClientSideEvents Click="function(s, e) {ProjectCodeChanged();}" />
                                                        </dxe:ASPxButton>
                                                    </div>
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                        </dxe:ASPxPopupControl>--%>
                                                                </div>
                                                                 
                                                                <div class="col-md-3 lblmTop8" id="hrchy" runat="server">
                                                                    <label>
                                                                        <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                                        </dxe:ASPxLabel>
                                                                    </label>
                                                                    <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                                    </asp:DropDownList>
                                                                </div>
                                                                     <%-- Rev Rajdip --%>
                                                                <div class="col-md-2 lblmTop8" id="dvrefcashbankreq">
                                                                    <label id="Label1" runat="server">Ref. Cash Fund Req.</label>
                                                                     <dxe:ASPxGridLookup ID="lookup_CashFund"  runat="server" ClientInstanceName="clookup_CashFund" DataSourceID="EntityServerModeDataCashFund"
                                KeyFieldName="Paymentreqhead_Id" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">
                                <Columns>
                                        <dxe:GridViewDataColumn FieldName="Paymentreqhead_Id" Visible="true" VisibleIndex="1" Caption="Paymentreqhead Id" EditFormSettings-Visible="False" Settings-AutoFilterCondition="Contains" Width="0px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Paymentrequisition_Number" Visible="true" VisibleIndex="1" Caption="Payment Requisition No." Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="2" Caption="Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <%--<dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" EditFormSettings-Visible="False" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>--%>
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
                               
                            </dxe:ASPxGridLookup>
                            <dx:LinqServerModeDataSource ID="EntityServerModeDataCashFund" runat="server" OnSelecting="EntityServerModeDataCashFund_Selecting"
                                ContextTypeName="ERPDataClassesDataContext" TableName="Trans_Paymentreqhead" />
                                                                </div>
                                                                <%-- End Rev Rajdip --%>
                                                                <div class="clear"></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                        <div class="makeFullscreen ">
                                                            <span class="fullScreenTitle">Cash/Bank Voucher Add</span>
                                                            <span class="makeFullscreen-icon half hovered " data-instance="InsgridBatch" title="Maximize Grid" id="expandInsgridBatch">
                                                                <i class="fa fa-expand"></i>
                                                            </span>
                                                            <dxe:ASPxGridView runat="server" ClientInstanceName="InsgridBatch" ID="gridBatch" KeyFieldName="CashBankID"
                                                                OnBatchUpdate="gridBatch_BatchUpdate"
                                                                OnCellEditorInitialize="gridBatch_CellEditorInitialize"
                                                                OnDataBinding="gridBatch_DataBinding"
                                                                Width="100%" Settings-ShowFooter="true"
                                                                SettingsBehavior-AllowSort="false"
                                                                SettingsBehavior-AllowDragDrop="false"
                                                                OnCustomCallback="gridBatch_CustomCallback"
                                                                SettingsPager-Mode="ShowAllRecords"
                                                                Settings-VerticalScrollBarMode="auto"
                                                                Settings-VerticalScrollableHeight="170"
                                                                OnRowInserting="Grid_RowInserting"
                                                                OnRowUpdating="Grid_RowUpdating"
                                                                OnRowDeleting="Grid_RowDeleting">
                                                                <SettingsPager Visible="false"></SettingsPager>
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4%" VisibleIndex="0" Caption="">
                                                                        <CustomButtons>
                                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                                            </dxe:GridViewCommandColumnCustomButton>
                                                                        </CustomButtons>
                                                                    </dxe:GridViewCommandColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="5%">
                                                                        <PropertiesTextEdit>
                                                                        </PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataButtonEditColumn FieldName="MainAccount" Caption="Main Account" VisibleIndex="2">
                                                                        <PropertiesButtonEdit>
                                                                            <ClientSideEvents ButtonClick="MainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                                                            <Buttons>
                                                                                <dxe:EditButton Text="..." Width="20px">
                                                                                </dxe:EditButton>
                                                                            </Buttons>
                                                                        </PropertiesButtonEdit>
                                                                    </dxe:GridViewDataButtonEditColumn>

                                                                    <dxe:GridViewDataButtonEditColumn FieldName="bthSubAccount" Caption="Sub Account" VisibleIndex="3">
                                                                        <PropertiesButtonEdit>
                                                                            <ClientSideEvents ButtonClick="SubAccountButnClick" KeyDown="SubAccountKeyDown" />
                                                                            <Buttons>
                                                                                <dxe:EditButton Text="..." Width="20px">
                                                                                </dxe:EditButton>
                                                                            </Buttons>
                                                                        </PropertiesButtonEdit>
                                                                    </dxe:GridViewDataButtonEditColumn>

                                                                  
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Receipt" FieldName="btnRecieve" Width="130" HeaderStyle-HorizontalAlign="Right">
                                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                                            <ClientSideEvents KeyDown="OnKeyDown" LostFocus="ReceiptTextChange"
                                                                                GotFocus="function(s,e){
                                                                        DebitGotFocus(s,e); 
                                                                        }" />
                                                                            <ClientSideEvents />
                                                                        </PropertiesTextEdit>
                                                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Payment" FieldName="btnPayment" Width="130" HeaderStyle-HorizontalAlign="Right">
                                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                                            <ClientSideEvents KeyDown="OnKeyDown" LostFocus="PaymentTextChange"
                                                                                GotFocus="function(s,e){
                                                        CreditGotFocus(s,e);
                                                        }" />
                                                                            <ClientSideEvents />

                                                                        </PropertiesTextEdit>
                                                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>

                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="6" Width="10%" HeaderStyle-HorizontalAlign="Right">
                                                                        <PropertiesButtonEdit Style-HorizontalAlign="Right">
                                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                                            <Buttons>
                                                                                <dxe:EditButton Text="..." Width="20px">
                                                                                </dxe:EditButton>
                                                                            </Buttons>
                                                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" />
                                                                        </PropertiesButtonEdit>
                                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                    </dxe:GridViewDataButtonEditColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="NetAmount" Caption="Net Amount" VisibleIndex="7" Width="12%" HeaderStyle-HorizontalAlign="Right">
                                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                                        </PropertiesTextEdit>
                                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>


                                                                     <dxe:GridViewDataButtonEditColumn FieldName="Project_Code" Caption="Project Code" VisibleIndex="8" Width="14%">
                                                                         <PropertiesButtonEdit>
                                                                            <ClientSideEvents ButtonClick="ProjectCodeButnClick" KeyDown="ProjectCodeKeyDown"/>
                                                                                  <Buttons>
                                                                            <dxe:EditButton Text="..." Width="20px">
                                                                            </dxe:EditButton>
                                                                                  </Buttons>
                                                                        </PropertiesButtonEdit>
                                                                     </dxe:GridViewDataButtonEditColumn>


                                               <%-- End--%>



                                                                    <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Remarks" FieldName="btnLineNarration" Width="160">
                                                                        <PropertiesTextEdit>
                                                                        </PropertiesTextEdit>
                                                                        <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30" VisibleIndex="10" Caption=" ">
                                                                        <CustomButtons>
                                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                                                <Image Url="/assests/images/add.png">
                                                                                </Image>
                                                                            </dxe:GridViewCommandColumnCustomButton>
                                                                        </CustomButtons>
                                                                    </dxe:GridViewCommandColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="CashBankID" Caption="Srl No" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="ReverseApplicable" Caption="Srl No" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="TDS" Caption="TDS" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="gvColMainAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="gvColSubAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="IsSubledger" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataTextColumn FieldName="ProjectId" Caption="ProjectId"  ReadOnly="True" Width="0"
                                                                             EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                                                                             PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                                             <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>

                                                                </Columns>
                                                                <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" BatchEditStartEditing="OnBatchEditStartEditing"
                                                                    CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" />
                                                                <%-- <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing"
                                                            CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" />--%>
                                                                <SettingsDataSecurity AllowEdit="true" />
                                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                                                                </SettingsEditing>
                                                                <Styles>
                                                                    <StatusBar CssClass="statusBar">
                                                                    </StatusBar>
                                                                </Styles>
                                                            </dxe:ASPxGridView>
                                                        </div>
                                                    </div>
                                                    <div class="text-center">
                                                        <table style="margin-left: 354px; margin-top: 10px">
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
                                                                <td style="padding-right: 50px; padding-left: 44px; display: none;"><b>Total Net Amount</b></td>
                                                                <td style="width: 203px; display: none;">
                                                                    <dxe:ASPxTextBox ID="txtTotalNetAmount" runat="server" Width="105px" ClientInstanceName="c_txtTotalNetAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                                        <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                                    </dxe:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>


                                                    <div class="text-center" id="divRoundOff" runat="server">
                                                        <div class="content reverse horizontal-images clearfix" style="margin-top: 8px; width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                                            <ul>
                                                                <li>
                                                                    <div class="text-left mBot4">Round Off Account</div>
                                                                    <dxe:ASPxButtonEdit ID="btnMARoundOff" runat="server" ReadOnly="true" ClientInstanceName="cbtnMARoundOff" Width="100%">
                                                                        <Buttons>
                                                                            <dxe:EditButton Text="...">
                                                                            </dxe:EditButton>
                                                                        </Buttons>
                                                                        <ClientSideEvents ButtonClick="MainAccountButnClickRO"
                                                                            KeyDown="MainAccountNewkeydownRO" />
                                                                    </dxe:ASPxButtonEdit>
                                                                </li>
                                                                <li>
                                                                    <div class="text-left mBot4">Amount</div>
                                                                    <dxe:ASPxTextBox ID="txtMainAccountAmount" runat="server" ClientInstanceName="ctxtMainAccountAmount"
                                                                        DisplayFormatString="0.00" Width="100%" CssClass="pull-left">
                                                                        <MaskSettings Mask="&lt;-999999999..999999999g&gt;.&lt;00..99&gt;" />
                                                                    </dxe:ASPxTextBox>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>





                                                    <table style="width: 100%;" id="BtnTable">
                                                        <tr>
                                                            <td style="padding: 15px 0;">
                                                                <span id="tdSaveNewButton">
                                                                    <%-- <% if (rights.CanAdd)
                                                                        { %>
                                                                    <a>--%>
                                                                    <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveNew" runat="server"
                                                                        AutoPostBack="false" CssClass="btn btn-success" TabIndex="0" Text="S&#818;ave & New"
                                                                        UseSubmitBehavior="False">
                                                                        <ClientSideEvents Click="function(s, e) {SaveButtonClickNew();}" />
                                                                    </dxe:ASPxButton>
                                                                    <%--   </a>
                                                                      <% } %>--%>
                                                                </span>
                                                                <span id="tdSaveButton">
                                                                    <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server"
                                                                        AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save & Ex&#818;it"
                                                                        UseSubmitBehavior="False">
                                                                        <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                                                    </dxe:ASPxButton>

                                                                </span>
                                                                <div runat="server" id="divTDS">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dxe:PanelContent>
                                            </PanelCollection>
                                            <%--  <ClientSideEvents EndCallback="acpEndCallbackGeneral" />--%>
                                        </dxe:ASPxCallbackPanel>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <ucBS:Purchase_BillingShipping runat="server" ID="Purchase_BillingShipping" />
                                        <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="CBE" />
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
                </dxe:PanelContent>

            </div>

            <div id="HiddenField">
                <%--  <asp:HiddenField ID="hdnDefaultBranch" runat="server" />
                <asp:HiddenField ID="hdnType" runat="server" />
                <asp:HiddenField ID="hdnAccountType" runat="server" />
                <asp:HiddenField ID="txtMainAccount_hidden" runat="server" />
                <asp:HiddenField ID="txtSubAccount_hidden" runat="server" />
                <asp:HiddenField ID="hdn_Brch_NonBrch" runat="server" />
                <asp:HiddenField ID="hdn_SubLedgerType" runat="server" />
                <asp:HiddenField ID="hdn_MainAcc_Type" runat="server" />
                <asp:HiddenField ID="hdn_SubAccountIDE" runat="server" />
                <asp:HiddenField ID="txtBankAccounts_hidden" runat="server" />--%>
                <input type="hidden" id="IsTaxApplicable" value="" />
                <asp:HiddenField ID="hdn_Mode" runat="server" Value="Edit" />
                <asp:HiddenField ID="hdnisprojectexists" runat="server"/>
                <%-- <asp:HiddenField ID="hdn_PayeeIDOnUpdate" runat="server" />
                <asp:HiddenField ID="hdn_Brch_NonBrchE" runat="server" />
                <asp:HiddenField ID="hdn_CurrentSegment" runat="server" />--%>
                <asp:HiddenField ID="hdn_CashBankType_InstType" runat="server" />
                <asp:HiddenField ID="hdn_SegID_SegmentName" runat="server" />
                <%--  <asp:HiddenField ID="hdn_EditVoucher_SegmentID_Name" runat="server" />
                <asp:HiddenField ID="hdn_OriginalTDate" runat="server" />--%>
                <asp:HiddenField ID="hdnCashBankId" runat="server" />
                <asp:HiddenField ID="hdnCashBankText" runat="server" />
                <asp:HiddenField ID="hdnMainAccountId" runat="server" />
                <%--<asp:HiddenField ID="hdnMainAccountText" runat="server" />--%>
                <%--   <asp:HiddenField ID="hdnSubAccountId" runat="server" />--%>
                <%--  <asp:HiddenField ID="hdnSubAccountText" runat="server" />--%>
                <asp:HiddenField ID="hdnBranchId" runat="server" />
                <asp:HiddenField ID="hfIsFilter" runat="server" />
                <asp:HiddenField ID="TaxAmountOngrid" runat="server" />
                <asp:HiddenField ID="VisibleIndexForTax" runat="server" />

                <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
                 <asp:HiddenField ID="hdnhiarchy" runat="server" />
                <%--  <asp:HiddenField ID="hdnBranchText" runat="server" />--%>
                <%--  <asp:HiddenField ID="hdnIssueBankId" runat="server" />--%>
                <%--  <asp:HiddenField ID="hdnIssueBankText" runat="server" />--%>
                <asp:HiddenField ID="hdnJNMode" runat="server" />
                <%--  <asp:HiddenField ID="hdnReceive" runat="server" />--%>
                <%--    <asp:HiddenField ID="hdnMainAccountEId" runat="server" />--%>
                <%-- <asp:HiddenField ID="hdnMainAccountEText" runat="server" />--%>
                <%--  <asp:HiddenField ID="hdnMainAccountChange" runat="server" />--%>
                <asp:HiddenField ID="hdnBtnClick" runat="server" />
                <%--  <asp:HiddenField ID="hdnSegmentid" runat="server" />--%>
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdnSchemaType" runat="server" />

                <asp:HiddenField ID="hdnInstrumentType" runat="server" />
                <asp:HiddenField ID="hdnInstrumentNo" runat="server" />
                <asp:HiddenField ID="hdnCurrenctId" runat="server" />
                <asp:HiddenField ID="hdnEditClick" runat="server" />
                <%--  <asp:HiddenField ID="HdnbranchIDSession" runat="server" />--%>
                <asp:HiddenField ID="hdfIsDelete" runat="server" />
                <asp:HiddenField ID="hdnEditCBID" runat="server" />
                <asp:HiddenField ID="hdnEditRfid" runat="server" />
                <asp:HiddenField ID="hdnPayment" runat="server" />
                <asp:HiddenField ID="hdnTaxGridBind" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hdnAutoPrint" runat="server" />
                <asp:HiddenField ID="hdnView" runat="server" />
                <asp:HiddenField ID="hdnSubAccountYESNO" runat="server" />
                <asp:HiddenField ID="hdnPaidToYesNO" runat="server" />
                <%-- for Tax --%>

                <asp:HiddenField ID="hfVendorGSTIN" runat="server" />
                <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
                <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
                <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
                <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />

                <%--  --%>
                <%-- <asp:HiddenField ID="hdnCheckAdd" runat="server" />
                --%>
            </div>
            <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                <ClientSideEvents ControlsInitialized="AllControlInitilize" />
            </dxe:ASPxGlobalEvents>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>
            <asp:SqlDataSource ID="SqlDataSourceMainAccount" runat="server"
                SelectCommand=""></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceSubAccount" runat="server"
                SelectCommand=""></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlSchematype" runat="server"
                SelectCommand=""></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlCurrency" runat="server"
                SelectCommand="Select * From ((Select '0' as Currency_ID , 'Select' as Currency_AlphaCode) Union select Currency_ID,Currency_AlphaCode from Master_Currency )tbl Order By Currency_ID"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlCurrencyBind" runat="server"></asp:SqlDataSource>

            <%-- -------------------POPUPControl   FOR Main & Sub Account-------------------------------------%>
            <dxe:ASPxPopupControl ID="MainAccountpopUp" runat="server" ClientInstanceName="cMainAccountpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Main Account" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <HeaderTemplate>
                    <span style="color: #fff"><strong>Search By Main Account (4 Char)</strong></span>
                    <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                        <ClientSideEvents Click="function(s, e){ 
                                                                       MainAccountClose();
                                                                    }" />
                    </dxe:ASPxImage>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By Main Account(4 Char)</strong> <span style="color: red">[Press Esc to Cancel]</span></label>
                        <div id="mainActMsg">
                            <span style="color: red; right: 46px;"><strong>* Invalid Main Account</strong> </span>
                        </div>
                        <dxe:ASPxComboBox ID="MainAccountComboBox" runat="server" EnableCallbackMode="true" CallbackPageSize="10" Width="100%"
                            ValueType="System.String" ValueField="MainAccount_ReferenceID" ClientInstanceName="cMainAccountComboBox"
                            OnItemsRequestedByFilterCondition="ASPxMainAccountComboBox_OnItemsRequestedByFilterCondition_SQL"
                            OnItemRequestedByValue="ASPxMainComboBox_OnItemRequestedByValue_SQL"
                            FilterMinLength="4"
                            TextFormatString="{0}"
                            DropDownStyle="DropDown">

                            <Columns>
                                <dxe:ListBoxColumn FieldName="MainAccount_Name" Caption="Main Account Name" Width="320px" />
                                <dxe:ListBoxColumn FieldName="MainAccount_SubLedgerType" Caption="Sub Account Type" Width="150px" />
                                <dxe:ListBoxColumn FieldName="MainAccount_ReverseApplicable" Caption="ReverseApplicable" Width="0" />
                                <dxe:ListBoxColumn FieldName="TAXable" Caption="TAXable" Width="0" />
                            </Columns>
                            <ClientSideEvents ValueChanged="function(s, e) {GetMainAcountComboBox(e)}" KeyDown="MainAccountComboBoxKeyDown" />
                        </dxe:ASPxComboBox>
                        <%--  <dxe:ASPxGridLookup ID="MainAccountLookUp" runat="server" DataSourceID="EntityServerMainDataSource" ClientInstanceName="cMainAccountLookUp"
                    KeyFieldName="MainAccount_ReferenceID" Width="800" TextFormatString="{0}" ClientSideEvents-TextChanged="MainAccountSelected" 
                    ClientSideEvents-KeyDown="MainAccountlookUpKeyDown">
                    <Columns>
                        <dxe:GridViewDataColumn FieldName="MainAccount_Name" Caption="Main Account Name" Width="220">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="MainAccount_SubLedgerType" Caption="Type" Width="80">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>

                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                   
                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                </dxe:ASPxGridLookup>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>
            <dxe:ASPxPopupControl ID="SubAccountpopUp" runat="server" ClientInstanceName="cSubAccountpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="200"
                Width="700" HeaderText="Select Sub Account" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <HeaderTemplate>
                    <span style="color: #fff"><strong>Search By Sub Account (4 Char)</strong></span>
                    <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                        <ClientSideEvents Click="function(s, e){ 
                                                                       SubAccountClose();
                                                                    }" />
                    </dxe:ASPxImage>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By Sub Account(4 Char)</strong><span style="color: red"> [Press Esc to Cancel]</span></label>
                        <div id="subActMsg">
                            <span style="color: red; right: 46px;"><strong>* Invalid Sub Account</strong> </span>
                        </div>
                        <dxe:ASPxComboBox ID="SubAcountComboBox" runat="server" EnableCallbackMode="true" CallbackPageSize="10" Width="95%"
                            ValueType="System.String" ValueField="SubAccount_ReferenceID" ClientInstanceName="cSubAcountComboBox"
                            OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL" FilterMinLength="4"
                            OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{0}"
                            DropDownStyle="DropDown" DropDownRows="7">
                            <Columns>
                                <dxe:ListBoxColumn FieldName="Contact_Name" Caption="Sub Account Name" Width="320px" />
                            </Columns>
                            <ClientSideEvents ValueChanged="function(s, e) {GetSubAcountComboBox(e)}" KeyDown="SubAccountComboBoxKeyDown" />
                        </dxe:ASPxComboBox>
                        <%--  <dxe:ASPxGridLookup ID="SubAccountLookup" runat="server" ClientInstanceName="cSubAccountLookUp"
                    KeyFieldName="SubAccount_ReferenceID" Width="800" TextFormatString="{0}" ClientSideEvents-TextChanged="SubAccountSelected"
                    ClientSideEvents-KeyDown="SubAccountlookUpKeyDown">
                    <Columns>
                        <dxe:GridViewDataColumn FieldName="Contact_Name" Caption="Sub Account Name" Width="220">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>

                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                </dxe:ASPxGridLookup>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>
            <%--  <dx:LinqServerModeDataSource ID="EntityServerMainDataSource" runat="server" OnSelecting="EntityServerMainDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="v_MainAccountList" />--%>



            <%-- -------------------End   POPUPControl   FOR Main & Sub Account-------------------------------------%>
        </div>

    </div>
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="gridBatch"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <dxe:ASPxCallbackPanel runat="server" ID="acpPaymentTDS" ClientInstanceName="cacpPaymentTDS">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
    </dxe:ASPxCallbackPanel>
    <dxe:ASPxCallbackPanel runat="server" ID="acpCrossBtn" ClientInstanceName="cacpCrossBtn" OnCallback="acpCrossBtn_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acpCrossBtnEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:SqlDataSource ID="dsBranch" runat="server"
        ConflictDetection="CompareAllValues"
        SelectCommand=""></asp:SqlDataSource>

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
                                <%-- indranil--%>
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
                                <%-- <ClientSideEvents  RowClick="GetTaxVisibleIndex" />--%>
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

    <div class="modal fade modalTop pmsModal  w90" id="TDSmodal" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">TDS Deposit</h4>
                </div>
                <div class="modal-body">
                    <div class="clearfix">
                        <div class="col-sm-2">
                            <label id="lblsection">Section</label>
                            <div class="relative">
                                <dxe:ASPxComboBox runat="server" SelectedIndex="0" ID="tdsSection" Width="100%" ClientInstanceName="ctdsSection" DataSourceID="dsTDS" ValueField="ID" TextField="Section_Code" ValueType="System.String">
                                    <ClientSideEvents SelectedIndexChanged="selectTDSChange" />
                                </dxe:ASPxComboBox>
                                <asp:SqlDataSource runat="server" ID="dsTDS" SelectCommand="select '0~' as ID ,'Select' Section_Code Union ALL  Select Section_Code +'~'+Section_Description  ID,  Section_Code from tbl_master_TDS_Section  SEC inner join Master_TDSTCS TDS on TDS.TDSTCS_Code=SEC.Section_Code"></asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <label>TDS Deducted on</label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtDeductionON" ClientEnabled="false" Width="100%" runat="server" ClientInstanceName="ctxtDeductionON"></dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <label>Date of Deposit</label>
                            <div class="relative">
                                <dxe:ASPxDateEdit ID="tdsDate" ClientEnabled="false" runat="server" ClientInstanceName="ctdsDate" EditFormat="Custom"
                                    Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                    <ButtonStyle Width="13px"></ButtonStyle>

                                </dxe:ASPxDateEdit>
                            </div>
                        </div>


                         <div class="col-sm-2">
                            <label>Financial Year</label>
                            <div class="relative">
                                <select id="ddlFinYear" runat="server" onchange="selectTDSChange(this,this);" class="form-control">
                                    <option value="2019-20">2019-20</option>
                                    <option value="2020-21">2020-21</option>
                                    <option value="2021-22">2021-22</option>
                                    <option value="2022-23">2022-23</option>
                                    <%--Rev 2.0--%>
                                    <%--<option value="2023-24">2024-25</option>--%>
                                    <option value="2023-24">2023-24</option>
                                    <option value="2024-25">2024-25</option>
                                    <%--End of Rev 2.0--%>

                                </select>
                            </div>
                        </div>


                        <div class="col-sm-2">
                            <label>Quarter</label>
                            <div class="relative">
                                <select id="ddlQuater" runat="server" onchange="selectTDSChange(this,this);" class="form-control">
                                    <option value="Q1">Q1</option>
                                    <option value="Q2">Q2</option>
                                    <option value="Q3">Q3</option>
                                    <option value="Q4">Q4</option>

                                </select>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <label>Type</label>
                            <div class="relative">
                                <select id="ddlEntityType" runat="server" onchange="selectTDSChange(this,this);" class="form-control">
                                    <option value="0">All</option>
                                    <option value="01">Company</option>
                                    <option value="02">Other than Company</option>                                  

                                </select>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <label>Surcharge</label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtSurcharge" DisplayFormatString="0.00" Width="100%" runat="server" ClientEnabled="false" ClientInstanceName="ctxtSurcharge">

                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />

                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        <div class="col-sm-2">
                            <label>Education Cess</label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txteduCess" DisplayFormatString="0.00" Width="100%" runat="server" ClientEnabled="false" ClientInstanceName="ctxteduCess">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        
                        <div class="col-sm-2">
                            <label>Interest</label>
                            <div class="relative ">
                                <dxe:ASPxTextBox ID="txtInterest" DisplayFormatString="0.00" runat="server" ClientInstanceName="ctxtInterest" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="RecalculateTotal" />

                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        
                        <div class="col-sm-2">
                            <label>Late Fees (u/s 234E)</label>
                            <div class="relative ">
                                <dxe:ASPxTextBox ID="txtLateFees" DisplayFormatString="0.00" runat="server" ClientInstanceName="ctxtLateFees " Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="RecalculateTotal" />
                                </dxe:ASPxTextBox>

                            </div>
                        </div>

                        <div class="col-sm-2">
                            <label><b>Total</b></label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtTotal" DisplayFormatString="0.00" ClientEnabled="false" runat="server" ClientInstanceName="ctxtTotal" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </dxe:ASPxTextBox>
                                <%--  --%>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <label><b>Tax</b></label>
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txtTax" DisplayFormatString="0.00" ClientEnabled="false" runat="server" ClientInstanceName="ctxtTax" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-sm-2">
                            <label>Others</label>
                            <div class="relative noerrormask">
                                <dxe:ASPxTextBox ID="txtOthers" DisplayFormatString="0.00" runat="server" ClientInstanceName="ctxtOthers" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="RecalculateTotal" />

                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        <%--Rev Mantis Issue 24161--%>
                        <div class="col-sm-2">
                            <label>Advance</label>
                            <div class="relative noerrormask">
                                <dxe:ASPxTextBox ID="txtAdvance" DisplayFormatString="0.00" runat="server" ClientInstanceName="ctxtAdvance" Width="100%">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="RecalculateTotal" />

                                </dxe:ASPxTextBox>

                            </div>
                        </div>
                        <%--End of Rev Mantis Issue 24161--%>

                        <div class="clear"></div>
                        <div class="bdbox clearfix">
                            <div class="headingTypeblo">Bank Details</div>
                            <div class="bdboxContent row">
                                <div class="col-sm-3">
                                    <label>Bank Name</label>
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtBankName" runat="server" ClientInstanceName="ctxtBankName" Width="100%"></dxe:ASPxTextBox>

                                    </div>
                                </div>

                                <div class="col-sm-3">
                                    <label>Branch</label>
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtBankBrach" runat="server" ClientInstanceName="ctxtBankBranch" Width="100%"></dxe:ASPxTextBox>

                                    </div>
                                </div>

                                <div class="col-sm-3">
                                    <label>BSR Code</label>
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtBRS" runat="server" ClientInstanceName="ctxtBRS" Width="100%"></dxe:ASPxTextBox>

                                    </div>
                                </div>

                                <div class="col-sm-3">
                                    <label>Challan No</label>
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtChallanNo" runat="server" ClientInstanceName="ctxtChallanNo" Width="100%"></dxe:ASPxTextBox>

                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="col-sm-3">
                                <label>Select/Deselect All</label>
                                <input type='checkbox' onclick="chechall()" id="chkall" />
                                <input type="hidden" id="hddnflag" />
                            </div>
                            <%--Mantis Issue 24153--%>
                            <div class="col-sm-3">
                                <label>Branch wise</label>
                                <input type='checkbox' onclick="selectTDSChange()" id="chkBranchWise" />
                                <input type="hidden" id="hddnflag" />
                            </div>
                            <%--End of Mantis Issue 24153--%>
                            <div class="col-sm-4"></div>
                            <div class="col-sm-2"></div>
                            <div class="col-sm-2">
                                <label><font color="red">Items Selected:</font></label>
                                <label id="lblcount" style="color: red;"></label>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-12">
                            <table class="HtmlGrid display  dataTable" id="tbltdsDetails">
                                <thead>
                                    <tr>
                                        <th>Select</th>
                                        <th class='hide'></th>
                                        <th class='hide'></th>
                                        <th class='hide'></th>
                                        <th>Document No.</th>
                                        <th>Party ID</th>
                                        <th>Section</th>
                                        <th>Payment/Credit Date</th>
                                        <th>Total Tax</th>
                                        <th>Amount of Tax</th>
                                        <th>Surcharge</th>
                                        <th>Edu. Cess</th>
                                    </tr>
                                </thead>

                            </table>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary m0" onclick="BindGridViaTDSData();" id="btnsave">Save</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <%--Main/Sub Account Model--%>

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


     <%--Chinmoy added inline Project code start 13-12-2019--%>

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





    <div class="modal fade" id="MainAccountModelRO" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeModal();">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydownRO(event)" id="txtMainAccountSearchRO" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTableRO">
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
    <input type="hidden" id="hddncount" />

    <div class="modal fade" id="SubAccountModel" role="dialog" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="CloseSubModal();">&times;</button>
                    <h4 class="modal-title">Sub Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SubAccountNewkeydown(event)" id="txtSubAccountSearch" autofocus width="100%" placeholder="Search By Sub Account Name or Code" />

                    <div id="SubAccountTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Sub Account Name [Unique ID]</th>
                                <th>Sub Account Code</th>
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



    <asp:HiddenField runat="server" ID="hdnROMainAc" />
    <asp:HiddenField runat="server" ID="hdnIsTDS" />
    <asp:HiddenField runat="server" ID="hdnTDSData" />
    <asp:HiddenField runat="server" ID="hdnTDSSection" />
     <asp:HiddenField ID="hdnAllowProjectInDetailsLevel" runat="server" />
      <asp:HiddenField ID="hdnEditProjId" runat="server" />
     <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnIsTdsProj" runat="server" />
      <asp:HiddenField ID="hdnTdsValcheck" runat="server" />
    <asp:HiddenField ID="hdnCheckSaveNew" runat="server" />

    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />

    <%-- Add hidden field for Lead Settings Tanmoy 01-12-2020 --%>
    <asp:HiddenField ID="hdnIsLeadAvailableinTransactions" runat="server" />
    <%-- Add hidden field for Lead Settings Tanmoy 01-12-2020 --%>
    <asp:HiddenField ID="hrCopy" runat="server" />  <%--Copy Mode--%>
</asp:Content>


