<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_SUBBRKGCOMMISSIONREPORT" CodeBehind="SUBBRKGCOMMISSIONREPORT.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

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

        .grid_scroll {
            overflow-y: scroll;
            overflow-x: scroll;
            width: 90%;
            height: 300px;
            scrollbar-base-color: #C0C0C0;
        }

        .AlternatingRowStyleClass {
            background-color: #fff0f5;
            cursor: default;
        }

        .RowStyleClass {
            background-color: White;
            cursor: default;
        }

        .SelectedRowStyle {
            background-color: #ffe1ac;
            cursor: default;
        }
    </style>

    <script language="javascript" type="text/javascript">

        function Page_Load()///Call Into Page Load
    {
        Hide('showFilter');
        Hide('tr_filter');
        Hide('Tab_Display');
        FnReportViewChange('1');
        document.getElementById('hiddencount').value = 0;
        FnddlGeneration('1');
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
                function FnReportViewChange(obj) {
                    if (obj == '1') {
                        Hide('Tr_Group');
                        Hide('Tr_Clients');
                        Show('Tr_Commission');
                        Show('Tr_CommissionFor');
                        Show('Tr_GenerateType');
                        FnddlGeneration('1');
                    }
                    else if (obj == '3') {
                        Show('Tr_Group');
                        Show('Tr_Clients');
                        Show('Tr_Commission');
                        Show('Tr_CommissionFor');
                        Hide('Tr_GenerateType');
                        Show('td_Export');
                        Hide('td_Screen');
                    }
                    else {
                        if (obj == '4') {
                            Hide('Tr_Commission');
                            Hide('Tr_CommissionFor');
                        }
                        else {
                            Show('Tr_Commission');
                            Show('Tr_CommissionFor');
                        }
                        Show('Tr_Group');
                        Show('Tr_Clients');
                        Show('Tr_GenerateType');
                        FnddlGeneration('1');
                    }

                }
                function fnddlGroup(obj) {
                    if (obj == "0") {
                        Hide('td_group');
                        Show('td_branch');
                    }
                    else {
                        Show('td_group');
                        Hide('td_branch');
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
                function FnClients(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'Clients';
                        Show('showFilter');
                    }

                }
                function FnBranch(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'Branch';
                        Show('showFilter');
                    }

                }
                function FnGroup(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'Group';
                        Show('showFilter');
                    }

                }
                function FnCommission(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'Commission';
                        Show('showFilter');
                    }

                }
                function FnSegment(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'Segment';
                        Show('showFilter');
                    }

                }
                function FnddlGeneration(obj) {
                    if (obj == "1") {
                        Show('td_Screen');
                        Hide('td_Export');

                    }
                    if (obj == "2") {
                        Hide('td_Screen');
                        Show('td_Export');
                    }
                    Hide('showFilter');
                    height();
                }
                function FunClientScrip(objID, objListFun, objEvent) {
                    var cmbVal;

                    if (document.getElementById('cmbsearchOption').value == "Clients") {
                        if (document.getElementById('ddlGroup').value == "0")//////////////Group By  selected are branch
                        {
                            if (document.getElementById('ddlGroup').value == "0") {
                                if (document.getElementById('rdbranchAll').checked == true) {
                                    cmbVal = 'ClientsBranch' + '~' + 'ALL';
                                }
                                else {
                                    cmbVal = 'ClientsBranch' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Branch').value;
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
                        if (document.getElementById('ddlCommissionFor').value == 'Sub Broker') {
                            cmbVal = document.getElementById('cmbsearchOption').value + '~' + 'Sub Broker';
                        }
                        else if (document.getElementById('ddlCommissionFor').value == 'Relationship Partner') {
                            cmbVal = document.getElementById('cmbsearchOption').value + '~' + 'Relationship Partner';
                        }
                        else {
                            cmbVal = document.getElementById('cmbsearchOption').value + '~' + 'Both';
                        }
                        cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
                    }
                    ajax_showOptions(objID, objListFun, objEvent, cmbVal);

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

                function selecttion() {
                    var combo = document.getElementById('ddlExport');
                    combo.value = 'Ex';
                }
                function fnRecord(obj) {
                    if (obj == "1") {
                        Hide('tr_filter');
                        Show('Tab1');
                        Hide('Tab_Display');
                        alert('For Display In Report View :ClientWise Detail !!' + '\n' + ' You Can Only Select Clients !!!');
                    }
                    if (obj == "2") {
                        Hide('tr_filter');
                        Show('Tab1');
                        Hide('Tab_Display');

                    }
                    if (obj == "3") {
                        Hide('tr_filter');
                        Show('Tab1');
                        Hide('Tab_Display');
                        FnReportViewChange('1');
                        document.getElementById('hiddencount').value = 0;
                        FnddlGeneration('1');
                        alert('No Record Found !!');
                    }
                    if (obj == "4")//////ReportView:Summary Wise or Clientwise Net Earning Report
                    {
                        Show('tr_filter');
                        Hide('Tab1');
                        Show('Tab_Display');
                        Hide('Tr_Prvnxt');
                    }
                    if (obj == "5")//////ReportView:ClientWise Summary
                    {
                        Show('tr_filter');
                        Hide('Tab1');
                        Show('Tab_Display');
                        Show('Tr_Prvnxt');
                    }
                    Hide('showFilter');

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
      if (j[0] == 'Commission') {
          document.getElementById('HiddenField_Commission').value = j[1];
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
    <table class="TableMain100">
        <tr>
            <td class="EHEADER" colspan="0" style="text-align: center; height: 22px;">
                <strong><span id="SpanHeader" style="color: #000099">Sub Brokerage & Commission Reports</span></strong>
            </td>
            <td class="EHEADER" width="25%" id="tr_filter" style="height: 22px">
                <a href="javascript:void(0);" onclick="fnRecord('2');"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                    <asp:ListItem Value="E">Excel</asp:ListItem>
                    <asp:ListItem Value="P">PDF</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table id="Tab1">
        <tr>
            <td valign="top" class="gridcellleft">
                <table>
                    <tr>
                        <td id="Td_DateWise" valign="top">
                            <table border="10" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">Period :</td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="DtFromDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                            Font-Size="12px" Width="150px" ClientInstanceName="DtFromDate">
                                            <DropDownButton Text="From">
                                            </DropDownButton>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="DtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                            Font-Size="12px" Width="150px" ClientInstanceName="DtToDate">
                                            <DropDownButton Text="To">
                                            </DropDownButton>
                                        </dxe:ASPxDateEdit>
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
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td class="gridcellleft">
                                                    <table border="10" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td class="gridcellleft" bgcolor="#B7CEEC">Report View :</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlReportView" runat="server" Width="180px" Font-Size="12px"
                                                                    onchange="FnReportViewChange(this.value)">
                                                                    <asp:ListItem Value="1">Summary</asp:ListItem>
                                                                    <asp:ListItem Value="2">ClientWise Summary</asp:ListItem>
                                                                    <asp:ListItem Value="3">ClientWise Detail</asp:ListItem>
                                                                    <asp:ListItem Value="4">Clientwise Net Earning Report</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="Tr_CommissionFor">
                                                <td class="gridcellleft">
                                                    <table border="10" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td class="gridcellleft" bgcolor="#B7CEEC">Commission For :</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCommissionFor" runat="server" Width="150px" Font-Size="12px">
                                                                    <asp:ListItem Value="1">Sub Broker</asp:ListItem>
                                                                    <asp:ListItem Value="2">Relationship Partner</asp:ListItem>
                                                                    <asp:ListItem Value="3">Both</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="Tr_Commission">
                                                <td class="gridcellleft">
                                                    <table border="10" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td class="gridcellleft" bgcolor="#B7CEEC">Commission :</td>
                                                            <td>
                                                                <asp:RadioButton ID="rdbCommissionAll" runat="server" Checked="True" GroupName="a"
                                                                    onclick="FnCommission('a')" />
                                                                All
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdbCommissionSelected" runat="server" GroupName="a" onclick="FnCommission('b')" />Selected
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="Tr_Group">
                                                <td class="gridcellleft">
                                                    <table border="10" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td class="gridcellleft" bgcolor="#B7CEEC">Group By</td>
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
                                                                            <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="b" onclick="FnBranch('a')" />
                                                                            All
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="b" onclick="FnBranch('b')" />Selected
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td id="td_group" style="display: none;">
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
                                                                            <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="c"
                                                                                onclick="FnGroup('a')" />
                                                                            All
                                                                                    <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="c" onclick="FnGroup('b')" />Selected
                                                                        </td>
                                                                    </tr>
                                                                </table>
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
                                                                <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="d" onclick="FnClients('a')" />
                                                                All
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="radPOAClient" runat="server" GroupName="d" onclick="FnClients('a')" />POA
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="d" onclick="FnClients('b')" />
                                                                Selected
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="Tr1">
                                                <td class="gridcellleft">
                                                    <table border="10" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td class="gridcellleft" bgcolor="#B7CEEC">Segment :</td>
                                                            <td>
                                                                <asp:RadioButton ID="RdbSegmentAll" runat="server" GroupName="e" onclick="FnSegment('a')" />
                                                                All
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="RdbSegmentSelected" runat="server" Checked="True" GroupName="e" onclick="FnSegment('b')" />
                                                                Selected [ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr id="Tr_GenerateType">
                                                <td class="gridcellleft">
                                                    <table border="10" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type :</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlGeneration" runat="server" Width="100px" Font-Size="12px"
                                                                    onchange="FnddlGeneration(this.value)">
                                                                    <asp:ListItem Value="1">Screen</asp:ListItem>
                                                                    <asp:ListItem Value="2">Export</asp:ListItem>
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
                                                            <td id="td_Screen">
                                                                <asp:Button ID="btnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Screen"
                                                                    Width="101px" OnClientClick="selecttion()" OnClick="btnScreen_Click" />
                                                            </td>
                                                            <td id="td_Export">
                                                                <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                                    Width="101px" OnClientClick="selecttion()" OnClick="btnExcel_Click" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="showFilter">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                                    id="TdFilter">
                                                    <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                                    <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                        Enabled="false">
                                                        <asp:ListItem>Clients</asp:ListItem>
                                                        <asp:ListItem>Branch</asp:ListItem>
                                                        <asp:ListItem>Group</asp:ListItem>
                                                        <asp:ListItem>Commission</asp:ListItem>
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
                <asp:HiddenField ID="HiddenField_Commission" runat="server" />
                <asp:HiddenField ID="hiddencount" runat="server" />
                <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                <asp:DropDownList ID="cmbrecord" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                </asp:DropDownList>
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
    <table style="display: none;" id="Tab_Display">

        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <table width="100%" border="1">
                            <tr>
                                <td>
                                    <div id="DivHeader" runat="server">
                                    </div>
                                </td>
                            </tr>

                            <tr bordercolor="Blue" id="Tr_Prvnxt">
                                <td align="left">
                                    <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="30%">
                                        <tr>
                                            <td width="20" style="padding: 5px">
                                                <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                                    OnClientClick="javascript:selecttion();showProgress();" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                            <td width="25">
                                                <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                    OnClientClick="javascript:selecttion();showProgress();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                            <td width="20" style="padding: 5px">
                                                <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                    OnClientClick="javascript:selecttion();showProgress();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                            <td width="20">
                                                <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();showProgress();"
                                                    OnClick="ASPxLast_Click">Last</asp:LinkButton></td>
                                            <td align="right">
                                                <asp:Label ID="listRecord" runat="server" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <div id="DivRecord" runat="server">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>


