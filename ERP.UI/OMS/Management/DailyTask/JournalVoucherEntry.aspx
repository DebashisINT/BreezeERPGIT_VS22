<%@ Page Title="Journal Voucher" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" Inherits="management_DailyTask_journalvoucherentry" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" CodeBehind="journalvoucherentry.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>

    <style type="text/css">
        .chosen-choices {
            width: 100% !important;
        }


        #lstMainAccount {
            width: 100%;
        }

        #lstMainAccount_chosen {
            width: 100% !important;
        }

        #lstSubAccount {
            width: 100%;
        }

        #lstSubAccount_chosen {
            width: 100% !important;
        }
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 100;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 5;
        }

        form {
            pers display: inline;
        }
    </style>
    <script type="text/javascript">
        function OnAddButtonClick() {

            document.getElementById('divAddNew').style.display = 'block';
            TblSearch.style.display = "none";
            btncross.style.display = "block";
            $('#<%=hdnMode.ClientID %>').val('0'); //Entry
            //alert($('#<%=hdnMode.ClientID %>').val());
            $('#<%=hdnJNMode.ClientID %>').val('0'); //Entry   
            $('#<%= lblHeading.ClientID %>').text("Add Journal Voucher");


            var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
            var st = $("#ddlBranch").val();
            document.getElementById('hdn_Brch_NonBrch').value = st;

            //cCmbScheme.SetValue("0");
            var CmbScheme = document.getElementById("<%=CmbScheme.ClientID%>");
            CmbScheme.options[0].selected = true;

            document.getElementById('<%= txtBillNo.ClientID %>').value = "";
            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;

            CountryID.PerformCallback(document.getElementById('ddlBranch').value);

            grid.AddNewRow();
            grid.batchEditApi.EndEdit();
            document.getElementById("<%=CmbScheme.ClientID%>").focus();

            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(false);

            //roupOwnerCombo.PerformCallback('Entry');
            //grid.UpdateEdit();
        }

        function OnCmbMainAccountSelectedIndexChanged() {

            // cGvAddRecordDisplay.PerformCallback('MainSubAccountChange~~');
            var SelectedValue = cCmbMainAccount.GetValue();
            var SelectedText = cCmbMainAccount.GetText();
            // cCmbMainAccount.PerformCallback("CmbSubAccount|" + SelectedValue + "|" + SelectedText);
            cCmbSubAccount.PerformCallback("CmbSubAccount|" + SelectedValue + "|" + SelectedText);
        }



        function SignOff() {
            window.parent.SignOff();
        }
        var isCtrl = false;
        document.onkeyup = function (e) {
            if (event.keyCode == 17) {
                isCtrl = false;
            }
            else if (event.keyCode == 27) {
                btnCancel_Click();
            }
        }


        document.onkeydown = function (e) {

            //if (event.keyCode == 17) isCtrl = true;
            if (event.keyCode == 83 && event.ctrlKey == true) {

                //run code for Ctrl+S -- ie, save!
                var debit = document.getElementById('txtTDebit').value;
                var credit = document.getElementById('txtTCredit').value;
                if (debit == credit) {
                    document.getElementById('btnSaveRecords').click();
                    return false;
                }
                else {
                    jAlert('Credit And Debit Must Be Same');
                    return false
                }
            }
            else if ((event.keyCode == 120 || event.keyCode == 88) && event.ctrlKey == true) {

                //run code for Ctrl+X -- ie, Save & Exit! 
                document.getElementById('btn_SaveRecords').click();
                return false;
            }
            else if ((event.keyCode == 120 || event.keyCode == 65) && event.ctrlKey == true) {
                //run code for Ctrl+E -- ie, Add New
                if (document.getElementById('divAddNew').style.display != 'block') {
                    OnAddButtonClick();
                }

            }
            //else if ((event.keyCode == 120 || event.keyCode == 68) && isCtrl == true) {

            //    //run code for CTRL+D -- ie, discard! 
            //    document.getElementById('btnDiscardEntry').click();
            //    return false;
            //}
            //else if ((event.keyCode == 120 || event.keyCode == 82) && isCtrl == true) {

            //    //run code for CTRL+R -- ie, Refresh! 
            //    document.getElementById('RefreshButtonClick').click();
            //    return false;
            //}
            //else if ((event.keyCode == 120 || event.keyCode == 76) && isCtrl == true) {

            //    //run code for CTRL+R -- ie, Add to List! 
            //    document.getElementById('btnadd').click();
            //    return false;
            //}
        }
        function height() {
            //if (document.body.scrollHeight >= 300)
            //    window.frameElement.height = document.body.scrollHeight;
            //else
            //    window.frameElement.height = '300px';
            //window.frameElement.Width = document.body.scrollWidth;
        }
        function CallList(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function updateEditorText() {
            var code = txtAccountCode.GetText().toUpperCase();
            if (code == 'X' || code == 'Y' || code == 'Z' || code == 'V' || code == 'U' || code == 'T') {
                jAlert('{T,U,V,W,X,Y,Z} are Reserved Key');
                txtAccountCode.SetText('JV');
            }
        }
        function CallMainAccount(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3, null, 'Main');
        }
        function CallSubAccount(obj1, obj2, obj3) {
            var valSub;
            var HdVal = document.getElementById("hddnEdit").value;
            if (HdVal == 'Edit') {
                var BranchID = document.getElementById("ddlBranch").value;
                valSub = val + '~' + BranchID;
            }
            else
                valSub = val + '~' + 'N';
            ajax_showOptions(obj1, obj2, obj3, valSub, 'Main');
        }
        function CallMainAccountE(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3, null, 'Main');
        }
        function CallSubAccountE(obj1, obj2, obj3) {
            var valSub;
            valSub = valE + '~' + 'N';
            ajax_showOptions(obj1, obj2, obj3, valSub, 'Main');
        }
        function keyVal(obj) {

            var spObj = obj.split('~');
            var WhichQuery = spObj[2];

            if (WhichQuery == 'MAINAC') {
                document.getElementById('txtsubaccount').value = '';
                if (spObj[3] != 'Edit') {
                    val = spObj[0];
                    valLedgerType = spObj[1];
                    document.getElementById("txtMainAccount_hidden").value = val;
                }
                else {
                    valE = spObj[0];
                    valLedgerTypeE = spObj[1];
                    document.getElementById("txtMainAccountE_hidden").value = val;
                }
                if (valLedgerType.toUpperCase() == 'NONE') {
                    document.getElementById('tdSubAccount').value = '';
                    document.getElementById('tdSubAccount').style.display = 'none';
                    document.getElementById('tdlblSubAccount').style.display = 'none';
                    document.getElementById('hdn_MainAcc_Type').value = 'None';
                    document.getElementById('hdn_Brch_NonBrch').value = 'NAB';
                    document.getElementById('txtSubAccount_hidden').value = '';
                }
                else {
                    document.getElementById('tdSubAccount').style.display = 'inline';
                    document.getElementById('tdlblSubAccount').style.display = 'inline';
                    document.getElementById('hdn_MainAcc_Type').value = 'NotNone';
                }
            }
            else if (WhichQuery == "SUBAC") {
                var Branch = spObj[1];
                document.getElementById('hdn_Brch_NonBrch').value = Branch;
                var MainAcCode = document.getElementById('txtMainAccount_hidden').value;
                var SubAc = document.getElementById('txtSubAccount_hidden').value;
                cCbpAcBalance.PerformCallback('AcBalance~' + MainAcCode + '~' + SubAc);
                //alert(Branch);
            }
            else {
                document.getElementById('hdn_Brch_NonBrch').value = 'NAB';
            }
        }
        function SubAccountCheck() {
            document.getElementById('btnnew').focus();
            var SubAccountBranch = document.getElementById('hdn_Brch_NonBrch').value;
            //alert(SubAccountBranch);
            cDetailsGrid.PerformCallback('Add~' + SubAccountBranch);
        }
        function SubAccountCheckUpdate(obj) {
            var obj1 = obj.split('_');
            if (valLedgerType.toUpperCase() != 'NONE') {
                var obj2 = 'grdAdd' + '_' + obj1[1] + '_' + 'txtEditSubAccount';
                var testSubAcc1 = document.getElementById(obj2);
                if (testSubAcc1.value == '') {
                    jAlert('SubAccount Name Required !!!');
                    testSubAcc1.focus();
                    testSubAcc1.select();
                    return false;
                }
            }
            var Withdraw = txtEditWithdraw.GetValue();
            var Receipt = txtEditRecpt.GetValue();
            var WithReceipt = parseFloat(Withdraw) + parseFloat(Receipt);
            if (WithReceipt == "0") {
                jAlert('Debit/Credit  Required !!!');
                return false;
            }
        }
        function btnCancel_Click() {

            jConfirm('Do you Want To Close This Window?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    parent.editwin.close();
                }
            })
        }

        function Page_Load() {
            //    //document.getElementById("btnSave").disabled = true;
            //    //document.getElementById("btnInsert").disabled = true;
        }
        function Page_Load1() {
            //    //document.getElementById("tdSeg1").style.display = "none";
            //    //document.getElementById("tdSeg2").style.display = "none";
        }
        //function Button_Click() {
        //    //document.getElementById("btnSave").disabled = false;
        //    //document.getElementById("btnInsert").disabled = false;
        //}
        function SetSubAcc1(obj) {
            var s = document.getElementById('txtSubAccount');
            s.focus();
            s.select();
        }
        //         function keyVal1(obj)
        //         {s
        //            alert(obj);
        //            val=obj
        //         }
        function PopulateData() {
            parent.RefreshGrid();
        }
        function Narration(obj) {
            document.getElementById("txtNarration1").value = obj;
        }
        function Narration1() {
            document.getElementById("txtNarration1").value = '';
        }
        function overChange(obj) {
            obj.style.backgroundColor = "#FFD497";
        }
        function OutChange(obj) {
            obj.style.backgroundColor = "#DDECFE";
        }
        function focusval(obj) {
            if (obj != '0.00') {
                ctxtcredit.SetEnabled(false);
                ctxtcredit.SetText('0000000000.00');
                OnlyPayment(obj, 'Dr');
            }
            else {
                ctxtcredit.SetEnabled(true);
            }
        }
        function focusval1(obj) {
            if (obj != '0.00') {
                ctxtdebit.SetEnabled(false);
                ctxtdebit.SetText('0000000000.00');
                OnlyPayment(obj, 'Cr');
            }
            else {
                ctxtdebit.SetEnabled(true);
            }
        }

        function Efocusval(obj) {
            if (obj != '0.00') {
                ctxtcreditE.SetEnabled(false);
                ctxtcreditE.SetText('0.00');
            }
            else {
                ctxtcreditE.SetEnabled(true);
            }
        }
        function Efocusval1(obj) {
            if (obj != '0.00') {
                ctxtdebitE.SetEnabled(false);
                ctxtdebitE.SetText('0.00');
            }
            else {
                ctxtdebitE.SetEnabled(true);
            }
        }
        function SelectSegment() {
            var obj = document.getElementById('ddlIntExchange').value;
            if (obj == "0") {
                document.getElementById("tdSeg1").style.display = "none";
                document.getElementById("tdSeg2").style.display = "none";
                txtAccountCode.SetText('JV');
                txtAccountCode.SetEnabled(true);
            }
            else {
                document.getElementById("tdSeg1").style.display = "inline";
                document.getElementById("tdSeg2").style.display = "inline";
                txtAccountCode.SetText('YF');
                txtAccountCode.SetEnabled(false);
            }
        }
        function SegmentName() {
            var obj = document.getElementById('ddlSegment').value;
            var obj1 = document.getElementById('ddlTntraExchange').value;
            if (obj == obj1) {
                jAlert('Segment And Segment2 Must Be Different');
                document.getElementById('ddlTntraExchange').selectedIndex = '0';
                return false;
            }
        }
        function Narration_Off() {
            document.getElementById('TrNarration').style.display = 'none';
        }
        function Narration_Val(obj) {
            document.getElementById('TrNarration').style.display = 'inline';
            document.getElementById('txtNarration1').value = obj;
        }
        function dateStringToTime(dateStr) {
            year = dateStr.substring(6, 8);
            month = dateStr.substring(0, 2);
            day = dateStr.substring(3, 5);

            return new Date(year, month, day).getTime();
        }

        ////This Method is Used For Checking Lock Date and Financial Year and Alert User For That if Date OutSide
        function DateChange() {

            var Ctype = $('#<%=hdnMode.ClientID %>').val();
            if (Ctype != 1) {
                var SelectedDate = new Date(tDate.GetDate());
                var monthnumber = SelectedDate.getMonth();
                var monthday = SelectedDate.getDate();
                var year = SelectedDate.getYear();

                var SelectedDateValue = new Date(year, monthnumber, monthday);
                ///Checking of Transaction Date For MaxLockDate
                var MaxLockDate = new Date('<%=Session["LCKJV"]%>');
                monthnumber = MaxLockDate.getMonth();
                monthday = MaxLockDate.getDate();
                year = MaxLockDate.getYear();
                var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();
                //                alert('TransactionDate='+TransactionDate+'\n'+'MaxLockDate= '+MaxLockDate);
                //alert(ValueDate+'~'+ValueDateNumeric+'~'+VisibleIndexE);
                if (SelectedDateValue <= MaxLockDateNumeric) {
                    jAlert('This Entry Date has been Locked.');
                    MaxLockDate.setDate(MaxLockDate.getDate() + 1);
                    tDate.SetDate(MaxLockDate);
                    return;
                }
            }

            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var SelectedDate = new Date(tDate.GetDate());
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);

            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);

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
                //alert("Between");

                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 1);
                }
            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    tDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    tDate.SetDate(new Date(FinYearEndDate));
                }
            }
        }
        ////This Method is Used For Checking Lock Date and Financial Year and With Out Alert User Change Date if Date OutSide(2012-04-07)
        function DateChange_WithOutAlert() {
            var Ctype = $('#<%=hdnMode.ClientID %>').val();
            if (Ctype != 1) {
                var SelectedDate = new Date(tDate.GetDate());
                var monthnumber = SelectedDate.getMonth();
                var monthday = SelectedDate.getDate();
                var year = SelectedDate.getYear();

                var SelectedDateValue = new Date(year, monthnumber, monthday);
                ///Checking of Transaction Date For MaxLockDate
                var MaxLockDate = new Date('<%=Session["LCKJV"]%>');
                monthnumber = MaxLockDate.getMonth();
                monthday = MaxLockDate.getDate();
                year = MaxLockDate.getYear();
                var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();
                //                alert('TransactionDate='+TransactionDate+'\n'+'MaxLockDate= '+MaxLockDate);
                //alert(ValueDate+'~'+ValueDateNumeric+'~'+VisibleIndexE);
                if (SelectedDateValue <= MaxLockDateNumeric) {
                    MaxLockDate.setDate(MaxLockDate.getDate() + 1);
                    tDate.SetDate(MaxLockDate);
                    return;
                }
            }

            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var SelectedDate = new Date(tDate.GetDate());
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);

            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);

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
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    tDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    tDate.SetDate(new Date(FinYearEndDate));
                }
            }
        }
        function AcBalance(obj) {
            var comp = document.getElementById('hdnCompanyid').value;
            var segmnt = document.getElementById('hdnSegmentid').value;
            var date = document.getElementById('tDate_I').value;
            var dest = document.getElementById('CbpAcBalance');
            var Suba = obj + "_hidden";
            var SubAcc = document.getElementById(Suba).value;
            var param = comp + '~' + segmnt + '~' + date + '~' + val + '~' + SubAcc;
            //alert(param);;
            PageMethods.GetContactName(param, CallSuccess, CallFailed, dest);
        }
        function AcBalance1(obj) {
            if (valLedgerType.toUpperCase() == 'NONE') {
                var comp = document.getElementById('hdnCompanyid').value;
                var segmnt = document.getElementById('hdnSegmentid').value;
                var date = document.getElementById('tDate_I').value;
                var dest = document.getElementById('CbpAcBalance');
                var Suba = obj + "_hidden";
                var SubAcc = document.getElementById(Suba).value;
                var param = comp + '~' + segmnt + '~' + date + '~' + val + '~' + '';
                //alert(param);;
                PageMethods.GetContactName(param, CallSuccess, CallFailed, dest);
            }
        }
        function CallSuccess(res, destCtrl) {
            //destCtrl.innerText=res;
            var cc = res.substr(0, 1);
            if (cc == '-') {
                //cc=res * (-1);
                cc = res + ' (DR)';
                lbltype = 'DR';
                destCtrl.innerText = cc;
                destCtrl.style.color = 'red';
            }
            else {
                cc = res + ' (CR)';
                lbltype = 'CR';
                destCtrl.innerText = cc;
                destCtrl.style.color = 'blue';
            }
            lblval = res;
        }
        function CallFailed(res, destCtrl) {
            alert(res.get_message());
        }
        function alertMessage() {
            jAlert('This Voucher has multi branch enters.\n Please Provide a single account for counter entry !!')
        }
        function btnInsert_Click() {
            document.getElementById('Div1').style.display = 'inline';
            document.getElementById('btnInsert').disabled = true;
            document.getElementById('btnSave').click();
        }
        function AlertAfterInsert() {
            var answer = confirm("Do You Want To Print this page??");
            if (answer) {
                document.getElementById('btnPrint').click();
            }
        }
        function OnlyPayment(obj, objType) {
            if (lbltype == 'DR' && objType == "Cr") {
                var str = lblval;
                str = str.replace(",", "");
                str = Math.abs(str);
                if (parseFloat(str) < parseFloat(obj)) {
                    document.getElementById('bDrCrStatus').innerHTML = '(Credit Is Greater Than Debit)';
                    document.getElementById('bDrCrStatus').style.font = "red";
                }
                else {
                    document.getElementById('bDrCrStatus').innerHTML = '';
                }
            }
            if (lbltype == 'CR' && objType == "Dr") {
                var str = lblval;
                str = str.replace(",", "");
                str = Math.abs(str);
                if (parseFloat(str) < parseFloat(obj)) {
                    document.getElementById('bDrCrStatus').innerHTML = '(Debit Is Greater Than Credit)';
                    document.getElementById('bDrCrStatus').style.font = "blue";
                }
                else {
                    document.getElementById('bDrCrStatus').innerHTML = '';
                }
            }
        }
        function OnlyNarration(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        FieldName = 'txtPrefix';
        function AddButtonClick() {
            cDetailsGrid.PerformCallback('Add');
        }
        function CancelButtonClick() {
            $('#<%=hdnSubAccountId.ClientID %>').val('');
            $('#<%=hdnSubAccountText.ClientID %>').val('');
            $('#<%=hdnMainAccountId.ClientID %>').val('');
            $('#<%=hdnMainAccountText.ClientID %>').val('');

            BindMainAccountList();
            BindSubAccountList();
            document.getElementById('<%=txtNarration1.ClientID%>').value = '';
            document.getElementById('<%=txtcredit.ClientID%>').value = '';
            document.getElementById('<%=txtdebit.ClientID%>').value = '';

        }
        function SaveButtonClick() {
            //var val = cCmbScheme.GetValue();

            var val = document.getElementById("CmbScheme").value;
            var Branchval = $("#ddlBranch").val();

            <%--var val = "";
            var AspRadio = document.getElementById('<%= rblScheme.ClientID %>');
            var AspRadio_ListItem = document.getElementsByTagName('input');
            for (var i = 0; i < AspRadio_ListItem.length; i++) {
                if (AspRadio_ListItem[i].checked) {
                    val = AspRadio_ListItem[i].value;
                }
            }--%>


            $('#<%=hdnRefreshType.ClientID %>').val('S');

            if (document.getElementById('<%= txtBillNo.ClientID %>').value == "") {
                jAlert('Enter Journal No');
                document.getElementById('<%= txtBillNo.ClientID %>').focus();
            }
            else if (Branchval == "0") {
                document.getElementById('<%= ddlBranch.ClientID %>').focus();
                jAlert('Enter Branch');
            }
            else {
                if (grid.GetVisibleRowsOnPage() > 0) {
                    //cDetailsGrid.PerformCallback('Save');
                    grid.UpdateEdit();
                }
                else {
                    jAlert('Please add atleast single record first');
                }
            }

            //    if (val != "0") {                
            //}
            //else {
            //    jAlert('Please Select Numbering Scheme');
            //    cCmbScheme.Focus();
            //}
    }
    function Save_ButtonClick() {
        //var val = cCmbScheme.GetValue();
        $('#<%=hdnRefreshType.ClientID %>').val('E');

        if (document.getElementById('<%= txtBillNo.ClientID %>').value == "") {
            jAlert('Enter Journal No');
        }
        else {
            if (grid.GetVisibleRowsOnPage() > 0) {
                //cDetailsGrid.PerformCallback('Save');
                grid.UpdateEdit();
            }
            else {
                jAlert('Please add atleast single record first');
            }
        }
    }
    function DiscardButtonClick() {

        jConfirm('Do you want to discard all data you entered?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                cDetailsGrid.PerformCallback('Discard');
            }
        });
    }
    function RefreshButtonClick() {
        jConfirm('Back To Initial Position?', 'Confirmation Dialog', function (r) {
            if (r == true) {

                if (cDetailsGrid.GetVisibleRowsOnPage() != '0') {
                    cDetailsGrid.PerformCallback('DiscardOnRefresh');
                }
                TblMainEntryForm.style.display = "none";

                //var SelectedValue = cComboMode.GetValue();
                var SelectedValue = $('#<%=hdnMode.ClientID %>').val();
                if (SelectedValue == "1") {
                    tdEntryButton.style.display = "none";
                    tdSearchButton.style.display = "inline";
                }
                else {
                    tdEntryButton.style.display = "inline";
                    tdSearchButton.style.display = "none";
                }
                // document.getElementById('txtMainAccount').style.display = 'inline';
                // document.getElementById('txtMainAccount').value = '';
                // document.getElementById('txtSubAccount').value = '';
                ctxtdebit.SetText('0000000000.00');
                ctxtcredit.SetText('0000000000.00');
                txtAccountCode.SetText('JV');
                document.getElementById('txtNarration').value = '';
                document.getElementById('txtBillNo').value = '';
                var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
                ddlBranch.options[0].selected = true;
                //             tDate.SetDate(new Date()); 
                document.getElementById('CbpAcBalance').innerHTML = '';
                tdAcBal.style.display = "none";
                document.getElementById('CbpAcBalance').innerHTML = '';
                document.getElementById('bDrCrStatus').innerHTML = '';

            }
        });
    }
    function NewButtonClick() {
        if (parseFloat(ctxtTDebit.GetValue()) > parseFloat(ctxtTCredit.GetValue())) {
            ctxtdebit.SetText('0000000000.00');
        }
        else {
            ctxtcredit.SetText('0000000000.00');
        }
        tdadd.style.display = 'inline'
        tdnew.style.display = 'none'
        ctxtcredit.SetEnabled(true);
        ctxtdebit.SetEnabled(true);
        //if (document.getElementById('tdSubAccount').style.display == 'none') {
        //   // document.getElementById('txtMainAccount').focus();
        //}
        //else {
        //    document.getElementById('txtSubAccount').value = '';
        //    document.getElementById('txtSubAccount').focus();

        //}
        document.getElementById('CbpAcBalance').innerHTML = '';
        document.getElementById('bDrCrStatus').innerHTML = '';
    }
    function SetDebitCreditValue(obj) {
        var TotalDebit = (obj.split('~')[0]);
        var TotalCredit = (obj.split('~')[1]);
        var RemainingDebit = (obj.split('~')[2]);
        var RemainingCredit = (obj.split('~')[3]);
        //alert(TotalDebit+' '+TotalDebit+' '+RemainingDebit+' '+RemainingCredit);
        ctxtdebit.SetText('0000000000.00');
        ctxtcredit.SetText('0000000000.00');
        ctxtdebit.SetText(RemainingDebit.toString());
        ctxtcredit.SetText(RemainingCredit.toString());
        ctxtTDebit.SetText('0000000000.00');
        ctxtTCredit.SetText('0000000000.00');
        ctxtTDebit.SetText(TotalDebit.toString());
        ctxtTCredit.SetText(TotalCredit.toString());

        if (TotalDebit == TotalCredit) {
            tdSaveButton.style.display = "inline"
            ctxtdebit.SetText('0000000000.00');
            ctxtcredit.SetText('0000000000.00');
        }
        else {
            if (parseFloat(ctxtTDebit.GetValue()) > parseFloat(ctxtTCredit.GetValue())) {
                ctxtdebit.SetText('0000000000.00');
            }
            else {
                ctxtcredit.SetText('0000000000.00');
            }
            tdSaveButton.style.display = "none"
        }
        if (cDetailsGrid.cpEntryNotAllow != "undefined") {
            if (cDetailsGrid.cpEntryNotAllow != "Empty") {
                jAlert(cDetailsGrid.cpEntryNotAllow);
                cDetailsGrid.cpEntryNotAllow = "undefined";
            }
        }
        if (cDetailsGrid.cpSaveSuccessOrFail != "undefined") {
            if (cDetailsGrid.cpSaveSuccessOrFail == "Problem") {
                jAlert("There is Some Problem. Sry for InConvenience");
                cDetailsGrid.cpSaveSuccessOrFail = "undefined";
            }
            else if (cDetailsGrid.cpSaveSuccessOrFail == "Success") {
                ResetPageOnSave();
                jAlert("Records saved successfully");
                cDetailsGrid.cpSaveSuccessOrFail = "undefined";
                //var answer = confirm("Do You Want To Print Saved JV/JVs?");
                //if (answer) {
                //    document.getElementById('btnPrint').click();
                //}
            }
            else {
            }
        }
        if (cDetailsGrid.cpSuccessDiscard != "undefined") {
            if (cDetailsGrid.cpSuccessDiscard == "Problem") {
                jAlert('There is Some Problem. Sry for InConvenience');
                cDetailsGrid.cpSuccessDiscard = "undefined";
            }
            else if (cDetailsGrid.cpSuccessDiscard == "SuccessDiscard") {
                ResetPageOnDiscard();
                jAlert('Records successfully discarded');
                cDetailsGrid.cpSuccessDiscard = "undefined";
            }
            else {
            }
        }
        if (cDetailsGrid.cpEntryData != "undefined") {

            if (cDetailsGrid.cpBillNo != "EmptyString") document.getElementById('txtBillNo').value = cDetailsGrid.cpBillNo;
            if (cDetailsGrid.cpJvNarration != "EmptyString") document.getElementById('txtNarration').value = cDetailsGrid.cpJvNarration;
            if (cDetailsGrid.cpPrefix != "EmptyString") txtAccountCode.SetText(cDetailsGrid.cpPrefix);
            var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
            if (cDetailsGrid.cpBranchSelectedValue != "EmptyString") ddlBranch.options[cDetailsGrid.cpBranchSelectedValue].selected = true;
        }
        if (cDetailsGrid.cpHideAddBtnOnLock == "true") {
            cbtnadd.SetEnabled(true);
            cDetailsGrid.cpHideAddBtnOnLock = null;
        }
        if (cDetailsGrid.cpHideAddBtnOnLock == "false") {
            cbtnadd.SetEnabled(false);
            cDetailsGrid.cpHideAddBtnOnLock = null;
        }

        //Currency Setting
        if (cDetailsGrid.cpSetCurrencyNameSymbol != null) {
            var ChoosenCurrencyName = cDetailsGrid.cpSetCurrencyNameSymbol.split('~')[0];
            var ChoosenCurrencySymbol = cDetailsGrid.cpSetCurrencyNameSymbol.split('~')[1];
            document.getElementById('<%=B_ChoosenCurrency.ClientID %>').innerHTML = "Voucher Currency : " + ChoosenCurrencyName + "[" + ChoosenCurrencySymbol + "]";
        }

        height();

    }
    function EntryButtonClick() {
        var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
        var st = $("#ddlBranch").val();
        document.getElementById('hdn_Brch_NonBrch').value = st;

        document.getElementById('divAddNew').style.display = 'block';
        TblMainEntryForm.style.display = "inline";
        tdEntryButton.style.display = "none";
        tdAcBal.style.display = "inline";

        //PanelMainAccount.style.display = "block";

        cDetailsGrid.PerformCallback('Entry');
        cDetailsGrid.cpEntryNotAllow = '';
    }
    function SearchButtonClick() {
        cSearchPopUp.Show();
    }
    function ResetPageOnSave() {
        // Asp_SetSpace('txtMainAccount');
        // Asp_SetSpace('txtSubAccount');
        ctxtdebit.SetEnabled(true);
        ctxtcredit.SetEnabled(true);
        ctxtcredit.SetText('0000000000.00');
        ctxtdebit.SetText('0000000000.00');
        ctxtTCredit.SetText('0000000000.00');
        ctxtTDebit.SetText('0000000000.00');
        TblMainEntryForm.style.display = 'none'

        //var SelectedValue = cComboMode.GetValue();
        var SelectedValue = $('#<%=hdnMode.ClientID %>').val();
        if (SelectedValue == "1") {
            tdEntryButton.style.display = "none";
            tdSearchButton.style.display = "inline";
        }
        else {
            tdEntryButton.style.display = "inline";
            tdSearchButton.style.display = "none";
        }
        tdadd.style.display = 'inline';
        tdnew.style.display = 'none';
        tdAcBal.style.display = "none";
        tdSaveButton.style.display = 'inline';
        txtAccountCode.SetText('JV');
        document.getElementById('txtNarration').value = '';
        document.getElementById('txtBillNo').value = '';
        var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
        ddlBranch.options[0].selected = true;
        tDate.SetDate(new Date());
        document.getElementById('CbpAcBalance').innerHTML = '';
        DateChange_WithOutAlert();
    }
    function ResetPageOnDiscard() {
        // Asp_SetSpace('txtMainAccount');
        //   Asp_SetSpace('txtSubAccount');
        ctxtcredit.SetText('0000000000.00');
        ctxtdebit.SetText('0000000000.00');
        ctxtTCredit.SetText('0000000000.00');
        ctxtTDebit.SetText('0000000000.00');
        ctxtdebit.SetEnabled(true);
        ctxtcredit.SetEnabled(true);
        tdadd.style.display = 'inline';
        tdnew.style.display = 'none';
        tdSaveButton.style.display = 'inline';
        //if (document.getElementById('txtMainAccount').style.display != 'none') {
        //    document.getElementById('txtMainAccount').focus();
        //}
        //else {
        //    tDate.focus();
        //}
        document.getElementById('CbpAcBalance').innerHTML = '';
        document.getElementById('txtNarration1').value = '';
    }
    function Asp_SetSpace(obj) {
        document.getElementById(obj).value = '';
    }
    function focustxtMainAccountOnUpdateCancel() {
        //document.getElementById('txtMainAccount').focus();
    }
    function OnComboModeSelectedIndexChanged() {

        TblMainEntryForm.style.display = "none";
        // PanelMainAccount.style.display = "none";
        TblSearch.style.display = "none";
        document.getElementById('txtBillNo').value = '';
        document.getElementById('txtNarration').value = '';
        //Asp_SetSpace('txtMainAccount');
        //Asp_SetSpace('txtSubAccount');
        Asp_SetSpace('txtNarration1');
        ctxtdebit.SetEnabled(true);
        ctxtcredit.SetEnabled(true);
        ctxtcredit.SetText('0000000000.00');
        ctxtdebit.SetText('0000000000.00');
        ctxtTCredit.SetText('0000000000.00');
        ctxtTDebit.SetText('0000000000.00');
        tdadd.style.display = 'inline'
        tdnew.style.display = 'none'
        var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
        ddlBranch.options[0].selected = true;
        tDate.Focus();
        var SelectedValue = $('#<%=hdnMode.ClientID %>').val();
        if (SelectedValue == "1") {
            tdEntryButton.style.display = "none";
            // PanelMainAccount.style.display = "none";
            tdSearchButton.style.display = "inline";
        }
        else {
            tdEntryButton.style.display = "inline";
            tdSearchButton.style.display = "none";
            tDate.SetDate(new Date());
            //cGvJvSearch.PerformCallback('PCB_EditEnd~');
        }
        tdAcBal.style.display = "none";
    }
    function btnShowClick() {
        cGvJvSearch.PerformCallback('PCB_BtnShow~');
        cSearchPopUp.Hide();
        //            if(cChkBranch.GetChecked()) cChkBranch.SetChecked(false);
        //            if(cChkBillNo.GetChecked()) cChkBillNo.SetChecked(false);
        //            if(cChkPrefix.GetChecked()) cChkPrefix.SetChecked(false);
        //            if(cChkNarration.GetChecked()) cChkNarration.SetChecked(false);
        TblSearch.style.display = "table";
    }
    function CustomButtonClick(s, e) {
        var TransactionDate = new Date(tDate.GetDate());
        monthnumber = TransactionDate.getMonth();
        monthday = TransactionDate.getDate();
        year = TransactionDate.getYear();
        var TransactionDateNumeric = new Date(year, monthnumber, monthday).getTime();

        var MaxLockDate = new Date('<%=Session["LCKJV"]%>');
        monthnumber = MaxLockDate.getMonth();
        monthday = MaxLockDate.getDate();
        year = MaxLockDate.getYear();
        var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();
        //                alert('TransactionDate='+TransactionDate+'\n'+'MaxLockDate= '+MaxLockDate);
        //alert(ValueDate+'~'+ValueDateNumeric+'~'+VisibleIndexE);
        if (TransactionDateNumeric <= MaxLockDateNumeric) {


            jAlert('This Entry has been Locked.You Can Only View The Detail');



            VisibleIndexE = e.visibleIndex;
            cGvJvSearch.PerformCallback('PCB_BtnOkE~' + e.visibleIndex);
            return;
        }
        if (e.buttonID == 'CustomBtnEdit') {
            //alert('Do u want to edit');
            //cMsgPopUp.Show();
            VisibleIndexE = e.visibleIndex;

            $('#<%=hdnMode.ClientID %>').val('1');
            $('#<%=hdnJNMode.ClientID %>').val('1');
            document.getElementById('div_Edit').style.display = 'none';
            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;

            //cGvJvSearch.PerformCallback('PCB_BtnOkE~' + VisibleIndexE);

            document.getElementById('divAddNew').style.display = 'block';
            btncross.style.display = "block";
            TblSearch.style.display = "none";
            // $('#ddlBranch').attr('Disabled', 'Disable');
            //cCbpGridBind.PerformCallback('Edit~' + VisibleIndexE);
            //grid.PerformCallback('PCB_BtnOkE~' + VisibleIndexE);
            grid.PerformCallback('Edit~' + VisibleIndexE);
            cSearchPopUp.Hide();

        }
        if (e.buttonID == 'CustomBtnDelete') {
            VisibleIndexE = e.visibleIndex;

            var result = confirm("Confirm delete?");
            if (result) {
                cGvJvSearch.PerformCallback('PCB_DeleteBtnOkE~' + VisibleIndexE);
            }

            //cDeleteMsgPopUp.Show();
        }

        if (e.buttonID == 'CustomBtnPrint') {
            var keyValueindex = e.visibleIndex;
            var result = confirm("Do you want to Print?");
            if (result) {
                //window.location.href = "../../reports/XtraReports/JournalVoucherReportViewer.aspx?id=12";

                cGvJvSearch.GetRowValues(keyValueindex, "JvID", onPrintJv)
            }
        }

    }

    function onPrintJv(id) {
        window.location.href = "../../reports/XtraReports/JournalVoucherReportViewer.aspx?id=" + id;
    }

    function btnOkClick() {
        $('#<%=hdnMode.ClientID %>').val('1');
        $('#<%=hdnJNMode.ClientID %>').val('1');
        document.getElementById('div_Edit').style.display = 'none';
        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;

        //cGvJvSearch.PerformCallback('PCB_BtnOkE~' + VisibleIndexE);

        document.getElementById('divAddNew').style.display = 'block';
        btncross.style.display = "block";
        TblSearch.style.display = "none";

        //cCbpGridBind.PerformCallback('Edit~' + VisibleIndexE);
        grid.PerformCallback('Edit~' + VisibleIndexE);
        //grid.PerformCallback('PCB_BtnOkE~' + VisibleIndexE);
        cSearchPopUp.Hide();
    }
    function DeletebtnOkClick() {
        cGvJvSearch.PerformCallback('PCB_DeleteBtnOkE~' + VisibleIndexE);
    }
    function btnContinueClick() {
        cGvJvSearch.PerformCallback('PCB_ContinueWith~' + VisibleIndexE);
    }
    function btnFreshEntryClick() {
        cGvJvSearch.PerformCallback('PCB_FreshEntry~' + VisibleIndexE);
    }
    function btnCloseClick() {
        cGvJvSearch.PerformCallback('PCB_CloseEntry~' + VisibleIndexE);
    }

    function GvJvSearch_EndCallBack() {
        if (cGvJvSearch.cpJVE_FileAlreadyUsedBy != undefined) {
            var obj = cGvJvSearch.cpJVE_FileAlreadyUsedBy;
            var WhichUser = (obj.split('~')[0]);
            if (WhichUser == "Other") {
                jAlert('This File Being Used By ' + obj.split('~')[1]);
            }
            else {
                cFileUsedByPopUp.Show();
            }

        }
        if (cGvJvSearch.cpEntryEventFire != undefined) {
            TblSearch.style.display = "none";
            tdSearchButton.style.display = "none";
            EntryButtonClick();
        }
        if (cGvJvSearch.cpJVDelete != undefined) {
            jAlert(cGvJvSearch.cpJVDelete);
            cGvJvSearch.PerformCallback('PCB_BindAfterDelete');
        }
        if (cGvJvSearch.cpJVClose != undefined) {
            jAlert(cGvJvSearch.cpJVClose);
        }
        height();
    }
    function blinkIt() {
        if (!document.all) return;
        else {
            for (i = 0; i < document.all.tags('blink').length; i++) {
                s = document.all.tags('blink')[i];
                s.style.visibility = (s.style.visibility == 'visible') ? 'hidden' : 'visible';
            }
        }
    }

    //Currency Setting
    function PageLoad_ForCurrency() {
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
        document.getElementById('<%=B_ChoosenCurrency.ClientID %>').innerHTML = "Voucher Currency : " + ActiveCurrencyName + "[" + ActiveCurrencySymbol + "]";
    }
    function ChangeCurrency() {
        cCbpChoosenCurrency.PerformCallback("ChangeCurrency");
    }
    function CbpChoosenCurrency_EndCallBack() {
        //            alert(cCbpChoosenCurrency.cpChangeCurrencyParam);
        if (cCbpChoosenCurrency.cpChangeCurrencyParam != null) {
            ActiveCurrencyName = cCbpChoosenCurrency.cpChangeCurrencyParam.split('~')[0];
            ActiveCurrencySymbol = cCbpChoosenCurrency.cpChangeCurrencyParam.split('~')[1];
            document.getElementById('<%=B_ChoosenCurrency.ClientID %>').innerHTML = "Voucher Currency : " + ActiveCurrencyName + "[" + ActiveCurrencySymbol + "]";
        }
    }
    function CbpAcBalance_EndCallBack() {
        var strUndefined = new String(cCbpAcBalance.cpAcBalance);
        if (strUndefined != "undefined") {
            document.getElementById('<%=B_AcBalance.ClientID %>').innerHTML = strUndefined.split('~')[0];
            document.getElementById('<%=B_AcBalance.ClientID %>').style.color = strUndefined.split('~')[1];
        }
    }
    ////////


    $(function () {
        BindMainAccountList();
        //ListAccountBind();
        ListMainAccountBind();
        ListSubAccountBind();

        $("#lstMainAccount").chosen().change(function () {
            var MainAccountId = $("#lstMainAccount").val();
            var MainAccountText = $("#lstMainAccount").find("option:selected").text()
            $('#<%=hdnMainAccountId.ClientID %>').val(MainAccountId);
            $('#<%=hdnMainAccountText.ClientID %>').val(MainAccountText);

            BindSubAccountList();
        })

        $("#lstSubAccount").chosen().change(function () {
            var SubAccountId = $("#lstSubAccount").val();
            var SubAccountText = $("#lstSubAccount").find("option:selected").text()
            $('#<%=hdnSubAccountId.ClientID %>').val(SubAccountId);
            $('#<%=hdnSubAccountText.ClientID %>').val(SubAccountText);
        })
    });

    //kaushik 28_11_2016 BindMainAccountList
    function BindMainAccountList() {
        var strQuery_Table = "Master_MainAccount";
        var strQuery_FieldName = "  MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType as MainAccount_ReferenceID";
        var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);


        var lBox = $('select[id$=lstMainAccount]');
        var lstMainAccountItems = [];
        //Customer or Lead radio button is clicked kaushik 21-11-2016

        lBox.empty();

        $.ajax({
            type: "POST",
            url: 'JournalVoucherEntry.aspx/GetMainAccountList',
            data: "{CombinedQuery:\"" + CombinedQuery + "\"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                if (list.length > 0) {

                    for (var i = 0; i < list.length; i++) {

                        var id = '';
                        var name = '';
                        id = list[i].split('|')[1];
                        name = list[i].split('|')[0];

                        lstMainAccountItems.push('<option value="' +
                        id + '">' + name
                        + '</option>');
                    }

                    $(lBox).append(lstMainAccountItems.join(''));
                    ListMainAccountBind();
                    $('#lstMainAccount').trigger("chosen:updated");
                    $('#lstMainAccount').prop('disabled', false).trigger("chosen:updated");

                }
                else {
                    lBox.empty();
                    ListMainAccountBind();
                    $('#lstMainAccount').trigger("chosen:updated");
                    $('#lstMainAccount').prop('disabled', true).trigger("chosen:updated");

                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });


    }



    function ListMainAccountBind() {
        var config = {
            '.chsn': {},
            '.chsn-deselect': { allow_single_deselect: true },
            '.chsn-no-single': { disable_search_threshold: 10 },
            '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chsn-width': { width: "100%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }
    }


    function ListSubAccountBind() {
        var config = {
            '.chsn': {},
            '.chsn-deselect': { allow_single_deselect: true },
            '.chsn-no-single': { disable_search_threshold: 10 },
            '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chsn-width': { width: "100%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }
    }

    //kaushik 28_11_2016 BindSubAccountList
    function BindSubAccountList() {
        var MainAccountCode = $('#<%=hdnMainAccountId.ClientID %>').val();
        var SegID = document.getElementById("hdnSegmentid").value;
        var SegmentName = document.getElementById("hdn_SegID_SegmentName").value;
        //alert(SegID + ' ' + SegmentName);
        var ProcedureName = "SubAccountSelect_New";
        var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
        var InputType = "V|V|V|V|V";
        var InputValue = MainAccountCode.split('~')[0] + "|RequestLetter|" + '<%=Session["userbranchHierarchy"] %>' + "|'" + '<%=Session["ExchangeSegmentID"] %>' + "'|'" + SegmentName + "'";
            var SplitChar = "|";
            var CombinedQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;
            var lBox = $('select[id$=lstSubAccount]');
            var lstSubAccountItems = [];
        //Customer or Lead radio button is clicked kaushik 21-11-2016

            lBox.empty();
            $.ajax({
                type: "POST",
                url: 'JournalVoucherEntry.aspx/GetSubAccountList',
                data: "{CombinedQuery:\"" + CombinedQuery + "\"}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    if (list.length > 0) {
                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            lstSubAccountItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lBox).append(lstSubAccountItems.join(''));
                        ListSubAccountBind();
                        $('#lstSubAccount').trigger("chosen:updated");
                        $('#lstSubAccount').prop('disabled', false).trigger("chosen:updated");

                    }
                    else {
                        lBox.empty();
                        ListSubAccountBind();
                        $('#lstSubAccount').trigger("chosen:updated");
                        $('#lstSubAccount').prop('disabled', true).trigger("chosen:updated");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        }

    </script>
    <style>
        #txtAccountCode_EC {
            position: absolute;
        }

        #GvJvSearch {
            width: 100% !important;
        }

        #CbpChoosenCurrency {
            float: left;
            line-height: 20px;
        }
    </style>
    <script type="text/javascript">
        var currentEditableVisibleIndex;
        var preventEndEditOnLostFocus = false;
        var lastCountryID;
        var setValueFlag;
        var debitOldValue;
        var debitNewValue;
        var CreditOldValue;
        var CreditNewValue;

        function CountriesCombo_SelectedIndexChanged(s, e) {
            var currentValue = grid.GetEditor('MainAccount1').GetValue();
            //var currentValue = s.GetValue();
            if (lastCountryID == currentValue) {
                if (CityID.GetSelectedIndex() < 0)
                    CityID.SetSelectedIndex(0);
                return;
            }
            lastCountryID = currentValue;
            CityID.PerformCallback(currentValue);
        }
        function IntializeGlobalVariables(grid) {
            lastCountryID = grid.cplastCountryID;
            currentEditableVisibleIndex = -1;
            setValueFlag = -1;
        }
        function OnInit(s, e) {
            IntializeGlobalVariables(s);
        }

        function OnEndCallback(s, e) {


            IntializeGlobalVariables(s);

            if (grid.cpEdit != null) {
                var VoucherNo = grid.cpEdit.split('~')[0];
                var Narration = grid.cpEdit.split('~')[1];
                var BranchID = grid.cpEdit.split('~')[2];
                var Credit = grid.cpEdit.split('~')[3];
                var Debit = grid.cpEdit.split('~')[4];

                document.getElementById('txtBillNo').value = VoucherNo;
                document.getElementById('txtNarration').value = Narration;

                <%--var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
                ddlBranch.options[BranchID].selected = true;--%>

                <%--var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
                ddlBranch.Items.FindByValue(BranchID).Selected = true;--%>

                var dropdownlistbox = document.getElementById("ddlBranch")

                for (var x = 0; x < dropdownlistbox.length - 1 ; x++) {
                    if (BranchID == dropdownlistbox.options[x].value) {
                        dropdownlistbox.selectedIndex = x;
                        break;
                    }
                }

                //Bind again the main account with respect to branch
                CountryID.PerformCallback(document.getElementById('ddlBranch').value);




                c_txt_Credit.SetValue(Credit);
                c_txt_Debit.SetValue(Debit);

                if (Debit == Credit) {
                    cbtnSaveRecords.SetVisible(true);
                    cbtn_SaveRecords.SetVisible(true);
                }
                else {
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);
                }
            }

            var value = document.getElementById('hdnRefreshType').value;

            if (grid.cpSaveSuccessOrFail == "outrange") {
                jAlert('Can Not Add More Journal Voucher as Journal Scheme Exausted.<br />Update The Scheme and Try Again');
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                jAlert('Can Not Save as Duplicate Journal Voucher No. Found');
            }
            else {
                var JV_Number = grid.cpVouvherNo;
                var JV_Msg = "Journal Voucher No. " + JV_Number + " generated.";
                var strSchemaType = document.getElementById('hdnSchemaType').value;

                if (value == "E") {
                    var IsComplete = "0";

                    if (JV_Number != "") {
                        var strconfirm = confirm(JV_Msg);
                        if (strconfirm == true) {
                            window.location.reload();
                        }
                        else {
                            window.location.reload();
                        }
                    } else {
                        window.location.reload();
                    }
                }
                else if (value == "S") {
                    var IsComp = "0";

                    if (JV_Number != "") {
                        var strconfirm = confirm(JV_Msg);
                        if (strconfirm == true) {
                            IsComp = "1";
                        } else {
                            IsComp = "1";
                        }
                    }
                    else {
                        IsComp = "1";
                    }

                    if (IsComp == "1") {
                        grid.AddNewRow();
                        $('#<%=hdnMode.ClientID %>').val('0');
                        cbtnSaveRecords.SetVisible(false);
                        cbtn_SaveRecords.SetVisible(false);
                        document.getElementById('div_Edit').style.display = 'block';
                        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                        //cCmbScheme.SetValue("0");
                        document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                        c_txt_Debit.SetValue("0");
                        c_txt_Credit.SetValue("0");
                        document.getElementById('<%= txtNarration.ClientID %>').value = "";
                        //cCmbScheme.Focus();

                        if (strSchemaType == "0") {
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                            //document.getElementById("txtBillNo").focus();
                            //cCmbScheme.Focus();

                            document.getElementById("CmbScheme").focus();
                        }
                        else if (strSchemaType == "1") {
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Auto";
                            grid.batchEditApi.StartEdit(-1, 1);
                        }
                        else if (strSchemaType == "2") {
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Datewise";
                            grid.batchEditApi.StartEdit(-1, 1);
                        }
                        else {
                            //cCmbScheme.SetValue("0");
                            //cCmbScheme.Focus();

                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "";

                            var CmbScheme = document.getElementById("<%=CmbScheme.ClientID%>");
                            CmbScheme.options[0].selected = true;
                            document.getElementById("CmbScheme").focus();
                        }
            }
        }
        else {
            grid.AddNewRow();
        }
}
}

function CitiesCombo_EndCallback(s, e) {
    if (setValueFlag == -1)
        s.SetSelectedIndex(0);
    else if (setValueFlag > -1) {
        CityID.SetSelectedItem(CityID.FindItemByValue(setValueFlag));
        setValueFlag = -1;
    }
}
function OnBatchEditStartEditing(s, e) {
    currentEditableVisibleIndex = e.visibleIndex;
    var currentCountryID = grid.batchEditApi.GetCellValue(currentEditableVisibleIndex, "MainAccount1");
    var cityIDColumn = s.GetColumnByField("SubAccount1");
    if (!e.rowValues.hasOwnProperty(cityIDColumn.index))
        return;
    var cellInfo = e.rowValues[cityIDColumn.index];
    if (lastCountryID == currentCountryID)
        if (CityID.FindItemByValue(cellInfo.value) != null)
            CityID.SetValue(cellInfo.value);
        else
            RefreshData(cellInfo, lastCountryID);
    else {
        if (currentCountryID == null) {
            CityID.SetSelectedIndex(-1);
            return;
        }
        lastCountryID = currentCountryID;
        RefreshData(cellInfo, lastCountryID);
    }
}
function RefreshData(cellInfo, countryID) {
    setValueFlag = cellInfo.value;
    setTimeout(function () {
        CityID.PerformCallback(countryID);
    }, 0);
}
//Debjyoti 
function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();

        var debit = grid.batchEditApi.GetCellValue(e.visibleIndex, "WithDrawl");
        var credit = grid.batchEditApi.GetCellValue(e.visibleIndex, "Receipt");
        if (debit != 0)
            c_txt_Debit.SetValue(c_txt_Debit.GetValue() - debit);
        if (credit != 0)
            c_txt_Credit.SetValue(c_txt_Credit.GetValue() - credit);

        var Debit = parseFloat(c_txt_Debit.GetValue());
        var Credit = parseFloat(c_txt_Credit.GetValue());

        if (Debit == 0 && Credit == 0) {
            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(false);
        }
        else if (Debit == Credit) {
            cbtnSaveRecords.SetVisible(true);
            cbtn_SaveRecords.SetVisible(true);
        }
        else {
            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(false);
        }

        grid.DeleteRow(e.visibleIndex);
    }
}


