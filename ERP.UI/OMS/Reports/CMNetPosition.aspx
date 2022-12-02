<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_CMNetPosition" CodeBehind="CMNetPosition.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        function Page_Load()///Call Into Page Load
        {

            Hide('showFilter');
            Hide('tr_filter');
            document.getElementById('hiddencount').value = 0;
            fn_ddldateChange('0');
            fnddlGeneration('3');
            Hide('td_lstfiltercolumn');
            dependgeneratetype('1');
            Hide('showFilter');
            height();

        }
        function dependgeneratetype(obj) {

            if (obj == '1') {
                Hide('tr_otherselection');
                Hide('tr_checkselection');
                Hide('tr_Securitytype');
                Hide('tr_filtercolumns');
                Hide('tr_generatetypeselection');
            }
            else {
                Show('tr_otherselection');
                Show('tr_checkselection');
                Show('tr_Securitytype');
                Show('tr_filtercolumns');
                Show('tr_generatetypeselection');
            }

        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollWidth;
            //document.getElementById('hidScreenwd').value=screen.width-25;

        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function fn_ddldateChange(obj) {
            if (obj == '0') {
                Show('td_dtfor');
                Hide('td_dtfrom');
                Hide('td_dtto');
                Show('tr_charges');
            }
            else {
                Hide('td_dtfor');
                Show('td_dtfrom');
                Show('td_dtto');
                Hide('tr_charges');
            }
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
        }

        function fnddlGeneration(obj) {
            if (obj == '0') {
                dependgeneratetype('2');
                Show('td_show');
                Hide('tr_excel');
                Show('tr_filtercolumns');
                Hide('td_mail');
                Hide('tr_mail');
                rptviewselection(document.getElementById('ddlrptview').value, 'screen');

            }
            if (obj == '1') {
                dependgeneratetype('2');
                Hide('td_show');
                Hide('tr_excel');
                Show('tr_filtercolumns');
                Show('td_mail');
                Show('tr_mail');
                Hide('showFilter');
                rptviewselection(document.getElementById('ddlrptview').value, 'other');

            }
            if (obj == '2') {
                dependgeneratetype('2');
                Hide('td_show');
                Show('tr_excel');
                Show('tr_filtercolumns');
                Hide('td_mail');
                Hide('tr_mail');
                Hide('showFilter');
                rptviewselection(document.getElementById('ddlrptview').value, 'other');
            }
            if (obj == '3') {
                dependgeneratetype('1');
                Hide('showFilter');
            }
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }

        }
        function rptviewselection(obj, obj1) {
            if (obj == '1') {
                document.getElementById('rdbranchclientSelected').checked = false;
                document.getElementById('rdbranchclientAll').checked = true;
                document.getElementById('rdbranchclientAll').disabled = false;
            }
            else {
                ifscreenthengroupby(obj1);
            }
        }
        function ifscreenthengroupby(obj) {
            if (obj == 'screen') {
                if (document.getElementById('ddlGroup').value == "0" || document.getElementById('ddlGroup').value == "2")//////////////Group By  selected are branch
                {
                    document.getElementById('rdbranchclientSelected').checked = true;
                    document.getElementById('rdbranchclientAll').checked = false;
                    document.getElementById('rdbranchclientAll').disabled = true;
                    fn_Branch();
                }
                else if (document.getElementById('ddlGroup').value == "1")///////group wise
                {
                    document.getElementById('rdddlgrouptypeSelected').checked = true;
                    document.getElementById('rdddlgrouptypeAll').checked = false;
                    document.getElementById('rdddlgrouptypeAll').disabled = true;
                    fn_Group();
                }
                else if (document.getElementById('ddlGroup').value == "3")///////client wise
                {
                    document.getElementById('rdbranchclientSelected').checked = true;
                    document.getElementById('rdbranchclientAll').checked = false;
                    document.getElementById('rdbranchclientAll').disabled = true;
                    fn_Clients();
                }
            }
            else {
                if (document.getElementById('ddlGroup').value == "0" || document.getElementById('ddlGroup').value == "2")//////////////Group By  selected are branch
                {
                    document.getElementById('rdbranchclientSelected').checked = false;
                    document.getElementById('rdbranchclientAll').checked = true;
                    document.getElementById('rdbranchclientAll').disabled = false;
                }
                else if (document.getElementById('ddlGroup').value == "1")///////group wise
                {
                    document.getElementById('rdddlgrouptypeSelected').checked = false;
                    document.getElementById('rdddlgrouptypeAll').checked = true;
                    document.getElementById('rdddlgrouptypeAll').disabled = false;
                }
                else if (document.getElementById('ddlGroup').value == "3")///////client wise
                {
                    document.getElementById('rdbranchclientSelected').checked = false;
                    document.getElementById('rdbranchclientAll').checked = true;
                    document.getElementById('rdbranchclientAll').disabled = false;
                }
            }
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
            else {
                cmbVal = document.getElementById('cmbsearchOption').value;
                cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
            }

            ajax_showOptions(objID, objListFun, objEvent, cmbVal);

        }
        function fn_Clients(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Clients';
                Show('showFilter');
            }
            selecttion();

        }
        function fn_Branch(obj) {
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
        function fn_Group(obj) {
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
            document.getElementById('btnshow').disabled = false;
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
        function fnddlGroup(obj) {
            if (obj == "0" || obj == "2" || obj == "3" || obj == "4") {
                if (obj == "3")
                    fn_Clients();
                if (obj == "4")
                    fnbroker();
                else
                    fn_Branch();
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
            height();
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
            height();
        }
        function fn_Scrip(obj) {
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
        function fn_Settlementtype(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'SettlementType';
                Show('showFilter');
            }
            selecttion();
            height();
        }
        function NORECORD(obj) {
            document.getElementById('ddlGeneration').value = "3";
            document.getElementById('chkfiltercolumns').checked = 'false';
            Hide('td_lstfiltercolumn');
            Hide('displayAll');
            Show('tab2');
            Hide('showFilter');
            fn_ddldateChange(document.getElementById('ddldate').value);
            fnddlGeneration(document.getElementById('ddlGeneration').value);
            fn_ddlrptchange(document.getElementById('ddlrptview').value);

            if (obj == '1')
                alert('No Record Found !! ');
            if (obj == '2')
                alert("Mail Sent Successfully !!");
            if (obj == '3')
                alert("Error on sending!Try again.. !!");
            if (obj == '4')
                alert("'Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...'");
            if (obj == '5')
                alert("E-Mail Id Could Not Be Found !!");
            if (obj == '7')
                alert("Please Select A Generate Type !!");

            if (obj == '8') {
                if (document.getElementById('ddlrptview').value == '0')
                    alert('You Have To Select Atleast One Client !!');
                if (document.getElementById('ddlrptview').value == '1')
                    alert('You Have To Select Atleast One Scrip !!');
            }
            document.getElementById('hiddencount').value = 0;
            Hide('showFilter');
            height();
        }
        function Display(obj) {

            Show('displayAll');
            Hide('tab2');
            Hide('showFilter');
            Show('tr_filter');
            document.getElementById('hiddencount').value = 0;
            if (obj == '1')
                Show('tr_prvnxt');
            if (obj == '2')
                Hide('tr_prvnxt');

            selecttion();
            height();
        }
        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = 'Ex';
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

        function fn_ddlrptchange(obj) {

            if (obj == '0') {
                Show('tr_grpselection');
                Show('tr_setttype');
                Show('tr_charges');
                Hide('tr_Securitytype');
            }
            else {
                Hide('tr_grpselection');
                Hide('tr_setttype');
                Hide('tr_charges');
                Hide('showFilter');
                Show('tr_Securitytype');

            }
            var ddlgentype = document.getElementById('ddlGeneration');
            ddlgentype.value = '3';
            dependgeneratetype('1');
        }
        function fnddloptionformail(obj) {
            if (obj == '2') {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'MAILEMPLOYEE';
                Show('showFilter');
            }
            else
                Hide('showFilter');
        }
        function fncolumnsdisplay(obj) {

            if (obj.checked == true) {
                Show('td_lstfiltercolumn');
            }
            else {
                Hide('td_lstfiltercolumn');
            }
            height();
        }

        FieldName = 'lstSlection';
    </script>
    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {

            var j = rValue.split('~');
            var btn = document.getElementById('btnhide');

            if (j[0] == 'Branch') {
                document.getElementById('HiddenField_Branch').value = j[1];
            }

            if (j[0] == 'Broker') {
                document.getElementById('HiddenField_Broker').value = j[1];
            }
            if (j[0] == 'Group') {
                document.getElementById('HiddenField_Group').value = j[1];
                btn.click();
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
            if (j[0] == 'SettlementType') {
                document.getElementById('HiddenField_SettlementType').value = j[1];
            }
            if (j[0] == 'MAILEMPLOYEE') {
                document.getElementById('HiddenField_emmail').value = j[1];
            }
        }
    </script>
    <script type="text/javascript">
        function SelectAllCheckboxes(chk) {
            $('#<%=chktfilter.ClientID %>').find("input:checkbox").each(function () {
         if (this != chk) {
             this.checked = chk.checked;
         }
     });
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
        <div>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                        <strong><span id="SpanHeader" style="color: #000099">Net Position </span></strong></td>
                    <td class="EHEADER" id="tr_filter" style="height: 22px">
                        <a href="javascript:void(0);" onclick="NORECORD(6);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>

                    </td>
                </tr>
            </table>

            <table id="tab2" border="0" cellpadding="0" cellspacing="0">
                <tr valign="top">
                    <td>
                        <table>
                            <tr>
                                <td class="gridcellleft">
                                    <table cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr valign="top">
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="ddldate" runat="server" Width="100px" Font-Size="12px" onchange="fn_ddldateChange(this.value)">
                                                    <asp:ListItem Value="0">For A Date</asp:ListItem>
                                                    <asp:ListItem Value="1">For A Period</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td id="td_dtfor" class="gridcellleft">
                                                <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfor">
                                                    <DropDownButton Text="For">
                                                    </DropDownButton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td id="td_dtfrom" class="gridcellleft">
                                                <dxe:ASPxDateEdit ID="dtfrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfrom">
                                                    <DropDownButton Text="From">
                                                    </DropDownButton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td id="td_dtto" class="gridcellleft">
                                                <dxe:ASPxDateEdit ID="dtto" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="dtto">
                                                    <DropDownButton Text="To">
                                                    </DropDownButton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">Report View :</td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="ddlrptview" runat="server" Width="120px" Font-Size="12px" onchange="fn_ddlrptchange(this.value)">
                                                    <asp:ListItem Value="0">Client Wise</asp:ListItem>
                                                    <asp:ListItem Value="1">Share Wise</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGeneration" runat="server" Width="130px" Font-Size="12px"
                                                    onchange="fnddlGeneration(this.value)">
                                                    <asp:ListItem Value="3">Select Type</asp:ListItem>
                                                    <asp:ListItem Value="0">Screen</asp:ListItem>
                                                    <asp:ListItem Value="1">Send Mail</asp:ListItem>
                                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <table>
                                        <tr id="tr_otherselection" valign="top">
                                            <td>
                                                <table border="10" cellpadding="1" cellspacing="1">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr id="tr_grpselection">
                                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Group By :</td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                                            <asp:ListItem Value="0">Branch</asp:ListItem>
                                                                            <asp:ListItem Value="1">Group</asp:ListItem>
                                                                            <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                                                            <asp:ListItem Value="3">Clients</asp:ListItem>
                                                                            <asp:ListItem Value="4">Broker</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td id="td_branch">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:RadioButton ID="rdbranchclientAll" runat="server" Checked="True" GroupName="a"
                                                                                        onclick="fn_Branch('a')" />
                                                                                    All
                                                                                </td>
                                                                                <td>
                                                                                    <asp:RadioButton ID="rdbranchclientSelected" runat="server" GroupName="a" onclick="fn_Branch('b')" />Selected
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td id="td_group" style="display: none;">
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
                                                                                        onclick="fn_Group('a')" />
                                                                                    All
                                                                                <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="fn_Group('b')" />Selected
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Scrip :</td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbScripAll" runat="server" Checked="True" GroupName="c" onclick="fn_Scrip('a')" />
                                                                        All
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbScripSelected" runat="server" GroupName="c" onclick="fn_Scrip('b')" />
                                                                        Selected
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_setttype">
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Settlement Type :</td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdSetttypeAll" runat="server" Checked="True" GroupName="cb"
                                                                            onclick="fn_Settlementtype('a')" />
                                                                        All
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdSetttypeSelected" runat="server" GroupName="cb" onclick="fn_Settlementtype('b')" />
                                                                        Selected
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            <asp:CheckBox ID="chkopen" runat="server" onclick="selecttion()" />
                                                            Show Only Open Position
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_charges">
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            <asp:CheckBox ID="ChkCalculateCharge" Checked="true" runat="server" onclick="selecttion()" />
                                                            Calculate Charges
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            <asp:CheckBox ID="Chksign" Checked="true" runat="server" onclick="selecttion()" />
                                                            Show Open Buy Position in +ve Sign
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td class="gridcellleft">
                                                <table id="showFilter" cellpadding="1" cellspacing="1">
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
                                                                <asp:ListItem>ScripsExchange</asp:ListItem>
                                                                <asp:ListItem>BranchGroup</asp:ListItem>
                                                                <asp:ListItem>SettlementType</asp:ListItem>
                                                                <asp:ListItem>MAILEMPLOYEE</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                                    style="color: #009900; font-size: 8pt;"> </span>
                                                        </td>
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
                            <tr id="tr_checkselection">
                                <td class="gridcellleft">
                                    <table cellpadding="1" cellspacing="1" class="tableClass">

                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">Consider Clients With Net Obligation >=
                                            </td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtAmntGreaterThan" runat="server" HorizontalAlign="Right" Width="100px">
                                                    <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                    <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr_Securitytype">
                                <td class="gridcellleft">
                                    <table cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">Security Type :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlSecurityType" runat="server" Font-Size="11px" Width="150px">
                                                    <asp:ListItem Value="ALL">ALL</asp:ListItem>
                                                    <asp:ListItem Value="Approved">Only Approved</asp:ListItem>
                                                    <asp:ListItem Value="UnApproved">Only UnApproved</asp:ListItem>
                                                    <asp:ListItem Value="Illiquid">Only Illiquid</asp:ListItem>
                                                    <asp:ListItem Value="liquid">Only liquid</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr_filtercolumns">
                                <td>
                                    <table>
                                        <tr valign="top">
                                            <td id="td_btnfiltercolumn" class="gridcellleft" bgcolor="#B7CEEC">
                                                <asp:CheckBox ID="chkfiltercolumns" runat="server" onclick="fncolumnsdisplay(this)" />
                                                Filter Columns
                                            </td>
                                            <td id="td_lstfiltercolumn">
                                                <table>
                                                    <tr>
                                                        <td class="gridcellleft">
                                                            <asp:CheckBox ID="chkAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);"
                                                                Checked="true" /><span style="font-family: Verdana; color: Teal; font-size: x-small;"><b>Check/UnCheck ALL</b></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="overflow: auto; border: 1px black solid; scrollbar-base-color: #C0C0C0">
                                                                <asp:CheckBoxList ID="chktfilter" runat="server" RepeatDirection="Horizontal" Width="800px"
                                                                    RepeatColumns="8">
                                                                    <asp:ListItem Value="Buy Qty" Selected="True">Buy Qty</asp:ListItem>
                                                                    <asp:ListItem Value="Buy MktValue" Selected="True">Buy MktValue</asp:ListItem>
                                                                    <asp:ListItem Value="BuyAvg Mkt" Selected="True">BuyAvg Mkt</asp:ListItem>
                                                                    <asp:ListItem Value="Buy NetValue" Selected="True">Buy NetValue</asp:ListItem>
                                                                    <asp:ListItem Value="BuyAvg Net" Selected="True">BuyAvg Net</asp:ListItem>
                                                                    <asp:ListItem Value="Sell Qty" Selected="True">Sell Qty</asp:ListItem>
                                                                    <asp:ListItem Value="Sell MktValue" Selected="True">Sell MktValue</asp:ListItem>
                                                                    <asp:ListItem Value="SellAvg Mkt" Selected="True">SellAvg Mkt</asp:ListItem>
                                                                    <asp:ListItem Value="Sell NetValue" Selected="True">Sell NetValue</asp:ListItem>
                                                                    <asp:ListItem Value="SellAvg Net" Selected="True">SellAvg Net</asp:ListItem>
                                                                    <asp:ListItem Value="Diff Qty" Selected="True">Diff Qty</asp:ListItem>
                                                                    <asp:ListItem Value="Diff P/L" Selected="True">Diff P/L</asp:ListItem>
                                                                    <asp:ListItem Value="Dlv Qty" Selected="True">Dlv Qty</asp:ListItem>
                                                                    <asp:ListItem Value="Dlv Value" Selected="True">Dlv Value</asp:ListItem>
                                                                    <asp:ListItem Value="Avg Dlv" Selected="True">Avg Dlv</asp:ListItem>
                                                                    <asp:ListItem Value="Net Amnt" Selected="True">Net Amnt</asp:ListItem>
                                                                    <asp:ListItem Value="STT" Selected="True">STT</asp:ListItem>
                                                                    <asp:ListItem Value="Close Price" Selected="True">Close Price</asp:ListItem>
                                                                    <asp:ListItem Value="MTM" Selected="True">MTM</asp:ListItem>
                                                                </asp:CheckBoxList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr_generatetypeselection">
                                <td class="gridcellleft">
                                    <table cellpadding="1" cellspacing="1" class="tableClass">

                                        <tr id="tr_mail">
                                            <td class="gridcellleft" bgcolor="#B7CEEC">Respective :</td>
                                            <td>
                                                <asp:DropDownList ID="ddloptionformail" runat="server" Width="100px" Font-Size="12px"
                                                    onchange="fnddloptionformail(this.value)">
                                                    <asp:ListItem Value="0">Client</asp:ListItem>
                                                    <asp:ListItem Value="1">Group/Branch</asp:ListItem>
                                                    <asp:ListItem Value="2">User</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr id="tr_excel">
                                            <td class="gridcellleft" bgcolor="#B7CEEC">File Type :</td>
                                            <td>
                                                <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px"
                                                    Width="100px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                                                    <asp:ListItem Value="E">Excel</asp:ListItem>
                                                    <asp:ListItem Value="P">PDF</asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td>
                                                <asp:CheckBox ID="chkrawprint" runat="server" /></td>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">Print Client Name and Code in Column
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="td_show">
                                                <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px"
                                                    Text="Show" Width="101px" OnClientClick="selecttion()" OnClick="btnshow_Click" /></td>
                                            <td id="td_mail">
                                                <asp:Button ID="btnmailsend" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Mail"
                                                    Width="101px" OnClientClick="selecttion()" OnClick="btnmailsend_Click" /></td>
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
                        <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                        <asp:HiddenField ID="HiddenField_Group" runat="server" />
                        <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                        <asp:HiddenField ID="HiddenField_Client" runat="server" />
                        <asp:HiddenField ID="HiddenField_Broker" runat="server" />
                        <asp:HiddenField ID="HiddenField_ScripsExchange" runat="server" />
                        <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                        <asp:HiddenField ID="HiddenField_SettlementType" runat="server" />
                        <asp:HiddenField ID="HiddenField_emmail" runat="server" />
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

        </div>
        <div id="displayAll" style="display: none; width: 99%">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="99%" border="1">
                        <tr style="display: none;">
                            <td>
                                <asp:DropDownList ID="cmbclient" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                                </asp:DropDownList><asp:HiddenField ID="hiddencount" runat="server" />
                            </td>
                        </tr>
                        <tr id="tr_DIVdisplayPERIOD">
                            <td>
                                <div id="DIVdisplayPERIOD" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr id="tr_group">
                            <td>
                                <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" runat="server">
                                            <tr valign="top">
                                                <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Prev" Text="[Prev]" OnCommand="NavigationLinkC_Click"
                                                        OnClientClick="javascript:selecttion();"> </asp:LinkButton>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:DropDownList ID="cmbgroup" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cmbgroup_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkNext" runat="server" CommandName="Next" Text="[Next]" OnCommand="NavigationLinkC_Click"
                                                        OnClientClick="javascript:selecttion();"> </asp:LinkButton>&nbsp;&nbsp;
                                                </td>

                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr bordercolor="Blue" id="tr_prvnxt">
                            <td align="left">
                                <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="800px">
                                    <tr>
                                        <td style="padding: 2px; width: 2%">
                                            <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                        <td style="padding: 2px; width: 2%">
                                            <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                        <td style="padding: 2px; width: 2%">
                                            <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                        <td style="padding: 2px; width: 2%">
                                            <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();"
                                                OnClick="ASPxLast_Click">Last</asp:LinkButton></td>
                                        <td style="padding: 2px; width: 80%" align="right">
                                            <asp:Label ID="listRecord" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td style="padding: 5px;"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="display" runat="server">
                                </div>
                            </td>
                        </tr>
                        <asp:HiddenField ID="TotalGrp" runat="server" />
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
            <asp:HiddenField ID="hidScreenwd" runat="Server" />
        </div>
    </div>
</asp:Content>
