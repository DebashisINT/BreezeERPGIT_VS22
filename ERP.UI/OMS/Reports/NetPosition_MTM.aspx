<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_NetPosition_MTM" CodeBehind="NetPosition_MTM.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            overflow-y: no;
            overflow-x: scroll;
            width: 80%;
            scrollbar-base-color: #C0C0C0;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function Page_Load()///Call Into Page Load
  {
      Hide('showFilter');
      Hide('tr_filter');
      Hide('Tr_Broker');
      document.getElementById('hiddencount').value = 0;
      Hide('Tr_ExposureInBuyPosition');
      fn_ddldateChange('0');
      fnddlGeneration('0');

      height();
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
                function Hide(obj) {
                    document.getElementById(obj).style.display = 'none';
                }
                function Show(obj) {
                    document.getElementById(obj).style.display = 'inline';
                }
                function fn_ddldateChange(obj) {
                    if (obj == '0') {
                        Show('td_dtfor');
                        Hide('td_dtfrom');
                        Hide('td_dtto');
                        Show('td_charges');
                        //                ShowHidePdfButton();

                    }
                    else {
                        Hide('td_dtfor');
                        Show('td_dtfrom');
                        Show('td_dtto');
                        Hide('td_charges');
                    }
                    Hide('td_lstfiltercolumn');
                    if (document.body.scrollHeight >= 350) {
                        window.frameElement.height = document.body.scrollHeight;
                    }
                }
                function fnddlGeneration(obj) {
                    if (obj == '0') {
                        Show('td_show');
                        Hide('td_excel');
                        Hide('td_mail');

                        Hide('tr_mail');
                        Hide('showFilter');
                        Show('tr_filtercolumns');
                    }
                    if (obj == '1') {
                        Hide('td_show');
                        Hide('td_excel');

                        Show('td_mail');
                        Show('tr_mail');
                        Show('tr_filtercolumns');
                        document.getElementById('btnMailOption').click();
                    }
                    if (obj == '2') {

                        Hide('td_show');
                        Show('td_excel');

                        Hide('td_mail');
                        Hide('tr_mail');
                        Hide('showFilter');
                        Show('tr_filtercolumns');

                    }
                    if (obj == '3') {

                        Hide('td_show');
                        Show('td_excel');

                        Hide('td_mail');
                        Hide('tr_mail');
                        Hide('showFilter');
                        Hide('tr_filtercolumns');

                    }



                    if (document.body.scrollHeight >= 350) {
                        window.frameElement.height = document.body.scrollHeight;
                    }

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
                    else if (document.getElementById('cmbsearchOption').value == "Expiry") {
                        fnexpirtycallajax(objID, objListFun, objEvent);
                    }
                    else {
                        cmbVal = document.getElementById('cmbsearchOption').value;
                        cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
                    }

                    ajax_showOptions(objID, objListFun, objEvent, cmbVal);

                }
                function fnexpirtycallajax(objID, objListFun, objEvent) {
                    var date;
                    if (document.getElementById('ddldate').value == '0') {
                        date = new Date(dtfor.GetDate());
                        date = parseInt(date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                    }
                    else {
                        date = new Date(dtfrom.GetDate());
                        date = parseInt(date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                    }
                    ajax_showOptions(objID, 'Searchproductandeffectuntil', objEvent, 'Expiry' + '~' + date);
                }
                function FnClients(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'Clients';
                        Show('showFilter');
                    }
                    //selecttion();
                    height();
                }
                function FnBranch(obj) {
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
                    // selecttion();
                    height();
                }
                function FnGroup(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'Group';
                        Show('showFilter');
                    }
                    // selecttion();
                    height();
                }
                function FnInstrument(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'ScripsExchange';
                        Show('showFilter');
                    }
                    // selecttion();
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
                    document.getElementById('btnshow').disabled = false;
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

                    document.getElementById('btnMailOption').click();

                    // selecttion();
                    height();
                }
                function fngrouptype(obj) {
                    if (obj == "0") {
                        Hide('td_allselect');
                        alert('Please Select Group Type !');
                    }
                    else {
                        Show('td_allselect');
                    }
                    // selecttion();
                    height();
                }
                function Fnunderlying(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'Product';
                        Show('showFilter');
                    }
                    // selecttion();
                    height();
                }
                function FnExpiry(obj) {
                    if (obj == "a")
                        Hide('showFilter');
                    else {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'Expiry';
                        Show('showFilter');
                    }
                    //selecttion();
                    height();
                }
                function NORECORD(obj) {
                    Hide('displayAll');
                    Show('tab2');
                    Hide('showFilter');
                    fn_ddldateChange(document.getElementById('ddldate').value);
                    var ddlmailopt = document.getElementById('ddlGeneration');
                    ddlmailopt.value = '0';
                    fnddlGeneration(document.getElementById('ddlGeneration').value);
                    if (obj == '1')
                        alert('No Record Found !! ');
                    if (obj == '2')
                        alert("Mail Sent Successfully !!");
                    if (obj == '4')
                        alert("Error on sending!Try again.. !!");
                    if (obj == '5')
                        alert("'Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...'");
                    if (obj == '6')
                        alert("E-Mail Id Could Not Be Found !!");
                    if (obj == '7')
                        alert("You Have To Chcek Atleast Three Columns !!");

                    if (obj == '10')
                        Hide('tr_filter');

                    document.getElementById('hiddencount').value = 0;
                    height();
                }
                function Display(obj, colcount) {
                    Show('displayAll');
                    Hide('tab2');
                    Hide('showFilter');
                    Show('tr_filter');
                    document.getElementById('hiddencount').value = 0;
                    if (colcount > 8)
                        document.getElementById('Divdisplay').className = "grid_scroll";
                    if (obj == "2")
                        Hide('tr_Record');
                    else
                        Show('tr_Record');

                    height();
                    // selecttion();
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
                function selecttion() {
                    var combo = document.getElementById('cmbExport');
                    combo.value = 'Ex';
                }
                function heightlight(obj) {

                    var colorcode = obj.split('&');

                    if ((document.getElementById('hiddencount').value) == 0) {
                        prevobj = '';
                        prevcolor = '';
                        document.getElementById('hiddencount').value = 1;

                    }
                    document.getElementById(obj).style.backgroundColor = '#ffe1ac';

                    if (prevobj != '') {
                        document.getElementById(prevobj).style.backgroundColor = prevcolor;
                    }
                    prevobj = obj;
                    prevcolor = colorcode[1];

                }

                function fn_ddlrptchange(obj) {

                    if (obj == '0') {
                        Show('Tr_viewby');
                        Show('tr_client');
                        Show('Tr_Broker');



                        Show('tr_grpselection');
                        Show('td_charges');
                        Hide('Tr_ExposureInBuyPosition');
                        Show('Tr_Option');
                        Show('tr_filtercolumns');
                        //                ShowHidePdfButton();


                    }
                    if (obj == '1') {
                        // fnddlview('3');
                        Show('Tr_viewby');
                        Hide('tr_client');
                        Hide('Tr_Broker');

                        Hide('tr_grpselection');
                        Hide('td_charges');
                        Hide('Tr_ExposureInBuyPosition');
                        Show('Tr_Option');
                        Show('tr_filtercolumns');


                    }
                    if (obj == '2') {

                        Show('Tr_viewby');
                        Show('tr_client');
                        Show('Tr_Broker');
                        Show('tr_grpselection');
                        Hide('td_charges');
                        Show('Tr_ExposureInBuyPosition');
                        Hide('Tr_Option');
                        Hide('tr_filtercolumns');

                    }
                    if (obj == '3') {

                        Show('Tr_viewby');
                        Show('tr_client');
                        Show('Tr_Broker');
                        Show('tr_grpselection');
                        Hide('td_charges');
                        Hide('Tr_ExposureInBuyPosition');
                        Hide('Tr_Option');
                        Hide('tr_filtercolumns');

                    }

                    fnddlGeneration(document.getElementById('ddlGeneration').value);
                    // selecttion();

                    height();

                }



                function ShowHidePdfButton() {

                    var date = document.getElementById('ddldate').value;

                    var rpt = document.getElementById('ddlrptview').value;

                    var gen = document.getElementById('ddlGeneration').value;

                    if (date == "0" && rpt == "0" && gen == "2") {
                        Show('td_pdf');
                    }
                    else {
                        Hide('td_pdf');
                    }


                }
                function fnddloptionformail(obj) {
                    if (obj == '2') {
                        var cmb = document.getElementById('cmbsearchOption');
                        cmb.value = 'MAILEMPLOYEE';
                        Show('showFilter');
                    }
                    else
                        Hide('showFilter');
                }
                function fncolumnsdisplay(obj) {
                    if (obj.checked == true) {
                        Show('td_lstfiltercolumn');
                    }
                    else {
                        Hide('td_lstfiltercolumn');
                    }
                    height();
                }
                function OnConsiderChange(obj) {
                    if (obj == 1) {
                        document.getElementById('ChkCalculateCharge').checked = false;
                        document.getElementById('ChkCalculateCharge').disabled = true;
                    }
                    else {
                        document.getElementById('ChkCalculateCharge').checked = true;
                        document.getElementById('ChkCalculateCharge').disabled = false;
                    }
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
      if (j[0] == 'Broker') {
          document.getElementById('HiddenField_Broker').value = j[1];
      }
      if (j[0] == 'BranchGroup') {
          document.getElementById('HiddenField_BranchGroup').value = j[1];
      }
      if (j[0] == 'Product') {
          document.getElementById('HiddenField_Product').value = j[1];
      }
      if (j[0] == 'Expiry') {
          document.getElementById('HiddenField_Expiry').value = j[1];
      }
      if (j[0] == 'ScripsExchange') {
          document.getElementById('HiddenField_ScripsExchange').value = j[1];
      }
      if (j[0] == 'MAILEMPLOYEE') {
          document.getElementById('HiddenField_emmail').value = j[1];
      }
  }
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
                    <strong><span id="SpanHeader" style="color: #000099">Net Position </span></strong>
                </td>
                <td class="EHEADER" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="NORECORD(10);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                </td>
            </tr>
        </table>
        <table id="tab2" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table border="10" cellpadding="1" cellspacing="1">
                                                <tr valign="top">
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddldate" runat="server" Width="100px" Font-Size="12px" onchange="fn_ddldateChange(this.value)">
                                                                        <asp:ListItem Value="0">For A Date</asp:ListItem>
                                                                        <asp:ListItem Value="1">For A Period</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td id="td_dtfor">
                                                                    <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                        Font-Size="12px" Width="108px" ClientInstanceName="dtfor">
                                                                        <DropDownButton Text="For">
                                                                        </DropDownButton>
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                                <td id="td_dtfrom">
                                                                    <dxe:ASPxDateEdit ID="dtfrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                        Font-Size="12px" Width="108px" ClientInstanceName="dtfrom">
                                                                        <DropDownButton Text="From">
                                                                        </DropDownButton>
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                                <td id="td_dtto" class="gridcellleft">
                                                                    <dxe:ASPxDateEdit ID="dtto" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                        Font-Size="12px" Width="108px" ClientInstanceName="dtto">
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
                                        <td class="gridcellleft" valign="top">
                                            <table border="10" cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Report View :</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlrptview" runat="server" Width="200px" Font-Size="12px" onchange="fn_ddlrptchange(this.value)">
                                                            <asp:ListItem Value="0">Client Wise</asp:ListItem>
                                                            <asp:ListItem Value="1">Share Wise</asp:ListItem>
                                                            <asp:ListItem Value="2">Only Open Position With Exposure</asp:ListItem>
                                                            <asp:ListItem Value="3">Purchase-Sale Consolidated</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <table>
                                    <tr valign="top">
                                        <td>
                                            <table>
                                                <tr id="tr_grpselection">
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td bgcolor="#B7CEEC">Group By</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                                        <asp:ListItem Value="0">Branch</asp:ListItem>
                                                                        <asp:ListItem Value="1">Group</asp:ListItem>
                                                                        <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td id="td_branch">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="FnBranch('a')" />
                                                                                All
                                                                            </td>
                                                                            <td>
                                                                                <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="FnBranch('b')" />Selected
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
                                                                                <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="b"
                                                                                    onclick="FnGroup('a')" />
                                                                                All
                                                                                    <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="FnGroup('b')" />Selected
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="Tr_viewby">
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
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
                                                <tr id="tr_client">
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td class="gridcellleft" bgcolor="#B7CEEC">Clients :</td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="FnClients('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="radPOAClient" runat="server" GroupName="c" onclick="FnClients('a')" />POA
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="FnClients('b')" />
                                                                    Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="Tr_Broker">
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
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
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td bgcolor="#B7CEEC">Underlying:
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbunderlyingAll" runat="server" Checked="true" GroupName="d"
                                                                        onclick="Fnunderlying('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbunderlyingSelected" runat="server" GroupName="d" onclick="Fnunderlying('b')" />
                                                                    Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td bgcolor="#B7CEEC">Instrument:
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbInstrumentAll" runat="server" Checked="true" GroupName="dd"
                                                                        onclick="FnInstrument('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbInstrumentSelected" runat="server" GroupName="dd" onclick="FnInstrument('b')" />
                                                                    Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td bgcolor="#B7CEEC">Expiry:
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbExpiryAll" runat="server" Checked="true" GroupName="dk" onclick="FnExpiry('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbExpirySelected" runat="server" GroupName="dk" onclick="FnExpiry('b')" />
                                                                    Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table border="10" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td bgcolor="#B7CEEC">Consider :</td>
                                                                <td colspan="2" style="text-align: left" valign="top">
                                                                    <asp:DropDownList ID="ddlNetMktValue" runat="server" Width="100px" Font-Size="12px" onchange="OnConsiderChange(this.value)">
                                                                        <asp:ListItem Value="0">Net Value</asp:ListItem>
                                                                        <asp:ListItem Value="1">Market Value</asp:ListItem>
                                                                    </asp:DropDownList>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table id="showFilter" border="10" cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td class="gridcellleft">
                                                        <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                                        <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                            Enabled="false">
                                                            <asp:ListItem>Clients</asp:ListItem>
                                                            <asp:ListItem>Broker</asp:ListItem>
                                                            <asp:ListItem>Branch</asp:ListItem>
                                                            <asp:ListItem>Group</asp:ListItem>
                                                            <asp:ListItem>Product</asp:ListItem>
                                                            <asp:ListItem>ScripsExchange</asp:ListItem>
                                                            <asp:ListItem>Expiry</asp:ListItem>
                                                            <asp:ListItem>BranchGroup</asp:ListItem>
                                                            <asp:ListItem>MAILEMPLOYEE</asp:ListItem>
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
                        <tr id="Tr_Option">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkOpenClientFUT" runat="server" onclick="fn_charge(this)" />
                                            Show Only Open Position Clients
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC" id="td_charges">
                                            <asp:CheckBox ID="ChkCalculateCharge" Checked="true" runat="server" onclick="selecttion()" />
                                            Calculate Charges
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="Chksign" Checked="true" runat="server" onclick="selecttion()" />
                                            Show Open Buy Position in +ve Sign
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_filtercolumns">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td id="td_btnfiltercolumn" class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="chkfiltercolumns" runat="server" onclick="fncolumnsdisplay(this)" />
                                            Filter Columns
                                        </td>
                                        <td id="td_lstfiltercolumn">
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
                                                                <asp:ListItem Value="BF Lots" Selected="True">BF Lots</asp:ListItem>
                                                                <asp:ListItem Value="Open Price" Selected="True">Open Price</asp:ListItem>
                                                                <asp:ListItem Value="BF Value" Selected="True">BF Value</asp:ListItem>
                                                                <asp:ListItem Value="Buy Lots" Selected="True">Buy Lots</asp:ListItem>
                                                                <asp:ListItem Value="Buy Value" Selected="True">Buy Value</asp:ListItem>
                                                                <asp:ListItem Value="Buy Avg." Selected="True">Buy Avg.</asp:ListItem>
                                                                <asp:ListItem Value="Sell Lots" Selected="True">Sell Lots</asp:ListItem>
                                                                <asp:ListItem Value="Sell Value" Selected="True">Sell Value</asp:ListItem>
                                                                <asp:ListItem Value="Sell Avg." Selected="True">Sell Avg.</asp:ListItem>
                                                                <asp:ListItem Value="Asn/Exc Lot" Selected="True">Asn/Exc Lot</asp:ListItem>
                                                                <asp:ListItem Value="CF Lots" Selected="True">CF Lots</asp:ListItem>
                                                                <asp:ListItem Value="Sett Price" Selected="True">Sett Price</asp:ListItem>
                                                                <asp:ListItem Value="CF Value" Selected="True">CF Value</asp:ListItem>
                                                                <asp:ListItem Value="Mtm" Selected="True">Mtm</asp:ListItem>
                                                                <asp:ListItem Value="Premium" Selected="True">Premium</asp:ListItem>
                                                                <asp:ListItem Value="Fin Sett" Selected="True">Fin Sett</asp:ListItem>
                                                                <asp:ListItem Value="Asn/Exc Value" Selected="True">Asn/Exc Value</asp:ListItem>
                                                                <asp:ListItem Value="Net Obligation" Selected="True">Net Obligation</asp:ListItem>
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_ExposureInBuyPosition">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Consider Exposure In Buy Position
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="ChkCall" runat="server" />Call
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkPut" runat="server" />Put
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
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr id="Tr_Generation">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGeneration" runat="server" Width="100px" Font-Size="12px"
                                                onchange="fnddlGeneration(this.value)">
                                                <asp:ListItem Value="0">Screen</asp:ListItem>
                                                <asp:ListItem Value="1">Send Mail</asp:ListItem>
                                                <asp:ListItem Value="2">Export To Excel</asp:ListItem>
                                                <asp:ListItem Value="3">Export To Pdf</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="tr_mail">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Respective :</td>
                                        <td>
                                            <asp:UpdatePanel ID="upanelGenerateType" UpdateMode="Conditional" runat="Server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddloptionformail" runat="server" Width="100px" Font-Size="12px"
                                                        onchange="fnddloptionformail(this.value)">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnMailOption" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td id="td_show">
                                                        <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                            Width="101px" OnClientClick="selecttion()" OnClick="btnshow_Click" /></td>
                                                    <td id="td_mail">
                                                        <asp:Button ID="btnmailsend" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Mail"
                                                            Width="101px" OnClientClick="selecttion()" OnClick="btnmailsend_Click" /></td>
                                                    <td id="td_excel">
                                                        <asp:Button ID="btnexcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export"
                                                            Width="101px" OnClientClick="selecttion()" OnClick="btnexcel_Click" /></td>
                                                    <%-- <td id="td_pdf">
                                                            <asp:Button ID="btnPdf" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Pdf"
                                                                Width="101px" OnClientClick="selecttion()"  /></td>--%>
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
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Broker" runat="server" />
                    <asp:HiddenField ID="HiddenField_Product" runat="server" />
                    <asp:HiddenField ID="HiddenField_ScripsExchange" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                    <asp:HiddenField ID="HiddenField_emmail" runat="server" />
                    <asp:HiddenField ID="HiddenField_Expiry" runat="server" />
                    <asp:Button ID="btnMailOption" runat="Server" OnClick="SetGenerateType" />
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
        <div id="displayAll" style="display: none; width: 99%">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="99%" border="1">
                        <tr style="display: none;">
                            <td>
                                <asp:HiddenField ID="hiddencount" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="DivHeader" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr id="tr_Record">
                            <td>
                                <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" runat="server">
                                            <tr valign="top">
                                                <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Prev" Text="[Prev]" OnClientClick="javascript:selecttion();"
                                                        OnCommand="NavigationLinkC_Click"> </asp:LinkButton>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:DropDownList ID="DdlRecord" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="DdlRecord_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkNext" runat="server" CommandName="Next" Text="[Next]" OnClientClick="javascript:selecttion();"
                                                        OnCommand="NavigationLinkC_Click"> </asp:LinkButton>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="Divdisplay" runat="server">
                                </div>
                            </td>
                        </tr>
                        <asp:HiddenField ID="TotalGrp" runat="server" />
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