function CreditGotFocus(s, e) {
    CreditOldValue = s.GetText();
    var indx = CreditOldValue.indexOf(',');
    if (indx != -1) {
        CreditOldValue = CreditOldValue.replace(/,/g, '');
    }
}

function CreditLostFocus(s, e) {
    CreditNewValue = s.GetText();
    var indx = CreditNewValue.indexOf(',');
    if (indx != -1) {
        CreditNewValue = CreditNewValue.replace(/,/g, '');
    }

    if (CreditOldValue != CreditNewValue) {
        changeCreditTotalSummary();
    }
}
function changeCreditTotalSummary() {
    var newDif = CreditOldValue - CreditNewValue;
    var CurrentSum = c_txt_Credit.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    c_txt_Credit.SetValue(parseFloat(CurrentSum - newDif));
}
function recalculateCredit(oldVal) {
    if (oldVal != 0) {
        CreditNewValue = 0;
        CreditOldValue = oldVal;
        changeCreditTotalSummary();
    }
}

function DebitGotFocus(s, e) {
    debitOldValue = s.GetText();
    var indx = debitOldValue.indexOf(',');
    if (indx != -1) {
        debitOldValue = debitOldValue.replace(/,/g, '');
    }
}

function DebitLostFocus(s, e) {
    debitNewValue = s.GetText();
    var indx = debitNewValue.indexOf(',');

    if (indx != -1) {
        debitNewValue = debitNewValue.replace(/,/g, '');
    }
    if (debitOldValue != debitNewValue) {
        changeDebitTotalSummary();
    }
}
function recalculateDebit(oldVal) {
    if (oldVal != 0) {
        debitNewValue = 0;
        debitOldValue = oldVal;
        changeDebitTotalSummary();
    }
}

