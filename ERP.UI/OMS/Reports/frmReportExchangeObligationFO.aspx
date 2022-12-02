<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmReportExchangeObligationFO" Codebehind="frmReportExchangeObligationFO.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
         <script type="text/javascript" src="/assests/js/init.js"></script>
    <%--<script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>
    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>
    
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

         document.getElementById('ShowTable').style.display = 'none';
         document.getElementById('showFilter1').style.display = 'none';
         Hide('tr_export');
         Hide('tdgrdBrkgclient');
         Hide('showFilter');
         height();

     }
     function Hide(obj) {
         document.getElementById(obj).style.display = 'none'
     }
     function Show(obj) {
         document.getElementById(obj).style.display = 'inline'
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
     function fnunderlying(obj) {
         if (obj == "a") {
             Hide('showFilter');
             var btn = document.getElementById('btnhide');
             btn.click();
         }
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'UNDERLYING';
             Show('showFilter');
         }
         selecttion();
         height();
     }
     function line() {
         Show('tdgrdBrkgclient');
         Show('tr_export');
         Hide('tr_btn');
         Hide('tr_date');
         Hide('tr_under');
         height();
     }
     function NoRecord(obj) {
         Hide('tdgrdBrkgclient');
         Hide('tr_export');
         Show('tr_btn');
         Show('tr_date');
         Show('tr_under');

         if (obj == 1) {
             alert('No Record Found');
         }
         else if (obj == 2) {
             alert('Rates for this date does not exists');
         }

         height();
     }
     function Filter() {
         document.getElementById('ShowTable').style.display = 'none';
         document.getElementById('showFilter1').style.display = 'none';
         selecttion();
         Hide('tdgrdBrkgclient');
         Hide('tr_export');
         Show('tr_btn');
         Show('tr_date');
         Show('tr_under');
         height();
     }
     function DisableExchange(obj) {

         var gridview = document.getElementById('grdExchange');
         var rCount = gridview.rows.length;

         if (rCount < 10)
             rCount = '0' + rCount;

         if (obj == 'P') {

             document.getElementById("grdExchange_ctl" + rCount + "_FirstPage").style.display = 'none';
             document.getElementById("grdExchange_ctl" + rCount + "_PreviousPage").style.display = 'none';
             document.getElementById("grdExchange_ctl" + rCount + "_NextPage").style.display = 'inline';
             document.getElementById("grdExchange_ctl" + rCount + "_LastPage").style.display = 'inline';
         }
         else {
             document.getElementById("grdExchange_ctl" + rCount + "_NextPage").style.display = 'none';
             document.getElementById("grdExchange_ctl" + rCount + "_LastPage").style.display = 'none';
         }
     }

     function ChangeRowColorgrid(rowID, rowNumber) {

         var gridview = document.getElementById('grdExchange');
         var rCount = gridview.rows.length;
         var rowIndex = 1;
         var rowCount = 0;
         if (rCount == 18)
             rowCount = 15;
         else
             rowCount = rCount - 2;
         if (rowNumber > 15 && rCount < 18)
             rowCount = rCount - 3;
         for (rowIndex; rowIndex <= rowCount; rowIndex++) {
             var rowElement = gridview.rows[rowIndex];
             rowElement.style.backgroundColor = '#FFFFFF'
         }
         var color = document.getElementById(rowID).style.backgroundColor;
         if (color != '#ffe1ac') {
             oldColor = color;
         }
         if (color == '#ffe1ac') {
             document.getElementById(rowID).style.backgroundColor = oldColor;
         }
         else
             document.getElementById(rowID).style.backgroundColor = '#ffe1ac';

     }

     //     function rdbtnSelected(obj)
     //        {
     //                 Show('showFilter');
     //                 document.getElementById('cmbsearchOption').value='UNDERLYING';
     //                 document.getElementById('btn_show').disabled=true;
     //        }
     function rdbcheckchange() {

         var rdball = document.getElementById('rdbunderlyingAll');
         rdball.checked = true;
         var rdbselected = document.getElementById('rdbunderlyingSelected');
         rdbselected.checked = false;
         document.getElementById('btn_show').disabled = false;
         Hide('showFilter');

     }
     //    function rdbtnALL(obj)
     //        {
     //          Hide('showFilter');
     //          document.getElementById('btn_show').disabled=false;
     //          var btn = document.getElementById('btnhide');
     //          btn.click();
     //        }
     //        function showOptions(obj1,obj2,obj3)
     //         {
     //         document.getElementById('lstSlection').style.display='none';
     //         var obj4='Main';
     //            var cmb=document.getElementById('cmbinstrutype');
     //            ajax_showOptions(obj1,obj2,obj3,cmb.value,obj4);

     //         }


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
         document.getElementById('btn_show').disabled = false;
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
     function datechange() {
         var btn = document.getElementById('btnhide');
         btn.click();
     }
     function displaydate(obj) {
         document.getElementById('spanshow2').innerText = obj;
     }
     function selecttion() {
         var combo = document.getElementById('ddlExport');
         combo.value = 'Ex';
     }
     FieldName = 'SelectionList';

     //THIS IS FOR EMAIL

     function btnAddEmailtolist_click() {

         var cmb = document.getElementById('cmbsearch');

         var userid = document.getElementById('txtSelectID');
         if (userid.value != '') {
             var ids = document.getElementById('txtSelectID_hidden');
             var listBox = document.getElementById('SelectionList');
             var tLength = listBox.length;


             var no = new Option();
             no.value = ids.value;
             no.text = userid.value;
             listBox[tLength] = no;
             var recipient = document.getElementById('txtSelectID');
             recipient.value = '';
         }
         else
             alert('Please search name and then Add!')
         var s = document.getElementById('txtSelectID');
         s.focus();
         s.select();

     }


     function callAjax1(obj1, obj2, obj3) {
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

             document.getElementById('ShowTable').style.display = 'none';
             document.getElementById('showFilter1').style.display = 'inline';
         }
         else {
             alert("Please select email from list.")
         }

         var i;
         for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
             listBoxSubs.remove(i);
         }

         height();
     }

     function btnRemoveEmailFromlist_click() {

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

     function Sendmail() {
         document.getElementById('ShowTable').style.display = 'inline';
         document.getElementById('showFilter1').style.display = 'none';
         height();
     }
     function ForFilterOff() {
         selecttion();
         document.getElementById('ShowTable').style.display = 'none';
         document.getElementById('showFilter1').style.display = 'none';
         Show('tdgrdBrkgclient');
         Show('tr_export');
         Hide('tr_btn');
         Hide('tr_date');
         Hide('tr_under');
         height();
     }
     function MailsendT() {
         alert("Mail Sent Successfully");
     }
     function MailsendF() {
         alert("Error on sending!Try again..");
     }
     function MailsendFT() {
         alert("Email id could not found!Try again...");
     }

     function keyVal(obj) {
         document.getElementById('lstSlection').style.display = 'inline';

     }
     FieldName = 'lstSlection';
    </script> 
     <script type="text/ecmascript">
         function ReceiveServerData(rValue) {

             var btn = document.getElementById('btnhide');
             btn.click();
             var j = rValue.split('~');
             if (j[0] == 'UNDERLYING') {
                 document.getElementById('HiddenField_Product').value = j[1];
             }
         }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
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



                     //if (postBackElement.id == 'ctl00_ContentPlaceHolder3_btnShow') 

                     $get('UpdateProgress1').style.display = 'block';
                     //document.getElementById('btn_show').disabled=true;

                 }



                 function EndRequest(sender, args) {

                     // if (postBackElement.id == 'ctl00_ContentPlaceHolder3_btnShow') 

                     $get('UpdateProgress1').style.display = 'none';
                     //document.getElementById('btn_show').disabled=false;




                 }
   </script>
