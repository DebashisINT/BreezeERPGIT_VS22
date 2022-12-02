<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_SegmentWiseLedgerExposureMargin" CodeBehind="SegmentWiseLedgerExposureMargin.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>
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



        function SignOff() {
            window.parent.SignOff();
        }
        FieldName = 'lstSuscriptions';
        function Page_Load() {
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdSelect').style.display = 'none';
            document.getElementById('TdFilter1').style.display = 'none';
            document.getElementById('rdddlgrouptypeAll').checked = true;
            document.getElementById('rdbranchAll').checked = true;
            document.getElementById('rdAllSegment').checked = true;
            document.getElementById('rdbClientALL').checked = true;
            document.getElementById('RdbCurrentCompany').checked = true;
            document.getElementById('ddlgrouptype').selectedIndex = 0;
            document.getElementById('ddlGroup').selectedIndex = 0;
            document.getElementById("HDNAccInd").value = '';
            document.getElementById("HdnMainAcc").value = '';
            document.getElementById("HdnClients").value = '';
            document.getElementById("HdnGroup").value = '';
            document.getElementById("HdnBranchId").value = '';
            document.getElementById("HdnSegment").value = '';
            document.getElementById("HdnEmployee").value = '';
            document.getElementById("HDNSeg").value = '';
            document.getElementById("hidden_Company").value = '';
        }
        function FunClientScrip(objID, objListFun, objEvent) {
            if (document.getElementById('cmbsearchOption').value == "Group") {
                CallGroup(objID, "GenericAjaxList", objEvent);
            }
            if (document.getElementById('cmbsearchOption').value == "Clients") {
                CallClient(objID, "GenericAjaxList", objEvent);
            }
            if (document.getElementById('cmbsearchOption').value == "Company") {
                CallCompany(objID, "GenericAjaxList", objEvent);
            }
            if (document.getElementById('cmbsearchOption').value == "Segment") {
                CallSegment(objID, "GenericAjaxList", objEvent);
            }
            if (document.getElementById('cmbsearchOption').value == "Branch") {
                CallBranch(objID, "GenericAjaxList", objEvent);
            }
        }
        function CallClient(objID, objListFun, objEvent) {
            var strQuery_Table = "Tbl_Master_contact";
            var strQuery_FieldName = "Top 10 Ltrim(Rtrim(Cnt_FirstName))+'['+Ltrim(Rtrim(Cnt_Ucc))+']' Text,Cnt_InternalId [Value]";
            var strQuery_WhereClause = "Left(Cnt_InternalID,2)='CL' and (Cnt_FirstName like '%RequestLetter%' Or Cnt_Ucc like '%RequestLetter%')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, objListFun, objEvent, replaceChars(CombinedQuery), 'Main');
        }
        function CallGroup(objID, objListFun, objEvent) {
            var GrpType = document.getElementById('ddlgrouptype').value;
            var strQuery_Table = "tbl_Master_GroupMaster";
            var strQuery_FieldName = "Top 10 Ltrim(Rtrim(Gpm_Description))+'['+Ltrim(Rtrim(Gpm_Code))+']' Text,Gpm_Code [Value]";
            var strQuery_WhereClause = "Gpm_Type='" + GrpType + "' and (Gpm_Description Like '%RequestLetter%' or Gpm_Code Like '%RequestLetter%')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, objListFun, objEvent, replaceChars(CombinedQuery), 'Main');
        }
        function CallCompany(objID, objListFun, objEvent) {
            var strQuery_Table = "Tbl_Master_Company";
            var strQuery_FieldName = "Top 10  Ltrim(Rtrim(Cmp_Name)) [Text],Cmp_InternalID [Value]";
            var strQuery_WhereClause = "Cmp_Name Like '%RequestLetter%'";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, objListFun, objEvent, replaceChars(CombinedQuery), 'Main');
        }
        function CallSegment(objID, objListFun, objEvent) {
            var strQuery_Table = "tbl_Master_CompanyExchange,tbl_Master_Exchange";
            var strQuery_FieldName = "Top 10 Ltrim(Rtrim(Exh_ShortName))+'-'+Ltrim(Rtrim(exch_segmentId)) [Text],exch_internalId [Value]";
            var strQuery_WhereClause = "Exh_CntID=exch_exchId And Exh_ShortName Like '%RequestLetter%' and exch_exchId is not null and Exch_SegmentID is not Null";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, objListFun, objEvent, replaceChars(CombinedQuery), 'Main');
        }
        function CallBranch(objID, objListFun, objEvent) {
            var strQuery_Table = "tbl_Master_Branch";
            var strQuery_FieldName = "Top 10 Ltrim(Rtrim(Branch_Description)) [Text],Branch_ID [Value]";
            var strQuery_WhereClause = "Branch_Description Like '%RequestLetter%'";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, objListFun, objEvent, replaceChars(CombinedQuery), 'Main');
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

            }
        }

        function btnCalcel_Click() {
            parent.editwin.close();
            // parent.FillValues();
        }
        document.body.style.cursor = 'pointer';
        var oldColor = '';

        function selecttion() {
            var combo = document.getElementById('ddlExport');
            combo.value = 'Ex';
        }


    </script>

    <script type="text/ecmascript">



        function ReceiveServerData(rValue) {

            //   var Data=rValue.split('~');



            var Data = rValue.split('~');
            if (Data[0] == 'MainAcc') {
                Mainvalue = Data[1];
                document.getElementById('HeaderGrid').style.display = 'none';
                document.getElementById('TdGrid').style.display = 'none';
                document.getElementById('HdnMainAcc').value = Data[1];
                var btn = document.getElementById('BtnDropdown');
                btn.click();
            }
            if (Data[0] == 'Group') {
                groupvalue = Data[1];
                document.getElementById('HdnGroup').value = Data[1];
            }
            if (Data[0] == 'Company') {
                var strTemp = Data[1].split(',');
                var CompanyIDs = '';
                var i;
                for (i = 0; i < strTemp.length; i++) {
                    if (CompanyIDs.length == 0)
                        CompanyIDs = strTemp[i].split(';')[0];
                    else
                        CompanyIDs = CompanyIDs + ',' + strTemp[i].split(';')[0];
                }
                document.getElementById('hidden_Company').value = CompanyIDs;
            }
            if (Data[0] == 'Branch') {
                groupvalue = Data[1];
                document.getElementById('HdnBranchId').value = Data[1];
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


        }
        function fnCompany(obj) {
            if (obj == "a") {
                document.getElementById('TdFilter').style.display = 'none';
                document.getElementById('TdFilter1').style.display = 'none';
                document.getElementById('TdSelect').style.display = 'none';
            }
            else {
                document.getElementById('TdFilter').style.display = 'inline';
                document.getElementById('TdFilter1').style.display = 'inline';
                document.getElementById('TdSelect').style.display = 'inline';
                document.getElementById('cmbsearchOption').value = 'Company';
            }
            selecttion();
        }




    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Segment Wise Ledger Exposure Margin Report</span></strong></td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr id="TrAll">
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td style="width: 584px">
                                <table cellspacing="1" cellpadding="2" style="background-color: #B7CEEC; border: solid 1px  #ffffff"
                                    border="1">
                                    <tr id="TrDate">
                                        <td class="gridcellleft">
                                            <div id="divAsOnDate">
                                                As On Date :
                                            </div>
                                        </td>
                                        <td class="gridcellleft">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                            Width="108px">
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdDebit" runat="server" GroupName="k" /><asp:Label ID="Label1"
                                                            runat="server" Text="Only Shortage"></asp:Label></td>
                                                    <td>
                                                        <asp:RadioButton ID="rdCredit" runat="server" GroupName="k" /><asp:Label ID="Label2"
                                                            runat="server" Text="Only Excess"></asp:Label></td>
                                                    <td>
                                                        <asp:RadioButton ID="rdBoth" runat="server" GroupName="k" Checked="true" /><asp:Label
                                                            ID="Label3" runat="server" Text="Both"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrAmount">
                                        <td class="gridcellleft">Debit/Credit Amount>=
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxTextBox ID="txtDebitCredit" runat="server" Width="140px" ClientInstanceName="ctxtDebitCredit" HorizontalAlign="Right" Font-Size="13px" meta:resourcekey="txtdebitResource1">
                                                <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />

                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr id="TrSeg" runat="server">
                                        <td class="gridcellleft">Segment</td>
                                        <td class="gridcellleft">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdAllSegment" runat="server" GroupName="c1" onclick="AllSelct('a','S')" Checked="True" />
                                                    </td>
                                                    <td>All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdSelSegment" runat="server" GroupName="c1" onclick="AllSelct('b','S')" />
                                                    </td>
                                                    <td>Selected
                                                    </td>
                                                    <td>[<span id="Span2" runat="server" style="color: Maroon"></span>]
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">Company :</td>
                                        <td>
                                            <asp:RadioButton ID="RdbAllCompany" runat="server" GroupName="dd" onclick="fnCompany('a')" Checked="True" />
                                            All
                                                <asp:RadioButton ID="RdbCurrentCompany" runat="server" GroupName="dd"
                                                    onclick="fnCompany('a')" />
                                            Current
                                                <asp:RadioButton ID="RdbSelectedCompany" runat="server" GroupName="dd" onclick="fnCompany('b')" />Selected
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
                                                                    <asp:DropDownList ID="ddlgrouptype" runat="server" Font-Size="12px" onchange="fngrouptype(this.value)">
                                                                    </asp:DropDownList>
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
                                    <tr>
                                        <td class="gridcellleft"></td>
                                        <td class="gridcellleft">&nbsp;<table>
                                            <tr>
                                                <td>&nbsp;<asp:CheckBox ID="ChkConsolidateBy" runat="server" />Consolidated By Group/Branch&nbsp;</td>

                                            </tr>
                                        </table>
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
                                                        <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="AllSelct('b','C')" /></td>
                                                    <td>Selected Client</td>
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
                                                    <td>
                                                        <asp:RadioButton ID="ResBranch" runat="server" GroupName="E1" Checked="true" onclick="AllSelct('a','E')" /></td>
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
                                </table>
                            </td>
                            <td style="vertical-align: top">
                                <table>
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
                                                            <asp:ListItem>Company</asp:ListItem>
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
                    <asp:Button ID="btnShow" runat="server" Text="Export To Excel" CssClass="btnUpdate" Height="23px"
                        OnClientClick="selecttion()" Width="144px" OnClick="btnShow_Click" />&nbsp;
                        <asp:Button ID="BtnRefresh" runat="server" Text="Refresh" CssClass="btnUpdate" Height="23px"
                            OnClientClick="selecttion()" Width="144px" OnClick="BtnRefresh_Click" /></td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td id="TrCancel" style="display: none">
                    <input id="btnCalcel" type="button" value="Close" onclick="btnCalcel_Click()" class="btnUpdate" />
                </td>
            </tr>

        </table>
        <table style="background-color: #DDECFE;" width="100%" height="200px">
            <tr>
                <td>
                    <asp:HiddenField ID="HDNAccInd" runat="server" />
                    <asp:HiddenField ID="HdnMainAcc" runat="server" />
                    <asp:HiddenField ID="HdnClients" runat="server" />
                    <asp:HiddenField ID="HdnGroup" runat="server" />
                    <asp:HiddenField ID="HdnBranchId" runat="server" />
                    <asp:HiddenField ID="HdnSegment" runat="server" />
                    <asp:HiddenField ID="HdnEmployee" runat="server" />
                    <asp:HiddenField ID="HDNSeg" runat="server" />
                    <asp:HiddenField ID="hidden_Company" runat="server" />
                </td>
                <td style="display: none">
                    <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
