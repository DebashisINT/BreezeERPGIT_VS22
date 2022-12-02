<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_IframeSubsidiaryTrial" CodeBehind="frmReport_IframeSubsidiaryTrial.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>


    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    <!--Centeral Data-->

    <script type="text/javascript" src="../CentralData/JSScript/GenericJScript.js"></script>

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
        IsAllMainAc_Customers = null;
        IsAllMainAc_SubledgrTypeSame = null;
        groupvalue = "";
        Mainvalue = "";
        var oldColorP = '';
        function SelectAccount() {
            alert('You Can Not View all Account!Please Select Atleast One Account.');
            var chk = document.getElementById("rdbMainSelected");
            chk.checked = true;
            if (chk.checked == true) {
                AllSelct('b', 'M');
                document.getElementById("txtsubscriptionID").focus();
            }
        }
        function ChangeRowColorForPeriod(rowID, rowNumber) {
            var gridview = document.getElementById('grdSubsidiaryTrialPeriod');
            var rCount = gridview.rows.length;
            var rowIndex = 1;
            var rowCount = 0;
            if (rCount == 18)
                rowCount = 15;
            else
                rowCount = rCount - 2;
            if (rowNumber > 15 && rCount < 18)
                rowCount = rCount - 3;
            for (rowIndex; rowIndex <= rowCount; rowIndex++) {
                var rowElement = gridview.rows[rowIndex];
                rowElement.style.backgroundColor = '#FFFFFF'
            }
            var color = document.getElementById(rowID).style.backgroundColor;
            if (color != '#ffe1ac') {
                oldColorP = color;
            }
            if (color == '#ffe1ac') {
                document.getElementById(rowID).style.backgroundColor = oldColorP;
            }
            else
                document.getElementById(rowID).style.backgroundColor = '#ffe1ac';
        }
        function SignOff() {
            window.parent.SignOff();
        }
        FieldName = 'lstSuscriptions';
        function Page_Load() {
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdSelect').style.display = 'none';
            document.getElementById('TrPrevNext').style.display = 'none';
            document.getElementById('TdExport').style.display = 'none';
            document.getElementById('TdFilter1').style.display = 'none';
            document.getElementById('TdFrom').style.display = 'none';
            document.getElementById('TdTo').style.display = 'none';
            document.getElementById('TrForGroup').style.display = 'none';
            document.getElementById('TrForClient').style.display = 'none';
            document.getElementById('TrShowPhnumber').style.display = 'none';
            document.getElementById('TrShowBranchNet').style.display = 'none';
            document.getElementById('btnPrint').style.display = 'none';
            document.getElementById('btndos').style.display = 'none';
            document.getElementById('TrCA').style.display = 'none';
            document.getElementById('<%=RadAsOnDate.ClientID%>').checked = true;
            document.getElementById('btnSend').style.display = 'none';
            document.getElementById('MailTo').style.display = 'none';
            cCmbReportType.SetEnabled(false);
            HideShow('divReportBy', 'H');
            divAsOnDate.innerText = "As On Date :";
            //for MainAccount Select (By Default)
            AllSelct('b', 'M')
        }
        function ShowDate(obj) {
            if (obj == 'a') {
                document.getElementById('TdFrom').style.display = 'none';
                document.getElementById('TdTo').style.display = 'none';
                document.getElementById('TdAsOn').style.display = 'inline';
                divAsOnDate.innerText = "As On Date :";
            }
            else {
                document.getElementById('TdFrom').style.display = 'inline';
                document.getElementById('TdTo').style.display = 'inline';
                document.getElementById('TdAsOn').style.display = 'none';
                divAsOnDate.innerText = "Period :";
            }
        }
        function FunClientScrip(objID, objListFun, objEvent) {
            var cmbVal;
            if (document.getElementById('cmbsearchOption').value == "Clients") {
                if (document.getElementById('ddlGroup').value == "0")//////////////Group By  selected are branch
                {
                    if (document.getElementById('rdbranchAll').checked == true) {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                        cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue;
                    }
                    else {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                        cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue + '~' + Mainvalue;
                    }
                }
                else //////////////Group By selected are Group
                {
                    if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                        cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue;
                    }
                    else {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                        cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue + '~' + Mainvalue;
                    }
                }
            }
            else {
                cmbVal = document.getElementById('cmbsearchOption').value;
                cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue;
            }
            //      } 
            ajax_showOptions(objID, objListFun, objEvent, cmbVal);
        }
        function LedType(obj) {
            if (obj.toUpperCase() == 'MAILTOEMPLOYEE' || obj.toUpperCase() == 'CUSTOMERS' || obj.toUpperCase() == 'CDSL CLIENTS' || obj.toUpperCase() == 'NSDL CLIENTS' || obj.toUpperCase() == 'EMPLOYEES' || obj.toUpperCase() == 'RELATIONSHIP PARTNERS' || obj.toUpperCase() == 'BUSINESS PARTNERS' || obj.toUpperCase() == 'BROKERS' || obj.toUpperCase() == 'SUB BROKERS' || obj.toUpperCase() == 'FRANCHISEES' || obj.toUpperCase() == 'VENDORS' || obj.toUpperCase() == 'DATA VENDORS' || obj.toUpperCase() == 'RECRUITMENT AGENTS' || obj.toUpperCase() == 'AGENTS' || obj.toUpperCase() == 'CONSULTANTS' || obj.toUpperCase() == 'SHARE HOLDER' || obj.toUpperCase() == 'DEBTORS' || obj.toUpperCase() == 'CREDITORS') {
                document.getElementById('TrCA').style.display = 'inline';
                document.getElementById('TrForGroup').style.display = 'inline';
                document.getElementById('TrForClient').style.display = 'inline';
                document.getElementById('TrCA').style.display = 'inline';
            }
            else {
                document.getElementById('TrCA').style.display = 'none';
                document.getElementById('TrForGroup').style.display = 'none';
                document.getElementById('TrForClient').style.display = 'none';
                document.getElementById('TrShowPhnumber').style.display = 'none';
                document.getElementById('TrShowBranchNet').style.display = 'none';
                document.getElementById('TrCA').style.display = 'none';
            }
        }
        function btnAddsubscriptionlist_click() {
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
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdSelect').style.display = 'none';
            document.getElementById('TdFilter1').style.display = 'none';

        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '500';
            }
            window.frameElement.width = document.body.scrollWidth;
        }
        function ShowMainAccountName(obj1, obj2, obj3, obj4) {
            var cmb = document.getElementById("cmbsearchOption");
            var obj4 = cmb.value;
            ajax_showOptions(obj1, obj2, obj3, obj4);
        }
        function MainAll(obj1, obj2) {
            document.getElementById('cmbsearchOption').value = obj2;
            if (obj1 == 'all') {
                SetValue('<%=HdnSegment.ClientID%>', '');
            GetObjectID('<%=Span2.ClientID%>').innerText = '';
            if (document.getElementById('<%=rdbMainAll.ClientID%>').checked == false) {
                if (IsAllMainAc_Customers == 'Yes') {
                    HideShow("Table_SegWise_Col_SubTrail", "S");
                }
                else {
                    HideShow("Table_SegWise_Col_SubTrail", "H");
                }
            }
            else
                HideShow("Table_SegWise_Col_SubTrail", "H");
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdSelect').style.display = 'none';
            document.getElementById('TdFilter1').style.display = 'none';
        }
        else {
            if (obj1 == 'Selc' && (document.getElementById('HDNAccInd').value == 'N')) {
                document.getElementById('TdFilter').style.display = 'none';
                document.getElementById('TdFilter1').style.display = 'none';
                document.getElementById('TdSelect').style.display = 'none';
            }
            else {
                HideShow("Table_SegWise_Col_SubTrail", "H");
                document.getElementById('TdFilter').style.display = 'inline';
                document.getElementById('TdSelect').style.display = 'inline';
                document.getElementById('TdFilter1').style.display = 'inline';
            }
        }
    }
    function ShowGrid() {
        document.getElementById('TdGrid').style.display = 'inline';
        document.getElementById('HeaderGrid').style.display = 'inline';

        document.getElementById('TdExport').style.display = 'inline';
        document.getElementById('TrAll').style.display = 'none';
        document.getElementById('TdGridPeriod').style.display = 'none';
        height();
    }
    function ShowGrid1() {
        document.getElementById('HeaderGrid').style.display = 'inline';
        document.getElementById('TdGrid').style.display = 'none';
        document.getElementById('TdExport').style.display = 'inline';
        document.getElementById('TrAll').style.display = 'none';
        document.getElementById('TdGridPeriod').style.display = 'inline';
        height();
    }
    function Filter() {
        document.getElementById('TrAll').style.display = 'inline';
        height();
    }
    function Disable(obj) {
        var gridview = document.getElementById('grdSubsidiaryTrial');
        var rCount = gridview.rows.length;
        if (rCount < 10)
            rCount = '0' + rCount;
        if (obj == 'P') {
            document.getElementById("grdSubsidiaryTrial_ctl18_FirstPage").style.display = 'none';
            document.getElementById("grdSubsidiaryTrial_ctl18_PreviousPage").style.display = 'none';
            document.getElementById("grdSubsidiaryTrial_ctl18_NextPage").style.display = 'inline';
            document.getElementById("grdSubsidiaryTrial_ctl18_LastPage").style.display = 'inline';
        }
        else {
            document.getElementById("grdSubsidiaryTrial_ctl" + rCount + "_NextPage").style.display = 'none';
            document.getElementById("grdSubsidiaryTrial_ctl" + rCount + "_LastPage").style.display = 'none';
        }
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
    function AllSelct(obj, obj1) {
        var FilTer = document.getElementById('cmbsearchOption');
        if (obj != 'a') {
            if (obj1 == 'C')
                FilTer.value = 'Clients';
            else if (obj1 == 'B')
                FilTer.value = 'Branch';
            else if (obj1 == 'G')
                FilTer.value = 'Group';
            else if (obj1 == 'M') {
                FilTer.value = 'MainAcc';
            }
            else if (obj1 == 'S') {
                FilTer.value = 'Segment';
            }
            else if (obj1 == 'E') {
                FilTer.value = 'Employee';
            }
            else if (obj1 == 'MailToEmp') {
                GetObjectID('cmbsearchOption').value = 'MailToEmployee';
            }
            document.getElementById('TdFilter').style.display = 'inline';
            document.getElementById('TdFilter1').style.display = 'inline';
            document.getElementById('TdSelect').style.display = 'inline';
        }
        else {
            if (obj1 == 'M') {
                document.getElementById('TrCA').style.display = 'none';
                document.getElementById('TrForGroup').style.display = 'none';
                document.getElementById('TrForClient').style.display = 'none';
                //---NEW ---
                document.getElementById('TrShowPhnumber').style.display = 'none';
                document.getElementById('TrShowBranchNet').style.display = 'none';
                //---NEW ---

                //Show Segment Wise Break Up Changes
                SetValue("HdnMainAcc", "");
                IsAllMainAc_Customers = null;
                GetObjectID("Chk_SegWiseBreakUp").checked = false;
                HideShow("Tr_TDay_Consolidate_Balance", "H");
                HideShow("Tr_BtnGenerate", "H");
                HideShow("Table_SegWise_Col_SubTrail", "H");
                HideShow("trlink", "S");
                HideShow("trbutton", "S");
            }
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdFilter1').style.display = 'none';
            document.getElementById('TdSelect').style.display = 'none';
        }
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
    function FromGeneralLedger() {
        document.getElementById('TrDate').style.display = 'none';
        document.getElementById('TrAccount').style.display = 'none';
        document.getElementById('TrLink').style.display = 'none';
        document.getElementById('TrButton').style.display = 'none';
        document.getElementById('TrAll').style.display = 'none';
        document.getElementById('TrCancel').style.display = 'inline';
        document.getElementById('TdExport').style.display = 'none';
    }
    function btnCalcel_Click() {
        parent.editwin.close();
    }
    document.body.style.cursor = 'pointer';
    var oldColor = '';
    function ChangeRowColor(rowID, rowNumber) {
        var gridview = document.getElementById('grdSubsidiaryTrial');
        var rCount = gridview.rows.length;
        var rowIndex = 1;
        var rowCount = 0;
        if (rCount == 18)
            rowCount = 15;
        else
            rowCount = rCount - 2;
        if (rowNumber > 15 && rCount < 18)
            rowCount = rCount - 3;
        for (rowIndex; rowIndex <= rowCount; rowIndex++) {
            var rowElement = gridview.rows[rowIndex];
            rowElement.style.backgroundColor = '#FFFFFF'
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
        var MonthDate = dtToDate.GetDate().getMonth() + 1;
        var DayDate = dtToDate.GetDate().getDate();
        var YearDate = dtToDate.GetDate().getYear();
        var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
        var Sto = new Date(sessionValTo).getMonth() + 1;
        var SFrom = new Date(sessionValFrom).getMonth() + 1;
        var SDto = new Date(sessionValTo).getDate();
        var SDFrom = new Date(sessionValFrom).getDate();

        if (YearDate <= objsession[1]) {
            if (MonthDate < SFrom && YearDate == objsession[0]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                dtToDate.SetDate(new Date(datePost));
            }
            else if (MonthDate > Sto && YearDate == objsession[1]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                dtToDate.SetDate(new Date(datePost));
            }
            else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                dtToDate.SetDate(new Date(datePost));
            }
        }
        else {
            alert('Enter Date Is Outside Of Financial Year !!');
            var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
            dtToDate.SetDate(new Date(datePost));
        }
    }
    function ShowLedger(objMainID, objSubID, objSegmentID, objMainAcc, objSubAcc, objUcc, objDate) {
        var URL = 'frmReport_IFrameLedgerView.aspx?MainID=' + objMainID + ' &SubID=' + objSubID + ' &SegmentID=' + objSegmentID + ' &date=' + objDate;
        OnMoreInfoClick(URL, "" + objMainAcc + "  -  " + objSubAcc + " [" + objUcc + "]", '1000px', '500px', "N");
    }
    </script>

    <script type="text/ecmascript">
        function ShowDropDown() {
            document.getElementById('TrPrevNext').style.display = 'inline'
        }
        function ReceiveServerData(rValue) {
            var Data = rValue.split('~');

            if (Data[0] == 'MainAcc') {
                Mainvalue = Data[1];
                document.getElementById('HeaderGrid').style.display = 'none';
                document.getElementById('TdGrid').style.display = 'none';
                document.getElementById('TrPrevNext').style.display = 'inline'
                document.getElementById('TrShowPhnumber').style.display = 'inline';
                document.getElementById('TrShowBranchNet').style.display = 'inline';
                document.getElementById('HdnMainAcc').value = Data[1];

                //Checking If All Account's SubLedgerType Customers
                if (Data[2] == "Customers")
                    IsAllMainAc_Customers = "Yes";
                else
                    IsAllMainAc_Customers = null;

                //Checking If All Accounts Have Same SubLedgerType
                if (typeof Data[2] != "undefined") {
                    IsAllMainAc_SubledgrTypeSame = "Yes"
                }
                var btn = document.getElementById('BtnDropdown');
                btn.click();
            }
            if (Data[0] == 'Group') {
                groupvalue = Data[1];
                document.getElementById('HdnGroup').value = Data[1];
            }
            if (Data[0] == 'Branch') {
                groupvalue = Data[1];
                document.getElementById('HdnBranchId').value = Data[1];
                var btn = document.getElementById('btnhide');
                btn.click();
            }
            if (Data[0] == 'Clients') {
                document.getElementById('HdnClients').value = Data[1];
            }
            if (Data[0] == 'Segment') {
                var combo = document.getElementById('Span2');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                var seg = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    if (val == '') {
                        seg = items[0];
                        val = items[1];
                    }
                    else {
                        seg += ',' + items[0];
                        val += ',' + items[1];
                    }
                }
                document.getElementById('HdnSegment').value = seg;
                document.getElementById('HDNSeg').value = val;
                combo.innerText = val;
            }
            if (Data[0] == 'Employee') {
                document.getElementById('HdnEmployee').value = Data[1];
            }
            if (Data[0] == 'MailToEmployee') {
                var items = Data[1].split(',');
                var recieveItems = '';
                for (var i = 0; i < items.length; i++) {
                    if (recieveItems == '')
                        recieveItems = items[i].split('^')[0] + "~" + items[i].split('^')[1].toLowerCase();
                    else
                        recieveItems = recieveItems + "#" + items[i].split('^')[0] + "~" + items[i].split('^')[1].toLowerCase();
                }
                document.getElementById('HiddenField_MailToEmp').value = recieveItems;
            }
            //New Change [Check If Table_SegWise_Col_SubTrail Should Show Or Not]       
            if (Data[0] == 'MainAcc' || Data[0] == 'Segment') {
                var TotalSegments = document.getElementById('HdnSegment').value.split(',').length;
                var mailToEmployeeByMainAcc = document.getElementById('HiddenField_MailToEmp').value.length;
                if (IsAllMainAc_Customers == "Yes") {
                    if ((TotalSegments > 1) || (GetObjectID('<%=rdAllSegment.ClientID%>').checked == true) || (mailToEmployeeByMainAcc > 0))
                        HideShow("Table_SegWise_Col_SubTrail", "S");
                    else
                        HideShow("Table_SegWise_Col_SubTrail", "H");
                }
                else
                    HideShow("Table_SegWise_Col_SubTrail", "H");
                height();
            }

            //Check if All Main Accounts SubLedger Type is Same
            if (IsAllMainAc_SubledgrTypeSame == "Yes")
                HideShow("TrConsolidateAccounts", "S");
            else
                HideShow("TrConsolidateAccounts", "H");

        }
        function fn_ddllistType(obj) {
            if (obj == '0') {

                document.getElementById('btnPrint').style.display = 'none';
                document.getElementById('btnShow').style.display = 'inline';
                document.getElementById('btnSend').style.display = 'none';
                document.getElementById('MailTo').style.display = 'none';
                document.getElementById('btndos').style.display = 'none';
            }
            if (obj == '1') {
                document.getElementById('btnPrint').style.display = 'inline';
                document.getElementById('btnShow').style.display = 'none';
                document.getElementById('btnSend').style.display = 'none';
                document.getElementById('MailTo').style.display = 'none';
                document.getElementById('btndos').style.display = 'none';
            }
            if (obj == '2') {
                document.getElementById('btnShow').style.display = 'none';
                document.getElementById('btnPrint').style.display = 'none';
                document.getElementById('btnSend').style.display = 'inline';
                document.getElementById('MailTo').style.display = 'inline';
                document.getElementById('TrShowPhnumber').style.display = 'none';
                document.getElementById('btndos').style.display = 'none';
            }
            if (obj == '3') {
                document.getElementById('btnPrint').style.display = 'none';
                document.getElementById('btnShow').style.display = 'none';
                document.getElementById('btnSend').style.display = 'none';
                document.getElementById('MailTo').style.display = 'none';
                document.getElementById('btndos').style.display = 'inline';
                document.getElementById('tr_hdrall').style.display = 'inline';
                document.getElementById('TrShowPhnumber').style.display = 'none';
            }
        }
        //New Addition For Show Segment Wise Break Up
        function Chk_SegWiseBreakUp_Click() {
            if (GetObjectID("Chk_SegWiseBreakUp").checked) {
                HideShow("Tr_TDay_Consolidate_Balance", "S");
                HideShow("Tr_BtnGenerate", "S");
                HideShow("trlink", "H");
                HideShow("trbutton", "H");
                //=========Hide For New====
                HideShow('divDebit', 'H');
                HideShow('divCredit', 'H');
                HideShow('divDrCrBoth', 'H');
                HideShow('TrAmount', 'H');
                HideShow('TrCA', 'H');
                HideShow('TrShowBranchNet', 'H');
                HideShow('TrShowZeroBalNet', 'H');
                cCmbReportType.SetSelectedIndex(0);
                cCmbReportType.SetEnabled(true);
                HideShow('divReportBy', 'S');
                fn_CmbReportType('XL');
            }
            else {
                HideShow("Tr_TDay_Consolidate_Balance", "H");
                HideShow("Tr_BtnGenerate", "H");
                HideShow("trlink", "S");
                HideShow("trbutton", "S");
                //=========Show For New====
                HideShow('divDebit', 'S');
                HideShow('divCredit', 'S');
                HideShow('divDrCrBoth', 'S');
                HideShow('TrAmount', 'S');
                HideShow('TrCA', 'S');
                HideShow('TrShowBranchNet', 'S');
                HideShow('TrShowZeroBalNet', 'S');
            }
            height();
        }
    </script>

    <script type="text/javascript">
        function fn_CmbReportType(ele) {
            if (ele != "XL") {
                HideShow('divSendMail', 'S');
                if (GetObjectID('<%=ddlGroup.ClientID%>').selectedIndex == 0) {
                    cCmbMailSendTo.PerformCallback('MailToBranch~');
                }
                else {
                    cCmbMailSendTo.PerformCallback('MailToGroup~');
                }
            }
            else {
                HideShow('divSendMail', 'H');
            }
        }
        function CmbMailSendTo_EndCallback() {
            HideShow('divSendMail', 'S');
            height();
        }
        function fn_CmbMailSendTo(obj) {
            if (obj == "0") {
                HideShow('divSendMail', 'S');
                HideShow('TdFilter1', 'H');
                HideShow('TdFilter', 'H');
                HideShow('TdSelect', 'H');
                document.getElementById('HiddenField_MailToEmp').value == '';
                EnableDisableControl('<%=btnGenerate.ClientID%>', 'D');
            height();
        }
        else if (obj == '3') {
            HideShow('TdFilter1', 'S');
            HideShow('TdFilter', 'S');
            HideShow('TdSelect', 'S');
            EnableDisableControl('<%=btnGenerate.ClientID%>', 'E');
            AllSelct('b', 'MailToEmp');
        }
}
function Reset() {
    window.location = '../Reports/frmReport_IframeSubsidiaryTrial.aspx';
}
function chkConsolidateAccounts_Click() {
    if (GetObjectID("chkConsolidateAccounts").checked)
        HideShow('TrPrevNext', 'H');
    else
        HideShow('TrPrevNext', 'S');
}
    </script>
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
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Subsidiary Trial</h3>
        </div>
    </div>
    <div class="form_main">
        <%--<table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Subsidiary Trial</span></strong>
                </td>
            </tr>
        </table>--%>
        <table class="TableMain100">
            <tr id="TrAll">
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td style="width: 774px">
                                <table cellspacing="1" cellpadding="2" style="background-color: #B7CEEC; border: solid 1px  #ffffff"
                                    border="1">
                                    <tr id="TrDateCheck">
                                        <td class="gridcellleft">Date :
                                        </td>
                                        <td class="gridcellleft">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RadAsOnDate" runat="server" GroupName="k1" Checked="true" onclick="ShowDate('a')" /></td>
                                                    <td class="gridcellleft">As On Date :
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadPeriod" runat="server" GroupName="k1" onclick="ShowDate('b')" /></td>
                                                    <td class="gridcellleft">Period :
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrDate">
                                        <td class="gridcellleft">
                                            <div id="divAsOnDate">
                                            </div>
                                        </td>
                                        <td class="gridcellleft">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td id="TdAsOn">
                                                                    <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                        Width="108px">
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                                <td id="TdFrom">
                                                                    <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" ClientInstanceName="dtFrom"
                                                                        UseMaskBehavior="True" Width="108px">
                                                                        <DropDownButton Text="From">
                                                                        </DropDownButton>
                                                                        <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                                <td id="TdTo">
                                                                    <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                                                        UseMaskBehavior="True" Width="108px">
                                                                        <DropDownButton Text="To">
                                                                        </DropDownButton>
                                                                        <ClientSideEvents DateChanged="function(s,e){DateChangeForTo();}" />
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <div id="divDebit">
                                                            <asp:RadioButton ID="rdDebit" runat="server" GroupName="k" /><asp:Label ID="Label1"
                                                                runat="server" Text="Only Debit"></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div id="divCredit">
                                                            <asp:RadioButton ID="rdCredit" runat="server" GroupName="k" /><asp:Label ID="Label2"
                                                                runat="server" Text="Only Credit"></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div id="divDrCrBoth">
                                                            <asp:RadioButton ID="rdBoth" runat="server" GroupName="k" Checked="true" /><asp:Label
                                                                ID="Label3" runat="server" Text="Both"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrAmount">
                                        <td class="gridcellleft">Debit/Credit Amount>=
                                        </td>
                                        <td class="gridcellleft">
                                            <asp:TextBox ID="txtDebitCredit" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="TrAccount">
                                        <td class="gridcellleft">Main Account
                                        </td>
                                        <td class="gridcellleft">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbMainAll" runat="server" GroupName="a12" onclick="AllSelct('a','M')" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbMainSelected" runat="server" GroupName="a12" onclick="AllSelct('b','M')"
                                                            Checked="True" />
                                                    </td>
                                                    <td>Selected
                                                    </td>
                                                    <td>
                                                        <span id="litSegment" runat="server" style="color: Maroon"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrSeg" runat="server">
                                        <td class="gridcellleft">Segment</td>
                                        <td class="gridcellleft">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdAllSegment" runat="server" GroupName="c1" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdSelSegment" runat="server" Checked="true" GroupName="c1" />
                                                    </td>
                                                    <td>Selected
                                                    </td>
                                                    <td>[<span id="Span2" runat="server" style="color: Maroon"></span>]
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrForGroup">
                                        <td class="gridcellleft">Group By</td>
                                        <td class="gridcellleft">
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
                                                                    <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="AllSelct('a','B')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="AllSelct('b','B')" />Selected
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
                                                                        onclick="AllSelct('a','G')" />
                                                                    All
                                                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="AllSelct('b','G')" />Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrCA">
                                        <td class="gridcellleft">Generate Type:
                                        </td>
                                        <td class="gridcellleft">
                                            <asp:DropDownList ID="ddllistType" runat="server" Width="120px" Font-Size="12px"
                                                onchange="fn_ddllistType(this.value)">
                                                <asp:ListItem Value="0">Screen</asp:ListItem>
                                                <asp:ListItem Value="1">Print</asp:ListItem>
                                                <asp:ListItem Value="2">Send Email</asp:ListItem>
                                                <asp:ListItem Value="3">Dos Print</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TrForClient">
                                        <td class="gridcellleft">Client :</td>
                                        <td class="gridcellleft">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="AllSelct('a','C')" /></td>
                                                    <td>All Client</td>
                                                    <td>
                                                        <asp:RadioButton ID="radPOAClient" runat="server" GroupName="c" onclick="AllSelct('a','C')" />
                                                    </td>
                                                    <td>POA Client</td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="AllSelct('b','C')" /></td>
                                                    <td>Selected Client</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%-- <tr id="tr_hdrall">
                                            <td colspan="2">
                                                <table>
                                                    <tr>
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            Use Header :
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
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            Use Footer :
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
                                        </tr>--%>
                                    <tr id="MailTo">
                                        <td class="gridcellleft">Mail To:
                                        </td>
                                        <td class="gridcellleft">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="ResBranch" runat="server" GroupName="E1" Checked="true" /></td>
                                                    <td class="gridcellleft">Respective Branch/Group
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="ResUser" runat="server" GroupName="E1" onclick="AllSelct('b','E')" /></td>
                                                    <td class="gridcellleft">Selected User
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrShowPhnumber">
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkPhNumber" runat="server" />
                                            Show Phone Number
                                        </td>
                                    </tr>
                                    <tr id="TrShowBranchNet">
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkBranchNet" runat="server" />
                                            Show Branch/Group Net
                                        </td>
                                    </tr>
                                    <tr id="TrShowZeroBalNet">
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkZero" runat="server" />
                                            Show Zero Balance Account
                                        </td>
                                    </tr>
                                    <tr id="TrConsolidateAccounts" style="display: none" onclick="chkConsolidateAccounts_Click()">
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkConsolidateAccounts" runat="server" />
                                            Consolidate Accounts
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table_SegWise_Col_SubTrail" style="border-right: #ffffff 1px solid; border-top: #ffffff 1px solid; border-left: #ffffff 1px solid; border-bottom: #ffffff 1px solid; background-color: #b7ceec; display: none"
                                    border="1">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="Chk_SegWiseBreakUp" runat="server" onclick="Chk_SegWiseBreakUp_Click()" /></td>
                                        <td>Show Segment Wise BreakUp</td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr id="Tr_TDay_Consolidate_Balance" style="display: none">
                                        <td>
                                            <asp:CheckBox ID="Chk_TDayBal" runat="server" /></td>
                                        <td>Show T-Day Net Balance Separately</td>
                                        <td>
                                            <asp:CheckBox ID="Chk_ConsolidateBal" runat="server" /></td>
                                        <td>Consolidate Balances Branch/Group Wise</td>
                                    </tr>
                                    <tr id="divReportBy">
                                        <td>Report By :
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="CmbReportType" ClientInstanceName="cCmbReportType" runat="server"
                                                Width="170px" Font-Size="13px" TabIndex="0">
                                                <Items>
                                                    <dxe:ListEditItem Text="Generate Excel" Value="XL" />
                                                    <dxe:ListEditItem Text="Send Email" Value="ML" />
                                                </Items>
                                                <ClientSideEvents ValueChanged="function(s, e) {fn_CmbReportType(s.GetValue());}" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr id="divSendMail" style="display: none;">
                                        <td class="gridcellleft">Mail To :
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="CmbMailSendTo" ClientInstanceName="cCmbMailSendTo" runat="server"
                                                Width="170px" Font-Size="13px" TabIndex="0" OnCallback="CmbMailSendTo_Callback">
                                                <ClientSideEvents ValueChanged="function(s, e) {fn_CmbMailSendTo(s.GetValue());}"
                                                    EndCallback="CmbMailSendTo_EndCallback" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr id="Tr_BtnGenerate" style="display: none">
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnGenerate" Text="Generate" runat="server" CssClass="btnUpdate"
                                                TabIndex="0" Width="100px" OnClick="btnGenerate_Click" /></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top; display: inline-block; border: 1px solid #ccc;">
                                <table border="1">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: left;">
                                            <table cellpadding="0" cellspacing="0" border="1" style="display: block; border: 2px solid #ddd">
                                                <tr>
                                                    <td id="TdFilter1" style="height: 23px">
                                                        <asp:DropDownList ID="cmbsearchOption" runat="server" Width="85px" Font-Size="11px"
                                                            Enabled="false">
                                                            <asp:ListItem>Clients</asp:ListItem>
                                                            <asp:ListItem>Branch</asp:ListItem>
                                                            <asp:ListItem>Group</asp:ListItem>
                                                            <asp:ListItem>Segment</asp:ListItem>
                                                            <asp:ListItem>MainAcc</asp:ListItem>
                                                            <asp:ListItem>Employee</asp:ListItem>
                                                            <asp:ListItem>MailToEmployee</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td id="TdFilter" style="height: 23px">
                                                        <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="253" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocksMainHead',event)"></asp:TextBox><a
                                                            id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                                    style="color: #009900; font-size: 8pt;">&nbsp;</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top;">
                                            <table cellpadding="0" cellspacing="0" id="TdSelect" border="1">
                                                <tr>
                                                    <td style="padding-left: 7px">
                                                        <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="253px"></asp:ListBox>
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
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TrLink">
                <td style="text-align: left" id="TrPrevNext">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="clientFilter" runat="server">
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkPrevClient" runat="server" CommandName="Prev" OnCommand="NavigationLinkC_Click"
                                            Text="[Prev Client]"> </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbclientsPager" runat="server" CssClass="normaltxt" Width="300px"
                                            Font-Size="12px" ValidationGroup="a" AutoPostBack="True" OnSelectedIndexChanged="cmbclientsPager_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkNextClient" runat="server" CommandName="Next" OnCommand="NavigationLinkC_Click"
                                            Text="[Next Client]"> </asp:LinkButton>&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnDropdown" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td style="display: none;">
                    <asp:Button ID="BtnDropdown" runat="server" Text="Show" CssClass="btnUpdate" Height="23px"
                        OnClientClick="selecttion()" Width="75px" OnClick="BtnDropdown_Click" />
                </td>
            </tr>
            <tr id="TrButton">
                <td id="TrBtn">
                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btnUpdate" OnClientClick="selecttion()"
                        Width="100px" OnClick="btnShow_Click" />
                    <asp:Button ID="btnPrint" Text="Print" runat="server" CssClass="btnUpdate" OnClick="btnPrint_Click"
                        TabIndex="6" Width="100px" />
                    <asp:Button ID="btnSend" Text="Send Email" runat="server" CssClass="btnUpdate" OnClick="btnSend_Click"
                        TabIndex="6" Width="100px" />
                    <asp:Button ID="btndos" Text="Dos Print" runat="server" CssClass="btnUpdate" OnClick="btndos_Click"
                        TabIndex="6" Width="100px" />
                </td>
                <td style="display: none">
                    <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
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
            <tr>
                <td style="text-align: right" id="TdExport">
                    <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                        <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                        <asp:ListItem Value="P">PDF</asp:ListItem>
                    </asp:DropDownList>
                    <a id="A3" href="javascript:void(0);" onclick="Filter()"><span style="color: #009900; text-decoration: underline; font-size: 8pt;">Filter</span></a>
                </td>
            </tr>
            <tr style="background-color: #DDECFE;">
                <td style="background-color: #DDECFE;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="background-color: #DDECFE;" class="TableMain100">
                                <tr>
                                    <td id="HeaderGrid" style="height: 10px; font-size: 12px; font-weight: bold;">
                                        <asp:Label ID="lblReportHeader" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TdGrid">
                                        <asp:GridView ID="grdSubsidiaryTrial" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                            ShowFooter="false" AutoGenerateColumns="false" AllowPaging="false" BorderStyle="Solid"
                                            AllowSorting="true" BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" PageSize="150000"
                                            OnRowCreated="grdSubsidiaryTrial_RowCreated" OnRowDataBound="grdSubsidiaryTrial_RowDataBound">
                                            <%--                      OnSorting="grdSubsidiaryTrial_Sorting"--%>
                                            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Branch Name">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("BranchName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Main Account">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrDate" runat="server" Text='<%# Eval("accountsledger_mainaccountid")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--SortExpression="accountsledger_mainaccountid"  SortExpression="BranchName"--%>
                                                <asp:TemplateField HeaderText="Sub Account">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValueDate" runat="server" Text='<%# Eval("accountsledger_subaccountid")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--   SortExpression="accountsledger_subaccountid"--%>
                                                <asp:TemplateField HeaderText="Trading UCC">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTradingucc" runat="server" Text='<%# Eval("Trading_UCC")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeaderBen" runat="Server" Text='<%# strHtext %>'></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblucc" runat="server" Text='<%# Eval("Ucc")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  SortExpression="Ucc"--%>
                                                <asp:TemplateField HeaderText="Debit">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("AmountDR")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Credit">
                                                    <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescrip" runat="server" Text='<%# Eval("AmountCR")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MainID" Visible="false">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMainID" runat="server" Text='<%# Eval("MainID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SubID" Visible="false">
                                                    <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubID" runat="server" Text='<%# Eval("SubID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mob.Number">
                                                    <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("PhoneNumber")%>'></asp:Label>
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
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TdGridPeriod">
                                        <%--   <div style="width:900px;overflow:scroll">--%>
                                        <asp:GridView ID="grdSubsidiaryTrialPeriod" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                            AutoGenerateColumns="false" BorderStyle="Solid" ShowFooter="true" AllowSorting="false"
                                            BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowCreated="grdSubsidiaryTrialPeriod_RowCreated"
                                            OnRowDataBound="grdSubsidiaryTrialPeriod_RowDataBound">
                                            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="BranchName">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("BranchName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--   SortExpression="BranchName"--%>
                                                <asp:TemplateField HeaderText="Main Account">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrDate" runat="server" Text='<%# Eval("accountsledger_mainaccountid")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sub Account">
                                                    <ItemStyle BorderWidth="1px" Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValueDate" runat="server" Text='<%# Eval("accountsledger_subaccountid")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--SortExpression="accountsledger_subaccountid"--%>
                                                <asp:TemplateField>
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeaderBenPer" runat="Server" Text='<%# strHtext %>'></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblucc" runat="server" Text='<%# Eval("Ucc")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--SortExpression="Ucc"--%>
                                                <asp:TemplateField HeaderText="Trading UCC">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTradingucc" runat="server" Text='<%# Eval("Trading_UCC")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dr">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOpDr" runat="server" Text='<%# Eval("OpeningDR")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cr">
                                                    <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOpCr" runat="server" Text='<%# Eval("OpeningCR")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dr">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDr" runat="server" Text='<%# Eval("AmountDR")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cr">
                                                    <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCr" runat="server" Text='<%# Eval("AmountCR")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dr">
                                                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClDr" runat="server" Text='<%# Eval("ClosingDR")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cr">
                                                    <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClCr" runat="server" Text='<%# Eval("ClosingCR")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerTemplate>
                                                <table>
                                                    <tr>
                                                        <td colspan="10">
                                                            <asp:LinkButton ID="FirstPagePeriod" runat="server" Font-Bold="true" CommandName="First"
                                                                OnClientClick="selecttion()" OnCommand="NavigationLinkPeriod_Click" Text="[First Page]"> </asp:LinkButton>
                                                            <asp:LinkButton ID="PreviousPagePeriod" runat="server" Font-Bold="true" CommandName="Prev"
                                                                OnClientClick="selecttion()" OnCommand="NavigationLinkPeriod_Click" Text="[Previous Page]">  </asp:LinkButton>
                                                            <asp:LinkButton ID="NextPagePeriod" runat="server" Font-Bold="true" CommandName="Next"
                                                                OnClientClick="selecttion()" OnCommand="NavigationLinkPeriod_Click" Text="[Next Page]">
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="LastPagePeriod" runat="server" Font-Bold="true" CommandName="Last"
                                                                OnClientClick="selecttion()" OnCommand="NavigationLinkPeriod_Click" Text="[Last Page]">
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
                                            <HeaderStyle ForeColor="Black" BorderWidth="1px" BackColor="#C6D6FD" BorderColor="AliceBlue"
                                                Font-Bold="False"></HeaderStyle>
                                            <%--<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>--%>
                                        </asp:GridView>
                                        <%--  </div>--%>
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="CurrentPage" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="TotalPages" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="TotalClient" runat="server" />
                            <asp:HiddenField ID="CurrentPagePeriod" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="TotalPagesPeriod" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="TotalClientPeriod" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSend" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lnkNextClient" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbclientsPager" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lnkPrevClient" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="txtHeader_hidden" runat="server" />
                    <asp:HiddenField ID="txtFooter_hidden" runat="server" />
                    <asp:HiddenField ID="HdnMainAcc" runat="server" />
                    <asp:HiddenField ID="HdnClients" runat="server" />
                    <asp:HiddenField ID="HdnGroup" runat="server" />
                    <asp:HiddenField ID="HdnBranchId" runat="server" />
                    <asp:HiddenField ID="HdnSegment" runat="server" />
                    <asp:HiddenField ID="HdnEmployee" runat="server" />
                    <asp:HiddenField ID="HDNSeg" runat="server" />
                </td>
            </tr>
            <tr>
                <td id="TrCancel" style="display: none">
                    <input id="btnCalcel" type="button" value="Close" onclick="btnCalcel_Click()" class="btnUpdate" />
                </td>
            </tr>
            <tr>
                <td style="display: none">
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
        <table style="background-color: #DDECFE;" width="100%" height="200px">
            <tr>
                <td>
                    <asp:HiddenField ID="HDNAccInd" runat="server" />
                    <asp:HiddenField ID="HiddenField_MailToEmp" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
