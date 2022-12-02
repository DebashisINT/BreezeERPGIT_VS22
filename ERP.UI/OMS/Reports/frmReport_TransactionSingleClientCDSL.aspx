<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmReport_TransactionSingleClientCDSL" Codebehind="frmReport_TransactionSingleClientCDSL.aspx.cs" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" /> 
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script> 
    <script type="text/javascript" src="/assests/js/init.js"></script> 
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <script language="javascript" type="text/javascript">
      function ForFilterOff() {
          hide('filter');
          show('btnfilter');

          //            document.getElementById("TrAll").style.display='none';
          //            document.getElementById("TdAll1").style.display='none';
          //            document.getElementById("TrBtn").style.display='none';
          //            document.getElementById('spanBtn').style.display='none';
          height();
      }
      function MailsendT() {
          alert("Mail Sent Successfully");
      }
      function MailsendF() {
          alert("Error on sending!Try again..");
      }
      function SignOff() {
          window.parent.SignOff();
      }
      function height() {
          if (document.body.scrollHeight >= 500)
              window.frameElement.height = document.body.scrollHeight;
          else
              window.frameElement.height = '500px';
          window.frameElement.Width = document.body.scrollWidth;
      }
      function PageLoad() {
          FieldName = 'SelectionList';
          document.getElementById('txtName_hidden').style.display = "none";
          document.getElementById('txtISIN_hidden').style.display = "none";
          document.getElementById('txtSettlement_hidden').style.display = "none";
          ShowEmployeeFilterForm('A');
          ShowISINFilterForm('A');
          ShowSettlementFilterForm('A');
      }

      function ShowISINFilterForm(obj) {

          if (obj == 'A') {
              hide('tdisinValue');
              hide('tdisin');
              document.getElementById('txtISIN_hidden').style.display = "none";
              document.getElementById('txtISIN').value = "";
              document.getElementById('txtISIN_hidden').value = "";
          }
          if (obj == 'S') {
              show('tdisinValue');
              show('tdisin');
              document.getElementById('txtISIN_hidden').style.display = "none";
          }
      }
      function ShowEmployeeFilterForm(obj) {
          if (obj == 'A') {
              hide('txttdname');
              hide('tdname');
              document.getElementById('txtName_hidden').style.display = "none";
              document.getElementById('txtName').value = "";
              document.getElementById('txtName_hidden').value = "";
          }
          if (obj == 'S') {
              show('txttdname');
              show('tdname');
              document.getElementById('txtName_hidden').style.display = "none";
          }
      }
      function NoOfRows(obj) {
          //alert(obj);
          Noofrows = obj;
          document.getElementById('txtName_hidden').style.display = "none";
      }
      function show(obj1) {
          //alert(obj1);
          document.getElementById(obj1).style.display = 'inline';
      }
      function hide(obj1) {
          document.getElementById(obj1).style.display = 'none';
      }
      FieldName = 'SelectionList'
      function CallAjax(obj1, obj2, obj3) {
          var cmbType = 'All';
          var Client = document.getElementById('txtName_hidden');
          var isin = document.getElementById('txtISIN_hidden');
          var obj4 = e1.GetDate() + '~' + e2.GetDate() + '~' + cmbType + '~' + Client.value + '~' + isin.value;
          ajax_showOptions(obj1, obj2, obj3, obj4);
      }
      function OnValueChanged(s, e) {
          Page_ClientValidate(""); // undocumented
      }
      function validateDates(s, e) {
          var startDate = e1.GetDate();
          var endDate = e2.GetDate();
          e.IsValid = startDate == null || endDate == null || startDate <= endDate;

      }

      function hidesearch() {
          hide('t1');
          hide('t2');
          hide('t3');
          show('f');
      }
      function ShowFilter() {

          show('t1');
          show('t2');
          show('t3');
          hide('f');
      }

      function OnClientTypeChanged(s, e) {
          document.getElementById('txtSettlement_hidden').value = "";
          document.getElementById('txtSettlement').value = "";
          var item = s.GetSelectedItem();
          if (item.text == 'Clearing Member' || item.text == 'All') {
              show('tdSettlementLabel');
              show('tdrbSettlment');
              ShowSettlementFilterForm('A');
          }
          else {
              hide('tdSettlementLabel');
              hide('tdrbSettlment');
              hide('tdtxtSettlement');

          }
      }

      function ShowSettlementFilterForm(obj) {
          document.getElementById('txtSettlement_hidden').value = "";
          show('tdSettlementLabel');
          if (obj == 'A') {
              hide('tdtxtSettlement');
              document.getElementById('txtSettlement_hidden').style.display = "none";
              document.getElementById('txtSettlement').value = "";
          }
          if (obj == 'S') {
              show('tdtxtSettlement');
              document.getElementById('txtSettlement_hidden').style.display = "none";
          }
      }

      function ShowHideButton(obj) {

          if (obj == 'Screen') {
              hide('prnt');
              show('show');
          }
          else {
              show('prnt');
              hide('show');
          }

      }
      function fullScreen() {
          if (this.name != 'fullscreen') {
              window.open(location.href, 'fullscreen', 'fullscreen,scrollbars')
          }
      }



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
          document.getElementById('SelectionList').style.display = 'none';
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
              //document.getElementById('txttdname').style.display='none';
              hide('txttdname');
              height();

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
      function selecttion() {
          var combo = document.getElementById('ddlExport');
          combo.value = 'Ex';
      }

    </script>
    <table class="TableMain100">
        <tr>
            <td class="EHEADER" colspan="0" align="center"  >
                <strong><span style="color: #000099;" >CDSL Transction Report  
                </span></strong> 
                <asp:ScriptManager runat="server" id="ScriptManager1" AsyncPostBackTimeout ="360000">
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
                        $get('UpdateProgress1').style.display = 'block';

                    }
                    function EndRequest(sender, args) {
                        $get('UpdateProgress1').style.display = 'none';
                    }
                </script>
                            
                </td>
                <td class="EHEADER" width="25%" id="f">
                                        <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged1" >
                                                    <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                                                    <asp:ListItem Value="E">Excel</asp:ListItem>
                                                    <asp:ListItem Value="P">PDF</asp:ListItem>
                                                </asp:DropDownList>||
                                    <a href="javascript:void(0);" onclick="ShowFilter();"><span
                        style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight:bold ">Filter</span></a>
                         || <asp:LinkButton ID="btnEmail" runat="server"  Font-Bold="True" Font-Underline="True" ForeColor="Blue" OnClick="btnEmail_Click">SendEmail</asp:LinkButton>

             </td>
        </tr>
    </table>
    
    <table border="0" class="TableMain" cellpadding="0" cellspacing="0" style="width: 100%;
        padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
        <tr>
            <td style="vertical-align: top; text-align: left;">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="gridcellleft">
                    <tr id="t1" style="display: none;">
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td>
                                        <span class="Ecoheadtxt" style="color: blue"><strong>From:</strong></span></td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="txtstartDate" runat="server" ClientInstanceName="e1" 
                                             
                                            Width="130px" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                                            AllowNull="False" Height="25px">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents ValueChanged="OnValueChanged" />
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td>
                                        <span class="Ecoheadtxt" style="color: blue"><strong>To:</strong></span></td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="txtendDate" runat="server" ClientInstanceName="e2" 
                                             
                                            Width="130px" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                                            AllowNull="False" Height="25px">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents ValueChanged="OnValueChanged" />
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="">
                           
                        </td>
                        <td style="">
                           </td>
                        <td style="">
                           
                        </td>
                        
                    </tr>
                    
                    <tr id="t2" style="display: none;">
                        <td colspan="2" style="text-align: center">
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="validateDates"
                                ErrorMessage="Please Enter Valid Date Range."></asp:CustomValidator></td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr id="txttdname">
                    <td style="">
                            <table>
                                <tr>
                                    <td id="tdname" valign="top">
                                        <span class="Ecoheadtxt" style="color: blue"><strong>Name:</strong></span>
                                    </td>
                                   <%-- <td id="txtname">
                                        <asp:TextBox ID="txtName_hidden" runat="server" Width="14px"></asp:TextBox>
                                        <asp:TextBox ID="txtName" runat="server" Width="220px" Font-Size="11px" Height="20px"></asp:TextBox>
                                    </td>--%>
                                                          <td>
                                                            <table width="100%">
                                                                  <tr>
                                                                    <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter1">
                                                                        <span id="spanal2">
                                                                            <asp:TextBox ID="txtName" runat="server" Font-Size="12px" Width="285px"></asp:TextBox></span>
                                                                            <span id="span1" visible="false">
                                                                            <asp:DropDownList ID="cmbsearch" runat="server" Width="70px" Font-Size="12px">
                                                                                <asp:ListItem>Clients</asp:ListItem>
                                                                            </asp:DropDownList></span>
                                                                        <a id="A3" href="javascript:void(0);" onclick="btnAddEmailtolist_click()"><span style="color: #009900;
                                                                            text-decoration: underline; font-size: 8pt;">Add to List</span></a><span style="color: #009900;
                                                                                font-size: 8pt;"> </span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left; vertical-align: top; height: 134px;">
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;&nbsp;<asp:ListBox ID="SelectionList" runat="server" Font-Size="12px" Height="90px"
                                                                                        Width="290px"></asp:ListBox>
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
                                                                    <td width="70px" style="text-align: left;">
                                                                    </td>
                                                                    <td style="height: 23px">
                                                                        <asp:TextBox ID="txtName_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                                        <asp:TextBox ID="dtFrom_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                                        <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                              
                                                            </table>
                                                        </td>
                                                        
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="t3" style="display: none;">
                        <td colspan="6">
                            <table>
                                <tr>
                                    <td>
                                        <span class="Ecoheadtxt" style="color: blue"><strong>ISIN:</strong></span>
                                    </td>
                                    <td>
                                        <dxe:ASPxRadioButtonList ID="rbISIN" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                            RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px">
                                            <Items>
                                                <dxe:ListEditItem Text="All" Value="A" />
                                                <dxe:ListEditItem Text="Specific" Value="S" />
                                            </Items>
                                            <ClientSideEvents ValueChanged="function(s, e) {ShowISINFilterForm(s.GetValue());}" />
                                            <Border BorderWidth="0px" />
                                        </dxe:ASPxRadioButtonList>
                                    </td>
                                    <td id="tdisin">
                                        <span class="Ecoheadtxt" style="color: blue"><strong>Value:</strong></span>
                                    </td>
                                    <td id="tdisinValue">
                                        <asp:TextBox ID="txtISIN_hidden" runat="server" Width="14px"></asp:TextBox>
                                        <asp:TextBox ID="txtISIN" runat="server" Width="190px" Font-Size="11px" Height="20px"></asp:TextBox>
                                    </td>
                                    <td id="tdSettlementLabel">
                                        <span class="Ecoheadtxt" style="color: blue"><strong>Settlement No.:</strong></span>
                                    </td>
                                    <td id="tdrbSettlment">
                                        <dxe:ASPxRadioButtonList ID="rbSettlement" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                            RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px">
                                            <Items>
                                                <dxe:ListEditItem Text="All" Value="A" />
                                                <dxe:ListEditItem Text="Specific" Value="S" />
                                            </Items>
                                            <ClientSideEvents ValueChanged="function(s, e) {ShowSettlementFilterForm(s.GetValue());}" />
                                            <Border BorderWidth="0px" />
                                        </dxe:ASPxRadioButtonList>
                                    </td>
                                    <td id="tdtxtSettlement">
                                        <asp:TextBox ID="txtSettlement_hidden" runat="server" Width="14px"></asp:TextBox>
                                        <asp:TextBox ID="txtSettlement" runat="server" Width="190px" Font-Size="11px" Height="20px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span class="Ecoheadtxt" style="color: blue"><strong>Report Type:</strong></span></td>
                                    <td>
                                        <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" 
                                            CssPostfix="BlackGlass" Height="2px" RepeatDirection="Horizontal" SelectedIndex="0"
                                            TextSpacing="3px">
                                            <Paddings PaddingBottom="0px"></Paddings>
                                            <ValidationSettings ErrorText="Error has occurred">
                                                <ErrorImage Height="14px" Width="14px" >
                                                </ErrorImage>
                                            </ValidationSettings>
                                            <Items>
                                                <dxe:ListEditItem Text="Screen" Value="Screen"></dxe:ListEditItem>
                                                <dxe:ListEditItem Text="Print" Value="Print"></dxe:ListEditItem>
                                            </Items>
                                            <ClientSideEvents ValueChanged="function(s, e) {ShowHideButton(s.GetValue());}" />
                                        </dxe:ASPxRadioButtonList>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td id="show">
                                                    <dxe:ASPxButton ID="btnShow" runat="server" 
                                                         OnClick="btnShow_Click" Text="Show" ValidationGroup="a">
                                                        <ClientSideEvents Click="function(s, e) { hidesearch();}" />
                                                    </dxe:ASPxButton>
                                                </td>
                                                <td id="prnt" style="display: none;">
                                                    <dxe:ASPxButton ID="btnShow1" runat="server" 
                                                         OnClick="btnShow_Click" Text="Show" ValidationGroup="a">
                                                        
                                                    </dxe:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="text-align: center">
                        <td colspan="6">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                                <progresstemplate>
                                      <div id='Div1' style='position:absolute; font-family:arial; font-size:30; left:50%; top:50%;background-color:white; layer-background-color:white; height:80; width:150;'>
                    <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'> 
                      <tr><td><table><tr> 
                         <td height='25' align='center' bgcolor='#FFFFFF'> 
                           <img src='/assests/images/progress.gif' width='18' height='18'></td>  
                            <td height='10' width='100%' align='center' bgcolor='#FFFFFF'><font size='2' face='Tahoma'> 
 	                        <strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td> 
                            </tr>  </table></td></tr>
                            </table> 
                    </div> 
                                </progresstemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                   
                    <tr>
                        <td colspan="6" width="100%">
                             <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <contenttemplate>
                                    
            <span style="color: red; text-align: center" id="norecord" class="Ecoheadtxt" runat="server">
                <strong> 
                    No Transction Found</strong></span>
            <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="50%">
                
                    <tr>
                        <td width="8%">
                           <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="Blue"
                                OnClick="btnFirst" OnClientClick="javascript:selecttion();">First</asp:LinkButton></td>
                        <td width="12%">
                            <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                OnClick="btnPrevious" OnClientClick="javascript:selecttion();">Previous</asp:LinkButton></td>
                        <td width="12%">
                            <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="Blue"
                                OnClick="btnNext" OnClientClick="javascript:selecttion();">Next</asp:LinkButton></td>
                        <td width="8%">
                           <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue"
                                OnClick="btnLast" OnClientClick="javascript:selecttion();">Last</asp:LinkButton></td>
                        <td align="right" width="60%">
                            
                            <asp:Label ID="listRecord" runat="server" Font-Bold="True"></asp:Label></td>
                    </tr>
                 
            </table>
            <table style="background-color: white" id="list" bordercolor="blue" cellspacing="0"
                cellpadding="0" width="100%" border="1" runat="server">
                <%--<tbody style="width:100%">--%>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%">
                                <%--<tbody style="width:100%">--%>
                                    <tr style="font-size: 12px; font-family: Calibri">
                                        <td>
                                            Client Id :
                                        </td>
                                        <td>
                                            <asp:Label ID="boid" runat="server" Font-Bold="True"></asp:Label></td>
                                        <td>
                                            Category:</td>
                                        <td colspan="3">
                                            <asp:Label ID="category" runat="server" Font-Bold="True"></asp:Label></td>
                                        <td>
                                            Status:
                                        </td>
                                        <td>
                                            <asp:Label ID="status" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name of Holders:</td>
                                        <td colspan="7">
                                            <asp:Label ID="holders" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="transRow" runat="server">
                                        <td style="font-size: 12px; font-family: Calibri; height: 19px;">
                                        </td>
                                        <td align="right" colspan="7" style="height: 19px">
                                            <asp:Label ID="lblTransction" runat="server" Text="0"></asp:Label>
                                            of
                                            <asp:Label ID="lblTotalTransction" runat="server" Text="0"></asp:Label>
                                            Transctions&nbsp;
                                            <asp:LinkButton ID="btnTransPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                OnClick="btnTransPrevious_Click">Previous</asp:LinkButton>&nbsp;
                                            <asp:LinkButton ID="btnTransnNext" runat="server" Font-Bold="True" ForeColor="Blue"
                                                OnClick="btnTransnNext_Click">Next</asp:LinkButton>
                                        </td>
                                    </tr>
                               <%-- </tbody>--%>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <div id="display" runat="server">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblTransction1" runat="server" Text="0"></asp:Label>
                            of
                            <asp:Label ID="lblTotalTransction1" runat="server" Text="0"></asp:Label>
                            Transctions&nbsp;
                            <asp:LinkButton ID="btnTransPrevious1" OnClick="btnTransPrevious_Click" runat="server"
                                Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();">Previous</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="btnTransnNext1" OnClick="btnTransnNext_Click" runat="server"
                                Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();">Next</asp:LinkButton></td>
                    </tr>
               <%-- </tbody>--%>
            </table>
       


                                </contenttemplate>
                                <triggers>
                                <asp:AsyncPostBackTrigger ControlID="ASPxFirst" EventName="Click"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="ASPxLast" EventName="Click"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="ASPxNext" EventName="Click"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="ASPxPrevious" EventName="Click"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click"></asp:AsyncPostBackTrigger>
                                 <asp:PostBackTrigger ControlID="btnShow1"  ></asp:PostBackTrigger>
                                 </triggers>
                            </asp:UpdatePanel>
                             
                             
                             
                             </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
