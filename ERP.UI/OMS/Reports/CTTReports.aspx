<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_CTTReports" Codebehind="CTTReports.aspx.cs" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
	

    #dhtmltooltip{
    position: absolute;
    left: -300px;
    width: 150px;
    border: 1px solid black;
    padding: 2px;
    background-color: lightyellow;
    visibility: hidden;
    z-index: 100;
    /*Remove below line to remove shadow. Below line should always appear last within this CSS*/
    filter: progid:DXImageTransform.Microsoft.Shadow(color=gray,direction=135);
    }

    #dhtmlpointer{
    position:absolute;
    left: -300px;
    z-index: 101;
    visibility: hidden;
    }




	</style>
 <script language="javascript" type="text/javascript">


     function Page_Load()///Call Into Page Load
     {
         Hide('showFilter');
         Hide('td_filter');
         document.getElementById('hiddencount').value = 0;
         fn_ReportView('1');
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
             if (document.getElementById('ddlGroup').value == "0")//////////////Group By  selected are branch
             {
                 if (document.getElementById('ddlGroup').value == "0") {
                     if (document.getElementById('rdbranchAll').checked == true) {
                         cmbVal = 'ClientsBranch' + '~' + 'ALL';
                     }
                     else {
                         cmbVal = 'ClientsBranch' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Branch').value;
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
         else if (document.getElementById('cmbsearchOption').value == "Company") {
             ajax_showOptions(objID, "Company", objEvent);
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
     function fnAsset(obj) {
         if (obj == "a")
             Hide('showFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Product';
             Show('showFilter');
         }
         selecttion();
     }
     function fnSegment(obj) {
         if (obj == "a")
             Hide('showFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Segment';
             Show('showFilter');
         }
         selecttion();
     }
     function fnBranch(obj) {
         if (obj == "a")
             Hide('showFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Branch';
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
     function fnCompany(obj) {
         if (obj == "a")
             Hide('showFilter');
         else {
             Show('showFilter');
             document.getElementById('cmbsearchOption').value = 'Company';
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

     function RecordDisplay() {
         Hide('showFilter');
         Show('td_filter');
         Hide('tab1');
         document.getElementById('hiddencount').value = 0;
         Show('displayAll');
         selecttion();
         height();

     }
     function fnNoRecord(obj) {
         Hide('showFilter');
         Hide('td_filter');
         Show('tab1');
         document.getElementById('hiddencount').value = 0;
         Hide('displayAll');
         if (obj == '1')
             alert('No Record Found!!');
         selecttion();
         fn_ReportView(document.getElementById('DLLReportView').value)
         height();
     }
     function fn_ReportView(obj) {

         if (obj == '3' || obj == '4')///Client + Instrument or Intrument Wise
         {
             Show('tr_ConsolidateSegmentScrip');
             Hide('Td_ConsolidatedAcrossSegment');
             if (obj == '4')///Intrument Wise
             {
                 Hide('Td_Consolidate');
                 document.getElementById('ChkConsolidate').checked = false;
             }
             else
                 Show('tr_Consolidate');
         }
         else if (obj == '5')///Instrument + Client
         {
             Show('tr_ConsolidateSegmentScrip');
             Show('Td_Consolidate');
             Hide('Td_ConsolidatedAcrossSegment');
         }
         else if (obj == '8' || obj == '9')///Across Segment
         {
             Hide('tr_ConsolidateSegmentScrip');
             Show('Td_ConsolidatedAcrossSegment');
             Hide('Td_Consolidate');
         }
         else if (obj == '10')///Across Segment
         {
             Hide('tr_ConsolidateSegmentScrip');
             Hide('Td_ConsolidatedAcrossSegment');
             Hide('Td_Consolidate');
         }
         else {
             Hide('tr_ConsolidateSegmentScrip');
             Hide('Td_ConsolidatedAcrossSegment');
             Show('Td_Consolidate');
             document.getElementById('ChkConsolidateSegmentScrip').checked = false;
         }

         if (document.getElementById('DLLCalculation').value == "3")
             Show('Td_ShowAll');
         else
             Hide('Td_ShowAll');

         FnddlGeneration(document.getElementById('ddlGeneration').value);
         document.getElementById('ChkShowALL').checked = false;

     }
     function fn_CalculationType(obj) {
         if (obj == '3')///Reconciliation Statement (Prov Vs Exch)
         {
             Show('Td_Consolidate');
             Show('Td_ShowAll');
             Hide('Tr_Asset');
         }
         else {
             Show('Tr_Asset');
             fn_ReportView(document.getElementById('DLLReportView').value);
         }

         height();
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
              btn.click();
              document.getElementById('HiddenField_Group').value = j[1];
          }
          if (j[0] == 'Clients') {
              document.getElementById('HiddenField_Client').value = j[1];
          }
          if (j[0] == 'Product') {
              document.getElementById('HiddenField_Product').value = j[1];
          }
          if (j[0] == 'Segment') {
              document.getElementById('HiddenField_Segment').value = j[1];
          }
          if (j[0] == 'Company') {
              document.getElementById('HiddenField_Company').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">STT Reports</span></strong></td>

              <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="fnNoRecord(2);"><span style="color: Blue; text-decoration: underline;
                        font-size: 8pt; font-weight: bold">Filter</span></a>
                 <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" >
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
                                            For A Period
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
                                            Calculation Type :</td> 
                                        <td>
                                            <asp:DropDownList ID="DLLCalculation" runat="server" Width="220px" Font-Size="12px"  AutoPostBack="true" OnSelectedIndexChanged="DLLCalculation_SelectedIndexChanged">
                                                <asp:ListItem Value="1">Collected From Clients (Prov)</asp:ListItem>
                                                <asp:ListItem Value="2">Paid To Exchange (Exch)</asp:ListItem>
                                                <asp:ListItem Value="3">Reconciliation Statement (Prov Vs Exch)</asp:ListItem>
                                            </asp:DropDownList>
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
                                            Group By</td>
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
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Clients :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="fnClients('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="fnClients('b')" />
                                            Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_Asset">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft"  bgcolor="#B7CEEC">
                                            Asset :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbAssetAll" runat="server" Checked="True" GroupName="e" onclick="fnAsset('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdAssetSelected" runat="server" GroupName="e" onclick="fnAsset('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                         <tr id="Tr_Company">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                   <tr>
                                <td class="gridcellleft"  bgcolor="#B7CEEC">
                                    Company :</td>
                                <td>
                                    <asp:RadioButton ID="RdbAllCompany" runat="server"  GroupName="dd" onclick="fnCompany('a')" />
                                    All
                                     <asp:RadioButton ID="RdbCurrentCompany" runat="server" Checked="True" GroupName="dd" onclick="fnCompany('a')" />
                                    Current
                                    <asp:RadioButton ID="RdbSelectedCompany" runat="server" GroupName="dd" onclick="fnCompany('b')" />Selected
                                </td>
                            </tr>
                                </table>
                            </td>
                            </tr>
                         <tr >
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft"  bgcolor="#B7CEEC">
                                            Segment :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentAll" runat="server" Checked="True" GroupName="d" onclick="fnSegment('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdSegmentSelected" runat="server" GroupName="d" onclick="fnSegment('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                      
                         <tr id="Tr_ReportView">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Report View :</td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="DLLReportView" runat="server" Width="220px" Font-Size="12px"
                                                        onchange="fn_ReportView(this.value)">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger  ControlID="DLLCalculation" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr >
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC" id="Td_Consolidate">
                                            <asp:CheckBox ID="ChkConsolidate" runat="server" />
                                            Consolidate Client Group/Branch Wise</td>
                                             <td class="gridcellleft" bgcolor="#B7CEEC" id="Td_ConsolidatedAcrossSegment">
                                            <asp:CheckBox ID="ChkCOnsolidatedAcrossSegment" runat="server" />
                                            Show Group/Branch BreakUp</td>
                                        <td id="Td_ShowAll">
                                            <table>
                                                <tr>
                                                    <td>
                                                        [
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkShowALL" runat="server" />
                                                    </td>
                                                    <td>
                                                        ]</td>
                                                    <td>
                                                        Show ALL</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_ConsolidateSegmentScrip">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkConsolidateSegmentScrip" runat="server" />
                                            Consolidate Segment [Cash/FO] Wise </td>
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
                                            <asp:Button ID="btnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                Width="101px" OnClick="btnScreen_Click" OnClientClick="selecttion()"/>
                                        </td>
                                         <td id="td_Export">
                                              <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                                Width="101px" OnClick="btnExcel_Click" OnClientClick="selecttion()" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                     <table  border="10" cellpadding="1" cellspacing="1" id="showFilter">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                            id="TdFilter">
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
                                                <asp:ListItem>Product</asp:ListItem>
                                                 <asp:ListItem>Company</asp:ListItem>
                                                 <asp:ListItem>Segment</asp:ListItem>
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
                    <asp:HiddenField ID="HiddenField_Company" runat="server" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Product" runat="server" />
                     <asp:HiddenField ID="HiddenField_Segment" runat="server" />
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
                             <tr style="display: none;">
                                 <td>
                                     <asp:HiddenField ID="hiddencount" runat="server" />
                                      <asp:Button ID="Button1" runat="server" Text="btnhide" OnClick="btnhide_Click" />
                                 </td>
                             </tr>
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
                              <tr >
                                 <td>
                                     <div id="DivFooter" runat="server">
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