<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_PortfolioPerformanceFO_New" CodeBehind="PortfolioPerformanceFO_New.aspx.cs" %>

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
            width: 80%;
            scrollbar-base-color: #C0C0C0;
        }
    </style>
    <script language="javascript" type="text/javascript">


     function Page_Load()///Call Into Page Load
     {
         Hide('showFilter');
         Hide('td_filter');
         Hide('td_sendmail');
         Hide('tr_mail');
         document.getElementById('hiddencount').value = 0;
         FnDateChange('0');
         FnddlGeneration('1');
         height();

     }
     function height() {
         if (document.body.scrollHeight >= 450) {
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
     function FnAsset(obj) {
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

     function FnddlGeneration(obj) {
         if (obj == "1") {
             Show('td_Screen');
             Hide('td_Export');
             Hide('td_sendmail');
             Hide('tr_mail');

         }
         if (obj == "2") {
             Hide('td_Screen');
             Show('td_Export');
             Hide('td_sendmail');
             Hide('tr_mail');

         }
         if (obj == "3") {
             Hide('td_Screen');
             Hide('td_Export');
             Show('td_sendmail');
             Show('tr_mail');

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
     function fn_Terminal(obj) {
         if (obj == "a")
             Hide('td_terminaltxt');
         else
             Show('td_terminaltxt');
         selecttion();
         height();
     }
     function fnexpirtycallajax(objID, objListFun, objEvent) {
         var date;
         if (document.getElementById('ddldate').value == '0') {
             date = new Date(DtFor.GetDate());
             date = parseInt(date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
         }
         else {
             date = new Date(DtFrom.GetDate());
             date = parseInt(date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
         }
         ajax_showOptions(objID, 'Searchproductandeffectuntil', objEvent, 'Expiry' + '~' + date);
     }
     function fnTerminalcallajax(objID, objListFun, objEvent) {
         var datefrom;
         var dateto;
         var date;

         datefrom = new Date(DtFrom.GetDate());
         dateto = new Date(DtTo.GetDate());

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
     function selecttion() {
         var combo = document.getElementById('cmbExport');
         combo.value = 'Ex';
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
     function FnDateChange(obj) {
         if (obj == "0") {
             Show('Td_For');
             Hide('Td_From');
             Hide('Td_To');
             Hide('Tr_DatePeriod');
             Hide('Td_IncludeCharge');
             Hide('Td_IncludeInterest');
         }
         if (obj == "1") {
             Hide('Td_For');
             Show('Td_From');
             Show('Td_To');
             Show('Tr_DatePeriod');
             Show('Td_IncludeCharge');
             Show('Td_IncludeInterest');
         }
     }
     function FnIgnoreBfPostion() {
         if (document.getElementById('ChkBfPosition').checked == true) {
             Show('tr_Terminalid');
             Hide('Td_ValuebfPostionAtPrevClose');

         }
         else {
             Hide('tr_Terminalid');
             Show('Td_ValuebfPostionAtPrevClose');
         }
     }
     function checkall() {
         Show('td_spotconsolidated');
     }
     function uncheckall() {
         Hide('td_spotconsolidated');
     }
     function FnRptView(obj) {
         if (obj == "1") {
             Show('Td_IncludeCharge');
             Show('Td_IncludeInterest');
             document.getElementById('tr_mail').disabled = false;
             document.getElementById('btnmail').disabled = false;
         }
         if (obj == "2") {
             Hide('Td_IncludeCharge');
             Hide('Td_IncludeInterest');
             document.getElementById('tr_mail').disabled = true;
             document.getElementById('btnmail').disabled = true;
             //Hide('btnmail');
         }
     }
     function FnRptType(obj) {
         if (obj == "1") {
             Show('Td_Sign');
             Show('Td_OpenPosition');
             //         Show('tr_mail');
             //         Show('btnmail');  
             document.getElementById('tr_mail').disabled = false;
             document.getElementById('btnmail').disabled = false;
         }
         if (obj == "2") {
             Hide('Td_Sign');
             Hide('Td_OpenPosition');
             document.getElementById('tr_mail').disabled = true;
             document.getElementById('btnmail').disabled = true;
         }
     }
     function fnRecord(obj) {
         if (obj == "1")////////For No Record
         {
             Hide('td_filter');
             Show('tab1');
             Hide('displayAll');
             alert('No Record Found!!');
         }
         if (obj == "3")////////For Filter
         {
             Hide('td_filter');
             Show('tab1');
             Hide('displayAll');
         }
         if (obj == "4")////////For Instrument Type Selection
         {
             Hide('td_filter');
             Show('tab1');
             Hide('displayAll');
             alert('Please Select Atleast One Instrument Type!!');
         }
         if (obj == "5")////////For RptType:Detail
         {
             Show('td_filter');
             Hide('tab1');
             Show('displayAll');
             Show('Tr_PrevNext');
             document.getElementById('Divdisplay').className = "grid_scroll";

         }
         if (obj == "6")////////For RptType:Summary
         {
             Show('td_filter');
             Hide('tab1');
             Show('displayAll');
             Hide('Tr_PrevNext');

         }
         document.getElementById('hiddencount').value = 0;
         Hide('showFilter');
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
          if (j[0] == 'Group') {
              document.getElementById('HiddenField_Group').value = j[1];
          }
          if (j[0] == 'Clients') {
              document.getElementById('HiddenField_Client').value = j[1];
          }
          if (j[0] == 'BranchGroup') {
              document.getElementById('HiddenField_BranchGroup').value = j[1];
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
                <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="fnRecord(3);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="tab1" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td class="gridcellleft">
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="ddldate" runat="server" Width="100px" Font-Size="12px" onchange="FnDateChange(this.value)">
                                    <asp:ListItem Value="0">As on Date</asp:ListItem>
                                    <asp:ListItem Value="1">For a Period</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td id="Td_For" class="gridcellleft">
                                <dxe:ASPxDateEdit ID="DtFor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="DtFor">
                                    <DropDownButton Text="For">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td id="Td_From" class="gridcellleft">
                                <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="DtFrom">
                                    <DropDownButton Text="From">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td id="Td_To" class="gridcellleft">
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
            <tr id="Tr_DatePeriod">
                <td class="gridcellleft">
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:CheckBox ID="ChkBfPosition" runat="server" onclick="FnIgnoreBfPostion(this.value)" />
                                Ignore Brought Forward Position</td>
                            <td id="Td_ValuebfPostionAtPrevClose">
                                <asp:CheckBox ID="ChkBfPositionValue" runat="server" />
                                Value B/F Position at Prev. Close</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <table>
                        <tr>
                            <td>
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td bgcolor="#B7CEEC">Report View :</td>
                                        <td>
                                            <asp:DropDownList ID="DdlRptView" runat="server" Font-Size="11px" Width="200px" Enabled="true"
                                                onchange="FnRptView(this.value)">
                                                <asp:ListItem Value="1">Branch/Group - Client Wise</asp:ListItem>
                                                <asp:ListItem Value="2">Asset - Instrument Wise</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report Type :</td>
                                        <td>
                                            <asp:DropDownList ID="DdlRptType" runat="server" Font-Size="11px" Width="200px" Enabled="true"
                                                onchange="FnRptType(this.value)">
                                                <asp:ListItem Value="1">Detail</asp:ListItem>
                                                <asp:ListItem Value="2">Summary</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Mtm Calculation Basis :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlMtmCalBasis" runat="server" Font-Size="11px" Width="200px">
                                                <asp:ListItem Value="1">Instrument Close</asp:ListItem>
                                                <asp:ListItem Value="0">Asset & Instr Close</asp:ListItem>
                                                <asp:ListItem Value="2">Asset Close</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td valign="top">
                    <table>
                        <tr>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" valign="top">
                                            <table border="10" cellpadding="1" cellspacing="1">
                                                <tr id="Tr_GroupBy">
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
                                                <tr id="Tr_Clients">
                                                    <td colspan="4">
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
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
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="tr_Terminalid" style="display: none;">
                                                    <td colspan="4">
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
                                                    <td colspan="4">
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td class="gridcellleft" style="width: 100px;" bgcolor="#B7CEEC">Asset:</td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbunderlyingall" runat="server" Checked="True" GroupName="d"
                                                                        onclick="FnAsset('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbunderlyingselected" runat="server" GroupName="d" onclick="FnAsset('b')" />Selected
                                                                    Asset
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
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
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table border="10" cellpadding="1" cellspacing="1" id="showFilter">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                            id="TdFilter">
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="ajaxcall(this,'ShowClientFORMarginStocks',event)"></asp:TextBox><asp:DropDownList
                                                ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px" Enabled="false">
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
            <tr>
                <td class="gridcellleft">
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">Instrument Type :</td>
                            <td>
                                <asp:CheckBoxList ID="chkinstrutype" runat="server" RepeatDirection="Horizontal"
                                    Width="150px">
                                    <asp:ListItem Value="FUT" Selected="True">Future</asp:ListItem>
                                    <asp:ListItem Value="C" Selected="True">Call</asp:ListItem>
                                    <asp:ListItem Value="P" Selected="True">Put</asp:ListItem>
                                    <asp:ListItem Value="Spot" Selected="True" onclick="if(this.checked){checkall()}else{uncheckall()}">Spot</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                            <td>
                                <table>
                                    <tr valign="top">
                                        <td class="gridcellleft" bgcolor="#B7CEEC" id="td_spotconsolidated">
                                            <asp:CheckBox ID="ChkConsolidatedSpot" runat="server" onclick="selecttion()" />
                                            Consolidated Spot Position Across Exchange</td>
                                    </tr>
                                </table>
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr_ShowClients" style="display: none;">
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
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft" id="Td_OpenPosition">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkopen" runat="server" onclick="selecttion()" />
                                            Show Only Open Position</td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellleft" id="Td_ClosingPrice">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkclosepricezero" runat="server" onclick="selecttion()" />
                                            Do Not Consider Closing Prices</td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellleft" id="Td_IncludeCharge">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkIncludeCharges" runat="server" onclick="selecttion()" />
                                            Include Charges</td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Calculate On:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlCalculateOn" runat="server" Font-Size="11px" Width="100px">
                                                <asp:ListItem Value="CustomerTrades_NetValue">Net Rate</asp:ListItem>
                                                <asp:ListItem Value="CustomerTrades_MarketValue">Market Rate</asp:ListItem>
                                            </asp:DropDownList>
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
                            <td class="gridcellleft" id="Td_Sign">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkopenbfpositive" Checked="true" runat="server" onclick="selecttion()" />
                                            Show Open Buy Position in +ve Sign
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellleft" id="Td_Premium">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chknetpremium" runat="server" onclick="selecttion()" />
                                            Show Net Premium
                                        </td>
                                        <td class="gridcellleft" id="Td_IncludeInterest">
                                            <table border="10" cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                                        <asp:CheckBox ID="ChkIncludeInterest" runat="server" onclick="selecttion()" />
                                                        Include Interest</td>
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
                <td class="gridcellleft">
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type :</td>
                            <td>
                                <asp:DropDownList ID="ddlGeneration" runat="server" Width="100px" Font-Size="12px"
                                    onchange="FnddlGeneration(this.value)">
                                    <asp:ListItem Value="1">Screen</asp:ListItem>
                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                    <asp:ListItem Value="3">Send Email</asp:ListItem>

                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="tr_mail">
                <td class="gridcellleft">
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">Respective :</td>
                            <td>
                                <asp:DropDownList ID="ddloptionformail" runat="server" Width="100px" Font-Size="12px"
                                    onchange="fnddloptionformail(this.value)">
                                    <asp:ListItem Value="0">Client</asp:ListItem>
                                    <asp:ListItem Value="1">Group/Branch</asp:ListItem>
                                    <asp:ListItem Value="2">User</asp:ListItem>
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
                            <td id="td_sendmail">
                                <asp:Button ID="btnmail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Email"
                                    Width="101px" OnClick="btnmail_Click" />
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
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:HiddenField ID="hiddencount" runat="server" />
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
        <div id="displayAll" style="display: none;" width="100%">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">
                        <tr style="display: none;">
                            <td>
                                <asp:DropDownList ID="cmbclient" runat="server" Font-Size="12px" Width="300px"
                                    AutoPostBack="True" onchange="selecttion()">
                                </asp:DropDownList></td>
                        </tr>

                        <tr>
                            <td>
                                <div id="DivHeader" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table runat="server">
                                            <tr valign="top">
                                                <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Prev" Text="[Prev]" OnCommand="NavigationLinkC_Click"
                                                        OnClientClick="javascript:selecttion();"> </asp:LinkButton>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:DropDownList ID="cmbgroup" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True"
                                                        onchange="selecttion()" OnSelectedIndexChanged="cmbgroup_SelectedIndexChanged">
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
                                        <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="border-color: Blue;" id="Tr_PrevNext">
                            <td align="left">
                                <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="10%">
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
                                        <td align="right" style="display: none;"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="Divdisplay" runat="server">
                                </div>
                            </td>
                        </tr>
                        <asp:HiddenField ID="TotalRecord" runat="server" />
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
