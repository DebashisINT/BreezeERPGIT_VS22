<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_ContractNote_CombinedSegment" CodeBehind="ContractNote_CombinedSegment.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function fn_GoToMain() {
            window.location = "../Reports/ContractNote.aspx";
        }
        function PageLoad()///Call Into Page Load
        {
            HideShow('C1_Row2_Col4', 'H');
            HideShow('C1_Row2_Col5', 'H');
            HideShow('C1_Row4_Col3', 'H');
            HideShow('Container2', 'H');
            HideShow('Row1', 'H');
            HideShow('C3_Row0', 'S');
            HideShow('div_location', 'H');
            HideShow('div_success', 'H');
            HideShow('div_fail', 'H');
            HideShow('DivDosPrntGrid', 'H');
            HideShow('divFilter', 'H');
            HideShow('C3_GeneratPDF', 'H');
            HideShow('C1_Row7', 'H');
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }

        function Fn_printAll() {
            HideShow("C1_Row4_Col3", 'H');
            HideShow('C3_Row1', 'S');
            HideShow('C3_Row0', 'S');

            cBtnupdate.SetEnabled(false);
            cbtnPrintAll.SetEnabled(false);
            cbtnPrintAll.SetText('Please Wait !!');
            cCbpExportPanel.PerformCallback(cCmbRptType.GetValue().toUpperCase() + '~');
        }

        function fn_show() {

            HideShow("C1_Row4_Col3", 'H');
            HideShow('C3_Row1', 'S');
            HideShow('C3_Row0', 'S');


            if (cCmbRptType.GetValue().toUpperCase() == 'DOS') {
                cBtnupdate.SetEnabled(false);
                cBtnupdate.SetText('Please Wait !!');
                cbtnPrintAll.SetEnabled(false);
                //HideShow('td_image','S');
                //document.getElementById('img_id').src="/assests/images/contractimages.gif";
                //document.getElementById('S1').innerHTML="Please Wait..";
                cGvDOSPrintView.PerformCallback("Bind~");
            }
            if (cCmbRptType.GetValue().toUpperCase() == 'PDF') {
                cBtnupdate.SetEnabled(false);
                cBtnupdate.SetText('Please Wait !!');
                cCbpExportPanel.PerformCallback(cCmbRptType.GetValue().toUpperCase() + '~');
            }
            if (cCmbRptType.GetValue().toUpperCase() == 'ECN') {
                cBtnupdate.SetEnabled(false);
                cBtnupdate.SetText('Please Wait !!');
                cCbpExportPanel.PerformCallback(cCmbRptType.GetValue().toUpperCase() + '~');
            }

        }
        function fn_ReportType(obj)//FnddlGeneration(obj)
        {
            if (obj == "PDF") {
                HideShow('C1_Row4', 'S');
                GetObjectID('txtEmpName').value = "";
                GetObjectID('txtEmpName_hidden').value = "";
                GetObjectID('chkSignature').checked = false;
                HideShow('C1_Row4_Col3', 'H');
                HideShow('div_location', 'H');
                HideShow('div_forchkbox', 'S');
                HideShow('div_BothSidePrnt', 'S');
                HideShow('div_ShowTotlBrkg', 'S');
                HideShow('div_ShowBrnchName', 'S');
                HideShow('div_AverageType', 'S');
                HideShow('C1_Row7', 'H');
                HideShow('Div_RptOf', 'S');
                cBtnupdate.SetText('Generate');
                //               HideShow('td_image','H');          
            }
            if (obj == "ECN") {
                HideShow('C1_Row4', 'H');
                GetObjectID('txtEmpName').value = "";
                GetObjectID('txtEmpName_hidden').value = "";
                GetObjectID('chkSignature').checked = false;
                HideShow('C1_Row4_Col3', 'H');
                HideShow('div_location', 'H');
                HideShow('div_forchkbox', 'H');
                HideShow('div_BothSidePrnt', 'H');
                HideShow('div_ShowTotlBrkg', 'H');
                HideShow('div_ShowBrnchName', 'H');
                HideShow('div_AverageType', 'H');
                HideShow('C1_Row7', 'H');
                HideShow('Div_RptOf', 'H');
                cBtnupdate.SetText('Generate');
                //                HideShow('td_image','H');        
            }
            if (obj == "DOS") {
                HideShow('C1_Row4', 'H');
                GetObjectID('txtEmpName').value = "";
                GetObjectID('txtEmpName_hidden').value = "";
                GetObjectID('chkSignature').checked = false;
                HideShow('C1_Row4_Col3', 'H');
                HideShow('div_location', 'S');
                HideShow('div_forchkbox', 'S');
                HideShow('div_BothSidePrnt', 'H');
                HideShow('div_ShowTotlBrkg', 'H');
                HideShow('div_ShowBrnchName', 'H');
                HideShow('div_AverageType', 'S');
                HideShow('C1_Row7', 'S');
                HideShow('Div_RptOf', 'H');
                //HideShow('td_image','H');  
                cBtnupdate.SetText('View Records');
            }

        }
        function ajaxFunction() {


            var aa = "";
            var path = "";
            path = document.getElementById('hdnpath').value;
            var xmlhttp;
            if (window.XMLHttpRequest) {

                xmlhttp = new XMLHttpRequest();
            }
            else {

                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");

            }
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4) {
                    var a = xmlhttp.responseText;
                    if (a != "") {

                        CreateFile(a);
                    }
                }
            }
            xmlhttp.open("GET", "Defaultnew.aspx?path=" + path + "&reportformat=combined", true);
            xmlhttp.send(null);

        }
        function CreateFile(value) {
            var rand_no = Math.random();
            var now = new Date();
            var then = now.getFullYear() + '_' + (now.getMonth() + 1) + '_' + now.getDate();
            then += '_' + now.getHours() + '_' + now.getMinutes() + '_' + now.getSeconds();
            var destination = document.getElementById('hdnLocationPath').value;
            var fso;
            try {
                fso = new ActiveXObject("Scripting.FileSystemObject");
            }
            catch (err) {
                //HideShow('td_image','H');
                PageLoad();
                cBtnupdate.SetEnabled(true);
                cBtnupdate.SetText('View Records');
                //HideShow('div_location','S');

                HideShow('C1_Row4', 'H');
                GetObjectID('txtEmpName').value = "";
                GetObjectID('txtEmpName_hidden').value = "";
                GetObjectID('chkSignature').checked = false;
                HideShow('C1_Row4_Col3', 'H');
                HideShow('div_location', 'S');
                alert("Please Enable The option\n'Initialize and Script Active X controls not marked as safe for scripting' \n Under 'Active X Controls & Piug-Ins' \n from Internet options -> Security Settings");

            }
            varFileObject = fso.OpenTextFile(destination, 2, true, 0);
            varFileObject.write(value);
            varFileObject.close();
            //HideShow('td_image','H');
            PageLoad();
            cBtnupdate.SetEnabled(true);
            cbtnPrintAll.SetEnabled(true);
            HideShow('C1_Row7', 'S');
            cBtnupdate.SetText('View Records');
            cbtnPrintAll.SetText('Print All');
            HideShow('C1_Row4', 'H');
            GetObjectID('txtEmpName').value = "";
            GetObjectID('txtEmpName_hidden').value = "";
            GetObjectID('chkSignature').checked = false;
            HideShow('C1_Row4_Col3', 'H');
            HideShow('div_location', 'S');
            alert("Print Send To Printer");

        }
        function fn_GroupBy(obj) {
            GetObjectID('<%=lstSelection.ClientID%>').length = 0;
            if (obj == "C") {
                cRblClient.SetSelectedIndex(0);
                fn_Client('A');
                HideShow('C1_Row2_Col4', 'H');
                HideShow('C1_Row2_Col5', 'H');
                HideShow('C1_Row2_Col3', 'S');
                CallServer("CallAjax-Client", "");
            }
            if (obj == "B") {
                cRblBranch.SetSelectedIndex(0);
                fn_Branch('A');
                HideShow('C1_Row2_Col3', 'H');
                HideShow('C1_Row2_Col5', 'H');
                HideShow('C1_Row2_Col4', 'S');
                CallServer("CallAjax-Branch", "");
            }
            if (obj == "G") {
                HideShow('C1_Row2_Col3', 'H');
                HideShow('C1_Row2_Col4', 'H');
                HideShow('Container2', 'H');
                HideShow('C1_Row2_Col5', 'S');
                HideShow('C1_Row2_Col6', 'H');
                cCmbGroupType.PerformCallback("GroupType~");
            }
        }


        function fn_Client(obj) {
            if (obj == "A") {
                SetValue('HiddenField_ClientBranchGroup', '');
                GetObjectID('<%=lstSelection.ClientID%>').length = 0;
                HideShow('Container2', 'H');
                HideShow('C1_Row6', 'S');
                CallServer("CallAjax-Client", "");
            }
            else if ((obj == "S") || (obj == "ABS") || (obj == "D")) {
                if (GetObjectID('Container2').style.display == "inline") {
                    cRblClient.SetSelectedIndex(0);
                    lnkBtnAddFinalSelection();
                }
                else {
                    HideShow('Container2', 'S');
                    HideShow('C1_Row6', 'H');
                    CallServer("CallAjax-Client", "");
                    //GetObjectID('txtSelectionID').focus();
                }
            }
        }
        function fn_Branch(obj) {
            if (obj == "A") {
                SetValue('HiddenField_ClientBranchGroup', '');
                GetObjectID('<%=lstSelection.ClientID%>').length = 0;
                HideShow('Container2', 'H');
                HideShow('C1_Row6', 'S');
            }
            else if ((obj == "S") || (obj == "ABS") || (obj == "D")) {
                if (GetObjectID('Container2').style.display == "inline") {
                    cRblBranch.SetSelectedIndex(0);
                    lnkBtnAddFinalSelection();
                }
                else {
                    HideShow('Container2', 'S');
                    HideShow('C1_Row6', 'H');
                    CallServer("CallAjax-Branch", "");
                    //GetObjectID('txtSelectionID').focus();
                }
            }
        }
        function CmbGroupType_EndCallback() {
            if (cCmbGroupType.cpBindGroupType != undefined) {
                if (cCmbGroupType.cpBindGroupType == "Y") {
                    cCmbGroupType.SetSelectedIndex(0);
                    SetValue('HiddenField_ClientBranchGroup', '');
                }
                else if (cCmbGroupType.cpBindGroupType == "N") {
                    cCmbGroupType.SetEnabled(false);
                }
            }
            Height('500', '500');
        }
        function fn_CmbGroupType(obj) {
            if (obj == "0") {
                HideShow('C1_Row2_Col6', 'H');
                alert('Please Select Group Type !');
                cbtnShow.SetEnabled(false);
            }
            else {
                cRblGroup.SetSelectedIndex(0);
                HideShow('C1_Row2_Col6', 'S');
                cbtnShow.SetEnabled(true);
            }
            Height('500', '500');
        }
        function fn_Group(obj) {
            if (obj == "A") {
                SetValue('HiddenField_ClientBranchGroup', '');
                GetObjectID('<%=lstSelection.ClientID%>').length = 0;
                HideShow('Container2', 'H');
                HideShow('C1_Row6', 'S');
            }
            else if ((obj == "S") || (obj == "ABS") || (obj == "D")) {
                if (GetObjectID('Container2').style.display == "inline") {
                    cRblGroup.SetSelectedIndex(0);
                    lnkBtnAddFinalSelection();
                }
                else {
                    HideShow('Container2', 'S');
                    HideShow('C1_Row6', 'H');
                    CallServer("CallAjax-Group~" + cCmbGroupType.GetText(), "");
                    //GetObjectID('txtSelectionID').focus();
                }
            }
        }
        function NORECORD() {
            alert('No Record Found !!!');
            Reset();
        }
        function ErrorMsg(msg) {
            if (msg == "ClientErr")
                alert("There is No Proper Client Selection!!!");
            else if (msg == "BranchErr")
                alert("There is No Proper Branch Selection!!!");
            else if (msg == "GroupErr")
                alert("There is No Proper Group Selection!!!");
            else
                alert("No of Period Generated : " + msg + " .");
        }
        function Reset() {
            cCmbGroupBy.SetSelectedIndex(0);
            cCmbGroupBy.SetEnabled(true);
            cRblClient.SetSelectedIndex(0);
            cRblClient.SetEnabled(true);
            cRblBranch.SetSelectedIndex(0);
            cRblBranch.SetEnabled(false);
            cRblGroup.SetSelectedIndex(0);
            cRblGroup.SetEnabled(false);
            cCmbGroupType.SetSelectedIndex(0);
            cCmbGroupType.SetEnabled(false);
            GetObjectID('<%=lstSelection.ClientID%>').length = 0;
            SetValue('HiddenField_ClientBranchGroup', '');
            Height('500', '500');
        }
        function cCbpExportPanel_EndCallBack() {
            btnopenpopup.SetEnabled(true);
            if (cCbpExportPanel.cppdfclick != null) {
                if (cCbpExportPanel.cppdfclick == "Success") {
                    cBtnupdate.SetEnabled(true);
                    cBtnupdate.SetText('Generate');
                    document.getElementById('btnPdfExport').click()
                }

            }
            if (cCbpExportPanel.cpdosprint != null) {
                if (cCbpExportPanel.cpdosprint == "click") {
                    document.getElementById('btndosprint').click()
                }

            }
            if ((cCbpExportPanel.cpallcontract != null) && (cCbpExportPanel.cpecnenable != null)) {

                var allcontract = cCbpExportPanel.cpallcontract;
                var ecnenable = cCbpExportPanel.cpecnenable;
                document.getElementById('<%=B_allcontract.ClientID %>').innerHTML = cCbpExportPanel.cpallcontract;
                document.getElementById('<%=B_ecnenable.ClientID %>').innerHTML = cCbpExportPanel.cpecnenable;
                document.getElementById('<%=B_deliveryrpt.ClientID %>').innerHTML = cCbpExportPanel.cpdeliveryrpt;
                //alert(allcontract);
                //alert(ecnenable);
                //                 if (allcontract=='0' || ecnenable=='0') 
                //                    {
                // alert('1')
                HideShow('Row1', 'S');
                if (cCbpExportPanel.cpsign != null) {
                    if ((cCbpExportPanel.cpsign == 'false') || (ecnenable == '0')) {
                        btnopenpopup.SetEnabled(false);
                    }
                    else {
                        btnopenpopup.SetEnabled(true);
                    }
                }
                //                    }
                //                else
                //                    {
                //                        //alert('2')
                //                        HideShow('Row1','H');
                //                    }
                cBtnupdate.SetEnabled(true);
                cBtnupdate.SetText('Generate');

            }
            if ((cCbpExportPanel.cpallcontractpop != null) && (cCbpExportPanel.cpecnenablepop != null)) {
                document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML = cCbpExportPanel.cpallcontractpop;
                document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML = cCbpExportPanel.cpecnenablepop;
                var remn = document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML = cCbpExportPanel.cpallcontractpop;
                var remn1 = document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML = cCbpExportPanel.cpecnenablepop;

                if (remn == '0' || remn1 == '0') {
                    //alert('1sttt');
                    Hide('btnremain');
                }
                else {
                    //alert('2nd');
                    Show('btnremain');
                }
                // alert('11');
                Hide('btnokdiv');
                Hide('div_fail');
                Hide('div_success');
                cPopUp_ScripAlert.Show();

            }
        }
        function btnsendall_Click() {
            cCbpSuggestISIN.PerformCallback('all');
        }
        function btnsendremaining_Click() {
            cCbpSuggestISIN.PerformCallback('remaining');
        }
        function btnok_Click() {
            window.location = '../reports/ContractNote.aspx';
        }
        function btncancel_Click() {

            cPopUp_ScripAlert.Hide();

        }
        function CbpSuggestISIN_EndCallBack() {
            //        alert(cCbpSuggestISIN.cpdownloadcomplete11);
            //        alert(cCbpSuggestISIN.cpdownloadcomplete12);
            if (cCbpSuggestISIN.cpallsendtclick != null) {
                document.getElementById('btnecn').click()
            }

            if (cCbpSuggestISIN.cpdownloadcomplete != null) {

                if (cCbpSuggestISIN.cpdownloadcomplete = 'yes') {
                    Hide('td_image');
                    document.getElementById('BtnForExportEvent3').click();
                }
            }

            if (cCbpSuggestISIN.cpsuccessandfailmsg != null) {
                if (cCbpSuggestISIN.cpsuccessandfailmsg == 'totalsuccess') {

                    alert('All Mail Send Successfully');
                    FnddlGeneration('4');


                }
                if (cCbpSuggestISIN.cpsuccessandfailmsg == 'totalfail') {

                    alert('Official Emailid not found for all sending mail');
                    FnddlGeneration('4');


                }
                if (cCbpSuggestISIN.cpsuccessandfailmsg == 'fewsuccessandfewfail') {

                    alert('Some Official Emailid not found \n Rest emails has been Sent.');
                    FnddlGeneration('4');


                }
            }

            if (cCbpSuggestISIN.cpnorecord != null) {
                if (cCbpSuggestISIN.cpnorecord = 'norecord') {

                    alert('No Record Found.');
                    FnddlGeneration('4');


                }
            }

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
           if (cCbpSuggestISIN.cpNoPDFGenerated != null) {
               alert("No of PDF Successfully Generated : " + cCbpSuggestISIN.cpNoPDFGenerated);
               cCbpSuggestISIN.cpNoPDFGenerated = null;
               cBtnGeneratePdf.SetEnabled(true);
               cBtnGeneratePdf.SetText("Click To Generate");

           }

       }

       function ASPxCBP_PDFGenerate_EndCallBack() {
           alert('its Called');
       }
       function ShowHideFilter(obj) {

           if (obj == 'v1') {
               if (document.getElementById('<%=B_allcontract.ClientID %>').innerHTML == '0')
                    alert('You dont have any record to export')
                else
                    document.getElementById('BtnForExportEvent').click()
            }
            if (obj == 'v2') {
                if (document.getElementById('<%=B_ecnenable.ClientID %>').innerHTML == '0')
                alert('You dont have any record to export')
            else
                document.getElementById('BtnForExportEvent1').click()
        }
        if (obj == 'v3') {
            if (document.getElementById('<%=B_deliveryrpt.ClientID %>').innerHTML == '0')
                    alert('You dont have any record to export')
                else
                    document.getElementById('BtnForExportEvent2').click()
            }
            //cCbptxtSelectionID.PerformCallback(obj);

        }
        function fn_showpopup() {


            if (document.getElementById('txtdigitalName_hidden').value != "") {

                cCbpExportPanel.PerformCallback('showpopup');
            }
            else {
                alert('Please select Signature !!');
            }

        }
        function fn_GenearatePDF() {
            cBtnGeneratePdf.SetEnabled(false);
            cBtnGeneratePdf.SetText("Please Wait..");
            cCbpSuggestISIN.PerformCallback('GeneratePDF');
        }
        function ChkSignature() {
            var chkbox = GetObjectID('chkSignature');
            if (chkbox.checked) {
                HideShow('C1_Row4_Col3', 'S');
                GetObjectID('txtEmpName').focus();
            }
            else {
                HideShow('C1_Row4_Col3', 'H');
                GetObjectID('txtEmpName_hidden').value = "";
                GetObjectID('txtEmpName').value = "";
            }
        }
        function DateChange(positionDate) {
            var FYS = '<%=Session["FinYearStart"]%>';
            var FYE = '<%=Session["FinYearEnd"]%>';
            var LFY = '<%=Session["LastFinYear"]%>';
            DevE_CheckForFinYear(positionDate, FYS, FYE, LFY);
        }
        function GvDOSPrintView_EndCallBack() {
            if (cGvDOSPrintView.cpprint == "click") {
                document.getElementById('btndosprint').click();
            }
            HideShow('DivDosPrntGrid', 'S');
            cBtnupdate.SetEnabled(true);
            cBtnupdate.SetText('View Records');
            HideShow('Row0', 'H');
            HideShow('divFilter', 'S');
            Height('500', '500');
        }
        function OnGridFocusedRowChanged() {
            cGvDOSPrintView.GetSelectedFieldValues('UID', OnGetRowValues);
        }
        function OnGetRowValues(values) {
            RowID = 'n';
            for (var j = 0; j < values.length; j++) {
                if (RowID != 'n')
                    RowID += ',' + values[j];
                else

                    RowID = values[j];

            }
        }
        function SelectAll(objCheckAll) {
            var inputs = document.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    //alert('123');
                    //               if(inputs[i].className=='dxtl__Sel')
                    //                    {
                    if (objCheckAll.checked == true) {
                        if (inputs[i].checked == false)
                            inputs[i].click();
                    }
                    else
                        if (inputs[i].checked == true)
                            inputs[i].click();
                    //                    }

                }

            }
        }
        function OnAllCheckedChanged(s, e) {
            if (cchkExclude.GetChecked() == false) {
                if (s.GetChecked())
                    cGvDOSPrintView.SelectRows();
                else
                    cGvDOSPrintView.UnselectRows();
            }
        }
        function OnSelectedCheckedChanged(s, e) {
            var inputs = document.getElementsByTagName("input");

            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (s.GetChecked()) {
                        inputs[i].click();
                    }
                    else
                        if (inputs[i].checked == true)
                            inputs[i].click();
                }
            }
            //          if (s.GetChecked())
            //            cGvDOSPrintView.UnselectRows();
            //          else
            //            cGvDOSPrintView.SelectRows();
        }
        function SelectExclude(objCheckAllExc) {
            var inputs = document.getElementsByTagName("input");
            //alert(inputs.length);
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    //alert('456');
                    //                       if(inputs[i].className=='dxtl__Sel')
                    //                            {
                    if (objCheckAllExc.checked == true) {
                        inputs[i].click();
                    }
                    else
                        if (inputs[i].checked == true)
                            inputs[i].click();
                    //                            }

                }

            }


        }
        function fn_Filter() {
            HideShow('Row0', 'S');
            HideShow('divFilter', 'H');
        }
        function fn_DosPrnt() {
            cbtnDosPrnt1.SetEnabled(false);
            cbtnDosPrnt1.SetText('Please Wait..!!');
            cbtnDosPrnt2.SetEnabled(false);
            cbtnDosPrnt2.SetText('Please Wait..!!');
            if (RowID == undefined || RowID == 'n') {
                alert('Please Select Atleast one Item !!..');
                cbtnDosPrnt1.SetEnabled(true);
                cbtnDosPrnt1.SetText('Print');
                cbtnDosPrnt2.SetEnabled(true);
                cbtnDosPrnt2.SetText('Print');
            }
            else
                cGvDOSPrintView.PerformCallback("Print~");
            //cCbpExportPanel.PerformCallback(cCmbRptType.GetValue().toUpperCase()+'~');  
        }
    </script>

    <!-- CallAjax and Receive Server Script-->

    <script language="javascript" type="text/javascript">
        FieldName = 'none';
        function btnAddToList_click() {
            var txtName = GetObjectID('txtSelectionID');
            if (txtName != '') {
                var txtId = GetValue('txtSelectionID_hidden');
                var listBox = GetObjectID('lstSelection');
                var listLength = listBox.length;
                var opt = new Option();
                opt.value = txtId;
                opt.text = txtName.value;
                listBox[listLength] = opt;
                txtName.value = '';
            }
            else
                alert('Please Search Name And Then Add!');
            txtName.focus();
            txtName.select();
        }
        function lnkBtnAddFinalSelection() {
            var listBox = GetObjectID('lstSelection');
            var listID = '';
            var i;
            if (listBox.length > 0) {
                for (i = 0; i < listBox.length; i++) {
                    if (listID == '')
                        listID = listBox.options[i].value + '!' + listBox.options[i].text;
                    else
                        listID += '^' + listBox.options[i].value + '!' + listBox.options[i].text;
                }
                CallServer(listID, "");
                var j;
                for (j = listBox.options.length - 1; j >= 0; j--) {
                    listBox.remove(j);
                }
                HideShow('Container2', 'H');
                HideShow('C1_Row6', 'S');
            }
            else if ((GetObjectID('Container2').style.display == "inline") && (listBox.length == 0)) {
                if ((cCmbGroupBy.GetSelectedIndex() == 0) && (cRblClient.GetSelectedIndex() == 1)) {
                    alert("Please Select Atleast One Client Item!!!");
                }
                else if ((cCmbGroupBy.GetSelectedIndex() == 1) && (cRblBranch.GetSelectedIndex() == 1)) {
                    alert("Please Select Atleast One Branch Item!!!");
                }
                else if ((cCmbGroupBy.GetSelectedIndex() == 2) && (cRblGroup.GetSelectedIndex() == 1)) {
                    alert("Please Select Atleast One Group Item!!!");
                }
            }
        }
        function lnkBtnRemoveFromSelection() {
            var listBox = GetObjectID('lstSelection');
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
        function string_contains(containerString, matchBySubString) {
            if (containerString.indexOf(matchBySubString) == -1) {
                return false;
            }
            else {
                return true;
            }
        }
        function ReceiveServerData(rValue) {
            var Data = rValue.split('@');
            if (Data[1] != "undefined") {
                if ((Data[0] == 'Branch') || (Data[0] == 'Group') || (Data[0] == 'Client'))
                    SetValue('HiddenField_ClientBranchGroup', Data[1]);
            }
            if (Data[0] == 'AjaxQuery') {
                AjaxComQuery = Data[1];
                var AjaxList_TextBox = GetObjectID('txtSelectionID');
                AjaxList_TextBox.value = '';
                if (window.addEventListener) {
                    AjaxList_TextBox.addEventListener('keyup', CallGenericAjaxJS);
                }
                else if (window.attachEvent) {
                    AjaxList_TextBox.attachEvent('onkeyup', CallGenericAjaxJS);
                }
                //                AjaxList_TextBox.attachEvent('onkeyup',CallGenericAjaxJS);
            }
        }
        function CallGenericAjaxJS(e) {

            var AjaxList_TextBox = GetObjectID('txtSelectionID');
            AjaxList_TextBox.focus();
            AjaxComQuery = AjaxComQuery.replace("\'", "'");
            //var GenericAjaxListAspxPath = '../CentralData/Pages/GenericAjaxList.aspx';            
            ajax_showOptions(AjaxList_TextBox, 'GenericAjaxList', e, replaceChars(AjaxComQuery), 'Main');
        }
        function CallAjax(obj1, obj2, obj3, Query) {
            var CombinedQuery = new String(Query);
            //var GenericAjaxListAspxPath = '../CentralData/Pages/GenericAjaxList.aspx';
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
        }
        function keyVal(obj) {
            var WhichCall = obj.split("~")[4];
            if (WhichCall == "DIGISIGN") {
                var isEtoken = obj.split("~")[2];
                if (isEtoken == "E") {
                    HideShow("C3_button", "H");
                    HideShow("C3_GeneratPDF", "S");
                }
                else {
                    HideShow("C3_button", "S");
                    HideShow("C3_GeneratPDF", "H");
                }
            }
        }


    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="MainFull">
        <div id="Header" class="Header">
            <div id="divReset" style="font-size: 7; text-align: right; margin-right: 100px;">
                <dxe:ASPxButton ID="btnGoToMain" runat="server" AutoPostBack="False" ClientInstanceName="cGoToMain"
                    Text="Go To Main Selection" Font-Size="7" TabIndex="0" CssClass="btnRight">
                    <ClientSideEvents Click="function(s,e){fn_GoToMain();}" />
                    <Paddings Padding="0" PaddingBottom="0" PaddingLeft="0" PaddingRight="0" />
                </dxe:ASPxButton>
            </div>
            <div id="divFilter" style="font-size: 7; text-align: right; margin-right: 100px;">
                <dxe:ASPxButton ID="btnFilter" runat="server" AutoPostBack="False" ClientInstanceName="cbtnFilter"
                    Text="Filter" Font-Size="7" ForeColor="blue" TabIndex="0" CssClass="btnRight">
                    <ClientSideEvents Click="function(s,e){fn_Filter();}" />
                    <Paddings Padding="0" PaddingBottom="0" PaddingLeft="0" PaddingRight="0" />
                </dxe:ASPxButton>
            </div>
            ContractNote CombinedSegment <span class="clear"></span>
        </div>
        <div id="Row0" class="Row">
            <div id="Container1" class="container">
                <div id="C1_Row1" class="Row">
                    <div id="C1_Row1_Col1" class="LFloat_Lable LableWidth">
                        <asp:Label ID="lblRptType" runat="server" Text="Report Gen Type : "></asp:Label>
                    </div>
                    <div id="C1_Row1_Col2" class="LFloat_Content">
                        <dxe:ASPxComboBox ID="CmbRptType" runat="server" ValueType="System.String" ClientInstanceName="cCmbRptType"
                            SelectedIndex="0" Width="160px" TabIndex="0">
                            <Items>
                                <dxe:ListEditItem Text="PDF" Value="PDF"></dxe:ListEditItem>
                                <dxe:ListEditItem Text="ECN" Value="ECN"></dxe:ListEditItem>
                                <dxe:ListEditItem Text="DosPrint(Preprinted)" Value="DOS"></dxe:ListEditItem>
                            </Items>
                            <ClientSideEvents SelectedIndexChanged="function(s, e) {fn_ReportType(s.GetValue());}" />
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <span class="clear"></span>
                <div id="C1_Row11" class="Row">
                    <div id="C1_Row11_Col1" class="LFloat_Lable LableWidth">
                        <asp:Label ID="Label3" runat="server" Text="Segment : "></asp:Label>
                    </div>
                    <div id="C1_Row11_Col2" class="LFloat_Content">
                        <dxe:ASPxRadioButtonList ID="RbSegment" runat="server" SelectedIndex="0" ItemSpacing="20px"
                            Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                            ClientInstanceName="cRblBranch" Width="160px">
                            <Border BorderWidth="0px" />
                            <Paddings PaddingTop="1px"></Paddings>
                            <ClientSideEvents ValueChanged="function(s, e) {fn_Branch(s.GetValue());}" />
                            <Items>
                                <dxe:ListEditItem Text="Current Segment" Value="True"></dxe:ListEditItem>
                                <dxe:ListEditItem Text="All" Value="False"></dxe:ListEditItem>
                            </Items>
                        </dxe:ASPxRadioButtonList>
                    </div>
                </div>
                <span class="clear"></span>
                <div id="C1_Row2" class="Row">
                    <div id="C1_Row2_Col1" class="LFloat_Lable LableWidth">
                        <asp:Label ID="lblGroupBy" runat="server" Text="Group By : "></asp:Label>
                    </div>
                    <div class="left">
                        <div id="C1_Row2_Col2" class="LFloat_Content ContentWidth">
                            <dxe:ASPxComboBox ID="CmbGroupBy" runat="server" ValueType="System.String" ClientInstanceName="cCmbGroupBy"
                                SelectedIndex="0" TabIndex="0" Width="125px">
                                <Items>
                                    <dxe:ListEditItem Text="Client" Value="C"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="Branch" Value="B"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="Group" Value="G"></dxe:ListEditItem>
                                </Items>
                                <ClientSideEvents SelectedIndexChanged="function(s, e) {fn_GroupBy(s.GetValue());}" />
                            </dxe:ASPxComboBox>
                        </div>
                        <div class="left">
                            <div>
                                <div id="C1_Row2_Col3" class="LFloat_Content ABS_ContentWidth">
                                    <dxe:ASPxRadioButtonList ID="RblClient" runat="server" SelectedIndex="0" ItemSpacing="20px"
                                        Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                        ClientInstanceName="cRblClient">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Selected" Value="S" />
                                            <dxe:ListEditItem Text="AllButSelected" Value="D" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_Client(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>
                            <div>
                                <div id="C1_Row2_Col4" class="LFloat_Content ABS_ContentWidth">
                                    <dxe:ASPxRadioButtonList ID="RblBranch" runat="server" SelectedIndex="0" ItemSpacing="20px"
                                        Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                        ClientInstanceName="cRblBranch">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Selected" Value="S" />
                                            <dxe:ListEditItem Text="AllButSelected" Value="D" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_Branch(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                                <span class="clear"></span>
                            </div>
                            <div id="C1_Row2_Col5">
                                <div class="LFloat_Content ContentWidth">
                                    <dxe:ASPxComboBox ID="CmbGroupType" ClientInstanceName="cCmbGroupType" runat="server"
                                        Font-Size="11px" TabIndex="0" Width="125px" OnCallback="CmbGroupType_Callback">
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_CmbGroupType(s.GetValue());}"
                                            EndCallback="CmbGroupType_EndCallback" />
                                    </dxe:ASPxComboBox>
                                </div>
                                <div id="C1_Row2_Col6" class="LFloat_Content ABS_ContentWidth" style="display: none; margin-top: 3px;">
                                    <dxe:ASPxRadioButtonList ID="RblGroup" runat="server" SelectedIndex="0" ItemSpacing="20px"
                                        Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                        ClientInstanceName="cRblGroup">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Selected" Value="S" />
                                            <dxe:ListEditItem Text="AllButSelected" Value="D" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_Group(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                                <span class="clear"></span>
                            </div>
                            <span class="clear"></span>
                        </div>
                        <span class="clear"></span>
                    </div>
                    <span class="clear"></span>
                </div>
                <span class="clear"></span>
                <div id="C1_Row3" class="Row">
                    <div id="C1_Row3_Col1" class="LFloat_Lable LableWidth">
                        <asp:Label ID="lblDate" runat="server" Text="Date : "></asp:Label>
                    </div>
                    <div id="C1_Row3_Col2" class="LFloat_Content ContentWidth">
                        <dxe:ASPxDateEdit ID="dtPosition" runat="server" ClientInstanceName="cdtPosition"
                            DateOnError="Today" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                            Width="125px" Font-Size="11px" TabIndex="0">
                            <DropDownButton Text="Select">
                            </DropDownButton>
                            <ClientSideEvents DateChanged="function(s,e){DateChange(cdtPosition);}"></ClientSideEvents>
                        </dxe:ASPxDateEdit>
                    </div>
                    <span class="clear"></span>
                </div>
                <span class="clear"></span>
                <div id="Div_RptOf" class="Row">
                    <div id="Div12" class="LFloat_Lable LableWidth">
                        <asp:Label ID="Label8" runat="server" Text="Report Type : "></asp:Label>
                    </div>
                    <div id="Div13" class="LFloat_Content">
                        <dxe:ASPxComboBox ID="CmbRptOf" runat="server" ValueType="System.String" ClientInstanceName="cCmbRptOf"
                            SelectedIndex="0" Width="160px" TabIndex="0">
                            <Items>
                                <dxe:ListEditItem Text="Contract Note" Value="1"></dxe:ListEditItem>
                                <dxe:ListEditItem Text="Trade Annexure" Value="2"></dxe:ListEditItem>
                            </Items>
                            <%--<clientsideevents selectedindexchanged="function(s, e) {fn_ReportType(s.GetValue());}" />--%>
                        </dxe:ASPxComboBox>
                    </div>
                    <span class="clear"></span>
                </div>
                <span class="clear"></span>
                <div id="C1_Row4" class="Row">
                    <div id="C1_Row4_Col1" class="LFloat_Lable LableWidth">
                        <asp:Label ID="lblAddSignatory" runat="server" Text="Add Signatory : "></asp:Label>
                    </div>
                    <div id="C1_Row4_Col2" class="left">
                        <div class="LFloat_Content">
                            <input id="chkSignature" type="checkbox" onclick="ChkSignature()" runat="server" />
                        </div>
                        <div id="C1_Row4_Col3" class="left">
                            <div class="LFloat_Lable LableWidth">
                                <asp:Label ID="lblEmplist" runat="server" Text="Employee:"></asp:Label>
                            </div>
                            <div id="C1_Row4_Col4" class="LFloat_Content">
                                <asp:TextBox ID="txtEmpName" runat="server" Width="205px"></asp:TextBox>
                            </div>
                            <span class="clear"></span>
                        </div>
                        <span class="clear"></span>
                    </div>
                    <span class="clear"></span>
                </div>
                <span class="clear"></span>
                <div id="div_forchkbox" class="Row">
                    <div id="div2" class="LFloat_Lable LableWidth">
                        <asp:Label ID="Label2" runat="server" Text="Only Contractnote"></asp:Label>
                    </div>
                    <div id="div3" class="LFloat_Content">
                        <asp:CheckBox ID="chkonlybill" runat="server" />
                    </div>
                    <span class="clear"></span>
                </div>
                <span class="clear"></span>
                <div id="div_BothSidePrnt" class="Row">
                    <div id="div4" class="LFloat_Lable LableWidth">
                        <asp:Label ID="Label4" runat="server" Text="Both Side Printing"></asp:Label>
                    </div>
                    <div id="div5" class="LFloat_Content">
                        <asp:CheckBox ID="chkBothSidePrnt" runat="server" />
                    </div>
                    <span class="clear"></span>
                </div>
                <span class="clear"></span>
                <div id="div_ShowTotlBrkg" class="Row">
                    <div id="div6" class="LFloat_Lable LableWidth" style="width: 180px">
                        <asp:Label ID="Label5" runat="server" Text="Dont Show Total Brokerage:"></asp:Label>
                    </div>
                    <div id="div7" class="LFloat_Content">
                        <asp:CheckBox ID="chkTotlBrkg" runat="server" />
                    </div>
                    <span class="clear"></span>
                </div>
                <span class="clear"></span>
                <div id="div_ShowBrnchName" class="Row">
                    <div id="div9" class="LFloat_Lable LableWidth" style="width: 180px">
                        <asp:Label ID="Label6" runat="server" Text="Dont Show BranchName:"></asp:Label>
                    </div>
                    <div id="div10" class="LFloat_Content">
                        <asp:CheckBox ID="chkBrnchName" runat="server" />
                    </div>
                    <span class="clear"></span>
                </div>
                <span class="clear"></span>
                <div id="div_AverageType" class="Row">
                    <div id="div8" class="LFloat_Lable LableWidth" style="width: 290px">
                        <asp:Label ID="Label7" runat="server" Text="Dont Show OrderNo\TradeNo for Avrg Trade:"></asp:Label>
                    </div>
                    <div id="div11" class="LFloat_Content">
                        <asp:CheckBox ID="chkAvgType" runat="server" />
                    </div>
                    <span class="clear"></span>
                </div>
                <br class="clear" />
                <br class="clear" />
                <div id="div_location" class="Row">
                    <div id="div_label" class="LFloat_Lable LableWidth">
                        <asp:Label ID="Label1" runat="server" Text="Location :"></asp:Label>
                    </div>
                    <div id="div_control1" class="LFloat_Content">
                        <asp:DropDownList ID="ddlLocation" Font-Size="12px" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <br class="clear" />
                <br class="clear" />
                <div id="row3_col1pop" class="Info" style="font-size: 14px; width: 250px; line-height: 0.2">
                    Provided Billing is done for the day.
                </div>
                <br class="clear" />
                <div style="float: left; width: 300px;">
                    <div id="C1_Row6" style="float: left">
                        <dxe:ASPxButton ID="btnshow" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                            Height="5px" Text="Generate" Width="101px" ClientInstanceName="cBtnupdate">
                            <ClientSideEvents Click="function (s, e) {fn_show();}" />
                        </dxe:ASPxButton>
                    </div>
                    <div id="C1_Row7" style="float: left; margin-left: 10px">
                        <dxe:ASPxButton ID="btnPrintAll" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                            Height="5px" Text="Print All" Width="110px" ClientInstanceName="cbtnPrintAll">
                            <ClientSideEvents Click="function (s, e) {Fn_printAll();}" />
                        </dxe:ASPxButton>
                    </div>
                </div>
            </div>
            <div id="Container2" class="container">
                <div id="C2_Row0" class="Row">
                    <div id="C2_Row0_Col1" class="LFloat_Content">
                        <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="300px" TabIndex="0"></asp:TextBox>
                    </div>
                    <div id="C2_Row0_Col2" class="LFloat_Lable">
                        <a href="javascript:void(0);" tabindex="0" onclick="btnAddToList_click()"><span class="lnkBtnAjax green">Add to List</span></a>
                    </div>
                </div>
                <div id="C2_Row1" class="Row">
                    <div id="C2_Row1_Col1" class="LFloat_Content finalSelectedBox">
                        <asp:ListBox ID="lstSelection" runat="server" Font-Size="12px" Height="100px" Width="400px"
                            TabIndex="0"></asp:ListBox>
                    </div>
                </div>
                <div id="C2_Row2" class="Row">
                    <div id="C2_Row2_Col1" class="LFloat_Lable">
                        <a href="javascript:void(0);" tabindex="0" onclick="lnkBtnAddFinalSelection()"><span
                            class="lnkBtnAjax blue">Done</span></a>&nbsp;&nbsp; <a href="javascript:void(0);"
                                tabindex="0" onclick="lnkBtnRemoveFromSelection()"><span class="lnkBtnAjax red">Remove</span></a>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div id="DivDosPrntGrid" class="left">
            <div class="left" style="padding-bottom: 10px">
                <dxe:ASPxButton ID="btnDosPrnt1" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                    Height="5px" Text="Print" ToolTip="Print Selected" Width="101px" ClientInstanceName="cbtnDosPrnt1">
                    <ClientSideEvents Click="function (s, e) {fn_DosPrnt();}" />
                </dxe:ASPxButton>
            </div>
            <div class="left" style="padding-bottom: 10px; height: 10px; width: 130px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid; margin-left: 4px;">
                <dxe:ASPxCheckBox ID="chkExclude" runat="server" ClientInstanceName="cchkExclude" Text="Exclude Selected"
                    ToolTip="Select/Deselect all rows">
                    <ClientSideEvents CheckedChanged="OnSelectedCheckedChanged"></ClientSideEvents>
                </dxe:ASPxCheckBox>
                <%--<asp:CheckBox ID="chkExclude" onclick="SelectExclude(this);" runat="Server" Text="Exclude Selected " Font-Size="Small" />--%>
            </div>
            <dxe:ASPxGridView ID="GvDOSPrintView" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="cGvDOSPrintView" KeyFieldName="UID" OnCustomCallback="GvDOSPrintView_CustomCallback"
                OnHtmlRowCreated="GvDOSPrintView_HtmlRowCreated" OnProcessColumnAutoFilter="GvDOSPrintView_ProcessColumnAutoFilter"
                Width="960px">
                <ClientSideEvents EndCallback="function(s, e) {GvDOSPrintView_EndCallBack();}" />
                <ClientSideEvents SelectionChanged="function(s, e) { OnGridFocusedRowChanged(); }" />
                <Columns>
                    <dxe:GridViewCommandColumn HeaderStyle-HorizontalAlign="Center"
                        ShowSelectCheckbox="True" VisibleIndxe="0" Width="40px">
                        <HeaderTemplate>
                            <dxe:ASPxCheckBox ID="cbAll" runat="server" BackColor="White" ClientInstanceName="cbAll"
                                OnInit="cbAll_Init"
                                ToolTip="Select/Deselect all rows">
                                <ClientSideEvents CheckedChanged="OnAllCheckedChanged"></ClientSideEvents>
                            </dxe:ASPxCheckBox>
                            <%-- --%>
                        </HeaderTemplate>
                    </dxe:GridViewCommandColumn>
                    <dex:gridviewdatatextcolumn caption="SerialNo" fieldname="SerialNo" visibleindex="0" width="42px">
                            <CellStyle  CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dex:gridviewdatatextcolumn>
                    <dex:gridviewdatacolumn caption="Client Name" headerstyle-horizontalalign="Center" fieldname="ClientName"
                        visibleindex="2" width="155px">
                            <CellStyle  CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dex:gridviewdatacolumn>
                    <dex:gridviewdatacolumn caption="ContractNoteNo" fieldname="contractnoteno"
                        visibleindex="3" width="90px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dex:gridviewdatacolumn>
                    <dex:gridviewdatacolumn caption="Segment" fieldname="ExchangeSegment" visibleindex="4" width="80px">
                            <CellStyle Wrap="False">
                            </CellStyle>
                        </dex:gridviewdatacolumn>
                    <dex:gridviewdatacolumn caption="TCode" fieldname="CurrentSeg_TCode" visibleindex="5" width="95px">
                            <CellStyle Wrap="False">
                            </CellStyle>
                        </dex:gridviewdatacolumn>
                    <dex:gridviewdatacolumn caption="SettlmntNoType" fieldname="Settno" visibleindex="6"
                        width="100px">
                            <CellStyle Wrap="False">
                            </CellStyle>
                        </dex:gridviewdatacolumn>
                    <dex:gridviewdatacolumn caption="Branch Name" headerstyle-horizontalalign="Center" fieldname="BranchName" visibleindex="7"
                        width="200px">
                            <CellStyle Wrap="False">
                            </CellStyle>
                        </dex:gridviewdatacolumn>
                </Columns>
                <SettingsBehavior AllowFocusedRow="True" AllowGroup="false" AllowSort="false" />
                <Settings GridLines="Both" ShowGroupPanel="True" ShowHorizontalScrollBar="True" />
                <Styles>
                    <FocusedRow BackColor="#FCA977" HorizontalAlign="Left" VerticalAlign="Top">
                    </FocusedRow>
                    <AlternatingRow Enabled="True">
                    </AlternatingRow>
                </Styles>
                <SettingsPager AlwaysShowPager="False" Mode="ShowAllRecords"
                    ShowSeparators="True">
                </SettingsPager>
            </dxe:ASPxGridView>
            <div class="left" style="padding-top: 10px">
                <dxe:ASPxButton ID="btnDosPrnt2" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                    Height="5px" Text="Print" ToolTip="Print Selected" Width="101px" ClientInstanceName="cbtnDosPrnt2">
                    <ClientSideEvents Click="function (s, e) {fn_DosPrnt();}" />
                </dxe:ASPxButton>
            </div>
        </div>
        <div id="Row1" class="Row">
            <div id="Container3" class="container">
                <div id="C3_Row0" style="margin-left: 10px;">
                    <table class="gridcellleft" cellpadding="0" cellspacing="0" border="1" style="padding: 2px;">
                        <tr style="background-color: lavender; text-align: left; font-size: 12px;">
                            <td colspan="5">
                                <b>ECN Related Detail </b>
                            </td>
                        </tr>
                        <tr style="background-color: #DBEEF3; font-size: 12px;">
                            <td>
                                <b>Total Client</b>
                            </td>
                            <td>
                                <b>ECN ENABLE</b>
                            </td>
                            <td>
                                <b>DELIVERED</b>
                            </td>
                        </tr>
                        <tr style="background-color: #eee;">
                            <td>
                                <b style="text-align: right; margin-right: 5px" id="B_allcontract" runat="server"></b>
                                <a href="javascript:ShowHideFilter('v1');"><span style="color: Blue; text-decoration: underline; font-size: 11px; margin-right: 2px;"
                                    id="spantotal">View Detail</span></a>
                            </td>
                            <td>
                                <b style="text-align: right; margin-right: 5px" id="B_ecnenable" runat="server"></b>
                                <a href="javascript:ShowHideFilter('v2');"><span style="color: Blue; text-decoration: underline; font-size: 11px; margin-right: 2px;"
                                    id="spanonlyimport">View Detail</span></a>
                            </td>
                            <td>
                                <b style="text-align: right; margin-right: 5px" id="B_deliveryrpt" runat="server"></b>
                                <a href="javascript:ShowHideFilter('v3');"><span style="color: Blue; text-decoration: underline; font-size: 11px; margin-right: 2px;"
                                    id="spannotimport">View Detail</span></a>
                            </td>
                        </tr>
                    </table>
                </div>
                <span class="clear"></span>
                <div id="C3_Row1">
                    <div id="C3_Row1_Col1" class="LFloat_Lable LableWidth">
                        <asp:Label ID="lblSigname" runat="server" Text="Signature : "></asp:Label>
                    </div>
                    <div id="C3_Row1_Col2" class="LFloat_Content ">
                        <asp:TextBox ID="txtdigitalName" runat="server" Width="300px"></asp:TextBox>
                    </div>
                </div>
                <span class="clear"></span>
                <div id="td_msg" runat="server">
                    <asp:Label ID="Location" runat="server" Text="You Dont have Permission to sent ECN/ Contact to Administrator"
                        ForeColor="red" Font-Bold="true"></asp:Label>
                </div>
                <span class="clear"></span>
                <div id="C3_button">
                    <dxe:ASPxButton ID="btnopenpopup" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                        Height="5px" Text="Click to Send" Width="175px">
                        <ClientSideEvents Click="function (s, e) {fn_showpopup();}" />
                    </dxe:ASPxButton>
                </div>
                <div id="C3_GeneratPDF">
                    <dxe:ASPxButton ID="BtnGeneratePdf" ClientInstanceName="cBtnGeneratePdf" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                        Height="5px" Text="Click to Generate" Width="175px">
                        <ClientSideEvents Click="function (s, e) {fn_GenearatePDF();}" />
                    </dxe:ASPxButton>
                </div>
            </div>
            <dxe:ASPxPopupControl ID="PopUp_ScripAlert" runat="server" ClientInstanceName="cPopUp_ScripAlert"
                Width="340px" HeaderText="ECN Detail Information" PopupHorizontalAlign="Center"
                PopupVerticalAlign="Middle" CloseAction="None" Modal="True" ShowCloseButton="False">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <dxe:ASPxCallbackPanel ID="CbpSuggestISIN" runat="server" ClientInstanceName="cCbpSuggestISIN"
                            BackColor="White" OnCallback="CbpSuggestISIN_Callback" LoadingPanelText="Sending....Please Wait !!"
                            LoadingPanelStyle-Font-Bold="true" LoadingPanelStyle-Cursor="wait" LoadingPanelStyle-ForeColor="gray"
                            LoadingPanelImage-Url='../images/Animated_Email.gif'>
                            <ClientSideEvents EndCallback="CbpSuggestISIN_EndCallBack" />
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <div id="div_fail" style="color: Red; font-weight: bold; font-size: 12px;">
                                        An Internal Error Occured during sending some ECNs.Please send Remaing.
                                    </div>
                                    <div id="div_success" style="color: Green; font-weight: bold; font-size: 12px;">
                                        Successfully send ECNs.
                                    </div>
                                    <%--<asp:Label runat="server" ID="div_success" Text="efgh"></asp:Label>--%>
                                    <br />
                                    <br />
                                    <div style="font-weight: bold; color: black; background-color: gainsboro; border-right: silver thin solid; border-top: silver thin solid; border-left: silver thin solid; border-bottom: silver thin solid;">
                                        No of ECN Sent : <b style="text-align: right" id="B_allcontractpop" runat="server"></b>
                                        <br />
                                        <%--<asp:Image src='../Documents/Animated_Email.gif' runat="server" />--%>
                                        <br />
                                        Remaining ECN (To Be Sent) : <b style="text-align: right" id="B_ecnenablepop" runat="server"></b>
                                    </div>
                                    <br />
                                    <br />
                                    <div class="frmleftCont" id="btnall">
                                        <dxe:ASPxButton ID="btnsendall" runat="server" AutoPostBack="False" Text="Send All">
                                            <ClientSideEvents Click="function (s, e) {btnsendall_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div class="frmleftCont" id="btnremain">
                                        <dxe:ASPxButton ID="btnsendremaining" runat="server" AutoPostBack="False" Text="Send Remaining">
                                            <ClientSideEvents Click="function (s, e) {btnsendremaining_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div class="frmleftCont" id="btnokdiv">
                                        <dxe:ASPxButton ID="btnok" runat="server" AutoPostBack="False" Text="Close">
                                            <ClientSideEvents Click="function (s, e) {btnok_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div class="frmleftCont" id="cancel">
                                        <dxe:ASPxButton ID="btncancel" runat="server" AutoPostBack="False" Text="Cancel">
                                            <ClientSideEvents Click="function (s, e) {btncancel_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </dxe:PanelContent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>

        </div>
        <div style="display: none">
            <asp:TextBox ID="txtEmpName_hidden" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtdigitalName_hidden" runat="server" Width="300px"></asp:TextBox>
            <asp:TextBox ID="txtSelectionID_hidden" runat="server"></asp:TextBox>
            <asp:HiddenField ID="HiddenField_ClientBranchGroup" runat="server" />
            <asp:Button ID="btnPdfExport" runat="server" OnClick="btnPdfExport_Click" />
            <asp:Button ID="btndosprint" runat="server" OnClick="btndosprint_Click" />
            <%--<asp:Button ID="btnecn" runat="server" OnClick="btnecn_Click" />--%>
            <asp:HiddenField ID="hdnpath" runat="server" />
            <asp:HiddenField ID="hdnLocationPath" runat="server" />
            <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged"
                BackColor="#DDECFE" BorderStyle="None" />
            <asp:Button ID="BtnForExportEvent1" runat="server" OnClick="cmbExport1_SelectedIndexChanged"
                BackColor="#DDECFE" BorderStyle="None" />
            <asp:Button ID="BtnForExportEvent2" runat="server" OnClick="cmbExport2_SelectedIndexChanged"
                BackColor="#DDECFE" BorderStyle="None" />
            <dxe:ASPxCallbackPanel ID="CbpExportPanel" runat="server" ClientInstanceName="cCbpExportPanel"
                OnCallback="CbpExportPanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="cCbpExportPanel_EndCallBack" />
            </dxe:ASPxCallbackPanel>
        </div>
    </div>
</asp:Content>