<table class="TableMain100"> <tr>
            <td class="EHEADER" colspan="0" style="text-align:center; height: 22px;">
                <strong><span id="SpanHeader" style="color: #000099">Exchange Obligation</span></strong>
            </td>
            <td class="EHEADER" width="25%" id="tr_export" style="height: 22px">
                                    <a href="javascript:void(0);" onclick="Filter();"><span
                        style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight:bold ">Filter</span></a>
                &nbsp; &nbsp;<asp:DropDownList ID="ddlExport" Font-Size="Smaller" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged" >
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                        </asp:DropDownList> || <a href="javascript:void(0);" onclick="Sendmail();"><span style="color: Blue;
                            text-decoration: underline; font-size: 8pt; font-weight: bold">Send Email</span></a>

             </td>
             
        </tr></table>
        <table>
            <tr id="tr_date">
                <td valign="top">
                    <table>
                        <tr >
                            <td valign="top">
                                For :</td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtFor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfor">
                                    <dropdownbutton text="For">
                                        </dropdownbutton>
                                <ClientSideEvents valuechanged="function(s, e) {datechange();}" />
                                </dxe:ASPxDateEdit></td>
                        </tr>
                    </table>
                </td>
                <td>
                   
                </td>
                <td valign="top">
                    Instrument Type :</td>
                <td valign="top">
                   
                            <asp:DropDownList ID="cmbinstrutype" runat="server" Font-Size="12px" AutoPostBack="True"
                                OnSelectedIndexChanged="cmbinstrutype_SelectedIndexChanged">
                                <asp:ListItem Value="0">ALL</asp:ListItem>
                                <asp:ListItem Value="1">FUTSTK &amp; OPTSTK</asp:ListItem>
                                <asp:ListItem Value="2">FUTSTK</asp:ListItem>
                                <asp:ListItem Value="3">OPTSTK</asp:ListItem>
                                <asp:ListItem Value="4">FUTIDX &amp; OPTIDX</asp:ListItem>
                                <asp:ListItem Value="5">FUTIDX</asp:ListItem>
                                <asp:ListItem Value="6">OPTIDX</asp:ListItem>
                            </asp:DropDownList>
                      
                    </td>
                <td valign="top" align="right">
                </td>
            </tr>
            <tr id="tr_under">
                <td valign="top">
                <table>  <tr>
                                <td  align="left">
                                    Underlying:
                                </td>
                                <td style="text-align: left;" colspan="2" valign="top">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <asp:RadioButton ID="rdbunderlyingAll" runat="server"  Checked="true" GroupName="d"  onclick="fnunderlying('a')" />
                                            </td>
                                            <td valign="top">
                                                All
                                            </td>
                                            <td valign="top">
                                                <asp:RadioButton ID="rdbunderlyingSelected" runat="server" GroupName="d"  onclick="fnunderlying('b')" />
                                            </td>
                                            <td valign="top">
                                                   Selected
                                            </td>
                                           
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                    <tr>
                        <td align="left" valign="top">
                            Expiry :</td>
                        <td colspan="2" style="text-align: left" valign="top">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="cmbexpiry" runat="server" Font-Size="12px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbinstrutype" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                    
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                </td>
                <td valign="top" colspan="4">
                
                        <table width="100%" id="showFilter">
                            <tr>
                                <td style="text-align: right; vertical-align: top; height: 134px;">
                                    <table cellpadding="0" cellspacing="0">
                                    <tr> <td class="gridcellleft" style="vertical-align: top; text-align: right" id="TdFilter">
                                  <span id="spanall">
                                        <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="ajax_showOptions(this,'Searchproductandeffectuntil',event,'Product')"></asp:TextBox></span>
                                    
                                    <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                        Enabled="false">
                                        <asp:ListItem>UNDERLYING</asp:ListItem>
                              
                                    </asp:DropDownList>
                                    <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                        style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                            style="color: #009900; font-size: 8pt;"> </span>
                                </td></tr>
                                        <tr>
                                            <td class="gridcellleft" style="vertical-align: top; text-align: right">
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
                            <tr style="display: none">
                                <td style="height: 23px" align="right">
                                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox></td>
                            </tr>
                        </table>
                </td>
              
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td valign="top">
                </td>
                <td valign="top" style="display: none;">
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                </td>
                <td valign="top">
                </td>
                <td align="right" valign="top">
                </td>
            </tr>
            <tr>
                <td colspan="5">
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
                                                                        <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
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
            
            <tr id="tr_btn">
                <td>
                    <asp:Button ID="btn_show" runat="server" CssClass="btnUpdate" Height="20px" OnClick="btn_show_Click"
                        Text="Show" Width="101px" OnClientClick="selecttion()"/></td>
                <td valign="top">
                </td>
                
               
                <td valign="top"> 
                </td>
                <td valign="top">
                </td>
                <td align="right" valign="top">
                </td>
            </tr>
            <tr>
                            <td colspan="5">
                                <table width="100%" id="ShowTable">
                               <tr><td width="70px" style="text-align:left;">Type:</td>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: left" id="Td1">
                                            <span id="span1">
                                                <asp:DropDownList ID="cmbsearch" runat="server" Width="150px" Font-Size="12px">
                                                </asp:DropDownList>
                                        </td>
                                    </tr>
                                    
                                     <tr><td width="70px" style="text-align:left;">Select User:</td>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter1">
                                            <span id="spanal2">
                                                <asp:TextBox ID="txtSelectID" runat="server" Font-Size="12px" Width="285px"></asp:TextBox></span>
                                            <a id="A3" href="javascript:void(0);" onclick="btnAddEmailtolist_click()"><span
                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                    style="color: #009900; font-size: 8pt;"> </span>
                                        </td>
                                    </tr>
                                                                       
                                    <tr><td width="70px" style="text-align:left;"></td>
                                        <td style="text-align: left; vertical-align: top; height: 134px;">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        &nbsp;&nbsp;<asp:ListBox ID="SelectionList" runat="server" Font-Size="12px" Height="90px" Width="290px">
                                                        </asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <a id="AA2" href="javascript:void(0);" onclick="clientselection()"><span style="color: #000099;
                                                                        text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                                    <a id="AA1" href="javascript:void(0);" onclick="btnRemoveEmailFromlist_click()">
                                                                        <span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="display: none"><td width="70px" style="text-align:left;"></td>
                                        <td style="height: 23px">
                                            <asp:TextBox ID="txtSelectID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                            <asp:HiddenField ID="HiddenField_Product" runat="server" />
                                        </td>
                                    </tr>
                                   
                                </table>
                            </td>
                        </tr>
                         <tr>
                            <td colspan="5">
                                <table width="100%" id="showFilter1">
                                    <tr>
                                        <td>
                                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btnUpdate"
                                                Text="Send" OnClientClick="selecttion()"/>
                                        </td>
                        </tr>
                    </table>
                    </td> </tr>
            <tr>
                <td colspan="5" id="tdgrdBrkgclient">
                    <span id="spanshow1" style="color: Blue; font-weight: bold">Exchange Obligation Report
                        For :</span>&nbsp;&nbsp;<span id="spanshow2"></span>
                        <br /><br />
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                           
                                <asp:GridView ID="grdExchange" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                    ShowFooter="True" AllowSorting="true" AutoGenerateColumns="False" BorderStyle="Solid"
                                    BorderWidth="2px"  AllowPaging="True" PageSize="15" ForeColor="#0000C0" OnRowCreated="grdExchange_RowCreated" OnRowDataBound="grdExchange_RowDataBound">
                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Instrument">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False" ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lbltabSymbol" runat="server" Text='<%# Eval("tabSymbol")%>'  CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                               <FooterTemplate >
                                                    Total
                                             </FooterTemplate>
                                               <FooterStyle Wrap="false" HorizontalAlign="left" VerticalAlign="Top" Font-Bold="true" ForeColor="white"  />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Expiry Date">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False" ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCommodity_ExpiryDate" runat="server" Text='<%# Eval("Equity_EffectUntil")%>'  CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                              
                                        </asp:TemplateField>
                                         
                                        <asp:TemplateField HeaderText="B/F Qty">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblLOTSRESULT_B" runat="server" Text='<%# Eval("LOTSRESULT_B")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                             
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Open Price">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSettlementPrice" runat="server" Text='<%# Eval("SettlementPrice")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                        
                                           <asp:TemplateField HeaderText="B/F Value">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBFVALUE" runat="server" Text='<%# Eval("BFVALUE")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                               <FooterTemplate>
                                                    <asp:Label ID="lblBFVALUE_Sum" runat="server" Text='<%# Eval("BFVALUE_Sum")%>'    ForeColor="white" ></asp:Label>
                                                  

                                             </FooterTemplate>
                                             <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="false"/>
                                               </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Day Buy">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDAYBUY" runat="server" Text='<%# Eval("DAYBUY")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                   
                                           <asp:TemplateField HeaderText="Buy Value">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBUYVALUE" runat="server" Text='<%# Eval("BUYVALUE")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                                    <asp:Label ID="lblBUYVALUE_Sum" runat="server" Text='<%# Eval("BUYVALUE_Sum")%>'    ForeColor="white" ></asp:Label>
                                                  

                                             </FooterTemplate>
                                             <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="false"/>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Day Sell">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDAYSELL" runat="server" Text='<%# Eval("DAYSELL")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                              
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Sell Value">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSELLVALUEt" runat="server" Text='<%# Eval("SELLVALUE")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                           <FooterTemplate>
                                                    <asp:Label ID="lblSELLVALUE_Sum" runat="server" Text='<%# Eval("SELLVALUE_Sum")%>'    ForeColor="white" ></asp:Label>
                                                  

                                             </FooterTemplate>
                                             <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="false"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="C/F Qty">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCFQTY_I" runat="server" Text='<%# Eval("CFQTY_I")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sett Price">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSETTPRICE" runat="server"  Text='<%# Eval("SETTPRICE")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                             
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="C/F Value" >
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCFVALUE" runat="server" Text='<%# Eval("CFVALUE")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                                    <asp:Label ID="lblCFVALUE_Sum" runat="server" Text='<%# Eval("CFVALUE_Sum")%>'    ForeColor="white" ></asp:Label>
                                                  

                                             </FooterTemplate>
                                             <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="false"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Premium">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRM" runat="server" Text='<%# Eval("PRM")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                           <FooterTemplate>
                                                <asp:Label ID="lblPRM_Sum" runat="server" Text='<%# Eval("PRM_Sum")%>' ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MTM">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMTM" runat="server" Text='<%# Eval("MTM")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                          <FooterTemplate>
                                                <asp:Label ID="lblMTM_Sum" runat="server" Text='<%# Eval("MTM_Sum")%>' ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="false" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Future FinSett">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblFutFin" runat="server" Text='<%# Eval("FutFin")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                    <asp:Label ID="lblFutFin_Sum" runat="server" Text='<%# Eval("FutFin_Sum")%>'    ForeColor="white" ></asp:Label>
                                                  

                                             </FooterTemplate>
                                             <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="false"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ASN/EXC Amount">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblASNEXCSETT" runat="server" Text='<%# Eval("ExcAsn")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                    <asp:Label ID="lblASNEXCSETT_Sum" runat="server" ForeColor="white" ></asp:Label>
                                                  

                                             </FooterTemplate>
                                             <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="false"/>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Net Obligation">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right" ></ItemStyle>
                                            <HeaderStyle HorizontalAlign="center" Font-Bold="False"  ></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblNETOBLIGATION" runat="server" Text='<%# Eval("NETOBLIGATION")%>' CssClass="gridstyleheight1" ></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                    <asp:Label ID="lblNETOBLIGATION_Sum" runat="server" Text='<%# Eval("NETOBLIGATION_Sum")%>'    ForeColor="white" ></asp:Label>
                                                  

                                             </FooterTemplate>
                                             <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="false"/>
                                        </asp:TemplateField>
                                      
                                    </Columns>
                                    <PagerTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="10">
                                                    <asp:LinkButton ID="FirstPage" runat="server" Font-Bold="true" CommandName="First"
                                                        OnCommand="NavigationLink_Click" Text="[First Page]" OnClientClick="selecttion()"> </asp:LinkButton>
                                                    <asp:LinkButton ID="PreviousPage" runat="server" Font-Bold="true" CommandName="Prev"
                                                        OnCommand="NavigationLink_Click" Text="[Previous Page]" OnClientClick="selecttion()">  </asp:LinkButton>
                                                    <asp:LinkButton ID="NextPage" runat="server" Font-Bold="true" CommandName="Next"
                                                        OnCommand="NavigationLink_Click" Text="[Next Page]" OnClientClick="selecttion()">
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="LastPage" runat="server" Font-Bold="true" CommandName="Last"
                                                        OnCommand="NavigationLink_Click" Text="[Last Page]" OnClientClick="selecttion()">
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </PagerTemplate>
                                    <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                        BorderWidth="1px"></RowStyle>
                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                        Font-Bold="False"></HeaderStyle>
                                </asp:GridView>
                                <asp:HiddenField ID="CurrentPage" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="TotalPages" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="TotalClient" runat="server" />
                            
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click" />
                               
                            </Triggers>
                        </asp:UpdatePanel>
                </td>
                
            </tr>
        </table>
    </div>
</asp:Content>
