<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_frm_ShowDocument" CodeBehind="frm_ShowDocument.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        //--------New Part Developed By Goutam Das ------------------

        function PopulateGrid(obj) {

            grid.PerformCallback(obj);
        }
        function Changestatus(obj) {
            var URL = "verify_documentremarks.aspx?id=" + obj;
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Verify Remarks", "width=995px,height=300px,center=0,resize=1,top=-1", "recal");
            editwin.onclose = function () {
                grid.PerformCallback();
            }
        }

        function OnDocumentView(keyValue) {
            //alert (keyValue);
            var url = 'viewImage.aspx?id=' + keyValue;
            popup.contentUrl = url;
            popup.Show();

        }


        function fnEntityChange(obj) {
            if (obj == "Accounts") {
                document.getElementById("tr_accounts").style.display = "inline";
                document.getElementById("tr_accounts").style.display = "table-row";
                document.getElementById("TrForGroup").style.display = "None";
                document.getElementById("TrForClient").style.display = "None";
            }
            else if (obj == "Company" || obj == "Building" || obj == "Branches" || obj == "Product") {
                document.getElementById("tr_accounts").style.display = "None";
                document.getElementById("TrForGroup").style.display = "None";
                document.getElementById("TrForClient").style.display = "None";
            }
            document.getElementById("HiddenField_EntityChange").value = "T";
            divAsOnDate.innerText = obj;
            var btn = document.getElementById('btnEntity');
            btn.click();

        }


        function fnDateTypeChange(obj) {
            var btn = document.getElementById('btnDt');
            btn.click();

        }

        function ShowDateRange(obj) {
            if (obj == 'a') {
                document.getElementById("trDateRange").style.display = "none";

            }
            else if (obj == 's') {
                document.getElementById("trDateRange").style.display = "inline";
                document.getElementById("trDateRange").style.display = "table-row";

            }

        }

        groupvalue = "";
        Mainvalue = "";
        FieldName = "btnEmail";

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function Page_Load() {
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdSelect').style.display = 'none';
            document.getElementById('TdFilter1').style.display = 'none';
            //  document.getElementById('TrForGroup').style.display='none';
            divAsOnDate.innerText = "Employee :";
            document.getElementById("trDateRange").style.display = "none";
            document.getElementById("tr_accounts").style.display = "none";
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

        function btnAddsubscriptionlist_click() {


            var userid = document.getElementById('txtsubscriptionID');

            if (userid.value != '') {
                var ids = document.getElementById('txtsubscriptionID_hidden');
                var listBox = document.getElementById('lstSuscriptions');
                var tLength = listBox.length;
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

        function FunClientScrip(objID, objListFun, objEvent) {
            var cmbVal;
            Mainvalue = document.getElementById("drpDocumentEntity").value;

            if (document.getElementById('cmbsearchOption').value == "Clients") {
                if (document.getElementById('ddlGroup').value == "0")//////////////Group By  selected are branch
                {
                    if (document.getElementById('rdbranchAll').checked == true) {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                        cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value + '~' + Mainvalue;
                    }
                    else {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                        cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue + '~' + Mainvalue;
                    }
                }
                else //////////////Group By selected are Group
                {
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

            ajax_showOptions(objID, objListFun, objEvent, cmbVal);
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
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdSelect').style.display = 'none';
            document.getElementById('TdFilter1').style.display = 'none';

        }


        function ReceiveServerData(rValue) {
            var Data = rValue.split('~');
            if (Data[0] == 'Group') {
                groupvalue = Data[1];
                document.getElementById('HdnGroup').value = Data[1];
            }
            if (Data[0] == 'Branch') {
                groupvalue = Data[1];
                document.getElementById('HdnBranchId').value = Data[1];
                var btn = document.getElementById('btnhide');
                btn.click();
            }
            if (Data[0] == 'Clients') {
                document.getElementById('HdnClients').value = Data[1];
            }
        }

        function AllSelct(obj, obj1) {
            var FilTer = document.getElementById('cmbsearchOption');
            if (obj != 'a') {
                if (obj1 == 'C')
                    FilTer.value = 'Clients';
                else if (obj1 == 'B')
                    FilTer.value = 'Branch';
                else if (obj1 == 'G')
                    FilTer.value = 'Group';
                else if (obj1 == 'S')
                    FilTer.value = 'Segment';
                document.getElementById('TdFilter').style.display = 'inline';
                document.getElementById('TdFilter1').style.display = 'inline';
                document.getElementById('TdSelect').style.display = 'inline';
            }
            else {
                document.getElementById('TdFilter').style.display = 'none';
                document.getElementById('TdFilter1').style.display = 'none';
                document.getElementById('TdSelect').style.display = 'none';
            }

        }

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

        //--------New Part Developed By Goutam Das ------------------


        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }

        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }

        function HideShowFilter(obj) {
            if (obj == 'H') {
                Hide('Tr_Filter_Content1');
                Hide('Tr_Filter_Content2');
                Hide('Tr_Filter_Content3');
                Show('Td_HideShowFilter');
            }
            else {
                Show('Tr_Filter_Content1');
                Show('Tr_Filter_Content2');
                Show('Tr_Filter_Content3');
                Hide('Td_HideShowFilter');
            }
            height();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Search Document</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td id="Td_HideShowFilter" style="display: none"><a id="A_HideShowFilter" style="color: Blue" href="#" onclick="HideShowFilter('S')">Show Report Selection Criteria</a></td>
            </tr>
            <tr id="Tr_Filter_Content1">
                <td valign="top">
                    <table cellspacing="1" cellpadding="2" style="background-color: #rgb(237,243,244); border: solid 1px  #ffffff"
                        border="1">
                        <tr>
                            <td class="gridcellleft">Document Entity</td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="drpDocumentEntity" runat="server" onchange="fnEntityChange(this.value)"
                                    Width="250px">
                                    <asp:ListItem>Employee</asp:ListItem>
                                    <asp:ListItem>Customer/Client</asp:ListItem>
                                    <asp:ListItem Value="Company">Company</asp:ListItem>
                                    <asp:ListItem Value="Building">Building</asp:ListItem>
                                    <asp:ListItem Value="Branches">Branches</asp:ListItem>
                                    <%--<asp:ListItem Value="ExchaOther Contacts/Entitiesnges">Other Contacts/Entities</asp:ListItem>--%>
                                    <asp:ListItem Value="Accounts">Accounts</asp:ListItem>
                                    <asp:ListItem Value="Product">Product</asp:ListItem>
                                    <%--    <asp:ListItem>Relationship Partner</asp:ListItem>
                       <asp:ListItem>Partner</asp:ListItem>
                       <asp:ListItem>Lead</asp:ListItem>
                       <asp:ListItem>Sub Broker</asp:ListItem>
                       <asp:ListItem>Franchisee</asp:ListItem>
                        <asp:ListItem>Products MF</asp:ListItem>
                        <asp:ListItem>Products Insurance</asp:ListItem>
                        <asp:ListItem>Products IPOs</asp:ListItem>
                        <asp:ListItem>Data Vendor</asp:ListItem>
                        <asp:ListItem>Referral Agents</asp:ListItem>
                        <asp:ListItem>Recruitment Agent</asp:ListItem>
                        <asp:ListItem>AMCs</asp:ListItem>
                        <asp:ListItem>Insurance Companies</asp:ListItem>
                        <asp:ListItem>RTAs</asp:ListItem>
                        <asp:ListItem>Branches</asp:ListItem>
                        <asp:ListItem>Companies</asp:ListItem>
                        <asp:ListItem>Building</asp:ListItem>
                        <asp:ListItem>ConsumerComp</asp:ListItem>
                        <asp:ListItem>OutsourcingComp</asp:ListItem>
                        <asp:ListItem>HRrecruitmentagent</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <%--  <tr id="tr_accounts">
                            <td></td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                    <td>
                                                            <asp:RadioButton ID="rdmainaccount" runat="server" Checked="true"  GroupName="z"/>
                                                        </td>
                                                        <td class="gridcellleft">
                                                            Subledger Type None
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdcustom" runat="server" GroupName="z"/>
                                                        </td>
                                                        <td class="gridcellleft">
                                                            Subledger Type Custom
                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                
                            </tr>--%>
                        <tr>
                            <td class="gridcellleft">Document Type</td>
                            <td style="text-align: left">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="drpDocumentType" runat="Server" Width="250px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnEntity" EventName="Click"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="TrDate">
                            <td class="gridcellleft">Search By Date:
                            </td>
                            <td align="left">
                                <table>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="cmbDate" runat="server" Height="20px" Font-Size="9" onchange="fnDateTypeChange(this.value)">
                                                            <asp:ListItem Text="Renewal Date" Value="Renewal"></asp:ListItem>
                                                            <asp:ListItem Text="Receive Date" Value="Receive"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadDateRangeA" runat="server" GroupName="G1" Checked="true"
                                                            onclick="ShowDateRange('a')" /></td>
                                                    <td class="gridcellleft">All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadDateRangeS" runat="server" GroupName="G1" onclick="ShowDateRange('s')" /></td>
                                                    <td class="gridcellleft">Specific Date Range
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="trDateRange">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td id="TdFrom">
                                                                <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" ClientInstanceName="dtFrom"
                                                                    UseMaskBehavior="True" Width="108px">
                                                                    <DropDownButton Text="From">
                                                                    </DropDownButton>
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                            <td id="TdTo">
                                                                <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                                                    UseMaskBehavior="True" Width="108px">
                                                                    <DropDownButton Text="To">
                                                                    </DropDownButton>
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnDt" EventName="Click"></asp:AsyncPostBackTrigger>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="TrForGroup">
                            <td class="gridcellleft">Branch/Group :</td>
                            <td class="gridcellleft">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                <asp:ListItem Value="0">Branch</asp:ListItem>
                                                <asp:ListItem Value="1">Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td id="td_branch">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="AllSelct('a','B')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="AllSelct('b','B')" />Selected
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
                                                            onclick="AllSelct('a','G')" />
                                                        All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="AllSelct('b','G')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="TrForClient">
                            <td class="gridcellleft">
                                <div id="divAsOnDate">
                                </div>
                            </td>
                            <td align="left">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbALLSCL" runat="server" Checked="True" GroupName="c" onclick="AllSelct('a','C')" /></td>
                                        <td>All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSCL" runat="server" GroupName="c" onclick="AllSelct('b','C')" /></td>
                                        <td>Specific</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_accounts">
                            <td class="gridcellleft">SubLedger Type
                            </td>
                            <td align="left">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdmainaccount" runat="server" Checked="true" GroupName="z" />
                                        </td>
                                        <td>None
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdcustom" runat="server" GroupName="z" />
                                        </td>
                                        <td>Custom</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align: top">
                    <table>
                        <tr>
                            <td class="gridcellleft" style="vertical-align: top; text-align: left;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td id="TdFilter1" style="height: 23px">
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Width="85px" Font-Size="11px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td id="TdFilter" style="height: 23px">
                                            <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="253" onkeyup="FunClientScrip(this,'ShowClientForDocument',event)"></asp:TextBox><a
                                                id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                        style="color: #009900; font-size: 8pt;">&nbsp;</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; vertical-align: top;">
                                <table cellpadding="0" cellspacing="0" id="TdSelect">
                                    <tr>
                                        <td style="padding-left: 7px">
                                            <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="253px"></asp:ListBox>
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
            <tr id="Tr_Filter_Content2">
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="BtnSubmit" runat="server" Text="Show" CssClass="btn btn-primary" OnClick="BtnSubmit_Click"
                                OnClientClick="return CheckWheatherMandatory();" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="Tr_Filter_Content3">
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                                    <ProgressTemplate>
                                        <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50px; background-color: white; layer-background-color: white; height: 80; width: 150;'>
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
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%">
                        <tr>
                            <td id="Td1" align="left" style="width: 20%">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-success"><span>All Records</span></a>--%>
                            </td>
                            <td align="right" style="width: 80%">
                                <asp:Button ID="btnExport" runat="server" TabIndex="4" Text="Export to Excel" CssClass="btn btn-primary"
                                    OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%" KeyFieldName="doc_id"
                                runat="server" AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback"
                                OnHtmlRowCreated="gridStatus_HtmlRowCreated">
                                <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                                <Styles>
                                    <FocusedRow CssClass="gridselectrow" BackColor="#FCA977">
                                    </FocusedRow>
                                    <FocusedGroupRow CssClass="gridselectrow" BackColor="#FCA977">
                                    </FocusedGroupRow>
                                </Styles>
                                <Settings ShowHorizontalScrollBar="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" AlwaysShowPager="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <Columns>
                                    <dxe:GridViewDataTextColumn Visible="False" FieldName="doc_id" Caption="ID">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CONTACT_NAME" Caption="Name">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="CONTACT_UCC" Caption="Short Name">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Visible="true" FieldName="CONTACT_BRANCH" Caption="Branch"
                                        VisibleIndex="3">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DOC_TYPE" Caption="Doc.Type">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Visible="true" FieldName="doc_documentName" Caption="FileName"
                                        VisibleIndex="5">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Visible="true" FieldName="doc_Note1" Caption="Note 1"
                                        VisibleIndex="6">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Visible="true" FieldName="doc_Note2" Caption="Note 2"
                                        VisibleIndex="7">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Visible="true" FieldName="filename" Caption="FileNumber"
                                        VisibleIndex="8">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Visible="true" FieldName="blndg" Caption="Building"
                                        VisibleIndex="9">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="DOC_LOCATION"
                                        Caption="Flr|Rm|Cab" ToolTip="Floor Number | Room Number | Cabinet Number">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="createuser" Caption="Upload By">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Verified By" FieldName="vrfy" ReadOnly="True"
                                        VisibleIndex="12">
                                        <EditFormSettings Visible="False" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="RecieveDate" Caption="Rcv.Date">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="RenewDate" Caption="Renewal Date">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <%-- <dxe:GridViewDataHyperLinkColumn   VisibleIndex="14"
                                            Width="15%">
                                            <DataItemTemplate>
                                                <a onclick="OnDocumentView('<%#Eval("doc_source") %>')" style="cursor: pointer;">View</a>
                                            </DataItemTemplate>
                                            <HeaderCaptionTemplate>View</HeaderCaptionTemplate>
                                        </dxe:GridViewDataHyperLinkColumn>--%>
                                </Columns>
                                <SettingsText ConfirmDelete="Confirm delete?" />
                                <StylesEditors>
                                    <ProgressBar Height="25px">
                                    </ProgressBar>
                                </StylesEditors>
                                <%--  <Settings ShowHorizontalScrollBar="True" />--%>
                                <SettingsSearchPanel Visible="True" />
                                <Settings ShowGroupedColumns="True" ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            </dxe:ASPxGridView>
                            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" CloseAction="CloseButton"
                                Top="100" Left="400" ClientInstanceName="popup" Height="500px" ContentUrl="frmAddDocuments.aspx"
                                Width="900px" HeaderText="Document View" AllowResize="true" ResizingMode="Postponed">
                                <ContentCollection>
                                    <dxe:PopupControlContentControl runat="server">
                                    </dxe:PopupControlContentControl>
                                </ContentCollection>
                                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                            </dxe:ASPxPopupControl>
                            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                            </dxe:ASPxGridViewExporter>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="display: none;">
                    <asp:Button ID="btnEntity" runat="server" Text="Button" OnClick="btnEntity_Click" />
                    <asp:Button ID="btnDt" runat="server" Text="Button" OnClick="btnDt_Click" />
                    <asp:HiddenField ID="HdnClients" runat="server" />
                    <asp:HiddenField ID="HdnGroup" runat="server" />
                    <asp:HiddenField ID="HdnBranchId" runat="server" />
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                    <asp:HiddenField ID="HiddenField_EntityChange" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
