<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master"
    ValidateRequest="false" EnableEventValidation="false" Inherits="ERP.OMS.Reports.Reports_frmReport_IFrameLedgerView" CodeBehind="frmReport_IFrameLedgerView.aspx.cs" %>


<%--<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    

    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />

    <script type="text/javascript" src="../modalfiles/modal.js"></script>

    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>

    <%-- <script type="text/javascript" src="/assests/js/loaddata.js"></script>--%>

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
            z-index: 32767;
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
            z-index: 3000;
        }

        form {
            display: inline;
        }
    </style>

    <script language="javascript" type="text/javascript">
        groupvalue = "";
        Mainvalue = "";

        function norecordfound() {

            Page_Load();

            alert('No Record Found  !!');

        }

        function ShowDateSelect() {
            document.getElementById("ShowDt").style.display = 'inline';
            document.getElementById("ShowHd").style.display = 'none';

        }

        function ForFilterOff() {
            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("TdAll1").style.display = 'none';
            document.getElementById("TrBtn").style.display = 'none';
            document.getElementById('spanBtn').style.display = 'none';
            document.getElementById('tr_hdrall').style.display = 'none';
            document.getElementById('tr_ftrall').style.display = 'none';
            document.getElementById('BtnGenerate').style.display = 'none';

            height();
        }
        function MailsendT() {
            alert("Mail Sent Successfully");
        }
        function MailsendF() {
            alert("Error on sending!Try again..");
        }
        function SignOff() {
            window.parent.SignOff();
        }
        FieldName = 'lstSuscriptions';
        function showOptions(obj1, obj2, obj3) {
            var cmb = document.getElementById('cmbsearchOption');
            //alert(cmb.value);
            ajax_showOptions(obj1, obj2, obj3, cmb.value, 'Sub');
        }
        function showOptions1(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3, 'Ac Name1', 'Main');
        }
        function showOptionsforSunAc(obj1, obj2, obj3) {
            var cmb = document.getElementById('ddlAccountType');
            var litMain = document.getElementById('litAcMain');
            if (cmb.value != '3') {
                ajax_showOptions(obj1, obj2, obj3, '0', 'Sub');
            }
            else {
                var valMainAc1 = document.getElementById('txtMainAccount_hidden').value;
                var valMainAc2 = valMainAc1.split('~');
                var valMainAc = "('" + valMainAc2[0] + "')";
                ajax_showOptions(obj1, obj2, obj3, valMainAc, 'Sub');
            }
        }
        function keyVal(obj) {

            var obj1 = obj.split('~');
            Mainvalue = "'" + obj1[0] + "'";
            document.getElementById('HdnMainAc').value = Mainvalue;
            document.getElementById('HdnSubLedgerType').value = obj1[1];
            document.getElementById('HdnForBranchGroup').value = 'a';

            if (obj1[1] == 'None') {
                document.getElementById('TrSubAccount').style.display = 'none';
                document.getElementById('TrForGroup').style.display = 'none';
                document.getElementById('TrForClient').style.display = 'none';

                //-- For Email 
                document.getElementById('Rcb').style.display = 'none';
                document.getElementById('Rct').style.display = 'none';
                document.getElementById('Rbb').style.display = 'none';
                document.getElementById('Rbt').style.display = 'none';

                dateTimeForSubledger = 'a';
            }
            else {
                if (obj1[1] == 'Custom' || obj1[1] == 'Products-Equity' || obj1[1] == 'Products-Commodity  ' || obj1[1] == 'Products-MF' || obj1[1] == 'Products-Insurance ' || obj1[1] == 'Products-ConsumerFinance' || obj1[1] == 'RTAs ' || obj1[1] == 'MFs' || obj1[1] == 'AMCs ' || obj1[1] == ' Insurance Cos' || obj1[1] == 'Consumer Finance Cos  ' || obj1[1] == 'Custodians ' || obj1[1] == 'Consultants' || obj1[1] == 'Share Holder' || obj1[1] == 'Debtors' || obj1[1] == 'Creditors') {
                    document.getElementById('TrForGroup').style.display = 'none';
                    document.getElementById('TrForClient').style.display = 'none';
                    document.getElementById('TrSubAccount').style.display = 'inline';
                    //-- For Email 
                    document.getElementById('Rcb').style.display = 'none';
                    document.getElementById('Rct').style.display = 'none';
                    document.getElementById('Rbb').style.display = 'none';
                    document.getElementById('Rbt').style.display = 'none';
                    document.getElementById('TrOB').style.display = 'none';
                }
                else {
                    document.getElementById('TrForGroup').style.display = 'inline';
                    document.getElementById('TrForClient').style.display = 'inline';
                    document.getElementById('TrSubAccount').style.display = 'none';
                    //-- For Email 
                    document.getElementById('Rcb').style.display = 'inline';
                    document.getElementById('Rct').style.display = 'inline';
                    document.getElementById('Rbb').style.display = 'inline';
                    document.getElementById('Rbt').style.display = 'inline';
                    document.getElementById('TrOB').style.display = 'inline';
                    if (obj1[1] != 'NSDL Clients' || obj1[1] != 'CDSL Clients')
                        document.getElementById('HdnForBranchGroup').value = obj1[1];
                }
                dateTimeForSubledger = 'b';
            }
        }
        function height() {
            if (document.body.scrollHeight >= 500) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '500';
            }
            window.frameElement.Width = document.body.scrollWidth;
        }
        function ShowHide(obj) {
            //document.getElementById('Div1').style.display="none";
            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("TdAll1").style.display = 'none';
            document.getElementById("TrBtn").style.display = 'none';
            document.getElementById('TrForFilter').style.display = 'inline';
            document.getElementById('showDetail').style.display = 'inline';
            // document.getElementById('Tr_Location').style.display='none';
            document.getElementById('tr_hdrall').style.display = 'none';
            document.getElementById('tr_ftrall').style.display = 'none';
            document.getElementById('BtnGenerate').style.display = 'none';
            document.getElementById('spanshow3').innerText = obj;
            var combo = document.getElementById('txtMainAccount');
            if (combo.value != '')
                combotext = combo.value;
            var AcVal = document.getElementById('ddlAccountType');
            if (AcVal.value == '3')
                document.getElementById('spanshow').innerText = combotext;
            else if (AcVal.value == '0')
                document.getElementById('spanshow').innerText = 'Clients - Trading A/c';
            else if (AcVal.value == '1')
                document.getElementById('spanshow').innerText = 'Clients - Margin Deposit A/c';
            else if (AcVal.value == '2')
                document.getElementById('spanshow').innerText = 'Clients - Trading A/c and Clients - Margin Deposit A/c';
            height();
        }
        function ForFilter() {
            document.getElementById("TrAll").style.display = 'inline';
            document.getElementById("TdAll1").style.display = 'inline';
            document.getElementById("TrBtn").style.display = 'inline';
            document.getElementById('TrForFilter').style.display = 'none';
            document.getElementById('TrSearch').style.display = 'none';
            document.getElementById('TrSearch1').style.display = 'none';
            document.getElementById('TrSearch2').style.display = 'none';
            document.getElementById('spanBtn').style.display = 'inline';
            document.getElementById('TrPrevNext').style.display = 'none';
            height();
        }
        function ChangeAccountType() {
            var AcVal = document.getElementById('ddlAccountType').value;
            if (AcVal == '3') {
                document.getElementById('TdAc').style.display = 'inline';
                document.getElementById('TdAc1').style.display = 'inline';
                document.getElementById('TrOB').style.display = 'none';
            }
            else {
                document.getElementById('TdAc').style.display = 'none';
                document.getElementById('TdAc1').style.display = 'none';
                document.getElementById('TrSubAccount').style.display = 'none';
                document.getElementById('TrForGroup').style.display = 'inline';
                document.getElementById('TrForClient').style.display = 'inline';
                document.getElementById('TrOB').style.display = 'inline';
            }
            //document.getElementById('Div1').style.display="inline"; 
            document.getElementById('txtMainAccount_hidden').value = '';
            document.getElementById('txtMainAccount').value = '';
            // document.getElementById('Button1').disabled=true;
            //            var btn = document.getElementById('ButtonUpdate');
            //            btn.click();
        }
        function Page_Load() {
            document.getElementById('showFilter').style.display = 'none';
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdAc').style.display = 'none';
            document.getElementById('TdAc1').style.display = 'none';
            document.getElementById('TrPrevNext').style.display = 'none';
            document.getElementById('TrForFilter').style.display = 'none';
            document.getElementById('TrSearch').style.display = 'none';
            document.getElementById('TrSearch1').style.display = 'none';
            document.getElementById('TrSearch2').style.display = 'none';
            document.getElementById('showDetail').style.display = 'none';
            document.getElementById('trDateVoucher').style.display = 'none';
            document.getElementById('TrSubAccount').style.display = 'none';
            document.getElementById('btnPrint').style.display = 'none';
            document.getElementById('TrPrint').style.display = 'none';
            document.getElementById('TrLogo').style.display = 'none';
            document.getElementById('MailTo').style.display = 'none';
            document.getElementById('btnSend').style.display = 'none';
            document.getElementById('btnExExport').style.display = 'none';
            document.getElementById('trManualJvOption').style.display = 'none';
            document.getElementById('trSystemJvOptions').style.display = 'none';
            document.getElementById('trCb').style.display = 'none';
            document.getElementById('trJv').style.display = 'none';
            //document.getElementById('Tr_Location').style.display='none';
            document.getElementById('tr_hdrall').style.display = 'none';
            document.getElementById('tr_ftrall').style.display = 'none';
            document.getElementById('BtnGenerate').style.display = 'none';
            dateTimeForSubledger = 'b';
            height();
        }
        function Page_Load_NsdlCdsl(obj) {
            document.getElementById('ddlAccountType').value = "3";
            document.getElementById('TdAc').style.display = 'inline';
            document.getElementById('TdAc1').style.display = 'inline';

            if (obj == 'NSDL') {
                document.getElementById('txtMainAccount').value = "NSDL Clients A/c";
                //alert(Mainvalue);
                document.getElementById('HdnMainAc').value = "'SYSTM00043'";

                document.getElementById('txtMainAccount_hidden').value = "SYSTM00043~NSDL Clients";
                document.getElementById('HdnSubLedgerType').value = "NSDL Clients";
            }
            else {
                document.getElementById('txtMainAccount').value = "CDSL Clients A/c";
                document.getElementById('HdnMainAc').value = "'SYSTM00042'";
                document.getElementById('txtMainAccount_hidden').value = "SYSTM00042~CDSL Clients";
                document.getElementById('HdnSubLedgerType').value = "CDSL Clients";
            }

            document.getElementById("ddlAccountType").disabled = true;
        }
        function SegAll(obj) {

            document.getElementById('showFilter').style.display = 'none';
            document.getElementById('TdFilter').style.display = 'none';
            if (obj == 'seg') {
                var segmentname = "<%=Session["SegMentName"]%>";
                var segsplit = segmentname.split('-');
                var segtype = segsplit[1];
                //               if(segtype=='FO')
                //               {
                ////                alert(segtype);
                //                document.getElementById('tdexpDateWise').style.display="none";
                //                document.getElementById('tdexpDateWise1').style.display="none";
                //               }
                //               if(segtype=='CDX')
                //               {
                ////                alert(segtype);
                //                document.getElementById('tdexpDateWise').style.display="none";
                //                document.getElementById('tdexpDateWise1').style.display="none";
                //               }
                //               if(segtype=='CM')
                //               {
                ////                alert(segtype);
                //                document.getElementById('tdexpDateWise').style.display="none";
                //                document.getElementById('tdexpDateWise1').style.display="none";
                //               }
                //               if(segtype=='COMM')
                //               {
                //                alert(segtype);
                document.getElementById('tdexpDateWise').style.display = "none";
                document.getElementById('tdexpDateWise1').style.display = "none";
                //}

            }
            else {
                document.getElementById('Button1').disabled = false;
            }
            if (obj == 'Sub') {
                dateChange();
                //                var btn = document.getElementById('ButtonUpdate');
                //                btn.click();
            }
            if (obj == 'date') {

                var segname = "<%=Session["SegMentName"]%>";

            }
        }
        function SegSelected(obj) {

            document.getElementById('showFilter').style.display = 'inline';
            document.getElementById('TdFilter').style.display = 'inline';
            if (obj == 'seg') {

                if (obj == 'seg' && (document.getElementById('HDNAccInd').value == 'N')) {
                    document.getElementById('spanSub').style.display = "none";
                    document.getElementById('spanall').style.display = "none";
                    document.getElementById('span2').style.display = "none";
                    document.getElementById('showFilter').style.display = 'none';
                    document.getElementById('TdFilter').style.display = 'none';
                    document.getElementById('spanSystemjv').style.display = "none";
                }

                else {
                    document.getElementById('cmbsearchOption').value = 'Segment';
                    document.getElementById('spanSub').style.display = "none";
                    document.getElementById('spanall').style.display = "inline";
                    document.getElementById('span2').style.display = "none";
                    document.getElementById('spanSystemjv').style.display = "none";
                }



                var seg = document.getElementById('litSegment').value;



            }
            else if (obj == 'branch') {
                document.getElementById('cmbsearchOption').value = 'Branch';
                document.getElementById('spanSub').style.display = "none";
                document.getElementById('spanall').style.display = "inline";
                document.getElementById('span2').style.display = "none";
                document.getElementById('Button1').disabled = true;
                document.getElementById('spanSystemjv').style.display = "none";
            }
            else if (obj == 'Ac') {
                document.getElementById('cmbsearchOption').value = 'Ac Name';
                document.getElementById('spanSub').style.display = "none";
                document.getElementById('spanall').style.display = "inline";
                document.getElementById('span2').style.display = "none";
                document.getElementById('Button1').disabled = true;
                document.getElementById('spanSystemjv').style.display = "none";
            }
            else if (obj == 'B') {
                document.getElementById('cmbsearchOption').value = 'Branch';
                document.getElementById('spanSub').style.display = "none";
                document.getElementById('spanall').style.display = "none";
                document.getElementById('span2').style.display = "inline";
                document.getElementById('Button1').disabled = true;
                document.getElementById('spanSystemjv').style.display = "none";
            }
            else if (obj == 'G') {
                document.getElementById('cmbsearchOption').value = 'Group';
                document.getElementById('spanSub').style.display = "none";
                document.getElementById('spanall').style.display = "none";
                document.getElementById('span2').style.display = "inline";
                document.getElementById('Button1').disabled = true;
                document.getElementById('spanSystemjv').style.display = "none";
            }
            else if (obj == 'SAC') {
                document.getElementById('cmbsearchOption').value = 'Clients';
                document.getElementById('spanSub').style.display = "none";
                document.getElementById('spanall').style.display = "none";
                document.getElementById('span2').style.display = "inline";
                document.getElementById('Button1').disabled = true;
                document.getElementById('spanSystemjv').style.display = "none";
            }
            else if (obj == 'Set') {
                document.getElementById('cmbsearchOption').value = 'Settlement';
                document.getElementById('spanSub').style.display = "none";
                document.getElementById('spanall').style.display = "none";
                document.getElementById('span2').style.display = "inline";
                document.getElementById('Button1').disabled = true;
                document.getElementById('spanSystemjv').style.display = "none";
            }
            else if (obj == 'Emp') {
                document.getElementById('cmbsearchOption').value = 'Employee';
                document.getElementById('spanSub').style.display = "none";
                document.getElementById('spanall').style.display = "none";
                document.getElementById('span2').style.display = "inline";
                document.getElementById('Button1').disabled = true;
                document.getElementById('spanSystemjv').style.display = "none";
            }

            else {
                document.getElementById('cmbsearchOption').value = 'Sub Ac';
                document.getElementById('spanSub').style.display = "inline";
                document.getElementById('spanall').style.display = "none";
                document.getElementById('span2').style.display = "none";
                document.getElementById('Button1').disabled = true;
                document.getElementById('spanSystemjv').style.display = "none";
            }
        }
        function btnAddsubscriptionlist_click() {

            var cmb = document.getElementById('cmbsearchOption');
            if (cmb.value == 'Clients' || cmb.value == 'Branch' || cmb.value == 'Group' || cmb.value == 'Settlement' || cmb.value == 'Employee') {
                var userid = document.getElementById('txtSubsubcriptionIDBranch');
                if (userid.value != '') {
                    var ids = document.getElementById('txtSubsubcriptionIDBranch_hidden');
                    var listBox = document.getElementById('lstSuscriptions');
                    var tLength = listBox.length;
                    //alert(tLength);

                    var no = new Option();
                    no.value = ids.value;
                    no.text = userid.value;
                    listBox[tLength] = no;
                    var recipient = document.getElementById('txtSubsubcriptionIDBranch');
                    recipient.value = '';
                }
                else
                    alert('Please search name and then Add!')
                var s = document.getElementById('txtSubsubcriptionIDBranch');
                s.focus();
                s.select();
            }
            else if (cmb.value == 'System JVs') {
                var userid = document.getElementById('txtSystemJvs');
                if (userid.value != '') {
                    var ids = document.getElementById('txtSystemJvs_hidden');
                    var listBox = document.getElementById('lstSuscriptions');
                    var tLength = listBox.length;
                    //alert(tLength);

                    var no = new Option();
                    no.value = ids.value;
                    no.text = userid.value;
                    listBox[tLength] = no;
                    var recipient = document.getElementById('txtSystemJvs');
                    recipient.value = '';

                }
                else
                    alert('Please search name and then Add!')
                var s = document.getElementById('txtSystemJvs');
                s.focus();
                s.select();

            }
            else if (cmb.value != 'Sub Ac') {
                var userid = document.getElementById('txtsubscriptionID');
                if (userid.value != '') {
                    var ids = document.getElementById('txtsubscriptionID_hidden');
                    var listBox = document.getElementById('lstSuscriptions');
                    var tLength = listBox.length;
                    //alert(tLength);

                    var no = new Option();
                    no.value = ids.value;
                    no.text = userid.value;
                    listBox[tLength] = no;
                    var recipient = document.getElementById('txtsubscriptionID');
                    recipient.value = '';
                }
                else
                    alert('Please search name and then Add!')
                var s = document.getElementById('txtsubscriptionID');
                s.focus();
                s.select();
                //var AcVal=document.getElementById('ddlAccountType').value;
                //if(AcVal=='3')
                //clientselectionfinal();
            }

            else {

                var userid = document.getElementById('txtSubsubcriptionID');
                if (userid.value != '') {
                    var ids = document.getElementById('txtSubsubcriptionID_hidden');
                    var listBox = document.getElementById('lstSuscriptions');
                    var tLength = listBox.length;
                    //alert(tLength);

                    var no = new Option();
                    no.value = ids.value;
                    no.text = userid.value;
                    listBox[tLength] = no;
                    var recipient = document.getElementById('txtSubsubcriptionID');
                    recipient.value = '';
                }
                else
                    alert('Please search name and then Add!')
                var s = document.getElementById('txtSubsubcriptionID');
                s.focus();
                s.select();
            }
        }
        function btnRemovefromsubscriptionlist_click() {

            var listBox = document.getElementById('lstSuscriptions');
            var tLength = listBox.length;

            var arrTbox = new Array();
            var arrLookup = new Array();
            var i;
            var j = 0;
            for (i = 0; i < listBox.options.length; i++) {
                if (listBox.options[i].selected && listBox.options[i].value != "") {

                }
                else {
                    arrLookup[listBox.options[i].text] = listBox.options[i].value;
                    arrTbox[j] = listBox.options[i].text;
                    j++;
                }
            }
            listBox.length = 0;
            for (i = 0; i < j; i++) {
                var no = new Option();
                no.value = arrLookup[arrTbox[i]];
                no.text = arrTbox[i];
                listBox[i] = no;
            }
        }
        function CallGeneratePrint() {
            //  alert(dtTo.GetDate());
            CallServer("GeneratePrint~");
            //CallServer("GeneratePrint~"+dtFrom.GetDate(),+dtTo.GetDate(),"");
        }
        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSuscriptions');
            var cmb = document.getElementById('cmbsearchOption');
            var listIDs = '';
            var i;
            if (listBoxSubs.length > 0) {
                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '')
                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                }
                var sendData = cmb.value + '~' + listIDs;
                CallServer(sendData, "");
            }
            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }
            document.getElementById('showFilter').style.display = 'none';
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('Button1').disabled = false;
        }
        function Disable(obj) {
            var gridview = document.getElementById('grdCashBankBook');
            var rCount = gridview.rows.length;
            if (rCount < 10)
                rCount = '0' + rCount;
            if (obj == 'P') {
                document.getElementById("grdCashBankBook_ctl09_FirstPage").style.display = 'none';
                document.getElementById("grdCashBankBook_ctl09_PreviousPage").style.display = 'none';
                document.getElementById("grdCashBankBook_ctl09_NextPage").style.display = 'inline';
                document.getElementById("grdCashBankBook_ctl09_LastPage").style.display = 'inline';
            }
            else {
                document.getElementById("grdCashBankBook_ctl" + rCount + "_NextPage").style.display = 'none';
                document.getElementById("grdCashBankBook_ctl" + rCount + "_LastPage").style.display = 'none';
            }
        }
        function search1() {
            document.getElementById('TrSearch').style.display = 'inline';
            document.getElementById('TrSearch1').style.display = 'inline';
            document.getElementById('TrSearch2').style.display = 'inline';
            document.getElementById('spanBtn').style.display = 'inline';
            height();
        }
        function ShowHideSearch() {
            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("TdAll1").style.display = 'none';
            document.getElementById("TrBtn").style.display = 'none';
            document.getElementById('TrSearch').style.display = 'none';
            document.getElementById('TrSearch1').style.display = 'none';
            document.getElementById('TrSearch2').style.display = 'none';
            document.getElementById('txtVouno').value = 'Voucher Number';
            //document.getElementById('Tr_Location').style.display='none';
            document.getElementById('tr_hdrall').style.display = 'none';
            document.getElementById('tr_ftrall').style.display = 'none';
            document.getElementById('BtnGenerate').style.display = 'none';
            //document.getElementById('dtSearchDate_I').GetValue() ='01-01-0100';
            height();
        }
        function btnCancel_Click() {
            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("TdAll1").style.display = 'none';
            document.getElementById("TrBtn").style.display = 'none';
            document.getElementById('TrForFilter').style.display = 'inline';
            document.getElementById('showDetail').style.display = 'inline';
            document.getElementById('spanBtn').style.display = 'none';
            //document.getElementById('Tr_Location').style.display='none';
            document.getElementById('tr_hdrall').style.display = 'none';
            document.getElementById('tr_ftrall').style.display = 'none';
            document.getElementById('BtnGenerate').style.display = 'none';
            height();
        }
        function alertMessage() {
            //document.getElementById('Div1').style.display="none";
            alert('No Matching Clients !');
            document.getElementById('spanBtn').style.display = 'none';
            document.getElementById('TrForFilter').style.display = 'none';
            document.getElementById('showDetail').style.display = 'none';
            document.getElementById('TrPrevNext').style.display = 'none';
            document.getElementById('HdnSelectLedger').value = 'P';
            height();
        }
        function DisableC(obj) {
            if (obj == 'P') {
                document.getElementById('lnkPrevClient').style.display = 'none';
                document.getElementById('lnkNextClient').style.display = 'inline';
            }
            else {
                document.getElementById('lnkPrevClient').style.display = 'inline';
                document.getElementById('lnkNextClient').style.display = 'none';
            }
        }
        function dateChange() {
            //            var SubChk=document.getElementById('rdSubAcAll').checked;
            //            if(SubChk==true)
            //            {
            //                if(dateTimeForSubledger!='a')
            //                {
            //                    var dtFrom1=Date.parse(dtFrom.GetDate());
            //                    var dtTo1=Date.parse(dtTo.GetDate());
            //                    var dateDiff=(dtTo1-dtFrom1)/(24*60*60*1000)
            //                    if(dateDiff>30)
            //                    {
            //                        alert('You can view ledger for all accounts only for a period of 30 days.\n Please Select another date range !!');
            //                        var datePost=(dtFrom.GetDate().getMonth()+2)+'-'+dtFrom.GetDate().getDate()+'-'+dtFrom.GetDate().getYear();
            //                        dtTo.SetDate(new Date(datePost));
            //                    }
            //                }
            //            }
            //document.getElementById('Button1').disabled=true;
            //            var btn = document.getElementById('ButtonUpdate');
            //            btn.click();
        }
        document.body.style.cursor = 'pointer';
        var oldColor = '';
        function ChangeRowColor(rowID, rowNumber) {
            var gridview = document.getElementById('grdCashBankBook');
            var rCount = gridview.rows.length;
            var rowIndex = 1;
            var rowCount = 0;
            if (rCount == 28)
                rowCount = 25;
            else
                rowCount = rCount - 2;
            if (rowNumber > 25 && rCount < 28)
                rowCount = rCount - 3;
            for (rowIndex; rowIndex <= rowCount; rowIndex++) {
                var rowElement = gridview.rows[rowIndex];
                rowElement.style.backgroundColor = '#FFFFFF';
            }
            var color = document.getElementById(rowID).style.backgroundColor;
            if (color != '#ffe1ac') {
                oldColor = color;
            }
            if (color == '#ffe1ac') {
                document.getElementById(rowID).style.backgroundColor = oldColor;
            }
            else
                document.getElementById(rowID).style.backgroundColor = '#ffe1ac';

        }
        function HideoffOnButton() {
            //document.getElementById('Div1').style.display="none";
            document.getElementById('Button1').disabled = false;
        }
        function HideOnOffLoading() {
            //document.getElementById('Div1').style.display="inline";
        }
        function DisabledDrp(obj) {
            if (obj == 'a')
                document.getElementById('TrPrevNext').style.display = "none";
            else
                document.getElementById('TrPrevNext').style.display = "inline";
            //            var valMainAc=document.getElementById('txtMainAccount_hidden');
            //            var valMainAc1=document.getElementById('txtMainAccount');
            //            valMainAc.value="";
            //            valMainAc1.value="";
        }
        function selecttion() {
            var combo = document.getElementById('ddlExport');
            combo.value = 'Ex';
        }
        function DateChangeForFrom() {
            //var datePost=(dtFrom.GetDate().getMonth()+2)+'-'+dtFrom.GetDate().getDate()+'-'+dtFrom.GetDate().getYear();

            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtFrom.GetDate().getMonth() + 1;
            var DayDate = dtFrom.GetDate().getDate();
            var YearDate = dtFrom.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();

            if (YearDate >= objsession[0]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                dtFrom.SetDate(new Date(datePost));
            }

        }
        function DateChangeForTo() {

            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtTo.GetDate().getMonth() + 1;
            var DayDate = dtTo.GetDate().getDate();
            var YearDate = dtTo.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();

            if (YearDate <= objsession[1]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    dtTo.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    dtTo.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    dtTo.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                dtTo.SetDate(new Date(datePost));
            }
        }
        function updateCashBankDetail(objDate, objVouNo, objMainID, objSubID, objCompID, objSegID) {

            if (document.getElementById("HDNMAIN").value != '') {

                var combo = document.getElementById('ddlExport');
                combo.value = 'Ex';
                document.getElementById('HdnSelectLedger').value = 'S';
                var URL = 'CashBankEntryEdit.aspx?date=' + objDate + ' &vNo=' + objVouNo + ' &Compid=' + objCompID + ' &SegID=' + objSegID;
                editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Cash/Bank Edit Form", "width=960px,height=450px,center=1,resize=1,top=0", "recal");

            }
            else {
                var combo = document.getElementById('ddlExport');
                combo.value = 'Ex';
                document.getElementById('HdnSelectLedger').value = 'S';
                var URL = 'CashBankEntryEdit.aspx?date=' + objDate + ' &vNo=' + objVouNo + ' &Compid=' + objCompID + ' &SegID=' + objSegID;
                OnMoreInfoClick(URL, "Cash/Bank Edit Form", '950px', '500px', "Y");
            }

        }
        function updateJournalDetail(objDate, objVouNo, objMainID, objSubID, objCompID, objSegID) {
            if (document.getElementById("HDNMAIN").value != '') {
                var combo = document.getElementById('ddlExport');
                combo.value = 'Ex';
                document.getElementById('HdnSelectLedger').value = 'S';
                var URL = 'JournalVoucherLedgerEdit.aspx?date=' + objDate + ' &id=' + objVouNo + ' &Compid=' + objCompID + ' &exch=' + objSegID;
                editwin = dhtmlmodal.open("Editbox", "iframe", URL, "JournalVoucher Edit Form", "width=960px,height=450px,center=1,resize=1,top=0", "recal");


            }
            else {
                var combo = document.getElementById('ddlExport');
                combo.value = 'Ex';
                document.getElementById('HdnSelectLedger').value = 'S';
                var URL = 'JournalVoucherLedgerEdit.aspx?date=' + objDate + ' &id=' + objVouNo + ' &Compid=' + objCompID + ' &exch=' + objSegID;
                OnMoreInfoClick(URL, "JournalVoucher Edit Form", '950px', '500px', "Y");
            }
        }
        function ShowObligationBreakUp(TradeDate, SetN, SetType, MainID, SubID, CompID, SegID, SegmentName, TranID) {

            if (document.getElementById("HDNMAIN").value != '') {

                var combo = document.getElementById('ddlExport');
                combo.value = 'Ex';
                document.getElementById('HdnSelectLedger').value = 'S';
                //	            var URL='ShowScrip.aspx?Bill='+objBill +' &TranID='+objVouId+' &SegMentName='+objSegment+' &TranDate='+objTranDate;	        
                var URL = 'ShowScrip.aspx?TradeDate=' + TradeDate + ' &SetN=' + SetN + ' &SetType=' + SetType + ' &MainID=' + MainID + ' &SubID=' + SubID + ' &CompID=' + CompID + ' &SegID=' + SegID + ' &SegmentName=' + SegmentName + ' &TranID=' + TranID;
                editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Show Obligation BreakUp", "width=900px,height=350px,center=1,resize=1,top=500", "recal");

            }
            else {

                var combo = document.getElementById('ddlExport');
                combo.value = 'Ex';
                document.getElementById('HdnSelectLedger').value = 'S';
                var URL = 'ShowScrip.aspx?Bill=' + objBill + ' &TranID=' + objVouId + ' &SegMentName=' + objSegment + ' &TranDate=' + objTranDate;
                OnMoreInfoClick(URL, "Show Obligation BreakUp", '940px', '450px', "N");

            }
        }
        function callback() {
            var btn = document.getElementById('Button1');
            btn.click();
        }
        function DateVoucher(obj) {
            if (obj == 'a') {

                var segmentname = "<%=Session["SegMentName"]%>";

            var segsplit = segmentname.split('-')[1];

            if ((segsplit == 'FO') || (segsplit == 'CDX')) {
                if (document.getElementById('rdbSegAll').checked == false) {
                    document.getElementById('tdexpDateWise').style.display = "inline";
                    document.getElementById('tdexpDateWise1').style.display = "inline";
                }
                else {
                    document.getElementById('tdexpDateWise').style.display = "none";
                    document.getElementById('tdexpDateWise1').style.display = "none";
                }
            }
            else {

                document.getElementById('tdexpDateWise').style.display = "none";
                document.getElementById('tdexpDateWise1').style.display = "none";

            }

            document.getElementById('trDateVoucher').style.display = "inline";

        }
        else
            document.getElementById('trDateVoucher').style.display = "none";
    }
    function dateChange11() {
        //document.getElementById('Button1').disabled=true;
    }
    function fngrouptype(obj) {
        if (obj == "0") {
            document.getElementById('td_allselect').style.display = 'none';
            alert('Please Select Group Type !');
        }
        else {
            document.getElementById('td_allselect').style.display = 'inline';
        }
    }
    function fnddlGroup(obj) {
        if (obj == "0") {
            document.getElementById('td_group').style.display = 'none';
            document.getElementById('td_branch').style.display = 'inline';
        }
        else {
            document.getElementById('td_group').style.display = 'inline';
            document.getElementById('td_branch').style.display = 'none';
            var btn = document.getElementById('btnhide');
            btn.click();
        }
    }
    function FunClientScrip(objID, objListFun, objEvent) {
        var cmbVal;
        var SegN = document.getElementById('HDNSeg').value;
        var docVal = document.getElementById('ddlAccountType').value
        if (docVal == '0')
            Mainvalue = "'SYSTM00001'";
        else if (docVal == '1')
            Mainvalue = "'SYSTM00002'";
        else if (docVal == '2')
            Mainvalue = "'SYSTM00001','SYSTM00002'";
        else if (docVal == '3') {
            var NsdlCdsl = document.getElementById('HdnSubLedgerType').value;
            if (NsdlCdsl == "NSDL Clients")
                Mainvalue = "'SYSTM00043'";
            else if (NsdlCdsl == "CDSL Clients")
                Mainvalue = "'SYSTM00042'";

        }
        if (document.getElementById('cmbsearchOption').value == "Clients") {
            if (document.getElementById('ddlGroup').value == "0")//////////////Group By  selected are branch
            {
                if (document.getElementById('rdbranchAll').checked == true) {
                    cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                    cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue + '~' + SegN;
                }
                else {
                    cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                    cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue + '~' + Mainvalue + '~' + SegN;
                }
            }
            else //////////////Group By selected are Group
            {
                if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                    cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                    cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue + '~' + SegN;
                }
                else {
                    cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                    cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue + '~' + Mainvalue + '~' + SegN;
                }
            }
        }
        else {
            cmbVal = document.getElementById('cmbsearchOption').value;
            cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue + '~' + '~' + SegN;
        }
        //          } 
        ajax_showOptions(objID, objListFun, objEvent, cmbVal);
    }
    function OnlySubsidiaryTrial() {
        if (document.getElementById("HDNMAIN").value != '') {
            document.getElementById("trDateRange").style.display = 'inline';
            document.getElementById("ShowDt").style.display = 'none';
            document.getElementById("ShowHd").style.display = 'inline';
            document.getElementById("showDetail").style.display = 'none';

        }
        document.getElementById('showDetail').style.display = 'none';
        document.getElementById('TrBtn').style.display = 'none';
        document.getElementById('TrOnlySub').style.display = 'none';
        //document.getElementById('TrOnlySub1').style.display='none';
        document.getElementById('TrAll').style.display = 'none';
        document.getElementById('TdAll1').style.display = 'none';
    }
    function ShowBillPrintingCM(objDate, objSettno, objCliID, objSegment, objComp) {
        if (document.getElementById("HDNMAIN").value != '') {
            var URL = '../management/ObligationStatementCM.aspx?date=' + objDate + ' &SettNo=' + objSettno + ' &Compid=' + objComp + ' &SegID=' + objSegment + ' &ClientID=' + objCliID;
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Bill  For - " + objSettno + "", "width=900px,height=400px,center=1,resize=1,top=500", "recal");

        }
        else {
            var URL = '../management/ObligationStatementCM.aspx?date=' + objDate + ' &SettNo=' + objSettno + ' &Compid=' + objComp + ' &SegID=' + objSegment + ' &ClientID=' + objCliID;
            OnMoreInfoClick(URL, "Bill  For - " + objSettno + "", '940px', '450px', "N");
        }
    }
    function ShowBillPrintingFO(objDate, objCliID, objSegment, objComp) {
        if (document.getElementById("HDNMAIN").value != '') {
            var URL = '../management/ObligationStatementFO_New.aspx?date=' + objDate + ' &Compid=' + objComp + ' &SegID=' + objSegment + ' &ClientID=' + objCliID;
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Show FO Bill", "width=900px,height=400px,center=1,resize=1,top=500", "recal");
        }
        else {
            var URL = '../management/ObligationStatementFO_New.aspx?date=' + objDate + ' &Compid=' + objComp + ' &SegID=' + objSegment + ' &ClientID=' + objCliID;
            OnMoreInfoClick(URL, "Show FO Bill", '940px', '450px', "N");
        }
    }

    function ShowBillPrintingCMSubTrial(objDate, objSettno, objCliID, objSegment, objComp) {
        var URL = '../management/ObligationStatementCM.aspx?date=' + objDate + ' &SettNo=' + objSettno + ' &Compid=' + objComp + ' &SegID=' + objSegment + ' &ClientID=' + objCliID;
        if (document.getElementById("HDNMAIN").value != '') {
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Bill For - " + objSettno + "", "width=900px,height=400px,center=1,resize=1,top=500", "recal");
        }
        else {
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Bill For - " + objSettno + "", "width=940px,height=450px,center=1,resize=1,top=500", "recal");
        }
    }
    function ShowBillPrintingFOSubTrial(objDate, objCliID, objSegment, objComp) {
        var URL = '../Reports/ObligationStatementFO.aspx?date=' + objDate + ' &Compid=' + objComp + ' &SegID=' + objSegment + ' &ClientID=' + objCliID;
        if (document.getElementById("HDNMAIN").value != '') {
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Show FO Bill", "width=900px,height=400px,center=1,resize=1,top=500", "recal");
        }
        else {
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Show FO Bill", "width=940px,height=450px,center=1,resize=1,top=500", "recal");
        }
    }
    </script>

    <script type="text/ecmascript">

        function ReceiveServerData(rValue) {

            var Data = rValue.split('~');
            //            if(Data[0]=='Segment')
            //            {
            //                document.getElementById('HdnBranch').value=Data[1];
            //            }

            if (Data[0] == 'Segment') {

                var combo = document.getElementById('litSegment');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                var seg = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    if (val == '') {
                        seg = items[0];
                        val = "'" + items[1] + "'";

                    }
                    else {
                        seg += ',' + items[0];
                        val += ",'" + items[1] + "'";

                    }
                }

                var sename = val.split(',');

                var distinctsegname = '';
                var breakcount = '';
                var replacecont = '';


                document.getElementById('HdnBranch').value = seg;
                combo.innerText = val;
                document.getElementById('HDNSeg').value = val;
                for (m = 0; m < sename.length; m++) {

                    distinctsegname = sename[m].split('-');
                    if (breakcount == '') {
                        breakcount = distinctsegname[1];
                        if (breakcount.trim().replace("'", "") == "COMM") {
                            document.getElementById('tdexpDateWise').style.display = "none";
                            document.getElementById('tdexpDateWise1').style.display = "none";
                            break;
                        }
                        else if (breakcount.trim().replace("'", "") == "CM") {
                            document.getElementById('tdexpDateWise').style.display = "none";
                            document.getElementById('tdexpDateWise1').style.display = "none";
                            break;
                        }
                        else {
                            document.getElementById('tdexpDateWise').style.display = "inline";
                            document.getElementById('tdexpDateWise1').style.display = "inline";

                        }
                    }
                    else {
                        if (breakcount != sename[m].split('-')[1]) {
                            document.getElementById('tdexpDateWise').style.display = "none";
                            document.getElementById('tdexpDateWise1').style.display = "none";
                            break;
                        }
                        else {


                            if (sename[m].split('-')[1].trim().replace("'", "") != "CM") {

                                document.getElementById('tdexpDateWise').style.display = "inline";
                                document.getElementById('tdexpDateWise1').style.display = "inline";
                            }
                            else {

                                document.getElementById('tdexpDateWise').style.display = "none";
                                document.getElementById('tdexpDateWise1').style.display = "none";
                                break;
                            }
                            if (sename[m].split('-')[1].trim().replace("'", "") != "COMM") {

                                document.getElementById('tdexpDateWise').style.display = "inline";
                                document.getElementById('tdexpDateWise1').style.display = "inline";
                            }
                            else {

                                document.getElementById('tdexpDateWise').style.display = "none";
                                document.getElementById('tdexpDateWise1').style.display = "none";
                                break;
                            }
                        }
                    }
                }

            }

            if (Data[0] == 'Branch') {
                groupvalue = Data[1];
                document.getElementById('HdnSegment').value = Data[1];
            }
            if (Data[0] == 'Sub Ac') {

                document.getElementById('HdnSubAc').value = '';
                document.getElementById('HdnSubAc').value = Data[1];
            }
            if (Data[0] == 'Clients') {
                document.getElementById('HdnSubAc').value = '';
                //groupvalue=Data[1];
                document.getElementById('HdnSubAc').value = Data[1];
            }
            if (Data[0] == 'Group') {
                groupvalue = Data[1];
                document.getElementById('HdnGroup').value = Data[1];
            }
            if (Data[0] == 'Ac Name') {
                Mainvalue = Data[1];
                document.getElementById('HdnMainAc').value = Data[1];
                document.getElementById('HdnSubLedgerType').value = Data[2];
            }
            if (Data[0] == 'Settlement') {

                document.getElementById('HdnSettlement').value = Data[1];


            }
            if (Data[0] == 'Employee') {

                document.getElementById('HDNEmp').value = Data[1];


            }
            if (Data[0] == 'System JVs') {
                document.getElementById('hdnSystemJvs').value = Data[1];
            }
            if (Data[0] == 'GeneratePrint') {
                //alert ('adcgkjfkj');
                Page_Load();
            }



        }
        function ChkCheckProperty(obj, objChk) {

            if (objChk == true) {
                if (obj == 'H')
                    document.getElementById('tdHeader').style.display = 'inline';
                else if (obj == 'F')
                    document.getElementById('tdfooter').style.display = 'inline';
            }
            else {
                if (obj == 'H')
                    document.getElementById('tdHeader').style.display = 'none';
                else if (obj == 'F')
                    document.getElementById('tdfooter').style.display = 'none';
            }
        }


        function fn_ddllistType(obj) {
            if (obj == '0') {

                document.getElementById('btnPrint').style.display = 'none';
                document.getElementById('Button1').style.display = 'inline';
                document.getElementById('TrPrint').style.display = 'none';
                document.getElementById('TrLogo').style.display = 'none';
                document.getElementById('MailTo').style.display = 'none';
                document.getElementById('btnSend').style.display = 'none';
                document.getElementById('btnExExport').style.display = 'none';
                //document.getElementById('Tr_Location').style.display='none';
                document.getElementById('tr_hdrall').style.display = 'none';
                document.getElementById('tr_ftrall').style.display = 'none';
                document.getElementById('BtnGenerate').style.display = 'none';

            }
            else if (obj == '1') {
                document.getElementById('btnPrint').style.display = 'inline';
                document.getElementById('Button1').style.display = 'none';
                document.getElementById('TrPrint').style.display = 'inline';
                document.getElementById('TrLogo').style.display = 'inline';
                document.getElementById('MailTo').style.display = 'none';
                document.getElementById('btnSend').style.display = 'none';
                document.getElementById('btnExExport').style.display = 'none';
                //document.getElementById('Tr_Location').style.display='none';
                document.getElementById('tr_hdrall').style.display = 'none';
                document.getElementById('tr_ftrall').style.display = 'none';
                document.getElementById('BtnGenerate').style.display = 'none';

            }
            else if (obj == '2') {
                document.getElementById('btnPrint').style.display = 'none';
                document.getElementById('Button1').style.display = 'none';
                document.getElementById('TrPrint').style.display = 'none';
                document.getElementById('TrLogo').style.display = 'none';
                document.getElementById('MailTo').style.display = 'inline';
                document.getElementById('btnSend').style.display = 'inline';
                document.getElementById('btnExExport').style.display = 'none';
                //document.getElementById('Tr_Location').style.display='none';
                document.getElementById('tr_hdrall').style.display = 'none';
                document.getElementById('tr_ftrall').style.display = 'none';
                document.getElementById('BtnGenerate').style.display = 'none';
            }
            else if (obj == '3') {
                document.getElementById('btnPrint').style.display = 'none';
                document.getElementById('Button1').style.display = 'none';
                document.getElementById('TrPrint').style.display = 'none';
                document.getElementById('TrLogo').style.display = 'none';
                document.getElementById('MailTo').style.display = 'none';
                document.getElementById('btnSend').style.display = 'none';
                document.getElementById('btnExExport').style.display = 'inline';
                //document.getElementById('Tr_Location').style.display='none';
                document.getElementById('tr_hdrall').style.display = 'none';
                document.getElementById('tr_ftrall').style.display = 'none';
                document.getElementById('BtnGenerate').style.display = 'none';

            }
            else if (obj == '4') {
                document.getElementById('btnPrint').style.display = 'none';
                document.getElementById('Button1').style.display = 'none';
                document.getElementById('TrPrint').style.display = 'none';
                document.getElementById('TrLogo').style.display = 'none';
                document.getElementById('MailTo').style.display = 'none';
                document.getElementById('btnSend').style.display = 'none';
                document.getElementById('btnExExport').style.display = 'none';
                //document.getElementById('Tr_Location').style.display='inline';
                document.getElementById('tr_hdrall').style.display = 'inline';
                document.getElementById('tr_ftrall').style.display = 'inline';

                document.getElementById('td_hdr').style.display = 'inline';

                document.getElementById('tdfooter').style.display = 'none';

                document.getElementById('tdHeader').style.display = 'none';

                document.getElementById('td_footer').style.display = 'inline';


                document.getElementById('BtnGenerate').style.display = 'inline';


            }

        }

        function TransactionType(trantype) {
            if (trantype == 'all') {
                document.getElementById('trCb').style.display = 'none';
                document.getElementById('trJv').style.display = 'none';
                document.getElementById('trManualJvOption').style.display = 'none';
            }
            else if (trantype == 'cb') {
                document.getElementById('trCb').style.display = 'inline';
                document.getElementById('trJv').style.display = 'none';
                document.getElementById('trManualJvOption').style.display = 'none';
            }
            else if (trantype == 'jv') {
                document.getElementById('trCb').style.display = 'none';
                document.getElementById('trJv').style.display = 'inline';
                document.getElementById('rbAllJV').checked = true;

            }


        }
        function DateChange(positionDate) {
            var FYS = '<%=Session["FinYearStart"]%>';
            var FYE = '<%=Session["FinYearEnd"]%>';
            var LFY = '<%=Session["LastFinYear"]%>';
            DevE_CheckForFinYear(positionDate, FYS, FYE, LFY);
            if (dtFrom.GetDate() == positionDate.GetDate()) {
                var setFromDate = '<%=fDate%>';
            CompareDate(dtFrom.GetDate(), dtTo.GetDate(), 'LE', 'From Date Can Not Be Greater Than To Date', dtFrom, setFromDate);
        }
        else if (dtTo.GetDate() == positionDate.GetDate()) {
            var setToDate = '<%=tDate%>';
                    CompareDate(dtFrom.GetDate(), dtTo.GetDate(), 'LE', 'To Date Can Not Be Less Than From Date', dtTo, setToDate);
                }
        }

        function FetchSystemJvs(obj1, obj2, obj3) {

            var strQuery_Table = "master_vouchertype";
            var strQuery_FieldName = " top 10 VoucherType_Name+' [ '+rtrim(ltrim(VoucherType_Code))+' ]' as VouchaerName,VoucherType_Code as voucherCode";
            var strQuery_WhereClause = " (VoucherType_Name like (\'%RequestLetter%\') or VoucherType_Code like (\'%RequestLetter%\')) ";;
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

        function SelectJvs() {
            document.getElementById('showFilter').style.display = 'inline';
            document.getElementById('TdFilter').style.display = 'inline';

            document.getElementById('cmbsearchOption').value = 'System JVs';
            document.getElementById('spanSub').style.display = "none";
            document.getElementById('spanall').style.display = "none";
            document.getElementById('span2').style.display = "none";
            document.getElementById('spanSystemjv').style.display = "inline";
            document.getElementById('Button1').disabled = true;

        }
        function FunHeaderFooter(objID, objListFun, objEvent, objParam) {
            ajax_showOptions(objID, objListFun, objEvent, objParam);
        }

        function OptionsJv(jvtype) {
            if (jvtype == 'all') {
                document.getElementById('trManualJvOption').style.display = "none";
                document.getElementById('trSystemJvOptions').style.display = "none";

            }
            else if (jvtype == 'man') {
                document.getElementById('trManualJvOption').style.display = "inline";
                document.getElementById('trSystemJvOptions').style.display = "none";

            }
            else if (jvtype == 'sys') {
                document.getElementById('trManualJvOption').style.display = "none";
                document.getElementById('trSystemJvOptions').style.display = "inline";


            }

        }

        function ManageMJV() {
            var txtMjv = document.getElementById('txtVoucherPrefix');
            if (txtMjv.value == '??')
                txtMjv.value = '';


        }
        function ManageMJV1() {
            var txtMjv = document.getElementById('txtVoucherPrefix');
            if (txtMjv.value == '')
                txtMjv.value = '??';


        }
        function FilterInput(val, event) {
            var chCode = ('charCode' in event) ? event.charCode : event.keyCode;

            if (val.length == 0) {
                if ((chCode > 116 && chCode < 123) || (chCode > 84 && chCode < 91))
                    return false;
                else
                    return true;
            }
            else
                return true;
        }



        //        function printd()
        //       {
        //       javascript:self.print();
        //        alert('1');
        //       }

    </script>


    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Reports Ledger View</h3>
        </div>
    </div>
    <div class="form_main inner">
        <%--<table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Reports Ledger View</span></strong>
                </td>
            </tr>
        </table>--%>
        <div>
            <script type="text/javascript">
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);
                var postBackElement;
                function InitializeRequest(sender, args) {
                    if (prm.get_isInAsyncPostBack())

                        args.set_cancel(true);
                    postBackElement = args.get_postBackElement();
                    $get('UpdateProgress1').style.display = 'block';

                }
                function EndRequest(sender, args) {
                    $get('UpdateProgress1').style.display = 'none';
                }
            </script>

            <table class="TableMain100">
                <tr id="TdAll1">
                    <td colspan="2">
                        <table width="100%">
                            <tr>
                                <td class="gridcellleft" style="vertical-align: top; text-align: right" id="TdFilter"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="TrAll">
                    <td style="text-align: left; vertical-align: top;">
                        <table cellspacing="1" cellpadding="2"
                           >
                            <tr>
                                <td class="gridcellleft">Account Type :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAccountType" runat="server" Width="154px" Font-Size="12px">
                                        <asp:ListItem Value="0">Trading</asp:ListItem>
                                        <asp:ListItem Value="1">Margin Deposit</asp:ListItem>
                                        <asp:ListItem Value="2">Both</asp:ListItem>
                                        <asp:ListItem Value="3">Other Account</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft" id="TdAc">A/C Name
                                </td>
                                <td style="text-align: left;" id="TdAc1">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtMainAccount" runat="server" Font-Size="12px" onkeyup="showOptions1(this,'SearchMainAccountBranchSegment',event)"
                                                    Width="198px"></asp:TextBox>
                                                <asp:HiddenField ID="txtMainAccount_hidden" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trSeg" runat="server">
                                <td class="gridcellleft">Segment:
                                </td>
                                <td style="text-align: left;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbSegAll" runat="server" GroupName="a" onclick="SegAll('date')" />
                                            </td>
                                            <td>All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbSegSelected" runat="server" Checked="True" GroupName="a" onclick="SegSelected('AS')" />
                                            </td>
                                            <td>Selected
                                            </td>
                                            <td>(<span id="litSegment" runat="server" style="color: Maroon"></span>)
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrForGroup">
                                <td class="gridcellleft">Group By:</td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                    <asp:ListItem Value="0">Branch</asp:ListItem>
                                                    <asp:ListItem Value="1">Group</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td id="td_branch">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a11" onclick="SegAll('B')" />
                                                            All
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a11" onclick="SegSelected('B')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td id="td_group" style="display: none;" colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlgrouptype" runat="server" Font-Size="12px" onchange="fngrouptype(this.value)">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td id="td_allselect" style="display: none;">
                                                            <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="b"
                                                                onclick="SegAll('G')" />
                                                            All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="SegSelected('G')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrForClient">
                                <td class="gridcellleft">Client :</td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c11"
                                                    onclick="SegAll('SAC')" /></td>
                                            <td>All Client</td>
                                            <td>
                                                <asp:RadioButton ID="radPOAClient" runat="server" GroupName="c11" onclick="SegAll('SAC')" />
                                            </td>
                                            <td>POA Client</td>
                                            <td>
                                                <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c11" onclick="SegSelected('SAC')" /></td>
                                            <td>Selected Client</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrSubAccount">
                                <td class="gridcellleft">Sub A/C:
                                </td>
                                <td style="text-align: left;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdSubAcAll" runat="server" Checked="True" GroupName="c" />
                                            </td>
                                            <td>All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdSubAcSelected" runat="server" GroupName="c" />
                                            </td>
                                            <td>Selected
                                            </td>
                                            <td>
                                                <span id="Span1" runat="server" style="color: Maroon"></span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">Report Type:
                                </td>
                                <td style="text-align: left;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radConsolidated" runat="server" onclick="DateVoucher('a')" GroupName="d" />
                                            </td>
                                            <td>Consolidated
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radDetail" runat="server" onclick="DateVoucher('b')" Checked="True"
                                                    GroupName="d" />
                                            </td>
                                            <td>Detail
                                            </td>
                                            <td align="left">
                                                <table>
                                                    <tr id="TrOB">
                                                        <td>
                                                            <asp:RadioButton ID="radBreakDetail" runat="server" onclick="DateVoucher('b')" GroupName="d" />
                                                        </td>
                                                        <td>Obligation Breakup
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trDateVoucher">
                                <td class="gridcellleft">Summarise
                                </td>
                                <td style="text-align: left;" colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radDateWise" runat="server" Checked="True" GroupName="e" />
                                            </td>
                                            <td>Date Wise
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radVoucherWise" runat="server" GroupName="e" />
                                            </td>
                                            <td>Voucher Wise
                                            </td>
                                            <td id="tdexpDateWise">
                                                <asp:RadioButton ID="radExpDateWise" runat="server" GroupName="e" />
                                            </td>
                                            <td id="tdexpDateWise1">Exp Date Wise
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrSett" runat="server">
                                <td class="gridcellleft">Settlement No.:
                                </td>
                                <td style="text-align: left;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RadSettA" runat="server" Checked="True" GroupName="P" />
                                            </td>
                                            <td>All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadSettS" runat="server" GroupName="P" />
                                            </td>
                                            <td>Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">Transaction Type
                                </td>
                                <td style="text-align: left;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rbTanAll" runat="Server" GroupName="TranType" onclick="TransactionType('all');"
                                                    Checked="true" />
                                            </td>
                                            <td>All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbTranCashBank" runat="Server" GroupName="TranType" onclick="TransactionType('cb');" />
                                            </td>
                                            <td>Cash Bank
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbTranJv" runat="Server" GroupName="TranType" onclick="TransactionType('jv');" />
                                            </td>
                                            <td>Journal Vouchar
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trCb">
                                <td></td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkPayment" runat="Server" Checked="true" />
                                            </td>
                                            <td>Payments
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkReceipts" runat="Server" Checked="true" />
                                            </td>
                                            <td>Receipts
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkContracts" runat="Server" Checked="true" />
                                            </td>
                                            <td>Contra
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trJv">
                                <td></td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rbAllJV" runat="server" GroupName="jvoptions" onclick="OptionsJv('all')"
                                                    Checked="true" />
                                            </td>
                                            <td>All Jvs
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbManualJV" runat="server" GroupName="jvoptions" onclick="OptionsJv('man')" />
                                            </td>
                                            <td>Only Manual Jvs
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbSystemJV" runat="server" GroupName="jvoptions" onclick="OptionsJv('sys')" />
                                            </td>
                                            <td>Only System Jvs
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trManualJvOption">
                                <td>Voucher Prefix:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVoucherPrefix" MaxLength="2" Width="25px" runat="server" onblur="ManageMJV1()"
                                        Text="??" onkeypress="return FilterInput(this.value,event)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trSystemJvOptions">
                                <td></td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rbSystemJVAll" runat="Server" GroupName="systemjvs" Checked="true"
                                                    onclick="SegAll('jv')" />
                                            </td>
                                            <td>All Types
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbSystemJVSelected" GroupName="systemjvs" onclick="SelectJvs()"
                                                    runat="Server" />
                                            </td>
                                            <td>Selected JVs
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">Date Range:
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 110px">
                                                <dxe:ASPxDateEdit ID="dtFrom" runat="server" ClientInstanceName="dtFrom" EditFormat="Custom"
                                                    UseMaskBehavior="True" Font-Size="12px" Width="108px" DateOnError="Today">
                                                    <DropDownButton Text="From">
                                                    </DropDownButton>
                                                    <%--<ClientSideEvents ValueChanged="function(s,e){DateChangeForFrom();}" GotFocus="function(s,e){dateChange11();}" />--%>
                                                    <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom(dtFrom,dtTo);}" />
                                                    <ClientSideEvents DateChanged="function(s,e){DateChange(dtFrom);}"></ClientSideEvents>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td style="width: 110px">
                                                <dxe:ASPxDateEdit ID="dtTo" runat="server" ClientInstanceName="dtTo" EditFormat="Custom"
                                                    UseMaskBehavior="True" Font-Size="12px" Width="98px" DateOnError="Today">
                                                    <DropDownButton Text="To">
                                                    </DropDownButton>
                                                    <%--<ClientSideEvents ValueChanged="function(s,e){DateChangeForTo();}" GotFocus="function(s,e){dateChange11();}" />--%>
                                                    <ClientSideEvents DateChanged="function(s,e){DateChangeForTo(dtFrom,dtTo);}" />
                                                    <ClientSideEvents DateChanged="function(s,e){DateChange(dtTo);}"></ClientSideEvents>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">Show Value Date</td>
                                <td>
                                    <asp:CheckBox ID="ChkShowValueDate" runat="server" Checked="True" /></td>
                            </tr>
                            <tr id="TrCA">
                                <td class="gridcellleft">View By
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddllistType" runat="server" Width="120px" Font-Size="12px"
                                        onchange="fn_ddllistType(this.value)">
                                        <asp:ListItem Value="0">Screen</asp:ListItem>
                                        <asp:ListItem Value="1">PDF</asp:ListItem>
                                        <asp:ListItem Value="2">Email</asp:ListItem>
                                        <asp:ListItem Value="3">Excel</asp:ListItem>
                                        <asp:ListItem Value="4">Dos Print</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <%-- <td>
                                                <asp:DropDownList ID="DropDownList1" runat="server" Width="300px" Font-Size="12px" >
                                                 
                                                </asp:DropDownList>
                                            </td>--%>
                            <%--<tr id="Tr_Location" runat="server" >
                                <td class="gridcellleft">
                                    Printer Selection :
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownList1" runat="server" Width="300px" Font-Size="12px">
                                    </asp:DropDownList>
                                </td>
                               </tr> --%>
                            <tr id="tr_hdrall">
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">Use Header :
                                            </td>
                                            <td id="td_hdr" runat="server">
                                                <asp:CheckBox ID="chkHeader" runat="server" onclick="ChkCheckProperty('H',this.checked);" />
                                            </td>
                                            <td id="tdHeader" runat="server">
                                                <asp:TextBox ID="txtHeader" runat="server" Width="279px" Font-Size="12px" onkeyup="FunHeaderFooter(this,'GetHeaderFooter',event,'H')"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr id="tr_ftrall">
                                            <td class="gridcellleft" bgcolor="#B7CEEC">Use Footer :
                                            </td>
                                            <td id="td_footer" runat="server">
                                                <asp:CheckBox ID="chkFooter" runat="server" onclick="ChkCheckProperty('F',this.checked);" />
                                            </td>
                                            <td id="tdfooter">
                                                <asp:TextBox ID="txtFooter" runat="server" Width="279px" Font-Size="12px" onkeyup="FunHeaderFooter(this,'GetHeaderFooter',event,'F')"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="MailTo">
                                <td class="gridcellleft">Mail To:
                                </td>
                                <td class="gridcellleft">
                                    <table>
                                        <tr>
                                            <td id="Rcb">
                                                <asp:RadioButton ID="ResClient" runat="server" GroupName="E1" onclick="SegAll('SAC')"
                                                    Checked="true" />
                                            </td>
                                            <td class="gridcellleft" id="Rct">Respective Client
                                            </td>
                                            <td id="Rbb">
                                                <asp:RadioButton ID="ResBranch" runat="server" onclick="SegAll('SAC')" GroupName="E1" />
                                            </td>
                                            <td class="gridcellleft" id="Rbt">Respective Branch/Group
                                            </td>
                                            <td id="Rub">
                                                <asp:RadioButton ID="ResUser" runat="server" GroupName="E1" onclick="SegSelected('Emp')" />
                                            </td>
                                            <td class="gridcellleft" id="Rut">Selected User
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrPrint">
                                <td class="gridcellleft" colspan="2">
                                    <asp:CheckBox ID="chkBothPrint" runat="server" />
                                    Both Side Print
                                </td>
                            </tr>
                            <tr id="TrLogo">
                                <td class="gridcellleft" colspan="2">
                                    <asp:CheckBox ID="ChkLogo" runat="server" Checked="true" />
                                    Use Company Logo
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: left; vertical-align: top;">
                        <table width="400px" id="showFilter">
                            <tr>
                                <td style="text-align: left; vertical-align: top">
                                    <table cellpadding="0" cellspacing="0" width="400px">
                                        <tr>
                                            <td align="LEFT">
                                                <table width="400px">
                                                    <tr>
                                                        <td align="left">
                                                            <span id="spanall">
                                                                <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="250px"></asp:TextBox></span>
                                                            <span id="spanSub">
                                                                <asp:TextBox ID="txtSubsubcriptionID" runat="server" Font-Size="12px" Width="250px"></asp:TextBox></span>
                                                            <span id="span2">
                                                                <asp:TextBox ID="txtSubsubcriptionIDBranch" runat="server" Font-Size="12px" onkeyup="FunClientScrip(this,'ShowFilterForLedger',event)"
                                                                    Width="250px"></asp:TextBox></span> <span id="spanSystemjv">
                                                                        <asp:TextBox ID="txtSystemJvs" runat="Server" Font-Size="12px" Width="250px" onkeyup="FetchSystemJvs(this,'GenericAjaxList',event)"></asp:TextBox>
                                                                    </span>
                                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                                Enabled="false">
                                                                <asp:ListItem>Segment</asp:ListItem>
                                                                <asp:ListItem>Branch</asp:ListItem>
                                                                <asp:ListItem>Ac Name</asp:ListItem>
                                                                <asp:ListItem>Sub Ac</asp:ListItem>
                                                                <asp:ListItem>Group</asp:ListItem>
                                                                <asp:ListItem>Clients</asp:ListItem>
                                                                <asp:ListItem>Settlement</asp:ListItem>
                                                                <asp:ListItem>Employee</asp:ListItem>
                                                                <asp:ListItem>System JVs</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                                    style="color: #009900; font-size: 8pt;"> </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="390px"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
                                                                <span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td>
                                    <asp:HiddenField ID="HDNMAIN" runat="server" />
                                    <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                    <asp:TextBox ID="txtSubsubcriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                    <asp:TextBox ID="txtSubsubcriptionIDBranch_hidden" runat="server" Font-Size="12px"
                                        Width="1px"></asp:TextBox>
                                    <asp:TextBox ID="txtSystemJvs_hidden" runat="Server" Width="1px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="TrOnlySub1">
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                        <ProgressTemplate>
                                            <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50px; background-color: white; layer-background-color: white; height: 80; width: 150;'>
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
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="TrOnlySub">
                    <td colspan="2" style="text-align: left; vertical-align: bottom;">
                        <table class="TableMain100">
                            <tr>
                                <td id="TrPrevNext">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="clientFilter" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lnkPrevClient" runat="server" CommandName="Prev" OnCommand="NavigationLinkC_Click"
                                                            OnClientClick="javascript:selecttion();" Text="[Prev]"> </asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbclientsPager" runat="server" CssClass="normaltxt" Width="200px"
                                                            Font-Size="12px" ValidationGroup="a" AutoPostBack="True" onchange="selecttion()"
                                                            OnSelectedIndexChanged="cmbclientsPager_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkNextClient" runat="server" CommandName="Next" OnCommand="NavigationLinkC_Click"
                                                            OnClientClick="javascript:selecttion();" Text="[Next]"> </asp:LinkButton>&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td class="gridcellright">
                                    <table width="100%">
                                        <tr style="display: none;">
                                            <td id="TrSearch">
                                                <dxe:ASPxDateEdit ID="dtSearchDate" runat="server" CssClass="water" NullText="Tr Date"
                                                    ToolTip="Tr Date" EditFormat="Custom" Font-Size="12px" UseMaskBehavior="True">
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td id="TrSearch1">
                                                <asp:TextBox ID="txtVouno" runat="server" CssClass="water" Text="Voucher Number"
                                                    ToolTip="Voucher Number" Font-Size="12px" Width="150px"></asp:TextBox>
                                            </td>
                                            <td id="TrSearch2">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btnUpdate" Height="17px"
                                                    OnClientClick="javascript:selecttion();" OnClick="btnSearch_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="TrForFilter" colspan="2">
                                                <a href="#" style="font-weight: bold; color: Blue" onclick="javascript:ForFilter();">Filter</a> ||
                                                <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged1">
                                                    <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                                                    <asp:ListItem Value="E">Excel</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="display: none;">
                                                <asp:CheckBox ID="chkDouble" runat="server" /><span style="color: Blue">Check here for
                                                    Both Page Printing , </span>
                                                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" ForeColor="Blue"><span style="font-weight: bold; color: Blue">Print</span> </asp:LinkButton>
                                                <a href="#" style="font-weight: bold; color: Blue" onclick="javascript:search1();">Search</a>
                                                ||
                                                <asp:LinkButton ID="btnEmail" runat="server" Font-Bold="True" Font-Underline="True"
                                                    ForeColor="Blue" OnClick="btnEmail_Click">SendEmail</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%-- <tr>
                    <td>
                        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                        ReportSourceID="CrystalReportSource1" PrintMode="ActiveX" BestFitPage="False" Height="3055px" SeparatePages="False" Width="901px"/>
                    <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                        <Report FileName="LedgerPrint.rpt">
                        </Report>
                    </CR:CrystalReportSource>
                    </td>
                </tr>--%>
                <tr id="TrBtn">
                    <td style="padding-left:100px">
                        <asp:Button ID="Button1" runat="server" Text="Show" CssClass="btnUpdate btn btn-primary" 
                            OnClientClick="javascript:selecttion();" OnClick="Button1_Click" />
                        <asp:Button ID="btnPrint" Text="Print" runat="server" CssClass="btnUpdate btn btn-primary" OnClick="btnPrint_Click"
                            TabIndex="6" />
                        <asp:Button ID="btnSend" Text="Send Email" runat="server" CssClass="btnUpdate btn btn-primary" OnClick="btnSend_Click"
                            TabIndex="6"  />
                        <asp:Button ID="btnExExport" runat="Server" Text="Export" OnClick="btnExExport_Click"
                            CssClass="btnUpdate btn btn-primary"  />
                        <span id="spanBtn" style="display: none;"><a href="#" style="font-weight: bold; color: Blue"
                            onclick="javascript:btnCancel_Click();">Cancel</a></span>
                        <asp:Button ID="BtnGenerate" runat="server" CssClass="btnUpdate btn btn-primary" Text="Generate"
                             OnClick="BtnGenerate_Click" /></td>

                    <%--<tr><td id="BtnGenerate" runat="server"><a id="A22" class="btnUpdate"  style="height:50px; width:101px" href="javascript:void(0);"  onclick="CallGeneratePrint();">
                                                                Generate</a>&nbsp;&nbsp;</td></tr></td>--%>
                    <%--<asp:Button ID="BtnGenerate" runat="server" CssClass="btnUpdate" Height="23px" Text="Generate"
                                    Width="101px" OnClientClick="CallGeneratePrint();" />--%>
                    <td style="display: none">
                        <asp:Button ID="ButtonUpdate" runat="server" Text="Button" OnClick="ButtonUpdate_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" id="showDetail">
                        <span id="spanshow" style="color: Blue; font-weight: bold"></span>&nbsp;&nbsp;&nbsp;&nbsp;
                        <span id="spanshow2" style="color: Blue; font-weight: bold">Period :</span>&nbsp;&nbsp;<span
                            id="spanshow3"></span>
                    </td>
                </tr>
                <tr id="trDateRange" style="display: none;">
                    <td colspan="2">
                        <table>
                            <tr id="ShowHd">
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <span id="SpanShowHeader" runat="server" style="font-weight: bold; color: Maroon"></span>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <a href="javascript:void(0);" onclick="ShowDateSelect()" style="font-weight: bold; color: Blue">Show Another Date Range</a>
                                </td>
                            </tr>
                            <tr id="ShowDt">
                                <td style="width: 110px">
                                    <dxe:ASPxDateEdit ID="dtFromG" runat="server" ClientInstanceName="dtFromG" EditFormat="Custom"
                                        UseMaskBehavior="True" Font-Size="12px" Width="108px">
                                        <DropDownButton Text="From">
                                        </DropDownButton>
                                        <ClientSideEvents ValueChanged="function(s,e){dateChange();DateChangeForFrom();}"
                                            GotFocus="function(s,e){dateChange11();}" />
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td style="width: 110px">
                                    <dxe:ASPxDateEdit ID="dtToG" runat="server" ClientInstanceName="dtToG" EditFormat="Custom"
                                        UseMaskBehavior="True" Font-Size="12px" Width="98px">
                                        <DropDownButton Text="To">
                                        </DropDownButton>
                                        <ClientSideEvents ValueChanged="function(s,e){dateChange();DateChangeForTo();}" GotFocus="function(s,e){dateChange11();}" />
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <asp:Button ID="Button2" runat="server" Text="Show" CssClass="btnUpdate" Width="100px"
                                        OnClientClick="javascript:selecttion();" OnClick="Button2_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%" style="background-color: #ffffff;">
                                    <tr>
                                        <td>
                                            <div id="DisplayOblg" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="grdCashBankBook" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                    ShowFooter="True" OnRowDataBound="grdCashBankSummary_RowDataBound" AllowSorting="true"
                                    AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="2px" CellPadding="4"
                                    ForeColor="#0000C0" OnRowCreated="grdCashBankBook_RowCreated" OnSorting="grdCashBankBook_Sorting">
                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Transaction Date" SortExpression="accountsledger_transactiondate">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrDate" runat="server" Text='<%# Eval("TrDate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Value Date">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblValueDate" runat="server" Text='<%# Eval("ValueDate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Document No." SortExpression="accountsledger_TransactionReferenceID">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("accountsledger_TransactionReferenceID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescrip" runat="server" Text='<%# Eval("accountsledger_Narration")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AccountName" Visible="false">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccName" runat="server" Text='<%# Eval("AccountName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Trade Date">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayoutDate" runat="server" Text='<%# Eval("PayoutDate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Branch Code">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranchCode" runat="server" Text='<%# Eval("BranchCode")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Settlement No." >
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSettNo" runat="server" Text='<%# Eval("SettlementNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Instrument No." SortExpression="accountsledger_InstrumentNumber">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInstNo" runat="server" Text='<%# Eval("accountsledger_InstrumentNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmtDr" runat="server" Text='<%# Eval("Accountsledger_AmountCr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmtCr" runat="server" Text='<%# Eval("Accountsledger_AmountDr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Closing">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblClosing" runat="server" Text='<%# Eval("Closing")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Closing" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTradeDate" runat="server" Text='<%# Eval("accountsledger_transactiondate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MainID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMainID" runat="server" Text='<%# Eval("MainID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SubID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubID" runat="server" Text='<%# Eval("SubID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MainID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompID" runat="server" Text='<%# Eval("CompanyID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SubID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSegID" runat="server" Text='<%# Eval("SegID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CashType" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCashType" runat="server" Text='<%# Eval("CashType")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="10">
                                                    <asp:LinkButton ID="FirstPage" runat="server" Font-Bold="true" CommandName="First"
                                                        OnClientClick="selecttion()" OnCommand="NavigationLink_Click" Text="[First Page]"> </asp:LinkButton>
                                                    <asp:LinkButton ID="PreviousPage" runat="server" Font-Bold="true" CommandName="Prev"
                                                        OnClientClick="selecttion()" OnCommand="NavigationLink_Click" Text="[Previous Page]">  </asp:LinkButton>
                                                    <asp:LinkButton ID="NextPage" runat="server" Font-Bold="true" CommandName="Next"
                                                        OnClientClick="selecttion()" OnCommand="NavigationLink_Click" Text="[Next Page]">
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="LastPage" runat="server" Font-Bold="true" CommandName="Last"
                                                        OnClientClick="selecttion()" OnCommand="NavigationLink_Click" Text="[Last Page]">
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </PagerTemplate>
                                    <RowStyle BackColor="#FFFFFF" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                        BorderWidth="1px"></RowStyle>
                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                        Font-Bold="False"></HeaderStyle>
                                    <%--<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>--%>
                                </asp:GridView>
                                <asp:HiddenField ID="CurrentPage" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="TotalPages" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="TotalClient" runat="server" />
                                <asp:HiddenField ID="HdnSelectLedger" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="cmbclientsPager" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="lnkPrevClient" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkNextClient" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSend" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="txtHeader_hidden" runat="server" />
                        <asp:HiddenField ID="txtFooter_hidden" runat="server" />
                        <asp:HiddenField ID="HdnBranch" runat="server" />
                        <asp:HiddenField ID="HdnSegment" runat="server" />
                        <asp:HiddenField ID="HdnMainAc" runat="server" />
                        <asp:HiddenField ID="HdnSubAc" runat="server" />
                        <asp:HiddenField ID="HdnSubLedgerType" runat="server" />
                        <asp:HiddenField ID="HdnGroup" runat="server" />
                        <asp:HiddenField ID="HdnForBranchGroup" runat="server" />
                        <asp:HiddenField ID="HdnSettlement" runat="server" />
                        <asp:HiddenField ID="HDNSeg" runat="server" />
                        <asp:HiddenField ID="HDNEmp" runat="server" />
                        <asp:HiddenField ID="hdnSystemJvs" runat="Server" />
                    </td>
                </tr>
                <tr>
                    <td style="display: none">
                        <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    </td>
                </tr>
            </table>
            <%--   <table width="100%">
              <tr>
              <td>
              <div id="DisplayOblg" runat="server"></div>
              </td>
            </tr>
            </table>--%>
            <table style="background-color: #DDECFE;" width="100%" height="300px">
                <tr>
                    <td>
                        <asp:HiddenField ID="HDNAccInd" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