function changeDebitTotalSummary() {
    var newDif = debitOldValue - debitNewValue;
    var CurrentSum = c_txt_Debit.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    c_txt_Debit.SetValue(parseFloat(CurrentSum - newDif));
}
function CalculateSummary(grid, rowValues, visibleIndex, isDeleting) {
    //ctxtTDebit
    var originalValue = grid.batchEditApi.GetCellValue(visibleIndex, "WithDrawl");
    var newValue = rowValues[(grid.GetColumnByField("WithDrawl").index)].value;
    var dif = isDeleting ? -newValue : newValue - originalValue;
    c_txt_Debit.SetValue((parseFloat(c_txt_Debit.GetValue()) + dif).toFixed(1));
    //ctxtTCredit
    var CoriginalValue = grid.batchEditApi.GetCellValue(visibleIndex, "Receipt");
    var CnewValue = rowValues[(grid.GetColumnByField("Receipt").index)].value;
    var Cdif = isDeleting ? -CnewValue : CnewValue - CoriginalValue;
    c_txt_Credit.SetValue((parseFloat(c_txt_Credit.GetValue()) + Cdif).toFixed(1));

    var Debit = parseFloat(c_txt_Debit.GetValue());
    var Credit = parseFloat(c_txt_Credit.GetValue());

    if (Debit == 0 && Credit == 0) {
        cbtnSaveRecords.SetVisible(false);
        cbtn_SaveRecords.SetVisible(false);
    }
    else if (Debit == Credit) {
        cbtnSaveRecords.SetVisible(true);
        cbtn_SaveRecords.SetVisible(true);
    }
    else {
        cbtnSaveRecords.SetVisible(false);
        cbtn_SaveRecords.SetVisible(false);
    }
}
//End here

