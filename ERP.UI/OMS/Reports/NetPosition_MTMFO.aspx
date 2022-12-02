<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_NetPosition_MTMFO" CodeBehind="NetPosition_MTMFO.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
     function Page_Load()///Call Into Page Load
     {

         Hide('showFilter');
         Hide('tr_filter');
         Hide('Tr_Broker');
         document.getElementById('hiddencount').value = 0;
         fn_ddldateChange('0');
         fnddlGeneration('0');

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
         Hide('td_lstfiltercolumn');
         if (document.body.scrollHeight >= 350) {
             window.frameElement.height = document.body.scrollHeight;
         }
     }
     function fnddlGeneration(obj) {
         if (document.getElementById('ddlrptview').value == "3") {
             Hide('td_show');
             Show('tr_excel');
             Hide('td_mail');
             Hide('tr_mail');
             Hide('Tr_Option');
             Hide('tr_filtercolumns');
             Hide('Tr_Generation');
             Show('Tr_ExposureInBuyPosition');
         }
         else {
             Show('Tr_Generation');
             Hide('Tr_ExposureInBuyPosition');

             if (obj == '0') {
                 Show('td_show');
                 Hide('tr_excel');
                 Hide('td_mail');
                 Hide('tr_mail');
             }
             if (obj == '1') {
                 Hide('td_show');
                 Hide('tr_excel');
                 Show('td_mail');
                 Show('tr_mail');
             }
             if (obj == '2') {
                 Hide('td_show');
                 Show('tr_excel');
                 Hide('td_mail');
                 Hide('tr_mail');
             }
             Show('Tr_Option');
             Show('tr_filtercolumns');
         }
         if (document.body.scrollHeight >= 350) {
             window.frameElement.height = document.body.scrollHeight;
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
             if (document.getElementById('ddlGroup').value == "0") {
                 document.getElementById('cmbsearchOption').value = 'Branch';
             }
             if (document.getElementById('ddlGroup').value == "2") {
                 document.getElementById('cmbsearchOption').value = 'BranchGroup';
             }
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
     function NORECORD(obj) {
         Hide('displayAll');
         Show('tab2');
         Hide('showFilter');
         fn_ddldateChange(document.getElementById('ddldate').value);
         fnddlGeneration(document.getElementById('ddlGeneration').value);

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
         document.getElementById('hiddencount').value = 0;
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
         document.getElementById('display').className = "grid_scroll";
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

     function rdbcheckchange() {

         var rdball = document.getElementById('rdbunderlyingAll');
         rdball.checked = true;
         var rdbselected = document.getElementById('rdbunderlyingSelected');
         rdbselected.checked = false;
         document.getElementById('btnshow').disabled = false;
         Hide('showFilter');
         height();
     }
     function fnddlview(obj) {
         if (obj == "1") {
             Show('tr_client');
             Hide('Tr_Broker');
         }
         if (obj == "2") {
             Hide('tr_client');
             Show('Tr_Broker');

         }
         if (obj == "3") {
             Hide('tr_client');
             Hide('Tr_Broker');

         }
         selecttion();
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
     function fn_ddlrptchange(obj) {

         if (obj == '0') {
             Show('tr_grpselection');
             Show('tr_charges');
             Hide('Tr_ExposureInBuyPosition');
         }
         if (obj == '2') {
             Hide('tr_grpselection');
             Hide('tr_charges');
             Hide('Tr_ExposureInBuyPosition');

         }
         if (obj == '3') {
             Hide('tr_charges');
             Show('Tr_ExposureInBuyPosition');

         }
         fnddlGeneration(document.getElementById('ddlGeneration').value);
         selecttion();
         height();
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
     function OnConsiderChange(obj) {
         if (obj == 1) {
             document.getElementById('ChkCalculateCharge').checked = false;
             document.getElementById('ChkCalculateCharge').disabled = true;
         }
         else {
             document.getElementById('ChkCalculateCharge').checked = true;
             document.getElementById('ChkCalculateCharge').disabled = false;
         }
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
          if (j[0] == 'Product') {
              document.getElementById('HiddenField_Product').value = j[1];
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
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Net Position </span></strong></td>
                <td class="EHEADER" width="15%" id="tr_filter" style="height: 22px">
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
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td>
                                            <asp:DropDownList ID="ddldate" runat="server" Width="100px" Font-Size="12px" onchange="fn_ddldateChange(this.value)">
                                                <asp:ListItem Value="0">For A Date</asp:ListItem>
                                                <asp:ListItem Value="1">For A Period</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td id="td_dtfor">
                                            <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="dtfor" OnValueChanged="dtfor_DateChanged" AutoPostBack="True">
                                                <DropDownButton Text="For">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                            <%--OnDateChanged="dtfor_DateChanged" AutoPostBack="True"--%>
                                        </td>
                                        <td id="td_dtfrom">
                                            <dxe:ASPxDateEdit ID="dtfrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="dtfrom">
                                                <DropDownButton Text="From">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td id="td_dtto">
                                            <dxe:ASPxDateEdit ID="dtto" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="dtto" OnValueChanged="dtto_DateChanged" AutoPostBack="True">
                                                <DropDownButton Text="To">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                            <%--OnDateChanged="dtto_DateChanged" AutoPostBack="True"--%>
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
                                        <td style="width: 200px">
                                            <asp:DropDownList ID="ddlrptview" runat="server" Width="200px" Font-Size="12px" onchange="fn_ddlrptchange(this.value)">
                                                <asp:ListItem Value="0">Client Wise</asp:ListItem>
                                                <asp:ListItem Value="1">Share Wise</asp:ListItem>
                                                <asp:ListItem Value="3">Only Open Position With Exposure</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr id="tr_grpselection">
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td bgcolor="#B7CEEC">Group By</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                                        <asp:ListItem Value="0">Branch</asp:ListItem>
                                                                        <asp:ListItem Value="1">Group</asp:ListItem>
                                                                        <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td id="td_branch">
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
                                                                                    onclick="Group('a')" />
                                                                                All
                                                                    <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="Group('b')" />Selected
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <%--<tr>
                                                  <td colspan="4"><table><tr>  <td class="gridcellleft" bgcolor="#B7CEEC">
                                                        Clients :</td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="Clients('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="radPOAClient" runat="server" GroupName="c" onclick="Clients('a')" />POA
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="Clients('b')" />
                                                        Selected
                                                    </td></tr></table></td>
                                                </tr>--%>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="Tr_viewby">
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td class="gridcellleft" bgcolor="#B7CEEC">View By :</td>
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
                                                                <td class="gridcellleft" bgcolor="#B7CEEC">Clients :</td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="Clients('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="radPOAClient" runat="server" GroupName="c" onclick="Clients('a')" />POA
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="Clients('b')" />
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
                                                                <td class="gridcellleft" bgcolor="#B7CEEC">Broker :</td>
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
                                                <tr>
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td bgcolor="#B7CEEC" style="height: 21px">Instrument Type :</td>
                                                                <td colspan="2" style="text-align: left; height: 21px;" valign="top">
                                                                    <asp:DropDownList ID="cmbinstrutype" runat="server" Font-Size="12px" AutoPostBack="True"
                                                                        OnSelectedIndexChanged="cmbinstrutype_SelectedIndexChanged">
                                                                        <asp:ListItem Value="0">ALL</asp:ListItem>
                                                                        <asp:ListItem Value="1">FUTSTK &amp; OPTSTK</asp:ListItem>
                                                                        <asp:ListItem Value="2">FUTSTK</asp:ListItem>
                                                                        <asp:ListItem Value="3">OPTSTK</asp:ListItem>
                                                                        <asp:ListItem Value="4">FUTIDX &amp; OPTIDX</asp:ListItem>
                                                                        <asp:ListItem Value="5">FUTIDX</asp:ListItem>
                                                                        <asp:ListItem Value="6">OPTIDX</asp:ListItem>
                                                                        <asp:ListItem Value="7">ALL FUTURE</asp:ListItem>
                                                                        <asp:ListItem Value="8">ALL OPTION</asp:ListItem>
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td bgcolor="#B7CEEC">Underlying:
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbunderlyingAll" runat="server" Checked="true" GroupName="d"
                                                                        onclick="fnunderlying('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbunderlyingSelected" runat="server" GroupName="d" onclick="fnunderlying('b')" />
                                                                    Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td bgcolor="#B7CEEC">Expiry :</td>
                                                                <td colspan="2" style="text-align: left" valign="top">
                                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList ID="cmbexpiry" runat="server" Font-Size="12px">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="cmbinstrutype" EventName="SelectedIndexChanged" />
                                                                            <asp:AsyncPostBackTrigger ControlID="dtfor" EventName="ValueChanged" />
                                                                            <asp:AsyncPostBackTrigger ControlID="dtto" EventName="ValueChanged" />
                                                                            <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td bgcolor="#B7CEEC">Consider :</td>
                                                                <td colspan="2" style="text-align: left" valign="top">
                                                                    <asp:DropDownList ID="ddlNetMktValue" runat="server" Width="100px" Font-Size="12px" onchange="OnConsiderChange(this.value)">
                                                                        <asp:ListItem Value="0">Net Value</asp:ListItem>
                                                                        <asp:ListItem Value="1">Market Value</asp:ListItem>
                                                                    </asp:DropDownList>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table id="showFilter" border="10" cellpadding="1" cellspacing="1">
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
                                                            <asp:ListItem>Product</asp:ListItem>
                                                            <asp:ListItem>BranchGroup</asp:ListItem>
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
                        <tr id="Tr_Option">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkOpenClientFUT" runat="server" onclick="fn_charge(this)" />
                                            Show Only Open Position Clients
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkOpenclinetOPT" Checked="true" runat="server" onclick="selecttion()" />
                                            Consider Open Position in Option Instuments
                                        </td>
                                    </tr>

                                    <tr id="tr_charges">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkCalculateCharge" Checked="true" runat="server" onclick="selecttion()" />
                                            Calculate Charges
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="Chksign" Checked="true" runat="server" onclick="selecttion()" />
                                            Show Open Buy Position in +ve Sign
                                        </td>
                                    </tr>


                                </table>
                            </td>
                        </tr>
                        <tr id="tr_filtercolumns">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
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
                                                            Checked="true" /><span style="font-family: Verdana; color: Teal; font-size: x-small;"><b>Check/UnCheck
                                                                            ALL</b></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; border: 1px black solid; scrollbar-base-color: #C0C0C0">
                                                            <asp:CheckBoxList ID="chktfilter" runat="server" RepeatDirection="Horizontal" Width="600px"
                                                                RepeatColumns="10">
                                                                <asp:ListItem Value="B/F Qty" Selected="True">B/F Qty</asp:ListItem>
                                                                <asp:ListItem Value="Open Price" Selected="True">Open Pricee</asp:ListItem>
                                                                <asp:ListItem Value="B/F Value" Selected="True">B/F Value</asp:ListItem>
                                                                <asp:ListItem Value="Day Buy" Selected="True">Day Buy</asp:ListItem>
                                                                <asp:ListItem Value="Buy Value" Selected="True">Buy Value</asp:ListItem>
                                                                <asp:ListItem Value="Buy Avg." Selected="True">Buy Avg.</asp:ListItem>
                                                                <asp:ListItem Value="Day Sell" Selected="True">Day Sell</asp:ListItem>
                                                                <asp:ListItem Value="Sell Value" Selected="True">Sell Value</asp:ListItem>
                                                                <asp:ListItem Value="Sell Avg." Selected="True">Sell Avg.</asp:ListItem>
                                                                <asp:ListItem Value="ASN/EXC Qty" Selected="True">ASN/EXC Qty</asp:ListItem>
                                                                <asp:ListItem Value="C/F Qty" Selected="True">C/F Qty</asp:ListItem>
                                                                <asp:ListItem Value="Sett Price" Selected="True">Sett Price</asp:ListItem>
                                                                <asp:ListItem Value="C/F Value" Selected="True">C/F Value</asp:ListItem>
                                                                <asp:ListItem Value="Premium" Selected="True">Premium</asp:ListItem>
                                                                <asp:ListItem Value="Mtm" Selected="True">Mtm</asp:ListItem>
                                                                <asp:ListItem Value="Future FinSett" Selected="True">Future FinSett</asp:ListItem>
                                                                <asp:ListItem Value="ASN/EXC Amount" Selected="True">ASN/EXC Amount</asp:ListItem>
                                                                <asp:ListItem Value="Net Obligation" Selected="True">Net Obligation</asp:ListItem>
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
                        <tr id="Tr_ExposureInBuyPosition">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Consider Exposure In Buy Position
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="ChkCall" runat="server" />Call
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkPut" runat="server" />Put
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Exposure @
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlExposure" runat="server" Font-Size="12px">
                                                <asp:ListItem Value="1">Underlying Price</asp:ListItem>
                                                <asp:ListItem Value="2">Instrument Price</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr id="Tr_Generation">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGeneration" runat="server" Width="100px" Font-Size="12px"
                                                onchange="fnddlGeneration(this.value)">
                                                <asp:ListItem Value="0">Screen</asp:ListItem>
                                                <asp:ListItem Value="1">Send Mail</asp:ListItem>
                                                <asp:ListItem Value="2">Export</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
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
                                    </tr>
                                    <tr>

                                        <td id="td_show">
                                            <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                Width="101px" OnClientClick="selecttion()" OnClick="btnshow_Click" /></td>
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
                    <asp:HiddenField ID="HiddenField_Product" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
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
