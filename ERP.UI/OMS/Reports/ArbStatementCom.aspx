<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_ArbStatementCom" CodeBehind="ArbStatementCom.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">


        function Page_Load()///Call Into Page Load
        {
            Hide('showFilter');
            FnReportType('1');
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
            ajax_showOptions(objID, objListFun, objEvent, document.getElementById('cmbsearchOption').value);
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
        function fnArbGroup(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'ArbGroup';
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

        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = 'Ex';
        }
        function FnReportType(Obj) {
            if (Obj == '1')////ArbGroup + Client [Summary]
            {
                Show('Tr_ForDate');
                Hide('Tr_Period');
                Hide('Tr_Cycle');
                Hide('tr_email');
            }
            if (Obj == '2' || Obj == '4')////ArbGroup + Client + Product [Detail]
            {
                Hide('Tr_ForDate');
                Show('Tr_Period');
                Show('Tr_Cycle');
                Show('tr_email');
            }
            if (Obj == '3')////Final Settlement
            {
                Hide('Tr_ForDate');
                Hide('Tr_Period');
                Show('Tr_Cycle');
                Hide('tr_email');
            }
        }
        function FnDateChange(s) {
            document.getElementById('btnhide').click();
        }
        function FnCycleChange(obj) {
            var objVal = obj.split('~');

            dtFrom1.SetDate(new Date(objVal[1]));
            dtTo1.SetDate(new Date(objVal[2]));
            DtFrom.SetDate(new Date(objVal[1]));
            DtTo.SetDate(new Date(objVal[2]));
        }
        function FromDateCheck(obj) {
            var date1 = DtFrom.GetDate();
            var date2 = (new Date(date1.getFullYear(), date1.getMonth(), date1.getDate())).getTime();

            var date3 = dtFrom1.GetDate();
            var date4 = (new Date(date3.getFullYear(), date3.getMonth(), date3.getDate())).getTime();

            if (date2 < date4) {
                DtFrom.SetDate(new Date(dtFrom1.GetDate()));
                alert('Dates Can Not Be Outside The Selected Settlement Cycle !!');
            }
        }
        function ToDateCheck(obj) {
            var date1 = DtTo.GetDate();
            var date2 = (new Date(date1.getFullYear(), date1.getMonth(), date1.getDate())).getTime();

            var date3 = dtTo1.GetDate();
            var date4 = (new Date(date3.getFullYear(), date3.getMonth(), date3.getDate())).getTime();

            if (date2 > date4) {
                DtTo.SetDate(new Date(dtTo1.GetDate()));
                alert('Dates Can Not Be Outside The Selected Settlement Cycle !! ');
            }
        }
        function fnAlert() {
            alert('No Record Found!!');
        }
        function FnValueInsert(obj) {

            document.getElementById('HiddenField_CycleFordate').value = obj
        }
        function RecordDisplay(obj) {
            if (obj == '4')
                alert("Error on sending!Try again.. !!");
            if (obj == '5')
                alert("'Mail Sent Successfully !!'+'\n'+'Emails not Sent For Some Clients...'");
            if (obj == '6')
                alert("Mail Sent Successfully !!");
            if (obj == '7')
                alert("Email Id Not Found!!");
            //FnReportType();
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
            }
            if (j[0] == 'Clients') {
                document.getElementById('HiddenField_Client').value = j[1];
            }
            if (j[0] == 'Product') {
                document.getElementById('HiddenField_Asset').value = j[1];
            }
            if (j[0] == 'BranchGroup') {
                document.getElementById('HiddenField_BranchGroup').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">Daily Arbitrage Statement</span></strong></td>


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
                                            <asp:DropDownList ID="DdlRptType" runat="server" Width="250px" Font-Size="12px" onchange="FnReportType(this.value)">
                                                <asp:ListItem Value="1">ArbGroup + Client</asp:ListItem>
                                                <asp:ListItem Value="2">ArbGroup + Client + Product</asp:ListItem>
                                                <asp:ListItem Value="4">ArbGroup + Client + Product (Only Open Position)</asp:ListItem>
                                                <asp:ListItem Value="3">Final Settlement</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_ForDate">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">For Date :
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtFor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtFor">
                                                <DropDownButton Text="For">
                                                </DropDownButton>
                                                <ClientSideEvents ValueChanged="function(s, e) {FnDateChange(s);}" />
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="DivDisplay" runat="server" style="border: solid 1px black">
                                                    </div>

                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_Cycle">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Settlement Cycle :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="DdlCycle" runat="server" Font-Size="12px" onchange="FnCycleChange(this.value)">
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_Period">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">For A Period :
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtFrom">
                                                <DropDownButton Text="From">
                                                </DropDownButton>
                                                <ClientSideEvents ValueChanged="function(s, e) {FromDateCheck(s);}" />
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtTo">
                                                <DropDownButton Text="To">
                                                </DropDownButton>
                                                <ClientSideEvents ValueChanged="function(s, e) {ToDateCheck(s);}" />
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr id="Tr_ArbGroup">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Arb-Group :</td>
                                        <td>
                                            <asp:RadioButton ID="rdArbGroupAll" runat="server" Checked="True" GroupName="cd" onclick="fnArbGroup('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdArbGroupSelected" runat="server" GroupName="cd" onclick="fnArbGroup('b')" />
                                            Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_Clients">
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
                                <table>
                                    <tr>

                                        <td id="td_Export">
                                            <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                Width="125px" OnClientClick="selecttion()" OnClick="btnExcel_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_email">
                            <td class="gridcellleft">
                                <table>
                                    <tr>

                                        <td>
                                            <asp:Button ID="btnemail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Email To Client"
                                                Width="125px" OnClientClick="selecttion()" OnClick="btnemail_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table border="10" cellpadding="1" cellspacing="1" id="showFilter">
                        <tr>
                            <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                id="TdFilter">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem>Clients</asp:ListItem>
                                    <asp:ListItem>ArbGroup</asp:ListItem>

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

                    <asp:HiddenField ID="HiddenField_ArbGroup" runat="server" />
                    <asp:HiddenField ID="HiddenField_CycleFordate" runat="server" />

                    <asp:HiddenField ID="HiddenField_Client" runat="server" />

                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClientClick="selecttion()" OnClick="btnhide_Click" />
                    <dxe:ASPxDateEdit ID="dtFrom1" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        Font-Size="12px" Width="108px" ClientInstanceName="dtFrom1">
                        <DropDownButton Text="For">
                        </DropDownButton>
                    </dxe:ASPxDateEdit>

                    <dxe:ASPxDateEdit ID="dtTo1" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        Font-Size="12px" Width="108px" ClientInstanceName="dtTo1">
                        <DropDownButton Text="To">
                        </DropDownButton>
                    </dxe:ASPxDateEdit>
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
        <div id="DivRecordDisplay" style="display: none;" width="100%">
            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                <ContentTemplate>
                    <table width="100%" border="1">

                        <tr>
                            <td>
                                <div id="DivHeader" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr bordercolor="Blue" id="Tr_DllPrevNext">
                            <td align="left">
                                <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr valign="top">
                                                <%-- <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Prev" Text="[Prev]" OnCommand="NavigationLinkC_Click"
                                                        OnClientClick="javascript:selecttion();"> </asp:LinkButton>
                                                </td>--%>
                                                <td style="height: 44px">
                                                    <asp:DropDownList ID="cmbrecord" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True"
                                                        onchange="selecttion()" OnSelectedIndexChanged="cmbrecord_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <%--<td style="height: 44px">
                                                    <asp:LinkButton ID="lnkNext" runat="server" CommandName="Next" Text="[Next]" OnCommand="NavigationLinkC_Click"
                                                        OnClientClick="javascript:selecttion();"> </asp:LinkButton>&nbsp;&nbsp;
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnemail" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="Div2" runat="server">
                                </div>
                            </td>
                        </tr>
                        <asp:HiddenField ID="TotalGrp" runat="server" />
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnemail" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>

    </div>
</asp:Content>

