<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_RegisterOfTransaction" CodeBehind="RegisterOfTransaction.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>
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
         Hide('TabshowFilter');
         Hide('Td_Screen');
         Hide('td_filter');
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
     function SignOff() {
         window.parent.SignOff();
     }
     function FunClientScrip(objID, objListFun, objEvent) {
         var cmbVal = document.getElementById('cmbsearchOption').value;
         if (cmbVal == 'ScripsExchange') {
             var exchangesegmnet = "<%=Session["ExchangeSegmentID"]%>";
             var criteritype = 'B';
             if (exchangesegmnet == "2" || exchangesegmnet == "5" || exchangesegmnet == "20") {
                 criteritype = ' AND Equity_EffectUntil>="' + date2 + '"  ';
                 criteritype = criteritype.replace('"', "'");
                 criteritype = criteritype.replace('"', "'");
                 cmbVal = cmbVal + '~' + 'Date' + '~' + criteritype;
             }
             else if (exchangesegmnet == "1" || exchangesegmnet == "4" || exchangesegmnet == "15" || exchangesegmnet == "19") {
                 cmbVal = cmbVal + '~' + 'NoDate';
             }

             else {
                 criteritype = ' AND Commodity_ExpiryDate>="' + date2 + '"  ';
                 criteritype = criteritype.replace('"', "'");
                 criteritype = criteritype.replace('"', "'");
                 cmbVal = cmbVal + '~' + 'Date' + '~' + criteritype;

             }

         }
         ajax_showOptions(objID, objListFun, objEvent, cmbVal);
     }

     function fnClients(obj) {
         if (obj == "a")
             Hide('TabshowFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Clients';
             Show('TabshowFilter');
         }

     }
     function fnScrips(obj) {
         if (obj == "a")
             Hide('TabshowFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'ScripsExchange';
             Show('TabshowFilter');
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

         Hide('TabshowFilter');

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
     function fnAlert(obj) {
         if (obj == '1' || obj == '3') {
             Hide('Div_Display');
             Show('tab1');
             Hide('td_filter');
             if (obj == '1')
                 alert('No Record Found!!');
         }
         else {
             Show('Div_Display');
             Hide('tab1');
             Show('td_filter');
         }
         document.getElementById('hiddencount').value = 0;
         Hide('TabshowFilter');
         height();
     }
     function fnrpttype(obj) {
         if (obj == "Excel") {
             Hide('Tr_PrintLogo');
         }
         else {
             Show('Tr_PrintLogo');
         }

     }
     function FnBannedEntity(obj) {
         if (obj.checked) {
             Show('Td_Screen');
             Hide('Td_Export');
             Hide('Tr_PrintLogo');
             Hide('Tr_Export');
         }
         else {
             Hide('Td_Screen');
             Show('Td_Export');
             Show('Tr_PrintLogo');
             Show('Tr_Export');
         }
     }
     function FnPopUp(obj, objtype) {

         if (objtype == 'BtnPanCard') {
             document.getElementById('HiddenField_PanCard').value = obj;
             document.getElementById('BtnPanCard').click();
         }
         if (objtype == 'PopUp') {
             var url = "../management/frmBannedClientDetail.aspx?id=" + obj;
             OnMoreInfoClick(url, 'Details For', '940px', '450px,resize=0', 'N');
         }



     }
     FieldName = 'lstSlection';
     </script>



    <script type="text/ecmascript">
      function ReceiveServerData(rValue) {

    var j = rValue.split('~');
    var btn = document.getElementById('btnhide');

    if (j[0] == 'Clients') {
        document.getElementById('HiddenField_Client').value = j[1];
    }
    if (j[0] == 'ScripsExchange') {
        document.getElementById('HiddenField_ScripsExchange').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">Register Of Transaction</span></strong></td>

                <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="fnAlert('3');"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>

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
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report Type :</td>
                                        <td>
                                            <asp:DropDownList ID="DdlRptType" runat="server" Width="250px" Font-Size="12px">
                                                <asp:ListItem Value="1">Trade Time Wise</asp:ListItem>
                                                <asp:ListItem Value="2">Client Wise</asp:ListItem>
                                                <asp:ListItem Value="3">Client+Share Wise</asp:ListItem>
                                                <asp:ListItem Value="4">Share Wise</asp:ListItem>
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
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Period :
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtFrom">
                                                <DropDownButton Text="From">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="gridcellleft">
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
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Clients :</td>
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
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Scrips :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbScripsAll" runat="server" Checked="True" GroupName="d" onclick="fnScrips('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbScripsSelected" runat="server" GroupName="d" onclick="fnScrips('b')" />
                                            Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC" style="width: 50%">Trade Time :
                                        </td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txtfromtime" runat="server" HorizontalAlign="Left" Width="100px"
                                                Text="00:00:00">
                                                <ValidationSettings ErrorDisplayMode="None">
                                                </ValidationSettings>
                                                <MaskSettings Mask="HH:mm:ss" />
                                            </dxe:ASPxTextBox>
                                        </td>
                                        <td>To
                                        </td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txttotime" runat="server" HorizontalAlign="Left" Width="100px"
                                                Text="23:59:59">
                                                <ValidationSettings ErrorDisplayMode="None">
                                                </ValidationSettings>
                                                <MaskSettings Mask="HH:mm:ss" />
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Pan Details :</td>
                                                    <td>
                                                        <asp:DropDownList ID="DdlPanDetails" runat="server" Width="150px" Font-Size="12px">
                                                            <asp:ListItem Value="Both">Both</asp:ListItem>
                                                            <asp:ListItem Value="WithPan">Only Pan</asp:ListItem>
                                                            <asp:ListItem Value="WithOutPan">WithOut Pan</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkBannedEntity" runat="server" onclick="FnBannedEntity(this)" />Show Only Banned Entity</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Security Type :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlSecurityType" runat="server" Font-Size="11px" Width="150px">
                                                <asp:ListItem Value="ALL">ALL</asp:ListItem>
                                                <asp:ListItem Value="Approved">Only Approved</asp:ListItem>
                                                <asp:ListItem Value="UnApproved">Only UnApproved</asp:ListItem>
                                                <asp:ListItem Value="Illiquid">Only Illiquid</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                </table>
                            </td>

                        </tr>
                        <tr id="Tr_Export">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Export :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlExport" runat="server" Width="150px" Font-Size="12px" onchange="fnrpttype(this.value)">
                                                <asp:ListItem Value="PDF">PDF</asp:ListItem>
                                                <asp:ListItem Value="Excel">Excel</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_PrintLogo">
                            <td class="gridcellleft">
                                <asp:CheckBox ID="ChkLogoPrint" runat="server" Checked="true" />
                                Do Not Print Logo</td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td id="Td_Screen">
                                            <asp:Button ID="BtnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Screen"
                                                Width="101px" OnClick="BtnScreen_Click" /></td>
                                        <td id="Td_Export">
                                            <asp:Button ID="BtnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export"
                                                Width="101px" OnClick="BtnExcel_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table border="10" cellpadding="1" cellspacing="1" id="TabshowFilter">
                        <tr>
                            <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                id="TdFilter">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem>Clients</asp:ListItem>
                                    <asp:ListItem>ScripsExchange</asp:ListItem>
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
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>

                    <asp:HiddenField ID="HiddenField_ScripsExchange" runat="server" />

                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="hiddencount" runat="server" />
                    <asp:HiddenField ID="HiddenField_PanCard" runat="server" />
                    <asp:Button ID="BtnPanCard" runat="server" OnClick="BtnPanCard_Click" />
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

        <div id="Div_Display" style="display: none;" width="100%">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">

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

                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="BtnPanCard" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

