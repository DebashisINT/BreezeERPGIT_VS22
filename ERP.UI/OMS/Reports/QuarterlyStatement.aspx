<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_QuarterlyStatement" CodeBehind="QuarterlyStatement.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v10.2.Export" Namespace="DevExpress.Web.Export"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>
    <script type="text/javascript" src="../CentralData/JSScript/GenericJScript.js"></script>
    
    <!--Start For Ajax-->
    <link type="text/css" href="../CentralData/CSS/GenericAjaxStyle.css" rel="Stylesheet" />
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
    <style type="text/css">
        form {
            display: inline;
        }

        .frmleftCont {
            float: left;
            margin: 2px;
            padding: 2px;
            height: 26px;
            border: solid 1px #D1E0F3;
            font-size: 12px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        groupvalue = "";
        var allcontract = '';
        var release = '';
        var ecnenable = '';
        var deliver = '';
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
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
        function AllSelct(obj, obj1) {
            var FilTer = document.getElementById('cmbsearchOption');
            if (obj != 'a') {
                if (obj1 == 'C')
                    FilTer.value = 'Clients';
                else if (obj1 == 'B')
                    FilTer.value = 'Branch';
                else if (obj1 == 'G')
                    FilTer.value = 'Group';
                Show('TdFilter');
            }
            else
                Hide('TdFilter');
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
        function keyVal(obj) {
            var WhichCall = obj.split("~")[4];
            if (WhichCall == "DIGISIGN") {
                var isEtoken = obj.split("~")[2];
                if (isEtoken == "E") {
                    Show('tdGenPDF');
                    Hide('tdShowPopUp');
                }
                else {
                    Hide('tdGenPDF');
                    Show('tdShowPopUp');
                }
            }
        }
        function CallAjax(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function CallAjax1(obj1, obj2, obj3, Query) {
            var CombinedQuery = new String(Query);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
        }
        Fieldname = 'none'
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
                cmbVal = document.getElementById('cmbsearchOption').value;
                cmbVal = cmbVal + '~' + document.getElementById('ddlgrouptype').value;
            }

            ajax_showOptions(objID, objListFun, objEvent, cmbVal);
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
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }
            Hide('TdFilter');
        }
        function btnAddsubscriptionlist_click() {
            var userid = document.getElementById('txtsegselected');
            if (userid.value != '') {
                var ids = document.getElementById('txtsegselected_hidden');
                var listBox = document.getElementById('lstSuscriptions');
                var tLength = listBox.length;
                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtsegselected');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!')
            var s = document.getElementById('txtsegselected');
            s.focus();
            s.select();
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
        function Page_Load() {
            height();
            Hide('TdFilter');
            Hide('tdHeader');
            Hide('tdfooter');
            Hide('tdAddSig');
            Hide('td_BtnPrint');
            Hide('Div_PDF');
            Hide('Tr_DigitalSign');
            Hide('tr_email');
            Hide('tr_range');
            Hide('ecndetail');
            Hide('trshow');
            Hide('tdGenPDF');
            // Hide('tr_period');

        }
        function height() {

            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function ChkCheckProperty(obj, objChk) {
            if (objChk == true) {
                if (obj == 'H')
                    Show('tdHeader');
                else if (obj == 'F')
                    Show('tdfooter');
            }
            else {
                if (obj == 'H')
                    Hide('tdHeader');
                else if (obj == 'F')
                    Hide('tdfooter');
            }
        }
        function FunHeaderFooter(objID, objListFun, objEvent, objParam) {
            ajax_showOptions(objID, objListFun, objEvent, objParam);
        }
        function ChkAddSig(obj) {
            if (obj == true)
                Show('tdAddSig');
            else
                Hide('tdAddSig');
        }
        function FunAddSig(objID, objListFun, objEvent) {
            ajax_showOptions(objID, objListFun, objEvent);
        }
        FieldName = 'lstSuscriptions'

        function btnShow_Click() {

            cCbpGenerateReport.PerformCallback("SearchByNavigation~" + 1);

            Show('div_PDF');
            height();

        }
        function OnLeftNav_Click() {
            var i = document.getElementById("CbpReportViewer_A1").innerText;

            if (parseInt(i) > 1) {
                i = parseInt(i) - 10;
                for (l = 1; l < 11; l++) {
                    var obj = "CbpReportViewer_A" + l;
                    document.getElementById(obj).innerText = i++;
                    var objimg = "imgA" + l;
                    document.getElementById(objimg).style.visibility = 'visible';
                    var print = "test_" + l;
                    document.getElementById(print).style.visibility = 'visible';
                    //document.getElementById(print).style.color="blue";
                }
                document.getElementById("CbpGenerateReport_B_PageNo").innerText = document.getElementById("CbpReportViewer_A1").innerText;

            }
            else {
                alert('You are on the Beginning');
            }
        }
        function OnRightNav_Click() {
            var TestEnd = document.getElementById("CbpReportViewer_A10").innerText;
            var TotalPage = document.getElementById("CbpGenerateReport_B_TotalPage").innerText;

            if (TestEnd == "" || TestEnd == TotalPage) {
                alert('You are at the End');
                return;
            }
            var i = document.getElementById("CbpReportViewer_A1").innerText;
            //document.getElementById("test_"+i).style.color="blue";
            if (parseInt(i) < TotalPage) {
                i = parseInt(i) + 10;
                var n = parseInt(TotalPage) - parseInt(i) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(i) + 2;
                for (r = 1; r < n; r++) {
                    var obj = "CbpReportViewer_A" + r;
                    document.getElementById(obj).innerText = i++;
                    var print = "test_" + r;
                    var objimg = "imgA" + r;

                    document.getElementById(print).style.visibility = 'visible';
                    document.getElementById(objimg).style.visibility = 'visible';
                    //document.getElementById(print).style.color="blue";

                }
                for (r = n; r < 11; r++) {
                    var objimg = "imgA" + r;
                    var obj = "CbpReportViewer_A" + r;
                    document.getElementById(objimg).style.visibility = 'hidden';
                    document.getElementById(obj).innerText = "";

                    var print = "test_" + r;

                    document.getElementById(print).style.visibility = 'hidden';
                    //document.getElementById(print).style.color="blue";

                }
                document.getElementById("CbpGenerateReport_B_PageNo").innerText = document.getElementById("CbpReportViewer_A1").innerText;
            }
            else {
                alert('You are at the End');
            }
        }
        function OnPageNo_Click(obj) {

            var ObjInContainer = "CbpReportViewer_" + obj;
            var i = document.getElementById(ObjInContainer).innerText;
            document.getElementById("CbpGenerateReport_B_PageNo").innerText = i;
            //alert(document.getElementById("CbpGenerateReport_B_PageNo").innerText);
            cCbpGenerateReport.PerformCallback("GeneratePDF~" + i);
            //document.getElementById("test_"+i).style.color="red";



        }
        function cCbpReportViewer_EndCallBack() {
            var strUndefined = new String(cCbpReportViewer.cpIsEmptyDsSearch);
            if (strUndefined != "undefined" && strUndefined != "NoRecord") {
                document.getElementById("CbpGenerateReport_B_PageNo").innerText = strUndefined.split('~')[1];
                document.getElementById("CbpGenerateReport_B_TotalPage").innerText = strUndefined.split('~')[2];
                document.getElementById("CbpGenerateReport_B_TotalRows").innerText = strUndefined.split('~')[3];

                var i = document.getElementById("CbpReportViewer_A1").innerText;
                var TotalPage = strUndefined.split('~')[2];
                if (parseInt(i) <= TotalPage && parseInt(i) == 1) {
                    n = (parseInt(TotalPage) - parseInt(i) > 10) ? parseInt(11) : parseInt(TotalPage) - parseInt(i) + 2;
                    for (a = 1; a < n; a++) {
                        var obj = "CbpReportViewer_A" + a;
                        document.getElementById(obj).innerText = a;
                    }
                    for (a = n; a < 11; a++) {
                        var obj = "CbpReportViewer_A" + a;
                        var objimg = "imgA" + a;
                        document.getElementById(objimg).style.visibility = 'hidden';
                        document.getElementById(obj).innerText = "";
                    }
                }
            }
        }
        function CbpGenerateReport_EndCallBack() {
            var strUndefined = new String(cCbpGenerateReport.cpIsEmptyDsSearch);
            if (strUndefined != "undefined" && strUndefined != "NoRecord") {
                document.getElementById("CbpGenerateReport_B_PageNo").innerText = strUndefined.split('~')[1];
                document.getElementById("CbpGenerateReport_B_TotalPage").innerText = strUndefined.split('~')[2];
                document.getElementById("CbpGenerateReport_B_TotalRows").innerText = strUndefined.split('~')[3];
                //var test=document.getElementById("CbpGenerateReport_B_TotalPage").innerText=strUndefined.split('~')[2];
                //                 if (test=="2")
                //                 {
                //                 Show('imgA1');
                //                 Show('imgA2');
                //                 Hide('imgA3');
                //                 }

                var i = document.getElementById("CbpReportViewer_A1").innerText;

                var TotalPage = strUndefined.split('~')[2];
                if ((parseInt(i) <= TotalPage) && (parseInt(i) == 1)) {


                    n = (parseInt(TotalPage) - parseInt(i) > 10) ? parseInt(11) : parseInt(TotalPage) - parseInt(i) + 2;
                    for (a = 1; a < n; a++) {

                        var obj = "CbpReportViewer_A" + a;
                        document.getElementById(obj).innerText = a;
                        var print = "test_" + a;
                        var objimg = "imgA" + a;

                        document.getElementById(print).style.visibility = 'visible';
                        document.getElementById(objimg).style.visibility = 'visible';


                    }
                    for (a = n; a < 11; a++) {

                        var obj = "CbpReportViewer_A" + a;
                        var objimg = "imgA" + a;
                        var print = "test_" + a;
                        document.getElementById(objimg).style.visibility = 'hidden';
                        document.getElementById(obj).innerText = "";
                        document.getElementById(print).style.visibility = 'hidden';
                        //document.getElementById(print).style.color="red";
                        //                        document.getElementById("test_"+i).innerHTML="Opened";
                    }
                }

            }
            var strUndefined = new String(cCbpGenerateReport.cpGeneratePDF);
            if (strUndefined == "GenratePDF") {
                document.getElementById("BtnPrint").click();
            }
        }
        function FnddlGeneration(obj) {
            if (obj == "1") {
                Hide('Tr_DigitalSign');
                Hide('tr_email');
                Show('tr_pdf');
                Show('tr_signatory');
                Show('tr_both');
                Show('tr_logo');
                Hide('ecndetail');
                Hide('trshow');

            }
            else {

                Hide('Tr_DigitalSign');

                Hide('tr_email');
                Hide('tr_pdf');
                Hide('tr_signatory');
                Hide('tr_both');
                Hide('tr_logo');
                Hide('ecndetail');
                Show('trshow');
            }
        }
        function FnddlGenerationtype(obj) {
            if (obj == "1") {
                Hide('tr_range');
                Show('tr_qtr');
                Show('tr_period');

            }
            else {
                Hide('tr_period');
                Show('tr_range');
                Hide('tr_qtr');
            }
        }
        function DateChangeForFrom() {
            //var datePost=(dtFrom.GetDate().getMonth()+2)+'-'+dtFrom.GetDate().getDate()+'-'+dtFrom.GetDate().getYear();
            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtDate.GetDate().getMonth() + 1;
            var DayDate = dtDate.GetDate().getDate();
            var YearDate = dtDate.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();

            if (YearDate >= objsession[0]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtDate.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                dtDate.SetDate(new Date(datePost));
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

        function fn_show() {
            Show('Tr_DigitalSign');
            cCbptxtSelectionID.PerformCallback('v4');
        }
        function alertcall() {
            alert('No Record To Export !!');
            Page_Load();
        }
        function CbptxtSelectionID_EndCallBack() {
            if (cCbptxtSelectionID.cpproperties == "allcontract_excel")
                document.getElementById('BtnForExportEvent').click();
            if (cCbptxtSelectionID.cpproperties == "releasetotal_excel")
                document.getElementById('BtnForExportEvent1').click();
            if (cCbptxtSelectionID.cpproperties == "ecnenable_excel")
                document.getElementById('BtnForExportEvent2').click();
            if (cCbptxtSelectionID.cpproperties == "deliveryrpt_excel")
                document.getElementById('BtnForExportEvent3').click();

            if ((cCbptxtSelectionID.cpallcontract != null) && (cCbptxtSelectionID.cpecnenable != null) && (cCbptxtSelectionID.cpdeliveryrpt != null) && (cCbptxtSelectionID.cpreleasetotal != null)) {
                document.getElementById('<%=B_allcontract.ClientID %>').innerHTML = cCbptxtSelectionID.cpallcontract;
            document.getElementById('<%=B_ecnenable.ClientID %>').innerHTML = cCbptxtSelectionID.cpecnenable;
            document.getElementById('<%=B_releasetotal.ClientID %>').innerHTML = cCbptxtSelectionID.cpreleasetotal;
            document.getElementById('<%=B_deliveryrpt.ClientID %>').innerHTML = cCbptxtSelectionID.cpdeliveryrpt;
            allcontract = cCbptxtSelectionID.cpallcontract;
            release = cCbptxtSelectionID.cpreleasetotal;
            ecnenable = cCbptxtSelectionID.cpecnenable;
            deliver = cCbptxtSelectionID.cpdeliveryrpt;
            Show('ecndetail');
            //document.getElementById('td_allcontract').disabled = true;
            //alert('1');
        }
        if ((cCbptxtSelectionID.cpallcontractpop != null) && (cCbptxtSelectionID.cpecnenablepop != null)) {

            document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML = cCbptxtSelectionID.cpallcontractpop;
          document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML = cCbptxtSelectionID.cpecnenablepop;
          var remn = document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML = cCbptxtSelectionID.cpallcontractpop;
          var remn1 = document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML = cCbptxtSelectionID.cpecnenablepop;
          if (remn == '0' || remn1 == '0') {

              Hide('btnremain');
          }
          else {
              Show('btnremain');
          }
          Hide('btnokdiv');
          Hide('div_fail');
          Hide('div_success');
          cPopUp_ScripAlert.Show();

      }
      if ((cCbptxtSelectionID.cpvisibletrue == "yes") || (cCbptxtSelectionID.cpecnenable == '0'))
          btnopenpopup.SetEnabled(false);
      if ((cCbptxtSelectionID.cpvisibletrue == "no") && (cCbptxtSelectionID.cpecnenable != '0'))
          btnopenpopup.SetEnabled(true);
      if (cCbptxtSelectionID.cpGeneratePDF == "Y") {
          Show('tdGenPDF');
          Hide('tdShowPopUp');
          cCbptxtSelectionID.cpGeneratePDF = null;
      }


  }
  function CbpSuggestISIN_EndCallBack() {
      if ((cCbpSuggestISIN.cpallcontractpops != null) && (cCbpSuggestISIN.cpecnenablepops != null)) {

          document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML = cCbpSuggestISIN.cpallcontractpops;
            document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML = cCbpSuggestISIN.cpecnenablepops;
            var remn2 = cCbpSuggestISIN.cpallcontractpops;
            var remn3 = cCbpSuggestISIN.cpecnenablepops;

            if (remn2 == '0' || remn3 == '0') {

                Hide('btnremain');
                Hide('btnall');
                Hide('cancel');


            }
            else {

                Show('btnremain');
                Hide('btnall');
                Hide('cancel');


            }
            if (remn3 != '0')
                //if('<%=Session["Error"]%>'=='err')
            {
                Show('div_fail');
                Hide('div_success');

            }
            else {
                Hide('div_fail');
                Show('div_success');

            }
            Show('btnokdiv');


        }
        if (cCbpSuggestISIN.cpGenSuccesssfully == "Y") {
            alert("PDF successfully Generated!!");
            cbtnGeneratePDF.SetEnabled(true);
            window.location = "../reports/QuarterlyStatement.aspx";
        }
        else if (cCbpSuggestISIN.cpGenSuccesssfully == "N") {
            alert("AN INTERNAL ERROR OCCURED!!");
            cbtnGeneratePDF.SetEnabled(true);
        }
    }
    function fn_showpopup() {
        if (document.getElementById('txtdigitalName_hidden').value != "")
            cCbptxtSelectionID.PerformCallback('v5');
        else
            alert('Please select Signature !!');
    }
    function btncancel_Click() {

        cPopUp_ScripAlert.Hide();
    }
    function btnsendall_Click() {
        cbtnGeneratePDF.SetText("Please Wait...");
        cbtnGeneratePDF.SetEnabled(false);
        cCbpSuggestISIN.PerformCallback('all');
    }
    function btnok_Click() {
        window.location = '../reports/QuarterlyStatement.aspx';
    }
    function btnsendremaining_Click() {
        cCbpSuggestISIN.PerformCallback('remain');
    }
    function excelexport(obj) {
        if (obj == 'allcontract_excel') {
            if (allcontract == '0')
                alert('No Record To Export !!')
            else
                cCbptxtSelectionID.PerformCallback(obj);
        }
        if (obj == 'releasetotal_excel') {
            if (release == '0')
                alert('No Record To Export !!')
            else
                cCbptxtSelectionID.PerformCallback(obj);
        }
        if (obj == 'ecnenable_excel') {
            if (ecnenable == '0')
                alert('No Record To Export !!')
            else
                cCbptxtSelectionID.PerformCallback(obj);
        }
        if (obj == 'deliveryrpt_excel') {
            if (deliver == '0')
                alert('No Record To Export !!')
            else
                cCbptxtSelectionID.PerformCallback(obj);
        }
        //alert('Still on working !! Released in coming version !!');

    }

    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            var j = rValue.split('~');
            var btn = document.getElementById('btnhide');

            if (j[0] == 'Branch') {
                document.getElementById('HiddenField_Branch').value = j[1];
            }
            if (j[0] == 'Group') {
                //btn.click();
                document.getElementById('HiddenField_Group').value = j[1];
            }
            if (j[0] == 'Clients') {
                document.getElementById('HiddenField_Client').value = j[1];
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Quarterly Statement Of Accounts</span></strong>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td valign="top">
                    <table>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <%-- border="10" cellpadding="1" cellspacing="1">--%>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report Generation Type :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlrptgeneration" runat="server" Width="200px" Font-Size="12px"
                                                onchange="FnddlGenerationtype(this.value)">
                                                <asp:ListItem Value="1">For A Quarter</asp:ListItem>
                                                <asp:ListItem Value="2">For A Date Range</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_qtr">
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Date :
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="DdlQuatarlyDate" runat="server" ClientInstanceName="DdlQuatarlyDate"
                                                Width="150px" Font-Size="12px">
                                                <%--<ClientSideEvents SelectedIndexChanged="FnQuatarlyDate()" />--%>
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_range">
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Date :
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" EditFormatString="dd-MM-yyyy" ClientInstanceName="dtDate">
                                                <DropDownButton Text="From">
                                                </DropDownButton>
                                                <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" EditFormatString="dd-MM-yyyy" ClientInstanceName="dtToDate">
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
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Group By</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                <asp:ListItem Value="0">Branch</asp:ListItem>
                                                <asp:ListItem Value="1">Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" id="td_branch">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="AllSelct('a','B')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="AllSelct('b','B')" />Selected
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
                                                            onclick="AllSelct('a','G')" />
                                                        All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="AllSelct('b','G')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">Client :</td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="AllSelct('a','C')" /></td>
                                        <td>All Client</td>
                                        <td>
                                            <asp:RadioButton ID="radPOAClient" runat="server" GroupName="c" onclick="AllSelct('a','C')" />
                                        </td>
                                        <td>POA Client</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="AllSelct('b','C')" /></td>
                                        <td>Selected Client</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <%-- border="10" cellpadding="1" cellspacing="1">--%>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report Output Type :</td>
                                        <td>
                                            <asp:DropDownList ID="DdlGeneRationType" runat="server" Width="200px" Font-Size="12px"
                                                onchange="FnddlGeneration(this.value)">
                                                <asp:ListItem Value="1">PDF Document</asp:ListItem>
                                                <asp:ListItem Value="2">Digital Signed Email</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Use Header :
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkHeader" runat="server" onclick="ChkCheckProperty('H',this.checked);" />
                                        </td>
                                        <td id="tdHeader">
                                            <asp:TextBox ID="txtHeader" runat="server" Width="279px" Font-Size="12px" onkeyup="FunHeaderFooter(this,'GetHeaderFooter',event,'H')"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Use Footer :
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkFooter" runat="server" onclick="ChkCheckProperty('F',this.checked);" />
                                        </td>
                                        <td id="tdfooter">
                                            <asp:TextBox ID="txtFooter" runat="server" Width="279px" Font-Size="12px" onkeyup="FunHeaderFooter(this,'GetHeaderFooter',event,'F')"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_both" runat="server">
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Both Side Print :
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkBothPrint" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_signatory" runat="server">
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Add Signatory :
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="ChkSignatory" runat="server" onclick="ChkAddSig(this.checked);" />
                                        </td>
                                        <td id="tdAddSig">
                                            <asp:TextBox ID="txtSignature" runat="server" Width="279px" Font-Size="12px" onkeyup="FunAddSig(this,'SearchByEmployeesWithSignature',event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_logo" runat="server">
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Use Company Logo :
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="ChkLogo" runat="server" Checked="true"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Statement Date :
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="DtSatementDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="98px">
                                                <ClientSideEvents ValueChanged="function(s,e){DateChangeForTo();}" />
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC" id="tr_period">
                                            <asp:CheckBox ID="ChkConsiderEntirePeriod" runat="server"></asp:CheckBox>Consider
                                                Entire Period
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkDoNotPrintSecurities" runat="server"></asp:CheckBox>Do Not
                                                Print Register Of Securities
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr id="trshow">
                            <td class="gridcellleft">
                                <dxe:ASPxButton ID="btnshowecn" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                                    Height="5px" Text="Show" Width="101px">
                                    <ClientSideEvents Click="function (s, e) {fn_show();}" />
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    <div style="margin-left: 10px;">
                        <table class="gridcellleft" cellpadding="0" cellspacing="0" border="1" id="ecndetail"
                            style="padding: 2px;">
                            <tr style="background-color: lavender; text-align: left">
                                <td colspan="5">
                                    <b>Quarterly Statement Related Detail </b>
                                </td>
                            </tr>
                            <tr style="background-color: #DBEEF3;">
                                <td colspan="2">
                                    <b>Total Client</b>
                                </td>
                                <td colspan="2">
                                    <b>Report Generating Client</b>
                                </td>
                                <td colspan="2">
                                    <b>Email Enable Report Generating Client</b>
                                </td>
                                <td colspan="2">
                                    <b>Delivered Client</b>
                                </td>
                            </tr>
                            <tr style="background-color: white;">
                                <td>

                                    <b style="text-align: right" id="B_allcontract" runat="server"></b>
                                </td>
                                <td id="td_allcontract">
                                    <a href="javascript:excelexport('allcontract_excel');"><span style="color: Blue; text-decoration: underline; vertical-align: bottom; font-size: 12px">View Detail</span></a>
                                </td>

                                <td>

                                    <b style="text-align: right" id="B_releasetotal" runat="server"></b>
                                </td>
                                <td>
                                    <a href="javascript:excelexport('releasetotal_excel');"><span style="color: Blue; text-decoration: underline; vertical-align: bottom; font-size: 12px">View Detail</span></a>
                                </td>



                                <td>

                                    <b style="text-align: right" id="B_ecnenable" runat="server"></b>
                                </td>
                                <td>
                                    <a href="javascript:excelexport('ecnenable_excel');"><span style="color: Blue; text-decoration: underline; vertical-align: bottom; font-size: 12px">View Detail</span></a>
                                </td>
                                <td>

                                    <b style="text-align: right" id="B_deliveryrpt" runat="server"></b>
                                </td>
                                <td>
                                    <a href="javascript:excelexport('deliveryrpt_excel');"><span style="color: Blue; text-decoration: underline; vertical-align: bottom; font-size: 12px">View Detail</span></a>
                                </td>
                            </tr>
                            <%--<tr id="tr_openpopup">
                                <td class="gridcellleft" colspan="5">
                                    <dxe:ASPxButton ID="btnopenpopup" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                                        Height="5px" Text="Generate" Width="101px">
                                        <clientsideevents click="function (s, e) {fn_showpopup();}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                    <table>
                        <tr id="Tr_DigitalSign">
                            <td colspan="2">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Select Signature :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdigitalName" runat="server" Width="250px" onkeyup="FunCallAjaxList(this,event,'Digital')"></asp:TextBox>
                                            <asp:TextBox ID="txtdigitalName_hidden" runat="server" TabIndex="11" Width="100px"
                                                Style="display: none"></asp:TextBox>
                                        </td>
                                        <td id="td_msg" runat="server">
                                            <asp:Label ID="Location" runat="server" Text="You Dont have Permission to send/ Contact to Administrator"
                                                ForeColor="red" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr_openpopup">
                                        <td id="tdShowPopUp" class="gridcellleft" style="text-align: left;">
                                            <dxe:ASPxButton ID="btnopenpopup" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                                                Height="5px" Text="Generate" Width="101px">
                                                <ClientSideEvents Click="function (s, e) {fn_showpopup();}" />
                                            </dxe:ASPxButton>
                                        </td>
                                        <td id="tdGenPDF" class="gridcellleft" style="text-align: left;">
                                            <dxe:ASPxButton ID="btnGeneratePDF" runat="server" ClientInstanceName="cbtnGeneratePDF" CssClass="btnUpdate" AutoPostBack="False"
                                                Height="5px" Text="Generate PDF" Width="150px">
                                                <ClientSideEvents Click="function (s, e) {btnsendall_Click();}" />
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_pdf" runat="server">
                            <td id="td_BtnPrint">
                                <asp:Button ID="BtnPrint" runat="server" Text="Print" CssClass="btnUpdate" Width="165px"
                                    OnClick="BtnPrint_Click" />
                            </td>
                            <td align="center">
                                <a id="btnShow" style="cursor: pointer;" onclick="btnShow_Click()">
                                    <div style="height: 12px; padding: 3px 2px 3px 2px; width: 90px; border: 1px solid blue; line-height: 14px; font-size: 11px; font-weight: bold; background: url(../images/EHeaderBack.gif) repeat-x 0px 0px;">
                                        <span>Generate</span>
                                    </div>
                                </a>
                            </td>
                        </tr>
                        <tr id="tr_email" runat="server">
                            <%--<td class="gridcellleft">
                                    <asp:Button ID="btnemail" runat="server" CssClass="btnUpdate" Height="20px" Text="Sent Email"
                                        Width="101px" OnClick="btnemail_Click" /></td>--%>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                    <div id="div_PDF">
                        <dxe:ASPxCallbackPanel ID="CbpGenerateReport" runat="server" ClientInstanceName="cCbpGenerateReport"
                            OnCallback="CbpGenerateReport_Callback">
                            <ClientSideEvents EndCallback="function(s, e) {CbpGenerateReport_EndCallBack(); }" />
                            <PanelCollection>
                                <dxe:panelcontent runat="server">
                                        <table>
                                            <tr>
                                                <td style="vertical-align: top; width: 34px; height: 11px; background-color: #b7ceec;
                                                    text-align: left" valign="top">
                                                    Page</td>
                                                <td style="width: 4px" valign="top">
                                                    <b id="B_PageNo" runat="server" style="text-align: right"></b>
                                                </td>
                                                <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                    valign="top">
                                                    Of
                                                </td>
                                                <td valign="top">
                                                    <b id="B_TotalPage" runat="server" style="text-align: right"></b>
                                                </td>
                                                <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                    valign="top">
                                                    ( <b id="B_TotalRows" runat="server" style="text-align: right"></b>&nbsp;items )
                                                </td>
                                                <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                    valign="top">
                                                    <a id="A_LeftNav" runat="server" href="javascript:void(0);" onclick="OnLeftNav_Click()">
                                                        <img src="/assests/images/LeftNav.gif" style="text-decoration: underline" width="10" /><span
                                                            style="color: #000000">Previous </span></a>
                                                </td>
                                                <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: right"
                                                    valign="top">
                                                    <a id="A_RightNav" runat="server" href="javascript:void(0);" onclick="OnRightNav_Click()">
                                                        <span style="color: #000000">Next </span>
                                                        <img src="../images/RightNav.gif" style="text-decoration: underline" width="10" /></a>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxe:panelcontent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>
                        <dxe:ASPxCallbackPanel ID="CbpReportViewer" runat="server" ClientInstanceName="cCbpReportViewer">
                            <%--<ClientSideEvents EndCallback="function(s, e) {cCbpReportViewer_EndCallBack(); }" />--%>
                            <PanelCollection>
                                <dxe:panelcontent runat="server">
                                        <table border="1" style="width: 60%">
                                            <tr>
                                                <td valign="top">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="vertical-align: top; color: #000000; height: 11px; background-color: #b7ceec;
                                                                text-align: left" valign="top">
                                                                <div id="imgA1">
                                                                    <img src="/assests/images/PDFICON.jpg" /></div>
                                                                <span id="test_1" style="color: Blue; text-decoration: underline; cursor: pointer;"
                                                                    onclick="OnPageNo_Click('A1')">Print</span> <a id="A1" runat="server" href="javascript:void(0);"
                                                                        onclick="OnPageNo_Click('A1')"><span style="color: Blue; text-decoration: underline;">
                                                                            1</span> </a>
                                                            </td>
                                                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                                valign="top">
                                                                <div id="imgA2">
                                                                    <img src="/assests/images/PDFICON.jpg" /></div>
                                                                <span id="test_2" style="color: Blue; text-decoration: underline; cursor: pointer;"
                                                                    onclick="OnPageNo_Click('A2')">Print</span> <a id="A2" runat="server" href="javascript:void(0);"
                                                                        onclick="OnPageNo_Click('A2')"><span style="color: Blue; text-decoration: underline;">
                                                                            2</span> </a>
                                                            </td>
                                                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                                valign="top">
                                                                <div id="imgA3">
                                                                    <img id="A3" src="/assests/images/PDFICON.jpg" /></div>
                                                                <span id="test_3" style="color: Blue; text-decoration: underline; cursor: pointer;"
                                                                    onclick="OnPageNo_Click('A3')">Print</span><a id="A3" runat="server" href="javascript:void(0);"
                                                                        onclick="OnPageNo_Click('A3')"> <span style="color: Blue; text-decoration: underline;">
                                                                            3</span> </a>
                                                            </td>
                                                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                                valign="top">
                                                                <div id="imgA4">
                                                                    <img id="Img1" src="/assests/images/PDFICON.jpg" /></div>
                                                                <span id="test_4" style="color: Blue; text-decoration: underline; cursor: pointer;"
                                                                    onclick="OnPageNo_Click('A4')">Print</span><a id="A4" runat="server" href="javascript:void(0);"
                                                                        onclick="OnPageNo_Click('A4')"> <span style="color: Blue; text-decoration: underline;">
                                                                            4</span> </a>
                                                            </td>
                                                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                                valign="top">
                                                                <div id="imgA5">
                                                                    <img id="Img2" src="/assests/images/PDFICON.jpg" /></div>
                                                                <span id="test_5" style="color: Blue; text-decoration: underline; cursor: pointer;"
                                                                    onclick="OnPageNo_Click('A5')">Print</span><a id="A5" runat="server" href="javascript:void(0);"
                                                                        onclick="OnPageNo_Click('A5')"> <span style="color: Blue; text-decoration: underline;">
                                                                            5</span> </a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                                valign="top">
                                                                <div id="imgA6">
                                                                    <img id="Img3" src="/assests/images/PDFICON.jpg" /></div>
                                                                <span id="test_6" style="color: Blue; text-decoration: underline; cursor: pointer;"
                                                                    onclick="OnPageNo_Click('A6')">Print</span><a id="A6" runat="server" href="javascript:void(0);"
                                                                        onclick="OnPageNo_Click('A6')"> <span style="color: Blue; text-decoration: underline;">
                                                                            6</span> </a>
                                                            </td>
                                                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                                valign="top">
                                                                <div id="imgA7">
                                                                    <img id="Img4" src="/assests/images/PDFICON.jpg" /></div>
                                                                <span id="test_7" style="color: Blue; text-decoration: underline; cursor: pointer;"
                                                                    onclick="OnPageNo_Click('A7')">Print</span><a id="A7" runat="server" href="javascript:void(0);"
                                                                        onclick="OnPageNo_Click('A7')"> <span style="color: Blue; text-decoration: underline;">
                                                                            7</span> </a>
                                                            </td>
                                                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                                valign="top">
                                                                <div id="imgA8">
                                                                    <img id="Img5" src="/assests/images/PDFICON.jpg" /></div>
                                                                <span id="test_8" style="color: Blue; text-decoration: underline; cursor: pointer;"
                                                                    onclick="OnPageNo_Click('A8')">Print</span><a id="A8" runat="server" href="javascript:void(0);"
                                                                        onclick="OnPageNo_Click('A8')"> <span style="color: Blue; text-decoration: underline;">
                                                                            8</span> </a>
                                                            </td>
                                                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                                valign="top">
                                                                <div id="imgA9">
                                                                    <img id="Img6" src="/assests/images/PDFICON.jpg" /></div>
                                                                <span id="test_9" style="color: Blue; text-decoration: underline; cursor: pointer;"
                                                                    onclick="OnPageNo_Click('A9')">Print</span><a id="A9" runat="server" href="javascript:void(0);"
                                                                        onclick="OnPageNo_Click('A9')"> <span style="color: Blue; text-decoration: underline;">
                                                                            9</span> </a>
                                                            </td>
                                                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left"
                                                                id="td_ten" runat="server" valign="top">
                                                                <div id="imgA10">
                                                                    <img id="Img7" src="/assests/images/PDFICON.jpg" /></div>
                                                                <span id="test_10" style="color: Blue; text-decoration: underline; cursor: pointer;"
                                                                    onclick="OnPageNo_Click('A10')">Print</span><a id="A10" runat="server" href="javascript:void(0);"
                                                                        onclick="OnPageNo_Click('A10')"> <span style="color: Blue; text-decoration: underline;">
                                                                            10</span> </a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxe:panelcontent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>
                    </div>
                </td>
                <td valign="top" id="TdFilter">
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtsegselected" runat="server" Width="128px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox><asp:HiddenField
                                    ID="txtsegselected_hidden" runat="server" />
                            </td>
                            <td id="TdFilter1" style="height: 23px">
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Width="85px" Font-Size="11px"
                                    Enabled="false">
                                    <asp:ListItem>Clients</asp:ListItem>
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                </asp:DropDownList>
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a></td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" id="TdSelect">
                        <tr>
                            <td>
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
                        <tr>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td style="display: none">
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="txtHeader_hidden" runat="server" />
                    <asp:HiddenField ID="txtFooter_hidden" runat="server" />
                    <asp:HiddenField ID="txtSignature_hidden" runat="server" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_SegmentName" runat="server" />
                    <asp:HiddenField ID="HiddenFieldtest" runat="server" />
                    <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged"
                        BackColor="#DDECFE" BorderStyle="None" />
                    <asp:Button ID="BtnForExportEvent1" runat="server" OnClick="cmbExport1_SelectedIndexChanged"
                        BackColor="#DDECFE" BorderStyle="None" />
                    <asp:Button ID="BtnForExportEvent2" runat="server" OnClick="cmbExport2_SelectedIndexChanged"
                        BackColor="#DDECFE" BorderStyle="None" />
                    <asp:Button ID="BtnForExportEvent3" runat="server" OnClick="cmbExport3_SelectedIndexChanged"
                        BackColor="#DDECFE" BorderStyle="None" />
                </td>
                <td>
                    <span id="spanall">
                        <dxe:ASPxCallbackPanel ID="Cexcelexportpanel" runat="server" ClientInstanceName="cCbptxtSelectionID"
                            OnCallback="Cexcelexportpanel_Callback" Width="206px">
                            <PanelCollection>
                                <dxe:panelcontent runat="server">
                                    </dxe:panelcontent>
                            </PanelCollection>
                            <ClientSideEvents EndCallback="CbptxtSelectionID_EndCallBack" />
                        </dxe:ASPxCallbackPanel>
                    </span>
                </td>
            </tr>
        </table>
        <dxe:ASPxPopupControl ID="PopUp_ScripAlert" runat="server" ClientInstanceName="cPopUp_ScripAlert"
            Width="340px" HeaderText="ECN Detail Information" PopupHorizontalAlign="Center"
            PopupVerticalAlign="Middle" CloseAction="None" Modal="True" ShowCloseButton="False">
            <ContentCollection>
                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">

                    <dxe:ASPxCallbackPanel ID="CbpSuggestISIN" runat="server" ClientInstanceName="cCbpSuggestISIN"
                        BackColor="White" OnCallback="CbpSuggestISIN_Callback" LoadingPanelText="Sending....Please Wait !!"
                        LoadingPanelStyle-Font-Bold="true" LoadingPanelStyle-Cursor="wait" LoadingPanelStyle-ForeColor="gray"
                        LoadingPanelImage-Url='../images/Animated_Email.gif'>
                        <ClientSideEvents EndCallback="CbpSuggestISIN_EndCallBack" />
                        <PanelCollection>
                            <dxe:panelcontent runat="server">
                                    
                                            <div id="div_fail" style="color: Red; font-weight: bold; font-size: 12px;">
                                                An Internal Error Occured during sending some ECNs.Please send Remaing.
                                            </div>
                                            <div id="div_success" style="color: Green; font-weight: bold; font-size: 12px;">
                                                Successfully send ECNs.
                                            </div>
                                            <br />
                                            <br />
                                        
                                        <div style="font-weight: bold; color: black; background-color: gainsboro; border-right: silver thin solid;
                                            border-top: silver thin solid; border-left: silver thin solid; border-bottom: silver thin solid;">
                                            No of ECN Sent : <b style="text-align: right" id="B_allcontractpop" runat="server"></b>
                                            <br />
                                            <br />
                                            Remaining ECN (To Be Sent) : <b style="text-align: right" id="B_ecnenablepop" runat="server">
                                            </b>
                                        </div>
                                        <br />
                                        <br />
                                        <div class="frmleftCont" id="btnall">
                                            <dxe:ASPxButton ID="btnsendall" runat="server" AutoPostBack="False" Text="Send All">
                                                <clientsideevents click="function (s, e) {btnsendall_Click();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                        <div class="frmleftCont" id="btnremain">
                                            <dxe:ASPxButton ID="btnsendremaining" runat="server" AutoPostBack="False" Text="Send Remaining">
                                                <clientsideevents click="function (s, e) {btnsendremaining_Click();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                        <div class="frmleftCont" id="btnokdiv">
                                            <dxe:ASPxButton ID="btnok" runat="server" AutoPostBack="False" Text="Close">
                                                <clientsideevents click="function (s, e) {btnok_Click();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                        <div class="frmleftCont" id="cancel">
                                            <dxe:ASPxButton ID="btncancel" runat="server" AutoPostBack="False" Text="Cancel">
                                                <clientsideevents click="function (s, e) {btncancel_Click();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                    
                                </dxe:panelcontent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
</asp:Content>
