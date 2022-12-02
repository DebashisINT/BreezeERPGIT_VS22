<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_portfolioperformancecm" CodeBehind="portfolioperformancecm.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>
    
    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>
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
            height: 300px;
            width: 70%;
            scrollbar-base-color: #C0C0C0;
        }

        .tableClass {
            /* border: 0px; */
            border: 1px solid #aaa !important;
            border-collapse: collapse !important;
        }

        .tableBorderClass {
            /* border: 0px; */
            border: 1px solid #aaa !important;
        }
    </style>
    <script language="javascript" type="text/javascript">

             function Page_Load()///Call Into Page Load
             {

                 querySt()
                 Hide('td_dtfrom');
                 Hide('td_dtto');
                 Hide('showFilter');
                 Show('td_btnshow');
                 Hide('td_btnprint');
                 Hide('tr_filter');
                 Hide('Td_Excel');
                 //Hide('Td_RadioBtnExcel');
                 document.getElementById('hiddencount').value = 0;
                 FnValuationMathod(document.getElementById('ddlclosmethod').value);
                 if (document.getElementById("rbPrint").checked) {
                     Hide('td_btnshow');
                     Show('td_btnPrint');
                     Hide('td_Excel');
                 }
                 if (document.getElementById("rbScreen").checked) {
                     Show('td_btnshow');
                     Hide('td_btnPrint');
                     Hide('td_Excel');
                 }
                 if (document.getElementById("rbExcel").checked) {
                     Hide('td_btnshow');
                     Hide('td_btnPrint');
                     Show('td_Excel');
                 }
                 dtfor.Focus();
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
             function querySt() {
                 if (window.location.search.substring(1) == 'type=01') {
                     Hide('tr_consolidated');
                     Hide('tr_grp');
                     Hide('tr_client');
                     document.getElementById("SpanHeader").innerHTML = "Portfolio Report (Trading A/c)";
                 }
                 else {
                     Hide('tr_consolidated');
                     document.getElementById("SpanHeader").innerHTML = "Portfolio Report (Client)";
                 }
             }
             function fn_Consolidated(obj) {

                 if (obj.checked == true) {
                     Hide('tr_grp');
                     Hide('tr_client');
                 }
                 else {
                     Show('tr_grp');
                     Show('tr_client');
                 }
                 selecttion();
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

                 }
                 else {
                     //                Hide('td_dtfor');
                     //                Show('td_dtfrom');
                     //                Show('td_dtto');
                     document.getElementById('ddldate').value = '0';
                 }
                 selecttion();
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

                 ajax_showOptions(objID, objListFun, objEvent, cmbVal);

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
                 height();
             }
             function Company(obj) {
                 if (obj == "A") {
                     Hide('showFilter');
                     document.getElementById("Span_SpecificComp").innerText = "None";
                 }
                 else {
                     var cmb = document.getElementById('cmbsearchOption');
                     cmb.value = 'Company';
                     Show('showFilter');
                 }
                 selecttion();
                 height();
             }
             function Branch(obj) {
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
                 height();
             }
             function Group(obj) {
                 if (obj == "a")
                     Hide('showFilter');
                 else {
                     var cmb = document.getElementById('cmbsearchOption');
                     cmb.value = 'Group';
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
                 selecttion();
                 height();
             }

             function RBShowHide(obj) {
                 if (obj == 'rbPrint') {
                     Hide('td_btnshow');
                     Show('td_btnprint');
                     Show('td_ChkDISTRIBUTION');
                     Hide('Td_Excel');
                 }
                 else if (obj == 'rbScreen') {
                     Show('td_btnshow');
                     Hide('td_btnprint');
                     Hide('td_ChkDISTRIBUTION');
                     Hide('Td_Excel');
                 }
                 else {
                     Hide('td_btnshow');
                     Hide('td_btnprint');
                     Hide('td_ChkDISTRIBUTION');
                     Show('Td_Excel');
                 }
                 height();
                 selecttion();
             }

             function fnunderlying(obj) {
                 if (obj == "a")
                     Hide('showFilter');
                 else {
                     var cmb = document.getElementById('cmbsearchOption');
                     cmb.value = 'Product';
                     Show('showFilter');
                 }
                 selecttion();
                 height();
             }

             function fnExpiry(obj) {
                 if (obj == "a")
                     Hide('showFilter');
                 else {
                     var cmb = document.getElementById('cmbsearchOption');
                     cmb.value = 'Expiry';
                     Show('showFilter');
                 }
                 selecttion();
                 height();

             }

             function NORECORD(obj) {
                 Hide('tr_filter');
                 Hide('displayAll');
                 Show('tab2');
                 Hide('showFilter');
                 if (obj == '1') {
                     alert('No Record Found!!');
                     var querystring = window.location.search.substring(1);
                     querystring = querystring.split('=')[1];
                     window.location = "../reports/portfolioperformancecm.aspx?type=" + querystring;
                 }

                 document.getElementById('hiddencount').value = 0;
                 height();

             }

             function Display() {
                 Show('tr_filter');
                 Show('displayAll');
                 Hide('tab2');
                 Hide('showFilter');
                 document.getElementById('hiddencount').value = 0;
                 if (document.getElementById('ddlrpttype').value == '0') {
                     document.getElementById('display').className = "grid_scroll";
                 }
                 height();
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
             function Filter() {
                 Hide('tr_filter');
                 Hide('displayAll');
                 Show('tab2');
                 Hide('showFilter');
                 FnRpttype(document.getElementById('ddlrpttype').value);
                 height();
                 selecttion();
             }

             function OnMoreInfoClick(CUSTOMERID, clienttype, Segment, MASTERPRODUCTID) {

                 var grp;
                 if (document.getElementById('ddlGroup').value == "0")///////////group
                 {
                     grp = 'Branch';
                 }
                 else {
                     grp = document.getElementById('ddlgrouptype').value;
                 }

                 var header = 'Profolio Performance Report';



                 var date;
                 var date1;
                 if (document.getElementById('ddldate').value == "0")///////////date check
                 {
                     date1 = dtfor.GetDate();
                     date = parseInt(date1.getMonth() + 1) + '-' + date1.getDate() + '-' + date1.getFullYear() + '~1';
                 }
                 else {
                     date1 = dtfrom.GetDate();
                     date = parseInt(date1.getMonth() + 1) + '-' + date1.getDate() + '-' + date1.getFullYear();
                     date1 = dtto.GetDate();
                     date = date + '~' + parseInt(date1.getMonth() + 1) + '-' + date1.getDate() + '-' + date1.getFullYear();
                 }


                 var URL = 'portfolioperformancePOPUPcm.aspx?CUSTOMERID=' + CUSTOMERID + ' &Segment=' + Segment + '&MASTERPRODUCTID=' + MASTERPRODUCTID + '&Date=' + date + ' &grp=' + grp + ' &closemethod=' + document.getElementById('ddlclosmethod').value + ' &consolidatedchk=' + document.getElementById('chkconsolidated').checked + ' &sqroffchk=' + document.getElementById('chkexcludesqr').checked + ' &clienttype=' + clienttype;

                 OnMoreInfoClick(URL, header, '990px', '450px', 'N');

             }
             function FnRpttype(obj) {
                 if (obj == "0" || obj == "1")///Only Summary And Detail
                 {
                     if (obj == "1")
                         Hide('Td_ExcludeSTT');
                     else
                         Show('Td_ExcludeSTT');

                     Show('Td_ExcludeSqrOff');
                     Show('Td_RadioBtnScreen');
                     Show('Td_RadioBtnPrint');
                     //            Hide('Td_Excel');

                     //Show Button on Appropriate Radio Selection
                     if (document.getElementById("rbPrint").checked) {
                         Hide('td_btnshow');
                         Show('td_btnPrint');
                         Hide('td_Excel');
                     }
                     if (document.getElementById("rbScreen").checked) {
                         Show('td_btnshow');
                         Hide('td_btnPrint');
                         Hide('td_Excel');
                     }
                     if (document.getElementById("rbExcel").checked) {
                         Hide('td_btnshow');
                         Hide('td_btnPrint');
                         Show('td_Excel');
                     }

                     ///Company Option Only For Report Type(3)
                     Hide("Tr_Company");
                     Hide("Tr_CompanyFilter");
                 }
                 else ///Only Closing Stock [Summary And Detail]
                 {
                     Hide('Td_ExcludeSTT');
                     Hide('Td_ExcludeSqrOff');

                     if (obj == "3") ///Only Closing Stock [Summary]
                     {
                         Hide('Td_RadioBtnPrint');
                         Hide('td_btnprint');
                         Hide('Td_Excel');
                         Show('Td_RadioBtnExcel');
                         document.getElementById("rbScreen").checked = true;
                         if (document.getElementById("rbPrint").checked)
                             Show('td_ChkDISTRIBUTION');
                         else
                             Hide('td_ChkDISTRIBUTION');

                         ///Company Option Only For Report Type(3)

                         if (window.location.search.substring(1) == 'type=01')// Only For Trading Portfolio
                         {
                             document.getElementById("rdbOnlyQty").checked = true;
                             document.getElementById("rdbCompanySelected").checked = true;
                             Show("Tr_Company");
                             Show("Tr_CompanyFilter");
                             var SenddataCompany = "Company~" + '<%=Session["LastCompany"]%>' + ";";
                     CallServer(SenddataCompany, "");//forcily called on initialization
                 }
                 else {
                     Hide("Tr_Company");
                     Hide("Tr_CompanyFilter");
                 }
             }
             else ///Only Closing Stock [Summary]
             {
                 Show('Td_RadioBtnPrint');
                 Show('td_btnprint');

                 ///Company Option Only For Report Type(3)
                 Hide("Tr_Company");
                 Hide("Tr_CompanyFilter");
             }

             //Show Button on Appropriate Radio Selection
             if (document.getElementById("rbPrint").checked) {
                 Hide('td_btnshow');
                 Show('td_btnPrint');
                 Hide('td_Excel');
             }
             if (document.getElementById("rbScreen").checked) {
                 Show('td_btnshow');
                 Hide('td_btnPrint');
                 Hide('td_Excel');
             }
             if (document.getElementById("rbExcel").checked) {
                 Hide('td_btnshow');
                 Hide('td_btnPrint');
                 Show('td_Excel');
             }
         }
         FnValuationMathod(document.getElementById('ddlclosmethod').value);
         height();
     }
     function FnValuationMathod(obj) {
         if (obj == "0")
             Hide('Tr_ValuationDate');
         else
             Show('Tr_ValuationDate');
         height();
     }
     FieldName = 'lstSlection';
     </script>
    <script type="text/ecmascript">
             function ReceiveServerData(rValue) {

                 var j = rValue.split('~');
                 var btn = document.getElementById('btnhide');


                 if (j[0] == 'Branch') {
                     document.getElementById('HiddenField_Branch').value = j[1];
                 }
                 if (j[0] == 'Group') {
                     document.getElementById('HiddenField_Group').value = j[1];
                     groupvalue = j[1];
                     btn.click();
                 }
                 if (j[0] == 'Clients') {
                     document.getElementById('HiddenField_Client').value = j[1];
                 }
                 if (j[0] == 'Product') {
                     document.getElementById('HiddenField_Product').value = j[1];
                 }
                 if (j[0] == 'BranchGroup') {
                     document.getElementById('HiddenField_BranchGroup').value = j[1];
                 }
                 if (j[0] == 'Company') {
                     document.getElementById('HiddenField_Company').value = j[1];
                     document.getElementById("Span_SpecificComp").innerText = j[2];
                 }

             }
             function dtforChange() {
                 dtValuationDate.SetDate(dtfor.GetDate());
             }

             ///Use this in GenericJS
             //For FinYear Check
             //        DateValidation(dtfor,dtValuationDate,'Y','N','N','N','Y','N','N','N','N',0,'N',0,'','');
             //        DateValidation(dtfrom,dtto,'Y','N','N','N','Y','N','N','N','N',0,'N',0,'To Date Can Not Be Lesser Than From Date','')

             function DateValidation(DateObjS, DateObjT,
             CheckForFinYearS, CheckForFinYearT,
             CheckForLockDateS, CheckForLockDateT,
             CheckForNotGreaterThanS, CheckForNotGreaterThanT,
             AddDayByComparingS, AddDayByComparingT,
             IsNoOfDayToAddS, NoOfDayToAddS, IsNoOfDayToAddT, NoOfDayToAddT,
             MsgCheckForNotGreaterThanS, MsgCheckForNotGreaterThanT) {

                 var Lck = '<%=Session["LCKBNK"] %>';
          var FYS = '<%=Session["FinYearStart"]%>';
          var FYE = '<%=Session["FinYearEnd"]%>';
          var LFY = '<%=Session["LastFinYear"]%>';

          ////////////CheckForFinYearS
          if (CheckForFinYearS == 'Y')
              DevE_CheckForFinYear(DateObjS, FYS, FYE, LFY);

          ///////////CheckForFinYearT
          if (CheckForFinYearT == 'Y')
              DevE_CheckForFinYear(DateObjT, FYS, FYE, LFY);
          //            
          //            ///////////CheckForNotGreaterThanS
          //            
          /////////////////////Add No Of Day After Check/////////////////////
          if (CheckForNotGreaterThanS == 'Y' && IsNoOfDayToAddS == 'Y')
              DevE_CompareDateForMin_AddDay(DateObjS, DateObjT, NoOfDayToAddS);
          /////////////////////Do Not Add No Of Day After Check/////////////////////
          if (CheckForNotGreaterThanS == 'Y' && IsNoOfDayToAddS == 'N')
              DevE_CompareDateForMin(DateObjS, DateObjT);
          //                
          //            
          //             ///////////CheckForNotGreaterThanT
          //            
          //            /////////////////////Add No Of Day After Check/////////////////////
          //            if(CheckForNotGreaterThanT=='Y' && IsNoOfDayToAddT=='Y')
          //                DevE_CompareDateForMin_AddDay(DateObjT,DateObjS,NoOfDayToAddT);
          //            /////////////////////Do Not Add No Of Day After Check/////////////////////
          //            if(CheckForNotGreaterThanT=='Y' && IsNoOfDayToAddT=='N')
          //                DevE_CompareDateForMin(DateObjT,DateObjS);
          //                
          //            ////////////////////////CheckForLockDate////////////////////////////
          //            if(CheckForLockDateS=='Y')
          //                DevE_CheckForLockDate(DateObjS,Lck);
          //            if(CheckForLockDateT=='Y')
          //                DevE_CheckForLockDate(DateObjT,Lck);
          //            
          /////////////////////////AddDayByComparing///////////////////////////
          if (AddDayByComparingS == 'Y' && IsNoOfDayToAddS == 'Y')
              DevE_CompareDateAndAddDay(DateObjS, DateObjT, NoOfDayToAddS);
          else
              DevE_CompareDateAndAddDay(DateObjS, DateObjT, 0);

          if (AddDayByComparingT == 'Y' && IsNoOfDayToAddT == 'Y')
              DevE_CompareDateAndAddDay(DateObjT, DateObjS, NoOfDayToAddT);
          else
              DevE_CompareDateAndAddDay(DateObjT, DateObjS, 0);
          //            

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
                    <strong><span id="SpanHeader" style="color: #000099">Portfolio Report</span></strong></td>
                <td class="EHEADER" width="15%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="Filter();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <%--                  <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>--%>
                </td>
            </tr>
        </table>
        <table id="tab2" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table class="tableBorderClass" style="margin-left: 2px">
                        <tr valign="top">
                            <td class="gridcellleft">
                                <table>
                                    <tr>
                                        <td id="td_dtfor" class="gridcellleft">
                                            <asp:DropDownList ID="ddldate" runat="server" Font-Size="12px">
                                                <asp:ListItem Value="0">Up To Date (Trade Basis)</asp:ListItem>
                                                <asp:ListItem Value="1">Up To Date (Bill Basis)</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True" Font-Size="12px" Width="108px" ClientInstanceName="dtfor">
                                                <DropDownButton Text="For">
                                                </DropDownButton>
                                                <ClientSideEvents DateChanged="function(s,e){
                                        DateValidation(dtfor,dtValuationDate,'Y','N','N','N','N','N','Y','N','N',0,'N',0,'N','N');}"></ClientSideEvents>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td id="td_dtfrom" class="gridcellleft">
                                <dxe:ASPxDateEdit ID="dtfrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfrom">
                                    <DropDownButton Text="From">
                                    </DropDownButton>
                                    <ClientSideEvents DateChanged="function(s,e){
                                       DateValidation(dtfrom,dtto,'Y','N','N','N','Y','N','N','N','N',0,'N',0,'To Date Can Not Be Lesser Than From Date','N');}"></ClientSideEvents>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td id="td_dtto" class="gridcellleft">
                                <dxe:ASPxDateEdit ID="dtto" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtto">
                                    <DropDownButton Text="To">
                                    </DropDownButton>
                                    <ClientSideEvents DateChanged="function(s,e){
                                         DateValidation(dtfrom,dtto,'N','Y','N','N','Y','N','N','N','N',0,'N',0,'To Date Can Not Be Lesser Than From Date','N');}"></ClientSideEvents>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td colspan="5" class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report View :</td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlrptview" runat="server" Font-Size="11px" Width="200px" Enabled="true">
                                                <asp:ListItem Value="0">Branch/Group - Client wise</asp:ListItem>
                                                <asp:ListItem Value="1">Branch/Group - Asset wise</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report Type :</td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlrpttype" runat="server" Font-Size="11px" Width="200px" Enabled="true"
                                                onchange="FnRpttype(this.value)">
                                                <asp:ListItem Value="0">Summary</asp:ListItem>
                                                <asp:ListItem Value="1">Detail</asp:ListItem>
                                                <asp:ListItem Value="3">Closing Stock [Summary]</asp:ListItem>
                                                <asp:ListItem Value="4">Closing Stock [Detail]</asp:ListItem>
                                                <asp:ListItem Value="5">Long-Term Gain/Loss</asp:ListItem>
                                                <asp:ListItem Value="6">Short-Term Gain/Loss</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC" colspan="2">Closing Stock Valuation Method :</td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlclosmethod" runat="server" Font-Size="11px" Width="120px"
                                                onchange="FnValuationMathod(this.value)">
                                                <asp:ListItem Value="0">At Cost</asp:ListItem>
                                                <asp:ListItem Value="1">At Market</asp:ListItem>
                                                <asp:ListItem Value="2">Cost/Market Lower</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Tr_ValuationDate">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Valuation Date :</td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtValuationDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="dtValuationDate">
                                                <DropDownButton Text="For">
                                                </DropDownButton>
                                                <%-- <clientsideevents datechanged="function(s,e){
                                                DateValidation(dtValuationDate,
                                                'N','N',
                                                'N','N',
                                                'Y','N',
                                                'N','N',
                                                'N',0,
                                                'N',0,
                                                'N','N');}">
                                        </clientsideevents>--%>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Segment :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentAll" runat="server" Checked="True" GroupName="a" />All
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButton ID="rdbSegmentSelected" runat="server" GroupName="a" />Specific
                                            [ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]
                                        </td>
                                    </tr>
                                    <tr id="tr_consolidated">
                                        <td class="gridcellleft" bgcolor="#B7CEEC" colspan="4">
                                            <asp:CheckBox ID="chkconsolidated" runat="server" onclick="fn_Consolidated(this)" />
                                            Consolidate for All Accounts
                                        </td>
                                    </tr>
                                    <tr id="tr_grp">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Group By :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                <asp:ListItem Value="0">Branch</asp:ListItem>
                                                <asp:ListItem Value="1">Group</asp:ListItem>
                                                <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" id="td_branch">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="b" onclick="Branch('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="b" onclick="Branch('b')" />Selected
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
                                                        <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="c"
                                                            onclick="Group('a')" />
                                                        All
                                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="c" onclick="Group('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="tr_client">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Clients :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="d" onclick="Clients('a')" />
                                            All Client
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radPOAClient" runat="server" GroupName="d" onclick="Clients('a')" />POA
                                            Client
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="d" onclick="Clients('b')" />
                                            Selected Client
                                        </td>
                                    </tr>
                                    <tr id="Tr_Company" style="display: none">
                                        <td bgcolor="#b7ceec" class="gridcellleft">Company :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbCompanyALL" runat="server" Checked="True" GroupName="RB_GCompany" onclick="Company('A')" />ALL</td>
                                        <td>
                                            <asp:RadioButton ID="rdbCompanySelected" runat="server" GroupName="RB_GCompany" onclick="Company('S')" Checked="true" />Specific
                                        </td>
                                        <td>[ <span id="Span_SpecificComp" runat="server" style="color: Maroon"></span>]
                                        </td>
                                    </tr>
                                    <tr id="Tr_CompanyFilter" style="display: none">
                                        <td bgcolor="#b7ceec" class="gridcellleft">Company-Filter :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbOnlyQty" runat="server" Checked="True" GroupName="RB_GCompanyFilter" />Only Qty.</td>
                                        <td>
                                            <asp:RadioButton ID="rdbWithValuation" runat="server" GroupName="RB_GCompanyFilter" />With Valuation</td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" class="gridcellleft">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" style="width: 100px;" bgcolor="#B7CEEC">Asset :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbunderlyingall" runat="server" Checked="True" GroupName="e"
                                                onclick="fnunderlying('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbunderlyingselected" runat="server" GroupName="e" onclick="fnunderlying('b')" />Selected
                                            Asset
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" class="gridcellleft">
                                <table>
                                    <tr>
                                        <td bgcolor="#B7CEEC" id="Td_ExcludeSqrOff">
                                            <asp:CheckBox ID="chkexcludesqr" runat="server" />
                                            Exclude Sqr-Off Trades
                                        </td>
                                        <td bgcolor="#B7CEEC" id="Td_ExcludeSTT">
                                            <asp:CheckBox ID="chkstt" runat="server" />
                                            Exclude STT
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="5">
                                <table class="tableClass">
                                    <tr>
                                        <td id="Td_RadioBtnScreen" class="gridcellleft">
                                            <asp:RadioButton ID="rbScreen" runat="server" GroupName="f" Checked="True" onclick="RBShowHide(this.value)" />Screen
                                        </td>
                                        <td id="Td_RadioBtnPrint" class="gridcellleft">
                                            <asp:RadioButton ID="rbPrint" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Print
                                        </td>
                                        <td style="display: none;" id="td_ChkDISTRIBUTION">
                                            <table>
                                                <tr>
                                                    <td id="tr_printlogo">
                                                        <asp:CheckBox ID="CHKLOGOPRINT" runat="server" Checked="true" />
                                                        Do Not Print Logo</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="ChkDISTRIBUTION" runat="server" />
                                                        Distribution Copy</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="Td_RadioBtnExcel" class="gridcellleft">
                                            <asp:RadioButton ID="rbExcel" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Excel
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td id="td_btnshow" class="gridcellleft">
                                            <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                Width="101px" OnClick="btnshow_Click" />
                                        </td>
                                        <td id="td_btnprint">
                                            <asp:Button ID="btnprint" runat="server" CssClass="btnUpdate" Height="20px" Text="Print"
                                                OnClientClick="selecttion()" Width="101px" OnClick="btnprint_Click" />
                                        </td>
                                        <td id="Td_Excel">
                                            <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Open To Excel"
                                                OnClientClick="selecttion()" Width="101px" OnClick="btnExcel_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table width="100%" id="showFilter" class="tableClass">
                        <tr>
                            <td style="text-align: right; vertical-align: top; height: 134px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                            id="TdFilter">
                                            <span id="spanunder"></span><span id="spanclient"></span>
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
                                                <asp:ListItem>Product</asp:ListItem>
                                                <asp:ListItem>Company</asp:ListItem>
                                                <asp:ListItem>BranchGroup</asp:ListItem>
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
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Product" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                    <asp:HiddenField ID="HiddenField_Company" runat="server" />
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
        <div id="displayAll" style="display: none;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">
                        <tr style="display: none;">
                            <td>
                                <asp:DropDownList ID="cmbclient" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                                </asp:DropDownList><asp:HiddenField ID="hiddencount" runat="server" />
                            </td>
                        </tr>
                        <tr id="tr_DIVdisplayPERIOD">
                            <td>
                                <div id="DIVdisplayPERIOD" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr id="tr_group">
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
                                                    <asp:DropDownList ID="cmbgroup" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cmbgroup_SelectedIndexChanged">
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
                        <tr bordercolor="Blue" id="tr_prvnxt">
                            <td align="center">
                                <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="100%">
                                    <tr>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();;" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                        <td width="25">
                                            <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                        <td width="20">
                                            <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();"
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
                                <div id="display" runat="server">
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
