<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_netpositionanalysis" CodeBehind="netpositionanalysis.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

    <style type="text/css">
        .tableClass {
            /* border: 0px; */
            border: 1px solid #aaa !important;
            border-collapse: collapse !important;
        }

        .tableBorderClass {
            /* border: 0px; */
            border: 1px solid #aaa !important;
        }
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
            Hide('showFilter');
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
            var cmbVal;

            if (document.getElementById('cmbsearchOption').value == "Clients") {
                if (document.getElementById('ddlGroup').value == "0" || document.getElementById('ddlGroup').value == "2")//////////////Group By  selected are branch
                {
                    if (document.getElementById('ddlGroup').value == "0") {
                        if (document.getElementById('rdbranchAll').checked == true) {
                            cmbVal = 'ClientsBranch' + '~' + 'ALL';
                        }
                        else {
                            cmbVal = 'ClientsBranch' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Branch').value;
                        }
                    }
                    if (document.getElementById('ddlGroup').value == "2") {
                        if (document.getElementById('rdbranchAll').checked == true) {
                            cmbVal = 'ClientsBranchGroup' + '~' + 'ALL';
                        }
                        else {
                            cmbVal = 'ClientsBranchGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_BranchGroup').value;
                        }
                    }
                }
                else //////////////Group By selected are Group
                {
                    if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                        cmbVal = 'ClientsGroup' + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                    }
                    else {
                        cmbVal = 'ClientsGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Group').value;
                    }
                }
            }

            else {
                cmbVal = document.getElementById('cmbsearchOption').value;
                cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
            }

            ajax_showOptions(objID, 'ShowClientFORMarginStocks', objEvent, cmbVal);

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
        function fn_Scrip(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Scrips';
                Show('showFilter');
            }
            selecttion();
            height();
        }
        function fnBranch(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                if (document.getElementById('ddlGroup').value == "0") {
                    document.getElementById('cmbsearchOption').value = 'Branch';
                }
                if (document.getElementById('ddlGroup').value == "2") {
                    document.getElementById('cmbsearchOption').value = 'BranchGroup';
                }

                Show('showFilter');
            }
            selecttion();
        }
        function fnGroup(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Group';
                Show('showFilter');
            }
            selecttion();
        }
        function fnSegment(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Segment';
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
            document.getElementById('btnScreen').disabled = false;
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
            if (obj == "0" || obj == "2") {
                Hide('td_group');
                Show('td_branch');
            }

            else {
                Show('td_group');
                Hide('td_branch');
                var btn = document.getElementById('btnhide');
                btn.click();
            }
            selecttion();
        }
        function fngrouptype(obj) {
            if (obj == "0") {
                Hide('td_allselect');
                alert('Please Select Group Type !');
            }
            else {
                Show('td_allselect');
            }
            selecttion();
        }

        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = 'Ex';
        }
        function AlertRecord(obj) {
            if (obj == '1')
                alert("You Have To Chcek Atleast Three Columns !!");
            if (obj == '2')
                alert("No Record Found !!");

        }
        FieldName = 'lstSlection';
    </script>


    <script type="text/javascript">
        function SelectAllCheckboxes(chk) {
            $('#<%=chktfilter.ClientID %>').find("input:checkbox").each(function () {
             if (this != chk) {
                 this.checked = chk.checked;
             }
         });
     }
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
            if (j[0] == 'Scrips') {
                document.getElementById('HiddenField_Scrips').value = j[1];
            }
            if (j[0] == 'BranchGroup') {
                document.getElementById('HiddenField_BranchGroup').value = j[1];
            }
            if (j[0] == 'Segment') {
                document.getElementById('HiddenField_Segment').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">Net Position Analysis</span></strong></td>


            </tr>
        </table>

        <table id="tab1" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
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
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report View :</td>
                                        <td>
                                            <asp:DropDownList ID="DLLRptView" runat="server" Width="250px" Font-Size="12px">
                                                <asp:ListItem Value="1">Client + Date +Scrip Wise</asp:ListItem>
                                                <asp:ListItem Value="2">Client + Scrip Wise</asp:ListItem>

                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_GroupBy">
                            <td class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Group By</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                <asp:ListItem Value="0">Branch</asp:ListItem>
                                                <asp:ListItem Value="1">Group</asp:ListItem>
                                                <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" id="td_branch">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="fnBranch('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="fnBranch('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="td_group" style="display: none;" colspan="2">
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
                                                        <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="b"
                                                            onclick="fnGroup('a')" />
                                                        All
                                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="fnGroup('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Clients :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="fnClients('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdPOAClient" runat="server" GroupName="c" onclick="fnClients('a')" />POA
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
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Segment :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentAll" runat="server" GroupName="ab" onclick="fnSegment('a')" />All
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButton ID="rdbSegmentSpecific" runat="server" Checked="True" GroupName="ab" onclick="fnSegment('a')" />Specific
                                [ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentSelected" runat="server" GroupName="ab" onclick="fnSegment('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Scrip :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbScripAll" runat="server" Checked="True" GroupName="cd" onclick="fn_Scrip('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbScripSelected" runat="server" GroupName="cd" onclick="fn_Scrip('b')" />
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
                                        <td class="gridcellleft">
                                            <asp:CheckBox ID="chkAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);"
                                                Checked="true" /><span style="font-family: Verdana; color: Teal; font-size: x-small;"><b>Check/UnCheck
                                                                    ALL</b></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; border: 1px black solid; scrollbar-base-color: #C0C0C0">
                                                <asp:CheckBoxList ID="chktfilter" runat="server" RepeatDirection="Horizontal" Width="600px"
                                                    RepeatColumns="10">
                                                    <asp:ListItem Value="Buy Qty" Selected="True">Buy Qty</asp:ListItem>
                                                    <asp:ListItem Value="Buy Value" Selected="True">Buy Value</asp:ListItem>
                                                    <asp:ListItem Value="Buy Avg" Selected="True">Buy Avg</asp:ListItem>
                                                    <asp:ListItem Value="Sell Qty" Selected="True">Sell Qty</asp:ListItem>
                                                    <asp:ListItem Value="Sell Value" Selected="True">Sell Value</asp:ListItem>
                                                    <asp:ListItem Value="Sell Avg" Selected="True">Sell Avg</asp:ListItem>
                                                    <asp:ListItem Value="Open BuyQty" Selected="True">Open BuyQty</asp:ListItem>
                                                    <asp:ListItem Value="Open SellQty" Selected="True">Open SellQty</asp:ListItem>
                                                    <asp:ListItem Value="Net Value" Selected="True">Net Value</asp:ListItem>
                                                    <asp:ListItem Value="Close Price" Selected="True">Close Price</asp:ListItem>
                                                    <asp:ListItem Value="Exposure" Selected="True">Exposure</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
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
                                                Width="101px" OnClientClick="selecttion()" OnClick="btnExcel_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                    </table>
                </td>
                <td>
                    <table class="tableClass" cellpadding="1" cellspacing="1" id="showFilter">
                        <tr>
                            <td id="TdFilter">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem>Clients</asp:ListItem>
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                    <asp:ListItem>Product</asp:ListItem>
                                    <asp:ListItem>BranchGroup</asp:ListItem>
                                    <asp:ListItem>Scrips</asp:ListItem>
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
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnhide" runat="server" Text="btnhide" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Scrips" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
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
