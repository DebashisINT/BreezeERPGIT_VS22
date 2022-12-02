<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_TradedClients" CodeBehind="TradedClients.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>
  
    <script language="javascript" type="text/javascript">
    function Page_Load()///Call Into Page Load
               {
                   document.getElementById('td_btnprint').style.display = 'none';
                   document.getElementById('tr_display').style.display = 'none';
                   document.getElementById('showFilter').style.display = 'none';
                   document.getElementById('hiddencount').value = 0;
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
               function fn_ddllist(obj) {
                   if (obj == '0') {
                       Hide('td_btnprint');
                       Show('td_show');
                   }
                   if (obj == '1') {
                       Hide('td_show');
                       Show('td_btnprint');
                   }
               }
               function fnSegment(obj) {
                   if (obj == "a")
                       Hide('showFilter');
                   else if (obj == "c") {
                       Hide('showFilter');
                       Show('Td_Specific');
                   }
                   else {
                       var cmb = document.getElementById('cmbsearchOption');
                       cmb.value = 'Segment';
                       Show('showFilter');
                   }
                   selecttion();
               }
               function Clients(obj) {
                   if (obj == "a") {
                       Hide('showFilter');
                       Show('tr_daterange');
                   }
                   else {
                       var cmb = document.getElementById('cmbsearchOption');
                       cmb.value = 'Clients';
                       Show('showFilter');
                       Hide('tr_daterange');
                   }

                 //  height();
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

               function FnRecord(obj) {
                   if (obj == '1') {
                       Show('tr_display');
                       Hide('tr_Selection');
                   }
                   if (obj == '2' || obj == '3') {
                       Hide('tr_display');
                       Show('tr_Selection');

                       if (obj == '3')
                           alert('No Record Found !!')
                   }
                   Hide('showFilter');
                   height();
               }
               FieldName = 'lstSlection';
               </script>
    <script type="text/ecmascript">
     function ReceiveServerData(rValue) {

    var j = rValue.split('~');

    if (j[0] == 'Clients') {
        document.getElementById('HiddenField_Client').value = j[1];
    }
    if (j[0] == 'Segment') {
        document.getElementById('HiddenField_Segment').value = j[1];
    }


}
               function CallAjax(Obj1, Obj2, Obj3) {
                   var DropDownValue = document.getElementById("cmbsearchOption").value;
                   if ((DropDownValue == "Clients") && (cComboClientType.GetText() != 'All')) {
                       ajax_showOptions(Obj1, Obj2, Obj3, "ClientParam~cnt_clienttype~" + cComboClientType.GetText());
                   }
                   else {
                       ajax_showOptions(Obj1, Obj2, Obj3, DropDownValue);
                   }
               }
               function ClientTypeChange() {
                   document.getElementById("rdbClientSelected").checked = 'false';
                   document.getElementById("rdbClientALL").checked = 'true';
                   Show('tr_daterange');
                   Hide('showFilter');
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
                    <strong><span id="SpanHeader" style="color: #000099">Clients Having Trades In A Period</span></strong></td>
                <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="FnRecord(2);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>

                </td>
            </tr>
        </table>
        <table id="tr_Selection">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Segment:</td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentAll" runat="server" GroupName="d" onclick="fnSegment('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentSpecific" runat="server" Checked="True" GroupName="d"
                                                onclick="fnSegment('c')" />Current
                                        </td>
                                        <td id="Td_Specific">[ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdSegmentSelected" runat="server" GroupName="d" onclick="fnSegment('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">ClientType :</td>
                                        <td>
                                            <dxe:ASPxComboBox ID="ComboClientType" runat="server" Width="150px" ClientInstanceName="cComboClientType" Font-Size="Small" ValueType="System.String" SelectedIndex="0" EnableIncrementalFiltering="True">
                                                <ClientSideEvents SelectedIndexChanged="ClientTypeChange" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Clients :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="Clients('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="Clients('b')" />
                                            Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_daterange" valign="top">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Period :</td>
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
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report Type :</td>
                                        <td>

                                            <asp:DropDownList ID="ddllist" runat="server" Width="160px" Font-Size="12px" onchange="fn_ddllist(this.value)">
                                                <asp:ListItem Value="0">Show</asp:ListItem>
                                                <asp:ListItem Value="1">Export To Excel</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td id="td_show">
                                                        <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                            Width="101px" OnClick="btnshow_Click" /></td>
                                                    <td id="td_btnprint">
                                                        <asp:Button ID="btnprint" runat="server" CssClass="btnUpdate" Height="20px" Text="Print"
                                                            Width="101px" OnClick="btnprint_Click" /></td>
                                                </tr>
                                            </table>
                                        </td>
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
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="CallAjax(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Segment</asp:ListItem>
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
        </table>
        <table>
            <tr id="tr_display">
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <div id="display" runat="server" style="overflow: scroll">
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="display: none;">
                    <asp:HiddenField ID="hiddencount" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
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
    </div>
</asp:Content>