function OnBatchEditEndEditing(s, e) {
    //Debjyoti
    //  CalculateSummary(s, e.rowValues, e.visibleIndex, false);
    //End here
    currentEditableVisibleIndex = -1;
    var cityIDColumn = s.GetColumnByField("SubAccount1");
    if (!e.rowValues.hasOwnProperty(cityIDColumn.index))
        return;
    var cellInfo = e.rowValues[cityIDColumn.index];
    if (CityID.GetSelectedIndex() > -1 || cellInfo.text != CityID.GetText()) {
        cellInfo.value = CityID.GetValue();
        cellInfo.text = CityID.GetText();
        CityID.SetValue(null);
    }
}
function OnBatchEditRowValidating(s, e) {
    var cityIDColumn = s.GetColumnByField("CityID");
    var cellValidationInfo = e.validationInfo[cityIDColumn.index];
    if (!cellValidationInfo) return;
    var value = cellValidationInfo.value;
    if (!ASPxClientUtils.IsExists(value) || ASPxClientUtils.Trim(value) === "") {
        cellValidationInfo.isValid = false;
        cellValidationInfo.errorText = "City is required";
    }
}
function CitiesCombo_KeyDown(s, e) {
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode !== ASPxKey.Tab && keyCode !== ASPxKey.Enter) return;
    var moveActionName = e.htmlEvent.shiftKey ? "MoveFocusBackward" : "MoveFocusForward";
    if (grid.batchEditApi[moveActionName]()) {
        ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
        preventEndEditOnLostFocus = true;
    }
}
function CitiesCombo_LostFocus(s, e) {
    if (!preventEndEditOnLostFocus)
        grid.batchEditApi.EndEdit();
    preventEndEditOnLostFocus = false;
}
function AddBatchNew(s, e) {
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode === 13) {
        var mainAccountValue = (grid.GetEditor('MainAccount1').GetValue() != null) ? grid.GetEditor('MainAccount1').GetValue() : "";
        if (mainAccountValue != "") {
            grid.AddNewRow();
            grid.SetFocusedRowIndex();
        }
    }
    else if (keyCode === 9) {
        document.getElementById("txtNarration").focus();
    }

    //if (keyCode !== ASPxKey.Tab && keyCode !== ASPxKey.Enter) return;

    //grid.AddNewRow();
    //grid.SetFocusedRowIndex();

    //var moveActionName = e.htmlEvent.shiftKey ? "MoveFocusBackward" : "MoveFocusForward";
    //if (grid.batchEditApi[moveActionName]()) {
    //    ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
    //    preventEndEditOnLostFocus = true;
    //}


    <%--var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);

            if (keyCode === 13) {

                var cashReportID = (grid.GetEditor('CashReportID').GetValue() != null) ? grid.GetEditor('CashReportID').GetValue() : "";
                var mainAccountValue = (grid.GetEditor('MainAccount1').GetValue() != null) ? grid.GetEditor('MainAccount1').GetValue() : "";
                var subAccountValue = (CityID.GetValue() != null) ? CityID.GetValue() : "0";
                var withDrawlValue = (grid.GetEditor('WithDrawl').GetValue() != null) ? grid.GetEditor('WithDrawl').GetValue() : "0";
                var receiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0";
                var narrationValue = (grid.GetEditor('Narration').GetValue() != null) ? grid.GetEditor('Narration').GetValue() : "";
                var mainAccountText = (grid.GetEditor('MainAccount1').GetValue() != null) ? grid.GetEditor('MainAccount1').GetValue() : "";
                var subAccountText = (CityID.GetValue() != null) ? CityID.GetText() : "";

                var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
                document.getElementById('hdn_Brch_NonBrch').value = $("#ddlBranch").val();
                var SubAccountBranch = document.getElementById('hdn_Brch_NonBrch').value;

                document.getElementById('hdnJournalMode').value = "";
                document.getElementById('hdncashReportID').value = "";
                document.getElementById('hdnMainAccountId').value = "";
                document.getElementById('hdnSubAccountId').value = "";
                document.getElementById('hdndebit').value = "";
                document.getElementById('hdncredit').value = "";
                document.getElementById('hdnNarration').value = "";
                document.getElementById('hdnMainAccountText').value = "";
                document.getElementById('hdnSubAccountText').value = "";

                document.getElementById('hdncashReportID').value = cashReportID;
                document.getElementById('hdnMainAccountId').value = mainAccountValue;
                document.getElementById('hdnSubAccountId').value = subAccountValue;
                document.getElementById('hdndebit').value = withDrawlValue;
                document.getElementById('hdncredit').value = receiptValue;
                document.getElementById('hdnNarration').value = narrationValue;
                document.getElementById('hdnMainAccountText').value = mainAccountValue;
                document.getElementById('hdnSubAccountText').value = subAccountText;

                if (mainAccountValue != "") {
                    grid.UpdateEdit();
                    ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
                    if (cashReportID == "") {
                        grid.AddNewRow();
                    }
                    grid.SetFocusedRowIndex();
                }
            }--%>


    //GroupOwnerCombo.PerformCallback('Add^' + mainAccountValue + '^' + subAccountValue + '^' + withDrawlValue + '^' + receiptValue + '^' + narrationValue + '^' + SubAccountBranch + '^' + mainAccountText + '^' + subAccountText + '^' + cashReportID);
    //grid.UpdateEdit();
               <%-- var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
                var st = $("#ddlBranch").val();
                document.getElementById('hdn_Brch_NonBrch').value = st;
                var SubAccountBranch = document.getElementById('hdn_Brch_NonBrch').value;

                if (mainAccountValue != null) {
                    //grid.PerformCallback('Add^' + mainAccountValue + '^' + subAccountValue + '^' + withDrawlValue + '^' + receiptValue + '^' + narrationValue + '^' + SubAccountBranch + '^' + mainAccountText + '^' + subAccountText);

                    grid.UpdateEdit();
                    //grid.AddNewRow();
                    //ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);

                    grid.SetFocusedRowIndex(0);
                }
                else {
                    return false;
                }--%>

}
        function OnAddNewClick() {
            //  $('#ddlBranch').attr('Disabled', false);
            var gridcount = grid.GetVisibleRowsOnPage();
            var mainAccountValue = grid.batchEditApi.GetCellValue(0, "MainAccount1");
            if (gridcount == 0) {
                grid.AddNewRow();
            }
            else if (gridcount > 0 && mainAccountValue != "") {
                grid.AddNewRow();
            }
        }
        function WithDrawlTextChange(s, e) {
            DebitLostFocus(s, e);
            var withDrawlValue = (grid.GetEditor('WithDrawl').GetValue() != null) ? parseFloat(grid.GetEditor('WithDrawl').GetValue()) : "0";
            var receiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0";

            if (withDrawlValue > 0) {
                recalculateCredit(grid.GetEditor('Receipt').GetValue());
                grid.GetEditor('Receipt').SetValue("0");
                //grid.GetEditor('Receipt').SetEnabled(false);
            }

            var Debit = parseFloat(c_txt_Debit.GetValue());
            var Credit = parseFloat(c_txt_Credit.GetValue());

            if (Debit == 0 && Credit == 0) {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);
            }
            else if (Debit == Credit) {
                cbtnSaveRecords.SetVisible(true);
                cbtn_SaveRecords.SetVisible(true);
            }
            else {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);
            }
        }
        function ReceiptTextChange(s, e) {
            CreditLostFocus(s, e);
            var receiptValue = (grid.GetEditor('Receipt').GetValue() != null) ? grid.GetEditor('Receipt').GetValue() : "0";
            var withDrawlValue = (grid.GetEditor('WithDrawl').GetValue() != null) ? parseFloat(grid.GetEditor('WithDrawl').GetValue()) : "0";

            if (receiptValue > 0) {
                recalculateDebit(grid.GetEditor('WithDrawl').GetValue());
                grid.GetEditor('WithDrawl').SetValue("0");

                //grid.GetEditor('WithDrawl').SetEnabled(false);
            }

            var Debit = parseFloat(c_txt_Debit.GetValue());
            var Credit = parseFloat(c_txt_Credit.GetValue());

            if (Debit == 0 && Credit == 0) {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);
            }
            else if (Debit == Credit) {
                cbtnSaveRecords.SetVisible(true);
                cbtn_SaveRecords.SetVisible(true);
            }
            else {
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);
            }
        }
        function CmbScheme_ValueChange() {
            //var val = cCmbScheme.GetValue();

            var val = document.getElementById("CmbScheme").value;
            $("#MandatoryBillNo").hide();

            if (val != "0") {
                $.ajax({
                    type: "POST",
                    url: 'JournalVoucherEntry.aspx/getSchemeType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{sel_scheme_id:\"" + val + "\"}",
                    success: function (type) {
                        console.log(type);
                        if (type.d == '0') {
                            $('#<%=hdnSchemaType.ClientID %>').val('0');
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                            document.getElementById("txtBillNo").focus();
                        }
                        else if (type.d == '1') {
                            $('#<%=hdnSchemaType.ClientID %>').val('1');
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Auto";
                            tDate.Focus();
                        }
                        else if (type.d == '2') {
                            $('#<%=hdnSchemaType.ClientID %>').val('2');
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Datewise";
                        }
                    }
                });
    }
    else {
        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtBillNo.ClientID %>').value = "";
            }
        }

