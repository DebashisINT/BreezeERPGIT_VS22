<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.management_Frm_SendEmailWithTemplate" ValidateRequest="false" CodeBehind="Frm_SendEmailWithTemplate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
    <%-- <%@ register assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
        namespace="DevExpress.Web" tagprefix="dxe" %>--%>


    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>

   
    <script type="text/javascript" language="javascript">

        Mainvalue = "";
               function fnddlGroup(obj) {
                   if (obj == "0") {
                       document.getElementById('td_group').style.display = 'none';
                       document.getElementById('td_branch').style.display = 'inline';
                   }
                   else {
                       document.getElementById('td_group').style.display = 'inline';
                       document.getElementById('td_branch').style.display = 'none';
                       var btn = document.getElementById('btnhide');
                       btn.click();
                   }
               }
               function height() {
                   if (document.body.scrollHeight >= 500) {
                       window.frameElement.height = document.body.scrollHeight;
                   }
                   else {
                       window.frameElement.height = '500';
                   }
                   window.frameElement.widht = document.body.scrollWidht;
               }

               function ShowSelect(objClients) {
                   if (objClients == 'n') {
                       document.getElementById('showFilter').style.display = "none";
                       Hide('TdFilter');
                   }
                   else if (objClients == 'c') {
                       var cmb = document.getElementById('cmbsearchOption');
                       cmb.value = 'Clients';
                       document.getElementById('showFilter').style.display = "inline";
                       document.getElementById('TdFilter').style.display = "inline";

                   }
                   else if (objClients == 'b') {
                       var cmb = document.getElementById('cmbsearchOption');
                       cmb.value = 'Branch';
                       document.getElementById('showFilter').style.display = "inline";
                       document.getElementById('TdFilter').style.display = "inline";
                   }
                   else if (objClients == 'g') {
                       var cmb = document.getElementById('cmbsearchOption');
                       cmb.value = 'Group';
                       document.getElementById('showFilter').style.display = "inline";
                       document.getElementById('TdFilter').style.display = "inline";
                   }
               }

               function Page_Load() {
                   document.getElementById('showFilter').style.display = "none";
                   document.getElementById('TdFilter').style.display = "none";
                   document.getElementById('TrPdf').style.display = 'none';
                   disableVar();
                   height();


               }
               function disableVar() {
                   document.getElementById("trClientName").style.display = "none";
                   document.getElementById("TradingCode").style.display = "none";

               }


               function FunClientScrip(objID, objListFun, objEvent) {
                   var cmbVal;
                   var cmbgGenerate = document.getElementById('cmbGenerate').value;
                   if (cmbgGenerate == '1') {
                       objListFun = 'ShowClientTemplatePDF';
                   }

                   Mainvalue = document.getElementById('cmbType').value;
                   if (document.getElementById('cmbsearchOption').value == "Clients") {
                       if (document.getElementById('ddlGroup').value == "0") {
                           if (document.getElementById('rdbranchAll').checked == true) {
                               cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                               cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue;
                           }
                           else {
                               cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                               cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue + '~' + Mainvalue;
                           }
                       }
                       else {
                           if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                               cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                               cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue;
                           }
                           else {
                               cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                               cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue + '~' + Mainvalue;
                           }
                       }
                   }
                   else {
                       cmbVal = document.getElementById('cmbsearchOption').value;
                       cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue;
                   }
                   ajax_showOptions(objID, objListFun, objEvent, cmbVal, 'Sub');
               }
               function btnAddsubscriptionlist_click() {
                   var userid = document.getElementById('txtsubscriptionID');
                   if (userid.value != '') {
                       var ids = document.getElementById('txtsubscriptionID_hidden');
                       var listBox = document.getElementById('lstSuscriptions');
                       var tLength = listBox.length;
                       //alert(tLength);

                       var no = new Option();
                       no.value = ids.value;
                       no.text = userid.value;
                       listBox[tLength] = no;
                       var recipient = document.getElementById('txtsubscriptionID');
                       recipient.value = '';
                   }
                   else
                       alert('Please search name and then Add!')
                   var s = document.getElementById('txtsubscriptionID');
                   s.focus();
                   s.select();
               }
               function btnRemovefromsubscriptionlist_click() {

                   var listBox = document.getElementById('lstSuscriptions');
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
               function clientselectionfinal() {
                   var listBoxSubs = document.getElementById('lstSuscriptions');
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
                   document.getElementById('showFilter').style.display = "none";
                   document.getElementById('TdFilter').style.display = "none";

               }
               function ReceiveServerData(rValue) {
                   var Data = rValue.split('~');
                   if (Data[0] == 'Group') {
                       groupvalue = Data[1];
                       document.getElementById('HdnGroup').value = Data[1];
                   }
                   if (Data[0] == 'Branch') {
                       groupvalue = Data[1];
                       document.getElementById('HdnBranch').value = Data[1];
                   }
                   if (Data[0] == 'Clients') {
                       document.getElementById('HdnClient').value = Data[1];
                   }

               }

               function fngrouptype(obj) {
                   if (obj == "0") {
                       document.getElementById('td_allselect').style.display = 'none';
                       alert('Please Select Group Type !');
                   }
                   else {
                       document.getElementById('td_allselect').style.display = 'inline';
                   }
               }

               function TypeSet(obj) {

                   if (obj == 'ND') {
                       document.getElementById("trFName").style.display = "none";
                       document.getElementById("trMName").style.display = "none";
                       document.getElementById("trLName").style.display = "none";
                       document.getElementById("trCode").style.display = "inline";
                       document.getElementById("trAdd1").style.display = "inline";
                       document.getElementById("trAdd2").style.display = "inline";
                       document.getElementById("trAdd3").style.display = "inline";
                       document.getElementById("trCity").style.display = "inline";
                       document.getElementById("trState").style.display = "none";
                       document.getElementById("trCountry").style.display = "none";
                       document.getElementById("trPIN").style.display = "inline";
                       document.getElementById("trISD").style.display = "none";
                       document.getElementById("trSTD").style.display = "none";
                       document.getElementById("trPhone").style.display = "inline";
                       document.getElementById("trMob").style.display = "none";
                       document.getElementById("trPAN").style.display = "inline";
                       document.getElementById("trDOB").style.display = "none";
                       document.getElementById("trClientName").style.display = "inline";
                       document.getElementById("TradingCode").style.display = "inline";


                   }
                   else if (obj == 'CD') {
                       document.getElementById("trFName").style.display = "none";
                       document.getElementById("trMName").style.display = "none";
                       document.getElementById("trLName").style.display = "none";
                       document.getElementById("trCode").style.display = "inline";
                       document.getElementById("trAdd1").style.display = "inline";
                       document.getElementById("trAdd2").style.display = "inline";
                       document.getElementById("trAdd3").style.display = "inline";
                       document.getElementById("trCity").style.display = "inline";
                       document.getElementById("trState").style.display = "inline";
                       document.getElementById("trCountry").style.display = "none";
                       document.getElementById("trPIN").style.display = "inline";
                       document.getElementById("trISD").style.display = "none";
                       document.getElementById("trSTD").style.display = "none";
                       document.getElementById("trPhone").style.display = "inline";
                       document.getElementById("trMob").style.display = "none";
                       document.getElementById("trPAN").style.display = "inline";
                       document.getElementById("trDOB").style.display = "none";
                       document.getElementById("trClientName").style.display = "inline";
                       document.getElementById("TradingCode").style.display = "inline";
                   }
                   else {
                       document.getElementById("trFName").style.display = "inline";
                       document.getElementById("trMName").style.display = "inline";
                       document.getElementById("trLName").style.display = "inline";
                       document.getElementById("trCode").style.display = "inline";
                       document.getElementById("trAdd1").style.display = "inline";
                       document.getElementById("trAdd2").style.display = "inline";
                       document.getElementById("trAdd3").style.display = "inline";
                       document.getElementById("trCity").style.display = "inline";
                       document.getElementById("trState").style.display = "inline";
                       document.getElementById("trCountry").style.display = "inline";
                       document.getElementById("trPIN").style.display = "inline";
                       document.getElementById("trISD").style.display = "inline";
                       document.getElementById("trSTD").style.display = "inline";
                       document.getElementById("trPhone").style.display = "inline";
                       document.getElementById("trMob").style.display = "inline";
                       document.getElementById("trPAN").style.display = "inline";
                       document.getElementById("trDOB").style.display = "inline";
                       document.getElementById("trClientName").style.display = "none";
                       document.getElementById("TradingCode").style.display = "none";
                   }

                   // var obj=document.getElementById('cmbType').value;
                   drpBranch.PerformCallback(obj);
                   height();


               }

               function GetContent(obj) {
                   var tmp = document.getElementById('HdnTemplate');
                   tmp.value = obj;
                   var btn = document.getElementById('btnContent');
                   btn.click();
                   height();

               }
               function ChangeType(obj) {
                   if (obj == "0") {
                       document.getElementById('TrEmail').style.display = 'inline';
                       document.getElementById('TrPdf').style.display = 'none';

                   }
                   else {
                       document.getElementById('TrEmail').style.display = 'none';
                       document.getElementById('TrPdf').style.display = 'inline';
                   }
               }

               FieldName = 'btnSend';

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
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Generate Template</h3>
        </div>

    </div> 
<div class="form_main inner">
        
        <table class="TableMain100">
            <tr>
                <td valign="top">
                    <table style="margin: 0px 0px 0px 0px;" width="500px"
                        >
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Generate: </span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="cmbGenerate" runat="server" Width="80px" Font-Size="12px" onchange="ChangeType(this.value)"
                                    TabIndex="2">
                                    <asp:ListItem Value="0">Email</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Recipients:</span>
                            </td>
                            <td class="gridcellleft" colspan="2">
                                <asp:DropDownList ID="cmbType" runat="server" Width="150px" Font-Size="11px" onchange="TypeSet(this.value)"
                                    TabIndex="1">
                                    <asp:ListItem Value="CL" Text="Customer"></asp:ListItem>
                                    <asp:ListItem Value="ND" Text="NSDL Clients"></asp:ListItem>
                                    <asp:ListItem Value="CD" Text="CDSL Clients"></asp:ListItem>
                                    <asp:ListItem Value="EM" Text="Employee"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TrForGroup">
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Group By:</span>
                            </td>
                            <td class="gridcellleft">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)"
                                                TabIndex="2">
                                                <asp:ListItem Value="0">Branch</asp:ListItem>
                                                <asp:ListItem Value="1">Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td id="td_branch">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="m" onclick="ShowSelect('n')"
                                                            TabIndex="3" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="m" onclick="ShowSelect('b')"
                                                            TabIndex="4" />Selected
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
                                                                <asp:DropDownList ID="ddlgrouptype" runat="server" Font-Size="12px" onchange="fngrouptype(this.value)"
                                                                    TabIndex="5">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td id="td_allselect" style="display: none;">
                                                        <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="g"
                                                            onclick="ShowSelect('n')" TabIndex="6" />
                                                        All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="g" onclick="ShowSelect('g')"
                                                                TabIndex="7" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="TrClients">
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Clients: </span>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbClientsAll" runat="server" Checked="True" GroupName="b" onclick="ShowSelect('n')"
                                                TabIndex="8" />
                                        </td>
                                        <td>All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientsSelected" runat="server" GroupName="b" onclick="ShowSelect('c')"
                                                TabIndex="9" />
                                        </td>
                                        <td>Selected
                                        </td>
                                        <td>
                                            <span id="litBranch" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Template: </span>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="drpBranch" ClientInstanceName="drpBranch" EnableIncrementalFiltering="True"
                                    runat="server" TabIndex="3" OnCallback="drpBranch_Callback" ValueType="System.String"
                                    Width="400px">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){GetContent(s.GetValue())}" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table width="100%" id="showFilter">
                        <tr>
                            <td style="text-align: right; vertical-align: top">
                                <table cellpadding="0" cellspacing="0" width="400px">
                                    <tr>
                                        <td align="left">
                                            <table width="100%">
                                                <tr>
                                                    <td class="gridcellleft" align="left" style="vertical-align: top; text-align: left"
                                                        id="TdFilter">
                                                        <span id="spanall">
                                                            <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="250px"
                                                                onkeyup="FunClientScrip(this,'ShowClientSendEmail',event)"></asp:TextBox></span>
                                                        <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                            Enabled="false">
                                                            <asp:ListItem>Clients</asp:ListItem>
                                                            <asp:ListItem>Branch</asp:ListItem>
                                                            <asp:ListItem>Group</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                            style="color: #009900; text-decoration: underline; font-size: 8pt;">Add List</span></a><span
                                                                style="color: #009900; font-size: 8pt;"> </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="gridcellleft" align="left">
                                                        <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="60px" Width="335px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="gridcellleft" align="left">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="height: 14px">
                                                                    <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                                </td>
                                                                <td style="height: 14px">
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
                        <tr style="display: none">
                            <td>
                                <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                <asp:HiddenField ID="txtSettlementNumber_hidden" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table style="margin: 0px 0px 0px 0px;" width="500px"
                        border="#ffffff">
                        <tr id="trEmail">
                            <td class="gridcellleft" style="border:none;width:78px">
                                <span class="Ecoheadtxt">Subject:</span>
                            </td>
                            <td class="gridcellleft" style="border:none">
                                <asp:TextBox ID="TxtSubject" runat="server" Width="780px" Height="25px"></asp:TextBox>
                            </td>
                            <td style="border:none;width:150px;">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSend" runat="server" Text="Send Mail" CssClass="btnUpdate btn btn-primary btn-xs" OnClick="btnSend_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSend" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="tr1">
                            <td colspan="3" align="right" id="TrPdf">
                                <asp:Button ID="btnPrint" runat="server" Text="Export PDF" CssClass="btnUpdate" OnClick="btnPrint_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="color: Maroon">[ Please Note: You can only use variable as shown beside the template content.For
                                    mandatory variable write the variable name whithin ##  e.g ##Addres1##. ]
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" valign="top" colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="Server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:PlaceHolder ID="FreeTextBoxPlaceHolder" runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnContent" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                        <td valign="top" style="padding-top: 110px;">
                                            <table style="background-color: #ffffff; border: solid 1px black;" cellpadding="1"
                                                cellspacing="1" border="1">
                                                <tr>
                                                    <td style="font-size: 10px; font-weight: bold; color: Maroon">User Variable
                                                    </td>
                                                </tr>

                                                <tr id="tr2" runat="server">
                                                    <td>
                                                        <asp:Label Text="#USER_NAME#" ID="Label19" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="tr3" runat="server">
                                                    <td>
                                                        <asp:Label Text="#USER_CODE#" ID="Label20" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="tr4" runat="server">
                                                    <td>
                                                        <asp:Label Text="#USER_DESIGNATION#" ID="Label21" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="tr5" runat="server">
                                                    <td>
                                                        <asp:Label Text="#USER_COMPANY#" ID="Label22" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="tr6" runat="server">
                                                    <td>
                                                        <asp:Label Text="#USER_BRANCH#" ID="Label23" runat="server"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="font-size: 10px; font-weight: bold; color: Maroon">Recipients Variable
                                                    </td>
                                                </tr>
                                                <tr id="trClientName" runat="server">
                                                    <td>
                                                        <asp:Label Text="#ClientName#" ID="Label18" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trFName" runat="server">
                                                    <td>
                                                        <asp:Label Text="#FirstName#" ID="textTE" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trMName" runat="server">
                                                    <td>
                                                        <asp:Label Text="#MiddleName#" ID="Label1" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trLName" runat="server">
                                                    <td>
                                                        <asp:Label Text="#LastName#" ID="Label2" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trCode" runat="server">
                                                    <td>
                                                        <asp:Label Text="#ClientID#" ID="Label3" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trAdd1" runat="server">
                                                    <td>
                                                        <asp:Label Text="#Addres1#" ID="Label4" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trAdd2" runat="server">
                                                    <td>
                                                        <asp:Label Text="#Addres2#" ID="Label5" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trAdd3" runat="server">
                                                    <td>
                                                        <asp:Label Text="#Addres3#" ID="Label6" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trCity" runat="server">
                                                    <td>
                                                        <asp:Label Text="#City#" ID="Label7" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trState" runat="server">
                                                    <td>
                                                        <asp:Label Text="#State#" ID="Label8" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trCountry" runat="server">
                                                    <td>
                                                        <asp:Label Text="#Country#" ID="Label9" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trPIN" runat="server">
                                                    <td>
                                                        <asp:Label Text="#Pin#" ID="Label10" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trISD" runat="server">
                                                    <td>
                                                        <asp:Label Text="#ISDCode#" ID="Label11" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trSTD" runat="server">
                                                    <td>
                                                        <asp:Label Text="#STDCode#" ID="Label12" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trPhone" runat="server">
                                                    <td>
                                                        <asp:Label Text="#TelephoneNumber#" ID="Label13" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trMob" runat="server">
                                                    <td>
                                                        <asp:Label Text="#MobNumber#" ID="Label14" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trDOB" runat="server">
                                                    <td>
                                                        <asp:Label Text="#DateOfBirth#" ID="Label15" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trPAN" runat="server">
                                                    <td>
                                                        <asp:Label Text="#PANNumber#" ID="Label16" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trDate" runat="server">
                                                    <td>
                                                        <asp:Label Text="#CurrentDate#" ID="Label17" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="TradingCode" runat="server">
                                                    <td>
                                                        <asp:Label Text="#TradingCode#" ID="Labeltr" runat="server"></asp:Label>
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
                <td colspan="3">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
                        <ProgressTemplate>
                            <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50px; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                                <table border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
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
            <tr>
                <td colspan="3"></td>
            </tr>
            <tr style="display: none;">
                <td>
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HdnBranch" runat="server" />
                    <asp:HiddenField ID="HdnGroup" runat="server" />
                    <asp:HiddenField ID="HdnClient" runat="server" />
                    <asp:HiddenField ID="HdnTemplate" runat="server" />
                    <asp:Button ID="btnContent" runat="server" OnClick="btnContent_Click" />
                </td>
                <td></td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="height: 550px;"></td>
            </tr>
        </table>
    </div>
</asp:Content>
