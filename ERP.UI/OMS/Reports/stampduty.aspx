<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_stampduty" CodeBehind="stampduty.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>

    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

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
        groupvalue = "";
        function Page_Load()///Call Into Page Load
        {
            //             var format=document.getElementById("ddlformat").value;
            //                var format2=document.getElementById("ddlRptViewPayableToAutority").value;
            //                if (format=="2")
            //                {
            //                document.getElementById("format2").disabled=true;
            //                }

            Hide('showFilter');
            Show('tab2');
            Hide('tr_filter');
            document.getElementById('hiddencount').value = 0;
            Hide('displayAll');
            Hide('tr_Clients');
            Hide('tr_Group');
            Hide('td_state');
            Show('tr_subtype');
            Hide('Tr_RptViewChargeToClient');
            Show('Tr_RptViewPayableToAutority');
            Hide('Tr_FilterColumn');
            Hide('Tr_Company');
            Hide('Tr_Segment');
            Hide('Tr_ConsiderDate');
            Hide('Td_ConsolidatedAcrossSegment');
            fnddlGeneration('1');
            height();

        }

        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function height() {
            if (document.body.scrollHeight >= 450) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '450px';
            }
            window.frameElement.width = document.body.scrollwidth;
        }

        ////////////////

        function DateChangeForFrom() {
            //var datePost=(dtFrom.GetDate().getMonth()+2)+'-'+dtFrom.GetDate().getDate()+'-'+dtFrom.GetDate().getYear();
            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtFrom.GetDate().getMonth() + 1;
            var DayDate = dtFrom.GetDate().getDate();
            var YearDate = dtFrom.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();

            if (YearDate >= objsession[0]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                dtFrom.SetDate(new Date(datePost));
            }
        }
        function DateChangeForTo() {
            var sessionValFrom = "<%=Session["FinYearStart"]%>";
       var sessionValTo = "<%=Session["FinYearEnd"]%>";
       var sessionVal = "<%=Session["LastFinYear"]%>";
       var objsession = sessionVal.split('-');
       var MonthDate = dtToDate.GetDate().getMonth() + 1;
       var DayDate = dtToDate.GetDate().getDate();
       var YearDate = dtToDate.GetDate().getYear();
       var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
       var Sto = new Date(sessionValTo).getMonth() + 1;
       var SFrom = new Date(sessionValFrom).getMonth() + 1;
       var SDto = new Date(sessionValTo).getDate();
       var SDFrom = new Date(sessionValFrom).getDate();

       if (YearDate <= objsession[1]) {
           if (MonthDate < SFrom && YearDate == objsession[0]) {
               alert('Enter Date Is Outside Of Financial Year !!');
               var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
               dtToDate.SetDate(new Date(datePost));
           }
           else if (MonthDate > Sto && YearDate == objsession[1]) {
               alert('Enter Date Is Outside Of Financial Year !!');
               var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
               dtToDate.SetDate(new Date(datePost));
           }
           else if (YearDate != objsession[0] && YearDate != objsession[1]) {
               alert('Enter Date Is Outside Of Financial Year !!');
               var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
               dtToDate.SetDate(new Date(datePost));
           }
       }
       else {
           alert('Enter Date Is Outside Of Financial Year !!');
           var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
           dtToDate.SetDate(new Date(datePost));
       }
   }







   /////////////////////////////
   function fnClients(obj) {
       if (obj == "a")
           Hide('showFilter');
       else {
           var cmb = document.getElementById('cmbsearchOption');
           cmb.value = 'Clients';
           Show('showFilter');
       }

   }
   function fnBranch(obj) {
       if (obj == "a")
           Hide('showFilter');
       else {
           var cmb = document.getElementById('cmbsearchOption');
           cmb.value = 'Branch';
           Show('showFilter');
       }

   }

   function fnstate(obj) {
       if (obj == "a")
           Hide('showFilter');
       else {
           var cmb = document.getElementById('cmbsearchOption');
           cmb.value = 'state';
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
   function fnCompany(obj) {
       if (obj == "a")
           Hide('showFilter');
       else {
           Show('showFilter');
           document.getElementById('cmbsearchOption').value = 'Company';
       }

   }
   function fnSegment(obj) {
       if (obj == "a")
           Hide('showFilter');
       else if (obj == "c") {
           Hide('showFilter');
           Show('Td_Specific');
       }
       else {
           var cmb = document.getElementById('cmbsearchOption');
           cmb.value = 'Segment';
           Show('showFilter');
       }

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
       else if (document.getElementById('cmbsearchOption').value == "Company") {
           ajax_showOptions(objID, "Company", objEvent);
       }
       else if (document.getElementById('cmbsearchOption').value == "state") {
           ajax_showOptions(objID, "state", objEvent);
       }
       else {
           cmbVal = document.getElementById('cmbsearchOption').value;
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
   function fn_client(obj) {
       if (obj == "1") {
           Hide('td_2');
           Show('td_1');
           Show('tr_Pdf');
       }
       if (obj == "2") {
           Show('td_2');
           Hide('td_1');
           Hide('tr_Pdf');
       }
   }

   function fnddlGroup(obj) {
       if (obj == "0") {
           //  alert ('1');
           Hide('td_group');
           Show('td_branch');
           Hide('td_state');
       }
       if (obj == "2") {
           //alert ('2');
           Hide('td_group');
           Show('td_state');
           Hide('td_branch');
       }
       if (obj == "1") {
           //alert ('3');
           Show('td_group');
           Hide('td_branch');
           Hide('td_state');
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
   function NORECORD() {
       Show('tab2');
       Hide('tr_filter');
       Hide('showFilter');
       Hide('displayAll');
       alert('No Record Found');
       height();
   }
   function DISPLAY(obj) {
       Hide('tab2');
       Show('tr_filter');
       Hide('showFilter');
       Show('displayAll');
       if (obj == "1")
           Show('tr_prvnxt');
       else
           Hide('tr_prvnxt');
       height();
   }
   function Filter() {
       Show('tab2');
       selecttion();
       Hide('tr_filter');
       Hide('showFilter');
       Hide('displayAll');
       height();
   }
   function selecttion() {
       var combo = document.getElementById('ddlExport');
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
   function fn_report(obj) {
       if (obj == '0') {
           Hide('tr_Clients');
           Hide('tr_Group');
           Show('tr_subtype');
           Hide('Tr_RptViewChargeToClient');
           Show('Tr_RptViewPayableToAutority');
           Hide('Tr_FilterColumn');
           Hide('Tr_Company');
           Hide('Tr_Segment');
           Hide('Td_ConsolidatedAcrossSegment');
           Hide('Tr_ConsiderDate');
       }
       else {
           Show('tr_Clients');
           Show('tr_Group');
           Hide('tr_subtype');
           Show('Tr_RptViewChargeToClient');
           Hide('Tr_RptViewPayableToAutority');
           Show('Tr_FilterColumn');
           Show('Tr_Company');
           Show('Tr_Segment');
           Hide('Td_ConsolidatedAcrossSegment');
           Show('Tr_ConsiderDate');

       }

       document.getElementById('ddlGeneration').value = '1';
       fnddlGeneration('1')
   }
   function fnddlGeneration(obj) {
       if (document.getElementById('ddlReporttype').value == '0') {
           document.getElementById('ddlGeneration').value = '2';
           Hide('tr_Screen');
           Show('tr_Export');
           Show('tr_Pdf');
           Hide('td_2');
       }
       else {
           if (obj == '1') {
               Show('tr_Screen');
               Hide('tr_Export');
               Hide('tr_Pdf');
           }
           else {
               Hide('tr_Screen');
               Show('tr_Export');
               Hide('tr_Pdf');
           }
       }
       height();
   }
   function FnRptViewChargeToClient(obj) {
       var stateid = document.getElementById('<%= ddlGroup.ClientID%>');
        //alert (stateid);
        if (obj == "3" || obj == "1")
            Hide('Td_ConsolidatedAcrossSegment');
        else
            Show('Td_ConsolidatedAcrossSegment');

        if (obj == "1") {

            //stateid.options[2].style.display='inline';
            //               stateid.options.remove(2);
            var myOptions = {
                val1: 'State',
                val2: '2'
            };
            $.each(myOptions, function (val, text) {
                $('#ddlGroup').append(
                 $('<option></option>').val(val).html(text)
                   );
            });
            //stateid.options.add(newOption);
            Show('Tr_FilterColumn');
        }
        else
            Hide('Tr_FilterColumn');
        //  stateid.options[2].style.display='none';
        stateid.options.remove(2);

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
            if (j[0] == 'Company') {
                document.getElementById('HiddenField_Company').value = j[1];
            }
            if (j[0] == 'Segment') {
                document.getElementById('HiddenField_Segment').value = j[1];
            }
            if (j[0] == 'state') {
                document.getElementById('HiddenField_state').value = j[1];
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
                <td class="EHEADER" colspan="0" style="text-align: center; height: 22px;">
                    <strong><span id="SpanHeader" style="color: #000099">Stamp Duty Statement</span></strong>
                </td>
                <td class="EHEADER" width="25%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);"
                        onclick="Filter();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                        <asp:ListItem Value="P">PDF</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="tab2">
            <tr>
                <td align="left">
                    <table>
                        <tr valign="top">
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Report Type :</td>
                                                    <td class="gridcellleft" colspan="5">
                                                        <asp:DropDownList ID="ddlReporttype" runat="server" Width="150px" Font-Size="12px"
                                                            onchange="fn_report(this.value);">
                                                            <asp:ListItem Value="0">Payabel To Authority</asp:ListItem>
                                                            <asp:ListItem Value="1">Charge To Client</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr id="tr_subtype">
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Report Format :</td>
                                                    <td class="gridcellleft" colspan="5">
                                                        <asp:DropDownList ID="ddlFormat" runat="server" Width="200px" Font-Size="12px"
                                                            onchange="fn_client(this.value);">
                                                            <asp:ListItem Value="1">Maharashtra State Format</asp:ListItem>
                                                            <asp:ListItem Value="2">State Wise</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="Tr_RptViewPayableToAutority">
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr id="tr_rptview">
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Report View :</td>
                                                    <td id="td_1" runat="server" class="gridcellleft">
                                                        <asp:DropDownList ID="ddlRptViewPayableToAutority" runat="server" Width="150px" Font-Size="12px">
                                                            <asp:ListItem Value="1">Detail</asp:ListItem>
                                                            <asp:ListItem Value="2">Summary</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="td_2" runat="server" class="gridcellleft">
                                                        <asp:DropDownList ID="ddlRptViewPayableToAutority1" runat="server" Width="150px" Font-Size="12px">
                                                            <asp:ListItem Value="1">Date Wise</asp:ListItem>
                                                            <asp:ListItem Value="2">Summary</asp:ListItem>
                                                            <asp:ListItem Value="3">State + Client</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="Tr_RptViewChargeToClient">
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Report View :</td>
                                                    <td class="gridcellleft">
                                                        <asp:DropDownList ID="ddlRptViewChargeToClient" runat="server" Width="250px" Font-Size="12px" onchange="FnRptViewChargeToClient(this.value);">
                                                            <asp:ListItem Value="1">TO With Stampduty</asp:ListItem>
                                                            <asp:ListItem Value="2">Month Wise -Across Segment</asp:ListItem>
                                                            <asp:ListItem Value="3">Month Wise -Across Segment +Branch/Group</asp:ListItem>
                                                            <asp:ListItem Value="4">Month Wise -Across Segment +Client</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr valign="top" id="Tr_ConsiderDate">
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Consider :
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbConsiderDate" runat="server" Width="100px" Font-Size="12px">
                                                            <asp:ListItem Value="Trade Date">Trade Date</asp:ListItem>
                                                            <asp:ListItem Value="Payout Date">Payout Date</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Period:</td>
                                                    <td class="gridcellleft">
                                                        <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                            Font-Size="12px" Width="108px" ClientInstanceName="dtFrom">
                                                            <DropDownButton Text="From">
                                                            </DropDownButton>
                                                            <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                            Font-Size="12px" Width="108px" ClientInstanceName="dtToDate">
                                                            <DropDownButton Text="To">
                                                            </DropDownButton>
                                                            <ClientSideEvents DateChanged="function(s,e){DateChangeForTo();}" />
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr id="tr_Group">
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Group By</td>
                                                    <td class="gridcellleft">
                                                        <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                            <asp:ListItem Value="0">Branch</asp:ListItem>
                                                            <asp:ListItem Value="1">Group</asp:ListItem>
                                                            <asp:ListItem Value="2">State</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="td_branch">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="c" onclick="fnBranch('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="c" onclick="fnBranch('b')" />Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td id="td_state">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbstateall" runat="server" Checked="True" GroupName="z" onclick="fnstate('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbstateselected" runat="server" GroupName="z" onclick="fnstate('b')" />Selected
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
                                                                            <asp:DropDownList ID="ddlgrouptype" runat="server" Font-Size="12px" onchange="fngrouptype(this.value)">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td id="td_allselect" style="display: none;">
                                                                    <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="e"
                                                                        onclick="fnGroup('a')" />
                                                                    All
                                                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="e" onclick="fnGroup('b')" />Selected
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
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr id="tr_Clients">
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Clients :</td>
                                                    <td class="gridcellleft">
                                                        <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="df" onclick="fnClients('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="df" onclick="fnClients('b')" />
                                                        Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="Tr_Company">
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Company :</td>
                                                    <td>
                                                        <asp:RadioButton ID="RdbAllCompany" runat="server" GroupName="dd" onclick="fnCompany('a')" />
                                                        All
                                                            <asp:RadioButton ID="RdbCurrentCompany" runat="server" Checked="True" GroupName="dd"
                                                                onclick="fnCompany('a')" />
                                                        Current
                                                            <asp:RadioButton ID="RdbSelectedCompany" runat="server" GroupName="dd" onclick="fnCompany('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="Tr_Segment">
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Segment:</td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbSegmentAll" runat="server" GroupName="d" onclick="fnSegment('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbSegmentSpecific" runat="server" Checked="True" GroupName="d"
                                                            onclick="fnSegment('c')" />Current
                                                    </td>
                                                    <td id="Td_Specific">[ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdSegmentSelected" runat="server" GroupName="d" onclick="fnSegment('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>

                                                    <td class="gridcellleft" bgcolor="#B7CEEC" id="Td_ConsolidatedAcrossSegment">
                                                        <asp:CheckBox ID="ChkCOnsolidatedAcrossSegment" runat="server" />
                                                        Show Group/Branch BreakUp</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="Tr_FilterColumn">
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Filter Columns :
                                                    </td>
                                                    <td>
                                                        <div style="overflow: auto; border: 1px black solid; scrollbar-base-color: #C0C0C0">
                                                            <asp:CheckBoxList ID="chktfilter" runat="server" RepeatDirection="Horizontal" Width="400px"
                                                                RepeatColumns="4">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type :</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlGeneration" runat="server" Width="130px" Font-Size="12px"
                                                            onchange="fnddlGeneration(this.value)">
                                                            <asp:ListItem Value="1">Screen</asp:ListItem>
                                                            <asp:ListItem Value="2">Export</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td id="tr_Screen">
                                                        <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Screen"
                                                            Width="101px" OnClientClick="selecttion()" OnClick="btnshow_Click" />
                                                    </td>
                                                    <td id="tr_Export">
                                                        <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Open To Excel"
                                                            Width="101px" OnClientClick="selecttion()" OnClick="btnExcel_Click" />
                                                    </td>
                                                    <td id="tr_Pdf">
                                                        <asp:Button ID="btnPDF" runat="server" CssClass="btnUpdate" Height="20px" Text="Open To PDF"
                                                            Width="101px" OnClientClick="selecttion()" OnClick="btnPDF_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table width="100%" id="showFilter">
                                    <tr>
                                        <td style="text-align: right; vertical-align: top; height: 134px;">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                                        id="TdFilter">
                                                        <span id="spanunder"></span><span id="spanclient">
                                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox><asp:DropDownList
                                                                ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px" Enabled="false">
                                                                <asp:ListItem>Clients</asp:ListItem>
                                                                <asp:ListItem>Branch</asp:ListItem>
                                                                <asp:ListItem>Group</asp:ListItem>
                                                                <asp:ListItem>Segment</asp:ListItem>
                                                                <asp:ListItem>state</asp:ListItem>
                                                                <asp:ListItem>Company</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                                    style="color: #009900; font-size: 8pt;"> </span></td>
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
        <table id="tab3">
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="hiddencount" runat="server" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Company" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:HiddenField ID="HiddenField_state" runat="server" />
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
    </div>
    <div id="displayAll">
        <asp:UpdatePanel runat="server" ID="u1">
            <ContentTemplate>
                <table width="100%" border="1">
                    <tr style="display: none;">
                        <td>
                            <asp:DropDownList ID="cmbGROUP" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <div id="displayPERIOD" runat="server">
                            </div>
                        </td>
                    </tr>

                    <tr bordercolor="Blue" id="tr_prvnxt">
                        <td align="center">
                            <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="100%">
                                <tr>
                                    <td width="20" style="padding: 5px">
                                        <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon" OnClientClick="javascript:selecttion();showProgress();" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                    <td width="25">
                                        <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();showProgress();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                    <td width="20" style="padding: 5px">
                                        <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon" OnClientClick="javascript:selecttion();showProgress();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                    <td width="20">
                                        <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();showProgress();" OnClick="ASPxLast_Click">Last</asp:LinkButton></td>
                                    <td align="right">
                                        <asp:Label ID="listRecord" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <div id="display" runat="server">
                            </div>
                        </td>
                    </tr>

                    <asp:HiddenField ID="TotalNO" runat="server" />

                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

