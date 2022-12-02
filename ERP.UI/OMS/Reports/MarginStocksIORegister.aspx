<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_MarginStocksIORegister" EnableEventValidation="false" Codebehind="MarginStocksIORegister.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
 <script language="javascript" type="text/javascript">


     function Page_Load()///Call Into Page Load
     {
         Hide('showFilter');
         Hide('td_filter');
         Hide('Td_ShoNegative');
         Hide('Td_CorpAcType');
         Hide('Tr_DdlCorpAcType');
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

         if (document.getElementById('cmbsearchOption').value == "ClientsSelction") {
             var criteritype = 'B';
             if (document.getElementById('rdbSegmentAll').checked) {
                 criteritype = 'ALL';
             }
             else {
                 criteritype = 'Exchange';
             }

             cmbVal = 'ClientsSelction' + '~' + criteritype;
         }
         else if (document.getElementById('cmbsearchOption').value == "Group") {
             cmbVal = 'Group' + '~' + document.getElementById('ddlgrouptype').value;
         }
         else {
             cmbVal = document.getElementById('cmbsearchOption').value;
         }
         ajax_showOptions(objID, objListFun, objEvent, cmbVal);

     }

     function fnBranchclient(obj) {

         if (obj == "a")
             Hide('showFilter');
         else {

             if (document.getElementById('ddlGroup').value == "1") {
                 document.getElementById('cmbsearchOption').value = 'Branch';
             }
             if (document.getElementById('ddlGroup').value == "3") {
                 document.getElementById('cmbsearchOption').value = 'ClientsSelction';
             }
             Show('showFilter');
         }

     }
     function fnInstrument(obj) {
         if (obj == "a")
             Hide('showFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Scrips';
             Show('showFilter');
         }

     }
     function fnSegment(obj) {
         if (obj == "a") {
             Hide('showFilter');
             Hide('Td_CurrentSpecific');
         }
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Segment';
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
         if (obj == "1" || obj == "3") {
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

     function FnddlGeneration(obj) {

         if (obj == "1")////////Screen
         {
             Show('td_Screen');
             Hide('td_Export');
             Hide('td_printlogo');
             Hide('td_Email');
             Hide('Tr_RptTypeAccountwise');
             Hide('Tr_RptTypeClientWise');
         }
         if (obj == "2")////////Export
         {
             Hide('td_Screen');
             Show('td_Export');
             Show('td_printlogo');
             Hide('td_Email');
             Hide('Tr_RptTypeAccountwise');
             Hide('Tr_RptTypeClientWise');
         }
         if (obj == "3")///////Email
         {
             Hide('td_Screen');
             Hide('td_Export');
             Hide('td_printlogo');
             Show('td_Email');
             EmailVisible();
         }

     }
     function EmailVisible() {
         if (document.getElementById('ddlreporttype').value == "1") {
             Hide('Tr_RptTypeAccountwise');
             Show('Tr_RptTypeClientWise');
             Hide('showFilter');
         }
         else {
             Show('Tr_RptTypeAccountwise');
             Hide('Tr_RptTypeClientWise');
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'MAILEMPLOYEE';
             Show('showFilter');
         }
     }
     function fn_ddlreportchange(obj) {

         if (obj == "1") {
             Show('Tr_ChkBoxOtherSelection');
             Hide('Td_ShoNegative');
             Hide('Td_CorpAcType');
             Show('Tr_PendingPurSaleValuemethod');
             Show('Tr_ChkBoxOtherSelection');
         }
         if (obj == "2" || obj == "3") {
             Hide('Tr_ChkBoxOtherSelection');
             Show('Td_ShoNegative');
             Show('Td_CorpAcType');
             Hide('Tr_PendingPurSaleValuemethod');
             Hide('Tr_ChkBoxOtherSelection');
         }
         Hide('Tr_DdlCorpAcType');
         FnddlGeneration(document.getElementById('ddlGeneration').value);
         document.getElementById('ChkPendingPurchase').checked = false;
         document.getElementById('ChkPendingSales').checked = false;
         document.getElementById('ChkLedgerBaln').checked = false;
         document.getElementById('ChkCashMarginDep').checked = false;
         document.getElementById('ChkNegative').checked = false;
         document.getElementById('ChkDpDetails').checked = false;
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
     function RecordStatus(obj) {

         Hide('showFilter');
         if (obj == "1" || obj == '4' || obj == '5' || obj == '6' || obj == '7') {
             Show('tab1');
             Hide('displayAll');
             Hide('td_filter');

             if (obj == '4')
                 alert("Mail Sent Successfully !!");
             if (obj == '5')
                 alert("Error on sending!Try again.. !!");
             if (obj == '6')
                 alert("'Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...'");
             if (obj == '7')
                 alert("E-Mail Id Could Not Be Found !!");
             if (obj == "1")
                 alert('No Record Found!!');

             document.getElementById('hiddencount').value = 0;
             FnddlGeneration(document.getElementById('ddlGeneration').value);
             fn_ddlreportchange(document.getElementById('ddlreporttype').value);

         }
         if (obj == "2") {
             Show('tab1');
             Hide('displayAll');
             Hide('td_filter');
             document.getElementById('hiddencount').value = 0;
             FnddlGeneration(document.getElementById('ddlGeneration').value);
             fn_ddlreportchange(document.getElementById('ddlreporttype').value);
         }
         if (obj == "3") {

             Hide('tab1');
             Show('displayAll');
             Show('td_filter');
             document.getElementById('hiddencount').value = 0;

         }


         selecttion();

         height();
     }
     function selecttion() {
         var combo = document.getElementById('ddlExport');
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
     function FnCorpAc(obj) {
         if (obj.checked == true) {
             Show('Tr_DdlCorpAcType');
         }
         else {
             Hide('Tr_DdlCorpAcType');
         }

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
          if (j[0] == 'ClientsSelction') {
              document.getElementById('HiddenField_Client').value = j[1];
          }
          if (j[0] == 'Scrips') {
              document.getElementById('HiddenField_Scrips').value = j[1];
          }
          if (j[0] == 'Segment') {
              document.getElementById('HiddenField_Segment').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">Margin Stocks Inward/Outward Register </span></strong></td>
                <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="RecordStatus(2);"><span style="color: Blue;
                        text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged"
                       >
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                        <asp:ListItem Value="P">PDF</asp:ListItem>
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
                                <table class="tableClass"cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td class="gridcellleft">
                                            <table>
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                                        For A Date :
                                                    </td>
                                                    <td class="gridcellleft">
                                                        <dxe:ASPxDateEdit ID="DtFor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                            Font-Size="12px" Width="108px" ClientInstanceName="DtFor">
                                                            <dropdownbutton text="For">
                                                </dropdownbutton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                                        Report For</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlreporttype" runat="server" Width="200px" Font-Size="12px"
                                                            onchange="fn_ddlreportchange(this.value)">
                                                            <asp:ListItem Value="1">Client Wise [Default]</asp:ListItem>
                                                           <asp:ListItem Value="2">DP Account+Scrip+Client</asp:ListItem>
                                                            <asp:ListItem Value="3">Scrip+Client+DPAccount</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                                        DP Accounts</td>
                                                    <td>
                                                          <asp:DropDownList ID="ddlDPAc" runat="server" Width="191px" Font-Size="12px" onchange="ddldpac(this.value)">
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
                                        <td>
                                           <table>
                                          
                                           <tr id="Tr_Segment">
                                                <td class="gridcellleft">
                                                    <table class="tableClass"cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                Segment :</td>
                                                            <td>
                                                                <asp:RadioButton ID="rdbSegmentAll" runat="server"  GroupName="ef"
                                                                    onclick="fnSegment('a')" />
                                                                All
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdbSegmentSelected" runat="server" Checked="True" GroupName="ef" onclick="fnSegment('b')" />Selected
                                                            </td>
                                                            <td id="Td_CurrentSpecific"> [ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="Tr_GroupBy">
                                                <td class="gridcellleft">
                                                    <table class="tableClass"cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                Group By</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                                    <asp:ListItem Value="1">Branch</asp:ListItem>
                                                                    <asp:ListItem Value="2">Group</asp:ListItem>
                                                                    <asp:ListItem Value="3">Client</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td id="td_branch">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButton ID="rdBranchClientAll" runat="server" Checked="True" GroupName="a"
                                                                                onclick="fnBranchclient('a')" />
                                                                            All
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButton ID="rdBranchClientSelected" runat="server" GroupName="a" onclick="fnBranchclient('b')" />Selected
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
                                            <tr id="Tr_Instrument">
                                                <td class="gridcellleft">
                                                    <table class="tableClass"cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                Instrument :</td>
                                                            <td>
                                                                <asp:RadioButton ID="rdbInstrumentAll" runat="server" Checked="True" GroupName="ee"
                                                                    onclick="fnInstrument('a')" />
                                                                All
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdInstrumentSelected" runat="server" GroupName="ee" onclick="fnInstrument('b')" />Selected
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                               <tr id="Tr_PendingPurSaleValuemethod">
                                                   <td class="gridcellleft">
                                                       <table class="tableClass"cellpadding="1" cellspacing="1">
                                                           <tr>
                                                               <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                   Pendg. Pur/Sales Valuation Method
                                                                  
                                                               </td>
                                                               <td> <asp:RadioButton ID="rdbCloseprice" runat="server" Checked="True" GroupName="dd" />Close
                                                                   Price
                                                                  </td><td> <asp:RadioButton ID="rdbTradeprice" runat="server" GroupName="dd" />Trade Price</td>
                                                           </tr>
                                                       </table>
                                                   </td>
                                               </tr>
<tr >
                            <td class="gridcellleft">
                                <table class="tableClass"cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Security Type :
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
                        
                                               <tr id="Tr_ChkBoxOtherSelection">
                                                   <td class="gridcellleft">
                                                       <table class="tableClass"cellpadding="1" cellspacing="1">
                                                           <tr>
                                                               <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                   <asp:CheckBox ID="ChkPendingPurchase" runat="server" />
                                                                   Show Pending Purchases
                                                               </td>
                                                               
                                                               <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                   <asp:CheckBox ID="ChkPendingSales" runat="server" />
                                                                   Show Pending Sales
                                                               </td>
                                                               <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                   <asp:CheckBox ID="ChkDpDetails" runat="server" />
                                                                   Show Holding in DP Account
                                                               </td>
                                                           </tr>
                                                            <tr>
                                                               <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                   <asp:CheckBox ID="ChkLedgerBaln" runat="server" />
                                                                    Show Ledger Baln. 
                                                               </td>
                                                               <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                   <asp:CheckBox ID="ChkCashMarginDep" runat="server" />
                                                                  Cash Margin Deposit 
                                                               </td>
                                                                <td class="gridcellleft" bgcolor="#B7CEEC" id="td_printlogo">
                                                                    <asp:CheckBox ID="ChkLogoPrint" runat="server" Checked="true" />
                                                                    Do Not Print Logo</td>
                                                           </tr>
                                                       </table>
                                                   </td>
                                               </tr>
                                               <tr >
                                                   <td class="gridcellleft">
                                                       <table class="tableClass"cellpadding="1" cellspacing="1">
                                                           <tr>
                                                               <td class="gridcellleft" bgcolor="#B7CEEC" style="color:Red;" id="Td_ShoNegative">
                                                                   <asp:CheckBox ID="ChkNegative" runat="server" />
                                                                   Show Only Negative Holding Clients
                                                               </td>
                                                                <td class="gridcellleft" bgcolor="#B7CEEC"  id="Td_CorpAcType">
                                                                   <asp:CheckBox ID="ChkCorpAcType" runat="server" onclick="FnCorpAc(this)"/>
                                                                   Show Only Corporate-Action Due Shares
                                                               </td>
                                                           </tr>
                                                       </table>
                                                   </td>
                                               </tr>
                                               <tr id="Tr_DdlCorpAcType">
                                                   <td class="gridcellleft">
                                                       <table class="tableClass"cellpadding="1" cellspacing="1">
                                                           <tr>
                                                               <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                  Corp.Action Type
                                                               </td>
                                                                <td >
                                                                    <asp:DropDownList ID="ddlCorpActionType" runat="server" Width="80px" Font-Size="12px">
                                                                    </asp:DropDownList>
                                                               </td>
                                                           </tr>
                                                       </table>
                                                   </td>
                                               </tr>
                                               
                                              
                                            <tr>
                                                <td class="gridcellleft">
                                                    <table class="tableClass"cellpadding="1" cellspacing="1">
                                                    
                                                        <tr>
                                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                Generate Type :</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlGeneration" runat="server" Width="100px" Font-Size="12px"
                                                                    onchange="FnddlGeneration(this.value)">
                                                                    <asp:ListItem Value="1">Screen</asp:ListItem>
                                                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                                                     <asp:ListItem Value="3">EMail</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                         <td class="gridcellleft" bgcolor="#B7CEEC">                               
                                                            Applicable Haircut Rate
                                                         </td>
                                                         <td>
                                                            <asp:RadioButton ID="rdbVarmarginElm" runat="server" Checked="True" GroupName="elm"/>
                                                            Var Margin+ELM
                                                            <asp:RadioButton ID="rdbVarmargin" runat="server" GroupName="elm" />Var Margin
                                                        </td>
                                                    </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                             <tr id="Tr_RptTypeClientWise">
                                                   <td class="gridcellleft">
                                                       <table class="tableClass"cellpadding="1" cellspacing="1">
                                                           <tr>
                                                               <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                   Respective :</td>
                                                               <td>
                                                                   <asp:DropDownList ID="ddloptionformail" runat="server" Width="100px" Font-Size="12px"
                                                                       onchange="fnddloptionformail(this.value)">
                                                                       <asp:ListItem Value="1">Group/Branch</asp:ListItem>
                                                                       <asp:ListItem Value="2">User</asp:ListItem>
											     <asp:ListItem Value="3">Client Wise</asp:ListItem>
                                                                   </asp:DropDownList></td>
                                                           </tr>
                                                       </table>
                                                   </td>
                                               </tr>
                                               <tr id="Tr_RptTypeAccountwise">
                                                   <td class="gridcellleft">
                                                       <table class="tableClass"cellpadding="1" cellspacing="1">
                                                           <tr>
                                                               <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                   Respective :</td>
                                                               <td>
                                                                   <asp:DropDownList ID="ddlEmailuserwise" runat="server" Width="100px" Font-Size="12px">
                                                                       <asp:ListItem Value="2">User</asp:ListItem>
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
                                                               <td id="td_Export">
                                                                   <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export"
                                                                       Width="101px" OnClick="btnExcel_Click" /></td>
                                                               <td id="td_Email">
                                                                   <asp:Button ID="btnEmail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Email"
                                                                       Width="101px" OnClick="btnEmail_Click" /></td>
                                                           </tr>
                                                       </table>
                                                   </td>
                                               </tr>
                                               
                                           </table>
                                        </td>
                                        <td valign="top">
                                            <table cellpadding="1" cellspacing="1" id="showFilter">
                                                <tr>
                                                    <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                                        id="TdFilter">
                                                        <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                                        <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                            Enabled="false">
                                                            <asp:ListItem>ClientsSelction</asp:ListItem>
                                                            <asp:ListItem>Branch</asp:ListItem>
                                                            <asp:ListItem>Group</asp:ListItem>
                                                            <asp:ListItem>Scrips</asp:ListItem>
                                                             <asp:ListItem>Segment</asp:ListItem>
                                                             <asp:ListItem>MAILEMPLOYEE</asp:ListItem>
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
                    <asp:HiddenField ID="HiddenField_Scrips" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:HiddenField ID="hiddencount" runat="server" />
                    <asp:HiddenField ID="HiddenField_emmail" runat="server" />
                    <asp:DropDownList ID="cmb" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                    </asp:DropDownList>
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
                        <tr bordercolor="Blue" id="tr_prvnxt">
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
                                             <td align="right" style="display:none;">
                                              
                                             </td>
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
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
      
    </div>
  </asp:Content>

