<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Reports.Reports_AccountsConfirmationStatement" CodeBehind="AccountsConfirmationStatement.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function btnAddsubscriptionlist_click() {
            var userid = document.getElementById('txtSubsubcriptionIDBranch');
            if (userid.value != '') {
                var ids = document.getElementById('txtSubsubcriptionIDBranch_hidden');
                var listBox = document.getElementById('lstSuscriptions');
                var tLength = listBox.length;
                //alert(tLength);

                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtSubsubcriptionIDBranch');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!')
            var s = document.getElementById('txtSubsubcriptionIDBranch');
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
            //            document.getElementById('showFilter').style.display='none';            
            Hide('TdFilter');
            //            document.getElementById('Button1').disabled=false;
        }
        function FunClientScrip(objID, objListFun, objEvent) {
            var cmbVal;


            var SegN = document.getElementById('litSegment');

            //var SegN=document.getElementById('HDNSeg').value;
            var docVal = document.getElementById('ddlAccountType').value
            if (docVal == '0')
                Mainvalue = "'SYSTM00001'";
            else if (docVal == '1')
                Mainvalue = "'SYSTM00002'";
            else if (docVal == '2')
                Mainvalue = "'SYSTM00001','SYSTM00002'";
            else if (docVal == '3') {
                var NsdlCdsl = document.getElementById('HdnSubLedgerType').value;
                if (NsdlCdsl == "NSDL Clients")
                    Mainvalue = "'SYSTM00043'";
                else if (NsdlCdsl == "CDSL Clients")
                    Mainvalue = "'SYSTM00042'";

            }
            if (document.getElementById('cmbsearchOption').value == "Clients") {
                if (document.getElementById('ddlGroup').value == "0")//////////////Group By  selected are branch
                {
                    if (document.getElementById('rdbranchAll').checked == true) {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                        cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue + '~' + SegN.innerText;
                    }
                    else {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                        cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue + '~' + Mainvalue + '~' + SegN.innerText;
                    }
                }
                else //////////////Group By selected are Group
                {
                    if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                        cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue + '~' + SegN.innerText;
                    }
                    else {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                        cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue + '~' + Mainvalue + '~' + SegN.innerText;
                    }
                }
            }
            else {
                cmbVal = document.getElementById('cmbsearchOption').value;
                cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue + '~' + SegN.innerText;
            }
            //          } 
            //alert(objID);
            //alert(objListFun);
            //alert(objEvent);
            //alert(cmbVal);

            ajax_showOptions(objID, objListFun, objEvent, cmbVal);
        }
        function height() {
            if (document.body.scrollHeight >= 500) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '500';
            }
            window.frameElement.Width = document.body.scrollWidth;
        }
        function ShowHide(type, valtype) {
            if (type == 'H')
                Hide('TdFilter');
            else
                Show('TdFilter');
            document.getElementById('cmbsearchOption').value = valtype;
        }
        function ForClients(type) {
            if (type == 'H')
                Hide('TdFilter');
            else
                Show('TdFilter');
            var cmbValue = document.getElementById('txtOtherAccount_hidden').value;
            if (cmbValue == '')
                document.getElementById('cmbsearchOption').value = 'Clients';
            else
                document.getElementById('cmbsearchOption').value = 'Sub Ac';

        }
        function fnddlGroup(obj) {
            if (obj == "0") {
                Hide('td_group');
                Show('td_branch');
            }
            else {
                Hide('td_branch');
                Show('td_group');
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
        function Page_Load() {
            // AccountChange('0');
            height();
            Hide('TdFilter');
            if (document.getElementById('ddlAccountType').value == '3') {
                Show('TdSelectAccount');
                AccountChange('3');
            }
            else {
                Hide('TdSelectAccount');
                AccountChange('0');
            }
            Hide('tdHeader');
            Hide('tdfooter');
            Hide('tdAddSig');
        }
        function AccountChange(obj) {

            if (obj == '0')
                document.getElementById('HdnMainAc').value = "'SYSTM00001'";
            else if (obj == '1')
                document.getElementById('HdnMainAc').value = "'SYSTM00002'";
            else if (obj == '2')
                document.getElementById('HdnMainAc').value = "'SYSTM00001','SYSTM00002'";
            if (obj == '3')
                Show('TdSelectAccount');
            else
                Hide('TdSelectAccount');
        }
        function showOptions1(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3, 'Ac Name1', 'Main');
        }
        function keyVal(obj) {
            document.getElementById('HdnBranch').value = '';
            document.getElementById('HdnSegment').value = '';
            document.getElementById('HdnSubAc').value = '';
            document.getElementById('HdnGroup').value = '';
            var obj1 = obj.split('~');
            Mainvalue = "'" + obj1[0] + "'";
            document.getElementById('HdnMainAc').value = Mainvalue;
            document.getElementById('HdnSubLedgerType').value = obj1[1];
            document.getElementById('HdnForBranchGroup').value = 'a';
            //alert(obj1[1]);
            if (obj1[1] == 'None') {
                Hide('TrSubAccount');
                Hide('TrForGroup');
                //dateTimeForSubledger='a';
            }
            else {
                if (obj1[1] == 'Custom' || obj1[1] == 'Products-Equity' || obj1[1] == 'Products-Commodity  ' || obj1[1] == 'Products-MF' || obj1[1] == 'Products-Insurance ' || obj1[1] == 'Products-ConsumerFinance' || obj1[1] == 'RTAs ' || obj1[1] == 'MFs' || obj1[1] == 'AMCs ' || obj1[1] == ' Insurance Cos' || obj1[1] == 'Consumer Finance Cos  ' || obj1[1] == 'Custodians ' || obj1[1] == 'Consultants' || obj1[1] == 'Share Holder' || obj1[1] == 'Debtors' || obj1[1] == 'Creditors') {
                    Hide('TrForGroup');
                    Show('TrSubAccount');
                }
                else if (obj1[1] == 'NSDL Clients' || obj1[1] == 'CDSL Clients' || obj1[1] == 'Customers') {
                    Show('TrSubAccount');
                    Show('TrForGroup');
                }
                else {

                    Hide('TrSubAccount');
                    Show('TrForGroup');
                    if (obj1[1] != 'NSDL Clients' || obj1[1] != 'CDSL Clients')
                        document.getElementById('HdnForBranchGroup').value = obj1[1];
                }
                //dateTimeForSubledger='b';                
            }
        }
        function ChkCheckProperty(obj, objChk) {
            if (objChk == true) {
                if (obj == 'H')
                    Show('tdHeader');
                else if (obj == 'F')
                    Show('tdfooter');
            }
            else {
                if (obj == 'H')
                    Hide('tdHeader');
                else if (obj == 'F')
                    Hide('tdfooter');
            }
        }
        function FunHeaderFooter(objID, objListFun, objEvent, objParam) {
            ajax_showOptions(objID, objListFun, objEvent, objParam);
        }
        function ChkAddSig(obj) {
            if (obj == true)
                Show('tdAddSig');
            else
                Hide('tdAddSig');
        }
        function FunAddSig(objID, objListFun, objEvent) {
            ajax_showOptions(objID, objListFun, objEvent);
        }
        FieldName = 'txtSubsubcriptionIDBranch_hidden'
    </script>

    <script type="text/ecmascript">

        function ReceiveServerData(rValue) {

            var Data = rValue.split('~');
            if (Data[0] == 'Segment') {

                var combo = document.getElementById('litSegment');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                var seg = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    if (val == '') {
                        seg = items[0];
                        val = "'" + items[1] + "'";

                    }
                    else {
                        seg += ',' + items[0];
                        val += ",'" + items[1] + "'";

                    }
                }
                document.getElementById('HdnSegment').value = seg;
                combo.innerText = val;

                //  document.getElementById('HdnSegment').value=Data[1];
            }
            if (Data[0] == 'Branch') {
                groupvalue = Data[1];
                document.getElementById('HdnBranch').value = Data[1];
            }
            if (Data[0] == 'Sub Ac') {
                document.getElementById('HdnSubAc').value = '';
                document.getElementById('HdnSubAc').value = Data[1];
            }
            if (Data[0] == 'Clients') {
                document.getElementById('HdnSubAc').value = '';
                groupvalue = Data[1];
                document.getElementById('HdnSubAc').value = Data[1];
            }
            if (Data[0] == 'Group') {
                groupvalue = Data[1];
                document.getElementById('HdnGroup').value = Data[1];
            }
            if (Data[0] == 'Ac Name') {
                Mainvalue = Data[1];
                document.getElementById('HdnMainAc').value = Data[1];
                document.getElementById('HdnSubLedgerType').value = Data[2];
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Accounts Confirmation Statement</h3>
        </div>
    </div>
    <div class="form_main inner">
        <table class="TableMain100">
            <%--<tr>
                <td class="EHEADER" style="text-align: center;" colspan="2">
                    <strong><span style="color: #000099">Accounts Confirmation Statement</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <table cellspacing="1" cellpadding="2" 
                       >
                        <tr>
                            <td class="gridcellleft">For the Period 
                            </td>
                            <td style="text-align: left;">
                                <table>
                                    <tr>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtFrom" runat="server" ClientInstanceName="dtFrom" EditFormat="Custom"
                                                UseMaskBehavior="True" Font-Size="12px" Width="108px">
                                                <DropDownButton Text="From">
                                                </DropDownButton>
                                                <ClientSideEvents ValueChanged="function(s,e){dateChange();DateChangeForFrom();}"
                                                    GotFocus="function(s,e){dateChange11();}" />
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtTo" runat="server" ClientInstanceName="dtTo" EditFormat="Custom"
                                                UseMaskBehavior="True" Font-Size="12px" Width="98px">
                                                <DropDownButton Text="To">
                                                </DropDownButton>
                                                <ClientSideEvents ValueChanged="function(s,e){dateChange();DateChangeForTo();}" GotFocus="function(s,e){dateChange11();}" />
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Segment
                            </td>
                            <td style="text-align: left;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbSegAll" runat="server" GroupName="a" onclick="ShowHide('H','Segment')" />
                                        </td>
                                        <td>All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegSelected" runat="server" Checked="True" GroupName="a"
                                                onclick="ShowHide('S','Segment')" />
                                        </td>
                                        <td>Selected
                                        </td>
                                        <td>(<span id="litSegment" runat="server" style="color: Maroon"></span>)
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Select Account
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlAccountType" runat="server" Width="154px" Font-Size="12px"
                                    onchange="AccountChange(this.value)">
                                    <asp:ListItem Value="0">Trading</asp:ListItem>
                                    <asp:ListItem Value="1">Margin Deposit</asp:ListItem>
                                    <asp:ListItem Value="2">Both</asp:ListItem>
                                    <asp:ListItem Value="3">Other Account</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TdSelectAccount">
                            <td class="gridcellleft">Account Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherAccount" runat="server" Font-Size="12px" onkeyup="showOptions1(this,'SearchMainAccountBranchSegment',event)"
                                    Width="259px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="TrForGroup">
                            <td class="gridcellleft">Group By</td>
                            <td>
                                <table>
                                    <tr>
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
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a11" onclick="ShowHide('H','Branch')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a11" onclick="ShowHide('S','Branch')" />Selected
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
                                                            onclick="ShowHide('H','Group')" />
                                                        All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="ShowHide('S','Group')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="TrSubAccount">
                            <td class="gridcellleft">Sub A/C
                            </td>
                            <td style="text-align: left;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdSubAcAll" runat="server" Checked="True" GroupName="c" onclick="ForClients('H')" />
                                        </td>
                                        <td>All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdSubAcSelected" runat="server" GroupName="c" onclick="ForClients('S')" />
                                        </td>
                                        <td>Selected
                                        </td>
                                        <td>
                                            <span id="Span1" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <asp:CheckBox ID="chkHeader" runat="server" onclick="ChkCheckProperty('H',this.checked);" />
                                Use Header
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td id="tdHeader">
                                            <asp:TextBox ID="txtHeader" runat="server" Width="279px" Font-Size="12px" onkeyup="FunHeaderFooter(this,'GetHeaderFooter',event,'H')"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <asp:CheckBox ID="chkFooter" runat="server" onclick="ChkCheckProperty('F',this.checked);" />
                                Use Footer
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td id="tdfooter">
                                            <asp:TextBox ID="txtFooter" runat="server" Width="279px" Font-Size="12px" onkeyup="FunHeaderFooter(this,'GetHeaderFooter',event,'F')"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <asp:CheckBox ID="chkBothPrint" runat="server" />
                                Both Side Print 
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <asp:CheckBox ID="ChkLogo" runat="server" Checked="true" />
                                Use Company Logo
                            </td>
                            <td></td>
                        </tr>

                        <tr>
                            <td class="gridcellleft">
                                <asp:CheckBox ID="ChkSignatory" runat="server" onclick="ChkAddSig(this.checked);" />
                                Add Signatory
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td id="tdAddSig">
                                            <asp:TextBox ID="txtSignature" runat="server" Width="279px" Font-Size="12px" onkeyup="FunAddSig(this,'SearchByEmployeesWithSignature',event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Print Order 
                            </td>
                            <td>
                                <table>
                                    <tr>

                                        <td>
                                            <asp:DropDownList ID="ddlPrintOrder" runat="server" Font-Size="12px" Width="153px">
                                                <asp:ListItem Value="BW">Branch Wise</asp:ListItem>
                                                <asp:ListItem Value="GW">Group Wise</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Statement Date 
                            </td>
                            <td>
                                <table>
                                    <tr>

                                        <td>
                                            <dxe:ASPxDateEdit ID="DtSatementDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="98px">
                                                <ClientSideEvents ValueChanged="function(s,e){DateChangeForTo();}" />
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="text-align: right; vertical-align: top;" id="TdFilter">
                    <table>
                        <tr>
                            <td class="gridcellleft" style="vertical-align: top; text-align: right">
                                <asp:TextBox ID="txtSubsubcriptionIDBranch" runat="server" Font-Size="12px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocksMainHead',event)"
                                    Width="150px" CssClass="pull-left"></asp:TextBox>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem>Segment</asp:ListItem>
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Ac Name</asp:ListItem>
                                    <asp:ListItem>Sub Ac</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                    <asp:ListItem>Clients</asp:ListItem>
                                </asp:DropDownList>
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                        style="color: #009900; font-size: 8pt;"> </span>
                                <asp:HiddenField ID="txtSubsubcriptionIDBranch_hidden" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; vertical-align: top">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="290px"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()" class="btn btn-primary"><span>Done</span></a>&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()" class="btn btn-danger">
                                                            <span>Remove</span></a>
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
                <td colspan="2" style="padding-left:129px">
                    <asp:Button ID="btnReport" runat="server" Text="Show Report" CssClass="btnUpdate btn btn-primary"
                        OnClick="btnReport_Click"  />
                </td>
            </tr>
            <tr style="display: none">
                <td colspan="2">
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:HiddenField ID="HdnBranch" runat="server" />
                    <asp:HiddenField ID="HdnSegment" runat="server" />
                    <asp:HiddenField ID="HdnMainAc" runat="server" />
                    <asp:HiddenField ID="HdnSubAc" runat="server" />
                    <asp:HiddenField ID="HdnSubLedgerType" runat="server" />
                    <asp:HiddenField ID="HdnGroup" runat="server" />
                    <asp:HiddenField ID="HdnForBranchGroup" runat="server" />
                    <asp:HiddenField ID="txtOtherAccount_hidden" runat="server" />
                    <asp:HiddenField ID="txtSignature_hidden" runat="server" />
                    <asp:HiddenField ID="txtHeader_hidden" runat="server" />
                    <asp:HiddenField ID="txtFooter_hidden" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

