<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_AccountConfirmationSummary" CodeBehind="AccountConfirmationSummary.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        function Page_Load()///Call Into Page Load
        {
            Hide('showFilter');
            fnddlGroup('1');
            height();
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.Width = document.body.scrollWidth;
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
                if (document.getElementById('ddlGroup').value == "2" || document.getElementById('ddlGroup').value == "4")//////////////Group By  selected are branch
                {
                    if (document.getElementById('ddlGroup').value == "2") {
                        if (document.getElementById('RdbBranchAll').checked == true) {
                            cmbVal = 'ClientsBranch' + '~' + 'ALL';
                        }
                        else {
                            cmbVal = 'ClientsBranch' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Branch').value;
                        }
                    }
                    if (document.getElementById('ddlGroup').value == "4") {
                        if (document.getElementById('RdbBranchAll').checked == true) {
                            cmbVal = 'ClientsBranchGroup' + '~' + 'ALL';
                        }
                        else {
                            cmbVal = 'ClientsBranchGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_BranchGroup').value;
                        }
                    }
                }
                else if (document.getElementById('ddlGroup').value == "3")//////////////Group By selected are Group
                {
                    if (document.getElementById('RdbGroupAll').checked == true) {
                        cmbVal = 'ClientsGroup' + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                    }
                    else {
                        cmbVal = 'ClientsGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Group').value;
                    }
                }
                else {
                    cmbVal = 'ClientsBranch' + '~' + 'ALL';
                }
            }
            else {
                cmbVal = document.getElementById('cmbsearchOption').value;
                cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
            }

            ajax_showOptions(objID, objListFun, objEvent, cmbVal);

        }

        function fnClient(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Clients';
                Show('showFilter');
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

        }

        function fnBranch(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                if (document.getElementById('ddlGroup').value == "2") {
                    document.getElementById('cmbsearchOption').value = 'Branch';
                }
                if (document.getElementById('ddlGroup').value == "4") {
                    document.getElementById('cmbsearchOption').value = 'BranchGroup';
                }

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
            if (obj == "2" || obj == "4") {
                Hide('Td_Group');
                Show('Td_Branch');
                Hide('Td_Clients');
            }
            else if (obj == "1") {
                Hide('Td_Group');
                Hide('Td_Branch');
                Show('Td_Clients');
            }

            else {
                Show('Td_Group');
                Hide('Td_Branch');
                Hide('Td_Clients');
                document.getElementById('btnhide').click();
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
        function fnNoRecord(obj) {
            alert(obj);
            height();
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
            if (j[0] == 'Clients') {
                document.getElementById('HiddenField_Client').value = j[1];
            }
            if (j[0] == 'Segment') {
                document.getElementById('HiddenField_Segment').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">Accounts Confirmation Summary Statement With Collaterals</span></strong></td>
            </tr>
        </table>
        <table id="tab1" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">For A Period
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
                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Group By</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                            <asp:ListItem Value="1">Clients</asp:ListItem>
                                                            <asp:ListItem Value="2">Branch</asp:ListItem>
                                                            <asp:ListItem Value="3">Group</asp:ListItem>
                                                            <asp:ListItem Value="4">Branch Group</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="Td_Clients">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RdbClientAll" runat="server" Checked="True" GroupName="a" onclick="fnClient('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RdbClientPOA" runat="server" GroupName="a" onclick="fnClient('a')" />
                                                        POA
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RdbClientSelected" runat="server" GroupName="a" onclick="fnClient('b')" />Selected
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RdbClientAllBtSelected" runat="server" GroupName="a" onclick="fnClient('b')" />
                                                        All But Selected
                                                    </td>

                                                </tr>
                                            </table>
                                        </td>
                                        <td id="Td_Branch">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RdbBranchAll" runat="server" Checked="True" GroupName="b" onclick="fnBranch('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RdbBranchSelected" runat="server" GroupName="b" onclick="fnBranch('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="Td_Group">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
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
                                                        <asp:RadioButton ID="RdbGroupAll" runat="server" Checked="True" GroupName="c" onclick="fnGroup('a')" />
                                                        All
                                                        <asp:RadioButton ID="RdbGroupSelected" runat="server" GroupName="c" onclick="fnGroup('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table cellpadding="1" cellspacing="1" class="tableClass">
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
                        <tr>
                            <td class="gridcellleft">
                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Use Header
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHeader" runat="server" Width="279px" Font-Size="12px" onkeyup="ajax_showOptions(this,'GetHeaderFooter',event,'H')"></asp:TextBox>
                                        </td>

                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Use Footer
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFooter" runat="server" Width="279px" Font-Size="12px" onkeyup="ajax_showOptions(this,'GetHeaderFooter',event,'F')"></asp:TextBox>

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkBothSidePrint" runat="server" />
                                            Both Side Print
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkCompanyLogo" runat="server" Checked="true" />
                                            Use Company Logo
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Add Signatory
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSignature" runat="server" Width="279px" Font-Size="12px" onkeyup="ajax_showOptions(this,'SearchByEmployeesWithSignature',event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <td class="gridcellleft">
                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">Print Date
                                    </td>
                                    <td class="gridcellleft">
                                        <dxe:ASPxDateEdit ID="DtPrintDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                            Font-Size="12px" Width="108px" ClientInstanceName="DtPrintDate">
                                            <DropDownButton Text="Print Date">
                                            </DropDownButton>
                                        </dxe:ASPxDateEdit>
                                    </td>

                                </tr>
                            </table>
                        </td>
                        <tr>
                            <td class="gridcellleft">
                                <asp:Button ID="BtnPrint" runat="server" CssClass="btnUpdate" Height="20px" Text="Print"
                                    Width="101px" OnClick="BtnPrint_Click" /></td>
                        </tr>

                    </table>
                </td>
                <td>
                    <table cellpadding="1" cellspacing="1" id="showFilter">
                        <tr>
                            <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                id="TdFilter">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem>Clients</asp:ListItem>
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                    <asp:ListItem>Segment</asp:ListItem>
                                    <asp:ListItem>BranchGroup</asp:ListItem>
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
                    <asp:Button ID="btnhide" runat="server" Text="btnhide" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                    <asp:HiddenField ID="txtSignature_hidden" runat="server" />
                    <asp:HiddenField ID="txtHeader_hidden" runat="server" />
                    <asp:HiddenField ID="txtFooter_hidden" runat="server" />
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
    </div>
</asp:Content>

