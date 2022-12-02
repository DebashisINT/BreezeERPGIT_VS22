<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_NetPositionUnprocessed" CodeBehind="NetPositionUnprocessed.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

    <style type="text/css">
        .tableClass {
            /* border: 0px; */
            border: 1px solid #aaa !important;
            border-collapse: collapse !important;
        }

        .tableBorderClass {
            /* border: 0px; */
            border: 1px solid #aaa !important;
        }
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


        function Page_Load()///Call Into Page Load
        {
            Hide('showFilter');
            Hide('td_filter');
            Hide('Tr_Broker');
            document.getElementById('hiddencount').value = 0;
            FnddlGeneration('1');
            height();
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
            else if (document.getElementById('cmbsearchOption').value == "UserID") {
                var exchangesegmnet = "<%=Session["ExchangeSegmentID"]%>";

             cmbVal = document.getElementById('cmbsearchOption').value;
             var date2 = null;
             var date1 = DtFrom.GetDate();
             if (date1 != null) {
                 date2 = parseInt(date1.getMonth() + 1) + '-' + date1.getDate() + '-' + date1.getFullYear();
             }
             var date3 = null;
             var date1 = DtTo.GetDate();
             if (date1 != null) {
                 date3 = parseInt(date1.getMonth() + 1) + '-' + date1.getDate() + '-' + date1.getFullYear();
             }
             var criteritype = 'B';
             if (exchangesegmnet == "1" || exchangesegmnet == "2" || exchangesegmnet == "4" || exchangesegmnet == "5" || exchangesegmnet == "15") {
                 criteritype = '  ExchangeTrades_TRADEDATE Between  "' + date2 + '"  and  "' + date3 + '" ';
             }
             else {
                 criteritype = '  ComExchangeTrades_TRADEDATE Between  "' + date2 + '"  and  "' + date3 + '"  ';
             }
             criteritype = criteritype.replace('"', "'");
             criteritype = criteritype.replace('"', "'");
             criteritype = criteritype.replace('"', "'");
             criteritype = criteritype.replace('"', "'");
             cmbVal = cmbVal + '~' + criteritype;
         }
         else if (document.getElementById('cmbsearchOption').value == 'ScripsExchange') {
             cmbVal = document.getElementById('cmbsearchOption').value;
             var exchangesegmnet = "<%=Session["ExchangeSegmentID"]%>";
                var criteritype = 'B';
                var date3 = null;
                var date1 = DtTo.GetDate();
                if (date1 != null) {
                    date3 = parseInt(date1.getMonth() + 1) + '-' + date1.getDate() + '-' + date1.getFullYear();
                }
                if (exchangesegmnet == "2" || exchangesegmnet == "5") {
                    criteritype = ' AND Equity_EffectUntil>="' + date3 + '"  ';
                    criteritype = criteritype.replace('"', "'");
                    criteritype = criteritype.replace('"', "'");
                    cmbVal = cmbVal + '~' + 'Date' + '~' + criteritype;
                }
                else if (exchangesegmnet == "1" || exchangesegmnet == "4" || exchangesegmnet == "15") {
                    cmbVal = cmbVal + '~' + 'NoDate';
                }

                else {
                    criteritype = ' AND Commodity_ExpiryDate>="' + date3 + '"  ';
                    criteritype = criteritype.replace('"', "'");
                    criteritype = criteritype.replace('"', "'");
                    cmbVal = cmbVal + '~' + 'Date' + '~' + criteritype;

                }

            }
            else if (document.getElementById('cmbsearchOption').value == "TradeCode") {
                var exchangesegmnet = "<%=Session["ExchangeSegmentID"]%>";

		         cmbVal = document.getElementById('cmbsearchOption').value;
		         var date2 = null;
		         var date1 = DtFrom.GetDate();
		         if (date1 != null) {
		             date2 = parseInt(date1.getMonth() + 1) + '-' + date1.getDate() + '-' + date1.getFullYear();
		         }
		         var date3 = null;
		         var date1 = DtTo.GetDate();
		         if (date1 != null) {
		             date3 = parseInt(date1.getMonth() + 1) + '-' + date1.getDate() + '-' + date1.getFullYear();
		         }
		         var criteritype = 'B';
		         if (exchangesegmnet == "1" || exchangesegmnet == "2" || exchangesegmnet == "4" || exchangesegmnet == "5" || exchangesegmnet == "15") {
		             criteritype = '  and ExchangeTrades_TradeDate Between  "' + date2 + '"  and  "' + date3 + '" ';
		         }
		         else {
		             criteritype = ' and ComExchangeTrades_TradeDate Between  "' + date2 + '"  and  "' + date3 + '"  ';
		         }
		         criteritype = criteritype.replace('"', "'");
		         criteritype = criteritype.replace('"', "'");
		         criteritype = criteritype.replace('"', "'");
		         criteritype = criteritype.replace('"', "'");
		         cmbVal = cmbVal + '~' + criteritype;
		     }
		     else {
		         cmbVal = document.getElementById('cmbsearchOption').value;
		         cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
		     }

    ajax_showOptions(objID, objListFun, objEvent, cmbVal);

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

function fnClients(obj) {
    if (obj == "a")
        Hide('showFilter');
    else {
        var cmb = document.getElementById('cmbsearchOption');
        cmb.value = 'Clients';
        Show('showFilter');
    }

}
function fnInstrument(obj) {
    if (obj == "a")
        Hide('showFilter');
    else {
        var cmb = document.getElementById('cmbsearchOption');
        cmb.value = 'ScripsExchange';
        Show('showFilter');
    }

}

function fnTerminalID(obj) {
    if (obj == "a")
        Hide('showFilter');
    else {
        var cmb = document.getElementById('cmbsearchOption');
        cmb.value = 'UserID';
        Show('showFilter');
    }

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

}
function fnGroup(obj) {
    if (obj == "a")
        Hide('showFilter');
    else {
        var cmb = document.getElementById('cmbsearchOption');
        cmb.value = 'Group';
        Show('showFilter');
    }

}
function fnTradeCode(obj) {
    if (obj == "a")
        Hide('showFilter');
    else {
        var cmb = document.getElementById('cmbsearchOption');
        cmb.value = 'TradeCode';
        Show('showFilter');
    }

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

}
function fnddlview(obj) {
    if (obj == "1") {
        Show('Tr_Clients');
        Hide('Tr_Broker');
    }
    else {
        Hide('Tr_Clients');
        Show('Tr_Broker');

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

}

function RecordDisplay() {
    Hide('showFilter');
    Show('td_filter');
    Hide('tab1');
    Show('displayAll');
    document.getElementById('hiddencount').value = 0;

    height();

}
function fnNoRecord(obj) {
    Hide('showFilter');
    Hide('td_filter');
    Show('tab1');
    Hide('displayAll');
    if (obj == '1')
        alert('No Record Found!!');
    if (obj == '2')
        alert("Mail Sent Successfully !!");
    if (obj == '3')
        alert("Error on sending!Try again.. !!");

    document.getElementById('hiddencount').value = 0;

    height();

}

function FnddlGeneration(obj) {
    if (obj == "1") {
        Show('td_Screen');
        Hide('td_Export');
        Hide('td_Mail');
        Hide('tr_MailSendOption');
    }
    if (obj == "2") {
        Hide('td_Screen');
        Show('td_Export');
        Hide('td_Mail');
        Hide('tr_MailSendOption');
    }
    if (obj == "3") {
        Hide('td_Screen');
        Hide('td_Export');
        Show('td_Mail');
        Show('tr_MailSendOption');
        var cmb = document.getElementById('cmbsearchOption');
        cmb.value = 'MAILEMPLOYEE';
        Show('showFilter');

    }
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
            if (j[0] == 'ScripsExchange') {
                document.getElementById('HiddenField_Scrips').value = j[1];
            }
            if (j[0] == 'UserID') {
                document.getElementById('HiddenField_TerminalID').value = j[1];
            }
            if (j[0] == 'BranchGroup') {
                document.getElementById('HiddenField_BranchGroup').value = j[1];
            }
            if (j[0] == 'MAILEMPLOYEE') {
                document.getElementById('HiddenField_emmail').value = j[1];
            }
            if (j[0] == 'TradeCode') {
                document.getElementById('HiddenField_TradeCode').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">Net Position (Unprocessed Trades)</span></strong></td>

                <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="fnNoRecord(4);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>

                </td>
            </tr>
        </table>

        <table id="tab1" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">For A Period :
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtFrom">
                                                <DropDownButton Text="From">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtTo">
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
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report View :</td>
                                        <td>
                                            <asp:DropDownList ID="DLLRptView" runat="server" Width="250px" Font-Size="12px">
                                                <asp:ListItem Value="1">Client + Instrument + Trd Code</asp:ListItem>
                                                <asp:ListItem Value="2">Client + Instrument </asp:ListItem>
                                                <asp:ListItem Value="3">TerminalID + Instrument + Client</asp:ListItem>
                                                <asp:ListItem Value="4">Instrument + Client + TerminalID </asp:ListItem>
                                                <asp:ListItem Value="5">Trading Code </asp:ListItem>
                                                <asp:ListItem Value="6">TerminalID + Client + Instrument </asp:ListItem>
                                                <asp:ListItem Value="7"> Client + TerminalID + Instrument </asp:ListItem>
                                                <asp:ListItem Value="8">Client + Date + Instrument </asp:ListItem>
                                                <asp:ListItem Value="9">Client + Date </asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_GroupBy">
                            <td class="gridcellleft">
                                <table class="tableBorderClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Group By</td>
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
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="fnBranch('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="fnBranch('b')" />Selected
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
                                    <tr id="Tr_viewby">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">View By :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlviewby" runat="server" Width="100px" Font-Size="12px" onchange="fnddlview(this.value)">
                                                <asp:ListItem Value="1">Client</asp:ListItem>
                                                <asp:ListItem Value="2">Broker</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Tr_Clients">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Clients :</td>
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
                                    <tr id="Tr_Broker">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Broker :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbbrokerall" runat="server" Checked="True" GroupName="M" onclick="fnbroker('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbbrokerselected" runat="server" GroupName="M" onclick="fnbroker('b')" />
                                            Selected  
                                        </td>
                                        <%--<td>
                                                &nbsp;
                                            </td>--%>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_Instrument">
                            <td class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Instrument :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbInstrumentAll" runat="server" Checked="True" GroupName="ee" onclick="fnInstrument('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdInstrumentSelected" runat="server" GroupName="ee" onclick="fnInstrument('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr id="tr_TerminalID">
                            <td class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Terminal-ID :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbTerminalIDAll" runat="server" Checked="True" GroupName="e" onclick="fnTerminalID('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbTerminalIDSelected" runat="server" GroupName="e" onclick="fnTerminalID('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr id="tr_Trade">
                            <td class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Trade Code :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbTradeCodeAll" runat="server" Checked="True" GroupName="f" onclick="fnTradeCode('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbTradeCodeSelected" runat="server" GroupName="f" onclick="fnTradeCode('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGeneration" runat="server" Width="100px" Font-Size="12px"
                                                onchange="FnddlGeneration(this.value)">
                                                <asp:ListItem Value="1">Screen</asp:ListItem>
                                                <asp:ListItem Value="2">Export</asp:ListItem>
                                                <asp:ListItem Value="3">Send Mail</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_MailSendOption">
                            <td class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Respective :</td>
                                        <td>
                                            <asp:DropDownList ID="ddloptionformail" runat="server" Width="100px" Font-Size="12px">
                                                <asp:ListItem Value="1">User</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table>
                                    <tr>
                                        <td id="td_Screen">
                                            <asp:Button ID="btnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                Width="101px" OnClick="btnScreen_Click" />
                                        </td>
                                        <td id="td_Mail">
                                            <asp:Button ID="btnSendmail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Mail"
                                                Width="101px" OnClick="btnSendmail_Click" /></td>
                                        <td id="td_Export">
                                            <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                Width="101px" OnClick="btnExcel_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                    </table>
                </td>
                <td>
                    <table cellpadding="1" cellspacing="1" id="showFilter">
                        <tr>
                            <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                id="TdFilter">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem>Clients</asp:ListItem>
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Broker</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                    <asp:ListItem>ScripsExchange</asp:ListItem>
                                    <asp:ListItem>BranchGroup</asp:ListItem>
                                    <asp:ListItem>UserID</asp:ListItem>
                                    <asp:ListItem>MAILEMPLOYEE</asp:ListItem>
                                    <asp:ListItem>TradeCode</asp:ListItem>
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
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnhide" runat="server" Text="btnhide" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Broker" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Scrips" runat="server" />
                    <asp:HiddenField ID="HiddenField_TerminalID" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                    <asp:HiddenField ID="hiddencount" runat="server" />
                    <asp:HiddenField ID="HiddenField_emmail" runat="server" />
                    <asp:HiddenField ID="HiddenField_TradeCode" runat="server" />

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
