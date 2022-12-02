<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_SettlementbillsCM" CodeBehind="SettlementbillsCM.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
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

        .grid_scroll {
            overflow-y: no;
            overflow-x: scroll;
            width: 90%;
            scrollbar-base-color: #C0C0C0;
        }
    </style>
    <script language="javascript" type="text/javascript">
        groupvalue = "";

        function Page_Load()///Call Into Page Load
        {
            SendmailFilter();
            Hide('displayAll');
            Show('tab2');
            Hide('showFilter');
            Hide('tr_filter');
            height();

        }

        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollwidth;
        }

        function Clients(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Clients';
                Show('showFilter');
            }
            selecttion();
        }
        function Branch(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Branch';
                Show('showFilter');
            }
            selecttion();
        }
        function Group(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Group';
                Show('showFilter');
            }
            selecttion();
        }
        function FunClientScrip(objID, objListFun, objEvent) {
            var cmbVal;
            if (groupvalue == "") {
                cmbVal = document.getElementById('cmbsearchOption').value;
                cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
            }
            else {
                if (document.getElementById('cmbsearchOption').value == "Clients") {
                    if (document.getElementById('ddlGroup').value == "0")//////////////Group By  selected are branch
                    {
                        if (document.getElementById('rdbranchAll').checked == true) {
                            cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                            cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;

                        }
                        else {
                            cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                            cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue;

                        }
                    }
                    else //////////////Group By selected are Group
                    {
                        if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                            cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                            cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                        }
                        else {
                            cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                            cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue;
                        }
                    }
                }
                else {
                    cmbVal = document.getElementById('cmbsearchOption').value;
                    cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
                }
            }

            ajax_showOptions(objID, objListFun, objEvent, cmbVal);

        }
        function btnAddsubscriptionlist_click() {

            var cmb = document.getElementById('cmbsearchOption');
            var userid = document.getElementById('txtSelectionID');
            if (userid.value != '') {
                var ids = document.getElementById('txtSelectionID_hidden');
                var listBox = document.getElementById('lstSlection');
                var tLength = listBox.length;


                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtSelectionID');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!')
            var s = document.getElementById('txtSelectionID');
            s.focus();
            s.select();

        }

        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSlection');

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

            Hide('showFilter');
            document.getElementById('btn_show').disabled = false;
            SendmailFilter();
            window.frameElement.height = document.body.scrollHeight;
        }


        function btnRemovefromsubscriptionlist_click() {

            var listBox = document.getElementById('lstSlection');
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
        function fnddlGroup(obj) {
            if (obj == "0") {
                Hide('td_group');
                Show('td_branch');
            }
            else {
                Show('td_group');
                Hide('td_branch');
                var btn = document.getElementById('btnhide');
                btn.click();
            }
            selecttion();
        }
        function fngrouptype(obj) {
            if (obj == "0") {
                Hide('td_allselect');
                alert('Please Select Group Type !');
            }
            else {
                Show('td_allselect');
            }
            selecttion();
        }
        function NORECORD() {
            Show('tab2');
            Hide('displayAll');
            Hide('tr_filter');
            Hide('showFilter');
            alert('No Record Found');
            height();
        }
        function DISPLAY() {
            Hide('tab2');
            Show('displayAll');
            Hide('showFilter');
            Show('tr_filter');
            height();
        }
        function Filter() {
            SendmailFilter();
            Show('tab2');
            Hide('displayAll');
            Hide('tabddl');
            selecttion();
            Hide('tr_filter');
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
        function btndisplay() {
            document.getElementById('BTNLODINGDDLGROUP').click();
        }
        function selecttion() {
            var combo = document.getElementById('ddlExport');
            combo.value = 'Ex';
        }
        FieldName = 'lstSlection';


        //THIS IS FOR EMAIL

        function btnAddEmailtolist_click() {

            var cmb = document.getElementById('cmbsearch');

            var userid = document.getElementById('txtSelectID');
            if (userid.value != '') {
                var ids = document.getElementById('txtSelectID_hidden');
                var listBox = document.getElementById('SelectionList');
                var tLength = listBox.length;


                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtSelectID');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!')
            var s = document.getElementById('txtSelectID');
            s.focus();
            s.select();

        }


        function callAjax1(obj1, obj2, obj3) {
            document.getElementById('SelectionList').style.display = 'none';
            var combo = document.getElementById("cmbsearch");
            var set_value = combo.value
            var obj4 = 'Main';

            if (set_value == '16') {
                ajax_showOptions(obj1, 'GetLeadId', obj3, set_value, obj4)
            }
            else {

                ajax_showOptions(obj1, obj2, obj3, set_value, obj4)
            }

        }

        function clientselection() {
            var listBoxSubs = document.getElementById('SelectionList');

            var cmb = document.getElementById('cmbsearch');

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

                CallServer1(sendData, "");
                document.getElementById('ShowSelectUser').style.display = 'none';
                document.getElementById('ShowTable').style.display = 'none';
                document.getElementById('showFilter1').style.display = 'inline';
            }
            else {
                alert("Please select email from list.")
            }

            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }

            window.frameElement.height = document.body.scrollHeight;
        }

        function btnRemoveEmailFromlist_click() {

            var listBox = document.getElementById('SelectionList');
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

        function ReceiveSvrData(rValue) {
            var Data = rValue.split('~');
            if (Data[0] == 'Clients') {
            }
        }

        function Sendmail() {


            var cmbVal = document.getElementById('ddlGroup').value;
            if (cmbVal == "0") {

                document.getElementById('lblSelectBrCl').value = 'Respective Branch';
                document.getElementById('ShowSelectUser').style.display = 'inline';
                document.getElementById('ShowTable').style.display = 'none';
                document.getElementById('showFilter1').style.display = 'inline';
                window.frameElement.height = document.body.scrollHeight;

            }
            else if (cmbVal == "1") {

                document.getElementById('lblSelectBrCl').value = 'Respective Group';
                document.getElementById('ShowSelectUser').style.display = 'inline';
                document.getElementById('ShowTable').style.display = 'none';
                document.getElementById('showFilter1').style.display = 'inline';
                window.frameElement.height = document.body.scrollHeight;

            }




            //            document.getElementById('ShowSelectUser').style.display='inline';
            //            document.getElementById('ShowTable').style.display='none';
            //            document.getElementById('showFilter1').style.display='inline';
            //             window.frameElement.height = document.body.scrollHeight;

        }
        function ForFilterOff() {
            Hide('tab2');
            document.getElementById('ShowSelectUser').style.display = 'none';
            selecttion();
            document.getElementById('ShowTable').style.display = 'none';
            document.getElementById('showFilter1').style.display = 'none';
            Show('tdgrdBrkgclient');
            Show('tr_export');
            Hide('tr_btn');
            Hide('tr_date');
            Hide('tr_under');

            window.frameElement.height = document.body.scrollHeight;
        }
        function MailsendT() {

            window.frameElement.height = document.body.scrollHeight;
            alert("Mail Sent Successfully");
        }
        function MailsendF() {
            window.frameElement.height = document.body.scrollHeight;
            alert("Error on sending!Try again..");
        }
        function MailsendFT() {
            alert("Email id could not found!Try again...");
        }

        function keyVal(obj) {
            document.getElementById('SelectionList').style.display = 'inline';

        }

        function SendmailFilter() {
            document.getElementById('ShowSelectUser').style.display = 'none';
            document.getElementById('ShowTable').style.display = 'none';
            document.getElementById('showFilter1').style.display = 'none';


            window.frameElement.height = document.body.scrollHeight;

        }
        function SelectUserClient(obj) {
            if (obj == 'Client') {

                document.getElementById('ShowSelectUser').style.display = 'inline';
                document.getElementById('ShowTable').style.display = 'none';
                document.getElementById('showFilter1').style.display = 'inline';

            }
            else if (obj == 'User') {
                document.getElementById('ShowTable').style.display = 'inline';
                document.getElementById('ShowSelectUser').style.display = 'inline';
                document.getElementById('showFilter1').style.display = 'none';
            }
        }

    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {

            var j = rValue.split('~');
            var btn = document.getElementById('btnhide');
            if (j[0] == 'Group') {
                groupvalue = j[1];
                btn.click();
            }
            if (j[0] == 'Branch') {
                groupvalue = j[1];
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
    <div>


        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 22px;">
                    <strong><span id="SpanHeader" style="color: #000099">Settlement Bills</span></strong>
                </td>
                <td class="EHEADER" width="25%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="Sendmail();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Send Email</span></a>||
                      <a href="javascript:void(0);"
                          onclick="Filter();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                        <asp:ListItem Value="P">PDF</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="tab2">
            <tr>
                <td align="left">
                    <table>
                        <tr valign="top">
                            <td>
                                <table>

                                    <tr id="tr_Group">
                                        <td>Group By</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                <asp:ListItem Value="0">Branch</asp:ListItem>
                                                <asp:ListItem Value="1">Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" id="td_branch">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="c" onclick="Branch('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="c" onclick="Branch('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="td_group" style="display: none;" colspan="2">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
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
                                                        <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="e"
                                                            onclick="Group('a')" />
                                                        All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="e" onclick="Group('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="tr_Clients">
                                        <td>Clients :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="d" onclick="Clients('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radPOAClient" runat="server" GroupName="d" onclick="Clients('a')" />POA Client
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="d" onclick="Clients('b')" />
                                            Selected
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>

                                    <tr id="tr_Settlementno">
                                        <td>Settelment No: </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtset" runat="server" Width="250px" Font-Size="12px" onkeyup="ajax_showOptions(this,'SearchSettlementWithoutbracket',event)"></asp:TextBox>
                                        </td>


                                    </tr>
                                    <tr id="tr_show">
                                        <td colspan="6">
                                            <asp:Button ID="btn_show" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                Width="101px" OnClientClick="selecttion()" OnClick="btn_show_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table width="100%" id="showFilter">
                                    <tr>
                                        <td style="text-align: right; vertical-align: top; height: 134px;">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                                        id="TdFilter">
                                                        <span id="spanunder"></span><span id="spanclient">
                                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                                Enabled="false">

                                                                <asp:ListItem>Clients</asp:ListItem>
                                                                <asp:ListItem>Branch</asp:ListItem>
                                                                <asp:ListItem>Group</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                                    style="color: #009900; font-size: 8pt;"> </span></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="290px"></asp:ListBox>
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
        </table>
        <table id="tab3">
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnhide" OnClientClick="selecttion()" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:TextBox ID="txtset_hidden" runat="server" Width="5px"></asp:TextBox>
                    <asp:Button ID="BTNLODINGDDLGROUP" OnClientClick="selecttion()" runat="server" Text="BTN" OnClick="BTNLODINGDDLGROUP_Click" />

                </td>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50%; background-color: white; layer-background-color: white; height: 80; width: 150;'>
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
    </div>

    <%--        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>--%>
    <table id="MainFilter">
        <tr>
            <td>
                <table id="ShowSelectUser">
                    <tr>
                        <td valign="top">
                            <asp:RadioButton ID="rbOnlyClient" runat="server" Checked="true" GroupName="h" />
                        </td>
                        <td valign="top">
                            <%-- Respective Branch--%>
                            <asp:TextBox ID="lblSelectBrCl"
                                runat="server"></asp:TextBox>
                        </td>
                        <td valign="top">
                            <asp:RadioButton ID="rbClientUser" runat="server" GroupName="h" />
                        </td>
                        <td valign="top">Selected User
                        </td>

                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table id="ShowTable">
                    <tr>
                        <td width="70px" style="text-align: left;">Type:</td>
                        <td class="gridcellleft" style="vertical-align: top; text-align: left" id="Td1">
                            <span id="span1">
                                <asp:DropDownList ID="cmbsearch" runat="server" Width="150px" Font-Size="12px">
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="70px" style="text-align: left;">Select User:</td>
                        <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter1">
                            <span id="spanal2">
                                <asp:TextBox ID="txtSelectID" runat="server" Font-Size="12px" Width="285px"></asp:TextBox></span>
                            <a id="A3" href="javascript:void(0);" onclick="btnAddEmailtolist_click()"><span style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span style="color: #009900; font-size: 8pt;"> </span>
                        </td>
                    </tr>
                    <tr>
                        <td width="70px" style="text-align: left;"></td>
                        <td style="text-align: left; vertical-align: top; height: 134px;">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>&nbsp;&nbsp;<asp:ListBox ID="SelectionList" runat="server" Font-Size="12px" Height="90px"
                                        Width="290px"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <a id="AA2" href="javascript:void(0);" onclick="clientselection()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <a id="AA1" href="javascript:void(0);" onclick="btnRemoveEmailFromlist_click()"><span
                                                        style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td width="70px" style="text-align: left;"></td>
                        <td style="height: 23px">
                            <asp:TextBox ID="txtSelectID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                            <asp:TextBox ID="dtFrom_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                            <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                        </td>
                    </tr>
                    <%--  <tr>
                                    <td style="text-align:left;">
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btnUpdate" Text="Send" /></td>
                                    </tr>--%>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table id="showFilter1">
                    <tr>
                        <td>
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btnUpdate"
                                Text="Send" OnClientClick="selecttion()" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

    </table>
    <%--            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>--%>
    <div id="displayAll">
        <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table id="brokerFilter" runat="server">



                    <tr id="tr_DIVdisplayPERIOD">
                        <td colspan="3" style="width: 50%">
                            <div id="DIVdisplayPERIOD" runat="server">
                            </div>
                        </td>
                        <td align="right">
                            <asp:Label ID="listRecord" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td style="height: 44px">
                            <asp:LinkButton ID="lnkPrevClient" runat="server" CommandName="Prev" Text="[Prev]"
                                OnCommand="NavigationLinkC_Click" OnClientClick="javascript:selecttion();"> </asp:LinkButton>
                        </td>
                        <td style="height: 44px">
                            <asp:DropDownList ID="cmbgroup" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True" onchange="btndisplay()">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 44px">
                            <asp:LinkButton ID="lnkNextClient" runat="server" CommandName="Next" Text="[Next]"
                                OnCommand="NavigationLinkC_Click" OnClientClick="javascript:selecttion();"> </asp:LinkButton>&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="BTNLODINGDDLGROUP" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server" ID="u1">
            <ContentTemplate>
                <table width="100%" border="1">

                    <tr>
                        <td>
                            <div id="display" runat="server">
                            </div>
                        </td>
                    </tr>
                    <asp:HiddenField ID="Totalgrp" runat="server" />
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

