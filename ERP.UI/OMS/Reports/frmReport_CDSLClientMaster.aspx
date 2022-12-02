<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_CDSLClientMaster" CodeBehind="frmReport_CDSLClientMaster.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

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
            function FunTemplate(objID, objListFun, objEvent) {
                ajax_showOptions(objID, objListFun, objEvent);
            }
            function ChkCheckProperty(obj, objChk) {
                if (objChk == true) {
                    if (obj == 'H') {
                        document.getElementById("tdHeader").style.display = "inline";//  Show('tdHeader');
                    }
                    else if (obj == 'F') {
                        document.getElementById("tdHeader").style.display = "none";
                    }
                    // Hide('tdHeader');
                }
                else {
                    document.getElementById("tdHeader").style.display = "none";
                    // Hide('tdHeader');
                }


            }

            function AddSignature(obj, objChk) {
                //alert(obj);
                if (objChk == true) {
                    document.getElementById("tdAddSig").style.display = "inline";
                }
                else {
                    document.getElementById("tdAddSig").style.display = "none";
                }

            }

            function FunAddSig(objID, objListFun, objEvent) {
                ajax_showOptions(objID, objListFun, objEvent);
            }
            function ChkAddSig(obj) {
                if (obj == true)
                    Show('tdAddSig');
                else
                    Hide('tdAddSig');
            }

            function fn_ddllistType(obj) {
                if (obj == '0') {
                    document.getElementById('USig').style.display = 'none';
                    document.getElementById('UTemplate').style.display = 'none';
                    document.getElementById('btnPrint').style.display = 'none';
                    document.getElementById('btnShow').style.display = 'inline';

                }
                if (obj == '1') {

                    document.getElementById('USig').style.display = 'inline';
                    document.getElementById('UTemplate').style.display = 'inline';
                    document.getElementById('btnPrint').style.display = 'inline';
                    document.getElementById('btnShow').style.display = 'none';



                }
            }

            function height() {
                if (document.body.scrollHeight >= 750) {
                    window.frameElement.height = document.body.scrollHeight;
                }
                else {
                    window.frameElement.height = '700';
                }
                window.frameElement.width = document.body.scrollWidth;
            }

            function SignOff() {
                window.parent.SignOff();
            }
            function ShowDate(obj) {
                if (obj == 'a') {
                    divAsOnDate.innerText = "Account Activation Date :";
                }
                else if (obj == 'c') {
                    divAsOnDate.innerText = "Account Closure Date :";
                }
            }

            function ShowDateRange(obj) {
                if (obj == 'a') {
                    document.getElementById("trDateRange").style.display = "none";

                }
                else if (obj == 's') {
                    document.getElementById("trDateRange").style.display = "inline";

                }

            }
            function Page_Load() {

                divAsOnDate.innerText = "Account Activation Date :";
                document.getElementById('TdFilter').style.display = 'none';
                document.getElementById('TdSelect').style.display = 'none';
                document.getElementById('TrButton').style.display = 'inline';
                document.getElementById('TdExport').style.display = 'none';
                document.getElementById('TdFilter1').style.display = 'none';
                document.getElementById('ShowSubType').style.display = 'none';
                document.getElementById("trDateRange").style.display = "none";

                document.getElementById("tdAddSig").style.display = "none";
                document.getElementById("tdHeader").style.display = "none";

                document.getElementById('USig').style.display = 'none';
                document.getElementById('UTemplate').style.display = 'none';
                document.getElementById('btnPrint').style.display = 'none';
                document.getElementById('btnShow').style.display = 'inline';
                Hide('tdHeader');
                Hide('tdAddSig');


            }




            function AllSelct(obj, obj1) {


                var FilTer = document.getElementById('cmbsearchOption');
                if (obj != 'a') {

                    if (obj1 == 'T') {
                        FilTer.value = 'Type';
                    }
                    else if (obj1 == 'S') {
                        FilTer.value = 'SubType';
                    }
                    else if (obj1 == 'C') {
                        FilTer.value = 'Client';
                    }

                    document.getElementById('TdFilter').style.display = 'inline';
                    document.getElementById('TdFilter1').style.display = 'inline';
                    document.getElementById('TdSelect').style.display = 'inline';
                }
                else {
                    if (obj1 == 'M') {
                        document.getElementById('TrForGroup').style.display = 'none';
                        document.getElementById('TrForClient').style.display = 'none';

                    }
                    document.getElementById('TdFilter').style.display = 'none';
                    document.getElementById('TdFilter1').style.display = 'none';
                    document.getElementById('TdSelect').style.display = 'none';
                }
            }

            function ShowGrid() {
                document.getElementById('TdGrid').style.display = 'inline';
                document.getElementById('HeaderGrid').style.display = 'inline';
                document.getElementById('TdExport').style.display = 'inline';
                document.getElementById('TrAll').style.display = 'none';
                document.getElementById('TrButton').style.display = 'none';
                height();
            }
            function Filter() {
                document.getElementById('TrAll').style.display = 'inline';
                document.getElementById('TrButton').style.display = 'inline';
                height();
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


            function FunClientScrip(objID, objListFun, objEvent) {
                var cmbVal;

                if (document.getElementById('cmbsearchOption').value == "SubType") {
                    cmbVal = document.getElementById('cmbsearchOption').value;
                    cmbVal = cmbVal + '~' + document.getElementById('HDNType').value;
                }
                else if (document.getElementById('cmbsearchOption').value == "Client") {
                    cmbVal = document.getElementById('cmbsearchOption').value;
                    cmbVal = cmbVal + '~C';
                }
                else {
                    cmbVal = document.getElementById('cmbsearchOption').value;
                    cmbVal = cmbVal + '~N';
                }
                ajax_showOptions(objID, objListFun, objEvent, cmbVal);
            }

            function selecttion() {
                var combo = document.getElementById('ddlExport');
                combo.value = 'Ex';
            }

            function ReceiveServerData(rValue) {
                var Data = rValue.split('~');
                if (Data[0] == 'Type') {
                    Mainvalue = Data[1];
                    document.getElementById('HDNType').value = Data[1];
                    var TypVal = Mainvalue.split(',');
                    var i = TypVal.length;
                    if (i < 2) {
                        document.getElementById('ShowSubType').style.display = 'inline';
                    }
                    else {
                        document.getElementById('ShowSubType').style.display = 'none';
                    }

                }
                if (Data[0] == 'SubType') {

                    groupvalue = Data[1];
                    alert("123");
                    alert(groupvalue);
                    document.getElementById('HDNSubType').value = Data[1];
                }
                if (Data[0] == 'Client') {

                    clientvalue = Data[1];
                    document.getElementById('HDNClient').value = Data[1];
                }

            }

            function Callheight() {
                height();
            }

            function ShowHideFilter(obj) {
                grid.PerformCallback(obj);
            }

            function ValidatePage() {
                alert("123");
            }
            function MaskMoney(evt) {
                return false;
            }

            function Groupselect(checkvalue) {

                if (checkvalue == true)
                    document.getElementById('trGroupType').style.display = 'inline';
                else
                    document.getElementById('trGroupType').style.display = 'none';
            }
            FieldName = 'lstSuscriptions';

            </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
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
                    <strong><span style="color: #000099">CDSL Client Master</span></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr id="TrAll">
                            <td>
                                <table class="TableMain100">
                                    <tr>
                                        <td>
                                            <table cellspacing="1" cellpadding="2" style="background-color: #B7CEEC; border: solid 1px  #ffffff"
                                                border="1">
                                                <tr id="TrDateCheck">
                                                    <td class="gridcellleft">Account Status :
                                                    </td>
                                                    <td class="gridcellleft">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="RadAll" runat="server" GroupName="k1" Checked="true" onclick="ShowDate('a')" /></td>
                                                                <td class="gridcellleft">All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="RadActive" runat="server" GroupName="k1" onclick="ShowDate('a')" /></td>
                                                                <td class="gridcellleft">Active
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="RadClosed" runat="server" GroupName="k1" onclick="ShowDate('c')" /></td>
                                                                <td class="gridcellleft">Closed
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="TrDate">
                                                    <td class="gridcellleft">
                                                        <div id="divAsOnDate">
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
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
                                                                    <table>
                                                                        <tr>
                                                                            <td id="TdFrom">
                                                                                <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" ClientInstanceName="dtFrom"
                                                                                    UseMaskBehavior="True" Width="108px">
                                                                                    <DropDownButton Text="From">
                                                                                    </DropDownButton>
                                                                                    <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />
                                                                                </dxe:ASPxDateEdit>
                                                                            </td>
                                                                            <td id="TdTo">
                                                                                <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                                                                    UseMaskBehavior="True" Width="108px">
                                                                                    <DropDownButton Text="To">
                                                                                    </DropDownButton>
                                                                                    <ClientSideEvents DateChanged="function(s,e){DateChangeForTo();}" />
                                                                                </dxe:ASPxDateEdit>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trClient">
                                                    <td class="gridcellleft">Select Client
                                                    </td>
                                                    <td align="left">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="RadClientAll" runat="server" Checked="True" GroupName="p1" onclick="AllSelct('a','C')" />
                                                                </td>
                                                                <td>All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="RadClientSelected" runat="server" GroupName="p1" onclick="AllSelct('b','C')" />
                                                                </td>
                                                                <td>Specific
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" runat="server" style="color: Maroon"></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="TrAccount">
                                                    <td class="gridcellleft">Account Type
                                                    </td>
                                                    <td align="left">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbMainAll" runat="server" Checked="True" GroupName="a12" onclick="AllSelct('a','T')" />
                                                                </td>
                                                                <td>All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbMainSelected" runat="server" GroupName="a12" onclick="AllSelct('b','T')" />
                                                                </td>
                                                                <td>Selected
                                                                </td>
                                                                <td>
                                                                    <span id="litSegment" runat="server" style="color: Maroon"></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="ShowSubType">
                                                    <td class="gridcellleft">Sub Type</td>
                                                    <td align="left">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdAllSegment" runat="server" Checked="True" GroupName="c1" onclick="AllSelct('a','S')" />
                                                                </td>
                                                                <td>All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdSelSegment" runat="server" GroupName="c1" onclick="AllSelct('b','S')" />
                                                                </td>
                                                                <td>Selected
                                                                </td>
                                                                <td>
                                                                    <span id="Span2" runat="server" style="color: Maroon"></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="gridcellleft">Beneficiary A/C Category
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlCat" runat="server" Font-Size="12px" Width="200px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="gridcellleft">Beneficiary Occupation
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlOcp" runat="server" Font-Size="12px" Width="200px">
                                                            <asp:ListItem Text="--ALL--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Business" Value="B"></asp:ListItem>
                                                            <asp:ListItem Text="House Wife" Value="H"></asp:ListItem>
                                                            <asp:ListItem Text="Professional" Value="P"></asp:ListItem>
                                                            <asp:ListItem Text="Service" Value="S"></asp:ListItem>
                                                            <asp:ListItem Text="Student" Value="ST"></asp:ListItem>
                                                            <asp:ListItem Text="Others" Value="O"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="TrShowPhnumber">
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="ChkAHolder" runat="server" />
                                                        Show Accounts Without Additional Holders
                                                    </td>
                                                </tr>
                                                <tr id="TrShowBranchNet">
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="ChkPOA" runat="server" />
                                                        Show Only POA Enabled Accounts
                                                    </td>
                                                </tr>
                                                <tr id="Tr1">
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkMinor" runat="server" />
                                                        Show Minor Accounts
                                                    </td>
                                                </tr>
                                                <tr id="Tr2">
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkNom" runat="server" />
                                                        Show Accounts Without Nomination
                                                    </td>
                                                </tr>
                                                <tr id="Tr3">
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkMinorNom" runat="server" />
                                                        Show POA Details
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkGroup" onclick="javascript:Groupselect(this.checked)" runat="server" />
                                                        Select Group
                                                    </td>

                                                </tr>
                                                <tr id="trGroupType">
                                                    <td class="gridcellleft">Group Type
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drpGroupType" Width="120px" Font-Size="12px" runat="Server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="gridcellleft">Report Type
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="ddllistType" runat="server" Width="120px" Font-Size="12px"
                                                            onchange="fn_ddllistType(this.value)">
                                                            <asp:ListItem Value="0">Screen</asp:ListItem>
                                                            <asp:ListItem Value="1">Print</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="UTemplate">
                                                    <td>
                                                        <asp:CheckBox ID="chkHeader" runat="server" onclick="ChkCheckProperty('H',this.checked);" />
                                                        Use Template
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>

                                                                <td id="tdHeader">
                                                                    <asp:TextBox ID="txtHeader" runat="server" Width="279px" Font-Size="12px" onkeyup="FunTemplate(this,'GetTeplate',event)"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="USig">
                                                    <td>
                                                        <asp:CheckBox ID="ChkSignatory" runat="server" onclick="AddSignature('S',this.checked);" />
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
                                                                        <asp:ListItem>Client</asp:ListItem>
                                                                        <asp:ListItem>Type</asp:ListItem>
                                                                        <asp:ListItem>SubType</asp:ListItem>
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td id="TdFilter" style="height: 23px">
                                                                    <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="253" onkeyup="FunClientScrip(this,'ShowCDSLClientTypeSubType',event)"></asp:TextBox><a
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
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr id="TrButton">
                            <td id="TrBtn">
                                <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btnUpdate" OnClientClick="selecttion()"
                                    OnClick="btnShow_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnPrint" Text="Print" runat="server" CssClass="btnUpdate" OnClick="btnPrint_Click"
                                    TabIndex="6" />
                            </td>
                            <td style="display: none">
                                <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                    <ProgressTemplate>
                                        <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50%; background-color: white; layer-background-color: white; height: 80; width: 150;'>
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
                <td style="text-align: right" id="TdExport">
                    <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                        <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                        <asp:ListItem Value="P">PDF</asp:ListItem>
                    </asp:DropDownList>
                    <a id="A3" href="javascript:void(0);" onclick="Filter()"><span style="color: #009900; text-decoration: underline; font-size: 8pt;">Filter</span></a>
                </td>
            </tr>
            <tr style="background-color: #DDECFE;">
                <td style="background-color: #DDECFE;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="background-color: #DDECFE;" class="TableMain100">
                                <tr>
                                    <td id="HeaderGrid" style="height: 10px; font-size: 12px; font-weight: bold;">
                                        <asp:Label ID="lblReportHeader" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top">
                                        <table>
                                            <tr>
                                                <td id="ShowFilter">
                                                    <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                                                </td>
                                                <td id="Td1">
                                                    <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TdGrid">
                                        <div style="width: 970px; overflow: scroll;">
                                            <dxe:ASPxGridView ID="EmployeeGrid" runat="server" KeyFieldName="BenAccountID"
                                                AutoGenerateColumns="False" Width="100%" ClientInstanceName="grid" OnCustomCallback="EmployeeGrid_CustomCallback"
                                                OnHtmlRowCreated="EmployeeGrid_HtmlRowCreated" OnPageIndexChanged="EmployeeGrid_PageIndexChanged">
                                                <ClientSideEvents ColumnResizing="function(s, e) { Callheight();
                                                        }" />
                                                <SettingsBehavior ColumnResizeMode="NextColumn" AllowFocusedRow="True" ConfirmDelete="True" />
                                                <Styles>
                                                    <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

                                                    <FocusedRow BackColor="#FFC0C0" Cursor="auto"></FocusedRow>

                                                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>

                                                    <Row Wrap="False"></Row>

                                                    <FocusedGroupRow BackColor="#FFC0C0"></FocusedGroupRow>
                                                </Styles>
                                                <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20">
                                                    <FirstPageButton Visible="True"></FirstPageButton>

                                                    <LastPageButton Visible="True"></LastPageButton>
                                                </SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="0" FieldName="FirstHolderName" FixedStyle="Left" Width="250px">
                                                        <DataItemTemplate>
                                                            <dxe:ASPxLabel ID="ASPxTextBox1" runat="server" Text='<%# Eval("FirstHolderName")%>' Width="100%">
                                                            </dxe:ASPxLabel>

                                                        </DataItemTemplate>

                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="BenAccountID" FixedStyle="Left" Width="75px" Caption="Ben. AccountID">
                                                        <DataItemTemplate>
                                                            <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("BenAccountID")%>' Width="100%">
                                                            </dxe:ASPxLabel>

                                                        </DataItemTemplate>

                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="SecondHolderName" Width="150px" Caption="Second Holder Name">
                                                        <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ThirdHolderName" Width="150px">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Branch" Width="150px">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="GroupDetails" Caption="Group Details" Width="150px">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="6" FieldName="BenAccountStatus" Width="75px" Caption="A/C Status">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="BenAccountCategory" Width="150px" Caption="A/C Category">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="BoStatus" Width="150px" Caption="BO Status">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="BoSubStatus" Width="150px" Caption="BO Sub Status">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="10" FieldName="TradingCode" Width="75px" Caption="Trading Code">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="11" FieldName="ActivationDate" Width="80px" Caption="Activation Date">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="12" FieldName="ClosureDate" Width="80px" Caption="Closure Date">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="13" FieldName="FirstHolderAddress" Width="250px" Caption="First Holder Address">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataDateColumn ReadOnly="True" VisibleIndex="14" FieldName="FirstHolderEmail" Width="150px" Caption="First Holder Email">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataDateColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="15" FieldName="FirstHolderFatherHusbandName" Width="150px" Caption="First Holder Father/Husband Name">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="16" FieldName="PhoneNumber" Width="75px" Caption="First Holder Mobile">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="17" FieldName="PermanentPhone" Width="150px" Caption="First Holder Phone">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="18" FieldName="FirstHolderPAN" Width="75px" Caption="First Holder PAN">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="19" FieldName="SecondHolderPAN" Width="75px" Caption="Second Holder PAN">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="20" FieldName="ThirdHolderPAN" Width="75px" Caption="Third Holder PAN">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--<dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="18" FieldName="NomineeType" Width="150px" Caption="Nominee Type">
<CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
</dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="21" FieldName="NomineeName" Width="150px" Caption="Nominee/Gurdian Name">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="22" FieldName="NOMINEEAddress" Width="250px" Caption="Nominee Address ">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--<dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="21" FieldName="MinorNomineeGurdianName" Width="250px" Caption="Minor Nominee Gurdian Name">
<CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
</dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="23" FieldName="AccNo" Width="200px" Caption="Bank A/C No.">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="24" FieldName="MICR" Caption="MICR ">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="25" FieldName="ECEDevidend" Width="200px" Caption="Electronic Devidend">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="26" FieldName="BankName" Width="200px" Caption="Bank Name">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="27" FieldName="BenBankCode" Width="200px" Caption="Ben. Bank Code">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="28" FieldName="BenAccountNo" Caption="Ben. Bank A/C" Width="200px">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="29" FieldName="IsPOA" Caption="POA" Width="200px">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="30" FieldName="CdslClients_POAID" Width="200px" Caption="POA ID">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="31" FieldName="CdslClients_POAName" Width="200px" Caption="POA Name">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="32" FieldName="POAStartDate" Caption="POA Start Date" Width="200px">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="33" FieldName="POAEndDate" Caption="POA End Date" Width="200px">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="34" FieldName="CDSLCLIENTS_boid" Caption="CDSLCLIENTS_boid" Width="200px">
                                                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <Settings ShowGroupPanel="True"></Settings>
                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                            </dxe:ASPxGridViewExporter>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="display: none;">
                    <asp:HiddenField ID="HDNType" runat="server" />
                    <asp:HiddenField ID="HDNSubType" runat="server" />
                    <asp:HiddenField ID="HDNClient" runat="server" />
                    <asp:HiddenField ID="txtSignature_hidden" runat="server" />
                    <asp:HiddenField ID="txtHeader_hidden" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
