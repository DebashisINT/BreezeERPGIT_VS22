<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.management_CashBankEntryEdit" CodeBehind="CashBankEntryEdit.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2.Export" Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>
    
    <script language="javascript" type="text/javascript">
        //ProtoType
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, "");
        }
        String.prototype.ltrim = function () {
            return this.replace(/^\s+/, "");
        }
        String.prototype.rtrim = function () {
            return this.replace(/\s+$/, "");
        }
        //
        //Global Variable
        FieldName = 'txtVoucherNo';
        IsSubAccountChange = "False";
        Param_SubAccountID = '';
        SubLedgerType = "";
        ActiveCurrencyID = "";
        ActiveCurrencyName = "";
        ActiveCurrencySymbol = "";
        //End

        function PageLoad() {
            cMsgPopUp.Show();
            var ActiveCurrency = '<%=Session["ActiveCurrency"]%>'
            ActiveCurrencyID = ActiveCurrency.split('~')[0];
            ActiveCurrencyName = ActiveCurrency.split('~')[1];
            ActiveCurrencySymbol = ActiveCurrency.split('~')[2];
        }
        function CurrencySetting(CParam) {
            var ActiveCurrency = CParam;
            ActiveCurrencyID = ActiveCurrency.split('~')[0];
            ActiveCurrencyName = ActiveCurrency.split('~')[1];
            ActiveCurrencySymbol = ActiveCurrency.split('~')[2];
            document.getElementById('<%=B_ChoosenCurrency.ClientID %>').innerHTML = "Currency : " + ActiveCurrencyName + "[" + ActiveCurrencySymbol + "]";
        }
        function ChangeCurrency() {
            cCbpChoosenCurrency.PerformCallback("ChangeCurrency");
        }
        function CbpChoosenCurrency_EndCallBack() {
            //            alert(cCbpChoosenCurrency.cpChangeCurrencyParam);
            if (cCbpChoosenCurrency.cpChangeCurrencyParam != null) {
                ActiveCurrencyName = cCbpChoosenCurrency.cpChangeCurrencyParam.split('~')[0];
                ActiveCurrencySymbol = cCbpChoosenCurrency.cpChangeCurrencyParam.split('~')[1];
                document.getElementById('<%=B_ChoosenCurrency.ClientID %>').innerHTML = "Currency : " + ActiveCurrencyName + "[" + ActiveCurrencySymbol + "]";
                document.getElementById('B_ImgSymbolDepstBankBal').innerHTML = ActiveCurrencySymbol;
                document.getElementById('<%=B_ImgSymbolAcBal.ClientID %>').innerHTML = ActiveCurrencySymbol;
                document.getElementById('B_ImgSymbolWithBankBal').innerHTML = ActiveCurrencySymbol;
                document.getElementById('<%=B_ImgSymbolBankBal.ClientID %>').innerHTML = ActiveCurrencySymbol;
            }
        }
        function height() {
            if (document.body.scrollHeight >= 300)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '300px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function OnlyNarration(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function checkTextAreaMaxLength(textBox, e, length) {

            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }
            }
        }
        function keyVal(obj) {
            //alert(obj);
            if (obj == "No Record Found") return;
            var WhichAcType = obj.split('~')[2];
            var Mode = document.getElementById("hdn_Mode").value;
            if (WhichAcType == 'MAINAC') {
                var isEdit = obj.split('~')[4];
                document.getElementById('hdnAccountType').value = obj.split('~')[3];
                EntityType = obj.split('~')[2];
                if (isEdit != undefined && isEdit != "") {
                    SubLedgerType = obj.split('~')[1];
                    MainAcCode = obj.split('~')[0];
                    document.getElementById('hdn_SubLedgerType').value = SubLedgerType;
                    cGvAddRecordDisplay.PerformCallback('MainAccountChange~~');
                }
                else {
                    SubLedgerType = obj.split('~')[1];
                    MainAcCode = obj.split('~')[0];
                    document.getElementById('txtsubaccount').value = '';
                    document.getElementById('hdn_SubLedgerType').value = SubLedgerType;
                    if (SubLedgerType.toUpperCase() == 'NONE') {
                        if (Mode != "Edit") {
                            if (cComboType.GetValue() != "C")
                                cCbpAcBalance.PerformCallback('AcBalance~' + MainAcCode + '~');
                            else {
                                var WhichBankCall = obj.split('~')[5];
                                if (WhichBankCall == "TOCALL")
                                    cCbpDepstBankBalance.PerformCallback('DepstBalance~' + MainAcCode + '~');
                                else
                                    cCbpWithBankBalance.PerformCallback('WithBalance~' + MainAcCode + '~');
                            }
                        }
                        else {
                            if (cSCmb_Type.GetValue() != "C")
                                cCbpAcBalance.PerformCallback('AcBalance~' + MainAcCode + '~');
                            else {
                                var WhichBankCall = obj.split('~')[5];
                                if (WhichBankCall == "TOCALL")
                                    cCbpDepstBankBalance.PerformCallback('DepstBalance~' + MainAcCode + '~');
                                else
                                    cCbpWithBankBalance.PerformCallback('WithBalance~' + MainAcCode + '~');
                            }
                        }
                    }
                    if (Mode == "Edit") {
                        document.getElementById('<%=StxtSubAccount.ClientID%>').value = '';
                        document.getElementById("txtMainAccount_hidden").value = obj.split('~')[0];
                    }
                }
                if (isEdit == "Edit") {
                    SubLedgerType = obj.split('~')[1];
                    MainAcCode = obj.split('~')[0];
                    document.getElementById('hdn_SubLedgerType').value = SubLedgerType;
                    cGvAddRecordDisplay.PerformCallback('MainAccountChange~~');
                }
                if (SubLedgerType.toUpperCase() == 'NONE') {

                    document.getElementById('tdSubAccountLabel').style.display = 'none';
                    document.getElementById('tdSubAccountValue').style.display = 'none';
                    document.getElementById('hdn_MainAcc_Type').value = 'None';
                    document.getElementById('hdn_Brch_NonBrch').value = 'NAB';
                    document.getElementById('txtSubAccount_hidden').value = '';
                    var isEdit = obj.split('~')[4];
                    if (isEdit != undefined) {
                        document.getElementById('hdn_Brch_NonBrchE').value = 'NAB';
                    }
                }
                else {
                    document.getElementById('tdSubAccountLabel').style.display = 'inline';
                    document.getElementById('tdSubAccountValue').style.display = 'inline';
                }
            }
            else if (WhichAcType == "SUBAC") {
                var Branch = obj.split('~')[1];
                var isSubAcEdit = obj.split('~')[4];
                var SubAc = obj.split('~')[0];
                document.getElementById('hdn_Brch_NonBrch').value = Branch;
                cCbpAcBalance.PerformCallback('AcBalance~' + MainAcCode + '~' + SubAc);
                if (isSubAcEdit == 'Edit') {
                    document.getElementById('hdn_Brch_NonBrchE').value = Branch;
                    cGvAddRecordDisplay.PerformCallback('SubAccountChange~~');
                }
            }
            else if (WhichAcType == "CASHBANK") {
                var MainAccountID = obj.split('~')[0];
                cCbpBankBalance.PerformCallback('BankBalance~' + MainAccountID);
            }
            else {
                document.getElementById('hdn_Brch_NonBrch').value = 'NAB';
                var isSubAcEdit = obj.split('~')[4];
                var SubAc = obj.split('~')[0];
                cCbpAcBalance.PerformCallback('AcBalance~' + MainAcCode + '~' + SubAc);
                if (isSubAcEdit == 'Edit') {
                    document.getElementById('hdn_Brch_NonBrchE').value = 'NAB';
                    cGvAddRecordDisplay.PerformCallback('SubAccountChange~~');
                }
            }

        }
        function OnComboInstTypeSelectedIndexChanged() {
            SetAllDisplayNone();
            var txtSubAccountText = document.getElementById("txtSubAccount").value;
            var Mode = document.getElementById("hdn_Mode").value;
            var VoucherType;
            var InstType;
            if (Mode == "Entry") {
                VoucherType = cComboType.GetValue();
                InstType = cComboInstType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
                InstType = cComboInstType.GetValue();
            }


            AccountType = document.getElementById('hdnAccountType').value;
            if (VoucherType == "P") {
                if (AccountType == 'EXPENCES' && InstType != "CH") {
                    document.getElementById("tdPayeeLable").style.visibility = 'visible';
                    document.getElementById("tdPayeeValue").style.visibility = 'visible';
                }
                document.getElementById("tdpayment").style.display = 'inline'
                document.getElementById("tdpaymentValue").style.display = 'inline'
                if (InstType == "E") {
                    ctxtInstNo.SetText('E - Net');
                }
                else {
                    CmbClientBankCI.PerformCallback("SetFocusOnInstType~");
                }
                if (InstType != "CH") {
                    document.getElementById("tdINoLable").style.visibility = 'inherit'
                    document.getElementById("tdINoValue").style.visibility = 'inherit'
                    document.getElementById("tdIDateLable").style.visibility = 'inherit'
                    document.getElementById("tdIDateValue").style.visibility = 'inherit'
                }

            }
            if (VoucherType == "R") {
                if (InstType == "C" || InstType == "E") {
                    if (SubLedgerType.toUpperCase() == 'CUSTOMERS') {
                        document.getElementById("tdCBankLable").style.display = 'inline';
                        document.getElementById("tdCBankValue").style.visibility = 'visible';
                        var SubAccountID = document.getElementById("txtSubAccount_hidden").value;
                        if (SubAccountID != '') {
                            SubAccountID = SubAccountID.split('~')[0];
                            CmbClientBankCI.PerformCallback("ClientBankBind~" + SubAccountID);
                        }
                        else {
                            SubAccountID = document.getElementById('hdn_SubAccountIDE').value;
                            CmbClientBankCI.PerformCallback("ClientBankBind~" + SubAccountID);
                        }
                    }
                }
                else {
                    if (InstType != "CH") {
                        document.getElementById("tdIBankLable").style.display = 'inline';
                        document.getElementById("tdIBankValue").style.display = 'inline';
                    }
                }
                document.getElementById("tdRecieve").style.display = 'inline'
                document.getElementById("tdRecieveValue").style.display = 'inline'
                if (InstType == "E") {
                    ctxtInstNo.SetText('E - Net');
                }
                else {
                    ctxtInstNo.SetText('');
                }
                if (InstType != "CH") {
                    document.getElementById("tdINoLable").style.visibility = 'inherit'
                    document.getElementById("tdINoValue").style.visibility = 'inherit'
                    document.getElementById("tdIDateLable").style.visibility = 'inherit'
                    document.getElementById("tdIDateValue").style.visibility = 'inherit'
                }
            }
            if (VoucherType == "C") {
                //Contra Change
                document.getElementById("tdContraEntry").style.display = 'inline'
                if (InstType != "CH") {
                    document.getElementById("tdINoLable").style.visibility = 'inherit'
                    document.getElementById("tdINoValue").style.visibility = 'inherit'
                    document.getElementById("tdIDateLable").style.visibility = 'inherit'
                    document.getElementById("tdIDateValue").style.visibility = 'inherit'
                }
                else {
                    document.getElementById("tdINoLable").style.visibility = 'hidden'
                    document.getElementById("tdINoValue").style.visibility = 'hidden'
                    document.getElementById("tdIDateLable").style.visibility = 'hidden'
                    document.getElementById("tdIDateValue").style.visibility = 'hidden'
                    document.getElementById('<%=txtWithFrom.ClientID %>').value = '';
                    document.getElementById('<%=txtWithFrom.ClientID %>').focus();
                }

                if (InstType == "E") {
                    ctxtInstNo.SetText('E - Net');
                }
                else {
                    ctxtInstNo.SetText('');
                }
            }
        }
        function PopUp_StartPage_abtnOK_Click() {
            ResetPageOnSave();// Set The Page In Initial Page...So It is Forcly Used;
            document.getElementById('bTypeText').innerHTML = cComboType.GetText();
            document.getElementById('bBranchText').innerHTML = cComboBranch.GetText();
            document.getElementById('hdnType').value = cComboType.GetValue();
            document.getElementById('hdnDefaultBranch').value = cComboBranch.GetValue();
            var VoucherType = cComboType.GetValue();
            if (VoucherType == "C") {
                document.getElementById("tdMainAccountValue").style.display = 'none';
                document.getElementById("tdSubAccountValue").style.display = 'none';
                document.getElementById("tdMainAccountLabel").style.display = 'none';
                document.getElementById("tdSubAccountLabel").style.display = 'none';
                document.getElementById("tdCashBankLabel").style.display = 'none';
                document.getElementById("tdCashBankValue").style.display = 'none';
                document.getElementById("tdCBankBalLable").style.display = 'none';
                document.getElementById("tdCBankBalValue").style.display = 'none';
                document.getElementById("tdAcBalanceLable").style.display = 'none';
                document.getElementById("tdAcBalanceValue").style.display = 'none';
                document.getElementById("TblChangable").style.width = "40%";
                document.getElementById("TblAccountDetail").style.width = "20%";
                document.getElementById("TblChangable").style.styleFloat = 'left';
                document.getElementById("TblAccountDetail").style.styleFloat = 'left';
                document.getElementById("aAddDetail").focus();
            }
            else {
                cdtTDate.Focus();
            }
            cGvAddRecordDisplay.PerformCallback('ShowHideColumn~~' + VoucherType);
        }
        function ShowInstTypePopUp() {
            var txtSubAccountText = document.getElementById("txtSubAccount").value;
            var txtMainAccountText = document.getElementById("txtMainAccount").value;
            var txtBankAccounts_hiddenValue = document.getElementById('txtBankAccounts_hidden').value;
            var txtBankAccounts_Value = document.getElementById('txtBankAccounts').value;
            var Mode = document.getElementById("hdn_Mode").value;
            var VoucherType;
            var InstType;
            if (Mode == "Entry") {
                VoucherType = cComboType.GetValue();
                InstType = cComboInstType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
                InstType = cComboInstType.GetValue();
            }
            document.getElementById("TdAdd").style.display = 'inline';
            document.getElementById("TdUpdate").style.display = 'none';
            document.getElementById("TdCancel").style.display = 'inline';
            document.getElementById("TdCancelE").style.display = 'none';
            SetAllDisplayNone();
            if (VoucherType != "C") {
                if (txtBankAccounts_hiddenValue == "" || txtBankAccounts_Value == "") {
                    alert('Please Choose Cash Bank Account First');
                    document.getElementById("txtBankAccounts").focus();
                    return;
                }
                if (SubLedgerType.toUpperCase() == 'NONE' || SubLedgerType == "") {
                    if (txtMainAccountText == "" || txtMainAccountText == "No Record Found") {
                        alert('Please Choose Main Account First Properly');
                        document.getElementById("txtMainAccount").focus();
                        return;
                    }
                }
                else {
                    if (txtMainAccountText == "" || txtMainAccountText == "No Record Found") {
                        alert('Please Choose Main Account First Properly');
                        document.getElementById("txtMainAccount").focus();
                        return;
                    }
                    else if (txtSubAccountText == "" || txtSubAccountText == "No Record Found") {
                        alert('Please Choose Sub Account First Properly');
                        document.getElementById("txtSubAccount").focus();
                        return;
                    }
                }
                var CashBankType = txtBankAccounts_hiddenValue.split('~')[1];
                AddRemove_InstTypeItem(CashBankType, "", "");
                AccountType = document.getElementById('hdnAccountType').value;
                if (VoucherType == "P") {
                    if (AccountType == 'EXPENCES' && CashBankType.toUpperCase() != "CASH") {
                        document.getElementById("tdPayeeLable").style.visibility = 'visible';
                        document.getElementById("tdPayeeValue").style.visibility = 'visible';
                        cCmbPayee.PerformCallback('BindPayeeCombo~');
                    }
                    if (AccountType == 'EXPENCES' && CashBankType.toUpperCase() == "CASH") {
                        cCmbPayee.PerformCallback('BindPayeeCombo~');
                    }
                    document.getElementById("tdpayment").style.display = 'inline'
                    document.getElementById("tdpaymentValue").style.display = 'inline'
                    CmbClientBankCI.PerformCallback("SetFocusOnInstType~");

                }
                if (VoucherType == "R") {
                    if (SubLedgerType.toUpperCase().trim() == 'CUSTOMERS' && CashBankType.toUpperCase().trim() != 'CASH') {
                        document.getElementById("tdCBankLable").style.display = 'inline';
                        document.getElementById("tdCBankValue").style.visibility = 'visible';
                        document.getElementById("tdRecieve").style.display = 'inline'
                        document.getElementById("tdRecieveValue").style.display = 'inline'
                        var SubAccountID = document.getElementById("txtSubAccount_hidden").value;
                        SubAccountID = SubAccountID.split('~')[0];
                        CmbClientBankCI.PerformCallback("ClientBankBind~" + SubAccountID);
                    }
                    else {
                        document.getElementById("tdRecieve").style.display = 'inline'
                        document.getElementById("tdRecieveValue").style.display = 'inline'
                        CmbClientBankCI.PerformCallback("SetFocusOnInstType~");
                    }
                }
                OnSwitchingBetweenEditing_RefreshField();
                cPopUp_InstrumentDetail.Show();

            }
            else {
                AddRemove_InstTypeItem(CashBankType, "", "");
                document.getElementById("tdContraEntry").style.display = 'inline'
                document.getElementById('hdnType').value = VoucherType;
                OnSwitchingBetweenEditing_RefreshField();
                cPopUp_InstrumentDetail.Show();
                CmbClientBankCI.PerformCallback("SetFocusOnInstType~");
            }

        }
        function ShowInstTypePopUpE() {
            //             alert(document.getElementById("txtBankAccounts_hidden").value);   
            //Bank Validation On 20120319 : 'No Record Found has Been Saved in Bank Detail'
            var BankAccountValue = document.getElementById("txtBankAccounts_hidden").value.toUpperCase().trim();
            if (BankAccountValue == 'NO RECORD FOUND') {
                alert("Please Choose Bank First");
                document.getElementById("txtBankAccounts").focus();
                return;
            }
            ///////End Bank Validation...............
            var Mode = document.getElementById("hdn_Mode").value;
            var VoucherType;
            if (Mode == "Entry") {
                VoucherType = cComboType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
            }
            document.getElementById('txtSubAccount').value = '';
            document.getElementById('txtMainAccount').value = '';
            document.getElementById('txtSubAccount_hidden').value = '';
            document.getElementById('txtMainAccount_hidden').value = '';

            var strBoolSub = new String(IsSubAccountChange);
            var strBoolMain = new String(IsMainAccountChange);
            if (strBoolSub == "True" || strBoolMain == "True") {
                cInstrumentDetail_CallbackPanel.PerformCallback("BindControlWhenAccountChange~" + VoucherType);
            }
            else {
                cInstrumentDetail_CallbackPanel.PerformCallback("BindControl~" + VoucherType);
            }

        }
        function OnCmbClientBank_ValueChange(obj) {
            if (obj == 0) {
                document.getElementById("tdIBankLable").style.display = 'inline';
                document.getElementById("tdIBankValue").style.display = 'inline';
                document.getElementById("tdCBankLable").style.display = 'inline';
                document.getElementById("tdCBankValue").style.visibility = 'visible';
                document.getElementById("tdAuthLable").style.display = 'inline';
                document.getElementById("tdAuthValue").style.display = 'inline';
                //cCmbIssuingBank.PerformCallback("ThirdPartySelect~"+"a");
            }
            else {
                document.getElementById("tdCBankLable").style.display = 'inline';
                document.getElementById("tdCBankValue").style.visibility = 'visible';
                document.getElementById("tdIBankLable").style.display = 'none';
                document.getElementById("tdIBankValue").style.display = 'none';
                document.getElementById("tdAuthLable").style.display = 'none';
                document.getElementById("tdAuthValue").style.display = 'none';
            }

        }
        function SetAllDisplayNone() {
            document.getElementById("tdIBankLable").style.display = 'none';
            document.getElementById("tdIBankValue").style.display = 'none';
            document.getElementById("tdCBankLable").style.display = 'none';
            document.getElementById("tdCBankValue").style.visibility = 'hidden'
            document.getElementById("tdAuthLable").style.display = 'none';
            document.getElementById("tdAuthValue").style.display = 'none';
            document.getElementById("tdPayeeLable").style.visibility = 'hidden';
            document.getElementById("tdPayeeValue").style.visibility = 'hidden';
            document.getElementById("tdContraEntry").style.display = 'none';
            document.getElementById("tdpayment").style.display = 'none';
            document.getElementById("tdpaymentValue").style.display = 'none';
            document.getElementById("tdRecieve").style.display = 'none';
            document.getElementById("tdRecieveValue").style.display = 'none';
            document.getElementById("tdINoLable").style.visibility = 'hidden'
            document.getElementById("tdINoValue").style.visibility = 'hidden'
            document.getElementById("tdIDateLable").style.visibility = 'hidden'
            document.getElementById("tdIDateValue").style.visibility = 'hidden'

        }
        function SetAllValueNone() {

        }
        function CallContraAccount(obj1, obj2, obj3, WhichCall) {
            var MainAcCodeFrom;
            var MainAcCodeInTo;
            var MainAcCodeFromText;
            var MainAcCodeInToText;
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var Mode = document.getElementById("hdn_Mode").value;
            if (Mode == "Entry") {
                MainAcCodeFrom = document.getElementById('<%=txtWithFrom_hidden.ClientID%>').value.split('~')[0];
                MainAcCodeInTo = document.getElementById('<%=txtDepositInto_hidden.ClientID%>').value.split('~')[0];
                MainAcCodeFromText = document.getElementById('<%=txtWithFrom.ClientID%>').value.split('~')[0];
                MainAcCodeInToText = document.getElementById('<%=txtDepositInto.ClientID%>').value.split('~')[0];
            }
            else {
                //             MainAcCodeFrom=document.getElementById('<%=StxtWithFrom_hidden.ClientID%>').value.split('~')[0];
                //             MainAcCodeInTo=document.getElementById('<%=StxtDepstInto_hidden.ClientID%>').value.split('~')[0];
                MainAcCodeFrom = document.getElementById('<%=txtWithFrom_hidden.ClientID%>').value.split('~')[0];
                MainAcCodeInTo = document.getElementById('<%=txtDepositInto_hidden.ClientID%>').value.split('~')[0];
                MainAcCodeFromText = document.getElementById('<%=StxtWithFrom.ClientID%>').value.split('~')[0];
                MainAcCodeInToText = document.getElementById('<%=StxtDepstInto.ClientID%>').value.split('~')[0];
            }

            if (MainAcCodeFromText != "No Record Found" && MainAcCodeInToText != "No Record Found") {
                var MainAcCode_NotShowInListF = MainAcCodeFrom;
                var MainAcCode_NotShowInListT = MainAcCodeInTo;
                if (WhichCall != 'From') {
                    if (MainAcCode_NotShowInListF != "") {
                        strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+\' ~ \'+MainAccount_BankCashType as IntegrateMainAccount," +
                                        " cast(MainAccount_ReferenceID as varchar(20))+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+ MainAccount_AccountType+\'~~TOCALL~\'+MainAccount_AccountCode+\'~\'+MainAccount_BankCashType as AccountCode," +
                                        " MainAccount_Name,MainAccount_BankAcNumber,MainAccount_AccountCode from Master_MainAccount" +
                                        " where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\')" +
                                        " and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' OR Isnull(MainAccount_BankCompany,'')='') and MainAccount_ReferenceID not in(" + MainAcCode_NotShowInListF + ")) as t1"
                        strQuery_FieldName = "Top 10 *";
                        strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
                        strQuery_OrderBy = '';
                        strQuery_GroupBy = '';
                    }
                    else {
                        strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+\' ~ \'+MainAccount_BankCashType as IntegrateMainAccount," +
                                        " cast(MainAccount_ReferenceID as varchar(20))+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+ MainAccount_AccountType+\'~~TOCALL~\'+MainAccount_AccountCode+\'~\'+MainAccount_BankCashType as AccountCode," +
                                        " MainAccount_Name,MainAccount_BankAcNumber,MainAccount_AccountCode from Master_MainAccount" +
                                        " where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\')" +
                                        " and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' OR Isnull(MainAccount_BankCompany,'')='')) as t1"
                        strQuery_FieldName = "Top 10 *";
                        strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
                        strQuery_OrderBy = '';
                        strQuery_GroupBy = '';
                    }
                }
                else {
                    if (MainAcCode_NotShowInListT != "") {
                        strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+\' ~ \'+MainAccount_BankCashType as IntegrateMainAccount," +
                                        " cast(MainAccount_ReferenceID as varchar(20))+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+ MainAccount_AccountType+\'~~FROMCALL~\'+MainAccount_AccountCode+\'~\'+MainAccount_BankCashType as AccountCode," +
                                        " MainAccount_Name,MainAccount_BankAcNumber,MainAccount_AccountCode from Master_MainAccount" +
                                        " where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\')" +
                                        " and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' OR Isnull(MainAccount_BankCompany,'')='') and MainAccount_ReferenceID not in(" + MainAcCode_NotShowInListT + ")) as t1"
                        strQuery_FieldName = "Top 10 *";
                        strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
                        strQuery_OrderBy = '';
                        strQuery_GroupBy = '';
                    }
                    else {
                        strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+\' ~ \'+MainAccount_BankCashType as IntegrateMainAccount," +
                                        " cast(MainAccount_ReferenceID as varchar(20))+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+ MainAccount_AccountType+\'~~FROMCALL~\'+MainAccount_AccountCode+\'~\'+MainAccount_BankCashType as AccountCode," +
                                        " MainAccount_Name,MainAccount_BankAcNumber,MainAccount_AccountCode from Master_MainAccount" +
                                        " where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\')" +
                                        " and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' OR Isnull(MainAccount_BankCompany,'')='')) as t1"
                        strQuery_FieldName = "Top 10 *";
                        strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
                        strQuery_OrderBy = '';
                        strQuery_GroupBy = '';
                    }
                }
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');

            }
            else {
                alert('There is any Problem Selecting Bank.Please Check Again');
            }

        }
        function CallMainAccount(obj1, obj2, obj3) {
            var strQuery_Table = "Master_MainAccount";
            var strQuery_FieldName = " top 10 MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType as MainAccount_ReferenceID";
            var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
        }

        function CallSubAccount(obj1, obj2, obj3) {
            var MainAccountCode = document.getElementById("txtMainAccount_hidden").value;
            ajax_showOptions(obj1, obj2, obj3, MainAccountCode.split('~')[0] + '~N', 'Main');
        }
        function CallMainAccountE(obj1, obj2, obj3) {
            var strQuery_Table = "Master_MainAccount";
            var strQuery_FieldName = " top 10 MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+'~'+MainAccount_SubLedgerType+'~MAINAC~'+MainAccount_AccountType+'~Edit' as MainAccount_ReferenceID";
            var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and MainAccount_BankCashType not in('Cash','Bank')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
        }
        function CallSubAccountE(obj1, obj2, obj3) {
            var MainAcValue = MainAcCode;
            MainAcValue = MainAcValue + '~' + 'N';
            ajax_showOptions(obj1, obj2, obj3, MainAcValue, 'Main');
        }
        function CallListBank(obj1, obj2, obj3) {
            var strQuery_Table = "dbo.tbl_master_Bank";
            var strQuery_FieldName = "top 10 (isnull(bnk_bankName,\'\')+ \'-\'+ isnull(bnk_micrno,\'\') ) as BankName,bnk_id";
            var strQuery_WhereClause = "bnk_bankName Like (\'%RequestLetter%\') or bnk_micrno Like (\'%RequestLetter%\')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery));
        }
        function CallBankAccount(obj1, obj2, obj3) {
            var CurrentSegment = document.getElementById('hdn_CurrentSegment').value;
            var Mode = document.getElementById("hdn_Mode").value;
            var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
            var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+\' ~ \'+MainAccount_BankCashType as IntegrateMainAccount,MainAccount_AccountCode+\'~\'+MainAccount_BankCashType+\'~CASHBANK\' as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='')" + strPutSegment + ") as t1";
            var strQuery_FieldName = " Top 10 * ";
            var strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
        }
        function CallPayeeAccount(obj1, obj2, obj3) {
            var strQuery_Table = "tbl_master_contact";
            var strQuery_FieldName = "isnull(ltrim(rtrim(cnt_firstName)),\'\')+\' \'+isnull(cnt_middlename,\'\')+\' \'+isnull(cnt_lastname,\'\')+\' [\'+isnull(cnt_shortname,\'\')+']' as Payee,cnt_internalId+\'~~~INTERNALID \' as cnt_internalId";
            var strQuery_WhereClause = "cnt_internalId like \'VR%\' and (cnt_firstName like (\'%RequestLetter%\') or cnt_middlename like (\'%RequestLetter%\') or cnt_lastname like (\'%RequestLetter%\') or cnt_shortname like  (\'%RequestLetter%\') or cnt_shortname like  (\'%RequestLetter%\'))";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery));
        }
        function CallCustBankAccount(obj1, obj2, obj3) {
            var strQuery_Table = "tbl_trans_contactBankDetails as  TCBD inner  join tbl_master_Bank as MB on MB.bnk_id=TCBD.cbd_bankCode";
            var strQuery_FieldName = "distinct top 10 ltrim(rtrim(MB.bnk_bankName))+\' [\'+TCBD.cbd_accountNumber+\']\' as BankName,Cast(MB.bnk_id as varchar)+\'~~~CustBank\' as BankID";
            var strQuery_WhereClause = "MB.bnk_bankName like (\'%RequestLetter%\') or TCBD.cbd_accountNumber like (\'%RequestLetter%\')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery));
        }
        function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                temp.substring((pos + out.length), temp.length));
            }
            return temp;

        }
        function OnCancelButtonClick() {
            var AccountType = document.getElementById('hdnAccountType').value;
            var Mode = document.getElementById("hdn_Mode").value;
            var VoucherType;
            var InstType;
            if (Mode == "Entry") {
                VoucherType = cComboType.GetValue();
                InstType = cComboInstType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
                InstType = cComboInstType.GetValue();
            }
            ctxtInstNo.SetText("");
            cInstDate.SetDate(new Date());
            cComboInstType.SetSelectedIndex(0);
            document.getElementById('txtSubAccount').value = '';
            document.getElementById('txtMainAccount').value = '';
            document.getElementById('<%=txtNarration1.ClientID%>').value = '';
            if (VoucherType == "P") {
                ctxtPayment.SetText("0000000000000.00");
                document.getElementById('<%=txtAuthLetterRef.ClientID%>').value = '';
                document.getElementById('<%=B_AcBalance.ClientID %>').innerHTML = '';
                document.getElementById("txtMainAccount").focus();
            }
            if (VoucherType == "R") {
                if (InstType == "C" || InstType == "E") {
                    ctxtRecieve.SetText("0000000000000.00");
                    document.getElementById('<%=txtAuthLetterRef.ClientID%>').value = '';
               }
               else {
                   ctxtRecieve.SetText("0000000000000.00");
                   document.getElementById('<%=txtIssuingBank.ClientID%>').value = '';
                }
                document.getElementById('<%=B_AcBalance.ClientID %>').innerHTML = '';
               document.getElementById("txtMainAccount").focus();
           }
           if (VoucherType == "C") {
               ctxtAmount.SetText("0000000000000.00");
               document.getElementById('<%=txtDepositInto.ClientID%>').value = '';
                document.getElementById('<%=txtWithFrom.ClientID%>').value = '';
                document.getElementById('<%=txtDepositInto_hidden.ClientID%>').value = '';
                document.getElementById('<%=txtWithFrom_hidden.ClientID%>').value = '';
                document.getElementById('B_ImgSymbolWithBankBal').innerHTML = '';
                document.getElementById("b_WithBalance").innerText = "";
                document.getElementById('B_ImgSymbolDepstBankBal').innerHTML = "";
                document.getElementById("b_DepstBalance").innerText = "";
                cdtTDate.Focus();
            }
            cPopUp_InstrumentDetail.Hide();
        }
        function OnCancelEButtonClick() {
            document.getElementById('B_ImgSymbolWithBankBal').innerHTML = "";
            document.getElementById("b_WithBalance").innerText = "";
            document.getElementById('B_ImgSymbolDepstBankBal').innerHTML = "";
            document.getElementById("b_DepstBalance").innerText = "";
            cPopUp_InstrumentDetail.Hide();
            cGvAddRecordDisplay.PerformCallback('CancelEdit~~');
            document.getElementById("aAddDetail").focus();
        }
        function CmbClientBank_EndCallBack() {
            var strUndefined = new String(CmbClientBankCI.cpSetIndexZero);
            if (strUndefined != "undefined") {
                CmbClientBankCI.SetSelectedIndex(1);
                if (CmbClientBankCI.GetValue() == '0') {
                    document.getElementById("tdIBankLable").style.display = 'inline';
                    document.getElementById("tdIBankValue").style.display = 'inline';
                    document.getElementById("tdAuthLable").style.display = 'inline';
                    document.getElementById("tdAuthValue").style.display = 'inline';
                }
                //cComboInstType.SetSelectedIndex(0);
                //cComboInstType.GetMainElement().style.backgroundColor = "blue"; 
                cComboInstType.Focus();
                CmbClientBankCI.cpSetIndexZero = "undefined";
            }
            else {
                cComboInstType.Focus();
            }
            if (CmbClientBankCI.cpSetNextInstNo != undefined) {
                ctxtInstNo.SetText(CmbClientBankCI.cpSetNextInstNo)
            }

        }
        function CmbIssuingBank_EndCallBack() {

            if (cCmbIssuingBank.cpSetIndexZero != "undefined") {
                CmbClientBankCI.SetSelectedIndex(0);
            }
        }

        //        function OnComboBox1KeyUp(e) {
        //            var keyCode = e.htmlEvent.keyCode;
        //            character =String.fromCharCode(keyCode);
        //            cASPxComboBox2.PerformCallback("ThirdPartySelect~"+character);
        //         }
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
            //                alert('TransactionDate='+TransactionDate+'\n'+'MaxLockDate= '+MaxLockDate);
            //alert(ValueDate+'~'+ValueDateNumeric+'~'+VisibleIndexE);
            if (SelectedDateValue <= MaxLockDateNumeric) {
                alert('This Entry Date has been Locked.');
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

            //                alert('SelectedDateValue :'+SelectedDateValue.getTime()+
            //                '\nFinYearStartDateValue :'+FinYearStartDateValue.getTime()+
            //                '\nFinYearEndDateValue :'+FinYearEndDateValue.getTime());

            var SelectedDateNumericValue = SelectedDateValue.getTime();
            var FinYearStartDateNumericValue = FinYearStartDateValue.getTime();
            var FinYearEndDatNumbericValue = FinYearEndDateValue.getTime();
            if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {
                //                   alert('Between');
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    cdtTDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    cdtTDate.SetDate(new Date(FinYearEndDate));
                }
            }
            ///End OF Date Should Between Current Fin Year StartDate and EndDate
        }
        function OnAddButtonClick() {
            var SubAccountBranch = document.getElementById('hdn_Brch_NonBrch').value;
            //            var Mode=document.getElementById("hdn_Mode").value;
            //            if(Mode=="Entry") SubAccountBranch=document.getElementById('hdn_Brch_NonBrch').value;
            //            else SubAccountBranch=document.getElementById('hdn_Brch_NonBrchE').value;

            var VoucherType = document.getElementById('hdnType').value;
            var DefaultBranch = document.getElementById('hdnDefaultBranch').value;
            AddOrNot = InstrumentDetailForm_Validation(VoucherType);
            if (AddOrNot.split('-')[0] == "True") {
                cGvAddRecordDisplay.PerformCallback('VerificationForAddRecord~' + SubAccountBranch + '~' + VoucherType + '~' + DefaultBranch + '~' + AddOrNot.split('-')[1]);
            }
        }
        function OnAddRecord_RefreshField() {
            var AccountType = document.getElementById('hdnAccountType').value;
            var VoucherType;
            var InstType;
            var Mode = document.getElementById("hdn_Mode").value;
            if (Mode == "Entry") {
                VoucherType = cComboType.GetValue();
                InstType = cComboInstType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
                InstType = cComboInstType.GetValue();
            }
            ctxtInstNo.SetText("");
            cInstDate.SetDate(new Date());
            cComboInstType.SetSelectedIndex(0);
            document.getElementById('txtSubAccount').value = '';
            document.getElementById('<%=txtNarration1.ClientID%>').value = '';
            if (VoucherType == "P") {
                ctxtPayment.SetText("0000000000000.00");
                document.getElementById('<%=txtAuthLetterRef.ClientID%>').value = '';
                if (SubLedgerType.toUpperCase() == 'NONE') {
                    document.getElementById("txtMainAccount").focus();
                }
                else {
                    document.getElementById("txtSubAccount").focus();
                }
                document.getElementById('<%=B_AcBalance.ClientID %>').innerHTML = '';
            }
            if (VoucherType == "R") {
                if (InstType == "C" || InstType == "E") {
                    ctxtRecieve.SetText("0000000000000.00");
                    document.getElementById('<%=txtIssuingBank.ClientID%>').value = '';
                    document.getElementById('<%=txtAuthLetterRef.ClientID%>').value = '';
                }
                else {
                    ctxtRecieve.SetText("0000000000000.00");
                    document.getElementById('<%=txtIssuingBank.ClientID%>').value = '';
                }
                if (SubLedgerType.toUpperCase() == 'NONE') {
                    document.getElementById("txtMainAccount").focus();
                }
                else {
                    document.getElementById("txtSubAccount").focus();
                }
                document.getElementById('<%=B_AcBalance.ClientID %>').innerHTML = '';
            }
            if (VoucherType == "C") {
                ctxtAmount.SetText("0000000000000.00");
                document.getElementById('<%=txtDepositInto.ClientID%>').value = '';
                document.getElementById('<%=txtDepositInto_hidden.ClientID%>').value = '';
                document.getElementById('<%=txtWithFrom.ClientID%>').value = '';
                document.getElementById('<%=txtWithFrom_hidden.ClientID%>').value = '';
                document.getElementById('B_ImgSymbolWithBankBal').innerHTML = "";
                document.getElementById("b_WithBalance").innerText = "";
                document.getElementById('B_ImgSymbolDepstBankBal').innerHTML = "";
                document.getElementById("b_DepstBalance").innerText = "";
                document.getElementById("aAddDetail").focus();
            }

            cPopUp_InstrumentDetail.Hide();
        }
        function OnSwitchingBetweenEditing_RefreshField() {
            var AccountType = document.getElementById('hdnAccountType').value;
            var VoucherType;
            var InstType;
            var Mode = document.getElementById("hdn_Mode").value;
            if (Mode == "Entry") {
                VoucherType = cComboType.GetValue();
                InstType = cComboInstType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
                InstType = cComboInstType.GetValue();
            }
            ctxtInstNo.SetText("");
            cInstDate.SetDate(new Date());
            cComboInstType.SetSelectedIndex(0);
            document.getElementById('<%=txtNarration1.ClientID%>').value = '';
           if (VoucherType == "P") {
               ctxtPayment.SetText("0000000000000.00");
               document.getElementById('<%=txtAuthLetterRef.ClientID%>').value = '';
            }
            if (VoucherType == "R") {
                if (InstType == "C" || InstType == "E") {
                    ctxtRecieve.SetText("0000000000000.00");
                    document.getElementById('<%=txtIssuingBank.ClientID%>').value = '';
                    document.getElementById('<%=txtAuthLetterRef.ClientID%>').value = '';
                }
                else {
                    ctxtRecieve.SetText("0000000000000.00");
                    document.getElementById('<%=txtAuthLetterRef.ClientID%>').value = '';
                    document.getElementById('<%=txtIssuingBank.ClientID%>').value = '';
                }
            }
            if (VoucherType == "C") {
                ctxtAmount.SetText("0000000000000.00");
                document.getElementById('<%=txtDepositInto.ClientID%>').value = '';
                document.getElementById('<%=txtWithFrom.ClientID%>').value = '';
            }
            cPopUp_InstrumentDetail.Hide();
        }
        function DiscardButtonClick() {
            var data = 'Are You Sure.It will Discard All Data You Entered';
            var answer = confirm(data)
            if (answer) {
                cGvAddRecordDisplay.PerformCallback('Discard~~~~');
            }
        }
        function GvAddRecordDisplay_EndCallback() {
            var TotalParameter;
            var ObjTd;

            var strUndefined = new String(cGvAddRecordDisplay.cpSuccessDiscard);
            if (strUndefined != "undefined") {
                if (cGvAddRecordDisplay.cpSuccessDiscard == "Problem") {
                    alert('There is Some Problem. Sry for InConvenience');
                    cGvAddRecordDisplay.cpSuccessDiscard = "undefined";
                }
                else if (cGvAddRecordDisplay.cpSuccessDiscard == "SuccessDiscard") {
                    alert('Records Successfully Discard');
                    cGvAddRecordDisplay.cpSuccessDiscard = "undefined";
                }
                else {
                    alert('No Record Exists');
                    cGvAddRecordDisplay.cpSuccessDiscard = "undefined";
                }
                OnCancelButtonClick();
            }
            strUndefined = new String(cGvAddRecordDisplay.cpMainAccountChange);
            if (strUndefined != "undefined") {
                IsMainAccountChange = 'True';
                //document.getElementById('GvAddRecordDisplay_DXTDGScol1').focus();
                //var visibleIndex=cGvAddRecordDisplay.cpSubAccountClear;
                //                var txtSubAccountE = aspxGetControlCollection().Get('GvAddRecordDisplay_ef' + visibleIndex + '_txtSubAccountE');
                //                document.getElementById('cGvAddRecordDisplay_txtMainAccountE').value='';
                //                document.getElementById('cGvAddRecordDisplay_txtMainAccountE').focus();
            }
            else {
                IsMainAccountChange = 'False';
            }
            strUndefined = new String(cGvAddRecordDisplay.cpSubAccountChange);
            if (strUndefined != "undefined") {
                IsSubAccountChange = "True";
                Param_SubAccountID = cGvAddRecordDisplay.cpSubAccountChange;
                //document.getElementById('cGvAddRecordDisplay_a1').focus();
            }
            else {
                IsSubAccountChange = "False";
                Param_SubAccountID = '';
            }
            strUndefined = new String(cGvAddRecordDisplay.cpClearEditSetUp);
            if (strUndefined != "undefined") {
                IsSubAccountChange = 'False';
                Param_SubAccountID = '';
                MainAcCode = '';
                SubLedgerType = '';
                var CashBankID = cGvAddRecordDisplay.cpClearEditSetUp.split('~')[1];
                //cCbpBankBalance.PerformCallback('BankBalance~'+CashBankID);
                cGvAddRecordDisplay.cpClearEditSetUp = "undefined";
            }
            strUndefined = new String(cGvAddRecordDisplay.cpSetMainAcCodeSubLedgerTypeWhenEdit);
            if (strUndefined != "undefined") {
                MainAcCode = cGvAddRecordDisplay.cpSetMainAcCodeSubLedgerTypeWhenEdit.split('~')[0];
                SubLedgerType = cGvAddRecordDisplay.cpSetMainAcCodeSubLedgerTypeWhenEdit.split('~')[1];
                cGvAddRecordDisplay.cpSetMainAcCodeSubLedgerTypeWhenEdit = "undefined";
            }
            strUndefined = new String(cGvAddRecordDisplay.cpInstDuplicateRecord);
            var SubAccountBranch = document.getElementById('hdn_Brch_NonBrch').value;
            var Mode = document.getElementById("hdn_Mode").value;
            var VoucherType;
            var InstType;
            if (Mode == "Entry") {
                VoucherType = cComboType.GetValue();
                InstType = cComboInstType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
                InstType = cComboInstType.GetValue();
            }
            var DefaultBranch = document.getElementById('hdnDefaultBranch').value;
            if (strUndefined != "undefined~AddRecord" && strUndefined != "undefined") {
                var data = cGvAddRecordDisplay.cpInstDuplicateRecord + "\n\nDo you Want To Continue?";
                var answer = confirm(data);
                if (answer) {
                    cGvAddRecordDisplay.PerformCallback('Add~' + SubAccountBranch + '~' + VoucherType + '~' + DefaultBranch + '~' + AddOrNot.split('-')[1] + '~' + InstType);
                    OnAddRecord_RefreshField();
                }
                cGvAddRecordDisplay.cpInstDuplicateRecord = "undefined";
                ctxtInstNo.Focus();
            }
            else {
                if (strUndefined.split('~')[1] == "AddRecord") {
                    cGvAddRecordDisplay.PerformCallback('Add~' + SubAccountBranch + '~' + VoucherType + '~' + DefaultBranch + '~' + AddOrNot.split('-')[1] + '~' + InstType);
                    OnAddRecord_RefreshField();
                }
            }
            strUndefined = new String(cGvAddRecordDisplay.cpAfterAddData_ToGride);
            if (strUndefined != "undefined") {
                document.getElementById("txtSubAccount_hidden").value = '';
                if (document.getElementById("hdn_Brch_NonBrch").value != 'NAB') {
                    //Cause hdn_Brch_NonBrch is assigned every time  subaccount selection
                    //but assion only one time when Main Account's SubledgerType is None
                    //Then I Assion NAB in This Field
                    document.getElementById("hdn_Brch_NonBrch").value = '';
                }
                document.getElementById('<%=txtWithFrom_hidden.ClientID%>').value = '';
                document.getElementById('<%=txtDepositInto_hidden.ClientID%>').value = '';
                document.getElementById('B_ImgSymbolWithBankBal').innerHTML = "";
                document.getElementById("b_WithBalance").innerText = "";
                document.getElementById('B_ImgSymbolDepstBankBal').innerHTML = "";
                document.getElementById("b_DepstBalance").innerText = "";
                if (cGvAddRecordDisplay.cpAfterAddData_ToGride.split('~')[1] == 'R') {
                    cCbpBankBalance.PerformCallback('BankBalanceAfterReceipt~' + document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML);
                }
                if (cGvAddRecordDisplay.cpAfterAddData_ToGride.split('~')[1] == 'P') {
                    cCbpBankBalance.PerformCallback('BankBalanceAfterPayement~' + cGvAddRecordDisplay.cpAfterAddData_ToGride.split('~')[2]);
                }

            }
            strUndefined = new String(cGvAddRecordDisplay.cpSaveSuccessOrFail);
            if (strUndefined != "undefined") {
                strUndefined = new String(cGvAddRecordDisplay.cpSaveSuccessOrFail);
                if (strUndefined == "Problem") {
                    alert("There is Some Problem. Sry for InConvenience");
                    cGvAddRecordDisplay.cpSaveSuccessOrFail = "undefined";
                }
                else if (strUndefined == "Success") {
                    ResetPageOnSave();
                    alert("Records Successfully Saved");
                    var Mode = document.getElementById("hdn_Mode").value;
                    parent.editwin.close();
                    parent.callback();
                }
                else {
                }
            }
            if (cGvAddRecordDisplay.cpSetValueOnLoad != undefined) {
                document.getElementById("txtBankAccounts").value = cGvAddRecordDisplay.cpSetValueOnLoad.split('*')[0];
                document.getElementById('txtBankAccounts_hidden').value = cGvAddRecordDisplay.cpSetValueOnLoad.split('*')[1];
                var BankCode = cGvAddRecordDisplay.cpSetValueOnLoad.split('*')[1].split('~')[0];
                cCbpBankBalance.PerformCallback('BankBalance~' + BankCode);
                document.getElementById("txtNarration").value = cGvAddRecordDisplay.cpSetValueOnLoad.split('*')[2];
                cdtTDate.SetDate(new Date(cGvAddRecordDisplay.cpSetValueOnLoad.split('*')[3]));
                CurrencySetting(cGvAddRecordDisplay.cpSetValueOnLoad.split('*')[4]);
            }
            if (cGvAddRecordDisplay.cpClearHiddenField != undefined) {
                ClearHiddenField();
                parent.editwin.close();
            }
            if (cGvAddRecordDisplay.cpExit != undefined) {
                cGvAddRecordDisplay.PerformCallback('ClearSession~Start~');
            }
            strUndefined = new String(cGvAddRecordDisplay.cpAfterRowDeleted);
            if (strUndefined != "undefined") {
                var MainAccountID = cGvAddRecordDisplay.cpAfterRowDeleted.split('~')[1];
                cCbpBankBalance.PerformCallback('BankBalance~' + MainAccountID);
                cGvAddRecordDisplay.cpAfterRowDeleted = "undefined";
            }
            if (cGvAddRecordDisplay.cpOneTagEntry.split('~')[0] != "undefined") {
                if (cGvAddRecordDisplay.cpOneTagEntry.split('~')[1] == "Exists") {
                    cdtTDate.SetEnabled(false);
                    BtnDeleteVoucher.SetEnabled(false);
                    document.getElementById('txtBankAccounts').disabled = true;
                    document.getElementById('txtNarration').focus();
                }
                if (cGvAddRecordDisplay.cpOneTagEntry.split('~')[1] == "NotExists") {
                    cdtTDate.SetEnabled(true);
                    BtnDeleteVoucher.SetEnabled(true);
                    document.getElementById('txtBankAccounts').disabled = false;
                    cdtTDate.SetFocus();

                }
            }
            if (cGvAddRecordDisplay.cpCBDelete != undefined) {
                cDeleteMsgPopUp.Hide();
                alert(cGvAddRecordDisplay.cpCBDelete);
                parent.editwin.close();
                parent.callback();
            }
            height();
        }
        function ClearHiddenField() {
            document.getElementById("hdnDefaultBranch").value = '';
            document.getElementById("hdnType").value = '';
            document.getElementById("hdnAccountType").value = '';
            document.getElementById("txtMainAccount_hidden").value = '';
            document.getElementById("txtSubAccount_hidden").value = '';
            document.getElementById("hdn_Brch_NonBrch").value = '';
            document.getElementById("hdn_SubLedgerType").value = '';
            document.getElementById("hdn_MainAcc_Type").value = '';
            document.getElementById("hdn_SubAccountIDE").value = '';
            document.getElementById("txtBankAccounts_hidden").value = '';
            document.getElementById("hdn_Mode").value = '';
            document.getElementById("hdn_PayeeIDOnUpdate").value = '';
            document.getElementById("hdn_Brch_NonBrchE").value = '';
            document.getElementById("<%=StxtPayeeAc_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtDepstInto_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtWithFrom_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtIssueBank_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtCustBank_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtSubAccount_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtMainAccount_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtCashBankAc_hidden.ClientID%>").value = '';
            document.getElementById("<%=txtDepositInto_hidden.ClientID%>").value = '';
            document.getElementById("<%=txtWithFrom_hidden.ClientID%>").value = '';
            document.getElementById("<%=txtIssuingBank_hidden.ClientID%>").value = '';
            document.getElementById("hdn_CashBankType_InstTypeE").value = "";
        }
        function InstrumentDetail_CallbackPanel_EndCallBack() {
            var TotalParameter;
            var VoucherType;
            var SubAccountID;
            var strUndefined;
            strUndefined = new String(cInstrumentDetail_CallbackPanel.cpSetHdnFldValueOnEditDetail);
            if (strUndefined != "undefined") {
                var strValues = cInstrumentDetail_CallbackPanel.cpSetHdnFldValueOnEditDetail;
                document.getElementById("hdnType").value = strValues.split('~')[0];
                document.getElementById("hdnDefaultBranch").value = strValues.split('~')[6];
                var Mode = document.getElementById("hdn_Mode").value;
                if (Mode == "Edit") {
                    document.getElementById("hdn_Brch_NonBrchE").value = strValues.split('~')[1];
                }
                else {
                    document.getElementById("hdn_Brch_NonBrch").value = strValues.split('~')[1];
                }
                document.getElementById("hdnAccountType").value = strValues.split('~')[2];
                document.getElementById("hdn_SubLedgerType").value = strValues.split('~')[3];
                document.getElementById("hdn_MainAcc_Type").value = strValues != '' ? strValues.split('~')[4] : '';
                document.getElementById("hdn_SubAccountIDE").value = strValues != '' ? strValues.split('~')[5] : '';
            }
            else {
                document.getElementById("hdnType").value = '';
                document.getElementById("hdnDefaultBranch").value = '';
                document.getElementById("hdnAccountType").value = '';
                document.getElementById("hdn_SubLedgerType").value = '';
                document.getElementById("hdn_MainAcc_Type").value = '';
                document.getElementById("hdn_SubAccountIDE").value = '';
            }
            strUndefined = new String(cInstrumentDetail_CallbackPanel.cpVisbleControlString);
            if (strUndefined != "undefined") {
                document.getElementById("hdn_CashBankType_InstTypeE").value = "";
                TotalParameter = cInstrumentDetail_CallbackPanel.cpVisbleControlString.split('~')[0];
                VoucherType = cInstrumentDetail_CallbackPanel.cpVisbleControlString.split('~')[1];
                SubAccountID = cInstrumentDetail_CallbackPanel.cpVisbleControlString.split('~')[2];
                var CashBankType = '';
                var CashBankType_InstType = '';
                if (cInstrumentDetail_CallbackPanel.cpCashBankType_InstTypeE != undefined) {
                    CashBankType = cInstrumentDetail_CallbackPanel.cpCashBankType_InstTypeE.split('_')[0];
                    CashBankType_InstType = cInstrumentDetail_CallbackPanel.cpCashBankType_InstTypeE.split('_')[1];
                    document.getElementById("hdn_CashBankType_InstTypeE").value = CashBankType_InstType;
                }
                var ParameterString = cInstrumentDetail_CallbackPanel.cpSetValuesParameter;
                SetAllDisplayNone();
                AddRemove_InstTypeItem(CashBankType, CashBankType_InstType, VoucherType);
                if (CashBankType_InstType == "DRAFT") {
                    document.getElementById("tdINoLable").style.visibility = 'inherit'
                    document.getElementById("tdINoValue").style.visibility = 'inherit'
                    document.getElementById("tdIDateLable").style.visibility = 'inherit'
                    document.getElementById("tdIDateValue").style.visibility = 'inherit'
                }
                for (var i = 3; i < TotalParameter; i++) {
                    ObjTd = cInstrumentDetail_CallbackPanel.cpVisbleControlString.split('~')[i];
                    document.getElementById(ObjTd).style.display = 'inline';
                    document.getElementById(ObjTd).style.visibility = 'visible';
                }
                if (VoucherType == 'C' && CashBankType_InstType == 'Cash')//Contra Change
                {
                    document.getElementById("tdINoLable").style.visibility = 'hidden'
                    document.getElementById("tdINoValue").style.visibility = 'hidden'
                    document.getElementById("tdIDateLable").style.visibility = 'hidden'
                    document.getElementById("tdIDateValue").style.visibility = 'hidden'
                }
                else if (VoucherType == 'R' && (TotalParameter == '7' || TotalParameter == '11')) {
                    //CmbClientBankCI.PerformCallback("ClientBankBind~"+SubAccountID);
                }
                else {


                }
                SetValueOnEdit(VoucherType, ParameterString);
                cPopUp_InstrumentDetail.Show();
                document.getElementById("TdAdd").style.display = 'none';
                document.getElementById("TdUpdate").style.display = 'inline';
                document.getElementById("TdCancel").style.display = 'none';
                document.getElementById("TdCancelE").style.display = 'inline';
            }
            strUndefined = new String(cInstrumentDetail_CallbackPanel.cpUpdated);
            if (strUndefined != "undefined") {
                alert('Data Successfully Updated.\n Please Save for Effect');
                cPopUp_InstrumentDetail.Hide();
                cGvAddRecordDisplay.PerformCallback('CancelEdit~~');
                document.getElementById("aAddDetail").focus();

            }
            strUndefined = new String(cInstrumentDetail_CallbackPanel.cpBindPayee);
            if (strUndefined != "undefined") {
                var PayeeAcID = strUndefined.split('~')[1];
                cCbpPayee.PerformCallback('BindPayeeComboWithValue~' + PayeeAcID);
            }
            strUndefined = new String(cInstrumentDetail_CallbackPanel.cpBindCustBank);
            if (strUndefined != "undefined") {
                var SubAccountID = strUndefined.split('~')[0];
                var SelectedValue = strUndefined.split('~')[1];
                CmbClientBankCI.PerformCallback("ClientBankBindWithSelectedValue~" + SubAccountID + '~' + SelectedValue);
            }

        }
        function SetValueOnEdit(VoucherType, ParameterString) {
            var WhichTypeSetting = ParameterString.split('~')[0];
            var InstType = ParameterString.split('~')[1];
            var InstNo = ParameterString.split('~')[2];
            var InstDate = ParameterString.split('~')[3];
            var CustBankID = ParameterString.split('~')[4];
            var IssueBankID = ParameterString.split('~')[5];
            var AuthLetterRef = ParameterString.split('~')[6];
            var PayeeAcID = ParameterString.split('~')[7];
            var LineNarration = ParameterString.split('~')[8];
            var Payment = ParameterString.split('~')[9];
            var Recieve = ParameterString.split('~')[10];
            var WithDrawFrom = ParameterString.split('~')[11];
            var DepositInto = ParameterString.split('~')[12];
            var Amount = ParameterString.split('~')[13];
            var IssueBankName = ParameterString.split('~')[14];
            var WithDrawFromName = ParameterString.split('~')[15];
            var DepositIntoName = ParameterString.split('~')[16];
            var MainAccountID = ParameterString.split('~')[17];
            var SubAccountID = ParameterString.split('~')[18];
            SubLedgerType = WhichTypeSetting;
            //            alert(WhichTypeSetting+'~ 1: '+InstType+'~ 2: '+InstNo+'~ 3: '+InstDate+'~ 4: '+CustBankID+
            //            '~ 5: '+IssueBankID+'~ 6: '+ AuthLetterRef+'~ 7: '+PayeeAcID+'~8: '+LineNarration+'~9: '+
            //            Payment+'~10: '+Recieve+'11: '+WithDrawFrom+'~ 12: '+ DepositInto+'~ 13: '+Amount+'~ 14: '+
            //            IssueBankName+'~ 15: '+WithDrawFromName+'~ 16: '+DepositIntoName+'~ 17: '+MainAccountID+'~18: '+
            //            SubAccountID);
            if (VoucherType == "P") {
                OnSwitchingBetweenEditing_RefreshField();
                strInstType = document.getElementById("hdn_CashBankType_InstTypeE").value.split('_')[1];
                cComboInstType.PerformCallback("SetItemsAndSelectValue~" + strInstType + "~~" + VoucherType);
                ctxtInstNo.SetText(InstNo);
                cInstDate.SetDate(new Date(InstDate));
                document.getElementById('<%=txtNarration1.ClientID %>').value = LineNarration;
                if (WhichTypeSetting == 'EXPENCES') {
                    //cInstrumentDetail_CallbackPanel.GetEditor("CmbPayee").SetValue(PayeeAcID);
                }
                var strPayment = new String(Payment);
                ctxtPayment.SetText("0000000000000.00");
                ctxtPayment.SetText(strPayment);

            }
            if (VoucherType == "R") {
                OnSwitchingBetweenEditing_RefreshField();
                strInstType = document.getElementById("hdn_CashBankType_InstTypeE").value.split('_')[1];
                cComboInstType.PerformCallback("SetItemsAndSelectValue~" + strInstType + "~~" + VoucherType);
                ctxtInstNo.SetText(InstNo);
                cInstDate.SetDate(new Date(InstDate));
                SubLedgerType = WhichTypeSetting;
                document.getElementById('<%=txtNarration1.ClientID %>').value = LineNarration;
                if (InstType != "CH") {
                    if (InstType == "C" || InstType == "E") {

                        if (WhichTypeSetting.toUpperCase() == 'CUSTOMERS' && CustBankID == '0') {
                            CmbClientBankCI.SetSelectedIndex(0);
                            document.getElementById('<%=txtIssuingBank.ClientID %>').value = IssueBankName;
                             document.getElementById('<%=txtIssuingBank_hidden.ClientID %>').value = IssueBankID;
                             document.getElementById('<%=txtAuthLetterRef.ClientID %>').value = AuthLetterRef;
                         }
                         else {
                             CmbClientBankCI.SetValue(CustBankID);
                         }
                     }
                     else {
                         document.getElementById('<%=txtIssuingBank.ClientID %>').value = IssueBankName;
                         document.getElementById('<%=txtIssuingBank_hidden.ClientID %>').value = IssueBankID;
                     }
                 }

                 var strRecieve = new String(Recieve);
                 ctxtRecieve.SetText("0000000000000.00");
                 ctxtRecieve.SetText(strRecieve);


             }
             if (VoucherType == "C") {
                 strInstType = document.getElementById("hdn_CashBankType_InstTypeE").value.split('_')[1];
                 OnSwitchingBetweenEditing_RefreshField();
                 cComboInstType.PerformCallback("SetItemsAndSelectValue~" + strInstType + "~~" + VoucherType);
                 ctxtInstNo.SetText(InstNo);
                 cInstDate.SetDate(new Date(InstDate));
                 document.getElementById('<%=txtNarration1.ClientID %>').value = LineNarration;
                document.getElementById('<%=txtWithFrom.ClientID %>').value = WithDrawFromName;
                document.getElementById('<%=txtDepositInto.ClientID %>').value = DepositIntoName;
                document.getElementById('<%=txtWithFrom_hidden.ClientID %>').value = WithDrawFrom;
                document.getElementById('<%=txtDepositInto_hidden.ClientID %>').value = DepositInto;
                cCbpWithBankBalance.PerformCallback('WithBalance~' + WithDrawFrom + '~');
                cCbpDepstBankBalance.PerformCallback('DepstBalance~' + DepositInto + '~');
                var strAmount = new String(Amount);
                ctxtAmount.SetText("0000000000000.00");
                ctxtAmount.SetText(strAmount);
            }

        }
        function OnUpdateButtonClick() {
            var SubAccountBranch;
            var DefaultBranch;
            var Mode = document.getElementById("hdn_Mode").value;
            if (Mode == "Entry") {
                SubAccountBranch = document.getElementById('hdn_Brch_NonBrch').value;
                DefaultBranch = cComboBranch.GetValue();
            }
            else {
                SubAccountBranch = document.getElementById('hdn_Brch_NonBrchE').value;
                DefaultBranch = document.getElementById("hdnDefaultBranch").value;
            }
            //alert(SubAccountBranch+'~'+DefaultBranch);
            var VoucherType = document.getElementById('hdnType').value;
            var ClientBankID = CmbClientBankCI.GetValue();
            var PayeeAcID = cCmbPayee.GetValue();
            var AddOrNotE = InstrumentDetailForm_Validation(VoucherType);
            var InstType = cComboInstType.GetValue();
            if (AddOrNotE.split('-')[0] == "True") {
                cInstrumentDetail_CallbackPanel.PerformCallback('Update~' + VoucherType + '~' + SubAccountBranch + '~' + DefaultBranch + '~' + ClientBankID + '~' + PayeeAcID + '~' + InstType);
            }
        }
        function ShowPopUpWhenAccountChangeInEditing() {
            SetAllDisplayNone();
            document.getElementById("TdAdd").style.display = 'none';
            document.getElementById("TdUpdate").style.display = 'inline';
            document.getElementById("TdCancel").style.display = 'none';
            document.getElementById("TdCancelE").style.display = 'inline';
            var VoucherType;
            var Mode = document.getElementById("hdn_Mode").value;
            if (Mode == "Entry") {
                VoucherType = cComboType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
            }
            if (VoucherType != "C") {
                var txtBankAccounts_hiddenValue = document.getElementById('txtBankAccounts_hidden').value;
                var CashBankType = txtBankAccounts_hiddenValue.split('~')[1];
                AddRemove_InstTypeItem(CashBankType, "", "");
                AccountType = document.getElementById('hdnAccountType').value;
                if (VoucherType == "P") {
                    if (AccountType == 'EXPENCES' && CashBankType.toUpperCase() != "CASH") {
                        document.getElementById("tdPayeeLable").style.visibility = 'visible';
                        document.getElementById("tdPayeeValue").style.visibility = 'visible';
                        cCmbPayee.PerformCallback('BindPayeeCombo~');
                    }
                    if (AccountType == 'EXPENCES' && CashBankType.toUpperCase() == "CASH") {
                        cCmbPayee.PerformCallback('BindPayeeCombo~');
                    }
                    document.getElementById("tdpayment").style.display = 'inline'
                    document.getElementById("tdpaymentValue").style.display = 'inline'
                    CmbClientBankCI.PerformCallback("SetFocusOnInstType~");
                }
                if (VoucherType == "R") {
                    if (SubLedgerType.toUpperCase() == 'CUSTOMERS' && CashBankType.toUpperCase() != 'CASH') {
                        document.getElementById("tdCBankLable").style.display = 'inline';
                        document.getElementById("tdCBankValue").style.visibility = 'visible';
                        document.getElementById("tdRecieve").style.display = 'inline'
                        document.getElementById("tdRecieveValue").style.display = 'inline'
                        CmbClientBankCI.PerformCallback("ClientBankBind~" + Param_SubAccountID);
                    }
                    else {
                        document.getElementById("tdRecieve").style.display = 'inline'
                        document.getElementById("tdRecieveValue").style.display = 'inline'
                        CmbClientBankCI.PerformCallback("SetFocusOnInstType~");
                    }
                }
                OnSwitchingBetweenEditing_RefreshField();
                cPopUp_InstrumentDetail.Show();

            }
            else {
                document.getElementById("tdContraEntry").style.display = 'inline'
                document.getElementById('hdnType').value = VoucherType;
                OnSwitchingBetweenEditing_RefreshField();
                cPopUp_InstrumentDetail.Show();
                CmbClientBankCI.PerformCallback("SetFocusOnInstType~");
            }
        }
        function ComboInstType_EndCallBack() {
            var Mode = document.getElementById("hdn_Mode").value;
            var VoucherType;
            var InstType;
            if (Mode == "Entry") {
                VoucherType = cComboType.GetValue();
                InstType = cComboInstType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
                InstType = cComboInstType.GetValue();
            }
            if (InstType != "CH") {
                document.getElementById("tdINoLable").style.visibility = 'inherit'
                document.getElementById("tdINoValue").style.visibility = 'inherit'
                document.getElementById("tdIDateLable").style.visibility = 'inherit'
                document.getElementById("tdIDateValue").style.visibility = 'inherit'
            }
            if (document.getElementById("hdn_CashBankType_InstTypeE").value == "DRAFT") cComboInstType.SetSelectedIndex(1);
            if (document.getElementById("hdn_CashBankType_InstTypeE").value == "CASH") cComboInstType.SetSelectedIndex(0);
            if (cComboInstType.cpSetValue != undefined) {
                if (cComboInstType.cpSetValue == "DRAFT")
                    cComboInstType.SetValue('D');
                else if (cComboInstType.cpSetValue == "Cheque")
                    cComboInstType.SetValue('C');
                else
                    cComboInstType.SetValue('E');
            }
        }
        function InstrumentDetailForm_Validation(VoucherTypeV) {
            var WhichInst_Verification;
            var InstTypeV = cComboInstType.GetValue();
            //            alert(VoucherTypeV+'~'+InstTypeV+'~'+SubLedgerType);

            if (VoucherTypeV != "C") {
                if (VoucherTypeV == "P") {
                    var strPayment = new String(ctxtPayment.GetText());
                    if (strPayment == "0.00") {
                        alert('Payment Amount Can not be Zero');
                        ctxtPayment.Focus();
                        return "false-";
                    }
                    WhichInst_Verification = 'CASHBANK';
                    return "True-" + WhichInst_Verification

                }
                if (VoucherTypeV == "R") {
                    if (InstTypeV.toUpperCase() != "CH") {
                        if (ctxtInstNo.GetText() == "") {
                            alert('Please Enter Instrument Number');
                            ctxtInstNo.Focus();
                            return "false-";
                        }
                        if (SubLedgerType == 'CUSTOMERS') {
                            var ClientBankID = CmbClientBankCI.GetValue();
                            if (ClientBankID == "0" && InstTypeV != "D") {
                                var IssueBankTextValue = document.getElementById('<%=txtIssuingBank.ClientID %>').value;
                                var IssueBankHiddenValue = document.getElementById('<%=txtIssuingBank_hidden.ClientID %>').value;

                                if (IssueBankTextValue == "" || IssueBankTextValue == "No Record Found") {
                                    alert('Please Select Issue Bank');
                                    document.getElementById('<%=txtIssuingBank.ClientID %>').value = '';
                                    document.getElementById('<%=txtIssuingBank.ClientID %>').focus();
                                    return "false-";
                                }
                                var AuthLetterTextValue = document.getElementById('<%=txtAuthLetterRef.ClientID %>').value;
                                if (AuthLetterTextValue == "") {
                                    alert('Auth. Letter Ref Required');
                                    document.getElementById('<%=txtAuthLetterRef.ClientID %>').focus();
                                    return "false-";
                                }
                                WhichInst_Verification = 'ISSUINGBANK';
                            }
                            else {
                                WhichInst_Verification = 'CLIENTBANK';
                            }
                            if (InstTypeV == "D") {
                                var IssueBankTextValue = document.getElementById('<%=txtIssuingBank.ClientID %>').value;
                                if (IssueBankTextValue == "" || IssueBankTextValue == "No Record Found") {
                                    alert('Please Select Issue Bank');
                                    document.getElementById('<%=txtIssuingBank.ClientID %>').value = '';
                                    document.getElementById('<%=txtIssuingBank.ClientID %>').focus();
                                    return "false-";
                                }
                                WhichInst_Verification = 'ISSUINGBANK';
                            }
                        }
                        else {
                            WhichInst_Verification = 'NonBranch';
                        }
                    }
                    var strRecieve = new String(ctxtRecieve.GetText());
                    if (strRecieve == "0.00") {
                        alert('Recieve Amount Can not be Zero');
                        ctxtRecieve.Focus();
                        return "false-";
                    }
                }
                return "True-" + WhichInst_Verification
            }
            else {
                if (InstTypeV != "CH")//Contra Change
                {
                    if (ctxtInstNo.GetText() == "") {
                        alert('Please Enter Instrument Number');
                        ctxtInstNo.Focus();
                        return "false-";
                    }
                    if (cInstDate.GetDate() == "01/01/0100") {
                        alert('Please Enter Some Date');
                        cInstDate.Focus();
                        return "false-";
                    }
                }
                var WithBankTextValue = document.getElementById('<%=txtWithFrom.ClientID %>').value;
                var WithBank_CBType = document.getElementById('<%=txtWithFrom_hidden.ClientID%>').value.split('~')[7];
                var DepositBankTextValue = document.getElementById('<%=txtDepositInto.ClientID %>').value;
                if (WithBankTextValue == "" || WithBankTextValue == "No Record Found") {
                    alert('Please Select WithDraw Bank');
                    document.getElementById('<%=txtWithFrom.ClientID %>').value = '';
                    document.getElementById('<%=txtWithFrom.ClientID %>').focus();
                    return "false-";
                }
                else {
                    //Contra Change
                    if (InstTypeV == "CH") {
                        var CashBankType = (WithBankTextValue.split('~')[1] != undefined) ? WithBank_CBType : (InstTypeV == "CH") ? "Cash" : "Bank";
                        if (CashBankType.toUpperCase().trim() != "CASH") {
                            alert('Please Select Cash Account in WithDrawFrom Field When InstrumentType is CASH!');
                            document.getElementById('<%=txtWithFrom.ClientID %>').value = '';
                            document.getElementById('<%=txtWithFrom.ClientID %>').focus();
                            return "false-";
                        }
                    }
                }
                if (DepositBankTextValue == "" || DepositBankTextValue == "No Record Found") {
                    alert('Please Select DepositInto Bank');
                    document.getElementById('<%=txtDepositInto.ClientID %>').value = '';
                    document.getElementById('<%=txtDepositInto.ClientID %>').focus();
                    return "false-";
                }
                var strAmount = new String(ctxtAmount.GetText());
                if (strAmount == "0.00") {
                    alert('Amount Can not be Zero');
                    strAmount.Focus();
                    return "false-";
                }
                WhichInst_Verification = 'WITHDRAW';
                return "True-" + WhichInst_Verification
            }

        }
        function SaveButtonClick() {
            var txtBankAccounts_hiddenValue = document.getElementById('txtBankAccounts_hidden').value;
            var txtBankAccounts_Value = document.getElementById('txtBankAccounts').value;
            var Mode = document.getElementById("hdn_Mode").value;
            var VoucherType;
            if (Mode == "Entry") {
                VoucherType = cComboType.GetValue();
            }
            else {
                VoucherType = cSCmb_Type.GetValue();
            }
            if (VoucherType != "C") {
                if (txtBankAccounts_hiddenValue == "" || txtBankAccounts_Value == "") {
                    alert('Please Choose Cash Bank Account First');
                    document.getElementById("txtBankAccounts").focus();
                    return;
                }
            }
            if (cGvAddRecordDisplay.GetVisibleRowsOnPage() != '0') {
                var VoucherType = cComboType.GetValue();
                cGvAddRecordDisplay.PerformCallback('Save~~~~');
            }
            else {
                alert('Please Add Atleast Single Record First');
                document.getElementById('txtMainAccount').focus();
            }

        }
        function ResetPageOnSave() {
            document.getElementById('txtBankAccounts').value = '';
            document.getElementById('txtBankAccounts_hidden').value = '';
            document.getElementById('txtNarration').value = '';
            document.getElementById("txtSubAccount").value = '';
            document.getElementById("txtSubAccount_hidden").value = '';
            document.getElementById("txtMainAccount").value = '';
            document.getElementById("txtMainAccount_hidden").value = '';
            //document.getElementById("hdnDefaultBranch").value='';
            document.getElementById("hdnType").value = '';
            document.getElementById("hdnAccountType").value = '';
            document.getElementById("hdn_Brch_NonBrch").value = '';
            document.getElementById("hdn_SubLedgerType").value = '';
            document.getElementById("hdn_MainAcc_Type").value = '';
            document.getElementById('<%=B_ImgSymbolBankBal.ClientID %>').innerHTML = '';
            document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = '';
            document.getElementById('<%=B_ImgSymbolAcBal.ClientID %>').innerHTML = '';
            document.getElementById('<%=B_AcBalance.ClientID %>').innerHTML = '';
            IsSubAccountChange = "False";
            Param_SubAccountID = ''
            SubLedgerType = '';
            MainAcCode = '';
            document.getElementById("hdn_CashBankType_InstTypeE").value = "";
        }
        function CbpBankBalance_EndCallBack() {
            var strUndefined = new String(cCbpBankBalance.cpBankBalance);
            if (strUndefined != "undefined") {
                document.getElementById('<%=B_ImgSymbolBankBal.ClientID %>').innerHTML = ActiveCurrencySymbol;
                document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = strUndefined.split('~')[0];
                document.getElementById('<%=B_BankBalance.ClientID %>').style.color = strUndefined.split('~')[1];
            }
        }
        function CbpAcBalance_EndCallBack() {
            var strUndefined = new String(cCbpAcBalance.cpAcBalance);
            if (strUndefined != "undefined") {
                document.getElementById('<%=B_ImgSymbolAcBal.ClientID %>').innerHTML = ActiveCurrencySymbol;
                document.getElementById('<%=B_AcBalance.ClientID %>').innerHTML = strUndefined.split('~')[0];
                document.getElementById('<%=B_AcBalance.ClientID %>').style.color = strUndefined.split('~')[1];
            }
        }
        function CbpDepstBankBalance_EndCallBack() {
            var strUndefined = new String(cCbpDepstBankBalance.cpDepstBalance);
            if (strUndefined != "undefined") {
                document.getElementById('B_ImgSymbolDepstBankBal').innerHTML = ActiveCurrencySymbol;
                document.getElementById('b_DepstBalance').innerHTML = strUndefined.split('~')[0];
                document.getElementById('b_DepstBalance').style.color = strUndefined.split('~')[1];
            }
        }
        function CbpWithBankBalance_EndCallBack() {
            var strUndefined = new String(cCbpWithBankBalance.cpWithBalance);
            if (strUndefined != "undefined") {
                document.getElementById('B_ImgSymbolWithBankBal').innerHTML = ActiveCurrencySymbol;
                document.getElementById('b_WithBalance').innerHTML = strUndefined.split('~')[0];
                document.getElementById('b_WithBalance').style.color = strUndefined.split('~')[1];
            }
        }
        function txtMainAccountE_OnChange(obj, objClientID) {

        }
        function OnComboModeSelectedIndexChanged() {
            ResetPageOnSave();// Set The Page In Initial MOde...So It is Forcly Used;
            var SelectedValue = cComboMode.GetValue();
            if (SelectedValue == "Edit") {
                document.getElementById("DivEntry").style.display = "none";
                document.getElementById("DivEdit").style.display = "inline";
                cPopup_Search.Show();
                cPopUp_StartPage.Hide();
                document.getElementById("hdn_Mode").value = "Edit";
                cSCmbBranch.PerformCallback();
                cSDtTDate.SetDate(new Date());
                cSDtInstdate.SetDate(new Date("01/01/0100"));
            }
        }
        function SBtnBack_Click() {
            SetAllClearOnSwitchingSearch();
            document.getElementById("DivEntry").style.display = "inline";
            document.getElementById("DivEdit").style.display = "none";
            cPopup_Search.Hide();
            cPopUp_StartPage.Show();
            cComboMode.SetSelectedIndex(0);
            document.getElementById("hdn_Mode").value = "Entry";
        }
        function SetAllDisplayNone_SearchItem() {
            document.getElementById("td_sPayeeAcValue").style.display = "none";
            document.getElementById("td_sAuthLetterValue").style.display = "none";
            document.getElementById("td_sPayeeAcLabel").style.display = "none";
            document.getElementById("td_sAuthLetterLabel").style.display = "none";
            document.getElementById("td_SIssueBankValue").style.display = "none";
            document.getElementById("td_SCustBankValue").style.display = "none";
            document.getElementById("td_SCustBankLabel").style.display = "none";
            document.getElementById("td_SIssueBankLabel").style.display = "none";
            document.getElementById("td_SWithDrawLabel").style.display = "none";
            document.getElementById("td_SDepstIntoLabel").style.display = "none";
            document.getElementById("td_SWithDrawValue").style.display = "none";
            document.getElementById("td_SDepstIntoValue").style.display = "none";
            //            document.getElementById("td_SRecieveValue").style.display="none";
            //            document.getElementById("td_SRecieveLable").style.display="none";
            //            document.getElementById("td_SPaymentValue").style.display="none";
            //            document.getElementById("td_SPaymentLable").style.display="none";
            document.getElementById("td_SCashBankValue").style.display = "none";
            document.getElementById("td_SCashBankLabel").style.display = "none";
            document.getElementById("td_SMainAcValue").style.display = "none";
            document.getElementById("td_SMainAcLabel").style.display = "none";
            document.getElementById("td_SSubAcValue").style.display = "none";
            document.getElementById("td_SSubAcLabel").style.display = "none";
        }
        function OnSCmb_TypeSelectedIndexChanged() {
            SetAllClearOnSwitchingSearch();
            var SelectedValue = cSCmb_Type.GetValue();
            if (SelectedValue == "P") {
                SetAllDisplayNone_SearchItem();
                document.getElementById("td_sPayeeAcValue").style.display = "inline";
                document.getElementById("td_sPayeeAcLabel").style.display = "inline";
                //                document.getElementById("td_SPaymentValue").style.display="inline";
                //                document.getElementById("td_SPaymentLable").style.display="inline";
                document.getElementById("td_SCashBankValue").style.display = "inline";
                document.getElementById("td_SCashBankLabel").style.display = "inline";
                document.getElementById("td_SMainAcValue").style.display = "inline";
                document.getElementById("td_SMainAcLabel").style.display = "inline";
                document.getElementById("td_SSubAcValue").style.display = "inline";
                document.getElementById("td_SSubAcLabel").style.display = "inline";
            }
            if (SelectedValue == "R") {
                SetAllDisplayNone_SearchItem();
                document.getElementById("td_SCustBankValue").style.display = "inline";
                document.getElementById("td_SCustBankLabel").style.display = "inline";
                document.getElementById("td_SIssueBankLabel").style.display = "inline";
                document.getElementById("td_SIssueBankValue").style.display = "inline";
                document.getElementById("td_sAuthLetterValue").style.display = "inline";
                document.getElementById("td_sAuthLetterLabel").style.display = "inline";
                //                document.getElementById("td_SRecieveValue").style.display="inline";
                //                document.getElementById("td_SRecieveLable").style.display="inline";
                document.getElementById("td_SCashBankValue").style.display = "inline";
                document.getElementById("td_SCashBankLabel").style.display = "inline";
                document.getElementById("td_SMainAcValue").style.display = "inline";
                document.getElementById("td_SMainAcLabel").style.display = "inline";
                document.getElementById("td_SSubAcValue").style.display = "inline";
                document.getElementById("td_SSubAcLabel").style.display = "inline";
            }
            if (SelectedValue == "C") {
                SetAllDisplayNone_SearchItem();
                document.getElementById("td_SWithDrawLabel").style.display = "inline";
                document.getElementById("td_SDepstIntoLabel").style.display = "inline";
                document.getElementById("td_SWithDrawValue").style.display = "inline";
                document.getElementById("td_SDepstIntoValue").style.display = "inline";
                //                document.getElementById("td_SRecieveValue").style.display="inline";
                //                document.getElementById("td_SRecieveLable").style.display="inline";
                //                document.getElementById("td_SPaymentValue").style.display="inline";
                //                document.getElementById("td_SPaymentLable").style.display="inline";
            }
        }
        function SBtnSearch_Click() {
            for (a = 1; a < 11; a++) {
                var obj = "A" + a;
                document.getElementById(obj).innerText = a;
            }
            var SelectedValue = cSCmb_Type.GetValue();
            var BranchID = '', TransactionDate = '', VoucherNumber = '', CashBankAc = '', MainNarration = '',
            MainAc = '', SubAc = '', InstType = '', InstNo = '', Instdate = '', CustBank = '', IssueBank = '',
            WithFrom = '', DepstInto = '', AuthLetterRef = '', PayeeAc = '', Amount = '', Payment = '',
            Recieve = '', LineNarration = '', WhatValue;
            if (SelectedValue == "A") {
                BranchID = cSCmbBranch.GetValue()
                TransactionDate = cSDtTDate.GetDate();
                VoucherNumber = cStxtVoucherNumber.GetText();
                WhatValue = document.getElementById("<%=StxtCashBankAc.ClientID%>").value;
                 if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                     CashBankAc = document.getElementById("<%=StxtCashBankAc_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtCashBankAc_hidden.ClientID%>").value = '';

                MainNarration = document.getElementById("<%=StxtMNarration.ClientID%>").value;
                 WhatValue = document.getElementById("<%=StxtMainAccount.ClientID%>").value;
                 if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                     MainAc = document.getElementById("<%=StxtMainAccount_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtMainAccount_hidden.ClientID%>").value = '';
                WhatValue = document.getElementById("<%=StxtSubAccount.ClientID%>").value;
                 if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                     SubAc = document.getElementById("<%=StxtSubAccount_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtSubAccount_hidden.ClientID%>").value = '';
                InstType = cSCmbInstType.GetValue();
                InstNo = cStxtInstNo.GetText();
                Instdate = cSDtInstdate.GetDate();
                WhatValue = document.getElementById("<%=StxtPayeeAc.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    PayeeAc = document.getElementById("<%=StxtPayeeAc_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtPayeeAc_hidden.ClientID%>").value = '';
                WhatValue = document.getElementById("<%=StxtWithFrom.ClientID%>").value;
                 if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                     WithFrom = document.getElementById("<%=StxtWithFrom_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtWithFrom_hidden.ClientID%>").value = '';
                WhatValue = document.getElementById("<%=StxtDepstInto.ClientID%>").value;
                 if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                     DepstInto = document.getElementById("<%=StxtDepstInto_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtDepstInto_hidden.ClientID%>").value = '';
                Payment = cStxtPayment.GetText();
                Recieve = cStxtRecieve.GetText();
                LineNarration = document.getElementById("<%=StxtLineNarration.ClientID%>").value;
            }
            if (SelectedValue == "P") {
                BranchID = cSCmbBranch.GetValue()
                TransactionDate = cSDtTDate.GetDate();
                VoucherNumber = cStxtVoucherNumber.GetText();
                WhatValue = document.getElementById("<%=StxtCashBankAc.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    CashBankAc = document.getElementById("<%=StxtCashBankAc_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtCashBankAc_hidden.ClientID%>").value = '';

                MainNarration = document.getElementById("<%=StxtMNarration.ClientID%>").value;
                WhatValue = document.getElementById("<%=StxtMainAccount.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    MainAc = document.getElementById("<%=StxtMainAccount_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtMainAccount_hidden.ClientID%>").value = '';
                WhatValue = document.getElementById("<%=StxtSubAccount.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    SubAc = document.getElementById("<%=StxtSubAccount_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtSubAccount_hidden.ClientID%>").value = '';
                InstType = cSCmbInstType.GetValue();
                InstNo = cStxtInstNo.GetText();
                Instdate = cSDtInstdate.GetDate();
                WhatValue = document.getElementById("<%=StxtPayeeAc.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    PayeeAc = document.getElementById("<%=StxtPayeeAc_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtPayeeAc_hidden.ClientID%>").value = '';
                Payment = cStxtPayment.GetText();
                AuthLetterRef = document.getElementById("<%=StxtAuthLetterRef.ClientID%>").value;
                Recieve = cStxtRecieve.GetText();
                LineNarration = document.getElementById("<%=StxtLineNarration.ClientID%>").value;
            }
            if (SelectedValue == "R") {
                BranchID = cSCmbBranch.GetValue()
                TransactionDate = cSDtTDate.GetDate();
                VoucherNumber = cStxtVoucherNumber.GetText();
                WhatValue = document.getElementById("<%=StxtCashBankAc.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    CashBankAc = document.getElementById("<%=StxtCashBankAc_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtCashBankAc_hidden.ClientID%>").value = '';
                MainNarration = document.getElementById("<%=StxtMNarration.ClientID%>").value;
                WhatValue = document.getElementById("<%=StxtMainAccount.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    MainAc = document.getElementById("<%=StxtMainAccount_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtMainAccount_hidden.ClientID%>").value = '';
                WhatValue = document.getElementById("<%=StxtSubAccount.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    SubAc = document.getElementById("<%=StxtSubAccount_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtSubAccount_hidden.ClientID%>").value = '';
                InstType = cSCmbInstType.GetValue();
                InstNo = cStxtInstNo.GetText();
                Instdate = cSDtInstdate.GetDate();
                WhatValue = document.getElementById("<%=StxtCustBank.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    CustBank = document.getElementById("<%=StxtCustBank_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtCustBank_hidden.ClientID%>").value = '';
                WhatValue = document.getElementById("<%=StxtIssueBank.ClientID%>").value
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    IssueBank = document.getElementById("<%=StxtIssueBank_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtIssueBank_hidden.ClientID%>").value = '';
                AuthLetterRef = document.getElementById("<%=StxtAuthLetterRef.ClientID%>").value;
                Recieve = cStxtRecieve.GetText();
                LineNarration = document.getElementById("<%=StxtLineNarration.ClientID%>").value;
            }
            if (SelectedValue == "C") {
                BranchID = cSCmbBranch.GetValue()
                TransactionDate = cSDtTDate.GetDate();
                VoucherNumber = cStxtVoucherNumber.GetText();
                WhatValue = document.getElementById("<%=StxtCashBankAc.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    CashBankAc = document.getElementById("<%=StxtCashBankAc_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtCashBankAc_hidden.ClientID%>").value = '';

                MainNarration = document.getElementById("<%=StxtMNarration.ClientID%>").value;
                InstType = cSCmbInstType.GetValue();
                InstNo = cStxtInstNo.GetText();
                Instdate = cSDtInstdate.GetDate();
                WhatValue = document.getElementById("<%=StxtWithFrom.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    WithFrom = document.getElementById("<%=StxtWithFrom_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtWithFrom_hidden.ClientID%>").value = '';
                WhatValue = document.getElementById("<%=StxtDepstInto.ClientID%>").value;
                if (WhatValue.trim() != '' && WhatValue != 'No Record Found')
                    DepstInto = document.getElementById("<%=StxtDepstInto_hidden.ClientID%>").value;
                else
                    document.getElementById("<%=StxtDepstInto_hidden.ClientID%>").value = '';
                Recieve = cStxtRecieve.GetText();
                Payment = cStxtPayment.GetText();
                LineNarration = document.getElementById("<%=StxtLineNarration.ClientID%>").value;

            }
             //             alert(" BranchID :" +BranchID+
             //             "\n TransactionDate :" +TransactionDate+
             //             "\n VoucherNumber :"+VoucherNumber+
             //             "\n CashBankAc :"+CashBankAc+
             //             "\n MainNarration :"+MainNarration+
             //             "\n MainAc :"+MainAc+
             //             "\n SubAc :"+SubAc+
             //             "\n InstType :"+InstType+
             //             "\n InstNo :"+InstNo+
             //             "\n Instdate :"+Instdate+
             //             "\n CustBank :"+CustBank+
             //             "\n IssueBank :"+IssueBank+
             //             "\n WithFrom :"+WithFrom+
             //             "\n DepstInto :"+DepstInto+
             //             "\n AuthLetterRef :"+AuthLetterRef+
             //             "\n PayeeAc :"+PayeeAc+
             //             "\n Payment :"+Payment+
             //             "\n Recieve :"+Recieve+
             //             "\n LineNarration :"+LineNarration);
            var SearchParam = BranchID + '^' + TransactionDate + '^' + VoucherNumber + '^' + CashBankAc + '^' + MainNarration + '^' +
            MainAc + '^' + SubAc + '^' + InstType + '^' + InstNo + '^' + Instdate + '^' + CustBank + '^' + IssueBank + '^' + WithFrom + '^' +
            DepstInto + '^' + AuthLetterRef + '^' + PayeeAc + '^' + Payment + '^' + Recieve + '^' + LineNarration;

        }
        function GvCBSearch_EndCallBack() {

            if (cGvCBSearch.cpEntryEventFire != undefined) {
                var VoucherValue = cGvCBSearch.cpEntryEventFire.split('*')[0];
                var VoucherText = cGvCBSearch.cpEntryEventFire.split('*')[1];
                var BranchID = cGvCBSearch.cpEntryEventFire.split('*')[2];
                var BranchName = cGvCBSearch.cpEntryEventFire.split('*')[3];
                var CashBankID = cGvCBSearch.cpEntryEventFire.split('*')[4];
                var CashBankName = cGvCBSearch.cpEntryEventFire.split('*')[5];
                var TransactionDate = cGvCBSearch.cpEntryEventFire.split('*')[6];
                document.getElementById('hdn_OriginalTDate').value = TransactionDate;
                var Narration = cGvCBSearch.cpEntryEventFire.split('*')[7];
                var SegmentID_Name = cGvCBSearch.cpEntryEventFire.split('*')[8];
                document.getElementById("DivEdit").style.display = "none";
                document.getElementById("DivEntry").style.display = "inline";
                document.getElementById("tddiscard").style.display = "none";
                document.getElementById("tdExit").style.display = "inline";
                cSCmb_Type.SetValue(VoucherValue);
                ReSetUp_Page_OnEdit(VoucherValue, VoucherText, BranchID, BranchName, CashBankID, CashBankName, TransactionDate, Narration, SegmentID_Name);
            }

            if (cGvCBSearch.cpCBE_FileAlreadyUsedBy != undefined) {
                var obj = cGvCBSearch.cpCBE_FileAlreadyUsedBy;
                var WhichUser = (obj.split('~')[0]);
                if (WhichUser == "Other") {
                    alert('This File is Being Used By ' + obj.split('~')[1]);
                }
                else {
                    cFileUsedByPopUp.Show();
                }

            }

            if (cGvCBSearch.cpCBClose != undefined) {
                alert(cGvCBSearch.cpCBClose);
            }
            if (cGvCBSearch.cpClearHiddenField != undefined) {
                ClearHiddenField();
                if (cGvCBSearch.cpClearHiddenField.split('~')[1] == "SearchByLink") {
                    document.getElementById("DivEntry").style.display = "none";
                    document.getElementById("DivEdit").style.display = "inline";
                    cSCmbBranch.PerformCallback();
                    cSDtTDate.SetDate(new Date());
                    cSDtInstdate.SetDate(new Date("01/01/0100"));
                    cComboMode.SetSelectedIndex(1);
                    document.getElementById("hdn_Mode").value = "Edit";

                }
            }
            height();
        }
        function SbtnBackToSearch_Click() {
            document.getElementById("DivEntry").style.display = "none";
            document.getElementById("DivEdit").style.display = "inline";
            cPopup_Search.Show();
            cSCmb_Type.Focus();
        }
        function SetAllClearOnSwitchingSearch() {
            cSCmbBranch.SetSelectedIndex(0);
            cSDtTDate.SetDate(new Date());
            cSDtInstdate.SetDate(new Date("01/01/0100"));
            cStxtVoucherNumber.SetText('');
            document.getElementById("<%=StxtCashBankAc.ClientID%>").value = '';
            document.getElementById("<%=StxtCashBankAc_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtMNarration.ClientID%>").value = '';
            document.getElementById("<%=StxtMainAccount.ClientID%>").value = '';
            document.getElementById("<%=StxtMainAccount_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtSubAccount.ClientID%>").value = '';
            document.getElementById("<%=StxtSubAccount_hidden.ClientID%>").value = '';
            cSCmbInstType.SetSelectedIndex(0);
            document.getElementById("<%=StxtPayeeAc.ClientID%>").value = '';
            document.getElementById("<%=StxtPayeeAc_hidden.ClientID%>").value = '';
            cStxtPayment.SetText('');
            document.getElementById("<%=StxtLineNarration.ClientID%>").value = '';
            document.getElementById("<%=StxtWithFrom.ClientID%>").value = '';
            document.getElementById("<%=StxtWithFrom_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtDepstInto.ClientID%>").value = '';
            document.getElementById("<%=StxtDepstInto_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtCustBank.ClientID%>").value = '';
            document.getElementById("<%=StxtCustBank_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtIssueBank.ClientID%>").value = '';
            document.getElementById("<%=StxtIssueBank_hidden.ClientID%>").value = '';
            document.getElementById("<%=StxtAuthLetterRef.ClientID%>").value = '';
            document.getElementById("<%=StxtLineNarration.ClientID%>").value = '';
            cStxtRecieve.SetText('0000000000000.00');
            cStxtPayment.SetText('0000000000000.00');
            document.getElementById("hdn_CashBankType_InstTypeE").value = "";
        }
        function OnLeftNav_Click() {
            var i = document.getElementById("A1").innerText;
            if (parseInt(i) > 1) {
                i = parseInt(i) - 10;
                for (l = 1; l < 11; l++) {
                    var obj = "A" + l;
                    document.getElementById(obj).innerText = i++;
                }

            }
            else {
                alert('You are on the Beginning');
            }
        }
        function OnRightNav_Click() {
            var TestEnd = document.getElementById("A10").innerText;
            var TotalPage = document.getElementById("B_TotalPage").innerText;
            if (TestEnd == "" || TestEnd == TotalPage) {
                alert('You are at the End');
                return;
            }
            var i = document.getElementById("A1").innerText;
            if (parseInt(i) < TotalPage) {
                i = parseInt(i) + 10;
                var n = parseInt(TotalPage) - parseInt(i) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(i) + 2;
                for (r = 1; r < n; r++) {
                    var obj = "A" + r;
                    document.getElementById(obj).innerText = i++;
                }
                for (r = n; r < 11; r++) {
                    var obj = "A" + r;
                    document.getElementById(obj).innerText = "";
                }

            }
            else {
                alert('You are at the End');
            }
        }
        function OnPageNo_Click(obj) {
            var i = document.getElementById(obj).innerText;

        }
        function CmbPayee_SelectedIndexChanged() {
            //document.getElementById("hdn_PayeeIDOnUpdate").value=cCmbPayee.GetValue();
        }
        function CustomButtonClick(s, e) {
            if (e.buttonID == 'CustomBtnEdit') {
                VisibleIndexE = e.visibleIndex;
                s.GetRowValues(e.visibleIndex, 'ValueDate', OnGetRowValuesOnEdit);
            }
            if (e.buttonID == 'CustomBtnDelete') {
                VisibleIndexE = e.visibleIndex;
                s.GetRowValues(e.visibleIndex, 'ValueDate', OnGetRowValuesOnDelete);
            }
        }
        function OnGetRowValuesOnEdit(values) {

            var ValueDate = new Date(values);
            var monthnumber = ValueDate.getMonth();
            var monthday = ValueDate.getDate();
            var year = ValueDate.getYear();
            var ValueDateNumeric = new Date(year, monthnumber, monthday).getTime();
            //alert(ValueDate+'~'+ValueDateNumeric+'~'+VisibleIndexE);

            if (ValueDateNumeric == -2209008600000) {
                cMsgPopUp.Show();
                cbtnOk.Focus();
            }
            else {
                alert('Entry Already Been Tagged.Please Remove Tag To Edit Entry!!!.');
            }

        }

        function OnGetRowValuesOnDelete(values) {
            var ValueDate = new Date(values);
            var monthnumber = ValueDate.getMonth();
            var monthday = ValueDate.getDate();
            var year = ValueDate.getYear();
            var ValueDateNumeric = new Date(year, monthnumber, monthday).getTime();
            if (ValueDateNumeric == -2209008600000) {
                cDeleteMsgPopUp.Show();
            }
            else {
                alert('Entry Already Been Tagged.Please Remove Tag To Delete Entry!!!.');
            }
        }

        function btnOkClick() {
            cGvCBSearch.PerformCallback('PCB_BtnOkE~');
        }
        function DeletebtnOkClick() {
            cGvAddRecordDisplay.PerformCallback('PCB_DeleteBtnOkE~~~~');
        }
        function btnContinueClick() {
            cGvCBSearch.PerformCallback('PCB_ContinueWith~');
        }
        function btnFreshEntryClick() {
            cGvCBSearch.PerformCallback('PCB_FreshEntry~');
        }
        function btnCloseClick() {
            cGvCBSearch.PerformCallback('PCB_CloseEntry~');
        }
        function ReSetUp_Page_OnEdit(VoucherValue, VoucherText, BranchID, BranchName, CashBankID, CashBankName, TransactionDate, Narration, SegmentID_Name) {
            //alert(VoucherValue+'~'+VoucherText+'~'+BranchID+'~'+BranchName+'~'+CashBankID+'~'+CashBankName+'~'+TransactionDate);
            //alert(CashBankID+'~'+CashBankName);
            SetAllDisplayNone();
            document.getElementById('bTypeText').innerHTML = VoucherText;
            document.getElementById('bBranchText').innerHTML = BranchName;
            document.getElementById('hdnType').value = VoucherValue;
            document.getElementById('hdnDefaultBranch').value = BranchID;
            document.getElementById('txtBankAccounts').value = CashBankName;
            document.getElementById('txtBankAccounts_hidden').value = CashBankID;
            document.getElementById('txtNarration').value = Narration;
            document.getElementById("hdn_EditVoucher_SegmentID_Name").value = SegmentID_Name;
            cdtTDate.SetDate(new Date(TransactionDate));
            if (VoucherValue == "C") {
                document.getElementById("tdMainAccountValue").style.display = 'none';
                document.getElementById("tdSubAccountValue").style.display = 'none';
                document.getElementById("tdMainAccountLabel").style.display = 'none';
                document.getElementById("tdSubAccountLabel").style.display = 'none';
                document.getElementById("tdCashBankLabel").style.display = 'none';
                document.getElementById("tdCashBankValue").style.display = 'none';
                document.getElementById("tdCBankBalLable").style.display = 'none';
                document.getElementById("tdCBankBalValue").style.display = 'none';
                document.getElementById("tdAcBalanceLable").style.display = 'none';
                document.getElementById("tdAcBalanceValue").style.display = 'none';
                document.getElementById("TblChangable").style.width = "40%";
                document.getElementById("TblAccountDetail").style.width = "20%";
                document.getElementById("TblChangable").style.styleFloat = 'left';
                document.getElementById("TblAccountDetail").style.styleFloat = 'left';
            }
            else {
                document.getElementById("tdMainAccountValue").style.display = 'inline';
                document.getElementById("tdSubAccountValue").style.display = 'inline';
                document.getElementById("tdMainAccountLabel").style.display = 'inline';
                document.getElementById("tdSubAccountLabel").style.display = 'inline';
                document.getElementById("tdCashBankLabel").style.display = 'inline';
                document.getElementById("tdCashBankValue").style.display = 'inline';
                document.getElementById("tdCBankBalLable").style.display = 'inline';
                document.getElementById("tdCBankBalValue").style.display = 'inline';
                document.getElementById("tdAcBalanceLable").style.display = 'inline';
                document.getElementById("tdAcBalanceValue").style.display = 'inline';
                document.getElementById("TblChangable").style.width = "98%";
                document.getElementById("TblAccountDetail").style.width = "98%";
                document.getElementById("TblChangable").style.styleFloat = 'none';
                document.getElementById("TblAccountDetail").style.styleFloat = 'none';
                cdtTDate.Focus();
            }
            cGvAddRecordDisplay.PerformCallback('ShowHideColumn~~' + VoucherValue);
        }
        function ClearPageSession() {
            cGvAddRecordDisplay.PerformCallback('ClearSession~Start~');
        }
        function BtnBack_Click() {
            cGvAddRecordDisplay.PerformCallback('Exit~Save~~~');
        }
        function Search_linkClick() {
            cGvAddRecordDisplay.PerformCallback('ClearSession~Search~');
            document.getElementById("DivEntry").style.display = "none";
            document.getElementById("DivEdit").style.display = "inline";
            cSCmbBranch.PerformCallback();
            cSDtTDate.SetDate(new Date());
            cSDtInstdate.SetDate(new Date("01/01/0100"));
            cComboMode.SetSelectedIndex(1);
            cPopup_Search.Show();
            cPopUp_StartPage.Hide();
        }
        function BackSearchResult_linkClick() {
            cGvCBSearch.PerformCallback("ClearSession~SearchByLink~");
        }
        function AddRemove_InstTypeItem(CashBankType, SelectedInstType, VoucherType) {
            if (SelectedInstType == "") cComboInstType.PerformCallback("SetItems~" + CashBankType);
            else cComboInstType.PerformCallback("SetItemsAndSelectValue~" + CashBankType + '~' + SelectedInstType + '~' + VoucherType);
        }
        function SetInstDate() {
            if (ctxtInstNo.GetText().trim() == "") {
                cInstDate.SetDate(new Date("01/01/0100"));
            }
        }
        function ExitButtonClick() {
            cGvAddRecordDisplay.PerformCallback('Exit~WithOutSave~~~');
        }
        function BtnDeleteVoucher_Click() {
            cDeleteMsgPopUp.Show();
        }


    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table border="1" width="98%">
            <tr>
                <td class="EHEADER" align="center" colspan="6" style="width: 777px">
                    <strong><span id="SpanHeader" style="color: #000099">CASH BANK</span></strong>
                </td>
                <td colspan="2" class="EHEADER" style="text-align: right"></td>

            </tr>
        </table>
        <div id="DivEntry" style="display: none">
            <div id="divChangable" runat="server">

                <table border="1" width="98%">

                    <tr>
                        <td style="height: 11px; background-color: #b7ceec; vertical-align: top; text-align: left;">Type
                        </td>
                        <td style="width: 207px; height: 20px;">
                            <b id="bTypeText" runat="server" style="width: 100px; font-size: 12px"></b>
                        </td>
                        <td style="height: 11px; background-color: #b7ceec; vertical-align: top; text-align: left;">Branch</td>
                        <td style="width: 600px; height: 20px;">
                            <b id="bBranchText" runat="server" style="width: 200px; font-size: 12px"></b>
                        </td>
                        <td style="height: 11px; background-color: #b7ceec; vertical-align: top; text-align: left; width: 124px;">Trans.Date</td>
                        <td style="width: 579px; height: 20px">
                            <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom"
                                Font-Size="12px" UseMaskBehavior="True" Width="100px" EditFormatString="dd-MM-yyyy">
                                <ButtonStyle Width="13px"></ButtonStyle>
                                <ClientSideEvents DateChanged="function(s,e){TDateChange(); }"></ClientSideEvents>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td style="width: 363px; height: 20px; text-align: right;">
                            <dxe:ASPxCallbackPanel ID="CbpChoosenCurrency" runat="server" ClientInstanceName="cCbpChoosenCurrency" BackColor="White" OnCallback="CbpChoosenCurrency_Callback">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">
                                        <b title="Switch To Active Currency" id="B_ChoosenCurrency" runat="server" style="text-decoration: underline; width: 100%; font-style: italic; color: Blue; float: left;"></b>
                                    </dxe:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="function(s, e) {
	                                                    CbpChoosenCurrency_EndCallBack(); }" />
                            </dxe:ASPxCallbackPanel>
                            <b id="bSegmentName" runat="server" style="text-decoration: underline; width: 100%; color: Blue;"></b>
                        </td>
                    </tr>
                </table>
                <table border="1" id="TblChangable" width="98%">
                    <tr>
                        <td id="tdCashBankLabel" style="background-color: #b7ceec; vertical-align: top; text-align: left; width: 40%; height: 23px;">Cash/Bank Accounts</td>
                        <td style="background-color: #b7ceec; vertical-align: top; text-align: left; width: 30%; height: 23px;">Main Narration</td>
                        <td id="tdCBankBalLable" style="background-color: #b7ceec; vertical-align: top; text-align: left; width: 15%; height: 23px;">Current Bank Balance</td>
                        <td id="tdAcBalanceLable" style="background-color: #b7ceec; vertical-align: top; text-align: left; width: 13%; height: 23px;">Account Balance</td>
                    </tr>
                    <tr>
                        <td id="tdCashBankValue" style="width: 40%">
                            <asp:TextBox ID="txtBankAccounts" runat="server" Font-Size="13px" onfocus="this.select()"
                                onkeyup="CallBankAccount(this,'GenericAjaxList',event)" Width="98%"></asp:TextBox>
                        </td>
                        <td style="width: 30%">
                            <asp:TextBox ID="txtNarration" runat="server" Font-Size="11px" MaxLength="500" onkeydown="checkTextAreaMaxLength(this,event,'500');"
                                onkeyup="OnlyNarration(this,'Narration',event)" onfocus="this.select()" TextMode="MultiLine"
                                Width="97%" OnTextChanged="txtNarration_TextChanged"></asp:TextBox></td>
                        <td id="tdCBankBalValue" style="width: 15%">
                            <dxe:ASPxCallbackPanel ID="CbpBankBalance" runat="server" ClientInstanceName="cCbpBankBalance" OnCallback="CbpBankBalance_Callback" BackColor="White">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">
                                        <div style="width: 100%; text-align: right;">
                                            <b style="text-align: right" id="B_ImgSymbolBankBal" runat="server"></b>
                                            <b style="text-align: right" id="B_BankBalance" runat="server"></b>
                                        </div>
                                    </dxe:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="function(s, e) {
	                                                    CbpBankBalance_EndCallBack(); }" />
                            </dxe:ASPxCallbackPanel>
                        </td>
                        <td id="tdAcBalanceValue" style="width: 13%">
                            <dxe:ASPxCallbackPanel ID="CbpAcBalance" runat="server" ClientInstanceName="cCbpAcBalance" OnCallback="CbpAcBalance_Callback" BackColor="White">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">
                                        <div style="width: 100%; text-align: right;">
                                            <b style="text-align: right" id="B_ImgSymbolAcBal" runat="server"></b>
                                            <b style="text-align: right" id="B_AcBalance" runat="server"></b>
                                        </div>
                                    </dxe:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="function(s, e) {
	                                                    CbpAcBalance_EndCallBack(); }" />
                            </dxe:ASPxCallbackPanel>
                        </td>
                    </tr>
                </table>
            </div>
            <table id="TblAccountDetail" border="1" style="width: 98%; display: none">
                <tr>
                    <td style="width: 154px"></td>
                    <td></td>
                    <td style="font-weight: bold; font-size: 10px; width: 40%; display: none;" id="tdAcBal">A/C Balance : <b id="lblAcBalance" runat="server" style="width: 100px"></b>&nbsp;
                                <blink><b style="color:Blue;font-size:10px;" id="bDrCrStatus" runat="server"></b></blink>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td id="tdMainAccountLabel" style="background-color: #b7ceec; vertical-align: top; height: 11px; text-align: left;">Main Account</td>
                    <td id="tdSubAccountLabel" style="background-color: #b7ceec; vertical-align: top; height: 11px; text-align: left;">SubAccount</td>
                    <td style="background-color: #b7ceec; vertical-align: top; height: 11px; text-align: left;">Other Detail 
                    </td>
                </tr>
                <tr>
                    <td id="tdMainAccountValue" style="width: 154px">
                        <asp:TextBox ID="txtMainAccount" runat="server" Font-Size="13px" onfocus="this.select()"
                            onkeyup="CallMainAccount(this,'GenericAjaxList',event)" Width="287px"></asp:TextBox></td>
                    <td id="tdSubAccountValue">
                        <asp:TextBox ID="txtSubAccount" runat="server" Font-Size="13px"
                            onfocus="this.select()" onkeyup="CallSubAccount(this,'GenericAjaxListSP',event)"
                            Width="303px"></asp:TextBox></td>
                    <td>
                        <a id="aAddDetail" runat="server" href="javascript:void(0);" onclick="javascript:alert('You Can Not Add More Entry.');">Add Detail</a>
                    </td>
                </tr>
            </table>
            <table style="height: 44px; width: 98%;" border="1">
                <tr>
                    <td id="tddiscard" style="width: 244px">
                        <dxe:ASPxButton ID="btnDiscardEntry" runat="server" AccessKey="X" AutoPostBack="false"
                            TabIndex="0" Text="Discard Entered Records[x]" Width="188px" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {DiscardButtonClick();}"></ClientSideEvents>
                        </dxe:ASPxButton>
                    </td>
                    <td id="tdExit" style="width: 244px; display: none">
                        <dxe:ASPxButton ID="btnExit" runat="server" AccessKey="X" AutoPostBack="false"
                            TabIndex="0" Text="E[x]it/Close" Width="200px" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {ExitButtonClick();}"></ClientSideEvents>
                        </dxe:ASPxButton>
                    </td>
                    <td id="tdSaveButton" runat="Server">
                        <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server"
                            AccessKey="S" AutoPostBack="false" TabIndex="0" Text="[S]ave Entered Records"
                            Width="188px" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                        </dxe:ASPxButton>
                    </td>
                    <td>
                        <dxe:ASPxButton ID="btnBack" runat="server" AccessKey="B" AutoPostBack="false"
                            TabIndex="0" Text="[B]ack/WithoutClose" Width="188px" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {BtnBack_Click();}"></ClientSideEvents>
                        </dxe:ASPxButton>
                    </td>
                    <td>
                        <dxe:ASPxButton ID="BtnDeleteVoucher" runat="server" AccessKey="B" AutoPostBack="false"
                            TabIndex="0" Text="[D]elete Voucher" Width="188px" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {BtnDeleteVoucher_Click();}" />
                        </dxe:ASPxButton>
                    </td>
                    <td class="EHEADER" style="text-align: right">
                        <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Gray"
                            ClientInstanceName="exp" Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                            SelectedIndex="0" ValueType="System.Int32" Width="130px">
                            <Items>
                                <dxe:ListEditItem Value="0" Text="Select"></dxe:ListEditItem>
                                <dxe:ListEditItem Value="1" Text="PDF"></dxe:ListEditItem>
                                <dxe:ListEditItem Value="2" Text="XLS"></dxe:ListEditItem>
                                <dxe:ListEditItem Value="3" Text="RTF"></dxe:ListEditItem>
                                <dxe:ListEditItem Value="4" Text="CSV"></dxe:ListEditItem>
                            </Items>
                            <%--<ClientSideEvents SelectedIndexChanged="OncmbExportSelectedIndexChanged"></ClientSideEvents>--%>
                            <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                            </ButtonStyle>
                            <ItemStyle BackColor="Navy" ForeColor="White">
                                <HoverStyle BackColor="#8080FF" ForeColor="White">
                                </HoverStyle>
                            </ItemStyle>
                            <Border BorderColor="White"></Border>
                            <DropDownButton Text="Export">
                            </DropDownButton>
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
            </table>
            <dxe:ASPxGridView ID="GvAddRecordDisplay" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="cGvAddRecordDisplay" Font-Size="12px" KeyFieldName="RecordID"
                Width="98%" OnCustomCallback="GvAddRecordDisplay_CustomCallback" OnRowDeleting="GvAddRecordDisplay_RowDeleting"
                OnHtmlEditFormCreated="GvAddRecordDisplay_HtmlEditFormCreated" OnCancelRowEditing="GvAddRecordDisplay_CancelRowEditing" OnCustomUnboundColumnData="GvAddRecordDisplay_CustomUnboundColumnData" OnCommandButtonInitialize="GvAddRecordDisplay_CommandButtonInitialize">
                <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                <Settings ShowTitlePanel="True" VerticalScrollableHeight="300" />
                <SettingsPager Mode="ShowAllRecords" NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <ClientSideEvents EndCallback="function(s, e) {GvAddRecordDisplay_EndCallback();}" />
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Main Account" FieldName="MainAccountName"
                        VisibleIndex="0" Width="21%">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="WithDraw From" FieldName="WithFromName" VisibleIndex="1"
                        Width="21%">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Sub Account" FieldName="SubAccountName" VisibleIndex="2"
                        Width="34%">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Deposit Into" FieldName="DepositIntoName"
                        VisibleIndex="3" Width="34%">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Inst.Type" FieldName="InstrumentTypeName"
                        VisibleIndex="4">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Inst.No" FieldName="InstrumentNumber" VisibleIndex="5">
                        <CellStyle CssClass="gridcellleft" HorizontalAlign="Right" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Inst.Date" FieldName="FormatedInstrumentDate" VisibleIndex="6">
                        <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Payment" FieldName="PaymentAmount" VisibleIndex="7">
                        <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Reciept" FieldName="ReceiptAmount" VisibleIndex="8">
                        <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount" VisibleIndex="9">
                        <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="ValueDate" VisibleIndex="10" Visible="False">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewCommandColumn VisibleIndex="10" ShowDeleteButton="True" ShowEditButton="True">
                        <CellStyle ForeColor="White">
                            <%-- <HoverStyle BackColor="#000040">
                                                    </HoverStyle>--%>
                        </CellStyle>
                    </dxe:GridViewCommandColumn>
                    <dxe:GridViewDataTextColumn FieldName="TotalRecPay" Caption="." VisibleIndex="11" UnboundType="Decimal" Width="0px">
                    </dxe:GridViewDataTextColumn>

                </Columns>
                <Templates>
                    <EditForm>
                        <table id="tblDetailE" border="1" width="98%">
                            <tr>
                                <td style="width: 154px"></td>
                                <td></td>
                                <td style="font-weight: bold; font-size: 10px; width: 40%; display: none;" id="td1">A/C Balance : <b id="B1" runat="server" style="width: 100px"></b>&nbsp;
                                <blink><b style="color:Blue;font-size:10px;" id="b2" runat="server"></b></blink>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td id="tdMainAccountLableE" style="color: #ffffff; background-color: gray; width: 154px;">Main Account</td>
                                <td id="tdSubAcountLableE" style="color: #ffffff; background-color: gray">SubAccount</td>
                                <td style="color: #ffffff; background-color: gray">Other Detail
                                </td>
                                <td style="color: #ffffff; background-color: gray">#</td>
                            </tr>
                            <tr>
                                <td id="tdMainAccountValueE" style="width: 154px">
                                    <asp:TextBox ID="txtMainAccountE" runat="server" Font-Size="13px" onfocus="this.select()"
                                        Text='<%# Bind("MainAccountName") %>' onkeyup="CallMainAccountE(this,'GenericAjaxList',event)"
                                        onblur="txtMainAccountE_OnChange(this,'<%= txtMainAccountE.ClientId %>')" Width="287px"></asp:TextBox>
                                    <asp:HiddenField ID="txtMainAccountE_hidden" runat="server" />
                                </td>
                                <td id="tdSubAcountValueE">
                                    <asp:TextBox ID="txtSubAccountE" runat="server" Font-Size="13px" Text='<%# Bind("SubAccountName") %>'
                                        onfocus="this.select()" onkeyup="CallSubAccountE(this,'SubAccountModE',event)"
                                        Width="303px"></asp:TextBox>
                                    <asp:HiddenField ID="txtSubAccountE_hidden" runat="server" />
                                </td>
                                <td>
                                    <a id="a1" runat="server" href="javascript:void(0);" onclick="ShowInstTypePopUpE()">Update Detail</a>
                                </td>
                                <%--<td>
                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                    runat="server">
                                </dxe:ASPxGridViewTemplateReplacement>
                            </td>--%>
                                <td>
                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                        </table>
                    </EditForm>
                </Templates>
                <Settings ShowFooter="true" />
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="PaymentAmount" ShowInColumn="PaymentAmount" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="ReceiptAmount" ShowInColumn="ReceiptAmount" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Amount" ShowInColumn="Amount" SummaryType="Sum" />
                </TotalSummary>
                <SettingsEditing Mode="EditForm" />
                <Styles>
                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                    <FocusedGroupRow CssClass="gridselectrow">
                    </FocusedGroupRow>
                    <FocusedRow CssClass="gridselectrow" HorizontalAlign="Left" VerticalAlign="Top">
                    </FocusedRow>
                    <Footer CssClass="gridfooter">
                    </Footer>
                    <LoadingPanel ImageSpacing="10px">
                    </LoadingPanel>
                </Styles>
            </dxe:ASPxGridView>

            <dxe:ASPxPopupControl ID="PopUp_InstrumentDetail" runat="server" ClientInstanceName="cPopUp_InstrumentDetail"
                HeaderText="Other Details" ShowSizeGrip="False" Width="692px" CloseAction="None" Font-Bold="True" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides" Modal="True">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <dxe:ASPxCallbackPanel ID="InstrumentDetail_CallbackPanel" runat="server" Width="400px" ClientInstanceName="cInstrumentDetail_CallbackPanel" OnCallback="InstrumentDetail_CallbackPanel_Callback">
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <table style="width: 746px; font-weight: normal;" border="1" id="TABLE1">
                                        <tr>
                                            <td valign="top" id="tdITypeLable" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Instrument Type</td>
                                            <td style="width: 63px;" valign="top" id="tdITypeValue">
                                                <dxe:ASPxComboBox ID="ComboInstType" runat="server" ClientInstanceName="cComboInstType" Font-Size="12px"
                                                    SelectedIndex="0" ValueType="System.String" Width="103px" EnableIncrementalFiltering="True" OnCallback="ComboInstType_Callback">
                                                    <ClientSideEvents SelectedIndexChanged="OnComboInstTypeSelectedIndexChanged" EndCallback="ComboInstType_EndCallBack" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td valign="top" id="tdINoLable" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Instrument No</td>
                                            <td valign="top" id="tdINoValue">
                                                <dxe:ASPxTextBox runat="server" ID="txtInstNo" ClientInstanceName="ctxtInstNo" Width="150px">
                                                    <ClientSideEvents LostFocus="SetInstDate" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td id="tdIDateLable" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;"
                                                valign="top">Instrument Date</td>
                                            <td id="tdIDateValue" style="width: 5px;" valign="top">
                                                <dxe:ASPxDateEdit ID="InstDate" runat="server" EditFormat="Custom" ClientInstanceName="cInstDate"
                                                    UseMaskBehavior="True" Font-Size="12px" Width="141px" EditFormatString="dd-MM-yyyy">
                                                    <ClientSideEvents DateChanged="function(s,e){ }" />
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" id="tdCBankLable" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Customer Bank</td>
                                            <td colspan="5" valign="top" id="tdCBankValue">
                                                <dxe:ASPxComboBox ID="CmbClientBank" runat="server" CallbackPageSize="30" ClientInstanceName="CmbClientBankCI"
                                                    DropDownWidth="550px" EnableIncrementalFiltering="True"
                                                    Height="20px" OnCallback="CmbClientBank_OnCallback" TextFormatString="{0} [{2}]"
                                                    ValueField="cbd_id" ValueType="System.String" Width="370px">
                                                    <ClientSideEvents ValueChanged="function(s,e){
                                                                                    var indexr = s.GetSelectedIndex();
                                                                                    OnCmbClientBank_ValueChange(indexr)
                                                                                    }"
                                                        EndCallback="CmbClientBank_EndCallBack" />
                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="bnk_bankName" Caption="Bank Name" ToolTip="Bank Name" Width="150px"></dxe:ListBoxColumn>
                                                        <dxe:ListBoxColumn FieldName="cbd_accountName" Caption="Account Holder Name" ToolTip="Account Holder Name" Width="200px"></dxe:ListBoxColumn>
                                                        <dxe:ListBoxColumn FieldName="cbd_accountNumber" Caption="Account Number" ToolTip="Account Number" Width="120px"></dxe:ListBoxColumn>
                                                        <dxe:ListBoxColumn FieldName="bnk_micrno" Caption="MICR Number" ToolTip="MICR Number" Width="80px"></dxe:ListBoxColumn>
                                                        <dxe:ListBoxColumn FieldName="cbd_Accountcategory" Caption="Account Type" ToolTip="MICR Number" Width="80px"></dxe:ListBoxColumn>
                                                    </Columns>
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" id="tdIBankLable" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Issuing Bank</td>
                                            <td colspan="5" id="tdIBankValue" valign="top">
                                                <asp:TextBox ID="txtIssuingBank" runat="server" Font-Size="12px" Width="365px" onkeyup="CallListBank(this,'GenericAjaxList',event)"></asp:TextBox>
                                                <asp:HiddenField ID="txtIssuingBank_hidden" runat="server" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" id="tdAuthLable" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Auth. Letter Ref</td>
                                            <td colspan="5" valign="top" id="tdAuthValue">
                                                <asp:TextBox ID="txtAuthLetterRef" runat="server" Width="364px"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="6" valign="top" id="tdContraEntry" style="display: none">
                                                <table border="1" style="width: 809px">
                                                    <tr>
                                                        <td style="color: #ffffff; background-color: darkgray" valign="top"></td>
                                                        <td valign="top">
                                                            <dxe:ASPxCallbackPanel ID="CbpWithBankBalance" runat="server" ClientInstanceName="cCbpWithBankBalance"
                                                                OnCallback="CbpWithBankBalance_Callback" Width="301px">
                                                                <ClientSideEvents EndCallback="function(s, e) {
	                                                    CbpWithBankBalance_EndCallBack(); }" />
                                                                <PanelCollection>
                                                                    <dxe:PanelContent runat="server">
                                                                        <div style="width: 100%; text-align: right;">
                                                                            <b style="text-align: right" id="B_ImgSymbolWithBankBal"></b>
                                                                            <b id="b_WithBalance" style="text-align: right"></b>
                                                                        </div>
                                                                    </dxe:PanelContent>
                                                                </PanelCollection>
                                                            </dxe:ASPxCallbackPanel>
                                                        </td>
                                                        <td style="color: #ffffff; background-color: darkgray" valign="top"></td>
                                                        <td valign="top">
                                                            <dxe:ASPxCallbackPanel ID="CbpDepstBankBalance" runat="server" ClientInstanceName="cCbpDepstBankBalance" OnCallback="CbpDepstBankBalance_Callback" Width="300px">
                                                                <ClientSideEvents EndCallback="function(s, e) {
	                                                    CbpDepstBankBalance_EndCallBack(); }" />
                                                                <PanelCollection>
                                                                    <dxe:PanelContent runat="server">
                                                                        <div style="width: 100%; text-align: right;">
                                                                            <b style="text-align: right" id="B_ImgSymbolDepstBankBal"></b>
                                                                            <b id="b_DepstBalance" style="text-align: right"></b>

                                                                        </div>
                                                                    </dxe:PanelContent>
                                                                </PanelCollection>
                                                            </dxe:ASPxCallbackPanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;" valign="top">WithDraw From</td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtWithFrom" runat="server" Font-Size="12px" Width="300px" onkeyup="CallContraAccount(this,'GenericAjaxList',event,'From')"></asp:TextBox>
                                                            <asp:HiddenField ID="txtWithFrom_hidden" runat="server" />
                                                        </td>
                                                        <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;" valign="top">Deposit Into</td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtDepositInto" runat="server" Font-Size="12px" Width="300px" onkeyup="CallContraAccount(this,'GenericAjaxList',event,'To')"></asp:TextBox>
                                                            <asp:HiddenField ID="txtDepositInto_hidden" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;" valign="top">Amount</td>
                                                        <td valign="top">
                                                            <dxe:ASPxTextBox ID="txtAmount" runat="server" Width="140px" ClientInstanceName="ctxtAmount" HorizontalAlign="Right" Font-Size="13px">
                                                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                                <ValidationSettings ErrorDisplayMode="None">
                                                                </ValidationSettings>

                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td valign="top"></td>
                                                        <td valign="top"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td id="tdpayment" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left; display: none"
                                                valign="top">Payment</td>
                                            <td colspan="5" style="display: none;" valign="top" id="tdpaymentValue">
                                                <dxe:ASPxTextBox ID="txtPayment" runat="server" Width="140px" ClientInstanceName="ctxtPayment"
                                                    HorizontalAlign="Right" Font-Size="13px">
                                                    <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                    <ValidationSettings ErrorDisplayMode="None">
                                                    </ValidationSettings>
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdRecieve" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left; display: none"
                                                valign="top">Recieve</td>
                                            <td colspan="5" style="display: none;" valign="top" id="tdRecieveValue">
                                                <dxe:ASPxTextBox ID="txtRecieve" runat="server" Width="140px" ClientInstanceName="ctxtRecieve" HorizontalAlign="Right" Font-Size="13px">
                                                    <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                    <ValidationSettings ErrorDisplayMode="None">
                                                    </ValidationSettings>
                                                    <ClientSideEvents KeyUp="function(s,e){focusval(s.GetValue());}" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdPayeeLable" valign="top" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Payee</td>
                                            <td id="tdPayeeValue" colspan="5" valign="top">
                                                <dxe:ASPxCallbackPanel ID="CbpPayee" runat="server" ClientInstanceName="cCbpPayee" OnCallback="CbpPayee_Callback">
                                                    <%--  <clientsideevents endcallback="function(s, e) {
	                                                    CbpPayee_EndCallBack(); }" />--%>
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxComboBox runat="server" Width="103px" ID="CmbPayee" ValueType="System.String" ClientInstanceName="cCmbPayee" Font-Size="12px" __designer:wfdid="w187" OnCallback="CmbPayee_Callback">
                                                                <ClientSideEvents SelectedIndexChanged="CmbPayee_SelectedIndexChanged"></ClientSideEvents>
                                                            </dxe:ASPxComboBox>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                </dxe:ASPxCallbackPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Line Narration</td>
                                            <td colspan="5" valign="top">
                                                <asp:TextBox ID="txtNarration1" runat="server" Font-Names="Arial" Font-Size="12px"
                                                    Height="27px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" colspan="3"></td>
                                            <td colspan="3" style="text-align: right" valign="top">
                                                <table border="1">
                                                    <tr>
                                                        <td id="TdAdd" style="width: 100px">
                                                            <dxe:ASPxButton ID="btnAdd" runat="server" AutoPostBack="False" Text="Add" Width="85px">
                                                                <ClientSideEvents Click="function (s, e) { OnAddButtonClick(); }" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                        <td id="TdUpdate" style="width: 100px; display: none;">
                                                            <dxe:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="False" Text="Update" Width="85px">
                                                                <ClientSideEvents Click="function (s, e) { OnUpdateButtonClick(); }" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                        <td id="TdCancel" style="width: 100px">
                                                            <dxe:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" Text="Cancel" Width="85px">
                                                                <ClientSideEvents Click="function (s, e) {OnCancelButtonClick();}" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                        <td id="TdCancelE" style="width: 100px; display: none;">
                                                            <dxe:ASPxButton ID="btnCancelE" runat="server" AutoPostBack="False" Text="Cancel" Width="85px">
                                                                <ClientSideEvents Click="function (s, e) {OnCancelEButtonClick();}" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </dxe:PanelContent>
                            </PanelCollection>
                            <ClientSideEvents EndCallback="function(s, e) {
	                                                    InstrumentDetail_CallbackPanel_EndCallBack();
	                                                }" />
                        </dxe:ASPxCallbackPanel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>

            <dxe:ASPxPopupControl ID="PopUp_StartPage" runat="server" ClientInstanceName="cPopUp_StartPage"
                HeaderText="Select Application" ShowSizeGrip="False" Width="387px" CloseAction="None" Modal="True" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides">
                <ClientSideEvents PopUp="function(s, e)
                                                    {
                                                    setTimeout('cComboMode.Focus()', 50);
                                                    }" />
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <asp:Panel ID="Panel_Start" runat="server" DefaultButton="abtnOk">
                            <table style="height: 48px" border="1">
                                <tr>
                                    <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;" valign="top">Mode</td>
                                    <td style="width: 100px" valign="top">
                                        <dxe:ASPxComboBox ID="ComboMode" runat="server" ClientInstanceName="cComboMode" Font-Size="12px"
                                            SelectedIndex="0" ValueType="System.String" Width="65px" EnableIncrementalFiltering="True">
                                            <Items>
                                                <dxe:ListEditItem Value="Entry" Text="Entry"></dxe:ListEditItem>
                                                <dxe:ListEditItem Value="Edit" Text="Edit"></dxe:ListEditItem>
                                            </Items>
                                            <ClientSideEvents SelectedIndexChanged="OnComboModeSelectedIndexChanged" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td valign="top"></td>
                                    <td style="width: 100px" valign="top"></td>
                                </tr>
                                <tr>
                                    <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;" valign="top">Type</td>
                                    <td style="width: 100px" valign="top">
                                        <dxe:ASPxComboBox ID="ComboType" runat="server" ClientInstanceName="cComboType" Font-Size="12px"
                                            SelectedIndex="0" ValueType="System.String" Width="103px" EnableIncrementalFiltering="True">
                                            <Items>
                                                <dxe:ListEditItem Value="R" Text="Reciept"></dxe:ListEditItem>
                                                <dxe:ListEditItem Value="P" Text="Payment"></dxe:ListEditItem>
                                                <dxe:ListEditItem Value="C" Text="Contra"></dxe:ListEditItem>
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;" valign="top">Branch</td>
                                    <td style="width: 100px" valign="top">
                                        <dxe:ASPxComboBox ID="ComboBranch" runat="server" ClientInstanceName="cComboBranch"
                                            Font-Size="12px" ValueType="System.String" Width="303px" EnableIncrementalFiltering="True">
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" colspan="2"></td>
                                    <td style="width: 39px" valign="top"></td>
                                    <td style="width: 100px; text-align: right;" valign="top">
                                        <dxe:ASPxButton ID="abtnOk" runat="server" AutoPostBack="False" Text="Ok" Width="85px">
                                            <ClientSideEvents Click="function (s, e) {PopUp_StartPage_abtnOK_Click();cPopUp_StartPage.Hide(); }" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            <dxe:ASPxPopupControl ID="DeleteMsgPopUp" runat="server" ClientInstanceName="cDeleteMsgPopUp"
                HeaderText="Notice" Left="100" meta:resourcekey="DeleteMsgPopUpResource1" ShowSizeGrip="False"
                Top="100" Width="387px" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides">
                <ContentCollection>
                    <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <table class="style1">
                            <tr>
                                <td colspan="5">Are u Sure? Do You Want To Delete This Voucher?</td>
                            </tr>
                            <tr>
                                <td style="width: 3px"></td>
                                <td></td>
                                <td style="width: 164px"></td>
                                <td style="width: 3px">
                                    <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False"
                                        Text="Ok">
                                        <ClientSideEvents Click="function (s, e) { DeletebtnOkClick(); }" />
                                    </dxe:ASPxButton>
                                </td>
                                <td style="width: 3px">
                                    <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False"
                                        Text="Cancel">
                                        <ClientSideEvents Click="function (s, e) { cDeleteMsgPopUp.Hide(); }" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
        </div>
        <div id="DivEdit">
            <dxe:ASPxPopupControl ID="Popup_Search" runat="server" ClientInstanceName="cPopup_Search"
                HeaderText="Search" ShowSizeGrip="False" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides">
                <ClientSideEvents PopUp="function(s, e)
                                                    {
                                                    height();
                                                    }" />
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <asp:Panel ID="Panel_Search" runat="server" DefaultButton="SBtnSearch">
                            <table width="100%" border="1" id="TABLE2">
                                <tr>
                                    <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Type</td>
                                    <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Branch</td>
                                    <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Trans.Date</td>
                                </tr>
                                <tr>
                                    <td style="width: 255px">
                                        <dxe:ASPxComboBox ID="SCmb_Type" runat="server" ClientInstanceName="cSCmb_Type" Font-Size="12px"
                                            SelectedIndex="0" ValueType="System.String" Width="103px" EnableIncrementalFiltering="True">
                                            <Items>
                                                <dxe:ListEditItem Value="A" Text="All"></dxe:ListEditItem>
                                                <dxe:ListEditItem Text="Reciept" Value="R" />
                                                <dxe:ListEditItem Text="Payment" Value="P" />
                                                <dxe:ListEditItem Text="Contra" Value="C" />
                                            </Items>
                                            <ClientSideEvents SelectedIndexChanged="OnSCmb_TypeSelectedIndexChanged" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="width: 306px">
                                        <dxe:ASPxComboBox ID="SCmbBranch" runat="server" ClientInstanceName="cSCmbBranch"
                                            Font-Size="12px" ValueType="System.String" Width="303px" EnableIncrementalFiltering="True"
                                            OnCallback="SCmbBranch_Callback">
                                            <ClientSideEvents EndCallback="function(s, e) {
	                                                    cSCmb_Type.Focus(); cSCmb_Type.SetSelectedIndex(0);OnSCmb_TypeSelectedIndexChanged();}" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="SDtTDate" runat="server" ClientInstanceName="cSDtTDate" EditFormat="Custom"
                                            Font-Size="12px" UseMaskBehavior="True" Width="100px" EditFormatString="dd-MM-yyyy">
                                            <ClientSideEvents DateChanged="function(s,e){ }" />
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 11px; background-color: gainsboro; vertical-align: top; text-align: left;" valign="top">Voucher Number</td>
                                    <td>
                                        <dxe:ASPxTextBox runat="server" ID="StxtVoucherNumber" ClientInstanceName="cStxtVoucherNumber" Width="150px">
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td id="td_SCashBankLabel" style="height: 11px; background-color: gainsboro; vertical-align: top; text-align: left;">CashBank A/c</td>
                                    <td style="height: 11px; background-color: gainsboro; vertical-align: top; text-align: left;">Main Narration</td>
                                    <td style="height: 8px"></td>
                                </tr>
                                <tr>
                                    <td id="td_SCashBankValue" valign="top" style="width: 255px">
                                        <asp:TextBox ID="StxtCashBankAc" runat="server" Font-Size="13px" onfocus="this.select()"
                                            onkeyup="CallBankAccount(this,'GenericAjaxList',event)" Width="287px"></asp:TextBox>
                                        <asp:HiddenField ID="StxtCashBankAc_hidden" runat="server" />
                                    </td>
                                    <td colspan="2" valign="top">
                                        <asp:TextBox ID="StxtMNarration" runat="server" Font-Size="11px" MaxLength="500"
                                            onfocus="this.select()" onkeydown="checkTextAreaMaxLength(this,event,'500');"
                                            onkeyup="OnlyNarration(this,'Narration',event)" OnTextChanged="txtNarration_TextChanged"
                                            TextMode="MultiLine" Width="97%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="td_SMainAcLabel" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Main A/c</td>
                                    <td id="td_SSubAcLabel" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Sub A/c</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td id="td_SMainAcValue" style="width: 255px">
                                        <asp:TextBox ID="StxtMainAccount" runat="server" Font-Size="13px" onfocus="this.select()"
                                            onkeyup="CallMainAccount(this,'GenericAjaxList',event)" Width="287px"></asp:TextBox>
                                        <asp:HiddenField ID="StxtMainAccount_hidden" runat="server" />
                                    </td>
                                    <td id="td_SSubAcValue">
                                        <asp:TextBox ID="StxtSubAccount" runat="server" Font-Size="13px" onfocus="this.select()"
                                            onkeyup="CallSubAccount(this,'SubAccountMod_New',event)" Width="303px"></asp:TextBox>
                                        <asp:HiddenField ID="StxtSubAccount_hidden" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Inst.Type</td>
                                    <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Inst.No</td>
                                    <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Inst.Date</td>
                                </tr>
                                <tr>
                                    <td style="width: 255px">
                                        <dxe:ASPxComboBox ID="SCmbInstType" runat="server" ClientInstanceName="cSCmbInstType"
                                            Font-Size="12px" SelectedIndex="0" ValueType="System.String" Width="103px" EnableIncrementalFiltering="True"
                                            EnableSynchronization="False">
                                            <Items>
                                                <dxe:ListEditItem Text="Select" Value="S" />
                                                <dxe:ListEditItem Text="Cheque" Value="C" />
                                                <dxe:ListEditItem Text="Draft" Value="D" />
                                                <dxe:ListEditItem Text="E.Transfer" Value="E" />
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="width: 306px">
                                        <dxe:ASPxTextBox runat="server" ID="StxtInstNo" ClientInstanceName="cStxtInstNo" Width="150px">
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="SDtInstdate" runat="server" EditFormat="Custom" ClientInstanceName="cSDtInstdate"
                                            UseMaskBehavior="True" Font-Size="12px" Width="141px" EditFormatString="dd-MM-yyyy">
                                            <ClientSideEvents DateChanged="function(s,e){ }" />
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="td_SCustBankLabel" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Customer Bank</td>
                                    <td id="td_SIssueBankLabel" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">IssueBank</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td valign="top" id="td_SCustBankValue" style="width: 255px">
                                        <asp:TextBox ID="StxtCustBank" runat="server" Font-Size="13px" onfocus="this.select()"
                                            onkeyup="CallCustBankAccount(this,'GenericAjaxList',event)" Width="287px"></asp:TextBox>
                                        <asp:HiddenField ID="StxtCustBank_hidden" runat="server" />
                                    </td>
                                    <td valign="top" id="td_SIssueBankValue">
                                        <asp:TextBox ID="StxtIssueBank" runat="server" Font-Size="13px" onfocus="this.select()"
                                            onkeyup="CallListBank(this,'GenericAjaxList',event)" Width="287px"></asp:TextBox>
                                        <asp:HiddenField ID="StxtIssueBank_hidden" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td id="td_SWithDrawLabel" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left; display: none" valign="top">WithDraw From</td>
                                    <td id="td_SDepstIntoLabel" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left; display: none" valign="top">Deposit Into</td>
                                    <td valign="top"></td>
                                </tr>
                                <tr>
                                    <td id="td_SWithDrawValue" valign="top" style="display: none; width: 255px;">
                                        <asp:TextBox ID="StxtWithFrom" runat="server" Font-Size="12px" Width="300px" onkeyup="CallContraAccount(this,'CashBankContraAccount',event,'From')"></asp:TextBox>
                                        <asp:HiddenField ID="StxtWithFrom_hidden" runat="server" />
                                    </td>
                                    <td id="td_SDepstIntoValue" style="display: none;">
                                        <asp:TextBox ID="StxtDepstInto" runat="server" Font-Size="12px" Width="300px" onkeyup="CallContraAccount(this,'CashBankContraAccount',event,'To')"></asp:TextBox>
                                        <asp:HiddenField ID="StxtDepstInto_hidden" runat="server" />
                                    </td>
                                    <td valign="top"></td>
                                </tr>
                                <tr>
                                    <td id="td_sAuthLetterLabel" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">AuthLetterRef.</td>
                                    <td id="td_sPayeeAcLabel" style="font-weight: normal; display: none; color: #ffffff; background-color: darkgray;">Payee A/c</td>
                                    <td id="td_SPaymentLable" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;"
                                        valign="top">Payment</td>
                                    <td id="td_SRecieveLable" style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;"
                                        valign="top">Recieve</td>
                                </tr>
                                <tr>
                                    <td id="td_sAuthLetterValue" valign="top" style="width: 255px">
                                        <asp:TextBox ID="StxtAuthLetterRef" runat="server" Font-Size="13px" onfocus="this.select()"
                                            Width="287px"></asp:TextBox>
                                    </td>
                                    <td id="td_sPayeeAcValue" style="display: none;">
                                        <asp:TextBox ID="StxtPayeeAc" runat="server" Font-Size="13px" onfocus="this.select()"
                                            onkeyup="CallPayeeAccount(this,'GenericAjaxList',event)" Width="287px"></asp:TextBox>
                                        <asp:HiddenField ID="StxtPayeeAc_hidden" runat="server" />
                                    </td>
                                    <td id="td_SPaymentValue" valign="top">
                                        <dxe:ASPxTextBox ID="StxtPayment" runat="server" Width="140px" ClientInstanceName="cStxtPayment"
                                            HorizontalAlign="Right" Font-Size="13px">
                                            <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                            <ValidationSettings ErrorDisplayMode="None">
                                            </ValidationSettings>
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td id="td_SRecieveValue" valign="top">
                                        <dxe:ASPxTextBox ID="StxtRecieve" runat="server" Width="140px" ClientInstanceName="cStxtRecieve"
                                            HorizontalAlign="Right" Font-Size="13px">
                                            <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                            <ValidationSettings ErrorDisplayMode="None">
                                            </ValidationSettings>
                                        </dxe:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: gainsboro; vertical-align: top; height: 11px; text-align: left;">Line Narration</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="StxtLineNarration" runat="server" Font-Size="13px" onfocus="this.select()"
                                            Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 255px; text-align: right;"></td>
                                    <td colspan="2" style="vertical-align: bottom; text-align: right">

                                        <div style="float: left; margin-left: 220px">
                                            <dxe:ASPxButton ID="SBtnSearch" runat="server" AutoPostBack="False" Text="Search"
                                                Width="85px">
                                                <ClientSideEvents Click="function (s, e) {SBtnSearch_Click();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                        <div style="float: left; margin-left: 5px">
                                            <dxe:ASPxButton ID="SBtnBack" runat="server" AutoPostBack="False" Text="BackToModeSelection" Width="85px">
                                                <ClientSideEvents Click="function (s, e) {SBtnBack_Click();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            <b style="background: #b7ceec; font-weight: normal; border: 1">Searched By</b> : ( <b id="B_SearchBy"></b>)
           <table style="width: 60%; display: none;" border="1">
               <tr>
                   <td valign="top" style="vertical-align: top; width: 34px; height: 11px; background-color: #b7ceec; text-align: left">Page</td>
                   <td valign="top" style="width: 4px">
                       <b style="text-align: right" id="B_PageNo" runat="server"></b>
                   </td>
                   <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left;">Of
                   </td>
                   <td valign="top">
                       <b style="text-align: right" id="B_TotalPage" runat="server"></b>
                   </td>
                   <td valign="top">( <b style="text-align: right" id="B_TotalRows" runat="server"></b>&nbsp;items )
                   </td>
                   <td valign="top">
                       <table width="100%" style="display: none">
                           <tr>
                               <%-- <td valign="top">
                                <a id="A_StartNav" runat="server" href="javascript:void(0);" onclick="OnStartNav_Click()">
                                    <img src="/assests/images/LeftNav.gif" width="10"/>
                                  </a>
                               </td>--%>
                               <td valign="top">
                                   <a id="A_LeftNav" runat="server" href="javascript:void(0);" onclick="OnLeftNav_Click()">&nbsp;</a></td>
                               <td valign="top">
                                   <a id="A1" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A1')"></a>
                               </td>
                               <td valign="top">
                                   <a id="A2" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A2')"></a>
                               </td>
                               <td valign="top">
                                   <a id="A3" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A3')"></a>
                               </td>
                               <td valign="top">
                                   <a id="A4" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A4')"></a>
                               </td>
                               <td valign="top">
                                   <a id="A5" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A5')"></a>
                               </td>
                               <td valign="top">
                                   <a id="A6" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A6')"></a>
                               </td>
                               <td valign="top">
                                   <a id="A7" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A7')"></a>
                               </td>
                               <td valign="top">
                                   <a id="A8" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A8')"></a>
                               </td>
                               <td valign="top">
                                   <a id="A9" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A9')"></a>
                               </td>
                               <td valign="top">
                                   <a id="A10" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A10')"></a>
                               </td>
                               <td style="text-align: right; vertical-align: top; height: 11px; background-color: #b7ceec;" valign="top">
                                   <a id="A_RightNav" runat="server" href="javascript:void(0);" onclick="OnRightNav_Click()">&nbsp;</a></td>
                               <%--<td style="text-align: right" valign="top">
                               <a id="A_LastNav" runat="server" href="javascript:void(0);" onclick="OnLastNav_Click()">
                                    <img src="/assests/images/LeftNav.gif" width="10"/>
                                  </a>
                               </td>--%>
                           </tr>
                       </table>
                   </td>
                   <td style="text-align: right" valign="top">
                       <dxe:ASPxButton ID="SbtnBackToSearch" runat="server" AutoPostBack="False" Text=""
                           Width="32px">
                       </dxe:ASPxButton>
                   </td>
               </tr>
           </table>

            <dxe:ASPxGridView ID="GvCBSearch" runat="server" AutoGenerateColumns="False" ClientInstanceName="cGvCBSearch"
                KeyFieldName="CBID" Width="950px" Font-Size="12px" OnCustomCallback="GvCBSearch_CustomCallback">
                <ClientSideEvents CustomButtonClick="CustomButtonClick" EndCallback="function(s, e) {GvCBSearch_EndCallBack();}" />
                <Columns>
                    <dxe:GridViewCommandColumn VisibleIndex="0" Width="50px">
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" Text="Edit">
                            </dxe:GridViewCommandColumnCustomButton>
                            <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" Text="Delete">
                            </dxe:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxe:GridViewCommandColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="MainAccountID" Width="200px"
                        Caption="MainAc.">
                        <CellStyle Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="SubAccountID" Width="220px"
                        Caption="SubAc.">
                        <CellStyle Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="3" FieldName="CBID" Width="50px">
                        <CellStyle Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="CashBankID" Width="80px"
                        Caption="CashBank">
                        <CellStyle Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="TransactionDate" Width="80px"
                        Caption="TransDate">
                        <CellStyle Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="VoucherNumber" Width="50px"
                        Caption="VoucherNo.">
                        <CellStyle Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="ReceiptAmount" Width="75px"
                        Caption="ReceiptAmount">
                        <CellStyle Wrap="False" HorizontalAlign="Right">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="PaymentAmount" Width="75px"
                        Caption="PaymentAmount">
                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="InstrumentNumber" Width="75px"
                        Caption="InstNo.">
                        <CellStyle Wrap="False" HorizontalAlign="Right">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Narration" Width="400px"
                        Caption="Narration">
                        <CellStyle Wrap="False">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="11" FieldName="IBRef">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="12" FieldName="ExchangeSegmentID">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="13" FieldName="ValueDate">
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                <SettingsPager NumericButtonCount="20" ShowSeparators="True" Mode="ShowAllRecords" PageSize="30">
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <Settings ShowHorizontalScrollBar="True" ShowVerticalScrollBar="True" VerticalScrollableHeight="450" />
            </dxe:ASPxGridView>

            <dxe:ASPxPopupControl ID="MsgPopUp" runat="server" ClientInstanceName="cMsgPopUp"
                HeaderText="Notice" Left="100" ShowSizeGrip="False"
                Top="100" Width="387px" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <table class="style1">
                            <tr>
                                <td colspan="5">&nbsp;Do You Want To Edit This Entry?</td>
                            </tr>
                            <tr>
                                <td style="width: 3px"></td>
                                <td></td>
                                <td style="width: 164px"></td>
                                <td style="width: 3px">
                                    <dxe:ASPxButton ID="btnOk" ClientInstanceName="cbtnOk" runat="server" AutoPostBack="False"
                                        Text="Ok">
                                        <ClientSideEvents Click="function (s, e) { btnOkClick(); cMsgPopUp.Hide(); }" />
                                    </dxe:ASPxButton>
                                </td>
                                <td style="width: 3px">
                                    <dxe:ASPxButton ID="abtnCancel" runat="server" AutoPostBack="False"
                                        Text="Cancel">
                                        <ClientSideEvents Click="function (s, e) { cMsgPopUp.Hide(); parent.editwin.close(); }" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <dxe:ASPxPopupControl ID="FileUsedByPopUp" runat="server" ClientInstanceName="cFileUsedByPopUp"
                HeaderText="Notice" Left="100" meta:resourcekey="FileUsedByPopUpResource1" ShowSizeGrip="False"
                Top="100" Width="387px" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server" meta:resourceKey="PopupControlContentControlResource4">
                        <table class="style1">
                            <tr>
                                <td colspan="5">This Entry Was Already Being Edited By You.</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="width: 3px">
                                    <dxe:ASPxButton ID="btnContinue" runat="server" AutoPostBack="False" Height="6px"
                                        meta:resourceKey="btnContinueResource1" Text="Continue Previous Edit" Width="192px">
                                        <ClientSideEvents Click="function (s, e) { btnContinueClick(); cFileUsedByPopUp.Hide(); }" />
                                    </dxe:ASPxButton>
                                </td>
                                <td style="width: 3px">
                                    <dxe:ASPxButton ID="btnNewEntry" runat="server" AutoPostBack="False"
                                        Text="Fresh Edit" Width="140px">
                                        <ClientSideEvents Click="function (s, e) { btnFreshEntryClick();cFileUsedByPopUp.Hide(); }" />
                                    </dxe:ASPxButton>
                                </td>
                                <td style="width: 3px">
                                    <dxe:ASPxButton ID="btnClose" runat="server" AutoPostBack="False"
                                        Text="Discard Previous Edit" Width="140px">
                                        <ClientSideEvents Click="function (s, e) {btnCloseClick(); cFileUsedByPopUp.Hide(); }" />
                                    </dxe:ASPxButton>
                                </td>
                                <td style="width: 3px">
                                    <dxe:ASPxButton ID="btnCancle" runat="server" AutoPostBack="False"
                                        Text="Cancel" Width="140px">
                                        <ClientSideEvents Click="function (s, e) {parent.editwin.close(); }" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
        </div>


        <asp:HiddenField ID="hdnDefaultBranch" runat="server" />
        <asp:HiddenField ID="hdnType" runat="server" />
        <asp:HiddenField ID="hdnAccountType" runat="server" />
        <asp:HiddenField ID="txtMainAccount_hidden" runat="server" />
        <asp:HiddenField ID="txtSubAccount_hidden" runat="server" />
        <asp:HiddenField ID="hdn_Brch_NonBrch" runat="server" />
        <asp:HiddenField ID="hdn_SubLedgerType" runat="server" />
        <asp:HiddenField ID="hdn_MainAcc_Type" runat="server" />
        <asp:HiddenField ID="hdn_SubAccountIDE" runat="server" />
        <asp:HiddenField ID="txtBankAccounts_hidden" runat="server" />
        <asp:HiddenField ID="hdn_Mode" runat="server" Value="Entry" />
        <asp:HiddenField ID="hdn_PayeeIDOnUpdate" runat="server" />
        <asp:HiddenField ID="hdn_Brch_NonBrchE" runat="server" />
        <asp:HiddenField ID="hdn_CurrentSegment" runat="server" />
        <asp:HiddenField ID="hdn_CashBankType_InstTypeE" runat="server" />
        <asp:HiddenField ID="hdn_SegID_SegmentName" runat="server" />
        <asp:HiddenField ID="hdn_EditVoucher_SegmentID_Name" runat="server" />
        <asp:HiddenField ID="hdn_OriginalTDate" runat="server" />

        <div style="display: none">
            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
        </div>

        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
