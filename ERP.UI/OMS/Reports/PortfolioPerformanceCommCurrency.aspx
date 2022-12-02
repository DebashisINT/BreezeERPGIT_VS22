<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_PortfolioPerformanceCommCurrency" CodeBehind="PortfolioPerformanceCommCurrency.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

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
                    Hide('td_dtfrom');
                    Hide('td_dtto');
                    Hide('showFilter');
                    Show('td_btnshow');
                    Hide('td_btnprint');
                    Hide('td_opentoexcel');
                    Hide('tr_filter');
                    Hide('td_sendmail');
                    Hide('tr_mail');
                    document.getElementById('hiddencount').value = 0;
                    height();
                }
                function height() {
                    if (document.body.scrollHeight >= 500) {
                        window.frameElement.height = document.body.scrollHeight;
                    }
                    else {
                        window.frameElement.height = '500px';
                    }
                    window.frameElement.width = document.body.scrollwidth;
                }
                function DateChange() {
                    cdtExpiry.SetDate(cdtfor.GetDate());
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
                        Show('td_Expiry');
                        Show('td_dtExpiry');
                        Hide('td_dtto');
                        Hide('td_chknfqty');
                        Hide('TR_ChargeInterest');
                        document.getElementById('chkCharge').checked = false;
                        document.getElementById('chkInterest').checked = false;

                    }
                    else {
                        Hide('td_dtfor');
                        Hide('td_Expiry');
                        Hide('td_dtExpiry');
                        Show('td_dtfrom');
                        Show('td_dtto');
                        Show('td_chknfqty');
                        Show('TR_ChargeInterest');

                    }
                    height();
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

                    ajax_showOptions(objID, 'ShowClientFORMarginStocks', objEvent, cmbVal);
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
                    height();
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
                    height();
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

                function RBShowHide(obj) {
                    if (obj == 'rbPrint') {
                        Hide('td_btnshow');
                        Show('td_btnprint');
                        Show('td_ChkDISTRIBUTION');
                        Hide('td_opentoexcel');
                        Hide('td_sendmail');
                        Hide('tr_mail');
                    }
                    if (obj == 'rbScreen') {
                        Show('td_btnshow');
                        Hide('td_btnprint');
                        Hide('td_ChkDISTRIBUTION');
                        Hide('td_opentoexcel');
                        Hide('td_sendmail');
                        Hide('tr_mail');
                    }
                    if (obj == 'rbExcel') {
                        Hide('td_btnshow');
                        Hide('td_btnprint');
                        Hide('td_ChkDISTRIBUTION');
                        Show('td_opentoexcel');
                        Hide('td_sendmail');
                        Hide('tr_mail');
                    }
                    if (obj == 'rbmail') {
                        Hide('td_btnshow');
                        Hide('td_btnprint');
                        Hide('td_ChkDISTRIBUTION');
                        Hide('td_opentoexcel');
                        Show('td_sendmail');
                        Show('tr_mail');

                        // if obj = rbmail { 
                    }

                    height();
                    selecttion();
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

                function fnExpiry(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'Expiry';
                        Show('showFilter');
                    }
                    selecttion();
                    height();

                }
                function fn_Terminal(obj) {
                    if (obj == "a")
                        Hide('td_terminaltxt');
                    else
                        Show('td_terminaltxt');
                    selecttion();
                    height();
                }
                function NORECORD(obj) {
                    Hide('tr_filter');
                    Hide('displayAll');
                    Show('tab2');
                    Hide('showFilter');
                    if (obj == '1')
                        alert('No Record Found!!');
                    else if (obj == '2')
                        alert('Please Select Atleast One Instrument Type!!');
                    document.getElementById('hiddencount').value = 0;
                    height();
                }
                function Display() {
                    Show('tr_filter');
                    Show('displayAll');
                    Hide('tab2');
                    Hide('showFilter');
                    document.getElementById('hiddencount').value = 0;

                    if (document.getElementById('ddlmtmcalbasis').value == '0' && document.getElementById('ddlrpttype').value == '0') {
                        document.getElementById('display').className = "grid_scroll";
                    }
                    if (document.getElementById('ddlrpttype').value == '0') {
                        document.getElementById('tr_prvnxt').style.display = 'inline';
                    }
                    if (document.getElementById('ddlrpttype').value == '1') {
                        document.getElementById('tr_prvnxt').style.display = 'none';
                    }
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
                function Filter() {
                    Hide('tr_filter');
                    Hide('displayAll');
                    Show('tab2');
                    Hide('showFilter');
                    selecttion();
                    height();
                }
                function fnexpirtycallajax(objID, objListFun, objEvent) {
                    var date;
                    if (document.getElementById('ddldate').value == '0') {
                        date = new Date(dtfor.GetDate());
                        date = parseInt(date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                    }
                    else {
                        date = new Date(dtfrom.GetDate());
                        date = parseInt(date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                    }
                    ajax_showOptions(objID, 'Searchproductandeffectuntil', objEvent, 'Expiry' + '~' + date);
                }
                function fnTerminalcallajax(objID, objListFun, objEvent) {
                    var datefrom;
                    var dateto;
                    var date;

                    datefrom = new Date(dtfrom.GetDate());
                    dateto = new Date(dtto.GetDate());

                    datefrom = parseInt(datefrom.getMonth() + 1) + '-' + datefrom.getDate() + '-' + datefrom.getFullYear();
                    dateto = parseInt(dateto.getMonth() + 1) + '-' + dateto.getDate() + '-' + dateto.getFullYear();

                    date = "'" + datefrom + "' and '" + dateto + "'";
                    ajax_showOptions(objID, 'ShowClientFORMarginStocks', objEvent, 'TerminalIdDate' + '~' + date);
                }

                function ajaxcall(objID, objListFun, objEvent) {

                    if (document.getElementById('cmbsearchOption').value == "Expiry") {

                        fnexpirtycallajax(objID, objListFun, objEvent);
                    }
                    else if (document.getElementById('cmbsearchOption').value == "Product") {

                        ajax_showOptions(objID, 'Searchproductandeffectuntil', objEvent, 'Product');
                    }
                    else {
                        FunClientScrip(objID, objListFun, objEvent);
                    }
                }
                function fnrpttype(obj) {
                    if (obj == "0") {
                        Hide('tr_shownet');
                        Show('td_ChkDISTRIBUTION');
                        Show('TR_SIGN');
                        Show('TR_OPENPOSITION');
                    }
                    else {
                        Show('tr_shownet');
                        Hide('td_ChkDISTRIBUTION');
                        Hide('TR_SIGN');
                        Hide('TR_OPENPOSITION');
                        Hide('td_email');
                    }

                    height();
                }
                function fn_ignore() {
                    if (document.getElementById('ChkBFQty').checked == true) {
                        Show('tr_terminal');
                        Hide('td_bfpositionvalue');
                    }
                    else {
                        Hide('tr_terminal');
                        Show('td_bfpositionvalue');
                    }
                    selecttion();
                    height();
                }

                function fnrptview(obj) {
                    if (obj == "0")
                        Show('td_print');
                    else
                        Hide('td_print');
                }
                function checkall() {
                    Show('Td_ConsolidatedAcroseeExchange');
                }
                function uncheckall() {
                    Hide('Td_ConsolidatedAcroseeExchange');
                }
                FieldName = 'lstSlection';
                </script>
    <script type="text/ecmascript">
      function ReceiveServerData(rValue) {

    var j = rValue.split('~');


    if (j[0] == 'Branch') {
        document.getElementById('HiddenField_Branch').value = j[1];
    }
    if (j[0] == 'Group') {
        document.getElementById('HiddenField_Group').value = j[1];
    }
    if (j[0] == 'Clients') {
        document.getElementById('HiddenField_Client').value = j[1];
    }
    if (j[0] == 'Product') {
        document.getElementById('HiddenField_Product').value = j[1];
    }
    if (j[0] == 'Expiry') {
        document.getElementById('HiddenField_Expiry').value = j[1];
    }
    if (j[0] == 'MAILEMPLOYEE') {
        document.getElementById('HiddenField_emmail').value = j[1];
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
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Portfolio Performance Report</span></strong></td>
                <td class="EHEADER" width="15%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="Filter();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <table id="tab2" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr valign="top">
                                        <td class="gridcellleft">
                                            <asp:DropDownList ID="ddldate" runat="server" Width="100px" Font-Size="12px" onchange="fn_ddldateChange(this.value)">
                                                <asp:ListItem Value="0">As on Date</asp:ListItem>
                                                <asp:ListItem Value="1">For a Period</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td id="td_dtfor" class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="cdtfor">
                                                <DropDownButton Text="For">
                                                </DropDownButton>
                                                <ClientSideEvents DateChanged="function(s,e){DateChange();}" />
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
                                        <td id="td_dtExpiry">Consider All Expiry Date>=                            
                                        </td>
                                        <td id="td_Expiry" class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtExpiry" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="128px" ClientInstanceName="cdtExpiry">
                                                <DropDownButton Text="ExpiryDate">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>

                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" colspan="3" style="display: none;" id="td_chknfqty">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="ChkBFQty" runat="server" onclick="fn_ignore()" />
                                                        Ignore Brought Forward Position</td>
                                                    <td id="td_bfpositionvalue">
                                                        <asp:CheckBox ID="ChkBFPositionValue" runat="server" />
                                                        Value B/F Position at Prev. Close</td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report View :</td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlrptview" runat="server" Font-Size="11px" Width="200px" Enabled="true"
                                                onchange="fnrptview(this.value)">
                                                <asp:ListItem Value="0">Branch/Group - Client Wise</asp:ListItem>
                                                <asp:ListItem Value="1">Asset - Instrument Wise</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report Type :</td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlrpttype" runat="server" Font-Size="11px" Width="200px" Enabled="true"
                                                onchange="fnrpttype(this.value)">
                                                <asp:ListItem Value="0">Detail</asp:ListItem>
                                                <asp:ListItem Value="1">Summary</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Group By</td>
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
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="Branch('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="Branch('b')" />Selected
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
                                                            onclick="Group('a')" />
                                                        All
                                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="Group('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Clients :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="Clients('a')" />
                                            All Client
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radPOAClient" runat="server" GroupName="c" onclick="Clients('a')" />POA
                                            Client
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="Clients('b')" />
                                            Selected Client
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_terminal" style="display: none;">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Terminal Id:</td>
                                        <td>
                                            <asp:RadioButton ID="rdbTerminalAll" runat="server" Checked="True" GroupName="ter"
                                                onclick="fn_Terminal('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbTerminalSpecific" runat="server" GroupName="ter" onclick="fn_Terminal('b')" />Specific
                                        </td>
                                        <td style="display: none;" id="td_terminaltxt">
                                            <asp:TextBox runat="server" Width="250px" Font-Size="12px" ID="txtTerminal" onkeyup="fnTerminalcallajax(this,'chkfn',event)"></asp:TextBox>
                                            <asp:TextBox ID="txtTerminal_hidden" runat="server" Width="14px" Style="display: none;"> </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" style="width: 100px;" bgcolor="#B7CEEC">Asset:</td>
                                        <td>
                                            <asp:RadioButton ID="rdbunderlyingall" runat="server" Checked="True" GroupName="d"
                                                onclick="fnunderlying('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbunderlyingselected" runat="server" GroupName="d" onclick="fnunderlying('b')" />Selected
                                            Asset
                                        </td>

                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" style="width: 100px;" bgcolor="#B7CEEC">Expiry :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbExpiryAll" runat="server" Checked="True" GroupName="e" onclick="fnExpiry('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbExpirySelected" runat="server" GroupName="e" onclick="fnExpiry('b')" />Selected
                                            Expiry
                                        </td>

                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" style="width: 100px;" bgcolor="#B7CEEC">Instrument Type :</td>
                                        <td>
                                            <asp:CheckBoxList ID="chkinstrutype" runat="server" RepeatDirection="Horizontal"
                                                Width="250px">
                                                <asp:ListItem Value="FUT" Selected="True">Future</asp:ListItem>
                                                <asp:ListItem Value="C" Selected="True">Call</asp:ListItem>
                                                <asp:ListItem Value="P" Selected="True">Put</asp:ListItem>
                                                <asp:ListItem Value="All" Selected="True" onclick="if(this.checked){checkall()}else{uncheckall()}">Across Exchange</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                        <td>
                                            <table>
                                                <tr valign="top">
                                                    <td class="gridcellleft" bgcolor="#B7CEEC" id="Td_ConsolidatedAcroseeExchange">
                                                        <asp:CheckBox ID="ChkConsolidatedExchange" runat="server" onclick="selecttion()" />
                                                        Consolidated Position Across Exchange</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" style="width: 170px;" bgcolor="#B7CEEC">MTM Calculation Basis :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlmtmcalbasis" runat="server" Font-Size="11px" Width="200px">
                                                <asp:ListItem Value="0">Asset & Instr Close</asp:ListItem>
                                                <asp:ListItem Value="1">Instrument Close</asp:ListItem>
                                                <asp:ListItem Value="2">Asset Close</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_shownet" style="display: none;">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Show Clients :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbnetclientboth" runat="server" Checked="True" GroupName="cc" />
                                            Both
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbnetclientrecivabel" runat="server" GroupName="cc" />
                                            Only Receivable
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbnetclientpayabel" runat="server" GroupName="cc" />
                                            Only Payable
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="TR_OPENPOSITION">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkopen" runat="server" onclick="selecttion()" />
                                            Show Only Open Position</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkclosepricezero" runat="server" onclick="selecttion()" />
                                            Do Not Consider Closing Prices</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="TR_SIGN">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkopenbfpositive" runat="server" onclick="selecttion()" Checked="True" />
                                            Show Open Buy Position in +ve Sign
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="TR_PREMIUM">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chknetpremium" runat="server" onclick="selecttion()" />
                                            Show Net Premium
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="TR_ChargeInterest" style="display: none">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkCharge" runat="server" />
                                            Show Charge
                                        </td>
                                        <td bgcolor="#b7ceec" class="gridcellleft">
                                            <asp:CheckBox ID="chkInterest" runat="server" />
                                            Show Interest</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <table>
                                                <tr>
                                                    <td class="gridcellleft" id="td_screen">
                                                        <asp:RadioButton ID="rbScreen" runat="server" GroupName="f" Checked="True" onclick="RBShowHide(this.value)" />Screen
                                                    </td>
                                                    <td class="gridcellleft" id="td_print">
                                                        <asp:RadioButton ID="rbPrint" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Print
                                                    </td>
                                                    <td class="gridcellleft" id="td_excel">
                                                        <asp:RadioButton ID="rbExcel" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Excel
                                                    </td>
                                                    <td class="gridcellleft" id="td_email">
                                                        <asp:RadioButton ID="rbmail" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Send Mail
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="display: none;" id="td_ChkDISTRIBUTION">
                                            <table>
                                                <tr>
                                                    <td id="tr_printlogo">
                                                        <asp:CheckBox ID="CHKLOGOPRINT" runat="server" Checked="true" />
                                                        Do Not Print Logo</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="ChkDISTRIBUTION" runat="server" />
                                                        Distribution Copy</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_mail">


                            <td>Respective :
                                            <asp:DropDownList ID="ddloptionformail" runat="server" Width="100px" Font-Size="12px"
                                                onchange="fnddloptionformail(this.value)">
                                                <asp:ListItem Value="0">Client</asp:ListItem>
                                                <asp:ListItem Value="1">Group/Branch</asp:ListItem>
                                                <asp:ListItem Value="2">User</asp:ListItem>
                                            </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td id="td_btnshow" class="gridcellleft">
                                            <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                Width="101px" OnClick="btnshow_Click" />
                                        </td>
                                        <td id="td_btnprint">
                                            <asp:Button ID="btnprint" runat="server" CssClass="btnUpdate" Height="20px" Text="Print"
                                                OnClientClick="selecttion()" Width="101px" OnClick="btnprint_Click" />
                                        </td>
                                        <td id="td_opentoexcel">
                                            <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Open To Excel"
                                                Width="101px" OnClick="btnExcel_Click" />
                                        </td>
                                        <td id="td_sendmail">
                                            <asp:Button ID="btnmail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Email"
                                                Width="101px" OnClick="btnmail_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table id="showFilter">
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                            id="TdFilter">
                                            <span id="spanunder"></span><span id="spanclient"></span>
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="ajaxcall(this,'chkfn',event)"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
                                                <asp:ListItem>Expiry</asp:ListItem>
                                                <asp:ListItem>Product</asp:ListItem>
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
        </table>
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Product" runat="server" />
                    <asp:HiddenField ID="HiddenField_Expiry" runat="server" />
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
        <div id="displayAll" style="display: none;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">
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
                                                        OnSelectedIndexChanged="cmbgroup_SelectedIndexChanged" onchange="selecttion()">
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
                            <td align="center">
                                <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="100%">
                                    <tr>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();;" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                        <td width="25">
                                            <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                        <td width="20">
                                            <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();"
                                                OnClick="ASPxLast_Click">Last</asp:LinkButton></td>
                                        <td align="right">
                                            <asp:Label ID="listRecord" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
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
        </div>
    </div>
</asp:Content>
