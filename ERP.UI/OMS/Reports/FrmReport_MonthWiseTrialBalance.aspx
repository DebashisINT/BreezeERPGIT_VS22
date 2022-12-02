<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_FrmReport_MonthWiseTrialBalance" CodeBehind="FrmReport_MonthWiseTrialBalance.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

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


        function HideAll() {
            document.getElementById('TdSelect').style.display = 'none';
            document.getElementById('TdExport').style.display = 'none';
            document.getElementById('btnPrint').style.display = 'none';
            alert('No  Record Found!..');

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


            document.getElementById('TrForGroup').style.display = 'none';
            document.getElementById('TrForClient').style.display = 'none';

            document.getElementById('TrShowBranchNet').style.display = 'none';
            document.getElementById('btnPrint').style.display = 'none';
            document.getElementById('TrCA').style.display = 'none';





        }

        function FunClientScrip(objID, objListFun, objEvent) {
            var cmbVal;
            //          if(groupvalue=="")
            //          {
            //               cmbVal=document.getElementById('cmbsearchOption').value;
            //               cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
            //          }
            //          else
            //          {
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
            //          } 
            ajax_showOptions(objID, objListFun, objEvent, cmbVal);
        }
        function LedType(obj) {
            if (obj.toUpperCase() == 'CUSTOMERS' || obj.toUpperCase() == 'CDSL CLIENTS' || obj.toUpperCase() == 'NSDL CLIENTS') {
                document.getElementById('TrCA').style.display = 'none';
                document.getElementById('TrForGroup').style.display = 'inline';
                document.getElementById('TrForClient').style.display = 'inline';
                document.getElementById('TrShowBranchNet').style.display = 'inline';

            }
            else {
                document.getElementById('TrCA').style.display = 'none';
                document.getElementById('TrForGroup').style.display = 'none';
                document.getElementById('TrForClient').style.display = 'none';

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
                    document.getElementById('TdFilter').style.display = 'inline';
                    document.getElementById('TdSelect').style.display = 'inline';
                    document.getElementById('TdFilter1').style.display = 'inline';
                }

            }
        }

        function NoRecordFound() {
            Page_Load();
            alert("No Record Found!..");

        }


        function ShowGrid() {

            document.getElementById('TdGrid').style.display = 'none';
            document.getElementById('HeaderGrid').style.display = 'none';
            document.getElementById('TdExport').style.display = 'none';
            document.getElementById('TrAll').style.display = 'inline';
            document.getElementById('TdGridPeriod').style.display = 'inline';
            document.getElementById('TDSelect').style.display = 'none';


            //document.getElementById('TdGrid').style.display='inline';
            //document.getElementById('HeaderGrid').style.display='inline';
            //document.getElementById('TdExport').style.display='inline';
            //document.getElementById('TrAll').style.display='none';
            //document.getElementById('TdGridPeriod').style.display='none';
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
                    //                    document.getElementById('TrForGroup').style.display='inline';
                    //                    document.getElementById('TrForClient').style.display='inline';
                }
                else if (obj1 == 'S') {
                    FilTer.value = 'Segment';
                }
                else if (obj1 == 'E') {
                    FilTer.value = 'Employee';
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

                    document.getElementById('TrShowBranchNet').style.display = 'none';
                    //---NEW ---
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

            document.getElementById('TrAccount').style.display = 'none';
            document.getElementById('TrLink').style.display = 'none';
            document.getElementById('TrButton').style.display = 'none';
            document.getElementById('TrAll').style.display = 'none';
            document.getElementById('TrCancel').style.display = 'inline';
            document.getElementById('TdExport').style.display = 'none';
        }
        function btnCalcel_Click() {
            parent.editwin.close();
            // parent.FillValues();
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


        function ShowLedger(objMainID, objSubID, objSegmentID, objMainAcc, objSubAcc, objUcc, objDate) {
            var URL = 'frmReport_IFrameLedgerView.aspx?MainID=' + objMainID + ' &SubID=' + objSubID + ' &SegmentID=' + objSegmentID + ' &date=' + objDate;
            OnMoreInfoClick(URL, "" + objMainAcc + "  -  " + objSubAcc + " [" + objUcc + "]", '1000px', '500px', "N");
        }

    </script>

    <script type="text/ecmascript">

        function ShowDropDown() {
            //alert("123");
            document.getElementById('TrPrevNext').style.display = 'inline'
        }

        function ReceiveServerData(rValue) {

            //   var Data=rValue.split('~');



            var Data = rValue.split('~');
            if (Data[0] == 'MainAcc') {
                Mainvalue = Data[1];
                document.getElementById('HeaderGrid').style.display = 'none';
                document.getElementById('TdGrid').style.display = 'none';
                document.getElementById('TrPrevNext').style.display = 'inline'

                document.getElementById('TrShowBranchNet').style.display = 'inline';
                document.getElementById('HdnMainAcc').value = Data[1];
                var btn = document.getElementById('BtnDropdown');
                btn.click();
            }
            if (Data[0] == 'Group') {
                groupvalue = Data[1];
                //                 var btn = document.getElementById('btnhide');
                //                btn.click();
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
                combo.innerText = val;
            }
            if (Data[0] == 'Employee') {
                document.getElementById('HdnEmployee').value = Data[1];
            }


        }


        function fn_ddllistType(obj) {
            if (obj == '0') {

                document.getElementById('btnPrint').style.display = 'none';
                document.getElementById('btnShow').style.display = 'inline';


            }
            if (obj == '1') {
                document.getElementById('btnPrint').style.display = 'inline';
                document.getElementById('btnShow').style.display = 'none';


            }

            if (obj == '2') {

                document.getElementById('btnShow').style.display = 'none';
                document.getElementById('btnPrint').style.display = 'none';





            }
        }

        function CallMainAccount(obj1, obj2, obj3) {
            var obj4 = '';
            var obj5 = 'Main';
            AccountValue = 'MainAccountSelect';
            ajax_showOptions(obj1, obj2, obj3, obj4, obj5);
        }
        function keyVal(obj) {
            if (AccountValue == 'MainAccountSelect') {
                document.getElementById('HdnMainAcc').value = obj;
                var btn = document.getElementById('BtnDropdown');
                btn.click();
            }
        }

    </script>
    <script language="javascript" type="text/javascript">
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
            <h3>Month Wise Trial Balance</h3>
        </div>
    </div>
    <div class="form_main inner">
        <%--<table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Month Wise Trial Balance </span></strong>
                </td>
            </tr>
        </table>--%>
        <table class="TableMain100">
            <tr id="TrAll">
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td style="vertical-align: top;">
                                <table cellspacing="1" cellpadding="2" 
                                    >
                                    <tr id="TrDateCheck">
                                        <td>Select Period:
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>From:</td>
                                                    <td>
                                                        <asp:DropDownList ID="dtStartMonth" runat="server" onchange="ValidateSMonth(this.value)">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>To:</td>
                                                    <td>
                                                        <asp:DropDownList ID="dtEndMonth" runat="server" onchange="ValidateEMonth(this.value)">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Main Account
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtMainAcc" Width="300px" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="TxtMainAcc_hidden" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="TrAccount" style="display: none;">
                                        <td class="gridcellleft">Main Account
                                        </td>
                                        <td class="gridcellleft" style="padding-left:50px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbMainAll" runat="server" Checked="True" GroupName="a12" onclick="AllSelct('a','M')" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbMainSelected" runat="server" GroupName="a12" onclick="AllSelct('b','M')" />
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
                                                        <asp:RadioButton ID="rdAllSegment" runat="server" GroupName="c1" onclick="AllSelct('a','S')" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdSelSegment" runat="server" Checked="True" GroupName="c1" onclick="AllSelct('b','S')" />
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
                                    <tr id="TrCA" style="display: none;">
                                        <td class="gridcellleft">Generate Type:
                                        </td>
                                        <td class="gridcellleft">
                                            <asp:DropDownList ID="ddllistType" runat="server" Width="120px" Font-Size="12px"
                                                onchange="fn_ddllistType(this.value)">
                                                <asp:ListItem Value="0">Screen</asp:ListItem>
                                                <asp:ListItem Value="1">Print</asp:ListItem>
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
                                    <tr id="TrShowBranchNet">
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkBranchNet" runat="server" />
                                            Show Branch/Group Net
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkZero" runat="server" />
                                            Show Zero Balance Account
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top">
                                <table id="TDSelect">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: left;">
                                            <table cellpadding="0" cellspacing="0">
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
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td id="TdFilter" style="height: 23px">
                                                        <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="253" onkeyup="FunClientScrip(this,'MonthlyTrialAjaxList',event)"></asp:TextBox><a
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
                                            <table cellpadding="0" cellspacing="0" id="TdSelect">
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
            <tr id="TrLink" style="display: none;">
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
                            <%-- <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />--%>
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
                    <asp:Button ID="btnShow" runat="server" Text="Export To Excel" CssClass="btnUpdate btn btn-primary"
                         OnClientClick="selecttion()" OnClick="btnShow_Click" />
                    <asp:Button ID="btnPrint" Text="Print" runat="server" CssClass="btnUpdate btn btn-primary" OnClick="btnPrint_Click"
                        TabIndex="6" />
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
                                    <td id="TdGrid" style="width: 900px; display: none;">
                                        <div id="DisplayLedger" runat="server" style="overflow: scroll;">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TdGridPeriod"></td>
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
                            <%-- <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            --%>
                            <asp:AsyncPostBackTrigger ControlID="lnkNextClient" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbclientsPager" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lnkPrevClient" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="HdnMainAcc" runat="server" />
                    <asp:HiddenField ID="HdnClients" runat="server" />
                    <asp:HiddenField ID="HdnGroup" runat="server" />
                    <asp:HiddenField ID="HdnBranchId" runat="server" />
                    <asp:HiddenField ID="HdnSegment" runat="server" />
                    <asp:HiddenField ID="HdnEmployee" runat="server" />
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
