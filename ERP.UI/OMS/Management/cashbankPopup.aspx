<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.management_cashbankPopup" CodeBehind="cashbankPopup.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>
    

    <script type="text/javascript">
        $(document).ready(function () {

            $(".water").each(function () {
                if ($(this).val() == this.title) {
                    $(this).addClass("opaque");
                }
            });

            $(".water").focus(function () {
                if ($(this).val() == this.title) {
                    $(this).val("");
                    $(this).removeClass("opaque");
                }
            });

            $(".water").blur(function () {
                if ($.trim($(this).val()) == "") {
                    $(this).val(this.title);
                    $(this).addClass("opaque");
                }
                else {
                    $(this).removeClass("opaque");
                }
            });
        });

    </script>

    <style type="text/css">
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

        /*form{
		display:inline;
	}*/
    </style>

    <script language="javascript" type="text/javascript">
        var isCtrl = false;
        document.onkeyup = function (e) {
            if (event.keyCode == 17) {
                isCtrl = false;
            }
            if (event.keyCode == 27) {
                btnCancel_Click();
            }
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 17) isCtrl = true;
            if (event.keyCode == 83 && isCtrl == true) {
                //run code for CTRL+S -- ie, save!
                document.getElementById('btnInsert').click();
                return false;
            }
        }
        function CallList(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function Page_Load() {
            document.getElementById("TrInsert").style.display = 'none';
            document.getElementById("tdLine1").style.display = 'none';
            document.getElementById("tdauth").style.display = 'none';
            document.getElementById("tdauth1").style.display = 'none';
        }
        function Page_Load1() {
            document.getElementById("TrInsert").style.display = 'inline';
            document.getElementById("tdLine1").style.display = 'none';
            document.getElementById("tdauth").style.display = 'none';
            document.getElementById("tdauth1").style.display = 'none';
        }
        function TypeSelect1() {
            var obj = document.getElementById("cmbType").value;
            if (obj == "C") {
                document.getElementById("tdcb").style.display = 'none';
                document.getElementById("tdcb1").style.display = 'none';
                document.getElementById("trIssuingBank").style.display = 'none';
                document.getElementById("trDetails").style.display = 'none';
                document.getElementById("trCustomerBank").style.display = 'none';
                document.getElementById("tdLine1").style.display = 'none';
                document.getElementById("tdauth").style.display = 'none';
                document.getElementById("tdauth1").style.display = 'none';
            }
            else {
                if (obj == "P") {
                    document.getElementById("trIssuingBank").style.display = 'none';
                    document.getElementById("trDetails").style.display = 'none';
                    document.getElementById("trCustomerBank").style.display = 'none';
                    document.getElementById("tdLine1").style.display = 'none';
                    document.getElementById("tdauth").style.display = 'none';
                    document.getElementById("tdauth1").style.display = 'none';
                }
                else {
                    document.getElementById("trIssuingBank").style.display = 'none';
                    document.getElementById("trDetails").style.display = 'none';
                    document.getElementById("trCustomerBank").style.display = 'none';
                    document.getElementById("tdLine1").style.display = 'none';
                    document.getElementById("tdauth").style.display = 'none';
                    document.getElementById("tdauth1").style.display = 'none';
                }
                document.getElementById("tdcb").style.display = 'inline';
                document.getElementById("tdcb1").style.display = 'inline';
            }

        }
        function CallListBank(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function OnInstmentTypeChange(obj) {
            var obj1 = document.getElementById("cmbType").value;
            objInstType = obj.value;
            if (obj1 == "P") {
                if (val1 == 'EXPENCES') {
                    document.getElementById("trDetails").style.display = 'inline';
                    document.getElementById("trDetails1").style.display = 'inline';
                    document.getElementById("trDetails2").style.display = 'inline';
                    document.getElementById("tdLine1").style.display = 'inline';
                }
                else {
                    document.getElementById("trDetails").style.display = 'inline';
                    document.getElementById("trDetails1").style.display = 'none';
                    document.getElementById("trDetails2").style.display = 'none';
                    document.getElementById("tdLine1").style.display = 'inline';
                }
                document.getElementById("trIssuingBank").style.display = 'none';
                document.getElementById("trCustomerBank").style.display = 'none';
                document.getElementById("tdLine1").style.display = 'inline';
                document.getElementById("tdauth").style.display = 'none';
                document.getElementById("tdauth1").style.display = 'none';
                if (objInstType == "0") {
                    document.getElementById("trIssuingBank").style.display = 'none';
                    document.getElementById("trCustomerBank").style.display = 'none';
                    document.getElementById("trDetails").style.display = 'none';
                    document.getElementById("tdLine1").style.display = 'none';
                    document.getElementById("tdauth").style.display = 'none';
                    document.getElementById("tdauth1").style.display = 'none';
                }


            }
            else if (obj1 == "C") {
                if (objInstType == "D") {
                    document.getElementById("trIssuingBank").style.display = 'none';
                    document.getElementById("trCustomerBank").style.display = 'none';
                    document.getElementById("tdauth").style.display = 'none';
                    document.getElementById("tdauth1").style.display = 'none';
                    document.getElementById("trDetails").style.display = 'none';
                    document.getElementById("tdLine1").style.display = 'none';
                }
                else {
                    document.getElementById("trIssuingBank").style.display = 'none';
                    document.getElementById("trCustomerBank").style.display = 'none';
                    document.getElementById("tdauth").style.display = 'none';
                    document.getElementById("tdauth1").style.display = 'none';
                    document.getElementById("trDetails").style.display = 'none';
                    document.getElementById("tdLine1").style.display = 'none';
                }
                if (objInstType == "0") {
                    document.getElementById("trIssuingBank").style.display = 'none';
                    document.getElementById("trCustomerBank").style.display = 'none';
                    document.getElementById("tdauth").style.display = 'none';
                    document.getElementById("tdauth1").style.display = 'none';
                    document.getElementById("trDetails").style.display = 'none';
                    document.getElementById("tdLine1").style.display = 'none';
                }
            }
            else {
                if (objInstType == "D") {
                    document.getElementById("trIssuingBank").style.display = 'inline';
                    document.getElementById("trCustomerBank").style.display = 'none';
                    document.getElementById("trDetails").style.display = 'inline';
                    document.getElementById("trDetails1").style.display = 'none';
                    document.getElementById("trDetails2").style.display = 'none';
                    document.getElementById("tdLine1").style.display = 'inline';
                    document.getElementById("tdauth").style.display = 'none';
                    document.getElementById("tdauth1").style.display = 'none'
                }
                else {
                    if (onlyforcdsl != 'CUSTOMERS') {
                        document.getElementById("trCustomerBank").style.display = 'none';
                    }
                    else {
                        document.getElementById("trCustomerBank").style.display = 'inline';
                    }
                    document.getElementById("trIssuingBank").style.display = 'none';
                    document.getElementById("trDetails").style.display = 'inline';
                    document.getElementById("trDetails1").style.display = 'none';
                    document.getElementById("trDetails2").style.display = 'none';
                    document.getElementById("tdLine1").style.display = 'inline';
                    document.getElementById("tdauth").style.display = 'none';
                    document.getElementById("tdauth1").style.display = 'none'
                    var subVal = document.getElementById("hddsub").value;
                    CmbClientBankCI.PerformCallback(subVal);
                }
                if (objInstType == "0") {
                    document.getElementById("trIssuingBank").style.display = 'none';
                    document.getElementById("trCustomerBank").style.display = 'none';
                    document.getElementById("trDetails").style.display = 'none';
                    document.getElementById("tdLine1").style.display = 'none';
                    document.getElementById("tdauth").style.display = 'none';
                    document.getElementById("tdauth1").style.display = 'none'
                }
            }

        }
        function BankShow(obj1, obj2, obj3) {
            document.getElementById("trIssuingBank").style.display = 'inline';
            document.getElementById("trCustomerBank").style.display = 'none';
            document.getElementById("tdauth").style.display = 'none';
            document.getElementById("tdauth1").style.display = 'none';
            document.getElementById("trDetails").style.display = 'inline';
            document.getElementById("trDetails1").style.display = 'none';
            document.getElementById("trDetails2").style.display = 'none';
            document.getElementById("tdLine1").style.display = 'inline';
            document.getElementById("txtIssuingBank").value = obj2;
            document.getElementById("txtIssuingBank_hidden").value = obj3;
            document.getElementById("txtLineNarration").value = obj1;
        }
        function BankShow1(obj1, obj2, obj3, obj4) {
            document.getElementById("trIssuingBank").style.display = 'none';
            document.getElementById("trCustomerBank").style.display = 'inline';
            document.getElementById("trDetails").style.display = 'inline';
            document.getElementById("trDetails1").style.display = 'none';
            document.getElementById("trDetails2").style.display = 'none';
            document.getElementById("tdLine1").style.display = 'inline';
            document.getElementById("tdauth").style.display = 'none';
            document.getElementById("tdauth1").style.display = 'none'
            document.getElementById("txtLineNarration").value = obj1;
            document.getElementById("txtAuthLetterRef").value = obj3;
            CmbClientBankCI.PerformCallback(obj4);
            document.getElementById("CmbClientBank_I").value = obj2;
        }
        function BankShow2(obj1, obj2, obj3, obj4, obj5) {
            document.getElementById("trIssuingBank").style.display = 'inline';
            document.getElementById("trCustomerBank").style.display = 'inline';
            document.getElementById("trDetails").style.display = 'inline';
            document.getElementById("trDetails1").style.display = 'none';
            document.getElementById("trDetails2").style.display = 'none';
            document.getElementById("tdLine1").style.display = 'inline';
            document.getElementById("tdauth").style.display = 'inline';
            document.getElementById("tdauth1").style.display = 'inline'
            document.getElementById("txtLineNarration").value = obj1;
            document.getElementById("txtAuthLetterRef").value = obj3;
            document.getElementById("txtIssuingBank").value = obj5;
            document.getElementById("txtIssuingBank_hidden").value = obj2;
            CmbClientBankCI.PerformCallback(obj4);
            //document.getElementById("CmbClientBank_VI").value=obj2; 
        }
        function OnCashBankReportSubAcChange(obj) {
            objSubAc = obj.value;
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
        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }
        function AllowNumericOnly(e) {
            var keycode;
            if (window.event) keycode = window.event.keyCode;
            else if (event) keycode = event.keyCode;
            else if (e) keycode = e.which;
            else return true;
            if ((keycode > 47 && keycode <= 57) || (keycode == 46)) {
                return true;
            }
            else {
                return false;
            }
            return true;

        }
        function testDetails(txt) {
            if (txt.value.indexOf(".") > 0) {
                var str = txt.value.substring(txt.value.indexOf(".") + 1);
                if (str.length > 4) {
                    alert("only four decimals are allowed");
                }
            }
            else {
                if (txt.value.length <= 11) {
                    return true;
                }
                else {
                    alert("11 digits only allowed");
                    txt.value = txt.value.substring(0, 11);
                }
            }
        }
        function PopulateData() {
            parent.RefreshGrid();
        }
        function btnCancel_Click() {
            var answer = confirm("Do you Want To Close This Window?");
            if (answer)
                // parent.RefreshGrid();
                parent.editwin.close();
        }
        function CallMainAccount(obj1, obj2, obj3) {
            var obj4 = document.getElementById("cmbType");
            ajax_showOptions(obj1, obj2, obj3, obj4.value, 'Main');
        }
        function CallSubAccount(obj1, obj2, obj3) {
            var valSub;
            var HdVal = document.getElementById("hddnEdit").value;
            if (HdVal == 'Edit') {
                var BranchID = document.getElementById("cmbBranch").value;
                valSub = val + '~' + BranchID;
            }
            else
                valSub = val + '~' + 'N';
            ajax_showOptions(obj1, obj2, obj3, valSub, 'Main');
        }
        function keyVal1(obj) {
            val = obj
        }
        function keyVal(obj) {
            mval = '';
            var expnse = obj.split('~')
            if (expnse.length == 3) {
                val = expnse[0];
                val1 = expnse[1];
                onlyforcdsl = expnse[2];
                document.getElementById("hddmain").value = val;
                var obj1 = document.getElementById("cmbType").value;
                if (obj1 == "P") {
                    if (val1 == 'EXPENCES') {
                        document.getElementById("trDetails1").style.display = 'inline';
                        document.getElementById("trDetails2").style.display = 'inline';
                    }
                    else {
                        document.getElementById("trDetails1").style.display = 'none';
                        document.getElementById("trDetails2").style.display = 'none';
                    }
                }

            }
            else {
                mval = obj
                document.getElementById("hddsub").value = mval;
                var obj1 = document.getElementById("cmbType").value;
                if (obj1 == "R") {
                    if (objInstType == "C" || objInstType == "E") {
                        var subVal = document.getElementById("hddsub").value;
                        CmbClientBankCI.PerformCallback(subVal);
                        document.getElementById("tdauth").style.display = 'none';
                        document.getElementById("tdauth1").style.display = 'none'
                        document.getElementById("trIssuingBank").style.display = 'none';
                    }
                }
            }
            //SearchOnPrefix('2');
        }
        function clear1() {
            document.getElementById("txtIssuingBank").value = "";
            document.getElementById("txtIssuingBank_hidden").value = "";
            document.getElementById("txtAuthLetterRef").value = "";
            //document.getElementById("txtLineNarration").value="";
            document.getElementById("TrInsert").style.display = 'inline';
        }
        function SetSubAcc(obj) {
            var s = document.getElementById(obj);
            s.focus();
            s.select();
        }
        function SetSubAcc1(obj) {
            var s = document.getElementById(obj);
            s.focus();
            s.select();
        }
        function ShowLineForPayment(obj) {
            document.getElementById("trDetails").style.display = 'inline';
            document.getElementById("trIssuingBank").style.display = 'none';
            document.getElementById("trCustomerBank").style.display = 'none';
            document.getElementById("tdauth").style.display = 'none';
            document.getElementById("tdauth1").style.display = 'none';
            document.getElementById("tdLine1").style.display = 'inline';
            document.getElementById("trDetails1").style.display = 'none';
            document.getElementById("trDetails2").style.display = 'none';
            document.getElementById("txtLineNarration").value = obj;
        }
        function ShowLineForPayment1(obj, obj1) {
            document.getElementById("trDetails").style.display = 'inline';
            document.getElementById("trIssuingBank").style.display = 'none';
            document.getElementById("trCustomerBank").style.display = 'none';
            document.getElementById("tdauth").style.display = 'none';
            document.getElementById("tdauth1").style.display = 'none';
            document.getElementById("tdLine1").style.display = 'inline';
            document.getElementById("trDetails1").style.display = 'inline';
            document.getElementById("trDetails2").style.display = 'inline';
            document.getElementById("txtLineNarration").value = obj;
            document.getElementById("cmbPayee").value = obj1;
        }
        function InvisibleAll() {
            document.getElementById("trDetails").style.display = 'none';
            document.getElementById("trIssuingBank").style.display = 'none';
            document.getElementById("trCustomerBank").style.display = 'none';
            document.getElementById("tdauth").style.display = 'none';
            document.getElementById("tdauth1").style.display = 'none';
            document.getElementById("tdLine1").style.display = 'none';
            document.getElementById("trDetails1").style.display = 'none';
            document.getElementById("trDetails2").style.display = 'none';
        }
        function setvaluetovariable(obj) {
            if (obj == 0) {
                document.getElementById("tdauth").style.display = 'inline';
                document.getElementById("tdauth1").style.display = 'inline'
                document.getElementById("trIssuingBank").style.display = 'inline'
            }
            else {
                document.getElementById("tdauth").style.display = 'none';
                document.getElementById("tdauth1").style.display = 'none';
                document.getElementById("trIssuingBank").style.display = 'none'
            }
        }
        function overChange(obj) {
            obj.style.backgroundColor = "#FFD497";
        }
        function OutChange(obj) {
            obj.style.backgroundColor = "#DDECFE";
        }
        function DateChange() {
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dteDate.GetDate().getMonth() + 1;
            var DayDate = dteDate.GetDate().getDate();
            var YearDate = dteDate.GetDate().getYear();
            var exd = new Date();
            if (YearDate >= objsession[0]) {
                if (MonthDate < 4 && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[1]);
                    dteDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > 3 && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[0]);
                    dteDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost;
                    if (MonthDate < 4)
                        datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[1]);
                    else
                        datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[0]);
                    dteDate.SetDate(new Date(datePost));
                }
                else {
                    dtAspxDate.SetValue(dteDate.GetValue());
                    SearchOnPrefix('1');
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost;
                if (MonthDate < 4)
                    datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[1]);
                else
                    datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[0]);
                dteDate.SetDate(new Date(datePost));
            }

        }
        function DateChangeAspx() {
            var sessionVal = "<%=Session["LastFinYear"]%>";
                 var objsession = sessionVal.split('-');
                 var MonthDate = dtAspxDate.GetDate().getMonth() + 1;
                 var DayDate = dtAspxDate.GetDate().getDate();
                 var YearDate = dtAspxDate.GetDate().getYear();
                 if (YearDate >= objsession[0]) {
                     if (MonthDate < 4 && YearDate == objsession[0]) {
                         alert('Enter Date Is Outside Of Financial Year !!');
                         dtAspxDate.SetValue(dteDate.GetValue());
                     }
                     else if (MonthDate > 3 && YearDate == objsession[1]) {
                         alert('Enter Date Is Outside Of Financial Year !!');
                         dtAspxDate.SetValue(dteDate.GetValue());
                     }
                     else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                         alert('Enter Date Is Outside Of Financial Year !!');
                         dtAspxDate.SetValue(dteDate.GetValue());
                     }
                 }
                 else {
                     alert('Enter Date Is Outside Of Financial Year !!');
                     dtAspxDate.SetValue(dteDate.GetValue());
                 }
             }
             function SearchOnPrefix(s) {
                 var comp = document.getElementById('cmbCompany').value;
                 var segmnt = document.getElementById('cmbSegment').value;
                 var date = document.getElementById('dteDate_I').value;
                 if (s == '1') {
                     var bank = document.getElementById('cmbCashBankAc').value;
                     var dest = document.getElementById('ASPxcurrBankBalance');
                     var param = date + '~' + s + '~' + comp + '~' + segmnt + '~' + bank;
                 }
                 else {
                     var dest = document.getElementById('ASPxcurrentAcBalance');
                     var mainac = document.getElementById('hddmain').value;
                     var subac = document.getElementById('hddsub').value;
                     var param = date + '~' + s + '~' + comp + '~' + segmnt + '~' + mainac + '~' + subac;
                 }
                 PageMethods.GetContactName(param, CallSuccess, CallFailed, dest);
             }
             function CallSuccess(res, destCtrl) {
                 destCtrl.innerText = res;
                 var cc = res.substr(0, 1);
                 if (cc == '-')
                     destCtrl.style.color = 'red';
             }
             function CallFailed(res, destCtrl) {
                 alert(res.get_message());
             }
             function AcBalance() {
                 SearchOnPrefix('2')
             }
             function SegSelect(obj) {
                 SearchOnPrefix('1');
             }
             function BankChange() {
                 SearchOnPrefix('1');
             }
             function SubAccountCheck(obj) {
                 var obj3 = obj.split('_');
                 var obj4 = 'grdAdd' + '_' + obj3[1] + '_' + 'txtMainAccount';
                 var testMainAcc = document.getElementById(obj4);
                 if (testMainAcc.value == '') {
                     alert('MainAccount Name Required !!!');
                     testMainAcc.focus();
                     testMainAcc.select();
                     return false;
                 }
                 if (onlyforcdsl != 'NONE') {
                     if (mval == '') {
                         var obj1 = obj.split('_');
                         var obj2 = 'grdAdd' + '_' + obj1[1] + '_' + 'txtSubAccount';
                         var testSubAcc = document.getElementById(obj2);
                         // var testSubAcc=document.getElementById('<%=((TextBox)grdAdd.FooterRow.FindControl("txtSubAccount")).ClientID%>');
                         alert('SubAccount Name Required !!!');
                         testSubAcc.focus();
                         testSubAcc.select();
                         return false;
                     }
                     else {
                         mval = '';
                     }
                 }
             }
             function AlertInstNumber(obj) {
                 var chkNum = obj.split(',');
                 var cell;
                 var rowID;
                 var grid = document.getElementById("<%= grdAdd.ClientID %>");
                for (j = 0; j < chkNum.length; j++) {
                    if (grid.rows.length > 0) {
                        for (i = 1; i < grid.rows.length - 1; i++) {
                            cell = grid.rows[i].cells[3];
                            if (cell.innerText.trim() == chkNum[j].trim()) {
                                grid.rows[i].cells[3].style.color = '#D10B0B';//'#ffe1ac'
                            }
                        }
                    }
                }
                var answer = confirm("Duplicate Instrument Number " + obj + " Do You Want To Continue ??");
                if (answer) {
                    document.getElementById('Button2').click();
                }
            }
            FieldName = 'txtVoucherNo';
        </script>

    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td>
                    <table>
                        <tr id="">
                            <td style="text-align: left;">Type :</td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="cmbType" runat="server" Width="100px" Font-Size="12px" AutoPostBack="True"
                                    OnChange="javascript:TypeSelect1(this);" OnSelectedIndexChanged="cmbType_SelectedIndexChanged1"
                                    TabIndex="1">
                                    <asp:ListItem Value="R">Receipt</asp:ListItem>
                                    <asp:ListItem Value="C">Contra</asp:ListItem>
                                    <asp:ListItem Value="P">Payment</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;">Date :
                            </td>
                            <td style="text-align: left;">
                                <dxe:ASPxDateEdit ID="dteDate" runat="server" EditFormat="Custom" ClientInstanceName="dteDate"
                                    UseMaskBehavior="True" Font-Size="12px" Width="100px" TabIndex="2">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <ClientSideEvents DateChanged="function(s,e){DateChange(); }" />
                                </dxe:ASPxDateEdit>
                            </td>
                            <td style="text-align: left;">Company :</td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="cmbCompany" runat="server" DataSourceID="dsCompany" DataTextField="CashBank_CompanyName"
                                    DataValueField="CashBank_CompanyID" Width="353px" Font-Size="12px" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbCompany_SelectedIndexChanged" TabIndex="3" Enabled="False">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;">Segment :
                            </td>
                            <td style="text-align: left;">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbSegment" runat="server" Width="100px" OnChange="javascript:SegSelect(this);" Font-Size="12px" TabIndex="4">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbCompany" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="trBankCashType">
                            <td style="text-align: left;" id="tdBankCashType">Voucher :
                            </td>
                            <td style="text-align: left;" id="tdBankCashType1">
                                <asp:TextBox ID="txtVoucherNo" runat="server" Font-Size="12px" ReadOnly="true" Width="97px"></asp:TextBox>
                            </td>
                            <td style="text-align: left;" id="tdBankAccountNo">Branch :
                            </td>
                            <td style="text-align: left;" id="tdBankAccountNo1">
                                <asp:DropDownList ID="cmbBranch" runat="server" DataSourceID="dsBranch" DataTextField="BANKBRANCH_NAME"
                                    DataValueField="BANKBRANCH_ID" Width="150px" Font-Size="12px" TabIndex="5">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;" id="tdcb">Cash/Bank A/c :
                            </td>
                            <td style="text-align: left;" id="tdcb1">
                                <asp:DropDownList ID="cmbCashBankAc" runat="server" DataSourceID="MainAccount" DataTextField="MainAccount_Name" OnChange="javascript:BankChange();"
                                    DataValueField="CashBank_MainAccountID" Width="353px" Font-Size="12px" TabIndex="6" AutoPostBack="True" OnSelectedIndexChanged="cmbCashBankAc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;" id="td3">Settl.No:
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtSettlementNo" runat="server" Width="100px" Font-Size="12px" TabIndex="7"></asp:TextBox>&nbsp;
                            </td>
                        </tr>
                        <tr id="tr6" style="background-color: #ddecfe;" align="center">
                            <td style="text-align: left;">Narration :</td>
                            <td colspan="5" style="text-align: left">
                                <asp:TextBox ID="txtNarration" MaxLength="500" TextMode="MultiLine" onkeyDown="checkTextAreaMaxLength(this,event,'500');"
                                    runat="server" Font-Size="11px" TabIndex="8" Width="700px"></asp:TextBox>
                            </td>
                            <td colspan="2" style="text-align: left">
                                <table cellpadding="0px" cellspacing="0px">
                                    <tr>
                                        <td style="text-align: left;">Curr. Bank Balan. </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="ASPxcurrBankBalance" runat="server" Font-Size="12px" Width="100px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" id="td13">Curr. A/C Balan.:
                                        </td>
                                        <td style="text-align: right;" id="td14">
                                            <asp:Label ID="ASPxcurrentAcBalance" runat="server" Font-Size="12px" Width="100px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="txtSettlementNo_hidden" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="height: 20px"></td>
            </tr>
            <tr style="background-color: #a4c6f8">
                <td>
                    <table style="width: 100%; border-top-width: thin; border-left-width: thin;" border="0"
                        id="Table1" cellspacing="0">
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdAdd" runat="server" DataKeyNames="CashReportID" CssClass="gridcellleft"
                                            CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                                            Width="100%" AutoGenerateColumns="False" OnRowEditing="grdAdd_RowEditing" OnRowDeleting="grdAdd_RowDeleting"
                                            ShowFooter="True" OnRowCommand="grdAdd_RowCommand" OnRowCreated="grdAdd_RowCreated"
                                            OnRowDataBound="grdAdd_RowDataBound" OnRowCancelingEdit="grdAdd_RowCancelingEdit"
                                            OnRowUpdating="grdAdd_RowUpdating">
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                            <EditRowStyle BackColor="#E59930" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue"
                                                BorderWidth="1px" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="MainAccount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMainAccount" runat="server" Text='<%# Eval("MainAccount1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditMainAccount" TabIndex="8" Font-Size="12px" Text='<%# Eval("MainAccount1") %>' Width="180px" onkeyup="CallMainAccount(this,'MainAccount',event)"
                                                            runat="server"></asp:TextBox>
                                                        <asp:HiddenField ID="txtEditMainAccount_hidden" runat="server" Value='<%# Eval("CashBank_MainAccountID") %>' />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtMainAccount" TabIndex="8" Font-Size="12px" Width="180px" onkeyup="CallMainAccount(this,'MainAccount',event)"
                                                            runat="server"></asp:TextBox>
                                                        <asp:HiddenField ID="txtMainAccount_hidden" runat="server" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SubAccount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubAccount" runat="server" Text='<%# Eval("SubAccount1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditSubAccount" TabIndex="9" Font-Size="12px" Text='<%# Eval("SubAccount1") %>' Width="180px" onkeyup="CallSubAccount(this,'SubAccountMod',event)"
                                                            runat="server"></asp:TextBox>
                                                        <asp:HiddenField ID="txtEditSubAccount_hidden" runat="server" Value='<%# Eval("SubAccountID") %>' />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtSubAccount" TabIndex="9" Font-Size="12px" Width="180px" onkeyup="CallSubAccount(this,'SubAccountMod',event)" onblur="AcBalance()"
                                                            runat="server"></asp:TextBox>
                                                        <asp:HiddenField ID="txtSubAccount_hidden" runat="server" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Inst. Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInstType" runat="server" Text='<%# Eval("InstType1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlEditInstType" Font-Size="12px" SelectedValue='<%# Eval("CashBank_InstrumentType") %>' OnChange="javascript:OnInstmentTypeChange(this);" runat="server" Width="65px"
                                                            TabIndex="11">
                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                            <asp:ListItem Value="C">Cheque</asp:ListItem>
                                                            <asp:ListItem Value="D">Draft</asp:ListItem>
                                                            <asp:ListItem Value="E">E. Trnsfr</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlInstType" Font-Size="12px" runat="server" OnChange="javascript:OnInstmentTypeChange(this);"
                                                            Width="65px" TabIndex="11">
                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                            <asp:ListItem Value="C">Cheque</asp:ListItem>
                                                            <asp:ListItem Value="D">Draft</asp:ListItem>
                                                            <asp:ListItem Value="E">E. Trnsfr</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Inst No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInstNo" runat="server" Text='<%# Eval("CashBank_InstrumentNumber") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditInstNumber" Font-Size="12px" TabIndex="16" Width="60px" runat="server"
                                                            Text='<%# Eval("CashBank_InstrumentNumber") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtInstNo" runat="server" Font-Size="12px" TabIndex="16" Width="60px"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("CashBank_InstrumentDate1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <dxe:ASPxDateEdit ID="dtEditAspxDate" runat="server" Value='<%# Eval("CashBank_InstrumentDate") %>'
                                                            EditFormat="Custom" TabIndex="17" EditFormatString="dd MMMM yyyy" UseMaskBehavior="True"
                                                            Font-Size="12px" Width="80px">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <dxe:ASPxDateEdit ID="dtAspxDate" runat="server" EditFormat="Custom" TabIndex="17" ClientInstanceName="dtAspxDate"
                                                            UseMaskBehavior="True" Font-Size="12px" Width="80px" EditFormatString="dd MMMM yyyy">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <ClientSideEvents DateChanged="function(s,e){DateChangeAspx(); }" />
                                                        </dxe:ASPxDateEdit>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payment">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWithdraw" runat="server" Text='<%# Eval("WithDrawl") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <%-- <asp:TextBox ID="txtEditWithdraw" TabIndex="18" Font-Size="12px" onkeyup="testDetails(this);"
                                                                onkeypress="return AllowNumericOnly(this);" Width="85px" runat="server" Text='<%# Eval("CashBank_AmountWithdrawl") %>'></asp:TextBox>--%>
                                                        <dxe:ASPxTextBox ID="txtEditWithdraw" runat="server" TabIndex="18" HorizontalAlign="Right" Width="120px" Text='<%# Eval("CashBank_AmountWithdrawl") %>'>
                                                            <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                            <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                                        </dxe:ASPxTextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <%--<asp:TextBox ID="txtWithdraw" runat="server" TabIndex="18" Font-Size="12px" onkeyup="testDetails(this);"
                                                                onkeypress="return AllowNumericOnly(this);" Width="85px"></asp:TextBox>--%>
                                                        <dxe:ASPxTextBox ID="txtWithdraw" ClientInstanceName="cwithdraw" runat="server" Width="120px" TabIndex="18" HorizontalAlign="Right">
                                                            <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                            <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                                        </dxe:ASPxTextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Receipt">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReceipt" runat="server" Text='<%# Eval("Receipt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <%--<asp:TextBox ID="txtEditRecpt" runat="server" Font-Size="12px" TabIndex="19" onkeyup="testDetails(this);"
                                                                onkeypress="return AllowNumericOnly(this);" Width="85px" Text='<%# Eval("CashBank_AmountDeposit") %>'></asp:TextBox>--%>
                                                        <dxe:ASPxTextBox ID="txtEditRecpt" runat="server" TabIndex="19" HorizontalAlign="Right" Width="120px" Text='<%# Eval("CashBank_AmountDeposit") %>'>
                                                            <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                            <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                                        </dxe:ASPxTextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <%--<asp:TextBox ID="txtReceipt" runat="server" Font-Size="12px" TabIndex="19" onkeyup="testDetails(this);"
                                                                onkeypress="return AllowNumericOnly(this);" Width="85px"></asp:TextBox>--%>
                                                        <dxe:ASPxTextBox ID="txtReceipt" runat="server" Width="120px" ClientInstanceName="creceipt" TabIndex="19" HorizontalAlign="Right">
                                                            <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                            <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                                        </dxe:ASPxTextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Font-Size="12px" Text='<%# Eval("CashReportID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="True" CssClass="btnUpdate" CommandName="Update"
                                                            Text="Save"></asp:LinkButton>|
                                                            
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CssClass="btnUpdate" CommandName="Edit"
                                                            Text="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CssClass="btnUpdate" CommandName="Delete"
                                                            OnClientClick="javascript:return confirm('Do You Want To Delete This Record ?')"
                                                            Text="Delete">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CssClass="btnUpdate" CommandName="Cancel"
                                                            Text="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="Button1" runat="server" Text="Add" CausesValidation="false" TabIndex="20" OnClientClick="javascript:return SubAccountCheck(this.id);" CommandName="Insert" onfocus="overChange(this)" onBlur="OutChange(this)" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbType" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbCashBankAc" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />

                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <table>

                                    <tr id="trCustomerBank" style="display: none">
                                        <td style="text-align: left;" id="tdCustomerBank">Customer Bank
                                        </td>
                                        <td style="text-align: left;" id="tdCustomerBank1">
                                            <dxe:ASPxComboBox ID="CmbClientBank" runat="server" Width="370px" Height="20" DropDownWidth="550"
                                                DropDownStyle="DropDownList" DataSourceID="dsgrdClientbank" ValueField="cbd_id"
                                                ValueType="System.String"
                                                TextFormatString="{0} -- {2}" EnableIncrementalFiltering="True" CallbackPageSize="30"
                                                ClientInstanceName="CmbClientBankCI" OnCallback="CmbClientBank_OnCallback"
                                                TabIndex="12">
                                                <Columns>
                                                    <dxe:ListBoxColumn FieldName="bnk_bankName" Caption="Bank Name" Width="150px" ToolTip="Bank Name" />
                                                    <dxe:ListBoxColumn FieldName="cbd_accountName" Caption="Account Holder Name" Width="200px"
                                                        ToolTip="Account Holder Name" />
                                                    <dxe:ListBoxColumn FieldName="cbd_accountNumber" Caption="Account Number" Width="120px"
                                                        ToolTip="Account Number" />
                                                    <dxe:ListBoxColumn FieldName="bnk_micrno" Caption="MICR Number" Width="80px" ToolTip="MICR Number" />
                                                    <dxe:ListBoxColumn FieldName="cbd_Accountcategory" Caption="Account Type" Width="80px"
                                                        ToolTip="MICR Number" />
                                                </Columns>
                                                <ClientSideEvents ValueChanged="function(s,e){
                                                                                    var indexr = s.GetSelectedIndex();
                                                                                    setvaluetovariable(indexr)
                                                                                    }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr id="trIssuingBank" style="display: none">
                                        <td style="text-align: left;">Issuing Bank
                                        </td>
                                        <td style="text-align: left;" colspan="3">
                                            <asp:TextBox ID="txtIssuingBank" runat="server" Font-Size="12px" TabIndex="12" Width="365px"></asp:TextBox>
                                            <asp:HiddenField ID="txtIssuingBank_hidden" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" id="tdauth">Auth. Letter Ref
                                        </td>
                                        <td style="text-align: left;" id="tdauth1">
                                            <asp:TextBox ID="txtAuthLetterRef" runat="server" Width="364px" TabIndex="13"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trDetails" style="display: none">
                                        <td style="text-align: left;" id="trDetails1">Payee:
                                        </td>
                                        <td style="text-align: left;" id="trDetails2">
                                            <asp:DropDownList ID="cmbPayee" runat="server" Width="366px" Font-Size="12px" TabIndex="14">
                                            </asp:DropDownList>
                                        </td>

                                    </tr>

                                </table>
                            </td>
                            <td style="vertical-align: top">
                                <table>
                                    <tr>
                                        <td style="text-align: left;" id="tdLine1">
                                            <asp:TextBox ID="txtLineNarration" MaxLength="500" CssClass="water" Text="Line Narration" ToolTip="Line Narration" Width="488px" TextMode="MultiLine" Font-Size="11px"
                                                onkeyDown="checkTextAreaMaxLength(this,event,'500');" runat="server" TabIndex="15" Height="70px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>


                    </table>
                </td>
            </tr>

        </table>
        <table width="100%">
            <tr id="TrInsert">
                <td style="text-align: right">
                    <asp:Button ID="btnInsert" runat="server" Text="Save Vouchers" AccessKey="S" CssClass="btnUpdate"
                        OnClick="btnInsert_Click" />
                    <input id="btnCancel" type="button" value="Cancel" class="btnUpdate" onclick="btnCancel_Click()" />
                    <asp:HiddenField ID="hddnEdit" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="display: none">
                    <input id="hddmain" type="text" />
                    <input id="hddsub" type="text" />
                    <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
                </td>
            </tr>
        </table>
    </div>
    <asp:SqlDataSource ID="dsCashBank" runat="server" ConflictDetection="CompareAllValues"
        SelectCommand=""
        InsertCommand="insert into table1 (temp123) values('11')" UpdateCommand="update table1 set temp123='123'"></asp:SqlDataSource>
    <asp:SqlDataSource ID="dsCompany" runat="server"
        ConflictDetection="CompareAllValues" SelectCommand="SELECT COMP.CMP_INTERNALID AS CashBank_CompanyID , COMP.CMP_NAME AS CashBank_CompanyName  FROM TBL_MASTER_COMPANY AS COMP">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectSegment" runat="server"  ConflictDetection="CompareAllValues"
        SelectCommand="SELECT LTRIM(RTRIM(A.EXCH_INTERNALID)) AS CashBank_ExchangeSegmentID ,TME.EXH_ShortName + '--' + A.EXCH_SEGMENTID AS EXCHANGENAME FROM (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID=@COMPANYID) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID">
        <SelectParameters>
            <asp:Parameter Name="COMPANYID" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsBranch" runat="server"
        ConflictDetection="CompareAllValues" SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="MainAccount" runat="server"
        SelectCommand="Select MainAccount_AccountCode as CashBank_MainAccountID, MainAccount_AccountCode+'-'+MainAccount_Name+' [ '+MainAccount_BankAcNumber+' ]'+' ~ '+MainAccount_BankCashType as MainAccount_Name from Master_MainAccount where (MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash') and MainAccount_BankCompany=@comp and (RTRIM(MainAccount_ExchangeSegment)=@seg or RTRIM(MainAccount_ExchangeSegment)=0)">
        <SelectParameters>
            <asp:SessionParameter Name="comp" SessionField="LastCompany" Type="string" />
            <asp:SessionParameter Name="seg" SessionField="usersegid" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsgrdClientbank" runat="server" ConflictDetection="CompareAllValues"
        InsertCommand="insert into table1 (temp123) values('11')"
        SelectCommand="select A.* , MB.bnk_id,MB.bnk_bankName,MB.bnk_BranchName,MB.bnk_micrno from (Select TCBD.cbd_id,TCBD.cbd_cntId,TCBD.cbd_bankCode, TCBD.cbd_Accountcategory,TCBD.cbd_Accountcategory as AccountType,TCBD.cbd_accountNumber,TCBD.cbd_accountType,cbd_accountName from tbl_trans_contactBankDetails as  TCBD where TCBD.cbd_cntId=@SubAccountCode) as A inner  join tbl_master_Bank as MB on MB.bnk_id=a.cbd_bankCode order by A.cbd_Accountcategory">
        <SelectParameters>
            <aspaSource"SubAccountCode" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
