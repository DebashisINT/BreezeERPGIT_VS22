<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_ObligationStatementFO" CodeBehind="ObligationStatementFO.aspx.cs" %>

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
    </style>

    <script language="javascript" type="text/javascript">
        //Region for email send 
               function Ledger() {
                   document.getElementById('TdAll1').style.display = 'none';
                   document.getElementById('TrAll').style.display = 'none';
                   document.getElementById('TrLed1').style.display = 'none';
                   document.getElementById('TrLed2').style.display = 'none';
                   document.getElementById('TdLedger').style.display = 'none';
                   document.getElementById('Div1').style.display = 'none';
                   document.getElementById('f').style.display = 'none';
               }
               function ForFilterOff() {
                   document.getElementById('TrAll1').style.display = 'none';
                   document.getElementById('TrAll').style.display = 'none';
                   document.getElementById('displayAll').style.display = 'inline';
                   height();
               }
               function MailsendT() {
                   alert("Mail Sent Successfully");
               }
               function MailsendF() {
                   alert("Error on sending!Try again..");
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
               function Page_Load() {

                   document.getElementById('txtSelectionID_hidden').style.display = "none";
                   document.getElementById('Div1').style.display = "none";
                   rsltChk = 0;
                   Hide('tdtxtname');
                   Hide('tdList');
                   Hide('TdPRINT');
                   Show('TdScreen');
                   Hide('displayAll');
                   Hide('td_dtto');
                   Hide('td_dtfrom');
                   Hide('td_chkprint');
                   height();
                   //--------For Email

                   hide('btnfilter');
                   hide('ShowSelectUser');
                   document.getElementById('showFilter1').style.display = "none";
               }

               function clientselectionfinal() {
                   var listBoxSubs = document.getElementById('lstSlection');
                   var listIDs = '';
                   var i;
                   if (listBoxSubs.length > 0) {
                       for (i = 0; i < listBoxSubs.length; i++) {
                           if (listIDs == '')
                               listIDs = "'" + listBoxSubs.options[i].value + "'";
                           else
                               listIDs += "," + "'" + listBoxSubs.options[i].value + "'";
                       }
                   }

                   document.getElementById('clientid').value = listIDs;
                   Show('Div1');
                   var i;
                   for (i = listBoxSubs.options.length; i >= 0; i--) {
                       listBoxSubs.remove(i);
                   }
                   MaiFilterOff();
               }
               function groupselection() {
                   var listBoxSubs = document.getElementById('lstSlection');
                   var listIDs = '';
                   var i;
                   if (listBoxSubs.length > 0) {
                       for (i = 0; i < listBoxSubs.length; i++) {
                           if (listIDs == '')
                               listIDs = listBoxSubs.options[i].value;
                           else
                               listIDs += "," + listBoxSubs.options[i].value;
                       }
                   }
                   if (document.getElementById('ddlGroup').value == "0")//////////////Group By  selected are branch
                   {
                       document.getElementById('branch').value = listIDs;
                   }
                   else {
                       document.getElementById('group').value = listIDs;
                   }
                   if (document.getElementById('cmbsearchOption').value == "Clients") {
                       document.getElementById('btnhide').click();
                   }
                   else {
                       var j;
                       for (j = listBoxSubs.options.length; j >= 0; j--) {
                           listBoxSubs.remove(j);
                       }
                   }

               }
               function afterPartialPostBack() {
                   document.getElementById('Div1').style.display = "none";
                   height();

                   if (rsltChk == 1) {
                       Show('td_main');
                       Hide('displayAll');
                       alert('No Record Found');
                       rsltChk = 0;
                   }
                   else if (rsltChk == 2) {
                       Show('td_main');
                       Hide('displayAll');
                       alert('Rates for this date does not exists');
                       rsltChk = 0;
                   }
                   else if (rsltChk == 3) {
                       Show('td_main');
                       Hide('displayAll');
                       rsltChk = 0;
                   }
                   else {
                       Show('displayAll');
                   }
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


               function btnAddsubscriptionlist_click() {
                   var userid = document.getElementById('txtSelectionID_hidden');
                   if (userid.value != '') {
                       var ids = document.getElementById('txtSelectionID_hidden');
                       var Client = document.getElementById('txtSelectionID');
                       var listBox = document.getElementById('lstSlection');
                       var tLength = listBox.length;

                       var no = new Option();
                       no.value = ids.value;
                       no.text = Client.value;
                       listBox[tLength] = no;
                       var recipient = document.getElementById('txtSelectionID_hidden');
                       recipient.value = '';
                   }
                   else
                       alert('Please search name and then Add!')
                   var s = document.getElementById('txtSelectionID');
                   s.value = '';
                   s.focus();
                   s.select();

               }


               function fnClients(s) {
                   document.getElementById('txtSelectionID_hidden').value = "";
                   if (s == 'A') {
                       Hide('tdtxtname');
                       Hide('tdList');
                       document.getElementById('txtSelectionID_hidden').style.display = "none";
                       document.getElementById('txtSelectionID').value = "";

                   }
                   if (s == 'S') {
                       var listBox = document.getElementById('lstSlection');
                       listBox.length = 0;

                       Show('tdtxtname');
                       Show('tdList');
                       document.getElementById('txtSelectionID_hidden').style.display = "none";

                   }
               }

               function done() {
                   Hide('tdtxtname');
                   Hide('tdList');
                   groupselection();
               }

               FieldName = 'lstSlection';


               function showProgress() {
                   document.getElementById('Div1').style.display = "inline";
               }

               function hideProg() {
                   document.getElementById('Div1').style.display = "none";
               }
               function displaydate(obj) {
                   document.getElementById('spandate').innerText = obj;
               }
               function filter() {
                   Show('td_main');
                   Hide('displayAll');
                   height();
               }
               function displayresult() {

                   Hide('td_main');
                   Show('displayAll');

               }
               function norecord(obj) {
                   rsltChk = obj;

               }
               function RBShowHide(obj) {
                   if (obj == 'rbPrint') {
                       Hide('td_chkprint');
                       Hide('TdScreen');
                       Show('TdPRINT');
                   }
                   else {
                       Hide('td_chkprint');
                       Hide('TdPRINT');
                       Show('TdScreen');
                   }
                   height();
               }
               function ddldateChange() {
                   var obj = document.getElementById('ddldate').value;

                   if (obj == "0") {
                       Hide('td_dtto');
                       Hide('td_dtfrom');
                       Show('td_dtfor');
                   }
                   else {
                       Show('td_dtto');
                       Show('td_dtfrom');
                       Hide('td_dtfor');
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
               function Clients(obj) {
                   if (obj == "a") {
                       Hide('tdList');
                       Hide('tdtxtname');
                   }
                   else {
                       var cmb = document.getElementById('cmbsearchOption');
                       cmb.value = 'Clients';
                       Show('tdList');
                       Show('tdtxtname');
                   }
               }

               function Branch(obj) {
                   if (obj == "a") {
                       Hide('tdList');
                       Hide('tdtxtname');
                   }
                   else {
                       var cmb = document.getElementById('cmbsearchOption');
                       cmb.value = 'Branch';
                       Show('tdList');
                       Show('tdtxtname');

                   }
               }

               function Group(obj) {
                   if (obj == "a") {
                       Hide('tdList');
                       Hide('tdtxtname');
                   }
                   else {
                       var cmb = document.getElementById('cmbsearchOption');
                       cmb.value = 'Group';
                       Show('tdList');
                       Show('tdtxtname');

                   }
               }

               function Hide(obj) {
                   document.getElementById(obj).style.display = 'none';
               }
               function Show(obj) {
                   document.getElementById(obj).style.display = 'inline';
               }
               function FunClientScrip(objID, objListFun, objEvent) {
                   var cmbVal;
                   if (document.getElementById('group').value == "" && document.getElementById('branch').value == "") {
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
                                   cmbVal = cmbVal + '~' + 'Selected' + '~' + document.getElementById('branch').value;
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
                                   cmbVal = cmbVal + '~' + 'Selected' + '~' + document.getElementById('group').value;
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
                   showProgress();
                   document.getElementById('BTNLODINGDDLGROUP').click();
               }

               //-----------------Email New------------------------------------


               function btnAddEmailtolist_click() {

                   var cmb = document.getElementById('cmbsearch');
                   var userid = document.getElementById('txtName');
                   if (userid.value != '') {
                       var ids = document.getElementById('txtName_hidden');

                       var listBox = document.getElementById('SelectionList');
                       var tLength = listBox.length;



                       var no = new Option();
                       no.value = ids.value;
                       no.text = userid.value;

                       listBox[tLength] = no;
                       var recipient = document.getElementById('txtName');
                       recipient.value = '';
                   }
                   else
                       alert('Please search name and then Add!')
                   var s = document.getElementById('txtName');
                   s.focus();
                   s.select();

               }


               function callAjax1(obj1, obj2, obj3) {
                   // document.getElementById('SelectionList').style.display='none';
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
                       document.getElementById('txttdname').style.display = 'none';

                   }
                   else {
                       alert("Please select email from list.")
                   }

                   var i;
                   for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                       listBoxSubs.remove(i);
                   }
                   if (cmb.value == "EM") {
                       document.getElementById('showFilter1').style.display = "inline";
                       document.getElementById('ShowSelectUser').style.display = "none";
                   }
                   height();


               }

               function btnRemoveEmailFromlist_click() {
                   selecttion();
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


               function SelectUserClient(obj) {
                   if (obj == 'Client') {

                       document.getElementById('ShowSelectUser').style.display = 'inline';
                       document.getElementById('ShowTable').style.display = 'none';
                       document.getElementById('showFilter1').style.display = 'inline';
                       window.frameElement.height = document.body.scrollHeight;

                   }
                   else if (obj == 'User') {
                       document.getElementById('ShowTable').style.display = 'inline';
                       document.getElementById('ShowSelectUser').style.display = 'inline';
                       document.getElementById('showFilter1').style.display = 'none';
                       window.frameElement.height = document.body.scrollHeight;
                   }
               }

               function Sendmail() {
                   document.getElementById('ShowSelectUser').style.display = 'inline';
                   if (document.getElementById('rbClientUser').checked == true) {
                       document.getElementById('showFilter1').style.display = "none";
                   }
                   else {
                       document.getElementById('showFilter1').style.display = "inline";
                   }
                   if (document.getElementById('ddlGroup').value == "0") {
                       document.getElementById('tdBrGr').style.display = "inline";
                       document.getElementById('tdCl').style.display = "inline";
                       document.getElementById('tdSU').style.display = "inline";
                       document.getElementById('lblSelectBrCl').value = 'Respective Branch';

                   }
                   else if (document.getElementById('ddlGroup').value == "1") {

                       document.getElementById('tdBrGr').style.display = "inline";
                       document.getElementById('tdCl').style.display = "inline";
                       document.getElementById('tdSU').style.display = "inline";
                       document.getElementById('lblSelectBrCl').value = 'Respective Group';

                   }

               }


               function User(obj) {

                   if (obj == "User") {
                       document.getElementById('txttdname').style.display = "inline";
                       document.getElementById('txtName_hidden').style.display = "none";
                       document.getElementById('txtName').focus();
                       document.getElementById('showFilter1').style.display = "none";
                   }
                   else {
                       document.getElementById('txttdname').style.display = "none";
                       document.getElementById('tdname').style.display = "none";
                       document.getElementById('showFilter1').style.display = "inline";

                   }

                   height();
               }

               function callAjaxEmail(obj1, obj2, obj3) {
                   var combo = document.getElementById("cmbsearch");
                   var set_value = combo.value
                   var obj4 = 'Main';
                   ajax_showOptions(obj1, obj2, obj3, set_value, obj4)
               }
               function MaiFilterOff() {
                   document.getElementById('ShowSelectUser').style.display = 'none';
                   document.getElementById('txttdname').style.display = "none";
                   document.getElementById('showFilter1').style.display = "none";
               }
               </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="clientid" runat="server" />
    <asp:HiddenField ID="group" runat="server" />
    <asp:HiddenField ID="branch" runat="server" />
    <div>
        <asp:ScriptManager runat="server" ID="s1" AsyncPostBackTimeout="360000">
        </asp:ScriptManager>

        <script language="javascript" type="text/javascript">

                function load() {
                 Sys.WebForms.PageRequestManager.getInstance().add_endRequest(afterPartialPostBack);
             }
                   </script>

        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Bill Printing</span></strong>
                </td>
                <td class="EHEADER" width="25%" id="f" style="height: 20px">
                    <a href="javascript:void(0);" onclick="filter();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a> || <a href="javascript:void(0);"
                        onclick="Sendmail();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Send Email</span></a>
                </td>
            </tr>
        </table>
        <table class="TableMain100" id="td_main">
            <tr id="TdAll1">
                <td colspan="2">
                    <table width="100%">
                        <tr valign="top">
                            <td style="width: 110px">
                                <asp:DropDownList ID="ddldate" runat="server" Width="100px" Font-Size="12px" onchange="ddldateChange()">
                                    <asp:ListItem Value="0">For a Date</asp:ListItem>
                                    <asp:ListItem Value="1">For a Period</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td id="td_dtfor" style="width: 130px;">
                                <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfor">
                                    <DropDownButton Text="For">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td id="td_dtfrom" style="width: 130px;">
                                <dxe:ASPxDateEdit ID="dtfrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfrom">
                                    <DropDownButton Text="From">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td id="td_dtto" style="width: 130px;">
                                <dxe:ASPxDateEdit ID="dtto" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtto">
                                    <DropDownButton Text="To">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td></td>
                            <td class="gridcellleft" style="vertical-align: top; text-align: right" id="tdtxtname">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem>Clients</asp:ListItem>
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                </asp:DropDownList>
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List
                                </span></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TrAll">
                <td style="text-align: left; vertical-align: top;">
                    <table border="0">
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
                                All Client
                            </td>
                            <td>
                                <asp:RadioButton ID="radPOAClient" runat="server" GroupName="d" onclick="Clients('a')" />POA
                                    Client
                            </td>
                            <td>
                                <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="d" onclick="Clients('b')" />
                                Selected Client
                            </td>
                            <td colspan="2"></td>
                        </tr>
                    </table>
                    <table border="0">
                        <tr>
                            <td id="Td3">
                                <asp:RadioButton ID="rbScreen" runat="server" GroupName="a" Checked="True" onclick="RBShowHide(this.value)" />Screen
                            </td>
                            <td id="Td4">
                                <asp:RadioButton ID="rbPrint" runat="server" GroupName="a" onclick="RBShowHide(this.value)" />Print
                            </td>
                            <td id="td_chkprint">
                                <asp:CheckBox ID="ChkPrint" runat="server" />
                                Check here for Both Page Printing
                            </td>
                        </tr>
                    </table>
                    <table border="0">
                        <tr>
                            <td id="TdScreen">
                                <dxe:ASPxButton ID="btnScreen" runat="server" Text="Generate" OnClick="btnScreen_Click">
                                    <ClientSideEvents Click="function(s, e) {
	                            clientselectionfinal();
                            }" />
                                </dxe:ASPxButton>
                            </td>
                            <td id="TdPRINT">
                                <dxe:ASPxButton ID="btnPrint" runat="server" AutoPostBack="True" Text="Generate"
                                    OnClick="btnPrint_Click">
                                    <ClientSideEvents Click="function(s, e) {
	                            clientselectionfinal();hideProg();
                            }" />
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="text-align: right; vertical-align: top; width: 16%">
                    <table width="100%" id="tdList">
                        <tr>
                            <td style="text-align: right; vertical-align: top; height: 134px;">
                                <table cellpadding="0" cellspacing="0">
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
                                                        <a id="A2" href="javascript:void(0);" onclick="done()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
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
                            <td style="height: 23px">
                                <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                                <asp:Button ID="BTNLODINGDDLGROUP" runat="server" Text="BTN" OnClick="BTNLODINGDDLGROUP_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="displayAll">
        <table width="100%" id="TdLedger">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <span id="spandate" style="color: Blue; font-weight: bold"></span>
                            </td>
                            <td>
                                <table id="ShowSelectUser">
                                    <tr>
                                        <td id="tdBrGr">
                                            <table>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:RadioButton ID="rbOnlyClient" runat="server" Checked="true" GroupName="h" />
                                                    </td>
                                                    <td valign="top">
                                                        <%-- <asp:Label ID="lblSelectBrCl" runat="server" Text=""></asp:Label>--%>
                                                        <asp:TextBox ID="lblSelectBrCl" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="tdCl">
                                            <table>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:RadioButton ID="rbRspctvClient" runat="server" Checked="true" GroupName="h" />
                                                    </td>
                                                    <td valign="top">
                                                        <%-- <asp:Label ID="lblSelectBrCl" runat="server" Text=""></asp:Label>--%>
                                                            Respective Client
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="tdSU">
                                            <table>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:RadioButton ID="rbClientUser" runat="server" GroupName="h" />
                                                    </td>
                                                    <td valign="top">Selected User
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="updatepanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="showFilter1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="Button2" runat="server" OnClick="Button1_Click" CssClass="btnUpdate"
                                                        Text="Send" OnClientClick="javascript:showProgress();clientselectionfinal();" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%--  <asp:LinkButton Font-Bold="True" Font-Underline="True" ForeColor="Blue" ID="LinkButton1"
                                       runat="server" OnClick="LinkButton1_Click" OnClientClick="javascript:showProgress();">Send Mail</asp:LinkButton>--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table id="txttdname">
                                    <%--<table id="txttdname">--%>
                                    <tr>
                                        <td id="tdname" valign="top">
                                            <span class="Ecoheadtxt" style="color: blue"><strong></strong></span>
                                        </td>
                                        <%-- <td id="txttdname">
                                                            <asp:TextBox ID="txtName_hidden" runat="server" Width="14px"></asp:TextBox>
                                                            <asp:TextBox ID="txtName" runat="server" Width="250px" Font-Size="11px" Height="20px"></asp:TextBox>
                                                        </td>--%>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter1">
                                                        <span id="spanal2">
                                                            <%-- <asp:TextBox ID="txtName" runat="server" Font-Size="12px" Width="285px"></asp:TextBox></span>--%>
                                                            <span id="span2">
                                                                <asp:TextBox ID="txtName" runat="server" Font-Size="12px" Width="285px"></asp:TextBox></span>
                                                            <span id="span1" visible="false">
                                                                <asp:DropDownList ID="cmbsearch" runat="server" Width="70px" Font-Size="12px" Enabled="false">
                                                                    <asp:ListItem Value="EM" Text="Employees"></asp:ListItem>
                                                                </asp:DropDownList></span> <a id="A3" href="javascript:void(0);" onclick="btnAddEmailtolist_click()">
                                                                    <span style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                                        style="color: #009900; font-size: 8pt;"> </span></td>
                                                </tr>
                                                <tr>
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
                                                        <asp:TextBox ID="txtName_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
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
                                    <tr style="display: none">
                                        <td style="height: 23px">
                                            <asp:TextBox ID="TextBox1" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                            <%-- <asp:Button ID="Button1" runat="server" Text="Button" OnClick="btnhide_Click" />--%>
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
                    <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="brokerFilter" runat="server">
                                <tr valign="top">
                                    <td style="height: 44px">
                                        <asp:LinkButton ID="lnkPrevClient" runat="server" CommandName="Prev" Text="[Prev]"
                                            OnCommand="NavigationLinkC_Click" OnClientClick="javascript:showProgress();"> </asp:LinkButton>
                                    </td>
                                    <td style="height: 44px">
                                        <asp:DropDownList ID="cmbgroup" runat="server" Font-Size="12px" Width="300px" onchange="btndisplay()">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="height: 44px">
                                        <asp:LinkButton ID="lnkNextClient" runat="server" CommandName="Next" Text="[Next]"
                                            OnCommand="NavigationLinkC_Click" OnClientClick="javascript:showProgress();"> </asp:LinkButton>&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="BTNLODINGDDLGROUP" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel runat="server" ID="u1">
            <ContentTemplate>
                <table width="100%" border="1">
                    <tr bordercolor="Blue" id="TrLed1">
                        <td align="center">
                            <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="100%">
                                <tr>
                                    <td width="20" style="padding: 5px">
                                        <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                            OnClientClick="javascript:showProgress();" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                    <td width="25">
                                        <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                            OnClientClick="javascript:showProgress();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                    <td width="20" style="padding: 5px">
                                        <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                            OnClientClick="javascript:showProgress();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                    <td width="20">
                                        <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:showProgress();"
                                            OnClick="ASPxLast_Click">Last</asp:LinkButton></td>
                                    <td align="right">
                                        <asp:Label ID="listRecord" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <span style="color: red; text-align: center; display: none" id="norecord" class="Ecoheadtxt"
                                runat="server"><strong>No Record Found</strong></span>
                        </td>
                    </tr>
                    <tr bordercolor="Gray" id="TrLed2">
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" style="text-align: left">
                                <tr style="font-size: 12px; font-family: Calibri">
                                    <td>
                                        <span style="color: Green;"><b>Client Name :</b></span>
                                        <asp:Label ID="CName" runat="server" Font-Bold="True"></asp:Label>
                                        <asp:HiddenField ID="CID" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="8" style="height: 12px"></td>
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
                    <asp:HiddenField ID="Totalgroup" runat="server" />
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="BTNLODINGDDLGROUP" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 20%; height: 80; width: 200;'>
        <table style="width: 15%;" height='35' border='1' cellpadding='0' cellspacing='0'
            bordercolor='#C0D6E4'>
            <tr>
                <td style="width: 100%;">
                    <table style="width: 100%;">
                        <tr>
                            <td align='center' bgcolor='#FFFFFF' style="height: 25px; width: 20%;">
                                <img src='/assests/images/progress.gif' width='18' height='18'></td>
                            <td align='center' bgcolor='#FFFFFF' style="height: 25px; width: 80%;">
                                <font size='1' face='Verdana'><strong align='center'>Please Wait..</strong></font></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

