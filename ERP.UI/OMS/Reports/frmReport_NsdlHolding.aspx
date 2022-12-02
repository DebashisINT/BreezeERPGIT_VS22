<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_NsdlHolding" EnableEventValidation="false" Codebehind="frmReport_NsdlHolding.aspx.cs" %>


<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
    groupvalue = "";
    function ForFilterOff() {
        hide('filter');
        show('btnfilter');
        height();
    }
    function MailsendT() {
        alert("Mail Sent Successfully");
    }
    function MailsendF() {
        alert("Error on sending!Try again..");
    }
    function SignOff() {
        window.parent.SignOff();
    }
    function height() {
        if (document.body.scrollHeight >= 500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '500px';

        window.frameElement.Width = document.body.scrollWidth;
    }
    function PageLoad() {
        FieldName = 'SelectionList';
        document.getElementById('txtName_hidden').style.display = "none";
        ShowEmployeeFilterForm('A');
        document.getElementById('txtISIN_hidden').style.display = "none";
        ShowISINFilterForm('A');
        document.getElementById('txtSettlement_hidden').style.display = "none";
        ShowSettlementFilterForm('A');
        hide('btnfilter');
        hide('ShowSelectUser');
        document.getElementById('showFilter1').style.display = "none";
    }
    function ShowEmployeeFilterForm(obj) {
        document.getElementById('txtName_hidden').value = "";
        document.getElementById('txtName').value = "";
        if (obj == 'A') {
            hide('ShowFilterByAjax');

            document.getElementById('txtName_hidden').style.display = "none";
        }
        if (obj == 'S') {
            var cmb = document.getElementById('cmbsearch');
            cmb.value = 'Clients';
            show('ShowFilterByAjax');

            document.getElementById('txtName_hidden').style.display = "none";
            document.getElementById('txtName').focus();
        }
    }
    function NoOfRows(obj) {
        Noofrows = obj;
        document.getElementById('txtName_hidden').style.display = "none";
    }
    function show(obj1) {
        if (!document.getElementById(obj1))
            alert('show-' + obj1);
        else
            document.getElementById(obj1).style.display = 'inline';
    }
    function hide(obj1) {
        //         if(!document.getElementById(obj1))
        //               alert('hide-'+obj1);
        //         else  
        document.getElementById(obj1).style.display = 'none';
    }
    FieldName = 'SelectionList';
    function CallAjax(obj1, obj2, obj3) {
        var cmbTime = document.getElementById('txtDate_I');
        var cmbType = document.getElementById('ASPxComboBox1_VI');
        var Client = document.getElementById('txtName_hidden');
        var isin = document.getElementById('txtISIN_hidden');

        var obj4 = cmbTime.value + '~' + cmbType.value + '~' + Client.value + '~' + isin.value;
        ajax_showOptions(obj1, obj2, obj3, obj4);
    }
    function ShowISINFilterForm(obj) {
        document.getElementById('txtISIN_hidden').value = "";
        document.getElementById('txtISIN').value = "";
        if (obj == 'A') {
            hide('tdtxtISIN');
            //hide('tdISIN');
            document.getElementById('txtISIN_hidden').style.display = "none";
        }
        if (obj == 'S') {
            show('tdtxtISIN');
            //show('tdISIN');
            document.getElementById('txtISIN_hidden').style.display = "none";
            document.getElementById('txtISIN').focus();
        }
    }
    function ShowSettlementFilterForm(obj) {
        document.getElementById('txtSettlement_hidden').value = "";
        document.getElementById('txtSettlement').value = "";
        if (obj == 'A') {
            hide('tdtxtSettlement');
            //hide('tdSettlement');
            document.getElementById('txtSettlement_hidden').style.display = "none";
        }
        if (obj == 'S') {
            show('tdtxtSettlement');
            //show('tdSettlement');
            document.getElementById('txtSettlement_hidden').style.display = "none";
            document.getElementById('txtSettlement').focus();
        }
    }
    function OnClientTypeChanged(s, e) {
        document.getElementById('txtSettlement_hidden').value = "";
        document.getElementById('txtSettlement').value = "";
        var item = s.GetSelectedItem();
        if (item.text == 'Clearing Members' || item.text == 'All') {
            show('tdSettlementLabel');
            show('tdrbSettlement');
            ShowSettlementFilterForm('A');
        }
        else {
            hide('tdSettlementLabel');
            hide('tdrbSettlement');
            hide('tdtxtSettlement');
            //hide('tdSettlement');
        }
        radioSettlement.SetSelectedIndex(0);
    }
    function ShowHideFilter(obj) {
        grid.PerformCallback(obj);
    }
    function btnAddEmailtolist_click() {
        var cmb = document.getElementById('cmbsearch');
        var userid = document.getElementById('txtName');
        if (userid.value != '') {
            var ids = document.getElementById('txtName_hidden');
            var listBox = document.getElementById('SelectionList');
            var tLength = listBox.length;

            var no = new Option();
            no.value = ids.value;
            no.text = userid.value;
            listBox[tLength] = no;
            var recipient = document.getElementById('txtName');
            recipient.value = '';
        }
        else
            alert('Please search name and then Add!')
        var s = document.getElementById('txtName');
        s.focus();
        s.select();
    }
    function callAjax1(obj1, obj2, obj3) {
        document.getElementById('SelectionList').style.display = 'none';
        var combo = document.getElementById("cmbsearch");
        var set_value = combo.value
        var obj4 = 'Main';

        if (set_value == '16') {
            ajax_showOptions(obj1, 'GetLeadId', obj3, set_value, obj4)
        }
        else {
            ajax_showOptions(obj1, obj2, obj3, set_value, obj4)
        }
    }
    function clientselection() {
        selecttion();
        var listBoxSubs = document.getElementById('SelectionList');
        var cmb = document.getElementById('cmbsearch');

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
            CallServer1(sendData, "");
            hide('ShowFilterByAjax');
        }
        else {
            alert("Please select email from list.")
        }
        var i;
        for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
            listBoxSubs.remove(i);
        }
        if (cmb.value == "User") {
            document.getElementById('showFilter1').style.display = "inline";
            document.getElementById('ShowSelectUser').style.display = "none";
        }
        height();
    }
    function btnRemoveEmailFromlist_click() {
        selecttion();
        var listBox = document.getElementById('SelectionList');
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
    function ReceiveSvrData(rValue) {
        var j = rValue.split('~');
        if (j[0] == 'Group') {
            groupvalue = j[1];
            document.getElementById('hidGroup').value = j[1];
        }
        if (j[0] == 'Branch') {
            groupvalue = j[1];
            document.getElementById('hidBranch').value = j[1];
        }
        if (j[0] == 'Clients') {
            document.getElementById('hidClients').value = j[1];
        }
        var btn = document.getElementById('btnhide');
        btn.click();
    }
    function selecttion() {
        var combo = document.getElementById('ddlExport');
        combo.value = 'Ex';
    }
    //-------------Select Groupwise and branchwise        
    function fnddlGroup(obj) {
        if (obj == "0") {
            document.getElementById('td_group').style.display = "none";
            document.getElementById('td_branch').style.display = "inline";
            document.getElementById('ddlgrouptype').value = "";
        }
        else {
            document.getElementById('td_group').style.display = "inline";
            document.getElementById('td_branch').style.display = "none";
            var btn = document.getElementById('btnhide');
            btn.click();
        }
    }
    function Branch(obj) {
        if (obj == "a") {
            hide('ShowFilterByAjax');

            document.getElementById('txtName_hidden').style.display = "none";
        }
        else {
            var cmb = document.getElementById('cmbsearch');
            cmb.value = 'Branch';
            show('ShowFilterByAjax');

            document.getElementById('txtName_hidden').style.display = "none";
            document.getElementById('txtName').focus();
        }
    }
    function Group(obj) {
        if (obj == "a") {
            hide('ShowFilterByAjax');

            document.getElementById('txtName_hidden').style.display = "none";
        }
        else {
            var cmb = document.getElementById('cmbsearch');
            cmb.value = 'Group';

            show('ShowFilterByAjax');

            document.getElementById('txtName_hidden').style.display = "none";
            document.getElementById('txtName').focus();
        }
    }
    function fngrouptype(obj) {
        if (obj == "0") {
            document.getElementById('td_allselect').style.display = "neone";
            alert('Please Select Group Type !');
        }
        else {
            document.getElementById('td_allselect').style.display = "inline";
        }
    }
    function FunClientScrip(objID, objListFun, objEvent) {
        if (document.getElementById('cmbsearch').value == "User") {
            ajax_showOptions(objID, 'GetMailId', objEvent, 'EM');
        }
        else {
            var cmbVal;
            var cmbTime = document.getElementById('txtDate_I');
            var cmbType = document.getElementById('ASPxComboBox1_VI');
            var Client = document.getElementById('txtName_hidden');
            var isin = document.getElementById('txtISIN_hidden');
            var GtCL = cmbTime.value + '~' + cmbType.value + '~' + Client.value + '~' + isin.value
            if (groupvalue == "") {
                if (document.getElementById('cmbsearch').value == "Clients") {
                    cmbVal = document.getElementById('cmbsearch').value;
                    cmbVal = cmbVal + '~' + GtCL + document.getElementById('ddlgrouptype').value;
                }
                else {
                    cmbVal = document.getElementById('cmbsearch').value;
                    cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
                }
            }
            else {
                if (document.getElementById('cmbsearch').value == "Clients") {
                    if (document.getElementById('ddlGroup').value == "0")//////////////Group By  selected are branch
                    {
                        if (document.getElementById('rdbranchAll').checked == true) {
                            cmbVal = document.getElementById('cmbsearch').value + 'Branch';
                            cmbVal = cmbVal + '~' + GtCL + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                        }
                        else {
                            cmbVal = document.getElementById('cmbsearch').value + 'Branch';
                            cmbVal = cmbVal + '~' + GtCL + 'Selected' + '~' + groupvalue;
                        }
                    }
                    else //////////////Group By selected are Group
                    {
                        if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                            cmbVal = document.getElementById('cmbsearch').value + 'Group';
                            cmbVal = cmbVal + '~' + GtCL + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                        }
                        else {
                            cmbVal = document.getElementById('cmbsearch').value + 'Group';
                            cmbVal = cmbVal + '~' + GtCL + 'Selected' + '~' + groupvalue;
                        }
                    }
                }
                else {
                    cmbVal = document.getElementById('cmbsearch').value;
                    cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
                }
            }
            ajax_showOptions(objID, objListFun, objEvent, cmbVal);
        }
    }
    ///-----------Email New      
    function SelectUserClient(obj) {
        if (obj == 'Client') {

            document.getElementById('ShowSelectUser').style.display = 'inline';
            document.getElementById('ShowTable').style.display = 'none';
            document.getElementById('showFilter1').style.display = 'inline';
            window.frameElement.height = document.body.scrollHeight;

        }
        else if (obj == 'User') {
            document.getElementById('ShowTable').style.display = 'inline';
            document.getElementById('ShowSelectUser').style.display = 'inline';
            document.getElementById('showFilter1').style.display = 'none';
            window.frameElement.height = document.body.scrollHeight;
        }
    }
    function Sendmail() {
        document.getElementById('ShowSelectUser').style.display = 'inline';
        if (cRBReportView.GetValue() == "C") {
            if (document.getElementById('rbClientUser').checked == true) {
                document.getElementById('showFilter1').style.display = "none";
            }
            else {
                document.getElementById('showFilter1').style.display = "inline";
            }
            if (document.getElementById('ddlGroup').value == "0") {
                if (document.getElementById('rdbranchAll').checked == true) {
                    document.getElementById('tdBrGr').style.display = "none";
                    document.getElementById('tdCl').style.display = "inline";
                    document.getElementById('tdSU').style.display = "inline";
                }
                else {
                    document.getElementById('tdBrGr').style.display = "inline";
                    document.getElementById('tdCl').style.display = "inline";
                    document.getElementById('tdSU').style.display = "inline";
                    document.getElementById('lblSelectBrCl').value = 'Respective Branch';
                }
            }
            else if (document.getElementById('ddlGroup').value == "1") {
                if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                    document.getElementById('tdBrGr').style.display = "none";
                    document.getElementById('tdCl').style.display = "inline";
                    document.getElementById('tdSU').style.display = "inline";
                }
                else {
                    document.getElementById('tdBrGr').style.display = "inline";
                    document.getElementById('tdCl').style.display = "inline";
                    document.getElementById('tdSU').style.display = "inline";
                    document.getElementById('lblSelectBrCl').value = 'Respective Group';
                }
            }
        }
        else {
            document.getElementById('tdBrGr').style.display = 'none';
            document.getElementById('tdCl').style.display = 'none';
            document.getElementById('tdSu').style.display = 'inline';
        }
    }
    function User(obj) {
        if (obj == "User") {
            var cmb = document.getElementById('cmbsearch');
            cmb.value = 'User';
            show('ShowFilterByAjax');

            document.getElementById('txtName_hidden').style.display = "none";
            document.getElementById('txtName').focus();
            document.getElementById('showFilter1').style.display = "none";
        }
        else {
            document.getElementById('ShowFilterByAjax').style.display = "none";
            //document.getElementById('tdname').style.display="none";
            document.getElementById('showFilter1').style.display = "inline";
        }
        height();
    }
    function fn_ShowPageSearchFilter() {
        show('filter');
        hide('btnfilter');
        hide('ShowFilterByAjax');
        hide('ShowSelectUser');
        hide('showFilter1');
        height();
    }
