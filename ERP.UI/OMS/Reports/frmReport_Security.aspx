<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_Security" Codebehind="frmReport_Security.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

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
        function Page_Load() {
            Hide('TdClient');
            Hide('showFilter');
            Hide('TrSettlement');
            Hide('TrSett');
            Hide('TrFilter');
            Hide('lnkButton');
            Hide('TrDpAcc');
            height();
        }
        function height() {
            if (document.body.scrollHeight >= 500) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '500';
            }
            window.frameElement.width = document.body.scrollWidth;
        }
        function HeightCall() {
            height();
            Hide('TrAll1');
            Hide('TrAll2');
            Show('TrFilter');
        }
        function ForFilter() {
            Show('TrAll1');
            Show('TrAll2');
            Hide('TrFilter');
            height();
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Scrip(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Scrips';
                Show('showFilter');
            }
        }
        function forClient(obj) {
            if (obj == "a")
                Show('TdClient');
            else
                Hide('TdClient');
        }
        function Client(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Clients';
                Show('showFilter');
            }
        }
        function ChngSett(obj) {
            if (obj == "A")
                Hide('TrSettlement');
            else
                Show('TrSettlement');
        }
        function SettFromDP(obj) {
            var objType = obj.split('~');
            if (objType[1] == '[POOL]')
                Show('TrSett');
            else
                Hide('TrSett');
        }
        function FunClientScrip(objID, objListFun, objEvent) {
            var cmbVal = document.getElementById('cmbsearchOption').value;
            ajax_showOptions(objID, objListFun, objEvent, cmbVal, 'Sub');
        }
        function FunSettNumber(objID, objListFun, objEvent) {
            ajax_showOptions(objID, objListFun, objEvent, 'SettType', 'Sub');
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
            Hide('showFilter');
        }
        function selecttion() {
            var combo = document.getElementById('ddlExport');
            combo.value = 'Ex';
        }
        function HideOn() {
            Show('lnkButton');
            document.getElementById('btnTransPrevious').disabled = true;
        }
        function HideOff() {
            Hide('lnkButton');
        }
        function TransNext(obj) {
            if (obj == 'a')
                document.getElementById('btnTransPrevious').disabled = true;
            else
                document.getElementById('btnTransPrevious').disabled = false;
            document.getElementById('btnTransnNext').disabled = false;
        }
        function NextTrans(obj) {
            if (obj == 'a')
                document.getElementById('btnTransnNext').disabled = true;
            else
                document.getElementById('btnTransnNext').disabled = false;
            document.getElementById('btnTransPrevious').disabled = false;
        }
        function AccountSelect(obj) {
            if (obj == 'a')
                Hide('TrDpAcc');
            else
                Show('TrDpAcc');
        }
        FieldName = 'lstSuscriptions';
    </script>

    <script type="text/ecmascript">

        function ReceiveServerData(rValue) {
            var Data = rValue.split('~');
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
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

            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Register of Security Report</span></strong>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr id="TrAll1">
                    <td style="text-align: left; vertical-align: top;">
                        <table border="0">
                            <tr>
                                <td class="gridcellleft">
                                    Date :
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="DtFrom" runat="server" Font-Size="12px" Width="189px" EditFormat="Custom"
                                        EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                        <DropDownButton Text="From">
                                        </DropDownButton>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtTo" runat="server" Font-Size="12px" Width="189px" EditFormat="Custom"
                                        EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                        <DropDownButton Text="To">
                                        </DropDownButton>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    Accounts :
                                </td>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radAllAccount" runat="server" GroupName="account" onclick="AccountSelect('a')"/>
                                            </td>
                                            <td>
                                                All</td>
                                            <td>
                                                <asp:RadioButton ID="radAllMarginAccount" runat="server" Checked="true" GroupName="account" onclick="AccountSelect('a')"/>
                                            </td>
                                            <td>
                                                All Margin Accounts</td>
                                        </tr>
                                         <tr>
                                            <td>
                                                <asp:RadioButton ID="radAllPoolAccounts" runat="server" GroupName="account" onclick="AccountSelect('a')"/>
                                            </td>
                                            <td>
                                                All Pool Accounts</td>
                                            <td>
                                                <asp:RadioButton ID="radSelectedAccount" runat="server" GroupName="account" onclick="AccountSelect('b')"/>
                                            </td>
                                            <td>
                                                Selected Account</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrDpAcc">
                                <td class="gridcellleft">
                                    DP A/C :
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlDPAc" runat="server" Width="191px" onchange="SettFromDP(this.value)"
                                        Font-Size="12px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    Scrips :
                                </td>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radScripAll" runat="server" Checked="true" GroupName="b" onclick="Scrip('a')" />
                                            </td>
                                            <td>
                                                All</td>
                                            <td>
                                                <asp:RadioButton ID="radScripSelected" runat="server" GroupName="b" onclick="Scrip('b')" />
                                            </td>
                                            <td>
                                                Selected</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrSett">
                                <td class="gridcellleft">
                                    Settlements :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSett" runat="server" onchange="ChngSett(this.value)" Width="191px"
                                        Font-Size="12px">
                                        <asp:ListItem Value="A">All</asp:ListItem>
                                        <asp:ListItem Value="F">For</asp:ListItem>
                                        <asp:ListItem Value="U">Upto</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td id="TrSettlement">
                                    <asp:TextBox ID="txtSettlement" runat="server" onkeyup="FunSettNumber(this,'ShowClientScrip',event)"
                                        Font-Size="12px" Width="184px"></asp:TextBox><asp:HiddenField ID="txtSettlement_hidden"
                                            runat="server" />
                                </td>
                            </tr>
                            <tr id="TrFor">
                                <td class="gridcellleft">
                                    For
                                </td>
                                <td style="text-align: left;" id="TrClientFor">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radClient" runat="server" GroupName="a1" onclick="forClient('a')" />
                                            </td>
                                            <td>
                                                Client
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radExchange" runat="server" GroupName="a1" onclick="forClient('b')" />
                                            </td>
                                            <td>
                                                Exchange
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radBoth" runat="server" Checked="True" GroupName="a1" onclick="forClient('b')" />
                                            </td>
                                            <td>
                                                Both
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="text-align: left;" id="TdClient">
                                    <asp:Panel ID="Panel1" BorderColor="white" BorderWidth="1px" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="radAll" runat="server" Checked="True" GroupName="a2" onclick="Client('a')" />
                                                </td>
                                                <td>
                                                    All Client
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="radPOAClient" runat="server" GroupName="a2" onclick="Client('a')" />
                                                </td>
                                                <td>
                                                    POA Client
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="radSelected" runat="server" GroupName="a2" onclick="Client('b')" />
                                                </td>
                                                <td>
                                                    Selected Client
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    Report Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReportType" runat="server" Font-Size="12px" Width="208px">
                                        <asp:ListItem Value="1">Security Wise</asp:ListItem>
                                        <asp:ListItem Value="2">Client Wise</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: right; vertical-align: top;">
                        <table width="100%" id="showFilter">
                            <tr>
                                <td style="text-align: right; vertical-align: top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter">
                                                            <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="150px"
                                                                onkeyup="FunClientScrip(this,'ShowClientScrip',event)"></asp:TextBox>
                                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                                Enabled="false">
                                                                <asp:ListItem>Clients</asp:ListItem>
                                                                <asp:ListItem>Scrips</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add List</span></a><span
                                                                    style="color: #009900; font-size: 8pt;"> </span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="290px">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="height: 14px">
                                                            <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                                text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
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
                            <tr style="display: none">
                                <td>
                                    <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="TrAll2">
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btnUpdate" Height="26px"
                                        OnClick="btnShow_Click" OnClientClick="javascript:selecttion();" Width="105px" />
                                </td>
                                <td style="text-align: center">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                        <ProgressTemplate>
                                            <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; background-color: white;
                                                layer-background-color: white;'>
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
                        </table>
                    </td>
                </tr>
                <tr id="TrFilter">
                    <td colspan="2" style="text-align: right">
                        <span style="font-weight: bold; color: Blue; cursor: pointer" onclick="javascript:ForFilter();">
                            Filter</span>
                        <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" Font-Size="12px"
                            OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="divShow" runat="server">
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnTransnNext" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnTransPrevious" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr id="lnkButton">
                    <td colspan="2">
                        <asp:LinkButton ID="btnTransPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                            OnClick="btnTransPrevious_Click">Previous</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="btnTransnNext" runat="server" Font-Bold="True" ForeColor="Blue"
                            OnClick="btnTransnNext_Click1">Next</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
