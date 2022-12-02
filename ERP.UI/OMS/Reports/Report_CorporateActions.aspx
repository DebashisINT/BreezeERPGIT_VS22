<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Report_CorporateActions" EnableEventValidation="false" CodeBehind="Report_CorporateActions.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <%-- <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>
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
            width: 55%;
            scrollbar-base-color: #C0C0C0;
        }
    </style>
    <script language="javascript" type="text/javascript">
        groupvalue = "";
        function Page_Load()///Call Into Page Load
        {
            height();
            Hide('showFilter');
            Hide('tblDisplay');
            Hide('td_filter');
            document.getElementById('hiddencount').value = 0;
            btndisplay('1');

        }
        function heightlight(obj) {

            var colorcode = obj.split('&');

            if ((document.getElementById('hiddencount').value) == 0) {
                prevobj = '';
                prevcolor = '';
                document.getElementById('hiddencount').value = 1;

            }
            document.getElementById(obj).style.backgroundColor = '#ffe1ac';

            if (prevobj != '') {
                document.getElementById(prevobj).style.backgroundColor = prevcolor;
            }
            prevobj = obj;
            prevcolor = colorcode[1];

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
            window.frameElement.width = document.body.scrollWidth;
        }


        function FunClientScrip(objID, objListFun, objEvent) {
            var cmbVal;
            var ExSeg;
            ExSeg = document.getElementById('HiddenField_ExSeg').value;
            if (document.getElementById('cmbsearchOption').value == "Clients") {
                if (document.getElementById('ddlGroup').value == "0" || document.getElementById('ddlGroup').value == "2")//////////////Group By  selected are branch
                {
                    if (document.getElementById('ddlGroup').value == "0")/////branch selection
                    {
                        if (document.getElementById('rdbranchAll').checked == true) {
                            cmbVal = 'ClientsBranch' + '~' + 'ALL';
                        }
                        else {
                            cmbVal = 'ClientsBranch' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Branch').value;
                        }
                    }
                    if (document.getElementById('ddlGroup').value == "2")/////branch-group selection
                    {
                        if (document.getElementById('rdbranchAll').checked == true) {
                            cmbVal = 'ClientsBranchGroup' + '~' + 'ALL';
                        }
                        else {
                            cmbVal = 'ClientsBranchGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_BranchGroup').value;
                        }
                    }
                }
                else if (document.getElementById('ddlGroup').value == "1")//////////////Group By selected are Group
                {
                    if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                        cmbVal = 'ClientsGroup' + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                    }
                    else {
                        cmbVal = 'ClientsGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Group').value;
                    }
                }
                else if (document.getElementById('ddlGroup').value == "3")///////client wise
                {
                    cmbVal = 'ClientsBranch' + '~' + 'ALL';
                }
            }
            else if (document.getElementById('cmbsearchOption').value == "ScripsExchange") {
                var dateto;
                dateto = new Date(dtFrom.GetDate());
                dateto = parseInt(dateto.getMonth() + 1) + '-' + dateto.getDate() + '-' + dateto.getFullYear();

                var criteritype = 'B';
                if (ExSeg == '1' || ExSeg == '2' || ExSeg == '4' || ExSeg == '5' || ExSeg == '15') {
                    criteritype = ' AND equity_effectuntil>="' + dateto + '"  ';
                }
                else {
                    criteritype = ' AND Commodity_ExpiryDate>="' + dateto + '"  ';
                }
                criteritype = criteritype.replace('"', "'");
                criteritype = criteritype.replace('"', "'");
                cmbVal = document.getElementById('cmbsearchOption').value + '~' + 'Date' + '~' + criteritype;
            }
            else {
                cmbVal = document.getElementById('cmbsearchOption').value;
            }

            ajax_showOptions(objID, 'ShowClientFORMarginStocks', objEvent, cmbVal);

        }
        function Filter() {

            Show('tab_selection');
            Hide('showFilter');
            Hide('tblDisplay');
            Hide('td_filter');
            document.getElementById('hiddencount').value = 0;
            height();

        }
        function fn_ScripsExchange(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'ScripsExchange';
                Show('showFilter');

            }
            selecttion();
            height();

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
            var s = document.getElementById('txtClientSelectionID');
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
            document.getElementById('ShowSelectUser').style.display = 'none';
            document.getElementById('btn_show').disabled = false;
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

        function selecttion() {
            var combo = document.getElementById('ddlExport');
            combo.value = 'Ex';
        }

        function line() {
            height();
            Hide('tab_selection');
            Show('td_filter');
            Show('tblDisplay');
            document.getElementById('hiddencount').value = 0;
            window.frameElement.height = document.body.scrollHeight;

        }
        FieldName = 'lstSlection';


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

        function Group(obj) {
            if (obj == "a") {
                Hide('showFilter');
            }
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Group';
                Show('showFilter');
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


    </script>
    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {

            var j = rValue.split('~');
            var btn = document.getElementById('btnhide');


            if (j[0] == 'Clients') {
                document.getElementById('HiddenField_Client').value = j[1];
            }
            if (j[0] == 'Segment') {
                document.getElementById('HiddenField_Segment').value = j[1];
            }
            if (j[0] == 'ScripsExchange') {

                document.getElementById('HiddenField_Instrument').value = j[1];

            }

            if (j[0] == 'Branch') {
                document.getElementById('HiddenField_Branch').value = j[1];
            }
            if (j[0] == 'BranchGroup') {
                document.getElementById('HiddenField_BranchGroup').value = j[1];
            }
            if (j[0] == 'Group') {
                document.getElementById('HiddenField_Group').value = j[1];
                btn.click();
            }
        }
        function btndisplay(obj) {

            if (obj == '1')////select screen
            {

                Show('td_Screen');
                Hide('td_Excel');
                Hide('showFilter');
            }
            if (obj == '2')////select export
            {

                Hide('td_Screen');
                Show('td_Excel');
                Hide('td_mail');
                Hide('showFilter');
            }
            if (obj == '3')////select email
            {

                Hide('td_Screen');
                Hide('td_Excel');
                Show('showFilter');
            }

        }
        function fnddlGeneration(obj) {
            btndisplay(obj);
        }
        function NORECORD() {
            Show('tab_selection');
            Hide('td_filter');
            Hide('showFilter');
            Hide('tblDisplay');
            btndisplay('1');
            Hide('tblDisplay');
            alert('No record Found');
            height();

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
            <tr class="EHEADER">
                <td colspan="0" style="text-align: center; height: 22px;">
                    <strong><span id="SpanHeader" style="color: #000099">Corporate Actions Report</span></strong>
                </td>
                <td style="width: 25%;" id="td_filter">
                    <a href="javascript:void(0);"
                        onclick="Filter();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a> &nbsp; &nbsp;||&nbsp;
                        <asp:DropDownList ID="ddlExport" Font-Size="Smaller" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                        </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="tab_selection">
            <tr>
                <td valign="top">
                    <table>
                        <tr>
                            <td bgcolor="#B7CEEC" class="gridcellleft">Generate Type :
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlGeneration" runat="server" Width="130px" Font-Size="12px"
                                    onchange="fnddlGeneration(this.value)">
                                    <asp:ListItem Value="1">Screen</asp:ListItem>
                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#B7CEEC" class="gridcellleft">CA Type : </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" Width="80px" Font-Size="12px">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="1">Bonus</asp:ListItem>
                                    <asp:ListItem Value="2">Split</asp:ListItem>
                                    <asp:ListItem Value="3">Rights</asp:ListItem>
                                    <asp:ListItem Value="4">Dividend</asp:ListItem>
                                    <asp:ListItem Value="5">AGM</asp:ListItem>
                                    <asp:ListItem Value="6">Other</asp:ListItem>
                                </asp:DropDownList>
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
                                                onclick="Group('a')" />
                                            All
                                                <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="Group('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="gridcellleft" bgcolor="#B7CEEC">Date :
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtFrom">
                                    <DropDownButton Text="From">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtTo">
                                    <DropDownButton Text="To">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>

                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">Products :</td>
                            <td>
                                <asp:RadioButton ID="rdInstrumentAll" runat="server" Checked="True" GroupName="d"
                                    onclick="fn_ScripsExchange('a')" />
                                All
                            </td>
                            <td colspan="2">
                                <asp:RadioButton ID="rdInstrumentSelected" runat="server" GroupName="d" onclick="fn_ScripsExchange('b')" />
                                Selected
                            </td>

                        </tr>

                        <tr>
                            <td id="td_Screen" align="right">
                                <asp:Button ID="btn_show" OnClientClick="selecttion()" CssClass="btnUpdate" Height="20px"
                                    Width="90px" runat="server" Text="Screen" OnClick="btn_show_Click" />
                                <td id="td_Excel" align="right">
                                    <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                        Width="101px" OnClientClick="selecttion()" OnClick="btnExcel_Click" />
                                </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" colspan="4">
                    <table width="100%" id="showFilter">
                        <tr>
                            <td style="text-align: right; vertical-align: top; height: 134px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: top; text-align: right" id="TdFilter">
                                            <span id="spanunder"></span><span id="spanclient">
                                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="200px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox></span>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
                                                <asp:ListItem>ScripsExchange</asp:ListItem>
                                            </asp:DropDownList>
                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                    style="color: #009900; font-size: 8pt;"> </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: right"></td>
                                    </tr>
                                    <tr>
                                        <td align="left">
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
            <tr>
                <td colspan="5">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
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

        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnhide" runat="server" Text="Button" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Instrument" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:HiddenField ID="HiddenField_SettNo" runat="server" />
                    <asp:HiddenField ID="HiddenField_Setttype" runat="server" />
                    <asp:HiddenField ID="HiddenField_ExSeg" runat="server" />
                </td>
            </tr>
        </table>
        <div id="tblDisplay" style="display: none;" width="100%">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">
                        <tr style="display: none;">
                            <td>
                                <asp:HiddenField ID="hiddencount" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="displayALL" runat="server">
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

