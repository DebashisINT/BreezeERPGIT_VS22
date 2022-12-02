<%@ Page Title="Interest & Penalty Generation" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_InterestAndPenaltyGeneation" CodeBehind="InterestAndPenaltyGeneation.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>--%>
   <%-- <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />--%>
    <script type="text/javascript" src="../CentralData/JSScript/GenericJScript.js"></script>
    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>


    <%--<style type="text/css">
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
    </style>--%>
    <script language="javascript" type="text/javascript">


        function Page_Load()///Call Into Page Load
        {

            Hide('showFilter');
            TxtGracePeriod.SetValue('1');
            document.getElementById('DtFrom').focus();
            //                 DateChange(positionDate);
            FnAccountChange('0');
            Hide('tr_BtnForExcel');
            height();
        }
        //function height() {
        //    if (document.body.scrollHeight >= 350) {
        //        window.frameElement.height = document.body.scrollHeight;
        //    }
        //    else {
        //        window.frameElement.height = '350px';
        //    }
        //    window.frameElement.width = document.body.scrollwidth;
        //}
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }

        function DateChange(positionDate) {

            var FYS = '<%=Session["FinYearStart"]%>';
	        var FYE = '<%=Session["FinYearEnd"]%>';
	        var LFY = '<%=Session["LastFinYear"]%>';
	        DevE_CheckForFinYear(positionDate, FYS, FYE, LFY);

	        if (cDtFrom.GetDate() == positionDate.GetDate()) {
	            var setFromDate = '<%=currentFromDate%>';
            CompareDate(cDtFrom.GetDate(), cDtTo.GetDate(), 'LE', 'From Date Can Not Be Greater Than To Date', cDtFrom, setFromDate);
        }
        else if (cDtTo.GetDate() == positionDate.GetDate()) {
            var setToDate = '<%=currentToDate%>';
                     CompareDate(cDtFrom.GetDate(), cDtTo.GetDate(), 'LE', 'To Date Can Not Be Less Than From Date', cDtTo, setToDate);
                 }


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
         function fnClients(obj) {
             if (obj == "a")
                 Hide('showFilter');
             else {
                 var cmb = document.getElementById('cmbsearchOption');
                 cmb.value = 'Clients';
                 Show('showFilter');
             }

         }
         function fnSubAc(obj) {
             if (obj == "a")
                 Hide('showFilter');
             else {
                 var cmb = document.getElementById('cmbsearchOption');
                 cmb.value = 'SubAccount';
                 Show('showFilter');
             }

         }
         function fnSegment(obj) {
             if (obj == "a")
                 Hide('showFilter');
             else {
                 var cmb = document.getElementById('cmbsearchOption');
                 cmb.value = 'Segment';
                 Show('showFilter');
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
             Hide('showFilter');
         }
         function fngrouptype(obj) {
             if (obj == "0") {
                 Hide('td_allselect');
                 alert('Please Select Group Type !');
             }
             else {
                 Show('td_allselect');
             }
             Hide('showFilter');
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
             document.getElementById('btnGenerateposition').disabled = false;
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
         function AjaxListCallForClients(objID, objListFun, objEvent) {
             if (document.getElementById('ddlAccountType').value == "0" || document.getElementById('ddlAccountType').value == "1" || document.getElementById('ddlAccountType').value == "2" || document.getElementById('HiddenField_txtSubLedgerType').value == 'Customers') {
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
             else if (document.getElementById('HiddenField_txtSubLedgerType').value == 'NSDL Clients') {
                 if (document.getElementById('ddlGroup').value == "0" || document.getElementById('ddlGroup').value == "2")//////////////Group By  selected are branch
                 {
                     if (document.getElementById('ddlGroup').value == "0") {
                         if (document.getElementById('rdbranchAll').checked == true) {
                             cmbVal = 'NsdlClientsBranch' + '~' + 'ALL';
                         }
                         else {
                             cmbVal = 'NsdlClientsBranch' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Branch').value;
                         }
                     }
                     if (document.getElementById('ddlGroup').value == "2") {
                         if (document.getElementById('rdbranchAll').checked == true) {
                             cmbVal = 'NsdlClientsBranchGroup' + '~' + 'ALL';
                         }
                         else {
                             cmbVal = 'NsdlClientsBranchGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_BranchGroup').value;
                         }
                     }
                 }
                 else //////////////Group By selected are Group
                 {
                     if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                         cmbVal = 'NsdlClientsGroup' + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                     }
                     else {
                         cmbVal = 'NsdlClientsGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Group').value;
                     }
                 }
             }
             else if (document.getElementById('HiddenField_txtSubLedgerType').value == 'CDSL Clients') {
                 if (document.getElementById('ddlGroup').value == "0" || document.getElementById('ddlGroup').value == "2")//////////////Group By  selected are branch
                 {
                     if (document.getElementById('ddlGroup').value == "0") {
                         if (document.getElementById('rdbranchAll').checked == true) {
                             cmbVal = 'CdslClientsBranch' + '~' + 'ALL';
                         }
                         else {
                             cmbVal = 'CdslClientsBranch' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Branch').value;
                         }
                     }
                     if (document.getElementById('ddlGroup').value == "2") {
                         if (document.getElementById('rdbranchAll').checked == true) {
                             cmbVal = 'CdslClientsBranchGroup' + '~' + 'ALL';
                         }
                         else {
                             cmbVal = 'CdslClientsBranchGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_BranchGroup').value;
                         }
                     }
                 }
                 else //////////////Group By selected are Group
                 {
                     if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                         cmbVal = 'CdslClientsGroup' + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                     }
                     else {
                         cmbVal = 'CdslClientsGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Group').value;
                     }
                 }
             }
             ajax_showOptions(objID, objListFun, objEvent, cmbVal);
         }
         function FunClientScrip(objID, objListFun, objEvent) {
             var cmbVal;

             if (document.getElementById('cmbsearchOption').value == "Clients") {
                 AjaxListCallForClients(objID, objListFun, objEvent);
             }
             else if (document.getElementById('cmbsearchOption').value == "Group") {
                 cmbVal = document.getElementById('cmbsearchOption').value;
                 cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
             }
             else if (document.getElementById('cmbsearchOption').value == "SubAccount") {
                 var criteritype = 'B';
                 if (document.getElementById('ddlAccountType').value == "0") {
                     criteritype = ' SubAccount_MainAcReferenceID="SYSTM00001"';
                     criteritype = criteritype.replace('"', "'");
                     criteritype = criteritype.replace('"', "'");
                 }
                 if (document.getElementById('ddlAccountType').value == "1") {
                     criteritype = ' SubAccount_MainAcReferenceID="SYSTM00002"';
                     criteritype = criteritype.replace('"', "'");
                     criteritype = criteritype.replace('"', "'");
                 }
                 if (document.getElementById('ddlAccountType').value == "2") {
                     criteritype = ' SubAccount_MainAcReferenceID in ("SYSTM00001","SYSTM00002")';
                     criteritype = criteritype.replace('"', "'");
                     criteritype = criteritype.replace('"', "'");
                     criteritype = criteritype.replace('"', "'");
                     criteritype = criteritype.replace('"', "'");
                 }
                 if (document.getElementById('ddlAccountType').value == "3") {
                     criteritype = ' SubAccount_MainAcReferenceID="' + document.getElementById('HiddenField_txtMainAccountCode').value + '"';
                     criteritype = criteritype.replace('"', "'");
                     criteritype = criteritype.replace('"', "'");
                 }

                 cmbVal = 'SubAccount~' + criteritype;

             }
             else {
                 cmbVal = document.getElementById('cmbsearchOption').value;
             }
             ajax_showOptions(objID, objListFun, objEvent, cmbVal);
         }

         function FnAjaxListCall(obj1, obj2, obj3) {
             ajax_showOptions(obj1, obj2, obj3, 'MainAcc', 'Main');
         }
         function keyVal(obj) {
             var obj1 = obj.split('~');
             document.getElementById('HiddenField_txtMainAccountCode').value = obj1[0];
             document.getElementById('HiddenField_txtSubLedgerType').value = obj1[1];

             if (obj1[1] == 'Custom' || obj1[1] == 'Brokers' || obj1[1] == 'Sub Brokers') {
                 Show('Tr_SubAccount');
                 Hide('Tr_GroupBy');
                 Hide('Tr_Client');
             }
             else if (obj1[1] == 'None') {
                 Hide('Tr_SubAccount');
                 Hide('Tr_GroupBy');
                 Hide('Tr_Client');
             }
             else {
                 Show('Tr_SubAccount');
                 Show('Tr_GroupBy');
                 Show('Tr_Client');
             }
             height();
         }
         function FnAccountChange(obj) {
             if (obj == '3')
                 Show('Td_MainAccount');
             else
                 Hide('Td_MainAccount');
         }
         function AlertRecord(Obj) {
             if (Obj == '1')////No Recod Found
             {
                 Show('tr_BtnCF');
                 Hide('tr_BtnForExcel');
                 alert('No Record Found');
             }
             if (Obj == '2')///After Generate Position Excel Btn Is Open
             {
                 Hide('tr_BtnCF');
                 Show('tr_BtnForExcel');
             }
             if (Obj == '3')///After Generate 
             {
                 Show('tr_BtnCF');
                 Hide('tr_BtnForExcel');
                 alert('Successfully Generate !!');
             }


         }
         function FnCancel(obj) {
             Hide('showFilter');
             document.getElementById('DtFrom').focus();
             document.getElementById('ddlAccountType').value = "0";
             FnAccountChange('0');
             document.getElementById('HiddenField_txtMainAccountCode').value = "";
             document.getElementById('HiddenField_txtSubLedgerType').value = "";
             document.getElementById('txtMainAccount').value = "";
             document.getElementById('ddlGroup').value = "0"
             document.getElementById('rdbranchAll').checked = true;
             document.getElementById('rdbranchSelected').checked = false;
             document.getElementById('rdbClientALL').checked = true;
             document.getElementById('rdPOAClient').checked = false;
             document.getElementById('rdbClientSelected').checked = false;
             document.getElementById('RdbSubAcAll').checked = true;
             document.getElementById('RdbSubAcSelected').checked = false;
             document.getElementById('rdbSegmentAll').checked = true;
             document.getElementById('rdSegmentSelected').checked = false;

             document.getElementById('ChkPreDifineRate').checked = false;
             txtRate.SetValue('0');
             TxtGracePeriod.SetValue('0');

             document.getElementById('DdlServiceTax').value = "Inclusive";
             document.getElementById('DdlBalanceType').value = "Only Debit";

             Hide('tr_BtnForExcel');
             Show('tr_BtnCF');

             if (obj == '2')
                 alert('Successfully Generate !!');
             if (obj == '3')
                 alert('Delete Successfully  !!');
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
            if (j[0] == 'BranchGroup') {
                document.getElementById('HiddenField_BranchGroup').value = j[1];
            }
            if (j[0] == 'SubAccount') {
                document.getElementById('HiddenField_SubAccount').value = j[1];
            }
            if (j[0] == 'Segment') {
                document.getElementById('HiddenField_Segment').value = j[1];
            }



        }
        var __initialText = "365";

        function OnTextBoxInit(textBox) {
            OnTextBoxLostFocus(textBox);
        }
        function OnTextBoxGotFocus(textBox) {
            if (textBox.GetText() == __initialText)
                textBox.SetText("");
        }
        function OnTextBoxLostFocus(textBox) {
            if (textBox.GetText() == "" || parseInt(textBox.GetText()) > 366)
                textBox.SetText(__initialText);
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
    <style>
        .w150 {
            width:150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Interest & Penalty Generation</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
           <%-- <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Interest & Penalty Generation</span></strong></td>
            </tr>--%>
        </table>
        <table id="tab1" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <table  cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="w150" >For The Period :
                                        </td>
                                        <td class="gridcellleft" style="padding-right:15px">
                                            <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="150px" ClientInstanceName="cDtFrom" TabIndex="0">
                                                <DropDownButton Text="From">
                                                </DropDownButton>
                                                <ClientSideEvents DateChanged="function(s,e){DateChange(cDtFrom);}"></ClientSideEvents>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="gridcellleft">

                                            <dxe:ASPxDateEdit ID="DtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="150px" ClientInstanceName="cDtTo" TabIndex="0">
                                                <DropDownButton Text="To">
                                                </DropDownButton>
                                                <ClientSideEvents DateChanged="function(s,e){DateChange(cDtTo);}"></ClientSideEvents>
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
                            <td class="gridcellleft">
                                <table  cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="w150" >Select Account
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAccountType" runat="server" Width="150px" Font-Size="12px"
                                                onchange="FnAccountChange(this.value)">
                                                <asp:ListItem Value="0">Trading</asp:ListItem>
                                                <asp:ListItem Value="1">Margin Deposit</asp:ListItem>
                                                <asp:ListItem Value="2">Both</asp:ListItem>
                                                <asp:ListItem Value="3">Other Account</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td id="Td_MainAccount">
                                <table  cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="w150" >Main Account :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMainAccount" TabIndex="0" runat="server" Width="250px" Font-Size="12px"
                                                onkeyup="FnAjaxListCall(this,'AcSearch',event)"></asp:TextBox>
                                        </td>
                                        <td style="display: none;">
                                            <asp:TextBox ID="txtMainAccount_hidden" runat="server" Width="5px"></asp:TextBox>
                                            <asp:TextBox ID="HiddenField_txtMainAccountCode" runat="server" Width="5px"></asp:TextBox>
                                            <asp:TextBox ID="HiddenField_txtSubLedgerType" runat="server" Width="5px"></asp:TextBox>
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
                        <tr valign="top">
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <table  cellpadding="1" cellspacing="1">
                                                <tr id="Tr_GroupBy">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td class="w150" >Group By</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlGroup" TabIndex="0" runat="server" Width="150px" Font-Size="12px"
                                                                        onchange="fnddlGroup(this.value)">
                                                                        <asp:ListItem Value="0">Branch</asp:ListItem>
                                                                        <asp:ListItem Value="1">Group</asp:ListItem>
                                                                        <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td id="td_branch">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:RadioButton ID="rdbranchAll" TabIndex="0" runat="server" Checked="True" GroupName="a"
                                                                                    onclick="fnBranch('a')" />
                                                                                All
                                                                            </td>
                                                                            <td>
                                                                                <asp:RadioButton ID="rdbranchSelected" TabIndex="0" runat="server" GroupName="a"
                                                                                    onclick="fnBranch('b')" />Selected
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td id="td_group" style="display: none;">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:DropDownList ID="ddlgrouptype" TabIndex="0" runat="server" Font-Size="12px"
                                                                                            onchange="fngrouptype(this.value)">
                                                                                        </asp:DropDownList>
                                                                                    </ContentTemplate>
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                                    </Triggers>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td id="td_allselect" style="display: none;">
                                                                                <asp:RadioButton ID="rdddlgrouptypeAll" TabIndex="0" runat="server" Checked="True"
                                                                                    GroupName="b" onclick="fnGroup('a')" />
                                                                                All
                                                                                <asp:RadioButton ID="rdddlgrouptypeSelected" TabIndex="0" runat="server" GroupName="b"
                                                                                    onclick="fnGroup('b')" />Selected
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="Tr_Client">
                                                    <td>
                                                        <table  cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td class="w150" >Clients :</td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbClientALL" TabIndex="0" runat="server" Checked="True" GroupName="c" onclick="fnClients('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdPOAClient" TabIndex="0" runat="server" GroupName="c" onclick="fnClients('a')" />POA
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbClientSelected" TabIndex="0" runat="server" GroupName="c" onclick="fnClients('b')" />
                                                                    Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="Tr_SubAccount">
                                                    <td>
                                                        <table  cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td class="w150" >Sub-Account :</td>
                                                                <td>
                                                                    <asp:RadioButton ID="RdbSubAcAll" runat="server" Checked="True" GroupName="d" onclick="fnSubAc('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="RdbSubAcSelected" runat="server" GroupName="d" onclick="fnSubAc('b')" />Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table  cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td class="w150" >Segment :</td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbSegmentAll" runat="server" Checked="True" GroupName="e" onclick="fnSegment('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdSegmentSelected" runat="server" GroupName="e" onclick="fnSegment('b')" />Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table  cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td  class="w150"></td>
                                                                <td class="gridcellleft" >
                                                                    <asp:CheckBox ID="ChkPreDifineRate" runat="server" />
                                                                    Consider Pre-Difined Rates If Available</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr valign="top">
                                                    <td>
                                                        <table  cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td class="w150" >Rate :
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxTextBox ID="txtRate" runat="server" HorizontalAlign="Right" Width="150px">
                                                                        <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                                        <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                                                    </dxe:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <b>% P.A.</b></td>

                                                                <td class="w150" >Grace Period :
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxTextBox ID="TxtGracePeriod" runat="server" HorizontalAlign="Right" Width="50px">
                                                                        <MaskSettings Mask="&lt;0..99g&gt;" IncludeLiterals="DecimalSymbol" />
                                                                        <ValidationSettings ErrorDisplayMode="None">
                                                                        </ValidationSettings>
                                                                    </dxe:ASPxTextBox>
                                                                </td>
                                                                <td>Days</td>

                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table  cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td class="w150" >Generate For Interest Amount >
                                                                </td>
                                                                <td>Rs/-</td>
                                                                <td>
                                                                    <dxe:ASPxTextBox ID="txtIntAmnt" runat="server" HorizontalAlign="Right" Width="100px">
                                                                        <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                                        <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                                                    </dxe:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table  cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td class="w150" >No of Days in a Year&nbsp;
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxTextBox ID="txtNoOfDayInYear" runat="server" HorizontalAlign="Right" Width="150px">
                                                                        <ClientSideEvents Init="OnTextBoxInit" GotFocus="OnTextBoxGotFocus" LostFocus="OnTextBoxLostFocus" />
                                                                        <ValidationSettings ErrorDisplayMode="None">
                                                                        </ValidationSettings>
                                                                    </dxe:ASPxTextBox>
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
                            <td>
                                <table  cellpadding="1" cellspacing="1" id="showFilter">
                                    <tr>
                                        <td class="gridcellleft" id="TdFilter">
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="160px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>NsdlClients</asp:ListItem>
                                                <asp:ListItem>CdslClients</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
                                                <asp:ListItem>SubAccount</asp:ListItem>
                                                <asp:ListItem>BranchGroup</asp:ListItem>
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
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <table  cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="w150" >Service Tax :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlServiceTax" runat="server" Width="150px" Font-Size="12px">
                                                <asp:ListItem Value="Inclusive">Inclusive</asp:ListItem>
                                                <%--<asp:ListItem Value="Exclusive">Exclusive</asp:ListItem>
                                                <asp:ListItem Value="Not Applicable">Not Applicable</asp:ListItem>--%>
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table  cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="w150" >Balance Type :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlBalanceType" runat="server" Width="150px" Font-Size="12px">
                                                <asp:ListItem Value="Only Debit">Only Debit</asp:ListItem>
                                                <asp:ListItem Value="Only Credit">Only Credit</asp:ListItem>
                                                <asp:ListItem Value="Both">Both</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <table  cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="w150" >Narration :
                            </td>
                            <td>
                                <asp:TextBox ID="TxtNarration" runat="server" Width="300px" Height="30px" Font-Size="12px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <table  cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="w150" >Report View :
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlRptView" runat="server" Width="120px" Font-Size="12px">
                                    <asp:ListItem Value="Detail">Detail</asp:ListItem>
                                    <asp:ListItem Value="Summary">Summary</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="w150" >Calculate On:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCalculateOn" runat="server" Width="120px" Font-Size="12px">
                                    <asp:ListItem Value="C">Closing Balance</asp:ListItem>
                                    <asp:ListItem Value="O">Opening Balance</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="tr_BtnCF">
                <td valign="top" class="gridcellleft">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnGenerateposition" runat="server" Text="Generate Interest" CssClass="btn btn-primary" OnClick="btnGenerateposition_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="tr_BtnForExcel">
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <asp:Button ID="BtnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Open To Excel"
                                    Width="120px" ForeColor="green" OnClick="BtnExcel_Click" />
                            </td>
                            <td class="gridcellleft">
                                <asp:Button ID="BtnGenerate" runat="server" CssClass="btnUpdate" Height="20px" Text="Generate"
                                    Width="101px" OnClick="BtnGenerate_Click" />
                            </td>

                            <td class="gridcellleft">
                                <asp:Button ID="BtnCancel" runat="server" CssClass="btnUpdate" Height="20px" Text="Cancel"
                                    Width="101px" OnClick="BtnCancel_Click" /></td>

                            <td class="gridcellleft">
                                <asp:Button ID="BtnDelete" runat="server" CssClass="btnUpdate" Height="20px" Text="Delete"
                                    Width="120px" ForeColor="red" OnClick="BtnDelete_Click" />

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
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                    <asp:HiddenField ID="HiddenField_SubAccount" runat="server" />
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

