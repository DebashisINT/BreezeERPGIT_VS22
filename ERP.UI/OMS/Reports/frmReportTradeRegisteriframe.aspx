<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReportComTradeRegisteriframe" EnableEventValidation="false" Codebehind="frmReportTradeRegisteriframe.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

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
	.grid_scroll
    {
        overflow-y: no;  
        overflow-x: scroll; 
        width:98%;
        scrollbar-base-color: #C0C0C0;
    
    }
	</style>

    <script language="javascript" type="text/javascript">

        function Page_Load()///Call Into Page Load
        {
            Hide('showFilter');
            Hide('tr_filter');
            fnddlGeneration('1');
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
        function DateCompare(DateobjFrm, DateobjTo) {
            var Msg = "To Date Can Not Be Less Than From Date!!!";
            DevE_CompareDateForMin(DateobjFrm, DateobjTo, Msg);
        }
        function DevE_CompareDateForMin(DateObjectFrm, DateObjectTo, Msg) {
            var SelectedDate = new Date(DateObjectFrm.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();
            var SelectedDateValueFrm = new Date(year, monthnumber, monthday);

            var SelectedDate = new Date(DateObjectTo.GetDate());
            monthnumber = SelectedDate.getMonth();
            monthday = SelectedDate.getDate();
            year = SelectedDate.getYear();
            var SelectedDateValueTo = new Date(year, monthnumber, monthday);
            var SelectedDateNumericValueFrm = SelectedDateValueFrm.getTime();
            var SelectedDateNumericValueTo = SelectedDateValueTo.getTime();
            if (SelectedDateNumericValueFrm > SelectedDateNumericValueTo) {
                alert(Msg);
                DateObjectTo.SetDate(new Date(DateObjectFrm.GetDate()));
            }
        }
        function DateChangeForFrom() {
            var sessionVal = "<%=Session["LastFinYear"]%>";
        var objsession = sessionVal.split('-');
        var MonthDate = dtFrom.GetDate().getMonth() + 1;
        var DayDate = dtFrom.GetDate().getDate();
        var YearDate = dtFrom.GetDate().getYear();
        if (YearDate >= objsession[0]) {
            if (MonthDate < 4 && YearDate == objsession[0]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                dtFrom.SetDate(new Date(datePost));
            }
            else if (MonthDate > 3 && YearDate == objsession[1]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                dtFrom.SetDate(new Date(datePost));
            }
            else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                dtFrom.SetDate(new Date(datePost));
            }
        }
        else {
            alert('Enter Date Is Outside Of Financial Year !!');
            var datePost = (4 + '-' + 1 + '-' + objsession[0]);
            dtFrom.SetDate(new Date(datePost));
        }
    }
    function DateChangeForTo() {
        var sessionVal = "<%=Session["LastFinYear"]%>";
        var objsession = sessionVal.split('-');
        var MonthDate = dtTo.GetDate().getMonth() + 1;
        var DayDate = dtTo.GetDate().getDate();
        var YearDate = dtTo.GetDate().getYear();

        if (YearDate <= objsession[1]) {
            if (MonthDate < 4 && YearDate == objsession[0]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                dtTo.SetDate(new Date(datePost));
            }
            else if (MonthDate > 3 && YearDate == objsession[1]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                dtTo.SetDate(new Date(datePost));
            }
            else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                dtTo.SetDate(new Date(datePost));
            }
        }
        else {
            alert('Enter Date Is Outside Of Financial Year !!');
            var datePost = (3 + '-' + 31 + '-' + objsession[1]);
            dtTo.SetDate(new Date(datePost));
        }
    }
    function ddltradetypechange(obj) {
        if (obj == "4") {
            document.getElementById('radPOAClient').disabled = true;
            document.getElementById('rdbClientSelected').disabled = true;

        }
        else {
            document.getElementById('radPOAClient').disabled = false;
            document.getElementById('rdbClientSelected').disabled = false;
        }
        document.getElementById('rdbClientALL').checked = true;
    }
    function FunClientScrip(objID, objListFun, objEvent) {
        var cmbVal;
        if (document.getElementById('cmbsearchOption').value == "Clients") {
            if (document.getElementById('ddlGroup').value == "0" || document.getElementById('ddlGroup').value == "2")//////////////Group By  selected are branch
            {
                if (document.getElementById('ddlGroup').value == "0")/////branch selection
                {
                    if (document.getElementById('rdbranchAll').checked == true) {
                        cmbVal = 'ClientsBranch' + '~' + 'ALL';
                    }
                    else {
                        cmbVal = 'ClientsBranch' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Branch').value;
                    }
                }
                if (document.getElementById('ddlGroup').value == "2")/////branch-group selection
                {
                    if (document.getElementById('rdbranchAll').checked == true) {
                        cmbVal = 'ClientsBranchGroup' + '~' + 'ALL';
                    }
                    else {
                        cmbVal = 'ClientsBranchGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_BranchGroup').value;
                    }
                }
            }
            else if (document.getElementById('ddlGroup').value == "1")//////////////Group By selected are Group
            {
                if (document.getElementById('rdddlgrouptypeAll').checked == true) {
                    cmbVal = 'ClientsGroup' + '~' + 'ALL' + '~' + document.getElementById('ddlgrouptype').value;
                }
                else {
                    cmbVal = 'ClientsGroup' + '~' + 'Selected' + '~' + document.getElementById('HiddenField_Group').value;
                }
            }
            else if (document.getElementById('ddlGroup').value == "3")///////client wise
            {
                cmbVal = 'ClientsBranch' + '~' + 'ALL';
            }
        }
        else if (document.getElementById('cmbsearchOption').value == "ScripsExchange") {
            var dateto;
            dateto = new Date(dtTo.GetDate());
            dateto = parseInt(dateto.getMonth() + 1) + '-' + dateto.getDate() + '-' + dateto.getFullYear();

            var criteritype = 'B';
            criteritype = ' AND Commodity_ExpiryDate>="' + dateto + '"  ';
            criteritype = criteritype.replace('"', "'");
            criteritype = criteritype.replace('"', "'");
            cmbVal = document.getElementById('cmbsearchOption').value + '~' + 'Date' + '~' + criteritype;
        }
        else {
            cmbVal = document.getElementById('cmbsearchOption').value;
        }
        if (cmbVal == 'Group') {
            var grpType = document.getElementById('<%=ddlgrouptype.ClientID%>').value;
            if (grpType != undefined)
                cmbVal = 'Group~' + grpType;
        }
        ajax_showOptions(objID, 'ShowClientFORMarginStocks', objEvent, cmbVal);
    }
    function fnTerminalCTCLcallajax(objID, objListFun, objEvent, ObjCriteria) {
        var datefrom;
        var dateto;
        var date;

        datefrom = new Date(dtFrom.GetDate());
        dateto = new Date(dtTo.GetDate());

        datefrom = parseInt(datefrom.getMonth() + 1) + '-' + datefrom.getDate() + '-' + datefrom.getFullYear();
        dateto = parseInt(dateto.getMonth() + 1) + '-' + dateto.getDate() + '-' + dateto.getFullYear();

        if (ObjCriteria == 'TERMINALID')
            ObjCriteria = 'TerminalIdCriteria' + '~' + "ComExchangeTrades_Tradedate between '" + datefrom + "' and '" + dateto + "'";
        else
            ObjCriteria = 'CTCLIdCriteria' + '~' + "ComExchangeTrades_Tradedate between '" + datefrom + "' and '" + dateto + "'";

        ajax_showOptions(objID, 'ShowClientFORMarginStocks', objEvent, ObjCriteria);
    }
    function fn_Terminal(obj) {
        if (obj == "a")
            Hide('td_terminaltxt');
        else
            Show('td_terminaltxt');
        selecttion();
        height();
    }
    function fn_CTCL(obj) {
        if (obj == "a")
            Hide('td_ctcltxt');
        else
            Show('td_ctcltxt');
        selecttion();
        height();
    }
    function fn_Branch(obj) {
        if (obj == "a") {
            Hide('showFilter');
            document.getElementById('rdbranchclientAll').checked = true;
        }
        else {
            document.getElementById('rdbranchclientSelected').checked = true;
            if (document.getElementById('ddlGroup').value == "0") {
                document.getElementById('cmbsearchOption').value = 'Branch';
            }
            if (document.getElementById('ddlGroup').value == "2") {
                document.getElementById('cmbsearchOption').value = 'BranchGroup';
            }
            Show('showFilter');
        }

    }
    function fn_Broker(obj) {
        if (obj == "a") {
            Hide('showFilter');
            document.getElementById('rdbranchclientAll').checked = true;
        }
        else {
            document.getElementById('rdbranchclientSelected').checked = true;
            var cmb = document.getElementById('cmbsearchOption');
            cmb.value = 'Broker';
            Show('showFilter');
        }
    }
    function fn_Clients(obj) {
        if (obj == "a") {
            Hide('showFilter');
            document.getElementById('rdbranchclientAll').checked = true;
        }
        else {
            document.getElementById('rdbranchclientSelected').checked = true;
            var cmb = document.getElementById('cmbsearchOption');
            cmb.value = 'Clients';
            Show('showFilter');
        }
    }
    function fn_Group(obj) {
        if (obj == "a")
            Hide('showFilter');
        else {
            var cmb = document.getElementById('cmbsearchOption');
            cmb.value = 'Group';
            Show('showFilter');
        }
    }
    function fnddlGroup(obj) {
        if (obj == "0" || obj == "2" || obj == "3" || obj == "4") {
            if (document.getElementById('ddlGeneration').value == '2') {
                if (obj == "3")
                    fn_Clients('b');
                if (obj == "4")
                    fn_Broker('b');
                else
                    fn_Branch('a');
            }
            else {
                if (obj == "3")
                    fn_Clients('b');
                if (obj == "4")
                    fn_Broker('b');
                if (obj == "0")
                    fn_Branch('b');
                //                 else
                //                    fn_Branch('b');  
            }
            Hide('td_group');
            Show('td_branch');
        }
        else {
            fn_Group('a');
            Show('td_group');
            Hide('td_branch');
            var btn = document.getElementById('btnhide');
            btn.click();
        }
        selecttion();
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
        var cmb = document.getElementById('cmbsearchOption');
        cmb.value = 'Group';
        selecttion();
        height();
    }
    function btndisplay(obj) {
        if (obj == '1')////select a type
        {
            Hide('td_btnshow');
            Hide('td_export');
            Hide('td_email');
            Hide('tr_mail');
            Hide('Tr_FilterColumn');
            Hide('Tr_ChkClientColumn');
        }
        if (obj == '2')////select screen
        {
            Show('td_btnshow');
            Hide('td_export');
            Hide('td_email');
            Hide('tr_mail');
            Hide('Tr_FilterColumn');
            Hide('Tr_ChkClientColumn');
        }
        if (obj == '3')////select export
        {
            Hide('td_btnshow');
            Show('td_export');
            Hide('td_email');
            Hide('tr_mail');
            Show('Tr_FilterColumn');
            Show('Tr_ChkClientColumn');
        }
        if (obj == '4')////select email
        {
            Hide('td_btnshow');
            Hide('td_export');
            Show('td_email');
            Show('tr_mail');
            Hide('Tr_FilterColumn');
            Hide('Tr_ChkClientColumn');
        }
        height();
    }
    function SelectAllFilter(chk) {
        $('#<%=ChkFilterDetail.ClientID %>').find("input:checkbox").each(function () {
             if (this != chk) {
                 this.checked = chk.checked;
             }
         });

     }
     function fnddlGeneration(obj) {
         if (obj == '1')////select a type
         {
             Hide('tab1');
         }
         if (obj == '2')////select screen
         {
             Show('tab1');
         }
         if (obj == '3')////select export
         {
             Show('tab1');
         }
         if (obj == '4')////select email
         {
             Show('tab1');
         }
         btndisplay(obj);
         ddlgroupselection(document.getElementById('ddlGroup').value, document.getElementById('ddlGeneration').value);
     }
     function ddlgroupselection(groupby, selecttype) {
         if (selecttype == '2') {
             document.getElementById('rdbranchclientSelected').checked = true;
             document.getElementById('rdbranchclientAll').checked = false;
             document.getElementById('rdbranchclientAll').disabled = true;
             document.getElementById('rdddlgrouptypeSelected').checked = true;
             document.getElementById('rdddlgrouptypeAll').checked = false;
             document.getElementById('rdddlgrouptypeAll').disabled = true;

         }
         else {
             document.getElementById('rdbranchclientSelected').checked = false;
             document.getElementById('rdbranchclientAll').checked = true;
             document.getElementById('rdbranchclientAll').disabled = false;
             document.getElementById('rdddlgrouptypeAll').checked = true;
             document.getElementById('rdddlgrouptypeAll').disabled = false;
             document.getElementById('rdbranchclientSelected').checked = false;
             document.getElementById('rdddlgrouptypeSelected').checked = false;

         }
     }

     function fn_Segment(obj) {

         if (obj == "a")
             Hide('showFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Segment';
             Show('showFilter');
         }
         selecttion();
         height();
     }
     function fn_ScripsExchange(obj) {
         if (obj == "a")
             Hide('showFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'ScripsExchange';
             Show('showFilter');
         }
         selecttion();
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
     function ExcelValidate(obj) {
         if (obj != null || obj != undefined) {
             if (obj == 1) {
                 alert("Please Select Atleast One Branch!!!");
             }
             else if (obj == 2) {
                 alert("Please Select Atleast One GroupType!!!");
             }
             else if (obj == 3) {
                 alert("Please Select Atleast One Branch Group!!!");
             }
             else if (obj == 4) {
                 alert("Please Select Atleast One Client!!!");
             }
             else if (obj == 5) {
                 alert("Please Select Atleast One Product!!!");
             }
             else if (obj == 6) {
                 alert("Please Select Atleast One Segment!!!");
             }
             Reset();
             dtFrom.Focus();
             height();
         }
     }
     function NORECORD() {
         alert('No Record Found!!');
         Reset();
         dtFrom.Focus();
         height();
     }
     function Reset() {
         document.getElementById('<%=txtSelectionID_hidden.ClientID%>').setValue = '';
      document.getElementById('<%=txtTerminal.ClientID%>').setvalue = '';
      document.getElementById('<%=txtCtCLID.ClientID%>').setvalue = '';
      document.getElementById('HiddenField_Group').setvalue = '';
      document.getElementById('HiddenField_Branch').setvalue = '';
      document.getElementById('HiddenField_BranchGroup').setvalue = '';
      document.getElementById('HiddenField_Client').setvalue = '';
      document.getElementById('HiddenField_Instrument').setvalue = '';
      document.getElementById('HiddenField_SettNo').setvalue = '';
      document.getElementById('HiddenField_Setttype').setvalue = '';
      document.getElementById('HiddenField_emmail').setvalue = '';
      document.getElementById("<%=ddlGeneration.ClientID%>").options[0].selected = true;
        document.getElementById("<%=ddlGroup.ClientID%>").options[0].selected = true;
      document.getElementById('<%=rdbranchclientAll.ClientID%>').checked = true;
      document.getElementById('<%=rdbTerminalAll.ClientID%>').checked = true;
      document.getElementById('<%=rdbCTCLAll.ClientID%>').checked = true;
      document.getElementById('<%=rdInstrumentAll.ClientID%>').checked = true;
      document.getElementById('tab1').style.display = 'none';
      Hide('Tr_FilterColumn');
  }
  function Display() {
      Show('tr_filter');
      Show('displayAll');
      Show('displayAll1');
      Hide('showFilter');
      Hide('tab1');
      Hide('tab2');
      selecttion();
      height();
  }
  function selecttion() {
      var combo = document.getElementById('cmbExport');
      combo.value = 'Ex';
  }

  function Filter() {
      Hide('tr_filter');
      Hide('displayAll');
      Hide('displayAll1');
      Show('tab2');
      Show('tab1');
      Hide('showFilter');
      selecttion();
      height();
  }
  function ChangeRowColor(rowID, rowNumber) {

      var gridview = document.getElementById('grdTradeRegister');
      var rCount = gridview.rows.length;
      var rowIndex = 1;
      var rowCount = 0;

      if (rCount == 28)
          rowCount = 25;
      else
          rowCount = rCount - 2;
      if (rowNumber > 25 && rCount < 28)
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
  function MailsendT() {
      alert("Mail Sent Successfully");
  }
  function MailsendF() {
      alert("Error on sending!Try again..");
  }
  function ForDamat() {
      Hide('tab1');
      Hide('tab2');
      Hide('displayAll');
      Show('displayAll1');
      Hide('tr_filter');
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
  FieldName = 'lstSlection';
    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            var j = rValue.split('~');
            var btn = document.getElementById('btnhide');
            if (j[0] == 'Clients') {
                document.getElementById('HiddenField_Client').value = j[1];
            }
            if (j[0] == 'Branch') {
                document.getElementById('HiddenField_Branch').value = j[1];
            }
            if (j[0] == 'BranchGroup') {
                document.getElementById('HiddenField_BranchGroup').value = j[1];
            }
            if (j[0] == 'Broker') {
                document.getElementById('HiddenField_Broker').value = j[1];
            }
            if (j[0] == 'Group') {
                document.getElementById('HiddenField_Group').value = j[1];
                btn.click();
            }
            if (j[0] == 'Segment') {
                document.getElementById('HiddenField_Segment').value = j[1];
            }
            if (j[0] == 'ScripsExchange') {
                document.getElementById('HiddenField_Instrument').value = j[1];
            }
            if (j[0] == 'MAILTOEMPLOYEE') {
                document.getElementById('HiddenField_emmail').value = j[1];
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                document.getElementById('btn_show').disabled = false;
                var sessionvalue = '<%=Session["ExchangeSegmentID"]%>';
                      divscroll();
                      height();
                  }
        </script>

        <div>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                        <strong><span id="SpanHeader" style="color: #000099">Trade Register</span></strong></td>
                    <td class="EHEADER" width="25%" id="tr_filter" style="height: 22px">
                        <a href="javascript:void(0);" onclick="Filter();"><span style="color: Blue; text-decoration: underline;
                            font-size: 8pt; font-weight: bold">Filter</span></a>||
                        <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                        </asp:DropDownList>||
                    </td>
                </tr>
            </table>
            <table id="tab2">
                <tr valign="top">
                    <td class="gridcellleft">
                        <table border="1">
                            <tr valign="top">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Date :
                                </td>
                                <td class="gridcellleft">
                                    <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        Font-Size="12px" Width="108px" ClientInstanceName="dtFrom">
                                        <dropdownbutton text="From">
                                        </dropdownbutton>
                                        <clientsideevents datechanged="function(s,e){DateChangeForFrom();DateCompare(dtFrom,dtTo);}" />
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td class="gridcellleft">
                                    <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        Font-Size="12px" Width="108px" ClientInstanceName="dtTo">
                                        <dropdownbutton text="To">
                                        </dropdownbutton>
                                        <clientsideevents datechanged="function(s,e){DateChangeForTo();DateCompare(dtFrom,dtTo);}" />
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Trade Type :
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlTradeType" runat="server" Width="250px" Font-Size="12px"
                                        onchange="ddltradetypechange(this.value)">
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
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Generate Type :</td>
                                <td>
                                    <asp:DropDownList ID="ddlGeneration" runat="server" Width="130px" Font-Size="12px"
                                        onchange="fnddlGeneration(this.value)">
                                        <asp:ListItem Value="1">Select Type</asp:ListItem>
                                        <asp:ListItem Value="2">Screen</asp:ListItem>
                                        <asp:ListItem Value="3">Export</asp:ListItem>
                                        <asp:ListItem Value="4">Send Mail</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div class="clear" style="height: 0px">
            </div>
            <table id="tab1">
                <tr valign="top">
                    <td class="gridcellleft">
                        <table border="10" cellpadding="1" cellspacing="1">
                            <tr>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Group By</td>
                                <td>
                                    <asp:DropDownList ID="ddlGroup" runat="server" Width="120px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                        <asp:ListItem Value="0">Branch</asp:ListItem>
                                        <asp:ListItem Value="1">Group</asp:ListItem>
                                        <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                        <asp:ListItem Value="3">Clients</asp:ListItem>
                                        <asp:ListItem Value="4">Broker</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2" id="td_branch">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbranchclientAll" runat="server" Checked="True" GroupName="a"
                                                    onclick="fn_Branch('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbranchclientSelected" runat="server" GroupName="a" onclick="fn_Branch('b')" />Selected
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
                                                    onclick="fn_Group('a')" />
                                                All
                                                <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="fn_Group('b')" />Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Terminal Id:</td>
                                <td>
                                    <asp:RadioButton ID="rdbTerminalAll" runat="server" Checked="True" GroupName="ter"
                                        onclick="fn_Terminal('a')" />
                                    All
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdbTerminalSpecific" runat="server" GroupName="ter" onclick="fn_Terminal('b')" />Specific
                                </td>
                                <td style="display: none;" id="td_terminaltxt">
                                    <asp:TextBox runat="server" Width="100px" Font-Size="12px" ID="txtTerminal" onkeyup="fnTerminalCTCLcallajax(this,'chkfn',event,'TERMINALID')"></asp:TextBox>
                                    <asp:TextBox ID="txtTerminal_hidden" runat="server" Width="14px" Style="display: none;"> </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    CTCL Id:</td>
                                <td>
                                    <asp:RadioButton ID="rdbCTCLAll" runat="server" Checked="True" GroupName="ctcl" onclick="fn_CTCL('a')" />
                                    All
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdbCTCLSpecific" runat="server" GroupName="ctcl" onclick="fn_CTCL('b')" />Specific
                                </td>
                                <td style="display: none;" id="td_ctcltxt">
                                    <asp:TextBox runat="server" Width="100px" Font-Size="12px" ID="txtCtCLID" onkeyup="fnTerminalCTCLcallajax(this,'chkfn',event,'CTCLID')"></asp:TextBox>
                                    <asp:TextBox ID="txtCtCLID_hidden" runat="server" Width="14px" Style="display: none;"> </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Products :</td>
                                <td>
                                    <asp:RadioButton ID="rdInstrumentAll" runat="server" Checked="True" GroupName="d"
                                        onclick="fn_ScripsExchange('a')" />
                                    All
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rdInstrumentSelected" runat="server" GroupName="d" onclick="fn_ScripsExchange('b')" />
                                    Selected
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Segment :</td>
                                <td>
                                    <asp:RadioButton ID="rdbSegAll" runat="server" GroupName="e" onclick="fn_Segment('a')" />
                                    All
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdbSegSelected" runat="server" Checked="True" GroupName="e"
                                        onclick="fn_Segment('b')" />
                                    Selected
                                </td>
                                <td>
                                    [ <span id="litSegment" runat="server" style="color: Maroon"></span>]
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Order By :
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlOrderBy" runat="server" Width="200px" Font-Size="12px">
                                        <asp:ListItem Value="1">Group+Client+Instrument+TradeDate</asp:ListItem>
                                        <asp:ListItem Value="2">Group+Client+TradeDate+Instrument</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table width="100%" id="showFilter">
                            <tr valign="top">
                                <td style="text-align: right; vertical-align: top; height: 134px;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                                id="TdFilter">
                                                <span id="spanunder"></span><span id="spanclient"></span>
                                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'fn_name',event)"></asp:TextBox><asp:DropDownList
                                                    ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px" Enabled="false">
                                                    <asp:ListItem>Clients</asp:ListItem>
                                                    <asp:ListItem>Group</asp:ListItem>
                                                    <asp:ListItem>Branch</asp:ListItem>
                                                    <asp:ListItem>BranchGroup</asp:ListItem>
                                                    <asp:ListItem>Broker</asp:ListItem>
                                                    <asp:ListItem>Segment</asp:ListItem>
                                                    <asp:ListItem>ScripsExchange</asp:ListItem>
                                                    <asp:ListItem>SettlementType</asp:ListItem>
                                                    <asp:ListItem>SettlementNo</asp:ListItem>
                                                    <asp:ListItem>MAILEMPLOYEE</asp:ListItem>
                                                </asp:DropDownList>
                                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                        style="color: #009900; font-size: 8pt;"> </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="290px">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                                text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
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
                <tr id="tr_mail" valign="top">
                    <td class="gridcellleft" style="height: 41px">
                        <table border="10" cellpadding="1" cellspacing="1">
                            <tr valign="top">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Respective :</td>
                                <td>
                                    <asp:DropDownList ID="ddloptionformail" runat="server" Width="100px" Font-Size="12px"
                                        onchange="fnddloptionformail(this.value)">
                                        <asp:ListItem Value="0">Client</asp:ListItem>
                                        <asp:ListItem Value="1">Group/Branch</asp:ListItem>
                                        <asp:ListItem Value="2">User</asp:ListItem>
                                        <asp:ListItem Value="4">Broker</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="Tr_FilterColumn" valign="top">
                    <td class="gridcellleft" valign="top" colspan="2">
                        <table border="10" cellpadding="1" cellspacing="1">
                            <tr  valign="top">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Filter Columns :
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkFilterALL" runat="server" onclick="javascript:SelectAllFilter(this);"
                                                    Checked="true" /><span style="font-family: Verdana; color: Teal; font-size: x-small;"><b>Check/UnCheck
                                                        ALL</b></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="overflow: auto; border: 1px black solid; scrollbar-base-color: #C0C0C0">
                                                    <asp:CheckBoxList ID="ChkFilterDetail" runat="server" RepeatDirection="Vertical"
                                                        Width="600px" RepeatColumns="7">
                                                        <asp:ListItem Value="Terminalid" Selected="True">Terminalid</asp:ListItem>
                                                        <asp:ListItem Value="TradeCode" Selected="True">Trade Code</asp:ListItem>
                                                        <asp:ListItem Value="OrderNo" Selected="True">Order No</asp:ListItem>
                                                        <asp:ListItem Value="OrderEntryTime" Selected="True">Order EntryTime</asp:ListItem>
                                                        <asp:ListItem Value="TradeNo" Selected="True">TradeNo</asp:ListItem>
                                                        <asp:ListItem Value="TradeEntryTime" Selected="True">Trade EntryTime</asp:ListItem>
                                                        <asp:ListItem Value="CntrNo" Selected="True">Contract No</asp:ListItem>
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
                <tr id="Tr_ChkClientColumn" valign="top">
                    <td class="gridcellleft">
                        <table border="10" cellpadding="1" cellspacing="1">
                            <tr valign="top">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Print Client Name and Code in Column
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkrawprint" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td id="td_btnshow" class="gridcellleft">
                                    <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                        Width="101px" OnClick="btnshow_Click" />
                                </td>
                                <td id="td_export" class="gridcellleft" style="width: 267px">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnexport" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                    Width="120px" OnClick="btnexport_Click" /></td>
                                            <td style="width:10px;">&nbsp;</td>
                                            <td>
                                                <asp:Button ID="btnPDF" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To PDF"
                                                    Width="120px" OnClick="btnPDF_Click" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td id="td_email" class="gridcellleft">
                                    <asp:Button ID="btnmail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Email"
                                        Width="120px" OnClick="btnmail_Click" />
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
                        <asp:HiddenField ID="HiddenField_Broker" runat="server" />
                        <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                        <asp:HiddenField ID="HiddenField_Client" runat="server" />
                        <asp:HiddenField ID="HiddenField_Instrument" runat="server" />
                        <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                        <asp:HiddenField ID="HiddenField_SettNo" runat="server" />
                        <asp:HiddenField ID="HiddenField_Setttype" runat="server" />
                        <asp:HiddenField ID="HiddenField_emmail" runat="server" />
                    </td>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                    top: 50%; background-color: white; layer-background-color: white; height: 80;
                                    width: 150;'>
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
            <div id="displayAll" style="display: none;">
                <table width="100%" border="1">
                    <tr id="tr_DIVdisplayPERIOD">
                        <td>
                            <asp:UpdatePanel ID="updatepanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="DIVdisplayPERIOD" runat="server">
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table1" runat="server">
                                        <tr valign="top">
                                            <td style="height: 44px">
                                                <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Prev" Text="[Prev]" OnCommand="NavigationLinkC_Click"
                                                    OnClientClick="javascript:selecttion();"> </asp:LinkButton>
                                            </td>
                                            <td style="height: 44px">
                                                <asp:DropDownList ID="cmbclient" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True"
                                                    onchange="selecttion()" OnSelectedIndexChanged="cmbclient_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 44px">
                                                <asp:LinkButton ID="lnkNext" runat="server" CommandName="Next" Text="[Next]" OnCommand="NavigationLinkC_Click"
                                                    OnClientClick="javascript:selecttion();"> </asp:LinkButton>&nbsp;&nbsp;
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
                    <asp:HiddenField ID="TotalGrp" runat="server" />
                </table>
            </div>
            <div id="displayAll1" style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="grid_scroll">
                            <asp:GridView ID="grdTradeRegister" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                ShowFooter="True" AutoGenerateColumns="False" BorderStyle="Solid" BorderWidth="2px"
                                ForeColor="#0000C0" OnRowCreated="grdTradeRegister_RowCreated" OnRowDataBound="grdTradeRegister_RowDataBound">
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Trade Date">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Center"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTrDate" runat="server" Text='<%# Eval("TradeDate")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            Client Total
                                            <br />
                                            Grand Total
                                        </FooterTemplate>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="true"
                                            ForeColor="white" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Segment">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSegment" runat="server" Text='<%# Eval("segmentname")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Order No.">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSettNo" runat="server" Text='<%# Eval("OrderNumber")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Trade No.">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTerminalid" runat="server" Text='<%# Eval("TradeNumber")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Trade Time">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTrdCode" runat="server" Text='<%# Eval("TradeEntryTime")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Terminal Id">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTerminalid" runat="server" Text='<%# Eval("Terminalid")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Instrument">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("Symbol")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lots">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTradeNo" runat="server" Text='<%# Eval("QuantityLots")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lot Size" SortExpression="TradeEntryTime">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTradeTime" runat="server" Text='<%# Eval("LotsSize")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bought" SortExpression="Symbol">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <headerstyle horizontalalign="Center" font-bold="False"></headerstyle>
                                            <asp:Label ID="lblBought" runat="server" Text='<%# Eval("Bought")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblBought_sum" runat="server" ForeColor="white"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sold">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSold" runat="server" Text='<%# Eval("Sold")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblSold_sum" runat="server" ForeColor="white"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Price">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSold" runat="server" Text='<%# Eval("MKTPRICE")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quote Unit">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Eval("Priceper")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTradeType" runat="server" Text='<%# Eval("brkgtype")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Brkg">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblBrkg" runat="server" Text='<%# Eval("UnitBrokerage")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Brkg">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lbltotalbrkg" runat="server" Text='<%# Eval("TotalBrokerage")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lbltotalbrkg_sum" runat="server" ForeColor="white"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Net Rate">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetRate" runat="server" Text='<%# Eval("NetRatePerUnit")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Net Value">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetAmount" runat="server" Text='<%# Eval("NetValue")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNetValue_sum" runat="server" ForeColor="white"></asp:Label>
                                            <br />
                                        </FooterTemplate>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Srv Tax">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetRate" runat="server" Text='<%# Eval("servicetax")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblSrvTax_sum" runat="server" ForeColor="white"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetRate" runat="server" Text='<%# Eval("ServiceTaxMode")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Net Amnt">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetRate" runat="server" Text='<%# Eval("netamount")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNetAmountsum" runat="server" ForeColor="white"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Net">
                                        <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity")%>' CssClass="gridstyleheight1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerTemplate>
                                </PagerTemplate>
                                <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                    BorderWidth="1px"></RowStyle>
                                <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                    Font-Bold="False"></HeaderStyle>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="cmbclient" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lnkPrev" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lnkNext" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
</asp:Content>
