<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.management_ObligationStatementFO_New" CodeBehind="ObligationStatementFO_New.aspx.cs" %>


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


        function Page_Load()///Call Into Page Load
        {
            Hide('td_dtfrom');
            Hide('td_dtto');
            Hide('showFilter');
            Hide('tr_filter');
            Hide('Tr_Broker');
            //Hide('Tr_Location');
            Hide('td_dos');
            Hide('td_excel');
            RBShowHide('rbShow');
            document.getElementById('tr_hdrall').style.display = 'none';
            document.getElementById('tr_ftrall').style.display = 'none';
            document.getElementById('hiddencount').value = 0;
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


                  }
                  else {
                      Hide('td_dtfor');
                      Show('td_dtfrom');
                      Show('td_dtto');


                  }
                  selecttion();
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
                  else if (obj == "1") {
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
                  height();
              }
              function fnbroker(obj) {
                  if (obj == "a")
                      Hide('showFilter');
                  else {
                      var cmb = document.getElementById('cmbsearchOption');
                      cmb.value = 'Broker';
                      Show('showFilter');
                  }
                  height();
              }

              function RBShowHide(obj) {
                  if (obj == 'rbPrint') {
                      Hide('td_excel');
                      Hide('td_btnshow');
                      Show('td_btnprint');
                      Show('td_bothside');
                      Hide('td_sendMail');
                      Hide('TR_OPTIONFORMAIL');
                      // Hide('Tr_Location');
                      Hide('td_dos');
                      document.getElementById('tr_hdrall').style.display = 'none';
                      document.getElementById('tr_ftrall').style.display = 'none';
                  }
                  else if (obj == 'rbMail') {
                      Hide('td_excel');
                      Hide('td_btnshow');
                      Hide('td_btnprint');
                      Hide('td_bothside');
                      Show('td_sendMail');
                      Show('TR_OPTIONFORMAIL');
                      //Hide('Tr_Location');
                      Hide('td_dos');
                      document.getElementById('tr_hdrall').style.display = 'none';
                      document.getElementById('tr_ftrall').style.display = 'none';
                  }
                  else if (obj == 'rbdos') {
                      Hide('td_excel');
                      Hide('td_btnshow');
                      Hide('td_btnprint');
                      Hide('td_bothside');
                      //Show('Tr_Location');
                      Show('td_dos');
                      document.getElementById('tr_hdrall').style.display = 'inline';
                      document.getElementById('tr_ftrall').style.display = 'inline';

                      document.getElementById('td_hdr').style.display = 'inline';

                      document.getElementById('tdfooter').style.display = 'none';

                      document.getElementById('tdHeader').style.display = 'none';

                      document.getElementById('td_footer').style.display = 'inline';
                      //Show('TR_OPTIONFORMAIL');
                  }
                  else if (obj == 'rbexcel') {
                      Show('td_excel');
                      Hide('td_btnshow');
                      Hide('td_btnprint');
                      Hide('td_bothside');
                      Hide('td_sendMail');
                      Hide('TR_OPTIONFORMAIL');
                      //Hide('Tr_Location');
                      Hide('td_dos');
                      document.getElementById('tr_hdrall').style.display = 'none';
                      document.getElementById('tr_ftrall').style.display = 'none';
                  }
                  else {
                      Show('td_btnshow');
                      Hide('td_btnprint');
                      Hide('td_bothside');
                      Hide('td_sendMail');
                      Hide('td_excel');
                      Hide('TR_OPTIONFORMAIL');
                      // Hide('Tr_Location');
                      Hide('td_dos');
                      document.getElementById('tr_hdrall').style.display = 'none';
                      document.getElementById('tr_ftrall').style.display = 'none';
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
              function ChkCheckProperty(obj, objChk) {

                  if (objChk == true) {
                      if (obj == 'H')
                          document.getElementById('tdHeader').style.display = 'inline';
                      else if (obj == 'F')
                          document.getElementById('tdfooter').style.display = 'inline';
                  }
                  else {
                      if (obj == 'H')
                          document.getElementById('tdHeader').style.display = 'none';
                      else if (obj == 'F')
                          document.getElementById('tdfooter').style.display = 'none';
                  }
              }

              function FunHeaderFooter(objID, objListFun, objEvent, objParam) {
                  ajax_showOptions(objID, objListFun, objEvent, objParam);
              }
              function NORECORD(obj) {
                  Show('tab2')
                  Hide('showFilter');
                  Hide('tr_filter');
                  Hide('displayAll');
                  Show('Td_DateDisply');

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

                  if (document.getElementById('rbScreen').checked == true)
                      RBShowHide('rbShow');
                  if (document.getElementById('rbPrint').checked == true)
                      RBShowHide('rbPrint');
                  if (document.getElementById('rbMail').checked == true)
                      RBShowHide('rbMail');
                  height();
              }
              function Display() {
                  Show('displayAll');
                  Hide('tab2')
                  Show('tr_filter');
                  Hide('showFilter');
                  Hide('Td_DateDisply');
                  height();
              }
              function DisplayLedger() {
                  Show('displayAll');
                  Hide('tab2')
                  Hide('tr_filter');
                  Hide('showFilter');
                  Hide('tr_group');
                  Hide('tr_prvnxt');
                  Hide('Td_DateDisply');
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
              function DisableC(obj) {
                  if (obj == 'P') {
                      Hide('lnkPrev');
                      Show('lnkNext');
                  }
                  else {
                      Show('lnkPrev');
                      Hide('lnkNext');
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
        if (j[0] == 'Group') {
            document.getElementById('HiddenField_Group').value = j[1];
            btn.click();
        }
        if (j[0] == 'BranchGroup') {
            document.getElementById('HiddenField_BranchGroup').value = j[1];
        }
        if (j[0] == 'Clients') {
            document.getElementById('HiddenField_Client').value = j[1];
        }
        if (j[0] == 'Broker') {
            document.getElementById('HiddenField_Broker').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">F& O Obligation/Margin Statement</span></strong></td>
                <td class="EHEADER" width="5%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="NORECORD(6);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                </td>
            </tr>
        </table>
        <table id="tab2" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table>
                        <tr valign="top">
                            <td>
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td class="gridcellleft">
                                            <asp:DropDownList ID="ddldate" runat="server" Width="100px" Font-Size="12px" onchange="fn_ddldateChange(this.value)">
                                                <asp:ListItem Value="0">For a Date</asp:ListItem>
                                                <asp:ListItem Value="1">For a Period</asp:ListItem>
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
                                <table border="10" cellpadding="1" cellspacing="1">
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
                                                    <td id="td_allselect">
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
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Clients :</td>
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
                                    </tr>--%>
                                    <tr id="Tr_viewby">
                                        <td colspan="3">
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
                                        <td colspan="3">
                                            <table>
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
                                    <tr id="Tr_Broker">
                                        <td colspan="3">
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
                                        <td class="gridcellleft" bgcolor="#B7CEEC" colspan="4">
                                            <asp:CheckBox ID="ChkCollateralDeposit" runat="server" />
                                            Generate For Client Without Position
                                                <br />
                                            But Having Collateral Deposit
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td id="Td3" class="gridcellleft">
                                            <asp:RadioButton ID="rbScreen" runat="server" GroupName="f" Checked="True" onclick="RBShowHide(this.value)" />Screen
                                        </td>
                                        <td id="Td1" class="gridcellleft">
                                            <asp:RadioButton ID="rbMail" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Send
                                                Mail
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td id="Td4" class="gridcellleft">
                                                        <asp:RadioButton ID="rbPrint" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />PDF
                                                    </td>
                                                    <td id="Td5" class="gridcellleft">
                                                        <asp:RadioButton ID="rbdos" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Dos
                                                            Print
                                                    </td>
                                                    <td id="Td6" class="gridcellleft">
                                                        <asp:RadioButton ID="rbexcel" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Excel
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%-- <tr id="Tr_Location">
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Printer Selection :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownList1" runat="server" Width="250px" Font-Size="12px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <tr id="tr_hdrall">
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Use Header :
                                        </td>
                                        <td id="td_hdr" runat="server">
                                            <asp:CheckBox ID="chkHeader" runat="server" onclick="ChkCheckProperty('H',this.checked);" />
                                        </td>
                                        <td id="tdHeader" runat="server">
                                            <asp:TextBox ID="txtHeader" runat="server" Width="279px" Font-Size="12px" onkeyup="FunHeaderFooter(this,'GetHeaderFooter',event,'H')"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr id="tr_ftrall">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Use Footer :
                                        </td>
                                        <td id="td_footer" runat="server">
                                            <asp:CheckBox ID="chkFooter" runat="server" onclick="ChkCheckProperty('F',this.checked);" />
                                        </td>
                                        <td id="tdfooter">
                                            <asp:TextBox ID="txtFooter" runat="server" Width="279px" Font-Size="12px" onkeyup="FunHeaderFooter(this,'GetHeaderFooter',event,'F')"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="TR_OPTIONFORMAIL">
                            <td>
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Respective :</td>
                                        <td>
                                            <asp:DropDownList ID="ddloptionformail" runat="server" Width="100px" Font-Size="12px"
                                                onchange="fnddloptionformail(this.value)">
                                                <asp:ListItem Value="0">Client</asp:ListItem>
                                                <asp:ListItem Value="1">Group/Branch</asp:ListItem>
                                                <asp:ListItem Value="2">User</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="td_bothside">
                            <td>
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkBothPrint" runat="server" />Both Side Print
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="CHKLOGOPRINT" runat="server" />
                                            Do Not Print Logo</td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkDISTRIBUTION" runat="server" Checked="true" />
                                            Distribution Copy</td>
                                    </tr>
                                </table>
                            </td>
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
                                                Width="101px" OnClick="btnprint_Click" />
                                        </td>
                                        <td id="td_sendMail" class="gridcellleft">
                                            <asp:Button ID="btnmailsend" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Mail"
                                                Width="101px" OnClick="btnmailsend_Click" /></td>
                                        <td id="td_dos" class="gridcellleft">
                                            <asp:Button ID="btndos" runat="server" CssClass="btnUpdate" Height="20px" Text="Generate"
                                                Width="101px" OnClick="btndos_Click" /></td>
                                        <td id="td_excel" class="gridcellleft">
                                            <asp:Button ID="btnexport" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                Width="101px" OnClick="btnexport_Click" /></td>
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
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Broker</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
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
                <td id="Td_DateDisply">
                    <table>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <div id="DivDateDisply" runat="server">
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
                    <asp:HiddenField ID="txtHeader_hidden" runat="server" />
                    <asp:HiddenField ID="txtFooter_hidden" runat="server" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Broker" runat="server" />
                    <asp:HiddenField ID="HiddenField_emmail" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
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
                                <asp:DropDownList ID="cmbproduct" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                                </asp:DropDownList>
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
                                                    <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Prev" Text="[Prev]" OnCommand="NavigationLinkC_Click"> </asp:LinkButton>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:DropDownList ID="cmbgroup" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cmbgroup_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkNext" runat="server" CommandName="Next" Text="[Next]" OnCommand="NavigationLinkC_Click"> </asp:LinkButton>
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
                        <tr bordercolor="blue" id="tr_prvnxt">
                            <td align="center">
                                <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="100%">
                                    <tr>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                        <td width="25">
                                            <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                        <td width="20">
                                            <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClick="ASPxLast_Click">Last</asp:LinkButton></td>
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