</script>


        <div>
            <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"
                AsyncPostBackTimeout="3600">
            </asp:ScriptManager>
            <div class="TableMain100">
                <div class="EHEADER" style="text-align: center;">
                    <div style="width: 35%; float: right;" id="btnfilter">
                        <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged1">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                            <asp:ListItem Value="PM">PDF DIFF PAGES</asp:ListItem>
                        </asp:DropDownList>||
                        <input id="Button1" type="button" value="Show Filter" class="btnUpdate" onclick="fn_ShowPageSearchFilter(); "
                            style="width: 66px; height: 19px" />
                        || <a href="javascript:void(0);" onclick="Sendmail();"><span style="color: Blue;
                            text-decoration: underline; font-size: 8pt; font-weight: bold">Send Email</span></a>
                    </div>
                    <strong><span style="color: #000099">NSDL Holding Report</span></strong>
                </div>
            </div>
            <div id="ShowSelectUser" class="frmContent left" style="margin-top: 10px;">
                <div id="tdBrGr" class="left">
                    <asp:RadioButton ID="rbOnlyClient" runat="server" Checked="true" GroupName="h" />
                    <asp:TextBox ID="lblSelectBrCl" runat="server"></asp:TextBox>
                </div>
                <div id="tdCl" class="left">
                    <asp:RadioButton ID="rbRspctvClient" runat="server" Checked="true" GroupName="h" />
                    Respective Client
                </div>
                <div id="tdSU" class="left">
                    <asp:RadioButton ID="rbClientUser" runat="server" GroupName="h" />
                    Selected User
                </div>
            </div>
            <div id="showFilter1" class="left" style="margin-left: 10px; margin-top: 10px;">
                <asp:Button ID="btnMailSend" runat="server" OnClick="btnMailSend_Click" BorderWidth="1"
                    CssClass="btnUpdate" Text="Send" OnClientClick="javascript:selecttion();" />
            </div>
            <div class="pageContent">
                <div class="left">
                    <div id="filter" class="frmContent left" style="width: 500px;">
                        <div id="tblNoDate" runat="server" style="text-align: left; font-size: 11px; font-weight: bold;
                            color: Red;">
                            No Data Found.
                        </div>
                        <span class="clear"></span>
                        <div id="forDate">
                           <div class="frmleftContent" style="width: 110px; line-height: 20px; display: inline-block; vertical-align: top;">
                                <asp:Label ID="lblDate" runat="server" Text="Date As On : "></asp:Label>
                            </div>
                            <div class="left" style="float:none; display:inline-block">                              
                                <div class="frmleftContent">
                                    <dxe:ASPxDateEdit ID="txtDate" runat="server" ClientInstanceName="e1" Width="130px"
                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy" UseMaskBehavior="True" AllowNull="False"
                                        Height="25px" Font-Size="13px">
                                        <clientsideevents datechanged="function(s, e) { OnDateChanged(); }" />
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divClntType">
                           <div class="frmleftContent" style="width: 110px; line-height: 20px; display: inline-block; vertical-align: top;">
                                <asp:Label ID="lblClntType" runat="server" Text="Client Type : "></asp:Label>
                            </div>
                            <div class="frmleftContent" style="float:none; display:inline-block">
                                <dxe:ASPxComboBox ID="ASPxComboBox1" ClientInstanceName="cmbClientType" Width="130px"
                                    runat="server" Font-Size="13px" ValueType="System.String" Font-Bold="False" SelectedIndex="0">
                                    <items>
                                        <dxe:ListEditItem Text="All" Value="All" />
                                        <dxe:ListEditItem Text="Individuals" Value="01" />
                                        <dxe:ListEditItem Text="Corporates" Value="05" />
                                        <dxe:ListEditItem Text="Clearing Members" Value="06" />
                                        <dxe:ListEditItem Text="NRIs" Value="04" />
                                        <dxe:ListEditItem Text="Other Accounts" Value="other" />
                                    </items>
                                    <clientsideevents selectedindexchanged="OnClientTypeChanged" />
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divGroupBy">
                            <div class="frmleftContent" style="width: 110px; line-height: 20px;display: inline-block; vertical-align: top;">
                                <asp:Label ID="lblGroupBy" runat="server" Text="Group By : "></asp:Label>
                            </div>
                            <div class="left" style="float:none; display:inline-block">
                                <div class="frmleftContent" style="padding-top: 3px">
                                    <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="13px" onchange="fnddlGroup(this.value)">
                                        <asp:ListItem Value="0">Branch</asp:ListItem>
                                        <asp:ListItem Value="1">Group</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div id="td_branch" class="frmleftContent" style="font-size: 11px;">
                                    <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="cd" onclick="Branch('a')" />
                                    All
                                    <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="cd" onclick="Branch('b')" />Selected
                                </div>
                                <div id="td_group" class="left" style="display: none;">
                                    <div class="frmleftContent">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlgrouptype" runat="server" Font-Size="13px" onchange="fngrouptype(this.value)">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div id="td_allselect" class="frmleftContent" style="display: none; font-size: 11px;">
                                        <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="ef"
                                            onclick="Group('a')" />
                                        All
                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="ef" onclick="Group('b')" />Selected
                                    </div>
                                </div>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divClient">
                            <div class="frmleftContent" style="width: 110px; line-height: 20px;display: inline-block; vertical-align: top;">
                                <asp:Label ID="lblClient" runat="server" Text="Client : "></asp:Label>
                            </div>
                            <div class="frmleftContent" style="width: 125px; padding-top: 3px; font-size: 12px; float:none; display:inline-block">
                                <dxe:ASPxRadioButtonList ID="rbUser" runat="server" SelectedIndex="0" RepeatDirection="Horizontal"
                                    TextWrap="False" EnableDefaultAppearance="False">
                                    <items>
                                        <dxe:ListEditItem Text="All" Value="A" />
                                        <dxe:ListEditItem Text="Selected" Value="S" />
                                    </items>
                                    <clientsideevents valuechanged="function(s, e) {ShowEmployeeFilterForm(s.GetValue());}" />
                                </dxe:ASPxRadioButtonList>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divISIN">
                               <div class="frmleftContent" style="width: 110px; line-height: 20px;display: inline-block; vertical-align: top;">
                                <asp:Label ID="lblIsin" runat="server" Text="ISIN : "></asp:Label>
                            </div>
                            <div class="frmleftContent" style="width: 125px; padding-top: 3px; font-size: 12px; float:none; display:inline-block">
                                <dxe:ASPxRadioButtonList ID="rbISIN" runat="server" SelectedIndex="0" RepeatDirection="Horizontal"
                                    TextWrap="False" EnableDefaultAppearance="False">
                                    <items>
                                        <dxe:ListEditItem Text="All" Value="A" />
                                        <dxe:ListEditItem Text="Specific" Value="S" />
                                    </items>
                                    <clientsideevents valuechanged="function(s, e) {ShowISINFilterForm(s.GetValue());}" />
                                    <border borderwidth="0px" />
                                </dxe:ASPxRadioButtonList>
                            </div>
                            <div id="tdtxtISIN" class="frmleftContent">
                                <asp:TextBox ID="txtISIN_hidden" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtISIN" runat="server" Width="200px" Font-Size="13px"></asp:TextBox>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="tdSettlementLabel">
                             <div class="frmleftContent" style="width: 110px; line-height: 20px;display: inline-block; vertical-align: top;">
                                <asp:Label ID="lblSettlementNo" runat="server" Text="Settlement No. : "></asp:Label>
                            </div>
                            <div id="tdrbSettlement" class="frmleftContent" style="width: 125px; padding-top: 3px; font-size: 12px; float:none; display:inline-block">
                                <dxe:ASPxRadioButtonList ID="rbSettlement" runat="server" SelectedIndex="0" RepeatDirection="Horizontal"
                                    TextWrap="False" Font-Size="12px" EnableDefaultAppearance="False" ClientInstanceName="radioSettlement">
                                    <items>
                                        <dxe:ListEditItem Text="All" Value="A" />
                                        <dxe:ListEditItem Text="Specific" Value="S" />
                                    </items>
                                    <clientsideevents valuechanged="function(s, e) {ShowSettlementFilterForm(s.GetValue());}" />
                                    <border borderwidth="0px" />
                                </dxe:ASPxRadioButtonList>
                            </div>
                            <%--<div id="tdSettlement" class="frmleftContent">
                            <span class="Ecoheadtxt" style="color: blue"><strong>Value:</strong></span>
                        </div>--%>
                            <div id="tdtxtSettlement" class="frmleftContent">
                                <asp:TextBox ID="txtSettlement_hidden" runat="server" Width="14px"></asp:TextBox>
                                <asp:TextBox ID="txtSettlement" runat="server" Width="100px" Font-Size="13px"></asp:TextBox>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divReportType">
                             <div class="frmleftContent" style="width: 110px; line-height: 20px;display: inline-block; vertical-align: top;">
                                <asp:Label ID="lblReportType" runat="server" Text="Report Type : "></asp:Label>
                            </div>
                            <div class="frmleftContent" style="width: 125px; padding-top: 3px; font-size: 12px; float:none; display:inline-block">
                                <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" TextWrap="False" RepeatDirection="Horizontal"
                                    Font-Size="13px" EnableDefaultAppearance="False" SelectedIndex="0">
                                    <items>
                                        <dxe:ListEditItem Text="Screen" Value="Screen" />
                                        <dxe:ListEditItem Text="Print" Value="Print" />
                                    </items>
                                </dxe:ASPxRadioButtonList>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divReportView">
                             <div class="frmleftContent" style="width: 110px; line-height: 20px;display: inline-block; vertical-align: top;">
                                <asp:Label ID="lblReportView" runat="server" Text="Report View : "></asp:Label>
                            </div>
                            <div class="frmleftContent" style="width: 160px; padding-top: 3px; font-size: 12px; float:none; display:inline-block">
                                <dxe:ASPxRadioButtonList ID="RBReportView" runat="server" TextWrap="False" RepeatDirection="Horizontal"
                                    Font-Size="13px" EnableDefaultAppearance="False" SelectedIndex="0" ClientInstanceName="cRBReportView">
                                    <items>
                                        <dxe:ListEditItem Text="Client Wise" Value="C" />
                                        <dxe:ListEditItem Text="ISIN Wise" Value="S" />
                                    </items>
                                </dxe:ASPxRadioButtonList>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divCalHoldingValue">
                            <div class="frmleftContent" style="width: 22px; padding-top: 3px; font-size: 12px;display: inline-block; vertical-align: top;">
                                <asp:CheckBox ID="chkCalHoldingValue" Checked="true" runat="Server" />
                            </div>
                            <div class="frmleftContent" style="width: 210px; line-height: 20px; float:none; display:inline-block">
                                <asp:Label ID="lblCalHoldVal" runat="server" Text="Show Calculated Holding Value"></asp:Label>
                            </div>
                        </div>
                        <br class="clear" />
                        <div class="right" style="margin-right: 10px;">
                            <dxe:ASPxButton ID="btnShow" runat="server" Width="80px" OnClick="btnShow_Click"
                                Text="Show" ValidationGroup="a" EnableDefaultAppearance="False" VerticalAlign="Middle"
                                CssClass="btnUpdate" TabIndex="0">
                                <clientsideevents click="function(s, e) {   selecttion();
                                                                        hide('filter');  
                                                                        show('btnfilter');                                                                        
                                                                        }" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                    <div class="left" style="width: 372px; margin-left: 20px;">
                        <div id="ShowFilterByAjax" class="left frmContent" style="display: none;">
                            <div style="width: 100%">
                                <div class="frmleftContent">
                                    <asp:TextBox ID="txtName" runat="server" Font-Size="12px" Height="20px" Width="170px"
                                        TabIndex="0" onkeyup="FunClientScrip(this,'ShowClientFORMCDSL',event)"></asp:TextBox>
                                </div>
                                <div class="frmleftContent" style="padding-top: 3px">
                                    <asp:DropDownList ID="cmbsearch" runat="server" Font-Size="13px" Width="80px" Enabled="false">
                                        <asp:ListItem>Clients</asp:ListItem>
                                        <asp:ListItem>Branch</asp:ListItem>
                                        <asp:ListItem>Group</asp:ListItem>
                                        <asp:ListItem>User</asp:ListItem>
                                    </asp:DropDownList></div>
                                <div class="frmleftContent">
                                    <a id="A3" href="javascript:void(0);" onclick="btnAddEmailtolist_click()"><span style="color: #009900;
                                        text-decoration: underline; font-size: 10pt; line-height: 2;">Add to List</span>
                                    </a>
                                </div>
                            </div>
                            <span class="clear" style="background-color: #B7CEEC;"></span>
                            <div class="frmleftContent" style="height: 105px; margin-top: 5px">
                                <asp:ListBox ID="SelectionList" runat="server" Font-Size="12px" Height="100px" Width="350px"
                                    TabIndex="0"></asp:ListBox>
                            </div>
                            <span class="clear" style="background-color: #B7CEEC;"></span>
                            <div class="frmleftContent" style="text-align: center">
                                <a id="AA2" href="javascript:void(0);" tabindex="0" onclick="clientselection()"><span
                                    style="color: #000099; text-decoration: underline; font-size: 10pt; line-height: 2;">
                                    Done</span></a> <a id="AA1" href="javascript:void(0);" tabindex="0" onclick="btnRemoveEmailFromlist_click()">
                                        <span style="color: #cc3300; text-decoration: underline; font-size: 10pt; line-height: 2;">
                                            Remove</span></a>
                            </div>
                        </div>
                    </div>
                    <br class="clear" />
                </div>
            </div>
            <div style="display: none">
                <asp:TextBox ID="txtName_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                <asp:TextBox ID="dtFrom_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
            </div>
            <br class="clear" />
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                        top: 50%; = background-color: white; layer-background-color: white; height: 80;
                        width: 150;'>
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
            <asp:UpdatePanel runat="server" ID="u1">
                <ContentTemplate>
                    <asp:Label ID="Label5" runat="server"></asp:Label>
                    <table id="t11" runat="server" class="TableMain100">
                      <%--  <tbody>--%>
                            <tr>
                                <td width="4%">
                                    <asp:LinkButton ID="ASPxFirst1" OnClick="btnFirst" runat="server" Font-Bold="True"
                                        ForeColor="Blue" OnClientClick="javascript:selecttion();">First</asp:LinkButton>
                                </td>
                                <td width="5%">
                                    <asp:LinkButton ID="ASPxPrevious1" OnClick="btnPrevious" runat="server" Font-Bold="True"
                                        ForeColor="Blue" OnClientClick="javascript:selecttion();">Previous</asp:LinkButton>
                                </td>
                                <td width="4%">
                                    <asp:LinkButton ID="ASPxNext1" OnClick="btnNext" runat="server" Font-Bold="True"
                                        ForeColor="Blue" OnClientClick="javascript:selecttion();">Next</asp:LinkButton>
                                </td>
                                <td width="3%">
                                    <asp:LinkButton ID="ASPxLast1" OnClick="btnLast" runat="server" Font-Bold="True"
                                        ForeColor="Blue" OnClientClick="javascript:selecttion();">Last</asp:LinkButton>
                                </td>
                                <td width="20%">
                                    <asp:Label ID="Label11" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                </td>
                            </tr>
                     <%--   </tbody>--%>
                    </table>
                    <table id="head" cellspacing="0" bordercolor="darkblue" cellpadding="6" width="100%"
                        bgcolor="#ffffff" border="1" runat="server">
                       <%-- <tbody>--%>
                            <tr>
                                <td width="20%" bgcolor="white">
                                    <%--Beneficiary AccountID:--%>
                                    <asp:Label ID="lblBoID" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                </td>
                                <td align="left" width="30%" bgcolor="white">
                                    <%--Beneficiary Name:--%>
                                    <asp:Label ID="lblBoName" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                </td>
                                <td width="25%" bgcolor="white">
                                    <%--Second Holder:--%>
                                    <asp:Label ID="lblSecondHolder" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                </td>
                                <td width="25%" bgcolor="white">
                                    <%--Third Holder:--%>
                                    <asp:Label ID="lblThirdHolder" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                </td>
                            </tr>
                       <%-- </tbody>--%>
                    </table>
                    <div>
                        <table class="TableMain100">
                        <%--    <tbody>--%>
                                <tr>
                                    <td id="ShowFilter" width="7%">
                                        <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                            Show Filter</span></a>
                                    </td>
                                    <td id="Td1" width="7%">
                                        <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                            All Records</span></a>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                           <%-- </tbody>--%>
                        </table>
                    </div>
                    <div style="background-color: white" id="griddiv" runat="server">
                        <dxe:ASPxGridView ID="gridHolding" runat="server" Width="100%" ClientInstanceName="grid"
                            AutoGenerateColumns="False" KeyFieldName="NsdlHolding_ISIN" OnCustomCallback="gridHolding_CustomCallback"
                            OnSummaryDisplayText="gridHolding_SummaryDisplayText" >                            
                            <SettingsBehavior AllowFocusedRow="True"></SettingsBehavior>
                            <Styles>
                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                </Header>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                            </Styles>
                            <SettingsPager PageSize="20">
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="NsdlHolding_ISIN" Caption="ISIN No." VisibleIndex="0">
                                    <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>
                                    <CellStyle CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <FooterTemplate>
                                        Total Holding Value
                                    </FooterTemplate>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CmpName" Caption="ISIN Name" VisibleIndex="1" Width="80">
                                    <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <EditItemTemplate>
                                    
                                     </EditItemTemplate>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="NsdlHolding_SettlementNumber" Caption="SettlementID"
                                    VisibleIndex="2">
                                    <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>
                                    <CellStyle CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Type" Caption="Type" VisibleIndex="3">
                                    <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>
                                    <CellStyle CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Total" UnboundType="Integer" Caption="Current Balance"
                                    VisibleIndex="4">
                                    <Settings AllowAutoFilter="False"></Settings>
                                    <CellStyle HorizontalAlign="Right" CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="#.###">
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Free" UnboundType="Integer" Caption="Free"
                                    VisibleIndex="5">
                                    <Settings AllowAutoFilter="False"></Settings>
                                    <CellStyle HorizontalAlign="Right" CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="#.###">
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Pledged" UnboundType="Integer" Caption="Pledged"
                                    VisibleIndex="6">
                                    <PropertiesTextEdit DisplayFormatString="#.###">
                                    </PropertiesTextEdit>
                                    <Settings AllowAutoFilter="False"></Settings>
                                    <CellStyle HorizontalAlign="Right" CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Remat" UnboundType="Integer" Caption="Pending Remat"
                                    VisibleIndex="7">
                                    <Settings AllowAutoFilter="False"></Settings>
                                    <CellStyle HorizontalAlign="Right" CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="#.###">
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Demat" UnboundType="Integer" Caption="Pending Demat"
                                    VisibleIndex="8">
                                    <Settings AllowAutoFilter="False"></Settings>
                                    <CellStyle HorizontalAlign="Right" CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="#.###">
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="9">
                                    <Settings AllowAutoFilter="False"></Settings>
                                    <CellStyle HorizontalAlign="Right" CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="0.0000">
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Rate_Date" Caption="Rate Date" VisibleIndex="10">
                                    <Settings AllowAutoFilter="False"></Settings>
                                    <CellStyle HorizontalAlign="Left" CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ISINValue" Caption="Value" VisibleIndex="11">
                                    <Settings AllowAutoFilter="False"></Settings>
                                    <CellStyle HorizontalAlign="Right" CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="0.00">
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowTitlePanel="True" ShowStatusBar="Visible" ShowFooter="True"></Settings>
                            <StylesEditors>
                                <ProgressBar Height="25px">
                                </ProgressBar>
                            </StylesEditors>
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="Total" ShowInColumn="Current Balance" ShowInGroupFooterColumn="Current Balance"
                                    SummaryType="Sum" DisplayFormat="#,##,###.##" />
                                <dxe:ASPxSummaryItem FieldName="Free" ShowInColumn="Free" ShowInGroupFooterColumn="Free"
                                    SummaryType="Sum" DisplayFormat="#,##,###.##" />
                                <dxe:ASPxSummaryItem FieldName="Pledged" ShowInColumn="Pledged" ShowInGroupFooterColumn="Pledged"
                                    SummaryType="Sum" DisplayFormat="#,##,###.##" />
                                <dxe:ASPxSummaryItem FieldName="Remat" ShowInColumn="Pending Remat" ShowInGroupFooterColumn="Pending Remat"
                                    SummaryType="Sum" DisplayFormat="#,##,###.##" />
                                <dxe:ASPxSummaryItem FieldName="Demat" ShowInColumn="Pending Demat" ShowInGroupFooterColumn="Pending Demat"
                                    SummaryType="Sum" DisplayFormat="#,##,###.##" />
                                <dxe:ASPxSummaryItem FieldName="ISINValue" ShowInColumn="Value" ShowInGroupFooterColumn="Value"
                                    SummaryType="Sum" Tag="Total Holding Value" DisplayFormat="#,##,###.00" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                    </div>
                    <div>
                        <table id="t1" runat="server" class="TableMain100">
                      <%--  <tbody>--%>
                                <tr>
                                    <td width="4%">
                                        <asp:LinkButton ID="ASPxFirst" OnClick="btnFirst" runat="server" Font-Bold="True"
                                            ForeColor="Blue" OnClientClick="javascript:selecttion();">First</asp:LinkButton></td>
                                    <td width="5%">
                                        <asp:LinkButton ID="ASPxPrevious" OnClick="btnPrevious" runat="server" Font-Bold="True"
                                            ForeColor="Blue" OnClientClick="javascript:selecttion();">Previous</asp:LinkButton></td>
                                    <td width="4%">
                                        <asp:LinkButton ID="ASPxNext" OnClick="btnNext" runat="server" Font-Bold="True" ForeColor="Blue"
                                            OnClientClick="javascript:selecttion();">Next</asp:LinkButton></td>
                                    <td width="3%">
                                        <asp:LinkButton ID="ASPxLast" OnClick="btnLast" runat="server" Font-Bold="True" ForeColor="Blue"
                                            OnClientClick="javascript:selecttion();">Last</asp:LinkButton></td>
                                    <td width="20%">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                         <%--   </tbody>--%>
                        </table>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <input id="hidClients" type="hidden" runat="server" />
        <input id="hidBranch" type="hidden" runat="server" />
        <input id="hidGroup" type="hidden" runat="server" />
        <input id="hiddenIsinMail" type="hidden" runat="server" />
</asp:Content>
