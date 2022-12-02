<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmReport_dailyBilling" EnableEventValidation="false" CodeBehind="frmReport_dailyBilling.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <%-- <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>
    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>
    
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

        .grid_scroll {
            overflow-y: no;
            overflow-x: scroll;
            width: 55%;
            scrollbar-base-color: #C0C0C0;
        }
    </style>
    <script language="javascript" type="text/javascript">
        groupvalue = "";
        function Page_Load()///Call Into Page Load
        {
            btndisplay('1');
            Hide('showFilter');
            Hide('tab_grid');
            Hide('td_filter');
            Hide('Tr_Broker');
            document.getElementById('ShowTable').style.display = 'none';
            document.getElementById('showFilter1').style.display = 'none';
            Hide('td_grid');
            Hide('td_sapn');
            Hide('tr_ddl');
            height();
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollWidth;

        }


        function FunClientScrip(objID, objListFun, objEvent) {
            var cmbVal;

            if (document.getElementById('cmbsearchOption').value == "Clients") {
                if (document.getElementById('ddlGroup').value == "0")//////////////Group By  selected are branch
                {
                    if (document.getElementById('rdbranchAll').checked == true) {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                        cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                    }
                    else {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Branch';
                        cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue;
                    }
                }
                else //////////////Group By selected are Group
                {
                    if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                        cmbVal = cmbVal + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                    }
                    else {
                        cmbVal = document.getElementById('cmbsearchOption').value + 'Group';
                        cmbVal = cmbVal + '~' + 'Selected' + '~' + groupvalue;
                    }
                }
            }
            else {
                cmbVal = document.getElementById('cmbsearchOption').value;
                cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
            }
            cmbVal = cmbVal + 'SelectedSegmentID,' + '<%=Session["UserSegID"]%>';
         ajax_showOptions(objID, objListFun, objEvent, cmbVal);
     }
     function Filter() {

         Show('tab_selection');
         Hide('showFilter');
         Hide('tab_grid');
         Hide('td_grid');
         Hide('td_sapn');
         Hide('tr_ddl');
         Hide('td_filter');
         height();

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
         var s = document.getElementById('txtClientSelectionID');
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
         document.getElementById('ShowSelectUser').style.display = 'none';
         document.getElementById('btn_show').disabled = false;
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
     function displaydate(obj) {
         document.getElementById('spanshow2').innerText = obj;
     }
     function line() {
         Hide('tab_selection');
         Show('tab_grid');
         Show('td_grid');
         Show('td_sapn');
         Show('tr_ddl');
         Show('td_filter');
         window.frameElement.height = document.body.scrollHeight;

         var gwidth = screen.width;
         gwidth = gwidth - 30;
         document.getElementById('divgrid').style.overflow = 'scroll';
         document.getElementById('divgrid').style.width = gwidth + 'px'
     }
     function ChangeRowColor(rowID, rowNumber) {

         var gridview = document.getElementById('grdBilling');
         var rCount = gridview.rows.length;
         var rowIndex = 1;
         var rowCount = 0;
         if (rCount == 18)
             rowCount = 15;
         else
             rowCount = rCount - 2;
         if (rowNumber > 15 && rCount < 18)
             rowCount = rCount - 3;
         for (rowIndex; rowIndex <= rowCount; rowIndex++) {
             var rowElement = gridview.rows[rowIndex];
             rowElement.style.backgroundColor = '#FFFFFF'
         }
         var color = document.getElementById(rowID).style.backgroundColor;
         if (color != '#ffe1ac') {
             oldColor = color;
         }
         if (color == '#ffe1ac') {
             document.getElementById(rowID).style.backgroundColor = oldColor;
         }
         else
             document.getElementById(rowID).style.backgroundColor = '#ffe1ac';

     }
     function Disable(obj) {
         var gridview = document.getElementById('grdBilling');
         var rCount = gridview.rows.length;

         if (rCount < 10)
             rCount = '0' + rCount;
         if (obj == 'P') {
             document.getElementById("grdBilling_ctl" + rCount + "_FirstPage").style.display = 'none';
             document.getElementById("grdBilling_ctl" + rCount + "_PreviousPage").style.display = 'none';
             document.getElementById("grdBilling_ctl" + rCount + "_NextPage").style.display = 'inline';
             document.getElementById("grdBilling_ctl" + rCount + "_LastPage").style.display = 'inline';
         }
         else {
             document.getElementById("grdBilling_ctl" + rCount + "_NextPage").style.display = 'none';
             document.getElementById("grdBilling_ctl" + rCount + "_LastPage").style.display = 'none';
         }
     }
     function DisableC(obj) {

         if (obj == 'P') {

             document.getElementById('lnkPrevClient').style.display = 'none';
             document.getElementById('lnkNextClient').style.display = 'inline';
         }
         else {
             document.getElementById('lnkPrevClient').style.display = 'inline';
             document.getElementById('lnkNextClient').style.display = 'none';
         }
     }
     FieldName = 'lstSlection';

     //THIS IS FOR EMAIL

     function btnAddEmailtolist_click() {

         var cmb = document.getElementById('cmbsearch');

         var userid = document.getElementById('txtSelectID');
         if (userid.value != '') {
             var ids = document.getElementById('txtSelectID_hidden');
             var listBox = document.getElementById('SelectionList');
             var tLength = listBox.length;


             var no = new Option();
             no.value = ids.value;
             no.text = userid.value;
             listBox[tLength] = no;
             var recipient = document.getElementById('txtSelectID');
             recipient.value = '';
         }
         else
             alert('Please search name and then Add!')
         var s = document.getElementById('txtSelectID');
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

             document.getElementById('ShowTable').style.display = 'none';
             document.getElementById('showFilter1').style.display = 'inline';
         }
         else {
             alert("Please select email from list.")
         }

         var i;
         for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
             listBoxSubs.remove(i);
         }

         window.frameElement.height = document.body.scrollHeight;
     }

     function btnRemoveEmailFromlist_click() {

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
         var Data = rValue.split('~');
         if (Data[0] == 'Clients') {
         }
     }

     function Sendmail() {


         var cmbVal = document.getElementById('ddlGroup').value;
         if (cmbVal == "0") {

             document.getElementById('lblSelectBrCl').value = 'Respective Branch';
             document.getElementById('ShowSelectUser').style.display = 'inline';
             document.getElementById('ShowTable').style.display = 'none';
             document.getElementById('showFilter1').style.display = 'inline';
             window.frameElement.height = document.body.scrollHeight;

         }
         else if (cmbVal == "1") {

             document.getElementById('lblSelectBrCl').value = 'Respective Group';
             document.getElementById('ShowSelectUser').style.display = 'inline';
             document.getElementById('ShowTable').style.display = 'none';
             document.getElementById('showFilter1').style.display = 'inline';
             window.frameElement.height = document.body.scrollHeight;

         }
         window.frameElement.height = document.body.scrollHeight;
         //        
         //             document.getElementById('ShowSelectUser').style.display='inline';
         //             document.getElementById('ShowTable').style.display='none';
         //             document.getElementById('showFilter1').style.display='inline';
         //             window.frameElement.height = document.body.scrollHeight;

     }
     function ForFilterOff() {
         selecttion();
         document.getElementById('ShowTable').style.display = 'none';
         document.getElementById('showFilter1').style.display = 'none';
         Show('tdgrdBrkgclient');
         Show('tr_export');
         Hide('tr_btn');
         Hide('tr_date');
         Hide('tr_under');
         window.frameElement.height = document.body.scrollHeight;
     }
     function MailsendT() {
         window.frameElement.height = document.body.scrollHeight;
         alert("Mail Sent Successfully");
     }
     function MailsendF() {
         window.frameElement.height = document.body.scrollHeight;
         alert("Error on sending!Try again..");
     }
     function MailsendFT() {
         alert("Email id could not found!Try again...");
     }

     function keyVal(obj) {
         document.getElementById('SelectionList').style.display = 'inline';

     }

     function SendmailFilter() {
         document.getElementById('ShowSelectUser').style.display = 'none';
         document.getElementById('ShowTable').style.display = 'none';
         document.getElementById('showFilter1').style.display = 'none';
         window.frameElement.height = document.body.scrollHeight;

     }
     function fnbroker(obj) {
         if (obj == "a")
             Hide('showFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Broker';
             Show('showFilter');
         }
         selecttion();
     }
     function fnddlview(obj) {
         if (obj == "1") {
             Show('tr_client');
             Hide('Tr_Broker');
         }
         if (obj == "2") {
             Hide('tr_client');
             Show('Tr_Broker');

         }
         if (obj == "3") {
             Hide('tr_client');
             Hide('Tr_Broker');

         }
         selecttion();
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
         selecttion();
     }
     function Branch(obj) {
         if (obj == "a") {
             Hide('showFilter');
         }
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Branch';
             Show('showFilter');
         }
         selecttion();
     }
     function Group(obj) {
         if (obj == "a") {
             Hide('showFilter');
         }
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Group';
             Show('showFilter');
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
     function Clients(obj) {
         if (obj == "a")
             Hide('showFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Clients';
             Show('showFilter');
         }
         selecttion();
     }
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

    </script>
    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            var j = rValue.split('~');

            if (j[0] == 'Group') {
                groupvalue = j[1];
            }
            if (j[0] == 'Branch') {
                groupvalue = j[1];
            }
            var btn = document.getElementById('btnhide');
            btn.click();
        }
        function btndisplay(obj) {

            if (obj == '1')////select screen
            {

                Show('td_Screen');
                Hide('td_Excel');
                Hide('td_PDF');
                Hide('showFilter');
            }
            if (obj == '2')////select export
            {

                Hide('td_Screen');
                Show('td_Excel');
                Show('td_PDF');
                Hide('td_mail');
                Hide('showFilter');
            }
            if (obj == '3')////select email
            {

                Hide('td_Screen');
                Hide('td_Excel');
                Hide('td_PDF');
                Show('showFilter');
            }

        }
        function fnddlGeneration(obj) {
            btndisplay(obj);
        }
        //function NoRecord()
        //  {
        //    
        //    Show('tab_All');
        //    Hide('displayAll');
        //    alert('No record Found');
        //    height();
        //  }
        function fnddlType(obj) {
            if (obj == '1')////select Client Wise
            {
                Show('td_PDF');
            }
            if (obj == '2')////select Branch Wise
            {
                Hide('td_PDF');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
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
            <tr class="EHEADER">
                <td colspan="0" style="text-align: center; height: 22px;">
                    <strong><span id="SpanHeader" style="color: #000099">Obligation/Margin Summary</span></strong>
                </td>
                <td style="width: 25%;" id="td_filter">
                    <a href="javascript:void(0);" onclick="Sendmail();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Send Email</span></a>|| <a href="javascript:void(0);"
                        onclick="Filter();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a> &nbsp; &nbsp;||&nbsp;
                        <asp:DropDownList ID="ddlExport" Font-Size="Smaller" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                        </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="tab_selection">
            <tr>
                <td valign="top" style="height: 247px">
                    <table>
                        <tr>
                            <td valign="top" bgcolor="#B7CEEC" class="gridcellleft">For :
                            </td>
                            <td style="width: 100px">
                                <dxe:ASPxDateEdit ID="dtFor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfor">
                                    <DropDownButton Text="For">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#B7CEEC" class="gridcellleft">Generate Type :
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlGeneration" runat="server" Width="130px" Font-Size="12px"
                                    onchange="fnddlGeneration(this.value)">
                                    <asp:ListItem Value="1">Screen</asp:ListItem>
                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#B7CEEC" class="gridcellleft" style="width: 122px">Group By</td>
                            <td style="width: 100px">
                                <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                    <asp:ListItem Value="0">Branch</asp:ListItem>
                                    <asp:ListItem Value="1">Group</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="2" id="td_branch">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="Branch('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="Branch('b')" />Selected
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
                                                onclick="Group('a')" />
                                            All
                                                <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="Group('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--<tr>
                                <td bgcolor="#B7CEEC" class="gridcellleft" style="width: 122px">
                                    Clients :</td>
                                <td style="width: 100px">
                                    <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="Clients('a')" />
                                    All Client
                                </td>
                                 <td>
                                    <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="Clients('b')" />
                                    Selected Client
                                </td>
                              
                            </tr>--%>
                        <tr id="Tr_viewby" class="tableBorderClass">
                            <td colspan="3">
                                <table class="tableBorderClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">View By :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlviewby" runat="server" Width="100px" Font-Size="12px" onchange="fnddlview(this.value)">
                                                <asp:ListItem Value="1">Client</asp:ListItem>
                                                <asp:ListItem Value="2">Broker</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_client" class="tableBorderClass">
                            <td colspan="3">
                                <table class="tableBorderClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td bgcolor="#B7CEEC" class="gridcellleft">Clients :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="Clients('a')" />
                                            All Client
                                        </td>
                                        <%-- <td>
                                                <asp:RadioButton ID="radPOAClient" runat="server" GroupName="c" onclick="Clients('a')" />POA
                                                Client
                                            </td>--%>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="Clients('b')" />
                                            Selected Client
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_Broker">
                            <td colspan="3">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Broker :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbbrokerall" runat="server" Checked="True" GroupName="M" onclick="fnbroker('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbbrokerselected" runat="server" GroupName="M" onclick="fnbroker('b')" />
                                            Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">Margin Calculation Type:</td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlCalType" runat="server" Width="108px" Font-Size="12px">
                                    <asp:ListItem Value="1">Exchange</asp:ListItem>
                                    <asp:ListItem Value="2">Calculated</asp:ListItem>
                                </asp:DropDownList></td>


                        </tr>
                        <tr>
                            <td bgcolor="#B7CEEC" class="gridcellleft">Show Margin Break Up : 
                            </td>
                            <td>
                                <asp:CheckBox ID="chkShowMargin" runat="server" />
                            </td>

                        </tr>
                        <tr>
                            <td id="td_Screen" align="right">
                                <asp:Button ID="btn_show" OnClientClick="selecttion()" CssClass="btnUpdate" Height="20px"
                                    Width="90px" runat="server" Text="Screen" OnClick="btn_show_Click" />
                            </td>
                            <td id="td_Excel" align="right">
                                <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                    Width="101px" OnClientClick="selecttion()" OnClick="btnExcel_Click" />
                            </td>
                            <td id="td_PDF">
                                <asp:Button ID="btnPDF" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To PDF"
                                    Width="101px" OnClientClick="selecttion()" OnClick="btnPDF_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" colspan="4" style="height: 247px">
                    <table width="100%" id="showFilter">
                        <tr>
                            <td style="text-align: right; vertical-align: top; height: 134px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: right" id="TdFilter">
                                            <span id="spanunder"></span><span id="spanclient">
                                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="200px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox></span>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Broker</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
                                            </asp:DropDownList>
                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                    style="color: #009900; font-size: 8pt;"> </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: right"></td>
                                    </tr>
                                    <tr>
                                        <td align="left">
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
                        <tr style="display: none">
                            <td style="height: 23px" align="right">
                                <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
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
        <table id="tab_grid" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <table id="ShowSelectUser">
                                                <tr>
                                                    <td valign="top">
                                                        <asp:RadioButton ID="rbOnlyClient" runat="server" Checked="true" GroupName="h" />
                                                    </td>
                                                    <td valign="top">
                                                        <asp:TextBox ID="lblSelectBrCl" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td valign="top">
                                                        <asp:RadioButton ID="rbClientUser" runat="server" GroupName="h" />
                                                    </td>
                                                    <td valign="top">Selected User
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table id="ShowTable">
                                                <tr>
                                                    <td width="70px" style="text-align: left;">Type:</td>
                                                    <td class="gridcellleft" style="vertical-align: top; text-align: left" id="Td1">
                                                        <span id="span1">
                                                            <asp:DropDownList ID="cmbsearch" runat="server" Width="150px" Font-Size="12px">
                                                            </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="70px" style="text-align: left;">Select User:</td>
                                                    <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter1">
                                                        <span id="spanal2">
                                                            <asp:TextBox ID="txtSelectID" runat="server" Font-Size="12px" Width="285px"></asp:TextBox></span>
                                                        <a id="A3" href="javascript:void(0);" onclick="btnAddEmailtolist_click()"><span style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span style="color: #009900; font-size: 8pt;"> </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="70px" style="text-align: left;"></td>
                                                    <td style="text-align: left; vertical-align: top; height: 134px;">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>&nbsp;&nbsp;<asp:ListBox ID="SelectionList" runat="server" Font-Size="12px" Height="90px"
                                                                    Width="290px"></asp:ListBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <a id="AA2" href="javascript:void(0);" onclick="clientselection()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <a id="AA1" href="javascript:void(0);" onclick="btnRemoveEmailFromlist_click()"><span
                                                                                    style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="display: none">
                                                    <td width="70px" style="text-align: left;"></td>
                                                    <td style="height: 23px">
                                                        <asp:TextBox ID="txtSelectID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                        <asp:TextBox ID="dtFrom_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                        <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                        <asp:HiddenField ID="HiddenField_Group" runat="server" />
                                                        <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                                                        <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                                                        <asp:HiddenField ID="HiddenField_Client" runat="server" />
                                                        <asp:HiddenField ID="HiddenField_Broker" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table id="showFilter1">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btnUpdate"
                                                            Text="Send" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="display: none;">
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClientClick="selecttion()"
                        OnClick="btnhide_Click" /></td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="clientFilter" runat="server">
                                <tr id="tr_ddl">
                                    <td style="height: 44px">
                                        <asp:LinkButton ID="lnkPrevClient" runat="server" CommandName="Prev" Text="[Prev]"
                                            OnCommand="NavigationLinkC_Click" OnClientClick="selecttion()"> </asp:LinkButton>
                                    </td>
                                    <td style="height: 44px">
                                        <asp:DropDownList ID="cmbgroupPager" runat="server" Width="300px" Font-Size="12px"
                                            ValidationGroup="a" onchange="selecttion()" AutoPostBack="True" OnSelectedIndexChanged="cmbgroupPager_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="height: 44px">
                                        <asp:LinkButton ID="lnkNextClient" runat="server" CommandName="Next" Text="[Next]"
                                            OnCommand="NavigationLinkC_Click" OnClientClick="selecttion()"> </asp:LinkButton>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr id="td_sapn">
                                    <td colspan="3">
                                        <span id="span2" style="color: Blue; font-weight: bold">Obligation Summary Report :</span>&nbsp;&nbsp;<span
                                            id="spanshow2"></span>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td id="td_grid">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divgrid">
                                <asp:GridView ID="grdBilling" runat="server" BorderColor="CornflowerBlue" ShowFooter="True"
                                    AutoGenerateColumns="false" BorderStyle="Solid" AllowSorting="true" BorderWidth="2px"
                                    CellPadding="4" ForeColor="#0000C0" OnRowCreated="grdBilling_RowCreated">
                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Account Name">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblclientname" runat="server" Text='<%# Eval("clientname")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Account Code">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblucc" runat="server" Text='<%# Eval("ucc")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Opening Balance">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblopenBal" runat="server" Text='<%# Eval("OpeningBal")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Premium">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPremium" runat="server" Text='<%# Eval("Premium")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MTM">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMTM" runat="server" Text='<%# Eval("MTM")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fin Sett">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblFutureSettlement" runat="server" Text='<%# Eval("FutureSettlement")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Charges">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCharges" runat="server" Text='<%# Eval("Charges")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serv.Tax">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblServTax" runat="server" Text='<%# Eval("ServTax")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Net Oblgtn">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblNetObligation" runat="server" Text='<%# Eval("NetObligation")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Net Adjustment">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblNetAdj" runat="server" Text='<%# Eval("NetAdj")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Closing Balance">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblopenBal" runat="server" Text='<%# Eval("ClosingBal")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cash Margin Deposit">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCashMrgnDepost" runat="server" Text='<%# Eval("CashMarnDeposit")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Collateral Value">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCollaleteralVal" runat="server" Text='<%# Eval("ColeteralVal")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective Deposit">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffectiveDeposit" runat="server" Text='<%# Eval("EffecTiveDeposit")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Appl Margin">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblApplMrgn" runat="server" Text='<%# Eval("ApplMargin")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Excess/Shortage(-)" SortExpression="ExcessShortage">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblExcessShortage" runat="server" Text='<%# Eval("ExcessShortage")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Exposure" SortExpression="Exposure">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblExposure" runat="server" Text='<%# Eval("Exposure")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="10">
                                                    <asp:LinkButton ID="FirstPage" runat="server" Font-Bold="true" CommandName="First"
                                                        OnCommand="NavigationLink_Click" Text="[First Page]"> </asp:LinkButton>
                                                    <asp:LinkButton ID="PreviousPage" runat="server" Font-Bold="true" CommandName="Prev"
                                                        OnCommand="NavigationLink_Click" Text="[Previous Page]">  </asp:LinkButton>
                                                    <asp:LinkButton ID="NextPage" runat="server" Font-Bold="true" CommandName="Next"
                                                        OnCommand="NavigationLink_Click" Text="[Next Page]">
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="LastPage" runat="server" Font-Bold="true" CommandName="Last"
                                                        OnCommand="NavigationLink_Click" Text="[Last Page]">
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </PagerTemplate>
                                    <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                        BorderWidth="1px"></RowStyle>
                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                        Font-Bold="False"></HeaderStyle>
                                </asp:GridView>
                                <asp:HiddenField ID="TotalGROUP" runat="server" />
                                <asp:HiddenField ID="CurrentPage" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="TotalPages" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="TotalClient" runat="server" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbgroupPager" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lnkPrevClient" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lnkNextClient" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

