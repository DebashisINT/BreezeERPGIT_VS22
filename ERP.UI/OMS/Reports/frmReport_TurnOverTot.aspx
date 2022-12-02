<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_TurnOverTot" Codebehind="frmReport_TurnOverTot.aspx.cs" %>

<<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

    <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32767;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	form{
		display:inline;
	}
	

	</style>

    <script language="javascript" type="text/javascript">


        function Page_Load()///Call Into Page Load
        {

            Hide('showFilter');

            Hide('td_filter');
            Show('Tr_Scrip');
            Hide('Tr_Broker');
            Hide('Tr_Assets');
            document.getElementById('hiddencount').value = 0;

            FnConsolidated('Exchange');

            FnddlGeneration('1');

            height();




        }
        function height() {
            if (document.body.scrollHeight >= 400) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '450px';
            }
            window.frameElement.width = document.body.scrollwidth;
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }

        function FunClientScrip(objID, objListFun, objEvent) {
            var cmbVal;

            if (document.getElementById('cmbsearchOption').value == "Clients") {
                if (document.getElementById('ddlGroup').value == "0" || document.getElementById('ddlGroup').value == "2")//////////////Group By  selected are branch
                {
                    if (document.getElementById('ddlGroup').value == "0") {
                        if (document.getElementById('rdbranchAll').checked == true) {
                            cmbVal = 'ClientsBranch' + '~' + 'ALL';
                        }
                        else {
                            cmbVal = 'ClientsBranch' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Branch').value;
                        }
                    }
                    if (document.getElementById('ddlGroup').value == "2") {
                        if (document.getElementById('rdbranchAll').checked == true) {
                            cmbVal = 'ClientsBranchGroup' + '~' + 'ALL';
                        }
                        else {
                            cmbVal = 'ClientsBranchGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_BranchGroup').value;
                        }
                    }
                }
                else //////////////Group By selected are Group
                {
                    if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                        cmbVal = 'ClientsGroup' + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                    }
                    else {
                        cmbVal = 'ClientsGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Group').value;
                    }
                }
            }

            else {
                cmbVal = document.getElementById('cmbsearchOption').value;
                cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
            }

            ajax_showOptions(objID, objListFun, objEvent, cmbVal);

        }


        function fnClients(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Clients';
                Show('showFilter');
            }
            selecttion();
        }
        function fnInstrument(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Product';
                Show('showFilter');
            }
            selecttion();
        }

        function fnTerminalID(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'UserID';
                Show('showFilter');
            }
            selecttion();
        }
        function fnBranch(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                if (document.getElementById('ddlGroup').value == "0") {
                    document.getElementById('cmbsearchOption').value = 'Branch';
                }
                if (document.getElementById('ddlGroup').value == "2") {
                    document.getElementById('cmbsearchOption').value = 'BranchGroup';
                }

                Show('showFilter');
            }
            selecttion();
        }
        function fnGroup(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Group';
                Show('showFilter');
            }
            selecttion();
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
            document.getElementById('btnScreen').disabled = false;
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
        function fnbroker(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Broker';
                Show('showFilter');
            }
            selecttion();
        }
        function fnddlview(obj) {
            if (obj == "1") {
                Show('tr_client');
                Hide('Tr_Broker');
            }
            else {
                Hide('tr_client');
                Show('Tr_Broker');

            }
            selecttion();
        }
        function fnCompany(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                Show('showFilter');
                document.getElementById('cmbsearchOption').value = 'Company';
            }
            selecttion();
        }
        function fnddlGroup(obj) {
            if (obj == "0" || obj == "2") {
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
        function FnConsolidated(obj) {
            if (obj == "Exchange") {
                Hide('Tr_RptViewClient');
                Show('Tr_RptViewExchange');
                Hide('Td_ConsolidateGrpBranch');
                Hide('Td_ConsolidatedAcrossSegment');
                FnrptViewExchange(document.getElementById('DdlrptViewExchange').value);
            }
            else {
                Show('Tr_RptViewClient');
                Hide('Tr_RptViewExchange');
                FnrptViewClient(document.getElementById('DdlrptViewClient').value);

            }

        }
        function FnrptViewClient(obj) {
            if (obj == "12") {
                Hide('Td_ConsolidateGrpBranch');
                Show('Td_ConsolidatedAcrossSegment');
                Show('Tr_Company');
                Hide('tr_productinstrument');
            }
            else if (obj == '11') {
                Hide('Td_ConsolidatedAcrossSegment');
                Hide('Td_ConsolidateGrpBranch');
                Show('Tr_Company');
                Hide('tr_productinstrument');

            }
            else {
                Hide('Td_ConsolidatedAcrossSegment');
                Show('Td_ConsolidateGrpBranch');
                Hide('Tr_Company');
                Show('tr_productinstrument');

            }

        }
        function FnrptViewExchange(obj) {
            if (obj != '10') {
                Hide('Tr_Company');
                Show('tr_productinstrument');
            }
            else {
                Show('Tr_Company');
                Hide('tr_productinstrument');
            }
        }
        function fnRecord(obj) {
            if (obj == "1") {
                Hide('td_filter');
                Show('tab1');
                Hide('displayAll');
                Hide('showFilter');
                alert('No Record Found!!');
            }
            if (obj == "2") {
                Show('td_filter');
                Hide('tab1');
                Show('displayAll');
                Hide('showFilter');

            }
            if (obj == "3") {
                Hide('td_filter');
                Show('tab1');
                Hide('displayAll');
                Hide('showFilter');

            }
            document.getElementById('hiddencount').value = 0;

            height();
            selecttion();
        }

        function FnddlGeneration(obj) {
            if (obj == "1") {
                Show('td_Screen');
                Hide('td_Export');
            }
            if (obj == "2") {
                Hide('td_Screen');
                Show('td_Export');
            }
            Hide('showFilter');
            height();
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
        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = 'Ex';
        }

        function FnScrips(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'ScripsExchange';
                Show('showFilter');
                //document.getElementById('btnshow').disabled=true;
            }
            selecttion();
            height();
        }
        function SearchByAssetOrScript(obj) {
            if (obj == "ASSETS") {
                document.getElementById("Tr_Assets").style.display = "inline";
                document.getElementById("rdbunderlyingall").checked = true;
                document.getElementById("Tr_Scrip").style.display = "none";
            }
            else {
                document.getElementById("Tr_Assets").style.display = "none";
                document.getElementById("Tr_Scrip").style.display = "inline";
            }

        }
        function fnunderlying(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Product';
                Show('showFilter');
            }
            selecttion();
            height();
        }

        FieldName = 'lstSlection';
    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            var j = rValue.split('~');
            if (j[0] == 'Branch') {
                document.getElementById('HiddenField_Branch').value = j[1];
            }
            if (j[0] == 'Broker') {
                document.getElementById('HiddenField_Broker').value = j[1];
            }
            if (j[0] == 'Group') {
                document.getElementById('HiddenField_Group').value = j[1];
            }
            if (j[0] == 'Clients') {
                document.getElementById('HiddenField_Client').value = j[1];
            }
            if (j[0] == 'BranchGroup') {
                document.getElementById('HiddenField_BranchGroup').value = j[1];
            }
            if (j[0] == 'ScripsExchange') {
                document.getElementById('HiddenField_ScripsExchange').value = j[1];
            }
            if (j[0] == 'Product') {
                document.getElementById('HiddenField_Product').value = j[1];
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
        </asp:ScriptManager>

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

        <div>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                        <strong><span id="SpanHeader" style="color: #000099">Turnover & Tot Reports</span></strong></td>
                    <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                        <a href="javascript:void(0);" onclick="fnRecord(3);"><span style="color: Blue; text-decoration: underline;
                            font-size: 8pt; font-weight: bold">Filter</span></a>
                        <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table id="tab1" border="0" cellpadding="0" cellspacing="0">
                <tr valign="top">
                    <td>
                        <table>
                            <tr>
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                For A Period :
                                            </td>
                                            <td class="gridcellleft">
                                                <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="DtFrom">
                                                    <dropdownbutton text="From">
                                                </dropdownbutton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td class="gridcellleft">
                                                <dxe:ASPxDateEdit ID="DtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="DtTo">
                                                    <dropdownbutton text="To">
                                                </dropdownbutton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Report For :</td>
                                            <td>
                                                <asp:DropDownList ID="DdlRptFor" runat="server" Width="250px" Font-Size="12px" onchange="FnConsolidated(this.value)">
                                                    <asp:ListItem Value="Exchange">Exchange</asp:ListItem>
                                                    <asp:ListItem Value="Client">Client</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_RptViewClient">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Report View :</td>
                                            <td>
                                                <asp:DropDownList ID="DdlrptViewClient" runat="server" Width="250px" Font-Size="12px"
                                                    onchange="FnrptViewClient(this.value)">
                                                    <asp:ListItem Value="1">Date Wise</asp:ListItem>
                                                    <asp:ListItem Value="2">Asset Wise</asp:ListItem>
                                                    <asp:ListItem Value="3">Consolidated [Pur/Sale]</asp:ListItem>
                                                    <asp:ListItem Value="11">Month Wise -Across Segment +Branch/Group </asp:ListItem>
                                                    <asp:ListItem Value="12">Month Wise -Across Segment +Client </asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_RptViewExchange">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Report View :</td>
                                            <td>
                                                <asp:DropDownList ID="DdlrptViewExchange" runat="server" Width="250px" Font-Size="12px"
                                                    onchange="FnrptViewExchange(this.value)">
                                                    <asp:ListItem Value="1">Date Wise</asp:ListItem>
                                                    <asp:ListItem Value="2">Asset Wise</asp:ListItem>
                                                    <asp:ListItem Value="3">Consolidated [Pur/Sale]</asp:ListItem>
                                                    <asp:ListItem Value="4">Consolidated</asp:ListItem>
                                                    <asp:ListItem Value="10">Month Wise Across Segment</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_GroupBy">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Group By</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                    <asp:ListItem Value="0">Branch</asp:ListItem>
                                                    <asp:ListItem Value="1">Group</asp:ListItem>
                                                    <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2" id="td_branch">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="z" onclick="fnBranch('a')" />
                                                            All
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="z" onclick="fnBranch('b')" />Selected
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
                                                                onclick="fnGroup('a')" />
                                                            All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="fnGroup('b')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_viewby">
                                <td>
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                View By :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlviewby" runat="server" Width="100px" Font-Size="12px" onchange="fnddlview(this.value)">
                                                    <asp:ListItem Value="1">Client</asp:ListItem>
                                                    <asp:ListItem Value="2">Broker</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr_client">
                                <td>
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Clients :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="fnClients('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdPOAClient" runat="server" GroupName="c" onclick="fnClients('a')" />POA
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="fnClients('b')" />
                                                Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_Broker">
                                <td>
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Broker :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbbrokerall" runat="server" Checked="True" GroupName="M" onclick="fnbroker('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbbrokerselected" runat="server" GroupName="M" onclick="fnbroker('b')" />
                                                Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_Company">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Company :</td>
                                            <td>
                                                <asp:RadioButton ID="RdbAllCompany" runat="server" GroupName="dh" onclick="fnCompany('a')" />
                                                All
                                                <asp:RadioButton ID="RdbCurrentCompany" runat="server" Checked="True" GroupName="dh"
                                                    onclick="fnCompany('a')" />
                                                Current
                                                <asp:RadioButton ID="RdbSelectedCompany" runat="server" GroupName="dh" onclick="fnCompany('b')" />Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Segment :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbSegmentAll" runat="server" GroupName="ab" />All
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButton ID="rdbSegmentSelected" runat="server" Checked="True" GroupName="ab" />Specific
                                                [ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr_productinstrument" runat="server">
                                <td colspan="4">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td bgcolor="#b7ceec" class="gridcellleft">
                                                Search By</td>
                                            <td>
                                                <asp:RadioButton ID="rdbAssets" runat="server" GroupName="SearchBy" onclick="SearchByAssetOrScript('ASSETS')"
                                                    Text="Product(s)" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbscrips" runat="server" Checked="True" GroupName="SearchBy"
                                                    onclick="SearchByAssetOrScript('SCRIP')" Text="Instument(s)" /></td>
                                        </tr>
                                        <tr id="Tr_Scrip">
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Instument(s)</td>
                                            <td>
                                                <asp:RadioButton ID="rdInstrumentAll" runat="server" Checked="True" GroupName="d"
                                                    onclick="FnScrips('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdInstrumentSelected" runat="server" GroupName="d" onclick="FnScrips('b')" />
                                                Selected
                                            </td>
                                        </tr>
                                        <tr id="Tr_Assets">
                                            <td bgcolor="#b7ceec" class="gridcellleft">
                                                Product(s)</td>
                                            <td>
                                                <asp:RadioButton ID="rdbunderlyingall" runat="server" Checked="True" GroupName="A"
                                                    onclick="fnunderlying('a')" />All</td>
                                            <td>
                                                <asp:RadioButton ID="rdbunderlyingselected" runat="server" GroupName="A" onclick="fnunderlying('b')" />Selected</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC" id="Td_ConsolidateGrpBranch">
                                                <asp:CheckBox ID="ChkConsolidated" runat="server" />
                                                Consolidated Group/Branch
                                            </td>
                                            <td class="gridcellleft" bgcolor="#B7CEEC" id="Td_ConsolidatedAcrossSegment">
                                                <asp:CheckBox ID="ChkCOnsolidatedAcrossSegment" runat="server" />
                                                Show Group/Branch BreakUp</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Generate Type :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGeneration" runat="server" Width="210px" Font-Size="12px"
                                                    onchange="FnddlGeneration(this.value)">
                                                    <asp:ListItem Value="1">Screen</asp:ListItem>
                                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table>
                                        <tr>
                                            <td id="td_Screen">
                                                <asp:Button ID="btnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Screen"
                                                    Width="101px" OnClientClick="selecttion()" OnClick="btnScreen_Click" />
                                            </td>
                                            <td id="td_Export">
                                                <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                    Width="101px" OnClientClick="selecttion()" OnClick="btnExcel_Click" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table border="10" cellpadding="1" cellspacing="1" id="showFilter">
                            <tr>
                                <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                    id="TdFilter">
                                    <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                    <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                        Enabled="false">
                                        <asp:ListItem>Clients</asp:ListItem>
                                        <asp:ListItem>Broker</asp:ListItem>
                                        <asp:ListItem>Branch</asp:ListItem>
                                        <asp:ListItem>Group</asp:ListItem>
                                        <asp:ListItem>Company</asp:ListItem>
                                        <asp:ListItem>BranchGroup</asp:ListItem>
                                        <asp:ListItem>Product</asp:ListItem>
                                        <asp:ListItem>ScripsExchange</asp:ListItem>
                                    </asp:DropDownList>
                                    <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                        style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                            style="color: #009900; font-size: 8pt;"> </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="290px">
                                    </asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                    text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
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
            <table>
                <tr>
                    <td style="display: none;">
                        <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                        <asp:Button ID="btnhide" runat="server" Text="btnhide" OnClick="btnhide_Click" />
                        <asp:HiddenField ID="HiddenField_Group" runat="server" />
                        <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                        <asp:HiddenField ID="HiddenField_Client" runat="server" />
                        <asp:HiddenField ID="HiddenField_Broker" runat="server" />
                        <asp:HiddenField ID="HiddenField_Company" runat="server" />
                        <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                        <asp:HiddenField ID="hiddencount" runat="server" />
                        <asp:HiddenField ID="HiddenField_ScripsExchange" runat="server" />
                        <asp:HiddenField ID="HiddenField_Product" runat="server" />
                    </td>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                    top: 50%; background-color: white; layer-background-color: white; height: 80;
                                    width: 150;'>
                                    <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td height='25' align='center' bgcolor='#FFFFFF'>
                                                            <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                        <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                            <font size='1' face='Tahoma'><strong align='center'>Please Wait..</strong></font></td>
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
            <div id="displayAll" style="display: none;" width="100%">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <table width="100%" border="1">
                            <tr>
                                <td>
                                    <div id="DivHeader" runat="server">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="Divdisplay" runat="server">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
</asp:Content>