<%--function rblScheme_ValueChange() {
    var val = "0";
    var AspRadio = document.getElementById('<%= rblScheme.ClientID %>');
    var AspRadio_ListItem = document.getElementsByTagName('input');
    for (var i = 0; i < AspRadio_ListItem.length; i++) {
        if (AspRadio_ListItem[i].checked) {
            val = AspRadio_ListItem[i].value;
        }
    }

    $.ajax({
        type: "POST",
        url: 'JournalVoucherEntry.aspx/getSchemeType',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{sel_scheme_id:\"" + val + "\"}",
        success: function (type) {
            console.log(type);
            if (type.d == '0') {
                document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
                document.getElementById('<%= txtBillNo.ClientID %>').value = "";
            }
            else if (type.d == '1') {
                document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtBillNo.ClientID %>').value = "Auto";
            }
            else if (type.d == '2') {
                document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtBillNo.ClientID %>').value = "Datewise";
            }
        }
    });
}--%>

        function GoToNextRow() {
            var gridcount = grid.GetVisibleRowsOnPage();
            grid.batchEditApi.StartEdit(gridcount - 2, 2);
            grid.GetEditor('MainAccount1').Focus();
        }

        function deleteAllRows() {
            var frontRow = 0;
            var backRow = -1;
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
                grid.DeleteRow(frontRow);
                grid.DeleteRow(backRow);
                backRow--;
                frontRow++;
            }
            grid.AddNewRow();

            c_txt_Credit.SetValue(0);
            c_txt_Debit.SetValue(0);
            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(false);
        }

        var oldBranchdata;
        function BranchGotFocus() {
            oldBranchdata = document.getElementById('ddlBranch').value;
        }

        function ddlBranch_ChangeIndex() {
            if (oldBranchdata != document.getElementById('ddlBranch').value) {

                //get the first row accounting value debjyoti 
                grid.batchEditApi.StartEdit(-1, 1);
                var accountingDataMin = grid.GetEditor('MainAccount1').GetValue();
                grid.batchEditApi.EndEdit();

                grid.batchEditApi.StartEdit(0, 1);
                var accountingDataplus = grid.GetEditor('MainAccount1').GetValue();
                grid.batchEditApi.EndEdit();



                if (accountingDataMin != null || accountingDataplus != null) {
                    jConfirm('You have changed Branch. All the entries of ledger in this voucher to be reset to blank. \n You have to select and re-enter. Continue?', 'Confirmation Dialog', function (r) {

                        if (r == true) {
                            deleteAllRows();
                            CountryID.PerformCallback(document.getElementById('ddlBranch').value);
                            if (grid.GetVisibleRowsOnPage() == 1) {
                                grid.batchEditApi.StartEdit(-1, 1);
                            }
                        } else {
                            document.getElementById('ddlBranch').value = oldBranchdata;
                        }
                    });
                }
                else {
                    CountryID.PerformCallback(document.getElementById('ddlBranch').value);
                }
            }
        }
    </script>
    <script type="text/javascript">
        function OnKeyDown(s, e) {
            if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
                return ASPxClientUtils.PreventEvent(e.htmlEvent);
        }
        function txtBillNo_TextChanged() {
            var VoucherNo = document.getElementById("txtBillNo").value;
            var type = $('#<%=hdnMode.ClientID %>').val();

            $.ajax({
                type: "POST",
                url: "JournalVoucherEntry.aspx/CheckUniqueName",
                data: JSON.stringify({ VoucherNo: VoucherNo, Type: type }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        $("#MandatoryBillNo").show();
                        document.getElementById("txtBillNo").value = '';
                        document.getElementById("txtBillNo").focus();
                    }
                    else {
                        $("#MandatoryBillNo").hide();
                    }
                }
            });
        }

        $(document).ready(function () {

            $('#ddlBranch').blur(function () {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 1);
                }
            })
        });
    </script>
    <style>
        .dxgv {
            display: none;
        }
            /*#grid_DXMainTable tr td:last-child {
            display: table-cell !important;
            display: none;
        }
        #grid_DXMainTable tr td:last-child {
            display: none !important;
        }*/
            .dxgv.dx-al, .dxgv.dx-ar, .dx-nowrap.dxgv, .gridcellleft.dxgv, .dxgv.dx-ac, .dxgvCommandColumn_PlasticBlue.dxgv.dx-ac {
                display: table-cell !important;
            }

        #grid_DXMainTable tr td:first-child {
            display: table-cell !important;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        #GvJvSearch_DXMainTable .dxgv {
            display: table-cell !important;
        }

        #GvJvSearch_DXFilterRow .dxgv {
            display: table-cell !important;
        }

        .pullleftClass {
            position: absolute;
            right: 10px;
            top: 32px;
        }
        /*#grid_DXStatus span>a {
            display:none;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="clearfix"><span class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Journal Voucher"></asp:Label></span>
                <div style="display: none;">
                    <span style="margin-top: -23px;"><span style="float: left; padding: 0 5px">(</span>
                        <dxe:ASPxCallbackPanel ID="CbpChoosenCurrency" runat="server" ClientInstanceName="cCbpChoosenCurrency" BackColor="White" OnCallback="CbpChoosenCurrency_Callback">
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <b title="Switch To Active Currency" id="B_ChoosenCurrency" runat="server" style="text-decoration: underline; width: 100%; font-style: italic; color: Blue; float: left;"></b>
                                </dxe:PanelContent>
                            </PanelCollection>
                            <ClientSideEvents EndCallback="function(s, e) {
	                                CbpChoosenCurrency_EndCallBack(); }" />
                        </dxe:ASPxCallbackPanel>
                        <span style="padding: 0 10px">)</span>
                    </span>
                </div>
            </h3>
            <div id="btncross" class="crossBtn" style="display: none; margin-left: 50px;"><a href="JournalVoucherEntry.aspx"><i class="fa fa-times"></i></a></div>
        </div>

    </div>


    <div class="form_main">
        <div id="divAddNew" style="display: none;">
            <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;">
                <%-- <div class="col-md-1">
                <label style="">Mode</label>
                <div>
                    <dxe:ASPxComboBox ID="ComboMode" runat="server" ClientInstanceName="cComboMode" Font-Size="12px" SelectedIndex="0"
                        ValueType="System.String" Width="100%" meta:resourcekey="ComboModeResource1">
                        <Items>
                            <dxe:ListEditItem Value="0" Text="Entry" meta:resourcekey="ListEditItemResource1"></dxe:ListEditItem>
                            <dxe:ListEditItem Value="1" Text="Edit" meta:resourcekey="ListEditItemResource2"></dxe:ListEditItem>
                        </Items>
                        <ClientSideEvents SelectedIndexChanged="OnComboModeSelectedIndexChanged" />
                    </dxe:ASPxComboBox>
                </div>
            </div>--%>
                <div class="col-md-3" id="div_Edit">
                    <label>Select Numbering Scheme</label>
                    <div>
                        <%-- <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme" DataSourceID="SqlSchematype"
                            TextField="SchemaName" ValueField="ID" TabIndex="1" SelectedIndex="0"
                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                            <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}"></ClientSideEvents>
                        </dxe:ASPxComboBox>--%>
                        <asp:DropDownList ID="CmbScheme" runat="server" DataSourceID="SqlSchematype" TabIndex="1"
                            DataTextField="SchemaName" DataValueField="ID" Width="100%"
                            onchange="CmbScheme_ValueChange()">
                        </asp:DropDownList>
                        <%-- <asp:RadioButtonList ID="rblScheme" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100%"
                            onclick="rblScheme_ValueChange()">
                        </asp:RadioButtonList>--%>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Journal No.</label>
                    <div>
                        <asp:TextBox ID="txtBillNo" runat="server" Width="95%" meta:resourcekey="txtBillNoResource1" TabIndex="2" MaxLength="30" onchange="txtBillNo_TextChanged()"></asp:TextBox>
                        <span id="MandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>
                <div class="col-md-3">
                    <label style="">Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="tDate" TabIndex="3"
                            UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                            <ClientSideEvents DateChanged="function(s,e){DateChange()}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Branch</label>
                    <div>
                        <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" TabIndex="4"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%"
                            meta:resourcekey="ddlBranchResource1" onchange="ddlBranch_ChangeIndex()" onfocus="BranchGotFocus()">
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="display: none;">
                    <div class="col-md-1">
                        <label>Prefix</label>
                        <div>
                            <dxe:ASPxTextBox ID="txtAccountCode" ClientInstanceName="txtAccountCode"
                                runat="server" Width="100%" MaxLength="2" meta:resourcekey="txtAccountCodeResource1">
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                    <RequiredField IsRequired="True" ErrorText="Select Account Code"></RequiredField>
                                </ValidationSettings>
                                <ClientSideEvents KeyPress="function(s,e){window.setTimeout('updateEditorText()', 10);}" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>

                    <div class="col-md-2" style="" id="tdAcBal">
                        <label>A/C Balance :</label>
                        <div>
                            <dxe:ASPxCallbackPanel ID="CbpAcBalance" runat="server" ClientInstanceName="cCbpAcBalance"
                                OnCallback="CbpAcBalance_Callback" BackColor="White">
                                <PanelCollection>
                                    <dxe:PanelContent runat="server">
                                        <div style="width: 100%; text-align: right;">
                                            <b style="text-align: right" id="B_AcBalance" runat="server"></b>
                                        </div>
                                    </dxe:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="function(s, e) {
	                                        CbpAcBalance_EndCallBack(); }" />
                            </dxe:ASPxCallbackPanel>
                            <blink><b style="color:Blue;font-size:10px;" id="bDrCrStatus" runat="server"></b></blink>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
                <div style="display: none;">
                    <div class="col-md-3">
                        <label>Main Account</label>
                        <div>
                            <asp:ListBox ID="lstMainAccount" runat="server" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsn" data-placeholder="Select ..."></asp:ListBox>

                        </div>
                    </div>
                    <div class="col-md-3">
                        <label id="tdlblSubAccount">SubAccount</label>
                        <div id="tdSubAccountValue">
                            <asp:ListBox ID="lstSubAccount" runat="server" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsn" data-placeholder="Select ..."></asp:ListBox>

                        </div>

                    </div>
                    <div class="col-md-3">
                        <td style="font-weight: bold; text-align: left; text-decoration: underline;">Debit</td>
                        <div>
                            <dxe:ASPxTextBox ID="txtdebit" runat="server" Width="100%" ClientInstanceName="ctxtdebit" HorizontalAlign="Right" meta:resourcekey="txtdebitResource1">
                                <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                <ClientSideEvents KeyUp="function(s,e){focusval(s.GetValue());}" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <td style="font-weight: bold; text-align: left; text-decoration: underline;">Credit</td>
                        <div>
                            <dxe:ASPxTextBox ID="txtcredit" runat="server" Width="100%" ClientInstanceName="ctxtcredit" HorizontalAlign="Right" meta:resourcekey="txtcreditResource1">
                                <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                <ClientSideEvents KeyUp="function(s,e){focusval1(s.GetValue());}" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div style="clear: both"></div>
                    <div class="col-md-6">
                        <label>Line Narration</label>
                        <div>
                            <asp:TextBox ID="txtNarration1" Font-Names="Arial" runat="server" TextMode="MultiLine"
                                Width="100%" meta:resourcekey="txtNarration1Resource1" Height="29px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">

                        <div style="padding-top: 20px;">

                            <span id="tdEntryButton">
                                <dxe:ASPxButton ID="abtnEntry" runat="server" AccessKey="E" AutoPostBack="False" Text="Entry" CssClass="btn btn-primary" meta:resourcekey="abtnEntryResource1">
                                    <%-- <ClientSideEvents Click="function(s, e) {EntryButtonClick();}" />--%>
                                </dxe:ASPxButton>
                            </span>
                        </div>
                    </div>
                    <div style="clear: both"></div>


                    <%--<div style="clear:both"></div>--%>
                    <div class="col-md-2">


                        <span style="display: none;" id="tdSearchButton">
                            <dxe:ASPxButton ID="abtnSearch" runat="server" AutoPostBack="False" Text="Search" CssClass="btn btn-primary" meta:resourcekey="abtnSearchResource1">
                                <ClientSideEvents Click="function(s, e) {SearchButtonClick();}" />
                            </dxe:ASPxButton>
                        </span>
                    </div>
                    <div style="clear: both"></div>
                </div>

            </div>
            <div>

                <div style="display: none;">
                    <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                </div>
                <div>
                    <br />
                </div>
                <%-- SettingsBehavior-AllowFocusedRow="true"  EnableRowsCache="False" --%>
                <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="CashReportID" ClientInstanceName="grid" ID="grid"
                    Width="100%" OnCellEditorInitialize="grid_CellEditorInitialize" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                    Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="220">
                    <SettingsPager Visible="false"></SettingsPager>
                    <Columns>
                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="true" Width="50" VisibleIndex="0" Caption="Action">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>
                        <dxe:GridViewDataComboBoxColumn Caption="Main Account" FieldName="MainAccount1" VisibleIndex="1" Width="220">
                            <PropertiesComboBox ValueField="CountryID" ClientInstanceName="CountryID" TextField="CountryName">
                                <%-- <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>--%>
                                <ClientSideEvents SelectedIndexChanged="CountriesCombo_SelectedIndexChanged" />
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataComboBoxColumn FieldName="SubAccount1" Caption="Sub Account" VisibleIndex="2" Width="220">
                            <PropertiesComboBox TextField="CityName" ValueField="CityID">
                            </PropertiesComboBox>
                            <EditItemTemplate>
                                <dxe:ASPxComboBox runat="server" Width="100%" OnInit="CityCmb_Init" EnableIncrementalFiltering="true" TextField="CityName" ClearButton-DisplayMode="Always"
                                    OnCallback="CityCmb_Callback" EnableCallbackMode="true" ValueField="CityID" ID="CityCmb" ClientInstanceName="CityID">
                                    <ClientSideEvents EndCallback="CitiesCombo_EndCallback" />
                                    <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                </dxe:ASPxComboBox>
                            </EditItemTemplate>
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="WithDrawl" Caption="Debit" meta:resourcekey="GridViewDataTextColumnResource3" Width="200">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                <ClientSideEvents KeyDown="OnKeyDown" LostFocus="WithDrawlTextChange"
                                    GotFocus="function(s,e){
                                    DebitGotFocus(s,e);
                                    }" />
                                <ClientSideEvents />
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>

                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Receipt" Caption="Credit" Width="200">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                <ClientSideEvents KeyDown="OnKeyDown" LostFocus="ReceiptTextChange"
                                    GotFocus="function(s,e){
                                    CreditGotFocus(s,e);
                                    }" />
                                <ClientSideEvents />
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Narration" Caption="Narration" Width="200">
                            <PropertiesTextEdit>
                                <ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Srl No" ReadOnly="true" HeaderStyle-CssClass="hide" Width="10">
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <TotalSummary>
                        <dxe:ASPxSummaryItem SummaryType="Sum" FieldName="C2" Tag="C2_Sum" />
                    </TotalSummary>
                    <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing"
                        CustomButtonClick="OnCustomButtonClick" />
                    <SettingsDataSecurity AllowEdit="true" />
                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                        <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                    </SettingsEditing>
                </dxe:ASPxGridView>
            </div>
            <div class="text-center">
                <table style="margin-left: 368px; margin-top: 5px;">
                    <tr>
                        <td style="padding-right: 50px">Total Amount</td>
                        <td>
                            <dxe:ASPxTextBox ID="txt_Debit" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                <MaskSettings Mask="<0..999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td>
                            <dxe:ASPxTextBox ID="txt_Credit" runat="server" Width="105px" ClientInstanceName="c_txt_Credit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                <MaskSettings Mask="<0..999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                            </dxe:ASPxTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <br />
            </div>
            <div style="display: none;">
                <table width="100%" border="0">
                    <tr id="Tdebit" runat="server">
                        <td style="width: 13017px; text-align: left; height: 14px;" valign="top"></td>
                        <td style="width: 75px; text-align: left; height: 14px;" valign="top">Total&nbsp;
                        </td>
                        <td style="width: 79px; text-align: left; height: 14px;" valign="top">
                            <dxe:ASPxTextBox ID="txtTDebit" runat="server" Width="105px" ClientInstanceName="ctxtTDebit" HorizontalAlign="Right" Font-Size="12px" meta:resourcekey="txtTDebitResource1">
                                <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="width: 75px; height: 14px; text-align: left;" valign="top">Dr.</td>
                        <td style="text-align: left; height: 14px;" valign="top">
                            <dxe:ASPxTextBox ID="txtTCredit" runat="server" Width="105px" ClientInstanceName="ctxtTCredit" HorizontalAlign="Right" Font-Size="12px" meta:resourcekey="txtTCreditResource1">
                                <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                            </dxe:ASPxTextBox>
                        </td>
                        <td style="height: 14px; text-align: left; width: 75px;" valign="top">Cr.</td>
                    </tr>
                </table>
            </div>
            <div class="clearfix" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                <div class="col-md-9">
                    <label>Main Narration</label>
                    <div>
                        <asp:TextBox ID="txtNarration" Font-Names="Arial" runat="server" TextMode="MultiLine"
                            Width="100%" onkeyup="OnlyNarration(this,'Narration',event)" meta:resourcekey="txtNarrationResource1" Height="40px"></asp:TextBox>
                    </div>
                </div>
            </div>

            <table style="float: left;">
                <tr>
                    <td style="width: 100px;" id="tdSaveButton" runat="Server">
                        <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AccessKey="S" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                        </dxe:ASPxButton>
                    </td>
                    <td style="width: 100px;" id="td_SaveButton" runat="Server">
                        <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>
                    </td>
                    <td style="width: 100px">
                        <dxe:ASPxButton ID="btnDiscardEntry" runat="server" AccessKey="D" AllowFocus="False" Visible="false"
                            AutoPostBack="False" Text="D&#818;iscard Entered Records" CssClass="btn btn-primary" meta:resourcekey="btnDiscardEntryResource1">
                            <ClientSideEvents Click="function(s, e) {DiscardButtonClick();}" />
                        </dxe:ASPxButton>
                    </td>
                    <td id="tdadd" style="width: 100px">
                        <dxe:ASPxButton ID="btnadd" ClientInstanceName="cbtnadd" runat="server" AccessKey="L" AutoPostBack="False" ClientVisible="false"
                            Text="Add Entry To L&#818;ist" CssClass="btn btn-primary" meta:resourcekey="btnaddResource1" Visible="false">
                            <ClientSideEvents Click="function(s, e) {SubAccountCheck();}" />
                        </dxe:ASPxButton>
                    </td>
                    <td id="tdnew" style="width: 100px; height: 16px; display: none">
                        <dxe:ASPxButton ID="btnnew" ClientInstanceName="cbtnnew" runat="server" AutoPostBack="False" Text="N&#818;ew Entry" ClientVisible="false"
                            CssClass="btn btn-primary" AccessKey="N" Font-Bold="False" Font-Underline="False" BackColor="Tan" meta:resourcekey="btnnewResource1">
                            <ClientSideEvents Click="function(s, e) {NewButtonClick();}" />
                        </dxe:ASPxButton>
                    </td>
                    <td style="width: 100px">
                        <dxe:ASPxButton ID="btnCancelEntry" runat="server" AccessKey="C" AutoPostBack="False" Text="C&#818;ancel Entry" CssClass="btn btn-primary" meta:resourcekey="btnCancelEntryResource1" ClientVisible="false">
                            <ClientSideEvents Click="function(s, e) {CancelButtonClick();}" />
                        </dxe:ASPxButton>
                    </td>

                    <td style="width: 100px">
                        <dxe:ASPxButton ID="btnUnsaveData" runat="server" AccessKey="R" AutoPostBack="False" Text="R&#818;efresh" CssClass="btn btn-primary" meta:resourcekey="btnUnsaveDataResource1" ClientVisible="false">
                            <ClientSideEvents Click="function(s, e) {RefreshButtonClick();}" />
                        </dxe:ASPxButton>
                    </td>

                </tr>
            </table>
        </div>

        <%--<div  style="background: #f5f4f3; padding: 22px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;display:none;"  id="PanelMainAccount">
           
        </div>--%>

        <table class="TableMain100" style="width: 100%">
            <tr>
                <td>
                    <div style="display: none">
                        <table style="width: 100%; display: none" id="TblMainEntryForm" border="0">

                            <tr>
                                <td style="height: 12px" colspan="4" valign="top">


                                    <div class="pull-left">
                                        <% if (rights.CanExport)
                                               { %>
                                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">Export to</asp:ListItem>
                                            <asp:ListItem Value="1">PDF</asp:ListItem>
                                            <asp:ListItem Value="2">XLS</asp:ListItem>
                                            <asp:ListItem Value="3">RTF</asp:ListItem>
                                            <asp:ListItem Value="4">CSV</asp:ListItem>
                                        </asp:DropDownList>
                                        <% } %>
                                    </div>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="height: 16px" valign="top">

                                    <dxe:ASPxGridView ID="DetailsGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cDetailsGrid"
                                        KeyFieldName="CashReportID" Width="100%" OnCustomCallback="DetailsGrid_CustomCallback"
                                        OnCustomJSProperties="DetailsGrid_CustomJSProperties" OnHtmlRowCreated="DetailsGrid_HtmlRowCreated"
                                        OnRowDeleting="DetailsGrid_RowDeleting" OnHtmlEditFormCreated="DetailsGrid_HtmlEditFormCreated" OnRowUpdating="DetailsGrid_RowUpdating" OnCancelRowEditing="DetailsGrid_CancelRowEditing"
                                        OnHtmlRowPrepared="DetailsGrid_HtmlRowPrepared" Font-Size="12px" meta:resourcekey="DetailsGridResource1" OnCommandButtonInitialize="DetailsGrid_CommandButtonInitialize">
                                        <ClientSideEvents EndCallback="function(s, e) {SetDebitCreditValue(s.cpTotalDebitCredit);}"></ClientSideEvents>
                                        <Templates>
                                            <EditForm>
                                                <div class="col-md-12">
                                                    <div class="col-md-3">
                                                        <label style="font-weight: bold;">Main Account</label>
                                                        <div>
                                                            <%--<asp:TextBox runat="server" Width="100%"  Text='<%# Bind("MainAccount1") %>' ID="txtMainAccountE" onkeyup="CallMainAccountE(this,'MainAccountJournalE',event)" meta:resourcekey="txtMainAccountEResource1"></asp:TextBox>--%>
                                                            <dxe:ASPxComboBox runat="server" ID="CmbMainAccount" ValueType="System.String" ClientInstanceName="cCmbMainAccount" Font-Size="12px" Width="253px" OnCallback="CmbMainAccount_Callback">
                                                                <ClientSideEvents SelectedIndexChanged="OnCmbMainAccountSelectedIndexChanged" />
                                                            </dxe:ASPxComboBox>
                                                            <asp:HiddenField runat="server" ID="txtMainAccountE_hidden"></asp:HiddenField>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label id="tdlblSubAccount" style="font-weight: bold;">Sub Account</label>
                                                        <div id="tdSubAccountE">
                                                            <dxe:ASPxComboBox runat="server" ID="CmbSubAccount" ValueType="System.String" ClientInstanceName="cCmbSubAccount" OnCallback="CmbSubAccount_Callback" Font-Size="12px" Width="253px">
                                                            </dxe:ASPxComboBox>
                                                            <%-- <asp:TextBox runat="server" Width="100%" Text='<%# Bind("SubAccount1") %>' ID="txtSubAccountE" onkeyup="CallSubAccountE(this,'SubAccountModE',event)" meta:resourcekey="txtSubAccountEResource1"></asp:TextBox>--%>

                                                            <asp:HiddenField runat="server" ID="txtSubAccountE_hidden"></asp:HiddenField>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label style="font-weight: bold;">Debit</label>
                                                        <div>
                                                            <dxe:ASPxTextBox runat="server" Text='<%# Bind("WithDrawl") %>' Width="100%" ID="txtdebit" ClientInstanceName="ctxtdebitE" HorizontalAlign="Right" meta:resourcekey="txtdebitResource2">
                                                                <MaskSettings IncludeLiterals="DecimalSymbol" Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;"></MaskSettings>

                                                                <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>

                                                                <ClientSideEvents LostFocus="function(s,e){Efocusval(s.GetValue());}"></ClientSideEvents>
                                                            </dxe:ASPxTextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label style="font-weight: bold;">Credit</label>
                                                        <div>
                                                            <dxe:ASPxTextBox runat="server" Text='<%# Bind("Receipt") %>' Width="100%" ID="txtcredit" ClientInstanceName="ctxtcreditE" HorizontalAlign="Right" meta:resourcekey="txtcreditResource2">
                                                                <MaskSettings IncludeLiterals="DecimalSymbol" Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;"></MaskSettings>

                                                                <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>

                                                                <ClientSideEvents LostFocus="function(s,e){Efocusval1(s.GetValue());}"></ClientSideEvents>
                                                            </dxe:ASPxTextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label style="font-weight: bold;">Line Narration</label>
                                                        <div>
                                                            <asp:TextBox runat="server" Width="100%" Text='<%# Bind("SubNarration") %>' ID="txtNarration1" TextMode="MultiLine" meta:resourcekey="txtNarration1Resource2"></asp:TextBox>
                                                        </div>

                                                    </div>
                                                    <div style="clear: both">
                                                    </div>
                                                    <div class="col-md-12">
                                                        <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormUpdateButton" ColumnID="" ID="UpdateButton"></dxe:ASPxGridViewTemplateReplacement>
                                                        <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormCancelButton" ColumnID="" ID="CancelButton"></dxe:ASPxGridViewTemplateReplacement>
                                                    </div>
                                                </div>
                                                <table>
                                                </table>

                                            </EditForm>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True"></SettingsBehavior>
                                        <Styles>
                                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

                                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>

                                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>

                                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>

                                            <Footer CssClass="gridfooter"></Footer>
                                        </Styles>
                                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" Mode="ShowAllRecords">
                                            <FirstPageButton Visible="True"></FirstPageButton>

                                            <LastPageButton Visible="True"></LastPageButton>
                                        </SettingsPager>
                                        <SettingsSearchPanel Visible="True" />
                                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                        <Columns>
                                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="MainAccount1" Width="300px" Caption="Main Account" meta:resourcekey="GridViewDataTextColumnResource1">
                                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SubAccount1" Width="200px" Caption="Sub Account" meta:resourcekey="GridViewDataTextColumnResource2">
                                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="WithDrawl" Width="200px" Caption="Debit" meta:resourcekey="GridViewDataTextColumnResource3">
                                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Receipt" Width="200px" Caption="Credit" meta:resourcekey="GridViewDataTextColumnResource4">
                                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Visible="False" FieldName="CashReportID" meta:resourcekey="GridViewDataTextColumnResource5"></dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Visible="False" FieldName="SubNarration" meta:resourcekey="GridViewDataTextColumnResource6"></dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn VisibleIndex="4" Width="15%" meta:resourcekey="GridViewCommandColumnResource1" ShowDeleteButton="true" ShowEditButton="true" Caption="Actions" HeaderStyle-HorizontalAlign="Center">
                                                <%--<DeleteButton Visible="True"></DeleteButton>--%>

                                                <CellStyle ForeColor="White">
                                                    <%--<HoverStyle BackColor="#000040"></HoverStyle>--%>
                                                </CellStyle>

                                                <%--<EditButton Visible="True"></EditButton>--%>
                                            </dxe:GridViewCommandColumn>
                                        </Columns>

                                        <SettingsCommandButton>

                                            <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                <Image AlternateText="Edit" Url="/assests/images/Edit.png"></Image>

                                                <Styles>
                                                    <Style CssClass="pad"></Style>
                                                </Styles>
                                            </EditButton>

                                            <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                <Image AlternateText="Delete" Url="/assests/images/Delete.png"></Image>
                                            </DeleteButton>
                                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">


                                                <Styles>
                                                    <Style CssClass="btn btn-danger"></Style>
                                                </Styles>


                                            </CancelButton>

                                        </SettingsCommandButton>
                                        <SettingsEditing Mode="EditForm" />
                                        <%-- <Settings VerticalScrollableHeight="300" ShowTitlePanel="True" ShowVerticalScrollBar="True" />--%>
                                    </dxe:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table style="width: 100%;" id="TblSearch">
                        <tr>
                            <td>
                                <div class="">
                                    <div class="clearfix">
                                        <div style="float: left; padding-right: 5px;">
                                            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a>
                                            <%--<asp:Button ID="btnJournalAdd" runat="server" Text="Add New" CssClass="btn btn-primary" OnClick="btnJournalAdd_Click" />--%>
                                            <asp:DropDownList ID="drdExport1" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport1_SelectedIndexChanged"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                                    ClientInstanceName="cGvJvSearch" KeyFieldName="JvID" Width="100%" OnCustomCallback="GvJvSearch_CustomCallback"
                                    OnCustomJSProperties="GvJvSearch_CustomJSProperties" meta:resourcekey="GvJvSearchResource1">
                                    <ClientSideEvents CustomButtonClick="CustomButtonClick" EndCallback="function(s, e) {GvJvSearch_EndCallBack();}" />
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <Styles>
                                        <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                                        <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                                        <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                                        <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                                        <Footer CssClass="gridfooter"></Footer>
                                    </Styles>
                                    <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" AlwaysShowPager="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="JvID" Caption="Main Account" meta:resourcekey="GridViewDataTextColumnResource7">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="TransactionDate" Caption="Date" meta:resourcekey="GridViewDataTextColumnResource7">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="BillNumber" Caption="Journal No." meta:resourcekey="GridViewDataTextColumnResource7">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="VoucherNumber" Caption="JV Number" meta:resourcekey="GridViewDataTextColumnResource8" Visible="false">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="BranchNameCode" Width="20%" Caption="Branch" meta:resourcekey="GridViewDataTextColumnResource9" Visible="false">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Narration" Caption="Narration" meta:resourcekey="GridViewDataTextColumnResource10">
                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="JournalVoucher_CreateUser" Caption="Entered By" meta:resourcekey="GridViewDataTextColumnResource9">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="JournalVoucher_ModifyDateTime" Caption="Last Update On" meta:resourcekey="GridViewDataTextColumnResource9">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="JournalVoucher_ModifyUser" Caption="Updated By" meta:resourcekey="GridViewDataTextColumnResource9">
                                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Visible="False" FieldName="IBRef" meta:resourcekey="GridViewDataTextColumnResource11"></dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Visible="False" FieldName="WhichTypeItem" meta:resourcekey="GridViewDataTextColumnResource12"></dxe:GridViewDataTextColumn>
                                        <dxe:GridViewCommandColumn VisibleIndex="8" Width="130px" meta:resourcekey="GridViewCommandColumnResource2" ButtonType="Image" Caption="Actions">
                                            <CustomButtons>
                                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" meta:resourcekey="GridViewCommandColumnCustomButtonResource1" Image-ToolTip="Edit" Styles-Style-CssClass="pad">
                                                    <Image Url="/assests/images/Edit.png"></Image>
                                                </dxe:GridViewCommandColumnCustomButton>
                                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" meta:resourcekey="GridViewCommandColumnCustomButtonResource2" Image-ToolTip="Delete" Styles-Style-CssClass="pad">
                                                    <Image Url="/assests/images/Delete.png"></Image>
                                                </dxe:GridViewCommandColumnCustomButton>

                                                <%--Print Journal Voucher--%>
                                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnPrint" meta:resourcekey="GridViewCommandColumnCustomButtonResource3" Image-ToolTip="Print" Styles-Style-CssClass="pad">
                                                    <Image Url="/assests/images/Print.png"></Image>
                                                </dxe:GridViewCommandColumnCustomButton>
                                                <%--End Print Journal Voucher--%>
                                            </CustomButtons>

                                        </dxe:GridViewCommandColumn>
                                    </Columns>
                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsSearchPanel Visible="True" />
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id='Div1' style="height: 20px; display: none">
                    <div style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50%; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                        <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td height='25' align='center' bgcolor='#FFFFFF'>
                                                <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                            <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>

        </table>

        <dxe:ASPxPopupControl ID="SearchPopUp" runat="server" HeaderText="Filter" ClientInstanceName="cSearchPopUp" meta:resourcekey="SearchPopUpResource1" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server" meta:resourcekey="PopupControlContentControlResource1">
                    <table class="style1">
                        <tr>
                            <td style="width: 3px">
                                <dxe:ASPxCheckBox ID="ChkTranDate" runat="server" Text="Tran.Date"
                                    ClientInstanceName="cChkTranDate" Checked="True" ReadOnly="True" meta:resourcekey="ChkTranDateResource1">
                                </dxe:ASPxCheckBox>
                            </td>
                            <td>
                                <dxe:ASPxCheckBox ID="ChkBranch" runat="server" Text="Branch"
                                    ClientInstanceName="cChkBranch" meta:resourcekey="ChkBranchResource1">
                                </dxe:ASPxCheckBox>
                            </td>
                            <td>
                                <dxe:ASPxCheckBox ID="ChkBillNo" runat="server" Text="BillNo"
                                    ClientInstanceName="cChkBillNo" meta:resourcekey="ChkBillNoResource1">
                                </dxe:ASPxCheckBox>
                            </td>
                            <td style="width: 3px">
                                <dxe:ASPxCheckBox ID="ChkPrefix" runat="server" Text="Prefix"
                                    ClientInstanceName="cChkPrefix" meta:resourcekey="ChkPrefixResource1">
                                </dxe:ASPxCheckBox>
                            </td>
                            <td style="width: 3px">
                                <dxe:ASPxCheckBox ID="ChkNarration" runat="server" Text="Narration"
                                    ClientInstanceName="cChkNarration" meta:resourcekey="ChkNarrationResource1">
                                </dxe:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>

                            <td colspan="5" style="text-align: center; padding-top: 10px">
                                <dxe:ASPxButton ID="btnShow" runat="server" CssClass="btn btn-primary" AutoPostBack="False" Text="Show" meta:resourcekey="btnShowResource1">
                                    <ClientSideEvents Click="function (s, e) { btnShowClick(); ExcludePopUp.Hide(); }" />
                                </dxe:ASPxButton>

                                <dxe:ASPxButton ID="btnCancel" runat="server" CssClass="btn btn-danger" AutoPostBack="False" Text="Cancel" Width="62px" meta:resourcekey="btnCancelResource1">
                                    <ClientSideEvents Click="function (s, e) { cSearchPopUp.Hide(); }" />
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <dxe:ASPxPopupControl ID="MsgPopUp" runat="server" HeaderText="Notice" ClientInstanceName="cMsgPopUp" Width="250px" Left="200" ShowSizeGrip="False" Top="200"
            meta:resourcekey="MsgPopUpResource1" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server" meta:resourcekey="PopupControlContentControlResource2">
                    <table width="100%">
                        <tr>
                            <td>&nbsp;Do You Want To Edit This Entry?</td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td align="right" valign="top">
                                            <dxe:ASPxButton ID="abtnOk" runat="server" CssClass="btn btn-primary" AutoPostBack="False" Text="Ok" meta:resourcekey="abtnOkResource1">
                                                <ClientSideEvents Click="function (s, e) { btnOkClick(); cMsgPopUp.Hide(); }" />
                                            </dxe:ASPxButton>
                                        </td>
                                        <td widyh="10px">&nbsp;
                                        </td>
                                        <td align="left" valign="top">
                                            <dxe:ASPxButton ID="abtnCancel" runat="server" CssClass="btn btn-primary" AutoPostBack="False" Text="Cancel" meta:resourcekey="abtnCancelResource1">
                                                <ClientSideEvents Click="function (s, e) { cMsgPopUp.Hide(); }" />
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>
                    </table>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="DeleteMsgPopUp" runat="server" HeaderText="Notice" ClientInstanceName="cDeleteMsgPopUp" Width="387px" Left="200" ShowSizeGrip="False" Top="200" meta:resourcekey="DeleteMsgPopUpResource1" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server" meta:resourcekey="PopupControlContentControlResource3">
                    <table class="style1">
                        <tr>
                            <td colspan="5">Are u Sure? Do You Want To Delete This JV?</td>
                        </tr>
                        <tr>
                            <td style="width: 3px"></td>
                            <td></td>
                            <td style="width: 164px"></td>
                            <td style="width: 3px">
                                <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Ok" meta:resourcekey="ASPxButton1Resource1">
                                    <ClientSideEvents Click="function (s, e) { DeletebtnOkClick(); cDeleteMsgPopUp.Hide(); }" />
                                </dxe:ASPxButton>
                            </td>
                            <td style="width: 3px">
                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Cancel" meta:resourcekey="ASPxButton2Resource1">
                                    <ClientSideEvents Click="function (s, e) { cDeleteMsgPopUp.Hide(); }" />
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <dxe:ASPxPopupControl ID="FileUsedByPopUp" runat="server" HeaderText="Notice" ClientInstanceName="cFileUsedByPopUp" Width="387px" Left="200" ShowSizeGrip="False" Top="200" meta:resourcekey="FileUsedByPopUpResource1" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server" meta:resourcekey="PopupControlContentControlResource4">
                    <table class="style1">
                        <tr>
                            <td colspan="5">This Entry Was Already Being Edited By You.</td>
                        </tr>
                        <tr>

                            <td></td>
                            <td style="width: 3px">
                                <dxe:ASPxButton ID="btnContinue" runat="server" AutoPostBack="False" Text="Continue Previous Edit" Width="140px" meta:resourcekey="btnContinueResource1">
                                    <ClientSideEvents Click="function (s, e) { btnContinueClick(); cFileUsedByPopUp.Hide(); }" />
                                </dxe:ASPxButton>
                            </td>
                            <td style="width: 3px">
                                <dxe:ASPxButton ID="btnNewEntry" runat="server" AutoPostBack="False" Text="Fresh Edit" Width="140px" meta:resourcekey="btnNewEntryResource1">
                                    <ClientSideEvents Click="function (s, e) { btnFreshEntryClick();cFileUsedByPopUp.Hide(); }" />
                                </dxe:ASPxButton>
                            </td>
                            <td style="width: 3px">
                                <dxe:ASPxButton ID="btnClose" runat="server" AutoPostBack="False" Text="Discard Previous Edit" Width="140px" meta:resourcekey="btnCloseResource1">
                                    <ClientSideEvents Click="function (s, e) {btnCloseClick(); cFileUsedByPopUp.Hide(); }" />
                                </dxe:ASPxButton>
                            </td>
                            <td style="width: 3px">
                                <dxe:ASPxButton ID="btnCancle" runat="server" AutoPostBack="False" Text="Cancel" Width="140px" meta:resourcekey="btnCancleResource1">
                                    <ClientSideEvents Click="function (s, e) { cFileUsedByPopUp.Hide(); }" />
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <asp:SqlDataSource ID="dsCompany" runat="server" 
            ConflictDetection="CompareAllValues" SelectCommand="select cmp_internalId,cmp_Name from tbl_master_company where cmp_internalId in(select exch_compId from tbl_master_companyExchange)"></asp:SqlDataSource>
        <asp:SqlDataSource ID="dsBranch" runat="server" 
            ConflictDetection="CompareAllValues" SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH"></asp:SqlDataSource>
        <%--SelectCommand="Select * From ((Select '0' as BANKBRANCH_ID , 'Select' as BANKBRANCH_NAME) Union (SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH)) as x Order By BANKBRANCH_ID asc"--%>
        <asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='1' AND IsActive=1)) as x Order By ID asc"></asp:SqlDataSource>

        <asp:HiddenField ID="hddnEdit" runat="server" />
        <asp:HiddenField ID="hdnSegmentid" runat="server" />
        <asp:HiddenField ID="hdn_SegID_SegmentName" runat="server" />

        <asp:HiddenField ID="hdnCompanyid" runat="server" />
        <asp:HiddenField ID="txtMainAccount_hidden" runat="server" />
        <asp:HiddenField ID="txtSubAccount_hidden" runat="server" />
        <asp:HiddenField ID="hdn_Brch_NonBrch" runat="server" />
        <asp:HiddenField ID="hdn_MainAcc_Type" runat="server" />


        <asp:HiddenField ID="hdncashReportID" runat="server" />
        <asp:HiddenField ID="hdnMainAccountId" runat="server" />
        <asp:HiddenField ID="hdnMainAccountText" runat="server" />
        <asp:HiddenField ID="hdnSubAccountId" runat="server" />
        <asp:HiddenField ID="hdnSubAccountText" runat="server" />
        <asp:HiddenField ID="hdndebit" runat="server" />
        <asp:HiddenField ID="hdncredit" runat="server" />
        <asp:HiddenField ID="hdnNarration" runat="server" />
        <asp:HiddenField ID="hdnMode" runat="server" />
        <asp:HiddenField ID="hdnJNMode" runat="server" />
        <asp:HiddenField ID="hdnJournalMode" runat="server" />
        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="hdnJournalCode" runat="server" />
        <asp:HiddenField ID="hdnSchemaType" runat="server" />
        <div style="display: none">
            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" meta:resourcekey="btnPrintResource1" />
            <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>
        </div>
    </div>
</asp:Content>
